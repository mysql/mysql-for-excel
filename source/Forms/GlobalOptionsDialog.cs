// Copyright (c) 2013, 2019, Oracle and/or its affiliates. All rights reserved.
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
using System.Globalization;
using System.Windows.Forms;
using MySql.Utility.Classes;
using MySQL.ForExcel.Classes;
using MySQL.ForExcel.Interfaces;
using MySQL.ForExcel.Properties;
using MySql.Utility.Classes.MySqlWorkbench;
using MySql.Utility.Enums;
using MySql.Utility.Forms;

namespace MySQL.ForExcel.Forms
{
  /// <summary>
  /// Advanced options dialog for the add-in.
  /// </summary>
  public partial class GlobalOptionsDialog : AutoStyleableBaseDialog
  {
    #region Constants

    /// <summary>
    /// The spacing in pixels defined for the inner panel of the dialog from controls.
    /// </summary>
    public const int DIALOG_BORDER_WIDTH = 8;

    /// <summary>
    /// The spacing in pixels defined for the inner panel of the dialog from controls.
    /// </summary>
    public const int DIALOG_RIGHT_SPACING_TO_CONTROLS = 50;

    #endregion Constants

    #region Fields

    /// <summary>
    /// The dialog's initial width.
    /// </summary>
    private int _initialWidth;

    /// <summary>
    /// Dialog showing saved <see cref="IConnectionInfo"/> entries that should be deleted.
    /// </summary>
    private ManageConnectionInfosDialog _manageConnectionInfosDialog;

    #endregion Fields

    /// <summary>
    /// Initializes a new instance of the <see cref="GlobalOptionsDialog"/> class.
    /// </summary>
    public GlobalOptionsDialog()
    {
      _manageConnectionInfosDialog = null;
      InitializeComponent();
      _initialWidth = Width;
      ConnectionTimeoutNumericUpDown.Maximum = Convert.ToInt32(int.MaxValue / 1000);
      SpatialTextFormatComboBox.InitializeComboBoxFromEnum(GeometryAsTextFormatType.None, true);
      RefreshControlValues();
      SetRestoreSessionsRadioButtonsEnabledStatus();
      SetAutomaticMigrationDelayText();
    }

    /// <summary>
    /// Gets a value indicating whether the <see cref="MigrateWorkbenchConnectionsButton"/> should be enabled.
    /// </summary>
    private bool MigrateConnectionsButtonEnabled => !Settings.Default.WorkbenchMigrationSucceeded &&
                                                    Settings.Default.WorkbenchMigrationLastAttempt != DateTime.MinValue &&
                                                    Settings.Default.WorkbenchMigrationRetryDelay != 0;

    /// <summary>
    /// Deletes the edit/import connection information objects marked to in the management dialog.
    /// </summary>
    private void DeleteConnectionInfos()
    {
      if (_manageConnectionInfosDialog?.ConnectionInfosToDelete == null)
      {
        return;
      }

      foreach (var connectionInfo in _manageConnectionInfosDialog.ConnectionInfosToDelete)
      {
        if (connectionInfo == null)
        {
          continue;
        }

        if (connectionInfo.GetType() == typeof(EditConnectionInfo))
        {
          WorkbookConnectionInfos.UserSettingsEditConnectionInfos.Remove(connectionInfo as EditConnectionInfo);
        }
        else
        {
          WorkbookConnectionInfos.UserSettingsImportConnectionInfos.Remove(connectionInfo as ImportConnectionInfo);
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

      var settings = Settings.Default;
      DeleteConnectionInfos();
      settings.GlobalConnectionConnectionTimeout = (uint)ConnectionTimeoutNumericUpDown.Value;
      settings.GlobalConnectionCommandTimeout = (uint)QueryTimeoutNumericUpDown.Value;
      settings.EditUseOptimisticUpdate = UseOptimisticUpdatesCheckBox.Checked;
      settings.GlobalSqlQueriesPreviewQueries = PreviewSqlQueriesRadioButton.Checked;
      settings.GlobalSqlQueriesShowQueriesWithResults = ShowExecutedSqlQueryRadioButton.Checked;
      settings.EditPreviewMySqlData = PreviewTableDataCheckBox.Checked;
      settings.GlobalEditToleranceForFloatAndDouble = decimal.Parse(ToleranceForFloatAndDoubleTextBox.Text);
      settings.EditSessionsRestoreWhenOpeningWorkbook = RestoreSavedEditSessionsCheckBox.Checked;
      settings.EditSessionsReuseWorksheets = ReuseWorksheetsRadioButton.Checked;
      settings.GlobalImportDataRestoreWhenOpeningWorkbook = OpeningWorkbookRadioButton.Checked;
      settings.GlobalSpatialDataAsTextFormat = SpatialTextFormatComboBox.SelectedValue.ToString();
      if (_manageConnectionInfosDialog != null)
      {
        settings.ConnectionInfosLastAccessDays = _manageConnectionInfosDialog.ConnectionInfosLastAccessDays;
        settings.DeleteAutomaticallyOrphanedConnectionInfos = _manageConnectionInfosDialog.DeleteAutomaticallyOrphanedConnectionInfos;
      }

      MiscUtilities.SaveSettings();
    }

    /// <summary>
    /// Handles the Click event of the ManageConnectionInfosButton control.
    /// </summary>
    /// <param name="sender">The source of the event.</param>
    /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
    private void ManageConnectionInfosButton_Click(object sender, EventArgs e)
    {
      if (_manageConnectionInfosDialog == null)
      {
        _manageConnectionInfosDialog = new ManageConnectionInfosDialog();
      }

      _manageConnectionInfosDialog.ShowDialog();
    }

    /// <summary>
    /// Event delegate method fired when the <see cref="MigrateWorkbenchConnectionsButton"/> is clicked.
    /// </summary>
    /// <param name="sender">Sender object.</param>
    /// <param name="e">Event arguments.</param>
    private void MigrateWorkbenchConnectionsButton_Click(object sender, EventArgs e)
    {
      Globals.ThisAddIn.MigrateExternalConnectionsToWorkbench(false);
      SetAutomaticMigrationDelayText();
    }

    /// <summary>
    /// Refreshes the dialog controls' values.
    /// </summary>
    /// <param name="useDefaultValues">Controls are set to their default values if <c>true</c>. Current stored values in application settings are used otherwise.</param>
    private void RefreshControlValues(bool useDefaultValues = false)
    {
      var settings = Settings.Default;
      QueryTimeoutNumericUpDown.Maximum = ConnectionTimeoutNumericUpDown.Maximum;
      GeometryAsTextFormatType spatialFormat;
      if (useDefaultValues)
      {
        ConnectionTimeoutNumericUpDown.Value = Math.Min(ConnectionTimeoutNumericUpDown.Maximum, MiscUtilities.GetPropertyDefaultValueByName<uint>(settings, "GlobalConnectionConnectionTimeout"));
        QueryTimeoutNumericUpDown.Value = MiscUtilities.GetPropertyDefaultValueByName<uint>(settings, "GlobalConnectionCommandTimeout");
        UseOptimisticUpdatesCheckBox.Checked = MiscUtilities.GetPropertyDefaultValueByName<bool>(settings, "EditUseOptimisticUpdate");
        PreviewSqlQueriesRadioButton.Checked = MiscUtilities.GetPropertyDefaultValueByName<bool>(settings, "GlobalSqlQueriesPreviewQueries");
        ShowExecutedSqlQueryRadioButton.Checked = MiscUtilities.GetPropertyDefaultValueByName<bool>(settings, "GlobalSqlQueriesShowQueriesWithResults");
        RestoreSavedEditSessionsCheckBox.Checked = MiscUtilities.GetPropertyDefaultValueByName<bool>(settings, "EditSessionsRestoreWhenOpeningWorkbook");
        ReuseWorksheetsRadioButton.Checked = MiscUtilities.GetPropertyDefaultValueByName<bool>(settings, "EditSessionsReuseWorksheets");
        PreviewTableDataCheckBox.Checked = MiscUtilities.GetPropertyDefaultValueByName<bool>(settings, "EditPreviewMySqlData");
        OpeningWorkbookRadioButton.Checked = MiscUtilities.GetPropertyDefaultValueByName<bool>(settings, "GlobalImportDataRestoreWhenOpeningWorkbook");
        SpatialTextFormatComboBox.SelectedValue = Enum.TryParse(MiscUtilities.GetPropertyDefaultValueByName<string>(settings, "GlobalSpatialDataAsTextFormat"), out spatialFormat)
            ? spatialFormat
            : GeometryAsTextFormatType.None;
        ToleranceForFloatAndDoubleTextBox.Text = MiscUtilities.GetPropertyDefaultValueByName<decimal>(settings, "GlobalEditToleranceForFloatAndDouble").ToString(CultureInfo.CurrentCulture);
      }
      else
      {
        ConnectionTimeoutNumericUpDown.Value = Math.Min(ConnectionTimeoutNumericUpDown.Maximum, settings.GlobalConnectionConnectionTimeout);
        QueryTimeoutNumericUpDown.Value = settings.GlobalConnectionCommandTimeout;
        UseOptimisticUpdatesCheckBox.Checked = settings.EditUseOptimisticUpdate;
        PreviewSqlQueriesRadioButton.Checked = settings.GlobalSqlQueriesPreviewQueries;
        ShowExecutedSqlQueryRadioButton.Checked = settings.GlobalSqlQueriesShowQueriesWithResults;
        RestoreSavedEditSessionsCheckBox.Checked = settings.EditSessionsRestoreWhenOpeningWorkbook;
        ReuseWorksheetsRadioButton.Checked = settings.EditSessionsReuseWorksheets;
        PreviewTableDataCheckBox.Checked = settings.EditPreviewMySqlData;
        OpeningWorkbookRadioButton.Checked = settings.GlobalImportDataRestoreWhenOpeningWorkbook;
        SpatialTextFormatComboBox.SelectedValue = Enum.TryParse(settings.GlobalSpatialDataAsTextFormat, out spatialFormat)
            ? spatialFormat
            : GeometryAsTextFormatType.None;
        ToleranceForFloatAndDoubleTextBox.Text = settings.GlobalEditToleranceForFloatAndDouble.ToString(CultureInfo.CurrentCulture);
      }

      NoSqlStatementsRadioButton.Checked = !PreviewSqlQueriesRadioButton.Checked && !ShowExecutedSqlQueryRadioButton.Checked;
      CreateNewWorksheetsRadioButton.Checked = !ReuseWorksheetsRadioButton.Checked;
      ShowingSidebarRadioButton.Checked = !OpeningWorkbookRadioButton.Checked;
      _manageConnectionInfosDialog?.RefreshControlValues(useDefaultValues);
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
      if (!RestoreSavedEditSessionsCheckBox.Checked || !ReuseWorksheetsRadioButton.CanFocus)
      {
        return;
      }

      // Give focus to the field related to the checkbox whose status changed.
      ReuseWorksheetsRadioButton.Focus();
    }

    /// <summary>
    /// Increases the width of the dialog in case the <see cref="AutomaticMigrationDelayLabel"/> gets too big.
    /// </summary>
    private void SetAutomaticMigrationDelayText()
    {
      SuspendLayout();

      AutomaticMigrationDelayValueLabel.Text = MySqlWorkbench.GetConnectionsMigrationDelayText(Globals.ThisAddIn.NextAutomaticConnectionsMigration, Settings.Default.WorkbenchMigrationSucceeded);
      MigrateWorkbenchConnectionsButton.Enabled = MigrateConnectionsButtonEnabled;
      Width = _initialWidth;
      var spacingDelta = AutomaticMigrationDelayValueLabel.Location.X + AutomaticMigrationDelayValueLabel.Size.Width + DIALOG_RIGHT_SPACING_TO_CONTROLS + (DIALOG_BORDER_WIDTH * 2) - Width;
      if (spacingDelta > 0)
      {
        Width += spacingDelta;
      }

      ResumeLayout();
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
    /// Event delegate method fired when the <see cref="ToleranceForFloatAndDoubleTextBox"/> is being validated.
    /// </summary>
    /// <param name="sender">Sender object.</param>
    /// <param name="e">Event arguments.</param>
    private void ToleranceForFloatAndDoubleTextBox_Validating(object sender, System.ComponentModel.CancelEventArgs e)
    {
      if (!(sender is TextBox textBox)
          || decimal.TryParse(textBox.Text, out _))
      {
        return;
      }

      e.Cancel = true;
    }

    /// <summary>
    /// Event delegate method fired when the <see cref="UseOptimisticUpdatesCheckBox"/> check state changes.
    /// </summary>
    /// <param name="sender">Sender object.</param>
    /// <param name="e">Event arguments.</param>
    private void UseOptimisticUpdatesCheckBox_CheckedChanged(object sender, EventArgs e)
    {
      ToleranceForFloatAndDoubleTextBox.ReadOnly = !UseOptimisticUpdatesCheckBox.Checked;
    }
  }
}