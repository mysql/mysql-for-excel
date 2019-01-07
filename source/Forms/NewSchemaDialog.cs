// Copyright (c) 2012, 2018, Oracle and/or its affiliates. All rights reserved.
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
using System.Linq;
using System.Text;
using System.Windows.Forms;
using MySQL.ForExcel.Classes;
using MySQL.ForExcel.Interfaces;
using MySQL.ForExcel.Properties;
using MySql.Utility.Classes.MySqlWorkbench;
using MySql.Utility.Forms;

namespace MySQL.ForExcel.Forms
{
  /// <summary>
  /// Lets users create a new schema in the connected MySQL Server instance.
  /// </summary>
  public partial class NewSchemaDialog : AutoStyleableBaseDialog
  {
    #region Fields

    /// <summary>
    /// The connection to a MySQL server instance selected by users.
    /// /// </summary>
    private MySqlWorkbenchConnection _wbConnection;

    #endregion Fields

    /// <summary>
    /// Initializes a new instance of the <see cref="NewSchemaDialog"/> class.
    /// </summary>
    public NewSchemaDialog(MySqlWorkbenchConnection wbConnection)
    {
      _wbConnection = wbConnection ?? throw new ArgumentNullException(nameof(wbConnection));
      InitializeComponent();
      SchemaName = _wbConnection.GetSchemaNameAvoidingDuplicates(null);
      CollationComboBox.SetupCollations(_wbConnection, "Server Default");
    }

    #region Properties

    /// <summary>
    /// Gets the character set for the collation selected by the user in the <see cref="CollationComboBox"/>;
    /// </summary>
    private string CharSet => CollationComboBox.SelectedItem is KeyValuePair<string, string[]>
      ? ((KeyValuePair<string, string[]>)CollationComboBox.SelectedItem).Value[0]
      : string.Empty;

    /// <summary>
    /// Gets the collation selected by the user in the <see cref="CollationComboBox"/>;
    /// </summary>
    private string Collation => CollationComboBox.SelectedItem is KeyValuePair<string, string[]>
      ? ((KeyValuePair<string, string[]>)CollationComboBox.SelectedItem).Value[1]
      : string.Empty;

    /// <summary>
    /// Gets or sets the name of the new schema.
    /// </summary>
    private string SchemaName
    {
      get => SchemaNameTextBox.Text.Trim();
      set => SchemaNameTextBox.Text = value;
    }

    #endregion Properties

    /// <summary>
    /// Creates a new schema with the given name.
    /// </summary>
    /// <returns><c>true</c> if the schema was created successfully, <c>false</c> otherwise.</returns>
    private bool CreateSchema()
    {
      var passwordFlags = _wbConnection.TestConnectionAndRetryOnWrongPassword();
      if (!passwordFlags.ConnectionSuccess)
      {
        return false;
      }

      Cursor = Cursors.WaitCursor;
      var createSql = _wbConnection.GetCreateSchemaSql(SchemaName, CharSet, Collation, false);
      var operationInfoText = string.Format(Resources.ScriptCreatingSchemaText, SchemaName);
      List<IMySqlDataRow> results;
      using (var sqlScriptDialog = new MySqlScriptDialog(_wbConnection, createSql, operationInfoText))
      {
        if (Settings.Default.GlobalSqlQueriesPreviewQueries)
        {
          sqlScriptDialog.ShowDialog();
        }
        else
        {
          sqlScriptDialog.ApplyScript();
        }

        var erroredOutRow = sqlScriptDialog.ErroredOutDataRow;
        results = sqlScriptDialog.ScriptResult == MySqlStatement.StatementResultType.ErrorThrown
          ? erroredOutRow != null ? new List<IMySqlDataRow>(1) { erroredOutRow } : null
          : sqlScriptDialog.ActualStatementRowsList;
      }

      if (results == null)
      {
        Cursor = Cursors.Default;
        return false;
      }

      string operationSummary;
      var success = true;
      var warningsFound = false;
      var operationDetails = new StringBuilder();
      var warningStatementDetails = new StringBuilder();
      foreach (var statement in results.Select(statementRow => statementRow.Statement))
      {
        // Create details text for the schema creation.
        switch (statement.StatementType)
        {
          case MySqlStatement.SqlStatementType.CreateSchema:
            break;

          case MySqlStatement.SqlStatementType.GrantAll:
            break;
        }

        if (Settings.Default.GlobalSqlQueriesShowQueriesWithResults)
        {
          operationDetails.AppendFormat(Resources.NewSchemaExecutedQuery, SchemaName);
          operationDetails.AddNewLine(2);
          operationDetails.Append(statement.SqlQuery);
          operationDetails.AddNewLine(2);
        }

        switch (statement.StatementResult)
        {
          case MySqlStatement.StatementResultType.Successful:
            operationDetails.AppendFormat(Resources.NewSchemaCreatedSuccessfullyText, SchemaName);
            break;

          case MySqlStatement.StatementResultType.WarningsFound:
            warningsFound = true;
            operationDetails.AppendFormat(Resources.NewSchemaCreatedWithWarningsText, SchemaName, statement.WarningsQuantity);
            operationDetails.AddNewLine();
            operationDetails.Append(statement.ResultText);
            break;

          case MySqlStatement.StatementResultType.ErrorThrown:
            success = false;
            operationDetails.AppendFormat(Resources.NewSchemaCreationErrorText, SchemaName);
            operationDetails.AddNewLine();
            operationDetails.Append(statement.ResultText);
            break;
        }
      }

      InfoDialog.InfoType operationsType;
      if (success)
      {
        operationSummary = string.Format(Resources.NewSchemaOperationSuccessSummaryText, SchemaName);
        operationsType = warningsFound ? InfoDialog.InfoType.Warning : InfoDialog.InfoType.Success;
      }
      else
      {
        operationSummary = string.Format(Resources.NewSchemaOperationErrorSummaryText, SchemaName);
        operationsType = InfoDialog.InfoType.Error;
      }

      Cursor = Cursors.Default;
      MiscUtilities.ShowCustomizedInfoDialog(operationsType, operationSummary, operationDetails.ToString(), false);
      operationDetails.Clear();
      warningStatementDetails.Clear();
      return success;
    }

    /// <summary>
    /// Event delegate method fired when the <see cref="NewSchemaDialog"/> is being closed.
    /// </summary>
    /// <param name="sender">Sender object.</param>
    /// <param name="e">Event arguments.</param>
    private void NewSchemaDialog_FormClosing(object sender, FormClosingEventArgs e)
    {
      if (DialogResult == DialogResult.OK)
      {
        e.Cancel = !CreateSchema();
      }
    }

    /// <summary>
    /// Event delegate method fired when the <see cref="SchemaNameTextBox"/> text changes.
    /// </summary>
    /// <param name="sender">Sender object.</param>
    /// <param name="e">Event arguments.</param>
    private void SchemaNameTextBox_TextChanged(object sender, EventArgs e)
    {
      DialogOKButton.Enabled = SchemaName.Length > 0;
    }
  }
}