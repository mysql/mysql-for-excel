// 
// Copyright (c) 2012, Oracle and/or its affiliates. All rights reserved.
//
// This program is free software; you can redistribute it and/or
// modify it under the terms of the GNU General Public License as
// published by the Free Software Foundation; version 2 of the
// License.
// 
// This program is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the
// GNU General Public License for more details.
// 
// You should have received a copy of the GNU General Public License
// along with this program; if not, write to the Free Software
// Foundation, Inc., 51 Franklin St, Fifth Floor, Boston, MA
// 02110-1301  USA
//

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
    private ulong mysqlMaxAllowedPacket = 0;
    private bool firstRowIsHeaders;
    private bool useFirstColumnAsPK;

    public string SchemaName { get; set; }
    public string SelectQuery { get; set; }
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
      set 
      { 
        firstRowIsHeaders = value;
        UseFirstRowAsHeaders(); 
      }
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
    public MySQLDataTable(string schemaName, string proposedTableName, bool addPrimaryKeyCol, bool useFormattedValues)
      : this(schemaName, proposedTableName)
    {
      AddPrimaryKeyColumn = addPrimaryKeyCol;
      RemoveEmptyColumns = true;
      IsFormatted = useFormattedValues;
    }

    // Constructor used for Append Data, fetching Schema Information for columns
    public MySQLDataTable(string tableName, bool fetchColumnsSchemaInfo, bool datesAsMySQLDates, MySqlWorkbenchConnection wbConnection)
      : this(wbConnection.Schema, tableName)
    {
      if (fetchColumnsSchemaInfo)
      {
        DataTable columnsInfoTable = MySQLDataUtilities.GetSchemaCollection(wbConnection, "Columns Short", null, wbConnection.Schema, tableName);
        if (columnsInfoTable != null)
        {
          foreach (DataRow columnInfoRow in columnsInfoTable.Rows)
          {
            string colName = columnInfoRow["Field"].ToString();
            string dataType = columnInfoRow["Type"].ToString();
            bool allowNulls = columnInfoRow["Null"].ToString() == "YES";
            bool isPrimaryKey = columnInfoRow["Key"].ToString() == "PRI";
            string extraInfo = columnInfoRow["Extra"].ToString();
            MySQLDataColumn column = new MySQLDataColumn(colName, dataType, datesAsMySQLDates, allowNulls, isPrimaryKey, extraInfo);
            Columns.Add(column);
          }
        }
      }
      mysqlMaxAllowedPacket = MySQLDataUtilities.GetMySQLServerMaxAllowedPacket(wbConnection);
    }

    // Constructor used for Edit Data where we copy the contents of a table imported to Excel for edition
    public MySQLDataTable(string tableName, DataTable filledTable, MySqlWorkbenchConnection wbConnection)
      : this(tableName, true, true, wbConnection)
    {
      try
      {
        foreach (DataRow dr in filledTable.Rows)
          ImportRow(dr);
      }
      catch (Exception ex)
      {
        InfoDialog infoDialog = new InfoDialog(false, ex.Message, ex.StackTrace);
        infoDialog.ShowDialog();
        MiscUtilities.GetSourceTrace().WriteError("Application Exception on MySQLDataTable constructor - " + (ex.Message + " " + ex.InnerException), 1);
      }
    }

    // Constructor setting just the Table Name
    public MySQLDataTable(string schemaName, string tableName) : this()
    {
      if (!String.IsNullOrEmpty(schemaName))
        SchemaName = schemaName;
      if (!String.IsNullOrEmpty(tableName))
        TableName = tableName;
      SelectQuery = String.Format("SELECT * FROM `{0}`.`{1}`", schemaName, tableName.Replace("`", "``"));
    }

    // Dummy constructor
    public MySQLDataTable()
    {
      SchemaName = String.Empty;
      SelectQuery = String.Format("SELECT * FROM `{0}`", TableName.Replace("`", "``"));
      AddPrimaryKeyColumn = false;
    }

    public void RefreshSelectQuery()
    {
      string schemaPiece = (!String.IsNullOrEmpty(SchemaName) ? String.Format("`{0}`.", SchemaName) : String.Empty);
      SelectQuery = String.Format("SELECT * FROM {0}`{1}`", schemaPiece, TableName.Replace("`", "``"));
    }

    public string GetNonDuplicateColumnName(string proposedName)
    {
      if (String.IsNullOrEmpty(proposedName) || Columns == null || Columns.Count == 0)
        return proposedName;
      proposedName = proposedName.Trim();
      string nonDupName = proposedName;
      int colIdx = 2;
      while (Columns.OfType<MySQLDataColumn>().Count(col => col.DisplayName == nonDupName) > 0)
      {
        nonDupName = proposedName + colIdx++;
      }
      return nonDupName;
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
        column.ColumnName = column.DisplayName = String.Format("{0}{1}id", TableName, (TableName.Length > 0 ? "_" : String.Empty));
        column.MySQLDataType = "Integer";
        column.AutoIncrement = true;
        column.AllowNull = false;
      }
    }

    private void AdjustAutoPKValues()
    {
      if (AddPrimaryKeyColumn && Columns.Count > 0)
      {
        int adjustIdx = (firstRowIsHeaders ? 0 : 1);
        for (int i = 0; i < Rows.Count; i++)
        {
          Rows[i][0] = i + adjustIdx;
        }
      }
    }

    private void UseFirstRowAsHeaders()
    {
      if (Rows.Count == 0)
        return;
      DataRow row = Rows[0];
      int startRow = (AddPrimaryKeyColumn ? 1 : 0);
      for (int i = startRow; i < Columns.Count; i++)
      {
        MySQLDataColumn col = Columns[i] as MySQLDataColumn;
        col.DisplayName = (firstRowIsHeaders ? DataToColName(row[i].ToString()) : col.ColumnName);
        col.MySQLDataType = (firstRowIsHeaders ? col.RowsFrom2ndDataType : col.RowsFrom1stDataType);
      }
      AdjustAutoPKValues();
    }

    public void SetData(Excel.Range dataRange, bool recreateColumnsFromData, bool detectTypes, bool addBufferToVarchar, bool createIndexForIntColumns, bool allowEmptyNonIdxCols, bool emptyColumnsToVarchar)
    {
      object[,] data;
      Clear();

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

      if (recreateColumnsFromData || Columns.Count == 0)
      {
        if (Columns.Count > 0)
          Columns.Clear();
        CreateColumns(numCols);
      }

      int pkRowValueAdjust = (firstRowIsHeaders ? 1 : 0);
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
          if (data[row, col] != null && data[row, col].Equals(0.0) && column.IsDate)
            dataRow[adjColIdx] = DataTypeUtilities.EMPTY_DATE;
          else
            dataRow[adjColIdx] = data[row, col];
        }
        if (rowHasAnyData)
          Rows.Add(dataRow);
        else
          pkRowValueAdjust++;
      }
      if (detectTypes)
        DetectTypes(data, addBufferToVarchar, createIndexForIntColumns, emptyColumnsToVarchar);

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

    private void DetectTypes(object[,] data, bool addBufferToVarchar, bool createIndexForIntColumns, bool emptyColumnsToVarchar)
    {
      int rowsCount = data.GetUpperBound(0);
      int colsCount = data.GetUpperBound(1);
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
          proposedType = strippedType = valueAsString = String.Empty;
          valueFromArray = data[rowPos, dataColPos];
          if (valueFromArray == null)
            continue;

          // Treat always as a Varchar value first in case all rows do not have a consistent datatype just to see the varchar len calculated by GetMySQLExportDataType
          valueAsString = valueFromArray.ToString();
          proposedType = DataTypeUtilities.GetMySQLExportDataType(valueAsString, out valueOverflow);
          if (proposedType == "Bool")
            proposedType = "Varchar(5)";
          else if (proposedType.StartsWith("Date"))
            proposedType = String.Format("Varchar({0})", valueAsString.Length);
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
              bool zeroDate = valueAsString.StartsWith("0000-00-00") || valueAsString.StartsWith("00-00-00");
              if (zeroDate)
                break;
              DateTime dtValue = (DateTime)valueFromArray;
              Rows[rowPos - 1][dataColPos - colAdjustIdx] = dtValue.ToString(DataTypeUtilities.DATE_FORMAT);
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
        if (emptyColumnsToVarchar && String.IsNullOrEmpty(proposedType))
          proposedType = "Varchar(255)";
        col.RowsFrom2ndDataType = proposedType;
        if (proposedType == "Datetime")
          foreach (DataRow dr in Rows)
          {
            if (dr[dataColPos - colAdjustIdx].ToString() == "0")
              dr[dataColPos - colAdjustIdx] = DataTypeUtilities.EMPTY_DATE;
          }

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

    public MySQLDataTable CloneSchema()
    {
      MySQLDataTable clonedTable = new MySQLDataTable(this.SchemaName, this.TableName, this.AddPrimaryKeyColumn, this.IsFormatted);
      clonedTable.UseFirstColumnAsPK = UseFirstColumnAsPK;
      clonedTable.RemoveEmptyColumns = RemoveEmptyColumns;
      clonedTable.IsFormatted = IsFormatted;
      clonedTable.FirstRowIsHeaders = FirstRowIsHeaders;

      foreach (MySQLDataColumn column in Columns)
      {
        MySQLDataColumn clonedColumn = column.CloneSchema();
        clonedTable.Columns.Add(clonedColumn);
      }

      return clonedTable;
    }

    public void SyncSchema(MySQLDataTable syncFromTable)
    {
      if (syncFromTable.Columns.Count != Columns.Count)
        return;

      for (int colIdx = 0; colIdx < Columns.Count; colIdx++)
      {
        MySQLDataColumn thisColumn = Columns[colIdx] as MySQLDataColumn;
        MySQLDataColumn syncFromColumn = syncFromTable.Columns[colIdx] as MySQLDataColumn;

        thisColumn.DisplayName = syncFromColumn.DisplayName;
        thisColumn.MySQLDataType = syncFromColumn.MySQLDataType;
        thisColumn.PrimaryKey = syncFromColumn.PrimaryKey;
        thisColumn.AllowNull = syncFromColumn.AllowNull;
        thisColumn.UniqueKey = syncFromColumn.UniqueKey;
        thisColumn.CreateIndex = syncFromColumn.CreateIndex;
        thisColumn.ExcludeColumn = syncFromColumn.ExcludeColumn;
      }
    }

    public bool ColumnIsPrimaryKey(string columnName, bool caseSensitive)
    {
      if (!caseSensitive)
        columnName = columnName.ToLowerInvariant();
      foreach (MySQLDataColumn col in Columns)
      {
        if ((caseSensitive ? col.DisplayName : col.DisplayName.ToLowerInvariant()) == columnName && col.PrimaryKey)
          return true;
      }
      return false;
    }

    public bool ColumnIsPrimaryKey(string columnName)
    {
      return ColumnIsPrimaryKey(columnName, true);
    }

    public int GetColumnIndex(string columnName, bool displayName, bool caseSensitive)
    {
      int index = -1;

      if (!caseSensitive)
        columnName = columnName.ToLowerInvariant();
      foreach (MySQLDataColumn col in Columns)
      {
        if (displayName && (caseSensitive ? col.DisplayName : col.DisplayName.ToLowerInvariant()) == columnName)
          index = col.Ordinal;
        else if (!displayName && (caseSensitive ? col.ColumnName : col.ColumnName.ToLowerInvariant()) == columnName)
          index = col.Ordinal;
        if (index >= 0)
          break;
      }

      return index;
    }

    public MySQLDataColumn GetColumnAtIndex(int index)
    {
      MySQLDataColumn col = null;

      if (index < Columns.Count)
        col = Columns[index] as MySQLDataColumn;

      return col;
    }

    public int GetColumnIndex(string columnName, bool displayName)
    {
      return GetColumnIndex(columnName, displayName, true);
    }

    public string[] GetColumnNamesArray(bool displayName)
    {
      string[] retArray = null;

      if (Columns.Count > 0)
      {
        retArray = new string[Columns.Count];
        for (int i = 0; i < Columns.Count; i++)
          if (displayName)
            retArray[i] = (Columns[i] as MySQLDataColumn).DisplayName;
          else
            retArray[i] = Columns[i].ColumnName;
      }

      return retArray;
    }

    public string[] GetColumnNamesArray()
    {
      return GetColumnNamesArray(false);
    }

    private string DataToColName(string dataValue)
    {
      return (dataValue != null ? dataValue.Replace(" ", "_").Replace("(", String.Empty).Replace(")", String.Empty) : String.Empty);
    }

    public string GetCreateSQL(bool formatNewLinesAndTabs)
    {
      StringBuilder sql = new StringBuilder();
      string nl = (formatNewLinesAndTabs ? Environment.NewLine : " ");
      string nlt = (formatNewLinesAndTabs ? String.Format("{0}\t", Environment.NewLine) : " ");

      sql.AppendFormat("CREATE TABLE `{0}`.`{1}`{2}(", SchemaName, TableName, nl);

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
          sql.AppendFormat("{0}`{1}`", pkDelimiter, col.DisplayName.Replace("`", "``"));
          pkDelimiter = ",";
        }
        sql.Append(")");
      }
      foreach (MySQLDataColumn col in Columns.OfType<MySQLDataColumn>().Where(c => !(c.AutoPK || c.PrimaryKey || c.UniqueKey || c.ExcludeColumn || !c.CreateIndex)))
        sql.AppendFormat("{0}INDEX `{1}_idx` (`{1}`)", delimiter, col.DisplayName.Replace("`", "``"));
      sql.Append(nl);
      sql.Append(")");
      return sql.ToString();
    }

    public DataTable CreateTable(MySqlWorkbenchConnection wbConnection, out Exception exception, out string sqlQuery)
    {
      DataSet warningsDS = null;
      string connectionString = MySQLDataUtilities.GetConnectionString(wbConnection);
      exception = null;
      sqlQuery = GetCreateSQL(true);

      try
      {
        using (MySqlConnection conn = new MySqlConnection(connectionString))
        {
          conn.Open();

          MySqlCommand cmd = new MySqlCommand(sqlQuery, conn);
          cmd.ExecuteNonQuery();
          warningsDS = MySqlHelper.ExecuteDataset(conn, "SHOW WARNINGS");
        }
      }
      catch (MySqlException ex)
      {
        exception = ex;
        MiscUtilities.GetSourceTrace().WriteError("Application Exception on MySQLDataTable.CreateTable - " + (ex.Message + " " + ex.InnerException), 1);
      }

      return (warningsDS != null && warningsDS.Tables.Count > 0 ? warningsDS.Tables[0] : null);
    }

    public string GetInsertSQL(int startRow, int limit, bool formatNewLinesAndTabs, bool newRowsOnly, out int nextRow)
    {
      nextRow = -1;
      DataRowCollection valueRows = null;

      if (newRowsOnly)
      {
        DataTable changesTable = GetChanges(DataRowState.Added);
        valueRows = (changesTable != null ? changesTable.Rows : null);
      }
      else
        valueRows = Rows;
      if (valueRows == null)
        return null;

      if (startRow < 0)
        startRow = 0;
      if (!newRowsOnly && firstRowIsHeaders && startRow == 0)
        startRow++;
      if (startRow >= valueRows.Count)
        return null;

      ulong maxByteCount = (mysqlMaxAllowedPacket > 0 ? mysqlMaxAllowedPacket - 10 : 0);
      ulong queryStringByteCount = 0;
      StringBuilder queryString = new StringBuilder();
      string nl = (formatNewLinesAndTabs ? Environment.NewLine : " ");
      int rowIdx = 0;
      int colIdx = 0;
      int startingColNum = (AddPrimaryKeyColumn ? (useFirstColumnAsPK ? 0 : 1) : 0);
      List<string> insertColumnNames = new List<string>(Columns.Count);

      string rowsSeparator = String.Empty;
      string colsSeparator = String.Empty;
      queryString.AppendFormat("INSERT INTO `{0}`.`{1}`{2}(",
                               SchemaName,
                               TableName.Replace("`", "``"),
                               nl);

      for (colIdx = startingColNum; colIdx < Columns.Count; colIdx++)
      {
        MySQLDataColumn column = Columns[colIdx] as MySQLDataColumn;
        if (column.ExcludeColumn)
          continue;
        queryString.AppendFormat("{0}`{1}`",
                                 colsSeparator,
                                 column.DisplayName.Replace("`", "``"));
        colsSeparator = ",";
        insertColumnNames.Add(column.ColumnName);
      }
      queryString.AppendFormat("){0}VALUES{0}", nl);

      if (valueRows != null)
      {
        bool insertingValueIsNull = false;
        int absRowIdx = 0;
        string valueToDB = String.Empty;
        StringBuilder singleRowValuesBuilder = new StringBuilder();
        string singleRowValuesString = String.Empty;
        if (maxByteCount > 0)
          queryStringByteCount = (ulong)ASCIIEncoding.ASCII.GetByteCount(queryString.ToString());

        for (rowIdx = startRow; rowIdx < valueRows.Count; rowIdx++)
        {
          if (limit > 0 && absRowIdx > limit)
          {
            if (rowIdx < valueRows.Count)
              nextRow = rowIdx;
            break;
          }
          else
            absRowIdx++;
          DataRow dr = valueRows[rowIdx];
          singleRowValuesBuilder.Clear();
          singleRowValuesString = String.Empty;
          singleRowValuesBuilder.AppendFormat("{0}(", rowsSeparator);
          colsSeparator = String.Empty;
          foreach (string insertingColName in insertColumnNames)
          {
            MySQLDataColumn column = Columns[insertingColName] as MySQLDataColumn;
            valueToDB = DataTypeUtilities.GetStringValueForColumn(dr[insertingColName], column, true, out insertingValueIsNull);
            singleRowValuesBuilder.AppendFormat("{0}{1}{2}{1}",
                                                colsSeparator,
                                                (column.ColumnsRequireQuotes && !insertingValueIsNull ? "'" : String.Empty),
                                                valueToDB);
            colsSeparator = ",";
          }
          singleRowValuesBuilder.Append(")");

          singleRowValuesString = singleRowValuesBuilder.ToString();
          if (maxByteCount > 0)
          {
            ulong singleValueRowQueryByteCount = (ulong)ASCIIEncoding.ASCII.GetByteCount(singleRowValuesString);
            if (queryStringByteCount + singleValueRowQueryByteCount > maxByteCount)
            {
              nextRow = rowIdx;
              break;
            }
            queryStringByteCount += singleValueRowQueryByteCount;
          }

          queryString.Append(singleRowValuesString);
          if (rowsSeparator.Length == 0)
            rowsSeparator = "," + nl;
        }
        if (nextRow >= 0)
          queryString.AppendFormat(";{0}", nl);
      }

      return queryString.ToString();
    }

    public string GetInsertSQL(int limit, bool formatNewLinesAndTabs, bool newRowsOnly)
    {
      int nextRow = -1;
      return GetInsertSQL(0, limit, formatNewLinesAndTabs, newRowsOnly, out nextRow);
    }

    public DataTable InsertDataWithManualQuery(MySqlWorkbenchConnection wbConnection, bool newRowsOnly, out Exception exception, out string sqlQuery, out int insertedRows)
    {
      DataSet warningsDS = null;
      insertedRows = 0;
      exception = null;
      MySqlTransaction transaction = null;
      string chunkQuery = sqlQuery = String.Empty;

      string connectionString = MySQLDataUtilities.GetConnectionString(wbConnection);
      using (MySqlConnection conn = new MySqlConnection(connectionString))
      {
        try
        {
          conn.Open();
          if (mysqlMaxAllowedPacket == 0)
            mysqlMaxAllowedPacket = MySQLDataUtilities.GetMySQLServerMaxAllowedPacket(conn);
          transaction = conn.BeginTransaction();
          MySqlCommand cmd = new MySqlCommand(String.Empty, conn, transaction);
          int nextRow = 0;
          while (nextRow >= 0)
          {
            chunkQuery = GetInsertSQL(nextRow, -1, true, newRowsOnly, out nextRow);
            cmd.CommandText = chunkQuery;
            sqlQuery += chunkQuery;
            insertedRows += cmd.ExecuteNonQuery();
          }
          transaction.Commit();
          if (newRowsOnly)
            foreach (DataRow dr in Rows)
              if (dr.RowState == DataRowState.Added)
                dr.AcceptChanges();
          warningsDS = MySqlHelper.ExecuteDataset(conn, "SHOW WARNINGS");
        }
        catch (MySqlException mysqlEx)
        {
          if (transaction != null)
            transaction.Rollback();
          exception = mysqlEx;
          MiscUtilities.GetSourceTrace().WriteError("Application Exception on MySQLDataTable.InsertDataWithManualQuery - " + (mysqlEx.Message + " " + mysqlEx.InnerException), 1);
        }
        catch (Exception ex)
        {
          if (transaction != null)
            transaction.Rollback();
          exception = ex;
          MiscUtilities.GetSourceTrace().WriteError("Application Exception on MySQLDataTable.InsertDataWithManualQuery - " + (ex.Message + " " + ex.InnerException), 1);
        }
      }

      return (warningsDS != null && warningsDS.Tables.Count > 0 ? warningsDS.Tables[0] : null);
    }

    public List<string> GetChangedColumns(DataRow changesRow)
    {
      List<string> changedColNamesList = null;

      if (changesRow != null)
      {
        changedColNamesList = new List<string>(changesRow.Table.Columns.Count);
        foreach (DataColumn col in changesRow.Table.Columns)
        {
          if (!changedColNamesList.Contains(col.ColumnName) && !changesRow[col, DataRowVersion.Original].Equals(changesRow[col, DataRowVersion.Current]))
            changedColNamesList.Add(col.ColumnName);
        }
      }

      return changedColNamesList;
    }

    public string GetAppendSQL(int startRow, int limit, bool formatNewLinesAndTabs, MySQLDataTable mappingFromTable, out int nextRow)
    {
      nextRow = -1;
      ulong maxByteCount = (mysqlMaxAllowedPacket > 0 ? mysqlMaxAllowedPacket - 10 : 0);
      int colsCount = Columns.Count;
      int rowsCount = mappingFromTable.Rows.Count;

      if (startRow < 0)
        startRow = 0;
      if (mappingFromTable.FirstRowIsHeaders && startRow == 0)
        startRow++;
      if (mappingFromTable != null && mappingFromTable.Rows.Count - (mappingFromTable.FirstRowIsHeaders ? 1 : 0) < 1)
        return null;
      if (startRow > rowsCount)
        return null;

      ulong queryStringByteCount = 0;
      StringBuilder queryString = new StringBuilder();
      string nl = (formatNewLinesAndTabs ? Environment.NewLine : " ");
      int rowIdx = 0;
      int colIdx = 0;
      int startingColNum = (AddPrimaryKeyColumn ? (useFirstColumnAsPK ? 0 : 1) : 0);
      List<string> fromColumnNames = new List<string>(colsCount);
      List<string> toColumnNames = new List<string>(colsCount);

      string rowsSeparator = String.Empty;
      string colsSeparator = String.Empty;
      queryString.AppendFormat("INSERT INTO `{0}`.`{1}`{2}(",
                               SchemaName,
                               TableName.Replace("`", "``"),
                               nl);

      for (colIdx = startingColNum; colIdx < colsCount; colIdx++)
      {
        MySQLDataColumn toColumn = Columns[colIdx] as MySQLDataColumn;
        string fromColumnName = toColumn.MappedDataColName;
        if (toColumn.ExcludeColumn || String.IsNullOrEmpty(fromColumnName))
          continue;
        queryString.AppendFormat("{0}`{1}`",
                                 colsSeparator,
                                 toColumn.ColumnName.Replace("`", "``"));
        colsSeparator = ",";
        fromColumnNames.Add(fromColumnName);
        toColumnNames.Add(toColumn.ColumnName);
      }
      queryString.AppendFormat("){0}VALUES{0}", nl);

      string valueToDB = String.Empty;
      int absRowIdx = 0;
      StringBuilder singleRowValuesBuilder = new StringBuilder();
      string singleRowValuesString = String.Empty;
      if (maxByteCount > 0)
        queryStringByteCount = (ulong)ASCIIEncoding.ASCII.GetByteCount(queryString.ToString());

      for (rowIdx = startRow; rowIdx < rowsCount; rowIdx++)
      {
        DataRow dr = mappingFromTable.Rows[rowIdx];
        if (limit > 0 && absRowIdx > limit)
        {
          if (rowIdx < rowsCount)
            nextRow = rowIdx;
          break;
        }
        else
          absRowIdx++;
        singleRowValuesBuilder.Clear();
        singleRowValuesString = String.Empty;
        singleRowValuesBuilder.AppendFormat("{0}(", rowsSeparator);
        colsSeparator = String.Empty;
        for (colIdx = 0; colIdx < toColumnNames.Count; colIdx++)
        {
          string fromColumnName = fromColumnNames[colIdx];
          string toColumnName = toColumnNames[colIdx];
          MySQLDataColumn toColumn = Columns[toColumnName] as MySQLDataColumn;
          bool insertingValueIsNull = false;
          valueToDB = DataTypeUtilities.GetStringValueForColumn(dr[fromColumnName], toColumn, true, out insertingValueIsNull);
          singleRowValuesBuilder.AppendFormat("{0}{1}{2}{1}",
                                              colsSeparator,
                                              (toColumn.ColumnsRequireQuotes && !insertingValueIsNull ? "'" : String.Empty),
                                              valueToDB);
          colsSeparator = ",";
        }
        singleRowValuesBuilder.Append(")");

        singleRowValuesString = singleRowValuesBuilder.ToString();
        if (maxByteCount > 0)
        {
          ulong singleValueRowQueryByteCount = (ulong)ASCIIEncoding.ASCII.GetByteCount(singleRowValuesString);
          if (queryStringByteCount + singleValueRowQueryByteCount > maxByteCount)
          {
            nextRow = rowIdx;
            break;
          }
          queryStringByteCount += singleValueRowQueryByteCount;
        }

        queryString.Append(singleRowValuesString);
        if (rowsSeparator.Length == 0)
          rowsSeparator = "," + nl;
      }
      if (nextRow >= 0)
        queryString.AppendFormat(";{0}", nl);

      return queryString.ToString();
    }

    public DataTable AppendDataWithManualQuery(MySqlWorkbenchConnection wbConnection, MySQLDataTable mappingFromTable, out Exception exception, out string sqlQuery, out int insertedCount)
    {
      DataSet warningsDS = null;
      insertedCount = 0;
      exception = null;
      MySqlTransaction transaction = null;
      string chunkQuery = sqlQuery = String.Empty;

      string connectionString = MySQLDataUtilities.GetConnectionString(wbConnection);
      using (MySqlConnection conn = new MySqlConnection(connectionString))
      {
        try
        {
          conn.Open();
          if (mysqlMaxAllowedPacket == 0)
            mysqlMaxAllowedPacket = MySQLDataUtilities.GetMySQLServerMaxAllowedPacket(conn);
          transaction = conn.BeginTransaction();
          MySqlCommand cmd = new MySqlCommand(String.Empty, conn, transaction);
          int nextRow = 0;
          while (nextRow >= 0)
          {
            chunkQuery = GetAppendSQL(nextRow, -1, true, mappingFromTable, out nextRow);
            cmd.CommandText = chunkQuery;
            sqlQuery += chunkQuery;
            insertedCount += cmd.ExecuteNonQuery();
          }
          transaction.Commit();
          warningsDS = MySqlHelper.ExecuteDataset(conn, "SHOW WARNINGS");
        }
        catch (MySqlException mysqlEx)
        {
          if (transaction != null)
            transaction.Rollback();
          exception = mysqlEx;
          MiscUtilities.GetSourceTrace().WriteError("Application Exception on MySQLDataTable.AppendDataWithManualQuery - " + (mysqlEx.Message + " " + mysqlEx.InnerException), 1);
        }
        catch (Exception ex)
        {
          if (transaction != null)
            transaction.Rollback();
          exception = ex;
          MiscUtilities.GetSourceTrace().WriteError("Application Exception on MySQLDataTable.AppendDataWithManualQuery- " + (ex.Message + " " + ex.InnerException), 1);
        }
      }

      return (warningsDS != null && warningsDS.Tables.Count > 0 ? warningsDS.Tables[0] : null);
    }

    public string GetUpdateSQL(int startRow, int limit, bool formatNewLinesAndTabs, out int nextRow)
    {
      nextRow = -1;
      DataTable changesTable = GetChanges(DataRowState.Modified);
      if (changesTable == null || changesTable.Rows.Count == 0)
        return null;

      ulong maxByteCount = (mysqlMaxAllowedPacket > 0 ? mysqlMaxAllowedPacket - 10 : 0);
      StringBuilder queryString = new StringBuilder();
      StringBuilder wClauseString = new StringBuilder();
      string nl = (formatNewLinesAndTabs ? Environment.NewLine : " ");
      int rowIdx = 0;
      string rowsSeparator = String.Empty;
      string colsSeparator = String.Empty;
      string wClauseColsSeparator = String.Empty;

      for (rowIdx = 0; rowIdx < changesTable.Rows.Count; rowIdx++)
      {
        DataRow changesRow = changesTable.Rows[rowIdx];
        List<string> changedColNamesList = GetChangedColumns(changesRow);
        queryString.AppendFormat("{0}UPDATE `{1}`.`{2}` SET ",
                                 rowsSeparator,
                                 SchemaName,
                                 TableName.Replace("`", "``"));
        wClauseString.Clear();
        wClauseString.Append(" WHERE ");
        wClauseColsSeparator = colsSeparator = String.Empty;
        foreach (MySQLDataColumn column in Columns)
        {
          bool updatingValueIsNull = false;
          string valueToDB = String.Empty;
          string finalColName = column.ColumnName.Replace("`", "``");

          if (column.PrimaryKey)
          {
            valueToDB = DataTypeUtilities.GetStringValueForColumn(changesRow[column.ColumnName, DataRowVersion.Original], column, false, out updatingValueIsNull);
            wClauseString.AppendFormat("{0}`{1}`={2}{3}{2}",
                                       colsSeparator,
                                       finalColName,
                                       (column.ColumnsRequireQuotes && !updatingValueIsNull ? "'" : String.Empty),
                                       valueToDB);
            wClauseColsSeparator = " AND ";
          }
          if (changedColNamesList.Contains(column.ColumnName))
          {
            valueToDB = DataTypeUtilities.GetStringValueForColumn(changesRow[column.ColumnName], column, true, out updatingValueIsNull);
            queryString.AppendFormat("{0}`{1}`={2}{3}{2}",
                                      colsSeparator,
                                      finalColName,
                                      (column.ColumnsRequireQuotes && !updatingValueIsNull ? "'" : String.Empty),
                                      valueToDB);
            colsSeparator = ",";
          }
        }
        rowsSeparator = ";" + nl;
        queryString.Append(wClauseString.ToString());
      }

      return queryString.ToString();
    }

    public DataTable UpdateDataWithManualQuery(MySqlWorkbenchConnection wbConnection, out Exception exception, out string sqlQuery, out int updatedCount)
    {
      DataSet warningsDS = null;
      updatedCount = 0;
      exception = null;
      MySqlTransaction transaction = null;
      string chunkQuery = sqlQuery = String.Empty;

      string connectionString = MySQLDataUtilities.GetConnectionString(wbConnection);
      using (MySqlConnection conn = new MySqlConnection(connectionString))
      {
        try
        {
          conn.Open();
          if (mysqlMaxAllowedPacket == 0)
            mysqlMaxAllowedPacket = MySQLDataUtilities.GetMySQLServerMaxAllowedPacket(conn);
          transaction = conn.BeginTransaction();
          MySqlCommand cmd = new MySqlCommand(String.Empty, conn, transaction);
          int nextRow = 0;
          while (nextRow >= 0)
          {
            chunkQuery = GetUpdateSQL(nextRow, -1, true, out nextRow);
            cmd.CommandText = chunkQuery;
            sqlQuery += chunkQuery;
            updatedCount += cmd.ExecuteNonQuery();
          }
          transaction.Commit();
          foreach (DataRow dr in Rows)
            if (dr.RowState == DataRowState.Modified)
              dr.AcceptChanges();
          warningsDS = MySqlHelper.ExecuteDataset(conn, "SHOW WARNINGS");
        }
        catch (MySqlException mysqlEx)
        {
          if (transaction != null)
            transaction.Rollback();
          exception = mysqlEx;
          MiscUtilities.GetSourceTrace().WriteError("Application Exception on MySQLDataTable.UpdateDataWithManualQuery - " + (mysqlEx.Message + " " + mysqlEx.InnerException), 1);
        }
        catch (Exception ex)
        {
          if (transaction != null)
            transaction.Rollback();
          exception = ex;
          MiscUtilities.GetSourceTrace().WriteError("Application Exception on MySQLDataTable.UpdateDataWithManualQuery - " + (ex.Message + " " + ex.InnerException), 1);
        }
      }

      return (warningsDS != null && warningsDS.Tables.Count > 0 ? warningsDS.Tables[0] : null);
    }

    public string GetDeleteSQL(int startRow, int limit, bool formatNewLinesAndTabs, out int nextRow)
    {
      nextRow = -1;
      DataTable changesTable = GetChanges(DataRowState.Deleted);
      if (changesTable == null || changesTable.Rows.Count == 0)
        return null;

      ulong maxByteCount = (mysqlMaxAllowedPacket > 0 ? mysqlMaxAllowedPacket - 10 : 0);
      StringBuilder queryString = new StringBuilder();
      string nl = (formatNewLinesAndTabs ? Environment.NewLine : " ");
      int rowIdx = 0;
      string rowsSeparator = String.Empty;
      string colsSeparator = String.Empty;
      bool pkValueIsNull = false;

      for (rowIdx = 0; rowIdx < changesTable.Rows.Count; rowIdx++)
      {
        DataRow changesRow = changesTable.Rows[rowIdx];
        queryString.AppendFormat("{0}DELETE FROM `{1}`.`{2}` WHERE ",
                                 rowsSeparator,
                                 SchemaName,
                                 TableName.Replace("`", "``"));
        colsSeparator = String.Empty;
        foreach (MySQLDataColumn pkCol in Columns)
        {
          if (!pkCol.PrimaryKey)
            continue;
          string valueToDB = DataTypeUtilities.GetStringValueForColumn(changesRow[pkCol.ColumnName, DataRowVersion.Original], pkCol, false, out pkValueIsNull);
          queryString.AppendFormat("{0}`{1}`={2}{3}{2}",
                                    colsSeparator,
                                    pkCol.ColumnName.Replace("`", "``"),
                                    (pkCol.ColumnsRequireQuotes && !pkValueIsNull ? "'" : String.Empty),
                                    valueToDB);
          colsSeparator = " AND ";
        }
        rowsSeparator = ";" + nl;
      }

      return queryString.ToString();
    }

    public DataTable DeleteDataWithManualQuery(MySqlWorkbenchConnection wbConnection, out Exception exception, out string sqlQuery, out int deletedCount)
    {
      DataSet warningsDS = null;
      deletedCount = 0;
      exception = null;
      MySqlTransaction transaction = null;
      string chunkQuery = sqlQuery = String.Empty;

      string connectionString = MySQLDataUtilities.GetConnectionString(wbConnection);
      using (MySqlConnection conn = new MySqlConnection(connectionString))
      {
        try
        {
          conn.Open();
          if (mysqlMaxAllowedPacket == 0)
            mysqlMaxAllowedPacket = MySQLDataUtilities.GetMySQLServerMaxAllowedPacket(conn);
          transaction = conn.BeginTransaction();
          MySqlCommand cmd = new MySqlCommand(String.Empty, conn, transaction);
          int nextRow = 0;
          while (nextRow >= 0)
          {
            chunkQuery = GetDeleteSQL(nextRow, -1, true, out nextRow);
            cmd.CommandText = chunkQuery;
            sqlQuery += chunkQuery;
            deletedCount += cmd.ExecuteNonQuery();
          }
          transaction.Commit();
          for (int rowIdx = 0; rowIdx < Rows.Count; rowIdx++)
          {
            DataRow dr = Rows[rowIdx];
            if (dr.RowState == DataRowState.Deleted)
            {
              dr.AcceptChanges();
              rowIdx--;
            }
          }
          warningsDS = MySqlHelper.ExecuteDataset(conn, "SHOW WARNINGS");
        }
        catch (MySqlException mysqlEx)
        {
          if (transaction != null)
            transaction.Rollback();
          exception = mysqlEx;
          MiscUtilities.GetSourceTrace().WriteError("Application Exception on MySQLDataTable.DeleteDataWithManualQuery - " + (mysqlEx.Message + " " + mysqlEx.InnerException), 1);
        }
        catch (Exception ex)
        {
          if (transaction != null)
            transaction.Rollback();
          exception = ex;
          MiscUtilities.GetSourceTrace().WriteError("Application Exception on MySQLDataTable.DeleteDataWithManualQuery - " + (ex.Message + " " + ex.InnerException), 1);
        }
      }

      return (warningsDS != null && warningsDS.Tables.Count > 0 ? warningsDS.Tables[0] : null);
    }

    public void RevertData(bool refreshFromDB, MySqlWorkbenchConnection wbConnection, out Exception exception)
    {
      exception = null;

      if (!refreshFromDB)
      {
        RejectChanges();
        return;
      }

      string connectionString = MySQLDataUtilities.GetConnectionString(wbConnection);
      // Create & open a SqlConnection, and dispose of it after we are done.
      using (MySqlConnection connection = new MySqlConnection(connectionString))
      {
        try
        {
          MySqlDataAdapter dataAdapter = new MySqlDataAdapter(SelectQuery, connection);
          Clear();
          dataAdapter.Fill(this);
        }
        catch (MySqlException mysqlEx)
        {
          exception = mysqlEx;
          MiscUtilities.GetSourceTrace().WriteError("Application Exception on MySQLDataTable.RevertData - " + (mysqlEx.Message + " " + mysqlEx.InnerException), 1);
        }
        catch (Exception ex)
        {
          exception = ex;
          MiscUtilities.GetSourceTrace().WriteError("Application Exception on MySQLDataTable.RevertData - " + (ex.Message + " " + ex.InnerException), 1);
        }
      }
    }

  }
  
}
