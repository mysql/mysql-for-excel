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

namespace MySQL.ForExcel
{
  using System;
  using System.ComponentModel;
  using System.Data;
  using System.Drawing;
  using System.Linq;
  using System.Text;
  using System.Windows.Forms;
  using MySql.Data.MySqlClient;
  using MySQL.ForExcel.Properties;
  using MySQL.Utility;
  using Excel = Microsoft.Office.Interop.Excel;

  /// <summary>
  /// Presents users with a wizard-like form to export selected Excel data to a new MySQL table.
  /// </summary>
  public partial class ExportDataForm : AutoStyleableBaseDialog
  {
    #region Fields

    /// <summary>
    /// Flag indicating whether when text changes on an input control was due user input or programatic.
    /// </summary>
    private bool _isUserInput;

    #endregion Fields

    /// <summary>
    /// Initializes a new instance of the <see cref="ExportDataForm"/> class.
    /// </summary>
    /// <param name="wbConnection">Connection to a MySQL server instance selected by users.</param>
    /// <param name="exportDataRange">Excel cells range containing the data being exported to a new MySQL table.</param>
    /// <param name="exportingWorksheetName">Name of the Excel worksheet containing the data to export.</param>
    public ExportDataForm(MySqlWorkbenchConnection wbConnection, Excel.Range exportDataRange, string exportingWorksheetName)
    {
      _isUserInput = true;
      WBConnection = wbConnection;
      ExportDataRange = exportDataRange;
      string proposedTableName = string.Empty;

      InitializeComponent();

      if (!exportingWorksheetName.ToLowerInvariant().StartsWith("sheet"))
      {
        proposedTableName = exportingWorksheetName.ToLower().Replace(' ', '_');
      }

      Text = string.Format("Export Data - {0} [{1}]", exportingWorksheetName, exportDataRange.Address.Replace("$", string.Empty));
      LoadPreviewData(wbConnection.Schema, proposedTableName);
      InitializeDataTypeCombo();
      CopySQLButton.Visible = Properties.Settings.Default.ExportShowCopySQLButton;
      FirstRowHeadersCheckBox_CheckedChanged(FirstRowHeadersCheckBox, EventArgs.Empty);
      SetDefaultPrimaryKey();

      if (!string.IsNullOrEmpty(proposedTableName))
      {
        SetControlTextValue(TableNameInputTextBox, proposedTableName);
      }
      PreviewTableWarningsChanged(PreviewDataTable, new TableWarningsChangedArgs(PreviewDataTable, false));

      TableNameInputTextBox.Focus();
      TableNameInputTextBox.SelectAll();
    }

    #region Properties

    /// <summary>
    /// Gets the Excel cells range containing the data being exported to a new MySQL table.
    /// </summary>
    public Excel.Range ExportDataRange { get; private set; }

    /// <summary>
    /// Gets a <see cref="MySQLDataTable"/> object containing the all data to be exported to a new MySQL table.
    /// </summary>
    public MySQLDataTable ExportDataTable { get; private set; }

    /// <summary>
    /// Gets a <see cref="MySQLDataTable"/> object containing a subset of the whole data which is shown in the preview grid.
    /// </summary>
    public MySQLDataTable PreviewDataTable { get; private set; }

    /// <summary>
    /// Gets the connection to a MySQL server instance selected by users.
    /// </summary>
    public MySqlWorkbenchConnection WBConnection { get; private set; }

    #endregion Properties

    /// <summary>
    /// Event delegate method fired when the <see cref="AddPrimaryKeyRadioButton"/> radio button checked state changes.
    /// </summary>
    /// <param name="sender">Sender object.</param>
    /// <param name="e">Event arguments.</param>
    private void AddPrimaryKeyRadioButton_CheckedChanged(object sender, EventArgs e)
    {
      if (!AddPrimaryKeyRadioButton.Checked)
      {
        return;
      }

      PreviewDataGrid.Columns[0].Visible = true;
      PreviewDataGrid.Columns[0].Selected = true;
      PreviewDataGrid.FirstDisplayedScrollingColumnIndex = 0;
      PrimaryKeyColumnsComboBox.Text = string.Empty;
      PrimaryKeyColumnsComboBox.SelectedIndex = -1;
      PrimaryKeyColumnsComboBox.Enabled = false;
      AddPrimaryKeyTextBox.Enabled = true;
      PreviewDataTable.UseFirstColumnAsPK = true;
    }

    /// <summary>
    /// Event delegate method fired when the <see cref="AddPrimaryKeyTextBox"/> textbox's text changes.
    /// </summary>
    /// <param name="sender">Sender object.</param>
    /// <param name="e">Event arguments.</param>
    private void AddPrimaryKeyTextBox_TextChanged(object sender, EventArgs e)
    {
      ResetTextChangedTimer();
    }

    /// <summary>
    /// Event delegate method fired when the <see cref="AddPrimaryKeyTextBox"/> textbox is being validated.
    /// </summary>
    /// <param name="sender">Sender object.</param>
    /// <param name="e">Event arguments.</param>
    private void AddPrimaryKeyTextBox_Validating(object sender, CancelEventArgs e)
    {
      TextChangedTimer.Stop();
      string newAutoPKName = AddPrimaryKeyTextBox.Text.Trim();
      MySQLDataColumn pkColumn = PreviewDataTable.GetColumnAtIndex(0);
      if (pkColumn.DisplayName == newAutoPKName && PreviewDataGrid.Columns[0].HeaderText == newAutoPKName)
      {
        return;
      }

      pkColumn.SetDisplayName(AddPrimaryKeyTextBox.Text);
      PreviewDataGrid.Columns[0].HeaderText = pkColumn.DisplayName;
      MySQLDataColumn currentColumn = GetCurrentMySQLDataColumn();
      if (currentColumn != null && currentColumn.Ordinal == 0)
      {
        SetControlTextValue(ColumnNameTextBox, currentColumn.DisplayName);
      }
    }

    /// <summary>
    /// Event delegate method fired when the <see cref="AdvancedOptionsButton"/> button is clicked.
    /// </summary>
    /// <param name="sender">Sender object.</param>
    /// <param name="e">Event arguments.</param>
    private void AdvancedOptionsButton_Click(object sender, EventArgs e)
    {
      ExportAdvancedOptionsDialog optionsDialog = new ExportAdvancedOptionsDialog();
      DialogResult dr = optionsDialog.ShowDialog();
      ////if (dr == DialogResult.OK)
      ////  btnCopySQL.Visible = Settings.Default.ExportShowCopySQLButton;
    }

    /// <summary>
    /// Event delegate method fired when the <see cref="AllowEmptyCheckBox"/> object's checked state changes.
    /// </summary>
    /// <param name="sender">Sender object.</param>
    /// <param name="ea">Event arguments.</param>
    private void AllowEmptyCheckBox_CheckedChanged(object sender, EventArgs ea)
    {
      MySQLDataColumn currentCol = GetCurrentMySQLDataColumn();
      if (currentCol == null || AllowEmptyCheckBox.Checked == currentCol.AllowNull)
      {
        return;
      }

      currentCol.AllowNull = AllowEmptyCheckBox.Checked;
      RefreshColumnControlsEnabledStatus(false);
    }

    /// <summary>
    /// Event delegate method fired when the <see cref="ColumnNameTextBox"/> textbox's text changes.
    /// </summary>
    /// <param name="sender">Sender object.</param>
    /// <param name="e">Event arguments.</param>
    private void ColumnNameTextBox_TextChanged(object sender, EventArgs e)
    {
      ResetTextChangedTimer();
    }

    /// <summary>
    /// Event delegate method fired when the <see cref="ColumnNameTextBox"/> textbox is being validated.
    /// </summary>
    /// <param name="sender">Sender object.</param>
    /// <param name="e">Event arguments.</param>
    private void ColumnNameTextBox_Validating(object sender, CancelEventArgs e)
    {
      TextChangedTimer.Stop();
      string newColumnName = ColumnNameTextBox.Text.Trim();
      MySQLDataColumn column = GetCurrentMySQLDataColumn();
      if (column == null || column.DisplayName == newColumnName)
      {
        return;
      }

      column.SetDisplayName(newColumnName, true);
      PreviewDataGrid.Columns[column.Ordinal].HeaderText = column.DisplayName;
      SetControlTextValue(AddPrimaryKeyTextBox, column.DisplayName);
      if (ColumnNameTextBox.Text != column.DisplayName)
      {
        SetControlTextValue(ColumnNameTextBox, column.DisplayName);
      }

      if (PrimaryKeyColumnsComboBox.Items.Count > 0)
      {
        //// Update the columnIndex for the cmbPrimaryKeyColumns combo box since it does not include Excluded columns
        int comboColumnIndex = -1;
        for (int i = 1; i < PreviewDataTable.Columns.Count && i != column.Ordinal; i++)
        {
          column = PreviewDataTable.GetColumnAtIndex(i);
          if (!column.ExcludeColumn)
          {
            comboColumnIndex++;
          }
        }

        if (comboColumnIndex >= 0)
        {
          PrimaryKeyColumnsComboBox.BeginUpdate();
          PrimaryKeyColumnsComboBox.Items[comboColumnIndex] = column.DisplayName;
          PrimaryKeyColumnsComboBox.EndUpdate();
        }
      }
    }

    /// <summary>
    /// Event delegate method fired when the <see cref="CopySQLButton"/> button is clicked.
    /// </summary>
    /// <param name="sender">Sender object.</param>
    /// <param name="e">Event arguments.</param>
    private void CopySQLButton_Click(object sender, EventArgs e)
    {
      StringBuilder queryString = new StringBuilder();
      queryString.Append(ExportDataTable.GetCreateSQL(true));
      queryString.AppendFormat(";{0}", Environment.NewLine);
      queryString.Append(ExportDataTable.GetInsertSQL(100, true));
      Clipboard.SetText(queryString.ToString());
    }

    /// <summary>
    /// Event delegate method fired when the <see cref="CreateIndexCheckBox"/> object's checked state changes.
    /// </summary>
    /// <param name="sender">Sender object.</param>
    /// <param name="ea">Event arguments.</param>
    private void CreateIndexCheckBox_CheckedChanged(object sender, EventArgs ea)
    {
      if (!_isUserInput)
      {
        return;
      }

      MySQLDataColumn currentCol = GetCurrentMySQLDataColumn();
      if (currentCol == null || CreateIndexCheckBox.Checked == currentCol.CreateIndex)
      {
        return;
      }

      currentCol.CreateIndex = CreateIndexCheckBox.Checked;
      RefreshColumnControlsEnabledStatus(false);
    }

    /// <summary>
    /// Event delegate method fired when the <see cref="DataTypeComboBox"/> combo box's selected index changes.
    /// </summary>
    /// <param name="sender">Sender object.</param>
    /// <param name="e">Event arguments.</param>
    private void DataTypeComboBox_SelectedIndexChanged(object sender, EventArgs e)
    {
      MySQLDataColumn currentCol = GetCurrentMySQLDataColumn();
      if (currentCol == null || DataTypeComboBox.Text.Length == 0 || DataTypeComboBox.Text == currentCol.MySQLDataType || (DataTypeComboBox.DataSource as DataTable).Select(string.Format("Value = '{0}'", DataTypeComboBox.Text)).Length == 0)
      {
        return;
      }

      currentCol.SetMySQLDataType(DataTypeComboBox.Text, false, true);
    }

    /// <summary>
    /// Event delegate method fired when the <see cref="DataTypeComboBox"/> combo's text changes.
    /// </summary>
    /// <param name="sender">Sender object.</param>
    /// <param name="e">Event arguments.</param>
    private void DataTypeComboBox_TextChanged(object sender, EventArgs e)
    {
      ResetTextChangedTimer();
    }

    /// <summary>
    /// Event delegate method fired when the <see cref="DataTypeComboBox"/> combo box is validating.
    /// </summary>
    /// <param name="sender">Sender object.</param>
    /// <param name="e">Event arguments.</param>
    private void DataTypeComboBox_Validating(object sender, CancelEventArgs e)
    {
      TextChangedTimer.Stop();
      string newDataType = DataTypeComboBox.Text.Trim();
      MySQLDataColumn currentCol = GetCurrentMySQLDataColumn();
      if (currentCol == null || DataTypeComboBox.SelectedIndex >= 0 || currentCol.MySQLDataType == newDataType)
      {
        return;
      }

      currentCol.SetMySQLDataType(newDataType, true, true);
    }

    /// <summary>
    /// Event delegate method fired when the <see cref="DataTypeComboBox"/> combo box's draws each internal item.
    /// </summary>
    /// <param name="sender">Sender object.</param>
    /// <param name="e">Event arguments.</param>
    private void DataTypeComboBoxDrawItem(object sender, DrawItemEventArgs e)
    {
      e.DrawBackground();
      e.Graphics.DrawString((DataTypeComboBox.Items[e.Index] as DataRowView)["Description"].ToString(), DataTypeComboBox.Font, System.Drawing.Brushes.Black, new RectangleF(e.Bounds.X, e.Bounds.Y, e.Bounds.Width, e.Bounds.Height));
      e.DrawFocusRectangle();
    }

    /// <summary>
    /// Event delegate method fired when the <see cref="ExcludeColumnCheckBox"/> object's checked state changes.
    /// </summary>
    /// <param name="sender">Sender object.</param>
    /// <param name="ea">Event arguments.</param>
    private void ExcludeCheckBox_CheckedChanged(object sender, EventArgs ea)
    {
      if (!_isUserInput)
      {
        return;
      }

      MySQLDataColumn currentCol = GetCurrentMySQLDataColumn();
      if (currentCol == null || ExcludeColumnCheckBox.Checked == currentCol.ExcludeColumn)
      {
        return;
      }

      currentCol.ExcludeColumn = ExcludeColumnCheckBox.Checked;
      RefreshColumnControlsEnabledStatus(true);
      RefreshPrimaryKeyColumnsCombo(false);
    }

    /// <summary>
    /// Event delegate method fired when the <see cref="ExportButton"/> button is clicked.
    /// </summary>
    /// <param name="sender">Sender object.</param>
    /// <param name="e">Event arguments.</param>
    private void ExportButton_Click(object sender, EventArgs e)
    {
      this.Cursor = Cursors.WaitCursor;
      if (ExportDataTable == null)
      {
        ExportDataTable = PreviewDataTable.CloneSchema();
        ExportDataTable.DetectDatatype = false;
        ExportDataTable.SetData(ExportDataRange, false, true);
      }
      else
      {
        ExportDataTable.SyncSchema(PreviewDataTable);
      }

      ExportDataTable.TableName = PreviewDataTable.TableName;
      this.Cursor = Cursors.Default;

      bool tableContainsDataToExport = ExportDataTable.Rows.Count > (ExportDataTable.FirstRowIsHeaders ? 1 : 0);
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
      string operationSummary = string.Format("The MySQL Table \"{0}\"", ExportDataTable.TableName);
      StringBuilder operationDetails = new StringBuilder();
      operationDetails.AppendFormat("Creating MySQL Table \"{0}\" with query...{1}{1}", ExportDataTable.TableName, Environment.NewLine);
      string queryString = string.Empty;
      warningsTable = ExportDataTable.CreateTable(out exception, out queryString);
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
        warningsTable = ExportDataTable.InsertDataWithManualQuery(out exception, out queryString, out insertingCount, out insertedCount);
        warningsCount = (warningsTable != null ? warningsTable.Rows.Count : 0) + (insertingCount > insertedCount ? 1 : 0);
        operationDetails.AppendFormat(
          "{1}{1}Inserting Excel data in MySQL Table \"{0}\" with query...{1}{1}{2}{1}{1}",
          ExportDataTable.TableName,
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
    /// Event delegate method fired when the <see cref="ExportDataForm"/> form is loaded.
    /// </summary>
    /// <param name="sender">Sender object.</param>
    /// <param name="e">Event arguments.</param>
    private void ExportDataForm_Load(object sender, EventArgs e)
    {
      PreviewDataGrid.Columns[PreviewDataGrid.Columns[0].Visible ? 0 : 1].Selected = true;
    }

    /// <summary>
    /// Event delegate method fired when the <see cref="FirstRowHeadersCheckBox"/> checkbox's checked state changes.
    /// </summary>
    /// <param name="sender">Sender object.</param>
    /// <param name="e">Event arguments.</param>
    private void FirstRowHeadersCheckBox_CheckedChanged(object sender, EventArgs e)
    {
      int grdIndex = PreviewDataGrid.SelectedColumns.Count > 0 ? PreviewDataGrid.SelectedColumns[0].Index : 0;
      PreviewDataTable.FirstRowIsHeaders = FirstRowHeadersCheckBox.Checked;
      RecreateColumns();
      SetControlTextValue(AddPrimaryKeyTextBox, PreviewDataTable.AutoPKName);
      PreviewDataGrid.CurrentCell = null;
      PreviewDataGrid.Rows[0].Visible = !FirstRowHeadersCheckBox.Checked;
      PreviewDataGrid.Columns[grdIndex].Selected = true;
      if (FirstRowHeadersCheckBox.Checked && PreviewDataGrid.Rows.Count < 2)
      {
        return;
      }

      PreviewDataGrid.FirstDisplayedScrollingRowIndex = FirstRowHeadersCheckBox.Checked ? 1 : 0;
    }

    /// <summary>
    /// Reflects in the Primary Key columns combo box if the index is composed of multiple columns or a single one.
    /// </summary>
    private void FlagMultiColumnPrimaryKey()
    {
      int pkQty = PreviewDataTable.NumberOfPK;
      AddPrimaryKeyRadioButton.Checked = pkQty == 0;
      UseExistingColumnRadioButton.Checked = pkQty > 0;
      if (PrimaryKeyColumnsComboBox.Items.Count == 0)
      {
        return;
      }

      if (pkQty < 2 && PrimaryKeyColumnsComboBox.Items[0].ToString() == "<Multiple Items>")
      {
        PrimaryKeyColumnsComboBox.Items.RemoveAt(0);
        var pkColumn = PreviewDataTable.Columns.Cast<MySQLDataColumn>().Skip(1).First(i => i.PrimaryKey == true);
        if (pkColumn != null)
        {
          PrimaryKeyColumnsComboBox.SelectedIndexChanged -= PrimaryKeyColumnsComboBox_SelectedIndexChanged;
          PrimaryKeyColumnsComboBox.SelectedItem = pkColumn.DisplayName;
          PrimaryKeyColumnsComboBox.SelectedIndexChanged += PrimaryKeyColumnsComboBox_SelectedIndexChanged;
        }
      }
      else if (pkQty > 1 && PrimaryKeyColumnsComboBox.Items[0].ToString() != "<Multiple Items>")
      {
        PrimaryKeyColumnsComboBox.Items.Insert(0, "<Multiple Items>");
        PrimaryKeyColumnsComboBox.SelectedIndex = 0;
      }
    }

    /// <summary>
    /// Gets the MySQL Column bound to the currently selected grid column.
    /// </summary>
    /// <returns><see cref="MySQLDataColumn"/> object bound to the currently selected grid column.</returns>
    private MySQLDataColumn GetCurrentMySQLDataColumn()
    {
      MySQLDataColumn currentColumn = null;
      if (PreviewDataGrid.SelectedColumns.Count > 0)
      {
        currentColumn = PreviewDataTable.GetColumnAtIndex(PreviewDataGrid.SelectedColumns[0].Index);
      }

      return currentColumn;
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

      _isUserInput = false;
      DataTypeComboBox.DataSource = dataTypesTable;
      DataTypeComboBox.ValueMember = "Value";
      DataTypeComboBox.DisplayMember = "Value";
      _isUserInput = true;
    }

    /// <summary>
    /// Creates the <see cref="MySQLDataTable"/> preview table and fills it with a subset of all the data to export.
    /// </summary>
    /// <param name="schemaName">Name of the schema where the MySQL table will be created.</param>
    /// <param name="proposedTableName">Name of the new MySQL table that will be created.</param>
    private void LoadPreviewData(string schemaName, string proposedTableName)
    {
      if (this.ExportDataRange == null)
      {
        return;
      }

      PreviewDataTable = new MySQLDataTable(
        schemaName,
        proposedTableName,
        true,
        Settings.Default.ExportUseFormattedValues,
        Settings.Default.ExportRemoveEmptyColumns,
        Settings.Default.ExportDetectDatatype,
        Settings.Default.ExportAddBufferToVarchar,
        Settings.Default.ExportAutoIndexIntColumns,
        Settings.Default.ExportAutoAllowEmptyNonIndexColumns,
        WBConnection);
      PreviewDataTable.TableColumnPropertyValueChanged += PreviewTableColumnPropertyValueChanged;
      PreviewDataTable.TableWarningsChanged += PreviewTableWarningsChanged;
      int previewRowsQty = Math.Min(ExportDataRange.Rows.Count, Settings.Default.ExportLimitPreviewRowsQuantity);
      Excel.Range previewRange = ExportDataRange.get_Resize(previewRowsQty, ExportDataRange.Columns.Count);
      PreviewDataTable.SetData(previewRange, true, true);
      PreviewDataGrid.DataSource = PreviewDataTable;
    }

    /// <summary>
    /// Event delegate method fired when the <see cref="PreviewDataGrid"/> grid cells will display a tooltip.
    /// </summary>
    /// <param name="sender">Sender object.</param>
    /// <param name="e">Event arguments.</param>
    private void PreviewDataGrid_CellToolTipTextNeeded(object sender, DataGridViewCellToolTipTextNeededEventArgs e)
    {
      if (e.RowIndex >= 0)
      {
        e.ToolTipText = Resources.ExportColumnsGridToolTipCaption;
      }
      else
      {
        e.ToolTipText = PreviewDataGrid.Columns[e.ColumnIndex].HeaderText;
      }
    }

    /// <summary>
    /// Event delegate method fired when the <see cref="PreviewDataGrid"/> grid data binding operation completes.
    /// </summary>
    /// <param name="sender">Sender object.</param>
    /// <param name="e">Event arguments.</param>
    private void PreviewDataGrid_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
    {
      if (e.ListChangedType != ListChangedType.Reset)
      {
        return;
      }

      PreviewDataGrid.CurrentCell = null;
      PreviewDataGrid.Rows[0].Visible = !FirstRowHeadersCheckBox.Checked;
    }

    /// <summary>
    /// Event delegate method fired when the <see cref="PreviewDataGrid"/> grid catches that a key is down.
    /// </summary>
    /// <param name="sender">Sender object.</param>
    /// <param name="e">Event arguments.</param>
    private void PreviewDataGrid_KeyDown(object sender, KeyEventArgs e)
    {
      if (PreviewDataGrid.SelectedColumns.Count == 0)
      {
        return;
      }

      if (e.Alt)
      {
        int currentSelectedIdx = PreviewDataGrid.SelectedColumns[0].Index;
        int newIdx = 0;
        switch (e.KeyCode.ToString())
        {
          case "P":
            newIdx = currentSelectedIdx - 1;
            if (newIdx >= (AddPrimaryKeyRadioButton.Checked ? 0 : 1))
            {
              PreviewDataGrid.Columns[newIdx].Selected = true;
              PreviewDataGrid.FirstDisplayedScrollingColumnIndex = newIdx;
            }

            break;

          case "N":
            newIdx = currentSelectedIdx + 1;
            if (newIdx < PreviewDataGrid.Columns.Count)
            {
              PreviewDataGrid.Columns[newIdx].Selected = true;
              PreviewDataGrid.FirstDisplayedScrollingColumnIndex = newIdx;
            }

            break;
        }
      }
    }

    /// <summary>
    /// Event delegate method fired when the <see cref="PreviewDataGrid"/> grid selection changes.
    /// </summary>
    /// <param name="sender">Sender object.</param>
    /// <param name="e">Event arguments.</param>
    private void PreviewDataGrid_SelectionChanged(object sender, EventArgs e)
    {
      RefreshColumnControlsAndWarnings();
    }

    /// <summary>
    /// Event delegate method fired when a property value on any of the columns in the <see cref="_previewDataTable"/> table changes.
    /// </summary>
    /// <param name="sender">A <see cref="MySQLDataColumn"/> object representing the column with a changed property.</param>
    /// <param name="args">Event arguments.</param>
    public void PreviewTableColumnPropertyValueChanged(object sender, PropertyChangedEventArgs args)
    {
      MySQLDataColumn changedColumn = sender as MySQLDataColumn;
      MySQLDataColumn currentColumn = GetCurrentMySQLDataColumn();
      if (changedColumn != currentColumn)
      {
        return;
      }

      _isUserInput = false;
      switch (args.PropertyName)
      {
        case "CreateIndex":
          if (CreateIndexCheckBox.Checked != changedColumn.CreateIndex)
          {
            CreateIndexCheckBox.Checked = changedColumn.CreateIndex;
          }
          break;

        case "ExcludeColumn":
          if (ExcludeColumnCheckBox.Checked != changedColumn.ExcludeColumn)
          {
            ExcludeColumnCheckBox.Checked = changedColumn.ExcludeColumn;
          }
          break;

        case "PrimaryKey":
          if (PrimaryKeyCheckBox.Checked != changedColumn.PrimaryKey)
          {
            PrimaryKeyCheckBox.Checked = changedColumn.PrimaryKey;
          }
          break;

        case "UniqueKey":
          if (UniqueIndexCheckBox.Checked != changedColumn.UniqueKey)
          {
            UniqueIndexCheckBox.Checked = changedColumn.UniqueKey;
          }
          break;
      }

      _isUserInput = true;
    }

    /// <summary>
    /// Event delegate method fired when the warning texts list of any column in the <see cref="_previewDataTable"/> table changes.
    /// </summary>
    /// <param name="sender">Sender object.</param>
    /// <param name="args">Event arguments.</param>
    private void PreviewTableWarningsChanged(object sender, TableWarningsChangedArgs args)
    {
      bool showWarning = false;
      switch (args.WarningsType)
      {
        case TableWarningsChangedArgs.TableWarningsType.AutoPrimaryKeyWarnings:
          ShowValidationWarning("PrimaryKeyWarning", args.WarningsQuantity > 0, Properties.Resources.PrimaryKeyColumnExistsWarning);
          break;

        case TableWarningsChangedArgs.TableWarningsType.ColumnWarnings:
          MySQLDataColumn column = sender as MySQLDataColumn;
          DataGridViewColumn gridCol = PreviewDataGrid.Columns[column.Ordinal];
          showWarning = args.WarningsQuantity > 0;
          ShowValidationWarning("ColumnOptionsWarning", showWarning, args.CurrentWarning);
          gridCol.DefaultCellStyle.BackColor = column.ExcludeColumn ? Color.LightGray : (showWarning ? Color.OrangeRed : PreviewDataGrid.DefaultCellStyle.BackColor);
          break;

        case TableWarningsChangedArgs.TableWarningsType.TableNameWarnings:
          ShowValidationWarning("TableNameWarning", args.WarningsQuantity > 0, args.CurrentWarning);
          break;
      }

      if (args.WarningsType != TableWarningsChangedArgs.TableWarningsType.ColumnWarnings)
      {
        ExportButton.Enabled = PreviewDataTable.IsTableNameValid && PreviewDataTable.IsAutoPKColumnNameValid;
      }
    }

    /// <summary>
    /// Event delegate method fired when the <see cref="PrimaryKeyCheckBox"/> object's checked state changes.
    /// </summary>
    /// <param name="sender">Sender object.</param>
    /// <param name="ea">Event arguments.</param>
    private void PrimaryKeyCheckBox_CheckedChanged(object sender, EventArgs ea)
    {
      if (!_isUserInput)
      {
        return;
      }

      MySQLDataColumn currentCol = GetCurrentMySQLDataColumn();
      if (currentCol == null || PrimaryKeyCheckBox.Checked == currentCol.PrimaryKey)
      {
        return;
      }

      currentCol.PrimaryKey = PrimaryKeyCheckBox.Checked;
      FlagMultiColumnPrimaryKey();
      RefreshColumnControlsEnabledStatus(false);
    }

    /// <summary>
    /// Event delegate method fired when the <see cref="PrimaryKeyColumnsComboBox"/> combo box selected index changes.
    /// </summary>
    /// <param name="sender">Sender object.</param>
    /// <param name="e">Event arguments.</param>
    private void PrimaryKeyColumnsComboBox_SelectedIndexChanged(object sender, EventArgs e)
    {
      if (PrimaryKeyColumnsComboBox.SelectedIndex == -1)
      {
        return;
      }

      if (PreviewDataTable.NumberOfPK > 1 && PrimaryKeyColumnsComboBox.SelectedIndex == 0)
      {
        return;
      }

      //// If <Multiple Items> was previously selected we need to remove it since we are selecting a single column now as a primary key
      if (PrimaryKeyColumnsComboBox.Items[0].ToString() == "<Multiple Items>")
      {
        PrimaryKeyColumnsComboBox.BeginUpdate();
        int index = PrimaryKeyColumnsComboBox.SelectedIndex;
        PrimaryKeyColumnsComboBox.Items.RemoveAt(0);
        if (index == 0)
        {
          PrimaryKeyColumnsComboBox.SelectedIndex = 0;
        }

        PrimaryKeyColumnsComboBox.EndUpdate();
      }

      //// Now we need to adjust the index of the actual column we want to set the PrimaryKey flag for
      int comboColumnIndex = 0;
      MySQLDataColumn currentColumn = GetCurrentMySQLDataColumn();
      for (int coldIdx = 1; coldIdx < PreviewDataTable.Columns.Count; coldIdx++)
      {
        MySQLDataColumn col = PreviewDataTable.GetColumnAtIndex(coldIdx);
        if (col.ExcludeColumn)
        {
          continue;
        }

        col.PrimaryKey = comboColumnIndex == PrimaryKeyColumnsComboBox.SelectedIndex;
        if (col.PrimaryKey)
        {
          col.CreateIndex = col.UniqueKey = col.AllowNull = col.ExcludeColumn = false;
          if (col != currentColumn)
          {
            PreviewDataGrid.Columns[col.ColumnName].Selected = true;
            PreviewDataGrid.FirstDisplayedScrollingColumnIndex = PreviewDataGrid.Columns[col.ColumnName].Index;
          }
        }

        comboColumnIndex++;
      }
    }

    /// <summary>
    /// Refreshes the columns names and data types based on the data having the first row (not used as column names) or not.
    /// </summary>
    private void RecreateColumns()
    {
      for (int colIdx = 0; colIdx < PreviewDataTable.Columns.Count; colIdx++)
      {
        MySQLDataColumn mysqlCol = PreviewDataTable.GetColumnAtIndex(colIdx);
        DataGridViewColumn gridCol = PreviewDataGrid.Columns[colIdx];
        gridCol.HeaderText = mysqlCol.DisplayName;
        PreviewDataGrid.Columns[colIdx].SortMode = DataGridViewColumnSortMode.NotSortable;
      }

      PreviewDataGrid.SelectionMode = DataGridViewSelectionMode.FullColumnSelect;
      RefreshPrimaryKeyColumnsCombo(true);
    }

    /// <summary>
    /// Refreshes the values of controls tied to column properties and its related warning controls.
    /// </summary>
    private void RefreshColumnControlsAndWarnings()
    {
      bool columnSelected = PreviewDataGrid.SelectedColumns.Count > 0;
      ColumnOptionsGroupBox.Enabled = columnSelected;
      if (!columnSelected)
      {
        return;
      }

      //// Set current column
      DataGridViewColumn gridCol = PreviewDataGrid.SelectedColumns[0];
      MySQLDataColumn mysqlCol = PreviewDataTable.GetColumnAtIndex(gridCol.Index);

      //// Set controls tied to column properties
      SetControlTextValue(ColumnNameTextBox, mysqlCol.DisplayName);
      SetControlTextValue(DataTypeComboBox, mysqlCol.MySQLDataType);
      CreateIndexCheckBox.Checked = mysqlCol.CreateIndex;
      UniqueIndexCheckBox.Checked = mysqlCol.UniqueKey;
      PrimaryKeyCheckBox.Checked = mysqlCol.PrimaryKey;
      AllowEmptyCheckBox.Checked = mysqlCol.AllowNull;
      ExcludeColumnCheckBox.Checked = mysqlCol.ExcludeColumn;

      //// Update column warnings
      bool showWarning = !string.IsNullOrEmpty(mysqlCol.CurrentColumnWarningText);
      ShowValidationWarning("ColumnOptionsWarning", showWarning, mysqlCol.CurrentColumnWarningText);

      //// Refresh column controls enabled status and related grid column background color
      RefreshColumnControlsEnabledStatus(true);
    }

    /// <summary>
    /// Enables or disables checkboxes in the form depending on specific rules.
    /// </summary>
    /// <param name="refreshGridColumnBkColor">Flag indicating if the grid column's background color should be refreshed.</param>
    private void RefreshColumnControlsEnabledStatus(bool refreshGridColumnBkColor)
    {
      if (PreviewDataGrid.SelectedColumns.Count == 0)
      {
        return;
      }

      MySQLDataColumn mysqlCol = GetCurrentMySQLDataColumn();
      ExcludeColumnCheckBox.Enabled = true;
      PrimaryKeyCheckBox.Enabled = !(ExcludeColumnCheckBox.Checked || AddPrimaryKeyRadioButton.Checked);
      UniqueIndexCheckBox.Enabled = !ExcludeColumnCheckBox.Checked;
      CreateIndexCheckBox.Enabled = !(ExcludeColumnCheckBox.Checked || UniqueIndexCheckBox.Checked || PrimaryKeyCheckBox.Checked);
      AllowEmptyCheckBox.Enabled = !(ExcludeColumnCheckBox.Checked || PrimaryKeyCheckBox.Checked);
      UseExistingColumnRadioButton.Enabled = !PreviewDataTable.Columns.Cast<MySQLDataColumn>().Skip(1).All(i => i.ExcludeColumn);
      PrimaryKeyColumnsComboBox.Enabled = UseExistingColumnRadioButton.Enabled && UseExistingColumnRadioButton.Checked;
      DataTypeComboBox.Enabled = !mysqlCol.AutoPK;

      if (mysqlCol.Ordinal == 0)
      {
        DataTypeComboBox.Enabled = UniqueIndexCheckBox.Enabled = CreateIndexCheckBox.Enabled = ExcludeColumnCheckBox.Enabled = AllowEmptyCheckBox.Enabled = PrimaryKeyCheckBox.Enabled = false;
      }

      if (refreshGridColumnBkColor)
      {
        DataGridViewColumn gridCol = PreviewDataGrid.SelectedColumns[0];
        gridCol.DefaultCellStyle.BackColor = mysqlCol.ExcludeColumn ? Color.LightGray : (mysqlCol.WarningsQuantity > 0 ? Color.OrangeRed : PreviewDataGrid.DefaultCellStyle.BackColor);
      }
    }

    /// <summary>
    /// Fills the <see cref="PrimaryKeyColumnsComboBox"/> combo box containing the names of column names to choose from to create a Primary Key.
    /// </summary>
    /// <param name="recreatingColumnNames">Flag indicating if the primarky key columns combobox is being refreshed due a recreation of all column names.</param>
    private void RefreshPrimaryKeyColumnsCombo(bool recreatingColumnNames)
    {
      int selectedIndex = -1;
      string selectedItem = null;
      if (UseExistingColumnRadioButton.Checked)
      {
        selectedIndex = PrimaryKeyColumnsComboBox.SelectedIndex;
        selectedItem = (string)PrimaryKeyColumnsComboBox.SelectedItem;
      }

      PrimaryKeyColumnsComboBox.BeginUpdate();
      PrimaryKeyColumnsComboBox.Items.Clear();
      if (selectedItem == "<Multiple Items>" && PreviewDataTable.NumberOfPK > 1)
      {
        PrimaryKeyColumnsComboBox.Items.Add("<Multiple Items>");
      }

      foreach (MySQLDataColumn mysqlCol in PreviewDataTable.Columns)
      {
        if (mysqlCol.Ordinal == 0 || mysqlCol.ExcludeColumn)
        {
          continue;
        }

        PrimaryKeyColumnsComboBox.Items.Add(mysqlCol.DisplayName);
      }

      PrimaryKeyColumnsComboBox.SelectedIndexChanged -= PrimaryKeyColumnsComboBox_SelectedIndexChanged;
      if (recreatingColumnNames)
      {
        //// All columns are being recreated, so the amounts of non-excluded columns has not changed, we need to select the same index.
        PrimaryKeyColumnsComboBox.SelectedIndex = selectedIndex;
      }
      else
      {
        //// A column is being excluded and it may have had its PrimaryKey property value set to true. We will try to set the saved SelectedItem
        //// value back, if it is not assigned it means the excluded column was a Primary Key and we need to reset the combo selected value.
        PrimaryKeyColumnsComboBox.SelectedItem = selectedItem;
        if (PrimaryKeyColumnsComboBox.SelectedItem == null)
        {
          int pkQty = PreviewDataTable.NumberOfPK;
          if (pkQty > 0)
          {
            var pkColumn = PreviewDataTable.Columns.Cast<MySQLDataColumn>().Skip(1).First(i => i.PrimaryKey == true);
            if (pkColumn != null)
            {
              PrimaryKeyColumnsComboBox.SelectedItem = pkColumn.DisplayName;
            }
          }
          else
          {
            AddPrimaryKeyRadioButton.Checked = pkQty == 0;
            UseExistingColumnRadioButton.Checked = pkQty > 0;
          }
        }
      }

      PrimaryKeyColumnsComboBox.SelectedIndexChanged += PrimaryKeyColumnsComboBox_SelectedIndexChanged;
      PrimaryKeyColumnsComboBox.EndUpdate();
    }

    /// <summary>
    /// Resets the timer used on text changes only if there was a user input.
    /// </summary>
    private void ResetTextChangedTimer()
    {
      if (!_isUserInput)
      {
        return;
      }

      TextChangedTimer.Stop();
      TextChangedTimer.Start();
    }

    /// <summary>
    /// Sets the text property value of the given control.
    /// </summary>
    /// <param name="control">Any object inheriting from <see cref="Control"/>.</param>
    /// <param name="textValue">Text to assign to the control's Text property.</param>
    private void SetControlTextValue(Control control, string textValue)
    {
      if (control.Text == textValue)
      {
        return;
      }

      _isUserInput = false;
      control.Text = textValue;
      _isUserInput = true;
    }

    /// <summary>
    /// Sets the default primary key column based on the data type of the first column.
    /// </summary>
    private void SetDefaultPrimaryKey()
    {
      SetControlTextValue(AddPrimaryKeyTextBox, PreviewDataTable.AutoPKName);
      if (PreviewDataTable.FirstColumnContainsIntegers)
      {
        UseExistingColumnRadioButton.Checked = true;
        PrimaryKeyColumnsComboBox.SelectedIndex = 0;
        PreviewDataGrid.Columns[1].Selected = true;
      }
      else
      {
        AddPrimaryKeyRadioButton.Checked = true;
      }
    }

    /// <summary>
    /// Shows or hides the visual controls to display warnings for columns or table name.
    /// </summary>
    /// <param name="warningControlPrefix">Prefix of the warning control names.</param>
    /// <param name="show">true to show the warnings, false to hide them.</param>
    /// <param name="text">Warning text to display.</param>
    private void ShowValidationWarning(string warningControlPrefix, bool show, string text)
    {
      show = show && !string.IsNullOrEmpty(text);
      string pictureBoxControlName = warningControlPrefix + "Picture";
      string labelControlName = warningControlPrefix + "Label";

      if (contentAreaPanel.Controls.ContainsKey(pictureBoxControlName) && contentAreaPanel.Controls.ContainsKey(labelControlName))
      {
        contentAreaPanel.Controls[pictureBoxControlName].Visible = show;
        contentAreaPanel.Controls[labelControlName].Text = string.IsNullOrEmpty(text) ? string.Empty : text;
        contentAreaPanel.Controls[labelControlName].Visible = show;
        return;
      }

      if (ColumnOptionsGroupBox.Controls.ContainsKey(pictureBoxControlName) && ColumnOptionsGroupBox.Controls.ContainsKey(labelControlName))
      {
        ColumnOptionsGroupBox.Controls[pictureBoxControlName].Visible = show;
        ColumnOptionsGroupBox.Controls[labelControlName].Text = string.IsNullOrEmpty(text) ? string.Empty : text;
        ColumnOptionsGroupBox.Controls[labelControlName].Visible = show;
        return;
      }
    }

    /// <summary>
    /// Event delegate method fired when the <see cref="TableNameInputTextBox"/> textbox's text changes.
    /// </summary>
    /// <param name="sender">Sender object.</param>
    /// <param name="e">Event arguments.</param>
    private void TableNameInputTextBox_TextChanged(object sender, EventArgs e)
    {
      ResetTextChangedTimer();
    }

    /// <summary>
    /// Event delegate method fired when the <see cref="TableNameInputTextBox"/> textbox is being validated.
    /// </summary>
    /// <param name="sender">Sender object.</param>
    /// <param name="e">Event arguments.</param>
    private void TableNameInputTextBox_Validating(object sender, CancelEventArgs e)
    {
      TextChangedTimer.Stop();
      string newTableName = TableNameInputTextBox.Text.Trim();
      if (PreviewDataTable == null || PreviewDataTable.TableName == newTableName)
      {
        return;
      }

      PreviewDataTable.TableName = newTableName;
      SetControlTextValue(AddPrimaryKeyTextBox, PreviewDataTable.AutoPKName);
      AddPrimaryKeyTextBox_Validating(AddPrimaryKeyTextBox, new CancelEventArgs());
    }

    /// <summary>
    /// Event delegate method fired when the <see cref="TextChangedTimer"/> timer's elapses.
    /// </summary>
    /// <param name="sender">Sender object.</param>
    /// <param name="e">Event arguments.</param>
    private void TextChangedTimerTick(object sender, EventArgs e)
    {
      if (TableNameInputTextBox.Focused)
      {
        TableNameInputTextBox_Validating(TableNameInputTextBox, new CancelEventArgs());
      }
      else if (AddPrimaryKeyTextBox.Focused)
      {
        AddPrimaryKeyTextBox_Validating(AddPrimaryKeyTextBox, new CancelEventArgs());
      }
      else if (ColumnNameTextBox.Focused)
      {
        ColumnNameTextBox_Validating(ColumnNameTextBox, new CancelEventArgs());
      }
      else if (DataTypeComboBox.Focused)
      {
        DataTypeComboBox_Validating(DataTypeComboBox, new CancelEventArgs());
      }
      else
      {
        //// The code should never hit this block in which case there is something wrong.
        MiscUtilities.WriteToLog("TextChangedTimer's Tick event fired but no valid control had focus.");
        TextChangedTimer.Stop();
      }
    }

    /// <summary>
    /// Event delegate method fired when the <see cref="UniqueIndexCheckBox"/> object's checked state changes.
    /// </summary>
    /// <param name="sender">Sender object.</param>
    /// <param name="ea">Event arguments.</param>
    private void UniqueIndexCheckBox_CheckedChanged(object sender, EventArgs ea)
    {
      if (!_isUserInput)
      {
        return;
      }

      MySQLDataColumn currentCol = GetCurrentMySQLDataColumn();
      if (currentCol == null || UniqueIndexCheckBox.Checked == currentCol.UniqueKey)
      {
        return;
      }

      currentCol.UniqueKey = UniqueIndexCheckBox.Checked;
      RefreshColumnControlsEnabledStatus(false);
    }

    /// <summary>
    /// Event delegate method fired when the <see cref="UseExistingColumnRadioButton"/> radio button checked state changes.
    /// </summary>
    /// <param name="sender">Sender object.</param>
    /// <param name="e">Event arguments.</param>
    private void UseExistingColumnRadioButton_CheckedChanged(object sender, EventArgs e)
    {
      if (!UseExistingColumnRadioButton.Checked)
      {
        return;
      }

      PreviewDataGrid.Columns[0].Visible = false;
      PreviewDataGrid.FirstDisplayedScrollingColumnIndex = 1;
      PrimaryKeyColumnsComboBox.Enabled = true;
      PrimaryKeyColumnsComboBox.SelectedIndex = 0;
      AddPrimaryKeyTextBox.Enabled = false;
      PreviewDataTable.UseFirstColumnAsPK = false;
      PreviewDataGrid.Columns[1].Selected = true;
    }
  }
}