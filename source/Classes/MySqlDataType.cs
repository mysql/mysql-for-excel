// Copyright (c) 2014, 2015, Oracle and/or its affiliates. All rights reserved.
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
using System.Linq;
using System.Windows.Forms;

namespace MySQL.ForExcel.Classes
{
  /// <summary>
  /// Specifies attributes that represent a MySQL valid data type.
  /// </summary>
  public class MySqlDataType
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
    private static List<MySqlDataType> _dataTypesList;

    /// <summary>
    /// The name and description for this data type.
    /// </summary>
    private string _nameAndDescription;

    #endregion Fields

    /// <summary>
    /// Initializes the <see cref="MySqlDataType"/> class.
    /// </summary>
    static MySqlDataType()
    {
      _allDataTypesDictionary = null;
      _baseTypeNamesList = null;
      _commonDataTypesDictionary = null;
      _dataTypesList = null;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="MySqlDataType"/> class.
    /// </summary>
    /// <param name="name">The MySQL data type name.</param>
    /// <param name="description">A friendly description for the data type.</param>
    /// <param name="parametersCount">The total allowed parameters or attributes inside parenthesis next to the data type name.</param>
    /// <param name="isBaseType">Flag indicating whether the data type is a base type without other specifiers.</param>
    /// <param name="display">Flag indicating whether the data type will be displayed as an option for users to select.</param>
    /// <param name="isCommon">Flag indicating whether the data type is a common one.</param>
    public MySqlDataType(string name, string description, int parametersCount, bool isBaseType, bool display, bool isCommon)
    {
      _nameAndDescription = null;
      IsBaseType = isBaseType;
      Display = display;
      Name = name;
      Description = description;
      IsCommon = isCommon;
      ParametersCount = parametersCount;
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
    public static List<MySqlDataType> DataTypesList
    {
      get
      {
        return _dataTypesList ?? (_dataTypesList = new List<MySqlDataType>(50)
        {
          // Commonly displayed data types
          new MySqlDataType("Integer", "Default for whole-number columns", 1, true, true, true),
          new MySqlDataType("VarChar(5)", "Small string up to 5 characters", 1, false, true, true),
          new MySqlDataType("VarChar(12)", "Small string up to 12 characters", 1, false, true, true),
          new MySqlDataType("VarChar(25)", "Small string up to 25 characters", 1, false, true, true),
          new MySqlDataType("VarChar(45)", "Standard string up to 45 characters", 1, false, true, true),
          new MySqlDataType("VarChar(255)", "Standard string up to 255 characters", 1, false, true, true),
          new MySqlDataType("VarChar(4000)", "Large string up to 4k characters", 1, false, true, true),
          new MySqlDataType("Text", "Maximum string up to 65k characters", 0, true, true, true),
          new MySqlDataType("DateTime", "For columns that store both, date and time", 0, true, true, true),
          new MySqlDataType("Date", "For columns that only store a date", 0, true, true, true),
          new MySqlDataType("Time", "For columns that only store a time", 0, true, true, true),
          new MySqlDataType("Bool", "Holds values like (0, 1), (True, False) or (Yes, No)", 0, true, true, true),
          new MySqlDataType("BigInt", "For columns containing large whole-number integers with up to 19 digits", 1, true, true, true),
          new MySqlDataType("Decimal(12, 2)", "Exact decimal numbers with 12 digits, 2 of them after the decimal point", 2, false, true, true),
          new MySqlDataType("Decimal(65, 30)", "Biggest exact decimal numbers with 65 digits, 30 of them after the decimal point", 2, false, true, true),
          new MySqlDataType("Double", "Biggest float pointing number with approximately 15 decimal places", 2, true, true, true),

          // Other data types to be displayed
          new MySqlDataType("Bit", "For columns containing numbers in binary notation", 1, true, true, false),
          new MySqlDataType("Enum", "Holds values from a specified list of enumerated permitted values", -1, true, true, false),
          new MySqlDataType("Set", "String that can have zero or more values out of a list of permitted values", -1, true, true, false),
          new MySqlDataType("JSON", "Enables efficient access to data in JSON (JavaScript Object Notation) documents.", 0, true, true, false),
          new MySqlDataType("TinyInt", "For columns containing tiny whole-number integers with up to  digits", 1, true, true, false),
          new MySqlDataType("SmallInt", "For columns containing small whole-number integers with up to 5 digits", 1, true, true, false),
          new MySqlDataType("MediumInt", "For columns containing medium whole-number integers with up to 7 digits", 1, true, true, false),
          new MySqlDataType("Float", "Floating point number with approximately 7 decimal places", 2, true, true, false),
          new MySqlDataType("Decimal", "For exact decimal numbers", 2, true, true, false),
          new MySqlDataType("Year", "For years in 2 or 4 digit format", 1, true, true, false),
          new MySqlDataType("Char", "For fixed-lenght strings up to 255 characters", 1, true, true, false),
          new MySqlDataType("TinyText", "Maximum string up to 255 characters", 0, true, true, false),
          new MySqlDataType("MediumText", "Maximum string up to 16M characters", 0, true, true, false),
          new MySqlDataType("LongText", "Maximum string up to 4G characters", 0, true, true, false),
          new MySqlDataType("Binary", "For fixed-lenght binary data up to 255 bytes", 1, true, true, false),
          new MySqlDataType("VarBinary", "For variable-length binary data", 1, true, true, false),
          new MySqlDataType("TinyBlob", "For binary large objects up to 256 bytes", 0, true, true, false),
          new MySqlDataType("Blob", "For binary large objects up to 65 Kb", 0, true, true, false),
          new MySqlDataType("MediumBlob", "For binary large objects up to 16 Mb", 0, true, true, false),
          new MySqlDataType("LongBlob", "For binary large objects up to 4 Gb", 0, true, true, false),
          new MySqlDataType("Curve", "For spatial data, one-dimensional geometry represented by a sequence of points", 0, true, true, false),
          new MySqlDataType("Geometry", "For spatial data, base type for all geometry values", 0, true, true, false),
          new MySqlDataType("GeometryCollection", "For spatial data, a collection of one or more geometries of any type", 0, true, true, false),
          new MySqlDataType("LineString", "For spatial data, a Curve with linear interpolation between points", 0, true, true, false),
          new MySqlDataType("MultiCurve", "For spatial data, a geometry collection composed of Curve elements", 0, true, true, false),
          new MySqlDataType("MultiLineString", "For spatial data, a geometry collection composed of LineString elements", 0, true, true, false),
          new MySqlDataType("MultiPoint", "For spatial data, a geometry collection composed of Point elements", 0, true, true, false),
          new MySqlDataType("MultiPolygon", "For spatial data, a geometry collection composed of Polygon elements", 0, true, true, false),
          new MySqlDataType("MultiSurface", "For spatial data, a geometry collection composed of Surface elements", 0, true, true, false),
          new MySqlDataType("Point", "For spatial data, a geometry that represents a single location in coordinate space", 0, true, true, false),
          new MySqlDataType("Polygon", "For spatial data, a planar Surface representing a multi-sided geometry", 0, true, true, false),
          new MySqlDataType("Surface", "For spatial data, a base type two-dimensional geometry", 0, true, true, false),

          // Other data types not to be displayed
          new MySqlDataType("Int", "Same as Integer", 1, true, false, false),
          new MySqlDataType("Numeric", "Same as Decimal", 2, true, false, false),
          new MySqlDataType("Real", "Same as Double)", 2, true, false, false),
          new MySqlDataType("Boolean", "Same as Bool)", 0, true, false, false),
          new MySqlDataType("Timestamp", "Same as DateTime", 0, true, false, false),
          new MySqlDataType("VarChar", "For variable-length strings", 1, true, false, false)
        });
      }
    }

    /// <summary>
    /// Gets a friendly description for the data type.
    /// </summary>
    public string Description { get; private set; }

    /// <summary>
    /// Gets or sets a value indicating whether the data type will be displayed as an option for users to select.
    /// </summary>
    public bool Display { get; set; }

    /// <summary>
    /// Gets a value indicating whether the data type is a base type without other specifiers.
    /// </summary>
    public bool IsBaseType { get; private set; }

    /// <summary>
    /// Gets a value indicating whether the data type is a common one.
    /// </summary>
    public bool IsCommon { get; set; }

    /// <summary>
    /// Gets the MySQL data type name.
    /// </summary>
    public string Name { get; private set; }

    /// <summary>
    /// Gets the name and description for this data type.
    /// </summary>
    public string NameAndDescription
    {
      get
      {
        if (string.IsNullOrEmpty(_nameAndDescription))
        {
          _nameAndDescription = Name + " - " + Description;
        }

        return _nameAndDescription;
      }
    }

    /// <summary>
    /// Gets the total allowed parameters or attributes inside parenthesis next to the data type name.
    /// </summary>
    public int ParametersCount { get; private set; }

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
      int longestLength = 0;
      var typesDictionary = commonTypesOnly ? CommonDataTypesDictionary : AllDataTypesDictionary;
      foreach (var dicItem in typesDictionary)
      {
        longestLength = Math.Max(longestLength, TextRenderer.MeasureText(dicItem.Value, font).Width);
      }

      return longestLength + addedPadding;
    }
  }
}
