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
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.IO;
using System.Windows.Forms;
using MySQL.Utility;
using MySQL.ForExcel.Properties;

namespace MySQL.ForExcel
{
  public partial class WelcomePanel : AutoStyleableBasePanel
  {
    private static string[] localValues = new string[] { string.Empty, "127.0.0.1", "localhost", "local" };

    public WelcomePanel()
    {
      InitializeComponent();

      InheritFontToControlsExceptionList.Add("openConnectionLabel");
      InheritFontToControlsExceptionList.Add("newConnectionLabel");
      InheritFontToControlsExceptionList.Add("manageConnectionsLabel");

      DoubleBuffered = true;
      manageConnectionsLabel.Enabled = MySqlWorkbench.IsInstalled;
      LoadConnections();
    }

    private void LoadConnections()
    {
      string pathFile = string.Empty;
      //TODO: CHECK if we need to clear list
      MySqlWorkbench.Connections.Clear();
      
      MySqlWorkbenchConnectionCollection connections;

      connections = GetConnections();
      
      foreach (TreeNode node in connectionList.Nodes)
        node.Nodes.Clear();

      foreach (MySqlWorkbenchConnection conn in MySqlWorkbench.Connections)
        AddConnectionToList(conn);
      if (connectionList.Nodes[0].GetNodeCount(true) > 0)
        connectionList.Nodes[0].Expand();
      
    }


    private MySqlWorkbenchConnectionCollection GetConnections()
    {

      string pathFile = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + @"\Oracle\MySQL For Excel\connections.xml";


      /* check if mysql excel connections file not exists*/
      if (!File.Exists(pathFile))      
      {
        /*create new file and load WB Connections file */
        string pathWorkbench = String.Format(@"{0}\MySQL\Workbench\connections.xml", Environment.GetFolderPath(System.Environment.SpecialFolder.ApplicationData));        
        
        var newConnectionsFileClass = new MySQLForExcelConnectionsFile();
        newConnectionsFileClass.CreateXMLFile(File.Exists(pathWorkbench), pathWorkbench);
      
      }

       var connections = new MySqlWorkbenchConnectionCollection(@pathFile);

        MySqlWorkbench workbench = new MySqlWorkbench(@pathFile);

        workbench.LoadDataWithFile();

        return MySqlWorkbench.Connections;
    }


    private void AddConnectionToList(MySqlWorkbenchConnection conn)
    {
      int nodeIdx = 1;
      bool isSSH = false;

      string hostName = (conn.Host ?? string.Empty).Trim();
      if (conn.DriverType == MySqlWorkbenchConnectionType.Ssh)
        isSSH = true;

      if (isSSH)
      {
        string[] sshConnection = conn.HostIdentifier.Split('@');
        string dbHost = sshConnection[1].Split(':')[0].Trim();
        if (localValues.Contains(dbHost.ToLowerInvariant()))
          nodeIdx = 0;
        hostName = dbHost + " (SSH)";
      }
      else if (localValues.Contains(hostName.ToLowerInvariant()))
        nodeIdx = 0;

      string subtitle = String.Format("User: {0}, Host: {1}", conn.UserName, hostName);
      MyTreeNode node = connectionList.AddNode(connectionList.Nodes[nodeIdx], conn.Name, subtitle);
      node.ImageIndex = (isSSH ? 1 : 0);
      node.Enable = !isSSH;
      node.Tag = conn;
    }

    private void newConnectionLabel_Click(object sender, EventArgs e)
    {
      // if Workbench is running we can't allow adding new connections
      InfoDialog id = new InfoDialog(false, Resources.UnableToAddConnectionsWhenWBRunning, string.Empty);
      id.OperationSummarySubText = Resources.CloseWBAdviceToAdd;
      if (MySqlWorkbench.IsRunning)
      {
        id.ShowDialog();
        return;
      }

      NewConnectionDialog dlg = new NewConnectionDialog();
      DialogResult result = dlg.ShowDialog();
      if (result == DialogResult.Cancel) return;

      var mysqlforExcelConnectionsFile = new MySQLForExcelConnectionsFile();
      mysqlforExcelConnectionsFile.Save(dlg.NewConnection);
      
      LoadConnections();
    }

    private void manageConnectionsLabel_Click(object sender, EventArgs e)
    {
      MySqlWorkbench.OpenManageConnectionsDialog();
    }

    private void connectionList_NodeMouseDoubleClick(object sender, TreeNodeMouseClickEventArgs e)
    {
      if (e.Node == null || e.Node.Level == 0 || e.Node.ImageIndex > 0)
        return;
      MySqlWorkbenchConnection c = connectionList.SelectedNode.Tag as MySqlWorkbenchConnection;
      (Parent as TaskPaneControl).OpenConnection(c);
    }

    void refreshItem_Click(object sender, EventArgs e)
    {
      LoadConnections();
    }

    private void deleteToolStripMenuItem_Click(object sender, EventArgs e)
    {
      InfoDialog id = new InfoDialog(false, Resources.UnableToDeleteConnectionsWhenWBRunning, string.Empty);
      id.OperationSummarySubText = Resources.CloseWBAdviceToDelete;
      if (MySqlWorkbench.IsRunning) { id.ShowDialog(); return; }
      if (connectionList.SelectedNode.Name == "LocalConnectionsNode" || connectionList.SelectedNode.Text == "RemoteConnectionsNode" || connectionList.SelectedNode == null) return;
      MySqlWorkbenchConnectionCollection wbcollect = new MySqlWorkbenchConnectionCollection();
      WarningDialog wd = new WarningDialog("Confirm Delete", "Are you sure you want to delete the selected connection?");
      if (wd.ShowDialog() == DialogResult.No) return;
      MySqlWorkbenchConnection connectionToRemove = new MySqlWorkbenchConnection();
      foreach (MySqlWorkbenchConnection c in MySqlWorkbench.Connections)
        if (c.Name == connectionList.SelectedNode.Tag.ToString())
        {
          connectionToRemove = c;
          break;
        }
      MySqlWorkbench.Connections.Remove(connectionToRemove);
      LoadConnections();
    }

    private void contextMenuStrip_Opened(object sender, EventArgs e)
    {
      contextMenuStrip.Items["deleteToolStripMenuItem"].Visible = (connectionList.SelectedNode == null ||
        connectionList.SelectedNode.Name == "LocalConnectionsNode" || connectionList.SelectedNode.Name == "RemoteConnectionsNode" ||
        MySqlWorkbench.Connections.Count <= 0) ? false : true;
    }    
  }
}