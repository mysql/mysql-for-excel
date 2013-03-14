// 
// Copyright (c) 2012-2013, Oracle and/or its affiliates. All rights reserved.
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
using System.Drawing;
using System.Drawing.Imaging;
using System.Windows.Forms;
using System.Data;
using MySql.Data.MySqlClient;
using MySQL.Utility;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Globalization;
using System.Diagnostics;


namespace MySQL.ForExcel
{
  public static class MiscUtilities
  {
    private static string logFile;

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
        InfoDialog errorDialog = new InfoDialog(false, Properties.Resources.SettingsFileSaveErrorTitle, ex.Message);
        errorDialog.WordWrapDetails = true;
        errorDialog.ShowDialog();
        MiscUtilities.WriteAppErrorToLog(ex);
        return false;
      }
      return true;
    }

    public static void WriteToLog(string message, SourceLevels messageType = SourceLevels.Error, int errorLevel = 1)
    {
      if (String.IsNullOrEmpty(logFile))
        logFile = String.Format(Properties.Resources.LogFile, Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData));
      MySQLSourceTrace sourceTrace = new MySQLSourceTrace("MySQLForExcel", logFile, String.Empty, messageType);
      switch (messageType)
      {
        case SourceLevels.Error:
          sourceTrace.WriteError(message, errorLevel);
          break;
        case SourceLevels.Information:
          sourceTrace.WriteInformation(message, errorLevel);
          break;
        case SourceLevels.Warning:
          sourceTrace.WriteWarning(message, errorLevel);
          break;
        case SourceLevels.Critical:
          sourceTrace.WriteCritical(message, errorLevel);
          break;
      }
    }

    public static void WriteAppErrorToLog(Exception ex)
    {
      MethodBase callingMethod = new StackFrame(1).GetMethod();
      WriteToLog(String.Format(Properties.Resources.ApplicationExceptionForLog,
                               callingMethod.DeclaringType.Name,
                               callingMethod.Name,
                               ex.Message,
                               ex.InnerException));
    }

    public static string TruncateString(string text, float maxWidth, Graphics graphics, Font font)
    {
      if (string.IsNullOrEmpty(text))
        return text;
      const string ellipsis = "...";
      string newText = text;
      float sizeText = graphics.MeasureString(newText, font).Width;
      if (sizeText > maxWidth)
      {
        int index = (int)((maxWidth / sizeText) * text.Length);
        newText = text.Substring(0, index);
        sizeText = graphics.MeasureString(newText + ellipsis, font).Width;
        if (sizeText < maxWidth)
        {
          while (sizeText < maxWidth)
          {
            newText = text.Substring(0, ++index);
            sizeText = graphics.MeasureString(newText + ellipsis, font).Width;
            if (sizeText > maxWidth)
            {
              newText = newText.Substring(0, newText.Length - 1);
              break;
            }
          }
        }
        else
        {
          while (sizeText > maxWidth)
          {
            newText = text.Substring(0, --index);
            sizeText = graphics.MeasureString(newText + ellipsis, font).Width;
          }
        }
        newText += ellipsis;
      }

      return newText;
    }

    /// <summary>
    /// Gets a text avoiding duplicates by adding a numeric suffix in case it already exists in the given list.
    /// </summary>
    /// <param name="listOfTexts">A list of texts.</param>
    /// <param name="proposedText">Proposed text.</param>
    /// <returns>Unique text.</returns>
    public static string GetNonDuplicateText(List<string> listOfTexts, string proposedText)
    {
      if (string.IsNullOrEmpty(proposedText) || listOfTexts == null || listOfTexts.Count == 0)
      {
        return proposedText;
      }

      proposedText = proposedText.Trim();
      string nonDuplicateText = proposedText;
      int textSuffixNumber = 2;
      while (listOfTexts.Exists(text => text == nonDuplicateText))
      {
        nonDuplicateText = proposedText + textSuffixNumber++;
      }

      return nonDuplicateText;
    }
  }

  public static class MySQLDataUtilities
  {
    public static string GetConnectionString(MySqlWorkbenchConnection connection, bool allowZeroDateTimeValues)
    {
      MySqlConnectionStringBuilder cs = new MySqlConnectionStringBuilder();
      cs.Server = connection.Host;
      cs.UserID = connection.UserName;
      cs.Password = connection.Password;
      cs.Database = connection.Schema;
      cs.Port = (uint)connection.Port;
      cs.PipeName = connection.Socket;
      cs.UseCompression = (connection.ClientCompress == 1) ? true : false;
      cs.AllowZeroDateTime = allowZeroDateTimeValues;
      cs.ConnectionProtocol = (connection.DriverType == MySqlWorkbenchConnectionType.Tcp) ? MySqlConnectionProtocol.Tcp : MySqlConnectionProtocol.NamedPipe;
      // force to populate IntegratedSecurity
      connection.TestConnection();
      cs.IntegratedSecurity = connection.IntegratedSecurity;
      
      //TODO:  use additional necessary options
      return cs.ConnectionString;
    }

    public static string GetConnectionString(MySqlWorkbenchConnection connection)
    {
      return GetConnectionString(connection, true);
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
        MiscUtilities.WriteAppErrorToLog(ex);
        throw;
      }

      return dt;
    }

    public static long GetRowsCountFromTableOrView(MySqlWorkbenchConnection connection, DBObject dbo)
    {
      if (dbo.Type == DBObjectType.Routine)
        return 0;

      string sql = String.Format("SELECT COUNT(*) FROM `{0}`.`{1}`", connection.Schema, dbo.Name);
      object objCount = MySqlHelper.ExecuteScalar(GetConnectionString(connection), sql);
      return (objCount != null ? (long)objCount : 0);
    }

    public static ulong GetMySQLServerMaxAllowedPacket(MySqlWorkbenchConnection connection)
    {
      string sql = "SELECT @@max_allowed_packet";
      object objCount = MySqlHelper.ExecuteScalar(GetConnectionString(connection), sql);
      return (objCount != null ? (ulong)objCount : 0);
    }

    public static ulong GetMySQLServerMaxAllowedPacket(MySqlConnection connection)
    {
      string sql = "SELECT @@max_allowed_packet";
      object objCount = MySqlHelper.ExecuteScalar(connection, sql);
      return (objCount != null ? (ulong)objCount : 0);
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
    public const string DATE_FORMAT = "yyyy-MM-dd HH:mm:ss";
    public const string EMPTY_DATE = "0000-00-00 00:00:00";

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
      if (String.IsNullOrEmpty(strippedType1))
        return true;
      if (String.IsNullOrEmpty(strippedType2))
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
      if (strippedType2.Contains("char") || strippedType2.Contains("text") || strippedType2.Contains("enum") || strippedType2.Contains("set"))
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
      int characterLen = 0;
      if (isCharacter)
      {
        if (lParensIndex >= 0)
        {
          string paramValue = mySQLDataType.Substring(lParensIndex + 1, mySQLDataType.Length - lParensIndex - 2);
          int.TryParse(paramValue, out characterLen);
        }
        else
        {
          characterLen = 1;
        }
      }

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
      {
        if (strValue.StartsWith("0000-00-00") || strValue.StartsWith("00-00-00"))
          return true;
        else
          return DateTime.TryParse(strValue, out tryDateTimeValue);
      }
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

    public static Type NameToType(string typeName, bool unsigned, bool datesAsMySQLDates)
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
          return (datesAsMySQLDates ? typeof(MySql.Data.Types.MySqlDateTime) : Type.GetType("System.DateTime"));
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

    /// <summary>
    /// Validates that a user typed data type is a valid MySQL data type.
    /// A blank data type is considered valid.
    /// </summary>
    /// <param name="dataType">A MySQL data type as specified for new columns in a CREATE TABLE statement.</param>
    /// <returns>true if the type is a valid MySQL data type, false otherwise.</returns>
    public static bool ValidateUserDataType(string proposedUserType)
    {
      //// If the proposed type is blank return true since a blank data type is considered valid.
      if (proposedUserType.Length == 0)
      {
        return true;
      }

      List<int> validParamsPerDataType;
      List<string> dataTypesList = GetMySQLDataTypes(out validParamsPerDataType);
      int rightParenthesisIndex = proposedUserType.IndexOf(")");
      int leftParenthesisIndex = proposedUserType.IndexOf("(");

      //// Check if we have parenthesis within the proposed data type and if the left and right parentheses are placed properly.
      //// Also check if there is no text beyond the right parenthesis.
      if (rightParenthesisIndex >= 0 && (leftParenthesisIndex < 0 || leftParenthesisIndex >= rightParenthesisIndex || proposedUserType.Length > rightParenthesisIndex + 1))
      {
        return false;
      }

      //// Check if the data type stripped of parenthesis is found in the list of valid MySQL types.
      string pureDataType = rightParenthesisIndex >= 0 ? proposedUserType.Substring(0, leftParenthesisIndex).ToLowerInvariant() : proposedUserType.ToLowerInvariant();
      int typeFoundAt = dataTypesList.IndexOf(pureDataType);
      if (typeFoundAt < 0)
      {
        return false;
      }

      //// Parameters checks.
      if (rightParenthesisIndex >= 0)
      {
        //// Check if the number of parameters is valid for the proposed MySQL data type
        int numOfValidParams = validParamsPerDataType[typeFoundAt];
        string[] parameterValues = proposedUserType.Substring(leftParenthesisIndex + 1, rightParenthesisIndex - leftParenthesisIndex - 1).Split(new Char[] { ',' });
        bool parametersQtyIsValid = false;
        if (pureDataType.StartsWith("var"))
        {
          parametersQtyIsValid = numOfValidParams >= 0 && numOfValidParams == parameterValues.Length;
        }
        else
        {
          parametersQtyIsValid = (numOfValidParams >= 0 && numOfValidParams == parameterValues.Length) || (numOfValidParams < 0 && parameterValues.Length > 0) || parameterValues.Length == 0;
        }

        if (!parametersQtyIsValid)
        {
          return false;
        }

        //// Check if the paremeter values are valid integers for data types with 1 or 2 parameters (varchar and numeric types).
        if (numOfValidParams >= 1 && numOfValidParams <= 2)
        {
          foreach (string paramValue in parameterValues)
          {
            int convertedValue = 0;
            if (!int.TryParse(paramValue, out convertedValue))
            {
              return false;
            }
          }
        }
      }

      return true;
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
      if (strType == "System.String")
      {
        if (strValue == "yes" || strValue == "no" || strValue == "ja" || strValue == "nein")
          strType = "System.Boolean";
        else if (strValue.StartsWith("0000-00-00") || strValue.StartsWith("00-00-00"))
          strType = "MySql.Data.Types.MySqlDateTime";
      }

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
        case "MySql.Data.Types.MySqlDateTime":
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
        case "MySql.Data.Types.MySqlDateTime":
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

      if (rowsDataTypesList.Count == 0)
      {
        consistentStrippedDataType = String.Empty;
        return String.Empty;
      }

      bool typesConsistent = rowsDataTypesList.All(str => str == proposedStrippedDataType);
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
        else if (rowsDataTypesList.Count(str => str == "Datetime") + rowsDataTypesList.Count(str => str == "Date") + rowsDataTypesList.Count(str => str == "Integer") == rowsDataTypesList.Count)
        {
          typesConsistent = true;
          fullDataType = "Datetime";
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

    public static object GetImportingValueForDateType(object rawValue)
    {
      object importingVaue = rawValue;

      if (rawValue != null && rawValue is MySql.Data.Types.MySqlDateTime)
      {
        MySql.Data.Types.MySqlDateTime mysqlDate = (MySql.Data.Types.MySqlDateTime)rawValue;
        if (mysqlDate.IsValidDateTime)
          importingVaue = new DateTime(mysqlDate.Year, mysqlDate.Month, mysqlDate.Day, mysqlDate.Hour, mysqlDate.Minute, mysqlDate.Second);
        else
          importingVaue = DateTime.MinValue;
      }

      return importingVaue;
    }

    public static object GetInsertingValueForColumnType(object rawValue, MySQLDataColumn againstTypeColumn)
    {
      object retValue = rawValue;
      if (againstTypeColumn == null)
        return rawValue;

      bool cellWithNoData = rawValue == null || rawValue == DBNull.Value;
      if (cellWithNoData)
      {
        if (againstTypeColumn.AllowNull)
          retValue = DBNull.Value;
        else
        {
          if (againstTypeColumn.IsNumeric || againstTypeColumn.IsBinary)
            retValue = 0;
          else if (againstTypeColumn.IsBool)
            retValue = false;
          else if (againstTypeColumn.IsDate)
          {
            if (againstTypeColumn.DataType.Name == "DateTime")
              retValue = DateTime.MinValue;
            else
              retValue = new MySql.Data.Types.MySqlDateTime(0, 0, 0, 0, 0, 0, 0);
          }
          else if (againstTypeColumn.ColumnsRequireQuotes)
            retValue = String.Empty;
        }
      }
      else
      {
        retValue = rawValue;
        if (againstTypeColumn.IsDate)
        {
          if (rawValue is DateTime)
          {
            DateTime dtValue = (DateTime)rawValue;
            if (againstTypeColumn.DataType.Name == "DateTime")
              retValue = dtValue;
            else
              retValue = new MySql.Data.Types.MySqlDateTime(dtValue);
          }
          else if (rawValue is MySql.Data.Types.MySqlDateTime)
          {
            MySql.Data.Types.MySqlDateTime dtValue = (MySql.Data.Types.MySqlDateTime)rawValue;
            if (againstTypeColumn.DataType.Name == "DateTime")
              retValue = (!dtValue.IsValidDateTime ? DateTime.MinValue : dtValue.GetDateTime());
            else
              retValue = dtValue;
          }
          else
          {
            DateTime dtValue;
            string rawValueAsString = rawValue.ToString();
            if (rawValueAsString.StartsWith("0000-00-00") || rawValueAsString.StartsWith("00-00-00") || rawValueAsString.Equals("0"))
            {
              if (againstTypeColumn.DataType.Name == "DateTime")
                retValue = DateTime.MinValue;
              else
                retValue = new MySql.Data.Types.MySqlDateTime(0, 0, 0, 0, 0, 0, 0);
            }
            else
            {
              if (DateTime.TryParse(rawValueAsString, out dtValue))
              {
                if (againstTypeColumn.DataType.Name == "DateTime")
                  retValue = dtValue;
                else
                  retValue = new MySql.Data.Types.MySqlDateTime(dtValue);
              }
              else
                retValue = rawValue;
            }
          }
        }
        else if (againstTypeColumn.IsBool)
        {
          string rawValueAsString = rawValue.ToString().ToLowerInvariant();
          if (rawValueAsString == "ja" || rawValueAsString == "yes" || rawValueAsString == "true" || rawValueAsString == "1")
            retValue = true;
          else if (rawValueAsString == "nein" || rawValueAsString == "no" || rawValueAsString == "false" || rawValueAsString == "0")
            retValue = false;
        }
        else if (againstTypeColumn.ColumnsRequireQuotes)
          retValue = MySQLDataUtilities.EscapeString(rawValue.ToString());  
      }

      return retValue;
    }

    public static string GetStringValueForColumn(object rawValue, MySQLDataColumn againstTypeColumn, bool dataForInsertion, out bool valueIsNull)
    {
      valueIsNull = true;
      string valueToDB = @"null";

      object valueObject = (dataForInsertion ? DataTypeUtilities.GetInsertingValueForColumnType(rawValue, againstTypeColumn) : rawValue);
      valueIsNull = valueObject == null || valueObject == DBNull.Value;
      if (!valueIsNull)
      {
        if (valueObject is DateTime)
        {
          DateTime dtValue = (DateTime)valueObject;
          if (dtValue.Equals(DateTime.MinValue))
          {
            valueIsNull = againstTypeColumn.AllowNull;
            valueToDB = (valueIsNull ? @"null" : EMPTY_DATE);
          }
          else
            valueToDB = dtValue.ToString(DATE_FORMAT);
        }
        else if (valueObject is MySql.Data.Types.MySqlDateTime)
        {
          MySql.Data.Types.MySqlDateTime dtValue = (MySql.Data.Types.MySqlDateTime)valueObject;
          if (!dtValue.IsValidDateTime || dtValue.GetDateTime().Equals(DateTime.MinValue))
          {
            valueIsNull = againstTypeColumn.AllowNull;
            valueToDB = (valueIsNull ? @"null" : EMPTY_DATE);
          }
          else
            valueToDB = dtValue.GetDateTime().ToString(DATE_FORMAT);
        }
        else
          valueToDB = GetStringRepresentationForNumericObject(valueObject);
      }

      return valueToDB;
    }

    public static string GetStringRepresentationForNumericObject(object boxedValue)
    {
      return GetStringRepresentationForNumericObject(boxedValue, CultureInfo.InvariantCulture);
    }

    public static string GetStringRepresentationForNumericObject(object boxedValue, CultureInfo ci)
    {
      if (boxedValue is sbyte)
      {
        return ((sbyte)boxedValue).ToString("G", CultureInfo.InvariantCulture);
      }
      else if (boxedValue is byte)
      {
        return ((byte)boxedValue).ToString("G", CultureInfo.InvariantCulture);
      }
      else if (boxedValue is short)
      {
        return ((short)boxedValue).ToString("G", CultureInfo.InvariantCulture);
      }
      else if (boxedValue is ushort)
      {
        return ((ushort)boxedValue).ToString("G", CultureInfo.InvariantCulture);
      }
      else if (boxedValue is int)
      {
        return ((int)boxedValue).ToString("G", CultureInfo.InvariantCulture);
      }
      else if (boxedValue is uint)
      {
        return ((uint)boxedValue).ToString("G", CultureInfo.InvariantCulture);
      }
      else if (boxedValue is long)
      {
        return ((long)boxedValue).ToString("G", CultureInfo.InvariantCulture);
      }
      else if (boxedValue is ulong)
      {
        return ((ulong)boxedValue).ToString("G", CultureInfo.InvariantCulture);
      }
      else if (boxedValue is float)
      {
        return ((float)boxedValue).ToString("G", CultureInfo.InvariantCulture);
      }
      else if (boxedValue is double)
      {
        return ((double)boxedValue).ToString("G", CultureInfo.InvariantCulture);
      }
      else if (boxedValue is decimal)
      {
        return ((decimal)boxedValue).ToString("G", CultureInfo.InvariantCulture);
      }

      return boxedValue.ToString();
    }

    public static string GetStringValueForColumn(object rawValue, MySQLDataColumn againstTypeColumn, bool dataForInsertion)
    {
      bool valueIsNull = false;
      return GetStringValueForColumn(rawValue, againstTypeColumn, dataForInsertion, out valueIsNull);
    }

    public static bool ExcelValueEqualsDataTableValue(object dataTableValue, object excelValue)
    {
      bool areEqual = dataTableValue.Equals(excelValue);

      if (!areEqual && dataTableValue != null)
      {
        string strExcelValue = excelValue.ToString();
        string strExcelValueIfBool = (excelValue.GetType().ToString() == "System.Boolean" ? ((bool)excelValue ? "1" : "0") : null);
        string nativeDataTableType = dataTableValue.GetType().ToString();
        switch(nativeDataTableType)
        {
          case "System.String":
            areEqual = String.Compare(dataTableValue.ToString(), strExcelValue, false) == 0;
            break;
          case "System.Byte":
            byte byteTableValue = (byte)dataTableValue;
            byte byteExcelValue = 0;
            if (strExcelValueIfBool != null)
              strExcelValue = strExcelValueIfBool;
            if (Byte.TryParse(strExcelValue, out byteExcelValue))
              areEqual = byteTableValue == byteExcelValue;
            break;
          case "System.UInt16":
            ushort ushortTableValue = (ushort)dataTableValue;
            ushort ushortExcelValue = 0;
            if (strExcelValueIfBool != null)
              strExcelValue = strExcelValueIfBool;
            if (UInt16.TryParse(strExcelValue, out ushortExcelValue))
              areEqual = ushortTableValue == ushortExcelValue;
            break;
          case "System.Int16":
            short shortTableValue = (short)dataTableValue;
            short shortExcelValue = 0;
            if (strExcelValueIfBool != null)
              strExcelValue = strExcelValueIfBool;
            if (Int16.TryParse(strExcelValue, out shortExcelValue))
              areEqual = shortTableValue == shortExcelValue;
            break;
          case "System.UInt32":
            uint uintTableValue = (uint)dataTableValue;
            uint uintExcelValue = 0;
            if (strExcelValueIfBool != null)
              strExcelValue = strExcelValueIfBool;
            if (UInt32.TryParse(strExcelValue, out uintExcelValue))
              areEqual = uintTableValue == uintExcelValue;
            break;
          case "System.Int32":
            int intTableValue = (int)dataTableValue;
            int intExcelValue = 0;
            if (strExcelValueIfBool != null)
              strExcelValue = strExcelValueIfBool;
            if (Int32.TryParse(strExcelValue, out intExcelValue))
              areEqual = intTableValue == intExcelValue;
            break;
          case "System.UInt64":
            ulong ulongTableValue = (ulong)dataTableValue;
            ulong ulongExcelValue = 0;
            if (strExcelValueIfBool != null)
              strExcelValue = strExcelValueIfBool;
            if (UInt64.TryParse(strExcelValue, out ulongExcelValue))
              areEqual = ulongTableValue == ulongExcelValue;
            break;
          case "System.Int64":
            long longTableValue = (long)dataTableValue;
            long longExcelValue = 0;
            if (strExcelValueIfBool != null)
              strExcelValue = strExcelValueIfBool;
            if (Int64.TryParse(strExcelValue, out longExcelValue))
              areEqual = longTableValue == longExcelValue;
            break;
          case "System.Decimal":
            decimal decimalTableValue = (decimal)dataTableValue;
            decimal decimalExcelValue = 0;
            if (Decimal.TryParse(strExcelValue, out decimalExcelValue))
              areEqual = decimalTableValue == decimalExcelValue;
            break;
          case "System.Single":
            float floatTableValue = (float)dataTableValue;
            float floatExcelValue = 0;
            if (Single.TryParse(strExcelValue, out floatExcelValue))
              areEqual = floatTableValue == floatExcelValue;
            break;
          case "System.Double":
            double doubleTableValue = (double)dataTableValue;
            double doubleExcelValue = 0;
            if (Double.TryParse(strExcelValue, out doubleExcelValue))
              areEqual = doubleTableValue == doubleExcelValue;
            break;
          case "System.Boolean":
            bool boolTableValue = (bool)dataTableValue;
            bool boolExcelValue = false;
            if (Boolean.TryParse(strExcelValue, out boolExcelValue))
              areEqual = boolTableValue == boolExcelValue;
            break;
          case "System.DateTime":
            DateTime dateTableValue = (DateTime)dataTableValue;
            DateTime dateExcelValue;
            if (DateTime.TryParse(strExcelValue, out dateExcelValue))
              areEqual = dateTableValue == dateExcelValue;
            break;
          case "MySql.Data.Types.MySqlDateTime":
            MySql.Data.Types.MySqlDateTime mySQLDateTableValue = (MySql.Data.Types.MySqlDateTime)dataTableValue;
            MySql.Data.Types.MySqlDateTime mySQLDateExcelValue;
            try
            {
              mySQLDateExcelValue = new MySql.Data.Types.MySqlDateTime(strExcelValue);
            }
            catch
            {
              break;
            }
            areEqual = mySQLDateTableValue.Equals(mySQLDateExcelValue);
            break;
          case "System.TimeSpan":
            TimeSpan timeTableValue = (TimeSpan)dataTableValue;
            TimeSpan timeExcelValue;
            if (TimeSpan.TryParse(strExcelValue, out timeExcelValue))
              areEqual = timeTableValue == timeExcelValue;
            break;
        }
      }

      return areEqual;
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
