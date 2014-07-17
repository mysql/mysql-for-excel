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
using System.Globalization;
using System.Linq;
using System.Text;
using MySql.Data.MySqlClient;
using MySQL.ForExcel.Properties;
using MySQL.Utility.Classes;
using MySQL.Utility.Classes.MySQLWorkbench;
using ExcelInterop = Microsoft.Office.Interop.Excel;

namespace MySQL.ForExcel.Classes
{
  /// <summary>
  /// Represents a MySQL View that MySQL for Excel can interact with.
  /// </summary>
  public class DbView : DbObject
  {
    /// <summary>
    /// Initializes a new instance of the <see cref="DbView"/> class.
    /// </summary>
    /// <param name="connection">The MySQL Workbench connection to a MySQL server instance selected by users.</param>
    /// <param name="name">The name of the MySQL database object.</param>
    public DbView(MySqlWorkbenchConnection connection, string name)
      : base(connection, name)
    {
      ImportParameters = new ImportDataParams(name);
    }

    #region Properties

    /// <summary>
    /// Gets the parameters used on Import Data operations.
    /// </summary>
    public ImportDataParams ImportParameters { get; private set; }

    #endregion Properties

    /// <summary>
    /// Gets a list of column names contained within this object.
    /// </summary>
    /// <returns>A list of column names contained within this object.</returns>
    public List<string> GetColumnNamesList()
    {
      if (Connection == null)
      {
        return null;
      }

      var columnsInfoTable = Connection.GetSchemaCollection("Columns", null, Connection.Schema, Name);
      if (columnsInfoTable == null)
      {
        return null;
      }

      var columnsList = new List<string>(columnsInfoTable.Rows.Count);
      columnsList.AddRange(from DataRow dr in columnsInfoTable.Rows select dr["COLUMN_NAME"].ToString());
      return columnsList;
    }

    /// <summary>
    /// Fetches the data from the corresponding MySQL object and places it in a <see cref="DataTable"/> object.
    /// </summary>
    /// <returns><see cref="DataTable"/> containing the results of the query.</returns>
    public DataTable GetData()
    {
      if (Connection == null)
      {
        return null;
      }

      string queryString = GetSelectQuery();
      return string.IsNullOrEmpty(queryString) ? null : Connection.GetDataFromSelectQuery(queryString);
    }

    /// <summary>
    /// Gets a <see cref="MySqlDataTable"/> filled with data for this <see cref="DbObject"/>.
    /// </summary>
    /// <returns>A <see cref="MySqlDataTable"/> filled with data for this <see cref="DbObject"/>.</returns>
    public MySqlDataTable GetMySqlDataTable()
    {
      if (Connection == null)
      {
        return null;
      }

      var selectQuery = GetSelectQuery();
      var operationType = ImportParameters.ForEditDataOperation
        ? MySqlDataTable.DataOperationType.Edit
        : MySqlDataTable.DataOperationType.ImportTableOrView;
      return Connection.CreateImportMySqlTable(operationType, ImportParameters.DbObjectName, ImportParameters.IncludeColumnNames, selectQuery);
    }

    /// <summary>
    /// Gets the total number of rows contained in the corresponding MySQL object.
    /// </summary>
    /// <returns>The number of rows in a given table or view.</returns>
    public long GetRowsCount()
    {
      if (Connection == null)
      {
        return 0;
      }

      string sql = string.Format("SELECT COUNT(*) FROM `{0}`.`{1}`", Connection.Schema, Name);
      object objCount = MySqlHelper.ExecuteScalar(Connection.GetConnectionStringBuilder().ConnectionString, sql);
      return objCount != null ? (long)objCount : 0;
    }

    /// <summary>
    /// Creates a SELECT query against this database object.
    /// </summary>
    /// <returns>The SELECT query text.</returns>
    public string GetSelectQuery()
    {
      if (Connection == null)
      {
        return string.Empty;
      }

      const string bigRowCountLimit = "18446744073709551615";
      var queryStringBuilder = new StringBuilder("SELECT ");
      if (ImportParameters.ColumnsNamesList == null || ImportParameters.ColumnsNamesList.Count == 0)
      {
        queryStringBuilder.Append("*");
      }
      else
      {
        foreach (string columnName in ImportParameters.ColumnsNamesList)
        {
          queryStringBuilder.AppendFormat("`{0}`,", columnName.Replace("`", "``"));
        }

        queryStringBuilder.Remove(queryStringBuilder.Length - 1, 1);
      }

      queryStringBuilder.AppendFormat(" FROM `{0}`.`{1}`", Connection.Schema, Name);
      if (ImportParameters.FirstRowIndex > 0)
      {
        string strCount = ImportParameters.RowsCount >= 0 ? ImportParameters.RowsCount.ToString(CultureInfo.InvariantCulture) : bigRowCountLimit;
        queryStringBuilder.AppendFormat(" LIMIT {0},{1}", ImportParameters.FirstRowIndex, strCount);
      }
      else if (ImportParameters.RowsCount >= 0)
      {
        queryStringBuilder.AppendFormat(" LIMIT {0}", ImportParameters.RowsCount);
      }

      var returnString = queryStringBuilder.ToString();
      return returnString;
    }

    /// <summary>
    /// Imports the data of this <see cref="DbView"/> to a <see cref="ExcelInterop.Worksheet"/>.
    /// </summary>
    /// <returns>A <see cref="Tuple"/> containing a <see cref="MySqlDataTable"/> filled with data for this <see cref="DbView"/> and either a <see cref="ExcelInterop.ListObject"/> or a <see cref="ExcelInterop.Range"/> where the data was imported to.</returns>
    public Tuple<MySqlDataTable, object> ImportData()
    {
      Tuple<MySqlDataTable, object> retTuple;
      try
      {
        // Create the MySqlDataTable that holds the data to be imported to Excel
        var mySqlTable = GetMySqlDataTable();
        object excelTableOrRange = null;
        if (mySqlTable == null)
        {
          return null;
        }

        // Create a new Excel Worksheet and import the table/view data there
        if (ImportParameters.IntoNewWorksheet)
        {
          var currentWorksheet = ActiveWorkbook.CreateWorksheet(mySqlTable.TableName, true);
          if (currentWorksheet == null)
          {
            return null;
          }
        }

        if (!ImportParameters.ForEditDataOperation)
        {
          excelTableOrRange = Settings.Default.ImportCreateExcelTable
            ? mySqlTable.ImportDataIntoExcelTable(ImportParameters.CreatePivotTable, ImportParameters.PivotTablePosition, ImportParameters.AddSummaryRow)
            : mySqlTable.ImportDataIntoExcelRange(ImportParameters.CreatePivotTable, ImportParameters.PivotTablePosition, ImportParameters.AddSummaryRow);
        }

        retTuple = new Tuple<MySqlDataTable, object>(mySqlTable, excelTableOrRange);
      }
      catch (Exception ex)
      {
        retTuple = null;
        MiscUtilities.ShowCustomizedErrorDialog(string.Format(Resources.UnableToRetrieveData, this is DbTable ? "table" : "view", Name), ex.Message);
        MySqlSourceTrace.WriteAppErrorToLog(ex);
      }

      return retTuple;
    }
  }
}
