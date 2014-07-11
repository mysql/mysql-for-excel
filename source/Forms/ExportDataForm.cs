// Copyright (c) 2012-2014, Oracle and/or its affiliates. All rights reserved.
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
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using MySQL.ForExcel.Classes;
using MySQL.ForExcel.Properties;
using MySQL.Utility.Classes.MySQLWorkbench;
using MySQL.Utility.Forms;
using Excel = Microsoft.Office.Interop.Excel;

namespace MySQL.ForExcel.Forms
{
  /// <summary>
  /// Presents users with a wizard-like form to export selected Excel data to a new MySQL table.
  /// </summary>
  public partial class ExportDataForm : AutoStyleableBaseDialog
  {
    #region Fields

    /// <summary>
    /// The Excel cells range containing the data being exported to a new MySQL table.
    /// </summary>
    private Excel.Range _exportDataRange;

    /// <summary>
    /// A <see cref="MySqlDataTable"/> object containing the all data to be exported to a new MySQL table.
    /// </summary>
    private MySqlDataTable _exportDataTable;

    /// <summary>
    /// Flag indicating whether when text changes on an input control was due user input or programatic.
    /// </summary>
    private bool _isUserInput;

    /// <summary>
    /// A <see cref="MySqlDataTable"/> object containing a subset of the whole data which is shown in the preview grid.
    /// </summary>
    private MySqlDataTable _previewDataTable;

    /// <summary>
    /// The proposed table name
    /// </summary>
    private readonly string _proposedTableName;

    /// <summary>
    /// The connection to a MySQL server instance selected by users.
    /// </summary>
    private MySqlWorkbenchConnection _wbConnection;

    #endregion Fields

    /// <summary>
    /// Initializes a new instance of the <see cref="ExportDataForm"/> class.
    /// </summary>
    /// <param name="wbConnection">MySQL Workbench connection to a MySQL server instance selected by users.</param>
    /// <param name="exportDataRange">Excel cells range containing the data being exported to a new MySQL table.</param>
    /// <param name="exportingWorksheetName">Name of the Excel worksheet containing the data to export.</param>
    public ExportDataForm(MySqlWorkbenchConnection wbConnection, Excel.Range exportDataRange, string exportingWorksheetName)
    {
      _isUserInput = true;
      _wbConnection = wbConnection;
      _exportDataRange = exportDataRange;

      InitializeComponent();
      if (!exportingWorksheetName.ToLowerInvariant().StartsWith("sheet"))
      {
        _proposedTableName = exportingWorksheetName.ToLower().Replace(' ', '_');
      }

      Text = string.Format("Export Data - {0} [{1}]", exportingWorksheetName, _exportDataRange.Address.Replace("$", string.Empty));
      LoadPreviewData();
      InitializeDataTypeCombo();
      CollationComboBox.SetupCollations(_wbConnection, "Schema Default");
      FirstRowHeadersCheckBox_CheckedChanged(FirstRowHeadersCheckBox, EventArgs.Empty);
      SetDefaultPrimaryKey();

      if (!string.IsNullOrEmpty(_proposedTableName))
      {
        SetControlTextValue(TableNameInputTextBox, _proposedTableName);
      }

      PreviewTableWarningsChanged(_previewDataTable, new TableWarningsChangedArgs(_previewDataTable, false));

      TableNameInputTextBox.Focus();
      TableNameInputTextBox.SelectAll();
    }

    #region Properties

    /// <summary>
    /// Gets or sets the text associated with this control.
    /// </summary>
    public override sealed string Text
    {
      get
      {
        return base.Text;
      }

      set
      {
        base.Text = value;
      }
    }

    #endregion Properties

    /// <summary>
    /// Event delegate method fired when the <see cref="AddPrimaryKeyRadioButton"/> radio button checked state changes.
    /// </summary>
    /// <param name="sender">Sender object.</param>
    /// <param name="e">Event arguments.</param>
    private void AddPrimaryKeyRadioButton_CheckedChanged(object sender, EventArgs e)
    {
      if (!AddPrimaryKeyRadioButton.Checked)
      {
        return;
      }

      PreviewDataGridView.Columns[0].Visible = true;
      PreviewDataGridView.Columns[0].Selected = true;
      PreviewDataGridView.FirstDisplayedScrollingColumnIndex = 0;
      PrimaryKeyColumnsComboBox.Text = string.Empty;
      PrimaryKeyColumnsComboBox.SelectedIndex = -1;
      PrimaryKeyColumnsComboBox.Enabled = false;
      AddPrimaryKeyTextBox.Enabled = true;
      _previewDataTable.UseFirstColumnAsPk = true;
    }

    /// <summary>
    /// Event delegate method fired when the <see cref="AddPrimaryKeyTextBox"/> textbox's text changes.
    /// </summary>
    /// <param name="sender">Sender object.</param>
    /// <param name="e">Event arguments.</param>
    private void AddPrimaryKeyTextBox_TextChanged(object sender, EventArgs e)
    {
      ResetTextChangedTimer();
    }

    /// <summary>
    /// Event delegate method fired when the <see cref="AddPrimaryKeyTextBox"/> textbox is being validated.
    /// </summary>
    /// <param name="sender">Sender object.</param>
    /// <param name="e">Event arguments.</param>
    private void AddPrimaryKeyTextBox_Validating(object sender, CancelEventArgs e)
    {
      TextChangedTimer.Stop();
      string newAutoPkName = AddPrimaryKeyTextBox.Text.Trim();
      MySqlDataColumn pkColumn = _previewDataTable.GetColumnAtIndex(0);
      if (pkColumn.DisplayName == newAutoPkName && PreviewDataGridView.Columns[0].HeaderText == newAutoPkName)
      {
        return;
      }

      pkColumn.SetDisplayName(AddPrimaryKeyTextBox.Text);
      PreviewDataGridView.Columns[0].HeaderText = pkColumn.DisplayName;
      MySqlDataColumn currentColumn = GetCurrentMySqlDataColumn();
      if (currentColumn != null && currentColumn.Ordinal == 0)
      {
        SetControlTextValue(ColumnNameTextBox, currentColumn.DisplayName);
      }
    }

    /// <summary>
    /// Event delegate method fired when the <see cref="AdvancedOptionsButton"/> button is clicked.
    /// </summary>
    /// <param name="sender">Sender object.</param>
    /// <param name="e">Event arguments.</param>
    private void AdvancedOptionsButton_Click(object sender, EventArgs e)
    {
      using (var optionsDialog = new ExportAdvancedOptionsDialog())
      {
        optionsDialog.ShowDialog();
        if (optionsDialog.ExportShowAllMySqlDataTypesChanged)
        {
          InitializeDataTypeCombo();
        }

        if (!optionsDialog.ParentFormRequiresRefresh)
        {
          return;
        }

        LoadPreviewData();
        if (optionsDialog.ExportDetectDatatypeChanged && Settings.Default.ExportDetectDatatype)
        {
          // Reset background colors to default since those aren't reset when the condition above is fulfilled.
          foreach (var mysqldc in _previewDataTable.Columns.Cast<MySqlDataColumn>().Where(mysqldc => mysqldc != null))
          {
            PreviewTableWarningsChanged(mysqldc, new TableWarningsChangedArgs(mysqldc));
          }
        }

        // Update table properties with user properties
        _previewDataTable.TableName = TableNameInputTextBox.Text.Trim();
        _previewDataTable.UseFirstColumnAsPk = AddPrimaryKeyRadioButton.Checked;
        _previewDataTable.FirstRowContainsColumnNames = FirstRowHeadersCheckBox.Checked;

        // Force Empty columns with emtpy column names from being stated defaulty when this is not desired.
        RecreateColumns();
        SetDefaultPrimaryKey();

        // Refresh first row headers accordingly
        PreviewDataGridView.CurrentCell = null;
        PreviewDataGridView.Rows[0].Visible = !FirstRowHeadersCheckBox.Checked;
      }
    }

    /// <summary>
    /// Event delegate method fired when the <see cref="AllowEmptyCheckBox"/> object's checked state changes.
    /// </summary>
    /// <param name="sender">Sender object.</param>
    /// <param name="ea">Event arguments.</param>
    private void AllowEmptyCheckBox_CheckedChanged(object sender, EventArgs ea)
    {
      MySqlDataColumn currentCol = GetCurrentMySqlDataColumn();
      if (currentCol == null || AllowEmptyCheckBox.Checked == currentCol.AllowNull)
      {
        return;
      }

      currentCol.AllowNull = AllowEmptyCheckBox.Checked;
      RefreshColumnControlsEnabledStatus(false);
    }

    /// <summary>
    /// Event delegate method fired when the <see cref="ColumnNameTextBox"/> textbox's text changes.
    /// </summary>
    /// <param name="sender">Sender object.</param>
    /// <param name="e">Event arguments.</param>
    private void ColumnNameTextBox_TextChanged(object sender, EventArgs e)
    {
      ResetTextChangedTimer();
    }

    /// <summary>
    /// Event delegate method fired when the <see cref="ColumnNameTextBox"/> textbox is being validated.
    /// </summary>
    /// <param name="sender">Sender object.</param>
    /// <param name="e">Event arguments.</param>
    private void ColumnNameTextBox_Validating(object sender, CancelEventArgs e)
    {
      TextChangedTimer.Stop();
      string newColumnName = ColumnNameTextBox.Text.Trim();
      MySqlDataColumn column = GetCurrentMySqlDataColumn();
      if (column == null || column.DisplayName == newColumnName)
      {
        return;
      }

      column.SetDisplayName(newColumnName, true);
      PreviewDataGridView.Columns[column.Ordinal].HeaderText = column.DisplayName;
      SetControlTextValue(AddPrimaryKeyTextBox, column.DisplayName);
      if (ColumnNameTextBox.Text != column.DisplayName)
      {
        SetControlTextValue(ColumnNameTextBox, column.DisplayName);
      }

      if (PrimaryKeyColumnsComboBox.Items.Count <= 0)
      {
        return;
      }

      // Update the columnIndex for the cmbPrimaryKeyColumns combo box since it does not include Excluded columns
      int comboColumnIndex = -1;
      for (int i = 1; i < _previewDataTable.Columns.Count && i != column.Ordinal; i++)
      {
        column = _previewDataTable.GetColumnAtIndex(i);
        if (!column.ExcludeColumn)
        {
          comboColumnIndex++;
        }
      }

      if (comboColumnIndex < 0)
      {
        return;
      }

      PrimaryKeyColumnsComboBox.BeginUpdate();
      PrimaryKeyColumnsComboBox.Items[comboColumnIndex] = column.DisplayName;
      PrimaryKeyColumnsComboBox.EndUpdate();
    }

    /// <summary>
    /// Event delegate method fired when the <see cref="CreateIndexCheckBox"/> object's checked state changes.
    /// </summary>
    /// <param name="sender">Sender object.</param>
    /// <param name="ea">Event arguments.</param>
    private void CreateIndexCheckBox_CheckedChanged(object sender, EventArgs ea)
    {
      if (!_isUserInput)
      {
        return;
      }

      MySqlDataColumn currentCol = GetCurrentMySqlDataColumn();
      if (currentCol == null || CreateIndexCheckBox.Checked == currentCol.CreateIndex)
      {
        return;
      }

      currentCol.CreateIndex = CreateIndexCheckBox.Checked;
      RefreshColumnControlsEnabledStatus(false);
    }

    /// <summary>
    /// Event delegate method fired when the <see cref="CreateTableToolStripMenuItem"/> context menu item is clicked.
    /// </summary>
    /// <param name="sender">Sender object.</param>
    /// <param name="e">Event arguments.</param>
    private void CreateTableToolStripMenuItem_Click(object sender, EventArgs e)
    {
      ExportButton.Text = Resources.CreateTableText;
      ExportDataToolStripMenuItem.Checked = false;
      CreateTableToolStripMenuItem.Checked = true;
    }

    /// <summary>
    /// Event delegate method fired when the <see cref="DataTypeComboBox"/> combo box's selected index changes.
    /// </summary>
    /// <param name="sender">Sender object.</param>
    /// <param name="e">Event arguments.</param>
    private void DataTypeComboBox_SelectedIndexChanged(object sender, EventArgs e)
    {
      MySqlDataColumn currentCol = GetCurrentMySqlDataColumn();
      var selectedType = DataTypeComboBox.SelectedValue != null ? DataTypeComboBox.SelectedValue.ToString() : string.Empty;
      if (currentCol == null || string.IsNullOrEmpty(selectedType) || string.Equals(selectedType, currentCol.MySqlDataType, StringComparison.InvariantCultureIgnoreCase))
      {
        return;
      }

      // Fill in sets and enums
      switch (selectedType)
      {
        case "Enum":
          currentCol.SetCollectionDataType(MySqlDataColumn.CollectionDataType.Enum);
          BeginInvoke(new Action(() => DataTypeComboBox.Text = currentCol.MySqlDataType));
          break;

        case "Set":
          currentCol.SetCollectionDataType(MySqlDataColumn.CollectionDataType.Set);
          BeginInvoke(new Action(() => DataTypeComboBox.Text = currentCol.MySqlDataType));
          break;

        default:
          currentCol.SetMySqlDataType(selectedType, false, true);
          break;
      }
    }

    /// <summary>
    /// Event delegate method fired when the <see cref="DataTypeComboBox"/> combo's text changes.
    /// </summary>
    /// <param name="sender">Sender object.</param>
    /// <param name="e">Event arguments.</param>
    private void DataTypeComboBox_TextChanged(object sender, EventArgs e)
    {
      ResetTextChangedTimer();
    }

    /// <summary>
    /// Event delegate method fired when the <see cref="DataTypeComboBox"/> combo box is validating.
    /// </summary>
    /// <param name="sender">Sender object.</param>
    /// <param name="e">Event arguments.</param>
    private void DataTypeComboBox_Validating(object sender, CancelEventArgs e)
    {
      TextChangedTimer.Stop();
      string newDataType = DataTypeComboBox.Text.Trim();
      MySqlDataColumn currentCol = GetCurrentMySqlDataColumn();
      if (currentCol == null || DataTypeComboBox.SelectedIndex >= 0 || currentCol.MySqlDataType == newDataType)
      {
        return;
      }

      currentCol.SetMySqlDataType(newDataType, true, true);
    }

    /// <summary>
    /// Event delegate method fired when the <see cref="DataTypeComboBox"/> combo box's draws each internal item.
    /// </summary>
    /// <param name="sender">Sender object.</param>
    /// <param name="e">Event arguments.</param>
    private void DataTypeComboBoxDrawItem(object sender, DrawItemEventArgs e)
    {
      e.DrawBackground();
      var comboItem = DataTypeComboBox.Items[e.Index];
      string itemText = comboItem is KeyValuePair<string, string>
        ? ((KeyValuePair<string, string>)comboItem).Value
        : comboItem.ToString();
      e.Graphics.DrawString(itemText, DataTypeComboBox.Font, Brushes.Black, new RectangleF(e.Bounds.X, e.Bounds.Y, e.Bounds.Width, e.Bounds.Height));
      e.DrawFocusRectangle();
    }

    /// <summary>
    /// Event delegate method fired when the <see cref="DropDownButton"/> button is clicked.
    /// </summary>
    /// <param name="sender">Sender object.</param>
    /// <param name="e">Event arguments.</param>
    private void DropDownButton_Click(object sender, EventArgs e)
    {
      ExportContextMenuStrip.Show(ExportButton, new Point(0, ExportButton.Height));
    }

    /// <summary>
    /// Event delegate method fired when the <see cref="ExcludeColumnCheckBox"/> object's checked state changes.
    /// </summary>
    /// <param name="sender">Sender object.</param>
    /// <param name="ea">Event arguments.</param>
    private void ExcludeCheckBox_CheckedChanged(object sender, EventArgs ea)
    {
      if (!_isUserInput)
      {
        return;
      }

      MySqlDataColumn currentCol = GetCurrentMySqlDataColumn();
      if (currentCol == null || ExcludeColumnCheckBox.Checked == currentCol.ExcludeColumn)
      {
        return;
      }

      currentCol.ExcludeColumn = ExcludeColumnCheckBox.Checked;
      RefreshColumnWarnings(currentCol);
      RefreshColumnControlsEnabledStatus(true);
      RefreshPrimaryKeyColumnsCombo(false);
    }

    /// <summary>
    /// Exports the selected Excel data to a new MySQL table.
    /// </summary>
    /// <returns><c>true</c> if the export is successful, <c>false</c> otherwise.</returns>
    private bool ExportData()
    {
      Cursor = Cursors.WaitCursor;

      bool setupDataSuccessful = true;
      if (_exportDataTable == null)
      {
        _exportDataTable = _previewDataTable.CloneSchema(false, true);
        _exportDataTable.DetectDatatype = false;
        _exportDataTable.IsPreviewTable = false;
        setupDataSuccessful = _exportDataTable.SetupColumnsWithData(_exportDataRange, false, true);
      }
      else
      {
        _exportDataTable.SyncSchema(_previewDataTable);
      }

      if (CollationComboBox.SelectedItem is KeyValuePair<string, string[]>)
      {
        var collationEntry = (KeyValuePair<string, string[]>)CollationComboBox.SelectedItem;
        _exportDataTable.CharSet = collationEntry.Value[0];
        _exportDataTable.Collation = collationEntry.Value[1];
      }
      else
      {
        _exportDataTable.CharSet = string.Empty;
        _exportDataTable.Collation = string.Empty;
      }

      Cursor = Cursors.Default;
      if (!setupDataSuccessful)
      {
        return false;
      }

      // Check if there is data to export, if there is not then ask the user if he wants to proceed with the table creation only.
      bool tableContainsDataToExport = _exportDataTable.Rows.Count > (_exportDataTable.FirstRowContainsColumnNames ? 1 : 0);
      if (!tableContainsDataToExport)
      {
        var dr = MiscUtilities.ShowCustomizedWarningDialog(Resources.ExportDataNoDataToExportTitleWarning, Resources.ExportDataNoDataToExportDetailWarning);
        if (dr == DialogResult.No)
        {
          return false;
        }

        _exportDataTable.CreateTableWithoutData = true;
      }
      else
      {
        _exportDataTable.CreateTableWithoutData = CreateTableToolStripMenuItem.Checked;
      }

      Cursor = Cursors.WaitCursor;
      int warningsCount = 0;
      bool success = true;
      bool warningsFound = false;
      bool tableCreated = true;
      string operationSummary;
      var modifiedRowsList = _exportDataTable.PushData(Settings.Default.GlobalSqlQueriesPreviewQueries);
      if (modifiedRowsList == null)
      {
        Cursor = Cursors.Default;
        return false;
      }

      bool warningDetailHeaderAppended = false;
      string statementsQuantityFormat = new string('0', modifiedRowsList.Count.StringSize());
      string sqlQueriesFormat = "{0:" + statementsQuantityFormat + "}: {1}";
      var operationDetails = new StringBuilder();
      var warningDetails = new StringBuilder();
      var warningStatementDetails = new StringBuilder();
      foreach (var statement in modifiedRowsList.Select(statementRow => statementRow.Statement))
      {
        // Create details text for the table creation.
        if (statement.StatementType == MySqlStatement.SqlStatementType.CreateTable)
        {
          if (Settings.Default.GlobalSqlQueriesShowQueriesWithResults)
          {
            operationDetails.AppendFormat(Resources.ExportDataTableExecutedQuery, _exportDataTable.TableName);
            operationDetails.AddNewLine(2);
            operationDetails.Append(statement.SqlQuery);
            operationDetails.AddNewLine(2);
          }

          switch (statement.StatementResult)
          {
            case MySqlStatement.StatementResultType.Successful:
              operationDetails.AppendFormat(Resources.ExportDataTableCreatedSuccessfullyText, _exportDataTable.TableName);
              break;

            case MySqlStatement.StatementResultType.WarningsFound:
              warningsFound = true;
              operationDetails.AppendFormat(Resources.ExportDataTableCreatedWithWarningsText, _exportDataTable.TableName, statement.WarningsQuantity);
              operationDetails.AddNewLine();
              operationDetails.Append(statement.ResultText);
              break;

            case MySqlStatement.StatementResultType.ErrorThrown:
              success = false;
              tableCreated = false;
              operationDetails.AppendFormat(Resources.ExportDataErrorCreatingTableText, _exportDataTable.TableName);
              operationDetails.AddNewLine();
              operationDetails.Append(statement.ResultText);
              break;
          }

          // If we are only creating the table without data then do not process other entries.
          if (_exportDataTable.CreateTableWithoutData)
          {
            break;
          }

          operationDetails.AddNewLine(2, true);

          // Create a title entry for the rows to be inserted if the creation was successful
          if (Settings.Default.GlobalSqlQueriesShowQueriesWithResults && success)
          {
            operationDetails.AppendFormat(Resources.InsertedExcelDataWithQueryText, _exportDataTable.TableName);
            operationDetails.AddNewLine();
          }

          continue;
        }

        // Create details text each row inserted in the new table.
        if (Settings.Default.GlobalSqlQueriesShowQueriesWithResults && statement.SqlQuery.Length > 0)
        {
          operationDetails.AddNewLine(1, true);
          operationDetails.AppendFormat(sqlQueriesFormat, statement.ExecutionOrder - 1, statement.SqlQuery);
        }

        switch (statement.StatementResult)
        {
          case MySqlStatement.StatementResultType.WarningsFound:
            if (Settings.Default.GlobalSqlQueriesPreviewQueries)
            {
              if (!warningDetailHeaderAppended)
              {
                warningDetailHeaderAppended = true;
                warningStatementDetails.AddNewLine(1, true);
                warningStatementDetails.Append(Resources.SqlStatementsProducingWarningsText);
              }

              if (statement.SqlQuery.Length > 0)
              {
                warningStatementDetails.AddNewLine(1, true);
                warningStatementDetails.AppendFormat(sqlQueriesFormat, statement.ExecutionOrder, statement.SqlQuery);
              }
            }

            warningsFound = true;
            warningDetails.AddNewLine(1, true);
            warningDetails.Append(statement.ResultText);
            warningsCount += statement.WarningsQuantity;
            break;

          case MySqlStatement.StatementResultType.ErrorThrown:
            success = false;
            operationDetails.AddNewLine(2, true);
            operationDetails.Append(Resources.ExportDataRowsInsertionErrorText);
            operationDetails.AddNewLine();
            operationDetails.Append(statement.ResultText);
            break;
        }

        if (success)
        {
          continue;
        }

        break;
      }

      InfoDialog.InfoType operationsType;
      if (success)
      {
        operationSummary = string.Format(_exportDataTable.CreateTableWithoutData ? Resources.ExportDataOperationSuccessNoDataText : Resources.ExportDataOperationSuccessWithDataText, _exportDataTable.TableName);
        operationsType = warningsFound ? InfoDialog.InfoType.Warning : InfoDialog.InfoType.Success;
        if (!_exportDataTable.CreateTableWithoutData)
        {
          int insertedCount = modifiedRowsList.GetResultsCount(MySqlStatement.SqlStatementType.Insert);
          int insertingCount = _exportDataTable.Rows.Count;
          if (warningsFound)
          {
            operationDetails.AppendFormat(Resources.ExportDataRowsInsertedWithWarningsText, insertedCount, warningsCount);
            if (insertingCount > insertedCount)
            {
              operationDetails.AddNewLine();
              operationDetails.AppendFormat(Resources.ExportDataLessRowsThanExpectedInsertedText, insertingCount, insertedCount);
            }

            operationDetails.AddNewLine();
            if (warningStatementDetails.Length > 0)
            {
              operationDetails.Append(warningStatementDetails);
              operationDetails.AddNewLine();
            }

            operationDetails.Append(warningDetails);
          }
          else
          {
            operationDetails.AddNewLine();
            operationDetails.AppendFormat(Resources.ExportDataRowsInsertedSuccessfullyText, insertedCount);
          }
        }
      }
      else
      {
        operationSummary = string.Format(tableCreated ? Resources.ExportDataOperationErrorNoRowsText : Resources.ExportDataOperationErrorNoTableText, _exportDataTable.TableName);
        operationsType = InfoDialog.InfoType.Error;
      }

      Cursor = Cursors.Default;
      MiscUtilities.ShowCustomizedInfoDialog(operationsType, operationSummary, operationDetails.ToString(), false);
      operationDetails.Clear();
      warningDetails.Clear();
      warningStatementDetails.Clear();
      return success;
    }

    /// <summary>
    /// Event delegate method fired when the <see cref="ExportDataForm"/> form is being closed.
    /// </summary>
    /// <param name="sender">Sender object.</param>
    /// <param name="e">Event arguments.</param>
    private void ExportDataForm_FormClosing(object sender, FormClosingEventArgs e)
    {
      if (DialogResult == DialogResult.OK)
      {
        e.Cancel = !ExportData();
      }
    }

    /// <summary>
    /// Event delegate method fired when the <see cref="ExportDataForm"/> form is loaded.
    /// </summary>
    /// <param name="sender">Sender object.</param>
    /// <param name="e">Event arguments.</param>
    private void ExportDataForm_Load(object sender, EventArgs e)
    {
      PreviewDataGridView.Columns[PreviewDataGridView.Columns[0].Visible ? 0 : 1].Selected = true;
    }

    /// <summary>
    /// Event delegate method fired when the <see cref="ExportDataToolStripMenuItem"/> context menu item is clicked.
    /// </summary>
    /// <param name="sender">Sender object.</param>
    /// <param name="e">Event arguments.</param>
    private void ExportDataToolStripMenuItem_Click(object sender, EventArgs e)
    {
      ExportButton.Text = Resources.ExportDataText;
      ExportDataToolStripMenuItem.Checked = true;
      CreateTableToolStripMenuItem.Checked = false;
    }

    /// <summary>
    /// Event delegate method fired when the <see cref="FirstRowHeadersCheckBox"/> checkbox's checked state changes.
    /// </summary>
    /// <param name="sender">Sender object.</param>
    /// <param name="e">Event arguments.</param>
    private void FirstRowHeadersCheckBox_CheckedChanged(object sender, EventArgs e)
    {
      int grdIndex = PreviewDataGridView.SelectedColumns.Count > 0 ? PreviewDataGridView.SelectedColumns[0].Index : 0;
      _previewDataTable.FirstRowContainsColumnNames = FirstRowHeadersCheckBox.Checked;
      RecreateColumns();
      SetControlTextValue(AddPrimaryKeyTextBox, _previewDataTable.AutoPkName);
      PreviewDataGridView.CurrentCell = null;
      PreviewDataGridView.Rows[0].Visible = !FirstRowHeadersCheckBox.Checked;
      PreviewDataGridView.Columns[grdIndex].Selected = true;
      if (FirstRowHeadersCheckBox.Checked && PreviewDataGridView.Rows.Count < 2)
      {
        return;
      }

      PreviewDataGridView.FirstDisplayedScrollingRowIndex = FirstRowHeadersCheckBox.Checked ? 1 : 0;
    }

    /// <summary>
    /// Reflects in the Primary Key columns combo box if the index is composed of multiple columns or a single one.
    /// </summary>
    private void FlagMultiColumnPrimaryKey()
    {
      int pkQty = _previewDataTable.NumberOfPk;
      AddPrimaryKeyRadioButton.Checked = pkQty == 0;
      UseExistingColumnRadioButton.Checked = pkQty > 0;
      if (PrimaryKeyColumnsComboBox.Items.Count == 0)
      {
        return;
      }

      if (pkQty < 2 && PrimaryKeyColumnsComboBox.Items[0].ToString() == Resources.ExportDataMultiPrimaryKeyText)
      {
        PrimaryKeyColumnsComboBox.Items.RemoveAt(0);
        var pkColumn = _previewDataTable.Columns.Cast<MySqlDataColumn>().Skip(1).First(i => i.PrimaryKey);
        if (pkColumn == null)
        {
          return;
        }

        PrimaryKeyColumnsComboBox.SelectedIndexChanged -= PrimaryKeyColumnsComboBox_SelectedIndexChanged;
        PrimaryKeyColumnsComboBox.SelectedItem = pkColumn.DisplayName;
        PrimaryKeyColumnsComboBox.SelectedIndexChanged += PrimaryKeyColumnsComboBox_SelectedIndexChanged;
      }
      else if (pkQty > 1 && PrimaryKeyColumnsComboBox.Items[0].ToString() != Resources.ExportDataMultiPrimaryKeyText)
      {
        PrimaryKeyColumnsComboBox.Items.Insert(0, Resources.ExportDataMultiPrimaryKeyText);
        PrimaryKeyColumnsComboBox.SelectedIndex = 0;
      }
    }

    /// <summary>
    /// Gets the MySQL Column bound to the currently selected grid column.
    /// </summary>
    /// <returns><see cref="MySqlDataColumn"/> object bound to the currently selected grid column.</returns>
    private MySqlDataColumn GetCurrentMySqlDataColumn()
    {
      MySqlDataColumn currentColumn = null;
      if (PreviewDataGridView.SelectedColumns.Count > 0)
      {
        currentColumn = _previewDataTable.GetColumnAtIndex(PreviewDataGridView.SelectedColumns[0].Index);
      }

      return currentColumn;
    }

    /// <summary>
    /// Fills the data type combo with the valid values for the columns data type.
    /// </summary>
    private void InitializeDataTypeCombo()
    {
      _isUserInput = false;
      DataTypeBindingSource.DataSource = Settings.Default.ExportShowAllMySqlDataTypes
        ? MySqlDataType.AllDataTypesDictionary
        : MySqlDataType.CommonDataTypesDictionary;
      DataTypeBindingSource.DataMember = null;
      DataTypeComboBox.DataSource = DataTypeBindingSource;
      DataTypeComboBox.DisplayMember = "Key";
      DataTypeComboBox.ValueMember = "Key";
      _isUserInput = true;
    }

    /// <summary>
    /// Creates the <see cref="MySqlDataTable"/> preview table and fills it with a subset of all the data to export.
    /// </summary>
    private void LoadPreviewData()
    {
      if (_exportDataRange == null)
      {
        return;
      }

      _previewDataTable = new MySqlDataTable(
        _wbConnection,
        _proposedTableName,
        true,
        Settings.Default.ExportUseFormattedValues,
        Settings.Default.ExportDetectDatatype,
        Settings.Default.ExportAddBufferToVarchar,
        Settings.Default.ExportAutoIndexIntColumns,
        Settings.Default.ExportAutoAllowEmptyNonIndexColumns)
      {
        IsPreviewTable = true
      };
      _previewDataTable.TableColumnPropertyValueChanged += PreviewTableColumnPropertyValueChanged;
      _previewDataTable.TableWarningsChanged += PreviewTableWarningsChanged;
      int previewRowsQty = Math.Min(_exportDataRange.Rows.Count, Settings.Default.ExportLimitPreviewRowsQuantity);
      _previewDataTable.SetupColumnsWithData(_exportDataRange, true, false, previewRowsQty);
      PreviewDataGridView.DataSource = _previewDataTable;
    }

    /// <summary>
    /// Event delegate method fired when the <see cref="PreviewDataGridView"/> grid cells will display a tooltip.
    /// </summary>
    /// <param name="sender">Sender object.</param>
    /// <param name="e">Event arguments.</param>
    private void PreviewDataGridView_CellToolTipTextNeeded(object sender, DataGridViewCellToolTipTextNeededEventArgs e)
    {
      e.ToolTipText = e.RowIndex >= 0 ? Resources.ExportColumnsGridToolTipCaption : PreviewDataGridView.Columns[e.ColumnIndex].HeaderText;
    }

    /// <summary>
    /// Event delegate method fired when the <see cref="PreviewDataGridView"/> grid data binding operation completes.
    /// </summary>
    /// <param name="sender">Sender object.</param>
    /// <param name="e">Event arguments.</param>
    private void PreviewDataGridView_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
    {
      if (e.ListChangedType != ListChangedType.Reset)
      {
        return;
      }

      PreviewDataGridView.CurrentCell = null;
      PreviewDataGridView.Rows[0].Visible = !FirstRowHeadersCheckBox.Checked;
    }

    /// <summary>
    /// Event delegate method fired when the <see cref="PreviewDataGridView"/> grid catches that a key is down.
    /// </summary>
    /// <param name="sender">Sender object.</param>
    /// <param name="e">Event arguments.</param>
    private void PreviewDataGridView_KeyDown(object sender, KeyEventArgs e)
    {
      if (PreviewDataGridView.SelectedColumns.Count == 0)
      {
        return;
      }

      if (!e.Alt)
      {
        return;
      }

      int currentSelectedIdx = PreviewDataGridView.SelectedColumns[0].Index;
      int newIdx;
      switch (e.KeyCode.ToString())
      {
        case "P":
          newIdx = currentSelectedIdx - 1;
          if (newIdx >= (AddPrimaryKeyRadioButton.Checked ? 0 : 1))
          {
            PreviewDataGridView.Columns[newIdx].Selected = true;
            PreviewDataGridView.FirstDisplayedScrollingColumnIndex = newIdx;
          }

          break;

        case "N":
          newIdx = currentSelectedIdx + 1;
          if (newIdx < PreviewDataGridView.Columns.Count)
          {
            PreviewDataGridView.Columns[newIdx].Selected = true;
            PreviewDataGridView.FirstDisplayedScrollingColumnIndex = newIdx;
          }

          break;
      }
    }

    /// <summary>
    /// Event delegate method fired when the <see cref="PreviewDataGridView"/> grid selection changes.
    /// </summary>
    /// <param name="sender">Sender object.</param>
    /// <param name="e">Event arguments.</param>
    private void PreviewDataGridView_SelectionChanged(object sender, EventArgs e)
    {
      RefreshColumnControlsAndWarnings();
    }

    /// <summary>
    /// Event delegate method fired when a property value on any of the columns in the <see cref="_previewDataTable"/> table changes.
    /// </summary>
    /// <param name="sender">A <see cref="MySqlDataColumn"/> object representing the column with a changed property.</param>
    /// <param name="args">Event arguments.</param>
    public void PreviewTableColumnPropertyValueChanged(object sender, PropertyChangedEventArgs args)
    {
      MySqlDataColumn changedColumn = sender as MySqlDataColumn;
      MySqlDataColumn currentColumn = GetCurrentMySqlDataColumn();
      if (changedColumn == null || changedColumn != currentColumn)
      {
        return;
      }

      _isUserInput = false;
      switch (args.PropertyName)
      {
        case "CreateIndex":
          CreateIndexCheckBox.Checked = changedColumn.CreateIndex;
          break;

        case "ExcludeColumn":
          ExcludeColumnCheckBox.Checked = changedColumn.ExcludeColumn;
          break;

        case "PrimaryKey":
          PrimaryKeyCheckBox.Checked = changedColumn.PrimaryKey;
          break;

        case "UniqueKey":
          UniqueIndexCheckBox.Checked = changedColumn.UniqueKey;
          break;
      }

      _isUserInput = true;
    }

    /// <summary>
    /// Event delegate method fired when the warning texts list of any column in the <see cref="_previewDataTable"/> table changes.
    /// </summary>
    /// <param name="sender">Sender object.</param>
    /// <param name="args">Event arguments.</param>
    private void PreviewTableWarningsChanged(object sender, TableWarningsChangedArgs args)
    {
      switch (args.WarningsType)
      {
        case TableWarningsChangedArgs.TableWarningsType.AutoPrimaryKeyWarnings:
          ShowValidationWarning("PrimaryKeyWarning", args.WarningsQuantity > 0, Resources.PrimaryKeyColumnExistsWarning);
          break;

        case TableWarningsChangedArgs.TableWarningsType.ColumnWarnings:
          MySqlDataColumn column = sender as MySqlDataColumn;
          if (column != null)
          {
            DataGridViewColumn gridCol = PreviewDataGridView.Columns[column.Ordinal];
            bool showWarning = args.WarningsQuantity > 0;
            ShowValidationWarning("ColumnOptionsWarning", showWarning, args.CurrentWarning);
            gridCol.DefaultCellStyle.BackColor = column.ExcludeColumn ? Color.LightGray : (showWarning ? Color.OrangeRed : PreviewDataGridView.DefaultCellStyle.BackColor);
          }
          break;

        case TableWarningsChangedArgs.TableWarningsType.TableNameWarnings:
          ShowValidationWarning("TableNameWarning", args.WarningsQuantity > 0, args.CurrentWarning);
          break;
      }

      if (args.WarningsType != TableWarningsChangedArgs.TableWarningsType.ColumnWarnings)
      {
        ExportButton.Enabled = _previewDataTable.IsTableNameValid && _previewDataTable.IsAutoPkColumnNameValid;
      }
    }

    /// <summary>
    /// Event delegate method fired when the <see cref="PrimaryKeyCheckBox"/> object's checked state changes.
    /// </summary>
    /// <param name="sender">Sender object.</param>
    /// <param name="ea">Event arguments.</param>
    private void PrimaryKeyCheckBox_CheckedChanged(object sender, EventArgs ea)
    {
      if (!_isUserInput)
      {
        return;
      }

      MySqlDataColumn currentCol = GetCurrentMySqlDataColumn();
      if (currentCol == null || PrimaryKeyCheckBox.Checked == currentCol.PrimaryKey)
      {
        return;
      }

      currentCol.PrimaryKey = PrimaryKeyCheckBox.Checked;
      FlagMultiColumnPrimaryKey();
      RefreshColumnControlsEnabledStatus(false);
    }

    /// <summary>
    /// Event delegate method fired when the <see cref="PrimaryKeyColumnsComboBox"/> combo box selected index changes.
    /// </summary>
    /// <param name="sender">Sender object.</param>
    /// <param name="e">Event arguments.</param>
    private void PrimaryKeyColumnsComboBox_SelectedIndexChanged(object sender, EventArgs e)
    {
      if (PrimaryKeyColumnsComboBox.SelectedIndex == -1)
      {
        return;
      }

      if (_previewDataTable.NumberOfPk > 1 && PrimaryKeyColumnsComboBox.SelectedIndex == 0)
      {
        return;
      }

      // If <Multiple Items> was previously selected we need to remove it since we are selecting a single column now as a primary key
      if (PrimaryKeyColumnsComboBox.Items[0].ToString() == Resources.ExportDataMultiPrimaryKeyText)
      {
        PrimaryKeyColumnsComboBox.BeginUpdate();
        int index = PrimaryKeyColumnsComboBox.SelectedIndex;
        PrimaryKeyColumnsComboBox.Items.RemoveAt(0);
        if (index == 0)
        {
          PrimaryKeyColumnsComboBox.SelectedIndex = 0;
        }

        PrimaryKeyColumnsComboBox.EndUpdate();
      }

      // Now we need to adjust the index of the actual column we want to set the PrimaryKey flag for
      int comboColumnIndex = 0;
      MySqlDataColumn currentColumn = GetCurrentMySqlDataColumn();
      for (int coldIdx = 1; coldIdx < _previewDataTable.Columns.Count; coldIdx++)
      {
        MySqlDataColumn col = _previewDataTable.GetColumnAtIndex(coldIdx);
        if (col.ExcludeColumn)
        {
          continue;
        }

        col.PrimaryKey = comboColumnIndex == PrimaryKeyColumnsComboBox.SelectedIndex;
        if (col.PrimaryKey)
        {
          col.CreateIndex = col.UniqueKey = col.AllowNull = col.ExcludeColumn = false;
          if (col != currentColumn)
          {
            var dataGridViewColumn = PreviewDataGridView.Columns[col.ColumnName];
            if (dataGridViewColumn != null)
            {
              dataGridViewColumn.Selected = true;
            }

            var gridViewColumn = PreviewDataGridView.Columns[col.ColumnName];
            if (gridViewColumn != null)
            {
              PreviewDataGridView.FirstDisplayedScrollingColumnIndex = gridViewColumn.Index;
            }
          }
        }

        comboColumnIndex++;
      }
    }

    /// <summary>
    /// Refreshes the columns names and data types based on the data having the first row (not used as column names) or not.
    /// </summary>
    private void RecreateColumns()
    {
      for (int colIdx = 0; colIdx < _previewDataTable.Columns.Count; colIdx++)
      {
        MySqlDataColumn mysqlCol = _previewDataTable.GetColumnAtIndex(colIdx);
        DataGridViewColumn gridCol = PreviewDataGridView.Columns[colIdx];
        gridCol.HeaderText = mysqlCol.DisplayName;
        PreviewDataGridView.Columns[colIdx].SortMode = DataGridViewColumnSortMode.NotSortable;
      }

      PreviewDataGridView.SelectionMode = DataGridViewSelectionMode.FullColumnSelect;
      RefreshPrimaryKeyColumnsCombo(true);
    }

    /// <summary>
    /// Refreshes the values of controls tied to column properties and its related warning controls.
    /// </summary>
    private void RefreshColumnControlsAndWarnings()
    {
      bool columnSelected = PreviewDataGridView.SelectedColumns.Count > 0;
      ColumnOptionsGroupBox.Enabled = columnSelected;
      if (!columnSelected)
      {
        return;
      }

      // Set current column
      DataGridViewColumn gridCol = PreviewDataGridView.SelectedColumns[0];
      MySqlDataColumn mysqlCol = _previewDataTable.GetColumnAtIndex(gridCol.Index);

      // Set controls tied to column properties
      SetControlTextValue(ColumnNameTextBox, mysqlCol.DisplayName);
      SetControlTextValue(DataTypeComboBox, mysqlCol.MySqlDataType);
      CreateIndexCheckBox.Checked = mysqlCol.CreateIndex;
      UniqueIndexCheckBox.Checked = mysqlCol.UniqueKey;
      PrimaryKeyCheckBox.Checked = mysqlCol.PrimaryKey;
      AllowEmptyCheckBox.Checked = mysqlCol.AllowNull;
      ExcludeColumnCheckBox.Checked = mysqlCol.ExcludeColumn;

      // Update column warnings
      RefreshColumnWarnings(mysqlCol);

      // Refresh column controls enabled status and related grid column background color
      RefreshColumnControlsEnabledStatus(true);
    }

    /// <summary>
    /// Enables or disables checkboxes in the form depending on specific rules.
    /// </summary>
    /// <param name="refreshGridColumnBkColor">Flag indicating if the grid column's background color should be refreshed.</param>
    private void RefreshColumnControlsEnabledStatus(bool refreshGridColumnBkColor)
    {
      if (PreviewDataGridView.SelectedColumns.Count == 0)
      {
        return;
      }

      MySqlDataColumn mysqlCol = GetCurrentMySqlDataColumn();
      ExcludeColumnCheckBox.Enabled = true;
      PrimaryKeyCheckBox.Enabled = !(ExcludeColumnCheckBox.Checked || AddPrimaryKeyRadioButton.Checked);
      UniqueIndexCheckBox.Enabled = !ExcludeColumnCheckBox.Checked;
      CreateIndexCheckBox.Enabled = !(ExcludeColumnCheckBox.Checked || UniqueIndexCheckBox.Checked || PrimaryKeyCheckBox.Checked);
      AllowEmptyCheckBox.Enabled = !(ExcludeColumnCheckBox.Checked || PrimaryKeyCheckBox.Checked);
      UseExistingColumnRadioButton.Enabled = !_previewDataTable.Columns.Cast<MySqlDataColumn>().Skip(1).All(i => i.ExcludeColumn);
      PrimaryKeyColumnsComboBox.Enabled = UseExistingColumnRadioButton.Enabled && UseExistingColumnRadioButton.Checked;
      DataTypeComboBox.Enabled = !mysqlCol.AutoPk;

      if (mysqlCol.Ordinal == 0)
      {
        DataTypeComboBox.Enabled = UniqueIndexCheckBox.Enabled = CreateIndexCheckBox.Enabled = ExcludeColumnCheckBox.Enabled = AllowEmptyCheckBox.Enabled = PrimaryKeyCheckBox.Enabled = false;
      }

      if (!refreshGridColumnBkColor)
      {
        return;
      }

      DataGridViewColumn gridCol = PreviewDataGridView.SelectedColumns[0];
      gridCol.DefaultCellStyle.BackColor = mysqlCol.ExcludeColumn ? Color.LightGray : (mysqlCol.WarningsQuantity > 0 ? Color.OrangeRed : PreviewDataGridView.DefaultCellStyle.BackColor);
    }

    /// <summary>
    /// Refreshes the warnings shown to users related to the given column.
    /// </summary>
    /// <param name="column">Column to refresh warnings for.</param>
    private void RefreshColumnWarnings(MySqlDataColumn column)
    {
      bool showWarning = !string.IsNullOrEmpty(column.CurrentColumnWarningText);
      ShowValidationWarning("ColumnOptionsWarning", showWarning, column.CurrentColumnWarningText);
    }

    /// <summary>
    /// Fills the <see cref="PrimaryKeyColumnsComboBox"/> combo box containing the names of column names to choose from to create a Primary Key.
    /// </summary>
    /// <param name="recreatingColumnNames">Flag indicating if the primarky key columns combobox is being refreshed due a recreation of all column names.</param>
    private void RefreshPrimaryKeyColumnsCombo(bool recreatingColumnNames)
    {
      int selectedIndex = -1;
      string selectedItem = null;
      if (UseExistingColumnRadioButton.Checked)
      {
        selectedIndex = PrimaryKeyColumnsComboBox.SelectedIndex;
        selectedItem = (string)PrimaryKeyColumnsComboBox.SelectedItem;
      }

      PrimaryKeyColumnsComboBox.BeginUpdate();
      PrimaryKeyColumnsComboBox.Items.Clear();
      if (!string.IsNullOrEmpty(Resources.ExportDataMultiPrimaryKeyText) && selectedItem == Resources.ExportDataMultiPrimaryKeyText && _previewDataTable.NumberOfPk > 1)
      {
        PrimaryKeyColumnsComboBox.Items.Add(Resources.ExportDataMultiPrimaryKeyText ?? string.Empty);
      }

      foreach (MySqlDataColumn mysqlCol in _previewDataTable.Columns.Cast<MySqlDataColumn>().Where(mysqlCol => mysqlCol.Ordinal != 0 && !mysqlCol.ExcludeColumn))
      {
        PrimaryKeyColumnsComboBox.Items.Add(mysqlCol.DisplayName);
      }

      PrimaryKeyColumnsComboBox.SelectedIndexChanged -= PrimaryKeyColumnsComboBox_SelectedIndexChanged;
      if (recreatingColumnNames)
      {
        // All columns are being recreated, so the amounts of non-excluded columns has not changed, we need to select the same index.
        PrimaryKeyColumnsComboBox.SelectedIndex = selectedIndex;
      }
      else
      {
        // A column is being excluded and it may have had its PrimaryKey property value set to true. We will try to set the saved SelectedItem
        // value back, if it is not assigned it means the excluded column was a Primary Key and we need to reset the combo selected value.
        PrimaryKeyColumnsComboBox.SelectedItem = selectedItem;
        if (PrimaryKeyColumnsComboBox.SelectedItem == null)
        {
          int pkQty = _previewDataTable.NumberOfPk;
          if (pkQty > 0)
          {
            var pkColumn = _previewDataTable.Columns.Cast<MySqlDataColumn>().Skip(1).First(i => i.PrimaryKey);
            if (pkColumn != null)
            {
              PrimaryKeyColumnsComboBox.SelectedItem = pkColumn.DisplayName;
            }
          }
          else
          {
            AddPrimaryKeyRadioButton.Checked = pkQty == 0;
            UseExistingColumnRadioButton.Checked = pkQty > 0;
          }
        }
      }

      PrimaryKeyColumnsComboBox.SelectedIndexChanged += PrimaryKeyColumnsComboBox_SelectedIndexChanged;
      PrimaryKeyColumnsComboBox.EndUpdate();
    }

    /// <summary>
    /// Resets the timer used on text changes only if there was a user input.
    /// </summary>
    private void ResetTextChangedTimer()
    {
      if (!_isUserInput)
      {
        return;
      }

      TextChangedTimer.Stop();
      TextChangedTimer.Start();
    }

    /// <summary>
    /// Sets the text property value of the given control.
    /// </summary>
    /// <param name="control">Any object inheriting from <see cref="Control"/>.</param>
    /// <param name="textValue">Text to assign to the control's Text property.</param>
    private void SetControlTextValue(Control control, string textValue)
    {
      if (control.Text == textValue)
      {
        return;
      }

      _isUserInput = false;
      control.Text = textValue;
      _isUserInput = true;
    }

    /// <summary>
    /// Sets the default primary key column based on the data type of the first column.
    /// </summary>
    private void SetDefaultPrimaryKey()
    {
      SetControlTextValue(AddPrimaryKeyTextBox, _previewDataTable.AutoPkName);
      if (_previewDataTable.FirstColumnContainsIntegers)
      {
        UseExistingColumnRadioButton.Checked = true;
        PrimaryKeyColumnsComboBox.SelectedIndex = 0;
        PreviewDataGridView.Columns[1].Selected = true;
      }
      else
      {
        AddPrimaryKeyRadioButton.Checked = true;
      }
    }

    /// <summary>
    /// Shows or hides the visual controls to display warnings for columns or table name.
    /// </summary>
    /// <param name="warningControlPrefix">Prefix of the warning control names.</param>
    /// <param name="show">true to show the warnings, false to hide them.</param>
    /// <param name="text">Warning text to display.</param>
    private void ShowValidationWarning(string warningControlPrefix, bool show, string text)
    {
      show = show && !string.IsNullOrEmpty(text);
      string pictureBoxControlName = warningControlPrefix + "PictureBox";
      string labelControlName = warningControlPrefix + "Label";

      if (ContentAreaPanel.Controls.ContainsKey(pictureBoxControlName) && ContentAreaPanel.Controls.ContainsKey(labelControlName))
      {
        ContentAreaPanel.Controls[pictureBoxControlName].Visible = show;
        ContentAreaPanel.Controls[labelControlName].Text = string.IsNullOrEmpty(text) ? string.Empty : text;
        ContentAreaPanel.Controls[labelControlName].Visible = show;
        return;
      }

      if (!ColumnOptionsGroupBox.Controls.ContainsKey(pictureBoxControlName) || !ColumnOptionsGroupBox.Controls.ContainsKey(labelControlName))
      {
        return;
      }

      ColumnOptionsGroupBox.Controls[pictureBoxControlName].Visible = show;
      ColumnOptionsGroupBox.Controls[labelControlName].Text = string.IsNullOrEmpty(text) ? string.Empty : text;
      ColumnOptionsGroupBox.Controls[labelControlName].Visible = show;
    }

    /// <summary>
    /// Event delegate method fired when the <see cref="TableNameInputTextBox"/> textbox's text changes.
    /// </summary>
    /// <param name="sender">Sender object.</param>
    /// <param name="e">Event arguments.</param>
    private void TableNameInputTextBox_TextChanged(object sender, EventArgs e)
    {
      ResetTextChangedTimer();
    }

    /// <summary>
    /// Event delegate method fired when the <see cref="TableNameInputTextBox"/> textbox is being validated.
    /// </summary>
    /// <param name="sender">Sender object.</param>
    /// <param name="e">Event arguments.</param>
    private void TableNameInputTextBox_Validating(object sender, CancelEventArgs e)
    {
      TextChangedTimer.Stop();
      string newTableName = TableNameInputTextBox.Text.Trim();
      if (_previewDataTable == null || _previewDataTable.TableName == newTableName)
      {
        return;
      }

      _previewDataTable.TableName = newTableName;
      SetControlTextValue(AddPrimaryKeyTextBox, _previewDataTable.AutoPkName);
      AddPrimaryKeyTextBox_Validating(AddPrimaryKeyTextBox, new CancelEventArgs());
    }

    /// <summary>
    /// Event delegate method fired when the <see cref="TextChangedTimer"/> timer's elapses.
    /// </summary>
    /// <param name="sender">Sender object.</param>
    /// <param name="e">Event arguments.</param>
    private void TextChangedTimerTick(object sender, EventArgs e)
    {
      if (TableNameInputTextBox.Focused)
      {
        TableNameInputTextBox_Validating(TableNameInputTextBox, new CancelEventArgs());
      }
      else if (AddPrimaryKeyTextBox.Focused)
      {
        AddPrimaryKeyTextBox_Validating(AddPrimaryKeyTextBox, new CancelEventArgs());
      }
      else if (ColumnNameTextBox.Focused)
      {
        ColumnNameTextBox_Validating(ColumnNameTextBox, new CancelEventArgs());
      }
      else if (DataTypeComboBox.Focused)
      {
        DataTypeComboBox_Validating(DataTypeComboBox, new CancelEventArgs());
      }
      else
      {
        // In case no control has focus no validation is needed, just stop the timer.
        TextChangedTimer.Stop();
      }
    }

    /// <summary>
    /// Event delegate method fired when the <see cref="UniqueIndexCheckBox"/> object's checked state changes.
    /// </summary>
    /// <param name="sender">Sender object.</param>
    /// <param name="ea">Event arguments.</param>
    private void UniqueIndexCheckBox_CheckedChanged(object sender, EventArgs ea)
    {
      if (!_isUserInput)
      {
        return;
      }

      MySqlDataColumn currentCol = GetCurrentMySqlDataColumn();
      if (currentCol == null || UniqueIndexCheckBox.Checked == currentCol.UniqueKey)
      {
        return;
      }

      currentCol.UniqueKey = UniqueIndexCheckBox.Checked;
      RefreshColumnControlsEnabledStatus(false);
    }

    /// <summary>
    /// Event delegate method fired when the <see cref="UseExistingColumnRadioButton"/> radio button checked state changes.
    /// </summary>
    /// <param name="sender">Sender object.</param>
    /// <param name="e">Event arguments.</param>
    private void UseExistingColumnRadioButton_CheckedChanged(object sender, EventArgs e)
    {
      if (!UseExistingColumnRadioButton.Checked)
      {
        return;
      }

      PreviewDataGridView.Columns[0].Visible = false;
      PreviewDataGridView.FirstDisplayedScrollingColumnIndex = 1;
      PrimaryKeyColumnsComboBox.Enabled = true;
      PrimaryKeyColumnsComboBox.SelectedIndex = 0;
      AddPrimaryKeyTextBox.Enabled = false;
      _previewDataTable.UseFirstColumnAsPk = false;
      PreviewDataGridView.Columns[1].Selected = true;
    }
  }
}