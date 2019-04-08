// Copyright (c) 2012, 2019, Oracle and/or its affiliates. All rights reserved.
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
using System.Globalization;
using System.Linq;
using System.Text;
using MySQL.ForExcel.Classes.EventArguments;
using MySQL.ForExcel.Classes.Exceptions;
using MySQL.ForExcel.Forms;
using MySQL.ForExcel.Interfaces;
using MySQL.ForExcel.Properties;
using MySql.Utility.Classes;
using MySql.Utility.Classes.Logging;
using MySql.Utility.Classes.MySqlWorkbench;
using MySql.Utility.Classes.Spatial;
using ExcelInterop = Microsoft.Office.Interop.Excel;
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
    /// Key used to represent a warning about the primary key column's name being a duplicate of another column's name.
    /// </summary>
    private const string DUPLICATE_PK_NAME_WARNING_KEY = "DUPLICATE_PK_NAME";

    /// <summary>
    /// Key used to represent a warning about the table's name being a duplicate of another table in the same schema.
    /// </summary>
    private const string DUPLICATE_TABLE_NAME_WARNING_KEY = "DUPLICATE_TABLE_NAME";

    /// <summary>
    /// Key used to represent a warning about the table's name being null or empty.
    /// </summary>
    private const string EMPTY_TABLE_NAME_WARNING_KEY = "EMPTY_TABLE_NAME";

    /// <summary>
    /// Key used to represent a warning about the table's name having spaces or upper case letters.
    /// </summary>
    private const string NON_STANDARD_TABLE_NAME_WARNING_KEY = "NON_STANDARD_TABLE_NAME";

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
    /// Container with warnings for users about the auto-generated primary key.
    /// </summary>
    private WarningsContainer _autoPkWarnings;

    /// <summary>
    /// Flag indicating if the column names where changed to use the first row of data.
    /// </summary>
    private bool _changedColumnNamesWithFirstRowOfData;

    /// <summary>
    /// The number of changed or deleted rows in this table. If less than 0 it means we want to recalculate its value.
    /// </summary>
    private int _changedOrDeletedRows;

    /// <summary>
    /// The character set used to store text data in this table.
    /// </summary>
    /// <remarks>If null or empty it means the schema character set is used.</remarks>
    private string _charSet;

    /// <summary>
    /// The collation used with the character set to store text data in this table.
    /// </summary>
    /// <remarks>If null or empty it means the default collation is used.</remarks>
    private string _collation;

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
    private bool _detectDataType;

    /// <summary>
    /// The default DB engine used for new table creations.
    /// </summary>
    private string _engine;

    /// <summary>
    /// Flag indicating if the first row in the Excel region to be exported contains the column names of the MySQL table that will be created.
    /// </summary>
    private bool _firstRowContainsColumnNames;

    /// <summary>
    /// Gets an approximation for a maximum SQL query length containing primary key column data only.
    /// </summary>
    private int _maxQueryForPrimaryColumnsLength;

    /// <summary>
    /// An approximation for a maximum SQL query length.
    /// </summary>
    private int _maxQueryLength;

    /// <summary>
    /// Contains the value of the max_allowed_packet system variable of the MySQL Server currently connected to.
    /// </summary>
    private int _mysqlMaxAllowedPacket;

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
    /// The <see cref="StringBuilder"/> used to build SQL SELECT queries text.
    /// </summary>
    private StringBuilder _sqlBuilderForSelect;

    /// <summary>
    /// The <see cref="StringBuilder"/> used to build SQL UPDATE queries text.
    /// </summary>
    private StringBuilder _sqlBuilderForUpdate;

    /// <summary>
    /// Flag indicating whether there is a MySQL table in the connected schema with the same name as in <see cref="TableName"/>.
    /// </summary>
    private bool? _tableExistsInSchema;

    /// <summary>
    /// Container with warnings for users about the table properties that could cause errors when creating this table in the database.
    /// </summary>
    private WarningsContainer _tableWarnings;

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
    /// This constructor is meant to be used by the <see cref="ExportDataForm"/> or <see cref="AppendDataForm"/> class.
    /// </summary>
    /// <param name="wbConnection">MySQL Workbench connection to a MySQL server instance selected by users.</param>
    /// <param name="proposedTableName">Proposed name for this new table.</param>
    /// <param name="addPrimaryKeyCol">Flag indicating if an auto-generated primary key column will be added as the first column in the table.</param>
    /// <param name="useFormattedValues">Flag indicating if the Excel excelData used to populate this table is formatted (numbers, dates, text) or not (numbers and text).</param>
    /// <param name="detectDataType">Flag indicating if the data type for each column is automatically detected when data is loaded by the <see cref="SetupColumnsWithData"/> method.</param>
    /// <param name="addBufferToVarChar">Flag indicating if columns with an auto-detected varchar type will get a padding buffer for its size.</param>
    /// <param name="autoIndexIntColumns">Flag indicating if columns with an integer-based data-type will have their <see cref="MySqlDataColumn.CreateIndex"/> property value set to true.</param>
    /// <param name="autoAllowEmptyNonIndexColumns">Flag indicating if columns that have their <see cref="MySqlDataColumn.CreateIndex"/> property value set to <c>false</c> will automatically get their <see cref="MySqlDataColumn.AllowNull"/> property value set to <c>true</c>.</param>
    /// <param name="forExportOperation">Flag indicating if the table will be used on an Export operation, otherwise it is considered to be used on an Append one.</param>
    public MySqlDataTable(MySqlWorkbenchConnection wbConnection, string proposedTableName, bool addPrimaryKeyCol, bool useFormattedValues, bool detectDataType, bool addBufferToVarChar, bool autoIndexIntColumns, bool autoAllowEmptyNonIndexColumns, bool forExportOperation = true)
      : this(wbConnection, proposedTableName)
    {
      AddBufferToVarChar = addBufferToVarChar;
      AddPrimaryKeyColumn = addPrimaryKeyCol;
      AutoAllowEmptyNonIndexColumns = autoAllowEmptyNonIndexColumns;
      AutoIndexIntColumns = autoIndexIntColumns;
      DetectDataType = detectDataType;
      IsFormatted = useFormattedValues;
      IsPreviewTable = true;
      OperationType = forExportOperation ? DataOperationType.Export : DataOperationType.Append;
      TableName = proposedTableName;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="MySqlDataTable"/> class.
    /// This constructor is meant to be used by the <see cref="ExportDataForm"/> class.
    /// </summary>
    /// <param name="fromTemplate">Template <see cref="MySqlDataTable"/> from which to build a new one, the new table is a copy with the same data, but the schema is built from the template not cloned.</param>
    /// <param name="dataRange">The <see cref="ExcelInterop.Range"/> containing the data to be fed to the new <see cref="MySqlDataTable"/>. If <c>null</c> the data is copied from the <seealso cref="fromTemplate"/> table.</param>
    public MySqlDataTable(MySqlDataTable fromTemplate, ExcelInterop.Range dataRange)
      : this(fromTemplate.WbConnection, fromTemplate.TableName)
    {
      AddBufferToVarChar = fromTemplate.AddBufferToVarChar;
      AddPrimaryKeyColumn = fromTemplate.AddPrimaryKeyColumn;
      AutoAllowEmptyNonIndexColumns = fromTemplate.AutoAllowEmptyNonIndexColumns;
      AutoIndexIntColumns = fromTemplate.AutoIndexIntColumns;
      CharSet = fromTemplate.CharSet;
      Collation = fromTemplate.Collation;
      DetectDataType = false;
      FirstRowContainsColumnNames = fromTemplate.FirstRowContainsColumnNames;
      IsFormatted = fromTemplate.IsFormatted;
      IsPreviewTable = false;
      OperationType = fromTemplate.OperationType;
      UseFirstColumnAsPk = fromTemplate.UseFirstColumnAsPk;
      UseOptimisticUpdate = fromTemplate.UseOptimisticUpdate;

      var schemaInfoTable = fromTemplate.GetColumnsSchemaInfo();
      CreateTableSchema(schemaInfoTable);
      if (dataRange == null)
      {
        CopyTableData(fromTemplate, true);
      }
      else
      {
        SetupColumnsWithData(dataRange, false);
      }
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="MySqlDataTable"/> class.
    /// This constructor is meant to be used by the <see cref="AppendDataForm"/> class to fetch schema information from the corresponding MySQL table before copying its excelData.
    /// </summary>
    /// <param name="fromDbTable">The <see cref="DbTable"/> object from which the new <see cref="MySqlDataTable"/> will get its schema information and its data.</param>
    /// <param name="useFormattedValues">Flag indicating if the Excel excelData used to populate this table is formatted (numbers, dates, text) or not (numbers and text).</param>
    public MySqlDataTable(DbView fromDbTable, bool useFormattedValues)
      : this(fromDbTable.Connection, fromDbTable.Name, true, useFormattedValues, null, DataOperationType.Append)
    {
      var dbTableData = fromDbTable.GetData();
      CopyTableData(dbTableData, false);
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="MySqlDataTable"/> class.
    /// </summary>
    /// <param name="wbConnection">MySQL Workbench connection to a MySQL server instance selected by users.</param>
    /// <param name="tableName">Name of the table.</param>
    /// <param name="fetchColumnsSchemaInfo">Flag indicating if the schema information from the corresponding MySQL table is fetched and recreated before any excelData is added to the table.</param>
    /// <param name="useFormattedValues">Flag indicating if the Excel excelData used to populate this table is formatted (numbers, dates, text) or not (numbers and text).</param>
    /// <param name="selectQuery">A SELECT query against a database object to fill the [MySqlDataTable] return object with.</param>
    /// <param name="operationType">The <see cref="DataOperationType"/> intended for this object.</param>
    public MySqlDataTable(MySqlWorkbenchConnection wbConnection, string tableName, bool fetchColumnsSchemaInfo, bool useFormattedValues, string selectQuery, DataOperationType operationType)
      : this(wbConnection, tableName)
    {
      if (!string.IsNullOrEmpty(selectQuery))
      {
        SelectQuery = selectQuery;
      }

      IsFormatted = useFormattedValues;
      OperationType = operationType;
      if (fetchColumnsSchemaInfo)
      {
        CreateTableSchema(tableName, true);
      }

      _mysqlMaxAllowedPacket = WbConnection.GetMySqlServerMaxAllowedPacket();
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="MySqlDataTable"/> class.
    /// This constructor is meant to be used by the <see cref="EditDataDialog"/> class to copy the contents of a table imported to Excel for edition and also by the import process.
    /// </summary>
    /// <param name="wbConnection">MySQL Workbench connection to a MySQL server instance selected by users.</param>
    /// <param name="tableName">Name of the table.</param>
    /// <param name="filledTable"><see cref="DataTable"/> object containing imported excelData from the MySQL table to be edited.</param>
    /// <param name="operationType">The <see cref="DataOperationType"/> intended for this object.</param>
    /// <param name="selectQuery">A SELECT query against a database object to fill the [MySqlDataTable] return object with.</param>
    public MySqlDataTable(MySqlWorkbenchConnection wbConnection, string tableName, DataTable filledTable, DataOperationType operationType, string selectQuery)
      : this(wbConnection, tableName, true, true, selectQuery, operationType)
    {
      OperationType = operationType;
      CopyTableData(filledTable, false);
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="MySqlDataTable"/> class.
    /// </summary>
    /// <param name="wbConnection">MySQL Workbench connection to a MySQL server instance selected by users.</param>
    /// <param name="tableName">Name of the table.</param>
    public MySqlDataTable(MySqlWorkbenchConnection wbConnection, string tableName)
      : this()
    {
      if (wbConnection != null && !string.IsNullOrEmpty(wbConnection.Schema))
      {
        SchemaName = wbConnection.Schema;
      }

      if (tableName != null)
      {
        TableName = tableName;
      }

      SelectQuery = $"SELECT * FROM `{SchemaName}`.`{TableNameForSqlQueries}`";
      WbConnection = wbConnection;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="MySqlDataTable"/> class.
    /// This constructor is meant to be used by the <see cref="ImportProcedureForm"/> class to copy the contents of result set tables from an executed procedure.
    /// </summary>
    /// <param name="wbConnection">MySQL Workbench connection to a MySQL server instance selected by users.</param>
    /// <param name="filledTable"><see cref="DataTable"/> object containing imported excelData from the MySQL table to be edited.</param>
    /// <param name="procedureSql">The SQL containing the schema name and the name of the stored procedure for which this table is created.</param>
    /// <param name="resultSetIndex">The index of the result set of a stored procedure this table contains data for. -1 represents the output parameters and return values result set.</param>
    public MySqlDataTable(MySqlWorkbenchConnection wbConnection, DataTable filledTable, string procedureSql, int resultSetIndex)
      : this(wbConnection, filledTable.TableName)
    {
      OperationType = DataOperationType.ImportProcedure;
      CopyTableSchemaAndData(filledTable);
      ProcedureResultSetIndex = resultSetIndex;
      _selectQuery = procedureSql;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="MySqlDataTable"/> class.
    /// </summary>
    public MySqlDataTable()
    {
      _changedColumnNamesWithFirstRowOfData = false;
      _changedOrDeletedRows = -1;
      _charSet = null;
      _collation = null;
      _columnsForInsertion = null;
      _columnsDataLength = 0;
      _copyingTableData = false;
      _createTableWithoutData = false;
      _detectDataType = false;
      _engine = null;
      _maxQueryLength = 0;
      _maxQueryForPrimaryColumnsLength = 0;
      _mysqlMaxAllowedPacket = 0;
      _preSqlBuilder = null;
      _preSqlForAddedRows = null;
      _preSqlLength = 0;
      _tableExistsInSchema = null;
      _selectQuery = $"SELECT * FROM `{TableNameForSqlQueries}`";
      _sqlBuilderForDelete = null;
      _sqlBuilderForInsert = null;
      _sqlBuilderForSelect = null;
      _sqlBuilderForUpdate = null;
      _useOptimisticUpdate = false;
      AddBufferToVarChar = false;
      AddPrimaryKeyColumn = false;
      AutoAllowEmptyNonIndexColumns = false;
      AutoIndexIntColumns = false;
      DataLoadException = null;
      FirstRowContainsColumnNames = false;
      IsTableNameValid = !string.IsNullOrEmpty(TableName);
      IsFormatted = false;
      IsPreviewTable = false;
      OperationType = DataOperationType.ImportTableOrView;
      ProcedureResultSetIndex = 0;
      SchemaName = string.Empty;
      UseFirstColumnAsPk = false;
      WbConnection = null;
      SetupWarnings();
    }

    #region Enums

    /// <summary>
    /// Specifies identifiers to indicate the type of action to perform in Append Data operations when new rows contain unique key values that duplicate old rows.
    /// </summary>
    public enum AppendDuplicateValuesActionType
    {
      /// <summary>
      /// Error out and abort the append operation.
      /// </summary>
      ErrorOutAndAbort,

      /// <summary>
      /// Ignore rows with duplicate unique key values.
      /// </summary>
      IgnoreDuplicates,

      /// <summary>
      /// Replace the values in the old rows with the ones in new rows.
      /// </summary>
      ReplaceDuplicates
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
      /// Import procedure data operation.
      /// </summary>
      ImportProcedure,

      /// <summary>
      /// Import table or view data operation.
      /// </summary>
      ImportTableOrView
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
    /// Gets the collation used to store text data in this table, looking up if not defined at this element.
    /// </summary>
    public string AbsoluteCollation
    {
      get
      {
        if (!string.IsNullOrEmpty(Collation))
        {
          return Collation;
        }

        var schemaCharSetAndCollation = WbConnection.GetSchemaCharSetAndCollation();
        return schemaCharSetAndCollation?.Item2;
      }
    }

    /// <summary>
    /// Gets a value indicating whether columns with an auto-detected varchar type will get a padding buffer for its size.
    /// </summary>
    public bool AddBufferToVarChar { get; }

    /// <summary>
    /// Gets a value indicating whether an auto-generated primary key column will be added as the first column in the table.
    /// </summary>
    public bool AddPrimaryKeyColumn
    {
      get => _addPrimaryKeyColumn;

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
    public bool AutoAllowEmptyNonIndexColumns { get; }

    /// <summary>
    /// Gets a value indicating whether columns with an integer-based data-type will have their <see cref="MySqlDataColumn.CreateIndex"/>property value set to <param name=">true"></param>.
    /// </summary>
    public bool AutoIndexIntColumns { get; }

    /// <summary>
    /// Gets the name of the auto-generated primary key column.
    /// </summary>
    public string AutoPkName => AddPrimaryKeyColumn && Columns.Count > 0 ? GetColumnAtIndex(0).DisplayName : string.Empty;

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
          _changedOrDeletedRows = changesTable?.Rows.Count ?? 0;
        }

        return _changedOrDeletedRows;
      }
    }

    /// <summary>
    /// Gets or sets the character set used to store text data in this table.
    /// </summary>
    /// <remarks>If null or empty it means the schema character set is used.</remarks>
    public string CharSet
    {
      get => _charSet;

      set
      {
        _charSet = value;
        OnPropertyChanged("CharSet");
      }
    }

    /// <summary>
    /// Gets or sets the collation used with the character set to store text data in this table.
    /// </summary>
    /// <remarks>If null or empty it means the default collation is used.</remarks>
    public string Collation
    {
      get => _collation;

      set
      {
        _collation = value;
        OnPropertyChanged("Collation");
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
          _columnsDataLength = Columns.Cast<MySqlDataColumn>().Sum(col => col.MySqlDataType.Length > 0 ? col.MySqlDataType.Length : col.MySqlDataType.MaxLength);
          _maxQueryLength = 0;
        }

        return _columnsDataLength;
      }
    }

    /// <summary>
    /// Gets an array of <see cref="MySqlDataColumn"/> objects to be used in INSERT queries.
    /// </summary>
    public MySqlDataColumn[] ColumnsForInsertion => _columnsForInsertion ?? (_columnsForInsertion = Columns.Cast<MySqlDataColumn>().Where(column => column.IncludeForInsertion).ToArray());

    /// <summary>
    /// Gets or sets a value indicating whether during an Export operation only the table will be created without any data.
    /// </summary>
    public bool CreateTableWithoutData
    {
      get => _createTableWithoutData;

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
    public string CurrentAutoPkWarningText => _autoPkWarnings.CurrentWarningText;

    /// <summary>
    /// Gets the last warning text associated to the table.
    /// </summary>
    public string CurrentTableWarningText => _tableWarnings.CurrentWarningText;

    /// <summary>
    /// Gets a <see cref="MySqlDataLoadException"/> indicating an error during a data load.
    /// </summary>
    public MySqlDataLoadException DataLoadException { get; private set; }

    /// <summary>
    /// Gets the number of deleted rows meaning the number of pending DELETE operations in an Edit Data operation.
    /// </summary>
    public int DeletingOperations
    {
      get
      {
        var changesDt = GetChanges(DataRowState.Deleted);
        return changesDt?.Rows.Count ?? 0;
      }
    }

    /// <summary>
    /// Gets or sets a value indicating whether data type for each column is automatically detected when data is loaded by the <see cref="SetupColumnsWithData"/> method.
    /// </summary>
    public bool DetectDataType
    {
      get => _detectDataType;

      set
      {
        if (_detectDataType != value)
        {
          OnPropertyChanged("DetectDataType");
        }

        _detectDataType = value;
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
    /// Gets a value indicating whether any value loaded into this <see cref="MySqlDataTable"/> must be checked to escape a starting equals sign so Excel does not mistake it as a formula.
    /// </summary>
    public bool EscapeFormulaTexts => (OperationType.IsForImport() || OperationType.IsForEdit()) && Settings.Default.ImportEscapeFormulaTextValues;

    /// <summary>
    /// Gets the name to be used in related <see cref="ExcelInterop.ListObject"/> objects.
    /// </summary>
    public string ExcelTableName
    {
      get
      {
        var excelTableNamePrefix = Settings.Default.ImportPrefixExcelTable &&
                                      !string.IsNullOrEmpty(Settings.Default.ImportPrefixExcelTableText)
          ? Settings.Default.ImportPrefixExcelTableText + "."
          : string.Empty;
        var excelTableNameSchemaPiece = !string.IsNullOrEmpty(SchemaName) ? SchemaName + "." : string.Empty;
        var excelTableNameTablePiece = !string.IsNullOrEmpty(TableName) ? TableName : "Table";
        return excelTableNamePrefix + excelTableNameSchemaPiece + excelTableNameTablePiece;
      }
    }

    /// <summary>
    /// Gets a value indicating whether the first column contains numeric whole numbers.
    /// </summary>
    public bool FirstColumnContainsIntegers
    {
      get
      {
        var containsIntegers = false;
        var firstColIdx = AddPrimaryKeyColumn ? 1 : 0;
        if (Columns.Count > firstColIdx)
        {
          containsIntegers = GetColumnAtIndex(firstColIdx).MySqlDataType.TypeName.Equals("integer", StringComparison.InvariantCultureIgnoreCase);
        }

        if (containsIntegers)
        {
          return true;
        }

        var rowsToAnalyzeCount = Math.Min(Rows.Count, 50);
        var startingRow = _firstRowContainsColumnNames ? 1 : 0;
        containsIntegers = startingRow < rowsToAnalyzeCount;
        for (var rowIdx = startingRow; rowIdx < rowsToAnalyzeCount; rowIdx++)
        {
          containsIntegers = containsIntegers && int.TryParse(Rows[rowIdx][1].ToString(), out _);
        }

        return containsIntegers;
      }
    }

    /// <summary>
    /// Gets or sets a value indicating whether the the first row of excelData contains the column names for a new table.
    /// </summary>
    public bool FirstRowContainsColumnNames
    {
      get => _firstRowContainsColumnNames;

      set
      {
        if (_firstRowContainsColumnNames != value)
        {
          OnPropertyChanged("FirstRowIsHeaders");
        }

        _firstRowContainsColumnNames = value;
        UseFirstRowAsHeaders();
      }
    }

    /// <summary>
    /// Gets or sets a value indicating whether column names are imported along with the table's data.
    /// </summary>
    public bool ImportColumnNames { get; set; }

    /// <summary>
    /// Gets the number of added rows meaning the number of pending INSERT operations in an Append, Edit or Export Data operation.
    /// </summary>
    public int InsertingOperations
    {
      get
      {
        var changesDt = GetChanges(DataRowState.Added);
        return changesDt?.Rows.Count ?? 0;
      }
    }

    /// <summary>
    /// Gets a value indicating whether the automatically added column used for the primary key is not a duplicate of another column.
    /// </summary>
    public bool IsAutoPkColumnNameValid => AddPrimaryKeyColumn && !GetColumnAtIndex(0).IsDisplayNameDuplicate;

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
        var mappedQty = 0;
        for (var colIdx = 0; colIdx < Columns.Count; colIdx++)
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
    /// Gets an approximation for a maximum SQL query length containing primary key column data only.
    /// </summary>
    public int MaxQueryForPrimaryColumnsLength
    {
      get
      {
        if (_maxQueryForPrimaryColumnsLength == 0)
        {
          var maxSize = PrimaryKeyColumnsDataLength + (MySqlDataType.MYSQL_DB_OBJECTS_MAX_LENGTH * 3);
          _maxQueryForPrimaryColumnsLength = (int)Math.Min(maxSize, int.MaxValue);
          _sqlBuilderForDelete = null;
        }

        return _maxQueryForPrimaryColumnsLength;
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
          var maxSize = ColumnsDataLength + (MySqlDataType.MYSQL_DB_OBJECTS_MAX_LENGTH * 3);
          _maxQueryLength = (int)Math.Min(maxSize, MySqlMaxAllowedPacket);
          _sqlBuilderForInsert = null;
          _sqlBuilderForUpdate = null;
        }

        return _maxQueryLength;
      }
    }

    /// <summary>
    /// Gets the value of the max_allowed_packet system variable of the MySQL Server currently connected to.
    /// </summary>
    public int MySqlMaxAllowedPacket
    {
      get
      {
        if (_mysqlMaxAllowedPacket == 0 && WbConnection != null)
        {
          var serverMaxAllowedPacket = WbConnection.GetMySqlServerMaxAllowedPacket();
          _mysqlMaxAllowedPacket = serverMaxAllowedPacket > 0 ? serverMaxAllowedPacket - SAFE_BYTES_BEFORE_REACHING_MAX_ALLOWED_PACKET : 0;
        }

        return _mysqlMaxAllowedPacket;
      }
    }

    /// <summary>
    /// Gets the number of columns that compose the table's primary key.
    /// </summary>
    public int NumberOfPk => Columns.OfType<MySqlDataColumn>().Skip(1).Count(col => col.PrimaryKey && !col.ExcludeColumn);

    /// <summary>
    /// Gets the data operation the table is used for.
    /// </summary>
    public DataOperationType OperationType { get; private set; }

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
    /// Gets an array of <see cref="MySqlDataColumn"/> objects that compose the primary key of this table.
    /// </summary>
    public MySqlDataColumn[] PrimaryKeyColumns => Columns.Cast<MySqlDataColumn>().Where(pkCol => pkCol.PrimaryKey).ToArray();

    /// <summary>
    /// Gets the combined length of data representation as text for primary key columns.
    /// </summary>
    public long PrimaryKeyColumnsDataLength => PrimaryKeyColumns.Sum(col => col.MySqlDataType.Length > 0 ? col.MySqlDataType.Length : col.MySqlDataType.MaxLength);

    /// <summary>
    /// Gets or sets the index of the result set of a stored procedure this table contains data for.
    /// </summary>
    /// <remarks>-1 represents the output parameters and return values result set.</remarks>
    public int ProcedureResultSetIndex { get; set; }

    /// <summary>
    /// Gets the name of the schema where this table exists or will be created.
    /// </summary>
    public string SchemaName { get; }

    /// <summary>
    /// Gets or sets the SELECT query used to retrieve the excelData from the corresponding MySQL table to fill this one.
    /// </summary>
    public string SelectQuery
    {
      get => _selectQuery;

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
    public StringBuilder SqlBuilderForDelete => _sqlBuilderForDelete ?? (_sqlBuilderForDelete = new StringBuilder(MaxQueryForPrimaryColumnsLength));

    /// <summary>
    /// The <see cref="StringBuilder"/> used to build SQL INSERT queries text.
    /// </summary>
    public StringBuilder SqlBuilderForInsert => _sqlBuilderForInsert ?? (_sqlBuilderForInsert = new StringBuilder(MaxQueryLength));

    /// <summary>
    /// The <see cref="StringBuilder"/> used to build SQL INSERT queries text.
    /// </summary>
    public StringBuilder SqlBuilderForSelect => _sqlBuilderForSelect ?? (_sqlBuilderForSelect = new StringBuilder(MaxQueryForPrimaryColumnsLength));

    /// <summary>
    /// The <see cref="StringBuilder"/> used to build SQL UPDATE queries text.
    /// </summary>
    public StringBuilder SqlBuilderForUpdate
    {
      get
      {
        var maxLen = Math.Min(MaxQueryLength * 2, int.MaxValue);
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
          var cleanTableName = TableName.ToLowerInvariant().Replace(" ", "_");
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
      get => base.TableName;

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
    public string TableNameForSqlQueries => TableName.Replace("`", "``");

    /// <summary>
    /// Gets the number of changed rows meaning the number of pending UPDATE operations in an Edit Data operation.
    /// </summary>
    public int UpdatingOperations
    {
      get
      {
        var changesDt = GetChanges(DataRowState.Modified);
        return changesDt?.Rows.Count ?? 0;
      }
    }

    /// <summary>
    /// Gets or sets a value indicating whether the first column of the table will be or is used in the table's primary key.
    /// </summary>
    public bool UseFirstColumnAsPk
    {
      get => _useFirstColumnAsPk;

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

        for (var i = 1; i < Columns.Count && value; i++)
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
      get => _useOptimisticUpdate;

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
    public MySqlWorkbenchConnection WbConnection { get; }

    /// <summary>
    /// The <see cref="StringBuilder"/> used to build SQL query text.
    /// </summary>
    private StringBuilder PreSqlBuilder => _preSqlBuilder ?? (_preSqlBuilder = new StringBuilder(PreSqlLength));

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
    /// Delegate handler for the <see cref="TableWarningsChanged"/> event.
    /// </summary>
    /// <param name="sender">Sender object.</param>
    /// <param name="args">Event arguments.</param>
    public delegate void TableWarningsChangedEventHandler(object sender, TableWarningsChangedArgs args);

    /// <summary>
    /// Occurs when a property value in this table changes.
    /// </summary>
    public event PropertyChangedEventHandler PropertyChanged;

    /// <summary>
    /// Occurs when a property value on any of the columns in this table changes.
    /// </summary>
    public event PropertyChangedEventHandler TableColumnPropertyValueChanged;

    /// <summary>
    /// Occurs when the warnings associated to any of the columns in this table change.
    /// </summary>
    public event TableWarningsChangedEventHandler TableWarningsChanged;

    #endregion Events

    /// <summary>
    /// Creates data rows in this table for data in the given Excel range.
    /// </summary>
    /// <param name="temporaryRange">Excel data range in a temporary Excel worksheet containing the data to fill the table.</param>
    /// <returns><c>true</c> if the data addition was successful, <c>false</c> otherwise.</returns>
    public bool AddExcelData(TempRange temporaryRange)
    {
      DataLoadException = null;
      if (temporaryRange?.Range == null)
      {
        return false;
      }

      var success = true;
      try
      {
        // Save the value of the computed property in a variable to avoid recalculating it over and over in the loop below.
        var escapeFormulaTexts = EscapeFormulaTexts;

        var numRows = temporaryRange.Range.Rows.Count;
        var rowAdjustValue = _firstRowContainsColumnNames && !IsPreviewTable ? 1 : 0;
        _copyingTableData = true;
        BeginLoadData();
        for (var row = 1 + rowAdjustValue; row <= numRows; row++)
        {
          var valuesArray = temporaryRange.Range.GetRowValuesAsLinearArray(row, IsFormatted);
          if (valuesArray == null || valuesArray.Length <= 0 || valuesArray.Length > Columns.Count)
          {
            continue;
          }

          PrepareCopyingItemArray(ref valuesArray, escapeFormulaTexts);
          LoadDataRow(valuesArray, IsPreviewTable);
        }

        if (rowAdjustValue == 0)
        {
          ResetFirstRowIsHeaderValue();
        }
      }
      catch (Exception ex)
      {
        DataLoadException = new MySqlDataLoadException(ex);
        var errorTitle = string.Format(Resources.TableDataAdditionErrorTitle, OperationType.IsForExport() ? "exporting" : "appending");
        Logger.LogException(ex, true, errorTitle);
        success = false;
      }
      finally
      {
        EndLoadData();
        _copyingTableData = false;
      }

      return success;
    }

    /// <summary>
    /// Checks that every <see cref="MySqlDataColumn"/> does not have a duplicate <see cref="MySqlDataColumn.DisplayName"/> and updates their corresponding warnings.
    /// </summary>
    /// <param name="excludeColumnsWithEmptyName">Flag indicating whether columns with an empty name should be excluded from the check.</param>
    public void CheckForDuplicatedColumnDisplayNames(bool excludeColumnsWithEmptyName = true)
    {
      foreach (var mySqlCol in
        Columns.Cast<MySqlDataColumn>()
          .Where(mySqlCol => !mySqlCol.ExcludeColumn && (!excludeColumnsWithEmptyName || !string.IsNullOrEmpty(mySqlCol.DisplayName))))
      {
        mySqlCol.CheckForDuplicatedDisplayName();
      }
    }

    /// <summary>
    /// Creates a new <see cref="MySqlDataTable"/> object with its schema cloned from this table but no data.
    /// </summary>
    /// <param name="autoPkCreationOnlyIfFirstColumnIsPk">Flag indicating whether an Auto PK column is prepended only if the value of the <see cref="UseFirstColumnAsPk"/> property is <c>true</c>.</param>
    /// <param name="subscribePropertyChangedEvent">Flag indicating whether the cloned columns subscribe to the parent table's <see cref="ColumnPropertyValueChanged"/> event.</param>
    /// <returns>Cloned <see cref="MySqlDataTable"/> object.</returns>
    public MySqlDataTable CloneSchema(bool autoPkCreationOnlyIfFirstColumnIsPk, bool subscribePropertyChangedEvent)
    {
      var createAutoPkColumn = autoPkCreationOnlyIfFirstColumnIsPk ? UseFirstColumnAsPk : AddPrimaryKeyColumn;
      var clonedTable = new MySqlDataTable(
        WbConnection,
        TableName,
        createAutoPkColumn,
        IsFormatted,
        DetectDataType,
        AddBufferToVarChar,
        AutoIndexIntColumns,
        AutoAllowEmptyNonIndexColumns)
      {
        CharSet = CharSet,
        Collation = Collation,
        FirstRowContainsColumnNames = FirstRowContainsColumnNames,
        IsFormatted = IsFormatted,
        IsPreviewTable = IsPreviewTable,
        OperationType = OperationType,
        UseFirstColumnAsPk = UseFirstColumnAsPk,
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
    /// Formats columns in the given <see cref="ExcelInterop.Range"/> depending on the data types of columns being imported.
    /// </summary>
    /// <param name="excelRange">The <see cref="ExcelInterop.Range"/> where this table's daa is going to be imported.</param>
    /// <param name="importingToExcelTable">Flag indicating whether the data is being imported to an Excel Table (ListObject) or to an Excel Range.</param>
    public void FormatImportExcelRange(ExcelInterop.Range excelRange, bool importingToExcelTable)
    {
      if (excelRange == null)
      {
        return;
      }

      if (!importingToExcelTable)
      {
        excelRange.ClearFormats();
      }

      var colsCount = excelRange.Columns.CountLarge;
      foreach (var col in Columns.Cast<MySqlDataColumn>().TakeWhile(col => col.Ordinal < colsCount))
      {
        string format;
        if (!importingToExcelTable && col.MySqlDataType.IsTime)
        {
          // Format columns that have a MySQL TIME data type
          format = Settings.Default.ImportExcelFormatTime;
        }
        else if (col.MySqlDataType.IsChar || col.MySqlDataType.IsText || col.MySqlDataType.IsSetOrEnum || col.MySqlDataType.IsJson)
        {
          // Format string columns containing numbers that may be misformatted by Excel as numbers
          format = ExcelUtilities.TEXT_FORMAT;
        }
        else if (col.MySqlDataType.IsDateTimeOrTimeStamp)
        {
          // Format DATETIME and TIMESTAMP data types with a long date format
          format = Settings.Default.ImportExcelFormatLongDates;
        }
        else if (col.MySqlDataType.TypeName.Equals("date", StringComparison.InvariantCultureIgnoreCase))
        {
          // Format DATE data type with a short date format
          format = Settings.Default.ImportExcelFormatShortDates;
        }
        else
        {
          continue;
        }

        ExcelInterop.Range dataColumnRange = excelRange.Columns[col.Ordinal + 1];
        dataColumnRange.NumberFormat = format;
      }
    }

    /// <summary>
    /// Creates a SQL query to lock or unlock the table.
    /// </summary>
    /// <param name="lockTable">Flag indicating if the query is locking or unlocking the table.</param>
    /// <returns>SQL query locking or unlocking the table.</returns>
    public string GeLockTableSql(bool lockTable)
    {
      return lockTable
        ? $"{MySqlStatement.STATEMENT_LOCK_TABLES} `{SchemaName}`.`{TableName}` WRITE"
        : MySqlStatement.STATEMENT_UNLOCK_TABLES;
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
    /// <param name="useDisplayName">Flag indicating whether the <see cref="MySqlDataColumn.DisplayName"/> or the <see cref="MySqlDataColumn.ColumnName"/> property must be used for the name comparison.</param>
    /// <param name="caseSensitive">Flag indicating if a case sensitive comparison against the column name should be done.</param>
    /// <param name="skipExcludedColumns">Flag indicating whether <see cref="MySqlDataColumn"/>s where <see cref="MySqlDataColumn.ExcludeColumn"/> is <c>true</c> should be skipped from the search.</param>
    /// <param name="exceptAtIndex">Index of a column to exclude from the name search.</param>
    /// <returns>The ordinal index within the columns collection.</returns>
    public int GetColumnIndex(string columnName, bool useDisplayName, bool caseSensitive, bool skipExcludedColumns, int exceptAtIndex = -1)
    {
      var comparisonMethod = caseSensitive
        ? StringComparison.InvariantCulture
        : StringComparison.InvariantCultureIgnoreCase;
      MySqlDataColumn mySqlCol = null;
      foreach (MySqlDataColumn col in Columns)
      {
        if ((col.ExcludeColumn && skipExcludedColumns)
            || col.Ordinal == exceptAtIndex
            || !string.Equals(useDisplayName ? col.DisplayName : col.ColumnName, columnName, comparisonMethod))
        {
          continue;
        }

        mySqlCol = col;
        break;
      }

      return mySqlCol?.Ordinal ?? -1;
    }

    /// <summary>
    /// Gets the ordinal index within the columns collection of the column with the given name doing a case sensitive comparison.
    /// </summary>
    /// <param name="columnName">Column name.</param>
    /// <param name="useDisplayName">Flag indicating whether the <see cref="MySqlDataColumn.DisplayName"/> or the <see cref="DataColumn.ColumnName"/> property must be used for the name comparison.</param>
    /// <param name="skipExcludedColumns">Flag indicating whether <see cref="MySqlDataColumn"/>s where <see cref="MySqlDataColumn.ExcludeColumn"/> is <c>true</c> should be skipped from the search.</param>
    /// <returns>The ordinal index within the columns collection.</returns>
    public int GetColumnIndex(string columnName, bool useDisplayName, bool skipExcludedColumns)
    {
      return GetColumnIndex(columnName, useDisplayName, true, skipExcludedColumns);
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

      var retArray = new string[Columns.Count];
      for (var i = 0; i < Columns.Count; i++)
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
    /// Gets the schema information defined in the collection of <see cref="MySqlDataColumn"/> objects.
    /// </summary>
    /// <returns>Table with schema information regarding this <see cref="MySqlDataTable"/> columns.</returns>
    public MySqlColumnsInformationTable GetColumnsSchemaInfo()
    {
      var schemaInfoTable = new MySqlColumnsInformationTable();
      foreach (MySqlDataColumn column in Columns)
      {
        var newRow = schemaInfoTable.NewRow();
        column.FillSchemaInfoRow(ref newRow);
        schemaInfoTable.Rows.Add(newRow);
      }

      return schemaInfoTable;
    }

    /// <summary>
    /// Creates a SQL query to disable the foreign key constraints.
    /// </summary>
    /// <remarks>Only useful for InnoDB tables.</remarks>
    /// <param name="disable">Flag indicating if the query is disabling or enabling foreign key constraints.</param>
    /// <returns>ALTER TABLE SQL query disabling or enabling foreign key constraints.</returns>
    public string GetDisableForeignKeysSql(bool disable)
    {
      return $"SET foreign_key_checks = {(disable ? "0" : "1")}";
    }

    /// <summary>
    /// Creates a SQL query to alter a table in the database disabling or enabling its keys.
    /// </summary>
    /// <remarks>Only useful for MyISAM tables.</remarks>
    /// <param name="disable">Flag indicating if the query is disabling or enabling keys.</param>
    /// <returns>ALTER TABLE SQL query disabling or enabling keys.</returns>
    public string GetDisableKeysSql(bool disable)
    {
      return $"ALTER TABLE `{SchemaName}`.`{TableName}` {(disable ? "DISABLE" : "ENABLE")} KEYS";
    }

    /// <summary>
    /// Creates a SQL query to disable the foreign key constraints.
    /// </summary>
    /// <remarks>Only useful for InnoDB tables.</remarks>
    /// <param name="disable">Flag indicating if the query is disabling or enabling unique key constraints.</param>
    /// <returns>ALTER TABLE SQL query disabling or enabling foreign key constraints.</returns>
    public string GetDisableUniqueKeysSql(bool disable)
    {
      return $"SET unique_checks = {(disable ? "0" : "1")}";
    }

    /// <summary>
    /// Gets a list of <see cref="MySqlDummyRow"/> objects containing statements related to the table creation or to performance optimizations.
    /// </summary>
    /// <param name="beforeInserts">Flag indicating if the SQL statements will appear before the INSERT statements.</param>
    /// <returns>A list of <see cref="MySqlDummyRow"/> objects containing statements related to the table creation or to performance optimizations.</returns>
    public IList<MySqlDummyRow> GetDummyRowsForTableCreationAndIndexOptimization(bool beforeInserts)
    {
      MySqlDummyRow dummyRow;
      var isForExport = OperationType.IsForExport();

      // Set the type of SQL statement generated by the GetNewTableSql method, notice that there is a difference on the type depending on value
      // of the beforeInserts parameter, meaning the query is executed before or after INSERT statements. The ones before normally disable keys
      // or create a new table without any keys, the ones after enable back the keys or add the keys for a new table.
      var sqlType = isForExport
        ? (CreateTableWithoutData || !Settings.Default.ExportSqlQueriesCreateIndexesLast
          ? (beforeInserts ? NewTableSqlType.CreateWithKeys : NewTableSqlType.None)
          : (beforeInserts ? NewTableSqlType.CreateWithoutKeys : NewTableSqlType.AlterOnlyKeys))
        : NewTableSqlType.None;
      if (isForExport && sqlType == NewTableSqlType.None)
      {
        return null;
      }

      var dummyRowsList = new List<MySqlDummyRow>(3);
      if (isForExport && sqlType != NewTableSqlType.None)
      {
        var createOrAlterTableSql = GetNewTableSql(true, sqlType);
        if (!string.IsNullOrEmpty(createOrAlterTableSql))
        {
          dummyRow = new MySqlDummyRow(createOrAlterTableSql);
          dummyRowsList.Add(dummyRow);
        }
      }

      if (isForExport || !Settings.Default.AppendSqlQueriesDisableIndexes)
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
    /// Gets an array of <see cref="ExcelInterop.Range"/> objects that the data of this <see cref="MySqlDataTable"/> would occupy if imported.
    /// The first element corresponds to the <see cref="ExcelInterop.Range"/> of the table's data, the second to the PivotTable placeholder.
    /// </summary>
    /// <param name="toLeftCell">The top left cell where the data would be imported.</param>
    /// <param name="withSummaryRow">Flag indicating whether a summary row is to be created for the imported data.</param>
    /// <param name="withPivotTable">Flag indicating whether a PivotTable is to be created along with the imported data.</param>
    /// <param name="pivotPosition">The <see cref="ExcelUtilities.PivotTablePosition"/> of the PivotTable to be created relative to the imported data.</param>
    /// <returns>An array of <see cref="ExcelInterop.Range"/> objects that the data of this <see cref="MySqlDataTable"/> would occupy if imported.</returns>
    public ExcelInterop.Range[] GetExcelRangesToOccupy(ExcelInterop.Range toLeftCell, bool withSummaryRow, bool withPivotTable, ExcelUtilities.PivotTablePosition pivotPosition = ExcelUtilities.PivotTablePosition.Right)
    {
      if (toLeftCell == null)
      {
        return null;
      }

      var ranges = new ExcelInterop.Range[withPivotTable ? 2 : 1];
      var rowsCount = Rows.Count + (ImportColumnNames || Settings.Default.ImportCreateExcelTable ? 1 : 0) + (withSummaryRow && Settings.Default.ImportCreateExcelTable ? 1 : 0);
      ranges[0] = toLeftCell.SafeResize(rowsCount, Columns.Count);
      if (withPivotTable)
      {
        ranges[1] = ranges[0].GetPivotTableTopLeftCell(pivotPosition).Resize[ExcelUtilities.PIVOT_TABLES_PLACEHOLDER_DEFAULT_ROWS_SIZE, ExcelUtilities.PIVOT_TABLES_PLACEHOLDER_DEFAULT_COLUMNS_SIZE];
      }

      return ranges;
    }

    /// <summary>
    /// Creates a SQL query to create a new table in the database based on this table's schema information.
    /// </summary>
    /// <param name="formatNewLinesAndTabs">Flag indicating if the SQL statement must be formatted to insert new line and tab characters for display purposes.</param>
    /// <param name="sqlType">The type of SQL statement used for the new table.</param>
    /// <returns>The SQL statement to create or alter a new table.</returns>
    public string GetNewTableSql(bool formatNewLinesAndTabs, NewTableSqlType sqlType)
    {
      var createTable = sqlType == NewTableSqlType.CreateWithKeys || sqlType == NewTableSqlType.CreateWithoutKeys;
      if (!createTable && Columns.OfType<MySqlDataColumn>().Count(col => col.CreateIndex || col.UniqueKey) == 0)
      {
        return null;
      }

      var sql = new StringBuilder(MiscUtilities.STRING_BUILDER_DEFAULT_CAPACITY);
      var nl = formatNewLinesAndTabs ? Environment.NewLine : " ";
      var nlt = formatNewLinesAndTabs ? Environment.NewLine + "   " : " ";
      sql.Append(createTable ? MySqlStatement.STATEMENT_CREATE_TABLE : MySqlStatement.STATEMENT_ALTER_TABLE);
      sql.AppendFormat(" `{0}`.`{1}`", SchemaName, TableName);
      if (createTable)
      {
        sql.Append(nl);
        sql.Append("(");
      }

      var delimiter = nlt;
      // Consider adding later on code to correctly assemble an ALTER statement, since for a NewTableSqlType.AlterComplete
      // the ALTER statement is not 100% correct, but the NewTableSqlType.AlterComplete is not yet used throughout the code.
      if (sqlType != NewTableSqlType.AlterOnlyKeys)
      {
        foreach (var col in Columns.OfType<MySqlDataColumn>())
        {
          // DO NOT CHANGE TO LINQ: Skipping excluded columns is better than simplifying the foreach using a .Where LINQ form
          // since using it will cause traversing the list of columns 2 times in most of the cases where none or just a very
          // few columns are excluded.
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

      if (!createTable)
      {
        return sql.ToString();
      }

      sql.Append(nl);
      sql.Append(")");
      if (!string.IsNullOrEmpty(CharSet))
      {
        sql.Append(nl);
        sql.AppendFormat("{0} = {1}", MySqlStatement.STATEMENT_DEFAULT_CHARSET, CharSet);
        if (!string.IsNullOrEmpty(Collation))
        {
          sql.Append(nl);
          sql.AppendFormat("{0} = {1}", MySqlStatement.STATEMENT_COLLATE, Collation);
        }
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
      var columnNames = Columns.Count > 0 ? Columns.OfType<MySqlDataColumn>().Where(col => col.Ordinal != forColumnIndex).Select(col => col.DisplayName).ToList() : null;
      return columnNames.GetNonDuplicateText(proposedName);
    }

    /// <summary>
    /// Gets a list of <see cref="MySqlDummyRow"/> objects containing INSERT statements for multiple rows as bulk operations.
    /// </summary>
    /// <returns>A list of <see cref="MySqlDummyRow"/> objects containing INSERT statements for multiple rows as bulk operations.</returns>
    public List<MySqlDummyRow> GetBulkInsertDummyRows()
    {
      var dummyRows = new List<MySqlDummyRow>();
      var nextRow = 0;
      do
      {
        var insertSql = GetBulkInsertSql(nextRow, -1, out nextRow, out _);
        var dummyRow = new MySqlDummyRow(insertSql);
        dummyRows.Add(dummyRow);
      } while (nextRow >= 0);

      return dummyRows;
    }

    /// <summary>
    /// Creates a SQL query to insert rows in this table into a MySQL table in a single bulk operation.
    /// </summary>
    /// <param name="startRow">Values to be inserted are taken from this row number forward.</param>
    /// <param name="limit">Maximum number of rows in the table to be inserted with this query, if -1 all rows are included.</param>
    /// <param name="nextRow">Last row processed if the query needs to be split, -1 if all rows were processed.</param>
    /// <param name="insertingRowsCount">Number of rows to be inserted into the database with this query.</param>
    /// <returns>A SQL query to insert rows in this table into a MySQL table in a single bulk operation.</returns>
    public string GetBulkInsertSql(int startRow, int limit, out int nextRow, out int insertingRowsCount)
    {
      nextRow = -1;
      insertingRowsCount = 0;

      if (startRow < 0)
      {
        startRow = 0;
      }

      if (startRow >= Rows.Count)
      {
        return null;
      }

      var maxByteCount = _mysqlMaxAllowedPacket > 0 ? _mysqlMaxAllowedPacket - SAFE_BYTES_BEFORE_REACHING_MAX_ALLOWED_PACKET : 0;
      var queryStringByteCount = 0;
      var nl = Environment.NewLine;
      var rowsSeparator = string.Empty;
      var sqlBuilderForInsert = SqlBuilderForInsert;
      sqlBuilderForInsert.Clear();
      sqlBuilderForInsert.Append(PreSqlForAddedRows);
      var absRowIdx = 0;
      var singleRowValuesBuilder = new StringBuilder();
      if (maxByteCount > 0)
      {
        queryStringByteCount = Encoding.ASCII.GetByteCount(sqlBuilderForInsert.ToString());
      }

      // Loop all rows in this table to include the values for insertion in the query.
      for (var rowIdx = startRow; rowIdx < Rows.Count; rowIdx++)
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
        var dr = Rows[rowIdx];
        singleRowValuesBuilder.Clear();
        singleRowValuesBuilder.AppendFormat("{0}(", rowsSeparator);
        var colsSeparator = string.Empty;
        foreach (var column in ColumnsForInsertion)
        {
          var valueToDb = column.GetStringValue(dr[column.ColumnName], out var insertingValueIsNull);
          var wrapValueCharacter = column.MySqlDataType.RequiresQuotesForValue && !insertingValueIsNull ? "'" : string.Empty;
          singleRowValuesBuilder.AppendFormat("{0}{1}{2}{1}", colsSeparator, wrapValueCharacter, valueToDb);
          colsSeparator = ",";
        }

        // Close the current row values piece of the query and check if we have not exceeded the maximum packet size allowed by the server,
        //  otherwise we return the query string as is and return the last row number that was processed so another INSERT INTO query is
        //  assembled starting from the row we left on.
        singleRowValuesBuilder.Append(")");
        var singleRowValuesString = singleRowValuesBuilder.ToString();
        if (maxByteCount > 0)
        {
          var singleValueRowQueryByteCount = Encoding.ASCII.GetByteCount(singleRowValuesString);
          if (queryStringByteCount + singleValueRowQueryByteCount > maxByteCount)
          {
            nextRow = rowIdx;
            break;
          }

          queryStringByteCount += singleValueRowQueryByteCount;
        }

        // Add a , separator for the collection of values in the INSERT QUERY.
        sqlBuilderForInsert.Append(singleRowValuesString);
        if (rowsSeparator.Length == 0)
        {
          rowsSeparator = "," + nl;
        }

        insertingRowsCount++;
      }

      return sqlBuilderForInsert.ToString();
    }

    /// <summary>
    /// Imports data contained in the given <see cref="MySqlDataTable"/> object into a <see cref="ExcelInterop.Range"/>.
    /// </summary>
    /// <param name="createPivotTable">Flag indicating whether a <see cref="ExcelInterop.PivotTable"/> is created for the imported data.</param>
    /// <param name="pivotPosition">The position where new <see cref="ExcelInterop.PivotTable"/> objects are placed relative to imported table's data.</param>
    /// <param name="addSummaryFields">Indicates whether to include a row with summary fields at the end of the data rows.</param>
    /// <returns>The <see cref="ExcelInterop.Range"/> or <see cref="ExcelInterop.ListObject"/> containing the cells with the imported data.</returns>
    public object ImportDataIntoExcelRange(bool createPivotTable, ExcelUtilities.PivotTablePosition pivotPosition = ExcelUtilities.PivotTablePosition.Right, bool addSummaryFields = false)
    {
      var atCell = Globals.ThisAddIn.Application.ActiveCell;
      var importedExcelRange = ImportDataIntoExcelRange(atCell);
      if (createPivotTable)
      {
        ExcelUtilities.CreatePivotTable(importedExcelRange, pivotPosition, ExcelTableName);
      }

      return importedExcelRange;
    }

    /// <summary>
    /// Imports the table's data at the specified Excel cell into a plain <see cref="ExcelInterop.Range"/>.
    /// </summary>
    /// <param name="atCell">The starting Excel (left-most and top-most) cell where the imported data is placed.</param>
    /// <returns>The <see cref="ExcelInterop.Range"/> containing the cells with the imported data.</returns>
    public ExcelInterop.Range ImportDataIntoExcelRange(ExcelInterop.Range atCell)
    {
      var rowsCount = Rows.Count;
      if (rowsCount == 0 && !ImportColumnNames)
      {
        return null;
      }

      ExcelInterop.Range fillingRange;
      try
      {
        var currentRow = atCell.Row;
        var activeWorkbook = atCell.Worksheet.Parent as ExcelInterop.Workbook;
        var maxRowNumber = activeWorkbook.GetWorkbookMaxRowNumber();
        var cappedNumRows = Math.Min(rowsCount, maxRowNumber + (ImportColumnNames ? 0 : 1) - currentRow);
        var headerRowModifier = ImportColumnNames ? 1 : 0;
        var cappedNumRowsWithHeaderRow = cappedNumRows + headerRowModifier;
        fillingRange = atCell.SafeResize(cappedNumRowsWithHeaderRow, Columns.Count);
        var fillingArray = new object[cappedNumRowsWithHeaderRow, Columns.Count];
        var excelMinDate = new DateTime(1900, 1, 1);

        // Fill the values of the column names if they are flagged to be imported
        if (ImportColumnNames)
        {
          for (var currCol = 0; currCol < Columns.Count; currCol++)
          {
            fillingArray[0, currCol] = Columns[currCol].ColumnName;
          }
        }

        // Skip Worksheet events
        Globals.ThisAddIn.SkipWorksheetChangeEvent = true;
        Globals.ThisAddIn.SkipSelectedDataContentsDetection = true;

        // Fill data values
        for (var rowIndex = 0; rowIndex < cappedNumRows; rowIndex++)
        {
          var absRowIndex = rowIndex + headerRowModifier;
          if (!(Rows[rowIndex] is MySqlDataRow mySqlRow))
          {
            continue;
          }

          for (var currentColumnIndex = 0; currentColumnIndex < Columns.Count; currentColumnIndex++)
          {
            var cellValue = mySqlRow[currentColumnIndex];

            if (cellValue is TimeSpan)
            {
              // Convert TimeSpan data to a format Excel recognizes as native time.
              cellValue = ((TimeSpan)mySqlRow[currentColumnIndex]).TotalDays;
            }
            else if (cellValue is DateTime dateValue)
            {
              // Convert DateTime values before the Excel minimum date of "1/1/1900" to text, otherwise Excel will throw an error
              if (dateValue < excelMinDate)
              {
                cellValue = dateValue.ToString(CultureInfo.CurrentCulture);
              }
            }
            else if (cellValue is Geometry geomValue)
            {
              cellValue = geomValue.ToString(Globals.ThisAddIn.SpatialDataAsTextFormat);
            }

            fillingArray[absRowIndex, currentColumnIndex] = cellValue;
          }

          mySqlRow.ExcelRange = fillingRange.Rows[absRowIndex + 1] as ExcelInterop.Range;
        }

        // Clear any formatting
        Globals.ThisAddIn.Application.Goto(fillingRange, false);

        // Format Excel column ranges depending on the imported column data types
        FormatImportExcelRange(fillingRange, false);

        // Assign values
        fillingRange.Value = fillingArray;

        // Format column names for imported range
        if (ImportColumnNames)
        {
          fillingRange.SetHeaderStyle();
        }

        fillingRange.Columns.AutoFit();
        fillingRange.Rows.AutoFit();
        atCell.Select();
      }
      catch (Exception ex)
      {
        fillingRange = null;
        Logger.LogException(ex, true, Resources.ImportDataErrorDetailText);
      }
      finally
      {
        Globals.ThisAddIn.SkipSelectedDataContentsDetection = false;
        Globals.ThisAddIn.SkipWorksheetChangeEvent = false;
      }

      return fillingRange;
    }

    /// <summary>
    /// Imports data contained in the given <see cref="MySqlDataTable"/> object into a <see cref="ExcelInterop.ListObject"/>.
    /// </summary>
    /// <param name="createPivotTable">Flag indicating whether a <see cref="ExcelInterop.PivotTable"/> is created for the imported data.</param>
    /// <param name="pivotPosition">The position where new <see cref="ExcelInterop.PivotTable"/> objects are placed relative to imported table's data.</param>
    /// <param name="addSummaryFields">Indicates whether to include a row with summary fields at the end of the data rows.</param>
    /// <returns>The <see cref="ExcelInterop.Range"/> or <see cref="ExcelInterop.ListObject"/> containing the cells with the imported data.</returns>
    public object ImportDataIntoExcelTable(bool createPivotTable, ExcelUtilities.PivotTablePosition pivotPosition = ExcelUtilities.PivotTablePosition.Right, bool addSummaryFields = false)
    {
      var atCell = Globals.ThisAddIn.Application.ActiveCell;
      var importedExcelTable = ImportDataIntoExcelTable(atCell, addSummaryFields);
      if (createPivotTable)
      {
        ExcelUtilities.CreatePivotTable(importedExcelTable, pivotPosition, ExcelTableName);
      }

      return importedExcelTable;
    }

    /// <summary>
    /// Imports the table's data at the specified Excel cell into a <see cref="ExcelInterop.ListObject"/>.
    /// </summary>
    /// <param name="atCell">The starting Excel (left-most and top-most) cell where the imported data is placed.</param>
    /// <param name="addSummaryRow">Flag indicating whether to include a row with summary fields at the end of the data rows.</param>
    /// <returns>The created <see cref="ExcelInterop.ListObject"/> containing the imported data.</returns>
    public ExcelInterop.ListObject ImportDataIntoExcelTable(ExcelInterop.Range atCell, bool addSummaryRow = false)
    {
      var startingRow = ImportColumnNames ? 1 : 0;
      var rowsCount = Rows.Count + startingRow;
      if (rowsCount == 0)
      {
        return null;
      }

      ImportConnectionInfo importConnectionInfo = null;
      try
      {
        Globals.ThisAddIn.SkipSelectedDataContentsDetection = true;
        Globals.ThisAddIn.Application.Goto(atCell, false);

        // Create Excel Table for the imported data
        importConnectionInfo = new ImportConnectionInfo(this, atCell, addSummaryRow);
        atCell.Select();
      }
      catch (Exception ex)
      {
        Logger.LogException(ex, true, Resources.ImportDataErrorDetailText);
      }
      finally
      {
        Globals.ThisAddIn.SkipSelectedDataContentsDetection = false;
      }

      return importConnectionInfo?.ExcelTable;
    }

    /// <summary>
    /// Prepares the array of objects to be inserted to a new <see cref="DataRow"/> of this <see cref="MySqlDataTable"/> to format its values (dates and strings) properly.
    /// </summary>
    /// <param name="itemArray">An array of objects to be loaded in a single <see cref="DataRow"/> of this <see cref="MySqlDataTable"/>.</param>
    /// <param name="escapeFormulaTexts">Flag indicating whether any value in the given item array must be checked to escape a starting equals sign so Excel does not mistake it as a formula.</param>
    public void PrepareCopyingItemArray(ref object[] itemArray, bool escapeFormulaTexts)
    {
      if (itemArray == null || itemArray.Length != Columns.Count)
      {
        return;
      }

      for (var colIdx = 0; colIdx < Columns.Count; colIdx++)
      {
        var importingValue = Columns[colIdx] is MySqlDataColumn targetColumn
          ? targetColumn.GetInsertingValueForType(itemArray[colIdx], false)
          : itemArray[colIdx];
        if (escapeFormulaTexts)
        {
          importingValue = importingValue.EscapeStartingEqualSign();
        }

        itemArray[colIdx] = importingValue;
      }
    }

    /// <summary>
    /// Pushes all changes in this table's data to its corresponding database table.
    /// </summary>
    /// <param name="showMySqlScriptDialog">Flag indicating whether the <see cref="MySqlScriptDialog"/> is shown before applying the query.</param>
    /// <returns>List of MySql data rows </returns>
    public List<IMySqlDataRow> PushData(bool showMySqlScriptDialog)
    {
      if (!CreateTableWithoutData && ChangedOrDeletedRows == 0)
      {
        return null;
      }

      using (var sqlScriptDialog = new MySqlScriptDialog(this, OperationType.IsForEdit()))
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
    /// <remarks>This method is designed so it throws an <see cref="Exception"/> if it occurs, any consumers must handle the thrown <see cref="Exception"/>.</remarks>
    public void RefreshData()
    {
      Clear();
      var filledTable = WbConnection.GetDataFromSelectQuery(SelectQuery, OperationType == DataOperationType.ImportProcedure ? ProcedureResultSetIndex : 0);
      if (OperationType == DataOperationType.ImportProcedure)
      {
        CopyTableSchemaAndData(filledTable);
        if (!TableName.EndsWith(DbProcedure.OUT_AND_RETURN_VALUES_TABLE_NAME))
        {
          return;
        }

        foreach (var column in Columns.Cast<MySqlDataColumn>().Where(column => column.ColumnName.StartsWith("@")))
        {
          column.ColumnName = column.ColumnName.Substring(1);
          column.SetDisplayName(column.ColumnName, false);
        }
      }
      else
      {
        CreateTableSchema(TableName);
        CopyTableData(filledTable, false);
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
      var notDeletedIdx = -1;
      if (Rows.Count == editingRangeRowsCount - 2)
      {
        return excelRowIdx;
      }

      for (var tableRowIdx = 0; tableRowIdx < Rows.Count; tableRowIdx++)
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
    /// <param name="limitRowsQuantity">Limit the number of loaded rows to this quantity. If less than 1 it means no limit is applied.</param>
    /// <return><c>true</c> if the columns setup is successful, <c>false</c> otherwise.</return>
    public bool SetupColumnsWithData(ExcelInterop.Range dataRange, bool recreateColumnsFromData, int limitRowsQuantity = 0)
    {
      Clear();

      // Add the Excel data to rows in this table.
      using (var temporaryRange = new TempRange(dataRange, true, false, true, _addPrimaryKeyColumn, _firstRowContainsColumnNames, limitRowsQuantity))
      {
        CreateColumns(temporaryRange, recreateColumnsFromData);
        var success = AddExcelData(temporaryRange);
        if (!success || !DetectDataType)
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
    /// Adds or removes warnings related to this table's auto-generated primary key.
    /// </summary>
    /// <param name="show">Flag indicating whether the warning is to be shown or hidden.</param>
    public void UpdateAutoPkWarning(bool show)
    {
      if (_autoPkWarnings.SetVisibility(DUPLICATE_PK_NAME_WARNING_KEY, show))
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
      if (IsPreviewTable)
      {
        return;
      }

      if (_copyingTableData)
      {
        if (e.Action != DataRowAction.Add)
        {
          return;
        }

        if (e.Row is MySqlDataRow mySqlRow)
        {
          mySqlRow.RowAdded();
        }

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
      TableWarningsChanged?.Invoke(column, new TableWarningsChangedArgs(column));
    }

    /// <summary>
    /// Raises the <see cref="TableWarningsChanged"/> event.
    /// </summary>
    /// <param name="autoPkWarning">Flag indicating if the warning is related to the auto-generated primary key or to the table.</param>
    protected virtual void OnTableWarningsChanged(bool autoPkWarning)
    {
      TableWarningsChanged?.Invoke(this, new TableWarningsChangedArgs(this, autoPkWarning));
    }

    /// <summary>
    /// Recreates the values of the automatically created first column for the table's primary key depending on the value of the <see cref="_firstRowContainsColumnNames"/> field.
    /// </summary>
    private void AdjustAutoPkValues()
    {
      if (!AddPrimaryKeyColumn || Columns.Count <= 0)
      {
        return;
      }

      var adjustIdx = _firstRowContainsColumnNames ? 0 : 1;
      for (var i = 0; i < Rows.Count; i++)
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

      TableColumnPropertyValueChanged?.Invoke(sender as MySqlDataColumn, args);
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
    /// <param name="fromSourceTable"><see cref="DataTable"/> object containing previously retrieved data from a MySQL table.</param>
    /// <param name="preserveChanges">Flag indicating whether the <see cref="DataRow.RowState"/> is preserved from the source copied rows..</param>
    private void CopyTableData(DataTable fromSourceTable, bool preserveChanges)
    {
      if (fromSourceTable == null)
      {
        return;
      }

      try
      {
        // Save the value of the computed property in a variable to avoid recalculating it over and over in the loop below.
        var escapeFormulaTexts = EscapeFormulaTexts;

        _copyingTableData = true;
        BeginLoadData();
        for (var rowIndex = 0; rowIndex < fromSourceTable.Rows.Count; rowIndex++)
        {
          if (FirstRowContainsColumnNames && rowIndex == 0)
          {
            continue;
          }

          var rowValues = fromSourceTable.Rows[rowIndex].ItemArray;
          PrepareCopyingItemArray(ref rowValues, escapeFormulaTexts);
          LoadDataRow(rowValues, !preserveChanges);
        }
      }
      catch (Exception ex)
      {
        Logger.LogException(ex, true, Resources.TableDataCopyErrorTitle);
      }
      finally
      {
        EndLoadData();
        _copyingTableData = false;
      }
    }

    /// <summary>
    /// Copies the schema and data contents of the given <see cref="DataTable"/> object to this table.
    /// </summary>
    /// <param name="filledTable"><see cref="DataTable"/> object containing previously retrieved data from a MySQL table.</param>
    /// <param name="schemaOnly">Flag indicating whether only the schema is copied without data.</param>
    private void CopyTableSchemaAndData(DataTable filledTable, bool schemaOnly = false)
    {
      var columnsInfoTable = filledTable.GetColumnsInformationTable(OperationType == DataOperationType.ImportProcedure);
      CreateTableSchema(columnsInfoTable);
      if (!schemaOnly)
      {
        CopyTableData(filledTable, false);
      }
    }

    /// <summary>
    /// Adds a specified number of <see cref="MySqlDataColumn"/> objects to the Columns collection where the first column may be an automatically created one for the table's primary index.
    /// </summary>
    /// <param name="temporaryRange">Excel data range in a temporary Excel worksheet containing the data to create columns for.</param>
    /// <param name="recreateColumnsFromData">Flag indicating if any existing columns in the table must be dropped and re-created based on the given data range.</param>
    private void CreateColumns(TempRange temporaryRange, bool recreateColumnsFromData)
    {
      if (temporaryRange?.Range == null || !recreateColumnsFromData && Columns.Count > 0)
      {
        return;
      }

      // Drop all columns and re-create them or create them if none have been created so far.
      Columns.Clear();
      var isAutoPkRange = temporaryRange.RangeType == TempRange.TempRangeType.AutoPkRange;
      var colIdx = isAutoPkRange ? 0 : 1;
      foreach (ExcelInterop.Range sourceColumnRange in temporaryRange.Range.Columns)
      {
        var autoPk = isAutoPkRange && sourceColumnRange.Column == 1;
        var column = new MySqlDataColumn(OperationType.IsForExport(), autoPk, "Column" + colIdx, sourceColumnRange.Column);
        column.ColumnWarningsChanged += ColumnWarningsChanged;
        column.PropertyChanged += ColumnPropertyValueChanged;
        Columns.Add(column);
        column.SubscribeToParentTablePropertyChange();
        colIdx++;
      }

      ResetAutoPkcolumnName();
    }

    /// <summary>
    /// Creates columns for this table using the information schema of a MySQL table with the given name to mirror their properties.
    /// </summary>
    /// <param name="tableName">Name of the table.</param>
    /// <param name="beautifyDataTypes">Flag indicating whether the data types are camel cased as shown in the Export Data data type combo box.</param>
    private void CreateTableSchema(string tableName, bool beautifyDataTypes = false)
    {
      var tableCollation = WbConnection.GetTableCollation(SchemaName, tableName, out var tableCharSet);
      if (!string.IsNullOrEmpty(tableCollation))
      {
        CharSet = tableCharSet;
        Collation = tableCollation;
      }

      var columnsInfoTable = WbConnection.GetColumnsInformationTable(SchemaName, tableName, beautifyDataTypes);
      CreateTableSchema(columnsInfoTable);
    }

    /// <summary>
    /// Creates columns for this table using the information schema of a MySQL table with the given name to mirror their properties.
    /// </summary>
    /// <param name="schemaInfoTable">A <see cref="MySqlColumnsInformationTable"/>.</param>
    private void CreateTableSchema(MySqlColumnsInformationTable schemaInfoTable)
    {
      if (schemaInfoTable == null)
      {
        return;
      }

      Columns.Clear();
      var columnsNames = SelectQuery.GetColumnNamesArrayFromSelectQuery();
      foreach (DataRow columnInfoRow in schemaInfoTable.Rows)
      {
        var colName = columnInfoRow["Name"].ToString();
        if (columnsNames != null
            && columnsNames.All(c => string.Compare(c, colName, CultureInfo.InvariantCulture, CompareOptions.IgnoreCase) != 0))
        {
          continue;
        }

        var dataType = columnInfoRow["Type"].ToString();
        if ((OperationType.IsForImport() || OperationType.IsForEdit())
            && Settings.Default.ImportFloatingPointDataAsDecimal)
        {
          if (dataType.StartsWith("float", StringComparison.OrdinalIgnoreCase))
          {
            dataType = $"Decimal{dataType.Substring(5)}";
          }
          else if (dataType.StartsWith("double", StringComparison.OrdinalIgnoreCase))
          {
            dataType = $"Decimal{dataType.Substring(6)}";
          }
        }

        var allowNulls = columnInfoRow["Null"].ToString() == "YES";
        var keyInfo = columnInfoRow["Key"].ToString();
        var defaultValue = columnInfoRow["Default"].ToString();
        var charSet = columnInfoRow["CharSet"].ToString();
        var collation = columnInfoRow["Collation"].ToString();
        var extraInfo = columnInfoRow["Extra"].ToString();
        var column = new MySqlDataColumn(colName, dataType, charSet, collation, false, allowNulls, keyInfo, defaultValue, extraInfo);
        column.PropertyChanged += ColumnPropertyValueChanged;
        Columns.Add(column);
        column.SubscribeToParentTablePropertyChange();
      }
    }

    /// <summary>
    /// Gets a piece of a CREATE TABLE statement containing only the index definitions given the index type.
    /// </summary>
    /// <param name="sqlType">The type of SQL statement used for the new table.</param>
    /// <param name="indexType">The type of index created for this table.</param>
    /// <param name="indexesDelimiter">The delimiter used to separate each index definition.</param>
    /// <returns>A piece of a CREATE TABLE statement containing only the index definitions given the index type.</returns>
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

      var delimiterStartsWithComma = indexesDelimiter.StartsWith(",");
      var alterTable = sqlType == NewTableSqlType.AlterComplete || sqlType == NewTableSqlType.AlterOnlyKeys;
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
      var colsSeparator = string.Empty;
      var valuesSeparator = OperationType.IsForEdit()
                               || (OperationType.IsForExport() && Settings.Default.ExportGenerateMultipleInserts)
                               || (OperationType.IsForAppend() && Settings.Default.AppendGenerateMultipleInserts)
        ? " "
        : Environment.NewLine;
      var insertStatement = MySqlStatement.STATEMENT_INSERT;
      if (OperationType.IsForAppend())
      {
        if (Enum.TryParse(Settings.Default.AppendDuplicateUniqueValuesAction, out AppendDuplicateValuesActionType duplicateValuesAction))
        {
          switch (duplicateValuesAction)
          {
            case AppendDuplicateValuesActionType.IgnoreDuplicates:
              insertStatement = MySqlStatement.STATEMENT_INSERT_IGNORE;
              break;

            case AppendDuplicateValuesActionType.ReplaceDuplicates:
              insertStatement = MySqlStatement.STATEMENT_REPLACE;
              break;
          }
        }
      }

      PreSqlBuilder.Append(insertStatement);
      PreSqlBuilder.AppendFormat(" `{0}`.`{1}`{2}(", SchemaName, TableNameForSqlQueries, valuesSeparator);
      foreach (var column in ColumnsForInsertion)
      {
        PreSqlBuilder.AppendFormat("{0}`{1}`", colsSeparator, column.DisplayNameForSqlQueries);
        colsSeparator = ",";
      }

      PreSqlBuilder.AppendFormat("){0}VALUES{0}", valuesSeparator);
      return PreSqlBuilder.ToString();
    }

    /// <summary>
    /// Gets the length of the first part a SQL query generated for this table may have.
    /// The first part contains schema, table and column names.
    /// </summary>
    /// <returns>The length of the first part a SQL query generated for this table may have.</returns>
    private int GetPreSqlLength()
    {
      var colNamesLength = Columns.Cast<MySqlDataColumn>().Sum(column => column.DisplayNameForSqlQueries.Length + 2);
      return colNamesLength + TableNameForSqlQueries.Length + SchemaName.Length + PRE_SQL_PADDING_LENGTH;
    }

    /// <summary>
    /// Raises the <see cref="PropertyChanged"/> event.
    /// </summary>
    /// <param name="propertyName">The name of the property whose value changed.</param>
    private void OnPropertyChanged(string propertyName)
    {
      PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
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

      var autoPKcolumn = GetColumnAtIndex(0);
      var tableIdName = TableName + (TableName.Length > 0 ? "_" : string.Empty) + "id";
      var autoPkName = GetNonDuplicateColumnName(tableIdName);
      autoPKcolumn.SetDisplayName(autoPkName, true);
    }

    /// <summary>
    /// Resets the value of <see cref="MySqlDataRow.IsHeadersRow"/> with the value of <see cref="FirstRowContainsColumnNames"/>.
    /// </summary>
    private void ResetFirstRowIsHeaderValue()
    {
      if (Rows.Count <= 0)
      {
        return;
      }

      if (Rows[0] is MySqlDataRow firstRow)
      {
        firstRow.IsHeadersRow = FirstRowContainsColumnNames;
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
      var mySqlRow = (MySqlDataRow)args.Row;
      mySqlRow.RowChanged(args.Action);
    }

    /// <summary>
    /// Initializes the warnings container for this column.
    /// </summary>
    private void SetupWarnings()
    {
      _autoPkWarnings = new WarningsContainer(WarningsContainer.CurrentWarningChangedMethodType.OnShowIfWarningNotPresent, 1);
      _autoPkWarnings.Add(DUPLICATE_PK_NAME_WARNING_KEY, Resources.PrimaryKeyColumnExistsWarning);

      _tableWarnings = new WarningsContainer(WarningsContainer.CurrentWarningChangedMethodType.OnShowIfWarningNotPresent, 3);
      _tableWarnings.Add(EMPTY_TABLE_NAME_WARNING_KEY, Resources.TableNameRequiredWarning);
      _tableWarnings.Add(DUPLICATE_TABLE_NAME_WARNING_KEY, Resources.TableNameExistsWarning);
      _tableWarnings.Add(NON_STANDARD_TABLE_NAME_WARNING_KEY, Resources.NamesWarning);
    }

    /// <summary>
    /// Updates the warnings related to the table name and the select query used to retrieve data based on the <see cref="TableName"/> property's value.
    /// </summary>
    private void UpdateTableNameWarningsAndSelectQuery()
    {
      // Update warning stating the table name cannot be empty
      var emptyTableName = string.IsNullOrWhiteSpace(TableName);
      var warningsChanged = _tableWarnings.SetVisibility(EMPTY_TABLE_NAME_WARNING_KEY, emptyTableName);
      IsTableNameValid = !emptyTableName;

      // Update warning stating a table with the given name already exists in the database
      if (IsTableNameValid && WbConnection != null)
      {
        warningsChanged = _tableWarnings.SetVisibility(DUPLICATE_TABLE_NAME_WARNING_KEY, TableExistsInSchema) || warningsChanged;
        IsTableNameValid = !TableExistsInSchema;
      }

      // Update warning stating the table name should not contain spaces or upper-case letters
      if (IsTableNameValid)
      {
        var nonStandardTableName = TableName.Contains(" ") || TableName.Any(char.IsUpper);
        warningsChanged = _tableWarnings.SetVisibility(NON_STANDARD_TABLE_NAME_WARNING_KEY, nonStandardTableName) || warningsChanged;
      }

      // Fire the TableWarningsChanged event.
      if (warningsChanged)
      {
        OnTableWarningsChanged(false);
      }
    }

    /// <summary>
    /// Updates the table's SELECT query based on the current table name.
    /// </summary>
    private void UpdateTableSelectQuery()
    {
      var schemaPiece = !string.IsNullOrEmpty(SchemaName) ? $"`{SchemaName}`." : string.Empty;
      SelectQuery = $"SELECT * FROM {schemaPiece}`{TableNameForSqlQueries}`";
    }

    /// <summary>
    /// Updates the column names and the automatically generated primary key column values depending on the value of the <see cref="_firstRowContainsColumnNames"/> field.
    /// </summary>
    private void UseFirstRowAsHeaders()
    {
      if (Rows.Count == 0)
      {
        return;
      }

      var row = Rows[0];
      if (!_changedColumnNamesWithFirstRowOfData && AddPrimaryKeyColumn && Columns.Count > 0)
      {
        var autoPkCol = GetColumnAtIndex(0);
        var autoPkName = autoPkCol.DisplayName;
        autoPkName = row.ItemArray.Skip(1).Select(obj => obj.ToString()).ToList().GetNonDuplicateText(autoPkName);
        autoPkCol.SetDisplayName(autoPkName, true);
      }

      // Set all column names first without warning for any duplicates, since at this stage duplicates may be found if the names given to columns
      // look like "Column1", "Column2", etc. because that naming convention is the one used for columns when they are created.
      var mySqlColumns = Columns.Cast<MySqlDataColumn>().Skip(AddPrimaryKeyColumn ? 1 : 0).ToList();
      foreach (var mySqlCol in mySqlColumns)
      {
        mySqlCol.SetDisplayName(_firstRowContainsColumnNames ? row[mySqlCol.Ordinal].ToString().ToValidMySqlColumnName() : mySqlCol.ColumnName, false);
        if (mySqlCol.MySqlDataTypeOverridenByUser)
        {
          mySqlCol.SetMySqlDataType(mySqlCol.MySqlDataType.FullType, mySqlCol.MySqlDataType.IsValid, true, true);
        }
        else
        {
          mySqlCol.SetMySqlDataType(_firstRowContainsColumnNames ? MySqlDataColumn.MySqlDataTypeFromRowType.FromSecond : MySqlDataColumn.MySqlDataTypeFromRowType.FromFirst);
        }
      }

      // Check about duplicate column names now that all column names were set to the ones given by the user.
      CheckForDuplicatedColumnDisplayNames();

      AdjustAutoPkValues();
      ResetFirstRowIsHeaderValue();
      _changedColumnNamesWithFirstRowOfData = true;
    }
  }
}