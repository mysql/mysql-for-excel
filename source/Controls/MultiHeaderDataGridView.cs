// Copyright (c) 2012, 2018, Oracle and/or its affiliates. All rights reserved.
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
using System.Linq;
using System.Windows.Forms;
using MySQL.ForExcel.Classes;
using MySQL.ForExcel.Classes.EventArguments;

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
    /// The horizontal alignment of the base column headers text.
    /// </summary>
    private HorizontalAlignment _baseColumnHeadersTextAlignment;

    /// <summary>
    /// The height, in pixels, of the base grid header row taking into account header text and the top and bottom paddings.
    /// </summary>
    private int _baseHeadersRowHeight;

    /// <summary>
    /// The <see cref="Color"/> of the column headers separators.
    /// </summary>
    /// <remarks>This color is only applied when <seealso cref="AllowChangingHeaderCellsColors"/> is true (so <seealso cref="EnableHeadersVisualStyles"/> is false).</remarks>
    private Color _columnHeadersSeparatorColor;

    /// <summary>
    /// The width, in pixels, of the column header separators.
    /// </summary>
    private int _columnHeadersSeparatorWidth;

    /// <summary>
    /// A list of header rows each containing <see cref="MultiHeaderCell"/> objects.
    /// </summary>
    private readonly List<MultiHeaderRow> _multiHeaderRowsList;

    /// <summary>
    /// A value indicating whether the additional header rows defined in the <seealso cref="MultiHeaderRowsCollection"/> is reversed.
    /// </summary>
    /// <remarks>If <c>true</c> the order of rows is from the original header row up, if <c>false</c> the order is from the top of the grid down.</remarks>
    private bool _reverseMultiHeaderRowOrder;

    /// <summary>
    /// Flag indicating whether the the call to <see cref="AdjustColumnsWidth"/> in the <see cref="OnColumnWidthChanged"/> event should be skipped.
    /// </summary>
    private bool _skipColumnWidthsAdjustment;

    /// <summary>
    /// Flag indicating whether the size of a header column cell is calculated adding the padding size on top of the text size.
    /// </summary>
    private bool _useColumnPaddings;

    /// <summary>
    /// Flag indicating whether the height of a column headers row is set to the value of <seealso cref="FixedColumnHeadersHeight"/> or computed based on their contents.
    /// </summary>
    private bool _useFixedColumnHeadersHeight;

    #endregion Fields

    /// <summary>
    /// Initializes a new instance of the <see cref="MultiHeaderDataGridView"/> class.
    /// </summary>
    public MultiHeaderDataGridView()
    {
      _baseColumnHeadersTextAlignment = HorizontalAlignment.Center;
      _baseHeadersRowHeight = 0;
      _columnHeadersSeparatorColor = SystemColors.ControlDark;
      _multiHeaderRowsList = new List<MultiHeaderRow>(INITIAL_ROWS_QUANTITY);
      _reverseMultiHeaderRowOrder = false;
      _skipColumnWidthsAdjustment = false;
      _useColumnPaddings = true;
      _useFixedColumnHeadersHeight = false;
      AllowChangingHeaderCellsColors = true;
      AllowDrop = true;
      AutoSizeColumnsBasedOnAdditionalHeadersContent = true;
      ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.EnableResizing;
      ColumnHeadersSeparatorWidth = DEFAULT_HEADERS_SEPARATOR_WIDTH;
      DoubleBuffered = true;
      FixedColumnHeadersHeight = DEFAULT_COLUMN_HEADERS_HEIGHT;
    }

    #region Properties

    /// <summary>
    /// Gets a value indicating whether row and column headers use the visual styles of the user's current theme if visual styles are enabled for the application.
    /// </summary>
    [Category("MySQL Custom"), Description("Flag indicating whether row and column headers use the visual styles of the user's current theme if visual styles are enabled for the application.")]
    public bool AllowChangingHeaderCellsColors
    {
      get => _allowChangingHeaderCellsColors;

      set
      {
        _allowChangingHeaderCellsColors = value;
        EnableHeadersVisualStyles = !value;
      }
    }

    /// <summary>
    /// Gets or sets a value indicating whether the widths of columns are automatically resized taking in consideration the contents of the additional header cells.
    /// </summary>
    /// <remarks>This property only has effect if the value of <see cref="DataGridView.AutoSizeColumnsMode"/> is different to <see cref="DataGridViewAutoSizeColumnsMode.None"/></remarks>
    [Category("MySQL Custom"), Description("Flag indicating whether the widths of columns are automatically resized taking in consideration the contents of the additional header cells.")]
    public bool AutoSizeColumnsBasedOnAdditionalHeadersContent { get; set; }

    /// <summary>
    /// Gets or sets the horizontal alignment of the base column headers text.
    /// </summary>
    [Category("MySQL Custom"), Description("The horizontal alignment of the base column headers text.")]
    public HorizontalAlignment BaseColumnHeadersTextAlignment
    {
      get => _baseColumnHeadersTextAlignment;

      set
      {
        _baseColumnHeadersTextAlignment = value;
        InvalidateHeadersVisibleArea();
      }
    }

    /// <summary>
    /// Gets the height, in pixels, of the column headers row(s).
    /// </summary>
    [Category("MySQL Custom"), Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    public new int ColumnHeadersHeight
    {
      get => base.ColumnHeadersHeight;
      private set => base.ColumnHeadersHeight = value;
    }

    /// <summary>
    /// Gets a value indicating whether the height of the column headers is adjustable and whether it can be adjusted by the user or is automatically adjusted to fit the contents of the headers.
    /// </summary>
    [Category("MySQL Custom"), Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    public new DataGridViewColumnHeadersHeightSizeMode ColumnHeadersHeightSizeMode
    {
      get => base.ColumnHeadersHeightSizeMode;
      private set => base.ColumnHeadersHeightSizeMode = value;
    }

    /// <summary>
    /// Gets or sets the <see cref="Color"/> of the column headers separators.
    /// </summary>
    /// <remarks>This color is only applied when <seealso cref="AllowChangingHeaderCellsColors"/> is true (so <seealso cref="EnableHeadersVisualStyles"/> is false).</remarks>
    [Category("MySQL Custom"), Description("The color of the column headers separators (only applied when AllowChangingHeaderCellsColors is true).")]
    public Color ColumnHeadersSeparatorColor
    {
      get => _columnHeadersSeparatorColor;

      set
      {
        _columnHeadersSeparatorColor = value;
        InvalidateHeadersVisibleArea();
      }
    }

    /// <summary>
    /// Gets or sets the width, in pixels, of the column header separators.
    /// </summary>
    [Category("MySQL Custom"), Description("The width, in pixels, of the column header separators.")]
    public int ColumnHeadersSeparatorWidth
    {
      get => _columnHeadersSeparatorWidth;

      set
      {
        _columnHeadersSeparatorWidth = value;
        RecalculateHeaderRowsSizes();
      }
    }

    /// <summary>
    /// Gets a value indicating whether this control should redraw its surface using a secondary buffer to reduce or prevent flicker.
    /// </summary>
    [Category("MySQL Custom"), Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    protected override bool DoubleBuffered
    {
      get => base.DoubleBuffered;
      set => base.DoubleBuffered = value;
    }

    /// <summary>
    /// Gets a value indicating whether row and column headers use the visual styles of the user's current theme if visual styles are enabled for the application.
    /// </summary>
    [Category("MySQL Custom"), Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    public new bool EnableHeadersVisualStyles
    {
      get => base.EnableHeadersVisualStyles;
      private set => base.EnableHeadersVisualStyles = value;
    }

    /// <summary>
    /// Gets or sets a fixed height, in pixels, for each column headers row.
    /// </summary>
    [Category("MySQL Custom"), Description("A fixed height, in pixels, for each column headers row.")]
    public int FixedColumnHeadersHeight { get; set; }

    /// <summary>
    /// Gets a read-only collection of header rows each containing <see cref="MultiHeaderCell"/> objects.
    /// </summary>
    [Category("MySQL Custom"), Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    public ReadOnlyCollection<MultiHeaderRow> MultiHeaderRowsCollection => _multiHeaderRowsList.AsReadOnly();

    /// <summary>
    /// Gets or sets a value indicating whether the additional header rows defined in the <seealso cref="MultiHeaderRowsCollection"/> is reversed.
    /// </summary>
    /// <remarks>If <c>true</c> the order of rows is from the original header row up, if <c>false</c> the order is from the top of the grid down.</remarks>
    [Category("MySQL Custom"), Description("Flag indicating whether the additional header rows defined in the MultiHeaderRowsCollection is reversed.")]
    public bool ReverseMultiHeaderRowOrder
    {
      get => _reverseMultiHeaderRowOrder;

      set
      {
        _reverseMultiHeaderRowOrder = value;
        InvalidateHeadersVisibleArea();
      }
    }

    /// <summary>
    /// Gets or sets a value indicating whether the size of a header column cell is calculated adding the padding size on top of the text size.
    /// </summary>
    [Category("MySQL Custom"), Description("Flag indicating whether the size of a header column cell is calculated adding the padding size on top of the text size.")]
    public bool UseColumnPaddings
    {
      get => _useColumnPaddings;

      set
      {
        _useColumnPaddings = value;
        RecalculateHeaderRowsSizes();
      }
    }

    /// <summary>
    /// Gets or sets a value indicating whether the height of a column headers row is set to the value of <seealso cref="FixedColumnHeadersHeight"/> or computed based on their contents.
    /// </summary>
    [Category("MySQL Custom"), Description("Flag indicating whether the height of a column headers row is set to the value of FixedColumnHeadersHeight or computed based on their contents.")]
    public bool UseFixedColumnHeadersHeight
    {
      get => _useFixedColumnHeadersHeight;

      set
      {
        _useFixedColumnHeadersHeight = value;
        RecalculateBaseHeadersRowHeight();
      }
    }

    #endregion Properties

    /// <summary>
    /// Adds a new <see cref="MultiHeaderRow"/> to the header rows collection.
    /// </summary>
    /// <param name="copyStyleFromColumnHeader">Flag indicating whether the style to use for the new header row is copied from the grid's header style, otherwise the default style is used.</param>
    /// <param name="autoGenerateHeaderText">If <c>true</c> header text will be autogenerated for all headers, otherwise they will be empty.</param>
    public void AddHeadersRow(bool copyStyleFromColumnHeader, bool autoGenerateHeaderText = false)
    {
      var columnsCount = Columns.Count;
      if (_multiHeaderRowsList == null || columnsCount == 0)
      {
        // Do not add any rows if columns have not been defined for the grid control.
        return;
      }

      var headersRow = new MultiHeaderRow(columnsCount);
      for (var columnIndex = 0; columnIndex < Columns.Count; columnIndex++)
      {
        var headerText = autoGenerateHeaderText
          ? $"Header{_multiHeaderRowsList.Count}-{columnIndex}"
          : string.Empty;
        var column = Columns[columnIndex];
        var style = CreateColumnHeaderCellStyle(copyStyleFromColumnHeader ? column.HeaderCell : null);
        headersRow.Add(headersRow.NewHeaderCell(headerText, style));
      }

      headersRow.HeaderCellColumnSpanChanged += HeaderCellColumnSpanChanged;
      headersRow.HeaderCellTextChanged += HeaderCellTextChanged;
      _multiHeaderRowsList.Add(headersRow);
      RecalculateHeaderRowsSizes();
      Invalidate();
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
      RecalculateHeaderRowsSizes();
    }

    /// <summary>
    /// Calculates the sizes, in pixels, of all header rows in this grid taking text heights and paddings into account.
    /// </summary>
    public void RecalculateHeaderRowsSizes()
    {
      RecalculateBaseHeadersRowHeight();
      foreach (var headersRow in _multiHeaderRowsList)
      {
        headersRow.RecalculateCellSizes();
        foreach (var headerCell in headersRow)
        {
          AdjustColumnsWidth(headerCell);
        }
      }

      Invalidate();
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
    /// Raises the <see cref="DataGridView.CellValueChanged"/> event.
    /// </summary>
    /// <param name="e">A DataGridViewCellEventArgs that contains the event data.</param>
    protected override void OnCellValueChanged(DataGridViewCellEventArgs e)
    {
      base.OnCellValueChanged(e);
      var baseColumn = Columns[e.ColumnIndex];
      if (e.RowIndex == 0)
      {
        // Recalculate the _baseHeadersRowHeight since a header column text changed.
        RecalculateBaseHeadersRowHeight(baseColumn);
      }
      else
      {
        baseColumn.AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
      }
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
      if (!_skipColumnWidthsAdjustment)
      {
        // Go through the additional header cells relative to the column changing widths to recalculate column width if needed
        foreach (var headerCell in _multiHeaderRowsList.Select(headerRow => headerRow.FirstOrDefault(hCell => hCell.ColumnIndex == e.Column.Index)).Where(headerCell => headerCell != null))
        {
          AdjustColumnsWidth(headerCell);
        }
      }

      base.OnColumnWidthChanged(e);
      InvalidateHeadersVisibleArea();
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

      // Enforce that the alignment of the original header text is at the bottom, so the other header rows can be drawn correctly.
      ColumnHeadersDefaultCellStyle.Alignment = BaseColumnHeadersTextAlignment.ToBottomAlignment();

      // Get the total height for header rows
      ColumnHeadersHeight = GetTotalHeaderRowsHeight();

      if (_multiHeaderRowsList == null || _multiHeaderRowsList.Count == 0)
      {
        return;
      }

      // Set other properties and process each additional row.
      var rowsQuantity = _multiHeaderRowsList.Count;
      var foregroundBrush = new SolidBrush(ColumnHeadersDefaultCellStyle.ForeColor);
      var backgroundBrush = new SolidBrush(ColumnHeadersDefaultCellStyle.BackColor);
      var accumulatedRowHeights = 0;
      for (var rowIndex = 0; rowIndex < rowsQuantity; rowIndex++)
      {
        var reversedRowIndex = ReverseMultiHeaderRowOrder ? rowsQuantity - rowIndex - 1 : rowIndex;
        var headerRow = _multiHeaderRowsList[reversedRowIndex];
        var headerRowHeight = UseFixedColumnHeadersHeight ? FixedColumnHeadersHeight : headerRow.Height;
        foreach (var headerCell in headerRow.Where(headerCell => !headerCell.InSpan))
        {
          // Calculate the information for the cells being spanned by the currently processed headerCell.
          var spanningInfo = MultiHeaderCellsSpanningInfo.GetHeaderCellSpanningInfo(this, headerCell);

          // If the spanning columns are not fully visible, then skip.
          if (spanningInfo.VisibleArea == Rectangle.Empty)
          {
            continue;
          }

          // Compute the rectangle areas to manually draw the additional header
          var headerAreaWithSeparatorsRectangle = new Rectangle(
            spanningInfo.VisibleArea.Left,
            spanningInfo.VisibleArea.Top + accumulatedRowHeights,
            spanningInfo.VisibleWidth,
            headerRowHeight);
          e.Graphics.SetClip(headerAreaWithSeparatorsRectangle);
          var headerDrawableAreaRectangle = new Rectangle(
            headerAreaWithSeparatorsRectangle.Left - spanningInfo.LeftOverflowingWidth,
            headerAreaWithSeparatorsRectangle.Top,
            spanningInfo.TotalSpanningWidth - Math.Max(ColumnHeadersSeparatorWidth, spanningInfo.RightDividerWidth + 1),
            headerAreaWithSeparatorsRectangle.Height - ColumnHeadersSeparatorWidth);
          var topPadding = UseColumnPaddings ? headerCell.Style.Padding.Top : 0;
          var bottomPadding = UseColumnPaddings ? headerCell.Style.Padding.Bottom : 0;
          var leftPadding = UseColumnPaddings ? headerCell.Style.Padding.Left : 0;
          var rightPadding = UseColumnPaddings ? headerCell.Style.Padding.Right : 0;
          var textAreaRectangle = new Rectangle(
            headerDrawableAreaRectangle.Left + leftPadding,
            headerDrawableAreaRectangle.Top + topPadding,
            headerDrawableAreaRectangle.Width - leftPadding - rightPadding,
            headerDrawableAreaRectangle.Height - bottomPadding - topPadding);

          // Draw area with separators
          backgroundBrush.Color = ColumnHeadersSeparatorColor;
          e.Graphics.FillRectangle(backgroundBrush, headerAreaWithSeparatorsRectangle);

          // Draw header area with its background color on top of the area with separators to emulate the separators
          backgroundBrush.Color = headerCell.Style.BackColor;
          e.Graphics.FillRectangle(backgroundBrush, headerDrawableAreaRectangle);

          // Draw the header text
          foregroundBrush.Color = headerCell.Style.ForeColor.IsEmpty
            ? ColumnHeadersDefaultCellStyle.ForeColor
            : headerCell.Style.ForeColor;
          var headerFont = headerCell.Style.Font ?? ColumnHeadersDefaultCellStyle.Font;
          e.Graphics.DrawString(headerCell.Text, headerFont, foregroundBrush, textAreaRectangle, headerCell.Style.Alignment.ToStringFormat());
        }

        accumulatedRowHeights += headerRowHeight;
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

      InvalidateHeadersVisibleArea();
    }

    /// <summary>
    /// Adjusts the column width relative to the given <see cref="MultiHeaderCell"/> based on its width.
    /// </summary>
    /// <param name="headerCell">A <see cref="MultiHeaderCell"/> object.</param>
    private void AdjustColumnsWidth(MultiHeaderCell headerCell)
    {
      if (headerCell == null || !AutoSizeColumnsBasedOnAdditionalHeadersContent || AutoSizeColumnsMode == DataGridViewAutoSizeColumnsMode.None)
      {
        return;
      }

      // Calculate the total width of the base columns the headerCell spans
      var spanningInfo = MultiHeaderCellsSpanningInfo.GetHeaderCellSpanningInfo(this, headerCell);

      // Nothing to do if the spanning columns width already accommodates space for the computed headerCell's width
      if (spanningInfo.TotalSpanningWidth >= headerCell.CellSize.Width)
      {
        return;
      }

      // Re-adjust grid's columns widths spanned by the headerCell
      var proportionalWidthToIncrease = Math.DivRem(headerCell.CellSize.Width - spanningInfo.VisibleWidth, headerCell.ColumnSpan, out var remainder);
      if (remainder > 0)
      {
        proportionalWidthToIncrease++;
      }

      var lastColumnIndex = headerCell.GetLastBaseColumnIndexFromSpan(Columns.Count);
      for (var idx = headerCell.ColumnIndex; idx <= lastColumnIndex; idx++)
      {
        var spanningColumn = Columns[idx];
        var newWidth = spanningColumn.Width + proportionalWidthToIncrease;
        _skipColumnWidthsAdjustment = true;
        spanningColumn.AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
        spanningColumn.Width = newWidth;
        _skipColumnWidthsAdjustment = false;
      }
    }

    /// <summary>
    /// Creates a <see cref="DataGridViewCellStyle"/> from the given <see cref="DataGridViewCell"/>, the grid's <see cref="DataGridView.ColumnHeadersDefaultCellStyle"/> or the <see cref="DataGridView"/> itself.
    /// </summary>
    /// <param name="fromCell">A <see cref="DataGridViewCell"/> to attempt to copy style values from.</param>
    /// <returns>A <see cref="DataGridViewCellStyle"/> instance.</returns>
    private DataGridViewCellStyle CreateColumnHeaderCellStyle(DataGridViewCell fromCell)
    {
      var fromStyle = fromCell != null && fromCell.HasStyle ? fromCell.Style : null;
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
      var columnHeadersStyle = ColumnHeadersDefaultCellStyle;
      var dataGridViewStyle = DefaultCellStyle;
      newCellStyle.BackColor = fromStyle != null && !fromStyle.BackColor.IsEmpty
        ? fromStyle.BackColor
        : !columnHeadersStyle.BackColor.IsEmpty
          ? columnHeadersStyle.BackColor
          : dataGridViewStyle.BackColor;

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

      if (fromStyle?.Font != null)
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

      if (fromStyle?.Tag != null)
      {
        newCellStyle.Tag = fromStyle.Tag;
      }
      else if (columnHeadersStyle?.Tag != null)
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
    /// Event delegate method fired when the <see cref="MultiHeaderCell.ColumnSpan"/> of a cell in one of the <see cref="MultiHeaderRow"/> objects in this grid changes value.
    /// </summary>
    /// <param name="sender">A <see cref="MultiHeaderRow"/> in this grid.</param>
    /// <param name="args">The <see cref="HeaderCellColumnSpanChangedArgs"/> related to the event.</param>
    private void HeaderCellColumnSpanChanged(object sender, HeaderCellColumnSpanChangedArgs args)
    {
      if (!(sender is MultiHeaderRow))
      {
        return;
      }

      AdjustColumnsWidth(args.HeaderCell);
    }

    /// <summary>
    /// Event delegate method fired when the <see cref="MultiHeaderCell.ColumnSpan"/> of a cell in one of the <see cref="MultiHeaderRow"/> objects in this grid changes value.
    /// </summary>
    /// <param name="sender">A <see cref="MultiHeaderRow"/> in this grid.</param>
    /// <param name="args">The <see cref="HeaderCellColumnSpanChangedArgs"/> related to the event.</param>
    private void HeaderCellTextChanged(object sender, HeaderCellTextChangedArgs args)
    {
      if (!(sender is MultiHeaderRow))
      {
        return;
      }

      AdjustColumnsWidth(args.HeaderCell);
    }

    /// <summary>
    /// Invalidates the headers visible area so it is repainted.
    /// </summary>
    private void InvalidateHeadersVisibleArea()
    {
      var headerRectangle = DisplayRectangle;
      headerRectangle.Height = ColumnHeadersHeight;
      Invalidate(headerRectangle);
    }

    /// <summary>
    /// Gets the accumulated heights, in pixels, of the base header row and all additional header rows in this grid.
    /// </summary>
    /// <returns>The accumulated header row heights, in pixels.</returns>
    private int GetTotalHeaderRowsHeight()
    {
      if (UseFixedColumnHeadersHeight)
      {
        return FixedColumnHeadersHeight * (_multiHeaderRowsList.Count + 1);
      }

      // If the height is 0, it means the texts for this base header row have not been calculated yet, so force the calculation.
      if (_baseHeadersRowHeight == 0)
      {
        RecalculateBaseHeadersRowHeight();
      }

      var totalHeight = _baseHeadersRowHeight + _multiHeaderRowsList.Sum(headerRow => headerRow.Height);

      // If the height is still 0 (which is an invalid value), set it then to the FixedColumnHeadersHeight
      if (totalHeight == 0)
      {
        totalHeight = FixedColumnHeadersHeight;
      }

      return totalHeight;
    }

    /// <summary>
    /// Recalculates the <see cref="_baseHeadersRowHeight"/> based on the contents of a given <see cref="DataGridViewColumn"/> header cell.
    /// </summary>
    /// <param name="column">A <see cref="DataGridViewColumn"/> that had a text change.</param>
    private void RecalculateBaseHeadersRowHeight(DataGridViewColumn column)
    {
      if (column == null)
      {
        return;
      }

      // No need to recalculate, use the fixed headers height
      if (UseFixedColumnHeadersHeight)
      {
        _baseHeadersRowHeight = FixedColumnHeadersHeight;
        return;
      }

      // Recalculate base header row height based on text sizes, paddings and separators width
      var separators = ColumnHeadersSeparatorWidth * 2;
      var style = CreateColumnHeaderCellStyle(column.HeaderCell);
      var text = string.IsNullOrEmpty(column.HeaderText) ? "Text" : column.HeaderText;
      var textHeight = TextRenderer.MeasureText(text, style.Font).Height;
      var paddings = UseColumnPaddings ? style.Padding.Top + style.Padding.Bottom : 0;
      _baseHeadersRowHeight = Math.Max(_baseHeadersRowHeight, textHeight + paddings + separators);
    }

    /// <summary>
    /// Recalculates the <see cref="_baseHeadersRowHeight"/> based on the contents of header cells for all <see cref="DataGridViewColumn"/>s.
    /// </summary>
    private void RecalculateBaseHeadersRowHeight()
    {
      for (var colIndex = 0; colIndex < (_useFixedColumnHeadersHeight ? 0 : Columns.Count); colIndex++)
      {
        var column = Columns[colIndex];
        RecalculateBaseHeadersRowHeight(column);
      }
    }
  }
}