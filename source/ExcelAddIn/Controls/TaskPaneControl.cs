using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using MySql.Data.MySqlClient;
using Excel = Microsoft.Office.Interop.Excel;
using MySQL.Utility;

namespace MySQL.ExcelAddIn
{
  public partial class TaskPaneControl : UserControl
  {
    private Excel.Application excelApplication;
    private MySqlWorkbenchConnection connection;
    //private MySQLSchemaInfo schemaInfo;

    public Excel.Worksheet ActiveWorksheet
    {
      get { return ((Excel.Worksheet)excelApplication.ActiveSheet); }
    }

    public TaskPaneControl(Excel.Application app)
    {
      excelApplication = app;
      excelApplication.SheetSelectionChange += new Excel.AppEvents_SheetSelectionChangeEventHandler(excelApplication_SheetSelectionChange);
      InitializeComponent();
    }

    void excelApplication_SheetSelectionChange(object Sh, Excel.Range Target)
    {
      if (!this.Visible)
        return;

      int selectedCellsCount = Target.Count;
      int blankCellsInRangeCount = Target.SpecialCells(Excel.XlCellType.xlCellTypeBlanks).Count;
      bool emptyRange = selectedCellsCount == blankCellsInRangeCount;
      dbObjectSelectionPanel1.ExportDataActionEnabled = !emptyRange;
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
      schemaSelectionPanel1.SetConnection(connection);
      schemaSelectionPanel1.BringToFront();
    }

    public void CloseConnection()
    {
      welcomePanel1.BringToFront();
      connection = null;
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

    private Excel.Range importDataTableToExelAtGivenCell(DataTable dt, bool importColumnNames, Excel.Range atCell)
    {
      Excel.Range endCell = null;

      if (dt != null && dt.Rows.Count > 0)
      {
        int rowsCount = dt.Rows.Count;
        int colsCount = dt.Columns.Count;
        int startingRow = (importColumnNames ? 1 : 0);

        Excel.Worksheet currentSheet = excelApplication.ActiveSheet as Excel.Worksheet;
        Excel.Range currentCell = excelApplication.ActiveCell;
        Excel.Range fillingRange = currentCell.get_Resize(rowsCount + startingRow, colsCount);
        string[,] fillingArray = new string[rowsCount + startingRow, colsCount];

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
            fillingArray[fillingRowIdx, currCol] = dt.Rows[currRow][currCol].ToString();
          }
          fillingRowIdx++;
        }
        fillingRange.set_Value(Type.Missing, fillingArray);
        endCell = fillingRange.Cells[fillingRange.Rows.Count, fillingRange.Columns.Count] as Excel.Range;
      }

      return endCell;
    }

    public void ImportDataToExcel(DataTable dt, bool importColumnNames)
    {
      importDataTableToExelAtGivenCell(dt, importColumnNames, excelApplication.ActiveCell);
    }

    public void ImportDataToExcel(DataSet ds, bool importColumnNames, ImportMultipleType importType)
    {
      Excel.Range atCell = excelApplication.ActiveCell;
      Excel.Range endCell = null;

      foreach (DataTable dt in ds.Tables)
      {
        endCell = importDataTableToExelAtGivenCell(dt, importColumnNames, atCell);
        switch (importType)
        {
          case ImportMultipleType.SingleWorkSheetHorizontally:
            endCell = endCell.get_Offset(0, 2);
            break;
          case ImportMultipleType.SingleWorkSheetVertically:
            endCell = endCell.get_Offset(2, 0);
            break;
          case ImportMultipleType.MultipleWorkSheets:
            Excel.Worksheet wSheet = excelApplication.Sheets.Add(Type.Missing, excelApplication.ActiveSheet, Type.Missing, Type.Missing);
            wSheet.Activate();
            endCell = wSheet.get_Range("A1", Type.Missing);
            endCell.Select();
            break;
        }
      }
    }

    public void AppendDataToTable(string toTableName)
    {
      ExportDataToTableDialog exportDataForm = new ExportDataToTableDialog(connection, connection.Schema, toTableName, excelApplication.Selection as Excel.Range);
      DialogResult dr = exportDataForm.ShowDialog();
    }

    public void CloseAddIn()
    {
      //      Globals.ThisAddIn.TaskPane.Visible = false;
      welcomePanel1.Visible = true;
      schemaSelectionPanel1.Visible = false;
      dbObjectSelectionPanel1.Visible = false;
    }
  }

}
