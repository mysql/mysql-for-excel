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

using System;

namespace MySQL.ForExcel.Interfaces
{
  /// <summary>
  /// Interface that bridges common functionality to both Import and Edit connections.
  /// </summary>
  public interface IConnectionInfo : IDisposable
  {
    /// <summary>
    /// Gets or sets the connection id, enabling to retrieve the credentials to access the referred MySQL instance in it.
    /// </summary>
    string ConnectionId { get; set; }

    /// <summary>
    /// Gets or sets the last date and time the connection info was saved.
    /// </summary>
    DateTime LastAccess { get; set; }

    /// <summary>
    /// Gets or sets the name of the schema.
    /// </summary>
    string SchemaName { get; set; }

    /// <summary>
    /// Gets or sets the name of the table.
    /// </summary>
    string TableName { get; set; }

    /// <summary>
    /// Gets or sets the workbook unique identifier.
    /// </summary>
    string WorkbookGuid { get; set; }

    /// <summary>
    /// Gets or sets the workbook full path name.
    /// </summary>
    string WorkbookFilePath { get; set; }

  }
}
