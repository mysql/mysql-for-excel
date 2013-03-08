//
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
//

namespace MySQL.ForExcel
{
  /// <summary>
  /// Represents an active editing session associating the name of the MySQL table being edited and its corresponding active <see cref="EditDataDialog"/> object.
  /// </summary>
  public class ActiveEditDialogContainer
  {
    /// <summary>
    /// Initializes a new instance of the <see cref="ActiveEditDialogContainer"/> class.
    /// </summary>
    /// <param name="tableName">The MySQL table associated with the active <see cref="EditDataDialog"/> object.</param>
    /// <param name="ActiveEditDialog">The active <see cref="EditDataDialog"/> object of an editing session.</param>
    public ActiveEditDialogContainer(string tableName, EditDataDialog editDialog)
    {
      TableName = tableName;
      EditDialog = editDialog;
    }

    /// <summary>
    /// Gets the active <see cref="EditDataDialog"/> object of an editing session.
    /// </summary>
    public EditDataDialog EditDialog { get; private set; }

    /// <summary>
    /// Gets the name of the MySQL table associated with the active <see cref="EditDataDialog"/> object.
    /// </summary>
    public string TableName { get; private set; }
  }
}