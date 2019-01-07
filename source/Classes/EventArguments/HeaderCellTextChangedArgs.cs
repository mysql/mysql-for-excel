// Copyright (c) 2015, 2018, Oracle and/or its affiliates. All rights reserved.
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
  /// Event arguments for the MultiHeaderCellTextChanged event.
  /// </summary>
  public class HeaderCellTextChangedArgs : EventArgs
  {
    /// <summary>
    /// Initializes a new instance of the <see cref="HeaderCellTextChangedArgs"/> class.
    /// </summary>
    /// <param name="headerCell">The <see cref="MultiHeaderCell"/> object whose column span changed.</param>
    /// <param name="oldText">The old value of the <see cref="MultiHeaderCell.Text"/> property.</param>
    public HeaderCellTextChangedArgs(MultiHeaderCell headerCell, string oldText)
    {
      HeaderCell = headerCell;
      OldText = oldText;
    }

    /// <summary>
    /// Gets the <see cref="MultiHeaderCell"/> object whose column span changed.
    /// </summary>
    public MultiHeaderCell HeaderCell { get; }

    /// <summary>
    /// Gets the new value of the <see cref="MultiHeaderCell.Text"/> property.
    /// </summary>
    public string NewText => HeaderCell?.Text;

    /// <summary>
    /// Gets the old value of the <see cref="MultiHeaderCell.Text"/> property.
    /// </summary>
    public string OldText { get; }
  }
}
