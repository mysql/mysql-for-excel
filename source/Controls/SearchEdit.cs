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
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using MySQL.ForExcel.Properties;

namespace MySQL.ForExcel
{
  public partial class SearchEdit : UserControl
  {
    private bool isEmpty;
    private bool initialPaint;
    private int width;

    public SearchEdit()
    {
      InitializeComponent();
      DoubleBuffered = true;
      width = Resources.ExcelAddinFilter.Width;
      innerText.Text = String.Empty;
      initialPaint = true;
    }

    public override string Text
    {
      get { return innerText.Text.Trim(); }
      set { innerText.Text = value; }
    }

    public string NoTextLabel { get; set; }

    protected override void OnPaint(PaintEventArgs e)
    {
      if (initialPaint)
      {
        innerText_Leave(innerText, EventArgs.Empty);
        initialPaint = false;
      }
      base.OnPaint(e);
      Image i = Resources.ExcelAddinFilter;
      int space = width * 3 / 2;
      e.Graphics.DrawImage(i, (space-width)/2, (Height - i.Height)/2);
    }

    private void innerText_Leave(object sender, EventArgs e)
    {
      if (innerText.Text.Trim().Length == 0)
      {
        innerText.Text = NoTextLabel; 
        innerText.ForeColor = Color.Silver;
        isEmpty = true;
      }
    }

    private void innerText_Enter(object sender, EventArgs e)
    {
      if (isEmpty)
      {
        innerText.Text = String.Empty;
        isEmpty = false;
        innerText.ForeColor = SystemColors.WindowText;
      }
    }

    private void SearchEdit_Resize(object sender, EventArgs e)
    {
      innerText.SetBounds(width*3/2 , (Height - innerText.Height)/2, Size.Width - width, innerText.Height);
    }

    private void innerText_KeyDown(object sender, KeyEventArgs e)
    {
      OnKeyDown(e);
    }

  }
}
