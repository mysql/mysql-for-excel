// Copyright (c) 2014, 2017, Oracle and/or its affiliates. All rights reserved.
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
using System.Globalization;
using System.Windows.Forms;
using MySQL.ForExcel.Classes;
using MySQL.ForExcel.Properties;
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
        throw new ArgumentNullException(nameof(previewTableOrView));
      }

      _previewTableOrView = previewTableOrView;

      InitializeComponent();

      PreviewDataGridView.SelectAllAfterBindingComplete = true;
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
      Cursor = Cursors.WaitCursor;
      SetImportParameterValues();
      PreviewDataGridView.Fill(_previewTableOrView);
      _totalRowsCount = _previewTableOrView.GetRowsCount();
      RowsCountSubLabel.Text = _totalRowsCount.ToString(CultureInfo.InvariantCulture);
      RowsNumericUpDown.Maximum = _totalRowsCount;
      Cursor = Cursors.Default;
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