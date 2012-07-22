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

    private void DrawChildNode(DrawTreeNodeEventArgs e)
    {
      bool disabled = e.Node.Name.StartsWith("DISABLED_");
      Point pt = e.Bounds.Location;
      string[] parts = e.Node.Text.Split('|');
      SizeF titleStringSize = (parts != null && parts.Length > 0 ? e.Graphics.MeasureString(parts[0], Font) : SizeF.Empty);
      SizeF descriptionStringSize = (parts != null && parts.Length > 1 ? e.Graphics.MeasureString(parts[1], DescriptionFont) : SizeF.Empty);
      Image img = (NodeImages != null && NodeImages.Images.Count > 0 && e.Node.ImageIndex >= 0 && e.Node.ImageIndex < NodeImages.Images.Count ? NodeImages.Images[e.Node.ImageIndex] : null);
      int textInitialY = (parts.Length == 1 ? ((e.Bounds.Height - Convert.ToInt32(titleStringSize.Height) + Convert.ToInt32(descriptionStringSize.Height)) / 2) : 0);
      e.Node.ToolTipText = String.Empty;

      // Paint background
      SolidBrush bkBrush = new SolidBrush(e.Node.IsSelected ? SystemColors.MenuHighlight : SystemColors.Window);
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
      SolidBrush titleBrush  = null;
      if (disabled)
        titleBrush = new SolidBrush(Color.FromArgb(80, 0, 0, 0));
      else
        titleBrush = new SolidBrush(Color.FromArgb(Convert.ToInt32(TitleColorOpacity * 255), ForeColor));
      if (parts != null && parts.Length >= 1)
      {  
        SizeF stringSize = e.Graphics.MeasureString(parts[0], Font);
        truncatedText = MiscUtilities.TruncateString(parts[0], e.Node.TreeView.ClientRectangle.Width - pt.X, e.Graphics, Font);
        e.Graphics.DrawString(truncatedText, Font, titleBrush, pt.X, pt.Y);
        pt.Y += (int)(stringSize.Height) + DescriptionTextVerticalPixelsOffset;
        if (truncatedText != parts[0])
          e.Node.ToolTipText = parts[0];
      }

      // Draw the description if there is one
      SolidBrush descBrush = null;
      if (disabled)
        descBrush = new SolidBrush(Color.FromArgb(80, 0, 0, 0));
      else
        descBrush = new SolidBrush(Color.FromArgb(Convert.ToInt32(DescriptionColorOpacity * 255), DescriptionColor));
      if (parts != null && parts.Length >= 2)
      {
        truncatedText = MiscUtilities.TruncateString(parts[1], e.Node.TreeView.ClientRectangle.Width - pt.X, e.Graphics, DescriptionFont);
        e.Graphics.DrawString(truncatedText, DescriptionFont, descBrush, pt.X, pt.Y);
        if (truncatedText != parts[1])
          e.Node.ToolTipText += (string.IsNullOrWhiteSpace(e.Node.ToolTipText) ? string.Empty : Environment.NewLine) + parts[1];
      }

      bkBrush.Dispose();
      titleBrush.Dispose();
      descBrush.Dispose();
    }
    
    public TreeNode AddNode(TreeNode parent, string text)
    {
      TreeNode node = (parent != null ? parent.Nodes.Add(text) : Nodes.Add(text));
      if (parent == null)
      {
        node.ForeColor = SystemColors.ControlText;
        node.BackColor = SystemColors.ControlLight;
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