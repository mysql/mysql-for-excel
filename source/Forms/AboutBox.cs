//
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
//

namespace MySQL.ForExcel
{
  using System;
  using System.Collections.Generic;
  using System.ComponentModel;
  using System.Diagnostics;
  using System.Drawing;
  using System.Linq;
  using System.Reflection;
  using System.Windows.Forms;

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

      lblExcelVersion.Text = String.Format(Properties.Resources.AboutMySQLForExcelVersion, AssemblyVersion);
      lblInstallerVersion.Text = Properties.Resources.AboutMySQLInstallerVersion;
    }

    /// <summary>
    /// Gets the AssemblyVersion property value stored in the AssemblyInfo.cs file
    /// </summary>
    public string AssemblyVersion
    {
      get
      {
        Assembly asm = Assembly.GetExecutingAssembly();
        FileVersionInfo fvi = FileVersionInfo.GetVersionInfo(asm.Location);
        return String.Format("{0}.{1}.{2}", fvi.ProductMajorPart, fvi.ProductMinorPart, fvi.ProductBuildPart);
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
