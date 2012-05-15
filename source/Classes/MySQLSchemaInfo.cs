using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using MySql.Data.MySqlClient;
using System.Windows.Forms;
using MySQL.Utility;

namespace MySQL.ExcelAddIn
{
  public class MySQLSchemaInfo : IDisposable
  {
    private bool disposed = false;
    private MySqlConnection mysqlConnection;
    private string currentSchema = String.Empty;
    private DataSet schemaInfoDS;

    public List<string> DataTypesList { get; private set; }
    public MySQLConnectionData ConnectionData { get; private set; }

    public string CurrentSchema
    {
      get { return currentSchema; }
      set
      {
        bool currentSchemaChanged = currentSchema != value;
        currentSchema = value;
        if (currentSchemaChanged && mysqlConnection.State == ConnectionState.Open)
          RefreshSchemaObjects();
      }
    }

    public DataTable EnginesTable { get { return schemaInfoDS.Tables["Engines"]; } }
    public DataTable SchemasTable { get { return schemaInfoDS.Tables["Schemas"]; } }
    public DataTable TablesTable { get { return schemaInfoDS.Tables["Tables"]; } }
    public DataTable ColumnsTable { get { return schemaInfoDS.Tables["Columns"]; } }
    public DataTable ViewsTable { get { return schemaInfoDS.Tables["Views"]; } }
    public DataTable RoutinesTable { get { return schemaInfoDS.Tables["Routines"]; } }
    public DataTable ParametersTable { get { return schemaInfoDS.Tables["Parameters"]; } }

    public MySQLSchemaInfo()
    {
      DataTypesList = new List<string>();
      DataTypesList.AddRange(new string[] {"bit", "tinyint", "smallint", "mediumint", "int", "bigint", "real", "double", "float",
                                           "decimal", "numeric", "date", "time", "timestamp", "datetime", "year", "char", "varchar",
                                           "binary", "varbinary", "tinyblob", "blob", "mediumblob", "longblob", "tinytext", "text",
                                           "mediumtext", "longtext", "enum", "set"});

      schemaInfoDS = new DataSet("SchemaInfoDS");
      schemaInfoDS.Tables.Add("Engines");
      schemaInfoDS.Tables.Add("Schemas");
      schemaInfoDS.Tables.Add("Tables");
      schemaInfoDS.Tables.Add("Columns");
      schemaInfoDS.Tables.Add("Views");
      schemaInfoDS.Tables.Add("Routines");
      schemaInfoDS.Tables.Add("Parameters");

      mysqlConnection = new MySqlConnection();
    }

    public void Dispose()
    {
      Dispose(true);
      GC.SuppressFinalize(this);
    }

    protected virtual void Dispose(bool disposing)
    {
      if (!disposed)
      {
        if (disposing)
        {
          if (schemaInfoDS != null)
            schemaInfoDS.Dispose();
          if (mysqlConnection != null)
          {
            if (mysqlConnection.State != ConnectionState.Closed)
              mysqlConnection.Close();
            mysqlConnection.Dispose();
          }
        }
        disposed = true;
      }
    }

    public bool OpenConnection(MySQLConnectionData connData)
    {
      bool success = false;

      if (connData == null)
        return false;

      if (connData.ConnectionString == String.Empty)
      {
        Utilities.ShowErrorBox(Properties.Resources.connectionStringNotSet);
        return success;
      }
      
      try
      {
        if (mysqlConnection.ConnectionString != connData.ConnectionString)
        {
          if (mysqlConnection.State != ConnectionState.Closed)
            mysqlConnection.Close();
          mysqlConnection.ConnectionString = connData.ConnectionString;
        }

        if (mysqlConnection.State == ConnectionState.Closed)
        {
          mysqlConnection.Open();
          success = true;
        }
      }
      catch (MySqlException mysqlEx)
      {
        Utilities.ShowExceptionBox(mysqlEx);
      }
      catch (Exception ex)
      {
        Utilities.ShowExceptionBox(ex);
      }
     

      if (success)
      {
        ConnectionData = connData;
        RefreshSchemas();
      }

      return success;
    }

    public bool CloseConnection()
    {
      bool success = false;
      if (mysqlConnection.State != ConnectionState.Closed)
      {
        schemaInfoDS.Clear();
        try
        {
          mysqlConnection.Close();
          success = true;
        }
        catch (MySqlException mysqlEx)
        {
          Utilities.ShowExceptionBox(mysqlEx);
        }
        catch (Exception ex)
        {
          Utilities.ShowExceptionBox(ex);
        }
      }
      ConnectionData = null;
      return success;
    }

    public void RefreshEngines()
    {
      EnginesTable.Clear();

      try
      {
        if (mysqlConnection.State == ConnectionState.Closed)
        {
          Utilities.ShowErrorBox(Properties.Resources.connectionClosedError);
          return;
        }
        
        MySqlDataAdapter mysqlAdapter = new MySqlDataAdapter("SELECT * FROM information_schema.engines ORDER BY engine", mysqlConnection);
        mysqlAdapter.Fill(EnginesTable);
      }
      catch (MySqlException mysqlEx)
      {
        Utilities.ShowExceptionBox(mysqlEx);
      }
      catch (Exception ex)
      {
        Utilities.ShowExceptionBox(ex);
      }
    }

    public void RefreshSchemas()
    {
      CurrentSchema = String.Empty;
      SchemasTable.Clear();

      if (ConnectionData == null)
        return;
      if (mysqlConnection.State == ConnectionState.Closed)
      {
        Utilities.ShowErrorBox(Properties.Resources.connectionClosedError);
        return;
      }

      if (schemaInfoDS.Tables["Engines"].Rows.Count == 0)
        RefreshEngines();

      try
      {
        MySqlDataAdapter mysqlAdapter = new MySqlDataAdapter("SELECT * FROM information_schema.schemata ORDER BY schema_name", mysqlConnection);
        mysqlAdapter.Fill(SchemasTable);
      }
      catch (MySqlException mysqlEx)
      {
        Utilities.ShowExceptionBox(mysqlEx);
      }
      catch (Exception ex)
      {
        Utilities.ShowExceptionBox(ex);
      }
    }

    public void RefreshSchemaObjects()
    {
      TablesTable.Clear();
      ColumnsTable.Clear();
      ViewsTable.Clear();
      RoutinesTable.Clear();
      ParametersTable.Clear();

      if (currentSchema == String.Empty)
        return;
      if (mysqlConnection.State == ConnectionState.Closed)
      {
        Utilities.ShowErrorBox(Properties.Resources.connectionClosedError);
        return;
      }

      string queryString;
      MySqlDataAdapter mysqlAdapter;

      try
      {
        queryString = String.Format("SELECT * FROM information_schema.tables WHERE table_schema = '{0}' and table_type = '{1}'",
                                  currentSchema,
                                  (currentSchema.ToLowerInvariant() == "information_schema" ? "SYSTEM VIEW" : "BASE TABLE"));
        mysqlAdapter = new MySqlDataAdapter(queryString, mysqlConnection);
        mysqlAdapter.Fill(TablesTable);

        // Refresh Columns
        queryString = String.Format("SELECT * FROM information_schema.columns WHERE table_schema = '{0}'",
                                    currentSchema);
        mysqlAdapter = new MySqlDataAdapter(queryString, mysqlConnection);
        mysqlAdapter.Fill(ColumnsTable);

        // Refresh Views
        queryString = String.Format("SELECT * FROM information_schema.views WHERE table_schema = '{0}'",
                                    currentSchema);
        mysqlAdapter = new MySqlDataAdapter(queryString, mysqlConnection);
        mysqlAdapter.Fill(ViewsTable);

        // Refresh Routines
        queryString = String.Format("SELECT * FROM information_schema.routines WHERE routine_schema = '{0}'",
                                    currentSchema);
        mysqlAdapter = new MySqlDataAdapter(queryString, mysqlConnection);
        mysqlAdapter.Fill(RoutinesTable);

        // Refresh Parameters
        queryString = String.Format("SELECT * FROM information_schema.parameters WHERE specific_schema = '{0}'",
                                    currentSchema);
        mysqlAdapter = new MySqlDataAdapter(queryString, mysqlConnection);
        mysqlAdapter.Fill(ParametersTable);
      }
      catch (MySqlException mysqlEx)
      {
        Utilities.ShowExceptionBox(mysqlEx);
      }
      catch (Exception ex)
      {
        Utilities.ShowExceptionBox(ex);
      }
    }

    public TableSchemaInfo GetTableSchemaInfo(string tableName)
    {
      TableSchemaInfo retTable = null;

      if (ColumnsTable.Rows.Count > 0)
      {
        DataRow newRow = null;
        retTable = new TableSchemaInfo();
        string columnType;

        //Dummy Row
        newRow = retTable.NewRow();

        foreach (DataRow dr in ColumnsTable.Select(String.Format("TABLE_NAME = '{0}'", tableName)))
        {
          newRow = retTable.NewRow();

          newRow["Name"] = dr["COLUMN_NAME"].ToString();
          newRow["Type"] = dr["DATA_TYPE"].ToString();
          if (dr["CHARACTER_MAXIMUM_LENGTH"] != null && dr["CHARACTER_MAXIMUM_LENGTH"] != DBNull.Value)
            newRow["Length"] = Convert.ToInt64(dr["CHARACTER_MAXIMUM_LENGTH"]);
          else if (dr["NUMERIC_PRECISION"] != null && dr["NUMERIC_PRECISION"] != DBNull.Value)
            newRow["Length"] = Convert.ToInt64(dr["NUMERIC_PRECISION"]);
          else
            newRow["Length"] = 0;
          if (dr["NUMERIC_SCALE"] != null && dr["NUMERIC_SCALE"] != DBNull.Value)
            newRow["Decimals"] = Convert.ToInt64(dr["NUMERIC_SCALE"]);
          columnType = dr["COLUMN_TYPE"].ToString();
          newRow["Unsigned"] = columnType.Contains("unsigned");
          newRow["ZeroFill"] = columnType.Contains("zerofill");
          newRow["Binary"] = columnType.Contains("binary");
          newRow["DefaultValue"] = (dr["COLUMN_DEFAULT"] != null && dr["COLUMN_DEFAULT"] != DBNull.Value ? dr["COLUMN_DEFAULT"].ToString() : String.Empty);
          newRow["AutoIncrement"] = dr["EXTRA"].ToString().Contains("auto_increment");
          newRow["Nullable"] = dr["IS_NULLABLE"].ToString() == "YES";
          newRow["PrimaryKey"] = dr["COLUMN_KEY"].ToString() == "PRI";
          newRow["UniqueKey"] = dr["COLUMN_KEY"].ToString() == "UNI";

          retTable.Rows.Add(newRow);
        }
      }
      return retTable;
    }

    public DataTable GetTableData(string tableName, List<string> columnNames, string whereClause)
    {
      DataTable retTable = null;

      if (mysqlConnection.State == ConnectionState.Closed)
      {
        Utilities.ShowErrorBox(Properties.Resources.connectionClosedError);
        return retTable;
      }
      if (String.IsNullOrEmpty(currentSchema))
      {
        Utilities.ShowErrorBox(Properties.Resources.selectedDBSchemaNull);
        return retTable;
      }
      if (string.IsNullOrEmpty(tableName))
      {
        Utilities.ShowErrorBox(Properties.Resources.selectedTableNull);
        return retTable;
      }

      StringBuilder queryString = new StringBuilder("SELECT ");
      if (columnNames == null || columnNames.Count == 0)
        queryString.Append("*");
      else
      {
        for (int idx = 0; idx < columnNames.Count; idx++)
        {
          queryString.Append(columnNames[idx]);
          if (idx < columnNames.Count - 1)
            queryString.Append(", ");
        }
      }
      queryString.AppendFormat(" FROM {0}.{1}",
                               currentSchema,
                               tableName);
      if (whereClause.Length > 0)
        queryString.AppendFormat(" WHERE {0}", whereClause);

      try
      {
        MySqlDataAdapter mysqlAdapter = new MySqlDataAdapter(queryString.ToString(), mysqlConnection);
        retTable = new DataTable(String.Format("{0}Data", tableName));
        mysqlAdapter.Fill(retTable);
      }
      catch (MySqlException mysqlEx)
      {
        Utilities.ShowExceptionBox(mysqlEx);
      }
      catch (Exception ex)
      {
        Utilities.ShowExceptionBox(ex);
      }

      return retTable;
    }

    public bool CreateTable(string newTableName, string dbEngine, TableSchemaInfo schemaInfo)
    {
      bool success = false;

      if (mysqlConnection.State == ConnectionState.Closed)
      {
        Utilities.ShowErrorBox(Properties.Resources.connectionClosedError);
        return success;
      }
      if (String.IsNullOrEmpty(currentSchema))
      {
        Utilities.ShowErrorBox(Properties.Resources.selectedDBSchemaNull);
        return success;
      }
      if (string.IsNullOrEmpty(newTableName))
      {
        Utilities.ShowErrorBox(Properties.Resources.selectedTableNull);
        return success;
      }

      StringBuilder queryString = new StringBuilder();
      queryString.AppendFormat("USE {0}; CREATE TABLE", currentSchema);
      queryString.AppendFormat(" {0} (", newTableName);
      DataRow[] resultSet = schemaInfo.Select("MappedColIdx >= 0", "MappedColIdx ASC");

      foreach (DataRow dr in resultSet)
      {
        queryString.AppendFormat("{0} {1}, ",
                                 dr["Name"].ToString(),
                                 schemaInfo.GetColumnDefinition(dr));
      }
      if (resultSet.Length > 0)
        queryString.Remove(queryString.Length - 2, 2);
      queryString.AppendFormat(") ENGINE={0};", dbEngine);

      try
      {
        MySqlCommand cmd = new MySqlCommand(queryString.ToString(), mysqlConnection);
        cmd.ExecuteNonQuery();
        success = true;
      }
      catch (MySqlException mysqlEx)
      {
        Utilities.ShowExceptionBox(mysqlEx);
      }
      catch (Exception ex)
      {
        Utilities.ShowExceptionBox(ex);
      }

      return success;
    }

    public bool InsertData(string toTableName, DataTable insertingData, bool firstRowHeader, TableSchemaInfo schemaInfo)
    {
      bool success = false;

      if (mysqlConnection.State == ConnectionState.Closed)
      {
        Utilities.ShowErrorBox(Properties.Resources.connectionClosedError);
        return success;
      }
      if (String.IsNullOrEmpty(currentSchema))
      {
        Utilities.ShowErrorBox(Properties.Resources.selectedDBSchemaNull);
        return success;
      }
      if (string.IsNullOrEmpty(toTableName))
      {
        Utilities.ShowErrorBox(Properties.Resources.selectedTableNull);
        return success;
      }

      StringBuilder queryString = new StringBuilder();
      queryString.AppendFormat("USE {0}; INSERT INTO", currentSchema);
      queryString.AppendFormat(" {0} (", toTableName);
      DataRow[] resultSet = schemaInfo.Select("MappedColIdx >= 0", "MappedColIdx ASC");
      List<int> mappedColumnIndexes = new List<int>();
      List<string> mappedColumnTypes = new List<string>();
      int rowIdx = 0;

      foreach (DataRow dr in resultSet)
      {
        mappedColumnIndexes.Add(Convert.ToInt32(dr["MappedColIdx"]));
        mappedColumnTypes.Add(dr["Type"].ToString());
        queryString.AppendFormat("{0},", dr["Name"].ToString());
      }
      if (resultSet.Length > 0)
        queryString.Remove(queryString.Length - 1, 1);
      queryString.Append(") VALUES ");

      foreach (DataRow dr in insertingData.Rows)
      {
        if (firstRowHeader && rowIdx++ == 0)
          continue;
        queryString.Append("(");
        for(int colIdx = 0; colIdx < mappedColumnIndexes.Count; colIdx++)
        {
          queryString.AppendFormat("{0}{1}{0},",
                                   (mappedColumnTypes[colIdx].Contains("char") || mappedColumnTypes[colIdx].Contains("text") || mappedColumnTypes[colIdx].Contains("date") ? "'" : String.Empty),
                                   dr[mappedColumnIndexes[colIdx]].ToString());
        }
        if (mappedColumnIndexes.Count > 0)
          queryString.Remove(queryString.Length - 1, 1);
        queryString.Append("),");
      }
      if (insertingData.Rows.Count > 0)
        queryString.Remove(queryString.Length - 1, 1);
      queryString.Append(";");

      try
      {
        MySqlCommand cmd = new MySqlCommand(queryString.ToString(), mysqlConnection);
        cmd.ExecuteNonQuery();
        success = true;
      }
      catch (MySqlException mysqlEx)
      {
        Utilities.ShowExceptionBox(mysqlEx);
      }
      catch (Exception ex)
      {
        Utilities.ShowExceptionBox(ex);
      }

      return success;
    }
  }

  public class TableSchemaInfo : DataTable
  {
    public TableSchemaInfo()
    {
      Columns.Add("Name", Type.GetType("System.String"));
      Columns["Name"].DefaultValue = String.Empty;
      Columns.Add("HeaderName", Type.GetType("System.String"));
      Columns["HeaderName"].DefaultValue = String.Empty;
      Columns.Add("GivenName", Type.GetType("System.String"));
      Columns["GivenName"].DefaultValue = String.Empty;
      Columns.Add("Type", Type.GetType("System.String"));
      Columns["Type"].DefaultValue = String.Empty;
      Columns.Add("Length", Type.GetType("System.Int64"));
      Columns["Length"].DefaultValue = 0;
      Columns.Add("Decimals", Type.GetType("System.Int64"));
      Columns["Decimals"].DefaultValue = 0;
      Columns.Add("Unsigned", Type.GetType("System.Boolean"));
      Columns["Unsigned"].DefaultValue = false;
      Columns.Add("ZeroFill", Type.GetType("System.Boolean"));
      Columns["ZeroFill"].DefaultValue = false;
      Columns.Add("Binary", Type.GetType("System.Boolean"));
      Columns["Binary"].DefaultValue = false;
      Columns.Add("DefaultValue", Type.GetType("System.String"));
      Columns["DefaultValue"].DefaultValue = String.Empty;
      Columns.Add("AutoIncrement", Type.GetType("System.Boolean"));
      Columns["AutoIncrement"].DefaultValue = false;
      Columns.Add("Nullable", Type.GetType("System.Boolean"));
      Columns["Nullable"].DefaultValue = false;
      Columns.Add("PrimaryKey", Type.GetType("System.Boolean"));
      Columns["PrimaryKey"].DefaultValue = false;
      Columns.Add("UniqueKey", Type.GetType("System.Boolean"));
      Columns["UniqueKey"].DefaultValue = false;
      Columns.Add("MappedColIdx", Type.GetType("System.Int32"));
      Columns["MappedColIdx"].DefaultValue = -1;
      PrimaryKey = new DataColumn[] { Columns["Name"] };
    }

    public static TableSchemaInfo GetTableSchemaInfo(MySqlWorkbenchConnection wbConnection, string tableName)
    {
      TableSchemaInfo retTable = null;

      DataTable columnsTable = Utilities.GetSchemaCollection(wbConnection, "Columns", null, wbConnection.Schema, tableName);
      if (columnsTable.Rows.Count > 0)
      {
        DataRow newRow = null;
        retTable = new TableSchemaInfo();
        string columnType;

        //Dummy Row
        newRow = retTable.NewRow();

        foreach (DataRow dr in columnsTable.Rows)
        {
          newRow = retTable.NewRow();

          newRow["Name"] = dr["COLUMN_NAME"].ToString();
          newRow["Type"] = dr["DATA_TYPE"].ToString();
          if (dr["CHARACTER_MAXIMUM_LENGTH"] != null && dr["CHARACTER_MAXIMUM_LENGTH"] != DBNull.Value)
            newRow["Length"] = Convert.ToInt64(dr["CHARACTER_MAXIMUM_LENGTH"]);
          else if (dr["NUMERIC_PRECISION"] != null && dr["NUMERIC_PRECISION"] != DBNull.Value)
            newRow["Length"] = Convert.ToInt64(dr["NUMERIC_PRECISION"]);
          else
            newRow["Length"] = 0;
          if (dr["NUMERIC_SCALE"] != null && dr["NUMERIC_SCALE"] != DBNull.Value)
            newRow["Decimals"] = Convert.ToInt64(dr["NUMERIC_SCALE"]);
          columnType = dr["COLUMN_TYPE"].ToString();
          newRow["Unsigned"] = columnType.Contains("unsigned");
          newRow["ZeroFill"] = columnType.Contains("zerofill");
          newRow["Binary"] = columnType.Contains("binary");
          newRow["DefaultValue"] = (dr["COLUMN_DEFAULT"] != null && dr["COLUMN_DEFAULT"] != DBNull.Value ? dr["COLUMN_DEFAULT"].ToString() : String.Empty);
          newRow["AutoIncrement"] = dr["EXTRA"].ToString().Contains("auto_increment");
          newRow["Nullable"] = dr["IS_NULLABLE"].ToString() == "YES";
          newRow["PrimaryKey"] = dr["COLUMN_KEY"].ToString() == "PRI";
          newRow["UniqueKey"] = dr["COLUMN_KEY"].ToString() == "UNI";

          retTable.Rows.Add(newRow);
        }
      }
      return retTable;
    }

    public string GetColumnDefinition(DataRow dr)
    {
      string dataType = dr["Type"].ToString().ToLowerInvariant();
      StringBuilder colDefinition =  new StringBuilder(dataType);
      bool isBit = dataType == "bit";
      bool isDecimal = dataType == "real" || dataType == "double" || dataType == "float" || dataType == "decimal" || dataType == "numeric";
      bool isNum = isDecimal || dataType.Contains("int");
      bool isChar = dataType.Contains("char");
      bool isBinary = dataType.Contains("binary");
      bool isText = dataType.Contains("text");
      long valLength = Convert.ToInt64(dr["Length"]);
      long valDecimals = Convert.ToInt64(dr["Decimals"]);
      bool valUnsigned = Convert.ToBoolean(dr["Unsigned"]);
      bool valZeroFill = Convert.ToBoolean(dr["ZeroFill"]);
      bool valBinary = Convert.ToBoolean(dr["Binary"]);
      bool valNullable = Convert.ToBoolean(dr["Nullable"]);
      string valDefValue = dr["DefaultValue"].ToString();
      bool valAutoIncrement = Convert.ToBoolean(dr["AutoIncrement"]);
      bool valUniqueKey = Convert.ToBoolean(dr["UniqueKey"]);
      bool valPrimaryKey = Convert.ToBoolean(dr["PrimaryKey"]);

      if (isBit || isNum || isChar || isBinary)
      {
        if (valLength > 0)
          colDefinition.AppendFormat("({0}", valLength);
        if (valDecimals > 0)
          colDefinition.AppendFormat(",{0}", valDecimals);
        if (valLength > 0)
          colDefinition.Append(")");
      }
      else if(isText && valBinary)
        colDefinition.Append(" binary");
      if (valUnsigned)
        colDefinition.Append(" unsigned");
      if (valZeroFill)
        colDefinition.Append(" zerofill");
      if (valNullable)
        colDefinition.Append(" null");
      if (valDefValue.Length > 0)
        colDefinition.AppendFormat(" default {0}{1}{0}",
                                   (isChar || isText ? "'" : String.Empty),
                                   valDefValue);
      if (valAutoIncrement)
        colDefinition.Append(" auto_increment");
      if (valUniqueKey)
        colDefinition.Append(" unique key");
      else if (valPrimaryKey)
        colDefinition.Append(" primary key");

      return colDefinition.ToString();
    }
  }
}
