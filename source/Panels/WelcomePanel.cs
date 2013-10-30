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

using System;
using System.ComponentModel;
using System.Linq;
using System.Windows.Forms;
using MySQL.ForExcel.Classes;
using MySQL.ForExcel.Controls;
using MySQL.ForExcel.Forms;
using MySQL.ForExcel.Properties;
using MySQL.Utility.Classes.MySQLWorkbench;
using MySQL.Utility.Forms;

namespace MySQL.ForExcel.Panels
{
  /// <summary>
  /// First panel shown to users within the Add-In's <see cref="ExcelAddInPane"/> where connections are managed.
  /// </summary>
  public sealed partial class WelcomePanel : AutoStyleableBasePanel
  {
    /// <summary>
    /// String array containing valid values for localhost
    /// </summary>
    private static string[] _localHostValues;

    /// <summary>
    /// Initializes a new instance of the <see cref="WelcomePanel"/> class.
    /// </summary>
    public WelcomePanel()
    {
      _localHostValues = new[] { string.Empty, "127.0.0.1", "localhost", "local" };
      InitializeComponent();

      InheritFontToControlsExceptionList.Add(OpenConnectionHotLabel.Name);
      InheritFontToControlsExceptionList.Add(NewConnectionHotLabel.Name);
      InheritFontToControlsExceptionList.Add(ManageConnectionsHotLabel.Name);

      DoubleBuffered = true;
      ManageConnectionsHotLabel.Enabled = MySqlWorkbench.AllowsExternalConnectionsManagement;
      LoadConnections(false);
    }

    /// <summary>
    /// Event delegate method fired when the <see cref="AboutHotLabel"/> label is clicked.
    /// </summary>
    /// <param name="sender">Sender object.</param>
    /// <param name="e">Event arguments.</param>
    private void AboutHotLabel_Click(object sender, EventArgs e)
    {
      AboutBox aboutBox = new AboutBox();
      aboutBox.ShowDialog();
    }

    /// <summary>
    /// Adds a given connection to the corresponding connections group list depending on its connection type (local VS remote).
    /// </summary>
    /// <param name="conn">Object containing the data to open a connection, normally shared with Workbench.</param>
    private void AddConnectionToList(MySqlWorkbenchConnection conn)
    {
      int nodeIdx = 1;
      bool isSsh = conn.DriverType == MySqlWorkbenchConnectionType.Ssh;
      string hostName = (conn.Host ?? string.Empty).Trim();

      if (isSsh)
      {
        string[] sshConnection = conn.HostIdentifier.Split('@');
        string dbHost = sshConnection[1].Split(':')[0].Trim();
        if (_localHostValues.Contains(dbHost.ToLowerInvariant()))
        {
          nodeIdx = 0;
        }

        hostName = dbHost + " (SSH)";
      }
      else if (_localHostValues.Contains(hostName.ToLowerInvariant()))
      {
        nodeIdx = 0;
      }

      string subtitle = string.Format("User: {0}, Host: {1}", conn.UserName, hostName);
      MyTreeNode node = ConnectionsList.AddNode(ConnectionsList.Nodes[nodeIdx], conn.Name, subtitle);
      node.ImageIndex = isSsh ? 1 : 0;
      node.Enable = !isSsh;
      node.Tag = conn;
    }

    /// <summary>
    /// Event delegate method fired when a node within the <see cref="ConnectionsList"/> tree view is double-clicked.
    /// </summary>
    /// <param name="sender">Sender object.</param>
    /// <param name="e">Event arguments.</param>
    private void ConnectionsList_NodeMouseDoubleClick(object sender, TreeNodeMouseClickEventArgs e)
    {
      if (e.Node == null || e.Node.Level == 0 || e.Node.ImageIndex > 0)
      {
        return;
      }

      MySqlWorkbenchConnection c = ConnectionsList.SelectedNode.Tag as MySqlWorkbenchConnection;
      var excelAddInPane = Parent as ExcelAddInPane;
      if (excelAddInPane != null)
      {
        excelAddInPane.OpenConnection(c);
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
      if (ConnectionsList.SelectedNode == null || ConnectionsList.SelectedNode.Level == 0)
      {
        return;
      }

      DialogResult dr = MiscUtilities.ShowCustomizedWarningDialog(Resources.DeleteConnectionWarningTitle, Resources.DeleteConnectionWarningDetail);
      if (dr == DialogResult.No)
      {
        return;
      }

      MySqlWorkbenchConnection connectionToRemove = ConnectionsList.SelectedNode.Tag as MySqlWorkbenchConnection;
      if (connectionToRemove != null && MySqlWorkbench.Connections.DeleteConnection(connectionToRemove.Id))
      {
        LoadConnections(false);
      }
    }

    /// <summary>
    /// Searches for Workbench connections to load on the Connections list, if Workbench is not installed it loads connections created locally in MySQL for Excel.
    /// <param name="reloadConnections">Flag indicating if connections are to be re-read from the connections file.</param>
    /// </summary>
    private void LoadConnections(bool reloadConnections)
    {
      if (reloadConnections)
      {
        MySqlWorkbench.Connections.Load();
      }

      // Avoids flickering of connections list while adding the items to it.
      ConnectionsList.BeginUpdate();

      // Clear currently loaded connections
      foreach (TreeNode node in ConnectionsList.Nodes)
      {
        node.Nodes.Clear();
      }

      // Load connections just obtained from Workbench or locally created
      foreach (MySqlWorkbenchConnection conn in MySqlWorkbench.Connections.OrderBy(conn => conn.Name))
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
        InfoDialog.ShowErrorDialog(Resources.OperationErrorTitle, Resources.UnableToAddConnectionsWhenWBRunning, Resources.CloseWBAdviceToAdd);
        return;
      }

      using (MySqlWorkbenchConnectionDialog newConnectionDialog = new MySqlWorkbenchConnectionDialog(null))
      {
        DialogResult result = newConnectionDialog.ShowDialog();
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