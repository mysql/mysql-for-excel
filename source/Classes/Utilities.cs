// 
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
//

namespace MySQL.ForExcel
{
  using System;
  using System.Collections.Generic;
  using System.Data;
  using System.Drawing;
  using System.Drawing.Imaging;
  using System.Globalization;
  using System.Linq;
  using System.Reflection;
  using System.Runtime.InteropServices;
  using System.Text;
  using System.Windows.Forms;
  using MySql.Data.MySqlClient;
  using MySQL.Utility;
  using MySQL.Utility.Forms;

  /// <summary>
  /// Provides extension methods and other static methods to leverage the work with MySQL and native ADO.NET data types.
  /// </summary>
  public static class DataTypeUtilities
  {
    /// <summary>
    /// The date format used by DateTime fields in MySQL databases.
    /// </summary>
    public const string MYSQL_DATE_FORMAT = "yyyy-MM-dd HH:mm:ss";

    /// <summary>
    /// Represents an empty date in MySQL DateTime format.
    /// </summary>
    public const string MYSQL_EMPTY_DATE = "0000-00-00 00:00:00";

    /// <summary>
    /// The maximum proposed length of the MySQL varchar data type.
    /// </summary>
    public const int MYSQL_VARCHAR_MAX_PROPOSED_LEN = 4000;

    /// <summary>
    /// Compares the values in a data table row-column and its corresponding Excel cell value.
    /// </summary>
    /// <param name="dataTableValue">The value stored in a <see cref="DataTable"/> row and column.</param>
    /// <param name="excelValue">The value contained in an Excel's cell.</param>
    /// <returns><c>true</c> if the values are considered equal, <c>false</c> otherwise.</returns>
    public static bool ExcelValueEqualsDataTableValue(object dataTableValue, object excelValue)
    {
      bool areEqual = dataTableValue.Equals(excelValue);

      if (!areEqual && dataTableValue != null)
      {
        string strExcelValue = excelValue.ToString();
        string strExcelValueIfBool = excelValue.GetType().ToString() == "System.Boolean" ? ((bool)excelValue ? "1" : "0") : null;
        string nativeDataTableType = dataTableValue.GetType().ToString();
        switch (nativeDataTableType)
        {
          case "System.String":
            areEqual = string.Compare(dataTableValue.ToString(), strExcelValue, false) == 0;
            break;

          case "System.Byte":
            byte byteTableValue = (byte)dataTableValue;
            byte byteExcelValue = 0;
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
            ushort ushortExcelValue = 0;
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
            short shortExcelValue = 0;
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
            uint uintExcelValue = 0;
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
            int intExcelValue = 0;
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
            ulong ulongExcelValue = 0;
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
            long longExcelValue = 0;
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
            decimal decimalExcelValue = 0;
            if (Decimal.TryParse(strExcelValue, out decimalExcelValue))
            {
              areEqual = decimalTableValue == decimalExcelValue;
            }

            break;

          case "System.Single":
            float floatTableValue = (float)dataTableValue;
            float floatExcelValue = 0;
            if (Single.TryParse(strExcelValue, out floatExcelValue))
            {
              areEqual = floatTableValue == floatExcelValue;
            }

            break;

          case "System.Double":
            double doubleTableValue = (double)dataTableValue;
            double doubleExcelValue = 0;
            if (Double.TryParse(strExcelValue, out doubleExcelValue))
            {
              areEqual = doubleTableValue == doubleExcelValue;
            }

            break;

          case "System.Boolean":
            bool boolTableValue = (bool)dataTableValue;
            bool boolExcelValue = false;
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
            MySql.Data.Types.MySqlDateTime mySQLDateTableValue = (MySql.Data.Types.MySqlDateTime)dataTableValue;
            MySql.Data.Types.MySqlDateTime mySQLDateExcelValue;
            try
            {
              mySQLDateExcelValue = new MySql.Data.Types.MySqlDateTime(strExcelValue);
            }
            catch
            {
              break;
            }

            areEqual = mySQLDateTableValue.Equals(mySQLDateExcelValue);
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
        int integerCount = 0;
        int decimalCount = 0;
        if (rowsDataTypesList.Count(str => str == "Varchar") + rowsDataTypesList.Count(str => str == "Text") == rowsDataTypesList.Count)
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
        else if (rowsDataTypesList.Count(str => str == "Datetime") + rowsDataTypesList.Count(str => str == "Date") + integerCount == rowsDataTypesList.Count)
        {
          typesConsistent = true;
          fullDataType = "Datetime";
        }
      }

      if (typesConsistent)
      {
        switch (proposedStrippedDataType)
        {
          case "Varchar":
            consistentStrippedDataType = proposedStrippedDataType;
            fullDataType = string.Format("Varchar({0})", varCharMaxLen[0]);
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
          consistentStrippedDataType = "Varchar";
          fullDataType = string.Format("Varchar({0})", varCharMaxLen[1]);
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

      if (rawValue != null && rawValue is MySql.Data.Types.MySqlDateTime)
      {
        MySql.Data.Types.MySqlDateTime mysqlDate = (MySql.Data.Types.MySqlDateTime)rawValue;
        if (mysqlDate.IsValidDateTime)
        {
          importingValue = new DateTime(mysqlDate.Year, mysqlDate.Month, mysqlDate.Day, mysqlDate.Hour, mysqlDate.Minute, mysqlDate.Second);
        }
        else
        {
          importingValue = DateTime.MinValue;
        }
      }

      return importingValue;
    }

    /// <summary>
    /// Gets a string representation of a raw value formatted so the value can be inserted in a target column.
    /// </summary>
    /// <param name="rawValue">The raw value to be inserted in a target column.</param>
    /// <param name="againstTypeColumn">The target column where the value will be inserted.</param>
    /// <param name="escapeStringForTextTypes">Flag indicating whether text values must have special characters escaped with a back-slash.</param>
    /// <returns>The formatted string representation of the raw value.</returns>
    public static object GetInsertingValueForColumnType(object rawValue, MySQLDataColumn againstTypeColumn, bool escapeStringForTextTypes)
    {
      object retValue = rawValue;
      if (againstTypeColumn == null)
      {
        return rawValue;
      }

      bool cellWithNoData = rawValue == null || rawValue == DBNull.Value;
      if (cellWithNoData)
      {
        if (againstTypeColumn.AllowNull)
        {
          retValue = DBNull.Value;
        }
        else
        {
          if (againstTypeColumn.IsNumeric || againstTypeColumn.IsBinary)
          {
            retValue = 0;
          }
          else if (againstTypeColumn.IsBool)
          {
            retValue = false;
          }
          else if (againstTypeColumn.IsDate)
          {
            if (againstTypeColumn.DataType.Name == "DateTime")
            {
              retValue = DateTime.MinValue;
            }
            else
            {
              retValue = new MySql.Data.Types.MySqlDateTime(0, 0, 0, 0, 0, 0, 0);
            }
          }
          else if (againstTypeColumn.ColumnsRequireQuotes)
          {
            retValue = string.Empty;
          }
        }
      }
      else
      {
        retValue = rawValue;
        if (againstTypeColumn.IsDate)
        {
          if (rawValue is DateTime)
          {
            DateTime dtValue = (DateTime)rawValue;
            if (againstTypeColumn.DataType.Name == "DateTime")
            {
              retValue = dtValue;
            }
            else
            {
              retValue = new MySql.Data.Types.MySqlDateTime(dtValue);
            }
          }
          else if (rawValue is MySql.Data.Types.MySqlDateTime)
          {
            MySql.Data.Types.MySqlDateTime dtValue = (MySql.Data.Types.MySqlDateTime)rawValue;
            if (againstTypeColumn.DataType.Name == "DateTime")
            {
              retValue = (!dtValue.IsValidDateTime ? DateTime.MinValue : dtValue.GetDateTime());
            }
            else
            {
              retValue = dtValue;
            }
          }
          else
          {
            DateTime dtValue;
            string rawValueAsString = rawValue.ToString();
            if (rawValueAsString.StartsWith("0000-00-00") || rawValueAsString.StartsWith("00-00-00") || rawValueAsString.Equals("0"))
            {
              if (againstTypeColumn.DataType.Name == "DateTime")
              {
                retValue = DateTime.MinValue;
              }
              else
              {
                retValue = new MySql.Data.Types.MySqlDateTime(0, 0, 0, 0, 0, 0, 0);
              }
            }
            else
            {
              if (DateTime.TryParse(rawValueAsString, out dtValue))
              {
                if (againstTypeColumn.DataType.Name == "DateTime")
                {
                  retValue = dtValue;
                }
                else
                {
                  retValue = new MySql.Data.Types.MySqlDateTime(dtValue);
                }
              }
              else
              {
                retValue = rawValue;
              }
            }
          }
        }
        else if (againstTypeColumn.IsBool)
        {
          string rawValueAsString = rawValue.ToString().ToLowerInvariant();
          if (rawValueAsString == "ja" || rawValueAsString == "yes" || rawValueAsString == "true" || rawValueAsString == "1")
          {
            retValue = true;
          }
          else if (rawValueAsString == "nein" || rawValueAsString == "no" || rawValueAsString == "false" || rawValueAsString == "0")
          {
            retValue = false;
          }
        }
        else if (againstTypeColumn.ColumnsRequireQuotes)
        {
          retValue = escapeStringForTextTypes ? rawValue.ToString().EscapeDataValueString() : rawValue.ToString();
        }
      }

      return retValue;
    }

    /// <summary>
    /// Gets the matching MySQL data type from unboxing a packed value.
    /// </summary>
    /// <param name="packedValue">The packed value.</param>
    /// <returns>The matching MySQL data type.</returns>
    public static string GetMySQLDataType(object packedValue)
    {
      string retType = string.Empty;
      if (packedValue == null)
      {
        return retType;
      }

      Type objUnpackedType = packedValue.GetType();
      string strType = objUnpackedType.FullName;
      int strLength = packedValue.ToString().Length;
      strLength = strLength + (10 - strLength % 10);
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
    /// Gets a list of all the MySQL data types.
    /// </summary>
    /// <param name="paramsInParenthesisList">Output list of the number of parameters used with the data types declaration.</param>
    /// <returns>The list of all the MySQL data types</returns>
    public static List<string> GetMySQLDataTypes(out List<int> paramsInParenthesisList)
    {
      List<string> retList = new List<string>();
      retList.AddRange(new string[] {
            "bit",
            "tinyint",
            "smallint",
            "mediumint",
            "int",
            "integer",
            "bigint",
            "float",
            "double",
            "decimal",
            "numeric",
            "real",
            "bool",
            "boolean",
            "date",
            "datetime",
            "timestamp",
            "time",
            "year",
            "char",
            "varchar",
            "binary",
            "varbinary",
            "tinyblob",
            "tinytext",
            "blob",
            "text",
            "mediumblob",
            "mediumtext",
            "longblob",
            "longtext",
            "enum",
            "set"});

      //// Assemble the list of the number of parameters used with each data type in the list above.
      paramsInParenthesisList = new List<int>(retList.Count);
      paramsInParenthesisList.AddRange(new int[] { 1, 1, 1, 1, 1, 1, 1, 2, 2, 2, 2, 2, 0, 0, 0, 0, 0, 0, 1, 1, 1, 1, 1, 0, 0, 0, 0, 0, 0, 0, 0, -1, -1 });
      return retList;
    }

    /// <summary>
    /// Gets a list of all the MySQL data types.
    /// </summary>
    /// <returns>The list of all the MySQL data types</returns>
    public static List<string> GetMySQLDataTypes()
    {
      List<int> unused;
      return GetMySQLDataTypes(out unused);
    }

    /// <summary>
    /// Gets the best match for the MySQL data type to be used for a given raw value exported to a MySQL table.
    /// </summary>
    /// <param name="packedValue">Raw value to export</param>
    /// <param name="valueOverflow">Output flag indicating whether the value would still overflow the proposed data type.</param>
    /// <returns>The best match for the MySQL data type to be used for the given raw value.</returns>
    public static string GetMySQLExportDataType(object packedValue, out bool valueOverflow)
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
      int decimalPointPos = strValue.IndexOf(".");
      int[] varCharApproxLen = new int[6] { 5, 12, 25, 45, 255, MYSQL_VARCHAR_MAX_PROPOSED_LEN };
      int[,] decimalApproxLen = new int[2, 2] { { 12, 2 }, { 65, 30 } };
      int intResult = 0;
      long longResult = 0;
      int intLen = 0;
      int fractLen = 0;

      if (strType == "System.Double")
      {
        if (decimalPointPos < 0)
        {
          if (Int32.TryParse(strValue, out intResult))
          {
            strType = "System.Int32";
          }
          else if (Int64.TryParse(strValue, out longResult))
          {
            strType = "System.Int64";
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
        else if (strValue.StartsWith("0000-00-00") || strValue.StartsWith("00-00-00"))
        {
          strType = "MySql.Data.Types.MySqlDateTime";
        }
      }

      switch (strType)
      {
        case "System.String":
          for (int i = 0; i < varCharApproxLen.Length; i++)
          {
            if (strLength <= varCharApproxLen[i])
            {
              return string.Format("Varchar({0})", varCharApproxLen[i]);
            }
          }

          return "Text";

        case "System.Double":
          return "Double";

        case "System.Decimal":
        case "System.Single":
          intLen = decimalPointPos;
          fractLen = strLength - intLen - 1;
          if (intLen <= decimalApproxLen[0, 0] && fractLen <= decimalApproxLen[0, 1])
          {
            return "Decimal(12,2)";
          }
          else if (intLen <= decimalApproxLen[1, 0] && fractLen <= decimalApproxLen[1, 1])
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
          if (strValue.Contains(":"))
          {
            return "Datetime";
          }

          return "Date";

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
      else if (boxedValue is byte)
      {
        return ((byte)boxedValue).ToString("G", CultureInfo.InvariantCulture);
      }
      else if (boxedValue is short)
      {
        return ((short)boxedValue).ToString("G", CultureInfo.InvariantCulture);
      }
      else if (boxedValue is ushort)
      {
        return ((ushort)boxedValue).ToString("G", CultureInfo.InvariantCulture);
      }
      else if (boxedValue is int)
      {
        return ((int)boxedValue).ToString("G", CultureInfo.InvariantCulture);
      }
      else if (boxedValue is uint)
      {
        return ((uint)boxedValue).ToString("G", CultureInfo.InvariantCulture);
      }
      else if (boxedValue is long)
      {
        return ((long)boxedValue).ToString("G", CultureInfo.InvariantCulture);
      }
      else if (boxedValue is ulong)
      {
        return ((ulong)boxedValue).ToString("G", CultureInfo.InvariantCulture);
      }
      else if (boxedValue is float)
      {
        return ((float)boxedValue).ToString("G", CultureInfo.InvariantCulture);
      }
      else if (boxedValue is double)
      {
        return ((double)boxedValue).ToString("G", CultureInfo.InvariantCulture);
      }
      else if (boxedValue is decimal)
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
    public static string GetStringValueForColumn(object rawValue, MySQLDataColumn againstTypeColumn, bool dataForInsertion, out bool valueIsNull)
    {
      valueIsNull = true;
      string valueToDB = @"null";

      object valueObject = dataForInsertion ? DataTypeUtilities.GetInsertingValueForColumnType(rawValue, againstTypeColumn, true) : rawValue;
      valueIsNull = valueObject == null || valueObject == DBNull.Value;
      if (!valueIsNull)
      {
        if (valueObject is DateTime)
        {
          DateTime dtValue = (DateTime)valueObject;
          if (dtValue.Equals(DateTime.MinValue))
          {
            valueIsNull = againstTypeColumn.AllowNull;
            valueToDB = valueIsNull ? @"null" : MYSQL_EMPTY_DATE;
          }
          else
          {
            valueToDB = dtValue.ToString(MYSQL_DATE_FORMAT);
          }
        }
        else if (valueObject is MySql.Data.Types.MySqlDateTime)
        {
          MySql.Data.Types.MySqlDateTime dtValue = (MySql.Data.Types.MySqlDateTime)valueObject;
          if (!dtValue.IsValidDateTime || dtValue.GetDateTime().Equals(DateTime.MinValue))
          {
            valueIsNull = againstTypeColumn.AllowNull;
            valueToDB = valueIsNull ? @"null" : MYSQL_EMPTY_DATE;
          }
          else
          {
            valueToDB = dtValue.GetDateTime().ToString(MYSQL_DATE_FORMAT);
          }
        }
        else
        {
          valueToDB = GetStringRepresentationForNumericObject(valueObject);
        }
      }

      return valueToDB;
    }

    /// <summary>
    /// Gets a text value from a raw value (object) converted to the data value of a specific target column.
    /// </summary>
    /// <param name="rawValue">The raw value.</param>
    /// <param name="againstTypeColumn">The MySQL data column where the raw value would be stored.</param>
    /// <param name="dataForInsertion">Flag indicating whether the data is meant to be inserted or read from the column.</param>
    /// <returns>The text representation of the raw value.</returns>
    public static string GetStringValueForColumn(object rawValue, MySQLDataColumn againstTypeColumn, bool dataForInsertion)
    {
      bool valueIsNull = false;
      return GetStringValueForColumn(rawValue, againstTypeColumn, dataForInsertion, out valueIsNull);
    }

    /// <summary>
    /// Gets the Connector.NET data type object corresponding to a given MySQL data type.
    /// </summary>
    /// <param name="mySqlDataType">The MySQL data type name.</param>
    /// <param name="unsigned">Flag indicating whether integer data types are unsigned.</param>
    /// <param name="realAsFloat">Flag indicating if real is translated to float or to double.</param>
    /// <returns>The Connector.NET data type object corresponding to the given MySQL data type.</returns>
    public static MySqlDbType NameToMySQLType(string mySqlDataType, bool unsigned, bool realAsFloat)
    {
      switch (mySqlDataType.ToUpper(CultureInfo.InvariantCulture))
      {
        case "CHAR":
          return MySqlDbType.String;

        case "VARCHAR":
          return MySqlDbType.VarChar;

        case "DATE":
          return MySqlDbType.Date;

        case "DATETIME":
          return MySqlDbType.DateTime;

        case "NUMERIC":
        case "DECIMAL":
        case "DEC":
        case "FIXED":
          //if (connection.driver.Version.isAtLeast(5, 0, 3))
          //  return MySqlDbType.NewDecimal;
          //else
          return MySqlDbType.Decimal;

        case "YEAR":
          return MySqlDbType.Year;

        case "TIME":
          return MySqlDbType.Time;

        case "TIMESTAMP":
          return MySqlDbType.Timestamp;

        case "SET":
          return MySqlDbType.Set;

        case "ENUM":
          return MySqlDbType.Enum;

        case "BIT":
          return MySqlDbType.Bit;

        case "TINYINT":
          return unsigned ? MySqlDbType.UByte : MySqlDbType.Byte;

        case "BOOL":
        case "BOOLEAN":
          return MySqlDbType.Byte;

        case "SMALLINT":
          return unsigned ? MySqlDbType.UInt16 : MySqlDbType.Int16;

        case "MEDIUMINT":
          return unsigned ? MySqlDbType.UInt24 : MySqlDbType.Int24;

        case "INT":
        case "INTEGER":
          return unsigned ? MySqlDbType.UInt32 : MySqlDbType.Int32;

        case "SERIAL":
          return MySqlDbType.UInt64;

        case "BIGINT":
          return unsigned ? MySqlDbType.UInt64 : MySqlDbType.Int64;

        case "FLOAT":
          return MySqlDbType.Float;

        case "DOUBLE":
          return MySqlDbType.Double;

        case "REAL":
          return realAsFloat ? MySqlDbType.Float : MySqlDbType.Double;

        case "TEXT":
          return MySqlDbType.Text;

        case "BLOB":
          return MySqlDbType.Blob;

        case "LONGBLOB":
          return MySqlDbType.LongBlob;

        case "LONGTEXT":
          return MySqlDbType.LongText;

        case "MEDIUMBLOB":
          return MySqlDbType.MediumBlob;

        case "MEDIUMTEXT":
          return MySqlDbType.MediumText;

        case "TINYBLOB":
          return MySqlDbType.TinyBlob;

        case "TINYTEXT":
          return MySqlDbType.TinyText;

        case "BINARY":
          return MySqlDbType.Binary;

        case "VARBINARY":
          return MySqlDbType.VarBinary;
      }

      throw new Exception("Unhandled type encountered");
    }

    /// <summary>
    /// Gets the .NET data type corresponding to a given MySQL data type.
    /// </summary>
    /// <param name="mySqlDataType">The MySQL data type name.</param>
    /// <param name="unsigned">Flag indicating whether integer data types are unsigned.</param>
    /// <param name="datesAsMySQLDates">Flag indicating if a date data type will use a Connector.NET MySQLDateTime type or the native DateTime type.</param>
    /// <returns>The .NET type corresponding to the given MySQL data type.</returns>
    public static Type NameToType(string mySqlDataType, bool unsigned, bool datesAsMySQLDates)
    {
      string upperType = mySqlDataType.ToUpper(CultureInfo.InvariantCulture);
      switch (upperType)
      {
        case "CHAR":
        case "VARCHAR":
        case "SET":
        case "ENUM":
        case "TEXT":
        case "MEDIUMTEXT":
        case "TINYTEXT":
        case "LONGTEXT":
          return Type.GetType("System.String");

        case "NUMERIC":
        case "DECIMAL":
        case "DEC":
        case "FIXED":
          return Type.GetType("System.Decimal");

        case "INT":
        case "INTEGER":
        case "MEDIUMINT":
        case "YEAR":
          return !unsigned || upperType == "YEAR" ? Type.GetType("System.Int32") : Type.GetType("System.UInt32");

        case "TINYINT":
          return Type.GetType("System.Byte");

        case "SMALLINT":
          return !unsigned ? Type.GetType("System.Int16") : Type.GetType("System.UInt16");

        case "BIGINT":
          return !unsigned ? Type.GetType("System.Int64") : Type.GetType("System.UInt64");

        case "BOOL":
        case "BOOLEAN":
        case "BIT(1)":
          return Type.GetType("System.Boolean");

        case "BIT":
        case "SERIAL":
          return Type.GetType("System.UInt64");

        case "FLOAT":
          return Type.GetType("System.Single");

        case "DOUBLE":
        case "REAL":
          return Type.GetType("System.Double");

        case "DATE":
        case "DATETIME":
        case "TIMESTAMP":
          return datesAsMySQLDates ? typeof(MySql.Data.Types.MySqlDateTime) : Type.GetType("System.DateTime");

        case "TIME":
          return Type.GetType("System.TimeSpan");

        case "BLOB":
        case "LONGBLOB":
        case "MEDIUMBLOB":
        case "TINYBLOB":
        case "BINARY":
        case "VARBINARY":
          return Type.GetType("System.Object");
      }

      throw new Exception("Unhandled type encountered");
    }

    /// <summary>
    /// Checks whether a given string value can be converted and stored in a column with the given MySQL data type.
    /// </summary>
    /// <param name="strValue">String value to convert and store.</param>
    /// <param name="mySQLDataType">MySQL data type of the column where the value would be saved.</param>
    /// <returns><c>true</c> if the string value can be stored using the given MySQL data type, <c>false</c> otherwise.</returns>
    public static bool StringValueCanBeStoredWithMySQLType(string strValue, string mySQLDataType)
    {
      mySQLDataType = mySQLDataType.ToLowerInvariant();

      //// Return immediately for big data types.
      if (mySQLDataType.Contains("text") || mySQLDataType == "blob" || mySQLDataType == "tinyblob" || mySQLDataType == "mediumblob" || mySQLDataType == "longblob" || mySQLDataType == "binary" || mySQLDataType == "varbinary")
      {
        return true;
      }

      //// Check for boolean
      if (mySQLDataType.StartsWith("bool") || mySQLDataType == "bit" || mySQLDataType == "bit(1)")
      {
        strValue = strValue.ToLowerInvariant();
        return (strValue == "true" || strValue == "false" || strValue == "0" || strValue == "1" || strValue == "yes" || strValue == "no" || strValue == "ja" || strValue == "nein");
      }

      //// Check for integer values
      if (mySQLDataType.StartsWith("int") || mySQLDataType.StartsWith("mediumint"))
      {
        int tryIntValue = 0;
        return Int32.TryParse(strValue, out tryIntValue);
      }

      if (mySQLDataType.StartsWith("year"))
      {
        int tryYearValue = 0;
        return Int32.TryParse(strValue, out tryYearValue) && (tryYearValue >= 0 && tryYearValue < 100) || (tryYearValue > 1900 && tryYearValue < 2156);
      }

      if (mySQLDataType.StartsWith("tinyint"))
      {
        byte tryByteValue = 0;
        return Byte.TryParse(strValue, out tryByteValue);
      }

      if (mySQLDataType.StartsWith("smallint"))
      {
        short trySmallIntValue = 0;
        return Int16.TryParse(strValue, out trySmallIntValue);
      }

      if (mySQLDataType.StartsWith("bigint"))
      {
        long tryBigIntValue = 0;
        return Int64.TryParse(strValue, out tryBigIntValue);
      }

      if (mySQLDataType.StartsWith("bit"))
      {
        ulong tryBitValue = 0;
        return UInt64.TryParse(strValue, out tryBitValue);
      }

      //// Check for big numeric values
      if (mySQLDataType.StartsWith("float"))
      {
        float tryFloatValue = 0;
        return Single.TryParse(strValue, out tryFloatValue);
      }

      if (mySQLDataType.StartsWith("double") || mySQLDataType.StartsWith("real"))
      {
        double tryDoubleValue = 0;
        return Double.TryParse(strValue, out tryDoubleValue);
      }

      //// Check for date and time values.
      if (mySQLDataType == "time")
      {
        TimeSpan tryTimeSpanValue = TimeSpan.Zero;
        return TimeSpan.TryParse(strValue, out tryTimeSpanValue);
      }

      if (mySQLDataType == "date" || mySQLDataType == "datetime" || mySQLDataType == "timestamp")
      {
        DateTime tryDateTimeValue = DateTime.Now;
        if (strValue.StartsWith("0000-00-00") || strValue.StartsWith("00-00-00"))
        {
          return true;
        }
        else
        {
          return DateTime.TryParse(strValue, out tryDateTimeValue);
        }
      }

      //// Check of char or varchar.
      int lParensIndex = mySQLDataType.IndexOf("(");
      int rParensIndex = mySQLDataType.IndexOf(")");
      if (mySQLDataType.StartsWith("varchar") || mySQLDataType.StartsWith("char"))
      {
        int characterLen = 0;
        if (lParensIndex >= 0)
        {
          string paramValue = mySQLDataType.Substring(lParensIndex + 1, mySQLDataType.Length - lParensIndex - 2);
          int.TryParse(paramValue, out characterLen);
        }
        else
        {
          characterLen = 1;
        }

        return strValue.Length <= characterLen;
      }

      //// Check if enum or set.
      bool isEnum = mySQLDataType.StartsWith("enum");
      bool isSet = mySQLDataType.StartsWith("set");
      if (isSet || isEnum)
      {
        List<string> setOrEnumMembers = new List<string>();
        if (lParensIndex >= 0 && rParensIndex >= 0 && lParensIndex < rParensIndex)
        {
          string membersString = mySQLDataType.Substring(lParensIndex + 1, rParensIndex - lParensIndex - 1);
          string[] setMembersArray = membersString.Split(new char[] { ',' });
          foreach (string s in setMembersArray)
          {
            setOrEnumMembers.Add(s.Trim(new char[] { '"', '\'' }));
          }
        }

        if (isEnum)
        {
          return setOrEnumMembers.Contains(strValue.ToLowerInvariant());
        }

        if (isSet)
        {
          string[] valueSet = strValue.Split(new char[] { ',' });
          bool setMatch = valueSet.Length > 0;
          foreach (string val in valueSet)
          {
            setMatch = setMatch && setOrEnumMembers.Contains(val.ToLowerInvariant());
          }

          return setMatch;
        }
      }

      //// Check for decimal values which is the more complex.
      bool mayContainFloatingPoint = mySQLDataType.StartsWith("decimal") || mySQLDataType.StartsWith("numeric") || mySQLDataType.StartsWith("double") || mySQLDataType.StartsWith("float") || mySQLDataType.StartsWith("real");
      int commaPos = mySQLDataType.IndexOf(",");
      int[] decimalLen = new int[2] { -1, -1 };
      if (mayContainFloatingPoint && lParensIndex >= 0 && rParensIndex >= 0 && lParensIndex < rParensIndex)
      {
        decimalLen[0] = Int32.Parse(mySQLDataType.Substring(lParensIndex + 1, (commaPos >= 0 ? commaPos : rParensIndex) - lParensIndex - 1));
        if (commaPos >= 0)
        {
          decimalLen[1] = Int32.Parse(mySQLDataType.Substring(commaPos + 1, rParensIndex - commaPos - 1));
        }
      }

      int floatingPointPos = strValue.IndexOf(".");
      bool floatingPointCompliant = true;
      if (floatingPointPos >= 0)
      {
        bool lengthCompliant = strValue.Substring(0, floatingPointPos).Length <= decimalLen[0];
        bool decimalPlacesCompliant = decimalLen[1] >= 0 ? strValue.Substring(floatingPointPos + 1, strValue.Length - floatingPointPos - 1).Length <= decimalLen[1] : true;
        floatingPointCompliant = lengthCompliant && decimalPlacesCompliant;
      }

      if (mySQLDataType.StartsWith("decimal") || mySQLDataType.StartsWith("numeric"))
      {
        decimal tryDecimalValue = 0;
        return Decimal.TryParse(strValue, out tryDecimalValue) && floatingPointCompliant;
      }

      return false;
    }

    /// <summary>
    /// Checks whether values with a given data type can be safely stored in a column with a second data type.
    /// </summary>
    /// <param name="strippedType1">The data type tested to fit within a second data type.</param>
    /// <param name="strippedType2">The second data type where values would fit or not.</param>
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
      List<string> dataTypesList = GetMySQLDataTypes();
      if (!dataTypesList.Contains(strippedType1) || !dataTypesList.Contains(strippedType2))
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

      bool type1IsChar = strippedType1.Contains("char");
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

      return false;
    }

    /// <summary>
    /// Validates that a user typed data type is a valid MySQL data type.
    /// A blank data type is considered valid.
    /// </summary>
    /// <param name="dataType">A MySQL data type as specified for new columns in a CREATE TABLE statement.</param>
    /// <returns>true if the type is a valid MySQL data type, false otherwise.</returns>
    public static bool ValidateUserDataType(string proposedUserType)
    {
      //// If the proposed type is blank return true since a blank data type is considered valid.
      if (proposedUserType.Length == 0)
      {
        return true;
      }

      List<int> validParamsPerDataType;
      List<string> dataTypesList = GetMySQLDataTypes(out validParamsPerDataType);
      int rightParenthesisIndex = proposedUserType.IndexOf(")");
      int leftParenthesisIndex = proposedUserType.IndexOf("(");

      //// Check if we have parenthesis within the proposed data type and if the left and right parentheses are placed properly.
      //// Also check if there is no text beyond the right parenthesis.
      if (rightParenthesisIndex >= 0 && (leftParenthesisIndex < 0 || leftParenthesisIndex >= rightParenthesisIndex || proposedUserType.Length > rightParenthesisIndex + 1))
      {
        return false;
      }

      //// Check if the data type stripped of parenthesis is found in the list of valid MySQL types.
      string pureDataType = rightParenthesisIndex >= 0 ? proposedUserType.Substring(0, leftParenthesisIndex).ToLowerInvariant() : proposedUserType.ToLowerInvariant();
      int typeFoundAt = dataTypesList.IndexOf(pureDataType);
      if (typeFoundAt < 0)
      {
        return false;
      }

      //// Parameters checks.
      bool enumOrSet = pureDataType == "enum" || pureDataType == "set";
      int numOfValidParams = validParamsPerDataType[typeFoundAt];
      if ((numOfValidParams != 0 && rightParenthesisIndex >= 0) || enumOrSet)
      {
        //// If an enum or set the data type must contain parenthesis along with its list of valid values.
        if (enumOrSet && rightParenthesisIndex < 0)
        {
          return false;
        }

        //// Check if the number of parameters is valid for the proposed MySQL data type
        string parametersText = proposedUserType.Substring(leftParenthesisIndex + 1, rightParenthesisIndex - leftParenthesisIndex - 1).Trim();
        string[] parameterValues = string.IsNullOrEmpty(parametersText) ? null : parametersText.Split(',');
        int parametersCount = parameterValues == null ? 0 : parameterValues.Length;

        //// If there are no parameters but parenthesis were provided the data type is invalid.
        if (parametersCount == 0)
        {
          return false;
        }

        //// If the quantity of parameters does not match the data type valid accepted parameters quantity the data type is invalid.
        bool parametersQtyIsValid = enumOrSet ? parametersCount > 0 : numOfValidParams == parametersCount;
        if (!parametersQtyIsValid)
        {
          return false;
        }

        //// Check if the paremeter values are valid integers for data types with 1 or 2 parameters (varchar and numeric types).
        if (!enumOrSet)
        {
          foreach (string paramValue in parameterValues)
          {
            int convertedValue = 0;
            if (!int.TryParse(paramValue, out convertedValue))
            {
              return false;
            }

            //// Specific check for year data type.
            if (pureDataType == "year" && convertedValue != 2 && convertedValue != 4)
            {
              return false;
            }
          }
        }
      }

      return true;
    }
  }

  /// <summary>
  /// Provides extension methods and other static methods to leverage miscelaneous tasks.
  /// </summary>
  public static class MiscUtilities
  {
    /// <summary>
    /// Creates a cursor from a bitmap image.
    /// </summary>
    /// <param name="bmp">Base image for the cursor.</param>
    /// <param name="xHotSpot">The x-coordinate of a cursor's hot spot (normally the upper-left corner of the cursor).</param>
    /// <param name="yHotSpot">The y-coordinate of a cursor's hot spot (normally the upper-left corner of the cursor).</param>
    /// <returns>The cursor created from the given bitmap.</returns>
    public static Cursor CreateCursor(Bitmap bmp, int xHotSpot, int yHotSpot)
    {
      IconInfo tmp = new IconInfo();
      GetIconInfo(bmp.GetHicon(), ref tmp);
      tmp.xHotspot = xHotSpot;
      tmp.yHotspot = yHotSpot;
      tmp.fIcon = false;
      return new Cursor(CreateIconIndirect(ref tmp));
    }

    /// <summary>
    /// Gets a text avoiding duplicates by adding a numeric suffix in case it already exists in the given list.
    /// </summary>
    /// <param name="listOfTexts">The list of texts.</param>
    /// <param name="proposedText">Proposed text.</param>
    /// <returns>Unique text.</returns>
    public static string GetNonDuplicateText(this List<string> listOfTexts, string proposedText)
    {
      if (string.IsNullOrEmpty(proposedText) || listOfTexts == null || listOfTexts.Count == 0)
      {
        return proposedText;
      }

      proposedText = proposedText.Trim();
      string nonDuplicateText = proposedText;
      int textSuffixNumber = 2;
      while (listOfTexts.Exists(text => text == nonDuplicateText))
      {
        nonDuplicateText = proposedText + textSuffixNumber++;
      }

      return nonDuplicateText;
    }

    /// <summary>
    /// Returns the position of a given integer number within an array of integers.
    /// </summary>
    /// <param name="intArray">The array of integers to look for the given number.</param>
    /// <param name="intElement">The integer to look for in the list.</param>
    /// <returns>The ordinal position of the given number within the list, or <c>-1</c> if not found.</returns>
    public static int IndexOfIntInArray(int[] intArray, int intElement)
    {
      int index = -1;

      if (intArray != null)
      {
        for (int i = 0; i < intArray.Length; i++)
        {
          if (intArray[i] == intElement)
          {
            index = i;
            break;
          }
        }
      }

      return index;
    }

    /// <summary>
    /// Returns the position of a given string number within an array of strings.
    /// </summary>
    /// <param name="stringArray">The array of strings to look for the given string.</param>
    /// <param name="stringElement">The string to look for in the list.</param>
    /// <param name="caseSensitive">Flag indicating whether the search is performed in a case sensitive way.</param>
    /// <returns>The ordinal position of the given string within the list, or <c>-1</c> if not found.</returns>
    public static int IndexOfStringInArray(string[] stringArray, string stringElement, bool caseSensitive)
    {
      int index = -1;
      if (!caseSensitive)
      {
        stringElement = stringElement.ToLowerInvariant();
      }

      if (stringArray != null)
      {
        for (int i = 0; i < stringArray.Length; i++)
        {
          bool areEqual = stringElement == (caseSensitive ? stringArray[i] : stringArray[i].ToLowerInvariant());
          if (areEqual)
          {
            index = i;
            break;
          }
        }
      }

      return index;
    }

    /// <summary>
    /// Creates a new bitmap based on a given bitmap with its colors converted to a grayscale color palette.
    /// </summary>
    /// <param name="original">The bitmap to convert to grayscale.</param>
    /// <returns>New grayscale bitmap.</returns>
    public static Bitmap MakeGrayscale(this Bitmap original)
    {
      //// Create a blank bitmap the same size as original
      Bitmap newBitmap = new Bitmap(original.Width, original.Height);

      //// Get a graphics object from the new image
      Graphics g = Graphics.FromImage(newBitmap);

      //// Create the grayscale ColorMatrix
      ColorMatrix colorMatrix = new ColorMatrix(
         new float[][]
      {
         new float[] {.3f, .3f, .3f, 0, 0},
         new float[] {.59f, .59f, .59f, 0, 0},
         new float[] {.11f, .11f, .11f, 0, 0},
         new float[] {0, 0, 0, 1, 0},
         new float[] {0, 0, 0, 0, 1}
      });

      //// Create some image attributes
      ImageAttributes attributes = new ImageAttributes();

      //// Set the color matrix attribute
      attributes.SetColorMatrix(colorMatrix);

      //// Draw the original image on the new image using the grayscale color matrix
      g.DrawImage(original, new Rectangle(0, 0, original.Width, original.Height),
         0, 0, original.Width, original.Height, GraphicsUnit.Pixel, attributes);

      //// Dispose the Graphics object
      g.Dispose();
      return newBitmap;
    }

    /// <summary>
    /// Attempts to save settings values into the settings file.
    /// </summary>
    /// <returns><c>true</c> if the settings file was saved successfully, <c>false</c> otherwise.</returns>
    public static bool SaveSettings()
    {
      string errorMessage = null;

      //// Attempt to save the settings file up to 3 times, if not successful show an error message to users.
      for (int i = 0; i < 3; i++)
      {
        try
        {
          Properties.Settings.Default.Save();
          errorMessage = null;
        }
        catch (Exception ex)
        {
          MySQLSourceTrace.WriteAppErrorToLog(ex);
          errorMessage = ex.Message;
        }
      }

      if (!string.IsNullOrEmpty(errorMessage))
      {
        MiscUtilities.ShowCustomizedErrorDialog(Properties.Resources.SettingsFileSaveErrorTitle, errorMessage);
      }

      return errorMessage == null;
    }

    /// <summary>
    /// Sets the DoubleBuffered property of the control to <c>true</c>.
    /// </summary>
    /// <param name="control">A <see cref="Control"/> object.</param>
    public static void SetDoubleBuffered(this System.Windows.Forms.Control control)
    {
      if (SystemInformation.TerminalServerSession)
      {
        return;
      }

      PropertyInfo aProp =
            typeof(System.Windows.Forms.Control).GetProperty(
                  "DoubleBuffered",
                  System.Reflection.BindingFlags.NonPublic |
                  System.Reflection.BindingFlags.Instance);

      aProp.SetValue(control, true, null);
    }

    /// <summary>
    /// Shows an error dialog customized for MySQL for Excel.
    /// </summary>
    /// <param name="detail">The text describing information details to the users.</param>
    /// <param name="moreInformation">The extended text users can see in the More Information text box.</param>
    /// <param name="wordWrapMoreInfo">Indicates if the More Information text box word wraps the text.</param>
    /// <returns>A dialog result with the user's selection.</returns>
    public static void ShowCustomizedErrorDialog(string detail, string moreInformation = null, bool wordWrapMoreInfo = false)
    {
      ShowCustomizedInfoDialog(InfoDialog.InfoType.Error, detail, moreInformation, wordWrapMoreInfo);
    }

    /// <summary>
    /// Shows a <see cref="InfoDialog"/> dialog customized for MySQL for Excel, only an OK/Back button is displayed to users.
    /// </summary>
    /// <param name="infoType">The type of information the dialog will display to users.</param>
    /// <param name="detail">The text describing information details to the users.</param>
    /// <param name="moreInformation">The extended text users can see in the More Information text box.</param>
    /// <param name="wordWrapMoreInfo">Indicates if the More Information text box word wraps the text.</param>
    /// <returns>A dialog result with the user's selection.</returns>
    public static DialogResult ShowCustomizedInfoDialog(InfoDialog.InfoType infoType, string detail, string moreInformation = null, bool wordWrapMoreInfo = true)
    {
      string title = string.Empty;
      InfoDialog.DialogType dialogType = InfoDialog.DialogType.OKOnly;
      switch (infoType)
      {
        case InfoDialog.InfoType.Success:
          title = Properties.Resources.OperationSuccessTitle;
          break;

        case InfoDialog.InfoType.Warning:
          title = Properties.Resources.OperationWarningTitle;
          break;

        case InfoDialog.InfoType.Error:
          title = Properties.Resources.OperationErrorTitle;
          dialogType = InfoDialog.DialogType.BackOnly;
          break;

        case InfoDialog.InfoType.Info:
          title = Properties.Resources.OperationInformationTitle;
          break;
      }

      string subDetailText = string.Format(Properties.Resources.OperationSubDetailText, infoType == InfoDialog.InfoType.Error ? "Back" : "OK");
      return InfoDialog.ShowDialog(dialogType, infoType, title, detail, subDetailText, moreInformation, wordWrapMoreInfo);
    }

    /// <summary>
    /// Shows a warning dialog customized for MySQL for Excel showing Yes/No buttons.
    /// </summary>
    /// <param name="title">The main short title of the warning.</param>
    /// <param name="detail">The detail text describing further the warning.</param>
    /// <returns>A dialog result with the user's selection.</returns>
    public static DialogResult ShowCustomizedWarningDialog(string title, string detail)
    {
      return InfoDialog.ShowYesNoDialog(InfoDialog.InfoType.Warning, title, detail);
    }

    /// <summary>
    /// Truncates the text given a maximum width and appends an ellipsis at the end of the truncated text.
    /// </summary>
    /// <param name="text">The text to truncate.</param>
    /// <param name="maxWidth">Maximum width to hold the given text.</param>
    /// <param name="graphics">Graphics canvas where the text is rendered.</param>
    /// <param name="font">Font used for the text.</param>
    /// <returns>A new string with the truncated text.</returns>
    public static string TruncateString(this string text, float maxWidth, Graphics graphics, Font font)
    {
      if (string.IsNullOrEmpty(text))
      {
        return text;
      }

      const string ellipsis = "...";
      string newText = text;
      float sizeText = graphics.MeasureString(newText, font).Width;
      if (sizeText > maxWidth)
      {
        int index = (int)((maxWidth / sizeText) * text.Length);
        newText = text.Substring(0, index);
        sizeText = graphics.MeasureString(newText + ellipsis, font).Width;
        if (sizeText < maxWidth)
        {
          while (sizeText < maxWidth)
          {
            newText = text.Substring(0, ++index);
            sizeText = graphics.MeasureString(newText + ellipsis, font).Width;
            if (sizeText > maxWidth)
            {
              newText = newText.Substring(0, newText.Length - 1);
              break;
            }
          }
        }
        else
        {
          while (sizeText > maxWidth)
          {
            newText = text.Substring(0, --index);
            sizeText = graphics.MeasureString(newText + ellipsis, font).Width;
          }
        }

        newText += ellipsis;
      }

      return newText;
    }

    /// <summary>
    /// Creates an icon or cursor from an ICONINFO structure.
    /// </summary>
    /// <param name="pIconInfo">A pointer to an ICONINFO structure the function uses to create the icon or cursor.</param>
    /// <returns>If the function succeeds, the return value is a handle to the icon or cursor that is created. Null if fails.</returns>
    [DllImport("user32.dll")]
    private static extern IntPtr CreateIconIndirect(ref IconInfo pIconInfo);

    /// <summary>
    /// Retrieves information about the specified icon or cursor.
    /// </summary>
    /// <param name="hIcon">A handle to the icon or cursor.</param>
    /// <param name="pIconInfo">A pointer to an ICONINFO structure. The function fills in the structure's members.</param>
    /// <returns><c>true</c> if the function succeeds and the function fills in the members of the specified ICONINFO structure, <c>false</c> otherwise.</returns>
    [DllImport("user32.dll")]
    [return: MarshalAs(UnmanagedType.Bool)]
    private static extern bool GetIconInfo(IntPtr hIcon, ref IconInfo pIconInfo);

    /// <summary>
    /// Contains information about an icon or a cursor.
    /// </summary>
    /// <remarks>DO NOT change the order of the struct elements since C++ expects it in this specific order.</remarks>
    public struct IconInfo
    {
      /// <summary>
      /// Specifies whether this structure defines an icon or a cursor. A value of <c>true</c> specifies an icon; <c>false</c> specifies a cursor.
      /// </summary>
      public bool fIcon;

      /// <summary>
      /// The x-coordinate of a cursor's hot spot. If this structure defines an icon, the hot spot is always in the center of the icon, and this member is ignored.
      /// </summary>
      public int xHotspot;

      /// <summary>
      /// The y-coordinate of the cursor's hot spot. If this structure defines an icon, the hot spot is always in the center of the icon, and this member is ignored.
      /// </summary>
      public int yHotspot;

      /// <summary>
      /// The icon bitmask bitmap. If this structure defines a black and white icon, this bitmask is formatted so that the upper half is
      /// the icon AND bitmask and the lower half is the icon XOR bitmask. Under this condition, the height should be an even multiple of two.
      /// If this structure defines a color icon, this mask only defines the AND bitmask of the icon.
      /// </summary>
      public IntPtr hbmMask;

      /// <summary>
      /// A handle to the icon color bitmap. This member can be optional if this structure defines a black and white icon.
      /// The AND bitmask of hbmMask is applied with the SRCAND flag to the destination; subsequently, the color bitmap is applied
      /// (using XOR) to the destination by using the SRCINVERT flag.
      /// </summary>
      public IntPtr hbmColor;
    }
  }

  /// <summary>
  /// Provides extension methods and other static methods to leverage the work with MySQL data.
  /// </summary>
  public static class MySQLDataUtilities
  {
    /// <summary>
    /// Adds or sets the values on extended properties within the <see cref="DataTable"/> object.
    /// </summary>
    /// <param name="dt">A data table where extended properties are set.</param>
    /// <param name="queryString">The last query string used to produce the result set saved in this data table.</param>
    /// <param name="importedHeaders">Flag indicating if the column names where returned by the query and stored in the first row of the data table.</param>
    /// <param name="tableName">The name of the MySQL table queried to produce the data stored in this data table.</param>
    public static void AddExtendedProperties(this DataTable dt, string queryString, bool importedHeaders, string tableName)
    {
      if (dt.ExtendedProperties.ContainsKey("QueryString"))
      {
        dt.ExtendedProperties["QueryString"] = queryString;
      }
      else
      {
        dt.ExtendedProperties.Add("QueryString", queryString);
      }

      if (dt.ExtendedProperties.ContainsKey("ImportedHeaders"))
      {
        dt.ExtendedProperties["ImportedHeaders"] = importedHeaders;
      }
      else
      {
        dt.ExtendedProperties.Add("ImportedHeaders", importedHeaders);
      }

      if (dt.ExtendedProperties.ContainsKey("TableName"))
      {
        dt.ExtendedProperties["TableName"] = tableName;
      }
      else
      {
        dt.ExtendedProperties.Add("TableName", tableName);
      }
    }

    /// <summary>
    /// Escapes special characters that cause problems when passed within queries, from this data value string.
    /// </summary>
    /// <param name="valueToEscape">The data value text containing special characters.</param>
    /// <returns>A new string built from the given data value string withouth the special characters.</returns>
    public static string EscapeDataValueString(this string valueToEscape)
    {
      const string quotesAndOtherDangerousChars =
          "\\" + "\u2216" + "\uFF3C"               // backslashes
        + "'" + "\u00B4" + "\u02B9" + "\u02BC" + "\u02C8" + "\u02CA"
                + "\u0301" + "\u2019" + "\u201A" + "\u2032"
                + "\u275C" + "\uFF07"            // single-quotes
        + "`" + "\u02CB" + "\u0300" + "\u2018" + "\u2035" + "\u275B"
                + "\uFF40"                       // back-tick
        + "\"" + "\u02BA" + "\u030E" + "\uFF02"; // double-quotes

      StringBuilder sb = new StringBuilder();
      foreach (char c in valueToEscape)
      {
        char escape = char.MinValue;
        switch (c)
        {
          case '\u0000':
            escape = '0';
            break;

          case '\n':
            escape = 'n';
            break;

          case '\r':
            escape = 'r';
            break;

          case '\u001F':
            escape = 'Z';
            break;

          default:
            if (quotesAndOtherDangerousChars.IndexOf(c) >= 0)
            {
              escape = c;
            }

            break;
        }

        if (escape != char.MinValue)
        {
          sb.Append('\\');
          sb.Append(escape);
        }
        else
        {
          sb.Append(c);
        }
      }

      return sb.ToString();
    }

    /// <summary>
    /// Executes a routine and returns all result sets as tables within a dataset.
    /// </summary>
    /// <param name="connection">MySQL Workbench connection to a MySQL server instance selected by users.</param>
    /// <param name="routineName">Qualified routine name (i.e. Schema.Routine).</param>
    /// <param name="routineParameters">Array of arguments passed to the routine parameters.</param>
    /// <returns><see cref="DataSet"/> where each table within it represents each of the result sets returned by the routine.</returns>
    public static DataSet ExecuteRoutine(this MySqlWorkbenchConnection connection, string routineName, params MySqlParameter[] routineParameters)
    {
      // Create & open a SqlConnection, and dispose of it after we are done.
      using (MySqlConnection baseConnection = new MySqlConnection(connection.GetConnectionStringBuilder().ConnectionString))
      {
        baseConnection.Open();

        //// Create a command and prepare it for execution
        MySqlCommand cmd = new MySqlCommand();
        cmd.Connection = baseConnection;
        cmd.CommandText = routineName;
        cmd.CommandType = CommandType.StoredProcedure;

        if (routineParameters != null)
        {
          foreach (MySqlParameter p in routineParameters)
          {
            cmd.Parameters.Add(p);
          }
        }

        //// Create the DataAdapter & DataSet
        MySqlDataAdapter da = new MySqlDataAdapter(cmd);
        DataSet ds = new DataSet();

        //// Fill the DataSet using default values for DataTable names, etc.
        da.Fill(ds);

        //// Detach the MySqlParameters from the command object, so they can be used again.
        cmd.Parameters.Clear();

        //// Return the dataset
        return ds;
      }
    }

    /// <summary>
    /// Executes the given query and returns the result set in a <see cref="DataTable"/> object.
    /// </summary>
    /// <param name="connection">MySQL Workbench connection to a MySQL server instance selected by users.</param>
    /// <param name="query">Select query to be sent to the MySQL Server.</param>
    /// <returns>Table containing the results of the query.</returns>
    public static DataTable GetDataFromTableOrView(this MySqlWorkbenchConnection connection, string query)
    {
      DataTable retTable = null;
      DataSet ds = MySqlHelper.ExecuteDataset(connection.GetConnectionStringBuilder().ConnectionString, query);
      if (ds.Tables.Count > 0)
      {
        retTable = ds.Tables[0];
        retTable.AddExtendedProperties(query, true, string.Empty);
      }

      return retTable;
    }

    /// <summary>
    /// Executes the given query and returns the result set in a <see cref="DataTable"/> object.
    /// </summary>
    /// <remarks>Only works against Tables or Views, but not with Procedures.</remarks>
    /// <param name="connection">MySQL Workbench connection to a MySQL server instance selected by users.</param>
    /// <param name="dbo">Type of database object to query (Table, View or Procedure).</param>
    /// <param name="columnsList">List of queries column names.</param>
    /// <param name="firstRowIdx">Row number from which to start returning results.</param>
    /// <param name="rowCount">Number of rows to return</param>
    /// <returns>Table containing the results of the query.</returns>
    public static DataTable GetDataFromTableOrView(this MySqlWorkbenchConnection connection, DBObject dbo, List<string> columnsList, int firstRowIdx, int rowCount)
    {
      string queryString = AssembleSelectQuery(connection.Schema, dbo, columnsList, firstRowIdx, rowCount);
      return string.IsNullOrEmpty(queryString) ? null : connection.GetDataFromTableOrView(queryString);
    }

    /// <summary>
    /// Executes the given query and returns the result set in a <see cref="DataTable"/> object.
    /// </summary>
    /// <remarks>Only works against Tables or Views, but not with Procedures.</remarks>
    /// <param name="connection">MySQL Workbench connection to a MySQL server instance selected by users.</param>
    /// <param name="dbo">Type of database object to query (Table, View or Procedure).</param>
    /// <param name="columnsList">List of queries column names.</param>
    /// <returns>Table containing the results of the query.</returns>
    public static DataTable GetDataFromTableOrView(this MySqlWorkbenchConnection connection, DBObject dbo, List<string> columnsList)
    {
      return GetDataFromTableOrView(connection, dbo, columnsList, -1, -1);
    }

    /// <summary>
    /// Executes the given procedure and returns its result sets in tables within a <see cref="DataSet"/> object.
    /// </summary>
    /// <remarks>Only works against Procedures, but not with Tables or Views.</remarks>
    /// <param name="connection">MySQL Workbench connection to a MySQL server instance selected by users.</param>
    /// <param name="dbo">Type of database object to query (Table, View or Procedure)</param>
    /// <param name="parameters">Array of arguments passed to the stored procedure parameters.</param>
    /// <returns><see cref="DataSet"/> where each table within it represents each of the result sets returned by the stored procedure.</returns>
    public static DataSet GetDataSetFromProcedure(this MySqlWorkbenchConnection connection, DBObject dbo, params MySqlParameter[] parameters)
    {
      DataSet retDS = null;

      if (dbo.Type == DBObject.DBObjectType.Procedure)
      {
        string sql = string.Format("`{0}`.`{1}`", connection.Schema, dbo.Name);
        retDS = connection.ExecuteRoutine(sql, parameters);
      }

      return retDS;
    }

    /// <summary>
    /// Gets the value of the MAX_ALLOWED_PACKET MySQL Server variable indicating the max size in bytes of the packet returned by a single query.
    /// </summary>
    /// <param name="connection">MySQL Workbench connection to a MySQL server instance selected by users.</param>
    /// <returns>The max size in bytes of the packet returned by a single query.</returns>
    public static ulong GetMySQLServerMaxAllowedPacket(this MySqlWorkbenchConnection connection)
    {
      string sql = "SELECT @@max_allowed_packet";
      object objCount = MySqlHelper.ExecuteScalar(connection.GetConnectionStringBuilder().ConnectionString, sql);
      return objCount != null ? (ulong)objCount : 0;
    }

    /// <summary>
    /// Gets the value of the MAX_ALLOWED_PACKET MySQL Server variable indicating the max size in bytes of the packet returned by a single query.
    /// </summary>
    /// <param name="connection">A MySQL connection.</param>
    /// <returns>The max size in bytes of the packet returned by a single query.</returns>
    public static ulong GetMySQLServerMaxAllowedPacket(this MySqlConnection connection)
    {
      string sql = "SELECT @@max_allowed_packet";
      object objCount = MySqlHelper.ExecuteScalar(connection, sql);
      return objCount != null ? (ulong)objCount : 0;
    }

    /// <summary>
    /// Gets the total number of rows contained in a table or view.
    /// </summary>
    /// <remarks>Only works against Tables or Views, but not with Procedures.</remarks>
    /// <param name="connection">MySQL Workbench connection to a MySQL server instance selected by users.</param>
    /// <param name="dbo">Type of database object to query (Table, View or Procedure).</param>
    /// <returns>The number of rows in a given table or view.</returns>
    public static long GetRowsCountFromTableOrView(this MySqlWorkbenchConnection connection, DBObject dbo)
    {
      if (dbo.Type == DBObject.DBObjectType.Procedure)
      {
        return 0;
      }

      string sql = string.Format("SELECT COUNT(*) FROM `{0}`.`{1}`", connection.Schema, dbo.Name);
      object objCount = MySqlHelper.ExecuteScalar(connection.GetConnectionStringBuilder().ConnectionString, sql);
      return objCount != null ? (long)objCount : 0;
    }

    /// <summary>
    /// Gets the schema information ofr the given database collection.
    /// </summary>
    /// <param name="connection">MySQL Workbench connection to a MySQL server instance selected by users.</param>
    /// <param name="collection">The type of database collection to return schema information for.</param>
    /// <param name="restrictions">Specific parameters that vary among database collections.</param>
    /// <returns>Schema information within a data table.</returns>
    public static DataTable GetSchemaCollection(this MySqlWorkbenchConnection connection, string collection, params string[] restrictions)
    {
      string connectionString = connection.GetConnectionStringBuilder().ConnectionString;
      DataTable dt = null;
      MySqlDataAdapter mysqlAdapter = null;

      try
      {
        using (MySqlConnection baseConnection = new MySqlConnection(connectionString))
        {
          baseConnection.Open();

          switch (collection.ToUpperInvariant())
          {
            case "COLUMNS SHORT":
              mysqlAdapter = new MySqlDataAdapter(string.Format("SHOW COLUMNS FROM `{0}`.`{1}`", restrictions[1], restrictions[2]), baseConnection);
              dt = new DataTable();
              mysqlAdapter.Fill(dt);
              break;

            case "ENGINES":
              mysqlAdapter = new MySqlDataAdapter("SELECT * FROM information_schema.engines ORDER BY engine", baseConnection);
              dt = new DataTable();
              mysqlAdapter.Fill(dt);
              break;

            case "COLLATIONS":
              string queryString;
              if (restrictions != null && restrictions.Length > 0 && !string.IsNullOrEmpty(restrictions[0]))
              {
                queryString = string.Format("SHOW COLLATION WHERE charset = '{0}'", restrictions[0]);
              }
              else
              {
                queryString = "SHOW COLLATION";
              }

              mysqlAdapter = new MySqlDataAdapter(queryString, baseConnection);
              dt = new DataTable();
              mysqlAdapter.Fill(dt);
              break;

            case "CHARSETS":
              mysqlAdapter = new MySqlDataAdapter("SHOW CHARSET", baseConnection);
              dt = new DataTable();
              mysqlAdapter.Fill(dt);
              break;

            default:
              dt = baseConnection.GetSchema(collection, restrictions);
              break;
          }
        }
      }
      catch (Exception ex)
      {
        MySQLSourceTrace.WriteAppErrorToLog(ex);
        throw;
      }

      return dt;
    }

    /// <summary>
    /// Checks if an index with the given name exists in the given schema and table.
    /// </summary>
    /// <param name="connection">MySQL Workbench connection to a MySQL server instance selected by users.</param>
    /// <param name="schemaName">Name of the database schema where the index resides.</param>
    /// <param name="tableName">Name of the database table where the index resides.</param>
    /// <param name="indexName">Name of the index to look for.</param>
    /// <returns><c>true</c> if the index exists, <c>false</c> otherwise.</returns>
    public static bool IndexExistsInSchema(this MySqlWorkbenchConnection connection, string schemaName, string tableName, string indexName)
    {
      if (string.IsNullOrEmpty(schemaName) || string.IsNullOrEmpty(indexName))
      {
        return false;
      }

      DataTable dt = GetSchemaCollection(connection, "Indexes", null, schemaName, tableName, indexName);
      return dt.Rows.Count > 0;
    }

    /// <summary>
    /// Checks if the given connection may be using SSL.
    /// </summary>
    /// <param name="connection">MySQL Workbench connection to a MySQL server instance selected by users.</param>
    /// <returns><c>true</c> if the connection uses SSL, <c>false</c> otherwise.</returns>
    public static bool IsSSL(this MySqlWorkbenchConnection connection)
    {
      return connection.UseSSL == 1
        || !(string.IsNullOrWhiteSpace(connection.SSLCA)
        && string.IsNullOrWhiteSpace(connection.SSLCert)
        && string.IsNullOrWhiteSpace(connection.SSLCipher)
        && string.IsNullOrWhiteSpace(connection.SSLKey));
    }

    /// <summary>
    /// Checks if a table with the given name exists in the given schema.
    /// </summary>
    /// <param name="connection">MySQL Workbench connection to a MySQL server instance selected by users.</param>
    /// <param name="schemaName">Name of the database schema where the table resides.</param>
    /// <param name="tableName">Name of the table to look for.</param>
    /// <returns><c>true</c> if the table exists, <c>false</c> otherwise.</returns>
    public static bool TableExistsInSchema(this MySqlWorkbenchConnection connection, string schemaName, string tableName)
    {
      if (string.IsNullOrEmpty(schemaName) || string.IsNullOrEmpty(tableName))
      {
        return false;
      }

      string sql = string.Format("SELECT COUNT(*) FROM information_schema.tables WHERE table_schema = '{0}' AND table_name = '{1}'", schemaName, tableName.EscapeDataValueString());
      object objCount = MySqlHelper.ExecuteScalar(connection.GetConnectionStringBuilder().ConnectionString, sql);
      long retCount = objCount != null ? (long)objCount : 0;
      return retCount > 0;
    }

    /// <summary>
    /// Checks if a table with the given name has a primary key defined.
    /// </summary>
    /// <param name="connection">MySQL Workbench connection to a MySQL server instance selected by users.</param>
    /// <param name="tableName">Name of the table.</param>
    /// <returns><c>true</c> if the table has a primary key, <c>false</c> otherwise.</returns>
    public static bool TableHasPrimaryKey(this MySqlWorkbenchConnection connection, string tableName)
    {
      if (string.IsNullOrEmpty(tableName))
      {
        return false;
      }

      string sql = string.Format("SHOW KEYS FROM `{0}` IN `{1}` WHERE Key_name = 'PRIMARY';", tableName, connection.Schema);
      DataTable dt = GetDataFromTableOrView(connection, sql);
      return dt != null ? dt.Rows.Count > 0 : false;
    }

    /// <summary>
    /// Tests the given connection to check if it can successfully connect to the corresponding MySQL instance.
    /// </summary>
    /// <param name="connection">MySQL Workbench connection to a MySQL server instance selected by users.</param>
    /// <param name="wrongPassword">Flag indicating if the reason for an unsuccessful connection is because of a bad password.</param>
    /// <returns><c>true</c> if successfully connects, <c>false</c> otherwise.</returns>
    public static bool TestConnectionAndShowError(this MySqlWorkbenchConnection connection, out bool wrongPassword)
    {
      wrongPassword = false;
      Exception connectionException = null;
      if (connection.TestConnection(out connectionException))
      {
        return true;
      }

      //// If the error returned is about the connection failing the password check, it may be because either the stored password is wrong or no password.
      if (connectionException is MySqlException && (connectionException as MySqlException).Number == 0)
      {
        if (!string.IsNullOrEmpty(connection.Password))
        {
          string moreInfoText = connection.IsSSL() ? Properties.Resources.ConnectSSLFailedDetailWarning : null;
          InfoDialog.ShowWarningDialog(Properties.Resources.ConnectFailedWarningTitle, connectionException.Message, null, moreInfoText);
        }

        wrongPassword = true;
      }
      else
      {
        InfoDialog.ShowErrorDialog(Properties.Resources.ConnectFailedWarningTitle, connectionException.Message, null, connectionException.InnerException != null ? connectionException.InnerException.Message : null);
      }

      return false;
    }

    /// <summary>
    /// Creates a SELECT query against a Table or View database object.
    /// </summary>
    /// <remarks>Only works against Tables or Views, but not with Procedures.</remarks>
    /// <param name="schemaName">Name of the schema (database) where the Table or View resides.</param>
    /// <param name="dbo">Type of database object to query (Table, View or Procedure).</param>
    /// <param name="columnsList">List of queries column names.</param>
    /// <param name="firstRowIdx">Row number from which to start returning results.</param>
    /// <param name="rowCount">Number of rows to return</param>
    /// <returns>The SELECT query text.</returns>
    private static string AssembleSelectQuery(string schemaName, DBObject dbo, List<string> columnsList, int firstRowIdx, int rowCount)
    {
      if (dbo.Type == DBObject.DBObjectType.Procedure)
      {
        return null;
      }

      StringBuilder queryStringBuilder = new StringBuilder("SELECT ");
      if (columnsList == null || columnsList.Count == 0)
      {
        queryStringBuilder.Append("*");
      }
      else
      {
        foreach (string columnName in columnsList)
        {
          queryStringBuilder.AppendFormat("`{0}`,", columnName.Replace("`", "``"));
        }

        queryStringBuilder.Remove(queryStringBuilder.Length - 1, 1);
      }

      queryStringBuilder.AppendFormat(" FROM `{0}`.`{1}`", schemaName, dbo.Name);
      if (firstRowIdx > 0)
      {
        string strCount = rowCount >= 0 ? rowCount.ToString() : "18446744073709551615";
        queryStringBuilder.AppendFormat(" LIMIT {0},{1}", firstRowIdx, strCount);
      }
      else if (rowCount >= 0)
      {
        queryStringBuilder.AppendFormat(" LIMIT {0}", rowCount);
      }

      return queryStringBuilder.ToString();
    }
  }
}