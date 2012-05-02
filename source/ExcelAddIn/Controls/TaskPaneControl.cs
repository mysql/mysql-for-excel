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

      bool emptyRange = Target.SpecialCells(Excel.XlCellType.xlCellTypeBlanks).Count == Target.Count;
      dbObjectSelectionPanel1.ExportDataActionEnabled = !emptyRange;
    }

    public void OpenConnection(MySqlWorkbenchConnection connection)
    {
      this.connection = connection;
      if (connection.Password == null)
      {
        PasswordForm dlg = new PasswordForm();
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
      dbObjectSelectionPanel1.SetConnectionAndSchema(connection, schema);
      dbObjectSelectionPanel1.BringToFront();
    }

    public void CloseSchema()
    {
      schemaSelectionPanel1.BringToFront();
    }

    //bool dbObjectSelectionPanel1_DBObjectSelectionPanelLeaving(object sender, DBObjectSelectionPanelLeavingArgs args)
    //{
    //  bool success = false;

    //  switch (args.SelectedAction)
    //  {
    //    case DBObjectSelectionPanelLeavingAction.Back:
    //      schemaInfo.CurrentSchema = String.Empty;
    //      welcomePanel1.Visible = false;
    //      schemaSelectionPanel1.Visible = true;
    //      dbObjectSelectionPanel1.Visible = false;
    //      success = true;
    //      break;
    //    case DBObjectSelectionPanelLeavingAction.Close:
    //      CloseAddIn();
    //      success = true;
    //      break;
    //    case DBObjectSelectionPanelLeavingAction.Import:
    //      success = importDataToExcel(args.DataForExcel);
    //      break;
    //    case DBObjectSelectionPanelLeavingAction.Edit:
    //      break;
    //    case DBObjectSelectionPanelLeavingAction.Append:
    //      success = appendDataToTable();
    //      break;
    //  }

    //  return success;
    //}

    private bool importDataToExcel(DataTable dt)
    {
      bool success = false;
      if (dt != null && dt.Rows.Count > 0)
      {
        int rowsCount = dt.Rows.Count;
        int colsCount = dt.Columns.Count;
        Excel.Worksheet currentSheet = excelApplication.ActiveSheet as Excel.Worksheet;
        Excel.Range currentCell = excelApplication.ActiveCell;
        Excel.Range fillingRange = currentCell.get_Resize(rowsCount, colsCount);
        string[,] fillingArray = new string[rowsCount, colsCount];

        for (int currRow = 0; currRow < rowsCount; currRow++)
        {
          for (int currCol = 0; currCol < colsCount; currCol++)
          {
            fillingArray[currRow, currCol] = dt.Rows[currRow][currCol].ToString();
          }
        }
        fillingRange.set_Value(Type.Missing, fillingArray);
      }
      return success;
    }

    private bool appendDataToTable()
    {
      bool success = false;
      if (excelApplication.Selection is Excel.Range)
      {
        Excel.Range selectedRange = excelApplication.Selection as Excel.Range;

      }
      return success;
    }

    public void CloseAddIn()
    {
//      Globals.ThisAddIn.TaskPane.Visible = false;
      welcomePanel1.Visible = true;
      schemaSelectionPanel1.Visible = false;
      dbObjectSelectionPanel1.Visible = false;

      //schemaInfo.Clear();
    }
  }

}
