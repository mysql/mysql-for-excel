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
using System.Globalization;
using System.Linq;
using System.Windows.Forms;
using MySQL.ForExcel.Classes;
using MySQL.ForExcel.Properties;
using MySQL.Utility.Forms;

namespace MySQL.ForExcel.Forms
{
  /// <summary>
  /// Advanced options dialog for the operations performed by the <see cref="AppendDataForm"/>.
  /// </summary>
  public partial class AppendAdvancedOptionsDialog : AutoStyleableBaseDialog
  {
    /// <summary>
    /// <c>true</c> when at least one of the _mappings was changed.
    /// </summary>
    private bool _mappingsWereChanged;

    /// <summary>
    /// Specific column mapping currently selected by the user.
    /// </summary>
    private MySqlColumnMapping _selectedMapping;

    /// <summary>
    /// List of column mappings for the current user.
    /// </summary>
    public readonly List<MySqlColumnMapping> Mappings;

    /// <summary>
    /// Gets or sets a value indicating whether the data in the parent form needs to be reloaded on the grids.
    /// </summary>
    /// <value>
    ///   <c>true</c> if requires refreshing; otherwise, <c>false</c>.
    /// </value>
    public bool ParentFormRequiresRefresh { get; private set; }

    /// <summary>
    /// Creates a new instance of the <see cref="AppendAdvancedOptionsDialog"/> class.
    /// </summary>
    public AppendAdvancedOptionsDialog(List<MySqlColumnMapping> mappings)
    {
      ParentFormRequiresRefresh = false;
      InitializeComponent();
      RefreshControlValues();
      Mappings = mappings.Select(item => (MySqlColumnMapping)item.Clone()).ToList();
      RefreshMappingList();
    }

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
      ParentFormRequiresRefresh = Settings.Default.AppendUseFormattedValues != UseFormattedValuesCheckBox.Checked ||
                                  Settings.Default.AppendLimitPreviewRowsQuantity != previewRowsQuantity ||
                                  _mappingsWereChanged;

      Settings.Default.AppendPerformAutoMap = DoNotPerformAutoMapCheckBox.Checked;
      Settings.Default.AppendAutoStoreColumnMapping = AutoStoreColumnMappingCheckBox.Checked;
      Settings.Default.AppendReloadColumnMapping = ReloadColumnMappingCheckBox.Checked;
      Settings.Default.AppendUseFormattedValues = UseFormattedValuesCheckBox.Checked;
      Settings.Default.AppendLimitPreviewRowsQuantity = previewRowsQuantity;
      Settings.Default.AppendSqlQueriesDisableIndexes = DisableTableIndexesCheckBox.Checked;
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

      _mappingsWereChanged = true;
      Mappings.Remove(_selectedMapping);
      RefreshMappingList();
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
      if (useDefaultValues)
      {
        var settings = Settings.Default;
        DoNotPerformAutoMapCheckBox.Checked = settings.GetPropertyDefaultValueByName<bool>("AppendPerformAutoMap");
        AutoStoreColumnMappingCheckBox.Checked = settings.GetPropertyDefaultValueByName<bool>("AppendAutoStoreColumnMapping");
        ReloadColumnMappingCheckBox.Checked = settings.GetPropertyDefaultValueByName<bool>("AppendReloadColumnMapping");
        UseFormattedValuesCheckBox.Checked = settings.GetPropertyDefaultValueByName<bool>("AppendUseFormattedValues");
        PreviewRowsQuantityNumericUpDown.Value = settings.GetPropertyDefaultValueByName<int>("AppendLimitPreviewRowsQuantity");
        DisableTableIndexesCheckBox.Checked = settings.GetPropertyDefaultValueByName<bool>("AppendSqlQueriesDisableIndexes");
      }
      else
      {
        DoNotPerformAutoMapCheckBox.Checked = Settings.Default.AppendPerformAutoMap;
        AutoStoreColumnMappingCheckBox.Checked = Settings.Default.AppendAutoStoreColumnMapping;
        ReloadColumnMappingCheckBox.Checked = Settings.Default.AppendReloadColumnMapping;
        UseFormattedValuesCheckBox.Checked = Settings.Default.AppendUseFormattedValues;
        PreviewRowsQuantityNumericUpDown.Value = Math.Min(PreviewRowsQuantityNumericUpDown.Maximum, Settings.Default.AppendLimitPreviewRowsQuantity);
        DisableTableIndexesCheckBox.Checked = Settings.Default.AppendSqlQueriesDisableIndexes;
      }
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
        _mappingsWereChanged = true;
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