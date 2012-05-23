using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using MySQL.ForExcel.Properties;

namespace MySQL.ForExcel.Controls
{
  public partial class SearchEdit : UserControl
  {
    private bool isEmpty;
    private int width;

    public SearchEdit()
    {
      InitializeComponent();
      DoubleBuffered = true;
      width = Resources.ExcelAddinFilter.Width;
      innerText.Text = NoTextLabel;
      innerText_Leave(null, EventArgs.Empty);
    }

    public string Text
    {
      get { return innerText.Text.Trim(); }
      set { innerText.Text = value; }
    }

    public string NoTextLabel { get; set; }

    protected override void OnPaint(PaintEventArgs e)
    {
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
