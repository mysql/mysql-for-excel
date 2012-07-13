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
    private bool useFirstColumnAsPK;

    public bool AddPrimaryKeyColumn { get; set; }
    public bool UseFirstColumnAsPK {
      get { return useFirstColumnAsPK; }
      set
      {
        useFirstColumnAsPK = value;
        if (AddPrimaryKeyColumn)
          for (int i = 1; i < Columns.Count && value; i++)
            (Columns[i] as MySQLDataColumn).PrimaryKey = false;
      }
    }
    public bool RemoveEmptyColumns { get; set; }
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
          containsIntegers = (Columns[1] as MySQLDataColumn).MySQLDataType.ToLowerInvariant() == "integer";
        if (!containsIntegers)
        {
          int rowsToAnalyzeCount = Math.Min(Rows.Count, 50);
          int startingRow = (firstRowIsHeaders ? 1 : 0);
          containsIntegers = (startingRow < rowsToAnalyzeCount);
          for (int rowIdx = startingRow; rowIdx < rowsToAnalyzeCount; rowIdx++)
          {
            containsIntegers = containsIntegers && Int32.TryParse(Rows[rowIdx][1].ToString(), out res);
          }
        }
        return containsIntegers;
      }
    }
    public int NumberOfPK
    {
      get { return Columns.OfType<MySQLDataColumn>().Skip(1).Count(col => col.PrimaryKey && !col.ExcludeColumn); }
    }
    public int MappedColumnsQuantity
    {
      get
      {
        int mappedQty = 0;
        for (int colIdx = 0; colIdx < Columns.Count; colIdx++)
        {
          if (!String.IsNullOrEmpty((Columns[colIdx] as MySQLDataColumn).MappedDataColName))
            mappedQty++;
        }
        return mappedQty;
      }
    }

    // Constructor used for Export Data
    public MySQLDataTable(string proposedTableName, Excel.Range exportDataRange, bool addPrimaryKeyCol, bool useFormattedValues, bool detectDatatype, bool addBufferToVarchar, bool autoIndexIntColumns, bool autoAllowEmptyNonIndexColumns)
      : this(proposedTableName)
    {
      AddPrimaryKeyColumn = addPrimaryKeyCol;
      RemoveEmptyColumns = true;
      IsFormatted = useFormattedValues;
      SetData(exportDataRange, detectDatatype, addBufferToVarchar, autoIndexIntColumns, autoAllowEmptyNonIndexColumns);
    }

    // Constructor to only fetch Schema Information for columns
    public MySQLDataTable(string tableName, bool fetchColumnsSchemaInfo, MySqlWorkbenchConnection wbConnection) : this(tableName)
    {
      if (fetchColumnsSchemaInfo)
      {
        List<string> primaryKeyColumnNames = new List<string>();
        DataTable indexesInfoTable = MySQLDataUtilities.GetSchemaCollection(wbConnection, "IndexColumns", null, wbConnection.Schema, tableName, null);
        if (indexesInfoTable != null)
        {
          foreach (DataRow indexInfoRow in indexesInfoTable.Rows)
          {
            if (indexInfoRow["INDEX_NAME"].ToString() == "PRIMARY")
              primaryKeyColumnNames.Add(indexInfoRow["COLUMN_NAME"].ToString());
          }
        }
        DataTable columnsInfoTable = MySQLDataUtilities.GetSchemaCollection(wbConnection, "Columns Short", null, wbConnection.Schema, tableName);
        if (columnsInfoTable != null)
          foreach (DataRow columnInfoRow in columnsInfoTable.Rows)
          {
            string colName = columnInfoRow["Field"].ToString();
            string dataType = columnInfoRow["Type"].ToString();
            bool allowNulls = columnInfoRow["Null"].ToString() == "YES";
            bool isPrimaryKey = primaryKeyColumnNames.Contains(colName) || columnInfoRow["Key"].ToString() == "PRI";
            string extraInfo = columnInfoRow["Extra"].ToString();
            MySQLDataColumn column = new MySQLDataColumn(colName, dataType, allowNulls, isPrimaryKey, extraInfo);
            Columns.Add(column);
          }
      }
    }

    // Constructor used for Append Data, totally dummy like the DataTable constructor
    public MySQLDataTable(string tableName) : this()
    {
      if (!String.IsNullOrEmpty(tableName))
        TableName = tableName;
    }

    // Basic constructor
    public MySQLDataTable()
    {
      AddPrimaryKeyColumn = false;
    }

    private void CreateColumns(int numCols)
    {
      MySQLDataColumn column = null;
      int startCol = (AddPrimaryKeyColumn ? 0 : 1);
      for (int colIdx = startCol; colIdx <= numCols; colIdx++)
      {
        string name = "Column" + colIdx;
        column = new MySQLDataColumn();
        column.ColumnName = column.DisplayName = name;
        Columns.Add(column);
      }
      if (AddPrimaryKeyColumn)
      {
        column = (Columns[0] as MySQLDataColumn);
        column.PrimaryKey = true;
        column.AutoPK = true;
        column.ColumnName = column.DisplayName = TableName + "_id";
        column.MySQLDataType = "Integer";
        column.AutoIncrement = true;
      }
    }

    private void UseFirstRowAsHeaders(bool useFirstRow)
    {
      DataRow row = Rows[0];
      int startRow = (AddPrimaryKeyColumn ? 1 : 0);
      for (int i = startRow; i < Columns.Count; i++)
      {
        MySQLDataColumn col = Columns[i] as MySQLDataColumn;
        col.DisplayName = (useFirstRow ? DataToColName(row[i].ToString()) : col.ColumnName);
        col.MySQLDataType = (useFirstRow ? col.RowsFrom2ndDataType : col.RowsFrom1stDataType);
      }
      if (AddPrimaryKeyColumn)
      {
        (Columns[0] as MySQLDataColumn).DisplayName = TableName + "_id";
        int adjustIdx = (useFirstRow ? 0 : 1);
        for (int i = 0; i < Rows.Count; i++)
        {
          Rows[i][0] = i + adjustIdx;
        }
      }
    }

    public void SetData(Excel.Range dataRange, bool detectTypes, bool addBufferToVarchar, bool createIndexForIntColumns, bool allowEmptyNonIdxCols)
    {
      object[,] data;

      // we have to treat a single cell specially.  It doesn't come in as an array but as a single value
      if (dataRange.Count == 1)
      {
        data = new object[2, 2];
        data[1, 1] = IsFormatted ? dataRange.Value : dataRange.Value2;
      }
      else
        data = IsFormatted ? dataRange.Value : dataRange.Value2;

      int numRows = data.GetUpperBound(0);
      int numCols = data.GetUpperBound(1);
      int colAdjustIdx = (AddPrimaryKeyColumn ? 0 : 1);

      List<bool> columnsHaveAnyDataList = new List<bool>(numCols + 1);
      List<string> colsToDelete = new List<string>(numCols);

      columnsHaveAnyDataList.Add(true);
      for (int colIdx = 1; colIdx <= numCols; colIdx++)
      {
        bool colHasAnyData = false;
        for (int rowIdx = 1; rowIdx <= numRows; rowIdx++)
        {
          if (data[rowIdx, colIdx] == null)
            continue;
          colHasAnyData = true;
          break;
        }
        columnsHaveAnyDataList.Add(colHasAnyData);
      }

      if (Columns.Count == 0)
        CreateColumns(numCols);

      int pkRowValueAdjust = 0;
      for (int row = 1; row <= numRows; row++)
      {
        bool rowHasAnyData = false;
        DataRow dataRow = NewRow();
        dataRow[0] = row - pkRowValueAdjust;
        for (int col = 1; col <= numCols; col++)
        {
          int adjColIdx = col - colAdjustIdx;
          MySQLDataColumn column = Columns[adjColIdx] as MySQLDataColumn;
          if (row == 1 && !columnsHaveAnyDataList[col])
          {
            column.ExcludeColumn = true;
            colsToDelete.Add(column.ColumnName);
          }
          rowHasAnyData = rowHasAnyData || data[row, col] != null;
          dataRow[adjColIdx] = data[row, col];
        }
        if (rowHasAnyData)
          Rows.Add(dataRow);
        else
          pkRowValueAdjust++;
      }
      if (detectTypes)
        DetectTypes(data, addBufferToVarchar, createIndexForIntColumns);

      if (RemoveEmptyColumns)
        foreach (string colName in colsToDelete)
          Columns.Remove(Columns[colName]);
      if (allowEmptyNonIdxCols)
        foreach (MySQLDataColumn mysqlCol in Columns)
          mysqlCol.AllowNull = !mysqlCol.CreateIndex;
    }

    [System.Obsolete("Use DetectTypes with parameters instead since this will analyze columns with all data as strings and will always detect everything as a Varchar.")]
    private void DetectTypes()
    {
      foreach (MySQLDataColumn col in Columns)
        col.DetectType(firstRowIsHeaders);
    }

    private void DetectTypes(object[,] data, bool addBufferToVarchar, bool createIndexForIntColumns)
    {
      int rowsCount = data.GetUpperBound(0);
      int colsCount = data.GetUpperBound(1);
      string dateFormat = "yyyy-MM-dd HH:mm:ss";
      int colAdjustIdx = (AddPrimaryKeyColumn ? 0 : 1);

      for (int dataColPos = 1; dataColPos <= colsCount; dataColPos++)
      {
        MySQLDataColumn col = Columns[dataColPos - colAdjustIdx] as MySQLDataColumn;
        if (col.ExcludeColumn)
          continue;

        object valueFromArray = null;
        string proposedType = String.Empty;
        string strippedType = String.Empty;
        string valueAsString = String.Empty;
        bool valueOverflow = false;
        List<string> typesListFor1stAndRest = new List<string>(2);
        List<string> typesListFrom2ndRow = new List<string>(rowsCount - 1);
        int[] varCharMaxLen = new int[2] { 0, 0 };    // 0 - All rows original datatype varcharmaxlen, 1 - All rows Varchar forced datatype maxlen
        int[] decimalMaxLen = new int[2] { 0, 0 };    // 0 - Integral part max length, 1 - decimal part max length
        int lParensIndex = -1;
        int varCharValueLength = 0;

        for (int rowPos = 1; rowPos <= rowsCount; rowPos++)
        {
          valueFromArray = data[rowPos, dataColPos];
          if (valueFromArray == null)
            continue;

          // Treat always as a Varchar value first in case all rows do not have a consistent datatype just to see the varchar len calculated by GetMySQLExportDataType
          valueAsString = valueFromArray.ToString();
          proposedType = DataTypeUtilities.GetMySQLExportDataType(valueAsString, out valueOverflow);
          if (proposedType == "Bool")
            proposedType = "Varchar(5)";
          lParensIndex = proposedType.IndexOf("(");
          varCharValueLength = (addBufferToVarchar ? Int32.Parse(proposedType.Substring(lParensIndex + 1, proposedType.Length - lParensIndex - 2)) : valueAsString.Length);
          varCharMaxLen[1] = Math.Max(varCharValueLength, varCharMaxLen[1]);

          // Normal datatype detection
          proposedType = DataTypeUtilities.GetMySQLExportDataType(valueFromArray, out valueOverflow);
          lParensIndex = proposedType.IndexOf("(");
          strippedType = (lParensIndex < 0 ? proposedType : proposedType.Substring(0, lParensIndex));
          switch (strippedType)
          {
            case "Date":
            case "Datetime":
              DateTime dtValue = (DateTime)valueFromArray;
              Rows[rowPos - 1][dataColPos - colAdjustIdx] = dtValue.ToString(dateFormat);
              break;
            case "Varchar":
                varCharValueLength = (addBufferToVarchar ? Int32.Parse(proposedType.Substring(lParensIndex + 1, proposedType.Length - lParensIndex - 2)) : valueAsString.Length);
                varCharMaxLen[0] = Math.Max(varCharValueLength, varCharMaxLen[0]);
              break;
            case "Decimal":
              int commaPos = proposedType.IndexOf(",");
              decimalMaxLen[0] = Math.Max(Int32.Parse(proposedType.Substring(lParensIndex + 1, commaPos - lParensIndex -1)), decimalMaxLen[0]);
              decimalMaxLen[1] = Math.Max(Int32.Parse(proposedType.Substring(commaPos + 1, proposedType.Length - commaPos - 2)), decimalMaxLen[1]);
              break;
          }
          if (rowPos == 1)
            typesListFor1stAndRest.Add(strippedType);
          else
            typesListFrom2ndRow.Add(strippedType);
        }

        // Get the consistent DataType for all rows except first one.
        proposedType = DataTypeUtilities.GetConsistentDataTypeOnAllRows(strippedType, typesListFrom2ndRow, decimalMaxLen, varCharMaxLen);
        col.RowsFrom2ndDataType = proposedType;

        // Get the consistent DataType between first columnInfoRow and the previously computed consistent DataType for the rest of the rows.
        lParensIndex = proposedType.IndexOf("(");
        strippedType = (lParensIndex < 0 ? proposedType : proposedType.Substring(0, lParensIndex));
        typesListFor1stAndRest.Add(strippedType);
        proposedType = DataTypeUtilities.GetConsistentDataTypeOnAllRows(strippedType, typesListFor1stAndRest, decimalMaxLen, varCharMaxLen);
        col.RowsFrom1stDataType = proposedType;

        col.MySQLDataType = (firstRowIsHeaders ? col.RowsFrom2ndDataType : col.RowsFrom1stDataType);
        col.CreateIndex = (createIndexForIntColumns && col.MySQLDataType == "Integer");
      }
    }

    public bool ColumnIsPrimaryKey(string columnName)
    {
      foreach (MySQLDataColumn col in Columns)
      {
        if (col.DisplayName == columnName && col.PrimaryKey)
          return true;
      }
      return false;
    }

    private string DataToColName(string dataValue)
    {
      return (dataValue != null ? dataValue.Replace(" ", "_").Replace("(", String.Empty).Replace(")", String.Empty) : String.Empty);
    }

    public bool CreateTable(MySqlWorkbenchConnection wbConnection, out Exception exception)
    {
      bool success = false;
      string connectionString = MySQLDataUtilities.GetConnectionString(wbConnection);
      string queryString = GetCreateSQL(false);
      exception = null;

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
      catch (MySqlException ex)
      {
        exception = ex;
      }

      return success;
    }

    public string GetCreateSQL(bool formatNewLinesAndTabs)
    {
      StringBuilder sql = new StringBuilder();
      string nl = (formatNewLinesAndTabs ? Environment.NewLine : " ");
      string nlt = (formatNewLinesAndTabs ? String.Format("{0}\t", Environment.NewLine) : " ");

      sql.AppendFormat("CREATE TABLE `{0}`{1}(", TableName, nl);

      string delimiter = nlt;
      int skipNum = (AddPrimaryKeyColumn ? (useFirstColumnAsPK ? 0 : 1) : 0);
      foreach (MySQLDataColumn col in Columns.OfType<MySQLDataColumn>().Skip(skipNum).Where(c => !c.ExcludeColumn))
      {
        sql.AppendFormat("{0}{1}", delimiter, col.GetSQL());
        delimiter = "," + nlt;
      }
      if (NumberOfPK > 1)
      {
        string pkDelimiter = String.Empty;
        sql.AppendFormat("{0}PRIMARY KEY (", delimiter);
        foreach (MySQLDataColumn col in Columns.OfType<MySQLDataColumn>().Skip(1).Where(c => c.PrimaryKey))
        {
          sql.AppendFormat("{0}`{1}`", pkDelimiter, col.DisplayName);
          pkDelimiter = ",";
        }
        sql.Append(")");
      }
      foreach (MySQLDataColumn col in Columns.OfType<MySQLDataColumn>().Where(c => !(c.AutoPK || c.PrimaryKey || c.UniqueKey || c.ExcludeColumn || !c.CreateIndex)))
        sql.AppendFormat("{0}INDEX `{1}_idx` (`{1}`)", delimiter, col.DisplayName);
      sql.Append(nl);
      sql.Append(")");
      return sql.ToString();
    }

    public string GetInsertSQL(int limit, bool formatNewLinesAndTabs)
    {
      return GetInsertSQL(limit, formatNewLinesAndTabs, false);
    }

    public string GetInsertSQL(int limit, bool formatNewLinesAndTabs, bool insertingMappedColumns)
    {
      int colsCount = Columns.Count;
      if (Rows.Count - (firstRowIsHeaders ? 1 : 0) < 1)
        return null;

      StringBuilder queryString = new StringBuilder();
      string nl = (formatNewLinesAndTabs ? Environment.NewLine : " ");
      int rowIdx = 0;
      int colIdx = 0;
      int startingColNum = (AddPrimaryKeyColumn ? (useFirstColumnAsPK ? 0 : 1) : 0);
      List<bool> columnsRequireQuotes = new List<bool>(colsCount);
      List<string> insertColumnNames = new List<string>(colsCount);

      string rowsSeparator = String.Empty;
      string colsSeparator = String.Empty;
      queryString.AppendFormat("INSERT INTO `{0}`{1}(", TableName, nl);

      for (colIdx = startingColNum; colIdx < colsCount; colIdx++)
      {
        MySQLDataColumn column = Columns[colIdx] as MySQLDataColumn;
        if (column.ExcludeColumn || (insertingMappedColumns && String.IsNullOrEmpty(column.MappedDataColName)))
          continue;
        string insertIntoColName = (insertingMappedColumns ? column.MappedDataColName : column.DisplayName);
        queryString.AppendFormat("{0}`{1}`",
                                 colsSeparator,
                                 insertIntoColName);
        colsSeparator = ",";
        columnsRequireQuotes.Add(column.ColumnsRequireQuotes);
        insertColumnNames.Add(column.DisplayName);
      }
      queryString.AppendFormat("){0}VALUES{0}", nl);

      colsCount = insertColumnNames.Count;
      foreach (DataRow dr in Rows)
      {
        if (firstRowIsHeaders && rowIdx++ == 0)
          continue;
        if (limit > 0 && rowIdx >= limit)
          break;
        queryString.AppendFormat("{0}(", rowsSeparator);
        colsSeparator = String.Empty;
        for (colIdx = startingColNum; colIdx < colsCount; colIdx++)
        {
          bool currColRequiresQuotes = columnsRequireQuotes[colIdx];
          string curentStrValue = dr[insertColumnNames[colIdx]].ToString();
          string valueToDB = String.Empty;

          if (currColRequiresQuotes)
            valueToDB = (String.IsNullOrEmpty(curentStrValue) ? String.Empty : curentStrValue);
          else  // for numeric type then insert a null value if the dr doesn't have any value for this column
            valueToDB = (String.IsNullOrEmpty(curentStrValue) ? @"null" : curentStrValue);
          queryString.AppendFormat("{0}{1}{2}{1}",
                                   colsSeparator,
                                   (currColRequiresQuotes ? "'" : String.Empty),
                                   valueToDB);
          colsSeparator = ",";
        }
        queryString.Append(")");
        if (rowsSeparator.Length == 0)
          rowsSeparator = "," + nl;
      }
      return queryString.ToString();
    }

    public bool InsertDataWithManualQuery(MySqlWorkbenchConnection wbConnection, bool insertingMappedColumns, out Exception exception, out string sqlQuery)
    {
      bool success = false;
      exception = null;
      sqlQuery = GetInsertSQL(-1, true, insertingMappedColumns);

      try
      {
        string connectionString = MySQLDataUtilities.GetConnectionString(wbConnection);
        using (MySqlConnection conn = new MySqlConnection(connectionString))
        {
          conn.Open();
          MySqlCommand cmd = new MySqlCommand(sqlQuery, conn);
          cmd.ExecuteNonQuery();
          success = true;
        }
      }
      catch (MySqlException mysqlEx)
      {
        exception = mysqlEx;
      }
      catch (Exception ex)
      {
        exception = ex;
      }

      return success;
    }

    public bool InsertDataWithAdapter(MySqlWorkbenchConnection wbConnection, out Exception exception)
    {
      bool success = false;
      exception = null;

      DataTable copyOriginal = this.Clone();
      copyOriginal.Merge(this);
      foreach (MySQLDataColumn col in Columns)
      {
        MySQLDataColumn copyCol = copyOriginal.Columns[col.ColumnName] as MySQLDataColumn;
        if (col.ExcludeColumn || (AddPrimaryKeyColumn && !useFirstColumnAsPK && col.AutoPK))
          copyOriginal.Columns.Remove(copyCol);
        else
          copyCol.ColumnName = col.DisplayName;
      }
      if (firstRowIsHeaders)
        copyOriginal.Rows.RemoveAt(0);
      copyOriginal.AcceptChanges();

      string connectionString = MySQLDataUtilities.GetConnectionString(wbConnection);
      // Create & open a SqlConnection, and dispose of it after we are done.
      using (MySqlConnection connection = new MySqlConnection(connectionString))
      {
        try
        {
          MySqlDataAdapter dataAdapter = new MySqlDataAdapter(String.Format("SELECT * FROM {0}", TableName), connection);
          DataTable exportingDataTable = new DataTable();
          dataAdapter.FillSchema(exportingDataTable, SchemaType.Source);
          foreach (DataRow row in copyOriginal.Rows)
          {
            exportingDataTable.LoadDataRow(row.ItemArray, LoadOption.Upsert);
          }
          MySqlCommandBuilder commBuilder = new MySqlCommandBuilder(dataAdapter);
          dataAdapter.InsertCommand = commBuilder.GetInsertCommand();

          int updatedCount = 0;
          updatedCount = dataAdapter.Update(exportingDataTable);
          success = updatedCount > 0;
        }
        catch (MySqlException mysqlEx)
        {
          exception = mysqlEx;
        }
        catch (Exception ex)
        {
          exception = ex;
        }
      }

      return success;
    }
  }

  public class MySQLDataColumn : DataColumn
  {
    private bool uniqueKey;
    private string displayName;
    private List<string> warningTextList = new List<string>(3);

    public bool AutoPK { get; set; }
    public bool CreateIndex { get; set; }
    public bool UniqueKey 
    {
      get { return uniqueKey; }
      set { uniqueKey = value; if (uniqueKey) CreateIndex = true; }
    }
    public string DisplayName
    {
      get { return displayName; }
      set 
      { 
        string trimmedName = value.Trim();
        displayName = trimmedName;
        if (Table == null || Table.Columns.Count < 2)
          return;
        int colIdx = 1;
        while (Table.Columns.OfType<MySQLDataColumn>().Count(col => col.DisplayName == displayName) > 1)
        {
          displayName = trimmedName + colIdx;
        }
      }
    }

    public bool PrimaryKey { get; set; }
    public bool AllowNull { get; set; }
    public bool ExcludeColumn { get; set; }
    public bool Unsigned { get; set; }
    public string MySQLDataType  { get; set; }
    public List<string> WarningTextList { get { return warningTextList; } }
    public string RowsFrom1stDataType { get; set; }
    public string RowsFrom2ndDataType { get; set; }
    public string MappedDataColName { get; set; }

    #region Getter Properties

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

    public string StrippedMySQLDataType
    {
      get
      {
        if (String.IsNullOrEmpty(MySQLDataType))
          return MySQLDataType;
        int lParensIndex = MySQLDataType.IndexOf("(");
        return (lParensIndex < 0 ? MySQLDataType : MySQLDataType.Substring(0, lParensIndex));
      }
    }

    public MySqlDbType MySQLDBType
    {
      get
      {
        string strippedType = StrippedMySQLDataType;
        return (!String.IsNullOrEmpty(strippedType) ? DataTypeUtilities.NameToMySQLType(strippedType, Unsigned, false) : MySqlDbType.VarChar);
      }
    }

    #endregion Getter Properties

    public MySQLDataColumn()
    {
      MappedDataColName = null;
    }

    public MySQLDataColumn(string columnName, string mySQLFullDataType, bool allowNulls, bool isPrimaryKey, string extraInfo) : this()
    {
      ColumnName = columnName;
      AllowDBNull = AllowNull = allowNulls;
      Unsigned = false;
      AutoIncrement = false;
      if (!String.IsNullOrEmpty(extraInfo))
      {
        Unsigned = extraInfo.Contains("unsigned");
        AutoIncrement = extraInfo.Contains("auto_increment");
      }
      MySQLDataType = mySQLFullDataType;
      DataType = DataTypeUtilities.NameToType(StrippedMySQLDataType, Unsigned);
      PrimaryKey = isPrimaryKey;
    }

    public MySQLDataColumn(string columnName, string mySQLFullDataType) : this(columnName, mySQLFullDataType, false, false, String.Empty)
    {
    }

    [System.Obsolete("This will analyze all data as strings and will always detect everything as a Varchar.")]
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
        proposedType = DataTypeUtilities.GetMySQLExportDataType(valueFromArray, out valueOverflow);
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
      RowsFrom1stDataType = headerType;
      RowsFrom2ndDataType = previousType;
      MySQLDataType = (firstRowIsHeaders ? headerType : previousType);
      rowPos++;
    }

    public bool CanBeOfMySQLDataType(string mySQLDataType)
    {
      bool result = true;

      MySQLDataTable parentTable = Table as MySQLDataTable;
      int rowIdx = 0;
      foreach (DataRow dr in parentTable.Rows)
      {
        if (parentTable.FirstRowIsHeaders && rowIdx++ == 0)
          continue;
        string strValueFromArray = dr[Ordinal].ToString();
        result = result && DataTypeUtilities.StringValueCanBeStoredWithMySQLType(strValueFromArray, mySQLDataType);
      }

      return result;
    }

    public string GetSQL()
    {
      if (String.IsNullOrEmpty(displayName))
        return null;

      StringBuilder colDefinition = new StringBuilder();
      colDefinition.AppendFormat("`{0}` {1}", displayName, MySQLDataType);
      if (AutoPK || (PrimaryKey && (Table as MySQLDataTable).NumberOfPK == 1))
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
