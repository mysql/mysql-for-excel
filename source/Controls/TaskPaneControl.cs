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

    public Hashtable WorkSheetEditFormsHashtable;
    public Hashtable TableNameEditFormsHashtable;
    public Excel.Worksheet ActiveWorksheet
    {
      get { return ((Excel.Worksheet)excelApplication.ActiveSheet); }
    }

    public TaskPaneControl(Excel.Application app)
    {
      excelApplication = app;
      excelApplication.SheetSelectionChange += new Excel.AppEvents_SheetSelectionChangeEventHandler(excelApplication_SheetSelectionChange);
      excelApplication.SheetActivate += new Excel.AppEvents_SheetActivateEventHandler(excelApplication_SheetActivate);
      excelApplication.SheetDeactivate += new Excel.AppEvents_SheetDeactivateEventHandler(excelApplication_SheetDeactivate);
      
      InitializeComponent();

      dbObjectSelectionPanel1.ExcelSelectionContainsData = false;
    }

    void excelApplication_SheetDeactivate(object Sh)
    {
      Excel.Worksheet deactivatedSheet = Sh as Excel.Worksheet;
      lastDeactivatedSheetName = (deactivatedSheet != null ? deactivatedSheet.Name : String.Empty);
      if (lastDeactivatedSheetName.Length > 0 && WorkSheetEditFormsHashtable != null && WorkSheetEditFormsHashtable.Contains(lastDeactivatedSheetName))
      {
        editDialog = WorkSheetEditFormsHashtable[lastDeactivatedSheetName] as EditDataDialog;
        editDialog.Hide();
      }
    }

    void excelApplication_SheetActivate(object Sh)
    {
      Excel.Worksheet activeSheet = Sh as Excel.Worksheet;
      string activeSheetName = (activeSheet != null ? activeSheet.Name : String.Empty);
      if (activeSheetName.Length > 0 && WorkSheetEditFormsHashtable != null && WorkSheetEditFormsHashtable.Contains(activeSheetName))
      {
        editDialog = WorkSheetEditFormsHashtable[activeSheetName] as EditDataDialog;
        editDialog.Show(editDialog.ParentWindow);
      }

      if (lastDeactivatedSheetName.Length > 0 && !WorksheetExists(lastDeactivatedSheetName))
      {
        // Worksheet was deleted or renamed
        if (WorkSheetEditFormsHashtable != null && WorkSheetEditFormsHashtable.ContainsKey(lastDeactivatedSheetName))
        {
          EditDataDialog editDlg = WorkSheetEditFormsHashtable[lastDeactivatedSheetName] as EditDataDialog;
          if (editDlg != null)
          {
            editDlg.Close();
            editDlg.Dispose();
          }
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

    public bool WorksheetExists(string workSheetName)
    {
      bool exists = false;

      if (workSheetName.Length > 0)
      {
        // Maybe the last deactivated sheet has been deleted?
        try
        {
          Excel._Worksheet wSheet = excelApplication.Worksheets[workSheetName] as Excel.Worksheet;
          exists = true;
        }
        catch
        {
          exists = false;
        }
      }

      return exists;
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
        WarningDialog warningDlg = new WarningDialog(WarningDialog.WarningButtons.OK, Resources.ConnectFailedTitleWarning, Resources.ConnectFailedDetailWarning);
        warningDlg.StartPosition = FormStartPosition.CenterScreen;
        warningDlg.ShowDialog();
        failed = true;
      }
      bool schemasLoaded = schemaSelectionPanel1.SetConnection(connection);
      if (schemasLoaded)
        schemaSelectionPanel1.BringToFront();
    }

    public void CloseConnection()
    {
      connection = null;
      welcomePanel1.BringToFront();
    }

    public void OpenSchema(string schema)
    {
      connection.Schema = schema;
      dbObjectSelectionPanel1.SetConnection(connection);
      dbObjectSelectionPanel1.BringToFront();
    }

    public void CloseSchema()
    {
      schemaSelectionPanel1.BringToFront();
    }

    private string GetWorksheetNameAvoidingDuplicates(string proposedName)
    {
      if (excelApplication.ActiveWorkbook != null)
      {
        int i = 0;
        foreach (Excel.Worksheet ws in excelApplication.Worksheets)
        {
          if (ws.Name.Contains(proposedName))
            i++;
        }
        if (i > 0)
          proposedName = String.Format("Copy ({0}) of {1}", i, proposedName);
      }
      return proposedName;
    }

    public Excel.Worksheet GetActiveOrCreateWorksheet(string proposedName, bool alwaysCreate, bool checkForDuplicates)
    {
      Excel.Worksheet currentWorksheet = excelApplication.ActiveSheet as Excel.Worksheet;
      if (currentWorksheet != null && !alwaysCreate)
        return currentWorksheet;
      proposedName = (checkForDuplicates ? GetWorksheetNameAvoidingDuplicates(proposedName) : proposedName);
      if (excelApplication.ActiveWorkbook != null)
        currentWorksheet = excelApplication.Sheets.Add(Type.Missing, excelApplication.ActiveSheet, Type.Missing, Type.Missing);
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
        if (dt != null && dt.Rows.Count > 0)
        {
          int rowsCount = dt.Rows.Count;
          int colsCount = dt.Columns.Count;
          int startingRow = (importColumnNames ? 1 : 0);

          Excel.Worksheet currentSheet = ActiveWorksheet;
          fillingRange = atCell.get_Resize(rowsCount + startingRow, colsCount);
          object[,] fillingArray = new object[rowsCount + startingRow, colsCount];

          if (importColumnNames)
          {
            for (int currCol = 0; currCol < colsCount; currCol++)
            {
              fillingArray[0, currCol] = dt.Columns[currCol].ColumnName;
            }
          }

          int fillingRowIdx = startingRow;
          for (int currRow = 0; currRow < rowsCount; currRow++)
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
        using (var errorDialog = new InfoDialog(false, "Operation Error", "An error ocurred when trying to import the data."))
        {
          errorDialog.ShowDialog();
          MiscUtilities.GetSourceTrace().WriteError("Application Exception - " + (ex.Message + " " + ex.InnerException), 1);
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
      Excel.Worksheet currentWorksheet = GetActiveOrCreateWorksheet("Sheet1", false, true);

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
              atCell = endCell.get_Offset(2, atCell.Column - endCell.Column);
              break;
          }
      }
    }

    public bool AppendDataToTable(DBObject toTableObject)
    {
      DialogResult dr = DialogResult.Cancel;
      Excel.Range exportRange = excelApplication.Selection as Excel.Range;

      if (toTableObject != null)
      {
        AppendDataForm appendDataForm = new AppendDataForm(connection, exportRange, toTableObject, ActiveWorksheet.Name);
        dr = appendDataForm.ShowDialog();
      }
      else
      {
        ExportDataForm exportForm = new ExportDataForm(connection, exportRange, ActiveWorksheet.Name);
        dr = exportForm.ShowDialog();
      }
      return dr == DialogResult.OK;
    }

    public bool EditTableData(DBObject tableObject)
    {
      // Check if selected Table has a Primary Key, it it does not we prompt an error and exit since Editing on such table is not permitted
      if (!MySQLDataUtilities.TableHasPrimaryKey(connection, tableObject.Name))
      {
        InfoDialog infoDialog = new InfoDialog(false, Properties.Resources.EditOpenSummaryError, Properties.Resources.EditOpenDetailsError);
        infoDialog.OperationStatusText = Properties.Resources.EditOpenSatusError;
        infoDialog.OperationSummarySubText = String.Empty;
        infoDialog.WordWrapDetails = true;
        infoDialog.ShowDialog();
        return false;
      }

      // Check if selected Table has already an Editing operation ongoing that has not been closed, if so prompt error and exit
      string schemaAndTableNames = String.Format("{0}.{1}", connection.Schema, tableObject.Name);
      if (TableNameEditFormsHashtable != null && TableNameEditFormsHashtable.ContainsKey(schemaAndTableNames))
      {
        InfoDialog infoDialog = new InfoDialog(false, String.Format(Properties.Resources.TableWithOperationOngoingError, schemaAndTableNames), null);
        infoDialog.OperationStatusText = "Editing not possible";
        infoDialog.ShowDialog();
        return false;
      }

      // Attempt to Import Data unless the yser cancels the import operation
      string proposedWorksheetName = GetWorksheetNameAvoidingDuplicates(tableObject.Name);
      ImportTableViewForm importForm = new ImportTableViewForm(connection, tableObject, proposedWorksheetName);
      DialogResult dr = importForm.ShowDialog();
      if (dr == DialogResult.Cancel)
        return false;
      if (importForm.ImportDataTable == null)
      {
        string msg = String.Format(Properties.Resources.UnableToRetrieveData, tableObject.Name);
        MessageBox.Show(msg, Properties.Resources.Error, MessageBoxButtons.OK, MessageBoxIcon.Error);
        return false;
      }

      // Before creating the new Excel Worksheet check if ActiveWorksheet is in Editing Mode and if so hide its Edit Dialog
      if (WorkSheetEditFormsHashtable != null && WorkSheetEditFormsHashtable.ContainsKey(ActiveWorksheet.Name))
      {
        EditDataDialog editDlg = WorkSheetEditFormsHashtable[ActiveWorksheet.Name] as EditDataDialog;
        if (editDlg != null)
          editDlg.Hide();
      }

      // Create the new Excel Worksheet and import the editing data there
      Excel.Worksheet currentWorksheet = GetActiveOrCreateWorksheet(proposedWorksheetName, true, false);
      currentWorksheet.Activate();
      Excel.Range atCell = currentWorksheet.get_Range("A1", Type.Missing);
      atCell.Select();
      Excel.Range editingRange = ImportDataTableToExcelAtGivenCell(importForm.ImportDataTable, importForm.ImportHeaders, atCell);
      
      // Create and show the Edit Data Dialog
      MySQLDataUtilities.AddExtendedProperties(ref importForm.ImportDataTable, importForm.ImportDataTable.ExtendedProperties["QueryString"].ToString(), importForm.ImportHeaders, tableObject.Name);
      editDialog = new EditDataDialog(connection, editingRange, importForm.ImportDataTable, currentWorksheet, true);
      editDialog.ParentWindow = new NativeWindowWrapper(excelApplication.Hwnd);
      editDialog.CallerTaskPane = this;
      editDialog.Show(editDialog.ParentWindow);

      // Maintain hashtables for open Edit Data Dialogs
      if (WorkSheetEditFormsHashtable == null)
        WorkSheetEditFormsHashtable = new Hashtable();
      WorkSheetEditFormsHashtable.Add(currentWorksheet.Name, editDialog);
      if (TableNameEditFormsHashtable == null)
        TableNameEditFormsHashtable = new Hashtable();
      TableNameEditFormsHashtable.Add(schemaAndTableNames, editDialog);

      return true;
    }

    public Excel.Range IntersectRanges(Excel.Range r1, Excel.Range r2)
    {
      return excelApplication.Intersect(r1, r2);
    }

    public void CloseAddIn()
    {
      CloseConnection();
      if (TableNameEditFormsHashtable != null)
      {
        foreach (string key in TableNameEditFormsHashtable.Keys)
        {
          EditDataDialog editDlg = TableNameEditFormsHashtable[key] as EditDataDialog;
          if (editDlg != null)
          {
            editDlg.Close();
            editDlg.Dispose();
          }
        }
        TableNameEditFormsHashtable.Clear();
        TableNameEditFormsHashtable = null;
      }
      if (WorkSheetEditFormsHashtable != null)
      {
        WorkSheetEditFormsHashtable.Clear();
        WorkSheetEditFormsHashtable = null;
      }
      Globals.ThisAddIn.TaskPane.Visible = false;
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
