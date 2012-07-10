using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Windows.Forms;

namespace MySQL.ForExcel
{
  class PreviewDataGridView : DataGridView
  {
    private bool skipWidthRecalculation = false;

    [Category("Appearance"), DefaultValue(0), Description("Gets or sets the maximum column width, in pixels, of all columns in the grid.")]
    public int ColumnsMaximumWidth { get; set; }

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
    }

    private void resetColumnWidth(DataGridViewColumn col)
    {
      if (ColumnsMaximumWidth > 0 && col.Width > ColumnsMaximumWidth && ColumnsMaximumWidth > col.MinimumWidth)
      {
        col.AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
        skipWidthRecalculation = true;
        col.Width = ColumnsMaximumWidth;
        skipWidthRecalculation = false;
      }
    }

    protected override void OnColumnWidthChanged(DataGridViewColumnEventArgs e)
    {
      base.OnColumnWidthChanged(e);
      if (!skipWidthRecalculation)
        resetColumnWidth(e.Column);
    }

  }
}
