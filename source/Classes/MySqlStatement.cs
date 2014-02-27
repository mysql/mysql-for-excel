// Copyright (c) 2013, Oracle and/or its affiliates. All rights reserved.
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
using System.Globalization;
using System.Text;
using MySql.Data.MySqlClient;
using MySQL.ForExcel.Interfaces;
using MySQL.ForExcel.Properties;
using MySQL.Utility.Classes;

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
    /// Key words used for a CREATE TABLE statement.
    /// </summary>
    public const string STATEMENT_CREATE_TABLE = "CREATE TABLE";

    /// <summary>
    /// Key word used for an DELETE statement.
    /// </summary>
    public const string STATEMENT_DELETE = "DELETE FROM";

    /// <summary>
    /// Key word used for an INSERT statement.
    /// </summary>
    public const string STATEMENT_INSERT = "INSERT INTO";

    /// <summary>
    /// Key word used for a LOCK TABLES statement.
    /// </summary>
    public const string STATEMENT_LOCK_TABLES = "LOCK TABLES";

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
    /// The <see cref="IMySqlDataRow"/> object holding a SQL statement to be applied against the database.
    /// </summary>
    private readonly IMySqlDataRow _mySqlRow;

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
      _mySqlRow = mySqlRow;
      _sqlQuery = string.Empty;
      _warningsBuilder = null;
      AffectedRows = 0;
      ExecutionOrder = 0;
      ResultText = string.Empty;
      StatementResult = StatementResultType.NotApplied;
      StatementsQuantityFormat = STATEMENTS_QUANTITY_DEFAULT_FORMAT;
      WarningsQuantity = 0;
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
      /// Statement to create a new table.
      /// </summary>
      CreateTable,

      /// <summary>
      /// Statement to delete rows from the corresponding database table.
      /// </summary>
      Delete,

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
      /// Statement was not applied, user cancelled it.
      /// </summary>
      NotApplied,

      /// <summary>
      /// Statement executed successfully and transaction committed.
      /// </summary>
      Successful,

      /// <summary>
      /// Statement had errors so transaction was rolled back.
      /// </summary>
      ErrorThrown,

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
    /// Gets the text returned by the MySQL server after executing this statement.
    /// </summary>
    public string ResultText { get; private set; }

    /// <summary>
    /// Gets the query text of this SQL statement to be applied against the database.
    /// </summary>
    public string SqlQuery
    {
      get
      {
        string freshQuery = _mySqlRow != null ? _mySqlRow.GetSql() : string.Empty;
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
    public SqlStatementType StatementType
    {
      get
      {
        return GetSqlStatementType(SqlQuery);
      }
    }

    /// <summary>
    /// Gets the result of the query after it is executed.
    /// </summary>
    public StatementResultType StatementResult { get; private set; }

    /// <summary>
    /// Gets or sets the format for displaying the statement execution order.
    /// </summary>
    public string StatementsQuantityFormat
    {
      get
      {
        return _statementsQuantityFormat;
      }

      set
      {
        _statementsQuantityFormat = value;
        _executionOrderFormat = "{0:" + _statementsQuantityFormat + "}";
      }
    }

    /// <summary>
    /// Gets a value indicating whether the statement was applied either successfuly or with warnings.
    /// </summary>
    public bool StatementWasApplied
    {
      get
      {
        return StatementResult.WasApplied();
      }
    }

    /// <summary>
    /// Gets the quantity of warnings thrown by executing the statement.
    /// </summary>
    public int WarningsQuantity { get; private set; }

    /// <summary>
    /// Gets the corresponding Excel row number converted to string.
    /// </summary>
    private string ExcelRowText
    {
      get
      {
        return _mySqlRow != null && _mySqlRow.ExcelRow > 0 ? _mySqlRow.ExcelRow.ToString(CultureInfo.InvariantCulture) : string.Empty;
      }
    }

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

      SqlStatementType statementType = SqlStatementType.Other;
      sqlStatement = sqlStatement.TrimStart().ToUpperInvariant();
      if (sqlStatement.StartsWith(STATEMENT_UPDATE))
      {
        statementType = SqlStatementType.Update;
      }
      else if (sqlStatement.StartsWith(STATEMENT_INSERT))
      {
        statementType = SqlStatementType.Insert;
      }
      else if (sqlStatement.StartsWith(STATEMENT_DELETE))
      {
        statementType = SqlStatementType.Delete;
      }
      else if (sqlStatement.StartsWith(STATEMENT_CREATE_TABLE))
      {
        statementType = SqlStatementType.CreateTable;
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
      if (mySqlCommand == null)
      {
        throw new ArgumentNullException("mySqlCommand");
      }

      StatementResult = StatementResultType.NotApplied;
      if (_mySqlRow == null)
      {
        return;
      }

      ExecutionOrder = executionOrder;
      try
      {
        WarningsQuantity = 0;
        mySqlCommand.CommandText = SqlQuery;
        mySqlCommand.Connection.InfoMessage -= FormatWarnings;
        mySqlCommand.Connection.InfoMessage += FormatWarnings;
        AffectedRows = mySqlCommand.ExecuteNonQuery();
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
        StatementResult = StatementResultType.ErrorThrown;
        AffectedRows = 0;
        _mySqlRow.RowError = ex.Message;
        MySqlSourceTrace.WriteAppErrorToLog(ex);
        if (ex is MySqlException)
        {
          MySqlException mysqlEx = ex as MySqlException;
          ResultText = string.Format(Resources.ErrorMySQLText, mysqlEx.Number) + Environment.NewLine + mysqlEx.Message;
        }
        else
        {
          ResultText = Resources.ErrorAdoNetText + Environment.NewLine + ex.Message;
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

      string nl = string.Empty;
      if (_warningsBuilder == null)
      {
        _warningsBuilder = new StringBuilder(MiscUtilities.STRING_BUILDER_DEFAULT_CAPACITY);
      }
      else
      {
        nl = Environment.NewLine;
      }

      string excelRowText = ExcelRowText;
      bool addExcelRowText = !string.IsNullOrEmpty(excelRowText);
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

      _mySqlRow.RowError = NO_MATCH;
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
