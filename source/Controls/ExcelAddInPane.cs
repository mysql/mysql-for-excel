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
using System.Text;
using System.Windows.Forms;
using MySQL.ForExcel.Classes;
using MySQL.ForExcel.Forms;
using MySQL.ForExcel.Properties;
using MySQL.Utility.Classes;
using MySQL.Utility.Classes.MySQLWorkbench;
using MySQL.Utility.Forms;
using Excel = Microsoft.Office.Interop.Excel;

namespace MySQL.ForExcel.Controls
{
  /// <summary>
  /// Represents a task pane that can be used in Excel to contain controls for an add-in.
  /// </summary>
  public partial class ExcelAddInPane : UserControl
  {

    /// <summary>
    /// True while restoring existing sessions for the current workbook, avoiding unwanted actions to be raised during the process.
    /// </summary>
    private bool _restoringExistingSessions;

    /// <summary>
    /// Initializes a new instance of the <see cref="ExcelAddInPane"/> class.
    /// </summary>
    public ExcelAddInPane()
    {
      _restoringExistingSessions = false;
      InitializeComponent();

      DBObjectSelectionPanel3.ExcelSelectionContainsData = false;
      ActiveEditDialog = null;
      FirstSession = null;
      LastDeactivatedSheetName = string.Empty;
      LastDeactivatedWorkbookName = string.Empty;
      WbConnection = null;
      RestoreEditSessions(ActiveWorkbook);
    }

    #region Properties

    /// <summary>
    /// Gets the active <see cref="EditDataDialog"/> used when clicking the Edit Data action label.
    /// </summary>
    [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    public EditDataDialog ActiveEditDialog { get; private set; }

    /// <summary>
    /// Gets the active <see cref="Excel.Workbook"/> in the Excel application.
    /// </summary>
    [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    public Excel.Workbook ActiveWorkbook
    {
      get
      {
        return Globals.ThisAddIn.Application.ActiveWorkbook ?? Globals.ThisAddIn.Application.Workbooks.Add(1);
      }
    }

    /// <summary>
    /// Gets the active <see cref="Excel.Workbook"/> unique identifier.
    /// </summary>
    [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    public string ActiveWorkbookId
    {
      get
      {
        return ActiveWorkbook.GetOrCreateId();
      }
    }

    /// <summary>
    /// Gets the active <see cref="Excel.Worksheet"/> in the Excel application.
    /// </summary>
    [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    public Excel.Worksheet ActiveWorksheet
    {
      get
      {
        return Globals.ThisAddIn.Application.ActiveSheet as Excel.Worksheet;
      }
    }

    /// <summary>
    /// Gets a value indicating whether the <see cref="ActiveWorksheet"/> is in edit mode.
    /// </summary>
    [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    public bool ActiveWorksheetInEditMode
    {
      get
      {
        return ActiveWorksheet != null
                && WorkbookEditSessions != null
                && WorkbookEditSessions.Count > 0
                && WorkbookEditSessions.Exists(session => session.EditDialog != null && session.EditDialog.EditingWorksheet == ActiveWorksheet);
      }
    }

    /// <summary>
    /// Gets or sets the first session of Edit sessions.
    /// </summary>
    [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    public EditSessionInfo FirstSession { get; private set; }

    /// <summary>
    /// Gets the name of the last deactivated Excel <see cref="Excel.Worksheet"/>.
    /// </summary>
    [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    public string LastDeactivatedSheetName { get; private set; }

    /// <summary>
    /// Gets the name of the last deactivated Excel <see cref="Excel.Workbook"/>.
    /// </summary>
    [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    public string LastDeactivatedWorkbookName { get; private set; }

    /// <summary>
    /// Gets a <see cref="MySqlWorkbenchConnection"/> object representing the connection to a MySQL server instance selected by users.
    /// </summary>
    [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    public MySqlWorkbenchConnection WbConnection { get; private set; }

    /// <summary>
    /// Gets or sets the List of Edit sessions.
    /// </summary>
    [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    public List<EditSessionInfo> WorkbookEditSessions { get; private set; }

    #endregion Properties

    /// <summary>
    /// Exports currently selected Excel data to a new MySQL table or appends it to an existing MySQL table.
    /// </summary>
    /// <param name="toTableObject">Table to append the data to, if null exports to a new table.</param>
    /// <returns><c>true</c> if the export/append action was executed, <c>false</c> otherwise.</returns>
    public bool AppendDataToTable(DbObject toTableObject)
    {
      DialogResult dr;
      Excel.Range exportRange = Globals.ThisAddIn.Application.Selection as Excel.Range;
      if (exportRange == null)
      {
        return false;
      }

      if (exportRange.Areas.Count > 1)
      {
        InfoDialog.ShowWarningDialog(Resources.MultipleAreasNotSupportedWarningTitle, Resources.MultipleAreasNotSupportedWarningDetail);
        return false;
      }

      if (toTableObject != null)
      {
        Cursor = Cursors.WaitCursor;
        AppendDataForm appendDataForm = new AppendDataForm(WbConnection, exportRange, toTableObject, ActiveWorksheet.Name);
        Cursor = Cursors.Default;
        dr = appendDataForm.ShowDialog();
      }
      else
      {
        Cursor = Cursors.WaitCursor;
        ExportDataForm exportForm = new ExportDataForm(WbConnection, exportRange, ActiveWorksheet.Name);
        Cursor = Cursors.Default;
        dr = exportForm.ShowDialog();
      }

      return dr == DialogResult.OK;
    }

    /// <summary>
    /// Shows or hides the <see cref="EditDataDialog"/> window associated to the given <see cref="Excel.Worksheet"/>.
    /// </summary>
    /// <param name="workSheet">A <see cref="Excel.Worksheet"/> object.</param>
    /// <param name="show">Flag indicating if the dialog will be shown or hidden.</param>
    public void ChangeEditDialogVisibility(Excel.Worksheet workSheet, bool show)
    {
      if (workSheet == null || WorkbookEditSessions.Count < 1 || _restoringExistingSessions)
      {
        return;
      }

      var activeSession = WorkbookEditSessions.GetActiveEditSession(workSheet);
      if (activeSession == null)
      {
        return;
      }

      if (show)
      {
        activeSession.EditDialog.ShowDialog();
      }
      else
      {
        activeSession.EditDialog.Hide();
      }
    }

    /// <summary>
    /// Closes and removes all Edit sessions associated to the given <see cref="Excel.Workbook"/>.
    /// </summary>
    /// <param name="workbook">The <see cref="Excel.Workbook"/> associated to the Edit sessions to close.</param>
    public void CloseWorkbookEditSessions(Excel.Workbook workbook)
    {
      if (workbook == null)
      {
        return;
      }

      var sessionsToFreeResources = WorkbookEditSessions.FindAll(session => session.EditDialog != null && string.Equals(session.EditDialog.WorkbookName, workbook.Name, StringComparison.InvariantCulture));
      foreach (var session in sessionsToFreeResources)
      {
        // The Close method is both closing the dialog and removing itself from the collection of EditSessionInfo objects.
        session.EditDialog.Close();
      }
    }

    /// <summary>
    /// Closes and removes the Edit session associated to the given <see cref="Excel.Worksheet"/>.
    /// </summary>
    /// <param name="worksheet">The <see cref="Excel.Worksheet"/> associated to the Edit session to close.</param>
    public void CloseWorksheetEditSession(Excel.Worksheet worksheet)
    {
      var wsSession = WorkbookEditSessions.FirstOrDefault(session => session.EditDialog.WorkbookName == worksheet.GetParentWorkbookName() &&
      string.Equals(session.EditDialog.WorksheetName, worksheet.Name, StringComparison.InvariantCulture));
      if (wsSession == null)
      {
        return;
      }

      wsSession.EditDialog.Close();
      if (WorkbookEditSessions.Contains(wsSession))
      {
        WorkbookEditSessions.Remove(wsSession);
      }
    }

    /// <summary>
    /// Closes the current connection, editing sessions and puts the welcome panel in focus.
    /// </summary>
    public void CloseConnection()
    {
      WbConnection = null;
      WelcomePanel1.BringToFront();

      // Free up open Edit Dialogs
      CloseWorkbookEditSessions(ActiveWorkbook);
    }

    /// <summary>
    /// Closes the current connection, editing sessions and puts the schema panel in focus.
    /// </summary>
    public void CloseSchema()
    {
      // If there are Active Edit sessions warn the users that by closing the schema the sessions will be terminated
      if (WorkbookEditSessions != null && WorkbookEditSessions.Count > 0)
      {
        DialogResult dr = MiscUtilities.ShowCustomizedWarningDialog(Resources.ActiveEditingSessionsCloseWarningTitle, Resources.ActiveEditingSessionsCloseWarningDetail);
        if (dr == DialogResult.No)
        {
          return;
        }

        CloseWorkbookEditSessions(ActiveWorkbook);
      }

      SchemaSelectionPanel2.BringToFront();
    }

    /// <summary>
    /// Delete the closed workbook's edit sessions from the settings file.
    /// </summary>
    private void DeleteCurrentWorkbookEditSessions(Excel.Workbook workbook)
    {
      if (WorkbookEditSessions == null || string.IsNullOrEmpty(workbook.GetOrCreateId()))
      {
        return;
      }

      if (!_restoringExistingSessions)
      {
        // Remove all sessions from the current workbook.
        foreach (var session in Globals.ThisAddIn.StoredEditSessions.FindAll(session => ActiveWorkbook != null && session.WorkbookGuid.Equals(workbook.GetOrCreateId())))
        {
          Globals.ThisAddIn.StoredEditSessions.Remove(session);
        }
      }

      WorkbookEditSessions = new List<EditSessionInfo>();
      Settings.Default.Save();
    }

    /// <summary>
    /// Opens an editing session for a MySQL table.
    /// </summary>
    /// <param name="tableObject">Table to start an editing session for.</param>
    /// <param name="showImportDialog">Indicates whether to open the import dialog for the user when MySQL for Excel its not (silently) restoring sessions.</param>
    /// <param name="workbook">The workbook.</param>
    /// <returns>
    ///   <c>true</c> If the export/append action was executed, <c>false</c> otherwise.
    /// </returns>
    public bool EditTableData(DbObject tableObject, bool showImportDialog, Excel.Workbook workbook)
    {
      string schemaAndTableNames = WbConnection.Schema + "." + tableObject.Name;

      // Check if the current dbobject has an edit ongoing
      if (TableHasEditOnGoing(tableObject.Name))
      {
        // Display an error since there is an ongoing Editing operation and return
        InfoDialog.ShowErrorDialog(Resources.TaskPaneEditingNotPossibleTitleText, string.Format(Resources.TableWithOperationOngoingError, schemaAndTableNames));
        return false;
      }

      // Check if selected Table has a Primary Key, it it does not we prompt an error and exit since Editing on such table is not permitted
      if (!WbConnection.TableHasPrimaryKey(tableObject.Name))
      {
        InfoDialog.ShowErrorDialog(Resources.EditOpenSatusError, Resources.EditOpenSummaryError, Resources.EditOpenDetailsError);
        return false;
      }

      // Attempt to Import Data unless the user cancels the import operation
      string proposedWorksheetName = _restoringExistingSessions ? tableObject.Name : ActiveWorkbook.GetWorksheetNameAvoidingDuplicates(tableObject.Name);
      ImportTableViewForm importForm = new ImportTableViewForm(WbConnection, tableObject, proposedWorksheetName, ActiveWorkbook.Excel8CompatibilityMode, true);
      DialogResult dr = showImportDialog ? importForm.ImportHidingDialog() : importForm.ShowDialog();
      if (dr == DialogResult.Cancel)
      {
        importForm.Dispose();
        return false;
      }

      if (importForm.ImportDataTable == null || importForm.ImportDataTable.Columns.Count == 0)
      {
        MiscUtilities.ShowCustomizedErrorDialog(string.Format(Resources.UnableToRetrieveData, tableObject.Name));
        importForm.Dispose();
        return false;
      }

      // Hide all other open EditDataDialog forms before opening a new one.
      foreach (var session in WorkbookEditSessions.Where(session => session.EditDialog != null && session.EditDialog.Visible))
      {
        session.EditDialog.Hide();
      }

      // Create the new Excel Worksheet and import the editing data there
      Excel.Worksheet currentWorksheet = _restoringExistingSessions ? workbook.GetOrCreateWorksheet(proposedWorksheetName, true) : ActiveWorkbook.CreateWorksheet(proposedWorksheetName, true);
      if (currentWorksheet == null)
      {
        importForm.Dispose();
        return false;
      }

      // Create and show the Edit Data Dialog
      var editSession = GetEditSession(tableObject, importForm, currentWorksheet);
      ActiveEditDialog = editSession.EditDialog;
      if (!_restoringExistingSessions)
      {
        ActiveEditDialog.ShowDialog();
      }

      // When restoring sessions (the import dialog was shown) the table being opened is already on the list.
      if (WorkbookEditSessions != null)
      {
        if (!_restoringExistingSessions && !WorkbookEditSessions.Contains(editSession))
        {
          WorkbookEditSessions.Add(editSession);
        }
        else
        {
          var editSessionBeingRestored = WorkbookEditSessions.FirstOrDefault(session => session != null &&
           session.TableName.Equals(editSession.TableName));
          if (editSessionBeingRestored != null)
          {
            editSessionBeingRestored.EditDialog = editSession.EditDialog;
          }
        }
      }

      importForm.Dispose();
      return true;
    }

    /// <summary>
    /// Imports data contained in the given <see cref="DataSet"/> object to the active Excel <see cref="Excel.Worksheet"/>.
    /// </summary>
    /// <param name="ds">The dataset containing the data to import to Excel.</param>
    /// <param name="importColumnNames">Flag indicating if column names will be imported as the first row of imported data.</param>
    /// <param name="importType">Indicates how to arrange multiple resultsets in the active Excel <see cref="Excel.Worksheet"/>.</param>
    /// <param name="selectedResultSet">Number of resultset to import when the <see cref="importType"/> is ImportMultipleType.SelectedResultSet.</param>
    public void ImportDataToExcel(DataSet ds, bool importColumnNames, ImportProcedureForm.ImportMultipleType importType, int selectedResultSet)
    {
      Excel.Range atCell = Globals.ThisAddIn.Application.ActiveCell;

      int tableIdx = 0;
      foreach (MySqlDataTable mySqlTable in ds.Tables)
      {
        if (importType == ImportProcedureForm.ImportMultipleType.SelectedResultSet && selectedResultSet < tableIdx)
        {
          continue;
        }

        tableIdx++;
        Excel.Range fillingRange = mySqlTable.ImportDataAtGivenExcelCell(importColumnNames, atCell);
        Excel.Range endCell;
        if (fillingRange != null)
        {
          endCell = fillingRange.Cells[fillingRange.Rows.Count, fillingRange.Columns.Count] as Excel.Range;
        }
        else
        {
          continue;
        }

        if (endCell == null || tableIdx >= ds.Tables.Count)
        {
          continue;
        }

        switch (importType)
        {
          case ImportProcedureForm.ImportMultipleType.AllResultSetsHorizontally:
            atCell = endCell.Offset[atCell.Row - endCell.Row, 2];
            break;

          case ImportProcedureForm.ImportMultipleType.AllResultSetsVertically:
            if (ActiveWorkbook.Excel8CompatibilityMode && endCell.Row + 2 > UInt16.MaxValue)
            {
              return;
            }

            atCell = endCell.Offset[2, atCell.Column - endCell.Column];
            break;
        }
      }
    }

    /// <summary>
    /// Sets and opens the current active connection used to browse schemas and DB objects.
    /// </summary>
    /// <param name="connection">A <see cref="MySqlWorkbenchConnection"/> object representing the connection to a MySQL server instance selected by users.</param>
    public PasswordDialogFlags OpenConnection(MySqlWorkbenchConnection connection)
    {
      WbConnection = connection;
      RefreshWbConnectionTimeouts();
      var passwordFlags = WbConnection.TestConnectionAndRetryOnWrongPassword();
      if (passwordFlags.ConnectionSuccess && SchemaSelectionPanel2.SetConnection(WbConnection))
      {
        SchemaSelectionPanel2.BringToFront();
      }
      return passwordFlags;
    }

    /// <summary>
    /// Checks for saved editing sessions, promts the user for action and reopens them if told to.
    /// </summary>
    /// <param name="workbook">The workbook.</param>
    public void RestoreEditSessions(Excel.Workbook workbook)
    {
      // First, we load the sessions from the settings file into memory. Since we'll only work with those related to the current workbook we find the subset that matches that cryteria.      
      WorkbookEditSessions = Globals.ThisAddIn.StoredEditSessions.FindAll(session => session != null && session.WorkbookGuid != null && session.WorkbookGuid.Equals(workbook.GetOrCreateId()));
      if (!Settings.Default.EditRestoreEditSessions || string.IsNullOrEmpty(workbook.Name) || WorkbookEditSessions.Count < 1)
      {
        return;
      }

      // In case we have some saved sessions for the current Workbook, we verify we can re-open them.
      FirstSession = WorkbookEditSessions.FirstOrDefault(session => session != null);
      if (FirstSession == null)
      {
        return;
      }

      var wbSessionConnection = MySqlWorkbench.Connections.GetConnectionForId(FirstSession.ConnectionId);
      if (wbSessionConnection == null || !OpenConnection(wbSessionConnection).ConnectionSuccess)
      {
        // If the session's workbench connection was deleted, no session could be loaded.
        var dialogResult = MiscUtilities.ShowCustomizedWarningDialog(Resources.EditReopeningWBConnectionNoLongerExistsFailedTitle, Resources.EditReopeningWBConnectionNoLongerExistsFailedDetail);
        if (dialogResult == DialogResult.Yes)
        {
          DeleteCurrentWorkbookEditSessions(workbook);
        }
        return;
      }

      var wbSessionHostIdentifier = wbSessionConnection.HostIdentifier;
      WbConnection = WbConnection ?? wbSessionConnection;
      var enableOpenSessions = true;
      var currentSchema = WbConnection.Schema;
      // If the Excel version is higher than 2013 or there's no currentSchema selected, we need to open the one the session specifies.
      if (Globals.ThisAddIn.ExcelVersionNumber >= 15 || string.IsNullOrEmpty(currentSchema))
      {
        OpenSchema(FirstSession.SchemaName);
      }

      // Otherwise, we verify the selected schema is the same than the one we are trying to restore, if it is not, we cannot reopen them.
      else if (!string.Equals(wbSessionHostIdentifier, WbConnection.HostIdentifier, StringComparison.InvariantCulture)
      || !string.Equals(currentSchema, FirstSession.SchemaName, StringComparison.InvariantCulture))
      {
        enableOpenSessions = false;
      }

      // Verify the session's schema still exists in the data base, if the schema was deleted, no session could be loaded.
      if (SchemaSelectionPanel2.SchemasList.Nodes.GetNode(FirstSession.SchemaName) == null)
      {
        var errorMessage = string.Format(Resources.EditReopeningSchemaNoLongerExistsFailed, wbSessionHostIdentifier, FirstSession.SchemaName);
        MiscUtilities.ShowCustomizedInfoDialog(InfoDialog.InfoType.Error, errorMessage);
        MySqlSourceTrace.WriteToLog(errorMessage);
        return;
      }

      switch (new OpenEditingSessionsDialog(enableOpenSessions, FirstSession.SchemaName, currentSchema).ShowDialog())
      {
        case DialogResult.Abort:
          // Discard: Do not open any and delete all saved sessions for the current workbook.
          DeleteCurrentWorkbookEditSessions(workbook);
          break;

        case DialogResult.Yes:
          OpenEditSessionTables(workbook);
          break;
      }
    }

    /// <summary>
    /// Sets the active Schema and puts the DB Objects Selection Panel in focus.
    /// </summary>
    /// <param name="schema">Schema name.</param>
    public void OpenSchema(string schema)
    {
      WbConnection.Schema = schema;
      DBObjectSelectionPanel3.WbConnection = WbConnection;
      DBObjectSelectionPanel3.BringToFront();
    }

    /// <summary>
    /// Opens the Edit session's tables.
    /// </summary>
    /// <param name="workbook">The workbook.</param>
    private void OpenEditSessionTables(Excel.Workbook workbook)
    {
      if (WorkbookEditSessions == null || WorkbookEditSessions.Count == 0)
      {
        return;
      }

      var missingTables = new List<string>();
      _restoringExistingSessions = true;
      foreach (var session in WorkbookEditSessions)
      {
        // Browsing the session's tables and verify those still exists in the data base.
        var tableNode = DBObjectSelectionPanel3.DBObjectList.Nodes.GetNode(session.TableName);
        var dboNode = tableNode != null ? tableNode.Tag as DbObject : null;
        if (dboNode == null)
        {
          missingTables.Add(session.TableName);
          continue;
        }

        EditTableData(dboNode, true, workbook);
      }

      if (WorkbookEditSessions.Count - missingTables.Count > 0)
      {
        ActiveEditDialog.ShowDialog();
      }

      _restoringExistingSessions = false;

      // If no errors were found at the opening sessions process do not display the warning message at the end.
      if (missingTables.Count <= 0)
      {
        return;
      }

      var errorMessage = new StringBuilder();
      if (missingTables.Count > 0)
      {
        errorMessage.AppendLine(Resources.EditReopeningMissingTablesMessage);
        foreach (var table in missingTables)
        {
          errorMessage.AppendLine(table);
        }
      }

      MiscUtilities.ShowCustomizedInfoDialog(InfoDialog.InfoType.Warning, Resources.EditReopeningWarningMessage, errorMessage.ToString());
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
      bool editActive = TableHasEditOnGoing(tableName);
      DBObjectSelectionPanel3.RefreshActionLabelsEnabledStatus(tableName, editActive);
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
    }

    /// <summary>
    /// Checks if there is an Editing Operation active for a table with the given name.
    /// </summary>
    /// <param name="tableName">Name of the table.</param>
    /// <returns><c>true</c> if the table has is in editing mode, <c>false</c> otherwise.</returns>
    public bool TableHasEditOnGoing(string tableName)
    {
      if (WorkbookEditSessions == null || WorkbookEditSessions.Count == 0)
      {
        return false;
      }

      var editContainer = WorkbookEditSessions.FirstOrDefault(ac => ac.EditDialog != null && ac.TableName == tableName);
      if (editContainer == null)
      {
        return false;
      }

      // Means has an edit ongoing we need to make sure the edit has a valid sheet otherwise we need to release it
      if (Globals.ThisAddIn.Application.Worksheets.Cast<Excel.Worksheet>().Contains(editContainer.EditDialog.EditingWorksheet))
      {
        return true;
      }

      editContainer.EditDialog.Close();
      return false;
    }

    /// <summary>
    /// Checks if the selected <see cref="Excel.Range"/> contains any data in it and updates that status in the corresponidng panel.
    /// </summary>
    /// <param name="range">The <see cref="Excel.Range"/> where the selection is.</param>
    public void UpdateExcelSelectedDataStatus(Excel.Range range)
    {
      if (!Visible)
      {
        return;
      }

      DBObjectSelectionPanel3.ExcelSelectionContainsData = range.ContainsAnyData();
    }

    /// <summary>
    /// Creates the editing session or restores the saved one.
    /// </summary>
    /// <param name="tableObject">The table used on the current session.</param>
    /// <param name="importForm">The import form used on the current session.</param>
    /// <param name="currentWorksheet">The current worksheet.</param>
    /// <returns>A new or restored <see cref="EditSessionInfo"/> object.</returns>
    private EditSessionInfo GetEditSession(DbObject tableObject, ImportTableViewForm importForm, Excel.Worksheet currentWorksheet)
    {
      Excel.Range atCell = currentWorksheet.Range["A1", Type.Missing];
      Excel.Range editingRange = importForm.ImportDataTable.ImportDataAtGivenExcelCell(importForm.ImportHeaders, atCell);
      EditSessionInfo session = null;

      if (WorkbookEditSessions != null && WorkbookEditSessions.Count > 0)
      {
        session = WorkbookEditSessions.GetActiveEditSession(ActiveWorkbook, tableObject.Name);
      }

      // The Session is new and has to be created from scratch.
      if (session == null)
      {
        session = new EditSessionInfo(ActiveWorkbookId, WbConnection.Id, WbConnection.Schema, tableObject.Name);
      }

      if (session.EditDialog != null)
      {
        return session;
      }

      // The Edit session is being either restored from the settings file or created for the newborn session.
      session.EditDialog = new EditDataDialog(this, new NativeWindowWrapper(Globals.ThisAddIn.Application.Hwnd), WbConnection, editingRange, importForm.ImportDataTable, currentWorksheet);
      currentWorksheet.StoreProtectionKey(session.EditDialog.WorksheetProtectionKey);
      return session;
    }
  }
}