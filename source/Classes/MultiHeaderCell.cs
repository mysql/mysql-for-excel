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
using System.Drawing;
using System.Windows.Forms;
using MySQL.ForExcel.Classes.EventArguments;
using MySQL.ForExcel.Controls;
using MySQL.ForExcel.Properties;

namespace MySQL.ForExcel.Classes
{
  /// <summary>
  /// Represents an additional header cell for a <see cref="MultiHeaderDataGridView"/> control.
  /// </summary>
  public class MultiHeaderCell
  {
    #region Constants

    /// <summary>
    /// Default width, in pixels, used as a separator between header columns.
    /// </summary>
    public const int DEFAULT_SEPARATOR_WIDTH = 1;

    #endregion Constants

    #region Fields

    /// <summary>
    /// The number of original columns this header will span.
    /// </summary>
    private int _columnSpan;

    /// <summary>
    /// The width, in pixels, of the column header separators.
    /// </summary>
    private int _separatorWidth;

    /// <summary>
    /// The text used in the top header of the grid column.
    /// </summary>
    private string _text;

    /// <summary>
    /// Flag indicating whether the size of a header column cell is calculated adding the padding size on top of the text size.
    /// </summary>
    private bool _useColumnPaddingsForSizeCalculation;

    #endregion Fields

    /// <summary>
    /// Initializes a new instance of the <see cref="MultiHeaderCell"/> class.
    /// </summary>
    /// <param name="text">The text used in the top header of the grid column.</param>
    /// <param name="columnIndex">The index of the original <see cref="DataGridViewColumn"/> this object is associated with.</param>
    /// <param name="style">The <see cref="DataGridViewCellStyle"/> containing formatting and style of the header cell.</param>
    protected internal MultiHeaderCell(string text, int columnIndex, DataGridViewCellStyle style)
    {
      if (columnIndex < 0)
      {
        throw new ArgumentOutOfRangeException("columnIndex", Resources.ValueLessThanZeroError);
      }

      _columnSpan = 1;
      _separatorWidth = DEFAULT_SEPARATOR_WIDTH;
      _text = text;
      _useColumnPaddingsForSizeCalculation = true;
      CellSize = Size.Empty;
      ColumnIndex = columnIndex;
      InSpan = false;
      Style = style;
      CalculateCellSize();
    }

    #region Properties

    /// <summary>
    /// Gets the calculated <see cref="Size"/>, in pixels, of this header cell.
    /// </summary>
    public Size CellSize { get; private set; }

    /// <summary>
    /// Gets the index of the original <see cref="DataGridViewColumn"/> this object is associated with.
    /// </summary>
    public int ColumnIndex { get; private set; }

    /// <summary>
    /// Gets or sets the number of original columns this header will span.
    /// </summary>
    /// <remarks>If less than 0 or this cell is already part of another cell's span, it will default to 1.</remarks>
    public int ColumnSpan
    {
      get
      {
        return _columnSpan;
      }

      set
      {
        if (_columnSpan == value)
        {
          return;
        }

        int oldColumnSpan = _columnSpan;
        _columnSpan = value < 1 || InSpan ? 1 : value;
        OnHeaderCellColumnSpanChanged(oldColumnSpan);
      }
    }

    /// <summary>
    /// Gets a value indicating whether this cell is part of a column a span in a <see cref="MultiHeaderRow"/>
    /// </summary>
    public bool InSpan { get; internal set; }

    /// <summary>
    /// The width, in pixels, of the column header separators.
    /// </summary>
    public int SeparatorWidth
    {
      get
      {
        return _separatorWidth;
      }

      set
      {
        _separatorWidth = value;
        CalculateCellSize();
      }
    }

    /// <summary>
    /// Gets the <see cref="DataGridViewCellStyle"/> containing formatting and style of the header cell.
    /// </summary>
    public DataGridViewCellStyle Style { get; private set; }

    /// <summary>
    /// Gets or sets the text used in the top header of the grid column.
    /// </summary>
    public string Text
    {
      get
      {
        return _text;
      }

      set
      {
        if (_text == value)
        {
          return;
        }

        string oldText = value;
        _text = value;
        OnHeaderCellTextChanged(oldText);
      }
    }

    /// <summary>
    /// Gets or sets a value indicating whether the size of a header column cell is calculated adding the padding size on top of the text size.
    /// </summary>
    public bool UseColumnPaddingsForSizeCalculation
    {
      get
      {
        return _useColumnPaddingsForSizeCalculation;
      }

      set
      {
        _useColumnPaddingsForSizeCalculation = value;
        CalculateCellSize();
      }
    }

    #endregion Properties

    #region Events

    /// <summary>
    /// Delegate handler for the <see cref="HeaderCellColumnSpanChanged"/> event.
    /// </summary>
    /// <param name="sender">Sender object.</param>
    /// <param name="args">Event arguments.</param>
    public delegate void HeaderCellColumnSpanChangedHandler(object sender, HeaderCellColumnSpanChangedArgs args);

    /// <summary>
    /// Occurs when the value of <see cref="ColumnSpan"/> changes.
    /// </summary>
    public event HeaderCellColumnSpanChangedHandler HeaderCellColumnSpanChanged;

    /// <summary>
    /// Delegate handler for the <see cref="HeaderCellTextChangedHandler"/> event.
    /// </summary>
    /// <param name="sender">Sender object.</param>
    /// <param name="args">Event arguments.</param>
    public delegate void HeaderCellTextChangedHandler(object sender, HeaderCellTextChangedArgs args);

    /// <summary>
    /// Occurs when the value of <see cref="Text"/> changes.
    /// </summary>
    public event HeaderCellTextChangedHandler HeaderCellTextChanged;

    #endregion Events

    /// <summary>
    /// Calculates this header cell size, in pixels, and stores it in <see cref="CellSize"/>.
    /// </summary>
    public void CalculateCellSize()
    {
      string computingText = Text;
      bool emptyText = string.IsNullOrEmpty(computingText);
      if (emptyText)
      {
        computingText = "Text";
      }

      // Compute text size
      var computedSize = TextRenderer.MeasureText(computingText, Style.Font);

      // Add paddings and separators width
      computedSize.Width += (_useColumnPaddingsForSizeCalculation ? Style.Padding.Left + Style.Padding.Right : 0) + _separatorWidth;
      computedSize.Height += (_useColumnPaddingsForSizeCalculation ? Style.Padding.Top + Style.Padding.Bottom : 0) + _separatorWidth;

      // If the text is null or empty, return a 0 width but a virtual height computed with any text (even an empty column needs to have a height).
      CellSize = new Size(emptyText ? 0 : computedSize.Width, computedSize.Height);
    }

    /// <summary>
    /// Gets the last base column index according to the columns spanned by this header cell.
    /// </summary>
    /// <param name="baseColumnsCount">The quantity of base columns.</param>
    /// <returns>The last base column index according to the columns spanned by this header cell.</returns>
    public int GetLastBaseColumnIndexFromSpan(int baseColumnsCount)
    {
      int lastColumnIndex = ColumnIndex + ColumnSpan - 1;
      if (lastColumnIndex >= baseColumnsCount)
      {
        lastColumnIndex = baseColumnsCount - 1;
      }

      return lastColumnIndex;
    }

    /// <summary>
    /// Raises the <see cref="HeaderCellColumnSpanChanged"/> event.
    /// </summary>
    /// <param name="oldColumnSpan">The old value of the <see cref="MultiHeaderCell.ColumnSpan"/> property.</param>
    protected virtual void OnHeaderCellColumnSpanChanged(int oldColumnSpan)
    {
      if (HeaderCellColumnSpanChanged == null)
      {
        return;
      }

      HeaderCellColumnSpanChanged(this, new HeaderCellColumnSpanChangedArgs(this, oldColumnSpan));
    }

    /// <summary>
    /// Raises the <see cref="HeaderCellColumnSpanChanged"/> event.
    /// </summary>
    /// <param name="oldText">The old value of the <see cref="MultiHeaderCell.Text"/> property.</param>
    protected virtual void OnHeaderCellTextChanged(string oldText)
    {
      CalculateCellSize();
      if (HeaderCellTextChanged == null)
      {
        return;
      }

      HeaderCellTextChanged(this, new HeaderCellTextChangedArgs(this, oldText));
    }
  }
}
