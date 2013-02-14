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

  /// <summary>
  /// Represents the schema of a MySQL table's column.
  /// </summary>
  public class MySQLDataColumn : DataColumn
  {
    /// <summary>
    /// Flag indicating if the column has a related unique index.
    /// </summary>
    private bool _uniqueKey;

    /// <summary>
    /// Initializes a new instance of the <see cref="MySQLDataColumn"/> class.
    /// </summary>
    public MySQLDataColumn()
    {
      WarningTextList = new List<string>(3);
      MappedDataColName = null;
      MySQLDataType = string.Empty;
      RowsFrom1stDataType = string.Empty;
      RowsFrom2ndDataType = string.Empty;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="MySQLDataColumn"/> class.
    /// </summary>
    /// <param name="columnName">Name of the column.</param>
    /// <param name="mySQLFullDataType">Data type for a table column supported by MySQL Server.</param>
    /// <param name="datesAsMySQLDates">Flag indicating if the data type for dates will be stored as <see cref="MySql.Data.Types.MySqlDateTime"/> or <see cref="System.DateTime"/>.</param>
    /// <param name="allowNulls">Flag indicating if the column will accept null values.</param>
    /// <param name="isPrimaryKey">Flag indicating if the column is part of the primary key.</param>
    /// <param name="extraInfo">Extra information related to the column's data type as stored by the MySQL server.</param>
    public MySQLDataColumn(string columnName, string mySQLFullDataType, bool datesAsMySQLDates, bool allowNulls, bool isPrimaryKey, string extraInfo)
      : this()
    {
      DisplayName = ColumnName = columnName;
      AllowNull = allowNulls;
      Unsigned = false;
      AutoIncrement = false;
      Unsigned  = mySQLFullDataType.Contains("unsigned");
      if (!string.IsNullOrEmpty(extraInfo))
      {
        AutoIncrement = extraInfo.Contains("auto_increment");
      }

      MySQLDataType = mySQLFullDataType;
      DataType = DataTypeUtilities.NameToType(StrippedMySQLDataType, Unsigned, datesAsMySQLDates);
      PrimaryKey = isPrimaryKey;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="MySQLDataColumn"/> class.
    /// </summary>
    /// <param name="columnName">Name of the column.</param>
    /// <param name="mySQLFullDataType">Data type for a table column supported by MySQL Server.</param>
    /// <param name="datesAsMySQLDates">Flag indicating if the data type for dates will be stored as <see cref="MySql.Data.Types.MySqlDateTime"/> or <see cref="System.DateTime"/>.</param>
    public MySQLDataColumn(string columnName, string mySQLFullDataType, bool datesAsMySQLDates)
      : this(columnName, mySQLFullDataType, datesAsMySQLDates, false, false, string.Empty)
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
    public bool AutoPK { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether this column has an index automatically created for it.
    /// </summary>
    public bool CreateIndex { get; set; }

    /// <summary>
    /// Gets or sets the name for this column, when its value is different than the one in <see cref="ColumnName"/> it means the latter represents an internal name and this column holds the real column name.
    /// </summary>
    public string DisplayName { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether this column will be excluded from the list of columns to be created on a new table's creation.
    /// </summary>
    public bool ExcludeColumn { get; set; }

    /// <summary>
    /// Gets or sets the name of the column in a source <see cref="MySQLDataTable"/> from which data will be appended from.
    /// </summary>
    public string MappedDataColName { get; set; }

    /// <summary>
    /// Gets or sets the corresponding data type supported by MySQL Server for this column.
    /// </summary>
    public string MySQLDataType { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether the column is part of the primary key.
    /// </summary>
    public bool PrimaryKey { get; set; }

    /// <summary>
    /// Gets or sets the consistent data type that can hold the data for all rows starting from the 1st row.
    /// </summary>
    public string RowsFrom1stDataType { get; set; }

    /// <summary>
    /// Gets or sets the consistent data type that can hold the data for all rows starting from the 2nd row.
    /// </summary>
    public string RowsFrom2ndDataType { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether numeric data in this column is unsigned, meaningful only if the data type is integer-based.
    /// </summary>
    public bool Unsigned { get; set; }

    /// <summary>
    /// Gets a list of text strings containing warnings for users about the column properties that could cause errors when creating this column in a database table.
    /// </summary>
    public List<string> WarningTextList { get; private set; }

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
        if (_uniqueKey)
        {
          CreateIndex = true;
        }
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
    /// Gets a value indicating whether this column's data type is of floating-point nature.
    /// </summary>
    public bool IsDecimal
    {
      get
      {
        if (string.IsNullOrEmpty(StrippedMySQLDataType))
        {
          return false;
        }

        string toLowerDataType = StrippedMySQLDataType.ToLowerInvariant();
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
        if (string.IsNullOrEmpty(StrippedMySQLDataType))
        {
          return false;
        }

        string toLowerDataType = StrippedMySQLDataType.ToLowerInvariant();
        return IsDecimal || toLowerDataType.Contains("int");
      }
    }

    /// <summary>
    /// Gets a value indicating whether this column's data type is fixed or variable sized character-based.
    /// </summary>
    public bool IsChar
    {
      get
      {
        if (string.IsNullOrEmpty(StrippedMySQLDataType))
        {
          return false;
        }

        string toLowerDataType = StrippedMySQLDataType.ToLowerInvariant();
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
        if (string.IsNullOrEmpty(StrippedMySQLDataType))
        {
          return false;
        }

        string toLowerDataType = StrippedMySQLDataType.ToLowerInvariant();
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
        if (string.IsNullOrEmpty(StrippedMySQLDataType))
        {
          return false;
        }

        string toLowerDataType = StrippedMySQLDataType.ToLowerInvariant();
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
        if (string.IsNullOrEmpty(StrippedMySQLDataType))
        {
          return false;
        }

        string toLowerDataType = MySQLDataType.ToLowerInvariant();
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
        if (string.IsNullOrEmpty(StrippedMySQLDataType))
        {
          return false;
        }

        string toLowerDataType = StrippedMySQLDataType.ToLowerInvariant();
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
        if (string.IsNullOrEmpty(StrippedMySQLDataType))
        {
          return false;
        }

        string toLowerDataType = StrippedMySQLDataType.ToLowerInvariant();
        return toLowerDataType.Contains("date") || toLowerDataType == "timestamp";
      }
    }

    /// <summary>
    /// Gets the MySQL data type descriptor without any options wrapped by parenthesis.
    /// </summary>
    public string StrippedMySQLDataType
    {
      get
      {
        if (string.IsNullOrEmpty(MySQLDataType))
        {
          return MySQLDataType;
        }

        int lParensIndex = MySQLDataType.IndexOf("(");
        return lParensIndex < 0 ? MySQLDataType : MySQLDataType.Substring(0, lParensIndex);
      }
    }

    /// <summary>
    /// Gets a <see cref="MySql.Data.MySQLClient.MySqlDbType"/> object corresponding to this column's data type.
    /// </summary>
    public MySqlDbType MySQLDBType
    {
      get
      {
        string strippedType = StrippedMySQLDataType;
        return !string.IsNullOrEmpty(strippedType) ? DataTypeUtilities.NameToMySQLType(strippedType, Unsigned, false) : MySqlDbType.VarChar;
      }
    }

    #endregion Properties

    /// <summary>
    /// Checks if the data stored in this column would fit within the given data type.
    /// </summary>
    /// <param name="mySQLDataType">Data type for a table column supported by MySQL Server.</param>
    /// <returns><see cref="true"/> if the data stored in this column would fit, <see cref="false"/> otherwise.</returns>
    public bool CanBeOfMySQLDataType(string mySQLDataType)
    {
      bool result = true;

      MySQLDataTable parentTable = Table as MySQLDataTable;
      int rowIdx = 0;
      foreach (DataRow dr in parentTable.Rows)
      {
        if (parentTable.FirstRowIsHeaders && rowIdx++ == 0)
        {
          continue;
        }

        string strValueFromArray = dr[Ordinal].ToString();
        if (strValueFromArray.Length == 0)
        {
          continue;
        }

        result = result && DataTypeUtilities.StringValueCanBeStoredWithMySQLType(strValueFromArray, mySQLDataType);
      }

      return result;
    }

    /// <summary>
    /// Creates a new <see cref="MySQLDataColumn"/> object with a schema identical to this column's schema.
    /// </summary>
    /// <returns>A new <see cref="MySQLDataColumn"/> object with a achema cloned from this column.</returns>
    public MySQLDataColumn CloneSchema()
    {
      MySQLDataColumn clonedColumn = new MySQLDataColumn();
      clonedColumn.ColumnName = this.ColumnName;
      clonedColumn.DisplayName = this.DisplayName;
      clonedColumn.DataType = this.DataType;
      clonedColumn.MySQLDataType = MySQLDataType;
      clonedColumn.AutoPK = AutoPK;
      clonedColumn.AllowNull = AllowNull;
      clonedColumn.PrimaryKey = PrimaryKey;
      clonedColumn.Unsigned = Unsigned;
      clonedColumn.AutoIncrement = AutoIncrement;
      clonedColumn.UniqueKey = UniqueKey;
      clonedColumn.ExcludeColumn = ExcludeColumn;
      return clonedColumn;
    }

    /// <summary>
    /// Creates a SQL query fragment meant to be used within a CREATE TABLE statement to create a column with this column's schema.
    /// </summary>
    /// <returns>A SQL query fragment describing this column's schema.</returns>
    public string GetSQL()
    {
      if (string.IsNullOrEmpty(DisplayName))
      {
        return null;
      }

      StringBuilder colDefinition = new StringBuilder();
      colDefinition.AppendFormat("`{0}` {1}", DisplayName.Replace("`", "``"), MySQLDataType);
      if (AutoPK || (PrimaryKey && (Table as MySQLDataTable).NumberOfPK == 1))
      {
        if (AutoIncrement)
        {
          colDefinition.Append(" auto_increment");
        }

        colDefinition.Append(" primary key");
      }
      else
      {
        colDefinition.AppendFormat(" {0}null", (AllowNull ? string.Empty : "not "));
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
  }
}
