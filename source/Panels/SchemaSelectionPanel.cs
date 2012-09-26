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
using System.Windows.Forms;
using MySql.Data.MySqlClient;
using MySQL.Utility;
using MySQL.ForExcel.Properties;

namespace MySQL.ForExcel
{
  public partial class SchemaSelectionPanel : AutoStyleableBasePanel
  {
    private MySqlWorkbenchConnection connection;
    private string filter;

    public SchemaSelectionPanel()
    {
      InitializeComponent();

      InheritFontToControlsExceptionList.Add("selectSchemaLabel");
      InheritFontToControlsExceptionList.Add("createNewSchemaLabel");

      databaseList.AddNode(null, "Schemas");
      databaseList.AddNode(null, "System Schemas");
    }

    public bool SetConnection(MySqlWorkbenchConnection connection)
    {
      bool schemasLoaded = false;
      this.connection = connection;
      lblConnectionName.Text = connection.Name;
      lblUserIP.Text = String.Format("User: {0}, IP: {1}", connection.UserName, connection.Host);
      schemasLoaded = LoadSchemas();
      if (schemasLoaded)
        databaseList_AfterSelect(null, null);
      return schemasLoaded;
    }


    private void refreshSchemasToolStripMenuItem_Click(object sender, EventArgs e)
    {
      if (LoadSchemas())
        databaseList_AfterSelect(null, null);
    }

    private void databaseList_AfterSelect(object sender, TreeViewEventArgs e)
    {
      btnNext.Enabled = e != null && e.Node != null && e.Node.Level > 0;
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
      try
      {
        string databaseName = databaseList.SelectedNode.Tag as string;
        (Parent as TaskPaneControl).OpenSchema(databaseName);
      }
      catch (Exception ex)
      {
        InfoDialog errorDialog = new InfoDialog(false, Resources.SchemaOpeningErrorTitle, ex.Message);
        errorDialog.WordWrapDetails = true;
        errorDialog.ShowDialog();
        MiscUtilities.WriteAppErrorToLog(ex);
      }
    }

    private void databaseList_NodeMouseDoubleClick(object sender, TreeNodeMouseClickEventArgs e)
    {
      if (e.Node.Level > 0)
        btnNext_Click(this, EventArgs.Empty);
    }

    private bool LoadSchemas()
    {
      try
      {
        foreach (TreeNode node in databaseList.Nodes)
          node.Nodes.Clear();
        DataTable databases = MySQLDataUtilities.GetSchemaCollection(connection, "Databases", null);
        foreach (DataRow row in databases.Rows)
        {
          string schemaName = row["DATABASE_NAME"].ToString();

          // if the user has specified a filter then check it        
          if (!String.IsNullOrEmpty(filter) && !schemaName.ToUpper().Contains(filter)) continue;

          string lcSchemaName = schemaName.ToLowerInvariant();
          int index = (lcSchemaName == "mysql" || lcSchemaName == "information_schema") ? 1 : 0;

          string text = schemaName;
          TreeNode node = databaseList.AddNode(databaseList.Nodes[index], text);
          node.Tag = schemaName;
          node.ImageIndex = 0;
        }
        if (databaseList.Nodes[0].GetNodeCount(true) > 0)
          databaseList.Nodes[0].Expand();
        return true;
      }
      catch (Exception ex)
      {
        InfoDialog errorDialog = new InfoDialog(false, Properties.Resources.SchemasLoadingErrorTitle, ex.Message);
        errorDialog.WordWrapDetails = true;
        errorDialog.ShowDialog();
        MiscUtilities.WriteAppErrorToLog(ex);
        return false;
      }
    }

    private void createNewSchema_Click(object sender, EventArgs e)
    {
      NewSchemaDialog dlg = new NewSchemaDialog();
      if (dlg.ShowDialog() == DialogResult.Cancel) return;

      string connectionString = MySQLDataUtilities.GetConnectionString(connection);
      string sql = String.Format("CREATE DATABASE `{0}`", dlg.SchemaName);
      try
      {
        MySqlHelper.ExecuteNonQuery(connectionString, sql);
      }
      catch (Exception ex)
      {
        InfoDialog errorDialog = new InfoDialog(false, Resources.ErrorCreatingNewSchema, ex.Message);
        errorDialog.WordWrapDetails = true;
        errorDialog.ShowDialog();
        MiscUtilities.WriteAppErrorToLog(ex);
        return;
      }
      LoadSchemas();
    }

    private void schemaFilter_KeyDown(object sender, KeyEventArgs e)
    {
      if (e.KeyCode == Keys.Enter)
      {
        filter = schemaFilter.Text.Trim().ToUpper();
        LoadSchemas();
      }
    }

    private void label_Paint(object sender, PaintEventArgs e)
    {
      Label label = sender as Label;
      // Set a rectangle size with same width and larger height than label's
      SizeF layoutSize = new SizeF(label.Width, label.Height + 1);
      // Get the actual size of rectangle needed for all of text.
      SizeF fullSize = e.Graphics.MeasureString(label.Text, label.Font);
      // Set a tooltip if not all text fits in label's size.
      if (fullSize.Width > label.Width || fullSize.Height > label.Height)
      {
        labelsToolTip.SetToolTip(label, label.Text);
      }
      else
      {
        labelsToolTip.SetToolTip(label, null);
      }
    }
  }
}
