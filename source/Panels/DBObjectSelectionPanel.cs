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
using MySQL.ForExcel.Classes;
using MySQL.ForExcel.Controls;
using MySQL.ForExcel.Forms;
using MySQL.ForExcel.Properties;
using MySQL.Utility.Classes;
using MySQL.Utility.Classes.MySQLWorkbench;
using Excel = Microsoft.Office.Interop.Excel;

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

      InheritFontToControlsExceptionList.Add(ExportToNewTableHotLabel.Name);
      InheritFontToControlsExceptionList.Add(SelectDatabaseObjectHotLabel.Name);
      InheritFontToControlsExceptionList.Add(ImportDataHotLabel.Name);
      InheritFontToControlsExceptionList.Add(EditDataHotLabel.Name);
      InheritFontToControlsExceptionList.Add(AppendDataHotLabel.Name);

      DBObjectList.AddNode(null, "Tables");
      DBObjectList.AddNode(null, "Views");
      DBObjectList.AddNode(null, "Procedures");
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
        if (DBObjectList.Nodes.Count > 0 && DBObjectList.SelectedNode != null && DBObjectList.SelectedNode.Level > 0)
        {
          return (DBObjectList.SelectedNode.Tag as DbObject);
        }
        
        return null;
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
        RefreshDbObjectsList();
        DBObjectList_AfterSelect(null, null);
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
      DbObject dbObj = CurrentSelectedDbObject;
      if (dbObj == null || Parent == null || (!string.IsNullOrEmpty(tableName) && dbObj.Name != tableName))
      {
        return;
      }

      ImportDataHotLabel.Enabled = true;
      EditDataHotLabel.Enabled = dbObj.Type == DbObject.DbObjectType.Table && !editActive;
      AppendDataHotLabel.Enabled = dbObj.Type == DbObject.DbObjectType.Table && ExcelSelectionContainsData;
    }

    /// <summary>
    /// Event delegate method fired when <see cref="AppendDataHotLabel"/> is clicked.
    /// </summary>
    /// <param name="sender">Sender object.</param>
    /// <param name="e">Event arguments.</param>
    private void AppendDataHotLabel_Click(object sender, EventArgs e)
    {
      if (DBObjectList.SelectedNode == null || WbConnection == null)
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
        DbObject selDbObject = DBObjectList.SelectedNode.Tag as DbObject;
        if (selDbObject != null && selDbObject.Type == DbObject.DbObjectType.Table)
        {
          ExportDataToTable(selDbObject);
        }
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
      var editPane = Parent as ExcelAddInPane;
      var editActive = e!=null && editPane != null && editPane.TableHasEditOnGoing(e.Node.Text);
      RefreshActionLabelsEnabledStatus(null, editActive);
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
        RefreshDbObjectsList();
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
      DbObject selDbObject = DBObjectList.SelectedNode != null ? DBObjectList.SelectedNode.Tag as DbObject : null;
      if (selDbObject == null || selDbObject.Type != DbObject.DbObjectType.Table)
      {
        return;
      }

      PasswordDialogFlags passwordFlags = WbConnection.TestConnectionAndRetryOnWrongPassword();
      if (!passwordFlags.ConnectionSuccess)
      {
        return;
      }

      try
      {
        var excelAddInPane = Parent as ExcelAddInPane;
        EditDataHotLabel.Enabled = excelAddInPane != null && !excelAddInPane.EditTableData(selDbObject, false, null);
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
      PasswordDialogFlags passwordFlags = WbConnection.TestConnectionAndRetryOnWrongPassword();
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
      LoadDataObjects(DbObject.DbObjectType.Table);
      DBObjectList_AfterSelect(DBObjectList, new TreeViewEventArgs(null));
    }

    /// <summary>
    /// Event delegate method fired when <see cref="ImportDataHotLabel"/> is clicked.
    /// </summary>
    /// <param name="sender">Sender object.</param>
    /// <param name="e">Event arguments.</param>
    private void ImportDataHotLabel_Click(object sender, EventArgs e)
    {
      if (DBObjectList.SelectedNode == null)
      {
        return;
      }

      ExcelAddInPane parentTaskPane = (Parent as ExcelAddInPane);
      if (parentTaskPane == null)
      {
        return;
      }

      DbObject dbo = DBObjectList.SelectedNode.Tag as DbObject;
      if (dbo == null)
      {
        return;
      }

      PasswordDialogFlags passwordFlags = WbConnection.TestConnectionAndRetryOnWrongPassword();
      if (!passwordFlags.ConnectionSuccess)
      {
        return;
      }

      if (parentTaskPane.ActiveWorksheetInEditMode)
      {
        DialogResult dr = MiscUtilities.ShowCustomizedWarningDialog(Resources.WorksheetInEditModeWarningTitle, Resources.WorksheetInEditModeWarningDetail);
        if (dr != DialogResult.Yes)
        {
          return;
        }

        Excel.Worksheet newWorksheet = parentTaskPane.ActiveWorkbook.CreateWorksheet(dbo.Name, true);
        if (newWorksheet == null)
        {
          return;
        }
      }

      try
      {
        switch (dbo.Type)
        {
          case DbObject.DbObjectType.Table:
          case DbObject.DbObjectType.View:
            ImportTableOrView(dbo);
            break;

          case DbObject.DbObjectType.Procedure:
            ImportProcedure(dbo);
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

      ImportProcedureForm importProcedureForm = new ImportProcedureForm(WbConnection, dbo, addInPane.ActiveWorksheet.Name, addInPane.ActiveWorkbook.Excel8CompatibilityMode);
      DialogResult dr = importProcedureForm.ShowDialog();
      if (dr == DialogResult.Cancel)
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

    /// <summary>
    /// Imports data from the selected table or view DB object to the current Excel worksheet.
    /// </summary>
    /// <param name="dbo">DB object.</param>
    private void ImportTableOrView(DbObject dbo)
    {
      var taskPaneControl = (ExcelAddInPane)Parent;
      ImportTableViewForm importForm = new ImportTableViewForm(WbConnection, dbo, taskPaneControl.ActiveWorkbook.ActiveSheet.Name, taskPaneControl.ActiveWorkbook.Excel8CompatibilityMode, false);

      DialogResult dr = importForm.ShowDialog();
      if (dr == DialogResult.Cancel)
      {
        return;
      }

      if (importForm.ImportDataTable == null)
      {
        MiscUtilities.ShowCustomizedErrorDialog(string.Format(Resources.UnableToRetrieveData, dbo.Type.ToString().ToLowerInvariant(), dbo.Name));
        return;
      }

      importForm.ImportDataTable.ImportDataAtActiveExcelCell(importForm.ImportHeaders);
    }

    /// <summary>
    /// Fetches all DB object names of the given type from the current connection and loads them in the <see cref="DBObjectList"/> tree.
    /// </summary>
    /// <param name="dataObjectType">Type of DB object to load.</param>
    private void LoadDataObjects(DbObject.DbObjectType dataObjectType)
    {
      DataTable dataObjects = null;
      TreeNode parentNode = null;
      string objectName = string.Empty;
      List<DbObject> loadedObjectsList = null;

      // 0 - Tables
      // 1 - Views
      // 2 - Procedures
      int objectIndex = 0;
      switch (dataObjectType)
      {
        case DbObject.DbObjectType.Procedure:
          objectIndex = 2;
          dataObjects = WbConnection.GetSchemaCollection("Procedures", null, WbConnection.Schema, null, "PROCEDURE");
          objectName = "ROUTINE_NAME";
          parentNode = DBObjectList.Nodes[objectIndex];
          loadedObjectsList = LoadedProcedures;
          break;

        case DbObject.DbObjectType.Table:
          dataObjects = WbConnection.GetSchemaCollection("Tables", null, WbConnection.Schema);
          objectName = "TABLE_NAME";
          parentNode = DBObjectList.Nodes[objectIndex];
          loadedObjectsList = LoadedTables;
          break;

        case DbObject.DbObjectType.View:
          objectIndex = 1;
          dataObjects = WbConnection.GetSchemaCollection("Views", null, WbConnection.Schema);
          objectName = "TABLE_NAME";
          parentNode = DBObjectList.Nodes[objectIndex];
          loadedObjectsList = LoadedViews;
          break;
      }

      if (dataObjects == null)
      {
        return;
      }

      loadedObjectsList.Clear();
      foreach (string dbObjectName
                in dataObjects.Rows.Cast<DataRow>()
                .Select(dataRow => dataRow[objectName].ToString())
                .Where(dbObjectName => string.IsNullOrEmpty(Filter) || dbObjectName.ToUpper().Contains(Filter)))
      {
        TreeNode node = DBObjectList.AddNode(parentNode, dbObjectName);
        var dbObj = new DbObject(dbObjectName, dataObjectType);
        node.Tag = dbObj;
        loadedObjectsList.Add(dbObj);
        node.ImageIndex = objectIndex;
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
    /// Refreshes the DB objects list control with current objects in the connected schema.
    /// </summary>
    private void RefreshDbObjectsList()
    {
      // Avoids flickering of DB Objects lists while adding the items to it.
      DBObjectList.BeginUpdate();

      foreach (TreeNode node in DBObjectList.Nodes)
      {
        node.Nodes.Clear();
      }

      LoadDataObjects(DbObject.DbObjectType.Table);
      LoadDataObjects(DbObject.DbObjectType.View);
      LoadDataObjects(DbObject.DbObjectType.Procedure);

      if (DBObjectList.Nodes[0].GetNodeCount(true) > 0)
      {
        DBObjectList.Nodes[0].Expand();
      }

      // Avoids flickering of DB Objects lists while adding the items to it.
      DBObjectList.EndUpdate();
    }

    /// <summary>
    /// Event delegate method fired when <see cref="RefreshDatabaseObjectsToolStripMenuItem"/> is clicked.
    /// </summary>
    /// <param name="sender">Sender object.</param>
    /// <param name="e">Event arguments.</param>
    private void RefreshDatabaseObjectsToolStripMenuItem_Click(object sender, EventArgs e)
    {
      try
      {
        RefreshDbObjectsList();
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