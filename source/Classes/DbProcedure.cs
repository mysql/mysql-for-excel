// Copyright (c) 2014, Oracle and/or its affiliates. All rights reserved.
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
using System.Linq;
using System.Text;
using System.Windows.Forms;
using MySql.Data.MySqlClient;
using MySQL.ForExcel.Properties;
using MySQL.Utility.Classes;
using MySQL.Utility.Classes.MySQLWorkbench;
using MySQL.Utility.Forms;
using ExcelInterop = Microsoft.Office.Interop.Excel;

namespace MySQL.ForExcel.Classes
{
  public class DbProcedure : DbObject
  {
    /// <summary>
    /// The name of the table containing output parameters and return values.
    /// </summary>
    public const string OUT_AND_RETURN_VALUES_TABLE_NAME = "OutAndReturnValues";

    /// <summary>
    /// Initializes a new instance of the <see cref="DbProcedure"/> class.
    /// </summary>
    /// <param name="connection">The MySQL Workbench connection to a MySQL server instance selected by users.</param>
    /// <param name="name">The name of the MySQL database object.</param>
    public DbProcedure(MySqlWorkbenchConnection connection, string name)
      : base(connection, name)
    {
      ImportParameters = new ImportDataParams(name);
      Parameters = null;
    }

    /// <summary>
    /// Specifies identifiers to indicate the type of import for multiple result sets returned by a MySQL procedure.
    /// </summary>
    public enum ProcedureResultSetsImportType
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
    /// Gets the parameters used on Import Data operations.
    /// </summary>
    public ImportDataParams ImportParameters { get; private set; }

    /// <summary>
    /// A list of data type names and parameters for this stored procedure.
    /// </summary>
    public List<Tuple<string, MySqlParameter>> Parameters { get; private set; }

    /// <summary>
    /// A list of data type names and parameters for this stored procedure.
    /// </summary>
    public List<Tuple<string, MySqlParameter>> ReadOnlyParameters
    {
      get
      {
        return Parameters != null ? Parameters.Where(tuple => tuple.Item2.IsReadOnly()).ToList() : null;
      }
    }

    #endregion Properties

    /// <summary>
    /// Executes the given procedure and returns its result sets in tables within a <see cref="DataSet"/> object.
    /// </summary>
    /// <remarks>Only works against Procedures, but not with Tables or Views.</remarks>
    /// <returns><see cref="DataSet"/> where each table within it represents each of the result sets returned by the stored procedure.</returns>
    public DataSet Execute()
    {
      if (Parameters == null)
      {
        InitializeParameters();
      }

      if (Parameters == null)
      {
        return null;
      }

      string sql = string.Format("`{0}`.`{1}`", Connection.Schema, Name);
      var resultSetDs = Connection.ExecuteRoutine(sql, Parameters.Select(tuple => tuple.Item2).ToArray());
      if (resultSetDs == null || resultSetDs.Tables.Count == 0)
      {
        return null;
      }

      // Create result set dataset and MySqlDataTable tables for each table in the result sets
      var returnDataSet = new DataSet(Name + "ResultSet");
      var procedureSql = GetSql();
      int resultIndex = 1;
      foreach (DataTable table in resultSetDs.Tables)
      {
        table.TableName = string.Format("Result{0}", resultIndex);
        var mySqlDataTable = new MySqlDataTable(Connection, table, procedureSql, resultIndex - 1);
        returnDataSet.Tables.Add(mySqlDataTable);
        resultIndex++;
      }

      if (ReadOnlyParameters == null || ReadOnlyParameters.Count <= 0)
      {
        return returnDataSet;
      }

      // Create a table containing output parameters and return values
      var outParamsTable = new DataTable(OUT_AND_RETURN_VALUES_TABLE_NAME);
      foreach (var readOnlyTuple in ReadOnlyParameters)
      {
        var dataType = readOnlyTuple.Item1;
        var parameter = readOnlyTuple.Item2;
        outParamsTable.Columns.Add(new MySqlDataColumn(parameter.ParameterName, dataType, true));
      }

      // Add output/return parameter values to OutAndReturnValues table
      var valuesRow = outParamsTable.NewRow();
      valuesRow.ItemArray = ReadOnlyParameters.Select(tuple => tuple.Item2.Value).ToArray();
      outParamsTable.Rows.Add(valuesRow);
      var outParamsMySqlTable = new MySqlDataTable(Connection, outParamsTable, procedureSql, resultIndex - 1);
      returnDataSet.Tables.Add(outParamsMySqlTable);
      return returnDataSet;
    }

    /// <summary>
    /// Gets the SQL query text needed to call this stored procedure.
    /// </summary>
    /// <returns>The SQL query text needed to call this stored procedure.</returns>
    public string GetSql()
    {
      if (Parameters == null)
      {
        InitializeParameters();
      }

      var sqlCallBuilder = new StringBuilder();
      sqlCallBuilder.AppendFormat("CALL `{0}`.`{1}`(", Connection.Schema, Name);
      if (Parameters == null || Parameters.Count == 0)
      {
        sqlCallBuilder.Append(");");
        return sqlCallBuilder.ToString();
      }

      var sqlSetBuilder = new StringBuilder();
      var sqlSelectBuilder = new StringBuilder();
      for (int parameterIndex = 0; parameterIndex < Parameters.Count; parameterIndex++)
      {
        var parameter = Parameters[parameterIndex].Item2;
        switch (parameter.Direction)
        {
          case ParameterDirection.Input:
          case ParameterDirection.InputOutput:
            sqlSetBuilder.Append(parameter.GetSetStatement(parameterIndex == 0));
            break;

          case ParameterDirection.Output:
          case ParameterDirection.ReturnValue:
            sqlSelectBuilder.AppendFormat("{0} @{1}", sqlSelectBuilder.Length == 0 ? "SELECT" : ",", parameter.ParameterName);
            break;
        }

        sqlCallBuilder.AppendFormat("{0}@{1}", parameterIndex > 0 ? ", " : string.Empty, parameter.ParameterName);
      }

      sqlCallBuilder.Append("); ");
      if (sqlSelectBuilder.Length > 0)
      {
        sqlSelectBuilder.Append(";");
      }

      if (sqlSetBuilder.Length > 0)
      {
        sqlSetBuilder.Append("; ");
      }

      sqlSetBuilder.Append(sqlCallBuilder);
      sqlSetBuilder.Append(sqlSelectBuilder);
      return sqlSetBuilder.ToString();
    }

    /// <summary>
    /// Imports the result sets of this stored procedure to a <see cref="ExcelInterop.Worksheet"/>.
    /// </summary>
    /// <param name="importType">The <see cref="ProcedureResultSetsImportType"/> defining what result sets are imported and how they are laid out in the Excel worksheet. </param>
    /// <param name="selectedResultSetIndex">The index of the result set selected for import in case the <see cref="importType"/> is <see cref="ProcedureResultSetsImportType.SelectedResultSet"/>.</param>
    /// <param name="resultSetsDataSet">The <see cref="DataSet"/> containing all result sets returned by the stored procedure's execution.</param>
    /// <returns><c>true</c> if the import is successful, <c>false</c> otherwise.</returns>
    public bool ImportData(ProcedureResultSetsImportType importType, int selectedResultSetIndex, DataSet resultSetsDataSet = null)
    {
      if (resultSetsDataSet == null)
      {
        resultSetsDataSet = Execute();
      }

      bool success = true;
      try
      {
        var activeWorkbook = Globals.ThisAddIn.Application.ActiveWorkbook;

        // Check if the data being imported does not overlap with the data of an existing Excel table.
        if (DetectDataForImportPossibleCollisions(importType, selectedResultSetIndex, resultSetsDataSet))
        {
          if (InfoDialog.ShowYesNoDialog(InfoDialog.InfoType.Warning, Resources.ImportOverExcelObjectErrorTitle, Resources.ImportOverExcelObjectErrorDetail, Resources.ImportOverExcelObjectErrorSubDetail) == DialogResult.No)
          {
            return false;
          }

          var newWorkSheet = activeWorkbook.CreateWorksheet(Name, true);
          if (newWorkSheet == null)
          {
            return false;
          }
        }

        int tableIdx = 0;
        bool createPivotTable = ImportParameters.CreatePivotTable;
        bool addSummaryRow = ImportParameters.AddSummaryRow;
        ExcelInterop.Range nextTopLeftCell = Globals.ThisAddIn.Application.ActiveCell;
        foreach (var mySqlTable in resultSetsDataSet.Tables.Cast<MySqlDataTable>().Where(mySqlTable => importType != ProcedureResultSetsImportType.SelectedResultSet || selectedResultSetIndex == tableIdx++))
        {
          Globals.ThisAddIn.Application.Goto(nextTopLeftCell, false);
          mySqlTable.ImportColumnNames = ImportParameters.IncludeColumnNames;
          mySqlTable.TableName = Name + "." + mySqlTable.TableName;
          var excelObj = Settings.Default.ImportCreateExcelTable
            ? mySqlTable.ImportDataIntoExcelTable(createPivotTable, ExcelUtilities.PivotTablePosition.Right, addSummaryRow)
            : mySqlTable.ImportDataIntoExcelRange(createPivotTable, ExcelUtilities.PivotTablePosition.Right, addSummaryRow);
          if (excelObj == null)
          {
            continue;
          }

          var fillingRange = excelObj is ExcelInterop.ListObject
            ? (excelObj as ExcelInterop.ListObject).Range
            : excelObj as ExcelInterop.Range;
          nextTopLeftCell = fillingRange.GetNextResultSetTopLeftCell(importType, createPivotTable);
        }
      }
      catch (Exception ex)
      {
        success = false;
        MiscUtilities.ShowCustomizedErrorDialog(string.Format(Resources.UnableToRetrieveData, "procedure", Name), ex.Message);
        MySqlSourceTrace.WriteAppErrorToLog(ex);
      }

      return success;
    }

    /// <summary>
    /// Prepares the procedure parameters needed to call the MySQL procedure.
    /// </summary>
    public void InitializeParameters()
    {
      var parametersTable = Connection.GetSchemaCollection("Procedure Parameters", null, Connection.Schema, Name);
      if (parametersTable == null)
      {
        return;
      }

      var parametersCount = parametersTable.Rows.Count;
      Parameters = new List<Tuple<string, MySqlParameter>>(parametersCount);
      for (int paramIdx = 0; paramIdx < parametersCount; paramIdx++)
      {
        DataRow dr = parametersTable.Rows[paramIdx];
        string dataType = dr["DATA_TYPE"].ToString().ToLowerInvariant();
        string paramName = dr["PARAMETER_NAME"].ToString();
        ParameterDirection paramDirection = ParameterDirection.Input;
        int paramSize = dr["CHARACTER_MAXIMUM_LENGTH"] != null && dr["CHARACTER_MAXIMUM_LENGTH"] != DBNull.Value ? Convert.ToInt32(dr["CHARACTER_MAXIMUM_LENGTH"]) : 0;
        byte paramPrecision = dr["NUMERIC_PRECISION"] != null && dr["NUMERIC_PRECISION"] != DBNull.Value ? Convert.ToByte(dr["NUMERIC_PRECISION"]) : (byte)0;
        byte paramScale = dr["NUMERIC_SCALE"] != null && dr["NUMERIC_SCALE"] != DBNull.Value ? Convert.ToByte(dr["NUMERIC_SCALE"]) : (byte)0;
        bool paramUnsigned = dr["DTD_IDENTIFIER"].ToString().Contains("unsigned");
        string paramDirectionStr = paramName != "RETURN_VALUE" ? dr["PARAMETER_MODE"].ToString().ToLowerInvariant() : "return";

        switch (paramDirectionStr)
        {
          case "in":
            paramDirection = ParameterDirection.Input;
            break;

          case "out":
            paramDirection = ParameterDirection.Output;
            break;

          case "inout":
            paramDirection = ParameterDirection.InputOutput;
            break;

          case "return":
            paramDirection = ParameterDirection.ReturnValue;
            break;
        }

        object objValue;
        var dbType = DataTypeUtilities.GetMySqlDbType(dataType, paramUnsigned, paramPrecision, out objValue);
        Parameters.Add(new Tuple<string, MySqlParameter>(dataType, new MySqlParameter(paramName, dbType, paramSize, paramDirection, false, paramPrecision, paramScale, null, DataRowVersion.Current, objValue)));
      }
    }

    /// <summary>
    /// Releases all resources used by the <see cref="DbTable"/> class
    /// </summary>
    /// <param name="disposing">If true this is called by Dispose(), otherwise it is called by the finalizer</param>
    protected override void Dispose(bool disposing)
    {
      if (Disposed)
      {
        return;
      }

      // Free managed resources
      if (disposing)
      {
        ImportParameters = null;
        if (Parameters != null)
        {
          Parameters.Clear();
          Parameters = null;
        }
      }

      base.Dispose(disposing);
    }

    /// <summary>
    /// Checks if any of the <see cref="ExcelInterop.Range"/>s where result sets returned by this procedure's execution are imported would collide with another Excel object.
    /// </summary>
    /// <param name="importType">The <see cref="ProcedureResultSetsImportType"/> defining what result sets are imported and how they are laid out in the Excel worksheet. </param>
    /// <param name="selectedResultSetIndex">The index of the result set selected for import in case the <see cref="importType"/> is <see cref="ProcedureResultSetsImportType.SelectedResultSet"/>.</param>
    /// <param name="resultSetsDataSet">The <see cref="DataSet"/> containing all result sets returned by the stored procedure's execution.</param>
    /// <returns><c>true</c> if any of the <see cref="ExcelInterop.Range"/>s where result sets returned by this procedure's execution are imported would collide with another Excel object, <c>false</c> otherwise.</returns>
    private bool DetectDataForImportPossibleCollisions(ProcedureResultSetsImportType importType, int selectedResultSetIndex, DataSet resultSetsDataSet)
    {
      if (resultSetsDataSet == null)
      {
        return false;
      }

      bool createPivotTable = ImportParameters.CreatePivotTable;
      bool collisionDetected = false;
      var atCell = Globals.ThisAddIn.Application.ActiveCell;
      int tableIdx = 0;
      var pivotPosition = importType == ProcedureResultSetsImportType.AllResultSetsHorizontally
        ? ExcelUtilities.PivotTablePosition.Below
        : ExcelUtilities.PivotTablePosition.Right;
      foreach (var mySqlTable in resultSetsDataSet.Tables.Cast<MySqlDataTable>().Where(mySqlTable => importType != ProcedureResultSetsImportType.SelectedResultSet || selectedResultSetIndex == tableIdx++))
      {
        var ranges = mySqlTable.GetExcelRangesToOccupy(atCell, ImportParameters.AddSummaryRow, ImportParameters.CreatePivotTable, pivotPosition);
        if (ranges == null)
        {
          continue;
        }

        collisionDetected = ranges.Aggregate(false, (current, range) => current || range.IntersectsWithAnyExcelObject());
        if (collisionDetected)
        {
          break;
        }

        atCell = ranges[0].GetNextResultSetTopLeftCell(importType, createPivotTable);
      }

      return collisionDetected;
    }
  }
}
