// Copyright (c) 2012, 2016, Oracle and/or its affiliates. All rights reserved.
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
using MySQL.ForExcel.Classes;
using MySQL.ForExcel.Controls;
using MySQL.ForExcel.Forms;
using MySQL.ForExcel.Properties;
using MySql.Utility.Classes.MySql;
using MySql.Utility.Classes.MySqlWorkbench;
using MySql.Utility.Enums;

namespace MySQL.ForExcel.Panels
{
  /// <summary>
  /// Third panel shown to users within the Add-In's <see cref="ExcelAddInPane"/> where DB objects are managed.
  /// </summary>
  public partial class DbObjectSelectionPanel : AutoStyleableBasePanel
  {
    #region Fields

    /// <summary>
    /// Flag indicating if the currently selected Excel range contains any data.
    /// </summary>
    private bool _excelSelectionContainsData;

    /// <summary>
    /// A <see cref="MySqlWorkbenchConnection"/> object representing the connection to a MySQL server instance selected by users.
    /// </summary>
    private MySqlWorkbenchConnection _wbConnection;

    #endregion Fields

    /// <summary>
    /// Initializes a new instance of the <see cref="DbObjectSelectionPanel"/> class.
    /// </summary>
    public DbObjectSelectionPanel()
    {
      _excelSelectionContainsData = false;
      _wbConnection = null;
      Filter = string.Empty;
      LoadedProcedures = new List<DbProcedure>();
      LoadedTables = new List<DbTable>();
      LoadedViews = new List<DbView>();
      InitializeComponent();

      ConnectionNameLabel.Paint += Label_Paint;
      UserIPLabel.Paint += Label_Paint;
      SchemaLabel.Paint += Label_Paint;

      InheritFontToControlsExceptionList.AddRange(new[]
      {
        ExportToNewTableHotLabel.Name,
        SelectDatabaseObjectHotLabel.Name,
        ImportDataHotLabel.Name,
        EditDataHotLabel.Name,
        AppendDataHotLabel.Name,
        ImportMultiHotLabel.Name
      });

      DBObjectList.AddHeaderNode("Tables");
      DBObjectList.AddHeaderNode("Views");
      DBObjectList.AddHeaderNode("Procedures");
    }

    #region Properties

    /// <summary>
    /// Gets the currently selected database object from the ones in the DB objects selection list.
    /// </summary>
    [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    public DbObject CurrentSelectedDbObject
    {
      get
      {
        var selectedNode = DBObjectList.SelectedNode;
        if (selectedNode == null || selectedNode.Type != MySqlListViewNode.MySqlNodeType.DbObject)
        {
          return null;
        }

        return selectedNode.DbObject;
      }
    }

    /// <summary>
    /// Gets or sets a value indicating whether the currently selected Excel range contains any data.
    /// </summary>
    [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    public bool ExcelSelectionContainsData
    {
      get
      {
        return _excelSelectionContainsData;
      }

      set
      {
        _excelSelectionContainsData = value;
        ExportToNewTableHotLabel.Enabled = value;
        AppendDataHotLabel.Enabled = value && CurrentSelectedDbObject is DbTable;
      }
    }

    /// <summary>
    /// Gets a string containing the filter to apply to the schemas list.
    /// </summary>
    [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    public string Filter { get; private set; }

    /// <summary>
    /// Gets a list of stored procedures loaded in this panel.
    /// </summary>
    [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    public List<DbProcedure> LoadedProcedures { get; private set; }

    /// <summary>
    /// Gets a list of tables loaded in this panel.
    /// </summary>
    [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    public List<DbTable> LoadedTables { get; private set; }

    /// <summary>
    /// Gets a list of views loaded in this panel.
    /// </summary>
    [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    public List<DbView> LoadedViews { get; private set; }

    #endregion Properties

    /// <summary>
    /// Refreshes the availability of action labels linked to the currently selected DB object.
    /// </summary>
    /// <param name="tableName">Name of the table with status update.</param>
    /// <param name="editActive">Flag indicating if the Edit Data action is enabled on the currently selected object.</param>
    public void RefreshActionLabelsEnabledStatus(string tableName, bool editActive)
    {
      bool multipleObjectsSelected = DBObjectList.SelectedNodes.Count > 1;
      ImportDataHotLabel.Visible = !multipleObjectsSelected;
      ImportDataHotLabel.Refresh();
      ImportMultiHotLabel.Visible = multipleObjectsSelected;
      ImportMultiHotLabel.Refresh();
      ImportMultiHotLabel.Enabled = multipleObjectsSelected;
      EditDataHotLabel.Visible = !multipleObjectsSelected;
      EditDataHotLabel.Refresh();
      AppendDataHotLabel.Visible = !multipleObjectsSelected;
      AppendDataHotLabel.Refresh();
      if (multipleObjectsSelected)
      {
        return;
      }

      DbObject dbObj = CurrentSelectedDbObject;
      bool isSelected = dbObj != null;
      bool isTable = dbObj is DbTable;
      bool tableNameMatches = isTable && !string.IsNullOrEmpty(tableName) && dbObj.Name == tableName;
      ImportDataHotLabel.Enabled = isSelected;
      EditDataHotLabel.Enabled = isTable && !editActive && (tableName == null || tableNameMatches);
      AppendDataHotLabel.Enabled = isTable && ExcelSelectionContainsData;
    }

    /// <summary>
    /// Sets the current active connection used to query the database.
    /// </summary>
    /// <param name="connection">A <see cref="MySqlWorkbenchConnection"/> object representing the current connection to a MySQL server.</param>
    /// <param name="schema"></param>
    /// <returns><c>true</c> if schemas were loaded into the schemas list, <c>false</c> otherwise.</returns>
    public bool SetConnection(MySqlWorkbenchConnection connection, string schema)
    {
      _wbConnection = connection;
      _wbConnection.Schema = schema;
      ConnectionNameLabel.Text = _wbConnection.Name;
      UserIPLabel.Text = string.Format("User: {0}, IP: {1}", _wbConnection.UserName, _wbConnection.Host);
      SchemaLabel.Text = string.Format("Schema: {0}", _wbConnection.Schema);
      DBObjectsFilter.Width = DBObjectList.Width;
      bool schemasLoadedSuccessfully = RefreshDbObjectsList(true);
      RefreshActionLabelsEnabledStatus(null, false);
      return schemasLoadedSuccessfully;
    }

    /// <summary>
    /// Event delegate method fired when <see cref="AppendDataHotLabel"/> is clicked.
    /// </summary>
    /// <param name="sender">Sender object.</param>
    /// <param name="e">Event arguments.</param>
    private void AppendDataHotLabel_Click(object sender, EventArgs e)
    {
      var selectedNode = DBObjectList.SelectedNode;
      if (selectedNode == null || selectedNode.Type != MySqlListViewNode.MySqlNodeType.DbObject || !(selectedNode.DbObject is DbTable) || _wbConnection == null)
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
        ExportDataToTable(selectedNode.DbObject as DbTable);
      }
      catch (Exception ex)
      {
        MySqlSourceTrace.WriteAppErrorToLog(ex, null, Resources.AppendDataErrorTitle, true);
      }
      finally
      {
        if (Cursor == Cursors.WaitCursor)
        {
          Cursor = Cursors.Default;
        }
      }
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
        excelAddInPane.CloseSchema(true, true);
      }
    }

    /// <summary>
    /// Event delegate method fired when <see cref="CloseButton"/> is clicked.
    /// </summary>
    /// <param name="sender">Sender object.</param>
    /// <param name="e">Event arguments.</param>
    private void CloseButton_Click(object sender, EventArgs e)
    {
      Globals.ThisAddIn.CloseExcelPane(Parent as ExcelAddInPane);

      // Synchronize the MySQL for Excel toggle button state of the currently activated window.
      Globals.ThisAddIn.CustomMySqlRibbon.ChangeShowMySqlForExcelPaneToggleState(false);
    }

    /// <summary>
    /// Event delegate method fired after a node in the <see cref="DBObjectList"/> is selected.
    /// </summary>
    /// <param name="sender">Sender object.</param>
    /// <param name="e">Event arguments.</param>
    private void DBObjectList_AfterSelect(object sender, TreeViewEventArgs e)
    {
      var listControl = sender as MySqlListView;
      if (listControl == null || listControl.SelectedNode == null)
      {
        return;
      }

      if (listControl.SelectedNodes.Count > 1)
      {
        // Refresh the actions related to a multiple selection of tables / views.
        RefreshActionLabelsEnabledStatus(null, false);
      }
      else
      {
        // Refresh the enabled/disabled status of actions related to the single selected DbObject related node.
        var editActive = false;
        if (listControl.SelectedNodes.Count == 1)
        {
          var addInPane = Parent as ExcelAddInPane;
          editActive = addInPane != null && addInPane.TableHasEditOnGoing(listControl.SelectedNode.DbObject.Name);
        }

        RefreshActionLabelsEnabledStatus(null, editActive);
      }
    }

    /// <summary>
    /// Event delegate method fired when the <see cref="DBObjectsContextMenuStrip"/> is being opened.
    /// </summary>
    /// <param name="sender">Sender object.</param>
    /// <param name="e">Event arguments.</param>
    private void DBObjectsContextMenuStrip_Opening(object sender, CancelEventArgs e)
    {
      bool selectedNodeIsDbObject = DBObjectList.SelectedNodes.Count == 1
        && DBObjectList.SelectedNode != null
        && DBObjectList.SelectedNode.Type == MySqlListViewNode.MySqlNodeType.DbObject;
      bool selectedNodeIsDbTable = selectedNodeIsDbObject && DBObjectList.SelectedNode.DbObject is DbTable;
      bool selectedNodeIsDbView = selectedNodeIsDbObject && DBObjectList.SelectedNode.DbObject is DbView;
      ImportRelatedToolStripMenuItem.Visible = selectedNodeIsDbTable;
      PreviewDataToolStripMenuItem.Visible = selectedNodeIsDbTable || selectedNodeIsDbView;
    }

    /// <summary>
    /// Event delegate method fired when a key is pressed that triggers the search.
    /// </summary>
    /// <param name="sender">Sender object.</param>
    /// <param name="e">Event arguments.</param>
    private void DBObjectsFilter_SearchFired(object sender, EventArgs e)
    {
      var searchBox = sender as SearchEdit;
      if (searchBox == null)
      {
        return;
      }

      Filter = DBObjectsFilter.Text.ToUpper();
      try
      {
        RefreshDbObjectsList(false);
      }
      catch (Exception ex)
      {
        MySqlSourceTrace.WriteAppErrorToLog(ex, true);
      }
    }

    /// <summary>
    /// Event delegate method fired when <see cref="EditDataHotLabel"/> is clicked.
    /// </summary>
    /// <param name="sender">Sender object.</param>
    /// <param name="e">Event arguments.</param>
    private void EditDataHotLabel_Click(object sender, EventArgs e)
    {
      var selectedNode = DBObjectList.SelectedNode;
      var selectedTable = selectedNode.DbObject as DbTable;
      if (selectedNode.Type != MySqlListViewNode.MySqlNodeType.DbObject || selectedTable == null || _wbConnection == null)
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
        var excelAddInPane = Parent as ExcelAddInPane;
        EditDataHotLabel.Enabled = excelAddInPane != null && !excelAddInPane.EditTableData(selectedTable, false, null);
      }
      catch (Exception ex)
      {
        MySqlSourceTrace.WriteAppErrorToLog(ex, null, Resources.EditDataErrorTitle, true);
      }
    }

    /// <summary>
    /// Exports currently selected Excel data to a new MySQL table or appends it to an existing MySQL table.
    /// </summary>
    /// <param name="appendToTable">Table to append the data to, if null exports to a new table.</param>
    /// <returns><c>true</c> if data was exported/appended successfully, <c>false</c> otherwise.</returns>
    private bool ExportDataToTable(DbTable appendToTable)
    {
      var excelAddInPane = Parent as ExcelAddInPane;
      return excelAddInPane != null && excelAddInPane.AppendDataToTable(appendToTable);
    }

    /// <summary>
    /// Event delegate method fired when <see cref="ExportToNewTableHotLabel"/> is clicked.
    /// </summary>
    /// <param name="sender">Sender object.</param>
    /// <param name="e">Event arguments.</param>
    private void ExportToNewTableHotLabel_Click(object sender, EventArgs e)
    {
      var passwordFlags = _wbConnection.TestConnectionAndRetryOnWrongPassword();
      if (!passwordFlags.ConnectionSuccess)
      {
        return;
      }

      bool success = ExportDataToTable(null);
      if (!success)
      {
        return;
      }

      DBObjectList.Nodes[0].Nodes.Clear();

      // Objects are rendered a little differently on Windows XP than in newer OS versions.
      // We need to verify which OS version is currently running to address the correct render method.
      if (Environment.OSVersion.Version.Major <= 5)
      {
        // This is the correct render method for Windows XP and older OS versions.
        RefreshDbObjectsList(true);
      }
      else
      {
        // This is the correct render method for Windows Vista and newer OS versions.
        LoadTables();
        RefreshDbObjectsList(false);
      }

      DBObjectList_AfterSelect(DBObjectList, new TreeViewEventArgs(null));
    }

    /// <summary>
    /// Event delegate method fired when <see cref="ImportDataHotLabel"/> is clicked.
    /// </summary>
    /// <param name="sender">Sender object.</param>
    /// <param name="e">Event arguments.</param>
    private void ImportDataHotLabel_Click(object sender, EventArgs e)
    {
      var selectedNode = DBObjectList.SelectedNode;
      var parentTaskPane = Parent as ExcelAddInPane;
      if (selectedNode == null || parentTaskPane == null || selectedNode.Type != MySqlListViewNode.MySqlNodeType.DbObject || selectedNode.DbObject == null || _wbConnection == null)
      {
        return;
      }

      var passwordFlags = _wbConnection.TestConnectionAndRetryOnWrongPassword();
      if (!passwordFlags.ConnectionSuccess)
      {
        return;
      }

      if (parentTaskPane.ActiveWorksheetInEditMode)
      {
        var dr = MiscUtilities.ShowCustomizedWarningDialog(Resources.WorksheetInEditModeWarningTitle, Resources.WorksheetInEditModeWarningDetail);
        if (dr != DialogResult.Yes)
        {
          return;
        }

        var newWorksheet = Globals.ThisAddIn.ActiveWorkbook.CreateWorksheet(selectedNode.DbObject.Name, true);
        if (newWorksheet == null)
        {
          return;
        }
      }

      try
      {
        DialogResult dr = DialogResult.Cancel;
        Cursor = Cursors.WaitCursor;
        var activeWorkbook = Globals.ThisAddIn.ActiveWorkbook;
        if (selectedNode.DbObject is DbTable)
        {
          var dbTable = selectedNode.DbObject as DbTable;
          dbTable.ImportParameters.ForEditDataOperation = false;
          using (var importForm = new ImportTableViewForm(dbTable, activeWorkbook.ActiveSheet.Name))
          {
            dr = importForm.ShowDialog();
          }
        }
        else if (selectedNode.DbObject is DbView)
        {
          var dbView = selectedNode.DbObject as DbView;
          dbView.ImportParameters.ForEditDataOperation = false;
          using (var importForm = new ImportTableViewForm(dbView, activeWorkbook.ActiveSheet.Name))
          {
            dr = importForm.ShowDialog();
          }
        }
        else if (selectedNode.DbObject is DbProcedure)
        {
          using (var importProcedureForm = new ImportProcedureForm(selectedNode.DbObject as DbProcedure, parentTaskPane.ActiveWorksheet.Name))
          {
            dr = importProcedureForm.ShowDialog();
          }
        }

        if (dr == DialogResult.OK)
        {
          RefreshActionLabelsEnabledStatus(null, false);
        }
      }
      catch (Exception ex)
      {
        MySqlSourceTrace.WriteAppErrorToLog(ex, null, Resources.ImportDataErrorTitle, true);
      }
      finally
      {
        Cursor = Cursors.Default;
      }
    }

    /// <summary>
    /// Event delegate method fired when <see cref="ImportMultiHotLabel"/> is clicked.
    /// </summary>
    /// <param name="sender">Sender object.</param>
    /// <param name="e">Event arguments.</param>
    private void ImportMultiHotLabel_Click(object sender, EventArgs e)
    {
      ImportMultipleDbObjects(false);
    }

    /// <summary>
    /// Opens the <see cref="ImportMultipleDialog"/> loading the selected database objects.
    /// </summary>
    /// <param name="selectAllRelatedDbObjects">Flag indicating whether all found related tables or views are selected by default.</param>
    private void ImportMultipleDbObjects(bool selectAllRelatedDbObjects)
    {
      var passwordFlags = _wbConnection.TestConnectionAndRetryOnWrongPassword();
      if (!passwordFlags.ConnectionSuccess)
      {
        return;
      }

      var tablesAndViewsList = new List<DbView>(LoadedTables);
      tablesAndViewsList.AddRange(LoadedViews);
      Cursor = Cursors.WaitCursor;
      using (var importDialog = new ImportMultipleDialog(tablesAndViewsList, selectAllRelatedDbObjects))
      {
        if (importDialog.ShowDialog() == DialogResult.OK)
        {
          RefreshActionLabelsEnabledStatus(null, false);
        }
      }

      Cursor = Cursors.Default;
    }

    /// <summary>
    /// Event delegate method fired when <see cref="ImportRelatedToolStripMenuItem"/> is clicked.
    /// </summary>
    /// <param name="sender">Sender object.</param>
    /// <param name="e">Event arguments.</param>
    private void ImportRelatedToolStripMenuItem_Click(object sender, EventArgs e)
    {
      ImportMultipleDbObjects(true);
    }

    /// <summary>
    /// Fetches all MySQL store procedure names of the given type from the current connection and loads them in the <see cref="LoadedProcedures"/> list.
    /// </summary>
    private void LoadProcedures()
    {
      var proceduresTable = _wbConnection.GetSchemaInformation(SchemaInformationType.Procedures, true, null, _wbConnection.Schema, null, "PROCEDURE");
      if (proceduresTable == null)
      {
        return;
      }

      LoadedProcedures.ForEach(dbo => dbo.Dispose());
      LoadedProcedures.Clear();
      LoadedProcedures.AddRange(proceduresTable.Rows.Cast<DataRow>().Select(dataRow => dataRow["ROUTINE_NAME"].ToString()).Select(procedureName => new DbProcedure(_wbConnection, procedureName)));
    }

    /// <summary>
    /// Fetches all MySQL table names of the given type from the current connection and loads them in the <see cref="LoadedTables"/> list.
    /// </summary>
    private void LoadTables()
    {
      var tablesTable = _wbConnection.GetSchemaInformation(SchemaInformationType.Tables, true, null, _wbConnection.Schema);
      if (tablesTable == null)
      {
        return;
      }

      LoadedTables.ForEach(dbo => dbo.Dispose());
      LoadedTables.Clear();
      LoadedTables.AddRange(tablesTable.Rows.Cast<DataRow>().Select(dataRow => dataRow["TABLE_NAME"].ToString()).Select(tableName => new DbTable(_wbConnection, tableName)));
    }

    /// <summary>
    /// Fetches all MySQL table names of the given type from the current connection and loads them in the <see cref="LoadedViews"/> list.
    /// </summary>
    private void LoadViews()
    {
      var viewsTable = _wbConnection.GetSchemaInformation(SchemaInformationType.Views, true, null, _wbConnection.Schema);
      if (viewsTable == null)
      {
        return;
      }

      LoadedViews.ForEach(dbo => dbo.Dispose());
      LoadedViews.Clear();
      LoadedViews.AddRange(viewsTable.Rows.Cast<DataRow>().Select(dataRow => dataRow["TABLE_NAME"].ToString()).Select(viewName => new DbView(_wbConnection, viewName)));
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
    /// Event delegate method fired when <see cref="PreviewDataToolStripMenuItem"/> is clicked.
    /// </summary>
    /// <param name="sender">Sender object.</param>
    /// <param name="e">Event arguments.</param>
    private void PreviewDataToolStripMenuItem_Click(object sender, EventArgs e)
    {
      if (DBObjectList.SelectedNodes.Count != 1 || DBObjectList.SelectedNode == null || !(DBObjectList.SelectedNode.DbObject is DbView))
      {
        return;
      }

      using (var previewDialog = new PreviewTableViewDialog(DBObjectList.SelectedNode.DbObject as DbView, false))
      {
        previewDialog.ShowDialog();
      }
    }

    /// <summary>
    /// Event delegate method fired when <see cref="RefreshDatabaseObjectsToolStripMenuItem"/> is clicked.
    /// </summary>
    /// <param name="sender">Sender object.</param>
    /// <param name="e">Event arguments.</param>
    private void RefreshDatabaseObjectsToolStripMenuItem_Click(object sender, EventArgs e)
    {
      RefreshDbObjectsList(true);
    }

    /// <summary>
    /// Refreshes the DB objects list control with current objects in the connected schema.
    /// </summary>
    /// <param name="reloadFromServer">Flag indicating whether DB objects must be reloaded from the connected MySQL server.</param>
    private bool RefreshDbObjectsList(bool reloadFromServer)
    {
      if (DBObjectList.HeaderNodes.Count < 3)
      {
        return false;
      }

      bool success = true;
      try
      {
        // Avoids flickering of DB Objects lists while adding the items to it.
        DBObjectList.BeginUpdate();

        DBObjectList.ClearChildNodes();
        if (reloadFromServer)
        {
          LoadTables();
          LoadViews();
          LoadProcedures();
        }

        // Refresh Tables
        foreach (var dbTable in LoadedTables.Where(table => string.IsNullOrEmpty(Filter) || table.Name.ToUpper().Contains(Filter)))
        {
          var node = DBObjectList.AddDbObjectNode(DBObjectList.HeaderNodes[0], dbTable);
          dbTable.Selected = false;
          node.ImageIndex = 0;
        }

        // Refresh Views
        foreach (var dbView in LoadedViews.Where(view => string.IsNullOrEmpty(Filter) || view.Name.ToUpper().Contains(Filter)))
        {
          var node = DBObjectList.AddDbObjectNode(DBObjectList.HeaderNodes[1], dbView);
          dbView.Selected = false;
          node.ImageIndex = 1;
        }

        // Refresh Procedures
        foreach (var dbProcedure in LoadedProcedures.Where(procedure => string.IsNullOrEmpty(Filter) || procedure.Name.ToUpper().Contains(Filter)))
        {
          var node = DBObjectList.AddDbObjectNode(DBObjectList.HeaderNodes[2], dbProcedure);
          dbProcedure.Selected = false;
          node.ImageIndex = 2;
        }

        DBObjectList.ExpandAll();
        DBObjectList.Nodes[0].EnsureVisible();

        // Avoids flickering of DB Objects lists while adding the items to it.
        DBObjectList.EndUpdate();
        DBObjectList_AfterSelect(null, null);
      }
      catch (Exception ex)
      {
        success = false;
        MySqlSourceTrace.WriteAppErrorToLog(ex, null, Resources.RefreshDBObjectsErrorTitle, true);
      }

      return success;
    }
  }
}