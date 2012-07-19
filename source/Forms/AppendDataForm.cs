using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using MySQL.Utility;
using MySql.Data.MySqlClient;
using Excel = Microsoft.Office.Interop.Excel;
using MySQL.ForExcel.Properties;

namespace MySQL.ForExcel
{
  public partial class AppendDataForm : AutoStyleableBaseDialog
  {
    private MySqlWorkbenchConnection wbConnection;
    private MySQLDataTable fromMySQLDataTable = null;
    private MySQLDataTable toMySQLDataTable = null;
    private Rectangle dragBoxFromMouseDown = Rectangle.Empty;
    private Point screenOffset;
    private int grdColumnIndexToDrag = -1;
    private int grdColumnClicked = -1;
    private int grdToTableColumnIndexToDrop = -1;
    private int maxMappingCols = 0;
    private Cursor draggingCursor;
    private Cursor trashCursor;
    private Cursor droppableCursor;
    private MySQLColumnMapping currentColumnMapping = null;
    private MySQLColumnMappingList columnsMappingInFileList = new MySQLColumnMappingList();
    private List<MySQLColumnMapping> storedColumnMappingsList
    {
      get { return columnsMappingInFileList.UserColumnMappingsList; }
    
    }    

    public AppendDataForm(MySqlWorkbenchConnection wbConnection, Excel.Range exportDataRange, DBObject importDBObject, string appendingWorksheetName)
    {
      this.wbConnection = wbConnection;
      draggingCursor = MiscUtilities.CreateCursor(new Bitmap(Properties.Resources.MySQLforExcel_Cursor_Dragging_32x32), 3, 3);
      droppableCursor = MiscUtilities.CreateCursor(new Bitmap(Properties.Resources.MySQLforExcel_Cursor_Dropable_32x32), 3, 3);
      trashCursor = MiscUtilities.CreateCursor(new Bitmap(Properties.Resources.MySQLforExcel_Cursor_Trash_32x32), 3, 3);

      InitializeComponent();

      grdFromExcelData.EnableHeadersVisualStyles = false;

      initializeFromTableGrid(wbConnection.Schema, importDBObject.Name, exportDataRange);
      initializeToTableGrid(importDBObject);

      string excelRangeAddress = exportDataRange.Address.Replace("$", String.Empty);
      Text = String.Format("Append Data - {0} [{1}]", appendingWorksheetName, excelRangeAddress);
      maxMappingCols = Math.Min(grdToMySQLTable.Columns.Count, grdFromExcelData.Columns.Count);
      clearMappings(true);
      refreshMappingMethodCombo();
      if (!selectStoredMappingForTargetTable())
        if (Settings.Default.AppendPerformAutoMap)
          cmbMappingMethod.SelectedIndex = 0;
        else
          cmbMappingMethod.SelectedIndex = 1;
    }

    private void initializeFromTableGrid(string schemaName, string fromTableName, Excel.Range excelDataRange)
    {
      fromMySQLDataTable = new MySQLDataTable(schemaName,
                                              fromTableName,
                                              excelDataRange,
                                              false,
                                              Properties.Settings.Default.AppendUseFormattedValues,
                                              true,
                                              false,
                                              false,
                                              false);
      grdFromExcelData.DataSource = fromMySQLDataTable;
      foreach (DataGridViewColumn gridCol in grdFromExcelData.Columns)
      {
        gridCol.SortMode = DataGridViewColumnSortMode.NotSortable;
      }
      grdFromExcelData.SelectionMode = DataGridViewSelectionMode.FullColumnSelect;
      chkFirstRowHeaders_CheckedChanged(chkFirstRowHeaders, EventArgs.Empty);
    }

    private void initializeToTableGrid(DBObject importDBObject)
    {
      toMySQLDataTable = new MySQLDataTable(importDBObject.Name, true, wbConnection);
      DataTable dt = MySQLDataUtilities.GetDataFromTableOrView(wbConnection, importDBObject, null, 0, 10);
      foreach (DataRow dr in dt.Rows)
      {
        toMySQLDataTable.ImportRow(dr);
      }
      long totalRowsCount = MySQLDataUtilities.GetRowsCountFromTableOrView(wbConnection, importDBObject);
      grdToMySQLTable.DataSource = toMySQLDataTable;
      foreach (DataGridViewColumn gridCol in grdToMySQLTable.Columns)
      {
        gridCol.SortMode = DataGridViewColumnSortMode.NotSortable;
      }
      grdToMySQLTable.SelectionMode = DataGridViewSelectionMode.FullColumnSelect;
    }

    private bool selectStoredMappingForTargetTable()
    {
      bool appliedStoredMapping = false;

      for (int mappingIdx = 0; mappingIdx < storedColumnMappingsList.Count; mappingIdx++)
      {
        MySQLColumnMapping mapping = storedColumnMappingsList[mappingIdx];
        if (mapping.TableName == toMySQLDataTable.TableName && mapping.AllColumnsMatch(toMySQLDataTable, true))
        {
          cmbMappingMethod.SelectedIndex = mappingIdx + 2;
          appliedStoredMapping = true;
          break;
        }
      }

      return appliedStoredMapping;
    }

    private void refreshMappingMethodCombo()
    {
      cmbMappingMethod.Items.Clear();
      cmbMappingMethod.Items.Add("Automatic");
      cmbMappingMethod.Items.Add("Manual");

      if (storedColumnMappingsList != null)
        foreach (MySQLColumnMapping mapping in storedColumnMappingsList)
          cmbMappingMethod.Items.Add(string.Format("{0} ({1}.{2})", mapping.Name, mapping.SchemaName, mapping.TableName));
      
      cmbMappingMethod.SelectedIndex = -1;
    }

    private MySQLColumnMapping createColumnMappingForAutomatic()
    {
      MySQLColumnMapping autoMapping = new MySQLColumnMapping("Automatic", fromMySQLDataTable.GetColumnNamesArray(), toMySQLDataTable.GetColumnNamesArray());
      autoMapping.SchemaName = wbConnection.Schema;
      autoMapping.TableName = toMySQLDataTable.TableName;
      autoMapping.ConnectionName = wbConnection.Name;
      autoMapping.Port = wbConnection.Port;
      int autoMappedColumns = 0;
      
      // Attempt to auto-map using toColumn names regardless of positioning if the data types match
      if (chkFirstRowHeaders.Checked)
      {
        for (int toColIdx = 0; toColIdx < toMySQLDataTable.Columns.Count; toColIdx++)
        {
          string targetColName = toMySQLDataTable.Columns[toColIdx].ColumnName;
          int fromColIdx = fromMySQLDataTable.GetColumnIndex(targetColName, true, false);
          if (fromColIdx >= 0)
          {
            MySQLDataColumn fromCol = fromMySQLDataTable.GetColumnAtIndex(fromColIdx);
            MySQLDataColumn toCol = toMySQLDataTable.GetColumnAtIndex(toColIdx);
            if (DataTypeUtilities.Type1FitsIntoType2(fromCol.StrippedMySQLDataType, toCol.StrippedMySQLDataType))
            {
              autoMapping.MappedSourceIndexes[toColIdx] = fromColIdx;
              autoMappedColumns++;
            }
          }
        }
      }

      // Auto-map 1-1 if just data types match
      if (autoMappedColumns == 0)
      {
        autoMapping.ClearMappings();
        for (int colIdx = 0; colIdx < toMySQLDataTable.Columns.Count; colIdx++)
        {
          if (colIdx >= maxMappingCols)
            break;
          MySQLDataColumn fromCol = fromMySQLDataTable.GetColumnAtIndex(colIdx);
          MySQLDataColumn toCol = toMySQLDataTable.GetColumnAtIndex(colIdx);
          if (DataTypeUtilities.Type1FitsIntoType2(fromCol.StrippedMySQLDataType, toCol.StrippedMySQLDataType))
          {
            autoMapping.MappedSourceIndexes[colIdx] = colIdx;
            autoMappedColumns++;
          }
        }
      }

      return autoMapping;
    }

    private MySQLColumnMapping createColumnMappingForManual()
    {
      MySQLColumnMapping manualMapping;
      if (currentColumnMapping == null)
      {
        manualMapping = new MySQLColumnMapping(fromMySQLDataTable.GetColumnNamesArray(), toMySQLDataTable.GetColumnNamesArray());
        manualMapping.SchemaName = wbConnection.Schema;
        manualMapping.TableName = toMySQLDataTable.TableName;
        manualMapping.ConnectionName = wbConnection.Name;
        manualMapping.Port = wbConnection.Port;
      }
      else
        manualMapping = currentColumnMapping;
      manualMapping.Name = "Manual";
      return manualMapping;
    }

    private void createColumnMappingForStoredMapping()
    {
      // Create a copy of the current stored mapping but with no source columns mapped that we will be doing the best matching on
      MySQLColumnMapping matchedMapping = new MySQLColumnMapping(currentColumnMapping, fromMySQLDataTable.GetColumnNamesArray(), toMySQLDataTable.GetColumnNamesArray());

      // Check if Target Columns still match with the Target Table, switch mapped indexes if columns changed positions
      //  and remove mapped indexes if target toColumn in stored mapping is not present anymore in Target Table
      for (int storedMappedIdx = 0; storedMappedIdx < currentColumnMapping.TargetColumns.Length; storedMappedIdx++)
      {
        // Check if Target Column in Stored Mapping is found within any of the TargetColumns of the matching mapping.
        // If not found we should not map so we skip this Target Column.
        string storedMappedColName = currentColumnMapping.TargetColumns[storedMappedIdx];
        int targetColumnIndex = matchedMapping.GetTargetColumnIndex(storedMappedColName);
        if (targetColumnIndex < 0)
          continue;
        MySQLDataColumn toCol = toMySQLDataTable.GetColumnAtIndex(targetColumnIndex);

        // Check if mapped source toColumn from Stored Mapping matches with a Source Column in current "From Table"
        //  in toColumn name and its data type matches its corresponding target toColumn, if so we are good to map it
        int proposedSourceMapping = currentColumnMapping.MappedSourceIndexes[storedMappedIdx];
        string mappedSourceColName = currentColumnMapping.SourceColumns[proposedSourceMapping];
        int sourceColFoundInFromTableIdx = fromMySQLDataTable.GetColumnIndex(mappedSourceColName, true);
        if (sourceColFoundInFromTableIdx >= 0)
        {
          MySQLDataColumn fromCol = fromMySQLDataTable.GetColumnAtIndex(sourceColFoundInFromTableIdx);
          if (DataTypeUtilities.Type1FitsIntoType2(fromCol.StrippedMySQLDataType, toCol.StrippedMySQLDataType))
            matchedMapping.MappedSourceIndexes[targetColumnIndex] = sourceColFoundInFromTableIdx;
        }
        // Since source columns do not match in name and type, try to match the mapped source toColumn's datatype
        //  with the From toColumn in that source index only if that From Column name is not in any source mapping.
        else if (matchedMapping.MappedSourceIndexes[targetColumnIndex] < 0 && proposedSourceMapping < fromMySQLDataTable.Columns.Count)
        {
          string fromTableColName = fromMySQLDataTable.GetColumnAtIndex(proposedSourceMapping).DisplayName;
          int fromTableColNameFoundInStoredMappingSourceColumnsIdx = currentColumnMapping.GetSourceColumnIndex(fromTableColName);
          if (fromTableColNameFoundInStoredMappingSourceColumnsIdx >= 0
            && fromTableColNameFoundInStoredMappingSourceColumnsIdx != proposedSourceMapping
            && currentColumnMapping.GetMappedSourceIndexIndex(fromTableColNameFoundInStoredMappingSourceColumnsIdx) >= 0)
            continue;
          MySQLDataColumn fromCol = fromMySQLDataTable.GetColumnAtIndex(proposedSourceMapping);
          if (DataTypeUtilities.Type1FitsIntoType2(fromCol.StrippedMySQLDataType, toCol.StrippedMySQLDataType))
            matchedMapping.MappedSourceIndexes[targetColumnIndex] = proposedSourceMapping;
        }
      }

      currentColumnMapping = matchedMapping;
      applySelectedStoredColumnMapping();
    }

    private void applySingleMapping(int fromColumnIndex, int toColumnIndex, string mappedColName)
    {
      int previouslyMappedFromIndex = currentColumnMapping.MappedSourceIndexes[toColumnIndex];
      bool mapping = !String.IsNullOrEmpty(mappedColName) && fromColumnIndex >= 0;
      DataGridViewCellStyle newStyle;

      // Change Text and Style of ToTable Column
      MultiHeaderColumn multiHeaderCol = grdToMySQLTable.MultiHeaderColumnList[toColumnIndex];
      multiHeaderCol.HeaderText = (mapping ? mappedColName : String.Empty);
      multiHeaderCol.BackgroundColor = (mapping ? Color.LightGreen : Color.OrangeRed);

      // Change Style of From Table Column being mapped or unmapped
      if (mapping)
      {
        newStyle = new DataGridViewCellStyle(grdFromExcelData.Columns[fromColumnIndex].HeaderCell.Style);
        newStyle.SelectionBackColor = newStyle.BackColor = Color.LightGreen;
        grdFromExcelData.Columns[fromColumnIndex].HeaderCell.Style = newStyle;
      }
      else if (previouslyMappedFromIndex >= 0 && currentColumnMapping.MappedSourceIndexes.Count(fromIdx => fromIdx == previouslyMappedFromIndex) <= 1)
      {
        newStyle = new DataGridViewCellStyle(grdFromExcelData.Columns[previouslyMappedFromIndex].HeaderCell.Style);
        newStyle.SelectionBackColor = newStyle.BackColor = SystemColors.Control;
        grdFromExcelData.Columns[previouslyMappedFromIndex].HeaderCell.Style = newStyle;
      }

      // Store the actual mapping
      MySQLDataColumn fromCol = (mapping ? fromMySQLDataTable.GetColumnAtIndex(fromColumnIndex) : null);
      MySQLDataColumn toCol = toMySQLDataTable.GetColumnAtIndex(toColumnIndex);
      toCol.MappedDataColName = (mapping ? fromCol.ColumnName : null);

      currentColumnMapping.MappedSourceIndexes[toColumnIndex] = fromColumnIndex;
    }

    private void applySelectedStoredColumnMapping()
    {
      if (currentColumnMapping != null)
      {
        clearMappings(true);

        for (int mappedIdx = 0; mappedIdx < currentColumnMapping.MappedSourceIndexes.Length; mappedIdx++)
        {
          if (mappedIdx >= maxMappingCols)
            break;
          int currentMappedSourceIndex = currentColumnMapping.MappedSourceIndexes[mappedIdx];
          string currentMappedColName = (currentMappedSourceIndex >= 0 ? currentColumnMapping.SourceColumns[currentMappedSourceIndex] : null);
          applySingleMapping(currentMappedSourceIndex, mappedIdx, currentMappedColName);
        }
        grdToMySQLTable.Refresh();
        grdFromExcelData.Refresh();
      }
      btnStoreMapping.Enabled = currentColumnMapping.MappedQuantity > 0;
    }

    private void clearMappings(bool onlyGrids)
    {
      bool newMappings = grdToMySQLTable.MultiHeaderColumnList.Count == 0;
      for (int colIdx = 0; colIdx < grdToMySQLTable.Columns.Count; colIdx++)
      {
        if (newMappings)
          grdToMySQLTable.MultiHeaderColumnList.Add(new MultiHeaderColumn(String.Empty, colIdx, colIdx));
        else
          grdToMySQLTable.MultiHeaderColumnList[colIdx].HeaderText = String.Empty;
        grdToMySQLTable.MultiHeaderColumnList[colIdx].BackgroundColor = Color.OrangeRed;

        MySQLDataColumn toCol = toMySQLDataTable.Columns[colIdx] as MySQLDataColumn;
        toCol.MappedDataColName = null;

        if (colIdx < grdFromExcelData.Columns.Count)
        {
          DataGridViewCellStyle newStyle = new DataGridViewCellStyle(grdFromExcelData.Columns[colIdx].HeaderCell.Style);
          newStyle.SelectionBackColor = newStyle.BackColor = SystemColors.Control;
          grdFromExcelData.Columns[colIdx].HeaderCell.Style = newStyle;
        }        
      }
      if (currentColumnMapping != null && !onlyGrids)
        currentColumnMapping.ClearMappings();
      grdToMySQLTable.Refresh();
      grdFromExcelData.Refresh();
      btnStoreMapping.Enabled = false;
    }

    private void performManualSingleColumnMapping(int fromColumnIndex, int toColumnIndex, string mappedColName)
    {
      if (currentColumnMapping.Name == "Automatic")
        cmbMappingMethod.SelectedIndex = 0;

      applySingleMapping(fromColumnIndex, toColumnIndex, mappedColName);

      // Refresh Grids
      grdToMySQLTable.Refresh();
      grdFromExcelData.Refresh();
      btnStoreMapping.Enabled = currentColumnMapping.MappedQuantity > 0;
    }

    private void swapMappings(int mappingSourceIndex1, int mappingSourceIndex2)
    {
      int mappingsCount = (currentColumnMapping != null ? currentColumnMapping.MappedSourceIndexes.Length : 0);
      if (mappingsCount == 0 || mappingSourceIndex1 < 0 || mappingSourceIndex1 >= mappingsCount || mappingSourceIndex2 < 0 || mappingSourceIndex2 >= mappingsCount)
        return;

      string mapping1ColName = grdToMySQLTable.MultiHeaderColumnList[mappingSourceIndex1].HeaderText;
      int mapping1Index = currentColumnMapping.MappedSourceIndexes[mappingSourceIndex1];
      string mapping2ColName = grdToMySQLTable.MultiHeaderColumnList[mappingSourceIndex2].HeaderText;
      int mapping2Index = currentColumnMapping.MappedSourceIndexes[mappingSourceIndex2];

      applySingleMapping(mapping1Index, mappingSourceIndex2, mapping1ColName);
      applySingleMapping(mapping2Index, mappingSourceIndex1, mapping2ColName);

      currentColumnMapping.MappedSourceIndexes[mappingSourceIndex1] = mapping2Index;
      currentColumnMapping.MappedSourceIndexes[mappingSourceIndex2] = mapping1Index;

      // Refresh Grids
      grdToMySQLTable.Refresh();
      grdFromExcelData.Refresh();
      btnStoreMapping.Enabled = currentColumnMapping.MappedQuantity > 0;
    }

    private bool storeColumnMappingInFile(MySQLColumnMapping mapping)
    {
      bool result = false;
      
      if (!storedColumnMappingsList.Contains(mapping))
      {
        MySQLColumnMappingList userList = new MySQLColumnMappingList();        
        result = userList.Add(mapping);
        if (result) 
          refreshMappingMethodCombo();
      }
      return result;
    }

    private void chkFirstRowHeaders_CheckedChanged(object sender, EventArgs e)
    {
      if (grdFromExcelData.Rows.Count == 0)
        return;
      bool firstRowColNames = chkFirstRowHeaders.Checked;

      // Flag the property in the "From" table
      fromMySQLDataTable.FirstRowIsHeaders = firstRowColNames;

      // Refresh the "From"/"Source" Grid and "From"/"Source" toColumn names in the current mapping
      grdFromExcelData.CurrentCell = null;
      for (int colIdx = 0; colIdx < grdFromExcelData.Columns.Count; colIdx++)
      {
        DataGridViewColumn gridCol = grdFromExcelData.Columns[colIdx];
        gridCol.HeaderText = (firstRowColNames ? grdFromExcelData.Rows[0].Cells[gridCol.Index].Value.ToString() : fromMySQLDataTable.Columns[gridCol.Index].ColumnName);
        if (currentColumnMapping != null)
          currentColumnMapping.SourceColumns[colIdx] = gridCol.HeaderText;
      }
      grdFromExcelData.Rows[0].Visible = !firstRowColNames;
      if (!(chkFirstRowHeaders.Checked && grdFromExcelData.Rows.Count < 2))
        grdFromExcelData.FirstDisplayedScrollingRowIndex = (chkFirstRowHeaders.Checked ? 1 : 0);

      // Refresh the mapped columns in the "To" Grid
      for (int colIdx = 0; colIdx < grdToMySQLTable.MultiHeaderColumnList.Count; colIdx++)
      {
        MultiHeaderColumn multiHeaderCol = grdToMySQLTable.MultiHeaderColumnList[colIdx];
        int mappedSourceIndex = currentColumnMapping.MappedSourceIndexes[colIdx];
        if (!String.IsNullOrEmpty(multiHeaderCol.HeaderText) && mappedSourceIndex >= 0)
          multiHeaderCol.HeaderText = grdFromExcelData.Columns[mappedSourceIndex].HeaderText;
      }
      grdToMySQLTable.Refresh();

      // Re-do the Currently Selected mapping (unless we are on Manual) since now columns may match
      if (currentColumnMapping != null && currentColumnMapping.Name != "Manual")
        cmbMappingMethod_SelectedIndexChanged(cmbMappingMethod, EventArgs.Empty);
    }

    private void grdFromExcelData_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
    {
      grdFromExcelData.ClearSelection();
    }

    private void btnAppend_Click(object sender, EventArgs e)
    {
      DialogResult dr;
      if (toMySQLDataTable.MappedColumnsQuantity < maxMappingCols)
      {
        WarningDialog wDiag = new WarningDialog(Properties.Resources.ColumnMappingIncompleteTitleWarning, Properties.Resources.ColumnMappingIncompleteDetailWarning);
        if (wDiag.ShowDialog() == DialogResult.No)
          return;
      }

      Exception exception;
      string insertQuery;
      string operationSummary = String.Empty;

      int appendCount = toMySQLDataTable.AppendDataWithManualQuery(wbConnection, fromMySQLDataTable, out exception, out insertQuery);
      bool success = exception == null;
      if (success)
        operationSummary = String.Format("Excel data was appended successfully to MySQL Table {0}.", toMySQLDataTable.TableName);
      else
        operationSummary = String.Format("Excel data could not be appended to MySQL Table {0}.", toMySQLDataTable.TableName);
      StringBuilder operationDetails = new StringBuilder();
      operationDetails.AppendFormat("Inserting Excel data in MySQL Table \"{0}\"...{1}{1}", toMySQLDataTable.TableName, Environment.NewLine);
      operationDetails.Append(insertQuery);
      operationDetails.Append(Environment.NewLine);
      operationDetails.Append(Environment.NewLine);
      if (success)
        operationDetails.AppendFormat("{0} rows were appended successfully.", appendCount);
      else
      {
        if (exception is MySqlException)
          operationDetails.AppendFormat("MySQL Error {0}:{1}", (exception as MySqlException).Number, Environment.NewLine);
        else
          operationDetails.AppendFormat("ADO.NET Error:{0}", Environment.NewLine);
        operationDetails.Append(exception.Message);
      }

      InfoDialog infoDialog = new InfoDialog(success, operationSummary, operationDetails.ToString());
      dr = infoDialog.ShowDialog();
      if (dr == DialogResult.Cancel)
        return;

      DialogResult = DialogResult.OK;
      Close();
    }

    private void grdMouseDown(object sender, MouseEventArgs e)
    {
      DataGridView gridObject = (sender as DataGridView);
      DataGridView.HitTestInfo info = gridObject.HitTest(e.X, e.Y);
      grdColumnClicked = -1;
      if (e.Button == MouseButtons.Left)
      {
        grdColumnIndexToDrag = info.ColumnIndex;
        if (grdColumnIndexToDrag >= 0)
        {
          // Remember the point where the mouse down occurred. The DragSize indicates the size that the mouse can move before a drag event should be started.  
          Size dragSize = SystemInformation.DragSize;

          // Create a rectangle using the DragSize, with the mouse position being at the center of the rectangle.
          dragBoxFromMouseDown = new Rectangle(new Point(e.X - (dragSize.Width / 2), e.Y - (dragSize.Height / 2)), dragSize);
        }
        else
          // Reset the rectangle if the mouse is not over an item.
          dragBoxFromMouseDown = Rectangle.Empty;
      }
      else if (e.Button == MouseButtons.Right)
      {
        grdColumnClicked = info.ColumnIndex;
      }
    }

    private void grdMouseUp(object sender, MouseEventArgs e)
    {
      // Reset the drag rectangle when the mouse button is raised.
      dragBoxFromMouseDown = Rectangle.Empty;
    }

    private void grdMouseMove(object sender, MouseEventArgs e)
    {
      if ((e.Button & MouseButtons.Left) == MouseButtons.Left)
      {
        // If the mouse moves outside the rectangle, start the drag.
        if (dragBoxFromMouseDown != Rectangle.Empty && !dragBoxFromMouseDown.Contains(e.X, e.Y))
        {
          DataGridView gridObject = (sender as DataGridView);

          // The screenOffset is used to account for any desktop bands that may be at the top or left side of the screen when determining when to cancel the drag drop operation.
          screenOffset = SystemInformation.WorkingArea.Location;

          // Proceed with the drag-and-drop, passing in the list item.
          switch (gridObject.Name)
          {
            case "grdFromExcelData":
              gridObject.DoDragDrop(grdColumnIndexToDrag, DragDropEffects.Link);
              break;
            case "grdToMySQLTable":
              if (grdColumnIndexToDrag >= 0 && currentColumnMapping != null && currentColumnMapping.MappedSourceIndexes[grdColumnIndexToDrag] >= 0)
                gridObject.DoDragDrop(grdColumnIndexToDrag, DragDropEffects.Move);
              break;
          }
        }
      }
    }

    private void grdGiveFeedback(object sender, GiveFeedbackEventArgs e)
    {
      bool feedBackFromGrid = (sender as DataGridView).Name == "grdFromExcelData";

      e.UseDefaultCursors = false;
      switch (e.Effect)
      {
        case DragDropEffects.Link:
          Cursor.Current = (feedBackFromGrid ? droppableCursor : Cursors.No);
          break;
        case DragDropEffects.Move:
          Cursor.Current = (feedBackFromGrid ? Cursors.No : (grdToTableColumnIndexToDrop >= 0 ? droppableCursor : trashCursor));
          break;
        case DragDropEffects.None:
          Cursor.Current = (feedBackFromGrid ? draggingCursor : trashCursor);
          break;
        default:
          Cursor.Current = Cursors.Default;
          break;
      }
    }

    private void grdQueryContinueDrag(object sender, QueryContinueDragEventArgs e)
    {
      // Cancel the drag if the mouse moves off the form. The screenOffset takes into account any desktop bands that may be at the top or left side of the screen.
      if (((Control.MousePosition.X - screenOffset.X) < this.DesktopBounds.Left) ||
          ((Control.MousePosition.X - screenOffset.X) > this.DesktopBounds.Right) ||
          ((Control.MousePosition.Y - screenOffset.Y) < this.DesktopBounds.Top) ||
          ((Control.MousePosition.Y - screenOffset.Y) > this.DesktopBounds.Bottom))
      {
        e.Action = DragAction.Cancel;
        return;
      }
    }

    private void grdToMySQLTable_DragOver(object sender, DragEventArgs e)
    {
      // Determine whether string data exists in the drop data. If not, then the drop effect reflects that the drop cannot occur.
      if (!e.Data.GetDataPresent(typeof(System.Int32)))
      {
        e.Effect = DragDropEffects.None;
        grdToTableColumnIndexToDrop = -1;
        return;
      }
      if ((e.AllowedEffect & DragDropEffects.Link) == DragDropEffects.Link)
      {
        e.Effect = DragDropEffects.Link;
        Point clientPoint = grdToMySQLTable.PointToClient(new Point(e.X, e.Y));
        DataGridView.HitTestInfo info = grdToMySQLTable.HitTest(clientPoint.X, clientPoint.Y);
        grdToTableColumnIndexToDrop = info.ColumnIndex;
      }
      else if ((e.AllowedEffect & DragDropEffects.Move) == DragDropEffects.Move)
      {
        e.Effect = DragDropEffects.Move;
        Point clientPoint = grdToMySQLTable.PointToClient(new Point(e.X, e.Y));
        DataGridView.HitTestInfo info = grdToMySQLTable.HitTest(clientPoint.X, clientPoint.Y);
        grdToTableColumnIndexToDrop = info.ColumnIndex;
      }
    }

    private void grdToMySQLTable_DragDrop(object sender, DragEventArgs e)
    {
      // Ensure that the dragged item is contained in the data.
      if (e.Data.GetDataPresent(typeof(System.Int32)))
      {
        int fromColumnIndex = Convert.ToInt32(e.Data.GetData(typeof(System.Int32)));
        string draggedColumnName = grdFromExcelData.Columns[fromColumnIndex].HeaderText;
        if (grdToTableColumnIndexToDrop >= 0)
        {
          switch (e.Effect)
          {
            // We are mapping a column from the Source Grid to the Target Grid
            case DragDropEffects.Link:
              MySQLDataColumn toCol = toMySQLDataTable.GetColumnAtIndex(grdToTableColumnIndexToDrop);
              if (!String.IsNullOrEmpty(toCol.MappedDataColName))
              {
                bool isIdenticalMapping = toCol.MappedDataColName == draggedColumnName;
                DialogResult dr = DialogResult.No;
                if (!isIdenticalMapping)
                {
                  WarningDialog wDiag = new WarningDialog(Properties.Resources.ColumnMappedOverwriteTitleWarning, Properties.Resources.ColumnMappedOverwriteDetailWarning);
                  dr = wDiag.ShowDialog();
                }
                if (dr == DialogResult.Yes)
                  performManualSingleColumnMapping(-1, grdToTableColumnIndexToDrop, null);
                else
                {
                  e.Effect = DragDropEffects.None;
                  return;
                }
              }
              performManualSingleColumnMapping(fromColumnIndex, grdToTableColumnIndexToDrop, draggedColumnName);
              break;

            // We are moving a column mapping from a column on the Target Grid to another column in the same grid
            case DragDropEffects.Move:
              if (currentColumnMapping == null)
                return;
              int mappedIndexFromDraggedTargetColumn = currentColumnMapping.MappedSourceIndexes[fromColumnIndex];
              int mappedIndexInDropTargetColumn = currentColumnMapping.MappedSourceIndexes[grdToTableColumnIndexToDrop];
              if (mappedIndexInDropTargetColumn >= 0)
              {
                bool isIdenticalMapping = mappedIndexInDropTargetColumn == mappedIndexFromDraggedTargetColumn;
                DialogResult dr = DialogResult.No;
                if (!isIdenticalMapping)
                {
                  WarningDialog wDiag = new WarningDialog(Properties.Resources.ColumnMappedOverwriteTitleWarning, Properties.Resources.ColumnMappedExchangeDetailWarning);
                  dr = wDiag.ShowDialog();
                }
                if (dr == DialogResult.No)
                {
                  e.Effect = DragDropEffects.None;
                  return;
                }
              }
              swapMappings(fromColumnIndex, grdToTableColumnIndexToDrop);
              break;
          }
        }
      }
      grdToTableColumnIndexToDrop = -1;
    }

    private void contentAreaPanel_QueryContinueDrag(object sender, QueryContinueDragEventArgs e)
    {
      // Cancel the drag if the mouse moves off the form. The screenOffset takes into account any desktop bands that may be at the top or left side of the screen.
      if (((Control.MousePosition.X - screenOffset.X) < this.DesktopBounds.Left) ||
          ((Control.MousePosition.X - screenOffset.X) > this.DesktopBounds.Right) ||
          ((Control.MousePosition.Y - screenOffset.Y) < this.DesktopBounds.Top) ||
          ((Control.MousePosition.Y - screenOffset.Y) > this.DesktopBounds.Bottom))
      {
        e.Action = DragAction.Cancel;
        return;
      }
    }

    private void contentAreaPanel_DragOver(object sender, DragEventArgs e)
    {
      // Determine whether data exists in the drop data. If not, then the drop effect reflects that the drop cannot occur.
      if (!e.Data.GetDataPresent(typeof(System.Int32)))
      {
        e.Effect = DragDropEffects.None;
        return;
      }
      if ((e.AllowedEffect & DragDropEffects.Move) == DragDropEffects.Move)
      {
        e.Effect = DragDropEffects.Move;
        grdToTableColumnIndexToDrop = -1;
      }
    }

    private void contentAreaPanel_DragDrop(object sender, DragEventArgs e)
    {
      if (e.Effect == DragDropEffects.Move && e.Data.GetDataPresent(typeof(System.Int32)))
      {
        if (grdColumnIndexToDrag > -1)
          performManualSingleColumnMapping(-1, grdColumnIndexToDrag, null);
      }
    }

    private void btnStoreMapping_Click(object sender, EventArgs e)
    {
      int numericSuffix = 1;
      string proposedMappingName = String.Empty;
      do
      {
        proposedMappingName = String.Format("{0}Mapping{1}", toMySQLDataTable.TableName, (numericSuffix > 1 ? numericSuffix.ToString() : String.Empty));
        numericSuffix++;
      }
      while (storedColumnMappingsList.Any(mapping => mapping.Name == proposedMappingName));
      AppendNewColumnMappingDialog newColumnMappingDialog = new AppendNewColumnMappingDialog(proposedMappingName);
      DialogResult dr = newColumnMappingDialog.ShowDialog();
      if (dr == DialogResult.Cancel)
        return;

      //initialize connection and dbobject information
      currentColumnMapping.Name = newColumnMappingDialog.ColumnMappingName;
      currentColumnMapping.ConnectionName = wbConnection.Name;
      currentColumnMapping.Port = wbConnection.Port;
      currentColumnMapping.SchemaName = wbConnection.Schema;
      currentColumnMapping.TableName = toMySQLDataTable.TableName;

      storeColumnMappingInFile(currentColumnMapping);
    }

    private void btnAdvanced_Click(object sender, EventArgs e)
    {
      AppendAdvancedOptionsDialog optionsDialog = new AppendAdvancedOptionsDialog();
      DialogResult dr = optionsDialog.ShowDialog();
    }

    private void cmbMappingMethod_SelectedIndexChanged(object sender, EventArgs e)
    {
      switch (cmbMappingMethod.SelectedIndex)
      {
        case 0:
          currentColumnMapping = createColumnMappingForAutomatic();
          applySelectedStoredColumnMapping();
          break;
        case 1:
          currentColumnMapping = createColumnMappingForManual();
          break;
        default:
          currentColumnMapping = new MySQLColumnMapping(storedColumnMappingsList[cmbMappingMethod.SelectedIndex - 2]);
          createColumnMappingForStoredMapping();
          break;
      }
    }

    private void removeColumnMappingToolStripMenuItem_Click(object sender, EventArgs e)
    {
      if (grdColumnClicked > -1)
        performManualSingleColumnMapping(-1, grdColumnClicked, null);
    }

    private void clearAllMappingsToolStripMenuItem_Click(object sender, EventArgs e)
    {
      clearMappings(false);
      if (currentColumnMapping.Name == "Automatic")
        cmbMappingMethod.SelectedIndex = 0;
    }

    private void contextMenu_Opening(object sender, CancelEventArgs e)
    {
      if (grdColumnClicked < 0 || currentColumnMapping == null || currentColumnMapping.MappedQuantity == 0)
        e.Cancel = true;
      contextMenu.Items["removeColumnMappingToolStripMenuItem"].Visible = (grdColumnClicked > -1 && currentColumnMapping.MappedSourceIndexes[grdColumnClicked] >= 0);
      contextMenu.Items["clearAllMappingsToolStripMenuItem"].Visible = (currentColumnMapping != null && currentColumnMapping.MappedQuantity > 0);
    }

  }

}
