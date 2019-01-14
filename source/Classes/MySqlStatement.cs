// Copyright (c) 2013, 2019, Oracle and/or its affiliates. All rights reserved.
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
using System.Globalization;
using System.Text;
using MySql.Data.MySqlClient;
using MySql.Utility.Classes.Logging;
using MySQL.ForExcel.Interfaces;
using MySQL.ForExcel.Properties;

namespace MySQL.ForExcel.Classes
{
  public class MySqlStatement
  {
    #region Constants

    /// <summary>
    /// String to identify the error code in a <see cref="IMySqlDataRow"/> as to be related to a row with the given primary key not being found in the MySQL table.
    /// </summary>
    public const string NO_MATCH = "NO_MATCH";

    /// <summary>
    /// OK text used to flag a successful statement execution.
    /// </summary>
    private const string OK_TEXT = "OK";

    /// <summary>
    /// Key words used for a CREATE TABLE statement.
    /// </summary>
    public const string STATEMENT_ALTER_TABLE = "ALTER TABLE";

    /// <summary>
    /// Key words used to specify a collation in a CREATE SCHEMA or CREATE TABLE statement.
    /// </summary>
    public const string STATEMENT_COLLATE = "COLLATE";

    /// <summary>
    /// Key words used for a CREATE SCHEMA statement.
    /// </summary>
    public const string STATEMENT_CREATE_SCHEMA = "CREATE SCHEMA";

    /// <summary>
    /// Key words used for a CREATE TABLE statement.
    /// </summary>
    public const string STATEMENT_CREATE_TABLE = "CREATE TABLE";

    /// <summary>
    /// Key words used to specify a default character set in a CREATE SCHEMA or CREATE TABLE statement.
    /// </summary>
    public const string STATEMENT_DEFAULT_CHARSET = "DEFAULT CHARACTER SET";

    /// <summary>
    /// Key word used for a DELETE statement.
    /// </summary>
    public const string STATEMENT_DELETE = "DELETE FROM";

    /// <summary>
    /// Key word used for a GRANT ALL statement.
    /// </summary>
    public const string STATEMENT_GRANT_ALL = "GRANT ALL ON";

    /// <summary>
    /// Key word used for an INSERT statement.
    /// </summary>
    public const string STATEMENT_INSERT = "INSERT INTO";

    /// <summary>
    /// Key word used for an INSERT IGNORE statement.
    /// </summary>
    public const string STATEMENT_INSERT_IGNORE = "INSERT IGNORE INTO";

    /// <summary>
    /// Key word used for a LOCK TABLES statement.
    /// </summary>
    public const string STATEMENT_LOCK_TABLES = "LOCK TABLES";

    /// <summary>
    /// Key word used for a REPLACE statement.
    /// </summary>
    public const string STATEMENT_REPLACE = "REPLACE INTO";

    /// <summary>
    /// Key word used for a SET statement.
    /// </summary>
    public const string STATEMENT_SET = "SET";

    /// <summary>
    /// Key word used for a SET GLOBAL statement.
    /// </summary>
    public const string STATEMENT_SET_GLOBAL = "SET GLOBAL";

    /// <summary>
    /// Key word used for a LOCK TABLES statement.
    /// </summary>
    public const string STATEMENT_UNLOCK_TABLES = "UNLOCK TABLES";

    /// <summary>
    /// Key word used for an UPDATE statement.
    /// </summary>
    public const string STATEMENT_UPDATE = "UPDATE";

    /// <summary>
    /// The default format for displaying the statement execution order.
    /// </summary>
    private const string STATEMENTS_QUANTITY_DEFAULT_FORMAT = "000";

    #endregion Constants

    #region Fields

    /// <summary>
    /// The format string used for the executed statement index.
    /// </summary>
    private string _executionOrderFormat;

    /// <summary>
    /// The query text of this SQL statement to be applied against the database.
    /// </summary>
    private string _sqlQuery;

    /// <summary>
    /// The format for displaying the statement execution order.
    /// </summary>
    private string _statementsQuantityFormat;

    /// <summary>
    /// The <see cref="StringBuilder"/> holding the warnings from an executed statement.
    /// </summary>
    private StringBuilder _warningsBuilder;

    #endregion Fields

    /// <summary>
    /// Initializes a new instance of the <see cref="MySqlStatement"/> class.
    /// </summary>
    /// <param name="mySqlRow">The <see cref="IMySqlDataRow"/> object holding a SQL statement to be applied against the database.</param>
    public MySqlStatement(IMySqlDataRow mySqlRow)
    {
      _sqlQuery = string.Empty;
      _warningsBuilder = null;
      AffectedRows = 0;
      ExecutionOrder = 0;
      MySqlRow = mySqlRow;
      ResultText = string.Empty;
      SetVariablesSqlQuery = null;
      StatementResult = StatementResultType.NotApplied;
      StatementsQuantityFormat = STATEMENTS_QUANTITY_DEFAULT_FORMAT;
      WarningsQuantity = 0;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="MySqlStatement"/> class.
    /// </summary>
    /// <param name="mySqlRow">A <see cref="MySqlDummyRow"/> instance.</param>
    /// <param name="errorMessage">An error message.</param>
    /// <remarks>This constructor is meant to be used to create a dummy statement that holds just an error message.</remarks>
    public MySqlStatement(MySqlDummyRow mySqlRow, string errorMessage)
      : this(mySqlRow)
    {
      ResultText = errorMessage;
      StatementResult = StatementResultType.ErrorThrown;
    }

    #region Enumerations

    /// <summary>
    /// Describes the type of operation done against the database server.
    /// </summary>
    public enum SqlStatementType
    {
      /// <summary>
      /// Statement to alter the definition of an existing table.
      /// </summary>
      AlterTable,

      /// <summary>
      /// Statement to create a new schema.
      /// </summary>
      CreateSchema,

      /// <summary>
      /// Statement to create a new table.
      /// </summary>
      CreateTable,

      /// <summary>
      /// Statement to delete rows from the corresponding database table.
      /// </summary>
      Delete,

      /// <summary>
      /// Statement to grant all privileges on a MySQL object to the user.
      /// </summary>
      GrantAll,

      /// <summary>
      /// Statement to insert new rows into the corresponding database table.
      /// </summary>
      Insert,

      /// <summary>
      /// Statement to lock a database table.
      /// </summary>
      LockTables,

      /// <summary>
      /// No statement.
      /// </summary>
      None,

      /// <summary>
      /// Statement unrelated to the common operations in MySQL for Excel.
      /// </summary>
      Other,

      /// <summary>
      /// Statement to set user variables.
      /// </summary>
      Set,

      /// <summary>
      /// Statement to set system variables.
      /// </summary>
      SetGlobal,

      /// <summary>
      /// Statement to unlock database tables locked in this session.
      /// </summary>
      UnlockTables,

      /// <summary>
      /// Operation to update rows from the corresponding database table.
      /// </summary>
      Update
    }

    /// <summary>
    /// Specifies identifiers to indicate the type of result of a query execution.
    /// </summary>
    public enum StatementResultType
    {
      /// <summary>
      /// Connection was lost so statement could not be applied.
      /// </summary>
      ConnectionLost,

      /// <summary>
      /// Statement had errors so transaction was rolled back.
      /// </summary>
      ErrorThrown,

      /// <summary>
      /// Statement was not applied, user cancelled it.
      /// </summary>
      NotApplied,

      /// <summary>
      /// Statement executed successfully and transaction committed.
      /// </summary>
      Successful,

      /// <summary>
      /// Statement executed, transaction was committed but warnings were found.
      /// </summary>
      WarningsFound
    }

    #endregion Enumerations

    #region Properties

    /// <summary>
    /// Gets the quantity of rows affected by this statement after it is executed.
    /// </summary>
    public int AffectedRows { get; private set; }

    /// <summary>
    /// Gets the numeric index with the order in which this statement was executed.
    /// </summary>
    public uint ExecutionOrder { get; private set; }

    /// <summary>
    /// Gets the <see cref="IMySqlDataRow"/> object holding a SQL statement to be applied against the database.
    /// </summary>
    public IMySqlDataRow MySqlRow { get; }

    /// <summary>
    /// Gets the text returned by the MySQL server after executing this statement.
    /// </summary>
    public string ResultText { get; private set; }

    /// <summary>
    /// Gets an optional query text that sets values of user variables used in the main <see cref="SqlQuery"/>.
    /// </summary>
    public string SetVariablesSqlQuery { get; set; }

    /// <summary>
    /// Gets the query text of this SQL statement to be applied against the database.
    /// </summary>
    public string SqlQuery
    {
      get
      {
        string setVariablesSqlQuery = null;
        var freshQuery = MySqlRow != null ? MySqlRow.GetSql(out setVariablesSqlQuery) : string.Empty;
        SetVariablesSqlQuery = setVariablesSqlQuery;
        if (!string.IsNullOrEmpty(freshQuery))
        {
          _sqlQuery = freshQuery;
        }

        return _sqlQuery;
      }
    }

    /// <summary>
    /// Gets the type of operation to be performed against the database.
    /// </summary>
    public SqlStatementType StatementType => GetSqlStatementType(SqlQuery);

    /// <summary>
    /// Gets the result of the query after it is executed.
    /// </summary>
    public StatementResultType StatementResult { get; private set; }

    /// <summary>
    /// Gets or sets the format for displaying the statement execution order.
    /// </summary>
    public string StatementsQuantityFormat
    {
      get => _statementsQuantityFormat;

      set
      {
        _statementsQuantityFormat = value;
        _executionOrderFormat = "{0:" + _statementsQuantityFormat + "}";
      }
    }

    /// <summary>
    /// Gets a value indicating whether the statement was applied either successfully or with warnings.
    /// </summary>
    public bool StatementWasApplied => StatementResult.WasApplied();

    /// <summary>
    /// Gets the quantity of warnings thrown by executing the statement.
    /// </summary>
    public int WarningsQuantity { get; private set; }

    /// <summary>
    /// Gets the corresponding Excel row number converted to string.
    /// </summary>
    private string ExcelRowText => MySqlRow != null && MySqlRow.ExcelRow > 0
                                    ? MySqlRow.ExcelRow.ToString(CultureInfo.InvariantCulture)
                                    : string.Empty;

    #endregion Properties

    /// <summary>
    /// Gets the corresponding <see cref="SqlStatementType"/> for a SQL statement.
    /// </summary>
    /// <param name="sqlStatement">The SQL statement.</param>
    /// <returns>Type of operation done against the database server by the given statement.</returns>
    public static SqlStatementType GetSqlStatementType(string sqlStatement)
    {
      if (string.IsNullOrEmpty(sqlStatement))
      {
        return SqlStatementType.None;
      }

      var statementType = SqlStatementType.Other;
      sqlStatement = sqlStatement.TrimStart().ToUpperInvariant();
      if (sqlStatement.StartsWith(STATEMENT_UPDATE))
      {
        statementType = SqlStatementType.Update;
      }
      else if (sqlStatement.StartsWith(STATEMENT_INSERT)
               || sqlStatement.StartsWith(STATEMENT_INSERT_IGNORE)
               || sqlStatement.StartsWith(STATEMENT_REPLACE))
      {
        statementType = SqlStatementType.Insert;
      }
      else if (sqlStatement.StartsWith(STATEMENT_DELETE))
      {
        statementType = SqlStatementType.Delete;
      }
      else if (sqlStatement.StartsWith(STATEMENT_SET_GLOBAL))
      {
        statementType = SqlStatementType.SetGlobal;
      }
      else if (sqlStatement.StartsWith(STATEMENT_SET))
      {
        statementType = SqlStatementType.Set;
      }
      else if (sqlStatement.StartsWith(STATEMENT_CREATE_TABLE))
      {
        statementType = SqlStatementType.CreateTable;
      }
      else if (sqlStatement.StartsWith(STATEMENT_CREATE_SCHEMA))
      {
        statementType = SqlStatementType.CreateSchema;
      }
      else if (sqlStatement.StartsWith(STATEMENT_ALTER_TABLE))
      {
        statementType = SqlStatementType.AlterTable;
      }
      else if (sqlStatement.StartsWith(STATEMENT_LOCK_TABLES))
      {
        statementType = SqlStatementType.LockTables;
      }
      else if (sqlStatement.StartsWith(STATEMENT_UNLOCK_TABLES))
      {
        statementType = SqlStatementType.UnlockTables;
      }
      else if (sqlStatement.StartsWith(STATEMENT_GRANT_ALL))
      {
        statementType = SqlStatementType.GrantAll;
      }

      return statementType;
    }

    /// <summary>
    /// Executes the statement pushing its related changes to the MySQL server connected in the given <see cref="MySqlCommand"/>.
    /// </summary>
    /// <param name="mySqlCommand">The <see cref="MySqlCommand"/> used to issue the statement to the server for execution.</param>
    /// <param name="executionOrder">The numeric index with the order in which this statement was executed.</param>
    /// <param name="useOptimisticUpdate">Flag indicating whether optimistic locking is used for the update of rows.</param>
    public void Execute(MySqlCommand mySqlCommand, uint executionOrder, bool useOptimisticUpdate)
    {
      ExecutionOrder = executionOrder;
      if (mySqlCommand == null)
      {
        throw new ArgumentNullException(nameof(mySqlCommand));
      }

      StatementResult = StatementResultType.NotApplied;
      if (MySqlRow == null || string.IsNullOrEmpty(SqlQuery))
      {
        return;
      }

      if (mySqlCommand.Connection == null || mySqlCommand.Connection.State != ConnectionState.Open)
      {
        StatementResult = StatementResultType.ConnectionLost;
        ResultText = Resources.ConnectionLostErrorText;
        MySqlRow.RowError = ResultText;
        return;
      }

      try
      {
        // Initialize warnings related code.
        WarningsQuantity = 0;
        mySqlCommand.Connection.InfoMessage -= FormatWarnings;
        mySqlCommand.Connection.InfoMessage += FormatWarnings;

        // If the optional SET statement exists, execute it first.
        if (!string.IsNullOrEmpty(SetVariablesSqlQuery))
        {
          mySqlCommand.CommandText = SetVariablesSqlQuery;
          mySqlCommand.ExecuteNonQuery();
        }

        // Execute the main query.
        mySqlCommand.CommandText = SqlQuery;
        AffectedRows = mySqlCommand.ExecuteNonQuery();

        // Disable warnings event and process warnings.
        mySqlCommand.Connection.InfoMessage -= FormatWarnings;
        if (WarningsQuantity > 0 || (AffectedRows == 0 && StatementType.AffectsRowsOnServer()))
        {
          FormatOptimisticUpdateWarning(useOptimisticUpdate);
          StatementResult = StatementResultType.WarningsFound;
          ResultText = _warningsBuilder.ToString();
          _warningsBuilder.Clear();
          _warningsBuilder = null;
        }
        else
        {
          StatementResult = StatementResultType.Successful;
          ResultText = OK_TEXT;
        }
      }
      catch (Exception ex)
      {
        var baseException = ex.GetBaseException();
        StatementResult = StatementResultType.ErrorThrown;
        AffectedRows = 0;
        MySqlRow.RowError = baseException.Message;
        Logger.LogException(baseException);
        if (baseException is MySqlException mysqlEx)
        {
          ResultText = string.Format(Resources.ErrorMySQLText, mysqlEx.Number) + Environment.NewLine + mysqlEx.Message;
        }
        else
        {
          ResultText = Resources.ErrorAdoNetText + Environment.NewLine + baseException.Message;
        }
      }
    }

    /// <summary>
    /// Computes the result of joining a <see cref="StatementResultType"/> with the current one.
    /// </summary>
    /// <param name="anotherResult">A <see cref="StatementResultType"/> to join with the current one.</param>
    /// <returns>A resulting <see cref="StatementResultType"/> after joining the result with another one.</returns>
    public StatementResultType JoinResultTypes(StatementResultType anotherResult)
    {
      if (anotherResult == StatementResultType.ConnectionLost || StatementResult == StatementResultType.ConnectionLost)
      {
        return StatementResultType.ConnectionLost;
      }

      if (anotherResult == StatementResultType.ErrorThrown || StatementResult == StatementResultType.ErrorThrown)
      {
        return StatementResultType.ErrorThrown;
      }

      if (anotherResult == StatementResultType.WarningsFound || StatementResult == StatementResultType.WarningsFound)
      {
        return StatementResultType.WarningsFound;
      }

      if (anotherResult == StatementResultType.Successful || StatementResult == StatementResultType.Successful)
      {
        return StatementResultType.Successful;
      }

      return StatementResultType.NotApplied;
    }

    /// <summary>
    /// Formats the warnings returned by an executed query contained in a <see cref="MySqlConnection"/> instance.
    /// </summary>
    /// <param name="sender">Sender object.</param>
    /// <param name="args">Event arguments.</param>
    private void FormatWarnings(object sender, MySqlInfoMessageEventArgs args)
    {
      if (args.errors.Length == 0)
      {
        return;
      }

      var nl = string.Empty;
      if (_warningsBuilder == null)
      {
        _warningsBuilder = new StringBuilder(MiscUtilities.STRING_BUILDER_DEFAULT_CAPACITY);
      }
      else
      {
        nl = Environment.NewLine;
      }

      var excelRowText = ExcelRowText;
      var addExcelRowText = !string.IsNullOrEmpty(excelRowText);
      foreach (var warning in args.errors)
      {
        WarningsQuantity++;
        _warningsBuilder.Append(nl);
        _warningsBuilder.AppendFormat(_executionOrderFormat, ExecutionOrder);
        _warningsBuilder.AppendFormat(": {0} - {1}", warning.Code, warning.Message);
        if (addExcelRowText)
        {
          _warningsBuilder.AppendFormat(" (Excel row: {0}).", excelRowText);
        }

        nl = Environment.NewLine;
      }
    }

    /// <summary>
    /// Formats the warnings returned by an executed query contained in a <see cref="MySqlConnection"/> instance.
    /// </summary>
    /// <param name="useOptimisticUpdate">Flag indicating whether optimistic locking is used for the update of rows.</param>
    private void FormatOptimisticUpdateWarning(bool useOptimisticUpdate)
    {
      if (AffectedRows > 0)
      {
        return;
      }

      if (_warningsBuilder == null)
      {
        _warningsBuilder = new StringBuilder(MiscUtilities.STRING_BUILDER_DEFAULT_CAPACITY);
      }

      MySqlRow.RowError = NO_MATCH;
      WarningsQuantity++;
      _warningsBuilder.AppendFormat(_executionOrderFormat, ExecutionOrder);
      _warningsBuilder.Append(": ");
      _warningsBuilder.AppendFormat(
        Resources.QueryDidNotMatchRowsWarning,
        useOptimisticUpdate ? string.Empty : Resources.PrimaryKeyText,
        !string.IsNullOrEmpty(ExcelRowText) ? ExcelRowText + " " : string.Empty);
    }
  }
}
