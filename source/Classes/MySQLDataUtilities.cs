// Copyright (c) 2012-2013, Oracle and/or its affiliates. All rights reserved.
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

namespace MySQL.ForExcel
{
  using System;
  using System.Collections.Generic;
  using System.Data;
  using System.Linq;
  using System.Text;
  using MySql.Data.MySqlClient;
  using MySQL.Utility;
  using MySQL.Utility.Forms;

  /// <summary>
  /// Provides extension methods and other static methods to leverage the work with MySQL data.
  /// </summary>
  public static class MySQLDataUtilities
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

        //// Create a command and prepare it for execution
        MySqlCommand cmd = new MySqlCommand();
        cmd.Connection = baseConnection;
        cmd.CommandText = routineName;
        cmd.CommandType = CommandType.StoredProcedure;

        if (routineParameters != null)
        {
          foreach (MySqlParameter p in routineParameters)
          {
            cmd.Parameters.Add(p);
          }
        }

        //// Create the DataAdapter & DataSet
        MySqlDataAdapter da = new MySqlDataAdapter(cmd);
        DataSet ds = new DataSet();

        //// Fill the DataSet using default values for DataTable names, etc.
        da.Fill(ds);

        //// Detach the MySqlParameters from the command object, so they can be used again.
        cmd.Parameters.Clear();

        //// Return the dataset
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
      DataTable retTable = null;
      DataSet ds = MySqlHelper.ExecuteDataset(connection.GetConnectionStringBuilder().ConnectionString, query);
      if (ds.Tables.Count > 0)
      {
        retTable = ds.Tables[0];
        retTable.AddExtendedProperties(query, true, string.Empty);
      }

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
    public static DataTable GetDataFromTableOrView(this MySqlWorkbenchConnection connection, DBObject dbo, List<string> columnsList, int firstRowIdx, int rowCount)
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
    public static DataTable GetDataFromTableOrView(this MySqlWorkbenchConnection connection, DBObject dbo, List<string> columnsList)
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
    public static DataSet GetDataSetFromProcedure(this MySqlWorkbenchConnection connection, DBObject dbo, params MySqlParameter[] parameters)
    {
      DataSet retDS = null;

      if (dbo.Type == DBObject.DBObjectType.Procedure)
      {
        string sql = string.Format("`{0}`.`{1}`", connection.Schema, dbo.Name);
        retDS = connection.ExecuteRoutine(sql, parameters);
      }

      return retDS;
    }

    /// <summary>
    /// Gets the value of the MAX_ALLOWED_PACKET MySQL Server variable indicating the max size in bytes of the packet returned by a single query.
    /// </summary>
    /// <param name="connection">MySQL Workbench connection to a MySQL server instance selected by users.</param>
    /// <returns>The max size in bytes of the packet returned by a single query.</returns>
    public static ulong GetMySQLServerMaxAllowedPacket(this MySqlWorkbenchConnection connection)
    {
      string sql = "SELECT @@max_allowed_packet";
      object objCount = MySqlHelper.ExecuteScalar(connection.GetConnectionStringBuilder().ConnectionString, sql);
      return objCount != null ? (ulong)objCount : 0;
    }

    /// <summary>
    /// Gets the value of the MAX_ALLOWED_PACKET MySQL Server variable indicating the max size in bytes of the packet returned by a single query.
    /// </summary>
    /// <param name="connection">A MySQL connection.</param>
    /// <returns>The max size in bytes of the packet returned by a single query.</returns>
    public static ulong GetMySQLServerMaxAllowedPacket(this MySqlConnection connection)
    {
      string sql = "SELECT @@max_allowed_packet";
      object objCount = MySqlHelper.ExecuteScalar(connection, sql);
      return objCount != null ? (ulong)objCount : 0;
    }

    /// <summary>
    /// Gets the total number of rows contained in a table or view.
    /// </summary>
    /// <remarks>Only works against Tables or Views, but not with Procedures.</remarks>
    /// <param name="connection">MySQL Workbench connection to a MySQL server instance selected by users.</param>
    /// <param name="dbo">Type of database object to query (Table, View or Procedure).</param>
    /// <returns>The number of rows in a given table or view.</returns>
    public static long GetRowsCountFromTableOrView(this MySqlWorkbenchConnection connection, DBObject dbo)
    {
      if (dbo.Type == DBObject.DBObjectType.Procedure)
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
      DataTable dt = null;
      MySqlDataAdapter mysqlAdapter = null;

      try
      {
        using (MySqlConnection baseConnection = new MySqlConnection(connectionString))
        {
          baseConnection.Open();

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
        MySQLSourceTrace.WriteAppErrorToLog(ex);
        throw;
      }

      return dt;
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
    /// Checks if the given connection may be using SSL.
    /// </summary>
    /// <param name="connection">MySQL Workbench connection to a MySQL server instance selected by users.</param>
    /// <returns><c>true</c> if the connection uses SSL, <c>false</c> otherwise.</returns>
    public static bool IsSSL(this MySqlWorkbenchConnection connection)
    {
      return connection.UseSSL == 1
        || !(string.IsNullOrWhiteSpace(connection.SSLCA)
        && string.IsNullOrWhiteSpace(connection.SSLCert)
        && string.IsNullOrWhiteSpace(connection.SSLCipher)
        && string.IsNullOrWhiteSpace(connection.SSLKey));
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
      return dt != null ? dt.Rows.Count > 0 : false;
    }

    /// <summary>
    /// Tests the current connection until the user enters a correct password.
    /// </summary>
    /// <param name="wbConnection">A <see cref="MySqlWorkbenchConnection"/> object representing the connection to a MySQL server instance selected by users.</param>
    /// <param name="tryConnectionBeforeAskingForPassword">Flag indicating whether a connection test is made with the connection as is before asking for a password</param>
    /// <returns>A <see cref="PasswordDialogFlags"/> containing data about the operation.</returns>
    public static PasswordDialogFlags TestConnectionAndRetryOnWrongPassword(this MySqlWorkbenchConnection wbConnection, bool tryConnectionBeforeAskingForPassword = true)
    {
      PasswordDialogFlags passwordFlags = new PasswordDialogFlags(wbConnection);

      //// Assume a wrong password at first so if the connection is not tested without a password we ensure to ask for one.
      passwordFlags.ConnectionResult = TestConnectionResult.WrongPassword;

      //// First connection attempt with the connection exactly as loaded (maybe without a password).
      if (tryConnectionBeforeAskingForPassword)
      {
        passwordFlags.ConnectionResult = wbConnection.TestConnectionAndReturnResult(false);
        passwordFlags.Cancelled = passwordFlags.ConnectionResult == TestConnectionResult.PasswordExpired;

        ///// If on the first attempt a connection could not be made and not because of a bad password, exit.
        if (!passwordFlags.ConnectionSuccess && !passwordFlags.WrongPassword)
        {
          return passwordFlags;
        }
      }

      //// If the connection does not have a stored password or the stored password failed then ask for one and retry.
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
      Globals.ThisAddIn.Application.Cursor = Microsoft.Office.Interop.Excel.XlMousePointer.xlWait;
      TestConnectionResult connectionResult = TestConnectionResult.None;
      Exception connectionException = null;
      if (connection.TestConnection(out connectionException))
      {
        connectionResult = TestConnectionResult.ConnectionSuccess;
        Globals.ThisAddIn.Application.Cursor = Microsoft.Office.Interop.Excel.XlMousePointer.xlDefault;
        return connectionResult;
      }

      //// If the error returned is about the connection failing the password check, it may be because either the stored password is wrong or no password.
      connectionResult = TestConnectionResult.ConnectionError;
      if (connectionException is MySqlException)
      {
        MySqlException mySqlException = connectionException as MySqlException;
        switch (mySqlException.Number)
        {
          //// Connection could not be made.
          case MySqlWorkbenchConnection.MYSQL_EXCEPTION_NUMBER_SERVER_UNREACHABLE:
            connectionResult = TestConnectionResult.HostUnreachable;
            InfoDialog.ShowErrorDialog(Properties.Resources.ConnectFailedWarningTitle, mySqlException.Message, null, mySqlException.InnerException != null ? mySqlException.InnerException.Message : null);
            break;

          //// Wrong password.
          case MySqlWorkbenchConnection.MYSQL_EXCEPTION_NUMBER_WRONG_PASSWORD:
            connectionResult = TestConnectionResult.WrongPassword;
            if (!string.IsNullOrEmpty(connection.Password) || displayErrorOnEmptyPassword)
            {
              string moreInfoText = connection.IsSSL() ? Properties.Resources.ConnectSSLFailedDetailWarning : null;
              InfoDialog.ShowWarningDialog(Properties.Resources.ConnectFailedWarningTitle, mySqlException.Message, null, moreInfoText);
            }
            break;

          //// Password has expired so any statement can't be run before resetting the expired password.
          case MySqlWorkbenchConnection.MYSQL_EXCEPTION_NUMBER_EXPIRED_PASSWORD:
            PasswordDialogFlags passwordFlags = PasswordDialog.ShowExpiredPasswordDialog(connection, false);
            if (!passwordFlags.Cancelled)
            {
              connection.Password = passwordFlags.NewPassword;
            }

            connectionResult = passwordFlags.Cancelled ? TestConnectionResult.PasswordExpired : TestConnectionResult.PasswordReset;
            break;

          //// Any other exception.
          default:
            InfoDialog.ShowErrorDialog(Properties.Resources.ConnectFailedWarningTitle, string.Format(Properties.Resources.GenericConnectionErrorText, mySqlException.Number, mySqlException.Message), null, mySqlException.InnerException != null ? mySqlException.InnerException.Message : null);
            break;
        }
      }
      else
      {
        InfoDialog.ShowErrorDialog(Properties.Resources.ConnectFailedWarningTitle, connectionException.Message, null, connectionException.InnerException != null ? connectionException.InnerException.Message : null);
      }

      Globals.ThisAddIn.Application.Cursor = Microsoft.Office.Interop.Excel.XlMousePointer.xlDefault;
      return connectionResult;
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
    private static string AssembleSelectQuery(string schemaName, DBObject dbo, List<string> columnsList, int firstRowIdx, int rowCount)
    {
      if (dbo.Type == DBObject.DBObjectType.Procedure)
      {
        return null;
      }

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
        string strCount = rowCount >= 0 ? rowCount.ToString() : "18446744073709551615";
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
