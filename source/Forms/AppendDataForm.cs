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
    private int grdToTableColumnIndexToDrop = -1;
    private int maxMappingCols = 0;
    private Cursor draggingCursor;
    private Cursor trashCursor;
    private Cursor droppableCursor;
    private MySQLColumnMapping currentColumnMapping = null;
    private List<MySQLColumnMapping> storedColumnMappingsList
    {
      get { return new MySQLColumnMappingList().UserColumnMappingsList; }
    
    }    

    public AppendDataForm(MySqlWorkbenchConnection wbConnection, Excel.Range exportDataRange, DBObject importDBObject, string appendingWorksheetName)
    {
      this.wbConnection = wbConnection;
      draggingCursor = MiscUtilities.CreateCursor(new Bitmap(Properties.Resources.MySQLforExcel_Cursor_Dragging_32x32), 3, 3);
      droppableCursor = MiscUtilities.CreateCursor(new Bitmap(Properties.Resources.MySQLforExcel_Cursor_Dropable_32x32), 3, 3);
      trashCursor = MiscUtilities.CreateCursor(new Bitmap(Properties.Resources.MySQLforExcel_Cursor_Trash_32x32), 3, 3);

      InitializeComponent();

      grdFromExcelData.EnableHeadersVisualStyles = false;

      initializeFromTableGrid(importDBObject.Name, exportDataRange);
      initializeToTableGrid(importDBObject);

      string excelRangeAddress = exportDataRange.Address.Replace("$", String.Empty);
      Text = String.Format("Append Data - {0} [{1}]", appendingWorksheetName, excelRangeAddress);
      maxMappingCols = Math.Min(grdToMySQLTable.Columns.Count, grdFromExcelData.Columns.Count);
      clearMappingsOnToTableGridAndMySQLTable();
      loadStoredColumnMappings();
      if (!selectStoredMappingForTargetTable())
        if (Settings.Default.AppendPerformAutoMap)
          cmbMappingMethod.SelectedIndex = 0;
        else
          cmbMappingMethod.SelectedIndex = 1;
    }

    private void initializeFromTableGrid(string fromTableName, Excel.Range excelDataRange)
    {
      fromMySQLDataTable = new MySQLDataTable(fromTableName,
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

      foreach (MySQLColumnMapping mapping in storedColumnMappingsList)
      {
        if (mapping.TableName == toMySQLDataTable.TableName && mapping.AllColumnsMatch(toMySQLDataTable, true))
        {
          cmbMappingMethod.SelectedIndex = cmbMappingMethod.Items.IndexOf(mapping.Name);
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
      
      cmbMappingMethod.SelectedIndex = 0;
    }

    /// <summary>
    /// Loads all Stored Data Mappings from settings file
    /// </summary>
    private void loadStoredColumnMappings()
    {                      
      refreshMappingMethodCombo();
    }

    private string[] getColumnNamesArray(DataTable dt)
    {
      string[] retArray = null;

      if (dt != null && dt.Columns.Count > 0)
      {
        retArray = new string[dt.Columns.Count];
        for (int i = 0; i < dt.Columns.Count; i++)
          retArray[i] = dt.Columns[i].ColumnName;
      }

      return retArray;
    }

    private MySQLColumnMapping createColumnMappingForAutomatic()
    {
      MySQLColumnMapping autoMapping = new MySQLColumnMapping("Automatic", getColumnNamesArray(fromMySQLDataTable), getColumnNamesArray(toMySQLDataTable));
      autoMapping.SchemaName = wbConnection.Schema;
      autoMapping.TableName = toMySQLDataTable.TableName;
      autoMapping.ConnectionName = wbConnection.Name;
      autoMapping.Port = wbConnection.Port;
      int autoMappedColumns = 0;
      
      // Attempt to auto-map using column names if the Excel data contains the column names
      if (chkFirstRowHeaders.Checked)
      {
        for (int toColIdx = 0; toColIdx < toMySQLDataTable.Columns.Count; toColIdx++)
        {
          string targetColName = toMySQLDataTable.Columns[toColIdx].ColumnName;
          string matchSourceName = String.Empty;
          int fromColIdx = -1;
          foreach (DataGridViewColumn gridCol in grdFromExcelData.Columns)
          {
            if (gridCol.HeaderText.ToLowerInvariant() == targetColName.ToLowerInvariant())
            {
              matchSourceName = gridCol.HeaderText;
              fromColIdx = gridCol.Index;
              break;
            }
          }
          if (matchSourceName.Length > 0)
          {
            autoMapping.MappedSourceIndexes[toColIdx] = fromColIdx;
            autoMappedColumns++;
          }
        }
      }

      // Auto-map 1-1 if data types match
      if (autoMappedColumns != maxMappingCols)
      {
        autoMapping.ClearMappings();
        for (int colIdx = 0; colIdx < toMySQLDataTable.Columns.Count; colIdx++)
        {
          if (colIdx >= maxMappingCols)
            break;
          MySQLDataColumn fromCol = fromMySQLDataTable.Columns[colIdx] as MySQLDataColumn;
          MySQLDataColumn toCol = toMySQLDataTable.Columns[colIdx] as MySQLDataColumn;
          if (DataTypeUtilities.Type1FitsIntoType2(toCol.StrippedMySQLDataType, fromCol.StrippedMySQLDataType))
          {
            autoMapping.MappedSourceIndexes[colIdx] = colIdx;
            autoMappedColumns++;
          }
        }
      }

      // If auto-map was not successful return object without mappings
      if (autoMappedColumns != maxMappingCols)
        autoMapping.ClearMappings();

      return autoMapping;
    }

    private MySQLColumnMapping createColumnMappingForManual()
    {
      MySQLColumnMapping manualMapping;
      if (currentColumnMapping == null)
      {
        manualMapping = new MySQLColumnMapping(getColumnNamesArray(fromMySQLDataTable), getColumnNamesArray(toMySQLDataTable));
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
      MySQLDataColumn fromCol;
      if (mapping)
      {
        fromCol = fromMySQLDataTable.Columns[fromColumnIndex] as MySQLDataColumn;
        fromCol.MappedDataColName = toMySQLDataTable.Columns[toColumnIndex].ColumnName;
      }
      else if (previouslyMappedFromIndex >= 0)
      {
        fromCol = fromMySQLDataTable.Columns[previouslyMappedFromIndex] as MySQLDataColumn;
        fromCol.MappedDataColName = null;
      }
      currentColumnMapping.MappedSourceIndexes[toColumnIndex] = fromColumnIndex;
    }

    private void applySelectedStoredColumnMapping()
    {
      if (currentColumnMapping != null)
      {
        if (currentColumnMapping.Name != "Manual")
          clearMappingsOnToTableGridAndMySQLTable();
        for (int mappedIdx = 0; mappedIdx < currentColumnMapping.MappedSourceIndexes.Length; mappedIdx++)
        {
          int currentMappedSourceIndex = currentColumnMapping.MappedSourceIndexes[mappedIdx];
          string currentMappedColName = (currentMappedSourceIndex >= 0 ? currentColumnMapping.SourceColumns[currentMappedSourceIndex] : null);
          applySingleMapping(currentMappedSourceIndex, mappedIdx, currentMappedColName);
        }
        grdToMySQLTable.Refresh();
        grdFromExcelData.Refresh();
      }
      btnStoreMapping.Enabled = currentColumnMapping.MappedQuantity > 0;
    }

    private void clearMappingsOnToTableGridAndMySQLTable()
    {
      bool newMappings = grdToMySQLTable.MultiHeaderColumnList.Count == 0;
      for (int colIdx = 0; colIdx < grdToMySQLTable.Columns.Count; colIdx++)
      {
        if (newMappings)
          grdToMySQLTable.MultiHeaderColumnList.Add(new MultiHeaderColumn(String.Empty, colIdx, colIdx));
        else
          grdToMySQLTable.MultiHeaderColumnList[colIdx].HeaderText = String.Empty;

        grdToMySQLTable.MultiHeaderColumnList[colIdx].BackgroundColor = Color.OrangeRed;

        if (colIdx < grdFromExcelData.Columns.Count)
        {
          DataGridViewCellStyle newStyle = new DataGridViewCellStyle(grdFromExcelData.Columns[colIdx].HeaderCell.Style);
          newStyle.SelectionBackColor = newStyle.BackColor = SystemColors.Control;
          grdFromExcelData.Columns[colIdx].HeaderCell.Style = newStyle;
        }
        MySQLDataColumn fromCol = fromMySQLDataTable.Columns[colIdx] as MySQLDataColumn;
        fromCol.MappedDataColName = null;
      }
      grdToMySQLTable.Refresh();
      grdFromExcelData.Refresh();
      btnStoreMapping.Enabled = false;
    }

    private void performManualSingleColumnMapping(int fromColumnIndex, int toColumnIndex, string mappedColName)
    {
      if (currentColumnMapping.Name == "Automatic")
        cmbMappingMethod.Text = "Manual";

      applySingleMapping(fromColumnIndex, toColumnIndex, mappedColName);

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
      grdFromExcelData.CurrentCell = null;
      foreach (DataGridViewColumn gridCol in grdFromExcelData.Columns)
        gridCol.HeaderText = (firstRowColNames ? grdFromExcelData.Rows[0].Cells[gridCol.Index].Value.ToString() : fromMySQLDataTable.Columns[gridCol.Index].ColumnName);
      grdFromExcelData.Rows[0].Visible = !firstRowColNames;
      if (!(chkFirstRowHeaders.Checked && grdFromExcelData.Rows.Count < 2))
        grdFromExcelData.FirstDisplayedScrollingRowIndex = (chkFirstRowHeaders.Checked ? 1 : 0);
      if (currentColumnMapping != null && currentColumnMapping.Name == "Automatic")
        cmbMappingMethod_SelectedIndexChanged(cmbMappingMethod, EventArgs.Empty);
    }

    private void grdFromExcelData_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
    {
      grdFromExcelData.ClearSelection();
    }

    private void btnAppend_Click(object sender, EventArgs e)
    {
      DialogResult dr;
      if (fromMySQLDataTable.MappedColumnsQuantity < maxMappingCols)
      {
        WarningDialog wDiag = new WarningDialog(Properties.Resources.ColumnMappingIncompleteTitleWarning, Properties.Resources.ColumnMappingIncompleteDetailWarning);
        if (wDiag.ShowDialog() == DialogResult.No)
          return;
      }

      Exception exception;
      string insertQuery;
      string operationSummary = String.Empty;

      bool success = fromMySQLDataTable.InsertDataWithManualQuery(wbConnection, true, out exception, out insertQuery);
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
        operationDetails.Append("Excel data was inserted successfully.");
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
      if (e.Button != MouseButtons.Left)
        return;
      DataGridView gridObject = (sender as DataGridView);
      DataGridView.HitTestInfo info = gridObject.HitTest(e.X, e.Y);
      grdColumnIndexToDrag = info.ColumnIndex;
      if (grdColumnIndexToDrag >= 0)
      {
        // Remember the point where the mouse down occurred. The DragSize indicates the size that the mouse can move before a drag event should be started.  
        Size dragSize = SystemInformation.DragSize;

        // Create a rectangle using the DragSize, with the mouse position being at the center of the rectangle.
        dragBoxFromMouseDown = new Rectangle(new Point(e.X - (dragSize.Width / 2), e.Y - (dragSize.Height / 2)), dragSize);
      }
      else
        // Reset the rectangle if the mouse is not over an item in the ListBox.
        dragBoxFromMouseDown = Rectangle.Empty;
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
          DragDropEffects dropEffect = gridObject.DoDragDrop(grdColumnIndexToDrag, DragDropEffects.Link);
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
          Cursor.Current = (feedBackFromGrid ? droppableCursor : trashCursor);
          break;
        case DragDropEffects.None:
          Cursor.Current = (feedBackFromGrid ? draggingCursor : Cursors.Default);
          break;
        default:
          Cursor.Current = Cursors.Default;
          break;
      }
    }

    private void grdQueryContinueDrag(object sender, QueryContinueDragEventArgs e)
    {
      if ((sender as DataGridView).Name == "grdFromExcelData" && Cursor.Current != Cursors.Default && Cursor.Current != droppableCursor && Cursor.Current != draggingCursor && Cursor.Current != trashCursor)
        Cursor.Current = draggingCursor;

      // Cancel the drag if the mouse moves off the form. The screenOffset takes into account any desktop bands that may be at the top or left side of the screen.
      if (((Control.MousePosition.X - screenOffset.X) < this.DesktopBounds.Left) ||
          ((Control.MousePosition.X - screenOffset.X) > this.DesktopBounds.Right) ||
          ((Control.MousePosition.Y - screenOffset.Y) < this.DesktopBounds.Top) ||
          ((Control.MousePosition.Y - screenOffset.Y) > this.DesktopBounds.Bottom))
      {
        e.Action = DragAction.Cancel;
      }
    }

    private void grdToMySQLTable_DragLeave(object sender, EventArgs e)
    {
      if (grdColumnIndexToDrag >- 1)
        performManualSingleColumnMapping(-1, grdColumnIndexToDrag, null);
    }

    private void grdToMySQLTable_DragDrop(object sender, DragEventArgs e)
    {
      // Ensure that the dragged item is contained in the data.
      if (e.Data.GetDataPresent(typeof(System.Int32)))
      {
        int fromColumnIndex = Convert.ToInt32(e.Data.GetData(typeof(System.Int32)));
        string draggedColumnName = grdFromExcelData.Columns[fromColumnIndex].HeaderText;
        string droppedOntoColumnName = grdToMySQLTable.Columns[grdToTableColumnIndexToDrop].HeaderText;
        if (e.Effect == DragDropEffects.Link && grdToTableColumnIndexToDrop >= 0)
        {
          MySQLDataColumn fromCol = fromMySQLDataTable.Columns[fromColumnIndex] as MySQLDataColumn;
          if (!String.IsNullOrEmpty(fromCol.MappedDataColName))
          {
            bool isIdenticalMapping = fromCol.MappedDataColName == droppedOntoColumnName;
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
        }
      }
      grdToTableColumnIndexToDrop = -1;
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
      e.Effect = DragDropEffects.Link;
      Point clientPoint = grdToMySQLTable.PointToClient(new Point(e.X, e.Y));
      DataGridView.HitTestInfo info = grdToMySQLTable.HitTest(clientPoint.X, clientPoint.Y);
      grdToTableColumnIndexToDrop = info.ColumnIndex;
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
      switch (cmbMappingMethod.Text)
      {
        case "Automatic":
          currentColumnMapping = createColumnMappingForAutomatic();
          if (currentColumnMapping.MappedQuantity == maxMappingCols)
            applySelectedStoredColumnMapping();
          break;
        case "Manual":
          currentColumnMapping = createColumnMappingForManual();
          break;
        default:
          currentColumnMapping.MatchWithOtherColumnMapping(storedColumnMappingsList[cmbMappingMethod.SelectedIndex - 2], false);
          applySelectedStoredColumnMapping();
          break;
      }
    }

  }

}
