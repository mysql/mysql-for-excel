// Copyright (c) 2014 - 2015, Oracle and/or its affiliates. All rights reserved.
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

using System.Drawing;
using System.Windows.Forms;
using MySQL.ForExcel.Controls;
using MySQL.Utility.Classes;
using MySQL.Utility.Classes.MySQLWorkbench;

namespace MySQL.ForExcel.Classes
{
  /// <summary>
  /// Represents a node of a <see cref="MySqlListView"/> .
  /// </summary>
  public class MySqlListViewNode : TreeNode
  {
    #region Fields

    /// <summary>
    ///Flag indicating whether the node is selected.
    /// </summary>
    private bool _isSelected;

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
    /// Initializes a new instance of the <see cref="MySqlListViewNode"/> class holding MySQL connection information.
    /// </summary>
    /// <param name="connection">The <see cref="MySqlWorkbenchConnection"/> associated to the node.</param>
    /// <param name="excludeFromMultiSelection">Flag indicating whether the tree node is skipped during a multiple selection.</param>
    public MySqlListViewNode(MySqlWorkbenchConnection connection, bool excludeFromMultiSelection = false)
      : this(connection.Name, string.Empty, MySqlNodeType.Connection, excludeFromMultiSelection)
    {
      string hostName = connection.GetHostNameForConnectionSubtitle();
      Subtitle = string.Format("User: {0}, Host: {1}:{2}", connection.UserName, hostName, connection.Port);
      WbConnection = connection;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="MySqlListViewNode"/> class holding database objects information.
    /// </summary>
    /// <param name="dbObject">The <see cref="DbObject"/> related to the node.</param>
    /// <param name="includeOnlyTablesAndViewsInMultiSelection">Flag indicating whether only tree nodes holding Tables and Views are included during a multiple selection.</param>
    public MySqlListViewNode(DbObject dbObject, bool includeOnlyTablesAndViewsInMultiSelection = true)
      : this(dbObject.Name, null, MySqlNodeType.DbObject, includeOnlyTablesAndViewsInMultiSelection && !(dbObject is DbTable) && !(dbObject is DbView))
    {
      DbObject = dbObject;
      var dbSchema = dbObject as DbSchema;
      if (dbSchema != null && dbSchema.DisplayCollation)
      {
        Subtitle = dbSchema.Collation;
      }
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="MySqlListViewNode"/> class intended to be a header node containing sub-nodes.
    /// </summary>
    /// <param name="headerTitle">The title text of the tree node.</param>
    /// <param name="type">The type of MySQL information related to the node.</param>
    /// <param name="excludeFromMultiSelection">Flag indicating whether the tree node is skipped during a multiple selection.</param>
    public MySqlListViewNode(string headerTitle, MySqlNodeType type, bool excludeFromMultiSelection = false)
      : this(headerTitle, null, type, excludeFromMultiSelection)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="MySqlListViewNode"/> class.
    /// </summary>
    /// <param name="title">The title text of the tree node.</param>
    /// <param name="subtitle">The sub-title text of the tree node.</param>
    /// <param name="type">The type of MySQL information related to the node.</param>
    /// <param name="excludeFromMultiSelection">Flag indicating whether the tree node is skipped during a multiple selection.</param>
    private MySqlListViewNode(string title, string subtitle, MySqlNodeType type, bool excludeFromMultiSelection)
      : base(title)
    {
      DbObject = null;
      ExcludeFromMultiSelection = excludeFromMultiSelection;
      _isSelected = false;
      WbConnection = null;
      Enable = true;
      Title = title;
      Subtitle = subtitle;
      Type = type;
    }

    /// <summary>
    /// Specifies identifiers to indicate the type of MySQL information related to the node.
    /// </summary>
    public enum MySqlNodeType
    {
      /// <summary>
      /// The node is related to connection information.
      /// </summary>
      Connection,

      /// <summary>
      /// The node is related to database object information.
      /// </summary>
      DbObject,

      /// <summary>
      /// The node is a header node and NOT related to any kind of information.
      /// </summary>
      Header
    }

    #region Properties

    /// <summary>
    /// Gets the <see cref="DbObject"/> related to the node.
    /// </summary>
    public DbObject DbObject { get; private set; }

    /// <summary>
    /// Gets or sets a value indicating whether the tree node is displayed as being enabled.
    /// </summary>
    public bool Enable { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether the tree node is skipped during a multiple selection.
    /// </summary>
    public bool ExcludeFromMultiSelection { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether the node is selected.
    /// </summary>
    public new bool IsSelected
    {
      get
      {
        return _isSelected;
      }

      set
      {
        _isSelected = value;
        if (Type == MySqlNodeType.DbObject && DbObject != null)
        {
          DbObject.Selected = _isSelected;
        }
      }
    }

    /// <summary>
    /// Gets or sets the subtitle text displayed in the tree node.
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
    /// Gets the type of MySQL information related to the node.
    /// </summary>
    public MySqlNodeType Type { get; private set; }

    /// <summary>
    /// Gets or sets a value indicating whether the sub-title text is truncated.
    /// </summary>
    public bool UpdateTruncatedSubtitle { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether the title text is truncated.
    /// </summary>
    public bool UpdateTruncatedTitle { get; set; }

    /// <summary>
    /// Gets the <see cref="MySqlWorkbenchConnection"/> associated to the node.
    /// </summary>
    public MySqlWorkbenchConnection WbConnection { get; private set; }

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
      if (!UpdateTruncatedSubtitle)
      {
        return _truncatedSubtitle;
      }

      if (maxWidth > 0)
      {
        _truncatedSubtitle = Subtitle.TruncateString(graphics, maxWidth, font);
      }

      UpdateTruncatedSubtitle = false;

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
      if (!UpdateTruncatedTitle)
      {
        return _truncatedTitle;
      }

      if (maxWidth > 0)
      {
        _truncatedTitle = Title.TruncateString(graphics, maxWidth, font);
      }

      UpdateTruncatedTitle = false;

      return _truncatedTitle;
    }
  }
}
