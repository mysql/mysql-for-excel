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
using System.Linq;
using System.Windows.Forms;
using MySQL.ForExcel.Classes;
using MySQL.ForExcel.Forms;
using MySQL.ForExcel.Panels;
using MySQL.ForExcel.Properties;
using MySql.Utility.Classes;
using MySql.Utility.Classes.MySqlWorkbench;
using MySql.Utility.Forms;
using MySql.Utility.Structs;
using ExcelInterop = Microsoft.Office.Interop.Excel;

namespace MySQL.ForExcel.Controls
{
  /// <summary>
  /// Represents a task pane that can be used in Excel to contain controls for an add-in.
  /// </summary>
  public partial class ExcelAddInPane : UserControl
  {
    /// <summary>
    /// Initializes a new instance of the <see cref="ExcelAddInPane"/> class.
    /// </summary>
    public ExcelAddInPane()
    {
      InitializeComponent();

      UpdateExcelSelectedDataStatus(Globals.ThisAddIn.Application.Selection as ExcelInterop.Range);
      ActiveEditDialog = null;
      FirstConnectionInfo = null;
      WbConnection = null;
      WelcomePanel1.LoadConnections(true);
    }

    #region Properties

    /// <summary>
    /// Gets the active <see cref="EditDataDialog"/> used when clicking the Edit Data action label.
    /// </summary>
    [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    public EditDataDialog ActiveEditDialog { get; private set; }

    /// <summary>
    /// Gets the active <see cref="ExcelInterop.Workbook"/> unique identifier.
    /// </summary>
    [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    public string ActiveWorkbookId => Globals.ThisAddIn.ActiveWorkbook.GetOrCreateId();

    /// <summary>
    /// Gets the active <see cref="ExcelInterop.Worksheet"/> in the Excel application.
    /// </summary>
    [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    public ExcelInterop.Worksheet ActiveWorksheet => Globals.ThisAddIn.Application.ActiveSheet as ExcelInterop.Worksheet;

    /// <summary>
    /// Gets a value indicating whether the <see cref="ActiveWorksheet"/> is in edit mode.
    /// </summary>
    [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    public bool ActiveWorksheetInEditMode
    {
      get
      {
        var activeWorkbookEditConnectionInfos = WorkbookConnectionInfos.GetWorkbookEditConnectionInfos(Globals.ThisAddIn.ActiveWorkbook);
        var activeWorkSheet = ActiveWorksheet;
        return activeWorkSheet != null
                && activeWorkbookEditConnectionInfos.Exists(connectionInfo => connectionInfo.EditDialog != null && connectionInfo.EditDialog.EditingWorksheet == activeWorkSheet);
      }
    }

    /// <summary>
    /// Gets or sets the first <see cref="EditConnectionInfo"/> object.
    /// </summary>
    [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    public EditConnectionInfo FirstConnectionInfo { get; }

    /// <summary>
    /// Gets a list of stored procedures loaded in this pane.
    /// </summary>
    [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    public List<DbProcedure> LoadedProcedures => DBObjectSelectionPanel3.LoadedProcedures;

    /// <summary>
    /// Gets a list of schemas loaded in this pane.
    /// </summary>
    [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    public List<DbSchema> LoadedSchemas => SchemaSelectionPanel2.LoadedSchemas;

    /// <summary>
    /// Gets a list of tables loaded in this pane.
    /// </summary>
    [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    public List<DbTable> LoadedTables => DBObjectSelectionPanel3.LoadedTables;

    /// <summary>
    /// Gets a list of views loaded in this pane.
    /// </summary>
    [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    public List<DbView> LoadedViews => DBObjectSelectionPanel3.LoadedViews;

    /// <summary>
    /// Gets a <see cref="MySqlWorkbenchConnection"/> object representing the connection to a MySQL server instance selected by users.
    /// </summary>
    [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    public MySqlWorkbenchConnection WbConnection { get; private set; }

    #endregion Properties

    /// <summary>
    /// Exports currently selected Excel data to a new MySQL table or appends it to an existing MySQL table.
    /// </summary>
    /// <param name="toTableObject">Table to append the data to, if null exports to a new table.</param>
    /// <returns><c>true</c> if the export/append action was executed, <c>false</c> otherwise.</returns>
    public bool AppendDataToTable(DbTable toTableObject)
    {
      DialogResult dr;
      if (!(Globals.ThisAddIn.Application.Selection is ExcelInterop.Range exportRange))
      {
        return false;
      }

      if (exportRange.Areas.Count > 1)
      {
        InfoDialog.ShowDialog(InfoDialogProperties.GetWarningDialogProperties(Resources.MultipleAreasNotSupportedWarningTitle, Resources.MultipleAreasNotSupportedWarningDetail));
        return false;
      }

      Cursor = Cursors.WaitCursor;
      if (toTableObject != null)
      {
        using (var appendDataForm = new AppendDataForm(toTableObject, exportRange, ActiveWorksheet.Name))
        {
          dr = appendDataForm.ShowDialog();
        }
      }
      else
      {
        using (var exportForm = new ExportDataForm(WbConnection, exportRange, ActiveWorksheet.Name))
        {
          dr = exportForm.ShowDialog();
        }
      }

      Cursor = Cursors.Default;
      return dr == DialogResult.OK;
    }

    /// <summary>
    /// Closes the current connection, editing dialogs and puts the welcome panel in focus.
    /// </summary>
    /// <param name="givePanelFocus">Flag indicating whether the <see cref="WelcomePanel"/> is given focus.</param>
    public void CloseConnection(bool givePanelFocus)
    {
      WbConnection = null;
      if (givePanelFocus)
      {
        WelcomePanel1.BringToFront();
      }

      // Free up open Edit Dialogs
      WorkbookConnectionInfos.CloseWorkbookEditConnectionInfos(Globals.ThisAddIn.ActiveWorkbook);
    }

    /// <summary>
    /// Closes the current connection, editing tables and puts the schema panel in focus.
    /// </summary>
    /// <param name="askToCloseConnections">Flag indicating whether users are asked for confirmation before closing active <see cref="EditConnectionInfo"/>.</param>
    /// <param name="givePanelFocus">Flag indicating whether the <see cref="SchemaSelectionPanel"/> is given focus.</param>
    /// <returns><c>true</c> if the schema and its open <see cref="EditConnectionInfo"/> objects are closed, <c>false</c> otherwise.</returns>
    public bool CloseSchema(bool askToCloseConnections, bool givePanelFocus)
    {
      if (askToCloseConnections && WorkbookConnectionInfos.GetWorkbookEditConnectionInfos(Globals.ThisAddIn.ActiveWorkbook).Count > 0)
      {
        // If there are Active OldStoredEditConnectionInfos warn the users that by closing the schema the active EditConnectionInfos will be closed.
        var dr = MiscUtilities.ShowCustomizedWarningDialog(Resources.ActiveEditConnectionInfosCloseWarningTitle, Resources.ActiveEditConnectionInfosCloseWarningDetail);
        if (dr == DialogResult.No)
        {
          return false;
        }
      }

      WorkbookConnectionInfos.CloseWorkbookEditConnectionInfos(Globals.ThisAddIn.ActiveWorkbook);
      if (givePanelFocus)
      {
        SchemaSelectionPanel2.BringToFront();
      }

      return true;
    }

    /// <summary>
    /// Opens an <see cref="EditConnectionInfo"/> for a MySQL table.
    /// </summary>
    /// <param name="tableObject">Table to start an editing for.</param>
    /// <param name="fromSavedConnectionInfo">Flag indicating whether the <see cref="EditConnectionInfo"/> to be opened corresponds.</param>
    /// <param name="workbook">The workbook.</param>
    /// <returns><c>true</c> If the export/append action was executed, <c>false</c> otherwise.</returns>
    public bool EditTableData(DbTable tableObject, bool fromSavedConnectionInfo, ExcelInterop.Workbook workbook)
    {
      if (tableObject == null)
      {
        return false;
      }

      var schemaAndTableNames = WbConnection.Schema + "." + tableObject.Name;

      // Check if the current DB object has an edit ongoing
      if (TableHasEditOnGoing(tableObject.Name))
      {
        // Display an error since there is an ongoing Editing operation and return
        InfoDialog.ShowDialog(InfoDialogProperties.GetErrorDialogProperties(Resources.TaskPaneEditingNotPossibleTitleText, string.Format(Resources.TableWithOperationOngoingError, schemaAndTableNames)));
        return false;
      }

      // Preview the table's data in case the user option for that is on
      if (!fromSavedConnectionInfo && Settings.Default.EditPreviewMySqlData)
      {
        using (var previewDataDialog = new PreviewTableViewDialog(tableObject, true))
        {
          if (previewDataDialog.ShowDialog() == DialogResult.Cancel)
          {
            return false;
          }
        }
      }

      // Check if selected Table has a Primary Key, it it does not we prompt an error and exit since Editing on such table is not permitted
      if (!WbConnection.TableHasPrimaryKey(tableObject.Name))
      {
        InfoDialog.ShowDialog(InfoDialogProperties.GetErrorDialogProperties(Resources.EditOpenSatusError, Resources.EditOpenSummaryError, Resources.EditOpenDetailsError));
        return false;
      }

      // Attempt to Import Data unless the user cancels the import operation
      var proposedWorksheetName = fromSavedConnectionInfo ? tableObject.Name : Globals.ThisAddIn.ActiveWorkbook.GetWorksheetNameAvoidingDuplicates(tableObject.Name);
      tableObject.ImportParameters.ForEditDataOperation = true;
      MySqlDataTable mySqlTable;
      using (var importForm = new ImportTableViewForm(tableObject, proposedWorksheetName))
      {
        if (importForm.ImportHidingDialog() == DialogResult.Cancel)
        {
          return false;
        }

        mySqlTable = importForm.MySqlTable;
      }

      if (mySqlTable == null || mySqlTable.Columns.Count == 0)
      {
        MiscUtilities.ShowCustomizedErrorDialog(string.Format(Resources.UnableToRetrieveData, tableObject.Name));
        return false;
      }

      var activeWorkbookEditConnectionInfos = WorkbookConnectionInfos.GetWorkbookEditConnectionInfos(Globals.ThisAddIn.ActiveWorkbook);

      // Hide all other open EditDataDialog forms before opening a new one.
      if (!fromSavedConnectionInfo)
      {
        foreach (var connectionInfo in activeWorkbookEditConnectionInfos.Where(connectionInfo => connectionInfo.EditDialog != null && connectionInfo.EditDialog.Visible))
        {
          connectionInfo.EditDialog.Hide();
        }
      }

      // Create the new Excel Worksheet and import the editing data there
      var editWorkbook = fromSavedConnectionInfo && workbook != null ? workbook : Globals.ThisAddIn.ActiveWorkbook;
      var currentWorksheet = fromSavedConnectionInfo && Settings.Default.EditSessionsReuseWorksheets
        ? editWorkbook.GetOrCreateWorksheet(proposedWorksheetName, true)
        : editWorkbook.CreateWorksheet(proposedWorksheetName, true);
      if (currentWorksheet == null)
      {
        return false;
      }

      // Clear the contents of the worksheet if we are restoring a saved <see cref="EditConnectionInfo"/> since the user may have input data into it.
      if (fromSavedConnectionInfo)
      {
        currentWorksheet.UsedRange.Clear();
      }

      // Create and show the Edit Data Dialog
      var editConnectionInfo = GetEditConnectionInfo(mySqlTable, currentWorksheet);
      ActiveEditDialog = editConnectionInfo.EditDialog;
      if (fromSavedConnectionInfo)
      {
        // If restoring EditConnectionInfo objects we need to create and link their corresponding EditDialog to it.
        var editConnectionInfoBeingRestored = activeWorkbookEditConnectionInfos.FirstOrDefault(connectionInfo => connectionInfo.TableName.Equals(editConnectionInfo.TableName));
        if (editConnectionInfoBeingRestored != null)
        {
          editConnectionInfoBeingRestored.EditDialog = editConnectionInfo.EditDialog;
        }
      }
      else
      {
        ActiveEditDialog.ShowDialog();

        // If not restoring EditConnectionInfo objects we need to add the manually triggered EditConnectionInfo to the list of the active workbook.
        activeWorkbookEditConnectionInfos.Add(editConnectionInfo);
      }

      return true;
    }

    /// <summary>
    /// Sets and opens the current active connection used to browse schemas and DB objects.
    /// </summary>
    /// <param name="connection">A <see cref="MySqlWorkbenchConnection"/> object representing the connection to a MySQL server instance selected by users.</param>
    /// <param name="givePanelFocus">Flag indicating whether the <see cref="SchemaSelectionPanel"/> is given focus.</param>
    public PasswordDialogFlags OpenConnection(MySqlWorkbenchConnection connection, bool givePanelFocus)
    {
      WbConnection = connection;
      var passwordFlags = WbConnection.TestConnectionAndRetryOnWrongPassword();
      if (passwordFlags.ConnectionSuccess && SchemaSelectionPanel2.SetConnection(WbConnection) && givePanelFocus)
      {
        RefreshWbConnectionTimeouts();
        SchemaSelectionPanel2.BringToFront();
      }

      return passwordFlags;
    }

    /// <summary>
    /// Sets the active Schema and puts the DB Objects Selection Panel in focus.
    /// </summary>
    /// <param name="schema">Schema name.</param>
    /// <param name="givePanelFocus">Flag indicating whether the <see cref="DbObjectSelectionPanel"/> is given focus.</param>
    public void OpenSchema(string schema, bool givePanelFocus)
    {
      if (DBObjectSelectionPanel3.SetConnection(WbConnection, schema) && givePanelFocus)
      {
        DBObjectSelectionPanel3.BringToFront();
      }
    }

    /// <summary>
    /// Refreshes the availability of action labels linked to a table with the given name.
    /// </summary>
    /// <param name="tableName">Name of the table with status update.</param>
    /// <param name="editActive">Flag indicating if the Edit Data action is enabled for a table with the given name.</param>
    public void RefreshDbObjectPanelActionLabelsEnabledStatus(string tableName, bool editActive)
    {
      DBObjectSelectionPanel3.RefreshActionLabelsEnabledStatus(tableName, editActive);
    }

    /// <summary>
    /// Refreshes the availability of action labels linked to a table with the given name.
    /// </summary>
    /// <param name="tableName">Name of the table with status update.</param>
    public void RefreshDbObjectPanelActionLabelsEnabledStatus(string tableName)
    {
      var editActive = TableHasEditOnGoing(tableName);
      RefreshDbObjectPanelActionLabelsEnabledStatus(tableName, editActive);
    }

    /// <summary>
    /// Refreshes the availability of action labels linked to the currently selected table.
    /// </summary>
    public void RefreshDbObjectPanelActionLabelsEnabledStatus()
    {
      if (DBObjectSelectionPanel3.CurrentSelectedDbObject != null)
      {
        RefreshDbObjectPanelActionLabelsEnabledStatus(DBObjectSelectionPanel3.CurrentSelectedDbObject.Name);
      }
    }

    /// <summary>
    /// Refreshes the connection timeout values from the settings file.
    /// </summary>
    public void RefreshWbConnectionTimeouts()
    {
      if (WbConnection == null)
      {
        return;
      }

      WbConnection.ConnectionTimeout = Settings.Default.GlobalConnectionConnectionTimeout;
      WbConnection.DefaultCommandTimeout = Settings.Default.GlobalConnectionCommandTimeout;
      WbConnection.SetClientSessionReadWriteTimeouts();
    }

    /// <summary>
    /// Checks if there is an Editing Operation active for a table with the given name.
    /// </summary>
    /// <param name="tableName">Name of the table.</param>
    /// <returns><c>true</c> if the table has is in editing mode, <c>false</c> otherwise.</returns>
    public bool TableHasEditOnGoing(string tableName)
    {
      var activeWorkbook = Globals.ThisAddIn.ActiveWorkbook;
      var activeWorkbookEditConnectionInfos = WorkbookConnectionInfos.GetWorkbookEditConnectionInfos(activeWorkbook);
      if (activeWorkbookEditConnectionInfos.Count == 0)
      {
        return false;
      }

      var editContainer = activeWorkbookEditConnectionInfos.FirstOrDefault(ac => ac.EditDialog != null && ac.TableName == tableName);
      if (editContainer == null)
      {
        return false;
      }

      // Means has an edit ongoing we need to make sure the edit has a valid sheet otherwise we need to release it
      if (Globals.ThisAddIn.Application.Worksheets.Cast<ExcelInterop.Worksheet>().Contains(editContainer.EditDialog.EditingWorksheet))
      {
        return true;
      }

      editContainer.EditDialog.Close();
      return false;
    }

    /// <summary>
    /// Checks if the selected <see cref="ExcelInterop.Range"/> contains any data in it and updates that status in the corresponding panel.
    /// </summary>
    /// <param name="range">The <see cref="ExcelInterop.Range"/> where the selection is.</param>
    public void UpdateExcelSelectedDataStatus(ExcelInterop.Range range)
    {
      if (!Visible)
      {
        return;
      }

      DBObjectSelectionPanel3.ExcelSelectionContainsData = range.ContainsAnyData();
    }

    /// <summary>
    /// Creates the <see cref="EditConnectionInfo"/> or restores the saved one.
    /// </summary>
    /// <param name="mySqlTable">The <see cref="MySqlDataTable"/> used for the Edit Data session.</param>
    /// <param name="currentWorksheet">The current worksheet.</param>
    /// <returns>A new or restored <see cref="EditConnectionInfo"/> object.</returns>
    private EditConnectionInfo GetEditConnectionInfo(MySqlDataTable mySqlTable, ExcelInterop.Worksheet currentWorksheet)
    {
      if (mySqlTable == null || currentWorksheet == null)
      {
        return null;
      }

      var atCell = currentWorksheet.Range["A1", Type.Missing];
      var editingRange = mySqlTable.ImportDataIntoExcelRange(atCell);
      EditConnectionInfo connectionInfo = null;

      var workbookEditConnectionInfos = WorkbookConnectionInfos.GetWorkbookEditConnectionInfos(Globals.ThisAddIn.ActiveWorkbook);
      if (workbookEditConnectionInfos.Count > 0)
      {
        connectionInfo = workbookEditConnectionInfos.GetActiveEditConnectionInfo(mySqlTable.TableName);
      }

      // The EditConnectionInfo is new and has to be created from scratch.
      if (connectionInfo == null)
      {
        var activeWorkbook = Globals.ThisAddIn.ActiveWorkbook;
        connectionInfo = new EditConnectionInfo(activeWorkbook.GetOrCreateId(), activeWorkbook.FullName, WbConnection.Id, WbConnection.Schema, mySqlTable.TableName);
      }

      if (connectionInfo.EditDialog != null)
      {
        return connectionInfo;
      }

      // The EditConnectionInfo is being either restored from the settings file or created for the newborn object.
      connectionInfo.EditDialog = new EditDataDialog(this, new NativeWindowWrapper(Globals.ThisAddIn.Application.Hwnd), WbConnection, editingRange, mySqlTable, currentWorksheet);
      currentWorksheet.StoreProtectionKey(connectionInfo.EditDialog.WorksheetProtectionKey);
      return connectionInfo;
    }
  }
}