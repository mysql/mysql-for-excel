// 
// Copyright (c) 2012, Oracle and/or its affiliates. All rights reserved.
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

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace MySQL.ForExcel
{
  public partial class ExportAdvancedOptionsDialog : AutoStyleableBaseDialog
  {
    public ExportAdvancedOptionsDialog()
    {
      InitializeComponent();

      numPreviewRowsQuantity.Value = Math.Min(numPreviewRowsQuantity.Maximum, Properties.Settings.Default.ExportLimitPreviewRowsQuantity);
      chkDetectDatatype.Checked = Properties.Settings.Default.ExportDetectDatatype;
      chkAddBufferToVarchar.Checked = Properties.Settings.Default.ExportAddBufferToVarchar;
      chkAutoIndexIntColumns.Checked = Properties.Settings.Default.ExportAutoIndexIntColumns;
      chkAutoAllowEmptyNonIndexColumns.Checked = Properties.Settings.Default.ExportAutoAllowEmptyNonIndexColumns;
      chkUseFormattedValues.Checked = Properties.Settings.Default.ExportUseFormattedValues;
      chkRemoveEmptyColumns.Checked = Properties.Settings.Default.ExportRemoveEmptyColumns;
      //chkShowCopySQLButton.Checked = Properties.Settings.Default.ExportShowCopySQLButton;

      chkAddBufferToVarchar.Enabled = chkDetectDatatype.Checked;
    }

    private void btnAccept_Click(object sender, EventArgs e)
    {
      Properties.Settings.Default.ExportLimitPreviewRowsQuantity = (int)numPreviewRowsQuantity.Value;
      Properties.Settings.Default.ExportDetectDatatype = chkDetectDatatype.Checked;
      Properties.Settings.Default.ExportAddBufferToVarchar = chkAddBufferToVarchar.Checked;
      Properties.Settings.Default.ExportAutoIndexIntColumns = chkAutoIndexIntColumns.Checked;
      Properties.Settings.Default.ExportAutoAllowEmptyNonIndexColumns = chkAutoAllowEmptyNonIndexColumns.Checked;
      Properties.Settings.Default.ExportUseFormattedValues = chkUseFormattedValues.Checked;
      Properties.Settings.Default.ExportRemoveEmptyColumns = chkRemoveEmptyColumns.Checked;
      //Properties.Settings.Default.ExportShowCopySQLButton = chkShowCopySQLButton.Checked;
      MiscUtilities.SaveSettings();
      DialogResult = DialogResult.OK;
      Close();
    }

    private void chkDetectDatatype_CheckedChanged(object sender, EventArgs e)
    {
      chkAddBufferToVarchar.Enabled = chkDetectDatatype.Checked;
      if (!chkDetectDatatype.Checked)
        chkAddBufferToVarchar.Checked = false;
    }

  }
}
