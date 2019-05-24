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
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using MySql.Utility.Classes.Logging;
using MySQL.ForExcel.Classes.Exceptions;
using MySQL.ForExcel.Interfaces;
using MySQL.ForExcel.Properties;
using ExcelInterop = Microsoft.Office.Interop.Excel;

namespace MySQL.ForExcel.Classes
{
  /// <summary>
  /// Represents a table row holding MySQL data mapped to Excel cells.
  /// </summary>
  public class MySqlDataRow : DataRow, IMySqlDataRow
  {
    #region Fields

    /// <summary>
    /// The Excel range representing the whole data row.
    /// </summary>
    private ExcelInterop.Range _excelRange;

    /// <summary>
    /// An array containing the previous colors assigned to cells before the last refresh from the database.
    /// </summary>
    private int[] _excelRangePreviousColors;

    /// <summary>
    /// Gets the parent <see cref="MySqlDataTable"/> for this row.
    /// </summary>
    private MySqlDataTable _mySqlTable;

    /// <summary>
    /// Flag indicating whether the row is refreshing its data after a push operation is made.
    /// </summary>
    private bool _refreshingData;

    /// <summary>
    /// An optional SET statement to initialize variables used in the returned SQL query.
    /// </summary>
    private string _setVariablesSql;

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
      _excelRange = null;
      _mySqlTable = null;
      _refreshingData = false;
      _setVariablesSql = null;
      _sqlQuery = null;
      ChangedColumnNames = new List<string>(Table.Columns.Count);
      IsBeingDeleted = false;
      IsHeadersRow = false;
      ExcelModifiedRangesList = new List<ExcelInterop.Range>(Table.Columns.Count);
      Statement = new MySqlStatement(this);
    }

    #region Properties

    /// <summary>
    /// Gets a list of column names with data changes.
    /// </summary>
    public List<string> ChangedColumnNames { get; }

    /// <summary>
    /// Gets or sets the Excel range representing the whole data row.
    /// </summary>
    public ExcelInterop.Range ExcelRange
    {
      get => _excelRange;

      set
      {
        _excelRange = value;
        if (_excelRange != null)
        {
          _excelRangePreviousColors = new int[_excelRange.Columns.Count];
        }
      }
    }

    /// <summary>
    /// Gets the related Excel row number if any.
    /// A value of 0 indicates there is no related Excel row.
    /// </summary>
    public int ExcelRow => ExcelRange?.Row ?? 0;

    /// <summary>
    /// Gets a list of <see cref="ExcelInterop.Range"/> objects representing cells with modified values.
    /// </summary>
    public List<ExcelInterop.Range> ExcelModifiedRangesList { get; }

    /// <summary>
    /// Gets a value indicating whether there are concurrency warnings in a row.
    /// </summary>
    public bool HasConcurrencyWarnings => !string.IsNullOrEmpty(RowError) && string.Equals(RowError, MySqlStatement.NO_MATCH, StringComparison.InvariantCulture);

    /// <summary>
    /// Gets a value indicating whether there are errors in a row.
    /// </summary>
    public new bool HasErrors => !string.IsNullOrEmpty(RowError) && !string.Equals(RowError, MySqlStatement.NO_MATCH, StringComparison.InvariantCulture);

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
    public MySqlDataTable MySqlTable => _mySqlTable ?? (_mySqlTable = Table as MySqlDataTable);

    /// <summary>
    /// Gets the <see cref="MySqlStatement"/> object containing a SQL query to push changes to the database.
    /// </summary>
    public MySqlStatement Statement { get; }

    #endregion Properties

    /// <summary>
    /// Returns a SQL query meant to push changes in this row to the database server.
    /// </summary>
    /// <param name="setVariablesSql">An optional SET statement to initialize variables used in the returned SQL query.</param>
    /// <returns>A SQL query containing the data changes.</returns>
    public string GetSql(out string setVariablesSql)
    {
      setVariablesSql = _setVariablesSql;
      if (_sqlQuery != null)
      {
        return _sqlQuery;
      }

      _setVariablesSql = null;
      if (RowState == DataRowState.Unchanged)
      {
        _sqlQuery = string.Empty;
        return _sqlQuery;
      }

      if (MySqlTable == null)
      {
        Logger.LogError(Resources.MySqlDataTableExpectedError);
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
          _sqlQuery = MySqlTable.UseOptimisticUpdate
            ? GetSqlForModifiedRowUsingOptimisticConcurrency()
            : GetSqlForModifiedRow();
          setVariablesSql = _setVariablesSql;
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

      var queryStringByteCount = Encoding.ASCII.GetByteCount(_sqlQuery);
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
      SaveCurrentColor(cellsColor);
    }

    /// <summary>
    /// Refreshes the row's data and reflects the changes on the <see cref="ExcelRow"/>.
    /// </summary>
    /// <param name="acceptChanges">Flag indicating whether the refreshed data is committed immediately to the row.</param>
    public void RefreshData(bool acceptChanges)
    {
      if (MySqlTable == null)
      {
        return;
      }

      if ((RowState != DataRowState.Added && RowState != DataRowState.Modified))
      {
        return;
      }

      var refreshSuccessful = true;
      try
      {
        var refreshQuery = GetSqlForRefreshingRow();
        if (string.IsNullOrEmpty(refreshQuery))
        {
          return;
        }

        var refreshTable = MySqlTable.WbConnection.GetDataFromSelectQuery(refreshQuery);
        if (refreshTable == null || refreshTable.Rows.Count == 0)
        {
          return;
        }

        var refreshedRow = refreshTable.Rows[0];
        var rowValues = refreshedRow.ItemArray;
        MySqlTable.PrepareCopyingItemArray(ref rowValues, MySqlTable.EscapeFormulaTexts);
        _refreshingData = true;
        ItemArray = rowValues;

        if (ExcelRange == null)
        {
          return;
        }

        Globals.ThisAddIn.SkipWorksheetChangeEvent = true;
        for (var columnIndex = 1; columnIndex <= ExcelRange.Columns.Count; columnIndex++)
        {
          ExcelRange.Cells[1, columnIndex] = rowValues[columnIndex - 1];
        }
      }
      catch (Exception ex)
      {
        Logger.LogException(ex);
        refreshSuccessful = false;
      }
      finally
      {
        _refreshingData = false;
        Globals.ThisAddIn.SkipWorksheetChangeEvent = false;
      }

      if (acceptChanges && refreshSuccessful)
      {
        AcceptChanges();
      }
    }

    /// <summary>
    /// Signals that the row has been just added to a <see cref="MySqlDataTable"/>.
    /// </summary>
    public void RowAdded()
    {
      if (_excelRangePreviousColors == null)
      {
        if (_excelRange != null)
        {
          _excelRangePreviousColors = new int[_excelRange.Columns.Count];
        }
      }
      else
      {
        _excelRangePreviousColors.Initialize();
      }
    }

    /// <summary>
    /// Signals that the row has been modified and takes actions on its related Excel cells accordingly.
    /// </summary>
    /// <param name="rowAction">An action performed on this row.</param>
    public void RowChanged(DataRowAction rowAction)
    {
      if (_refreshingData)
      {
        return;
      }

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
      var parentTable = MySqlTable;
      if (parentTable == null || RowState != DataRowState.Added || parentTable.ColumnsForInsertion == null)
      {
        return string.Empty;
      }

      var sqlBuilderForInsert = parentTable.SqlBuilderForInsert;
      sqlBuilderForInsert.Clear();
      sqlBuilderForInsert.Append(parentTable.PreSqlForAddedRows);
      sqlBuilderForInsert.Append('(');
      var colsSeparator = string.Empty;
      foreach (var column in parentTable.ColumnsForInsertion)
      {
        var valueToDb = column.GetStringValue(this[column.ColumnName], out var insertingValueIsNull);
        var wrapValueCharacter = column.MySqlDataType.RequiresQuotesForValue && !insertingValueIsNull ? "'" : string.Empty;
        sqlBuilderForInsert.AppendFormat("{0}{1}{2}{1}", colsSeparator, wrapValueCharacter, valueToDb);
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
      var parentTable = MySqlTable;
      if (parentTable == null || RowState != DataRowState.Deleted || parentTable.PrimaryKeyColumns == null)
      {
        return string.Empty;
      }

      var sqlBuilderForDelete = parentTable.SqlBuilderForDelete;
      sqlBuilderForDelete.Clear();
      sqlBuilderForDelete.Append(MySqlStatement.STATEMENT_DELETE);
      sqlBuilderForDelete.AppendFormat(" `{0}`.`{1}`", parentTable.SchemaName, parentTable.TableNameForSqlQueries);
      sqlBuilderForDelete.Append(GetWhereClauseFromPrimaryKey(true));

      return sqlBuilderForDelete.ToString();
    }

    /// <summary>
    /// Creates an UPDATE statement SQL text for a row being modified.
    /// </summary>
    /// <returns>The UPDATE SQL query.</returns>
    private string GetSqlForModifiedRow()
    {
      var parentTable = MySqlTable;
      if (parentTable == null || RowState != DataRowState.Modified)
      {
        return string.Empty;
      }

      var sqlBuilderForUpdate = parentTable.SqlBuilderForUpdate;
      sqlBuilderForUpdate.Clear();
      var colsSeparator = string.Empty;
      sqlBuilderForUpdate.Append(MySqlStatement.STATEMENT_UPDATE);
      sqlBuilderForUpdate.AppendFormat(" `{0}`.`{1}` SET ", parentTable.SchemaName, parentTable.TableNameForSqlQueries);
      foreach (var column in parentTable.Columns.Cast<MySqlDataColumn>().Where(col => ChangedColumnNames.Contains(col.ColumnName)))
      {
        var valueToDb = column.GetStringValue(this[column.ColumnName], out var updatingValueIsNull);
        var wrapValueCharacter = column.MySqlDataType.RequiresQuotesForValue && !updatingValueIsNull ? "'" : string.Empty;
        sqlBuilderForUpdate.AppendFormat("{0}`{1}`={2}{3}{2}", colsSeparator, column.ColumnNameForSqlQueries, wrapValueCharacter, valueToDb);
        colsSeparator = ",";
      }

      sqlBuilderForUpdate.Append(GetWhereClauseFromPrimaryKey(true));
      return sqlBuilderForUpdate.ToString();
    }

    /// <summary>
    /// Creates sET and UPDATE statements SQL text for a row being modified where an optimistic concurrency model is used.
    /// </summary>
    /// <returns>The UPDATE SQL query.</returns>
    private string GetSqlForModifiedRowUsingOptimisticConcurrency()
    {
      var parentTable = MySqlTable;
      if (parentTable == null || RowState != DataRowState.Modified)
      {
        return string.Empty;
      }

      // Reuse builders instead of using new ones in order to save memory.
      var setVariablesBuilder = parentTable.SqlBuilderForDelete;
      var wClauseBuilder = parentTable.SqlBuilderForInsert;
      var sqlBuilderForUpdate = parentTable.SqlBuilderForUpdate;
      setVariablesBuilder.Clear();
      wClauseBuilder.Clear();
      sqlBuilderForUpdate.Clear();

      var serverCollation = parentTable.WbConnection.ServerCollation;
      var setSeparator = "SET";
      var colsSeparator = string.Empty;
      var wClauseColsSeparator = string.Empty;
      var epsilonTolerance = Settings.Default.GlobalEditToleranceForFloatAndDouble;
      wClauseBuilder.Append(" WHERE ");
      sqlBuilderForUpdate.Append(MySqlStatement.STATEMENT_UPDATE);
      sqlBuilderForUpdate.AppendFormat(" `{0}`.`{1}` SET ", parentTable.SchemaName, parentTable.TableNameForSqlQueries);
      foreach (MySqlDataColumn column in parentTable.Columns)
      {
        var columnRequiresQuotes = column.MySqlDataType.RequiresQuotesForValue;
        var columnIsText = column.MySqlDataType.IsChar || column.MySqlDataType.IsText || column.MySqlDataType.IsSetOrEnum;
        var columnIsJson = column.MySqlDataType.IsJson;
        var columnIsFloatOrDouble = column.ServerDataType.IsFloatingPoint;
        var valueToDb = column.GetStringValue(this[column.ColumnName, DataRowVersion.Original], out var updatingValueIsNull);
        var wrapValueCharacter = columnRequiresQuotes && !updatingValueIsNull ? "'" : string.Empty;
        var valueForClause = string.Format("{0}{1}{0}", wrapValueCharacter, valueToDb);
        if (column.AllowNull)
        {
          var columnCollation = column.AbsoluteCollation;
          var needToCollateTextValue = columnIsText && !updatingValueIsNull
                                        && serverCollation != null
                                        && !serverCollation.Equals(columnCollation, StringComparison.InvariantCultureIgnoreCase)
                                        && columnCollation.IsUnicodeCharSetOrCollation();

          // If the length of the string value * 2 is greater than the string it requires to declare a variable for it, then declare the variable to save query space.
          var needToCreateVariable = valueToDb.Length * 2 > valueToDb.Length + 24 + (needToCollateTextValue ? columnCollation.Length + 9 : 0);
          if (needToCreateVariable)
          {
            var sqlVariableName = $"@OldCol{column.Ordinal + 1}Value";
            setVariablesBuilder.AppendFormat("{0} {1} = {2}", setSeparator, sqlVariableName, valueForClause);

            // Assigning the variable name to valueForClause so it's used in the next section instead of the actual value when assembling the WHERE clause.
            valueForClause = sqlVariableName;
            setSeparator = ",";
            if (needToCollateTextValue)
            {
              setVariablesBuilder.Append(" COLLATE ");
              setVariablesBuilder.Append(columnCollation);
            }
          }

          // At this point valueForClause contains the value already wrapped in single quotes if needed.
          wClauseBuilder.AppendFormat("{0}(({2} IS NULL AND `{1}` IS NULL)", wClauseColsSeparator, column.ColumnNameForSqlQueries, valueForClause);
          wClauseColsSeparator = " OR ";
        }

        wClauseBuilder.AppendFormat(columnIsJson && !updatingValueIsNull
            ? "{0}`{1}`=CAST({2} AS JSON){3}"
            : columnIsFloatOrDouble
              ? "{0}`{1}` BETWEEN {2}-{4} AND {2}+{4}{3}"
              : "{0}`{1}`={2}{3}"
          , wClauseColsSeparator, column.ColumnNameForSqlQueries, valueForClause, column.AllowNull ? ")" : string.Empty, epsilonTolerance);
        wClauseColsSeparator = " AND ";
        if (!ChangedColumnNames.Contains(column.ColumnName))
        {
          continue;
        }

        valueToDb = column.GetStringValue(this[column.ColumnName], out updatingValueIsNull);
        wrapValueCharacter = columnRequiresQuotes && !updatingValueIsNull ? "'" : string.Empty;
        sqlBuilderForUpdate.AppendFormat("{0}`{1}`={2}{3}{2}", colsSeparator, column.ColumnNameForSqlQueries, wrapValueCharacter, valueToDb);
        colsSeparator = ",";
      }

      if (setVariablesBuilder.Length > 0)
      {
        _setVariablesSql = setVariablesBuilder.ToString();
      }

      sqlBuilderForUpdate.Append(wClauseBuilder);
      return sqlBuilderForUpdate.ToString();
    }

    /// <summary>
    /// Returns a SELECT statement SQL query to refresh the row contents.
    /// </summary>
    /// <returns>The SELECT SQL query.</returns>
    private string GetSqlForRefreshingRow()
    {
      var parentTable = MySqlTable;
      if (parentTable == null || string.IsNullOrEmpty(parentTable.SelectQuery))
      {
        return string.Empty;
      }

      return parentTable.SelectQuery + GetWhereClauseFromPrimaryKey(false);
    }

    /// <summary>
    /// Creates the WHERE clause part of a SQL statement based on the primary key columns.
    /// </summary>
    /// <param name="useOriginalData">Flag indicating whether the version of the data to use to extract primary key values is <see cref="DataRowVersion.Original"/>, otherwise <see cref="DataRowVersion.Current"/> is used.</param>
    /// <returns>The WHERE clause part of a SQL statement.</returns>
    private string GetWhereClauseFromPrimaryKey(bool useOriginalData)
    {
      if (useOriginalData && !HasVersion(DataRowVersion.Original))
      {
        throw new VersionNotFoundException(Resources.OriginalRowVersionNotFoundErrorText);
      }

      var parentTable = MySqlTable;
      if (parentTable?.PrimaryKeyColumns == null)
      {
        return string.Empty;
      }

      // Reuse the builder we use for INSERT queries now for where clauses, instead of using a new one in order to save memory.
      var wClauseBuilder = parentTable.SqlBuilderForInsert;
      wClauseBuilder.Clear();
      var colsSeparator = string.Empty;
      wClauseBuilder.Append(" WHERE ");
      var dataRowVersion = useOriginalData ? DataRowVersion.Original : DataRowVersion.Current;
      foreach (var pkCol in parentTable.PrimaryKeyColumns)
      {
        var valueToDb = pkCol.GetStringValue(this[pkCol.ColumnName, dataRowVersion], out var pkValueIsNull);
        var wrapValueCharacter = pkCol.MySqlDataType.RequiresQuotesForValue && !pkValueIsNull ? "'" : string.Empty;
        wClauseBuilder.AppendFormat("{0}`{1}`={2}{3}{2}", colsSeparator, pkCol.ColumnNameForSqlQueries, wrapValueCharacter, valueToDb);
        colsSeparator = " AND ";
      }

      return wClauseBuilder.ToString();
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
    /// Reflects changes in Excel worksheet if this row has just been committed.
    /// </summary>
    private void ReflectChangesForCommittedRow()
    {
      if (!IsBeingDeleted && ExcelRange != null)
      {
        ExcelModifiedRangesList.SetInteriorColor(ExcelUtilities.CommittedCellsOleColor);
        SaveCurrentColor(ExcelUtilities.CommittedCellsOleColor);
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
      if (RowState == DataRowState.Added)
      {
        // A recently added row's value is being modified, we just need to re-paint the whole "added" row.
        ExcelRange?.SetInteriorColor(ExcelUtilities.UncommittedCellsOleColor);
      }

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
      for (var colIndex = 0; colIndex < Table.Columns.Count; colIndex++)
      {
        ExcelInterop.Range columnCell = ExcelRange?.Cells[1, colIndex + 1];
        var originalAndModifiedIdentical = this[colIndex].Equals(this[colIndex, DataRowVersion.Original]);
        if (!originalAndModifiedIdentical)
        {
          if (columnCell != null)
          {
            ExcelModifiedRangesList.Add(columnCell);
          }

          ChangedColumnNames.Add(Table.Columns[colIndex].ColumnName);
        }

        if (columnCell == null)
        {
          continue;
        }

        var cellColor = originalAndModifiedIdentical ? _excelRangePreviousColors[colIndex] : ExcelUtilities.UncommittedCellsOleColor;
        columnCell.SetInteriorColor(cellColor);
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
        for (var colIndex = 0; colIndex < Table.Columns.Count; colIndex++)
        {
          ExcelInterop.Range columnCell = ExcelRange?.Cells[1, colIndex + 1];
          columnCell?.SetInteriorColor(_excelRangePreviousColors[colIndex]);
        }

        ExcelModifiedRangesList.Clear();
      }

      ChangedColumnNames.Clear();
      IsBeingDeleted = false;
    }

    /// <summary>
    /// Saves the given color in an array for the modified Excel cells related to the current row.
    /// </summary>
    /// <param name="oleColor">The new interior color for the Excel cells.</param>
    private void SaveCurrentColor(int oleColor)
    {
      if (_excelRangePreviousColors == null)
      {
        return;
      }

      foreach (var colIndex in ExcelModifiedRangesList.Select(modifiedRange => modifiedRange.Column).Where(colIndex => colIndex <= _excelRangePreviousColors.Length))
      {
        _excelRangePreviousColors[colIndex - 1] = oleColor;
      }
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
