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
using System.Data;
using System.Globalization;
using System.Linq;
using MySql.Data.MySqlClient;
using MySQL.ForExcel.Classes.Exceptions;

namespace MySQL.ForExcel.Classes
{
  /// <summary>
  /// Provides extension methods and other static methods to leverage the work with MySQL and native ADO.NET data types.
  /// </summary>
  public static class DataTypeUtilities
  {
    #region Constants

    /// <summary>
    /// The maximum length a MySQL bigint column can hold.
    /// </summary>
    public const int MYSQL_BIGINT_MAX_LENGTH = 20;

    /// <summary>
    /// The maximum length a MySQL bit column can hold.
    /// </summary>
    public const int MYSQL_BIT_MAX_LENGTH = 64;

    /// <summary>
    /// The date format used by DateTime fields in MySQL databases.
    /// </summary>
    public const string MYSQL_DATE_FORMAT = "yyyy-MM-dd HH:mm:ss";

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

    #endregion Constants

    /// <summary>
    /// Compares the values in a data table row-column and its corresponding Excel cell value.
    /// </summary>
    /// <param name="dataTableValue">The value stored in a <see cref="System.Data.DataTable"/> row and column.</param>
    /// <param name="excelValue">The value contained in an Excel's cell.</param>
    /// <returns><c>true</c> if the values are considered equal, <c>false</c> otherwise.</returns>
    public static bool ExcelValueEqualsDataTableValue(object dataTableValue, object excelValue)
    {
      bool areEqual = dataTableValue.Equals(excelValue);

      if (areEqual)
      {
        return true;
      }

      string strExcelValue = excelValue.ToString();
      string strExcelValueIfBool = excelValue.GetType().ToString() == "System.Boolean" ? ((bool)excelValue ? "1" : "0") : null;
      string nativeDataTableType = dataTableValue.GetType().ToString();
      switch (nativeDataTableType)
      {
        case "System.String":
          areEqual = string.CompareOrdinal(dataTableValue.ToString(), strExcelValue) == 0;
          break;

        case "System.Byte":
          byte byteTableValue = (byte)dataTableValue;
          byte byteExcelValue;
          if (strExcelValueIfBool != null)
          {
            strExcelValue = strExcelValueIfBool;
          }

          if (Byte.TryParse(strExcelValue, out byteExcelValue))
          {
            areEqual = byteTableValue == byteExcelValue;
          }

          break;

        case "System.UInt16":
          ushort ushortTableValue = (ushort)dataTableValue;
          ushort ushortExcelValue;
          if (strExcelValueIfBool != null)
          {
            strExcelValue = strExcelValueIfBool;
          }

          if (UInt16.TryParse(strExcelValue, out ushortExcelValue))
          {
            areEqual = ushortTableValue == ushortExcelValue;
          }

          break;

        case "System.Int16":
          short shortTableValue = (short)dataTableValue;
          short shortExcelValue;
          if (strExcelValueIfBool != null)
          {
            strExcelValue = strExcelValueIfBool;
          }

          if (Int16.TryParse(strExcelValue, out shortExcelValue))
          {
            areEqual = shortTableValue == shortExcelValue;
          }

          break;

        case "System.UInt32":
          uint uintTableValue = (uint)dataTableValue;
          uint uintExcelValue;
          if (strExcelValueIfBool != null)
          {
            strExcelValue = strExcelValueIfBool;
          }

          if (UInt32.TryParse(strExcelValue, out uintExcelValue))
          {
            areEqual = uintTableValue == uintExcelValue;
          }

          break;

        case "System.Int32":
          int intTableValue = (int)dataTableValue;
          int intExcelValue;
          if (strExcelValueIfBool != null)
          {
            strExcelValue = strExcelValueIfBool;
          }

          if (Int32.TryParse(strExcelValue, out intExcelValue))
          {
            areEqual = intTableValue == intExcelValue;
          }

          break;

        case "System.UInt64":
          ulong ulongTableValue = (ulong)dataTableValue;
          ulong ulongExcelValue;
          if (strExcelValueIfBool != null)
          {
            strExcelValue = strExcelValueIfBool;
          }

          if (UInt64.TryParse(strExcelValue, out ulongExcelValue))
          {
            areEqual = ulongTableValue == ulongExcelValue;
          }

          break;

        case "System.Int64":
          long longTableValue = (long)dataTableValue;
          long longExcelValue;
          if (strExcelValueIfBool != null)
          {
            strExcelValue = strExcelValueIfBool;
          }

          if (Int64.TryParse(strExcelValue, out longExcelValue))
          {
            areEqual = longTableValue == longExcelValue;
          }

          break;

        case "System.Decimal":
          decimal decimalTableValue = (decimal)dataTableValue;
          decimal decimalExcelValue;
          if (Decimal.TryParse(strExcelValue, out decimalExcelValue))
          {
            areEqual = decimalTableValue == decimalExcelValue;
          }

          break;

        case "System.Single":
          float floatTableValue = (float)dataTableValue;
          float floatExcelValue;
          if (Single.TryParse(strExcelValue, out floatExcelValue))
          {
            areEqual = floatTableValue.CompareTo(floatExcelValue) == 0;
          }

          break;

        case "System.Double":
          double doubleTableValue = (double)dataTableValue;
          double doubleExcelValue;
          if (Double.TryParse(strExcelValue, out doubleExcelValue))
          {
            areEqual = doubleTableValue.CompareTo(doubleExcelValue) == 0;
          }

          break;

        case "System.Boolean":
          bool boolTableValue = (bool)dataTableValue;
          bool boolExcelValue;
          if (Boolean.TryParse(strExcelValue, out boolExcelValue))
          {
            areEqual = boolTableValue == boolExcelValue;
          }

          break;

        case "System.DateTime":
          DateTime dateTableValue = (DateTime)dataTableValue;
          DateTime dateExcelValue;
          if (DateTime.TryParse(strExcelValue, out dateExcelValue))
          {
            areEqual = dateTableValue == dateExcelValue;
          }

          break;

        case "MySql.Data.Types.MySqlDateTime":
          MySql.Data.Types.MySqlDateTime mySqlDateTableValue = (MySql.Data.Types.MySqlDateTime)dataTableValue;
          MySql.Data.Types.MySqlDateTime mySqlDateExcelValue;
          try
          {
            mySqlDateExcelValue = new MySql.Data.Types.MySqlDateTime(strExcelValue);
          }
          catch
          {
            break;
          }

          areEqual = mySqlDateTableValue.Equals(mySqlDateExcelValue);
          break;

        case "System.TimeSpan":
          TimeSpan timeTableValue = (TimeSpan)dataTableValue;
          TimeSpan timeExcelValue;
          if (TimeSpan.TryParse(strExcelValue, out timeExcelValue))
          {
            areEqual = timeTableValue == timeExcelValue;
          }

          break;
      }

      return areEqual;
    }

    /// <summary>
    /// Gets a MySQL data type that can be used to store all values in a column, doing a best match from the list of detected data types on all rows of the column.
    /// </summary>
    /// <param name="proposedStrippedDataType">The proposed MySQL data type to store all values, without specifying length or size.</param>
    /// <param name="rowsDataTypesList">The list of detected data types on all rows of the column.</param>
    /// <param name="decimalMaxLen">The maximum length detected for the integral and decimal parts in case the column is of decimal origin.</param>
    /// <param name="varCharMaxLen">The maximum length detected for the text in case the column is of text origin.</param>
    /// <param name="consistentStrippedDataType">Output MySQL data type for all values, without the length of the data.</param>
    /// <returns>The consistent MySQL data type for all values, specifying the length for the data.</returns>
    public static string GetConsistentDataTypeOnAllRows(string proposedStrippedDataType, List<string> rowsDataTypesList, int[] decimalMaxLen, int[] varCharMaxLen, out string consistentStrippedDataType)
    {
      string fullDataType = proposedStrippedDataType;

      if (rowsDataTypesList.Count == 0)
      {
        consistentStrippedDataType = string.Empty;
        return string.Empty;
      }

      bool typesConsistent = rowsDataTypesList.All(str => str == proposedStrippedDataType);
      if (!typesConsistent)
      {
        int integerCount;
        int decimalCount;
        if (rowsDataTypesList.Count(str => str == "VarChar") + rowsDataTypesList.Count(str => str == "Text") == rowsDataTypesList.Count)
        {
          typesConsistent = true;
          fullDataType = "Text";
          proposedStrippedDataType = fullDataType;
        }
        else if ((integerCount = rowsDataTypesList.Count(str => str == "Integer")) + rowsDataTypesList.Count(str => str == "Bool") == rowsDataTypesList.Count)
        {
          typesConsistent = true;
          fullDataType = "Integer";
        }
        else if (integerCount + rowsDataTypesList.Count(str => str == "BigInt") == rowsDataTypesList.Count)
        {
          typesConsistent = true;
          fullDataType = "BigInt";
        }
        else if (integerCount + (decimalCount = rowsDataTypesList.Count(str => str == "Decimal")) == rowsDataTypesList.Count)
        {
          typesConsistent = true;
          proposedStrippedDataType = "Decimal";
        }
        else if (integerCount + decimalCount + rowsDataTypesList.Count(str => str == "Double") == rowsDataTypesList.Count)
        {
          typesConsistent = true;
          fullDataType = "Double";
        }
        else if (rowsDataTypesList.Count(str => str == "DateTime") + rowsDataTypesList.Count(str => str == "Date") + integerCount == rowsDataTypesList.Count)
        {
          typesConsistent = true;
          fullDataType = "DateTime";
        }
      }

      if (typesConsistent)
      {
        switch (proposedStrippedDataType)
        {
          case "VarChar":
            consistentStrippedDataType = proposedStrippedDataType;
            fullDataType = string.Format("VarChar({0})", varCharMaxLen[0]);
            break;

          case "Decimal":
            consistentStrippedDataType = proposedStrippedDataType;
            if (decimalMaxLen[0] > 12 || decimalMaxLen[1] > 2)
            {
              decimalMaxLen[0] = 65;
              decimalMaxLen[1] = 30;
            }
            else
            {
              decimalMaxLen[0] = 12;
              decimalMaxLen[1] = 2;
            }

            fullDataType = string.Format("Decimal({0}, {1})", decimalMaxLen[0], decimalMaxLen[1]);
            break;

          default:
            consistentStrippedDataType = fullDataType;
            break;
        }
      }
      else
      {
        if (varCharMaxLen[1] <= MYSQL_VARCHAR_MAX_PROPOSED_LEN)
        {
          consistentStrippedDataType = "VarChar";
          fullDataType = string.Format("VarChar({0})", varCharMaxLen[1]);
        }
        else
        {
          consistentStrippedDataType = "Text";
          fullDataType = consistentStrippedDataType;
        }
      }

      return fullDataType;
    }

    /// <summary>
    /// Gets a MySQL data type that can be used to store all values in a column, doing a best match from the list of detected data types on all rows of the column.
    /// </summary>
    /// <param name="proposedStrippedDataType">The proposed MySQL data type to store all values, without specifying length or size.</param>
    /// <param name="rowsDataTypesList">The list of detected data types on all rows of the column.</param>
    /// <param name="decimalMaxLen">The maximum length detected for the integral and decimal parts in case the column is of decimal origin.</param>
    /// <param name="varCharMaxLen">The maximum length detected for the text in case the column is of text origin.</param>
    /// <returns>The consistent MySQL data type for all values, specifying the length for the data.</returns>
    public static string GetConsistentDataTypeOnAllRows(string proposedStrippedDataType, List<string> rowsDataTypesList, int[] decimalMaxLen, int[] varCharMaxLen)
    {
      string outConsistentStrippedType;
      return GetConsistentDataTypeOnAllRows(proposedStrippedDataType, rowsDataTypesList, decimalMaxLen, varCharMaxLen, out outConsistentStrippedType);
    }

    /// <summary>
    /// An object where its data is converted to the proper date type if its of date origin.
    /// </summary>
    /// <param name="rawValue">Raw value.</param>
    /// <returns>Objected converted to the proper date type.</returns>
    public static object GetImportingValueForDateType(object rawValue)
    {
      object importingValue = rawValue;

      if (!(rawValue is MySql.Data.Types.MySqlDateTime))
      {
        return importingValue;
      }

      MySql.Data.Types.MySqlDateTime mysqlDate = (MySql.Data.Types.MySqlDateTime)rawValue;
      importingValue = mysqlDate.IsValidDateTime ? new DateTime(mysqlDate.Year, mysqlDate.Month, mysqlDate.Day, mysqlDate.Hour, mysqlDate.Minute, mysqlDate.Second) : DateTime.MinValue;
      return importingValue;
    }

    /// <summary>
    /// Gets a string representation of a raw value formatted so the value can be inserted in a target column.
    /// </summary>
    /// <param name="rawValue">The raw value to be inserted in a target column.</param>
    /// <param name="againstTypeColumn">The target column where the value will be inserted.</param>
    /// <param name="escapeStringForTextTypes">Flag indicating whether text values must have special characters escaped with a back-slash.</param>
    /// <returns>The formatted string representation of the raw value.</returns>
    public static object GetInsertingValueForColumnType(object rawValue, MySqlDataColumn againstTypeColumn, bool escapeStringForTextTypes)
    {
      if (againstTypeColumn == null)
      {
        return rawValue;
      }

      // Return values for empty raw values
      bool nullRawValue = rawValue == null || rawValue == DBNull.Value;
      if (nullRawValue)
      {
        if (againstTypeColumn.AllowNull)
        {
          return DBNull.Value;
        }

        if (againstTypeColumn.IsNumeric || againstTypeColumn.IsBinary)
        {
          return 0;
        }

        if (againstTypeColumn.IsBool)
        {
          return false;
        }

        if (!againstTypeColumn.IsDate)
        {
          return againstTypeColumn.ColumnRequiresQuotes ? string.Empty : rawValue;
        }

        if (againstTypeColumn.DataType.Name == "DateTime")
        {
          return DateTime.MinValue;
        }

        return new MySql.Data.Types.MySqlDateTime(0, 0, 0, 0, 0, 0, 0);
      }

      // Return values for raw values with data
      if (againstTypeColumn.IsDate)
      {
        DateTime dtValue;
        if (rawValue is DateTime)
        {
          dtValue = (DateTime)rawValue;
          if (againstTypeColumn.DataType.Name == "DateTime")
          {
            return dtValue;
          }

          return new MySql.Data.Types.MySqlDateTime(dtValue);
        }

        if (rawValue is MySql.Data.Types.MySqlDateTime)
        {
          MySql.Data.Types.MySqlDateTime mdtValue = (MySql.Data.Types.MySqlDateTime)rawValue;
          if (againstTypeColumn.DataType.Name == "DateTime")
          {
            return !mdtValue.IsValidDateTime ? DateTime.MinValue : mdtValue.GetDateTime();
          }

          return mdtValue;
        }

        string rawValueAsString = rawValue.ToString();
        if (rawValueAsString.IsMySqlZeroDateValue(true))
        {
          if (againstTypeColumn.DataType.Name == "DateTime")
          {
            return DateTime.MinValue;
          }

          return new MySql.Data.Types.MySqlDateTime(0, 0, 0, 0, 0, 0, 0);
        }

        if (!DateTime.TryParse(rawValueAsString, out dtValue))
        {
          return rawValue;
        }

        if (againstTypeColumn.DataType.Name == "DateTime")
        {
          return dtValue;
        }

        return new MySql.Data.Types.MySqlDateTime(dtValue);
      }

      if (againstTypeColumn.IsBool)
      {
        string rawValueAsString = rawValue.ToString().ToLowerInvariant();
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
            throw new UnrecognizedBooleanValueException();
        }
      }

      if (againstTypeColumn.ColumnRequiresQuotes)
      {
        return escapeStringForTextTypes ? rawValue.ToString().EscapeDataValueString() : rawValue.ToString();
      }

      return rawValue;
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
      strLength = strLength + (10 - strLength%10);
      return objUnpackedType.GetMySqlDataType(strLength);
    }

    /// <summary>
    /// Gets the matching MySQL data type from unboxing a packed value.
    /// </summary>
    /// <param name="dotNetType">A valid .NET data type.</param>
    /// <param name="strLength">In case of a string type, the lenght of the string data.</param>
    /// <returns>The matching MySQL data type.</returns>
    public static string GetMySqlDataType(this Type dotNetType, int strLength = 0)
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
    /// Gets the estimated maximum length of the data hold in a given MySQL data type when converted to a string representation.
    /// </summary>
    /// <param name="strippedMySqlDataType">The MySQL data type name stripped of formatting and modifiers.</param>
    /// <param name="unsigned">Flag indicating whether integer data types are unsigned.</param>
    /// <param name="realAsFloat">Flag indicating if real is translated to float or to double.</param>
    /// <returns>The estimated maximum length of the data hold in a given MySQL data type when converted to a string representation.</returns>
    public static long GetMySqlDataTypeMaxLength(string strippedMySqlDataType, bool unsigned, bool realAsFloat)
    {
      switch (strippedMySqlDataType.ToLowerInvariant())
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
          return MYSQL_DOUBLE_MAX_LENGTH;

        case "real":
          return realAsFloat ? MYSQL_FLOAT_MAX_LENGTH : MYSQL_DOUBLE_MAX_LENGTH;

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
    /// Gets the <see cref="MySqlDbType"/> corresponding to the given MySQL data type.
    /// </summary>
    /// <param name="mySqlType">A MySQL data type name.</param>
    /// <param name="unsigned">Flag indicating whether the type is unsigned.</param>
    /// <param name="bitPrecision">The precision for a bit data type to determine if it represents a boolean value or a number.</param>
    /// <param name="defaultValue">The default value of the data type.</param>
    /// <returns>The <see cref="MySqlDbType"/> corresponding to the given MySQL data type.</returns>
    public static MySqlDbType GetMySqlDbType(string mySqlType, bool unsigned, byte bitPrecision, out object defaultValue)
    {
      MySqlDbType dbType;
      mySqlType = mySqlType.ToLowerInvariant();
      switch (mySqlType)
      {
        case "bit":
          dbType = MySqlDbType.Bit;
          if (bitPrecision > 1)
          {
            defaultValue = 0;
          }
          else
          {
            defaultValue = false;
          }
          break;

        case "int":
        case "integer":
          dbType = MySqlDbType.Int32;
          defaultValue = 0;
          break;

        case "tinyint":
          dbType = unsigned ? MySqlDbType.UByte : MySqlDbType.Byte;
          defaultValue = (Byte)0;
          break;

        case "smallint":
          dbType = unsigned ? MySqlDbType.UInt16 : MySqlDbType.Int16;
          defaultValue = (Int16)0;
          break;

        case "mediumint":
          dbType = unsigned ? MySqlDbType.UInt24 : MySqlDbType.Int24;
          defaultValue = 0;
          break;

        case "bigint":
          dbType = unsigned ? MySqlDbType.UInt64 : MySqlDbType.Int64;
          defaultValue = (Int64)0;
          break;

        case "numeric":
        case "decimal":
        case "float":
        case "double":
        case "real":
          dbType = mySqlType == "float" ? MySqlDbType.Float : (mySqlType == "double" || mySqlType == "real" ? MySqlDbType.Double : MySqlDbType.Decimal);
          defaultValue = (Double)0;
          break;

        case "char":
        case "varchar":
          dbType = MySqlDbType.VarChar;
          defaultValue = string.Empty;
          break;

        case "binary":
        case "varbinary":
          dbType = mySqlType.StartsWith("var") ? MySqlDbType.VarBinary : MySqlDbType.Binary;
          defaultValue = string.Empty;
          break;

        case "text":
        case "tinytext":
        case "mediumtext":
        case "longtext":
          dbType = mySqlType.StartsWith("var") ? MySqlDbType.VarBinary : MySqlDbType.Binary;
          defaultValue = string.Empty;
          break;

        case "date":
        case "datetime":
        case "timestamp":
          dbType = mySqlType == "date" ? MySqlDbType.Date : MySqlDbType.DateTime;
          defaultValue = DateTime.Today;
          break;

        case "time":
          dbType = MySqlDbType.Time;
          defaultValue = TimeSpan.Zero;
          break;

        case "blob":
          dbType = MySqlDbType.Blob;
          defaultValue = null;
          break;

        default:
          dbType = MySqlDbType.Guid;
          defaultValue = null;
          break;
      }

      return dbType;
    }

    /// <summary>
    /// Gets the best match for the MySQL data type to be used for a given raw value exported to a MySQL table.
    /// </summary>
    /// <param name="packedValue">Raw value to export</param>
    /// <param name="valueOverflow">Output flag indicating whether the value would still overflow the proposed data type.</param>
    /// <returns>The best match for the MySQL data type to be used for the given raw value.</returns>
    public static string GetMySqlExportDataType(object packedValue, out bool valueOverflow)
    {
      valueOverflow = false;
      if (packedValue == null)
      {
        return string.Empty;
      }

      Type objUnpackedType = packedValue.GetType();
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
        else if (strValue.IsMySqlZeroDateValue())
        {
          strType = "MySql.Data.Types.MySqlDateTime";
        }
      }

      switch (strType)
      {
        case "System.String":
          foreach (int t in varCharApproxLen.Where(t => strLength <= t))
          {
            return string.Format("VarChar({0})", t);
          }

          return "Text";

        case "System.Double":
          return "Double";

        case "System.Decimal":
        case "System.Single":
          int intLen = decimalPointPos;
          int fractLen = strLength - intLen - 1;
          if (intLen <= decimalApproxLen[0, 0] && fractLen <= decimalApproxLen[0, 1])
          {
            return "Decimal(12,2)";
          }

          if (intLen <= decimalApproxLen[1, 0] && fractLen <= decimalApproxLen[1, 1])
          {
            return "Decimal(65,30)";
          }

          valueOverflow = true;
          return "Double";

        case "System.Byte":
        case "System.UInt16":
        case "System.Int16":
        case "System.UInt32":
        case "System.Int32":
          return "Integer";

        case "System.UInt64":
        case "System.Int64":
          return "BigInt";

        case "System.Boolean":
          return "Bool";

        case "System.DateTime":
        case "MySql.Data.Types.MySqlDateTime":
          return strValue.Contains(":") ? "DateTime" : "Date";

        case "System.TimeSpan":
          return "Time";
      }

      return string.Empty;
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
    /// Gets a text value from a raw value (object) converted to the data value of a specific target column.
    /// </summary>
    /// <param name="rawValue">The raw value.</param>
    /// <param name="againstTypeColumn">The MySQL data column where the raw value would be stored.</param>
    /// <param name="dataForInsertion">Flag indicating whether the data is meant to be inserted or read from the column.</param>
    /// <param name="valueIsNull">Output flag indicating whether the raw value is a null one.</param>
    /// <returns>The text representation of the raw value.</returns>
    public static string GetStringValueForColumn(object rawValue, MySqlDataColumn againstTypeColumn, bool dataForInsertion, out bool valueIsNull)
    {
      string valueToDb = @"null";
      object valueObject = dataForInsertion ? GetInsertingValueForColumnType(rawValue, againstTypeColumn, true) : rawValue;
      valueIsNull = valueObject == null || valueObject == DBNull.Value;
      if (valueIsNull)
      {
        return valueToDb;
      }

      if (valueObject is DateTime)
      {
        DateTime dtValue = (DateTime)valueObject;
        if (dtValue.Equals(DateTime.MinValue))
        {
          valueIsNull = againstTypeColumn.AllowNull;
          valueToDb = valueIsNull ? @"null" : MYSQL_EMPTY_DATE;
        }
        else
        {
          valueToDb = dtValue.ToString(MYSQL_DATE_FORMAT);
        }
      }
      else if (valueObject is MySql.Data.Types.MySqlDateTime)
      {
        MySql.Data.Types.MySqlDateTime dtValue = (MySql.Data.Types.MySqlDateTime)valueObject;
        if (!dtValue.IsValidDateTime || dtValue.GetDateTime().Equals(DateTime.MinValue))
        {
          valueIsNull = againstTypeColumn.AllowNull;
          valueToDb = valueIsNull ? @"null" : MYSQL_EMPTY_DATE;
        }
        else
        {
          valueToDb = dtValue.GetDateTime().ToString(MYSQL_DATE_FORMAT);
        }
      }
      else
      {
        valueToDb = GetStringRepresentationForNumericObject(valueObject);
      }

      return valueToDb;
    }

    /// <summary>
    /// Gets a text value from a raw value (object) converted to the data value of a specific target column.
    /// </summary>
    /// <param name="rawValue">The raw value.</param>
    /// <param name="againstTypeColumn">The MySQL data column where the raw value would be stored.</param>
    /// <param name="dataForInsertion">Flag indicating whether the data is meant to be inserted or read from the column.</param>
    /// <returns>The text representation of the raw value.</returns>
    public static string GetStringValueForColumn(object rawValue, MySqlDataColumn againstTypeColumn, bool dataForInsertion)
    {
      bool valueIsNull;
      return GetStringValueForColumn(rawValue, againstTypeColumn, dataForInsertion, out valueIsNull);
    }

    /// <summary>
    /// Gets the MySQL data type name stripped of formatting and modifiers.
    /// </summary>
    /// <param name="mySqlDataType">The full MySQL data type.</param>
    /// <param name="maxLenIfNotSpecified">Flag indicating whether the maximum length of the data type should be returned if not specified within the full data type.</param>
    /// <param name="length">The length declared for this data type.</param>
    /// <returns>The MySQL data type name stripped of formatting and modifiers.</returns>
    public static string GetStrippedMySqlDataType(string mySqlDataType, bool maxLenIfNotSpecified, out long length)
    {
      mySqlDataType = mySqlDataType.Trim();
      string strippedType = string.Empty;
      length = 0;
      if (string.IsNullOrEmpty(mySqlDataType))
      {
        return strippedType;
      }

      bool unsigned = mySqlDataType.ToLowerInvariant().Contains("unsigned");
      int spaceIndex = mySqlDataType.IndexOf(' ');
      if (spaceIndex > 0)
      {
        mySqlDataType = mySqlDataType.Substring(0, spaceIndex);
      }

      int lParensIndex = mySqlDataType.IndexOf('(');
      if (lParensIndex < 0)
      {
        strippedType = mySqlDataType;
      }
      else
      {
        strippedType = mySqlDataType.Substring(0, lParensIndex);
        int commaIndex = mySqlDataType.IndexOf(',');
        int rParensIndex = mySqlDataType.IndexOf(')');
        int rPos = commaIndex < 0 ? rParensIndex : commaIndex;
        if (rPos >= 0)
        {
          string lengthStr = mySqlDataType.Substring(lParensIndex + 1, rPos - lParensIndex - 1);
          long.TryParse(lengthStr, out length);
        }
      }

      if (length == 0 && maxLenIfNotSpecified)
      {
        length = GetMySqlDataTypeMaxLength(strippedType, unsigned, false);
      }

      return strippedType;
    }

    /// <summary>
    /// Checks if the string value representing a date is a MySQL zero date.
    /// </summary>
    /// <param name="dateValueAsString">The string value representing a date.</param>
    /// <param name="checkIfIntZero">Flag indicating whether a value of 0 should also be treated as a zero date.</param>
    /// <returns><c>true</c> if the passed string value is a MySQL zero date, <c>false</c> otherwise.</returns>
    public static bool IsMySqlZeroDateValue(this string dateValueAsString, bool checkIfIntZero = false)
    {
      bool isZeroDate = dateValueAsString.StartsWith("0000-00-00") || dateValueAsString.StartsWith("00-00-00");
      if (checkIfIntZero)
      {
        isZeroDate = isZeroDate || dateValueAsString.Equals("0");
      }

      return isZeroDate;
    }

    /// <summary>
    /// Gets a value indicating whether the value of the given parameter should not be written to depending on its direction.
    /// </summary>
    /// <param name="parameter">A <see cref="MySqlParameter"/> object.</param>
    /// <returns><c>true</c> if the parameter's direction is <see cref="ParameterDirection.Output"/> or <see cref="ParameterDirection.ReturnValue"/>, <c>false</c> otherwise.</returns>
    public static bool IsReadOnly(this MySqlParameter parameter)
    {
      return parameter != null && (parameter.Direction == ParameterDirection.Output || parameter.Direction == ParameterDirection.ReturnValue);
    }

    /// <summary>
    /// Gets the Connector.NET data type object corresponding to a given MySQL data type.
    /// </summary>
    /// <param name="strippedMySqlDataType">The MySQL data type name stripped of formatting and modifiers.</param>
    /// <param name="unsigned">Flag indicating whether integer data types are unsigned.</param>
    /// <param name="realAsFloat">Flag indicating if real is translated to float or to double.</param>
    /// <returns>The Connector.NET data type object corresponding to the given MySQL data type.</returns>
    public static MySqlDbType NameToMySqlType(string strippedMySqlDataType, bool unsigned, bool realAsFloat)
    {
      switch (strippedMySqlDataType.ToLowerInvariant())
      {
        case "char":
          return MySqlDbType.String;

        case "varchar":
          return MySqlDbType.VarChar;

        case "date":
          return MySqlDbType.Date;

        case "datetime":
          return MySqlDbType.DateTime;

        case "numeric":
        case "decimal":
        case "dec":
        case "fixed":
          //if (connection.driver.Version.isAtLeast(5, 0, 3))
          //  return MySqlDbType.NewDecimal;
          //else
          return MySqlDbType.Decimal;

        case "year":
          return MySqlDbType.Year;

        case "time":
          return MySqlDbType.Time;

        case "timestamp":
          return MySqlDbType.Timestamp;

        case "set":
          return MySqlDbType.Set;

        case "enum":
          return MySqlDbType.Enum;

        case "bit":
          return MySqlDbType.Bit;

        case "tinyint":
          return unsigned ? MySqlDbType.UByte : MySqlDbType.Byte;

        case "bool":
        case "boolean":
          return MySqlDbType.Byte;

        case "smallint":
          return unsigned ? MySqlDbType.UInt16 : MySqlDbType.Int16;

        case "mediumint":
          return unsigned ? MySqlDbType.UInt24 : MySqlDbType.Int24;

        case "int":
        case "integer":
          return unsigned ? MySqlDbType.UInt32 : MySqlDbType.Int32;

        case "serial":
          return MySqlDbType.UInt64;

        case "bigint":
          return unsigned ? MySqlDbType.UInt64 : MySqlDbType.Int64;

        case "float":
          return MySqlDbType.Float;

        case "double":
          return MySqlDbType.Double;

        case "real":
          return realAsFloat ? MySqlDbType.Float : MySqlDbType.Double;

        case "text":
          return MySqlDbType.Text;

        case "blob":
          return MySqlDbType.Blob;

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

        case "binary":
          return MySqlDbType.Binary;

        case "varbinary":
          return MySqlDbType.VarBinary;
      }

      throw new UnhandledMySqlTypeException();
    }

    /// <summary>
    /// Gets the .NET data type corresponding to a given MySQL data type.
    /// </summary>
    /// <param name="strippedMySqlDataType">The MySQL data type name stripped of formatting and modifiers.</param>
    /// <param name="unsigned">Flag indicating whether integer data types are unsigned.</param>
    /// <param name="datesAsMySqlDates">Flag indicating if a date data type will use a Connector.NET MySQLDateTime type or the native DateTime type.</param>
    /// <returns>The .NET type corresponding to the given MySQL data type.</returns>
    public static Type NameToType(string strippedMySqlDataType, bool unsigned, bool datesAsMySqlDates)
    {
      strippedMySqlDataType = strippedMySqlDataType.ToLowerInvariant();
      switch (strippedMySqlDataType)
      {
        case "char":
        case "varchar":
        case "set":
        case "enum":
        case "text":
        case "mediumtext":
        case "tinytext":
        case "longtext":
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
          return !unsigned || strippedMySqlDataType == "year" ? Type.GetType("System.Int32") : Type.GetType("System.UInt32");

        case "tinyint":
          return Type.GetType("System.Byte");

        case "smallint":
          return !unsigned ? Type.GetType("System.Int16") : Type.GetType("System.UInt16");

        case "bigint":
          return !unsigned ? Type.GetType("System.Int64") : Type.GetType("System.UInt64");

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
          return Type.GetType("System.Double");

        case "date":
        case "datetime":
        case "timestamp":
          return datesAsMySqlDates ? typeof(MySql.Data.Types.MySqlDateTime) : Type.GetType("System.DateTime");

        case "time":
          return Type.GetType("System.TimeSpan");

        case "blob":
        case "longblob":
        case "mediumblob":
        case "tinyblob":
        case "binary":
        case "varbinary":
          return Type.GetType("System.Object");
      }

      throw new UnhandledMySqlTypeException();
    }

    /// <summary>
    /// Checks if a value for the given <see cref="DbType"/> must be wrapped in quotes.
    /// </summary>
    /// <param name="dbType">A <see cref="DbType"/>.</param>
    /// <returns><c>true</c> if the given <see cref="DbType"/> must be wrapped in quotes, <c>false</c> otherwise.</returns>
    public static bool RequiresQuotesForValue(this DbType dbType)
    {
      return dbType == DbType.AnsiString
             || dbType == DbType.AnsiStringFixedLength
             || dbType == DbType.Date
             || dbType == DbType.DateTime
             || dbType == DbType.DateTime2
             || dbType == DbType.Guid
             || dbType == DbType.String
             || dbType == DbType.StringFixedLength;
    }

    /// <summary>
    /// Checks if a value for the given <see cref="MySqlDbType"/> must be wrapped in quotes.
    /// </summary>
    /// <param name="mySqlDbType">A <see cref="MySqlDbType"/>.</param>
    /// <returns><c>true</c> if the given <see cref="MySqlDbType"/> must be wrapped in quotes, <c>false</c> otherwise.</returns>
    public static bool RequiresQuotesForValue(this MySqlDbType mySqlDbType)
    {
      return mySqlDbType == MySqlDbType.Date
             || mySqlDbType == MySqlDbType.DateTime
             || mySqlDbType == MySqlDbType.Enum
             || mySqlDbType == MySqlDbType.Guid
             || mySqlDbType == MySqlDbType.LongText
             || mySqlDbType == MySqlDbType.MediumText
             || mySqlDbType == MySqlDbType.Newdate
             || mySqlDbType == MySqlDbType.Set
             || mySqlDbType == MySqlDbType.String
             || mySqlDbType == MySqlDbType.Text
             || mySqlDbType == MySqlDbType.Timestamp
             || mySqlDbType == MySqlDbType.TinyText
             || mySqlDbType == MySqlDbType.VarChar
             || mySqlDbType == MySqlDbType.VarString;
    }

    /// <summary>
    /// Checks whether a given string value can be converted and stored in a column with the given MySQL data type.
    /// </summary>
    /// <param name="strValue">String value to convert and store.</param>
    /// <param name="mySqlDataType">MySQL data type of the column where the value would be saved.</param>
    /// <returns><c>true</c> if the string value can be stored using the given MySQL data type, <c>false</c> otherwise.</returns>
    public static bool StringValueCanBeStoredWithMySqlType(string strValue, string mySqlDataType)
    {
      mySqlDataType = mySqlDataType.ToLowerInvariant();

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
        if (strValue.IsMySqlZeroDateValue())
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
        List<string> setOrEnumMembers = new List<string>();
        if (lParensIndex >= 0 && rParensIndex >= 0 && lParensIndex < rParensIndex)
        {
          string membersString = mySqlDataType.Substring(lParensIndex + 1, rParensIndex - lParensIndex - 1);
          string[] setMembersArray = membersString.Split(new[] { ',' });
          setOrEnumMembers.AddRange(setMembersArray.Select(s => s.Trim(new[] {'"', '\''})));
        }

        if (isEnum)
        {
          return setOrEnumMembers.Contains(strValue.ToLowerInvariant());
        }

        string[] valueSet = strValue.Split(new[] { ',' });
        bool setMatch = valueSet.Length > 0;
        return valueSet.Aggregate(setMatch, (current, val) => current && setOrEnumMembers.Contains(val.ToLowerInvariant()));
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
    /// Checks whether values with a given data type can be safely stored in a column with a second data type.
    /// </summary>
    /// <param name="strippedType1">The first MySQL data type name stripped of formatting and modifiers.</param>
    /// <param name="strippedType2">The second MySQL data type name stripped of formatting and modifiers.</param>
    /// <returns><c>true</c> if the first data type fits in the second one, <c>false</c> otherwise.</returns>
    public static bool Type1FitsIntoType2(string strippedType1, string strippedType2)
    {
      if (string.IsNullOrEmpty(strippedType1))
      {
        return true;
      }

      if (string.IsNullOrEmpty(strippedType2))
      {
        return false;
      }

      strippedType1 = strippedType1.ToLowerInvariant();
      strippedType2 = strippedType2.ToLowerInvariant();
      if (!MySqlDataType.BaseTypeNamesList.Contains(strippedType1) || !MySqlDataType.BaseTypeNamesList.Contains(strippedType2))
      {
        System.Diagnostics.Debug.WriteLine("Type1FitsIntoType2: One of the 2 types is Invalid.");
        return false;
      }

      if (strippedType2 == strippedType1)
      {
        return true;
      }

      if (strippedType2.Contains("char") || strippedType2.Contains("text") || strippedType2.Contains("enum") || strippedType2.Contains("set"))
      {
        return true;
      }

      bool type1IsInt = strippedType1.Contains("int");
      bool type2IsInt = strippedType2.Contains("int");
      bool type1IsDecimal = strippedType1 == "float" || strippedType1 == "numeric" || strippedType1 == "decimal" || strippedType1 == "real" || strippedType1 == "double";
      bool type2IsDecimal = strippedType2 == "float" || strippedType2 == "numeric" || strippedType2 == "decimal" || strippedType2 == "real" || strippedType2 == "double";
      if ((type1IsInt || strippedType1 == "year") && (type2IsInt || type2IsDecimal || strippedType2 == "year"))
      {
        return true;
      }

      if (type1IsDecimal && type2IsDecimal)
      {
        return true;
      }

      if ((strippedType1.Contains("bool") || strippedType1 == "tinyint" || strippedType1 == "bit") && (strippedType2.Contains("bool") || strippedType2 == "tinyint" || strippedType2 == "bit"))
      {
        return true;
      }

      bool type1IsDate = strippedType1.Contains("date") || strippedType1 == "timestamp";
      bool type2IsDate = strippedType2.Contains("date") || strippedType2 == "timestamp";
      if (type1IsDate && type2IsDate)
      {
        return true;
      }

      if (strippedType1 == "time" && strippedType2 == "time")
      {
        return true;
      }

      if (strippedType1.Contains("blob") && strippedType2.Contains("blob"))
      {
        return true;
      }

      if (strippedType1.Contains("binary") && strippedType2.Contains("binary"))
      {
        return true;
      }

      // Spatial data
      var type2IsGeometryCollection = strippedType2.Contains("geometrycollection");
      var type2IsGeometry = strippedType2.Contains("geometry") && !type2IsGeometryCollection;
      var type2IsMultiCurve = strippedType2.Contains("multicurve");
      var type2IsCurve = strippedType2.Contains("curve") && !type2IsMultiCurve;
      var type2IsMultiSurface = strippedType2.Contains("multisurface");
      var type2IsSurface = strippedType2.Contains("surface") && !type2IsMultiSurface;
      var type1IsMultiSpatial = strippedType1.Contains("multi");
      if (strippedType1.Contains("multilinestring") && type2IsMultiCurve)
      {
        return true;
      }

      if (strippedType1.Contains("multipolygon") && type2IsMultiSurface)
      {
        return true;
      }

      if (type1IsMultiSpatial && (type2IsGeometryCollection || type2IsGeometry))
      {
        return true;
      }

      if (strippedType1.Contains("polygon") && type2IsSurface || type2IsGeometry)
      {
        return true;
      }

      var type1IsLineString = strippedType1.Contains("linestring");
      if (type1IsLineString && (type2IsCurve || type2IsGeometry))
      {
        return true;
      }

      if (!type1IsMultiSpatial && !type1IsLineString && strippedType1.Contains("line") && (strippedType2.Contains("linestring") || type2IsCurve || type2IsGeometry))
      {
        return true;
      }

      if ((strippedType2.Contains("geometrycollection") || strippedType2.Contains("surface") || strippedType2.Contains("curve") || strippedType2.Contains("point")) && type2IsGeometry)
      {
        return true;
      }

      return false;
    }

    /// <summary>
    /// Validates that a user typed data type is a valid MySQL data type.
    /// A blank data type is considered valid.
    /// </summary>
    /// <param name="proposedUserType">A MySQL data type as specified for new columns in a CREATE TABLE statement.</param>
    /// <returns><c>true</c> if the type is a valid MySQL data type, <c>false</c> otherwise.</returns>
    public static bool ValidateUserDataType(string proposedUserType)
    {
      // If the proposed type is blank return true since a blank data type is considered valid.
      if (proposedUserType.Length == 0)
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
      var mySqlDataType = MySqlDataType.DataTypesList.FirstOrDefault(mType => mType.IsBaseType && string.Equals(mType.Name, pureDataType, StringComparison.InvariantCultureIgnoreCase));
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
      string[] parameterValues = string.IsNullOrEmpty(parametersText) ? null : parametersText.Split(',');
      int parametersCount = parameterValues == null ? 0 : parameterValues.Length;

      // If there are no parameters but parenthesis were provided the data type is invalid.
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

      // Check if the paremeter values are valid integers for data types with 1 or 2 parameters (varchar and numeric types).
      if (enumOrSet)
      {
        return true;
      }

      if (parameterValues == null)
      {
        return true;
      }

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
