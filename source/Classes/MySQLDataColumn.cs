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
using MySQL.ForExcel.Properties;

namespace MySQL.ForExcel.Classes
{
  /// <summary>
  /// Represents the schema of a MySQL table's column.
  /// </summary>
  public class MySqlDataColumn : DataColumn, INotifyPropertyChanged
  {
    #region Fields

    /// <summary>
    /// Flag indicating whether this column has an index automatically created for it.
    /// </summary>
    private bool _createIndex;

    /// <summary>
    /// Flag indicating whether this column will be excluded from the list of columns to be created on a new table's creation.
    /// </summary>
    private bool _excludeColumn;

    /// <summary>
    /// Flag indicating whether the column is part of the primary key.
    /// </summary>
    private bool _primaryKey;

    /// <summary>
    /// Flag indicating if the column has a related unique index.
    /// </summary>
    private bool _uniqueKey;

    /// <summary>
    /// List of text strings containing warnings for users about the column properties that could cause errors when creating this column in a database table.
    /// </summary>
    private readonly List<string> _columnWarningTextsList;

    #endregion Fields

    /// <summary>
    /// Initializes a new instance of the <see cref="MySqlDataColumn"/> class.
    /// </summary>
    public MySqlDataColumn()
    {
      _columnWarningTextsList = new List<string>(3);
      AutoIncrement = false;
      DisplayName = string.Empty;
      InExportMode = false;
      IsDisplayNameDuplicate = false;
      IsEmpty = true;
      ExcludeColumn = false;
      IsMySqlDataTypeValid = true;
      MappedDataColName = null;
      MappedDataColOrdinal = -1;
      MySqlDataType = string.Empty;
      PrimaryKey = false;
      RowsFrom1stDataType = string.Empty;
      RowsFrom2ndDataType = string.Empty;
      Unsigned = false;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="MySqlDataColumn"/> class.
    /// </summary>
    /// <param name="inExportMode">Flag indicating if the column is being constructed for exporting it to a new MySQL table.</param>
    public MySqlDataColumn(bool inExportMode)
      : this()
    {
      InExportMode = inExportMode;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="MySqlDataColumn"/> class.
    /// </summary>
    /// <param name="columnName">Name of the column.</param>
    /// <param name="mySqlFullDataType">Data type for a table column supported by MySQL Server.</param>
    /// <param name="datesAsMySqlDates">Flag indicating if the data type for dates will be stored as <see cref="MySql.Data.Types.MySqlDateTime"/>
    /// or <see cref="System.DateTime"/>.</param>
    /// <param name="allowNulls">Flag indicating if the column will accept null values.</param>
    /// <param name="isPrimaryKey">Flag indicating if the column is part of the primary key.</param>
    /// <param name="extraInfo">Extra information related to the column's data type as stored by the MySQL server.</param>
    public MySqlDataColumn(string columnName, string mySqlFullDataType, bool datesAsMySqlDates, bool allowNulls, bool isPrimaryKey, string extraInfo)
      : this()
    {
      DisplayName = ColumnName = columnName;
      AllowNull = allowNulls;
      Unsigned = mySqlFullDataType.Contains("unsigned");
      if (!string.IsNullOrEmpty(extraInfo))
      {
        AutoIncrement = extraInfo.Contains("auto_increment");
      }

      MySqlDataType = mySqlFullDataType;
      DataType = DataTypeUtilities.NameToType(StrippedMySqlDataType, Unsigned, datesAsMySqlDates);
      PrimaryKey = isPrimaryKey;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="MySqlDataColumn"/> class.
    /// </summary>
    /// <param name="columnName">Name of the column.</param>
    /// <param name="mySqlFullDataType">Data type for a table column supported by MySQL Server.</param>
    /// <param name="datesAsMySqlDates">Flag indicating if the data type for dates will be stored as <see cref="MySql.Data.Types.MySqlDateTime"/>
    /// or <see cref="System.DateTime"/>.</param>
    public MySqlDataColumn(string columnName, string mySqlFullDataType, bool datesAsMySqlDates)
      : this(columnName, mySqlFullDataType, datesAsMySqlDates, false, false, string.Empty)
    {
    }

    #region Properties

    /// <summary>
    /// Gets or sets a value indicating whether the column will accept null values.
    /// </summary>
    public bool AllowNull { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether the column is used in an auto-generated primary key.
    /// </summary>
    public bool AutoPk { get; set; }

    /// <summary>
    /// Gets the <see cref="DataColumn.ColumnName"/> escaping the back-tick character.
    /// </summary>
    public string ColumnNameForSqlQueries
    {
      get
      {
        return ColumnName.Replace("`", "``");
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
    /// Gets a value indicating whether the column is being constructed for exporting it to a new MySQL table.
    /// </summary>
    public bool InExportMode { get; private set; }

    /// <summary>
    /// Gets a value indicating if the <see cref="DisplayName"/> property value is not a duplicate of the one in another column.
    /// </summary>
    public bool IsDisplayNameDuplicate { get; private set; }

    /// <summary>
    /// Gets or sets a value indicating whether the column has no data.
    /// </summary>
    public bool IsEmpty { get; set; }

    /// <summary>
    /// Gets a value indicating whether the column's data type is a valid MySQL data type.
    /// </summary>
    public bool IsMySqlDataTypeValid { get; private set; }

    /// <summary>
    /// Gets or sets the name of the column in a source <see cref="MySqlDataTable"/> from which data will be appended from.
    /// </summary>
    public string MappedDataColName { get; set; }

    /// <summary>
    /// Gets or sets the ordinal index of the column in a source <see cref="MySqlDataTable"/> from which data will be appended from.
    /// </summary>
    public int MappedDataColOrdinal { get; set; }

    /// <summary>
    /// Gets or sets the corresponding data type supported by MySQL Server for this column.
    /// </summary>
    public string MySqlDataType { get; private set; }

    /// <summary>
    /// Gets the consistent data type that can hold the data for all rows starting from the first row.
    /// </summary>
    public string RowsFrom1stDataType { get; set; }

    /// <summary>
    /// Gets the consistent data type that can hold the data for all rows starting from the second row.
    /// </summary>
    public string RowsFrom2ndDataType { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether numeric data in this column is unsigned, meaningful only if the data type is integer-based.
    /// </summary>
    public bool Unsigned { get; set; }

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

        var mySqlDataTable = Table as MySqlDataTable;
        if (mySqlDataTable != null && (!_createIndex && mySqlDataTable.AutoAllowEmptyNonIndexColumns))
        {
          AllowNull = true;
        }

        OnPropertyChanged("CreateIndex");
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

        if (_excludeColumn && PrimaryKey)
        {
          PrimaryKey = false;
        }

        if (UpdateWarnings(!_excludeColumn, null))
        {
          OnColumnWarningsChanged();
        }

        OnPropertyChanged("ExcludeColumn");
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

        if (UpdateWarnings(!columnValuesAreUnique, Resources.ColumnDataNotUniqueWarning))
        {
          OnColumnWarningsChanged();
        }

        OnPropertyChanged("UniqueKey");
      }
    }

    /// <summary>
    /// Gets a value indicating whether the column's data must be wrapped with quotes to be used in queries.
    /// </summary>
    public bool ColumnsRequireQuotes
    {
      get
      {
        return IsCharOrText || IsDate || IsSetOrEnum;
      }
    }

    /// <summary>
    /// Gets the last warning text associated to this column.
    /// </summary>
    public string CurrentColumnWarningText
    {
      get
      {
        return _columnWarningTextsList != null && _columnWarningTextsList.Count > 0 && !ExcludeColumn ? _columnWarningTextsList.Last() : string.Empty;
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
    /// Gets a <see cref="MySql.Data.MySqlClient.MySqlDbType"/> object corresponding to this column's data type.
    /// </summary>
    public MySqlDbType MySqlDbType
    {
      get
      {
        string strippedType = StrippedMySqlDataType;
        return !string.IsNullOrEmpty(strippedType) ? DataTypeUtilities.NameToMySqlType(strippedType, Unsigned, false) : MySqlDbType.VarChar;
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
    /// Gets the MySQL data type descriptor without any options wrapped by parenthesis.
    /// </summary>
    public string StrippedMySqlDataType
    {
      get
      {
        if (string.IsNullOrEmpty(MySqlDataType))
        {
          return MySqlDataType;
        }

        int lParensIndex = MySqlDataType.IndexOf("(", StringComparison.Ordinal);
        string typeMinusParenthesis = lParensIndex < 0 ? MySqlDataType : MySqlDataType.Substring(0, lParensIndex);
        return typeMinusParenthesis.Replace("unsigned", string.Empty).TrimEnd();
      }
    }

    /// <summary>
    /// Gets the number of warnings associated to this column.
    /// </summary>
    public int WarningsQuantity
    {
      get
      {
        return _columnWarningTextsList != null ? _columnWarningTextsList.Count : 0;
      }
    }

    #endregion Properties

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

      foreach (string strValueFromArray in parentTable.Rows.Cast<DataRow>().Where(dr => !parentTable.FirstRowIsHeaders || rowIdx++ != 0).Select(dr => dr[Ordinal].ToString()).Where(strValueFromArray => strValueFromArray.Length != 0))
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
    /// Creates a new <see cref="MySqlDataColumn"/> object with a schema identical to this column's schema.
    /// </summary>
    /// <returns>A new <see cref="MySqlDataColumn"/> object with a schema cloned from this column.</returns>
    public MySqlDataColumn CloneSchema()
    {
      MySqlDataColumn clonedColumn = new MySqlDataColumn { ColumnName = ColumnName };
      clonedColumn.SetDisplayName(DisplayName);
      clonedColumn.DataType = DataType;
      clonedColumn.SetMySqlDataType(MySqlDataType);
      clonedColumn.RowsFrom1stDataType = RowsFrom1stDataType;
      clonedColumn.RowsFrom2ndDataType = RowsFrom2ndDataType;
      clonedColumn.AutoPk = AutoPk;
      clonedColumn.AllowNull = AllowNull;
      clonedColumn.PrimaryKey = PrimaryKey;
      clonedColumn.Unsigned = Unsigned;
      clonedColumn.AutoIncrement = AutoIncrement;
      clonedColumn.UniqueKey = UniqueKey;
      clonedColumn.ExcludeColumn = ExcludeColumn;
      clonedColumn.CreateIndex = CreateIndex;
      clonedColumn.MappedDataColName = MappedDataColName;
      clonedColumn.MappedDataColOrdinal = MappedDataColOrdinal;
      return clonedColumn;
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

      var mySqlDataTable = Table as MySqlDataTable;
      if (mySqlDataTable == null)
      {
        return null;
      }

      StringBuilder colDefinition = new StringBuilder();
      colDefinition.AppendFormat("`{0}` {1}", DisplayName.Replace("`", "``"), MySqlDataType);
      if (AutoPk || (PrimaryKey && mySqlDataTable.NumberOfPk == 1))
      {
        if (AutoIncrement)
        {
          colDefinition.Append(" auto_increment");
        }

        colDefinition.Append(" primary key");
      }
      else
      {
        colDefinition.AppendFormat(" {0}null", AllowNull ? string.Empty : "not ");
        if (AutoIncrement)
        {
          colDefinition.Append(" auto_increment");
        }

        if (UniqueKey)
        {
          colDefinition.Append(" unique key");
        }
      }

      return colDefinition.ToString();
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
    /// Sets the <see cref="DisplayName"/> property to the given display name.
    /// </summary>
    /// <param name="displayName">Display name.</param>
    /// <param name="addSuffixIfDuplicate">Flag indicating if a suffix is added to the display name if an existing column with the same name is found.</param>
    public void SetDisplayName(string displayName, bool addSuffixIfDuplicate = false)
    {
      if (DisplayName == displayName)
      {
        return;
      }

      bool colNameEmpty = displayName.Length == 0;
      string nonDuplicateDisplayName = displayName;
      if (UpdateWarnings(colNameEmpty, Resources.ColumnNameRequiredWarning))
      {
        OnColumnWarningsChanged();
      }

      if (!colNameEmpty && Table is MySqlDataTable)
      {
        nonDuplicateDisplayName = ParentTable.GetNonDuplicateColumnName(displayName, Ordinal);
      }

      IsDisplayNameDuplicate = !addSuffixIfDuplicate && displayName != nonDuplicateDisplayName;
      if (AutoPk)
      {
        ParentTable.UpdateAutoPkWarnings(IsDisplayNameDuplicate, Resources.PrimaryKeyColumnExistsWarning);
      }
      else
      {
        if (UpdateWarnings(IsDisplayNameDuplicate, Resources.ColumnExistsWarning))
        {
          OnColumnWarningsChanged();
        }
      }

      DisplayName = addSuffixIfDuplicate ? nonDuplicateDisplayName : displayName;
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
      IsMySqlDataTypeValid = true;
      dataType = dataType.Trim();
      MySqlDataType = dataType;

      if (MySqlDataType.Length == 0)
      {
        if (UpdateWarnings(true, Resources.ColumnDataTypeRequiredWarning))
        {
          OnColumnWarningsChanged();
        }

        return IsMySqlDataTypeValid;
      }

      bool warningsChanged = UpdateWarnings(false, Resources.ColumnDataTypeRequiredWarning);
      if (validateType)
      {
        IsMySqlDataTypeValid = DataTypeUtilities.ValidateUserDataType(dataType);
      }

      warningsChanged = UpdateWarnings(!IsMySqlDataTypeValid, Resources.ExportDataTypeNotValidWarning) || warningsChanged;
      if (IsMySqlDataTypeValid && testTypeOnData)
      {
        TestColumnDataTypeAgainstColumnData();
      }

      MySqlDataTable parentTable = Table as MySqlDataTable;
      if (parentTable != null)
      {
        if (!CreateIndex && IsInteger && parentTable.AutoIndexIntColumns)
        {
          CreateIndex = true;
        }

        if (!parentTable.DetectDatatype)
        {
          RowsFrom1stDataType = MySqlDataType;
          RowsFrom2ndDataType = MySqlDataType;
        }
      }

      if (warningsChanged)
      {
        OnColumnWarningsChanged();
      }

      return IsMySqlDataTypeValid;
    }

    /// <summary>
    /// Checks if this column's data type is right for the data currently stored in the column.
    /// </summary>
    /// <returns><c>true</c> if the column's data fits the data type, <c>false</c> otherwise.</returns>
    private bool TestColumnDataTypeAgainstColumnData()
    {
      bool dataFitsIntoType = MySqlDataType.Length > 0 && CanBeOfMySqlDataType(MySqlDataType);
      if (UpdateWarnings(!dataFitsIntoType, Resources.ExportDataTypeNotSuitableWarning))
      {
        OnColumnWarningsChanged();
      }

      return dataFitsIntoType;
    }

    /// <summary>
    /// Adds or removes warnings related to this column's creation.
    /// </summary>
    /// <param name="addWarning">true to add a new warning to the column's warnings collection, false to remove the given warning and display another existing warning.</param>
    /// <param name="warningResourceText">Warning text to display to users.</param>
    /// <returns><c>true</c> if a warning was added or removed, <c>false</c> otherwise.</returns>
    private bool UpdateWarnings(bool addWarning, string warningResourceText)
    {
      bool warningsChanged = false;

      if (addWarning)
      {
        // Only add the warning text if it is not empty and not already added to the warnings list
        if (string.IsNullOrEmpty(warningResourceText) || _columnWarningTextsList.Contains(warningResourceText))
        {
          return false;
        }

        _columnWarningTextsList.Add(warningResourceText);
        warningsChanged = true;
      }
      else
      {
        // We do not want to show a warning or we want to remove a warning if warningResourceText != null
        if (!string.IsNullOrEmpty(warningResourceText))
        {
          // Remove the warning and check if there is an stored warning, if so we want to pull it and show it
          warningsChanged = _columnWarningTextsList.Remove(warningResourceText);
        }
      }

      return warningsChanged;
    }
  }

  /// <summary>
  /// Event arguments for the ColumnWarningsChanged event.
  /// </summary>
  public class ColumnWarningsChangedArgs : EventArgs
  {
    /// <summary>
    /// Initializes a new instance of the <see cref="ColumnWarningsChangedArgs"/> class.
    /// </summary>
    /// <param name="column">The column that warnings are related to.</param>
    public ColumnWarningsChangedArgs(MySqlDataColumn column)
    {
      CurrentWarningText = column.CurrentColumnWarningText;
      WarningsQuantity = column.WarningsQuantity;
    }

    /// <summary>
    /// Gets the last warning text associated to this column.
    /// </summary>
    public string CurrentWarningText { get; private set; }

    /// <summary>
    /// Gets the number of warnings associated to this column.
    /// </summary>
    public int WarningsQuantity { get; private set; }
  }
}