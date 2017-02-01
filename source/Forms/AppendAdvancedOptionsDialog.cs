// Copyright (c) 2012, 2017, Oracle and/or its affiliates. All rights reserved.
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
using System.Globalization;
using System.Linq;
using System.Windows.Forms;
using MySQL.ForExcel.Classes;
using MySQL.ForExcel.Properties;
using MySql.Utility.Forms;

namespace MySQL.ForExcel.Forms
{
  /// <summary>
  /// Advanced options dialog for the operations performed by the <see cref="AppendDataForm"/>.
  /// </summary>
  public partial class AppendAdvancedOptionsDialog : AutoStyleableBaseDialog
  {
    #region Fields

    /// <summary>
    /// Specific column mapping currently selected by the user.
    /// </summary>
    private MySqlColumnMapping _selectedMapping;

    #endregion Fields

    /// <summary>
    /// Creates a new instance of the <see cref="AppendAdvancedOptionsDialog"/> class.
    /// </summary>
    public AppendAdvancedOptionsDialog(IEnumerable<MySqlColumnMapping> mappings)
    {
      LimitPreviewRowsQuantityChanged = false;
      MappingsChanged = false;
      ShowDataTypesChanged = false;
      UseFormattedValuesChanged = false;
      InitializeComponent();
      RefreshControlValues();
      Mappings = mappings.Select(item => (MySqlColumnMapping)item.Clone()).ToList();
      RefreshMappingList();
    }

    #region Properties

    /// <summary>
    /// Gets a value indicating whether the number of preview rows changed.
    /// </summary>
    public bool LimitPreviewRowsQuantityChanged { get; private set; }

    /// <summary>
    /// List of column mappings for the current user.
    /// </summary>
    public List<MySqlColumnMapping> Mappings { get; private set; }

    /// <summary>
    /// Gets a value indicatng whether mappings were renamed or deleted.
    /// </summary>
    public bool MappingsChanged { get; private set; }

    /// <summary>
    /// Gets a value indicating whether the setting to show data types above column names changed.
    /// </summary>
    public bool ShowDataTypesChanged { get; private set; }

    /// <summary>
    /// Gets a value indicating whether the setting to use formatted values changed.
    /// </summary>
    public bool UseFormattedValuesChanged { get; private set; }

    #endregion Properties

    /// <summary>
    /// Event delegate method fired when the <see cref="ImportAdvancedOptionsDialog"/> is being closed.
    /// </summary>
    /// <param name="sender">Sender object.</param>
    /// <param name="e">Event arguments.</param>
    private void AppendAdvancedOptionsDialog_FormClosing(object sender, FormClosingEventArgs e)
    {
      if (DialogResult == DialogResult.Cancel)
      {
        return;
      }

      var previewRowsQuantity = (int)PreviewRowsQuantityNumericUpDown.Value;
      LimitPreviewRowsQuantityChanged = Settings.Default.AppendLimitPreviewRowsQuantity != previewRowsQuantity;
      ShowDataTypesChanged = Settings.Default.AppendShowDataTypes != ShowDataTypesCheckBox.Checked;
      UseFormattedValuesChanged = Settings.Default.AppendUseFormattedValues != UseFormattedValuesCheckBox.Checked;
      Settings.Default.AppendPerformAutoMap = DoNotPerformAutoMapCheckBox.Checked;
      Settings.Default.AppendAutoStoreColumnMapping = AutoStoreColumnMappingCheckBox.Checked;
      Settings.Default.AppendReloadColumnMapping = ReloadColumnMappingCheckBox.Checked;
      Settings.Default.AppendConfirmColumnMappingOverwriting = ConfirmMappingOverwritingCheckBox.Checked;
      Settings.Default.AppendUseFormattedValues = UseFormattedValuesCheckBox.Checked;
      Settings.Default.AppendShowDataTypes = ShowDataTypesCheckBox.Checked;
      Settings.Default.AppendLimitPreviewRowsQuantity = previewRowsQuantity;
      Settings.Default.AppendSqlQueriesDisableIndexes = DisableTableIndexesCheckBox.Checked;
      Settings.Default.AppendDuplicateUniqueValuesAction = ErrorAndAbortRadioButton.Checked
        ? MySqlDataTable.AppendDuplicateValuesActionType.ErrorOutAndAbort.ToString()
        : (IgnoreDuplicatesRadioButton.Checked
          ? MySqlDataTable.AppendDuplicateValuesActionType.IgnoreDuplicates.ToString()
          : MySqlDataTable.AppendDuplicateValuesActionType.ReplaceDuplicates.ToString());
      Settings.Default.AppendGenerateMultipleInserts = GenerateMultipleInsertsCheckBox.Checked;
      Settings.Default.StoredDataMappings = Mappings;
      MiscUtilities.SaveSettings();
    }

    /// <summary>
    /// Event delegate method fired when the <see cref="DeleteMappingButton"/> button is clicked.
    /// </summary>
    /// <param name="sender">Sender object.</param>
    /// <param name="e">Event arguments.</param>
    private void DeleteMappingButton_Click(object sender, EventArgs e)
    {
      if (_selectedMapping == null)
      {
        return;
      }

      MappingsChanged = true;
      Mappings.Remove(_selectedMapping);
      RefreshMappingList();
    }

    /// <summary>
    /// Event delegate method fired when an item within the <see cref="GenerateMultipleInsertsCheckBox"/> list view is selected.
    /// </summary>
    /// <param name="sender">Sender object.</param>
    /// <param name="e">Event arguments.</param>
    private void GenerateMultipleInsertsCheckBox_CheckedChanged(object sender, EventArgs e)
    {
      DisableTableIndexesCheckBox.Enabled = GenerateMultipleInsertsCheckBox.Checked;
      if (!GenerateMultipleInsertsCheckBox.Checked)
      {
        DisableTableIndexesCheckBox.Checked = false;
      }
    }

    /// <summary>
    /// Event delegate method fired when an item within the <see cref="MappingsListView"/> list view is selected.
    /// </summary>
    /// <param name="sender">Sender object.</param>
    /// <param name="e">Event arguments.</param>
    private void MappingsListView_SelectedIndexChanged(object sender, EventArgs e)
    {
      DeleteMappingButton.Enabled = MappingsListView.SelectedItems.Count > 0;
      _selectedMapping = MappingsListView.SelectedItems.Count > 0 ? MappingsListView.SelectedItems[0].Tag as MySqlColumnMapping : null;
      RenameMappingButton.Enabled = _selectedMapping != null;
    }

    /// <summary>
    /// Refreshes the dialog controls' values.
    /// </summary>
    /// <param name="useDefaultValues">Controls are set to their default values if <c>true</c>. Current stored values in application settings are used otherwise.</param>
    private void RefreshControlValues(bool useDefaultValues = false)
    {
      MySqlDataTable.AppendDuplicateValuesActionType duplicateValuesAction;
      if (useDefaultValues)
      {
        var settings = Settings.Default;
        DoNotPerformAutoMapCheckBox.Checked = settings.GetPropertyDefaultValueByName<bool>("AppendPerformAutoMap");
        AutoStoreColumnMappingCheckBox.Checked = settings.GetPropertyDefaultValueByName<bool>("AppendAutoStoreColumnMapping");
        ReloadColumnMappingCheckBox.Checked = settings.GetPropertyDefaultValueByName<bool>("AppendReloadColumnMapping");
        ConfirmMappingOverwritingCheckBox.Checked = settings.GetPropertyDefaultValueByName<bool>("AppendConfirmColumnMappingOverwriting");
        UseFormattedValuesCheckBox.Checked = settings.GetPropertyDefaultValueByName<bool>("AppendUseFormattedValues");
        ShowDataTypesCheckBox.Checked = settings.GetPropertyDefaultValueByName<bool>("AppendShowDataTypes");
        PreviewRowsQuantityNumericUpDown.Value = settings.GetPropertyDefaultValueByName<int>("AppendLimitPreviewRowsQuantity");
        DisableTableIndexesCheckBox.Checked = settings.GetPropertyDefaultValueByName<bool>("AppendSqlQueriesDisableIndexes");
        GenerateMultipleInsertsCheckBox.Checked = settings.GetPropertyDefaultValueByName<bool>("AppendGenerateMultipleInserts");
        if (Enum.TryParse(settings.GetPropertyDefaultValueByName<string>("AppendDuplicateUniqueValuesAction"), out duplicateValuesAction))
        {
          ErrorAndAbortRadioButton.Checked = duplicateValuesAction == MySqlDataTable.AppendDuplicateValuesActionType.ErrorOutAndAbort;
          IgnoreDuplicatesRadioButton.Checked = duplicateValuesAction == MySqlDataTable.AppendDuplicateValuesActionType.IgnoreDuplicates;
          ReplaceDuplicatesRadioButton.Checked = duplicateValuesAction == MySqlDataTable.AppendDuplicateValuesActionType.ReplaceDuplicates;
        }
      }
      else
      {
        DoNotPerformAutoMapCheckBox.Checked = Settings.Default.AppendPerformAutoMap;
        AutoStoreColumnMappingCheckBox.Checked = Settings.Default.AppendAutoStoreColumnMapping;
        ReloadColumnMappingCheckBox.Checked = Settings.Default.AppendReloadColumnMapping;
        ConfirmMappingOverwritingCheckBox.Checked = Settings.Default.AppendConfirmColumnMappingOverwriting;
        UseFormattedValuesCheckBox.Checked = Settings.Default.AppendUseFormattedValues;
        ShowDataTypesCheckBox.Checked = Settings.Default.AppendShowDataTypes;
        PreviewRowsQuantityNumericUpDown.Value = Math.Min(PreviewRowsQuantityNumericUpDown.Maximum, Settings.Default.AppendLimitPreviewRowsQuantity);
        DisableTableIndexesCheckBox.Checked = Settings.Default.AppendSqlQueriesDisableIndexes;
        GenerateMultipleInsertsCheckBox.Checked = Settings.Default.AppendGenerateMultipleInserts;
        if (Enum.TryParse(Settings.Default.AppendDuplicateUniqueValuesAction, out duplicateValuesAction))
        {
          ErrorAndAbortRadioButton.Checked = duplicateValuesAction == MySqlDataTable.AppendDuplicateValuesActionType.ErrorOutAndAbort;
          IgnoreDuplicatesRadioButton.Checked = duplicateValuesAction == MySqlDataTable.AppendDuplicateValuesActionType.IgnoreDuplicates;
          ReplaceDuplicatesRadioButton.Checked = duplicateValuesAction == MySqlDataTable.AppendDuplicateValuesActionType.ReplaceDuplicates;
        }
      }

      DisableTableIndexesCheckBox.Enabled = GenerateMultipleInsertsCheckBox.Checked;
    }

    /// <summary>
    /// Refreshes the list of column mappings shown in the mappings list view.
    /// </summary>
    private void RefreshMappingList()
    {
      MappingsListView.Items.Clear();

      foreach (var item in Mappings)
      {
        ListViewItem itemList = new ListViewItem
        {
          Text = string.Format("{0} ({1}.{2})", item.Name, item.SchemaName, item.TableName)
        };
        itemList.SubItems.Add(string.Empty);
        itemList.Tag = item;
        MappingsListView.Items.Add(itemList);
      }

      if (MappingsListView.Items.Count > 0)
      {
        MappingsListView.Items[0].Selected = true;
      }
      else
      {
        DeleteMappingButton.Enabled = false;
        RenameMappingButton.Enabled = false;
      }
    }

    /// <summary>
    /// Event delegate method fired when the <see cref="RenameMappingButton"/> button is clicked.
    /// </summary>
    /// <param name="sender">Sender object.</param>
    /// <param name="e">Event arguments.</param>
    private void RenameMappingButton_Click(object sender, EventArgs e)
    {
      if (_selectedMapping == null)
      {
        return;
      }

      int indexForName = 1;
      string proposedMappingName;
      do
      {
        proposedMappingName = _selectedMapping.TableName + "Mapping" + (indexForName > 1 ? indexForName.ToString(CultureInfo.InvariantCulture) : string.Empty);
        indexForName++;
      }
      while (Mappings.Any(mapping => mapping.Name == proposedMappingName));

      string newName;
      using (var newColumnMappingDialog = new AppendNewColumnMappingDialog(proposedMappingName))
      {
        DialogResult dr = newColumnMappingDialog.ShowDialog();
        if (dr == DialogResult.Cancel)
        {
          return;
        }

        MappingsChanged = true;
        newName = newColumnMappingDialog.ColumnMappingName;
      }

      // Show error if name already exists
      if (Mappings.Count(t => string.Compare(t.Name, newName, StringComparison.InvariantCultureIgnoreCase) == 0) > 0)
      {
        MiscUtilities.ShowCustomizedErrorDialog(Resources.MappingNameAlreadyExistsTitle, Resources.MappingNameAlreadyExistsDetail);
        return;
      }

      _selectedMapping.Name = newName;
      RefreshMappingList();
      ListViewItem item = MappingsListView.FindItemWithText(string.Format("{0} ({1}.{2})", newName, _selectedMapping.SchemaName, _selectedMapping.TableName));
      if (item != null)
      {
        MappingsListView.Items[item.Index].Selected = true;
      }

      MappingsListView.Focus();
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
  }
}