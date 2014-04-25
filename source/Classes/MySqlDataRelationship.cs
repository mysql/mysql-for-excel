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

using System;

namespace MySQL.ForExcel.Classes
{
  /// <summary>
  /// Represents a relationship between 2 MySQL tables or views based on a single column on both database objects.
  /// </summary>
  public class MySqlDataRelationship
  {
    /// <summary>
    /// Initializes a new instance of the <see cref="MySqlDataRelationship"/> class.
    /// </summary>
    /// <param name="mySqlForeignKeyName">The name of the foreign key constraint from which the relationship was created from.</param>
    /// <param name="tableOrViewName">The name of the table or view defining the relationsip to a foreign one.</param>
    /// <param name="relatedTableOrViewName">The name of the related foreign table or view.</param>
    /// <param name="columnName">The name of the column defining the relationship to a foreign one.</param>
    /// <param name="relatedColumnName">The name of the related foreign column.</param>
    public MySqlDataRelationship(string mySqlForeignKeyName, string tableOrViewName, string relatedTableOrViewName, string columnName, string relatedColumnName)
    {
      MySqlForeignKeyName = mySqlForeignKeyName;
      if (string.IsNullOrEmpty(tableOrViewName))
      {
        throw new ArgumentNullException("tableOrViewName");
      }

      if (string.IsNullOrEmpty(relatedTableOrViewName))
      {
        throw new ArgumentNullException("relatedTableOrViewName");
      }

      if (string.IsNullOrEmpty(columnName))
      {
        throw new ArgumentNullException("columnName");
      }

      if (string.IsNullOrEmpty(relatedColumnName))
      {
        throw new ArgumentNullException("relatedColumnName");
      }

      TableOrViewName = tableOrViewName;
      RelatedTableOrViewName = relatedTableOrViewName;
      ColumnName = columnName;
      RelatedColumnName = relatedColumnName;
      Excluded = false;
    }

    #region Properties

    /// <summary>
    /// Gets the name of the column defining the relationship to a foreign one.
    /// </summary>
    public string ColumnName { get; private set; }

    /// <summary>
    /// Gets or sets a value indicating whether the relationship is excluded for further processing.
    /// </summary>
    public bool Excluded { get; set; }

    /// <summary>
    /// Gets the name of the foreign key constraint from which the relationship was created from.
    /// If <c>null</c> it means it was created by a user.
    /// </summary>
    public string MySqlForeignKeyName { get; private set; }

    /// <summary>
    /// Gets the name of the related foreign column.
    /// </summary>
    public string RelatedColumnName { get; private set; }

    /// <summary>
    /// Gets the name of the related foreign table or view.
    /// </summary>
    public string RelatedTableOrViewName { get; private set; }

    /// <summary>
    /// Gets the name of the table or view defining the relationsip to a foreign one.
    /// </summary>
    public string TableOrViewName { get; private set; }

    #endregion Properties
  }
}
