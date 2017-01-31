// Copyright (c) 2012, 2017, Oracle and/or its affiliates. All rights reserved.
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
using System.Reflection;
using System.Windows.Forms;
using MySQL.ForExcel.Classes;
using MySQL.ForExcel.Properties;
using MySql.Utility.Classes;
using MySql.Utility.Classes.MySql;
using MySql.Utility.Forms;
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
    /// The <see cref="DataSet"/> containing the tables with all the result sets returned by the MySQL procedure.
    /// </summary>
    private DataSet _importDataSet;

    /// <summary>
    /// The Procedure DB object selected by the users to import data from.
    /// </summary>
    private DbProcedure _dbProcedure;

    /// <summary>
    /// The <see cref="DataSet"/> with a subset of data to import from the procedure's result set to show in the preview grid.
    /// </summary>
    private DataSet _previewDataSet;

    /// <summary>
    /// Collection of properties of the MySQL procedure's parameters.
    /// </summary>
    private readonly PropertiesCollection _procedureParamsProperties;

    /// <summary>
    /// The index of the table representing the result set selected by users.
    /// </summary>
    private int _selectedResultSetIndex;

    /// <summary>
    /// A value indicating whether the sum of rows in all result sets returned by the procedure exceeds the maximum number of rows in an Excel worksheet open in compatibility mode.
    /// </summary>
    private bool _sumOfResultSetsExceedsMaxCompatibilityRows;

    /// <summary>
    /// A value indicating whether the Excel workbook where data will be imported is open in compatibility mode.
    /// </summary>
    private readonly bool _workbookInCompatibilityMode;

    #endregion Fields

    /// <summary>
    /// Initializes a new instance of the <see cref="ImportProcedureForm"/> class.
    /// </summary>
    /// <param name="dbProcedure">The Procedure DB object selected by the users to import data from.</param>
    /// <param name="importToWorksheetName">The name of the Excel worksheet where data will be imported.</param>
    public ImportProcedureForm(DbProcedure dbProcedure, string importToWorksheetName)
    {
      if (dbProcedure == null)
      {
        throw new ArgumentNullException(nameof(dbProcedure));
      }

      _dbProcedure = dbProcedure;
      _previewDataSet = null;
      _procedureParamsProperties = new PropertiesCollection();
      _selectedResultSetIndex = -1;
      _sumOfResultSetsExceedsMaxCompatibilityRows = false;
      _workbookInCompatibilityMode = Globals.ThisAddIn.ActiveWorkbook.Excel8CompatibilityMode;

      InitializeComponent();

      Text = @"Import Data - " + importToWorksheetName;
      ProcedureNameLabel.Text = dbProcedure.Name;
      OptionsWarningLabel.Text = Resources.ImportDataWillBeTruncatedWarning;
      ParametersPropertyGrid.SelectedObject = _procedureParamsProperties;
      AddSummaryFieldsCheckBox.Enabled = Settings.Default.ImportCreateExcelTable;

      ImportResultsetsComboBox.InitializeComboBoxFromEnum(DbProcedure.ProcedureResultSetsImportType.SelectedResultSet);
      PrepareParameters();
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

    /// <summary>
    /// Gets the import type selected by users.
    /// </summary>
    private DbProcedure.ProcedureResultSetsImportType ProcedureResultSetsImportType
    {
      get
      {
        var retType = DbProcedure.ProcedureResultSetsImportType.SelectedResultSet;
        int multTypeValue = ImportResultsetsComboBox != null && ImportResultsetsComboBox.Items.Count > 0 ? (int)ImportResultsetsComboBox.SelectedValue : 0;
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
        for (int paramIdx = 0; paramIdx < _procedureParamsProperties.Count; paramIdx++)
        {
          var parameter = _dbProcedure.Parameters[paramIdx].Item2;
          parameter.Value = _procedureParamsProperties[paramIdx].Value;
        }

        // Call stored procedure
        _importDataSet = _dbProcedure.Execute();
        if (_importDataSet == null || _importDataSet.Tables.Count == 0)
        {
          ImportButton.Enabled = false;
          return;
        }

        // Refresh output/return parameter values in PropertyGrid
        for (int paramIdx = 0; paramIdx < _procedureParamsProperties.Count; paramIdx++)
        {
          var parameter = _dbProcedure.Parameters[paramIdx].Item2;
          if (!parameter.IsReadOnly())
          {
            continue;
          }

          _procedureParamsProperties[paramIdx].Value = parameter.Value;
        }

        ParametersPropertyGrid.Refresh();

        // Prepare Preview DataSet to show it on Grids
        _previewDataSet = _importDataSet.Clone();
        int resultSetsRowSum = 0;
        for (int tableIdx = 0; tableIdx < _importDataSet.Tables.Count; tableIdx++)
        {
          resultSetsRowSum += _importDataSet.Tables[tableIdx].Rows.Count;
          if (_workbookInCompatibilityMode)
          {
            _sumOfResultSetsExceedsMaxCompatibilityRows = _sumOfResultSetsExceedsMaxCompatibilityRows || resultSetsRowSum > UInt16.MaxValue;
          }

          int limitRows = Math.Min(_importDataSet.Tables[tableIdx].Rows.Count, Settings.Default.ImportPreviewRowsQuantity);
          for (int rowIdx = 0; rowIdx < limitRows; rowIdx++)
          {
            _previewDataSet.Tables[tableIdx].ImportRow(_importDataSet.Tables[tableIdx].Rows[rowIdx]);
          }
        }

        // Refresh ResultSets in Tab Control
        ResultSetsDataGridView.DataSource = null;
        ResultSetsTabControl.TabPages.Clear();
        for (int dtIdx = 0; dtIdx < _importDataSet.Tables.Count; dtIdx++)
        {
          ResultSetsTabControl.TabPages.Add(_importDataSet.Tables[dtIdx].TableName);
        }

        if (ResultSetsTabControl.TabPages.Count > 0)
        {
          _selectedResultSetIndex = ResultSetsTabControl.SelectedIndex = 0;
          ResultSetsTabControl_SelectedIndexChanged(ResultSetsTabControl, EventArgs.Empty);
        }
      }
      catch (Exception ex)
      {
        MySqlSourceTrace.WriteAppErrorToLog(ex, null, Resources.ImportProcedureErrorTitle, true);
      }
      finally
      {
        ImportButton.Enabled = true;
        Cursor = Cursors.Default;
      }
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

      if (_sumOfResultSetsExceedsMaxCompatibilityRows && ProcedureResultSetsImportType == DbProcedure.ProcedureResultSetsImportType.AllResultSetsVertically && _importDataSet.Tables.Count > 1)
      {
        InfoDialog.ShowDialog(InfoDialogProperties.GetWarningDialogProperties(
          Resources.ImportVerticallyExceedsMaxRowsTitleWarning,
          Resources.ImportVerticallyExceedsMaxRowsDetailWarning));
      }

      Cursor = Cursors.WaitCursor;

      // Refresh import parameter values
      _dbProcedure.ImportParameters.AddSummaryRow = AddSummaryFieldsCheckBox.Checked;
      _dbProcedure.ImportParameters.CreatePivotTable = CreatePivotTableCheckBox.Checked;
      _dbProcedure.ImportParameters.IncludeColumnNames = IncludeHeadersCheckBox.Checked;
      _dbProcedure.ImportParameters.IntoNewWorksheet = false;

      // Import the result sets into Excel
      bool success = _dbProcedure.ImportData(ProcedureResultSetsImportType, _selectedResultSetIndex, _importDataSet);

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
          Description = string.Format("Direction: {0}, Data Type: {1}", parameter.Direction, dataType)
        };

        _procedureParamsProperties.Add(customProperty);
      }

      FieldInfo fi = ParametersPropertyGrid.GetType().GetField("gridView", BindingFlags.NonPublic | BindingFlags.Instance);
      object gridViewRef = fi != null ? fi.GetValue(ParametersPropertyGrid) : ParametersPropertyGrid;
      Type gridViewType = gridViewRef.GetType();
      MethodInfo mi = gridViewType.GetMethod("MoveSplitterTo", BindingFlags.NonPublic | BindingFlags.Instance);
      int gridColWidth = (int)Math.Truncate(ParametersPropertyGrid.Width * 0.4);
      mi.Invoke(gridViewRef, new object[] { gridColWidth });
      ParametersPropertyGrid.Refresh();
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
      ResultSetsTabControl.TabPages[_selectedResultSetIndex].Controls.Add(ResultSetsDataGridView);
      ResultSetsDataGridView.Dock = DockStyle.Fill;
      ResultSetsDataGridView.SelectionMode = DataGridViewSelectionMode.CellSelect;
      if (ResultSetsDataGridView.DataSource == null)
      {
        ResultSetsDataGridView.DataSource = _previewDataSet;
      }

      ResultSetsDataGridView.DataMember = _previewDataSet.Tables[_selectedResultSetIndex].TableName;
      bool cappingAtMaxCompatRows = _workbookInCompatibilityMode && _importDataSet.Tables[_selectedResultSetIndex].Rows.Count > UInt16.MaxValue;
      SetCompatibilityWarning(cappingAtMaxCompatRows);
      foreach (DataGridViewColumn gridCol in ResultSetsDataGridView.Columns)
      {
        gridCol.SortMode = DataGridViewColumnSortMode.NotSortable;
      }

      ResultSetsDataGridView.SelectionMode = DataGridViewSelectionMode.FullColumnSelect;
    }

    /// <summary>
    /// Shows or hides the Excel worksheet in compatibility mode warning controls.
    /// </summary>
    /// <param name="show"></param>
    private void SetCompatibilityWarning(bool show)
    {
      OptionsWarningLabel.Visible = show;
      OptionsWarningPictureBox.Visible = show;
    }
  }
}