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
  using System.ComponentModel;
  using System.Windows.Forms;

  /// <summary>
  /// Displays data in a read-only grid for preview purposes only.
  /// </summary>
  internal class PreviewDataGridView : DataGridView
  {
    /// <summary>
    /// Flag indicating if recalculation of column width is not necessary so it must be skipped.
    /// </summary>
    private bool _skipWidthRecalculation = false;

    /// <summary>
    /// Initializes a new instance of the <see cref="PreviewDataGridView"/> class.
    /// </summary>
    public PreviewDataGridView()
    {
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
      ColumnsMaximumWidth = 0;
      DisableColumnsSelection = false;
    }

    #region Properties

    /// <summary>
    /// Gets a value indicating whether the option to add rows is displayed to the user.
    /// </summary>
    [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
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
    [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
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
    [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
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
    [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
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
    [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
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
    [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
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
    [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
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
    [Category("Appearance"), DefaultValue(0), Description("Gets or sets the maximum column width, in pixels, of all columns in the grid.")]
    public int ColumnsMaximumWidth { get; set; }

    /// <summary>
    /// Gets or sets a value indicating if column selection is disabled for users.
    /// </summary>
    [Category("Behavior"), DefaultValue(false), Description("Gets or sets a value indicating if column selection is disabled for users.")]
    public bool DisableColumnsSelection { get; set; }

    /// <summary>
    /// Gets a value indicating whether the user can edit the cells of the DataGridView control.
    /// </summary>
    [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
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
    [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
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
    [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
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
    [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
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
    [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
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
    [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
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
    /// Raises the <see cref="CellMouseDown"/> event.
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
    /// Raises the <see cref="CellValueChanged"/> event.
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
        if (this.Columns[e.ColumnIndex].AutoSizeMode != DataGridViewAutoSizeColumnMode.AllCells)
        {
          this.Columns[e.ColumnIndex].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
        }
      }
    }

    /// <summary>
    /// Raises the <see cref="ColumnWidthChanged"/> event.
    /// </summary>
    /// <param name="e">A DataGridViewColumnEventArgs that contains the event data.</param>
    protected override void OnColumnWidthChanged(DataGridViewColumnEventArgs e)
    {
      if (_skipWidthRecalculation)
      {
        return;
      }

      base.OnColumnWidthChanged(e);
      if (ColumnsMaximumWidth > 0 && e.Column.Width > ColumnsMaximumWidth && ColumnsMaximumWidth > e.Column.MinimumWidth)
      {
        _skipWidthRecalculation = true;
        e.Column.AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
        e.Column.HeaderCell.Style.WrapMode = DataGridViewTriState.False;
        e.Column.Width = ColumnsMaximumWidth;
        _skipWidthRecalculation = false;
      }
      else
      {
        e.Column.HeaderCell.Style.WrapMode = DataGridViewTriState.True;
      }
    }
  }
}