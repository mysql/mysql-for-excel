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
  public partial class AppendNewColumnMappingDialog : AutoStyleableBaseDialog
  {
    public string ColumnMappingName
    {
      get { return txtMappingName.Text.Trim(); }
      set { txtMappingName.Text = value; }
    }

    public AppendNewColumnMappingDialog(string proposedMappingName)
    {
      InitializeComponent();
      txtMappingName.Text = proposedMappingName;
      txtMappingName.SelectAll();
    }

    public AppendNewColumnMappingDialog() : this(String.Empty)
    {
    }

    private void txtMappingName_TextChanged(object sender, EventArgs e)
    {
      btnOK.Enabled = ColumnMappingName.Length > 0;
    }
  }
}
