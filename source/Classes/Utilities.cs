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
  public static class MiscUtilities
  {
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

    public static int IndexOfStringInArray(string[] stringArray, string stringElement, bool caseSensitive)
    {
      int index = -1;
      if (!caseSensitive)
        stringElement = stringElement.ToLowerInvariant();

      if (stringArray != null)
        for (int i = 0; i < stringArray.Length; i++)
        {
          bool areEqual = stringElement == (caseSensitive ? stringArray[i] : stringArray[i].ToLowerInvariant());
          if (areEqual)
          {
            index = i;
            break;
          }
        }
      return index;
    }

    public static int IndexOfIntInArray(int[] intArray, int intElement)
    {
      int index = -1;

      if (intArray != null)
        for (int i = 0; i < intArray.Length; i++)
          if (intArray[i] == intElement)
          {
            index = i;
            break;
          }

      return index;
    }

    public static bool SaveSettings()
    {
      try
      {
        for (int i = 0; i < 3; i++)
        {
          Properties.Settings.Default.Save();
          break;
        }
      }
      catch (Exception ex)
      {
        InfoDialog infoDialog = new InfoDialog(false, "An error ocurred when savings user settings file", ex.Message);
        infoDialog.ShowDialog();
        return false;
      }
      return true;
    }

  }

  public static class MySQLDataUtilities
  {
    public static string GetConnectionString(MySqlWorkbenchConnection connection)
    {
      MySqlConnectionStringBuilder cs = new MySqlConnectionStringBuilder();
      cs.Server = connection.Host;
      cs.UserID = connection.UserName;
      cs.Password = connection.Password;
      cs.Database = connection.Schema;
      cs.Port = (uint)connection.Port;
      cs.AllowZeroDateTime = true;
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
            case "COLUMNS SHORT":
              mysqlAdapter = new MySqlDataAdapter(String.Format("SHOW COLUMNS FROM `{0}`.`{1}`", restrictions[1], restrictions[2]), conn);
              dt = new DataTable();
              mysqlAdapter.Fill(dt);
              break;
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

    public static long GetRowsCountFromTableOrView(MySqlWorkbenchConnection connection, DBObject dbo)
    {
      if (dbo.Type == DBObjectType.Routine)
        return 0;

      string sql = String.Format("SELECT COUNT(*) FROM `{0}`.`{1}`", connection.Schema, dbo.Name);
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

    public static void AddExtendedProperties(ref MySQLDataTable dt, string queryString, bool importedHeaders, string tableName)
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

    private static string assembleSelectQuery(string schemaName, DBObject dbo, List<string> columnsList, int firstRowIdx, int rowCount)
    {
      StringBuilder queryStringBuilder = new StringBuilder("SELECT ");
      if (columnsList == null || columnsList.Count == 0)
        queryStringBuilder.Append("*");
      else
      {
        foreach (string columnName in columnsList)
        {
          queryStringBuilder.AppendFormat("`{0}`,", columnName.Replace("`", "``"));
        }
        queryStringBuilder.Remove(queryStringBuilder.Length - 1, 1);
      }
      queryStringBuilder.AppendFormat(" FROM `{0}`.`{1}`", schemaName, dbo.Name);
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

      string queryString = assembleSelectQuery(connection.Schema, dbo, columnsList, firstRowIdx, rowCount);
      return GetDataFromTableOrView(connection, queryString);
    }

    public static DataTable GetDataFromTableOrView(MySqlWorkbenchConnection connection, DBObject dbo, List<string> columnsList)
    {
      return GetDataFromTableOrView(connection, dbo, columnsList, -1, -1);
    }

    public static DataSet GetDataSetFromRoutine(MySqlWorkbenchConnection connection, DBObject dbo, params MySqlParameter[] parameters)
    {
      DataSet retDS = null;

      if (dbo.Type == DBObjectType.Routine)
      {
        string sql = String.Format("`{0}`.`{1}`", connection.Schema, dbo.Name);
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

    public static bool TableHasPrimaryKey(MySqlWorkbenchConnection connection, string tableName)
    {
      if (String.IsNullOrEmpty(tableName))
        return false;

      string sql = String.Format("SHOW KEYS FROM `{0}` IN `{1}` WHERE Key_name = 'PRIMARY';", tableName, connection.Schema);
      DataTable dt = GetDataFromTableOrView(connection, sql);
      return (dt != null ? dt.Rows.Count > 0 : false);
    }

    public static bool TableExistsInSchema(MySqlWorkbenchConnection connection, string schemaName, string tableName)
    {
      if (String.IsNullOrEmpty(schemaName) || String.IsNullOrEmpty(tableName))
        return false;

      string sql = String.Format("SELECT COUNT(*) FROM information_schema.tables WHERE table_schema = '{0}' and table_name = '{1}'", schemaName, MySQLDataUtilities.EscapeString(tableName));
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

    public static string EscapeString(string valueToScape)
    {
      const string quotesAndOtherDangerousChars =
          "\\" + "\u2216" + "\uFF3C"               // backslashes
        + "'" + "\u00B4" + "\u02B9" + "\u02BC" + "\u02C8" + "\u02CA"
                +  "\u0301" + "\u2019" + "\u201A" + "\u2032"
                + "\u275C" + "\uFF07"            // single-quotes
        + "`" + "\u02CB" + "\u0300" + "\u2018" + "\u2035" + "\u275B"
                + "\uFF40"                       // back-tick
        + "\"" + "\u02BA" + "\u030E" + "\uFF02"; // double-quotes
      
      StringBuilder sb = new StringBuilder();
      foreach (char c in valueToScape)
      {
        char escape = char.MinValue;
        switch (c)
        {
          case '\u0000':          
            escape = '0';
            break;
          case '\n':              
            escape = 'n';
            break;
          case '\r':
            escape = 'r';
            break;
          case '\u001F':          
            escape = 'Z';
            break;
          default:
            if (quotesAndOtherDangerousChars.IndexOf(c) >= 0)
              escape = c;
            break;
        }
        if (escape != char.MinValue)
        {
          sb.Append('\\');
          sb.Append(escape);
        }
        else
        {
          sb.Append(c);
        }
      }
      return sb.ToString();
    }
  }

  public static class DataTypeUtilities
  {
    public const int VARCHAR_MAX_LEN = 65535;

    public static List<string> GetMySQLDataTypes(out List<int> paramsInParenthesisList)
    {
      List<string> retList = new List<string>();
      retList.AddRange(new string[] {
            "bit",
            "tinyint",
            "smallint",
            "mediumint",
            "int",
            "integer",
            "bigint",
            "float",
            "double",
            "decimal",
            "numeric",
            "real",
            "bool",
            "boolean",
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
            "enum",
            "set"});
      paramsInParenthesisList = new List<int>(retList.Count);
      paramsInParenthesisList.AddRange(new int[] { 1, 1, 1, 1, 1, 1, 1, 2, 2, 2, 2, 2, 0, 0, 0, 0, 0, 0, 0, 1, 1, 1, 1, 0, 0, 0, 0, 0, 0, 0, 0, -1, -1 });
      return retList;
    }

    public static List<string> GetMySQLDataTypes()
    {
      List<int> unused;
      return GetMySQLDataTypes(out unused);
    }

    public static bool Type1FitsIntoType2(string strippedType1, string strippedType2)
    {
      if (String.IsNullOrEmpty(strippedType1) || String.IsNullOrEmpty(strippedType2))
        return false;
      strippedType1 = strippedType1.ToLowerInvariant();
      strippedType2 = strippedType2.ToLowerInvariant();
      List<string> dataTypesList = GetMySQLDataTypes();
      if (!dataTypesList.Contains(strippedType1) || !dataTypesList.Contains(strippedType2))
      {
        System.Diagnostics.Debug.WriteLine("Type1FitsIntoType2: One of the 2 types is Invalid.");
        return false;
      }
      if (strippedType2 == strippedType1)
        return true;
      bool type2IsChar = strippedType2.Contains("char");
      bool type2IsText = strippedType2.Contains("text");
      if (type2IsChar || type2IsText)
        return true;
      bool type1IsChar = strippedType1.Contains("char");

      bool type1IsInt = strippedType1.Contains("int");
      bool type2IsInt = strippedType2.Contains("int");
      bool type1IsDecimal = strippedType1 == "float" || strippedType1 == "numeric" || strippedType1 == "decimal" || strippedType1 == "real" || strippedType1 == "double";
      bool type2IsDecimal = strippedType2 == "float" || strippedType2 == "numeric" || strippedType2 == "decimal" || strippedType2 == "real" || strippedType2 == "double";
      if ((type1IsInt || strippedType1 == "year") && (type2IsInt || type2IsDecimal || strippedType2 == "year"))
        return true;
      if (type1IsDecimal && type2IsDecimal)
        return true;

      if ((strippedType1.Contains("bool") || strippedType1 == "tinyint" || strippedType1 == "bit") && (strippedType2.Contains("bool") || strippedType2 == "tinyint" || strippedType2 == "bit"))
        return true;

      bool type1IsDate = strippedType1.Contains("date") || strippedType1 == "timestamp";
      bool type2IsDate = strippedType2.Contains("date") || strippedType2 == "timestamp";
      if (type1IsDate && type2IsDate)
        return true;

      if (strippedType1 == "time" && strippedType2 == "time")
        return true;
      if (strippedType1.Contains("blob") && strippedType2.Contains("blob"))
        return true;
      if (strippedType1.Contains("binary") && strippedType2.Contains("binary"))
        return true;
      return false;
    }

    public static bool StringValueCanBeStoredWithMySQLType(string strValue, string mySQLDataType)
    {
      mySQLDataType = mySQLDataType.ToLowerInvariant();
      bool isCharacter = mySQLDataType.StartsWith("varchar") || mySQLDataType.StartsWith("char") || mySQLDataType.Contains("text");
      bool isEnum = mySQLDataType.StartsWith("enum");
      bool isSet = mySQLDataType.StartsWith("set");
      bool mayContainFloatingPoint = mySQLDataType.StartsWith("decimal") || mySQLDataType.StartsWith("numeric") || mySQLDataType.StartsWith("double") || mySQLDataType.StartsWith("float") || mySQLDataType.StartsWith("real");
      int lParensIndex = mySQLDataType.IndexOf("(");
      int rParensIndex = mySQLDataType.IndexOf(")");
      int commaPos = mySQLDataType.IndexOf(",");
      int characterLen = (isCharacter ? (lParensIndex >= 0 ? Int32.Parse(mySQLDataType.Substring(lParensIndex + 1, mySQLDataType.Length - lParensIndex - 2)) : 1) : 0);
      int[] decimalLen = new int[2] { -1, -1 };
      List<string> setOrEnumMembers = null;
      if (mayContainFloatingPoint && lParensIndex >= 0 && rParensIndex >= 0 && lParensIndex < rParensIndex)
      {
        decimalLen[0] = Int32.Parse(mySQLDataType.Substring(lParensIndex + 1, (commaPos >= 0 ? commaPos : rParensIndex) - lParensIndex - 1));
        if (commaPos >= 0)
          decimalLen[1] = Int32.Parse(mySQLDataType.Substring(commaPos + 1, rParensIndex - commaPos - 1));
      }
      if ((isSet || isEnum) && lParensIndex >= 0 && rParensIndex >= 0 && lParensIndex < rParensIndex)
      {
        setOrEnumMembers = new List<string>();
        string membersString = mySQLDataType.Substring(lParensIndex + 1, rParensIndex - lParensIndex - 1);
        string[] setMembersArray = membersString.Split(new char[] { ',' });
        foreach (string s in setMembersArray)
          setOrEnumMembers.Add(s.Trim(new char[] { '"', '\'' }));
      }
      ulong tryBitValue = 0;
      byte tryByteValue = 0;
      int tryIntValue = 0;
      short trySmallIntValue = 0;
      long tryBigIntValue = 0;
      decimal tryDecimalValue = 0;
      double tryDoubleValue = 0;
      float tryFloatValue = 0;
      DateTime tryDateTimeValue = DateTime.Now;
      TimeSpan tryTimeSpanValue = TimeSpan.Zero;

      int floatingPointPos = strValue.IndexOf(".");
      bool floatingPointCompliant = true;
      if (floatingPointPos >= 0)
      {
        bool lengthCompliant = strValue.Substring(0, floatingPointPos).Length <= decimalLen[0];
        bool decimalPlacesCompliant = (decimalLen[1] >= 0 ? strValue.Substring(floatingPointPos + 1, strValue.Length - floatingPointPos - 1).Length <= decimalLen[1] : true);
        floatingPointCompliant = lengthCompliant && decimalPlacesCompliant;
      }

      if (isCharacter)
        return strValue.Length <= characterLen;
      if (mySQLDataType.StartsWith("decimal") || mySQLDataType.StartsWith("numeric"))
        return Decimal.TryParse(strValue, out tryDecimalValue) && floatingPointCompliant;
      if (mySQLDataType.StartsWith("int") || mySQLDataType.StartsWith("mediumint") || mySQLDataType == "year")
        return Int32.TryParse(strValue, out tryIntValue);
      if (mySQLDataType.StartsWith("tinyint"))
        return Byte.TryParse(strValue, out tryByteValue);
      if (mySQLDataType.StartsWith("smallint"))
        return Int16.TryParse(strValue, out trySmallIntValue);
      if (mySQLDataType.StartsWith("bigint"))
        return Int64.TryParse(strValue, out tryBigIntValue);
      if (mySQLDataType.StartsWith("bool") || mySQLDataType == "bit" || mySQLDataType == "bit(1)")
      {
        strValue = strValue.ToLowerInvariant();
        return (strValue == "true" || strValue == "false" || strValue == "0" || strValue == "1" || strValue == "yes" || strValue == "no" || strValue == "ja" || strValue == "nein");
      }
      if (mySQLDataType.StartsWith("bit"))
        return UInt64.TryParse(strValue, out tryBitValue);
      if (mySQLDataType.StartsWith("float"))
        return Single.TryParse(strValue, out tryFloatValue);
      if (mySQLDataType.StartsWith("double") || mySQLDataType.StartsWith("real"))
        return Double.TryParse(strValue, out tryDoubleValue);
      if (mySQLDataType == "date" || mySQLDataType == "datetime" || mySQLDataType == "timestamp")
        return DateTime.TryParse(strValue, out tryDateTimeValue);
      if (mySQLDataType == "time")
        return TimeSpan.TryParse(strValue, out tryTimeSpanValue);
      if (mySQLDataType == "blob" || mySQLDataType == "tinyblob" || mySQLDataType == "mediumblob" || mySQLDataType == "longblob" || mySQLDataType == "binary" || mySQLDataType == "varbinary")
        return true;
      if (isEnum)
        return setOrEnumMembers.Contains(strValue.ToLowerInvariant());
      if (isSet)
      {
        string[] valueSet = strValue.Split(new char[] { ',' });
        bool setMatch = valueSet.Length > 0;
        foreach (string val in valueSet)
          setMatch = setMatch && setOrEnumMembers.Contains(val.ToLowerInvariant());
        return setMatch;
      }
      return false;
    }

    public static Type NameToType(string typeName, bool unsigned)
    {
      string upperType = typeName.ToUpper(CultureInfo.InvariantCulture);
      switch (upperType)
      {
        case "CHAR":
        case "VARCHAR":
        case "SET":
        case "ENUM":
        case "TEXT":
        case "MEDIUMTEXT":
        case "TINYTEXT":
        case "LONGTEXT":
          return Type.GetType("System.String");
        case "NUMERIC":
        case "DECIMAL":
        case "DEC":
        case "FIXED":
          return Type.GetType("System.Decimal");
        case "INT":
        case "INTEGER":
        case "MEDIUMINT":
        case "YEAR":
          return (!unsigned || upperType == "YEAR" ? Type.GetType("System.Int32") : Type.GetType("System.UInt32"));
        case "TINYINT":
          return Type.GetType("System.Byte");
        case "SMALLINT":
          return (!unsigned ? Type.GetType("System.Int16") : Type.GetType("System.UInt16"));
        case "BIGINT":
          return (!unsigned ? Type.GetType("System.Int64") : Type.GetType("System.UInt64"));
        case "BOOL":
        case "BOOLEAN":
        case "BIT(1)":
          return Type.GetType("System.Boolean");
        case "BIT":
        case "SERIAL":
          return Type.GetType("System.UInt64");
        case "FLOAT":
          return Type.GetType("System.Single");
        case "DOUBLE":
        case "REAL":
          return Type.GetType("System.Double");
        case "DATE":
        case "DATETIME":
        case "TIMESTAMP":
          return Type.GetType("System.DateTime");
        case "TIME":
          return Type.GetType("System.TimeSpan");
        case "BLOB":
        case "LONGBLOB":
        case "MEDIUMBLOB":
        case "TINYBLOB":
        case "BINARY":
        case "VARBINARY":
          return Type.GetType("System.Object");
      }
      throw new Exception("Unhandled type encountered");
    }

    public static MySqlDbType NameToMySQLType(string typeName, bool unsigned, bool realAsFloat)
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
      int[] varCharApproxLen = new int[7] { 5, 12, 25, 45, 255, 4000, VARCHAR_MAX_LEN };
      int[,] decimalApproxLen = new int[2, 2] { { 12, 2 }, { 65, 30 } };
      int intResult = 0;
      long longResult = 0;
      int intLen = 0;
      int fractLen = 0;

      if (strType == "System.Double")
        if (decimalPointPos < 0)
        {
          if (Int32.TryParse(strValue, out intResult))
            strType = "System.Int32";
          else if (Int64.TryParse(strValue, out longResult))
            strType = "System.Int64";
        }
        else
          strType = "System.Decimal";
      strValue = strValue.ToLowerInvariant();
      if (strType == "System.String" && (strValue == "yes" || strValue == "no" || strValue == "ja" || strValue == "nein"))
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
          return String.Format("Varchar({0})", VARCHAR_MAX_LEN);
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
          if (strLength > VARCHAR_MAX_LEN)
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

    public static string GetConsistentDataTypeOnAllRows(string proposedStrippedDataType, List<string> rowsDataTypesList, int[] decimalMaxLen, int[] varCharMaxLen, out string consistentStrippedDataType)
    {
      string fullDataType = proposedStrippedDataType;
      bool typesConsistent = true;

      typesConsistent = rowsDataTypesList.All(str => str == proposedStrippedDataType);
      if (!typesConsistent)
      {
        if (rowsDataTypesList.Count(str => str == "Integer") + rowsDataTypesList.Count(str => str == "Bool") == rowsDataTypesList.Count)
        {
          typesConsistent = true;
          fullDataType = "Integer";
        }
        else if (rowsDataTypesList.Count(str => str == "Integer") + rowsDataTypesList.Count(str => str == "BigInt") == rowsDataTypesList.Count)
        {
          typesConsistent = true;
          fullDataType = "BigInt";
        }
        else if (rowsDataTypesList.Count(str => str == "Integer") + rowsDataTypesList.Count(str => str == "Decimal") == rowsDataTypesList.Count)
        {
          typesConsistent = true;
          proposedStrippedDataType = "Decimal";
        }
        else if (rowsDataTypesList.Count(str => str == "Integer") + rowsDataTypesList.Count(str => str == "Decimal") + rowsDataTypesList.Count(str => str == "Double") == rowsDataTypesList.Count)
        {
          typesConsistent = true;
          fullDataType = "Double";
        }
      }

      if (typesConsistent)
        switch (proposedStrippedDataType)
        {
          case "Varchar":
            consistentStrippedDataType = proposedStrippedDataType;
            fullDataType = String.Format("Varchar({0})", varCharMaxLen[0]);
            break;
          case "Decimal":
            consistentStrippedDataType = proposedStrippedDataType;
            if (decimalMaxLen[0] > 12 || decimalMaxLen[1] > 2)
            {
              decimalMaxLen[0] = 65;
              decimalMaxLen[1] = 30;
            }
            else
            {
              decimalMaxLen[0] = 12;
              decimalMaxLen[1] = 2;
            }
            fullDataType = String.Format("Decimal({0}, {1})", decimalMaxLen[0], decimalMaxLen[1]);
            break;
          default:
            consistentStrippedDataType = fullDataType;
            break;
        }
      else
      {
        consistentStrippedDataType = "Varchar";
        fullDataType = String.Format("Varchar({0})", varCharMaxLen[1]);
      }

      return fullDataType;
    }

    public static string GetConsistentDataTypeOnAllRows(string proposedStrippedDataType, List<string> rowsDataTypesList, int[] decimalMaxLen, int[] varCharMaxLen)
    {
      string outConsistentStrippedType;
      return GetConsistentDataTypeOnAllRows(proposedStrippedDataType, rowsDataTypesList, decimalMaxLen, varCharMaxLen, out outConsistentStrippedType);
    }

    public static object GetInsertingValueForColumnType(object rawValue, MySQLDataColumn column)
    {
      object retValue = rawValue;
      if (column == null)
        return rawValue;

      bool cellWithNoData = rawValue == null || rawValue == DBNull.Value;
      if (cellWithNoData)
      {
        if (column.AllowNull)
          retValue = DBNull.Value;
        else
        {
          if (column.IsNumeric || column.IsBinary)
            retValue = 0;
          else if (column.IsBool)
            retValue = false;
          else if (column.IsDate)
            retValue = "0000-00-00 00:00:00";
          else if (column.IsCharOrText)
            retValue = String.Empty;
        }
      }
      else
      {
        if (column.IsBool)
        {
          string rawValueAsString = rawValue.ToString().ToLowerInvariant();
          if (rawValueAsString == "ja" || rawValueAsString == "yes" || rawValueAsString == "true" || rawValueAsString == "1")
            retValue = true;
          else if (rawValueAsString == "nein" || rawValueAsString == "no" || rawValueAsString == "false" || rawValueAsString == "0")
            retValue = false;
          else
            retValue = rawValue;
        }
        else if (column.IsCharOrText)
          retValue = MySQLDataUtilities.EscapeString(rawValue.ToString());
        else
          retValue = rawValue;
      }

      return retValue;
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
