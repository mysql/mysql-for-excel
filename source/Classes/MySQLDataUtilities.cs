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
using System.Globalization;
using System.Linq;
using System.Text;
using MySql.Data.MySqlClient;
using MySQL.ForExcel.Forms;
using MySQL.ForExcel.Interfaces;
using MySQL.ForExcel.Properties;
using MySQL.Utility.Classes;
using MySQL.Utility.Classes.MySQLWorkbench;
using MySQL.Utility.Forms;
using Excel = Microsoft.Office.Interop.Excel;

namespace MySQL.ForExcel.Classes
{
  /// <summary>
  /// Provides extension methods and other static methods to leverage the work with MySQL data.
  /// </summary>
  public static class MySqlDataUtilities
  {
    /// <summary>
    /// Adds or sets the values on extended properties within the <see cref="DataTable"/> object.
    /// </summary>
    /// <param name="dt">A data table where extended properties are set.</param>
    /// <param name="queryString">The last query string used to produce the result set saved in this data table.</param>
    /// <param name="importedHeaders">Flag indicating if the column names where returned by the query and stored in the first row of the data table.</param>
    /// <param name="tableName">The name of the MySQL table queried to produce the data stored in this data table.</param>
    public static void AddExtendedProperties(this DataTable dt, string queryString, bool importedHeaders, string tableName)
    {
      if (dt.ExtendedProperties.ContainsKey("QueryString"))
      {
        dt.ExtendedProperties["QueryString"] = queryString;
      }
      else
      {
        dt.ExtendedProperties.Add("QueryString", queryString);
      }

      if (dt.ExtendedProperties.ContainsKey("ImportedHeaders"))
      {
        dt.ExtendedProperties["ImportedHeaders"] = importedHeaders;
      }
      else
      {
        dt.ExtendedProperties.Add("ImportedHeaders", importedHeaders);
      }

      if (dt.ExtendedProperties.ContainsKey("TableName"))
      {
        dt.ExtendedProperties["TableName"] = tableName;
      }
      else
      {
        dt.ExtendedProperties.Add("TableName", tableName);
      }
    }

    /// <summary>
    /// Escapes special characters that cause problems when passed within queries, from this data value string.
    /// </summary>
    /// <param name="valueToEscape">The data value text containing special characters.</param>
    /// <returns>A new string built from the given data value string withouth the special characters.</returns>
    public static string EscapeDataValueString(this string valueToEscape)
    {
      const string quotesAndOtherDangerousChars =
          "\\" + "\u2216" + "\uFF3C"               // backslashes
        + "'" + "\u00B4" + "\u02B9" + "\u02BC" + "\u02C8" + "\u02CA"
                + "\u0301" + "\u2019" + "\u201A" + "\u2032"
                + "\u275C" + "\uFF07"            // single-quotes
        + "`" + "\u02CB" + "\u0300" + "\u2018" + "\u2035" + "\u275B"
                + "\uFF40"                       // back-tick
        + "\"" + "\u02BA" + "\u030E" + "\uFF02"; // double-quotes

      StringBuilder sb = new StringBuilder();
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
    /// Executes the given query and returns the result set in a <see cref="DataTable"/> object.
    /// </summary>
    /// <param name="connection">MySQL Workbench connection to a MySQL server instance selected by users.</param>
    /// <param name="query">Select query to be sent to the MySQL Server.</param>
    /// <returns>Table containing the results of the query.</returns>
    public static DataTable GetDataFromTableOrView(this MySqlWorkbenchConnection connection, string query)
    {
      DataSet ds = MySqlHelper.ExecuteDataset(connection.GetConnectionStringBuilder().ConnectionString, query);
      if (ds.Tables.Count <= 0)
      {
        return null;
      }

      DataTable retTable = ds.Tables[0];
      retTable.AddExtendedProperties(query, true, string.Empty);
      return retTable;
    }

    /// <summary>
    /// Executes the given query and returns the result set in a <see cref="DataTable"/> object.
    /// </summary>
    /// <remarks>Only works against Tables or Views, but not with Procedures.</remarks>
    /// <param name="connection">MySQL Workbench connection to a MySQL server instance selected by users.</param>
    /// <param name="dbo">Type of database object to query (Table, View or Procedure).</param>
    /// <param name="columnsList">List of queries column names.</param>
    /// <param name="firstRowIdx">Row number from which to start returning results.</param>
    /// <param name="rowCount">Number of rows to return</param>
    /// <returns>Table containing the results of the query.</returns>
    public static DataTable GetDataFromTableOrView(this MySqlWorkbenchConnection connection, DbObject dbo, List<string> columnsList, int firstRowIdx, int rowCount)
    {
      string queryString = AssembleSelectQuery(connection.Schema, dbo, columnsList, firstRowIdx, rowCount);
      return string.IsNullOrEmpty(queryString) ? null : connection.GetDataFromTableOrView(queryString);
    }

    /// <summary>
    /// Executes the given query and returns the result set in a <see cref="DataTable"/> object.
    /// </summary>
    /// <remarks>Only works against Tables or Views, but not with Procedures.</remarks>
    /// <param name="connection">MySQL Workbench connection to a MySQL server instance selected by users.</param>
    /// <param name="dbo">Type of database object to query (Table, View or Procedure).</param>
    /// <param name="columnsList">List of queries column names.</param>
    /// <returns>Table containing the results of the query.</returns>
    public static DataTable GetDataFromTableOrView(this MySqlWorkbenchConnection connection, DbObject dbo, List<string> columnsList)
    {
      return GetDataFromTableOrView(connection, dbo, columnsList, -1, -1);
    }

    /// <summary>
    /// Executes the given procedure and returns its result sets in tables within a <see cref="DataSet"/> object.
    /// </summary>
    /// <remarks>Only works against Procedures, but not with Tables or Views.</remarks>
    /// <param name="connection">MySQL Workbench connection to a MySQL server instance selected by users.</param>
    /// <param name="dbo">Type of database object to query (Table, View or Procedure)</param>
    /// <param name="parameters">Array of arguments passed to the stored procedure parameters.</param>
    /// <returns><see cref="DataSet"/> where each table within it represents each of the result sets returned by the stored procedure.</returns>
    public static DataSet GetDataSetFromProcedure(this MySqlWorkbenchConnection connection, DbObject dbo, params MySqlParameter[] parameters)
    {
      if (dbo.Type != DbObject.DbObjectType.Procedure)
      {
        return null;
      }

      string sql = string.Format("`{0}`.`{1}`", connection.Schema, dbo.Name);
      DataSet retDs = connection.ExecuteRoutine(sql, parameters);
      return retDs;
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
    /// Gets the total number of rows contained in a table or view.
    /// </summary>
    /// <remarks>Only works against Tables or Views, but not with Procedures.</remarks>
    /// <param name="connection">MySQL Workbench connection to a MySQL server instance selected by users.</param>
    /// <param name="dbo">Type of database object to query (Table, View or Procedure).</param>
    /// <returns>The number of rows in a given table or view.</returns>
    public static long GetRowsCountFromTableOrView(this MySqlWorkbenchConnection connection, DbObject dbo)
    {
      if (dbo.Type == DbObject.DbObjectType.Procedure)
      {
        return 0;
      }

      string sql = string.Format("SELECT COUNT(*) FROM `{0}`.`{1}`", connection.Schema, dbo.Name);
      object objCount = MySqlHelper.ExecuteScalar(connection.GetConnectionStringBuilder().ConnectionString, sql);
      return objCount != null ? (long)objCount : 0;
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
      string connectionString = connection.GetConnectionStringBuilder().ConnectionString;
      DataTable dt;

      try
      {
        using (MySqlConnection baseConnection = new MySqlConnection(connectionString))
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
    /// Gets the schema information ofr the given database collection.
    /// </summary>
    /// <param name="dataTable">The data table to get the schema info for.</param>
    /// <returns>Table with schema information.</returns>
    public static DataTable GetSchemaInfo(this DataTable dataTable)
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
      if (string.IsNullOrEmpty(schemaName) || string.IsNullOrEmpty(indexName))
      {
        return false;
      }

      DataTable dt = GetSchemaCollection(connection, "Indexes", null, schemaName, tableName, indexName);
      return dt.Rows.Count > 0;
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
    /// Checks if a table with the given name exists in the given schema.
    /// </summary>
    /// <param name="connection">MySQL Workbench connection to a MySQL server instance selected by users.</param>
    /// <param name="schemaName">Name of the database schema where the table resides.</param>
    /// <param name="tableName">Name of the table to look for.</param>
    /// <returns><c>true</c> if the table exists, <c>false</c> otherwise.</returns>
    public static bool TableExistsInSchema(this MySqlWorkbenchConnection connection, string schemaName, string tableName)
    {
      if (string.IsNullOrEmpty(schemaName) || string.IsNullOrEmpty(tableName))
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
      if (string.IsNullOrEmpty(tableName))
      {
        return false;
      }

      string sql = string.Format("SHOW KEYS FROM `{0}` IN `{1}` WHERE Key_name = 'PRIMARY';", tableName, connection.Schema);
      DataTable dt = GetDataFromTableOrView(connection, sql);
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
      Globals.ThisAddIn.Application.Cursor = Excel.XlMousePointer.xlWait;
      TestConnectionResult connectionResult;
      Exception connectionException;
      if (connection.TestConnection(out connectionException))
      {
        connectionResult = TestConnectionResult.ConnectionSuccess;
        Globals.ThisAddIn.Application.Cursor = Excel.XlMousePointer.xlDefault;
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

      Globals.ThisAddIn.Application.Cursor = Excel.XlMousePointer.xlDefault;
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
    /// Verifies if a statement result was applied to the server, i.e. that it was successful or had warnings.
    /// </summary>
    /// <param name="statementResult">The statement result to evaluate.</param>
    /// <returns><c>true</c> if the result is successful or had warnings, <c>false</c> otherwise.</returns>
    public static bool WasApplied(this MySqlStatement.StatementResultType statementResult)
    {
      return statementResult == MySqlStatement.StatementResultType.Successful || statementResult == MySqlStatement.StatementResultType.WarningsFound;
    }

    /// <summary>
    /// Creates a SELECT query against a Table or View database object.
    /// </summary>
    /// <remarks>Only works against Tables or Views, but not with Procedures.</remarks>
    /// <param name="schemaName">Name of the schema (database) where the Table or View resides.</param>
    /// <param name="dbo">Type of database object to query (Table, View or Procedure).</param>
    /// <param name="columnsList">List of queries column names.</param>
    /// <param name="firstRowIdx">Row number from which to start returning results.</param>
    /// <param name="rowCount">Number of rows to return</param>
    /// <returns>The SELECT query text.</returns>
    private static string AssembleSelectQuery(string schemaName, DbObject dbo, ICollection<string> columnsList, int firstRowIdx, int rowCount)
    {
      if (dbo.Type == DbObject.DbObjectType.Procedure)
      {
        return null;
      }

      const string bigRowCountLimit = "18446744073709551615";
      StringBuilder queryStringBuilder = new StringBuilder("SELECT ");
      if (columnsList == null || columnsList.Count == 0)
      {
        queryStringBuilder.Append("*");
      }
      else
      {
        foreach (string columnName in columnsList)
        {
          queryStringBuilder.AppendFormat("`{0}`,", columnName.Replace("`", "``"));
        }

        queryStringBuilder.Remove(queryStringBuilder.Length - 1, 1);
      }

      queryStringBuilder.AppendFormat(" FROM `{0}`.`{1}`", schemaName, dbo.Name);
      if (firstRowIdx > 0)
      {
        string strCount = rowCount >= 0 ? rowCount.ToString(CultureInfo.InvariantCulture) : bigRowCountLimit;
        queryStringBuilder.AppendFormat(" LIMIT {0},{1}", firstRowIdx, strCount);
      }
      else if (rowCount >= 0)
      {
        queryStringBuilder.AppendFormat(" LIMIT {0}", rowCount);
      }

      return queryStringBuilder.ToString();
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
