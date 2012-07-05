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
  public partial class MultiHeaderDataGridView : UserControl
  {
    private int columnHeadersHeight = 46;

    #region Properties

    [Browsable(false)]
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    public List<MultiHeaderColumn> MultiHeaderColumnList { get; set; }
    
    public object DataSource
    {
      get { return (grdView != null ? grdView.DataSource : null); }
      set { if (grdView != null) grdView.DataSource = value; }
    }

    [Browsable(false)]
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    public DataGridViewColumnCollection Columns
    {
      get { return (grdView != null ? grdView.Columns : null); }
    }

    [Browsable(false)]
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    public DataGridViewRowCollection Rows
    {
      get { return (grdView != null ? grdView.Rows : null); }
    }

    public DataGridViewSelectionMode SelectionMode
    {
      get { return (grdView != null ? grdView.SelectionMode : DataGridViewSelectionMode.RowHeaderSelect); }
      set
      {
        if (grdView != null)
        {
          if (value == DataGridViewSelectionMode.FullColumnSelect)
          {
            grdView.SelectionMode = DataGridViewSelectionMode.RowHeaderSelect;
            foreach (DataGridViewColumn gridCol in grdView.Columns)
            {
              gridCol.SortMode = DataGridViewColumnSortMode.NotSortable;
            }
          }
          grdView.SelectionMode = value;
        }
      }
    }

    [Browsable(false)]
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    public DataGridViewSelectedColumnCollection SelectedColumns
    {
      get { return (grdView != null ? grdView.SelectedColumns : null); }
    }

    [Browsable(false)]
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    public DataGridViewSelectedRowCollection SelectedRows
    {
      get { return (grdView != null ? grdView.SelectedRows : null); }
    }

    [Browsable(false)]
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    public DataGridViewSelectedCellCollection SelectedCells
    {
      get { return (grdView != null ? grdView.SelectedCells : null); }
    }

    [Browsable(false)]
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    public DataGridViewCell CurrentCell
    {
      get { return (grdView != null ? grdView.CurrentCell : null); }
      set { if (grdView != null) grdView.CurrentCell = value; }
    }

    [Browsable(false)]
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    public int FirstDisplayedScrollingRowIndex
    {
      get { return (grdView != null ? grdView.FirstDisplayedScrollingRowIndex : 0); }
      set { if (grdView != null) grdView.FirstDisplayedScrollingRowIndex = value; }
    }

    public bool ReadOnly
    {
      get { return (grdView != null ? grdView.ReadOnly : false); }
      set { if (grdView != null) grdView.ReadOnly = value; }
    }

    public bool MultiSelect
    {
      get { return (grdView != null ? grdView.MultiSelect : false); }
      set { if (grdView != null) grdView.MultiSelect = value; }
    }

    public bool AllowUserToAddRows
    {
      get { return (grdView != null ? grdView.AllowUserToAddRows : false); }
      set { if (grdView != null) grdView.AllowUserToAddRows = value; }
    }

    public bool AllowUserToDeleteRows
    {
      get { return (grdView != null ? grdView.AllowUserToDeleteRows : false); }
      set { if (grdView != null) grdView.AllowUserToDeleteRows = value; }
    }

    public bool AllowUserToResizeColumns
    {
      get { return (grdView != null ? grdView.AllowUserToResizeColumns : false); }
      set { if (grdView != null) grdView.AllowUserToResizeColumns = value; }
    }

    public bool AllowUserToResizeRows
    {
      get { return (grdView != null ? grdView.AllowUserToResizeRows : false); }
      set { if (grdView != null) grdView.AllowUserToResizeRows = value; }
    }

    public DataGridViewColumnHeadersHeightSizeMode ColumnHeadersHeightSizeMode
    {
      get { return (grdView != null ? grdView.ColumnHeadersHeightSizeMode : DataGridViewColumnHeadersHeightSizeMode.DisableResizing); }
      set { if (grdView != null) grdView.ColumnHeadersHeightSizeMode = value; }
    }

    public DataGridViewAutoSizeColumnsMode AutoSizeColumnsMode
    {
      get { return (grdView != null ? grdView.AutoSizeColumnsMode : DataGridViewAutoSizeColumnsMode.None); }
      set { if (grdView != null) grdView.AutoSizeColumnsMode = value; }
    }

    public bool RowHeadersVisible
    {
      get { return (grdView != null ? grdView.RowHeadersVisible : false); }
      set { if (grdView != null) grdView.RowHeadersVisible = value; }
    }

    public bool GridAllowsDrop
    {
      get { return (grdView != null ? grdView.AllowDrop : false); }
      set { if (grdView != null) grdView.AllowDrop = value; }
    }

    #endregion Properties
    #region Events

    public event EventHandler SelectionChanged;
    public event DragEventHandler GridDragOver;
    public event DragEventHandler GridDragDrop;
    public event DragEventHandler GridDragEnter;
    public event EventHandler GridDragLeave;
    public event GiveFeedbackEventHandler GridGiveFeedback;
    public event QueryContinueDragEventHandler GridQueryContinueDrag;
    public event MouseEventHandler GridMouseDown;
    public event MouseEventHandler GridMouseUp;
    public event MouseEventHandler GridMouseMove;

    private void grdView_SelectionChanged(object sender, EventArgs e)
    {
      if (SelectionChanged != null)
        SelectionChanged(sender, e);
    }

    private void grdView_DragOver(object sender, DragEventArgs e)
    {
      if (GridDragOver != null)
        GridDragOver(sender, e);
    }

    private void grdView_DragDrop(object sender, DragEventArgs e)
    {
      if (GridDragDrop != null)
        GridDragDrop(sender, e);
    }

    private void grdView_DragEnter(object sender, DragEventArgs e)
    {
      if (GridDragEnter != null)
        GridDragEnter(sender, e);
    }

    private void grdView_DragLeave(object sender, EventArgs e)
    {
      if (GridDragLeave != null)
        GridDragLeave(sender, e);
    }

    private void grdView_GiveFeedback(object sender, GiveFeedbackEventArgs e)
    {
      if (GridGiveFeedback != null)
        GridGiveFeedback(sender, e);
    }

    private void grdView_QueryContinueDrag(object sender, QueryContinueDragEventArgs e)
    {
      if (GridQueryContinueDrag != null)
        GridQueryContinueDrag(sender, e);
    }

    private void grdView_MouseDown(object sender, MouseEventArgs e)
    {
      if (GridMouseDown != null)
        GridMouseDown(sender, e);
    }

    private void grdView_MouseUp(object sender, MouseEventArgs e)
    {
      if (GridMouseUp != null)
        GridMouseUp(sender, e);
    }

    private void grdView_MouseMove(object sender, MouseEventArgs e)
    {
      if (GridMouseMove != null)
        GridMouseMove(sender, e);
    }

    #endregion Events

    public MultiHeaderDataGridView()
    {
      InitializeComponent();

      DoubleBuffered = true;
      Utilities.SetDoubleBuffered(grdView);

      grdView.AllowDrop = true;
      grdView.SelectionChanged += new EventHandler(grdView_SelectionChanged);
      grdView.DragOver += new DragEventHandler(grdView_DragOver);
      grdView.DragDrop += new DragEventHandler(grdView_DragDrop);
      grdView.DragEnter += new DragEventHandler(grdView_DragEnter);
      grdView.DragLeave += new EventHandler(grdView_DragLeave);
      grdView.GiveFeedback += new GiveFeedbackEventHandler(grdView_GiveFeedback);
      grdView.QueryContinueDrag += new QueryContinueDragEventHandler(grdView_QueryContinueDrag);
      grdView.MouseDown += new MouseEventHandler(grdView_MouseDown);
      grdView.MouseUp += new MouseEventHandler(grdView_MouseUp);
      grdView.MouseMove += new MouseEventHandler(grdView_MouseMove);

      MultiHeaderColumnList = new List<MultiHeaderColumn>();
    }

    public void ClearSelection()
    {
      grdView.ClearSelection();
    }

    public DataGridView.HitTestInfo HitTest(int x, int y)
    {
      return grdView.HitTest(x, y);
    }

    private void grdView_Paint(object sender, PaintEventArgs e)
    {
      grdView.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.EnableResizing;
      grdView.ColumnHeadersHeight = columnHeadersHeight;
      grdView.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.BottomCenter;
      foreach (MultiHeaderColumn mHeader in MultiHeaderColumnList)
      {
        int lastDivWidth = grdView.Columns[mHeader.LastColumnIndex].DividerWidth;
        int multiWidth = 0;
        for (int idx = mHeader.FirstColumnIndex; idx <= mHeader.LastColumnIndex; idx++)
        {
          multiWidth += grdView.Columns[idx].Width;
        }
        var firstRec = grdView.GetCellDisplayRectangle(mHeader.FirstColumnIndex, -1, true);
        if (firstRec.IsEmpty)
          continue;
        Rectangle headerRect = new Rectangle(firstRec.Left + 1, firstRec.Y, multiWidth - 2 - lastDivWidth, Convert.ToInt32(grdView.ColumnHeadersHeight / 2) - 2);
        Color backColor = (mHeader.BackgroundColor.IsEmpty ? grdView.ColumnHeadersDefaultCellStyle.BackColor : mHeader.BackgroundColor);
        e.Graphics.FillRectangle(new SolidBrush(backColor), headerRect);
        StringFormat format = new StringFormat();
        format.Alignment = StringAlignment.Center;
        format.LineAlignment = StringAlignment.Center;
        e.Graphics.DrawString(mHeader.HeaderText, grdView.ColumnHeadersDefaultCellStyle.Font, new SolidBrush(grdView.ColumnHeadersDefaultCellStyle.ForeColor), headerRect, format);
      }
    }

    private void grdView_Scroll(object sender, ScrollEventArgs e)
    {
      if (e.ScrollOrientation == ScrollOrientation.HorizontalScroll)
      {
        Rectangle rtHeader = grdView.DisplayRectangle;
        rtHeader.Height = grdView.ColumnHeadersHeight / 2;
        grdView.Invalidate(rtHeader);
      }
    }

    private void grdView_ColumnWidthChanged(object sender, DataGridViewColumnEventArgs e)
    {
      Rectangle rtHeader = grdView.DisplayRectangle;
      rtHeader.Height = grdView.ColumnHeadersHeight / 2;
      grdView.Invalidate(rtHeader);
    }

    private void grdView_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
    {
      grdView.ClearSelection();
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
