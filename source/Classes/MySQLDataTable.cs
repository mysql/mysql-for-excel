using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using MySQL.Utility;
using MySql.Data.MySqlClient;

namespace MySQL.ForExcel
{
  public class MySQLDataTable : DataTable
  {
    private bool useFirstRowAsHeaders = false;
    public bool UseFirstRowAsHeaders
    {
      set
      {
        useFirstRowAsHeaders = value;
        DataRow row = Rows[0];
        for (int i = 1; i < Columns.Count; i++)
        {
          MySQLDataColumn col = Columns[i] as MySQLDataColumn;
          string name = useFirstRowAsHeaders ? DataToColName(row[i].ToString()) : col.SavedName;
          col.SavedName = useFirstRowAsHeaders ? col.ColumnName : null;
          col.ColumnName = name;
          col.DataType = (useFirstRowAsHeaders ? col.FirstRowDataType : col.OtherRowsDataType);
        }
      }
      get
      {
        return useFirstRowAsHeaders;
      }
    }

    public void SetData(object[,] data, bool detectTypes)
    {
      int numRows = data.GetUpperBound(0);
      int numCols = data.GetUpperBound(1);

      if (Columns.Count == 0)
        CreateColumns(numCols);

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
      int rowsCount = data.GetUpperBound(0);
      int colsCount = data.GetUpperBound(1);

      object valueFromArray = null;
      string proposedType = String.Empty;
      string previousType = String.Empty;
      string headerType = String.Empty;
      bool typesConsistent = true;
      bool valueOverflow = false;
      string dateFormat = "yyyy-MM-dd HH:mm:ss";

      for (int colPos = 1; colPos <= colsCount; colPos++)
      {
        for (int rowPos = 1; rowPos <= rowsCount; rowPos++)
        {
          valueFromArray = data[rowPos, colPos];
          if (valueFromArray == null)
            continue;
          proposedType = Utilities.GetMySQLExportDataType(valueFromArray, out valueOverflow);
          if (proposedType.StartsWith("Date") && valueFromArray is DateTime)
          {
            DateTime dtValue = (DateTime)valueFromArray;
            Rows[rowPos - 1][colPos - 1] = dtValue.ToString(dateFormat);
          }
          if (rowPos == 1)
            headerType = proposedType;
          else
          {
            typesConsistent = typesConsistent && (rowPos > 2 ? previousType == proposedType : true);
            previousType = proposedType;
          }
        }
        if (previousType.Length == 0)
          previousType = "Varchar(255)";
        if (headerType.Length == 0)
          headerType = previousType;
        MySQLDataColumn col = Columns[colPos - 1] as MySQLDataColumn;
        col.FirstRowDataType = headerType;
        col.OtherRowsDataType = previousType;
        col.DataType = (useFirstRowAsHeaders ? headerType : previousType);
      }
    }

    private void CreateColumns(int numCols)
    {
      for (int col = 0; col <= numCols; col++)
      {
        string name = "Column" + col;
        MySQLDataColumn column = new MySQLDataColumn();
        column.ColumnName = name;
        Columns.Add(column);
      }
    }

    private string DataToColName(string dataValue)
    {
      return (dataValue != null ? dataValue.Replace(" ", "_").Replace("(", String.Empty).Replace(")", String.Empty) : String.Empty);
    }

    public bool CreateTable(MySqlWorkbenchConnection wbConnection)
    {
      bool success = false;
      string connectionString = Utilities.GetConnectionString(wbConnection);
      string queryString = GetCreateSQL();

      try
      {
        using (MySqlConnection conn = new MySqlConnection(connectionString))
        {
          conn.Open();

          MySqlCommand cmd = new MySqlCommand(queryString, conn);
          cmd.ExecuteNonQuery();
          success = true;
        }
      }
      catch (Exception ex)
      {
        System.Diagnostics.Debug.WriteLine(ex.Message);
      }

      return success;
    }

    public string GetCreateSQL()
    {
      StringBuilder sql = new StringBuilder();
      sql.AppendFormat("CREATE TABLE `{0}` (", TableName);

      string delimiter = "";
      foreach (MySQLDataColumn column in Columns)
      {
        sql.AppendFormat("{0}{1}", delimiter, column.GetSQL());
        delimiter = ", ";
      }
      sql.Append(")");
      return sql.ToString();
    }

    public string GetInsertSQL()
    {
      int exportColsCount = Columns.Count;
      if (Rows.Count - (useFirstRowAsHeaders ? 1 : 0) < 1)
        return null;

      StringBuilder queryString = new StringBuilder("INSERT INTO");
      int rowIdx = 0;
      int colIdx = 0;

      string separator = String.Empty;
      queryString.AppendFormat(" {0} (", TableName);

      for (colIdx = 0; colIdx < exportColsCount; colIdx++)
      {
        MySQLDataColumn column = Columns[colIdx] as MySQLDataColumn;
        if (column.ExcludeColumn)
          continue;
        queryString.AppendFormat("{0}{1}",
                                 separator,
                                 column.ColumnName);
        separator = ",";
      }
      queryString.Append(") VALUES ");

      foreach (DataRow dr in Rows)
      {
        if (useFirstRowAsHeaders && rowIdx++ == 0)
          continue;
        queryString.Append("(");
        separator = String.Empty;
        for (colIdx = 0; colIdx < exportColsCount; colIdx++)
        {
          MySQLDataColumn column = Columns[colIdx] as MySQLDataColumn;
          if (column.ExcludeColumn)
            continue;
          string toLowerDataType = column.DataType.ToLowerInvariant();
          queryString.AppendFormat("{0}{1}{2}{1}",
                                   separator,
                                   (column.ColumnsRequireQuotes ? "'" : String.Empty),
                                   dr[column.ColumnName].ToString());
          separator = ",";
        }        
        queryString.Append("),");
      }
      if (Rows.Count > 0)
        queryString.Remove(queryString.Length - 1, 1);
      return queryString.ToString();
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
    public string FirstRowDataType { get; set; }
    public string OtherRowsDataType { get; set; }

    #region Properties

    public bool IsDecimal
    {
      get
      {
        string toLowerDataType = DataType.ToLowerInvariant();
        return toLowerDataType == "real" || toLowerDataType == "double" || toLowerDataType == "float" || toLowerDataType == "decimal" || toLowerDataType == "numeric";
      }
    }

    public bool IsNumeric
    {
      get
      {
        string toLowerDataType = DataType.ToLowerInvariant();
        return IsDecimal || toLowerDataType.Contains("int");
      }
    }

    public bool IsChar
    {
      get
      {
        string toLowerDataType = DataType.ToLowerInvariant();
        return toLowerDataType.Contains("char");
      }
    }

    public bool IsCharOrText
    {
      get
      {
        string toLowerDataType = DataType.ToLowerInvariant();
        return toLowerDataType.Contains("char") || toLowerDataType.Contains("text");
      }
    }

    public bool IsBinary
    {
      get
      {
        string toLowerDataType = DataType.ToLowerInvariant();
        return toLowerDataType.Contains("binary");
      }
    }

    public bool IsDate
    {
      get
      {
        string toLowerDataType = DataType.ToLowerInvariant();
        return toLowerDataType.Contains("date") || toLowerDataType == "timestamp";
      }
    }

    public bool ColumnsRequireQuotes
    {
      get { return IsCharOrText || IsDate; }
    }

    #endregion Properties

    //public void DetectType()
    //{
    //  foreach (DataRow row in Table.Rows)
    //  {
    //    // look at the data and try to determine what our type is
    //    // copy detection code from ExportDataHelper
    //  }
    //}

    public string GetSQL()
    {
      if (String.IsNullOrEmpty(ColumnName))
        return null;

      StringBuilder colDefinition = new StringBuilder(ColumnName);
      colDefinition.AppendFormat(" {0}", DataType);
      if (AllowNull)
        colDefinition.Append(" null");
      if (AutoIncrement)
        colDefinition.Append(" auto_increment");
      if (UniqueKey)
        colDefinition.Append(" unique key");

      return colDefinition.ToString();
    }

  }
}
