// Copyright (c) 2014, 2016, Oracle and/or its affiliates. All rights reserved.
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
using System.Globalization;
using System.Linq;
using System.Text;
using MySql.Data.MySqlClient;
using MySql.Data.Types;
using MySQL.ForExcel.Classes.Exceptions;
using MySql.Utility.Classes;

namespace MySQL.ForExcel.Classes
{
  /// <summary>
  /// Represents a data type supported by the MySQL Server.
  /// </summary>
  public class MySqlDataType : ICloneable
  {
    #region Constants

    /// <summary>
    /// The MySQL attribute CURRENT_TIMESTAMP.
    /// </summary>
    public const string ATTRIBUTE_CURRENT_TIMESTAMP = "CURRENT_TIMESTAMP";

    /// <summary>
    /// The data type used for columns with no data.
    /// </summary>
    public const string DEFAULT_DATA_TYPE = "VarChar";

    /// <summary>
    /// The data type used for columns with no data.
    /// </summary>
    public const long DEFAULT_DATA_TYPE_LENGTH = 255;

    /// <summary>
    /// The maximum length a MySQL bigint column can hold.
    /// </summary>
    public const int MYSQL_BIGINT_MAX_LENGTH = 20;

    /// <summary>
    /// The maximum length a MySQL bit column can hold.
    /// </summary>
    public const int MYSQL_BIT_MAX_LENGTH = 64;

    /// <summary>
    /// The date format used by Date fields in MySQL databases.
    /// </summary>
    public const string MYSQL_DATE_FORMAT = "yyyy-MM-dd";

    /// <summary>
    /// The date format used by DateTime fields in MySQL databases.
    /// </summary>
    public const string MYSQL_DATETIME_FORMAT = "yyyy-MM-dd HH:mm:ss";

    /// <summary>
    /// The maximum length a MySQL date column can hold.
    /// </summary>
    public const int MYSQL_DATE_MAX_LENGTH = 10;

    /// <summary>
    /// The maximum length a MySQL date column can hold.
    /// </summary>
    public const int MYSQL_DATETIME_MAX_LENGTH = 26;

    /// <summary>
    /// The maximum length MySQL database objects can hold.
    /// </summary>
    public const int MYSQL_DB_OBJECTS_MAX_LENGTH = 64;

    /// <summary>
    /// The maximum length a MySQL decimal column can hold.
    /// </summary>
    public const int MYSQL_DECIMAL_MAX_LENGTH = 65;

    /// <summary>
    /// The maximum length a MySQL double column can hold.
    /// </summary>
    public const int MYSQL_DOUBLE_MAX_LENGTH = 310;

    /// <summary>
    /// Represents an empty date in MySQL DateTime format.
    /// </summary>
    public const string MYSQL_EMPTY_DATE = "0000-00-00 00:00:00";

    /// <summary>
    /// The maximum length a MySQL float column can hold.
    /// </summary>
    public const int MYSQL_FLOAT_MAX_LENGTH = 41;

    /// <summary>
    /// The maximum length a MySQL int column can hold.
    /// </summary>
    public const int MYSQL_INT_MAX_LENGTH = 11;

    /// <summary>
    /// The maximum length a MySQL mediumint column can hold.
    /// </summary>
    public const int MYSQL_MEDIUMINT_MAX_LENGTH = 8;

    /// <summary>
    /// The maximum length a MySQL mediumtext column can hold.
    /// </summary>
    public const int MYSQL_MEDIUMTEXT_MAX_LENGTH = 16777215;

    /// <summary>
    /// The maximum length a MySQL smallint column can hold.
    /// </summary>
    public const int MYSQL_SMALLINT_MAX_LENGTH = 6;

    /// <summary>
    /// The maximum length a MySQL time column can hold.
    /// </summary>
    public const int MYSQL_TIME_MAX_LENGTH = 17;

    /// <summary>
    /// The maximum length a MySQL tinyint column can hold.
    /// </summary>
    public const int MYSQL_TINYINT_MAX_LENGTH = 4;

    /// <summary>
    /// The maximum proposed length of the MySQL varchar data type.
    /// </summary>
    public const int MYSQL_VARCHAR_MAX_PROPOSED_LEN = 4000;

    /// <summary>
    /// The MySQL attribute UNSIGNED.
    /// </summary>
    private const string ATTRIBUTE_UNSIGNED = "UNSIGNED";

    /// <summary>
    /// The MySQL attribute ZEROFILL.
    /// </summary>
    private const string ATTRIBUTE_ZEROFILL = "ZEROFILL";

    #endregion Constants

    #region Fields

    /// <summary>
    /// An array of attributes specified in the <see cref="FullType"/>.
    /// </summary>
    private List<string> _attributes;

    /// <summary>
    /// The number of places after the decimal point for floating point data types.
    /// </summary>
    private int _decimalPlaces;

    /// <summary>
    /// The <see cref="Type"/> used in .NET corresponding to this data type.
    /// </summary>
    private Type _dotNetType;

    /// <summary>
    /// The full MySQL data type declaration as appears in a CREATE TABLE statement.
    /// </summary>
    private string _fullType;

    /// <summary>
    /// The full MySQL data type declaration where the type name is upper cased.
    /// </summary>
    private string _fullTypeSql;

    /// <summary>
    /// Flag indicating whether this data type is of binary nature.
    /// </summary>
    private bool? _isBinary;

    /// <summary>
    /// Flag indicating whether this data type is bit.
    /// </summary>
    private bool? _isBit;

    /// <summary>
    /// Flag indicating whether this data type is a BLOB.
    /// </summary>
    private bool? _isBlob;

    /// <summary>
    /// Flag indicating whether this data type can hold boolean values.
    /// </summary>
    private bool? _isBool;

    /// <summary>
    /// Flag indicating whether this data type is fixed or variable sized character-based.
    /// </summary>
    private bool? _isChar;

    /// <summary>
    /// Flag indicating whether this data type is a DATE.
    /// </summary>
    private bool? _isDate;

    /// <summary>
    /// Flag indicating whether this data type is used for dates.
    /// </summary>
    private bool? _isDateBased;

    /// <summary>
    /// Flag indicating whether this data type is a DATETIME or TIMESTAMP.
    /// </summary>
    private bool? _isDateTimeOrTimeStamp;

    /// <summary>
    /// Flag indicating whether this data type stores fixed-point decimal numbers.
    /// </summary>
    private bool? _isFixedPoint;

    /// <summary>
    /// Flag indicating whether this data type stores floating-point decimal numbers.
    /// </summary>
    private bool? _isFloatingPoint;

    /// <summary>
    /// Flag indicating whether this data type stores a geometry-based object.
    /// </summary>
    private bool? _isGeometry;

    /// <summary>
    /// Flag indicating whether this data type is integer-based.
    /// </summary>
    private bool? _isInteger;

    /// <summary>
    /// Flag indicating whether this data type is a JSON type.
    /// </summary>
    private bool? _isJson;

    /// <summary>
    /// Flag indicating whether this data type is a Set or Enumeration.
    /// </summary>
    private bool? _isSetOrEnum;

    /// <summary>
    /// Flag indicating whether this data type is fixed or variable sized character-based.
    /// </summary>
    private bool? _isText;

    /// <summary>
    /// Flag indicating whether this data type is Time.
    /// </summary>
    private bool? _isTime;

    /// <summary>
    /// Flag indicating whether this represents a valid MySQL data type.
    /// </summary>
    private bool? _isValid;

    /// <summary>
    /// Flag indicating whether this data type is a variable one like VARCHAR or VARBINARY.
    /// </summary>
    private bool? _isVariable;

    /// <summary>
    /// Flag indicating whether this data type is Year.
    /// </summary>
    private bool? _isYear;

    /// <summary>
    /// The length defined for this data type.
    /// </summary>
    private long _length;

    /// <summary>
    /// The maximum length this data type can hold.
    /// </summary>
    private long _maxLength;

    /// <summary>
    /// Flag indicating whether this data type is a tinyint(1) or bit or bit(1) and may hold boolean values.
    /// </summary>
    private bool? _mayBeBool;

    /// <summary>
    /// The <see cref="MySql.Data.MySqlClient.MySqlDbType"/> object corresponding to this column's data type.
    /// </summary>
    private MySqlDbType _mySqlDbType;

    /// <summary>
    /// A list of parameters specified in the <see cref="FullType"/>.
    /// </summary>
    private List<string> _parameters;

    /// <summary>
    /// Flag indicating whether real is translated to float or to double.
    /// </summary>
    private bool _treatRealAsFloat;

    /// <summary>
    /// The default value inherent to the MySQL data type, used when no default value is specified in <see cref="FullType"/>.
    /// </summary>
    private object _typeDefaultValue;

    /// <summary>
    /// The data type descriptor without any options wrapped by parenthesis.
    /// </summary>
    private string _typeName;

    /// <summary>
    /// Flag indicating whether numeric data is unsigned, meaningful only if the data type is integer-based.
    /// </summary>
    private bool? _unsigned;

    /// <summary>
    /// Flag indicating whether the type name is a valid MySQL one.
    /// </summary>
    private bool _validTypeName;

    /// <summary>
    /// Flag indicating whether numeric data is padded with zeros, meaningful only if the data type is integer-based.
    /// </summary>
    private bool? _zeroFill;

    #endregion Fields

    /// <summary>
    /// Initializes a new instance of the <see cref="MySqlDataType"/> class.
    /// </summary>
    /// <param name="fullType">The full MySQL data type declaration as appears in a CREATE TABLE statement.</param>
    /// <param name="isValid">Flag indicating whether this represents a valid MySQL data type.</param>
    /// <param name="datesAsMySqlDates">Flag indicating whether the <see cref="Type"/> used for dates will be stored as <see cref="MySql.Data.Types.MySqlDateTime"/> or as <see cref="DateTime"/>.</param>
    public MySqlDataType(string fullType, bool isValid, bool datesAsMySqlDates = true)
      : this()
    {
      DatesAsMySqlDates = datesAsMySqlDates;
      if (string.IsNullOrEmpty(fullType))
      {
        _dotNetType = Type.GetType("System.String");
        _mySqlDbType = MySqlDbType.VarChar;
        _typeName = DEFAULT_DATA_TYPE;
        _validTypeName = true;
        _length = DEFAULT_DATA_TYPE_LENGTH;
        _fullType = string.Format("{0}({1})", _typeName, _length);
        _fullTypeSql = _fullType.ToUpperInvariant();
        _isValid = true;
        _parameters = new List<string> { _length.ToString(CultureInfo.InvariantCulture) };
      }
      else
      {
        _fullType = fullType;
        if (!isValid)
        {
          _fullType = GetFullTypeFromComponents(false);
          return;
        }

        _validTypeName = true;
        _isValid = true;
      }
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="MySqlDataType"/> class.
    /// </summary>
    private MySqlDataType()
    {
      _treatRealAsFloat = false;
      ResetFields();
      DatesAsMySqlDates = false;
      InvalidSetOrEnumElementsIndexes = null;
    }

    #region Properties

    /// <summary>
    /// Gets an array of attributes specified in the <see cref="FullType"/>.
    /// </summary>
    public List<string> Attributes
    {
      get
      {
        return _attributes ?? (_attributes = GetAttributes());
      }
    }

    /// <summary>
    /// Gets a value indicating whether the <see cref="Type"/> used for dates will be stored as <see cref="MySql.Data.Types.MySqlDateTime"/> or as <see cref="DateTime"/>.
    /// </summary>
    public bool DatesAsMySqlDates { get; private set; }

    /// <summary>
    /// Gets or sets the number of places after the decimal point for floating point data types.
    /// </summary>
    public int DecimalPlaces
    {
      get
      {
        if (_decimalPlaces < 0)
        {
          if (IsDecimal && Parameters != null && _parameters.Count > 1)
          {
            int.TryParse(_parameters[1], out _decimalPlaces);
          }
          else
          {
            _decimalPlaces = 0;
          }
        }

        return _decimalPlaces < 0 ? 0 : _decimalPlaces;
      }

      set
      {
        if (!IsDecimal)
        {
          return;
        }

        _decimalPlaces = value;
        ResetFullType(false ,true);
      }
    }

    /// <summary>
    /// Gets the <see cref="Type"/> used in .NET corresponding to this data type.
    /// </summary>
    public Type DotNetType
    {
      get
      {
        return _dotNetType ?? (_dotNetType = GetDotNetType());
      }
    }

    /// <summary>
    /// Gets the full MySQL data type declaration as appears in a CREATE TABLE statement.
    /// </summary>
    public string FullType
    {
      get
      {
        if (string.IsNullOrEmpty(_fullType))
        {
          _fullType = GetFullTypeFromComponents(false);
        }

        return _fullType;
      }
    }

    /// <summary>
    /// Gets the full MySQL data type declaration where the type name is upper cased.
    /// </summary>
    public string FullTypeSql
    {
      get
      {
        if (string.IsNullOrEmpty(_fullTypeSql))
        {
          _fullTypeSql = GetFullTypeFromComponents(true);
        }

        return _fullTypeSql;
      }
    }

    /// <summary>
    /// Gets a list of indexes of elements of a SET or ENUM declaration that are improperly quoted.
    /// </summary>
    public int[] InvalidSetOrEnumElementsIndexes { get; private set; }

    /// <summary>
    /// Gets a value indicating whether this data type is of binary nature.
    /// </summary>
    public bool IsBinary
    {
      get
      {
        if (_isBinary == null)
        {
          _isBinary = !string.IsNullOrEmpty(TypeName) && _typeName.Contains("binary", StringComparison.InvariantCultureIgnoreCase);
        }

        return (bool)_isBinary;
      }
    }

    /// <summary>
    /// Gets a value indicating whether this data type is bit.
    /// </summary>
    public bool IsBit
    {
      get
      {
        if (_isBit == null)
        {
          _isBit = !string.IsNullOrEmpty(TypeName) && _typeName.Equals("bit", StringComparison.InvariantCultureIgnoreCase);
        }

        return (bool)_isBit;
      }
    }

    /// <summary>
    /// Gets a value indicating whether this data type is a BLOB.
    /// </summary>
    public bool IsBlob
    {
      get
      {
        if (_isBlob == null)
        {
          _isBlob = !string.IsNullOrEmpty(TypeName) && _typeName.EndsWith("blob", StringComparison.InvariantCultureIgnoreCase);
        }

        return (bool)_isBlob;
      }
    }

    /// <summary>
    /// Gets a value indicating whether this data type can hold boolean values.
    /// </summary>
    public bool IsBool
    {
      get
      {
        if (_isBool == null)
        {
          _isBool = !string.IsNullOrEmpty(TypeName)
                    && !string.IsNullOrEmpty(_fullType)
                    && _typeName.StartsWith("bool", StringComparison.InvariantCultureIgnoreCase);
        }

        return (bool)_isBool;
      }
    }

    /// <summary>
    /// Gets a value indicating whether this data type is fixed or variable sized character-based.
    /// </summary>
    public bool IsChar
    {
      get
      {
        if (_isChar == null)
        {
          _isChar = !string.IsNullOrEmpty(TypeName) && _typeName.Contains("char", StringComparison.InvariantCultureIgnoreCase);
        }

        return (bool)_isChar;
      }
    }

    /// <summary>
    /// Gets a value indicating whether this data type is a DATE.
    /// </summary>
    public bool IsDate
    {
      get
      {
        if (_isDate == null)
        {
          _isDate = !string.IsNullOrEmpty(TypeName) && _typeName.Equals("date", StringComparison.InvariantCultureIgnoreCase);
        }

        return (bool)_isDate;
      }
    }

    /// <summary>
    /// Gets a value indicating whether this data type is used for dates.
    /// </summary>
    public bool IsDateBased
    {
      get
      {
        if (_isDateBased == null)
        {
          _isDateBased = IsDateTimeOrTimeStamp || IsDate;
        }

        return (bool)_isDateBased;
      }
    }

    /// <summary>
    /// Gets a value indicating whether this data type is a DATETIME or TIMESTAMP.
    /// </summary>
    public bool IsDateTimeOrTimeStamp
    {
      get
      {
        if (_isDateTimeOrTimeStamp == null)
        {
          _isDateTimeOrTimeStamp = !string.IsNullOrEmpty(TypeName)
                                    && (_typeName.Equals("datetime", StringComparison.InvariantCultureIgnoreCase)
                                        || _typeName.Equals("timestamp", StringComparison.InvariantCultureIgnoreCase));
        }

        return (bool)_isDateTimeOrTimeStamp;
      }
    }

    /// <summary>
    /// Gets a value indicating whether this data type is of floating-point nature.
    /// </summary>
    public bool IsDecimal
    {
      get
      {
        return IsFixedPoint || IsFloatingPoint;
      }
    }

    /// <summary>
    /// Gets a value indicating whether this data type stores fixed-point decimal numbers.
    /// </summary>
    public bool IsFixedPoint
    {
      get
      {
        if (_isFixedPoint == null)
        {
          _isFixedPoint = !string.IsNullOrEmpty(TypeName)
                            && (_typeName.Equals("decimal", StringComparison.InvariantCultureIgnoreCase)
                                || _typeName.Equals("numeric", StringComparison.InvariantCultureIgnoreCase));
        }

        return (bool)_isFixedPoint;
      }
    }

    /// <summary>
    /// Gets a value indicating whether this data type stores floating-point decimal numbers.
    /// </summary>
    public bool IsFloatingPoint
    {
      get
      {
        if (_isFloatingPoint == null)
        {
          _isFloatingPoint = !string.IsNullOrEmpty(TypeName)
                                && (_typeName.Equals("real", StringComparison.InvariantCultureIgnoreCase)
                                    || _typeName.StartsWith("double", StringComparison.InvariantCultureIgnoreCase)
                                    || _typeName.Equals("float", StringComparison.InvariantCultureIgnoreCase));
        }

        return (bool)_isFloatingPoint;
      }
    }

    /// <summary>
    /// Gets a value indicating whether this data type stores a geometry-based object.
    /// </summary>
    public bool IsGeometry
    {
      get
      {
        if (_isGeometry == null)
        {
          _isGeometry = !string.IsNullOrEmpty(TypeName)
                        && (_typeName.Contains("curve", StringComparison.InvariantCultureIgnoreCase)
                            || _typeName.Contains("geometry", StringComparison.InvariantCultureIgnoreCase)
                            || _typeName.Contains("line", StringComparison.InvariantCultureIgnoreCase)
                            || _typeName.Contains("point", StringComparison.InvariantCultureIgnoreCase)
                            || _typeName.Contains("polygon", StringComparison.InvariantCultureIgnoreCase)
                            || _typeName.Contains("surface", StringComparison.InvariantCultureIgnoreCase));
        }

        return (bool)_isGeometry;
      }
    }

    /// <summary>
    /// Gets a value indicating whether this data type is integer-based.
    /// </summary>
    public bool IsInteger
    {
      get
      {
        if (_isInteger == null)
        {
          _isInteger = !string.IsNullOrEmpty(TypeName) && _typeName.Contains("int", StringComparison.InvariantCultureIgnoreCase);
        }

        return (bool)_isInteger;
      }
    }

    /// <summary>
    /// Gets a value indicating whether this data type is a JSON type.
    /// </summary>
    public bool IsJson
    {
      get
      {
        if (_isJson == null)
        {
          _isJson = !string.IsNullOrEmpty(TypeName) && _typeName.Equals("json", StringComparison.InvariantCultureIgnoreCase);
        }

        return (bool)_isJson;
      }
    }

    /// <summary>
    /// Gets a value indicating whether this data type is numeric.
    /// </summary>
    public bool IsNumeric
    {
      get
      {
        return IsDecimal || IsInteger;
      }
    }

    /// <summary>
    /// Gets a value indicating whether this data type is a Set or Enumeration.
    /// </summary>
    public bool IsSetOrEnum
    {
      get
      {
        if (_isSetOrEnum == null)
        {
          _isSetOrEnum = !string.IsNullOrEmpty(TypeName)
                          && (_typeName.Equals("set", StringComparison.InvariantCultureIgnoreCase)
                              || _typeName.Equals("enum", StringComparison.InvariantCultureIgnoreCase));
        }

        return (bool)_isSetOrEnum;
      }
    }

    /// <summary>
    /// Gets a value indicating whether this data type is fixed or variable sized character-based.
    /// </summary>
    public bool IsText
    {
      get
      {
        if (_isText == null)
        {
          _isText = !string.IsNullOrEmpty(TypeName) && _typeName.Contains("text", StringComparison.InvariantCultureIgnoreCase);
        }

        return (bool)_isText;
      }
    }

    /// <summary>
    /// Gets a value indicating whether this data type is Time.
    /// </summary>
    public bool IsTime
    {
      get
      {
        if (_isTime == null)
        {
          _isTime = !string.IsNullOrEmpty(TypeName) && _typeName.Equals("time", StringComparison.InvariantCultureIgnoreCase);
        }

        return (bool)_isTime;
      }
    }

    /// <summary>
    /// Gets a value indicating whether this represents a valid MySQL data type.
    /// </summary>
    public bool IsValid
    {
      get
      {
        if (_isValid == null)
        {
          _isValid = MySqlDisplayDataType.ValidateTypeName(TypeName) && ValidateAttributes() && ValidateParameters();
        }

        return (bool)_isValid;
      }
    }

    /// <summary>
    /// Gets a value indicating whether this data type is a variable one like VARCHAR or VARBINARY.
    /// </summary>
    public bool IsVariable
    {
      get
      {
        if (_isVariable == null)
        {
          _isVariable = !string.IsNullOrEmpty(TypeName) && _typeName.StartsWith("var", StringComparison.InvariantCultureIgnoreCase);
        }

        return (bool)_isVariable;
      }
    }

    /// <summary>
    /// Gets a value indicating whether this data type is Year.
    /// </summary>
    public bool IsYear
    {
      get
      {
        if (_isYear == null)
        {
          _isYear = !string.IsNullOrEmpty(TypeName) && _typeName.Equals("year", StringComparison.InvariantCultureIgnoreCase);
        }

        return (bool)_isYear;
      }
    }

    /// <summary>
    /// Gets or sets the length defined for this data type.
    /// </summary>
    public long Length
    {
      get
      {
        if (_length < 0)
        {
          if (RequiresParameters && !IsSetOrEnum && Parameters != null && _parameters.Count > 0)
          {
            long.TryParse(_parameters[0], out _length);
          }
          else
          {
            _length = 0;
          }
        }

        return _length < 0 ? 0 : _length;
      }

      set
      {
        if (!RequiresParameters || IsSetOrEnum)
        {
          return;
        }

        _length = value;
        ResetFullType(false, true);
      }
    }

    /// <summary>
    /// Gets the maximum length this data type can hold.
    /// </summary>
    public long MaxLength
    {
      get
      {
        if (_maxLength < 0)
        {
          _maxLength = GetMaxLength();
        }

        return _maxLength;
      }
    }

    /// <summary>
    /// Gets a value indicating whether this data type is a tinyint(1) or bit or bit(1) and may hold boolean values.
    /// </summary>
    public bool MayBeBool
    {
      get
      {
        if (_mayBeBool == null)
        {
          _mayBeBool = !string.IsNullOrEmpty(TypeName)
                    && !string.IsNullOrEmpty(_fullType)
                    && ((_typeName.Equals("bit", StringComparison.InvariantCultureIgnoreCase) && (Length == 0 || Length == 1))
                        || (_typeName.Equals("tinyint", StringComparison.InvariantCultureIgnoreCase) && Length == 1));
        }

        return (bool)_mayBeBool;
      }
    }

    /// <summary>
    /// Gets a <see cref="MySql.Data.MySqlClient.MySqlDbType"/> object corresponding to this column's data type.
    /// </summary>
    public MySqlDbType MySqlDbType
    {
      get
      {
        if (_mySqlDbType == MySqlDbType.Guid)
        {
          SetDbTypeAndItsDefaultValue();
        }

        return _mySqlDbType;
      }
    }

    /// <summary>
    /// Gets list of parameters specified in the <see cref="FullType"/>.
    /// </summary>
    public List<string> Parameters
    {
      get
      {
        return _parameters ?? (_parameters = GetParameters());
      }
    }

    /// <summary>
    /// Gets a value indicating whether the data type requires parameters in parenthesis.
    /// </summary>
    public bool RequiresParameters
    {
      get
      {
        return IsChar || IsNumeric || IsBit || IsYear || IsBinary;
      }
    }

    /// <summary>
    /// Gets a value indicating whether a value for the given <see cref="MySqlDbType"/> must be wrapped in quotes when assembling a SQL query.
    /// </summary>
    public bool RequiresQuotesForValue
    {
      get
      {
        return IsChar || IsText || IsSetOrEnum || IsDateBased || IsJson;
      }
    }

    /// <summary>
    /// Gets a list of elements included in a SET or ENUM declaration.
    /// </summary>
    public List<string> SetOrEnumElements
    {
      get
      {
        return IsSetOrEnum ? Parameters : null;
      }
    }

    /// <summary>
    /// Gets or sets a value indicating whether real is translated to float or to double.
    /// </summary>
    public bool TreatRealAsFloat
    {
      get
      {
        return _treatRealAsFloat;
      }

      set
      {
        bool oldValue = _treatRealAsFloat;
        _treatRealAsFloat = value;
        if (_treatRealAsFloat == oldValue) return;
        _maxLength = -1;
        _mySqlDbType = MySqlDbType.Guid;
      }
    }

    /// <summary>
    /// Gets the default value inherent to the MySQL data type, used when no default value is specified in <see cref="FullType"/>.
    /// </summary>
    public object TypeDefaultValue
    {
      get
      {
        if (_typeDefaultValue == null)
        {
          SetDbTypeAndItsDefaultValue();
        }

        return _typeDefaultValue;
      }
    }

    /// <summary>
    /// Gets the data type descriptor without any options wrapped by parenthesis.
    /// </summary>
    public string TypeName
    {
      get
      {
        if (string.IsNullOrEmpty(_typeName))
        {
          _typeName = GetTypeName();
        }

        return _typeName;
      }
    }

    /// <summary>
    /// Gets or sets value indicating whether numeric data is unsigned, meaningful only if the data type is integer-based.
    /// </summary>
    public bool Unsigned
    {
      get
      {
        if (_unsigned == null)
        {
          _unsigned = ZeroFill || (IsNumeric && Attributes.Contains(ATTRIBUTE_UNSIGNED));
        }

        return (bool)_unsigned;
      }

      set
      {
        _unsigned = value;
        ResetFullType(true, false);
      }
    }

    /// <summary>
    /// Gets or sets value indicating whether numeric data is padded with zeros, meaningful only if the data type is integer-based.
    /// </summary>
    public bool ZeroFill
    {
      get
      {
        if (_zeroFill == null)
        {
          _zeroFill = IsNumeric && Attributes.Contains(ATTRIBUTE_ZEROFILL);
        }

        return (bool)_zeroFill;
      }

      set
      {
        _zeroFill = value;
        ResetFullType(true, false);
      }
    }

    #endregion Properties

    /// <summary>
    /// Gets the best match for the <see cref="MySqlDataType"/> to be used for a given raw value exported to a MySQL table.
    /// </summary>
    /// <param name="packedValue">Raw value to export</param>
    /// <param name="valueOverflow">Output flag indicating whether the value would still overflow the proposed data type.</param>
    /// <param name="datesAsMySqlDates">Flag indicating whether the <see cref="Type"/> used for dates will be stored as <see cref="MySql.Data.Types.MySqlDateTime"/> or as <see cref="DateTime"/>.</param>
    /// <returns>The best match for the <see cref="MySqlDataType"/> to be used for the given raw value.</returns>
    public static MySqlDataType DetectDataType(object packedValue, out bool valueOverflow, bool datesAsMySqlDates = true)
    {
      valueOverflow = false;
      if (packedValue == null)
      {
        return null;
      }

      Type objUnpackedType = packedValue.GetType();
      string fullType = null;
      string strType = objUnpackedType.FullName;
      string strValue = packedValue.ToString();
      int strLength = strValue.Length;
      int decimalPointPos = strValue.IndexOf(".", StringComparison.Ordinal);
      int[] varCharApproxLen = { 5, 12, 25, 45, 255, MYSQL_VARCHAR_MAX_PROPOSED_LEN };
      int[,] decimalApproxLen = { { 12, 2 }, { 65, 30 } };

      if (strType == "System.Double")
      {
        if (decimalPointPos < 0)
        {
          int intResult;
          if (Int32.TryParse(strValue, out intResult))
          {
            strType = "System.Int32";
          }
          else
          {
            long longResult;
            if (Int64.TryParse(strValue, out longResult))
            {
              strType = "System.Int64";
            }
          }
        }
        else
        {
          strType = "System.Decimal";
        }
      }

      strValue = strValue.ToLowerInvariant();
      if (strType == "System.String")
      {
        if (strValue == "yes" || strValue == "no" || strValue == "ja" || strValue == "nein")
        {
          strType = "System.Boolean";
        }
        else if (strValue.IsMySqlZeroDateTimeValue())
        {
          strType = "MySql.Data.Types.MySqlDateTime";
        }
      }

      switch (strType)
      {
        case "System.String":
          foreach (int t in varCharApproxLen.Where(t => strLength <= t))
          {
            fullType = string.Format("VarChar({0})", t);
            break;
          }

          if (string.IsNullOrEmpty(fullType))
          {
            fullType = "Text";
          }
          break;

        case "System.Double":
          fullType = "Double";
          break;

        case "System.Decimal":
        case "System.Single":
          int intLen = decimalPointPos;
          int fractLen = strLength - intLen - 1;
          if (intLen <= decimalApproxLen[0, 0] && fractLen <= decimalApproxLen[0, 1])
          {
            fullType = "Decimal(12,2)";
          }
          else if (intLen <= decimalApproxLen[1, 0] && fractLen <= decimalApproxLen[1, 1])
          {
            fullType = "Decimal(65,30)";
          }
          else
          {
            valueOverflow = true;
            fullType = "Double";
          }
          break;

        case "System.Byte":
        case "System.UInt16":
        case "System.Int16":
        case "System.UInt32":
        case "System.Int32":
          fullType = "Integer";
          break;

        case "System.UInt64":
        case "System.Int64":
          fullType = "BigInt";
          break;

        case "System.Boolean":
          fullType = "Bool";
          break;

        case "System.DateTime":
        case "MySql.Data.Types.MySqlDateTime":
          fullType = strValue.Contains(":") ? "DateTime" : "Date";
          break;

        case "System.TimeSpan":
          fullType = "Time";
          break;
      }

      return fullType != null
        ? new MySqlDataType(fullType, true, datesAsMySqlDates)
        : null;
    }

    /// <summary>
    /// Gets the <see cref="MySqlDbType"/> corresponding to the <see cref="FullType"/>.
    /// </summary>
    /// <param name="mySqlDbType">A <see cref="MySqlDbType"/>.</param>
    /// <param name="datesAsMySqlDates">Flag indicating whether the <see cref="Type"/> used for dates will be stored as <see cref="MySql.Data.Types.MySqlDateTime"/> or as <see cref="DateTime"/>.</param>
    /// <returns>The <see cref="MySqlDbType"/> corresponding to the <see cref="FullType"/>.</returns>
    public static MySqlDataType FromMySqlDbType(MySqlDbType mySqlDbType, bool datesAsMySqlDates = true)
    {
      switch (mySqlDbType)
      {
        case MySqlDbType.Bit:
          return new MySqlDataType("Bit", true, datesAsMySqlDates);

        case MySqlDbType.Int32:
        case MySqlDbType.UInt32:
          return new MySqlDataType(string.Format("Integer{0}", mySqlDbType == MySqlDbType.UInt32 ? " " + ATTRIBUTE_UNSIGNED : string.Empty), true, datesAsMySqlDates);

        case MySqlDbType.Byte:
        case MySqlDbType.UByte:
          return new MySqlDataType(string.Format("TinyInt{0}", mySqlDbType == MySqlDbType.UByte ? " " + ATTRIBUTE_UNSIGNED : string.Empty), true, datesAsMySqlDates);

        case MySqlDbType.Int16:
        case MySqlDbType.UInt16:
          return new MySqlDataType(string.Format("SmallInt{0}", mySqlDbType == MySqlDbType.UInt16 ? " " + ATTRIBUTE_UNSIGNED : string.Empty), true, datesAsMySqlDates);

        case MySqlDbType.Int24:
        case MySqlDbType.UInt24:
          return new MySqlDataType(string.Format("MediumInt{0}", mySqlDbType == MySqlDbType.UInt24 ? " " + ATTRIBUTE_UNSIGNED : string.Empty), true, datesAsMySqlDates);

        case MySqlDbType.Int64:
        case MySqlDbType.UInt64:
          return new MySqlDataType(string.Format("BigInt{0}", mySqlDbType == MySqlDbType.UInt64 ? " " + ATTRIBUTE_UNSIGNED : string.Empty), true, datesAsMySqlDates);

        case MySqlDbType.Float:
          return new MySqlDataType("Float", true, datesAsMySqlDates);

        case MySqlDbType.Double:
          return new MySqlDataType("Double", true, datesAsMySqlDates);

        case MySqlDbType.Decimal:
          return new MySqlDataType("Decimal", true, datesAsMySqlDates);

        case MySqlDbType.VarChar:
          return new MySqlDataType(string.Format("VarChar({0})", DEFAULT_DATA_TYPE_LENGTH), true, datesAsMySqlDates);

        case MySqlDbType.Binary:
          return new MySqlDataType("Binary", true, datesAsMySqlDates);

        case MySqlDbType.VarBinary:
          return new MySqlDataType(string.Format("VarBinary({0})", DEFAULT_DATA_TYPE_LENGTH), true, datesAsMySqlDates);

        case MySqlDbType.Set:
          return new MySqlDataType("Set('')", true, datesAsMySqlDates);

        case MySqlDbType.Enum:
          return new MySqlDataType("Enum('')", true, datesAsMySqlDates);

        case MySqlDbType.Blob:
          return new MySqlDataType("Blob", true, datesAsMySqlDates);

        case MySqlDbType.Text:
          return new MySqlDataType("Text", true, datesAsMySqlDates);

        case MySqlDbType.LongBlob:
          return new MySqlDataType("LongBlob", true, datesAsMySqlDates);

        case MySqlDbType.LongText:
          return new MySqlDataType("LongText", true, datesAsMySqlDates);

        case MySqlDbType.MediumBlob:
          return new MySqlDataType("MediumBlob", true, datesAsMySqlDates);

        case MySqlDbType.MediumText:
          return new MySqlDataType("MediumText", true, datesAsMySqlDates);

        case MySqlDbType.TinyBlob:
          return new MySqlDataType("TinyBlob", true, datesAsMySqlDates);

        case MySqlDbType.TinyText:
          return new MySqlDataType("TinyText", true, datesAsMySqlDates);

        case MySqlDbType.Date:
          return new MySqlDataType("Date", true, datesAsMySqlDates);

        case MySqlDbType.DateTime:
          return new MySqlDataType("DateTime", true, datesAsMySqlDates);

        case MySqlDbType.Timestamp:
          return new MySqlDataType("Timestamp", true, datesAsMySqlDates);

        case MySqlDbType.Time:
          return new MySqlDataType("Time", true, datesAsMySqlDates);

        case MySqlDbType.Year:
          return new MySqlDataType("Year", true, datesAsMySqlDates);

        case MySqlDbType.JSON:
          return new MySqlDataType("JSON", true, datesAsMySqlDates);

        case MySqlDbType.Geometry:
          return new MySqlDataType("Geometry", true, datesAsMySqlDates);
      }

      return null;
    }

    /// <summary>
    /// Gets the matching MySQL data type from unboxing a packed value.
    /// </summary>
    /// <param name="packedValue">The packed value.</param>
    /// <returns>The matching MySQL data type.</returns>
    public static string GetMySqlDataType(object packedValue)
    {
      if (packedValue == null)
      {
        return string.Empty;
      }

      Type objUnpackedType = packedValue.GetType();
      int strLength = packedValue.ToString().Length;
      strLength = strLength + (10 - strLength % 10);
      return GetMySqlDataType(objUnpackedType, strLength);
    }

    /// <summary>
    /// Gets the matching MySQL data type from unboxing a packed value.
    /// </summary>
    /// <param name="dotNetType">A valid .NET data type.</param>
    /// <param name="strLength">In case of a string type, the lenght of the string data.</param>
    /// <returns>The matching MySQL data type.</returns>
    public static string GetMySqlDataType(Type dotNetType, int strLength = 0)
    {
      string retType = string.Empty;
      if (dotNetType == null)
      {
        return retType;
      }

      string strType = dotNetType.FullName;
      bool unsigned = strType.Contains(".U");

      switch (strType)
      {
        case "System.String":
          retType = strLength > MYSQL_VARCHAR_MAX_PROPOSED_LEN ? "text" : "varchar";
          break;

        case "System.Byte":
          retType = "tinyint";
          break;

        case "System.UInt16":
        case "System.Int16":
          retType = string.Format("smallint{0}", unsigned ? " unsigned" : string.Empty);
          break;

        case "System.UInt32":
        case "System.Int32":
          retType = string.Format("int{0}", unsigned ? " unsigned" : string.Empty);
          break;

        case "System.UInt64":
        case "System.Int64":
          retType = string.Format("bigint{0}", unsigned ? " unsigned" : string.Empty);
          break;

        case "System.Decimal":
          retType = "decimal";
          break;

        case "System.Single":
          retType = "float";
          break;

        case "System.Double":
          retType = "double";
          break;

        case "System.Boolean":
          retType = "bit";
          break;

        case "System.DateTime":
        case "MySql.Data.Types.MySqlDateTime":
          retType = "datetime";
          break;

        case "System.TimeSpan":
          retType = "time";
          break;

        case "System.Guid":
          retType = "binary(16)";
          break;
      }

      return retType;
    }

    /// <summary>
    /// Gets the string representation for a numerical value boxed in an object.
    /// </summary>
    /// <param name="boxedValue">Boxed numerical value.</param>
    /// <returns>String representation of the given boxed number.</returns>
    public static string GetStringRepresentationForNumericObject(object boxedValue)
    {
      return GetStringRepresentationForNumericObject(boxedValue, CultureInfo.InvariantCulture);
    }

    /// <summary>
    /// Gets the string representation for a numerical value boxed in an object.
    /// </summary>
    /// <param name="boxedValue">Boxed numerical value.</param>
    /// <param name="ci">Locale used to convert the number to a string.</param>
    /// <returns>String representation of the given boxed number.</returns>
    public static string GetStringRepresentationForNumericObject(object boxedValue, CultureInfo ci)
    {
      if (boxedValue is sbyte)
      {
        return ((sbyte)boxedValue).ToString("G", CultureInfo.InvariantCulture);
      }

      if (boxedValue is byte)
      {
        return ((byte)boxedValue).ToString("G", CultureInfo.InvariantCulture);
      }

      if (boxedValue is short)
      {
        return ((short)boxedValue).ToString("G", CultureInfo.InvariantCulture);
      }

      if (boxedValue is ushort)
      {
        return ((ushort)boxedValue).ToString("G", CultureInfo.InvariantCulture);
      }

      if (boxedValue is int)
      {
        return ((int)boxedValue).ToString("G", CultureInfo.InvariantCulture);
      }

      if (boxedValue is uint)
      {
        return ((uint)boxedValue).ToString("G", CultureInfo.InvariantCulture);
      }

      if (boxedValue is long)
      {
        return ((long)boxedValue).ToString("G", CultureInfo.InvariantCulture);
      }

      if (boxedValue is ulong)
      {
        return ((ulong)boxedValue).ToString("G", CultureInfo.InvariantCulture);
      }

      if (boxedValue is float)
      {
        return ((float)boxedValue).ToString("G", CultureInfo.InvariantCulture);
      }

      if (boxedValue is double)
      {
        return ((double)boxedValue).ToString("G", CultureInfo.InvariantCulture);
      }

      if (boxedValue is decimal)
      {
        return ((decimal)boxedValue).ToString("G", CultureInfo.InvariantCulture);
      }

      return boxedValue.ToString();
    }

    /// <summary>
    /// Gets a boxed <see cref="bool"/> value from .
    /// </summary>
    /// <param name="rawValue">An object.</param>
    /// <returns>A boxed <see cref="DateTime"/> object where its data is converted to a proper date value if it is of date origin, or the same object if not.</returns>
    public static object GetValueAsBoolean(object rawValue)
    {
      if (rawValue.IsEmptyValue())
      {
        return false;
      }

      if (rawValue is bool)
      {
        return rawValue;
      }

      var rawValueAsString = rawValue.ToString().ToLowerInvariant();
      switch (rawValueAsString)
      {
        case "1":
        case "true":
        case "yes":
        case "ja":
          return true;

        case "0":
        case "false":
        case "no":
        case "nein":
          return false;

        default:
          throw new ValueNotSuitableForConversionException(rawValueAsString, "bool");
      }
    }

    /// <summary>
    /// Gets a boxed <see cref="DateTime"/> object where its data is converted to a proper date value if it is of date origin, or the same object if not.
    /// </summary>
    /// <param name="rawValue">An object.</param>
    /// <returns>A boxed <see cref="DateTime"/> object where its data is converted to a proper date value if it is of date origin, or the same object if not..</returns>
    public static object GetValueAsDateTime(object rawValue)
    {
      if (rawValue.IsEmptyValue())
      {
        return null;
      }

      if (rawValue is DateTime)
      {
        var dtValue = (DateTime)rawValue;
        if (dtValue.CompareTo(DateTime.MinValue) == 0 || dtValue.CompareTo(DateTime.FromOADate(0)) == 0)
        {
          return null;
        }

        return dtValue;
      }

      if (rawValue is MySqlDateTime)
      {
        var mysqlDate = (MySqlDateTime)rawValue;
        if (!mysqlDate.IsValidDateTime)
        {
          return null;
        }

        return GetValueAsDateTime(mysqlDate.GetDateTime());
      }

      if (rawValue is string)
      {
        var rawValueAsString = rawValue.ToString();
        DateTime dtValue;
        if (DateTime.TryParse(rawValueAsString, out dtValue))
        {
          return GetValueAsDateTime(dtValue);
        }

        if (rawValueAsString.IsMySqlZeroDateTimeValue(true))
        {
          return null;
        }
      }

      throw new ValueNotSuitableForConversionException(rawValue.ToString(), "DateTime");
    }

    /// <summary>
    /// Checks if the given string value can be parsed into a <see cref="MySqlDateTime"/> object.
    /// </summary>
    /// <param name="dateValueAsString">The string value representing a date.</param>
    /// <param name="isZeroDateTime"></param>
    /// <returns><c>true</c> if the given string value can be parsed into a <see cref="MySqlDateTime"/> object, <c>false</c> otherwise.</returns>
    public static bool IsMySqlDateTimeValue(string dateValueAsString, out bool isZeroDateTime)
    {
      isZeroDateTime = false;
      if (string.IsNullOrEmpty(dateValueAsString))
      {
        return false;
      }

      // Step 1: Attempt to parse the string value into a regular DateTime, if it can be parsed then it can be stored in a MySqlDateTime, so return true.
      DateTime parsedDateTime;
      bool canBeParsedByDateTime = DateTime.TryParse(dateValueAsString, out parsedDateTime);
      if (canBeParsedByDateTime)
      {
        return true;
      }

      // Step 2: Convert all 0s into 1s and see if that can be parsed into a regular DateTime, if it can't be parsed it can't be stored in a MySqlDateTime, so return false.
      canBeParsedByDateTime = DateTime.TryParse(dateValueAsString.Replace("0", "1"), out parsedDateTime);
      if (!canBeParsedByDateTime)
      {
        return false;
      }

      bool isMySqlDateTimeValue;
      try
      {
        // Step 3: Convert back the 1s into 0s and store them in individual date/time components.
        int year = int.Parse(parsedDateTime.Year.ToString(CultureInfo.InvariantCulture).Replace("1", "0"));
        int month = int.Parse(parsedDateTime.Month.ToString(CultureInfo.InvariantCulture).Replace("1", "0"));
        int day = int.Parse(parsedDateTime.Month.ToString(CultureInfo.InvariantCulture).Replace("1", "0"));
        int hour = int.Parse(parsedDateTime.Hour.ToString(CultureInfo.InvariantCulture).Replace("1", "0"));
        int minute = int.Parse(parsedDateTime.Minute.ToString(CultureInfo.InvariantCulture).Replace("1", "0"));
        int second = int.Parse(parsedDateTime.Second.ToString(CultureInfo.InvariantCulture).Replace("1", "0"));
        int millisecond = int.Parse(parsedDateTime.Millisecond.ToString(CultureInfo.InvariantCulture).Replace("1", "0"));

        // Step 4: Create a new MySqlDateTime struct with the date/time components.
        var mySqlDateObject = new MySqlDateTime(year, month, day, hour, minute, second, millisecond);
        isMySqlDateTimeValue = true;
        isZeroDateTime = !mySqlDateObject.IsValidDateTime;
      }
      catch (Exception)
      {
        isMySqlDateTimeValue = false;
      }

      return isMySqlDateTimeValue;
    }

    /// <summary>
    /// Checks whether a given string value can be stored using this data type.
    /// </summary>
    /// <param name="strValue">The value as a string representation to store in this column.</param>
    /// <returns><c>true</c> if the string value can be stored using this data type, <c>false</c> otherwise.</returns>
    public bool CanStoreValue(string strValue)
    {
      // If the value is null, treat it as an empty string.
      if (strValue == null)
      {
        strValue = string.Empty;
      }

      // Return immediately for big data types.
      if (IsText || IsJson || IsBlob || IsBinary)
      {
        return true;
      }

      // Return immediately for spatial data types since values for them can be created in a wide variety of ways
      // (using WKT, WKB or MySQL spatial functions that return spatial objects), so leave the validation to the MySQL Server.
      if (IsGeometry)
      {
        return true;
      }

      // Check for boolean
      if (IsBool)
      {
        return strValue.IsBooleanValue();
      }

      // Check for tinyint(1), bit and bit(1), which may be boolean
      if (MayBeBool && strValue.IsBooleanValue())
      {
        return true;
      }

      // Check for integer values
      var lowerTypeName = TypeName.ToLowerInvariant();
      if (lowerTypeName == "int" || lowerTypeName == "integer" || lowerTypeName == "mediumint")
      {
        int tryIntValue;
        return int.TryParse(strValue, out tryIntValue);
      }

      if (IsYear)
      {
        int parsedYearValue;
        return string.IsNullOrEmpty(strValue) || (int.TryParse(strValue, out parsedYearValue) && (Length == 2 && parsedYearValue >= 0 && parsedYearValue < 100) || (parsedYearValue > 1900 && parsedYearValue < 2156));
      }

      if (lowerTypeName == "tinyint")
      {
        byte tryByteValue;
        return byte.TryParse(strValue, out tryByteValue);
      }

      if (lowerTypeName == "smallint")
      {
        short trySmallIntValue;
        return short.TryParse(strValue, out trySmallIntValue);
      }

      if (lowerTypeName == "bigint")
      {
        long tryBigIntValue;
        return long.TryParse(strValue, out tryBigIntValue);
      }

      if (lowerTypeName == "bit")
      {
        ulong tryBitValue;
        return ulong.TryParse(strValue, out tryBitValue);
      }

      // Check for big numeric values
      if (lowerTypeName.StartsWith("float"))
      {
        float tryFloatValue;
        return float.TryParse(strValue, out tryFloatValue);
      }

      if (lowerTypeName.StartsWith("double") || lowerTypeName == "real")
      {
        double tryDoubleValue;
        return double.TryParse(strValue, out tryDoubleValue);
      }

      // Check for date and time values.
      if (IsTime)
      {
        TimeSpan tryTimeSpanValue;
        return TimeSpan.TryParse(strValue, out tryTimeSpanValue);
      }

      if (IsDateBased)
      {
        if (strValue.IsMySqlZeroDateTimeValue())
        {
          return true;
        }

        DateTime tryDateTimeValue;
        return DateTime.TryParse(strValue, out tryDateTimeValue);
      }

      // Check of char or varchar.
      if (IsChar)
      {
        return strValue.Length <= (Length == 0 ? 1 : Length);
      }

      // Check if enum or set.
      if (IsSetOrEnum)
      {
        if (SetOrEnumElements == null)
        {
          return false;
        }

        strValue = strValue.ToLowerInvariant();
        var superSet = new HashSet<string>(SetOrEnumElements.Select(el => el.ToLowerInvariant().Trim(new[] { '\'' })));
        if (lowerTypeName == "enum")
        {
          return superSet.Contains(strValue);
        }

        var valueSet = strValue.Split(new[] {','}, StringSplitOptions.RemoveEmptyEntries).Select(el => el.Trim());
        return superSet.IsSupersetOf(valueSet);
      }

      // Check for decimal values.
      if (IsDecimal)
      {
        int floatingPointPos = strValue.IndexOf(".", StringComparison.Ordinal);
        int floatingPointLen = floatingPointPos >= 0 ? 1 : 0;
        int signLen = strValue.StartsWith("+") || strValue.StartsWith("-") ? 1 : 0;
        bool lengthCompliant = (strValue.Length - floatingPointLen - signLen) <= Length;
        bool decimalPlacesCompliant = floatingPointPos < 0 || strValue.Substring(floatingPointPos + 1, strValue.Length - floatingPointPos - 1).Length <= DecimalPlaces;
        bool floatingPointCompliant = lengthCompliant && decimalPlacesCompliant;
        if (lowerTypeName == "decimal" || lowerTypeName == "numeric")
        {
          decimal tryDecimalValue;
          return decimal.TryParse(strValue, out tryDecimalValue) && floatingPointCompliant;
        }

        double tryDoubleValue;
        return double.TryParse(strValue, out tryDoubleValue) && floatingPointCompliant;
      }

      // For future types non recognized by MySQL for Excel.
      return true;
    }

    /// <summary>
    /// Creates a new object that is a shallow copy of the current instance.
    /// </summary>
    /// <returns>A new object that is a copy of this instance.</returns>
    public object Clone()
    {
      return MemberwiseClone();
    }

    /// <summary>
    /// Checks whether values with this data type can be safely stored in a column with the target data type.
    /// </summary>
    /// <param name="targetType">The target <see cref="MySqlDataType"/>.</param>
    /// <returns><c>true</c> if values with this data type can be safely stored in a column with the target data type, <c>false</c> otherwise.</returns>
    public bool FitsIntoTargetType(MySqlDataType targetType)
    {
      if (targetType == null || string.IsNullOrEmpty(targetType.TypeName))
      {
        return false;
      }

      if (string.IsNullOrEmpty(TypeName))
      {
        return true;
      }

      var sourceTypeName = TypeName.ToLowerInvariant();
      var targetTypeName = targetType.TypeName.ToLowerInvariant();
      if (!MySqlDisplayDataType.BaseTypeNamesList.Contains(sourceTypeName) || !MySqlDisplayDataType.BaseTypeNamesList.Contains(targetTypeName))
      {
        System.Diagnostics.Debug.WriteLine("FitsIntoTargetType: One of the 2 types is Invalid.");
        return false;
      }

      if (targetTypeName == sourceTypeName)
      {
        return true;
      }

      if (targetTypeName.Contains("char") || targetTypeName.Contains("text") || targetTypeName.Contains("enum") || targetTypeName.Contains("set") || targetTypeName == "json")
      {
        return true;
      }

      bool type1IsInt = sourceTypeName.Contains("int");
      bool type2IsInt = targetTypeName.Contains("int");
      bool type1IsDecimal = sourceTypeName == "float" || sourceTypeName == "numeric" || sourceTypeName == "decimal" || sourceTypeName == "real" || sourceTypeName == "double";
      bool type2IsDecimal = targetTypeName == "float" || targetTypeName == "numeric" || targetTypeName == "decimal" || targetTypeName == "real" || targetTypeName == "double";
      if ((type1IsInt || sourceTypeName == "year") && (type2IsInt || type2IsDecimal || targetTypeName == "year"))
      {
        return true;
      }

      if (type1IsDecimal && type2IsDecimal)
      {
        return true;
      }

      if ((sourceTypeName.Contains("bool") || sourceTypeName == "tinyint" || sourceTypeName == "bit") && (targetTypeName.Contains("bool") || targetTypeName == "tinyint" || targetTypeName == "bit"))
      {
        return true;
      }

      bool type1IsDate = sourceTypeName.Contains("date") || sourceTypeName == "timestamp";
      bool type2IsDate = targetTypeName.Contains("date") || targetTypeName == "timestamp";
      if (type1IsDate && type2IsDate)
      {
        return true;
      }

      if (sourceTypeName == "time" && targetTypeName == "time")
      {
        return true;
      }

      if (sourceTypeName.Contains("blob") && targetTypeName.Contains("blob"))
      {
        return true;
      }

      if (sourceTypeName.Contains("binary") && targetTypeName.Contains("binary"))
      {
        return true;
      }

      // Spatial data
      var type2IsGeometryCollection = targetTypeName.Contains("geometrycollection");
      var type2IsGeometry = targetTypeName.Contains("geometry") && !type2IsGeometryCollection;
      var type2IsMultiCurve = targetTypeName.Contains("multicurve");
      var type2IsCurve = targetTypeName.Contains("curve") && !type2IsMultiCurve;
      var type2IsMultiSurface = targetTypeName.Contains("multisurface");
      var type2IsSurface = targetTypeName.Contains("surface") && !type2IsMultiSurface;
      var type1IsMultiSpatial = sourceTypeName.Contains("multi");
      if (sourceTypeName.Contains("multilinestring") && type2IsMultiCurve)
      {
        return true;
      }

      if (sourceTypeName.Contains("multipolygon") && type2IsMultiSurface)
      {
        return true;
      }

      if (type1IsMultiSpatial && (type2IsGeometryCollection || type2IsGeometry))
      {
        return true;
      }

      if (sourceTypeName.Contains("polygon") && type2IsSurface || type2IsGeometry)
      {
        return true;
      }

      var type1IsLineString = sourceTypeName.Contains("linestring");
      if (type1IsLineString && (type2IsCurve || type2IsGeometry))
      {
        return true;
      }

      if (!type1IsMultiSpatial && !type1IsLineString && sourceTypeName.Contains("line") && (targetTypeName.Contains("linestring") || type2IsCurve || type2IsGeometry))
      {
        return true;
      }

      if ((targetTypeName.Contains("geometrycollection") || targetTypeName.Contains("surface") || targetTypeName.Contains("curve") || targetTypeName.Contains("point")) && type2IsGeometry)
      {
        return true;
      }

      return false;
    }

    /// <summary>
    /// Gets the <see cref="MySqlDbType"/> corresponding to the <see cref="FullType"/>.
    /// </summary>
    /// <param name="typeDefaultValue">The default value corresponding to the <see cref="FullType"/>.</param>
    /// <returns>The <see cref="MySqlDbType"/> corresponding to the <see cref="FullType"/>.</returns>
    public MySqlDbType GetMySqlDbType(out object typeDefaultValue)
    {
      typeDefaultValue = null;
      switch (TypeName.ToLowerInvariant())
      {
        case "bit":
          typeDefaultValue = (ulong) 0;
          return MySqlDbType.Bit;

        case "int":
        case "integer":
          typeDefaultValue = Unsigned ? (uint)0 : 0;
          return Unsigned ? MySqlDbType.UInt32 : MySqlDbType.Int32;

        case "tinyint":
        case "bool":
        case "boolean":
          if (Unsigned)
          {
            typeDefaultValue = (byte)0;
            return MySqlDbType.UByte;
          }

          typeDefaultValue = (sbyte)0;
          return MySqlDbType.Byte;

        case "smallint":
          if (Unsigned)
          {
            typeDefaultValue = (ushort)0;
            return MySqlDbType.UInt16;
          }

          typeDefaultValue = (short)0;
          return MySqlDbType.Int16;

        case "mediumint":
          typeDefaultValue = Unsigned ? (uint)0 : 0;
          return Unsigned ? MySqlDbType.UInt24 : MySqlDbType.Int24;

        case "serial":
        case "bigint":
          typeDefaultValue = Unsigned ? (ulong)0 : (long)0;
          return Unsigned ? MySqlDbType.UInt64 : MySqlDbType.Int64;

        case "float":
          typeDefaultValue = (float)0;
          return MySqlDbType.Float;

        case "double":
        case "double precision":
          typeDefaultValue = (double)0;
          return MySqlDbType.Double;

        case "real":
          typeDefaultValue = TreatRealAsFloat ? (float)0 : (double)0;
          return TreatRealAsFloat ? MySqlDbType.Float : MySqlDbType.Double;

        case "numeric":
        case "dec":
        case "decimal":
        case "fixed":
          typeDefaultValue = (decimal)0;
          return MySqlDbType.Decimal;

        case "char":
        case "varchar":
          typeDefaultValue = string.Empty;
          return MySqlDbType.VarChar;

        case "binary":
          return MySqlDbType.Binary;

        case "varbinary":
          return MySqlDbType.VarBinary;

        case "set":
          typeDefaultValue = string.Empty;
          return MySqlDbType.Set;

        case "enum":
          typeDefaultValue = string.Empty;
          return MySqlDbType.Enum;

        case "blob":
          return MySqlDbType.Blob;

        case "text":
          return MySqlDbType.Text;

        case "longblob":
          return MySqlDbType.LongBlob;

        case "longtext":
          return MySqlDbType.LongText;

        case "mediumblob":
          return MySqlDbType.MediumBlob;

        case "mediumtext":
          return MySqlDbType.MediumText;

        case "tinyblob":
          return MySqlDbType.TinyBlob;

        case "tinytext":
          return MySqlDbType.TinyText;

        case "date":
          typeDefaultValue = DateTime.Today;
          return MySqlDbType.Date;

        case "datetime":
          typeDefaultValue = DateTime.Now;
          return MySqlDbType.DateTime;

        case "timestamp":
          typeDefaultValue = DateTime.Now;
          return MySqlDbType.Timestamp;

        case "time":
          typeDefaultValue = DateTime.Now.TimeOfDay;
          return MySqlDbType.Time;

        case "year":
          typeDefaultValue = DateTime.Today.Year;
          return MySqlDbType.Year;

        case "json":
          return MySqlDbType.JSON;

        case "geometry":
        case "curve":
        case "geometrycollection":
        case "linestring":
        case "multicurve":
        case "multilinestring":
        case "multipoint":
        case "multipolygon":
        case "multisurface":
        case "point":
        case "polygon":
        case "surface":
          return MySqlDbType.Geometry;
      }

      return MySqlDbType.Guid;
    }

    /// <summary>
    /// Gets a list of attributes specified in the <see cref="FullType"/>.
    /// </summary>
    /// <returns>A list of attributes specified in the <see cref="FullType"/>.</returns>
    private List<string> GetAttributes()
    {
      if (string.IsNullOrEmpty(_fullType))
      {
        return new List<string>();
      }

      int rightParensIndex = _fullType.IndexOf(')');
      int spaceIndex = _fullType.IndexOf(' ', rightParensIndex >= 0 ? rightParensIndex : TypeName.Length - 1);
      if (spaceIndex < 0)
      {
        return new List<string>();
      }

      string attributesText = _fullType.Substring(spaceIndex + 1);
      var attributesArray = attributesText.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
      var tempAttributesList = new List<string>(attributesArray.Length);
      tempAttributesList.AddRange(attributesArray.Select(attribute => attribute.ToUpperInvariant()));

      // Set attribute properties
      return tempAttributesList;
    }

    /// <summary>
    /// Gets the <see cref="DotNetType"/> corresponding to the <see cref="FullType"/>.
    /// </summary>
    /// <returns>The <see cref="DotNetType"/> corresponding to the <see cref="FullType"/>.</returns>
    private Type GetDotNetType()
    {
      switch (TypeName.ToLowerInvariant())
      {
        case "char":
        case "varchar":
        case "set":
        case "enum":
        case "text":
        case "mediumtext":
        case "tinytext":
        case "longtext":
        case "json":
          return Type.GetType("System.String");

        case "numeric":
        case "decimal":
        case "dec":
        case "fixed":
          return Type.GetType("System.Decimal");

        case "int":
        case "integer":
        case "mediumint":
        case "year":
          return !Unsigned || TypeName == "year" ? Type.GetType("System.Int32") : Type.GetType("System.UInt32");

        case "tinyint":
          return Type.GetType("System.Byte");

        case "smallint":
          return !Unsigned ? Type.GetType("System.Int16") : Type.GetType("System.UInt16");

        case "bigint":
          return !Unsigned ? Type.GetType("System.Int64") : Type.GetType("System.UInt64");

        case "bool":
        case "boolean":
        case "bit(1)":
          return Type.GetType("System.Boolean");

        case "bit":
        case "serial":
          return Type.GetType("System.UInt64");

        case "float":
          return Type.GetType("System.Single");

        case "double":
        case "real":
        case "double precision":
          return Type.GetType("System.Double");

        case "date":
        case "datetime":
        case "timestamp":
          return DatesAsMySqlDates ? typeof(MySqlDateTime) : Type.GetType("System.DateTime");

        case "time":
          return Type.GetType("System.TimeSpan");

        case "blob":
        case "longblob":
        case "mediumblob":
        case "tinyblob":
        case "binary":
        case "varbinary":
          return Type.GetType("System.Object");

        case "geometry":
        case "curve":
        case "geometrycollection":
        case "linestring":
        case "multicurve":
        case "multilinestring":
        case "multipoint":
        case "multipolygon":
        case "multisurface":
        case "point":
        case "polygon":
        case "surface":
          return typeof(MySqlGeometry);
      }

      return null;
    }

    /// <summary>
    /// Gets the full SQL definition for this MySQL type assembled from its component type name, parameters and attributes.
    /// </summary>
    /// <param name="upperCaseTypeName">Flag indicating whether the type name is returned in upper case.</param>
    /// <returns>The full SQL definition for this MySQL type.</returns>
    private string GetFullTypeFromComponents(bool upperCaseTypeName)
    {
      if (string.IsNullOrEmpty(TypeName))
      {
        return string.Empty;
      }

      // Reassemble the full type from its components
      var fullTypeBuilder = new StringBuilder(_typeName.Length * 4);
      fullTypeBuilder.Append(upperCaseTypeName ? _typeName.ToUpperInvariant() : _typeName);

      // Assemble parameters
      var paramsCount = Parameters != null ? _parameters.Count : 0;
      if (paramsCount == 0 && RequiresParameters && !IsSetOrEnum && Length > 0)
      {
        var newParameters = new List<string> { _length.ToString(CultureInfo.InvariantCulture) };
        if (IsFloatingPoint || (IsFixedPoint && DecimalPlaces > 0))
        {
          newParameters.Add(_decimalPlaces.ToString(CultureInfo.InvariantCulture));
        }

        _parameters = newParameters;
      }

      if (_parameters.Count > 0)
      {
        fullTypeBuilder.AppendFormat("({0})", string.Join(", ", _parameters));
      }

      // Assemble attributes
      var unsigned = _unsigned != null && (bool)_unsigned;
      var zeroFill = _zeroFill != null && (bool)_zeroFill;
      if ((Attributes == null || _attributes.Count == 0) && (unsigned || zeroFill))
      {
        if (_attributes == null)
        {
          _attributes = new List<string>(2);
        }

        if (unsigned)
        {
          _attributes.Add(ATTRIBUTE_UNSIGNED);
        }

        if (zeroFill)
        {
          _attributes.Add(ATTRIBUTE_ZEROFILL);
        }
      }

      if (_attributes.Count > 0)
      {
        fullTypeBuilder.AppendFormat(" {0}", string.Join(" ", _attributes));
      }

      return fullTypeBuilder.ToString();
    }

    /// <summary>
    /// Gets the maximum length this data type can hold.
    /// </summary>
    private long GetMaxLength()
    {
      if (!_validTypeName)
      {
        return 0;
      }

      switch (TypeName.ToLowerInvariant())
      {
        case "tinyint":
        case "year":
          return MYSQL_TINYINT_MAX_LENGTH;

        case "bool":
        case "boolean":
          return MYSQL_TINYINT_MAX_LENGTH + 1;

        case "bit":
          return MYSQL_BIT_MAX_LENGTH;

        case "smallint":
          return MYSQL_SMALLINT_MAX_LENGTH;

        case "mediumint":
          return MYSQL_MEDIUMINT_MAX_LENGTH;

        case "int":
        case "integer":
          return MYSQL_INT_MAX_LENGTH;

        case "bigint":
        case "serial":
          return MYSQL_BIGINT_MAX_LENGTH;

        case "numeric":
        case "decimal":
        case "dec":
        case "fixed":
          return MYSQL_DECIMAL_MAX_LENGTH;

        case "float":
          return MYSQL_FLOAT_MAX_LENGTH;

        case "double":
        case "double precision":
          return MYSQL_DOUBLE_MAX_LENGTH;

        case "real":
          return TreatRealAsFloat ? MYSQL_FLOAT_MAX_LENGTH : MYSQL_DOUBLE_MAX_LENGTH;

        case "char":
        case "binary":
        case "tinytext":
        case "tinyblob":
          return byte.MaxValue;

        case "varchar":
        case "varbinary":
        case "blob":
        case "text":
        case "set":
        case "enum":
        case "json":
        case "curve":
        case "geometry":
        case "geometrycollection":
        case "linestring":
        case "multicurve":
        case "multilinestring":
        case "multipoint":
        case "multipolygon":
        case "multisurface":
        case "point":
        case "polygon":
        case "surface":
          return ushort.MaxValue;

        case "mediumblob":
        case "mediumtext":
          return MYSQL_MEDIUMTEXT_MAX_LENGTH;

        case "longblob":
        case "longtext":
          return uint.MaxValue;

        case "date":
          return MYSQL_DATE_MAX_LENGTH;

        case "datetime":
        case "timestamp":
          return MYSQL_DATETIME_MAX_LENGTH;

        case "time":
          return MYSQL_TIME_MAX_LENGTH;
      }

      // Unknown data type.
      return 0;
    }

    /// <summary>
    /// Gets a list of parameters from the <see cref="FullType"/>.
    /// </summary>
    /// <returns>A list of parameters.</returns>
    private List<string> GetParameters()
    {
      if (string.IsNullOrEmpty(_fullType))
      {
        return new List<string>();
      }

      int lParensIndex = _fullType.IndexOf('(');
      int rParensIndex = _fullType.IndexOf(')');
      if (lParensIndex < 0 || rParensIndex < 0)
      {
        return new List<string>();
      }

      int firstParamIndex = lParensIndex + 1;
      return _fullType.Substring(firstParamIndex, rParensIndex - firstParamIndex).Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries).Select(element => element.Trim()).ToList();
    }

    /// <summary>
    /// Gets just the type name from the <see cref="FullType"/>.
    /// </summary>
    /// <returns>The type name.</returns>
    private string GetTypeName()
    {
      if (string.IsNullOrEmpty(_fullType))
      {
        return string.Empty;
      }

      string typeName;
      int lParensIndex = _fullType.IndexOf('(');
      if (lParensIndex >= 0)
      {
        typeName = _fullType.Substring(0, lParensIndex);
      }
      else
      {
        int spaceIndex = _fullType.IndexOf(' ');
        if (spaceIndex >= 0 &&
            _fullType.Substring(spaceIndex + 1).StartsWith("precision", StringComparison.InvariantCultureIgnoreCase))
        {
          spaceIndex = _fullType.IndexOf(' ', spaceIndex + 1);
        }

        typeName = spaceIndex >= 0 ? _fullType.Substring(0, spaceIndex) : _fullType;
      }

      string displayTypeName = MySqlDisplayDataType.GetDisplayTypeName(typeName, out _validTypeName);
      return displayTypeName;
    }

    /// <summary>
    /// Resets fields used by properties so they are set when the property is accessed.
    /// </summary>
    private void ResetFields()
    {
      _attributes = null;
      _decimalPlaces = -1;
      _dotNetType = null;
      _fullType = null;
      _fullTypeSql = null;
      _isBinary = null;
      _isBit = null;
      _isBlob = null;
      _isBool = null;
      _isChar = null;
      _isDate = null;
      _isDateBased = null;
      _isDateTimeOrTimeStamp = null;
      _isFixedPoint = null;
      _isFloatingPoint = null;
      _isGeometry = null;
      _isInteger = null;
      _isJson = null;
      _isSetOrEnum = null;
      _isText = null;
      _isTime = null;
      _isValid = null;
      _isVariable = null;
      _isYear = null;
      _length = -1;
      _maxLength = -1;
      _mayBeBool = null;
      _mySqlDbType = MySqlDbType.Guid;
      _parameters = null;
      _typeName = null;
      _unsigned = null;
      _validTypeName = true;
      _zeroFill = null;
    }

    /// <summary>
    /// Resets the full type so it can get reassembled by its components.
    /// </summary>
    /// <param name="resetAttributes">Flag indicating whether the attributes list is also reset so they get reassembled by attribute properties.</param>
    /// <param name="resetParameters">Flag indicating whether the parameters list is also reset so they get reassembled by parameter properties.</param>
    private void ResetFullType(bool resetAttributes, bool resetParameters)
    {
      // Before setting _fullType to null, make sure its components are stored in its fields
      if (string.IsNullOrEmpty(TypeName) || Attributes == null || Parameters == null)
      {
        return;
      }

      _fullType = null;
      _fullTypeSql = null;

      if (resetAttributes)
      {
        _attributes = null;
      }

      if (resetParameters)
      {
        _parameters = null;
      }
    }

    /// <summary>
    /// Sets the <seealso cref="_mySqlDbType"/> and <seealso cref="_typeDefaultValue"/> fields.
    /// </summary>
    private void SetDbTypeAndItsDefaultValue()
    {
      _mySqlDbType = GetMySqlDbType(out _typeDefaultValue);
    }

    /// <summary>
    /// Validates the attributes specified in the <see cref="FullType"/>.
    /// </summary>
    /// <returns><c>true</c> if attributes are correctly specified, <c>false</c> otherwise.</returns>
    private bool ValidateAttributes()
    {
      if (Attributes == null || _attributes.Count == 0)
      {
        return true;
      }

      bool allValid = true;
      int unsignedCount = 0;
      int zeroFillCount = 0;
      foreach (string attribute in _attributes)
      {
        if (attribute.Equals(ATTRIBUTE_UNSIGNED, StringComparison.InvariantCultureIgnoreCase))
        {
          unsignedCount++;
        }
        else if (attribute.Equals(ATTRIBUTE_ZEROFILL, StringComparison.InvariantCultureIgnoreCase))
        {
          zeroFillCount++;
        }
        else
        {
          allValid = false;
          break;
        }
      }

      allValid = allValid && unsignedCount < 2 && zeroFillCount < 2;
      return allValid;
    }

    /// <summary>
    /// Validates the parameters specified in the <see cref="FullType"/>.
    /// </summary>
    /// <returns><c>true</c> if attributes are correctly specified, <c>false</c> otherwise.</returns>
    private bool ValidateParameters()
    {
      int paramsCount = Parameters == null ? 0 : Parameters.Count;
      if (IsSetOrEnum)
      {
        if (paramsCount == 0)
        {
          return false;
        }

        InvalidSetOrEnumElementsIndexes = SetOrEnumElements.CheckForCorrectSingleQuoting();
        return InvalidSetOrEnumElementsIndexes == null || InvalidSetOrEnumElementsIndexes.Length == 0;
      }

      // If not a type that could contain parameters (except SET or ENUM) then consider valid if no parameters are found.
      if (!IsChar && !IsNumeric && !IsBit && !IsYear && !IsBinary)
      {
        return paramsCount == 0;
      }

      // If no parameters then only flag as invalid if a variable type.
      if (paramsCount == 0)
      {
        return !IsVariable;
      }

      // Validate first the number of parameters
      bool validParameters = (IsFloatingPoint && paramsCount == 2) || (IsFixedPoint && paramsCount >= 1) || paramsCount == 1;

      // Validate then if the parameters are numeric
      long length = -1;
      validParameters = validParameters && long.TryParse(_parameters[0], out length);
      if (IsDecimal && paramsCount > 1)
      {
        int decimalPlaces;
        return validParameters && int.TryParse(_parameters[1], out decimalPlaces);
      }

      if (IsYear)
      {
        var min51VersionDeprecatingYear2 = new Version(5, 1, 65);
        var min55VersionDeprecatingYear2 = new Version(5, 5, 27);
        var min56VersionDeprecatingYear2 = new Version(5, 6, 6);
        var min57VersionDeprecatingYear2 = new Version(5, 7, 5);
        var serverVersion = Globals.ThisAddIn.ActiveExcelPane != null
                            && Globals.ThisAddIn.ActiveExcelPane.WbConnection != null
                            && !string.IsNullOrEmpty(Globals.ThisAddIn.ActiveExcelPane.WbConnection.ServerVersion)
          ? Version.Parse(Globals.ThisAddIn.ActiveExcelPane.WbConnection.ServerVersion)
          : null;
        var deprecatedYear2 = serverVersion != null
                              && (serverVersion.Major > min57VersionDeprecatingYear2.Major
                                  || (serverVersion.Minor == min51VersionDeprecatingYear2.Minor && serverVersion.Build >= min51VersionDeprecatingYear2.Build)
                                  || (serverVersion.Minor == min55VersionDeprecatingYear2.Minor && serverVersion.Build >= min55VersionDeprecatingYear2.Build)
                                  || (serverVersion.Minor == min56VersionDeprecatingYear2.Minor && serverVersion.Build >= min56VersionDeprecatingYear2.Build)
                                  || (serverVersion.Minor == min57VersionDeprecatingYear2.Minor && serverVersion.Build >= min57VersionDeprecatingYear2.Build));
        return validParameters && (length == 4 || (length == 2 && !deprecatedYear2));
      }

      return validParameters && length > 0;
    }
  }
}
