// Copyright (c) 2012, 2016, Oracle and/or its affiliates. All rights reserved.
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
using System.ComponentModel;
using System.Linq;
using System.Windows.Forms;
using MySQL.ForExcel.Classes;
using MySQL.ForExcel.Controls;
using MySQL.ForExcel.Forms;
using MySQL.ForExcel.Properties;
using MySql.Utility.Classes;
using MySql.Utility.Classes.MySqlWorkbench;
using MySql.Utility.Forms;

namespace MySQL.ForExcel.Panels
{
  /// <summary>
  /// First panel shown to users within the Add-In's <see cref="ExcelAddInPane"/> where connections are managed.
  /// </summary>
  public sealed partial class WelcomePanel : AutoStyleableBasePanel
  {
    /// <summary>
    /// Initializes a new instance of the <see cref="WelcomePanel"/> class.
    /// </summary>
    public WelcomePanel()
    {

      InitializeComponent();

      InheritFontToControlsExceptionList.Add(OpenConnectionHotLabel.Name);
      InheritFontToControlsExceptionList.Add(NewConnectionHotLabel.Name);
      InheritFontToControlsExceptionList.Add(ManageConnectionsHotLabel.Name);

      DoubleBuffered = true;
      ManageConnectionsHotLabel.Enabled = MySqlWorkbench.AllowsExternalConnectionsManagement;
      ConnectionsList.AddHeaderNode("Local Connections");
      ConnectionsList.AddHeaderNode("Remote Connections");
      LoadConnections(false);
    }

    /// <summary>
    /// Searches for Workbench connections to load on the Connections list, if Workbench is not installed it loads connections created locally in MySQL for Excel.
    /// <param name="reloadConnections">Flag indicating if connections are to be re-read from the connections file.</param>
    /// </summary>
    public void LoadConnections(bool reloadConnections)
    {
      if (reloadConnections)
      {
        MySqlWorkbench.Connections.Load(true);
      }

      // Avoids flickering of connections list while adding the items to it.
      ConnectionsList.BeginUpdate();

      // Clear currently loaded connections
      foreach (TreeNode node in ConnectionsList.Nodes)
      {
        node.Nodes.Clear();
      }

      // Load connections just obtained from Workbench or locally created
      foreach (var conn in MySqlWorkbench.Connections.OrderBy(conn => conn.Name))
      {
        conn.AllowZeroDateTimeValues = true;
        AddConnectionToList(conn);
      }

      // Expand connection nodes
      ConnectionsList.ExpandAll();
      if (ConnectionsList.Nodes.Count > 0)
      {
        ConnectionsList.Nodes[0].EnsureVisible();
      }

      // Avoids flickering of connections list while adding the items to it.
      ConnectionsList.EndUpdate();
    }

    /// <summary>
    /// Event delegate method fired when the <see cref="AboutHotLabel"/> label is clicked.
    /// </summary>
    /// <param name="sender">Sender object.</param>
    /// <param name="e">Event arguments.</param>
    private void AboutHotLabel_Click(object sender, EventArgs e)
    {
      using (var aboutBox = new AboutBox())
      {
        aboutBox.ShowDialog();
      }
    }

    /// <summary>
    /// Adds a given connection to the corresponding connections group list depending on its connection type (local VS remote).
    /// </summary>
    /// <param name="conn">Object containing the data to open a connection, normally shared with Workbench.</param>
    private void AddConnectionToList(MySqlWorkbenchConnection conn)
    {
      if (conn == null || ConnectionsList.HeaderNodes.Count < 2)
      {
        return;
      }

      var isSsh = conn.ConnectionMethod == MySqlWorkbenchConnection.ConnectionMethodType.Ssh;
      var headerNode = ConnectionsList.HeaderNodes[conn.IsLocalConnection ? 0 : 1];
      var node = ConnectionsList.AddConnectionNode(headerNode, conn);
      node.Enable = !isSsh;
      switch (conn.ConnectionMethod)
      {
        case MySqlWorkbenchConnection.ConnectionMethodType.Tcp:
        case MySqlWorkbenchConnection.ConnectionMethodType.LocalUnixSocketOrWindowsPipe:
          node.ImageIndex = node.Enable ? 0 : 1;
          break;

        case MySqlWorkbenchConnection.ConnectionMethodType.FabricManaged:
          node.ImageIndex = 4;
          break;

        case MySqlWorkbenchConnection.ConnectionMethodType.Ssh:
          node.ImageIndex = node.Enable ? 2 : 3;
          break;
      }
    }

    /// <summary>
    /// Event delegate method fired when a node within the <see cref="ConnectionsList"/> tree view is double-clicked.
    /// </summary>
    /// <param name="sender">Sender object.</param>
    /// <param name="e">Event arguments.</param>
    private void ConnectionsList_NodeMouseDoubleClick(object sender, TreeNodeMouseClickEventArgs e)
    {
      var selectedNode = ConnectionsList.SelectedNode;
      if (selectedNode == null || selectedNode.Type == MySqlListViewNode.MySqlNodeType.Header || selectedNode.WbConnection == null)
      {
        return;
      }

      var excelAddInPane = Parent as ExcelAddInPane;
      if (excelAddInPane != null)
      {
        excelAddInPane.OpenConnection(selectedNode.WbConnection, true);
      }
    }

    /// <summary>
    /// Event delegate method fired when the <see cref="ConnectionsContextMenuStrip"/> context-menu strip is opening.
    /// </summary>
    /// <param name="sender">Sender object.</param>
    /// <param name="e">Event arguments.</param>
    private void ConnectionsContextMenuStrip_Opening(object sender, CancelEventArgs e)
    {
      DeleteToolStripMenuItem.Visible = MySqlWorkbench.Connections.Count > 0 && ConnectionsList.SelectedNode != null && ConnectionsList.SelectedNode.Level != 0;
    }

    /// <summary>
    /// Event delegate method fired when the <see cref="DeleteToolStripMenuItem"/> context-menu item is clicked.
    /// </summary>
    /// <param name="sender">Sender object.</param>
    /// <param name="e">Event arguments.</param>
    private void DeleteToolStripMenuItem_Click(object sender, EventArgs e)
    {
      var selectedNode = ConnectionsList.SelectedNode;
      if (selectedNode == null || selectedNode.Type != MySqlListViewNode.MySqlNodeType.Connection || selectedNode.WbConnection == null)
      {
        return;
      }

      var dr = MiscUtilities.ShowCustomizedWarningDialog(Resources.DeleteConnectionWarningTitle, Resources.DeleteConnectionWarningDetail);
      if (dr == DialogResult.No)
      {
        return;
      }

      if (selectedNode.WbConnection != null && MySqlWorkbench.Connections.DeleteConnection(selectedNode.WbConnection.Id))
      {
        LoadConnections(false);
      }
    }

    /// <summary>
    /// Event delegate method fired when the <see cref="EditConnectionToolStripMenuItem"/> context-menu item is clicked.
    /// </summary>
    /// <param name="sender">Sender object.</param>
    /// <param name="e">Event arguments.</param>
    private void EditConnectionToolStripMenuItem_Click(object sender, EventArgs e)
    {
      var selectedNode = ConnectionsList.SelectedNode;
      if (selectedNode == null || selectedNode.Type != MySqlListViewNode.MySqlNodeType.Connection || selectedNode.WbConnection == null)
      {
        return;
      }

      var connectionToEdit = ConnectionsList.SelectedNode.WbConnection;
      bool editedConnection;
      using (var instanceConnectionDialog = new MySqlWorkbenchConnectionDialog(connectionToEdit, false))
      {
        editedConnection = instanceConnectionDialog.ShowIfWorkbenchNotRunning() == DialogResult.OK;
      }

      if (editedConnection)
      {
        LoadConnections(false);
      }
    }

    /// <summary>
    /// Event delegate method fired when the <see cref="ManageConnectionsHotLabel"/> label is clicked.
    /// </summary>
    /// <param name="sender">Sender object.</param>
    /// <param name="e">Event arguments.</param>
    private void ManageConnectionsHotLabel_Click(object sender, EventArgs e)
    {
      MySqlWorkbench.OpenManageConnectionsDialog();
    }

    /// <summary>
    /// Event delegate method fired when the <see cref="NewConnectionHotLabel"/> label is clicked.
    /// </summary>
    /// <param name="sender">Sender object.</param>
    /// <param name="e">Event arguments.</param>
    private void NewConnectionHotLabel_Click(object sender, EventArgs e)
    {
      if (MySqlWorkbench.IsRunning)
      {
        // If Workbench is running we can't allow adding new connections
        InfoDialog.ShowDialog(InfoDialogProperties.GetErrorDialogProperties(Resources.OperationErrorTitle, Resources.UnableToAddConnectionsWhenWBRunning, Resources.CloseWBAdviceToAdd));
        return;
      }

      using (var newConnectionDialog = new MySqlWorkbenchConnectionDialog(null, false))
      {
        var result = newConnectionDialog.ShowDialog();
        if (result == DialogResult.OK)
        {
          LoadConnections(false);
        }
      }
    }

    /// <summary>
    /// Event delegate method fired when the <see cref="RefreshToolStripMenuItem"/> context-menu item is clicked.
    /// </summary>
    /// <param name="sender">Sender object.</param>
    /// <param name="e">Event arguments.</param>
    private void RefreshItem_Click(object sender, EventArgs e)
    {
      LoadConnections(true);
    }
  }
}