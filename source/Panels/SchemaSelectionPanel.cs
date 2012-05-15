using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using MySql.Data.MySqlClient;
using MySQL.Utility;
using MySQL.ExcelAddIn.Properties;

namespace MySQL.ExcelAddIn
{
  public partial class SchemaSelectionPanel : UserControl
  {
    private MySqlWorkbenchConnection connection;

    public SchemaSelectionPanel()
    {
      InitializeComponent();
    }

    public void SetConnection(MySqlWorkbenchConnection connection)
    {
      this.connection = connection;
      lblConnectionName.Text = connection.Name;
      lblUserIP.Text = String.Format("User: {0}, IP: {1}", connection.UserName, connection.Host);
      LoadSchemas();
    }

    private void databaseList_AfterSelect(object sender, TreeViewEventArgs e)
    {
      btnNext.Enabled = e.Node != null && e.Node.Level > 0;
    }

    private void btnHelp_Click(object sender, EventArgs e)
    {
      MessageBox.Show("Showing Help...");
    }

    private void btnBack_Click(object sender, EventArgs e)
    {
      (Parent as TaskPaneControl).CloseConnection();
    }

    private void btnNext_Click(object sender, EventArgs e)
    {
      string databaseName = databaseList.SelectedNode.Tag as string;
      (Parent as TaskPaneControl).OpenSchema(databaseName);
    }

    private void databaseList_NodeMouseDoubleClick(object sender, TreeNodeMouseClickEventArgs e)
    {
      btnNext_Click(this, EventArgs.Empty);
    }

    private void LoadSchemas()
    {
      foreach (TreeNode node in databaseList.Nodes)
        node.Nodes.Clear();
      DataTable databases = Utilities.GetSchemaCollection(connection, "Databases", null);
      if (databases == null)
      {
        MessageBox.Show(Resources.UnableToLoadDatabases);
        return;
      }

      foreach (DataRow row in databases.Rows)
      {
        string schemaName = row["DATABASE_NAME"].ToString();
        string lcSchemaName = schemaName.ToLowerInvariant();
        int index = (lcSchemaName == "mysql" || lcSchemaName == "information_schema") ? 1 : 0;

        string text = String.Format("{0}|{1}", schemaName,
          String.Format("CharSet: {0}", row["DEFAULT_CHARACTER_SET_NAME"].ToString()));
        TreeNode node = databaseList.AddNode(databaseList.Nodes[index], text);
        node.Tag = schemaName;
        node.ImageIndex = 0;
      }
    }

    private void createNewSchema_Click(object sender, EventArgs e)
    {
      NewSchemaDialog dlg = new NewSchemaDialog();
      if (dlg.ShowDialog() == DialogResult.Cancel) return;

      string connectionString = Utilities.GetConnectionString(connection);
      string sql = String.Format("CREATE DATABASE `{0}`", dlg.SchemaName);
      try
      {
        MySqlHelper.ExecuteNonQuery(connectionString, sql);
      }
      catch (Exception ex)
      {
        string msg = String.Format(Resources.ErrorCreatingNewSchema, ex.Message);
        MessageBox.Show(msg, Resources.Error, MessageBoxButtons.OK, MessageBoxIcon.Error);
        return;
      }
      LoadSchemas();
    }

  }
}
