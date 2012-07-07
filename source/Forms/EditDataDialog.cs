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

namespace MySQL.ForExcel
{
  public partial class EditDataDialog : AutoStyleableBaseForm
  {
    private Point mouseDownPoint = Point.Empty;
    private MySqlWorkbenchConnection wbConnection;
    private DataTable editingTable = null;
    private Excel.Range editDataRange;
    private bool importedHeaders = false;
    private string queryString = String.Empty;
    private string tableName = String.Empty;
    private MySQLTable editMySQLTable;
    private MySqlDataAdapter dataAdapter;
    private MySqlConnection connection;
    public Excel.Worksheet EditingWorksheet = null;
    public TaskPaneControl CallerTaskPane;

    public EditDataDialog(MySqlWorkbenchConnection wbConnection, Excel.Range editDataRange, DataTable importTable, Excel.Worksheet editingWorksheet)
    {
      InitializeComponent();

      this.wbConnection = wbConnection;
      this.editingTable = importTable;
      this.editDataRange = editDataRange;
      tableName = importTable.ExtendedProperties["TableName"].ToString();
      importedHeaders = (bool)importTable.ExtendedProperties["ImportedHeaders"];
      queryString = importTable.ExtendedProperties["QueryString"].ToString();
      getMySQLTableSchemaInfo(tableName);
      initializeDataAdapter();
      EditingWorksheet = editingWorksheet;
      EditingWorksheet.Change += new Excel.DocEvents_ChangeEventHandler(EditingWorksheet_Change);
      EditingWorksheet.SelectionChange += new Excel.DocEvents_SelectionChangeEventHandler(EditingWorksheet_SelectionChange);
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

    private void getMySQLTableSchemaInfo(string tableName)
    {
      DataTable tablesData = Utilities.GetSchemaCollection(wbConnection, "Tables", null, wbConnection.Schema, tableName);
      if (tablesData.Rows.Count == 0)
      {
        System.Diagnostics.Debug.WriteLine(String.Format("Schema info for table {0} not found.", tableName));
        return;
      }
      DataTable columnsData = Utilities.GetSchemaCollection(wbConnection, "Columns", null, wbConnection.Schema, tableName);
      editMySQLTable = new MySQLTable(wbConnection, tablesData.Rows[0], columnsData);
    }

    private void initializeDataAdapter()
    {
      connection = new MySqlConnection(Utilities.GetConnectionString(wbConnection));
      dataAdapter = new MySqlDataAdapter(this.queryString, connection);
      dataAdapter.UpdateCommand = new MySqlCommand(String.Empty, connection);
      StringBuilder queryString = new StringBuilder();
      StringBuilder wClauseString = new StringBuilder(" WHERE ");
      StringBuilder setClauseString = new StringBuilder();
      string wClause = String.Empty;
      MySqlParameter updateParam = null;

      string wClauseSeparator = String.Empty;
      string sClauseSeparator = String.Empty;
      queryString.AppendFormat("USE {0}; UPDATE", wbConnection.Schema);
      queryString.AppendFormat(" {0} SET ", editMySQLTable.Name);

      foreach (MySQLColumn mysqlCol in editMySQLTable.Columns)
      {
        bool isPrimaryKeyColumn = editMySQLTable.PrimaryKey != null && editMySQLTable.PrimaryKey.Columns.Any(idx => idx.ColumnName == mysqlCol.ColumnName);
        MySqlDbType mysqlColType = Utilities.NameToType(mysqlCol.DataType, mysqlCol.IsUnsigned, false);

        updateParam = new MySqlParameter(String.Format("@W_{0}", mysqlCol.ColumnName), mysqlColType);
        updateParam.SourceColumn = mysqlCol.ColumnName;
        updateParam.SourceVersion = DataRowVersion.Original;
        dataAdapter.UpdateCommand.Parameters.Add(updateParam);
        wClauseString.AppendFormat("{0}{1}=@W_{1}", wClauseSeparator, mysqlCol.ColumnName);

        if (!isPrimaryKeyColumn)
        {
          updateParam = new MySqlParameter(String.Format("@S_{0}", mysqlCol.ColumnName), mysqlColType);
          updateParam.SourceColumn = mysqlCol.ColumnName;
          dataAdapter.UpdateCommand.Parameters.Add(updateParam);
          setClauseString.AppendFormat("{0}{1}=@S_{1}", sClauseSeparator, mysqlCol.ColumnName);
        }
        wClauseSeparator = " AND ";
        sClauseSeparator = ",";
      }
      queryString.Append(setClauseString.ToString());
      queryString.Append(wClauseString.ToString());
      dataAdapter.UpdateCommand.CommandText = queryString.ToString();
    }

    private void revertDataChanges(bool refetchFromDB)
    {
      if (refetchFromDB)
      {
        //editingTable = Utilities.GetDataFromTableOrView(wbConnection, editingTable.ExtendedProperties["QueryString"].ToString());
        editingTable.Clear();
        dataAdapter.Fill(editingTable);
        Utilities.AddExtendedProperties(ref editingTable, queryString, importedHeaders, tableName);
        Excel.Range topLeftCell = editDataRange.Cells[1, 1];
        CallerTaskPane.ImportDataTableToExcelAtGivenCell(editingTable, importedHeaders, topLeftCell);
      }
      else
      {
        editingTable.RejectChanges();
      }
    }

    private void pushDataChanges()
    {
      int updatedCount = 0;
      try
      {
        updatedCount = dataAdapter.Update(editingTable);
      }
      catch (Exception ex)
      {
        System.Diagnostics.Debug.WriteLine(ex.Message);
      }
      if (updatedCount == 0)
      {
        System.Diagnostics.Debug.WriteLine("Problem with Adapter Update, no rows were updated.");
        return;
      }
      //for (int rowIdx = 0; rowIdx < grdPreview.Rows.Count; rowIdx++)
      //{
      //  for (int colIdx = 0; colIdx < grdPreview.Columns.Count; colIdx++)
      //  {
      //    if (grdPreview.Rows[rowIdx].Cells[colIdx].Style.BackColor == Color.OrangeRed)
      //      grdPreview.Rows[rowIdx].Cells[colIdx].Style.BackColor = Color.LightGreen;
      //  }
      //}
    }

    private void EditingWorksheet_Change(Excel.Range Target)
    {
      Excel.Range intersectRange = CallerTaskPane.IntersectRanges(editDataRange, Target);
      if (intersectRange == null || intersectRange.Count == 0)
        return;
      Excel.Range startCell = (intersectRange.Item[1, 1] as Excel.Range);
      string startCellAddress = startCell.Address;
      int startRow = startCell.Row - (importedHeaders ? 1 : 0) - 1;
      int startCol = startCell.Column - 1;
      object[,] formattedArrayFromRange;
      if (intersectRange.Count > 1)
        formattedArrayFromRange = intersectRange.Value as object[,];
      else
      {
        formattedArrayFromRange = new object[2, 2];
        formattedArrayFromRange[1, 1] = intersectRange.Value;
      }
      for (int rowIdx = 0; rowIdx < intersectRange.Rows.Count; rowIdx++)
      {
        for (int colIdx = 0; colIdx < intersectRange.Columns.Count; colIdx++)
        {
          int absRow = startRow + rowIdx;
          int absCol = startCol + colIdx;
          editingTable.Rows[absRow][absCol] = formattedArrayFromRange[rowIdx + 1, colIdx + 1];
          //grdPreview.Rows[absRow].Cells[absCol].Style.BackColor = Color.OrangeRed;
        }
      }
      if (chkAutoCommit.Checked)
        pushDataChanges();
    }

    void EditingWorksheet_SelectionChange(Excel.Range Target)
    {
      Excel.Range intersectRange = CallerTaskPane.IntersectRanges(editDataRange, Target);
      if (intersectRange == null || intersectRange.Count == 0)
        Hide();
      else
        Show();
    }

    protected override void OnMouseDown(MouseEventArgs e)
    {
      base.OnMouseDown(e);
      mouseDownPoint = new Point(e.X, e.Y);
    }

    protected override void OnMouseUp(MouseEventArgs e)
    {
      base.OnMouseUp(e);
      mouseDownPoint = Point.Empty;
    }

    protected override void OnMouseMove(MouseEventArgs e)
    {
      base.OnMouseMove(e);
      if (mouseDownPoint.IsEmpty)
        return;
      Location = new Point(Location.X + (e.X - mouseDownPoint.X), Location.Y + (e.Y - mouseDownPoint.Y));
    }

    private void exitEditModeToolStripMenuItem_Click(object sender, EventArgs e)
    {
      Close();
    }

    private void btnRevert_Click(object sender, EventArgs e)
    {
      DialogResult dr = Utilities.ShowWarningBox(Properties.Resources.RevertDataConfirmation);
      if (dr != DialogResult.Yes)
        return;
      //revertDataChanges(chkRefreshFromDB.Checked);
      revertDataChanges(true);
    }

    private void btnCommit_Click(object sender, EventArgs e)
    {
      pushDataChanges();
    }

    private void chkAutoCommit_CheckedChanged(object sender, EventArgs e)
    {
      btnCommit.Enabled = !chkAutoCommit.Checked;
      btnRevert.Enabled = !chkAutoCommit.Checked;
    }

    private void EditDataDialog_Activated(object sender, EventArgs e)
    {
      Opacity = 1;
    }

    private void EditDataDialog_Deactivate(object sender, EventArgs e)
    {
      Opacity = 0.60;
    }
  }
}
