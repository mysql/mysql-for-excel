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
using System.Data;
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
    private const string STATEMENT_CREATE_TABLE = "CREATE TABLE";

    /// <summary>
    /// Key word used for an DELETE statement.
    /// </summary>
    private const string STATEMENT_DELETE = "DELETE";

    /// <summary>
    /// Key word used for an INSERT statement.
    /// </summary>
    private const string STATEMENT_INSERT = "INSERT";

    /// <summary>
    /// Key word used for an UPDATE statement.
    /// </summary>
    private const string STATEMENT_UPDATE = "UPDATE";

    #endregion Constants

    #region Fields

    /// <summary>
    /// The <see cref="IMySqlDataRow"/> object holding a SQL statement to be applied against the database.
    /// </summary>
    private readonly IMySqlDataRow _mySqlRow;

    /// <summary>
    /// The query text of this SQL statement to be applied against the database.
    /// </summary>
    private string _sqlQuery;

    #endregion Fields

    /// <summary>
    /// Initializes a new instance of the <see cref="MySqlStatement"/> class.
    /// </summary>
    /// <param name="mySqlRow">The <see cref="IMySqlDataRow"/> object holding a SQL statement to be applied against the database.</param>
    public MySqlStatement(IMySqlDataRow mySqlRow)
    {
      _mySqlRow = mySqlRow;
      _sqlQuery = string.Empty;
      AffectedRows = 0;
      ExecutionOrder = 0;
      ResultText = string.Empty;
      StatementResult = StatementResultType.NotApplied;
      WarningsQuantity = 0;
    }

    /// <summary>
    /// Describes the type of operation done against the database server.
    /// </summary>
    public enum SqlStatementType
    {
      /// <summary>
      /// No statement or unrecognized one.
      /// </summary>
      Unknown,

      /// <summary>
      /// Statement to create a new table before rows are inserted to it.
      /// </summary>
      CreateTable,

      /// <summary>
      /// Statement to insert new rows into the corresponding database table.
      /// </summary>
      Insert,

      /// <summary>
      /// Statement to delete rows from the corresponding database table.
      /// </summary>
      Delete,

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
    /// Gets the result of the query after it is executed.
    /// </summary>
    public StatementResultType StatementResult { get; private set; }

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

    #endregion Properties

    /// <summary>
    /// Gets the corresponding <see cref="SqlStatementType"/> for a <see cref="DataRowState"/> enumeration.
    /// </summary>
    /// <param name="rowState">The state of a <see cref="DataRow"/> object.</param>
    /// <returns>Type of operation done against the database server</returns>
    public static SqlStatementType GetRelatedStatementType(DataRowState rowState)
    {
      SqlStatementType operationType = SqlStatementType.Unknown;
      switch (rowState)
      {
        case DataRowState.Deleted:
          operationType = SqlStatementType.Delete;
          break;

        case DataRowState.Added:
          operationType = SqlStatementType.Insert;
          break;

        case DataRowState.Modified:
          operationType = SqlStatementType.Update;
          break;
      }

      return operationType;
    }

    /// <summary>
    /// Gets the corresponding <see cref="SqlStatementType"/> for a SQL statement.
    /// </summary>
    /// <param name="sqlStatement">The SQL statement.</param>
    /// <returns>Type of operation done against the database server by the given statement.</returns>
    public static SqlStatementType GetSqlStatementType(string sqlStatement)
    {
      if (string.IsNullOrEmpty(sqlStatement))
      {
        return SqlStatementType.Unknown;
      }

      SqlStatementType statementType = SqlStatementType.Unknown;
      sqlStatement = sqlStatement.TrimStart().ToUpperInvariant();
      if (sqlStatement.StartsWith(STATEMENT_DELETE))
      {
        statementType = SqlStatementType.Delete;
      }
      else if (sqlStatement.StartsWith(STATEMENT_INSERT))
      {
        statementType = SqlStatementType.Insert;
      }
      else if (sqlStatement.StartsWith(STATEMENT_UPDATE))
      {
        statementType = SqlStatementType.Update;
      }
      else if (sqlStatement.StartsWith(STATEMENT_CREATE_TABLE))
      {
        statementType = SqlStatementType.CreateTable;
      }

      return statementType;
    }

    /// <summary>
    /// Executes the statement pushing its related changes to the MySQL server connected in the given <see cref="MySqlCommand"/>.
    /// </summary>
    /// <param name="mySqlTransaction">The <see cref="MySqlTransaction"/> transaction this statement belongs to.</param>
    /// <param name="executionOrder">The numeric index with the order in which this statement was executed.</param>
    /// <param name="useOptimisticUpdate">Flag indicating whether optimistic locking is used for the update of rows.</param>
    public void Execute(MySqlTransaction mySqlTransaction, uint executionOrder, bool useOptimisticUpdate)
    {
      StatementResult = StatementResultType.NotApplied;
      if (_mySqlRow == null)
      {
        return;
      }

      ExecutionOrder = executionOrder;
      DataSet warningsDs = null;
      MySqlCommand mySqlCommand = null;
      try
      {
        WarningsQuantity = 0;
        StringBuilder warningText = new StringBuilder();
        mySqlCommand = new MySqlCommand(SqlQuery, mySqlTransaction.Connection, mySqlTransaction);
        AffectedRows = mySqlCommand.ExecuteNonQuery();
        warningsDs = MySqlHelper.ExecuteDataset(mySqlCommand.Connection, "SHOW WARNINGS");
        if ((warningsDs != null && warningsDs.Tables.Count > 0 && warningsDs.Tables[0].Rows.Count > 0)
            || (AffectedRows == 0 && StatementType != SqlStatementType.CreateTable))
        {
          string nl = string.Empty;
          if (AffectedRows == 0)
          {
            WarningsQuantity++;
            _mySqlRow.RowError = NO_MATCH;
            warningText.AppendFormat(
              "{2}{0:000}: {1}",
              ExecutionOrder,
              string.Format(Resources.QueryDidNotMatchRowsWarning, useOptimisticUpdate ? string.Empty : Resources.PrimaryKeyText),
              nl);
            nl = Environment.NewLine;
          }

          if (warningsDs != null)
          {
            foreach (DataRow warningRow in warningsDs.Tables[0].Rows)
            {
              WarningsQuantity++;
              warningText.AppendFormat(
                "{3}{0:000}: {1} - {2}",
                ExecutionOrder,
                warningRow[1],
                warningRow[2],
                nl);
              nl = Environment.NewLine;
            }
          }

          StatementResult = StatementResultType.WarningsFound;
          ResultText = warningText.ToString();
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
      finally
      {
        if (mySqlCommand != null)
        {
          mySqlCommand.Dispose();
        }

        if (warningsDs != null)
        {
          warningsDs.Dispose();
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
  }
}
