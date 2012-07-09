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

namespace MySQL.ForExcel
{
  public partial class TaskPaneControl : UserControl
  {
    private Excel.Application excelApplication;
    private MySqlWorkbenchConnection connection;
    private EditDataDialog editDialog = null;

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
      Excel.Worksheet currentSheet = Sh as Excel.Worksheet;
      if (WorkSheetEditFormsHashtable != null && WorkSheetEditFormsHashtable.Contains(currentSheet.Name))
      {
        editDialog = WorkSheetEditFormsHashtable[currentSheet.Name] as EditDataDialog;
        editDialog.Hide();
      }
    }

    void excelApplication_SheetActivate(object Sh)
    {
      Excel.Worksheet currentSheet = Sh as Excel.Worksheet;
      if (WorkSheetEditFormsHashtable != null && WorkSheetEditFormsHashtable.Contains(currentSheet.Name))
      {
        editDialog = WorkSheetEditFormsHashtable[currentSheet.Name] as EditDataDialog;
        editDialog.Show();
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

    public void OpenConnection(MySqlWorkbenchConnection connection)
    {
      this.connection = connection;
      if (connection.Password == null)
      {
        PasswordDialog dlg = new PasswordDialog();
        dlg.HostIdentifier = connection.HostIdentifier;
        dlg.UserName = connection.UserName;
        dlg.PasswordText = String.Empty;
        if (dlg.ShowDialog() == DialogResult.Cancel) return;
        connection.Password = dlg.PasswordText;
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

    public Excel.Worksheet GetActiveOrCreateWorksheet(string proposedName, bool alwaysCreate)
    {
      Excel.Worksheet currentWorksheet = excelApplication.ActiveSheet as Excel.Worksheet;
      if (currentWorksheet != null && !alwaysCreate)
        return currentWorksheet;
      if (excelApplication.ActiveWorkbook != null)
      {
        currentWorksheet = excelApplication.Sheets.Add(Type.Missing, excelApplication.ActiveSheet, Type.Missing, Type.Missing);
        int i = 0;
        foreach (Excel.Worksheet ws in excelApplication.Worksheets)
        {
          if (ws.Name.Contains(proposedName))
            i++;
        }
        if (i > 0)
          proposedName = String.Format("Copy ({0}) of {1}", i, proposedName);
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
            fillingArray[fillingRowIdx, currCol] = dt.Rows[currRow][currCol];
          }
          fillingRowIdx++;
        }
        fillingRange.set_Value(Type.Missing, fillingArray);
        excelApplication_SheetSelectionChange(currentSheet, excelApplication.ActiveCell);
      }

      return fillingRange;
    }

    public void ImportDataToExcel(DataTable dt, bool importColumnNames)
    {
      ImportDataTableToExcelAtGivenCell(dt, importColumnNames, excelApplication.ActiveCell);
    }

    public void ImportDataToExcel(DataSet ds, bool importColumnNames, ImportMultipleType importType, int selectedResultSet)
    {
      Excel.Worksheet currentWorksheet = GetActiveOrCreateWorksheet("Sheet1", false);

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
        AppendDataForm appendDataForm = new AppendDataForm(connection, exportRange, toTableObject, ActiveWorksheet);
        dr = appendDataForm.ShowDialog();
      }
      else
      {
        ExportDataForm exportForm = new ExportDataForm(connection, exportRange, ActiveWorksheet);
        dr = exportForm.ShowDialog();
      }
      return dr == DialogResult.OK;
    }

    public bool EditTableData(DBObject tableObject)
    {
      if (TableNameEditFormsHashtable != null && TableNameEditFormsHashtable.Contains(tableObject.Name))
      {
        Utilities.ShowErrorBox(String.Format("Table {0} already has an Edit operation ongoing.", tableObject.Name));
        return false;
      }

      Excel.Worksheet currentWorksheet = GetActiveOrCreateWorksheet(tableObject.Name, true);
      currentWorksheet.Activate();
      Excel.Range atCell = currentWorksheet.get_Range("A1", Type.Missing);
      atCell.Select();

      // Import Data
      ImportTableViewForm importForm = new ImportTableViewForm(connection, tableObject, currentWorksheet);
      DialogResult dr = importForm.ShowDialog();
      if (dr == DialogResult.Cancel)
        return false;
      if (importForm.ImportDataTable == null)
      {
        string msg = String.Format(Properties.Resources.UnableToRetrieveData, tableObject.Name);
        MessageBox.Show(msg, Properties.Resources.Error, MessageBoxButtons.OK, MessageBoxIcon.Error);
        return false;
      }
      Excel.Range editingRange = ImportDataTableToExcelAtGivenCell(importForm.ImportDataTable, importForm.ImportHeaders, atCell);
      
      // Edit Data
      Utilities.AddExtendedProperties(ref importForm.ImportDataTable, importForm.ImportDataTable.ExtendedProperties["QueryString"].ToString(), importForm.ImportHeaders, tableObject.Name);
      editDialog = new EditDataDialog(connection, editingRange, importForm.ImportDataTable, currentWorksheet);
      editDialog.CallerTaskPane = this;
      editDialog.Show(new NativeWindowWrapper(excelApplication.Hwnd));

      // Maintain hashtables for open Edit Data Dialogs
      if (WorkSheetEditFormsHashtable == null)
        WorkSheetEditFormsHashtable = new Hashtable();
      WorkSheetEditFormsHashtable.Add(tableObject.Name, editDialog);
      if (TableNameEditFormsHashtable == null)
        TableNameEditFormsHashtable = new Hashtable();
      TableNameEditFormsHashtable.Add(tableObject.Name, editDialog);

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
          editDlg.Close();
          editDlg.Dispose();
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
