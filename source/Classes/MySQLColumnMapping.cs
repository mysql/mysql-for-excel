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
using System.Data;
using System.Linq;
using System.Xml.Serialization;

namespace MySQL.ForExcel.Classes
{
  /// <summary>
  /// Represents a structure with mappings between source Excel columns and a target MySQL table columns in a specific Schema.
  /// </summary>
  [Serializable]
  public class MySqlColumnMapping
  {
    /// <summary>
    /// Initializes a new instance of the <see cref="MySqlColumnMapping"/> class.
    /// </summary>
    public MySqlColumnMapping()
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="MySqlColumnMapping"/> class.
    /// </summary>
    /// <param name="mappingName">Name for this columns mapping structure.</param>
    /// <param name="sourceColNames">An array of column names in the source Excel data range.</param>
    /// <param name="targetColNames">An array of column names in the target MySQL table.</param>
    public MySqlColumnMapping(string mappingName, string[] sourceColNames, string[] targetColNames)
      : this()
    {
      Name = mappingName;

      // Initialization of these values occurs in the AppendDataForm dialog
      SchemaName = string.Empty;
      TableName = string.Empty;
      ConnectionName = string.Empty;
      Port = 0;

      if (sourceColNames != null)
      {
        SourceColumns = new string[sourceColNames.Length];
        sourceColNames.CopyTo(SourceColumns, 0);
      }

      if (targetColNames != null)
      {
        TargetColumns = new string[targetColNames.Length];
        targetColNames.CopyTo(TargetColumns, 0);
        MappedSourceIndexes = new int[targetColNames.Length];
      }

      ClearMappings();
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="MySqlColumnMapping"/> class.
    /// </summary>
    /// <param name="sourceColNames">An array of column names in the source Excel data range.</param>
    /// <param name="targetColNames">An array of column names in the target MySQL table.</param>
    public MySqlColumnMapping(string[] sourceColNames, string[] targetColNames)
      : this(string.Empty, sourceColNames, targetColNames)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="MySqlColumnMapping"/> class.
    /// </summary>
    /// <param name="likeMapping">A <see cref="MySqlColumnMapping"/> object from which to clone the mapping structure data.</param>
    /// <param name="newSourceColNames">An array of column names in the source Excel data range.</param>
    /// <param name="newTargetColNames">An array of column names in the target MySQL table.</param>
    public MySqlColumnMapping(MySqlColumnMapping likeMapping, string[] newSourceColNames, string[] newTargetColNames)
      : this(likeMapping.Name, newSourceColNames, newTargetColNames)
    {
      SchemaName = likeMapping.SchemaName;
      TableName = likeMapping.TableName;
      ConnectionName = likeMapping.ConnectionName;
      Port = likeMapping.Port;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="MySqlColumnMapping"/> class.
    /// </summary>
    /// <param name="likeMapping">A <see cref="MySqlColumnMapping"/> object from which to clone the mapping structure data.</param>
    public MySqlColumnMapping(MySqlColumnMapping likeMapping)
      : this(likeMapping, likeMapping.SourceColumns, likeMapping.TargetColumns)
    {
      for (int idx = 0; idx < likeMapping.MappedSourceIndexes.Length; idx++)
      {
        MappedSourceIndexes[idx] = likeMapping.MappedSourceIndexes[idx];
      }
    }

    #region Properties

    /// <summary>
    /// Gets or sets the name for this columns mapping structure.
    /// </summary>
    [XmlAttribute(AttributeName = "MappingName")]
    public string Name { get; set; }

    /// <summary>
    /// Gets or sets the name of the connection used to connect to a MySQL server instance.
    /// </summary>
    [XmlAttribute(AttributeName = "ConnectionName")]
    public string ConnectionName { get; set; }

    /// <summary>
    /// Gets or sets the port number used for the MySQL connection.
    /// </summary>
    [XmlAttribute(AttributeName = "Port")]
    public uint Port { get; set; }

    /// <summary>
    /// Gets or sets the schema name where the mapped table resides.
    /// </summary>
    [XmlAttribute(AttributeName = "Schema")]
    public string SchemaName { get; set; }

    /// <summary>
    /// Gets or sets the name of the table to map to.
    /// </summary>
    [XmlAttribute(AttributeName = "Table")]
    public string TableName { get; set; }

    /// <summary>
    /// Gets or sets an array of column names in the source Excel data range.
    /// </summary>
    [XmlAttribute(AttributeName = "SourceColumns")]
    public string[] SourceColumns { get; set; }

    /// <summary>
    /// Gets or sets an array of column names in the target MySQL table.
    /// </summary>
    [XmlAttribute(AttributeName = "TargetColumns")]
    public string[] TargetColumns { get; set; }

    /// <summary>
    /// Gets or sets an array with the positions of source columns mapped to the target columns.
    /// </summary>
    [XmlAttribute(AttributeName = "SourceIndexes")]
    public int[] MappedSourceIndexes { get; set; }

    /// <summary>
    /// Gets the quantity of source columns mapped to target columns.
    /// </summary>
    [XmlIgnore]
    public int MappedQuantity
    {
      get
      {
        return MappedSourceIndexes.Count(idx => idx >= 0);
      }
    }

    /// <summary>
    /// Gets a value indicating whether all source columns are mapped to target columns.
    /// </summary>
    [XmlIgnore]
    public bool AllColumnsMapped
    {
      get
      {
        return MappedQuantity == MappedSourceIndexes.Length;
      }
    }

    #endregion Properties

    /// <summary>
    /// Checks if all columns stored in this mapping structure match the columns in another target table so to know if this mapping can be reused with the given <see cref="DataTable"/> object.
    /// </summary>
    /// <param name="dataTable"><see cref="DataTable"/> object representing a possible new target table.</param>
    /// <param name="sameOrdinals">Flag indicating if the check must consider matching column positions besides matching the column names.</param>
    /// <returns><c>true</c> if all columns in the mapping match the possible new target table columns, <c>false</c> otherwise.</returns>
    public bool AllColumnsMatch(DataTable dataTable, bool sameOrdinals)
    {
      return TargetColumns != null && GetMatchingColumnsQuantity(dataTable, sameOrdinals) == TargetColumns.Length;
    }

    /// <summary>
    /// Clears the list of mapped source to target columns.
    /// </summary>
    public void ClearMappings()
    {
      if (MappedSourceIndexes == null || TargetColumns == null)
      {
        return;
      }

      for (int i = 0; i < TargetColumns.Length; i++)
      {
        MappedSourceIndexes[i] = -1;
      }
    }

    /// <summary>
    /// Gets the number of columns in the mapping structure whose names match the column names in the given <see cref="DataTable"/> object.
    /// </summary>
    /// <param name="dataTable"><see cref="DataTable"/> object representing a possible new target table.</param>
    /// <param name="sameOrdinals">Flag indicating if the check must consider matching column positions besides matching the column names.</param>
    /// <returns>Number of matching columns.</returns>
    public int GetMatchingColumnsQuantity(DataTable dataTable, bool sameOrdinals)
    {
      int matchingColumnsQty = 0;
      if (dataTable == null || TargetColumns == null)
      {
        return matchingColumnsQty;
      }

      for (int colIdx = 0; colIdx < TargetColumns.Length; colIdx++)
      {
        string colName = TargetColumns[colIdx];
        if (sameOrdinals)
        {
          if (string.Equals(dataTable.Columns[colIdx].ColumnName, colName, StringComparison.InvariantCultureIgnoreCase))
          {
            matchingColumnsQty++;
          }
        }
        else
        {
          if (dataTable.Columns.Contains(colName))
          {
            matchingColumnsQty++;
          }
        }
      }

      return matchingColumnsQty;
    }

    /// <summary>
    /// Gets the position of the given source index within the <see cref="MappedSourceIndexes"/> array.
    /// </summary>
    /// <param name="sourceIndex">Source index.</param>
    /// <returns>Source index position.</returns>
    public int GetMappedSourceIndexIndex(int sourceIndex)
    {
      return MiscUtilities.IndexOfIntInArray(MappedSourceIndexes, sourceIndex);
    }

    /// <summary>
    /// Gets the position of a column with the given name within the <see cref="SourceColumns"/> array.
    /// </summary>
    /// <param name="colName">Column name.</param>
    /// <returns>Column name position.</returns>
    public int GetSourceColumnIndex(string colName)
    {
      return MiscUtilities.IndexOfStringInArray(SourceColumns, colName, true);
    }

    /// <summary>
    /// Gets the position of a column with the given name within the <see cref="TargetColumns"/> array.
    /// </summary>
    /// <param name="colName">Column name.</param>
    /// <returns>Column name position.</returns>
    public int GetTargetColumnIndex(string colName)
    {
      return MiscUtilities.IndexOfStringInArray(TargetColumns, colName, true);
    }
  }
}