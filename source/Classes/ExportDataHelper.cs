using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using Excel = Microsoft.Office.Interop.Excel;
using System.Globalization;
using MySQL.Utility;
using MySql.Data.MySqlClient;

namespace MySQL.ForExcel
{
  class ExportDataHelper
  {
    private MySqlWorkbenchConnection wbConnection;

    public DataTable FormattedExcelData { get; private set; }
    public DataTable UnformattedExcelData { get; private set; }
    public MySQLTable ExportTable { get; private set; }
    public List<ColumnGuessData> HeaderRowColumnsGuessData { get; private set; }
    public List<ColumnGuessData> DataRowsColumnsGuessData { get; private set; }

    public ExportDataHelper(MySqlWorkbenchConnection wbConnection, Excel.Range exportDataRange, string toTableName)
    {
      this.wbConnection = wbConnection;
      if (String.IsNullOrEmpty(toTableName))
        createMySQLTable();
      else
        getMySQLTableSchemaInfo(toTableName);
      fillDataTablesFromRange(exportDataRange);
    }

    private void createMySQLTable()
    {
      ExportTable = new MySQLTable(wbConnection, null, null);
      ExportTable.Engine = "InnoDB";

      int tableCount = 1;
      string tableName = String.Empty;
      bool tableExists = true;
      while (tableExists)
      {
        tableName = String.Format("Table{0}", tableCount++);
        tableExists = Utilities.TableExistsInSchema(wbConnection, wbConnection.Schema, tableName);
      }

      ExportTable.Name = tableName;
      ExportTable.CharacterSet = "latin1";
      ExportTable.Collation = "latin1_swedish_ci";
    }

    private void getMySQLTableSchemaInfo(string toTableName)
    {
      DataTable tablesData = Utilities.GetSchemaCollection(wbConnection, "Tables", null, wbConnection.Schema, toTableName);
      if (tablesData.Rows.Count == 0)
      {
        System.Diagnostics.Debug.WriteLine(String.Format("Schema info for table {0} not found.", toTableName));
        return;
      }
      DataTable columnsData = Utilities.GetSchemaCollection(wbConnection, "Columns", null, wbConnection.Schema, toTableName);
      ExportTable = new MySQLTable(wbConnection, tablesData.Rows[0], columnsData);
    }

    private void fillDataTablesFromRange(Excel.Range selectedRange)
    {
      FormattedExcelData = new DataTable();
      UnformattedExcelData = new DataTable();

      object[,] formattedArrayFromRange = selectedRange.Value as object[,];
      object[,] unformattedArrayFromRange = selectedRange.Value2 as object[,];
      object valueFromArray = null;
      DataRow formattedRow;
      DataRow unformattedRow;
      Excel.Range colRange;
      string colNameFromRange = String.Empty;

      int rowsCount = formattedArrayFromRange.GetUpperBound(0);
      int colsCount = formattedArrayFromRange.GetUpperBound(1);

      for (int colPos = 1; colPos <= colsCount; colPos++)
      {
        if (ExportTable.IsNew)
        {
          FormattedExcelData.Columns.Add();
          UnformattedExcelData.Columns.Add();
          ExportTable.Columns.Add(new MySQLColumn(null, ExportTable));
          ExportTable.Columns[colPos - 1].MappedDataColName = FormattedExcelData.Columns[colPos - 1].ColumnName;
        }
        else
        {
          colRange = selectedRange.Columns[colPos, Type.Missing] as Excel.Range;
          colNameFromRange = colRange.Address.Substring(1, colRange.Address.IndexOf("$", 1) - 1);
          FormattedExcelData.Columns.Add(colNameFromRange);
          UnformattedExcelData.Columns.Add(colNameFromRange);
        }
      }

      for (int rowPos = 1; rowPos <= rowsCount; rowPos++)
      {
        bool wholeRowNull = true;
        formattedRow = FormattedExcelData.NewRow();
        unformattedRow = UnformattedExcelData.NewRow();

        for (int colPos = 1; colPos <= colsCount; colPos++)
        {
          valueFromArray = formattedArrayFromRange[rowPos, colPos];
          wholeRowNull = wholeRowNull && valueFromArray == null;
          formattedRow[colPos - 1] = (valueFromArray != null ? valueFromArray.ToString() : String.Empty);
          valueFromArray = unformattedArrayFromRange[rowPos, colPos];
          unformattedRow[colPos - 1] = (valueFromArray != null ? valueFromArray.ToString() : String.Empty);
        }

        if (!wholeRowNull)
        {
          FormattedExcelData.Rows.Add(formattedRow);
          UnformattedExcelData.Rows.Add(unformattedRow);
        }
      }

      guessDataTypesFromData(formattedArrayFromRange);
    }

    private void guessDataTypesFromData(object[,] formattedArrayFromRange)
    {
      int rowsCount = formattedArrayFromRange.GetUpperBound(0);
      int colsCount = formattedArrayFromRange.GetUpperBound(1);
      HeaderRowColumnsGuessData = new List<ColumnGuessData>(colsCount);
      DataRowsColumnsGuessData = new List<ColumnGuessData>(colsCount);

      object valueFromArray = null;
      string strValue = String.Empty;
      string proposedType = String.Empty;
      string previousType = String.Empty;
      string headerType = String.Empty;
      bool typesConsistent = true;
      int maxStrValue = 0;
      string nameFromHeader;
      string nameGeneric;
      CultureInfo cultureForDates = new CultureInfo("en-US");
      string dateFormat = "yyyy-MM-dd HH:mm:ss";

      for (int colPos = 1; colPos <= colsCount; colPos++)
      {
        HeaderRowColumnsGuessData.Add(new ColumnGuessData());
        DataRowsColumnsGuessData.Add(new ColumnGuessData());

        for (int rowPos = 1; rowPos <= rowsCount; rowPos++)
        {
          valueFromArray = formattedArrayFromRange[rowPos, colPos];
          if (valueFromArray == null)
            continue;
          strValue = valueFromArray.ToString();
          proposedType = Utilities.GetMySQLDataType(valueFromArray);
          if (proposedType == "datetime" && valueFromArray is DateTime)
          {
            DateTime dtValue = (DateTime)valueFromArray;
            FormattedExcelData.Rows[rowPos - 1][colPos - 1] = dtValue.ToString(dateFormat);
          }
          maxStrValue = Math.Max(strValue.Length, maxStrValue);
          if (rowPos == 1)
            headerType = proposedType;
          else
          {
            typesConsistent = typesConsistent && (rowPos > 2 ? previousType == proposedType : true);
            previousType = proposedType;
          }
        }

        nameFromHeader = (formattedArrayFromRange[1, colPos] != null ? formattedArrayFromRange[1, colPos].ToString().Replace(" ", "_").Replace("(", String.Empty).Replace(")", String.Empty) : String.Empty);
        nameGeneric = String.Format("Column{0}", colPos);
        if (nameFromHeader.Length == 0)
          nameFromHeader = nameGeneric;
        int charLen = (maxStrValue + (10 - maxStrValue % 10));
        headerType = (headerType.Length == 0 ? previousType : (headerType == "varchar" ? (charLen > 65535 ? "text" : "varchar") : headerType));
        previousType = (previousType.Length == 0 ? "varchar" : (previousType == "varchar" ? (charLen > 65535 ? "text" : "varchar") : previousType));
        HeaderRowColumnsGuessData[colPos - 1].ColumnName = nameFromHeader;
        HeaderRowColumnsGuessData[colPos - 1].MySQLType = headerType;
        HeaderRowColumnsGuessData[colPos - 1].StrLen = charLen;
        DataRowsColumnsGuessData[colPos - 1].ColumnName = nameGeneric;
        DataRowsColumnsGuessData[colPos - 1].MySQLType = previousType;
        DataRowsColumnsGuessData[colPos - 1].StrLen = charLen;
      }
    }

    public bool CreateTableInDB()
    {
      if (!ExportTable.IsNew)
      {
        System.Diagnostics.Debug.WriteLine(Properties.Resources.TableNotNewInCreate);
        return false;
      }

      bool success = false;
      string connectionString = Utilities.GetConnectionString(wbConnection);
      string queryString = ExportTable.GetSQL();

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

    public bool InsertData(bool firstRowHeader, bool useFormattedData)
    {
      bool success = false;

      DataTable insertingData = (useFormattedData ? FormattedExcelData : UnformattedExcelData);
      if (insertingData.Rows.Count - (firstRowHeader ? 1 : 0) < 1)
        return true;

      string connectionString = Utilities.GetConnectionString(wbConnection);
      StringBuilder queryString = new StringBuilder();
      int rowIdx = 0;
      int colIdx = 0;
      int exportColsCount = ExportTable.Columns.Count;
      List<bool> columnsRequireQuotes = new List<bool>();
      List<string> mappedColumnNames = new List<string>(ExportTable.Columns.Count);
      
      string separator = String.Empty;

      queryString.AppendFormat("USE {0}; INSERT INTO", wbConnection.Schema);
      queryString.AppendFormat(" {0} (", ExportTable.Name);

      for (colIdx = 0; colIdx < exportColsCount; colIdx++)
      {
        if (String.IsNullOrEmpty(ExportTable.Columns[colIdx].MappedDataColName))
          continue;
        MySQLColumn column = ExportTable.Columns[colIdx];
        queryString.AppendFormat("{0}{1}",
                                 separator,
                                 column.ColumnName);
        separator = ",";
        columnsRequireQuotes.Add(column.ColumnsRequireQuotes);
        mappedColumnNames.Add(ExportTable.Columns[colIdx].MappedDataColName);
      }
      queryString.Append(") VALUES ");

      foreach (DataRow dr in insertingData.Rows)
      {
        if (firstRowHeader && rowIdx++ == 0)
          continue;
        queryString.Append("(");
        separator = String.Empty;
        for (colIdx = 0; colIdx < mappedColumnNames.Count; colIdx++)
        {
          queryString.AppendFormat("{0}{1}{2}{1}",
                                   separator,
                                   (columnsRequireQuotes[colIdx] ? "'" : String.Empty),
                                   dr[mappedColumnNames[colIdx]].ToString());
          separator = ",";
        }
        queryString.Append("),");
      }
      if (insertingData.Rows.Count > 0)
        queryString.Remove(queryString.Length - 1, 1);
      queryString.Append(";");

      try
      {
        using (MySqlConnection conn = new MySqlConnection(connectionString))
        {
          conn.Open();

          MySqlCommand cmd = new MySqlCommand(queryString.ToString(), conn);
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

  }
}
