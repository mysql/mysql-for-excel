// 
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
//

namespace MySQL.ForExcel
{
  using System;
  using System.Windows.Forms;
  using MySQL.Utility.Forms;

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

      PreviewRowsQuantityNumericUpDown.Value = Math.Min(PreviewRowsQuantityNumericUpDown.Maximum, Properties.Settings.Default.ExportLimitPreviewRowsQuantity);
      DetectDatatypeCheckBox.Checked = Properties.Settings.Default.ExportDetectDatatype;
      AddBufferToVarcharCheckBox.Checked = Properties.Settings.Default.ExportAddBufferToVarchar;
      AutoIndexIntColumnsCheckBox.Checked = Properties.Settings.Default.ExportAutoIndexIntColumns;
      AutoAllowEmptyNonIndexColumnsCheckBox.Checked = Properties.Settings.Default.ExportAutoAllowEmptyNonIndexColumns;
      UseFormattedValuesCheckBox.Checked = Properties.Settings.Default.ExportUseFormattedValues;
      RemoveEmptyColumnsCheckBox.Checked = Properties.Settings.Default.ExportRemoveEmptyColumns;
      //chkShowCopySQLButton.Checked = Properties.Settings.Default.ExportShowCopySQLButton;
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
    /// Event delegate method fired when the <see cref="DialogAcceptButton"/> button is clicked.
    /// </summary>
    /// <param name="sender">Sender object.</param>
    /// <param name="e">Event arguments.</param>
    private void DialogAcceptButton_Click(object sender, EventArgs e)
    {
      Properties.Settings.Default.ExportLimitPreviewRowsQuantity = (int)PreviewRowsQuantityNumericUpDown.Value;
      Properties.Settings.Default.ExportDetectDatatype = DetectDatatypeCheckBox.Checked;
      Properties.Settings.Default.ExportAddBufferToVarchar = AddBufferToVarcharCheckBox.Checked;
      Properties.Settings.Default.ExportAutoIndexIntColumns = AutoIndexIntColumnsCheckBox.Checked;
      Properties.Settings.Default.ExportAutoAllowEmptyNonIndexColumns = AutoAllowEmptyNonIndexColumnsCheckBox.Checked;
      Properties.Settings.Default.ExportUseFormattedValues = UseFormattedValuesCheckBox.Checked;
      Properties.Settings.Default.ExportRemoveEmptyColumns = RemoveEmptyColumnsCheckBox.Checked;
      //Properties.Settings.Default.ExportShowCopySQLButton = chkShowCopySQLButton.Checked;
      MiscUtilities.SaveSettings();
      DialogResult = DialogResult.OK;
      Close();
    }
  }
}