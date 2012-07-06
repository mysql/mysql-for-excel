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
    private MySQLDataTable dataTable;
    private MySqlWorkbenchConnection wbConnection;
    private bool multiColumnPK = false;

    public ExportDataForm(MySqlWorkbenchConnection wbConnection, Excel.Range exportDataRange, Excel.Worksheet exportingWorksheet)
    {
      string proposedTableName = String.Empty;
      this.wbConnection = wbConnection;

      InitializeComponent();

      if (!exportingWorksheet.Name.ToLowerInvariant().StartsWith("sheet"))
        proposedTableName = exportingWorksheet.Name.ToLower().Replace(' ', '_');
      Text = String.Format("Export Data - {0} [{1}])", exportingWorksheet.Name, exportDataRange.Address.Replace("$", String.Empty));

      LoadDataAndCreateColumns(exportDataRange, proposedTableName);
      SetDefaultPrimaryKey();
      initializeDataTypeCombo();

      if (!String.IsNullOrEmpty(proposedTableName))
        txtTableNameInput.Text = proposedTableName;
      txtTableNameInput.SelectAll();
      btnCopySQL.Visible = Properties.Settings.Default.ExportShowCopySQLButton;
      chkFirstRowHeaders_CheckedChanged(chkFirstRowHeaders, EventArgs.Empty);
    }

    private void refreshPrimaryKeyColumnsCombo()
    {
      string selectedItem = (radUseExistingColumn.Checked ? cmbPrimaryKeyColumns.SelectedItem.ToString() : String.Empty);
      cmbPrimaryKeyColumns.Items.Clear();
      foreach (MySQLDataColumn mysqlCol in dataTable.Columns.OfType<MySQLDataColumn>().Skip(1))
      {
        if (mysqlCol.ExcludeColumn)
          continue;
        cmbPrimaryKeyColumns.Items.Add(mysqlCol.DisplayName);
      }
      if (selectedItem.Length > 0)
        cmbPrimaryKeyColumns.SelectedItem = selectedItem;
    }

    private void LoadDataAndCreateColumns(Excel.Range exportDataRange, string proposedTableName)
    {
      if (exportDataRange != null)
      {
        dataTable = new MySQLDataTable();
        dataTable.RemoveEmptyColumns = true;
        if (!String.IsNullOrEmpty(proposedTableName))
          dataTable.TableName = proposedTableName;
        dataTable.SetData(exportDataRange, 
                          Settings.Default.ExportUseFormattedValues, 
                          Settings.Default.ExportDetectDatatype,
                          Settings.Default.ExportAddBufferToVarchar,
                          Settings.Default.ExportAutoIndexIntColumns, 
                          Settings.Default.ExportAutoAllowEmptyNonIndexColumns);
        grdPreviewData.DataSource = dataTable;
        columnBindingSource.DataSource = dataTable.Columns;
        return;
      }

      for (int colIdx = 0; colIdx < dataTable.Columns.Count; colIdx++)
      {
        MySQLDataColumn mysqlCol = dataTable.Columns[colIdx] as MySQLDataColumn;
        DataGridViewColumn gridCol = grdPreviewData.Columns[colIdx];
        gridCol.HeaderText = mysqlCol.DisplayName;
        if (gridCol.ToolTipText.Length == 0)
          gridCol.ToolTipText = Resources.ExportColumnsGridToolTipCaption;
        grdPreviewData.Columns[colIdx].SortMode = DataGridViewColumnSortMode.NotSortable;
      }
      refreshPrimaryKeyColumnsCombo();

      grdPreviewData.SelectionMode = DataGridViewSelectionMode.FullColumnSelect;
    }

    private void SetDefaultPrimaryKey()
    {
      txtAddPrimaryKey.DataBindings.Add(new Binding("Text", dataTable.Columns[0], "DisplayName"));
      if (dataTable.FirstColumnContainsIntegers)
      {
        radUseExistingColumn.Checked = true;
        cmbPrimaryKeyColumns.SelectedIndex = 0;
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
      if (pkQty < 2 && cmbPrimaryKeyColumns.Items[0].ToString() == "<Multiple Items>")
        cmbPrimaryKeyColumns.Items.RemoveAt(0);
      else if (pkQty > 1 && cmbPrimaryKeyColumns.Items[0].ToString() != "<Multiple Items>")
        cmbPrimaryKeyColumns.Items.Insert(0, "<Multiple Items>");
      cmbPrimaryKeyColumns.SelectedIndex = 0;
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
      List<string> dataTypesList = Utilities.GetDataTypes(out paramsInParenthesis);
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
      queryString.AppendFormat("USE `{0}`;\n", wbConnection.Schema);
      queryString.Append(dataTable.GetCreateSQL(true));
      queryString.Append(";\n");
      queryString.Append(dataTable.GetInsertSQL(100, true));
      queryString.Append(";");
      Clipboard.SetText(queryString.ToString());
    }

    private void btnExport_Click(object sender, EventArgs e)
    {
      bool success = dataTable.CreateTable(wbConnection);
      success = success && dataTable.InsertDataWithAdapter(wbConnection, chkFirstRowHeaders.Checked, Properties.Settings.Default.ExportUseFormattedValues);
      if (success)
      {
        DialogResult = DialogResult.OK;
        Close();
      }
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
      dataTable.FirstRowIsHeaders = chkFirstRowHeaders.Checked;
      LoadDataAndCreateColumns(null, null);
      grdPreviewData.CurrentCell = null;
      grdPreviewData.Rows[0].Visible = !chkFirstRowHeaders.Checked;
      if (chkFirstRowHeaders.Checked && grdPreviewData.Rows.Count < 2)
        return;
      grdPreviewData.FirstDisplayedScrollingRowIndex = (chkFirstRowHeaders.Checked ? 1 : 0);
    }

    private void txtTableNameInput_Validating(object sender, CancelEventArgs e)
    {
      timerTextChanged.Stop();

      dataTable.TableName = txtTableNameInput.Text;

      string cleanTableName = txtTableNameInput.Text.ToLowerInvariant().Replace(" ", "_");
      bool tableExistsInSchema = Utilities.TableExistsInSchema(wbConnection, wbConnection.Schema, cleanTableName);
      if (tableExistsInSchema)
      {
        showValidationWarning("TableNameWarning", true, Properties.Resources.TableNameExistsWarning);
        btnExport.Enabled = false;
        return;
      }

      if (txtTableNameInput.Text.Contains(" ") || txtTableNameInput.Text.Any(char.IsUpper))
      {
        showValidationWarning("TableNameWarning", true, Properties.Resources.NamesWarning);
        btnExport.Enabled = true;
        return;
      }
      
      showValidationWarning("TableNameWarning", false, null);
      btnExport.Enabled = true;
    }

    private void txtTableNameInput_TextChanged(object sender, EventArgs e)
    {
      timerTextChanged.Stop();
      string name = txtTableNameInput.Text.Trim();
      if (dataTable != null)
        dataTable.TableName = name;
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
      grdPreviewData.Columns[0].Visible = true;
      grdPreviewData.Columns[0].Selected = true;
      grdPreviewData.FirstDisplayedScrollingColumnIndex = 0;
      cmbPrimaryKeyColumns.Text = String.Empty;
      cmbPrimaryKeyColumns.Enabled = false;
      txtAddPrimaryKey.Enabled = true;
      dataTable.AddPK = true;
    }

    private void radUseExistingColumn_CheckedChanged(object sender, EventArgs e)
    {
      if (!radUseExistingColumn.Checked)
        return;
      grdPreviewData.Columns[0].Visible = false;
      grdPreviewData.Columns[1].Selected = true;
      grdPreviewData.FirstDisplayedScrollingColumnIndex = 1;
      cmbPrimaryKeyColumns.Enabled = true;
      cmbPrimaryKeyColumns.SelectedIndex = 0;
      txtAddPrimaryKey.Enabled = false;
      dataTable.AddPK = false;
    }

    private void grdPreviewData_SelectionChanged(object sender, EventArgs e)
    {
      if (grdPreviewData.SelectedColumns.Count > 0)
      {
        columnBindingSource.Position = grdPreviewData.SelectedColumns[0].Index;
        MySQLDataColumn column = columnBindingSource.Current as MySQLDataColumn;
        string warningText = (column.WarningTextList.Count > 0 ? column.WarningTextList.Last() : null);
        showValidationWarning("ColumnOptionsWarning", !String.IsNullOrEmpty(warningText), warningText);
      }
      grpColumnOptions.Enabled = grdPreviewData.SelectedColumns.Count > 0;
      chkUniqueIndex.Enabled = chkCreateIndex.Enabled = chkExcludeColumn.Enabled = chkAllowEmpty.Enabled = !grdPreviewData.Columns[0].Selected;
      chkPrimaryKey.Enabled = !radAddPrimaryKey.Checked;
      chkExcludeColumn.Enabled = chkUniqueIndex.Enabled = chkCreateIndex.Enabled = !chkPrimaryKey.Checked;
    }

    private void cmbPrimaryKeyColumns_SelectedIndexChanged(object sender, EventArgs e)
    {
      if (multiColumnPK && cmbPrimaryKeyColumns.SelectedIndex == 0)
        return;
      multiColumnPK = false;
      for (int coldIdx = 1; coldIdx < dataTable.Columns.Count; coldIdx++)
      {
        MySQLDataColumn col = (dataTable.Columns[coldIdx] as MySQLDataColumn);
        col.PrimaryKey = (col.DisplayName == cmbPrimaryKeyColumns.Text);
      }
      if (cmbPrimaryKeyColumns.Items[0].ToString() == "<Multiple Items>")
        cmbPrimaryKeyColumns.Items.RemoveAt(0);
      if (grdPreviewData.Columns[cmbPrimaryKeyColumns.SelectedIndex + 1].Selected)
      {
        columnBindingSource.ResetCurrentItem();
        chkExcludeColumn.Enabled = chkUniqueIndex.Enabled = chkCreateIndex.Enabled = !chkPrimaryKey.Checked;
      }
      else
      {
        grdPreviewData.Columns[cmbPrimaryKeyColumns.SelectedIndex + 1].Selected = true;
        grdPreviewData.FirstDisplayedScrollingColumnIndex = cmbPrimaryKeyColumns.SelectedIndex + 1;
      }
    }

    private void txtColumnName_TextChanged(object sender, EventArgs e)
    {
      if (txtColumnName.Text == (columnBindingSource.Current as MySQLDataColumn).DisplayName)
        return;
      string name = txtColumnName.Text.Trim();
      int index = grdPreviewData.SelectedColumns[0].Index;
      grdPreviewData.Columns[index].HeaderText = name;

      bool showWarning = (txtTableNameInput.Text.Contains(" ") || txtTableNameInput.Text.Any(char.IsUpper));
      MySQLDataColumn column = columnBindingSource.Current as MySQLDataColumn;
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
        cmbPrimaryKeyColumns.Items[index - 1] = txtColumnName.Text;
    }

    private void chkUniqueIndex_CheckedChanged(object sender, EventArgs e)
    {
      MySQLDataColumn currentCol = columnBindingSource.Current as MySQLDataColumn;
      if (chkUniqueIndex.Checked == currentCol.UniqueKey)
        return;
      DataGridViewColumn gridCol = grdPreviewData.SelectedColumns[0];
      MySQLDataColumn column = dataTable.Columns[gridCol.Index] as MySQLDataColumn;
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
      if (chkUniqueIndex.Checked && !chkCreateIndex.Checked)
        chkCreateIndex.Checked = true;
      if (chkAllowEmpty.Checked && chkUniqueIndex.Checked)
        chkAllowEmpty.Checked = false;
      currentCol.UniqueKey = chkUniqueIndex.Checked;
    }

    private void chkExcludeColumn_CheckedChanged(object sender, EventArgs e)
    {
      if (chkExcludeColumn.Checked == (columnBindingSource.Current as MySQLDataColumn).ExcludeColumn)
        return;
      DataGridViewColumn gridCol = grdPreviewData.SelectedColumns[0];
      gridCol.DefaultCellStyle.BackColor = chkExcludeColumn.Checked ? Color.LightGray : grdPreviewData.DefaultCellStyle.BackColor;
    }

    private void chkPrimaryKey_CheckedChanged(object sender, EventArgs e)
    {
      if (chkPrimaryKey.Checked == (columnBindingSource.Current as MySQLDataColumn).PrimaryKey)
        return;
      if (chkExcludeColumn.Checked && chkPrimaryKey.Checked)
        chkExcludeColumn.Checked = chkUniqueIndex.Checked = chkCreateIndex.Checked = false;
      if (chkAllowEmpty.Checked && chkPrimaryKey.Checked)
        chkAllowEmpty.Checked = false;
      chkExcludeColumn.Enabled = chkUniqueIndex.Enabled = chkCreateIndex.Enabled = !chkPrimaryKey.Checked;
    }

    private void chkCreateIndex_CheckedChanged(object sender, EventArgs e)
    {
      if (chkCreateIndex.Checked == (columnBindingSource.Current as MySQLDataColumn).CreateIndex)
        return;
      if (Settings.Default.ExportAutoAllowEmptyNonIndexColumns && !chkCreateIndex.Checked)
        chkAllowEmpty.Checked = true;
    }

    private void grdPreviewData_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
    {
      if (e.ListChangedType != ListChangedType.Reset)
        return;
      grdPreviewData.Columns[0].Visible = radAddPrimaryKey.Checked;
      grdPreviewData.Rows[0].Visible = !chkFirstRowHeaders.Checked;
    }

    private void txtAddPrimaryKey_TextChanged(object sender, EventArgs e)
    {
      bool showWarning = false;
      string warningText = null;
      for (int colIdx = 1; colIdx < dataTable.Columns.Count; colIdx++)
      {
        MySQLDataColumn col = dataTable.Columns[colIdx] as MySQLDataColumn;
        showWarning = showWarning || col.DisplayName.ToLowerInvariant() == txtAddPrimaryKey.Text.ToLowerInvariant();
        if (showWarning)
        {
          warningText = Resources.PrimaryKeyColumnExistsWarning;
          break;
        }
      }
      btnExport.Enabled = !showWarning;
      showValidationWarning("PrimaryKeyWarning", showWarning, Properties.Resources.PrimaryKeyColumnExistsWarning);
      (dataTable.Columns[0] as MySQLDataColumn).DisplayName = txtAddPrimaryKey.Text;
      grdPreviewData.Columns[0].HeaderText = txtAddPrimaryKey.Text;
      if (columnBindingSource.Position == 0)
        columnBindingSource.ResetCurrentItem();
    }

    private void txtColumnName_Validated(object sender, EventArgs e)
    {
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
    }

    private void chkExcludeColumn_Validated(object sender, EventArgs e)
    {
      refreshPrimaryKeyColumnsCombo();
    }

    private void chkPrimaryKey_Validated(object sender, EventArgs e)
    {
      int currentPKQty = dataTable.NumberOfPK;
      multiColumnPK = currentPKQty > 1;
      flagMultiColumnPrimaryKey(currentPKQty);
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
      e.ToolTipText = Resources.ExportColumnsGridToolTipCaption;
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

  }
}
