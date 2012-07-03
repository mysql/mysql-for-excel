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
    private ExportDataHelper exportDataHelper;
    private MySQLTable exportTable { get { return exportDataHelper.ExportTable; } }
    private List<ColumnGuessData> headerRowColumnsGuessData { get { return exportDataHelper.HeaderRowColumnsGuessData; } }
    private List<ColumnGuessData> dataRowsColumnsGuessData { get { return exportDataHelper.DataRowsColumnsGuessData; } }
    private DataTable formattedExcelData { get { return exportDataHelper.FormattedExcelData; } }
    private DataTable unformattedExcelData { get { return exportDataHelper.UnformattedExcelData; } }
    private DataTable previewDataTable = null;
    private Rectangle dragBoxFromMouseDown = Rectangle.Empty;
    private Point screenOffset;
    private int grdPreviewColumnIndexToDrag = -1;
    private int grdToTableColumnIndexToDrop = -1;
    private int maxMappingCols = 0;
    private Cursor linkCursor;
    private MySQLColumnMapping currentColumnMapping = null;
    private List<MySQLColumnMapping> storedColumnMappingsList;

    public AppendDataForm(MySqlWorkbenchConnection wbConnection, Excel.Range exportDataRange, DBObject importDBObject)
    {
      this.wbConnection = wbConnection;
      linkCursor = Utilities.CreateCursor(new Bitmap(Properties.Resources.chain_link_24x24), 3, 3);

      InitializeComponent();

      grdPreviewData.EnableHeadersVisualStyles = false;
      exportDataHelper = new ExportDataHelper(wbConnection, exportDataRange, importDBObject.Name);
      initializeToTableGrid(importDBObject);
      Text = String.Format("Append Data [{0}])", exportDataRange.Address.Replace("$", String.Empty));
      changeFormattedDataSource();
      chkFirstRowHeaders_CheckedChanged(chkFirstRowHeaders, EventArgs.Empty);
      maxMappingCols = Math.Min(grdToTable.Columns.Count, grdPreviewData.Columns.Count);
      performOneToOneColumnMapping(false);
      loadStoredColumnMappings();
      if (Settings.Default.AppendPerformAutoMap)
        cmbMappingMethod.SelectedIndex = 0;
      else if (!selectStoredMappingForTargetTable())
        cmbMappingMethod.SelectedIndex = 1;
    }

    private bool selectStoredMappingForTargetTable()
    {
      bool appliedStoredMapping = false;

      foreach (MySQLColumnMapping mapping in storedColumnMappingsList)
      {
        if (mapping.TableName == previewDataTable.TableName && mapping.AllColumnsMatch(previewDataTable, true))
        {
          cmbMappingMethod.SelectedIndex = cmbMappingMethod.Items.IndexOf(mapping.Name);
          appliedStoredMapping = true;
          break;
        }
      }

      return appliedStoredMapping;
    }

    private void initializeToTableGrid(DBObject importDBObject)
    {
      previewDataTable = Utilities.GetDataFromTableOrView(wbConnection, importDBObject, null, 0, 10);
      long totalRowsCount = Utilities.GetRowsCountFromTableOrView(wbConnection, importDBObject);
      grdToTable.DataSource = previewDataTable;
      foreach (DataGridViewColumn gridCol in grdToTable.Columns)
      {
        gridCol.SortMode = DataGridViewColumnSortMode.NotSortable;
      }
      grdToTable.SelectionMode = DataGridViewSelectionMode.FullColumnSelect;
    }

    private void refreshMappingMethodCombo()
    {
      cmbMappingMethod.Items.Clear();
      cmbMappingMethod.Items.Add("Automatic");
      cmbMappingMethod.Items.Add("Manual");

      if (storedColumnMappingsList != null)
        foreach (MySQLColumnMapping mapping in storedColumnMappingsList)
          cmbMappingMethod.Items.Add(mapping.Name);
      
      cmbMappingMethod.SelectedIndex = 0;
    }

    private void loadStoredColumnMappings()
    {
      storedColumnMappingsList = new List<MySQLColumnMapping>();

      // TODO: Insert logic here to read from \users\<username>\AppData\Local\MySQL For Excel\column_mappings.xml and fill storedColumnMappingsList

      refreshMappingMethodCombo();
    }

    private MySQLColumnMapping createColumnMappingForAutomatic()
    {
      MySQLColumnMapping autoMapping = new MySQLColumnMapping();
      autoMapping.Name = "Automatic";
      autoMapping.SchemaName = wbConnection.Schema;
      autoMapping.TableName = exportDataHelper.ExportTable.Name;
      int autoMappedColumns = 0;
      
      // Attempt to auto-map using column names if the Excel data contains the column names
      if (chkFirstRowHeaders.Checked)
      {
        for (int colIdx = 0; colIdx < previewDataTable.Columns.Count; colIdx++)
        {
          if (colIdx >= maxMappingCols)
            break;
          string targetColName = previewDataTable.Columns[colIdx].ColumnName;
          string matchSourceName = String.Empty;
          foreach (DataGridViewColumn gridCol in grdPreviewData.Columns)
          {
            if (gridCol.HeaderText.ToLowerInvariant() == targetColName)
            {
              matchSourceName = gridCol.HeaderText;
              break;
            }
          }
          if (matchSourceName.Length > 0)
          {
            autoMapping.SourceColumns.Add(matchSourceName);
            autoMapping.TargetColumns.Add(targetColName);
            autoMappedColumns++;
          }
        }
      }

      // Auto-map 1-1 if data types match
      if (autoMappedColumns != maxMappingCols)
      {
        autoMapping.SourceColumns.Clear();
        autoMapping.TargetColumns.Clear();
        for (int colIdx = 0; colIdx < previewDataTable.Columns.Count; colIdx++)
        {
          if (colIdx >= maxMappingCols)
            break;
          string targetColName = previewDataTable.Columns[colIdx].ColumnName;
          string matchSourceName = String.Empty;
          if (exportDataHelper.ExportTable.Columns[colIdx].DataType.ToLowerInvariant() == exportDataHelper.DataRowsColumnsGuessData[colIdx].MySQLType.ToLowerInvariant())
          {
            autoMapping.SourceColumns.Add(matchSourceName);
            autoMapping.TargetColumns.Add(targetColName);
            autoMappedColumns++;
          }
        }
      }

      // If auto-map was not successful return object without mappings
      if (autoMappedColumns != maxMappingCols)
      {
        autoMapping.SourceColumns.Clear();
        autoMapping.TargetColumns.Clear();
      }

      return autoMapping;
    }

    private MySQLColumnMapping createColumnMappingForManual()
    {
      MySQLColumnMapping manualMapping;
      if (currentColumnMapping == null)
      {
        manualMapping = new MySQLColumnMapping();
        manualMapping.SchemaName = wbConnection.Schema;
        manualMapping.TableName = exportDataHelper.ExportTable.Name;
      }
      else
        manualMapping = currentColumnMapping;
      manualMapping.Name = "Manual";
      return manualMapping;
    }

    private void applySelectedStoredColumnMapping()
    {

    }

    private void performOneToOneColumnMapping(bool mapped)
    {
      bool newMappings = grdToTable.MultiHeaderColumnList.Count == 0;

      for (int colIdx = 0; colIdx < grdToTable.Columns.Count; colIdx++)
      {
        if (!newMappings && colIdx >= maxMappingCols)
          break;
        string colHeadText = (mapped ? grdPreviewData.Columns[colIdx].HeaderText : String.Empty);
        string mappedColName = (mapped ? grdPreviewData.Columns[colIdx].HeaderText : null);
        Color backColor = (mapped ? Color.LightGreen : Color.OrangeRed);
        if (newMappings)
          grdToTable.MultiHeaderColumnList.Add(new MultiHeaderColumn(colHeadText, colIdx, colIdx));
        else
          grdToTable.MultiHeaderColumnList[colIdx].HeaderText = colHeadText;

        grdToTable.MultiHeaderColumnList[colIdx].BackgroundColor = backColor;

        if (colIdx < grdPreviewData.Columns.Count)
        {
          DataGridViewCellStyle newStyle = new DataGridViewCellStyle(grdPreviewData.Columns[colIdx].HeaderCell.Style);
          newStyle.BackColor = (mapped ? Color.LightGreen : SystemColors.Control);
          grdPreviewData.Columns[colIdx].HeaderCell.Style = newStyle;
        }
        exportTable.Columns[colIdx].MappedDataColName = mappedColName;
      }
      grdToTable.Refresh();
      grdPreviewData.Refresh();
      grdToTable_SelectionChanged(grdToTable, EventArgs.Empty);
    }

    private void performManualSingleColumnMapping(int columnIndex, string mappedColName)
    {
      string previouslyMappedColName = exportTable.Columns[columnIndex].MappedDataColName;
      bool nullMappedColName = String.IsNullOrEmpty(mappedColName);

      MultiHeaderColumn multiHeaderCol = grdToTable.MultiHeaderColumnList[columnIndex];
      multiHeaderCol.HeaderText = (nullMappedColName ? String.Empty : mappedColName);
      multiHeaderCol.BackgroundColor = (nullMappedColName ? Color.OrangeRed : Color.LightGreen);
      exportTable.Columns[columnIndex].MappedDataColName = mappedColName;

      if (!String.IsNullOrEmpty(previouslyMappedColName))
      {
        DataGridViewCellStyle newStyle = new DataGridViewCellStyle(grdPreviewData.Columns[previouslyMappedColName].HeaderCell.Style);
        newStyle.BackColor = (nullMappedColName ? SystemColors.Control : Color.LightGreen);
        grdPreviewData.Columns[previouslyMappedColName].HeaderCell.Style = newStyle;
      }

      grdToTable.Refresh();
      grdPreviewData.Refresh();
      grdToTable_SelectionChanged(grdToTable, EventArgs.Empty);
    }

    private void changeFormattedDataSource()
    {
      grdPreviewData.DataSource = (Settings.Default.AppendUseFormattedValues ? formattedExcelData : unformattedExcelData);
      foreach (DataGridViewColumn gridCol in grdPreviewData.Columns)
      {
        gridCol.SortMode = DataGridViewColumnSortMode.NotSortable;
      }
      grdPreviewData.SelectionMode = DataGridViewSelectionMode.FullColumnSelect;
      chkFirstRowHeaders_CheckedChanged(chkFirstRowHeaders, EventArgs.Empty);
    }

    private void chkFirstRowHeaders_CheckedChanged(object sender, EventArgs e)
    {
      if (grdPreviewData.Rows.Count == 0)
        return;
      bool firstRowColNames = chkFirstRowHeaders.Checked;
      grdPreviewData.CurrentCell = null;
      foreach (DataGridViewColumn gridCol in grdPreviewData.Columns)
        gridCol.HeaderText = (firstRowColNames ? grdPreviewData.Rows[0].Cells[gridCol.Index].Value.ToString() : formattedExcelData.Columns[gridCol.Index].ColumnName);
      grdPreviewData.Rows[0].Visible = !firstRowColNames;
      if (chkFirstRowHeaders.Checked && grdPreviewData.Rows.Count < 2)
        return;
    }

    private void grdPreviewData_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
    {
      grdPreviewData.ClearSelection();
    }

    private void grdPreviewData_SelectionChanged(object sender, EventArgs e)
    {
      btnRemove.Enabled = grdPreviewData.SelectedColumns.Count > 0;
    }

    private void grdToTable_SelectionChanged(object sender, EventArgs e)
    {
      bool anySelected = grdToTable.SelectedColumns.Count > 0;
      string mappedColName = (anySelected ? exportTable.Columns[grdToTable.SelectedColumns[0].DisplayIndex].MappedDataColName : null);
      btnUnmap.Enabled = anySelected && !String.IsNullOrEmpty(mappedColName);
    }

    private void btnAppend_Click(object sender, EventArgs e)
    {
      if (exportTable.Columns.Count(col => !String.IsNullOrEmpty(col.MappedDataColName)) < maxMappingCols)
      {
        DialogResult dr = Utilities.ShowWarningBox(Properties.Resources.ColumnMappingIncomplete);
        if (dr == DialogResult.No)
          return;
      }
      bool success = exportDataHelper.InsertData(chkFirstRowHeaders.Checked, Settings.Default.AppendUseFormattedValues);
      if (success)
      {
        DialogResult = DialogResult.OK;
        Close();
      }
    }

    private void btnUnmap_Click(object sender, EventArgs e)
    {
      performManualSingleColumnMapping(grdToTable.SelectedColumns[0].DisplayIndex, null);
    }

    private void btnRemove_Click(object sender, EventArgs e)
    {
      if (grdPreviewData.SelectedColumns.Count > 0)
      {
        string selectedPreviewColName = grdPreviewData.SelectedColumns[0].HeaderText;
        int toTableMappedIndex = exportTable.Columns.FindIndex(col => col.MappedDataColName == selectedPreviewColName);
        if (toTableMappedIndex >= 0)
        {
          DialogResult dr = Utilities.ShowWarningBox(Properties.Resources.ColumnMappedRemove);
          if (dr == DialogResult.Yes)
            performManualSingleColumnMapping(toTableMappedIndex, null);
          else
            return;
        }
        else
        {
          DialogResult dr = Utilities.ShowWarningBox(Properties.Resources.RemoveColumnConfirmation);
          if (dr != DialogResult.Yes)
            return;
        }
        int removeColdIndex = grdPreviewData.SelectedColumns[0].DisplayIndex;
        formattedExcelData.Columns.RemoveAt(removeColdIndex);
        unformattedExcelData.Columns.RemoveAt(removeColdIndex);
        headerRowColumnsGuessData.RemoveAt(removeColdIndex);
        dataRowsColumnsGuessData.RemoveAt(removeColdIndex);
        grdPreviewData.Refresh();
        grdPreviewData.ClearSelection();
      }
    }

    private void btnAutoMap_Click(object sender, EventArgs e)
    {
      performOneToOneColumnMapping(true);
    }

    private void grdPreviewData_MouseDown(object sender, MouseEventArgs e)
    {
      if (e.Button != MouseButtons.Left)
        return;
      DataGridView.HitTestInfo info = grdPreviewData.HitTest(e.X, e.Y);
      grdPreviewColumnIndexToDrag = info.ColumnIndex;
      if (grdPreviewColumnIndexToDrag >= 0)
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

    private void grdPreviewData_MouseUp(object sender, MouseEventArgs e)
    {
      // Reset the drag rectangle when the mouse button is raised.
      dragBoxFromMouseDown = Rectangle.Empty;
    }

    private void grdPreviewData_MouseMove(object sender, MouseEventArgs e)
    {
      if ((e.Button & MouseButtons.Left) == MouseButtons.Left)
      {
        // If the mouse moves outside the rectangle, start the drag.
        if (dragBoxFromMouseDown != Rectangle.Empty && !dragBoxFromMouseDown.Contains(e.X, e.Y))
        {
          // The screenOffset is used to account for any desktop bands that may be at the top or left side of the screen when determining when to cancel the drag drop operation.
          screenOffset = SystemInformation.WorkingArea.Location;

          // Proceed with the drag-and-drop, passing in the list item.                    
          DragDropEffects dropEffect = grdPreviewData.DoDragDrop(grdPreviewData.Columns[grdPreviewColumnIndexToDrag].HeaderText, DragDropEffects.All | DragDropEffects.Link);
        }
      }
    }

    private void grdPreviewData_GiveFeedback(object sender, GiveFeedbackEventArgs e)
    {
      System.Diagnostics.Debug.WriteLine(String.Format("grdPreviewData_GiveFeedback - e.Effect: {0}", e.Effect.ToString()));
      e.UseDefaultCursors = false;
      if ((e.Effect & DragDropEffects.Link) == DragDropEffects.Link)
        Cursor.Current = linkCursor;
      else
        Cursor.Current = Cursors.Default;
    }

    private void grdPreviewData_QueryContinueDrag(object sender, QueryContinueDragEventArgs e)
    {
      // Cancel the drag if the mouse moves off the form. The screenOffset takes into account any desktop bands that may be at the top or left side of the screen.
      if (((Control.MousePosition.X - screenOffset.X) < this.DesktopBounds.Left) ||
          ((Control.MousePosition.X - screenOffset.X) > this.DesktopBounds.Right) ||
          ((Control.MousePosition.Y - screenOffset.Y) < this.DesktopBounds.Top) ||
          ((Control.MousePosition.Y - screenOffset.Y) > this.DesktopBounds.Bottom))
      {
        e.Action = DragAction.Cancel;
      }
    }

    private void grdToTable_GridDragDrop(object sender, DragEventArgs e)
    {
      // Ensure that the dragged item is contained in the data.
      if (e.Data.GetDataPresent(typeof(System.String)))
      {
        string draggedColumnName = (string)e.Data.GetData(typeof(System.String));
        if (e.Effect == DragDropEffects.Link && grdToTableColumnIndexToDrop >= 0)
        {
          if (!String.IsNullOrEmpty(exportTable.Columns[grdToTableColumnIndexToDrop].MappedDataColName))
          {
            bool isIdenticalMapping = exportTable.Columns[grdToTableColumnIndexToDrop].MappedDataColName == draggedColumnName;
            DialogResult dr = (isIdenticalMapping ? DialogResult.No : Utilities.ShowWarningBox(Properties.Resources.ColumnMappedOverwrite));
            if (dr == DialogResult.Yes)
              performManualSingleColumnMapping(grdToTableColumnIndexToDrop, null);
            else
            {
              e.Effect = DragDropEffects.None;
              return;
            }
          }
          performManualSingleColumnMapping(grdToTableColumnIndexToDrop, draggedColumnName);
        }
      }
      grdToTableColumnIndexToDrop = -1;
    }

    private void grdToTable_GridDragOver(object sender, DragEventArgs e)
    {
      // Determine whether string data exists in the drop data. If not, then the drop effect reflects that the drop cannot occur.
      if (!e.Data.GetDataPresent(typeof(System.String)))
      {
        e.Effect = DragDropEffects.None;
        grdToTableColumnIndexToDrop = -1;
        return;
      }
      e.Effect = DragDropEffects.Link;
      Point clientPoint = grdToTable.PointToClient(new Point(e.X, e.Y));
      DataGridView.HitTestInfo info = grdToTable.HitTest(clientPoint.X, clientPoint.Y);
      grdToTableColumnIndexToDrop = info.ColumnIndex;
    }

    private void btnStoreMapping_Click(object sender, EventArgs e)
    {
      AppendNewColumnMappingDialog newColumnMappingDialog = new AppendNewColumnMappingDialog();
      DialogResult dr = newColumnMappingDialog.ShowDialog();
      if (dr == DialogResult.Cancel)
        return;
      MessageBox.Show(String.Format("Mapping Name = {0}, REPLACE THIS BY STORING LOGIC", newColumnMappingDialog.ColumnMappingName));
      refreshMappingMethodCombo();
    }

    private void btnAdvanced_Click(object sender, EventArgs e)
    {
      bool previousUseFormattedValue = Settings.Default.AppendUseFormattedValues;
      AppendAdvancedOptionsDialog optionsDialog = new AppendAdvancedOptionsDialog();
      DialogResult dr = optionsDialog.ShowDialog();
      if (dr == DialogResult.Cancel)
        return;
      if (previousUseFormattedValue != Settings.Default.AppendUseFormattedValues)
        changeFormattedDataSource();
    }

    private void cmbMappingMethod_SelectedIndexChanged(object sender, EventArgs e)
    {
      switch (cmbMappingMethod.Text)
      {
        case "Automatic":
          currentColumnMapping = createColumnMappingForAutomatic();
          if (currentColumnMapping.TargetColumns.Count < previewDataTable.Columns.Count)
            cmbMappingMethod.SelectedIndex = 0;
          else
            applySelectedStoredColumnMapping();
          break;
        case "Manual":
          currentColumnMapping = createColumnMappingForManual();
          applySelectedStoredColumnMapping();
          break;
        default:
          currentColumnMapping = storedColumnMappingsList[cmbMappingMethod.SelectedIndex - 2];
          applySelectedStoredColumnMapping();
          break;
      }
    }

  }

  public class MySQLColumnMapping
  {
    public string Name { get; set; }
    public string SchemaName { get; set; }
    public string TableName { get; set; }
    public List<string> SourceColumns { get; set; }
    public List<string> TargetColumns { get; set; }

    public MySQLColumnMapping(string mappingName)
    {
      Name = mappingName;
      SchemaName = String.Empty;
      TableName = String.Empty;
      SourceColumns = new List<string>();
      TargetColumns = new List<string>();
    }

    public MySQLColumnMapping() : this(String.Empty)
    {
    }

    public int GetMatchingColumnsQuantity(DataTable dataTable, bool sameOrdinals)
    {
      int matchingColumnsQty = 0;
      if (dataTable != null)
      {
        for (int colIdx = 0; colIdx < TargetColumns.Count; colIdx++)
        {
          string colName = TargetColumns[colIdx];
          if (sameOrdinals)
          {
            if (dataTable.Columns[colIdx].ColumnName.ToLowerInvariant() == colName.ToLowerInvariant())
              matchingColumnsQty++;
          }
          else
          {
            if (dataTable.Columns.Contains(colName))
              matchingColumnsQty++;
          }
        }
      }
      return matchingColumnsQty;
    }

    public bool AllColumnsMatch(DataTable dataTable, bool sameOrdinals)
    {
      return Math.Min(TargetColumns.Count, dataTable.Columns.Count) == GetMatchingColumnsQuantity(dataTable, sameOrdinals);
    }
  }

}
