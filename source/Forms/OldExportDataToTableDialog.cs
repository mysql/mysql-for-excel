using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using MySQL.Utility;
using MySql.Data.MySqlClient;
using Excel = Microsoft.Office.Interop.Excel;

namespace MySQL.ExcelAddIn
{
  public partial class OldExportDataToTableDialog : Form
  {
    //private MySQLSchemaInfo localSchemaInfo;
    private MySqlWorkbenchConnection wbConnection;
    private TableSchemaInfo columnSchemaInfo;
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
    private bool allColumnsMapped
    {
      get { return (columnSchemaInfo != null && formattedExcelData != null ? columnSchemaInfo.Select("MappedColIdx > -1").Length == formattedExcelData.Columns.Count : false); }
    }
    private bool noColumnsMapped
    {
      get { return (columnSchemaInfo != null && formattedExcelData != null ? columnSchemaInfo.Select("MappedColIdx > -1").Length == 0 : true); }
    }

    public OldExportDataToTableDialog(MySqlWorkbenchConnection wbConnection, string toSchemaName, string toTableName, Excel.Range exportDataRange)
    {
      //localSchemaInfo = schemaInfo;
      this.wbConnection = wbConnection;
      InitializeComponent();
      this.Text = String.Format("Export Data to Table (Range: {0})", exportDataRange.Address);
      initializeDefaultData(toSchemaName, toTableName);
      fillDataTablesFromRange(exportDataRange);

      bool toNewTable = String.IsNullOrEmpty(toTableName);
      radExistingTable.Checked = !toNewTable;
      radNewTable.Checked = toNewTable;
    }

    private void initializeDefaultData(string selectedSchema, string toTableName)
    {
      // Databases
      if (cmbExistingSchema.Items.Count == 0)
      {
        DataTable databases = Utilities.GetSchemaCollection(wbConnection, "Databases", null);
        cmbExistingSchema.DataSource = databases;
        cmbExistingSchema.DisplayMember = cmbExistingSchema.ValueMember = "database_name";
        cmbExistingSchema.Text = selectedSchema;
      }

      // Tables
      DataTable tables = Utilities.GetSchemaCollection(wbConnection, "Tables", null, wbConnection.Schema);
      cmbExistingTable.DataSource = tables;
      cmbExistingTable.DisplayMember = cmbExistingTable.ValueMember = "TABLE_NAME";
      cmbExistingTable.Text = toTableName;

      // Engines
      DataTable engines = Utilities.GetSchemaCollection(wbConnection, "Engines", null);
      cmbDBEngine.DataSource = engines;
      cmbDBEngine.DisplayMember = cmbDBEngine.ValueMember = "ENGINE";
      
      //Data Types
      DataTable dataTypes = Utilities.GetSchemaCollection(wbConnection, "DataTypes");
      cmbColumnType.DataSource = dataTypes;
      cmbColumnType.DisplayMember = cmbColumnType.ValueMember = "TypeName";
            
      columnSchemaInfo = (!String.IsNullOrEmpty(toTableName) ? TableSchemaInfo.GetTableSchemaInfo(wbConnection, toTableName) : new TableSchemaInfo());
      setColumnsGroupText(columnSchemaInfo.Rows.Count);
      columnsBindingSource.DataSource = columnSchemaInfo;
    }

    private void fillDataTablesFromRange(Excel.Range selectedRange)
    {
      formattedExcelData = new DataTable();
      unformattedExcelData = new DataTable();

      object[,] formattedArrayFromRange = selectedRange.Value as object[,];
      object[,] unformattedArrayFromRange = selectedRange.Value2 as object[,];
      object valueFromArray = null;
      string colName;
      DataRow formattedRow;
      DataRow unformattedRow;

      int rowsCount = formattedArrayFromRange.GetUpperBound(0);
      int colsCount = formattedArrayFromRange.GetUpperBound(1);

      for (int colPos = 1; colPos <= colsCount; colPos++)
      {
        colName = String.Format("Unmapped{0}", colPos);
        formattedExcelData.Columns.Add(colName);
        unformattedExcelData.Columns.Add(colName);
      }

      for (int rowPos = 1; rowPos <= rowsCount; rowPos++)
      {
        formattedRow = formattedExcelData.NewRow();
        unformattedRow = unformattedExcelData.NewRow();

        for (int colPos = 1; colPos <= colsCount; colPos++)
        {
          valueFromArray = formattedArrayFromRange[rowPos, colPos];
          formattedRow[colPos - 1] = (valueFromArray != null ? valueFromArray.ToString() : String.Empty);
          valueFromArray = unformattedArrayFromRange[rowPos, colPos];
          unformattedRow[colPos - 1] = (valueFromArray != null ? valueFromArray.ToString() : String.Empty);
        }

        formattedExcelData.Rows.Add(formattedRow);
        unformattedExcelData.Rows.Add(unformattedRow);
      }
      grdPreviewData.DataSource = (chkUseFormattedValues.Checked ? formattedExcelData : unformattedExcelData);
      foreach (DataGridViewColumn gridCol in grdPreviewData.Columns)
      {
        setGridColumnHeaderColorAndText(gridCol, Color.Red, null);
        gridCol.SortMode = DataGridViewColumnSortMode.NotSortable;
      }
      grdPreviewData.SelectionMode = DataGridViewSelectionMode.FullColumnSelect;
    }

    private void fillDataTablesFromRange2(Excel.Range selectedRange)
    {
      foreach (Excel.Range colsRange in selectedRange.Columns)
      {

      }
    }

    public void fillDataTablesFromMultiRange(Excel.Range selectedRange)
    {
      foreach (Excel.Range area in selectedRange)
      {
        //SetSelectedExcelData(area);
      }
    }

    private void resetForm(bool toNewTable, bool confirmChanges)
    {
      if (confirmChanges)
      {
        DialogResult dr = MessageBox.Show(Properties.Resources.CurrentChangesLostConfirmation, this.Text, MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
        if (dr == DialogResult.No)
          return;
      }

      #region Resetting Values and Availability

      chkMakeSelectedTable.Checked = false;
      cmbExistingTable.Text = String.Empty;
      cmbExistingTable.Enabled = !toNewTable;
      txtNewTable.Text = String.Empty;
      txtNewTable.Enabled = toNewTable;
      cmbDBEngine.Text = (toNewTable ? "InnoDB" : String.Empty);
      cmbDBEngine.Enabled = toNewTable;
      setColumnMappingAvailability(toNewTable);
      cmbColumnName.Enabled = cmbColumnName.Visible = !toNewTable;
      cmbColumnName.Text = String.Empty;
      txtColumnName.Visible = toNewTable;
      txtColumnName.Text = String.Empty;
      cmbColumnType.Text = String.Empty;
      numColumnTypeLength.Value = 0;
      numColumnTypeDecimals.Value = 0;
      chkColumnTypeUnsigned.Checked = false;
      chkColumnTypeZeroFill.Checked = false;
      chkColumnTypeBinary.Checked = false;
      txtColumnDefaultValue.Text = String.Empty;
      chkColumnNullable.Checked = false;
      chkColumnAutoIncrement.Checked = false;
      chkColumnPrimaryKey.Checked = false;
      chkColumnUniqueKey.Checked = false;
      chkUseFormattedValues.Checked = true;
      chkUseFormattedValues.Enabled = true;
      chkFirstRowHeaders.Checked = false;
      chkFirstRowHeaders.Enabled = true;
      foreach (DataGridViewColumn gridCol in grdPreviewData.Columns)
      {
        setGridColumnHeaderColorAndText(gridCol, Color.Red, gridCol.DataPropertyName);
      }

      #endregion Resetting Values and Availability

      if (toNewTable && formattedExcelData.Columns.Count > 0)
      {
        if (columnSchemaInfo.Rows.Count > 0)
          columnSchemaInfo.Clear();
        string colName;
        for (int colIdx = 0; colIdx <= formattedExcelData.Columns.Count; colIdx++)
        {
          DataRow dr = columnSchemaInfo.NewRow();
          colName = String.Format("Column{0}", colIdx);
          if (colIdx > 0)
          {
            dr["Name"] = colName;
            dr["HeaderName"] = formattedExcelData.Rows[0][colIdx - 1].ToString().Replace(" ", "_");
            dr["GivenName"] = colName;
          }
          else
          {
            dr["Name"] = String.Empty;
            dr["HeaderName"] = String.Empty;
            dr["GivenName"] = String.Empty; 
          }
          dr["MappedColIdx"] = colIdx - 1;
          columnSchemaInfo.Rows.Add(dr);
        }
        setColumnsGroupText(columnSchemaInfo.Rows.Count);
        lblMappedColumns.Text = String.Format("Mapped Columns: {0}", formattedExcelData.Columns.Count);
      }
    }

    private void setColumnMappingAvailability(bool enabled)
    {
      cmbColumnName.Enabled = enabled;
      txtColumnName.Enabled = enabled;
      cmbColumnType.Enabled = enabled;
      numColumnTypeLength.Enabled = enabled;
      numColumnTypeDecimals.Enabled = enabled;
      chkColumnTypeUnsigned.Enabled = enabled;
      chkColumnTypeZeroFill.Enabled = enabled;
      chkColumnTypeBinary.Enabled = enabled;
      txtColumnDefaultValue.Enabled = enabled;
      chkColumnNullable.Enabled = enabled;
      chkColumnAutoIncrement.Enabled = enabled;
      chkColumnPrimaryKey.Enabled = enabled;
      chkColumnUniqueKey.Enabled = enabled;
      btnMap.Enabled = enabled;
      btnUnmap.Enabled = enabled;
    }

    private void setGridColumnHeaderColorAndText(DataGridViewColumn gridColumn, Color newColor, string headerText)
    {
      if (headerText != null)
        gridColumn.HeaderText = headerText;
      DataGridViewCellStyle newStyle = new DataGridViewCellStyle();
      newStyle.ForeColor = newColor;
      newStyle.BackColor = newColor;
      gridColumn.HeaderCell.Style = newStyle;
    }

    private void setColumnsGroupText(int columnCount)
    {
      grpColumnMapping.Text = String.Format("Column Mapping ({0} columns)", columnCount);
    }

    private void setMappingButtonsState(int currRowMappedIndex)
    {
      bool somethingSelected = grdPreviewData.SelectedColumns.Count > 0;
      btnMap.Enabled = (somethingSelected && currRowMappedIndex < 0);
      btnUnmap.Enabled = (somethingSelected && currRowMappedIndex >= 0);
    }

    private void cmbExistingSchema_SelectionChangeCommitted(object sender, EventArgs e)
    {
      initializeDefaultData(cmbExistingSchema.Text, String.Empty);
      //localSchemaInfo.CurrentSchema = SelectedSchema;
    }

    private void cmbExistingTable_SelectionChangeCommitted(object sender, EventArgs e)
    {
      columnSchemaInfo.Clear();
      if (cmbExistingTable.Text.Length > 0)
        //columnSchemaInfo = localSchemaInfo.GetTableSchemaInfo(SelectedTable);
        columnSchemaInfo = TableSchemaInfo.GetTableSchemaInfo(wbConnection, SelectedTable);
      setColumnsGroupText(columnSchemaInfo.Rows.Count);
      columnsBindingSource.DataSource = columnSchemaInfo;
      columnsBindingSource.ResetBindings(false);
    }

    private void cmbColumnType_SelectedIndexChanged(object sender, EventArgs e)
    {
      string colType = cmbColumnType.Text.ToLowerInvariant();
      bool isDecimal = colType == "real" || colType == "double" || colType == "float" || colType == "decimal" || colType == "numeric";
      bool isNum = isDecimal || colType.Contains("int");
      numColumnTypeLength.Enabled = isNum || colType == "bit" || colType.Contains("char") || colType.Contains("binary");
      numColumnTypeDecimals.Enabled = isDecimal;
      chkColumnTypeUnsigned.Enabled = isNum;
      chkColumnTypeZeroFill.Enabled = isNum;
      chkColumnTypeBinary.Enabled = colType.Contains("text");
      chkColumnAutoIncrement.Enabled = isNum;
    }

    private void chkColumnPrimaryKey_CheckedChanged(object sender, EventArgs e)
    {
      if (radExistingTable.Checked)
        return;
      chkColumnUniqueKey.Enabled = !chkColumnPrimaryKey.Checked;
      if (chkColumnPrimaryKey.Checked && chkColumnUniqueKey.Checked)
        chkColumnUniqueKey.Checked = false;
    }

    private void chkColumnUniqueKey_CheckedChanged(object sender, EventArgs e)
    {
      if (radExistingTable.Checked)
        return;
      chkColumnPrimaryKey.Enabled = !chkColumnUniqueKey.Checked;
    }

    private void radAnyTable_CheckedChanged(object sender, EventArgs e)
    {
      RadioButton senderRadioButton = sender as RadioButton;
      if (senderRadioButton.Checked)
        resetForm(senderRadioButton.Name.Equals("radNewTable"), !noColumnsMapped);
    }

    private void btnMap_Click(object sender, EventArgs e)
    {
      if (grdPreviewData.SelectedColumns.Count == 0)
        return;

      int previewDataSelectedColIdx = grdPreviewData.SelectedColumns[0].DisplayIndex;
      string selectedColumnName = String.Empty;

      DataRowView currentRow = columnsBindingSource.Current as DataRowView;
      selectedColumnName = currentRow["Name"].ToString();
      currentRow.BeginEdit();
      currentRow["MappedColIdx"] = previewDataSelectedColIdx;
      currentRow.EndEdit();
      columnSchemaInfo.AcceptChanges();

      setGridColumnHeaderColorAndText(grdPreviewData.SelectedColumns[0], Color.Green, selectedColumnName);
      setMappingButtonsState(previewDataSelectedColIdx);
      lblMappedColumns.Text = String.Format("Mapped Columns: {0}", columnSchemaInfo.Select("MappedColIdx >= 0").Length);
    }

    private void btnUnmap_Click(object sender, EventArgs e)
    {
      if (grdPreviewData.SelectedColumns.Count == 0)
        return;
      int previewDataSelectedColIdx = grdPreviewData.SelectedColumns[0].DisplayIndex;
      DataRow[] resultSet = columnSchemaInfo.Select(String.Format("MappedColIdx = {0}", previewDataSelectedColIdx));
      int mappedColIdx = (resultSet.Length > 0 ? Convert.ToInt32(resultSet[0]["MappedColIdx"]) : -1);
      setGridColumnHeaderColorAndText(grdPreviewData.Columns[mappedColIdx], Color.Red, grdPreviewData.Columns[mappedColIdx].DataPropertyName);
      resultSet[0].BeginEdit();
      resultSet[0]["MappedColIdx"] = -1;
      resultSet[0].EndEdit();
      resultSet[0].AcceptChanges();
      setMappingButtonsState(previewDataSelectedColIdx);
      lblMappedColumns.Text = String.Format("Mapped Columns: {0}", columnSchemaInfo.Select("MappedColIdx >= 0").Length);
    }

    private void grdPreviewData_SelectionChanged(object sender, EventArgs e)
    {
      bool somethingSelected = grdPreviewData.SelectedColumns.Count > 0;
      int selIndex = (somethingSelected ? grdPreviewData.SelectedColumns[0].DisplayIndex : -1);

      DataRow[] resultSet = columnSchemaInfo.Select(String.Format("MappedColIdx = {0}", selIndex));
      selIndex = (somethingSelected && resultSet.Length > 0 ? columnSchemaInfo.Rows.IndexOf(resultSet[0]) : -1);

      columnsBindingSource.Position = selIndex;
      setColumnMappingAvailability(somethingSelected);
      if (somethingSelected)
        setMappingButtonsState(selIndex);
    }

    private void chkUseFormattedValues_CheckedChanged(object sender, EventArgs e)
    {
      DataTable sourceTable = (chkUseFormattedValues.Checked ? formattedExcelData : unformattedExcelData);
      if (grdPreviewData.DataSource != sourceTable)
      {
        grdPreviewData.DataSource = sourceTable;
        grdPreviewData.AutoResizeColumns();
      }
    }

    private void btnExport_Click(object sender, EventArgs e)
    {
      if (!allColumnsMapped)
      {
        DialogResult dr = Utilities.ShowWarningBox(Properties.Resources.columnMappingIncomplete);
        if (dr == DialogResult.No)
          return;
      }

      bool success = false;

      if (radNewTable.Checked)
        success = exportDataToNewTable();
      else if (radExistingTable.Checked)
        success = exportDataToExistingTable();

      if (success)
      {
        this.DialogResult = DialogResult.OK;
        this.Close();
      }
    }

    private void cmbColumnName_SelectedIndexChanged(object sender, EventArgs e)
    {
      columnsBindingSource.Position = cmbColumnName.SelectedIndex;
    }

    private bool exportDataToExistingTable()
    {
      bool success = false;

      success = InsertData(cmbExistingTable.Text, (chkUseFormattedValues.Checked ? formattedExcelData : unformattedExcelData), chkFirstRowHeaders.Checked, columnSchemaInfo);

      return success;
    }

    private bool exportDataToNewTable()
    {
      bool success = false;

      success = CreateTable(txtNewTable.Text, cmbDBEngine.Text, columnSchemaInfo);
      success = success && InsertData(txtNewTable.Text, (chkUseFormattedValues.Checked ? formattedExcelData : unformattedExcelData), chkFirstRowHeaders.Checked, columnSchemaInfo);

      return success;
    }

    public bool CreateTable(string newTableName, string dbEngine, TableSchemaInfo schemaInfo)
    {
      bool success = false;
      string connectionString = Utilities.GetConnectionString(wbConnection);

      StringBuilder queryString = new StringBuilder();
      queryString.AppendFormat("USE {0}; CREATE TABLE", wbConnection.Schema);
      queryString.AppendFormat(" {0} (", newTableName);
      DataRow[] resultSet = schemaInfo.Select("MappedColIdx >= 0", "MappedColIdx ASC");

      foreach (DataRow dr in resultSet)
      {
        queryString.AppendFormat("{0} {1}, ",
                                 dr["Name"].ToString(),
                                 schemaInfo.GetColumnDefinition(dr));
      }
      if (resultSet.Length > 0)
        queryString.Remove(queryString.Length - 2, 2);
      queryString.AppendFormat(") ENGINE={0};", dbEngine);

      try
      {
        using (MySqlConnection conn = new MySqlConnection(connectionString))
        {
          conn.Open();

          MySqlCommand cmd = new MySqlCommand(queryString.ToString(), conn);
          cmd.ExecuteNonQuery();
          success = true;
        }
      }
      catch (Exception ex)
      {
        System.Diagnostics.Debug.WriteLine(ex.Message);
      }

      return success;
    }

    public bool InsertData(string toTableName, DataTable insertingData, bool firstRowHeader, TableSchemaInfo schemaInfo)
    {
      bool success = false;

      string connectionString = Utilities.GetConnectionString(wbConnection);

      StringBuilder queryString = new StringBuilder();
      queryString.AppendFormat("USE {0}; INSERT INTO", wbConnection.Schema);
      queryString.AppendFormat(" {0} (", toTableName);
      DataRow[] resultSet = schemaInfo.Select("MappedColIdx >= 0", "MappedColIdx ASC");
      List<int> mappedColumnIndexes = new List<int>();
      List<string> mappedColumnTypes = new List<string>();
      int rowIdx = 0;

      foreach (DataRow dr in resultSet)
      {
        mappedColumnIndexes.Add(Convert.ToInt32(dr["MappedColIdx"]));
        mappedColumnTypes.Add(dr["Type"].ToString().ToLowerInvariant());
        queryString.AppendFormat("{0},", dr["Name"].ToString());
      }
      if (resultSet.Length > 0)
        queryString.Remove(queryString.Length - 1, 1);
      queryString.Append(") VALUES ");

      foreach (DataRow dr in insertingData.Rows)
      {
        if (firstRowHeader && rowIdx++ == 0)
          continue;
        queryString.Append("(");
        for (int colIdx = 0; colIdx < mappedColumnIndexes.Count; colIdx++)
        {
          queryString.AppendFormat("{0}{1}{0},",
                                   (mappedColumnTypes[colIdx].Contains("char") || mappedColumnTypes[colIdx].Contains("text") || mappedColumnTypes[colIdx].Contains("date") ? "'" : String.Empty),
                                   dr[mappedColumnIndexes[colIdx]].ToString());
        }
        if (mappedColumnIndexes.Count > 0)
          queryString.Remove(queryString.Length - 1, 1);
        queryString.Append("),");
      }
      if (insertingData.Rows.Count > 0)
        queryString.Remove(queryString.Length - 1, 1);
      queryString.Append(";");

      try
      {
        using (MySqlConnection conn = new MySqlConnection(connectionString))
        {
          conn.Open();

          MySqlCommand cmd = new MySqlCommand(queryString.ToString(), conn);
          cmd.ExecuteNonQuery();
          success = true;
        }
      }
      catch (Exception ex)
      {
        System.Diagnostics.Debug.WriteLine(ex.Message);
      }

      return success;
    }

    private void txtColumnName_Validating(object sender, CancelEventArgs e)
    {
      if (radNewTable.Checked && grdPreviewData.SelectedColumns.Count > 0)
        grdPreviewData.SelectedColumns[0].HeaderText = txtColumnName.Text;
    }

    private void chkFirstRowHeaders_CheckedChanged(object sender, EventArgs e)
    {
      if (columnSchemaInfo == null)
        return;
      bool useHeaderName = chkFirstRowHeaders.Checked;
      if (radNewTable.Checked)
      {
        for (int colIdx = 1; colIdx <= grdPreviewData.Columns.Count; colIdx++)
        {
          DataRow dr = columnSchemaInfo.Rows[colIdx];
          DataGridViewColumn gridCol = grdPreviewData.Columns[colIdx - 1];
          string displayName = (useHeaderName ? dr["HeaderName"].ToString() : dr["GivenName"].ToString());
          dr["Name"] = displayName;
          setGridColumnHeaderColorAndText(gridCol, gridCol.HeaderCell.Style.BackColor, displayName);
        }
      }
      grdPreviewData.CurrentCell = null;
      grdPreviewData.Rows[0].Visible = !useHeaderName;
      grdPreviewData.FirstDisplayedScrollingRowIndex = (useHeaderName ? 1 :0);
    }

    private void txtNewTable_Validating(object sender, CancelEventArgs e)
    {
      if (txtNewTable.Text.Length > 0)
      {
        if ((cmbExistingTable.DataSource as DataTable).Select(String.Format("TABLE_NAME = '{0}'", txtNewTable.Text)).Length > 0)
        {
          Utilities.ShowErrorBox("A table with that name already exists in the database.");
          e.Cancel = true;
        }
      }
      setColumnMappingAvailability(txtNewTable.Text.Length > 0);
    }

  }

}
