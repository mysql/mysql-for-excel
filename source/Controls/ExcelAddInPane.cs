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
using MySQL.ForExcel.Forms;
using MySQL.ForExcel.Panels;
using MySQL.ForExcel.Properties;
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
    /// Initializes a new instance of the <see cref="ExcelAddInPane"/> class.
    /// </summary>
    public ExcelAddInPane()
    {
      InitializeComponent();

      DBObjectSelectionPanel3.ExcelSelectionContainsData = false;
      ActiveEditDialog = null;
      FirstSession = null;
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
                && Globals.ThisAddIn.ActiveWorkbookSessions.Exists(session => session.EditDialog != null && session.EditDialog.EditingWorksheet == ActiveWorksheet);
      }
    }

    /// <summary>
    /// Gets or sets the first session of Edit sessions.
    /// </summary>
    [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    public EditSessionInfo FirstSession { get; private set; }

    /// <summary>
    /// Gets a list of stored procedures loaded in this pane.
    /// </summary>
    [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    public List<DbObject> LoadedProcedures
    {
      get
      {
        return DBObjectSelectionPanel3.LoadedProcedures;
      }
    }

    /// <summary>
    /// Gets a list of schemas loaded in this pane.
    /// </summary>
    [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    public List<string> LoadedSchemas
    {
      get
      {
        return SchemaSelectionPanel2.LoadedSchemas;
      }
    }

    /// <summary>
    /// Gets a list of tables loaded in this pane.
    /// </summary>
    [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    public List<DbObject> LoadedTables
    {
      get
      {
        return DBObjectSelectionPanel3.LoadedTables;
      }
    }

    /// <summary>
    /// Gets a list of views loaded in this pane.
    /// </summary>
    [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    public List<DbObject> LoadedViews
    {
      get
      {
        return DBObjectSelectionPanel3.LoadedViews;
      }
    }

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
    /// Closes the current connection, editing sessions and puts the welcome panel in focus.
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
      Globals.ThisAddIn.CloseWorkbookEditSessions(ActiveWorkbook);
    }

    /// <summary>
    /// Closes the current connection, editing sessions and puts the schema panel in focus.
    /// </summary>
    /// <param name="askToCloseConnections">Flag indicating whether users are asked for confirmation before closing active Edit sessions.</param>
    /// <param name="givePanelFocus">Flag indicating whether the <see cref="SchemaSelectionPanel"/> is given focus.</param>
    /// <returns><c>true</c> if the schema and its open Edit sessions are closed, <c>false</c> otherwise.</returns>
    public bool CloseSchema(bool askToCloseConnections, bool givePanelFocus)
    {
      if (askToCloseConnections && Globals.ThisAddIn.ActiveWorkbookSessions.Count > 0)
      {
        // If there are Active Edit sessions warn the users that by closing the schema the sessions will be terminated
        DialogResult dr = MiscUtilities.ShowCustomizedWarningDialog(Resources.ActiveEditingSessionsCloseWarningTitle, Resources.ActiveEditingSessionsCloseWarningDetail);
        if (dr == DialogResult.No)
        {
          return false;
        }
      }

      Globals.ThisAddIn.CloseWorkbookEditSessions(ActiveWorkbook);
      if (givePanelFocus)
      {
        SchemaSelectionPanel2.BringToFront();
      }

      return true;
    }

    /// <summary>
    /// Opens an editing session for a MySQL table.
    /// </summary>
    /// <param name="tableObject">Table to start an editing session for.</param>
    /// <param name="fromSavedSession">Flag indicating whether the Edit session to be opened corresponds.</param>
    /// <param name="workbook">The workbook.</param>
    /// <returns>
    ///   <c>true</c> If the export/append action was executed, <c>false</c> otherwise.
    /// </returns>
    public bool EditTableData(DbObject tableObject, bool fromSavedSession, Excel.Workbook workbook)
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
      string proposedWorksheetName = fromSavedSession ? tableObject.Name : ActiveWorkbook.GetWorksheetNameAvoidingDuplicates(tableObject.Name);
      ImportTableViewForm importForm = new ImportTableViewForm(WbConnection, tableObject, proposedWorksheetName, ActiveWorkbook.Excel8CompatibilityMode, true);
      DialogResult dr = fromSavedSession ? importForm.ImportHidingDialog() : importForm.ShowDialog();
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
      if (!fromSavedSession)
      {
        foreach (var session in Globals.ThisAddIn.ActiveWorkbookSessions.Where(session => session.EditDialog != null && session.EditDialog.Visible))
        {
          session.EditDialog.Hide();
        }
      }

      // Create the new Excel Worksheet and import the editing data there
      Excel.Workbook editWorkbook = fromSavedSession && workbook != null ? workbook : ActiveWorkbook;
      var currentWorksheet = fromSavedSession && Settings.Default.EditSessionsReuseWorksheets
        ? editWorkbook.GetOrCreateWorksheet(proposedWorksheetName, true)
        : editWorkbook.CreateWorksheet(proposedWorksheetName, true);
      if (currentWorksheet == null)
      {
        importForm.Dispose();
        return false;
      }

      // Clear the contents of the worksheet if we are restoring a saved Edit session since the user may have input data into it.
      if (fromSavedSession)
      {
        currentWorksheet.UsedRange.Clear();
      }

      // Create and show the Edit Data Dialog
      var editSession = GetEditSession(tableObject, importForm, currentWorksheet);
      ActiveEditDialog = editSession.EditDialog;
      if (fromSavedSession)
      {
        // If restoring sessions we need to set the EditDialog to its session.
        var editSessionBeingRestored = Globals.ThisAddIn.ActiveWorkbookSessions.FirstOrDefault(session => session.TableName.Equals(editSession.TableName));
        if (editSessionBeingRestored != null)
        {
          editSessionBeingRestored.EditDialog = editSession.EditDialog;
        }
      }
      else
      {
        ActiveEditDialog.ShowDialog();

        // If not restoring sessions we need to add the manually triggered Edit session to the list of Edit sessions of the active workbook.
        Globals.ThisAddIn.ActiveWorkbookSessions.Add(editSession);
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
      WbConnection.Schema = schema;
      DBObjectSelectionPanel3.WbConnection = WbConnection;
      if (givePanelFocus)
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
      bool editActive = TableHasEditOnGoing(tableName);
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
      WbConnection.SetSessionReadWriteTimeouts();
    }

    /// <summary>
    /// Checks if there is an Editing Operation active for a table with the given name.
    /// </summary>
    /// <param name="tableName">Name of the table.</param>
    /// <returns><c>true</c> if the table has is in editing mode, <c>false</c> otherwise.</returns>
    public bool TableHasEditOnGoing(string tableName)
    {
      if (Globals.ThisAddIn.ActiveWorkbookSessions.Count == 0)
      {
        return false;
      }

      var editContainer = Globals.ThisAddIn.ActiveWorkbookSessions.FirstOrDefault(ac => ac.EditDialog != null && ac.TableName == tableName);
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

      if (Globals.ThisAddIn.ActiveWorkbookSessions.Count > 0)
      {
        session = Globals.ThisAddIn.ActiveWorkbookSessions.GetActiveEditSession(ActiveWorkbook, tableObject.Name);
      }

      // The Session is new and has to be created from scratch.
      if (session == null)
      {
        session = new EditSessionInfo(ActiveWorkbookId, WbConnection.Id, WbConnection.Schema, tableObject.Name, ActiveWorkbook.FullName);
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