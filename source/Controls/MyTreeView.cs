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
using System.Windows.Forms;
using System.Runtime.InteropServices;
using System.Drawing;

namespace MySQL.ForExcel
{
  public class MyTreeView : TreeView
  {
    private int nodeHeightMultiple = 2;
    private const int TVM_SETITEMHEIGHT = 0x111B;
    private const int TVM_SETITEM = 0x113F;
    private const int TVIF_INTEGRAL = 0x80;
    private const int TVIF_HANDLE = 0x10;
    public const int WM_PRINTCLIENT = 0x0318;
    public const int PRF_CLIENT = 0x00000004;
    public const int TVS_EX_DOUBLEBUFFER = 0x0004;
    public const int TVM_SETEXTENDEDSTYLE = 0x112C;

    [DllImport("user32.dll")]
    static public extern bool ShowScrollBar(System.IntPtr hWnd, int wBar, bool bShow);

    public MyTreeView()
    {
      DrawMode = TreeViewDrawMode.OwnerDrawAll;
      DoubleBuffered = true;

      ImageHorizontalPixelsOffset = 5;
      ImageToTextHorizontalPixelsOffset = 5;
      TitleColorOpacity = 0.8;
      DescriptionColorOpacity = 0.6;
      TitleTextVerticalPixelsOffset = 0;
      DescriptionTextVerticalPixelsOffset = 0;
      this.Scrollable = true;
      this.ShowNodeToolTips = true;      
    }

    public double TitleColorOpacity { get; set; }
    public Font DescriptionFont { get; set; }
    public Color DescriptionColor { get; set; }
    public double DescriptionColorOpacity { get; set; }
    public Image CollapsedIcon { get; set; }
    public Image ExpandedIcon { get; set; }
    public ImageList NodeImages { get; set; }
    public int ImageHorizontalPixelsOffset { get; set; }
    public int ImageToTextHorizontalPixelsOffset { get; set; }
    public int TitleTextVerticalPixelsOffset { get; set; }
    public int DescriptionTextVerticalPixelsOffset { get; set; }
    private ToolTip toolTipLowLevelNode { get; set; }

    public int NodeHeightMultiple 
    {
      get { return nodeHeightMultiple; }
      set 
      {
        if (value < 1)
          throw new ArgumentOutOfRangeException("NodeHeightMultiple", "Value must be at least 1");
        nodeHeightMultiple = value;
      }
    }

    protected override void OnMouseClick(MouseEventArgs e)
    {
      TreeNode node = GetNodeAt(e.Location);
      if (node != null)
        SelectedNode = node;
      base.OnMouseClick(e);
    }

    protected override void OnFontChanged(EventArgs e)
    {
      base.OnFontChanged(e);
      if (DescriptionFont != null)
      {
        if (DescriptionFont.Name != Font.Name)
          DescriptionFont = new Font(Font.FontFamily, DescriptionFont.Size, DescriptionFont.Style);
      }
      else
        DescriptionFont = new Font(Font.FontFamily, Font.Size - 1, FontStyle.Regular);
      MarkTruncate(Nodes);
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
      ShowScrollBar(this.Handle, 0, false);
    }

    private void DrawTopLevelNode(DrawTreeNodeEventArgs e)
    {
      Graphics g = e.Graphics;
      SolidBrush nodeBackbrush = new SolidBrush(e.Node.BackColor);
      g.FillRectangle(nodeBackbrush, e.Bounds);

      Point pt = e.Bounds.Location;

      // draw icon centered
      Image i = e.Node.IsExpanded ? ExpandedIcon : CollapsedIcon;
      pt.Y += (e.Bounds.Height - i.Height) / 2;
      e.Graphics.DrawImageUnscaled(i, pt.X, pt.Y, i.Width, i.Height);

      SolidBrush textBrush = new SolidBrush(Color.FromArgb(Convert.ToInt32(TitleColorOpacity * 255), e.Node.ForeColor));
      Font f = e.Node.NodeFont != null ? e.Node.NodeFont : Font;
      if (!f.Bold)      
        f = new Font(f.FontFamily, f.Size, FontStyle.Bold);

      SizeF size = g.MeasureString(e.Node.Text, f);
      pt.X += (ImageToTextHorizontalPixelsOffset + i.Width);
      pt.Y = e.Bounds.Top + ((e.Bounds.Height - (int)size.Height) / 2);
      g.DrawString(e.Node.Text, f, textBrush, pt.X, pt.Y);

      nodeBackbrush.Dispose();
      textBrush.Dispose();
    }

    protected override void OnResize(EventArgs e)
    {
      base.OnResize(e);
      MarkTruncate(Nodes);
    }
    
    private void DrawChildNode(DrawTreeNodeEventArgs e)
    {
      if (e.Node is MyTreeNode)
      {
        MyTreeNode myNode = e.Node as MyTreeNode;

        bool disabled = !myNode.Enable;
        Point pt = e.Bounds.Location;
        SizeF titleStringSize = e.Graphics.MeasureString(myNode.Title, Font);
        SizeF descriptionStringSize = e.Graphics.MeasureString(myNode.Subtitle, DescriptionFont);
        Image img = (NodeImages != null && NodeImages.Images.Count > 0 && e.Node.ImageIndex >= 0 && e.Node.ImageIndex < NodeImages.Images.Count ? NodeImages.Images[e.Node.ImageIndex] : null);
        int textInitialY = (myNode.Subtitle == null ? ((e.Bounds.Height - Convert.ToInt32(titleStringSize.Height) + Convert.ToInt32(descriptionStringSize.Height)) / 2) : 0);
        myNode.ToolTipText = String.Empty;

        // Paint background
        SolidBrush bkBrush = new SolidBrush(myNode.IsSelected ? SystemColors.MenuHighlight : SystemColors.Window);
        e.Graphics.FillRectangle(bkBrush, e.Bounds);

        // Paint node Image
        if (img != null)
        {
          pt.X += ImageHorizontalPixelsOffset;
          int y = pt.Y + ((e.Bounds.Height - img.Height) / 2);
          e.Graphics.DrawImage(img, pt.X, y, img.Width, img.Height);
          pt.X += img.Width;
        }

        pt.X += ImageToTextHorizontalPixelsOffset;
        pt.Y += textInitialY + TitleTextVerticalPixelsOffset;

        // Draw the title if we have one
        string truncatedText = null;
        SolidBrush titleBrush = null;
        if (disabled)
          titleBrush = new SolidBrush(Color.FromArgb(80, 0, 0, 0));
        else
          titleBrush = new SolidBrush(Color.FromArgb(Convert.ToInt32(TitleColorOpacity * 255), ForeColor));

        // Draw the title
        if (myNode.Title != null)
        {
          SizeF stringSize = e.Graphics.MeasureString(myNode.Title, Font);
          truncatedText = myNode.GetTruncatedTitle(e.Node.TreeView.ClientRectangle.Width - pt.X, e.Graphics, Font);
          e.Graphics.DrawString(truncatedText, Font, titleBrush, pt.X, pt.Y);
          pt.Y += (int)(stringSize.Height) + DescriptionTextVerticalPixelsOffset;
          if (truncatedText != myNode.Title)
            e.Node.ToolTipText = myNode.Title;
        }

        // Draw the description if there is one
        SolidBrush descBrush = null;
        if (disabled)
          descBrush = new SolidBrush(Color.FromArgb(80, 0, 0, 0));
        else
          descBrush = new SolidBrush(Color.FromArgb(Convert.ToInt32(DescriptionColorOpacity * 255), DescriptionColor));
        if (myNode.Subtitle != null)
        {
          truncatedText = myNode.GetTruncatedSubtitle(e.Node.TreeView.ClientRectangle.Width - pt.X, e.Graphics, DescriptionFont);
          e.Graphics.DrawString(truncatedText, DescriptionFont, descBrush, pt.X, pt.Y);
          if (truncatedText != myNode.Subtitle)
            e.Node.ToolTipText += (string.IsNullOrWhiteSpace(e.Node.ToolTipText) ? string.Empty : Environment.NewLine) + myNode.Subtitle;
        }

        bkBrush.Dispose();
        titleBrush.Dispose();
        descBrush.Dispose();
      }
      else
      {
        if (!string.IsNullOrWhiteSpace(e.Node.Text))
        {
          //e.Graphics.DrawString(e.Node.Text, Font,)
        }
      }
    }
    
    public MyTreeNode AddNode(TreeNode parent, string title, string subtitle = null)
    {
      MyTreeNode node = null;
      if (parent == null)
      {
        node = Nodes[Nodes.Add(new MyTreeNode(title, subtitle))] as MyTreeNode;
        node.ForeColor = SystemColors.ControlText;
        node.BackColor = SystemColors.ControlLight;
      }
      else
      {
        node = parent.Nodes[parent.Nodes.Add(new MyTreeNode(title, subtitle))] as MyTreeNode;
      }
      SetNodeHeight(node, parent != null ? nodeHeightMultiple : (nodeHeightMultiple > 1 ? nodeHeightMultiple - 1 : 1));
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

    private void MarkTruncate(TreeNodeCollection nodes)
    {
      foreach (TreeNode child in nodes)
      {
        MyTreeNode myChild = child as MyTreeNode;
        if (myChild != null)
        {
          myChild.UpdateTruncatedTitle = true;
          myChild.UpdateTruncatedSubtitle = true;
        }
        MarkTruncate(child.Nodes);
      }
    }

    [DllImport("user32.dll")]
    static extern IntPtr SendMessage(IntPtr hWnd, uint Msg, IntPtr wParam, IntPtr lParam);
  }

  public class MyTreeNode : TreeNode
  {
    private string _title;
    private string _subtitle;
    private string _truncatedTitle;
    private string _truncatedSubtitle;
    public bool Enable { get; set; }
    public string Title
    {
      get { return _title; }
      set
      {
        _title = value;
        UpdateTruncatedTitle = true;
      }
    }
    public string Subtitle
    {
      get { return _subtitle; }
      set
      {
        _subtitle = value;
        UpdateTruncatedSubtitle = true;
      }
    }
    public bool UpdateTruncatedTitle { get; set; }
    public bool UpdateTruncatedSubtitle { get; set; }

    public MyTreeNode():base() { }
    public MyTreeNode(string title) : this(title, null) { }
    public MyTreeNode(string title, string subtitle)
      : base(title)
    {
      Title = title;
      Subtitle = subtitle;
      Enable = true;
    }

    public string GetTruncatedTitle(float maxWidth, Graphics graphics, Font font)
    {
      if (UpdateTruncatedTitle)
      {
        _truncatedTitle = MiscUtilities.TruncateString(Title, maxWidth, graphics, font);
        UpdateTruncatedTitle = false;
      }
      return _truncatedTitle;
    }

    public string GetTruncatedSubtitle(float maxWidth, Graphics graphics, Font font)
    {
      if (UpdateTruncatedSubtitle)
      {
        _truncatedSubtitle = MiscUtilities.TruncateString(Subtitle, maxWidth, graphics, font);
        UpdateTruncatedSubtitle = false;
      }
      return _truncatedSubtitle;
    }
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