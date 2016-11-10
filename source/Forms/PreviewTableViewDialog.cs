// Copyright (c) 2014, 2016, Oracle and/or its affiliates. All rights reserved.
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
using System.Data;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Windows.Forms;
using MySQL.ForExcel.Classes;
using MySQL.ForExcel.Properties;
using MySql.Utility.Classes.MySql;
using MySql.Utility.Forms;

namespace MySQL.ForExcel.Forms
{
  /// <summary>
  /// Previews a MySQL table's data and lets users select columns and rows to import to an Excel spreadsheet.
  /// </summary>
  public partial class PreviewTableViewDialog : AutoStyleableBaseDialog
  {
    #region Fields

    /// <summary>
    /// The MySQL table or view to preview data for.
    /// </summary>
    private readonly DbView _previewTableOrView;

    /// <summary>
    /// A <see cref="System.Data.DataTable"/> object containing a subset of the whole data which is shown in the preview grid.
    /// </summary>
    private DataTable _previewDataTable;

    /// <summary>
    /// The total rows contained in the MySQL table or view selected for import.
    /// </summary>
    private long _totalRowsCount;

    #endregion Fields

    /// <summary>
    /// Initializes a new instance of the <see cref="PreviewTableViewDialog"/> class.
    /// </summary>
    /// <param name="previewTableOrView">The type of DB object (MySQL table or view) to preview data for.</param>
    /// <param name="showCancelButton">Flag indicating whether the Cancel button is shown along with the OK one, or hidden.</param>
    public PreviewTableViewDialog(DbView previewTableOrView, bool showCancelButton)
    {
      if (previewTableOrView == null)
      {
        throw new ArgumentNullException("previewTableOrView");
      }

      _previewDataTable = null;
      _previewTableOrView = previewTableOrView;

      InitializeComponent();

      InitializeDialogButtons(showCancelButton);
      RowsNumericUpDown.Value = Settings.Default.ImportPreviewRowsQuantity;
      TableNameMainLabel.Text = previewTableOrView is DbTable ? "Table Name" : "View Name";
      TableNameSubLabel.Text = previewTableOrView.Name;
      FillPreviewGrid();
    }

    #region Properties

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

    #endregion Properties

    /// <summary>
    /// Prepares and fills the preview grid with data.
    /// </summary>
    private void FillPreviewGrid()
    {
      SetImportParameterValues();
      _previewDataTable = _previewTableOrView.GetData();
      _totalRowsCount = _previewTableOrView.GetRowsCount();
      RowsCountSubLabel.Text = _totalRowsCount.ToString(CultureInfo.InvariantCulture);
      PreviewDataGridView.DataSource = _previewDataTable;
      foreach (DataGridViewColumn gridCol in PreviewDataGridView.Columns)
      {
        gridCol.SortMode = DataGridViewColumnSortMode.NotSortable;
      }

      PreviewDataGridView.SelectionMode = DataGridViewSelectionMode.FullColumnSelect;
      RowsNumericUpDown.Maximum = _totalRowsCount;
    }

    /// <summary>
    /// Initializes the visibility and position of the dialog buttons.
    /// </summary>
    /// <param name="showCancelButton">Flag indicating whether the Cancel button is shown along with the OK one, or hidden.</param>
    private void InitializeDialogButtons(bool showCancelButton)
    {
      if (showCancelButton)
      {
        return;
      }

      DialogCancelButton.Visible = false;
      OkButton.Anchor = AnchorStyles.None;
      OkButton.Location = DialogCancelButton.Location;
      OkButton.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
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
        using (var ms = new MemoryStream(img))
        {
          Image.FromStream(ms);
        }
      }
      catch (ArgumentException argEx)
      {
        MySqlSourceTrace.WriteAppErrorToLog(argEx, false);
      }
      catch (Exception ex)
      {
        MySqlSourceTrace.WriteAppErrorToLog(ex, null, Resources.DataLoadingError, true);
      }
    }

    /// <summary>
    /// Event delegate method fired when the <see cref="RefreshButton"/> button is clicked.
    /// </summary>
    /// <param name="sender">Sender object.</param>
    /// <param name="e">Event arguments.</param>
    private void RefreshButton_Click(object sender, EventArgs e)
    {
      FillPreviewGrid();
    }

    /// <summary>
    /// Sets the import parameter values into the database object.
    /// This is needed before getting any data from it.
    /// </summary>
    private void SetImportParameterValues()
    {
      _previewTableOrView.ImportParameters.ColumnsNamesList = null;
      _previewTableOrView.ImportParameters.FirstRowIndex = 0;
      _previewTableOrView.ImportParameters.RowsCount = (int)RowsNumericUpDown.Value;
    }
  }
}