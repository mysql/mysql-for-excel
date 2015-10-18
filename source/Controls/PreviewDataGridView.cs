// Copyright (c) 2012, 2015, Oracle and/or its affiliates. All rights reserved.
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
using System.Windows.Forms;

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
      ReadOnly = true;
      RowHeadersWidthSizeMode = DataGridViewRowHeadersWidthSizeMode.DisableResizing;
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
      get
      {
        return base.AllowUserToAddRows;
      }

      protected set
      {
        base.AllowUserToAddRows = value;
      }
    }

    /// <summary>
    /// Gets a value indicating whether the user is allowed to delete rows from the DataGridView.
    /// </summary>
    [Category("MySQL Custom"), Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    public new bool AllowUserToDeleteRows
    {
      get
      {
        return base.AllowUserToDeleteRows;
      }

      protected set
      {
        base.AllowUserToDeleteRows = value;
      }
    }

    /// <summary>
    /// Gets a value indicating whether manual column repositioning is enabled.
    /// </summary>
    [Category("MySQL Custom"), Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    public new bool AllowUserToOrderColumns
    {
      get
      {
        return base.AllowUserToOrderColumns;
      }

      protected set
      {
        base.AllowUserToOrderColumns = value;
      }
    }

    /// <summary>
    /// Gets a value indicating whether users can resize columns.
    /// </summary>
    [Category("MySQL Custom"), Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    public new bool AllowUserToResizeColumns
    {
      get
      {
        return base.AllowUserToResizeColumns;
      }

      protected set
      {
        base.AllowUserToResizeColumns = value;
      }
    }

    /// <summary>
    /// Gets a value indicating whether users can resize rows.
    /// </summary>
    [Category("MySQL Custom"), Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    public new bool AllowUserToResizeRows
    {
      get
      {
        return base.AllowUserToResizeRows;
      }

      protected set
      {
        base.AllowUserToResizeRows = value;
      }
    }

    /// <summary>
    /// Gets a value indicating how column widths are determined.
    /// </summary>
    [Category("MySQL Custom"), Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    public new DataGridViewAutoSizeColumnsMode AutoSizeColumnsMode
    {
      get
      {
        return base.AutoSizeColumnsMode;
      }

      protected set
      {
        base.AutoSizeColumnsMode = value;
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

      protected set
      {
        base.ColumnHeadersHeightSizeMode = value;
      }
    }

    /// <summary>
    /// Gets or sets the maximum column width, in pixels, of all columns in the grid.
    /// </summary>
    [Category("MySQL Custom"), DefaultValue(0), Description("The maximum column width, in pixels, of all columns in the grid. If 0 the column is automatically sized to fit its contents.")]
    public int ColumnsMaximumWidth
    {
      get
      {
        return _columnsMaximumWidth;
      }

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
      get
      {
        return _columnsMinimumWidth;
      }

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
      get
      {
        return base.ReadOnly;
      }

      protected set
      {
        base.ReadOnly = value;
      }
    }

    /// <summary>
    /// Gets a value indicating whether the column that contains row headers is displayed.
    /// </summary>
    [Category("MySQL Custom"), Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    public new bool RowHeadersVisible
    {
      get
      {
        return base.RowHeadersVisible;
      }

      protected set
      {
        base.RowHeadersVisible = value;
      }
    }

    /// <summary>
    /// Gets a value indicating whether the width of the row headers is adjustable and whether it can be adjusted by the user or is automatically adjusted to fit the contents of the headers.
    /// </summary>
    [Category("MySQL Custom"), Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    public new DataGridViewRowHeadersWidthSizeMode RowHeadersWidthSizeMode
    {
      get
      {
        return base.RowHeadersWidthSizeMode;
      }

      protected set
      {
        base.RowHeadersWidthSizeMode = value;
      }
    }

    /// <summary>
    /// Gets a value indicating whether to show cell errors.
    /// </summary>
    [Category("MySQL Custom"), Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    public new bool ShowCellErrors
    {
      get
      {
        return base.ShowCellErrors;
      }

      protected set
      {
        base.ShowCellErrors = value;
      }
    }

    /// <summary>
    /// Gets a value indicating whether or not the editing glyph is visible in the row header of the cell being edited.
    /// </summary>
    [Category("MySQL Custom"), Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    public new bool ShowEditingIcon
    {
      get
      {
        return base.ShowEditingIcon;
      }

      protected set
      {
        base.ShowEditingIcon = value;
      }
    }

    /// <summary>
    /// Gets a value indicating whether row headers will display error glyphs for each row that contains a data entry error.
    /// </summary>
    [Category("MySQL Custom"), Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    public new bool ShowRowErrors
    {
      get
      {
        return base.ShowRowErrors;
      }

      protected set
      {
        base.ShowRowErrors = value;
      }
    }

    #endregion Properties

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
    /// Recalculates the given <see cref="DataGridViewColumn"/> width comparing it to the <see cref="ColumnsMaximumWidth"/> value.
    /// </summary>
    /// <param name="column">A <see cref="DataGridViewColumn"/>.</param>
    protected void PerformColumnWidthRecalculation(DataGridViewColumn column)
    {
      bool wrapText = false;
      int cappedWidth = column.Width;
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
  }
}