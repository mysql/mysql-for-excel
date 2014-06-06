// Copyright (c) 2012-2014, Oracle and/or its affiliates. All rights reserved.
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

using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using MySQL.ForExcel.Classes;
using MySQL.ForExcel.Structs;
using MySQL.Utility.Classes;
using MySQL.Utility.Classes.MySQLWorkbench;

namespace MySQL.ForExcel.Controls
{
  /// <summary>
  /// Custom <see cref="TreeView"/> that lists MySQL related objects.
  /// </summary>
  public class MySqlListView : TreeView
  {
    #region Constants

    /// <summary>
    /// The default color opacity factor used for the description text.
    /// </summary>
    public const double DEFAULT_DESCRIPTION_COLOR_OPACITY = 0.6;

    /// <summary>
    /// The vertical offset in pixels for the description text.
    /// </summary>
    public const int DEFAULT_DESCRIPTION_TEXT_VERTICAL_PIXELS_OFFSET = 0;

    /// <summary>
    /// The default horizontal offset in pixels for the node image.
    /// </summary>
    public const int DEFAULT_IMAGE_HORIZONTAL_PIXELS_OFFSET = 5;

    /// <summary>
    /// The default horizontal offset in pixels for the node text relative to the node image.
    /// </summary>
    public const int DEFAULT_IMAGE_TO_TEXT_HORIZONTAL_PIXELS_OFFSET = 5;

    /// <summary>
    /// The default multiple number for the height of tree nodes.
    /// </summary>
    public const int DEFAULT_NODE_HEIGH_TMULTIPLE = 2;

    /// <summary>
    /// The default tree view title color opacity factor.
    /// </summary>
    public const double DEFAULT_TITLE_COLOR_OPACITY = 0.8;

    /// <summary>
    /// The default vertical offset in pixels for the tree view title.
    /// </summary>
    public const int DEFAULT_TITLE_TEXT_VERTICAL_PIXELS_OFFSET = 0;

    /// <summary>
    /// The hItem member is valid.
    /// </summary>
    private const int TVIF_HANDLE = 0x10;

    /// <summary>
    /// The iIntegral member is valid.
    /// </summary>
    private const int TVIF_INTEGRAL = 0x80;

    /// <summary>
    /// Informs the tree-view control to set extended styles.
    /// </summary>
    private const int TVM_SETEXTENDEDSTYLE = 0x112C;

    /// <summary>
    /// Sets some or all of a tree-view item's attributes.
    /// </summary>
    private const int TVM_SETITEM = 0x113F;

    /// <summary>
    /// Specifies how the background is erased or filled.
    /// </summary>
    private const int TVS_EX_DOUBLEBUFFER = 0x0004;

    #endregion Constants

    #region Fields

    /// <summary>
    /// Multiple number for the height of tree nodes.
    /// </summary>
    private int _nodeHeightMultiple;

    /// <summary>
    /// The node that is currently selected.
    /// </summary>
    private MySqlListViewNode _selectedNode;

    /// <summary>
    /// Collection of selected nodes.
    /// </summary>
    private List<MySqlListViewNode> _selectedNodes;

    #endregion Fields

    /// <summary>
    /// Initializes a new instance of the <see cref="MySqlListView"/> class.
    /// </summary>
    public MySqlListView()
    {
      base.SelectedNode = null;
      _selectedNode = null;
      _selectedNodes = new List<MySqlListViewNode>();
      HeaderNodes = new List<MySqlListViewNode>();
      MultiSelect = false;
      NodeHeightMultiple = DEFAULT_NODE_HEIGH_TMULTIPLE;
      DoubleBuffered = true;
      DrawMode = TreeViewDrawMode.OwnerDrawAll;
      ImageHorizontalPixelsOffset = DEFAULT_IMAGE_HORIZONTAL_PIXELS_OFFSET;
      ImageToTextHorizontalPixelsOffset = DEFAULT_IMAGE_TO_TEXT_HORIZONTAL_PIXELS_OFFSET;
      TitleColorOpacity = DEFAULT_TITLE_COLOR_OPACITY;
      DescriptionColorOpacity = DEFAULT_DESCRIPTION_COLOR_OPACITY;
      TitleTextVerticalPixelsOffset = DEFAULT_TITLE_TEXT_VERTICAL_PIXELS_OFFSET;
      DescriptionTextVerticalPixelsOffset = DEFAULT_DESCRIPTION_TEXT_VERTICAL_PIXELS_OFFSET;
      Scrollable = true;
      ShowNodeToolTips = true;
    }

    #region Enums

    /// <summary>
    /// Specifies identifiers to indicate the type of source that triggered a single node selection.
    /// </summary>
    public enum SingleSelectionSource
    {
      /// <summary>
      /// A <see cref="TreeView.ItemDrag"/> event.
      /// </summary>
      ItemDrag,

      /// <summary>
      /// A <see cref="Control.KeyDown"/> event.
      /// </summary>
      KeyDown,

      /// <summary>
      /// The <see cref="SelectNode"/> method.
      /// </summary>
      SelectNode
    }

    /// <summary>
    /// Specifies identifiers to indicate the direction in which nodes are traversed within a collection.
    /// </summary>
    private enum NodesTraversingDirection
    {
      /// <summary>
      /// Nodes are traversed from end to start.
      /// </summary>
      Backward,

      /// <summary>
      /// Nodes are traversed from start to end.
      /// </summary>
      Forward
    }

    #endregion Enums

    #region Properties

    /// <summary>
    /// Gets or sets a value indicating whether check boxes are displayed next to list view items.
    /// </summary>
    /// <remarks>Overriding this property so it does not affect the control refresh.</remarks>
    public new bool CheckBoxes { get; set; }

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
    /// Gets the list of header nodes containing sub-nodes.
    /// </summary>
    [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    public List<MySqlListViewNode> HeaderNodes { get; private set; }

    /// <summary>
    /// Gets or sets the horizontal offset in pixels for the node image.
    /// </summary>
    public int ImageHorizontalPixelsOffset { get; set; }

    /// <summary>
    /// Gets or sets the horizontal offset in pixels for the node text relative to the node image.
    /// </summary>
    public int ImageToTextHorizontalPixelsOffset { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether the control allows the selection of multiple nodes.
    /// </summary>
    public bool MultiSelect { get; set; }

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
          throw new IndexOutOfRangeException("Value must be at least 1");
        }

        _nodeHeightMultiple = value;
      }
    }

    /// <summary>
    /// Gets or sets the list of images to be used by the tree view nodes.
    /// </summary>
    public ImageList NodeImages { get; set; }

    /// <summary>
    /// Gets or sets the node that is currently selected.
    /// </summary>
    /// <remarks>Overriding this property to implement own selection method.</remarks>
    [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    public new MySqlListViewNode SelectedNode
    {
      get
      {
        return _selectedNode;
      }

      set
      {
        ClearSelectedNodes();
        if (value != null)
        {
          SelectNodes(value);
        }
      }
    }

    /// <summary>
    /// Gets or sets the collection of selected nodes.
    /// </summary>
    [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    public List<MySqlListViewNode> SelectedNodes
    {
      get
      {
        return _selectedNodes;
      }

      set
      {
        if (!MultiSelect || value == null)
        {
          return;
        }

        ClearSelectedNodes();
        if (value.Count == 0)
        {
          return;
        }

        foreach (var node in value)
        {
          MarkNodeAsSelected(node, true);
        }

        OnAfterSelect(new TreeViewEventArgs(value[value.Count - 1]));
      }
    }

    /// <summary>
    /// Gets or sets the tree view title color opacity factor.
    /// </summary>
    public double TitleColorOpacity { get; set; }

    /// <summary>
    /// Gets or sets the vertical offset in pixels for the tree view title.
    /// </summary>
    public int TitleTextVerticalPixelsOffset { get; set; }

    /// <summary>
    /// Overriden property to hide the horizontal scrollbar.
    /// </summary>
    [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    protected override CreateParams CreateParams
    {
      get
      {
        var cp = base.CreateParams;
        cp.Style |= 0x8000; // TVS_NOHSCROLL
        return cp;
      }
    }

    /// <summary>
    /// Gets or sets a value indicating whether the control should redraw its surface using a secondary buffer.
    /// </summary>
    protected override sealed bool DoubleBuffered
    {
      get
      {
        return base.DoubleBuffered;
      }

      set
      {
        base.DoubleBuffered = value;
      }
    }

    /// <summary>
    /// Gets the collection of tree nodes that are assigned to the control.
    /// </summary>
    private new TreeNodeCollection Nodes
    {
      get
      {
        return base.Nodes;
      }
    }

    #endregion Properties

    /// <summary>
    /// Creates a new header node that will contain sub-nodes.
    /// </summary>
    /// <param name="title">The new node's title text.</param>
    /// <returns>The newly created <see cref="MySqlListViewNode"/> object.</returns>
    public MySqlListViewNode AddHeaderNode(string title)
    {
      var node = new MySqlListViewNode(title);
      Nodes.Add(node);
      HeaderNodes.Add(node);
      SetNodeHeight(node, NodeHeightMultiple - 1);
      node.ForeColor = SystemColors.ControlText;
      node.BackColor = SystemColors.ControlLight;
      return node;
    }

    /// <summary>
    /// Creates a new header node that holds connection information.
    /// </summary>
    /// <param name="parentNode">The parent node under which to add the new node.</param>
    /// <param name="connection">The <see cref="MySqlWorkbenchConnection"/> associated to the node</param>
    /// <returns>The newly created <see cref="MySqlListViewNode"/> object.</returns>
    public MySqlListViewNode AddConnectionNode(MySqlListViewNode parentNode, MySqlWorkbenchConnection connection)
    {
      var node = new MySqlListViewNode(connection);
      parentNode.Nodes.Add(node);
      SetNodeHeight(node, NodeHeightMultiple);
      return node;
    }

    /// <summary>
    /// Creates a new header node that holds database objects information.
    /// </summary>
    /// <param name="parentNode">The parent node under which to add the new node.</param>
    /// <param name="dbObject">The <see cref="DbObject"/> related to the node.</param>
    /// <returns>The newly created <see cref="MySqlListViewNode"/> object.</returns>
    public MySqlListViewNode AddDbObjectNode(MySqlListViewNode parentNode, DbObject dbObject)
    {
      var node = new MySqlListViewNode(dbObject);
      parentNode.Nodes.Add(node);
      SetNodeHeight(node, NodeHeightMultiple);
      return node;
    }

    /// <summary>
    /// Clears the nodes under header nodes.
    /// </summary>
    public void ClearNodes()
    {
      foreach (var headerNode in HeaderNodes)
      {
        headerNode.Nodes.Clear();
      }
    }

    /// <summary>
    /// Raises the <see cref="TreeView.AfterSelect"/> event.
    /// </summary>
    /// <param name="e">A <see cref="TreeViewEventArgs"/> that contains the event data.</param>
    protected override void OnAfterSelect(TreeViewEventArgs e)
    {
      base.OnAfterSelect(e);

      // Never allow base.SelectedNode to be set since we are overriding the selection behavior.
      base.SelectedNode = null;
    }

    /// <summary>
    /// Raises the <see cref="TreeView.BeforeSelect"/> event.
    /// </summary>
    /// <param name="e">A <see cref="TreeViewCancelEventArgs"/> that contains the event data.</param>
    protected override void OnBeforeSelect(TreeViewCancelEventArgs e)
    {
      // Never allow base.SelectedNode to be set since we are overriding the selection behavior.
      base.SelectedNode = null;
      e.Cancel = true;
      base.OnBeforeSelect(e);
    }

    /// <summary>
    /// Raises the <see cref="TreeView.DrawNode"/> event.
    /// </summary>
    /// <param name="e">A <see cref="DrawTreeNodeEventArgs"/> that contains the event data.</param>
    protected override void OnDrawNode(DrawTreeNodeEventArgs e)
    {
      var node = e.Node as MySqlListViewNode;
      if (node == null)
      {
        return;
      }

      try
      {
        if (node.Type == MySqlListViewNode.MySqlNodeType.Header)
        {
          DrawTopLevelNode(e);
        }
        else
        {
          DrawChildNode(e);
        }
      }
      catch (Exception ex)
      {
        MiscUtilities.ShowCustomizedErrorDialog(Properties.Resources.RefreshDBObjectsErrorTitle, ex.Message, true);
        MySqlSourceTrace.WriteAppErrorToLog(ex);
      }
    }

    /// <summary>
    /// Raises the <see cref="Control.FontChanged"/> event.
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
    /// Raises the <see cref="Control.GotFocus"/> event.
    /// </summary>
    /// <param name="e">An <see cref="EventArgs"/> that contains the event data.</param>
    protected override void OnGotFocus(EventArgs e)
    {
      // Make sure at least one node has a selection this way we can tab to the control and use the keyboard to select nodes
      if (_selectedNode == null && TopNode != null)
      {
        MarkNodeAsSelected(TopNode as MySqlListViewNode, true);
      }

      base.OnGotFocus(e);
    }

    /// <summary>
    /// Raises the <see cref="Control.HandleCreated"/> event.
    /// </summary>
    /// <param name="e">An <see cref="EventArgs"/> that contains the event data.</param>
    protected override void OnHandleCreated(EventArgs e)
    {
      base.OnHandleCreated(e);
      UpdateExtendedStyles();
    }

    /// <summary>
    /// Raises the <see cref="TreeView.ItemDrag"/> event.
    /// </summary>
    /// <param name="e">An <see cref="ItemDragEventArgs"/> that contains the event data.</param>
    protected override void OnItemDrag(ItemDragEventArgs e)
    {
      // If the user drags a node and the node being dragged is NOT selected, then clear the active selection, select the node being dragged and drag it.
      // Otherwise if the node being dragged is selected, drag the entire selection.
      var node = e.Item as MySqlListViewNode;
      if (node != null && !_selectedNodes.Contains(node))
      {
        SelectSingleNode(node, SingleSelectionSource.ItemDrag);
        MarkNodeAsSelected(node, true);
      }

      base.OnItemDrag(e);
    }

    /// <summary>
    /// Raises the <see cref="Control.KeyDown"/> event.
    /// </summary>
    /// <param name="e">An <see cref="KeyEventArgs"/> that contains the event data.</param>
    protected override void OnKeyDown(KeyEventArgs e)
    {
      // Handle all possible key strokes for the control including navigation, selection, etc.
      base.OnKeyDown(e);
      if (e.KeyCode == Keys.ShiftKey)
      {
        return;
      }

      BeginUpdate();
      var shiftPressed = ModifierKeys == Keys.Shift;
      var controlPressed = ModifierKeys == Keys.Control;
      try
      {
        // Nothing is selected in the tree, this isn't a good state select the first node that is not a header node.
        if (_selectedNode == null)
        {
          MarkNodeAsSelected(GetFirstChildNode(), true);
        }

        // Nothing is still selected in the tree, this isn't a good state, leave.
        if (_selectedNode == null)
        {
          return;
        }

        TreeNode ndCurrent;
        int nodesCount;
        switch (e.KeyCode)
        {
          case Keys.Enter:
            OnNodeMouseDoubleClick(new TreeNodeMouseClickEventArgs(_selectedNode, MouseButtons.Left, 2, _selectedNode.Bounds.X, _selectedNode.Bounds.Y));
            break;

          case Keys.Left:
            if (_selectedNode.IsExpanded && _selectedNode.Nodes.Count > 0)
            {
              // Collapse an expanded node that has children
              _selectedNode.Collapse();
            }
            else if (_selectedNode.Parent != null)
            {
              // Node is already collapsed, try to select its parent.
              SelectSingleNode(_selectedNode.Parent as MySqlListViewNode, SingleSelectionSource.KeyDown);
            }
            break;

          case Keys.Right:
            if (_selectedNode.Nodes.Count == 0)
            {
              break;
            }

            if (!_selectedNode.IsExpanded)
            {
              // Expand a collpased node's children
              _selectedNode.Expand();
            }
            else
            {
              // Node was already expanded, select the first child
              SelectSingleNode(_selectedNode.FirstNode as MySqlListViewNode, SingleSelectionSource.KeyDown);
            }
            break;

          case Keys.Up:
            // Select the previous node
            if (_selectedNode.PrevVisibleNode != null)
            {
              SelectNodes(_selectedNode.PrevVisibleNode as MySqlListViewNode);
            }
            break;

          case Keys.Down:
            // Select the next node
            if (_selectedNode.NextVisibleNode != null)
            {
              SelectNodes(_selectedNode.NextVisibleNode as MySqlListViewNode);
            }
            break;

          case Keys.Home:
            if (shiftPressed && MultiSelect)
            {
              if (_selectedNode.Parent != null)
              {
                // Select all of the nodes up to this point under this nodes parent
                SelectNodes(_selectedNode.Parent.FirstNode as MySqlListViewNode);
              }
            }
            else
            {
              // Select this first node in this branch
              if (Nodes.Count > 0)
              {
                SelectSingleNode(_selectedNode.Parent.FirstNode as MySqlListViewNode, SingleSelectionSource.KeyDown);
              }
            }
            break;

          case Keys.End:
            if (_selectedNode.Parent == null)
            {
              break;
            }

            if (shiftPressed && MultiSelect)
            {
              // Select the last node in this branch
              SelectNodes(_selectedNode.Parent.LastNode as MySqlListViewNode);
            }
            else
            {
              // Select the last node in the group.
              SelectSingleNode(_selectedNode.Parent.LastNode as MySqlListViewNode, SingleSelectionSource.KeyDown);
            }
            break;

          case Keys.PageUp:
            // Select the highest node in the display
            nodesCount = VisibleCount;
            ndCurrent = _selectedNode;
            while ((nodesCount) > 0 && (ndCurrent.PrevVisibleNode != null))
            {
              ndCurrent = ndCurrent.PrevVisibleNode;
              nodesCount--;
            }

            SelectSingleNode(ndCurrent as MySqlListViewNode, SingleSelectionSource.KeyDown);
            break;

          case Keys.PageDown:
            // Select the lowest node in the display
            nodesCount = VisibleCount;
            ndCurrent = _selectedNode;
            while ((nodesCount) > 0 && (ndCurrent.NextVisibleNode != null))
            {
              ndCurrent = ndCurrent.NextVisibleNode;
              nodesCount--;
            }

            SelectSingleNode(ndCurrent as MySqlListViewNode, SingleSelectionSource.KeyDown);
            break;

          case Keys.A:
            if (!controlPressed)
            {
              goto default;
            }

            if (MultiSelect && !_selectedNode.ExcludeFromMultiSelection)
            {
              SelectAllNodes();
            }
            break;

          default:
            // Assume this is a search character a-z, A-Z, 0-9, etc.
            // Select the first node after the current node that starts with this character
            var sSearch = ((char)e.KeyValue).ToString(CultureInfo.InvariantCulture);
            ndCurrent = _selectedNode;
            while ((ndCurrent.NextVisibleNode != null))
            {
              ndCurrent = ndCurrent.NextVisibleNode;
              if (!ndCurrent.Text.StartsWith(sSearch))
              {
                continue;
              }

              SelectSingleNode(ndCurrent as MySqlListViewNode, SingleSelectionSource.KeyDown);
              break;
            }
            break;
        }
      }
      catch (Exception ex)
      {
        MySqlSourceTrace.WriteAppErrorToLog(ex);
      }
      finally
      {
        EndUpdate();
      }
    }

    /// <summary>
    /// Raises the <see cref="Control.MouseDown"/> event.
    /// </summary>
    /// <param name="e">An <see cref="MouseEventArgs"/> that contains the event data.</param>
    protected override void OnMouseDown(MouseEventArgs e)
    {
      // If the user clicks on a node that was not previously selected, select it now.
      base.SelectedNode = null;
      var node = GetNodeAt(e.Location) as MySqlListViewNode;
      if (node != null)
      {
        int leftBound = node.Bounds.X; // - 20; // Allow user to click on image
        int rightBound = node.Bounds.Right + 10; // Give a little extra room
        if (e.Location.X > leftBound && e.Location.X < rightBound)
        {
          if (ModifierKeys == Keys.None && (_selectedNodes.Contains(node)))
          {
            // Possible mouse drop, let MouseUp handle the case.
          }
          else
          {
            SelectNodes(node);
          }
        }
      }

      base.OnMouseDown(e);
    }

    /// <summary>
    /// Raises the <see cref="Control.MouseUp"/> event.
    /// </summary>
    /// <param name="e">An <see cref="MouseEventArgs"/> that contains the event data.</param>
    protected override void OnMouseUp(MouseEventArgs e)
    {
      // If the user clicked on a node that was previously selected then reselect it now.
      // This will clear any other selected nodes. e.g. A B C D are selected the user clicks on B, now A C & D are no longer selected.
      var node = GetNodeAt(e.Location) as MySqlListViewNode;
      if (node != null)
      {
        if (ModifierKeys == Keys.None && _selectedNodes.Contains(node))
        {
          int leftBound = node.Bounds.X; // -20; // Allow user to click on image
          int rightBound = node.Bounds.Right + 10; // Give a little extra room
          if (e.Location.X > leftBound && e.Location.X < rightBound)
          {

            SelectNodes(node);
          }
        }
      }

      base.OnMouseUp(e);
    }

    /// <summary>
    /// Raises the <see cref="Control.Resize"/> event.
    /// </summary>
    /// <param name="e">An <see cref="EventArgs"/> that contains the event data.</param>
    protected override void OnResize(EventArgs e)
    {
      base.OnResize(e);
      MarkTruncate(Nodes);
    }

    /// <summary>
    /// Gets the first node that is not a header node starting the search from the top of the given <see cref="MySqlListViewNode"/>.
    /// </summary>
    /// <param name="parentNode">Node containing child nodes to traverse. If <c>null</c> it means we start at the very top root node.</param>
    /// <returns>The first node that is not a header node.</returns>
    private MySqlListViewNode GetFirstChildNode(MySqlListViewNode parentNode = null)
    {
      var childNodes = parentNode == null ? Nodes : parentNode.Nodes;
      foreach (MySqlListViewNode node in childNodes)
      {
        if (node.Type == MySqlListViewNode.MySqlNodeType.Header || node.Nodes.Count > 0)
        {
          return GetFirstChildNode(node);
        }

        return node;
      }

      return null;
    }

    /// <summary>
    /// Truncates the text on child tree nodes.
    /// </summary>
    /// <param name="nodes">Nodes collection to flag their text for truncation.</param>
    private static void MarkTruncate(IEnumerable nodes)
    {
      foreach (MySqlListViewNode child in nodes)
      {
        child.UpdateTruncatedTitle = true;
        child.UpdateTruncatedSubtitle = true;
        MarkTruncate(child.Nodes);
      }
    }

    /// <summary>
    /// Clears the nodes selection.
    /// </summary>
    private void ClearSelectedNodes()
    {
      foreach (var node in _selectedNodes)
      {
        node.BackColor = BackColor;
        node.IsSelected = false;
      }

      _selectedNodes.Clear();
      _selectedNode = null;
    }

    /// <summary>
    /// Draws each child node.
    /// </summary>
    /// <param name="e">Event arguments containing a child tree node.</param>
    private void DrawChildNode(DrawTreeNodeEventArgs e)
    {
      string truncatedText;
      var myNode = e.Node as MySqlListViewNode;
      if (myNode == null)
      {
        return;
      }

      bool disabled = !myNode.Enable;
      Point pt = e.Bounds.Location;
      SizeF titleStringSize = e.Graphics.MeasureString(myNode.Title, Font);
      SizeF descriptionStringSize = e.Graphics.MeasureString(myNode.Subtitle, DescriptionFont);
      Image img = NodeImages != null && NodeImages.Images.Count > 0 && e.Node.ImageIndex >= 0 && e.Node.ImageIndex < NodeImages.Images.Count ? NodeImages.Images[e.Node.ImageIndex] : null;
      int textInitialY = myNode.Subtitle == null ? ((e.Bounds.Height - Convert.ToInt32(titleStringSize.Height) + Convert.ToInt32(descriptionStringSize.Height)) / 2) : 0;
      myNode.ToolTipText = string.Empty;

      // Paint background
      var bkBrush = new SolidBrush(myNode.BackColor);
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
      var titleBrush = disabled ? new SolidBrush(Color.FromArgb(80, 0, 0, 0)) : new SolidBrush(Color.FromArgb(Convert.ToInt32(TitleColorOpacity * 255), ForeColor));
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

      // Draw the description if there is one
      var descBrush = disabled ? new SolidBrush(Color.FromArgb(80, 0, 0, 0)) : new SolidBrush(Color.FromArgb(Convert.ToInt32(DescriptionColorOpacity * 255), DescriptionColor));
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

      // Draw icon centered
      Image i = e.Node.IsExpanded ? ExpandedIcon : CollapsedIcon;
      pt.Y += (e.Bounds.Height - i.Height) / 2;
      e.Graphics.DrawImageUnscaled(i, pt.X, pt.Y, i.Width, i.Height);

      SolidBrush textBrush = new SolidBrush(Color.FromArgb(Convert.ToInt32(TitleColorOpacity * 255), e.Node.ForeColor));
      Font f = e.Node.NodeFont ?? Font;
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
    /// Adds the given node to the selected nodes collection and marks it as the currently selected node.
    /// </summary>
    /// <param name="node">A <see cref="MySqlListViewNode"/> object.</param>
    /// <param name="isSelected">Flag indicating if the node is to be marked as selected or not.</param>
    private void MarkNodeAsSelected(MySqlListViewNode node, bool isSelected)
    {
      if (node == null)
      {
        return;
      }

      var unselectableNode = node.Type == MySqlListViewNode.MySqlNodeType.Header;
      _selectedNode = node;
      if (unselectableNode)
      {
        return;
      }

      node.BackColor = isSelected ? SystemColors.Highlight : BackColor;
      node.IsSelected = isSelected;
      if (isSelected)
      {
        if (!_selectedNodes.Contains(node))
        {
          _selectedNodes.Add(node);
        }
      }
      else
      {
        _selectedNodes.Remove(node);
      }
    }

    /// <summary>
    /// Adds the nodes within the nodes range to the selected nodes collection.
    /// </summary>
    /// <param name="startingNode">The starting node of the range.</param>
    /// <param name="endingNode">The ending node of the range.</param>
    /// <param name="direction">The direction in which nodes are traversed within the range.</param>
    /// <param name="includeStartingNode">Flag indicating whether the starting node is marked as selected.</param>
    private void MarkNodesRangeAsSelected(MySqlListViewNode startingNode, MySqlListViewNode endingNode, NodesTraversingDirection direction, bool includeStartingNode = false)
    {
      if (startingNode == null || endingNode == null)
      {
        return;
      }

      if (includeStartingNode)
      {
        MarkNodeAsSelected(startingNode, true);
      }

      while (startingNode != endingNode)
      {
        startingNode = (direction == NodesTraversingDirection.Forward
          ? startingNode.NextVisibleNode
          : startingNode.PrevVisibleNode) as MySqlListViewNode;
        if (startingNode == null)
        {
          break;
        }

        if (startingNode.ExcludeFromMultiSelection)
        {
          continue;
        }

        MarkNodeAsSelected(startingNode, true);
      }
    }

    /// <summary>
    /// Marks all nodes as selected.
    /// </summary>
    /// <param name="parentNode">Node containing child nodes to traverse. If <c>null</c> it means we start at the very top root node.</param>
    private void SelectAllNodes(MySqlListViewNode parentNode = null)
    {
      if (parentNode == null)
      {
        ClearSelectedNodes();
      }

      var nodes = parentNode != null ? parentNode.Nodes : Nodes;
      foreach (var childNode in nodes.Cast<MySqlListViewNode>().Where(childNode => !childNode.ExcludeFromMultiSelection))
      {
        MarkNodeAsSelected(childNode, true);
        if (childNode.Nodes.Count > 0)
        {
          SelectAllNodes(childNode);
        }
      }

      if (parentNode != null || _selectedNodes.Count <= 0)
      {
        return;
      }

      // When we are at the very top of the tree-control flag the first node as the selected node.
      _selectedNode = _selectedNodes[0];
      OnAfterSelect(new TreeViewEventArgs(_selectedNode));
    }

    /// <summary>
    /// Marks the given node as selected and a range of nodes from the currently selected node to the given node if the modifier key is SHIFT.
    /// </summary>
    /// <param name="node">A <see cref="MySqlListViewNode"/> object.</param>
    private void SelectNodes(MySqlListViewNode node)
    {
      try
      {
        BeginUpdate();

        if (MultiSelect)
        {
          if (_selectedNode == null || ModifierKeys == Keys.Control)
          {
            // Ctrl+Click selects an unselected node, or unselects a selected node.
            bool isSelected = _selectedNodes.Contains(node);
            MarkNodeAsSelected(node, !isSelected);
          }
          else if (ModifierKeys == Keys.Shift)
          {
            // Shift+Click selects nodes between the selected node and here.
            var ndStart = _selectedNode;
            var ndEnd = node;
            if (ndStart.Parent == ndEnd.Parent)
            {
              // Selected node and clicked node have same parent, easy case.
              if (ndStart.Index < ndEnd.Index)
              {
                // If the selected node is beneath the clicked node walk down selecting each Visible node until we reach the end.
                MarkNodesRangeAsSelected(ndStart, ndEnd, NodesTraversingDirection.Forward);
              }
              else if (ndStart.Index == ndEnd.Index)
              {
                // Clicked same node, do nothing
              }
              else
              {
                // If the selected node is above the clicked node walk up selecting each Visible node until we reach the end.
                MarkNodesRangeAsSelected(ndStart, ndEnd, NodesTraversingDirection.Backward);
              }
            }
            else
            {
              // Selected node and clicked node have different parents, hard case.
              // We need to find a common parent to determine if we need to walk down selecting, or walk up selecting.
              TreeNode ndStartP = ndStart;
              TreeNode ndEndP = ndEnd;
              int startDepth = Math.Min(ndStartP.Level, ndEndP.Level);

              // Bring lower node up to common depth
              while (ndStartP.Level > startDepth)
              {
                ndStartP = ndStartP.Parent;
              }

              // Bring lower node up to common depth
              while (ndEndP.Level > startDepth)
              {
                ndEndP = ndEndP.Parent;
              }

              // Walk up the tree until we find the common parent
              while (ndStartP.Parent != ndEndP.Parent)
              {
                ndStartP = ndStartP.Parent;
                ndEndP = ndEndP.Parent;
              }

              // Select the node
              if (ndStartP.Index < ndEndP.Index)
              {
                // If the selected node is beneath the clicked node walk down selecting each Visible node until we reach the end.
                MarkNodesRangeAsSelected(ndStart, ndEnd, NodesTraversingDirection.Forward);
              }
              else if (ndStartP.Index == ndEndP.Index)
              {
                if (ndStart.Level < ndEnd.Level)
                {
                  MarkNodesRangeAsSelected(ndStart, ndEnd, NodesTraversingDirection.Forward);
                }
                else
                {
                  MarkNodesRangeAsSelected(ndStart, ndEnd, NodesTraversingDirection.Backward);
                }
              }
              else
              {
                // If the selected node is above the clicked node walk up selecting each Visible node until we reach the end.
                MarkNodesRangeAsSelected(ndStart, ndEnd, NodesTraversingDirection.Backward);
              }
            }
          }
          else
          {
            // Just clicked a node, select it
            SelectSingleNode(node, SingleSelectionSource.SelectNode);
          }
        }
        else
        {
          SelectSingleNode(node, SingleSelectionSource.SelectNode);
        }

        OnAfterSelect(new TreeViewEventArgs(_selectedNode));
      }
      catch (Exception ex)
      {
        MySqlSourceTrace.WriteAppErrorToLog(ex);
      }
      finally
      {
        EndUpdate();
      }
    }

    /// <summary>
    /// Marks the given node as selected.
    /// </summary>
    /// <param name="node">A <see cref="MySqlListViewNode"/> object.</param>
    /// <param name="source">The method source that called this method.</param>
    private void SelectSingleNode(MySqlListViewNode node, SingleSelectionSource source)
    {
      if (node == null)
      {
        return;
      }

      ClearSelectedNodes();
      MarkNodeAsSelected(node, true);
      node.EnsureVisible();
      if (source == SingleSelectionSource.SelectNode)
      {
        return;
      }

      OnAfterSelect(new TreeViewEventArgs(node));
    }

    /// <summary>
    /// Sets the node height given a multiple number for the height.
    /// </summary>
    /// <param name="node">The tree node to have its height modified.</param>
    /// <param name="heightMultiplier">The multiple number for the height of tree nodes.</param>
    private void SetNodeHeight(TreeNode node, int heightMultiplier)
    {
      if (heightMultiplier <= 1)
      {
        return;
      }

      var tex = new TvItemEx
      {
        mask = TVIF_HANDLE | TVIF_INTEGRAL,
        hItem = node.Handle,
        iIntegral = heightMultiplier
      };

      IntPtr ptr = Marshal.AllocHGlobal(Marshal.SizeOf(tex));
      Marshal.StructureToPtr(tex, ptr, false);
      MiscUtilities.SendMessage(Handle, TVM_SETITEM, IntPtr.Zero, ptr);
      Marshal.FreeHGlobal(ptr);
    }

    /// <summary>
    /// Updates the extended styles of the tree view.
    /// </summary>
    private void UpdateExtendedStyles()
    {
      int style = 0;
      if (DoubleBuffered)
      {
        style |= TVS_EX_DOUBLEBUFFER;
      }

      if (style != 0)
      {
        MiscUtilities.SendMessage(Handle, TVM_SETEXTENDEDSTYLE, (IntPtr)TVS_EX_DOUBLEBUFFER, (IntPtr)style);
      }
    }
  }
}