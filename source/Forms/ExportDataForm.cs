// 
// Copyright (c) 2012, Oracle and/or its affiliates. All rights reserved.
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

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using MySQL.Utility;
using Excel = Microsoft.Office.Interop.Excel;
using MySql.Data.MySqlClient;
using MySQL.ForExcel.Properties;

namespace MySQL.ForExcel
{
  public partial class ExportDataForm : AutoStyleableBaseDialog
  {
    private MySQLDataTable previewDataTable;
    private MySQLDataTable exportDataTable;
    private MySqlWorkbenchConnection wbConnection;
    private bool multiColumnPK = false;
    private bool isChanging = false;
    private Excel.Range exportDataRange;
    private bool isTableNameValid = false;
    private bool isColumnPKValid = true;

    public ExportDataForm(MySqlWorkbenchConnection wbConnection, Excel.Range exportDataRange, string exportingWorksheetName)
    {
      string proposedTableName = String.Empty;
      this.wbConnection = wbConnection;
      this.exportDataRange = exportDataRange;

      InitializeComponent();

      if (!exportingWorksheetName.ToLowerInvariant().StartsWith("sheet"))
        proposedTableName = exportingWorksheetName.ToLower().Replace(' ', '_');
      Text = String.Format("Export Data - {0} [{1}])", exportingWorksheetName, exportDataRange.Address.Replace("$", String.Empty));

      LoadPreviewData(wbConnection.Schema, proposedTableName);
      RecreateColumns();
      initializeDataTypeCombo();

      if (!String.IsNullOrEmpty(proposedTableName))
        txtTableNameInput.Text = proposedTableName;
      txtTableNameInput.SelectAll();
      btnCopySQL.Visible = Properties.Settings.Default.ExportShowCopySQLButton;
      chkFirstRowHeaders_CheckedChanged(chkFirstRowHeaders, EventArgs.Empty);
      SetDefaultPrimaryKey();
    }

    private void refreshPrimaryKeyColumnsCombo()
    {
      string selectedItem = null;
      if (radUseExistingColumn.Checked)
        selectedItem = (string)cmbPrimaryKeyColumns.SelectedItem;
      cmbPrimaryKeyColumns.BeginUpdate();
      cmbPrimaryKeyColumns.Text = string.Empty;
      cmbPrimaryKeyColumns.Items.Clear();
      if (selectedItem == "<Multiple Items>")
        cmbPrimaryKeyColumns.Items.Add("<Multiple Items>");
      foreach (MySQLDataColumn mysqlCol in previewDataTable.Columns.OfType<MySQLDataColumn>().Skip(1))
      {
        if (mysqlCol.ExcludeColumn)
          continue;
        cmbPrimaryKeyColumns.Items.Add(mysqlCol.DisplayName);
      }
      cmbPrimaryKeyColumns.SelectedItem = selectedItem;
      cmbPrimaryKeyColumns.EndUpdate();
    }

    private void LoadPreviewData(string schemaName, string proposedTableName)
    {
      if (this.exportDataRange == null)
        return;
      previewDataTable = new MySQLDataTable(schemaName, proposedTableName, true, Settings.Default.ExportUseFormattedValues);
      int previewRowsQty = Math.Min(this.exportDataRange.Rows.Count, Settings.Default.ExportLimitPreviewRowsQuantity);
      Excel.Range previewRange = this.exportDataRange.get_Resize(previewRowsQty, this.exportDataRange.Columns.Count);
      previewDataTable.SetData(previewRange,
                               true,
                               Settings.Default.ExportDetectDatatype,
                               Settings.Default.ExportAddBufferToVarchar,
                               Settings.Default.ExportAutoIndexIntColumns,
                               Settings.Default.ExportAutoAllowEmptyNonIndexColumns,
                               true);
      grdPreviewData.DataSource = previewDataTable;
      columnBindingSource.DataSource = previewDataTable.Columns;
    }

    private void RecreateColumns()
    {
      for (int colIdx = 0; colIdx < previewDataTable.Columns.Count; colIdx++)
      {
        MySQLDataColumn mysqlCol = previewDataTable.GetColumnAtIndex(colIdx);
        DataGridViewColumn gridCol = grdPreviewData.Columns[colIdx];
        gridCol.HeaderText = mysqlCol.DisplayName;
        grdPreviewData.Columns[colIdx].SortMode = DataGridViewColumnSortMode.NotSortable;
      }
      grdPreviewData.SelectionMode = DataGridViewSelectionMode.FullColumnSelect;
      refreshPrimaryKeyColumnsCombo();
    }

    private void SetDefaultPrimaryKey()
    {
      txtAddPrimaryKey.DataBindings.Add(new Binding("Text", previewDataTable.Columns[0], "DisplayName"));
      if (previewDataTable.FirstColumnContainsIntegers)
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

    private void initializeDataTypeCombo()
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

    private void showValidationWarning(string warningControlSuffix, bool show, string text)
    {
      string picBoxName = String.Format("pic{0}", warningControlSuffix);
      string lblName = String.Format("lbl{0}", warningControlSuffix);

      if (contentAreaPanel.Controls.ContainsKey(picBoxName) && contentAreaPanel.Controls.ContainsKey(lblName))
      {
        contentAreaPanel.Controls[picBoxName].Visible = show;
        contentAreaPanel.Controls[lblName].Text = (String.IsNullOrEmpty(text) ? String.Empty : text);
        contentAreaPanel.Controls[lblName].Visible = show;
        return;
      }
      if (grpColumnOptions.Controls.ContainsKey(picBoxName) && grpColumnOptions.Controls.ContainsKey(lblName))
      {
        grpColumnOptions.Controls[picBoxName].Visible = show;
        grpColumnOptions.Controls[lblName].Text = (String.IsNullOrEmpty(text) ? String.Empty : text);
        grpColumnOptions.Controls[lblName].Visible = show;
        return;
      }
    }

    private void flagMultiColumnPrimaryKey(int pkQty)
    {
      radAddPrimaryKey.Checked = pkQty == 0;
      radUseExistingColumn.Checked = pkQty > 0;
      if (cmbPrimaryKeyColumns.Items.Count == 0)
        return;
      if (pkQty < 2 && cmbPrimaryKeyColumns.Items[0].ToString() == "<Multiple Items>")
      {
        cmbPrimaryKeyColumns.Items.RemoveAt(0);
        var name = previewDataTable.Columns.Cast<MySQLDataColumn>().Skip(1).First(i => i.PrimaryKey == true);
        cmbPrimaryKeyColumns.SelectedItem = name.DisplayName;
      }
      else if (pkQty > 1 && cmbPrimaryKeyColumns.Items[0].ToString() != "<Multiple Items>")
      {
        cmbPrimaryKeyColumns.Items.Insert(0, "<Multiple Items>");
        cmbPrimaryKeyColumns.SelectedIndex = 0;
      }
    }

    private bool testColumnDataTypeAgainstColumnData(MySQLDataColumn currentCol)
    {
      bool showWarning = !currentCol.CanBeOfMySQLDataType(cmbDatatype.Text);

      string warningText = (showWarning ? Resources.ExportDataTypeNotSuitableWarning : null);
      if (warningText == null)
      {
        currentCol.WarningTextList.Remove(Resources.ExportDataTypeNotSuitableWarning);
        if (showWarning = currentCol.WarningTextList.Count > 0)
          warningText = currentCol.WarningTextList.Last();
      }
      else
        if (!currentCol.WarningTextList.Contains(Resources.ExportDataTypeNotSuitableWarning))
          currentCol.WarningTextList.Add(Resources.ExportDataTypeNotSuitableWarning);
      showValidationWarning("ColumnOptionsWarning", showWarning, warningText);
      grdPreviewData.SelectedColumns[0].DefaultCellStyle.BackColor = (showWarning ? Color.OrangeRed : grdPreviewData.DefaultCellStyle.BackColor);

      return !showWarning;
    }

    private bool validateUserDataType(MySQLDataColumn currentCol, string proposedUserType)
    {
      bool isValid = false;

      List<int> paramsInParenthesis;
      List<string> dataTypesList = DataTypeUtilities.GetMySQLDataTypes(out paramsInParenthesis);
      int rightParentFound = proposedUserType.IndexOf(")");
      int leftParentFound = proposedUserType.IndexOf("(");
      string pureDataType = String.Empty;
      int typeParametersNum = 0;

      proposedUserType = proposedUserType.Trim().Replace(" ", String.Empty);
      if (rightParentFound >= 0)
      {
        if (leftParentFound < 0 || leftParentFound >= rightParentFound)
          return false;
        typeParametersNum = proposedUserType.Substring(leftParentFound + 1, rightParentFound - leftParentFound - 1).Count(c => c == ',') + 1;
        pureDataType = proposedUserType.Substring(0, leftParentFound).ToLowerInvariant();
      }
      else
        pureDataType = proposedUserType.ToLowerInvariant();
      int typeFoundAt = dataTypesList.IndexOf(pureDataType);
      int numOfValidParams = (typeFoundAt >= 0 ? paramsInParenthesis[typeFoundAt] : -1);
      bool numParamsMatch = (pureDataType.StartsWith("var") ? (numOfValidParams >= 0 && numOfValidParams == typeParametersNum) : (numOfValidParams >= 0 && numOfValidParams == typeParametersNum) || (numOfValidParams < 0 && typeParametersNum > 0) || typeParametersNum == 0);
      isValid = typeFoundAt >= 0 && numParamsMatch;

      bool showWarning = !isValid;
      string warningText = (showWarning ? Resources.ExportDataTypeNotValidWarning : null);
      if (warningText == null)
      {
        currentCol.WarningTextList.Remove(Resources.ExportDataTypeNotValidWarning);
        if (showWarning = currentCol.WarningTextList.Count > 0)
          warningText = currentCol.WarningTextList.Last();
      }
      else
        if (!currentCol.WarningTextList.Contains(Resources.ExportDataTypeNotValidWarning))
          currentCol.WarningTextList.Add(Resources.ExportDataTypeNotValidWarning);
      showValidationWarning("ColumnOptionsWarning", showWarning, warningText);
      grdPreviewData.SelectedColumns[0].DefaultCellStyle.BackColor = (showWarning ? Color.OrangeRed : grdPreviewData.DefaultCellStyle.BackColor);

      return isValid;
    }

    private void btnCopySQL_Click(object sender, EventArgs e)
    {
      StringBuilder queryString = new StringBuilder();
      queryString.Append(exportDataTable.GetCreateSQL(true));
      queryString.AppendFormat(";{0}", Environment.NewLine);
      queryString.Append(exportDataTable.GetInsertSQL(100, true, false));
      Clipboard.SetText(queryString.ToString());
    }

    private void btnExport_Click(object sender, EventArgs e)
    {
      this.Cursor = Cursors.WaitCursor;
      if (exportDataTable == null)
      {
        exportDataTable = previewDataTable.CloneSchema();
        exportDataTable.SetData(this.exportDataRange, false, false, false, false, false, true);
      }
      else
        exportDataTable.SyncSchema(previewDataTable);
      this.Cursor = Cursors.Default;

      bool tableContainsDataToExport = exportDataTable.Rows.Count > (exportDataTable.FirstRowIsHeaders ? 1 : 0);
      if (!tableContainsDataToExport)
      {
        WarningDialog wDiag = new WarningDialog(Properties.Resources.ExportDataNoDataToExportTitleWarning, Properties.Resources.ExportDataNoDataToExportDetailWarning);
        if (wDiag.ShowDialog() == DialogResult.No)
          return;
      }

      this.Cursor = Cursors.WaitCursor;
      Exception exception;
      DataTable warningsTable;
      bool warningsFound = false;
      string operationSummary = String.Format("The MySQL Table \"{0}\"", exportDataTable.TableName);
      StringBuilder operationDetails = new StringBuilder();
      operationDetails.AppendFormat("Creating MySQL Table \"{0}\" with query...{1}{1}", exportDataTable.TableName, Environment.NewLine);
      string queryString = String.Empty;
      warningsTable = exportDataTable.CreateTable(wbConnection, out exception, out queryString);
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
            operationDetails.AppendFormat("{2}Code {0} - {1}",
                                          warningRow[1].ToString(),
                                          warningRow[2].ToString(),
                                          Environment.NewLine);
          }
          operationDetails.Append(Environment.NewLine);
        }
        else
          operationDetails.Append(" successfully.");
      }
      else
      {
        if (exception is MySqlException)
          operationDetails.AppendFormat("MySQL Error {0}:{1}", (exception as MySqlException).Number, Environment.NewLine);
        else
          operationDetails.AppendFormat("ADO.NET Error:{0}", Environment.NewLine);
        operationDetails.Append(exception.Message);
      }
      operationSummary += (success ? "has been created " : "could not be created.");

      if (success && tableContainsDataToExport)
      {
        int insertedCount = 0;
        warningsTable = exportDataTable.InsertDataWithManualQuery(wbConnection, false, out exception, out queryString, out insertedCount);
        operationDetails.AppendFormat("{1}{1}Inserting Excel data in MySQL Table \"{0}\" with query...{1}{1}{2}{1}{1}",
                                    exportDataTable.TableName,
                                    Environment.NewLine,
                                    queryString);
        success = exception == null;
        if (success)
        {
          operationDetails.AppendFormat("{0} rows have been inserted", insertedCount);
          operationSummary += "with data.";
          if (warningsTable != null && warningsTable.Rows.Count > 0)
          {
            warningsFound = true;
            operationDetails.AppendFormat(" with {0} warnings:", warningsTable.Rows.Count);
            foreach (DataRow warningRow in warningsTable.Rows)
            {
              operationDetails.AppendFormat("{2}Code {0} - {1}",
                                            warningRow[1].ToString(),
                                            warningRow[2].ToString(),
                                            Environment.NewLine);
            }
            operationDetails.Append(Environment.NewLine);
          }
          else
            operationDetails.Append(" successfully.");
        }
        else
        {
          operationDetails.AppendFormat("Error while inserting rows...{0}{0}", Environment.NewLine);
          if (exception is MySqlException)
            operationDetails.AppendFormat("MySQL Error {0}:{1}", (exception as MySqlException).Number, Environment.NewLine);
          else
            operationDetails.AppendFormat("ADO.NET Error:{0}", Environment.NewLine);
          operationDetails.Append(exception.Message);
          operationSummary += "with no data.";
        }
      }
      this.Cursor = Cursors.Default;

      InfoDialog.InfoType operationsType = (success ? (warningsFound ? InfoDialog.InfoType.Warning : InfoDialog.InfoType.Success) : InfoDialog.InfoType.Error);
      InfoDialog infoDialog = new InfoDialog(operationsType, operationSummary, operationDetails.ToString());
      DialogResult dr = infoDialog.ShowDialog();
      if (dr == DialogResult.Cancel)
        return;

      DialogResult = DialogResult.OK;
      Close();
    }

    private void btnAdvanced_Click(object sender, EventArgs e)
    {
      ExportAdvancedOptionsDialog optionsDialog = new ExportAdvancedOptionsDialog();
      DialogResult dr = optionsDialog.ShowDialog();
      //if (dr == DialogResult.OK)
      //  btnCopySQL.Visible = Settings.Default.ExportShowCopySQLButton;
    }

    private void chkFirstRowHeaders_CheckedChanged(object sender, EventArgs e)
    {
      int cmbIndex = cmbPrimaryKeyColumns.SelectedIndex;
      int grdIndex = columnBindingSource.Position;
      previewDataTable.FirstRowIsHeaders = chkFirstRowHeaders.Checked;
      RecreateColumns();
      grdPreviewData.CurrentCell = null;
      grdPreviewData.Rows[0].Visible = !chkFirstRowHeaders.Checked;
      cmbPrimaryKeyColumns.SelectedIndex = cmbIndex;
      grdPreviewData.Columns[grdIndex].Selected = true;
      grdPreviewData.FirstDisplayedScrollingColumnIndex = grdIndex;
      if (chkFirstRowHeaders.Checked && grdPreviewData.Rows.Count < 2)
        return;
      grdPreviewData.FirstDisplayedScrollingRowIndex = (chkFirstRowHeaders.Checked ? 1 : 0);
    }

    private void txtTableNameInput_Validating(object sender, CancelEventArgs e)
    {
      timerTextChanged.Stop();

      if (string.IsNullOrWhiteSpace(txtTableNameInput.Text))
      {
        isTableNameValid = false;
        btnExport.Enabled = false;
        return;
      }

      previewDataTable.TableName = txtTableNameInput.Text;

      string cleanTableName = txtTableNameInput.Text.ToLowerInvariant().Replace(" ", "_");
      bool tableExistsInSchema = MySQLDataUtilities.TableExistsInSchema(wbConnection, wbConnection.Schema, cleanTableName);
      if (tableExistsInSchema)
      {
        showValidationWarning("TableNameWarning", true, Properties.Resources.TableNameExistsWarning);
        btnExport.Enabled = false;
        isTableNameValid = false;
        return;
      }

      if (txtTableNameInput.Text.Contains(" ") || txtTableNameInput.Text.Any(char.IsUpper))
      {
        showValidationWarning("TableNameWarning", true, Properties.Resources.NamesWarning);
        btnExport.Enabled = isColumnPKValid;
        isTableNameValid = true;
        return;
      }
      
      showValidationWarning("TableNameWarning", false, null);
      previewDataTable.RefreshSelectQuery();
      isTableNameValid = true;
      btnExport.Enabled = isColumnPKValid;
    }

    private void txtTableNameInput_TextChanged(object sender, EventArgs e)
    {
      timerTextChanged.Stop();
      string name = txtTableNameInput.Text.Trim();
      if (previewDataTable != null)
        previewDataTable.TableName = name;
      txtAddPrimaryKey.Text = (name.Length > 0 ? name + "_id" : name);
      timerTextChanged.Start();
    }

    private void timerTextChanged_Tick(object sender, EventArgs e)
    {
      txtTableNameInput_Validating(txtTableNameInput, new CancelEventArgs());
    }

    private void radAddPrimaryKey_CheckedChanged(object sender, EventArgs e)
    {
      if (!radAddPrimaryKey.Checked)
        return;
      isChanging = true;
      grdPreviewData.Columns[0].Visible = true;
      grdPreviewData.Columns[0].Selected = true;
      grdPreviewData.FirstDisplayedScrollingColumnIndex = 0;
      cmbPrimaryKeyColumns.Text = String.Empty;
      cmbPrimaryKeyColumns.SelectedIndex = -1;
      cmbPrimaryKeyColumns.Enabled = false;
      txtAddPrimaryKey.Enabled = true;
      previewDataTable.UseFirstColumnAsPK = true;
      isChanging = false;
      //EnableChecks(null);
    }

    private void radUseExistingColumn_CheckedChanged(object sender, EventArgs e)
    {
      if (!radUseExistingColumn.Checked)
        return;
      isChanging = true;
      grdPreviewData.Columns[0].Visible = false;
      grdPreviewData.FirstDisplayedScrollingColumnIndex = 1;
      cmbPrimaryKeyColumns.Enabled = true;
      multiColumnPK = false;
      cmbPrimaryKeyColumns.SelectedIndex = 0;
      columnBindingSource.ResetCurrentItem();
      txtAddPrimaryKey.Enabled = false;
      previewDataTable.UseFirstColumnAsPK = false;
      EnableChecks(null);
      isChanging = false;
    }

    private void grdPreviewData_SelectionChanged(object sender, EventArgs e)
    {
      isChanging = true;
      if (grdPreviewData.SelectedColumns.Count > 0)
      {
        columnBindingSource.Position = grdPreviewData.SelectedColumns[0].Index;
        MySQLDataColumn column = columnBindingSource.Current as MySQLDataColumn;
        string warningText = (column.WarningTextList.Count > 0 ? column.WarningTextList.Last() : null);
        showValidationWarning("ColumnOptionsWarning", !String.IsNullOrEmpty(warningText), warningText);
      }
      grpColumnOptions.Enabled = grdPreviewData.SelectedColumns.Count > 0;
      EnableChecks(null);
      if (grdPreviewData.Columns[0].Selected)
        chkUniqueIndex.Enabled = chkCreateIndex.Enabled = chkExcludeColumn.Enabled = chkAllowEmpty.Enabled = chkPrimaryKey.Enabled = false;
      isChanging = false;
    }

    private void cmbPrimaryKeyColumns_SelectedIndexChanged(object sender, EventArgs e)
    {
      if (cmbPrimaryKeyColumns.SelectedIndex == -1)
        return;
      if (multiColumnPK && cmbPrimaryKeyColumns.SelectedIndex == 0)
        return;
      multiColumnPK = false;
      if (cmbPrimaryKeyColumns.Items[0].ToString() == "<Multiple Items>")
      {
        cmbPrimaryKeyColumns.BeginUpdate();
        int index = cmbPrimaryKeyColumns.SelectedIndex;
        cmbPrimaryKeyColumns.Items.RemoveAt(0);
        if (index == 0)
          cmbPrimaryKeyColumns.SelectedIndex = 0;
        cmbPrimaryKeyColumns.EndUpdate();
      }
      for (int coldIdx = 1; coldIdx < previewDataTable.Columns.Count; coldIdx++)
      {
        MySQLDataColumn col = previewDataTable.GetColumnAtIndex(coldIdx);
        col.PrimaryKey = (col.DisplayName == cmbPrimaryKeyColumns.Text);
        if (col.PrimaryKey)
        {
          col.CreateIndex = col.UniqueKey = col.AllowNull = col.ExcludeColumn = false;
          grdPreviewData.Columns[col.ColumnName].Selected = true;
          grdPreviewData.FirstDisplayedScrollingColumnIndex = grdPreviewData.Columns[col.ColumnName].Index;
        }
      }
    }

    private void txtColumnName_TextChanged(object sender, EventArgs e)
    {
      if (txtColumnName.Text == (columnBindingSource.Current as MySQLDataColumn).DisplayName)
        return;
      isChanging = true;
      string name = txtColumnName.Text.Trim();
      int index = grdPreviewData.SelectedColumns[0].Index;
      grdPreviewData.Columns[index].HeaderText = name;

      bool showWarning = (txtTableNameInput.Text.Contains(" ") || txtTableNameInput.Text.Any(char.IsUpper));
      MySQLDataColumn column = columnBindingSource.Current as MySQLDataColumn;
      column.DisplayName = name;
      string warningText = (showWarning ? Resources.NamesWarning : null);
      if (warningText == null)
      {
        column.WarningTextList.Remove(Resources.NamesWarning);
        if (showWarning = column.WarningTextList.Count > 0)
          warningText = column.WarningTextList.Last();
      }
      else
        if (!column.WarningTextList.Contains(Resources.NamesWarning))
          column.WarningTextList.Add(Resources.NamesWarning);
      showValidationWarning("ColumnOptionsWarning", showWarning, warningText);

      if (index > 0)
      {
        cmbPrimaryKeyColumns.BeginUpdate();
        cmbPrimaryKeyColumns.Items[index - 1] = txtColumnName.Text;
        cmbPrimaryKeyColumns.EndUpdate();
      }
      isChanging = false;
    }

    private void chkUniqueIndex_CheckedChanged(object sender, EventArgs e)
    {
      MySQLDataColumn currentCol = columnBindingSource.Current as MySQLDataColumn;
      if (chkUniqueIndex.Checked == currentCol.UniqueKey)
        return;
      currentCol.UniqueKey = chkUniqueIndex.Checked;
      DataGridViewColumn gridCol = grdPreviewData.SelectedColumns[0];
      MySQLDataColumn column = previewDataTable.GetColumnAtIndex(gridCol.Index);
      bool good = true;
      try
      {
        column.Unique = chkUniqueIndex.Checked;
      }
      catch (InvalidConstraintException)
      {
        good = false;
      }
      string warningText = (good ? null : Resources.ColumnDataNotUniqueWarning);
      if (warningText == null)
      {
        column.WarningTextList.Remove(Resources.ColumnDataNotUniqueWarning);
        if (column.WarningTextList.Count > 0)
        {
          good = false;
          warningText = column.WarningTextList.Last();
        }
      }
      else
        if (!column.WarningTextList.Contains(Resources.ColumnDataNotUniqueWarning))
          column.WarningTextList.Add(Resources.ColumnDataNotUniqueWarning);
      showValidationWarning("ColumnOptionsWarning", !good, warningText);
      gridCol.DefaultCellStyle.BackColor = good ? grdPreviewData.DefaultCellStyle.BackColor : Color.OrangeRed;
      EnableChecks(chkUniqueIndex);
    }

    private void chkExcludeColumn_CheckedChanged(object sender, EventArgs e)
    {
      if (chkExcludeColumn.Checked == (columnBindingSource.Current as MySQLDataColumn).ExcludeColumn)
        return;
      (columnBindingSource.Current as MySQLDataColumn).ExcludeColumn = chkExcludeColumn.Checked;
      DataGridViewColumn gridCol = grdPreviewData.SelectedColumns[0];
      gridCol.DefaultCellStyle.BackColor = chkExcludeColumn.Checked ? Color.LightGray : grdPreviewData.DefaultCellStyle.BackColor;
      int grdIndex = grdPreviewData.SelectedColumns[0].Index;
      EnableChecks(chkExcludeColumn);
      refreshPrimaryKeyColumnsCombo();
      grdPreviewData.Columns[grdIndex].Selected = true;
    }

    private void chkPrimaryKey_CheckedChanged(object sender, EventArgs e)
    {
      if (chkPrimaryKey.Checked == (columnBindingSource.Current as MySQLDataColumn).PrimaryKey)
        return;
      (columnBindingSource.Current as MySQLDataColumn).PrimaryKey = chkPrimaryKey.Checked;
      EnableChecks(chkPrimaryKey);
      chkPrimaryKey_Validated(sender, e);
    }

    private void chkCreateIndex_CheckedChanged(object sender, EventArgs e)
    {
      if (chkCreateIndex.Checked == (columnBindingSource.Current as MySQLDataColumn).CreateIndex)
        return;
      EnableChecks(chkCreateIndex);
    }

    private void grdPreviewData_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
    {
      if (e.ListChangedType != ListChangedType.Reset)
        return;
      grdPreviewData.CurrentCell = null;
      grdPreviewData.Rows[0].Visible = !chkFirstRowHeaders.Checked;
    }

    private void txtAddPrimaryKey_TextChanged(object sender, EventArgs e)
    {
      bool showWarning = false;
      string warningText = null;
      for (int colIdx = 1; colIdx < previewDataTable.Columns.Count; colIdx++)
      {
        MySQLDataColumn col = previewDataTable.GetColumnAtIndex(colIdx);
        showWarning = showWarning || col.DisplayName.ToLowerInvariant() == txtAddPrimaryKey.Text.ToLowerInvariant();
        if (showWarning)
        {
          warningText = Resources.PrimaryKeyColumnExistsWarning;
          break;
        }
      }
      isColumnPKValid = !showWarning;
      btnExport.Enabled = isColumnPKValid && isTableNameValid;
      showValidationWarning("PrimaryKeyWarning", showWarning, Properties.Resources.PrimaryKeyColumnExistsWarning);
      previewDataTable.GetColumnAtIndex(0).DisplayName = txtAddPrimaryKey.Text;
      grdPreviewData.Columns[0].HeaderText = txtAddPrimaryKey.Text;
      if (columnBindingSource.Position == 0)
        columnBindingSource.ResetCurrentItem();
    }

    private void txtColumnName_Validated(object sender, EventArgs e)
    {
      isChanging = true;
      if (txtColumnName.Text != (columnBindingSource.Current as MySQLDataColumn).DisplayName)
      {
        columnBindingSource.ResetCurrentItem();
        int index = (grdPreviewData.SelectedColumns.Count > 0 ? grdPreviewData.SelectedColumns[0].Index : -1);
        if (index > 0)
        {
          cmbPrimaryKeyColumns.Items[index - 1] = txtColumnName.Text;
          grdPreviewData.SelectedColumns[0].HeaderText = txtColumnName.Text;
        }
      }
      isChanging = false;
    }

    private void chkPrimaryKey_Validated(object sender, EventArgs e)
    {
      if (!isChanging)
      {
        int currentPKQty = previewDataTable.NumberOfPK;
        multiColumnPK = currentPKQty > 1;
        flagMultiColumnPrimaryKey(currentPKQty);
      }
    }

    private void grdPreviewData_KeyDown(object sender, KeyEventArgs e)
    {
      if (grdPreviewData.SelectedColumns.Count == 0)
        return;
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

    private void cmbDatatype_SelectedIndexChanged(object sender, EventArgs e)
    {
      MySQLDataColumn currentCol = columnBindingSource.Current as MySQLDataColumn;
      if (cmbDatatype.Text == currentCol.MySQLDataType || cmbDatatype.Text.Length == 0 || (cmbDatatype.DataSource as DataTable).Select(String.Format("Value = '{0}'", cmbDatatype.Text)).Length == 0)
        return;
      currentCol.MySQLDataType = cmbDatatype.Text;
      testColumnDataTypeAgainstColumnData(currentCol);
      if (Settings.Default.ExportAutoIndexIntColumns && cmbDatatype.Text.StartsWith("Integer") && !chkCreateIndex.Checked)
        chkCreateIndex.Checked = true;
    }

    private void cmbDatatype_DrawItem(object sender, DrawItemEventArgs e)
    {
      e.DrawBackground();
      e.Graphics.DrawString((cmbDatatype.Items[e.Index] as DataRowView)["Description"].ToString(), cmbDatatype.Font, System.Drawing.Brushes.Black, new RectangleF(e.Bounds.X, e.Bounds.Y, e.Bounds.Width, e.Bounds.Height));
      e.DrawFocusRectangle();
    }

    private void grdPreviewData_CellToolTipTextNeeded(object sender, DataGridViewCellToolTipTextNeededEventArgs e)
    {
      if (e.RowIndex >= 0)
        e.ToolTipText = Resources.ExportColumnsGridToolTipCaption;
      else
        e.ToolTipText = grdPreviewData.Columns[e.ColumnIndex].HeaderText;
    }

    private void cmbDatatype_Validating(object sender, CancelEventArgs e)
    {
      if (cmbDatatype.SelectedIndex >= 0)
        return;
      MySQLDataColumn currentCol = columnBindingSource.Current as MySQLDataColumn;
      bool valid = validateUserDataType(currentCol, cmbDatatype.Text);
      if (valid)
        testColumnDataTypeAgainstColumnData(currentCol);
      if (Settings.Default.ExportAutoIndexIntColumns && cmbDatatype.Text.StartsWith("Integer") && !chkCreateIndex.Checked)
        chkCreateIndex.Checked = true;
    }

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
          chkAllowEmpty.Checked = true;
      }

      //toColumn.ExcludeColumn = chkExcludeColumn.Checked;
      columnBindingSource.EndEdit();

      chkExcludeColumn.Enabled = true;
      chkPrimaryKey.Enabled = !(chkExcludeColumn.Checked || radAddPrimaryKey.Checked);
      chkUniqueIndex.Enabled = !chkExcludeColumn.Checked;
      chkCreateIndex.Enabled = !(chkExcludeColumn.Checked || chkUniqueIndex.Checked || chkPrimaryKey.Checked);
      chkAllowEmpty.Enabled = !(chkExcludeColumn.Checked || chkPrimaryKey.Checked);
      radUseExistingColumn.Enabled = !(previewDataTable.Columns.Cast<MySQLDataColumn>().Skip(1).All(i => i.ExcludeColumn));
      cmbPrimaryKeyColumns.Enabled = radUseExistingColumn.Enabled && radUseExistingColumn.Checked;
      cmbDatatype.Enabled = !column.AutoPK;

      if(columnBindingSource.Position == 0)
        chkUniqueIndex.Enabled = chkCreateIndex.Enabled = chkExcludeColumn.Enabled = chkAllowEmpty.Enabled = chkPrimaryKey.Enabled = false;
    }

    private void ExportDataForm_Load(object sender, EventArgs e)
    {
      grdPreviewData.Columns[grdPreviewData.Columns[0].Visible ? 0 : 1].Selected = true;
    }
  }
}
