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
  /// Lets users create a new schema in the connected MySQL Server instance.
  /// </summary>
  public partial class NewSchemaDialog : AutoStyleableBaseDialog
  {
    /// <summary>
    /// Initializes a new instance of the <see cref="NewSchemaDialog"/> class.
    /// </summary>
    public NewSchemaDialog()
    {
      InitializeComponent();
    }

    /// <summary>
    /// Gets or sets the name of the new schema.
    /// </summary>
    public string SchemaName
    {
      get
      {
        return SchemaNameTextBox.Text.Trim();
      }

      set
      {
        SchemaNameTextBox.Text = value;
      }
    }

    /// <summary>
    /// Event delegate method fired when the <see cref="SchemaNameTextBox"/> text changes.
    /// </summary>
    /// <param name="sender">Sender object.</param>
    /// <param name="e">Event arguments.</param>
    private void SchemaNameTextBox_TextChanged(object sender, EventArgs e)
    {
      DialogOKButton.Enabled = SchemaName.Length > 0;
    }
  }
}