// 
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
//

namespace MySQL.ForExcel
{
  using Excel = Microsoft.Office.Interop.Excel;
  using MySQL.Utility;
  using System;
  using System.Collections.Generic;
  using System.ComponentModel;
  using System.Data;
  using System.Drawing;
  using System.IO;
  using System.Linq;
  using System.Text;
  using System.Windows.Forms;

  /// <summary>
  /// Previews a MySQL table's data and lets users select columns and rows to import to an Excel spreadsheet.
  /// </summary>
  public partial class ImportTableViewForm : AutoStyleableBaseDialog
  {
    #region Fields

    /// <summary>
    /// <see cref="DataTable"/> containing the data to be imported to the Excel spreadsheet.
    /// </summary>
    public DataTable ImportDataTable;

    /// <summary>
    /// Connection to a MySQL server instance selected by users.
    /// </summary>
    private MySqlWorkbenchConnection _wbConnection;

    /// <summary>
    /// MySQL table, view or procedure from which to import data to an Excel spreadsheet.
    /// </summary>
    private DBObject _importDBObject;

    /// <summary>
    /// <see cref="DataTable"/> object containing a subset of the whole data which is shown in the preview grid.
    /// </summary>
    private DataTable _previewDataTable;

    /// <summary>
    /// Flag indicating if the Excel worksheet where the data will be imported to is in Excel 2003 compatibility mode.
    /// </summary>
    private bool _workSheetInCompatibilityMode;

    /// <summary>
    /// Flag indicatinf if the import operation generated errors so the form must not be closed right away.
    /// </summary>
    private bool _hasError;

    #endregion Fields

    /// <summary>
    /// Initializes a new instance of the <see cref="ImportTableViewForm"/> class.
    /// </summary>
    /// <param name="wbConnection">Connection to a MySQL server instance selected by users.</param>
    /// <param name="importDBObject">MySQL table, view or procedure from which to import data to an Excel spreadsheet.</param>
    /// <param name="importToWorksheetName">Name of the Excel worksheet where the data will be imported to.</param>
    /// <param name="workSheetInCompatibilityMode">Flag indicating if the Excel worksheet where the data will be imported to is in Excel 2003 compatibility mode.</param>
    /// <param name="importForEditData">true if the import is part of an Edit operation, false otherwise.</param>
    public ImportTableViewForm(MySqlWorkbenchConnection wbConnection, DBObject importDBObject, string importToWorksheetName, bool workSheetInCompatibilityMode, bool importForEditData)
    {
      _previewDataTable = null;
      _hasError = false;
      _wbConnection = wbConnection;
      _importDBObject = importDBObject;
      _workSheetInCompatibilityMode = workSheetInCompatibilityMode;
      ImportDataTable = null;

      InitializeComponent();
      grdPreviewData.DataError += new DataGridViewDataErrorEventHandler(grdPreviewData_DataError);

      chkIncludeHeaders.Checked = true;
      chkIncludeHeaders.Enabled = !importForEditData;
      ImportWithinEditOperation = importForEditData;
      grdPreviewData.DisableColumnsSelection = ImportWithinEditOperation;
      if (importForEditData)
      {
        grdPreviewData.ContextMenuStrip = null;
      }

      chkLimitRows.Checked = false;
      lblTableNameMain.Text = String.Format("{0} Name:", importDBObject.Type.ToString());
      lblOptionsWarning.Text = Properties.Resources.WorkSheetInCompatibilityModeWarning;
      Text = String.Format("Import Data - {0}", importToWorksheetName);
      lblTableNameSub.Text = importDBObject.Name;
      FillPreviewGrid();
    }

    #region Properties

    /// <summary>
    /// Gets a value indicating whether all columns in the preview grid are selected for import.
    /// </summary>
    public bool AllColumnsSelected
    {
      get { return (grdPreviewData.SelectedColumns.Count == grdPreviewData.Columns.Count); }
    }

    /// <summary>
    /// Gets a value indicating whether the column names will be imported as data headers in the first row of the Excel spreadsheet.
    /// </summary>
    public bool ImportHeaders
    {
      get { return chkIncludeHeaders.Checked; }
    }

    /// <summary>
    /// Gets or sets a value indicating whether the import is part of an Edit operation.
    /// </summary>
    public bool ImportWithinEditOperation { get; private set; }

    /// <summary>
    /// Gets the total rows contained in the MySQL table or view selected for import.
    /// </summary>
    public long TotalRowsCount { get; private set; }

    /// <summary>
    /// Shows or hides the compatibility warning controls to let the users know if the Excel spreadsheet is running in Excel 2003 compatibility mode.
    /// </summary>
    /// <param name="show">Flag indicating if the compatibility warning controls should be shown.</param>
    private void SetCompatibilityWarningControlsVisibility(bool show)
    {
      lblOptionsWarning.Visible = show;
      picOptionsWarning.Visible = show;
    }

    #endregion Properties

    /// <summary>
    /// Prepares and fills the preview grid with data.
    /// </summary>
    private void FillPreviewGrid()
    {
      _previewDataTable = MySQLDataUtilities.GetDataFromTableOrView(_wbConnection, _importDBObject, null, 0, 10);
      TotalRowsCount = MySQLDataUtilities.GetRowsCountFromTableOrView(_wbConnection, _importDBObject);
      lblRowsCountSub.Text = TotalRowsCount.ToString();
      grdPreviewData.DataSource = _previewDataTable;
      foreach (DataGridViewColumn gridCol in grdPreviewData.Columns)
      {
        gridCol.SortMode = DataGridViewColumnSortMode.NotSortable;
      }

      grdPreviewData.SelectionMode = DataGridViewSelectionMode.FullColumnSelect;
      bool cappingAtMaxCompatRows = _workSheetInCompatibilityMode && TotalRowsCount > UInt16.MaxValue;
      SetCompatibilityWarningControlsVisibility(cappingAtMaxCompatRows);
      numFromRow.Maximum = cappingAtMaxCompatRows ? UInt16.MaxValue : TotalRowsCount;
      numRowsToReturn.Maximum = numFromRow.Maximum - numFromRow.Value + 1;
    }

    /// <summary>
    /// Event delegate method fired when the <see cref="btnImport"/> button is clicked.
    /// </summary>
    /// <param name="sender">Sender object.</param>
    /// <param name="e">Event arguments.</param>
    private void btnImport_Click(object sender, EventArgs e)
    {
      List<string> importColumns = null;
      List<DataGridViewColumn> selectedColumns = new List<DataGridViewColumn>();
      if (grdPreviewData.SelectedColumns.Count < grdPreviewData.Columns.Count)
      {
        importColumns = new List<string>(grdPreviewData.SelectedColumns.Count);
        foreach (DataGridViewColumn selCol in grdPreviewData.SelectedColumns)
        {
          selectedColumns.Add(selCol);
        }

        if (selectedColumns.Count > 1)
        {
          selectedColumns.Sort(delegate(DataGridViewColumn c1, DataGridViewColumn c2)
          {
            return c1.Index.CompareTo(c2.Index);
          });
        }

        foreach (DataGridViewColumn selCol in selectedColumns)
        {
          importColumns.Add(selCol.HeaderText);
        }
      }

      try
      {
        this.Cursor = Cursors.WaitCursor;
        if (chkLimitRows.Checked)
        {
          ImportDataTable = MySQLDataUtilities.GetDataFromTableOrView(_wbConnection, _importDBObject, importColumns, Convert.ToInt32(numFromRow.Value) - 1, Convert.ToInt32(numRowsToReturn.Value));
        }
        else if (_workSheetInCompatibilityMode)
        {
          ImportDataTable = MySQLDataUtilities.GetDataFromTableOrView(_wbConnection, _importDBObject, importColumns, 0, UInt16.MaxValue);
        }
        else
        {
          ImportDataTable = MySQLDataUtilities.GetDataFromTableOrView(_wbConnection, _importDBObject, importColumns);
        }
      }
      catch (Exception ex)
      {
        InfoDialog errorDialog = new InfoDialog(false, Properties.Resources.ImportTableErrorTitle, ex.Message);
        errorDialog.WordWrapDetails = true;
        errorDialog.ShowDialog();
        _hasError = true;
        MiscUtilities.WriteAppErrorToLog(ex);
      }

      this.Cursor = Cursors.Default;
    }

    /// <summary>
    /// Event delegate method fired when the <see cref="chkLimitRows"/> checked state changes.
    /// </summary>
    /// <param name="sender">Sender object.</param>
    /// <param name="e">Event arguments.</param>
    private void chkLimitRows_CheckedChanged(object sender, EventArgs e)
    {
      numRowsToReturn.Enabled = numFromRow.Enabled = chkLimitRows.Checked;
    }

    /// <summary>
    /// Event delegate method fired when the <see cref="grdPreviewData"/> grid is done with its data binding operation.
    /// </summary>
    /// <param name="sender">Sender object.</param>
    /// <param name="e">Event arguments.</param>
    private void grdPreviewData_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
    {
      grdPreviewData.SelectAll();
    }

    /// <summary>
    /// Event delegate method fired when the <see cref="grdPreviewData"/> detects a data error in one of its cells.
    /// </summary>
    /// <param name="sender">Sender object.</param>
    /// <param name="e">Event arguments.</param>
    private void grdPreviewData_DataError(object sender, DataGridViewDataErrorEventArgs e)
    {
      if (grdPreviewData.Rows[e.RowIndex].Cells[e.ColumnIndex].ValueType == Type.GetType("System.Byte[]"))
      {
        try
        {
          var img = (byte[])(grdPreviewData.Rows[e.RowIndex].Cells[e.ColumnIndex]).Value;
          using (MemoryStream ms = new MemoryStream(img))
          {
            Image.FromStream(ms);
          }
        }
        catch (ArgumentException argEx)
        {
          MiscUtilities.WriteAppErrorToLog(argEx);
        }
        catch (Exception ex)
        {
          InfoDialog errorDialog = new InfoDialog(false, Properties.Resources.DataLoadingError, ex.Message);
          errorDialog.WordWrapDetails = true;
          errorDialog.ShowDialog();
          MiscUtilities.WriteAppErrorToLog(ex);
        }
      }
    }

    /// <summary>
    /// Event delegate method fired when the selection of the <see cref="grdPreviewData"/> grid changes.
    /// </summary>
    /// <param name="sender">Sender object.</param>
    /// <param name="e">Event arguments.</param>
    private void grdPreviewData_SelectionChanged(object sender, EventArgs e)
    {
      contextMenuForGrid.Items[0].Text = AllColumnsSelected ? "Select None" : "Select All";
      btnImport.Enabled = grdPreviewData.SelectedColumns.Count > 0;
    }

    /// <summary>
    /// Event delegate method fired when the value of the <see cref="numFromRow"/> control changes.
    /// </summary>
    /// <param name="sender">Sender object</param>
    /// <param name="e">Event arguments</param>
    private void numFromRow_ValueChanged(object sender, EventArgs e)
    {
      bool cappingAtMaxCompatRows = _workSheetInCompatibilityMode && TotalRowsCount > UInt16.MaxValue;
      numRowsToReturn.Maximum = numFromRow.Maximum - numFromRow.Value + 1;
    }

    /// <summary>
    /// Event delegate method fired when the <see cref="selectAllToolStripMenuItem"/> context menu item is clicked.
    /// </summary>
    /// <param name="sender">Sender object.</param>
    /// <param name="e">Event arguments.</param>
    private void selectAllToolStripMenuItem_Click(object sender, EventArgs e)
    {
      if (AllColumnsSelected)
      {
        grdPreviewData.ClearSelection();
      }
      else
      {
        grdPreviewData.SelectAll();
      }
    }

    /// <summary>
    /// Event delegate method fired when the <see cref="ImportTableViewForm"/> is closed.
    /// </summary>
    /// <param name="sender">Sender object.</param>
    /// <param name="e">Event arguments.</param>
    private void ImportTableViewForm_FormClosing(object sender, FormClosingEventArgs e)
    {
      e.Cancel = _hasError;
      _hasError = false;
    }
  }
}
