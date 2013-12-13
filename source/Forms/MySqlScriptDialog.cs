// Copyright (c) 2013, Oracle and/or its affiliates. All rights reserved.
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

using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using MySql.Data.MySqlClient;
using MySQL.ForExcel.Classes;
using MySQL.ForExcel.Interfaces;
using MySQL.ForExcel.Properties;
using MySQL.Utility.Classes;
using MySQL.Utility.Classes.MySQLWorkbench;
using MySQL.Utility.Forms;

namespace MySQL.ForExcel.Forms
{
  /// <summary>
  /// Presents user with a dialog to review and modify a SQL query before it is submitted to the connected MySQL server.
  /// </summary>
  public partial class MySqlScriptDialog : AutoStyleableBaseDialog
  {
    #region Constants

    /// <summary>
    /// The zooming step used to increase or decrease the font size of text in the rich text editor. 
    /// </summary>
    public const float ZOOMING_STEP = 1.1F;

    /// <summary>
    /// The default height for the <see cref="QueryTextBox"/> control when the original operations information label is not shown.
    /// </summary>
    private const int DEFAULT_HEIGHT_QUERY_TEXTBOX_NO_INFO = 337;

    /// <summary>
    /// The default height for the <see cref="QueryTextBox"/> control when the original operations information label is shown.
    /// </summary>
    private const int DEFAULT_HEIGHT_QUERY_TEXTBOX_WITH_INFO = 317;

    /// <summary>
    /// The default vertical location for the <see cref="QueryTextBox"/> control when the original operations information label is not shown.
    /// </summary>
    private const int DEFAULT_Y_LOCATION_QUERY_TEXTBOX_NO_INFO = 48;

    /// <summary>
    /// The default vertical location for the <see cref="QueryTextBox"/> control when the original operations information label is shown.
    /// </summary>
    private const int DEFAULT_Y_LOCATION_QUERY_TEXTBOX_WITH_INFO = 68;

    #endregion Constants

    #region Fields

    /// <summary>
    /// String builder containing error messages displayed to the user.
    /// </summary>
    private StringBuilder _errorDetails;

    /// <summary>
    /// Contains the summary text displayed to users if the script executes with errors.
    /// </summary>
    private string _errorDialogSummary;

    /// <summary>
    /// Flag indicating whether when text changes in the <see cref="QueryTextBox"/> was due user input or programatic.
    /// </summary>
    private bool _isUserInput;

    /// <summary>
    /// The value of the MAX_ALLOWED_PACKET MySQL Server variable indicating the max size in bytes of the packet returned by a single query.
    /// </summary>
    private ulong _mySqlMaxAllowedPacket;

    /// <summary>
    /// A <see cref="MySqlDataTable"/> object containing data changes to be committed to the database.
    /// </summary>
    private readonly MySqlDataTable _mySqlTable;

    /// <summary>
    /// MySQL Workbench connection to a MySQL server instance selected by users.
    /// </summary>
    private readonly MySqlWorkbenchConnection _wbConnection;

    #endregion Fields

    /// <summary>
    /// Initializes a new instance of the <see cref="MySqlScriptDialog"/> class.
    /// </summary>
    /// <param name="wbConnection">The connection to a MySQL server instance selected by users.</param>
    /// <param name="sqlScript">The proposed SQL query for user review and possible modification.</param>
    /// <param name="useOptimisticUpdate">Flag indicating whether optimistic locking is used for the update of rows.</param>
    public MySqlScriptDialog(MySqlWorkbenchConnection wbConnection, string sqlScript, bool useOptimisticUpdate)
      : this()
    {
      _errorDialogSummary = Resources.ScriptErrorThrownSummary;
      _wbConnection = wbConnection;
      OriginalSqlScript = sqlScript;
      SqlScript = OriginalSqlScript;
      UseOptimisticUpdate = useOptimisticUpdate;
      CreateOriginalStatementsList();
      ApplyButton.Enabled = SqlScript.Trim().Length > 0;
      SetOriginalOperationsInfoAvailability();
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="MySqlScriptDialog"/> class.
    /// </summary>
    /// <param name="mySqlTable">The <see cref="MySqlDataTable"/> object containing data changes to be committed to the database.</param>
    public MySqlScriptDialog(MySqlDataTable mySqlTable)
      : this()
    {
      if (mySqlTable != null)
      {
        switch (mySqlTable.OperationType)
        {
          case MySqlDataTable.DataOperationType.Export:
            _errorDialogSummary = string.Format(Resources.ExportDataOperationErrorText, mySqlTable.TableName);
            break;

          case MySqlDataTable.DataOperationType.Append:
            _errorDialogSummary = string.Format(Resources.AppendDataDetailsDoneErrorText, mySqlTable.TableName);
            break;

          case MySqlDataTable.DataOperationType.Edit:
            _errorDialogSummary = string.Format(Resources.EditedDataForTable, mySqlTable.TableName) + Resources.EditedDataCommittedError;
            break;

          default:
            _errorDialogSummary = Resources.ScriptErrorThrownSummary;
            break;
        }

        _mySqlTable = mySqlTable;
        _wbConnection = _mySqlTable.WbConnection;
        UseOptimisticUpdate = _mySqlTable.UseOptimisticUpdate;
        ShowOriginalOperationsInformation = true;
        CreateOriginalStatementsList();
        SetOriginalOperationsInfoAvailability();
      }

      SqlScript = OriginalSqlScript;
      ApplyButton.Enabled = SqlScript.Trim().Length > 0;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="MySqlScriptDialog"/> class.
    /// </summary>
    private MySqlScriptDialog()
    {
      _errorDetails = null;
      _errorDialogSummary = null;
      _isUserInput = true;
      _mySqlMaxAllowedPacket = 0;
      _mySqlTable = null;
      _wbConnection = null;
      ActualStatementRowsList = null;
      OriginalSqlScript = null;
      OriginalStatementRowsList = null;
      ScriptResult = MySqlStatement.StatementResultType.NotApplied;
      ShowOriginalOperationsInformation = false;

      InitializeComponent();
      OriginalQueryButton.Enabled = false;
      ResetTextZoom();
    }

    #region Properties

    /// <summary>
    /// Gets a list of SQL statements tied to a specific data row.
    /// </summary>
    public List<IMySqlDataRow> ActualStatementRowsList { get; private set; }

    /// <summary>
    /// Gets the number of delete operations successfully performed against the database server.
    /// </summary>
    public int DeletedOperations
    {
      get
      {
        return ActualStatementRowsList.GetResultsCount(MySqlStatement.SqlStatementType.Delete);
      }
    }

    /// <summary>
    /// Gets the number of insert operations successfully performed against the database server.
    /// </summary>
    public int InsertedOperations
    {
      get
      {
        return ActualStatementRowsList.GetResultsCount(MySqlStatement.SqlStatementType.Insert);
      }
    }

    /// <summary>
    /// Gets the value of the MAX_ALLOWED_PACKET MySQL Server variable indicating the max size in bytes of the packet returned by a single query.
    /// </summary>
    public ulong MySqlMaxAllowedPacket
    {
      get
      {
        if (_mySqlMaxAllowedPacket == 0)
        {
          _mySqlMaxAllowedPacket = _wbConnection.GetMySqlServerMaxAllowedPacket();
        }

        return _mySqlMaxAllowedPacket;
      }
    }

    /// <summary>
    /// Gets the original SQL script without any user modifications.
    /// </summary>
    public string OriginalSqlScript { get; private set; }

    /// <summary>
    /// Gets the overall result type of the applied script.
    /// </summary>
    public MySqlStatement.StatementResultType ScriptResult { get; private set; }

    /// <summary>
    /// Gets a value indicating whether original operations from a <see cref="MySqlDataTable"/> are shown above the SQL statements.
    /// </summary>
    public bool ShowOriginalOperationsInformation { get; private set; }

    /// <summary>
    /// Gets the SQL query edited by the user.
    /// </summary>
    public string SqlScript
    {
      get
      {
        return QueryTextBox.Text.Replace("\n", Environment.NewLine);
      }

      private set
      {
        _isUserInput = false;
        QueryTextBox.Text = value;
        _isUserInput = true;
      }
    }

    /// <summary>
    /// Gets a list of SQL statements tied to a specific data row.
    /// </summary>
    public List<IMySqlDataRow> OriginalStatementRowsList { get; private set; }

    /// <summary>
    /// Gets the number of update operations successfully performed against the database server.
    /// </summary>
    public int UpdatedOperations
    {
      get
      {
        return ActualStatementRowsList.GetResultsCount(MySqlStatement.SqlStatementType.Update);
      }
    }

    /// <summary>
    /// Gets or sets a value indicating whether optimistic locking is used for the update of rows.
    /// </summary>
    public bool UseOptimisticUpdate { get; private set; }

    #endregion Properties

    /// <summary>
    /// Event delegate method fired when the <see cref="ApplyButton"/> is clicked.
    /// </summary>
    /// <param name="sender">Sender object.</param>
    /// <param name="e">Event arguments.</param>
    private void ApplyButton_Click(object sender, EventArgs e)
    {
      Cursor = Cursors.WaitCursor;
      ApplyScript();
      if (ScriptResult != MySqlStatement.StatementResultType.ErrorThrown)
      {
        DialogResult = ScriptResult.WasApplied() ? DialogResult.OK : DialogResult.Cancel;
        Cursor = Cursors.Default;
        Close();
        return;
      }

      // Handle error messages thrown back by the server and show them to the user.
      if (_errorDetails == null)
      {
        _errorDetails = new StringBuilder();
      }
      else
      {
        _errorDetails.Clear();
      }

      foreach (var statement in ActualStatementRowsList.Select(row => row.Statement).Where(statement => statement.StatementResult == MySqlStatement.StatementResultType.ErrorThrown))
      {
        _errorDetails.Append(statement.ResultText);
        _errorDetails.AddNewLine();
      }

      Cursor = Cursors.Default;
      MiscUtilities.ShowCustomizedInfoDialog(InfoDialog.InfoType.Error, _errorDialogSummary, _errorDetails.ToString(), false);
    }

    /// <summary>
    /// Applies the SQL query by breaking it into stataments and executing one by one inside a transaction.
    /// </summary>
    public void ApplyScript()
    {
      ScriptResult = MySqlStatement.StatementResultType.NotApplied;
      CreateActualStatementsList();
      if (ActualStatementRowsList == null || ActualStatementRowsList.Count == 0)
      {
        return;
      }

      string connectionString = _wbConnection.GetConnectionStringBuilder().ConnectionString;
      using (MySqlConnection conn = new MySqlConnection(connectionString))
      {
        conn.Open();
        MySqlTransaction transaction = conn.BeginTransaction();
        uint executionOrder = 1;
        foreach (var mySqlRow in ActualStatementRowsList)
        {
          mySqlRow.Statement.Execute(transaction, executionOrder++, UseOptimisticUpdate);
          ScriptResult = mySqlRow.Statement.JoinResultTypes(ScriptResult);
          if (mySqlRow.Statement.StatementResult == MySqlStatement.StatementResultType.ErrorThrown)
          {
            mySqlRow.ReflectError();
            break;
          }
        }

        switch (ScriptResult)
        {
          case MySqlStatement.StatementResultType.ErrorThrown:
            transaction.Rollback();
            break;

          case MySqlStatement.StatementResultType.WarningsFound:
          case MySqlStatement.StatementResultType.Successful:
            // After commiting the transaction, selectively commit the rows that did not result in errors.
            transaction.Commit();
            foreach (var mySqlRow in ActualStatementRowsList.Where(mySqlRow => mySqlRow.Statement.StatementWasApplied))
            {
              if (mySqlRow.HasConcurrencyWarnings)
              {
                mySqlRow.ReflectError();
                continue;
              }

              mySqlRow.AcceptChanges();
              mySqlRow.ClearErrors();
            }
            break;
        }
      }
    }

    /// <summary>
    /// Creates a list of statements to apply to the database based on the script editable by the user.
    /// </summary>
    private void CreateActualStatementsList()
    {
      ActualStatementRowsList = null;
      if (string.IsNullOrEmpty(SqlScript))
      {
        return;
      }

      ActualStatementRowsList = new List<IMySqlDataRow>(OriginalStatementRowsList.Count);
      foreach (string sqlQuery in SqlScript.Split(';').Select(sqlStatement => sqlStatement.Trim()).Where(sqlQuery => sqlQuery.Length > 0))
      {
        var originalRow = OriginalStatementRowsList.FirstOrDefault(iMySqlRow => iMySqlRow.Statement.SqlQuery == sqlQuery && !ActualStatementRowsList.Contains(iMySqlRow));
        ActualStatementRowsList.Add(originalRow ?? new MySqlDummyRow(sqlQuery));
      }
    }

    /// <summary>
    /// Creates the list of original statements this script contains before the user makes any changes.
    /// </summary>
    private void CreateOriginalStatementsList()
    {
      if (OriginalStatementRowsList == null)
      {
        OriginalStatementRowsList = new List<IMySqlDataRow>();
      }

      if (_mySqlTable != null)
      {
        OriginalStatementRowsList.Clear();
        StringBuilder sqlScript = new StringBuilder(_mySqlTable.Rows.Count);

        // Create CREATE statement if the table is being exported to a new MySQL table.
        if (_mySqlTable.OperationType == MySqlDataTable.DataOperationType.Export)
        {
          var dummyRow = new MySqlDummyRow(_mySqlTable.GetCreateSql(true));
          OriginalStatementRowsList.Add(dummyRow);
          sqlScript.AppendFormat("{0};{1}", dummyRow.Statement.SqlQuery, Environment.NewLine);
        }

        // Create DELETE, INSERT and UPDATE statements for table rows
        if (_mySqlTable.OperationType != MySqlDataTable.DataOperationType.Export || !_mySqlTable.CreateTableWithoutData)
        {
          DataRowState[] rowStatesWithChanges = { DataRowState.Deleted, DataRowState.Added, DataRowState.Modified };
          foreach (MySqlDataRow mySqlRow in rowStatesWithChanges.SelectMany(rowState => _mySqlTable.Rows.Cast<MySqlDataRow>().Where(dr => !dr.IsHeadersRow && dr.RowState == rowState)))
          {
            OriginalStatementRowsList.Add(mySqlRow);
            sqlScript.AppendFormat("{0};{1}", mySqlRow.Statement.SqlQuery, Environment.NewLine);
          }
        }

        OriginalSqlScript = sqlScript.ToString();
      }
      else if (!string.IsNullOrEmpty(OriginalSqlScript) && OriginalStatementRowsList.Count == 0)
      {
        foreach (string sqlQuery in SqlScript.Split(';').Select(sqlStatement => sqlStatement.Trim()).Where(sqlQuery => sqlQuery.Length > 0))
        {
          OriginalStatementRowsList.Add(new MySqlDummyRow(sqlQuery));
        }
      }
    }

    /// <summary>
    /// Event delegate method fired when the <see cref="DialogCancelButton"/> is clicked.
    /// </summary>
    /// <param name="sender">Sender object.</param>
    /// <param name="e">Event arguments.</param>
    private void DialogCancelButton_Click(object sender, EventArgs e)
    {
      ActualStatementRowsList = null;
      ScriptResult = MySqlStatement.StatementResultType.NotApplied;
    }

    /// <summary>
    /// Event delegate method fired when the <see cref="OriginalQueryButton"/> is clicked.
    /// </summary>
    /// <param name="sender">Sender object.</param>
    /// <param name="e">Event arguments.</param>
    private void OriginalQueryButton_Click(object sender, EventArgs e)
    {
      if (InfoDialog.ShowYesNoDialog(InfoDialog.InfoType.Warning, Resources.ReviewSqlQueryRevertTitle, Resources.ReviewSqlQueryRevertDetail) != DialogResult.Yes)
      {
        return;
      }

      OriginalQueryButton.Enabled = false;
      SqlScript = OriginalSqlScript;
    }

    /// <summary>
    /// Event delegate method fired when the <see cref="QueryChangedTimer"/> text changes.
    /// </summary>
    /// <param name="sender">Sender object.</param>
    /// <param name="e">Event arguments.</param>
    private void QueryChangedTimer_Tick(object sender, EventArgs e)
    {
      if (QueryTextBox.Focused)
      {
        QueryTextBox_Validated(QueryTextBox, EventArgs.Empty);
      }
      else
      {
        // The code should never hit this block in which case there is something wrong.
        MySqlSourceTrace.WriteToLog("QueryChangedTimer's Tick event fired but no valid control had focus.");
        QueryChangedTimer.Stop();
      }
    }

    /// <summary>
    /// Event delegate method fired when the <see cref="QueryTextBox"/> text changes.
    /// </summary>
    /// <param name="sender">Sender object.</param>
    /// <param name="e">Event arguments.</param>
    private void QueryTextBox_TextChanged(object sender, EventArgs e)
    {
      if (!_isUserInput)
      {
        return;
      }

      ResetQueryChangedTimer();
    }

    /// <summary>
    /// Event delegate method fired when the <see cref="QueryTextBox"/> has been validated.
    /// </summary>
    /// <param name="sender">Sender object.</param>
    /// <param name="e">Event arguments.</param>
    private void QueryTextBox_Validated(object sender, EventArgs e)
    {
      QueryChangedTimer.Stop();

      // Identify the statements that would exceed the server's max allowed packet value and highlight them for the user.
      string queryText = QueryTextBox.Text.Trim();
      if (queryText.Length <= 0)
      {
        return;
      }

      QueryTextBox.ReadOnly = true;
      Cursor = Cursors.WaitCursor;

      bool statementsExceedingMaxAllowedPacketValueFound = false;
      bool reachedEnd = false;
      int statementStartPosition = 0;
      do
      {
        int statementEndPosition = queryText.IndexOf(";", statementStartPosition, StringComparison.Ordinal);
        if (statementEndPosition < 0)
        {
          reachedEnd = true;
          statementEndPosition = queryText.Length - 1;
        }

        // Get SQL statement
        string sqlStatement = queryText.Substring(statementStartPosition, statementEndPosition - statementStartPosition).Trim();

        // TODO: Split statement in tokens using MySQL parser classes and paint them accordingly.

        // Highlight the statement if it exceeds the MySQL Servers's max allowed packet value.
        if (sqlStatement.ExceedsMySqlMaxAllowedPacketValue(MySqlMaxAllowedPacket))
        {
          QueryTextBox.Select(statementStartPosition, statementEndPosition - statementStartPosition);
          QueryTextBox.SelectionBackColor = Color.GreenYellow;
          statementsExceedingMaxAllowedPacketValueFound = true;
        }

        statementStartPosition = statementEndPosition + 1;
        reachedEnd = reachedEnd || statementStartPosition >= queryText.Length;
      }
      while (!reachedEnd);

      QueryWarningPictureBox.Visible = statementsExceedingMaxAllowedPacketValueFound;
      QueryWarningLabel.Visible = statementsExceedingMaxAllowedPacketValueFound;

      QueryTextBox.ReadOnly = false;
      OriginalQueryButton.Enabled = !string.Equals(OriginalSqlScript, SqlScript, StringComparison.InvariantCultureIgnoreCase);
      ApplyButton.Enabled = SqlScript.Trim().Length > 0;
      Cursor = Cursors.Default;
    }

    /// <summary>
    /// Resets the timer used on query text changes only if there was a user input.
    /// </summary>
    private void ResetQueryChangedTimer()
    {
      QueryChangedTimer.Stop();
      QueryChangedTimer.Start();
    }

    /// <summary>
    /// Resets the <see cref="QueryTextBox"/> text zooming factor.
    /// </summary>
    private void ResetTextZoom()
    {
      QueryTextBox.ZoomFactor = 1;
      ZoomResetToolStripMenuItem.Visible = false;
      ZoomInToolStripMenuItem.Enabled = true;
      ZoomOutToolStripMenuItem.Enabled = true;
    }

    /// <summary>
    /// Sets up the visibility and arrangement of the label showing original operations information reflected on the SQL script.
    /// </summary>
    private void SetOriginalOperationsInfoAvailability()
    {
      SetOriginalOperationsInfoText();
      OriginalOperationsLabel.Visible = ShowOriginalOperationsInformation;
      QueryTextBox.Anchor = AnchorStyles.None;
      QueryTextBox.Location = ShowOriginalOperationsInformation
        ? new Point(QueryTextBox.Location.X, DEFAULT_Y_LOCATION_QUERY_TEXTBOX_WITH_INFO)
        : new Point(QueryTextBox.Location.X, DEFAULT_Y_LOCATION_QUERY_TEXTBOX_NO_INFO);
      QueryTextBox.Height = ShowOriginalOperationsInformation
        ? DEFAULT_HEIGHT_QUERY_TEXTBOX_WITH_INFO
        : DEFAULT_HEIGHT_QUERY_TEXTBOX_NO_INFO;
      QueryTextBox.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Bottom | AnchorStyles.Right;
    }

    /// <summary>
    /// Sets up the label text showing original operations information reflected on the SQL script.
    /// </summary>
    private void SetOriginalOperationsInfoText()
    {
      if (ShowOriginalOperationsInformation)
      {
        StringBuilder originalOperationsInfo = new StringBuilder(3);
        if (_mySqlTable.OperationType == MySqlDataTable.DataOperationType.Export)
        {
          originalOperationsInfo.AppendFormat(Resources.ScriptCreatingTableText, _mySqlTable.TableNameForSqlQueries);
        }

        if (_mySqlTable.OperationType != MySqlDataTable.DataOperationType.Export || !_mySqlTable.CreateTableWithoutData)
        {
          int operationRows = _mySqlTable.DeletingOperations;
          int totalOperationRows = operationRows;
          if (operationRows > 0)
          {
            originalOperationsInfo.AddSeparator(", ", true);
            originalOperationsInfo.AppendFormat(Resources.ScriptDeletingRowsText, operationRows);
          }

          operationRows = _mySqlTable.InsertingOperations;
          totalOperationRows += operationRows;
          if (operationRows > 0)
          {
            originalOperationsInfo.AddSeparator(", ", true);
            originalOperationsInfo.AppendFormat(Resources.ScriptInsertingRowsText, operationRows);
          }

          operationRows = _mySqlTable.UpdatingOperations;
          totalOperationRows += operationRows;
          if (operationRows > 0)
          {
            originalOperationsInfo.AddSeparator(", ", true);
            originalOperationsInfo.AppendFormat(Resources.ScriptUpdatingRowsText, operationRows);
          }

          if (totalOperationRows > 0)
          {
            originalOperationsInfo.AddSeparator(" ", true);
            originalOperationsInfo.Append(Resources.ScriptRowsText);
          }
        }

        if (originalOperationsInfo.Length > 0)
        {
          originalOperationsInfo.Append(Resources.ScriptWithSqlStatementsText);
        }

        OriginalOperationsLabel.Text = originalOperationsInfo.ToString();
      }
    }

    /// <summary>
    /// Event delegate method fired when the <see cref="ZoomInToolStripMenuItem"/> is clicked.
    /// </summary>
    /// <param name="sender">Sender object.</param>
    /// <param name="e">Event arguments.</param>
    private void ZoomInToolStripMenuItem_Click(object sender, EventArgs e)
    {
      float newValue = QueryTextBox.ZoomFactor * ZOOMING_STEP;
      if (newValue.CompareTo(64) >= 0)
      {
        ZoomInToolStripMenuItem.Visible = false;
        return;
      }

      ZoomResetToolStripMenuItem.Visible = true;
      ZoomOutToolStripMenuItem.Visible = true;
      QueryTextBox.ZoomFactor = newValue;
    }

    /// <summary>
    /// Event delegate method fired when the <see cref="ZoomInToolStripMenuItem"/> is clicked.
    /// </summary>
    /// <param name="sender">Sender object.</param>
    /// <param name="e">Event arguments.</param>
    private void ZoomOutToolStripMenuItem_Click(object sender, EventArgs e)
    {
      float newValue = QueryTextBox.ZoomFactor / ZOOMING_STEP;
      if (newValue.CompareTo(1 / 64) <= 0)
      {
        ZoomOutToolStripMenuItem.Visible = false;
        return;
      }

      ZoomResetToolStripMenuItem.Visible = true;
      ZoomInToolStripMenuItem.Visible = true;
      QueryTextBox.ZoomFactor = newValue;
    }

    /// <summary>
    /// Event delegate method fired when the <see cref="ZoomInToolStripMenuItem"/> is clicked.
    /// </summary>
    /// <param name="sender">Sender object.</param>
    /// <param name="e">Event arguments.</param>
    private void ZoomResetToolStripMenuItem_Click(object sender, EventArgs e)
    {
      ResetTextZoom();
    }
  }
}
