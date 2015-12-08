// Copyright (c) 2012, 2015, Oracle and/or its affiliates. All rights reserved.
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
using System.Windows.Forms;
using MySQL.ForExcel.Classes;
using MySQL.ForExcel.Properties;
using MySQL.Utility.Forms;

namespace MySQL.ForExcel.Forms
{
  /// <summary>
  /// Advanced options dialog for the operations performed by the <see cref="ExportDataForm"/>.
  /// </summary>
  public partial class ExportAdvancedOptionsDialog : AutoStyleableBaseDialog
  {
    /// <summary>
    /// Initializes a new instance of the <see cref="ExportAdvancedOptionsDialog"/> class.
    /// </summary>
    public ExportAdvancedOptionsDialog()
    {
      InitializeComponent();
      RefreshControlValues(false);
    }

    #region Properties

    /// <summary>
    /// Gets a value indicating whether the parent form requires to refresh its data grid view control.
    /// </summary>
    public bool ParentFormRequiresRefresh { get; private set; }

    /// <summary>
    /// Gets a value indicating whether the auto-detect datatypes setting was changed by the user.
    /// </summary>
    public bool ExportDetectDatatypeChanged { get; private set; }

    /// <summary>
    /// Gets a value indicating whether the setting to show all MySQL data types in the Data Type drop-down list was changed by the user.
    /// </summary>
    public bool ExportShowAllMySqlDataTypesChanged { get; private set; }

    #endregion Properties

    /// <summary>
    /// Event delegate method fired when the <see cref="AutoIndexIntColumnsCheckBox"/> checked state changes.
    /// </summary>
    /// <param name="sender">Sender object.</param>
    /// <param name="e">Event arguments.</param>
    private void AutoIndexIntColumnsCheckBox_CheckedChanged(object sender, EventArgs e)
    {
      RecalculateParentFormRequiresRefresh();
    }

    /// <summary>
    /// Event delegate method fired when the <see cref="AutoAllowEmptyNonIndexColumnsCheckBox"/> checked state changes.
    /// </summary>
    /// <param name="sender">Sender object.</param>
    /// <param name="e">Event arguments.</param>
    private void AutoAllowEmptyNonIndexColumnsCheckBox_CheckedChanged(object sender, EventArgs e)
    {
      RecalculateParentFormRequiresRefresh();
    }

    /// <summary>
    /// Event delegate method fired when the <see cref="DetectDatatypeCheckBox"/> checked state changes.
    /// </summary>
    /// <param name="sender">Sender object.</param>
    /// <param name="e">Event arguments.</param>
    private void DetectDatatypeCheckBox_CheckedChanged(object sender, EventArgs e)
    {
      AddBufferToVarCharCheckBox.Enabled = DetectDatatypeCheckBox.Checked;
      if (!DetectDatatypeCheckBox.Checked)
      {
        AddBufferToVarCharCheckBox.Checked = false;
      }

      ExportDetectDatatypeChanged = Settings.Default.ExportDetectDatatype != DetectDatatypeCheckBox.Checked;
      RecalculateParentFormRequiresRefresh();
    }

    /// <summary>
    /// Event delegate method fired when the <see cref="ImportAdvancedOptionsDialog"/> is being closed.
    /// </summary>
    /// <param name="sender">Sender object.</param>
    /// <param name="e">Event arguments.</param>
    private void ExportAdvancedOptionsDialog_FormClosing(object sender, FormClosingEventArgs e)
    {
      if (DialogResult == DialogResult.Cancel)
      {
        return;
      }

      Settings.Default.ExportLimitPreviewRowsQuantity = (int)PreviewRowsQuantityNumericUpDown.Value;
      Settings.Default.ExportDetectDatatype = DetectDatatypeCheckBox.Checked;
      Settings.Default.ExportAddBufferToVarchar = AddBufferToVarCharCheckBox.Checked;
      Settings.Default.ExportAutoIndexIntColumns = AutoIndexIntColumnsCheckBox.Checked;
      Settings.Default.ExportAutoAllowEmptyNonIndexColumns = AutoAllowEmptyNonIndexColumnsCheckBox.Checked;
      Settings.Default.ExportShowAllMySqlDataTypes = ShowAllDataTypesCheckBox.Checked;
      Settings.Default.ExportUseFormattedValues = UseFormattedValuesCheckBox.Checked;
      Settings.Default.ExportSqlQueriesCreateIndexesLast = CreateTableIndexesLastCheckBox.Checked;
      Settings.Default.ExportGenerateMultipleInserts = GenerateMultipleInsertsCheckBox.Checked;
      MiscUtilities.SaveSettings();
    }

    /// <summary>
    /// Event delegate method fired when the <see cref="GenerateMultipleInsertsCheckBox"/> checked state changes.
    /// </summary>
    /// <param name="sender">Sender object.</param>
    /// <param name="e">Event arguments.</param>
    private void GenerateMultipleInsertsCheckBox_CheckedChanged(object sender, EventArgs e)
    {
      CreateTableIndexesLastCheckBox.Enabled = GenerateMultipleInsertsCheckBox.Checked;
      if (!GenerateMultipleInsertsCheckBox.Checked)
      {
        CreateTableIndexesLastCheckBox.Checked = false;
      }
    }

    /// <summary>
    /// Event delegate method fired when the <see cref="PreviewRowsQuantityNumericUpDown"/> value changes.
    /// </summary>
    /// <param name="sender">Sender object.</param>
    /// <param name="e">Event arguments.</param>
    private void PreviewRowsQuantityNumericUpDown_ValueChanged(object sender, EventArgs e)
    {
      RecalculateParentFormRequiresRefresh();
    }

    /// <summary>
    /// Recalculates the value of the <see cref="ParentFormRequiresRefresh"/> property.
    /// </summary>
    /// <returns>The recalculated value of the <see cref="ParentFormRequiresRefresh"/> property.</returns>
    private void RecalculateParentFormRequiresRefresh()
    {
      ParentFormRequiresRefresh = ExportDetectDatatypeChanged
                                  || Settings.Default.ExportLimitPreviewRowsQuantity != (int)PreviewRowsQuantityNumericUpDown.Value
                                  || Settings.Default.ExportAutoIndexIntColumns != AutoIndexIntColumnsCheckBox.Checked
                                  || Settings.Default.ExportAutoAllowEmptyNonIndexColumns != AutoAllowEmptyNonIndexColumnsCheckBox.Checked
                                  || Settings.Default.ExportUseFormattedValues != UseFormattedValuesCheckBox.Checked;
      SetWarningControlsVisibility();
    }

    /// <summary>
    /// Refreshes the dialog controls' values.
    /// </summary>
    /// <param name="useDefaultValues">Controls are set to their default values if <c>true</c>. Current stored values in application settings are used otherwise.</param>
    private void RefreshControlValues(bool useDefaultValues)
    {
      if (useDefaultValues)
      {
        var settings = Settings.Default;
        PreviewRowsQuantityNumericUpDown.Value = settings.GetPropertyDefaultValueByName<int>("ExportLimitPreviewRowsQuantity");
        DetectDatatypeCheckBox.Checked = settings.GetPropertyDefaultValueByName<bool>("ExportDetectDatatype");
        AddBufferToVarCharCheckBox.Checked = settings.GetPropertyDefaultValueByName<bool>("ExportAddBufferToVarchar");
        AutoIndexIntColumnsCheckBox.Checked = settings.GetPropertyDefaultValueByName<bool>("ExportAutoIndexIntColumns");
        AutoAllowEmptyNonIndexColumnsCheckBox.Checked = settings.GetPropertyDefaultValueByName<bool>("ExportAutoAllowEmptyNonIndexColumns");
        ShowAllDataTypesCheckBox.Checked = settings.GetPropertyDefaultValueByName<bool>("ExportShowAllMySqlDataTypes");
        UseFormattedValuesCheckBox.Checked = settings.GetPropertyDefaultValueByName<bool>("ExportUseFormattedValues");
        CreateTableIndexesLastCheckBox.Checked = settings.GetPropertyDefaultValueByName<bool>("ExportSqlQueriesCreateIndexesLast");
        GenerateMultipleInsertsCheckBox.Checked = settings.GetPropertyDefaultValueByName<bool>("ExportGenerateMultipleInserts");
      }
      else
      {
        ExportDetectDatatypeChanged = false;
        ParentFormRequiresRefresh = false;
        PreviewRowsQuantityNumericUpDown.Value = Math.Min(PreviewRowsQuantityNumericUpDown.Maximum, Settings.Default.ExportLimitPreviewRowsQuantity);
        DetectDatatypeCheckBox.Checked = Settings.Default.ExportDetectDatatype;
        AddBufferToVarCharCheckBox.Checked = Settings.Default.ExportAddBufferToVarchar;
        AutoIndexIntColumnsCheckBox.Checked = Settings.Default.ExportAutoIndexIntColumns;
        AutoAllowEmptyNonIndexColumnsCheckBox.Checked = Settings.Default.ExportAutoAllowEmptyNonIndexColumns;
        ShowAllDataTypesCheckBox.Checked = Settings.Default.ExportShowAllMySqlDataTypes;
        UseFormattedValuesCheckBox.Checked = Settings.Default.ExportUseFormattedValues;
        CreateTableIndexesLastCheckBox.Checked = Settings.Default.ExportSqlQueriesCreateIndexesLast;
        GenerateMultipleInsertsCheckBox.Checked = Settings.Default.ExportGenerateMultipleInserts;
      }

      AddBufferToVarCharCheckBox.Enabled = DetectDatatypeCheckBox.Checked;
      CreateTableIndexesLastCheckBox.Enabled = GenerateMultipleInsertsCheckBox.Checked;
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
    /// Sets the visibility of the controls depicting a warning about column options changes being lost.
    /// </summary>
    private void SetWarningControlsVisibility()
    {
      ColumnOptionsLostWarningLabel.Visible = ParentFormRequiresRefresh;
      ColumnOptionsLostWarningPictureBox.Visible = ParentFormRequiresRefresh;
    }

    /// <summary>
    /// Event delegate method fired when the <see cref="ShowAllDataTypesCheckBox"/> checked state changes.
    /// </summary>
    /// <param name="sender">Sender object.</param>
    /// <param name="e">Event arguments.</param>
    private void ShowAllDataTypesCheckBox_CheckedChanged(object sender, EventArgs e)
    {
      ExportShowAllMySqlDataTypesChanged = ShowAllDataTypesCheckBox.Checked != Settings.Default.ExportShowAllMySqlDataTypes;
    }

    /// <summary>
    /// Event delegate method fired when the <see cref="UseFormattedValuesCheckBox"/> checked state changes.
    /// </summary>
    /// <param name="sender">Sender object.</param>
    /// <param name="e">Event arguments.</param>
    private void UseFormattedValuesCheckBox_CheckedChanged(object sender, EventArgs e)
    {
      RecalculateParentFormRequiresRefresh();
    }
  }
}