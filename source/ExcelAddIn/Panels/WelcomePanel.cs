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

namespace MySQL.ExcelAddIn
{
  public partial class WelcomePanel : UserControl
  {
    private Dictionary<string, MySQLConnectionData> connectionDataDictionary;

    public Dictionary<string, MySQLConnectionData> ConnectionDataDictionary { get { return connectionDataDictionary; } }
    private MySQLConnectionData selectedConnectionData
    {
      get
      {
        return (lisConnections.SelectedItems.Count > 0 && connectionDataDictionary != null && connectionDataDictionary.Count > 0 ? connectionDataDictionary[lisConnections.SelectedItems[0].Name] : null);
      }
    }
    public delegate void WelcomePanelLeavingHandler(object sender, WelcomePanelLeavingArgs args);
    public event WelcomePanelLeavingHandler WelcomePanelLeaving;

    public WelcomePanel()
    {
      InitializeComponent();
      Utilities.SetDoubleBuffered(lisConnections);
      loadWorkbenchConnections();
    }

    private void loadWorkbenchConnections()
    {
      var fileName = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "MySQL\\Workbench\\connections.xml");
      var exists = File.Exists(fileName);

      if (!exists)
      {
        MessageBox.Show(Properties.Resources.connectionsFileNotFound, "Error", MessageBoxButtons.OK);
        return;
      }

      connectionDataDictionary = new Dictionary<string, MySQLConnectionData>();

      try
      {
        var connectionsFromFile = XElement.Load(fileName);
        var connectionStructs = from c in connectionsFromFile.Descendants("value")
                                where c.HasAttributes && c.Attribute("struct-name") != null && c.Attribute("struct-name").Value == "db.mgmt.Connection"
                                select c;
        foreach (XElement connectionStruct in connectionStructs)
        {
          string stringGuid = connectionStruct.Attribute("id").Value;
          MySQLConnectionData connData = new MySQLConnectionData(new Guid(stringGuid));

          foreach (XElement innerElement in connectionStruct.Descendants())
          {
            if (!innerElement.HasAttributes)
              continue;
            switch (innerElement.Attribute("key").Value)
            {
              case "hostIdentifier":
                connData.HostIdentifier = innerElement.Value;
                break;
              case "hostName":
                connData.HostName = innerElement.Value;
                break;
              case "port":
                connData.Port = Convert.ToInt32(innerElement.Value);
                break;
              case "userName":
                connData.UserName = innerElement.Value;
                break;
              case "name":
                connData.Name = innerElement.Value;
                break;
            }
          }
          connectionDataDictionary.Add(connData.StringId, connData);
        }
        connectionDataDictionary.OrderBy(conn => conn.Value.Name);
      }
      catch (Exception ex)
      {
        MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
      }
    }

    private void fillConnectionsListView()
    {
      lisConnections.Items.Clear();

      foreach (var element in connectionDataDictionary)
      {
        var wbConn = element.Value;
        string[] tileItems = new string[] { wbConn.Name, wbConn.GluedConnection };
        ListViewItem lvi = new ListViewItem(tileItems, 0, lisConnections.Groups["grpLocalConnections"]);
        lvi.Name = wbConn.StringId;
        lvi.Font = new Font("Arial", 8, FontStyle.Regular);
        lisConnections.Items.Add(lvi);
      }
      lisConnections.Sort();
    }

    protected virtual void OnWelcomePanelLeaving(WelcomePanelLeavingArgs args)
    {
      if (WelcomePanelLeaving != null)
        WelcomePanelLeaving(this, args);
    }

    private void WelcomePanel_VisibleChanged(object sender, EventArgs e)
    {
      if (this.Visible)
        fillConnectionsListView();
    }

    private void lisConnections_ItemActivate(object sender, EventArgs e)
    {
      if (selectedConnectionData != null)
        OnWelcomePanelLeaving(new WelcomePanelLeavingArgs(selectedConnectionData));
    }

    private void connectionsContextMenu_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
    {
      lisConnections_ItemActivate(this, EventArgs.Empty);
    }

    private void connectionsContextMenu_Opening(object sender, CancelEventArgs e)
    {
      if (lisConnections.SelectedItems.Count == 0)
        e.Cancel = true;
    }

    private void infManageConnections_PictureClick(object sender, EventArgs e)
    {
      MessageBox.Show("Opening Workbench...");
    }

    private void infNewConnection_PictureClick(object sender, EventArgs e)
    {
      MessageBox.Show("New Connection...");
    }

  }

  public class WelcomePanelLeavingArgs : EventArgs
  {
    private MySQLConnectionData selectedConnectionData;

    public MySQLConnectionData SelectedConnectionData
    {
      get { return selectedConnectionData; }
    }

    public WelcomePanelLeavingArgs(MySQLConnectionData selectedConn)
    {
      selectedConnectionData = selectedConn;
    }
  }
}
