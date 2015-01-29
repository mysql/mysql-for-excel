// Copyright (c) 2015, Oracle and/or its affiliates. All rights reserved.
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

namespace MySQL.ForExcel.Classes.EventArguments
{
  /// <summary>
  /// Event arguments for the ColumnWarningsChanged event.
  /// </summary>
  public class ColumnWarningsChangedArgs : EventArgs
  {
    /// <summary>
    /// Initializes a new instance of the <see cref="ColumnWarningsChangedArgs"/> class.
    /// </summary>
    /// <param name="column">The column that warnings are related to.</param>
    public ColumnWarningsChangedArgs(MySqlDataColumn column)
    {
      CurrentWarningText = column.CurrentWarningText;
    }

    /// <summary>
    /// Gets the last warning text associated to this column.
    /// </summary>
    public string CurrentWarningText { get; private set; }
  }
}
