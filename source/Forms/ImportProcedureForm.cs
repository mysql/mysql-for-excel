// Copyright (c) 2012-2014, Oracle and/or its affiliates. All rights reserved.
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
using MySql.Data.MySqlClient;
using MySQL.ForExcel.Classes;
using MySQL.ForExcel.Properties;
using MySQL.Utility.Classes;
using MySQL.Utility.Classes.MySQLWorkbench;
using MySQL.Utility.Forms;
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
    private DbObject _dbObject;

    /// <summary>
    /// Array of parameters for the selected MySQL procedure.
    /// </summary>
    private MySqlParameter[] _mysqlParameters;

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
    /// The connection to a MySQL server instance selected by users.
    /// </summary>
    private MySqlWorkbenchConnection _wbConnection;

    /// <summary>
    /// A value indicating whether the Excel workbook where data will be imported is open in compatibility mode.
    /// </summary>
    private readonly bool _workbookInCompatibilityMode;

    #endregion Fields

    /// <summary>
    /// Initializes a new instance of the <see cref="ImportProcedureForm"/> class.
    /// </summary>
    /// <param name="wbConnection">The connection to a MySQL server instance selected by users.</param>
    /// <param name="dbObject">The Procedure DB object selected by the users to import data from.</param>
    /// <param name="importToWorksheetName">The name of the Excel worksheet where data will be imported.</param>
    /// <param name="workbookInCompatibilityMode">Flag indicating whether the Excel workbook where data will be imported is open in compatibility mode.</param>
    public ImportProcedureForm(MySqlWorkbenchConnection wbConnection, DbObject dbObject, string importToWorksheetName, bool workbookInCompatibilityMode)
    {
      _dbObject = dbObject;
      _previewDataSet = null;
      _procedureParamsProperties = new PropertiesCollection();
      _selectedResultSetIndex = -1;
      _sumOfResultSetsExceedsMaxCompatibilityRows = false;
      _wbConnection = wbConnection;
      _workbookInCompatibilityMode = workbookInCompatibilityMode;

      InitializeComponent();

      Text = @"Import Data - " + importToWorksheetName;
      ProcedureNameLabel.Text = dbObject.Name;
      OptionsWarningLabel.Text = Resources.WorkbookInCompatibilityModeWarning;
      ParametersPropertyGrid.SelectedObject = _procedureParamsProperties;
      AddSummaryFieldsCheckBox.Enabled = Settings.Default.ImportCreateExcelTable;

      InitializeMultipleResultSetsCombo();
      PrepareParameters();
    }

    /// <summary>
    /// Specifies identifiers to indicate the type of import for multiple result sets returned by a MySQL procedure.
    /// </summary>
    private enum ImportMultipleType
    {
      /// <summary>
      /// Only the result seet selected by users is imported.
      /// </summary>
      SelectedResultSet,

      /// <summary>
      /// All result sets returned by the procedure are imported and arranged horizontally in the Excel worksheet.
      /// </summary>
      AllResultSetsHorizontally,

      /// <summary>
      /// All result sets returned by the procedure are imported and arranged vertically in the Excel worksheet.
      /// </summary>
      AllResultSetsVertically
    }

    #region Properties

    /// <summary>
    /// Gets or sets a value indicating whether <see cref="ExcelInterop.PivotTable"/> objects are created for each imported result set.
    /// </summary>
    public bool CreatePivotTables
    {
      get
      {
        return CreatePivotTableCheckBox.Checked;
      }

      set
      {
        CreatePivotTableCheckBox.Checked = value;
      }
    }

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
    /// Gets or sets a value indicating whether column names are imported as the first data row in the Excel worksheet.
    /// </summary>
    private bool ImportColumnNames
    {
      get
      {
        return IncludeHeadersCheckBox.Checked;
      }

      set
      {
        IncludeHeadersCheckBox.Checked = value;
      }
    }

    /// <summary>
    /// Gets the import type selected by users.
    /// </summary>
    private ImportMultipleType ImportType
    {
      get
      {
        var retType = ImportMultipleType.SelectedResultSet;
        int multTypeValue = ImportResultsetsComboBox != null && ImportResultsetsComboBox.Items.Count > 0 ? (int)ImportResultsetsComboBox.SelectedValue : 0;
        switch (multTypeValue)
        {
          case 0:
            retType = ImportMultipleType.SelectedResultSet;
            break;

          case 1:
            retType = ImportMultipleType.AllResultSetsHorizontally;
            break;

          case 2:
            retType = ImportMultipleType.AllResultSetsVertically;
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
        // Prepare parameters and execute the procedure and create OutAndReturnValues table
        var outParamsTable = new MySqlDataTable(_wbConnection, "OutAndReturnValues");
        for (int paramIdx = 0; paramIdx < _procedureParamsProperties.Count; paramIdx++)
        {
          _mysqlParameters[paramIdx].Value = _procedureParamsProperties[paramIdx].Value;
          if (_mysqlParameters[paramIdx].Direction == ParameterDirection.Output ||
              _mysqlParameters[paramIdx].Direction == ParameterDirection.ReturnValue)
          {
            outParamsTable.Columns.Add(new MySqlDataColumn(
            _procedureParamsProperties[paramIdx].Name,
            _procedureParamsProperties[paramIdx].Value.GetType().GetMySqlDataType(),
            true));
          }
        }

        var resultSetDs = _wbConnection.GetDataSetFromProcedure(_dbObject, _mysqlParameters);
        if (resultSetDs == null || resultSetDs.Tables.Count == 0)
        {
          ImportButton.Enabled = false;
          return;
        }

        // Clear or create result set dataset.
        ImportButton.Enabled = true;
        if (_importDataSet == null)
        {
          _importDataSet = new DataSet(_dbObject.Name + "ResultSet");
        }
        else
        {
          _importDataSet.Tables.Clear();
        }

        // Create MySqlDataTable tables for each table in the result sets
        int resultIndex = 1;
        foreach (DataTable table in resultSetDs.Tables)
        {
          table.TableName = string.Format("Result{0}", resultIndex++);
          var mySqlDataTable = new MySqlDataTable(_wbConnection, table);
          _importDataSet.Tables.Add(mySqlDataTable);
        }

        // Refresh output/return parameter values in PropertyGrid and add them to OutAndReturnValues table
        if (outParamsTable.Columns.Count > 0)
        {
          DataRow valuesRow = outParamsTable.NewRow();
          for (int paramIdx = 0; paramIdx < _procedureParamsProperties.Count; paramIdx++)
          {
            if (_mysqlParameters[paramIdx].Direction != ParameterDirection.Output &&
                _mysqlParameters[paramIdx].Direction != ParameterDirection.ReturnValue)
            {
              continue;
            }

            _procedureParamsProperties[paramIdx].Value = _mysqlParameters[paramIdx].Value;
            valuesRow[_mysqlParameters[paramIdx].ParameterName] = _mysqlParameters[paramIdx].Value;
          }

          outParamsTable.Rows.Add(valuesRow);
          _importDataSet.Tables.Add(outParamsTable);
          ParametersPropertyGrid.Refresh();
        }

        // Prepare Preview DataSet to show it on Grids
        _previewDataSet = _importDataSet.Clone();
        int resultSetsRowSum = 0;
        for (int tableIdx = 0; tableIdx < _importDataSet.Tables.Count; tableIdx++)
        {
          resultSetsRowSum += _importDataSet.Tables[tableIdx].Rows.Count;
          if (_workbookInCompatibilityMode)
          {
            _sumOfResultSetsExceedsMaxCompatibilityRows = _sumOfResultSetsExceedsMaxCompatibilityRows ||
                                                          resultSetsRowSum > UInt16.MaxValue;
          }

          int limitRows = Math.Min(_importDataSet.Tables[tableIdx].Rows.Count,
            Settings.Default.ImportPreviewRowsQuantity);
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
        MiscUtilities.ShowCustomizedErrorDialog(Resources.ImportProcedureErrorTitle, ex.Message, true);
        MySqlSourceTrace.WriteAppErrorToLog(ex);
      }
      finally
      {
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
        MiscUtilities.ShowCustomizedErrorDialog(string.Format(Resources.UnableToRetrieveData, _dbObject.Type.ToString().ToLowerInvariant(), _dbObject.Name));
        return false;
      }

      if (_sumOfResultSetsExceedsMaxCompatibilityRows && ImportType == ImportMultipleType.AllResultSetsVertically && _importDataSet.Tables.Count > 1)
      {
        InfoDialog.ShowWarningDialog(Resources.ImportVerticallyExceedsMaxRowsTitleWarning, Resources.ImportVerticallyExceedsMaxRowsDetailWarning);
      }

      bool success = true;
      try
      {
        Cursor = Cursors.WaitCursor;
        var atCell = Globals.ThisAddIn.Application.ActiveCell;
        var activeWorkbook = Globals.ThisAddIn.Application.ActiveWorkbook;
        int tableIdx = 0;
        var pivotPosition = ImportType == ImportMultipleType.AllResultSetsHorizontally
          ? MySqlDataTable.PivotTablePosition.Below
          : MySqlDataTable.PivotTablePosition.Right;
        foreach (MySqlDataTable mySqlTable in _importDataSet.Tables)
        {
          mySqlTable.ImportColumnNames = ImportColumnNames;
          mySqlTable.TableName = _dbObject.Name + "." + mySqlTable.TableName;
          if (ImportType == ImportMultipleType.SelectedResultSet && _selectedResultSetIndex < tableIdx)
          {
            continue;
          }

          tableIdx++;
          var excelObj = mySqlTable.ImportDataAtActiveExcelCell(Settings.Default.ImportCreateExcelTable, CreatePivotTables, pivotPosition, AddSummaryFieldsCheckBox.Checked);
          if (excelObj == null)
          {
            continue;
          }

          var fillingRange = excelObj is ExcelInterop.ListObject
            ? (excelObj as ExcelInterop.ListObject).Range
            : excelObj as ExcelInterop.Range;
          ExcelInterop.Range endCell;
          if (fillingRange != null)
          {
            endCell = fillingRange.Cells[fillingRange.Rows.Count, fillingRange.Columns.Count] as ExcelInterop.Range;
          }
          else
          {
            continue;
          }

          if (endCell == null || tableIdx >= _importDataSet.Tables.Count)
          {
            continue;
          }

          switch (ImportType)
          {
            case ImportMultipleType.AllResultSetsHorizontally:
              atCell = endCell.Offset[atCell.Row - endCell.Row, 2];
              break;

            case ImportMultipleType.AllResultSetsVertically:
              if (activeWorkbook.Excel8CompatibilityMode && endCell.Row + 2 > UInt16.MaxValue)
              {
                return true;
              }

              atCell = endCell.Offset[2, atCell.Column - endCell.Column];
              break;
          }

          Globals.ThisAddIn.Application.Goto(atCell, false);
        }
      }
      catch (Exception ex)
      {
        success = false;
        MiscUtilities.ShowCustomizedErrorDialog(string.Format(Resources.UnableToRetrieveData, _dbObject.Type.ToString().ToLowerInvariant(), _dbObject.Name), ex.Message);
        MySqlSourceTrace.WriteAppErrorToLog(ex);
      }
      finally
      {
        Cursor = Cursors.Default;
      }

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
    /// Initializes the result sets combo box with the different import options.
    /// </summary>
    private void InitializeMultipleResultSetsCombo()
    {
      var dt = new DataTable();
      dt.Columns.Add("Value", Type.GetType("System.Int32"));
      dt.Columns.Add("Description");
      dt.Rows.Add(new object[] { ImportMultipleType.SelectedResultSet, Resources.ImportProcedureSelectedResultSet });
      dt.Rows.Add(new object[] { ImportMultipleType.AllResultSetsHorizontally, Resources.ImportProcedureAllResultSetsHorizontally });
      dt.Rows.Add(new object[] { ImportMultipleType.AllResultSetsVertically, Resources.ImportProcedureAllResultSetsVertically });
      ImportResultsetsComboBox.DataSource = dt;
      ImportResultsetsComboBox.DisplayMember = "Description";
      ImportResultsetsComboBox.ValueMember = "Value";
    }

    /// <summary>
    /// Prepares the procedure parameters needed to call the MySQL procedure.
    /// </summary>
    private void PrepareParameters()
    {
      DataTable parametersTable = _wbConnection.GetSchemaCollection("Procedure Parameters", null, _wbConnection.Schema, _dbObject.Name);
      _mysqlParameters = new MySqlParameter[parametersTable.Rows.Count];
      int paramIdx = 0;
      MySqlDbType dbType = MySqlDbType.Guid;
      object objValue = null;

      foreach (DataRow dr in parametersTable.Rows)
      {
        string dataType = dr["DATA_TYPE"].ToString().ToLowerInvariant();
        string paramName = dr["PARAMETER_NAME"].ToString();
        ParameterDirection paramDirection = ParameterDirection.Input;
        int paramSize = dr["CHARACTER_MAXIMUM_LENGTH"] != null && dr["CHARACTER_MAXIMUM_LENGTH"] != DBNull.Value ? Convert.ToInt32(dr["CHARACTER_MAXIMUM_LENGTH"]) : 0;
        byte paramPrecision = dr["NUMERIC_PRECISION"] != null && dr["NUMERIC_PRECISION"] != DBNull.Value ? Convert.ToByte(dr["NUMERIC_PRECISION"]) : (byte)0;
        byte paramScale = dr["NUMERIC_SCALE"] != null && dr["NUMERIC_SCALE"] != DBNull.Value ? Convert.ToByte(dr["NUMERIC_SCALE"]) : (byte)0;
        bool paramUnsigned = dr["DTD_IDENTIFIER"].ToString().Contains("unsigned");
        string paramDirectionStr = paramName != "RETURN_VALUE" ? dr["PARAMETER_MODE"].ToString().ToLowerInvariant() : "return";
        bool paramIsReadOnly = false;

        switch (paramDirectionStr)
        {
          case "in":
            paramDirection = ParameterDirection.Input;
            paramIsReadOnly = false;
            break;

          case "out":
            paramDirection = ParameterDirection.Output;
            paramIsReadOnly = true;
            break;

          case "inout":
            paramDirection = ParameterDirection.InputOutput;
            paramIsReadOnly = false;
            break;

          case "return":
            paramDirection = ParameterDirection.ReturnValue;
            paramIsReadOnly = true;
            break;
        }

        switch (dataType)
        {
          case "bit":
            dbType = MySqlDbType.Bit;
            if (paramPrecision > 1)
            {
              objValue = 0;
            }
            else
            {
              objValue = false;
            }
            break;

          case "int":
          case "integer":
            dbType = MySqlDbType.Int32;
            objValue = 0;
            break;

          case "tinyint":
            dbType = paramUnsigned ? MySqlDbType.UByte : MySqlDbType.Byte;
            objValue = (Byte)0;
            break;

          case "smallint":
            dbType = paramUnsigned ? MySqlDbType.UInt16 : MySqlDbType.Int16;
            objValue = (Int16)0;
            break;

          case "mediumint":
            dbType = paramUnsigned ? MySqlDbType.UInt24 : MySqlDbType.Int24;
            objValue = 0;
            break;

          case "bigint":
            dbType = paramUnsigned ? MySqlDbType.UInt64 : MySqlDbType.Int64;
            objValue = (Int64)0;
            break;

          case "numeric":
          case "decimal":
          case "float":
          case "double":
          case "real":
            dbType = dataType == "float" ? MySqlDbType.Float : (dataType == "double" || dataType == "real" ? MySqlDbType.Double : MySqlDbType.Decimal);
            objValue = (Double)0;
            break;

          case "char":
          case "varchar":
            dbType = MySqlDbType.VarChar;
            objValue = string.Empty;
            break;

          case "binary":
          case "varbinary":
            dbType = dataType.StartsWith("var") ? MySqlDbType.VarBinary : MySqlDbType.Binary;
            objValue = string.Empty;
            break;

          case "text":
          case "tinytext":
          case "mediumtext":
          case "longtext":
            dbType = dataType.StartsWith("var") ? MySqlDbType.VarBinary : MySqlDbType.Binary;
            objValue = string.Empty;
            break;

          case "date":
          case "datetime":
          case "timestamp":
            dbType = dataType == "date" ? MySqlDbType.Date : MySqlDbType.DateTime;
            objValue = DateTime.Today;
            break;

          case "time":
            dbType = MySqlDbType.Time;
            objValue = TimeSpan.Zero;
            break;

          case "blob":
            dbType = MySqlDbType.Blob;
            objValue = null;
            break;
        }

        CustomProperty parameter = new CustomProperty(paramName, objValue, paramIsReadOnly, true)
        {
          Description = string.Format("Direction: {0}, Data Type: {1}", paramDirection.ToString(), dataType)
        };
        _mysqlParameters[paramIdx] = new MySqlParameter(paramName, dbType, paramSize, paramDirection, false, paramPrecision, paramScale, null, DataRowVersion.Current, objValue);
        _procedureParamsProperties.Add(parameter);
        paramIdx++;
      }

      FieldInfo fi = ParametersPropertyGrid.GetType().GetField("gridView", BindingFlags.NonPublic | BindingFlags.Instance);
      object gridViewRef = fi.GetValue(ParametersPropertyGrid);
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