// Copyright (c) 2014, Oracle and/or its affiliates. All rights reserved.
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

using DataTable = System.Data.DataTable;

namespace MySQL.ForExcel.Classes
{
  /// <summary>
  /// Represents a <see cref="DataTable"/> containing schema information of MySQL table columns.
  /// </summary>
  public class MySqlColumnsInformationTable : DataTable
  {
    /// <summary>
    /// Initializes a new instance of the <see cref="MySqlColumnsInformationTable"/> class.
    /// </summary>
    public MySqlColumnsInformationTable() : this(null)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="MySqlColumnsInformationTable"/> class.
    /// </summary>
    /// <param name="tableName">The name of the table.</param>
    public MySqlColumnsInformationTable(string tableName)
    {
      TableName = string.IsNullOrEmpty(tableName) ? "ColumnsInfo" : tableName;
      Columns.Add("Name");
      Columns.Add("Type");
      Columns.Add("Null");
      Columns.Add("Key");
      Columns.Add("Default");
      Columns.Add("CharSet");
      Columns.Add("Collation");
      Columns.Add("Extra");
    }
  }
}
