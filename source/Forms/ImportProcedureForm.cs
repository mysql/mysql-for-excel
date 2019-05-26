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
using System.Data;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;
using MySql.Data.MySqlClient;
using MySQL.ForExcel.Classes;
using MySQL.ForExcel.Properties;
using MySql.Utility.Classes;
using MySql.Utility.Classes.Logging;
using MySql.Utility.Classes.Spatial;
using MySql.Utility.Forms;
using MySQL.ForExcel.Controls;
using ExcelInterop = Microsoft.Office.Interop.Excel;

namespace MySQL.ForExcel.Forms
{
  /// <summary>
  /// Previews the results of a procedure and lets users select rows to import to an Excel spreadsheet.
  /// </summary>
  public partial class ImportProcedureForm : AutoStyleableBaseDialog
  {
    #region Fields

    /// <summary>
    /// The Procedure DB object selected by the users to import data from.
    /// </summary>
    private readonly DbProcedure _dbProcedure;

    /// <summary>
    /// The <see cref="DataSet"/> containing the tables with all the result sets returned by the MySQL procedure.
    /// </summary>
    private DataSet _importDataSet;

    /// <summary>
    /// Information of the rows that can be imported to the active worksheet.
    /// </summary>
    private List<ImportingRowsInfo> _importingRowsInfo;

    /// <summary>
    /// The <see cref="DataSet"/> with a subset of data to import from the procedure's result set to show in the preview grid.
    /// </summary>
    private DataSet _previewDataSet;

    /// <summary>
    /// The <see cref="DataGridViewCellStyle"/> used for regular cells.
    /// </summary>
    private DataGridViewCellStyle _previewGridCellsStyle;

    /// <summary>
    /// The <see cref="DataGridViewCellStyle"/> used for column headers.
    /// </summary>
    private DataGridViewCellStyle _previewGridColumnHeaderCellsStyle;

    /// <summary>
    /// Collection of properties of the MySQL procedure's parameters.
    /// </summary>
    private readonly PropertiesCollection _procedureParamsProperties;

    /// <summary>
    /// The index of the table representing the result set selected by users.
    /// </summary>
    private int _selectedResultSetIndex;

    #endregion Fields

    /// <summary>
    /// Initializes a new instance of the <see cref="ImportProcedureForm"/> class.
    /// </summary>
    /// <param name="dbProcedure">The Procedure DB object selected by the users to import data from.</param>
    /// <param name="importToWorksheetName">The name of the Excel worksheet where data will be imported.</param>
    public ImportProcedureForm(DbProcedure dbProcedure, string importToWorksheetName)
    {
      _dbProcedure = dbProcedure ?? throw new ArgumentNullException(nameof(dbProcedure));
      _previewDataSet = null;
      _procedureParamsProperties = new PropertiesCollection();
      _selectedResultSetIndex = -1;

      InitializeComponent();

      Text = @"Import Data - " + importToWorksheetName;
      ProcedureNameLabel.Text = dbProcedure.Name;
      OptionsWarningLabel.Text = Resources.ImportDataWillBeTruncatedWarning;
      ParametersPropertyGrid.SelectedObject = _procedureParamsProperties;
      AddSummaryFieldsCheckBox.Enabled = Settings.Default.ImportCreateExcelTable;

      ImportResultsetsComboBox.InitializeComboBoxFromEnum(DbProcedure.ProcedureResultSetsImportType.SelectedResultSet);
      PrepareParameters();
      InitializeCellStyles();
    }

    #region Properties

    /// <summary>
    /// Gets or sets the text associated with this control.
    /// </summary>
    public sealed override string Text
    {
      get => base.Text;
      set => base.Text = value;
    }

    /// <summary>
    /// Gets the import type selected by users.
    /// </summary>
    private DbProcedure.ProcedureResultSetsImportType ProcedureResultSetsImportType
    {
      get
      {
        var retType = DbProcedure.ProcedureResultSetsImportType.SelectedResultSet;
        var multTypeValue = ImportResultsetsComboBox != null && ImportResultsetsComboBox.Items.Count > 0 ? (int)ImportResultsetsComboBox.SelectedValue : 0;
        switch (multTypeValue)
        {
          case 0:
            retType = DbProcedure.ProcedureResultSetsImportType.SelectedResultSet;
            break;

          case 1:
            retType = DbProcedure.ProcedureResultSetsImportType.AllResultSetsHorizontally;
            break;

          case 2:
            retType = DbProcedure.ProcedureResultSetsImportType.AllResultSetsVertically;
            break;
        }

        return retType;
      }
    }

    #endregion Properties

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
        AddSummaryFieldsCheckBox.Checked = Settings.Default.ImportCreateExcelTable && AddSummaryFieldsCheckBox.Checked;
        AddSummaryFieldsCheckBox.Enabled = Settings.Default.ImportCreateExcelTable;
      }
    }

    /// <summary>
    /// Event delegate method fired when the <see cref="CallButton"/> button is clicked.
    /// </summary>
    /// <param name="sender">Sender object.</param>
    /// <param name="e">Event arguments.</param>
    private void CallButton_Click(object sender, EventArgs e)
    {
      Cursor = Cursors.WaitCursor;
      try
      {
        if (_dbProcedure.Parameters == null)
        {
          _dbProcedure.InitializeParameters();
        }

        // Fill procedure parameter values
        for (var paramIdx = 0; paramIdx < _procedureParamsProperties.Count; paramIdx++)
        {
          var parameter = _dbProcedure.Parameters[paramIdx].Item2;
          var parameterValue = _procedureParamsProperties[paramIdx].Value;
          if (_dbProcedure.Parameters[paramIdx].Item1.Equals("geometry", StringComparison.OrdinalIgnoreCase)
              && parameter.MySqlDbType == MySqlDbType.Blob)
          {
            // Spatial data
            var textValue = parameterValue?.ToString();
            if (string.IsNullOrEmpty(textValue))
            {
              parameterValue = null;
            }
            else
            {
              var geometry = Geometry.Parse(textValue, Globals.ThisAddIn.SpatialDataAsTextFormat);
              parameterValue = geometry == null
                ? null
                : WkbHandler.GetBinaryWkbFromGeometry(geometry, WkbHandler.DefaultByteOrder);
            }
          }

          parameter.Value = parameterValue;
        }

        // Call stored procedure
        _importDataSet = _dbProcedure.Execute();
        if (_importDataSet == null || _importDataSet.Tables.Count == 0)
        {
          ImportButton.Enabled = false;
          return;
        }

        // Refresh output/return parameter values in PropertyGrid
        for (var paramIdx = 0; paramIdx < _procedureParamsProperties.Count; paramIdx++)
        {
          var parameter = _dbProcedure.Parameters[paramIdx].Item2;
          if (!parameter.IsReadOnly())
          {
            continue;
          }

          _procedureParamsProperties[paramIdx].Value = parameter.Value;
        }

        ParametersPropertyGrid.Refresh();

        // Fetch importing rows information
        _importingRowsInfo = ImportingRowsInfo.FromDataSet(_importDataSet, IncludeHeadersCheckBox.Checked, AddSummaryFieldsCheckBox.Checked);

        // Prepare Preview DataSet to show it on Grids
        _previewDataSet = _importDataSet.Clone();
        for (var tableIdx = 0; tableIdx < _importDataSet.Tables.Count; tableIdx++)
        {
          var limitRows = Math.Min(_importDataSet.Tables[tableIdx].Rows.Count, Settings.Default.ImportPreviewRowsQuantity);
          for (var rowIdx = 0; rowIdx < limitRows; rowIdx++)
          {
            _previewDataSet.Tables[tableIdx].ImportRow(_importDataSet.Tables[tableIdx].Rows[rowIdx]);
          }
        }

        // Refresh ResultSets in Tab Control
        ResultSetsTabControl.TabPages.Cast<TabPage>().ToList().ForEach(tabPage => tabPage.Controls.OfType<DataGridView>().FirstOrDefault()?.Dispose());
        ResultSetsTabControl.TabPages.Clear();
        for (var dtIdx = 0; dtIdx < _importDataSet.Tables.Count; dtIdx++)
        {
          var tableName = _previewDataSet.Tables[dtIdx].TableName;
          var tabPage = new TabPage(tableName);
          var previewDataGridView = CreateResultSetDataGridView(tableName);
          tabPage.Controls.Add(previewDataGridView);
          ResultSetsTabControl.TabPages.Add(tabPage);
          previewDataGridView.Fill(_previewDataSet.Tables[dtIdx]);
        }

        if (ResultSetsTabControl.TabPages.Count > 0)
        {
          ResultSetsTabControl_SelectedIndexChanged(ResultSetsTabControl, EventArgs.Empty);
        }
      }
      catch (Exception ex)
      {
        Logger.LogException(ex, true, Resources.ImportProcedureErrorTitle);
      }
      finally
      {
        ImportButton.Enabled = true;
        Cursor = Cursors.Default;
      }
    }

    /// <summary>
    /// Creates a new <see cref="PreviewDataGridView"/> with the default preview styles.
    /// </summary>
    /// <param name="name">The name of the grid view.</param>
    /// <returns>A new <see cref="PreviewDataGridView"/>.</returns>
    private PreviewDataGridView CreateResultSetDataGridView(string name)
    {
      var resultSetDataGridView = new PreviewDataGridView
      {
        ColumnHeadersDefaultCellStyle = _previewGridColumnHeaderCellsStyle,
        ColumnsMaximumWidth = 200,
        ColumnsMinimumWidth = 5,
        DefaultCellStyle = _previewGridCellsStyle,
        Name = name,
        TabIndex = 9,
        Dock = DockStyle.Fill,
        ContextMenuStrip = GridContextMenuStrip,
        SelectAllAfterBindingComplete = true
      };
      resultSetDataGridView.SelectionChanged += PreviewDataGridViewSelectionChanged;
      return resultSetDataGridView;
    }

    /// <summary>
    /// Imports the selected MySQL procedure's result sets into the active <see cref="ExcelInterop.Worksheet"/>.
    /// </summary>
    private bool ImportData()
    {
      if (_importDataSet == null)
      {
        MiscUtilities.ShowCustomizedErrorDialog(string.Format(Resources.UnableToRetrieveData, "procedure", _dbProcedure.Name));
        return false;
      }

      Cursor = Cursors.WaitCursor;

      foreach (TabPage tabPage in ResultSetsTabControl.TabPages)
      {
        var previewGrid = tabPage.Controls.OfType<PreviewDataGridView>().FirstOrDefault();
        if (previewGrid == null
            || previewGrid.SelectedColumns.Count == 0
            || previewGrid.SelectedColumns.Count == previewGrid.Columns.Count
            || !(_importDataSet.Tables[tabPage.TabIndex] is MySqlDataTable mySqlTable))
        {
          continue;
        }

        foreach (MySqlDataColumn mySqlColumn in mySqlTable.Columns)
        {
          mySqlColumn.ExcludeColumn = !previewGrid.Columns[mySqlColumn.Ordinal].Selected;
        }
      }

      // Refresh import parameter values
      _dbProcedure.ImportParameters.AddSummaryRow = AddSummaryFieldsCheckBox.Checked;
      _dbProcedure.ImportParameters.CreatePivotTable = CreatePivotTableCheckBox.Checked;
      _dbProcedure.ImportParameters.IncludeColumnNames = IncludeHeadersCheckBox.Checked;
      _dbProcedure.ImportParameters.IntoNewWorksheet = false;

      // Import the result sets into Excel
      var success = _dbProcedure.ImportData(ProcedureResultSetsImportType, _selectedResultSetIndex, _importDataSet);

      Cursor = Cursors.Default;
      return success;
    }

    /// <summary>
    /// Event delegate method fired when the <see cref="ImportProcedureForm"/> is being closed.
    /// </summary>
    /// <param name="sender">Sender object.</param>
    /// <param name="e">Event arguments.</param>
    private void ImportProcedureForm_FormClosing(object sender, FormClosingEventArgs e)
    {
      if (DialogResult == DialogResult.OK)
      {
        e.Cancel = !ImportData();
      }
    }

    /// <summary>
    /// Event delegate method fired when the <see cref="ImportProcedureForm"/> is loaded.
    /// </summary>
    /// <param name="sender">Sender object.</param>
    /// <param name="e">Event arguments.</param>
    private void ImportProcedureForm_Load(object sender, EventArgs e)
    {
      ResetPropertyGridSplitterPosition();
    }

    /// <summary>
    /// Event delegate method fired when the <see cref="ImportResultsetsComboBox"/> selected index changes.
    /// </summary>
    /// <param name="sender">Sender object.</param>
    /// <param name="e">Event arguments.</param>
    private void ImportResultsetsComboBox_SelectedIndexChanged(object sender, EventArgs e)
    {
      SetCompatibilityWarningControlsVisibility();
    }

    /// <summary>
    /// Initializes the styles used for the grid view controls holding result sets.
    /// </summary>
    private void InitializeCellStyles()
    {
      if (_previewGridColumnHeaderCellsStyle == null)
      {
        _previewGridColumnHeaderCellsStyle = new DataGridViewCellStyle
        {
          Alignment = DataGridViewContentAlignment.MiddleCenter,
          BackColor = System.Drawing.SystemColors.InactiveCaption,
          Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0),
          ForeColor = System.Drawing.SystemColors.InactiveCaptionText,
          SelectionBackColor = System.Drawing.SystemColors.Control,
          SelectionForeColor = System.Drawing.SystemColors.ControlText,
          WrapMode = DataGridViewTriState.False
        };
      }

      if (_previewGridCellsStyle == null)
      {
        _previewGridCellsStyle = new DataGridViewCellStyle
        {
          Alignment = DataGridViewContentAlignment.MiddleLeft,
          BackColor = System.Drawing.SystemColors.InactiveCaption,
          Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0),
          ForeColor = System.Drawing.SystemColors.ControlText,
          SelectionBackColor = System.Drawing.SystemColors.Window,
          SelectionForeColor = System.Drawing.SystemColors.ControlText,
          WrapMode = DataGridViewTriState.False
        };
      }
    }

    /// <summary>
    /// Prepares the procedure parameters needed to call the MySQL procedure.
    /// </summary>
    private void PrepareParameters()
    {
      _dbProcedure.InitializeParameters();
      foreach (var dataTypeAndParameterTuple in _dbProcedure.Parameters)
      {
        var dataType = dataTypeAndParameterTuple.Item1;
        var parameter = dataTypeAndParameterTuple.Item2;
        var customProperty = new CustomProperty(parameter.ParameterName, dataType, parameter.Value, parameter.IsReadOnly(), true)
        {
          Description = $"Direction: {parameter.Direction}, Data Type: {dataType}"
        };

        _procedureParamsProperties.Add(customProperty);
      }
    }

    /// <summary>
    /// Event delegate method fired when a <see cref="PreviewDataGridView"/> changes selection.
    /// </summary>
    /// <param name="sender">Sender object.</param>
    /// <param name="e">Event arguments.</param>
    private void PreviewDataGridViewSelectionChanged(object sender, EventArgs e)
    {
      ImportButton.Enabled = ResultSetsTabControl.TabPages.Cast<TabPage>().ToList().Sum(tabPage => tabPage.Controls.OfType<DataGridView>().Sum(grid => grid.SelectedColumns.Count)) > 0;
    }

    /// <summary>
    /// Resets the position of the splitter dividing parameter names and their values.
    /// </summary>
    private void ResetPropertyGridSplitterPosition()
    {
      var methodInfo = typeof(PropertyGrid).GetMethod("GetPropertyGridView", BindingFlags.NonPublic | BindingFlags.Instance);
      var gridView = methodInfo?.Invoke(ParametersPropertyGrid, new object[] { });
      methodInfo = gridView?.GetType().GetMethod("MoveSplitterTo", BindingFlags.NonPublic | BindingFlags.Instance);
      var parametersTextWidth = _dbProcedure.GetMaxParameterNameLength(ParametersPropertyGrid.Font);
      var newPosition = parametersTextWidth + 30;
      methodInfo?.Invoke(gridView, new object[] { newPosition });
    }

    /// <summary>
    /// Event delegate method fired when the <see cref="ResultSetsTabControl"/> selected tab index changes.
    /// </summary>
    /// <param name="sender">Sender object.</param>
    /// <param name="e">Event arguments.</param>
    private void ResultSetsTabControl_SelectedIndexChanged(object sender, EventArgs e)
    {
      if (ResultSetsTabControl.SelectedIndex < 0)
      {
        return;
      }

      _selectedResultSetIndex = ResultSetsTabControl.SelectedIndex;
      RowsCountSubLabel.Text = _importingRowsInfo[_selectedResultSetIndex].RowsCount.ToString(CultureInfo.CurrentCulture);
      SetCompatibilityWarningControlsVisibility();
    }

    /// <summary>
    /// Event delegate method fired when the <see cref="SelectAllToolStripMenuItem"/> context menu item is clicked.
    /// </summary>
    /// <param name="sender">Sender object.</param>
    /// <param name="e">Event arguments.</param>
    private void SelectAllToolStripMenuItem_Click(object sender, EventArgs e)
    {
      if (!(sender is ToolStripMenuItem toolStripMenuItem))
      {
        return;
      }

      if (!(toolStripMenuItem.Owner is ContextMenuStrip contextMenuStrip))
      {
        return;
      }

      if (!(contextMenuStrip.SourceControl is PreviewDataGridView previewDataGridView))
      {
        return;
      }

      previewDataGridView.SelectAll();
    }

    /// <summary>
    /// Event delegate method fired when the <see cref="SelectNoneToolStripMenuItem"/> context menu item is clicked.
    /// </summary>
    /// <param name="sender">Sender object.</param>
    /// <param name="e">Event arguments.</param>
    private void SelectNoneToolStripMenuItem_Click(object sender, EventArgs e)
    {
      if (!(sender is ToolStripMenuItem toolStripMenuItem))
      {
        return;
      }

      if (!(toolStripMenuItem.Owner is ContextMenuStrip contextMenuStrip))
      {
        return;
      }

      if (!(contextMenuStrip.SourceControl is PreviewDataGridView previewDataGridView))
      {
        return;
      }

      previewDataGridView.ClearSelection();
    }

    /// <summary>
    /// Shows or hides the compatibility warning controls to let the users know if the rows to be imported exceed the limit of rows of the current Excel Worksheet.
    /// </summary>
    private void SetCompatibilityWarningControlsVisibility()
    {
      long totalRowsCount = 0;
      if (_importingRowsInfo != null)
      {
        switch (ProcedureResultSetsImportType)
        {
          case DbProcedure.ProcedureResultSetsImportType.AllResultSetsHorizontally:
            totalRowsCount = _importingRowsInfo.Max(info => info.RowsCount);
            break;

          case DbProcedure.ProcedureResultSetsImportType.AllResultSetsVertically:
            totalRowsCount = _importingRowsInfo.Sum(info => info.RowsCount) + _importingRowsInfo.Count - 1;
            break;

          case DbProcedure.ProcedureResultSetsImportType.SelectedResultSet:
            totalRowsCount = _importingRowsInfo[ResultSetsTabControl.SelectedIndex].RowsCount;
            break;
        }
      }

      var show = ExcelUtilities.CheckIfRowsExceedWorksheetLimit(totalRowsCount);
      OptionsWarningPictureBox.Visible = show;
      OptionsWarningLabel.Visible = show;
    }
  }
}