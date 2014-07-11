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
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using MySql.Data.MySqlClient;
using MySQL.ForExcel.Forms;
using MySQL.ForExcel.Interfaces;
using MySQL.ForExcel.Properties;
using MySQL.ForExcel.Structs;
using MySQL.Utility.Classes;
using MySQL.Utility.Classes.MySQLWorkbench;
using MySQL.Utility.Forms;
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
    /// Creates the import my SQL table.
    /// </summary>
    /// <param name="isEditOperation"><c>true</c> if the import is part of an Edit operation, <c>false</c> otherwise.</param>
    /// <param name="wbConnection">The wb connection.</param>
    /// <param name="tableOrViewName">The name of the MySQL table or view to import data from..</param>
    /// <param name="importColumnNames">Flag indicating if column names will be imported as the first row of imported data.</param>
    /// <param name="selectQuery">a SELECT query against a database object to fill the [MySqlDataTable] return object with.</param>
    /// <returns>MySql Table created from the selectQuery.</returns>
    public static MySqlDataTable CreateImportMySqlTable(this MySqlWorkbenchConnection wbConnection, bool isEditOperation, string tableOrViewName, bool importColumnNames, string selectQuery)
    {
      DataTable dt = GetDataFromSelectQuery(wbConnection, selectQuery);
      if (dt == null)
      {
        MySqlSourceTrace.WriteToLog(string.Format(Resources.SelectQueryReturnedNothing, selectQuery));
        return null;
      }

      var importMySqlDataTable = new MySqlDataTable(wbConnection, tableOrViewName, dt, isEditOperation)
      {
        ImportColumnNames = importColumnNames,
        SelectQuery = selectQuery
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

      string sql = string.Format("DROP TABLE IF EXISTS `{0}`.`{1}`", connection.Schema, tableName);
      MySqlHelper.ExecuteNonQuery(connection.GetConnectionStringBuilder().ConnectionString, sql);
    }

    /// <summary>
    /// Escapes special characters that cause problems when passed within queries, from this data value string.
    /// </summary>
    /// <param name="valueToEscape">The data value text containing special characters.</param>
    /// <returns>A new string built from the given data value string withouth the special characters.</returns>
    public static string EscapeDataValueString(this string valueToEscape)
    {
      if (string.IsNullOrEmpty(valueToEscape))
      {
        return valueToEscape;
      }

      const string quotesAndOtherDangerousChars =
          "\\" + "\u2216" + "\uFF3C"               // backslashes
        + "'" + "\u00B4" + "\u02B9" + "\u02BC" + "\u02C8" + "\u02CA"
                + "\u0301" + "\u2019" + "\u201A" + "\u2032"
                + "\u275C" + "\uFF07"            // single-quotes
        + "`" + "\u02CB" + "\u0300" + "\u2018" + "\u2035" + "\u275B"
                + "\uFF40"                       // back-tick
        + "\"" + "\u02BA" + "\u030E" + "\uFF02"; // double-quotes

      StringBuilder sb = new StringBuilder(valueToEscape.Length * 2);
      foreach (char c in valueToEscape)
      {
        char escape = char.MinValue;
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
            if (quotesAndOtherDangerousChars.IndexOf(c) >= 0)
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
    /// <returns><c>true</c> if the length of the statement exceeds the vlaue of the server's MAX_ALLOWED_PACKET variable, <c>false</c> otherwise.</returns>
    public static bool ExceedsMySqlMaxAllowedPacketValue(this string sqlStatement, ulong maxAllowedPacketValue, ulong safetyBytes = 0)
    {
      ulong maxByteCount = maxAllowedPacketValue > 0 ? maxAllowedPacketValue - safetyBytes : 0;
      ulong statementByteCount = (ulong)Encoding.ASCII.GetByteCount(sqlStatement);
      return statementByteCount > maxByteCount;
    }

    /// <summary>
    /// Checks if the length of the SQL statement would exceed the MySQL Server's MAX_ALLOWED_PACKET variable value.
    /// </summary>
    /// <param name="sqlStatement">The string representing the SQL statement to be sent to the MySQL server.</param>
    /// <param name="wbConnection">MySQL Workbench connection to a MySQL server instance selected by users.</param>
    /// <param name="safetyBytes">A safety value before reaching the MAX_ALLOWED_PACKET variable value.</param>
    /// <returns><c>true</c> if the length of the statement exceeds the vlaue of the server's MAX_ALLOWED_PACKET variable, <c>false</c> otherwise.</returns>
    public static bool ExceedsMySqlMaxAllowedPacketValue(this string sqlStatement, MySqlWorkbenchConnection wbConnection, ulong safetyBytes = 0)
    {
      ulong maxAllowedPacketValue = wbConnection.GetMySqlServerMaxAllowedPacket();
      return ExceedsMySqlMaxAllowedPacketValue(sqlStatement, maxAllowedPacketValue, safetyBytes);
    }

    /// <summary>
    /// Executes a routine and returns all result sets as tables within a dataset.
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

      // Create & open a SqlConnection, and dispose of it after we are done.
      using (MySqlConnection baseConnection = new MySqlConnection(connection.GetConnectionStringBuilder().ConnectionString))
      {
        baseConnection.Open();

        // Create a command and prepare it for execution
        MySqlCommand cmd = new MySqlCommand
        {
          Connection = baseConnection,
          CommandText = routineName,
          CommandType = CommandType.StoredProcedure
        };

        if (routineParameters != null)
        {
          foreach (MySqlParameter p in routineParameters)
          {
            cmd.Parameters.Add(p);
          }
        }

        // Create the DataAdapter & DataSet
        MySqlDataAdapter da = new MySqlDataAdapter(cmd);
        DataSet ds = new DataSet();

        // Fill the DataSet using default values for DataTable names, etc.
        da.Fill(ds);

        // Detach the MySqlParameters from the command object, so they can be used again.
        cmd.Parameters.Clear();

        // Return the dataset
        return ds;
      }
    }

    /// <summary>
    /// Gets a list of all MySQL character sets along with their available collations.
    /// </summary>
    /// <param name="connection">MySQL Workbench connection to a MySQL server instance selected by users.</param>
    /// <param name="firstElement">A custom string for the first element of the dictioary.</param>
    /// <returns>A list of all MySQL character sets along with their available collations.</returns>
    public static Dictionary<string, string[]> GetCollationsDictionary(this MySqlWorkbenchConnection connection, string firstElement = null)
    {
      if (connection == null)
      {
        return null;
      }

      var charSetsTable = connection.GetSchemaCollection("Collations", null);
      if (charSetsTable == null)
      {
        return null;
      }

      const string defaultCollation = "default collation";
      var rowsByGroup = charSetsTable.Select(string.Empty, "Charset, Collation").GroupBy(r => r["Charset"].ToString());
      var collationsDictionary = new Dictionary<string, string[]>(270);
      if (!string.IsNullOrEmpty(firstElement))
      {
        collationsDictionary.Add(firstElement, new[] { string.Empty, string.Empty });
      }

      foreach (var rowsGroup in rowsByGroup)
      {
        var charset = rowsGroup.Key;
        collationsDictionary.Add(string.Format("{0} - {1}", charset, defaultCollation), new[] { charset, string.Empty });
        foreach (var collation in rowsGroup.Select(row => row["Collation"].ToString()))
        {
          collationsDictionary.Add(string.Format("{0} - {1}", charset, collation), new[] { charset, collation });
        }
      }

      return collationsDictionary;
    }

    /// <summary>
    /// Returns the connection string used for a new <see cref="ExcelInterop.WorkbookConnection"/> that uses a <see cref="ExcelInterop.XlCmdType.xlCmdDefault"/> command type.
    /// </summary>
    /// <param name="connection">MySQL Workbench connection to a MySQL server instance selected by users.</param>
    /// <returns>The connection string used for a new <see cref="ExcelInterop.WorkbookConnection"/> that uses a <see cref="ExcelInterop.XlCmdType.xlCmdDefault"/> command type.</returns>
    public static string GetConnectionStringForCmdDefault(this MySqlWorkbenchConnection connection)
    {
      return connection == null ? string.Empty : string.Format("OLEDB;Provider=MSDASQL;Driver={{MySQL ODBC 5.2 ANSI Driver}};Server={0};Database={1};User={2};Option=3;", connection.Host, connection.Schema, connection.UserName);
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
        if (!string.IsNullOrEmpty(collation))
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
    /// <returns>Table containing the results of the query.</returns>
    public static DataTable GetDataFromSelectQuery(this MySqlWorkbenchConnection connection, string query)
    {
      if (connection == null)
      {
        return null;
      }

      DataSet ds = MySqlHelper.ExecuteDataset(connection.GetConnectionStringBuilder().ConnectionString, query);
      if (ds.Tables.Count <= 0)
      {
        return null;
      }

      DataTable retTable = ds.Tables[0];
      return retTable;
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

      bool isSsh = connection.DriverType == MySqlWorkbenchConnectionType.Ssh;
      string hostName = (connection.Host ?? string.Empty).Trim();
      if (!isSsh)
      {
        return hostName;
      }

      string[] sshConnection = connection.HostIdentifier.Split('@');
      string dbHost = sshConnection[1].Split(':')[0].Trim();
      hostName = dbHost + @" (SSH)";
      return hostName;
    }

    /// <summary>
    /// Gets the value of the DEFAULT_STORAGE_ENGINE MySQL Server variable indicating the default DB engine used for new table creations.
    /// </summary>
    /// <param name="connection">MySQL Workbench connection to a MySQL server instance selected by users.</param>
    /// <returns>The default DB engine used for new table creations.</returns>
    public static string GetMySqlServerDefaultEngine(this MySqlWorkbenchConnection connection)
    {
      const string sql = "SELECT @@default_storage_engine";
      object objEngine = connection != null ? MySqlHelper.ExecuteScalar(connection.GetConnectionStringBuilder().ConnectionString, sql) : null;
      return objEngine != null ? objEngine.ToString() : string.Empty;
    }

    /// <summary>
    /// Gets the value of the MAX_ALLOWED_PACKET MySQL Server variable indicating the max size in bytes of the packet returned by a single query.
    /// </summary>
    /// <param name="connection">MySQL Workbench connection to a MySQL server instance selected by users.</param>
    /// <returns>The max size in bytes of the packet returned by a single query.</returns>
    public static ulong GetMySqlServerMaxAllowedPacket(this MySqlWorkbenchConnection connection)
    {
      const string sql = "SELECT @@max_allowed_packet";
      object objCount = connection != null ? MySqlHelper.ExecuteScalar(connection.GetConnectionStringBuilder().ConnectionString, sql) : null;
      return objCount != null ? (ulong)objCount : 0;
    }

    /// <summary>
    /// Gets the value of the MAX_ALLOWED_PACKET MySQL Server variable indicating the max size in bytes of the packet returned by a single query.
    /// </summary>
    /// <param name="connection">A MySQL connection.</param>
    /// <returns>The max size in bytes of the packet returned by a single query.</returns>
    public static ulong GetMySqlServerMaxAllowedPacket(this MySqlConnection connection)
    {
      const string sql = "SELECT @@max_allowed_packet";
      object objCount = connection != null ? MySqlHelper.ExecuteScalar(connection, sql) : null;
      return objCount != null ? (ulong)objCount : 0;
    }

    /// <summary>
    /// Gets the total count of affected rows within the given list of rows with statements of a given type.
    /// </summary>
    /// <param name="rowsList">The list of <see cref="IMySqlDataRow"/> objects holding <see cref="MySqlStatement"/>s.</param>
    /// <param name="statementType">The type of statements to account affected rows for.</param>
    /// <returns>The total count of affected rows for a given statement type.</returns>
    public static int GetResultsCount(this List<IMySqlDataRow> rowsList, MySqlStatement.SqlStatementType statementType)
    {
      return rowsList != null
          ? rowsList.Where(iMsqlRow => iMsqlRow.Statement.StatementType == statementType && iMsqlRow.Statement.AffectedRows > 0).Sum(iMsqlRow => iMsqlRow.Statement.AffectedRows)
          : 0;
    }

    /// <summary>
    /// Gets the schema information ofr the given database collection.
    /// </summary>
    /// <param name="connection">MySQL Workbench connection to a MySQL server instance selected by users.</param>
    /// <param name="collection">The type of database collection to return schema information for.</param>
    /// <param name="restrictions">Specific parameters that vary among database collections.</param>
    /// <returns>Schema information within a data table.</returns>
    public static DataTable GetSchemaCollection(this MySqlWorkbenchConnection connection, string collection, params string[] restrictions)
    {
      if (connection == null)
      {
        return null;
      }

      string connectionString = connection.GetConnectionStringBuilder().ConnectionString;
      DataTable dt;
      try
      {
        using (var baseConnection = new MySqlConnection(connectionString))
        {
          baseConnection.Open();
          MySqlDataAdapter mysqlAdapter;
          switch (collection.ToUpperInvariant())
          {
            case "COLUMNS SHORT":
              mysqlAdapter = new MySqlDataAdapter(string.Format("SHOW COLUMNS FROM `{0}`.`{1}`", restrictions[1], restrictions[2]), baseConnection);
              dt = new DataTable();
              mysqlAdapter.Fill(dt);
              break;

            case "ENGINES":
              mysqlAdapter = new MySqlDataAdapter("SELECT * FROM information_schema.engines ORDER BY engine", baseConnection);
              dt = new DataTable();
              mysqlAdapter.Fill(dt);
              break;

            case "COLLATIONS":
              string queryString;
              if (restrictions != null && restrictions.Length > 0 && !string.IsNullOrEmpty(restrictions[0]))
              {
                queryString = string.Format("SHOW COLLATION WHERE charset = '{0}'", restrictions[0]);
              }
              else
              {
                queryString = "SHOW COLLATION";
              }

              mysqlAdapter = new MySqlDataAdapter(queryString, baseConnection);
              dt = new DataTable();
              mysqlAdapter.Fill(dt);
              break;

            case "CHARSETS":
              mysqlAdapter = new MySqlDataAdapter("SHOW CHARSET", baseConnection);
              dt = new DataTable();
              mysqlAdapter.Fill(dt);
              break;

            default:
              dt = baseConnection.GetSchema(collection, restrictions);
              break;
          }
        }
      }
      catch (Exception ex)
      {
        MySqlSourceTrace.WriteAppErrorToLog(ex);
        throw;
      }

      return dt;
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

      var schemas = connection.GetSchemaCollection("Databases", null);
      if (schemas == null || schemas.Rows.Count == 0)
      {
        return proposedName;
      }

      int suffix = 2;
      string finalName = proposedName;
      while (schemas.Rows.Cast<DataRow>().Any(schemaRow => string.Equals(schemaRow["DATABASE_NAME"].ToString(), finalName, StringComparison.InvariantCultureIgnoreCase)))
      {
        finalName = proposedName + suffix++;
      }

      return finalName;
    }

    /// <summary>
    /// Gets the columns schema information for the given database table.
    /// </summary>
    /// <param name="dataTable">The data table to get the schema info for.</param>
    /// <returns>Table with schema information regarding its columns.</returns>
    public static DataTable GetColumnsSchemaInfo(this DataTable dataTable)
    {
      if (dataTable == null)
      {
        return null;
      }

      DataTable schemaInfoTable = new DataTable("SchemaInfo");
      schemaInfoTable.Columns.Add("Field");
      schemaInfoTable.Columns.Add("Type");
      schemaInfoTable.Columns.Add("Null");
      schemaInfoTable.Columns.Add("Key");
      schemaInfoTable.Columns.Add("Default");
      schemaInfoTable.Columns.Add("Extra");

      foreach (DataColumn column in dataTable.Columns)
      {
        var newRow = schemaInfoTable.NewRow();
        newRow["Field"] = column.ColumnName;
        newRow["Type"] = column.DataType.GetMySqlDataType();
        newRow["Null"] = column.AllowDBNull ? "YES" : "NO";
        newRow["Key"] = dataTable.PrimaryKey.Any(indexCol => indexCol.ColumnName == column.ColumnName) ? "PRI" : string.Empty;
        newRow["Default"] = column.DefaultValue != null ? column.DefaultValue.ToString() : string.Empty;
        newRow["Extra"] = column.AutoIncrement ? "auto_increment" : string.Empty;
        schemaInfoTable.Rows.Add(newRow);
      }

      return schemaInfoTable;
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
      if (connection == null || string.IsNullOrEmpty(schemaName) || string.IsNullOrEmpty(indexName))
      {
        return false;
      }

      DataTable dt = connection.GetSchemaCollection("Indexes", null, schemaName, tableName, indexName);
      return dt != null && dt.Rows.Count > 0;
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
      return operationType == MySqlDataTable.DataOperationType.Import;
    }

    /// <summary>
    /// Checks if the given connection may be using SSL.
    /// </summary>
    /// <param name="connection">MySQL Workbench connection to a MySQL server instance selected by users.</param>
    /// <returns><c>true</c> if the connection uses SSL, <c>false</c> otherwise.</returns>
    public static bool IsSsl(this MySqlWorkbenchConnection connection)
    {
      return connection.UseSsl == 1
        || !(string.IsNullOrWhiteSpace(connection.SslCa)
        && string.IsNullOrWhiteSpace(connection.SslCert)
        && string.IsNullOrWhiteSpace(connection.SslCipher)
        && string.IsNullOrWhiteSpace(connection.SslKey));
    }

    /// <summary>
    /// Removes the desired session depending on the type.
    /// </summary>
    /// <param name="sessionToRemove">The session to remove.</param>
    public static void RemoveSession(ISessionInfo sessionToRemove)
    {
      if (sessionToRemove.GetType() == typeof(EditSessionInfo))
      {
        Globals.ThisAddIn.StoredEditSessions.Remove((EditSessionInfo)sessionToRemove);
      }
      else if (sessionToRemove.GetType() == typeof(ImportSessionInfo))
      {
        Globals.ThisAddIn.StoredImportSessions.Remove((ImportSessionInfo)sessionToRemove);
      }

      MiscUtilities.SaveSettings();
    }

    /// <summary>
    /// Sets the net_write_timeout and net_read_timeout MySQL server variables to the given value for the duration of the current session.
    /// </summary>
    /// <param name="connection">MySQL Workbench connection to a MySQL server instance selected by users.</param>
    /// <param name="timeoutInSeconds">
    /// The number of seconds to wait for more data from a connection before aborting the read or for a block to be written to a connection before aborting the write.
    /// If the parameter is ommitted or a value of <c>0</c> is passed to it, the <see cref="MySqlWorkbenchConnection.DefaultCommandTimeout"/> property value is used.
    /// </param>
    public static void SetSessionReadWriteTimeouts(this MySqlWorkbenchConnection connection, uint timeoutInSeconds = 0)
    {
      if (connection == null)
      {
        return;
      }

      if (timeoutInSeconds == 0)
      {
        timeoutInSeconds = connection.DefaultCommandTimeout;
      }

      if (timeoutInSeconds < 1)
      {
        return;
      }

      string sql = string.Format("SET SESSION net_write_timeout = {0}, SESSION net_read_timeout = {0}", timeoutInSeconds);
      MySqlHelper.ExecuteNonQuery(connection.GetConnectionStringBuilder().ConnectionString, sql);
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

      string sql = string.Format("SELECT COUNT(*) FROM information_schema.tables WHERE table_schema = '{0}' AND table_name = '{1}'", schemaName, tableName.EscapeDataValueString());
      object objCount = MySqlHelper.ExecuteScalar(connection.GetConnectionStringBuilder().ConnectionString, sql);
      long retCount = objCount != null ? (long)objCount : 0;
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

      string sql = string.Format("SHOW KEYS FROM `{0}` IN `{1}` WHERE Key_name = 'PRIMARY';", tableName, connection.Schema);
      DataTable dt = GetDataFromSelectQuery(connection, sql);
      return dt != null && dt.Rows.Count > 0;
    }

    /// <summary>
    /// Tests the current connection until the user enters a correct password.
    /// </summary>
    /// <param name="wbConnection">A <see cref="MySqlWorkbenchConnection"/> object representing the connection to a MySQL server instance selected by users.</param>
    /// <param name="tryConnectionBeforeAskingForPassword">Flag indicating whether a connection test is made with the connection as is before asking for a password</param>
    /// <returns>A <see cref="PasswordDialogFlags"/> containing data about the operation.</returns>
    public static PasswordDialogFlags TestConnectionAndRetryOnWrongPassword(this MySqlWorkbenchConnection wbConnection, bool tryConnectionBeforeAskingForPassword = true)
    {
      PasswordDialogFlags passwordFlags = new PasswordDialogFlags(wbConnection)
      {
        // Assume a wrong password at first so if the connection is not tested without a password we ensure to ask for one.
        ConnectionResult = TestConnectionResult.WrongPassword
      };

      // First connection attempt with the connection exactly as loaded (maybe without a password).
      if (tryConnectionBeforeAskingForPassword)
      {
        passwordFlags.ConnectionResult = wbConnection.TestConnectionAndReturnResult(false);
        passwordFlags.Cancelled = passwordFlags.ConnectionResult == TestConnectionResult.PasswordExpired;

        // If on the first attempt a connection could not be made and not because of a bad password, exit.
        if (!passwordFlags.ConnectionSuccess && !passwordFlags.WrongPassword)
        {
          return passwordFlags;
        }
      }

      // If the connection does not have a stored password or the stored password failed then ask for one and retry.
      while (!passwordFlags.ConnectionSuccess && passwordFlags.WrongPassword)
      {
        passwordFlags = PasswordDialog.ShowConnectionPasswordDialog(wbConnection, true);
        if (passwordFlags.Cancelled)
        {
          break;
        }

        wbConnection.Password = passwordFlags.NewPassword;
      }

      return passwordFlags;
    }

    /// <summary>
    /// Tests the given connection to check if it can successfully connect to the corresponding MySQL instance.
    /// </summary>
    /// <param name="connection">MySQL Workbench connection to a MySQL server instance selected by users.</param>
    /// <param name="displayErrorOnEmptyPassword">Flag indicating whether errors caused by a blank or null password are displayed to the user.</param>
    /// <returns>Enumeration indicating the result of the connection test.</returns>
    public static TestConnectionResult TestConnectionAndReturnResult(this MySqlWorkbenchConnection connection, bool displayErrorOnEmptyPassword)
    {
      if (connection == null)
      {
        return TestConnectionResult.None;
      }

      Globals.ThisAddIn.Application.Cursor = ExcelInterop.XlMousePointer.xlWait;
      TestConnectionResult connectionResult;
      Exception connectionException;
      if (connection.TestConnection(out connectionException))
      {
        connectionResult = TestConnectionResult.ConnectionSuccess;
        Globals.ThisAddIn.Application.Cursor = ExcelInterop.XlMousePointer.xlDefault;
        return connectionResult;
      }

      // If the error returned is about the connection failing the password check, it may be because either the stored password is wrong or no password.
      connectionResult = TestConnectionResult.ConnectionError;
      if (connectionException is MySqlException)
      {
        MySqlException mySqlException = connectionException as MySqlException;
        switch (mySqlException.Number)
        {
          // Connection could not be made.
          case MySqlWorkbenchConnection.MYSQL_EXCEPTION_NUMBER_SERVER_UNREACHABLE:
            connectionResult = TestConnectionResult.HostUnreachable;
            InfoDialog.ShowErrorDialog(Resources.ConnectFailedWarningTitle, mySqlException.Message, null, mySqlException.InnerException != null ? mySqlException.InnerException.Message : null);
            break;

          // Wrong password.
          case MySqlWorkbenchConnection.MYSQL_EXCEPTION_NUMBER_WRONG_PASSWORD:
            connectionResult = TestConnectionResult.WrongPassword;
            if (!string.IsNullOrEmpty(connection.Password) || displayErrorOnEmptyPassword)
            {
              string moreInfoText = connection.IsSsl() ? Resources.ConnectSSLFailedDetailWarning : null;
              InfoDialog.ShowWarningDialog(Resources.ConnectFailedWarningTitle, mySqlException.Message, null, moreInfoText);
            }
            break;

          // Password has expired so any statement can't be run before resetting the expired password.
          case MySqlWorkbenchConnection.MYSQL_EXCEPTION_NUMBER_EXPIRED_PASSWORD:
            PasswordDialogFlags passwordFlags = PasswordDialog.ShowExpiredPasswordDialog(connection, false);
            if (!passwordFlags.Cancelled)
            {
              connection.Password = passwordFlags.NewPassword;
            }

            connectionResult = passwordFlags.Cancelled ? TestConnectionResult.PasswordExpired : TestConnectionResult.PasswordReset;
            break;

          // Any other exception.
          default:
            InfoDialog.ShowErrorDialog(Resources.ConnectFailedWarningTitle, string.Format(Resources.GenericConnectionErrorText, mySqlException.Number, mySqlException.Message), null, mySqlException.InnerException != null ? mySqlException.InnerException.Message : null);
            break;
        }
      }
      else
      {
        InfoDialog.ShowErrorDialog(Resources.ConnectFailedWarningTitle, connectionException.Message, null, connectionException.InnerException != null ? connectionException.InnerException.Message : null);
      }

      Globals.ThisAddIn.Application.Cursor = ExcelInterop.XlMousePointer.xlDefault;
      return connectionResult;
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
    public static void UnlockTablesInSession(this MySqlWorkbenchConnection connection)
    {
      const string sql = "UNLOCK TABLES";
      MySqlHelper.ExecuteNonQuery(connection.GetConnectionStringBuilder().ConnectionString, sql);
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

  /// <summary>
  /// Specifies identifiers to indicate the result of a connection test.
  /// </summary>
  public enum TestConnectionResult
  {
    /// <summary>
    /// No connection test was made.
    /// </summary>
    None,

    /// <summary>
    /// An error was thrown by the server and was shown to the user.
    /// </summary>
    ConnectionError,

    /// <summary>
    /// Connection was successful.
    /// </summary>
    ConnectionSuccess,

    /// <summary>
    /// Could not connect to the specified MySQL host.
    /// </summary>
    HostUnreachable,

    /// <summary>
    /// The password of the current user has expired and must be reset.
    /// </summary>
    PasswordExpired,

    /// <summary>
    /// The password of the current user has been reset.
    /// </summary>
    PasswordReset,

    /// <summary>
    /// Could not connect to the MySQL host with the specified password for the current user.
    /// </summary>
    WrongPassword
  }
}
