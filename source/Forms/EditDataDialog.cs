// Copyright (c) 2012-2014, Oracle and/or its affiliates. All rights reserved.
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
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;
using MySQL.ForExcel.Classes;
using MySQL.ForExcel.Controls;
using MySQL.ForExcel.Properties;
using MySQL.Utility.Classes;
using MySQL.Utility.Classes.MySQLWorkbench;
using MySQL.Utility.Forms;
using Excel = Microsoft.Office.Interop.Excel;

namespace MySQL.ForExcel.Forms
{
  /// <summary>
  /// Provides a minimalistic floating interface used for editing sessions against MySQL tables.
  /// </summary>
  public partial class EditDataDialog : AutoStyleableBaseForm
  {
    #region Constants

    /// <summary>
    /// Places the window above all non-topmost windows (that is, behind all topmost windows). This flag has no effect if the window is already a non-topmost window.
    /// </summary>
    private const int HWND_NOTOPMOST = -2;

    /// <summary>
    /// Displays a window in its most recent size and position. This value is similar to SW_SHOWNORMAL, except that the window is not activated.
    /// </summary>
    private const int SW_SHOWNOACTIVATE = 4;

    /// <summary>
    /// Does not activate the window. If this flag is not set, the window is activated and moved to the top of either the topmost or non-topmost group (depending on the setting of the hWndInsertAfter parameter).
    /// </summary>
    private const uint SWP_NOACTIVATE = 0x0010;

    #endregion Constants

    #region Fields

    /// <summary>
    /// True since the EditDataDialog is created until the first time its displayed, allows to handle the Show events correctly.
    /// </summary>
    private bool _neverBeenShown;

    /// <summary>
    /// A point object used as a placeholder to track where the mouse has been pressed.
    /// </summary>
    private Point _mouseDownPoint;

    /// <summary>
    /// Flag indicating whether the editing session is in process of undoing changes done
    /// </summary>
    private bool _undoingChanges;

    /// <summary>
    /// Flag indicating whether this editing session is changing the value of the global Use Optimistic Update setting.
    /// </summary>
    private bool _updatingUSeOptimisticUpdateSetting;

    #endregion Fields

    /// <summary>
    /// Initializes a new instance of the <see cref="EditDataDialog"/> class.
    /// </summary>
    /// <param name="parentTaskPane">The <see cref="ExcelAddInPane"/> from which the <see cref="EditDataDialog"/> is called.</param>
    /// <param name="parentWindow">The parent window assigned to the <see cref="EditDataDialog"/> to be opened as a dialog.</param>
    /// <param name="wbConnection">The connection to a MySQL server instance selected by users.</param>
    /// <param name="originalEditDataRange">The Excel cells range containing the MySQL table's data being edited.</param>
    /// <param name="importTable">The table containing the data imported from the MySQL table that will be edited.</param>
    /// <param name="editingWorksheet">The Excel worksheet tied to the current editing session.</param>
    public EditDataDialog(ExcelAddInPane parentTaskPane, IWin32Window parentWindow, MySqlWorkbenchConnection wbConnection, Excel.Range originalEditDataRange, MySqlDataTable importTable, Excel.Worksheet editingWorksheet)
    {
      _mouseDownPoint = Point.Empty;
      _neverBeenShown = true;
      _undoingChanges = false;
      _updatingUSeOptimisticUpdateSetting = false;

      InitializeComponent();

      var existingProtectionKey = editingWorksheet.GetProtectionKey();
      WorksheetProtectionKey = string.IsNullOrEmpty(existingProtectionKey) ? Guid.NewGuid().ToString() : existingProtectionKey;
      ParentTaskPane = parentTaskPane;
      ParentWindow = parentWindow;
      WbConnection = wbConnection;
      EditDataRange = originalEditDataRange;
      EditMySqlDataTable = importTable;
      EditingWorksheet = editingWorksheet;
      EditingWorksheet.SelectionChange += EditingWorksheet_SelectionChange;
      ResetToolTip();
      EditingColsQuantity = editingWorksheet.UsedRange.Columns.Count;
      Opacity = 0.60;
      AddNewRowToEditingRange(false);
      UseOptimisticUpdateForThisSession = Settings.Default.EditUseOptimisticUpdate;
      ForThisSessionToolStripMenuItem.Checked = UseOptimisticUpdateForThisSession;
      ForAllSessionsToolStripMenuItem.Checked = UseOptimisticUpdateForThisSession;
      UseOptimisticUpdateToolStripMenuItem.Checked = UseOptimisticUpdateForThisSession;
      Settings.Default.PropertyChanged += SettingsPropertyValueChanged;
    }

    /// <summary>
    /// Protects the edit dialog's worksheet.
    /// </summary>
    public void ProtectWorksheet()
    {
      EditingWorksheet.ProtectEditingWorksheet(EditingWorksheet_Change, WorksheetProtectionKey, EditDataRange);
    }

    #region Properties

    /// <summary>
    /// Gets the Excel cells range containing the MySQL table's data being edited.
    /// </summary>
    public Excel.Range EditDataRange { get; private set; }

    /// <summary>
    /// Gets the number of columns in the current editing session.
    /// </summary>
    public long EditingColsQuantity { get; private set; }

    /// <summary>
    /// Gets the number of rows in the current editing session.
    /// </summary>
    public long EditingRowsQuantity { get; private set; }

    /// <summary>
    /// Gets the name of the MySQL table whose data is being edited.
    /// </summary>
    public string EditingTableName
    {
      get
      {
        return EditMySqlDataTable != null ? EditMySqlDataTable.TableName : null;
      }
    }

    /// <summary>
    /// Gets the Excel worksheet tied to the current editing session.
    /// </summary>
    public Excel.Worksheet EditingWorksheet { get; private set; }

    /// <summary>
    /// Gets a value indicating whether the Excel worksheet tied to the current editing session still exists.
    /// </summary>
    public bool EditingWorksheetExists
    {
      get
      {
        bool exists;
        if (EditingWorksheet == null)
        {
          return false;
        }

        try
        {
          // Do NOT remove the following line although the wb variable is not used in the method the casting of the
          // EditingWorksheet.Parent is needed to determine if the parent Workbook is valid and has not been disposed of.
          Excel.Workbook wb = EditingWorksheet.Parent as Excel.Workbook;
          exists = true;
        }
        catch
        {
          exists = false;
        }

        return exists;
      }
    }

    /// <summary>
    /// Gets the <see cref="MySqlDataTable"/> whose data is being edited.
    /// </summary>
    public MySqlDataTable EditMySqlDataTable { get; private set; }

    /// <summary>
    /// Gets the <see cref="ExcelAddInPane"/> from which the <see cref="EditDataDialog"/> is called.
    /// </summary>
    public ExcelAddInPane ParentTaskPane { get; private set; }

    /// <summary>
    /// Gets the parent window assigned to the <see cref="EditDataDialog"/> to be opened as a dialog.
    /// </summary>
    public IWin32Window ParentWindow { get; private set; }

    /// <summary>
    /// Gets a value indicating whether uncommited data exists in the editing session.
    /// </summary>
    public bool UncommitedDataExists
    {
      get
      {
        return EditMySqlDataTable.ChangedOrDeletedRows > 0;
      }
    }

    /// <summary>
    /// Gets a value indicating whether optimistic update is used for the current editing session.
    /// </summary>
    public bool UseOptimisticUpdateForThisSession { get; private set; }

    /// <summary>
    /// Gets the connection to a MySQL server instance selected by users.
    /// </summary>
    public MySqlWorkbenchConnection WbConnection { get; private set; }

    /// <summary>
    /// Gets the name of the Excel workbook that contains the worksheet tied to the current editing session.
    /// </summary>
    public string WorkbookName
    {
      get
      {
        try
        {
          var workbook = EditingWorksheet.Parent as Excel.Workbook;
          if (workbook != null)
          {
            return workbook.Name;
          }
        }
        catch
        {
          return null;
        }

        return null;
      }
    }

    /// <summary>
    /// Gets the name of the Excel worksheet tied to the current editing session.
    /// </summary>
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

    /// <summary>
    /// Gets the GUID used as a key to protect the editing Excel worksheet.
    /// </summary>
    public string WorksheetProtectionKey { get; private set; }

    #endregion Properties

    /// <summary>
    /// Displays the control to the user.
    /// </summary>
    public new void ShowDialog()
    {
      if (_neverBeenShown)
      {
        Show(ParentWindow);
        _neverBeenShown = false;
      }
      else
      {
        ShowInactiveTopmost();
      }
    }

    /// <summary>
    /// Unprotects the edit dialog's worksheet.
    /// </summary>
    public void UnprotectWorksheet()
    {
      EditingWorksheet.UnprotectEditingWorksheet(EditingWorksheet_Change, WorksheetProtectionKey);
    }

    /// <summary>
    /// Raises the Closing event.
    /// </summary>
    /// <param name="e">A <see cref="CancelEventArgs"/> that contains the event data.</param>
    protected override void OnClosing(CancelEventArgs e)
    {
      base.OnClosing(e);
      ParentTaskPane.RefreshDbObjectPanelActionLabelsEnabledStatus(EditingTableName, false);
      if (EditingWorksheetExists)
      {
        UnprotectWorksheet();
        EditingWorksheet.UsedRange.Interior.ColorIndex = Excel.XlColorIndex.xlColorIndexNone;
        if (!string.IsNullOrEmpty(EditingWorksheet.GetProtectionKey()))
        {
          EditingWorksheet.RemoveProtectionKey();
        }
      }

      var session = Globals.ThisAddIn.ActiveWorkbookEditSessions.FirstOrDefault(ac => ac.EditDialog.Equals(this));
      if (session != null)
      {
        Globals.ThisAddIn.ActiveWorkbookEditSessions.Remove(session);
      }

      Dispose();
    }

    /// <summary>
    /// Raises the MouseDown event.
    /// </summary>
    /// <param name="e">A <see cref="MouseEventArgs"/> that contains the event data.</param>
    protected override void OnMouseDown(MouseEventArgs e)
    {
      base.OnMouseDown(e);
      GenericMouseDown(this, e);
    }

    /// <summary>
    /// Raises the MouseMove event.
    /// </summary>
    /// <param name="e">A <see cref="MouseEventArgs"/> that contains the event data.</param>
    protected override void OnMouseMove(MouseEventArgs e)
    {
      base.OnMouseMove(e);
      GenericMouseMove(this, e);
    }

    /// <summary>
    /// Raises the MouseUp event.
    /// </summary>
    /// <param name="e">A <see cref="MouseEventArgs"/> that contains the event data.</param>
    protected override void OnMouseUp(MouseEventArgs e)
    {
      base.OnMouseUp(e);
      GenericMouseUp(this, e);
    }

    /// <summary>
    /// Paints the background of the control.
    /// </summary>
    /// <param name="e">A <see cref="PaintEventArgs"/> that contains the event data.</param>
    protected override void OnPaintBackground(PaintEventArgs e)
    {
      base.OnPaintBackground(e);
      Pen pen = new Pen(Color.White, 3f);
      e.Graphics.DrawRectangle(pen, 0, 0, Width - 2, Height - 2);
      pen.Width = 1f;
      e.Graphics.DrawLine(pen, 0, 25, Width, 25);
      pen.Dispose();
    }

    /// <summary>
    /// Changes the size, position, and Z-order of child, pop-up, and top-level windows.
    /// </summary>
    /// <param name="hWnd">The window handle.</param>
    /// <param name="hWndInsertAfter">Identifies the CWnd object that will precede (be higher than) this CWnd object in the Z-order.</param>
    /// <param name="x">Specifies the new position of the left side of the window.</param>
    /// <param name="y">Specifies the new position of the top of the window.</param>
    /// <param name="cx">Specifies the new width of the window.</param>
    /// <param name="cy">Specifies the new height of the window.</param>
    /// <param name="uFlags">Specifies sizing and positioning options.</param>
    /// <returns><c>true</c> if the function is successful; <c>false</c> otherwise.</returns>
    [DllImport("user32.dll", EntryPoint = "SetWindowPos")]
    private static extern bool SetWindowPos(int hWnd, int hWndInsertAfter, int x, int y, int cx, int cy, uint uFlags);

    /// <summary>
    /// Sets the specified window's show state.
    /// </summary>
    /// <param name="hWnd">A handle to the window.</param>
    /// <param name="nCmdShow">Controls how the window is to be shown.</param>
    /// <returns><c>true</c> if the window was previously visible, <c>false</c> if the window was previously hidden.</returns>
    [DllImport("user32.dll")]
    private static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);

    /// <summary>
    /// Adds a new row at the bottom of the Excel editing range.
    /// </summary>
    /// <param name="clearColoringOfOldNewRow">Flag indicating whether the previous row that was placeholder for new rows is cleared of its formatting.</param>
    /// <returns>An Excel range containing just the newly added row.</returns>
    private Excel.Range AddNewRowToEditingRange(bool clearColoringOfOldNewRow)
    {
      Excel.Range newRowRange;
      EditingWorksheet.UnprotectEditingWorksheet(EditingWorksheet_Change, WorksheetProtectionKey);
      EditDataRange = EditDataRange.AddNewRow(clearColoringOfOldNewRow, out newRowRange);
      EditingWorksheet.ProtectEditingWorksheet(EditingWorksheet_Change, WorksheetProtectionKey, EditDataRange);
      EditingRowsQuantity = EditDataRange.Rows.Count;
      return newRowRange;
    }

    /// <summary>
    /// Event delegate method called when the <see cref="AutoCommitCheckBox"/> checked property value changes.
    /// </summary>
    /// <param name="sender">Sender object.</param>
    /// <param name="e">Event arguments.</param>
    private void AutoCommitCheckBox_CheckedChanged(object sender, EventArgs e)
    {
      CommitChangesButton.Enabled = !AutoCommitCheckBox.Checked && UncommitedDataExists;
    }

    /// <summary>
    /// Event delegate method called when the <see cref="CommitChangesButton"/> is clicked.
    /// </summary>
    /// <param name="sender">Sender object.</param>
    /// <param name="e">Event arguments.</param>
    private void CommitChangesButton_Click(object sender, EventArgs e)
    {
      PushDataChanges();
    }

    /// <summary>
    /// Event delegate method called when the <see cref="EditDataDialog"/> window is activated.
    /// </summary>
    /// <param name="sender">Sender object.</param>
    /// <param name="e">Event arguments.</param>
    private void EditDataDialog_Activated(object sender, EventArgs e)
    {
      ResetToolTip();
    }

    /// <summary>
    /// Event delegate method called when the <see cref="EditDataDialog"/> window is shown for the first time.
    /// </summary>
    /// <param name="sender">Sender object.</param>
    /// <param name="e">Event arguments.</param>
    private void EditDataDialog_Shown(object sender, EventArgs e)
    {
      // Need to call the ShowInactiveTopmost method when the form is shown in order to make it topmost and
      // to avoid that the controls inside it activate so focus remains on excel cells.
      ShowInactiveTopmost();
    }

    /// <summary>
    /// Event delegate method fired when any value in a cell within the <see cref="EditingWorksheet"/> changes.
    /// </summary>
    /// <remarks>
    /// This method is used to record any changes done by users to the data and prepare corresponding changes within a data table object
    /// that later will generate queries to commit the data changes to the MySQL server.
    /// </remarks>
    /// <param name="target"></param>
    private void EditingWorksheet_Change(Excel.Range target)
    {
      if (_undoingChanges)
      {
        return;
      }

      bool rowWasDeleted = EditingWorksheet.UsedRange.Rows.Count < EditingRowsQuantity && target.Columns.Count == EditingWorksheet.Columns.Count;
      bool undoChanges = false;
      string operationSummary = null;
      string operationDetails = null;

      Excel.Range intersectRange = EditDataRange.IntersectWith(target);
      if (intersectRange == null || intersectRange.CountLarge == 0)
      {
        undoChanges = true;
        if (rowWasDeleted)
        {
          // The row for insertions is attempted to be deleted, we need to undo
          operationSummary = Resources.EditDataDeleteLastRowNotPermittedErrorTitle;
          operationDetails = Resources.EditDataDeleteLastRowNotPermittedErrorDetail;
        }
        else
        {
          // It is a modification and outside the permitted range
          operationSummary = Resources.EditDataOutsideEditingRangeNotPermittedErrorTitle;
          operationDetails = Resources.EditDataOutsideEditingRangeNotPermittedErrorDetail;
        }
      }

      if (undoChanges)
      {
        MiscUtilities.ShowCustomizedErrorDialog(operationSummary, operationDetails, true);
        UndoChanges();
        if (!rowWasDeleted)
        {
          return;
        }

        EditDataRange = EditingWorksheet.UsedRange;
        return;
      }

      Excel.Range startCell = intersectRange.Item[1, 1] as Excel.Range;
      if (startCell != null)
      {
        // Substract from the Excel indexes since they start at 1, ExcelRow is subtracted by 2 if we imported headers.
        int startDataTableRow = startCell.Row - 2;
        int startDataTableCol = startCell.Column - 1;

        // Detect if a row was deleted and if so flag it for deletion
        if (rowWasDeleted)
        {
          List<int> skipDeletedRowsList = new List<int>();
          foreach (Excel.Range deletedRow in target.Rows)
          {
            startDataTableRow = deletedRow.Row - 2;
            startDataTableRow = EditMySqlDataTable.SearchRowIndexNotDeleted(startDataTableRow, skipDeletedRowsList, EditDataRange.Rows.Count);
            DataRow dr = EditMySqlDataTable.Rows[startDataTableRow];
            dr.Delete();
            skipDeletedRowsList.Add(startDataTableRow);
          }

          EditingRowsQuantity = EditDataRange.Rows.Count;
        }
        else
        {
          // The change was a modification of cell values
          MySqlDataColumn currCol = null;
          try
          {
            for (int rowIdx = 1; rowIdx <= intersectRange.Rows.Count; rowIdx++)
            {
              for (int colIdx = 1; colIdx <= intersectRange.Columns.Count; colIdx++)
              {
                Excel.Range cell = intersectRange.Cells[rowIdx, colIdx];
                if (cell == null)
                {
                  continue;
                }

                // Detect if a data row has been added by the user and if so flag it for addition
                if (cell.Row == EditDataRange.Rows.Count)
                {
                  if (cell.Value == null)
                  {
                    continue;
                  }

                  Excel.Range insertingRowRange = AddNewRowToEditingRange(true);
                  MySqlDataRow newRow = EditMySqlDataTable.NewRow() as MySqlDataRow;
                  if (newRow != null)
                  {
                    newRow.ExcelRange = insertingRowRange;
                    EditMySqlDataTable.Rows.Add(newRow);
                  }
                }

                int absRow = startDataTableRow + rowIdx - 1;
                absRow = EditMySqlDataTable.SearchRowIndexNotDeleted(absRow, null, EditDataRange.Rows.Count);
                int absCol = startDataTableCol + colIdx - 1;

                currCol = EditMySqlDataTable.GetColumnAtIndex(absCol);
                object insertingValue = DBNull.Value;
                if (cell.Value != null)
                {
                  insertingValue = DataTypeUtilities.GetInsertingValueForColumnType(cell.Value, currCol, false);
                }

                EditMySqlDataTable.Rows[absRow][absCol] = insertingValue;
              }
            }
          }
          catch (ArgumentException argEx)
          {
            undoChanges = true;
            operationSummary = string.Format(Resources.EditDataInvalidValueError, currCol != null ? currCol.MySqlDataType : "Unknown");
            operationDetails = argEx.Message;
          }
          catch (Exception ex)
          {
            undoChanges = true;
            operationSummary = Resources.EditDataCellModificationError;
            operationDetails = ex.Message;
            MySqlSourceTrace.WriteAppErrorToLog(ex);
          }
          finally
          {
            if (undoChanges)
            {
              MiscUtilities.ShowCustomizedErrorDialog(operationSummary, operationDetails, true);
              UndoChanges();
            }
          }
        }
      }

      CommitChangesButton.Enabled = !AutoCommitCheckBox.Checked && UncommitedDataExists;
      if (AutoCommitCheckBox.Checked && UncommitedDataExists)
      {
        PushDataChanges();
      }
    }

    /// <summary>
    /// Event delegate method fired when the Excel cells selection changes within the <see cref="EditingWorksheet"/>.
    /// </summary>
    /// <param name="target"></param>
    private void EditingWorksheet_SelectionChange(Excel.Range target)
    {
      Excel.Range intersectRange = EditDataRange.IntersectWith(target);
      if (intersectRange == null || intersectRange.CountLarge == 0)
      {
        Hide();
      }
      else
      {
        ShowInactiveTopmost();
      }
    }

    /// <summary>
    /// Event delegate method fired when the <see cref="ExitEditModeToolStripMenuItem"/> is clicked.
    /// </summary>
    /// <param name="sender">Sender object.</param>
    /// <param name="e">Event arguments.</param>
    private void ExitEditModeToolStripMenuItem_Click(object sender, EventArgs e)
    {
      Close();
    }

    /// <summary>
    /// Event delegate method fired when the <see cref="ForAllSessionsToolStripMenuItem"/> is clicked.
    /// </summary>
    /// <param name="sender">Sender object.</param>
    /// <param name="e">Event arguments.</param>
    private void ForAllSessionsToolStripMenuItem_Click(object sender, EventArgs e)
    {
      SetUseOptimisticUpdateForAllSessions(!UseOptimisticUpdateToolStripMenuItem.Checked, true);
    }

    /// <summary>
    /// Event delegate method fired when the <see cref="ForThisSessionToolStripMenuItem"/> is clicked.
    /// </summary>
    /// <param name="sender">Sender object.</param>
    /// <param name="e">Event arguments.</param>
    private void ForThisSessionToolStripMenuItem_Click(object sender, EventArgs e)
    {
      UseOptimisticUpdateForThisSession = !UseOptimisticUpdateForThisSession;
      ForThisSessionToolStripMenuItem.Checked = UseOptimisticUpdateForThisSession;
      UseOptimisticUpdateToolStripMenuItem.Checked = UseOptimisticUpdateForThisSession || ForAllSessionsToolStripMenuItem.Checked;
    }

    /// <summary>
    /// Event delegate method fired when a mouse button is pressed down.
    /// </summary>
    /// <param name="sender">Sender object.</param>
    /// <param name="e">Event arguments.</param>
    private void GenericMouseDown(object sender, MouseEventArgs e)
    {
      if (e.Button == MouseButtons.Left)
      {
        _mouseDownPoint = new Point(e.X, e.Y);
      }
    }

    /// <summary>
    /// Event delegate method fired when the mouse is moved.
    /// </summary>
    /// <param name="sender">Sender object.</param>
    /// <param name="e">Event arguments.</param>
    private void GenericMouseMove(object sender, MouseEventArgs e)
    {
      if (e.Button != MouseButtons.Left)
      {
        return;
      }

      if (_mouseDownPoint.IsEmpty)
      {
        return;
      }

      Location = new Point(Location.X + (e.X - _mouseDownPoint.X), Location.Y + (e.Y - _mouseDownPoint.Y));
    }

    /// <summary>
    /// Event delegate method fired when a mouse button is up.
    /// </summary>
    /// <param name="sender">Sender object.</param>
    /// <param name="e">Event arguments.</param>
    private void GenericMouseUp(object sender, MouseEventArgs e)
    {
      if (e.Button == MouseButtons.Left)
      {
        _mouseDownPoint = Point.Empty;
      }
    }

    /// <summary>
    /// Pushes the data changes currently done in the Excel worksheet to attempt to commit them to the MySQL server.
    /// </summary>
    /// <returns><c>true</c> if the transaction was committed successfully to the database, <c>false</c> otherwise.</returns>
    private bool PushDataChanges()
    {
      bool warningsFound = false;
      bool errorsFound = false;
      bool autoCommitOn = AutoCommitCheckBox.Checked;
      int warningsCount = 0;

      Cursor = Cursors.WaitCursor;
      EditMySqlDataTable.UseOptimisticUpdate = UseOptimisticUpdateForThisSession;
      var modifiedRowsList = EditMySqlDataTable.PushData(Settings.Default.GlobalSqlQueriesPreviewQueries);
      if (modifiedRowsList == null)
      {
        Cursor = Cursors.Default;
        return false;
      }

      StringBuilder operationSummary = new StringBuilder();
      StringBuilder operationDetails = new StringBuilder();
      StringBuilder warningDetails = new StringBuilder();
      StringBuilder warningStatementDetails = new StringBuilder();
      operationSummary.AppendFormat(Resources.EditedDataForTable, EditingTableName);
      if (Settings.Default.GlobalSqlQueriesShowQueriesWithResults)
      {
        operationDetails.AppendFormat(
          Resources.EditDataCommittedWithQueryText,
          modifiedRowsList.GetResultsCount(MySqlStatement.SqlStatementType.Delete),
          modifiedRowsList.GetResultsCount(MySqlStatement.SqlStatementType.Insert),
          modifiedRowsList.GetResultsCount(MySqlStatement.SqlStatementType.Update));
        operationDetails.AddNewLine();
      }

      bool warningDetailHeaderAppended = false;
      string statementsQuantityFormat = new string('0', modifiedRowsList.Count.StringSize());
      string sqlQueriesFormat = "{0:" + statementsQuantityFormat + "}: {1}";
      foreach (var statement in modifiedRowsList.Select(statementRow => statementRow.Statement))
      {
        if (Settings.Default.GlobalSqlQueriesShowQueriesWithResults && statement.SqlQuery.Length > 0)
        {
          operationDetails.AddNewLine();
          operationDetails.AppendFormat(sqlQueriesFormat, statement.ExecutionOrder, statement.SqlQuery);
        }

        switch (statement.StatementResult)
        {
          case MySqlStatement.StatementResultType.WarningsFound:
            if (Settings.Default.GlobalSqlQueriesPreviewQueries)
            {
              if (!warningDetailHeaderAppended)
              {
                warningDetailHeaderAppended = true;
                warningStatementDetails.AddNewLine(1, true);
                warningStatementDetails.Append(Resources.SqlStatementsProducingWarningsText);
              }

              if (statement.SqlQuery.Length > 0)
              {
                warningStatementDetails.AddNewLine(1, true);
                warningStatementDetails.AppendFormat(sqlQueriesFormat, statement.ExecutionOrder, statement.SqlQuery);
              }
            }

            warningsFound = true;
            warningDetails.AddNewLine(1, true);
            warningDetails.Append(statement.ResultText);
            warningsCount += statement.WarningsQuantity;
            break;

          case MySqlStatement.StatementResultType.ErrorThrown:
            errorsFound = true;
            operationDetails.AddNewLine(2, true);
            operationDetails.Append(statement.ResultText);
            break;
        }

        if (!errorsFound)
        {
          continue;
        }

        break;
      }

      if (warningsFound)
      {
        operationDetails.AddNewLine(2, true);
        operationDetails.AppendFormat(Resources.EditDataCommittedWarningsFound, warningsCount);
        operationDetails.AddNewLine();
        if (warningStatementDetails.Length > 0)
        {
          operationDetails.Append(warningStatementDetails);
          operationDetails.AddNewLine();
        }

        operationDetails.Append(warningDetails);
      }

      if (!Settings.Default.GlobalSqlQueriesShowQueriesWithResults)
      {
        operationDetails.AddNewLine(2, true);
        operationDetails.AppendFormat(
          Resources.EditDataCommittedText,
          modifiedRowsList.GetResultsCount(MySqlStatement.SqlStatementType.Delete),
          modifiedRowsList.GetResultsCount(MySqlStatement.SqlStatementType.Insert),
          modifiedRowsList.GetResultsCount(MySqlStatement.SqlStatementType.Update));
      }

      InfoDialog.InfoType operationsType;
      if (!errorsFound)
      {
        if (warningsFound)
        {
          operationSummary.Append(Resources.EditedDataCommittedWarning);
          operationsType = InfoDialog.InfoType.Warning;
        }
        else
        {
          operationSummary.Append(Resources.EditedDataCommittedSucess);
          operationsType = InfoDialog.InfoType.Success;
        }
      }
      else
      {
        operationSummary.Append(Resources.EditedDataCommittedError);
        operationsType = InfoDialog.InfoType.Error;
      }

      if (!autoCommitOn || warningsFound || errorsFound)
      {
        MiscUtilities.ShowCustomizedInfoDialog(operationsType, operationSummary.ToString(), operationDetails.ToString(), false);
      }

      operationSummary.Clear();
      operationDetails.Clear();
      warningDetails.Clear();
      warningStatementDetails.Clear();
      CommitChangesButton.Enabled = UncommitedDataExists && !autoCommitOn;
      Cursor = Cursors.Default;
      return !errorsFound;
    }

    /// <summary>
    /// Resets the tooltip shown in the <see cref="EditDataDialog"/> to show information on its corresponding editing session.
    /// </summary>
    private void ResetToolTip()
    {
      DialogToolTip.SetToolTip(this, string.Format(Resources.EditDataFormTooltipText, Environment.NewLine, WbConnection.Schema, EditingTableName, WorkbookName, WorksheetName));
    }

    /// <summary>
    /// Event delegate method fired when the <see cref="RevertDataButton"/> is clicked.
    /// </summary>
    /// <param name="sender">Sender object.</param>
    /// <param name="e">Event arguments.</param>
    private void RevertDataButton_Click(object sender, EventArgs e)
    {
      EditDataRevertDialog revertDialog = new EditDataRevertDialog(!AutoCommitCheckBox.Checked && UncommitedDataExists);
      DialogResult dr = revertDialog.ShowDialog();
      if (dr == DialogResult.Cancel)
      {
        return;
      }

      RevertDataChanges(revertDialog.SelectedAction == EditDataRevertDialog.EditUndoAction.RefreshData);
    }

    /// <summary>
    /// Reverts the changes done to Excel cell values after the last commit.
    /// </summary>
    /// <param name="refreshFromDb">Flag indicating if instead of reverting the data back to the way it was when the editing session started, it is pulled to have the most recent version of it.</param>
    private void RevertDataChanges(bool refreshFromDb)
    {
      if (!refreshFromDb)
      {
        EditMySqlDataTable.RejectChanges();
      }
      else
      {
        Exception exception;
        EditMySqlDataTable.RefreshData(out exception);
        MiscUtilities.ShowCustomizedErrorDialog(Resources.EditDataRefreshErrorText, exception.Message);
      }

      Globals.ThisAddIn.SkipSelectedDataContentsDetection = true;
      EditingWorksheet.UnprotectEditingWorksheet(EditingWorksheet_Change, WorksheetProtectionKey);
      EditDataRange.Clear();
      Excel.Range topLeftCell = EditDataRange.Cells[1, 1];
      topLeftCell.Select();
      EditDataRange = EditMySqlDataTable.ImportDataIntoExcelRange(topLeftCell);
      CommitChangesButton.Enabled = false;
      AddNewRowToEditingRange(false);
    }

    /// <summary>
    /// Sets the value of the global optimistic update for all sessions property and updates the context menu options accordingly.
    /// </summary>
    /// <param name="value">The new value of the property.</param>
    /// <param name="saveInSettings">Flag indicating whether the new value must be saved in the settings file.</param>
    private void SetUseOptimisticUpdateForAllSessions(bool value, bool saveInSettings)
    {
      _updatingUSeOptimisticUpdateSetting = true;
      if (saveInSettings)
      {
        Settings.Default.EditUseOptimisticUpdate = value;
        MiscUtilities.SaveSettings();
      }

      ForAllSessionsToolStripMenuItem.Checked = value;
      if (value)
      {
        UseOptimisticUpdateForThisSession = true;
        ForThisSessionToolStripMenuItem.Checked = true;
        UseOptimisticUpdateToolStripMenuItem.Checked = true;
      }

      _updatingUSeOptimisticUpdateSetting = false;
    }

    /// <summary>
    /// Event delegate method fired when a settings property value changes.
    /// </summary>
    /// <param name="sender">Sender object.</param>
    /// <param name="e">Event arguments.</param>
    private void SettingsPropertyValueChanged(object sender, PropertyChangedEventArgs e)
    {
      if (e.PropertyName != "EditUseOptimisticUpdate" || _updatingUSeOptimisticUpdateSetting)
      {
        return;
      }

      SetUseOptimisticUpdateForAllSessions(Settings.Default.EditUseOptimisticUpdate, false);
    }

    /// <summary>
    /// Shows the dialog as the topmost window without placing the focus on it (i.e. leaving the focus on the parent window).
    /// </summary>
    private void ShowInactiveTopmost()
    {
      ShowWindow(Handle, SW_SHOWNOACTIVATE);
      SetWindowPos(Handle.ToInt32(), HWND_NOTOPMOST, Left, Top, Width, Height, SWP_NOACTIVATE);
    }

    /// <summary>
    /// Undoes changes in the <see cref="EditingWorksheet"/> only.
    /// </summary>
    private void UndoChanges()
    {
      _undoingChanges = true;
      try
      {
        EditingWorksheet.Application.Undo();
      }
      catch (Exception ex)
      {
        MySqlSourceTrace.WriteAppErrorToLog(ex);
      }

      _undoingChanges = false;
    }
  }
}