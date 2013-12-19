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
    /// Initializes a new instance of the <see cref="ExcelAddInPane"/> class.
    /// </summary>
    public ExcelAddInPane()
    {
      InitializeComponent();

      DBObjectSelectionPanel3.ExcelSelectionContainsData = false;
      ActiveEditDialog = null;
      ActiveEditDialogsList = null;
      ProtectedWorksheetPasskeys = new Dictionary<string, string>();
      WbConnection = null;
    }

    #region Properties

    /// <summary>
    /// Gets the active <see cref="EditDataDialog"/> used when clicking the Edit Data action label.
    /// </summary>
    [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    public EditDataDialog ActiveEditDialog { get; private set; }

    /// <summary>
    /// Gets a list of <see cref="ActiveEditDialogContainer"/> objects.
    /// </summary>
    [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    public List<ActiveEditDialogContainer> ActiveEditDialogsList { get; private set; }

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
                && ActiveEditDialogsList != null
                && ActiveEditDialogsList.Count > 0
                && ActiveEditDialogsList.Exists(ac => ac.EditDialog.EditingWorksheet == ActiveWorksheet);
      }
    }

    /// <summary>
    /// Gets a dictionary with the names and passkeys of Worksheets that were protected in Edit Data operations and saved to disk.
    /// </summary>
    [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    public Dictionary<string, string> ProtectedWorksheetPasskeys { get; private set; }

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
    /// Shows or hides the <see cref="EditDataDialog"/> window associated to the given <see cref="Excel.Worksheet"/>.
    /// </summary>
    /// <param name="workSheet">A <see cref="Excel.Worksheet"/> object.</param>
    /// <param name="show">Flag indicating if the dialog will be shown or hidden.</param>
    public void ChangeEditDialogVisibility(Excel.Worksheet workSheet, bool show)
    {
      if (workSheet == null || ActiveEditDialogsList == null || ActiveEditDialogsList.Count <= 0)
      {
        return;
      }

      ActiveEditDialogContainer activeEditContainer = ActiveEditDialogsList.Find(ac => ac.EditDialog.EditingWorksheet == workSheet);
      if (activeEditContainer == null)
      {
        return;
      }

      if (show)
      {
        activeEditContainer.EditDialog.ShowInactiveTopmost();
      }
      else
      {
        activeEditContainer.EditDialog.Hide();
      }
    }

    /// <summary>
    /// Closes and removes all Edit sessions associated to the given <see cref="Excel.Workbook"/>.
    /// </summary>
    /// <param name="workbook">The <see cref="Excel.Workbook"/> associated to the Edit sessions to close.</param>
    public void CloseWorkbookEditSessions(Excel.Workbook workbook)
    {
      if (workbook == null || ActiveEditDialogsList == null)
      {
        return;
      }

      foreach (var workbookEditContainer in ActiveEditDialogsList.Where(ac => string.Equals(ac.EditDialog.WorkbookName, workbook.Name, StringComparison.InvariantCulture)).ToList())
      {
        // The Close method is both closing the dialog and removing itself from the collection of ActiveEditDialogContainer objects.
        workbookEditContainer.EditDialog.Close();
      }

      if (ActiveEditDialogsList.Count == 0)
      {
        ActiveEditDialogsList = null;
      }
    }

    /// <summary>
    /// Signals that an Excel <see cref="Excel.Workbook"/> is about to be saved to disk.
    /// </summary>
    /// <param name="workBook">A <see cref="Excel.Workbook"/> object.</param>
    public void CloseWorkbookEditSessionsOnSave(Excel.Workbook workBook)
    {
      if (workBook == null || ActiveEditDialogsList == null || !ActiveEditDialogsList.Exists(editContainer => editContainer.EditDialog.WorkbookName == workBook.Name))
      {
        return;
      }

      bool closeEditingWorksheets = InfoDialog.ShowYesNoDialog(InfoDialog.InfoType.Warning, Resources.WorksheetInEditModeSavingWarningTitle, Resources.WorksheetInEditModeSavingWarningDetail, null, Resources.WorksheetInEditModeSavingWarningMoreInfo) == DialogResult.Yes;
      foreach (Excel.Worksheet worksheet in workBook.Worksheets)
      {
        ActiveEditDialogContainer editContainer = ActiveEditDialogsList.FirstOrDefault(editDialogContainer => editDialogContainer.EditDialog.EditingWorksheet == worksheet);
        if (editContainer == null)
        {
          continue;
        }

        if (closeEditingWorksheets)
        {
          if (editContainer.EditDialog != null)
          {
            editContainer.EditDialog.Close();
          }

          if (ActiveEditDialogsList.Contains(editContainer))
          {
            ActiveEditDialogsList.Remove(editContainer);
          }

          ProtectedWorksheetPasskeys.Remove(worksheet.Name);
        }
        else if (!ProtectedWorksheetPasskeys.ContainsKey(worksheet.Name))
        {
          ProtectedWorksheetPasskeys.Add(worksheet.Name, editContainer.EditDialog.WorksheetProtectionKey);
        }
      }
    }

    /// <summary>
    /// Closes and removes the Edit session associated to the given <see cref="Excel.Worksheet"/>.
    /// </summary>
    /// <param name="worksheet">The <see cref="Excel.Worksheet"/> associated to the Edit session to close.</param>
    public void CloseWorksheetEditSession(Excel.Worksheet worksheet)
    {
      ActiveEditDialogContainer activeEditContainer = worksheet == null || ActiveEditDialogsList == null
        ? null
        : ActiveEditDialogsList.FirstOrDefault(ac => ac.EditDialog.WorkbookName == worksheet.GetParentWorkbookName() && ac.EditDialog.WorksheetName == worksheet.Name);
      if (activeEditContainer == null)
      {
        return;
      }

      activeEditContainer.EditDialog.Close();
      if (ActiveEditDialogsList.Contains(activeEditContainer))
      {
        ActiveEditDialogsList.Remove(activeEditContainer);
      }
    }

    /// <summary>
    /// Closes all the active <see cref="EditDataDialog"/> forms.
    /// </summary>
    public void CloseAllEditingSessions()
    {
      if (ActiveEditDialogsList == null)
      {
        return;
      }

      int listCount = ActiveEditDialogsList.Count;
      for (int containerIndex = 0; containerIndex < listCount; containerIndex++)
      {
        ActiveEditDialogContainer activeEditContainer = ActiveEditDialogsList[containerIndex];
        if (activeEditContainer.EditDialog != null)
        {
          activeEditContainer.EditDialog.Close();
        }

        if (ActiveEditDialogsList.Contains(activeEditContainer))
        {
          ActiveEditDialogsList.Remove(activeEditContainer);
        }

        if (listCount == ActiveEditDialogsList.Count)
        {
          continue;
        }

        listCount = ActiveEditDialogsList.Count;
        containerIndex--;
      }

      ActiveEditDialogsList.Clear();
      ActiveEditDialogsList = null;
    }

    /// <summary>
    /// Closes the current connection, editing sessions and puts the welcome panel in focus.
    /// </summary>
    public void CloseConnection()
    {
      WbConnection = null;
      WelcomePanel1.BringToFront();

      // Free up open Edit Dialogs
      CloseAllEditingSessions();
    }

    /// <summary>
    /// Closes the current connection, editing sessions and puts the schema panel in focus.
    /// </summary>
    public void CloseSchema()
    {
      // If there are Active Edit sessions warn the users that by closing the schema the sessions will be terminated
      if (ActiveEditDialogsList != null && ActiveEditDialogsList.Count > 0)
      {
        DialogResult dr = MiscUtilities.ShowCustomizedWarningDialog(Resources.ActiveEditingSessionsCloseWarningTitle, Resources.ActiveEditingSessionsCloseWarningDetail);
        if (dr == DialogResult.No)
        {
          return;
        }

        CloseAllEditingSessions();
      }

      SchemaSelectionPanel2.BringToFront();
    }

    /// <summary>
    /// Creates a new Excel <see cref="Excel.Worksheet"/>.
    /// </summary>
    /// <param name="proposedWorksheetName">The name of the new <see cref="Excel.Worksheet"/>.</param>
    /// <param name="checkForDuplicates">Flag indicating if the name of the Worksheet is set to avoid a duplicate name.</param>
    /// <returns>The newly created <see cref="Excel.Worksheet"/> object.</returns>
    public Excel.Worksheet CreateNewWorksheet(string proposedWorksheetName, bool checkForDuplicates)
    {
      Excel.Worksheet newWorksheet = null;

      try
      {
        newWorksheet = GetActiveOrCreateWorksheet(proposedWorksheetName, true, checkForDuplicates);
        Excel.Range atCell = newWorksheet.Range["A1", Type.Missing];
        atCell.Select();
      }
      catch (Exception ex)
      {
        MiscUtilities.ShowCustomizedErrorDialog(Resources.TaskPaneErrorCreatingWorksheetText, ex.Message, true);
        MySqlSourceTrace.WriteAppErrorToLog(ex);
      }

      return newWorksheet;
    }

    /// <summary>
    /// Opens an editing session for a MySQL table.
    /// </summary>
    /// <param name="tableObject">Table to start an editing session for.</param>
    /// <returns><c>true</c> if the export/append action was executed, <c>false</c> otherwise.</returns>
    public bool EditTableData(DbObject tableObject)
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
      string proposedWorksheetName = tableObject.Name.GetWorksheetNameAvoidingDuplicates();
      ImportTableViewForm importForm = new ImportTableViewForm(WbConnection, tableObject, proposedWorksheetName, ActiveWorkbook.Excel8CompatibilityMode, true);
      DialogResult dr = importForm.ShowDialog();
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

      // Before creating the new Excel Worksheet check if ActiveWorksheet is in Editing Mode and if so hide its Edit Dialog
      if (ActiveEditDialogsList != null)
      {
        ActiveEditDialogContainer activeEditContainer = ActiveEditDialogsList.Find(ac => ac.EditDialog.EditingWorksheet.Equals(ActiveWorksheet));
        if (activeEditContainer != null && activeEditContainer.EditDialog.Visible)
        {
          activeEditContainer.EditDialog.Hide();
        }
      }

      // Create the new Excel Worksheet and import the editing data there
      Excel.Worksheet currentWorksheet = CreateNewWorksheet(proposedWorksheetName, false);
      if (currentWorksheet == null)
      {
        importForm.Dispose();
        return false;
      }

      Excel.Range atCell = currentWorksheet.Cells[1, 1];
      Excel.Range editingRange = importForm.ImportDataTable.ImportDataAtGivenExcelCell(importForm.ImportHeaders, atCell);

      // Create and show the Edit Data Dialog
      ActiveEditDialog = new EditDataDialog(this, new NativeWindowWrapper(Globals.ThisAddIn.Application.Hwnd), WbConnection, editingRange, importForm.ImportDataTable, currentWorksheet);
      ActiveEditDialog.Show(ActiveEditDialog.ParentWindow);

      // Maintain hashtables for open Edit Data Dialogs
      if (ActiveEditDialogsList == null)
      {
        ActiveEditDialogsList = new List<ActiveEditDialogContainer>();
      }

      ActiveEditDialogsList.Add(new ActiveEditDialogContainer(tableObject.Name, ActiveEditDialog));
      importForm.Dispose();
      return true;
    }

    /// <summary>
    /// Gets an active Excel <see cref="Excel.Worksheet"/> or creates a new one.
    /// </summary>
    /// <param name="proposedName">The name of the new <see cref="Excel.Worksheet"/>.</param>
    /// <param name="alwaysCreate">Flag indicating if a new <see cref="Excel.Worksheet"/> will always be created skipping the check for an active one.</param>
    /// <param name="checkForDuplicates">Flag indicating if the name of the Worksheet is set to avoid a duplicate name.</param>
    /// <returns>The active or new <see cref="Excel.Worksheet"/> object.</returns>
    public Excel.Worksheet GetActiveOrCreateWorksheet(string proposedName, bool alwaysCreate, bool checkForDuplicates)
    {
      Excel.Worksheet currentWorksheet = Globals.ThisAddIn.Application.ActiveSheet as Excel.Worksheet;
      if (currentWorksheet != null && !alwaysCreate)
      {
        return currentWorksheet;
      }

      proposedName = checkForDuplicates ? proposedName.GetWorksheetNameAvoidingDuplicates() : proposedName;
      if (Globals.ThisAddIn.Application.ActiveWorkbook != null)
      {
        Excel.Worksheet currentActiveSheet = ActiveWorksheet;
        currentWorksheet = Globals.ThisAddIn.Application.Sheets.Add(Type.Missing, Globals.ThisAddIn.Application.ActiveSheet, Type.Missing, Type.Missing);
        if (ActiveEditDialogsList != null)
        {
          ActiveEditDialogContainer activeEditContainer = ActiveEditDialogsList.Find(ac => ac.EditDialog.EditingWorksheet.Equals(currentActiveSheet));
          if (activeEditContainer != null && activeEditContainer.EditDialog.Visible)
          {
            activeEditContainer.EditDialog.Hide();
          }
        }
      }
      else
      {
        Excel.Workbook currentWorkbook = Globals.ThisAddIn.Application.Workbooks.Add(Type.Missing);
        currentWorksheet = currentWorkbook.Worksheets[1] as Excel.Worksheet;
      }

      if (currentWorksheet != null)
      {
        currentWorksheet.Name = proposedName;
      }

      return currentWorksheet;
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
    public void OpenConnection(MySqlWorkbenchConnection connection)
    {
      WbConnection = connection;
      RefreshWbConnectionTimeouts();
      PasswordDialogFlags passwordFlags = WbConnection.TestConnectionAndRetryOnWrongPassword();
      if (!passwordFlags.ConnectionSuccess)
      {
        return;
      }

      if (SchemaSelectionPanel2.SetConnection(WbConnection))
      {
        SchemaSelectionPanel2.BringToFront();
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
      if (ActiveEditDialogsList == null || ActiveEditDialogsList.Count == 0)
      {
        return false;
      }

      ActiveEditDialogContainer editContainer = ActiveEditDialogsList.Find(ac => ac.TableName == tableName);
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
      if (ActiveEditDialogsList.Contains(editContainer))
      {
        ActiveEditDialogsList.Remove(editContainer);
      }

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
  }
}