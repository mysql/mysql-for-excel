// Copyright (c) 2013-2014, Oracle and/or its affiliates. All rights reserved.
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
using System.Diagnostics;
using System.Text;
using MySQL.ForExcel.Classes.Exceptions;
using MySQL.ForExcel.Interfaces;
using MySQL.ForExcel.Properties;
using MySQL.Utility.Classes;
using Excel = Microsoft.Office.Interop.Excel;

namespace MySQL.ForExcel.Classes
{
  /// <summary>
  /// Represents a table row holding MySQL data mapped to Excel cells.
  /// </summary>
  public class MySqlDataRow : DataRow, IMySqlDataRow
  {
    #region Fields

    /// <summary>
    /// Gets the parent <see cref="MySqlDataTable"/> for this row.
    /// </summary>
    private MySqlDataTable _mySqlTable;

    /// <summary>
    /// The SQL query needed to commit changes contained in this row to the SQL server.
    /// </summary>
    private string _sqlQuery;

    #endregion Fields

    /// <summary>
    /// Initializes a new instance of the DataRow. Constructs a row from the builder.
    /// </summary>
    /// <remarks>Only for internal usage.</remarks>
    /// <param name="builder">A <see cref="DataRowBuilder"/> to construct the row.</param>
    protected internal MySqlDataRow(DataRowBuilder builder) : base(builder)
    {
      _mySqlTable = null;
      _sqlQuery = null;
      ChangedColumnNames = new List<string>(Table.Columns.Count);
      ExcelRange = null;
      IsBeingDeleted = false;
      IsHeadersRow = false;
      ExcelModifiedRangesList = new List<Excel.Range>(Table.Columns.Count);
      Statement = new MySqlStatement(this);
    }

    #region Properties

    /// <summary>
    /// Gets a list of column names with data changes.
    /// </summary>
    public List<string> ChangedColumnNames { get; private set; }

    /// <summary>
    /// Gets or sets the Excel range representing the whole data row.
    /// </summary>
    public Excel.Range ExcelRange { get; set; }

    /// <summary>
    /// Gets the related Excel row number if any.
    /// A valule of 0 indicates there is no related Excel row.
    /// </summary>
    public int ExcelRow
    {
      get
      {
        return ExcelRange != null ? ExcelRange.Row : 0;
      }
    }

    /// <summary>
    /// Gets a list of <see cref="Excel.Range"/> objects representing cells with modified values.
    /// </summary>
    public List<Excel.Range> ExcelModifiedRangesList { get; private set; }

    /// <summary>
    /// Gets a value indicating whether there are concurrency warnings in a row.
    /// </summary>
    public bool HasConcurrencyWarnings
    {
      get
      {
        return !string.IsNullOrEmpty(RowError) && string.Equals(RowError, MySqlStatement.NO_MATCH, StringComparison.InvariantCulture);
      }
    }

    /// <summary>
    /// Gets a value indicating whether there are errors in a row.
    /// </summary>
    public new bool HasErrors
    {
      get
      {
        return !string.IsNullOrEmpty(RowError) && !string.Equals(RowError, MySqlStatement.NO_MATCH, StringComparison.InvariantCulture);
      }
    }

    /// <summary>
    /// Gets a value indicating whether the row is being deleted.
    /// </summary>
    public bool IsBeingDeleted { get; private set; }

    /// <summary>
    /// Gets or sets a value indicating whether the row represents the row containing column names.
    /// </summary>
    public bool IsHeadersRow { get; set; }

    /// <summary>
    /// Gets the parent <see cref="MySqlDataTable"/> for this row.
    /// </summary>
    public MySqlDataTable MySqlTable
    {
      get
      {
        return _mySqlTable ?? (_mySqlTable = Table as MySqlDataTable);
      }
    }

    /// <summary>
    /// Gets the <see cref="MySqlStatement"/> object containing a SQL query to push changes to the database.
    /// </summary>
    public MySqlStatement Statement { get; private set; }

    #endregion Properties

    /// <summary>
    /// Returns a SQL query meant to push changes in this row to the database server.
    /// </summary>
    /// <returns>A SQL query containing the data changes.</returns>
    public string GetSql()
    {
      if (_sqlQuery != null)
      {
        return _sqlQuery;
      }

      if (RowState == DataRowState.Unchanged)
      {
        _sqlQuery = string.Empty;
        return _sqlQuery;
      }

      if (MySqlTable == null)
      {
        MySqlSourceTrace.WriteToLog(Resources.MySqlDataTableExpectedError, SourceLevels.Critical);
        _sqlQuery = null;
        return _sqlQuery;
      }

      _sqlQuery = string.Empty;
      switch (RowState)
      {
        case DataRowState.Added:
          _sqlQuery = GetSqlForAddedRow();
          MySqlTable.SqlBuilderForInsert.Clear();
          break;

        case DataRowState.Deleted:
          _sqlQuery = GetSqlForDeletedRow();
          MySqlTable.SqlBuilderForDelete.Clear();
          break;

        case DataRowState.Modified:
          _sqlQuery = GetSqlForModifiedRow();
          MySqlTable.SqlBuilderForUpdate.Clear();
          break;

        case DataRowState.Unchanged:
          _sqlQuery = string.Empty;
          break;
      }

      // Verify we have not exceeded the maximum packet size allowed by the server, otherwise throw an Exception.
      if (MySqlTable.MySqlMaxAllowedPacket <= 0)
      {
        return _sqlQuery;
      }

      ulong queryStringByteCount = (ulong)Encoding.ASCII.GetByteCount(_sqlQuery);
      if (queryStringByteCount > MySqlTable.MySqlMaxAllowedPacket)
      {
        throw new QueryExceedsMaxAllowedPacketException();
      }

      return _sqlQuery;
    }

    /// <summary>
    /// Reflects the error set to the row on its corresponding Excel range cells.
    /// </summary>
    public void ReflectError()
    {
      if (IsBeingDeleted || ExcelRange == null)
      {
        return;
      }

      var cellsColor = HasConcurrencyWarnings ? ExcelUtilities.WarningCellsOleColor : ExcelUtilities.ErroredCellsOleColor;
      ExcelModifiedRangesList.SetInteriorColor(cellsColor);
    }

    /// <summary>
    /// Signals that the row has been modified and takes actions on its related Excel cells accordingly.
    /// </summary>
    /// <param name="rowAction">An action performed on this row.</param>
    public void RowChanged(DataRowAction rowAction)
    {
      _sqlQuery = null;
      switch (rowAction)
      {
        case DataRowAction.Add:
          SetupTablePropertyListener(true);
          ReflectChangesForAddedRow();
          break;

        case DataRowAction.Change:
          SetupTablePropertyListener(true);
          ReflectChangesForModifiedRow();
          break;

        case DataRowAction.Commit:
          SetupTablePropertyListener(false);
          ReflectChangesForCommittedRow();
          break;

        case DataRowAction.Delete:
          SetupTablePropertyListener(true);
          ExcelRange = null;
          IsBeingDeleted = true;
          break;

        case DataRowAction.Rollback:
          SetupTablePropertyListener(false);
          ReflectChangesForRolledbackRow();
          break;
      }
    }

    /// <summary>
    /// Creates an INSERT statement SQL query for a row being added.
    /// </summary>
    /// <returns>The INSERT SQL query.</returns>
    private string GetSqlForAddedRow()
    {
      if (MySqlTable == null || RowState != DataRowState.Added || MySqlTable.ColumnsForInsertion == null)
      {
        return string.Empty;
      }

      var sqlBuilderForInsert = MySqlTable.SqlBuilderForInsert;
      sqlBuilderForInsert.Clear();
      sqlBuilderForInsert.Append(MySqlTable.PreSqlForAddedRows);
      string colsSeparator = string.Empty;
      foreach (MySqlDataColumn column in MySqlTable.ColumnsForInsertion)
      {
        bool insertingValueIsNull;
        string valueToDb = DataTypeUtilities.GetStringValueForColumn(this[column.ColumnName], column, true, out insertingValueIsNull);
        sqlBuilderForInsert.AppendFormat(
          "{0}{1}{2}{1}",
          colsSeparator,
          column.ColumnRequiresQuotes && !insertingValueIsNull ? "'" : string.Empty,
          valueToDb);
        colsSeparator = ",";
      }

      sqlBuilderForInsert.Append(')');
      return sqlBuilderForInsert.ToString();
    }

    /// <summary>
    /// Creates a DELETE statement SQL query for a row being deleted.
    /// </summary>
    /// <returns>The DELETE SQL query.</returns>
    private string GetSqlForDeletedRow()
    {
      if (MySqlTable == null || RowState != DataRowState.Deleted || MySqlTable.PrimaryKeyColumns == null)
      {
        return string.Empty;
      }

      var sqlBuilderForDelete = MySqlTable.SqlBuilderForDelete;
      sqlBuilderForDelete.Clear();
      string colsSeparator = string.Empty;
      sqlBuilderForDelete.Append(MySqlStatement.STATEMENT_DELETE);
      sqlBuilderForDelete.AppendFormat(" `{0}`.`{1}` WHERE ", MySqlTable.SchemaName, MySqlTable.TableNameForSqlQueries);
      foreach (MySqlDataColumn pkCol in MySqlTable.PrimaryKeyColumns)
      {
        bool pkValueIsNull;
        string valueToDb = DataTypeUtilities.GetStringValueForColumn(this[pkCol.ColumnName, DataRowVersion.Original], pkCol, false, out pkValueIsNull);
        sqlBuilderForDelete.AppendFormat(
          "{0}`{1}`={2}{3}{2}",
          colsSeparator,
          pkCol.ColumnNameForSqlQueries,
          pkCol.ColumnRequiresQuotes && !pkValueIsNull ? "'" : string.Empty,
          valueToDb);
        colsSeparator = " AND ";
      }

      return sqlBuilderForDelete.ToString();
    }

    /// <summary>
    /// Creates an UPDATE statement SQL query for a row being modified.
    /// </summary>
    /// <returns>The UPDATE SQL query.</returns>
    private string GetSqlForModifiedRow()
    {
      if (MySqlTable == null || RowState != DataRowState.Modified)
      {
        return string.Empty;
      }

      // Reuse the builder we use for INSERT queries now for where clauses, instead of using a new one in order to save memory.
      var wClauseBuilder = MySqlTable.SqlBuilderForInsert;
      var sqlBuilderForUpdate = MySqlTable.SqlBuilderForUpdate;
      wClauseBuilder.Clear();
      sqlBuilderForUpdate.Clear();
      string colsSeparator = string.Empty;
      wClauseBuilder.Append(" WHERE ");
      string wClauseColsSeparator = string.Empty;
      sqlBuilderForUpdate.Append(MySqlStatement.STATEMENT_UPDATE);
      sqlBuilderForUpdate.AppendFormat(" `{0}`.`{1}` SET ", MySqlTable.SchemaName, MySqlTable.TableNameForSqlQueries);
      bool useOptimisticUpdates = MySqlTable.UseOptimisticUpdate;
      foreach (MySqlDataColumn column in MySqlTable.Columns)
      {
        bool updatingValueIsNull;
        string valueToDb;
        if (column.PrimaryKey || useOptimisticUpdates)
        {
          valueToDb = DataTypeUtilities.GetStringValueForColumn(this[column.ColumnName, DataRowVersion.Original], column, false, out updatingValueIsNull);
          if (useOptimisticUpdates && column.AllowNull)
          {
            wClauseBuilder.AppendFormat(
              "{0}(({2}{3}{2} IS NULL AND `{1}` IS NULL) OR `{1}`={2}{3}{2})",
              wClauseColsSeparator,
              column.ColumnNameForSqlQueries,
              column.ColumnRequiresQuotes && !updatingValueIsNull ? "'" : string.Empty,
              valueToDb);
            wClauseColsSeparator = " AND ";
          }
          else
          {
            wClauseBuilder.AppendFormat(
              "{0}`{1}`={2}{3}{2}",
              wClauseColsSeparator,
              column.ColumnNameForSqlQueries,
              column.ColumnRequiresQuotes && !updatingValueIsNull ? "'" : string.Empty,
              valueToDb);
            wClauseColsSeparator = " AND ";
          }
        }

        if (!ChangedColumnNames.Contains(column.ColumnName))
        {
          continue;
        }

        valueToDb = DataTypeUtilities.GetStringValueForColumn(this[column.ColumnName], column, true, out updatingValueIsNull);
        sqlBuilderForUpdate.AppendFormat(
          "{0}`{1}`={2}{3}{2}",
          colsSeparator,
          column.ColumnNameForSqlQueries,
          column.ColumnRequiresQuotes && !updatingValueIsNull ? "'" : string.Empty,
          valueToDb);
        colsSeparator = ",";
      }

      sqlBuilderForUpdate.Append(wClauseBuilder);
      wClauseBuilder.Clear();
      return sqlBuilderForUpdate.ToString();
    }

    /// <summary>
    /// Event delegate method fired when a property value in the parent <see cref="MySqlTable"/> changes.
    /// </summary>
    /// <param name="sender">Sender object.</param>
    /// <param name="e">Event arguments.</param>
    private void MySqlTablePropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
    {
      switch (e.PropertyName)
      {
        case "UseOptimisticUpdate":
        case "ColumnExcluded":
          _sqlQuery = null;
          break;
      }
    }

    /// <summary>
    /// Reflects changes in Excel worksheet if this row has just been added to a <see cref="MySqlDataTable"/>.
    /// </summary>
    private void ReflectChangesForAddedRow()
    {
      if (ExcelRange == null)
      {
        return;
      }

      ExcelRange.SetInteriorColor(ExcelUtilities.UncommittedCellsOleColor);
      ExcelModifiedRangesList.Add(ExcelRange);
    }

    /// <summary>
    /// Reflects changes in Excel worksheet if this row has just been commited.
    /// </summary>
    private void ReflectChangesForCommittedRow()
    {
      if (!IsBeingDeleted && ExcelRange != null)
      {
        ExcelModifiedRangesList.SetInteriorColor(ExcelUtilities.CommitedCellsOleColor);
        if (!HasErrors)
        {
          ExcelModifiedRangesList.Clear();
        }
      }

      if (!HasErrors)
      {
        ChangedColumnNames.Clear();
      }
    }

    /// <summary>
    /// Reflects changes in Excel worksheet if this row has just been modified.
    /// </summary>
    private void ReflectChangesForModifiedRow()
    {
      if (RowState != DataRowState.Modified)
      {
        return;
      }

      if (ExcelRange != null)
      {
        ExcelModifiedRangesList.Clear();
      }

      ChangedColumnNames.Clear();

      // Check column by column for data changes, set related Excel cells color accordingly.
      for (int colIndex = 0; colIndex < Table.Columns.Count; colIndex++)
      {
        Excel.Range columnCell = ExcelRange != null ? ExcelRange.Cells[1, colIndex + 1] : null;
        bool originalAndModifiedIdentical = this[colIndex].Equals(this[colIndex, DataRowVersion.Original]);
        if (!originalAndModifiedIdentical)
        {
          if (columnCell != null)
          {
            ExcelModifiedRangesList.Add(columnCell);
          }

          ChangedColumnNames.Add(Table.Columns[colIndex].ColumnName);
        }

        if (columnCell != null)
        {
          var cellColor = originalAndModifiedIdentical ? ExcelUtilities.EMPTY_CELLS_OLE_COLOR : ExcelUtilities.UncommittedCellsOleColor;
          columnCell.SetInteriorColor(cellColor);
        }
      }

      // If the row resulted with no modifications (maybe some values set back to their original values by the user) then undo changes.
      if (ChangedColumnNames.Count == 0)
      {
        RejectChanges();
      }
    }

    /// <summary>
    /// Reflects changes in Excel worksheet if this row has just been rolled back.
    /// </summary>
    private void ReflectChangesForRolledbackRow()
    {
      if (!IsBeingDeleted)
      {
        ExcelRange.SetInteriorColor(ExcelUtilities.EMPTY_CELLS_OLE_COLOR);
        ExcelModifiedRangesList.Clear();
      }

      ChangedColumnNames.Clear();
      IsBeingDeleted = false;
    }

    /// <summary>
    /// Subscribes on unsubscribes to the table's property changed event.
    /// </summary>
    /// <param name="subscribe">Flag indicating whether the event is subscribed or unsubscribed.</param>
    private void SetupTablePropertyListener(bool subscribe)
    {
      if (MySqlTable == null)
      {
        return;
      }

      MySqlTable.PropertyChanged -= MySqlTablePropertyChanged;
      if (subscribe)
      {
        MySqlTable.PropertyChanged += MySqlTablePropertyChanged;
      }
    }
  }
}
