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
using System.Windows.Forms;
using MySQL.ForExcel.Controls;
using MySQL.ForExcel.Properties;

namespace MySQL.ForExcel.Classes
{
  /// <summary>
  /// Represents a header column for a <see cref="MultiHeaderDataGridView"/> control.
  /// </summary>
  public class MultiHeaderColumn
  {
    #region Fields

    /// <summary>
    /// The number of original columns this header will span.
    /// </summary>
    private int _columnSpan;

    #endregion Fields

    /// <summary>
    /// Initializes a new instance of the <see cref="MultiHeaderColumn"/> class.
    /// </summary>
    /// <param name="text">The text used in the top header of the grid column.</param>
    /// <param name="columnIndex">The index of the original <see cref="DataGridViewColumn"/> this object is associated with.</param>
    /// <param name="columnSpan">The number of original columns this header will span.</param>
    /// <param name="style">The <see cref="DataGridViewCellStyle"/> containing formatting and style of the header cell.</param>
    public MultiHeaderColumn(string text, int columnIndex, int columnSpan, DataGridViewCellStyle style)
    {
      _columnSpan = columnSpan;
      if (columnIndex < 0)
      {
        throw new ArgumentOutOfRangeException("columnIndex", Resources.ValueLessThanZeroError);
      }

      ColumnIndex = columnIndex;
      Style = style;
      Text = text;
    }

    #region Properties

    /// <summary>
    /// Gets or sets the index of the original <see cref="DataGridViewColumn"/> this object is associated with.
    /// </summary>
    public int ColumnIndex { get; set; }

    /// <summary>
    /// Gets or sets the number of original columns this header will span.
    /// If less than 0 it will default to 1.
    /// </summary>
    public int ColumnSpan
    {
      get
      {
        return _columnSpan;
      }

      set
      {
        _columnSpan = value < 1 ? 1 : value;
      }
    }

    /// <summary>
    /// Gets the <see cref="DataGridViewCellStyle"/> containing formatting and style of the header cell.
    /// </summary>
    public DataGridViewCellStyle Style { get; private set; }

    /// <summary>
    /// Gets or sets the text used in the top header of the grid column.
    /// </summary>
    public string Text { get; set; }

    #endregion Properties
  }
}
