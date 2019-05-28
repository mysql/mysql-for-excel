// Copyright (c) 2012, 2019, Oracle and/or its affiliates. All rights reserved.
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
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using MySql.Utility.Classes.Logging;
using MySQL.ForExcel.Classes;
using MySQL.ForExcel.Controls;
using MySQL.ForExcel.Forms;
using MySQL.ForExcel.Properties;
using MySql.Utility.Classes.MySqlWorkbench;
using MySql.Utility.Enums;

namespace MySQL.ForExcel.Panels
{
  /// <summary>
  /// Second panel shown to users within the Add-In's <see cref="ExcelAddInPane"/> where schemas are managed.
  /// </summary>
  public sealed partial class SchemaSelectionPanel : AutoStyleableBasePanel
  {
    #region Fields

    /// <summary>
    /// A string containing the filter to apply to the schemas list.
    /// </summary>
    private string _filter;

    /// <summary>
    /// A <see cref="MySqlWorkbenchConnection"/> object representing the connection to a MySQL server instance selected by users.
    /// </summary>
    private MySqlWorkbenchConnection _wbConnection;

    #endregion Fields

    /// <summary>
    /// Initializes a new instance of the <see cref="SchemaSelectionPanel"/> class.
    /// </summary>
    public SchemaSelectionPanel()
    {
      InitializeComponent();
      AdjustColorsForColorTheme(false, null);
      DisplaySchemaCollationsToolStripMenuItem.Checked = Settings.Default.SchemasDisplayCollations;
      SetItemsAppearance(false);
      InheritFontToControlsExceptionList.Add(SelectSchemaHotLabel.Name);
      InheritFontToControlsExceptionList.Add(CreateNewSchemaHotLabel.Name);
      LoadedSchemas = new List<DbSchema>();
    }

    #region Properties

    /// <summary>
    /// Gets a list of schemas loaded in this panel.
    /// </summary>
    [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    public List<DbSchema> LoadedSchemas { get; }

    #endregion Properties

    /// <summary>
    /// Adjusts the colors to match the current color theme.
    /// </summary>
    /// <param name="callBase">Calls the base functionality.</param>
    /// <param name="officeTheme">The current <see cref="OfficeTheme"/>.</param>
    public override void AdjustColorsForColorTheme(bool callBase, OfficeTheme officeTheme)
    {
      if (callBase)
      {
        base.AdjustColorsForColorTheme(false, officeTheme);
      }

      ConnectionInfoLabel.ForeColor =
        UserLabel.ForeColor = officeTheme != null && officeTheme.ThemeColor.IsThemeColorDark()
          ? Color.LightGray
          : SystemColors.ControlDarkDark;
    }

    /// <summary>
    /// Sets the current active connection used to query the database.
    /// </summary>
    /// <param name="connection">A <see cref="MySqlWorkbenchConnection"/> object representing the current connection to a MySQL server.</param>
    /// <returns><c>true</c> if schemas were loaded into the schemas list, <c>false</c> otherwise.</returns>
    public bool SetConnection(MySqlWorkbenchConnection connection)
    {
      _filter = string.Empty;
      _wbConnection = connection;
      ConnectionNameLabel.Text = connection.Name;
      UserLabel.Text = connection.UserName;
      ConnectionInfoLabel.Text = connection.DisplayConnectionSummaryText;
      var schemasLoaded = LoadSchemas();
      if (schemasLoaded)
      {
        SchemasList_AfterSelect(null, null);
      }

      SchemaFilter.Width = SchemasList.Width;
      return schemasLoaded;
    }

    /// <summary>
    /// Event delegate method fired when <see cref="BackButton"/> is clicked.
    /// </summary>
    /// <param name="sender">Sender object.</param>
    /// <param name="e">Event arguments.</param>
    private void BackButton_Click(object sender, EventArgs e)
    {
      if (Parent is ExcelAddInPane excelAddInPane)
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
      using (var newSchemaDialog = new NewSchemaDialog(_wbConnection))
      {
        if (newSchemaDialog.ShowDialog() == DialogResult.OK)
        {
          LoadSchemas();
        }
      }
    }

    /// <summary>
    /// Event delegate method fired when <see cref="DisplaySchemaCollationsToolStripMenuItem"/> is clicked.
    /// </summary>
    /// <param name="sender">Sender object.</param>
    /// <param name="e">Event arguments.</param>
    private void DisplaySchemaCollationsToolStripMenuItem_Click(object sender, EventArgs e)
    {
      SetItemsAppearance(true);
    }

    /// <summary>
    /// Fetches all schema names from the current connection and loads them in the <see cref="SchemasList"/> tree.
    /// </summary>
    /// <returns><c>true</c> if schemas were loaded successfully, <c>false</c> otherwise.</returns>
    private bool LoadSchemas()
    {
      if (SchemasList.HeaderNodes.Count < 2)
      {
        return false;
      }

      Cursor = Cursors.WaitCursor;
      try
      {
        // Avoids flickering of schemas list while adding the items to it.
        SchemasList.BeginUpdate();

        LoadedSchemas.ForEach(schema => schema.Dispose());
        LoadedSchemas.Clear();
        foreach (TreeNode node in SchemasList.Nodes)
        {
          node.Nodes.Clear();
        }

        var databases = _wbConnection.GetSchemaInformation(SchemaInformationType.Databases, false, null);
        foreach (DataRow row in databases.Rows)
        {
          var schemaName = row["DATABASE_NAME"].ToString();

          // If the user has specified a filter then check it
          if (!string.IsNullOrEmpty(_filter) && !schemaName.ToUpper().Contains(_filter))
          {
            continue;
          }

          // Create the DbSchema and MySqlListViewNode objects
          var schemaObject = new DbSchema(_wbConnection, schemaName,
                                          row["DEFAULT_CHARACTER_SET_NAME"].ToString(),
                                          row["DEFAULT_COLLATION_NAME"].ToString(),
                                          DisplaySchemaCollationsToolStripMenuItem.Checked);
          var lcSchemaName = schemaName.ToLowerInvariant();
          var headerNode = SchemasList.HeaderNodes[MySqlWorkbenchConnection.SystemSchemaNames.Contains(lcSchemaName) ? 1 : 0];
          LoadedSchemas.Add(schemaObject);
          var node = SchemasList.AddDbObjectNode(headerNode, schemaObject);
          node.ImageIndex = DisplaySchemaCollationsToolStripMenuItem.Checked ? 1 : 0;
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
        Logger.LogException(ex, true, Resources.SchemasLoadingErrorTitle);
        return false;
      }
      finally
      {
        Cursor = Cursors.Default;
      }
    }

    /// <summary>
    /// Event delegate method fired when <see cref="NextButton"/> is clicked.
    /// </summary>
    /// <param name="sender">Sender object.</param>
    /// <param name="e">Event arguments.</param>
    private void NextButton_Click(object sender, EventArgs e)
    {
      var selectedNode = SchemasList.SelectedNode;
      if (selectedNode == null || selectedNode.Type == MySqlListViewNode.MySqlNodeType.Header || string.IsNullOrEmpty(selectedNode.DbObject.Name))
      {
        return;
      }

      var passwordFlags = _wbConnection.TestConnectionAndRetryOnWrongPassword();
      if (!passwordFlags.ConnectionSuccess)
      {
        return;
      }

      try
      {
        if (Parent is ExcelAddInPane excelAddInPane)
        {
          excelAddInPane.OpenSchema(selectedNode.DbObject.Name, true);
        }
      }
      catch (Exception ex)
      {
        Logger.LogException(ex, true, Resources.SchemaOpeningErrorTitle);
      }
    }

    /// <summary>
    /// Event delegate method fired when <see cref="OptionsButton"/> is clicked.
    /// </summary>
    /// <param name="sender">Sender object.</param>
    /// <param name="e">Event arguments.</param>
    private void OptionsButton_Click(object sender, EventArgs e)
    {
      Globals.ThisAddIn.ShowGlobalOptionsDialog();
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
    /// Event delegate method fired when a key is pressed that triggers the search.
    /// </summary>
    /// <param name="sender">Sender object.</param>
    /// <param name="e">Event arguments.</param>
    private void SchemaFilter_SearchFired(object sender, EventArgs e)
    {
      if (!(sender is SearchEdit))
      {
        return;
      }

      _filter = SchemaFilter.Text.ToUpper();
      LoadSchemas();
    }

    /// <summary>
    /// Event delegate method fired after a node in the <see cref="SchemasList"/> is selected.
    /// </summary>
    /// <param name="sender">Sender object.</param>
    /// <param name="e">Event arguments.</param>
    private void SchemasList_AfterSelect(object sender, TreeViewEventArgs e)
    {
      NextButton.Enabled = e?.Node != null && e.Node.Level > 0;
    }

    /// <summary>
    /// Event delegate method fired when a node in the <see cref="SchemasList"/> is double-clicked.
    /// </summary>
    /// <param name="sender">Sender object.</param>
    /// <param name="e">Event arguments.</param>
    private void SchemasList_NodeMouseDoubleClick(object sender, TreeNodeMouseClickEventArgs e)
    {
      NextButton_Click(this, EventArgs.Empty);
    }

    /// <summary>
    /// Sets the appearance of <see cref="MySqlListViewNode"/> objects appearing in the <see cref="SchemasList"/>.
    /// </summary>
    /// <param name="refreshSchemasList">Flag indicating whether the <see cref="SchemasList"/> must be refreshed after resetting the appearance.</param>
    private void SetItemsAppearance(bool refreshSchemasList)
    {
      var displayCollations = DisplaySchemaCollationsToolStripMenuItem.Checked;
      if (Settings.Default.SchemasDisplayCollations != displayCollations)
      {
        Settings.Default.SchemasDisplayCollations = displayCollations;
        MiscUtilities.SaveSettings();
      }

      SchemasList.ClearHeaderNodes();
      SchemasList.SetItemsAppearance(displayCollations, false);
      SchemasList.AddHeaderNode("Schemas");
      SchemasList.AddHeaderNode("System Schemas");
      if (refreshSchemasList)
      {
        RefreshSchemasToolStripMenuItem_Click(null, EventArgs.Empty);
      }
    }
  }
}