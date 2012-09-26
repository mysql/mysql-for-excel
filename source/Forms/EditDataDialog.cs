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
    private const string SHEET_PROTECTION_KEY = "84308893-7292-49BE-97C0-3A28E81AA2EF";

    private Point mouseDownPoint = Point.Empty;
    private MySqlWorkbenchConnection wbConnection;
    private Excel.Range editDataRange;
    private string queryString = String.Empty;
    private MySQLDataTable editMySQLDataTable;
    private List<RangeAndAddress> rangesAndAddressesList;
    private int whiteOLEColor = ColorTranslator.ToOle(Color.White);
    private int commitedCellsOLEColor = ColorTranslator.ToOle(ColorTranslator.FromHtml("#B8E5F7"));
    private int uncommitedCellsOLEColor = ColorTranslator.ToOle(ColorTranslator.FromHtml("#7CC576"));
    private int erroredCellsOLEColor = ColorTranslator.ToOle(ColorTranslator.FromHtml("#FF8282"));
    private int newRowCellsOLEColor = ColorTranslator.ToOle(ColorTranslator.FromHtml("#FFFCC7"));
    private int lockedCellsOLEColor = ColorTranslator.ToOle(ColorTranslator.FromHtml("#D7D7D7"));
    private long editingRowsQuantity = 0;
    private long editingColsQuantity = 0;
    private bool undoingChanges = false;
    private bool uncommitedDataExists
    {
      get { return (rangesAndAddressesList != null ? rangesAndAddressesList.Count > 0 : false); }
    }

    public Excel.Worksheet EditingWorksheet = null;
    public TaskPaneControl CallerTaskPane;
    public bool LockByProtectingWorksheet { get; set; }
    public IWin32Window ParentWindow { get; set; }
    public string EditingSchema
    {
      get { return (wbConnection != null ? wbConnection.Schema : null); }
    }
    public string EditingTableName
    {
      get { return (editMySQLDataTable != null ? editMySQLDataTable.TableName : null); }
    }
    public string WorksheetName
    {
      get 
      {
        try
        {
          return EditingWorksheet.Name;
        }
        catch
        {
          return null;
        }
      }
    }
    public string WorkbookName
    {
      get 
      {
        try
        {
          return (EditingWorksheet.Parent as Excel.Workbook).Name;
        }
        catch
        {
          return null;
        }
      }
    }
    public bool EditingWorksheetExists
    {
      get
      {
        bool exists = false;
        if (EditingWorksheet != null)
        {
          try
          {
            Excel.Workbook wb = EditingWorksheet.Parent as Excel.Workbook;
            exists = true;
          }
          catch
          {
            exists = false;
          }
        }
        return exists;
      }
    }

    public EditDataDialog(MySqlWorkbenchConnection wbConnection, Excel.Range originalEditDataRange, DataTable importTable, Excel.Worksheet editingWorksheet, bool protectWorksheet)
    {
      InitializeComponent();

      this.wbConnection = wbConnection;
      editDataRange = originalEditDataRange;
      queryString = importTable.ExtendedProperties["QueryString"].ToString();
      string tableName = importTable.TableName;
      if (importTable.ExtendedProperties.ContainsKey("TableName") && !String.IsNullOrEmpty(importTable.ExtendedProperties["TableName"].ToString()))
        tableName = importTable.ExtendedProperties["TableName"].ToString();
      editMySQLDataTable = new MySQLDataTable(tableName, importTable, wbConnection);
      if (importTable.ExtendedProperties.ContainsKey("QueryString") && !String.IsNullOrEmpty(importTable.ExtendedProperties["QueryString"].ToString()))
        editMySQLDataTable.SelectQuery = importTable.ExtendedProperties["QueryString"].ToString();
      EditingWorksheet = editingWorksheet;
      EditingWorksheet.SelectionChange += new Excel.DocEvents_SelectionChangeEventHandler(EditingWorksheet_SelectionChange);
      ResetToolTip();
      editingColsQuantity = editingWorksheet.UsedRange.Columns.Count;
      Opacity = 0.60;
      LockByProtectingWorksheet = protectWorksheet;
      addNewRowToEditingRange(false);

      rangesAndAddressesList = new List<RangeAndAddress>();
    }

    private void EditDataDialog_Activated(object sender, EventArgs e)
    {
      ResetToolTip();
    }

    private void ResetToolTip()
    {
      toolTip.SetToolTip(this, String.Format(Properties.Resources.EditDataFormTooltipText,
                                             Environment.NewLine,
                                             wbConnection.Schema,
                                             EditingTableName,
                                             WorkbookName,
                                             WorksheetName));
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
      CallerTaskPane.RefreshDBObjectPanelActionLabelsEnabledStatus(EditingTableName, false);
      if (EditingWorksheetExists)
      {
        EditingWorksheet.Unprotect(SHEET_PROTECTION_KEY);
        EditingWorksheet.UsedRange.Interior.ColorIndex = Excel.XlColorIndex.xlColorIndexNone;
      }
      ActiveEditDialogContainer editContainer = CallerTaskPane.ActiveEditDialogsList.Find(ac => ac.EditDialog.Equals(this));
      if (editContainer != null)
        CallerTaskPane.ActiveEditDialogsList.Remove(editContainer);
      Dispose();
    }

    private void initializeWorksheetProtection()
    {
      if (editDataRange != null)
      {
        Excel.Range extendedRange = editDataRange.get_Range(String.Format("A{0}", 2));
        extendedRange = extendedRange.get_Resize(editDataRange.Rows.Count - 1, EditingWorksheet.Columns.Count);
        extendedRange.Locked = false;

        // Column names range code
        Excel.Range headersRange = EditingWorksheet.get_Range("A1");
        headersRange = headersRange.get_Resize(1, editDataRange.Columns.Count);
        lockRange(headersRange, true);
      }
      EditingWorksheet.Protect(SHEET_PROTECTION_KEY,
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
      if (rangesAndAddressesList == null)
        return;
      foreach (RangeAndAddress ra in rangesAndAddressesList)
        changeExcelCellsColor(ra.Range, oleColor);
      rangesAndAddressesList.Clear();
    }

    private void changeExcelCellsToCommmitedColor(bool commitSuccessful)
    {
      if (rangesAndAddressesList == null)
        return;
      for (int idx = 0; idx < rangesAndAddressesList.Count; idx++)
      {
        RangeAndAddress ra = rangesAndAddressesList[idx];
        if (ra.TableRow.HasErrors)
        {
          changeExcelCellsColor(ra.Range, erroredCellsOLEColor);
          continue;
        }
        if (!commitSuccessful)
          continue;
        if (ra.TableRow.RowState != DataRowState.Detached && ra.TableRow.RowState != DataRowState.Deleted)
          changeExcelCellsColor(ra.Range, commitedCellsOLEColor);
        rangesAndAddressesList.Remove(ra);
        idx--;
      }
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
        if (clearColoringOfOldNewRow && editDataRange.Rows.Count > 0)
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
      EditingWorksheet.Unprotect(SHEET_PROTECTION_KEY);
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
        InfoDialog errorDialog = new InfoDialog(false, String.Format("{0} Data Error", (refreshFromDB ? "Refresh" : "Revert")), exception.Message);
        errorDialog.ShowDialog();
      }

      UnprotectEditingWorksheet();
      editDataRange.Clear();
      Excel.Range topLeftCell = editDataRange.Cells[1, 1];
      topLeftCell.Select();
      editDataRange = CallerTaskPane.ImportDataTableToExcelAtGivenCell(editMySQLDataTable, true, topLeftCell);
      if (refreshFromDB)
      {
        changeExcelCellsColor(editDataRange, 0);
        rangesAndAddressesList.Clear();
      }
      else
        changeExcelCellsColor(rangesAndAddressesList, 0);
      btnCommit.Enabled = false;
      addNewRowToEditingRange(false);
    }

    private bool pushDataChanges()
    {
      bool success = true;
      bool warningsFound = false;
      bool errorsFound = false;
      bool autoCommitOn = chkAutoCommit.Checked;

      int warningsCount = 0;
      StringBuilder operationSummary = new StringBuilder();
      operationSummary.AppendFormat(Properties.Resources.EditedDataForTable, EditingTableName);
      string sqlQuery = String.Empty;
      StringBuilder operationDetails = new StringBuilder();
      StringBuilder warningDetails = new StringBuilder();
      this.Cursor = Cursors.WaitCursor;

      operationDetails.AppendFormat(Properties.Resources.EditDataCommittingText,
                                    editMySQLDataTable.DeletingOperations,
                                    editMySQLDataTable.InsertingOperations,
                                    editMySQLDataTable.UpdatingOperations);
      PushResultsDataTable resultsDT = editMySQLDataTable.PushData(wbConnection);
      operationDetails.Append(Environment.NewLine);
      foreach (DataRow operationRow in resultsDT.Rows)
      {
        sqlQuery = operationRow["QueryText"].ToString();
        if (sqlQuery.Length > 0)
        {
          operationDetails.AppendFormat("{0}{1:000}: {2}",
                                        Environment.NewLine,
                                        (int)operationRow["OperationIndex"],
                                        sqlQuery);
        }
        string operationResult = operationRow["OperationResult"].ToString();
        switch (operationResult)
        {
          case "Warning":
            warningsFound = true;
            warningDetails.AppendFormat("{0}{1}",
                                        Environment.NewLine,
                                        operationRow["ResultText"].ToString());
            warningsCount++;
            break;
          case "Error":
            errorsFound = true;
            operationDetails.AppendFormat("{0}{0}{1}",
                                        Environment.NewLine,
                                        operationRow["ResultText"].ToString());
            break;
        }
        if (errorsFound)
        {
          success = false;
          break;
        }
      }

      if (warningsFound)
      {
        operationDetails.Append(Environment.NewLine);
        operationDetails.Append(Environment.NewLine);
        operationDetails.AppendFormat(Properties.Resources.EditDataCommittedWarningsFound,
                                      warningsCount);
        operationDetails.Append(Environment.NewLine);
        operationDetails.Append(warningDetails.ToString());
      }
      operationDetails.Append(Environment.NewLine);
      operationDetails.Append(Environment.NewLine);
      operationDetails.AppendFormat(Properties.Resources.EditDataCommittedText,
                                    resultsDT.DeletedOperations,
                                    resultsDT.InsertedOperations,
                                    resultsDT.UpdatedOperations);

      changeExcelCellsToCommmitedColor(success);

      foreach (DataRow dr in editMySQLDataTable.Rows)
      {
        dr.ClearErrors();
      }

      InfoDialog.InfoType operationsType;
      if (!errorsFound)
      {
        if (warningsFound)
        {
          operationSummary.Append(Properties.Resources.EditedDataCommittedWarning);
          operationsType = InfoDialog.InfoType.Warning;
        }
        else
        {
          operationSummary.Append(Properties.Resources.EditedDataCommittedSucess);
          operationsType = InfoDialog.InfoType.Success;
        }
      }
      else
      {
        operationSummary.AppendFormat(Properties.Resources.EditedDataCommittedError);
        operationsType = InfoDialog.InfoType.Error;
      }

      if (!autoCommitOn || warningsFound || errorsFound)
      {
        InfoDialog infoDialog = new InfoDialog(operationsType, operationSummary.ToString(), operationDetails.ToString());
        infoDialog.StartPosition = FormStartPosition.CenterScreen;
        DialogResult dr = infoDialog.ShowDialog();
      }

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
      catch(Exception ex)
      {
        MiscUtilities.WriteAppErrorToLog(ex);
      }
      undoingChanges = false;
    }

    private int SearchRowIndexNotDeleted(int excelRowIdx, List<int> skipIndexesList)
    {
      int notDeletedIdx = -1;

      if (editMySQLDataTable != null)
      {
        if (editMySQLDataTable.Rows.Count == editDataRange.Rows.Count - 2)
          return excelRowIdx;
        for (int tableRowIdx = 0; tableRowIdx < editMySQLDataTable.Rows.Count; tableRowIdx++)
        {
          if (editMySQLDataTable.Rows[tableRowIdx].RowState != DataRowState.Deleted)
            notDeletedIdx++;
          if (skipIndexesList != null)
            notDeletedIdx += skipIndexesList.Count(n => n == tableRowIdx);
          if (notDeletedIdx == excelRowIdx)
            return tableRowIdx;
        }
      }

      return -1;
    }

    private int RefreshAddressesOfStoredRanges()
    {
      int qtyUpdated = 0;

      if (rangesAndAddressesList != null && rangesAndAddressesList.Count > 0)
      {
        foreach (RangeAndAddress ra in rangesAndAddressesList)
        {
          if (ra.Modification != RangeAndAddress.RangeModification.Added && ra.Modification != RangeAndAddress.RangeModification.Updated)
            continue;
          try
          {
            if (ra.Address != ra.Range.Address)
            {
              ra.Address = ra.Range.Address;
              ra.ExcelRow = ra.Range.Row;
              qtyUpdated++;
            }
          }
          catch
          {
            ra.Range = EditingWorksheet.get_Range(ra.Address);
            ra.ExcelRow = ra.Range.Row;
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
          int changedRangesQty = RefreshAddressesOfStoredRanges();
          editDataRange = EditingWorksheet.UsedRange;
        }
        return;
      }
      
      // We substract from the Excel indexes since they start at 1, ExcelRow is subtracted by 2 if we imported headers.
      Excel.Range startCell = (intersectRange.Item[1, 1] as Excel.Range);
      int startDataTableRow = startCell.Row - 2;
      int startDataTableCol = startCell.Column - 1;

      // Detect if a row was deleted and if so flag it for deletion
      if (rowWasDeleted)
      {
        List<int> skipDeletedRowsList = new List<int>();
        foreach (Excel.Range deletedRow in Target.Rows)
        {
          startDataTableRow = deletedRow.Row - 2;
          startDataTableRow = SearchRowIndexNotDeleted(startDataTableRow, skipDeletedRowsList);
          DataRow dr = editMySQLDataTable.Rows[startDataTableRow];
          dr.Delete();
          skipDeletedRowsList.Add(startDataTableRow);
          RangeAndAddress addedRA = rangesAndAddressesList.Find(ra => ra.Modification == RangeAndAddress.RangeModification.Added && ra.ExcelRow == deletedRow.Row);
          if (addedRA != null)
            rangesAndAddressesList.Remove(addedRA);
          else if (!rangesAndAddressesList.Exists(ra => ra.Modification == RangeAndAddress.RangeModification.Deleted && ra.Address == deletedRow.Address))
            rangesAndAddressesList.Add(new RangeAndAddress(RangeAndAddress.RangeModification.Deleted, deletedRow, deletedRow.Address, (int)deletedRow.Interior.Color, deletedRow.Row, dr));
        }
        for (int rangeIdx = 0; rangeIdx < rangesAndAddressesList.Count; rangeIdx++)
        {
          bool removeFromList = false;
          RangeAndAddress ra = rangesAndAddressesList[rangeIdx];
          if (ra.Modification == RangeAndAddress.RangeModification.Deleted)
            continue;
          try
          {
            ra.Address = ra.Range.Address;
          }
          catch 
          {
            removeFromList = true;
          }
          if (removeFromList)
          {
            rangesAndAddressesList.Remove(ra);
            rangeIdx--;
          }
        }
        int changedRangesQty = RefreshAddressesOfStoredRanges();
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
                if (!rangesAndAddressesList.Exists(ra => ra.Modification == RangeAndAddress.RangeModification.Added && ra.Address == insertingRowRange.Address))
                  rangesAndAddressesList.Add(new RangeAndAddress(RangeAndAddress.RangeModification.Added, insertingRowRange, insertingRowRange.Address, (int)insertingRowRange.Interior.Color, insertingRowRange.Row, newRow));
                insertingRowRange.Interior.Color = uncommitedCellsOLEColor;
              }

              int absRow = startDataTableRow + rowIdx - 1;
              absRow = SearchRowIndexNotDeleted(absRow, null);
              int absCol = startDataTableCol + colIdx - 1;

              currCol = editMySQLDataTable.GetColumnAtIndex(absCol);
              object insertingValue = DBNull.Value;
              if (cell.Value != null)
                insertingValue = DataTypeUtilities.GetInsertingValueForColumnType(cell.Value, currCol);
              if (editMySQLDataTable.Rows[absRow].RowState != DataRowState.Added)
              {
                if (DataTypeUtilities.ExcelValueEqualsDataTableValue(editMySQLDataTable.Rows[absRow][absCol, DataRowVersion.Original], insertingValue))
                {
                  var existingRA = rangesAndAddressesList.Find(ra => ra.Modification == RangeAndAddress.RangeModification.Updated && ra.Address == cell.Address);
                  if (existingRA != null)
                  {
                    changeExcelCellsColor(cell, (existingRA.RangeColor == whiteOLEColor ? 0 : existingRA.RangeColor));
                    rangesAndAddressesList.RemoveAll(ra => ra.Modification == RangeAndAddress.RangeModification.Updated && ra.Address == cell.Address);
                    editMySQLDataTable.Rows[absRow][absCol] = insertingValue;
                    int changedColsQty = editMySQLDataTable.GetChangedColumns(editMySQLDataTable.Rows[absRow]).Count;
                    if (changedColsQty == 0)
                      editMySQLDataTable.Rows[absRow].RejectChanges();
                  }
                  continue;
                }
                // Need to set the value before coloring the cell in case there is an invalid value it does not reach the coloring code
                DataRow dr = editMySQLDataTable.Rows[absRow];
                dr[absCol] = insertingValue;
                if (!rangesAndAddressesList.Exists(ra => ra.Modification == RangeAndAddress.RangeModification.Updated && ra.Address == cell.Address))
                  rangesAndAddressesList.Add(new RangeAndAddress(RangeAndAddress.RangeModification.Updated, cell, cell.Address, (int)cell.Interior.Color, cell.Row, dr));
              }
              else
                editMySQLDataTable.Rows[absRow][absCol] = insertingValue;
              cell.Interior.Color = uncommitedCellsOLEColor;
            }
        }
        catch (ArgumentException argEx)
        {
          undoChanges = true;
          operationSummary = String.Format(Properties.Resources.EditDataInvalidValueError, (currCol != null ? currCol.MySQLDataType : "Unknown"));
          operationDetails = argEx.Message;
        }
        catch (Exception ex)
        {
          undoChanges = true;
          operationSummary = Properties.Resources.EditDataCellModificationError;
          operationDetails = ex.Message;
          MiscUtilities.WriteAppErrorToLog(ex);
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
      EditDataRevertDialog revertDialog = new EditDataRevertDialog(!chkAutoCommit.Checked && uncommitedDataExists);
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

    private void EditDataDialog_Shown(object sender, EventArgs e)
    {
      // Need to call the ShowInactiveTopmost method when the form is shown in order to make it topmost and
      // to avoid that the controls inside it activate so focus remains on excel cells.
      ShowInactiveTopmost();
    }

  }

  public class RangeAndAddress
  {
    public enum RangeModification { Added, Deleted, Updated };
    public RangeModification Modification;
    public Excel.Range Range;
    public string Address;
    public int RangeColor;
    public int ExcelRow;
    public DataRow TableRow;

    public RangeAndAddress(RangeModification modification, Excel.Range range, string address, int rangeColor, int excelRow, DataRow tableRow)
    {
      Modification = modification;
      Range = range;
      Address = address;
      RangeColor = rangeColor;
      ExcelRow = excelRow;
      TableRow = tableRow;
    }
  }
}
