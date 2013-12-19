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

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using MySQL.ForExcel.Classes;
using MySQL.ForExcel.Properties;
using MySQL.Utility.Classes;
using MySQL.Utility.Classes.MySQLWorkbench;
using MySQL.Utility.Forms;

namespace MySQL.ForExcel.Forms
{
  /// <summary>
  /// Previews a MySQL table's data and lets users select columns and rows to import to an Excel spreadsheet.
  /// </summary>
  public partial class ImportTableViewForm : AutoStyleableBaseDialog
  {
    /// <summary>
    /// Initializes a new instance of the <see cref="ImportTableViewForm"/> class.
    /// </summary>
    /// <param name="wbConnection">MySQL Workbench connection to a MySQL server instance selected by users.</param>
    /// <param name="importDbObject">MySQL table, view or procedure from which to import data to an Excel spreadsheet.</param>
    /// <param name="importToWorksheetName">Name of the Excel worksheet where the data will be imported to.</param>
    /// <param name="workSheetInCompatibilityMode">Flag indicating if the Excel worksheet where the data will be imported to is in Excel 2003 compatibility mode.</param>
    /// <param name="importForEditData"><c>true</c> if the import is part of an Edit operation, <c>false</c> otherwise.</param>
    public ImportTableViewForm(MySqlWorkbenchConnection wbConnection, DbObject importDbObject, string importToWorksheetName, bool workSheetInCompatibilityMode, bool importForEditData)
    {
      PreviewDataTable = null;
      ImportOperationGeneratedErrors = false;
      WbConnection = wbConnection;
      ImportDbObject = importDbObject;
      WorkSheetInCompatibilityMode = workSheetInCompatibilityMode;
      ImportDataTable = null;

      InitializeComponent();
      PreviewDataGridView.DataError += PreviewDataGridView_DataError;

      IncludeHeadersCheckBox.Checked = true;
      IncludeHeadersCheckBox.Enabled = !importForEditData;
      ImportWithinEditOperation = importForEditData;
      PreviewDataGridView.DisableColumnsSelection = ImportWithinEditOperation;
      if (importForEditData)
      {
        PreviewDataGridView.ContextMenuStrip = null;
      }

      LimitRowsCheckBox.Checked = false;
      TableNameMainLabel.Text = importDbObject.Type.ToString() + @" Name:";
      OptionsWarningLabel.Text = Resources.WorksheetInCompatibilityModeWarning;
      Text = @"Import Data - " + importToWorksheetName;
      TableNameSubLabel.Text = importDbObject.Name;
      FillPreviewGrid();
    }

    #region Properties

    /// <summary>
    /// Gets a value indicating whether all columns in the preview grid are selected for import.
    /// </summary>
    public bool AllColumnsSelected
    {
      get
      {
        return (PreviewDataGridView.SelectedColumns.Count == PreviewDataGridView.Columns.Count);
      }
    }

    /// <summary>
    /// Gets a <see cref="MySqlDataTable"/> object containing the data to be imported to the active Excel Worksheet.
    /// </summary>
    public MySqlDataTable ImportDataTable { get; private set; }

    /// <summary>
    /// Gets the type of DB object (MySQL table or view) from which to import data to the active Excel Worksheet.
    /// </summary>
    public DbObject ImportDbObject { get; private set; }

    /// <summary>
    /// Gets a value indicating whether the column names will be imported as data headers in the first row of the Excel spreadsheet.
    /// </summary>
    public bool ImportHeaders
    {
      get
      {
        return IncludeHeadersCheckBox.Checked;
      }
    }

    /// <summary>
    /// Gets a value indicating whether the import operation generated errors so the form must not be closed right away.
    /// </summary>
    public bool ImportOperationGeneratedErrors { get; private set; }

    /// <summary>
    /// Gets or sets a value indicating whether the import is part of an Edit operation.
    /// </summary>
    public bool ImportWithinEditOperation { get; private set; }

    /// <summary>
    /// Gets a <see cref="DataTable"/> object containing a subset of the whole data which is shown in the preview grid.
    /// </summary>
    public DataTable PreviewDataTable { get; private set; }

    /// <summary>
    /// Gets or sets the text associated with this control.
    /// </summary>
    public override sealed string Text
    {
      get
      {
        return base.Text;
      }

      set
      {
        base.Text = value;
      }
    }

    /// <summary>
    /// Gets the total rows contained in the MySQL table or view selected for import.
    /// </summary>
    public long TotalRowsCount { get; private set; }

    /// <summary>
    /// Gets the connection to a MySQL server instance selected by users.
    /// </summary>
    public MySqlWorkbenchConnection WbConnection { get; private set; }

    /// <summary>
    /// Gets a value indicating whether the Excel Worksheet where the data will be imported to is in Excel 2003 compatibility mode.
    /// </summary>
    public bool WorkSheetInCompatibilityMode { get; private set; }

    #endregion Properties

    /// <summary>
    /// Event delegate method fired when the <see cref="AdvancedOptionsButton"/> button is clicked.
    /// </summary>
    /// <param name="sender">Sender object.</param>
    /// <param name="e">Event arguments.</param>
    private void AdvancedOptionsButton_Click(object sender, EventArgs e)
    {
      using (ImportAdvancedOptionsDialog optionsDialog = new ImportAdvancedOptionsDialog())
      {
        optionsDialog.ShowDialog();
        if (optionsDialog.ParentFormRequiresRefresh)
        {
          FillPreviewGrid();
        }
      }
    }

    /// <summary>
    /// Event delegate method fired when the <see cref="ContextMenuForGrid"/> context menu strip is opening.
    /// </summary>
    /// <param name="sender">Sender object.</param>
    /// <param name="e">Event arguments.</param>
    private void ContextMenuForGrid_Opening(object sender, CancelEventArgs e)
    {
      SelectAllToolStripMenuItem.Visible = PreviewDataGridView.SelectedColumns.Count < PreviewDataGridView.Columns.Count;
      SelectNoneToolStripMenuItem.Visible = PreviewDataGridView.SelectedColumns.Count > 0;
    }

    /// <summary>
    /// Prepares and fills the preview grid with data.
    /// </summary>
    private void FillPreviewGrid()
    {
      PreviewDataTable = WbConnection.GetDataFromTableOrView(ImportDbObject, null, 0, Settings.Default.ImportPreviewRowsQuantity);
      TotalRowsCount = WbConnection.GetRowsCountFromTableOrView(ImportDbObject);
      RowsCountSubLabel.Text = TotalRowsCount.ToString(CultureInfo.InvariantCulture);
      PreviewDataGridView.DataSource = PreviewDataTable;
      foreach (DataGridViewColumn gridCol in PreviewDataGridView.Columns)
      {
        gridCol.SortMode = DataGridViewColumnSortMode.NotSortable;
      }

      PreviewDataGridView.SelectionMode = DataGridViewSelectionMode.FullColumnSelect;
      bool cappingAtMaxCompatRows = WorkSheetInCompatibilityMode && TotalRowsCount > UInt16.MaxValue;
      SetCompatibilityWarningControlsVisibility(cappingAtMaxCompatRows);
      FromRowNumericUpDown.Maximum = cappingAtMaxCompatRows ? UInt16.MaxValue : TotalRowsCount;
      RowsToReturnNumericUpDown.Maximum = FromRowNumericUpDown.Maximum - FromRowNumericUpDown.Value + 1;
    }

    /// <summary>
    /// Event delegate method fired when the value of the <see cref="FromRowNumericUpDown"/> control changes.
    /// </summary>
    /// <param name="sender">Sender object</param>
    /// <param name="e">Event arguments</param>
    private void FromRowNumericUpDown_ValueChanged(object sender, EventArgs e)
    {
      RowsToReturnNumericUpDown.Maximum = FromRowNumericUpDown.Maximum - FromRowNumericUpDown.Value + 1;
    }

    /// <summary>
    /// Event delegate method fired when the <see cref="ImportButton"/> button is clicked.
    /// </summary>
    /// <param name="sender">Sender object.</param>
    /// <param name="e">Event arguments.</param>
    private void ImportButton_Click(object sender, EventArgs e)
    {
      List<string> importColumns = null;
      List<DataGridViewColumn> selectedColumns = new List<DataGridViewColumn>();
      if (PreviewDataGridView.SelectedColumns.Count < PreviewDataGridView.Columns.Count)
      {
        importColumns = new List<string>(PreviewDataGridView.SelectedColumns.Count);
        selectedColumns.AddRange(PreviewDataGridView.SelectedColumns.Cast<DataGridViewColumn>());

        if (selectedColumns.Count > 1)
        {
          selectedColumns.Sort((c1, c2) => c1.Index.CompareTo(c2.Index));
        }

        importColumns.AddRange(selectedColumns.Select(selCol => selCol.HeaderText));
      }

      try
      {
        Cursor = Cursors.WaitCursor;
        DataTable dt;
        if (LimitRowsCheckBox.Checked)
        {
          dt = WbConnection.GetDataFromTableOrView(ImportDbObject, importColumns, Convert.ToInt32(FromRowNumericUpDown.Value) - 1, Convert.ToInt32(RowsToReturnNumericUpDown.Value));
        }
        else if (WorkSheetInCompatibilityMode)
        {
          dt = WbConnection.GetDataFromTableOrView(ImportDbObject, importColumns, 0, UInt16.MaxValue);
        }
        else
        {
          dt = WbConnection.GetDataFromTableOrView(ImportDbObject, importColumns);
        }

        if (dt != null)
        {
          ImportDataTable = new MySqlDataTable(ImportDbObject.Name, dt, WbConnection, ImportWithinEditOperation);
          ImportDataTable.AddExtendedProperties(dt.ExtendedProperties["QueryString"].ToString(), ImportHeaders, ImportDbObject.Name);
        }
      }
      catch (Exception ex)
      {
        MiscUtilities.ShowCustomizedErrorDialog(Resources.ImportTableErrorTitle, ex.Message, true);
        ImportOperationGeneratedErrors = true;
        MySqlSourceTrace.WriteAppErrorToLog(ex);
      }

      Cursor = Cursors.Default;
    }

    /// <summary>
    /// Event delegate method fired when the <see cref="ImportTableViewForm"/> is closing.
    /// </summary>
    /// <param name="sender">Sender object.</param>
    /// <param name="e">Event arguments.</param>
    private void ImportTableViewForm_FormClosing(object sender, FormClosingEventArgs e)
    {
      e.Cancel = ImportOperationGeneratedErrors;
      ImportOperationGeneratedErrors = false;
    }

    /// <summary>
    /// Event delegate method fired when the <see cref="LimitRowsCheckBox"/> checked state changes.
    /// </summary>
    /// <param name="sender">Sender object.</param>
    /// <param name="e">Event arguments.</param>
    private void LimitRowsCheckBox_CheckedChanged(object sender, EventArgs e)
    {
      RowsToReturnNumericUpDown.Enabled = FromRowNumericUpDown.Enabled = LimitRowsCheckBox.Checked;
    }

    /// <summary>
    /// Event delegate method fired when the <see cref="PreviewDataGridView"/> grid is done with its data binding operation.
    /// </summary>
    /// <param name="sender">Sender object.</param>
    /// <param name="e">Event arguments.</param>
    private void PreviewDataGridView_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
    {
      PreviewDataGridView.SelectAll();
    }

    /// <summary>
    /// Event delegate method fired when the <see cref="PreviewDataGridView"/> detects a data error in one of its cells.
    /// </summary>
    /// <param name="sender">Sender object.</param>
    /// <param name="e">Event arguments.</param>
    private void PreviewDataGridView_DataError(object sender, DataGridViewDataErrorEventArgs e)
    {
      if (PreviewDataGridView.Rows[e.RowIndex].Cells[e.ColumnIndex].ValueType != Type.GetType("System.Byte[]"))
      {
        return;
      }

      try
      {
        var img = (byte[])(PreviewDataGridView.Rows[e.RowIndex].Cells[e.ColumnIndex]).Value;
        using (MemoryStream ms = new MemoryStream(img))
        {
          Image.FromStream(ms);
        }
      }
      catch (ArgumentException argEx)
      {
        MySqlSourceTrace.WriteAppErrorToLog(argEx);
      }
      catch (Exception ex)
      {
        MiscUtilities.ShowCustomizedErrorDialog(Resources.DataLoadingError, ex.Message);
        MySqlSourceTrace.WriteAppErrorToLog(ex);
      }
    }

    /// <summary>
    /// Event delegate method fired when the selection of the <see cref="PreviewDataGridView"/> grid changes.
    /// </summary>
    /// <param name="sender">Sender object.</param>
    /// <param name="e">Event arguments.</param>
    private void PreviewDataGridView_SelectionChanged(object sender, EventArgs e)
    {
      ImportButton.Enabled = PreviewDataGridView.SelectedColumns.Count > 0;
    }

    /// <summary>
    /// Event delegate method fired when the <see cref="SelectAllToolStripMenuItem"/> context menu item is clicked.
    /// </summary>
    /// <param name="sender">Sender object.</param>
    /// <param name="e">Event arguments.</param>
    private void SelectAllToolStripMenuItem_Click(object sender, EventArgs e)
    {
      PreviewDataGridView.SelectAll();
    }

    /// <summary>
    /// Event delegate method fired when the <see cref="SelectNoneToolStripMenuItem"/> context menu item is clicked.
    /// </summary>
    /// <param name="sender">Sender object.</param>
    /// <param name="e">Event arguments.</param>
    private void SelectNoneToolStripMenuItem_Click(object sender, EventArgs e)
    {
      PreviewDataGridView.ClearSelection();
    }

    /// <summary>
    /// Shows or hides the compatibility warning controls to let the users know if the Excel spreadsheet is running in Excel 2003 compatibility mode.
    /// </summary>
    /// <param name="show">Flag indicating if the compatibility warning controls should be shown.</param>
    private void SetCompatibilityWarningControlsVisibility(bool show)
    {
      OptionsWarningLabel.Visible = show;
      OptionsWarningPictureBox.Visible = show;
    }

    /// <summary>
    /// Hides the Import form from the user at the same time it fakes the click over OK, silently opening an edit session.
    /// </summary>
    /// <returns>Always DialogResult.OK</returns>
    public DialogResult ImportHidingDialog()
    {
      ImportButton_Click(this, EventArgs.Empty);
      return DialogResult.OK;
    }
  }
}