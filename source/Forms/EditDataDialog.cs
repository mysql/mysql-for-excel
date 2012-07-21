using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using MySQL.Utility;
using MySql.Data.MySqlClient;
using System.Windows.Forms;
using Excel = Microsoft.Office.Interop.Excel;
using System.Runtime.InteropServices;

namespace MySQL.ForExcel
{
  public partial class EditDataDialog : AutoStyleableBaseForm
  {
    private const int SW_SHOWNOACTIVATE = 4;
    private const int HWND_TOPMOST = -1;
    private const uint SWP_NOACTIVATE = 0x0010;

    private Point mouseDownPoint = Point.Empty;
    private MySqlWorkbenchConnection wbConnection;
    private Excel.Range editDataRange;
    private bool importedHeaders = false;
    private string queryString = String.Empty;
    private MySQLDataTable editMySQLDataTable;
    private MySqlDataAdapter dataAdapter;
    private MySqlConnection connection;
    private List<string> modifiedCellAddressesList;
    private List<string> addedRowAddressesList;
    private List<string> deletedRowAddressesList;
    private int commitedCellsOLEColor = ColorTranslator.ToOle(ColorTranslator.FromHtml("#B8E5F7"));
    private int uncommitedCellsOLEColor = ColorTranslator.ToOle(ColorTranslator.FromHtml("#FF8282"));
    private int newRowCellsOLEColor = ColorTranslator.ToOle(ColorTranslator.FromHtml("#FFFCC7"));
    private int lockedCellsOLEColor = ColorTranslator.ToOle(ColorTranslator.FromHtml("#D7D7D7"));
    private int defaultCellsOLEColor = ColorTranslator.ToOle(Color.White);
    private long editingRowsQuantity = 0;
    private long editingColsQuantity = 0;
    private string editingWorksheetName = String.Empty;

    public Excel.Worksheet EditingWorksheet = null;
    public TaskPaneControl CallerTaskPane;
    public string EditingTableName { get; private set; }
    public IWin32Window ParentWindow { get; set; }
    public bool LockByProtectingWorksheet { get; set; }
    public string SchemaAndTableName
    {
      get { return String.Format("{0}.{1}", wbConnection.Schema, EditingTableName); }
    }

    public EditDataDialog(MySqlWorkbenchConnection wbConnection, Excel.Range editDataRange, DataTable importTable, Excel.Worksheet editingWorksheet, bool protectWorksheet)
    {
      InitializeComponent();

      this.wbConnection = wbConnection;
      this.editDataRange = editDataRange;
      importedHeaders = (bool)importTable.ExtendedProperties["ImportedHeaders"];
      queryString = importTable.ExtendedProperties["QueryString"].ToString();
      EditingTableName = importTable.TableName;
      if (importTable.ExtendedProperties.ContainsKey("TableName") && !String.IsNullOrEmpty(importTable.ExtendedProperties["TableName"].ToString()))
        EditingTableName = importTable.ExtendedProperties["TableName"].ToString();
      editMySQLDataTable = new MySQLDataTable(EditingTableName, importTable, wbConnection);
      if (importTable.ExtendedProperties.ContainsKey("QueryString") && !String.IsNullOrEmpty(importTable.ExtendedProperties["QueryString"].ToString()))
        editMySQLDataTable.SelectQuery = importTable.ExtendedProperties["QueryString"].ToString();
      EditingWorksheet = editingWorksheet;
      editingWorksheetName = editingWorksheet.Name;
      EditingWorksheet.Change += new Excel.DocEvents_ChangeEventHandler(EditingWorksheet_Change);
      EditingWorksheet.SelectionChange += new Excel.DocEvents_SelectionChangeEventHandler(EditingWorksheet_SelectionChange);
      toolTip.SetToolTip(this, String.Format("Editing data for Table {0} on Worksheet {1}", EditingTableName, editingWorksheetName));
      editingRowsQuantity = editingWorksheet.UsedRange.Rows.Count;
      editingColsQuantity = editingWorksheet.UsedRange.Columns.Count;
      Opacity = 0.60;
      LockByProtectingWorksheet = protectWorksheet;
      initializeWorksheetProtection(editDataRange);

      if (editDataRange != null)
      {
        modifiedCellAddressesList = new List<string>(editDataRange.Count);
        addedRowAddressesList = new List<string>(editDataRange.Count);
        deletedRowAddressesList = new List<string>(editDataRange.Count);
      }
      else
      {
        modifiedCellAddressesList = new List<string>();
        addedRowAddressesList = new List<string>();
        deletedRowAddressesList = new List<string>();
      }
    }

    protected override void OnPaintBackground(PaintEventArgs e)
    {
      base.OnPaintBackground(e);
      Pen pen = new Pen(Color.White, 3f);
      e.Graphics.DrawRectangle(pen, 0, 0, this.Width - 2, this.Height - 2);
      pen.Width = 1f;
      e.Graphics.DrawLine(pen, 0, 25, this.Width, 25);
      pen.Dispose();
    }

    protected override void OnClosing(CancelEventArgs e)
    {
      base.OnClosing(e);
      if (CallerTaskPane.WorksheetExists(editingWorksheetName))
        EditingWorksheet.Unprotect(Type.Missing);
      if (CallerTaskPane.TableNameEditFormsHashtable.ContainsKey(SchemaAndTableName))
        CallerTaskPane.TableNameEditFormsHashtable.Remove(SchemaAndTableName);
      if (CallerTaskPane.WorkSheetEditFormsHashtable.ContainsKey(editingWorksheetName))
        CallerTaskPane.WorkSheetEditFormsHashtable.Remove(editingWorksheetName);
    }

    private void initializeWorksheetProtection(Excel.Range permittedRange)
    {
      if (permittedRange != null)
      {
        permittedRange.Locked = false;
        if (importedHeaders)
        {
          Excel.Range headersRange = EditingWorksheet.get_Range("A1");
          headersRange = headersRange.get_Resize(1, permittedRange.Columns.Count);
          lockRange(headersRange, true);
        }
      }
      EditingWorksheet.Protect(Type.Missing,
                               false,
                               true,
                               true,
                               Type.Missing,
                               true,
                               true,
                               false,
                               false,
                               false,
                               false,
                               false,
                               true,
                               false,
                               false,
                               false);
    }

    private void lockRange(Excel.Range range, bool lockRange)
    {
      range.Interior.Color = (lockRange ? lockedCellsOLEColor : defaultCellsOLEColor);
      range.Locked = lockRange;
    }

    private void changeExcelCellsColor(List<string> cellAddressesList, int oleColor)
    {
      Excel.Range modifiedRange = null;
      foreach (string modifiedRangeAddress in cellAddressesList)
      {
        string[] startAndEndRange = modifiedRangeAddress.Split(new char[] { ':' });
        if (startAndEndRange.Length > 1)
          modifiedRange = EditingWorksheet.get_Range(startAndEndRange[0], startAndEndRange[1]);
        else
          modifiedRange = EditingWorksheet.get_Range(modifiedRangeAddress);
        modifiedRange.Interior.Color = oleColor;
      }
      cellAddressesList.Clear();
    }

    private void revertDataChanges(bool refreshFromDB)
    {
      Exception exception = null;
      editMySQLDataTable.RevertData(refreshFromDB, wbConnection, out exception);
      if (exception != null)
      {
        InfoDialog infoDialog = new InfoDialog(false, String.Format("{0} Data Error", (refreshFromDB ? "Refresh" : "Revert")), exception.Message);
        infoDialog.ShowDialog();
      }

      EditingWorksheet.Change -= new Excel.DocEvents_ChangeEventHandler(EditingWorksheet_Change);
      EditingWorksheet.Unprotect();
      Excel.Range topLeftCell = editDataRange.Cells[1, 1];
      editDataRange = CallerTaskPane.ImportDataTableToExcelAtGivenCell(editMySQLDataTable, importedHeaders, topLeftCell);
      changeExcelCellsColor(modifiedCellAddressesList, defaultCellsOLEColor);
      changeExcelCellsColor(addedRowAddressesList, defaultCellsOLEColor);
      changeExcelCellsColor(deletedRowAddressesList, defaultCellsOLEColor);
      btnCommit.Enabled = false;
      EditingWorksheet.Change += new Excel.DocEvents_ChangeEventHandler(EditingWorksheet_Change);
      initializeWorksheetProtection(editDataRange);
    }

    private void pushDataChanges()
    {
      bool success = true;
      int updatedCount = 0;
      Exception exception;
      bool autoCommitOn = chkAutoCommit.Checked;

      string operationSummary = String.Format("Edited data for Table {0} ", EditingTableName);
      StringBuilder operationDetails = new StringBuilder();

      // Added Rows that will be working until 1.1
      DataTable changesTable = editMySQLDataTable.GetChanges(DataRowState.Added);
      int addingRowsCount = (changesTable != null ? changesTable.Rows.Count : 0);
      if (addingRowsCount > 0)
      {
        if (!autoCommitOn)
        {
          operationDetails.AppendFormat("Adding {0} rows to MySQL Table \"{1}\"...{2}{2}", addingRowsCount, editMySQLDataTable.TableName, Environment.NewLine);
          operationDetails.Append(editMySQLDataTable.GetInsertSQL(-1, true, true));
          operationDetails.AppendFormat("{0}{0}", Environment.NewLine);
        }
        updatedCount = editMySQLDataTable.InsertDataWithAdapter(wbConnection, true, out exception);
        success = exception == null;
        if (success)
        {
          changeExcelCellsColor(addedRowAddressesList, commitedCellsOLEColor);
          if (!autoCommitOn)
            operationDetails.AppendFormat("{0} rows have been added successfully.", updatedCount);
        }
        else
        {
          if (!autoCommitOn)
          {
            operationDetails.AppendFormat("{0} rows were added but the following error ocurred.{1}{1}", updatedCount, Environment.NewLine);
            if (exception is MySqlException)
              operationDetails.AppendFormat("MySQL Error {0}:{1}", (exception as MySqlException).Number, Environment.NewLine);
            else
              operationDetails.AppendFormat("ADO.NET Error:{0}", Environment.NewLine);
            operationDetails.Append(exception.Message);
          }
          else
            System.Diagnostics.Debug.WriteLine(exception.Message);
        }
      }
      
      // Delete Rows
      changesTable = editMySQLDataTable.GetChanges(DataRowState.Deleted);
      int deletingRowsCount = (changesTable != null ? changesTable.Rows.Count : 0);
      if (deletingRowsCount > 0)
      {
        if (!autoCommitOn)
        {
          if (operationDetails.Length > 0)
            operationDetails.AppendFormat("{0}{0}", Environment.NewLine);
          operationDetails.AppendFormat("Deleting {0} rows on MySQL Table \"{1}\"...{2}{2}", deletingRowsCount, editMySQLDataTable.TableName, Environment.NewLine);
          operationDetails.Append(editMySQLDataTable.GetDeleteSQL(-1, true));
          operationDetails.AppendFormat("{0}{0}", Environment.NewLine);
        }
        updatedCount = editMySQLDataTable.DeleteDataWithAdapter(wbConnection, out exception);
        success = exception == null;
        if (success)
        {
          if (!autoCommitOn)
            operationDetails.AppendFormat("{0} rows have been deleted successfully.", updatedCount);
        }
        else
        {
          if (!autoCommitOn)
          {
            operationDetails.AppendFormat("{0} rows were deleted but the following error ocurred.{1}{1}", updatedCount, Environment.NewLine);
            if (exception is MySqlException)
              operationDetails.AppendFormat("MySQL Error {0}:{1}", (exception as MySqlException).Number, Environment.NewLine);
            else
              operationDetails.AppendFormat("ADO.NET Error:{0}", Environment.NewLine);
            operationDetails.Append(exception.Message);
          }
          else
            System.Diagnostics.Debug.WriteLine(exception.Message);
        }
      }

      // Modified cells
      changesTable = editMySQLDataTable.GetChanges(DataRowState.Modified);
      int modifiedRowsCount = (changesTable != null ? changesTable.Rows.Count : 0);
      if (modifiedRowsCount > 0)
      {
        if (!autoCommitOn)
        {
          if (operationDetails.Length > 0)
            operationDetails.AppendFormat("{0}{0}", Environment.NewLine);
          operationDetails.AppendFormat("Committing changes on {0} rows on MySQL Table \"{1}\"...{2}{2}", modifiedRowsCount, editMySQLDataTable.TableName, Environment.NewLine);
          operationDetails.Append(editMySQLDataTable.GetUpdateSQL(-1, true));
          operationDetails.AppendFormat("{0}{0}", Environment.NewLine);
        }
        updatedCount = editMySQLDataTable.UpdateDataWithAdapter(wbConnection, out exception);
        success = exception == null;
        if (success)
        {
          changeExcelCellsColor(modifiedCellAddressesList, commitedCellsOLEColor);
          if (!autoCommitOn)
            operationDetails.AppendFormat("Changes on {0} rows have been committed successfully.", updatedCount);
        }
        else
        {
          if (!autoCommitOn)
          {
            operationDetails.AppendFormat("Changes on {0} rows were committed but the following error ocurred.{1}{1}", updatedCount, Environment.NewLine);
            if (exception is MySqlException)
              operationDetails.AppendFormat("MySQL Error {0}:{1}", (exception as MySqlException).Number, Environment.NewLine);
            else
              operationDetails.AppendFormat("ADO.NET Error:{0}", Environment.NewLine);
            operationDetails.Append(exception.Message);
          }
          else
            System.Diagnostics.Debug.WriteLine(exception.Message);
        }
      }

      if (!autoCommitOn)
      {
        operationSummary += (success ? "was committed to MySQL successfully." : "had errors on commit.");
        InfoDialog infoDialog = new InfoDialog(success, operationSummary, operationDetails.ToString());
        infoDialog.StartPosition = FormStartPosition.CenterScreen;
        DialogResult dr = infoDialog.ShowDialog();
        btnCommit.Enabled = (modifiedCellAddressesList.Count + deletedRowAddressesList.Count + addedRowAddressesList.Count) > 0;
        if (dr == DialogResult.Cancel)
          return;
      }
    }

    private void EditingWorksheet_Change(Excel.Range Target)
    {
      Excel.Range intersectRange = CallerTaskPane.IntersectRanges(editDataRange, Target);
      if (intersectRange == null || intersectRange.Count == 0)
        return;
      
      // We substract from the Excel indexes since they start at 1, Row is subtracted by 2 if we imported headers.
      Excel.Range startCell = (intersectRange.Item[1, 1] as Excel.Range);
      int startDataTableRow = startCell.Row - 1;
      int startDataTableCol = startCell.Column - 1;

      // Detect if a row was deleted and if so flag it for deletion
      if (EditingWorksheet.UsedRange.Rows.Count < editingRowsQuantity)
      {
        editMySQLDataTable.Rows[startDataTableRow].Delete();
        editDataRange = editDataRange.get_Resize(editDataRange.Rows.Count - 1, editDataRange.Columns.Count);
        editingRowsQuantity = editDataRange.Rows.Count;
        if (!chkAutoCommit.Checked && !deletedRowAddressesList.Contains(intersectRange.Address))
          deletedRowAddressesList.Add(intersectRange.Address);
      }
      // Detect if a data row has been added by the user and if so flag it for addition
      else if (false)
      {
        // Code here for Row Insertion (targeted to 1.1)
        editDataRange = editDataRange.get_Resize(editDataRange.Rows.Count + 1, editDataRange.Columns.Count);
        editingRowsQuantity = editDataRange.Rows.Count;
        DataRow newRow = editMySQLDataTable.NewRow();
        editMySQLDataTable.Rows.Add(newRow);
        editingRowsQuantity = EditingWorksheet.UsedRange.Rows.Count;
        if (!chkAutoCommit.Checked && !deletedRowAddressesList.Contains(intersectRange.Address))
          deletedRowAddressesList.Add(intersectRange.Address);
        // Actually paint the changed columns with the changed/committed color, the rest with uncommited and the 
        //  new row range with the newcell color, the code below is dummy wrong code to remember the color.
        intersectRange.Interior.Color = newRowCellsOLEColor;
        if (!chkAutoCommit.Checked && !addedRowAddressesList.Contains(intersectRange.Address))
          addedRowAddressesList.Add(intersectRange.Address);
      }
      // The change was a modification of cell values
      else
      {
        object[,] formattedArrayFromRange;
        if (intersectRange.Count > 1)
          formattedArrayFromRange = intersectRange.Value as object[,];
        else
        {
          formattedArrayFromRange = new object[2, 2];
          formattedArrayFromRange[1, 1] = intersectRange.Value;
        }
        for (int rowIdx = 1; rowIdx <= intersectRange.Rows.Count; rowIdx++)
        {
          for (int colIdx = 1; colIdx <= intersectRange.Columns.Count; colIdx++)
          {
            int absRow = startDataTableRow + rowIdx - 1 - (importedHeaders ? 1 : 0);
            int absCol = startDataTableCol + colIdx - 1;
            MySQLDataColumn currCol = editMySQLDataTable.GetColumnAtIndex(absCol);
            editMySQLDataTable.Rows[absRow][absCol] = DataTypeUtilities.GetInsertingValueForColumnType(formattedArrayFromRange[rowIdx, colIdx], currCol);
          }
        }
        if (!chkAutoCommit.Checked && !modifiedCellAddressesList.Contains(intersectRange.Address))
          modifiedCellAddressesList.Add(intersectRange.Address);
        intersectRange.Interior.Color = (chkAutoCommit.Checked ? commitedCellsOLEColor : uncommitedCellsOLEColor);
      }

      btnCommit.Enabled = intersectRange.Count > 0 && !chkAutoCommit.Checked;
      if (chkAutoCommit.Checked)
        pushDataChanges();
    }

    void EditingWorksheet_SelectionChange(Excel.Range Target)
    {
      Excel.Range intersectRange = CallerTaskPane.IntersectRanges(editDataRange, Target);
      if (intersectRange == null || intersectRange.Count == 0)
        Hide();
      else
        ShowInactiveTopmost();
    }

    private void GenericMouseDown(object sender, MouseEventArgs e)
    {
      if (e.Button == MouseButtons.Left)
        mouseDownPoint = new Point(e.X, e.Y);
    }

    private void GenericMouseUp(object sender, MouseEventArgs e)
    {
      if (e.Button == MouseButtons.Left)
        mouseDownPoint = Point.Empty;
    }

    private void GenericMouseMove(object sender, MouseEventArgs e)
    {
      if (e.Button == MouseButtons.Left)
      {
        if (mouseDownPoint.IsEmpty)
          return;
        Location = new Point(Location.X + (e.X - mouseDownPoint.X), Location.Y + (e.Y - mouseDownPoint.Y));
      }
    }

    protected override void OnMouseDown(MouseEventArgs e)
    {
      base.OnMouseDown(e);
      GenericMouseDown(this, e);
    }

    protected override void OnMouseUp(MouseEventArgs e)
    {
      base.OnMouseUp(e);
      GenericMouseUp(this, e);
    }

    protected override void OnMouseMove(MouseEventArgs e)
    {
      base.OnMouseMove(e);
      GenericMouseMove(this, e);
    }

    private void exitEditModeToolStripMenuItem_Click(object sender, EventArgs e)
    {
      if (connection != null)
        connection.Close();
      Close();
      Dispose();
    }

    private void btnRevert_Click(object sender, EventArgs e)
    {
      EditDataRevertDialog reverDialog = new EditDataRevertDialog(chkAutoCommit.Checked);
      DialogResult dr = reverDialog.ShowDialog();
      if (dr == DialogResult.Cancel)
        return;
      revertDataChanges(reverDialog.SelectedAction == EditDataRevertDialog.EditUndoAction.RefreshData);
    }

    private void btnCommit_Click(object sender, EventArgs e)
    {
      pushDataChanges();
    }

    private void chkAutoCommit_CheckedChanged(object sender, EventArgs e)
    {
      btnCommit.Enabled = !chkAutoCommit.Checked && modifiedCellAddressesList != null && modifiedCellAddressesList.Count > 0;
      btnRevert.Enabled = !chkAutoCommit.Checked;
    }

    [DllImport("user32.dll", EntryPoint = "SetWindowPos")]
    static extern bool SetWindowPos(
         int hWnd,           // window handle
         int hWndInsertAfter,    // placement-order handle
         int X,          // horizontal position
         int Y,          // vertical position
         int cx,         // width
         int cy,         // height
         uint uFlags);       // window positioning flags

    [DllImport("user32.dll")]
    static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);

    [DllImport("user32.dll", SetLastError = true)]
    static extern IntPtr SetParent(IntPtr hWndChild, IntPtr hWndNewParent);

    public void ShowInactiveTopmost()
    {
      ShowWindow(Handle, SW_SHOWNOACTIVATE);
      SetWindowPos(Handle.ToInt32(), HWND_TOPMOST, Left, Top, Width, Height, SWP_NOACTIVATE);
      //SetParent(Handle, ParentWindow.Handle);
    }

  }
}
