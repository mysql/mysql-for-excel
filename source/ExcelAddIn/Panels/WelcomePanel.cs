using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Xml.Linq;
using System.Text;
using System.IO;
using System.Windows.Forms;
using MySQL.Utility;

namespace MySQL.ExcelAddIn
{
  public partial class WelcomePanel : UserControl
  {
    public WelcomePanel()
    {
      InitializeComponent();
      DoubleBuffered = true;
      manageConnectionsLabel.Enabled = MySqlWorkbench.IsInstalled;
      Utilities.SetDoubleBuffered(connectionList);
      LoadConnections();
    }

    private void LoadConnections()
    {
      connectionList.Clear();
      foreach (MySqlWorkbenchConnection conn in MySqlWorkbench.Connections)
      {
        AddConnectionToList(conn);
      }
    }

    private void AddConnectionToList(MySqlWorkbenchConnection conn)
    {
      string[] items = new string[2];
      items[0] = conn.Name;
      items[1] = String.Format("User: {0}, IP: {1}", conn.UserName, conn.Host);
      ListViewItem lvi = new ListViewItem(items, 0, connectionList.Groups["grpLocalConnection"]);
      lvi.Tag = conn;
      connectionList.Items.Add(lvi);
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
      MySqlWorkbench.LaunchConfigure(null);
    }

    private void connectionList_ItemActivate(object sender, EventArgs e)
    {
      MySqlWorkbenchConnection c = connectionList.SelectedItems[0].Tag as MySqlWorkbenchConnection;
      (Parent as TaskPaneControl).OpenConnection(c);
    }
  }
}
