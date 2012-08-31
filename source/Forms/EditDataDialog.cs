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
    private const int HWND_NOTOPMOST = -2;
    private const uint SWP_NOACTIVATE = 0x0010;

    private Point mouseDownPoint = Point.Empty;
    private MySqlWorkbenchConnection wbConnection;
    private Excel.Range editDataRange;
    private bool importedHeaders = false;
    private string queryString = String.Empty;
    private MySQLDataTable editMySQLDataTable;
    private MySqlDataAdapter dataAdapter;
    private MySqlConnection connection;
    private List<RangeAndAddress> modifiedRangesAndAddressesList;
    private List<RangeAndAddress> addedRangesAndAddressesList;
    private List<RangeAndAddress> deletedRangesAndAddressesList;
    private int commitedCellsOLEColor = ColorTranslator.ToOle(ColorTranslator.FromHtml("#B8E5F7"));
    private int uncommitedCellsOLEColor = ColorTranslator.ToOle(ColorTranslator.FromHtml("#FF8282"));
    private int newRowCellsOLEColor = ColorTranslator.ToOle(ColorTranslator.FromHtml("#FFFCC7"));
    private int lockedCellsOLEColor = ColorTranslator.ToOle(ColorTranslator.FromHtml("#D7D7D7"));
    private long editingRowsQuantity = 0;
    private long editingColsQuantity = 0;
    private string editingWorksheetName = String.Empty;
    private bool undoingChanges = false;
    private bool uncommitedDataExists
    {
      get { return (modifiedRangesAndAddressesList != null && addedRangesAndAddressesList != null && deletedRangesAndAddressesList != null ? modifiedRangesAndAddressesList.Count + deletedRangesAndAddressesList.Count + addedRangesAndAddressesList.Count > 0 : false); }
    }

    public Excel.Worksheet EditingWorksheet = null;
    public TaskPaneControl CallerTaskPane;
    public string EditingTableName { get; private set; }
    public IWin32Window ParentWindow { get; set; }
    public bool LockByProtectingWorksheet { get; set; }
    public string WorkbookName { get; private set; }
    public string SchemaAndTableName
    {
      get { return String.Format("{0}.{1}", wbConnection.Schema, EditingTableName); }
    }

    public EditDataDialog(MySqlWorkbenchConnection wbConnection, Excel.Range originalEditDataRange, DataTable importTable, Excel.Worksheet editingWorksheet, bool protectWorksheet)
    {
      InitializeComponent();

      this.wbConnection = wbConnection;
      editDataRange = originalEditDataRange;
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
      EditingWorksheet.SelectionChange += new Excel.DocEvents_SelectionChangeEventHandler(EditingWorksheet_SelectionChange);
      toolTip.SetToolTip(this, String.Format("Editing data for Table {0} on Worksheet {1}", EditingTableName, editingWorksheetName));
      editingColsQuantity = editingWorksheet.UsedRange.Columns.Count;
      Opacity = 0.60;
      LockByProtectingWorksheet = protectWorksheet;
      WorkbookName = Globals.ThisAddIn.Application.ActiveWorkbook.Name;
      addNewRowToEditingRange(false);

      addedRangesAndAddressesList = new List<RangeAndAddress>();
      if (editDataRange != null)
      {
        modifiedRangesAndAddressesList = new List<RangeAndAddress>(editDataRange.Count);
        deletedRangesAndAddressesList = new List<RangeAndAddress>(editDataRange.Rows.Count);
      }
      else
      {
        modifiedRangesAndAddressesList = new List<RangeAndAddress>();
        deletedRangesAndAddressesList = new List<RangeAndAddress>();
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
      {
        EditingWorksheet.Unprotect("84308893-7292-49BE-97C0-3A28E81AA2EF");
        EditingWorksheet.UsedRange.Interior.ColorIndex = Excel.XlColorIndex.xlColorIndexNone;
      }
      if (CallerTaskPane.TableNameEditFormsHashtable.ContainsKey(SchemaAndTableName))
        CallerTaskPane.TableNameEditFormsHashtable.Remove(SchemaAndTableName);
      if (CallerTaskPane.WorkSheetEditFormsHashtable.ContainsKey(editingWorksheetName))
        CallerTaskPane.WorkSheetEditFormsHashtable.Remove(editingWorksheetName);
      Dispose();
    }

    private void initializeWorksheetProtection()
    {
      if (editDataRange != null)
      {
        int headersAdditionalRow = (importedHeaders ? 1 : 0);
        Excel.Range extendedRange = editDataRange.get_Range(String.Format("A{0}", 1 + headersAdditionalRow));
        extendedRange = extendedRange.get_Resize(editDataRange.Rows.Count - headersAdditionalRow, EditingWorksheet.Columns.Count);
        extendedRange.Locked = false;
        if (importedHeaders)
        {
          Excel.Range headersRange = EditingWorksheet.get_Range("A1");
          headersRange = headersRange.get_Resize(1, editDataRange.Columns.Count);
          lockRange(headersRange, true);
        }
      }
      EditingWorksheet.Protect("84308893-7292-49BE-97C0-3A28E81AA2EF",
                               false,
                               true,
                               true,
                               true,
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
      if (lockRange)
        range.Interior.Color = lockedCellsOLEColor;
      else
        range.Interior.ColorIndex = Excel.XlColorIndex.xlColorIndexNone;
      range.Locked = lockRange;
    }

    private void changeExcelCellsColor(Excel.Range modifiedRange, int oleColor)
    {
      if (modifiedRange == null)
        return;
      if (oleColor > 0)
        modifiedRange.Interior.Color = oleColor;
      else
        modifiedRange.Interior.ColorIndex = Excel.XlColorIndex.xlColorIndexNone;
    }

    private void changeExcelCellsColor(List<RangeAndAddress> rangesAndAddressesList, int oleColor)
    {
      foreach (RangeAndAddress ra in rangesAndAddressesList)
        changeExcelCellsColor(ra.Range, oleColor);
      rangesAndAddressesList.Clear();
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
        changeExcelCellsColor(modifiedRange, oleColor);
      }
      cellAddressesList.Clear();
    }

    private Excel.Range addNewRowToEditingRange(bool clearColoringOfOldNewRow)
    {
      Excel.Range rowRange = null;

      if (editDataRange != null)
      {
        UnprotectEditingWorksheet();
        editDataRange = editDataRange.get_Resize(editDataRange.Rows.Count + 1, editDataRange.Columns.Count);
        rowRange = editDataRange.Rows[editDataRange.Rows.Count] as Excel.Range;
        rowRange.Interior.Color = newRowCellsOLEColor;
        editingRowsQuantity = editDataRange.Rows.Count;
        ProtectEditingWorksheet();
        if (clearColoringOfOldNewRow)
        {
          rowRange = editDataRange.Rows[editDataRange.Rows.Count - 1] as Excel.Range;
          rowRange.Interior.ColorIndex = Excel.XlColorIndex.xlColorIndexNone;
        }
      }

      return rowRange;
    }

    private void UnprotectEditingWorksheet()
    {
      EditingWorksheet.Change -= new Excel.DocEvents_ChangeEventHandler(EditingWorksheet_Change);
      EditingWorksheet.Unprotect("84308893-7292-49BE-97C0-3A28E81AA2EF");
    }

    private void ProtectEditingWorksheet()
    {
      EditingWorksheet.Change += new Excel.DocEvents_ChangeEventHandler(EditingWorksheet_Change);
      initializeWorksheetProtection();
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

      UnprotectEditingWorksheet();
      editDataRange.Clear();
      Excel.Range topLeftCell = editDataRange.Cells[1, 1];
      topLeftCell.Select();
      editDataRange = CallerTaskPane.ImportDataTableToExcelAtGivenCell(editMySQLDataTable, importedHeaders, topLeftCell);
      if (refreshFromDB)
      {
        changeExcelCellsColor(editDataRange, 0);
        modifiedRangesAndAddressesList.Clear();
        addedRangesAndAddressesList.Clear();
        deletedRangesAndAddressesList.Clear();
      }
      else
      {
        changeExcelCellsColor(modifiedRangesAndAddressesList, 0);
        changeExcelCellsColor(addedRangesAndAddressesList, 0);
        changeExcelCellsColor(deletedRangesAndAddressesList, 0);
      }
      btnCommit.Enabled = false;
      addNewRowToEditingRange(false);
    }

    private bool pushDataChanges()
    {
      bool success = true;
      bool warningsFound = false;
      bool errorsFound = false;
      int updatedCount = 0;
      Exception exception;
      DataTable warningsTable;
      bool autoCommitOn = chkAutoCommit.Checked;

      string operationSummary = String.Format("Edited data for Table {0} ", EditingTableName);
      string sqlQuery = String.Empty;
      StringBuilder operationDetails = new StringBuilder();
      this.Cursor = Cursors.WaitCursor;

      // Added Rows
      DataTable changesTable = editMySQLDataTable.GetChanges(DataRowState.Added);
      int addingRowsCount = (changesTable != null ? changesTable.Rows.Count : 0);
      if (addingRowsCount > 0)
      {
        operationDetails.AppendFormat("Adding {0} rows to MySQL Table \"{1}\"...{2}{2}",
                                      addingRowsCount,
                                      editMySQLDataTable.TableName,
                                      Environment.NewLine);
        warningsTable = editMySQLDataTable.InsertDataWithManualQuery(wbConnection, true, out exception, out sqlQuery, out updatedCount);
        success = exception == null;
        operationDetails.AppendFormat("{0}{1}{1}",
                                      sqlQuery,
                                      Environment.NewLine);
        if (success)
        {
          changeExcelCellsColor(addedRangesAndAddressesList, commitedCellsOLEColor);
          operationDetails.AppendFormat("{0} rows have been added", updatedCount);
          if (warningsTable != null && warningsTable.Rows.Count > 0)
          {
            warningsFound = true;
            operationDetails.AppendFormat(" with {0} warnings:", warningsTable.Rows.Count);
            foreach (DataRow warningRow in warningsTable.Rows)
            {
              operationDetails.AppendFormat("{2}Code {0} - {1}",
                                            warningRow[1].ToString(),
                                            warningRow[2].ToString(),
                                            Environment.NewLine);
            }
            operationDetails.Append(Environment.NewLine);
          }
          else
            operationDetails.Append(" successfully.");
        }
        else
        {
          errorsFound = true;
          operationDetails.AppendFormat("{0} rows were added but the following error ocurred.{1}{1}", updatedCount, Environment.NewLine);
          if (exception is MySqlException)
            operationDetails.AppendFormat("MySQL Error {0}:{1}", (exception as MySqlException).Number, Environment.NewLine);
          else
            operationDetails.AppendFormat("ADO.NET Error:{0}", Environment.NewLine);
          operationDetails.Append(exception.Message);
        }
      }
      
      // Deleted Rows
      changesTable = editMySQLDataTable.GetChanges(DataRowState.Deleted);
      int deletingRowsCount = (changesTable != null ? changesTable.Rows.Count : 0);
      if (deletingRowsCount > 0)
      {
        operationDetails.AppendFormat("{3}{3}Deleting {0} rows on MySQL Table \"{1}\"...{2}{2}",
                                      deletingRowsCount,
                                      editMySQLDataTable.TableName,
                                      Environment.NewLine,
                                      (operationDetails.Length > 0 ? Environment.NewLine : String.Empty));
        warningsTable = editMySQLDataTable.DeleteDataWithManualQuery(wbConnection, out exception, out sqlQuery, out updatedCount);
        success = exception == null;
        operationDetails.AppendFormat("{0}{1}{1}",
                                      sqlQuery,
                                      Environment.NewLine);
        if (success)
        {
          deletedRangesAndAddressesList.Clear();
          operationDetails.AppendFormat("{0} rows have been deleted", updatedCount);
          if (warningsTable != null && warningsTable.Rows.Count > 0)
          {
            warningsFound = true;
            operationDetails.AppendFormat(" with {0} warnings:", warningsTable.Rows.Count);
            foreach (DataRow warningRow in warningsTable.Rows)
            {
              operationDetails.AppendFormat("{2}Code {0} - {1}",
                                            warningRow[1].ToString(),
                                            warningRow[2].ToString(),
                                            Environment.NewLine);
            }
            operationDetails.Append(Environment.NewLine);
          }
          else
            operationDetails.Append(" successfully.");
        }
        else
        {
          errorsFound = true;
          operationDetails.AppendFormat("{0} rows were deleted but the following error ocurred.{1}{1}", updatedCount, Environment.NewLine);
          if (exception is MySqlException)
            operationDetails.AppendFormat("MySQL Error {0}:{1}", (exception as MySqlException).Number, Environment.NewLine);
          else
            operationDetails.AppendFormat("ADO.NET Error:{0}", Environment.NewLine);
          operationDetails.Append(exception.Message);
        }
      }

      // Modified cells
      changesTable = editMySQLDataTable.GetChanges(DataRowState.Modified);
      int modifiedRowsCount = (changesTable != null ? changesTable.Rows.Count : 0);
      if (modifiedRowsCount > 0)
      {
        operationDetails.AppendFormat("{3}{3}Committing changes on {0} rows on MySQL Table \"{1}\"...{2}{2}",
                                      modifiedRowsCount,
                                      editMySQLDataTable.TableName,
                                      Environment.NewLine,
                                      (operationDetails.Length > 0 ? Environment.NewLine : String.Empty));
        warningsTable = editMySQLDataTable.UpdateDataWithManualQuery(wbConnection, out exception, out sqlQuery, out updatedCount);
        success = exception == null;
        operationDetails.AppendFormat("{0}{1}{1}",
                                      sqlQuery,
                                      Environment.NewLine);
        if (success)
        {
          changeExcelCellsColor(modifiedRangesAndAddressesList, commitedCellsOLEColor);
          operationDetails.AppendFormat("Changes on {0} rows have been committed", updatedCount);
          if (warningsTable != null && warningsTable.Rows.Count > 0)
          {
            warningsFound = true;
            operationDetails.AppendFormat(" with {0} warnings:", warningsTable.Rows.Count);
            foreach (DataRow warningRow in warningsTable.Rows)
            {
              operationDetails.AppendFormat("{2}Code {0} - {1}",
                                            warningRow[1].ToString(),
                                            warningRow[2].ToString(),
                                            Environment.NewLine);
            }
            operationDetails.Append(Environment.NewLine);
          }
          else
            operationDetails.Append(" successfully.");
        }
        else
        {
          errorsFound = true;
          operationDetails.AppendFormat("Changes on {0} rows were committed but the following error ocurred.{1}{1}", updatedCount, Environment.NewLine);
          if (exception is MySqlException)
            operationDetails.AppendFormat("MySQL Error {0}:{1}", (exception as MySqlException).Number, Environment.NewLine);
          else
            operationDetails.AppendFormat("ADO.NET Error:{0}", Environment.NewLine);
          operationDetails.Append(exception.Message);
        }
      }

      InfoDialog.InfoType operationsType;
      if (!errorsFound)
      {
        if (warningsFound)
        {
          operationSummary += "was committed to MySQL with warnings.";
          operationsType = InfoDialog.InfoType.Warning;
        }
        else
        {
          operationSummary += "was committed to MySQL successfully.";
          operationsType = InfoDialog.InfoType.Success;
        }
      }
      else
      {
        operationSummary += "had errors on commit.";
        operationsType = InfoDialog.InfoType.Error;
      }

      if (!autoCommitOn || warningsFound || errorsFound)
      {
        InfoDialog infoDialog = new InfoDialog(operationsType, operationSummary, operationDetails.ToString());
        infoDialog.StartPosition = FormStartPosition.CenterScreen;
        DialogResult dr = infoDialog.ShowDialog();
      }

      //btnCommit.Enabled = uncommitedDataExists && !autoCommitOn;
      btnCommit.Enabled = uncommitedDataExists && !autoCommitOn;
      this.Cursor = Cursors.Default;

      return !errorsFound;
    }

    private void UndoChanges()
    {
      undoingChanges = true;
      try
      {
        EditingWorksheet.Application.Undo();
      }
      catch { }
      undoingChanges = false;
    }

    private int SearchRowIndexNotDeleted(int excelRowIdx, List<int> skipIndexesList)
    {
      int notDeletedIdx = -1;

      if (editMySQLDataTable != null)
      {
        if (editMySQLDataTable.Rows.Count == editDataRange.Rows.Count - (importedHeaders ? 1 : 0) - 1)
          return excelRowIdx;
        for (int tableRowIdx = 0; tableRowIdx < editMySQLDataTable.Rows.Count; tableRowIdx++)
        {
          if (editMySQLDataTable.Rows[tableRowIdx].RowState != DataRowState.Deleted || (skipIndexesList != null && skipIndexesList.Contains(tableRowIdx)))
            notDeletedIdx++;
          if (notDeletedIdx == excelRowIdx)
            return tableRowIdx;
        }
      }

      return -1;
    }

    private int RefreshAddressesOfStoredRanges(List<RangeAndAddress> rangeAndAddressesList)
    {
      int qtyUpdated = 0;

      if (rangeAndAddressesList != null && rangeAndAddressesList.Count > 0)
      {
        foreach (RangeAndAddress ra in rangeAndAddressesList)
        {
          try
          {
            if (ra.Address != ra.Range.Address)
            {
              ra.Address = ra.Range.Address;
              qtyUpdated++;
            }
          }
          catch
          {
            ra.Range = EditingWorksheet.get_Range(ra.Address);
            qtyUpdated++;
          }
        }
      }

      return qtyUpdated;
    }

    private void EditingWorksheet_Change(Excel.Range Target)
    {
      if (undoingChanges)
        return;

      InfoDialog errorDialog = null;
      bool rowWasDeleted = EditingWorksheet.UsedRange.Rows.Count < editingRowsQuantity && Target.Columns.Count == EditingWorksheet.Columns.Count;
      bool undoChanges = false;
      string operationSummary = null;
      string operationDetails = null;

      Excel.Range intersectRange = CallerTaskPane.IntersectRanges(editDataRange, Target);
      if (intersectRange == null || intersectRange.Count == 0)
      {
        if (rowWasDeleted)
        {
          // The row for insertions is attempted to be deleted, we need to undo
          undoChanges = true;
          operationSummary = Properties.Resources.EditDataDeleteLastRowNotPermittedErrorTitle;
          operationDetails = Properties.Resources.EditDataDeleteLastRowNotPermittedErrorDetail;
        }
        else
        {
          // It is a modification and outside the permitted range
          undoChanges = true;
          operationSummary = Properties.Resources.EditDataOutsideEditingRangeNotPermittedErrorTitle;
          operationDetails = Properties.Resources.EditDataOutsideEditingRangeNotPermittedErrorDetail;
        }
      }
      if (undoChanges)
      {
        errorDialog = new InfoDialog(false, operationSummary, operationDetails);
        errorDialog.WordWrapDetails = true;
        errorDialog.StartPosition = FormStartPosition.CenterScreen;
        errorDialog.ShowDialog();
        UndoChanges();
        if (rowWasDeleted)
        {
          int changedAddedRangesQty = RefreshAddressesOfStoredRanges(addedRangesAndAddressesList);
          int changedModifiedRangesQty = RefreshAddressesOfStoredRanges(modifiedRangesAndAddressesList);
          editDataRange = EditingWorksheet.UsedRange;
        }
        return;
      }
      
      // We substract from the Excel indexes since they start at 1, Row is subtracted by 2 if we imported headers.
      Excel.Range startCell = (intersectRange.Item[1, 1] as Excel.Range);
      int startDataTableRow = startCell.Row - 1 - (importedHeaders ? 1 : 0);
      int startDataTableCol = startCell.Column - 1;

      // Detect if a row was deleted and if so flag it for deletion
      if (rowWasDeleted)
      {
        List<int> skipDeletedRowsList = new List<int>();
        foreach (Excel.Range deletedRow in Target.Rows)
        {
          startDataTableRow = deletedRow.Row - 1 - (importedHeaders ? 1 : 0);
          startDataTableRow = SearchRowIndexNotDeleted(startDataTableRow, skipDeletedRowsList);
          editMySQLDataTable.Rows[startDataTableRow].Delete();
          skipDeletedRowsList.Add(startDataTableRow);
          if (!deletedRangesAndAddressesList.Exists(ra => ra.Address == deletedRow.Address))
            deletedRangesAndAddressesList.Add(new RangeAndAddress(deletedRow, deletedRow.Address));
        }
        for (int rangeIdx = 0; rangeIdx < modifiedRangesAndAddressesList.Count; rangeIdx++)
        {
          bool removeFromList = false;
          RangeAndAddress modifiedCellRangeAndAddress = modifiedRangesAndAddressesList[rangeIdx];
          try
          {
            modifiedCellRangeAndAddress.Address = modifiedCellRangeAndAddress.Range.Address;
          }
          catch 
          {
            removeFromList = true;
          }
          if (removeFromList)
          {
            modifiedRangesAndAddressesList.Remove(modifiedCellRangeAndAddress);
            rangeIdx--;
          }
        }
        for (int rangeIdx = 0; rangeIdx < addedRangesAndAddressesList.Count; rangeIdx++)
        {
          bool removeFromList = false;
          RangeAndAddress addedRowRangeAndAddress = addedRangesAndAddressesList[rangeIdx];
          try
          {
            addedRowRangeAndAddress.Address = addedRowRangeAndAddress.Range.Address;
          }
          catch
          {
            removeFromList = true;
          }
          if (removeFromList)
          {
            addedRangesAndAddressesList.Remove(addedRowRangeAndAddress);
            rangeIdx--;
          }
        }
        editingRowsQuantity = editDataRange.Rows.Count;
      }
      else
      {
        // The change was a modification of cell values
        MySQLDataColumn currCol = null;
        try
        {
          for (int rowIdx = 1; rowIdx <= intersectRange.Rows.Count; rowIdx++)
            for (int colIdx = 1; colIdx <= intersectRange.Columns.Count; colIdx++)
            {
              Excel.Range cell = intersectRange.Cells[rowIdx, colIdx] as Excel.Range;

              // Detect if a data row has been added by the user and if so flag it for addition
              if (cell.Row == editDataRange.Rows.Count)
              {
                if (cell.Value == null)
                  continue;
                Excel.Range insertingRowRange = addNewRowToEditingRange(true);
                DataRow newRow = editMySQLDataTable.NewRow();
                editMySQLDataTable.Rows.Add(newRow);
                if (!addedRangesAndAddressesList.Exists(ra => ra.Address == insertingRowRange.Address))
                  addedRangesAndAddressesList.Add(new RangeAndAddress(insertingRowRange, insertingRowRange.Address));
                insertingRowRange.Interior.Color = uncommitedCellsOLEColor;
              }

              int absRow = startDataTableRow + rowIdx - 1;
              absRow = SearchRowIndexNotDeleted(absRow, null);
              int absCol = startDataTableCol + colIdx - 1;

              currCol = editMySQLDataTable.GetColumnAtIndex(absCol);
              object insertingValue = DataTypeUtilities.GetInsertingValueForColumnType(cell.Value, currCol);
              if (editMySQLDataTable.Rows[absRow].RowState != DataRowState.Added)
              {
                if (DataTypeUtilities.ExcelValueEqualsDataTableValue(editMySQLDataTable.Rows[absRow][absCol, DataRowVersion.Original], insertingValue))
                {
                  if (modifiedRangesAndAddressesList.Exists(ra => ra.Address == cell.Address))
                  {
                    changeExcelCellsColor(cell, 0);
                    modifiedRangesAndAddressesList.RemoveAll(ra => ra.Address == cell.Address);
                    editMySQLDataTable.Rows[absRow][absCol] = insertingValue;
                    int changedColsQty = editMySQLDataTable.GetChangedColumns(editMySQLDataTable.Rows[absRow]).Count;
                    if (changedColsQty == 0)
                      editMySQLDataTable.Rows[absRow].RejectChanges();
                  }
                  continue;
                }
                // Need to set the value before coloring the cell in case there is an invalid value it does not reach the coloring code
                editMySQLDataTable.Rows[absRow][absCol] = insertingValue;
                if (!modifiedRangesAndAddressesList.Exists(ra => ra.Address == cell.Address))
                  modifiedRangesAndAddressesList.Add(new RangeAndAddress(cell, cell.Address));
              }
              else
                editMySQLDataTable.Rows[absRow][absCol] = insertingValue;
              cell.Interior.Color = uncommitedCellsOLEColor;
            }
        }
        catch (ArgumentException argEx)
        {
          undoChanges = true;
          operationSummary = String.Format("Invalid value for column of type: {0}", (currCol != null ? currCol.MySQLDataType : "Unknown"));
          operationDetails = argEx.Message;
        }
        catch (Exception ex)
        {
          undoChanges = true;
          operationSummary = "Error modifying cell's value.";
          operationDetails = ex.Message;
        }
        finally
        {
          if (undoChanges)
          {
            errorDialog = new InfoDialog(false, operationSummary, operationDetails);
            errorDialog.WordWrapDetails = true;
            errorDialog.StartPosition = FormStartPosition.CenterScreen;
            errorDialog.ShowDialog();
            UndoChanges();
          }
        }
      }

      btnCommit.Enabled = !chkAutoCommit.Checked && uncommitedDataExists;
      if (chkAutoCommit.Checked && uncommitedDataExists)
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
      Close();
    }

    private void btnRevert_Click(object sender, EventArgs e)
    {
      EditDataRevertDialog revertDialog = new EditDataRevertDialog(chkAutoCommit.Checked);
      DialogResult dr = revertDialog.ShowDialog();
      if (dr == DialogResult.Cancel)
        return;
      revertDataChanges(revertDialog.SelectedAction == EditDataRevertDialog.EditUndoAction.RefreshData);
    }

    private void btnCommit_Click(object sender, EventArgs e)
    {
      pushDataChanges();
    }

    private void chkAutoCommit_CheckedChanged(object sender, EventArgs e)
    {
      btnCommit.Enabled = !chkAutoCommit.Checked && uncommitedDataExists;
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

    public void ShowInactiveTopmost()
    {
      ShowWindow(Handle, SW_SHOWNOACTIVATE);
      SetWindowPos(Handle.ToInt32(), HWND_NOTOPMOST, Left, Top, Width, Height, SWP_NOACTIVATE);
    }

  }

  public class RangeAndAddress
  {
    public Excel.Range Range;
    public string Address;

    public RangeAndAddress(Excel.Range range, string address)
    {
      Range = range;
      Address = address;
    }
  }
}
