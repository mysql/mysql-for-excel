// Copyright (c) 2012, 2015, Oracle and/or its affiliates. All rights reserved.
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
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using MySQL.ForExcel.Classes;
using MySQL.ForExcel.Controls;
using MySQL.ForExcel.Interfaces;
using MySQL.ForExcel.Properties;
using MySQL.Utility.Classes;
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
    /// The maximum number of columns that can be mapped based on the maximum number of columns between the source and the target tables.
    /// </summary>
    private readonly int _maxMappingColumnsQuantity;

    /// <summary>
    /// The Excel range containing the data to append to a MySQL table.
    /// </summary>
    private Excel.Range _appendDataRange;

    /// <summary>
    /// The <see cref="DbTable"/> to which to append data to.
    /// </summary>
    private readonly DbTable _appendDbTable;

    /// <summary>
    /// The column mapping currently being used in the append session.
    /// </summary>
    private MySqlColumnMapping _currentColumnMapping;

    /// <summary>
    /// Rectangle used to measure drag and drop operations.
    /// </summary>
    private Rectangle _dragBoxFromMouseDown;

    /// <summary>
    /// Cursor displayed during a mapping operation when a column is being dragged.
    /// </summary>
    private readonly Cursor _draggingCursor;

    /// <summary>
    /// Cursor displayed during a mapping operation when a column is being dropped.
    /// </summary>
    private readonly Cursor _droppableCursor;

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
    /// The table containing a small preview subset of Excel data to append to a MySQL Server table.
    /// </summary>
    private MySqlDataTable _sourceMySqlPreviewDataTable;

    /// <summary>
    /// The table containing a small preview subset of the MySQL Server table where data is going to be appended to.
    /// </summary>
    private MySqlDataTable _targetMySqlPreviewDataTable;

    /// <summary>
    /// Cursor displayed during a mapping operation when a column is being dragged out of the columns area, so the column is unmapped.
    /// </summary>
    private readonly Cursor _trashCursor;

    #endregion Fields

    /// <summary>
    /// Initializes a new instance of the <see cref="AppendDataForm"/> class.
    /// </summary>
    /// <param name="appendDbTable">The <see cref="DbTable"/> to which to append data to.</param>
    /// <param name="appendDataRange">The Excel range containing the data to append to a MySQL table.</param>
    /// <param name="appendingWorksheetName">The name of the worksheet holding the appending data.</param>
    public AppendDataForm(DbTable appendDbTable, Excel.Range appendDataRange, string appendingWorksheetName)
    {
      if (appendDbTable == null)
      {
        throw new ArgumentNullException("appendDbTable");
      }

      _appendDataRange = appendDataRange;
      _appendDbTable = appendDbTable;
      _sourceMySqlPreviewDataTable = null;
      _targetMySqlPreviewDataTable = null;

      _dragBoxFromMouseDown = Rectangle.Empty;
      _draggingCursor = new Bitmap(Resources.MySQLforExcel_Cursor_Dragging_32x32).CreateCursor(3, 3);
      _droppableCursor = new Bitmap(Resources.MySQLforExcel_Cursor_Dropable_32x32).CreateCursor(3, 3);
      _gridColumnClicked = -1;
      _gridColumnIndexToDrag = -1;
      _gridTargetTableColumnIndexToDrop = -1;
      _trashCursor = new Bitmap(Resources.MySQLforExcel_Cursor_Trash_32x32).CreateCursor(3, 3);

      InitializeComponent();

      InitializeSourceTableGrid();
      InitializeTargetTableGrid();

      string excelRangeAddress = appendDataRange.Address.Replace("$", string.Empty);
      Text = string.Format("Append Data - {0} [{1}]", appendingWorksheetName, excelRangeAddress);
      _maxMappingColumnsQuantity = Math.Min(TargetMySqlTableDataGridView.Columns.Count, SourceExcelDataDataGridView.Columns.Count);
      ClearMappings(true);
      RefreshMappingMethodCombo();
      if (!SelectStoredMappingForTargetTable())
      {
        MappingMethodComboBox.SelectedIndex = Settings.Default.AppendPerformAutoMap ? 0 : 1;
      }
    }

    #region Properties

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
    /// Gets a list of column mappings for the current user.
    /// </summary>
    private List<MySqlColumnMapping> StoredColumnMappingsList
    {
      get
      {
        return Settings.Default.StoredDataMappings ?? (Settings.Default.StoredDataMappings = new List<MySqlColumnMapping>());
      }
    }

    #endregion Properties

    /// <summary>
    /// Event delegate method fired when the <see cref="AdvancedOptionsButton"/> button is clicked.
    /// </summary>
    /// <param name="sender">Sender object.</param>
    /// <param name="e">Event arguments.</param>
    private void AdvancedOptionsButton_Click(object sender, EventArgs e)
    {
      using (var optionsDialog = new AppendAdvancedOptionsDialog(StoredColumnMappingsList))
      {
        optionsDialog.ShowDialog();
        if (optionsDialog.MappingsChanged)
        {
          // A stored mapping was renamed or deleted, so we need to refresh the combobox that shows stored mappings and re-select the mapping that matches the current table
          //  or apply an automatic mapping. Either of these 2 options re-applies the mappings to the target grid so the mapping is shown on the additional column headers.
          RefreshMappingMethodCombo();
          if (!SelectStoredMappingForTargetTable())
          {
            MappingMethodComboBox.SelectedIndex = Settings.Default.AppendPerformAutoMap ? 0 : 1;
          }
        }

        // The following code may look like it can be refactored (looks repetitive), it is written like this so code only runs when needed depending on changed values.
        // If refactored (or left as it was before) some code runs unnecessarily.
        if (optionsDialog.UseFormattedValuesChanged || optionsDialog.LimitPreviewRowsQuantityChanged)
        {
          // Values that affect how data types are detected for the target table changed, so we need to re-create the data sources of the grids and refresh the grids completely.
          // The current mapping needs to be re-applied only if it was not re-applied in the code above.
          InitializeSourceTableGrid();
          InitializeTargetTableGrid();
          if (!optionsDialog.MappingsChanged)
          {
            ApplySelectedStoredColumnMapping();
          }
        }
        else if (optionsDialog.ShowDataTypesChanged)
        {
          // Only the checkbox to show/hide data types changed. Note this is on an "else if" clause because this same code runs in the previous if block within the Initialize methods.
          // The current mapping needs to be re-applied only if it was not re-applied in the code above.
          RefreshAdditionalColumnHeaderRows(SourceExcelDataDataGridView);
          RefreshAdditionalColumnHeaderRows(TargetMySqlTableDataGridView);
          if (!optionsDialog.MappingsChanged)
          {
            ApplySelectedStoredColumnMapping();
          }
        }
      }
    }

    /// <summary>
    /// Event delegate method fired when the <see cref="AppendContextMenu"/> menu is being opened.
    /// </summary>
    /// <param name="sender">Sender object.</param>
    /// <param name="e">Event arguments.</param>
    private void AppendContextMenu_Opening(object sender, CancelEventArgs e)
    {
      if (_gridColumnClicked < 0 || _currentColumnMapping == null || _currentColumnMapping.MappedQuantity == 0)
      {
        e.Cancel = true;
        return;
      }

      bool columnHasMapping = AppendContextMenu.SourceControl.Name == TargetMySqlTableDataGridView.Name
        ? _currentColumnMapping.MappedSourceIndexes[_gridColumnClicked] >= 0
        : _currentColumnMapping.MappedSourceIndexes.Contains(_gridColumnClicked);
      AppendContextMenu.Items["RemoveColumnMappingToolStripMenuItem"].Visible = columnHasMapping;
      AppendContextMenu.Items["ClearAllMappingsToolStripMenuItem"].Visible = _currentColumnMapping.MappedQuantity > 0;
    }

    /// <summary>
    /// Appends the selected Excel data to the selected MySQL table.
    /// </summary>
    /// <returns><c>true</c> if the append operation is successful, <c>false</c> otherwise.</returns>
    private bool AppendData()
    {
      // If there are no data rows to be appended, show a warning message and abort the operation
      if (_sourceMySqlPreviewDataTable.Rows.Count == 0
        || (_sourceMySqlPreviewDataTable.Rows.Count == 1 && _sourceMySqlPreviewDataTable.FirstRowContainsColumnNames))
      {
        InfoDialog.ShowDialog(InfoDialogProperties.GetWarningDialogProperties(
          Resources.AppendDataNoDataToAppendTitleWarning,
          string.Format(Resources.AppendDataNoDataToAppendDetailWarning,
          _targetMySqlPreviewDataTable.TableName),
          null,
          Resources.AppendDataNoDataToAppendMoreInfo));
        return false;
      }

      // If not all columns where mapped between the source and target tables ask the user if he still wants to proceed with the append operation.
      if (_targetMySqlPreviewDataTable.MappedColumnsQuantity < _maxMappingColumnsQuantity
        && InfoDialog.ShowDialog(InfoDialogProperties.GetYesNoDialogProperties(InfoDialog.InfoType.Warning, Resources.ColumnMappingIncompleteTitleWarning, Resources.ColumnMappingIncompleteDetailWarning)).DialogResult == DialogResult.No)
      {
        return false;
      }

      Cursor = Cursors.WaitCursor;
      var targetMySqlFinalDataTable = _targetMySqlPreviewDataTable.CloneSchema(true, false);
      targetMySqlFinalDataTable.FirstRowContainsColumnNames = _sourceMySqlPreviewDataTable.FirstRowContainsColumnNames;
      var mappedIndexes = new List<int>(targetMySqlFinalDataTable.Columns.Count);
      foreach (var sourceColumnIndex in from MySqlDataColumn targetColumn in targetMySqlFinalDataTable.Columns select targetColumn.MappedDataColOrdinal)
      {
        if (sourceColumnIndex < 0)
        {
          mappedIndexes.Add(0);
          continue;
        }

        var sourceColumn = _sourceMySqlPreviewDataTable.GetColumnAtIndex(sourceColumnIndex);
        mappedIndexes.Add(sourceColumn == null ? 0 : sourceColumn.RangeColumnIndex);
      }

      bool addDataSuccessful;
      using (var temporaryRange = new TempRange(_appendDataRange, true, false, true, mappedIndexes))
      {
        addDataSuccessful = targetMySqlFinalDataTable.AddExcelData(temporaryRange);
      }

      if (!addDataSuccessful)
      {
        Cursor = Cursors.Default;
        return false;
      }

      var modifiedRowsList = targetMySqlFinalDataTable.PushData(Settings.Default.GlobalSqlQueriesPreviewQueries);
      if (!AssembleAndShowOperationResults(modifiedRowsList, targetMySqlFinalDataTable.TableName))
      {
        return false;
      }

      if (Settings.Default.AppendAutoStoreColumnMapping
          && !StoredColumnMappingsList.Exists(mapping => mapping.ConnectionName == _appendDbTable.Connection.Name && mapping.SchemaName == _appendDbTable.Connection.Schema && mapping.TableName == targetMySqlFinalDataTable.TableName))
      {
        StoreCurrentColumnMapping(false);
      }

      return true;
    }

    /// <summary>
    /// Event delegate method fired when the <see cref="AppendDataForm"/> form is being closed.
    /// </summary>
    /// <param name="sender">Sender object.</param>
    /// <param name="e">Event arguments.</param>
    private void AppendDataForm_FormClosing(object sender, FormClosingEventArgs e)
    {
      if (DialogResult == DialogResult.OK)
      {
        e.Cancel = !AppendData();
      }
    }

    /// <summary>
    /// Applies the column mapping the users selects from a list of stored mappings.
    /// </summary>
    private void ApplySelectedStoredColumnMapping()
    {
      if (_currentColumnMapping == null)
      {
        return;
      }

      ClearMappings(true);
      for (int mappedIdx = 0; mappedIdx < _currentColumnMapping.MappedSourceIndexes.Length; mappedIdx++)
      {
        int currentMappedSourceIndex = _currentColumnMapping.MappedSourceIndexes[mappedIdx];
        string currentMappedColName = currentMappedSourceIndex >= 0 ? _currentColumnMapping.SourceColumns[currentMappedSourceIndex] : null;
        ApplySingleMapping(currentMappedSourceIndex, mappedIdx, currentMappedColName);
      }

      TargetMySqlTableDataGridView.Refresh();
      SourceExcelDataDataGridView.Refresh();
      StoreMappingButton.Enabled = _currentColumnMapping.MappedQuantity > 0;
    }

    /// <summary>
    /// Applies a single-column mapping.
    /// </summary>
    /// <param name="sourceColumnIndex">Column index in the source table being mapped.</param>
    /// <param name="targetColumnIndex">Column index in the target table where data is mapped to.</param>
    /// <param name="mappedColName">Name of the target column in the target table.</param>
    private void ApplySingleMapping(int sourceColumnIndex, int targetColumnIndex, string mappedColName)
    {
      int previouslyMappedFromIndex = _currentColumnMapping.MappedSourceIndexes[targetColumnIndex];
      bool mapping = mappedColName != null && sourceColumnIndex >= 0;

      // Change text and style of target table column
      var headerCell = TargetMySqlTableDataGridView.MultiHeaderRowsCollection[0][targetColumnIndex];
      headerCell.Text = mapping ? mappedColName : string.Empty;
      headerCell.Style.BackColor = mapping ? Color.LightGreen : Color.OrangeRed;

      // Change style of source table column being mapped or unmapped
      if (mapping)
      {
        var sourceColumnStyle = SourceExcelDataDataGridView.Columns[sourceColumnIndex].HeaderCell.Style;
        sourceColumnStyle.SelectionBackColor = sourceColumnStyle.BackColor = Color.LightGreen;
      }
      else if (previouslyMappedFromIndex >= 0 && _currentColumnMapping.MappedSourceIndexes.Count(sourceIdx => sourceIdx == previouslyMappedFromIndex) <= 1)
      {
        var sourceColumnStyle = SourceExcelDataDataGridView.Columns[previouslyMappedFromIndex].HeaderCell.Style;
        sourceColumnStyle.SelectionBackColor = sourceColumnStyle.BackColor = SystemColors.Control;
      }

      // Store the actual mapping
      MySqlDataColumn sourceColumn = mapping ? _sourceMySqlPreviewDataTable.GetColumnAtIndex(sourceColumnIndex) : null;
      MySqlDataColumn targetColumn = _targetMySqlPreviewDataTable.GetColumnAtIndex(targetColumnIndex);
      targetColumn.MappedDataColName = mapping ? sourceColumn.ColumnName : null;
      targetColumn.MappedDataColOrdinal = mapping ? sourceColumnIndex : -1;

      _currentColumnMapping.MappedSourceIndexes[targetColumnIndex] = sourceColumnIndex;
      if (mapping)
      {
        CheckSourceDataAgainstMappedTargetColumn(sourceColumn, targetColumn);
      }
      else
      {
        ClearTargetColumnVisualWarnings(targetColumnIndex);
      }
    }

    /// <summary>
    /// Assembles the informational messages to be displayed after the Export Data operation executed and shows it to the user.
    /// </summary>
    /// <param name="modifiedRowsList">A list of <see cref="IMySqlDataRow"/> objects result of a push data operation.</param>
    /// <param name="targetMySqlTableName">The name of the MySQL table where the data is appended to.</param>
    /// <returns><c>true</c> if the overall result of the operation was successful (even if warnings were found), <c>false</c> if an error was thrown.</returns>
    private bool AssembleAndShowOperationResults(List<IMySqlDataRow> modifiedRowsList, string targetMySqlTableName)
    {
      if (modifiedRowsList == null)
      {
        Cursor = Cursors.Default;
        return false;
      }

      int warningsCount = 0;
      bool errorsFound = false;
      bool warningsFound = false;
      string operationSummary;
      var operationDetails = new StringBuilder();
      var warningDetails = new StringBuilder();
      var warningStatementDetails = new StringBuilder();
      if (Settings.Default.GlobalSqlQueriesShowQueriesWithResults)
      {
        operationDetails.AppendFormat(Resources.InsertedExcelDataWithQueryText, targetMySqlTableName);
        operationDetails.AddNewLine();
      }

      bool warningDetailHeaderAppended = false;
      var statementsQuantityFormat = new string('0', modifiedRowsList.Count.StringSize());
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
        operationSummary = string.Format(Resources.AppendDataDetailsDoneErrorText, targetMySqlTableName);
        operationsType = InfoDialog.InfoType.Error;
      }
      else
      {
        operationSummary = string.Format(warningsFound ? Resources.AppendDataDetailsDoneWarningsText : Resources.AppendDataDetailsDoneSuccessText, targetMySqlTableName);
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
      return !errorsFound;
    }

    /// <summary>
    /// Checks if the source data is suitable for the target's column data type to raise visual warnings.
    /// </summary>
    /// <param name="sourceColumn">Source data column.</param>
    /// <param name="targetColumn">Target column.</param>
    private void CheckSourceDataAgainstMappedTargetColumn(MySqlDataColumn sourceColumn, MySqlDataColumn targetColumn)
    {
      if (sourceColumn == null || targetColumn == null)
      {
        return;
      }

      sourceColumn.CanDataBeStoredInGivenColumn(targetColumn);
      var targetGridCol = TargetMySqlTableDataGridView.Columns[targetColumn.Ordinal];
      SetGridColumnColor(targetGridCol, targetColumn);
      SetGridColumnWarningVisibility(targetGridCol);
    }

    /// <summary>
    /// Event delegate method fired when the <see cref="ClearAllMappingsToolStripMenuItem"/> menu item is clicked.
    /// </summary>
    /// <param name="sender">Sender object.</param>
    /// <param name="e">Event arguments.</param>
    private void ClearAllMappingsToolStripMenuItem_Click(object sender, EventArgs e)
    {
      ClearMappings(false);
      if (_currentColumnMapping.Name == "Automatic")
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
      for (int colIdx = 0; colIdx < TargetMySqlTableDataGridView.Columns.Count; colIdx++)
      {
        var headerCell = TargetMySqlTableDataGridView.MultiHeaderRowsCollection[0][colIdx];
        headerCell.Text = string.Empty;
        headerCell.Style.BackColor = Color.OrangeRed;
        var toCol = _targetMySqlPreviewDataTable.Columns[colIdx] as MySqlDataColumn;
        if (toCol != null)
        {
          toCol.MappedDataColName = null;
          toCol.MappedDataColOrdinal = -1;

          // Clear target column warnings
          toCol.ClearWarnings();
          SetGridColumnColor(TargetMySqlTableDataGridView.Columns[colIdx], toCol);
        }

        if (colIdx >= SourceExcelDataDataGridView.Columns.Count)
        {
          continue;
        }

        DataGridViewCellStyle newStyle = new DataGridViewCellStyle(SourceExcelDataDataGridView.Columns[colIdx].HeaderCell.Style);
        newStyle.SelectionBackColor = newStyle.BackColor = SystemColors.Control;
        SourceExcelDataDataGridView.Columns[colIdx].HeaderCell.Style = newStyle;
      }

      if (_currentColumnMapping != null && !onlyGrids)
      {
        _currentColumnMapping.ClearMappings();
      }

      TargetMySqlTableDataGridView.Refresh();
      SourceExcelDataDataGridView.Refresh();
      StoreMappingButton.Enabled = false;
    }

    /// <summary>
    /// Clears all visual warnings related to a target grid column.
    /// </summary>
    /// <param name="targetColumnIndex">Target column index.</param>
    private void ClearTargetColumnVisualWarnings(int targetColumnIndex)
    {
      if (targetColumnIndex < 0)
      {
        return;
      }

      var targetDataColumn = _targetMySqlPreviewDataTable.GetColumnAtIndex(targetColumnIndex);
      targetDataColumn.ClearWarnings();
      var targetGridCol = TargetMySqlTableDataGridView.Columns[targetColumnIndex];
      SetGridColumnColor(targetGridCol, targetDataColumn);
      SetGridColumnWarningVisibility(targetGridCol);
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
      MySqlColumnMapping autoMapping = new MySqlColumnMapping("Automatic", _sourceMySqlPreviewDataTable.GetColumnNamesArray(true), _targetMySqlPreviewDataTable.GetColumnNamesArray())
      {
        SchemaName = _appendDbTable.Connection.Schema,
        TableName = _targetMySqlPreviewDataTable.TableName,
        ConnectionName = _appendDbTable.Connection.Name,
        Port = _appendDbTable.Connection.Port
      };
      int autoMappedColumns = 0;

      // Attempt to auto-map using toColumn names regardless of positioning if the data types match
      if (FirstRowHeadersCheckBox.Checked)
      {
        for (int targetColumnIndex = 0; targetColumnIndex < _targetMySqlPreviewDataTable.Columns.Count; targetColumnIndex++)
        {
          string targetColumnName = _targetMySqlPreviewDataTable.Columns[targetColumnIndex].ColumnName;
          int sourceColumnIndex = _sourceMySqlPreviewDataTable.GetColumnIndex(targetColumnName, true, false);
          if (sourceColumnIndex < 0)
          {
            continue;
          }

          MySqlDataColumn sourceColumn = _sourceMySqlPreviewDataTable.GetColumnAtIndex(sourceColumnIndex);
          MySqlDataColumn targetColumn = _targetMySqlPreviewDataTable.GetColumnAtIndex(targetColumnIndex);
          if (!DataTypeUtilities.Type1FitsIntoType2(sourceColumn.StrippedMySqlDataType, targetColumn.StrippedMySqlDataType))
          {
            continue;
          }

          autoMapping.MappedSourceIndexes[targetColumnIndex] = sourceColumnIndex;
          autoMappedColumns++;
        }
      }

      if (autoMappedColumns != 0)
      {
        return autoMapping;
      }

      // Auto-map 1-1 if just data types match
      autoMapping.ClearMappings();
      for (int columnIndex = 0; columnIndex < _targetMySqlPreviewDataTable.Columns.Count; columnIndex++)
      {
        if (columnIndex >= _maxMappingColumnsQuantity)
        {
          break;
        }

        MySqlDataColumn sourceColumn = _sourceMySqlPreviewDataTable.GetColumnAtIndex(columnIndex);
        MySqlDataColumn targetColumn = _targetMySqlPreviewDataTable.GetColumnAtIndex(columnIndex);
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
      if (_currentColumnMapping == null)
      {
        manualMapping = new MySqlColumnMapping(_sourceMySqlPreviewDataTable.GetColumnNamesArray(), _targetMySqlPreviewDataTable.GetColumnNamesArray())
        {
          SchemaName = _appendDbTable.Connection.Schema,
          TableName = _targetMySqlPreviewDataTable.TableName,
          ConnectionName = _appendDbTable.Connection.Name,
          Port = _appendDbTable.Connection.Port
        };
      }
      else
      {
        manualMapping = _currentColumnMapping;
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
      MySqlColumnMapping matchedMapping = new MySqlColumnMapping(_currentColumnMapping, _sourceMySqlPreviewDataTable.GetColumnNamesArray(true), _targetMySqlPreviewDataTable.GetColumnNamesArray());

      // Check if Target Columns still match with the Target Table, switch mapped indexes if columns changed positions
      //  and skip target column in stored mapping is not present anymore in Target Table
      for (int storedMappedIdx = 0; storedMappedIdx < _currentColumnMapping.TargetColumns.Length; storedMappedIdx++)
      {
        // Get the source index of the stored mapping for the current tartet column, if -1 there was no mapping for the
        // target column at that position so we skip it.
        int proposedSourceMapping = _currentColumnMapping.MappedSourceIndexes[storedMappedIdx];
        if (proposedSourceMapping < 0)
        {
          continue;
        }

        // Check if Target Column in Stored Mapping is found within any of the TargetColumns of the matching mapping.
        // If not found we should not map so we skip this Target Column.
        string storedMappedColName = _currentColumnMapping.TargetColumns[storedMappedIdx];
        int targetColumnIndex = matchedMapping.GetTargetColumnIndex(storedMappedColName);
        if (targetColumnIndex < 0)
        {
          continue;
        }

        MySqlDataColumn targetColumn = _targetMySqlPreviewDataTable.GetColumnAtIndex(targetColumnIndex);

        // Check if mapped source column from Stored Mapping matches a Source Column in current "From Table"
        //  and if its data type matches its corresponding target column's data type, if so we are good to map it
        string mappedSourceColName = _currentColumnMapping.SourceColumns[proposedSourceMapping];
        int sourceColFoundInFromTableIdx = _sourceMySqlPreviewDataTable.GetColumnIndex(mappedSourceColName, true, false);
        if (sourceColFoundInFromTableIdx >= 0)
        {
          MySqlDataColumn sourceColumn = _sourceMySqlPreviewDataTable.GetColumnAtIndex(sourceColFoundInFromTableIdx);
          if (DataTypeUtilities.Type1FitsIntoType2(sourceColumn.StrippedMySqlDataType, targetColumn.StrippedMySqlDataType))
          {
            matchedMapping.MappedSourceIndexes[targetColumnIndex] = sourceColFoundInFromTableIdx;
          }
        }
        // Since source columns do not match in name and type, try to match the mapped source column's datatype
        //  with the From column in that source index only if that From Column name is not in any source mapping.
        else if (matchedMapping.MappedSourceIndexes[targetColumnIndex] < 0 && proposedSourceMapping < _sourceMySqlPreviewDataTable.Columns.Count)
        {
          string fromTableColName = _sourceMySqlPreviewDataTable.GetColumnAtIndex(proposedSourceMapping).DisplayName;
          int fromTableColNameFoundInStoredMappingSourceColumnsIdx = _currentColumnMapping.GetSourceColumnIndex(fromTableColName);
          if (fromTableColNameFoundInStoredMappingSourceColumnsIdx >= 0
            && fromTableColNameFoundInStoredMappingSourceColumnsIdx != proposedSourceMapping
            && _currentColumnMapping.GetMappedSourceIndexIndex(fromTableColNameFoundInStoredMappingSourceColumnsIdx) >= 0)
          {
            continue;
          }

          MySqlDataColumn sourceColumn = _sourceMySqlPreviewDataTable.GetColumnAtIndex(proposedSourceMapping);
          if (DataTypeUtilities.Type1FitsIntoType2(sourceColumn.StrippedMySqlDataType, targetColumn.StrippedMySqlDataType))
          {
            matchedMapping.MappedSourceIndexes[targetColumnIndex] = proposedSourceMapping;
          }
        }
      }

      _currentColumnMapping = matchedMapping;
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

        case "TargetMySqlTableDataGridView":
          if (_gridColumnIndexToDrag >= 0 && _currentColumnMapping != null && _currentColumnMapping.MappedSourceIndexes[_gridColumnIndexToDrag] >= 0)
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

      // Update the value of the first row being used for column names in the "From" table
      _sourceMySqlPreviewDataTable.FirstRowContainsColumnNames = firstRowColNames;

      // Refresh the header rows in the source grid since data types might have changed after changing the FirstRowContainsColumnNames property value
      RefreshAdditionalColumnHeaderRows(SourceExcelDataDataGridView);

      // Refresh the "From"/"Source" Grid and "From"/"Source" toColumn names in the current mapping
      SourceExcelDataDataGridView.CurrentCell = null;
      for (int colIdx = 0; colIdx < SourceExcelDataDataGridView.Columns.Count; colIdx++)
      {
        DataGridViewColumn gridCol = SourceExcelDataDataGridView.Columns[colIdx];
        gridCol.HeaderText = firstRowColNames ? SourceExcelDataDataGridView.Rows[0].Cells[gridCol.Index].Value.ToString() : _sourceMySqlPreviewDataTable.Columns[gridCol.Index].ColumnName;
        if (_currentColumnMapping != null)
        {
          _currentColumnMapping.SourceColumns[colIdx] = gridCol.HeaderText;
        }
      }

      SourceExcelDataDataGridView.Rows[0].Visible = !firstRowColNames;
      if (!(FirstRowHeadersCheckBox.Checked && SourceExcelDataDataGridView.Rows.Count < 2))
      {
        SourceExcelDataDataGridView.FirstDisplayedScrollingRowIndex = FirstRowHeadersCheckBox.Checked ? 1 : 0;
      }

      // Refresh the mapped columns in the "To" Grid
      for (int colIdx = 0; colIdx < TargetMySqlTableDataGridView.Columns.Count; colIdx++)
      {
        var headerCell = TargetMySqlTableDataGridView.MultiHeaderRowsCollection[0][colIdx];
        if (_currentColumnMapping == null)
        {
          continue;
        }

        int mappedSourceIndex = _currentColumnMapping.MappedSourceIndexes[colIdx];
        if (!string.IsNullOrEmpty(headerCell.Text) && mappedSourceIndex >= 0)
        {
          headerCell.Text = SourceExcelDataDataGridView.Columns[mappedSourceIndex].HeaderText;
        }
      }

      // Re-do the Currently Selected mapping (unless we are on Manual) since now columns may match
      if (_currentColumnMapping != null && _currentColumnMapping.Name != "Manual")
      {
        MappingMethodComboBox_SelectedIndexChanged(MappingMethodComboBox, EventArgs.Empty);
      }
    }

    /// <summary>
    /// Initilizes the grid containing the source data contained in the From table.
    /// </summary>
    private void InitializeSourceTableGrid()
    {
      _sourceMySqlPreviewDataTable = new MySqlDataTable(
        _appendDbTable.Connection,
        "Source Excel Range",
        false,
        Settings.Default.AppendUseFormattedValues,
        true,
        false,
        false,
        false,
        false);
      int previewRowsQty = Math.Min(_appendDataRange.Rows.Count, Settings.Default.AppendLimitPreviewRowsQuantity);
      _sourceMySqlPreviewDataTable.SetupColumnsWithData(_appendDataRange, true, previewRowsQty);
      SourceExcelDataDataGridView.DataSource = _sourceMySqlPreviewDataTable;
      SourceExcelDataDataGridView.SelectionMode = DataGridViewSelectionMode.FullColumnSelect;
      FirstRowHeadersCheckBox_CheckedChanged(FirstRowHeadersCheckBox, EventArgs.Empty);
    }

    /// <summary>
    /// Initilizes the grid containing preview data contained in the target table.
    /// </summary>
    private void InitializeTargetTableGrid()
    {
      SetPreviewParameterValues();
      _targetMySqlPreviewDataTable = new MySqlDataTable(_appendDbTable, Settings.Default.AppendUseFormattedValues);
      TargetMySqlTableDataGridView.DataSource = _targetMySqlPreviewDataTable;
      TargetMySqlTableDataGridView.SelectionMode = DataGridViewSelectionMode.FullColumnSelect;
      foreach (DataGridViewColumn column in TargetMySqlTableDataGridView.Columns)
      {
        column.HeaderCell.Style.BackColor = SystemColors.Control;
      }

      RefreshAdditionalColumnHeaderRows(TargetMySqlTableDataGridView);
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
        case -1:
        case 0:
          _currentColumnMapping = CreateColumnMappingForAutomatic();
          ApplySelectedStoredColumnMapping();
          break;

        case 1:
          _currentColumnMapping = CreateColumnMappingForManual();
          break;

        default:
          _currentColumnMapping = new MySqlColumnMapping(StoredColumnMappingsList[MappingMethodComboBox.SelectedIndex - 2]);
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
      if (_currentColumnMapping.Name == "Automatic")
      {
        MappingMethodComboBox.SelectedIndex = 1;
      }

      ApplySingleMapping(fromColumnIndex, toColumnIndex, mappedColName);

      // Refresh Grids
      TargetMySqlTableDataGridView.Refresh();
      SourceExcelDataDataGridView.Refresh();
      StoreMappingButton.Enabled = _currentColumnMapping.MappedQuantity > 0;
    }

    /// <summary>
    /// Sets the tooltip text shown on column headers, containing the data type of each column.
    /// </summary>
    /// <param name="grid">The <see cref="DataGridView"/> which column header tooltips need to be refreshed.</param>
    private void RefreshColumnHeaderDataTypeToolTips(DataGridView grid)
    {
      if (grid == null)
      {
        return;
      }

      bool showDataTypes = Settings.Default.AppendShowDataTypes;
      var mySqlTable = grid.DataSource as MySqlDataTable;
      foreach (DataGridViewColumn gridColumn in grid.Columns)
      {
        if (mySqlTable != null)
        {
          var mySqlColumn = mySqlTable.Columns[gridColumn.Index] as MySqlDataColumn;
          if (mySqlColumn != null)
          {
            gridColumn.Tag = mySqlColumn.StrippedMySqlDataType;
          }
        }

        gridColumn.ToolTipText = showDataTypes ? string.Empty : gridColumn.Tag.ToString();
      }
    }

    /// <summary>
    /// Rebuilds the rows of additional column headers and their column header tooltips.
    /// </summary>
    /// <param name="multiHeaderGrid">The <see cref="MultiHeaderDataGridView"/> object to refresh.</param>
    private void RefreshAdditionalColumnHeaderRows(MultiHeaderDataGridView multiHeaderGrid)
    {
      if (multiHeaderGrid == null)
      {
        return;
      }

      RefreshColumnHeaderDataTypeToolTips(multiHeaderGrid);
      SetupAdditionalHeaderRows(multiHeaderGrid, multiHeaderGrid.Name == "TargetMySqlTableDataGridView");
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
      int targetColumnIndex = AppendContextMenu.SourceControl.Name == TargetMySqlTableDataGridView.Name
        ? _gridColumnClicked
        : _currentColumnMapping.MappedSourceIndexes.ToList().FindIndex(sourceIndex => sourceIndex == _gridColumnClicked);
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
        if (mapping.TableName != _targetMySqlPreviewDataTable.TableName || !mapping.AllColumnsMatch(_targetMySqlPreviewDataTable, true))
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
      gridCol.DefaultCellStyle.BackColor = string.IsNullOrEmpty(mysqlCol.CurrentWarningText) 
        ? gridCol.DataGridView.DefaultCellStyle.BackColor
        : Color.OrangeRed;
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

      MySqlDataColumn mysqlCol = _targetMySqlPreviewDataTable.GetColumnAtIndex(gridCol.Index);
      bool showWarning = !string.IsNullOrEmpty(mysqlCol.CurrentWarningText);
      ColumnWarningPictureBox.Visible = showWarning;
      ColumnWarningLabel.Visible = showWarning;
    }

    /// <summary>
    /// Sets the import parameter values into the database object.
    /// This is needed before getting any data from it.
    /// </summary>
    private void SetPreviewParameterValues()
    {
      _appendDbTable.ImportParameters.ColumnsNamesList = null;
      _appendDbTable.ImportParameters.FirstRowIndex = 0;
      _appendDbTable.ImportParameters.RowsCount = Settings.Default.AppendLimitPreviewRowsQuantity;
    }

    /// <summary>
    /// Creates additional header rows in the given <see cref="MultiHeaderDataGridView"/> to show column data types and column mappings.
    /// </summary>
    /// <param name="multiHeaderGrid">The <see cref="MultiHeaderDataGridView"/> object where additional header rows will be created.</param>
    /// <param name="addMappingsHeader">Flag indicating whether an extra header row should be added to show column mappings.</param>
    private void SetupAdditionalHeaderRows(MultiHeaderDataGridView multiHeaderGrid, bool addMappingsHeader)
    {
      if (multiHeaderGrid == null)
      {
        return;
      }

      multiHeaderGrid.ClearHeadersRows();
      bool showDataTypes = Settings.Default.AppendShowDataTypes;
      int mappingsRowsQuantity = addMappingsHeader ? 1 : 0;
      int additionalHeaderRowsQuantity = (showDataTypes ? 1 : 0) + mappingsRowsQuantity;
      for (int additionalRowIndex = 0; additionalRowIndex < additionalHeaderRowsQuantity; additionalRowIndex++)
      {
        multiHeaderGrid.AddHeadersRow(false);
      }

      if (!showDataTypes)
      {
        return;
      }

      foreach (var headerCell in multiHeaderGrid.MultiHeaderRowsCollection[mappingsRowsQuantity])
      {
        var gridColumn = multiHeaderGrid.Columns[headerCell.ColumnIndex];
        headerCell.Style.Font = new Font(multiHeaderGrid.ColumnHeadersDefaultCellStyle.Font, FontStyle.Italic);
        headerCell.Text = gridColumn.Tag != null ? gridColumn.Tag.ToString() : string.Empty;
      }
    }

    /// <summary>
    /// Event delegate method fired when an element is dragged over the <see cref="SourceExcelDataDataGridView"/> control.
    /// </summary>
    /// <param name="sender">Sender object.</param>
    /// <param name="e">Event arguments.</param>
    private void SourceExcelDataDataGridView_DragDrop(object sender, DragEventArgs e)
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
    /// Event delegate method fired when an element is dragged over the <see cref="SourceExcelDataDataGridView"/> control.
    /// </summary>
    /// <param name="sender">Sender object.</param>
    /// <param name="e">Event arguments.</param>
    private void SourceExcelDataDataGridView_DragOver(object sender, DragEventArgs e)
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
    /// Saves a column mapping object in file.
    /// </summary>
    /// <param name="mapping">Column mapping object to save.</param>
    /// <returns><c>true</c> if the mapping object did not exist in file already, <c>false</c> otherwise.</returns>
    private void StoreColumnMappingInFile(MySqlColumnMapping mapping)
    {
      if (StoredColumnMappingsList.Contains(mapping))
      {
        return;
      }

      StoredColumnMappingsList.Add(mapping);
      if (MiscUtilities.SaveSettings())
      {
        RefreshMappingMethodCombo();
      }
    }

    /// <summary>
    /// Stores the current column mapping in file, automatically proposes a mapping name given the target To table's name.
    /// </summary>
    /// <param name="showNewColumnMappingDialog">Flag indicating whether a dialog asking the user to confirm or change the proposed mapping name is shown.</param>
    private void StoreCurrentColumnMapping(bool showNewColumnMappingDialog)
    {
      int numericSuffix = 1;
      var proposedMappingName = new[] { string.Empty };
      do
      {
        var suffix = numericSuffix > 1 ? string.Format("_{0}", numericSuffix) : string.Empty;
        proposedMappingName[0] = string.Format("{0}_mapping{1}", _targetMySqlPreviewDataTable.TableName, suffix);
        numericSuffix++;
      }
      while (StoredColumnMappingsList.Any(mapping => string.Equals(mapping.Name, proposedMappingName[0], StringComparison.InvariantCultureIgnoreCase)));

      if (showNewColumnMappingDialog)
      {
        DialogResult dr;
        using (var newColumnMappingDialog = new AppendNewColumnMappingDialog(proposedMappingName[0]))
        {
          dr = newColumnMappingDialog.ShowDialog();
          proposedMappingName[0] = newColumnMappingDialog.ColumnMappingName;
        }

        if (dr == DialogResult.Cancel)
        {
          return;
        }
      }

      // Initialize connection and DBObject information
      _currentColumnMapping.Name = proposedMappingName[0];
      _currentColumnMapping.ConnectionName = _appendDbTable.Connection.Name;
      _currentColumnMapping.Port = _appendDbTable.Connection.Port;
      _currentColumnMapping.SchemaName = _appendDbTable.Connection.Schema;
      _currentColumnMapping.TableName = _targetMySqlPreviewDataTable.TableName;

      StoreColumnMappingInFile(_currentColumnMapping);
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
      if (_currentColumnMapping == null)
      {
        return;
      }

      int mappingsCount = _currentColumnMapping.MappedSourceIndexes.Length;
      if (mappingsCount == 0 || mappingSourceIndex1 < 0 || mappingSourceIndex1 >= mappingsCount || mappingSourceIndex2 < 0 || mappingSourceIndex2 >= mappingsCount)
      {
        return;
      }

      string mapping1ColName = TargetMySqlTableDataGridView.MultiHeaderRowsCollection[0][mappingSourceIndex1].Text;
      int mapping1Index = _currentColumnMapping.MappedSourceIndexes[mappingSourceIndex1];
      string mapping2ColName = TargetMySqlTableDataGridView.MultiHeaderRowsCollection[0][mappingSourceIndex2].Text;
      int mapping2Index = _currentColumnMapping.MappedSourceIndexes[mappingSourceIndex2];

      ApplySingleMapping(mapping1Index, mappingSourceIndex2, mapping1ColName);
      ApplySingleMapping(mapping2Index, mappingSourceIndex1, mapping2ColName);

      _currentColumnMapping.MappedSourceIndexes[mappingSourceIndex1] = mapping2Index;
      _currentColumnMapping.MappedSourceIndexes[mappingSourceIndex2] = mapping1Index;

      // Refresh Grids
      TargetMySqlTableDataGridView.Refresh();
      SourceExcelDataDataGridView.Refresh();
      StoreMappingButton.Enabled = _currentColumnMapping.MappedQuantity > 0;
    }

    /// <summary>
    /// Event delegate method fired when the <see cref="TargetMySqlTableDataGridView"/> receives a drop operation.
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
              MySqlDataColumn toCol = _targetMySqlPreviewDataTable.GetColumnAtIndex(_gridTargetTableColumnIndexToDrop);
              if (!string.IsNullOrEmpty(toCol.MappedDataColName))
              {
                bool isIdenticalMapping = toCol.MappedDataColName == draggedColumnName;
                bool proceedWithMappingOverwriting = true;
                if (!isIdenticalMapping && Settings.Default.AppendConfirmColumnMappingOverwriting)
                {
                  proceedWithMappingOverwriting = InfoDialog.ShowDialog(
                    InfoDialogProperties.GetYesNoDialogProperties(InfoDialog.InfoType.Warning,
                      Resources.ColumnMappedOverwriteTitleWarning, Resources.ColumnMappedOverwriteDetailWarning))
                    .DialogResult == DialogResult.Yes;
                }

                if (proceedWithMappingOverwriting)
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
              if (_currentColumnMapping == null)
              {
                return;
              }

              int mappedIndexFromDraggedTargetColumn = _currentColumnMapping.MappedSourceIndexes[fromColumnIndex];
              int mappedIndexInDropTargetColumn = _currentColumnMapping.MappedSourceIndexes[_gridTargetTableColumnIndexToDrop];
              if (mappedIndexInDropTargetColumn >= 0)
              {
                bool isIdenticalMapping = mappedIndexInDropTargetColumn == mappedIndexFromDraggedTargetColumn;
                DialogResult dr = DialogResult.No;
                if (!isIdenticalMapping)
                {
                  dr = InfoDialog.ShowDialog(InfoDialogProperties.GetYesNoDialogProperties(InfoDialog.InfoType.Warning, Resources.ColumnMappedOverwriteTitleWarning, Resources.ColumnMappedExchangeDetailWarning)).DialogResult;
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
    /// Event delegate method fired when an element is dragged over the <see cref="TargetMySqlTableDataGridView"/> control.
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
        Point clientPoint = TargetMySqlTableDataGridView.PointToClient(new Point(e.X, e.Y));
        DataGridView.HitTestInfo info = TargetMySqlTableDataGridView.HitTest(clientPoint.X, clientPoint.Y);
        _gridTargetTableColumnIndexToDrop = info.ColumnIndex;
      }
      else if ((e.AllowedEffect & DragDropEffects.Move) == DragDropEffects.Move)
      {
        e.Effect = DragDropEffects.Move;
        Point clientPoint = TargetMySqlTableDataGridView.PointToClient(new Point(e.X, e.Y));
        DataGridView.HitTestInfo info = TargetMySqlTableDataGridView.HitTest(clientPoint.X, clientPoint.Y);
        _gridTargetTableColumnIndexToDrop = info.ColumnIndex;
      }
    }

    /// <summary>
    /// Event delegate method fired when the <see cref="SourceExcelDataDataGridView"/> grid selection changes.
    /// </summary>
    /// <param name="sender">Sender object.</param>
    /// <param name="e">Event arguments.</param>
    private void TargetMySQLTableDataGridView_SelectionChanged(object sender, EventArgs e)
    {
      if (TargetMySqlTableDataGridView.SelectedColumns.Count == 0)
      {
        return;
      }

      SetGridColumnWarningVisibility(TargetMySqlTableDataGridView.SelectedColumns[0]);
    }
  }
}