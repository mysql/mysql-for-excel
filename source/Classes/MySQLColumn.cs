using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Data;
using MySQL.Utility;
using MySql.Data.MySqlClient;

namespace MySQL.ForExcel
{
  public class MySQLColumn : INotifyPropertyChanged
  {
    private string characterSet;
    private bool isNew;

    public MySQLColumn(DataRow row, MySQLTable table)
    {
      isNew = row == null;
      OwningTable = table;
      if (row != null)
        ParseColumnInfo(row);
      AllowNull = true;
      MappedDataColName = null;
    }

    public event PropertyChangedEventHandler PropertyChanged;

    #region Properties

    [Browsable(false)]
    public string MappedDataColName;

    [Browsable(false)]
    public MySQLTable OwningTable;

    private string _columnName;
    [Category("General")]
    [Description("The name of this column")]
    public string ColumnName
    {
      get { return _columnName; }
      set 
      { 
        _columnName = value;
        if (PropertyChanged != null)
          PropertyChanged(this, new PropertyChangedEventArgs("Name"));
      }
    }

    private string _dataType;
    [Category("General")]
    [DisplayName("Data Type")]
    [TypeConverter(typeof(DataTypeConverter))]
    [RefreshProperties(RefreshProperties.All)]
    public string DataType
    {
      get { return _dataType; }
      set 
      {
        _dataType = value;
        cleanDataType(false);
        if (PropertyChanged != null)
          PropertyChanged(this, new PropertyChangedEventArgs("DataType"));
      }
    }

    private bool _allowNull;
    [TypeConverter(typeof(YesNoTypeConverter))]
    [Category("Options")]
    [DisplayName("Allow Nulls")]
    public bool AllowNull
    {
      get { return _allowNull; }
      set { _allowNull = value; }
    }

    private bool _isUnsigned;
    [TypeConverter(typeof(YesNoTypeConverter))]
    [Category("Options")]
    [DisplayName("Is Unsigned")]
    public bool IsUnsigned
    {
      get { return _isUnsigned; }
      set { _isUnsigned = value; }
    }

    private bool _isZeroFill;
    [TypeConverter(typeof(YesNoTypeConverter))]
    [Category("Options")]
    [DisplayName("Is Zerofill")]
    public bool IsZerofill
    {
      get { return _isZeroFill; }
      set { _isZeroFill = value; }
    }

    private string _defaultValue;
    [Category("General")]
    [DisplayName("Default Value")]
    public string DefaultValue
    {
      get { return _defaultValue; }
      set { _defaultValue = value; }
    }

    private bool _autoIncrement;
    [TypeConverter(typeof(YesNoTypeConverter))]
    [Category("Options")]
    [DisplayName("Autoincrement")]
    public bool AutoIncrement
    {
      get { return _autoIncrement; }
      set { _autoIncrement = value; }
    }

    private bool _primaryKey;
    [TypeConverter(typeof(YesNoTypeConverter))]
    [Category("Options")]
    [DisplayName("Primary Key")]
    public bool PrimaryKey
    {
      get { return _primaryKey; }
      set
      {
        _primaryKey = value;
        if (PropertyChanged != null)
          PropertyChanged(this, new PropertyChangedEventArgs("PrimaryKey"));
      }
    }

    private bool _uniqueKey;
    [TypeConverter(typeof(YesNoTypeConverter))]
    [Category("Options")]
    [DisplayName("Unique Key")]
    public bool UniqueKey
    {
      get { return _uniqueKey; }
      set { _uniqueKey = value; }
    }

    private int _charMaxLength;
    [DisplayName("Character Length")]
    public int CharMaxLength
    {
      get { return _charMaxLength; }
      set { _charMaxLength = value; }
    }

    private int _precision;
    public int Precision
    {
      get { return _precision; }
      set { _precision = value; }
    }

    private int _scale;
    public int Scale
    {
      get { return _scale; }
      set { _scale = value; }
    }

    [Category("Encoding")]
    [DisplayName("Character Set")]
    [TypeConverter(typeof(CharacterSetTypeConverter))]
    public string CharacterSet
    {
      get { return characterSet; }
      set
      {
        if (value != characterSet)
          Collation = String.Empty;
        characterSet = value;
      }
    }

    private string _collation;
    [Category("Encoding")]
    [TypeConverter(typeof(CollationTypeConverter))]
    public string Collation
    {
      get { return _collation; }
      set { _collation = value; }
    }

    private string _comment;
    [Category("Miscellaneous")]
    public string Comment
    {
      get { return _comment; }
      set { _comment = value; }
    }

    [Browsable(false)]
    public bool IsDecimal
    {
      get
      {
        string toLowerDataType = _dataType.ToLowerInvariant();
        return toLowerDataType == "real" || toLowerDataType == "double" || toLowerDataType == "float" || toLowerDataType == "decimal" || toLowerDataType == "numeric";
      }
    }

    [Browsable(false)]
    public bool IsNumeric
    {
      get
      {
        string toLowerDataType = _dataType.ToLowerInvariant();
        return IsDecimal || toLowerDataType.Contains("int");
      }
    }

    [Browsable(false)]
    public bool IsChar
    {
      get
      {
        string toLowerDataType = _dataType.ToLowerInvariant();
        return toLowerDataType.Contains("char");
      }
    }

    [Browsable(false)]
    public bool IsCharOrText
    {
      get
      {
        string toLowerDataType = _dataType.ToLowerInvariant();
        return toLowerDataType.Contains("char") || toLowerDataType.Contains("text");
      }
    }

    [Browsable(false)]
    public bool IsBinary
    {
      get
      {
        string toLowerDataType = _dataType.ToLowerInvariant();
        return toLowerDataType.Contains("binary");
      }
    }

    [Browsable(false)]
    public bool IsDate
    {
      get
      {
        string toLowerDataType = _dataType.ToLowerInvariant();
        return toLowerDataType.Contains("date") || toLowerDataType == "timestamp";
      }
    }

    [Browsable(false)]
    public bool ColumnsRequireQuotes
    {
      get
      {
        return IsCharOrText || IsDate;
      }
    }

    [Browsable(false)]
    public MySqlDbType MySQLDBType
    {
      get
      {
        return Utilities.NameToType(_dataType, _isUnsigned, false);
      }
    }

    private bool _createIndex;
    [Category("Export")]
    public bool CreateIndex
    {
      get { return _createIndex; }
      set { _createIndex = value; }
    }

    private bool _excludeColumn;
    [Category("Export")]
    public bool ExcludeColumn
    {
      get { return _excludeColumn; }
      set { _excludeColumn = value; }
    }

    #endregion

    private void ParseColumnInfo(DataRow row)
    {
      ColumnName = row["COLUMN_NAME"].ToString();
      AllowNull = row["IS_NULLABLE"] != DBNull.Value && row["IS_NULLABLE"].ToString() == "YES";
      Comment = row["COLUMN_COMMENT"].ToString();
      Collation = row["COLLATION_NAME"].ToString();
      CharacterSet = row["CHARACTER_SET_NAME"].ToString();
      DefaultValue = row["COLUMN_DEFAULT"].ToString();

      bool supportsCharMaxLength = row.Table.Columns.Contains("CHARACTER_MAXIMUM_LENGTH");
      string columnType = row["COLUMN_TYPE"].ToString().ToLowerInvariant();
      int index = columnType.IndexOf(' ');
      if (index == -1)
        index = columnType.Length;
      DataType = columnType.Substring(0, index);
      if (supportsCharMaxLength)
        CharMaxLength = (row["CHARACTER_MAXIMUM_LENGTH"] == DBNull.Value ? 0 : Convert.ToInt32(row["CHARACTER_MAXIMUM_LENGTH"]));

      columnType = columnType.Substring(index);
      IsUnsigned = columnType.IndexOf("unsigned") != -1;
      IsZerofill = columnType.IndexOf("zerofill") != -1;

      PrimaryKey = row["COLUMN_KEY"].ToString() == "PRI";
      Precision = (row["NUMERIC_PRECISION"] == DBNull.Value ? 0 : Convert.ToInt32(row["NUMERIC_PRECISION"]));
      Scale = (row["NUMERIC_SCALE"] == DBNull.Value ? 0 : Convert.ToInt32(row["NUMERIC_SCALE"]));

      string extra = row["EXTRA"].ToString().ToLowerInvariant();
      if (extra != null)
        AutoIncrement = extra.IndexOf("auto_increment") != -1;
    }

    private void cleanDataType(bool getCharMaxLengthFromDataType)
    {
      int lParensIdx = _dataType.IndexOf("(");
      if (lParensIdx == -1)
        return;
      if (getCharMaxLengthFromDataType && (_dataType.Contains("char") || _dataType.Contains("binary")))
      {
        int rParensIdx = _dataType.IndexOf(")");
        CharMaxLength = Int32.Parse(_dataType.Substring(lParensIdx + 1, rParensIdx - lParensIdx - 1));
      }
      _dataType = _dataType.Substring(0, lParensIdx);
    }

    #region Methods needed so PropertyGrid won't bold our values

    private bool ShouldSerializeColumnName() { return false; }
    private bool ShouldSerializeDataType() { return false; }
    private bool ShouldSerializeAllowNull() { return false; }
    private bool ShouldSerializeIsUnsigned() { return false; }
    private bool ShouldSerializeIsZerofill() { return false; }
    private bool ShouldSerializeDefaultValue() { return false; }
    private bool ShouldSerializeAutoIncrement() { return false; }
    private bool ShouldSerializePrimaryKey() { return false; }
    private bool ShouldSerializePrecision() { return false; }
    private bool ShouldSerializeScale() { return false; }
    private bool ShouldSerializeCharacterSet() { return false; }
    private bool ShouldSerializeCollation() { return false; }
    private bool ShouldSerializeComment() { return false; }

    #endregion

    public void ResetProperties()
    {
      Collation = "";
      CharacterSet = "";
      AutoIncrement = false;
      DefaultValue = "";
      IsUnsigned = false;
      IsZerofill = false;
      Precision = 0;
      Scale = 0;
      CharMaxLength = 0;
    }

    public void AssignDataType(string dataType, int charMaxLen)
    {
      bool unsigned = false;

      if (dataType.Contains("unsigned"))
      {
        dataType = dataType.Substring(0, dataType.IndexOf(" "));
        unsigned = true;
      }
      if (dataType != _dataType)
        ResetProperties();
      DataType = dataType;
      if (IsCharOrText || IsBinary)
        CharMaxLength = charMaxLen;
      IsUnsigned = unsigned;
    }

    public string GetSQL()
    {
      if (String.IsNullOrEmpty(ColumnName))
        return null;

      StringBuilder colDefinition = new StringBuilder(ColumnName);
      colDefinition.AppendFormat(" {0}", _dataType);
      if (IsNumeric)
      {
        if (Precision > 0)
          colDefinition.AppendFormat("({0}", Precision);
        if (Scale > 0)
          colDefinition.AppendFormat(",{0}", Scale);
        if (Precision > 0)
          colDefinition.Append(")");
      }
      if (IsBinary || IsChar)
        colDefinition.AppendFormat("({0})", CharMaxLength);
      if (IsUnsigned)
        colDefinition.Append(" unsigned");
      if (IsZerofill)
        colDefinition.Append(" zerofill");
      if (AllowNull)
        colDefinition.Append(" null");
      if (!String.IsNullOrEmpty(DefaultValue))
        colDefinition.AppendFormat(" default {0}{1}{0}",
                                   (IsCharOrText ? "'" : String.Empty),
                                   DefaultValue);
      if (AutoIncrement)
        colDefinition.Append(" auto_increment");
      if (UniqueKey)
        colDefinition.Append(" unique key");

      return colDefinition.ToString();
    }
  }

  public class DataTypeConverter : StringConverter
  {
    private List<string> dataTypes;

    public DataTypeConverter()
    {
      dataTypes = Utilities.GetDataTypes();
    }

    public override bool GetStandardValuesSupported(ITypeDescriptorContext context)
    {
      return true;
    }

    public override bool GetStandardValuesExclusive(ITypeDescriptorContext context)
    {
      return true;
    }

    public override StandardValuesCollection GetStandardValues(ITypeDescriptorContext context)
    {
      StandardValuesCollection coll = new StandardValuesCollection(dataTypes);
      return coll;
    }
  }

  public class YesNoTypeConverter : TypeConverter
  {
    public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
    {
      if (sourceType == typeof(string))
        return true;
      return base.CanConvertFrom(context, sourceType);
    }

    public override bool CanConvertTo(ITypeDescriptorContext context, Type destinationType)
    {
      if (destinationType == typeof(string))
        return true;
      return base.CanConvertTo(context, destinationType);
    }

    public override object ConvertFrom(ITypeDescriptorContext context, System.Globalization.CultureInfo culture, object value)
    {
      if (value.GetType() == typeof(string))
      {
        if (((string)value).ToLower() == "yes")
          return true;
        if (((string)value).ToLower() == "no")
          return false;
        throw new Exception("Values must be \"Yes\" or \"No\"");
      }

      return base.ConvertFrom(context, culture, value);
    }

    public override object ConvertTo(ITypeDescriptorContext context, System.Globalization.CultureInfo culture, object value, Type destinationType)
    {
      if (destinationType == typeof(string))
      {
        return (((bool)value) ? "Yes" : "No");
      }

      return base.ConvertTo(context, culture, value, destinationType);
    }

    public override bool GetStandardValuesSupported(ITypeDescriptorContext context)
    {
      return true;
    }

    public override System.ComponentModel.TypeConverter.StandardValuesCollection GetStandardValues(ITypeDescriptorContext context)
    {
      bool[] bools = new bool[] { true, false };
      System.ComponentModel.TypeConverter.StandardValuesCollection svc = new System.ComponentModel.TypeConverter.StandardValuesCollection(bools);
      return svc;
    }
  }
}
