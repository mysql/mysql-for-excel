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
  using MySQL.Utility.Forms;

  /// <summary>
  /// Provides an interface to enter the password required by a MySQL connection.
  /// </summary>
  public partial class PasswordDialog : AutoStyleableBaseDialog
  {
    /// <summary>
    /// Initializes a new instance of the <see cref="PasswordDialog"/> class.
    /// </summary>
    /// <param name="hostIdentifier">The complete identifier of the MySQL host of the connection (instance name + host name + port).</param>
    /// <param name="user">The user name used by the connection.</param>
    public PasswordDialog(string hostIdentifier, string user)
    {
      InitializeComponent();
      HostIdentifier = hostIdentifier;
      UserName = user;
    }

    /// <summary>
    /// Gets the complete identifier of the MySQL host of the connection (instance name + host name + port).
    /// </summary>
    public string HostIdentifier
    {
      get
      {
        return ConnectionValueLabel.Text;
      }

      private set
      {
        ConnectionValueLabel.Text = value;
      }
    }

    /// <summary>
    /// Gets the password entered by the user for the connection.
    /// </summary>
    public string PasswordText
    {
      get
      {
        return PasswordTextBox.Text;
      }

      private set
      {
        PasswordTextBox.Text = value;
      }
    }

    /// <summary>
    /// Gets the user name used by the connection.
    /// </summary>
    public string UserName
    {
      get
      {
        return UserValueLabel.Text;
      }

      private set
      {
        UserValueLabel.Text = value;
      }
    }
  }
}