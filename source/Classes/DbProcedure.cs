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
using MySql.Data.MySqlClient;
using MySQL.ForExcel.Properties;
using MySQL.Utility.Classes;
using MySQL.Utility.Classes.MySQLWorkbench;
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
      int resultIndex = 1;
      foreach (DataTable table in resultSetDs.Tables)
      {
        table.TableName = string.Format("Result{0}", resultIndex++);
        var mySqlDataTable = new MySqlDataTable(Connection, table);
        returnDataSet.Tables.Add(mySqlDataTable);
      }

      if (ReadOnlyParameters == null || ReadOnlyParameters.Count <= 0)
      {
        return returnDataSet;
      }

      // Create a table containing output parameters and return values
      var outParamsTable = new MySqlDataTable(Connection, OUT_AND_RETURN_VALUES_TABLE_NAME);
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
      returnDataSet.Tables.Add(outParamsTable);
      return returnDataSet;
    }

    /// <summary>
    /// Imports the result sets of this stored procedure to a <see cref="ExcelInterop.Worksheet"/>.
    /// </summary>
    /// <param name="importType"></param>
    /// <param name="selectedResultSetIndex"></param>
    /// <param name="resultSetsDataSet"></param>
    /// <returns></returns>
    public bool ImportData(ProcedureResultSetsImportType importType, int selectedResultSetIndex, DataSet resultSetsDataSet = null)
    {
      if (resultSetsDataSet == null)
      {
        resultSetsDataSet = Execute();
      }

      bool success = true;
      try
      {
        var atCell = Globals.ThisAddIn.Application.ActiveCell;
        var activeWorkbook = Globals.ThisAddIn.Application.ActiveWorkbook;
        int tableIdx = 0;
        var pivotPosition = importType == ProcedureResultSetsImportType.AllResultSetsHorizontally
          ? MySqlDataTable.PivotTablePosition.Below
          : MySqlDataTable.PivotTablePosition.Right;
        foreach (MySqlDataTable mySqlTable in resultSetsDataSet.Tables)
        {
          mySqlTable.ImportColumnNames = ImportParameters.IncludeColumnNames;
          mySqlTable.TableName = Name + "." + mySqlTable.TableName;
          if (importType == ProcedureResultSetsImportType.SelectedResultSet && selectedResultSetIndex < tableIdx)
          {
            continue;
          }

          tableIdx++;
          var excelObj = mySqlTable.ImportDataAtActiveExcelCell(Settings.Default.ImportCreateExcelTable, ImportParameters.CreatePivotTable, pivotPosition, ImportParameters.AddSummaryFields);
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

          if (endCell == null || tableIdx >= resultSetsDataSet.Tables.Count)
          {
            continue;
          }

          switch (importType)
          {
            case ProcedureResultSetsImportType.AllResultSetsHorizontally:
              atCell = endCell.Offset[atCell.Row - endCell.Row, 2];
              break;

            case ProcedureResultSetsImportType.AllResultSetsVertically:
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
  }
}
