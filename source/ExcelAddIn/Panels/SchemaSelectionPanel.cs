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
      Utilities.SetDoubleBuffered(lisDatabases);
    }

    public void SetConnection(MySqlWorkbenchConnection connection)
    {
      this.connection = connection;
      lblConnectionName.Text = connection.Name;
      lblUserIP.Text = String.Format("User: {0}, IP: {1}", connection.UserName, connection.Host);
      LoadSchemas();
    }

    private void lisDatabases_ItemSelectionChanged(object sender, ListViewItemSelectionChangedEventArgs e)
    {
      if (lisDatabases.SelectedItems.Count > 0 && !e.Item.Equals(lisDatabases.SelectedItems[0]))
        return;
      btnNext.Enabled = e.IsSelected;
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
      string databaseName = lisDatabases.SelectedItems[0].Text;
      (Parent as TaskPaneControl).OpenSchema(databaseName);
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

    private void LoadSchemas()
    {
      DataTable databases = Utilities.GetSchemaCollection(connection, "Databases", null);
      if (databases == null)
      {
        MessageBox.Show(Resources.UnableToLoadDatabases);
        return;
      }
      int systemCounter = 0;
      int usersCounter = 0;
      ListViewGroup lvg;
      foreach (DataRow row in databases.Rows)
      {
        string schemaName = row["DATABASE_NAME"].ToString();
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
        string[] tileItems = new string[] { schemaName, String.Format("CharSet: {0}", row["DEFAULT_CHARACTER_SET_NAME"].ToString()) };
        ListViewItem lvi = new ListViewItem(tileItems, 0, lvg);
        lvi.Name = schemaName;
        lvi.Font = new Font("Arial", 8, FontStyle.Regular);
        lisDatabases.Items.Add(lvi);
      }
      lisDatabases.Groups["grpUserSchemas"].Header = String.Format("Schemas ({0})", usersCounter);
      lisDatabases.Groups["grpSystemSchemas"].Header = String.Format("System Schemas ({0})", systemCounter);
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
