// 
// Copyright (c) 2012-2013, Oracle and/or its affiliates. All rights reserved.
//
// This program is free software; you can redistribute it and/or
// modify it under the terms of the GNU General Public License as
// published by the Free Software Foundation; version 2 of the
// License.
// 
// This program is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the
// GNU General Public License for more details.
// 
// You should have received a copy of the GNU General Public License
// along with this program; if not, write to the Free Software
// Foundation, Inc., 51 Franklin St, Fifth Floor, Boston, MA
// 02110-1301  USA
//

namespace MySQL.ForExcel
{
  using System;
  using System.Collections.Generic;
  using System.ComponentModel;
  using System.Data;
  using System.Drawing;
  using System.Linq;
  using System.Text;
  using System.Windows.Forms;
  using Excel = Microsoft.Office.Interop.Excel;
  using MySql.Data.MySqlClient;
  using MySQL.ForExcel.Properties;
  using MySQL.Utility;

  /// <summary>
  /// Presents users with a wizard-like form to export selected Excel data to a new MySQL table.
  /// </summary>
  public partial class ExportDataForm : AutoStyleableBaseDialog
  {
    #region Fields

    /// <summary>
    /// <see cref="MySQLDataTable"/> object containing a subset of the whole data which is shown in the preview grid.
    /// </summary>
    private MySQLDataTable _previewDataTable;

    /// <summary>
    /// <see cref="MySQLDataTable"/> object containing the all data to be exported to a new MySQL table.
    /// </summary>
    private MySQLDataTable _exportDataTable;

    /// <summary>
    /// Connection to a MySQL server instance selected by users.
    /// </summary>
    private MySqlWorkbenchConnection _wbConnection;

    /// <summary>
    /// Flag indicating if the primary column is composed of more than 1 column.
    /// </summary>
    private bool _multiColumnPK;

    /// <summary>
    /// Flag indicating if any of the column or primary key properties are being changed.
    /// </summary>
    private bool _isChanging;

    /// <summary>
    /// Excel cells range containing the data being exported to a new MySQL table.
    /// </summary>
    private Excel.Range _exportDataRange;

    /// <summary>
    /// Flag indicating if the table name is valid so it would not throw errors when the MySQL table is created.
    /// </summary>
    private bool _isTableNameValid = false;

    /// <summary>
    /// Flag indicating if the column name for the automatically added Primary Key does not already exist in another column name.
    /// </summary>
    private bool _isColumnPKValid = true;

    #endregion Fields

    /// <summary>
    /// Initializes a new instance of the <see cref="ExportDataForm"/> class.
    /// </summary>
    /// <param name="wbConnection">Connection to a MySQL server instance selected by users.</param>
    /// <param name="exportDataRange">Excel cells range containing the data being exported to a new MySQL table.</param>
    /// <param name="exportingWorksheetName">Name of the Excel worksheet containing the data to export.</param>
    public ExportDataForm(MySqlWorkbenchConnection wbConnection, Excel.Range exportDataRange, string exportingWorksheetName)
    {
      _multiColumnPK = false;
      _isChanging = false;
      _wbConnection = wbConnection;
      _exportDataRange = exportDataRange;
      string proposedTableName = string.Empty;

      InitializeComponent();

      if (!exportingWorksheetName.ToLowerInvariant().StartsWith("sheet"))
      {
        proposedTableName = exportingWorksheetName.ToLower().Replace(' ', '_');
      }

      Text = string.Format("Export Data - {0} [{1}])", exportingWorksheetName, exportDataRange.Address.Replace("$", string.Empty));
      LoadPreviewData(wbConnection.Schema, proposedTableName);
      InitializeDataTypeCombo();

      if (!string.IsNullOrEmpty(proposedTableName))
      {
        txtTableNameInput.Text = proposedTableName;
      }

      txtTableNameInput.SelectAll();
      btnCopySQL.Visible = Properties.Settings.Default.ExportShowCopySQLButton;
      chkFirstRowHeaders_CheckedChanged(chkFirstRowHeaders, EventArgs.Empty);
      SetDefaultPrimaryKey();
    }

    /// <summary>
    /// Fills the <see cref="cmbPrimaryKeyColumns"/> combo box containing the names of column names to choose from to create a Primary Key.
    /// </summary>
    private void RefreshPrimaryKeyColumnsCombo()
    {
      string selectedItem = null;
      if (radUseExistingColumn.Checked)
      {
        selectedItem = (string)cmbPrimaryKeyColumns.SelectedItem;
      }

      cmbPrimaryKeyColumns.BeginUpdate();
      cmbPrimaryKeyColumns.Items.Clear();
      if (selectedItem == "<Multiple Items>")
      {
        cmbPrimaryKeyColumns.Items.Add("<Multiple Items>");
      }

      foreach (MySQLDataColumn mysqlCol in _previewDataTable.Columns)
      {
        if (mysqlCol.Ordinal == 0 || mysqlCol.ExcludeColumn)
        {
          continue;
        }

        cmbPrimaryKeyColumns.Items.Add(mysqlCol.DisplayName);
      }

      cmbPrimaryKeyColumns.SelectedItem = selectedItem;
      cmbPrimaryKeyColumns.EndUpdate();
    }

    /// <summary>
    /// Creates the <see cref="MySQLDataTable"/> preview table and fills it with a subset of all the data to export.
    /// </summary>
    /// <param name="schemaName">Name of the schema where the MySQL table will be created.</param>
    /// <param name="proposedTableName">Name of the new MySQL table that will be created.</param>
    private void LoadPreviewData(string schemaName, string proposedTableName)
    {
      if (this._exportDataRange == null)
      {
        return;
      }

      _previewDataTable = new MySQLDataTable(schemaName, proposedTableName, true, Settings.Default.ExportUseFormattedValues, Settings.Default.ExportRemoveEmptyColumns);
      int previewRowsQty = Math.Min(this._exportDataRange.Rows.Count, Settings.Default.ExportLimitPreviewRowsQuantity);
      Excel.Range previewRange = this._exportDataRange.get_Resize(previewRowsQty, this._exportDataRange.Columns.Count);
      _previewDataTable.SetData(
        previewRange,
        true,
        Settings.Default.ExportDetectDatatype,
        Settings.Default.ExportAddBufferToVarchar,
        Settings.Default.ExportAutoIndexIntColumns,
        Settings.Default.ExportAutoAllowEmptyNonIndexColumns,
        true);
      grdPreviewData.DataSource = _previewDataTable;
      columnBindingSource.DataSource = _previewDataTable.Columns;
    }

    /// <summary>
    /// Adds or removes warnings for a specific table column and updates the visual warning controls with the corresponding message.
    /// </summary>
    /// <param name="mysqlCol"><see cref="MySQLDataColumn"/> object representing the column to update.</param>
    /// <param name="showWarning">true to add a new warning to the column's warnings collection, false to remove the given warning and display another existing warning.</param>
    /// <param name="warningResourceText">Text to display to users about the specific warning related to a given table column.</param>
    private void UpdateColumnWarning(MySQLDataColumn mysqlCol, bool showWarning, string warningResourceText)
    {
      int columnIndex = mysqlCol.Ordinal;
      DataGridViewColumn gridCol = grdPreviewData.Columns[columnIndex];
      string currentWarningText = showWarning ? warningResourceText : null;
      if (!showWarning)
      {
        //// We do not want to show a warning or we want to remove a warning if warningResourceText != null
        if (!string.IsNullOrEmpty(warningResourceText))
        {
          //// Remove the warning and check if there is an stored warning, if so we want to pull it and show it
          mysqlCol.WarningTextList.Remove(warningResourceText);
          if (mysqlCol.WarningTextList.Count > 0)
          {
            showWarning = true;
            currentWarningText = mysqlCol.WarningTextList.Last();
          }
        }
      }
      else
      {
        //// We want to show a warning, the last one stored if any, if not nothing will be shown
        if (string.IsNullOrEmpty(warningResourceText))
        {
          currentWarningText = mysqlCol.WarningTextList.Count > 0 ? mysqlCol.WarningTextList.Last() : null;
          showWarning = !string.IsNullOrEmpty(currentWarningText);
        }
        else if (!mysqlCol.WarningTextList.Contains(warningResourceText))
        {
          mysqlCol.WarningTextList.Add(warningResourceText);
        }
      }

      showWarning = showWarning && !chkExcludeColumn.Checked;
      currentWarningText = showWarning ? currentWarningText : null;
      ShowValidationWarning("ColumnOptionsWarning", showWarning, currentWarningText);
      gridCol.DefaultCellStyle.BackColor = mysqlCol.ExcludeColumn ? Color.LightGray : (showWarning ? Color.OrangeRed : grdPreviewData.DefaultCellStyle.BackColor);
    }

    /// <summary>
    /// Updates the warnings related to the given <see cref="MySQLDataColumn"/> column's data type.
    /// </summary>
    /// <param name="mysqlCol"><see cref="MySQLDataColumn"/> object representing the column to update.</param>
    private void RefreshColumnDataTypeWarning(MySQLDataColumn mysqlCol)
    {
      bool showWarning = mysqlCol.MySQLDataType.Length == 0;
      UpdateColumnWarning(mysqlCol, showWarning, Resources.ColumnDataTypeRequiredWarning);
    }

    /// <summary>
    /// Updates the warnings related to the given <see cref="MySQLDataColumn"/> column's name.
    /// </summary>
    /// <param name="mysqlCol"><see cref="MySQLDataColumn"/> object representing the column to update.</param>
    private void RefreshColumnNameWarning(MySQLDataColumn mysqlCol)
    {
      bool showWarning = mysqlCol.DisplayName.Length == 0;
      UpdateColumnWarning(mysqlCol, showWarning, Resources.ColumnNameRequiredWarning);
    }

    /// <summary>
    /// Refreshes the columns names and data types based on the data having the first row (not used as column names) or not.
    /// </summary>
    private void RecreateColumns()
    {
      for (int colIdx = 0; colIdx < _previewDataTable.Columns.Count; colIdx++)
      {
        MySQLDataColumn mysqlCol = _previewDataTable.GetColumnAtIndex(colIdx);
        DataGridViewColumn gridCol = grdPreviewData.Columns[colIdx];
        gridCol.HeaderText = mysqlCol.DisplayName;
        grdPreviewData.Columns[colIdx].SortMode = DataGridViewColumnSortMode.NotSortable;

        //// Check if current DataType is empty, and if so add a warning for column
        RefreshColumnDataTypeWarning(mysqlCol);

        //// Check if Name is empty, and if so add a warning for column
        RefreshColumnNameWarning(mysqlCol);
      }

      grdPreviewData.SelectionMode = DataGridViewSelectionMode.FullColumnSelect;
      RefreshPrimaryKeyColumnsCombo();
    }

    /// <summary>
    /// Sets the default primary key column based on the data type of the first column.
    /// </summary>
    private void SetDefaultPrimaryKey()
    {
      txtAddPrimaryKey.DataBindings.Add(new Binding("Text", _previewDataTable.Columns[0], "DisplayName"));
      if (_previewDataTable.FirstColumnContainsIntegers)
      {
        radUseExistingColumn.Checked = true;
        columnBindingSource.Position = 1;
        cmbPrimaryKeyColumns.SelectedIndex = 0;
        grdPreviewData.Columns[1].Selected = true;
      }
      else
      {
        radAddPrimaryKey.Checked = true;
      }
    }

    /// <summary>
    /// Fills the data type combo with the valid values for the columns data type.
    /// </summary>
    private void InitializeDataTypeCombo()
    {
      DataTable dataTypesTable = new DataTable();
      dataTypesTable.Columns.Add("Value");
      dataTypesTable.Columns.Add("Description");

      dataTypesTable.Rows.Add(new string[] { "Integer", "Integer - Default for whole-number columns" });
      dataTypesTable.Rows.Add(new string[] { "Varchar(5)", "Varchar(5) - Small string up to 5 characters" });
      dataTypesTable.Rows.Add(new string[] { "Varchar(12)", "Varchar(12) - Small string up to 12 characters" });
      dataTypesTable.Rows.Add(new string[] { "Varchar(25)", "Varchar(25) - Small string up to 25 characters" });
      dataTypesTable.Rows.Add(new string[] { "Varchar(45)", "Varchar(45) - Standard string up to 45 characters" });
      dataTypesTable.Rows.Add(new string[] { "Varchar(255)", "Varchar(255) - Standard string up to 255 characters" });
      dataTypesTable.Rows.Add(new string[] { "Varchar(4000)", "Varchar(4000) - Large string up to 4k characters" });
      dataTypesTable.Rows.Add(new string[] { "Varchar(65535)", "Varchar(65535) - Maximum string up to 65k characters" });
      dataTypesTable.Rows.Add(new string[] { "Datetime", "Datetime - For columns that store both, date and time" });
      dataTypesTable.Rows.Add(new string[] { "Date", "Date - For columns that only store a date" });
      dataTypesTable.Rows.Add(new string[] { "Time", "Time - For columns that only store a time" });
      dataTypesTable.Rows.Add(new string[] { "Bool", "Bool - Holds values like (0, 1), (True, False) or (Yes, No)" });
      dataTypesTable.Rows.Add(new string[] { "BigInt", "BigInt - For columns containing large whole-number integers with up to 19 digits" });
      dataTypesTable.Rows.Add(new string[] { "Decimal(12, 2)", "Decimal(12, 2) - Exact decimal numbers with 12 digits with 2 of them after decimal point" });
      dataTypesTable.Rows.Add(new string[] { "Decimal(65, 30)", "Decimal(65, 30) - Biggest exact decimal numbers with 65 digits with 30 of them after decimal point" });
      dataTypesTable.Rows.Add(new string[] { "Double", "Double - Biggest float pointing number with approximately 15 decimal places" });

      cmbDatatype.DataSource = dataTypesTable;
      cmbDatatype.ValueMember = "Value";
      cmbDatatype.DisplayMember = "Value";
    }

    /// <summary>
    /// Shows or hides the visual controls to display warnings for columns or table name.
    /// </summary>
    /// <param name="warningControlSuffix">Suffix of the warning control names.</param>
    /// <param name="show">true to show the warnings, false to hide them.</param>
    /// <param name="text">Warning text to display.</param>
    private void ShowValidationWarning(string warningControlSuffix, bool show, string text)
    {
      show = show && !string.IsNullOrEmpty(text);
      string picBoxName = string.Format("pic{0}", warningControlSuffix);
      string lblName = string.Format("lbl{0}", warningControlSuffix);

      if (contentAreaPanel.Controls.ContainsKey(picBoxName) && contentAreaPanel.Controls.ContainsKey(lblName))
      {
        contentAreaPanel.Controls[picBoxName].Visible = show;
        contentAreaPanel.Controls[lblName].Text = string.IsNullOrEmpty(text) ? string.Empty : text;
        contentAreaPanel.Controls[lblName].Visible = show;
        return;
      }

      if (grpColumnOptions.Controls.ContainsKey(picBoxName) && grpColumnOptions.Controls.ContainsKey(lblName))
      {
        grpColumnOptions.Controls[picBoxName].Visible = show;
        grpColumnOptions.Controls[lblName].Text = string.IsNullOrEmpty(text) ? string.Empty : text;
        grpColumnOptions.Controls[lblName].Visible = show;
        return;
      }
    }

    /// <summary>
    /// Reflects in the Primary Key columns combo box if the index is composed of multiple columns or a single one.
    /// </summary>
    /// <param name="pkQty">Number of columns composing the Primary Key.</param>
    private void FlagMultiColumnPrimaryKey(int pkQty)
    {
      radAddPrimaryKey.Checked = pkQty == 0;
      radUseExistingColumn.Checked = pkQty > 0;
      if (cmbPrimaryKeyColumns.Items.Count == 0)
      {
        return;
      }

      if (pkQty < 2 && cmbPrimaryKeyColumns.Items[0].ToString() == "<Multiple Items>")
      {
        cmbPrimaryKeyColumns.Items.RemoveAt(0);
        var name = _previewDataTable.Columns.Cast<MySQLDataColumn>().Skip(1).First(i => i.PrimaryKey == true);
        cmbPrimaryKeyColumns.SelectedItem = name.DisplayName;
      }
      else if (pkQty > 1 && cmbPrimaryKeyColumns.Items[0].ToString() != "<Multiple Items>")
      {
        cmbPrimaryKeyColumns.Items.Insert(0, "<Multiple Items>");
        cmbPrimaryKeyColumns.SelectedIndex = 0;
      }
    }

    /// <summary>
    /// Checks if the given <see cref="MySQLDataColumn"/> column's data type is right for the column's current data.
    /// </summary>
    /// <param name="currentCol"><see cref="MySQLDataColumn"/> object representing the column to test.</param>
    /// <returns>true if the column's data fits the data type, false otherwise.</returns>
    private bool TestColumnDataTypeAgainstColumnData(MySQLDataColumn currentCol)
    {
      bool showWarning = cmbDatatype.Text.Length > 0 && !currentCol.CanBeOfMySQLDataType(cmbDatatype.Text);
      UpdateColumnWarning(currentCol, showWarning, Resources.ExportDataTypeNotSuitableWarning);
      return !showWarning;
    }

    /// <summary>
    /// Validates that a user typed data type is a valid MySQL data type and that the given column's data fits into that type.
    /// </summary>
    /// <param name="currentCol"><see cref="MySQLDataColumn"/> object representing the column to validate.</param>
    /// <param name="proposedUserType">Data type selected from the data type combo box or typed in by the user.</param>
    /// <returns>true if the type is valid and data fits into it, false otherwise.</returns>
    private bool ValidateUserDataType(MySQLDataColumn currentCol, string proposedUserType)
    {
      bool isValid = false;

      if (proposedUserType.Length > 0)
      {
        List<int> paramsInParenthesis;
        List<string> dataTypesList = DataTypeUtilities.GetMySQLDataTypes(out paramsInParenthesis);
        int rightParentFound = proposedUserType.IndexOf(")");
        int leftParentFound = proposedUserType.IndexOf("(");
        string pureDataType = string.Empty;
        int typeParametersNum = 0;

        proposedUserType = proposedUserType.Trim().Replace(" ", string.Empty);
        if (rightParentFound >= 0)
        {
          if (leftParentFound < 0 || leftParentFound >= rightParentFound)
          {
            return false;
          }

          typeParametersNum = proposedUserType.Substring(leftParentFound + 1, rightParentFound - leftParentFound - 1).Count(c => c == ',') + 1;
          pureDataType = proposedUserType.Substring(0, leftParentFound).ToLowerInvariant();
        }
        else
        {
          pureDataType = proposedUserType.ToLowerInvariant();
        }

        int typeFoundAt = dataTypesList.IndexOf(pureDataType);
        int numOfValidParams = typeFoundAt >= 0 ? paramsInParenthesis[typeFoundAt] : -1;
        bool numParamsMatch = pureDataType.StartsWith("var") ? numOfValidParams >= 0 && numOfValidParams == typeParametersNum : (numOfValidParams >= 0 && numOfValidParams == typeParametersNum) || (numOfValidParams < 0 && typeParametersNum > 0) || typeParametersNum == 0;
        isValid = typeFoundAt >= 0 && numParamsMatch;
      }
      else
      {
        isValid = true;
      }

      bool showWarning = !isValid;
      UpdateColumnWarning(currentCol, showWarning, Resources.ExportDataTypeNotValidWarning);

      return isValid;
    }

    /// <summary>
    /// Checks or unchecks checkboxes in the form depending on specific rules.
    /// </summary>
    /// <param name="control"><see cref="CheckBox"/> control to apply rules to.</param>
    private void EnableChecks(CheckBox control)
    {
      MySQLDataColumn column = columnBindingSource.Current as MySQLDataColumn;

      if (control == chkExcludeColumn && control.Checked)
      {
        if (chkPrimaryKey.Checked)
        {
          chkPrimaryKey.CheckedChanged -= chkPrimaryKey_CheckedChanged;
          chkPrimaryKey.Checked = false;
          column.PrimaryKey = false;
          chkPrimaryKey_Validated(null, EventArgs.Empty);
          chkPrimaryKey.CheckedChanged += chkPrimaryKey_CheckedChanged;
        }
      }

      if (control == chkPrimaryKey && control.Checked)
      {
        chkCreateIndex.Checked = false;
        chkUniqueIndex.Checked = false;
        chkAllowEmpty.Checked = false;
      }

      if (control == chkUniqueIndex && control.Checked)
      {
        chkCreateIndex.Checked = true;
        chkPrimaryKey.CheckedChanged -= chkPrimaryKey_CheckedChanged;
        chkPrimaryKey.Checked = false;
        column.PrimaryKey = false;
        chkPrimaryKey_Validated(null, EventArgs.Empty);
        chkPrimaryKey.CheckedChanged += chkPrimaryKey_CheckedChanged;
      }

      if (control == chkCreateIndex && !control.Checked)
      {
        if (Settings.Default.ExportAutoAllowEmptyNonIndexColumns)
        {
          chkAllowEmpty.Checked = true;
        }
      }

      //toColumn.ExcludeColumn = chkExcludeColumn.Checked;
      columnBindingSource.EndEdit();

      chkExcludeColumn.Enabled = true;
      chkPrimaryKey.Enabled = !(chkExcludeColumn.Checked || radAddPrimaryKey.Checked);
      chkUniqueIndex.Enabled = !chkExcludeColumn.Checked;
      chkCreateIndex.Enabled = !(chkExcludeColumn.Checked || chkUniqueIndex.Checked || chkPrimaryKey.Checked);
      chkAllowEmpty.Enabled = !(chkExcludeColumn.Checked || chkPrimaryKey.Checked);
      radUseExistingColumn.Enabled = !_previewDataTable.Columns.Cast<MySQLDataColumn>().Skip(1).All(i => i.ExcludeColumn);
      cmbPrimaryKeyColumns.Enabled = radUseExistingColumn.Enabled && radUseExistingColumn.Checked;
      cmbDatatype.Enabled = !column.AutoPK;

      if (columnBindingSource.Position == 0)
      {
        cmbDatatype.Enabled = chkUniqueIndex.Enabled = chkCreateIndex.Enabled = chkExcludeColumn.Enabled = chkAllowEmpty.Enabled = chkPrimaryKey.Enabled = false;
      }
    }

    /// <summary>
    /// Event delegate method fired when the <see cref="btnCopySQL"/> button is clicked.
    /// </summary>
    /// <param name="sender">Sender object.</param>
    /// <param name="e">Event arguments.</param>
    private void btnCopySQL_Click(object sender, EventArgs e)
    {
      StringBuilder queryString = new StringBuilder();
      queryString.Append(_exportDataTable.GetCreateSQL(true));
      queryString.AppendFormat(";{0}", Environment.NewLine);
      queryString.Append(_exportDataTable.GetInsertSQL(100, true));
      Clipboard.SetText(queryString.ToString());
    }

    /// <summary>
    /// Event delegate method fired when the <see cref="btnExport"/> button is clicked.
    /// </summary>
    /// <param name="sender">Sender object.</param>
    /// <param name="e">Event arguments.</param>
    private void btnExport_Click(object sender, EventArgs e)
    {
      this.Cursor = Cursors.WaitCursor;
      if (_exportDataTable == null)
      {
        _exportDataTable = _previewDataTable.CloneSchema();
        _exportDataTable.SetData(this._exportDataRange, false, false, false, false, false, true);
      }
      else
      {
        _exportDataTable.SyncSchema(_previewDataTable);
      }

      _exportDataTable.TableName = _previewDataTable.TableName;
      this.Cursor = Cursors.Default;

      bool tableContainsDataToExport = _exportDataTable.Rows.Count > (_exportDataTable.FirstRowIsHeaders ? 1 : 0);
      if (!tableContainsDataToExport)
      {
        WarningDialog wDiag = new WarningDialog(Properties.Resources.ExportDataNoDataToExportTitleWarning, Properties.Resources.ExportDataNoDataToExportDetailWarning);
        if (wDiag.ShowDialog() == DialogResult.No)
        {
          return;
        }
      }

      this.Cursor = Cursors.WaitCursor;
      Exception exception;
      DataTable warningsTable;
      bool warningsFound = false;
      string operationSummary = string.Format("The MySQL Table \"{0}\"", _exportDataTable.TableName);
      StringBuilder operationDetails = new StringBuilder();
      operationDetails.AppendFormat("Creating MySQL Table \"{0}\" with query...{1}{1}", _exportDataTable.TableName, Environment.NewLine);
      string queryString = string.Empty;
      warningsTable = _exportDataTable.CreateTable(_wbConnection, out exception, out queryString);
      bool success = exception == null;
      operationDetails.Append(queryString);
      operationDetails.AppendFormat("{0}{0}", Environment.NewLine);
      if (success)
      {
        operationDetails.Append("Table has been created");
        if (warningsTable != null && warningsTable.Rows.Count > 0)
        {
          warningsFound = true;
          operationDetails.AppendFormat(" with {0} warnings:", warningsTable.Rows.Count);
          foreach (DataRow warningRow in warningsTable.Rows)
          {
            operationDetails.AppendFormat(
              "{2}Code {0} - {1}",
              warningRow[1].ToString(),
              warningRow[2].ToString(),
              Environment.NewLine);
          }

          operationDetails.Append(Environment.NewLine);
        }
        else
        {
          operationDetails.Append(" successfully.");
        }
      }
      else
      {
        if (exception is MySqlException)
        {
          operationDetails.AppendFormat("MySQL Error {0}:{1}", (exception as MySqlException).Number, Environment.NewLine);
        }
        else
        {
          operationDetails.AppendFormat("ADO.NET Error:{0}", Environment.NewLine);
        }

        operationDetails.Append(exception.Message);
      }

      operationSummary += success ? "has been created " : "could not be created.";
      if (success && tableContainsDataToExport)
      {
        int insertedCount = 0;
        int insertingCount = 0;
        int warningsCount = 0;
        warningsTable = _exportDataTable.InsertDataWithManualQuery(_wbConnection, out exception, out queryString, out insertingCount, out insertedCount);
        warningsCount = (warningsTable != null ? warningsTable.Rows.Count : 0) + (insertingCount > insertedCount ? 1 : 0);
        operationDetails.AppendFormat(
          "{1}{1}Inserting Excel data in MySQL Table \"{0}\" with query...{1}{1}{2}{1}{1}",
          _exportDataTable.TableName,
          Environment.NewLine,
          queryString);
        success = exception == null;
        if (success)
        {
          operationDetails.AppendFormat("{0} rows have been inserted", insertedCount);
          operationSummary += "with data.";
          if (warningsCount > 0)
          {
            warningsFound = true;
            operationDetails.AppendFormat(" with {0} warnings:", warningsCount);
            if (insertingCount > insertedCount)
            {
              operationDetails.AppendFormat(
                "{2}Attempted to insert {0} rows, but only {1} rows were inserted with no further errors. Please check the MySQL Server log for more information.",
                insertingCount,
                insertedCount,
                Environment.NewLine);
            }

            if (warningsTable != null)
            {
              foreach (DataRow warningRow in warningsTable.Rows)
              {
                operationDetails.AppendFormat(
                  "{2}Code {0} - {1}",
                  warningRow[1].ToString(),
                  warningRow[2].ToString(),
                  Environment.NewLine);
              }
            }

            operationDetails.Append(Environment.NewLine);
          }
          else
          {
            operationDetails.Append(" successfully.");
          }
        }
        else
        {
          operationDetails.AppendFormat("Error while inserting rows...{0}{0}", Environment.NewLine);
          if (exception is MySqlException)
          {
            operationDetails.AppendFormat("MySQL Error {0}:{1}", (exception as MySqlException).Number, Environment.NewLine);
          }
          else
          {
            operationDetails.AppendFormat("ADO.NET Error:{0}", Environment.NewLine);
          }

          operationDetails.Append(exception.Message);
          operationSummary += "with no data.";
        }
      }

      this.Cursor = Cursors.Default;

      InfoDialog.InfoType operationsType = success ? (warningsFound ? InfoDialog.InfoType.Warning : InfoDialog.InfoType.Success) : InfoDialog.InfoType.Error;
      InfoDialog infoDialog = new InfoDialog(operationsType, operationSummary, operationDetails.ToString());
      DialogResult dr = infoDialog.ShowDialog();
      if (dr == DialogResult.Cancel)
      {
        return;
      }

      DialogResult = DialogResult.OK;
      Close();
    }

    /// <summary>
    /// Event delegate method fired when the <see cref="btnAdvanced"/> button is clicked.
    /// </summary>
    /// <param name="sender">Sender object.</param>
    /// <param name="e">Event arguments.</param>
    private void btnAdvanced_Click(object sender, EventArgs e)
    {
      ExportAdvancedOptionsDialog optionsDialog = new ExportAdvancedOptionsDialog();
      DialogResult dr = optionsDialog.ShowDialog();
      ////if (dr == DialogResult.OK)
      ////  btnCopySQL.Visible = Settings.Default.ExportShowCopySQLButton;
    }

    /// <summary>
    /// Event delegate method fired when the <see cref="chkFirstRowHeaders"/> checkbox's checked state changes.
    /// </summary>
    /// <param name="sender">Sender object.</param>
    /// <param name="e">Event arguments.</param>
    private void chkFirstRowHeaders_CheckedChanged(object sender, EventArgs e)
    {
      int cmbIndex = cmbPrimaryKeyColumns.SelectedIndex;
      int grdIndex = columnBindingSource.Position;
      _previewDataTable.FirstRowIsHeaders = chkFirstRowHeaders.Checked;
      RecreateColumns();
      grdPreviewData.CurrentCell = null;
      grdPreviewData.Rows[0].Visible = !chkFirstRowHeaders.Checked;
      cmbPrimaryKeyColumns.SelectedIndex = cmbIndex;
      grdPreviewData.Columns[grdIndex].Selected = true;
      grdPreviewData.FirstDisplayedScrollingColumnIndex = grdIndex;
      if (chkFirstRowHeaders.Checked && grdPreviewData.Rows.Count < 2)
      {
        return;
      }

      grdPreviewData.FirstDisplayedScrollingRowIndex = chkFirstRowHeaders.Checked ? 1 : 0;
    }

    /// <summary>
    /// Event delegate method fired when the <see cref="txtTableNameInput"/> textbox is being validated.
    /// </summary>
    /// <param name="sender">Sender object.</param>
    /// <param name="e">Event arguments.</param>
    private void txtTableNameInput_Validating(object sender, CancelEventArgs e)
    {
      timerTextChanged.Stop();

      if (string.IsNullOrWhiteSpace(txtTableNameInput.Text))
      {
        ShowValidationWarning("TableNameWarning", true, Properties.Resources.TableNameRequiredWarning);
        _isTableNameValid = false;
        btnExport.Enabled = false;
        return;
      }

      _previewDataTable.TableName = txtTableNameInput.Text;

      string cleanTableName = txtTableNameInput.Text.ToLowerInvariant().Replace(" ", "_");
      bool tableExistsInSchema = MySQLDataUtilities.TableExistsInSchema(_wbConnection, _wbConnection.Schema, cleanTableName);
      if (tableExistsInSchema)
      {
        ShowValidationWarning("TableNameWarning", true, Properties.Resources.TableNameExistsWarning);
        btnExport.Enabled = false;
        _isTableNameValid = false;
        return;
      }

      if (txtTableNameInput.Text.Contains(" ") || txtTableNameInput.Text.Any(char.IsUpper))
      {
        ShowValidationWarning("TableNameWarning", true, Properties.Resources.NamesWarning);
        btnExport.Enabled = _isColumnPKValid;
        _isTableNameValid = true;
        return;
      }

      ShowValidationWarning("TableNameWarning", false, null);
      _isTableNameValid = true;
      btnExport.Enabled = _isColumnPKValid;
    }

    /// <summary>
    /// Event delegate method fired when the <see cref="txtTableNameInput"/> textbox's text changes.
    /// </summary>
    /// <param name="sender">Sender object.</param>
    /// <param name="e">Event arguments.</param>
    private void txtTableNameInput_TextChanged(object sender, EventArgs e)
    {
      timerTextChanged.Stop();
      string name = txtTableNameInput.Text.Trim();
      if (_previewDataTable != null)
      {
        _previewDataTable.TableName = name;
      }

      string autoPKColumnName = string.Format("{0}{1}id", name, name.Length > 0 ? "_" : string.Empty);
      txtAddPrimaryKey.Text = _previewDataTable.GetNonDuplicateColumnName(autoPKColumnName);
      timerTextChanged.Start();
    }

    /// <summary>
    /// Event delegate method fired when the <see cref="timerTextChanged"/> timer's elapses.
    /// </summary>
    /// <param name="sender">Sender object.</param>
    /// <param name="e">Event arguments.</param>
    private void timerTextChanged_Tick(object sender, EventArgs e)
    {
      txtTableNameInput_Validating(txtTableNameInput, new CancelEventArgs());
    }

    /// <summary>
    /// Event delegate method fired when the <see cref="radAddPrimaryKey"/> radio button checked state changes.
    /// </summary>
    /// <param name="sender">Sender object.</param>
    /// <param name="e">Event arguments.</param>
    private void radAddPrimaryKey_CheckedChanged(object sender, EventArgs e)
    {
      if (!radAddPrimaryKey.Checked)
      {
        return;
      }

      _isChanging = true;
      grdPreviewData.Columns[0].Visible = true;
      grdPreviewData.Columns[0].Selected = true;
      grdPreviewData.FirstDisplayedScrollingColumnIndex = 0;
      cmbPrimaryKeyColumns.Text = string.Empty;
      cmbPrimaryKeyColumns.SelectedIndex = -1;
      cmbPrimaryKeyColumns.Enabled = false;
      txtAddPrimaryKey.Enabled = true;
      _previewDataTable.UseFirstColumnAsPK = true;
      _isChanging = false;
      ////EnableChecks(null);
    }

    /// <summary>
    /// Event delegate method fired when the <see cref="radUseExistingColumn"/> radio button checked state changes.
    /// </summary>
    /// <param name="sender">Sender object.</param>
    /// <param name="e">Event arguments.</param>
    private void radUseExistingColumn_CheckedChanged(object sender, EventArgs e)
    {
      if (!radUseExistingColumn.Checked)
      {
        return;
      }

      _isChanging = true;
      grdPreviewData.Columns[0].Visible = false;
      grdPreviewData.FirstDisplayedScrollingColumnIndex = 1;
      cmbPrimaryKeyColumns.Enabled = true;
      _multiColumnPK = false;
      cmbPrimaryKeyColumns.SelectedIndex = 0;
      columnBindingSource.ResetCurrentItem();
      txtAddPrimaryKey.Enabled = false;
      _previewDataTable.UseFirstColumnAsPK = false;
      EnableChecks(null);
      _isChanging = false;
    }

    /// <summary>
    /// Event delegate method fired when the <see cref="grdPreviewData"/> grid selection changes.
    /// </summary>
    /// <param name="sender">Sender object.</param>
    /// <param name="e">Event arguments.</param>
    private void grdPreviewData_SelectionChanged(object sender, EventArgs e)
    {
      _isChanging = true;
      if (grdPreviewData.SelectedColumns.Count > 0)
      {
        columnBindingSource.Position = grdPreviewData.SelectedColumns[0].Index;
        MySQLDataColumn column = columnBindingSource.Current as MySQLDataColumn;
        UpdateColumnWarning(column, true, null);
      }

      grpColumnOptions.Enabled = grdPreviewData.SelectedColumns.Count > 0;
      EnableChecks(null);
      if (grdPreviewData.Columns[0].Selected)
      {
        chkUniqueIndex.Enabled = chkCreateIndex.Enabled = chkExcludeColumn.Enabled = chkAllowEmpty.Enabled = chkPrimaryKey.Enabled = false;
      }

      _isChanging = false;
    }

    /// <summary>
    /// Event delegate method fired when the <see cref="cmbPrimaryKeyColumns"/> combo box selected index changes.
    /// </summary>
    /// <param name="sender">Sender object.</param>
    /// <param name="e">Event arguments.</param>
    private void cmbPrimaryKeyColumns_SelectedIndexChanged(object sender, EventArgs e)
    {
      if (cmbPrimaryKeyColumns.SelectedIndex == -1)
      {
        return;
      }

      if (_multiColumnPK && cmbPrimaryKeyColumns.SelectedIndex == 0)
      {
        return;
      }

      //// If <Multiple Items> was previously selected we need to remove it since we are selecting a single column now as a primary key
      _multiColumnPK = false;
      if (cmbPrimaryKeyColumns.Items[0].ToString() == "<Multiple Items>")
      {
        cmbPrimaryKeyColumns.BeginUpdate();
        int index = cmbPrimaryKeyColumns.SelectedIndex;
        cmbPrimaryKeyColumns.Items.RemoveAt(0);
        if (index == 0)
        {
          cmbPrimaryKeyColumns.SelectedIndex = 0;
        }

        cmbPrimaryKeyColumns.EndUpdate();
      }

      //// Now we need to adjust the index of the actual column we want to set the PrimaryKey flag for
      int comboColumnIndex = 0;
      for (int coldIdx = 1; coldIdx < _previewDataTable.Columns.Count; coldIdx++)
      {
        MySQLDataColumn col = _previewDataTable.GetColumnAtIndex(coldIdx);
        if (col.ExcludeColumn)
        {
          continue;
        }

        col.PrimaryKey = comboColumnIndex == cmbPrimaryKeyColumns.SelectedIndex;
        if (col.PrimaryKey)
        {
          col.CreateIndex = col.UniqueKey = col.AllowNull = col.ExcludeColumn = false;
          grdPreviewData.Columns[col.ColumnName].Selected = true;
          grdPreviewData.FirstDisplayedScrollingColumnIndex = grdPreviewData.Columns[col.ColumnName].Index;
        }

        comboColumnIndex++;
      }
    }

    /// <summary>
    /// Event delegate method fired when the <see cref="txtColumnName"/> textbox's text changes.
    /// </summary>
    /// <param name="sender">Sender object.</param>
    /// <param name="e">Event arguments.</param>
    private void txtColumnName_TextChanged(object sender, EventArgs e)
    {
      if (txtColumnName.Text == (columnBindingSource.Current as MySQLDataColumn).DisplayName)
      {
        return;
      }

      _isChanging = true;
      string name = txtColumnName.Text;
      bool colNameEmpty = name.Length == 0;

      int columnIndex = grdPreviewData.SelectedColumns[0].Index;
      if (!colNameEmpty)
      {
        name = _previewDataTable.GetNonDuplicateColumnName(name);
        if (txtColumnName.Text != name)
        {
          txtColumnName.Text = name;
        }
      }

      MySQLDataColumn column = columnBindingSource.Current as MySQLDataColumn;
      column.DisplayName = name;
      grdPreviewData.Columns[columnIndex].HeaderText = name;
      UpdateColumnWarning(column, colNameEmpty, Resources.ColumnNameRequiredWarning);

      if (cmbPrimaryKeyColumns.Items.Count > 0)
      {
        //// Update the columnIndex for the cmbPrimaryKeyColumns combo box since it does not include Excluded columns
        int comboColumnIndex = -1;
        for (int i = 1; i < _previewDataTable.Columns.Count; i++)
        {
          column = _previewDataTable.GetColumnAtIndex(i);
          if (!column.ExcludeColumn)
          {
            comboColumnIndex++;
          }

          if (i == columnIndex)
          {
            break;
          }
        }

        if (comboColumnIndex >= 0)
        {
          cmbPrimaryKeyColumns.BeginUpdate();
          cmbPrimaryKeyColumns.Items[comboColumnIndex] = name;
          cmbPrimaryKeyColumns.EndUpdate();
        }
      }

      _isChanging = false;
    }

    /// <summary>
    /// Event delegate method fired when the <see cref="chkUniqueIndex"/> checkbox checked state changes.
    /// </summary>
    /// <param name="sender">Sender object.</param>
    /// <param name="e">Event arguments.</param>
    private void chkUniqueIndex_CheckedChanged(object sender, EventArgs e)
    {
      MySQLDataColumn currentCol = columnBindingSource.Current as MySQLDataColumn;
      if (chkUniqueIndex.Checked == currentCol.UniqueKey)
      {
        return;
      }

      currentCol.UniqueKey = chkUniqueIndex.Checked;
      DataGridViewColumn gridCol = grdPreviewData.SelectedColumns[0];
      MySQLDataColumn column = _previewDataTable.GetColumnAtIndex(gridCol.Index);
      bool good = true;
      try
      {
        column.Unique = chkUniqueIndex.Checked;
      }
      catch (InvalidConstraintException)
      {
        good = false;
      }

      UpdateColumnWarning(column, !good, Resources.ColumnDataNotUniqueWarning);
      EnableChecks(chkUniqueIndex);
    }

    /// <summary>
    /// Event delegate method fired when the <see cref="chkExcludeColumn"/> checkbox checked state changes.
    /// </summary>
    /// <param name="sender">Sender object.</param>
    /// <param name="e">Event arguments.</param>
    private void chkExcludeColumn_CheckedChanged(object sender, EventArgs e)
    {
      if (chkExcludeColumn.Checked == (columnBindingSource.Current as MySQLDataColumn).ExcludeColumn)
      {
        return;
      }

      MySQLDataColumn column = columnBindingSource.Current as MySQLDataColumn;
      column.ExcludeColumn = chkExcludeColumn.Checked;
      DataGridViewColumn gridCol = grdPreviewData.SelectedColumns[0];
      UpdateColumnWarning(column, !column.ExcludeColumn, null);
      int grdIndex = grdPreviewData.SelectedColumns[0].Index;
      EnableChecks(chkExcludeColumn);
      RefreshPrimaryKeyColumnsCombo();
      grdPreviewData.Columns[grdIndex].Selected = true;
    }

    /// <summary>
    /// Event delegate method fired when the <see cref="chkPrimaryKey"/> checkbox checked state changes.
    /// </summary>
    /// <param name="sender">Sender object.</param>
    /// <param name="e">Event arguments.</param>
    private void chkPrimaryKey_CheckedChanged(object sender, EventArgs e)
    {
      if (chkPrimaryKey.Checked == (columnBindingSource.Current as MySQLDataColumn).PrimaryKey)
      {
        return;
      }

      (columnBindingSource.Current as MySQLDataColumn).PrimaryKey = chkPrimaryKey.Checked;
      EnableChecks(chkPrimaryKey);
      chkPrimaryKey_Validated(sender, e);
    }

    /// <summary>
    /// Event delegate method fired when the <see cref="chkCreateIndex"/> checkbox checked state changes.
    /// </summary>
    /// <param name="sender">Sender object.</param>
    /// <param name="e">Event arguments.</param>
    private void chkCreateIndex_CheckedChanged(object sender, EventArgs e)
    {
      if (chkCreateIndex.Checked == (columnBindingSource.Current as MySQLDataColumn).CreateIndex)
      {
        return;
      }

      EnableChecks(chkCreateIndex);
    }

    /// <summary>
    /// Event delegate method fired when the <see cref="grdPreviewData"/> grid data binding operation completes.
    /// </summary>
    /// <param name="sender">Sender object.</param>
    /// <param name="e">Event arguments.</param>
    private void grdPreviewData_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
    {
      if (e.ListChangedType != ListChangedType.Reset)
      {
        return;
      }

      grdPreviewData.CurrentCell = null;
      grdPreviewData.Rows[0].Visible = !chkFirstRowHeaders.Checked;
    }

    /// <summary>
    /// Event delegate method fired when the <see cref="txtAddPrimaryKey"/> textbox's text changes.
    /// </summary>
    /// <param name="sender">Sender object.</param>
    /// <param name="e">Event arguments.</param>
    private void txtAddPrimaryKey_TextChanged(object sender, EventArgs e)
    {
      bool showWarning = false;
      string warningText = null;
      for (int colIdx = 1; colIdx < _previewDataTable.Columns.Count; colIdx++)
      {
        MySQLDataColumn col = _previewDataTable.GetColumnAtIndex(colIdx);
        showWarning = showWarning || col.DisplayName.ToLowerInvariant() == txtAddPrimaryKey.Text.ToLowerInvariant();
        if (showWarning)
        {
          warningText = Resources.PrimaryKeyColumnExistsWarning;
          break;
        }
      }

      _isColumnPKValid = !showWarning;
      btnExport.Enabled = _isColumnPKValid && _isTableNameValid;
      ShowValidationWarning("PrimaryKeyWarning", showWarning, Properties.Resources.PrimaryKeyColumnExistsWarning);
      _previewDataTable.GetColumnAtIndex(0).DisplayName = txtAddPrimaryKey.Text;
      grdPreviewData.Columns[0].HeaderText = txtAddPrimaryKey.Text;
      if (columnBindingSource.Position == 0)
      {
        columnBindingSource.ResetCurrentItem();
      }
    }

    /// <summary>
    /// Event delegate method fired when the <see cref="txtColumnName"/> textbox completes validations.
    /// </summary>
    /// <param name="sender">Sender object.</param>
    /// <param name="e">Event arguments.</param>
    private void txtColumnName_Validated(object sender, EventArgs e)
    {
      _isChanging = true;
      if (txtColumnName.Text != (columnBindingSource.Current as MySQLDataColumn).DisplayName)
      {
        columnBindingSource.ResetCurrentItem();
        int index = grdPreviewData.SelectedColumns.Count > 0 ? grdPreviewData.SelectedColumns[0].Index : -1;
        if (index > 0)
        {
          cmbPrimaryKeyColumns.Items[index - 1] = txtColumnName.Text;
          grdPreviewData.SelectedColumns[0].HeaderText = txtColumnName.Text;
        }
      }

      _isChanging = false;
    }

    /// <summary>
    /// Event delegate method fired when the <see cref="chkPrimaryKey"/> checkbox completes validations.
    /// </summary>
    /// <param name="sender">Sender object.</param>
    /// <param name="e">Event arguments.</param>
    private void chkPrimaryKey_Validated(object sender, EventArgs e)
    {
      if (!_isChanging)
      {
        int currentPKQty = _previewDataTable.NumberOfPK;
        _multiColumnPK = currentPKQty > 1;
        FlagMultiColumnPrimaryKey(currentPKQty);
      }
    }

    /// <summary>
    /// Event delegate method fired when the <see cref="grdPreviewData"/> grid catches that a key is down.
    /// </summary>
    /// <param name="sender">Sender object.</param>
    /// <param name="e">Event arguments.</param>
    private void grdPreviewData_KeyDown(object sender, KeyEventArgs e)
    {
      if (grdPreviewData.SelectedColumns.Count == 0)
      {
        return;
      }

      if (e.Alt)
      {
        int currentSelectedIdx = grdPreviewData.SelectedColumns[0].Index;
        int newIdx = 0;
        switch (e.KeyCode.ToString())
        {
          case "P":
            newIdx = currentSelectedIdx - 1;
            if (newIdx >= (radAddPrimaryKey.Checked ? 0 : 1))
            {
              grdPreviewData.Columns[newIdx].Selected = true;
              grdPreviewData.FirstDisplayedScrollingColumnIndex = newIdx;
            }

            break;
          case "N":
            newIdx = currentSelectedIdx + 1;
            if (newIdx < grdPreviewData.Columns.Count)
            {
              grdPreviewData.Columns[newIdx].Selected = true;
              grdPreviewData.FirstDisplayedScrollingColumnIndex = newIdx;
            }

            break;
        }
      }
    }

    /// <summary>
    /// Event delegate method fired when the <see cref="cmbDatatype"/> combo box's selected index changes.
    /// </summary>
    /// <param name="sender">Sender object.</param>
    /// <param name="e">Event arguments.</param>
    private void cmbDatatype_SelectedIndexChanged(object sender, EventArgs e)
    {
      MySQLDataColumn currentCol = columnBindingSource.Current as MySQLDataColumn;
      if (cmbDatatype.Text == currentCol.MySQLDataType || cmbDatatype.Text.Length == 0 || (cmbDatatype.DataSource as DataTable).Select(string.Format("Value = '{0}'", cmbDatatype.Text)).Length == 0)
      {
        return;
      }

      currentCol.MySQLDataType = cmbDatatype.Text;
      RefreshColumnDataTypeWarning(currentCol);
      TestColumnDataTypeAgainstColumnData(currentCol);
      if (Settings.Default.ExportAutoIndexIntColumns && cmbDatatype.Text.StartsWith("Integer") && !chkCreateIndex.Checked)
      {
        chkCreateIndex.Checked = true;
      }
    }

    /// <summary>
    /// Event delegate method fired when the <see cref="cmbDatatype"/> combo box's draws each internal item.
    /// </summary>
    /// <param name="sender">Sender object.</param>
    /// <param name="e">Event arguments.</param>
    private void cmbDatatype_DrawItem(object sender, DrawItemEventArgs e)
    {
      e.DrawBackground();
      e.Graphics.DrawString((cmbDatatype.Items[e.Index] as DataRowView)["Description"].ToString(), cmbDatatype.Font, System.Drawing.Brushes.Black, new RectangleF(e.Bounds.X, e.Bounds.Y, e.Bounds.Width, e.Bounds.Height));
      e.DrawFocusRectangle();
    }

    /// <summary>
    /// Event delegate method fired when the <see cref="grdPreviewData"/> grid cells will display a tooltip.
    /// </summary>
    /// <param name="sender">Sender object.</param>
    /// <param name="e">Event arguments.</param>
    private void grdPreviewData_CellToolTipTextNeeded(object sender, DataGridViewCellToolTipTextNeededEventArgs e)
    {
      if (e.RowIndex >= 0)
      {
        e.ToolTipText = Resources.ExportColumnsGridToolTipCaption;
      }
      else
      {
        e.ToolTipText = grdPreviewData.Columns[e.ColumnIndex].HeaderText;
      }
    }

    /// <summary>
    /// Event delegate method fired when the <see cref="cmbDatatype"/> combo box is validating.
    /// </summary>
    /// <param name="sender">Sender object.</param>
    /// <param name="e">Event arguments.</param>
    private void cmbDatatype_Validating(object sender, CancelEventArgs e)
    {
      if (cmbDatatype.SelectedIndex >= 0)
      {
        return;
      }

      MySQLDataColumn currentCol = columnBindingSource.Current as MySQLDataColumn;
      bool valid = ValidateUserDataType(currentCol, cmbDatatype.Text);
      if (valid)
      {
        TestColumnDataTypeAgainstColumnData(currentCol);
      }

      if (Settings.Default.ExportAutoIndexIntColumns && cmbDatatype.Text.StartsWith("Integer") && !chkCreateIndex.Checked)
      {
        chkCreateIndex.Checked = true;
      }
    }

    /// <summary>
    /// Event delegate method fired when the <see cref="cmbDatatype"/> combo box completed validations.
    /// </summary>
    /// <param name="sender">Sender object.</param>
    /// <param name="e">Event arguments.</param>
    private void cmbDatatype_Validated(object sender, EventArgs e)
    {
      MySQLDataColumn currentCol = columnBindingSource.Current as MySQLDataColumn;
      if (!Properties.Settings.Default.ExportDetectDatatype)
      {
        currentCol.RowsFrom1stDataType = currentCol.MySQLDataType;
        currentCol.RowsFrom2ndDataType = currentCol.MySQLDataType;
      }

      RefreshColumnDataTypeWarning(currentCol);
    }

    /// <summary>
    /// Event delegate method fired when the <see cref="ExportDataForm"/> form is loaded.
    /// </summary>
    /// <param name="sender">Sender object.</param>
    /// <param name="e">Event arguments.</param>
    private void ExportDataForm_Load(object sender, EventArgs e)
    {
      grdPreviewData.Columns[grdPreviewData.Columns[0].Visible ? 0 : 1].Selected = true;
    }
  }
}
