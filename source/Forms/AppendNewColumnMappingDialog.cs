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
  using MySQL.Utility.Forms;

  /// <summary>
  /// Provides an interface to let users input a column mapping name that will be saved to file.
  /// </summary>
  public partial class AppendNewColumnMappingDialog : AutoStyleableBaseDialog
  {
    /// <summary>
    /// Initializes a new instance of the <see cref="AppendNewColumnMappingDialog"/> class.
    /// </summary>
    /// <param name="proposedMappingName">The name of the column mapping proposed by the system.</param>
    public AppendNewColumnMappingDialog(string proposedMappingName)
    {
      InitializeComponent();
      MappingNameTextBox.Text = proposedMappingName;
      MappingNameTextBox.SelectAll();
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="AppendNewColumnMappingDialog"/> class.
    /// </summary>
    public AppendNewColumnMappingDialog()
      : this(string.Empty)
    {
    }

    /// <summary>
    /// Gets or sets the name of the column mapping that will be saved to file.
    /// </summary>
    public string ColumnMappingName
    {
      get
      {
        return MappingNameTextBox.Text.Trim();
      }

      set
      {
        MappingNameTextBox.Text = value;
      }
    }

    /// <summary>
    /// Event delegate method fired when the text in the <see cref="MappingNameTextBox"/> changes.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void MappingNameTextBox_TextChanged(object sender, EventArgs e)
    {
      OKButton.Enabled = ColumnMappingName.Length > 0;
    }
  }
}