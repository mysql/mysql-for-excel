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
  public partial class GlobalOptionsDialog : AutoStyleableBaseDialog
  {
    /// <summary>
    /// Initializes a new instance of the <see cref="GlobalOptionsDialog"/> class.
    /// </summary>
    public GlobalOptionsDialog()
    {
      InitializeComponent();

      ConnectionTimeoutNumericUpDown.Maximum = Int32.MaxValue / 1000;
      ConnectionTimeoutNumericUpDown.Value = Math.Min(ConnectionTimeoutNumericUpDown.Maximum, Settings.Default.GlobalConnectionConnectionTimeout);
      QueryTimeoutNumericUpDown.Value = Settings.Default.GlobalConnectionCommandTimeout;
      UseOptimisticUpdatesCheckBox.Checked = Settings.Default.EditUseOptimisticUpdate;
      PreviewSqlQueriesCheckBox.Checked = Settings.Default.GlobalSqlQueriesPreviewQueries;
      ShowExecutedSqlQueryCheckBox.Checked = Settings.Default.GlobalSqlQueriesShowQueriesWithResults;
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

      Settings.Default.GlobalConnectionConnectionTimeout = (uint)ConnectionTimeoutNumericUpDown.Value;
      Settings.Default.GlobalConnectionCommandTimeout = (uint)QueryTimeoutNumericUpDown.Value;
      Settings.Default.EditUseOptimisticUpdate = UseOptimisticUpdatesCheckBox.Checked;
      Settings.Default.GlobalSqlQueriesPreviewQueries = PreviewSqlQueriesCheckBox.Checked;
      Settings.Default.GlobalSqlQueriesShowQueriesWithResults = ShowExecutedSqlQueryCheckBox.Checked;
      MiscUtilities.SaveSettings();
    }

    /// <summary>
    /// Event delegate method fired when the <see cref="PreviewSqlQueriesCheckBox"/> checked state is changed.
    /// </summary>
    /// <param name="sender">Sender object.</param>
    /// <param name="e">Event arguments.</param>
    private void PreviewSqlQueriesCheckBox_CheckedChanged(object sender, EventArgs e)
    {
      if (PreviewSqlQueriesCheckBox.Checked)
      {
        ShowExecutedSqlQueryCheckBox.Checked = false;
      }
    }

    /// <summary>
    /// Event delegate method fired when the <see cref="PreviewSqlQueriesCheckBox"/> checked state is changed.
    /// </summary>
    /// <param name="sender">Sender object.</param>
    /// <param name="e">Event arguments.</param>
    private void ShowExecutedSqlQueryCheckBox_CheckedChanged(object sender, EventArgs e)
    {
      if (ShowExecutedSqlQueryCheckBox.Checked)
      {
        PreviewSqlQueriesCheckBox.Checked = false;
      }
    }
  }
}