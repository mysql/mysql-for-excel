// 
// Copyright (c) 2012, Oracle and/or its affiliates. All rights reserved.
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

using System;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;


namespace MySQL.ForExcel
{
  [Serializable]
  public class MySQLColumnMapping
  {
    [XmlAttribute(AttributeName = "MappingName")]
    public string Name { get; set; }

    [XmlAttribute(AttributeName = "ConnectionName")]
    public string ConnectionName { get; set; }

    [XmlAttribute(AttributeName = "Port")]
    public int Port { get; set; }

    [XmlAttribute(AttributeName = "Schema")]
    public string SchemaName { get; set; }

    [XmlAttribute(AttributeName = "Table")]
    public string TableName { get; set; }

    [XmlAttribute(AttributeName = "SourceColumns")]
    public string[] SourceColumns { get; set; }

    [XmlAttribute(AttributeName = "TargetColumns")]
    public string[] TargetColumns { get; set; }

    [XmlAttribute(AttributeName = "SourceIndexes")]
    public int[] MappedSourceIndexes { get; set; }

    [XmlIgnore]
    public int MappedQuantity
    {
      get { return MappedSourceIndexes.Count(idx => idx >= 0); }
    }

    [XmlIgnore]
    public bool AllColumnsMapped
    {
      get { return MappedQuantity == MappedSourceIndexes.Length; }
    }

    public MySQLColumnMapping()
    { }

    public MySQLColumnMapping(string mappingName, string[] sourceColNames, string[] targetColNames)
    {
      Name = mappingName;

      /*Initialization of these values occurs in the AppendDataForm dialog */
      SchemaName = String.Empty;
      TableName = String.Empty;
      ConnectionName = String.Empty;
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

    public MySQLColumnMapping(string[] sourceColNames, string[] targetColNames)
      : this(String.Empty, sourceColNames, targetColNames)
    {
    }

    public MySQLColumnMapping(MySQLColumnMapping likeMapping, string[] newSourceColNames, string[] newTargetColNames) 
      : this(likeMapping.Name, newSourceColNames, newTargetColNames)
    {
      SchemaName = likeMapping.SchemaName;
      TableName = likeMapping.TableName;
      ConnectionName = likeMapping.ConnectionName;
      Port = likeMapping.Port;
    }

    public MySQLColumnMapping(MySQLColumnMapping likeMapping)
      : this(likeMapping, likeMapping.SourceColumns, likeMapping.TargetColumns)
    {
      for (int idx = 0; idx < likeMapping.MappedSourceIndexes.Length; idx++)
        MappedSourceIndexes[idx] = likeMapping.MappedSourceIndexes[idx];
    }

    public void ClearMappings()
    {
      if (MappedSourceIndexes != null && TargetColumns != null)
        for (int i = 0; i < TargetColumns.Length; i++)
          MappedSourceIndexes[i] = -1;
    }

    public int GetMatchingColumnsQuantity(DataTable dataTable, bool sameOrdinals)
    {
      int matchingColumnsQty = 0;
      if (dataTable != null && TargetColumns != null)
      {
        for (int colIdx = 0; colIdx < TargetColumns.Length; colIdx++)
        {
          string colName = TargetColumns[colIdx];
          if (sameOrdinals)
          {
            if (dataTable.Columns[colIdx].ColumnName.ToLowerInvariant() == colName.ToLowerInvariant())
              matchingColumnsQty++;
          }
          else
          {
            if (dataTable.Columns.Contains(colName))
              matchingColumnsQty++;
          }
        }
      }
      return matchingColumnsQty;
    }

    public bool AllColumnsMatch(DataTable dataTable, bool sameOrdinals)
    {
      return (TargetColumns != null ? GetMatchingColumnsQuantity(dataTable, sameOrdinals) == TargetColumns.Length : false);
    }

    public int GetSourceColumnIndex(string colName)
    {
      return MiscUtilities.IndexOfStringInArray(SourceColumns, colName, true);
    }

    public int GetTargetColumnIndex(string colName)
    {
      return MiscUtilities.IndexOfStringInArray(TargetColumns, colName, true);
    }

    public int GetMappedSourceIndexIndex(int sourceIndex)
    {
      return MiscUtilities.IndexOfIntInArray(MappedSourceIndexes, sourceIndex);
    }
  }

}
