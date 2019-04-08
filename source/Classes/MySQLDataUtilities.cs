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
using System.Linq;
using System.Text;
using MySql.Data.MySqlClient;
using MySql.Utility.Classes.Logging;
using MySQL.ForExcel.Interfaces;
using MySQL.ForExcel.Properties;
using MySql.Utility.Classes.MySqlWorkbench;
using MySql.Utility.Enums;
using ExcelInterop = Microsoft.Office.Interop.Excel;

namespace MySQL.ForExcel.Classes
{
  /// <summary>
  /// Provides extension methods and other static methods to leverage the work with MySQL data.
  /// </summary>
  public static class MySqlDataUtilities
  {
    #region Constants

    /// <summary>
    /// The default name given to newly created schemas.
    /// </summary>
    public const string DEFAULT_NEW_SCHEMA_NAME = "new_schema";

    /// <summary>
    /// The OLEDB connection string used for <see cref="ExcelInterop.WorkbookConnection"/> instances that connect to MySQL Server instances.
    /// </summary>
    public const string OLEDB_MYSQL_CONNECTION_STRING_FULL = OLEDB_MYSQL_CONNECTION_STRING_STATIC + "Server={0};Port={1};Database={2};User={3};Option=3;";

    /// <summary>
    /// The OLEDB connection string used for <see cref="ExcelInterop.WorkbookConnection"/> instances that connect to MySQL Server instances.
    /// </summary>
    public const string OLEDB_MYSQL_CONNECTION_STRING_STATIC = "OLEDB;Driver={{MySQL ODBC 5.3 ANSI Driver}};Provider=MSDASQL;";

    #endregion Constants

    /// <summary>
    /// Verifies if a <see cref="MySqlConnection"/> is open and has not been disconnected by the server.
    /// </summary>
    /// <param name="connection">A <see cref="MySqlConnection"/> object.</param>
    /// <returns><c>true</c> if the connection is open and has not been disconnected by the server, <c>false</c> otherwise.</returns>
    public static bool CheckIfOpenAndNotDisconnected(this MySqlConnection connection)
    {
      return connection != null && (connection.State == ConnectionState.Open || connection.Ping());
    }

    /// <summary>
    /// Converts the <see cref="float"/> columns in a <see cref="DataTable"/> to <see cref="decimal"/> to workaround the problems inherent to floating point precision.
    /// </summary>
    /// <param name="dataTable">A <see cref="DataTable"/> instance.</param>
    /// <returns>A clone of the original <see cref="DataTable"/> with all <see cref="float"/> columns converted to <see cref="decimal"/>.</returns>
    public static DataTable ConvertApproximateFloatingPointDataTypeColumnsToExact(this DataTable dataTable)
    {
      if (dataTable == null)
      {
        return null;
      }

      if (!dataTable.Columns.Cast<DataColumn>().Any(col => col.DataType.IsApproximateFloatingPointDataType()))
      {
        // Nothing to convert.
        return dataTable;
      }

      var newTable = dataTable.Clone();
      for (int columnIndex = 0; columnIndex < dataTable.Columns.Count; columnIndex++)
      {
        if (!newTable.Columns[columnIndex].DataType.IsApproximateFloatingPointDataType())
        {
          continue;
        }

        newTable.Columns[columnIndex].DataType = typeof(decimal);
      }

      newTable.AcceptChanges();
      foreach (DataRow row in dataTable.Rows)
      {
        newTable.ImportRow(row);
      }

      return newTable;
    }

    /// <summary>
    /// Creates the import my SQL table.
    /// </summary>
    /// <param name="wbConnection">The wb connection.</param>
    /// <param name="operationType">The <see cref="MySqlDataTable.DataOperationType"/> intended for the new <see cref="MySqlDataTable"/>.</param>
    /// <param name="tableOrViewName">The name of the MySQL table or view to import data from..</param>
    /// <param name="importColumnNames">Flag indicating if column names will be imported as the first row of imported data.</param>
    /// <param name="selectQuery">A SELECT query against a database object to fill the [MySqlDataTable] return object with.</param>
    /// <param name="procedureResultSetIndex">The index of the result set of a stored procedure this table contains data for.</param>
    /// <returns>MySql Table created from the selectQuery.</returns>
    public static MySqlDataTable CreateImportMySqlTable(this MySqlWorkbenchConnection wbConnection, MySqlDataTable.DataOperationType operationType, string tableOrViewName, bool importColumnNames, string selectQuery, int procedureResultSetIndex = 0)
    {
      var dt = GetDataFromSelectQuery(wbConnection, selectQuery);
      if (dt == null)
      {
        Logger.LogVerbose(string.Format(Resources.SelectQueryReturnedNothing, selectQuery));
        return null;
      }

      var importMySqlDataTable = new MySqlDataTable(wbConnection, tableOrViewName, dt, operationType, selectQuery)
      {
        ImportColumnNames = importColumnNames,
        ProcedureResultSetIndex = procedureResultSetIndex
      };

      return importMySqlDataTable;
    }

    /// <summary>
    /// Drops the given table from the connected schema.
    /// </summary>
    /// <param name="connection">MySQL Workbench connection to a MySQL server instance selected by users.</param>
    /// <param name="tableName">The name of the table to drop.</param>
    public static void DropTableIfExists(this MySqlWorkbenchConnection connection, string tableName)
    {
      if (connection == null || string.IsNullOrEmpty(connection.Schema) || string.IsNullOrEmpty(tableName))
      {
        return;
      }

      try
      {
        var sql = $"DROP TABLE IF EXISTS `{connection.Schema}`.`{tableName}`";
        MySqlHelper.ExecuteNonQuery(connection.GetConnectionStringBuilder().ConnectionString, sql);
      }
      catch (Exception ex)
      {
        Logger.LogException(ex, true, string.Format(Resources.UnableToDropTableError, tableName));
      }
    }

    /// <summary>
    /// Escapes special characters that cause problems when passed within queries, from this data value string.
    /// </summary>
    /// <param name="valueToEscape">The data value text containing special characters.</param>
    /// <returns>A new string built from the given data value string without the special characters.</returns>
    public static string EscapeDataValueString(this string valueToEscape)
    {
      if (string.IsNullOrEmpty(valueToEscape))
      {
        return valueToEscape;
      }

      const string QUOTES_AND_OTHER_DANGEROUS_CHARS =
          "\\" + "\u2216" + "\uFF3C"               // backslashes
        + "'" + "\u00B4" + "\u02B9" + "\u02BC" + "\u02C8" + "\u02CA"
                + "\u0301" + "\u2019" + "\u201A" + "\u2032"
                + "\u275C" + "\uFF07"            // single-quotes
        + "`" + "\u02CB" + "\u0300" + "\u2018" + "\u2035" + "\u275B"
                + "\uFF40"                       // back-tick
        + "\"" + "\u02BA" + "\u030E" + "\uFF02"; // double-quotes

      var sb = new StringBuilder(valueToEscape.Length * 2);
      foreach (var c in valueToEscape)
      {
        var escape = char.MinValue;
        switch (c)
        {
          case '\u0000':
            escape = '0';
            break;

          case '\n':
            escape = 'n';
            break;

          case '\r':
            escape = 'r';
            break;

          case '\u001F':
            escape = 'Z';
            break;

          default:
            if (QUOTES_AND_OTHER_DANGEROUS_CHARS.IndexOf(c) >= 0)
            {
              escape = c;
            }

            break;
        }

        if (escape != char.MinValue)
        {
          sb.Append('\\');
          sb.Append(escape);
        }
        else
        {
          sb.Append(c);
        }
      }

      return sb.ToString();
    }

    /// <summary>
    /// Checks if the length of the SQL statement would exceed the MySQL Server's MAX_ALLOWED_PACKET variable value.
    /// </summary>
    /// <param name="sqlStatement">The string representing the SQL statement to be sent to the MySQL server.</param>
    /// <param name="maxAllowedPacketValue">The value of the the MySQL Server's MAX_ALLOWED_PACKET variable.</param>
    /// <param name="safetyBytes">A safety value before reaching the MAX_ALLOWED_PACKET variable value.</param>
    /// <returns><c>true</c> if the length of the statement exceeds the value of the server's MAX_ALLOWED_PACKET variable, <c>false</c> otherwise.</returns>
    public static bool ExceedsMySqlMaxAllowedPacketValue(this string sqlStatement, int maxAllowedPacketValue, int safetyBytes = 0)
    {
      var maxByteCount = maxAllowedPacketValue > 0 ? maxAllowedPacketValue - safetyBytes : 0;
      var statementByteCount = Encoding.ASCII.GetByteCount(sqlStatement);
      return statementByteCount > maxByteCount;
    }

    /// <summary>
    /// Checks if the length of the SQL statement would exceed the MySQL Server's MAX_ALLOWED_PACKET variable value.
    /// </summary>
    /// <param name="sqlStatement">The string representing the SQL statement to be sent to the MySQL server.</param>
    /// <param name="wbConnection">MySQL Workbench connection to a MySQL server instance selected by users.</param>
    /// <param name="safetyBytes">A safety value before reaching the MAX_ALLOWED_PACKET variable value.</param>
    /// <returns><c>true</c> if the length of the statement exceeds the value of the server's MAX_ALLOWED_PACKET variable, <c>false</c> otherwise.</returns>
    public static bool ExceedsMySqlMaxAllowedPacketValue(this string sqlStatement, MySqlWorkbenchConnection wbConnection, int safetyBytes = 0)
    {
      var maxAllowedPacketValue = wbConnection.GetMySqlServerMaxAllowedPacket();
      return ExceedsMySqlMaxAllowedPacketValue(sqlStatement, maxAllowedPacketValue, safetyBytes);
    }

    /// <summary>
    /// Executes a routine and returns all result sets as tables within a <see cref="DataSet"/>.
    /// </summary>
    /// <param name="connection">MySQL Workbench connection to a MySQL server instance selected by users.</param>
    /// <param name="routineName">Qualified routine name (i.e. Schema.Routine).</param>
    /// <param name="routineParameters">Array of arguments passed to the routine parameters.</param>
    /// <returns><see cref="DataSet"/> where each table within it represents each of the result sets returned by the routine.</returns>
    public static DataSet ExecuteRoutine(this MySqlWorkbenchConnection connection, string routineName, params MySqlParameter[] routineParameters)
    {
      if (connection == null)
      {
        return null;
      }

      // Create empty return DataSet
      var ds = new DataSet();

      // Create & open a SqlConnection, and dispose of it after we are done.
      using (var baseConnection = new MySqlConnection(connection.GetConnectionStringBuilder().ConnectionString))
      {
        baseConnection.Open();

        // Create a command and prepare it for execution
        using (var cmd = new MySqlCommand
        {
          Connection = baseConnection,
          CommandText = routineName,
          CommandType = CommandType.StoredProcedure
        })
        {
          if (routineParameters != null)
          {
            foreach (var p in routineParameters)
            {
              cmd.Parameters.Add(p);
            }
          }

          using (var reader = cmd.ExecuteReader())
          {
            var resultSetTable = reader.ReadResultSet("ResultSet");
            if (resultSetTable != null)
            {
              ds.Tables.Add(resultSetTable);
            }

            var resultSetIndex = 1;
            while (reader.NextResult())
            {
              resultSetTable = reader.ReadResultSet("ResultSet" + resultSetIndex++);
              if (resultSetTable != null)
              {
                ds.Tables.Add(resultSetTable);
              }
            }
          }

          // Detach the MySqlParameters from the command object, so they can be used again.
          cmd.Parameters.Clear();
        }

        // Return the data set
        return ds;
      }
    }

    /// <summary>
    /// Gets the array of column names from a given SelectQuery.
    /// </summary>
    /// <param name="selectQuery">The select query to get the array of column names from.</param>
    /// <returns></returns>
    public static string[] GetColumnNamesArrayFromSelectQuery(this string selectQuery)
    {
      if (string.IsNullOrEmpty(selectQuery))
      {
        return null;
      }

      // We calculate the index from the 'select' word to start parsing from.
      var start = selectQuery.ToLower().IndexOf("select", StringComparison.InvariantCulture);

      // We calculate the index from the 'from' word to finish parsing with.
      var end = selectQuery.ToLower().LastIndexOf("from", StringComparison.InvariantCulture);

      // If the words select and from are not contained in the selectQuery or the 'from' word is located 
      // before 'select' in the selectQuery, it is not in the right format and we quit.
      if (start == -1 || end == -1 || start > end)
      {
        return null;
      }

      // start points to the index where the words 'select' starts from, we need the index of the first character afterwards to begin parsing.
      start += 6;

      // We calculate the length between start and end to parse only the part of selectQuery that contains the column names.
      var length = end - start;
      var queryToAnalyze = selectQuery.Substring(start, length);

      //If all columns are listed, we don't need to enumerate them.
      if (queryToAnalyze.Contains("*"))
      {
        return null;
      }

      var result = queryToAnalyze.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
      for (var i = 0; i < result.Length; i++)
      {
        result[i] = result[i].Trim(' ', '`');
      }

      return result;
    }

    /// <summary>
    /// Returns a table containing schema information for columns contained in a MySQL table with the given name.
    /// </summary>
    /// <param name="connection">MySQL Workbench connection to a MySQL server instance selected by users.</param>
    /// <param name="schemaName">The schema the MySQL table belongs to.</param>
    /// <param name="tableName">The name of a MySQL table.</param>
    /// <param name="beautifyDataTypes">Flag indicating whether the data types are camel cased as shown in the Export Data data type combo box.</param>
    /// <returns>A table containing schema information for columns contained in a MySQL table with the given name.</returns>
    public static MySqlColumnsInformationTable GetColumnsInformationTable(this MySqlWorkbenchConnection connection, string schemaName, string tableName, bool beautifyDataTypes = false)
    {
      if (connection == null)
      {
        return null;
      }

      schemaName = string.IsNullOrEmpty(schemaName) ? connection.Schema : schemaName;
      var schemaTable = connection.GetSchemaInformation(SchemaInformationType.ColumnsFull, true, null, schemaName, tableName);
      if (schemaTable == null)
      {
        return null;
      }

      var columnsInfoTable = new MySqlColumnsInformationTable(schemaTable.TableName);
      foreach (DataRow row in schemaTable.Rows)
      {
        var dataType = row["COLUMN_TYPE"].ToString();
        if (beautifyDataTypes)
        {
          var mySqlDataType = new MySqlDataType(dataType, false);
          dataType = mySqlDataType.FullType;
        }

        var infoRow = columnsInfoTable.NewRow();
        infoRow["Name"] = row["COLUMN_NAME"];
        infoRow["Type"] = dataType;
        infoRow["Null"] = row["IS_NULLABLE"];
        infoRow["Key"] = row["COLUMN_KEY"];
        infoRow["Default"] = row["COLUMN_DEFAULT"];
        infoRow["CharSet"] = row["CHARACTER_SET_NAME"];
        infoRow["Collation"] = row["COLLATION_NAME"];
        infoRow["Extra"] = row["EXTRA"];
        columnsInfoTable.Rows.Add(infoRow);
      }

      return columnsInfoTable;
    }

    /// <summary>
    /// Gets the columns schema information for the given database table.
    /// </summary>
    /// <param name="dataTable">The data table to get the schema info for.</param>
    /// <param name="dataTypeFromCaption">Flag indicating whether the column data type should be taken from the <see cref="DataColumn"/>'s Caption property.</param>
    /// <returns>Table with schema information regarding its columns.</returns>
    public static MySqlColumnsInformationTable GetColumnsInformationTable(this DataTable dataTable, bool dataTypeFromCaption = false)
    {
      if (dataTable == null)
      {
        return null;
      }

      var schemaInfoTable = new MySqlColumnsInformationTable();
      foreach (DataColumn column in dataTable.Columns)
      {
        var newRow = schemaInfoTable.NewRow();
        newRow["Name"] = column.ColumnName;
        newRow["Type"] = dataTypeFromCaption && !string.IsNullOrEmpty(column.Caption) && MySqlDisplayDataType.ValidateTypeName(column.Caption)
          ? column.Caption
          : MySqlDataType.GetMySqlDataType(column.DataType);
        newRow["Null"] = column.AllowDBNull ? "YES" : "NO";
        newRow["Key"] = dataTable.PrimaryKey.Any(indexCol => indexCol.ColumnName == column.ColumnName) ? "PRI" : string.Empty;
        newRow["Default"] = column.DefaultValue != null ? column.DefaultValue.ToString() : string.Empty;
        newRow["CharSet"] = null;
        newRow["Collation"] = null;
        newRow["Extra"] = column.AutoIncrement ? MySqlDataColumn.ATTRIBUTE_AUTO_INCREMENT : string.Empty;
        schemaInfoTable.Rows.Add(newRow);
      }

      return schemaInfoTable;
    }

    /// <summary>
    /// Returns the connection string used for a new <see cref="ExcelInterop.WorkbookConnection"/> that uses a <see cref="ExcelInterop.XlCmdType.xlCmdDefault"/> command type.
    /// </summary>
    /// <param name="connection">MySQL Workbench connection to a MySQL server instance selected by users.</param>
    /// <returns>The connection string used for a new <see cref="ExcelInterop.WorkbookConnection"/> that uses a <see cref="ExcelInterop.XlCmdType.xlCmdDefault"/> command type.</returns>
    public static string GetConnectionStringForCmdDefault(this MySqlWorkbenchConnection connection)
    {
      return connection == null ? string.Empty : string.Format(OLEDB_MYSQL_CONNECTION_STRING_FULL, connection.Host, connection.Port, connection.Schema, connection.UserName);
    }

    /// <summary>
    /// Gets the SQL statements needed to create a new schema in the MySQL server instance specified in the given connection.
    /// </summary>
    /// <param name="connection">MySQL Workbench connection to a MySQL server instance selected by users.</param>
    /// <param name="schemaName">The name of the new schema.</param>
    /// <param name="grantPrivileges">Flag indicating whether all privileges are granted to the user that opened the connection.</param>
    /// <returns>The SQL statement used to create the new schema.</returns>
    public static string GetCreateSchemaSql(this MySqlWorkbenchConnection connection, string schemaName, bool grantPrivileges = false)
    {
      return GetCreateSchemaSql(connection, schemaName, null, null, grantPrivileges);
    }

    /// <summary>
    /// Gets the SQL statements needed to create a new schema in the MySQL server instance specified in the given connection.
    /// </summary>
    /// <param name="connection">MySQL Workbench connection to a MySQL server instance selected by users.</param>
    /// <param name="schemaName">The name of the new schema.</param>
    /// <param name="charset">The default character set assigned to the new schema. If <c>null</c> or empty the server's default character set is used.</param>
    /// <param name="collation">The collation of the character set, meaningful only if the <see cref="charset"/> parameter is not null or empty. If <c>null</c> or empty the default collation is used.</param>
    /// <param name="grantPrivileges">Flag indicating whether all privileges are granted to the user that opened the connection.</param>
    /// <returns>The SQL statement used to create the new schema.</returns>
    public static string GetCreateSchemaSql(this MySqlWorkbenchConnection connection, string schemaName, string charset, string collation, bool grantPrivileges)
    {
      if (connection == null && grantPrivileges)
      {
        return null;
      }

      var sqlBuilder = new StringBuilder(100);
      sqlBuilder.Append(MySqlStatement.STATEMENT_CREATE_SCHEMA);
      sqlBuilder.AppendFormat(" `{0}`", schemaName);
      if (!string.IsNullOrEmpty(charset))
      {
        sqlBuilder.AppendFormat(" {0} {1}", MySqlStatement.STATEMENT_DEFAULT_CHARSET, charset);
        if (!string.IsNullOrEmpty(collation) && !charset.Equals("binary", StringComparison.OrdinalIgnoreCase))
        {
          sqlBuilder.AppendFormat(" {0} {1}", MySqlStatement.STATEMENT_COLLATE, collation);
        }
      }

      if (!grantPrivileges)
      {
        return sqlBuilder.ToString();
      }

      sqlBuilder.Append(";");
      sqlBuilder.Append(Environment.NewLine);
      sqlBuilder.AppendFormat("{0} `{1}`.* TO '{2}'.'{3}';", MySqlStatement.STATEMENT_GRANT_ALL, schemaName, connection.UserName, connection.Host);
      return sqlBuilder.ToString();
    }

    /// <summary>
    /// Executes the given query and returns the result set in a <see cref="DataTable"/> object.
    /// </summary>
    /// <param name="connection">MySQL Workbench connection to a MySQL server instance selected by users.</param>
    /// <param name="query">Select query to be sent to the MySQL Server.</param>
    /// <param name="tableIndex">The index of the table in the <see cref="DataSet"/> to be returned.</param>
    /// <returns>Table containing the results of the query.</returns>
    public static DataTable GetDataFromSelectQuery(this MySqlWorkbenchConnection connection, string query, int tableIndex = 0)
    {
      if (connection == null)
      {
        return null;
      }

      DataSet ds = null;
      try
      {
        var connectionBuilder = connection.GetConnectionStringBuilder();
        ds = MySqlHelper.ExecuteDataset(connectionBuilder.ConnectionString, query);
      }
      catch (Exception ex)
      {
        Logger.LogException(ex, true, string.Format(Resources.UnableToRetrieveData, "query: ", query));
      }

      var dataTable = ds == null || ds.Tables.Count <= 0 || tableIndex < 0 || tableIndex >= ds.Tables.Count
        ? null
        : ds.Tables[tableIndex];
      return Settings.Default.ImportFloatingPointDataAsDecimal
        ? dataTable.ConvertApproximateFloatingPointDataTypeColumnsToExact()
        : dataTable;
    }

    /// <summary>
    /// Gets the host name used in connection nodes.
    /// </summary>
    /// <param name="connection">MySQL Workbench connection to a MySQL server instance selected by users.</param>
    /// <returns>The host string for connection nodes subtitles.</returns>
    public static string GetHostNameForConnectionSubtitle(this MySqlWorkbenchConnection connection)
    {
      if (connection == null)
      {
        return string.Empty;
      }

      var isSsh = connection.ConnectionMethod == MySqlWorkbenchConnection.ConnectionMethodType.Ssh;
      var hostName = (connection.Host ?? string.Empty).Trim();
      if (!isSsh)
      {
        return hostName;
      }

      var sshConnection = connection.HostIdentifier.Split('@');
      var dbHost = sshConnection[1].Split(':')[0].Trim();
      hostName = dbHost + @" (SSH)";
      return hostName;
    }

    /// <summary>
    /// Gets the total count of affected rows within the given list of rows with statements of a given type.
    /// </summary>
    /// <param name="rowsList">The list of <see cref="IMySqlDataRow"/> objects holding <see cref="MySqlStatement"/>s.</param>
    /// <param name="statementType">The type of statements to account affected rows for.</param>
    /// <returns>The total count of affected rows for a given statement type.</returns>
    public static int GetResultsCount(this List<IMySqlDataRow> rowsList, MySqlStatement.SqlStatementType statementType)
    {
      return rowsList?.Where(iMsqlRow => iMsqlRow.Statement.StatementType == statementType && iMsqlRow.Statement.AffectedRows > 0).Sum(iMsqlRow => iMsqlRow.Statement.AffectedRows) ?? 0;
    }

    /// <summary>
    /// Gets a schema name that is unique among the schemas in the current connection.
    /// </summary>
    /// <param name="connection">MySQL Workbench connection to a MySQL server instance selected by users.</param>
    /// <param name="proposedName">The proposed name for a new schema.</param>
    /// <returns>A unique schema name.</returns>
    public static string GetSchemaNameAvoidingDuplicates(this MySqlWorkbenchConnection connection, string proposedName)
    {
      if (connection == null)
      {
        return null;
      }

      if (string.IsNullOrEmpty(proposedName))
      {
        proposedName = DEFAULT_NEW_SCHEMA_NAME;
      }

      var schemas = connection.GetSchemaInformation(SchemaInformationType.Databases, true, null);
      if (schemas == null || schemas.Rows.Count == 0)
      {
        return proposedName;
      }

      var suffix = 2;
      var finalName = proposedName;
      while (schemas.Rows.Cast<DataRow>().Any(schemaRow => string.Equals(schemaRow["DATABASE_NAME"].ToString(), finalName, StringComparison.InvariantCultureIgnoreCase)))
      {
        finalName = proposedName + suffix++;
      }

      return finalName;
    }

    /// <summary>
    /// Assembles a SET statement that declares a user variable that contains the given <see cref="MySqlParameter"/> value.
    /// </summary>
    /// <param name="parameter">A <see cref="MySqlParameter"/> object.</param>
    /// <param name="firstParameter">When <c>true</c> the SET token is prepended, otherwise a comma is.</param>
    /// <returns>A SET statement that declares a user variable that contains the given <see cref="MySqlParameter"/> value.</returns>
    public static string GetSetStatement(this MySqlParameter parameter, bool firstParameter = true)
    {
      if (parameter == null)
      {
        return string.Empty;
      }

      var mySqlDataType = MySqlDataType.FromMySqlDbType(parameter.MySqlDbType);
      var requireQuotes = mySqlDataType.RequiresQuotesForValue;
      return string.Format("{0} @{1} = {2}{3}{2}",
        firstParameter ? "SET" : ",",
        parameter.ParameterName,
        requireQuotes ? "'" : string.Empty,
        requireQuotes ? parameter.Value.ToString().EscapeDataValueString() : parameter.Value);
    }

    /// <summary>
    /// Checks if an index with the given name exists in the given schema and table.
    /// </summary>
    /// <param name="connection">MySQL Workbench connection to a MySQL server instance selected by users.</param>
    /// <param name="schemaName">Name of the database schema where the index resides.</param>
    /// <param name="tableName">Name of the database table where the index resides.</param>
    /// <param name="indexName">Name of the index to look for.</param>
    /// <returns><c>true</c> if the index exists, <c>false</c> otherwise.</returns>
    public static bool IndexExistsInSchema(this MySqlWorkbenchConnection connection, string schemaName, string tableName, string indexName)
    {
      if (string.IsNullOrEmpty(indexName))
      {
        return false;
      }

      schemaName = string.IsNullOrEmpty(schemaName) ? connection.Schema : schemaName;
      var dt = connection.GetSchemaInformation(SchemaInformationType.Indexes, true, null, schemaName, tableName, indexName);
      return dt != null && dt.Rows.Count > 0;
    }

    /// <summary>
    /// Checks if the given type corresponds to a data type that MySQL deems as an approximate floating point type.
    /// </summary>
    /// <param name="type">A data type.</param>
    /// <returns><c>true</c> if the given type corresponds to a data type that MySQL deems as an approximate floating point type, <c>false</c> otherwise.</returns>
    public static bool IsApproximateFloatingPointDataType(this Type type)
    {
      return type != null
             && (type == typeof(float) || type == typeof(double));
    }

    /// <summary>
    /// Checks if the given data operation type is for appending data.
    /// </summary>
    /// <param name="operationType">A <see cref="MySqlDataTable.DataOperationType"/> enumeration value.</param>
    /// <returns><c>true</c> if the given data operation type is for appending data, <c>false</c> otherwise.</returns>
    public static bool IsForAppend(this MySqlDataTable.DataOperationType operationType)
    {
      return operationType == MySqlDataTable.DataOperationType.Append;
    }

    /// <summary>
    /// Checks if the given data operation type is for editing data.
    /// </summary>
    /// <param name="operationType">A <see cref="MySqlDataTable.DataOperationType"/> enumeration value.</param>
    /// <returns><c>true</c> if the given data operation type is for editing data, <c>false</c> otherwise.</returns>
    public static bool IsForEdit(this MySqlDataTable.DataOperationType operationType)
    {
      return operationType == MySqlDataTable.DataOperationType.Edit;
    }

    /// <summary>
    /// Checks if the given data operation type is for exporting data.
    /// </summary>
    /// <param name="operationType">A <see cref="MySqlDataTable.DataOperationType"/> enumeration value.</param>
    /// <returns><c>true</c> if the given data operation type is for exporting data, <c>false</c> otherwise.</returns>
    public static bool IsForExport(this MySqlDataTable.DataOperationType operationType)
    {
      return operationType == MySqlDataTable.DataOperationType.Export;
    }

    /// <summary>
    /// Checks if the given data operation type is for importing data.
    /// </summary>
    /// <param name="operationType">A <see cref="MySqlDataTable.DataOperationType"/> enumeration value.</param>
    /// <returns><c>true</c> if the given data operation type is for importing data, <c>false</c> otherwise.</returns>
    public static bool IsForImport(this MySqlDataTable.DataOperationType operationType)
    {
      return operationType == MySqlDataTable.DataOperationType.ImportTableOrView || operationType == MySqlDataTable.DataOperationType.ImportProcedure;
    }

    /// <summary>
    /// Checks if the string value representing a date is a MySQL zero date.
    /// </summary>
    /// <param name="dateValueAsString">The string value representing a date.</param>
    /// <param name="checkIfIntZero">Flag indicating whether a value of 0 should also be treated as a zero date.</param>
    /// <returns><c>true</c> if the passed string value is a MySQL zero date, <c>false</c> otherwise.</returns>
    public static bool IsMySqlZeroDateTimeValue(this string dateValueAsString, bool checkIfIntZero = false)
    {
      var isDateValueZero = checkIfIntZero && int.TryParse(dateValueAsString, out var zeroValue) && zeroValue == 0;
      MySqlDataType.IsMySqlDateTimeValue(dateValueAsString, out var isDateValueAZeroDate);
      return isDateValueZero || isDateValueAZeroDate;
    }

    /// <summary>
    /// Gets a value indicating whether the value of the given parameter should not be written to depending on its direction.
    /// </summary>
    /// <param name="parameter">A <see cref="MySqlParameter"/> object.</param>
    /// <returns><c>true</c> if the parameter's direction is <see cref="ParameterDirection.Output"/> or <see cref="ParameterDirection.ReturnValue"/>, <c>false</c> otherwise.</returns>
    public static bool IsReadOnly(this MySqlParameter parameter)
    {
      return parameter != null && (parameter.Direction == ParameterDirection.Output || parameter.Direction == ParameterDirection.ReturnValue);
    }

    /// <summary>
    /// Checks if a MySQL collation with the given name is a Unicode one.
    /// </summary>
    /// <param name="charSetOrCollation">A MySQL character set or collation name.</param>
    /// <returns><c>true</c> if a MySQL collation with the given name is a Unicode one, <c>false</c> otherwise.</returns>
    public static bool IsUnicodeCharSetOrCollation(this string charSetOrCollation)
    {
      if (string.IsNullOrEmpty(charSetOrCollation))
      {
        return false;
      }

      charSetOrCollation = charSetOrCollation.ToLowerInvariant();
      return charSetOrCollation.StartsWith("ucs2")
             || charSetOrCollation.StartsWith("utf8")
             || charSetOrCollation.StartsWith("utf16")
             || charSetOrCollation.StartsWith("utf16le")
             || charSetOrCollation.StartsWith("utf32")
             || charSetOrCollation.StartsWith("utf8mb4");
    }

    /// <summary>
    /// Returns a <see cref="DataTable"/> with the data read from the given <see cref="IDataReader"/>.
    /// </summary>
    /// <param name="dataReader">A <see cref="IDataReader"/> instance.</param>
    /// <param name="resultSetTableName">An optional name for the returned <see cref="DataTable"/>.</param>
    /// <returns>A <see cref="DataTable"/> with the data read from the given <see cref="IDataReader"/>.</returns>
    public static DataTable ReadResultSet(this IDataReader dataReader, string resultSetTableName = null)
    {
      if (dataReader == null || dataReader.IsClosed)
      {
        return null;
      }

      if (string.IsNullOrEmpty(resultSetTableName))
      {
        resultSetTableName = "ResultSet";
      }

      var resultSetDataTable = new DataTable(resultSetTableName);
      for (var colIdx = 0; colIdx < dataReader.FieldCount; colIdx++)
      {
        var type = dataReader.GetFieldType(colIdx);
        var newColumn = new DataColumn(dataReader.GetName(colIdx), type ?? typeof(string))
        {
          // Hack the Caption property of the DataColumn to store the MySQL data type (useful only for spatial data which data type is a byte array which can be mistaken as a BLOB)
          Caption = dataReader.GetDataTypeName(colIdx).ToLowerInvariant()
        };
        resultSetDataTable.Columns.Add(newColumn);
      }

      while (dataReader.Read())
      {
        var newRow = resultSetDataTable.NewRow();
        for (var colIdx = 0; colIdx < dataReader.FieldCount; colIdx++)
        {
          newRow[colIdx] = dataReader[colIdx];
        }

        resultSetDataTable.Rows.Add(newRow);
      }

      return resultSetDataTable;
    }

    /// <summary>
    /// Sets additional properties used by each connection opened in MySQL for Excel that are not persisted in a <see cref="MySqlWorkbenchConnection"/>.
    /// </summary>
    /// <param name="connection">MySQL Workbench connection to a MySQL server instance selected by users.</param>
    public static void SetAdditionalConnectionProperties(this MySqlWorkbenchConnection connection)
    {
      if (connection == null)
      {
        return;
      }

      connection.AllowUserVariables = true;
      connection.AllowZeroDateTimeValues = true;
      connection.CharacterSet = "utf8";
      connection.TreatTinyIntAsBoolean = false;
    }

    /// <summary>
    /// Checks if a table with the given name exists in the given schema.
    /// </summary>
    /// <param name="connection">MySQL Workbench connection to a MySQL server instance selected by users.</param>
    /// <param name="schemaName">Name of the database schema where the table resides.</param>
    /// <param name="tableName">Name of the table to look for.</param>
    /// <returns><c>true</c> if the table exists, <c>false</c> otherwise.</returns>
    public static bool TableExistsInSchema(this MySqlWorkbenchConnection connection, string schemaName, string tableName)
    {
      if (connection == null || string.IsNullOrEmpty(schemaName) || string.IsNullOrEmpty(tableName))
      {
        return false;
      }

      object objCount = null;
      try
      {
        var sql = $"SELECT COUNT(*) FROM information_schema.tables WHERE table_schema = '{schemaName}' AND table_name = '{tableName.EscapeDataValueString()}'";
        objCount = MySqlHelper.ExecuteScalar(connection.GetConnectionStringBuilder().ConnectionString, sql);
      }
      catch (Exception ex)
      {
        Logger.LogException(ex, true, string.Format(Resources.UnableToRetrieveData, $"`{schemaName}`.", tableName));
      }

      var retCount = (long?)objCount ?? 0;
      return retCount > 0;
    }

    /// <summary>
    /// Checks if a table with the given name has a primary key defined.
    /// </summary>
    /// <param name="connection">MySQL Workbench connection to a MySQL server instance selected by users.</param>
    /// <param name="tableName">Name of the table.</param>
    /// <returns><c>true</c> if the table has a primary key, <c>false</c> otherwise.</returns>
    public static bool TableHasPrimaryKey(this MySqlWorkbenchConnection connection, string tableName)
    {
      if (connection == null || string.IsNullOrEmpty(tableName))
      {
        return false;
      }

      var sql = $"SHOW KEYS FROM `{tableName}` IN `{connection.Schema}` WHERE Key_name = 'PRIMARY';";
      var dt = GetDataFromSelectQuery(connection, sql);
      return dt != null && dt.Rows.Count > 0;
    }

    /// <summary>
    /// Converts a data string to a valid column name.
    /// </summary>
    /// <param name="proposedName">String intended to be used as a MySQL column name.</param>
    /// <returns>A string formatted as a valid column name.</returns>
    public static string ToValidMySqlColumnName(this string proposedName)
    {
      return proposedName != null ? proposedName.Replace(" ", "_").Replace("(", string.Empty).Replace(")", string.Empty) : string.Empty;
    }

    /// <summary>
    /// Unlocks tables locked in the current session.
    /// </summary>
    /// <param name="connection">MySQL Workbench connection to a MySQL server instance selected by users.</param>
    public static void UnlockTablesInClientSession(this MySqlWorkbenchConnection connection)
    {
      try
      {
        const string SQL = "UNLOCK TABLES";
        MySqlHelper.ExecuteNonQuery(connection.GetConnectionStringBuilder().ConnectionString, SQL);
      }
      catch (Exception ex)
      {
        Logger.LogException(ex, true, Resources.UnableToUnlockTablesError);
      }
    }

    /// <summary>
    /// Verifies if a statement result was applied to the server, i.e. that it was successful or had warnings.
    /// </summary>
    /// <param name="statementResult">The statement result to evaluate.</param>
    /// <returns><c>true</c> if the result is successful or had warnings, <c>false</c> otherwise.</returns>
    public static bool WasApplied(this MySqlStatement.StatementResultType statementResult)
    {
      return statementResult == MySqlStatement.StatementResultType.Successful || statementResult == MySqlStatement.StatementResultType.WarningsFound;
    }

    /// <summary>
    /// Verifies if a statement result was executed without any error.
    /// </summary>
    /// <param name="statementResult">The statement result to evaluate.</param>
    /// <returns><c>true</c> if the result does not contain any kind of error, <c>false</c> otherwise.</returns>
    public static bool WithoutErrors(this MySqlStatement.StatementResultType statementResult)
    {
      return statementResult != MySqlStatement.StatementResultType.ConnectionLost && statementResult != MySqlStatement.StatementResultType.ErrorThrown;
    }
  }
}
