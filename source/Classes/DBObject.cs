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

namespace MySQL.ForExcel.Classes
{
  /// <summary>
  /// Represents a MySQL database object that MySQL for Excel can interact with.
  /// </summary>
  public class DbObject
  {
    /// <summary>
    /// Initializes a new instance of the <see cref="DbObject"/> class.
    /// </summary>
    /// <param name="name">The name of the MySQL database object.</param>
    /// <param name="type">The MySQL database object type.</param>
    public DbObject(string name, DbObjectType type)
    {
      Name = name;
      Type = type;
    }

    /// <summary>
    /// Specifies identifiers to indicate the MySQL database object type.
    /// </summary>
    public enum DbObjectType
    {
      /// <summary>
      /// A MySQL table object.
      /// </summary>
      Table,

      /// <summary>
      /// A MySQL view object.
      /// </summary>
      View,

      /// <summary>
      /// A MySQL stored procedure object.
      /// </summary>
      Procedure
    }

    /// <summary>
    /// Gets the name of the MySQL database object.
    /// </summary>
    public string Name { get; private set; }

    /// <summary>
    /// Gets the MySQL database object type.
    /// </summary>
    public DbObjectType Type { get; private set; }
  }
}