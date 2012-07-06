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
  public partial class WelcomePanel : UserControl
  {
    public WelcomePanel()
    {
      InitializeComponent();
      DoubleBuffered = true;
      manageConnectionsLabel.Enabled = MySqlWorkbench.IsInstalled;
      LoadConnections();
    }

    private void LoadConnections()
    {
      foreach (TreeNode node in connectionList.Nodes)
        node.Nodes.Clear();
      foreach (MySqlWorkbenchConnection conn in MySqlWorkbench.Connections)
        AddConnectionToList(conn);
      if (connectionList.Nodes[0].GetNodeCount(true) > 0)
        connectionList.Nodes[0].Expand();
    }

    private void AddConnectionToList(MySqlWorkbenchConnection conn)
    {
      int nodeIdx = (conn.Host == "127.0.0.1" || conn.Host.ToLowerInvariant() == "localhost" ? 0 : 1);
      string s = String.Format("{0}|{1}", conn.Name, String.Format("User: {0}, IP: {1}", conn.UserName, conn.Host)); ;
      TreeNode node = connectionList.AddNode(connectionList.Nodes[nodeIdx], s);
      node.ImageIndex = 0;
      node.Tag = conn;
    }

    private void newConnectionLabel_Click(object sender, EventArgs e)
    {
      NewConnectionDialog dlg = new NewConnectionDialog();
      DialogResult result = dlg.ShowDialog();
      if (result == DialogResult.Cancel) return;

      // add it to our connection list
      AddConnectionToList(dlg.NewConnection);

      // now add it to the workbench connection list
      MySqlWorkbench.Connections.Add(dlg.NewConnection);
      MySqlWorkbench.Connections.Save();
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
  }
}
