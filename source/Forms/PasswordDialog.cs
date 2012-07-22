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
using MySQL.Utility;

namespace MySQL.ForExcel
{
  public partial class PasswordDialog : AutoStyleableBaseDialog
  {
    public string PasswordText
    {
      set { txtPassword.Text = value; }
      get { return txtPassword.Text; }
    }

    public string HostIdentifier
    {
      set { lblConnectionValue.Text = value; }
    }

    public string UserName
    {
      set { lblUserValue.Text = value; }
    }

    public PasswordDialog()
    {
      InitializeComponent();
    }

    public PasswordDialog(string Host, int Port, string User)
    {
      InitializeComponent();
      lblConnection.Text = "Service:";
      HostIdentifier = "Mysql@" + Host + ":" + Port;
      UserName = User ?? "guest";
    }
  }
}
