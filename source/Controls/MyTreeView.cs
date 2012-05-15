using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using System.Drawing;

namespace TreeViewTest
{
  public class MyTreeView : TreeView
  {
    private bool heightsSet;
    private Brush grayBrush;
    private Brush blackBrush;

    private const int TVM_SETITEMHEIGHT = 0x111B;
    private const int TVM_SETITEM = 0x113F;
    private const int TVIF_INTEGRAL = 0x80;
    private const int TVIF_HANDLE = 0x10;
    public const int WM_PRINTCLIENT = 0x0318;
    public const int PRF_CLIENT = 0x00000004;
    public const int TVS_EX_DOUBLEBUFFER = 0x0004;
    public const int TVM_SETEXTENDEDSTYLE = 0x112C;

    public MyTreeView()
    {
      DrawMode = TreeViewDrawMode.OwnerDrawAll;
      grayBrush = new SolidBrush(Color.Gray);
      blackBrush = new SolidBrush(Color.Black);
      DoubleBuffered = true;

      // Enable default double buffering processing (DoubleBuffered returns true)
      //SetStyle(ControlStyles.OptimizedDoubleBuffer, true); // | ControlStyles.AllPaintingInWmPaint, true);
      // Disable default CommCtrl painting on non-Vista systems
      //if (Environment.OSVersion.Version.Major < 6)
      //  SetStyle(ControlStyles.UserPaint, true);
    }

    public Font DescriptionFont { get; set; }
    public Color DescriptionColor { get; set; }
    public Image CollapsedIcon { get; set; }
    public Image ExpandedIcon { get; set; }
    public ImageList NodeImages { get; set; }

    //protected override void OnPaint(PaintEventArgs e)
    //{
    //  if (GetStyle(ControlStyles.UserPaint))
    //  {
    //    Message m = new Message();
    //    m.HWnd = Handle;
    //    m.Msg = WM_PRINTCLIENT;
    //    m.WParam = e.Graphics.GetHdc();
    //    m.LParam = (IntPtr)PRF_CLIENT;
    //    DefWndProc(ref m);
    //    e.Graphics.ReleaseHdc(m.WParam);
    //  }
    //  base.OnPaint(e);
    //}

    protected override void OnMouseClick(MouseEventArgs e)
    {
      TreeNode node = GetNodeAt(e.Location);
      if (node != null)
        SelectedNode = node;
      base.OnMouseClick(e);
    }

    private void UpdateExtendedStyles()
    {
      int Style = 0;

      if (DoubleBuffered)
        Style |= TVS_EX_DOUBLEBUFFER;

      if (Style != 0)
        SendMessage(Handle, TVM_SETEXTENDEDSTYLE, (IntPtr)TVS_EX_DOUBLEBUFFER, (IntPtr)Style);
    }

    protected override void OnHandleCreated(EventArgs e)
    {
      base.OnHandleCreated(e);
      UpdateExtendedStyles();
    }

    protected override void OnDrawNode(DrawTreeNodeEventArgs e)
    {
      if (e.Node.Level == 0)
        DrawTopLevelNode(e);
      else
        DrawChildNode(e);
    }

    private void DrawTopLevelNode(DrawTreeNodeEventArgs e)
    {
      Graphics g = e.Graphics;
      Brush brush = new SolidBrush(e.Node.BackColor);
      g.FillRectangle(brush, e.Bounds);

      Point pt = e.Bounds.Location;

      // draw icon centered
      Image i = e.Node.IsExpanded ? ExpandedIcon : CollapsedIcon;
      pt.Y += (e.Bounds.Height - i.Height) / 2;
      e.Graphics.DrawImageUnscaled(i, pt.X, pt.Y, i.Width, i.Height);

      brush = new SolidBrush(e.Node.ForeColor);
      Font f = e.Node.NodeFont != null ? e.Node.NodeFont : Font;
      SizeF size = g.MeasureString(e.Node.Text, f);
      pt.X += (5 + i.Width);
      pt.Y = e.Bounds.Top + ((e.Bounds.Height - (int)size.Height) / 2);
      g.DrawString(e.Node.Text, Font, brush, pt.X, pt.Y);
    }

    private void DrawChildNode(DrawTreeNodeEventArgs e)
    {
      Point pt = e.Bounds.Location;

      // paint background
      Brush brush = new SolidBrush(e.Node.IsSelected ? SystemColors.MenuHighlight : SystemColors.Window);
      e.Graphics.FillRectangle(brush, e.Bounds);

      if (NodeImages != null)
      {
        Image i = null;
        if (NodeImages.Images.Count > 0 && e.Node.ImageIndex >= 0 && e.Node.ImageIndex < NodeImages.Images.Count)
          i = NodeImages.Images[e.Node.ImageIndex];
        if (i != null)
        {
          pt.X += 5;
          int y = pt.Y + ((e.Bounds.Height - i.Height) / 2);
          e.Graphics.DrawImage(i, pt.X, y, i.Width, i.Height);
          pt.X += i.Width + 5;
        }
      }

      string[] parts = e.Node.Text.Split('|');

      // draw the title if we have one
      brush = new SolidBrush(ForeColor);
      if (parts != null && parts.Length >= 1)
      {
        e.Graphics.DrawString(parts[0], Font, brush, pt.X, pt.Y);
        SizeF stringSize = e.Graphics.MeasureString(parts[0], Font);
        pt.Y += (int)(stringSize.Height + 3);
      }

      // draw the description if there is one
      brush = new SolidBrush(DescriptionColor);
      if (parts != null && parts.Length >= 2)
        e.Graphics.DrawString(parts[1], DescriptionFont, brush, pt.X, pt.Y);
    }

    public TreeNode AddNode(TreeNode parent, string text)
    {
      TreeNode node = parent.Nodes.Add(text);
      SetNodeHeight(node, parent != null ? 2 : 1);
      return node;
    }

    private void SetNodeHeight(TreeNode node, int integral)
    {
      TVITEMEX tex = new TVITEMEX();
      tex.mask = TVIF_HANDLE | TVIF_INTEGRAL;
      tex.hItem = node.Handle;
      tex.iIntegral = integral;

        IntPtr ptr = Marshal.AllocHGlobal(Marshal.SizeOf(tex));
        Marshal.StructureToPtr(tex, ptr, false);

        SendMessage(this.Handle, TVM_SETITEM, IntPtr.Zero, ptr);
        Marshal.FreeHGlobal(ptr);
    }

    [DllImport("user32.dll")]
    static extern IntPtr SendMessage(IntPtr hWnd, uint Msg, IntPtr wParam, IntPtr lParam);
  }

  // struct used to set node properties
  struct TVITEM
  {
    public int mask;
    public IntPtr hItem;
    public int state;
    public int stateMask;
    [MarshalAs(UnmanagedType.LPTStr)]
    public String lpszText;
    public int cchTextMax;
    public int iImage;
    public int iSelectedImage;
    public int cChildren;
    public IntPtr lParam;

  }

  struct TVITEMEX
  {
    public int mask;
    public IntPtr hItem;
    public int state;
    public int stateMask;
    [MarshalAs(UnmanagedType.LPTStr)]
    public String lpszText;
    public int cchTextMax;
    public int iImage;
    public int iSelectedImage;
    public int cChildren;
    public IntPtr lParam;
    public int iIntegral;
  }
}
