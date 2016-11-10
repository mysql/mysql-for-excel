// Copyright (c) 2012, 2013, Oracle and/or its affiliates. All rights reserved.
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
using MySql.Utility.Forms;

namespace MySQL.ForExcel.Forms
{
  /// <summary>
  /// Offers users options to undo changes to data edition by reverting changes or fetching an updated copy of the data from the database.
  /// </summary>
  public partial class EditDataRevertDialog : AutoStyleableBaseDialog
  {
    /// <summary>
    /// Initializes a new instance of the <see cref="EditDataRevertDialog"/> class.
    /// </summary>
    /// <param name="enableRevert">Flag indicating whether the revert data checkbox is available for selection.</param>
    public EditDataRevertDialog(bool enableRevert)
    {
      InitializeComponent();
      RevertDataButton.Enabled = enableRevert;
    }

    /// <summary>
    /// Indicates the type of action for the undo operation.
    /// </summary>
    public enum EditUndoAction
    {
      /// <summary>
      /// An updated copy of the data is to be fetched from the database after changes are undone.
      /// </summary>
      RefreshData,

      /// <summary>
      /// Existing changes are reverted to the state when the data was retrieved for edition.
      /// </summary>
      RevertData
    }

    /// <summary>
    /// Gets the undo action selected by the user.
    /// </summary>
    public EditUndoAction SelectedAction { get; private set; }

    /// <summary>
    /// Event delegate method fired when the <see cref="RefreshDataButton"/> button is clicked.
    /// </summary>
    /// <param name="sender">Sender object.</param>
    /// <param name="e">Event arguments.</param>
    private void RefreshDataButton_Click(object sender, EventArgs e)
    {
      SelectedAction = EditUndoAction.RefreshData;
    }

    /// <summary>
    /// Event delegate method fired when the <see cref="RevertDataButton"/> button is clicked.
    /// </summary>
    /// <param name="sender">Sender object.</param>
    /// <param name="e">Event arguments.</param>
    private void RevertDataButton_Click(object sender, EventArgs e)
    {
      SelectedAction = EditUndoAction.RevertData;
    }
  }
}