// Copyright (c) 2012, 2019, Oracle and/or its affiliates. All rights reserved.
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
using System.Globalization;
using System.Linq;
using System.Windows.Forms;
using MySQL.ForExcel.Classes;
using MySQL.ForExcel.Properties;
using MySql.Utility.Forms;
using ExcelInterop = Microsoft.Office.Interop.Excel;

namespace MySQL.ForExcel.Forms
{
  /// <summary>
  /// Previews a MySQL table's data and lets users select columns and rows to import to an Excel spreadsheet.
  /// </summary>
  public partial class ImportTableViewForm : AutoStyleableBaseDialog
  {
    #region Fields

    /// <summary>
    /// The type of DB object (MySQL table or view) from which to import data to the active Excel Worksheet.
    /// </summary>
    private readonly DbView _dbTableOrView;

    /// <summary>
    /// The list of columns selected by the user to be imported to Excel.
    /// </summary>
    private List<string> _importColumns;

    /// <summary>
    /// Information of the rows that can be imported to the active worksheet.
    /// </summary>
    private ImportingRowsInfo _importingRowsInfo;

    /// <summary>
    /// Flag indicating whether the returning rows number exceeds the Worksheet's rows maximum limit.
    /// </summary>
    private bool? _rowsExceedWorksheetLimit;

    #endregion Fields

    /// <summary>
    /// Initializes a new instance of the <see cref="ImportTableViewForm"/> class.
    /// </summary>
    /// <param name="importDbTableOrView">MySQL table or view from which to import data to an Excel spreadsheet.</param>
    /// <param name="importToWorksheetName">Name of the Excel worksheet where the data will be imported to.</param>
    public ImportTableViewForm(DbView importDbTableOrView, string importToWorksheetName)
    {
      _dbTableOrView = importDbTableOrView ?? throw new ArgumentNullException(nameof(importDbTableOrView));
      _importColumns = null;
      _importingRowsInfo = null;
      _rowsExceedWorksheetLimit = null;
      MySqlTable = null;
      InitializeComponent();

      PreviewDataGridView.SelectAllAfterBindingComplete = true;
      SetAnchors();
      TableNameMainLabel.Text = importDbTableOrView is DbTable ? "Table Name" : "View Name";
      PickColumnsSubLabel.Text = string.Format(Resources.ImportTableOrViewSubText, importDbTableOrView is DbTable ? "table" : "view");
      Text = @"Import Data - " + importToWorksheetName;
      TableNameSubLabel.Text = importDbTableOrView.Name;
      FillPreviewGrid();
      InitializeOptions();
      SetCompatibilityWarningControlsVisibility();
    }

    #region Properties

    /// <summary>
    /// Gets the <see cref="MySqlDataTable"/> object containing the data to be imported to the active Excel Worksheet.
    /// </summary>
    [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    public MySqlDataTable MySqlTable { get; private set; }

    /// <summary>
    /// Gets or sets the text associated with this control.
    /// </summary>
    public sealed override string Text
    {
      get => base.Text;
      set => base.Text = value;
    }

    /// <summary>
    /// Gets a value indicating the index of the first row obtained by the select query.
    /// </summary>
    private int FirstRowIndex
    {
      get
      {
        var firstRowIndex = (int)FromRowNumericUpDown.Value - 1;
        return LimitRowsCheckBox.Checked ? firstRowIndex : -1;
      }
    }

    /// <summary>
    /// Gets a value indicating whether the <see cref="RowsToReturn"/> value exceeds the Worksheet's rows maximum limit.
    /// </summary>
    private bool RowsExceedWorksheetLimit
    {
      get
      {
        if (_rowsExceedWorksheetLimit == null)
        {
          _rowsExceedWorksheetLimit = ExcelUtilities.CheckIfRowsExceedWorksheetLimit(RowsToReturn < 0 ? _importingRowsInfo.RowsCount : RowsToReturn);
        }

        return _rowsExceedWorksheetLimit.Value;
      }
    }

    /// <summary>
    /// Gets a value indicating the number of rows to be fetched by the select query.
    /// </summary>
    private int RowsToReturn => LimitRowsCheckBox.Checked ? (int)RowsLimitNumericUpDown.Value : -1;

    #endregion Properties

    /// <summary>
    /// Hides the Import form from the user at the same time it fakes the click over OK, silently opening an edit session.
    /// </summary>
    /// <returns>Always DialogResult.OK</returns>
    public DialogResult ImportHidingDialog()
    {
      var success = ImportData();
      return success ? DialogResult.OK : DialogResult.Cancel;
    }

    /// <summary>
    /// Handles the CheckedChanged event of the AddSummaryFieldsCheckBox control.
    /// </summary>
    /// <param name="sender">The source of the event.</param>
    /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
    private void AddSummaryFieldsCheckBox_CheckedChanged(object sender, EventArgs e)
    {
      RecalculateImportingRowsInformation();
    }

    /// <summary>
    /// Event delegate method fired when the <see cref="AdvancedOptionsButton"/> button is clicked.
    /// </summary>
    /// <param name="sender">Sender object.</param>
    /// <param name="e">Event arguments.</param>
    private void AdvancedOptionsButton_Click(object sender, EventArgs e)
    {
      using (var optionsDialog = new ImportAdvancedOptionsDialog())
      {
        optionsDialog.ShowDialog();
        if (optionsDialog.ParentFormRequiresDataReload)
        {
          FillPreviewGrid();
        }

        if (optionsDialog.ParentFormRequiresLimitRecalculation)
        {
          RecalculateImportingRowsInformation();
        }

        AddSummaryFieldsCheckBox.Checked = Settings.Default.ImportCreateExcelTable && AddSummaryFieldsCheckBox.Checked;
        AddSummaryFieldsCheckBox.Enabled = Settings.Default.ImportCreateExcelTable;
      }
    }

    /// <summary>
    /// Prepares and fills the preview grid with data.
    /// </summary>
    private void FillPreviewGrid()
    {
      Cursor = Cursors.WaitCursor;
      _importColumns = null;
      SetImportParameterValues(Settings.Default.ImportPreviewRowsQuantity);
      PreviewDataGridView.Fill(_dbTableOrView);
      RecalculateImportingRowsInformation();
      RowsCountSubLabel.Text = _importingRowsInfo.RowsCount.ToString(CultureInfo.CurrentCulture);
      FromRowNumericUpDown.Maximum = _importingRowsInfo.RowsCount;
      RecalculateImportingRowsInformation();
      Cursor = Cursors.Default;
    }

    /// <summary>
    /// Event delegate method fired when the value of the <see cref="FromRowNumericUpDown"/> control changes.
    /// </summary>
    /// <param name="sender">Sender object</param>
    /// <param name="e">Event arguments</param>
    private void FromRowNumericUpDown_ValueChanged(object sender, EventArgs e)
    {
      RecalculateImportingRowsInformation();
    }

    /// <summary>
    /// Event delegate method fired when the <see cref="GridContextMenuStrip"/> context menu strip is opening.
    /// </summary>
    /// <param name="sender">Sender object.</param>
    /// <param name="e">Event arguments.</param>
    private void GridContextMenuStrip_Opening(object sender, CancelEventArgs e)
    {
      SelectAllToolStripMenuItem.Visible = PreviewDataGridView.SelectedColumns.Count < PreviewDataGridView.Columns.Count;
      SelectNoneToolStripMenuItem.Visible = PreviewDataGridView.SelectedColumns.Count > 0;
    }

    /// <summary>
    /// Imports the selected MySQL table's data into the active Excel worksheet.
    /// </summary>
    /// <returns><c>true</c> if the import is successful, <c>false</c> if errors were found during the import.</returns>
    private bool ImportData()
    {
      _importColumns = null;
      var selectedColumns = new List<DataGridViewColumn>();
      if (PreviewDataGridView.SelectedColumns.Count < PreviewDataGridView.Columns.Count)
      {
        _importColumns = new List<string>(PreviewDataGridView.SelectedColumns.Count);
        selectedColumns.AddRange(PreviewDataGridView.SelectedColumns.Cast<DataGridViewColumn>());
        if (selectedColumns.Count > 1)
        {
          selectedColumns.Sort((c1, c2) => c1.Index.CompareTo(c2.Index));
        }

        _importColumns.AddRange(selectedColumns.Select(selCol => selCol.HeaderText));
      }

      Cursor = Cursors.WaitCursor;

      // If the importing data exceeds the number of available rows and no row limit was set we will force it for the Select Query.
      SetImportParameterValues(RowsExceedWorksheetLimit ? (int)_importingRowsInfo.RowsLimit : RowsToReturn);

      // Import data into Excel
      var importTuple = _dbTableOrView.ImportData();
      MySqlTable = importTuple?.Item1;

      Cursor = Cursors.Default;
      return MySqlTable != null;
    }

    /// <summary>
    /// Event delegate method fired when the <see cref="ImportTableViewForm"/> is closing.
    /// </summary>
    /// <param name="sender">Sender object.</param>
    /// <param name="e">Event arguments.</param>
    private void ImportTableViewForm_FormClosing(object sender, FormClosingEventArgs e)
    {
      if (DialogResult == DialogResult.OK)
      {
        e.Cancel = !ImportData();
      }
    }

    /// <summary>
    /// Sets the initial state of controls representing user options.
    /// </summary>
    private void InitializeOptions()
    {
      IncludeHeadersCheckBox.Checked = true;
      LimitRowsCheckBox.Checked = false;
      AddSummaryFieldsCheckBox.Enabled = Settings.Default.ImportCreateExcelTable;
    }

    /// <summary>
    /// Handles the CheckedChanged event of the IncludeHeadersCheckBox control.
    /// </summary>
    /// <param name="sender">The source of the event.</param>
    /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
    private void IncludeHeadersCheckBox_CheckedChanged(object sender, EventArgs e)
    {
      RecalculateImportingRowsInformation();
    }

    /// <summary>
    /// Event delegate method fired when the <see cref="LimitRowsCheckBox"/> checked state changes.
    /// </summary>
    /// <param name="sender">Sender object.</param>
    /// <param name="e">Event arguments.</param>
    private void LimitRowsCheckBox_CheckedChanged(object sender, EventArgs e)
    {
      _rowsExceedWorksheetLimit = null;
      var limitRows = LimitRowsCheckBox.Checked;
      FromRowNumericUpDown.Enabled = limitRows;
      RowsLimitNumericUpDown.Enabled = limitRows;
      RecalculateImportingRowsInformation();
      if (!limitRows || !RowsLimitNumericUpDown.CanFocus)
      {
        return;
      }

      // Give focus to the field related to the checkbox whose status changed.
      RowsLimitNumericUpDown.Focus();
      RowsLimitNumericUpDown.Select(0, RowsLimitNumericUpDown.Text.Length);
    }

    /// <summary>
    /// Event delegate method fired when the <see cref="MaxValueToolStripMenuItem"/> context menu item is clicked.
    /// </summary>
    /// <param name="sender">Sender object.</param>
    /// <param name="e">Event arguments.</param>
    private void MaxValueToolStripMenuItem_Click(object sender, EventArgs e)
    {
      var item = sender as ToolStripMenuItem;
      if (!(item?.Owner is ContextMenuStrip owner))
      {
        return;
      }

      if (owner.SourceControl == FromRowNumericUpDown)
      {
        FromRowNumericUpDown.Value = FromRowNumericUpDown.Maximum;
      }
      else if (owner.SourceControl == RowsLimitNumericUpDown)
      {
        RowsLimitNumericUpDown.Value = RowsLimitNumericUpDown.Maximum;
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
    /// Recalculates information for importing rows and assigns the rows limit to the <see cref="RowsLimitNumericUpDown"/> control's Maximum property.
    /// </summary>
    private void RecalculateImportingRowsInformation()
    {
      _importingRowsInfo = PreviewDataGridView.DataSource is DataTable dataTable
        ? ImportingRowsInfo.FromDataTable(dataTable, IncludeHeadersCheckBox.Checked, AddSummaryFieldsCheckBox.Checked, (long)FromRowNumericUpDown.Value)
        : ImportingRowsInfo.FromRowsCount(_dbTableOrView.GetRowsCount(), IncludeHeadersCheckBox.Checked, AddSummaryFieldsCheckBox.Checked, (long)FromRowNumericUpDown.Value);
      RowsLimitNumericUpDown.Maximum = _importingRowsInfo.RowsLimit;
      SetCompatibilityWarningControlsVisibility();
    }

    /// <summary>
    /// Event delegate method fired when the value of the <see cref="RowsLimitNumericUpDown"/> control changes.
    /// </summary>
    /// <param name="sender">Sender object</param>
    /// <param name="e">Event arguments</param>
    private void RowsLimitNumericUpDown_ValueChanged(object sender, EventArgs e)
    {
      _rowsExceedWorksheetLimit = null;
      SetCompatibilityWarningControlsVisibility();
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
    /// Shows or hides the compatibility warning controls to let the users know if the rows to be imported exceed the limit of rows of the current Excel Worksheet.
    /// </summary>
    private void SetCompatibilityWarningControlsVisibility()
    {
      // Warning for importing rows exceeding the Worksheet's row limit
      var show = RowsExceedWorksheetLimit;
      OptionsWarningPictureBox.Visible = show;
      OptionsWarningLabel.Visible = show;
      if (show)
      {
        OptionsWarningLabel.Text = Resources.ImportDataWillBeTruncatedWarning;
        return;
      }

      // Warning for capping returning rows limit to avoid exceeding the Worksheet's row limit
      show = _importingRowsInfo.RowsLimit < _importingRowsInfo.RowsCount;
      OptionsWarningLabel.Text = show
        ? _importingRowsInfo.MaximumExcelRowsThatFit < _importingRowsInfo.RowsToImport
          ? Resources.ImportDataRowsLimitCappedDueWorksheetSpaceWarning
          : Resources.ImportDataRowsLimitCappedDueStartRowWarning
        :string.Empty;
      OptionsWarningPictureBox.Visible = show;
      OptionsWarningLabel.Visible = show;
    }

    /// <summary>
    /// Sets the anchors for some controls that for some reason can't be set at design time.
    /// </summary>
    private void SetAnchors()
    {
      PreviewDataGridView.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
    }

    /// <summary>
    /// Sets the import parameter values into the database object.
    /// This is needed before getting any data from it.
    /// </summary>
    private void SetImportParameterValues(int rowsCount)
    {
      _dbTableOrView.ImportParameters.AddSummaryRow = AddSummaryFieldsCheckBox.Checked;
      _dbTableOrView.ImportParameters.ColumnsNamesList = _importColumns;
      _dbTableOrView.ImportParameters.CreatePivotTable = CreatePivotTableCheckBox.Checked;
      _dbTableOrView.ImportParameters.FirstRowIndex = FirstRowIndex;
      _dbTableOrView.ImportParameters.IncludeColumnNames = IncludeHeadersCheckBox.Checked;
      _dbTableOrView.ImportParameters.IntoNewWorksheet = false;
      _dbTableOrView.ImportParameters.RowsCount = rowsCount;
    }
  }
}