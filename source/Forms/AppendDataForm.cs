// 
// Copyright (c) 2012-2013, Oracle and/or its affiliates. All rights reserved.
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

namespace MySQL.ForExcel
{
  using System;
  using System.Collections.Generic;
  using System.ComponentModel;
  using System.Data;
  using System.Drawing;
  using System.Linq;
  using System.Text;
  using System.Windows.Forms;
  using MySql.Data.MySqlClient;
  using MySQL.ForExcel.Properties;
  using MySQL.Utility;
  using MySQL.Utility.Forms;
  using Excel = Microsoft.Office.Interop.Excel;

  /// <summary>
  /// Provides an interface to append Excel data into an existing MySQL table.
  /// </summary>
  public partial class AppendDataForm : AutoStyleableBaseDialog
  {
    #region Fields

    /// <summary>
    /// Rectangle used to measure drag&drop operations.
    /// </summary>
    private Rectangle _dragBoxFromMouseDown;

    /// <summary>
    /// Cursor displayed during a mapping operation when a column is being dragged.
    /// </summary>
    private Cursor _draggingCursor;

    /// <summary>
    /// Cursor displayed during a mapping operation when a column is being dropped.
    /// </summary>
    private Cursor _droppableCursor;

    /// <summary>
    /// The index of the grid column where the mouse is right-clicked to open a context menu.
    /// </summary>
    private int _gridColumnClicked;

    /// <summary>
    /// The index of the grid column being dragged in a drag&drop operation to map or unmap a column.
    /// </summary>
    private int _gridColumnIndexToDrag;

    /// <summary>
    /// The index of the grid column where a dragged column is being dropped during a drag&drop operation to map a column.
    /// </summary>
    private int _gridTargetTableColumnIndexToDrop;

    /// <summary>
    /// The reference point used in drag&drop operations.
    /// </summary>
    private Point _screenOffset;

    /// <summary>
    /// Cursor displayed during a mapping operation when a column is being dragged out of the columns area, so the column is unmapped.
    /// </summary>
    private Cursor _trashCursor;

    #endregion Fields

    /// <summary>
    /// Initializes a new instance of the <see cref="AppendDataForm"/> class.
    /// </summary>
    /// <param name="wbConnection">The connection to a MySQL server instance selected by users.</param>
    /// <param name="appendDataRange">The Excel range containing the data to append to a MySQL table.</param>
    /// <param name="importDBObject">The database object to which to append data to.</param>
    /// <param name="appendingWorksheetName">The name of the worksheet holding the appending data.</param>
    public AppendDataForm(MySqlWorkbenchConnection wbConnection, Excel.Range appendDataRange, DBObject importDBObject, string appendingWorksheetName)
    {
      AppendDataRange = appendDataRange;
      ColumnsMappingInFileList = new MySQLColumnMappingList();
      SourceMySQLCompleteDataTable = null;
      SourceMySQLPreviewDataTable = null;
      TargetMySQLDataTable = null;
      WBConnection = wbConnection;

      _dragBoxFromMouseDown = Rectangle.Empty;
      _draggingCursor = MiscUtilities.CreateCursor(new Bitmap(Properties.Resources.MySQLforExcel_Cursor_Dragging_32x32), 3, 3);
      _droppableCursor = MiscUtilities.CreateCursor(new Bitmap(Properties.Resources.MySQLforExcel_Cursor_Dropable_32x32), 3, 3);
      _gridColumnClicked = -1;
      _gridColumnIndexToDrag = -1;
      _gridTargetTableColumnIndexToDrop = -1;
      _trashCursor = MiscUtilities.CreateCursor(new Bitmap(Properties.Resources.MySQLforExcel_Cursor_Trash_32x32), 3, 3);

      InitializeComponent();

      SourceExcelDataDataGridView.EnableHeadersVisualStyles = false;

      InitializeFromTableGrid(wbConnection.Schema, importDBObject.Name);
      InitializeToTableGrid(importDBObject);

      string excelRangeAddress = appendDataRange.Address.Replace("$", string.Empty);
      Text = string.Format("Append Data - {0} [{1}]", appendingWorksheetName, excelRangeAddress);
      MaxMappingColumnsQuantity = Math.Min(TargetMySQLTableDataGridView.Columns.Count, SourceExcelDataDataGridView.Columns.Count);
      ClearMappings(true);
      RefreshMappingMethodCombo();
      if (!SelectStoredMappingForTargetTable())
      {
        MappingMethodComboBox.SelectedIndex = Settings.Default.AppendPerformAutoMap ? 0 : 1;
      }
    }

    #region Properties

    /// <summary>
    /// Gets the Excel range containing the data to append to a MySQL table.
    /// </summary>
    public Excel.Range AppendDataRange { get; private set; }

    /// <summary>
    /// Gets the list of column mappings saved in file.
    /// </summary>
    public MySQLColumnMappingList ColumnsMappingInFileList { get; private set; }

    /// <summary>
    /// Gets the column mapping currently being used in the append session.
    /// </summary>
    public MySQLColumnMapping CurrentColumnMapping { get; private set; }

    /// <summary>
    /// Gets the maximum number of columns that can be mapped based on the maximum number of columns between the source and the target tables.
    /// </summary>
    public int MaxMappingColumnsQuantity { get; private set; }

    /// <summary>
    /// Gets the table containing the whole set of Excel data to append to a MySQL Server table.
    /// </summary>
    public MySQLDataTable SourceMySQLCompleteDataTable { get; private set; }

    /// <summary>
    /// Gets the table containing a small preview subset of Excel data to append to a MySQL Server table.
    /// </summary>
    public MySQLDataTable SourceMySQLPreviewDataTable { get; private set; }

    /// <summary>
    /// Gets a list of column mappings for the current user.
    /// </summary>
    public List<MySQLColumnMapping> StoredColumnMappingsList
    {
      get
      {
        return ColumnsMappingInFileList.UserColumnMappingsList;
      }
    }

    /// <summary>
    /// Gets the table containing a small preview subset of the MySQL Server table where data is going to be appended to.
    /// </summary>
    public MySQLDataTable TargetMySQLDataTable { get; private set; }

    /// <summary>
    /// Gets the connection to a MySQL server instance selected by users.
    /// </summary>
    public MySqlWorkbenchConnection WBConnection { get; private set; }

    #endregion Properties

    /// <summary>
    /// Event delegate method fired when the <see cref="AdvancedOptionsButton"/> button is clicked.
    /// </summary>
    /// <param name="sender">Sender object.</param>
    /// <param name="e">Event arguments.</param>
    private void AdvancedOptionsButton_Click(object sender, EventArgs e)
    {
      using (AppendAdvancedOptionsDialog optionsDialog = new AppendAdvancedOptionsDialog())
      {
        optionsDialog.ShowDialog();
      }
    }

    /// <summary>
    /// Event delegate method fired when the <see cref="AppendButton"/> button is clicked.
    /// </summary>
    /// <param name="sender">Sender object.</param>
    /// <param name="e">Event arguments.</param>
    private void AppendButton_Click(object sender, EventArgs e)
    {
      DialogResult dr;
      if (TargetMySQLDataTable.MappedColumnsQuantity < MaxMappingColumnsQuantity)
      {
        dr = InfoDialog.ShowWarningDialog(Properties.Resources.ColumnMappingIncompleteTitleWarning, Properties.Resources.ColumnMappingIncompleteDetailWarning);
        if (dr == DialogResult.No)
        {
          return;
        }
      }

      Cursor = Cursors.WaitCursor;
      if (SourceMySQLCompleteDataTable == null)
      {
        SourceMySQLCompleteDataTable = SourceMySQLPreviewDataTable.CloneSchema();
        SourceMySQLCompleteDataTable.DetectDatatype = false;
        SourceMySQLCompleteDataTable.SetData(AppendDataRange, false, false);
      }
      else
      {
        SourceMySQLCompleteDataTable.SyncSchema(SourceMySQLPreviewDataTable);
      }

      Exception exception;
      bool warningsFound = false;
      int appendCount = 0;
      string insertQuery;
      string operationDetail = string.Empty;

      DataTable warningsTable = TargetMySQLDataTable.AppendDataWithManualQuery(SourceMySQLCompleteDataTable, out exception, out insertQuery, out appendCount);
      bool success = exception == null;
      StringBuilder operationMoreInfo = new StringBuilder();
      operationMoreInfo.AppendFormat(Resources.InsertingExcelDataWithQueryText, TargetMySQLDataTable.TableName);
      operationMoreInfo.Append(Environment.NewLine);
      operationMoreInfo.Append(Environment.NewLine);
      operationMoreInfo.Append(insertQuery);
      operationMoreInfo.Append(Environment.NewLine);
      operationMoreInfo.Append(Environment.NewLine);

      if (success)
      {
        if (warningsTable != null && warningsTable.Rows.Count > 0)
        {
          warningsFound = true;
          operationMoreInfo.AppendFormat(Resources.AppendDataRowsAppendedWithWarningsText, appendCount, warningsTable.Rows.Count);
          foreach (DataRow warningRow in warningsTable.Rows)
          {
            operationMoreInfo.Append(Environment.NewLine);
            operationMoreInfo.AppendFormat(Resources.WarningSingleText, warningRow[1].ToString(), warningRow[2].ToString());
          }
        }
        else
        {
          operationMoreInfo.AppendFormat(Resources.AppendDataRowsAppendedSuccessfullyText, appendCount);
        }
      }
      else
      {
        operationMoreInfo.Append(Resources.AppendDataRowsInsertionErrorText);
        operationMoreInfo.Append(Environment.NewLine);
        operationMoreInfo.Append(Environment.NewLine);

        if (exception is MySqlException)
        {
          operationMoreInfo.AppendFormat(Resources.ErrorMySQLText, (exception as MySqlException).Number);
          operationMoreInfo.Append(Environment.NewLine);
        }
        else
        {
          operationMoreInfo.Append(Resources.ErrorAdoNetText);
          operationMoreInfo.Append(Environment.NewLine);
        }

        operationMoreInfo.Append(exception.Message);
      }

      InfoDialog.InfoType operationsType;
      if (success)
      {
        if (warningsFound)
        {
          operationDetail = string.Format(Resources.AppendDataDetailsDoneWarningsText, TargetMySQLDataTable.TableName);
          operationsType = InfoDialog.InfoType.Warning;
        }
        else
        {
          operationDetail = string.Format(Resources.AppendDataDetailsDoneSuccessText, TargetMySQLDataTable.TableName);
          operationsType = InfoDialog.InfoType.Success;
        }
      }
      else
      {
        operationDetail = string.Format(Resources.AppendDataDetailsDoneErrorText, TargetMySQLDataTable.TableName);
        operationsType = InfoDialog.InfoType.Error;
      }

      Cursor = Cursors.Default;
      dr = MiscUtilities.ShowCustomizedInfoDialog(operationsType, operationDetail, operationMoreInfo.ToString(), false);
      if (dr == DialogResult.Cancel)
      {
        return;
      }

      if (Settings.Default.AppendAutoStoreColumnMapping
          && !StoredColumnMappingsList.Exists(mapping => mapping.ConnectionName == WBConnection.Name && mapping.SchemaName == WBConnection.Schema && mapping.TableName == TargetMySQLDataTable.TableName))
      {
        StoreCurrentColumnMapping(false);
      }

      DialogResult = DialogResult.OK;
      Close();
    }

    /// <summary>
    /// Event delegate method fired when the <see cref="AppendContextMenu"/> menu is being opened.
    /// </summary>
    /// <param name="sender">Sender object.</param>
    /// <param name="e">Event arguments.</param>
    private void AppendContextMenu_Opening(object sender, CancelEventArgs e)
    {
      if (_gridColumnClicked < 0 || CurrentColumnMapping == null || CurrentColumnMapping.MappedQuantity == 0)
      {
        e.Cancel = true;
      }

      AppendContextMenu.Items["removeColumnMappingToolStripMenuItem"].Visible = (_gridColumnClicked > -1 && CurrentColumnMapping.MappedSourceIndexes[_gridColumnClicked] >= 0);
      AppendContextMenu.Items["clearAllMappingsToolStripMenuItem"].Visible = (CurrentColumnMapping != null && CurrentColumnMapping.MappedQuantity > 0);
    }

    /// <summary>
    /// Applies the column mapping the users selects from a list of stored mappings.
    /// </summary>
    private void ApplySelectedStoredColumnMapping()
    {
      if (CurrentColumnMapping != null)
      {
        ClearMappings(true);

        for (int mappedIdx = 0; mappedIdx < CurrentColumnMapping.MappedSourceIndexes.Length; mappedIdx++)
        {
          if (mappedIdx >= MaxMappingColumnsQuantity)
          {
            break;
          }

          int currentMappedSourceIndex = CurrentColumnMapping.MappedSourceIndexes[mappedIdx];
          string currentMappedColName = currentMappedSourceIndex >= 0 ? CurrentColumnMapping.SourceColumns[currentMappedSourceIndex] : null;
          ApplySingleMapping(currentMappedSourceIndex, mappedIdx, currentMappedColName);
        }

        TargetMySQLTableDataGridView.Refresh();
        SourceExcelDataDataGridView.Refresh();
      }

      StoreMappingButton.Enabled = CurrentColumnMapping.MappedQuantity > 0;
    }

    /// <summary>
    /// Applies a single-column mapping.
    /// </summary>
    /// <param name="sourceColumnIndex">Column index in the source table being mapped.</param>
    /// <param name="targetColumnIndex">Column index in the target table where data is mapped to.</param>
    /// <param name="mappedColName">Name of the target column in the target table.</param>
    private void ApplySingleMapping(int sourceColumnIndex, int targetColumnIndex, string mappedColName)
    {
      int previouslyMappedFromIndex = CurrentColumnMapping.MappedSourceIndexes[targetColumnIndex];
      bool mapping = !string.IsNullOrEmpty(mappedColName) && sourceColumnIndex >= 0;
      DataGridViewCellStyle newStyle;

      //// Change text and style of target table column
      MultiHeaderColumn multiHeaderCol = TargetMySQLTableDataGridView.MultiHeaderColumnList[targetColumnIndex];
      multiHeaderCol.HeaderText = mapping ? mappedColName : string.Empty;
      multiHeaderCol.BackgroundColor = mapping ? Color.LightGreen : Color.OrangeRed;

      //// Change style of source table column being mapped or unmapped
      if (mapping)
      {
        newStyle = new DataGridViewCellStyle(SourceExcelDataDataGridView.Columns[sourceColumnIndex].HeaderCell.Style);
        newStyle.SelectionBackColor = newStyle.BackColor = Color.LightGreen;
        SourceExcelDataDataGridView.Columns[sourceColumnIndex].HeaderCell.Style = newStyle;
      }
      else if (previouslyMappedFromIndex >= 0 && CurrentColumnMapping.MappedSourceIndexes.Count(sourceIdx => sourceIdx == previouslyMappedFromIndex) <= 1)
      {
        newStyle = new DataGridViewCellStyle(SourceExcelDataDataGridView.Columns[previouslyMappedFromIndex].HeaderCell.Style);
        newStyle.SelectionBackColor = newStyle.BackColor = SystemColors.Control;
        SourceExcelDataDataGridView.Columns[previouslyMappedFromIndex].HeaderCell.Style = newStyle;
      }

      //// Store the actual mapping
      MySQLDataColumn sourceColumn = mapping ? SourceMySQLPreviewDataTable.GetColumnAtIndex(sourceColumnIndex) : null;
      MySQLDataColumn targetColumn = TargetMySQLDataTable.GetColumnAtIndex(targetColumnIndex);
      targetColumn.MappedDataColName = mapping ? sourceColumn.ColumnName : null;

      CurrentColumnMapping.MappedSourceIndexes[targetColumnIndex] = sourceColumnIndex;
    }

    /// <summary>
    /// Event delegate method fired when the <see cref="ClearAllMappingsToolStripMenuItem"/> menu item is clicked.
    /// </summary>
    /// <param name="sender">Sender object.</param>
    /// <param name="e">Event arguments.</param>
    private void ClearAllMappingsToolStripMenuItem_Click(object sender, EventArgs e)
    {
      ClearMappings(false);
      if (CurrentColumnMapping.Name == "Automatic")
      {
        MappingMethodComboBox.SelectedIndex = 0;
      }
    }

    /// <summary>
    /// Clears the current column mappings.
    /// </summary>
    /// <param name="onlyGrids">Flag indicating whether only the grids are cleared but not the mapping in memory.</param>
    private void ClearMappings(bool onlyGrids)
    {
      bool newMappings = TargetMySQLTableDataGridView.MultiHeaderColumnList.Count == 0;
      for (int colIdx = 0; colIdx < TargetMySQLTableDataGridView.Columns.Count; colIdx++)
      {
        if (newMappings)
        {
          TargetMySQLTableDataGridView.MultiHeaderColumnList.Add(new MultiHeaderColumn(string.Empty, colIdx, colIdx));
        }
        else
        {
          TargetMySQLTableDataGridView.MultiHeaderColumnList[colIdx].HeaderText = string.Empty;
        }

        TargetMySQLTableDataGridView.MultiHeaderColumnList[colIdx].BackgroundColor = Color.OrangeRed;
        MySQLDataColumn toCol = TargetMySQLDataTable.Columns[colIdx] as MySQLDataColumn;
        toCol.MappedDataColName = null;
        if (colIdx < SourceExcelDataDataGridView.Columns.Count)
        {
          DataGridViewCellStyle newStyle = new DataGridViewCellStyle(SourceExcelDataDataGridView.Columns[colIdx].HeaderCell.Style);
          newStyle.SelectionBackColor = newStyle.BackColor = SystemColors.Control;
          SourceExcelDataDataGridView.Columns[colIdx].HeaderCell.Style = newStyle;
        }
      }

      if (CurrentColumnMapping != null && !onlyGrids)
      {
        CurrentColumnMapping.ClearMappings();
      }

      TargetMySQLTableDataGridView.Refresh();
      SourceExcelDataDataGridView.Refresh();
      StoreMappingButton.Enabled = false;
    }

    /// <summary>
    /// Event delegate method fired when the <see cref="ContentAreaPanel"/> receives a drop operation.
    /// </summary>
    /// <param name="sender">Sender object.</param>
    /// <param name="e">Event arguments.</param>
    private void ContentAreaPanel_DragDrop(object sender, DragEventArgs e)
    {
      if (e.Effect == DragDropEffects.Move && e.Data.GetDataPresent(typeof(System.Int32)))
      {
        if (_gridColumnIndexToDrag > -1)
        {
          PerformManualSingleColumnMapping(-1, _gridColumnIndexToDrag, null);
        }
      }
    }

    /// <summary>
    /// Event delegate method fired when an element is dragged over the <see cref="ContentAreaPanel"/>.
    /// </summary>
    /// <param name="sender">Sender object.</param>
    /// <param name="e">Event arguments.</param>
    private void ContentAreaPanel_DragOver(object sender, DragEventArgs e)
    {
      //// Determine whether data exists in the drop data. If not, then the drop effect reflects that the drop cannot occur.
      if (!e.Data.GetDataPresent(typeof(System.Int32)))
      {
        e.Effect = DragDropEffects.None;
        return;
      }

      if ((e.AllowedEffect & DragDropEffects.Move) == DragDropEffects.Move)
      {
        e.Effect = DragDropEffects.Move;
        _gridTargetTableColumnIndexToDrop = -1;
      }
    }

    /// <summary>
    /// Event delegate method fired while an element is being dragged over the <see cref="ContentAreaPanel"/>.
    /// </summary>
    /// <param name="sender">Sender object.</param>
    /// <param name="e">Event arguments.</param>
    private void ContentAreaPanel_QueryContinueDrag(object sender, QueryContinueDragEventArgs e)
    {
      // Cancel the drag if the mouse moves off the form. The screenOffset takes into account any desktop bands that may be at the top or left side of the screen.
      if (((Control.MousePosition.X - _screenOffset.X) < this.DesktopBounds.Left) ||
          ((Control.MousePosition.X - _screenOffset.X) > this.DesktopBounds.Right) ||
          ((Control.MousePosition.Y - _screenOffset.Y) < this.DesktopBounds.Top) ||
          ((Control.MousePosition.Y - _screenOffset.Y) > this.DesktopBounds.Bottom))
      {
        e.Action = DragAction.Cancel;
        return;
      }
    }

    /// <summary>
    /// Creates the mapping of columns between the From and To tables for an automatic mapping.
    /// The mapping is done matching first column names regardless of their positions, otherwise map columns in the same positions if their data types match.
    /// </summary>
    /// <returns>A mapping object containing all column mappings.</returns>
    private MySQLColumnMapping CreateColumnMappingForAutomatic()
    {
      MySQLColumnMapping autoMapping = new MySQLColumnMapping("Automatic", SourceMySQLPreviewDataTable.GetColumnNamesArray(true), TargetMySQLDataTable.GetColumnNamesArray());
      autoMapping.SchemaName = WBConnection.Schema;
      autoMapping.TableName = TargetMySQLDataTable.TableName;
      autoMapping.ConnectionName = WBConnection.Name;
      autoMapping.Port = WBConnection.Port;
      int autoMappedColumns = 0;

      //// Attempt to auto-map using toColumn names regardless of positioning if the data types match
      if (FirstRowHeadersCheckBox.Checked)
      {
        for (int targetColumnIndex = 0; targetColumnIndex < TargetMySQLDataTable.Columns.Count; targetColumnIndex++)
        {
          string targetColumnName = TargetMySQLDataTable.Columns[targetColumnIndex].ColumnName;
          int sourceColumnIndex = SourceMySQLPreviewDataTable.GetColumnIndex(targetColumnName, true, false);
          if (sourceColumnIndex >= 0)
          {
            MySQLDataColumn sourceColumn = SourceMySQLPreviewDataTable.GetColumnAtIndex(sourceColumnIndex);
            MySQLDataColumn targetColumn = TargetMySQLDataTable.GetColumnAtIndex(targetColumnIndex);
            if (DataTypeUtilities.Type1FitsIntoType2(sourceColumn.StrippedMySQLDataType, targetColumn.StrippedMySQLDataType))
            {
              autoMapping.MappedSourceIndexes[targetColumnIndex] = sourceColumnIndex;
              autoMappedColumns++;
            }
          }
        }
      }

      //// Auto-map 1-1 if just data types match
      if (autoMappedColumns == 0)
      {
        autoMapping.ClearMappings();
        for (int columnIndex = 0; columnIndex < TargetMySQLDataTable.Columns.Count; columnIndex++)
        {
          if (columnIndex >= MaxMappingColumnsQuantity)
          {
            break;
          }

          MySQLDataColumn sourceColumn = SourceMySQLPreviewDataTable.GetColumnAtIndex(columnIndex);
          MySQLDataColumn targetColumn = TargetMySQLDataTable.GetColumnAtIndex(columnIndex);
          if (DataTypeUtilities.Type1FitsIntoType2(sourceColumn.StrippedMySQLDataType, targetColumn.StrippedMySQLDataType))
          {
            autoMapping.MappedSourceIndexes[columnIndex] = columnIndex;
            autoMappedColumns++;
          }
        }
      }

      return autoMapping;
    }

    /// <summary>
    /// Initializes the mapping of columns between the From and To tables for a manual user mapping.
    /// </summary>
    /// <returns>A mapping object containing no column mappings to prepare for manual mapping by users.</returns>
    private MySQLColumnMapping CreateColumnMappingForManual()
    {
      MySQLColumnMapping manualMapping;
      if (CurrentColumnMapping == null)
      {
        manualMapping = new MySQLColumnMapping(SourceMySQLPreviewDataTable.GetColumnNamesArray(), TargetMySQLDataTable.GetColumnNamesArray());
        manualMapping.SchemaName = WBConnection.Schema;
        manualMapping.TableName = TargetMySQLDataTable.TableName;
        manualMapping.ConnectionName = WBConnection.Name;
        manualMapping.Port = WBConnection.Port;
      }
      else
      {
        manualMapping = CurrentColumnMapping;
      }

      manualMapping.Name = "Manual";
      return manualMapping;
    }

    /// <summary>
    /// Creates column mappings for currently selected tables based on a stored mapping.
    /// </summary>
    private void CreateColumnMappingForStoredMapping()
    {
      //// Create a copy of the current stored mapping but with no source columns mapped that we will be doing the best matching on
      MySQLColumnMapping matchedMapping = new MySQLColumnMapping(CurrentColumnMapping, SourceMySQLPreviewDataTable.GetColumnNamesArray(true), TargetMySQLDataTable.GetColumnNamesArray());

      //// Check if Target Columns still match with the Target Table, switch mapped indexes if columns changed positions
      ////  and skip target column in stored mapping is not present anymore in Target Table
      for (int storedMappedIdx = 0; storedMappedIdx < CurrentColumnMapping.TargetColumns.Length; storedMappedIdx++)
      {
        //// Get the source index of the stored mapping for the current tartet column, if -1 there was no mapping for the
        //// target column at that position so we skip it.
        int proposedSourceMapping = CurrentColumnMapping.MappedSourceIndexes[storedMappedIdx];
        if (proposedSourceMapping < 0)
        {
          continue;
        }

        //// Check if Target Column in Stored Mapping is found within any of the TargetColumns of the matching mapping.
        //// If not found we should not map so we skip this Target Column.
        string storedMappedColName = CurrentColumnMapping.TargetColumns[storedMappedIdx];
        int targetColumnIndex = matchedMapping.GetTargetColumnIndex(storedMappedColName);
        if (targetColumnIndex < 0)
        {
          continue;
        }

        MySQLDataColumn targetColumn = TargetMySQLDataTable.GetColumnAtIndex(targetColumnIndex);

        //// Check if mapped source column from Stored Mapping matches a Source Column in current "From Table"
        ////  and if its data type matches its corresponding target column's data type, if so we are good to map it
        string mappedSourceColName = CurrentColumnMapping.SourceColumns[proposedSourceMapping];
        int sourceColFoundInFromTableIdx = SourceMySQLPreviewDataTable.GetColumnIndex(mappedSourceColName, true);
        if (sourceColFoundInFromTableIdx >= 0)
        {
          MySQLDataColumn sourceColumn = SourceMySQLPreviewDataTable.GetColumnAtIndex(sourceColFoundInFromTableIdx);
          if (DataTypeUtilities.Type1FitsIntoType2(sourceColumn.StrippedMySQLDataType, targetColumn.StrippedMySQLDataType))
          {
            matchedMapping.MappedSourceIndexes[targetColumnIndex] = sourceColFoundInFromTableIdx;
          }
        }
        //// Since source columns do not match in name and type, try to match the mapped source column's datatype
        ////  with the From column in that source index only if that From Column name is not in any source mapping.
        else if (matchedMapping.MappedSourceIndexes[targetColumnIndex] < 0 && proposedSourceMapping < SourceMySQLPreviewDataTable.Columns.Count)
        {
          string fromTableColName = SourceMySQLPreviewDataTable.GetColumnAtIndex(proposedSourceMapping).DisplayName;
          int fromTableColNameFoundInStoredMappingSourceColumnsIdx = CurrentColumnMapping.GetSourceColumnIndex(fromTableColName);
          if (fromTableColNameFoundInStoredMappingSourceColumnsIdx >= 0
            && fromTableColNameFoundInStoredMappingSourceColumnsIdx != proposedSourceMapping
            && CurrentColumnMapping.GetMappedSourceIndexIndex(fromTableColNameFoundInStoredMappingSourceColumnsIdx) >= 0)
          {
            continue;
          }

          MySQLDataColumn sourceColumn = SourceMySQLPreviewDataTable.GetColumnAtIndex(proposedSourceMapping);
          if (DataTypeUtilities.Type1FitsIntoType2(sourceColumn.StrippedMySQLDataType, targetColumn.StrippedMySQLDataType))
          {
            matchedMapping.MappedSourceIndexes[targetColumnIndex] = proposedSourceMapping;
          }
        }
      }

      CurrentColumnMapping = matchedMapping;
      ApplySelectedStoredColumnMapping();
    }

    /// <summary>
    /// Event delegate method fired when a <see cref="DataGridView"/> control gives feedback regarding a drag&drop operation.
    /// </summary>
    /// <param name="sender">Sender object.</param>
    /// <param name="e">Event arguments.</param>
    private void DataGridView_GiveFeedback(object sender, GiveFeedbackEventArgs e)
    {
      bool feedBackFromGrid = (sender as DataGridView).Name == "grdFromExcelData";

      e.UseDefaultCursors = false;
      switch (e.Effect)
      {
        case DragDropEffects.Link:
          Cursor.Current = feedBackFromGrid ? _droppableCursor : Cursors.No;
          break;

        case DragDropEffects.Move:
          Cursor.Current = feedBackFromGrid ? Cursors.No : (_gridTargetTableColumnIndexToDrop >= 0 ? _droppableCursor : _trashCursor);
          break;

        case DragDropEffects.None:
          Cursor.Current = feedBackFromGrid ? _draggingCursor : _trashCursor;
          break;

        default:
          Cursor.Current = Cursors.Default;
          break;
      }
    }

    /// <summary>
    /// Event delegate method fired when a mouse down operation occurs on a <see cref="DataGridView"/> control.
    /// </summary>
    /// <param name="sender">Sender object.</param>
    /// <param name="e">Event arguments.</param>
    private void DataGridView_MouseDown(object sender, MouseEventArgs e)
    {
      DataGridView gridObject = sender as DataGridView;
      DataGridView.HitTestInfo info = gridObject.HitTest(e.X, e.Y);
      _gridColumnClicked = -1;
      if (e.Button == MouseButtons.Left)
      {
        _gridColumnIndexToDrag = info.ColumnIndex;
        if (_gridColumnIndexToDrag >= 0)
        {
          //// Remember the point where the mouse down occurred. The DragSize indicates the size that the mouse can move before a drag event should be started.
          Size dragSize = SystemInformation.DragSize;

          //// Create a rectangle using the DragSize, with the mouse position being at the center of the rectangle.
          _dragBoxFromMouseDown = new Rectangle(new Point(e.X - (dragSize.Width / 2), e.Y - (dragSize.Height / 2)), dragSize);
        }
        else
        {
          //// Reset the rectangle if the mouse is not over an item.
          _dragBoxFromMouseDown = Rectangle.Empty;
        }
      }
      else if (e.Button == MouseButtons.Right)
      {
        _gridColumnClicked = info.ColumnIndex;
      }
    }

    /// <summary>
    /// Event delegate method fired when a mouse move operation occurs on a <see cref="DataGridView"/> control.
    /// </summary>
    /// <param name="sender">Sender object.</param>
    /// <param name="e">Event arguments.</param>
    private void DataGridView_MouseMove(object sender, MouseEventArgs e)
    {
      if ((e.Button & MouseButtons.Left) == MouseButtons.Left)
      {
        //// If the mouse moves outside the rectangle, start the drag.
        if (_dragBoxFromMouseDown != Rectangle.Empty && !_dragBoxFromMouseDown.Contains(e.X, e.Y))
        {
          DataGridView gridObject = sender as DataGridView;

          //// The screenOffset is used to account for any desktop bands that may be at the top or left side of the screen when determining when to cancel the drag drop operation.
          _screenOffset = SystemInformation.WorkingArea.Location;

          //// Proceed with the drag-and-drop, passing in the list item.
          switch (gridObject.Name)
          {
            case "grdFromExcelData":
              gridObject.DoDragDrop(_gridColumnIndexToDrag, DragDropEffects.Link);
              break;

            case "grdToMySQLTable":
              if (_gridColumnIndexToDrag >= 0 && CurrentColumnMapping != null && CurrentColumnMapping.MappedSourceIndexes[_gridColumnIndexToDrag] >= 0)
              {
                gridObject.DoDragDrop(_gridColumnIndexToDrag, DragDropEffects.Move);
              }

              break;
          }
        }
      }
    }

    /// <summary>
    /// Event delegate method fired when a mouse up operation occurs on a <see cref="DataGridView"/> control.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void DataGridView_MouseUp(object sender, MouseEventArgs e)
    {
      //// Reset the drag rectangle when the mouse button is raised.
      _dragBoxFromMouseDown = Rectangle.Empty;
    }

    /// <summary>
    /// Event delegate method fired while an element is being dragged over a <see cref="DataGridView"/> control.
    /// </summary>
    /// <param name="sender">Sender object.</param>
    /// <param name="e">Event arguments.</param>
    private void DataGridView_QueryContinueDrag(object sender, QueryContinueDragEventArgs e)
    {
      //// Cancel the drag if the mouse moves off the form. The screenOffset takes into account any desktop bands that may be at the top or left side of the screen.
      if (((Control.MousePosition.X - _screenOffset.X) < this.DesktopBounds.Left) ||
          ((Control.MousePosition.X - _screenOffset.X) > this.DesktopBounds.Right) ||
          ((Control.MousePosition.Y - _screenOffset.Y) < this.DesktopBounds.Top) ||
          ((Control.MousePosition.Y - _screenOffset.Y) > this.DesktopBounds.Bottom))
      {
        e.Action = DragAction.Cancel;
        return;
      }
    }

    /// <summary>
    /// Event delegate method fired when the <see cref="FirstRowHeadersCheckBox"/> checkbox is checked.
    /// </summary>
    /// <param name="sender">Sender object.</param>
    /// <param name="e">Event arguments.</param>
    private void FirstRowHeadersCheckBox_CheckedChanged(object sender, EventArgs e)
    {
      if (SourceExcelDataDataGridView.Rows.Count == 0)
      {
        return;
      }

      bool firstRowColNames = FirstRowHeadersCheckBox.Checked;

      //// Flag the property in the "From" table
      SourceMySQLPreviewDataTable.FirstRowIsHeaders = firstRowColNames;

      //// Refresh the "From"/"Source" Grid and "From"/"Source" toColumn names in the current mapping
      SourceExcelDataDataGridView.CurrentCell = null;
      for (int colIdx = 0; colIdx < SourceExcelDataDataGridView.Columns.Count; colIdx++)
      {
        DataGridViewColumn gridCol = SourceExcelDataDataGridView.Columns[colIdx];
        gridCol.HeaderText = firstRowColNames ? SourceExcelDataDataGridView.Rows[0].Cells[gridCol.Index].Value.ToString() : SourceMySQLPreviewDataTable.Columns[gridCol.Index].ColumnName;
        if (CurrentColumnMapping != null)
        {
          CurrentColumnMapping.SourceColumns[colIdx] = gridCol.HeaderText;
        }
      }

      SourceExcelDataDataGridView.Rows[0].Visible = !firstRowColNames;
      if (!(FirstRowHeadersCheckBox.Checked && SourceExcelDataDataGridView.Rows.Count < 2))
      {
        SourceExcelDataDataGridView.FirstDisplayedScrollingRowIndex = FirstRowHeadersCheckBox.Checked ? 1 : 0;
      }

      //// Refresh the mapped columns in the "To" Grid
      for (int colIdx = 0; colIdx < TargetMySQLTableDataGridView.MultiHeaderColumnList.Count; colIdx++)
      {
        MultiHeaderColumn multiHeaderCol = TargetMySQLTableDataGridView.MultiHeaderColumnList[colIdx];
        int mappedSourceIndex = CurrentColumnMapping.MappedSourceIndexes[colIdx];
        if (!string.IsNullOrEmpty(multiHeaderCol.HeaderText) && mappedSourceIndex >= 0)
        {
          multiHeaderCol.HeaderText = SourceExcelDataDataGridView.Columns[mappedSourceIndex].HeaderText;
        }
      }

      TargetMySQLTableDataGridView.Refresh();

      //// Re-do the Currently Selected mapping (unless we are on Manual) since now columns may match
      if (CurrentColumnMapping != null && CurrentColumnMapping.Name != "Manual")
      {
        MappingMethodComboBox_SelectedIndexChanged(MappingMethodComboBox, EventArgs.Empty);
      }
    }

    /// <summary>
    /// Initilizes the grid containing the source data contained in the From table.
    /// </summary>
    /// <param name="schemaName">The name of the schema containing the From table.</param>
    /// <param name="fromTableName">The name of the source DB object in Excel.</param>
    private void InitializeFromTableGrid(string schemaName, string fromTableName)
    {
      SourceMySQLPreviewDataTable = new MySQLDataTable(
        schemaName,
        fromTableName,
        false,
        Properties.Settings.Default.AppendUseFormattedValues,
        false,
        true,
        false,
        false,
        false,
        WBConnection);

      int previewRowsQty = Math.Min(this.AppendDataRange.Rows.Count, Settings.Default.AppendLimitPreviewRowsQuantity);
      Excel.Range previewRange = this.AppendDataRange.get_Resize(previewRowsQty, this.AppendDataRange.Columns.Count);
      SourceMySQLPreviewDataTable.SetData(previewRange, true, false);
      SourceExcelDataDataGridView.DataSource = SourceMySQLPreviewDataTable;
      foreach (DataGridViewColumn gridCol in SourceExcelDataDataGridView.Columns)
      {
        gridCol.SortMode = DataGridViewColumnSortMode.NotSortable;
      }

      SourceExcelDataDataGridView.SelectionMode = DataGridViewSelectionMode.FullColumnSelect;
      FirstRowHeadersCheckBox_CheckedChanged(FirstRowHeadersCheckBox, EventArgs.Empty);
    }

    /// <summary>
    /// Initilizes the grid containing preview data contained in the target table.
    /// </summary>
    /// <param name="importDBObject">The name of the target DB object in Excel.</param>
    private void InitializeToTableGrid(DBObject importDBObject)
    {
      TargetMySQLDataTable = new MySQLDataTable(importDBObject.Name, true, false, WBConnection);
      DataTable dt = MySQLDataUtilities.GetDataFromTableOrView(WBConnection, importDBObject, null, 0, 10);
      foreach (DataRow dr in dt.Rows)
      {
        object[] rowValues = dr.ItemArray;
        for (int colIdx = 0; colIdx < dt.Columns.Count; colIdx++)
        {
          rowValues[colIdx] = DataTypeUtilities.GetImportingValueForDateType(rowValues[colIdx]);
        }

        TargetMySQLDataTable.LoadDataRow(rowValues, true);
      }

      long totalRowsCount = MySQLDataUtilities.GetRowsCountFromTableOrView(WBConnection, importDBObject);
      TargetMySQLTableDataGridView.DataSource = TargetMySQLDataTable;
      foreach (DataGridViewColumn gridCol in TargetMySQLTableDataGridView.Columns)
      {
        gridCol.SortMode = DataGridViewColumnSortMode.NotSortable;
      }

      TargetMySQLTableDataGridView.SelectionMode = DataGridViewSelectionMode.FullColumnSelect;
    }

    /// <summary>
    /// Event delegate method fired when the <see cref="MappingMethodComboBox"/> combobox's selected index changes.
    /// </summary>
    /// <param name="sender">Sender object.</param>
    /// <param name="e">Event arguments.</param>
    private void MappingMethodComboBox_SelectedIndexChanged(object sender, EventArgs e)
    {
      switch (MappingMethodComboBox.SelectedIndex)
      {
        case 0:
          CurrentColumnMapping = CreateColumnMappingForAutomatic();
          ApplySelectedStoredColumnMapping();
          break;

        case 1:
          CurrentColumnMapping = CreateColumnMappingForManual();
          break;

        default:
          CurrentColumnMapping = new MySQLColumnMapping(StoredColumnMappingsList[MappingMethodComboBox.SelectedIndex - 2]);
          CreateColumnMappingForStoredMapping();
          break;
      }
    }

    /// <summary>
    /// Maps a single column between the From and the To tables.
    /// </summary>
    /// <param name="fromColumnIndex">The index of the mapping column in the From table.</param>
    /// <param name="toColumnIndex">The index of the mapping column in the To table.</param>
    /// <param name="mappedColName">The name of the column in the To table where data is being mapped to.</param>
    private void PerformManualSingleColumnMapping(int fromColumnIndex, int toColumnIndex, string mappedColName)
    {
      if (CurrentColumnMapping.Name == "Automatic")
      {
        MappingMethodComboBox.SelectedIndex = 1;
      }

      ApplySingleMapping(fromColumnIndex, toColumnIndex, mappedColName);

      //// Refresh Grids
      TargetMySQLTableDataGridView.Refresh();
      SourceExcelDataDataGridView.Refresh();
      StoreMappingButton.Enabled = CurrentColumnMapping.MappedQuantity > 0;
    }

    /// <summary>
    /// Refreshes the mapping method combo with all column mappings (static and user's stored).
    /// </summary>
    private void RefreshMappingMethodCombo()
    {
      MappingMethodComboBox.Items.Clear();
      MappingMethodComboBox.Items.Add("Automatic");
      MappingMethodComboBox.Items.Add("Manual");

      if (StoredColumnMappingsList != null)
      {
        foreach (MySQLColumnMapping mapping in StoredColumnMappingsList)
        {
          MappingMethodComboBox.Items.Add(string.Format("{0} ({1}.{2})", mapping.Name, mapping.SchemaName, mapping.TableName));
        }
      }

      MappingMethodComboBox.SelectedIndex = -1;
    }

    /// <summary>
    /// Event delegate method fired when the <see cref="RemoveColumnMappingToolStripMenuItem"/> menu item is clicked.
    /// </summary>
    /// <param name="sender">Sender object.</param>
    /// <param name="e">Event arguments.</param>
    private void RemoveColumnMappingToolStripMenuItem_Click(object sender, EventArgs e)
    {
      if (_gridColumnClicked > -1)
      {
        PerformManualSingleColumnMapping(-1, _gridColumnClicked, null);
      }
    }

    /// <summary>
    /// Selects the first stored mapping from the user's stored mappings that matches with the target table.
    /// </summary>
    /// <returns><c>true</c> if a stored mapping was found that matches the target table, <c>false</c> otherwise.</returns>
    private bool SelectStoredMappingForTargetTable()
    {
      bool appliedStoredMapping = false;

      if (!Settings.Default.AppendReloadColumnMapping)
      {
        return appliedStoredMapping;
      }

      for (int mappingIdx = 0; mappingIdx < StoredColumnMappingsList.Count; mappingIdx++)
      {
        MySQLColumnMapping mapping = StoredColumnMappingsList[mappingIdx];
        if (mapping.TableName == TargetMySQLDataTable.TableName && mapping.AllColumnsMatch(TargetMySQLDataTable, true))
        {
          MappingMethodComboBox.SelectedIndex = mappingIdx + 2;
          appliedStoredMapping = true;
          break;
        }
      }

      return appliedStoredMapping;
    }

    /// <summary>
    /// Event delegate method fired when the <see cref="SourceExcelDataDataGridView"/> data binding is complete.
    /// </summary>
    /// <param name="sender">Sender object.</param>
    /// <param name="e">Event arguments.</param>
    private void SourceExcelDataDataGridView_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
    {
      SourceExcelDataDataGridView.ClearSelection();
    }

    /// <summary>
    /// Saves a column mapping object in file.
    /// </summary>
    /// <param name="mapping">Column mapping object to save.</param>
    /// <returns><c>true</c> if the mapping object did not exist in file already, <c>false</c> otherwise.</returns>
    private bool StoreColumnMappingInFile(MySQLColumnMapping mapping)
    {
      bool result = false;

      if (!StoredColumnMappingsList.Contains(mapping))
      {
        MySQLColumnMappingList userList = new MySQLColumnMappingList();
        result = userList.Add(mapping);
        if (result)
        {
          RefreshMappingMethodCombo();
        }
      }

      return result;
    }

    /// <summary>
    /// Stores the current column mapping in file, automatically proposes a mapping name given the target To table's name.
    /// </summary>
    /// <param name="showNewColumnMappingDialog">Flag indicating whether a dialog asking the user to confirm or change the proposed mapping name is shown.</param>
    private void StoreCurrentColumnMapping(bool showNewColumnMappingDialog)
    {
      int numericSuffix = 1;
      string proposedMappingName = string.Empty;
      do
      {
        proposedMappingName = string.Format("{0}Mapping{1}", TargetMySQLDataTable.TableName, numericSuffix > 1 ? numericSuffix.ToString() : string.Empty);
        numericSuffix++;
      }
      while (StoredColumnMappingsList.Any(mapping => mapping.Name == proposedMappingName));

      if (showNewColumnMappingDialog)
      {
        DialogResult dr;
        using (AppendNewColumnMappingDialog newColumnMappingDialog = new AppendNewColumnMappingDialog(proposedMappingName))
        {
          dr = newColumnMappingDialog.ShowDialog();
          proposedMappingName = newColumnMappingDialog.ColumnMappingName;
        }

        if (dr == DialogResult.Cancel)
        {
          return;
        }
      }

      //// Initialize connection and DBObject information
      CurrentColumnMapping.Name = proposedMappingName;
      CurrentColumnMapping.ConnectionName = WBConnection.Name;
      CurrentColumnMapping.Port = WBConnection.Port;
      CurrentColumnMapping.SchemaName = WBConnection.Schema;
      CurrentColumnMapping.TableName = TargetMySQLDataTable.TableName;

      StoreColumnMappingInFile(CurrentColumnMapping);
    }

    /// <summary>
    /// Event delegate method fired when the <see cref="StoreMappingButton"/> button is clicked.
    /// </summary>
    /// <param name="sender">Sender object.</param>
    /// <param name="e">Event arguments.</param>
    private void StoreMappingButton_Click(object sender, EventArgs e)
    {
      StoreCurrentColumnMapping(true);
    }

    /// <summary>
    /// Swaps the mappings between 2 source columns.
    /// </summary>
    /// <param name="mappingSourceIndex1">Index of the first column mapping.</param>
    /// <param name="mappingSourceIndex2">Index of the second column mapping.</param>
    private void SwapMappings(int mappingSourceIndex1, int mappingSourceIndex2)
    {
      int mappingsCount = CurrentColumnMapping != null ? CurrentColumnMapping.MappedSourceIndexes.Length : 0;
      if (mappingsCount == 0 || mappingSourceIndex1 < 0 || mappingSourceIndex1 >= mappingsCount || mappingSourceIndex2 < 0 || mappingSourceIndex2 >= mappingsCount)
      {
        return;
      }

      string mapping1ColName = TargetMySQLTableDataGridView.MultiHeaderColumnList[mappingSourceIndex1].HeaderText;
      int mapping1Index = CurrentColumnMapping.MappedSourceIndexes[mappingSourceIndex1];
      string mapping2ColName = TargetMySQLTableDataGridView.MultiHeaderColumnList[mappingSourceIndex2].HeaderText;
      int mapping2Index = CurrentColumnMapping.MappedSourceIndexes[mappingSourceIndex2];

      ApplySingleMapping(mapping1Index, mappingSourceIndex2, mapping1ColName);
      ApplySingleMapping(mapping2Index, mappingSourceIndex1, mapping2ColName);

      CurrentColumnMapping.MappedSourceIndexes[mappingSourceIndex1] = mapping2Index;
      CurrentColumnMapping.MappedSourceIndexes[mappingSourceIndex2] = mapping1Index;

      //// Refresh Grids
      TargetMySQLTableDataGridView.Refresh();
      SourceExcelDataDataGridView.Refresh();
      StoreMappingButton.Enabled = CurrentColumnMapping.MappedQuantity > 0;
    }

    /// <summary>
    /// Event delegate method fired when the <see cref="TargetMySQLTableDataGridView"/> receives a drop operation.
    /// </summary>
    /// <param name="sender">Sender object.</param>
    /// <param name="e">Event arguments.</param>
    private void TargetMySQLTableDataGridView_DragDrop(object sender, DragEventArgs e)
    {
      //// Ensure that the dragged item is contained in the data.
      if (e.Data.GetDataPresent(typeof(System.Int32)))
      {
        int fromColumnIndex = Convert.ToInt32(e.Data.GetData(typeof(System.Int32)));
        string draggedColumnName = SourceExcelDataDataGridView.Columns[fromColumnIndex].HeaderText;
        if (_gridTargetTableColumnIndexToDrop >= 0)
        {
          switch (e.Effect)
          {
            //// We are mapping a column from the Source Grid to the Target Grid
            case DragDropEffects.Link:
              MySQLDataColumn toCol = TargetMySQLDataTable.GetColumnAtIndex(_gridTargetTableColumnIndexToDrop);
              if (!string.IsNullOrEmpty(toCol.MappedDataColName))
              {
                bool isIdenticalMapping = toCol.MappedDataColName == draggedColumnName;
                DialogResult dr = DialogResult.No;
                if (!isIdenticalMapping)
                {
                  dr = InfoDialog.ShowWarningDialog(Properties.Resources.ColumnMappedOverwriteTitleWarning, Properties.Resources.ColumnMappedOverwriteDetailWarning);
                }

                if (dr == DialogResult.Yes)
                {
                  PerformManualSingleColumnMapping(-1, _gridTargetTableColumnIndexToDrop, null);
                }
                else
                {
                  e.Effect = DragDropEffects.None;
                  return;
                }
              }

              PerformManualSingleColumnMapping(fromColumnIndex, _gridTargetTableColumnIndexToDrop, draggedColumnName);
              break;

            //// We are moving a column mapping from a column on the Target Grid to another column in the same grid
            case DragDropEffects.Move:
              if (CurrentColumnMapping == null)
              {
                return;
              }

              int mappedIndexFromDraggedTargetColumn = CurrentColumnMapping.MappedSourceIndexes[fromColumnIndex];
              int mappedIndexInDropTargetColumn = CurrentColumnMapping.MappedSourceIndexes[_gridTargetTableColumnIndexToDrop];
              if (mappedIndexInDropTargetColumn >= 0)
              {
                bool isIdenticalMapping = mappedIndexInDropTargetColumn == mappedIndexFromDraggedTargetColumn;
                DialogResult dr = DialogResult.No;
                if (!isIdenticalMapping)
                {
                  dr = InfoDialog.ShowWarningDialog(Properties.Resources.ColumnMappedOverwriteTitleWarning, Properties.Resources.ColumnMappedExchangeDetailWarning);
                }

                if (dr == DialogResult.No)
                {
                  e.Effect = DragDropEffects.None;
                  return;
                }
              }

              SwapMappings(fromColumnIndex, _gridTargetTableColumnIndexToDrop);
              break;
          }
        }
      }

      _gridTargetTableColumnIndexToDrop = -1;
    }

    /// <summary>
    /// Event delegate method fired when an element is dragged over the <see cref="TargetMySQLTableDataGridView"/> control.
    /// </summary>
    /// <param name="sender">Sender object.</param>
    /// <param name="e">Event arguments.</param>
    private void TargetMySQLTableDataGridView_DragOver(object sender, DragEventArgs e)
    {
      //// Determine whether string data exists in the drop data. If not, then the drop effect reflects that the drop cannot occur.
      if (!e.Data.GetDataPresent(typeof(System.Int32)))
      {
        e.Effect = DragDropEffects.None;
        _gridTargetTableColumnIndexToDrop = -1;
        return;
      }

      if ((e.AllowedEffect & DragDropEffects.Link) == DragDropEffects.Link)
      {
        e.Effect = DragDropEffects.Link;
        Point clientPoint = TargetMySQLTableDataGridView.PointToClient(new Point(e.X, e.Y));
        DataGridView.HitTestInfo info = TargetMySQLTableDataGridView.HitTest(clientPoint.X, clientPoint.Y);
        _gridTargetTableColumnIndexToDrop = info.ColumnIndex;
      }
      else if ((e.AllowedEffect & DragDropEffects.Move) == DragDropEffects.Move)
      {
        e.Effect = DragDropEffects.Move;
        Point clientPoint = TargetMySQLTableDataGridView.PointToClient(new Point(e.X, e.Y));
        DataGridView.HitTestInfo info = TargetMySQLTableDataGridView.HitTest(clientPoint.X, clientPoint.Y);
        _gridTargetTableColumnIndexToDrop = info.ColumnIndex;
      }
    }
  }
}