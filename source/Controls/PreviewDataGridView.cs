// 
// Copyright (c) 2012, Oracle and/or its affiliates. All rights reserved.
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
