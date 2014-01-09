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
using MySQL.ForExcel.Forms;
using MySQL.ForExcel.Interfaces;
using MySQL.ForExcel.Properties;
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
    /// Bytes to subtract from the maximum allowed packet size to build a query that is safely processed by the database server.
    /// </summary>
    public const int SAFE_BYTES_BEFORE_REACHING_MAX_ALLOWED_PACKET = 10;

    #region Fields

    /// <summary>
    /// List of text strings containing warnings for users about the auto-generated primary key.
    /// </summary>
    private readonly List<string> _autoPkWarningTextsList;

    /// <summary>
    /// Flag indicating if data is being copied from a regular <see cref="DataTable"/> object.
    /// </summary>
    private bool _copyingTableData;

    /// <summary>
    /// Flag indicating whether data type for each column is automatically detected when data is loaded by the <see cref="SetupColumnsWithData"/> method.
    /// </summary>
    private bool _detectDatatype;

    /// <summary>
    /// Flag indicating if the first row in the Excel region to be exported contains the column names of the MySQL table that will be created.
    /// </summary>
    private bool _firstRowIsHeaders;

    /// <summary>
    /// Flag indicating if the column names where changed to use the first row of data.
    /// </summary>
    private bool _changedColumnNamesWithFirstRowOfData;

    /// <summary>
    /// Flag indicating whether during an Export operation only the table will be created without any data.
    /// </summary>
    private bool _createTableWithoutData;

    /// <summary>
    /// Contains the value of the max_allowed_packet system variable of the MySQL Server currently connected to.
    /// </summary>
    private ulong _mysqlMaxAllowedPacket;

    /// <summary>
    /// The SELECT query used to retrieve the excelData from the corresponding MySQL table to fill this one.
    /// </summary>
    private string _selectQuery;

    /// <summary>
    /// Flag indicating whether there is a MySQL table in the connected schema with the same name as in <see cref="TableName"/>.
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

    /// <summary>
    /// Flag indicating whether optimistic locking is used for the update of rows.
    /// </summary>
    private bool _useOptimisticUpdate;

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
    /// <param name="detectDataType">Flag indicating if the data type for each column is automatically detected when data is loaded by the <see cref="SetupColumnsWithData"/> method.</param>
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
      IsFormatted = useFormattedValues;
      OperationType = DataOperationType.Export;
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
    /// <param name="useFormattedValues">Flag indicating if the Excel excelData used to populate this table is formatted (numbers, dates, text) or not (numbers and text).</param>
    /// <param name="wbConnection">MySQL Workbench connection to a MySQL server instance selected by users.</param>
    public MySqlDataTable(string tableName, bool fetchColumnsSchemaInfo, bool datesAsMySqlDates, bool useFormattedValues, MySqlWorkbenchConnection wbConnection)
      : this(wbConnection.Schema, tableName)
    {
      IsFormatted = useFormattedValues;
      OperationType = DataOperationType.Append;
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
    /// <param name="importForEditData"><c>true</c> if the import is part of an Edit operation, <c>false</c> otherwise.</param>
    public MySqlDataTable(string tableName, DataTable filledTable, MySqlWorkbenchConnection wbConnection, bool importForEditData)
      : this(tableName, true, true, false, wbConnection)
    {
      CopyTableData(filledTable);
      OperationType = importForEditData ? DataOperationType.Edit : DataOperationType.Import;
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
    /// This constructor is meant to be used by the <see cref="ImportProcedureForm"/> class to copy the contents of result set tables from an executed procedure.
    /// </summary>
    /// <param name="filledTable"><see cref="DataTable"/> object containing imported excelData from the MySQL table to be edited.</param>
    /// <param name="schemaName">Name of the schema where this table exists.</param>
    public MySqlDataTable(DataTable filledTable, string schemaName)
      : this(schemaName, filledTable.TableName)
    {
      CopyTableSchemaAndData(filledTable);
      OperationType = DataOperationType.Import;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="MySqlDataTable"/> class.
    /// </summary>
    public MySqlDataTable()
    {
      _autoPkWarningTextsList = new List<string>(1);
      _changedColumnNamesWithFirstRowOfData = false;
      _copyingTableData = false;
      _createTableWithoutData = false;
      _detectDatatype = false;
      _mysqlMaxAllowedPacket = 0;
      _tableExistsInSchema = null;
      _tableWarningsTextList = new List<string>(3);
      _selectQuery = string.Format("SELECT * FROM `{0}`", TableNameForSqlQueries);
      _useOptimisticUpdate = false;
      AddBufferToVarchar = false;
      AddPrimaryKeyColumn = false;
      AutoAllowEmptyNonIndexColumns = false;
      AutoIndexIntColumns = false;
      FirstRowIsHeaders = false;
      IsTableNameValid = !string.IsNullOrEmpty(TableName);
      IsFormatted = false;
      OperationType = DataOperationType.Import;
      RemoveEmptyColumns = false;
      SchemaName = string.Empty;
      UseFirstColumnAsPk = false;
      WbConnection = null;
    }

    /// <summary>
    /// Specifies identifiers to indicate the type of operation a <see cref="MySqlDataTable"/> is used for.
    /// </summary>
    public enum DataOperationType
    {
      /// <summary>
      /// Append data operation.
      /// </summary>
      Append,

      /// <summary>
      /// Edit data operation.
      /// </summary>
      Edit,

      /// <summary>
      /// Export data operation.
      /// </summary>
      Export,

      /// <summary>
      /// Import data operation.
      /// </summary>
      Import
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
    /// Gets or sets a value indicating whether during an Export operation only the table will be created without any data.
    /// </summary>
    public bool CreateTableWithoutData
    {
      get
      {
        return _createTableWithoutData;
      }

      set
      {
        if (_createTableWithoutData != value)
        {
          OnPropertyChanged("CreateTableWithoutData");
        }

        _createTableWithoutData = value;
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
    /// Gets a value indicating whether data type for each column is automatically detected when data is loaded by the <see cref="SetupColumnsWithData"/> method.
    /// </summary>
    public bool DetectDatatype
    {
      get
      {
        return _detectDatatype;
      }

      set
      {
        if (_detectDatatype != value)
        {
          OnPropertyChanged("DetectDatatype");
        }

        _detectDatatype = value;
      }
    }

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
        if (_firstRowIsHeaders != value)
        {
          OnPropertyChanged("FirstRowIsHeaders");
        }

        _firstRowIsHeaders = value;
        UseFirstRowAsHeaders();
      }
    }

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
    /// Gets the value of the max_allowed_packet system variable of the MySQL Server currently connected to.
    /// </summary>
    public ulong MySqlMaxAllowedPacket
    {
      get
      {
        if (_mysqlMaxAllowedPacket == 0 && WbConnection != null)
        {
          _mysqlMaxAllowedPacket = WbConnection.GetMySqlServerMaxAllowedPacket();
        }

        return _mysqlMaxAllowedPacket;
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
    /// Gets the data operation the table is used for.
    /// </summary>
    public DataOperationType OperationType { get; private set; }

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
    public string SelectQuery
    {
      get
      {
        return _selectQuery;
      }

      set
      {
        if (!string.Equals(_selectQuery, value, StringComparison.InvariantCulture))
        {
          OnPropertyChanged("SelectQuery");
        }

        _selectQuery = value;
      }
    }

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
        if (!string.Equals(base.TableName, value, StringComparison.InvariantCulture))
        {
          OnPropertyChanged("TableName");
          _tableExistsInSchema = null;
        }

        base.TableName = value;
        if (OperationType != DataOperationType.Export)
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
        if (_useFirstColumnAsPk != value)
        {
          OnPropertyChanged("UseFirstColumnAsPk");
        }

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
    /// Gets or sets a value indicating whether optimistic locking is used for the update of rows.
    /// </summary>
    public bool UseOptimisticUpdate
    {
      get
      {
        return _useOptimisticUpdate;
      }

      set
      {
        if (_useOptimisticUpdate != value)
        {
          OnPropertyChanged("UseOptimisticUpdate");
        }

        _useOptimisticUpdate = value;
      }
    }

    /// <summary>
    /// Gets a <see cref="MySqlWorkbenchConnection"/> object representing the connection to a MySQL server instance selected by users.
    /// </summary>
    public MySqlWorkbenchConnection WbConnection { get; private set; }

    #endregion Properties

    #region Events

    /// <summary>
    /// Occurs when a property value on any of the columns in this table changes.
    /// </summary>
    public event PropertyChangedEventHandler TableColumnPropertyValueChanged;

    /// <summary>
    /// Occurs when a property value in this table changes.
    /// </summary>
    public event PropertyChangedEventHandler PropertyChanged;

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

    #endregion Events

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
        FirstRowIsHeaders = FirstRowIsHeaders,
        OperationType = OperationType,
        UseOptimisticUpdate = UseOptimisticUpdate
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
      string nlt = formatNewLinesAndTabs ? Environment.NewLine + "   " : " ";

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
    /// Imports data contained in the given <see cref="MySqlDataTable"/> object at the active Excel cell.
    /// </summary>
    /// <param name="importColumnNames">Flag indicating if column names will be imported as the first row of imported data.</param>
    /// <returns>The <see cref="Excel.Range"/> containing the cells with the imported data.</returns>
    public Excel.Range ImportDataAtActiveExcelCell(bool importColumnNames)
    {
      return ImportDataAtGivenExcelCell(importColumnNames, Globals.ThisAddIn.Application.ActiveCell);
    }

    /// <summary>
    /// Imports data contained in the given <see cref="MySqlDataTable"/> object at the specified cell <see cref="Excel.Range"/>.
    /// </summary>
    /// <param name="importColumnNames">Flag indicating if column names will be imported as the first row of imported data.</param>
    /// <param name="atCell">The starting Excel (left-most and top-most) cell where the imported data is placed.</param>
    /// <returns>The <see cref="Excel.Range"/> containing the cells with the imported data.</returns>
    public Excel.Range ImportDataAtGivenExcelCell(bool importColumnNames, Excel.Range atCell)
    {
      int startingRow = importColumnNames ? 1 : 0;
      int rowsCount = Rows.Count + startingRow;
      if (rowsCount == 0)
      {
        return null;
      }

      Excel.Range fillingRange = null;
      try
      {
        int currentRow = atCell.Row - 1;
        int colsCount = Columns.Count;
        Excel.Workbook activeWorkbook = atCell.Worksheet.Parent as Excel.Workbook;
        int cappedNumRows = activeWorkbook != null && activeWorkbook.Excel8CompatibilityMode ? Math.Min(rowsCount, UInt16.MaxValue - currentRow) : rowsCount;
        bool escapeFormulaTexts = Settings.Default.ImportEscapeFormulaTextValues;

        fillingRange = atCell.Resize[cappedNumRows, colsCount];
        object[,] fillingArray = new object[cappedNumRows, colsCount];

        if (importColumnNames)
        {
          for (int currCol = 0; currCol < colsCount; currCol++)
          {
            fillingArray[0, currCol] = Columns[currCol].ColumnName;
          }
        }

        int fillingRowIdx = startingRow;
        cappedNumRows -= startingRow;
        for (int currRow = 0; currRow < cappedNumRows; currRow++)
        {
          MySqlDataRow mySqlRow = Rows[currRow] as MySqlDataRow;
          if (mySqlRow == null)
          {
            continue;
          }

          for (int currCol = 0; currCol < colsCount; currCol++)
          {
            object importingValue = DataTypeUtilities.GetImportingValueForDateType(mySqlRow[currCol]);
            if (importingValue is string)
            {
              string importingValueText = importingValue as string;

              // If the imported value is a text that starts with an equal sign Excel will treat it as a formula
              //  so it needs to be escaped prepending an apostrophe to it for Excel to treat it as standard text.
              if (escapeFormulaTexts && importingValueText.StartsWith("="))
              {
                importingValue = "'" + importingValueText;
              }
            }

            fillingArray[fillingRowIdx, currCol] = importingValue;
          }

          mySqlRow.ExcelRange = fillingRange.Rows[fillingRowIdx + 1] as Excel.Range;
          fillingRowIdx++;
        }

        Globals.ThisAddIn.Application.Goto(fillingRange, false);
        fillingRange.ClearFormats();
        fillingRange.Value = fillingArray;

        // Create Named Table for the imported data
        if (Settings.Default.ImportCreateExcelTable && OperationType.IsForImport())
        {
          Excel.XlYesNoGuess containsColumnNames = importColumnNames ? Excel.XlYesNoGuess.xlYes : Excel.XlYesNoGuess.xlNo;
          var excelTable = fillingRange.Worksheet.ListObjects.Add(Excel.XlListObjectSourceType.xlSrcRange, fillingRange, containsColumnNames);
          string excelTableNamePrefix = Settings.Default.ImportPrefixExcelTable && !string.IsNullOrEmpty(Settings.Default.ImportPrefixExcelTableText) ? Settings.Default.ImportPrefixExcelTableText + "." : string.Empty;
          string excelTableNameSchemaPiece = !string.IsNullOrEmpty(SchemaName) ? SchemaName + "." : string.Empty;
          string excelTableNameTablePiece = !string.IsNullOrEmpty(TableName) ? TableName : "Table";
          string excelTableName = excelTableNamePrefix + excelTableNameSchemaPiece + excelTableNameTablePiece;
          excelTable.Name = excelTableName.GetExcelTableNameAvoidingDuplicates();
          excelTable.DisplayName = excelTable.Name;
          excelTable.TableStyle = Settings.Default.ImportExcelTableStyleName;
        }
        else if (importColumnNames)
        {
          Excel.Range headerRange = fillingRange.GetColumnNamesRange();
          headerRange.SetInteriorColor(ExcelUtilities.LockedCellsOleColor);
          headerRange.Font.Bold = true;
        }

        atCell.Worksheet.Columns.AutoFit();
        fillingRange.Rows.AutoFit();
        atCell.Select();
      }
      catch (Exception ex)
      {
        MiscUtilities.ShowCustomizedErrorDialog(Resources.ImportDataErrorDetailText, ex.Message, true);
        MySqlSourceTrace.WriteAppErrorToLog(ex);
      }

      return fillingRange;
    }

    /// <summary>
    /// Pushes all changes in this table's data to its corresponding database table.
    /// </summary>
    /// <param name="showMySqlScriptDialog">Flag indicating whether the <see cref="MySqlScriptDialog"/> is shown before applying the query.</param>
    /// <returns></returns>
    public List<IMySqlDataRow> PushData(bool showMySqlScriptDialog)
    {
      var dataTable = GetChanges();
      if (dataTable != null && dataTable.Rows.Count == 0 && !CreateTableWithoutData)
      {
        // No rows have any changes, so exit.
        return null;
      }

      List<IMySqlDataRow> statementRowsList;
      using (var sqlScriptDialog = new MySqlScriptDialog(this))
      {
        if (showMySqlScriptDialog)
        {
          sqlScriptDialog.ShowDialog();
        }
        else
        {
          sqlScriptDialog.ApplyScript();
        }

        statementRowsList = sqlScriptDialog.ActualStatementRowsList;
      }

      return statementRowsList;
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
    /// Creates data rows in this table for data in the given Excel range.
    /// </summary>
    /// <param name="dataRange">Excel data range containing the data to fill the table.</param>
    /// <param name="useMappedColumns">Flag indicating if the data is added to the mapped column instead of to the column with the same index as the Excel data.</param>
    /// <param name="columnsHaveAnyDataList">A list of boolean values for each of the columns in the Excel range representing if the column has data or not.</param>
    /// <returns><c>true</c> if the data addition was successful, <c>false</c> otherwise.</returns>
    public bool AddExcelData(Excel.Range dataRange, bool useMappedColumns, List<bool> columnsHaveAnyDataList = null)
    {
      if (dataRange == null)
      {
        return false;
      }

      try
      {
        if (columnsHaveAnyDataList == null)
        {
          columnsHaveAnyDataList = dataRange.GetColumnsWithDataInfoList(AddPrimaryKeyColumn);
        }

        var excelData = dataRange.ToBidimensionalArray(IsFormatted);
        int numRows = excelData.GetUpperBound(0);
        int colAdjustIdx = AddPrimaryKeyColumn ? 1 : 0;
        int rowAdjustValue = _firstRowIsHeaders ? 1 : 0;
        for (int row = 1 + rowAdjustValue; row <= numRows; row++)
        {
          bool rowHasAnyData = false;
          var dataRow = NewRow() as MySqlDataRow;
          if (dataRow == null)
          {
            continue;
          }

          foreach (MySqlDataColumn mySqlColumn in Columns.Cast<MySqlDataColumn>().Where(mySqlColumn => !useMappedColumns || (!mySqlColumn.ExcludeColumn && mySqlColumn.MappedDataColOrdinal >= 0)))
          {
            if (mySqlColumn.AutoPk)
            {
              dataRow[0] = row - rowAdjustValue;
              continue;
            }

            var rangeColumnIndex = useMappedColumns
              ? mySqlColumn.MappedDataColOrdinal
              : mySqlColumn.Ordinal - colAdjustIdx;
            if (!columnsHaveAnyDataList[rangeColumnIndex])
            {
              continue;
            }

            // Increment the rangeColumnIndex by 1 because the indexes within the Excel range begin with 1 not 0.
            rangeColumnIndex++;
            rowHasAnyData = rowHasAnyData || excelData[row, rangeColumnIndex] != null;
            dataRow[mySqlColumn.Ordinal] = excelData[row, rangeColumnIndex] != null &&
                                           excelData[row, rangeColumnIndex].Equals(0.0) && mySqlColumn.IsDate
              ? DataTypeUtilities.MYSQL_EMPTY_DATE
              : dataRow[mySqlColumn.Ordinal] = excelData[row, rangeColumnIndex];
          }

          if (rowHasAnyData)
          {
            if (row == 1 && _firstRowIsHeaders)
            {
              dataRow.IsHeadersRow = true;
            }

            Rows.Add(dataRow);
          }
          else
          {
            rowAdjustValue++;
          }
        }
      }
      catch (Exception ex)
      {
        MySqlSourceTrace.WriteAppErrorToLog(ex);
        string errorTitle = string.Format(Resources.TableDataAdditionErrorTitle, OperationType.IsForExport() ? "exporting" : "appending");
        MiscUtilities.ShowCustomizedErrorDialog(errorTitle, ex.Message + Environment.NewLine + Environment.NewLine + ex.StackTrace, false);
        return false;
      }

      return true;
    }

    /// <summary>
    /// Creates data rows, fills them with the given Excel data and sets column properties automatically given user options.
    /// </summary>
    /// <param name="dataRange">Excel data range containing the data to fill the table.</param>
    /// <param name="recreateColumnsFromData">Flag indicating if any existing columns in the table must be dropped and re-created based on the given data range.</param>
    /// <param name="emptyColumnsToVarchar">Flag indicating if the data type for columns with no data is automatically set to varchar(255).</param>
    /// <return><c>true</c> if the columns setup is successful, <c>false</c> otherwise.</return>
    public bool SetupColumnsWithData(Excel.Range dataRange, bool recreateColumnsFromData, bool emptyColumnsToVarchar)
    {
      Clear();
      int numCols = dataRange.Columns.Count;
      List<string> colsToDelete = new List<string>(numCols);

      // Create a list of boolean values that state if each column has any data or none.
      var columnsHaveAnyDataList = dataRange.GetColumnsWithDataInfoList(AddPrimaryKeyColumn);

      // Drop all columns and re-create them or create them if none have been created so far.
      if (recreateColumnsFromData || Columns.Count == 0)
      {
        CreateColumns(numCols);
      }

      // Set the IsEmpty and Exclude properties of columns based on the filling data
      List<bool> columnsContainDatesList = new List<bool>(numCols + 1);
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

      // Add the Excel data to rows in this table.
      if (!AddExcelData(dataRange, false, columnsHaveAnyDataList))
      {
        return false;
      }

      // Automatically detect the excelData type for columns based on their data.
      if (DetectDatatype)
      {
        DetectTypes(dataRange, emptyColumnsToVarchar);
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
      if (AutoAllowEmptyNonIndexColumns)
      {
        foreach (MySqlDataColumn mysqlCol in Columns)
        {
          mysqlCol.AllowNull = !mysqlCol.CreateIndex;
        }
      }

      return true;
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
    /// Gets the row type.
    /// </summary>
    /// <remarks>This API supports the .NET Framework infrastructure and is not intended to be used directly from your code.</remarks>
    /// <returns>Returns the type of the <see cref="DataRow"/>.</returns>
    protected override Type GetRowType()
    {
      return typeof(MySqlDataRow);
    }

    /// <summary>
    /// Creates a new row from an existing row.
    /// </summary>
    /// <param name="builder">A <see cref="DataRowBuilder"/> object.</param>
    /// <returns>A <see cref="DataRow"/> derived class.</returns>
    protected override DataRow NewRowFromBuilder(DataRowBuilder builder)
    {
      return new MySqlDataRow(builder);
    }

    /// <summary>
    /// Raises the <see cref="DataTable.RowChanged"/> event.
    /// </summary>
    /// <param name="e">Event arguments.</param>
    protected override void OnRowChanged(DataRowChangeEventArgs e)
    {
      base.OnRowChanged(e);
      if (_copyingTableData)
      {
        return;
      }

      RowWasChangedOrDeleted(e);
    }

    /// <summary>
    /// Raises the <see cref="DataTable.RowDeleted"/> event.
    /// </summary>
    /// <param name="e">Event arguments.</param>
    protected override void OnRowDeleted(DataRowChangeEventArgs e)
    {
      base.OnRowDeleted(e);
      RowWasChangedOrDeleted(e);
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
      if (filledTable == null)
      {
        return;
      }

      _copyingTableData = true;
      try
      {
        foreach (DataRow dr in filledTable.Rows)
        {
          ImportRow(dr);
        }
      }
      catch (Exception ex)
      {
        MiscUtilities.ShowCustomizedErrorDialog(Resources.TableDataCopyErrorTitle, ex.Message + Environment.NewLine + Environment.NewLine + ex.StackTrace, true);
        MySqlSourceTrace.WriteAppErrorToLog(ex);
      }

      _copyingTableData = false;
    }

    /// <summary>
    /// Copies the schema and data contents of the given <see cref="DataTable"/> object to this table.
    /// </summary>
    /// <param name="filledTable"><see cref="DataTable"/> object containing previously retrieved data from a MySQL table.</param>
    /// <param name="schemaOnly">Flag indicating whether only the schema is copied without data.</param>
    private void CopyTableSchemaAndData(DataTable filledTable, bool schemaOnly = false)
    {
      DataTable columnsInfoTable = filledTable.GetSchemaInfo();
      CreateTableSchema(columnsInfoTable, true);
      if (!schemaOnly)
      {
        CopyTableData(filledTable);
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
        MySqlDataColumn column = new MySqlDataColumn(OperationType.IsForExport())
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
      DataTable columnsInfoTable = WbConnection.GetSchemaCollection("Columns Short", null, WbConnection.Schema, tableName);
      CreateTableSchema(columnsInfoTable, datesAsMySqlDates);
    }

    /// <summary>
    /// Creates columns for this table using the information schema of a MySQL table with the given name to mirror their properties.
    /// </summary>
    /// <param name="schemaInfoTable">Table with schema information.</param>
    /// <param name="datesAsMySqlDates">Flag indicating if the dates are stored in the table as <see cref="System.DateTime"/> or <see cref="MySql.Data.Types.MySqlDateTime"/> objects.</param>
    private void CreateTableSchema(DataTable schemaInfoTable, bool datesAsMySqlDates)
    {
      if (schemaInfoTable == null)
      {
        return;
      }

      Columns.Clear();
      foreach (DataRow columnInfoRow in schemaInfoTable.Rows)
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
    /// Analyzes the given Excel data by columns and automatically detects the table columns data types.
    /// </summary>
    /// <param name="dataRange">The Excel range containing data to detect data types from.</param>
    /// <param name="emptyColumnsToVarchar">Flag indicating if the data type for columns with no data is automatically set to varchar(255).</param>
    private void DetectTypes(Excel.Range dataRange, bool emptyColumnsToVarchar)
    {
      object[,] excelData = dataRange.ToBidimensionalArray(IsFormatted);
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
                if (valueAsString.IsMySqlZeroDateValue())
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
    /// Raises the <see cref="PropertyChanged"/> event.
    /// </summary>
    /// <param name="propertyName">The name of the property whose value changed.</param>
    private void OnPropertyChanged(string propertyName)
    {
      if (PropertyChanged != null)
      {
        PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
      }
    }

    /// <summary>
    /// Notifies a <see cref="MySqlDataRow"/> that it has been modified or deleted.
    /// </summary>
    /// <param name="args">Event arguments.</param>
    private void RowWasChangedOrDeleted(DataRowChangeEventArgs args)
    {
      if (OperationType.IsForImport() || !(args.Row is MySqlDataRow))
      {
        return;
      }

      MySqlDataRow mySqlRow = args.Row as MySqlDataRow;
      mySqlRow.RowChanged(args.Action);
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
      bool warningsChanged = UpdateWarnings(emptyTableName, Resources.TableNameRequiredWarning);
      IsTableNameValid = !emptyTableName;

      // Update warning stating a table with the given name already exists in the database
      if (IsTableNameValid && WbConnection != null)
      {
        warningsChanged = UpdateWarnings(TableExistsInSchema, Resources.TableNameExistsWarning) || warningsChanged;
        IsTableNameValid = !TableExistsInSchema;
      }

      // Update warning stating the table name cannot be empty
      if (IsTableNameValid)
      {
        bool nonStandardTableName = TableName.Contains(" ") || TableName.Any(char.IsUpper);
        warningsChanged = UpdateWarnings(nonStandardTableName, Resources.NamesWarning) || warningsChanged;
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
        col.SetDisplayName(_firstRowIsHeaders ? row[i].ToString().ToValidMySqlColumnName() : col.ColumnName);
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