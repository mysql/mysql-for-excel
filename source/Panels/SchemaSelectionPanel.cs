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
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Windows.Forms;
using MySql.Data.MySqlClient;
using MySQL.ForExcel.Classes;
using MySQL.ForExcel.Controls;
using MySQL.ForExcel.Forms;
using MySQL.ForExcel.Properties;
using MySQL.Utility.Classes;
using MySQL.Utility.Classes.MySQLWorkbench;

namespace MySQL.ForExcel.Panels
{
  /// <summary>
  /// Second panel shown to users within the Add-In's <see cref="ExcelAddInPane"/> where schemas are managed.
  /// </summary>
  public partial class SchemaSelectionPanel : AutoStyleableBasePanel
  {
    #region Fields

    /// <summary>
    /// String array containing schema names considered system schemas.
    /// </summary>
    private static string[] _systemSchemasListValues;

    #endregion Fields

    /// <summary>
    /// Initializes a new instance of the <see cref="SchemaSelectionPanel"/> class.
    /// </summary>
    public SchemaSelectionPanel()
    {
      _systemSchemasListValues = new[] { "mysql", "information_schema", "performance_schema" };
      InitializeComponent();

      ConnectionNameLabel.Paint += Label_Paint;
      UserIPLabel.Paint += Label_Paint;
      InheritFontToControlsExceptionList.Add(SelectSchemaHotLabel.Name);
      InheritFontToControlsExceptionList.Add(CreateNewSchemaHotLabel.Name);
      LoadedSchemas = new List<string>();
      SchemasList.AddNode(null, "Schemas");
      SchemasList.AddNode(null, "System Schemas");
    }

    #region Properties

    /// <summary>
    /// Gets a string containing the filter to apply to the schemas list.
    /// </summary>
    [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    public string Filter { get; private set; }

    /// <summary>
    /// Gets a list of schemas loaded in this panel.
    /// </summary>
    [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    public List<string> LoadedSchemas { get; private set; }

    /// <summary>
    /// Gets a <see cref="MySqlWorkbenchConnection"/> object representing the connection to a MySQL server instance selected by users.
    /// </summary>
    [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    public MySqlWorkbenchConnection WbConnection { get; private set; }

    #endregion Properties

    /// <summary>
    /// Sets the current active connection used to query the database.
    /// </summary>
    /// <param name="connection">A <see cref="MySqlWorkbenchConnection"/> object representing the current connection to a MySQL server.</param>
    /// <returns><c>true</c> if schemas were loaded into the schemas list, <c>false</c> otherwise.</returns>
    public bool SetConnection(MySqlWorkbenchConnection connection)
    {
      Filter = string.Empty;
      WbConnection = connection;
      ConnectionNameLabel.Text = connection.Name;
      UserIPLabel.Text = string.Format("User: {0}, IP: {1}", connection.UserName, connection.Host);
      bool schemasLoaded = LoadSchemas();
      if (schemasLoaded)
      {
        SchemasList_AfterSelect(null, null);
      }

      return schemasLoaded;
    }

    /// <summary>
    /// Event delegate method fired when <see cref="BackButton"/> is clicked.
    /// </summary>
    /// <param name="sender">Sender object.</param>
    /// <param name="e">Event arguments.</param>
    private void BackButton_Click(object sender, EventArgs e)
    {
      var excelAddInPane = Parent as ExcelAddInPane;
      if (excelAddInPane != null)
      {
        excelAddInPane.CloseConnection(true);
      }
    }

    /// <summary>
    /// Event delegate method fired when <see cref="CreateNewSchemaHotLabel"/> is clicked.
    /// </summary>
    /// <param name="sender">Sender object.</param>
    /// <param name="e">Event arguments.</param>
    private void CreateNewSchemaHotLabel_Click(object sender, EventArgs e)
    {
      NewSchemaDialog dlg = new NewSchemaDialog();
      if (dlg.ShowDialog() == DialogResult.Cancel)
      {
        return;
      }

      PasswordDialogFlags passwordFlags = WbConnection.TestConnectionAndRetryOnWrongPassword();
      if (!passwordFlags.ConnectionSuccess)
      {
        return;
      }

      string connectionString = WbConnection.GetConnectionStringBuilder().ConnectionString;
      string sql = string.Format("CREATE DATABASE `{0}`", dlg.SchemaName);
      try
      {
        MySqlHelper.ExecuteNonQuery(connectionString, sql);
      }
      catch (Exception ex)
      {
        MiscUtilities.ShowCustomizedErrorDialog(Resources.ErrorCreatingNewSchema, ex.Message, true);
        MySqlSourceTrace.WriteAppErrorToLog(ex);
        return;
      }

      LoadSchemas();
    }

    /// <summary>
    /// Fetches all schema names from the current connection and loads them in the <see cref="SchemasList"/> tree.
    /// </summary>
    /// <returns><c>true</c> if schemas were loaded successfully, <c>false</c> otherwise.</returns>
    private bool LoadSchemas()
    {
      try
      {
        // Avoids flickering of schemas list while adding the items to it.
        SchemasList.BeginUpdate();

        LoadedSchemas.Clear();
        foreach (TreeNode node in SchemasList.Nodes)
        {
          node.Nodes.Clear();
        }

        DataTable databases = WbConnection.GetSchemaCollection("Databases", null);
        foreach (DataRow row in databases.Rows)
        {
          string schemaName = row["DATABASE_NAME"].ToString();

          // If the user has specified a filter then check it
          if (!string.IsNullOrEmpty(Filter) && !schemaName.ToUpper().Contains(Filter))
          {
            continue;
          }

          string lcSchemaName = schemaName.ToLowerInvariant();
          int index = _systemSchemasListValues.Contains(lcSchemaName) ? 1 : 0;
          LoadedSchemas.Add(schemaName);
          TreeNode node = SchemasList.AddNode(SchemasList.Nodes[index], schemaName);
          node.Tag = schemaName;
          node.ImageIndex = 0;
        }

        if (SchemasList.Nodes[0].GetNodeCount(true) > 0)
        {
          SchemasList.Nodes[0].Expand();
        }

        // Avoids flickering of schemas list while adding the items to it.
        SchemasList.EndUpdate();

        return true;
      }
      catch (Exception ex)
      {
        MiscUtilities.ShowCustomizedErrorDialog(Resources.SchemasLoadingErrorTitle, ex.Message, true);
        MySqlSourceTrace.WriteAppErrorToLog(ex);
        return false;
      }
    }

    /// <summary>
    /// Event delegate method fired when <see cref="NextButton"/> is clicked.
    /// </summary>
    /// <param name="sender">Sender object.</param>
    /// <param name="e">Event arguments.</param>
    private void NextButton_Click(object sender, EventArgs e)
    {
      PasswordDialogFlags passwordFlags = WbConnection.TestConnectionAndRetryOnWrongPassword();
      if (!passwordFlags.ConnectionSuccess)
      {
        return;
      }

      try
      {
        string databaseName = SchemasList.SelectedNode.Tag as string;
        var excelAddInPane = Parent as ExcelAddInPane;
        if (excelAddInPane != null)
        {
          excelAddInPane.OpenSchema(databaseName, true);
        }
      }
      catch (Exception ex)
      {
        MiscUtilities.ShowCustomizedErrorDialog(Resources.SchemaOpeningErrorTitle, ex.Message, true);
        MySqlSourceTrace.WriteAppErrorToLog(ex);
      }
    }

    /// <summary>
    /// Event delegate method fired when <see cref="OptionsButton"/> is clicked.
    /// </summary>
    /// <param name="sender">Sender object.</param>
    /// <param name="e">Event arguments.</param>
    private void OptionsButton_Click(object sender, EventArgs e)
    {
      using (GlobalOptionsDialog optionsDialog = new GlobalOptionsDialog())
      {
        if (optionsDialog.ShowDialog() != DialogResult.OK)
        {
          return;
        }

        var excelAddInPane = Parent as ExcelAddInPane;
        if (excelAddInPane != null)
        {
          excelAddInPane.RefreshWbConnectionTimeouts();
        }
      }
    }

    /// <summary>
    /// Event delegate method fired when <see cref="RefreshSchemasToolStripMenuItem"/> is clicked.
    /// </summary>
    /// <param name="sender">Sender object.</param>
    /// <param name="e">Event arguments.</param>
    private void RefreshSchemasToolStripMenuItem_Click(object sender, EventArgs e)
    {
      if (LoadSchemas())
      {
        SchemasList_AfterSelect(null, null);
      }
    }

    /// <summary>
    /// Event delegate method fired when a key is pressed within the <see cref="SchemaFilter"/> control.
    /// </summary>
    /// <param name="sender">Sender object.</param>
    /// <param name="e">Event arguments.</param>
    private void SchemaFilter_KeyDown(object sender, KeyEventArgs e)
    {
      if (e.KeyCode != Keys.Enter)
      {
        return;
      }

      Filter = SchemaFilter.Text.Trim().ToUpper();
      LoadSchemas();
    }

    /// <summary>
    /// Event delegate method fired after a node in the <see cref="SchemasList"/> is selected.
    /// </summary>
    /// <param name="sender">Sender object.</param>
    /// <param name="e">Event arguments.</param>
    private void SchemasList_AfterSelect(object sender, TreeViewEventArgs e)
    {
      NextButton.Enabled = e != null && e.Node != null && e.Node.Level > 0;
    }

    /// <summary>
    /// Event delegate method fired when a node in the <see cref="SchemasList"/> is double-clicked.
    /// </summary>
    /// <param name="sender">Sender object.</param>
    /// <param name="e">Event arguments.</param>
    private void SchemasList_NodeMouseDoubleClick(object sender, TreeNodeMouseClickEventArgs e)
    {
      if (e.Node.Level > 0)
      {
        NextButton_Click(this, EventArgs.Empty);
      }
    }
  }
}