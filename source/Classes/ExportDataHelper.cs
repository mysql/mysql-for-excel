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
    public Excel.Range ExportingRange { get; private set; }

    public ExportDataHelper(MySqlWorkbenchConnection wbConnection, Excel.Range exportDataRange, string toTableName, bool guessDataTypes, bool addPKColumn)
    {
      this.wbConnection = wbConnection;
      ExportingRange = exportDataRange;

      if (String.IsNullOrEmpty(toTableName))
        createMySQLTable(false);
      else
        getMySQLTableSchemaInfo(toTableName);
      fillDataTablesFromRange(addPKColumn);
      GuessDataTypesFromData(guessDataTypes, addPKColumn);
    }

    public ExportDataHelper(MySqlWorkbenchConnection wbConnection, Excel.Range exportDataRange, string toTableName) : this(wbConnection, exportDataRange, toTableName, true, false)
    {
    }

    private void createMySQLTable(bool autoAssignName)
    {
      ExportTable = new MySQLTable(wbConnection, null, null);
      ExportTable.Engine = "InnoDB";

      int tableCount = 1;
      string tableName = String.Empty;

      if (autoAssignName)
      {
        bool tableExists = true;
        while (tableExists)
        {
          tableName = String.Format("Table{0}", tableCount++);
          tableExists = Utilities.TableExistsInSchema(wbConnection, wbConnection.Schema, tableName);
        }
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

    private object[,] getArrayFromRange(Excel.Range dataRange, bool formatted)
    {
      object[,] arrayFromRange;

      // we have to treat a single cell specially.  It doesn't come in as an array but as a single value
      if (dataRange.Count == 1)
      {
        arrayFromRange = new object[2, 2];
        arrayFromRange[1, 1] = (formatted ? dataRange.Value : dataRange.Value2);
      }
      else
        arrayFromRange = (formatted ? dataRange.Value : dataRange.Value2);

      return arrayFromRange;
    }

    private void fillDataTablesFromRange(bool addPKColumn)
    {
      FormattedExcelData = new DataTable();
      UnformattedExcelData = new DataTable();
      object[,] formattedArrayFromRange = getArrayFromRange(ExportingRange, true);
      object[,] unformattedArrayFromRange = getArrayFromRange(ExportingRange, false);

      object valueFromArray = null;
      DataRow formattedRow;
      DataRow unformattedRow;
      Excel.Range colRange;
      string colNameFromRange = String.Empty;

      int rowsCount = formattedArrayFromRange.GetUpperBound(0);
      int colsCount = formattedArrayFromRange.GetUpperBound(1);

      if (addPKColumn)
      {
        FormattedExcelData.Columns.Add();
        UnformattedExcelData.Columns.Add();
        if (ExportTable.IsNew)
        {
          MySQLColumn pkCol = new MySQLColumn(null, ExportTable);
          pkCol.ColumnName = "Column1";
          pkCol.DataType = "Integer";
          pkCol.AutoIncrement = true;
          pkCol.PrimaryKey = true;
          pkCol.MappedDataColName = "Column1";
          ExportTable.Columns.Add(pkCol);
        }
      }
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
          colRange = ExportingRange.Columns[colPos, Type.Missing] as Excel.Range;
          colNameFromRange = colRange.Address.Substring(1, colRange.Address.IndexOf("$", 1) - 1);
          FormattedExcelData.Columns.Add(colNameFromRange);
          UnformattedExcelData.Columns.Add(colNameFromRange);
        }
      }

      int addedPKColIdx = (addPKColumn ? 1 : 0);
      for (int rowPos = 1; rowPos <= rowsCount; rowPos++)
      {
        bool wholeRowNull = true;
        formattedRow = FormattedExcelData.NewRow();
        unformattedRow = UnformattedExcelData.NewRow();

        if (addPKColumn)
        {
          formattedRow[0] = rowPos;
          unformattedRow[0] = rowPos;
        }

        for (int colPos = 1; colPos <= colsCount; colPos++)
        {
          int arrayColPos = colPos;
          int tablesColPos = colPos + addedPKColIdx - 1;
          valueFromArray = formattedArrayFromRange[rowPos, arrayColPos];
          wholeRowNull = wholeRowNull && valueFromArray == null;
          formattedRow[tablesColPos] = (valueFromArray != null ? valueFromArray.ToString() : String.Empty);
          valueFromArray = unformattedArrayFromRange[rowPos, arrayColPos];
          unformattedRow[tablesColPos] = (valueFromArray != null ? valueFromArray.ToString() : String.Empty);
        }

        if (!wholeRowNull)
        {
          FormattedExcelData.Rows.Add(formattedRow);
          UnformattedExcelData.Rows.Add(unformattedRow);
        }
      }
    }

    public void GuessDataTypesFromData(bool guessTypes, bool addPKColumn)
    {
      object[,] formattedArrayFromRange = getArrayFromRange(ExportingRange, true);

      int rowsCount = formattedArrayFromRange.GetUpperBound(0);
      int colsCount = formattedArrayFromRange.GetUpperBound(1);
      HeaderRowColumnsGuessData = new List<ColumnGuessData>(colsCount);
      DataRowsColumnsGuessData = new List<ColumnGuessData>(colsCount);

      if (!guessTypes)
        return;

      object valueFromArray = null;
      string strValue = String.Empty;
      string proposedType = String.Empty;
      string previousType = String.Empty;
      string headerType = String.Empty;
      bool typesConsistent = true;
      int maxStrValue = 0;
      int addedPKColIdx = (addPKColumn ? 1 : 0);
      string nameFromHeader;
      string nameGeneric;
      CultureInfo cultureForDates = new CultureInfo("en-US");
      string dateFormat = "yyyy-MM-dd HH:mm:ss";

      if (addPKColumn)
      {
        maxStrValue = rowsCount.ToString().Length;
        maxStrValue = (maxStrValue + (10 - maxStrValue % 10));
        HeaderRowColumnsGuessData.Add(new ColumnGuessData());
        DataRowsColumnsGuessData.Add(new ColumnGuessData());
        HeaderRowColumnsGuessData[0].ColumnName = String.Format("{0}_id", ExportTable.Name);
        HeaderRowColumnsGuessData[0].MySQLType = "integer";
        HeaderRowColumnsGuessData[0].StrLen = maxStrValue;
        DataRowsColumnsGuessData[0].ColumnName = "Column1";
        DataRowsColumnsGuessData[0].MySQLType = "integer";
        DataRowsColumnsGuessData[0].StrLen = maxStrValue;
      }
      for (int colPos = 1; colPos <= colsCount; colPos++)
      {
        int arrayColPos = colPos;
        int tablesColPos = colPos + addedPKColIdx;

        ColumnGuessData hColGuessData = new ColumnGuessData();
        ColumnGuessData dColGuessData = new ColumnGuessData();

        for (int rowPos = 1; rowPos <= rowsCount; rowPos++)
        {
          valueFromArray = formattedArrayFromRange[rowPos, arrayColPos];
          if (valueFromArray == null)
            continue;
          strValue = valueFromArray.ToString();
          proposedType = Utilities.GetMySQLDataType(valueFromArray);
          if (proposedType == "datetime" && valueFromArray is DateTime)
          {
            DateTime dtValue = (DateTime)valueFromArray;
            FormattedExcelData.Rows[rowPos - 1][tablesColPos - 1] = dtValue.ToString(dateFormat);
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

        nameFromHeader = (formattedArrayFromRange[1, arrayColPos] != null ? formattedArrayFromRange[1, arrayColPos].ToString().Replace(" ", "_").Replace("(", String.Empty).Replace(")", String.Empty) : String.Empty);
        nameGeneric = String.Format("Column{0}", tablesColPos);
        if (nameFromHeader.Length == 0)
          nameFromHeader = nameGeneric;
        int charLen = (maxStrValue + (10 - maxStrValue % 10));
        headerType = (headerType.Length == 0 ? previousType : (headerType == "varchar" ? (charLen > 65535 ? "text" : "varchar") : headerType));
        previousType = (previousType.Length == 0 ? "varchar" : (previousType == "varchar" ? (charLen > 65535 ? "text" : "varchar") : previousType));

        hColGuessData.ColumnName = nameFromHeader;
        hColGuessData.MySQLType = headerType;
        hColGuessData.StrLen = charLen;
        dColGuessData.ColumnName = nameGeneric;
        dColGuessData.MySQLType = previousType;
        dColGuessData.StrLen = charLen;

        HeaderRowColumnsGuessData.Add(hColGuessData);
        DataRowsColumnsGuessData.Add(dColGuessData);
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

    public bool InsertDataWithAdapter(bool firstRowHeader, bool useFormattedData)
    {
      bool success = false;

      string connectionString = Utilities.GetConnectionString(wbConnection);
      // Create & open a SqlConnection, and dispose of it after we are done.
      using (MySqlConnection connection = new MySqlConnection(connectionString))
      {
        MySqlDataAdapter dataAdapter = new MySqlDataAdapter(String.Format("SELECT * FROM {0} LIMIT 0", ExportTable.Name), connection);
        DataTable exportingDataTable = new DataTable();
        dataAdapter.Fill(exportingDataTable);
        exportingDataTable.Merge((useFormattedData ? FormattedExcelData : UnformattedExcelData));
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

    public bool InsertData(bool firstRowHeader, bool useFormattedData, out string insertQuery, out MySqlException exception)
    {
      bool success = false;
      insertQuery = String.Empty;
      exception = null;

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
      
      string colsSeparator = String.Empty;
      string rowsSeparator = String.Empty;

      queryString.AppendFormat("USE {0};{1}INSERT INTO", wbConnection.Schema, Environment.NewLine);
      queryString.AppendFormat(" {0}{1}(", ExportTable.Name, Environment.NewLine);

      for (colIdx = 0; colIdx < exportColsCount; colIdx++)
      {
        if (String.IsNullOrEmpty(ExportTable.Columns[colIdx].MappedDataColName))
          continue;
        MySQLColumn column = ExportTable.Columns[colIdx];
        queryString.AppendFormat("{0}{1}",
                                 colsSeparator,
                                 column.ColumnName);
        colsSeparator = ",";
        columnsRequireQuotes.Add(column.ColumnsRequireQuotes);
        mappedColumnNames.Add(ExportTable.Columns[colIdx].MappedDataColName);
      }
      queryString.AppendFormat("){0}VALUES{0}", Environment.NewLine);

      foreach (DataRow dr in insertingData.Rows)
      {
        if (firstRowHeader && rowIdx++ == 0)
          continue;
        queryString.AppendFormat("{0}(", rowsSeparator);
        colsSeparator = String.Empty;
        for (colIdx = 0; colIdx < mappedColumnNames.Count; colIdx++)
        {
          queryString.AppendFormat("{0}{1}{2}{1}",
                                   colsSeparator,
                                   (columnsRequireQuotes[colIdx] ? "'" : String.Empty),
                                   dr[mappedColumnNames[colIdx]].ToString());
          colsSeparator = ",";
        }
        queryString.Append(")");
        if (rowsSeparator.Length == 0)
          rowsSeparator = "," + Environment.NewLine;
      }
      queryString.Append(";");

      insertQuery = queryString.ToString();

      try
      {
        using (MySqlConnection conn = new MySqlConnection(connectionString))
        {
          conn.Open();

          MySqlCommand cmd = new MySqlCommand(insertQuery, conn);
          cmd.ExecuteNonQuery();
          success = true;
        }
      }
      catch (MySqlException ex)
      {
        exception = ex;
      }

      return success;
    }

    public bool InsertData(bool firstRowHeader, bool useFormattedData)
    {
      string insertQuery;
      MySqlException exception;
      return InsertData(firstRowHeader, useFormattedData, out insertQuery, out exception);
    }

  }

  public class ColumnGuessData
  {
    public string ColumnName = String.Empty;
    public string MySQLType = "varchar";
    public int StrLen = 10;
    public bool MySQLTypeConsistentInAllRows = true;
  };
}
