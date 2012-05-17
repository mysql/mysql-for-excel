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

namespace MySQL.ForExcel
{
  public partial class ExportDataToTableDialog : Form
  {
    private MySqlWorkbenchConnection wbConnection;
    private ToolStripButton columnPropsButton;
    private ToolStripButton tablePropsButton;
    private ExportDataHelper exportDataHelper;
    private MySQLTable exportTable { get { return exportDataHelper.ExportTable; } }
    private List<ColumnGuessData> headerRowColumnsGuessData { get { return exportDataHelper.HeaderRowColumnsGuessData; } }
    private List<ColumnGuessData> dataRowsColumnsGuessData { get { return exportDataHelper.DataRowsColumnsGuessData; } }
    private DataTable formattedExcelData { get { return exportDataHelper.FormattedExcelData; } }
    private DataTable unformattedExcelData { get { return exportDataHelper.UnformattedExcelData; } }

    public ExportDataToTableDialog(MySqlWorkbenchConnection wbConnection, Excel.Range exportDataRange)
    {
      this.wbConnection = wbConnection;

      InitializeComponent();

      exportDataHelper = new ExportDataHelper(wbConnection, exportDataRange, null);
      addPropertyButtonsToToolbar();
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
      if (chkFirstRowHeaders.Checked && grdPreviewData.Rows.Count < 2)
        return;
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
      bool success = exportDataHelper.CreateTableInDB();
      success = success && exportDataHelper.InsertData(chkFirstRowHeaders.Checked, chkUseFormatted.Checked);
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
      chkFirstRowHeaders_CheckedChanged(chkFirstRowHeaders, EventArgs.Empty);
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
