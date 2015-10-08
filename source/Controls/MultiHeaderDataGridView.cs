// Copyright (c) 2012-2015, Oracle and/or its affiliates. All rights reserved.
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
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using MySQL.ForExcel.Classes;

namespace MySQL.ForExcel.Controls
{
  /// <summary>
  /// Displays data in a read-only grid that supports a two-row header for preview purposes only.
  /// </summary>
  public sealed class MultiHeaderDataGridView : PreviewDataGridView
  {
    #region Constants

    /// <summary>
    /// Default height, in pixels, of the top column headers.
    /// </summary>
    public const int DEFAULT_COLUMN_HEADERS_HEIGHT = 23;

    /// <summary>
    /// Default padding, in pixels, used for left and right.
    /// </summary>
    public const int DEFAULT_HEADERS_HORIZONTAL_PADDING = 5;

    /// <summary>
    /// Default padding, in pixels, used for top and bottom.
    /// </summary>
    public const int DEFAULT_HEADERS_VERTICAL_PADDING = 3;

    /// <summary>
    /// Default width used as a separator between header columns;
    /// </summary>
    public const int DEFAULT_HEADERS_SEPARATOR_WIDTH = 1;

    /// <summary>
    /// Initial number of rows this control is likely to have.
    /// </summary>
    private const int INITIAL_ROWS_QUANTITY = 2;

    #endregion Constants

    #region Fields

    /// <summary>
    /// Flag indicating whether row and column headers use the visual styles of the user's current theme if visual styles are enabled for the application.
    /// </summary>
    private bool _allowChangingHeaderCellsColors;

    /// <summary>
    /// A list of header rows each containing <see cref="MultiHeaderColumn"/> objects.
    /// </summary>
    private readonly List<MultiHeaderRow> _multiHeaderRowsList;

    #endregion Fields

    /// <summary>
    /// Initializes a new instance of the <see cref="MultiHeaderDataGridView"/> class.
    /// </summary>
    public MultiHeaderDataGridView()
    {
      _multiHeaderRowsList = new List<MultiHeaderRow>(INITIAL_ROWS_QUANTITY);
      AllowChangingHeaderCellsColors = true;
      AllowDrop = true;
      AutoAdjustColumnHeadersHeight = true;
      BaseColumnHeadersTextAlignment = HorizontalAlignment.Center;
      ColumnHeadersSeparatorColor = SystemColors.ControlDark;
      ColumnHeadersSeparatorWidth = DEFAULT_HEADERS_SEPARATOR_WIDTH;
      DoubleBuffered = true;
      FixedColumnHeadersHeight = DEFAULT_COLUMN_HEADERS_HEIGHT;
      ReverseMultiHeaderRowOrder = false;
      UseColumnPaddings = true;
    }

    #region Properties

    /// <summary>
    /// Gets a value indicating whether row and column headers use the visual styles of the user's current theme if visual styles are enabled for the application.
    /// </summary>
    [Category("MySQL Custom")]
    public bool AllowChangingHeaderCellsColors
    {
      get
      {
        return _allowChangingHeaderCellsColors;
      }

      set
      {
        _allowChangingHeaderCellsColors = value;
        EnableHeadersVisualStyles = !value;
      }
    }

    /// <summary>
    /// Gets or sets a value indicating whether the height of a column headers row is computed based on their fonts or the <seealso cref="FixedColumnHeadersHeight"/> value is used.
    /// </summary>
    [Category("MySQL Custom")]
    public bool AutoAdjustColumnHeadersHeight { get; set; }

    /// <summary>
    /// Gets or sets the horizontal alignment of the base column headers text.
    /// </summary>
    [Category("MySQL Custom")]
    public HorizontalAlignment BaseColumnHeadersTextAlignment { get; set; }

    /// <summary>
    /// Gets the height, in pixels, of the column headers row(s).
    /// </summary>
    [Category("MySQL Custom"), Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    public new int ColumnHeadersHeight
    {
      get
      {
        return base.ColumnHeadersHeight;
      }

      private set
      {
        base.ColumnHeadersHeight = value;
      }
    }

    /// <summary>
    /// Gets a value indicating whether the height of the column headers is adjustable and whether it can be adjusted by the user or is automatically adjusted to fit the contents of the headers.
    /// </summary>
    [Category("MySQL Custom"), Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    public new DataGridViewColumnHeadersHeightSizeMode ColumnHeadersHeightSizeMode
    {
      get
      {
        return base.ColumnHeadersHeightSizeMode;
      }

      private set
      {
        base.ColumnHeadersHeightSizeMode = value;
      }
    }

    /// <summary>
    /// Gets or sets the <see cref="Color"/> of the column headers separators.
    /// </summary>
    /// <remarks>This color is only applied when <seealso cref="AllowChangingHeaderCellsColors"/> is true (so <seealso cref="EnableHeadersVisualStyles"/> is false).</remarks>
    [Category("MySQL Custom")]
    public Color ColumnHeadersSeparatorColor { get; set; }

    /// <summary>
    /// Gets or sets the width, in pixels, of the column header separators.
    /// </summary>
    [Category("MySQL Custom")]
    public int ColumnHeadersSeparatorWidth { get; set; }

    /// <summary>
    /// Gets a value indicating whether this control should redraw its surface using a secondary buffer to reduce or prevent flicker.
    /// </summary>
    [Category("MySQL Custom"), Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    protected override bool DoubleBuffered
    {
      get
      {
        return base.DoubleBuffered;
      }

      set
      {
        base.DoubleBuffered = value;
      }
    }

    /// <summary>
    /// Gets a value indicating whether row and column headers use the visual styles of the user's current theme if visual styles are enabled for the application.
    /// </summary>
    [Category("MySQL Custom"), Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    public new bool EnableHeadersVisualStyles
    {
      get
      {
        return base.EnableHeadersVisualStyles;
      }

      private set
      {
        base.EnableHeadersVisualStyles = value;
      }
    }

    /// <summary>
    /// Gets or sets a fixed height, in pixels, for each column headers row.
    /// </summary>
    [Category("MySQL Custom")]
    public int FixedColumnHeadersHeight { get; set; }

    /// <summary>
    /// Gets a read-only collection of header rows each containing <see cref="MultiHeaderColumn"/> objects.
    /// </summary>
    [Category("MySQL Custom"), Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    public ReadOnlyCollection<MultiHeaderRow> MultiHeaderRowsCollection
    {
      get
      {
        return _multiHeaderRowsList.AsReadOnly();
      }
    }

    /// <summary>
    /// Gets or sets a value indicating whether the additional header rows defined in the <seealso cref="MultiHeaderRowsCollection"/> is reversed.
    /// </summary>
    /// <remarks>If <c>true</c> the order of rows is from the original header row up, if <c>false</c> the order is from the top of the grid down.</remarks>
    [Category("MySQL Custom")]
    public bool ReverseMultiHeaderRowOrder { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether the height of a column headers row is computed based on their fonts or the <seealso cref="FixedColumnHeadersHeight"/> value is used.
    /// </summary>
    [Category("MySQL Custom")]
    public bool UseColumnPaddings { get; set; }

    #endregion Properties

    /// <summary>
    /// Adds a new <see cref="MultiHeaderRow"/> to the header rows collection.
    /// </summary>
    /// <param name="copyStyleFromColumnHeader">Flag indicating whether the style to use for the new header row is copied from the grid's header style, otherwise the default style is used.</param>
    /// <param name="autoGenerateHeaderText">If <c>true</c> header text will be autogenerated for all headers, otherwise they will be empty.</param>
    public void AddHeadersRow(bool copyStyleFromColumnHeader, bool autoGenerateHeaderText = false)
    {
      int columnsCount = Columns.Count;
      if (_multiHeaderRowsList == null || columnsCount == 0)
      {
        // Do not add any rows if columns have not been defined for the grid control.
        return;
      }

      var headersRow = new MultiHeaderRow(FixedColumnHeadersHeight, columnsCount);
      for (int columnIndex = 0; columnIndex < Columns.Count; columnIndex++)
      {
        string headerText = autoGenerateHeaderText
          ? string.Format("Header{0}-{1}", _multiHeaderRowsList.Count, columnIndex)
          : string.Empty;
        var column = Columns[columnIndex];
        var style = CreateColumnHeaderCellStyle(copyStyleFromColumnHeader ? column.HeaderCell : null);
        //style.Padding = new Padding(DEFAULT_HEADERS_HORIZONTAL_PADDING, DEFAULT_HEADERS_VERTICAL_PADDING, DEFAULT_HEADERS_HORIZONTAL_PADDING, DEFAULT_HEADERS_VERTICAL_PADDING);
        var headerColumn = new MultiHeaderColumn(headerText, columnIndex, 1, style);
        headersRow.Add(headerColumn);
      }

      _multiHeaderRowsList.Add(headersRow);
    }


    /// <summary>
    /// Clears the header rows collection.
    /// </summary>
    public void ClearHeadersRows()
    {
      if (_multiHeaderRowsList == null)
      {
        return;
      }

      _multiHeaderRowsList.Clear();
    }

    /// <summary>
    /// Removes an existing <see cref="MultiHeaderRow"/> from the header rows collection.
    /// </summary>
    /// <param name="rowIndex">The index of the <see cref="MultiHeaderRow"/> to remove from the collection.</param>
    public void RemoveHeadersRow(int rowIndex)
    {
      if (_multiHeaderRowsList == null || rowIndex < 0 || rowIndex >= _multiHeaderRowsList.Count)
      {
        return;
      }

      _multiHeaderRowsList.RemoveAt(rowIndex);
    }

    /// <summary>
    /// Raises the <see cref="DataGridView.ColumnAdded"/> event.
    /// </summary>
    /// <param name="e">A <see cref="DataGridViewColumnEventArgs"/> that contains the event data.</param>
    protected override void OnColumnAdded(DataGridViewColumnEventArgs e)
    {
      base.OnColumnAdded(e);

      // Set columns as non-sortable, since allowing sorting affects the custom drawing of additional headers.
      e.Column.SortMode = DataGridViewColumnSortMode.NotSortable;
    }

    /// <summary>
    /// Raises the <see cref="DataGridView.ColumnWidthChanged"/> event.
    /// </summary>
    /// <param name="e">A <see cref="DataGridViewColumnEventArgs"/> that contains the event data.</param>
    protected override void OnColumnWidthChanged(DataGridViewColumnEventArgs e)
    {
      base.OnColumnWidthChanged(e);
      var headerRectangle = DisplayRectangle;
      headerRectangle.Height = ColumnHeadersHeight;
      Invalidate(headerRectangle);
    }

    /// <summary>
    /// Raises the <see cref="DataGridView.DataBindingComplete"/> event.
    /// </summary>
    /// <param name="e">A <see cref="DataGridViewBindingCompleteEventArgs"/> that contains the event data.</param>
    protected override void OnDataBindingComplete(DataGridViewBindingCompleteEventArgs e)
    {
      base.OnDataBindingComplete(e);
      ClearSelection();
    }

    /// <summary>
    /// Raises the <see cref="Control.Paint"/> event.
    /// </summary>
    /// <param name="e">A <see cref="PaintEventArgs"/> that contains the event data.</param>
    protected override void OnPaint(PaintEventArgs e)
    {
      base.OnPaint(e);
      if (_multiHeaderRowsList == null || _multiHeaderRowsList.Count == 0)
      {
        return;
      }

      // Enforce that the alignment of the original header text is at the bottom, so the other header rows can be drawn correctly.
      ColumnHeadersDefaultCellStyle.Alignment = BaseColumnHeadersTextAlignment.ToBottomAlignment();

      // Compute the  height of the original header plus all additional header rows
      ColumnHeadersHeight = GetCumulativeColumnHeaderRowsHeight(e.Graphics);

      // Set other properties and process each additional row.
      ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.EnableResizing;
      var rowsQuantity = _multiHeaderRowsList.Count;
      var foregroundBrush = new SolidBrush(ColumnHeadersDefaultCellStyle.ForeColor);
      var backgroundBrush = new SolidBrush(ColumnHeadersDefaultCellStyle.BackColor);
      int columnsQuantity = Columns.Count;
      int accumulatedRowHeights = 0;
      for (int rowIndex = 0; rowIndex < rowsQuantity; rowIndex++)
      {
        int reversedRowIndex = ReverseMultiHeaderRowOrder ? rowsQuantity - rowIndex - 1 : rowIndex;
        var headerRow = _multiHeaderRowsList[reversedRowIndex];
        var previousColumnSpan = 0;
        for (int columnIndex = 0; columnIndex < columnsQuantity; columnIndex++)
        {
          // If the current column is part of a span, skip it.
          if (previousColumnSpan > 1)
          {
            previousColumnSpan--;
            continue;
          }

          var headerColumn = headerRow[columnIndex];

          // In case the column span goes beyond the number of columns, then set the last index using the last column of the grid control.
          int lastColumnIndex = columnIndex + headerColumn.ColumnSpan - 1;
          if (lastColumnIndex >= columnsQuantity)
          {
            lastColumnIndex = columnsQuantity - 1;
          }

          // Compute the length of the currently processed header since it could be spanning more than one grid column, in which case add columns to skip to the collection.
          int multiWidth = 0;
          int lastSpanningColumnDividerWidth = Columns[lastColumnIndex].DividerWidth;
          for (int idx = columnIndex; idx <= lastColumnIndex; idx++)
          {
            DataGridViewColumn spanningColumn = Columns[idx];
            multiWidth += spanningColumn.Width;
            if (idx < lastColumnIndex)
            {
              multiWidth += spanningColumn.DividerWidth;
            }
          }

          // Get the rectangle space corresponding to the grid column header, which is the area where all multiple headers will be manually drawn
          var baseHeaderAreaRectangle = GetCellDisplayRectangle(columnIndex, -1, true);

          // If the rectangle is empty it means the column is non in the visible scrolling area.
          if (baseHeaderAreaRectangle.IsEmpty)
          {
            continue;
          }

          // Compute the rectangle areas to manually draw the additional header
          var headerAreaWithSeparatorsRectangle = new Rectangle(
            baseHeaderAreaRectangle.Left,
            baseHeaderAreaRectangle.Top + accumulatedRowHeights,
            multiWidth,
            headerRow.Height);
          var headerDrawableAreaRectangle = new Rectangle(
            headerAreaWithSeparatorsRectangle.Left,
            headerAreaWithSeparatorsRectangle.Top,
            headerAreaWithSeparatorsRectangle.Width - Math.Max(ColumnHeadersSeparatorWidth, lastSpanningColumnDividerWidth + 1),
            headerAreaWithSeparatorsRectangle.Height - ColumnHeadersSeparatorWidth);
          var topPadding = UseColumnPaddings ? headerColumn.Style.Padding.Top : 0;
          var bottomPadding = UseColumnPaddings ? headerColumn.Style.Padding.Bottom : 0;
          var leftPadding = UseColumnPaddings ? headerColumn.Style.Padding.Left : 0;
          var rightPadding = UseColumnPaddings ? headerColumn.Style.Padding.Right : 0;
          var textAreaRectangle = new Rectangle(
            headerDrawableAreaRectangle.Left + leftPadding,
            headerDrawableAreaRectangle.Top + topPadding,
            headerDrawableAreaRectangle.Width - leftPadding - rightPadding,
            headerDrawableAreaRectangle.Height - bottomPadding - topPadding);

          // Draw area with separators
          backgroundBrush.Color = ColumnHeadersSeparatorColor;
          e.Graphics.FillRectangle(backgroundBrush, headerAreaWithSeparatorsRectangle);

          // Draw header area with its background color on top of the area with separators to emulate the separators
          backgroundBrush.Color = headerColumn.Style.BackColor;
          e.Graphics.FillRectangle(backgroundBrush, headerDrawableAreaRectangle);

          // Draw the header text
          foregroundBrush.Color = headerColumn.Style.ForeColor.IsEmpty
            ? ColumnHeadersDefaultCellStyle.ForeColor
            : headerColumn.Style.ForeColor;
          var headerFont = headerColumn.Style.Font ?? ColumnHeadersDefaultCellStyle.Font;
          e.Graphics.DrawString(headerColumn.Text, headerFont, foregroundBrush, textAreaRectangle, headerColumn.Style.Alignment.ToStringFormat());

          // Set the column span for the next iteration.
          previousColumnSpan = headerColumn.ColumnSpan;
        }

        accumulatedRowHeights += headerRow.Height;
      }

      foregroundBrush.Dispose();
      backgroundBrush.Dispose();
    }

    /// <summary>
    /// Raises the <see cref="DataGridView.Scroll"/> event.
    /// </summary>
    /// <param name="e">A <see cref="ScrollEventArgs"/> that contains the event data.</param>
    protected override void OnScroll(ScrollEventArgs e)
    {
      base.OnScroll(e);
      if (e.ScrollOrientation != ScrollOrientation.HorizontalScroll)
      {
        return;
      }

      Rectangle rtHeader = DisplayRectangle;
      rtHeader.Height = ColumnHeadersHeight;
      Invalidate(rtHeader);
    }

    /// <summary>
    /// Creates a <see cref="DataGridViewCellStyle"/> from the given <see cref="DataGridViewCell"/>, the grid's <see cref="DataGridView.ColumnHeadersDefaultCellStyle"/> or the <see cref="DataGridView"/> itself.
    /// </summary>
    /// <param name="fromCell">A <see cref="DataGridViewCell"/> to attempt to copy style values from.</param>
    /// <returns>A <see cref="DataGridViewCellStyle"/> instance.</returns>
    private DataGridViewCellStyle CreateColumnHeaderCellStyle(DataGridViewCell fromCell)
    {
      DataGridViewCellStyle fromStyle = fromCell != null && fromCell.HasStyle ? fromCell.Style : null;
      return CreateColumnHeaderCellStyle(fromStyle);
    }

    /// <summary>
    /// Creates a <see cref="DataGridViewCellStyle"/> from the given <see cref="DataGridViewCellStyle"/>, the grid's <see cref="DataGridView.ColumnHeadersDefaultCellStyle"/> or the <see cref="DataGridView"/> itself.
    /// </summary>
    /// <param name="fromStyle">A <see cref="DataGridViewCellStyle"/> to attempt to copy style values from.</param>
    /// <returns>A <see cref="DataGridViewCellStyle"/> instance.</returns>
    private DataGridViewCellStyle CreateColumnHeaderCellStyle(DataGridViewCellStyle fromStyle)
    {
      var newCellStyle = new DataGridViewCellStyle();
      DataGridViewCellStyle columnHeadersStyle = ColumnHeadersDefaultCellStyle;
      DataGridViewCellStyle dataGridViewStyle = DefaultCellStyle;
      if (fromStyle != null && !fromStyle.BackColor.IsEmpty)
      {
        newCellStyle.BackColor = fromStyle.BackColor;
      }
      else if (!columnHeadersStyle.BackColor.IsEmpty)
      {
        newCellStyle.BackColor = columnHeadersStyle.BackColor;
      }
      else
      {
        newCellStyle.BackColor = dataGridViewStyle.BackColor;
      }

      if (fromStyle != null && !fromStyle.ForeColor.IsEmpty)
      {
        newCellStyle.ForeColor = fromStyle.ForeColor;
      }
      else if (!columnHeadersStyle.ForeColor.IsEmpty)
      {
        newCellStyle.ForeColor = columnHeadersStyle.ForeColor;
      }
      else
      {
        newCellStyle.ForeColor = dataGridViewStyle.ForeColor;
      }

      if (fromStyle != null && !fromStyle.SelectionBackColor.IsEmpty)
      {
        newCellStyle.SelectionBackColor = fromStyle.SelectionBackColor;
      }
      else if (!columnHeadersStyle.SelectionBackColor.IsEmpty)
      {
        newCellStyle.SelectionBackColor = columnHeadersStyle.SelectionBackColor;
      }
      else
      {
        newCellStyle.SelectionBackColor = dataGridViewStyle.SelectionBackColor;
      }

      if (fromStyle != null && !fromStyle.SelectionForeColor.IsEmpty)
      {
        newCellStyle.SelectionForeColor = fromStyle.SelectionForeColor;
      }
      else if (!columnHeadersStyle.SelectionForeColor.IsEmpty)
      {
        newCellStyle.SelectionForeColor = columnHeadersStyle.SelectionForeColor;
      }
      else
      {
        newCellStyle.SelectionForeColor = dataGridViewStyle.SelectionForeColor;
      }

      if (fromStyle != null && fromStyle.Font != null)
      {
        newCellStyle.Font = fromStyle.Font;
      }
      else if (columnHeadersStyle.Font != null)
      {
        newCellStyle.Font = columnHeadersStyle.Font;
      }
      else
      {
        newCellStyle.Font = dataGridViewStyle.Font;
      }

      if (fromStyle != null && !fromStyle.IsNullValueDefault)
      {
        newCellStyle.NullValue = fromStyle.NullValue;
      }
      else if (!columnHeadersStyle.IsNullValueDefault)
      {
        newCellStyle.NullValue = columnHeadersStyle.NullValue;
      }
      else
      {
        newCellStyle.NullValue = dataGridViewStyle.NullValue;
      }

      if (fromStyle != null && !fromStyle.IsDataSourceNullValueDefault)
      {
        newCellStyle.DataSourceNullValue = fromStyle.DataSourceNullValue;
      }
      else if (!columnHeadersStyle.IsDataSourceNullValueDefault)
      {
        newCellStyle.DataSourceNullValue = columnHeadersStyle.DataSourceNullValue;
      }
      else
      {
        newCellStyle.DataSourceNullValue = dataGridViewStyle.DataSourceNullValue;
      }

      if (fromStyle != null && fromStyle.Format.Length != 0)
      {
        newCellStyle.Format = fromStyle.Format;
      }
      else if (columnHeadersStyle.Format.Length != 0)
      {
        newCellStyle.Format = columnHeadersStyle.Format;
      }
      else
      {
        newCellStyle.Format = dataGridViewStyle.Format;
      }

      if (fromStyle != null && !fromStyle.IsFormatProviderDefault)
      {
        newCellStyle.FormatProvider = fromStyle.FormatProvider;
      }
      else if (!columnHeadersStyle.IsFormatProviderDefault)
      {
        newCellStyle.FormatProvider = columnHeadersStyle.FormatProvider;
      }
      else
      {
        newCellStyle.FormatProvider = dataGridViewStyle.FormatProvider;
      }

      if (fromStyle != null && fromStyle.Alignment != DataGridViewContentAlignment.NotSet)
      {
        newCellStyle.Alignment = fromStyle.Alignment;
      }
      else if (columnHeadersStyle != null && columnHeadersStyle.Alignment != DataGridViewContentAlignment.NotSet)
      {
        newCellStyle.Alignment = columnHeadersStyle.Alignment;
      }
      else
      {
        newCellStyle.Alignment = dataGridViewStyle.Alignment;
      }

      if (fromStyle != null && fromStyle.WrapMode != DataGridViewTriState.NotSet)
      {
        newCellStyle.WrapMode = fromStyle.WrapMode;
      }
      else if (columnHeadersStyle != null && columnHeadersStyle.WrapMode != DataGridViewTriState.NotSet)
      {
        newCellStyle.WrapMode = columnHeadersStyle.WrapMode;
      }
      else
      {
        newCellStyle.WrapMode = dataGridViewStyle.WrapMode;
      }

      if (fromStyle != null && fromStyle.Tag != null)
      {
        newCellStyle.Tag = fromStyle.Tag;
      }
      else if (columnHeadersStyle != null && columnHeadersStyle.Tag != null)
      {
        newCellStyle.Tag = columnHeadersStyle.Tag;
      }
      else
      {
        newCellStyle.Tag = dataGridViewStyle.Tag;
      }

      if (fromStyle != null && fromStyle.Padding != Padding.Empty)
      {
        newCellStyle.Padding = fromStyle.Padding;
      }
      else if (columnHeadersStyle != null && columnHeadersStyle.Padding != Padding.Empty)
      {
        newCellStyle.Padding = columnHeadersStyle.Padding;
      }
      else
      {
        newCellStyle.Padding = dataGridViewStyle.Padding;
      }

      return newCellStyle;
    }

    /// <summary>
    /// Gets the accumulated heights, in pixels, of all header rows in this grid taking text heights and paddings into account.
    /// </summary>
    /// <param name="graphics">The <see cref="Graphics"/> instance used to draw the text.</param>
    /// <returns>The accumulated heights of all header rows.</returns>
    private int GetCumulativeColumnHeaderRowsHeight(Graphics graphics)
    {
      if (!AutoAdjustColumnHeadersHeight)
      {
        return FixedColumnHeadersHeight * (_multiHeaderRowsList.Count + 1);
      }

      int cumulativeHeight = GetMaxHeadersHeight(graphics);
      foreach (var headersRow in _multiHeaderRowsList)
      {
        headersRow.ComputeHeight(graphics, UseColumnPaddings, ColumnHeadersSeparatorWidth);
        cumulativeHeight += headersRow.Height;
      }

      return cumulativeHeight;
    }

    /// <summary>
    /// Gets the maximum height, in pixels, of the header cells taking into account header text and the top and bottom paddings.
    /// </summary>
    /// <param name="graphics">The <see cref="Graphics"/> instance used to draw the text.</param>
    /// <returns>The maximum height of the header cells.</returns>
    private int GetMaxHeadersHeight(Graphics graphics)
    {
      if (graphics == null)
      {
        return 0;
      }

      int maxHeight = 0;
      int separators = ColumnHeadersSeparatorWidth * 2;
      foreach (DataGridViewColumn column in Columns)
      {
        var style = CreateColumnHeaderCellStyle(column.HeaderCell);
        var text = string.IsNullOrEmpty(column.HeaderText) ? "Text" : column.HeaderText;
        var textHeight = Convert.ToInt32(graphics.MeasureString(text, style.Font).Height);
        var paddings = UseColumnPaddings
          ? column.HeaderCell.Style.Padding.Top + column.HeaderCell.Style.Padding.Bottom
          : 0;
        maxHeight = Math.Max(maxHeight, textHeight + paddings + separators);
      }

      return maxHeight;
    }
  }
}