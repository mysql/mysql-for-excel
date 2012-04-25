using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Excel = Microsoft.Office.Interop.Excel;

namespace MySQL.ExcelAddIn.Forms
{
  public partial class ExportDataToTableForm : Form
  {
    private MySQLSchemaInfo localSchemaInfo;
    private TableSchemaInfo columnSchemaInfo;
    
    private DataTable columnMappingData;
    private DataTable formattedExcelData;
    private DataTable unformattedExcelData;

    public string SelectedSchema
    {
      get { return cmbExistingSchema.SelectedValue.ToString(); }
    }
    public string SelectedTable
    {
      get { return (radExistingTable.Checked ? cmbExistingTable.SelectedValue.ToString() : txtNewTable.Text); }
    }
    public DataTable ColumnMappingData
    {
      get { return columnMappingData; }
    }

    public ExportDataToTableForm(bool toNewTable)
    {
      InitializeComponent();
      ResetForm(toNewTable, false);
    }

    public void InitializeDefaultData(MySQLSchemaInfo schemaInfo)
    {
      localSchemaInfo = schemaInfo;
      if (SelectedSchema != String.Empty && SelectedTable != String.Empty)
      {
        columnSchemaInfo = localSchemaInfo.GetTableSchemaInfo(SelectedTable);
        columnsBindingSource.DataSource = columnSchemaInfo;
      }

      cmbExistingSchema.DataSource = localSchemaInfo.SchemasTable;
      cmbExistingSchema.DisplayMember = cmbExistingSchema.ValueMember = "SCHEMA_NAME";
      cmbExistingTable.DataSource = localSchemaInfo.TablesTable;
      cmbExistingTable.DisplayMember = cmbExistingTable.ValueMember = "TABLE_NAME";
      cmbDBEngine.DataSource = localSchemaInfo.EnginesTable;
      cmbDBEngine.DisplayMember = cmbDBEngine.ValueMember = "ENGINE";
      cmbColumnType.DataSource = localSchemaInfo.DataTypesList;
    }

    public void SetSelectedExcelData(Excel.Range selectedRange)
    {
      MessageBox.Show(selectedRange.Address);
      object[,] formattedArrayFromRange = selectedRange.Value as object[,];
      object[,] unformattedArrayFromRange = selectedRange.Value2 as object[,];
      DataRow formattedRow;
      DataRow unformattedRow;

      int rowsCount = formattedArrayFromRange.GetUpperBound(0);
      int colsCount = formattedArrayFromRange.GetUpperBound(1);

      for (long colPos = 1; colPos <= colsCount; colPos++)
      {
        formattedExcelData.Columns.Add(String.Empty);
        unformattedExcelData.Columns.Add(String.Empty);
      }

      for (int rowPos = 1; rowPos <= rowsCount; rowPos++)
      {
        formattedRow = formattedExcelData.NewRow();
        unformattedRow = unformattedExcelData.NewRow();

        for (int colPos = 1; colPos <= colsCount; colPos++)
        {
          formattedRow[colPos] = formattedArrayFromRange[rowPos, colPos].ToString();
          unformattedRow[colPos] = unformattedArrayFromRange[rowPos, colPos].ToString();
        }

        formattedExcelData.Rows.Add(formattedRow);
        unformattedExcelData.Rows.Add(unformattedRow);
      }
    }

    public void SetSelectedExcelData2(Excel.Range selectedRange)
    {
      foreach (Excel.Range colsRange in selectedRange.Columns)
      {

      }
    }

    public void SetMultipleSelectedExcelData(Excel.Range selectedRange)
    {
      foreach (Excel.Range area in selectedRange)
      {
        SetSelectedExcelData(area);
      }
    }

    public void ResetForm(bool toNewTable, bool confirmChanges)
    {
      if (confirmChanges)
      {
        DialogResult dr = MessageBox.Show(Properties.Resources.CurrentChangesLostConfirmation, this.Text, MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
        if (dr == DialogResult.No)
          return;
      }

      columnMappingData.Clear();

      #region Resetting Values and Availability

      radExistingTable.Checked = !toNewTable;
      radNewTable.Checked = toNewTable;
      chkMakeSelectedTable.Checked = false;
      cmbExistingTable.Text = String.Empty;
      cmbExistingTable.Enabled = !toNewTable;
      txtNewTable.Text = String.Empty;
      txtNewTable.Enabled = toNewTable;
      cmbDBEngine.Text = String.Empty;
      cmbDBEngine.Enabled = toNewTable;
      cmbColumnName.Enabled = cmbColumnName.Visible = !toNewTable;
      cmbColumnName.Text = String.Empty;
      txtColumnName.Enabled = txtColumnName.Visible = toNewTable;
      txtColumnName.Text = String.Empty;
      cmbColumnType.Enabled = toNewTable;
      cmbColumnType.Text = String.Empty;
      numColumnTypeLength.Enabled = false;
      numColumnTypeLength.Value = 0;
      numColumnTypeDecimals.Enabled = false;
      numColumnTypeDecimals.Value = 0;
      chkColumnTypeUnsigned.Checked = chkColumnTypeUnsigned.Enabled = false;
      chkColumnTypeZeroFill.Checked = chkColumnTypeZeroFill.Enabled = false;
      chkColumnTypeBinary.Checked = chkColumnTypeBinary.Enabled = false;
      txtColumnDefaultValue.Enabled = toNewTable;
      txtColumnDefaultValue.Text = String.Empty;
      chkColumnNullable.Checked = false;
      chkColumnAutoIncrement.Checked = chkColumnAutoIncrement.Enabled = false;
      chkColumnPrimaryKey.Checked = false;
      chkColumnUniqueKey.Checked = false;
      chkUseFormattedValues.Checked = true;

      #endregion Resetting Values and Availability
    }

    private void cmbExistingSchema_SelectionChangeCommitted(object sender, EventArgs e)
    {
      localSchemaInfo.CurrentSchema = SelectedSchema;
    }

    private void cmbExistingTable_SelectionChangeCommitted(object sender, EventArgs e)
    {
      columnSchemaInfo.Clear();
      columnSchemaInfo = localSchemaInfo.GetTableSchemaInfo(SelectedTable);
      columnsBindingSource.DataSource = columnSchemaInfo;
    }

    private void cmbColumnName_SelectionChangeCommitted(object sender, EventArgs e)
    {

    }

    private void cmbColumnType_SelectionChangeCommitted(object sender, EventArgs e)
    {

    }

    private void chkColumnPrimaryKey_CheckedChanged(object sender, EventArgs e)
    {

    }

    private void chkColumnUniqueKey_CheckedChanged(object sender, EventArgs e)
    {

    }

    private void radExistingTable_CheckedChanged(object sender, EventArgs e)
    {
      ResetForm(false, true);
    }

    private void radNewTable_CheckedChanged(object sender, EventArgs e)
    {
      ResetForm(true, true);
    }

    private void btnMap_Click(object sender, EventArgs e)
    {

    }

    private void chkUseFormattedValues_CheckedChanged(object sender, EventArgs e)
    {
      grdPreviewData.DataSource = (chkUseFormattedValues.Checked ? formattedExcelData : unformattedExcelData);
      grdPreviewData.AutoResizeColumns();
    }

  }

}
