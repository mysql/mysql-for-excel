using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using MySQL.Utility;

namespace MySQL.ForExcel
{
  public partial class EditTableDialog : Form
  {
    private MySqlWorkbenchConnection wbConnection;
    private DBObject importDBObject;
    private DataTable previewDataTable = null;
    private bool allColumnsSelected { get { return (grdPreview.SelectedColumns.Count == grdPreview.Columns.Count); } }
    public DataTable ImportDataTable = null;
    public bool ImportHeaders { get { return chkIncludeHeaders.Checked; } }

    public EditTableDialog(MySqlWorkbenchConnection wbConnection, DBObject importDBObject)
    {
      this.wbConnection = wbConnection;
      this.importDBObject = importDBObject;

      InitializeComponent();

      chkIncludeHeaders.Checked = true;
      chkLimitRows.Checked = false;
      lblFrom.Text = String.Format("From {0}: {1}", importDBObject.Type.ToString(), importDBObject.Name);

      fillPreviewGrid();
    }

    private void fillPreviewGrid()
    {
      previewDataTable = Utilities.GetDataFromTableOrView(wbConnection, importDBObject, null, 0, 10);
      long totalRowsCount = Utilities.GetRowsCountFromTableOrView(wbConnection, importDBObject);
      lblRowsCount.Text = String.Format("Total Rows Count: {0}", totalRowsCount);
      grdPreview.DataSource = previewDataTable;
      foreach (DataGridViewColumn gridCol in grdPreview.Columns)
      {
        gridCol.SortMode = DataGridViewColumnSortMode.NotSortable;
        //gridCol.HeaderCell.Tag = true;
      }
      grdPreview.SelectionMode = DataGridViewSelectionMode.FullColumnSelect;
    }

    private void btnImport_Click(object sender, EventArgs e)
    {
      List<string> importColumns = new List<string>();
      List<DataGridViewColumn> selectedColumns = new List<DataGridViewColumn>();
      foreach (DataGridViewColumn selCol in grdPreview.Columns)
      {
        if ((bool)selCol.HeaderCell.Tag)
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
        ImportDataTable = Utilities.GetDataFromTableOrView(wbConnection, importDBObject, importColumns, Convert.ToInt32(numFromRow.Value) - 1, Convert.ToInt32(numRowsCount.Value));
      else
        ImportDataTable = Utilities.GetDataFromTableOrView(wbConnection, importDBObject, importColumns);
    }

    private void chkLimitRows_CheckedChanged(object sender, EventArgs e)
    {
      numRowsCount.Enabled = numFromRow.Enabled = chkLimitRows.Checked;
    }

    private void btnSelect_Click(object sender, EventArgs e)
    {
      if (allColumnsSelected)
        grdPreview.ClearSelection();
      else
        grdPreview.SelectAll();
    }

    private void grdPreview_SelectionChanged(object sender, EventArgs e)
    {
      btnSelect.Text = (allColumnsSelected ? "Select None" : "Select All");
      //if (grdPreview.CurrentCell != null)
      //{
      //  DataGridViewColumn dgvc = grdPreview.CurrentCell.OwningColumn;
      //  dgvc.Selected = (bool)dgvc.HeaderCell.Tag;
      //}
    }

    private void grdPreview_ColumnStateChanged(object sender, DataGridViewColumnStateChangedEventArgs e)
    {
      //if (e.StateChanged == DataGridViewElementStates.Selected)
      //  e.Column.HeaderCell.Tag = !(bool)grdPreview.CurrentCell.OwningColumn.HeaderCell.Tag;
    }

    private void grdPreview_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
    {
      grdPreview.SelectAll();
    }
  }
}
