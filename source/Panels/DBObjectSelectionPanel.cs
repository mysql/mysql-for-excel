// Copyright (c) 2012-2014, Oracle and/or its affiliates. All rights reserved.
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
using MySQL.Utility.Classes;
using MySQL.Utility.Classes.MySQLWorkbench;
using ExcelInterop = Microsoft.Office.Interop.Excel;

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
      LoadedProcedures = new List<DbObject>();
      LoadedTables = new List<DbObject>();
      LoadedViews = new List<DbObject>();
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
        ImportMultiHotLabel.Name,
        ImportJoinedDataHotLabel.Name
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
        AppendDataHotLabel.Enabled = value && CurrentSelectedDbObject != null && CurrentSelectedDbObject.Type == DbObject.DbObjectType.Table;
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
    public List<DbObject> LoadedProcedures { get; private set; }

    /// <summary>
    /// Gets a list of tables loaded in this panel.
    /// </summary>
    [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    public List<DbObject> LoadedTables { get; private set; }

    /// <summary>
    /// Gets a list of views loaded in this panel.
    /// </summary>
    [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    public List<DbObject> LoadedViews { get; private set; }

    /// <summary>
    /// Gets a <see cref="MySqlWorkbenchConnection"/> object representing the connection to a MySQL server instance selected by users.
    /// </summary>
    [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    public MySqlWorkbenchConnection WbConnection
    {
      get
      {
        return _wbConnection;
      }

      set
      {
        _wbConnection = value;
        ConnectionNameLabel.Text = _wbConnection.Name;
        UserIPLabel.Text = string.Format("User: {0}, IP: {1}", _wbConnection.UserName, _wbConnection.Host);
        SchemaLabel.Text = string.Format("Schema: {0}", _wbConnection.Schema);
        RefreshDbObjectsList(true);
      }
    }

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
      ImportJoinedDataHotLabel.Visible = multipleObjectsSelected;
      ImportJoinedDataHotLabel.Refresh();
      ImportJoinedDataHotLabel.Enabled = multipleObjectsSelected;
      AppendDataHotLabel.Visible = !multipleObjectsSelected;
      AppendDataHotLabel.Refresh();
      if (multipleObjectsSelected)
      {
        return;
      }

      DbObject dbObj = CurrentSelectedDbObject;
      bool isSelected = dbObj != null;
      bool isTable = isSelected && dbObj.Type == DbObject.DbObjectType.Table;
      bool tableNameMatches = isSelected && isTable && !string.IsNullOrEmpty(tableName) && dbObj.Name == tableName;
      ImportDataHotLabel.Enabled = isSelected;
      EditDataHotLabel.Enabled = isSelected && isTable && !editActive;
      AppendDataHotLabel.Enabled = tableNameMatches && ExcelSelectionContainsData;
    }

    /// <summary>
    /// Event delegate method fired when <see cref="AppendDataHotLabel"/> is clicked.
    /// </summary>
    /// <param name="sender">Sender object.</param>
    /// <param name="e">Event arguments.</param>
    private void AppendDataHotLabel_Click(object sender, EventArgs e)
    {
      var selectedNode = DBObjectList.SelectedNode;
      if (selectedNode == null || selectedNode.Type != MySqlListViewNode.MySqlNodeType.DbObject || selectedNode.DbObject.Type != DbObject.DbObjectType.Table || WbConnection == null)
      {
        return;
      }

      var passwordFlags = WbConnection.TestConnectionAndRetryOnWrongPassword();
      if (!passwordFlags.ConnectionSuccess)
      {
        return;
      }

      try
      {
        ExportDataToTable(selectedNode.DbObject);
      }
      catch (Exception ex)
      {
        MiscUtilities.ShowCustomizedErrorDialog(Resources.AppendDataErrorTitle, ex.Message, true);
        MySqlSourceTrace.WriteAppErrorToLog(ex);
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
    }

    /// <summary>
    /// Event delegate method fired after a node in the <see cref="DBObjectList"/> is selected.
    /// </summary>
    /// <param name="sender">Sender object.</param>
    /// <param name="e">Event arguments.</param>
    private void DBObjectList_AfterSelect(object sender, TreeViewEventArgs e)
    {
      var listControl = sender as MySqlListView;
      if (listControl == null || listControl.SelectedNode == null || listControl.SelectedNodes.Count == 0)
      {
        return;
      }

      if (listControl.SelectedNodes.Count > 1)
      {
        // Check if procedures are among the multiple selection, if so cancel the multi-selection since procedures are not allowed in it.
        var proceduresInSelection = listControl.SelectedNodes.Any(node => node.DbObject.Type == DbObject.DbObjectType.Procedure);
        if (proceduresInSelection)
        {
          listControl.SelectedNode = e.Node as MySqlListViewNode;
          return;
        }

        RefreshActionLabelsEnabledStatus(null, false);
      }
      else
      {
        // Refresh the enabled/disabled status of actions related to the selected DbObject related node.
        var addInPane = Parent as ExcelAddInPane;
        var editActive = addInPane != null && addInPane.TableHasEditOnGoing(listControl.SelectedNode.DbObject.Name);
        RefreshActionLabelsEnabledStatus(null, editActive);
      }
    }

    /// <summary>
    /// Event delegate method fired when a key is pressed within the <see cref="DBObjectsFilter"/> control.
    /// </summary>
    /// <param name="sender">Sender object.</param>
    /// <param name="e">Event arguments.</param>
    private void DBObjectsFilter_KeyDown(object sender, KeyEventArgs e)
    {
      if (e.KeyCode != Keys.Enter)
      {
        return;
      }

      Filter = DBObjectsFilter.Text.Trim().ToUpper();
      try
      {
        RefreshDbObjectsList(false);
      }
      catch (Exception ex)
      {
        MiscUtilities.ShowCustomizedErrorDialog(ex.Message);
        MySqlSourceTrace.WriteAppErrorToLog(ex);
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
      if (selectedNode == null || selectedNode.Type != MySqlListViewNode.MySqlNodeType.DbObject || selectedNode.DbObject == null || selectedNode.DbObject.Type != DbObject.DbObjectType.Table || WbConnection == null)
      {
        return;
      }

      var passwordFlags = WbConnection.TestConnectionAndRetryOnWrongPassword();
      if (!passwordFlags.ConnectionSuccess)
      {
        return;
      }

      try
      {
        var excelAddInPane = Parent as ExcelAddInPane;
        EditDataHotLabel.Enabled = excelAddInPane != null && !excelAddInPane.EditTableData(selectedNode.DbObject, false, null);
      }
      catch (Exception ex)
      {
        MiscUtilities.ShowCustomizedErrorDialog(Resources.EditDataErrorTitle, ex.Message, true);
        MySqlSourceTrace.WriteAppErrorToLog(ex);
      }
    }

    /// <summary>
    /// Exports currently selected Excel data to a new MySQL table or appends it to an existing MySQL table.
    /// </summary>
    /// <param name="appendToTable">Table to append the data to, if null exports to a new table.</param>
    /// <returns><c>true</c> if data was exported/appended successfully, <c>false</c> otherwise.</returns>
    private bool ExportDataToTable(DbObject appendToTable)
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
      var passwordFlags = WbConnection.TestConnectionAndRetryOnWrongPassword();
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
        LoadDataObjects(DbObject.DbObjectType.Table);
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
      if (selectedNode == null || parentTaskPane == null || selectedNode.Type != MySqlListViewNode.MySqlNodeType.DbObject || selectedNode.DbObject == null || WbConnection == null)
      {
        return;
      }

      var passwordFlags = WbConnection.TestConnectionAndRetryOnWrongPassword();
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

        var newWorksheet = parentTaskPane.ActiveWorkbook.CreateWorksheet(selectedNode.DbObject.Name, true);
        if (newWorksheet == null)
        {
          return;
        }
      }

      try
      {
        switch (selectedNode.DbObject.Type)
        {
          case DbObject.DbObjectType.Table:
          case DbObject.DbObjectType.View:
            ImportTableOrView(selectedNode.DbObject);
            break;

          case DbObject.DbObjectType.Procedure:
            ImportProcedure(selectedNode.DbObject);
            break;
        }
      }
      catch (Exception ex)
      {
        MiscUtilities.ShowCustomizedErrorDialog(Resources.ImportDataErrorTitle, ex.Message, true);
        MySqlSourceTrace.WriteAppErrorToLog(ex);
      }
    }

    /// <summary>
    /// Event delegate method fired when <see cref="ImportMultiHotLabel"/> is clicked.
    /// </summary>
    /// <param name="sender">Sender object.</param>
    /// <param name="e">Event arguments.</param>
    private void ImportMultiHotLabel_Click(object sender, EventArgs e)
    {
      var addInPane = Parent as ExcelAddInPane;
      if (addInPane == null)
      {
        return;
      }

      var tablesAndViewsList = new List<DbObject>(LoadedTables);
      tablesAndViewsList.AddRange(LoadedViews);
      using (var importDialog = new ImportMultipleDialog(WbConnection, tablesAndViewsList, addInPane.ActiveWorkbook.Excel8CompatibilityMode))
      {
        if (importDialog.ShowDialog() == DialogResult.Cancel || importDialog.ImportDataSet == null)
        {
          return;
        }

        // Import tables data in Excel worksheets
        var activeWorkbook = Globals.ThisAddIn.Application.ActiveWorkbook;
        var excelTablesDictionary = new Dictionary<string, ExcelInterop.ListObject>(importDialog.ImportDataSet.Tables.Count);
        foreach (MySqlDataTable mySqlTable in importDialog.ImportDataSet.Tables)
        {
          // Create a new Excel Worksheet and import the table/view data there
          var currentWorksheet = activeWorkbook.CreateWorksheet(mySqlTable.TableName, true);
          if (currentWorksheet == null)
          {
            continue;
          }

          var listObj = mySqlTable.ImportDataIntoExcelTable(Globals.ThisAddIn.Application.ActiveCell);
          var excelTable = listObj;
          if (excelTable == null)
          {
            continue;
          }

          excelTablesDictionary.Add(mySqlTable.TableName, excelTable);
        }

        // Create Excel relationships
        foreach (var relationship in importDialog.RelationshipsList)
        {
          if (relationship.Excluded)
          {
            continue;
          }

          // Get the ModelColumnName objects needed to define the relationship
          ExcelInterop.ListObject excelTable;
          ExcelInterop.ListObject relatedExcelTable;
          bool excelTableExists = excelTablesDictionary.TryGetValue(relationship.TableOrViewName, out excelTable);
          bool relatedExcelTableExists = excelTablesDictionary.TryGetValue(relationship.RelatedTableOrViewName, out relatedExcelTable);
          if (!excelTableExists || !relatedExcelTableExists)
          {
            continue;
          }

          var table = activeWorkbook.Model.ModelTables.Cast<ExcelInterop.ModelTable>().FirstOrDefault(mt => string.Equals(mt.Name, excelTable.Name.Replace(".", " "), StringComparison.InvariantCulture));
          var relatedTable = activeWorkbook.Model.ModelTables.Cast<ExcelInterop.ModelTable>().FirstOrDefault(mt => string.Equals(mt.Name, relatedExcelTable.Name.Replace(".", " "), StringComparison.InvariantCulture));
          if (table == null || relatedTable == null)
          {
            continue;
          }

          var column = table.ModelTableColumns.Cast<ExcelInterop.ModelTableColumn>().FirstOrDefault(col => string.Equals(col.Name, relationship.ColumnName, StringComparison.InvariantCulture));
          var relatedColumn = relatedTable.ModelTableColumns.Cast<ExcelInterop.ModelTableColumn>().FirstOrDefault(col => string.Equals(col.Name, relationship.RelatedColumnName, StringComparison.InvariantCulture));
          if (column == null || relatedColumn == null)
          {
            continue;
          }

          activeWorkbook.Model.ModelRelationships.Add(column, relatedColumn);
        }

        excelTablesDictionary.Clear();
      }
    }

    /// <summary>
    /// Imports data from the selected procedure DB object to the current Excel worksheet.
    /// </summary>
    /// <param name="dbo">DB object.</param>
    private void ImportProcedure(DbObject dbo)
    {
      var addInPane = Parent as ExcelAddInPane;
      if (addInPane == null)
      {
        return;
      }

      using (var importProcedureForm = new ImportProcedureForm(WbConnection, dbo, addInPane.ActiveWorksheet.Name, addInPane.ActiveWorkbook.Excel8CompatibilityMode))
      {
        if (importProcedureForm.ShowDialog() == DialogResult.Cancel)
        {
          return;
        }

        if (importProcedureForm.ImportDataSet == null)
        {
          MiscUtilities.ShowCustomizedErrorDialog(string.Format(Resources.UnableToRetrieveData, dbo.Type.ToString().ToLowerInvariant(), dbo.Name));
          return;
        }

        var excelAddInPane = addInPane;
        excelAddInPane.ImportDataToExcel(importProcedureForm.ImportDataSet, importProcedureForm.ImportHeaders, importProcedureForm.ImportType, importProcedureForm.SelectedResultSetIndex);
      }
    }

    /// <summary>
    /// Imports data from the selected table or view DB object to the current Excel worksheet.
    /// </summary>
    /// <param name="dbo">DB object.</param>
    private void ImportTableOrView(DbObject dbo)
    {
      var taskPaneControl = (ExcelAddInPane)Parent;
      using (var importForm = new ImportTableViewForm(WbConnection, dbo, taskPaneControl.ActiveWorkbook.ActiveSheet.Name, taskPaneControl.ActiveWorkbook.Excel8CompatibilityMode, false))
      {
        if (importForm.ShowDialog() == DialogResult.Cancel)
        {
          return;
        }

        if (importForm.ImportDataTable == null)
        {
          MiscUtilities.ShowCustomizedErrorDialog(string.Format(Resources.UnableToRetrieveData, dbo.Type.ToString().ToLowerInvariant(), dbo.Name));
          return;
        }

        var excelTableName = importForm.ImportDataTable.ImportDataIntoExcelTable(Globals.ThisAddIn.Application.ActiveCell).DisplayName;
        Globals.ThisAddIn.ActiveImportSessions.Add(new ImportSessionInfo(importForm.ImportDataTable, excelTableName));
        var listObject = Globals.ThisAddIn.Application.ActiveCell.ListObject;
        if (listObject == null)
        {
          return;
        }

        var toolsListObject = Globals.Factory.GetVstoObject(listObject);
        if (toolsListObject == null)
        {
          return;
        }

        toolsListObject.SetDataBinding(importForm.ImportDataTable);
        if (toolsListObject.ShowHeaders)
        {
          foreach (MySqlDataColumn col in importForm.ImportDataTable.Columns)
          {
            toolsListObject.ListColumns[col.Ordinal + 1].Name = col.DisplayName;
          }
        }
      }
    }

    /// <summary>
    /// Fetches all DB object names of the given type from the current connection and loads them in the <see cref="DBObjectList"/> tree.
    /// </summary>
    /// <param name="dataObjectType">Type of DB object to load.</param>
    private void LoadDataObjects(DbObject.DbObjectType dataObjectType)
    {
      DataTable dataObjects = null;
      string objectName = string.Empty;
      List<DbObject> loadedObjectsList = null;

      // 0 - Tables
      // 1 - Views
      // 2 - Procedures
      switch (dataObjectType)
      {
        case DbObject.DbObjectType.Procedure:
          dataObjects = WbConnection.GetSchemaCollection("Procedures", null, WbConnection.Schema, null, "PROCEDURE");
          objectName = "ROUTINE_NAME";
          loadedObjectsList = LoadedProcedures;
          break;

        case DbObject.DbObjectType.Table:
          dataObjects = WbConnection.GetSchemaCollection("Tables", null, WbConnection.Schema);
          objectName = "TABLE_NAME";
          loadedObjectsList = LoadedTables;
          break;

        case DbObject.DbObjectType.View:
          dataObjects = WbConnection.GetSchemaCollection("Views", null, WbConnection.Schema);
          objectName = "TABLE_NAME";
          loadedObjectsList = LoadedViews;
          break;
      }

      if (dataObjects == null)
      {
        return;
      }

      loadedObjectsList.Clear();
      loadedObjectsList.AddRange(dataObjects.Rows.Cast<DataRow>().Select(dataRow => dataRow[objectName].ToString()).Select(dbObjectName => new DbObject(dbObjectName, dataObjectType)));
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
    /// <param name="includeTypes">Flags indicating what DB object types are included on the refresh.</param>
    private void RefreshDbObjectsList(bool reloadFromServer, DbObject.DbObjectType includeTypes = DbObject.ALL_DB_OBJECT_TYPES)
    {
      if (DBObjectList.HeaderNodes.Count < 3)
      {
        return;
      }

      try
      {
        // Avoids flickering of DB Objects lists while adding the items to it.
        DBObjectList.BeginUpdate();

        DBObjectList.ClearNodes();
        if (reloadFromServer)
        {
          LoadDataObjects(DbObject.DbObjectType.Table);
          LoadDataObjects(DbObject.DbObjectType.View);
          LoadDataObjects(DbObject.DbObjectType.Procedure);
        }

        // 1 - Table
        // 2 - View
        // 4 - Procedure
        foreach (DbObject.DbObjectType dbObjectType in Enum.GetValues(typeof(DbObject.DbObjectType)))
        {
          var andValue = (short)(dbObjectType & includeTypes);
          if (andValue == 0)
          {
            continue;
          }

          int imageIndex = -1;
          List<DbObject> objectsList = null;
          MySqlListViewNode parentNode = null;
          switch (dbObjectType)
          {
            case DbObject.DbObjectType.Table:
              imageIndex = 0;
              objectsList = LoadedTables;
              parentNode = DBObjectList.HeaderNodes[0];
              break;

            case DbObject.DbObjectType.View:
              imageIndex = 1;
              objectsList = LoadedViews;
              parentNode = DBObjectList.HeaderNodes[1];
              break;

            case DbObject.DbObjectType.Procedure:
              imageIndex = 2;
              objectsList = LoadedProcedures;
              parentNode = DBObjectList.HeaderNodes[2];
              break;
          }

          if (parentNode == null || objectsList == null)
          {
            continue;
          }

          foreach (var dbObject in objectsList.Where(dbObject => string.IsNullOrEmpty(Filter) || dbObject.Name.ToUpper().Contains(Filter)))
          {
            var node = DBObjectList.AddDbObjectNode(parentNode, dbObject);
            dbObject.Selected = false;
            node.ImageIndex = imageIndex;
          }
        }

        DBObjectList.ExpandAll();
        DBObjectList.Nodes[0].EnsureVisible();

        // Avoids flickering of DB Objects lists while adding the items to it.
        DBObjectList.EndUpdate();
        DBObjectList_AfterSelect(null, null);
      }
      catch (Exception ex)
      {
        MiscUtilities.ShowCustomizedErrorDialog(Resources.RefreshDBObjectsErrorTitle, ex.Message, true);
        MySqlSourceTrace.WriteAppErrorToLog(ex);
      }
    }
  }
}