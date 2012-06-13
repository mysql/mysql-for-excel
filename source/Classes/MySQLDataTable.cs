using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace MySQL.ForExcel
{
  public class MySQLDataTable : DataTable
  {
    public void SetData(object[,] data, bool detectTypes)
    {
      if (Columns.Count == 0)
        CreateColumns(data);

      int numRows = data.GetUpperBound(0);
      int numCols = data.GetUpperBound(1);

      for (int row = 1; row <= numRows; row++)
      {
        DataRow dataRow = NewRow();
        dataRow[0] = row;
        for (int col = 1; col <= numCols; col++)
          dataRow[col] = data[row, col];
        Rows.Add(dataRow);
      }
      if (detectTypes)
        DetectTypes(data);
    }

    private void DetectTypes(object[,] data)
    {
      foreach (MySQLDataColumn col in Columns)
        col.DetectType();
    }

    private void CreateColumns(object[,] data)
    {
      int numRows = data.GetUpperBound(0);
      int numCols = data.GetUpperBound(1);

      for (int col = 0; col <= numCols; col++)
      {
        string name = "Column" + col;
        MySQLDataColumn column = new MySQLDataColumn();
        column.ColumnName = name;
        Columns.Add(column);
      }
    }

    public void UseFirstRowAsHeaders(bool useFirstRow)
    {
      DataRow row = Rows[0];
      for (int i = 1; i < Columns.Count; i++)
      {
        MySQLDataColumn col = Columns[i] as MySQLDataColumn;
        string name = useFirstRow ? row[i].ToString() : col.SavedName;
        col.SavedName = useFirstRow ? col.ColumnName : null;
        col.ColumnName = name;
      }
    }

    public string GenerateCreateTableSQL()
    {
      //Move code from ExportDataHelper
      return null;
    }

    public string GenerateSQL()
    {
      // Move code from ExportDataHelper
      return null;
    }

    public void ApplyChanges()
    {
    }
  }

  public class MySQLDataColumn : DataColumn
  {
    private bool uniqueKey;

    public bool CreateIndex { get; set; }
    public bool UniqueKey 
    {
      get { return uniqueKey; }
      set { uniqueKey = value; if (uniqueKey) CreateIndex = true; }
    }

    public bool PrimaryKey { get; set; }
    public bool AllowNull { get; set; }
    public bool ExcludeColumn { get; set; }
    public string DataType { get; set; }
    public string SavedName { get; set; }

    public void DetectType()
    {
      foreach (DataRow row in Table.Rows)
      {
        // look at the data and try to determine what our type is
        // copy detection code from ExportDataHelper
      }
    }
  }
}
