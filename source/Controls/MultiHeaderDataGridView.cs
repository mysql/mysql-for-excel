using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace MySQL.ForExcel
{
  class MultiHeaderDataGridView : DataGridView
  {
    private const int columnHeadersHeight = 46;

    [Browsable(false)]
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    public List<MultiHeaderColumn> MultiHeaderColumnList { get; set; }

    public MultiHeaderDataGridView()
    {
      DoubleBuffered = true;

      AllowDrop = true;
      MultiHeaderColumnList = new List<MultiHeaderColumn>();
    }

    protected override void OnPaint(PaintEventArgs e)
    {
      base.OnPaint(e);
      ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.EnableResizing;
      ColumnHeadersHeight = columnHeadersHeight;
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
          continue;
        Rectangle headerRect = new Rectangle(firstRec.Left + 1, firstRec.Y, multiWidth - 2 - lastDivWidth, Convert.ToInt32(ColumnHeadersHeight / 2) - 2);
        backColor = (mHeader.BackgroundColor.IsEmpty ? ColumnHeadersDefaultCellStyle.BackColor : mHeader.BackgroundColor);
        backgroundBrush.Color = backColor;
        e.Graphics.FillRectangle(backgroundBrush, headerRect);
        e.Graphics.DrawString(mHeader.HeaderText, ColumnHeadersDefaultCellStyle.Font, foregroundBrush, headerRect, format);
      }
      foregroundBrush.Dispose();
      backgroundBrush.Dispose();
    }

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

    protected override void OnColumnWidthChanged(DataGridViewColumnEventArgs e)
    {
      base.OnColumnWidthChanged(e);
      Rectangle rtHeader = DisplayRectangle;
      rtHeader.Height = ColumnHeadersHeight / 2;
      Invalidate(rtHeader);
    }

    protected override void OnDataBindingComplete(DataGridViewBindingCompleteEventArgs e)
    {
      base.OnDataBindingComplete(e);
      ClearSelection();
    }

  }

  public class MultiHeaderColumn
  {
    public string HeaderText { get; set; }
    public int FirstColumnIndex { get; private set; }
    public int LastColumnIndex { get; private set; }
    public Color BackgroundColor { get; set; }

    public MultiHeaderColumn(string hdrText, int firstIdx, int lastIdx)
    {
      HeaderText = hdrText;
      FirstColumnIndex = firstIdx;
      LastColumnIndex = lastIdx;
      BackgroundColor = SystemColors.Control;
    }
  }
}
