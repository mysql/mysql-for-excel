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
using System.Linq;
using System.Xml.Serialization;
using MySQL.ForExcel.Interfaces;
using MySQL.ForExcel.Properties;
using MySQL.Utility.Classes;
using MySQL.Utility.Classes.MySQLWorkbench;
using ExcelInterop = Microsoft.Office.Interop.Excel;
using ExcelTools = Microsoft.Office.Tools.Excel;

namespace MySQL.ForExcel.Classes
{
  /// <summary>
  /// This class stores all the information required by an import Session to be stored in disk, able to be reopened if excel is closed and restarted without closing the session.
  /// </summary>
  [Serializable]
  public class ImportSessionInfo : ISessionInfo
  {
    #region Fields

    /// <summary>
    /// The workbench connection object the session works with.
    /// </summary>
    private MySqlWorkbenchConnection _connection;

    /// <summary>
    /// The connection identifier the session works with.
    /// </summary>
    private string _connectionId;

    /// <summary>
    /// Flag indicating whether the <seealso cref="Dispose"/> method has already been called.
    /// </summary>
    private bool _disposed;

    /// <summary>
    /// The <see cref="ExcelInterop.ListObject"/> object related to the import session.
    /// </summary>
    private ExcelInterop.ListObject _excelTable;

    /// <summary>
    /// The Excel table name.
    /// </summary>
    private string _excelTableName;

    /// <summary>
    /// The name of the schema the connection works with.
    /// </summary>
    private string _schemaName;

    #endregion Fields

    /// <summary>
    /// DO NOT REMOVE. Default constructor required for serialization-deserialization.
    /// </summary>
    public ImportSessionInfo()
    {
      _connection = null;
      _connectionId = null;
      _excelTable = null;
      _excelTableName = string.Empty;
      _schemaName = string.Empty;
      SessionError = SessionErrorType.None;
      LastAccess = DateTime.Now;
      MySqlTable = null;
      ToolsExcelTable = null;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="ImportSessionInfo" /> class.
    /// </summary>
    /// <param name="mySqlTable">MySqlDataTable object related to the import session.</param>
    /// <param name="atCell">The top left Excel cell of the new <see cref="ExcelInterop.ListObject"/>.</param>
    /// <param name="addSummaryFields">Indicates wheather to include a row with summary fields</param>
    /// <param name="refreshOnCreation">Flag indicating whether the session is refreshed right away after initialized.</param>
    public ImportSessionInfo(MySqlDataTable mySqlTable, ExcelInterop.Range atCell, bool addSummaryFields, bool refreshOnCreation)
      : this()
    {
      if (mySqlTable == null)
      {
        throw new ArgumentNullException("mySqlTable");
      }

      _connection = mySqlTable.WbConnection;
      MySqlTable = mySqlTable;
      SchemaName = mySqlTable.SchemaName;
      TableName = mySqlTable.TableName;
      ConnectionId = mySqlTable.WbConnection.Id;
      ImportColumnNames = mySqlTable.ImportColumnNames;
      SelectQuery = mySqlTable.SelectQuery;
      WorkbookGuid = Globals.ThisAddIn.Application.ActiveWorkbook.GetOrCreateId();
      WorkbookName = Globals.ThisAddIn.Application.ActiveWorkbook.Name;
      WorkbookFilePath = Globals.ThisAddIn.Application.ActiveWorkbook.FullName;
      ExcelInterop.Worksheet worksheet = Globals.ThisAddIn.Application.ActiveWorkbook.ActiveSheet;
      WorksheetName = worksheet.Name;
      CreateExcelTable(atCell, addSummaryFields, refreshOnCreation);
    }

    #region Properties

    /// <summary>
    /// Gets or sets the connection identifier the session works with.
    /// </summary>
    [XmlAttribute]
    public string ConnectionId
    {
      get
      {
        return _connectionId;
      }

      set
      {
        _connectionId = value;
        if (string.IsNullOrEmpty(_connectionId))
        {
          return;
        }

        _connection = MySqlWorkbench.Connections.GetConnectionForId(ConnectionId);
        if (_connection == null)
        {
          SessionError = SessionErrorType.WorkbenchConnectionDoesNotExist;
        }
        else
        {
          _connection.Schema = SchemaName;
          _connection.AllowZeroDateTimeValues = true;
          HostIdentifier = _connection.HostIdentifier;
        }
      }
    }

    /// <summary>
    /// Gets or sets the <see cref="ExcelInterop.ListObject"/> object related to the import session.
    /// </summary>
    [XmlIgnore]
    public ExcelInterop.ListObject ExcelTable
    {
      get
      {
        return _excelTable;
      }

      set
      {
        _excelTable = value;
        if (_excelTable == null)
        {
          return;
        }

        _excelTableName = _excelTable.Name;
        ToolsExcelTable = Globals.Factory.GetVstoObject(_excelTable);
      }
    }

    /// <summary>
    /// Gets or sets the Excel table name.
    /// </summary>
    [XmlAttribute]
    public string ExcelTableName
    {
      get
      {
        return _excelTableName;
      }

      set
      {
        if (_excelTable == null)
        {
          _excelTableName = value;
        }
      }
    }

    /// <summary>
    /// Gets or sets the host identifier.
    /// </summary>
    [XmlAttribute]
    public string HostIdentifier { get; set; }

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
    public string SchemaName
    {
      get
      {
        return _schemaName;
      }

      set
      {
        _schemaName = value;
        if (_connection == null)
        {
          return;
        }

        _connection.Schema = _schemaName;
      }
    }

    /// <summary>
    /// Gets or sets the query to re-generate the contents of the MySqldataTable the session is based on.
    /// </summary>
    [XmlAttribute]
    public string SelectQuery { get; set; }

    /// <summary>
    /// Gets or sets a session error identifier.
    /// </summary>
    [XmlAttribute]
    public SessionErrorType SessionError { get; set; }

    /// <summary>
    /// Gets or sets the table name the connection works with.
    /// </summary>
    [XmlAttribute]
    public string TableName { get; set; }

    /// <summary>
    /// Gets or sets the table name the connection works with.
    /// </summary>
    [XmlIgnore]
    public ExcelTools.ListObject ToolsExcelTable { get; private set; }

    /// <summary>
    /// Gets or sets the workbook full path name.
    /// </summary>
    [XmlAttribute]
    public string WorkbookFilePath { get; set; }

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
    /// Gets or sets the name of active worksheet.
    /// </summary>
    [XmlAttribute]
    public string WorksheetName { get; set; }

    #endregion Properties

    #region Enums

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

    #endregion Enums

    /// <summary>
    /// Releases all resources used by the <see cref="ImportSessionInfo"/> class
    /// </summary>
    public void Dispose()
    {
      Dispose(true);
      GC.SuppressFinalize(this);
    }

    /// <summary>
    /// Refreshes the Import Session non serializable objects and specified cells on the excel table.
    /// </summary>
    public void Refresh()
    {
      if (MySqlTable == null || ToolsExcelTable == null)
      {
        return;
      }

      // Test the connection before attempting the data refresh.
      if (!TestConnection())
      {
        if (SessionError == SessionErrorType.WorkbenchConnectionDoesNotExist)
        {
          MySqlSourceTrace.WriteToLog(string.Format("Session for excel table '{0}.{1}.{2}' on was removed since the connection no longer exists.", WorkbookName, WorksheetName, ExcelTableName), SourceLevels.Warning);
          Globals.ThisAddIn.ActiveImportSessions.Remove(this);
        }

        return;
      }

      try
      {
        // In case the table is bound (it should not be) then disconnect it.
        if (ToolsExcelTable.IsBinding)
        {
          ToolsExcelTable.Disconnect();
        }

        // Refresh the data on the MySqlDataTable and bind it so the Excel table is refreshed.
        Exception ex;
        MySqlTable.RefreshData(out ex);

        // Resize the ExcelTools.ListObject by giving it an ExcelInterop.Range calculated with the refreshed MySqlDataTable dimensions.
        // Detection of a collision with another Excel object must be performed first and if any then shift rows and columns to fix the collision.
        ExcelInterop.Range newRange = ToolsExcelTable.Range.Cells[1, 1];
        newRange = newRange.Resize[MySqlTable.Rows.Count + 1, MySqlTable.Columns.Count];
        var intersectingRange = newRange.GetIntersectingRangeWithAnyExcelObject(true, true, true, _excelTable.Comment);
        if (intersectingRange != null && intersectingRange.CountLarge != 0)
        {
          ExcelInterop.Range bottomRightCell = newRange.Cells[newRange.Rows.Count, newRange.Columns.Count];

          // Determine if the collision is avoided by inserting either new columns or new rows.
          if (intersectingRange.Columns.Count < intersectingRange.Rows.Count)
          {
            for (int colIdx = 0; colIdx <= intersectingRange.Columns.Count; colIdx++)
            {
              bottomRightCell.EntireColumn.Insert(ExcelInterop.XlInsertShiftDirection.xlShiftToRight, Type.Missing);
            }
          }
          else
          {
            for (int rowIdx = 0; rowIdx <= intersectingRange.Rows.Count; rowIdx++)
            {
              bottomRightCell.EntireRow.Insert(ExcelInterop.XlInsertShiftDirection.xlShiftDown, Type.Missing);
            }
          }

          // Redimension the new range. This is needed since the new rows or columns inserted are not present in the previously calculated one.
          newRange = ToolsExcelTable.Range.Cells[1, 1];
          newRange = newRange.Resize[MySqlTable.Rows.Count + 1, MySqlTable.Columns.Count];
        }

        ToolsExcelTable.Resize(newRange);

        // Bind the redimensioned ExcelTools.ListObject to the MySqlDataTable.
        ToolsExcelTable.SetDataBinding(MySqlTable);
        foreach (MySqlDataColumn col in MySqlTable.Columns)
        {
          ToolsExcelTable.ListColumns[col.Ordinal + 1].Name = col.DisplayName;
        }

        ToolsExcelTable.Range.Columns.AutoFit();

        // Disconnect the table so users can freely modify the data imported to the Excel table's range.
        ToolsExcelTable.Disconnect();
      }
      catch (Exception ex)
      {
        MiscUtilities.ShowCustomizedErrorDialog(string.Format(Resources.RefreshDataError, _excelTableName), ex.Message, true);
        MySqlSourceTrace.WriteAppErrorToLog(ex);
      }
    }

    /// <summary>
    /// Restores the internal session objects.
    /// </summary>
    public void Restore()
    {
      Restore(Globals.ThisAddIn.Application.Workbooks.Cast<ExcelInterop.Workbook>().FirstOrDefault(wb => string.Equals(wb.Name, WorkbookName, StringComparison.InvariantCultureIgnoreCase)));
    }

    /// <summary>
    /// Restores the internal session objects.
    /// </summary>
    /// <param name="workbook">The <see cref="ExcelInterop.Workbook"/> tied to the session.</param>
    public void Restore(ExcelInterop.Workbook workbook)
    {
      if (workbook == null || workbook.GetOrCreateId() != WorkbookGuid)
      {
        return;
      }

      if (_excelTable == null && !string.IsNullOrEmpty(_excelTableName))
      {
        ExcelTable = workbook.GetExcelTableByName(WorksheetName, _excelTableName);
        if (ExcelTable == null)
        {
          return;
        }
      }

      if (MySqlTable == null)
      {
        if (_connection != null)
        {
          MySqlTable = _connection.CreateImportMySqlTable(false, TableName, ImportColumnNames, SelectQuery);
        }
        else
        {
          SessionError = SessionErrorType.WorkbenchConnectionDoesNotExist;
        }
      }
    }

    /// <summary>
    /// Tests the import session connection.
    /// </summary>
    /// <returns><c>true</c> if all connection parameters are valid to stablish the connection.</returns>
    public bool TestConnection()
    {
      if (_connection == null)
      {
        SessionError = SessionErrorType.WorkbenchConnectionDoesNotExist;
        return false;
      }

      Exception connectionException;
      bool connectionIsValid = _connection.TestConnection(out connectionException);
      if (connectionException != null)
      {
        SessionError = SessionErrorType.ConnectionRefused;
      }

      return connectionIsValid;
    }

    /// <summary>
    /// Releases all resources used by the <see cref="ImportSessionInfo"/> class
    /// </summary>
    /// <param name="disposing">If true this is called by Dispose(), otherwise it is called by the finalizer</param>
    protected virtual void Dispose(bool disposing)
    {
      if (_disposed)
      {
        return;
      }

      // Free managed resources
      if (disposing)
      {
        if (ToolsExcelTable != null)
        {
          if (ToolsExcelTable.IsBinding)
          {
            ToolsExcelTable.Disconnect();
          }

          ToolsExcelTable.DeleteSafely(true);
        }

        if (MySqlTable != null)
        {
          MySqlTable.Dispose();
        }

        // Set variables to null so this object does not hold references to them and the GC disposes of them sooner.
        _connection = null;
        MySqlTable = null;
        ExcelTable = null;
        ToolsExcelTable = null;

        // Attempt to remove the dummy connection created for this import session
        RemoveWorkbookConnection();
      }

      // Add class finalizer if unmanaged resources are added to the class
      // Free unmanaged resources if there are any
      _disposed = true;
    }

    /// <summary>
    /// Creates an Excel table starting at the given cell containing the data in a <see cref="MySqlDataTable"/> instance.
    /// </summary>
    /// <param name="atCell">The top left Excel cell of the new <see cref="ExcelInterop.ListObject"/>.</param>
    /// <param name="addSummaryFields">Indicates wheather to include a row with summary fields</param>
    /// <param name="refreshOnCreation">Flag indicating whether the session is refreshed right away after initialized.</param>
    private void CreateExcelTable(ExcelInterop.Range atCell, bool addSummaryFields, bool refreshOnCreation)
    {
      if (atCell == null)
      {
        throw new ArgumentNullException("atCell");
      }

      string proposedName = MySqlTable.ExcelTableName;
      var worksheet = Globals.Factory.GetVstoObject(atCell.Worksheet);
      var workbook = worksheet.Parent as ExcelInterop.Workbook;
      if (workbook == null)
      {
        MySqlSourceTrace.WriteToLog(string.Format(Resources.ParentWorkbookNullError, worksheet.Name, proposedName));
        return;
      }

      string workbookGuid = workbook.GetOrCreateId();
      try
      {
        int consecutiveIfOrphanedTable = 2;
        string commandText;
        string connectionName;
        string connectionStringForCmdDefault = MySqlTable.WbConnection.GetConnectionStringForCmdDefault();
        do
        {
          // Prepare Excel table name and dummy connection
          proposedName = proposedName.GetExcelTableNameAvoidingDuplicates();
          commandText = workbook.GetCommandText(proposedName);
          connectionName = workbook.GetConnectionName(proposedName);

          // Check first if there is an orphaned Tools Excel table (leftover from a deleted Interop Excel table) and if so then attempt to free resources.
          if (!worksheet.Controls.Contains(proposedName))
          {
            break;
          }

          var toolsExcelTable = worksheet.Controls[proposedName] as ExcelTools.ListObject;
          toolsExcelTable.DisconnectAndDelete(false);

          // At this point a new name is needed since for some reason or bug the Globals.Factory throws an error
          // trying to check if there is a Tools Excel table already for the existing name, so go back to that point.
          proposedName = string.Format("{0}-{1}", MySqlTable.ExcelTableName, consecutiveIfOrphanedTable++);
        } while (true);

        // Create empty Interop Excel table that will be connected to a data source
        var hasHeaders = ImportColumnNames ? ExcelInterop.XlYesNoGuess.xlYes : ExcelInterop.XlYesNoGuess.xlNo;
        var excelTable = worksheet.ListObjects.Add(ExcelInterop.XlListObjectSourceType.xlSrcExternal, connectionStringForCmdDefault, false, hasHeaders, atCell);
        excelTable.Name = proposedName;
        excelTable.TableStyle = Settings.Default.ImportExcelTableStyleName;
        excelTable.QueryTable.BackgroundQuery = false;
        excelTable.QueryTable.CommandText = commandText;
        excelTable.Comment = Guid.NewGuid().ToString();
        excelTable.ShowTotals = addSummaryFields;
        ExcelTable = excelTable;

        // Add a connection to the Workbook, the method used to add it differs since the Add method is obsolete for Excel 2013 and higher.
        if (Globals.ThisAddIn.ExcelVersionNumber < ThisAddIn.EXCEL_2013_VERSION_NUMBER)
        {
          workbook.Connections.Add(connectionName, string.Empty, connectionStringForCmdDefault, commandText, ExcelInterop.XlCmdType.xlCmdDefault);
        }
        else
        {
          workbook.Connections.Add2(connectionName, string.Empty, connectionStringForCmdDefault, commandText, ExcelInterop.XlCmdType.xlCmdDefault);
        }

        // Add this instance of the ImportSessionInfo class if not present already in the global collection.;
        if (!Globals.ThisAddIn.ActiveImportSessions.Exists(session => session.WorkbookGuid == workbookGuid && session.MySqlTable == MySqlTable && string.Equals(session.ExcelTableName, proposedName, StringComparison.InvariantCultureIgnoreCase)))
        {
          Globals.ThisAddIn.ActiveImportSessions.Add(this);
        }

        if (refreshOnCreation)
        {
          Refresh();
        }
      }
      catch (Exception ex)
      {
        MiscUtilities.ShowCustomizedErrorDialog(string.Format(Resources.ExcelTableCreationError, proposedName), ex.Message, true);
        MySqlSourceTrace.WriteAppErrorToLog(ex);
      }
    }

    /// <summary>
    /// Attempts to delete the <see cref="ExcelInterop.WorkbookConnection"/> created for this session.
    /// </summary>
    private void RemoveWorkbookConnection()
    {
      if (_excelTable == null)
      {
        return;
      }

      var workbook = Globals.ThisAddIn.Application.ActiveWorkbook;
      var workbookConnection = workbook.Connections.Cast<ExcelInterop.WorkbookConnection>().FirstOrDefault(conn => conn.Name == workbook.GetConnectionName(_excelTable.Name));
      if (workbookConnection == null)
      {
        return;
      }

      workbookConnection.Delete();
    }
  }
}