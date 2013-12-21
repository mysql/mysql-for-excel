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
using MySQL.Utility.Forms;

namespace MySQL.ForExcel.Forms
{
  /// <summary>
  /// Offers users options to delete, keep for later or reopen saved edit sessions in the current workbook.
  /// </summary>
  public partial class RestoreEditSessionsDialog : AutoStyleableBaseDialog
  {
    /// <summary>
    /// Initializes a new instance of the <see cref="RestoreEditSessionsDialog"/> class.
    /// </summary>
    public RestoreEditSessionsDialog()
    {
      InitializeComponent();
    }

    /// <summary>
    /// Shows the <see cref="RestoreEditSessionsDialog"/>.
    /// </summary>
    /// <returns>One of the <see cref="DialogResult"/> values.</returns>
    public static DialogResult ShowAndDispose()
    {
      DialogResult dr;
      using (var openEditSessionsDialog = new RestoreEditSessionsDialog())
      {
        dr = openEditSessionsDialog.ShowDialog();
      }

      return dr;
    }

    /// <summary>
    /// Handles the event when the user clicks the Discard button.
    /// </summary>
    /// <param name="sender">The source of the event.</param>
    /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
    private void DiscardButton_Click(object sender, EventArgs e)
    {
      DialogResult = DialogResult.Abort;
    }

    /// <summary>
    /// Handles the event when the user clicks the Persist button.
    /// </summary>
    /// <param name="sender">The source of the event.</param>
    /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
    private void PersistButton_Click(object sender, EventArgs e)
    {
      DialogResult = DialogResult.No;
    }

    /// <summary>
    /// Handles the event when the user clicks the Open button.
    /// </summary>
    /// <param name="sender">The source of the event.</param>
    /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
    private void OpenButton_Click(object sender, EventArgs e)
    {
      DialogResult = DialogResult.Yes;
    }
  }
}