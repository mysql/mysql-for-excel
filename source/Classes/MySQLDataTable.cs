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
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using MySQL.ForExcel.Forms;
using MySQL.ForExcel.Interfaces;
using MySQL.ForExcel.Properties;
using MySQL.Utility.Classes;
using MySQL.Utility.Classes.MySQLWorkbench;
using MySQL.Utility.Forms;
using Excel = Microsoft.Office.Interop.Excel;
using ExcelTools = Microsoft.Office.Tools.Excel;

namespace MySQL.ForExcel.Classes
{
  /// <summary>
  /// Represents an in-memory table for a corresponding MySQL database table.
  /// </summary>
  public class MySqlDataTable : DataTable
  {
    #region Constants

    /// <summary>
    /// Number of characters that the static part of a SQL query may occupy.
    /// </summary>
    public const int PRE_SQL_PADDING_LENGTH = 20;

    /// <summary>
    /// Bytes to subtract from the maximum allowed packet size to build a query that is safely processed by the database server.
    /// </summary>
    public const int SAFE_BYTES_BEFORE_REACHING_MAX_ALLOWED_PACKET = 10;

    #endregion Constants

    #region Fields

    /// <summary>
    /// Flag indicating whether an auto-generated primary key column will be added as the first column in the table.
    /// </summary>
    private bool _addPrimaryKeyColumn;

    /// <summary>
    /// List of text strings containing warnings for users about the auto-generated primary key.
    /// </summary>
    private readonly List<string> _autoPkWarningTextsList;

    /// <summary>
    /// Flag indicating if the column names where changed to use the first row of data.
    /// </summary>
    private bool _changedColumnNamesWithFirstRowOfData;

    /// <summary>
    /// The number of changed or deleted rows in this table. If less than 0 it means we want to recalculate its value.
    /// </summary>
    private int _changedOrDeletedRows;

    /// <summary>
    /// The combined length of data representation as text for all columns.
    /// </summary>
    private long _columnsDataLength;

    /// <summary>
    /// Array of <see cref="MySqlDataColumn"/> objects to be used in INSERT queries.
    /// </summary>
    private MySqlDataColumn[] _columnsForInsertion;

    /// <summary>
    /// Flag indicating if data is being copied from a regular <see cref="DataTable"/> object.
    /// </summary>
    private bool _copyingTableData;

    /// <summary>
    /// Flag indicating whether during an Export operation only the table will be created without any data.
    /// </summary>
    private bool _createTableWithoutData;

    /// <summary>
    /// Flag indicating whether data type for each column is automatically detected when data is loaded by the <see cref="SetupColumnsWithData"/> method.
    /// </summary>
    private bool _detectDatatype;

    /// <summary>
    /// The default DB engine used for new table creations.
    /// </summary>
    private string _engine;

    /// <summary>
    /// Flag indicating if the first row in the Excel region to be exported contains the column names of the MySQL table that will be created.
    /// </summary>
    private bool _firstRowIsHeaders;

    /// <summary>
    /// An approximation for a maximum SQL query length.
    /// </summary>
    private int _maxQueryLength;

    /// <summary>
    /// Gets an approximation for a maximum SQL quey length containing primary key column data only.
    /// </summary>
    private int _maxQueryForPrimaryColumnsLength;

    /// <summary>
    /// Contains the value of the max_allowed_packet system variable of the MySQL Server currently connected to.
    /// </summary>
    private ulong _mysqlMaxAllowedPacket;

    /// <summary>
    /// The <see cref="StringBuilder"/> used to build pre-SQL query text.
    /// </summary>
    private StringBuilder _preSqlBuilder;

    /// <summary>
    /// The static piece of an INSERT SQL query that does not change from row to row containing schema, table and column names.
    /// </summary>
    private string _preSqlForAddedRows;

    /// <summary>
    /// The length of the static part of the SQL queries made up of schema, table and column names plus the SQL statement keywords.
    /// </summary>
    private int _preSqlLength;

    /// <summary>
    /// Gets an array of <see cref="MySqlDataColumn"/> objects that compose the primary key of this table.
    /// </summary>
    private MySqlDataColumn[] _primaryKeyColumns;

    /// <summary>
    /// Gets the combined length of data representation as text for primary key columns.
    /// </summary>
    private long _primaryKeyColumnsDataLength;

    /// <summary>
    /// The SELECT query used to retrieve the excelData from the corresponding MySQL table to fill this one.
    /// </summary>
    private string _selectQuery;

    /// <summary>
    /// The <see cref="StringBuilder"/> used to build SQL DELETE queries text.
    /// </summary>
    private StringBuilder _sqlBuilderForDelete;

    /// <summary>
    /// The <see cref="StringBuilder"/> used to build SQL INSERT queries text.
    /// </summary>
    private StringBuilder _sqlBuilderForInsert;

    /// <summary>
    /// The <see cref="StringBuilder"/> used to build SQL UPDATE queries text.
    /// </summary>
    private StringBuilder _sqlBuilderForUpdate;

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
    /// <param name="detectDataType">Flag indicating if the data type for each column is automatically detected when data is loaded by the <see cref="SetupColumnsWithData"/> method.</param>
    /// <param name="addBufferToVarchar">Flag indicating if columns with an auto-detected varchar type will get a padding buffer for its size.</param>
    /// <param name="autoIndexIntColumns">Flag indicating if columns with an integer-based data-type will have their <see cref="MySqlDataColumn.CreateIndex"/> property value set to true.</param>
    /// <param name="autoAllowEmptyNonIndexColumns">Flag indicating if columns that have their <see cref="MySqlDataColumn.CreateIndex"/> property value
    /// set to <c>false</c> will automatically get their <see cref="MySqlDataColumn.AllowNull"/> property value set to <c>true</c>.</param>
    /// <param name="wbConnection">MySQL Workbench connection to a MySQL server instance selected by users.</param>
    public MySqlDataTable(string schemaName, string proposedTableName, bool addPrimaryKeyCol, bool useFormattedValues, bool detectDataType, bool addBufferToVarchar, bool autoIndexIntColumns, bool autoAllowEmptyNonIndexColumns, MySqlWorkbenchConnection wbConnection)
      : this(schemaName, proposedTableName)
    {
      AddBufferToVarchar = addBufferToVarchar;
      AddPrimaryKeyColumn = addPrimaryKeyCol;
      AutoAllowEmptyNonIndexColumns = autoAllowEmptyNonIndexColumns;
      AutoIndexIntColumns = autoIndexIntColumns;
      DetectDatatype = detectDataType;
      IsFormatted = useFormattedValues;
      OperationType = DataOperationType.Export;
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
      _changedOrDeletedRows = -1;
      _columnsForInsertion = null;
      _columnsDataLength = 0;
      _copyingTableData = false;
      _createTableWithoutData = false;
      _detectDatatype = false;
      _engine = null;
      _maxQueryLength = 0;
      _maxQueryForPrimaryColumnsLength = 0;
      _mysqlMaxAllowedPacket = 0;
      _preSqlBuilder = null;
      _preSqlForAddedRows = null;
      _preSqlLength = 0;
      _primaryKeyColumns = null;
      _primaryKeyColumnsDataLength = 0;
      _tableExistsInSchema = null;
      _tableWarningsTextList = new List<string>(3);
      _selectQuery = string.Format("SELECT * FROM `{0}`", TableNameForSqlQueries);
      _sqlBuilderForDelete = null;
      _sqlBuilderForInsert = null;
      _sqlBuilderForUpdate = null;
      _useOptimisticUpdate = false;
      AddBufferToVarchar = false;
      AddPrimaryKeyColumn = false;
      AutoAllowEmptyNonIndexColumns = false;
      AutoIndexIntColumns = false;
      FirstRowIsHeaders = false;
      IsTableNameValid = !string.IsNullOrEmpty(TableName);
      IsFormatted = false;
      IsPreviewTable = false;
      OperationType = DataOperationType.Import;
      SchemaName = string.Empty;
      UseFirstColumnAsPk = false;
      WbConnection = null;
    }

    #region Enums

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

    /// <summary>
    /// Specifies identifiers to indicate the type of index created for a table.
    /// </summary>
    public enum IndexType
    {
      /// <summary>
      /// A regular index.
      /// </summary>
      Index,

      /// <summary>
      /// A primary key index.
      /// </summary>
      Primary,

      /// <summary>
      /// A unique index.
      /// </summary>
      Unique
    }

    /// <summary>
    /// Specifies identifiers to indicate the type of SQL statement created for a new <see cref="MySqlDataTable"/>.
    /// </summary>
    public enum NewTableSqlType
    {
      /// <summary>
      /// An ALTER TABLE statement affecting the columns definition and its keys.
      /// </summary>
      AlterComplete,

      /// <summary>
      /// An ALTER TABLE statement affecting only its secondary keys.
      /// </summary>
      AlterOnlyKeys,

      /// <summary>
      /// A CREATE TABLE statement affecting the columns definition and its secondary keys.
      /// </summary>
      CreateWithKeys,

      /// <summary>
      /// A CREATE TABLE statement affecting only the columns definition and its primary key.
      /// </summary>
      CreateWithoutKeys,

      /// <summary>
      /// No SQL statement is created.
      /// </summary>
      None
    }

    #endregion Enums

    #region Properties

    /// <summary>
    /// Gets a value indicating whether columns with an auto-detected varchar type will get a padding buffer for its size.
    /// </summary>
    public bool AddBufferToVarchar { get; private set; }

    /// <summary>
    /// Gets a value indicating whether an auto-generated primary key column will be added as the first column in the table.
    /// </summary>
    public bool AddPrimaryKeyColumn
    {
      get { return _addPrimaryKeyColumn; }

      private set
      {
        _addPrimaryKeyColumn = value;
        _columnsForInsertion = null;
      }
    }

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
    /// Gets the number of changed or deleted rows in this table.
    /// </summary>
    public int ChangedOrDeletedRows
    {
      get
      {
        if (_changedOrDeletedRows < 0)
        {
          var changesTable = GetChanges();
          _changedOrDeletedRows = changesTable != null ? changesTable.Rows.Count : 0;
        }

        return _changedOrDeletedRows;
      }
    }

    /// <summary>
    /// Gets the combined length of data representation as text for all columns.
    /// </summary>
    public long ColumnsDataLength
    {
      get
      {
        if (_columnsDataLength == 0)
        {
          _columnsDataLength = Columns.Cast<MySqlDataColumn>().Sum(col => col.MySqlDataTypeLength);
          _maxQueryLength = 0;
        }

        return _columnsDataLength;
      }
    }

    /// <summary>
    /// Gets an array of <see cref="MySqlDataColumn"/> objects to be used in INSERT queries.
    /// </summary>
    public MySqlDataColumn[] ColumnsForInsertion
    {
      get
      {
        return _columnsForInsertion ?? (_columnsForInsertion = Columns.Cast<MySqlDataColumn>()
          .Where(column => column.IncludeForInsertion).ToArray());
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
    /// Gets the default DB engine used for new table creations.
    /// </summary>
    public string Engine
    {
      get
      {
        if (string.IsNullOrEmpty(_engine))
        {
          _engine = WbConnection.GetMySqlServerDefaultEngine();
        }

        return _engine;
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
        var adjustOperationsValue = _firstRowIsHeaders && OperationType.IsForExport() ? 1 : 0;
        return changesDt != null ? changesDt.Rows.Count - adjustOperationsValue : 0;
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
    /// Gets a value indicating whether the Excel Data used to populate this table is formatted (numbers, dates, text) or not (numbers and text).
    /// </summary>
    public bool IsFormatted { get; private set; }

    /// <summary>
    /// Gets or sets a value indicating whether the table is used for preview purposes and row value changes are not monitored to trigger SQL queries regeneration.
    /// </summary>
    public bool IsPreviewTable { get; set; }

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
    /// Gets an approximation for a maximum SQL query length.
    /// </summary>
    public int MaxQueryLength
    {
      get
      {
        if (_maxQueryLength == 0)
        {
          long maxSize = ColumnsDataLength + (DataTypeUtilities.MYSQL_DB_OBJECTS_MAX_LENGTH * 3);
          _maxQueryLength = (int)Math.Min(maxSize, int.MaxValue);
          _sqlBuilderForInsert = null;
          _sqlBuilderForUpdate = null;
        }

        return _maxQueryLength;
      }
    }

    /// <summary>
    /// Gets an approximation for a maximum SQL quey length containing primary key column data only.
    /// </summary>
    public int MaxQueryForPrimaryColumnsLength
    {
      get
      {
        if (_maxQueryForPrimaryColumnsLength == 0)
        {
          long maxSize = PrimaryKeyColumnsDataLength + (DataTypeUtilities.MYSQL_DB_OBJECTS_MAX_LENGTH * 3);
          _maxQueryForPrimaryColumnsLength = (int)Math.Min(maxSize, int.MaxValue);
          _sqlBuilderForDelete = null;
        }

        return _maxQueryForPrimaryColumnsLength;
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
          var serverMaxallowedPacket = WbConnection.GetMySqlServerMaxAllowedPacket();
          _mysqlMaxAllowedPacket = serverMaxallowedPacket > 0 ? serverMaxallowedPacket - SAFE_BYTES_BEFORE_REACHING_MAX_ALLOWED_PACKET : 0;
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
    /// Gets an array of <see cref="MySqlDataColumn"/> objects that compose the primary key of this table.
    /// </summary>
    public MySqlDataColumn[] PrimaryKeyColumns
    {
      get
      {
        return _primaryKeyColumns ??
               (_primaryKeyColumns = Columns.Cast<MySqlDataColumn>().Where(pkCol => pkCol.PrimaryKey).ToArray());
      }
    }

    /// <summary>
    /// Gets the combined length of data representation as text for primary key columns.
    /// </summary>
    public long PrimaryKeyColumnsDataLength
    {
      get
      {
        if (_primaryKeyColumnsDataLength == 0)
        {
          _primaryKeyColumnsDataLength = Columns.Cast<MySqlDataColumn>().Where(pkCol => pkCol.PrimaryKey).Sum(col => col.MySqlDataTypeLength);
        }

        return _primaryKeyColumnsDataLength;
      }
    }

    /// <summary>
    /// Gets the static piece of an INSERT SQL query that does not change from row to row containing schema, table and column names.
    /// </summary>
    public string PreSqlForAddedRows
    {
      get
      {
        if (string.IsNullOrEmpty(_preSqlForAddedRows))
        {
          _preSqlForAddedRows = GetPreSqlForAddedRows();
        }

        return _preSqlForAddedRows;
      }
    }

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
    /// The <see cref="StringBuilder"/> used to build SQL DELETE queries text.
    /// </summary>
    public StringBuilder SqlBuilderForDelete
    {
      get { return _sqlBuilderForDelete ?? (_sqlBuilderForDelete = new StringBuilder(MaxQueryForPrimaryColumnsLength)); }
    }

    /// <summary>
    /// The <see cref="StringBuilder"/> used to build SQL INSERT queries text.
    /// </summary>
    public StringBuilder SqlBuilderForInsert
    {
      get { return _sqlBuilderForInsert ?? (_sqlBuilderForInsert = new StringBuilder(MaxQueryLength)); }
    }

    /// <summary>
    /// The <see cref="StringBuilder"/> used to build SQL UPDATE queries text.
    /// </summary>
    public StringBuilder SqlBuilderForUpdate
    {
      get
      {
        int maxLen = Math.Min(MaxQueryLength * 2, int.MaxValue);
        return _sqlBuilderForUpdate ?? (_sqlBuilderForUpdate = new StringBuilder(maxLen));
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

        ResetAutoPkcolumnName();
        UpdateTableSelectQuery();
        UpdateTableNameWarningsAndSelectQuery();
        _preSqlLength = 0;
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
        _columnsForInsertion = null;
        _preSqlForAddedRows = null;
        if (!AddPrimaryKeyColumn)
        {
          return;
        }

        if (Columns.Count > 0)
        {
          GetColumnAtIndex(0).ExcludeColumn = !_useFirstColumnAsPk;
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

    /// <summary>
    /// The <see cref="StringBuilder"/> used to build SQL query text.
    /// </summary>
    private StringBuilder PreSqlBuilder
    {
      get { return _preSqlBuilder ?? (_preSqlBuilder = new StringBuilder(PreSqlLength)); }
    }

    /// <summary>
    /// The length of the static part of the SQL queries made up of schema, table and column names plus the SQL statement keywords.
    /// </summary>
    private int PreSqlLength
    {
      get
      {
        if (_preSqlLength == 0)
        {
          _preSqlLength = GetPreSqlLength();
          _preSqlBuilder = null;
        }

        return _preSqlLength;
      }
    }

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
    /// <param name="autoPkCreationOnlyIfFirstColumnIsPk">Flag indicating whether an Auto PK column is prepended only if the value of the <see cref="UseFirstColumnAsPk"/> property is <c>strue</c>.</param>
    /// <param name="subscribePropertyChangedEvent">Flag indicating whether the cloned columns subscribe to the parent table's <see cref="ColumnPropertyValueChanged"/> event.</param>
    /// <returns>Cloned <see cref="MySqlDataTable"/> object.</returns>
    public MySqlDataTable CloneSchema(bool autoPkCreationOnlyIfFirstColumnIsPk, bool subscribePropertyChangedEvent)
    {
      bool createAutoPkColumn = autoPkCreationOnlyIfFirstColumnIsPk ? UseFirstColumnAsPk : AddPrimaryKeyColumn;
      MySqlDataTable clonedTable = new MySqlDataTable(
        SchemaName,
        TableName,
        createAutoPkColumn,
        IsFormatted,
        DetectDatatype,
        AddBufferToVarchar,
        AutoIndexIntColumns,
        AutoAllowEmptyNonIndexColumns,
        WbConnection)
      {
        UseFirstColumnAsPk = UseFirstColumnAsPk,
        IsFormatted = IsFormatted,
        FirstRowIsHeaders = FirstRowIsHeaders,
        IsPreviewTable = IsPreviewTable,
        OperationType = OperationType,
        UseOptimisticUpdate = UseOptimisticUpdate
      };

      foreach (var clonedColumn in from MySqlDataColumn column in Columns where !column.AutoPk || createAutoPkColumn select column.CloneSchema())
      {
        if (subscribePropertyChangedEvent)
        {
          clonedColumn.PropertyChanged += clonedTable.ColumnPropertyValueChanged;
        }

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
    /// Creates an Excel table from a given <see cref="Excel.Range"/> object.
    /// </summary>
    /// <param name="range">A <see cref="Excel.Range"/> object.</param>
    /// <param name="excelTableName">The proposed name for the new Excel table.</param>
    /// <param name="containsColumnNames">Flag indicating whether column names appear in the first row of the Excel range.</param>
    public Excel.ListObject CreateExcelTable(Excel.Range range, string excelTableName, bool containsColumnNames)
    {
      if (range == null)
      {
        return null;
      }

      var worksheet = Globals.Factory.GetVstoObject(range.Worksheet);
      var workbook = worksheet.Parent as Excel.Workbook;
      if (workbook == null)
      {
        return null;
      }

      excelTableName = excelTableName.GetExcelTableNameAvoidingDuplicates();
      var workbookName = Globals.ThisAddIn.Application.ActiveWorkbook.Name;
      var commandText = string.Format("{0}!{1}", workbookName, excelTableName);
      var connectionName = @"WorksheetConnection_" + commandText;
      var connectionString = "WORKSHEET;" + workbookName;
      var hasHeaders = containsColumnNames ? Excel.XlYesNoGuess.xlYes : Excel.XlYesNoGuess.xlNo;
      var namedTable = range.Worksheet.ListObjects.Add(Excel.XlListObjectSourceType.xlSrcExternal, ExcelUtilities.DUMMY_WORKBOOK_CONNECTION_STRING, false, hasHeaders, range);
      namedTable.Name = excelTableName;
      namedTable.DisplayName = excelTableName;
      namedTable.TableStyle = Settings.Default.ImportExcelTableStyleName;
      namedTable.QueryTable.BackgroundQuery = false;
      namedTable.QueryTable.CommandText = commandText;
      var excelTable = Globals.Factory.GetVstoObject(namedTable);
      excelTable.SetDataBinding(this);
      foreach (MySqlDataColumn col in Columns)
      {
        excelTable.ListColumns[col.Ordinal + 1].Name = col.DisplayName;
      }

      excelTable.Range.Columns.AutoFit();

      // Add a connection to the Workbook, the method used to add it differs since the Add method is obsolete for Excel 2013 and higher.
      if (Globals.ThisAddIn.ExcelVersionNumber < ThisAddIn.EXCEL_2013_VERSION_NUMBER)
      {
        workbook.Connections.Add(connectionName, string.Empty, connectionString, commandText, Excel.XlCmdType.xlCmdExcel);
      }
      else
      {
        workbook.Connections.Add2(connectionName, string.Empty, connectionString, commandText, Excel.XlCmdType.xlCmdExcel, true, false);
      }
      
      return namedTable;
      
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
    /// Creates a SQL query to disable the foreign key constraints.
    /// </summary>
    /// <remarks>Only useful for InnoDB tables.</remarks>
    /// <param name="disable">Flag indicating if the query is disabling or enabling foreign key constraints.</param>
    /// <returns>ALTER TABLE SQL query disabling or enabling foreign key constraints.</returns>
    public string GetDisableForeignKeysSql(bool disable)
    {
      return string.Format("SET foreign_key_checks = {0}", (disable ? "0" : "1"));
    }

    /// <summary>
    /// Creates a SQL query to alter a table in the database disabling or enabling its keys.
    /// </summary>
    /// <remarks>Only useful for MyISAM tables.</remarks>
    /// <param name="disable">Flag indicating if the query is disabling or enabling keys.</param>
    /// <returns>ALTER TABLE SQL query disabling or enabling keys.</returns>
    public string GetDisableKeysSql(bool disable)
    {
      return string.Format("ALTER TABLE `{0}`.`{1}` {2} KEYS", SchemaName, TableName, (disable ? "DISABLE" : "ENABLE"));
    }

    /// <summary>
    /// Creates a SQL query to disable the foreign key constraints.
    /// </summary>
    /// <remarks>Only useful for InnoDB tables.</remarks>
    /// <param name="disable">Flag indicating if the query is disabling or enabling unique key constraints.</param>
    /// <returns>ALTER TABLE SQL query disabling or enabling foreign key constraints.</returns>
    public string GetDisableUniqueKeysSql(bool disable)
    {
      return string.Format("SET unique_checks = {0}", (disable ? "0" : "1"));
    }

    /// <summary>
    /// Creates a SQL query to lock or unlock the table.
    /// </summary>
    /// <param name="lockTable">Flag indicating if the query is locking or unlocking the table.</param>
    /// <returns>SQL query locking or unlocking the table.</returns>
    public string GeLockTableSql(bool lockTable)
    {
      return lockTable
        ? string.Format("{0} `{1}`.`{2}` WRITE", MySqlStatement.STATEMENT_LOCK_TABLES, SchemaName, TableName)
        : MySqlStatement.STATEMENT_UNLOCK_TABLES;
    }

    /// <summary>
    /// Creates a SQL query to create a new table in the database based on this table's schema information.
    /// </summary>
    /// <param name="formatNewLinesAndTabs">Flag indicating if the SQL statement must be formatted to insert new line and tab characters for display purposes.</param>
    /// <param name="sqlType">The type of SQL statement used for the new table.</param>
    /// <returns>The SQL statement to create or alter a new table.</returns>
    public string GetNewTableSql(bool formatNewLinesAndTabs, NewTableSqlType sqlType)
    {
      bool createTable = sqlType == NewTableSqlType.CreateWithKeys || sqlType == NewTableSqlType.CreateWithoutKeys;
      if (!createTable && Columns.OfType<MySqlDataColumn>().Count(col => col.CreateIndex || col.UniqueKey) == 0)
      {
        return null;
      }

      StringBuilder sql = new StringBuilder(MiscUtilities.STRING_BUILDER_DEFAULT_CAPACITY);
      string nl = formatNewLinesAndTabs ? Environment.NewLine : " ";
      string nlt = formatNewLinesAndTabs ? Environment.NewLine + "   " : " ";
      sql.Append(createTable ? MySqlStatement.STATEMENT_CREATE_TABLE : MySqlStatement.STATEMENT_ALTER_TABLE);
      sql.AppendFormat(" `{0}`.`{1}`", SchemaName, TableName);
      if (createTable)
      {
        sql.Append(nl);
        sql.Append("(");
      }

      string delimiter = nlt;
      // Consider adding later on code to correctly assemble an ALTER statement, since for a NewTableSqlType.AlterComplete
      // the ALTER statement is not 100% correct, but the NewTableSqlType.AlterComplete is not yet used throughout the code.
      if (sqlType != NewTableSqlType.AlterOnlyKeys)
      {
        foreach (var col in Columns.OfType<MySqlDataColumn>())
        {
          // Skipping excluded columns is better than simplifying the foreach using a .Where LINQ form since using it will cause
          // traversing the list of columns 2 times in most of the cases where none or just a very few columns are excluded.
          if (col.ExcludeColumn)
          {
            continue;
          }

          sql.AppendFormat("{0}{1}", delimiter, col.GetSql());
          delimiter = "," + nlt;
        }

        sql.Append(GetIndexesSqlPiece(sqlType, IndexType.Primary, ref delimiter));
      }

      if (sqlType != NewTableSqlType.CreateWithoutKeys)
      {
        sql.Append(GetIndexesSqlPiece(sqlType, IndexType.Unique, ref delimiter));
        sql.Append(GetIndexesSqlPiece(sqlType, IndexType.Index, ref delimiter));
      }

      if (createTable)
      {
        sql.Append(nl);
        sql.Append(")");
      }

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
    /// Gets a list of <see cref="MySqlDummyRow"/> objects containing statements related to the table creation or to performance optimizations.
    /// </summary>
    /// <param name="beforeInserts">Flag indicating if the SQL statements will appear before the INSERT statements.</param>
    /// <returns>A list of <see cref="MySqlDummyRow"/> objects containing statements related to the table creation or to performance optimizations.</returns>
    public IList<MySqlDummyRow> GetTableDummyRows(bool beforeInserts)
    {
      MySqlDummyRow dummyRow;
      bool newTable = OperationType.IsForExport();

      // Set the type of SQL statement generated by the GetNewTableSql method, notice that there is a difference on the type depending on value
      // of the beforeInserts paramenter, meaning the query is executed before or after INSERT statements. The ones before normally disable keys
      // or create a new table without any keys, the ones after enable back the keys or add the keys for a new table.
      var sqlType = newTable
        ? (CreateTableWithoutData || !Settings.Default.ExportSqlQueriesCreateIndexesLast
          ? (beforeInserts ? NewTableSqlType.CreateWithKeys : NewTableSqlType.None)
          : (beforeInserts ? NewTableSqlType.CreateWithoutKeys : NewTableSqlType.AlterOnlyKeys))
        : NewTableSqlType.None;
      if (newTable && sqlType == NewTableSqlType.None)
      {
        return null;
      }

      var dummyRowsList = new List<MySqlDummyRow>(3);
      if (newTable && sqlType != NewTableSqlType.None)
      {
        string createOrAlterTableSql = GetNewTableSql(true, sqlType);
        if (!string.IsNullOrEmpty(createOrAlterTableSql))
        {
          dummyRow = new MySqlDummyRow(createOrAlterTableSql);
          dummyRowsList.Add(dummyRow);
        }
      }

      if (newTable || !Settings.Default.AppendSqlQueriesDisableIndexes)
      {
        return dummyRowsList;
      }

      switch (Engine)
      {
        case "InnoDB":
          dummyRow = new MySqlDummyRow(GetDisableUniqueKeysSql(beforeInserts));
          dummyRowsList.Add(dummyRow);
          dummyRow = new MySqlDummyRow(GetDisableForeignKeysSql(beforeInserts));
          dummyRowsList.Add(dummyRow);
          break;

        case "MyISAM":
          dummyRow = new MySqlDummyRow(GetDisableKeysSql(beforeInserts));
          dummyRowsList.Add(dummyRow);
          dummyRow = new MySqlDummyRow(GeLockTableSql(beforeInserts));
          if (beforeInserts)
          {
            dummyRowsList.Insert(0, dummyRow);
          }
          else
          {
            dummyRowsList.Add(dummyRow);
          }

          break;
      }

      return dummyRowsList;
    }

    /// <summary>
    /// Imports data contained in the given <see cref="MySqlDataTable"/> object at the active Excel cell.
    /// </summary>
    /// <param name="importColumnNames">Flag indicating if column names will be imported as the first row of imported data.</param>
    /// <param name="createExcelTable">Flag indicating whether an Excel table is created for the imported data.</param>
    /// <returns>The <see cref="Excel.Range"/> or <see cref="Excel.ListObject"/> containing the cells with the imported data.</returns>
    public object ImportDataAtActiveExcelCell(bool importColumnNames, bool createExcelTable)
    {
      var atCell = Globals.ThisAddIn.Application.ActiveCell;
      object retObj = null;
      if (createExcelTable)
      {
        retObj = ImportDataIntoExcelTable(importColumnNames, atCell);
      }
      else
      {
        retObj = ImportDataIntoExcelRange(importColumnNames, atCell);
      }

      return retObj;
    }

    /// <summary>
    /// Imports the table's data at the specified Excel cell into a plain <see cref="Excel.Range"/>.
    /// </summary>
    /// <param name="importColumnNames">Flag indicating if column names will be imported as the first row of imported data.</param>
    /// <param name="atCell">The starting Excel (left-most and top-most) cell where the imported data is placed.</param>
    /// <returns>The <see cref="Excel.Range"/> containing the cells with the imported data.</returns>
    public Excel.Range ImportDataIntoExcelRange(bool importColumnNames, Excel.Range atCell)
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
        Excel.Workbook activeWorkbook = atCell.Worksheet.Parent as Excel.Workbook;
        int cappedNumRows = activeWorkbook != null && activeWorkbook.Excel8CompatibilityMode
          ? Math.Min(rowsCount, UInt16.MaxValue - currentRow)
          : rowsCount;
        fillingRange = atCell.Resize[cappedNumRows, Columns.Count];

        // Check if the data being imported does not overlap with the data of an existing Excel table.
        if (fillingRange.IntersectsWithAnyExcelTable())
        {
          if (
            InfoDialog.ShowYesNoDialog(InfoDialog.InfoType.Warning, Resources.ImportOverExcelTableErrorTitle,
              Resources.ImportOverExcelTableErrorDetail, Resources.ImportOverExcelTableErrorSubDetail) ==
            DialogResult.No)
          {
            return null;
          }

          var newWorkSheet = activeWorkbook.CreateWorksheet(TableName, true);
          if (newWorkSheet == null)
          {
            return null;
          }

          Excel.Range newWorkSheetCell = newWorkSheet.Range["A1", Type.Missing];
          return ImportDataIntoExcelRange(importColumnNames, newWorkSheetCell);
        }

        bool escapeFormulaTexts = Settings.Default.ImportEscapeFormulaTextValues;
        var fillingArray = new object[cappedNumRows, Columns.Count];
        if (importColumnNames)
        {
          for (int currCol = 0; currCol < Columns.Count; currCol++)
          {
            fillingArray[0, currCol] = Columns[currCol].ColumnName;
          }
        }

        int fillingRowIdx = startingRow;
        cappedNumRows -= startingRow;
        for (int currRow = 0; currRow < cappedNumRows; currRow++)
        {
          var mySqlRow = Rows[currRow] as MySqlDataRow;
          if (mySqlRow == null)
          {
            continue;
          }

          for (int currCol = 0; currCol < Columns.Count; currCol++)
          {
            var importingValue = DataTypeUtilities.GetImportingValueForDateType(mySqlRow[currCol]);
            if (importingValue is string)
            {
              var importingValueText = importingValue as string;

              // If the imported value is a text that starts with an equal sign Excel will treat it as a formula so it needs to be escaped
              // prepending an apostrophe to it for Excel to treat it as standard text.
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

        Globals.ThisAddIn.SkipSelectedDataContentsDetection = true;
        Globals.ThisAddIn.Application.Goto(fillingRange, false);
        fillingRange.ClearFormats();
        fillingRange.Value = fillingArray;

        if (importColumnNames)
        {
          fillingRange.SetHeaderStyle();
        }

        fillingRange.Columns.AutoFit();
        fillingRange.Rows.AutoFit();
        atCell.Select();
      }
      catch (Exception ex)
      {
        MiscUtilities.ShowCustomizedErrorDialog(Resources.ImportDataErrorDetailText, ex.Message, true);
        MySqlSourceTrace.WriteAppErrorToLog(ex);
      }
      finally
      {
        Globals.ThisAddIn.SkipSelectedDataContentsDetection = false;
      }

      return fillingRange;
    }

    /// <summary>
    /// Imports the table's data at the specified Excel cell into a <see cref="Excel.ListObject"/>.
    /// </summary>
    /// <param name="importColumnNames">Flag indicating if column names will be imported as the first row of imported data.</param>
    /// <param name="atCell">The starting Excel (left-most and top-most) cell where the imported data is placed.</param>
    /// <returns>The created <see cref="Excel.ListObject"/> containing the imported data.</returns>
    public Excel.ListObject ImportDataIntoExcelTable(bool importColumnNames, Excel.Range atCell)
    {
      int startingRow = importColumnNames ? 1 : 0;
      int rowsCount = Rows.Count + startingRow;
      if (rowsCount == 0)
      {
        return null;
      }

      Excel.ListObject excelTable = null;
      try
      {
        Excel.Workbook activeWorkbook = atCell.Worksheet.Parent as Excel.Workbook;

        // Check if the data being imported does not overlap with the data of an existing Excel table.
        if (atCell.IntersectsWithAnyExcelTable())
        {
          if (
            InfoDialog.ShowYesNoDialog(InfoDialog.InfoType.Warning, Resources.ImportOverExcelTableErrorTitle,
              Resources.ImportOverExcelTableErrorDetail, Resources.ImportOverExcelTableErrorSubDetail) ==
            DialogResult.No)
          {
            return null;
          }

          var newWorkSheet = activeWorkbook.CreateWorksheet(TableName, true);
          if (newWorkSheet == null)
          {
            return null;
          }

          Excel.Range newWorkSheetCell = newWorkSheet.Range["A1", Type.Missing];
          return ImportDataIntoExcelTable(importColumnNames, newWorkSheetCell);
        }

        Globals.ThisAddIn.SkipSelectedDataContentsDetection = true;
        Globals.ThisAddIn.Application.Goto(atCell, false);

        // Create Excel Table for the imported data
        string excelTableName = ExcelUtilities.GetExcelTableNameAvoidingDuplicates(SchemaName, TableName);
        excelTable = CreateExcelTable(atCell, excelTableName, importColumnNames);
        atCell.Select();
      }
      catch (Exception ex)
      {
        MiscUtilities.ShowCustomizedErrorDialog(Resources.ImportDataErrorDetailText, ex.Message, true);
        MySqlSourceTrace.WriteAppErrorToLog(ex);
      }
      finally
      {
        Globals.ThisAddIn.SkipSelectedDataContentsDetection = false;
      }

      return excelTable;
    }

    /// <summary>
    /// Pushes all changes in this table's data to its corresponding database table.
    /// </summary>
    /// <param name="showMySqlScriptDialog">Flag indicating whether the <see cref="MySqlScriptDialog"/> is shown before applying the query.</param>
    /// <returns></returns>
    public List<IMySqlDataRow> PushData(bool showMySqlScriptDialog)
    {
      if (!CreateTableWithoutData && ChangedOrDeletedRows == 0)
      {
        return null;
      }

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

        var erroredOutRow = sqlScriptDialog.ErroredOutDataRow;
        return sqlScriptDialog.ScriptResult == MySqlStatement.StatementResultType.ErrorThrown
          ? erroredOutRow != null ? new List<IMySqlDataRow>(1) { erroredOutRow } : null
          : sqlScriptDialog.ActualStatementRowsList;
      }
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
    /// <param name="temporaryRange">Excel data range in a temporary Excel worksheet containing the data to fill the table.</param>
    /// <param name="useMappedColumns">Flag indicating if the data is added to the mapped column instead of to the column with the same index as the Excel data.</param>
    /// <param name="asynchronous">Flag indicating whether the operation should run asynchronously and show its progress.</param>
    /// <returns><c>true</c> if the data addition was successful, <c>false</c> otherwise.</returns>
    public bool AddExcelData(TempRange temporaryRange, bool useMappedColumns, bool asynchronous)
    {
      if (temporaryRange == null || temporaryRange.Range == null)
      {
        return false;
      }

      try
      {
        int numRows = temporaryRange.Range.Rows.Count;
        int rowAdjustValue = _firstRowIsHeaders && !OperationType.IsForExport() ? 1 : 0;
        for (int row = 1 + rowAdjustValue; row <= numRows; row++)
        {
          var valuesArray = temporaryRange.Range.GetRowValuesAsLinearArray(row, IsFormatted);
          if (valuesArray == null || valuesArray.Length <= 0 || valuesArray.Length > Columns.Count)
          {
            continue;
          }

          LoadDataRow(valuesArray, IsPreviewTable);
        }

        if (rowAdjustValue == 0)
        {
          ResetFirstRowIsHeaderValue();
        }
      }
      catch (Exception ex)
      {
        MySqlSourceTrace.WriteAppErrorToLog(ex);
        string errorTitle = string.Format(Resources.TableDataAdditionErrorTitle, OperationType.IsForExport() ? "exporting" : "appending");
        MiscUtilities.ShowCustomizedErrorDialog(errorTitle, ex.Message + Environment.NewLine + Environment.NewLine + ex.StackTrace);
        return false;
      }

      return true;
    }

    /// <summary>
    /// Creates data rows, fills them with the given Excel data and sets column properties automatically given user options.
    /// </summary>
    /// <param name="dataRange">Excel data range containing the data to fill the table.</param>
    /// <param name="recreateColumnsFromData">Flag indicating if any existing columns in the table must be dropped and re-created based on the given data range.</param>
    /// <param name="asynchronous">Flag indicating whether the call to <see cref="AddExcelData"/> should run asynchronously and show its progress.</param>
    /// <param name="limitRowsQuantity">Limit the number of loaded rows to this quantity. If less than 1 it means no limit is applied.</param>
    /// <return><c>true</c> if the columns setup is successful, <c>false</c> otherwise.</return>
    public bool SetupColumnsWithData(Excel.Range dataRange, bool recreateColumnsFromData, bool asynchronous, int limitRowsQuantity = 0)
    {
      Clear();

      // Add the Excel data to rows in this table.
      var dateColumnIndexes = new List<int>(Columns.Count);
      int dateColumnIndexAdjust = AddPrimaryKeyColumn ? 1 : 0;
      dateColumnIndexes.AddRange(from MySqlDataColumn column in Columns where column.IsDate select column.RangeColumnIndex - dateColumnIndexAdjust);
      using (var temporaryRange = new TempRange(dataRange, true, true, true, AddPrimaryKeyColumn, dateColumnIndexes.ToArray(), limitRowsQuantity))
      {
        CreateColumns(temporaryRange, recreateColumnsFromData);
        bool success = AddExcelData(temporaryRange, false, asynchronous);
        if (!success || !DetectDatatype)
        {
          return success;
        }

        // Automatically detect the excelData type for columns based on their data.
        foreach (MySqlDataColumn column in Columns)
        {
          column.DetectMySqlDataType(temporaryRange.Range, false);
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
      if (!string.Equals(TableName, syncFromTable.TableName, StringComparison.InvariantCulture))
      {
        TableName = syncFromTable.TableName;
      }

      FirstRowIsHeaders = syncFromTable.FirstRowIsHeaders;
      UseFirstColumnAsPk = syncFromTable.UseFirstColumnAsPk;
      foreach (MySqlDataColumn syncFromColumn in syncFromTable.Columns)
      {
        var thisColumn = Columns[syncFromColumn.ColumnName] as MySqlDataColumn;
        if (thisColumn == null)
        {
          continue;
        }

        thisColumn.SyncSchema(syncFromColumn);
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
      if (_copyingTableData || IsPreviewTable)
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
      if (IsPreviewTable)
      {
        return;
      }

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
      switch (args.PropertyName)
      {
        case "PrimaryKey":
          _primaryKeyColumns = null;
          _primaryKeyColumnsDataLength = 0;
          _maxQueryForPrimaryColumnsLength = 0;
          break;

        case "DisplayName":
          // Update the pre SQL length since a column name changed.
          _preSqlLength = 0;
          break;

        case "MySqlDataType":
          // Update the columns data length since a column data type changed.
          _columnsDataLength = 0;
          break;

        case "ExcludeColumn":
          // If a column gets excluded then notify all child MySqlDataRow objects to recalculate SQL queries.
          _columnsForInsertion = null;
          _preSqlForAddedRows = null;
          OnPropertyChanged("ColumnExcluded");
          break;
      }

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
      DataTable columnsInfoTable = filledTable.GetColumnsSchemaInfo();
      CreateTableSchema(columnsInfoTable, true);
      if (!schemaOnly)
      {
        CopyTableData(filledTable);
      }
    }

    /// <summary>
    /// Adds a specified number of <see cref="MySqlDataColumn"/> objects to the Columns collection where the first column may be an automatically created one for the table's primary index.
    /// </summary>
    /// <param name="temporaryRange">Excel data range in a temporary Excel worksheet containing the data to create columns for.</param>
    /// <param name="recreateColumnsFromData">Flag indicating if any existing columns in the table must be dropped and re-created based on the given data range.</param>
    private void CreateColumns(TempRange temporaryRange, bool recreateColumnsFromData)
    {
      if (temporaryRange == null || temporaryRange.Range == null || (!recreateColumnsFromData && Columns.Count > 0))
      {
        return;
      }

      // Drop all columns and re-create them or create them if none have been created so far.
      Columns.Clear();
      bool isAutoPkRange = temporaryRange.RangeType == TempRange.TempRangeType.AutoPkRange;
      int colIdx = isAutoPkRange ? 0 : 1;
      foreach (Excel.Range sourceColumnRange in temporaryRange.Range.Columns)
      {
        bool autoPk = isAutoPkRange && sourceColumnRange.Column == 1;
        MySqlDataColumn column = new MySqlDataColumn(OperationType.IsForExport(), autoPk, "Column" + colIdx, sourceColumnRange.Column);
        column.ColumnWarningsChanged += ColumnWarningsChanged;
        column.PropertyChanged += ColumnPropertyValueChanged;
        Columns.Add(column);
        colIdx++;
      }

      ResetAutoPkcolumnName();
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
    /// Gets a piece of a CREATE TABLE statament containing only the index definitions given the index type.
    /// </summary>
    /// <param name="sqlType">The type of SQL statement used for the new table.</param>
    /// <param name="indexType">The type of index created for this table.</param>
    /// <param name="indexesDelimiter">The delimiter used to separate each index definition.</param>
    /// <returns>A piece of a CREATE TABLE statament containing only the index definitions given the index type.</returns>
    private string GetIndexesSqlPiece(NewTableSqlType sqlType, IndexType indexType, ref string indexesDelimiter)
    {
      IEnumerable<MySqlDataColumn> indexColumns = null;
      switch (indexType)
      {
        case IndexType.Index:
          indexColumns = Columns.OfType<MySqlDataColumn>().Where(c => !c.ExcludeColumn && !c.UniqueKey && c.CreateIndex);
          break;

        case IndexType.Primary:
          indexColumns = Columns.OfType<MySqlDataColumn>().Where(c => !c.ExcludeColumn && c.PrimaryKey);
          break;

        case IndexType.Unique:
          indexColumns = Columns.OfType<MySqlDataColumn>().Where(c => !c.ExcludeColumn && c.UniqueKey);
          break;
      }

      if (indexColumns == null)
      {
        return null;
      }

      bool delimiterStartsWithComma = indexesDelimiter.StartsWith(",");
      bool alterTable = sqlType == NewTableSqlType.AlterComplete || sqlType == NewTableSqlType.AlterOnlyKeys;
      var sql = new StringBuilder(MiscUtilities.STRING_BUILDER_DEFAULT_CAPACITY);
      if (indexType == IndexType.Primary)
      {
        var columnsDelimiter = string.Empty;
        sql.Append(indexesDelimiter);
        if (alterTable)
        {
          sql.Append("ADD ");
        }

        sql.Append("PRIMARY KEY");
        sql.Append(" (");
        foreach (var col in indexColumns)
        {
          sql.Append(columnsDelimiter);
          sql.AppendFormat("`{0}`", col.DisplayNameForSqlQueries);
          columnsDelimiter = ",";
        }

        sql.Append(")");
        if (!delimiterStartsWithComma)
        {
          indexesDelimiter = "," + indexesDelimiter;
        }
      }
      else
      {
        string indexNameSuffix;
        string indexTokens;
        if (indexType == IndexType.Unique)
        {
          indexTokens = (alterTable ? "ADD " : string.Empty) + "UNIQUE INDEX";
          indexNameSuffix = "unique";
        }
        else
        {
          indexTokens = (alterTable ? "ADD " : string.Empty) + "INDEX";
          indexNameSuffix = "index";
        }

        foreach (var col in indexColumns)
        {
          sql.Append(indexesDelimiter);
          sql.Append(indexTokens);
          sql.AppendFormat(" `{0}_{1}` (`{0}`)", col.DisplayNameForSqlQueries, indexNameSuffix);
          if (delimiterStartsWithComma)
          {
            continue;
          }

          indexesDelimiter = "," + indexesDelimiter;
          delimiterStartsWithComma = true;
        }
      }

      return sql.ToString();
    }

    /// <summary>
    /// Gets the static piece of an INSERT SQL query that does not change from row to row containing schema, table and column names.
    /// </summary>
    /// <returns>The static piece of an INSERT SQL query that does not change from row to row containing schema, table and column names.</returns>
    private string GetPreSqlForAddedRows()
    {
      if (ColumnsForInsertion == null)
      {
        return string.Empty;
      }

      PreSqlBuilder.Clear();
      string colsSeparator = string.Empty;
      PreSqlBuilder.Append(MySqlStatement.STATEMENT_INSERT);
      PreSqlBuilder.AppendFormat(" `{0}`.`{1}` (", SchemaName, TableNameForSqlQueries);
      foreach (var column in ColumnsForInsertion)
      {
        PreSqlBuilder.AppendFormat("{0}`{1}`", colsSeparator, column.DisplayNameForSqlQueries);
        colsSeparator = ",";
      }

      PreSqlBuilder.Append(") VALUES (");
      return PreSqlBuilder.ToString();
    }

    /// <summary>
    /// Gets the length of the first part a SQL query generated for this table may have.
    /// The first part contains schema, table and column names.
    /// </summary>
    /// <returns>The length of the first part a SQL query generated for this table may have.</returns>
    private int GetPreSqlLength()
    {
      int colNamesLength = Columns.Cast<MySqlDataColumn>().Sum(column => column.DisplayNameForSqlQueries.Length + 2);
      return colNamesLength + TableNameForSqlQueries.Length + SchemaName.Length + PRE_SQL_PADDING_LENGTH;
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
    /// Updates the table's automatically generated primary key's name based on the current table name.
    /// </summary>
    private void ResetAutoPkcolumnName()
    {
      if (!AddPrimaryKeyColumn || Columns.Count <= 0)
      {
        return;
      }

      MySqlDataColumn autoPKcolumn = GetColumnAtIndex(0);
      string tableIdName = TableName + (TableName.Length > 0 ? "_" : string.Empty) + "id";
      string autoPkName = GetNonDuplicateColumnName(tableIdName);
      autoPKcolumn.SetDisplayName(autoPkName);
    }

    /// <summary>
    /// Resets the value of <see cref="MySqlDataRow.IsHeadersRow"/> with the value of <see cref="FirstRowIsHeaders"/>.
    /// </summary>
    private void ResetFirstRowIsHeaderValue()
    {
      if (Rows.Count <= 0)
      {
        return;
      }

      MySqlDataRow firstRow = Rows[0] as MySqlDataRow;
      if (firstRow != null)
      {
        firstRow.IsHeadersRow = FirstRowIsHeaders;
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

      _changedOrDeletedRows = -1;
      MySqlDataRow mySqlRow = args.Row as MySqlDataRow;
      mySqlRow.RowChanged(args.Action);
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
        col.SetMySqlDataType(_firstRowIsHeaders ? col.RowsFromSecondDataType : col.RowsFromFirstDataType);
        col.CreateIndex = AutoIndexIntColumns && col.IsInteger;
      }

      AdjustAutoPkValues();
      ResetFirstRowIsHeaderValue();
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