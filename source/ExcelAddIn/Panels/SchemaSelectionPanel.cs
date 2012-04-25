using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

namespace MySQL.ExcelAddIn
{
  public partial class SchemaSelectionPanel : UserControl
  {
    private string selectedDatabaseName
    {
      get
      {
        return (lisDatabases.SelectedItems.Count > 0 ? lisDatabases.SelectedItems[0].Name : String.Empty);
      }
    }

    public MySQLSchemaInfo SchemaInfo { set; private get; }

    public delegate void SchemaSelectionPanelLeavingHandler(object sender, SchemaSelectionPanelLeavingArgs args);
    public event SchemaSelectionPanelLeavingHandler SchemaSelectionPanelLeaving;

    public SchemaSelectionPanel()
    {
      InitializeComponent();
      Utilities.SetDoubleBuffered(lisDatabases);
    }

    private void resetSchemaSelectionPanel()
    {
      lisDatabases.Items.Clear();
      lblConnectionName.Text = "Connection Name";
      lblUserIP.Text = "User: ??, IP: ??";

      int systemCounter = 0;
      int usersCounter = 0;
      ListViewGroup lvg;
      foreach (DataRow schemaRow in SchemaInfo.SchemasTable.Rows)
      {
        string schemaName = schemaRow["SCHEMA_NAME"].ToString();
        string lcSchemaName = schemaName.ToLowerInvariant();
        if (lcSchemaName == "mysql" || lcSchemaName == "information_schema")
        {
          lvg = lisDatabases.Groups["grpSystemSchemas"];
          systemCounter++;
        }
        else
        {
          lvg = lisDatabases.Groups["grpUserSchemas"];
          usersCounter++;
        }
        string[] tileItems = new string[] { schemaName, String.Format("CharSet: {0}", schemaRow["DEFAULT_CHARACTER_SET_NAME"].ToString()) };
        ListViewItem lvi = new ListViewItem(tileItems, 0, lvg);
        lvi.Name = schemaName;
        lvi.Font = new Font("Arial", 8, FontStyle.Regular);
        lisDatabases.Items.Add(lvi);
      }
      lisDatabases.Groups["grpUserSchemas"].Header = String.Format("Schemas ({0})", usersCounter);
      lisDatabases.Groups["grpSystemSchemas"].Header = String.Format("System Schemas ({0})", systemCounter);
      if (SchemaInfo.ConnectionData != null)
      {
        lblConnectionName.Text = SchemaInfo.ConnectionData.Name;
        lblUserIP.Text = SchemaInfo.ConnectionData.GluedConnection;
      }
    }

    protected virtual void OnSchemaSelectionPanelLeaving(SchemaSelectionPanelLeavingArgs args)
    {
      if (SchemaSelectionPanelLeaving != null)
        SchemaSelectionPanelLeaving(this, args);
      lisDatabases.SelectedItems.Clear();
    }

    private void SchemaSelectionPanel_VisibleChanged(object sender, EventArgs e)
    {
      if (this.Visible)
      {
        btnNext.Enabled = false;
        resetSchemaSelectionPanel();
      }
    }

    private void lisDatabases_ItemSelectionChanged(object sender, ListViewItemSelectionChangedEventArgs e)
    {
      if (lisDatabases.SelectedItems.Count > 0 && !e.Item.Equals(lisDatabases.SelectedItems[0]))
        return;
      btnNext.Enabled = e.IsSelected;
    }

    private void infNewSchema_PictureClick(object sender, EventArgs e)
    {
      MessageBox.Show("Creating New Schema...");
    }

    private void btnHelp_Click(object sender, EventArgs e)
    {
      MessageBox.Show("Showing Help...");
    }

    private void btnBack_Click(object sender, EventArgs e)
    {
      OnSchemaSelectionPanelLeaving(new SchemaSelectionPanelLeavingArgs(selectedDatabaseName, SchemaSelectionPanelLeavingAction.Back));
    }

    private void btnNext_Click(object sender, EventArgs e)
    {
      if (selectedDatabaseName != String.Empty)
        OnSchemaSelectionPanelLeaving(new SchemaSelectionPanelLeavingArgs(selectedDatabaseName, SchemaSelectionPanelLeavingAction.Next));
    }

    private void lisDatabases_ItemActivate(object sender, EventArgs e)
    {
      btnNext_Click(this, EventArgs.Empty);
    }

    private void schemasContextMenu_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
    {
      btnNext_Click(this, EventArgs.Empty);
    }

    private void schemasContextMenu_Opening(object sender, CancelEventArgs e)
    {
      if (lisDatabases.SelectedItems.Count == 0)
        e.Cancel = true;
    }
  }

  public enum SchemaSelectionPanelLeavingAction { Back, Next };

  public class SchemaSelectionPanelLeavingArgs : EventArgs
  {
    private string selectedSchemaName;
    private SchemaSelectionPanelLeavingAction selectedAction;

    public string SelectedSchemaName
    {
      get { return selectedSchemaName; }
    }
    public SchemaSelectionPanelLeavingAction SelectedAction
    {
      get { return selectedAction; }
    }

    public SchemaSelectionPanelLeavingArgs(string selSchema, SchemaSelectionPanelLeavingAction selAction)
    {
      selectedSchemaName = selSchema;
      selectedAction = selAction;
    }
  }

}
