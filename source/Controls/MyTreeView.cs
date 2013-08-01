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
  using System;
  using System.Drawing;
  using System.Runtime.InteropServices;
  using System.Windows.Forms;

  /// <summary>
  /// 
  /// </summary>
  public class MyTreeView : TreeView
  {
    #region Constants

    /// <summary>
    /// Informs the tree-view control to set extended styles.
    /// </summary>
    private const int TVM_SETEXTENDEDSTYLE = 0x112C;

    /// <summary>
    /// Specifies how the background is erased or filled.
    /// </summary>
    private const int TVS_EX_DOUBLEBUFFER = 0x0004;

    /// <summary>
    /// The hItem member is valid.
    /// </summary>
    private const int TVIF_HANDLE = 0x10;

    /// <summary>
    /// The iIntegral member is valid.
    /// </summary>
    private const int TVIF_INTEGRAL = 0x80;

    /// <summary>
    /// Sets some or all of a tree-view item's attributes.
    /// </summary>
    private const int TVM_SETITEM = 0x113F;

    #endregion Constants

    /// <summary>
    /// Multiple number for the height of tree nodes.
    /// </summary>
    private int _nodeHeightMultiple;

    public MyTreeView()
    {
      NodeHeightMultiple = 2;
      DrawMode = TreeViewDrawMode.OwnerDrawAll;
      DoubleBuffered = true;
      ImageHorizontalPixelsOffset = 5;
      ImageToTextHorizontalPixelsOffset = 5;
      TitleColorOpacity = 0.8;
      DescriptionColorOpacity = 0.6;
      TitleTextVerticalPixelsOffset = 0;
      DescriptionTextVerticalPixelsOffset = 0;
      Scrollable = true;
      ShowNodeToolTips = true;
    }

    #region Properties

    /// <summary>
    /// Gets or sets the image to be used for collapsed tree nodes.
    /// </summary>
    public Image CollapsedIcon { get; set; }

    /// <summary>
    /// Gets or sets the color used for the nodes sub-text or description.
    /// </summary>
    public Color DescriptionColor { get; set; }

    /// <summary>
    /// Gets or sets the color opacity factor used for the description text.
    /// </summary>
    public double DescriptionColorOpacity { get; set; }

    /// <summary>
    /// Gets or sets the font used for the description text.
    /// </summary>
    public Font DescriptionFont { get; set; }

    /// <summary>
    /// Gets or sets the vertical offset in pixels for the description text.
    /// </summary>
    public int DescriptionTextVerticalPixelsOffset { get; set; }

    /// <summary>
    /// Gets or sets the image to be used for expanded tree nodes.
    /// </summary>
    public Image ExpandedIcon { get; set; }

    /// <summary>
    /// Gets or sets the horizontal offset in pixels for the node image.
    /// </summary>
    public int ImageHorizontalPixelsOffset { get; set; }

    /// <summary>
    /// Gets or sets the horizontal offset in pixels for the node text relative to the node image.
    /// </summary>
    public int ImageToTextHorizontalPixelsOffset { get; set; }

    /// <summary>
    /// Gets or sets the multiple number for the height of tree nodes.
    /// </summary>
    public int NodeHeightMultiple
    {
      get
      {
        return _nodeHeightMultiple;
      }

      set
      {
        if (value < 1)
        {
          throw new ArgumentOutOfRangeException("NodeHeightMultiple", "Value must be at least 1");
        }

        _nodeHeightMultiple = value;
      }
    }

    /// <summary>
    /// Gets or sets the list of images to be used by the tree view nodes.
    /// </summary>
    public ImageList NodeImages { get; set; }

    /// <summary>
    /// Gets or sets the tree view title color opacity factor.
    /// </summary>
    public double TitleColorOpacity { get; set; }

    /// <summary>
    /// Gets or sets the vertical offset in pixels for the tree view title.
    /// </summary>
    public int TitleTextVerticalPixelsOffset { get; set; }

    #endregion Properties

    /// <summary>
    /// Shows or hides the specified scroll bar.
    /// </summary>
    /// <param name="hWnd">Handle to a scroll bar control or a window with a standard scroll bar, depending on the value of the wBar parameter.</param>
    /// <param name="wBar">Specifies the scroll bar(s) to be shown or hidden.</param>
    /// <param name="bShow">Specifies whether the scroll bar is shown or hidden.</param>
    /// <returns></returns>
    [DllImport("user32.dll")]
    public static extern bool ShowScrollBar(System.IntPtr hWnd, int wBar, bool bShow);

    /// <summary>
    /// Creates a new <see cref="MyTreeNode"/> object and adds it to the specified parent node.
    /// </summary>
    /// <param name="parent">The parent tree node where to add the new node to.</param>
    /// <param name="title">The new node's title text.</param>
    /// <param name="subtitle">The new node's sub-title text.</param>
    /// <returns>The newly created <see cref="MyTreeNode"/> object.</returns>
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

      SetNodeHeight(node, parent != null ? NodeHeightMultiple : (NodeHeightMultiple > 1 ? NodeHeightMultiple - 1 : 1));
      return node;
    }

    /// <summary>
    /// Raises the <see cref="DrawNode"/> event.
    /// </summary>
    /// <param name="e">A <see cref="DrawTreeNodeEventArgs"/> that contains the event data.</param>
    protected override void OnDrawNode(DrawTreeNodeEventArgs e)
    {
      if (e.Node.Level == 0)
      {
        DrawTopLevelNode(e);
      }
      else
      {
        DrawChildNode(e);
      }

      ShowScrollBar(Handle, 0, false);
    }

    /// <summary>
    /// Raises the <see cref="FontChanged"/> event.
    /// </summary>
    /// <param name="e">An <see cref="EventArgs"/> that contains the event data.</param>
    protected override void OnFontChanged(EventArgs e)
    {
      base.OnFontChanged(e);
      if (DescriptionFont != null)
      {
        if (DescriptionFont.Name != Font.Name)
        {
          DescriptionFont = new Font(Font.FontFamily, DescriptionFont.Size, DescriptionFont.Style);
        }
      }
      else
      {
        DescriptionFont = new Font(Font.FontFamily, Font.Size - 1, FontStyle.Regular);
      }

      MarkTruncate(Nodes);
    }

    /// <summary>
    /// Raises the <see cref="HandleCreated"/> event.
    /// </summary>
    /// <param name="e">An <see cref="EventArgs"/> that contains the event data.</param>
    protected override void OnHandleCreated(EventArgs e)
    {
      base.OnHandleCreated(e);
      UpdateExtendedStyles();
    }

    /// <summary>
    /// Raises the <see cref="MouseClick"/> event.
    /// </summary>
    /// <param name="e">An <see cref="MouseEventArgs"/> that contains the event data.</param>
    protected override void OnMouseClick(MouseEventArgs e)
    {
      TreeNode node = GetNodeAt(e.Location);
      if (node != null)
      {
        SelectedNode = node;
      }

      base.OnMouseClick(e);
    }

    /// <summary>
    /// Raises the <see cref="Resize"/> event.
    /// </summary>
    /// <param name="e">An <see cref="EventArgs"/> that contains the event data.</param>
    protected override void OnResize(EventArgs e)
    {
      base.OnResize(e);
      MarkTruncate(Nodes);
    }

    /// <summary>
    /// Sends the specified message to a window or windows. The SendMessage function calls the window procedure for
    /// the specified window and does not return until the window procedure has processed the message.
    /// </summary>
    /// <param name="hWnd">A handle to the window whose window procedure will receive the message.
    /// If this parameter is HWND_BROADCAST ((HWND)0xffff), the message is sent to all top-level windows in the system,
    /// including disabled or invisible unowned windows, overlapped windows, and pop-up windows; but the message is not sent to child windows.</param>
    /// <param name="Msg">The message to be sent.</param>
    /// <param name="wParam">Additional message-specific information.</param>
    /// <param name="lParam">Additional message-specific information.</param>
    /// <returns>Specifies the result of the message processing; it depends on the message sent.</returns>
    [DllImport("user32.dll")]
    private static extern IntPtr SendMessage(IntPtr hWnd, uint Msg, IntPtr wParam, IntPtr lParam);

    /// <summary>
    /// Draws each child node.
    /// </summary>
    /// <param name="e">Event arguments containing a child tree node.</param>
    private void DrawChildNode(DrawTreeNodeEventArgs e)
    {
      if (e.Node is MyTreeNode)
      {
        MyTreeNode myNode = e.Node as MyTreeNode;

        bool disabled = !myNode.Enable;
        Point pt = e.Bounds.Location;
        SizeF titleStringSize = e.Graphics.MeasureString(myNode.Title, Font);
        SizeF descriptionStringSize = e.Graphics.MeasureString(myNode.Subtitle, DescriptionFont);
        Image img = NodeImages != null && NodeImages.Images.Count > 0 && e.Node.ImageIndex >= 0 && e.Node.ImageIndex < NodeImages.Images.Count ? NodeImages.Images[e.Node.ImageIndex] : null;
        int textInitialY = myNode.Subtitle == null ? ((e.Bounds.Height - Convert.ToInt32(titleStringSize.Height) + Convert.ToInt32(descriptionStringSize.Height)) / 2) : 0;
        myNode.ToolTipText = string.Empty;

        //// Paint background
        SolidBrush bkBrush = new SolidBrush(myNode.IsSelected ? SystemColors.MenuHighlight : SystemColors.Window);
        e.Graphics.FillRectangle(bkBrush, e.Bounds);

        //// Paint node Image
        if (img != null)
        {
          pt.X += ImageHorizontalPixelsOffset;
          int y = pt.Y + ((e.Bounds.Height - img.Height) / 2);
          e.Graphics.DrawImage(img, pt.X, y, img.Width, img.Height);
          pt.X += img.Width;
        }

        pt.X += ImageToTextHorizontalPixelsOffset;
        pt.Y += textInitialY + TitleTextVerticalPixelsOffset;

        //// Draw the title if we have one
        string truncatedText = null;
        SolidBrush titleBrush = null;
        if (disabled)
        {
          titleBrush = new SolidBrush(Color.FromArgb(80, 0, 0, 0));
        }
        else
        {
          titleBrush = new SolidBrush(Color.FromArgb(Convert.ToInt32(TitleColorOpacity * 255), ForeColor));
        }

        //// Draw the title
        if (myNode.Title != null)
        {
          SizeF stringSize = e.Graphics.MeasureString(myNode.Title, Font);
          truncatedText = myNode.GetTruncatedTitle(e.Node.TreeView.ClientRectangle.Width - pt.X, e.Graphics, Font);
          e.Graphics.DrawString(truncatedText, Font, titleBrush, pt.X, pt.Y);
          pt.Y += (int)(stringSize.Height) + DescriptionTextVerticalPixelsOffset;
          if (truncatedText != myNode.Title)
          {
            e.Node.ToolTipText = myNode.Title;
          }
        }

        //// Draw the description if there is one
        SolidBrush descBrush = null;
        if (disabled)
        {
          descBrush = new SolidBrush(Color.FromArgb(80, 0, 0, 0));
        }
        else
        {
          descBrush = new SolidBrush(Color.FromArgb(Convert.ToInt32(DescriptionColorOpacity * 255), DescriptionColor));
        }

        if (myNode.Subtitle != null)
        {
          truncatedText = myNode.GetTruncatedSubtitle(e.Node.TreeView.ClientRectangle.Width - pt.X, e.Graphics, DescriptionFont);
          e.Graphics.DrawString(truncatedText, DescriptionFont, descBrush, pt.X, pt.Y);
          if (truncatedText != myNode.Subtitle)
          {
            e.Node.ToolTipText += (string.IsNullOrWhiteSpace(e.Node.ToolTipText) ? string.Empty : Environment.NewLine) + myNode.Subtitle;
          }
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

    /// <summary>
    /// Draws a group node containing child nodes.
    /// </summary>
    /// <param name="e">Event arguments containing a group node.</param>
    private void DrawTopLevelNode(DrawTreeNodeEventArgs e)
    {
      Graphics g = e.Graphics;
      SolidBrush nodeBackbrush = new SolidBrush(e.Node.BackColor);
      g.FillRectangle(nodeBackbrush, e.Bounds);

      Point pt = e.Bounds.Location;

      //// Draw icon centered
      Image i = e.Node.IsExpanded ? ExpandedIcon : CollapsedIcon;
      pt.Y += (e.Bounds.Height - i.Height) / 2;
      e.Graphics.DrawImageUnscaled(i, pt.X, pt.Y, i.Width, i.Height);

      SolidBrush textBrush = new SolidBrush(Color.FromArgb(Convert.ToInt32(TitleColorOpacity * 255), e.Node.ForeColor));
      Font f = e.Node.NodeFont != null ? e.Node.NodeFont : Font;
      if (!f.Bold)
      {
        f = new Font(f.FontFamily, f.Size, FontStyle.Bold);
      }

      SizeF size = g.MeasureString(e.Node.Text, f);
      pt.X += (ImageToTextHorizontalPixelsOffset + i.Width);
      pt.Y = e.Bounds.Top + ((e.Bounds.Height - (int)size.Height) / 2);
      g.DrawString(e.Node.Text, f, textBrush, pt.X, pt.Y);

      nodeBackbrush.Dispose();
      textBrush.Dispose();
    }

    /// <summary>
    /// Truncates the text on child tree nodes.
    /// </summary>
    /// <param name="nodes">Nodes collection to flag their text for truncation.</param>
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

    /// <summary>
    /// Sets the node height for a given tree node given a multiple number for the height.
    /// </summary>
    /// <param name="node">The tree node to have its height modified.</param>
    /// <param name="integral">The multiple number for the height of tree nodes.</param>
    private void SetNodeHeight(TreeNode node, int integral)
    {
      TVITEMEX tex = new TVITEMEX();
      tex.mask = TVIF_HANDLE | TVIF_INTEGRAL;
      tex.hItem = node.Handle;
      tex.iIntegral = integral;

      IntPtr ptr = Marshal.AllocHGlobal(Marshal.SizeOf(tex));
      Marshal.StructureToPtr(tex, ptr, false);

      SendMessage(Handle, TVM_SETITEM, IntPtr.Zero, ptr);
      Marshal.FreeHGlobal(ptr);
    }

    /// <summary>
    /// Updates the extended styles of the tree view.
    /// </summary>
    private void UpdateExtendedStyles()
    {
      int Style = 0;

      if (DoubleBuffered)
      {
        Style |= TVS_EX_DOUBLEBUFFER;
      }

      if (Style != 0)
      {
        SendMessage(Handle, TVM_SETEXTENDEDSTYLE, (IntPtr)TVS_EX_DOUBLEBUFFER, (IntPtr)Style);
      }
    }
  }

  /// <summary>
  /// Specifies or receives attributes of a tree-view item.
  /// </summary>
  /// <remarks>DO NOT change the order of the struct elements since C++ expects it in this specific order.</remarks>
  internal struct TVITEMEX
  {
    /// <summary>
    /// Array of flags that indicate which of the other structure members contain valid data.
    /// When this structure is used with the TVM_GETITEM message, the mask member indicates the item attributes to retrieve.
    /// If used with the TVM_SETITEM message, the mask indicates the attributes to set.
    /// </summary>
    public int mask;

    /// <summary>
    /// Handle to the item.
    /// </summary>
    public IntPtr hItem;

    /// <summary>
    /// Set of bit flags and image list indexes that indicate the item's state.
    /// When setting the state of an item, the stateMask member indicates the valid bits of this member.
    /// When retrieving the state of an item, this member returns the current state for the bits indicated in the stateMask member.
    /// </summary>
    public int state;

    /// <summary>
    /// Bits of the state member that are valid. If you are retrieving an item's state, set the bits of the stateMask member
    /// to indicate the bits to be returned in the state member. If you are setting an item's state, set the bits of the stateMask
    /// member to indicate the bits of the state member that you want to set. To set or retrieve an item's overlay image index,
    /// set the TVIS_OVERLAYMASK bits. To set or retrieve an item's state image index, set the TVIS_STATEIMAGEMASK bits.
    /// </summary>
    public int stateMask;

    /// <summary>
    /// Pointer to a null-terminated string that contains the item text if the structure specifies item attributes.
    /// If this member is the LPSTR_TEXTCALLBACK value, the parent window is responsible for storing the name.
    /// In this case, the tree-view control sends the parent window a TVN_GETDISPINFO notification code when it needs
    /// the item text for displaying, sorting, or editing and a TVN_SETDISPINFO notification code when the item text changes.
    /// If the structure is receiving item attributes, this member is the address of the buffer that receives the item text.
    /// Note that although the tree-view control allows any length string to be stored as item text, only the first 260 characters are displayed.
    /// </summary>
    [MarshalAs(UnmanagedType.LPTStr)]
    public String lpszText;

    /// <summary>
    /// Size of the buffer pointed to by the pszText member, in characters.
    /// If this structure is being used to set item attributes, this member is ignored.
    /// </summary>
    public int cchTextMax;

    /// <summary>
    /// Index in the tree-view control's image list of the icon image to use when the item is in the nonselected state.
    /// If this member is the I_IMAGECALLBACK value, the parent window is responsible for storing the index. In this case,
    /// the tree-view control sends the parent a TVN_GETDISPINFO notification code to retrieve the index when it needs to display the image.
    /// </summary>
    public int iImage;

    /// <summary>
    /// Index in the tree-view control's image list of the icon image to use when the item is in the selected state.
    /// If this member is the I_IMAGECALLBACK value, the parent window is responsible for storing the index. In this case,
    /// the tree-view control sends the parent a TVN_GETDISPINFO notification code to retrieve the index when it needs to display the image.
    /// </summary>
    public int iSelectedImage;

    /// <summary>
    /// Flag that indicates whether the item has associated child items.
    /// </summary>
    public int cChildren;

    /// <summary>
    /// A value to associate with the item.
    /// </summary>
    public IntPtr lParam;

    /// <summary>
    /// Height of the item, in multiples of the standard item height (see TVM_SETITEMHEIGHT).
    /// For example, setting this member to 2 will give the item twice the standard height.
    /// The tree-view control does not draw in the extra area, which appears below the item content, but this space can be used by
    /// the application for drawing when using custom draw. Applications that are not using custom draw should set this value to 1,
    /// as otherwise the behavior is undefined.
    /// </summary>
    public int iIntegral;
  }

  /// <summary>
  /// Represents a node of a <see cref="MyTreeView"/> .
  /// </summary>
  public class MyTreeNode : TreeNode
  {
    #region Fields

    /// <summary>
    /// The sub-title text displayed in the tree node.
    /// </summary>
    private string _subtitle;

    /// <summary>
    /// The title text displayed in the tree node.
    /// </summary>
    private string _title;

    /// <summary>
    /// The truncated sub-title text.
    /// </summary>
    private string _truncatedSubtitle;

    /// <summary>
    /// The truncated title text.
    /// </summary>
    private string _truncatedTitle;

    #endregion Fields
    
    /// <summary>
    /// Initializes a new instance of the <see cref="MyTreeNode"/> class.
    /// </summary>
    public MyTreeNode()
      : base()
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="MyTreeNode"/> class.
    /// </summary>
    /// <param name="title">The title text of the tree node.</param>
    public MyTreeNode(string title)
      : this(title, null)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="MyTreeNode"/> class.
    /// </summary>
    /// <param name="title">The title text of the tree node.</param>
    /// <param name="subtitle">The sub-title text of the tree node.</param>
    public MyTreeNode(string title, string subtitle)
      : base(title)
    {
      Title = title;
      Subtitle = subtitle;
      Enable = true;
    }

    #region Properties

    /// <summary>
    /// Gets or sets a flag indicating whether the tree node is displayed as being enabled.
    /// </summary>
    public bool Enable { get; set; }

    /// <summary>
    /// Gets or sets the sub-title text displayed in the tree node.
    /// </summary>
    public string Subtitle
    {
      get
      {
        return _subtitle;
      }

      set
      {
        _subtitle = value;
        UpdateTruncatedSubtitle = true;
      }
    }

    /// <summary>
    /// Gets or sets the title text displayed in the tree node.
    /// </summary>
    public string Title
    {
      get
      {
        return _title;
      }

      set
      {
        _title = value;
        UpdateTruncatedTitle = true;
      }
    }

    /// <summary>
    /// Gets or sets a value indicating whether the sub-title text is truncated.
    /// </summary>
    public bool UpdateTruncatedSubtitle { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether the title text is truncated.
    /// </summary>
    public bool UpdateTruncatedTitle { get; set; }

    #endregion Properties

    /// <summary>
    /// Truncates the sub-title text and returns a new string with the truncated text.
    /// </summary>
    /// <param name="maxWidth">Maximum text width.</param>
    /// <param name="graphics">Graphics used to draw the text.</param>
    /// <param name="font">The font used by the sub-title text.</param>
    /// <returns>A new string with the truncated text.</returns>
    public string GetTruncatedSubtitle(float maxWidth, Graphics graphics, Font font)
    {
      if (UpdateTruncatedSubtitle)
      {
        if (maxWidth > 0)
        {
          _truncatedSubtitle = MiscUtilities.TruncateString(Subtitle, maxWidth, graphics, font);
        }

        UpdateTruncatedSubtitle = false;
      }

      return _truncatedSubtitle;
    }

    /// <summary>
    /// Truncates the title text and returns a new string with the truncated text.
    /// </summary>
    /// <param name="maxWidth">Maximum text width.</param>
    /// <param name="graphics">Graphics used to draw the text.</param>
    /// <param name="font">The font used by the title text.</param>
    /// <returns>A new string with the truncated text.</returns>
    public string GetTruncatedTitle(float maxWidth, Graphics graphics, Font font)
    {
      if (UpdateTruncatedTitle)
      {
        if (maxWidth > 0)
        {
          _truncatedTitle = MiscUtilities.TruncateString(Title, maxWidth, graphics, font);
        }

        UpdateTruncatedTitle = false;
      }

      return _truncatedTitle;
    }
  }
}