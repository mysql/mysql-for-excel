using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using MySQL.Utility;
using System.IO;
using Excel = Microsoft.Office.Interop.Excel;

namespace MySQL.ForExcel
{
  public partial class ImportTableViewForm : AutoStyleableBaseDialog
  {
    private MySqlWorkbenchConnection wbConnection;
    private DBObject importDBObject;
    private DataTable previewDataTable = null;
    private bool allColumnsSelected { get { return (grdPreviewData.SelectedColumns.Count == grdPreviewData.Columns.Count); } }

    public DataTable ImportDataTable = null;
    public bool ImportHeaders { get { return chkIncludeHeaders.Checked; } }
    public long TotalRowsCount { get; set; }

    public ImportTableViewForm(MySqlWorkbenchConnection wbConnection, DBObject importDBObject, string importToWorksheetName)
    {
      this.wbConnection = wbConnection;
      this.importDBObject = importDBObject;

      InitializeComponent();
      grdPreviewData.DataError += new DataGridViewDataErrorEventHandler(grdPreviewData_DataError);

      chkIncludeHeaders.Checked = true;
      chkLimitRows.Checked = false;
      lblTableNameMain.Text = String.Format("{0} Name:", importDBObject.Type.ToString());
      Text = String.Format("Import Data - {0}", importToWorksheetName);
      lblTableNameSub.Text = importDBObject.Name;
      fillPreviewGrid();
    }

    void grdPreviewData_DataError(object sender, DataGridViewDataErrorEventArgs e)
    {      
       if (grdPreviewData.Rows[e.RowIndex].Cells[e.ColumnIndex].ValueType == Type.GetType("System.Byte[]"))
       {
         try
         {
           var img = (byte[])(grdPreviewData.Rows[e.RowIndex].Cells[e.ColumnIndex]).Value;
           using (MemoryStream ms = new MemoryStream(img))
             Image.FromStream(ms);
         }
         catch (ArgumentException)
         {
         }
         catch (Exception ex)
         {
           MessageBox.Show("Loading Data Error " + ex.Message);
         }
       }           
    }

    private void fillPreviewGrid()
    {
      previewDataTable = Utilities.GetDataFromTableOrView(wbConnection, importDBObject, null, 0, 10);
      TotalRowsCount = Utilities.GetRowsCountFromTableOrView(wbConnection, importDBObject);
      lblRowsCountSub.Text = TotalRowsCount.ToString();
      grdPreviewData.DataSource = previewDataTable;
      foreach (DataGridViewColumn gridCol in grdPreviewData.Columns)
      {
        gridCol.SortMode = DataGridViewColumnSortMode.NotSortable;
      }
      grdPreviewData.SelectionMode = DataGridViewSelectionMode.FullColumnSelect;
      numFromRow.Maximum = TotalRowsCount;
      numRowsToReturn.Maximum = TotalRowsCount - numFromRow.Value + 1;
    }

    private void btnImport_Click(object sender, EventArgs e)
    {
      List<string> importColumns = new List<string>();
      List<DataGridViewColumn> selectedColumns = new List<DataGridViewColumn>();
      foreach (DataGridViewColumn selCol in grdPreviewData.SelectedColumns)
      {
        selectedColumns.Add(selCol);
      }
      if (selectedColumns.Count > 1)
        selectedColumns.Sort(delegate(DataGridViewColumn c1, DataGridViewColumn c2)
        {
          return c1.Index.CompareTo(c2.Index);
        });
      foreach (DataGridViewColumn selCol in selectedColumns)
      {
        importColumns.Add(selCol.HeaderText);
      }
      if (chkLimitRows.Checked)
        ImportDataTable = Utilities.GetDataFromTableOrView(wbConnection, importDBObject, importColumns, Convert.ToInt32(numFromRow.Value) - 1, Convert.ToInt32(numRowsToReturn.Value));
      else
        ImportDataTable = Utilities.GetDataFromTableOrView(wbConnection, importDBObject, importColumns);
    }

    private void chkLimitRows_CheckedChanged(object sender, EventArgs e)
    {
      numRowsToReturn.Enabled = numFromRow.Enabled = chkLimitRows.Checked;
    }

    private void grdPreviewData_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
    {
      grdPreviewData.SelectAll();
    }

    private void grdPreviewData_SelectionChanged(object sender, EventArgs e)
    {
      contextMenuForGrid.Items[0].Text = (allColumnsSelected ? "Select None" : "Select All");
      btnImport.Enabled = grdPreviewData.SelectedColumns.Count > 0;
    }

    private void numFromRow_ValueChanged(object sender, EventArgs e)
    {
      numRowsToReturn.Maximum = TotalRowsCount - numFromRow.Value + 1;
    }

    private void selectAllToolStripMenuItem_Click(object sender, EventArgs e)
    {
      if (allColumnsSelected)
        grdPreviewData.ClearSelection();
      else
        grdPreviewData.SelectAll();
    }

  }
}
