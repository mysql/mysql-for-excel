// Copyright (c) 2014, 2019, Oracle and/or its affiliates. All rights reserved.
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

using System.Collections.Generic;
using System.Linq;
using MySql.Utility.Classes.MySqlWorkbench;
using MySql.Utility.Enums;

namespace MySQL.ForExcel.Classes
{
  /// <summary>
  /// Represents a MySQL Table that MySQL for Excel can interact with.
  /// </summary>
  public class DbTable : DbView
  {
    #region Fields

    /// <summary>
    /// A list of <see cref="MySqlDataRelationship"/> objects representing relationships between this <see cref="DbObject"/> and others.
    /// </summary>
    private List<MySqlDataRelationship> _relationships;

    #endregion Fields

    /// <summary>
    /// Initializes a new instance of the <see cref="DbTable"/> class.
    /// </summary>
    /// <param name="connection">The MySQL Workbench connection to a MySQL server instance selected by users.</param>
    /// <param name="name">The name of the MySQL database object.</param>
    public DbTable(MySqlWorkbenchConnection connection, string name)
      : base(connection, name)
    {
    }

    #region Properties

    /// <summary>
    /// Gets a list of the names of other related <see cref="DbTable"/>s.
    /// </summary>
    public List<string> RelatedTableNames
    {
      get
      {
        if (_relationships == null)
        {
          _relationships = GetMySqlRelationships();
        }

        return _relationships.Select(rel => rel.RelatedTableName).Distinct().ToList();
      }
    }

    /// <summary>
    /// Gets a string containing a comma delimited list of the names of other related <see cref="DbTable"/>s.
    /// </summary>
    public string RelatedTableNamesDelimitedList => string.Join(",", RelatedTableNames);

    /// <summary>
    /// Gets a list of <see cref="MySqlDataRelationship"/> objects representing relationships between this <see cref="DbObject"/> and others.
    /// </summary>
    public List<MySqlDataRelationship> Relationships
    {
      get
      {
        _relationships = GetMySqlRelationships();
        return _relationships;
      }
    }

    #endregion Properties

    /// <summary>
    /// Releases all resources used by the <see cref="DbTable"/> class
    /// </summary>
    /// <param name="disposing">If true this is called by Dispose(), otherwise it is called by the finalizer</param>
    protected override void Dispose(bool disposing)
    {
      if (Disposed)
      {
        return;
      }

      // Free managed resources
      if (disposing)
      {
        if (_relationships != null)
        {
          _relationships.Clear();
          _relationships = null;
        }
      }

      base.Dispose(disposing);
    }

    /// <summary>
    /// Gets a list of <see cref="MySqlDataRelationship"/> objects representing relationships among this <see cref="DbObject"/> and other ones.
    /// </summary>
    /// <returns>A list of <see cref="MySqlDataRelationship"/> objects representing relationships among this <see cref="DbObject"/> and other ones.</returns>
    private List<MySqlDataRelationship> GetMySqlRelationships()
    {
      var relationshipsList = new List<MySqlDataRelationship>();
      if (Connection == null)
      {
        return relationshipsList;
      }

      var dt = Connection.GetSchemaInformation(SchemaInformationType.ForeignKeyColumns, true, null, Connection.Schema);

      // Detect relationships with Normal direction
      var rows = dt.Select($"TABLE_NAME = '{Name}'");
      relationshipsList.AddRange(rows.Select(row => new MySqlDataRelationship(MySqlDataRelationship.DirectionType.Normal, row["CONSTRAINT_NAME"].ToString(), row["TABLE_NAME"].ToString(), row["REFERENCED_TABLE_NAME"].ToString(), row["COLUMN_NAME"].ToString(), row["REFERENCED_COLUMN_NAME"].ToString())));

      // Detect relationships with Reverse direction where this object is not already defining a Normal direction one
      rows = dt.Select(string.Format("TABLE_NAME <> '{0}' AND REFERENCED_TABLE_NAME = '{0}'", Name));
      relationshipsList.AddRange(rows.Select(row => new MySqlDataRelationship(MySqlDataRelationship.DirectionType.Reverse, row["CONSTRAINT_NAME"].ToString(), row["REFERENCED_TABLE_NAME"].ToString(), row["TABLE_NAME"].ToString(), row["REFERENCED_COLUMN_NAME"].ToString(), row["COLUMN_NAME"].ToString())));

      return relationshipsList;
    }
  }
}