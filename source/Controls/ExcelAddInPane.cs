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

namespace MySQL.ForExcel
{
  using System;
  using System.Collections.Generic;
  using System.ComponentModel;
  using System.Data;
  using System.Linq;
  using System.Windows.Forms;
  using MySql.Data.MySqlClient;
  using MySQL.ForExcel.Properties;
  using MySQL.Utility;
  using MySQL.Utility.Forms;
  using Excel = Microsoft.Office.Interop.Excel;

  /// <summary>
  /// Represents a task pane that can be used in Excel to contain controls for an add-in.
  /// </summary>
  public partial class ExcelAddInPane : UserControl
  {
    /// <summary>
    /// Initializes a new instance of the <see cref="ExcelAddInPane"/> class.
    /// </summary>
    /// <param name="app">An instance of the Excel application running the add-in.</param>
    public ExcelAddInPane(Excel.Application app)
    {
      ExcelApplication = app;
      ExcelApplication.SheetChange += ExcelApplication_SheetChange;
      ExcelApplication.SheetSelectionChange += ExcelApplication_SheetSelectionChange;
      ExcelApplication.SheetActivate += ExcelApplication_SheetActivate;
      ExcelApplication.SheetDeactivate += ExcelApplication_SheetDeactivate;
      ExcelApplication.WorkbookDeactivate += ExcelApplication_WorkbookDeactivate;
      ExcelApplication.WorkbookActivate += ExcelApplication_WorkbookActivate;
      ExcelApplication.WorkbookBeforeSave += ExcelApplication_WorkbookBeforeSave;

      InitializeComponent();

      DBObjectSelectionPanel3.ExcelSelectionContainsData = false;
      ActiveEditDialog = null;
      ActiveEditDialogsList = null;
      LastDeactivatedSheetName = string.Empty;
      LastDeactivatedWorkbookName = string.Empty;
      ProtectedWorksheetPasskeys = new Dictionary<string, string>();
      WBConnection = null;
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
    /// Gets the active <see cref="Microsoft.Office.Interop.Excel.Workbook"/> in the Excel application.
    /// </summary>
    [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    public Excel.Workbook ActiveWorkbook
    {
      get
      {
        if (ExcelApplication.ActiveWorkbook != null)
        {
          return ExcelApplication.ActiveWorkbook;
        }
        else
        {
          return ExcelApplication.Workbooks.Add(1);
        }
      }
    }

    /// <summary>
    /// Gets the active <see cref="Microsoft.Office.Interop.Excel.Worksheet"/> in the Excel application.
    /// </summary>
    [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    public Excel.Worksheet ActiveWorksheet
    {
      get
      { 
        return ExcelApplication.ActiveSheet as Excel.Worksheet;
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
    /// Gets an instance of the Excel application running the add-in.
    /// </summary>
    [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    public Excel.Application ExcelApplication { get; private set; }

    /// <summary>
    /// Gets the name of the last deactivated Excel <see cref="Microsoft.Office.Interop.Excel.Worksheet"/>.
    /// </summary>
    [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    public string LastDeactivatedSheetName { get; private set; }

    /// <summary>
    /// Gets the name of the last deactivated Excel <see cref="Microsoft.Office.Interop.Excel.Workbook"/>.
    /// </summary>
    [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    public string LastDeactivatedWorkbookName { get; private set; }

    /// <summary>
    /// Gets a dictionary with the names and passkeys of Worksheets that were protected in Edit Data operations and saved to disk.
    /// </summary>
    [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    public Dictionary<string, string> ProtectedWorksheetPasskeys { get; private set; }

    /// <summary>
    /// Gets a <see cref="MySqlWorkbenchConnection"/> object representing the connection to a MySQL server instance selected by users.
    /// </summary>
    [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    public MySqlWorkbenchConnection WBConnection { get; private set; }

    #endregion Properties

    /// <summary>
    /// Exports currently selected Excel data to a new MySQL table or appends it to an existing MySQL table.
    /// </summary>
    /// <param name="toTableObject">Table to append the data to, if null exports to a new table.</param>
    /// <returns><see cref="true"/> if the export/append action was executed, <see cref="false"/> otherwise.</returns>
    public bool AppendDataToTable(DBObject toTableObject)
    {
      DialogResult dr = DialogResult.Cancel;
      Excel.Range exportRange = ExcelApplication.Selection as Excel.Range;

      if (exportRange.Areas.Count > 1)
      {
        InfoDialog.ShowWarningDialog(Resources.MultipleAreasNotSupportedWarningTitle, Resources.MultipleAreasNotSupportedWarningDetail);
        return false;
      }

      if (toTableObject != null)
      {
        this.Cursor = Cursors.WaitCursor;
        AppendDataForm appendDataForm = new AppendDataForm(WBConnection, exportRange, toTableObject, ActiveWorksheet.Name);
        this.Cursor = Cursors.Default;
        dr = appendDataForm.ShowDialog();
      }
      else
      {
        this.Cursor = Cursors.WaitCursor;
        ExportDataForm exportForm = new ExportDataForm(WBConnection, exportRange, ActiveWorksheet.Name);
        this.Cursor = Cursors.Default;
        dr = exportForm.ShowDialog();
      }

      return dr == DialogResult.OK;
    }

    /// <summary>
    /// Closes all the active <see cref="EditDialog"/> forms.
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

        if (listCount != ActiveEditDialogsList.Count)
        {
          listCount = ActiveEditDialogsList.Count;
          containerIndex--;
        }
      }

      ActiveEditDialogsList.Clear();
      ActiveEditDialogsList = null;
    }

    /// <summary>
    /// Closes the current connection, editing sessions and puts the welcome panel in focus.
    /// </summary>
    public void CloseConnection()
    {
      WBConnection = null;
      WelcomePanel1.BringToFront();

      //// Free up open Edit Dialogs
      CloseAllEditingSessions();
    }

    /// <summary>
    /// Closes the current connection, editing sessions and puts the schema panel in focus.
    /// </summary>
    public void CloseSchema()
    {
      //// If there are Active Edit sessions warn the users that by closing the schema the sessions will be terminated
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
    /// Creates a new Excel <see cref="Microsoft.Office.Interop.Excel.Worksheet"/>.
    /// </summary>
    /// <param name="proposedWorksheetName">The name of the new <see cref="Microsoft.Office.Interop.Excel.Worksheet"/>.</param>
    /// <param name="checkForDuplicates">Flag indicating if the name of the Worksheet is set to avoid a duplicate name.</param>
    /// <returns>The newly created <see cref="Microsoft.Office.Interop.Excel.Worksheet"/> object.</returns>
    public Excel.Worksheet CreateNewWorksheet(string proposedWorksheetName, bool checkForDuplicates)
    {
      Excel.Worksheet newWorksheet = null;

      try
      {
        newWorksheet = GetActiveOrCreateWorksheet(proposedWorksheetName, true, checkForDuplicates);
        Excel.Range atCell = newWorksheet.get_Range("A1", Type.Missing);
        atCell.Select();
      }
      catch (Exception ex)
      {
        MiscUtilities.ShowCustomizedErrorDialog(Resources.TaskPaneErrorCreatingWorksheetText, ex.Message, true);
        MySQLSourceTrace.WriteAppErrorToLog(ex);
      }

      return newWorksheet;
    }

    /// <summary>
    /// Opens an editing session for a MySQL table.
    /// </summary>
    /// <param name="tableObject">Table to start an editing session for.</param>
    /// <returns><see cref="true"/> if the export/append action was executed, <see cref="false"/> otherwise.</returns>
    public bool EditTableData(DBObject tableObject)
    {
      string schemaAndTableNames = WBConnection.Schema + "." + tableObject.Name;

      //// Check if the current dbobject has an edit ongoing
      if (TableHasEditOnGoing(tableObject.Name))
      {
        //// Display an error since there is an ongoing Editing operation and return
        InfoDialog.ShowErrorDialog(Resources.TaskPaneEditingNotPossibleTitleText, string.Format(Properties.Resources.TableWithOperationOngoingError, schemaAndTableNames));
        return false;
      }

      //// Check if selected Table has a Primary Key, it it does not we prompt an error and exit since Editing on such table is not permitted
      if (!WBConnection.TableHasPrimaryKey(tableObject.Name))
      {
        InfoDialog.ShowErrorDialog(Resources.EditOpenSatusError, Resources.EditOpenSummaryError, Resources.EditOpenDetailsError);
        return false;
      }

      //// Attempt to Import Data unless the user cancels the import operation
      string proposedWorksheetName = GetWorksheetNameAvoidingDuplicates(tableObject.Name);
      ImportTableViewForm importForm = new ImportTableViewForm(WBConnection, tableObject, proposedWorksheetName, ActiveWorkbook.Excel8CompatibilityMode, true);
      DialogResult dr = importForm.ShowDialog();
      if (dr == DialogResult.Cancel)
      {
        importForm.Dispose();
        return false;
      }

      if (importForm.ImportDataTable == null || importForm.ImportDataTable.Columns == null || importForm.ImportDataTable.Columns.Count == 0)
      {
        MiscUtilities.ShowCustomizedErrorDialog(string.Format(Properties.Resources.UnableToRetrieveData, tableObject.Name));
        importForm.Dispose();
        return false;
      }

      //// Before creating the new Excel Worksheet check if ActiveWorksheet is in Editing Mode and if so hide its Edit Dialog
      if (ActiveEditDialogsList != null)
      {
        ActiveEditDialogContainer activeEditContainer = ActiveEditDialogsList.Find(ac => ac.EditDialog.EditingWorksheet.Equals(ActiveWorksheet));
        if (activeEditContainer != null && activeEditContainer.EditDialog.Visible)
        {
          activeEditContainer.EditDialog.Hide();
        }
      }

      //// Create the new Excel Worksheet and import the editing data there
      Excel.Worksheet currentWorksheet = CreateNewWorksheet(proposedWorksheetName, false);
      if (currentWorksheet == null)
      {
        importForm.Dispose();
        return false;
      }

      Excel.Range atCell = currentWorksheet.Cells[1, 1];
      Excel.Range editingRange = ImportDataTableToExcelAtGivenCell(importForm.ImportDataTable, importForm.ImportHeaders, atCell);

      //// Create and show the Edit Data Dialog
      importForm.ImportDataTable.AddExtendedProperties(importForm.ImportDataTable.ExtendedProperties["QueryString"].ToString(), importForm.ImportHeaders, tableObject.Name);
      ActiveEditDialog = new EditDataDialog(this, new NativeWindowWrapper(ExcelApplication.Hwnd), WBConnection, editingRange, importForm.ImportDataTable, currentWorksheet);
      ActiveEditDialog.Show(ActiveEditDialog.ParentWindow);

      //// Maintain hashtables for open Edit Data Dialogs
      if (ActiveEditDialogsList == null)
      {
        ActiveEditDialogsList = new List<ActiveEditDialogContainer>();
      }

      ActiveEditDialogsList.Add(new ActiveEditDialogContainer(tableObject.Name, ActiveEditDialog));
      importForm.Dispose();
      return true;
    }

    /// <summary>
    /// Gets an active Excel <see cref="Microsoft.Office.Interop.Excel.Worksheet"/> or creates a new one.
    /// </summary>
    /// <param name="proposedName">The name of the new <see cref="Microsoft.Office.Interop.Excel.Worksheet"/>.</param>
    /// <param name="alwaysCreate">Flag indicating if a new <see cref="Microsoft.Office.Interop.Excel.Worksheet"/> will always be created skipping the check for an active one.</param>
    /// <param name="checkForDuplicates">Flag indicating if the name of the Worksheet is set to avoid a duplicate name.</param>
    /// <returns>The active or new <see cref="Microsoft.Office.Interop.Excel.Worksheet"/> object.</returns>
    public Excel.Worksheet GetActiveOrCreateWorksheet(string proposedName, bool alwaysCreate, bool checkForDuplicates)
    {
      Excel.Worksheet currentWorksheet = ExcelApplication.ActiveSheet as Excel.Worksheet;
      if (currentWorksheet != null && !alwaysCreate)
      {
        return currentWorksheet;
      }

      proposedName = checkForDuplicates ? GetWorksheetNameAvoidingDuplicates(proposedName) : proposedName;
      if (ExcelApplication.ActiveWorkbook != null)
      {
        Excel.Worksheet currentActiveSheet = ActiveWorksheet;
        currentWorksheet = ExcelApplication.Sheets.Add(Type.Missing, ExcelApplication.ActiveSheet, Type.Missing, Type.Missing);
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
        Excel.Workbook currentWorkbook = ExcelApplication.Workbooks.Add(Type.Missing);
        currentWorksheet = currentWorkbook.Worksheets[1] as Excel.Worksheet;
      }

      currentWorksheet.Name = proposedName;
      return currentWorksheet;
    }

    /// <summary>
    /// Gets a valid name for a new <see cref="Microsoft.Office.Interop.Excel.Worksheet"/> that avoids duplicates with existing ones in the current <see cref="Microsoft.Office.Interop.Excel.Workbook"/>.
    /// </summary>
    /// <param name="proposedName">The proposed name for a <see cref="Microsoft.Office.Interop.Excel.Worksheet"/>.</param>
    /// <param name="copyIndex">Number of the copy of a <see cref="Microsoft.Office.Interop.Excel.Worksheet"/> within its name.</param>
    /// <returns>A <see cref="Microsoft.Office.Interop.Excel.Worksheet"/> valid name.</returns>
    public string GetWorksheetNameAvoidingDuplicates(string proposedName, int copyIndex)
    {
      string retName = copyIndex > 0 ? string.Format("Copy {0} of {1}", copyIndex, proposedName) : proposedName;
      if (ExcelApplication.ActiveWorkbook == null)
      {
        return retName;
      }

      foreach (Excel.Worksheet ws in ExcelApplication.Worksheets)
      {
        if (ws.Name == retName)
        {
          return GetWorksheetNameAvoidingDuplicates(proposedName, copyIndex + 1);
        }
      }

      return retName;
    }

    /// <summary>
    /// Gets a valid name for a new <see cref="Microsoft.Office.Interop.Excel.Worksheet"/> that avoids duplicates with existing ones in the current <see cref="Microsoft.Office.Interop.Excel.Workbook"/>.
    /// </summary>
    /// <param name="proposedName">The proposed name for a <see cref="Microsoft.Office.Interop.Excel.Worksheet"/>.</param>
    /// <returns>A <see cref="Microsoft.Office.Interop.Excel.Worksheet"/> valid name.</returns>
    public string GetWorksheetNameAvoidingDuplicates(string proposedName)
    {
      return GetWorksheetNameAvoidingDuplicates(proposedName, 0);
    }

    /// <summary>
    /// Imports data contained in the given <see cref="DataTable"/> object to the active Excel <see cref="Microsoft.Office.Interop.Excel.Worksheet"/>.
    /// </summary>
    /// <param name="dt">The table containing the data to import to Excel.</param>
    /// <param name="importColumnNames">Flag indicating if column names will be imported as the first row of imported data.</param>
    /// <param name="atCell">The starting Excel (left-most and top-most) cell where the imported data is placed.</param>
    /// <returns>The Excel range containing the cells with the imported data.</returns>
    public Excel.Range ImportDataTableToExcelAtGivenCell(DataTable dt, bool importColumnNames, Excel.Range atCell)
    {
      Excel.Range fillingRange = null;
      try
      {
        if (dt != null && (dt.Rows.Count > 0 || importColumnNames))
        {
          int currentRow = atCell.Row - 1;
          int rowsCount = dt.Rows.Count;
          int colsCount = dt.Columns.Count;
          int startingRow = importColumnNames ? 1 : 0;
          int cappedNumRows = ActiveWorkbook.Excel8CompatibilityMode ? Math.Min(rowsCount + startingRow, UInt16.MaxValue - currentRow) : rowsCount + startingRow;
          bool escapeFormulaTexts = Properties.Settings.Default.ImportEscapeFormulaTextValues;

          Excel.Worksheet currentSheet = ActiveWorksheet;
          fillingRange = atCell.get_Resize(cappedNumRows, colsCount);
          object[,] fillingArray = new object[cappedNumRows, colsCount];

          if (importColumnNames)
          {
            for (int currCol = 0; currCol < colsCount; currCol++)
            {
              fillingArray[0, currCol] = dt.Columns[currCol].ColumnName;
            }
          }

          int fillingRowIdx = startingRow;
          cappedNumRows -= startingRow;
          for (int currRow = 0; currRow < cappedNumRows; currRow++)
          {
            for (int currCol = 0; currCol < colsCount; currCol++)
            {
              object importingValue = DataTypeUtilities.GetImportingValueForDateType(dt.Rows[currRow][currCol]);
              if (importingValue is string)
              {
                string importingValueText = importingValue as string;

                //// If the imported value is a text that starts with an equal sign Excel will treat it as a formula
                ////  so it needs to be escaped prepending an apostrophe to it for Excel to treat it as standard text.
                if (escapeFormulaTexts && importingValueText.StartsWith("="))
                {
                  importingValue = "'" + importingValueText;
                }
              }

              fillingArray[fillingRowIdx, currCol] = importingValue;
            }

            fillingRowIdx++;
          }

          fillingRange.ClearFormats();
          fillingRange.set_Value(Type.Missing, fillingArray);
          if (importColumnNames)
          {
            Excel.Range headerRange = fillingRange.GetColumnNamesRange();
            headerRange.SetInteriorColor(ExcelUtilities.LockedCellsOLEColor);
          }

          currentSheet.Columns.AutoFit();
          fillingRange.Rows.AutoFit();
          ExcelApplication_SheetSelectionChange(currentSheet, ExcelApplication.ActiveCell);
        }
      }
      catch (Exception ex)
      {
        MiscUtilities.ShowCustomizedErrorDialog(Resources.ImportDataErrorDetailText, ex.Message, true);
        MySQLSourceTrace.WriteAppErrorToLog(ex);
      }

      return fillingRange;
    }

    /// <summary>
    /// Imports data contained in the given <see cref="DataTable"/> object to the active Excel <see cref="Microsoft.Office.Interop.Excel.Worksheet"/>.
    /// </summary>
    /// <param name="dt">The table containing the data to import to Excel.</param>
    /// <param name="importColumnNames">Flag indicating if column names will be imported as the first row of imported data.</param>
    public void ImportDataToExcel(DataTable dt, bool importColumnNames)
    {
      ImportDataTableToExcelAtGivenCell(dt, importColumnNames, ExcelApplication.ActiveCell);
    }

    /// <summary>
    /// Imports data contained in the given <see cref="DataSet"/> object to the active Excel <see cref="Microsoft.Office.Interop.Excel.Worksheet"/>.
    /// </summary>
    /// <param name="ds">The dataset containing the data to import to Excel.</param>
    /// <param name="importColumnNames">Flag indicating if column names will be imported as the first row of imported data.</param>
    /// <param name="importType">Indicates how to arrange multiple resultsets in the active Excel <see cref="Microsoft.Office.Interop.Excel.Worksheet"/>.</param>
    /// <param name="selectedResultSet">Number of resultset to import when the <see cref="importType"/> is ImportMultipleType.SelectedResultSet.</param>
    public void ImportDataToExcel(DataSet ds, bool importColumnNames, ImportProcedureForm.ImportMultipleType importType, int selectedResultSet)
    {
      Excel.Range atCell = ExcelApplication.ActiveCell;
      Excel.Range endCell = null;
      Excel.Range fillingRange = null;

      int tableIdx = 0;
      foreach (DataTable dt in ds.Tables)
      {
        if (importType == ImportProcedureForm.ImportMultipleType.SelectedResultSet && selectedResultSet < tableIdx)
        {
          continue;
        }

        tableIdx++;
        fillingRange = ImportDataTableToExcelAtGivenCell(dt, importColumnNames, atCell);
        if (fillingRange != null)
        {
          endCell = fillingRange.Cells[fillingRange.Rows.Count, fillingRange.Columns.Count] as Excel.Range;
        }
        else
        {
          continue;
        }

        if (tableIdx < ds.Tables.Count)
        {
          switch (importType)
          {
            case ImportProcedureForm.ImportMultipleType.AllResultSetsHorizontally:
              atCell = endCell.get_Offset(atCell.Row - endCell.Row, 2);
              break;

            case ImportProcedureForm.ImportMultipleType.AllResultSetsVertically:
              if (ActiveWorkbook.Excel8CompatibilityMode && endCell.Row + 2 > UInt16.MaxValue)
              {
                return;
              }

              atCell = endCell.get_Offset(2, atCell.Column - endCell.Column);
              break;
          }
        }
      }
    }

    /// <summary>
    /// Sets and opens the current active connection used to browse schemas and DB objects.
    /// </summary>
    /// <param name="connection">A <see cref="MySqlWorkbenchConnection"/> object representing the connection to a MySQL server instance selected by users.</param>
    public void OpenConnection(MySqlWorkbenchConnection connection)
    {
      WBConnection = connection;
      RefreshWbConnectionTimeouts();
      PasswordDialogFlags passwordFlags = WBConnection.TestConnectionAndRetryOnWrongPassword();
      if (!passwordFlags.ConnectionSuccess)
      {
        return;
      }

      if (SchemaSelectionPanel2.SetConnection(WBConnection))
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
      WBConnection.Schema = schema;
      DBObjectSelectionPanel3.WBConnection = WBConnection;
      DBObjectSelectionPanel3.BringToFront();
    }

    /// <summary>
    /// Refreshes the availability of action labels linked to a table with the given name.
    /// </summary>
    /// <param name="tableName">Name of the table with status update.</param>
    /// <param name="editActive">Flag indicating if the Edit Data action is enabled for a table with the given name.</param>
    public void RefreshDBObjectPanelActionLabelsEnabledStatus(string tableName, bool editActive)
    {
      DBObjectSelectionPanel3.RefreshActionLabelsEnabledStatus(tableName, editActive);
    }

    /// <summary>
    /// Refreshes the availability of action labels linked to a table with the given name.
    /// </summary>
    /// <param name="tableName">Name of the table with status update.</param>
    public void RefreshDBObjectPanelActionLabelsEnabledStatus(string tableName)
    {
      bool editActive = TableHasEditOnGoing(tableName);
      DBObjectSelectionPanel3.RefreshActionLabelsEnabledStatus(tableName, editActive);
    }

    /// <summary>
    /// Refreshes the connection timeout values from the settings file.
    /// </summary>
    public void RefreshWbConnectionTimeouts()
    {
      if (WBConnection != null)
      {
        WBConnection.ConnectionTimeout = Settings.Default.GlobalConnectionConnectionTimeout;
        WBConnection.DefaultCommandTimeout = Settings.Default.GlobalConnectionCommandTimeout;
      }
    }

    /// <summary>
    /// Checks if there is an Editing Operation active for a table with the given name.
    /// </summary>
    /// <param name="tableName">Name of the table.</param>
    /// <returns><see cref="true"/> if the table has is in editing mode, <see cref="false"/> otherwise.</returns>
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

      //// Means has an edit ongoing we need to make sure the edit has a valid sheet otherwise we need to release it
      foreach (Excel.Worksheet workSheet in ExcelApplication.Worksheets)
      {
        if (editContainer.EditDialog.EditingWorksheet.Equals(workSheet))
        {
          return true;
        }
      }

      editContainer.EditDialog.Close();
      if (ActiveEditDialogsList.Contains(editContainer))
      {
        ActiveEditDialogsList.Remove(editContainer);
      }

      return false;
    }

    /// <summary>
    /// Checks if an Excel <see cref="Microsoft.Office.Interop.Excel.Worksheet"/> with a given name exists in a <see cref="Microsoft.Office.Interop.Excel.Workbook"/> with the given name.
    /// </summary>
    /// <param name="workBookName">Name of the <see cref="Microsoft.Office.Interop.Excel.Workbook"/>.</param>
    /// <param name="workSheetName">Name of the <see cref="Microsoft.Office.Interop.Excel.Worksheet"/>.</param>
    /// <returns><see cref="true"/> if the <see cref="Microsoft.Office.Interop.Excel.Worksheet"/> exists, <see cref="false"/> otherwise.</returns>
    public bool WorksheetExists(string workBookName, string workSheetName)
    {
      bool exists = false;

      if (workBookName.Length > 0 && workSheetName.Length > 0)
      {
        //// Maybe the last deactivated sheet has been deleted?
        try
        {
          Excel.Workbook wBook = ExcelApplication.Workbooks[workBookName] as Excel.Workbook;
          Excel.Worksheet wSheet = wBook.Worksheets[workSheetName] as Excel.Worksheet;
          exists = true;
        }
        catch
        {
          exists = false;
        }
      }

      return exists;
    }

    /// <summary>
    /// Shows or hides the <see cref="EditDataDialog"/> window associated to the given <see cref="Microsoft.Office.Interop.Excel.Worksheet"/>.
    /// </summary>
    /// <param name="workSheet">A <see cref="Microsoft.Office.Interop.Excel.Worksheet"/> object.</param>
    /// <param name="show">Flag indicating if the dialog will be shown or hidden.</param>
    private void ChangeEditDialogVisibility(Excel.Worksheet workSheet, bool show)
    {
      if (workSheet != null && ActiveEditDialogsList != null && ActiveEditDialogsList.Count > 0)
      {
        ActiveEditDialogContainer activeEditContainer = ActiveEditDialogsList.Find(ac => ac.EditDialog.EditingWorksheet == workSheet);
        if (activeEditContainer != null)
        {
          if (show)
          {
            activeEditContainer.EditDialog.ShowInactiveTopmost();
          }
          else
          {
            activeEditContainer.EditDialog.Hide();
          }
        }
      }
    }

    /// <summary>
    /// Event delegate method fired when an Excel <see cref="Microsoft.Office.Interop.Excel.Worksheet"/> is activated.
    /// </summary>
    /// <param name="workSheet">A <see cref="Microsoft.Office.Interop.Excel.Worksheet"/> object.</param>
    private void ExcelApplication_SheetActivate(object workSheet)
    {
      if (ActiveEditDialogsList == null || ActiveEditDialogsList.Count == 0)
      {
        return;
      }

      Excel.Worksheet activeSheet = workSheet as Excel.Worksheet;
      ChangeEditDialogVisibility(activeSheet, true);
      if (LastDeactivatedSheetName.Length > 0 && !WorksheetExists(ActiveWorkbook.Name, LastDeactivatedSheetName))
      {
        //// Worksheet was deleted
        ActiveEditDialogContainer activeEditContainer = ActiveEditDialogsList.Find(ac => !ac.EditDialog.EditingWorksheetExists);
        if (activeEditContainer != null)
        {
          activeEditContainer.EditDialog.Close();
          if (ActiveEditDialogsList.Contains(activeEditContainer))
          {
            ActiveEditDialogsList.Remove(activeEditContainer);
          }
        }
      }
    }

    /// <summary>
    /// Event delegate method fired when the contents of the current selection of Excel cells in a given <see cref="Microsoft.Office.Interop.Excel.Worksheet"/> change.
    /// </summary>
    /// <param name="workSheet">A <see cref="Microsoft.Office.Interop.Excel.Worksheet"/> object.</param>
    /// <param name="targetRange">A selection of Excel cells.</param>
    private void ExcelApplication_SheetChange(object workSheet, Excel.Range targetRange)
    {
      UpdateExcelSelectedDataStatus(targetRange);
    }

    /// <summary>
    /// Event delegate method fired when an Excel <see cref="Microsoft.Office.Interop.Excel.Worksheet"/> is deactivated.
    /// </summary>
    /// <param name="workSheet">A <see cref="Microsoft.Office.Interop.Excel.Worksheet"/> object.</param>
    private void ExcelApplication_SheetDeactivate(object workSheet)
    {
      if (ActiveEditDialogsList == null || ActiveEditDialogsList.Count == 0)
      {
        return;
      }

      Excel.Worksheet deactivatedSheet = workSheet as Excel.Worksheet;
      LastDeactivatedSheetName = deactivatedSheet != null ? deactivatedSheet.Name : string.Empty;
      ChangeEditDialogVisibility(deactivatedSheet, false);
    }

    /// <summary>
    /// Event delegate method fired when the selection of Excel cells in a given <see cref="Microsoft.Office.Interop.Excel.Worksheet"/> changes.
    /// </summary>
    /// <param name="workSheet">A <see cref="Microsoft.Office.Interop.Excel.Worksheet"/> object.</param>
    /// <param name="targetRange">The new selection of Excel cells.</param>
    private void ExcelApplication_SheetSelectionChange(object workSheet, Excel.Range targetRange)
    {
      UpdateExcelSelectedDataStatus(targetRange);
    }

    /// <summary>
    /// Event delegate method fired when an Excel <see cref="Microsoft.Office.Interop.Excel.Workbook"/> is activated.
    /// </summary>
    /// <param name="workBook">A <see cref="Microsoft.Office.Interop.Excel.Workbook"/> object.</param>
    private void ExcelApplication_WorkbookActivate(object workBook)
    {
      if (ActiveEditDialogsList == null || ActiveEditDialogsList.Count == 0)
      {
        return;
      }

      Excel.Workbook activeWorkbook = workBook as Excel.Workbook;
      ChangeEditDialogVisibility(activeWorkbook.ActiveSheet as Excel.Worksheet, true);

      //// Check if last active was closed or unactivated
      if (string.IsNullOrEmpty(LastDeactivatedWorkbookName))
      {
        return;
      }

      //// Search in the collection of Workbooks
      var workbooks = Globals.ThisAddIn.Application.Workbooks;
      foreach (Excel.Workbook workbook in workbooks)
      {
        if (workbook.Name == LastDeactivatedWorkbookName)
        {
          return;
        }
      }

      //// Free resorces from the missing workbook
      int listCount = ActiveEditDialogsList.Count;
      for (int containerIndex = 0; containerIndex < listCount; containerIndex++)
      {
        ActiveEditDialogContainer activeEditContainer = ActiveEditDialogsList[containerIndex];
        if (!string.IsNullOrEmpty(activeEditContainer.EditDialog.WorkbookName) && activeEditContainer.EditDialog.WorkbookName != LastDeactivatedWorkbookName)
        {
          continue;
        }

        activeEditContainer.EditDialog.Close();
        if (ActiveEditDialogsList.Contains(activeEditContainer))
        {
          ActiveEditDialogsList.Remove(activeEditContainer);
        }

        if (listCount != ActiveEditDialogsList.Count)
        {
          listCount = ActiveEditDialogsList.Count;
          containerIndex--;
        }
      }

      if (ActiveEditDialogsList.Count == 0)
      {
        ActiveEditDialogsList = null;
      }
    }

    /// <summary>
    /// Event delegate method fired before an Excel <see cref="Microsoft.Office.Interop.Excel.Workbook"/> is saved to disk.
    /// </summary>
    /// <param name="Wb">A <see cref="Microsoft.Office.Interop.Excel.Workbook"/> object.</param>
    /// <param name="SaveAsUI">Flag indicating whether the Save As dialog was displayed.</param>
    /// <param name="Cancel">Flag indicating whether the event is cancelled.</param>
    private void ExcelApplication_WorkbookBeforeSave(Excel.Workbook Wb, bool SaveAsUI, ref bool Cancel)
    {
      if (ActiveEditDialogsList.Exists(editContainer => editContainer.EditDialog.WorkbookName == Wb.Name))
      {
        bool closeEditingWorksheets = InfoDialog.ShowYesNoDialog(InfoDialog.InfoType.Warning, Resources.WorkSheetInEditModeSavingWarningTitle, Resources.WorkSheetInEditModeSavingWarningDetail, null, Resources.WorkSheetInEditModeSavingWarningMoreInfo) == DialogResult.Yes;
        foreach (Excel.Worksheet worksheet in Wb.Worksheets)
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
          else
          {
            ProtectedWorksheetPasskeys.Add(worksheet.Name, editContainer.EditDialog.WorksheetProtectionKey);
          }
        }
      }
    }

    /// <summary>
    /// Event delegate method fired when an Excel <see cref="Microsoft.Office.Interop.Excel.Workbook"/> is deactivated.
    /// </summary>
    /// <param name="workBook">A <see cref="Microsoft.Office.Interop.Excel.Workbook"/> object.</param>
    private void ExcelApplication_WorkbookDeactivate(object workBook)
    {
      if (ActiveEditDialogsList == null || ActiveEditDialogsList.Count == 0)
      {
        return;
      }

      Excel.Workbook deactivatedWorkbook = workBook as Excel.Workbook;
      LastDeactivatedWorkbookName = deactivatedWorkbook.Name;

      //// Hide editDialogs from deactivated Workbook
      foreach (Excel.Worksheet wSheet in deactivatedWorkbook.Worksheets)
      {
        ChangeEditDialogVisibility(wSheet, false);
      }
    }

    /// <summary>
    /// Checks if the given Excel range contains data in any of its cells.
    /// </summary>
    /// <param name="range">An excel range.</param>
    /// <returns><see cref="true"/> if the given range is not empty, <see cref="false"/> otherwise.</returns>
    private bool ExcelRangeContainsAnyData(Excel.Range range)
    {
      bool hasData = false;
      int selectedCellsCount = range.Count;
      if (range.Count == 1)
      {
        hasData = range.Value2 != null;
      }
      else if (range.Count > 1)
      {
        object[,] values = range.Value2;
        if (values != null)
        {
          foreach (object o in values)
          {
            if (o == null)
            {
              continue;
            }

            hasData = true;
            break;
          }
        }
      }

      return hasData;
    }

    private void UpdateExcelSelectedDataStatus(Excel.Range range)
    {
      if (!this.Visible)
      {
        return;
      }

      bool hasData = ExcelRangeContainsAnyData(range);
      DBObjectSelectionPanel3.ExcelSelectionContainsData = hasData;
    }
  }
}