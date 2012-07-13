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
    private const uint SWP_NOACTIVATE = 0x0010;

    private Point mouseDownPoint = Point.Empty;
    private MySqlWorkbenchConnection wbConnection;
    private DataTable editingTable = null;
    private Excel.Range editDataRange;
    private bool importedHeaders = false;
    private string queryString = String.Empty;
    private MySQLDataTable editMySQLDataTable;
    private MySqlDataAdapter dataAdapter;
    private MySqlConnection connection;
    private List<string> modifiedCellAddressesList;
    private int commitedCellsOLEColor = ColorTranslator.ToOle(ColorTranslator.FromHtml("#B8E5F7"));
    private int uncommitedCellsOLEColor = ColorTranslator.ToOle(ColorTranslator.FromHtml("#FF8282"));
    private int newRowCellsOLEColor = ColorTranslator.ToOle(ColorTranslator.FromHtml("#FFFCC7"));
    private int defaultCellsOLEColor = ColorTranslator.ToOle(Color.White);
    private long editingRowsQuantity = 0;
    private long editingColsQuantity = 0;

    public Excel.Worksheet EditingWorksheet = null;
    public TaskPaneControl CallerTaskPane;
    public string EditingTableName { get; private set; }
    public IWin32Window ParentWindow { get; set; }

    public EditDataDialog(MySqlWorkbenchConnection wbConnection, Excel.Range editDataRange, DataTable importTable, Excel.Worksheet editingWorksheet)
    {
      InitializeComponent();

      //SetParent(Handle, ParentWindow.Handle);
      this.wbConnection = wbConnection;
      this.editingTable = importTable;
      this.editDataRange = editDataRange;
      EditingTableName = importTable.ExtendedProperties["TableName"].ToString();
      importedHeaders = (bool)importTable.ExtendedProperties["ImportedHeaders"];
      queryString = importTable.ExtendedProperties["QueryString"].ToString();
      editMySQLDataTable = new MySQLDataTable(EditingTableName, true, wbConnection);
      initializeDataAdapter();
      EditingWorksheet = editingWorksheet;
      EditingWorksheet.Change += new Excel.DocEvents_ChangeEventHandler(EditingWorksheet_Change);
      EditingWorksheet.SelectionChange += new Excel.DocEvents_SelectionChangeEventHandler(EditingWorksheet_SelectionChange);
      toolTip.SetToolTip(this, String.Format("Editing data for Table {0} on Worksheet {1}", EditingTableName, editingWorksheet.Name));
      editingRowsQuantity = editingWorksheet.UsedRange.Rows.Count;
      editingColsQuantity = editingWorksheet.UsedRange.Columns.Count;
      Opacity = 0.60;

      if (editDataRange != null)
        modifiedCellAddressesList = new List<string>(editDataRange.Count);
      else
        modifiedCellAddressesList = new List<string>();
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

    private void initializeDataAdapter()
    {
      connection = new MySqlConnection(MySQLDataUtilities.GetConnectionString(wbConnection));
      dataAdapter = new MySqlDataAdapter(this.queryString, connection);
      dataAdapter.UpdateCommand = new MySqlCommand(String.Empty, connection);
      StringBuilder queryString = new StringBuilder();
      StringBuilder wClauseString = new StringBuilder();
      wClauseString.AppendFormat("{0}WHERE{0}", Environment.NewLine);
      StringBuilder setClauseString = new StringBuilder();
      string wClause = String.Empty;
      MySqlParameter updateParam = null;

      string wClauseSeparator = String.Empty;
      string sClauseSeparator = String.Empty;
      queryString.AppendFormat("{1}UPDATE `{0}`{1}SET{1}", EditingTableName, Environment.NewLine);

      foreach (MySQLDataColumn mysqlCol in editMySQLDataTable.Columns)
      {
        MySqlDbType mysqlColType = mysqlCol.MySQLDBType;
        int colSuffix = mysqlCol.Ordinal + 1;

        updateParam = new MySqlParameter(String.Format("@W_Column{0}", colSuffix), mysqlColType);
        updateParam.SourceColumn = mysqlCol.ColumnName;
        updateParam.SourceVersion = DataRowVersion.Original;
        dataAdapter.UpdateCommand.Parameters.Add(updateParam);

        if (mysqlCol.PrimaryKey)
          wClauseString.AppendFormat("{0}`{1}`=@W_Column{2}", (mysqlCol.Ordinal == 0 ? String.Empty : wClauseSeparator), mysqlCol.ColumnName, colSuffix);
        
        if (editMySQLDataTable.PrimaryKey == null)
          wClauseString.AppendFormat("{0}`{1}`=@W_Column{2}", (mysqlCol.Ordinal == 0 ? String.Empty : wClauseSeparator), mysqlCol.ColumnName, colSuffix);

        updateParam = new MySqlParameter(String.Format("@S_Column{0}", colSuffix), mysqlColType);
        updateParam.SourceColumn = mysqlCol.ColumnName;

        dataAdapter.UpdateCommand.Parameters.Add(updateParam);
        setClauseString.AppendFormat("{0}`{1}`=@S_Column{2}", sClauseSeparator, mysqlCol.ColumnName, colSuffix);

        wClauseSeparator = " AND ";
        sClauseSeparator = ",";
      }
      queryString.Append(setClauseString.ToString());
      queryString.Append(wClauseString.ToString());
      dataAdapter.UpdateCommand.CommandText = queryString.ToString();
    }

    private void changeExcelCellsColor(int oleColor)
    {
      Excel.Range modifiedRange = null;
      foreach (string modifiedRangeAddress in modifiedCellAddressesList)
      {
        string[] startAndEndRange = modifiedRangeAddress.Split(new char[] { ':' });
        if (startAndEndRange.Length > 1)
          modifiedRange = EditingWorksheet.get_Range(startAndEndRange[0], startAndEndRange[1]);
        else
          modifiedRange = EditingWorksheet.get_Range(modifiedRangeAddress);
        modifiedRange.Interior.Color = oleColor;
      }
      modifiedCellAddressesList.Clear();
    }

    private void revertDataChanges(bool refreshFromDB)
    {
      if (refreshFromDB)
      {
        editingTable.Clear();
        dataAdapter.Fill(editingTable);
        MySQLDataUtilities.AddExtendedProperties(ref editingTable, queryString, importedHeaders, EditingTableName);
      }
      else
      {
        editingTable.RejectChanges();
      }
      Excel.Range topLeftCell = editDataRange.Cells[1, 1];
      CallerTaskPane.ImportDataTableToExcelAtGivenCell(editingTable, importedHeaders, topLeftCell);
      changeExcelCellsColor(defaultCellsOLEColor);
      btnCommit.Enabled = false;
    }

    private void pushDataChanges()
    {
      int updatedCount = 0;
      bool pushSuccessful = true;
      string operationSummary = String.Format("Edited data for Table {0} was committed to MySQL successfully.", EditingTableName);
      StringBuilder operationDetails = new StringBuilder();
      operationDetails.AppendFormat("Updating data rows...{0}{0}", Environment.NewLine);
      operationDetails.Append(dataAdapter.UpdateCommand.CommandText);
      operationDetails.Append(Environment.NewLine);
      operationDetails.Append(Environment.NewLine);

      try
      {
        DataTable changesTable = editingTable.GetChanges();
        int editingRowsCount = (changesTable != null ? changesTable.Rows.Count : 0);
        updatedCount = dataAdapter.Update(editingTable);
        operationDetails.AppendFormat("{1}{0} rows have been updated successfully.", editingRowsCount, Environment.NewLine);
      }
      catch (MySqlException ex)
      {
        if (chkAutoCommit.Checked)
        {
          System.Diagnostics.Debug.WriteLine(ex.Message);
          return;
        }
        pushSuccessful = false;
        operationSummary = String.Format("Edited data for Table {0} could not be committed to MySQL.", EditingTableName);
        operationDetails.AppendFormat("MySQL Error {0}:{1}", ex.Number, Environment.NewLine);
        operationDetails.Append(ex.Message);
      }

      if (!chkAutoCommit.Checked)
      {
        InfoDialog infoDialog = new InfoDialog(pushSuccessful, operationSummary, operationDetails.ToString());
        infoDialog.StartPosition = FormStartPosition.CenterScreen;
        DialogResult dr = infoDialog.ShowDialog();
        if (dr == DialogResult.Cancel)
          return;
      }
      changeExcelCellsColor(commitedCellsOLEColor);
      btnCommit.Enabled = false;
    }

    private void EditingWorksheet_Change(Excel.Range Target)
    {
      Excel.Range intersectRange = CallerTaskPane.IntersectRanges(editDataRange, Target);
      if (intersectRange == null || intersectRange.Count == 0)
        return;

      //if change was done in the first columnInfoRow and we have headers we won't change the name of the column
      if (intersectRange.Row == 1 && importedHeaders)
        return;

      if (!chkAutoCommit.Checked && !modifiedCellAddressesList.Contains(intersectRange.Address))
        modifiedCellAddressesList.Add(intersectRange.Address);
      intersectRange.Interior.Color = (chkAutoCommit.Checked ? commitedCellsOLEColor : uncommitedCellsOLEColor);
      
      // We subtract from the Excel indexes since they start at 1, Row is subtracted by 2 if we imported headers.
      Excel.Range startCell = (intersectRange.Item[1, 1] as Excel.Range);
      int startDataTableRow = startCell.Row - (importedHeaders ? 2 : 1);
      int startDataTableCol = startCell.Column - 1;

      // Detect if a columnInfoRow was deleted and if so flag a columnInfoRow for deletion
      if (EditingWorksheet.UsedRange.Rows.Count < editingRowsQuantity)
      {
        editingTable.Rows[startDataTableRow].Delete();
        editingRowsQuantity = EditingWorksheet.UsedRange.Rows.Count;
      }
      // Detect if a column was deleted and if so remove the column from the Columns colletion
      else if (EditingWorksheet.UsedRange.Columns.Count < editingColsQuantity)
      {
        editingTable.Columns.RemoveAt(startDataTableCol);
      }
      else
      {
        object[,] formattedArrayFromRange;
        if (intersectRange.Count > 1)
          formattedArrayFromRange = intersectRange.Value as object[,];
        else
        {
          formattedArrayFromRange = new object[2, 2];
          formattedArrayFromRange[1, 1] = intersectRange.Value;
        }
        int startRangeRow = 1;
        if (startDataTableRow < 0)
        {
          for (int colIdx = 1; colIdx <= intersectRange.Columns.Count; colIdx++)
            editingTable.Columns[startDataTableCol + colIdx - 1].ColumnName = formattedArrayFromRange[startRangeRow, colIdx].ToString();
          startDataTableRow++;
          startRangeRow++;
        }
        for (int rowIdx = startRangeRow; rowIdx <= intersectRange.Rows.Count; rowIdx++)
        {
          for (int colIdx = 1; colIdx <= intersectRange.Columns.Count; colIdx++)
          {
            int absRow = startDataTableRow + rowIdx - 1;
            int absCol = startDataTableCol + colIdx - 1;
            editingTable.Rows[absRow][absCol] = formattedArrayFromRange[rowIdx, colIdx];
          }
        }
      }

      btnCommit.Enabled = intersectRange.Count > 0 && !chkAutoCommit.Checked;
      if (chkAutoCommit.Checked)
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
      if (connection != null)
        connection.Close();
      Close();
      CallerTaskPane.TableNameEditFormsHashtable.Remove(EditingTableName);
      CallerTaskPane.WorkSheetEditFormsHashtable.Remove(EditingWorksheet.Name);
      Dispose();
    }

    private void btnRevert_Click(object sender, EventArgs e)
    {
      EditDataRevertDialog reverDialog = new EditDataRevertDialog(chkAutoCommit.Checked);
      DialogResult dr = reverDialog.ShowDialog();
      if (dr == DialogResult.Cancel)
        return;
      revertDataChanges(reverDialog.SelectedAction == EditDataRevertDialog.EditUndoAction.RefreshData);
    }

    private void btnCommit_Click(object sender, EventArgs e)
    {
      pushDataChanges();
    }

    private void chkAutoCommit_CheckedChanged(object sender, EventArgs e)
    {
      btnCommit.Enabled = !chkAutoCommit.Checked && modifiedCellAddressesList != null && modifiedCellAddressesList.Count > 0;
      btnRevert.Enabled = !chkAutoCommit.Checked;
    }

    [DllImport("user32.dll", SetLastError = true)]
    static extern IntPtr SetParent(IntPtr hWndChild, IntPtr hWndNewParent);

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
      SetWindowPos(Handle.ToInt32(), HWND_TOPMOST, Left, Top, Width, Height, SWP_NOACTIVATE);
    }

  }
}
