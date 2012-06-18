using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using MySQL.Utility;
using MySql.Data.MySqlClient;
using Excel = Microsoft.Office.Interop.Excel;

namespace MySQL.ForExcel
{
  public class MySQLDataTable : DataTable
  {
    private bool firstRowIsHeaders;
    private bool addPK;

    public bool AddPK {
      get { return addPK; }
      set
      {
        addPK = value;
        for (int i = 1; i < Columns.Count && value; i++)
          (Columns[i] as MySQLDataColumn).PrimaryKey = false;
      }
    }
    public bool IsFormatted { get; private set; }
    public bool FirstRowIsHeaders
    {
      get { return firstRowIsHeaders; }
      set { firstRowIsHeaders = value; UseFirstRowAsHeaders(value); }
    }
    public bool FirstColumnContainsIntegers
    {
      get
      {
        bool containsIntegers = false;
        int res = 0;
        if (Columns.Count > 1)
          containsIntegers = (Columns[2] as MySQLDataColumn).MySQLDataType.ToLowerInvariant() == "Integer";
        if (!containsIntegers)
        {
          containsIntegers = true;
          for (int rowIdx = 0; rowIdx < Math.Min(Rows.Count, 50); rowIdx++)
          {
            containsIntegers = containsIntegers && Int32.TryParse(Rows[rowIdx][1].ToString(), out res);
          }
        }
        return containsIntegers;
      }
    }
    public int NumberOfPK
    {
      get
      {
        int num = 0;
        foreach (MySQLDataColumn c in Columns)
          if (c.PrimaryKey) num++;
        return AddPK ? num : num - 1;
      }
    }

    private void UseFirstRowAsHeaders(bool useFirstRow)
    {
      DataRow row = Rows[0];
      for (int i = 1; i < Columns.Count; i++)
      {
        MySQLDataColumn col = Columns[i] as MySQLDataColumn;
        col.DisplayName = useFirstRow ? row[i].ToString().Trim().Replace(" ", "_").Replace("(", String.Empty).Replace(")", String.Empty) : col.ColumnName;
      }
      int minus = (useFirstRow ? 1 : 0);
      for (int i = 0; i < Rows.Count; i++)
      {
        Rows[i][0] = i - minus;
      }
    }

    public void SetData(Excel.Range dataRange, bool useFormattedData, bool detectTypes)
    {
      object[,] data;

      // we have to treat a single cell specially.  It doesn't come in as an array but as a single value
      if (dataRange.Count == 1)
      {
        data = new object[2, 2];
        data[1, 1] = useFormattedData ? dataRange.Value : dataRange.Value2;
      }
      else
        data = useFormattedData ? dataRange.Value : dataRange.Value2;

      IsFormatted = useFormattedData;

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

    private void DetectTypes()
    {
      foreach (MySQLDataColumn col in Columns)
        col.DetectType(firstRowIsHeaders);
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
            Rows[rowPos - 1][colPos] = dtValue.ToString(dateFormat);
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
        MySQLDataColumn col = Columns[colPos] as MySQLDataColumn;
        col.FirstRowDataType = headerType;
        col.OtherRowsDataType = previousType;
        col.MySQLDataType = (firstRowIsHeaders ? headerType : previousType);
      }
    }

    private void CreateColumns(int numCols)
    {
      MySQLDataColumn column = null;
      for (int colIdx = 0; colIdx <= numCols; colIdx++)
      {
        string name = "Column" + colIdx;
        column = new MySQLDataColumn();
        column.ColumnName = column.DisplayName = name;
        Columns.Add(column);
      }
      column = (Columns[0] as MySQLDataColumn);
      column.PrimaryKey = true;
      column.AutoPK = true;
      column.ColumnName = column.DisplayName = TableName + "_id";
      column.MySQLDataType = "Integer";
      column.AutoIncrement = true;
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
        if (column.ExcludeColumn)
          continue;
        sql.AppendFormat("{0}{1}", delimiter, column.GetSQL());
        delimiter = ", ";
      }
      foreach (MySQLDataColumn col in Columns)
      {
        if (col.AutoPK || col.PrimaryKey || col.UniqueKey || !col.CreateIndex)
          continue;
        sql.AppendFormat("{0}INDEX {1}_idx ({1})", delimiter, col.DisplayName);
      }
      sql.Append(")");
      return sql.ToString();
    }

    public string GetInsertSQL(int limit)
    {
      int exportColsCount = Columns.Count;
      if (Rows.Count - (firstRowIsHeaders ? 1 : 0) < 1)
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
                                 column.DisplayName);
        separator = ",";
      }
      queryString.Append(") VALUES ");

      foreach (DataRow dr in Rows)
      {
        if (firstRowIsHeaders && rowIdx++ == 0)
          continue;
        if (limit > 0 && rowIdx >= limit)
          break;
        queryString.Append("(");
        separator = String.Empty;
        for (colIdx = 0; colIdx < exportColsCount; colIdx++)
        {
          MySQLDataColumn column = Columns[colIdx] as MySQLDataColumn;
          if (column.ExcludeColumn)
            continue;
          string toLowerDataType = column.MySQLDataType.ToLowerInvariant();
          queryString.AppendFormat("{0}{1}{2}{1}",
                                   separator,
                                   (column.ColumnsRequireQuotes ? "'" : String.Empty),
                                   dr[column.DisplayName].ToString());
          separator = ",";
        }        
        queryString.Append("),");
      }
      if (Rows.Count > 0)
        queryString.Remove(queryString.Length - 1, 1);
      return queryString.ToString();
    }

    public bool InsertDataWithAdapter(MySqlWorkbenchConnection wbConnection, bool firstRowHeader, bool useFormattedData)
    {
      bool success = false;

      DataTable copyOriginal = this.Clone();
      copyOriginal.Merge(this);
      foreach (MySQLDataColumn col in Columns)
      {
        MySQLDataColumn copyCol = copyOriginal.Columns[col.ColumnName] as MySQLDataColumn;
        if (col.ExcludeColumn)
          copyOriginal.Columns.Remove(copyCol);
        else
          copyCol.ColumnName = col.DisplayName;
      }
      if (firstRowHeader)
        copyOriginal.Rows.RemoveAt(0);
      copyOriginal.AcceptChanges();

      string connectionString = Utilities.GetConnectionString(wbConnection);
      // Create & open a SqlConnection, and dispose of it after we are done.
      using (MySqlConnection connection = new MySqlConnection(connectionString))
      {
        MySqlDataAdapter dataAdapter = new MySqlDataAdapter(String.Format("SELECT * FROM {0} LIMIT 0", TableName), connection);
        DataTable exportingDataTable = new DataTable();
        dataAdapter.Fill(exportingDataTable);
        foreach (DataRow row in copyOriginal.Rows)
        {
          exportingDataTable.LoadDataRow(row.ItemArray, LoadOption.OverwriteChanges);
        }
        MySqlCommandBuilder commBuilder = new MySqlCommandBuilder(dataAdapter);
        dataAdapter.InsertCommand = commBuilder.GetInsertCommand();

        int updatedCount = 0;
        try
        {
          updatedCount = dataAdapter.Update(exportingDataTable);
          success = updatedCount > 0;
        }
        catch (Exception ex)
        {
          System.Diagnostics.Debug.WriteLine(ex.Message);
        }
      }

      return success;
    }
  }

  public class MySQLDataColumn : DataColumn
  {
    private bool uniqueKey;

    public bool AutoPK { get; set; }
    public bool CreateIndex { get; set; }
    public bool UniqueKey 
    {
      get { return uniqueKey; }
      set { uniqueKey = value; if (uniqueKey) CreateIndex = true; }
    }

    public bool PrimaryKey { get; set; }
    public bool AllowNull { get; set; }
    public bool ExcludeColumn { get; set; }
    public string MySQLDataType { get; set; }
    public string DisplayName { get; set; }
    //public string SavedName { get; set; }
    public string FirstRowDataType { get; set; }
    public string OtherRowsDataType { get; set; }

    #region Properties

    public bool IsDecimal
    {
      get
      {
        string toLowerDataType = MySQLDataType.ToLowerInvariant();
        return toLowerDataType == "real" || toLowerDataType == "double" || toLowerDataType == "float" || toLowerDataType == "decimal" || toLowerDataType == "numeric";
      }
    }

    public bool IsNumeric
    {
      get
      {
        string toLowerDataType = MySQLDataType.ToLowerInvariant();
        return IsDecimal || toLowerDataType.Contains("int");
      }
    }

    public bool IsChar
    {
      get
      {
        string toLowerDataType = MySQLDataType.ToLowerInvariant();
        return toLowerDataType.Contains("char");
      }
    }

    public bool IsCharOrText
    {
      get
      {
        string toLowerDataType = MySQLDataType.ToLowerInvariant();
        return toLowerDataType.Contains("char") || toLowerDataType.Contains("text");
      }
    }

    public bool IsBinary
    {
      get
      {
        string toLowerDataType = MySQLDataType.ToLowerInvariant();
        return toLowerDataType.Contains("binary");
      }
    }

    public bool IsDate
    {
      get
      {
        string toLowerDataType = MySQLDataType.ToLowerInvariant();
        return toLowerDataType.Contains("date") || toLowerDataType == "timestamp";
      }
    }

    public bool ColumnsRequireQuotes
    {
      get { return IsCharOrText || IsDate; }
    }

    #endregion Properties

    public void DetectType(bool firstRowIsHeaders)
    {
      object valueFromArray = null;
      string proposedType = String.Empty;
      string previousType = String.Empty;
      string headerType = String.Empty;
      bool typesConsistent = true;
      bool valueOverflow = false;
      string dateFormat = "yyyy-MM-dd HH:mm:ss";
      int rowPos = 0;

      foreach (DataRow dr in Table.Rows)
      {
        valueFromArray = dr[Ordinal];
        if (valueFromArray == null)
          continue;
        proposedType = Utilities.GetMySQLExportDataType(valueFromArray, out valueOverflow);
        if (proposedType.StartsWith("Date") && valueFromArray is DateTime)
        {
          DateTime dtValue = (DateTime)valueFromArray;
          dr[Ordinal] = dtValue.ToString(dateFormat);
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
      FirstRowDataType = headerType;
      OtherRowsDataType = previousType;
      MySQLDataType = (firstRowIsHeaders ? headerType : previousType);
      rowPos++;
    }

    public string GetSQL()
    {
      if (String.IsNullOrEmpty(DisplayName))
        return null;

      StringBuilder colDefinition = new StringBuilder(DisplayName);
      colDefinition.AppendFormat(" {0}", MySQLDataType);
      if (AutoPK)
        colDefinition.Append(" primary key");
      else if (UniqueKey)
        colDefinition.Append(" unique key");
      if (AllowNull)
        colDefinition.Append(" null");
      if (AutoIncrement)
        colDefinition.Append(" auto_increment");

      return colDefinition.ToString();
    }

  }
}
