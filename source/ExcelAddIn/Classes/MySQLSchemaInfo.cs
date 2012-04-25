using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using MySql.Data.MySqlClient;
using System.Windows.Forms;

namespace MySQL.ExcelAddIn
{
  public class MySQLSchemaInfo : IDisposable
  {
    private bool disposed = false;
    private MySqlConnection mysqlConnection;
    private MySQLConnectionData connectionData;
    private string currentSchema = String.Empty;
    private List<string> dataTypesList;
    private DataSet schemaInfoDS;

    public MySQLConnectionData ConnectionData 
    {
      get { return connectionData; }
      set
      {
        connectionData = value;
        if (connectionData != null)
          openConnection();
        RefreshSchemas();
      }
    }

    public List<string> DataTypesList
    {
      get { return dataTypesList; }
    }

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
      dataTypesList = new List<string>();
      dataTypesList.AddRange(new string[] {"bit", "tinyint", "smallint", "mediumint", "int", "bigint", "real", "double", "float",
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

    public void Clear()
    {
      schemaInfoDS.Clear();
      closeConnection();
    }

    private bool openConnection()
    {
      bool success = false;

      if (connectionData != null && connectionData.ConnectionString == String.Empty)
        throw new Exception(Properties.Resources.connectionStringNotSet);

      try
      {
        if (mysqlConnection.ConnectionString != connectionData.ConnectionString)
        {
          if (mysqlConnection.State != ConnectionState.Closed)
            mysqlConnection.Close();
          mysqlConnection.ConnectionString = connectionData.ConnectionString;
        }

        if (mysqlConnection.State == ConnectionState.Closed)
        {
          mysqlConnection.Open();
          success = true;
        }
      }
      catch (MySqlException mysqlEx)
      {
        MessageBox.Show(mysqlEx.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
      }
      catch (Exception ex)
      {
        MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
      }

      return success;
    }

    private void closeConnection()
    {
      if (mysqlConnection.State != ConnectionState.Closed)
      {
        try
        {
          mysqlConnection.Close();
        }
        catch (MySqlException mysqlEx)
        {
          MessageBox.Show(mysqlEx.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
        catch (Exception ex)
        {
          MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
      }
      connectionData = null;
    }

    public void RefreshEngines()
    {
      EnginesTable.Clear();

      try
      {
        if (mysqlConnection.State == ConnectionState.Closed)
          Utilities.ShowErrorBox(Properties.Resources.connectionClosedError);

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

      if (connectionData == null)
        return;

      if (schemaInfoDS.Tables["Engines"].Rows.Count == 0)
        RefreshEngines();

      try
      {
        if (mysqlConnection.State == ConnectionState.Closed)
          Utilities.ShowErrorBox(Properties.Resources.connectionClosedError);

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

      try
      {
        if (mysqlConnection.State == ConnectionState.Closed)
          Utilities.ShowErrorBox(Properties.Resources.connectionClosedError);

        string queryString;
        MySqlDataAdapter mysqlAdapter;

        // Refresh Tables
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

        foreach (DataRow dr in ColumnsTable.Select(String.Format("TABLE_NAME = '{0}'", tableName)))
        {
          newRow = retTable.NewRow();

          newRow["MappedColIdx"] = -1;
          newRow["Name"] = dr["COLUMN_NAME"].ToString();
          newRow["Type"] = dr["DATA_TYPE"].ToString();
          if (dr["CHARACTER_MAXIMUM_LENGTH"] != null && dr["CHARACTER_MAXIMUM_LENGTH"] != DBNull.Value)
            newRow["Length"] = Convert.ToInt32(dr["CHARACTER_MAXIMUM_LENGTH"]);
          else if (dr["NUMERIC_PRECISION"] != null && dr["NUMERIC_PRECISION"] != DBNull.Value)
            newRow["Length"] = Convert.ToInt32(dr["NUMERIC_PRECISION"]);
          else
            newRow["Length"] = 0;
          if (dr["NUMERIC_SCALE"] != null && dr["NUMERIC_SCALE"] != DBNull.Value)
            newRow["Decimals"] = Convert.ToInt32(dr["NUMERIC_SCALE"]);
          columnType = dr["COLUMN_TYPE"].ToString();
          newRow["Unsigned"] = columnType.Contains("unsigned");
          newRow["ZeroFill"] = columnType.Contains("zerofill");
          newRow["Binary"] = columnType.Contains("binary");
          newRow["DefaultValue"] = (dr["COLUMN_DEFAULT"] != null && dr["COLUMN_DEFAULT"] != DBNull.Value ? dr["COLUMN_DEFAULT"].ToString() : String.Empty);
          newRow["AutoIncrement"] = dr["EXTRA"].ToString().Contains("auto_increment");
          newRow["Nullable"] = dr["IS_NULLABLE"].ToString() == "YES";
          newRow["PrimaryKey"] = dr["COLUMN_KEY"].ToString() == "PRI";
          newRow["UniqueKey"] = dr["COLUMN_KEY"].ToString() == "UNI";
        }
      }
      return retTable;
    }

    public DataTable GetTableData(string tableName, List<string> columnNames, string whereClause)
    {
      DataTable retTable = null;

      if (mysqlConnection.State == ConnectionState.Closed)
        Utilities.ShowErrorBox(Properties.Resources.connectionClosedError);
      if (String.IsNullOrEmpty(currentSchema))
        Utilities.ShowErrorBox(Properties.Resources.selectedDBSchemaNull);
      if (string.IsNullOrEmpty(tableName))
        Utilities.ShowErrorBox(Properties.Resources.selectedTableNull);

      try
      {
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
  }

  public class TableSchemaInfo : DataTable
  {
    public TableSchemaInfo()
    {
      Columns.Add("MappedColIdx", Type.GetType("System.Int32"));
      Columns.Add("Name", Type.GetType("System.String"));
      Columns.Add("Type", Type.GetType("System.String"));
      Columns.Add("Length", Type.GetType("System.Int32"));
      Columns.Add("Decimals", Type.GetType("System.Int32"));
      Columns.Add("Unsigned", Type.GetType("System.Boolean"));
      Columns.Add("ZeroFill", Type.GetType("System.Boolean"));
      Columns.Add("Binary", Type.GetType("System.Boolean"));
      Columns.Add("DefaultValue", Type.GetType("System.String"));
      Columns.Add("AutoIncrement", Type.GetType("System.Boolean"));
      Columns.Add("Nullable", Type.GetType("System.Boolean"));
      Columns.Add("PrimaryKey", Type.GetType("System.Boolean"));
      Columns.Add("UniqueKey", Type.GetType("System.Boolean"));
    }
  }
}
