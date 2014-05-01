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

namespace MySQL.ForExcel.Forms
{
  /// <summary>
  /// Previews the results of a procedure and lets users select rows to import to an Excel spreadsheet.
  /// </summary>
  public partial class ImportProcedureForm : AutoStyleableBaseDialog
  {
    #region Fields

    /// <summary>
    /// Array of parameters for the selected MySQL procedure.
    /// </summary>
    private MySqlParameter[] _mysqlParameters;

    /// <summary>
    /// Collection of properties of the MySQL procedure's parameters.
    /// </summary>
    private readonly PropertiesCollection _procedureParamsProperties;

    #endregion Fields

    /// <summary>
    /// Initializes a new instance of the <see cref="ImportProcedureForm"/> class.
    /// </summary>
    /// <param name="wbConnection">The connection to a MySQL server instance selected by users.</param>
    /// <param name="importDbObject">The Procedure DB object selected by the users to import data from.</param>
    /// <param name="importToWorksheetName">The name of the Excel worksheet where data will be imported.</param>
    /// <param name="workbookInCompatibilityMode">Flag indicating whether the Excel workbook where data will be imported is open in compatibility mode.</param>
    public ImportProcedureForm(MySqlWorkbenchConnection wbConnection, DbObject importDbObject, string importToWorksheetName, bool workbookInCompatibilityMode)
    {
      ImportDbObject = importDbObject;
      PreviewDataSet = null;
      SumOfResultSetsExceedsMaxCompatibilityRows = false;
      WbConnection = wbConnection;
      WorkbookInCompatibilityMode = workbookInCompatibilityMode;

      InitializeComponent();

      SelectedResultSetIndex = -1;
      Text = @"Import Data - " + importToWorksheetName;
      _procedureParamsProperties = new PropertiesCollection();
      ProcedureNameLabel.Text = importDbObject.Name;
      OptionsWarningLabel.Text = Resources.WorkbookInCompatibilityModeWarning;
      ParametersPropertyGrid.SelectedObject = _procedureParamsProperties;

      InitializeMultipleResultSetsCombo();
      PrepareParameters();
      IncludeHeadersCheckBox.Checked = true;
    }

    /// <summary>
    /// Specifies identifiers to indicate the type of import for multiple result sets returned by a MySQL procedure.
    /// </summary>
    public enum ImportMultipleType
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
    /// Gets the <see cref="DataSet"/> containing the tables with all the result sets returned by the MySQL procedure.
    /// </summary>
    public DataSet ImportDataSet { get; private set; }

    /// <summary>
    /// Get a value indicating whether column names are imported as the first data row in the Excel worksheet.
    /// </summary>
    public bool ImportHeaders
    {
      get
      {
        return IncludeHeadersCheckBox.Checked;
      }
    }

    /// <summary>
    /// Gets the Procedure DB object selected by the users to import data from.
    /// </summary>
    public DbObject ImportDbObject { get; private set; }

    /// <summary>
    /// Gets the import type selected by users.
    /// </summary>
    public ImportMultipleType ImportType
    {
      get
      {
        ImportMultipleType retType = ImportMultipleType.SelectedResultSet;
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

    /// <summary>
    /// Gets the <see cref="DataSet"/> with a subset of data to import from the procedure's result set to show in the preview grid.
    /// </summary>
    public DataSet PreviewDataSet { get; private set; }

    /// <summary>
    /// Gets the index of the table representing the result set selected by users.
    /// </summary>
    public int SelectedResultSetIndex { get; private set; }

    /// <summary>
    /// Gets a value indicating whether the sum of rows in all result sets returned by the procedure exceeds
    /// the maximum number of rows in an Excel worksheet open in compatibility mode.
    /// </summary>
    public bool SumOfResultSetsExceedsMaxCompatibilityRows { get; private set; }

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
    /// Gets the connection to a MySQL server instance selected by users.
    /// </summary>
    public MySqlWorkbenchConnection WbConnection { get; private set; }

    /// <summary>
    /// Gets a value indicating whether the Excel workbook where data will be imported is open in compatibility mode.
    /// </summary>
    public bool WorkbookInCompatibilityMode { get; private set; }

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
        MySqlDataTable outParamsTable = new MySqlDataTable(WbConnection.Schema, "OutAndReturnValues");
        for (int paramIdx = 0; paramIdx < _procedureParamsProperties.Count; paramIdx++)
        {
          _mysqlParameters[paramIdx].Value = _procedureParamsProperties[paramIdx].Value;
          if (_mysqlParameters[paramIdx].Direction == ParameterDirection.Output || _mysqlParameters[paramIdx].Direction == ParameterDirection.ReturnValue)
          {
            outParamsTable.Columns.Add(_procedureParamsProperties[paramIdx].Name, _procedureParamsProperties[paramIdx].Value.GetType());
          }
        }

        var resultSetDs = WbConnection.GetDataSetFromProcedure(ImportDbObject, _mysqlParameters);
        if (resultSetDs == null || resultSetDs.Tables.Count == 0)
        {
          ImportButton.Enabled = false;
          return;
        }

        // Clear or create result set dataset.
        ImportButton.Enabled = true;
        if (ImportDataSet == null)
        {
          ImportDataSet = new DataSet(ImportDbObject.Name + "ResultSet");
        }
        else
        {
          ImportDataSet.Tables.Clear();
        }

        // Create MySqlDataTable tables for each table in the result sets
        int resultIndex = 1;
        foreach (DataTable table in resultSetDs.Tables)
        {
          table.TableName = string.Format("Result{0}", resultIndex++);
          var mySqlDataTable = new MySqlDataTable(table, WbConnection.Schema);
          ImportDataSet.Tables.Add(mySqlDataTable);
        }

        // Refresh output/return parameter values in PropertyGrid and add them to OutAndReturnValues table
        if (outParamsTable.Columns.Count > 0)
        {
          DataRow valuesRow = outParamsTable.NewRow();
          for (int paramIdx = 0; paramIdx < _procedureParamsProperties.Count; paramIdx++)
          {
            if (_mysqlParameters[paramIdx].Direction != ParameterDirection.Output && _mysqlParameters[paramIdx].Direction != ParameterDirection.ReturnValue)
            {
              continue;
            }

            _procedureParamsProperties[paramIdx].Value = _mysqlParameters[paramIdx].Value;
            valuesRow[_mysqlParameters[paramIdx].ParameterName] = _mysqlParameters[paramIdx].Value;
          }

          outParamsTable.Rows.Add(valuesRow);
          ImportDataSet.Tables.Add(outParamsTable);
          ParametersPropertyGrid.Refresh();
        }

        // Prepare Preview DataSet to show it on Grids
        PreviewDataSet = ImportDataSet.Clone();
        int resultSetsRowSum = 0;
        for (int tableIdx = 0; tableIdx < ImportDataSet.Tables.Count; tableIdx++)
        {
          resultSetsRowSum += ImportDataSet.Tables[tableIdx].Rows.Count;
          if (WorkbookInCompatibilityMode)
          {
            SumOfResultSetsExceedsMaxCompatibilityRows = SumOfResultSetsExceedsMaxCompatibilityRows || resultSetsRowSum > UInt16.MaxValue;
          }

          int limitRows = Math.Min(ImportDataSet.Tables[tableIdx].Rows.Count, Settings.Default.ImportPreviewRowsQuantity);
          for (int rowIdx = 0; rowIdx < limitRows; rowIdx++)
          {
            PreviewDataSet.Tables[tableIdx].ImportRow(ImportDataSet.Tables[tableIdx].Rows[rowIdx]);
          }
        }

        // Refresh ResultSets in Tab Control
        ResultSetsDataGridView.DataSource = null;
        ResultSetsTabControl.TabPages.Clear();
        for (int dtIdx = 0; dtIdx < ImportDataSet.Tables.Count; dtIdx++)
        {
          ResultSetsTabControl.TabPages.Add(ImportDataSet.Tables[dtIdx].TableName);
        }

        if (ResultSetsTabControl.TabPages.Count > 0)
        {
          SelectedResultSetIndex = ResultSetsTabControl.SelectedIndex = 0;
          ResultSetsTabControl_SelectedIndexChanged(ResultSetsTabControl, EventArgs.Empty);
        }

        Cursor = Cursors.Default;
      }
      catch (Exception ex)
      {
        Cursor = Cursors.Default;
        MiscUtilities.ShowCustomizedErrorDialog(Resources.ImportProcedureErrorTitle, ex.Message, true);
        MySqlSourceTrace.WriteAppErrorToLog(ex);
      }
    }

    /// <summary>
    /// Event delegate method fired when the <see cref="ImportButton"/> button is clicked.
    /// </summary>
    /// <param name="sender">Sender object.</param>
    /// <param name="e">Event arguments.</param>
    private void ImportButton_Click(object sender, EventArgs e)
    {
      foreach (MySqlDataTable table in ImportDataSet.Tables)
      {
        table.TableName = ImportDbObject.Name + "." + table.TableName;
      }

      if (SumOfResultSetsExceedsMaxCompatibilityRows && ImportType == ImportMultipleType.AllResultSetsVertically && ImportDataSet.Tables.Count > 1)
      {
        InfoDialog.ShowWarningDialog(Resources.ImportVerticallyExceedsMaxRowsTitleWarning, Resources.ImportVerticallyExceedsMaxRowsDetailWarning);
      }
    }

    /// <summary>
    /// Initializes the result sets combo box with the different import options.
    /// </summary>
    private void InitializeMultipleResultSetsCombo()
    {
      DataTable dt = new DataTable();
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
      DataTable parametersTable = WbConnection.GetSchemaCollection("Procedure Parameters", null, WbConnection.Schema, ImportDbObject.Name);
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

      SelectedResultSetIndex = ResultSetsTabControl.SelectedIndex;
      ResultSetsTabControl.TabPages[SelectedResultSetIndex].Controls.Add(ResultSetsDataGridView);
      ResultSetsDataGridView.Dock = DockStyle.Fill;
      ResultSetsDataGridView.SelectionMode = DataGridViewSelectionMode.CellSelect;
      if (ResultSetsDataGridView.DataSource == null)
      {
        ResultSetsDataGridView.DataSource = PreviewDataSet;
      }

      ResultSetsDataGridView.DataMember = PreviewDataSet.Tables[SelectedResultSetIndex].TableName;
      bool cappingAtMaxCompatRows = WorkbookInCompatibilityMode && ImportDataSet.Tables[SelectedResultSetIndex].Rows.Count > UInt16.MaxValue;
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