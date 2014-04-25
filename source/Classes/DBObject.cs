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

namespace MySQL.ForExcel.Classes
{
  /// <summary>
  /// Represents a MySQL database object that MySQL for Excel can interact with.
  /// </summary>
  public class DbObject
  {
    #region Constants

    /// <summary>
    /// The value representing all DB object types
    /// </summary>
    public const DbObjectType ALL_DB_OBJECT_TYPES = DbObjectType.Table | DbObjectType.View | DbObjectType.Procedure;

    #endregion Constants

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
    [Flags]
    public enum DbObjectType : short
    {
      /// <summary>
      /// A MySQL table object.
      /// </summary>
      Table = 1,

      /// <summary>
      /// A MySQL view object.
      /// </summary>
      View = 2,

      /// <summary>
      /// A MySQL stored procedure object.
      /// </summary>
      Procedure = 4,

      /// <summary>
      /// A MySQL schema.
      /// </summary>
      Schema = 8
    }

    #region Properties

    /// <summary>
    /// Gets the name of the MySQL database object.
    /// </summary>
    public string Name { get; private set; }

    /// <summary>
    /// Gets or sets a value indicating whether the object is selected by the user or not.
    /// </summary>
    public bool Selected { get; set; }

    /// <summary>
    /// Gets the MySQL database object type.
    /// </summary>
    public DbObjectType Type { get; private set; }

    #endregion Properties
  }
}