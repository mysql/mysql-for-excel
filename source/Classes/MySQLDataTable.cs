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

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Text;
using MySql.Data.MySqlClient;
using MySQL.ForExcel.Forms;
using MySQL.Utility.Classes;
using MySQL.Utility.Classes.MySQLWorkbench;
using Excel = Microsoft.Office.Interop.Excel;

namespace MySQL.ForExcel.Classes
{
  /// <summary>
  /// Represents an in-memory table for a corresponding MySQL database table.
  /// </summary>
  public class MySqlDataTable : DataTable
  {
    /// <summary>
    /// String to identify the error code in a <see cref="DataRow"/> as to be related to a row with the given primary key not being found in the MySQL table.
    /// </summary>
    public const string NO_MATCH = "NO_MATCH";

    /// <summary>
    /// Bytes to subtract from the maximum allowed packet size to build a query that is safely processed by the database server.
    /// </summary>
    private const int SAFE_BYTES_BEFORE_REACHING_MAX_ALLOWED_PACKET = 10;

    #region Fields

    /// <summary>
    /// List of text strings containing warnings for users about the auto-generated primary key.
    /// </summary>
    private readonly List<string> _autoPkWarningTextsList;

    /// <summary>
    /// Flag indicating if the first row in the Excel region to be exported contains the column names of the MySQL table that will be created.
    /// </summary>
    private bool _firstRowIsHeaders;

    /// <summary>
    /// Flag indicating if the column names where changed to use the first row of data.
    /// </summary>
    private bool _changedColumnNamesWithFirstRowOfData;

    /// <summary>
    /// Contains the value of the max_allowed_packet system variable of the MySQL Server currently connected to.
    /// </summary>
    private ulong _mysqlMaxAllowedPacket;

    /// <summary>
    ///
    /// </summary>
    private bool? _tableExistsInSchema;

    /// <summary>
    /// List of text strings containing warnings for users about the table properties that could cause errors when creating this table in the database.
    /// </summary>
    private readonly List<string> _tableWarningsTextList;

    /// <summary>
    /// Flag indicating if the first column in the Excel region to be exported will be used to create the MySQL table's primary key.
    /// </summary>
    private bool _useFirstColumnAsPk;

    #endregion Fields

    /// <summary>
    /// Initializes a new instance of the <see cref="MySqlDataTable"/> class.
    /// This constructor is meant to be used by the <see cref="ExportDataForm"/> class.
    /// </summary>
    /// <param name="schemaName">Name of the schema where this table will be created.</param>
    /// <param name="proposedTableName">Proposed name for this new table.</param>
    /// <param name="addPrimaryKeyCol">Flag indicating if an auto-generated primary key column will be added as the first column in the table.</param>
    /// <param name="useFormattedValues">Flag indicating if the Excel excelData used to populate this table is formatted (numbers, dates, text) or not (numbers and text).</param>
    /// <param name="removeEmptyColumns">Flag indicating if columns with no excelData will be skipped for export to a new table so they are not created.</param>
    /// <param name="detectDataType">Flag indicating if the data type for each column is automatically detected when data is loaded by the <see cref="SetData"/> method.</param>
    /// <param name="addBufferToVarchar">Flag indicating if columns with an auto-detected varchar type will get a padding buffer for its size.</param>
    /// <param name="autoIndexIntColumns">Flag indicating if columns with an integer-based data-type will have their <see cref="MySqlDataColumn.CreateIndex"/> property value set to true.</param>
    /// <param name="autoAllowEmptyNonIndexColumns">Flag indicating if columns that have their <see cref="MySqlDataColumn.CreateIndex"/> property value
    /// set to <c>false</c> will automatically get their <see cref="MySqlDataColumn.AllowNull"/> property value set to <c>true</c>.</param>
    /// <param name="wbConnection">MySQL Workbench connection to a MySQL server instance selected by users.</param>
    public MySqlDataTable(string schemaName, string proposedTableName, bool addPrimaryKeyCol, bool useFormattedValues, bool removeEmptyColumns, bool detectDataType, bool addBufferToVarchar, bool autoIndexIntColumns, bool autoAllowEmptyNonIndexColumns, MySqlWorkbenchConnection wbConnection)
      : this(schemaName, proposedTableName)
    {
      AddBufferToVarchar = addBufferToVarchar;
      AddPrimaryKeyColumn = addPrimaryKeyCol;
      AutoAllowEmptyNonIndexColumns = autoAllowEmptyNonIndexColumns;
      AutoIndexIntColumns = autoIndexIntColumns;
      DetectDatatype = detectDataType;
      InExportMode = true;
      IsFormatted = useFormattedValues;
      RemoveEmptyColumns = removeEmptyColumns;
      TableName = proposedTableName;
      WbConnection = wbConnection;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="MySqlDataTable"/> class.
    /// This constructor is meant to be used by the <see cref="AppendDataForm"/> class to fetch schema information from the corresponding MySQL table before copying its excelData.
    /// </summary>
    /// <param name="tableName">Name of the table.</param>
    /// <param name="fetchColumnsSchemaInfo">Flag indicating if the schema information from the corresponding MySQL table is fetched and recreated before any excelData is added to the table.</param>
    /// <param name="datesAsMySqlDates">Flag indicating if the dates are stored in the table as <see cref="System.DateTime"/> or <see cref="MySql.Data.Types.MySqlDateTime"/> objects.</param>
    /// <param name="wbConnection">MySQL Workbench connection to a MySQL server instance selected by users.</param>
    public MySqlDataTable(string tableName, bool fetchColumnsSchemaInfo, bool datesAsMySqlDates, MySqlWorkbenchConnection wbConnection)
      : this(wbConnection.Schema, tableName)
    {
      WbConnection = wbConnection;
      if (fetchColumnsSchemaInfo)
      {
        CreateTableSchema(tableName, datesAsMySqlDates);
      }

      _mysqlMaxAllowedPacket = WbConnection.GetMySqlServerMaxAllowedPacket();
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="MySqlDataTable"/> class.
    /// This constructor is meant to be used by the <see cref="EditDataDialog"/> class to copy the contents of a table imported to Excel for edition.
    /// </summary>
    /// <param name="tableName">Name of the table.</param>
    /// <param name="filledTable"><see cref="DataTable"/> object containing imported excelData from the MySQL table to be edited.</param>
    /// <param name="wbConnection">MySQL Workbench connection to a MySQL server instance selected by users.</param>
    public MySqlDataTable(string tableName, DataTable filledTable, MySqlWorkbenchConnection wbConnection)
      : this(tableName, true, true, wbConnection)
    {
      CopyTableData(filledTable);
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="MySqlDataTable"/> class.
    /// </summary>
    /// <param name="schemaName">Name of the schema where this table exists.</param>
    /// <param name="tableName">Name of the table.</param>
    public MySqlDataTable(string schemaName, string tableName)
      : this()
    {
      if (!string.IsNullOrEmpty(schemaName))
      {
        SchemaName = schemaName;
      }

      if (tableName != null)
      {
        TableName = tableName;
      }

      SelectQuery = string.Format("SELECT * FROM `{0}`.`{1}`", SchemaName, TableNameForSqlQueries);
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="MySqlDataTable"/> class.
    /// </summary>
    public MySqlDataTable()
    {
      _autoPkWarningTextsList = new List<string>(1);
      _changedColumnNamesWithFirstRowOfData = false;
      _mysqlMaxAllowedPacket = 0;
      _tableExistsInSchema = null;
      _tableWarningsTextList = new List<string>(3);
      AddBufferToVarchar = false;
      AddPrimaryKeyColumn = false;
      AutoAllowEmptyNonIndexColumns = false;
      AutoIndexIntColumns = false;
      DetectDatatype = false;
      FirstRowIsHeaders = false;
      InExportMode = false;
      IsTableNameValid = !string.IsNullOrEmpty(TableName);
      IsFormatted = false;
      RemoveEmptyColumns = false;
      SchemaName = string.Empty;
      SelectQuery = string.Format("SELECT * FROM `{0}`", TableNameForSqlQueries);
      UseFirstColumnAsPk = false;
      WbConnection = null;
    }

    #region Properties

    /// <summary>
    /// Gets a value indicating whether columns with an auto-detected varchar type will get a padding buffer for its size.
    /// </summary>
    public bool AddBufferToVarchar { get; private set; }

    /// <summary>
    /// Gets a value indicating whether an auto-generated primary key column will be added as the first column in the table.
    /// </summary>
    public bool AddPrimaryKeyColumn { get; private set; }

    /// <summary>
    /// Gets a value indicating whether columns that have their <see cref="MySqlDataColumn.CreateIndex"/> property value set to <c>false</c>
    /// will automatically get their <see cref="MySqlDataColumn.AllowNull"/> property value set to <c>true</c>.
    /// </summary>
    public bool AutoAllowEmptyNonIndexColumns { get; private set; }

    /// <summary>
    /// Gets a value indicating whether columns with an integer-based data-type will have their <see cref="MySqlDataColumn.CreateIndex"/>property value set to <param name=">true"></param>.
    /// </summary>
    public bool AutoIndexIntColumns { get; private set; }

    /// <summary>
    /// Gets the name of the auto-generated primary key column.
    /// </summary>
    public string AutoPkName
    {
      get
      {
        return AddPrimaryKeyColumn && Columns.Count > 0 ? GetColumnAtIndex(0).DisplayName : string.Empty;
      }
    }

    /// <summary>
    /// Gets the number of warnings associated to the auto-generated primary key.
    /// </summary>
    public int AutoPkWarningsQuantity
    {
      get
      {
        return _autoPkWarningTextsList != null ? _autoPkWarningTextsList.Count : 0;
      }
    }

    /// <summary>
    /// Gets the last warning text associated to the auto-generated primary key.
    /// </summary>
    public string CurrentAutoPkWarningText
    {
      get
      {
        return _autoPkWarningTextsList.Count > 0 ? _autoPkWarningTextsList.Last() : string.Empty;
      }
    }

    /// <summary>
    /// Gets the last warning text associated to the table.
    /// </summary>
    public string CurrentTableWarningText
    {
      get
      {
        return _tableWarningsTextList.Count > 0 ? _tableWarningsTextList.Last() : string.Empty;
      }
    }

    /// <summary>
    /// Gets the number of deleted rows meaning the number of pending DELETE operations in an Edit Data operation.
    /// </summary>
    public int DeletingOperations
    {
      get
      {
        DataTable changesDt = GetChanges(DataRowState.Deleted);
        return changesDt != null ? changesDt.Rows.Count : 0;
      }
    }

    /// <summary>
    /// Gets a value indicating whether data type for each column is automatically detected when data is loaded by the <see cref="SetData"/> method.
    /// </summary>
    public bool DetectDatatype { get; set; }

    /// <summary>
    /// Gets a value indicating whether the first column contains numeric whole numbers.
    /// </summary>
    public bool FirstColumnContainsIntegers
    {
      get
      {
        bool containsIntegers = false;
        int firstColIdx = AddPrimaryKeyColumn ? 1 : 0;
        if (Columns.Count > firstColIdx)
        {
          containsIntegers = GetColumnAtIndex(firstColIdx).MySqlDataType.ToLowerInvariant() == "integer";
        }

        if (containsIntegers)
        {
          return true;
        }

        int rowsToAnalyzeCount = Math.Min(Rows.Count, 50);
        int startingRow = _firstRowIsHeaders ? 1 : 0;
        containsIntegers = startingRow < rowsToAnalyzeCount;
        for (int rowIdx = startingRow; rowIdx < rowsToAnalyzeCount; rowIdx++)
        {
          int res;
          containsIntegers = containsIntegers && int.TryParse(Rows[rowIdx][1].ToString(), out res);
        }

        return containsIntegers;
      }
    }

    /// <summary>
    /// Gets or sets a value indicating whether the the first row of excelData contains the column names for a new table.
    /// </summary>
    public bool FirstRowIsHeaders
    {
      get
      {
        return _firstRowIsHeaders;
      }

      set
      {
        _firstRowIsHeaders = value;
        UseFirstRowAsHeaders();
      }
    }

    /// <summary>
    /// Gets a value indicating whether the table is being constructed for exporting it to a new MySQL table.
    /// </summary>
    public bool InExportMode { get; private set; }

    /// <summary>
    /// Gets the number of added rows meaning the number of pending INSERT operations in an Edit Data operation.
    /// </summary>
    public int InsertingOperations
    {
      get
      {
        DataTable changesDt = GetChanges(DataRowState.Added);
        return changesDt != null ? changesDt.Rows.Count : 0;
      }
    }

    /// <summary>
    /// Gets a value indicating whether the automatically added column used for the primary key is not a duplicate of another column.
    /// </summary>
    public bool IsAutoPkColumnNameValid
    {
      get
      {
        return AddPrimaryKeyColumn && !GetColumnAtIndex(0).IsDisplayNameDuplicate;
      }
    }

    /// <summary>
    /// Gets a value indicating whether the Excel excelData used to populate this table is formatted (numbers, dates, text) or not (numbers and text).
    /// </summary>
    public bool IsFormatted { get; private set; }

    /// <summary>
    /// Gets a value indicating whether the table name is valid so it would not throw errors when it is created.
    /// </summary>
    public bool IsTableNameValid { get; private set; }

    /// <summary>
    /// Gets the number of columns mapped for append to a target table, meaningful only for an Append Data operation.
    /// </summary>
    public int MappedColumnsQuantity
    {
      get
      {
        int mappedQty = 0;
        for (int colIdx = 0; colIdx < Columns.Count; colIdx++)
        {
          if (!string.IsNullOrEmpty(GetColumnAtIndex(colIdx).MappedDataColName))
          {
            mappedQty++;
          }
        }

        return mappedQty;
      }
    }

    /// <summary>
    /// Gets the number of columns that compose the table's primary key.
    /// </summary>
    public int NumberOfPk
    {
      get
      {
        return Columns.OfType<MySqlDataColumn>().Skip(1).Count(col => col.PrimaryKey && !col.ExcludeColumn);
      }
    }

    /// <summary>
    /// Gets a value indicating whether columns with no excelData will be skipped for export to a new table so they are not created.
    /// </summary>
    public bool RemoveEmptyColumns { get; private set; }

    /// <summary>
    /// Gets the name of the schema where this table exists or will be created.
    /// </summary>
    public string SchemaName { get; private set; }

    /// <summary>
    /// Gets or sets the SELECT query used to retrieve the excelData from the corresponding MySQL table to fill this one.
    /// </summary>
    public string SelectQuery { get; set; }

    /// <summary>
    /// Gets a value indicating whether there is a MySQL table in the connected schema with the same name as in <see cref="TableName"/>.
    /// </summary>
    public bool TableExistsInSchema
    {
      get
      {
        if (_tableExistsInSchema == null)
        {
          string cleanTableName = TableName.ToLowerInvariant().Replace(" ", "_");
          _tableExistsInSchema = WbConnection != null && WbConnection.TableExistsInSchema(WbConnection.Schema, cleanTableName);
        }

        return (bool)_tableExistsInSchema;
      }
    }

    /// <summary>
    /// Gets or sets the name of the <see cref="MySqlDataTable"/>.
    /// </summary>
    public new string TableName
    {
      get
      {
        return base.TableName;
      }

      set
      {
        if (base.TableName != value)
        {
          _tableExistsInSchema = null;
        }

        base.TableName = value;
        if (!InExportMode)
        {
          return;
        }

        ResetAutoPkName();
        UpdateTableSelectQuery();
        UpdateTableNameWarningsAndSelectQuery();
      }
    }

    /// <summary>
    /// Gets the <see cref="TableName"/> escaping the back-tick character.
    /// </summary>
    public string TableNameForSqlQueries
    {
      get
      {
        return TableName.Replace("`", "``");
      }
    }

    /// <summary>
    /// Gets the number of warnings associated to the table.
    /// </summary>
    public int TableWarningsQuantity
    {
      get
      {
        return _tableWarningsTextList != null ? _tableWarningsTextList.Count : 0;
      }
    }

    /// <summary>
    /// Gets the number of changed rows meaning the number of pending UPDATE operations in an Edit Data operation.
    /// </summary>
    public int UpdatingOperations
    {
      get
      {
        DataTable changesDt = GetChanges(DataRowState.Modified);
        return changesDt != null ? changesDt.Rows.Count : 0;
      }
    }

    /// <summary>
    /// Gets or sets a value indicating whether the first column of the table will be or is used in the table's primary key.
    /// </summary>
    public bool UseFirstColumnAsPk
    {
      get
      {
        return _useFirstColumnAsPk;
      }

      set
      {
        _useFirstColumnAsPk = value;
        if (!AddPrimaryKeyColumn)
        {
          return;
        }

        for (int i = 1; i < Columns.Count && value; i++)
        {
          GetColumnAtIndex(i).PrimaryKey = false;
        }
      }
    }

    /// <summary>
    /// Gets a <see cref="MySqlWorkbenchConnection"/> object representing the connection to a MySQL server instance selected by users.
    /// </summary>
    public MySqlWorkbenchConnection WbConnection { get; private set; }

    #endregion Properties

    /// <summary>
    /// Occurs when a property value on any of the columns in this table changes.
    /// </summary>
    public event PropertyChangedEventHandler TableColumnPropertyValueChanged;

    /// <summary>
    /// Delegate handler for the <see cref="TableWarningsChanged"/> event.
    /// </summary>
    /// <param name="sender">Sender object.</param>
    /// <param name="args">Event arguments.</param>
    public delegate void TableWarningsChangedEventHandler(object sender, TableWarningsChangedArgs args);

    /// <summary>
    /// Occurs when the warnings associated to any of the columns in this table change.
    /// </summary>
    public event TableWarningsChangedEventHandler TableWarningsChanged;

    /// <summary>
    /// Inserts data taken from the given <see cref="MySqlDataTable"/> source table into the corresponding database table with the same name given the column mappings defined on this table.
    /// </summary>
    /// <param name="mappingFromTable"><see cref="MySqlDataTable"/> source table containing the data to insert.</param>
    /// <param name="exception">Exception thrown back (if any) when trying to insert data into the database table.</param>
    /// <param name="sqlQuery">The SQL query to insert data in the given <see cref="MySqlDataTable"/> source table into the database table.</param>
    /// <param name="insertedCount">Number of rows actually inserted into the database.</param>
    /// <returns><see cref="DataTable"/> object containing warnings thrown by the data append operation, null if no warnings were generated.</returns>
    public DataTable AppendDataWithManualQuery(MySqlDataTable mappingFromTable, out Exception exception, out string sqlQuery, out int insertedCount)
    {
      DataSet warningsDs = null;
      insertedCount = 0;
      exception = null;
      MySqlTransaction transaction = null;
      sqlQuery = string.Empty;

      string connectionString = WbConnection.GetConnectionStringBuilder().ConnectionString;
      using (MySqlConnection conn = new MySqlConnection(connectionString))
      {
        try
        {
          conn.Open();
          if (_mysqlMaxAllowedPacket == 0)
          {
            _mysqlMaxAllowedPacket = conn.GetMySqlServerMaxAllowedPacket();
          }

          transaction = conn.BeginTransaction();
          MySqlCommand cmd = new MySqlCommand(string.Empty, conn, transaction);
          int nextRow = 0;
          while (nextRow >= 0)
          {
            string chunkQuery = GetAppendSql(nextRow, -1, true, mappingFromTable, out nextRow);
            cmd.CommandText = chunkQuery;
            sqlQuery += chunkQuery;
            insertedCount += cmd.ExecuteNonQuery();
          }

          transaction.Commit();
          warningsDs = MySqlHelper.ExecuteDataset(conn, "SHOW WARNINGS");
        }
        catch (MySqlException mysqlEx)
        {
          if (transaction != null)
          {
            transaction.Rollback();
          }

          exception = mysqlEx;
          MySqlSourceTrace.WriteAppErrorToLog(mysqlEx);
        }
        catch (Exception ex)
        {
          if (transaction != null)
          {
            transaction.Rollback();
          }

          exception = ex;
          MySqlSourceTrace.WriteAppErrorToLog(ex);
        }
      }

      return warningsDs != null && warningsDs.Tables.Count > 0 ? warningsDs.Tables[0] : null;
    }

    /// <summary>
    /// Creates a new <see cref="MySqlDataTable"/> object with its schema cloned from this table but no data.
    /// </summary>
    /// <returns>Cloned <see cref="MySqlDataTable"/> object.</returns>
    public MySqlDataTable CloneSchema()
    {
      MySqlDataTable clonedTable = new MySqlDataTable(
        SchemaName,
        TableName,
        AddPrimaryKeyColumn,
        IsFormatted,
        RemoveEmptyColumns,
        DetectDatatype,
        AddBufferToVarchar,
        AutoIndexIntColumns,
        AutoAllowEmptyNonIndexColumns,
        WbConnection)
      {
        UseFirstColumnAsPk = UseFirstColumnAsPk,
        IsFormatted = IsFormatted,
        FirstRowIsHeaders = FirstRowIsHeaders
      };

      foreach (MySqlDataColumn clonedColumn in from MySqlDataColumn column in Columns select column.CloneSchema())
      {
        clonedTable.Columns.Add(clonedColumn);
      }

      return clonedTable;
    }

    /// <summary>
    /// Checks if a column with the given column name is being used in the table's primary key.
    /// </summary>
    /// <param name="columnName">Column name.</param>
    /// <param name="caseSensitive">Flag indicating if a case sensitive comparison against the column name should be done.</param>
    /// <returns><c>true</c> if the given column is used in the table's primary key, <c>false</c> otherwise.</returns>
    public bool ColumnIsPrimaryKey(string columnName, bool caseSensitive)
    {
      if (!caseSensitive)
      {
        columnName = columnName.ToLowerInvariant();
      }

      return (from MySqlDataColumn col in Columns
              let columnDisplayName = caseSensitive ? col.DisplayName : col.DisplayName.ToLowerInvariant()
              where columnDisplayName == columnName && col.PrimaryKey
              select col).Any();
    }

    /// <summary>
    /// Checks if a column with the given column name is being used in the table's primary key doing a case sensitive comparison of its name.
    /// </summary>
    /// <param name="columnName">Column name.</param>
    /// <returns><c>true</c> if the given column is used in the table's primary key, <c>false</c> otherwise.</returns>
    public bool ColumnIsPrimaryKey(string columnName)
    {
      return ColumnIsPrimaryKey(columnName, true);
    }

    /// <summary>
    /// Creates a new table in the database based on this table's schema information.
    /// </summary>
    /// <param name="exception">Exception thrown back (if any) when trying to create the table in the database.</param>
    /// <param name="sqlQuery">The SQL query to create a new table in the database based on this table's schema information.</param>
    /// <returns><see cref="DataTable"/> object containing warnings thrown by the table's creation, null if no warnings were generated.</returns>
    public DataTable CreateTable(out Exception exception, out string sqlQuery)
    {
      DataSet warningsDs = null;
      string connectionString = WbConnection.GetConnectionStringBuilder().ConnectionString;
      exception = null;
      sqlQuery = GetCreateSql(true);

      try
      {
        using (MySqlConnection conn = new MySqlConnection(connectionString))
        {
          conn.Open();
          MySqlCommand cmd = new MySqlCommand(sqlQuery, conn);
          cmd.ExecuteNonQuery();
          warningsDs = MySqlHelper.ExecuteDataset(conn, "SHOW WARNINGS");
        }
      }
      catch (MySqlException ex)
      {
        exception = ex;
        MySqlSourceTrace.WriteAppErrorToLog(ex);
      }

      return warningsDs != null && warningsDs.Tables.Count > 0 ? warningsDs.Tables[0] : null;
    }

    /// <summary>
    /// Creates a SQL query to insert rows taken from the given <see cref="MySqlDataTable"/> source table into mapped columns in this table.
    /// </summary>
    /// <param name="startRow">Values to be inserted are taken from this row number forward.</param>
    /// <param name="limit">Maximum number of rows in the table to be inserted with this query, if -1 all rows are included.</param>
    /// <param name="formatNewLinesAndTabs">Flag indicating if the SQL statement must be formatted to insert new line and tab characters for display purposes.</param>
    /// <param name="mappingFromTable"><see cref="MySqlDataTable"/> source table containing the data to insert.</param>
    /// <param name="nextRow">Last row processed if the query needs to be split, -1 if all rows were processed.</param>
    /// <returns>INSERT INTO SQL query.</returns>
    public string GetAppendSql(int startRow, int limit, bool formatNewLinesAndTabs, MySqlDataTable mappingFromTable, out int nextRow)
    {
      nextRow = -1;
      ulong maxByteCount = _mysqlMaxAllowedPacket > 0 ? _mysqlMaxAllowedPacket - SAFE_BYTES_BEFORE_REACHING_MAX_ALLOWED_PACKET : 0;
      int colsCount = Columns.Count;
      int rowsCount = mappingFromTable.Rows.Count;

      if (startRow < 0)
      {
        startRow = 0;
      }

      if (mappingFromTable.FirstRowIsHeaders && startRow == 0)
      {
        startRow++;
      }

      if (mappingFromTable.Rows.Count - (mappingFromTable.FirstRowIsHeaders ? 1 : 0) < 1)
      {
        return null;
      }

      if (startRow > rowsCount)
      {
        return null;
      }

      ulong queryStringByteCount = 0;
      StringBuilder queryString = new StringBuilder();
      string nl = formatNewLinesAndTabs ? Environment.NewLine : " ";
      int rowIdx;
      int colIdx;
      int startingColNum = AddPrimaryKeyColumn ? (_useFirstColumnAsPk ? 0 : 1) : 0;
      List<string> fromColumnNames = new List<string>(colsCount);
      List<string> toColumnNames = new List<string>(colsCount);

      string rowsSeparator = string.Empty;
      string colsSeparator = string.Empty;
      queryString.AppendFormat(
        "INSERT INTO `{0}`.`{1}`{2}(",
        SchemaName,
        TableNameForSqlQueries,
        nl);

      // Loop columns to assemble the piece of the query that includes the column names that we will insert data into.
      for (colIdx = startingColNum; colIdx < colsCount; colIdx++)
      {
        MySqlDataColumn toColumn = GetColumnAtIndex(colIdx);
        string fromColumnName = toColumn.MappedDataColName;
        if (toColumn.ExcludeColumn || string.IsNullOrEmpty(fromColumnName))
        {
          continue;
        }

        queryString.AppendFormat(
          "{0}`{1}`",
          colsSeparator,
          toColumn.ColumnNameForSqlQueries);
        colsSeparator = ",";
        fromColumnNames.Add(fromColumnName);
        toColumnNames.Add(toColumn.ColumnName);
      }

      queryString.AppendFormat("){0}VALUES{0}", nl);
      int absRowIdx = 0;
      StringBuilder singleRowValuesBuilder = new StringBuilder();
      if (maxByteCount > 0)
      {
        queryStringByteCount = (ulong)Encoding.ASCII.GetByteCount(queryString.ToString());
      }

      // Loop all rows in this table to include the values for insertion in the query.
      for (rowIdx = startRow; rowIdx < rowsCount; rowIdx++)
      {
        DataRow dr = mappingFromTable.Rows[rowIdx];
        if (limit > 0 && absRowIdx > limit)
        {
          if (rowIdx < rowsCount)
          {
            nextRow = rowIdx;
          }

          break;
        }

        absRowIdx++;

        // Within the current row, loop all columns to extract each value and append it to the query string.
        singleRowValuesBuilder.Clear();
        singleRowValuesBuilder.AppendFormat("{0}(", rowsSeparator);
        colsSeparator = string.Empty;
        for (colIdx = 0; colIdx < toColumnNames.Count; colIdx++)
        {
          string fromColumnName = fromColumnNames[colIdx];
          string toColumnName = toColumnNames[colIdx];
          MySqlDataColumn toColumn = Columns[toColumnName] as MySqlDataColumn;
          bool insertingValueIsNull;
          string valueToDb = DataTypeUtilities.GetStringValueForColumn(dr[fromColumnName], toColumn, true, out insertingValueIsNull);
          singleRowValuesBuilder.AppendFormat(
            "{0}{1}{2}{1}",
            colsSeparator,
            toColumn != null && toColumn.ColumnsRequireQuotes && !insertingValueIsNull ? "'" : string.Empty,
            valueToDb);
          colsSeparator = ",";
        }

        // Close the current row values piece of the query and check if we have not exceeded the maximum packet size allowed by the server,
        //  otherwise we return the query string as is and return the last row number that was processed so another INSERT INTO query is
        //  assembled starting from the row we left on.
        singleRowValuesBuilder.Append(")");
        string singleRowValuesString = singleRowValuesBuilder.ToString();
        if (maxByteCount > 0)
        {
          ulong singleValueRowQueryByteCount = (ulong)Encoding.ASCII.GetByteCount(singleRowValuesString);
          if (queryStringByteCount + singleValueRowQueryByteCount > maxByteCount)
          {
            nextRow = rowIdx;
            break;
          }

          queryStringByteCount += singleValueRowQueryByteCount;
        }

        // Add a , separator for the collection of values in the INSERT QUERY.
        queryString.Append(singleRowValuesString);
        if (rowsSeparator.Length == 0)
        {
          rowsSeparator = "," + nl;
        }
      }

      if (nextRow >= 0)
      {
        queryString.AppendFormat(";{0}", nl);
      }

      return queryString.ToString();
    }

    /// <summary>
    /// Gets a collection of column names with data changes within the given <see cref="DataRow"/> object.
    /// </summary>
    /// <param name="changesRow"><see cref="DataRow"/> object to inspect for changes.</param>
    /// <returns>List of column names with data changes.</returns>
    public List<string> GetChangedColumns(DataRow changesRow)
    {
      if (changesRow == null)
      {
        return null;
      }

      List<string> changedColNamesList = new List<string>(changesRow.Table.Columns.Count);
      foreach (DataColumn col in changesRow.Table.Columns.Cast<DataColumn>().Where(col => !changedColNamesList.Contains(col.ColumnName) && !changesRow[col, DataRowVersion.Original].Equals(changesRow[col, DataRowVersion.Current])))
      {
        changedColNamesList.Add(col.ColumnName);
      }

      return changedColNamesList;
    }

    /// <summary>
    /// Gets the <see cref="MySqlDataColumn"/> object at the given position within the columns collection.
    /// </summary>
    /// <param name="index">Ordinal index within the columns collection</param>
    /// <returns>A <see cref="MySqlDataColumn"/> object.</returns>
    public MySqlDataColumn GetColumnAtIndex(int index)
    {
      MySqlDataColumn col = null;
      if (index >= 0 && index < Columns.Count)
      {
        col = Columns[index] as MySqlDataColumn;
      }

      return col;
    }

    /// <summary>
    /// Gets the ordinal index within the columns collection of the column with the given name.
    /// </summary>
    /// <param name="columnName">Column name.</param>
    /// <param name="useDisplayName">Flag indicating whether the <see cref="MySqlDataColumn.DisplayName"/> or the <see cref="MySqlDataColumn.ColumnName"/>
    /// property must be used for the name comparison.</param>
    /// <param name="caseSensitive">Flag indicating if a case sensitive comparison against the column name should be done.</param>
    /// <returns>The ordinal index within the columns collection.</returns>
    public int GetColumnIndex(string columnName, bool useDisplayName, bool caseSensitive)
    {
      int index = -1;

      if (!caseSensitive)
      {
        columnName = columnName.ToLowerInvariant();
      }

      foreach (MySqlDataColumn col in Columns)
      {
        string localColumnName = useDisplayName ? col.DisplayName : col.ColumnName;
        if (!caseSensitive)
        {
          localColumnName = localColumnName.ToLowerInvariant();
        }

        if (localColumnName != columnName)
        {
          continue;
        }

        index = col.Ordinal;
        break;
      }

      return index;
    }

    /// <summary>
    /// Gets the ordinal index within the columns collection of the column with the given name doing a case sensitive comparison.
    /// </summary>
    /// <param name="columnName">Column name.</param>
    /// <param name="useDisplayName">Flag indicating whether the <see cref="MySqlDataColumn.DisplayName"/> or the <see cref="DataColumn.ColumnName"/> property must be used for the name comparison.</param>
    /// <returns>The ordinal index within the columns collection.</returns>
    public int GetColumnIndex(string columnName, bool useDisplayName)
    {
      return GetColumnIndex(columnName, useDisplayName, true);
    }

    /// <summary>
    /// Gets an array with the names of all columns in this table.
    /// </summary>
    /// <param name="useDisplayName">Flag indicating whether the <see cref="MySqlDataColumn.DisplayName"/> or the <see cref="DataColumn.ColumnName"/>
    /// property must be used for the name comparison.</param>
    /// <returns>A string array containing all column names in this table.</returns>
    public string[] GetColumnNamesArray(bool useDisplayName)
    {
      if (Columns.Count <= 0)
      {
        return null;
      }

      string[] retArray = new string[Columns.Count];
      for (int i = 0; i < Columns.Count; i++)
      {
        retArray[i] = useDisplayName ? GetColumnAtIndex(i).DisplayName : Columns[i].ColumnName;
      }

      return retArray;
    }

    /// <summary>
    /// Gets an array with the names of all columns in this table using the <see cref="DataColumn.ColumnName"/> property of the columns.
    /// </summary>
    /// <returns>A string array containing all column names in this table.</returns>
    public string[] GetColumnNamesArray()
    {
      return GetColumnNamesArray(false);
    }

    /// <summary>
    /// Creates a SQL query to create a new table in the database based on this table's schema information.
    /// </summary>
    /// <param name="formatNewLinesAndTabs">Flag indicating if the SQL statement must be formatted to insert new line and tab characters for display purposes.</param>
    /// <returns>CREATE TABLE SQL query.</returns>
    public string GetCreateSql(bool formatNewLinesAndTabs)
    {
      StringBuilder sql = new StringBuilder();
      string nl = formatNewLinesAndTabs ? Environment.NewLine : " ";
      string nlt = formatNewLinesAndTabs ? Environment.NewLine + "\t" : " ";

      sql.AppendFormat("CREATE TABLE `{0}`.`{1}`{2}(", SchemaName, TableName, nl);

      string delimiter = nlt;
      int skipNum = AddPrimaryKeyColumn ? (_useFirstColumnAsPk ? 0 : 1) : 0;
      foreach (MySqlDataColumn col in Columns.OfType<MySqlDataColumn>().Skip(skipNum).Where(c => !c.ExcludeColumn))
      {
        sql.AppendFormat("{0}{1}", delimiter, col.GetSql());
        delimiter = "," + nlt;
      }

      if (NumberOfPk > 1)
      {
        string pkDelimiter = string.Empty;
        sql.AppendFormat("{0}PRIMARY KEY (", delimiter);
        foreach (MySqlDataColumn col in Columns.OfType<MySqlDataColumn>().Skip(1).Where(c => c.PrimaryKey))
        {
          sql.AppendFormat("{0}`{1}`", pkDelimiter, col.DisplayNameForSqlQueries);
          pkDelimiter = ",";
        }

        sql.Append(")");
      }

      foreach (MySqlDataColumn col in Columns.OfType<MySqlDataColumn>().Where(c => !(c.AutoPk || c.PrimaryKey || c.UniqueKey || c.ExcludeColumn || !c.CreateIndex)))
      {
        sql.AppendFormat("{0}INDEX `{1}_idx` (`{1}`)", delimiter, col.DisplayNameForSqlQueries);
      }

      sql.Append(nl);
      sql.Append(")");
      return sql.ToString();
    }

    /// <summary>
    /// Creates a SQL query to insert rows in this table into the corresponding MySQL database table.
    /// </summary>
    /// <param name="startRow">Values to be inserted are taken from this row number forward.</param>
    /// <param name="limit">Maximum number of rows in the table to be inserted with this query, if -1 all rows are included.</param>
    /// <param name="formatNewLinesAndTabs">Flag indicating if the SQL statement must be formatted to insert new line and tab characters for display purposes.</param>
    /// <param name="nextRow">Last row processed if the query needs to be split, -1 if all rows were processed.</param>
    /// <param name="insertingRowsCount">Number of rows to be inserted into the database with this query.</param>
    /// <returns>INSERT INTO SQL query.</returns>
    public string GetInsertSql(int startRow, int limit, bool formatNewLinesAndTabs, out int nextRow, out int insertingRowsCount)
    {
      nextRow = -1;
      insertingRowsCount = 0;

      if (startRow < 0)
      {
        startRow = 0;
      }

      if (_firstRowIsHeaders && startRow == 0)
      {
        startRow++;
      }

      if (startRow >= Rows.Count)
      {
        return null;
      }

      ulong maxByteCount = _mysqlMaxAllowedPacket > 0 ? _mysqlMaxAllowedPacket - SAFE_BYTES_BEFORE_REACHING_MAX_ALLOWED_PACKET : 0;
      ulong queryStringByteCount = 0;
      StringBuilder queryString = new StringBuilder();
      string nl = formatNewLinesAndTabs ? Environment.NewLine : " ";
      int rowIdx;
      int colIdx;
      int startingColNum = AddPrimaryKeyColumn ? (_useFirstColumnAsPk ? 0 : 1) : 0;
      List<string> insertColumnNames = new List<string>(Columns.Count);

      string rowsSeparator = string.Empty;
      string colsSeparator = string.Empty;
      queryString.AppendFormat(
        "INSERT INTO `{0}`.`{1}`{2}(",
        SchemaName,
        TableNameForSqlQueries,
        nl);

      // Loop columns to assemble the piece of the query that includes the column names that we will insert data into.
      for (colIdx = startingColNum; colIdx < Columns.Count; colIdx++)
      {
        MySqlDataColumn column = GetColumnAtIndex(colIdx);
        if (column.ExcludeColumn)
        {
          continue;
        }

        queryString.AppendFormat(
          "{0}`{1}`",
          colsSeparator,
          column.DisplayNameForSqlQueries);
        colsSeparator = ",";
        insertColumnNames.Add(column.ColumnName);
      }

      queryString.AppendFormat("){0}VALUES{0}", nl);
      int absRowIdx = 0;
      StringBuilder singleRowValuesBuilder = new StringBuilder();
      if (maxByteCount > 0)
      {
        queryStringByteCount = (ulong)Encoding.ASCII.GetByteCount(queryString.ToString());
      }

      // Loop all rows in this table to include the values for insertion in the query.
      for (rowIdx = startRow; rowIdx < Rows.Count; rowIdx++)
      {
        if (limit > 0 && absRowIdx > limit)
        {
          if (rowIdx < Rows.Count)
          {
            nextRow = rowIdx;
          }

          break;
        }

        absRowIdx++;

        // Within the current row, loop all columns to extract each value and append it to the query string.
        DataRow dr = Rows[rowIdx];
        singleRowValuesBuilder.Clear();
        singleRowValuesBuilder.AppendFormat("{0}(", rowsSeparator);
        colsSeparator = string.Empty;
        foreach (string insertingColName in insertColumnNames)
        {
          MySqlDataColumn column = Columns[insertingColName] as MySqlDataColumn;
          bool insertingValueIsNull;
          string valueToDb = DataTypeUtilities.GetStringValueForColumn(dr[insertingColName], column, true, out insertingValueIsNull);
          singleRowValuesBuilder.AppendFormat(
            "{0}{1}{2}{1}",
            colsSeparator,
            column != null && column.ColumnsRequireQuotes && !insertingValueIsNull ? "'" : string.Empty,
            valueToDb);
          colsSeparator = ",";
        }

        // Close the current row values piece of the query and check if we have not exceeded the maximum packet size allowed by the server,
        //  otherwise we return the query string as is and return the last row number that was processed so another INSERT INTO query is
        //  assembled starting from the row we left on.
        singleRowValuesBuilder.Append(")");
        string singleRowValuesString = singleRowValuesBuilder.ToString();
        if (maxByteCount > 0)
        {
          ulong singleValueRowQueryByteCount = (ulong)Encoding.ASCII.GetByteCount(singleRowValuesString);
          if (queryStringByteCount + singleValueRowQueryByteCount > maxByteCount)
          {
            nextRow = rowIdx;
            break;
          }

          queryStringByteCount += singleValueRowQueryByteCount;
        }

        // Add a , separator for the collection of values in the INSERT QUERY.
        queryString.Append(singleRowValuesString);
        if (rowsSeparator.Length == 0)
        {
          rowsSeparator = "," + nl;
        }

        insertingRowsCount++;
      }

      if (nextRow >= 0)
      {
        queryString.AppendFormat(";{0}", nl);
      }

      return queryString.ToString();
    }

    /// <summary>
    /// Creates a SQL query to insert all rows in this table into the corresponding MySQL database table.
    /// </summary>
    /// <param name="limit">Maximum number of rows in the table to be inserted with this query, if -1 all rows are included.</param>
    /// <param name="formatNewLinesAndTabs">Flag indicating if the SQL statement must be formatted to insert new line and tab characters for display purposes.</param>
    /// <returns>INSERT INTO SQL query.</returns>
    public string GetInsertSql(int limit, bool formatNewLinesAndTabs)
    {
      int nextRow;
      int insertingRowsCount;
      return GetInsertSql(0, limit, formatNewLinesAndTabs, out nextRow, out insertingRowsCount);
    }

    /// <summary>
    /// Gets a column name avoiding duplicates by adding a numeric suffix in case it already exists in the table.
    /// </summary>
    /// <param name="proposedName">Proposed column name.</param>
    /// <param name="forColumnIndex">Index of the column this name will be used for.</param>
    /// <returns>Unique column name.</returns>
    public string GetNonDuplicateColumnName(string proposedName, int forColumnIndex = -1)
    {
      List<string> columnNames = Columns.Count > 0 ? Columns.OfType<MySqlDataColumn>().Where(col => col.Ordinal != forColumnIndex).Select(col => col.DisplayName).ToList() : null;
      return columnNames.GetNonDuplicateText(proposedName);
    }

    /// <summary>
    /// Creates a SQL query meant to push changes in the given <see cref="DataRow"/> object to the database server.
    /// </summary>
    /// <param name="row"><see cref="DataRow"/> object with changes to push to the database server.</param>
    /// <returns>A SQL query containing the data changes.</returns>
    public string GetSql(DataRow row)
    {
      if (row == null || row.RowState == DataRowState.Unchanged)
      {
        return string.Empty;
      }

      string valueToDb;
      ulong maxByteCount = _mysqlMaxAllowedPacket > 0 ? _mysqlMaxAllowedPacket - SAFE_BYTES_BEFORE_REACHING_MAX_ALLOWED_PACKET : 0;
      StringBuilder queryString = new StringBuilder();
      string colsSeparator = string.Empty;

      switch (row.RowState)
      {
        case DataRowState.Deleted:
          queryString.AppendFormat(
            "DELETE FROM `{0}`.`{1}` WHERE ",
            SchemaName,
            TableNameForSqlQueries);
          foreach (MySqlDataColumn pkCol in Columns.Cast<MySqlDataColumn>().Where(pkCol => pkCol.PrimaryKey))
          {
            bool pkValueIsNull;
            valueToDb = DataTypeUtilities.GetStringValueForColumn(row[pkCol.ColumnName, DataRowVersion.Original], pkCol, false, out pkValueIsNull);
            queryString.AppendFormat(
              "{0}`{1}`={2}{3}{2}",
              colsSeparator,
              pkCol.ColumnNameForSqlQueries,
              pkCol.ColumnsRequireQuotes && !pkValueIsNull ? "'" : string.Empty,
              valueToDb);
            colsSeparator = " AND ";
          }

          break;

        case DataRowState.Added:
          int colIdx;
          int startingColNum = AddPrimaryKeyColumn ? (_useFirstColumnAsPk ? 0 : 1) : 0;
          List<string> insertColumnNames = new List<string>(Columns.Count);
          queryString.AppendFormat(
            "INSERT INTO `{0}`.`{1}` (",
            SchemaName,
            TableNameForSqlQueries);
          for (colIdx = startingColNum; colIdx < Columns.Count; colIdx++)
          {
            MySqlDataColumn column = GetColumnAtIndex(colIdx);
            if (column.ExcludeColumn)
            {
              continue;
            }

            queryString.AppendFormat(
              "{0}`{1}`",
              colsSeparator,
              column.DisplayNameForSqlQueries);
            colsSeparator = ",";
            insertColumnNames.Add(column.ColumnName);
          }

          queryString.Append(") VALUES (");
          colsSeparator = string.Empty;
          foreach (string insertingColName in insertColumnNames)
          {
            MySqlDataColumn column = Columns[insertingColName] as MySqlDataColumn;
            bool insertingValueIsNull;
            valueToDb = DataTypeUtilities.GetStringValueForColumn(row[insertingColName], column, true, out insertingValueIsNull);
            queryString.AppendFormat(
              "{0}{1}{2}{1}",
              colsSeparator,
              column != null && column.ColumnsRequireQuotes && !insertingValueIsNull ? "'" : string.Empty,
              valueToDb);
            colsSeparator = ",";
          }

          queryString.Append(")");
          break;

        case DataRowState.Modified:
          StringBuilder wClauseString = new StringBuilder(" WHERE ");
          string wClauseColsSeparator = string.Empty;
          List<string> changedColNamesList = GetChangedColumns(row);
          queryString.AppendFormat(
            "UPDATE `{0}`.`{1}` SET ",
            SchemaName,
            TableNameForSqlQueries);
          foreach (MySqlDataColumn column in Columns)
          {
            bool updatingValueIsNull;
            string finalColName = column.ColumnNameForSqlQueries;
            if (column.PrimaryKey)
            {
              valueToDb = DataTypeUtilities.GetStringValueForColumn(row[column.ColumnName, DataRowVersion.Original], column, false, out updatingValueIsNull);
              wClauseString.AppendFormat(
                "{0}`{1}`={2}{3}{2}",
                wClauseColsSeparator,
                finalColName,
                column.ColumnsRequireQuotes && !updatingValueIsNull ? "'" : string.Empty,
                valueToDb);
              wClauseColsSeparator = " AND ";
            }

            if (!changedColNamesList.Contains(column.ColumnName))
            {
              continue;
            }

            valueToDb = DataTypeUtilities.GetStringValueForColumn(row[column.ColumnName], column, true, out updatingValueIsNull);
            queryString.AppendFormat(
              "{0}`{1}`={2}{3}{2}",
              colsSeparator,
              finalColName,
              column.ColumnsRequireQuotes && !updatingValueIsNull ? "'" : string.Empty,
              valueToDb);
            colsSeparator = ",";
          }

          wClauseString.Append(";");
          queryString.Append(wClauseString);
          break;
      }

      // Verify we have not exceeded the maximum packet size allowed by the server, otherwise throw an Exception.
      string retQuery = queryString.ToString();
      if (maxByteCount <= 0)
      {
        return retQuery;
      }

      ulong queryStringByteCount = (ulong)Encoding.ASCII.GetByteCount(retQuery);
      if (queryStringByteCount > maxByteCount)
      {
        throw new Exception(Properties.Resources.QueryExceedsMaxAllowedPacketError);
      }

      return retQuery;
    }

    /// <summary>
    /// Creates a SQL query meant to push changes in the <see cref="DataRow"/> object found at the given row index.
    /// </summary>
    /// <param name="rowIndex">Row index of the <see cref="DataRow"/> object containing the changes.</param>
    /// <returns>A SQL query containing the data changes.</returns>
    public string GetSql(int rowIndex)
    {
      if (rowIndex < 0 || rowIndex >= Rows.Count)
      {
        return string.Empty;
      }

      return GetSql(Rows[rowIndex]);
    }

    /// <summary>
    /// Inserts data contained in this table into the corresponding database table with the same name.
    /// </summary>
    /// <param name="exception">Exception thrown back (if any) when trying to insert data into the database table.</param>
    /// <param name="sqlQuery">The SQL query to insert data in this table into the database table.</param>
    /// <param name="insertingRows">Number of rows to be inserted into the database with this query.</param>
    /// <param name="insertedRows">Number of rows actually inserted into the database.</param>
    /// <returns><see cref="DataTable"/> object containing warnings thrown by the data insertion, null if no warnings were generated.</returns>
    public DataTable InsertDataWithManualQuery(out Exception exception, out string sqlQuery, out int insertingRows, out int insertedRows)
    {
      DataSet warningsDs = null;
      insertedRows = 0;
      insertingRows = 0;
      exception = null;
      MySqlTransaction transaction = null;
      sqlQuery = string.Empty;

      string connectionString = WbConnection.GetConnectionStringBuilder().ConnectionString;
      using (MySqlConnection conn = new MySqlConnection(connectionString))
      {
        try
        {
          conn.Open();
          if (_mysqlMaxAllowedPacket == 0)
          {
            _mysqlMaxAllowedPacket = conn.GetMySqlServerMaxAllowedPacket();
          }

          transaction = conn.BeginTransaction();
          MySqlCommand cmd = new MySqlCommand(string.Empty, conn, transaction);
          int nextRow = 0;
          while (nextRow >= 0)
          {
            int insertingRowsCount;
            string chunkQuery = GetInsertSql(nextRow, -1, true, out nextRow, out insertingRowsCount);
            cmd.CommandText = chunkQuery;
            sqlQuery += chunkQuery;
            insertedRows += cmd.ExecuteNonQuery();
            insertingRows += insertingRowsCount;
          }

          transaction.Commit();
          warningsDs = MySqlHelper.ExecuteDataset(conn, "SHOW WARNINGS");
        }
        catch (MySqlException mysqlEx)
        {
          if (transaction != null)
          {
            transaction.Rollback();
          }

          exception = mysqlEx;
          MySqlSourceTrace.WriteAppErrorToLog(mysqlEx);
        }
        catch (Exception ex)
        {
          if (transaction != null)
          {
            transaction.Rollback();
          }

          exception = ex;
          MySqlSourceTrace.WriteAppErrorToLog(ex);
        }
      }

      return warningsDs != null && warningsDs.Tables.Count > 0 ? warningsDs.Tables[0] : null;
    }

    /// <summary>
    /// Pushes all changes in this table's data to its corresponding database table.
    /// </summary>
    /// <returns>A <see cref="PushResultsDataTable"/> object containing a log of the results of each query executed against the database server.</returns>
    public PushResultsDataTable PushData()
    {
      var dataTable = GetChanges();
      if (dataTable != null && dataTable.Rows.Count == 0)
      {
        return null;
      }

      PushResultsDataTable resultsDt = new PushResultsDataTable();
      MySqlTransaction transaction = null;
      string connectionString = WbConnection.GetConnectionStringBuilder().ConnectionString;
      DataRowState[] pushOperationsArray = { DataRowState.Deleted, DataRowState.Added, DataRowState.Modified };
      PushResultsDataTable.OperationType currentOperationType = PushResultsDataTable.OperationType.Prepare;
      string queryText = string.Empty;
      StringBuilder warningText = new StringBuilder();
      int operationIndex = 0;
      DataRow lastRow = null;

      using (MySqlConnection conn = new MySqlConnection(connectionString))
      {
        string errorText;
        try
        {
          conn.Open();
          if (_mysqlMaxAllowedPacket == 0)
          {
            _mysqlMaxAllowedPacket = conn.GetMySqlServerMaxAllowedPacket();
          }

          transaction = conn.BeginTransaction();
          MySqlCommand cmd = new MySqlCommand(string.Empty, conn, transaction);

          foreach (DataRowState operation in pushOperationsArray)
          {
            DataRowState operationCopy = operation;
            foreach (DataRow dr in Rows.Cast<DataRow>().Where(dr => dr.RowState == operationCopy))
            {
              warningText.Clear();
              operationIndex++;
              lastRow = dr;
              queryText = GetSql(dr);
              cmd.CommandText = queryText;
              int executedCount = cmd.ExecuteNonQuery();
              DataSet warningsDs = MySqlHelper.ExecuteDataset(conn, "SHOW WARNINGS");
              currentOperationType = PushResultsDataTable.GetRelatedOperationType(operation);
              if ((warningsDs != null && warningsDs.Tables.Count > 0 && warningsDs.Tables[0].Rows.Count > 0) || executedCount == 0)
              {
                string nl = string.Empty;
                if (executedCount == 0)
                {
                  dr.RowError = NO_MATCH;
                  warningText.AppendFormat(
                    "{2}{0:000}: {1}",
                    operationIndex,
                    Properties.Resources.QueryDidNotMatchRowsWarning,
                    nl);
                  nl = Environment.NewLine;
                }

                if (warningsDs != null)
                {
                  foreach (DataRow warningRow in warningsDs.Tables[0].Rows)
                  {
                    warningText.AppendFormat(
                      "{3}{0:000}: {1} - {2}",
                      operationIndex,
                      warningRow[1],
                      warningRow[2],
                      nl);
                    nl = Environment.NewLine;
                  }
                }

                resultsDt.AddResult(operationIndex, currentOperationType, PushResultsDataTable.OperationResult.Warning, queryText, warningText.ToString(), executedCount);
              }
              else
              {
                resultsDt.AddResult(operationIndex, currentOperationType, PushResultsDataTable.OperationResult.Success, queryText, "OK", executedCount);
              }
            }
          }

          transaction.Commit();
          for (int rowIdx = 0; rowIdx < Rows.Count; rowIdx++)
          {
            DataRow dr = Rows[rowIdx];
            if (dr.RowState == DataRowState.Unchanged || dr.RowError == NO_MATCH)
            {
              continue;
            }

            if (dr.RowState == DataRowState.Deleted)
            {
              rowIdx--;
            }

            dr.AcceptChanges();
          }
        }
        catch (MySqlException mysqlEx)
        {
          if (transaction != null)
          {
            transaction.Rollback();
          }

          if (lastRow != null)
          {
            lastRow.RowError = mysqlEx.Message;
          }

          errorText = string.Format(Properties.Resources.ErrorMySQLText, mysqlEx.Number) + Environment.NewLine + mysqlEx.Message;
          resultsDt.AddResult(operationIndex, currentOperationType, PushResultsDataTable.OperationResult.Error, queryText, errorText, 0);
          MySqlSourceTrace.WriteAppErrorToLog(mysqlEx);
        }
        catch (Exception ex)
        {
          if (transaction != null)
          {
            transaction.Rollback();
          }

          if (lastRow != null)
          {
            lastRow.RowError = ex.Message;
          }

          errorText = Properties.Resources.ErrorAdoNetText + Environment.NewLine + ex.Message;
          resultsDt.AddResult(operationIndex, currentOperationType, PushResultsDataTable.OperationResult.Error, queryText, errorText, 0);
          MySqlSourceTrace.WriteAppErrorToLog(ex);
        }
      }

      return resultsDt;
    }

    /// <summary>
    /// Resets the name of the auto-generated primary key column.
    /// </summary>
    public void ResetAutoPkName()
    {
      SetupAutoPkColumn(false);
    }

    /// <summary>
    /// Reverts any changes done to the table since the last data push operation or refreshes its data with a fresh copy of the data.
    /// </summary>
    /// <param name="refreshFromDb">Flag indicating if instead of just reverting present changes a fresh copy of the data must be retrieved from the database.</param>
    /// <param name="exception">Exception thrown back (if any) when trying to fetch data from the database to refresh the data for this table.</param>
    public void RevertData(bool refreshFromDb, out Exception exception)
    {
      exception = null;

      if (!refreshFromDb)
      {
        RejectChanges();
        return;
      }

      try
      {
        Clear();
        DataTable filledTable = WbConnection.GetDataFromTableOrView(SelectQuery);
        CreateTableSchema(TableName, true);
        CopyTableData(filledTable);
      }
      catch (MySqlException mysqlEx)
      {
        exception = mysqlEx;
        MySqlSourceTrace.WriteAppErrorToLog(mysqlEx);
      }
      catch (Exception ex)
      {
        exception = ex;
        MySqlSourceTrace.WriteAppErrorToLog(ex);
      }
    }

    /// <summary>
    /// Searches for the row index in this table corresponding to the given Excel row index skipping deleted rows.
    /// </summary>
    /// <param name="excelRowIdx">The Excel row index.</param>
    /// <param name="skipIndexesList">A list of row indexes to skip since they are flagged for deletion.</param>
    /// <param name="editingRangeRowsCount">The number of Excel rows in the editing Excel range.</param>
    /// <returns>The corresponding row index in the <see cref="MySqlDataTable"/>.</returns>
    public int SearchRowIndexNotDeleted(int excelRowIdx, IList<int> skipIndexesList, int editingRangeRowsCount)
    {
      int notDeletedIdx = -1;

      if (Rows.Count == editingRangeRowsCount - 2)
      {
        return excelRowIdx;
      }

      for (int tableRowIdx = 0; tableRowIdx < Rows.Count; tableRowIdx++)
      {
        if (Rows[tableRowIdx].RowState != DataRowState.Deleted)
        {
          notDeletedIdx++;
        }

        if (skipIndexesList != null)
        {
          notDeletedIdx += skipIndexesList.Count(n => n == tableRowIdx);
        }

        if (notDeletedIdx == excelRowIdx)
        {
          return tableRowIdx;
        }
      }

      return -1;
    }

    /// <summary>
    /// Creates data rows, fills them with the given Excel data and sets column properties automatically given user options.
    /// </summary>
    /// <param name="dataRange">Excel data range containing the data to fill the table.</param>
    /// <param name="recreateColumnsFromData">Flag indicating if any existing columns in the table must be dropped and re-created based on the given data range.</param>
    /// <param name="emptyColumnsToVarchar">Flag indicating if the data type for columns with no data is automatically set to varchar(255).</param>
    public void SetData(Excel.Range dataRange, bool recreateColumnsFromData, bool emptyColumnsToVarchar)
    {
      object[,] excelData;
      Clear();

      // We have to treat a single cell specially.  It doesn't come in as an array but as a single value
      if (dataRange.Count == 1)
      {
        excelData = new object[2, 2];
        excelData[1, 1] = IsFormatted ? dataRange.Value : dataRange.Value2;
      }
      else
      {
        excelData = IsFormatted ? dataRange.Value : dataRange.Value2;
      }

      int numRows = excelData.GetUpperBound(0);
      int numCols = excelData.GetUpperBound(1);
      int colAdjustIdx = AddPrimaryKeyColumn ? 0 : 1;
      List<bool> columnsHaveAnyDataList = new List<bool>(numCols + 1);
      List<bool> columnsContainDatesList = new List<bool>(numCols + 1);
      List<string> colsToDelete = new List<string>(numCols);

      // Create a list of boolean values that state if each column has any data or none.
      if (AddPrimaryKeyColumn)
      {
        columnsHaveAnyDataList.Add(true);
      }

      for (int colIdx = 1; colIdx <= numCols; colIdx++)
      {
        bool colHasAnyData = false;
        for (int rowIdx = 1; rowIdx <= numRows; rowIdx++)
        {
          if (excelData[rowIdx, colIdx] == null)
          {
            continue;
          }

          colHasAnyData = true;
          break;
        }

        columnsHaveAnyDataList.Add(colHasAnyData);
      }

      // Drop all columns and re-create them or create them if none have been created so far.
      if (recreateColumnsFromData || Columns.Count == 0)
      {
        CreateColumns(numCols);
      }

      // Set the IsEmpty and Exclude properties of columns based on the filling data
      for (int colIdx = 0; colIdx < Columns.Count; colIdx++)
      {
        MySqlDataColumn column = GetColumnAtIndex(colIdx);
        columnsContainDatesList.Add(column.IsDate);
        column.IsEmpty = !columnsHaveAnyDataList[colIdx];
        if (!recreateColumnsFromData)
        {
          continue;
        }

        column.ExcludeColumn = column.IsEmpty;
        if (column.IsEmpty)
        {
          colsToDelete.Add(column.ColumnName);
        }
      }

      // Create excelData rows and fill them with the Excel data.
      int pkRowValueAdjust = _firstRowIsHeaders ? 1 : 0;
      for (int row = 1; row <= numRows; row++)
      {
        bool rowHasAnyData = false;
        DataRow dataRow = NewRow();
        dataRow[0] = row - pkRowValueAdjust;
        for (int col = 1; col <= numCols; col++)
        {
          int adjColIdx = col - colAdjustIdx;
          if (!columnsHaveAnyDataList[adjColIdx])
          {
            continue;
          }

          rowHasAnyData = rowHasAnyData || excelData[row, col] != null;
          dataRow[adjColIdx] = excelData[row, col] != null && excelData[row, col].Equals(0.0) && columnsContainDatesList[adjColIdx] ? DataTypeUtilities.MYSQL_EMPTY_DATE : dataRow[adjColIdx] = excelData[row, col];
        }

        if (rowHasAnyData)
        {
          Rows.Add(dataRow);
        }
        else
        {
          pkRowValueAdjust++;
        }
      }

      // Automatically detect the excelData type for columns based on their data.
      if (DetectDatatype)
      {
        DetectTypes(excelData, emptyColumnsToVarchar);
      }

      // Remove from the Columns collection the columns without data if the Remove Empty Columns option is true.
      if (RemoveEmptyColumns)
      {
        foreach (string colName in colsToDelete)
        {
          Columns.Remove(Columns[colName]);
        }
      }

      // Flag columns as allowing nulls if the option to Allow Empty columns is true, valid only for non-index columns.
      if (!AutoAllowEmptyNonIndexColumns)
      {
        return;
      }

      foreach (MySqlDataColumn mysqlCol in Columns)
      {
        mysqlCol.AllowNull = !mysqlCol.CreateIndex;
      }
    }

    /// <summary>
    /// Synchronizes the column properties of this table with the column properties of the given <see cref="MySqlDataTable"/> table.
    /// </summary>
    /// <param name="syncFromTable">A <see cref="MySqlDataTable"/> object from which columns will be synchronized.</param>
    public void SyncSchema(MySqlDataTable syncFromTable)
    {
      if (syncFromTable.Columns.Count != Columns.Count)
      {
        return;
      }

      for (int colIdx = 0; colIdx < Columns.Count; colIdx++)
      {
        MySqlDataColumn thisColumn = GetColumnAtIndex(colIdx);
        MySqlDataColumn syncFromColumn = syncFromTable.GetColumnAtIndex(colIdx);
        thisColumn.SetDisplayName(syncFromColumn.DisplayName);
        thisColumn.SetMySqlDataType(syncFromColumn.MySqlDataType);
        thisColumn.RowsFrom1stDataType = syncFromColumn.RowsFrom1stDataType;
        thisColumn.RowsFrom2ndDataType = syncFromColumn.RowsFrom2ndDataType;
        thisColumn.PrimaryKey = syncFromColumn.PrimaryKey;
        thisColumn.AllowNull = syncFromColumn.AllowNull;
        thisColumn.UniqueKey = syncFromColumn.UniqueKey;
        thisColumn.CreateIndex = syncFromColumn.CreateIndex;
        thisColumn.ExcludeColumn = syncFromColumn.ExcludeColumn;
      }
    }

    /// <summary>
    /// Adds or removes warnings related to this table's auto-generated primary key.
    /// </summary>
    /// <param name="addWarning">true to add a new warning to the auto-generated primary key warnings collection, false to remove the given warning.</param>
    /// <param name="warningResourceText">Warning text to display to users.</param>
    public void UpdateAutoPkWarnings(bool addWarning, string warningResourceText)
    {
      if (UpdateWarnings(addWarning, warningResourceText, true))
      {
        OnTableWarningsChanged(true);
      }
    }

    /// <summary>
    /// Raises the <see cref="TableWarningsChanged"/> event.
    /// </summary>
    /// <param name="column">The <see cref="MySqlDataColumn"/> object that contains changes in its warning texts.</param>
    protected virtual void OnTableWarningsChanged(MySqlDataColumn column)
    {
      if (TableWarningsChanged != null)
      {
        TableWarningsChanged(column, new TableWarningsChangedArgs(column));
      }
    }

    /// <summary>
    /// Raises the <see cref="TableWarningsChanged"/> event.
    /// </summary>
    /// <param name="autoPkWarning">Flag indicating if the warning is related to the auto-generated primary key or to the table.</param>
    protected virtual void OnTableWarningsChanged(bool autoPkWarning)
    {
      if (TableWarningsChanged != null)
      {
        TableWarningsChanged(this, new TableWarningsChangedArgs(this, autoPkWarning));
      }
    }

    /// <summary>
    /// Recreates the values of the automatically created first column for the table's primary key depending on the value of the <see cref="_firstRowIsHeaders"/> field.
    /// </summary>
    private void AdjustAutoPkValues()
    {
      if (!AddPrimaryKeyColumn || Columns.Count <= 0)
      {
        return;
      }

      int adjustIdx = _firstRowIsHeaders ? 0 : 1;
      for (int i = 0; i < Rows.Count; i++)
      {
        Rows[i][0] = i + adjustIdx;
      }
    }

    /// <summary>
    /// Event delegate method fired when a column's property value changes.
    /// </summary>
    /// <param name="sender">A <see cref="MySqlDataColumn"/> object.</param>
    /// <param name="args">Event arguments</param>
    private void ColumnPropertyValueChanged(object sender, PropertyChangedEventArgs args)
    {
      if (TableColumnPropertyValueChanged != null)
      {
        TableColumnPropertyValueChanged(sender as MySqlDataColumn, args);
      }
    }

    /// <summary>
    /// Event delegate method fired when the warning texts list of any column changes.
    /// </summary>
    /// <param name="sender">A <see cref="MySqlDataColumn"/> object.</param>
    /// <param name="args">Event arguments</param>
    private void ColumnWarningsChanged(object sender, ColumnWarningsChangedArgs args)
    {
      OnTableWarningsChanged(sender as MySqlDataColumn);
    }

    /// <summary>
    /// Copies the data contents of the given <see cref="DataTable"/> object to this table.
    /// </summary>
    /// <param name="filledTable"><see cref="DataTable"/> object containing previously retrieved data from a MySQL table.</param>
    private void CopyTableData(DataTable filledTable)
    {
      try
      {
        foreach (DataRow dr in filledTable.Rows)
        {
          ImportRow(dr);
        }
      }
      catch (Exception ex)
      {
        MiscUtilities.ShowCustomizedErrorDialog(Properties.Resources.TableDataCopyErrorTitle, ex.Message + Environment.NewLine + Environment.NewLine + ex.StackTrace, true);
        MySqlSourceTrace.WriteAppErrorToLog(ex);
      }
    }

    /// <summary>
    /// Adds a specified number of <see cref="MySqlDataColumn"/> objects to the Columns collection where the first column may be an automatically created one for the table's primary index.
    /// </summary>
    /// <param name="numCols">Number of columns to add to the table not counting the auto-generated primary key column.</param>
    private void CreateColumns(int numCols)
    {
      Columns.Clear();

      int startCol = AddPrimaryKeyColumn ? 0 : 1;
      for (int colIdx = startCol; colIdx <= numCols; colIdx++)
      {
        MySqlDataColumn column = new MySqlDataColumn(InExportMode)
        {
          ColumnName = AddPrimaryKeyColumn && colIdx == 0 ? "AutoPK" : "Column" + colIdx
        };
        column.SetDisplayName(column.ColumnName);
        column.ColumnWarningsChanged += ColumnWarningsChanged;
        column.PropertyChanged += ColumnPropertyValueChanged;
        Columns.Add(column);
      }

      SetupAutoPkColumn(true);
    }

    /// <summary>
    /// Creates columns for this table using the information schema of a MySQL table with the given name to mirror their properties.
    /// </summary>
    /// <param name="tableName">Name of the table.</param>
    /// <param name="datesAsMySqlDates">Flag indicating if the dates are stored in the table as <see cref="System.DateTime"/> or <see cref="MySql.Data.Types.MySqlDateTime"/> objects.</param>
    private void CreateTableSchema(string tableName, bool datesAsMySqlDates)
    {
      Columns.Clear();
      DataTable columnsInfoTable = WbConnection.GetSchemaCollection("Columns Short", null, WbConnection.Schema, tableName);
      if (columnsInfoTable == null)
      {
        return;
      }

      foreach (DataRow columnInfoRow in columnsInfoTable.Rows)
      {
        string colName = columnInfoRow["Field"].ToString();
        string dataType = columnInfoRow["Type"].ToString();
        bool allowNulls = columnInfoRow["Null"].ToString() == "YES";
        bool isPrimaryKey = columnInfoRow["Key"].ToString() == "PRI";
        string extraInfo = columnInfoRow["Extra"].ToString();
        MySqlDataColumn column = new MySqlDataColumn(colName, dataType, datesAsMySqlDates, allowNulls, isPrimaryKey, extraInfo);
        Columns.Add(column);
      }
    }

    /// <summary>
    /// Converts a data string to a valid column name.
    /// </summary>
    /// <param name="dataValue">String coming from the Excel data intended to be used as a column name.</param>
    /// <returns>A string formatted as a valid column name.</returns>
    private string DataToColName(string dataValue)
    {
      return dataValue != null ? dataValue.Replace(" ", "_").Replace("(", string.Empty).Replace(")", string.Empty) : string.Empty;
    }

    /// <summary>
    /// Analyzes the given Excel data by columns and automatically detects the table columns data types.
    /// </summary>
    /// <param name="excelData">Two-dimensional array containing the Excel data used to fill the table.</param>
    /// <param name="emptyColumnsToVarchar">Flag indicating if the data type for columns with no data is automatically set to varchar(255).</param>
    private void DetectTypes(object[,] excelData, bool emptyColumnsToVarchar)
    {
      int rowsCount = excelData.GetUpperBound(0);
      int colsCount = excelData.GetUpperBound(1);
      int colAdjustIdx = AddPrimaryKeyColumn ? 0 : 1;

      for (int dataColPos = 1; dataColPos <= colsCount; dataColPos++)
      {
        string proposedType = string.Empty;
        string strippedType = string.Empty;
        int leftParensIndex;
        List<string> typesListFor1stAndRest = new List<string>(2);
        List<string> typesListFrom2ndRow = new List<string>(rowsCount - 1);
        int[] varCharLengths1stRow = { 0, 0 }; // 0 - All rows original datatype varcharmaxlen, 1 - All rows Varchar forced datatype maxlen
        int[] varCharMaxLen = { 0, 0 };        // 0 - All rows original datatype varcharmaxlen, 1 - All rows Varchar forced datatype maxlen
        int[] decimalMaxLen1stRow = { 0, 0 };  // 0 - Integral part max length, 1 - decimal part max length
        int[] decimalMaxLen = { 0, 0 };        // 0 - Integral part max length, 1 - decimal part max length

        MySqlDataColumn col = GetColumnAtIndex(dataColPos - colAdjustIdx);
        if (!col.IsEmpty)
        {
          for (int rowPos = 1; rowPos <= rowsCount; rowPos++)
          {
            strippedType = string.Empty;
            object valueFromArray = excelData[rowPos, dataColPos];
            if (valueFromArray == null)
            {
              continue;
            }

            // Treat always as a Varchar value first in case all rows do not have a consistent datatype just to see the varchar len calculated by GetMySQLExportDataType
            string valueAsString = valueFromArray.ToString();
            bool valueOverflow;
            proposedType = DataTypeUtilities.GetMySqlExportDataType(valueAsString, out valueOverflow);
            if (proposedType == "Bool")
            {
              proposedType = "Varchar(5)";
            }
            else if (proposedType.StartsWith("Date"))
            {
              proposedType = string.Format("Varchar({0})", valueAsString.Length);
            }

            int varCharValueLength;
            if (proposedType != "Text")
            {
              leftParensIndex = proposedType.IndexOf("(", StringComparison.Ordinal);
              varCharValueLength = AddBufferToVarchar ? int.Parse(proposedType.Substring(leftParensIndex + 1, proposedType.Length - leftParensIndex - 2)) : valueAsString.Length;
              varCharMaxLen[1] = Math.Max(varCharValueLength, varCharMaxLen[1]);
            }

            // Normal datatype detection
            proposedType = DataTypeUtilities.GetMySqlExportDataType(valueFromArray, out valueOverflow);
            leftParensIndex = proposedType.IndexOf("(", StringComparison.Ordinal);
            strippedType = leftParensIndex < 0 ? proposedType : proposedType.Substring(0, leftParensIndex);
            switch (strippedType)
            {
              case "Date":
              case "Datetime":
                bool zeroDate = valueAsString.StartsWith("0000-00-00") || valueAsString.StartsWith("00-00-00");
                if (zeroDate)
                {
                  break;
                }

                DateTime dtValue = (DateTime)valueFromArray;
                Rows[rowPos - 1][dataColPos - colAdjustIdx] = dtValue.ToString(DataTypeUtilities.MYSQL_DATE_FORMAT);
                break;

              case "Varchar":
                varCharValueLength = AddBufferToVarchar ? int.Parse(proposedType.Substring(leftParensIndex + 1, proposedType.Length - leftParensIndex - 2)) : valueAsString.Length;
                varCharMaxLen[0] = Math.Max(varCharValueLength, varCharMaxLen[0]);
                break;

              case "Decimal":
                int commaPos = proposedType.IndexOf(",", StringComparison.Ordinal);
                decimalMaxLen[0] = Math.Max(int.Parse(proposedType.Substring(leftParensIndex + 1, commaPos - leftParensIndex - 1)), decimalMaxLen[0]);
                decimalMaxLen[1] = Math.Max(int.Parse(proposedType.Substring(commaPos + 1, proposedType.Length - commaPos - 2)), decimalMaxLen[1]);
                break;
            }

            if (rowPos == 1)
            {
              typesListFor1stAndRest.Add(strippedType);
              varCharLengths1stRow[0] = varCharMaxLen[0];
              varCharMaxLen[0] = 0;
              varCharLengths1stRow[1] = varCharMaxLen[1];
              varCharMaxLen[1] = 0;
              decimalMaxLen1stRow[0] = decimalMaxLen[0];
              decimalMaxLen[0] = 0;
              decimalMaxLen1stRow[1] = decimalMaxLen[1];
              decimalMaxLen[1] = 0;
            }
            else
            {
              typesListFrom2ndRow.Add(strippedType);
            }
          }

          // Get the consistent DataType for all rows except first one.
          proposedType = DataTypeUtilities.GetConsistentDataTypeOnAllRows(strippedType, typesListFrom2ndRow, decimalMaxLen, varCharMaxLen);
        }

        if (emptyColumnsToVarchar && string.IsNullOrEmpty(proposedType))
        {
          proposedType = "Varchar(255)";
          strippedType = "Varchar";
          typesListFor1stAndRest.Add("Varchar");
          varCharMaxLen[0] = 255;
          varCharMaxLen[1] = 255;
        }

        col.RowsFrom2ndDataType = proposedType;
        if (proposedType == "Datetime")
        {
          int pos = dataColPos;
          foreach (DataRow dr in Rows.Cast<DataRow>().Where(dr => dr[pos - colAdjustIdx].ToString() == "0"))
          {
            dr[dataColPos - colAdjustIdx] = DataTypeUtilities.MYSQL_EMPTY_DATE;
          }
        }

        // Get the consistent DataType between first columnInfoRow and the previously computed consistent DataType for the rest of the rows.
        if (typesListFrom2ndRow.Count > 0)
        {
          leftParensIndex = proposedType.IndexOf("(", StringComparison.Ordinal);
          strippedType = leftParensIndex < 0 ? proposedType : proposedType.Substring(0, leftParensIndex);
          typesListFor1stAndRest.Add(strippedType);
        }

        varCharMaxLen[0] = Math.Max(varCharMaxLen[0], varCharLengths1stRow[0]);
        varCharMaxLen[1] = Math.Max(varCharMaxLen[1], varCharLengths1stRow[1]);
        decimalMaxLen[0] = Math.Max(decimalMaxLen[0], decimalMaxLen1stRow[0]);
        decimalMaxLen[1] = Math.Max(decimalMaxLen[1], decimalMaxLen1stRow[1]);
        proposedType = DataTypeUtilities.GetConsistentDataTypeOnAllRows(strippedType, typesListFor1stAndRest, decimalMaxLen, varCharMaxLen);
        col.RowsFrom1stDataType = proposedType;
        col.SetMySqlDataType(_firstRowIsHeaders ? col.RowsFrom2ndDataType : col.RowsFrom1stDataType);
        col.CreateIndex = AutoIndexIntColumns && col.IsInteger;
      }
    }

    /// <summary>
    /// Updates the table's automatically generated primary key's name based on the current table name.
    /// </summary>
    /// <param name="firstSet">Flag indicating if the AutoPK column is a new column and all of its properties must be set.</param>
    private void SetupAutoPkColumn(bool firstSet)
    {
      if (!AddPrimaryKeyColumn || Columns.Count <= 0)
      {
        return;
      }

      MySqlDataColumn autoPKcolumn = GetColumnAtIndex(0);
      string tableIdName = TableName + (TableName.Length > 0 ? "_" : string.Empty) + "id";
      string autoPkName = GetNonDuplicateColumnName(tableIdName);
      autoPKcolumn.SetDisplayName(autoPkName);
      if (!firstSet)
      {
        return;
      }

      autoPKcolumn.PrimaryKey = true;
      autoPKcolumn.AutoPk = true;
      autoPKcolumn.SetMySqlDataType("Integer");
      autoPKcolumn.AutoIncrement = true;
      autoPKcolumn.AllowNull = false;
    }

    /// <summary>
    /// Updates the table's SELECT query based on the current table name.
    /// </summary>
    private void UpdateTableSelectQuery()
    {
      string schemaPiece = !string.IsNullOrEmpty(SchemaName) ? string.Format("`{0}`.", SchemaName) : string.Empty;
      SelectQuery = string.Format("SELECT * FROM {0}`{1}`", schemaPiece, TableNameForSqlQueries);
    }

    /// <summary>
    /// Updates the warnings related to the table name and the select query used to retrieve data based on the <see cref="TableName"/> property's value.
    /// </summary>
    private void UpdateTableNameWarningsAndSelectQuery()
    {
      // Update warning stating the table name cannot be empty
      bool emptyTableName = string.IsNullOrWhiteSpace(TableName);
      bool warningsChanged = UpdateWarnings(emptyTableName, Properties.Resources.TableNameRequiredWarning);
      IsTableNameValid = !emptyTableName;

      // Update warning stating a table with the given name already exists in the database
      if (IsTableNameValid && WbConnection != null)
      {
        warningsChanged = UpdateWarnings(TableExistsInSchema, Properties.Resources.TableNameExistsWarning) || warningsChanged;
        IsTableNameValid = !TableExistsInSchema;
      }

      // Update warning stating the table name cannot be empty
      if (IsTableNameValid)
      {
        bool nonStandardTableName = TableName.Contains(" ") || TableName.Any(char.IsUpper);
        warningsChanged = UpdateWarnings(nonStandardTableName, Properties.Resources.NamesWarning) || warningsChanged;
      }

      // Fire the TableWarningsChanged event.
      if (warningsChanged)
      {
        OnTableWarningsChanged(false);
      }
    }

    /// <summary>
    /// Adds or removes warnings related to this table's creation.
    /// </summary>
    /// <param name="addWarning">true to add a new warning to the corresponding warnings collection, false to remove the given warning.</param>
    /// <param name="warningResourceText">Warning text to display to users.</param>
    /// <param name="autoPkWarning">Flag indicating if the warning is to be added to the collection related to auto-generated primary keys or the table's one.</param>
    /// <returns><c>true</c> if a warning was added or removed, <c>false</c> otherwise.</returns>
    private bool UpdateWarnings(bool addWarning, string warningResourceText, bool autoPkWarning = false)
    {
      bool warningsChanged = false;

      List<string> warningsList = autoPkWarning ? _autoPkWarningTextsList : _tableWarningsTextList;
      if (addWarning)
      {
        // Only add the warning text if it is not empty and not already added to the warnings list
        if (string.IsNullOrEmpty(warningResourceText) || warningsList.Contains(warningResourceText))
        {
          return false;
        }

        warningsList.Add(warningResourceText);
        warningsChanged = true;
      }
      else
      {
        // We do not want to show a warning or we want to remove a warning if warningResourceText != null
        if (!string.IsNullOrEmpty(warningResourceText))
        {
          // Remove the warning and check if there is an stored warning, if so we want to pull it and show it
          warningsChanged = warningsList.Remove(warningResourceText);
        }
      }

      return warningsChanged;
    }

    /// <summary>
    /// Updates the column names and the automatically generated primary key column values depending on the value of the <see cref="_firstRowIsHeaders"/> field.
    /// </summary>
    private void UseFirstRowAsHeaders()
    {
      if (Rows.Count == 0)
      {
        return;
      }

      DataRow row = Rows[0];
      if (!_changedColumnNamesWithFirstRowOfData && AddPrimaryKeyColumn && Columns.Count > 0)
      {
        MySqlDataColumn autoPkCol = GetColumnAtIndex(0);
        string autoPkName = autoPkCol.DisplayName;
        autoPkName = row.ItemArray.Skip(1).Select(obj => obj.ToString()).ToList().GetNonDuplicateText(autoPkName);
        autoPkCol.SetDisplayName(autoPkName);
      }

      int startCol = AddPrimaryKeyColumn ? 1 : 0;
      for (int i = startCol; i < Columns.Count; i++)
      {
        MySqlDataColumn col = GetColumnAtIndex(i);
        col.SetDisplayName(_firstRowIsHeaders ? DataToColName(row[i].ToString()) : col.ColumnName);
        col.SetMySqlDataType(_firstRowIsHeaders ? col.RowsFrom2ndDataType : col.RowsFrom1stDataType);
        col.CreateIndex = AutoIndexIntColumns && col.IsInteger;
      }

      AdjustAutoPkValues();
      _changedColumnNamesWithFirstRowOfData = true;
    }
  }

  /// <summary>
  /// Event arguments for the TableWarningsChanged event.
  /// </summary>
  public class TableWarningsChangedArgs : EventArgs
  {
    /// <summary>
    /// Initializes a new instance of the <see cref="TableWarningsChangedArgs"/> class.
    /// </summary>
    /// <param name="column">The <see cref="MySqlDataColumn"/> object that contains changes in its warning texts.</param>
    public TableWarningsChangedArgs(MySqlDataColumn column)
    {
      CurrentWarning = column.CurrentColumnWarningText;
      WarningsType = TableWarningsType.ColumnWarnings;
      WarningsQuantity = column.WarningsQuantity;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="TableWarningsChangedArgs"/> class.
    /// </summary>
    /// <param name="table">The <see cref="MySqlDataTable"/> object that contains changes in its warning texts.</param>
    /// <param name="autoPkWarning">Flag indicating if the warning is related to the auto-generated primary key or to the table.</param>
    public TableWarningsChangedArgs(MySqlDataTable table, bool autoPkWarning)
    {
      CurrentWarning = autoPkWarning ? table.CurrentAutoPkWarningText : table.CurrentTableWarningText;
      WarningsType = autoPkWarning ? TableWarningsType.AutoPrimaryKeyWarnings : TableWarningsType.TableNameWarnings;
      WarningsQuantity = autoPkWarning ? table.AutoPkWarningsQuantity : table.TableWarningsQuantity;
    }

    /// <summary>
    /// Specifies identifiers to indicate the type of warnings that were updated.
    /// </summary>
    public enum TableWarningsType
    {
      /// <summary>
      /// Warnings belong to a table column.
      /// </summary>
      ColumnWarnings,

      /// <summary>
      /// Warnings belong to the table's auto-generated primary key.
      /// </summary>
      AutoPrimaryKeyWarnings,

      /// <summary>
      /// Warnings belong to the table name.
      /// </summary>
      TableNameWarnings
    }

    /// <summary>
    /// Gets the last warning text in the warnings collection.
    /// </summary>
    public string CurrentWarning { get; private set; }

    /// <summary>
    /// Gets the number of warnings currenlty in the warnings collection.
    /// </summary>
    public int WarningsQuantity { get; private set; }

    /// <summary>
    /// Gets the type of warnings that were updated.
    /// </summary>
    public TableWarningsType WarningsType { get; set; }
  }
}