// Copyright (c) 2013, Oracle and/or its affiliates. All rights reserved.
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

namespace MySQL.ForExcel
{
  using System;
  using System.Windows.Forms;
  using MySQL.Utility.Forms;

  /// <summary>
  /// Advanced options dialog for the operations performed by the <see cref="ExportDataForm"/>.
  /// </summary>
  public partial class GlobalOptionsDialog : AutoStyleableBaseDialog
  {
    /// <summary>
    /// Initializes a new instance of the <see cref="GlobalOptionsDialog"/> class.
    /// </summary>
    public GlobalOptionsDialog()
    {
      InitializeComponent();

      ConnectionTimeoutNumericUpDown.Maximum = Int32.MaxValue / 1000;
      ConnectionTimeoutNumericUpDown.Value = Math.Min(ConnectionTimeoutNumericUpDown.Maximum, Properties.Settings.Default.GlobalConnectionConnectionTimeout);
      QueryTimeoutNumericUpDown.Value = Properties.Settings.Default.GlobalConnectionCommandTimeout;
    }

    /// <summary>
    /// Event delegate method fired when the <see cref="DialogAcceptButton"/> button is clicked.
    /// </summary>
    /// <param name="sender">Sender object.</param>
    /// <param name="e">Event arguments.</param>
    private void DialogAcceptButton_Click(object sender, EventArgs e)
    {
      Properties.Settings.Default.GlobalConnectionConnectionTimeout = (uint)ConnectionTimeoutNumericUpDown.Value;
      Properties.Settings.Default.GlobalConnectionCommandTimeout = (uint)QueryTimeoutNumericUpDown.Value;
      MiscUtilities.SaveSettings();
      DialogResult = DialogResult.OK;
      Close();
    }
  }
}