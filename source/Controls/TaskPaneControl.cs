// 
// Copyright (c) 2012, Oracle and/or its affiliates. All rights reserved.
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
//

using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using MySql.Data.MySqlClient;
using Excel = Microsoft.Office.Interop.Excel;
using ExcelTools = Microsoft.Office.Tools.Excel;
using MySQL.Utility;
using MySQL.ForExcel.Properties;

namespace MySQL.ForExcel
{
  public partial class TaskPaneControl : UserControl
  {
    private Excel.Application excelApplication;
    private MySqlWorkbenchConnection connection;
    private EditDataDialog editDialog = null;
    private string lastDeactivatedSheetName = String.Empty;
    private string lastDeactivatedWorkbookName { get; set; }

    public List<ActiveEditDialogContainer> ActiveEditDialogsList;
    public Excel.Worksheet ActiveWorksheet
    {
      get { return excelApplication.ActiveSheet as Excel.Worksheet; }
    }
    public bool ActiveWorksheetInEditMode
    {
      get { return (ActiveWorksheet != null 
                    && ActiveEditDialogsList != null
                    && ActiveEditDialogsList.Count > 0
                    && ActiveEditDialogsList.Exists(ac => ac.EditDialog.EditingWorksheet == ActiveWorksheet)); }
    }

    public Excel.Workbook ActiveWorkbook
    {
      get
      {
        if (excelApplication.ActiveWorkbook != null)
          return excelApplication.ActiveWorkbook;
        else
          return excelApplication.Workbooks.Add(1);
      }
    }

    public TaskPaneControl(Excel.Application app)
    {
      excelApplication = app;
      excelApplication.SheetSelectionChange += new Excel.AppEvents_SheetSelectionChangeEventHandler(excelApplication_SheetSelectionChange);
      excelApplication.SheetActivate += new Excel.AppEvents_SheetActivateEventHandler(excelApplication_SheetActivate);
      excelApplication.SheetDeactivate += new Excel.AppEvents_SheetDeactivateEventHandler(excelApplication_SheetDeactivate);
      excelApplication.WorkbookDeactivate += new Excel.AppEvents_WorkbookDeactivateEventHandler(excelApplication_WorkbookDeactivate);
      excelApplication.WorkbookActivate +=new Excel.AppEvents_WorkbookActivateEventHandler(excelApplication_WorkbookActivate);
    
      InitializeComponent();

      dbObjectSelectionPanel1.ExcelSelectionContainsData = false;
    }

    private void ChangeEditDialogVisibility(Excel.Worksheet workSheet, bool show)
    {
      if (workSheet != null && ActiveEditDialogsList != null && ActiveEditDialogsList.Count > 0)
      {
        ActiveEditDialogContainer activeEditContainer = ActiveEditDialogsList.Find(ac => ac.EditDialog.EditingWorksheet == workSheet);
        if (activeEditContainer != null)
        {
          if (show)
            activeEditContainer.EditDialog.ShowInactiveTopmost();
          else
            activeEditContainer.EditDialog.Hide();
        }
      }
    }

    void excelApplication_WorkbookActivate(object sh)
    {
      if (ActiveEditDialogsList == null || ActiveEditDialogsList.Count == 0)
        return;

      Excel.Workbook activeWorkbook = sh as Excel.Workbook;
      ChangeEditDialogVisibility(activeWorkbook.ActiveSheet as Excel.Worksheet, true);

      // check if last active was closed or unactivated
      if (String.IsNullOrEmpty(lastDeactivatedWorkbookName))
        return;

      //search in the collection of Workbooks
      var workbooks = Globals.ThisAddIn.Application.Workbooks;     
      foreach (Excel.Workbook workbook in workbooks)
      {
        if (workbook.Name == lastDeactivatedWorkbookName)
          return;
      }

      //Free resorces from the missing workbook
      int listCount = ActiveEditDialogsList.Count;
      for (int containerIndex = 0; containerIndex < listCount; containerIndex++)
      {
        ActiveEditDialogContainer activeEditContainer = ActiveEditDialogsList[containerIndex];
        if (!String.IsNullOrEmpty(activeEditContainer.EditDialog.WorkbookName) && activeEditContainer.EditDialog.WorkbookName != lastDeactivatedWorkbookName)
          continue;
        activeEditContainer.EditDialog.Close();
        if (ActiveEditDialogsList.Contains(activeEditContainer))
          ActiveEditDialogsList.Remove(activeEditContainer);
        if (listCount != ActiveEditDialogsList.Count)
        {
          listCount = ActiveEditDialogsList.Count;
          containerIndex--;
        }
      }
      if (ActiveEditDialogsList.Count == 0)
        ActiveEditDialogsList = null;
    }

    void excelApplication_WorkbookDeactivate(object sh)
    {
      if (ActiveEditDialogsList == null || ActiveEditDialogsList.Count == 0)
        return;

      Excel.Workbook deactivatedWorkbook = sh as Excel.Workbook;
      lastDeactivatedWorkbookName = deactivatedWorkbook.Name;
      
      // Hide editDialogs from deactivated Workbook
      foreach (Excel.Worksheet wSheet in deactivatedWorkbook.Worksheets)
      {
        ChangeEditDialogVisibility(wSheet, false);
      }
    }

    void excelApplication_SheetDeactivate(object Sh)
    {
      if (ActiveEditDialogsList == null || ActiveEditDialogsList.Count == 0)
        return;
      Excel.Worksheet deactivatedSheet = Sh as Excel.Worksheet;
      lastDeactivatedSheetName = (deactivatedSheet != null ? deactivatedSheet.Name : String.Empty);
      ChangeEditDialogVisibility(deactivatedSheet, false);
    }

    void excelApplication_SheetActivate(object Sh)
    {
      if (ActiveEditDialogsList == null || ActiveEditDialogsList.Count == 0)
        return;
      Excel.Worksheet activeSheet = Sh as Excel.Worksheet;
      ChangeEditDialogVisibility(activeSheet, true);

      if (lastDeactivatedSheetName.Length > 0 && !WorksheetExists(ActiveWorkbook.Name, lastDeactivatedSheetName))
      {
        // Worksheet was deleted
        ActiveEditDialogContainer activeEditContainer = ActiveEditDialogsList.Find(ac => !ac.EditDialog.EditingWorksheetExists);
        if (activeEditContainer != null)
        {
          activeEditContainer.EditDialog.Close();
          if (ActiveEditDialogsList.Contains(activeEditContainer))
            ActiveEditDialogsList.Remove(activeEditContainer);
        }
      }
    }

    void excelApplication_SheetSelectionChange(object Sh, Excel.Range Target)
    {
      if (!this.Visible)  return;

      int selectedCellsCount = Target.Count;

      bool hasData = false;

      if (Target.Count == 1)
        hasData = Target.Value2 != null;
      else if (Target.Count > 1)
      {
        object[,] values = Target.Value2;

        if (values != null)
        {
          foreach (object o in values)
          {
            if (o == null) continue;
            hasData = true;
            break;
          }
        }
      }
      dbObjectSelectionPanel1.ExcelSelectionContainsData = hasData;
    }

    public bool WorksheetExists(string workBookName, string workSheetName)
    {
      bool exists = false;

      if (workBookName.Length > 0 && workSheetName.Length > 0)
      {
        // Maybe the last deactivated sheet has been deleted?
        try
        {
          Excel.Workbook wBook = excelApplication.Workbooks[workBookName] as Excel.Workbook;
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

    public void RefreshDBObjectPanelActionLabelsEnabledStatus(string tableName, bool editActive)
    {
      dbObjectSelectionPanel1.RefreshActionLabelsEnabledStatus(tableName, editActive);
    }

    public void RefreshDBObjectPanelActionLabelsEnabledStatus(string tableName)
    {
      bool editActive = TableHasEditOnGoing(tableName);
      dbObjectSelectionPanel1.RefreshActionLabelsEnabledStatus(tableName, editActive);
    }

    public void OpenConnection(MySqlWorkbenchConnection connection)
    {
      this.connection = connection;
      bool failed = false;
      while (true)
      {
        if (connection.Password == null || failed)
        {
          PasswordDialog dlg = new PasswordDialog();
          dlg.HostIdentifier = connection.Name + " - " + connection.HostIdentifier;
          dlg.UserName = connection.UserName;
          dlg.PasswordText = String.Empty;
          if (dlg.ShowDialog() == DialogResult.Cancel)
          {
            connection.Password = null;
            return;
          }
          connection.Password = dlg.PasswordText;
        }
        if (connection.TestConnection())
          break;

        bool isSSL = false;
        if (connection.UseSSL == 1 ||
            !(string.IsNullOrWhiteSpace(connection.SSLCA) &&
              string.IsNullOrWhiteSpace(connection.SSLCert) &&
              string.IsNullOrWhiteSpace(connection.SSLCipher) &&
              string.IsNullOrWhiteSpace(connection.SSLKey))
        )
          isSSL = true;

        InfoDialog infoDialog = new InfoDialog(InfoDialog.InfoType.Warning, Resources.ConnectFailedDetailWarning, null);
        infoDialog.OperationStatusText = Resources.ConnectFailedTitleWarning;
        infoDialog.OperationSummarySubText = string.Empty;
        if (isSSL)
        {
          infoDialog.OperationDetailsText = Resources.ConnectSSLFailedDetailWarning;
        }
        infoDialog.ShowDialog();
        failed = true;
      }
      bool schemasLoaded = schemaSelectionPanel1.SetConnection(connection);
      if (schemasLoaded)
        schemaSelectionPanel1.BringToFront();
    }

    public void CloseAllEditingSessions()
    {
      if (ActiveEditDialogsList == null)
        return;
      int listCount = ActiveEditDialogsList.Count;
      for (int containerIndex = 0; containerIndex < listCount; containerIndex++)
      {
        ActiveEditDialogContainer activeEditContainer = ActiveEditDialogsList[containerIndex];
        if (activeEditContainer.EditDialog != null)
          activeEditContainer.EditDialog.Close();
        if (ActiveEditDialogsList.Contains(activeEditContainer))
          ActiveEditDialogsList.Remove(activeEditContainer);
        if (listCount != ActiveEditDialogsList.Count)
        {
          listCount = ActiveEditDialogsList.Count;
          containerIndex--;
        }
      }
      ActiveEditDialogsList.Clear();
      ActiveEditDialogsList = null;
    }

    public void CloseConnection()
    {
      connection = null;
      welcomePanel1.BringToFront();

      // Free up open Edit Dialogs
      CloseAllEditingSessions();
    }

    public void OpenSchema(string schema)
    {
      connection.Schema = schema;
      dbObjectSelectionPanel1.SetConnection(connection);
      dbObjectSelectionPanel1.BringToFront();
    }

    public void CloseSchema()
    {
      // If there are Active Edit sessions warn the users that by closing the schema the sessions will be terminated
      if (ActiveEditDialogsList != null && ActiveEditDialogsList.Count > 0)
      {
        WarningDialog warningDiag = new WarningDialog(Resources.ActiveEditingSessionsCloseWarningTitle, Resources.ActiveEditingSessionsCloseWarningDetail);
        DialogResult dr = warningDiag.ShowDialog();
        if (dr == DialogResult.No)
          return;
        CloseAllEditingSessions();
      }
      schemaSelectionPanel1.BringToFront();
    }

    public string GetWorksheetNameAvoidingDuplicates(string proposedName, int copyIndex)
    {
      string retName = (copyIndex > 0 ? String.Format("Copy {0} of {1}", copyIndex, proposedName) : proposedName);
      if (excelApplication.ActiveWorkbook == null)
        return retName;
      foreach (Excel.Worksheet ws in excelApplication.Worksheets)
      {
        if (ws.Name == retName)
          return GetWorksheetNameAvoidingDuplicates(proposedName, copyIndex + 1);
      }
      return retName;
    }

    public string GetWorksheetNameAvoidingDuplicates(string proposedName)
    {
      return GetWorksheetNameAvoidingDuplicates(proposedName, 0);
    }

    public Excel.Worksheet GetActiveOrCreateWorksheet(string proposedName, bool alwaysCreate, bool checkForDuplicates)
    {
      Excel.Worksheet currentWorksheet = excelApplication.ActiveSheet as Excel.Worksheet;
      if (currentWorksheet != null && !alwaysCreate)
        return currentWorksheet;
      proposedName = (checkForDuplicates ? GetWorksheetNameAvoidingDuplicates(proposedName) : proposedName);
      if (excelApplication.ActiveWorkbook != null)
      {
        Excel.Worksheet currentActiveSheet = ActiveWorksheet;
        currentWorksheet = excelApplication.Sheets.Add(Type.Missing, excelApplication.ActiveSheet, Type.Missing, Type.Missing);
        if (ActiveEditDialogsList != null)
        {
          ActiveEditDialogContainer activeEditContainer = ActiveEditDialogsList.Find(ac => ac.EditDialog.EditingWorksheet.Equals(currentActiveSheet));
          if (activeEditContainer != null && activeEditContainer.EditDialog.Visible)
            activeEditContainer.EditDialog.Hide();
        }
      }
      else
      {
        Excel.Workbook currentWorkbook = excelApplication.Workbooks.Add(Type.Missing);
        currentWorksheet = (currentWorkbook.Worksheets[1] as Excel.Worksheet);
      }
      currentWorksheet.Name = proposedName;
      return currentWorksheet;
    }

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
          int startingRow = (importColumnNames ? 1 : 0);
          int cappedNumRows = (ActiveWorkbook.Excel8CompatibilityMode ? Math.Min(rowsCount + startingRow, UInt16.MaxValue - currentRow) : rowsCount + startingRow);

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
              fillingArray[fillingRowIdx, currCol] = DataTypeUtilities.GetImportingValueForDateType(dt.Rows[currRow][currCol]);
            }
            fillingRowIdx++;
          }
          fillingRange.set_Value(Type.Missing, fillingArray);
          fillingRange.Columns.AutoFit();
          excelApplication_SheetSelectionChange(currentSheet, excelApplication.ActiveCell);
        }
      }
      catch (Exception ex)
      {
        using (var errorDialog = new InfoDialog(false, "An error ocurred when trying to import the data.", ex.Message))
        {
          errorDialog.WordWrapDetails = true;
          errorDialog.ShowDialog();
          MiscUtilities.WriteAppErrorToLog(ex);
        }
      }

      return fillingRange;
    }

    public void ImportDataToExcel(DataTable dt, bool importColumnNames)
    {
      ImportDataTableToExcelAtGivenCell(dt, importColumnNames, excelApplication.ActiveCell);
    }

    public void ImportDataToExcel(DataSet ds, bool importColumnNames, ImportMultipleType importType, int selectedResultSet)
    {
      Excel.Range atCell = excelApplication.ActiveCell;
      Excel.Range endCell = null;
      Excel.Range fillingRange = null;

      int tableIdx = 0;
      foreach (DataTable dt in ds.Tables)
      {
        if (importType == ImportMultipleType.SelectedResultSet && selectedResultSet < tableIdx)
          continue;
        tableIdx++;
        fillingRange = ImportDataTableToExcelAtGivenCell(dt, importColumnNames, atCell);
        if (fillingRange != null)
          endCell = fillingRange.Cells[fillingRange.Rows.Count, fillingRange.Columns.Count] as Excel.Range;
        else
          continue;
        if (tableIdx < ds.Tables.Count)
          switch (importType)
          {
            case ImportMultipleType.AllResultSetsHorizontally:
              atCell = endCell.get_Offset(atCell.Row - endCell.Row, 2);
              break;
            case ImportMultipleType.AllResultSetsVertically:
              if (ActiveWorkbook.Excel8CompatibilityMode && endCell.Row + 2 > UInt16.MaxValue)
                return;
              atCell = endCell.get_Offset(2, atCell.Column - endCell.Column);
              break;
          }
      }
    }

    public bool AppendDataToTable(DBObject toTableObject)
    {
      DialogResult dr = DialogResult.Cancel;
      Excel.Range exportRange = excelApplication.Selection as Excel.Range;

      if (exportRange.Areas.Count > 1)
      {
        WarningDialog warningDiag = new WarningDialog(WarningDialog.WarningButtons.OK, Resources.MultipleAreasNotSupportedWarningTitle, Resources.MultipleAreasNotSupportedWarningDetail);
        warningDiag.ShowDialog();
        return false;
      }

      if (toTableObject != null)
      {
        this.Cursor = Cursors.WaitCursor;
        AppendDataForm appendDataForm = new AppendDataForm(connection, exportRange, toTableObject, ActiveWorksheet.Name);
        this.Cursor = Cursors.Default;
        dr = appendDataForm.ShowDialog();
      }
      else
      {
        this.Cursor = Cursors.WaitCursor;
        ExportDataForm exportForm = new ExportDataForm(connection, exportRange, ActiveWorksheet.Name);
        this.Cursor = Cursors.Default;
        dr = exportForm.ShowDialog();
      }
      return dr == DialogResult.OK;
    }

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
        InfoDialog errorDialog = new InfoDialog(false, "Error while creating new Excel Worksheet", ex.Message);
        errorDialog.WordWrapDetails = true;
        errorDialog.ShowDialog();
        MiscUtilities.WriteAppErrorToLog(ex);
      }

      return newWorksheet;
    }

    public bool EditTableData(DBObject tableObject)
    {
      InfoDialog errorDialog = null;
      string schemaAndTableNames = String.Format("{0}.{1}", connection.Schema, tableObject.Name);     

      // Check if the current dbobject has an edit ongoing 
      if (TableHasEditOnGoing(tableObject.Name))
      {
        // Display an error since there is an ongoing Editing operation and return
        errorDialog = new InfoDialog(false, String.Format(Properties.Resources.TableWithOperationOngoingError, schemaAndTableNames), null);
        errorDialog.OperationStatusText = "Editing not possible";
        errorDialog.ShowDialog();        
        return false;
      }
      
      // Check if selected Table has a Primary Key, it it does not we prompt an error and exit since Editing on such table is not permitted
      if (!MySQLDataUtilities.TableHasPrimaryKey(connection, tableObject.Name))
      {
        errorDialog = new InfoDialog(false, Properties.Resources.EditOpenSummaryError, Properties.Resources.EditOpenDetailsError);
        errorDialog.OperationStatusText = Properties.Resources.EditOpenSatusError;
        errorDialog.OperationSummarySubText = String.Empty;
        errorDialog.WordWrapDetails = true;
        errorDialog.ShowDialog();
        return false;
      }
     
      // Attempt to Import Data unless the user cancels the import operation
      string proposedWorksheetName = GetWorksheetNameAvoidingDuplicates(tableObject.Name);
      ImportTableViewForm importForm = new ImportTableViewForm(connection, tableObject, proposedWorksheetName, ActiveWorkbook.Excel8CompatibilityMode, true);
      DialogResult dr = importForm.ShowDialog();
      if (dr == DialogResult.Cancel)
        return false;
      
      if (importForm.ImportDataTable == null || importForm.ImportDataTable.Columns == null || importForm.ImportDataTable.Columns.Count == 0)
      {
        errorDialog = new InfoDialog(false, String.Format(Properties.Resources.UnableToRetrieveData, tableObject.Name), null);
        errorDialog.ShowDialog();
        return false;
      }

      // Before creating the new Excel Worksheet check if ActiveWorksheet is in Editing Mode and if so hide its Edit Dialog
      if (ActiveEditDialogsList != null)
      {
        ActiveEditDialogContainer activeEditContainer = ActiveEditDialogsList.Find(ac => ac.EditDialog.EditingWorksheet.Equals(ActiveWorksheet));
          if (activeEditContainer != null && activeEditContainer.EditDialog.Visible)
            activeEditContainer.EditDialog.Hide();
      }

      // Create the new Excel Worksheet and import the editing data there
      Excel.Worksheet currentWorksheet = CreateNewWorksheet(proposedWorksheetName, false);
      if (currentWorksheet == null)
        return false;
      Excel.Range atCell = currentWorksheet.Cells[1, 1];
      Excel.Range editingRange = ImportDataTableToExcelAtGivenCell(importForm.ImportDataTable, importForm.ImportHeaders, atCell);
      
      // Create and show the Edit Data Dialog
      MySQLDataUtilities.AddExtendedProperties(ref importForm.ImportDataTable, importForm.ImportDataTable.ExtendedProperties["QueryString"].ToString(), importForm.ImportHeaders, tableObject.Name);
      editDialog = new EditDataDialog(connection, editingRange, importForm.ImportDataTable, currentWorksheet, true);
      editDialog.ParentWindow = new NativeWindowWrapper(excelApplication.Hwnd);
      editDialog.CallerTaskPane = this;
      editDialog.Show(editDialog.ParentWindow);

      // Maintain hashtables for open Edit Data Dialogs
      if (ActiveEditDialogsList == null)
        ActiveEditDialogsList = new List<ActiveEditDialogContainer>();
      ActiveEditDialogsList.Add(new ActiveEditDialogContainer(tableObject.Name, editDialog));
     
      return true;
    }

    public Excel.Range IntersectRanges(Excel.Range r1, Excel.Range r2)
    {
      return excelApplication.Intersect(r1, r2);
    }

    public void CloseAddIn()
    {
      CloseConnection();
      Globals.ThisAddIn.TaskPane.Visible = false;
    }

    /// <summary>
    /// Checks if there is an Editing Operation active for a Schema.Table
    /// release resources
    /// </summary>
    /// <param name="dbObjectSelectedName"></param>
    /// <returns>true if the program has an edit on going</returns>
    public bool TableHasEditOnGoing(string tableName)
    {
      if (ActiveEditDialogsList == null || ActiveEditDialogsList.Count == 0)
        return false;
      ActiveEditDialogContainer editContainer = ActiveEditDialogsList.Find(ac => ac.TableName == tableName);
      if (editContainer == null)
        return false;
      // Means has an edit ongoing we need to make sure the edit has a valid sheet otherwise we need to release it
      foreach (Excel.Worksheet workSheet in excelApplication.Worksheets)
      {
        if (editContainer.EditDialog.EditingWorksheet.Equals(workSheet))
          return true;
      }
      editContainer.EditDialog.Close();
      if (ActiveEditDialogsList.Contains(editContainer))
        ActiveEditDialogsList.Remove(editContainer);
      return false;
    }

  }

  public class ActiveEditDialogContainer
  {
    public string TableName;
    public EditDataDialog EditDialog;

    public ActiveEditDialogContainer(string tableName, EditDataDialog editDialog)
    {
      TableName = tableName;
      EditDialog = editDialog;
    }
  }

  class NativeWindowWrapper : IWin32Window
  {
    IntPtr _handle;

    public NativeWindowWrapper(int Hwnd)
    {
      _handle = new IntPtr(Hwnd);
    }

    public IntPtr Handle
    {
      get { return _handle; }
    }
  }

}
