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
using System.Reflection;
using System.Globalization;

namespace MySQL.ExcelAddIn
{
  public partial class ExportDataToTableDialog : Form
  {
    private MySqlWorkbenchConnection wbConnection;
    private DataTable formattedExcelData;
    private DataTable unformattedExcelData;
    private MySQLTable exportTable;
    ToolStripButton columnPropsButton;
    ToolStripButton tablePropsButton;
    List<ColumnGuessData> headerRowColumnsGuessData;
    List<ColumnGuessData> dataRowsColumnsGuessData;

    public ExportDataToTableDialog(MySqlWorkbenchConnection wbConnection, Excel.Range exportDataRange)
    {
      this.wbConnection = wbConnection;

      InitializeComponent();

      addPropertyButtonsToToolbar();
      createMySQLTable();
      fillDataTablesFromRange(exportDataRange);
      initializeGridCombos();

      Text = String.Format("Export Data to Table (Range: {0})", exportDataRange.Address);
      txtTableName.DataBindings.Add(new Binding("Text", exportTable, "Name"));
      columnBindingSource.DataSource = exportTable.Columns;
      grdColumnProperties.DataSource = columnBindingSource;
      chkUseFormatted.Checked = true;
      chkFirstRowHeaders_CheckedChanged(chkFirstRowHeaders, EventArgs.Empty);
    }

    private void initializeGridCombos()
    {
      DataGridViewComboBoxColumn dataTypeCombo = grdColumnProperties.Columns["dataTypeDataGridViewComboBoxColumn"] as DataGridViewComboBoxColumn;
      dataTypeCombo.DataSource = Utilities.GetDataTypes();
      DataGridViewComboBoxColumn charSetCombo = grdColumnProperties.Columns["characterSetDataGridViewComboBoxColumn"] as DataGridViewComboBoxColumn;
      charSetCombo.DataSource = Utilities.GetSchemaCollection(wbConnection, "Charsets");
      charSetCombo.DisplayMember = "Charset";
      DataGridViewComboBoxColumn collationCombo = grdColumnProperties.Columns["collationDataGridViewComboBoxColumn"] as DataGridViewComboBoxColumn;
      collationCombo.DataSource = Utilities.GetSchemaCollection(wbConnection, "Collations");
      collationCombo.DisplayMember = "Collation";
    }

    private void resetCollationGridCombo(string charset)
    {
      DataGridViewComboBoxColumn collationCombo = grdColumnProperties.Columns["collationDataGridViewComboBoxColumn"] as DataGridViewComboBoxColumn;
      collationCombo.DataSource = Utilities.GetSchemaCollection(wbConnection, "Collations", charset);
      collationCombo.DataPropertyName = "Collation";
    }

    private void createMySQLTable()
    {
      exportTable = new MySQLTable(wbConnection, null, null);
      exportTable.Engine = "InnoDB";

      int tableCount = 1;
      string tableName = String.Empty ;
      bool tableExists = true;
      while (tableExists)
      {
        tableName = String.Format("Table{0}", tableCount++);
        tableExists = Utilities.TableExistsInSchema(wbConnection, wbConnection.Schema, tableName);
      }

      exportTable.Name = tableName;
      exportTable.CharacterSet = "latin1";
      exportTable.Collation = "latin1_swedish_ci";
    }

    private void addPropertyButtonsToToolbar()
    {
      ToolStrip propsToolStrip = (ToolStrip)typeof(PropertyGrid).InvokeMember("toolStrip",
                                                                            BindingFlags.GetField | BindingFlags.NonPublic | BindingFlags.Instance,
                                                                            null,
                                                                            columnPropertiesGrid,
                                                                            null);
      int currentImagesCount = propsToolStrip.ImageList.Images.Count;
      propsToolStrip.ImageList.Images.Add(Properties.Resources.db_Table_16x16);
      propsToolStrip.ImageList.Images.Add(Properties.Resources.db_Column_16x16);

      tablePropsButton = new ToolStripButton();
      tablePropsButton.CheckOnClick = true;
      tablePropsButton.Click += new EventHandler(tablePropsButton_Click);
      tablePropsButton.Name = "btnTableProps";
      tablePropsButton.ImageIndex = currentImagesCount;
      tablePropsButton.ToolTipText = "Table Properties";

      columnPropsButton = new ToolStripButton();
      columnPropsButton.CheckOnClick = true;
      columnPropsButton.Click += new EventHandler(columnPropsButton_Click);
      columnPropsButton.Name = "btnColumnProps";
      columnPropsButton.ImageIndex = currentImagesCount + 1;
      columnPropsButton.ToolTipText = "Column Properties";

      tablePropsButton.Checked = true;

      propsToolStrip.Items.Add(new ToolStripSeparator());
      propsToolStrip.Items.Add(tablePropsButton);
      propsToolStrip.Items.Add(columnPropsButton);
    }

    void columnPropsButton_Click(object sender, EventArgs e)
    {
      columnPropsButton.Checked = true;
      tablePropsButton.Checked = false;

      columnPropertiesGrid.SelectedObject = columnBindingSource.Current as MySQLColumn;
    }

    void tablePropsButton_Click(object sender, EventArgs e)
    {
      tablePropsButton.Checked = true;
      columnPropsButton.Checked = false;

      columnPropertiesGrid.SelectedObject = exportTable;
    }

    private void fillDataTablesFromRange(Excel.Range selectedRange)
    {
      formattedExcelData = new DataTable();
      unformattedExcelData = new DataTable();

      object[,] formattedArrayFromRange = selectedRange.Value as object[,];
      object[,] unformattedArrayFromRange = selectedRange.Value2 as object[,];
      object valueFromArray = null;
      DataRow formattedRow;
      DataRow unformattedRow;
      MySQLColumn newColumn;

      int rowsCount = formattedArrayFromRange.GetUpperBound(0);
      int colsCount = formattedArrayFromRange.GetUpperBound(1);

      for (int colPos = 1; colPos <= colsCount; colPos++)
      {
        newColumn = new MySQLColumn(null, exportTable);
        exportTable.Columns.Add(newColumn);
        formattedExcelData.Columns.Add();
        unformattedExcelData.Columns.Add();
      }

      for (int rowPos = 1; rowPos <= rowsCount; rowPos++)
      {
        bool wholeRowNull = true;
        formattedRow = formattedExcelData.NewRow();
        unformattedRow = unformattedExcelData.NewRow();

        for (int colPos = 1; colPos <= colsCount; colPos++)
        {
          valueFromArray = formattedArrayFromRange[rowPos, colPos];
          wholeRowNull = wholeRowNull && valueFromArray == null;
          formattedRow[colPos - 1] = (valueFromArray != null ? valueFromArray.ToString() : String.Empty);
          valueFromArray = unformattedArrayFromRange[rowPos, colPos];
          unformattedRow[colPos - 1] = (valueFromArray != null ? valueFromArray.ToString() : String.Empty);
        }

        if (!wholeRowNull)
        {
          formattedExcelData.Rows.Add(formattedRow);
          unformattedExcelData.Rows.Add(unformattedRow);
        }
      }

      guessDataTypesFromData(formattedArrayFromRange);
    }

    private void guessDataTypesFromData(object[,] formattedArrayFromRange)
    {
      int rowsCount = formattedArrayFromRange.GetUpperBound(0);
      int colsCount = formattedArrayFromRange.GetUpperBound(1);
      headerRowColumnsGuessData = new List<ColumnGuessData>(colsCount);
      dataRowsColumnsGuessData = new List<ColumnGuessData>(colsCount);

      object valueFromArray = null;
      string strValue = String.Empty;
      string proposedType = String.Empty;
      string previousType = String.Empty;
      string headerType = String.Empty;
      bool typesConsistent = true;
      int maxStrValue = 0;
      string nameFromHeader;
      string nameGeneric;
      CultureInfo cultureForDates = new CultureInfo("en-US");
      string dateFormat = "yyyy-MM-dd HH:mm:ss";

      for (int colPos = 1; colPos <= colsCount; colPos++)
      {
        headerRowColumnsGuessData.Add(new ColumnGuessData());
        dataRowsColumnsGuessData.Add(new ColumnGuessData());

        for (int rowPos = 1; rowPos <= rowsCount; rowPos++)
        {
          valueFromArray = formattedArrayFromRange[rowPos, colPos];
          if (valueFromArray == null)
            continue;
          strValue = valueFromArray.ToString();
          proposedType = Utilities.GetMySQLDataType(valueFromArray);
          if (proposedType == "datetime" && valueFromArray is DateTime)
          {
            DateTime dtValue = (DateTime)valueFromArray;
            formattedExcelData.Rows[rowPos - 1][colPos - 1] = dtValue.ToString(dateFormat);
          }
          maxStrValue = Math.Max(strValue.Length, maxStrValue);
          if (rowPos == 1)
            headerType = proposedType;
          else
          {
            typesConsistent = typesConsistent && (rowPos >  2 ? previousType == proposedType : true);
            previousType = proposedType;
          }
        }

        nameFromHeader = (formattedArrayFromRange[1, colPos] != null ? formattedArrayFromRange[1, colPos].ToString().Replace(" ", "_").Replace("(", String.Empty).Replace(")", String.Empty) : String.Empty);
        nameGeneric = String.Format("Column{0}", colPos);
        if (nameFromHeader.Length == 0)
          nameFromHeader = nameGeneric;
        int charLen = (maxStrValue + (10 - maxStrValue % 10));
        headerType = (headerType.Length == 0 ? previousType : (headerType == "varchar" ? (charLen > 65535 ? "text" : "varchar") : headerType));
        previousType = (previousType.Length == 0 ? "varchar" : (previousType == "varchar" ? (charLen > 65535 ? "text" : "varchar") : previousType));
        headerRowColumnsGuessData[colPos - 1].ColumnName = nameFromHeader;
        headerRowColumnsGuessData[colPos - 1].MySQLType = headerType;
        headerRowColumnsGuessData[colPos - 1].StrLen = charLen;
        dataRowsColumnsGuessData[colPos - 1].ColumnName = nameGeneric;
        dataRowsColumnsGuessData[colPos - 1].MySQLType = previousType;
        dataRowsColumnsGuessData[colPos - 1].StrLen = charLen;
      }
    }

    private void refreshColumnsNameAndType()
    {
      ColumnGuessData headerColData;
      ColumnGuessData otherColData;

      for (int colIdx = 0; colIdx < exportTable.Columns.Count; colIdx++)
      {
        headerColData = headerRowColumnsGuessData[colIdx];
        otherColData = dataRowsColumnsGuessData[colIdx];
        if (exportTable.Columns[colIdx].DataType != null && exportTable.Columns[colIdx].DataType != headerColData.MySQLType && exportTable.Columns[colIdx].DataType != otherColData.MySQLType)
          continue;
        if (chkFirstRowHeaders.Checked)
        {
          exportTable.Columns[colIdx].ColumnName = headerColData.ColumnName;
          exportTable.Columns[colIdx].AssignDataType(otherColData.MySQLType, otherColData.StrLen);
        }
        else
        {
          exportTable.Columns[colIdx].ColumnName = otherColData.ColumnName;
          exportTable.Columns[colIdx].AssignDataType((headerColData.MySQLType == otherColData.MySQLType ? otherColData.MySQLType : "varchar"), otherColData.StrLen);
        }
      }
      columnBindingSource.ResetBindings(false);
    }

    private void refreshPreviewGridWithHeaderSelection()
    {

    }

    private bool createTable()
    {
      bool success = false;
      string connectionString = Utilities.GetConnectionString(wbConnection);
      string queryString = exportTable.GetSQL();

      try
      {
        using (MySqlConnection conn = new MySqlConnection(connectionString))
        {
          conn.Open();

          MySqlCommand cmd = new MySqlCommand(queryString, conn);
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

    private bool insertData()
    {
      bool success = false;

      string connectionString = Utilities.GetConnectionString(wbConnection);
      StringBuilder queryString = new StringBuilder();
      int rowIdx = 0;
      int exportColsCount = exportTable.Columns.Count;
      List<bool> columnsRequireQuotes = new List<bool>();
      bool firstRowHeader = chkFirstRowHeaders.Checked;
      DataTable insertingData = (chkUseFormatted.Checked ? formattedExcelData : unformattedExcelData);

      queryString.AppendFormat("USE {0}; INSERT INTO", wbConnection.Schema);
      queryString.AppendFormat(" {0} (", exportTable.Name);

      foreach (MySQLColumn column in exportTable.Columns)
      {
        queryString.AppendFormat("{0},", column.ColumnName);
        columnsRequireQuotes.Add(column.IsCharOrText || column.IsDate);
      }
      if (exportColsCount > 0)
        queryString.Remove(queryString.Length - 1, 1);
      queryString.Append(") VALUES ");

      foreach (DataRow dr in insertingData.Rows)
      {
        if (firstRowHeader && rowIdx++ == 0)
          continue;
        queryString.Append("(");
        for (int colIdx = 0; colIdx < exportTable.Columns.Count; colIdx++)
        {
          queryString.AppendFormat("{0}{1}{0},",
                                   (columnsRequireQuotes[colIdx] ? "'" : String.Empty),
                                   dr[colIdx].ToString());
        }
        if (exportColsCount > 0)
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

    private void chkFirstRowHeaders_CheckedChanged(object sender, EventArgs e)
    {
      if (exportTable == null || exportTable.Columns.Count == 0)
        return;
      refreshColumnsNameAndType();
      for (int colIdx = 0; colIdx < grdPreviewData.Columns.Count; colIdx++)
      {
        MySQLColumn mysqlCol = exportTable.Columns[colIdx];
        DataGridViewColumn gridCol = grdPreviewData.Columns[colIdx];
        gridCol.HeaderText = mysqlCol.ColumnName;
      }
      grdPreviewData.CurrentCell = null;
      grdPreviewData.Rows[0].Visible = !chkFirstRowHeaders.Checked;
      grdPreviewData.FirstDisplayedScrollingRowIndex = (chkFirstRowHeaders.Checked ? 1 : 0);
    }

    private void grdPreviewData_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
    {
      grdPreviewData.ClearSelection();
    }

    private void columnBindingSource_CurrentChanged(object sender, EventArgs e)
    {
      columnPropertiesGrid.SelectedObject = columnBindingSource.Current as MySQLColumn;
    }

    private void grdPreviewData_SelectionChanged(object sender, EventArgs e)
    {
      if (grdPreviewData.SelectedColumns.Count > 0)
      {
        btnRemove.Enabled = true;
        columnBindingSource.Position = grdPreviewData.SelectedColumns[0].DisplayIndex;
        if (!(columnPropertiesGrid.SelectedObject is MySQLColumn))
          columnPropsButton_Click(columnPropsButton, EventArgs.Empty);
      }
      else
      {
        btnRemove.Enabled = false;
        tablePropsButton_Click(tablePropsButton, EventArgs.Empty);
      }
    }

    private void btnExport_Click(object sender, EventArgs e)
    {
      bool success = createTable();
      success = success && insertData();
      if (success)
      {
        DialogResult = DialogResult.OK;
        Close();
      }
    }

    private void btnRemove_Click(object sender, EventArgs e)
    {
      if (grdPreviewData.SelectedColumns.Count > 0)
      {
        DialogResult dr = Utilities.ShowWarningBox(Properties.Resources.RemoveColumnConfirmation);
        if (dr != DialogResult.Yes)
          return;

        int removeColdIndex = grdPreviewData.SelectedColumns[0].DisplayIndex;
        columnBindingSource.RemoveCurrent();
        formattedExcelData.Columns.RemoveAt(removeColdIndex);
        unformattedExcelData.Columns.RemoveAt(removeColdIndex);
        headerRowColumnsGuessData.RemoveAt(removeColdIndex);
        dataRowsColumnsGuessData.RemoveAt(removeColdIndex);
        grdPreviewData.Refresh();
        //grdPreviewData.Columns.Remove(grdPreviewData.SelectedColumns[0]);
        grdPreviewData.ClearSelection();
      }
    }

    private void chkUseFormatted_CheckedChanged(object sender, EventArgs e)
    {
      grdPreviewData.DataSource = (chkUseFormatted.Checked ? formattedExcelData : unformattedExcelData);
      foreach (DataGridViewColumn gridCol in grdPreviewData.Columns)
      {
        gridCol.SortMode = DataGridViewColumnSortMode.NotSortable;
      }
      grdPreviewData.SelectionMode = DataGridViewSelectionMode.FullColumnSelect;
    }

    private void grdColumnProperties_CurrentCellDirtyStateChanged(object sender, EventArgs e)
    {
      if (grdColumnProperties.IsCurrentCellDirty && grdColumnProperties.CurrentCell.OwningColumn is DataGridViewComboBoxColumn)
        grdColumnProperties.CommitEdit(DataGridViewDataErrorContexts.Commit);
    }

    private void grdColumnProperties_CellValueChanged(object sender, DataGridViewCellEventArgs e)
    {
      if (grdColumnProperties.CurrentCell == null)
        return;
      if (grdColumnProperties.CurrentCell.OwningColumn is DataGridViewComboBoxColumn && grdColumnProperties.CurrentCell.OwningColumn.Name == "characterSetDataGridViewComboBoxColumn")
      {
        grdColumnProperties[e.ColumnIndex + 1, e.RowIndex].Value = String.Empty;
        resetCollationGridCombo(grdColumnProperties.CurrentCell.Value.ToString());
      }
    }
  }

  internal class ColumnGuessData
  {
    public string ColumnName = String.Empty;
    public string MySQLType = "varchar";
    public int StrLen = 10;
    public bool MySQLTypeConsistentInAllRows = true;
  };
}
