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
  using MySQL.Utility;
  using MySQL.Utility.Forms;

  /// <summary>
  /// Provides an interface to enter the password required by a MySQL connection.
  /// </summary>
  public partial class PasswordDialog : AutoStyleableBaseDialog
  {
    /// <summary>
    /// Flag indicating whether the connection is tested after setting the password.
    /// </summary>
    private bool _testConnection;

    /// <summary>
    /// Initializes a new instance of the <see cref="PasswordDialog"/> class.
    /// </summary>
    /// <param name="wbConnection">A <see cref="MySqlWorkbenchConnection"/> object representing the connection to a MySQL server instance selected by users.</param>
    /// <param name="testConnection">Flag indicating whether the connection is tested after setting the password.</param>
    public PasswordDialog(MySqlWorkbenchConnection wbConnection, bool testConnection)
    {
      _testConnection = testConnection;
      InitializeComponent();
      WBConnection = wbConnection;
      UserValueLabel.Text = WBConnection.UserName;
      ConnectionValueLabel.Text = WBConnection.Name + " - " + WBConnection.HostIdentifier;
      PasswordTextBox.Text = WBConnection.Password;
      DialogOKButton.Enabled = PasswordTextBox.Text.Trim().Length > 0;
    }

    /// <summary>
    /// Gets a value indicating whether the password is saved in the password vault.
    /// </summary>
    public bool StorePasswordSecurely
    {
      get
      {
        return StorePasswordSecurelyCheckBox.Checked;
      }

      private set
      {
        StorePasswordSecurelyCheckBox.Checked = value;
      }
    }

    /// <summary>
    /// Gets the connection to a MySQL server instance selected by users
    /// </summary>
    public MySqlWorkbenchConnection WBConnection { get; private set; }

    private void DialogOKButton_Click(object sender, System.EventArgs e)
    {
      WBConnection.Password = PasswordTextBox.Text;
      bool connectionSuccessful = true;
      if (_testConnection)
      {
        bool wrongPassword = false;
        connectionSuccessful = WBConnection.TestConnectionAndShowError(out wrongPassword);
      }

      if (StorePasswordSecurely && !string.IsNullOrEmpty(WBConnection.Password) && connectionSuccessful)
      {
        MySqlWorkbenchPasswordVault.StorePassword(WBConnection.HostIdentifier, WBConnection.UserName, WBConnection.Password);
      }
    }

    /// <summary>
    /// Event delegate method fired when the <see cref="PasswordChangedTimer"/> timer elapses.
    /// </summary>
    /// <param name="sender">Sender object.</param>
    /// <param name="e">Event arguments.</param>
    private void PasswordChangedTimer_Tick(object sender, EventArgs e)
    {
      PasswordTextBox_Validated(PasswordTextBox, EventArgs.Empty);
    }

    /// <summary>
    /// Event delegate method fired when the <see cref="PasswordTextBox"/> text changes.
    /// </summary>
    /// <param name="sender">Sender object.</param>
    /// <param name="e">Event arguments.</param>
    private void PasswordTextBox_TextChanged(object sender, EventArgs e)
    {
      PasswordChangedTimer.Stop();
      PasswordChangedTimer.Start();
    }

    /// <summary>
    /// Event delegate method fired when the <see cref="PasswordTextBox"/> is validated.
    /// </summary>
    /// <param name="sender">Sender object.</param>
    /// <param name="e">Event arguments.</param>
    private void PasswordTextBox_Validated(object sender, EventArgs e)
    {
      PasswordChangedTimer.Stop();
      PasswordTextBox.Text = PasswordTextBox.Text.Trim();
      DialogOKButton.Enabled = PasswordTextBox.TextLength > 0;
    }
  }
}