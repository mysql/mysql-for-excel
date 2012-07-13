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
      MySqlWorkbench.Connections.Clear();
      MySqlWorkbench.LoadData();

      foreach (TreeNode node in connectionList.Nodes)
        node.Nodes.Clear();

      foreach (MySqlWorkbenchConnection conn in MySqlWorkbench.Connections)
        AddConnectionToList(conn);
      if (connectionList.Nodes[0].GetNodeCount(true) > 0)
        connectionList.Nodes[0].Expand();
    }

    private void AddConnectionToList(MySqlWorkbenchConnection conn)
    {
      int nodeIdx = 1;
      string hostName = (conn.Host ?? string.Empty).Trim();
      if (conn.DriverType == MySqlWorkbenchConnectionType.Ssh)
      {
        string[] sshConnection = conn.HostIdentifier.Split('@');
        string dbHost = sshConnection[1].Split(':')[0].Trim();
        string sshHost = sshConnection[2].Split(':')[0].Trim();
        if (localValues.Contains(sshHost.ToLowerInvariant()) && localValues.Contains(dbHost.ToLowerInvariant()))
          nodeIdx = 0;
        hostName = dbHost + " (ssh)";
      }
      else if (localValues.Contains(hostName.ToLowerInvariant()))
        nodeIdx = 0;

      string s = String.Format("{0}|{1}", conn.Name, String.Format("User: {0}, Host: {1}", conn.UserName, hostName));
      TreeNode node = connectionList.AddNode(connectionList.Nodes[nodeIdx], s);
      node.ImageIndex = 0;
      node.Tag = conn;
    }

    private void newConnectionLabel_Click(object sender, EventArgs e)
    {
      NewConnectionDialog dlg = new NewConnectionDialog();
      DialogResult result = dlg.ShowDialog();
      if (result == DialogResult.Cancel) return;

      MySqlWorkbench.Connections.Add(dlg.NewConnection);
      MySqlWorkbench.Connections.Save();
      LoadConnections();
    }

    private void manageConnectionsLabel_Click(object sender, EventArgs e)
    {
      MySqlWorkbench.OpenManageConnectionsDialog();
    }

    private void connectionList_NodeMouseDoubleClick(object sender, TreeNodeMouseClickEventArgs e)
    {
      if (e.Node == null || e.Node.Level == 0) return;
      MySqlWorkbenchConnection c = connectionList.SelectedNode.Tag as MySqlWorkbenchConnection;
      (Parent as TaskPaneControl).OpenConnection(c);
    }

    private void connectionList_MouseClick(object sender, MouseEventArgs e)
    {
      if (e.Button == MouseButtons.Right)
      {
        ContextMenuStrip contextMenu = new ContextMenuStrip();

        ToolStripMenuItem refreshItem = new ToolStripMenuItem("Refresh");
        contextMenu.ShowImageMargin = false;
        contextMenu.ShowCheckMargin = false;


        refreshItem.Click += new EventHandler(refreshItem_Click);
        contextMenu.Items.Clear();
        contextMenu.Items.Add(refreshItem);

        contextMenu.Show(connectionList, this.connectionList.PointToClient(Cursor.Position));
      }
    }

    void refreshItem_Click(object sender, EventArgs e)
    {
      LoadConnections();
    }

  }
}
