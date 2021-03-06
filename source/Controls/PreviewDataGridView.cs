﻿// Copyright (c) 2012, 2019, Oracle and/or its affiliates. All rights reserved.
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
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using MySQL.ForExcel.Classes;
using MySQL.ForExcel.Properties;

namespace MySQL.ForExcel.Controls
{
  /// <summary>
  /// Displays data in a read-only grid for preview purposes only.
  /// </summary>
  public class PreviewDataGridView : DataGridView
  {
    #region Constants

    /// <summary>
    /// The default minimum column width, in pixels, of all columns in the grid.
    /// </summary>
    public const int DEFAULT_COLUMNS_MINIMUM_WIDTH = 5;

    #endregion Constants

    #region Fields

    /// <summary>
    /// The maximum column width, in pixels, of all columns in the grid.
    /// </summary>
    private int _columnsMaximumWidth;

    /// <summary>
    /// The minimum column width, in pixels, of all columns in the grid.
    /// </summary>
    private int _columnsMinimumWidth;

    /// <summary>
    /// A <see cref="DataTable"/> object containing a subset of the whole data which is shown in the preview grid.
    /// </summary>
    private readonly DataTable _previewDataTable;

    /// <summary>
    /// Flag indicating if recalculation of column width is not necessary so it must be skipped.
    /// </summary>
    private bool _skipWidthRecalculation;

    #endregion Fields

    /// <summary>
    /// Initializes a new instance of the <see cref="PreviewDataGridView"/> class.
    /// </summary>
    public PreviewDataGridView()
    {
      _columnsMaximumWidth = 0;
      _columnsMinimumWidth = DEFAULT_COLUMNS_MINIMUM_WIDTH;
      _previewDataTable = null;
      RowHeadersVisible = false;
      ShowCellErrors = false;
      ShowEditingIcon = false;
      ShowRowErrors = false;
      AllowUserToAddRows = false;
      AllowUserToDeleteRows = false;
      AllowUserToOrderColumns = false;
      AllowUserToResizeColumns = false;
      AllowUserToResizeRows = false;
      ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
      CellPainting += PreviewDataGridView_CellPainting;
      ReadOnly = true;
      RowHeadersWidthSizeMode = DataGridViewRowHeadersWidthSizeMode.DisableResizing;
      SelectAllAfterBindingComplete = false;
      ShowDataTypesOnColumnToolTips = true;
      AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
      DisableColumnsSelection = false;
    }

    #region Properties

    /// <summary>
    /// Gets a value indicating whether the option to add rows is displayed to the user.
    /// </summary>
    [Category("MySQL Custom"), Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    public new bool AllowUserToAddRows
    {
      get => base.AllowUserToAddRows;
      protected set => base.AllowUserToAddRows = value;
    }

    /// <summary>
    /// Gets a value indicating whether the user is allowed to delete rows from the DataGridView.
    /// </summary>
    [Category("MySQL Custom"), Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    public new bool AllowUserToDeleteRows
    {
      get => base.AllowUserToDeleteRows;
      protected set => base.AllowUserToDeleteRows = value;
    }

    /// <summary>
    /// Gets a value indicating whether manual column repositioning is enabled.
    /// </summary>
    [Category("MySQL Custom"), Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    public new bool AllowUserToOrderColumns
    {
      get => base.AllowUserToOrderColumns;
      protected set => base.AllowUserToOrderColumns = value;
    }

    /// <summary>
    /// Gets a value indicating whether users can resize columns.
    /// </summary>
    [Category("MySQL Custom"), Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    public new bool AllowUserToResizeColumns
    {
      get => base.AllowUserToResizeColumns;
      protected set => base.AllowUserToResizeColumns = value;
    }

    /// <summary>
    /// Gets a value indicating whether users can resize rows.
    /// </summary>
    [Category("MySQL Custom"), Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    public new bool AllowUserToResizeRows
    {
      get => base.AllowUserToResizeRows;
      protected set => base.AllowUserToResizeRows = value;
    }

    /// <summary>
    /// Gets a value indicating how column widths are determined.
    /// </summary>
    [Category("MySQL Custom"), Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    public new DataGridViewAutoSizeColumnsMode AutoSizeColumnsMode
    {
      get => base.AutoSizeColumnsMode;
      protected set => base.AutoSizeColumnsMode = value;
    }

    /// <summary>
    /// Gets a value indicating whether the height of the column headers is adjustable and whether it can be adjusted by the user or is automatically adjusted to fit the contents of the headers.
    /// </summary>
    [Category("MySQL Custom"), Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    public new DataGridViewColumnHeadersHeightSizeMode ColumnHeadersHeightSizeMode
    {
      get => base.ColumnHeadersHeightSizeMode;
      protected set => base.ColumnHeadersHeightSizeMode = value;
    }

    /// <summary>
    /// Gets or sets the maximum column width, in pixels, of all columns in the grid.
    /// </summary>
    [Category("MySQL Custom"), DefaultValue(0), Description("The maximum column width, in pixels, of all columns in the grid. If 0 the column is automatically sized to fit its contents.")]
    public int ColumnsMaximumWidth
    {
      get => _columnsMaximumWidth;

      set
      {
        _columnsMaximumWidth = value == 0 ? 0 : Math.Max(_columnsMinimumWidth, value);
        foreach (DataGridViewColumn col in Columns)
        {
          OnColumnWidthChanged(new DataGridViewColumnEventArgs(col));
        }
      }
    }

    /// <summary>
    /// Gets or sets the minimum column width, in pixels, of all columns in the grid.
    /// </summary>
    [Category("MySQL Custom"), DefaultValue(0), Description("The minimum column width, in pixels, of all columns in the grid. If 0 the column is automatically sized to fit its contents.")]
    public int ColumnsMinimumWidth
    {
      get => _columnsMinimumWidth;

      set
      {
        _columnsMinimumWidth = _columnsMaximumWidth > 0 ?  Math.Min(_columnsMaximumWidth, value) : value;
        foreach (DataGridViewColumn col in Columns)
        {
          col.MinimumWidth = _columnsMinimumWidth;
          OnColumnWidthChanged(new DataGridViewColumnEventArgs(col));
        }
      }
    }

    /// <summary>
    /// Gets or sets a value indicating if column selection is disabled for users.
    /// </summary>
    [Category("MySQL Custom"), DefaultValue(false), Description("A value indicating if column selection is disabled for users.")]
    public bool DisableColumnsSelection { get; set; }

    /// <summary>
    /// Gets a value indicating whether the user can edit the cells of the DataGridView control.
    /// </summary>
    [Category("MySQL Custom"), Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    public new bool ReadOnly
    {
      get => base.ReadOnly;
      protected set => base.ReadOnly = value;
    }

    /// <summary>
    /// Gets a value indicating whether the column that contains row headers is displayed.
    /// </summary>
    [Category("MySQL Custom"), Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    public new bool RowHeadersVisible
    {
      get => base.RowHeadersVisible;
      protected set => base.RowHeadersVisible = value;
    }

    /// <summary>
    /// Gets a value indicating whether the width of the row headers is adjustable and whether it can be adjusted by the user or is automatically adjusted to fit the contents of the headers.
    /// </summary>
    [Category("MySQL Custom"), Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    public new DataGridViewRowHeadersWidthSizeMode RowHeadersWidthSizeMode
    {
      get => base.RowHeadersWidthSizeMode;
      protected set => base.RowHeadersWidthSizeMode = value;
    }

    /// <summary>
    /// Gets or sets a value indicating whether all cells are selected after the data binding is done.
    /// </summary>
    [Category("MySQL Custom"), Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    public bool SelectAllAfterBindingComplete { get; set; }

    /// <summary>
    /// Gets a value indicating whether to show cell errors.
    /// </summary>
    [Category("MySQL Custom"), Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    public new bool ShowCellErrors
    {
      get => base.ShowCellErrors;
      protected set => base.ShowCellErrors = value;
    }

    /// <summary>
    /// Gets or sets a value indicating whether the grid column tool tips show the data type associated with the source table column.
    /// </summary>
    [Category("MySQL Custom"), Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    public bool ShowDataTypesOnColumnToolTips { get; set; }

    /// <summary>
    /// Gets a value indicating whether or not the editing glyph is visible in the row header of the cell being edited.
    /// </summary>
    [Category("MySQL Custom"), Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    public new bool ShowEditingIcon
    {
      get => base.ShowEditingIcon;
      protected set => base.ShowEditingIcon = value;
    }

    /// <summary>
    /// Gets a value indicating whether row headers will display error glyphs for each row that contains a data entry error.
    /// </summary>
    [Category("MySQL Custom"), Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    public new bool ShowRowErrors
    {
      get => base.ShowRowErrors;
      protected set => base.ShowRowErrors = value;
    }

    #endregion Properties

    /// <summary>
    /// Fills the <see cref="PreviewDataGridView"/> with data coming from the given <see cref="DataTable"/> instance.
    /// </summary>
    /// <param name="dataTable">A <see cref="DataTable"/> instance.</param>
    public void Fill(DataTable dataTable)
    {
      if (dataTable == null)
      {
        return;
      }

      DoubleBuffered = true;
      SelectionMode = DataGridViewSelectionMode.CellSelect;
      DataSource = dataTable;
      var nullImage = Resources._null;
      var blobImage = Resources.blob;
      foreach (DataGridViewColumn gridCol in Columns)
      {
        gridCol.SortMode = DataGridViewColumnSortMode.NotSortable;
        var imageColumn = gridCol as DataGridViewImageColumn;
        foreach (DataGridViewRow row in Rows)
        {
          var cell = row.Cells[gridCol.Index];
          if (imageColumn == null)
          {
            continue;
          }

          cell.Value = cell.Value == DBNull.Value ? nullImage : blobImage;
        }
      }

      SelectionMode = DataGridViewSelectionMode.FullColumnSelect;
      SelectAll();
    }

    /// <summary>
    /// Fills the <see cref="PreviewDataGridView"/> with data coming from the given <see cref="DbView"/> instance.
    /// </summary>
    /// <param name="dbTableOrView">A <see cref="DbView"/> instance.</param>
    public void Fill(DbView dbTableOrView)
    {
      if (dbTableOrView == null)
      {
        return;
      }

      var mySqlDataTable = new MySqlDataTable(dbTableOrView.Connection, dbTableOrView.Name, dbTableOrView.GetData(), MySqlDataTable.DataOperationType.ImportTableOrView, dbTableOrView.GetSelectQuery());
      Fill(mySqlDataTable);
    }

    /// <summary>
    /// Sets the tooltip text shown on column headers, containing the data type of each column.
    /// </summary>
    public void RefreshColumnHeaderDataTypeToolTips()
    {
      if (!ShowDataTypesOnColumnToolTips)
      {
        return;
      }

      if (!(DataSource is MySqlDataTable mySqlTable))
      {
        return;
      }

      foreach (DataGridViewColumn gridColumn in Columns)
      {
        if (!(mySqlTable.Columns[gridColumn.Index] is MySqlDataColumn mySqlColumn)
            || mySqlColumn.ServerDataType == null)
        {
          continue;
        }

        gridColumn.Tag = mySqlColumn.ServerDataType.TypeName;
        gridColumn.ToolTipText = gridColumn.Tag.ToString();
      }
    }

    /// <summary>
    /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
    /// </summary>
    /// <param name="disposing">Indicates whether the method was invoked from the <see cref="IDisposable.Dispose"/> implementation or from the finalizer.</param>
    protected override void Dispose(bool disposing)
    {
      if (disposing)
      {
        _previewDataTable?.Dispose();
      }

      base.Dispose(disposing);
    }

    /// <summary>
    /// Raises the <see cref="DataGridView.CellMouseDown"/> event.
    /// </summary>
    /// <param name="e">A DataGridViewCellMouseEventArgs that contains the event data.</param>
    protected override void OnCellMouseDown(DataGridViewCellMouseEventArgs e)
    {
      if (DisableColumnsSelection)
      {
        return;
      }

      base.OnCellMouseDown(e);
    }

    /// <summary>
    /// Raises the <see cref="DataGridView.CellValueChanged"/> event.
    /// </summary>
    /// <param name="e">A DataGridViewCellEventArgs that contains the event data.</param>
    protected override void OnCellValueChanged(DataGridViewCellEventArgs e)
    {
      if (_skipWidthRecalculation)
      {
        return;
      }

      base.OnCellValueChanged(e);
      if (e.RowIndex < 0)
      {
        Columns[e.ColumnIndex].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
      }
    }

    /// <summary>
    /// Raises the <see cref="DataGridView.ColumnAdded"/> event.
    /// </summary>
    /// <param name="e">A <see cref="DataGridViewColumnEventArgs"/> that contains the event data.</param>
    protected override void OnColumnAdded(DataGridViewColumnEventArgs e)
    {
      base.OnColumnAdded(e);
      e.Column.MinimumWidth = _columnsMinimumWidth;
    }

    /// <summary>
    /// Raises the <see cref="DataGridView.ColumnWidthChanged"/> event.
    /// </summary>
    /// <param name="e">A <see cref="DataGridViewColumnEventArgs"/> that contains the event data.</param>
    protected override void OnColumnWidthChanged(DataGridViewColumnEventArgs e)
    {
      if (_skipWidthRecalculation)
      {
        return;
      }

      base.OnColumnWidthChanged(e);
      PerformColumnWidthRecalculation(e.Column);
    }

    /// <summary>
    /// Raises the <see cref="DataGridView.DataBindingComplete"/> event.
    /// </summary>
    /// <param name="e">A <see cref="DataGridViewBindingCompleteEventArgs"/> that contains the event data.</param>
    protected override void OnDataBindingComplete(DataGridViewBindingCompleteEventArgs e)
    {
      base.OnDataBindingComplete(e);
      RefreshColumnHeaderDataTypeToolTips();
      if (SelectAllAfterBindingComplete)
      {
        SelectAll();
      }
    }

    /// <summary>
    /// Raises the <see cref="DataGridView.DataError"/> event.
    /// </summary>
    /// <param name="displayErrorDialogIfNoHandler">A <see cref="DataGridViewDataErrorEventArgs"/> that contains the event data.</param>
    /// <param name="e"></param>
    protected override void OnDataError(bool displayErrorDialogIfNoHandler, DataGridViewDataErrorEventArgs e)
    {
      // Do not error out.
    }

    /// <summary>
    /// Recalculates the given <see cref="DataGridViewColumn"/> width comparing it to the <see cref="ColumnsMaximumWidth"/> value.
    /// </summary>
    /// <param name="column">A <see cref="DataGridViewColumn"/> instance.</param>
    protected void PerformColumnWidthRecalculation(DataGridViewColumn column)
    {
      var wrapText = false;
      var cappedWidth = column.Width;
      if (column.Width > _columnsMaximumWidth && _columnsMaximumWidth > 0)
      {
        cappedWidth = _columnsMaximumWidth;
        wrapText = true;
      }
      else if (column.Width < column.MinimumWidth)
      {
        cappedWidth = column.MinimumWidth;
      }

      if (column.Width != cappedWidth)
      {
        _skipWidthRecalculation = true;
        column.AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
        column.HeaderCell.Style.WrapMode = wrapText ? DataGridViewTriState.True : DataGridViewTriState.False;
        column.Width = cappedWidth;
        _skipWidthRecalculation = false;
      }
    }

    /// <summary>
    /// Event delegate method fired when the <see cref="PreviewDataGridView"/> paints every cell.
    /// </summary>
    /// <param name="sender">Sender object.</param>
    /// <param name="e">Event arguments.</param>
    private void PreviewDataGridView_CellPainting(object sender, DataGridViewCellPaintingEventArgs e)
    {
      e.PaintBackground(e.ClipBounds, true);
      if (e.Value == DBNull.Value)
      {
        var nullImage = Resources._null;
        e.Graphics.DrawImage(nullImage, e.CellBounds.Left + (e.CellBounds.Width - nullImage.Width) / 2, e.CellBounds.Top + (e.CellBounds.Height - nullImage.Height) / 2);
        e.PaintContent(e.ClipBounds);
      }
      else if (e.Value.GetType() == typeof(byte[]))
      {
        var blobImage = Resources.blob;
        e.PaintBackground(e.ClipBounds, true);
        var imageRectangle = new Rectangle(e.CellBounds.Left + (e.CellBounds.Width - blobImage.Width) / 2,
                                           e.CellBounds.Top + (e.CellBounds.Height - blobImage.Height) / 2,
                                           blobImage.Width,
                                           blobImage.Height);
        //e.Graphics.DrawImage(blobImage, e.CellBounds.Left + (e.CellBounds.Width - blobImage.Width) / 2, e.CellBounds.Top + (e.CellBounds.Height - blobImage.Height) / 2);
        e.Graphics.DrawImage(blobImage, imageRectangle);
      }
      else
      {
        e.PaintContent(e.ClipBounds);
      }

      e.Handled = true;
    }
  }
}