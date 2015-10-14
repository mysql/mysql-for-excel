// Copyright (c) 2012-2015, Oracle and/or its affiliates. All rights reserved.
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
using MySQL.ForExcel.Classes.EventArguments;
using MySQL.ForExcel.Properties;
using Excel = Microsoft.Office.Interop.Excel;

namespace MySQL.ForExcel.Classes
{
  /// <summary>
  /// Represents the schema of a MySQL table's column.
  /// </summary>
  public class MySqlDataColumn : DataColumn, INotifyPropertyChanged
  {
    #region Constants

    /// <summary>
    /// Key used to represent a warning about the column's data not being suitable for the data type in the Append Data dialog.
    /// </summary>
    private const string DATA_NOT_SUITABLE_APPEND_WARNING_KEY = "DATA_NOT_SUITABLE_APPEND";

    /// <summary>
    /// Key used to represent a warning about the column's data not being suitable for the data type in the Export Data dialog.
    /// </summary>
    private const string DATA_NOT_SUITABLE_EXPORT_WARNING_KEY = "DATA_NOT_SUITABLE_EXPORT";

    /// <summary>
    /// Key used to represent a warning about the column's data not being unique.
    /// </summary>
    private const string DATA_NOT_UNIQUE_WARNING_KEY = "DATA_NOT_UNIQUE";

    /// <summary>
    /// The data type used for columns with no data.
    /// </summary>
    private const string DEFAULT_FULL_DATA_TYPE_FOR_EMPTY_COLUMNS = "VarChar(255)";

    /// <summary>
    /// Key used to represent a warning about the column's name being a duplicate of one in another column.
    /// </summary>
    private const string DUPLICATE_NAME_WARNING_KEY = "DUPLICATE_NAME";

    /// <summary>
    /// Key used to represent a warning about the column's name being null or empty.
    /// </summary>
    private const string EMPTY_NAME_WARNING_KEY = "EMPTY_NAME";

    /// <summary>
    /// Key used to represent a warning about the column's data type being a MySQL invalid one.
    /// </summary>
    private const string INVALID_DATA_TYPE_WARNING_KEY = "INVALID_DATA_TYPE";

    /// <summary>
    /// Key used to represent a warning about the column's data type declaraton for an ENUM or SET being incorrect due to a specific element.
    /// </summary>
    private const string INVALID_SET_ENUM_WARNING_KEY = "INVALID_SET_ENUM";

    /// <summary>
    /// Key used to represent a warning about the column's data type being null or empty.
    /// </summary>
    private const string NO_DATA_TYPE_WARNING_KEY = "NO_DATA_TYPE";

    #endregion Constants

    #region Fields

    /// <summary>
    /// Flag indicating whether the column will accept null values.
    /// </summary>
    private bool _allowNull;

    /// <summary>
    /// The <see cref="DataColumn.ColumnName"/> escaping the back-tick character.
    /// </summary>
    private string _columnNameForSqlQueries;

    /// <summary>
    /// Flag indicating whether the column's data must be wrapped with quotes to be used in queries.
    /// </summary>
    private bool? _columnRequiresQuotes;

    /// <summary>
    /// Flag indicating whether this column has an index automatically created for it.
    /// </summary>
    private bool _createIndex;

    /// <summary>
    /// Flag indicating whether this column will be excluded from the list of columns to be created on a new table's creation.
    /// </summary>
    private bool _excludeColumn;

    /// <summary>
    /// List of indexes of elements of a SET or ENUM declaration that are improperly quoted.
    /// </summary>
    private List<int> _invalidSetOrEnumElementsIndexes;

    /// <summary>
    /// The ordinal index of the column in a source <see cref="MySqlDataTable"/> from which data will be appended from.
    /// </summary>
    private int _mappedDataColOrdinal;

    /// <summary>
    /// The corresponding data type supported by MySQL Server for this column.
    /// </summary>
    private string _mySqlDataType;

    /// <summary>
    /// Flag indicating whether the column is part of the primary key.
    /// </summary>
    private bool _primaryKey;

    /// <summary>
    /// The consistent data type that can hold the data for all rows starting from the first row.
    /// </summary>
    private string _rowsFromFirstDataType;

    /// <summary>
    /// The consistent data type that can hold the data for all rows starting from the second row.
    /// </summary>
    private string _rowsFromSecondDataType;

    /// <summary>
    /// List of elements included in a SET or ENUM declaration.
    /// </summary>
    private List<string> _setOrEnumElements;

    /// <summary>
    /// Flag indicating if the column has a related unique index.
    /// </summary>
    private bool _uniqueKey;

    /// <summary>
    /// Container with warnings for users about the column properties that could cause errors when creating this column in a database table.
    /// </summary>
    private WarningsContainer _warnings;

    /// <summary>
    /// Dictionary with additional information related to specific warnings.
    /// </summary>
    private Dictionary<string, Tuple<string, string>> _warningsMoreInfosDictionary;

    #endregion Fields

    /// <summary>
    /// Initializes a new instance of the <see cref="MySqlDataColumn"/> class.
    /// </summary>
    public MySqlDataColumn()
    {
      _allowNull = false;
      _columnNameForSqlQueries = null;
      _columnRequiresQuotes = null;
      _excludeColumn = false;
      _invalidSetOrEnumElementsIndexes = null;
      _mappedDataColOrdinal = -1;
      _mySqlDataType = string.Empty;
      _rowsFromFirstDataType = string.Empty;
      _rowsFromSecondDataType = string.Empty;
      _setOrEnumElements = null;
      AutoIncrement = false;
      AutoPk = false;
      CharSet = null;
      Collation = null;
      DisplayName = string.Empty;
      DuplicateGroupsFound = 0;
      InExportMode = false;
      IsDisplayNameDuplicate = false;
      IsMySqlDataTypeValid = true;
      MappedDataColName = null;
      PrimaryKey = false;
      RangeColumnIndex = 0;
      StrippedMySqlDataType = string.Empty;
      Unsigned = false;
      SetupWarnings();
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="MySqlDataColumn"/> class.
    /// </summary>
    /// <param name="inExportMode">Flag indicating if the column is being constructed for exporting it to a new MySQL table.</param>
    /// <param name="autoPk">Flag indicating whether the column is used in an auto-generated primary key.</param>
    /// <param name="columnName">The name of the column.</param>
    /// <param name="rangeColumnIndex">The index of the Excel range column used to populate this column with data.</param>
    public MySqlDataColumn(bool inExportMode, bool autoPk, string columnName, int rangeColumnIndex)
      : this()
    {
      AutoPk = autoPk;
      if (AutoPk)
      {
        AutoIncrement = true;
        PrimaryKey = true;
        SetMySqlDataType("Integer");
      }

      ColumnName = autoPk ? "AutoPK" : columnName;
      DisplayName = ColumnName;
      InExportMode = inExportMode;
      RangeColumnIndex = autoPk ? 0 : rangeColumnIndex;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="MySqlDataColumn"/> class.
    /// </summary>
    /// <param name="columnName">Name of the column.</param>
    /// <param name="mySqlFullDataType">Data type for a table column supported by MySQL Server.</param>
    /// <param name="charSet">The character set used to store text data in this column.</param>
    /// <param name="collation">The collation used with the character set to store text data in this column.</param>
    /// <param name="datesAsMySqlDates">Flag indicating if the data type for dates will be stored as <see cref="MySql.Data.Types.MySqlDateTime"/>
    /// or <see cref="System.DateTime"/>.</param>
    /// <param name="allowNulls">Flag indicating if the column will accept null values.</param>
    /// <param name="keyInfo">Information about the type of key this column belongs to.</param>
    /// <param name="extraInfo">Extra information related to the column's data type as stored by the MySQL server.</param>
    public MySqlDataColumn(string columnName, string mySqlFullDataType, string charSet, string collation, bool datesAsMySqlDates, bool allowNulls, string keyInfo, string extraInfo)
      : this()
    {
      keyInfo = keyInfo.ToUpperInvariant();
      extraInfo = extraInfo.ToLowerInvariant();
      DisplayName = ColumnName = columnName;
      AllowNull = allowNulls;
      Unsigned = mySqlFullDataType.ToLowerInvariant().Contains("unsigned");
      if (!string.IsNullOrEmpty(extraInfo))
      {
        AutoIncrement = extraInfo.Contains("auto_increment");
        AutoPk = extraInfo.Contains("auto_pk");
        ExcludeColumn = extraInfo.Contains("exclude");
      }

      CharSet = charSet;
      Collation = collation;
      MySqlDataType = mySqlFullDataType;
      DataType = DataTypeUtilities.NameToType(StrippedMySqlDataType, Unsigned, datesAsMySqlDates);
      CreateIndex = keyInfo == "MUL";
      PrimaryKey = keyInfo == "PRI";
      UniqueKey = keyInfo == "UNI";
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="MySqlDataColumn"/> class.
    /// </summary>
    /// <param name="columnName">Name of the column.</param>
    /// <param name="mySqlFullDataType">Data type for a table column supported by MySQL Server.</param>
    /// <param name="datesAsMySqlDates">Flag indicating if the data type for dates will be stored as <see cref="MySql.Data.Types.MySqlDateTime"/>
    /// or <see cref="System.DateTime"/>.</param>
    public MySqlDataColumn(string columnName, string mySqlFullDataType, bool datesAsMySqlDates)
      : this(columnName, mySqlFullDataType, null, null, datesAsMySqlDates, false, string.Empty, string.Empty)
    {
    }

    #region Enumerations

    /// <summary>
    /// Describes the type of a MySQL collecion data type.
    /// </summary>
    public enum CollectionDataType
    {
      /// <summary>
      /// An Enum type where only 1 value from a valid values list is stored.
      /// </summary>
      Enum,

      /// <summary>
      /// A Set type where a set of N values from a valid values list is stored.
      /// </summary>
      Set
    }

    #endregion Enumerations

    #region Properties

    /// <summary>
    /// Gets the collation used to store text data in this column, looking up if not defined at this element.
    /// </summary>
    public string AbsoluteCollation
    {
      get
      {
        return string.IsNullOrEmpty(Collation) ? ParentTable.AbsoluteCollation : Collation;
      }
    }

    /// <summary>
    /// Gets or sets a value indicating whether the column will accept null values.
    /// </summary>
    public bool AllowNull
    {
      get
      {
        return _allowNull;
      }

      set
      {
        _allowNull = value;
        OnPropertyChanged("AllowNull");
        if (_uniqueKey)
        {
          UpdateDataUniquenessWarnings();
        }
      }
    }

    /// <summary>
    /// Gets a value indicating whether the column is used in an auto-generated primary key.
    /// </summary>
    public bool AutoPk { get; private set; }

    /// <summary>
    /// Gets or sets the character set used to store text data in this column.
    /// </summary>
    /// <remarks>If null or empty it means the parent table character set is used.</remarks>
    public string CharSet { get; set; }

    /// <summary>
    /// Gets or sets the collation used with the character set to store text data in this column.
    /// </summary>
    /// <remarks>If null or empty it means the default collation is used.</remarks>
    public string Collation { get; set; }

    /// <summary>
    /// Gets or sets the name of the column in the <see cref="DataColumnCollection"/>.
    /// </summary>
    public new string ColumnName
    {
      get
      {
        return base.ColumnName;
      }

      set
      {
        if (AutoPk)
        {
          value = "AutoPK";
        }

        base.ColumnName = value;
        _columnNameForSqlQueries = null;
      }
    }

    /// <summary>
    /// Gets the <see cref="DataColumn.ColumnName"/> escaping the back-tick character.
    /// </summary>
    public string ColumnNameForSqlQueries
    {
      get
      {
        return _columnNameForSqlQueries ?? (_columnNameForSqlQueries = ColumnName.Replace("`", "``"));
      }
    }

    /// <summary>
    /// Gets a value indicating whether the column's data must be wrapped with quotes to be used in queries.
    /// </summary>
    public bool ColumnRequiresQuotes
    {
      get
      {
        if (_columnRequiresQuotes == null)
        {
          _columnRequiresQuotes = IsCharOrText || IsDate || IsSetOrEnum || IsTime;
        }

        return (bool)_columnRequiresQuotes;
      }
    }

    /// <summary>
    /// Gets or sets a value indicating whether this column has an index automatically created for it.
    /// </summary>
    public bool CreateIndex
    {
      get
      {
        return _createIndex;
      }

      set
      {
        _createIndex = value;
        if (!InExportMode)
        {
          return;
        }

        if (!PrimaryKey && ParentTable != null && (!_createIndex && ParentTable.AutoAllowEmptyNonIndexColumns))
        {
          AllowNull = true;
        }

        OnPropertyChanged("CreateIndex");
      }
    }

    /// <summary>
    /// Gets the last warning text associated to this column.
    /// </summary>
    public string CurrentWarningText
    {
      get
      {
        string currentWarningText = _warnings.CurrentWarningText;
        return ExcludeColumn || string.IsNullOrEmpty(currentWarningText)
          ? string.Empty
          : currentWarningText;
      }
    }

    /// <summary>
    /// Gets a tuple containing a title and description texts, of additional information related to the <see cref="CurrentWarningText"/>.
    /// </summary>
    public Tuple<string, string> CurrentWarningMoreInfo
    {
      get
      {
        return ExcludeColumn || string.IsNullOrEmpty(_warnings.CurrentWarningKey) || !_warningsMoreInfosDictionary.ContainsKey(_warnings.CurrentWarningKey)
          ? null
          : _warningsMoreInfosDictionary[_warnings.CurrentWarningKey];
      }
    }

    /// <summary>
    /// Gets the name for this column, when its value is different than the one in <see cref="DataColumn.ColumnName"/>
    /// it means the latter represents an internal name and this property holds the real column name.
    /// </summary>
    public string DisplayName { get; private set; }

    /// <summary>
    /// Gets the <see cref="DisplayName"/> escaping the back-tick character.
    /// </summary>
    public string DisplayNameForSqlQueries
    {
      get
      {
        return DisplayName.Replace("`", "``");
      }
    }

    /// <summary>
    /// Gets the number of duplicate groups found when doing a unique data check.
    /// </summary>
    public int DuplicateGroupsFound { get; private set; }

    /// <summary>
    /// Gets or sets a value indicating whether this column will be excluded from the list of columns to be created on a new table's creation.
    /// </summary>
    public bool ExcludeColumn
    {
      get
      {
        return _excludeColumn;
      }

      set
      {
        bool valueChanged = _excludeColumn != value;
        _excludeColumn = value;
        if (!InExportMode)
        {
          return;
        }

        if (_excludeColumn && !AutoPk && PrimaryKey)
        {
          PrimaryKey = false;
        }

        OnColumnWarningsChanged();
        OnPropertyChanged("ExcludeColumn");

        // Fire a duplicates check on all columns to update those warnings, since now that this column is
        // being excluded it should not be considered a duplicate of others.
        if (valueChanged && Table is MySqlDataTable)
        {
          ParentTable.CheckForDuplicatedColumnDisplayNames();
        }
      }
    }

    /// <summary>
    /// Gets a value indicating whether the column is included for INSERT queries.
    /// </summary>
    public bool IncludeForInsertion
    {
      get
      {
        return !ExcludeColumn && (!ParentTable.OperationType.IsForAppend() || MappedDataColOrdinal >= 0);
      }
    }

    /// <summary>
    /// Gets a value indicating whether the column is being constructed for exporting it to a new MySQL table.
    /// </summary>
    public bool InExportMode { get; private set; }

    /// <summary>
    /// Gets a value indicating whether this column's data type is of binary nature.
    /// </summary>
    public bool IsBinary
    {
      get
      {
        if (string.IsNullOrEmpty(StrippedMySqlDataType))
        {
          return false;
        }

        string toLowerDataType = StrippedMySqlDataType.ToLowerInvariant();
        return toLowerDataType.Contains("binary");
      }
    }

    /// <summary>
    /// Gets a value indicating whether this column's data type can hold boolean values.
    /// </summary>
    public bool IsBool
    {
      get
      {
        if (string.IsNullOrEmpty(StrippedMySqlDataType))
        {
          return false;
        }

        string toLowerDataType = MySqlDataType.ToLowerInvariant();
        return toLowerDataType.StartsWith("bool") || toLowerDataType == "tinyint(1)" || toLowerDataType == "bit(1)";
      }
    }

    /// <summary>
    /// Gets a value indicating whether this column's data type is fixed or variable sized character-based.
    /// </summary>
    public bool IsChar
    {
      get
      {
        if (string.IsNullOrEmpty(StrippedMySqlDataType))
        {
          return false;
        }

        string toLowerDataType = StrippedMySqlDataType.ToLowerInvariant();
        return toLowerDataType.Contains("char");
      }
    }

    /// <summary>
    /// Gets a value indicating whether this column's data type stores any kind of text.
    /// </summary>
    public bool IsCharOrText
    {
      get
      {
        if (string.IsNullOrEmpty(StrippedMySqlDataType))
        {
          return false;
        }

        string toLowerDataType = StrippedMySqlDataType.ToLowerInvariant();
        return toLowerDataType.Contains("char") || toLowerDataType.Contains("text");
      }
    }

    /// <summary>
    /// Gets a value indicating whether this column's data type is used for dates.
    /// </summary>
    public bool IsDate
    {
      get
      {
        if (string.IsNullOrEmpty(StrippedMySqlDataType))
        {
          return false;
        }

        string toLowerDataType = StrippedMySqlDataType.ToLowerInvariant();
        return toLowerDataType.Contains("date") || toLowerDataType == "timestamp";
      }
    }

    /// <summary>
    /// Gets a value indicating whether this column's data type is of floating-point nature.
    /// </summary>
    public bool IsDecimal
    {
      get
      {
        if (string.IsNullOrEmpty(StrippedMySqlDataType))
        {
          return false;
        }

        string toLowerDataType = StrippedMySqlDataType.ToLowerInvariant();
        return toLowerDataType == "real" || toLowerDataType == "double" || toLowerDataType == "float" || toLowerDataType == "decimal" || toLowerDataType == "numeric";
      }
    }

    /// <summary>
    /// Gets a value indicating if the <see cref="DisplayName"/> property value is not a duplicate of the one in another column.
    /// </summary>
    public bool IsDisplayNameDuplicate { get; private set; }

    /// <summary>
    /// Gets a value indicating whether this column's data type is integer-based.
    /// </summary>
    public bool IsInteger
    {
      get
      {
        if (string.IsNullOrEmpty(StrippedMySqlDataType))
        {
          return false;
        }

        string toLowerDataType = StrippedMySqlDataType.ToLowerInvariant();
        return toLowerDataType.Contains("int");
      }
    }

    /// <summary>
    /// Gets a value indicating whether the column's data type is a valid MySQL data type.
    /// </summary>
    public bool IsMySqlDataTypeValid { get; private set; }

    /// <summary>
    /// Gets a value indicating whether this column's data type is numeric.
    /// </summary>
    public bool IsNumeric
    {
      get
      {
        return IsDecimal || IsInteger;
      }
    }

    /// <summary>
    /// Gets a value indicating whether this column's data type is Set or Enumeration.
    /// </summary>
    public bool IsSetOrEnum
    {
      get
      {
        if (string.IsNullOrEmpty(StrippedMySqlDataType))
        {
          return false;
        }

        string toLowerDataType = StrippedMySqlDataType.ToLowerInvariant();
        return toLowerDataType.StartsWith("set") || toLowerDataType.StartsWith("enum");
      }
    }

    /// <summary>
    /// Gets a value indicating whether this column's data type is Time.
    /// </summary>
    public bool IsTime
    {
      get
      {
        return !string.IsNullOrEmpty(StrippedMySqlDataType) &&
               StrippedMySqlDataType.Equals("time", StringComparison.InvariantCultureIgnoreCase);
      }
    }

    /// <summary>
    /// Gets or sets the name of the column in a source <see cref="MySqlDataTable"/> from which data will be appended from.
    /// </summary>
    public string MappedDataColName { get; set; }

    /// <summary>
    /// Gets or sets the ordinal index of the column in a source <see cref="MySqlDataTable"/> from which data will be appended from.
    /// </summary>
    public int MappedDataColOrdinal
    {
      get
      {
        return _mappedDataColOrdinal;
      }

      set
      {
        _mappedDataColOrdinal = value;
        OnPropertyChanged("MappedDataColOrdinal");
      }
    }

    /// <summary>
    /// Gets the corresponding data type supported by MySQL Server for this column.
    /// </summary>
    public string MySqlDataType
    {
      get
      {
        return _mySqlDataType;
      }

      private set
      {
        _mySqlDataType = value;
        _columnRequiresQuotes = null;
        long dataTypeLength;
        StrippedMySqlDataType = DataTypeUtilities.GetStrippedMySqlDataType(_mySqlDataType, true, out dataTypeLength);
        MySqlDataTypeLength = dataTypeLength;
        OnPropertyChanged("MySqlDataType");
      }
    }

    /// <summary>
    /// Gets the corresponding data type supported by MySQL Server for this column.
    /// </summary>
    public long MySqlDataTypeLength { get; private set; }

    /// <summary>
    /// Gets a <see cref="MySql.Data.MySqlClient.MySqlDbType"/> object corresponding to this column's data type.
    /// </summary>
    public MySqlDbType MySqlDbType
    {
      get
      {
        string strippedType = StrippedMySqlDataType;
        return !string.IsNullOrEmpty(strippedType)
          ? DataTypeUtilities.NameToMySqlType(strippedType, Unsigned, false)
          : MySqlDbType.VarChar;
      }
    }

    /// <summary>
    /// Gets the parent table of this column as a <see cref="MySqlDataTable"/> object.
    /// </summary>
    public MySqlDataTable ParentTable
    {
      get
      {
        return Table as MySqlDataTable;
      }
    }

    /// <summary>
    /// Gets or sets a value indicating whether the column is part of the primary key.
    /// </summary>
    public bool PrimaryKey
    {
      get
      {
        return _primaryKey;
      }

      set
      {
        _primaryKey = value;
        if (!InExportMode)
        {
          return;
        }

        if (_primaryKey)
        {
          CreateIndex = false;
          UniqueKey = false;
          AllowNull = false;
        }

        OnPropertyChanged("PrimaryKey");
      }
    }

    /// <summary>
    /// Gets the index of the Excel range column used to populate this column with data.
    /// </summary>
    public int RangeColumnIndex { get; private set; }

    /// <summary>
    /// Gets the consistent data type that can hold the data for all rows starting from the first row.
    /// </summary>
    public string RowsFromFirstDataType
    {
      get
      {
        return _rowsFromFirstDataType;
      }

      set
      {
        _rowsFromFirstDataType = string.IsNullOrEmpty(value)
          ? DEFAULT_FULL_DATA_TYPE_FOR_EMPTY_COLUMNS
          : value;
      }
    }

    /// <summary>
    /// Gets the consistent data type that can hold the data for all rows starting from the second row.
    /// </summary>
    public string RowsFromSecondDataType
    {
      get
      {
        return _rowsFromSecondDataType;
      }

      set
      {
        _rowsFromSecondDataType = string.IsNullOrEmpty(value)
          ? DEFAULT_FULL_DATA_TYPE_FOR_EMPTY_COLUMNS
          : value;
      }
    }

    /// <summary>
    /// Gets the MySQL data type descriptor without any options wrapped by parenthesis.
    /// </summary>
    public string StrippedMySqlDataType { get; private set; }

    /// <summary>
    /// Gets or sets a value indicating whether the column has a related unique index.
    /// </summary>
    public bool UniqueKey
    {
      get
      {
        return _uniqueKey;
      }

      set
      {
        _uniqueKey = value;
        if (!InExportMode)
        {
          return;
        }

        if (_uniqueKey)
        {
          CreateIndex = true;
          PrimaryKey = false;
        }

        UpdateDataUniquenessWarnings();
        OnPropertyChanged("UniqueKey");
      }
    }

    /// <summary>
    /// Gets or sets a value indicating whether numeric data in this column is unsigned, meaningful only if the data type is integer-based.
    /// </summary>
    public bool Unsigned { get; set; }

    #endregion Properties

    #region Events

    /// <summary>
    /// Delegate handler for the <see cref="ColumnWarningsChanged"/> event.
    /// </summary>
    /// <param name="sender">Sender object.</param>
    /// <param name="args">Event arguments.</param>
    public delegate void ColumnWarningsChangedEventHandler(object sender, ColumnWarningsChangedArgs args);

    /// <summary>
    /// Occurs when the warnings associated to this column change.
    /// </summary>
    public event ColumnWarningsChangedEventHandler ColumnWarningsChanged;

    /// <summary>
    /// Occurs when a property value changes.
    /// </summary>
    public event PropertyChangedEventHandler PropertyChanged;

    #endregion Events

    /// <summary>
    /// Checks if the current data in this column could be stored in the target column.
    /// </summary>
    /// <param name="targetColumn">A target <see cref="MySqlDataColumn"/> where the data on this column would be stored.</param>
    /// <returns><c>true</c> if the current data in this column could be stored in the target column, <c>false</c> otherwise.</returns>
    public bool CanDataBeStoredInGivenColumn(MySqlDataColumn targetColumn)
    {
      if (targetColumn == null)
      {
        return false;
      }

      // Test the data on each row of the parent table in this specific column to check if it could be stored in the target column
      bool dataFitsIntoType = true;
      MySqlDataTable parentTable = Table as MySqlDataTable;
      int rowIdx = 0;
      if (parentTable != null)
      {
        foreach (string strValueFromArray in parentTable.Rows.Cast<DataRow>()
            .Where(dr => !parentTable.FirstRowContainsColumnNames || rowIdx++ != 0)
            .Select(dr => dr[Ordinal].ToString())
            .Where(strValueFromArray => strValueFromArray.Length != 0))
        {
          dataFitsIntoType = targetColumn.CanStoreValue(strValueFromArray);

          // If found a value where the data type is not good for it break since there is no need testing more values.
          if (!dataFitsIntoType)
          {
            break;
          }
        }
      }

      // Update warning stating the column's data type is not suitable for all of its data (in the preview table)
      // either for the Append or Export Data operation.
      string warningKey = ParentTable != null && ParentTable.OperationType.IsForAppend()
        ? DATA_NOT_SUITABLE_APPEND_WARNING_KEY
        : DATA_NOT_SUITABLE_EXPORT_WARNING_KEY;
      if (_warnings.SetVisibility(warningKey, !dataFitsIntoType))
      {
        OnColumnWarningsChanged();
      }

      return dataFitsIntoType;
    }

    /// <summary>
    /// Checks whether a given string value can be converted and stored in this column.
    /// </summary>
    /// <param name="strValue">The value as a string representation to store in this column.</param>
    /// <returns><c>true</c> if the string value can be stored in this column, <c>false</c> otherwise.</returns>
    public bool CanStoreValue(string strValue)
    {
      // If the value is null, treat it as an empty string.
      if (strValue == null)
      {
        strValue = string.Empty;
      }

      var mySqlDataType = MySqlDataType.ToLowerInvariant();

      // Return immediately for big data types.
      if (mySqlDataType.Contains("text") || mySqlDataType == "blob" || mySqlDataType == "tinyblob" || mySqlDataType == "mediumblob" || mySqlDataType == "longblob" || mySqlDataType == "binary" || mySqlDataType == "varbinary")
      {
        return true;
      }

      // Return immediately for spatial data types since values for them can be created in a wide variety of ways
      // (using WKT, WKB or MySQL spatial functions that return spatial objects), so leave the validation to the MySQL Server.
      if (mySqlDataType.Contains("curve") || mySqlDataType.Contains("geometry") || mySqlDataType.Contains("line") || mySqlDataType.Contains("curve") || mySqlDataType.Contains("point") || mySqlDataType.Contains("polygon") || mySqlDataType.Contains("surface"))
      {
        return true;
      }

      // Check for boolean
      if (mySqlDataType.StartsWith("bool") || mySqlDataType == "bit" || mySqlDataType == "bit(1)")
      {
        strValue = strValue.ToLowerInvariant();
        return (strValue == "true" || strValue == "false" || strValue == "0" || strValue == "1" || strValue == "yes" || strValue == "no" || strValue == "ja" || strValue == "nein");
      }

      // Check for integer values
      if (mySqlDataType.StartsWith("int") || mySqlDataType.StartsWith("mediumint"))
      {
        int tryIntValue;
        return Int32.TryParse(strValue, out tryIntValue);
      }

      if (mySqlDataType.StartsWith("year"))
      {
        int tryYearValue;
        return Int32.TryParse(strValue, out tryYearValue) && (tryYearValue >= 0 && tryYearValue < 100) || (tryYearValue > 1900 && tryYearValue < 2156);
      }

      if (mySqlDataType.StartsWith("tinyint"))
      {
        byte tryByteValue;
        return Byte.TryParse(strValue, out tryByteValue);
      }

      if (mySqlDataType.StartsWith("smallint"))
      {
        short trySmallIntValue;
        return Int16.TryParse(strValue, out trySmallIntValue);
      }

      if (mySqlDataType.StartsWith("bigint"))
      {
        long tryBigIntValue;
        return Int64.TryParse(strValue, out tryBigIntValue);
      }

      if (mySqlDataType.StartsWith("bit"))
      {
        ulong tryBitValue;
        return UInt64.TryParse(strValue, out tryBitValue);
      }

      // Check for big numeric values
      if (mySqlDataType.StartsWith("float"))
      {
        float tryFloatValue;
        return Single.TryParse(strValue, out tryFloatValue);
      }

      if (mySqlDataType.StartsWith("double") || mySqlDataType.StartsWith("real"))
      {
        double tryDoubleValue;
        return Double.TryParse(strValue, out tryDoubleValue);
      }

      // Check for date and time values.
      if (mySqlDataType == "time")
      {
        TimeSpan tryTimeSpanValue;
        return TimeSpan.TryParse(strValue, out tryTimeSpanValue);
      }

      if (mySqlDataType == "date" || mySqlDataType == "datetime" || mySqlDataType == "timestamp")
      {
        if (strValue.IsMySqlZeroDateTimeValue())
        {
          return true;
        }

        DateTime tryDateTimeValue;
        return DateTime.TryParse(strValue, out tryDateTimeValue);
      }

      // Check of char or varchar.
      int lParensIndex = mySqlDataType.IndexOf("(", StringComparison.Ordinal);
      int rParensIndex = mySqlDataType.IndexOf(")", StringComparison.Ordinal);
      if (mySqlDataType.StartsWith("varchar") || mySqlDataType.StartsWith("char"))
      {
        int characterLen;
        if (lParensIndex >= 0)
        {
          string paramValue = mySqlDataType.Substring(lParensIndex + 1, mySqlDataType.Length - lParensIndex - 2);
          int.TryParse(paramValue, out characterLen);
        }
        else
        {
          characterLen = 1;
        }

        return strValue.Length <= characterLen;
      }

      // Check if enum or set.
      bool isEnum = mySqlDataType.StartsWith("enum");
      bool isSet = mySqlDataType.StartsWith("set");
      if (isSet || isEnum)
      {
        if (_setOrEnumElements == null)
        {
          return false;
        }

        strValue = strValue.ToLowerInvariant();
        var superSet = new HashSet<string>(_setOrEnumElements.Select(el => el.ToLowerInvariant().Trim(new[] { '\'' })));
        if (isEnum)
        {
          return superSet.Contains(strValue);
        }

        string[] valueSet = strValue.Split(new[] { ',' });
        return superSet.IsSupersetOf(valueSet);
      }

      // Check for decimal values which is the more complex.
      bool mayContainFloatingPoint = mySqlDataType.StartsWith("decimal") || mySqlDataType.StartsWith("numeric") || mySqlDataType.StartsWith("double") || mySqlDataType.StartsWith("float") || mySqlDataType.StartsWith("real");
      int commaPos = mySqlDataType.IndexOf(",", StringComparison.Ordinal);
      int[] decimalLen = { -1, -1 };
      if (mayContainFloatingPoint && lParensIndex >= 0 && rParensIndex >= 0 && lParensIndex < rParensIndex)
      {
        decimalLen[0] = Int32.Parse(mySqlDataType.Substring(lParensIndex + 1, (commaPos >= 0 ? commaPos : rParensIndex) - lParensIndex - 1));
        if (commaPos >= 0)
        {
          decimalLen[1] = Int32.Parse(mySqlDataType.Substring(commaPos + 1, rParensIndex - commaPos - 1));
        }
      }

      int floatingPointPos = strValue.IndexOf(".", StringComparison.Ordinal);
      bool floatingPointCompliant = true;
      if (floatingPointPos >= 0)
      {
        bool lengthCompliant = strValue.Substring(0, floatingPointPos).Length <= decimalLen[0];
        bool decimalPlacesCompliant = decimalLen[1] < 0 || strValue.Substring(floatingPointPos + 1, strValue.Length - floatingPointPos - 1).Length <= decimalLen[1];
        floatingPointCompliant = lengthCompliant && decimalPlacesCompliant;
      }

      if (!mySqlDataType.StartsWith("decimal") && !mySqlDataType.StartsWith("numeric"))
      {
        return false;
      }

      decimal tryDecimalValue;
      return Decimal.TryParse(strValue, out tryDecimalValue) && floatingPointCompliant;
    }

    /// <summary>
    /// Checks whether another column in the <see cref="ParentTable"/> has the same <see cref="DisplayName"/> as this column.
    /// </summary>
    public void CheckForDuplicatedDisplayName()
    {
      if (!(Table is MySqlDataTable))
      {
        return;
      }

      IsDisplayNameDuplicate = ParentTable.GetColumnIndex(DisplayName, true, false, true, Ordinal) > -1;
      if (AutoPk)
      {
        // Update warning on the parent table regarding the AutoPK column name being a duplicate of another existing column's name
        ParentTable.UpdateAutoPkWarning(IsDisplayNameDuplicate);
      }
      else
      {
        // Update warning stating the column name is a duplicate of another existing column's name
        if (_warnings.SetVisibility(DUPLICATE_NAME_WARNING_KEY, IsDisplayNameDuplicate))
        {
          OnColumnWarningsChanged();
        }
      }
    }

    /// <summary>
    /// Runs a check on the column's data to determine if it is unique (i.e. there are no data duplicates).
    /// </summary>
    /// <remarks>If the <see cref="AllowNull"/> value is <c>true</c> then nulls are considered as allowed duplicate values in MySQL, so uniqueness is checked only for non null values.</remarks>
    /// <returns><c>true</c> if the data in the column is unique, <c>false</c> otherwise.</returns>
    public bool CheckForDataUniqueness()
    {
      var duplicates = GetDuplicateValuesInColumn();
      if (duplicates == null)
      {
        DuplicateGroupsFound = 0;
        _warningsMoreInfosDictionary.Remove(DATA_NOT_UNIQUE_WARNING_KEY);
      }
      else
      {
        DuplicateGroupsFound = duplicates.Count;
        var moreInfoTextBuilder = new StringBuilder(DuplicateGroupsFound * byte.MaxValue);
        foreach (var dictPair in duplicates)
        {
          moreInfoTextBuilder.AppendLine(string.Format("{0} ({1})", dictPair.Key, dictPair.Value));
        }

        var moreInfoTuple = new Tuple<string, string>(Resources.ColumnDataNotUniqueMoreInfoTitle, moreInfoTextBuilder.ToString());
        if (_warningsMoreInfosDictionary.ContainsKey(DATA_NOT_UNIQUE_WARNING_KEY))
        {
          _warningsMoreInfosDictionary[DATA_NOT_UNIQUE_WARNING_KEY] = moreInfoTuple;
        }
        else
        {
          _warningsMoreInfosDictionary.Add(DATA_NOT_UNIQUE_WARNING_KEY, moreInfoTuple);
        }
      }

      return DuplicateGroupsFound == 0;
    }

    /// <summary>
    /// Clears all warnings from this column.
    /// </summary>
    public void ClearWarnings()
    {
      _warnings.Clear();
    }

    /// <summary>
    /// Creates a new <see cref="MySqlDataColumn"/> object with a schema identical to this column's schema.
    /// </summary>
    /// <returns>A new <see cref="MySqlDataColumn"/> object with a schema cloned from this column.</returns>
    public MySqlDataColumn CloneSchema()
    {
      MySqlDataColumn clonedColumn = new MySqlDataColumn { ColumnName = ColumnName };
      clonedColumn.SyncSchema(this);
      return clonedColumn;
    }

    /// <summary>
    /// Analyzes the data stored in this column and automatically detects the MySQL data type for it.
    /// </summary>
    /// <param name="dataRange">Excel data range containing the data to fill the table.</param>
    /// <param name="cropRange">Attempts to crop the data range to a subrange containing only formulas or constants.</param>
    /// <remarks>
    /// The data type detection cannot be done with the data already stored in the column's table, the columns which need data type detection
    /// are created by default with a string data type, when Excel data is inserted to it it is already converted to string so the data type
    /// detection must be done using the original Excel data.
    /// </remarks>
    public void DetectMySqlDataType(Excel.Range dataRange, bool cropRange)
    {
      if (AutoPk || ParentTable == null || ParentTable.Rows.Count == 0 || dataRange == null)
      {
        return;
      }

      Excel.Range columnRange = dataRange.Columns[RangeColumnIndex];
      if (cropRange)
      {
        columnRange = columnRange.GetNonEmptyRange();
      }

      if (columnRange == null)
      {
        return;
      }

      var useFormattedValues = ParentTable.IsFormatted;
      var typesListForFirstAndRest = new List<string>(2);
      var typesListFromSecondRow = new List<string>(Table.Rows.Count);
      int[] varCharLengthsFirstRow = { 0, 0 };  // 0 - All rows original datatype varcharmaxlen, 1 - All rows Varchar forced datatype maxlen
      int[] varCharMaxLen = { 0, 0 };           // 0 - All rows original datatype varcharmaxlen, 1 - All rows Varchar forced datatype maxlen
      int[] decimalMaxLenFirstRow = { 0, 0 };   // 0 - Integral part max length, 1 - decimal part max length
      int[] decimalMaxLen = { 0, 0 };           // 0 - Integral part max length, 1 - decimal part max length
      bool addBufferToVarChar = ParentTable.AddBufferToVarChar;
      for (int rowPos = 1; rowPos <= columnRange.Rows.Count; rowPos++)
      {
        Excel.Range excelCell = columnRange.Cells[rowPos, 1];
        object rawValue = excelCell != null ? excelCell.GetCellPackedValue(useFormattedValues) : null;
        if (rawValue == null)
        {
          continue;
        }

        // Treat always as a Varchar value first in case all rows do not have a consistent datatype just to see the varchar len calculated by GetMySQLExportDataType
        string valueAsString = rawValue.ToString();
        bool valueOverflow;
        string proposedType = DataTypeUtilities.GetMySqlExportDataType(valueAsString, out valueOverflow);
        if (proposedType == "Bool")
        {
          proposedType = "VarChar(5)";
        }
        else if (proposedType.StartsWith("Date"))
        {
          proposedType = string.Format("VarChar({0})", valueAsString.Length);
        }

        int varCharValueLength;
        int leftParensIndex;
        if (proposedType != "Text")
        {
          leftParensIndex = proposedType.IndexOf("(", StringComparison.Ordinal);
          varCharValueLength = addBufferToVarChar
            ? int.Parse(proposedType.Substring(leftParensIndex + 1, proposedType.Length - leftParensIndex - 2))
            : valueAsString.Length;
          varCharMaxLen[1] = Math.Max(varCharValueLength, varCharMaxLen[1]);
        }

        // Normal datatype detection
        proposedType = DataTypeUtilities.GetMySqlExportDataType(rawValue, out valueOverflow);
        leftParensIndex = proposedType.IndexOf("(", StringComparison.Ordinal);
        string strippedType = leftParensIndex < 0 ? proposedType : proposedType.Substring(0, leftParensIndex);
        switch (strippedType)
        {
          case "VarChar":
            varCharValueLength = addBufferToVarChar
              ? int.Parse(proposedType.Substring(leftParensIndex + 1, proposedType.Length - leftParensIndex - 2))
              : valueAsString.Length;
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
          typesListForFirstAndRest.Add(strippedType);
          varCharLengthsFirstRow[0] = varCharMaxLen[0];
          varCharMaxLen[0] = 0;
          varCharLengthsFirstRow[1] = varCharMaxLen[1];
          varCharMaxLen[1] = 0;
          decimalMaxLenFirstRow[0] = decimalMaxLen[0];
          decimalMaxLen[0] = 0;
          decimalMaxLenFirstRow[1] = decimalMaxLen[1];
          decimalMaxLen[1] = 0;
        }
        else
        {
          typesListFromSecondRow.Add(strippedType);
        }
      }

      if (typesListFromSecondRow.Count + typesListForFirstAndRest.Count == 0)
      {
        // There is no data on the column, so set the data types to the default for empty columns.
        RowsFromFirstDataType = DEFAULT_FULL_DATA_TYPE_FOR_EMPTY_COLUMNS;
        RowsFromSecondDataType = DEFAULT_FULL_DATA_TYPE_FOR_EMPTY_COLUMNS;
      }
      else
      {
        // Get the consistent DataType for all rows except first one.
        RowsFromSecondDataType = DataTypeUtilities.GetConsistentDataTypeOnAllRows(typesListFromSecondRow, decimalMaxLen, varCharMaxLen);
        if (typesListFromSecondRow.Count > 0)
        {
          string parameters;
          typesListForFirstAndRest.Add(DataTypeUtilities.GetStrippedMySqlDataType(_rowsFromSecondDataType, out parameters));
        }

        // Get the consistent DataType between first row and the previously computed consistent DataType for the rest of the rows.
        varCharMaxLen[0] = Math.Max(varCharMaxLen[0], varCharLengthsFirstRow[0]);
        varCharMaxLen[1] = Math.Max(varCharMaxLen[1], varCharLengthsFirstRow[1]);
        decimalMaxLen[0] = Math.Max(decimalMaxLen[0], decimalMaxLenFirstRow[0]);
        decimalMaxLen[1] = Math.Max(decimalMaxLen[1], decimalMaxLenFirstRow[1]);
        RowsFromFirstDataType = DataTypeUtilities.GetConsistentDataTypeOnAllRows(typesListForFirstAndRest, decimalMaxLen, varCharMaxLen);
      }

      // Set the DataType in the column depending on the setting to treat the first row of data as the names of the columns
      SetMySqlDataType(ParentTable.FirstRowContainsColumnNames ? RowsFromSecondDataType : RowsFromFirstDataType);
    }

    /// <summary>
    /// Fills a given <see cref="DataRow"/> with schema information about this column.
    /// </summary>
    /// <param name="schemaInfoRow">A <see cref="DataRow"/> formatted to hold Field, Type, Null, Key, Default and Extra information.</param>
    public void FillSchemaInfoRow(ref DataRow schemaInfoRow)
    {
      if (schemaInfoRow == null)
      {
        return;
      }

      var extraBuilder = new StringBuilder();
      schemaInfoRow["Name"] = DisplayName;
      schemaInfoRow["Type"] = MySqlDataType;
      schemaInfoRow["Null"] = AllowNull ? "YES" : "NO";
      if (PrimaryKey)
      {
        schemaInfoRow["Key"] = "PRI";
      }
      else if (UniqueKey)
      {
        schemaInfoRow["Key"] = "UNI";
      }
      else if (CreateIndex)
      {
        schemaInfoRow["Key"] = "MUL";
      }

      schemaInfoRow["Default"] = DefaultValue != null ? DefaultValue.ToString() : string.Empty;
      schemaInfoRow["CharSet"] = CharSet;
      schemaInfoRow["Collation"] = Collation;
      if (AutoIncrement)
      {
        extraBuilder.Append("auto_increment");
      }

      if (AutoPk)
      {
        if (extraBuilder.Length > 0)
        {
          extraBuilder.Append(" ");
        }

        extraBuilder.Append("auto_pk");
      }

      if (ExcludeColumn)
      {
        if (extraBuilder.Length > 0)
        {
          extraBuilder.Append(" ");
        }

        extraBuilder.Append("exclude");
      }

      schemaInfoRow["Extra"] = extraBuilder.ToString();
      extraBuilder.Clear();
    }

    /// <summary>
    /// Gets a dictionary of duplicate values within the column's rows and the count of each duplicate value.
    /// </summary>
    /// <remarks>If the <see cref="AllowNull"/> value is <c>true</c> then nulls are considered as allowed duplicate values in MySQL, so uniqueness is checked only for non null values.</remarks>
    /// <returns>A dictionary of duplicate values within the column's rows and the count of each duplicate value.</returns>
    public Dictionary<object, int> GetDuplicateValuesInColumn()
    {
      if (Table == null || Table.Rows.Count == 0)
      {
        return null;
      }

      var duplicates = new Dictionary<object, int>(Table.Rows.Count);
      foreach (
        var group in
          Table.Rows.Cast<DataRow>()
            .Select(row => row[Ordinal])
            .GroupBy(value => value)
            .Where(group => group.Count() > 1)
            .Where(group => !_allowNull || group.Key != DBNull.Value))
      {
        duplicates.Add(group.Key, group.Count());
      }

      return duplicates;
    }

    /// <summary>
    /// Creates a SQL query fragment meant to be used within a CREATE TABLE statement to create a column with this column's schema.
    /// </summary>
    /// <returns>A SQL query fragment describing this column's schema.</returns>
    public string GetSql()
    {
      if (string.IsNullOrEmpty(DisplayName))
      {
        return null;
      }

      var colDefinitionBuilder = new StringBuilder();
      colDefinitionBuilder.AppendFormat("`{0}` {1}", DisplayName.Replace("`", "``"), MySqlDataType.ToLowerInvariant());
      colDefinitionBuilder.AppendFormat(" {0}null", AllowNull ? string.Empty : "not ");
      if (AutoIncrement)
      {
        colDefinitionBuilder.Append(" auto_increment");
      }

      return colDefinitionBuilder.ToString();
    }

    /// <summary>
    /// Sets the <see cref="DisplayName"/> property to the given display name.
    /// </summary>
    /// <param name="displayName">Display name.</param>
    /// <param name="checkForDuplicates">Flag indicating whether a check for duplicate display names should be done on this and other columns.</param>
    /// <param name="addSuffixIfDuplicate">Flag indicating if a suffix is added to the display name if an existing column with the same name is found.</param>
    public void SetDisplayName(string displayName, bool checkForDuplicates, bool addSuffixIfDuplicate = false)
    {
      if (DisplayName == displayName)
      {
        return;
      }

      // Update warning stating the column name cannot be empty
      if (_warnings.SetVisibility(EMPTY_NAME_WARNING_KEY, displayName.Length == 0))
      {
        OnColumnWarningsChanged();
      }

      DisplayName = addSuffixIfDuplicate && Table is MySqlDataTable
          ? ParentTable.GetNonDuplicateColumnName(displayName, Ordinal)
          : displayName;

      // If addSuffixIfDuplicate = true, we already made sure above to assign to this column a DisplayName that will not be a duplicate,
      // so we need to fire a duplicates check on all columns to update those warnings if needed.
      if ((addSuffixIfDuplicate || checkForDuplicates) && Table is MySqlDataTable)
      {
        ParentTable.CheckForDuplicatedColumnDisplayNames();
      }

      OnPropertyChanged("DisplayName");
    }

    /// <summary>
    /// Sets the data type of the column to an enum or set type with a valid list of values built from the column values in all rows.
    /// </summary>
    /// <param name="type">A <see cref="CollectionDataType"/>.</param>
    public void SetCollectionDataType(CollectionDataType type)
    {
      if (AutoPk)
      {
        return;
      }

      var collectionElements = new List<string>(ParentTable.Rows.Count);
      switch (type)
      {
        case CollectionDataType.Enum:
          // For the ENUM we need to consider each value as a single element, then remove the duplicates
          collectionElements.AddRange(
            ParentTable.Rows.Cast<MySqlDataRow>()
              .Select(row => string.Format("'{0}'", row[Ordinal].ToString().Replace("'", "''"))));
          break;

        case CollectionDataType.Set:
          // For the SET we need to break up each value in sub-tokens using the comma as a delimiter, then remove the duplicates.
          collectionElements.AddRange(
            ParentTable.Rows.Cast<MySqlDataRow>()
              .SelectMany(row => row[Ordinal].ToString().Replace("'", "''").Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries).Select(element => string.Format("'{0}'", element))));
          break;
      }

      // Remove duplicates and sort the list for easier reading
      var firstRowElement = collectionElements.FirstOrDefault();
      collectionElements = collectionElements.Skip(1).Distinct().ToList();
      collectionElements.Sort();

      // Join the resulting list of elements into a list delimited by commas.
      var values = string.Join(",", collectionElements.ToArray());
      RowsFromSecondDataType = string.Format("{0}({1})", type, values);
      if (!collectionElements.Contains(firstRowElement))
      {
        values = firstRowElement + "," + values;
      }

      RowsFromFirstDataType = string.Format("{0}({1})", type, values);
      MySqlDataType = ParentTable.FirstRowContainsColumnNames ? RowsFromSecondDataType : RowsFromFirstDataType;
    }

    /// <summary>
    /// Checks if a user typed MySQL data type is valid and assigns it to the <see cref="MySqlDataType"/> property.
    /// </summary>
    /// <param name="dataType">A MySQL data type as specified for new columns in a CREATE TABLE statement.</param>
    /// <param name="validateType">Flag indicating if the data type will be checked if it's a valid MySQL data type.</param>
    /// <param name="testTypeOnData">Flag indicating if the data type will be tested against the column's data to see if the type is suitable for the data.</param>
    /// <returns><c>true</c> if the type is a valid MySQL data type, <c>false</c> otherwise.</returns>
    public bool SetMySqlDataType(string dataType, bool validateType = false, bool testTypeOnData = false)
    {
      bool warningsChanged = false;
      IsMySqlDataTypeValid = true;
      if (AutoPk)
      {
        MySqlDataType = "Integer";
        RowsFromFirstDataType = MySqlDataType;
        RowsFromSecondDataType = MySqlDataType;
      }
      else
      {
        dataType = dataType.Trim();
        MySqlDataType = dataType;

        if (MySqlDataType.Length == 0)
        {
          // Show warning stating the column data type cannot be empty
          if (_warnings.Show(NO_DATA_TYPE_WARNING_KEY))
          {
            OnColumnWarningsChanged();
          }

          return IsMySqlDataTypeValid;
        }

        // Hide warning stating the column data type cannot be empty
        warningsChanged = _warnings.Hide(NO_DATA_TYPE_WARNING_KEY);
        bool showInvalidSetOrEnumWarning = false;
        Tuple<string, string> moreInfoTuple = null;
        if (validateType)
        {
          IsMySqlDataTypeValid = ValidateUserDataType();
          showInvalidSetOrEnumWarning = _invalidSetOrEnumElementsIndexes != null && _invalidSetOrEnumElementsIndexes.Count > 0;
          if (showInvalidSetOrEnumWarning)
          {
            var invalidElementsBuilder = new StringBuilder();
            foreach (int index in _invalidSetOrEnumElementsIndexes)
            {
              invalidElementsBuilder.AppendLine(_setOrEnumElements[index]);
            }

            moreInfoTuple = new Tuple<string, string>(Resources.ColumnDataSetOrEnumNotValidMoreInfoTitle, invalidElementsBuilder.ToString());
            if (_warningsMoreInfosDictionary.ContainsKey(INVALID_SET_ENUM_WARNING_KEY))
            {
              _warningsMoreInfosDictionary[INVALID_SET_ENUM_WARNING_KEY] = moreInfoTuple;
            }
            else
            {
              _warningsMoreInfosDictionary.Add(INVALID_SET_ENUM_WARNING_KEY, moreInfoTuple);
            }
          }
        }

        // Update warning stating the column's data type is not a valid MySQL data type
        warningsChanged = _warnings.SetVisibility(INVALID_DATA_TYPE_WARNING_KEY, !IsMySqlDataTypeValid) || warningsChanged;

        // Update warning stating a SET or ENUM declaration is invalid because of an error in a specific element
        warningsChanged = _warnings.SetVisibility(INVALID_SET_ENUM_WARNING_KEY, showInvalidSetOrEnumWarning) || warningsChanged;
        if (moreInfoTuple == null)
        {
          _warningsMoreInfosDictionary.Remove(INVALID_SET_ENUM_WARNING_KEY);
        }

        if (IsMySqlDataTypeValid && testTypeOnData)
        {
          CanDataBeStoredInGivenColumn(this);
        }
      }

      if (ParentTable != null)
      {
        CreateIndex = ParentTable.AutoIndexIntColumns && IsInteger;
        if (ParentTable.AutoAllowEmptyNonIndexColumns)
        {
          AllowNull = !PrimaryKey && !CreateIndex;
        }

        if (!ParentTable.DetectDatatype)
        {
          RowsFromFirstDataType = MySqlDataType;
          RowsFromSecondDataType = MySqlDataType;
        }
      }

      if (warningsChanged)
      {
        OnColumnWarningsChanged();
      }

      return IsMySqlDataTypeValid;
    }

    /// <summary>
    /// Subsribes this column to the event fired when a property value in the parent table changes.
    /// </summary>
    public void SubscribeToParentTablePropertyChange()
    {
      if (ParentTable == null)
      {
        return;
      }

      ParentTable.PropertyChanged -= ParentTable_PropertyChanged;
      ParentTable.PropertyChanged += ParentTable_PropertyChanged;
    }

    /// <summary>
    /// Synchronizes this object properties copying the corresponding property values from another <see cref="MySqlDataColumn"/> object.
    /// </summary>
    /// <param name="fromColumn">The <see cref="MySqlDataColumn"/> object from which to copy property values.</param>
    public void SyncSchema(MySqlDataColumn fromColumn)
    {
      // Set first some properties that need to be set before all others because of dependencies among them.
      SetDisplayName(fromColumn.DisplayName, false);
      DataType = fromColumn.DataType;
      SetMySqlDataType(fromColumn.MySqlDataType);
      RowsFromFirstDataType = fromColumn.RowsFromFirstDataType;
      RowsFromSecondDataType = fromColumn.RowsFromSecondDataType;
      AutoPk = fromColumn.AutoPk;
      InExportMode = fromColumn.InExportMode;

      // Set the rest of the properties.
      AllowNull = fromColumn.AllowNull;
      AutoIncrement = fromColumn.AutoIncrement;
      CharSet = fromColumn.CharSet;
      Collation = fromColumn.Collation;
      CreateIndex = fromColumn.CreateIndex;
      ExcludeColumn = fromColumn.ExcludeColumn;
      MappedDataColName = fromColumn.MappedDataColName;
      MappedDataColOrdinal = fromColumn.MappedDataColOrdinal;
      PrimaryKey = fromColumn.PrimaryKey;
      RangeColumnIndex = fromColumn.RangeColumnIndex;
      UniqueKey = fromColumn.UniqueKey;
      Unsigned = fromColumn.Unsigned;
    }

    /// <summary>
    /// Raises the <see cref="ColumnWarningsChanged"/> event.
    /// </summary>
    protected virtual void OnColumnWarningsChanged()
    {
      if (ColumnWarningsChanged != null)
      {
        ColumnWarningsChanged(this, new ColumnWarningsChangedArgs(this));
      }
    }

    /// <summary>
    /// Raises the <see cref="PropertyChanged"/> event.
    /// </summary>
    /// <param name="args">Event arguments.</param>
    protected void OnPropertyChanged(PropertyChangedEventArgs args)
    {
      if (PropertyChanged != null)
      {
        PropertyChanged(this, args);
      }
    }

    /// <summary>
    /// Raises the <see cref="PropertyChanged"/> event.
    /// </summary>
    /// <param name="propertyName">Name of the property whose value changed.</param>
    protected void OnPropertyChanged(string propertyName)
    {
      OnPropertyChanged(new PropertyChangedEventArgs(propertyName));
    }

    /// <summary>
    /// Event delegate method fired when a property value in the <see cref="ParentTable"/> changes.
    /// </summary>
    /// <param name="sender">Sender object.</param>
    /// <param name="e">Event arguments.</param>
    private void ParentTable_PropertyChanged(object sender, PropertyChangedEventArgs e)
    {
      switch (e.PropertyName)
      {
        case "CharSet":
          if (string.IsNullOrEmpty(CharSet))
          {
            CharSet = ParentTable.CharSet;
          }
          break;

        case "Collation":
          if (string.IsNullOrEmpty(Collation))
          {
            Collation = ParentTable.Collation;
          }
          break;
      }
    }

    /// <summary>
    /// Initializes the warnings container for this column.
    /// </summary>
    private void SetupWarnings()
    {
      _warnings = new WarningsContainer(WarningsContainer.CurrentWarningChangedMethodType.OnShowIfWarningNotPresent, 8);
      _warnings.Add(DATA_NOT_UNIQUE_WARNING_KEY, Resources.ColumnDataNotUniqueWarning);
      _warnings.Add(EMPTY_NAME_WARNING_KEY, Resources.ColumnNameRequiredWarning);
      _warnings.Add(DUPLICATE_NAME_WARNING_KEY, Resources.ColumnExistsWarning);
      _warnings.Add(NO_DATA_TYPE_WARNING_KEY, Resources.ColumnDataTypeRequiredWarning);
      _warnings.Add(INVALID_DATA_TYPE_WARNING_KEY, Resources.ColumnDataTypeNotValidWarning);
      _warnings.Add(INVALID_SET_ENUM_WARNING_KEY, Resources.ColumnDataSetOrEnumNotValidWarning);
      _warnings.Add(DATA_NOT_SUITABLE_APPEND_WARNING_KEY, Resources.AppendDataNotSuitableForColumnTypeWarning);
      _warnings.Add(DATA_NOT_SUITABLE_EXPORT_WARNING_KEY, Resources.ExportDataTypeNotSuitableWarning);
      _warningsMoreInfosDictionary = new Dictionary<string, Tuple<string, string>>(_warnings.DefinedQuantity);
    }

    /// <summary>
    /// Updates warning stating the column's data is not unique.
    /// </summary>
    private void UpdateDataUniquenessWarnings()
    {
      // Storing the value in a variable since it is easier to debug.
      var currentDuplicateGroupsCount = DuplicateGroupsFound;
      bool dataIsUnique = !_uniqueKey || CheckForDataUniqueness();
      bool duplicateGroupsCountChanged = DuplicateGroupsFound > 0 &&
                                         DuplicateGroupsFound != currentDuplicateGroupsCount;
      if (_warnings.SetVisibility(DATA_NOT_UNIQUE_WARNING_KEY, !dataIsUnique) || duplicateGroupsCountChanged)
      {
        OnColumnWarningsChanged();
      }
    }

    /// <summary>
    /// Validates that a user typed data type is a valid MySQL data type.
    /// A blank data type is considered valid.
    /// </summary>
    /// <returns><c>true</c> if the type is a valid MySQL data type, <c>false</c> otherwise.</returns>
    private bool ValidateUserDataType()
    {
      // If the proposed type is blank return true since a blank data type is considered valid.
      string proposedUserType = MySqlDataType;
      if (MySqlDataType.Length == 0)
      {
        return true;
      }

      int rightParenthesisIndex = proposedUserType.IndexOf(")", StringComparison.Ordinal);
      int leftParenthesisIndex = proposedUserType.IndexOf("(", StringComparison.Ordinal);

      // Check if we have parenthesis within the proposed data type and if the left and right parentheses are placed properly.
      // Also check if there is no text beyond the right parenthesis.
      if (rightParenthesisIndex >= 0 && (leftParenthesisIndex < 0 || leftParenthesisIndex >= rightParenthesisIndex || proposedUserType.Length > rightParenthesisIndex + 1))
      {
        return false;
      }

      // Check if the data type stripped of parenthesis is found in the list of valid MySQL types.
      var pureDataType = rightParenthesisIndex >= 0 ? proposedUserType.Substring(0, leftParenthesisIndex).ToLowerInvariant() : proposedUserType.ToLowerInvariant();
      var mySqlDataType = Classes.MySqlDataType.DataTypesList.FirstOrDefault(mType => mType.IsBaseType && string.Equals(mType.Name, pureDataType, StringComparison.InvariantCultureIgnoreCase));
      if (mySqlDataType == null)
      {
        return false;
      }

      // Parameters checks.
      bool enumOrSet = pureDataType == "enum" || pureDataType == "set";
      if ((mySqlDataType.ParametersCount == 0 || rightParenthesisIndex < 0) && !enumOrSet)
      {
        return true;
      }

      // If an enum or set the data type must contain parenthesis along with its list of valid values.
      if (enumOrSet && rightParenthesisIndex < 0)
      {
        return false;
      }

      // Check if the number of parameters is valid for the proposed MySQL data type
      string parametersText = proposedUserType.Substring(leftParenthesisIndex + 1, rightParenthesisIndex - leftParenthesisIndex - 1).Trim();
      var parameterValues = parametersText.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries).ToList();
      int parametersCount = parameterValues.Count;

      // If there are no parameters but parenthesis were provided the data type is invalid (parenthesis were already checked above).
      if (parametersCount == 0)
      {
        return false;
      }

      // If the quantity of parameters does not match the data type valid accepted parameters quantity the data type is invalid.
      bool parametersQtyIsValid = enumOrSet ? parametersCount > 0 : mySqlDataType.ParametersCount == parametersCount;
      if (!parametersQtyIsValid)
      {
        return false;
      }

      // If an enum or set, check that the values specified within the declaration are correctly wrapped in single quotes, otherwise the declaration is wrong.
      if (enumOrSet)
      {
        _setOrEnumElements = parameterValues;
        _invalidSetOrEnumElementsIndexes = _setOrEnumElements.CheckForCorrectSingleQuoting();
        return _invalidSetOrEnumElementsIndexes == null || _invalidSetOrEnumElementsIndexes.Count == 0;
      }

      // Check if the paremeter values are valid integers for data types with 1 or 2 parameters (varchar and numeric types).
      foreach (string paramValue in parameterValues)
      {
        int convertedValue;
        if (!int.TryParse(paramValue, out convertedValue))
        {
          return false;
        }

        // Specific check for year data type.
        if (pureDataType == "year" && convertedValue != 2 && convertedValue != 4)
        {
          return false;
        }
      }

      return true;
    }
  }
}