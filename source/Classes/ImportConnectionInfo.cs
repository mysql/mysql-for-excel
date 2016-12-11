// Copyright (c) 2014, 2016, Oracle and/or its affiliates. All rights reserved.
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
using MySQL.ForExcel.Classes.Exceptions;
using MySQL.ForExcel.Interfaces;
using MySQL.ForExcel.Properties;
using MySql.Utility.Classes.MySql;
using MySql.Utility.Classes.MySqlWorkbench;
using ExcelInterop = Microsoft.Office.Interop.Excel;
using ExcelTools = Microsoft.Office.Tools.Excel;

namespace MySQL.ForExcel.Classes
{
  /// <summary>
  /// This class stores all the connection information required by an import to be stored in disk, able to be reopened if excel is closed and restarted.
  /// </summary>
  [Serializable]
  public class ImportConnectionInfo : IConnectionInfo
  {
    #region Fields

    /// <summary>
    /// The workbench connection object the <see cref="ImportConnectionInfo" /> works with.
    /// </summary>
    private MySqlWorkbenchConnection _connection;

    /// <summary>
    /// The connection identifier the <see cref="ImportConnectionInfo" /> works with.
    /// </summary>
    private string _connectionId;

    /// <summary>
    /// Flag indicating whether the <seealso cref="Dispose"/> method has already been called.
    /// </summary>
    private bool _disposed;

    /// <summary>
    /// The <see cref="ExcelInterop.ListObject"/> object related to the <see cref="ImportConnectionInfo" />.
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
    public ImportConnectionInfo()
    {
      _connection = null;
      _connectionId = null;
      _excelTable = null;
      _excelTableName = string.Empty;
      _schemaName = string.Empty;
      ConnectionInfoError = ConnectionInfoErrorType.None;
      LastAccess = DateTime.Now;
      MySqlTable = null;
      ToolsExcelTable = null;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="ImportConnectionInfo" /> class.
    /// </summary>
    /// <param name="mySqlTable">MySqlDataTable object related to the <see cref="ImportConnectionInfo" />.</param>
    /// <param name="atCell">The top left Excel cell of the new <see cref="ExcelInterop.ListObject"/>.</param>
    /// <param name="addSummaryRow">Flag indicating whether to include a row with summary fields at the end of the data rows.</param>
    public ImportConnectionInfo(MySqlDataTable mySqlTable, ExcelInterop.Range atCell, bool addSummaryRow)
      : this()
    {
      if (mySqlTable == null)
      {
        throw new ArgumentNullException(nameof(mySqlTable));
      }

      _connection = mySqlTable.WbConnection;
      MySqlTable = mySqlTable;
      SchemaName = mySqlTable.SchemaName;
      TableName = mySqlTable.TableName;
      ConnectionId = mySqlTable.WbConnection.Id;
      ImportColumnNames = mySqlTable.ImportColumnNames;
      OperationType = mySqlTable.OperationType;
      ProcedureResultSetIndex = mySqlTable.ProcedureResultSetIndex;
      SelectQuery = mySqlTable.SelectQuery;
      var activeWorkbook = Globals.ThisAddIn.ActiveWorkbook;
      WorkbookGuid = activeWorkbook.GetOrCreateId();
      WorkbookName = activeWorkbook.Name;
      WorkbookFilePath = activeWorkbook.FullName;
      ExcelInterop.Worksheet worksheet = activeWorkbook.ActiveSheet;
      WorksheetName = worksheet.Name;
      InitializeConnectionObjects(atCell, addSummaryRow);
    }

    #region Properties

    /// <summary>
    /// Gets or sets the connection identifier the <see cref="ImportConnectionInfo" /> object works with.
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
          ConnectionInfoError = ConnectionInfoErrorType.WorkbenchConnectionDoesNotExist;
        }
        else
        {
          _connection.SetAdditionalConnectionProperties();
          HostIdentifier = _connection.HostIdentifier;
        }
      }
    }

    /// <summary>
    /// Gets or sets the <see cref="ExcelInterop.ListObject"/> object related to the <see cref="ImportConnectionInfo" />.
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
    /// Gets or sets the last date and time the <see cref="ImportConnectionInfo" /> was saved.
    /// </summary>
    [XmlAttribute]
    public DateTime LastAccess { get; set; }

    /// <summary>
    /// Gets or sets MySQL table for the <see cref="ImportConnectionInfo" />.
    /// </summary>
    [XmlIgnore]
    public MySqlDataTable MySqlTable { get; private set; }

    /// <summary>
    /// Gets or sets the <see cref="MySqlDataTable.DataOperationType"/> intended for the <see cref="MySqlTable"/>.
    /// </summary>
    [XmlAttribute]
    public MySqlDataTable.DataOperationType OperationType { get; set; }

    /// <summary>
    /// Gets or sets the index of the result set of a stored procedure this table contains data for.
    /// </summary>
    /// <remarks>-1 represents the output parameters and return values result set.</remarks>
    [XmlAttribute]
    public int ProcedureResultSetIndex { get; set; }

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
    /// Gets or sets the query to re-generate the contents of the MySqldataTable the <see cref="ImportConnectionInfo" /> is based on.
    /// </summary>
    [XmlAttribute]
    public string SelectQuery { get; set; }

    /// <summary>
    /// Gets or sets a <see cref="ImportConnectionInfo" /> error identifier if there is a problem with the MySqlWorkbenchConnection.
    /// </summary>
    [XmlAttribute]
    public ConnectionInfoErrorType ConnectionInfoError { get; set; }

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
    /// Gets or sets the workbook guid on excel the <see cref="ImportConnectionInfo" /> is making the import.
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
    /// This Enumeration is used to mark the error type the <see cref="ImportConnectionInfo" /> presented when tried to refresh.
    /// </summary>
    [FlagsAttribute]
    public enum ConnectionInfoErrorType
    {
      /// <summary>
      /// The import <see cref="ImportConnectionInfo" /> is working correctly.
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
      /// The excel table no longer exists, the <see cref="ImportConnectionInfo" /> is no longer valid and would be deleted.
      /// </summary>
      ExcelTableNoLongerExists = 16,
    }

    #endregion Enums

    /// <summary>
    /// Releases all resources used by the <see cref="ImportConnectionInfo"/> class
    /// </summary>
    public void Dispose()
    {
      Dispose(true);
      GC.SuppressFinalize(this);
    }

    /// <summary>
    /// Unbinds the <see cref="ToolsExcelTable"/>, refreshes the data on the <see cref="MySqlTable"/> and binds it again to the <see cref="ToolsExcelTable"/>.
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
        if (ConnectionInfoError != ConnectionInfoErrorType.WorkbenchConnectionDoesNotExist)
        {
          return;
        }

        // If the Workbench connection does not exist anymore, log a message to the log, remove this object from the global connections collection and exit.
        MySqlSourceTrace.WriteToLog(string.Format(Resources.ImportConnectionInfoRemovedConnectionText, WorkbookName, WorksheetName, ExcelTableName), false, SourceLevels.Warning);
        Globals.ThisAddIn.StoredImportConnectionInfos.Remove(this);
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
        MySqlTable.RefreshData();

        // Bind the table again after it was refreshed.
        BindMySqlDataTable();
      }
      catch (Exception ex)
      {
        MySqlSourceTrace.WriteAppErrorToLog(ex, null, string.Format(Resources.ImportDataRefreshError, _excelTableName), true);
      }
    }

    /// <summary>
    /// Restores the internal objects.
    /// </summary>
    /// <param name="workbook">The <see cref="ExcelInterop.Workbook"/> tied to this <see cref="ImportConnectionInfo"/> object.</param>
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

      if (MySqlTable != null)
      {
        return;
      }

      if (!TestConnection())
      {
        return;
      }

      _connection.Schema = SchemaName;
      MySqlTable = _connection.CreateImportMySqlTable(OperationType, TableName, ImportColumnNames, SelectQuery, ProcedureResultSetIndex);
    }

    /// <summary>
    /// Tests the <see cref="ImportConnectionInfo"/> connection.
    /// </summary>
    /// <returns><c>true</c> if all connection parameters are valid to establish the connection.</returns>
    public bool TestConnection()
    {
      if (_connection == null)
      {
        ConnectionInfoError = ConnectionInfoErrorType.WorkbenchConnectionDoesNotExist;
        return false;
      }

      if (_connection.ConnectionStatus == MySqlWorkbenchConnection.ConnectionStatusType.AcceptingConnections)
      {
        ConnectionInfoError = ConnectionInfoErrorType.None;
        return true;
      }

      var passwordFlags = _connection.TestConnectionAndRetryOnWrongPassword();
      ConnectionInfoError = !passwordFlags.ConnectionSuccess
          ? ConnectionInfoErrorType.ConnectionRefused
          : ConnectionInfoErrorType.None;
      return ConnectionInfoError == ConnectionInfoErrorType.None;
    }

    /// <summary>
    /// Releases all resources used by the <see cref="ImportConnectionInfo"/> class
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

          ToolsExcelTable.DeleteSafely(false);
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
      }

      // Add class finalizer if unmanaged resources are added to the class
      // Free unmanaged resources if there are any
      _disposed = true;
    }

    /// <summary>
    /// Binds the <see cref="MySqlTable"/> to the <see cref="ToolsExcelTable" /> so its data can be refreshed.
    /// </summary>
    public void BindMySqlDataTable()
    {
      if (MySqlTable == null || ToolsExcelTable == null)
      {
        return;
      }

      try
      {
        // In case the table is bound (it should not be) then disconnect it.
        if (ToolsExcelTable.IsBinding)
        {
          ToolsExcelTable.Disconnect();
        }

        // Skip Worksheet events
        Globals.ThisAddIn.SkipWorksheetChangeEvent = true;
        Globals.ThisAddIn.SkipSelectedDataContentsDetection = true;

        // Resize the ExcelTools.ListObject by giving it an ExcelInterop.Range calculated with the refreshed MySqlDataTable dimensions.
        // Detection of a collision with another Excel object must be performed first and if any then shift rows and columns to fix the collision.
        const int headerRows = 1;
        int summaryRows = ExcelTable.ShowTotals ? 1 : 0;
        ExcelInterop.Range newRange = ToolsExcelTable.Range.Cells[1, 1];
        newRange = newRange.SafeResize(MySqlTable.Rows.Count + headerRows + summaryRows, MySqlTable.Columns.Count);
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
          newRange = newRange.SafeResize(MySqlTable.Rows.Count + headerRows + summaryRows, MySqlTable.Columns.Count);
        }

        // Redimension the ExcelTools.ListObject's range
        ToolsExcelTable.Resize(newRange);

        // Re-format the importing range
        ExcelInterop.Range dataOnlyRange = newRange.Offset[headerRows];
        dataOnlyRange = dataOnlyRange.Resize[newRange.Rows.Count - headerRows - summaryRows];
        MySqlTable.FormatImportExcelRange(dataOnlyRange, true);

        // Bind the redimensioned ExcelTools.ListObject to the MySqlDataTable
        ToolsExcelTable.SetDataBinding(MySqlTable);

        // Rename columns
        var importColumnNames = MySqlTable.ImportColumnNames;
        for (int colIndex = MySqlTable.Columns.Count - 1; colIndex >= 0; colIndex--)
        {
          var col = MySqlTable.Columns[colIndex] as MySqlDataColumn;
          if (col == null)
          {
            continue;
          }

          ToolsExcelTable.ListColumns[col.Ordinal + 1].Name = importColumnNames ? col.DisplayName : col.OrdinalColumnName;
        }

        ToolsExcelTable.Range.Columns.AutoFit();

        // Disconnect the table so users can freely modify the data imported to the Excel table's range.
        ToolsExcelTable.Disconnect();
      }
      catch (Exception ex)
      {
        MySqlSourceTrace.WriteAppErrorToLog(ex, null, string.Format(Resources.ImportDataBindError, _excelTableName), true);
      }
      finally
      {
        Globals.ThisAddIn.SkipWorksheetChangeEvent = false;
        Globals.ThisAddIn.SkipSelectedDataContentsDetection = false;
      }
    }

    /// <summary>
    /// Creates both <see cref="ExcelInterop.ListObject"/> and <see cref="ExcelTools.ListObject"/> from an external data source and places the data at the given Excel cell.
    /// </summary>
    /// <remarks>This method must be used in Excel versions lesser than 15 (2013) where the Data Model is not supported.</remarks>
    /// <param name="worksheet"></param>
    /// <param name="atCell">The top left Excel cell of the new <see cref="ExcelInterop.ListObject"/>.</param>
    /// <param name="addSummaryRow">Flag indicating whether to include a row with summary fields at the end of the data rows.</param>
    private void CreateExcelTableFromExternalSource(ExcelTools.Worksheet worksheet, ExcelInterop.Range atCell, bool addSummaryRow)
    {
      // Prepare Excel table name and dummy connection string
      string proposedName = MySqlTable.ExcelTableName;
      string excelTableName = worksheet.GetExcelTableNameAvoidingDuplicates(proposedName);
      string workbookConnectionName = excelTableName.StartsWith("MySQL.") ? excelTableName : "MySQL." + excelTableName;
      workbookConnectionName = workbookConnectionName.GetWorkbookConnectionNameAvoidingDuplicates();
      string connectionStringForCmdDefault = MySqlTable.WbConnection.GetConnectionStringForCmdDefault();

      // Create empty Interop Excel table that will be connected to a data source.
      // This automatically creates a Workbook connection as well although the data refresh does not use the Workbook connection since it is a dummy one.
      var hasHeaders = ImportColumnNames ? ExcelInterop.XlYesNoGuess.xlYes : ExcelInterop.XlYesNoGuess.xlNo;
      var excelTable = worksheet.ListObjects.Add(ExcelInterop.XlListObjectSourceType.xlSrcExternal, connectionStringForCmdDefault, false, hasHeaders, atCell);
      excelTable.Name = excelTableName;
      excelTable.TableStyle = Settings.Default.ImportExcelTableStyleName;
      excelTable.QueryTable.BackgroundQuery = false;
      excelTable.QueryTable.CommandText = MySqlTable.SelectQuery.Replace("`", string.Empty);
      excelTable.QueryTable.WorkbookConnection.Name = workbookConnectionName;
      excelTable.QueryTable.WorkbookConnection.Description = Resources.WorkbookConnectionForExcelTableDescription;
      excelTable.Comment = Guid.NewGuid().ToString();
      if (addSummaryRow)
      {
        excelTable.AddSummaryRow();
      }

      ExcelTable = excelTable;
    }

    /// <summary>
    /// Creates an Excel table starting at the given cell containing the data in a <see cref="MySqlDataTable"/> instance.
    /// </summary>
    /// <param name="importDataAtCell">The top left Excel cell of the new <see cref="ExcelInterop.ListObject"/>.</param>
    /// <param name="addSummaryRow">Flag indicating whether to include a row with summary fields at the end of the data rows.</param>
    private void InitializeConnectionObjects(ExcelInterop.Range importDataAtCell, bool addSummaryRow)
    {
      if (importDataAtCell == null)
      {
        throw new ArgumentNullException("importDataAtCell");
      }

      var worksheet = Globals.Factory.GetVstoObject(importDataAtCell.Worksheet);
      var workbook = worksheet.Parent as ExcelInterop.Workbook;
      if (workbook == null)
      {
        throw new ParentWorkbookNullException(worksheet.Name);
      }

      string workbookGuid = workbook.GetOrCreateId();
      try
      {
        // Create the Excel table needed to place the imported data into the Excel worksheet.
        CreateExcelTableFromExternalSource(worksheet, importDataAtCell, addSummaryRow);

        // Bind the MySqlDataTable already filled with data to the Excel table.
        BindMySqlDataTable();

        // Add this instance of the ImportConnectionInfo class if not present already in the global collection.
        if (!Globals.ThisAddIn.StoredImportConnectionInfos.Exists(connectionInfo => connectionInfo.WorkbookGuid == workbookGuid && connectionInfo.MySqlTable == MySqlTable && string.Equals(connectionInfo.ExcelTableName, ExcelTable.Name, StringComparison.InvariantCultureIgnoreCase)))
        {
          Globals.ThisAddIn.StoredImportConnectionInfos.Add(this);
        }
      }
      catch (Exception ex)
      {
        MySqlSourceTrace.WriteAppErrorToLog(ex, null, string.Format(Resources.ExcelTableCreationError, ExcelTable != null ? ExcelTable.Name : MySqlTable.ExcelTableName), false);
      }
    }
  }
}