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
  using System;
  using System.Collections.Generic;
  using System.ComponentModel;
  using System.Drawing;
  using System.Windows.Forms;

  /// <summary>
  /// 
  /// </summary>
  internal class MultiHeaderDataGridView : PreviewDataGridView
  {
    /// <summary>
    /// Default height in pixels of the top column headers.
    /// </summary>
    public const int COLUMN_HEADERS_HEIGHT = 46;

    /// <summary>
    /// Initializes a new instance of the <see cref="MultiHeaderDataGridView"/> class.
    /// </summary>
    public MultiHeaderDataGridView()
    {
      DoubleBuffered = true;
      AllowDrop = true;
      MultiHeaderColumnList = new List<MultiHeaderColumn>();
    }

    /// <summary>
    /// Gets or sets a list
    /// </summary>
    [Browsable(false)]
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    public List<MultiHeaderColumn> MultiHeaderColumnList { get; set; }

    /// <summary>
    /// Raises the <see cref="ColumnWidthChanged"/> event.
    /// </summary>
    /// <param name="e">A <see cref="DataGridViewColumnEventArgs"/> that contains the event data.</param>
    protected override void OnColumnWidthChanged(DataGridViewColumnEventArgs e)
    {
      base.OnColumnWidthChanged(e);
      Rectangle rtHeader = DisplayRectangle;
      rtHeader.Height = ColumnHeadersHeight / 2;
      Invalidate(rtHeader);
    }

    /// <summary>
    /// Raises the <see cref="DataBindingComplete"/> event.
    /// </summary>
    /// <param name="e">A <see cref="DataGridViewBindingCompleteEventArgs"/> that contains the event data.</param>
    protected override void OnDataBindingComplete(DataGridViewBindingCompleteEventArgs e)
    {
      base.OnDataBindingComplete(e);
      ClearSelection();
    }

    /// <summary>
    /// Raises the <see cref="Paint"/> event.
    /// </summary>
    /// <param name="e">A <see cref="PaintEventArgs"/> that contains the event data.</param>
    protected override void OnPaint(PaintEventArgs e)
    {
      base.OnPaint(e);
      ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.EnableResizing;
      ColumnHeadersHeight = COLUMN_HEADERS_HEIGHT;
      ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.BottomCenter;
      SolidBrush foregroundBrush = new SolidBrush(ColumnHeadersDefaultCellStyle.ForeColor);
      Color backColor = ColumnHeadersDefaultCellStyle.BackColor;
      SolidBrush backgroundBrush = new SolidBrush(backColor);
      StringFormat format = new StringFormat();
      format.Alignment = StringAlignment.Center;
      format.LineAlignment = StringAlignment.Center;
      foreach (MultiHeaderColumn mHeader in MultiHeaderColumnList)
      {
        int lastDivWidth = Columns[mHeader.LastColumnIndex].DividerWidth;
        int multiWidth = 0;
        for (int idx = mHeader.FirstColumnIndex; idx <= mHeader.LastColumnIndex; idx++)
        {
          multiWidth += Columns[idx].Width;
        }

        var firstRec = GetCellDisplayRectangle(mHeader.FirstColumnIndex, -1, true);
        if (firstRec.IsEmpty)
        {
          continue;
        }

        Rectangle headerRect = new Rectangle(firstRec.Left + 1, firstRec.Y, multiWidth - 2 - lastDivWidth, Convert.ToInt32(ColumnHeadersHeight / 2) - 2);
        backColor = mHeader.BackgroundColor.IsEmpty ? ColumnHeadersDefaultCellStyle.BackColor : mHeader.BackgroundColor;
        backgroundBrush.Color = backColor;
        e.Graphics.FillRectangle(backgroundBrush, headerRect);
        e.Graphics.DrawString(mHeader.HeaderText, ColumnHeadersDefaultCellStyle.Font, foregroundBrush, headerRect, format);
      }

      foregroundBrush.Dispose();
      backgroundBrush.Dispose();
    }

    /// <summary>
    /// Raises the <see cref="Scroll"/> event.
    /// </summary>
    /// <param name="e">A <see cref="ScrollEventArgs"/> that contains the event data.</param>
    protected override void OnScroll(ScrollEventArgs e)
    {
      base.OnScroll(e);
      if (e.ScrollOrientation == ScrollOrientation.HorizontalScroll)
      {
        Rectangle rtHeader = DisplayRectangle;
        rtHeader.Height = ColumnHeadersHeight / 2;
        Invalidate(rtHeader);
      }
    }
  }

  /// <summary>
  /// Represents a column with 2 header rows that is used in the <see cref="MultiHeaderDataGridView"/> control.
  /// </summary>
  public class MultiHeaderColumn
  {
    /// <summary>
    /// Initializes a new instance of the <see cref="MultiHeaderColumn"/> class.
    /// </summary>
    /// <param name="headerText">The text used in the top header of the grid column.</param>
    /// <param name="firstIndex">The first column index that this column spans.</param>
    /// <param name="lastIndex">The last column index that this column spans.</param>
    public MultiHeaderColumn(string headerText, int firstIndex, int lastIndex)
    {
      HeaderText = headerText;
      FirstColumnIndex = firstIndex;
      LastColumnIndex = lastIndex;
      BackgroundColor = SystemColors.Control;
    }

    /// <summary>
    /// Gets or sets the background color of the header rows.
    /// </summary>
    public Color BackgroundColor { get; set; }

    /// <summary>
    /// Gets the first column index that this column spans.
    /// </summary>
    public int FirstColumnIndex { get; private set; }

    /// <summary>
    /// Gets or sets the text used in the top header of the grid column.
    /// </summary>
    public string HeaderText { get; set; }

    /// <summary>
    /// Gets the last column index that this column spans.
    /// </summary>
    public int LastColumnIndex { get; private set; }
  }
}