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
using System.Reflection;
using System.Windows.Forms;
using MySQL.Utility.Classes;
using MySQL.Utility.Forms;

namespace MySQL.ForExcel.Forms
{
  /// <summary>
  /// Represents an About modal box for this project
  /// </summary>
  public partial class AboutBox : AutoStyleableBaseForm
  {
    /// <summary>
    /// Initializes a new instance of the <see cref="AboutBox"/> class
    /// </summary>
    public AboutBox()
    {
      InitializeComponent();
      ExcelVersionLabel.Text = string.Format("{0} {1}.{2}.{3}", AssemblyInfo.AssemblyTitle, Version[0], Version[1], Version[2]);
    }

    /// <summary>
    /// Gets the executing assembly version splitted in the sub-versions as an array.
    /// </summary>
    public string[] Version
    {
      get
      {
        return Assembly.GetExecutingAssembly().GetName().Version.ToString().Split('.');
      }
    }

    /// <summary>
    /// Event delegate method fired when the About box is clicked on
    /// </summary>
    /// <param name="sender">Sender object</param>
    /// <param name="e">Event arguments</param>
    private void AboutBox_Click(object sender, EventArgs e)
    {
      Close();
    }

    /// <summary>
    /// Event delegate method fired when a key is down
    /// </summary>
    /// <param name="sender">Sender object</param>
    /// <param name="e">Event arguments</param>
    private void AboutBox_KeyDown(object sender, KeyEventArgs e)
    {
      if (e.KeyCode == Keys.Escape)
      {
        Close();
      }
    }
  }
}