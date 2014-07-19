// Copyright (c) 2013-2014, Oracle and/or its affiliates. All rights reserved.
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
using System.Windows.Forms;
using MySQL.ForExcel.Classes;
using MySQL.ForExcel.Interfaces;
using MySQL.ForExcel.Properties;
using MySQL.Utility.Forms;

namespace MySQL.ForExcel.Forms
{
  /// <summary>
  /// Advanced options dialog for the operations performed by the <see cref="ExportDataForm"/>.
  /// </summary>
  public partial class GlobalOptionsDialog : AutoStyleableBaseDialog
  {
    /// <summary>
    /// Gets or sets the sessions to be deleted.
    /// </summary>
    private List<IConnectionInfo> _connectionInfosToDelete;

    /// <summary>
    /// Initializes a new instance of the <see cref="GlobalOptionsDialog"/> class.
    /// </summary>
    public GlobalOptionsDialog()
    {
      _connectionInfosToDelete = new List<IConnectionInfo>();
      InitializeComponent();
      ConnectionTimeoutNumericUpDown.Maximum = Int32.MaxValue / 1000;
      RefreshControlValues();
      SetRestoreSessionsRadioButtonsEnabledStatus();
    }

    /// <summary>
    /// Deletes the edit/import connection information objects marked to in the management dialog.
    /// </summary>
    private void DeleteConnectionInfos()
    {
      foreach (var connectionInfo in _connectionInfosToDelete)
      {
        if (connectionInfo != null && connectionInfo.GetType() == typeof(EditConnectionInfo))
        {
          Globals.ThisAddIn.EditConnectionInfos.Remove(connectionInfo as EditConnectionInfo);
        }
        else
        {
          Globals.ThisAddIn.StoredImportConnectionInfos.Remove(connectionInfo as ImportConnectionInfo);
        }
      }
    }

    /// <summary>
    /// Event delegate method fired when the <see cref="ImportAdvancedOptionsDialog"/> is being closed.
    /// </summary>
    /// <param name="sender">Sender object.</param>
    /// <param name="e">Event arguments.</param>
    private void GlobalOptionsDialog_FormClosing(object sender, FormClosingEventArgs e)
    {
      if (DialogResult == DialogResult.Cancel)
      {
        return;
      }

      if (_connectionInfosToDelete.Count > 0)
      {
        DeleteConnectionInfos();
      }

      Settings.Default.GlobalConnectionConnectionTimeout = (uint)ConnectionTimeoutNumericUpDown.Value;
      Settings.Default.GlobalConnectionCommandTimeout = (uint)QueryTimeoutNumericUpDown.Value;
      Settings.Default.EditUseOptimisticUpdate = UseOptimisticUpdatesCheckBox.Checked;
      Settings.Default.GlobalSqlQueriesPreviewQueries = PreviewSqlQueriesRadioButton.Checked;
      Settings.Default.GlobalSqlQueriesShowQueriesWithResults = ShowExecutedSqlQueryRadioButton.Checked;
      Settings.Default.EditSessionsRestoreWhenOpeningWorkbook = RestoreSavedEditSessionsCheckBox.Checked;
      Settings.Default.EditSessionsReuseWorksheets = ReuseWorksheetsRadioButton.Checked;
      MiscUtilities.SaveSettings();
    }

    /// <summary>
    /// Refreshes the dialog controls' values.
    /// </summary>
    /// <param name="useDefaultValues">Controls are set to their default values if <c>true</c>. Current stored values in application settings are used otherwise.</param>
    private void RefreshControlValues(bool useDefaultValues = false)
    {
      QueryTimeoutNumericUpDown.Maximum = ConnectionTimeoutNumericUpDown.Maximum;

      if (useDefaultValues)
      {
        var settings = Settings.Default;
        ConnectionTimeoutNumericUpDown.Value = Math.Min(ConnectionTimeoutNumericUpDown.Maximum, settings.GetPropertyDefaultValueByName<uint>("GlobalConnectionConnectionTimeout"));
        QueryTimeoutNumericUpDown.Value = settings.GetPropertyDefaultValueByName<uint>("GlobalConnectionCommandTimeout");
        UseOptimisticUpdatesCheckBox.Checked = settings.GetPropertyDefaultValueByName<bool>("EditUseOptimisticUpdate");
        PreviewSqlQueriesRadioButton.Checked = settings.GetPropertyDefaultValueByName<bool>("GlobalSqlQueriesPreviewQueries");
        ShowExecutedSqlQueryRadioButton.Checked = settings.GetPropertyDefaultValueByName<bool>("GlobalSqlQueriesShowQueriesWithResults");
        RestoreSavedEditSessionsCheckBox.Checked = settings.GetPropertyDefaultValueByName<bool>("EditSessionsRestoreWhenOpeningWorkbook");
        ReuseWorksheetsRadioButton.Checked = settings.GetPropertyDefaultValueByName<bool>("EditSessionsReuseWorksheets");
      }
      else
      {
        ConnectionTimeoutNumericUpDown.Value = Math.Min(ConnectionTimeoutNumericUpDown.Maximum, Settings.Default.GlobalConnectionConnectionTimeout);
        QueryTimeoutNumericUpDown.Value = Settings.Default.GlobalConnectionCommandTimeout;
        UseOptimisticUpdatesCheckBox.Checked = Settings.Default.EditUseOptimisticUpdate;
        PreviewSqlQueriesRadioButton.Checked = Settings.Default.GlobalSqlQueriesPreviewQueries;
        ShowExecutedSqlQueryRadioButton.Checked = Settings.Default.GlobalSqlQueriesShowQueriesWithResults;
        RestoreSavedEditSessionsCheckBox.Checked = Settings.Default.EditSessionsRestoreWhenOpeningWorkbook;
        ReuseWorksheetsRadioButton.Checked = Settings.Default.EditSessionsReuseWorksheets;
      }

      NoSqlStatementsRadioButton.Checked = !PreviewSqlQueriesRadioButton.Checked && !ShowExecutedSqlQueryRadioButton.Checked;
      CreateNewWorksheetsRadioButton.Checked = !ReuseWorksheetsRadioButton.Checked;
    }

    /// <summary>
    /// Handles the Click event of the ResetToDefaultsButton control.
    /// </summary>
    /// <param name="sender">The source of the event.</param>
    /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
    private void ResetToDefaultsButton_Click(object sender, EventArgs e)
    {
      RefreshControlValues(true);
      Refresh();
    }

    /// <summary>
    /// Event delegate method fired when the <see cref="RestoreSavedEditSessionsCheckBox"/> checked value changes.
    /// </summary>
    /// <param name="sender">Sender object.</param>
    /// <param name="e">Event arguments.</param>
    private void RestoreSavedEditSessionsCheckBox_CheckedChanged(object sender, EventArgs e)
    {
      SetRestoreSessionsRadioButtonsEnabledStatus();
    }

    /// <summary>
    /// Enables or disables the radio buttons related to the restore Edit sessions options based on the value of the <see cref="RestoreSavedEditSessionsCheckBox"/> checkbox.
    /// </summary>
    private void SetRestoreSessionsRadioButtonsEnabledStatus()
    {
      ReuseWorksheetsRadioButton.Enabled = RestoreSavedEditSessionsCheckBox.Checked;
      CreateNewWorksheetsRadioButton.Enabled = RestoreSavedEditSessionsCheckBox.Checked;
    }

    /// <summary>
    /// Handles the Click event of the ManageConnectionInfosButton control.
    /// </summary>
    /// <param name="sender">The source of the event.</param>
    /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
    private void ManageConnectionInfosButton_Click(object sender, EventArgs e)
    {
      using (var manageConnectionInfosDialog = new ManageConnectionInfosDialog())
      {
        _connectionInfosToDelete.Clear();
        manageConnectionInfosDialog.ShowDialog();
        if (manageConnectionInfosDialog.DialogResult != DialogResult.Cancel)
        {
          _connectionInfosToDelete = manageConnectionInfosDialog.ConnectionInfosToDelete;
        }
      }
    }
  }
}