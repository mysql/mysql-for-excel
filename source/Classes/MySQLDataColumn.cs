using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using MySql.Data.MySqlClient;

namespace MySQL.ForExcel
{
  class MySQLDataColumn : DataColumn
  {
    private bool uniqueKey;
    private string displayName;
    private List<string> warningTextList = new List<string>(3);

    public bool AutoPK { get; set; }
    public bool CreateIndex { get; set; }
    public bool UniqueKey
    {
      get { return uniqueKey; }
      set { uniqueKey = value; if (uniqueKey) CreateIndex = true; }
    }
    public string DisplayName
    {
      get { return displayName; }
      set
      {
        string trimmedName = value.Trim();
        displayName = trimmedName;
        if (Table == null || Table.Columns.Count < 2)
          return;
        int colIdx = 1;
        while (Table.Columns.OfType<MySQLDataColumn>().Count(col => col.DisplayName == displayName) > 1)
        {
          displayName = trimmedName + colIdx;
        }
      }
    }

    public bool PrimaryKey { get; set; }
    public bool AllowNull { get; set; }
    public bool ExcludeColumn { get; set; }
    public bool Unsigned { get; set; }
    public string MySQLDataType { get; set; }
    public List<string> WarningTextList { get { return warningTextList; } }
    public string RowsFrom1stDataType { get; set; }
    public string RowsFrom2ndDataType { get; set; }
    public string MappedDataColName { get; set; }

    #region Getter Properties

    public bool IsDecimal
    {
      get
      {
        string toLowerDataType = MySQLDataType.ToLowerInvariant();
        return toLowerDataType == "real" || toLowerDataType == "double" || toLowerDataType == "float" || toLowerDataType == "decimal" || toLowerDataType == "numeric";
      }
    }

    public bool IsNumeric
    {
      get
      {
        string toLowerDataType = MySQLDataType.ToLowerInvariant();
        return IsDecimal || toLowerDataType.Contains("int");
      }
    }

    public bool IsChar
    {
      get
      {
        string toLowerDataType = MySQLDataType.ToLowerInvariant();
        return toLowerDataType.Contains("char");
      }
    }

    public bool IsCharOrText
    {
      get
      {
        string toLowerDataType = MySQLDataType.ToLowerInvariant();
        return toLowerDataType.Contains("char") || toLowerDataType.Contains("text");
      }
    }

    public bool IsBinary
    {
      get
      {
        string toLowerDataType = MySQLDataType.ToLowerInvariant();
        return toLowerDataType.Contains("binary");
      }
    }

    public bool IsDate
    {
      get
      {
        string toLowerDataType = MySQLDataType.ToLowerInvariant();
        return toLowerDataType.Contains("date") || toLowerDataType == "timestamp";
      }
    }

    public bool ColumnsRequireQuotes
    {
      get { return IsCharOrText || IsDate; }
    }

    public string StrippedMySQLDataType
    {
      get
      {
        if (String.IsNullOrEmpty(MySQLDataType))
          return MySQLDataType;
        int lParensIndex = MySQLDataType.IndexOf("(");
        return (lParensIndex < 0 ? MySQLDataType : MySQLDataType.Substring(0, lParensIndex));
      }
    }

    public MySqlDbType MySQLDBType
    {
      get
      {
        string strippedType = StrippedMySQLDataType;
        return (!String.IsNullOrEmpty(strippedType) ? DataTypeUtilities.NameToMySQLType(strippedType, Unsigned, false) : MySqlDbType.VarChar);
      }
    }

    #endregion Getter Properties

    public MySQLDataColumn()
    {
      MappedDataColName = null;
    }

    public MySQLDataColumn(string columnName, string mySQLFullDataType, bool allowNulls, bool isPrimaryKey, string extraInfo)
      : this()
    {
      DisplayName = ColumnName = columnName;
      AllowDBNull = AllowNull = allowNulls;
      Unsigned = false;
      AutoIncrement = false;
      if (!String.IsNullOrEmpty(extraInfo))
      {
        Unsigned = extraInfo.Contains("unsigned");
        AutoIncrement = extraInfo.Contains("auto_increment");
      }
      MySQLDataType = mySQLFullDataType;
      DataType = DataTypeUtilities.NameToType(StrippedMySQLDataType, Unsigned);
      PrimaryKey = isPrimaryKey;
    }

    public MySQLDataColumn(string columnName, string mySQLFullDataType)
      : this(columnName, mySQLFullDataType, false, false, String.Empty)
    {
    }

    [System.Obsolete("This will analyze all data as strings and will always detect everything as a Varchar.")]
    public void DetectType(bool firstRowIsHeaders)
    {
      object valueFromArray = null;
      string proposedType = String.Empty;
      string previousType = String.Empty;
      string headerType = String.Empty;
      bool typesConsistent = true;
      bool valueOverflow = false;
      string dateFormat = "yyyy-MM-dd HH:mm:ss";
      int rowPos = 0;

      foreach (DataRow dr in Table.Rows)
      {
        valueFromArray = dr[Ordinal];
        if (valueFromArray == null)
          continue;
        proposedType = DataTypeUtilities.GetMySQLExportDataType(valueFromArray, out valueOverflow);
        if (proposedType.StartsWith("Date") && valueFromArray is DateTime)
        {
          DateTime dtValue = (DateTime)valueFromArray;
          dr[Ordinal] = dtValue.ToString(dateFormat);
        }
        if (rowPos == 1)
          headerType = proposedType;
        else
        {
          typesConsistent = typesConsistent && (rowPos > 2 ? previousType == proposedType : true);
          previousType = proposedType;
        }
      }
      if (previousType.Length == 0)
        previousType = "Varchar(255)";
      if (headerType.Length == 0)
        headerType = previousType;
      RowsFrom1stDataType = headerType;
      RowsFrom2ndDataType = previousType;
      MySQLDataType = (firstRowIsHeaders ? headerType : previousType);
      rowPos++;
    }

    public bool CanBeOfMySQLDataType(string mySQLDataType)
    {
      bool result = true;

      MySQLDataTable parentTable = Table as MySQLDataTable;
      int rowIdx = 0;
      foreach (DataRow dr in parentTable.Rows)
      {
        if (parentTable.FirstRowIsHeaders && rowIdx++ == 0)
          continue;
        string strValueFromArray = dr[Ordinal].ToString();
        result = result && DataTypeUtilities.StringValueCanBeStoredWithMySQLType(strValueFromArray, mySQLDataType);
      }

      return result;
    }

    public string GetSQL()
    {
      if (String.IsNullOrEmpty(displayName))
        return null;

      StringBuilder colDefinition = new StringBuilder();
      colDefinition.AppendFormat("`{0}` {1}", displayName, MySQLDataType);
      if (AutoPK || (PrimaryKey && (Table as MySQLDataTable).NumberOfPK == 1))
        colDefinition.Append(" primary key");
      else if (UniqueKey)
        colDefinition.Append(" unique key");
      if (AllowNull)
        colDefinition.Append(" null");
      if (AutoIncrement)
        colDefinition.Append(" auto_increment");

      return colDefinition.ToString();
    }

  }
}
