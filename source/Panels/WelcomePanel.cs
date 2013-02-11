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
  using System.Collections.Generic;
  using System.ComponentModel;
  using System.Drawing;
  using System.Data;
  using System.Linq;
  using System.Text;
  using System.IO;
  using System.Windows.Forms;
  using MySQL.ForExcel.Properties;
  using MySQL.Utility;

  /// <summary>
  /// First panel shown to users within the Add-In's <seealso cref="TaskPaneControl"/>.
  /// </summary>
  public partial class WelcomePanel : AutoStyleableBasePanel
  {
    /// <summary>
    /// String array containing valid values for localhost
    /// </summary>
    private static string[] _localValues = new string[] { string.Empty, "127.0.0.1", "localhost", "local" };

    /// <summary>
    /// Initializes a new instance of the <see cref="WelcomePanel"/> class.
    /// </summary>
    public WelcomePanel()
    {
      InitializeComponent();

      InheritFontToControlsExceptionList.Add("openConnectionLabel");
      InheritFontToControlsExceptionList.Add("newConnectionLabel");
      InheritFontToControlsExceptionList.Add("manageConnectionsLabel");

      DoubleBuffered = true;
      manageConnectionsLabel.Enabled = MySqlWorkbench.AllowsExternalConnectionsManagement;
      LoadConnections();
    }

    /// <summary>
    /// Searches for Workbench connections to load on the Connections list, if Workbench is not installed it loads connections created locally in MySQL for Excel.
    /// </summary>
    private void LoadConnections()
    {
      List<MySqlWorkbenchConnection> connections = MySQLForExcelConnectionsHelper.GetConnections();

      if (connections == null)
      {
        return;
      }

      //// Clear currently loaded connections
      foreach (TreeNode node in connectionList.Nodes)
      {
        node.Nodes.Clear();
      }

      //// Load connections just obtained from Workbench or locally created
      foreach (MySqlWorkbenchConnection conn in connections)
      {
        AddConnectionToList(conn);
      }

      //// Expand each Connections group node if it contains connections
      foreach (TreeNode groupNode in connectionList.Nodes)
      {
        if (groupNode.GetNodeCount(true) > 0)
        {
          groupNode.Expand();
        }
      }
    }

    /// <summary>
    /// Adds a given connection to the corresponding connections group list depending on its connection type (local VS remote).
    /// </summary>
    /// <param name="conn">Object containing the data to open a connection, normally shared with Workbench.</param>
    private void AddConnectionToList(MySqlWorkbenchConnection conn)
    {
      int nodeIdx = 1;
      bool isSSH = conn.DriverType == MySqlWorkbenchConnectionType.Ssh;
      string hostName = (conn.Host ?? string.Empty).Trim();

      if (isSSH)
      {
        string[] sshConnection = conn.HostIdentifier.Split('@');
        string dbHost = sshConnection[1].Split(':')[0].Trim();
        if (_localValues.Contains(dbHost.ToLowerInvariant()))
        {
          nodeIdx = 0;
        }

        hostName = dbHost + " (SSH)";
      }
      else if (_localValues.Contains(hostName.ToLowerInvariant()))
      {
        nodeIdx = 0;
      }

      string subtitle = string.Format("User: {0}, Host: {1}", conn.UserName, hostName);
      MyTreeNode node = connectionList.AddNode(connectionList.Nodes[nodeIdx], conn.Name, subtitle);
      node.ImageIndex = isSSH ? 1 : 0;
      node.Enable = !isSSH;
      node.Tag = conn;
    }

    /// <summary>
    /// Event delegate method fired when the <see cref="newConnectionLabel"/> label is clicked.
    /// </summary>
    /// <param name="sender">Sender object.</param>
    /// <param name="e">Event arguments.</param>
    private void newConnectionLabel_Click(object sender, EventArgs e)
    {
      if (MySqlWorkbench.IsRunning)
      {
        //// If Workbench is running we can't allow adding new connections
        InfoDialog id = new InfoDialog(false, Resources.UnableToAddConnectionsWhenWBRunning, string.Empty);
        id.OperationSummarySubText = Resources.CloseWBAdviceToAdd;
        id.ShowDialog();
        return;
      }

      NewConnectionDialog dlg = new NewConnectionDialog();
      DialogResult result = dlg.ShowDialog();
      if (result == DialogResult.Cancel)
      {
        return;
      }

      MySQLForExcelConnectionsHelper.SaveConnection(dlg.NewConnection);
      LoadConnections();
    }

    /// <summary>
    /// Event delegate method fired when the <see cref="manageConnectionsLabel"/> label is clicked.
    /// </summary>
    /// <param name="sender">Sender object.</param>
    /// <param name="e">Event arguments.</param>
    private void manageConnectionsLabel_Click(object sender, EventArgs e)
    {
      MySqlWorkbench.OpenManageConnectionsDialog();
    }

    /// <summary>
    /// Event delegate method fired when the <see cref="aboutLabel"/> label is clicked.
    /// </summary>
    /// <param name="sender">Sender object.</param>
    /// <param name="e">Event arguments.</param>
    private void aboutLabel_Click(object sender, EventArgs e)
    {
      AboutBox aboutBox = new AboutBox();
      aboutBox.ShowDialog();
    }

    /// <summary>
    /// Event delegate method fired when a node within the <see cref="connectionList"/> tree view is double-clicked.
    /// </summary>
    /// <param name="sender">Sender object.</param>
    /// <param name="e">Event arguments.</param>
    private void connectionList_NodeMouseDoubleClick(object sender, TreeNodeMouseClickEventArgs e)
    {
      if (e.Node == null || e.Node.Level == 0 || e.Node.ImageIndex > 0)
      {
        return;
      }

      MySqlWorkbenchConnection c = connectionList.SelectedNode.Tag as MySqlWorkbenchConnection;
      (Parent as TaskPaneControl).OpenConnection(c);
    }

    /// <summary>
    /// Event delegate method fired when the <see cref="refreshToolStripMenuItem"/> context-menu item is clicked.
    /// </summary>
    /// <param name="sender">Sender object.</param>
    /// <param name="e">Event arguments.</param>
    private void refreshItem_Click(object sender, EventArgs e)
    {
      LoadConnections();
    }

    /// <summary>
    /// Event delegate method fired when the <see cref="deleteToolStripMenuItem"/> context-menu item is clicked.
    /// </summary>
    /// <param name="sender">Sender object.</param>
    /// <param name="e">Event arguments.</param>
    private void deleteToolStripMenuItem_Click(object sender, EventArgs e)
    {
      if (connectionList.SelectedNode == null || connectionList.SelectedNode.Level == 0)
      {
        return;
      }

      WarningDialog warningDlg = new WarningDialog(Resources.DeleteConnectionWarningTitle, Resources.DeleteConnectionWarningDetail);
      if (warningDlg.ShowDialog() == DialogResult.No)
      {
        return;
      }

      MySqlWorkbenchConnection connectionToRemove = connectionList.SelectedNode.Tag as MySqlWorkbenchConnection;
      MySQLForExcelConnectionsHelper.RemoveConnection(connectionToRemove.Id);
      LoadConnections();
    }

    /// <summary>
    /// Event delegate method fired when the <see cref="contextMenuStrip"/> context-menu strip is opening.
    /// </summary>
    /// <param name="sender">Sender object.</param>
    /// <param name="e">Event arguments.</param>
    private void contextMenuStrip_Opening(object sender, CancelEventArgs e)
    {
      deleteToolStripMenuItem.Visible = MySqlWorkbench.Connections.Count <= 0 || connectionList.SelectedNode == null || connectionList.SelectedNode.Level == 0 ? false : true;
    }
  }
}