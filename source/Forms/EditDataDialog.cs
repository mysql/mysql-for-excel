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
using ExcelInterop = Microsoft.Office.Interop.Excel;

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
    /// The Excel cells range containing the MySQL table's data being edited.
    /// </summary>
    private ExcelInterop.Range _editDataRange;

    /// <summary>
    /// The number of rows in the current editing session.
    /// </summary>
    private long _editingRowsQuantity;

    /// <summary>
    /// True since the EditDataDialog is created until the first time its displayed, allows to handle the Show events correctly.
    /// </summary>
    private bool _neverBeenShown;

    /// <summary>
    /// A point object used as a placeholder to track where the mouse has been pressed.
    /// </summary>
    private Point _mouseDownPoint;

    /// <summary>
    /// The <see cref="MySqlDataTable"/> whose data is being edited.
    /// </summary>
    private readonly MySqlDataTable _mySqlTable;

    /// <summary>
    /// The <see cref="ExcelAddInPane"/> from which the <see cref="EditDataDialog"/> is called.
    /// </summary>
    private ExcelAddInPane _parentTaskPane;

    /// <summary>
    /// The parent window assigned to the <see cref="EditDataDialog"/> to be opened as a dialog.
    /// </summary>
    private IWin32Window _parentWindow;

    /// <summary>
    /// Flag indicating whether the editing session is in process of undoing changes done
    /// </summary>
    private bool _undoingChanges;

    /// <summary>
    /// Flag indicating whether this editing session is changing the value of the global Use Optimistic Update setting.
    /// </summary>
    private bool _updatingUSeOptimisticUpdateSetting;

    /// <summary>
    /// Flag indicating whether optimistic update is used for the current editing session.
    /// </summary>
    private bool _useOptimisticUpdateForThisSession;

    /// <summary>
    /// The connection to a MySQL server instance selected by users.
    /// </summary>
    private MySqlWorkbenchConnection _wbConnection;

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
    public EditDataDialog(ExcelAddInPane parentTaskPane, IWin32Window parentWindow, MySqlWorkbenchConnection wbConnection, ExcelInterop.Range originalEditDataRange, MySqlDataTable importTable, ExcelInterop.Worksheet editingWorksheet)
    {
      _mouseDownPoint = Point.Empty;
      _neverBeenShown = true;
      _undoingChanges = false;
      _updatingUSeOptimisticUpdateSetting = false;

      InitializeComponent();

      var existingProtectionKey = editingWorksheet.GetProtectionKey();
      WorksheetProtectionKey = string.IsNullOrEmpty(existingProtectionKey) ? Guid.NewGuid().ToString() : existingProtectionKey;
      _parentTaskPane = parentTaskPane;
      _parentWindow = parentWindow;
      _wbConnection = wbConnection;
      _editDataRange = originalEditDataRange;
      _mySqlTable = importTable;
      EditingWorksheet = editingWorksheet;
      EditingWorksheet.SelectionChange += EditingWorksheet_SelectionChange;
      ResetToolTip();
      Opacity = 0.60;
      AddNewRowToEditingRange(false);
      _useOptimisticUpdateForThisSession = Settings.Default.EditUseOptimisticUpdate;
      ForThisSessionToolStripMenuItem.Checked = _useOptimisticUpdateForThisSession;
      ForAllSessionsToolStripMenuItem.Checked = _useOptimisticUpdateForThisSession;
      UseOptimisticUpdateToolStripMenuItem.Checked = _useOptimisticUpdateForThisSession;
      Settings.Default.PropertyChanged += SettingsPropertyValueChanged;
    }

    #region Properties

    /// <summary>
    /// Gets the name of the MySQL table whose data is being edited.
    /// </summary>
    [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    public string EditingTableName
    {
      get
      {
        return _mySqlTable != null ? _mySqlTable.TableName : null;
      }
    }

    /// <summary>
    /// Gets the Excel worksheet tied to the current editing session.
    /// </summary>
    [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    public ExcelInterop.Worksheet EditingWorksheet { get; private set; }

    /// <summary>
    /// Gets a value indicating whether the Excel worksheet tied to the current editing session still exists.
    /// </summary>
    [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
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
          ExcelInterop.Workbook wb = EditingWorksheet.Parent as ExcelInterop.Workbook;
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
    /// Gets the name of the Excel workbook that contains the worksheet tied to the current editing session.
    /// </summary>
    [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    public string WorkbookName
    {
      get
      {
        try
        {
          var workbook = EditingWorksheet.Parent as ExcelInterop.Workbook;
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
    [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
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
    [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    public string WorksheetProtectionKey { get; private set; }

    /// <summary>
    /// Gets a value indicating whether uncommited data exists in the editing session.
    /// </summary>
    private bool UncommitedDataExists
    {
      get
      {
        return _mySqlTable.ChangedOrDeletedRows > 0;
      }
    }

    #endregion Properties

    /// <summary>
    /// Protects the edit dialog's worksheet.
    /// </summary>
    public void ProtectWorksheet()
    {
      EditingWorksheet.ProtectEditingWorksheet(EditingWorksheet_Change, WorksheetProtectionKey, _editDataRange);
    }

    /// <summary>
    /// Displays the control to the user.
    /// </summary>
    public new void ShowDialog()
    {
      if (_neverBeenShown)
      {
        Show(_parentWindow);
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
      _parentTaskPane.RefreshDbObjectPanelActionLabelsEnabledStatus(EditingTableName, false);
      if (EditingWorksheetExists)
      {
        UnprotectWorksheet();
        EditingWorksheet.UsedRange.Interior.ColorIndex = ExcelInterop.XlColorIndex.xlColorIndexNone;
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
    private ExcelInterop.Range AddNewRowToEditingRange(bool clearColoringOfOldNewRow)
    {
      ExcelInterop.Range newRowRange;
      EditingWorksheet.UnprotectEditingWorksheet(EditingWorksheet_Change, WorksheetProtectionKey);
      _editDataRange = _editDataRange.AddNewRow(clearColoringOfOldNewRow, out newRowRange);
      EditingWorksheet.ProtectEditingWorksheet(EditingWorksheet_Change, WorksheetProtectionKey, _editDataRange);
      _editingRowsQuantity = _editDataRange.Rows.Count;
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
    private void EditingWorksheet_Change(ExcelInterop.Range target)
    {
      if (_undoingChanges)
      {
        return;
      }

      bool rowWasDeleted = EditingWorksheet.UsedRange.Rows.Count < _editingRowsQuantity && target.Columns.Count == EditingWorksheet.Columns.Count;
      bool undoChanges = false;
      string operationSummary = null;
      string operationDetails = null;

      ExcelInterop.Range intersectRange = _editDataRange.IntersectWith(target);
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

        _editDataRange = EditingWorksheet.UsedRange;
        return;
      }

      ExcelInterop.Range startCell = intersectRange.Item[1, 1] as ExcelInterop.Range;
      if (startCell != null)
      {
        // Substract from the Excel indexes since they start at 1, ExcelRow is subtracted by 2 if we imported headers.
        int startDataTableRow = startCell.Row - 2;
        int startDataTableCol = startCell.Column - 1;

        // Detect if a row was deleted and if so flag it for deletion
        if (rowWasDeleted)
        {
          List<int> skipDeletedRowsList = new List<int>();
          foreach (ExcelInterop.Range deletedRow in target.Rows)
          {
            startDataTableRow = deletedRow.Row - 2;
            startDataTableRow = _mySqlTable.SearchRowIndexNotDeleted(startDataTableRow, skipDeletedRowsList, _editDataRange.Rows.Count);
            DataRow dr = _mySqlTable.Rows[startDataTableRow];
            dr.Delete();
            skipDeletedRowsList.Add(startDataTableRow);
          }

          _editingRowsQuantity = _editDataRange.Rows.Count;
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
                ExcelInterop.Range cell = intersectRange.Cells[rowIdx, colIdx];
                if (cell == null)
                {
                  continue;
                }

                // Detect if a data row has been added by the user and if so flag it for addition
                if (cell.Row == _editDataRange.Rows.Count)
                {
                  if (cell.Value == null)
                  {
                    continue;
                  }

                  ExcelInterop.Range insertingRowRange = AddNewRowToEditingRange(true);
                  MySqlDataRow newRow = _mySqlTable.NewRow() as MySqlDataRow;
                  if (newRow != null)
                  {
                    newRow.ExcelRange = insertingRowRange;
                    _mySqlTable.Rows.Add(newRow);
                  }
                }

                int absRow = startDataTableRow + rowIdx - 1;
                absRow = _mySqlTable.SearchRowIndexNotDeleted(absRow, null, _editDataRange.Rows.Count);
                int absCol = startDataTableCol + colIdx - 1;

                currCol = _mySqlTable.GetColumnAtIndex(absCol);
                object insertingValue = DBNull.Value;
                if (cell.Value != null)
                {
                  insertingValue = DataTypeUtilities.GetInsertingValueForColumnType(cell.Value, currCol, false);
                }

                _mySqlTable.Rows[absRow][absCol] = insertingValue;
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
    private void EditingWorksheet_SelectionChange(ExcelInterop.Range target)
    {
      ExcelInterop.Range intersectRange = _editDataRange.IntersectWith(target);
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
      _useOptimisticUpdateForThisSession = !_useOptimisticUpdateForThisSession;
      ForThisSessionToolStripMenuItem.Checked = _useOptimisticUpdateForThisSession;
      UseOptimisticUpdateToolStripMenuItem.Checked = _useOptimisticUpdateForThisSession || ForAllSessionsToolStripMenuItem.Checked;
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
      _mySqlTable.UseOptimisticUpdate = _useOptimisticUpdateForThisSession;
      var modifiedRowsList = _mySqlTable.PushData(Settings.Default.GlobalSqlQueriesPreviewQueries);
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
      DialogToolTip.SetToolTip(this, string.Format(Resources.EditDataFormTooltipText, Environment.NewLine, _wbConnection.Schema, EditingTableName, WorkbookName, WorksheetName));
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
        _mySqlTable.RejectChanges();
      }
      else
      {
        Exception exception;
        _mySqlTable.RefreshData(out exception);
        MiscUtilities.ShowCustomizedErrorDialog(Resources.EditDataRefreshErrorText, exception.Message);
      }

      Globals.ThisAddIn.SkipSelectedDataContentsDetection = true;
      EditingWorksheet.UnprotectEditingWorksheet(EditingWorksheet_Change, WorksheetProtectionKey);
      _editDataRange.Clear();
      ExcelInterop.Range topLeftCell = _editDataRange.Cells[1, 1];
      topLeftCell.Select();
      _editDataRange = _mySqlTable.ImportDataIntoExcelRange(topLeftCell);
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
        _useOptimisticUpdateForThisSession = true;
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