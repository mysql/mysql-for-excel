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
  public partial class ExportDataForm : Form
  {
    private MySQLDataTable dataTable;
    private MySqlWorkbenchConnection wbConnection;
    private bool multiColumnPK = false;

    public ExportDataForm(MySqlWorkbenchConnection wbConnection, Excel.Range exportDataRange, Excel.Worksheet exportingWorksheet)
    {
      this.wbConnection = wbConnection;

      InitializeComponent();

      if (!exportingWorksheet.Name.ToLowerInvariant().StartsWith("sheet"))
        txtTableNameInput.Text = exportingWorksheet.Name.ToLower().Replace(' ', '_');
      Text = String.Format("Export Data - {0} [{1}])", exportingWorksheet.Name, exportDataRange.Address.Replace("$", String.Empty));

      LoadDataAndCreateColumns(exportDataRange);
      SetDefaultPrimaryKey();
      initializeDataTypeCombo();

      txtTableNameInput.SelectAll();
      btnCopySQL.Visible = Properties.Settings.Default.ExportShowCopySQLButton;
      chkFirstRowHeaders_CheckedChanged(chkFirstRowHeaders, EventArgs.Empty);
    }

    private void LoadDataAndCreateColumns(Excel.Range exportDataRange)
    {
      if (exportDataRange != null)
      {
        dataTable = new MySQLDataTable();
        dataTable.TableName = txtTableNameInput.Text;
        dataTable.SetData(exportDataRange, Settings.Default.ExportUseFormattedValues, Settings.Default.ExportDetectDatatype);
        grdPreviewData.DataSource = dataTable;
        columnBindingSource.DataSource = dataTable.Columns;
        return;
      }
      cmbPrimaryKeyColumns.Items.Clear();
      for (int colIdx = 0; colIdx < dataTable.Columns.Count; colIdx++)
      {
        MySQLDataColumn mysqlCol = dataTable.Columns[colIdx] as MySQLDataColumn;
        DataGridViewColumn gridCol = grdPreviewData.Columns[colIdx];
        gridCol.HeaderText = mysqlCol.DisplayName;
        grdPreviewData.Columns[colIdx].SortMode = DataGridViewColumnSortMode.NotSortable;
        if (colIdx > 0)
          cmbPrimaryKeyColumns.Items.Add(mysqlCol);
      }
      cmbPrimaryKeyColumns.ValueMember = "DisplayName";
      cmbPrimaryKeyColumns.DisplayMember = "DisplayName";
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
      cmbDatatype.DropDownWidth = 300;
    }

    private void showValidationWarning(string warningControlSuffix, bool show, string text)
    {
      string picBoxName = String.Format("pic{0}", warningControlSuffix);
      string lblName = String.Format("lbl{0}", warningControlSuffix);

      if (!ExportDataPanel.Controls.ContainsKey(picBoxName) || !ExportDataPanel.Controls.ContainsKey(lblName))
        return;

      ExportDataPanel.Controls[picBoxName].Visible = show;
      if (!String.IsNullOrEmpty(text))
        ExportDataPanel.Controls[lblName].Text = text;
      ExportDataPanel.Controls[lblName].Visible = show;
    }

    private void flagMultiColumnPrimaryKey(bool multiColPK)
    {
      radAddPrimaryKey.Checked = false;
      radUseExistingColumn.Checked = multiColPK;
      cmbDatatype.Text = (multiColPK ? "<Multiple columns>" : String.Empty);
      cmbDatatype.Enabled = !multiColPK;
    }

    private void btnCopySQL_Click(object sender, EventArgs e)
    {

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
    }

    private void chkFirstRowHeaders_CheckedChanged(object sender, EventArgs e)
    {
      dataTable.FirstRowIsHeaders = chkFirstRowHeaders.Checked;
      LoadDataAndCreateColumns(null);
      grdPreviewData.CurrentCell = null;
      grdPreviewData.Rows[0].Visible = !chkFirstRowHeaders.Checked;
      if (chkFirstRowHeaders.Checked && grdPreviewData.Rows.Count < 2)
        return;
      grdPreviewData.FirstDisplayedScrollingRowIndex = (chkFirstRowHeaders.Checked ? 1 : 0);
    }

    private void txtTableNameInput_Validating(object sender, CancelEventArgs e)
    {
      timerTextChanged.Stop();

      bool showWarning = (txtTableNameInput.Text.Contains(" ") || txtTableNameInput.Text.Any(char.IsUpper));
      showValidationWarning("TableNameWarning", showWarning, Properties.Resources.NamesWarning);

      string cleanTableName = txtTableNameInput.Text.ToLowerInvariant().Replace(" ", "_");
      bool tableExistsInSchema = Utilities.TableExistsInSchema(wbConnection, wbConnection.Schema, cleanTableName);

      if (tableExistsInSchema)
      {
        showValidationWarning("TableNameWarning", true, Properties.Resources.TableNameExistsWarning);
        btnExport.Enabled = false;
      }
      else
      {
        showValidationWarning("TableNameWarning", showWarning, null);
        btnExport.Enabled = true;
      }
      dataTable.TableName = txtTableNameInput.Text;
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
      grdPreviewData.CurrentCell = grdPreviewData[0, 1];
      cmbPrimaryKeyColumns.Enabled = false;
      txtAddPrimaryKey.Enabled = true;
      dataTable.AddPK = true;
      chkPrimaryKey.Enabled = !radAddPrimaryKey.Checked;
    }

    private void radUseExistingColumn_CheckedChanged(object sender, EventArgs e)
    {
      if (!radUseExistingColumn.Checked)
        return;
      grdPreviewData.Columns[0].Visible = false;
      grdPreviewData.CurrentCell = grdPreviewData[1, 1];
      cmbPrimaryKeyColumns.Enabled = true;
      txtAddPrimaryKey.Enabled = false;
      dataTable.AddPK = false;
      chkPrimaryKey.Enabled = !radAddPrimaryKey.Checked;
    }

    private void txtAddPrimaryKey_Validating(object sender, CancelEventArgs e)
    {
      bool showWarning = false;
      foreach (MySQLDataColumn col in dataTable.Columns)
      {
        showWarning = showWarning || col.DisplayName.ToLowerInvariant() == txtAddPrimaryKey.Text.ToLowerInvariant();
        if (showWarning)
          break;
      }
      btnExport.Enabled = !showWarning;
      showValidationWarning("PrimaryKeyWarning", showWarning, Properties.Resources.PrimaryKeyColumnExistsWarning);
      (dataTable.Columns[0] as MySQLDataColumn).DisplayName = txtAddPrimaryKey.Text;
    }

    //private void bindColsList_ListChanged(object sender, ListChangedEventArgs e)
    //{
    //  if (e.PropertyDescriptor == null || e.PropertyDescriptor.Name != "PrimaryKey")
    //    return;
    //  List<MySQLColumn> realColumns = new List<MySQLColumn>(exportTable.Columns);
    //  realColumns.RemoveAt(0);
    //  bool multiKey = realColumns.Count(col => col.PrimaryKey) > 1;
    //  if (multiKey && !multiColumnPK)
    //    flagMultiColumnPrimaryKey(true);
    //  else if (!multiKey && multiColumnPK)
    //    flagMultiColumnPrimaryKey(false);
    //  multiColumnPK = multiKey;
    //}

    //private void columnBindingSource_CurrentChanged(object sender, EventArgs e)
    //{
    //  if (columnBindingSource.Current != null && grdPreviewData.ColumnCount > 0)
    //    grdPreviewData.Columns[columnBindingSource.Position].Selected = true;
    //}

    private void grdPreviewData_SelectionChanged(object sender, EventArgs e)
    {
      if (grdPreviewData.SelectedColumns.Count > 0)
      {
        columnBindingSource.Position = grdPreviewData.SelectedColumns[0].DisplayIndex;
        grdPreviewData.SelectedColumns[0].HeaderText = (columnBindingSource.Current as MySQLDataColumn).DisplayName;
      }
      grpColumnOptions.Enabled = grdPreviewData.SelectedColumns.Count > 0;
      chkUniqueIndex.Enabled = chkCreateIndex.Enabled = chkExcludeColumn.Enabled = chkAllowEmpty.Enabled = !grdPreviewData.Columns[0].Selected;
      chkPrimaryKey.Enabled = !radAddPrimaryKey.Checked;
    }

    //private void columnBindingSource_BindingComplete(object sender, BindingCompleteEventArgs e)
    //{
    //  grdPreviewData.Columns[0].Visible = radAddPrimaryKey.Checked;
    //  if (txtTableNameInput.Text.Length > 0)
    //    timerTextChanged.Start();
    //}

    private void cmbDatatype_DropDown(object sender, EventArgs e)
    {
      cmbDatatype.DisplayMember = "Description";
    }

    private void cmbDatatype_DropDownClosed(object sender, EventArgs e)
    {
      cmbDatatype.DisplayMember = "Value";
//      string item = cmbDatatype.SelectedItem as string;
  //    item = item.Substring(0, item.IndexOf(" - "));
    //  cmbDatatype.Text = item;
      //cmbDatatype.DisplayMember = "Value";
    }

    private void cmbPrimaryKeyColumns_SelectedIndexChanged(object sender, EventArgs e)
    {
      
    }

    private void cmbPrimaryKeyColumns_Validating(object sender, CancelEventArgs e)
    {
      if (multiColumnPK)
      {
        DialogResult dr = Utilities.ShowWarningBox("Do you want to reset the Primary Key to the single selected column?");
        if (dr == DialogResult.No)
        {
          e.Cancel = true;
          return;
        }
        multiColumnPK = false;
      }
      for (int coldIdx = 1; coldIdx < dataTable.Columns.Count; coldIdx++)
      {
        MySQLDataColumn col = (dataTable.Columns[coldIdx] as MySQLDataColumn);
        col.PrimaryKey = (col.ColumnName == cmbPrimaryKeyColumns.Text);
      }
      if (cmbPrimaryKeyColumns.Items[0].ToString() == "<Multiple Items>")
        cmbPrimaryKeyColumns.Items.RemoveAt(0);
    }

    private void txtColumnName_TextChanged(object sender, EventArgs e)
    {
      //string name = txtColumnName.Text.Trim();
      //int index = grdPreviewData.SelectedColumns[0].Index;
      //grdPreviewData.Columns[index].HeaderText = name;
      //grdPreviewData.Columns[index].DataPropertyName = name;
    }

    private void chkUniqueIndex_CheckedChanged(object sender, EventArgs e)
    {
      DataGridViewColumn gridCol = grdPreviewData.SelectedColumns[0];
      DataColumn column = dataTable.Columns[gridCol.Index];
      bool good = true;
      try
      {
        column.Unique = chkUniqueIndex.Checked;
      }
      catch (InvalidConstraintException)
      {
        good = false;
      }
      gridCol.DefaultCellStyle.BackColor = good ? grdPreviewData.DefaultCellStyle.BackColor : Color.FromArgb(255, 200, 200);
    }

    private void chkExcludeColumn_CheckedChanged(object sender, EventArgs e)
    {
      DataGridViewColumn gridCol = grdPreviewData.SelectedColumns[0];
      DataColumn column = dataTable.Columns[gridCol.Index];
      gridCol.DefaultCellStyle.BackColor = chkExcludeColumn.Checked ? Color.LightGray : grdPreviewData.DefaultCellStyle.BackColor;
    }

    private void chkPrimaryKey_CheckedChanged(object sender, EventArgs e)
    {
      if (dataTable.NumberOfPK > 1)
      {
        cmbPrimaryKeyColumns.Items.Insert(0, "<Multiple Items>");
        cmbPrimaryKeyColumns.SelectedIndex = 0;
        radUseExistingColumn.Checked = true;
      }
    }

    private void grdPreviewData_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
    {
      if (e.ListChangedType != ListChangedType.Reset)
        return;
      grdPreviewData.Columns[0].Visible = radAddPrimaryKey.Checked;
      grdPreviewData.Rows[0].Visible = !chkFirstRowHeaders.Checked;
    }

  }
}
