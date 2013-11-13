// Copyright (c) 2012-2013, Oracle and/or its affiliates. All rights reserved.
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
    /// Gets a value indicating whether the parent form requires to refresh its data grid view control.
    /// </summary>
    public bool ParentFormRequiresRefresh { get; private set; }

    /// <summary>
    /// Gets a value indicating whether the auto-detect datatypes setting was changed by the user.
    /// </summary>
    public bool ExportDetectDatatypeChanged { get; private set; }

    /// <summary>
    /// Gets a value indicating whether the auto-remove empty columns setting changed by the user.
    /// </summary>
    public bool ExportRemoveEmptyColumnsChanged { get; private set; }

    /// <summary>
    /// Initializes a new instance of the <see cref="ExportAdvancedOptionsDialog"/> class.
    /// </summary>
    public ExportAdvancedOptionsDialog()
    {
      ParentFormRequiresRefresh = false;
      ExportDetectDatatypeChanged = false;
      ExportRemoveEmptyColumnsChanged = false;

      InitializeComponent();

      PreviewRowsQuantityNumericUpDown.Value = Math.Min(PreviewRowsQuantityNumericUpDown.Maximum, Settings.Default.ExportLimitPreviewRowsQuantity);
      DetectDatatypeCheckBox.Checked = Settings.Default.ExportDetectDatatype;
      AddBufferToVarcharCheckBox.Checked = Settings.Default.ExportAddBufferToVarchar;
      AutoIndexIntColumnsCheckBox.Checked = Settings.Default.ExportAutoIndexIntColumns;
      AutoAllowEmptyNonIndexColumnsCheckBox.Checked = Settings.Default.ExportAutoAllowEmptyNonIndexColumns;
      UseFormattedValuesCheckBox.Checked = Settings.Default.ExportUseFormattedValues;
      RemoveEmptyColumnsCheckBox.Checked = Settings.Default.ExportRemoveEmptyColumns;
      //chkShowCopySQLButton.Checked = Settings.Default.ExportShowCopySQLButton;
      AddBufferToVarcharCheckBox.Enabled = DetectDatatypeCheckBox.Checked;
    }

    /// <summary>
    /// Event delegate method fired when the <see cref="DetectDatatypeCheckBox"/> checkbox is checked.
    /// </summary>
    /// <param name="sender">Sender object.</param>
    /// <param name="e">Event arguments.</param>
    private void DetectDatatypeCheckBox_CheckedChanged(object sender, EventArgs e)
    {
      AddBufferToVarcharCheckBox.Enabled = DetectDatatypeCheckBox.Checked;
      if (!DetectDatatypeCheckBox.Checked)
      {
        AddBufferToVarcharCheckBox.Checked = false;
      }
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

      var previewRowsQuantity = (int)PreviewRowsQuantityNumericUpDown.Value;

      ExportDetectDatatypeChanged = Settings.Default.ExportDetectDatatype != DetectDatatypeCheckBox.Checked;
      ExportRemoveEmptyColumnsChanged = Settings.Default.ExportRemoveEmptyColumns != RemoveEmptyColumnsCheckBox.Checked;
      ParentFormRequiresRefresh = ExportDetectDatatypeChanged ||
                                  ExportRemoveEmptyColumnsChanged ||
                                  Settings.Default.ExportLimitPreviewRowsQuantity != previewRowsQuantity ||
                                  Settings.Default.ExportAutoIndexIntColumns != AutoIndexIntColumnsCheckBox.Checked ||
                                  Settings.Default.ExportAutoAllowEmptyNonIndexColumns != AutoAllowEmptyNonIndexColumnsCheckBox.Checked ||
                                  Settings.Default.ExportUseFormattedValues != UseFormattedValuesCheckBox.Checked;


      Settings.Default.ExportLimitPreviewRowsQuantity = previewRowsQuantity;
      Settings.Default.ExportDetectDatatype = DetectDatatypeCheckBox.Checked;
      Settings.Default.ExportAddBufferToVarchar = AddBufferToVarcharCheckBox.Checked;
      Settings.Default.ExportAutoIndexIntColumns = AutoIndexIntColumnsCheckBox.Checked;
      Settings.Default.ExportAutoAllowEmptyNonIndexColumns = AutoAllowEmptyNonIndexColumnsCheckBox.Checked;
      Settings.Default.ExportUseFormattedValues = UseFormattedValuesCheckBox.Checked;
      Settings.Default.ExportRemoveEmptyColumns = RemoveEmptyColumnsCheckBox.Checked;
      //Settings.Default.ExportShowCopySQLButton = chkShowCopySQLButton.Checked;
      MiscUtilities.SaveSettings();
    }
  }
}