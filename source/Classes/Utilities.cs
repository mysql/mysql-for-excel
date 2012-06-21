using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Drawing.Imaging;
using System.Windows.Forms;
using System.Data;
using MySql.Data.MySqlClient;
using MySQL.Utility;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Globalization;

namespace MySQL.ForExcel
{
  public static class Utilities
  {
    public static string GetConnectionString(MySqlWorkbenchConnection connection)
    {
      MySqlConnectionStringBuilder cs = new MySqlConnectionStringBuilder();
      cs.Server = connection.Host;
      cs.UserID = connection.UserName;
      cs.Password = connection.Password;
      cs.Database = connection.Schema;
      //TODO:  use additional necessary options
      return cs.ConnectionString;
    }

    public static DataTable GetSchemaCollection(MySqlWorkbenchConnection wbConnection, string collection, params string[] restrictions)
    {
      string connectionString = GetConnectionString(wbConnection);
      DataTable dt = null;
      MySqlDataAdapter mysqlAdapter = null;

      try
      {
        using (MySqlConnection conn = new MySqlConnection(connectionString))
        {
          conn.Open();

          switch (collection.ToUpperInvariant())
          {
            case "ENGINES":
              mysqlAdapter = new MySqlDataAdapter("SELECT * FROM information_schema.engines ORDER BY engine", conn);
              dt = new DataTable();
              mysqlAdapter.Fill(dt);
              break;
            case "COLLATIONS":
              string queryString;
              if (restrictions != null && restrictions.Length > 0 && !String.IsNullOrEmpty(restrictions[0]))
                queryString = String.Format("SHOW COLLATION WHERE charset = '{0}'", restrictions[0]);
              else
                queryString = "SHOW COLLATION";
              mysqlAdapter = new MySqlDataAdapter(queryString, conn);
              dt = new DataTable();
              mysqlAdapter.Fill(dt);
              break;
            case "CHARSETS":
              mysqlAdapter = new MySqlDataAdapter("SHOW CHARSET", conn);
              dt = new DataTable();
              mysqlAdapter.Fill(dt);
              break;
            default:
              dt = conn.GetSchema(collection, restrictions);
              break;
          }
        }
      }
      catch (Exception ex)
      {
        System.Diagnostics.Debug.WriteLine(ex.Message);
      }

      return dt;
    }

    public static DataTable GetDataFromDbObject(MySqlWorkbenchConnection connection, DBObject dbo)
    {
      string sql;
      if (dbo.Type == DBObjectType.Routine)
        sql = String.Format("CALL `{0}`", dbo.Name);
      else
        sql = String.Format("SELECT * FROM `{0}`", dbo.Name);

      DataSet ds = MySqlHelper.ExecuteDataset(GetConnectionString(connection), sql);
      if (ds.Tables.Count == 0) return null;
      return ds.Tables[0];
    }

    public static long GetRowsCountFromTableOrView(MySqlWorkbenchConnection connection, DBObject dbo)
    {
      if (dbo.Type == DBObjectType.Routine)
        return 0;

      string sql = String.Format("SELECT COUNT(*) FROM `{0}`", dbo.Name);
      object objCount = MySqlHelper.ExecuteScalar(GetConnectionString(connection), sql);
      long retCount = (objCount != null ? (long)objCount : 0);
      return retCount;
    }

    public static void AddExtendedProperties(ref DataTable dt, string queryString, bool importedHeaders, string tableName)
    {
      if (dt.ExtendedProperties.ContainsKey("QueryString"))
        dt.ExtendedProperties["QueryString"] = queryString;
      else
        dt.ExtendedProperties.Add("QueryString", queryString);
      if (dt.ExtendedProperties.ContainsKey("ImportedHeaders"))
        dt.ExtendedProperties["ImportedHeaders"] = importedHeaders;
      else
        dt.ExtendedProperties.Add("ImportedHeaders", importedHeaders);
      if (dt.ExtendedProperties.ContainsKey("TableName"))
        dt.ExtendedProperties["TableName"] = tableName;
      else
        dt.ExtendedProperties.Add("TableName", tableName);
    }

    private static string assembleSelectQuery(DBObject dbo, List<string> columnsList, int firstRowIdx, int rowCount)
    {
      StringBuilder queryStringBuilder = new StringBuilder("SELECT ");
      if (columnsList == null || columnsList.Count == 0)
        queryStringBuilder.Append("*");
      else
      {
        foreach (string columnName in columnsList)
        {
          queryStringBuilder.AppendFormat("`{0}`,", columnName);
        }
        queryStringBuilder.Remove(queryStringBuilder.Length - 1, 1);
      }
      queryStringBuilder.AppendFormat(" FROM `{0}`", dbo.Name);
      if (firstRowIdx > 0)
      {
        string strCount = (rowCount >= 0 ? rowCount.ToString() : "18446744073709551615");
        queryStringBuilder.AppendFormat(" LIMIT {0},{1}", firstRowIdx, strCount);
      }
      else if (rowCount >= 0)
        queryStringBuilder.AppendFormat(" LIMIT {0}", rowCount);
      return queryStringBuilder.ToString();
    }

    public static DataTable GetDataFromTableOrView(MySqlWorkbenchConnection connection, string query)
    {
      DataTable retTable = null;
      DataSet ds = MySqlHelper.ExecuteDataset(GetConnectionString(connection), query);
      if (ds.Tables.Count > 0)
      {
        retTable = ds.Tables[0];
        AddExtendedProperties(ref retTable, query, true, String.Empty);
      }
      return retTable;
    }

    public static DataTable GetDataFromTableOrView(MySqlWorkbenchConnection connection, DBObject dbo, List<string> columnsList, int firstRowIdx, int rowCount)
    {
      if (dbo.Type == DBObjectType.Routine)
        return null;

      string queryString = assembleSelectQuery(dbo, columnsList, firstRowIdx, rowCount);
      return GetDataFromTableOrView(connection, queryString);
    }

    public static DataTable GetDataFromTableOrView(MySqlWorkbenchConnection connection, DBObject dbo, List<string> columnsList)
    {
      return GetDataFromTableOrView(connection, dbo, columnsList, -1, -1);
    }

    public static MySqlDataAdapter GetDataAdapterFromTable(MySqlWorkbenchConnection connection, string query)
    {
      MySqlDataAdapter retAdapter = new MySqlDataAdapter(query, GetConnectionString(connection));
      MySqlCommandBuilder commandBuilder = new MySqlCommandBuilder(retAdapter);
      retAdapter.UpdateCommand = commandBuilder.GetUpdateCommand();
      return retAdapter;
    }

    public static MySqlDataAdapter GetDataAdapterFromTable(MySqlWorkbenchConnection connection, DBObject dbo, List<string> columnsList, int firstRowIdx, int rowCount)
    {
      if (dbo.Type != DBObjectType.Table)
        return null;

      string queryString = assembleSelectQuery(dbo, columnsList, firstRowIdx, rowCount);
      return GetDataAdapterFromTable(connection, queryString);
    }

    public static MySqlDataAdapter GetDataAdapterFromTable(MySqlWorkbenchConnection connection, DBObject dbo, List<string> columnsList)
    {
      return GetDataAdapterFromTable(connection, dbo, columnsList, -1, -1);
    }

    public static DataSet GetDataSetFromRoutine(MySqlWorkbenchConnection connection, DBObject dbo, params MySqlParameter[] parameters)
    {
      DataSet retDS = null;
      
      if (dbo.Type == DBObjectType.Routine)
      {
        string sql = String.Format("`{0}`", dbo.Name);
        retDS = ExecuteDatasetSP(GetConnectionString(connection), sql, parameters);
      }

      return retDS;
    }

    public static DataSet ExecuteDatasetSP(MySqlConnection connection, string commandText, params MySqlParameter[] commandParameters)
    {
      //create a command and prepare it for execution
      MySqlCommand cmd = new MySqlCommand();
      cmd.Connection = connection;
      cmd.CommandText = commandText;
      cmd.CommandType = CommandType.StoredProcedure;

      if (commandParameters != null)
        foreach (MySqlParameter p in commandParameters)
          cmd.Parameters.Add(p);

      // Create the DataAdapter & DataSet
      MySqlDataAdapter da = new MySqlDataAdapter(cmd);
      DataSet ds = new DataSet();

      // Fill the DataSet using default values for DataTable names, etc.
      da.Fill(ds);

      // Detach the MySqlParameters from the command object, so they can be used again.			
      cmd.Parameters.Clear();

      // Return the dataset
      return ds;
    }

    public static DataSet ExecuteDatasetSP(string connectionString, string commandText, params MySqlParameter[] commandParameters)
    {
      // Create & open a SqlConnection, and dispose of it after we are done.
      using (MySqlConnection cn = new MySqlConnection(connectionString))
      {
        cn.Open();

        // Call the overload that takes a connection in place of the connection string
        return ExecuteDatasetSP(cn, commandText, commandParameters);
      }
    }

    public static List<string> GetDataTypes()
    {
      List<string> retList = new List<string>();
      retList.AddRange(new string[] {
            "bit",
            "tinyint",
            "boolean",
            "smallint",
            "mediumint",
            "int",
            "serial",
            "float",
            "double",
            "decimal",
            "date",
            "datetime",
            "timestamp",
            "time",
            "year",
            "char",
            "varchar",
            "binary",
            "varbinary",
            "tinyblob",
            "tinytext",
            "blob",
            "text",
            "mediumblob",
            "mediumtext",
            "longblob",
            "longtext",
            "enum(x,y,z)",
            "set(x,y,z)"});
      return retList;
    }

    public static MySqlDbType NameToType(string typeName, bool unsigned, bool realAsFloat)
    {
      switch (typeName.ToUpper(CultureInfo.InvariantCulture))
      {
        case "CHAR":
          return MySqlDbType.String;
        case "VARCHAR":
          return MySqlDbType.VarChar;
        case "DATE":
          return MySqlDbType.Date;
        case "DATETIME":
          return MySqlDbType.DateTime;
        case "NUMERIC":
        case "DECIMAL":
        case "DEC":
        case "FIXED":
          //if (connection.driver.Version.isAtLeast(5, 0, 3))
          //  return MySqlDbType.NewDecimal;
          //else
            return MySqlDbType.Decimal;
        case "YEAR":
          return MySqlDbType.Year;
        case "TIME":
          return MySqlDbType.Time;
        case "TIMESTAMP":
          return MySqlDbType.Timestamp;
        case "SET":
          return MySqlDbType.Set;
        case "ENUM":
          return MySqlDbType.Enum;
        case "BIT":
          return MySqlDbType.Bit;

        case "TINYINT":
          return unsigned ? MySqlDbType.UByte : MySqlDbType.Byte;
        case "BOOL":
        case "BOOLEAN":
          return MySqlDbType.Byte;
        case "SMALLINT":
          return unsigned ? MySqlDbType.UInt16 : MySqlDbType.Int16;
        case "MEDIUMINT":
          return unsigned ? MySqlDbType.UInt24 : MySqlDbType.Int24;
        case "INT":
        case "INTEGER":
          return unsigned ? MySqlDbType.UInt32 : MySqlDbType.Int32;
        case "SERIAL":
          return MySqlDbType.UInt64;
        case "BIGINT":
          return unsigned ? MySqlDbType.UInt64 : MySqlDbType.Int64;
        case "FLOAT":
          return MySqlDbType.Float;
        case "DOUBLE":
          return MySqlDbType.Double;
        case "REAL":
          return realAsFloat ? MySqlDbType.Float : MySqlDbType.Double;
        case "TEXT":
          return MySqlDbType.Text;
        case "BLOB":
          return MySqlDbType.Blob;
        case "LONGBLOB":
          return MySqlDbType.LongBlob;
        case "LONGTEXT":
          return MySqlDbType.LongText;
        case "MEDIUMBLOB":
          return MySqlDbType.MediumBlob;
        case "MEDIUMTEXT":
          return MySqlDbType.MediumText;
        case "TINYBLOB":
          return MySqlDbType.TinyBlob;
        case "TINYTEXT":
          return MySqlDbType.TinyText;
        case "BINARY":
          return MySqlDbType.Binary;
        case "VARBINARY":
          return MySqlDbType.VarBinary;
      }
      throw new Exception("Unhandled type encountered");
    }

    public static string GetMySQLExportDataType(object packedValue, out bool valueOverflow)
    {
      valueOverflow = false;
      if (packedValue == null)
        return String.Empty;

      Type objUnpackedType = packedValue.GetType();
      string strType = objUnpackedType.FullName;
      string strValue = packedValue.ToString();
      int strLength = strValue.Length;
      int decimalPointPos = strValue.IndexOf("."); ;
      int[] varCharApproxLen = new int[7] {5,12,25,45,255,4000,65535};
      int[,] decimalApproxLen = new int[2,2] { {12,2}, {65,30} };
      int intResult = 0;
      long longResult = 0;
      int intLen = 0;
      int fractLen = 0;

      if (strType == "System.Double")
        if (decimalPointPos < 0)
        {
          if (Int32.TryParse(strValue, out intResult))
            strType = (intResult == 0 || intResult == 1 ? "System.Boolean" : "System.Int32");
          else if (Int64.TryParse(strValue, out longResult))
            strType = "System.Int64";
        }
        else
          strType = "System.Decimal";
      if (strType.StartsWith("Varchar") && (strValue == "Yes" || strValue == "No" || strValue == "Ja" || strValue == "Nein"))
        strType = "System.Boolean";

      switch (strType)
      {
        case "System.String":
          for (int i = 0; i < varCharApproxLen.Length; i++)
          {
            if (strLength <= varCharApproxLen[i])
              return String.Format("Varchar({0})", varCharApproxLen[i]);
          }
          valueOverflow = true;
          return "Varchar(65535)";
        case "System.Double":
          return "Double";
        case "System.Decimal":
        case "System.Single":
          intLen = decimalPointPos;
          fractLen = strLength - intLen - 1;
          if (intLen <= decimalApproxLen[0, 0] && fractLen <= decimalApproxLen[0, 1])
            return "Decimal(12,2)";
          else if (intLen <= decimalApproxLen[1, 0] && fractLen <= decimalApproxLen[1, 1])
            return "Decimal(65,30)";
          valueOverflow = true;
          return "Double";
        case "System.Byte":
        case "System.UInt16":
        case "System.Int16":
        case "System.UInt32":
        case "System.Int32":
          return "Integer";
        case "System.UInt64":
        case "System.Int64":
          return "BigInt";
        case "System.Boolean":
          return "Bool";
        case "System.DateTime":
          if (strValue.Contains(":"))
            return "Datetime";
          return "Date";
        case "System.TimeSpan":
          return "Time";
      }
      return String.Empty;
    }

    public static string GetMySQLDataType(object packedValue)
    {
      string retType = String.Empty;
      if (packedValue == null)
        return retType;

      Type objUnpackedType = packedValue.GetType();
      string strType = objUnpackedType.FullName;
      int strLength = packedValue.ToString().Length;
      strLength = strLength + (10 - strLength % 10);
      bool unsigned = strType.Contains(".U");

      switch (strType)
      {
        case "System.String":
          if (strLength > 65535)
            retType = "text";
          else
            retType = "varchar";
          break;
        case "System.Byte":
          retType = "tinyint";
          break;
        case "System.UInt16":
        case "System.Int16":
          retType = String.Format("smallint{0}", (unsigned ? " unsigned" : String.Empty));
          break;
        case "System.UInt32":
        case "System.Int32":
          retType = String.Format("int{0}", (unsigned ? " unsigned" : String.Empty));
          break;
        case "System.UInt64":
        case "System.Int64":
          retType = String.Format("bigint{0}", (unsigned ? " unsigned" : String.Empty));
          break;
        case "System.Decimal":
          retType = "decimal";
          break;
        case "System.Single":
          retType = "float";
          break;
        case "System.Double":
          retType = "double";
          break;
        case "System.Boolean":
          retType = "bit";
          break;
        case "System.DateTime":
          retType = "datetime";
          break;
        case "System.TimeSpan":
          retType = "time";
          break;
        case "System.Guid":
          retType = "binary(16)";
          break;
      }

      return retType;
    }

    public static bool TableExistsInSchema(MySqlWorkbenchConnection connection, string schemaName, string tableName)
    {
      if (String.IsNullOrEmpty(schemaName) || String.IsNullOrEmpty(tableName))
        return false;

      string sql = String.Format("SELECT COUNT(*) FROM information_schema.tables WHERE table_schema = '{0}' and table_name = '{1}'", schemaName, tableName);
      object objCount = MySqlHelper.ExecuteScalar(GetConnectionString(connection), sql);
      long retCount = (objCount != null ? (long)objCount : 0);
      return (retCount > 0);
    }

    public static bool IndexExistsInSchema(MySqlWorkbenchConnection connection, string schemaName, string tableName, string indexName)
    {
      if (String.IsNullOrEmpty(schemaName) || String.IsNullOrEmpty(indexName))
        return false;

      DataTable dt = GetSchemaCollection(connection, "Indexes", null, schemaName, tableName, indexName);
      return dt.Rows.Count > 0;
    }

    public static void SetDoubleBuffered(System.Windows.Forms.Control c)
    {
      if (SystemInformation.TerminalServerSession)
        return;

      PropertyInfo aProp =
            typeof(System.Windows.Forms.Control).GetProperty(
                  "DoubleBuffered",
                  System.Reflection.BindingFlags.NonPublic |
                  System.Reflection.BindingFlags.Instance);

      aProp.SetValue(c, true, null);
    }

    public static Bitmap MakeGrayscale(Bitmap original)
    {
      // Create a blank bitmap the same size as original
      Bitmap newBitmap = new Bitmap(original.Width, original.Height);

      // Get a graphics object from the new image
      Graphics g = Graphics.FromImage(newBitmap);

      // Create the grayscale ColorMatrix
      ColorMatrix colorMatrix = new ColorMatrix(
         new float[][] 
      {
         new float[] {.3f, .3f, .3f, 0, 0},
         new float[] {.59f, .59f, .59f, 0, 0},
         new float[] {.11f, .11f, .11f, 0, 0},
         new float[] {0, 0, 0, 1, 0},
         new float[] {0, 0, 0, 0, 1}
      });

      // Create some image attributes
      ImageAttributes attributes = new ImageAttributes();

      // Set the color matrix attribute
      attributes.SetColorMatrix(colorMatrix);

      // Draw the original image on the new image using the grayscale color matrix
      g.DrawImage(original, new Rectangle(0, 0, original.Width, original.Height),
         0, 0, original.Width, original.Height, GraphicsUnit.Pixel, attributes);

      // Dispose the Graphics object
      g.Dispose();
      return newBitmap;
    }

    [DllImport("user32.dll")]
    public static extern IntPtr CreateIconIndirect(ref IconInfo icon);

    [DllImport("user32.dll")]
    [return: MarshalAs(UnmanagedType.Bool)]
    public static extern bool GetIconInfo(IntPtr hIcon, ref IconInfo pIconInfo);

    public static Cursor CreateCursor(Bitmap bmp, int xHotSpot, int yHotSpot)
    {
      IconInfo tmp = new IconInfo();
      GetIconInfo(bmp.GetHicon(), ref tmp);
      tmp.xHotspot = xHotSpot;
      tmp.yHotspot = yHotSpot;
      tmp.fIcon = false;
      return new Cursor(CreateIconIndirect(ref tmp));
    }

    public static DialogResult ShowWarningBox(string warningMessage)
    {
      return MessageBox.Show(warningMessage, "Error", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
    }

    public static void ShowErrorBox(string errorMessage)
    {
      MessageBox.Show(errorMessage, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
    }

    public static void ShowExceptionBox(Exception ex)
    {
      ShowErrorBox(ex.Message);
    }

    public static List<string> DataRowsToList(DataRowCollection rowCollection, string colName)
    {
      List<string> retList = null;

      if (rowCollection != null && rowCollection.Count > 0)
      {
        foreach (DataRow dr in rowCollection)
        {
          retList.Add(dr[colName].ToString());
        }
      }

      return retList;
    }

    public static List<string> DataRowsToList(DataRow[] dataRows, string colName)
    {
      List<string> retList = null;

      if (dataRows != null && dataRows.Length > 0)
      {
        foreach (DataRow dr in dataRows)
        {
          retList.Add(dr[colName].ToString());
        }
      }

      return retList;
    }
  }

  public struct IconInfo
  {
    public bool fIcon;
    public int xHotspot;
    public int yHotspot;
    public IntPtr hbmMask;
    public IntPtr hbmColor;
  }
}
