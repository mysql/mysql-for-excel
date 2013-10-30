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
using System.Collections.Generic;
using System.Linq;
using MySQL.ForExcel.Properties;
using MySQL.Utility.Classes;

namespace MySQL.ForExcel.Classes
{
  /// <summary>
  /// Represents a collection of column mappings for the current user.
  /// </summary>
  public class MySqlColumnMappingList
  {
    /// <summary>
    /// Initializes a new instance of the <see cref="MySqlColumnMappingList"/> class.
    /// </summary>
    public MySqlColumnMappingList()
    {
      if (UserColumnMappingsList == null)
      {
        UserColumnMappingsList = new List<MySqlColumnMapping>();
      }
    }

    /// <summary>
    /// Gets or sets a list of <see cref="MySqlColumnMapping"/> objects for the current user.
    /// </summary>
    public List<MySqlColumnMapping> UserColumnMappingsList
    {
      get
      {
        return Settings.Default.StoredDataMappings;
      }

      set
      {
        Settings.Default.StoredDataMappings = value;
      }
    }

    /// <summary>
    /// Adds a new columns mapping structure to the user's mappings list.
    /// </summary>
    /// <param name="mapping">A <see cref="MySqlColumnMapping"/> object.</param>
    /// <returns><c>true</c> if the given columns mapping structure was successfully saved, <c>false</c> otherwise.</returns>
    public bool Add(MySqlColumnMapping mapping)
    {
      // Any other initialization for mapping can be done here.
      UserColumnMappingsList.Add(mapping);
      return MiscUtilities.SaveSettings();
    }

    /// <summary>
    /// Removes a given columns mapping structure from the user's mappings list.
    /// </summary>
    /// <param name="mapping">A <see cref="MySqlColumnMapping"/> object.</param>
    /// <returns><c>true</c> if the given columns mapping structure was successfully saved, <c>false</c> otherwise.</returns>
    public bool Remove(MySqlColumnMapping mapping)
    {
      try
      {
        // Check if it really exists.
        if (UserColumnMappingsList.Contains(mapping))
        {
          UserColumnMappingsList.Remove(mapping);
          return MiscUtilities.SaveSettings();
        }
      }
      catch (Exception ex)
      {
        MiscUtilities.ShowCustomizedErrorDialog(Resources.ColumnMappingDeletionErrorTitle, ex.Message, true);
        MySqlSourceTrace.WriteAppErrorToLog(ex);
        return false;
      }

      return false;
    }

    /// <summary>
    /// Renames the given columns mapping structure if exists in the user's mapping list.
    /// </summary>
    /// <param name="mapping">A <see cref="MySqlColumnMapping"/> object.</param>
    /// <param name="newName">New name for the columns mapping structure.</param>
    /// <returns><c>true</c> if the given columns mapping structure was successfully saved, <c>false</c> otherwise.</returns>
    public bool Rename(MySqlColumnMapping mapping, string newName)
    {
      try
      {
        // Check if it really exists.
        if (UserColumnMappingsList.Contains(mapping))
        {
          UserColumnMappingsList.Single(t => t.Equals(mapping)).Name = newName;
          return MiscUtilities.SaveSettings();
        }
      }
      catch (Exception ex)
      {
        MiscUtilities.ShowCustomizedErrorDialog(Resources.ColumnMappingRenameErrorTitle, ex.Message, true);
        MySqlSourceTrace.WriteAppErrorToLog(ex);
        return false;
      }

      return false;
    }

    /// <summary>
    /// Gets a list of <see cref="MySqlColumnMapping"/> objects for the given connection and port.
    /// </summary>
    /// <param name="connectionName">Name of the connection used to connect to a MySQL server instance.</param>
    /// <param name="port">Port number used for the MySQL connection.</param>
    /// <returns>List of <see cref="MySqlColumnMapping"/> objects.</returns>
    public List<MySqlColumnMapping> GetMappingsByConnection(string connectionName, int port)
    {
      if (UserColumnMappingsList != null && !string.IsNullOrEmpty(connectionName))
      {
        return UserColumnMappingsList.Where(t => t.ConnectionName.Equals(connectionName) && t.Port == port).ToList();
      }

      return null;
    }

    /// <summary>
    /// Gets a list of <see cref="MySqlColumnMapping"/> objects for the given connection, port, schema and target table.
    /// </summary>
    /// <param name="connectionName">Name of the connection used to connect to a MySQL server instance.</param>
    /// <param name="port">Port number used for the MySQL connection.</param>
    /// <param name="schema">Schema name where the mapped table resides.</param>
    /// <param name="tableName">Name of the table to map to.</param>
    /// <returns>List of <see cref="MySqlColumnMapping"/> objects.</returns>
    public List<MySqlColumnMapping> GetMappingsByConnectionSchemaAndTable(string connectionName, uint port, string schema, string tableName)
    {
      if (UserColumnMappingsList != null && !string.IsNullOrEmpty(connectionName))
      {
        return UserColumnMappingsList.Where(t => t.ConnectionName.Equals(connectionName) && t.Port == port && t.SchemaName.Equals(schema) && t.TableName.Equals(tableName)).ToList();
      }

      return null;
    }
  }
}