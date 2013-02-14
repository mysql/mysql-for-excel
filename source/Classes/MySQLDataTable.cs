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
  using Excel = Microsoft.Office.Interop.Excel;

  /// <summary>
  /// Represents an in-memory table for a corresponding MySQL database table.
  /// </summary>
  public class MySQLDataTable : DataTable
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
    /// Flag indicating if the first row in the Excel region to be exported contains the column names of the MySQL table that will be created.
    /// </summary>
    private bool _firstRowIsHeaders;

    /// <summary>
    /// Contains the value of the max_allowed_packet system variable of the MySQL Server currently connected to.
    /// </summary>
    private ulong _mysqlMaxAllowedPacket = 0;

    /// <summary>
    /// Flag indicating if the first column in the Excel region to be exported will be used to create the MySQL table's primary key.
    /// </summary>
    private bool _useFirstColumnAsPK;

    #endregion Fields

    /// <summary>
    /// Initializes a new instance of the <see cref="MySQLDataTable"/> class.
    /// This constructor is meant to be used by the <see cref="ExportDataForm"/> class.
    /// </summary>
    /// <param name="schemaName">Name of the schema where this table will be created.</param>
    /// <param name="proposedTableName">Proposed name for this new table.</param>
    /// <param name="addPrimaryKeyCol">Flag indicating if an auto-generated primary key column will be added as the first column in the table.</param>
    /// <param name="useFormattedValues">Flag indicating if the Excel excelData used to populate this table is formatted (numbers, dates, text) or not (numbers and text).</param>
    /// <param name="removeEmptyColumns">Flag indicating if columns with no excelData will be skipped for export to a new table so they are not created.</param>
    public MySQLDataTable(string schemaName, string proposedTableName, bool addPrimaryKeyCol, bool useFormattedValues, bool removeEmptyColumns)
      : this(schemaName, proposedTableName)
    {
      AddPrimaryKeyColumn = addPrimaryKeyCol;
      RemoveEmptyColumns = removeEmptyColumns;
      IsFormatted = useFormattedValues;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="MySQLDataTable"/> class.
    /// This constructor is meant to be used by the <see cref="AppendDataForm"/> class to fetch schema information from the corresponding MySQL table before copying its excelData.
    /// </summary>
    /// <param name="tableName">Name of the table.</param>
    /// <param name="fetchColumnsSchemaInfo">Flag indicating if the schema information from the corresponding MySQL table is fetched and recreated before any excelData is added to the table.</param>
    /// <param name="datesAsMySQLDates">Flag indicating if the dates are stored in the table as <see cref="System.DateTime"/> or <see cref="MySql.Data.Types.MySqlDateTime"/> objects.</param>
    /// <param name="wbConnection">Connection to a MySQL server instance selected by users.</param>
    public MySQLDataTable(string tableName, bool fetchColumnsSchemaInfo, bool datesAsMySQLDates, MySqlWorkbenchConnection wbConnection)
      : this(wbConnection.Schema, tableName)
    {
      if (fetchColumnsSchemaInfo)
      {
        CreateTableSchema(tableName, datesAsMySQLDates, wbConnection);
      }

      _mysqlMaxAllowedPacket = MySQLDataUtilities.GetMySQLServerMaxAllowedPacket(wbConnection);
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="MySQLDataTable"/> class.
    /// This constructor is meant to be used by the <see cref="EditDataForm"/> class to copy the contents of a table imported to Excel for edition.
    /// </summary>
    /// <param name="tableName">Name of the table.</param>
    /// <param name="filledTable"><see cref="DataTable"/> object containing imported excelData from the MySQL table to be edited.</param>
    /// <param name="wbConnection">Connection to a MySQL server instance selected by users.</param>
    public MySQLDataTable(string tableName, DataTable filledTable, MySqlWorkbenchConnection wbConnection)
      : this(tableName, true, true, wbConnection)
    {
      CopyTableData(filledTable);
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="MySQLDataTable"/> class.
    /// </summary>
    /// <param name="schemaName">Name of the schema where this table exists.</param>
    /// <param name="tableName">Name of the table.</param>
    public MySQLDataTable(string schemaName, string tableName)
      : this()
    {
      if (!string.IsNullOrEmpty(schemaName))
      {
        SchemaName = schemaName;
      }

      if (!string.IsNullOrEmpty(tableName))
      {
        TableName = tableName;
      }

      SelectQuery = string.Format("SELECT * FROM `{0}`.`{1}`", schemaName, tableName.Replace("`", "``"));
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="MySQLDataTable"/> class.
    /// </summary>
    public MySQLDataTable()
    {
      SchemaName = string.Empty;
      SelectQuery = string.Format("SELECT * FROM `{0}`", TableName.Replace("`", "``"));
      AddPrimaryKeyColumn = false;
    }

    #region Properties

    /// <summary>
    /// Gets a value indicating whether an auto-generated primary key column will be added as the first column in the table.
    /// </summary>
    public bool AddPrimaryKeyColumn { get; private set; }

    /// <summary>
    /// Gets a value indicating whether the first column contains numeric whole numbers.
    /// </summary>
    public bool FirstColumnContainsIntegers
    {
      get
      {
        bool containsIntegers = false;
        int res = 0;
        if (Columns.Count > 1)
        {
          containsIntegers = (Columns[1] as MySQLDataColumn).MySQLDataType.ToLowerInvariant() == "integer";
        }

        if (!containsIntegers)
        {
          int rowsToAnalyzeCount = Math.Min(Rows.Count, 50);
          int startingRow = _firstRowIsHeaders ? 1 : 0;
          containsIntegers = startingRow < rowsToAnalyzeCount;
          for (int rowIdx = startingRow; rowIdx < rowsToAnalyzeCount; rowIdx++)
          {
            containsIntegers = containsIntegers && int.TryParse(Rows[rowIdx][1].ToString(), out res);
          }
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
    /// Gets the number of deleted rows meaning the number of pending DELETE operations in an Edit Data operation.
    /// </summary>
    public int DeletingOperations
    {
      get
      {
        DataTable changesDT = GetChanges(DataRowState.Deleted);
        return changesDT != null ? changesDT.Rows.Count : 0;
      }
    }

    /// <summary>
    /// Gets the number of added rows meaning the number of pending INSERT operations in an Edit Data operation.
    /// </summary>
    public int InsertingOperations
    {
      get
      {
        DataTable changesDT = GetChanges(DataRowState.Added);
        return changesDT != null ? changesDT.Rows.Count : 0;
      }
    }

    /// <summary>
    /// Gets the number of changed rows meaning the number of pending UPDATE operations in an Edit Data operation.
    /// </summary>
    public int UpdatingOperations
    {
      get
      {
        DataTable changesDT = GetChanges(DataRowState.Modified);
        return changesDT != null ? changesDT.Rows.Count : 0;
      }
    }

    /// <summary>
    /// Gets a value indicating whether the Excel excelData used to populate this table is formatted (numbers, dates, text) or not (numbers and text).
    /// </summary>
    public bool IsFormatted { get; private set; }

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
          if (!string.IsNullOrEmpty((Columns[colIdx] as MySQLDataColumn).MappedDataColName))
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
    public int NumberOfPK
    {
      get
      {
        return Columns.OfType<MySQLDataColumn>().Skip(1).Count(col => col.PrimaryKey && !col.ExcludeColumn);
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
    /// Gets or sets the name of the <see cref="MySQLDataTable"/>.
    /// </summary>
    public new string TableName
    {
      get
      {
        return base.TableName;
      }

      set
      {
        base.TableName = value;
        string schemaPiece = !string.IsNullOrEmpty(SchemaName) ? string.Format("`{0}`.", SchemaName) : string.Empty;
        SelectQuery = string.Format("SELECT * FROM {0}`{1}`", schemaPiece, base.TableName.Replace("`", "``"));
      }
    }

    /// <summary>
    /// Gets or sets a value indicating whether the first column of the table will be or is used in the table's primary key.
    /// </summary>
    public bool UseFirstColumnAsPK
    {
      get
      {
        return _useFirstColumnAsPK;
      }

      set
      {
        _useFirstColumnAsPK = value;
        if (AddPrimaryKeyColumn)
        {
          for (int i = 1; i < Columns.Count && value; i++)
          {
            (Columns[i] as MySQLDataColumn).PrimaryKey = false;
          }
        }
      }
    }

    #endregion Properties

    /// <summary>
    /// Gets a column name avoiding duplicates by adding a numeric suffix in case it already exists in the table.
    /// </summary>
    /// <param name="proposedName">Proposed column name.</param>
    /// <returns>Unique column name.</returns>
    public string GetNonDuplicateColumnName(string proposedName)
    {
      if (string.IsNullOrEmpty(proposedName) || Columns == null || Columns.Count == 0)
      {
        return proposedName;
      }

      proposedName = proposedName.Trim();
      string nonDupName = proposedName;
      int colIdx = 2;
      while (Columns.OfType<MySQLDataColumn>().Count(col => col.DisplayName == nonDupName) > 0)
      {
        nonDupName = proposedName + colIdx++;
      }

      return nonDupName;
    }

    /// <summary>
    /// Creates data rows, fills them with the given Excel data and sets column properties automatically given user options.
    /// </summary>
    /// <param name="dataRange">Excel data range containing the data to fill the table.</param>
    /// <param name="recreateColumnsFromData">Flag indicating if any existing columns in the table must be dropped and re-created based on the given data range.</param>
    /// <param name="detectTypes">Flag indicating if the columns data types must be automatically detected based on columns data.</param>
    /// <param name="addBufferToVarchar">Flag indicating if a buffer is added for columns whose data type was automatically detected as varchar.</param>
    /// <param name="createIndexForIntColumns">Flag indicating if the CreateIndex property of <see cref="MySQLDataColumn"/> columns is automatically set if their data type is Integer.</param>
    /// <param name="allowEmptyNonIdxCols">Flag indicating if the AllowNull property of <see cref="MySQLDataColumn"/> columns is automatically set if its CreateIndex property has a false value.</param>
    /// <param name="emptyColumnsToVarchar">Flag indicating if the data type for columns with no data is automatically set to varchar(255).</param>
    public void SetData(Excel.Range dataRange, bool recreateColumnsFromData, bool detectTypes, bool addBufferToVarchar, bool createIndexForIntColumns, bool allowEmptyNonIdxCols, bool emptyColumnsToVarchar)
    {
      object[,] excelData;
      Clear();

      //// We have to treat a single cell specially.  It doesn't come in as an array but as a single value
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
      List<string> colsToDelete = new List<string>(numCols);

      //// Create a list of boolean values that state if each column has any data or none.
      columnsHaveAnyDataList.Add(true);
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

      //// Drop all columns and re-create them or create them if none have been created so far.
      if (recreateColumnsFromData || Columns.Count == 0)
      {
        if (Columns.Count > 0)
        {
          Columns.Clear();
        }

        CreateColumns(numCols);
      }

      //// Create excelData rows and fill them with the Excel data.
      int pkRowValueAdjust = _firstRowIsHeaders ? 1 : 0;
      for (int row = 1; row <= numRows; row++)
      {
        bool rowHasAnyData = false;
        DataRow dataRow = NewRow();
        dataRow[0] = row - pkRowValueAdjust;
        for (int col = 1; col <= numCols; col++)
        {
          int adjColIdx = col - colAdjustIdx;
          MySQLDataColumn column = Columns[adjColIdx] as MySQLDataColumn;
          if (row == 1 && !columnsHaveAnyDataList[col])
          {
            column.ExcludeColumn = true;
            colsToDelete.Add(column.ColumnName);
          }

          rowHasAnyData = rowHasAnyData || excelData[row, col] != null;
          dataRow[adjColIdx] = excelData[row, col] != null && excelData[row, col].Equals(0.0) && column.IsDate ? DataTypeUtilities.EMPTY_DATE : dataRow[adjColIdx] = excelData[row, col];
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

      //// Automatically detect the excelData type for columns based on their data.
      if (detectTypes)
      {
        DetectTypes(excelData, addBufferToVarchar, createIndexForIntColumns, emptyColumnsToVarchar);
      }

      //// Remove from the Columns collection the columns without data if the Remove Empty Columns option is true.
      if (RemoveEmptyColumns)
      {
        foreach (string colName in colsToDelete)
        {
          Columns.Remove(Columns[colName]);
        }
      }

      //// Flag columns as allowing nulls if the option to Allow Empty columns is true, valid only for non-index columns.
      if (allowEmptyNonIdxCols)
      {
        foreach (MySQLDataColumn mysqlCol in Columns)
        {
          mysqlCol.AllowNull = !mysqlCol.CreateIndex;
        }
      }
    }

    /// <summary>
    /// Creates a new <see cref="MySQLDataTable"/> object with its schema cloned from this table but no data.
    /// </summary>
    /// <returns>Cloned <see cref="MySQLDataTable"/> object.</returns>
    public MySQLDataTable CloneSchema()
    {
      MySQLDataTable clonedTable = new MySQLDataTable(this.SchemaName, this.TableName, this.AddPrimaryKeyColumn, this.IsFormatted, this.RemoveEmptyColumns);
      clonedTable.UseFirstColumnAsPK = UseFirstColumnAsPK;
      clonedTable.IsFormatted = IsFormatted;
      clonedTable.FirstRowIsHeaders = FirstRowIsHeaders;

      foreach (MySQLDataColumn column in Columns)
      {
        MySQLDataColumn clonedColumn = column.CloneSchema();
        clonedTable.Columns.Add(clonedColumn);
      }

      return clonedTable;
    }

    /// <summary>
    /// Synchronizes the column properties of this table with the column properties of the given <see cref="MySQLDataTable"/> table.
    /// </summary>
    /// <param name="syncFromTable">A <see cref="MySQLDataTable"/> object from which columns will be synchronized.</param>
    public void SyncSchema(MySQLDataTable syncFromTable)
    {
      if (syncFromTable.Columns.Count != Columns.Count)
      {
        return;
      }

      for (int colIdx = 0; colIdx < Columns.Count; colIdx++)
      {
        MySQLDataColumn thisColumn = Columns[colIdx] as MySQLDataColumn;
        MySQLDataColumn syncFromColumn = syncFromTable.Columns[colIdx] as MySQLDataColumn;

        thisColumn.DisplayName = syncFromColumn.DisplayName;
        thisColumn.MySQLDataType = syncFromColumn.MySQLDataType;
        thisColumn.PrimaryKey = syncFromColumn.PrimaryKey;
        thisColumn.AllowNull = syncFromColumn.AllowNull;
        thisColumn.UniqueKey = syncFromColumn.UniqueKey;
        thisColumn.CreateIndex = syncFromColumn.CreateIndex;
        thisColumn.ExcludeColumn = syncFromColumn.ExcludeColumn;
      }
    }

    /// <summary>
    /// Checks if a column with the given column name is being used in the table's primary key.
    /// </summary>
    /// <param name="columnName">Column name.</param>
    /// <param name="caseSensitive">Flag indicating if a case sensitive comparison against the column name should be done.</param>
    /// <returns><see cref="true"/> if the given column is used in the table's primary key, <see cref="false"/> otherwise.</returns>
    public bool ColumnIsPrimaryKey(string columnName, bool caseSensitive)
    {
      if (!caseSensitive)
      {
        columnName = columnName.ToLowerInvariant();
      }

      foreach (MySQLDataColumn col in Columns)
      {
        string columnDisplayName = caseSensitive ? col.DisplayName : col.DisplayName.ToLowerInvariant();
        if (columnDisplayName == columnName && col.PrimaryKey)
        {
          return true;
        }
      }

      return false;
    }

    /// <summary>
    /// Checks if a column with the given column name is being used in the table's primary key doing a case sensitive comparison of its name.
    /// </summary>
    /// <param name="columnName">Column name.</param>
    /// <returns><see cref="true"/> if the given column is used in the table's primary key, <see cref="false"/> otherwise.</returns>
    public bool ColumnIsPrimaryKey(string columnName)
    {
      return ColumnIsPrimaryKey(columnName, true);
    }

    /// <summary>
    /// Gets the ordinal index within the columns collection of the column with the given name.
    /// </summary>
    /// <param name="columnName">Column name.</param>
    /// <param name="useDisplayName">Flag indicating whether the <see cref="MySQLDataColumn.DisplayName"/> or the <see cref="MySQLDataColumn.ColumnName"/> property must be used for the name comparison.</param>
    /// <param name="caseSensitive">Flag indicating if a case sensitive comparison against the column name should be done.</param>
    /// <returns>The ordinal index within the columns collection.</returns>
    public int GetColumnIndex(string columnName, bool useDisplayName, bool caseSensitive)
    {
      int index = -1;

      if (!caseSensitive)
      {
        columnName = columnName.ToLowerInvariant();
      }

      foreach (MySQLDataColumn col in Columns)
      {
        string localColumnName = useDisplayName ? col.DisplayName : col.ColumnName;
        if (!caseSensitive)
        {
          localColumnName = localColumnName.ToLowerInvariant();
        }

        if (localColumnName == columnName)
        {
          index = col.Ordinal;
          break;
        }
      }

      return index;
    }

    /// <summary>
    /// Gets the ordinal index within the columns collection of the column with the given name doing a case sensitive comparison.
    /// </summary>
    /// <param name="columnName">Column name.</param>
    /// <param name="useDisplayName">Flag indicating whether the <see cref="MySQLDataColumn.DisplayName"/> or the <see cref="DataColumn.ColumnName"/> property must be used for the name comparison.</param>
    /// <returns>The ordinal index within the columns collection.</returns>
    public int GetColumnIndex(string columnName, bool useDisplayName)
    {
      return GetColumnIndex(columnName, useDisplayName, true);
    }

    /// <summary>
    /// Gets the <see cref="MySQLDataColumn"/> object at the given position within the columns collection.
    /// </summary>
    /// <param name="index">Ordinal index within the columns collection</param>
    /// <returns>A <see cref="MySQLDataColumn"/> object.</returns>
    public MySQLDataColumn GetColumnAtIndex(int index)
    {
      MySQLDataColumn col = null;
      if (index >= 0 && index < Columns.Count)
      {
        col = Columns[index] as MySQLDataColumn;
      }

      return col;
    }

    /// <summary>
    /// Gets an array with the names of all columns in this table.
    /// </summary>
    /// <param name="useDisplayName">Flag indicating whether the <see cref="MySQLDataColumn.DisplayName"/> or the <see cref="DataColumn.ColumnName"/> property must be used for the name comparison.</param>
    /// <returns>A string array containing all column names in this table.</returns>
    public string[] GetColumnNamesArray(bool useDisplayName)
    {
      string[] retArray = null;

      if (Columns.Count > 0)
      {
        retArray = new string[Columns.Count];
        for (int i = 0; i < Columns.Count; i++)
        {
          retArray[i] = useDisplayName ? (Columns[i] as MySQLDataColumn).DisplayName : Columns[i].ColumnName;
        }
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
    public string GetCreateSQL(bool formatNewLinesAndTabs)
    {
      StringBuilder sql = new StringBuilder();
      string nl = formatNewLinesAndTabs ? Environment.NewLine : " ";
      string nlt = formatNewLinesAndTabs ? Environment.NewLine + "\t" : " ";

      sql.AppendFormat("CREATE TABLE `{0}`.`{1}`{2}(", SchemaName, TableName, nl);

      string delimiter = nlt;
      int skipNum = AddPrimaryKeyColumn ? (_useFirstColumnAsPK ? 0 : 1) : 0;
      foreach (MySQLDataColumn col in Columns.OfType<MySQLDataColumn>().Skip(skipNum).Where(c => !c.ExcludeColumn))
      {
        sql.AppendFormat("{0}{1}", delimiter, col.GetSQL());
        delimiter = "," + nlt;
      }

      if (NumberOfPK > 1)
      {
        string pkDelimiter = string.Empty;
        sql.AppendFormat("{0}PRIMARY KEY (", delimiter);
        foreach (MySQLDataColumn col in Columns.OfType<MySQLDataColumn>().Skip(1).Where(c => c.PrimaryKey))
        {
          sql.AppendFormat("{0}`{1}`", pkDelimiter, col.DisplayName.Replace("`", "``"));
          pkDelimiter = ",";
        }

        sql.Append(")");
      }

      foreach (MySQLDataColumn col in Columns.OfType<MySQLDataColumn>().Where(c => !(c.AutoPK || c.PrimaryKey || c.UniqueKey || c.ExcludeColumn || !c.CreateIndex)))
      {
        sql.AppendFormat("{0}INDEX `{1}_idx` (`{1}`)", delimiter, col.DisplayName.Replace("`", "``"));
      }

      sql.Append(nl);
      sql.Append(")");
      return sql.ToString();
    }

    /// <summary>
    /// Creates a new table in the database based on this table's schema information.
    /// </summary>
    /// <param name="wbConnection">Connection to a MySQL server instance selected by users.</param>
    /// <param name="exception">Exception thrown back (if any) when trying to create the table in the database.</param>
    /// <param name="sqlQuery">The SQL query to create a new table in the database based on this table's schema information.</param>
    /// <returns><see cref="DataTable"/> object containing warnings thrown by the table's creation, null if no warnings were generated.</returns>
    public DataTable CreateTable(MySqlWorkbenchConnection wbConnection, out Exception exception, out string sqlQuery)
    {
      DataSet warningsDS = null;
      string connectionString = MySQLDataUtilities.GetConnectionString(wbConnection);
      exception = null;
      sqlQuery = GetCreateSQL(true);

      try
      {
        using (MySqlConnection conn = new MySqlConnection(connectionString))
        {
          conn.Open();
          MySqlCommand cmd = new MySqlCommand(sqlQuery, conn);
          cmd.ExecuteNonQuery();
          warningsDS = MySqlHelper.ExecuteDataset(conn, "SHOW WARNINGS");
        }
      }
      catch (MySqlException ex)
      {
        exception = ex;
        MiscUtilities.WriteAppErrorToLog(ex);
      }

      return warningsDS != null && warningsDS.Tables.Count > 0 ? warningsDS.Tables[0] : null;
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
    public string GetInsertSQL(int startRow, int limit, bool formatNewLinesAndTabs, out int nextRow, out int insertingRowsCount)
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
      int rowIdx = 0;
      int colIdx = 0;
      int startingColNum = AddPrimaryKeyColumn ? (_useFirstColumnAsPK ? 0 : 1) : 0;
      List<string> insertColumnNames = new List<string>(Columns.Count);

      string rowsSeparator = string.Empty;
      string colsSeparator = string.Empty;
      queryString.AppendFormat(
        "INSERT INTO `{0}`.`{1}`{2}(",
        SchemaName,
        TableName.Replace("`", "``"),
        nl);

      //// Loop columns to assemble the piece of the query that includes the column names that we will insert data into.
      for (colIdx = startingColNum; colIdx < Columns.Count; colIdx++)
      {
        MySQLDataColumn column = Columns[colIdx] as MySQLDataColumn;
        if (column.ExcludeColumn)
        {
          continue;
        }

        queryString.AppendFormat(
          "{0}`{1}`",
          colsSeparator,
          column.DisplayName.Replace("`", "``"));
        colsSeparator = ",";
        insertColumnNames.Add(column.ColumnName);
      }

      queryString.AppendFormat("){0}VALUES{0}", nl);
      bool insertingValueIsNull = false;
      int absRowIdx = 0;
      string valueToDB = string.Empty;
      StringBuilder singleRowValuesBuilder = new StringBuilder();
      string singleRowValuesString = string.Empty;
      if (maxByteCount > 0)
      {
        queryStringByteCount = (ulong)ASCIIEncoding.ASCII.GetByteCount(queryString.ToString());
      }

      //// Loop all rows in this table to include the values for insertion in the query.
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
        else
        {
          absRowIdx++;
        }

        //// Within the current row, loop all columns to extract each value and append it to the query string.
        DataRow dr = Rows[rowIdx];
        singleRowValuesBuilder.Clear();
        singleRowValuesString = string.Empty;
        singleRowValuesBuilder.AppendFormat("{0}(", rowsSeparator);
        colsSeparator = string.Empty;
        foreach (string insertingColName in insertColumnNames)
        {
          MySQLDataColumn column = Columns[insertingColName] as MySQLDataColumn;
          valueToDB = DataTypeUtilities.GetStringValueForColumn(dr[insertingColName], column, true, out insertingValueIsNull);
          singleRowValuesBuilder.AppendFormat(
            "{0}{1}{2}{1}",
            colsSeparator,
            column.ColumnsRequireQuotes && !insertingValueIsNull ? "'" : string.Empty,
            valueToDB);
          colsSeparator = ",";
        }

        //// Close the current row values piece of the query and check if we have not exceeded the maximum packet size allowed by the server,
        ////  otherwise we return the query string as is and return the last row number that was processed so another INSERT INTO query is
        ////  assembled starting from the row we left on.
        singleRowValuesBuilder.Append(")");
        singleRowValuesString = singleRowValuesBuilder.ToString();
        if (maxByteCount > 0)
        {
          ulong singleValueRowQueryByteCount = (ulong)ASCIIEncoding.ASCII.GetByteCount(singleRowValuesString);
          if (queryStringByteCount + singleValueRowQueryByteCount > maxByteCount)
          {
            nextRow = rowIdx;
            break;
          }

          queryStringByteCount += singleValueRowQueryByteCount;
        }

        //// Add a , separator for the collection of values in the INSERT QUERY.
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
    public string GetInsertSQL(int limit, bool formatNewLinesAndTabs)
    {
      int nextRow = -1;
      int insertingRowsCount = -1;
      return GetInsertSQL(0, limit, formatNewLinesAndTabs, out nextRow, out insertingRowsCount);
    }

    /// <summary>
    /// Inserts data contained in this table into the corresponding database table with the same name.
    /// </summary>
    /// <param name="wbConnection">Connection to a MySQL server instance selected by users.</param>
    /// <param name="exception">Exception thrown back (if any) when trying to insert data into the database table.</param>
    /// <param name="sqlQuery">The SQL query to insert data in this table into the database table.</param>
    /// <param name="insertingRows">Number of rows to be inserted into the database with this query.</param>
    /// <param name="insertedRows">Number of rows actually inserted into the database.</param>
    /// <returns><see cref="DataTable"/> object containing warnings thrown by the data insertion, null if no warnings were generated.</returns>
    public DataTable InsertDataWithManualQuery(MySqlWorkbenchConnection wbConnection, out Exception exception, out string sqlQuery, out int insertingRows, out int insertedRows)
    {
      DataSet warningsDS = null;
      insertedRows = 0;
      insertingRows = 0;
      exception = null;
      MySqlTransaction transaction = null;
      string chunkQuery = sqlQuery = string.Empty;

      string connectionString = MySQLDataUtilities.GetConnectionString(wbConnection);
      using (MySqlConnection conn = new MySqlConnection(connectionString))
      {
        try
        {
          conn.Open();
          if (_mysqlMaxAllowedPacket == 0)
          {
            _mysqlMaxAllowedPacket = MySQLDataUtilities.GetMySQLServerMaxAllowedPacket(conn);
          }

          transaction = conn.BeginTransaction();
          MySqlCommand cmd = new MySqlCommand(string.Empty, conn, transaction);
          int nextRow = 0;
          while (nextRow >= 0)
          {
            int insertingRowsCount = 0;
            chunkQuery = GetInsertSQL(nextRow, -1, true, out nextRow, out insertingRowsCount);
            cmd.CommandText = chunkQuery;
            sqlQuery += chunkQuery;
            insertedRows += cmd.ExecuteNonQuery();
            insertingRows += insertingRowsCount;
          }

          transaction.Commit();
          warningsDS = MySqlHelper.ExecuteDataset(conn, "SHOW WARNINGS");
        }
        catch (MySqlException mysqlEx)
        {
          if (transaction != null)
          {
            transaction.Rollback();
          }

          exception = mysqlEx;
          MiscUtilities.WriteAppErrorToLog(mysqlEx);
        }
        catch (Exception ex)
        {
          if (transaction != null)
          {
            transaction.Rollback();
          }

          exception = ex;
          MiscUtilities.WriteAppErrorToLog(ex);
        }
      }

      return warningsDS != null && warningsDS.Tables.Count > 0 ? warningsDS.Tables[0] : null;
    }

    /// <summary>
    /// Gets a collection of column names with data changes within the given <see cref="DataRow"/> object.
    /// </summary>
    /// <param name="changesRow"><see cref="DataRow"/> object to inspect for changes.</param>
    /// <returns>List of column names with data changes.</returns>
    public List<string> GetChangedColumns(DataRow changesRow)
    {
      List<string> changedColNamesList = null;

      if (changesRow != null)
      {
        changedColNamesList = new List<string>(changesRow.Table.Columns.Count);
        foreach (DataColumn col in changesRow.Table.Columns)
        {
          if (!changedColNamesList.Contains(col.ColumnName) && !changesRow[col, DataRowVersion.Original].Equals(changesRow[col, DataRowVersion.Current]))
          {
            changedColNamesList.Add(col.ColumnName);
          }
        }
      }

      return changedColNamesList;
    }

    /// <summary>
    /// Creates a SQL query to insert rows taken from the given <see cref="MySQLDataTable"/> source table into mapped columns in this table.
    /// </summary>
    /// <param name="startRow">Values to be inserted are taken from this row number forward.</param>
    /// <param name="limit">Maximum number of rows in the table to be inserted with this query, if -1 all rows are included.</param>
    /// <param name="formatNewLinesAndTabs">Flag indicating if the SQL statement must be formatted to insert new line and tab characters for display purposes.</param>
    /// <param name="mappingFromTable"><see cref="MySQLDataTable"/> source table containing the data to insert.</param>
    /// <param name="nextRow">Last row processed if the query needs to be split, -1 if all rows were processed.</param>
    /// <returns>INSERT INTO SQL query.</returns>
    public string GetAppendSQL(int startRow, int limit, bool formatNewLinesAndTabs, MySQLDataTable mappingFromTable, out int nextRow)
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

      if (mappingFromTable != null && mappingFromTable.Rows.Count - (mappingFromTable.FirstRowIsHeaders ? 1 : 0) < 1)
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
      int rowIdx = 0;
      int colIdx = 0;
      int startingColNum = AddPrimaryKeyColumn ? (_useFirstColumnAsPK ? 0 : 1) : 0;
      List<string> fromColumnNames = new List<string>(colsCount);
      List<string> toColumnNames = new List<string>(colsCount);

      string rowsSeparator = string.Empty;
      string colsSeparator = string.Empty;
      queryString.AppendFormat(
        "INSERT INTO `{0}`.`{1}`{2}(",
        SchemaName,
        TableName.Replace("`", "``"),
        nl);

      //// Loop columns to assemble the piece of the query that includes the column names that we will insert data into.
      for (colIdx = startingColNum; colIdx < colsCount; colIdx++)
      {
        MySQLDataColumn toColumn = Columns[colIdx] as MySQLDataColumn;
        string fromColumnName = toColumn.MappedDataColName;
        if (toColumn.ExcludeColumn || string.IsNullOrEmpty(fromColumnName))
        {
          continue;
        }

        queryString.AppendFormat(
          "{0}`{1}`",
          colsSeparator,
          toColumn.ColumnName.Replace("`", "``"));
        colsSeparator = ",";
        fromColumnNames.Add(fromColumnName);
        toColumnNames.Add(toColumn.ColumnName);
      }

      queryString.AppendFormat("){0}VALUES{0}", nl);
      string valueToDB = string.Empty;
      int absRowIdx = 0;
      StringBuilder singleRowValuesBuilder = new StringBuilder();
      string singleRowValuesString = string.Empty;
      if (maxByteCount > 0)
      {
        queryStringByteCount = (ulong)ASCIIEncoding.ASCII.GetByteCount(queryString.ToString());
      }

      //// Loop all rows in this table to include the values for insertion in the query.
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
        else
        {
          absRowIdx++;
        }

        //// Within the current row, loop all columns to extract each value and append it to the query string.
        singleRowValuesBuilder.Clear();
        singleRowValuesString = string.Empty;
        singleRowValuesBuilder.AppendFormat("{0}(", rowsSeparator);
        colsSeparator = string.Empty;
        for (colIdx = 0; colIdx < toColumnNames.Count; colIdx++)
        {
          string fromColumnName = fromColumnNames[colIdx];
          string toColumnName = toColumnNames[colIdx];
          MySQLDataColumn toColumn = Columns[toColumnName] as MySQLDataColumn;
          bool insertingValueIsNull = false;
          valueToDB = DataTypeUtilities.GetStringValueForColumn(dr[fromColumnName], toColumn, true, out insertingValueIsNull);
          singleRowValuesBuilder.AppendFormat(
            "{0}{1}{2}{1}",
            colsSeparator,
            toColumn.ColumnsRequireQuotes && !insertingValueIsNull ? "'" : string.Empty,
            valueToDB);
          colsSeparator = ",";
        }

        //// Close the current row values piece of the query and check if we have not exceeded the maximum packet size allowed by the server,
        ////  otherwise we return the query string as is and return the last row number that was processed so another INSERT INTO query is
        ////  assembled starting from the row we left on.
        singleRowValuesBuilder.Append(")");
        singleRowValuesString = singleRowValuesBuilder.ToString();
        if (maxByteCount > 0)
        {
          ulong singleValueRowQueryByteCount = (ulong)ASCIIEncoding.ASCII.GetByteCount(singleRowValuesString);
          if (queryStringByteCount + singleValueRowQueryByteCount > maxByteCount)
          {
            nextRow = rowIdx;
            break;
          }

          queryStringByteCount += singleValueRowQueryByteCount;
        }

        //// Add a , separator for the collection of values in the INSERT QUERY.
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
    /// Inserts data taken from the given <see cref="MySQLDataTable"/> source table into the corresponding database table with the same name given the column mappings defined on this table.
    /// </summary>
    /// <param name="wbConnection">Connection to a MySQL server instance selected by users.</param>
    /// <param name="mappingFromTable"><see cref="MySQLDataTable"/> source table containing the data to insert.</param>
    /// <param name="exception">Exception thrown back (if any) when trying to insert data into the database table.</param>
    /// <param name="sqlQuery">The SQL query to insert data in the given <see cref="MySQLDataTable"/> source table into the database table.</param>
    /// <param name="insertedCount">Number of rows actually inserted into the database.</param>
    /// <returns><see cref="DataTable"/> object containing warnings thrown by the data append operation, null if no warnings were generated.</returns>
    public DataTable AppendDataWithManualQuery(MySqlWorkbenchConnection wbConnection, MySQLDataTable mappingFromTable, out Exception exception, out string sqlQuery, out int insertedCount)
    {
      DataSet warningsDS = null;
      insertedCount = 0;
      exception = null;
      MySqlTransaction transaction = null;
      string chunkQuery = sqlQuery = string.Empty;

      string connectionString = MySQLDataUtilities.GetConnectionString(wbConnection);
      using (MySqlConnection conn = new MySqlConnection(connectionString))
      {
        try
        {
          conn.Open();
          if (_mysqlMaxAllowedPacket == 0)
          {
            _mysqlMaxAllowedPacket = MySQLDataUtilities.GetMySQLServerMaxAllowedPacket(conn);
          }

          transaction = conn.BeginTransaction();
          MySqlCommand cmd = new MySqlCommand(string.Empty, conn, transaction);
          int nextRow = 0;
          while (nextRow >= 0)
          {
            chunkQuery = GetAppendSQL(nextRow, -1, true, mappingFromTable, out nextRow);
            cmd.CommandText = chunkQuery;
            sqlQuery += chunkQuery;
            insertedCount += cmd.ExecuteNonQuery();
          }

          transaction.Commit();
          warningsDS = MySqlHelper.ExecuteDataset(conn, "SHOW WARNINGS");
        }
        catch (MySqlException mysqlEx)
        {
          if (transaction != null)
          {
            transaction.Rollback();
          }

          exception = mysqlEx;
          MiscUtilities.WriteAppErrorToLog(mysqlEx);
        }
        catch (Exception ex)
        {
          if (transaction != null)
          {
            transaction.Rollback();
          }

          exception = ex;
          MiscUtilities.WriteAppErrorToLog(ex);
        }
      }

      return warningsDS != null && warningsDS.Tables.Count > 0 ? warningsDS.Tables[0] : null;
    }

    /// <summary>
    /// Creates a SQL query meant to push changes in the given <see cref="DataRow"/> object to the database server.
    /// </summary>
    /// <param name="row"><see cref="DataRow"/> object with changes to push to the database server.</param>
    /// <returns>A SQL query containing the data changes.</returns>
    public string GetSQL(DataRow row)
    {
      if (row == null || row.RowState == DataRowState.Unchanged)
      {
        return string.Empty;
      }

      string valueToDB;
      ulong queryStringByteCount = 0;
      ulong maxByteCount = _mysqlMaxAllowedPacket > 0 ? _mysqlMaxAllowedPacket - SAFE_BYTES_BEFORE_REACHING_MAX_ALLOWED_PACKET : 0;
      StringBuilder queryString = new StringBuilder();
      string colsSeparator = string.Empty;
      bool pkValueIsNull = false;

      switch (row.RowState)
      {
        case DataRowState.Deleted:
          queryString.AppendFormat(
            "DELETE FROM `{0}`.`{1}` WHERE ",
            SchemaName,
            TableName.Replace("`", "``"));
          foreach (MySQLDataColumn pkCol in Columns)
          {
            if (!pkCol.PrimaryKey)
            {
              continue;
            }

            valueToDB = DataTypeUtilities.GetStringValueForColumn(row[pkCol.ColumnName, DataRowVersion.Original], pkCol, false, out pkValueIsNull);
            queryString.AppendFormat(
              "{0}`{1}`={2}{3}{2}",
              colsSeparator,
              pkCol.ColumnName.Replace("`", "``"),
              pkCol.ColumnsRequireQuotes && !pkValueIsNull ? "'" : string.Empty,
              valueToDB);
            colsSeparator = " AND ";
          }

          break;
        case DataRowState.Added:
          int colIdx = 0;
          int startingColNum = AddPrimaryKeyColumn ? (_useFirstColumnAsPK ? 0 : 1) : 0;
          List<string> insertColumnNames = new List<string>(Columns.Count);
          queryString.AppendFormat(
            "INSERT INTO `{0}`.`{1}` (",
            SchemaName,
            TableName.Replace("`", "``"));
          for (colIdx = startingColNum; colIdx < Columns.Count; colIdx++)
          {
            MySQLDataColumn column = Columns[colIdx] as MySQLDataColumn;
            if (column.ExcludeColumn)
            {
              continue;
            }

            queryString.AppendFormat(
              "{0}`{1}`",
              colsSeparator,
              column.DisplayName.Replace("`", "``"));
            colsSeparator = ",";
            insertColumnNames.Add(column.ColumnName);
          }

          queryString.Append(") VALUES (");
          bool insertingValueIsNull = false;
          valueToDB = string.Empty;
          colsSeparator = string.Empty;
          foreach (string insertingColName in insertColumnNames)
          {
            MySQLDataColumn column = Columns[insertingColName] as MySQLDataColumn;
            valueToDB = DataTypeUtilities.GetStringValueForColumn(row[insertingColName], column, true, out insertingValueIsNull);
            queryString.AppendFormat(
              "{0}{1}{2}{1}",
              colsSeparator,
              column.ColumnsRequireQuotes && !insertingValueIsNull ? "'" : string.Empty,
              valueToDB);
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
            TableName.Replace("`", "``"));
          foreach (MySQLDataColumn column in Columns)
          {
            bool updatingValueIsNull = false;
            string finalColName = column.ColumnName.Replace("`", "``");
            if (column.PrimaryKey)
            {
              valueToDB = DataTypeUtilities.GetStringValueForColumn(row[column.ColumnName, DataRowVersion.Original], column, false, out updatingValueIsNull);
              wClauseString.AppendFormat(
                "{0}`{1}`={2}{3}{2}",
                wClauseColsSeparator,
                finalColName,
                column.ColumnsRequireQuotes && !updatingValueIsNull ? "'" : string.Empty,
                valueToDB);
              wClauseColsSeparator = " AND ";
            }

            if (changedColNamesList.Contains(column.ColumnName))
            {
              valueToDB = DataTypeUtilities.GetStringValueForColumn(row[column.ColumnName], column, true, out updatingValueIsNull);
              queryString.AppendFormat(
                "{0}`{1}`={2}{3}{2}",
                colsSeparator,
                finalColName,
                column.ColumnsRequireQuotes && !updatingValueIsNull ? "'" : string.Empty,
                valueToDB);
              colsSeparator = ",";
            }
          }

          wClauseString.Append(";");
          queryString.Append(wClauseString.ToString());
          break;
      }

      //// Verify we have not exceeded the maximum packet size allowed by the server, otherwise throw an Exception.
      string retQuery = queryString.ToString();
      if (maxByteCount > 0)
      {
        queryStringByteCount = (ulong)ASCIIEncoding.ASCII.GetByteCount(retQuery);
        if (queryStringByteCount > maxByteCount)
        {
          throw new Exception(Properties.Resources.QueryExceedsMaxAllowedPacketError);
        }
      }

      return retQuery;
    }

    /// <summary>
    /// Creates a SQL query meant to push changes in the <see cref="DataRow"/> object found at the given row index.
    /// </summary>
    /// <param name="rowIndex">Row index of the <see cref="DataRow"/> object containing the changes.</param>
    /// <returns>A SQL query containing the data changes.</returns>
    public string GetSQL(int rowIndex)
    {
      if (rowIndex < 0 || rowIndex >= Rows.Count)
      {
        return string.Empty;
      }

      return GetSQL(Rows[rowIndex]);
    }

    /// <summary>
    /// Pushes all changes in this table's data to its corresponding database table.
    /// </summary>
    /// <param name="wbConnection">Connection to a MySQL server instance selected by users.</param>
    /// <returns>A <see cref="PushResultsDataTable"/> object containing a log of the results of each query executed against the database server.</returns>
    public PushResultsDataTable PushData(MySqlWorkbenchConnection wbConnection)
    {
      if (GetChanges().Rows.Count == 0)
      {
        return null;
      }

      PushResultsDataTable resultsDT = new PushResultsDataTable();
      MySqlTransaction transaction = null;
      DataSet warningsDS = null;
      string connectionString = MySQLDataUtilities.GetConnectionString(wbConnection);
      DataRowState[] pushOperationsArray = new DataRowState[3] { DataRowState.Deleted, DataRowState.Added, DataRowState.Modified };
      PushResultsDataTable.OperationType currentOperationType = PushResultsDataTable.OperationType.Prepare;
      string queryText = string.Empty;
      string errorText = string.Empty;
      StringBuilder warningText = new StringBuilder();
      int executedCount = 0;
      int operationIndex = 0;
      DataRow lastRow = null;

      using (MySqlConnection conn = new MySqlConnection(connectionString))
      {
        try
        {
          conn.Open();
          if (_mysqlMaxAllowedPacket == 0)
          {
            _mysqlMaxAllowedPacket = MySQLDataUtilities.GetMySQLServerMaxAllowedPacket(conn);
          }

          transaction = conn.BeginTransaction();
          MySqlCommand cmd = new MySqlCommand(string.Empty, conn, transaction);

          foreach (DataRowState operation in pushOperationsArray)
          {
            foreach (DataRow dr in Rows)
            {
              if (dr.RowState != operation)
              {
                continue;
              }

              executedCount = 0;
              warningText.Clear();
              operationIndex++;
              lastRow = dr;
              queryText = GetSQL(dr);
              cmd.CommandText = queryText;
              executedCount = cmd.ExecuteNonQuery();
              warningsDS = MySqlHelper.ExecuteDataset(conn, "SHOW WARNINGS");
              currentOperationType = PushResultsDataTable.GetRelatedOperationType(operation);
              if ((warningsDS != null && warningsDS.Tables.Count > 0 && warningsDS.Tables[0].Rows.Count > 0) || executedCount == 0)
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

                foreach (DataRow warningRow in warningsDS.Tables[0].Rows)
                {
                  warningText.AppendFormat(
                    "{3}{0:000}: {1} - {2}",
                    operationIndex,
                    warningRow[1].ToString(),
                    warningRow[2].ToString(),
                    nl);
                  nl = Environment.NewLine;
                }

                resultsDT.AddResult(operationIndex, currentOperationType, PushResultsDataTable.OperationResult.Warning, queryText, warningText.ToString(), executedCount);
              }
              else
              {
                resultsDT.AddResult(operationIndex, currentOperationType, PushResultsDataTable.OperationResult.Success, queryText, "OK", executedCount);
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

          errorText = string.Format("MySQL Error {0}:{1}{2}", mysqlEx.Number, Environment.NewLine, mysqlEx.Message);
          resultsDT.AddResult(operationIndex, currentOperationType, PushResultsDataTable.OperationResult.Error, queryText, errorText, 0);
          MiscUtilities.WriteAppErrorToLog(mysqlEx);
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

          errorText = string.Format("ADO.NET Error:{0}{1}", Environment.NewLine, ex.Message);
          resultsDT.AddResult(operationIndex, currentOperationType, PushResultsDataTable.OperationResult.Error, queryText, errorText, 0);
          MiscUtilities.WriteAppErrorToLog(ex);
        }
      }

      return resultsDT;
    }

    /// <summary>
    /// Reverts any changes done to the table since the last data push operation or refreshes its data with a fresh copy of the data.
    /// </summary>
    /// <param name="refreshFromDB">Flag indicating if instead of just reverting present changes a fresh copy of the data must be retrieved from the database.</param>
    /// <param name="wbConnection">Connection to a MySQL server instance selected by users.</param>
    /// <param name="exception">Exception thrown back (if any) when trying to fetch data from the database to refresh the data for this table.</param>
    public void RevertData(bool refreshFromDB, MySqlWorkbenchConnection wbConnection, out Exception exception)
    {
      exception = null;

      if (!refreshFromDB)
      {
        RejectChanges();
        return;
      }

      try
      {
        Clear();
        DataTable filledTable = MySQLDataUtilities.GetDataFromTableOrView(wbConnection, SelectQuery);
        CreateTableSchema(TableName, true, wbConnection);
        CopyTableData(filledTable);
      }
      catch (MySqlException mysqlEx)
      {
        exception = mysqlEx;
        MiscUtilities.WriteAppErrorToLog(mysqlEx);
      }
      catch (Exception ex)
      {
        exception = ex;
        MiscUtilities.WriteAppErrorToLog(ex);
      }
    }

    /// <summary>
    /// Recreates the values of the automatically created first column for the table's primary key depending on the value of the <see cref="_firstRowIsHeaders"/> field.
    /// </summary>
    private void AdjustAutoPKValues()
    {
      if (AddPrimaryKeyColumn && Columns.Count > 0)
      {
        int adjustIdx = _firstRowIsHeaders ? 0 : 1;
        for (int i = 0; i < Rows.Count; i++)
        {
          Rows[i][0] = i + adjustIdx;
        }
      }
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
        InfoDialog errorDialog = new InfoDialog(false, Properties.Resources.TableDataCopyErrorTitle, string.Format("{0}{2}{2}{1}", ex.Message, ex.StackTrace, Environment.NewLine));
        errorDialog.WordWrapDetails = true;
        errorDialog.ShowDialog();
        MiscUtilities.WriteAppErrorToLog(ex);
      }
    }

    /// <summary>
    /// Adds a specified number of <see cref="MySQLDataColumn"/> objects to the Columns collection where the first column may be an automatically created one for the table's primary index.
    /// </summary>
    /// <param name="numCols">Number of columns to add to the table.</param>
    private void CreateColumns(int numCols)
    {
      MySQLDataColumn column = null;
      int startCol = AddPrimaryKeyColumn ? 0 : 1;
      for (int colIdx = startCol; colIdx <= numCols; colIdx++)
      {
        string name = "Column" + colIdx;
        column = new MySQLDataColumn();
        column.ColumnName = column.DisplayName = name;
        Columns.Add(column);
      }

      if (AddPrimaryKeyColumn)
      {
        column = Columns[0] as MySQLDataColumn;
        column.PrimaryKey = true;
        column.AutoPK = true;
        column.ColumnName = column.DisplayName = string.Format("{0}{1}id", TableName, TableName.Length > 0 ? "_" : string.Empty);
        column.MySQLDataType = "Integer";
        column.AutoIncrement = true;
        column.AllowNull = false;
      }
    }

    /// <summary>
    /// Creates columns for this table using the information schema of a MySQL table with the given name to mirror their properties.
    /// </summary>
    /// <param name="tableName">Name of the table.</param>
    /// <param name="datesAsMySQLDates">Flag indicating if the dates are stored in the table as <see cref="System.DateTime"/> or <see cref="MySql.Data.Types.MySqlDateTime"/> objects.</param>
    /// <param name="wbConnection">Connection to a MySQL server instance selected by users.</param>
    private void CreateTableSchema(string tableName, bool datesAsMySQLDates, MySqlWorkbenchConnection wbConnection)
    {
      Columns.Clear();
      DataTable columnsInfoTable = MySQLDataUtilities.GetSchemaCollection(wbConnection, "Columns Short", null, wbConnection.Schema, tableName);
      if (columnsInfoTable != null)
      {
        foreach (DataRow columnInfoRow in columnsInfoTable.Rows)
        {
          string colName = columnInfoRow["Field"].ToString();
          string dataType = columnInfoRow["Type"].ToString();
          bool allowNulls = columnInfoRow["Null"].ToString() == "YES";
          bool isPrimaryKey = columnInfoRow["Key"].ToString() == "PRI";
          string extraInfo = columnInfoRow["Extra"].ToString();
          MySQLDataColumn column = new MySQLDataColumn(colName, dataType, datesAsMySQLDates, allowNulls, isPrimaryKey, extraInfo);
          Columns.Add(column);
        }
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
    /// <param name="addBufferToVarchar">Flag indicating if a buffer is added for columns whose data type was automatically detected as varchar.</param>
    /// <param name="createIndexForIntColumns">Flag indicating if the CreateIndex property of <see cref="MySQLDataColumn"/> columns is automatically set if their data type is Integer.</param>
    /// <param name="emptyColumnsToVarchar">Flag indicating if the data type for columns with no data is automatically set to varchar(255).</param>
    private void DetectTypes(object[,] excelData, bool addBufferToVarchar, bool createIndexForIntColumns, bool emptyColumnsToVarchar)
    {
      int rowsCount = excelData.GetUpperBound(0);
      int colsCount = excelData.GetUpperBound(1);
      int colAdjustIdx = AddPrimaryKeyColumn ? 0 : 1;

      for (int dataColPos = 1; dataColPos <= colsCount; dataColPos++)
      {
        MySQLDataColumn col = Columns[dataColPos - colAdjustIdx] as MySQLDataColumn;
        if (col.ExcludeColumn)
        {
          continue;
        }

        object valueFromArray = null;
        string proposedType = string.Empty;
        string strippedType = string.Empty;
        string valueAsString = string.Empty;
        bool valueOverflow = false;
        List<string> typesListFor1stAndRest = new List<string>(2);
        List<string> typesListFrom2ndRow = new List<string>(rowsCount - 1);
        int[] varCharMaxLen = new int[2] { 0, 0 };    //// 0 - All rows original datatype varcharmaxlen, 1 - All rows Varchar forced datatype maxlen
        int[] decimalMaxLen = new int[2] { 0, 0 };    //// 0 - Integral part max length, 1 - decimal part max length
        int leftParensIndex = -1;
        int varCharValueLength = 0;

        for (int rowPos = 1; rowPos <= rowsCount; rowPos++)
        {
          proposedType = strippedType = valueAsString = string.Empty;
          valueFromArray = excelData[rowPos, dataColPos];
          if (valueFromArray == null)
          {
            continue;
          }

          //// Treat always as a Varchar value first in case all rows do not have a consistent datatype just to see the varchar len calculated by GetMySQLExportDataType
          valueAsString = valueFromArray.ToString();
          proposedType = DataTypeUtilities.GetMySQLExportDataType(valueAsString, out valueOverflow);
          if (proposedType == "Bool")
          {
            proposedType = "Varchar(5)";
          }
          else if (proposedType.StartsWith("Date"))
          {
            proposedType = string.Format("Varchar({0})", valueAsString.Length);
          }

          leftParensIndex = proposedType.IndexOf("(");
          varCharValueLength = addBufferToVarchar ? int.Parse(proposedType.Substring(leftParensIndex + 1, proposedType.Length - leftParensIndex - 2)) : valueAsString.Length;
          varCharMaxLen[1] = Math.Max(varCharValueLength, varCharMaxLen[1]);

          //// Normal datatype detection
          proposedType = DataTypeUtilities.GetMySQLExportDataType(valueFromArray, out valueOverflow);
          leftParensIndex = proposedType.IndexOf("(");
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
              Rows[rowPos - 1][dataColPos - colAdjustIdx] = dtValue.ToString(DataTypeUtilities.DATE_FORMAT);
              break;
            case "Varchar":
              varCharValueLength = addBufferToVarchar ? int.Parse(proposedType.Substring(leftParensIndex + 1, proposedType.Length - leftParensIndex - 2)) : valueAsString.Length;
              varCharMaxLen[0] = Math.Max(varCharValueLength, varCharMaxLen[0]);
              break;
            case "Decimal":
              int commaPos = proposedType.IndexOf(",");
              decimalMaxLen[0] = Math.Max(int.Parse(proposedType.Substring(leftParensIndex + 1, commaPos - leftParensIndex - 1)), decimalMaxLen[0]);
              decimalMaxLen[1] = Math.Max(int.Parse(proposedType.Substring(commaPos + 1, proposedType.Length - commaPos - 2)), decimalMaxLen[1]);
              break;
          }

          if (rowPos == 1)
          {
            typesListFor1stAndRest.Add(strippedType);
          }
          else
          {
            typesListFrom2ndRow.Add(strippedType);
          }
        }

        //// Get the consistent DataType for all rows except first one.
        proposedType = DataTypeUtilities.GetConsistentDataTypeOnAllRows(strippedType, typesListFrom2ndRow, decimalMaxLen, varCharMaxLen);
        if (emptyColumnsToVarchar && string.IsNullOrEmpty(proposedType))
        {
          proposedType = "Varchar(255)";
        }

        col.RowsFrom2ndDataType = proposedType;
        if (proposedType == "Datetime")
        {
          foreach (DataRow dr in Rows)
          {
            if (dr[dataColPos - colAdjustIdx].ToString() == "0")
            {
              dr[dataColPos - colAdjustIdx] = DataTypeUtilities.EMPTY_DATE;
            }
          }
        }

        //// Get the consistent DataType between first columnInfoRow and the previously computed consistent DataType for the rest of the rows.
        leftParensIndex = proposedType.IndexOf("(");
        strippedType = leftParensIndex < 0 ? proposedType : proposedType.Substring(0, leftParensIndex);
        typesListFor1stAndRest.Add(strippedType);
        proposedType = DataTypeUtilities.GetConsistentDataTypeOnAllRows(strippedType, typesListFor1stAndRest, decimalMaxLen, varCharMaxLen);
        col.RowsFrom1stDataType = proposedType;

        col.MySQLDataType = _firstRowIsHeaders ? col.RowsFrom2ndDataType : col.RowsFrom1stDataType;
        col.CreateIndex = createIndexForIntColumns && col.MySQLDataType == "Integer";
      }
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
      int startRow = AddPrimaryKeyColumn ? 1 : 0;
      for (int i = startRow; i < Columns.Count; i++)
      {
        MySQLDataColumn col = Columns[i] as MySQLDataColumn;
        col.DisplayName = _firstRowIsHeaders ? DataToColName(row[i].ToString()) : col.ColumnName;
        col.MySQLDataType = _firstRowIsHeaders ? col.RowsFrom2ndDataType : col.RowsFrom1stDataType;
      }

      AdjustAutoPKValues();
    }
  }
}
