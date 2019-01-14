// Copyright (c) 2015, 2019, Oracle and/or its affiliates. All rights reserved.
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

using System.Drawing;
using System.Windows.Forms;
using MySQL.ForExcel.Controls;

namespace MySQL.ForExcel.Classes
{
  /// <summary>
  /// Specifies information about spanning <see cref="MultiHeaderCell"/> objects used in the <see cref="MultiHeaderDataGridView"/>.
  /// </summary>
  internal class MultiHeaderCellsSpanningInfo
  {
    /// <summary>
    /// Initializes a new instance of the <see cref="MultiHeaderCellsSpanningInfo"/> class.
    /// </summary>
    private MultiHeaderCellsSpanningInfo()
    {
      RightDividerWidth = 0;
      VisibleWidth = 0;
      LeftOverflowingWidth = 0;
      RightOverflowingWidth = 0;
      TotalArea = Rectangle.Empty;
      VisibleArea = Rectangle.Empty;
    }

    #region Properties

    /// <summary>
    /// Gets the width, in pixels, of the left part that lies beyond the visible area of the control.
    /// </summary>
    public int LeftOverflowingWidth { get; private set; }

    /// <summary>
    /// Gets the direction in which a width overflows relative to the control visible area.
    /// </summary>
    public OverflowingDirectionType OverflowingDirection
    {
      get
      {
        if (TotalOverflowingWidth == TotalSpanningWidth)
        {
          return OverflowingDirectionType.Full;
        }

        if (LeftOverflowingWidth > 0 && RightOverflowingWidth > 0)
        {
          return OverflowingDirectionType.Both;
        }

        if (LeftOverflowingWidth > 0)
        {
          return OverflowingDirectionType.Left;
        }

        if (RightOverflowingWidth > 0)
        {
          return OverflowingDirectionType.Right;
        }

        return OverflowingDirectionType.None;
      }
    }

    /// <summary>
    /// Gets the width of the columns divider, in pixels, corresponding to the rightmost base column in the span.
    /// </summary>
    public int RightDividerWidth { get; private set; }

    /// <summary>
    /// Gets the width, in pixels, of the right part that lies beyond the visible area of the control.
    /// </summary>
    public int RightOverflowingWidth { get; private set; }

    /// <summary>
    /// Gets a <see cref="Rectangle"/> specifying the total area (visible and overflowing) of the spanning columns.
    /// </summary>
    /// <remarks>This will return <see cref="Rectangle.Empty"/> if there is no visible area at all.</remarks>
    public Rectangle TotalArea { get; private set; }

    /// <summary>
    /// Gets the total overflowing width (both directions), in pixels.
    /// </summary>
    public int TotalOverflowingWidth => LeftOverflowingWidth + RightOverflowingWidth;

    /// <summary>
    /// Gets the total spanning width, in pixels.
    /// </summary>
    public int TotalSpanningWidth => VisibleWidth + TotalOverflowingWidth;

    /// <summary>
    /// Gets a <see cref="Rectangle"/> specifying the visible area of the spanning columns.
    /// </summary>
    /// <remarks>This will return <see cref="Rectangle.Empty"/> if there is no visible area at all.</remarks>
    public Rectangle VisibleArea { get; private set; }

    /// <summary>
    /// Gets the visible width, in pixels, of the accumulated columns span.
    /// </summary>
    public int VisibleWidth { get; private set; }

    #endregion Properties

    #region Enums

    /// <summary>
    /// Specifies identifiers to indicate the direction in which a width overflows relative to the control visible area.
    /// </summary>
    public enum OverflowingDirectionType
    {
      /// <summary>
      /// Overflow happens to the left and right of the display bounds.
      /// </summary>
      Both,

      /// <summary>
      /// The whole width is not in the visible area.
      /// </summary>
      Full,

      /// <summary>
      /// Overflow happens to the left of the display bounds.
      /// </summary>
      Left,

      /// <summary>
      /// No overflow is present.
      /// </summary>
      None,

      /// <summary>
      /// Overflow happens to the right of the display bounds.
      /// </summary>
      Right
    }

    #endregion Enums

    /// <summary>
    /// Calculates the total width and overflow, in pixels, of an additional header cell based on the corresponding grid column widths the header cell spans.
    /// </summary>
    /// <param name="multiHeaderGrid">The <see cref="MultiHeaderDataGridView"/> control containing the <see cref="MultiHeaderCell"/> being measured.</param>
    /// <param name="headerCell">A <see cref="MultiHeaderCell"/> to measure its spanning width.</param>
    /// <returns>A <see cref="MultiHeaderCellsSpanningInfo"/> containing information about the header cells span.</returns>
    public static MultiHeaderCellsSpanningInfo GetHeaderCellSpanningInfo(MultiHeaderDataGridView multiHeaderGrid, MultiHeaderCell headerCell)
    {
      var spanningInfo = new MultiHeaderCellsSpanningInfo();
      if (multiHeaderGrid == null || headerCell == null)
      {
        return spanningInfo;
      }

      // In case the column span goes beyond the number of columns, then set the last index using the last column of the grid control.
      var lastColumnIndex = headerCell.GetLastBaseColumnIndexFromSpan(multiHeaderGrid.Columns.Count);

      // Calculate the displayed columns
      var firstDisplayedColumnIndex = multiHeaderGrid.FirstDisplayedScrollingColumnIndex;
      var lastDisplayedColumnIndex = multiHeaderGrid.DisplayedColumnCount(true) + firstDisplayedColumnIndex - 1;

      // Loop through spanning columns to accumulate information on each of them
      spanningInfo.RightDividerWidth = multiHeaderGrid.Columns[lastColumnIndex].DividerWidth;
      for (var idx = headerCell.ColumnIndex; idx <= lastColumnIndex; idx++)
      {
        var spanningColumn = multiHeaderGrid.Columns[idx];

        // Check if column is being displayed
        if (spanningColumn.Index >= firstDisplayedColumnIndex && spanningColumn.Index <= lastDisplayedColumnIndex)
        {
          // Get visible area of the current spanned column, we already know by this point it has at least some visible part
          var visibleColumnHeaderRectangle = multiHeaderGrid.GetCellDisplayRectangle(spanningColumn.Index, -1, true);

          // Check if the current column has overflow and to what direction is the overflow happening
          var overflow = spanningColumn.Width - visibleColumnHeaderRectangle.Width;
          var hitTestInfo = multiHeaderGrid.HitTest(visibleColumnHeaderRectangle.X + spanningColumn.Width, visibleColumnHeaderRectangle.Y);
          var overFlowToRight = hitTestInfo.Type == DataGridViewHitTestType.None;

          // Widths may be split among the overflowing and regular accumulated width if the column is partially displayed
          spanningInfo.VisibleWidth += visibleColumnHeaderRectangle.Width;
          if (overFlowToRight)
          {
            spanningInfo.RightOverflowingWidth += overflow;
          }
          else
          {
            spanningInfo.LeftOverflowingWidth += overflow;
          }

          // Update visible and total areas
          if (spanningInfo.VisibleArea == Rectangle.Empty)
          {
            spanningInfo.VisibleArea = new Rectangle(
              visibleColumnHeaderRectangle.Location,
              visibleColumnHeaderRectangle.Size);
            spanningInfo.TotalArea = new Rectangle(
                visibleColumnHeaderRectangle.X - spanningInfo.LeftOverflowingWidth,
                visibleColumnHeaderRectangle.Y,
                visibleColumnHeaderRectangle.Width + spanningInfo.LeftOverflowingWidth,
                visibleColumnHeaderRectangle.Height);
          }
          else
          {
            spanningInfo.VisibleArea.Inflate(visibleColumnHeaderRectangle.Width, 0);
            spanningInfo.TotalArea.Inflate(visibleColumnHeaderRectangle.Width + overflow, 0);
          }
        }
        else if (spanningColumn.Index < firstDisplayedColumnIndex)
        {
          spanningInfo.LeftOverflowingWidth += spanningColumn.Width;
        }
        else
        {
          spanningInfo.RightOverflowingWidth += spanningColumn.Width;
          if (spanningInfo.TotalArea != Rectangle.Empty)
          {
            spanningInfo.TotalArea.Inflate(spanningColumn.Width, 0);
          }
        }
      }

      return spanningInfo;
    }
  }
}
