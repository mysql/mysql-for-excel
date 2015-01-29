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
    /// The element of an ENUM or SET declaration that makes it invalid.
    /// </summary>
    private string _invalidEnumOrSetElement;

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
    /// Flag indicating if the column has a related unique index.
    /// </summary>
    private bool _uniqueKey;

    /// <summary>
    /// Container with warnings for users about the column properties that could cause errors when creating this column in a database table.
    /// </summary>
    private WarningsContainer _warnings;

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
      _invalidEnumOrSetElement = null;
      _mappedDataColOrdinal = -1;
      _mySqlDataType = string.Empty;
      AutoIncrement = false;
      AutoPk = false;
      CharSet = null;
      Collation = null;
      DisplayName = string.Empty;
      InExportMode = false;
      IsDisplayNameDuplicate = false;
      IsMySqlDataTypeValid = true;
      MappedDataColName = null;
      PrimaryKey = false;
      RangeColumnIndex = 0;
      RowsFromFirstDataType = string.Empty;
      RowsFromSecondDataType = string.Empty;
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
        return !ExcludeColumn && !string.IsNullOrEmpty(currentWarningText)
          ? currentWarningText + (_warnings.CurrentWarningKey.Equals(INVALID_SET_ENUM_WARNING_KEY) && !string.IsNullOrEmpty(_invalidEnumOrSetElement) ? _invalidEnumOrSetElement : string.Empty)
          : string.Empty;
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
        if (Table is MySqlDataTable)
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
    public string RowsFromFirstDataType { get; set; }

    /// <summary>
    /// Gets the consistent data type that can hold the data for all rows starting from the second row.
    /// </summary>
    public string RowsFromSecondDataType { get; set; }

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

        bool columnValuesAreUnique = true;
        try
        {
          Unique = _uniqueKey;
        }
        catch (InvalidConstraintException)
        {
          columnValuesAreUnique = false;
        }

        // Update warning stating the column's data is not unique
        if (_warnings.SetVisibility(DATA_NOT_UNIQUE_WARNING_KEY, !columnValuesAreUnique))
        {
          OnColumnWarningsChanged();
        }

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
    /// Checks if the data stored in this column would fit within the given data type.
    /// </summary>
    /// <param name="mySqlDataType">Data type for a table column supported by MySQL Server.</param>
    /// <returns><c>true</c> if the data stored in this column would fit, <c>false</c> otherwise.</returns>
    public bool CanBeOfMySqlDataType(string mySqlDataType)
    {
      bool result = true;

      MySqlDataTable parentTable = Table as MySqlDataTable;
      int rowIdx = 0;
      if (parentTable == null)
      {
        return true;
      }

      foreach (
        string strValueFromArray in
          parentTable.Rows.Cast<DataRow>()
            .Where(dr => !parentTable.FirstRowContainsColumnNames || rowIdx++ != 0)
            .Select(dr => dr[Ordinal].ToString())
            .Where(strValueFromArray => strValueFromArray.Length != 0))
      {
        result = DataTypeUtilities.StringValueCanBeStoredWithMySqlType(strValueFromArray, mySqlDataType);

        // If found a value where the data type is not good for it break since there is no need testing more values.
        if (!result)
        {
          break;
        }
      }

      return result;
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
      string proposedType;
      string strippedType = string.Empty;
      int leftParensIndex;
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
        proposedType = DataTypeUtilities.GetMySqlExportDataType(valueAsString, out valueOverflow);
        if (proposedType == "Bool")
        {
          proposedType = "VarChar(5)";
        }
        else if (proposedType.StartsWith("Date"))
        {
          proposedType = string.Format("VarChar({0})", valueAsString.Length);
        }

        int varCharValueLength;
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
        strippedType = leftParensIndex < 0 ? proposedType : proposedType.Substring(0, leftParensIndex);
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

      // Get the consistent DataType for all rows except first one.
      proposedType = DataTypeUtilities.GetConsistentDataTypeOnAllRows(strippedType, typesListFromSecondRow, decimalMaxLen, varCharMaxLen);

      if (string.IsNullOrEmpty(proposedType) && ParentTable.OperationType.IsForExport())
      {
        proposedType = "VarChar(255)";
        strippedType = "VarChar";
        typesListForFirstAndRest.Add("VarChar");
        varCharMaxLen[0] = 255;
        varCharMaxLen[1] = 255;
      }

      RowsFromSecondDataType = proposedType;

      // Get the consistent DataType between first columnInfoRow and the previously computed consistent DataType for the rest of the rows.
      if (typesListFromSecondRow.Count > 0)
      {
        bool emptyProposedType = string.IsNullOrEmpty(proposedType);
        leftParensIndex = emptyProposedType ? -1 : proposedType.IndexOf("(", StringComparison.Ordinal);
        strippedType = leftParensIndex < 0 || emptyProposedType
          ? proposedType
          : proposedType.Substring(0, leftParensIndex);
        typesListForFirstAndRest.Add(strippedType);
      }

      varCharMaxLen[0] = Math.Max(varCharMaxLen[0], varCharLengthsFirstRow[0]);
      varCharMaxLen[1] = Math.Max(varCharMaxLen[1], varCharLengthsFirstRow[1]);
      decimalMaxLen[0] = Math.Max(decimalMaxLen[0], decimalMaxLenFirstRow[0]);
      decimalMaxLen[1] = Math.Max(decimalMaxLen[1], decimalMaxLenFirstRow[1]);
      proposedType = DataTypeUtilities.GetConsistentDataTypeOnAllRows(strippedType, typesListForFirstAndRest, decimalMaxLen, varCharMaxLen);
      RowsFromFirstDataType = proposedType;
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
        _invalidEnumOrSetElement = null;
        if (validateType)
        {
          IsMySqlDataTypeValid = DataTypeUtilities.ValidateUserDataType(dataType, out _invalidEnumOrSetElement);
        }

        // Update warning stating the column's data type is not a valid MySQL data type
        warningsChanged = _warnings.SetVisibility(INVALID_DATA_TYPE_WARNING_KEY, !IsMySqlDataTypeValid) || warningsChanged;

        // Update warning stating a SET or ENUM declaration is invalid because of an error in a specific element
        warningsChanged = _warnings.SetVisibility(INVALID_SET_ENUM_WARNING_KEY, !string.IsNullOrEmpty(_invalidEnumOrSetElement)) || warningsChanged;

        if (IsMySqlDataTypeValid && testTypeOnData)
        {
          TestColumnDataTypeAgainstColumnData(MySqlDataType);
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
    /// Checks if this column's data type is right for the data currently stored in the column.
    /// </summary>
    /// <param name="mySqlDataType">The MySQL data type to test the column's data with.</param>
    /// <returns><c>true</c> if the column's data fits the data type, <c>false</c> otherwise.</returns>
    public bool TestColumnDataTypeAgainstColumnData(string mySqlDataType)
    {
      // Update warning stating the column's data type is not suitable for all of its data (in the preview table)
      // either for the Append or Export Data operation.
      bool dataFitsIntoType = mySqlDataType.Length > 0 && CanBeOfMySqlDataType(mySqlDataType);
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
    }
  }
}