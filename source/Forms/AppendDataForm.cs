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
using System.Globalization;
using System.Linq;
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
  /// Provides an interface to append Excel data into an existing MySQL table.
  /// </summary>
  public partial class AppendDataForm : AutoStyleableBaseDialog
  {
    #region Fields

    /// <summary>
    /// The _import database object.
    /// </summary>
    private DbObject _importDbObject;

    /// <summary>
    /// Rectangle used to measure drag and drop operations.
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
    /// The index of the grid column being dragged in a drag and drop operation to map or unmap a column.
    /// </summary>
    private int _gridColumnIndexToDrag;

    /// <summary>
    /// The index of the grid column where a dragged column is being dropped during a drag and drop operation to map a column.
    /// </summary>
    private int _gridTargetTableColumnIndexToDrop;

    /// <summary>
    /// The reference point used in drag and drop operations.
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
    /// <param name="importDbObject">The database object to which to append data to.</param>
    /// <param name="appendingWorksheetName">The name of the worksheet holding the appending data.</param>
    public AppendDataForm(MySqlWorkbenchConnection wbConnection, Excel.Range appendDataRange, DbObject importDbObject, string appendingWorksheetName)
    {
      AppendDataRange = appendDataRange;
      ColumnsMappingInFileList = new MySqlColumnMappingList();
      SourceMySqlPreviewDataTable = null;
      TargetMySqlPreviewDataTable = null;
      WbConnection = wbConnection;

      _dragBoxFromMouseDown = Rectangle.Empty;
      _draggingCursor = new Bitmap(Resources.MySQLforExcel_Cursor_Dragging_32x32).CreateCursor(3, 3);
      _droppableCursor = new Bitmap(Resources.MySQLforExcel_Cursor_Dropable_32x32).CreateCursor(3, 3);
      _gridColumnClicked = -1;
      _gridColumnIndexToDrag = -1;
      _gridTargetTableColumnIndexToDrop = -1;
      _trashCursor = new Bitmap(Resources.MySQLforExcel_Cursor_Trash_32x32).CreateCursor(3, 3);

      InitializeComponent();

      SourceExcelDataDataGridView.EnableHeadersVisualStyles = false;
      _importDbObject = importDbObject;

      InitializeSourceTableGrid();
      InitializeTargetTableGrid();

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
    public MySqlColumnMappingList ColumnsMappingInFileList { get; private set; }

    /// <summary>
    /// Gets the column mapping currently being used in the append session.
    /// </summary>
    public MySqlColumnMapping CurrentColumnMapping { get; private set; }

    /// <summary>
    /// Gets the maximum number of columns that can be mapped based on the maximum number of columns between the source and the target tables.
    /// </summary>
    public int MaxMappingColumnsQuantity { get; private set; }

    /// <summary>
    /// Gets the table containing a small preview subset of Excel data to append to a MySQL Server table.
    /// </summary>
    public MySqlDataTable SourceMySqlPreviewDataTable { get; private set; }

    /// <summary>
    /// Gets a list of column mappings for the current user.
    /// </summary>
    public List<MySqlColumnMapping> StoredColumnMappingsList
    {
      get
      {
        return ColumnsMappingInFileList.UserColumnMappingsList;
      }
    }

    /// <summary>
    /// Gets the table containing a small preview subset of the MySQL Server table where data is going to be appended to.
    /// </summary>
    public MySqlDataTable TargetMySqlPreviewDataTable { get; private set; }

    /// <summary>
    /// Gets or sets the text associated with this control.
    /// </summary>
    public override sealed string Text
    {
      get
      {
        return base.Text;
      }

      set
      {
        base.Text = value;
      }
    }

    /// <summary>
    /// Gets the connection to a MySQL server instance selected by users.
    /// </summary>
    public MySqlWorkbenchConnection WbConnection { get; private set; }

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
        if (!optionsDialog.ParentFormRequiresRefresh)
        {
          return;
        }

        InitializeSourceTableGrid();
        for (int targetColumnIndex = 0; targetColumnIndex < CurrentColumnMapping.MappedSourceIndexes.Length; targetColumnIndex++)
        {
          int mappedSourceColumnIndex = CurrentColumnMapping.MappedSourceIndexes[targetColumnIndex];
          if (mappedSourceColumnIndex < 0)
          {
            continue;
          }

          var sourceColumn = SourceMySqlPreviewDataTable.GetColumnAtIndex(mappedSourceColumnIndex);
          var targetColumn = TargetMySqlPreviewDataTable.GetColumnAtIndex(targetColumnIndex);
          CheckIfSourceDataAgainstMappedTargetColumn(sourceColumn, targetColumn);
        }
      }
    }

    /// <summary>
    /// Event delegate method fired when the <see cref="AppendButton"/> button is clicked.
    /// </summary>
    /// <param name="sender">Sender object.</param>
    /// <param name="e">Event arguments.</param>
    private void AppendButton_Click(object sender, EventArgs e)
    {
      // If not all columns where mapped between the source and target tables ask the user if he still wants to produce with the append operation.
      if (TargetMySqlPreviewDataTable.MappedColumnsQuantity < MaxMappingColumnsQuantity
        && InfoDialog.ShowYesNoDialog(InfoDialog.InfoType.Warning, Resources.ColumnMappingIncompleteTitleWarning, Resources.ColumnMappingIncompleteDetailWarning) == DialogResult.No)
      {
        return;
      }

      Cursor = Cursors.WaitCursor;
      int warningsCount = 0;
      bool errorsFound = false;
      bool warningsFound = false;
      string operationSummary;
      var targetMySqlFinalDataTable = TargetMySqlPreviewDataTable.CloneSchema(true, false);
      targetMySqlFinalDataTable.FirstRowIsHeaders = SourceMySqlPreviewDataTable.FirstRowIsHeaders;
      var mappedIndexes = new List<int>(targetMySqlFinalDataTable.Columns.Count);
      foreach (var sourceColumnIndex in from MySqlDataColumn targetColumn in targetMySqlFinalDataTable.Columns select targetColumn.MappedDataColOrdinal)
      {
        if (sourceColumnIndex < 0)
        {
          mappedIndexes.Add(0);
          continue;
        }

        var sourceColumn = SourceMySqlPreviewDataTable.GetColumnAtIndex(sourceColumnIndex);
        mappedIndexes.Add(sourceColumn == null ? 0 : sourceColumn.RangeColumnIndex);
      }

      bool addDataSuccessful;
      using (var temporaryRange = new TempRange(AppendDataRange, true, true, false, mappedIndexes))
      {
        addDataSuccessful = targetMySqlFinalDataTable.AddExcelData(temporaryRange, true, true);
      }

      if (!addDataSuccessful)
      {
        Cursor = Cursors.Default;
        return;
      }

      var modifiedRowsList = targetMySqlFinalDataTable.PushData(Settings.Default.GlobalSqlQueriesPreviewQueries);
      if (modifiedRowsList == null)
      {
        Cursor = Cursors.Default;
        return;
      }

      StringBuilder operationDetails = new StringBuilder();
      StringBuilder warningDetails = new StringBuilder();
      StringBuilder warningStatementDetails = new StringBuilder();
      if (Settings.Default.GlobalSqlQueriesShowQueriesWithResults)
      {
        operationDetails.AppendFormat(Resources.InsertedExcelDataWithQueryText, targetMySqlFinalDataTable.TableName);
        operationDetails.AddNewLine();
      }

      bool warningDetailHeaderAppended = false;
      string statementsQuantityFormat = new string('0', modifiedRowsList.Count.StringSize());
      string sqlQueriesFormat = "{0:" + statementsQuantityFormat + "}: {1}";
      foreach (var statement in modifiedRowsList.Select(statementRow => statementRow.Statement))
      {
        // Create details text each row inserted in the new table.
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
            operationDetails.Append(Resources.AppendDataRowsInsertionErrorText);
            operationDetails.AddNewLine(2);
            operationDetails.Append(statement.ResultText);
            break;
        }

        if (!errorsFound)
        {
          continue;
        }

        break;
      }

      InfoDialog.InfoType operationsType;
      if (errorsFound)
      {
        operationSummary = string.Format(Resources.AppendDataDetailsDoneErrorText, targetMySqlFinalDataTable.TableName);
        operationsType = InfoDialog.InfoType.Error;
      }
      else
      {
        operationSummary = string.Format(warningsFound ? Resources.AppendDataDetailsDoneWarningsText : Resources.AppendDataDetailsDoneSuccessText, targetMySqlFinalDataTable.TableName);
        operationsType = warningsFound ? InfoDialog.InfoType.Warning : InfoDialog.InfoType.Success;
        int appendedCount = modifiedRowsList.GetResultsCount(MySqlStatement.SqlStatementType.Insert);
        if (warningsFound)
        {
          operationDetails.AddNewLine(2, true);
          operationDetails.AppendFormat(Resources.AppendDataRowsAppendedWithWarningsText, appendedCount, warningsCount);
          operationDetails.AddNewLine();
          if (warningStatementDetails.Length > 0)
          {
            operationDetails.Append(warningStatementDetails);
            operationDetails.AddNewLine();
          }

          operationDetails.Append(warningDetails);
        }
        else
        {
          operationDetails.AddNewLine(2, true);
          operationDetails.AppendFormat(Resources.AppendDataRowsAppendedSuccessfullyText, appendedCount);
        }
      }

      Cursor = Cursors.Default;
      MiscUtilities.ShowCustomizedInfoDialog(operationsType, operationSummary, operationDetails.ToString(), false);
      operationDetails.Clear();
      warningDetails.Clear();
      warningStatementDetails.Clear();
      if (errorsFound)
      {
        return;
      }

      if (Settings.Default.AppendAutoStoreColumnMapping
          && !StoredColumnMappingsList.Exists(mapping => mapping.ConnectionName == WbConnection.Name && mapping.SchemaName == WbConnection.Schema && mapping.TableName == targetMySqlFinalDataTable.TableName))
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
        return;
      }

      bool columnHasMapping = AppendContextMenu.SourceControl.Name == TargetMySQLTableDataGridView.Name
        ? CurrentColumnMapping.MappedSourceIndexes[_gridColumnClicked] >= 0
        : CurrentColumnMapping.MappedSourceIndexes.Contains(_gridColumnClicked);
      AppendContextMenu.Items["RemoveColumnMappingToolStripMenuItem"].Visible = columnHasMapping;
      AppendContextMenu.Items["ClearAllMappingsToolStripMenuItem"].Visible = CurrentColumnMapping.MappedQuantity > 0;
    }

    /// <summary>
    /// Applies the column mapping the users selects from a list of stored mappings.
    /// </summary>
    private void ApplySelectedStoredColumnMapping()
    {
      if (CurrentColumnMapping == null)
      {
        return;
      }

      ClearMappings(true);
      for (int mappedIdx = 0; mappedIdx < CurrentColumnMapping.MappedSourceIndexes.Length; mappedIdx++)
      {
        int currentMappedSourceIndex = CurrentColumnMapping.MappedSourceIndexes[mappedIdx];
        string currentMappedColName = currentMappedSourceIndex >= 0 ? CurrentColumnMapping.SourceColumns[currentMappedSourceIndex] : null;
        ApplySingleMapping(currentMappedSourceIndex, mappedIdx, currentMappedColName);
      }

      TargetMySQLTableDataGridView.Refresh();
      SourceExcelDataDataGridView.Refresh();
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

      // Change text and style of target table column
      MultiHeaderColumn multiHeaderCol = TargetMySQLTableDataGridView.MultiHeaderColumnList[targetColumnIndex];
      multiHeaderCol.HeaderText = mapping ? mappedColName : string.Empty;
      multiHeaderCol.BackgroundColor = mapping ? Color.LightGreen : Color.OrangeRed;

      // Change style of source table column being mapped or unmapped
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

      // Store the actual mapping
      MySqlDataColumn sourceColumn = mapping ? SourceMySqlPreviewDataTable.GetColumnAtIndex(sourceColumnIndex) : null;
      MySqlDataColumn targetColumn = TargetMySqlPreviewDataTable.GetColumnAtIndex(targetColumnIndex);
      targetColumn.MappedDataColName = mapping ? sourceColumn.ColumnName : null;
      targetColumn.MappedDataColOrdinal = mapping ? sourceColumnIndex : -1;

      CurrentColumnMapping.MappedSourceIndexes[targetColumnIndex] = sourceColumnIndex;
      if (mapping)
      {
        CheckIfSourceDataAgainstMappedTargetColumn(sourceColumn, targetColumn);
      }
      else
      {
        ClearSourceColumnVisualWarnings(previouslyMappedFromIndex);
      }
    }

    /// <summary>
    /// Checks if the source data is suitable for the target's column data type to raise visual warnings.
    /// </summary>
    /// <param name="sourceColumn">Source data column.</param>
    /// <param name="targetColumn">Target column.</param>
    private void CheckIfSourceDataAgainstMappedTargetColumn(MySqlDataColumn sourceColumn, MySqlDataColumn targetColumn)
    {
      if (sourceColumn == null || targetColumn == null)
      {
        return;
      }

      sourceColumn.TestColumnDataTypeAgainstColumnData(targetColumn.MySqlDataType);
      var gridCol = SourceExcelDataDataGridView.Columns[sourceColumn.Ordinal];
      SetGridColumnColor(gridCol, sourceColumn);
      SetGridColumnWarningVisibility(gridCol);
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
    /// Clears all visual warnings related to a source grid column.
    /// </summary>
    /// <param name="sourceColumnIndex">Source column index.</param>
    private void ClearSourceColumnVisualWarnings(int sourceColumnIndex)
    {
      if (sourceColumnIndex < 0)
      {
        return;
      }

      var previousSourceColumn = SourceMySqlPreviewDataTable.GetColumnAtIndex(sourceColumnIndex);
      previousSourceColumn.ClearWarnings();
      var gridCol = SourceExcelDataDataGridView.Columns[sourceColumnIndex];
      SetGridColumnColor(SourceExcelDataDataGridView.Columns[sourceColumnIndex], previousSourceColumn);
      SetGridColumnWarningVisibility(gridCol);
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
        MySqlDataColumn toCol = TargetMySqlPreviewDataTable.Columns[colIdx] as MySqlDataColumn;
        if (toCol != null)
        {
          toCol.MappedDataColName = null;
          toCol.MappedDataColOrdinal = -1;
        }

        if (colIdx >= SourceExcelDataDataGridView.Columns.Count)
        {
          continue;
        }

        DataGridViewCellStyle newStyle = new DataGridViewCellStyle(SourceExcelDataDataGridView.Columns[colIdx].HeaderCell.Style);
        newStyle.SelectionBackColor = newStyle.BackColor = SystemColors.Control;
        SourceExcelDataDataGridView.Columns[colIdx].HeaderCell.Style = newStyle;

        // Clear source column warnings
        var previousSourceColumn = SourceMySqlPreviewDataTable.GetColumnAtIndex(colIdx);
        previousSourceColumn.ClearWarnings();
        SetGridColumnColor(SourceExcelDataDataGridView.Columns[colIdx], previousSourceColumn);
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
    /// Event delegate method fired when the ContentAreaPanel receives a drop operation.
    /// </summary>
    /// <param name="sender">Sender object.</param>
    /// <param name="e">Event arguments.</param>
    private void ContentAreaPanel_DragDrop(object sender, DragEventArgs e)
    {
      if (e.Effect != DragDropEffects.Move || !e.Data.GetDataPresent(typeof(Int32)))
      {
        return;
      }

      if (_gridColumnIndexToDrag > -1)
      {
        PerformManualSingleColumnMapping(-1, _gridColumnIndexToDrag, null);
      }
    }

    /// <summary>
    /// Event delegate method fired when an element is dragged over the ContentAreaPanel.
    /// </summary>
    /// <param name="sender">Sender object.</param>
    /// <param name="e">Event arguments.</param>
    private void ContentAreaPanel_DragOver(object sender, DragEventArgs e)
    {
      // Determine whether data exists in the drop data. If not, then the drop effect reflects that the drop cannot occur.
      if (!e.Data.GetDataPresent(typeof(Int32)))
      {
        e.Effect = DragDropEffects.None;
        return;
      }

      if ((e.AllowedEffect & DragDropEffects.Move) != DragDropEffects.Move)
      {
        return;
      }

      e.Effect = DragDropEffects.Move;
      _gridTargetTableColumnIndexToDrop = -1;
    }

    /// <summary>
    /// Event delegate method fired while an element is being dragged over the ContentAreaPanel.
    /// </summary>
    /// <param name="sender">Sender object.</param>
    /// <param name="e">Event arguments.</param>
    private void ContentAreaPanel_QueryContinueDrag(object sender, QueryContinueDragEventArgs e)
    {
      // Cancel the drag if the mouse moves off the form. The screenOffset takes into account any desktop bands that may be at the top or left side of the screen.
      if (((MousePosition.X - _screenOffset.X) >= DesktopBounds.Left) &&
          ((MousePosition.X - _screenOffset.X) <= DesktopBounds.Right) &&
          ((MousePosition.Y - _screenOffset.Y) >= DesktopBounds.Top) &&
          ((MousePosition.Y - _screenOffset.Y) <= DesktopBounds.Bottom))
      {
        return;
      }

      e.Action = DragAction.Cancel;
    }

    /// <summary>
    /// Creates the mapping of columns between the From and To tables for an automatic mapping.
    /// The mapping is done matching first column names regardless of their positions, otherwise map columns in the same positions if their data types match.
    /// </summary>
    /// <returns>A mapping object containing all column mappings.</returns>
    private MySqlColumnMapping CreateColumnMappingForAutomatic()
    {
      MySqlColumnMapping autoMapping = new MySqlColumnMapping("Automatic", SourceMySqlPreviewDataTable.GetColumnNamesArray(true), TargetMySqlPreviewDataTable.GetColumnNamesArray())
      {
        SchemaName = WbConnection.Schema,
        TableName = TargetMySqlPreviewDataTable.TableName,
        ConnectionName = WbConnection.Name,
        Port = WbConnection.Port
      };
      int autoMappedColumns = 0;

      // Attempt to auto-map using toColumn names regardless of positioning if the data types match
      if (FirstRowHeadersCheckBox.Checked)
      {
        for (int targetColumnIndex = 0; targetColumnIndex < TargetMySqlPreviewDataTable.Columns.Count; targetColumnIndex++)
        {
          string targetColumnName = TargetMySqlPreviewDataTable.Columns[targetColumnIndex].ColumnName;
          int sourceColumnIndex = SourceMySqlPreviewDataTable.GetColumnIndex(targetColumnName, true, false);
          if (sourceColumnIndex < 0)
          {
            continue;
          }

          MySqlDataColumn sourceColumn = SourceMySqlPreviewDataTable.GetColumnAtIndex(sourceColumnIndex);
          MySqlDataColumn targetColumn = TargetMySqlPreviewDataTable.GetColumnAtIndex(targetColumnIndex);
          if (!DataTypeUtilities.Type1FitsIntoType2(sourceColumn.StrippedMySqlDataType, targetColumn.StrippedMySqlDataType))
          {
            continue;
          }

          autoMapping.MappedSourceIndexes[targetColumnIndex] = sourceColumnIndex;
          autoMappedColumns++;
        }
      }

      // Auto-map 1-1 if just data types match
      if (autoMappedColumns != 0)
      {
        return autoMapping;
      }

      autoMapping.ClearMappings();
      for (int columnIndex = 0; columnIndex < TargetMySqlPreviewDataTable.Columns.Count; columnIndex++)
      {
        if (columnIndex >= MaxMappingColumnsQuantity)
        {
          break;
        }

        MySqlDataColumn sourceColumn = SourceMySqlPreviewDataTable.GetColumnAtIndex(columnIndex);
        MySqlDataColumn targetColumn = TargetMySqlPreviewDataTable.GetColumnAtIndex(columnIndex);
        if (!DataTypeUtilities.Type1FitsIntoType2(sourceColumn.StrippedMySqlDataType, targetColumn.StrippedMySqlDataType))
        {
          continue;
        }

        autoMapping.MappedSourceIndexes[columnIndex] = columnIndex;
        autoMappedColumns++;
      }

      return autoMapping;
    }

    /// <summary>
    /// Initializes the mapping of columns between the From and To tables for a manual user mapping.
    /// </summary>
    /// <returns>A mapping object containing no column mappings to prepare for manual mapping by users.</returns>
    private MySqlColumnMapping CreateColumnMappingForManual()
    {
      MySqlColumnMapping manualMapping;
      if (CurrentColumnMapping == null)
      {
        manualMapping = new MySqlColumnMapping(SourceMySqlPreviewDataTable.GetColumnNamesArray(), TargetMySqlPreviewDataTable.GetColumnNamesArray())
        {
          SchemaName = WbConnection.Schema,
          TableName = TargetMySqlPreviewDataTable.TableName,
          ConnectionName = WbConnection.Name,
          Port = WbConnection.Port
        };
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
      // Create a copy of the current stored mapping but with no source columns mapped that we will be doing the best matching on
      MySqlColumnMapping matchedMapping = new MySqlColumnMapping(CurrentColumnMapping, SourceMySqlPreviewDataTable.GetColumnNamesArray(true), TargetMySqlPreviewDataTable.GetColumnNamesArray());

      // Check if Target Columns still match with the Target Table, switch mapped indexes if columns changed positions
      //  and skip target column in stored mapping is not present anymore in Target Table
      for (int storedMappedIdx = 0; storedMappedIdx < CurrentColumnMapping.TargetColumns.Length; storedMappedIdx++)
      {
        // Get the source index of the stored mapping for the current tartet column, if -1 there was no mapping for the
        // target column at that position so we skip it.
        int proposedSourceMapping = CurrentColumnMapping.MappedSourceIndexes[storedMappedIdx];
        if (proposedSourceMapping < 0)
        {
          continue;
        }

        // Check if Target Column in Stored Mapping is found within any of the TargetColumns of the matching mapping.
        // If not found we should not map so we skip this Target Column.
        string storedMappedColName = CurrentColumnMapping.TargetColumns[storedMappedIdx];
        int targetColumnIndex = matchedMapping.GetTargetColumnIndex(storedMappedColName);
        if (targetColumnIndex < 0)
        {
          continue;
        }

        MySqlDataColumn targetColumn = TargetMySqlPreviewDataTable.GetColumnAtIndex(targetColumnIndex);

        // Check if mapped source column from Stored Mapping matches a Source Column in current "From Table"
        //  and if its data type matches its corresponding target column's data type, if so we are good to map it
        string mappedSourceColName = CurrentColumnMapping.SourceColumns[proposedSourceMapping];
        int sourceColFoundInFromTableIdx = SourceMySqlPreviewDataTable.GetColumnIndex(mappedSourceColName, true);
        if (sourceColFoundInFromTableIdx >= 0)
        {
          MySqlDataColumn sourceColumn = SourceMySqlPreviewDataTable.GetColumnAtIndex(sourceColFoundInFromTableIdx);
          if (DataTypeUtilities.Type1FitsIntoType2(sourceColumn.StrippedMySqlDataType, targetColumn.StrippedMySqlDataType))
          {
            matchedMapping.MappedSourceIndexes[targetColumnIndex] = sourceColFoundInFromTableIdx;
          }
        }
        // Since source columns do not match in name and type, try to match the mapped source column's datatype
        //  with the From column in that source index only if that From Column name is not in any source mapping.
        else if (matchedMapping.MappedSourceIndexes[targetColumnIndex] < 0 && proposedSourceMapping < SourceMySqlPreviewDataTable.Columns.Count)
        {
          string fromTableColName = SourceMySqlPreviewDataTable.GetColumnAtIndex(proposedSourceMapping).DisplayName;
          int fromTableColNameFoundInStoredMappingSourceColumnsIdx = CurrentColumnMapping.GetSourceColumnIndex(fromTableColName);
          if (fromTableColNameFoundInStoredMappingSourceColumnsIdx >= 0
            && fromTableColNameFoundInStoredMappingSourceColumnsIdx != proposedSourceMapping
            && CurrentColumnMapping.GetMappedSourceIndexIndex(fromTableColNameFoundInStoredMappingSourceColumnsIdx) >= 0)
          {
            continue;
          }

          MySqlDataColumn sourceColumn = SourceMySqlPreviewDataTable.GetColumnAtIndex(proposedSourceMapping);
          if (DataTypeUtilities.Type1FitsIntoType2(sourceColumn.StrippedMySqlDataType, targetColumn.StrippedMySqlDataType))
          {
            matchedMapping.MappedSourceIndexes[targetColumnIndex] = proposedSourceMapping;
          }
        }
      }

      CurrentColumnMapping = matchedMapping;
      ApplySelectedStoredColumnMapping();
    }

    /// <summary>
    /// Event delegate method fired when a <see cref="DataGridView"/> control gives feedback regarding a drag and drop operation.
    /// </summary>
    /// <param name="sender">Sender object.</param>
    /// <param name="e">Event arguments.</param>
    private void DataGridView_GiveFeedback(object sender, GiveFeedbackEventArgs e)
    {
      var dataGridView = sender as DataGridView;
      bool feedBackFromGrid = dataGridView != null && dataGridView.Name == "SourceExcelDataDataGridView";

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
      if (gridObject == null)
      {
        return;
      }

      DataGridView.HitTestInfo info = gridObject.HitTest(e.X, e.Y);
      _gridColumnClicked = -1;
      switch (e.Button)
      {
        case MouseButtons.Left:
          _gridColumnIndexToDrag = info.ColumnIndex;
          if (_gridColumnIndexToDrag >= 0)
          {
            // Remember the point where the mouse down occurred. The DragSize indicates the size that the mouse can move before a drag event should be started.
            Size dragSize = SystemInformation.DragSize;

            // Create a rectangle using the DragSize, with the mouse position being at the center of the rectangle.
            _dragBoxFromMouseDown = new Rectangle(new Point(e.X - (dragSize.Width / 2), e.Y - (dragSize.Height / 2)), dragSize);
          }
          else
          {
            // Reset the rectangle if the mouse is not over an item.
            _dragBoxFromMouseDown = Rectangle.Empty;
          }
          break;

        case MouseButtons.Right:
          _gridColumnClicked = info.ColumnIndex;
          break;
      }
    }

    /// <summary>
    /// Event delegate method fired when a mouse move operation occurs on a <see cref="DataGridView"/> control.
    /// </summary>
    /// <param name="sender">Sender object.</param>
    /// <param name="e">Event arguments.</param>
    private void DataGridView_MouseMove(object sender, MouseEventArgs e)
    {
      if ((e.Button & MouseButtons.Left) != MouseButtons.Left)
      {
        return;
      }

      // If the mouse moves outside the rectangle, start the drag.
      if (_dragBoxFromMouseDown == Rectangle.Empty || _dragBoxFromMouseDown.Contains(e.X, e.Y))
      {
        return;
      }

      DataGridView gridObject = sender as DataGridView;
      if (gridObject == null)
      {
        return;
      }

      // The screenOffset is used to account for any desktop bands that may be at the top or left side of the screen when determining when to cancel the drag drop operation.
      _screenOffset = SystemInformation.WorkingArea.Location;

      // Proceed with the drag-and-drop, passing in the list item.
      switch (gridObject.Name)
      {
        case "SourceExcelDataDataGridView":
          gridObject.DoDragDrop(_gridColumnIndexToDrag, DragDropEffects.Link);
          break;

        case "TargetMySQLTableDataGridView":
          if (_gridColumnIndexToDrag >= 0 && CurrentColumnMapping != null && CurrentColumnMapping.MappedSourceIndexes[_gridColumnIndexToDrag] >= 0)
          {
            gridObject.DoDragDrop(_gridColumnIndexToDrag, DragDropEffects.Move);
          }

          break;
      }
    }

    /// <summary>
    /// Event delegate method fired when a mouse up operation occurs on a <see cref="DataGridView"/> control.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void DataGridView_MouseUp(object sender, MouseEventArgs e)
    {
      // Reset the drag rectangle when the mouse button is raised.
      _dragBoxFromMouseDown = Rectangle.Empty;
    }

    /// <summary>
    /// Event delegate method fired while an element is being dragged over a <see cref="DataGridView"/> control.
    /// </summary>
    /// <param name="sender">Sender object.</param>
    /// <param name="e">Event arguments.</param>
    private void DataGridView_QueryContinueDrag(object sender, QueryContinueDragEventArgs e)
    {
      // Cancel the drag if the mouse moves off the form. The screenOffset takes into account any desktop bands that may be at the top or left side of the screen.
      if (((MousePosition.X - _screenOffset.X) >= DesktopBounds.Left) &&
          ((MousePosition.X - _screenOffset.X) <= DesktopBounds.Right) &&
          ((MousePosition.Y - _screenOffset.Y) >= DesktopBounds.Top) &&
          ((MousePosition.Y - _screenOffset.Y) <= DesktopBounds.Bottom))
      {
        return;
      }

      e.Action = DragAction.Cancel;
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

      // Flag the property in the "From" table
      SourceMySqlPreviewDataTable.FirstRowIsHeaders = firstRowColNames;

      // Refresh the "From"/"Source" Grid and "From"/"Source" toColumn names in the current mapping
      SourceExcelDataDataGridView.CurrentCell = null;
      for (int colIdx = 0; colIdx < SourceExcelDataDataGridView.Columns.Count; colIdx++)
      {
        DataGridViewColumn gridCol = SourceExcelDataDataGridView.Columns[colIdx];
        gridCol.HeaderText = firstRowColNames ? SourceExcelDataDataGridView.Rows[0].Cells[gridCol.Index].Value.ToString() : SourceMySqlPreviewDataTable.Columns[gridCol.Index].ColumnName;
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

      // Refresh the mapped columns in the "To" Grid
      for (int colIdx = 0; colIdx < TargetMySQLTableDataGridView.MultiHeaderColumnList.Count; colIdx++)
      {
        MultiHeaderColumn multiHeaderCol = TargetMySQLTableDataGridView.MultiHeaderColumnList[colIdx];
        if (CurrentColumnMapping == null)
        {
          continue;
        }

        int mappedSourceIndex = CurrentColumnMapping.MappedSourceIndexes[colIdx];
        if (!string.IsNullOrEmpty(multiHeaderCol.HeaderText) && mappedSourceIndex >= 0)
        {
          multiHeaderCol.HeaderText = SourceExcelDataDataGridView.Columns[mappedSourceIndex].HeaderText;
        }
      }

      TargetMySQLTableDataGridView.Refresh();

      // Re-do the Currently Selected mapping (unless we are on Manual) since now columns may match
      if (CurrentColumnMapping != null && CurrentColumnMapping.Name != "Manual")
      {
        MappingMethodComboBox_SelectedIndexChanged(MappingMethodComboBox, EventArgs.Empty);
      }
    }

    /// <summary>
    /// Initilizes the grid containing the source data contained in the From table.
    /// </summary>
    private void InitializeSourceTableGrid()
    {
      SourceMySqlPreviewDataTable = new MySqlDataTable(
        WbConnection.Schema,
        _importDbObject.Name,
        false,
        Settings.Default.AppendUseFormattedValues,
        true,
        false,
        false,
        false,
        WbConnection) { IsPreviewTable = true };
      int previewRowsQty = Math.Min(AppendDataRange.Rows.Count, Settings.Default.AppendLimitPreviewRowsQuantity);
      SourceMySqlPreviewDataTable.SetupColumnsWithData(AppendDataRange, true, false, previewRowsQty);
      SourceExcelDataDataGridView.DataSource = SourceMySqlPreviewDataTable;
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
    private void InitializeTargetTableGrid()
    {
      TargetMySqlPreviewDataTable = new MySqlDataTable(
        _importDbObject.Name,
        true,
        false,
        Settings.Default.AppendUseFormattedValues,
        WbConnection);
      DataTable dt = WbConnection.GetDataFromTableOrView(_importDbObject, null, 0, 10);
      foreach (object[] rowValues in from DataRow dr in dt.Rows select dr.ItemArray)
      {
        for (int colIdx = 0; colIdx < dt.Columns.Count; colIdx++)
        {
          rowValues[colIdx] = DataTypeUtilities.GetImportingValueForDateType(rowValues[colIdx]);
        }

        TargetMySqlPreviewDataTable.LoadDataRow(rowValues, true);
      }

      WbConnection.GetRowsCountFromTableOrView(_importDbObject);
      TargetMySQLTableDataGridView.DataSource = TargetMySqlPreviewDataTable;
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
          CurrentColumnMapping = new MySqlColumnMapping(StoredColumnMappingsList[MappingMethodComboBox.SelectedIndex - 2]);
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

      // Refresh Grids
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
        foreach (MySqlColumnMapping mapping in StoredColumnMappingsList)
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
      int targetColumnIndex = AppendContextMenu.SourceControl.Name == TargetMySQLTableDataGridView.Name
        ? _gridColumnClicked
        : CurrentColumnMapping.MappedSourceIndexes.ToList().FindIndex(sourceIndex => sourceIndex == _gridColumnClicked);
      if (targetColumnIndex > -1)
      {
        PerformManualSingleColumnMapping(-1, targetColumnIndex, null);
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
        return false;
      }

      for (int mappingIdx = 0; mappingIdx < StoredColumnMappingsList.Count; mappingIdx++)
      {
        MySqlColumnMapping mapping = StoredColumnMappingsList[mappingIdx];
        if (mapping.TableName != TargetMySqlPreviewDataTable.TableName || !mapping.AllColumnsMatch(TargetMySqlPreviewDataTable, true))
        {
          continue;
        }

        MappingMethodComboBox.SelectedIndex = mappingIdx + 2;
        appliedStoredMapping = true;
        break;
      }

      return appliedStoredMapping;
    }

    /// <summary>
    /// Sets the background color of a grid column bound to a <see cref="MySqlDataColumn"/> depending on its warnings.
    /// </summary>
    /// <param name="gridCol">The <see cref="DataGridViewColumn"/> to color.</param>
    /// <param name="mysqlCol">The <see cref="MySqlDataColumn"/> bound to the grid column.</param>
    private void SetGridColumnColor(DataGridViewColumn gridCol, MySqlDataColumn mysqlCol)
    {
      gridCol.DefaultCellStyle.BackColor = mysqlCol.WarningsQuantity > 0 ? Color.OrangeRed : gridCol.DataGridView.DefaultCellStyle.BackColor;
    }

    /// <summary>
    /// Shows or hides the warning controls related to the given grid column.
    /// </summary>
    /// <param name="gridCol">The <see cref="DataGridViewColumn"/> to color.</param>
    private void SetGridColumnWarningVisibility(DataGridViewColumn gridCol)
    {
      if (gridCol == null)
      {
        return;
      }

      MySqlDataColumn mysqlCol = SourceMySqlPreviewDataTable.GetColumnAtIndex(gridCol.Index);
      bool showWarning = mysqlCol.WarningsQuantity > 0;
      ColumnWarningPictureBox.Visible = showWarning;
      ColumnWarningLabel.Visible = showWarning;
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
    /// Event delegate method fired when the <see cref="SourceExcelDataDataGridView"/> grid selection changes.
    /// </summary>
    /// <param name="sender">Sender object.</param>
    /// <param name="e">Event arguments.</param>
    private void SourceExcelDataDataGridView_SelectionChanged(object sender, EventArgs e)
    {
      if (SourceExcelDataDataGridView.SelectedColumns.Count == 0)
      {
        return;
      }

      SetGridColumnWarningVisibility(SourceExcelDataDataGridView.SelectedColumns[0]);
    }

    /// <summary>
    /// Saves a column mapping object in file.
    /// </summary>
    /// <param name="mapping">Column mapping object to save.</param>
    /// <returns><c>true</c> if the mapping object did not exist in file already, <c>false</c> otherwise.</returns>
    private bool StoreColumnMappingInFile(MySqlColumnMapping mapping)
    {
      if (StoredColumnMappingsList.Contains(mapping))
      {
        return false;
      }

      MySqlColumnMappingList userList = new MySqlColumnMappingList();
      bool result = userList.Add(mapping);
      if (result)
      {
        RefreshMappingMethodCombo();
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
      string proposedMappingName;
      do
      {
        proposedMappingName = string.Format("{0}Mapping{1}", TargetMySqlPreviewDataTable.TableName, numericSuffix > 1 ? numericSuffix.ToString(CultureInfo.InvariantCulture) : string.Empty);
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

      // Initialize connection and DBObject information
      CurrentColumnMapping.Name = proposedMappingName;
      CurrentColumnMapping.ConnectionName = WbConnection.Name;
      CurrentColumnMapping.Port = WbConnection.Port;
      CurrentColumnMapping.SchemaName = WbConnection.Schema;
      CurrentColumnMapping.TableName = TargetMySqlPreviewDataTable.TableName;

      StoreColumnMappingInFile(CurrentColumnMapping);
      MappingMethodComboBox.SelectedIndex = MappingMethodComboBox.Items.Count - 1;
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
      if (CurrentColumnMapping == null)
      {
        return;
      }

      int mappingsCount = CurrentColumnMapping.MappedSourceIndexes.Length;
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

      // Refresh Grids
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
      // Ensure that the dragged item is contained in the data.
      if (e.Data.GetDataPresent(typeof(Int32)))
      {
        int fromColumnIndex = Convert.ToInt32(e.Data.GetData(typeof(Int32)));
        string draggedColumnName = SourceExcelDataDataGridView.Columns[fromColumnIndex].HeaderText;
        if (_gridTargetTableColumnIndexToDrop >= 0)
        {
          switch (e.Effect)
          {
            // We are mapping a column from the Source Grid to the Target Grid
            case DragDropEffects.Link:
              MySqlDataColumn toCol = TargetMySqlPreviewDataTable.GetColumnAtIndex(_gridTargetTableColumnIndexToDrop);
              if (!string.IsNullOrEmpty(toCol.MappedDataColName))
              {
                bool isIdenticalMapping = toCol.MappedDataColName == draggedColumnName;
                DialogResult dr = DialogResult.No;
                if (!isIdenticalMapping)
                {
                  dr = InfoDialog.ShowYesNoDialog(InfoDialog.InfoType.Warning, Resources.ColumnMappedOverwriteTitleWarning, Resources.ColumnMappedOverwriteDetailWarning);
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

            // We are moving a column mapping from a column on the Target Grid to another column in the same grid
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
                  dr = InfoDialog.ShowYesNoDialog(InfoDialog.InfoType.Warning, Resources.ColumnMappedOverwriteTitleWarning, Resources.ColumnMappedExchangeDetailWarning);
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
      // Determine whether string data exists in the drop data. If not, then the drop effect reflects that the drop cannot occur.
      if (!e.Data.GetDataPresent(typeof(Int32)))
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