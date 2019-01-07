// Copyright (c) 2015, 2018, Oracle and/or its affiliates. All rights reserved.
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
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Windows.Forms;

namespace MySQL.ForExcel.Classes
{
  /// <summary>
  /// Specifies attributes that represent a MySQL valid data type.
  /// </summary>
  public sealed class MySqlDisplayDataType
  {
    #region Fields

    /// <summary>
    /// A static dictionary containing names and descriptions for all permitted MySQL data types.
    /// </summary>
    private static Dictionary<string, string> _allDataTypesDictionary;

    /// <summary>
    /// A static list containing the names of all allowed base MySQL data types.
    /// </summary>
    private static List<string> _baseTypeNamesList;

    /// <summary>
    /// A static dictionary containing names and descriptions for commonly used MySQL data types.
    /// </summary>
    private static Dictionary<string, string> _commonDataTypesDictionary;

    /// <summary>
    /// A static list containing all allowed MySQL data types.
    /// </summary>
    private static List<MySqlDisplayDataType> _dataTypesList;

    /// <summary>
    /// The name and description for this data type.
    /// </summary>
    private string _nameAndDescription;

    #endregion Fields

    /// <summary>
    /// Initializes the <see cref="MySqlDisplayDataType"/> class.
    /// </summary>
    static MySqlDisplayDataType()
    {
      _allDataTypesDictionary = null;
      _baseTypeNamesList = null;
      _commonDataTypesDictionary = null;
      _dataTypesList = null;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="MySqlDisplayDataType"/> class.
    /// </summary>
    /// <param name="name">The MySQL data type name.</param>
    /// <param name="description">A friendly description for the data type.</param>
    /// <param name="isBaseType">Flag indicating whether the data type is a base type without other specifiers.</param>
    /// <param name="display">Flag indicating whether the data type will be displayed as an option for users to select.</param>
    /// <param name="isCommon">Flag indicating whether the data type is a common one.</param>
    private MySqlDisplayDataType(string name, string description, bool isBaseType, bool display, bool isCommon)
    {
      _nameAndDescription = null;
      IsBaseType = isBaseType;
      Display = display;
      Name = name;
      Description = description;
      IsCommon = isCommon;
    }

    #region Properties

    /// <summary>
    /// Gets a static dictionary containing names and descriptions for all permitted MySQL data types.
    /// </summary>
    public static Dictionary<string, string> AllDataTypesDictionary
    {
      get
      {
        if (_allDataTypesDictionary != null)
        {
          return _allDataTypesDictionary;
        }

        _allDataTypesDictionary = new Dictionary<string, string>(DataTypesList.Count(t => t.Display));
        foreach (var displayDataType in DataTypesList.Where(displayDataType => displayDataType.Display))
        {
          _allDataTypesDictionary.Add(displayDataType.Name, displayDataType.NameAndDescription);
        }

        return _allDataTypesDictionary;
      }
    }

    /// <summary>
    /// Gets a static list containing the names of all allowed base MySQL data types.
    /// </summary>
    public static List<string> BaseTypeNamesList
    {
      get
      {
        if (_baseTypeNamesList != null)
        {
          return _baseTypeNamesList;
        }

        _baseTypeNamesList = new List<string>(DataTypesList.Count);
        foreach (var mySqlDataType in DataTypesList.Where(mySqlDataType => mySqlDataType.IsBaseType))
        {
          _baseTypeNamesList.Add(mySqlDataType.Name.ToLowerInvariant());
        }

        return _baseTypeNamesList;
      }
    }

    /// <summary>
    /// Gets a static dictionary containing names and descriptions for commonly used MySQL data types.
    /// </summary>
    public static Dictionary<string, string> CommonDataTypesDictionary
    {
      get
      {
        if (_commonDataTypesDictionary != null)
        {
          return _commonDataTypesDictionary;
        }

        _commonDataTypesDictionary = new Dictionary<string, string>(DataTypesList.Count(t => t.Display && t.IsCommon));
        foreach (var displayDataType in DataTypesList.Where(displayDataType => displayDataType.Display && displayDataType.IsCommon))
        {
          _commonDataTypesDictionary.Add(displayDataType.Name, displayDataType.NameAndDescription);
        }

        return _commonDataTypesDictionary;
      }
    }

    /// <summary>
    /// Gets a static list containing all allowed MySQL data types.
    /// </summary>
    public static List<MySqlDisplayDataType> DataTypesList => _dataTypesList ?? (_dataTypesList = new List<MySqlDisplayDataType>(50)
    {
      // Commonly displayed data types
      new MySqlDisplayDataType("Integer", "Default for whole-number columns", true, true, true),
      new MySqlDisplayDataType("VarChar(5)", "Small string up to 5 characters", false, true, true),
      new MySqlDisplayDataType("VarChar(12)", "Small string up to 12 characters", false, true, true),
      new MySqlDisplayDataType("VarChar(25)", "Small string up to 25 characters", false, true, true),
      new MySqlDisplayDataType("VarChar(45)", "Standard string up to 45 characters", false, true, true),
      new MySqlDisplayDataType("VarChar(255)", "Standard string up to 255 characters", false, true, true),
      new MySqlDisplayDataType("VarChar(4000)", "Large string up to 4k characters", false, true, true),
      new MySqlDisplayDataType("Text", "Maximum string up to 65k characters", true, true, true),
      new MySqlDisplayDataType("DateTime", "For columns that store both, date and time", true, true, true),
      new MySqlDisplayDataType("Date", "For columns that only store a date", true, true, true),
      new MySqlDisplayDataType("Time", "For columns that only store a time", true, true, true),
      new MySqlDisplayDataType("Bool", "Holds values like (0, 1), (True, False) or (Yes, No)", true, true, true),
      new MySqlDisplayDataType("BigInt", "For columns containing large whole-number integers with up to 19 digits", true, true, true),
      new MySqlDisplayDataType("Decimal(12, 2)", "Exact decimal numbers with 12 digits, 2 of them after the decimal point", false, true, true),
      new MySqlDisplayDataType("Decimal(65, 30)", "Biggest exact decimal numbers with 65 digits, 30 of them after the decimal point", false, true, true),
      new MySqlDisplayDataType("Double", "Biggest float pointing number with approximately 15 decimal places", true, true, true),

      // Other data types to be displayed
      new MySqlDisplayDataType("Bit", "For columns containing numbers in binary notation", true, true, false),
      new MySqlDisplayDataType("Enum", "Holds values from a specified list of enumerated permitted values", true, true, false),
      new MySqlDisplayDataType("Set", "String that can have zero or more values out of a list of permitted values", true, true, false),
      new MySqlDisplayDataType("JSON", "Enables efficient access to data in JSON (JavaScript Object Notation) documents.", true, true, false),
      new MySqlDisplayDataType("TinyInt", "For columns containing tiny whole-number integers with up to  digits", true, true, false),
      new MySqlDisplayDataType("SmallInt", "For columns containing small whole-number integers with up to 5 digits", true, true, false),
      new MySqlDisplayDataType("MediumInt", "For columns containing medium whole-number integers with up to 7 digits", true, true, false),
      new MySqlDisplayDataType("Float", "Floating point number with approximately 7 decimal places", true, true, false),
      new MySqlDisplayDataType("Decimal", "For exact decimal numbers", true, true, false),
      new MySqlDisplayDataType("TimeStamp", "For columns that store both, date and time in UTC format", true, true, false),
      new MySqlDisplayDataType("Year", "For years in 2 or 4 digit format", true, true, false),
      new MySqlDisplayDataType("Char", "For fixed-lenght strings up to 255 characters", true, true, false),
      new MySqlDisplayDataType("TinyText", "Maximum string up to 255 characters", true, true, false),
      new MySqlDisplayDataType("MediumText", "Maximum string up to 16M characters", true, true, false),
      new MySqlDisplayDataType("LongText", "Maximum string up to 4G characters", true, true, false),
      new MySqlDisplayDataType("Binary", "For fixed-lenght binary data up to 255 bytes", true, true, false),
      new MySqlDisplayDataType("VarBinary", "For variable-length binary data", true, true, false),
      new MySqlDisplayDataType("TinyBlob", "For binary large objects up to 256 bytes", true, true, false),
      new MySqlDisplayDataType("Blob", "For binary large objects up to 65 Kb", true, true, false),
      new MySqlDisplayDataType("MediumBlob", "For binary large objects up to 16 Mb", true, true, false),
      new MySqlDisplayDataType("LongBlob", "For binary large objects up to 4 Gb", true, true, false),
      new MySqlDisplayDataType("Geometry", "For spatial data, base type for all geometry values", true, true, false),
      new MySqlDisplayDataType("GeometryCollection", "For spatial data, a collection of one or more geometries of any type", true, true, false),
      new MySqlDisplayDataType("LineString", "For spatial data, a Curve with linear interpolation between points", true, true, false),
      new MySqlDisplayDataType("MultiLineString", "For spatial data, a geometry collection composed of LineString elements", true, true, false),
      new MySqlDisplayDataType("MultiPoint", "For spatial data, a geometry collection composed of Point elements", true, true, false),
      new MySqlDisplayDataType("MultiPolygon", "For spatial data, a geometry collection composed of Polygon elements", true, true, false),
      new MySqlDisplayDataType("Point", "For spatial data, a geometry that represents a single location in coordinate space", true, true, false),
      new MySqlDisplayDataType("Polygon", "For spatial data, a planar Surface representing a multi-sided geometry", true, true, false),

      // Other data types not to be displayed
      new MySqlDisplayDataType("Int", "Same as Integer", true, false, false),
      new MySqlDisplayDataType("Numeric", "Same as Decimal", true, false, false),
      new MySqlDisplayDataType("Fixed", "Same as Decimal", true, false, false),
      new MySqlDisplayDataType("Real", "Same as Double)", true, false, false),
      new MySqlDisplayDataType("Double Precision", "Same as Double)", true, false, false),
      new MySqlDisplayDataType("Boolean", "Same as Bool)", true, false, false),
      new MySqlDisplayDataType("VarChar", "For variable-length strings", true, false, false)
    });

    /// <summary>
    /// Gets a friendly description for the data type.
    /// </summary>
    public string Description { get; }

    /// <summary>
    /// Gets or sets a value indicating whether the data type will be displayed as an option for users to select.
    /// </summary>
    public bool Display { get; set; }

    /// <summary>
    /// Gets a value indicating whether the data type is a base type without other specifiers.
    /// </summary>
    public bool IsBaseType { get; }

    /// <summary>
    /// Gets a value indicating whether the data type is a common one.
    /// </summary>
    public bool IsCommon { get; set; }

    /// <summary>
    /// Gets the MySQL data type name.
    /// </summary>
    public string Name { get; }

    /// <summary>
    /// Gets the name and description for this data type.
    /// </summary>
    public string NameAndDescription
    {
      get
      {
        if (String.IsNullOrEmpty(_nameAndDescription))
        {
          _nameAndDescription = Name + " - " + Description;
        }

        return _nameAndDescription;
      }
    }

    #endregion Properties

    /// <summary>
    /// Gets the length, in pixels, of the longest description among the specified dictionary of MySQL data types.
    /// </summary>
    /// <param name="commonTypesOnly">Flag indicating whether the dictionary of common types or the one with all types is used.</param>
    /// <param name="font">The <see cref="Font"/> used to draw the text.</param>
    /// <param name="addedPadding">Length, in pixels, of any padding to add to the computed length.</param>
    /// <returns>The length, in pixels, of the longest description among the specified dictionary of MySQL data types.</returns>
    public static int GetCommonDataTypesLongestDescriptionLength(bool commonTypesOnly, Font font, int addedPadding = 0)
    {
      var longestLength = 0;
      var typesDictionary = commonTypesOnly ? CommonDataTypesDictionary : AllDataTypesDictionary;
      foreach (var dicItem in typesDictionary)
      {
        longestLength = Math.Max(longestLength, TextRenderer.MeasureText(dicItem.Value, font).Width);
      }

      return longestLength + addedPadding;
    }

    /// <summary>
    /// Gets a corresponding <see cref="MySqlDisplayDataType"/> from the given <see cref="MySqlDataType"/>.
    /// </summary>
    /// <param name="fromMySqlDataType">A <see cref="MySqlDataType"/> instance.</param>
    /// <returns>A corresponding <see cref="MySqlDisplayDataType"/>.</returns>
    public static MySqlDisplayDataType GetDisplayType(MySqlDataType fromMySqlDataType)
    {
      return fromMySqlDataType == null ? null : DataTypesList.FirstOrDefault(dispType => dispType.Name.Equals(fromMySqlDataType.TypeName, StringComparison.InvariantCultureIgnoreCase));
    }

    /// <summary>
    /// Gets the type name as is displayed in dialogs.
    /// </summary>
    /// <param name="typeName">A MySQL type name.</param>
    /// <param name="isValid">Flag indicating whether the supplied type name is a valid one.</param>
    /// <returns>The type name as is displayed in dialogs.</returns>
    public static string GetDisplayTypeName(string typeName, out bool isValid)
    {
      if (string.IsNullOrEmpty(typeName))
      {
        isValid = true;
        return string.Empty;
      }

      var mySqlDisplayType = DataTypesList.FirstOrDefault(mType => mType.IsBaseType && mType.Name.Equals(typeName, StringComparison.InvariantCultureIgnoreCase));
      isValid = mySqlDisplayType != null;
      return isValid
        ? mySqlDisplayType.Name
        : CultureInfo.CurrentCulture.TextInfo.ToTitleCase(typeName);
    }

    /// <summary>
    /// Validates the given data type name is a valid MySQL one.
    /// </summary>
    /// <param name="typeName">The name of the data type.</param>
    /// <remarks>A blank data type is considered valid.</remarks>
    /// <returns><c>true</c> if the given type is a valid MySQL data type, <c>false</c> otherwise.</returns>
    public static bool ValidateTypeName(string typeName)
    {
      GetDisplayTypeName(typeName, out var isValid);
      return isValid;
    }
  }
}
