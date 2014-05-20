// Copyright (c) 2014, Oracle and/or its affiliates. All rights reserved.
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

using System.Diagnostics;
using System;
using System.Xml.Serialization;
using Microsoft.Office.Interop.Excel;
using MySQL.ForExcel.Interfaces;
using MySQL.Utility.Classes;
using MySQL.Utility.Classes.MySQLWorkbench;

namespace MySQL.ForExcel.Classes
{
  /// <summary>
  /// This class stores all the information required by an import Session to be stored in disk, able to be reopened if excel is closed and restarted without closing the session.
  /// </summary>
  [Serializable]
  public class ImportSessionInfo : ISessionInfo
  {
    /// <summary>
    /// DO NOT REMOVE. Default constructor required for serialization-deserialization.
    /// </summary>
    public ImportSessionInfo()
    {
      SessionStatus = SessionErrorType.None;
      Connection = null;
      MySqlTable = null;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="ImportSessionInfo" /> class.
    /// </summary>
    /// <param name="mySqlTable">MySqlDataTable object related to the import session.</param>
    /// <param name="excelTableName">Excel Table Name</param>
    public ImportSessionInfo(MySqlDataTable mySqlTable, string excelTableName)
    {
      MySqlTable = mySqlTable;
      SchemaName = mySqlTable.SchemaName;
      TableName = mySqlTable.TableName;
      Connection = mySqlTable.WbConnection;
      ConnectionId = mySqlTable.WbConnection.Id;
      ImportColumnNames = mySqlTable.ImportColumnNames;
      SelectQuery = mySqlTable.SelectQuery;
      WorkbookGuid = Globals.ThisAddIn.Application.ActiveWorkbook.GetOrCreateId();
      WorkbookName = Globals.ThisAddIn.Application.ActiveWorkbook.Name;
      WorkbookFilePath = Globals.ThisAddIn.Application.ActiveWorkbook.FullName;
      Worksheet worksheet = Globals.ThisAddIn.Application.ActiveWorkbook.ActiveSheet;
      WorksheetName = worksheet.Name;
      ExcelTableName = excelTableName;
    }

    #region Properties

    /// <summary>
    /// Gets or sets the workbench connection object the session works with.
    /// </summary>
    [XmlIgnore]
    public MySqlWorkbenchConnection Connection { get; set; }

    /// <summary>
    /// Gets or sets the connection identifier the session works with.
    /// </summary>
    [XmlAttribute]
    public string ConnectionId { get; set; }

    /// <summary>
    /// Gets or sets the named table object.
    /// </summary>
    [XmlAttribute]
    public string ExcelTableName { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether [import column names].
    /// </summary>
    [XmlAttribute]
    public bool ImportColumnNames { get; set; }

    /// <summary>
    /// Gets or sets the last date and time the session was saved.
    /// </summary>
    [XmlAttribute]
    public DateTime LastAccess { get; set; }

    /// <summary>
    /// Gets or sets MySQL table for the import session.
    /// </summary>
    [XmlIgnore]
    public MySqlDataTable MySqlTable { get; private set; }

    /// <summary>
    /// Gets or sets the name of the schema the connection works with.
    /// </summary>
    [XmlAttribute]
    public string SchemaName { get; set; }

    /// <summary>
    /// Gets or sets the query to re-generate the contents of the MySqldataTable the session is based on.
    /// </summary>
    [XmlAttribute]
    public string SelectQuery { get; set; }

    /// <summary>
    /// Gets or sets the session type.
    /// </summary>
    [XmlAttribute]
    public SessionErrorType SessionStatus { get; set; }

    /// <summary>
    /// Gets or sets the table name the connection works with.
    /// </summary>
    [XmlAttribute]
    public string TableName { get; set; }

    /// <summary>
    /// Gets or sets the workbook guid on excel the session is making the import.
    /// </summary>
    [XmlAttribute]
    public string WorkbookGuid { get; set; }

    /// <summary>
    /// Gets or sets the name of the worbook.
    /// </summary>
    [XmlAttribute]
    public string WorkbookName { get; set; }

    /// <summary>
    /// Gets or sets the workbook full path name.
    /// </summary>
    [XmlAttribute]
    public string WorkbookFilePath { get; set; }

    /// <summary>
    /// Gets or sets the name of active worksheet.
    /// </summary>
    [XmlAttribute]
    public string WorksheetName { get; set; }

    #endregion Properties

    /// <summary>
    /// Closes this Import Session instance.
    /// </summary>
    public void Close()
    {
      Connection = null;
      MySqlTable.Dispose();
    }

    /// <summary>
    /// Tests the import session connection.
    /// </summary>
    /// <returns><c>true</c> if all connection parameters are valid to stablish the connection.</returns>
    public bool TestConnection()
    {
      Connection = MySqlWorkbench.Connections.GetConnectionForId(ConnectionId);
      if (Connection == null)
      {
        SessionStatus = SessionErrorType.WorkbenchConnectionDoesNotExist;
        return false;
      }

      Exception connectionException;
      bool connectionIsValid = Connection.TestConnection(out connectionException);
      if (connectionException != null)
      {
        //TODO: Handle specific Exception Numbers (e.g. (connectionException as MySqlException).Number)
        SessionStatus = SessionErrorType.ConnectionRefused;
      }

      return connectionIsValid;
    }

    /// <summary>
    /// Refreshes the Import Session non serializable objects and specified cells on the excel table.
    /// </summary>
    public void Refresh()
    {
      if (!TestConnection())
      {
        if (SessionStatus == SessionErrorType.WorkbenchConnectionDoesNotExist)
        {
          MySqlSourceTrace.WriteToLog(string.Format("Session for excel table '{0}.{1}.{2}' on was removed since the connection no longer exists.", WorkbookName, WorksheetName, ExcelTableName), SourceLevels.Warning);
          Globals.ThisAddIn.ActiveImportSessions.Remove(this);
        }

        return;
      }

      Connection.Schema = SchemaName;
      Connection.AllowZeroDateTimeValues = true;
      MySqlTable = Connection.CreateImportMySqlTable(false, TableName, ImportColumnNames, SelectQuery);
    }

    /// <summary>
    /// This Enumeration is used to mark the error type the session presented when tried to refresh.
    /// </summary>
    [FlagsAttribute]
    public enum SessionErrorType
    {
      /// <summary>
      /// The import session is working correctly.
      /// </summary>
      None = 0,
      /// <summary>
      /// The workbench connection was deleted and no longer exists.
      /// </summary>
      WorkbenchConnectionDoesNotExist = 1,
      /// <summary>
      /// The connection refused the current credentials or no password is provided.
      /// </summary>
      ConnectionRefused = 2,
      /// <summary>
      /// The schema was deleted from the database and no longer exists.
      /// </summary>
      SchemaNoLongerExists = 4,
      /// <summary>
      /// The table was deleted from the schema and longer exists.
      /// </summary>
      TableNoLongerExists = 8,
      /// <summary>
      /// The excel table no longer exists, the session is no longer valid and would be deleted.
      /// </summary>
      ExcelTableNoLongerExists = 16,
    }
  }
}