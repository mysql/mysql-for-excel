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
using System.Drawing;
using System.Windows.Forms;
using MySQL.ForExcel.Classes;
using MySQL.Utility.Classes;
using MySQL.Utility.Classes.MySQLWorkbench;
using MySQL.Utility.Forms;

namespace MySQL.ForExcel.Forms
{
  /// <summary>
  /// Provides an interface to enter the password required by a MySQL connection.
  /// </summary>
  public partial class PasswordDialog : AutoStyleableBaseDialog
  {
    #region Constants

    /// <summary>
    /// The height in pixels of the dialog when used to enter a new password after an old one expired.
    /// </summary>
    public const int EXPANDED_DIALOG_HEIGHT = 325;

    /// <summary>
    /// The height in pixels of the dialog when used to ask for a connection's password.
    /// </summary>
    public const int REGULAR_DIALOG_HEIGHT = 255;

    /// <summary>
    /// The vertical space in pixels the top password label is shifted if the regular dialog is used.
    /// </summary>
    public const int TOP_LABEL_VERTICAL_DELTA = 5;

    #endregion Constants

    #region Fields

    /// <summary>
    /// Containins data about the password operation.
    /// </summary>
    private PasswordDialogFlags _passwordFlags;

    /// <summary>
    /// Flag indicating whether the connection is tested after setting the password.
    /// </summary>
    private bool _testConnection;

    #endregion Fields

    /// <summary>
    /// Initializes a new instance of the <see cref="PasswordDialog"/> class.
    /// </summary>
    /// <param name="wbConnection">A <see cref="MySqlWorkbenchConnection"/> object representing the connection to a MySQL server instance selected by users.</param>
    /// <param name="testConnection">Flag indicating whether the connection is tested after setting the password.</param>
    /// <param name="passwordExpired">Flag indicating if the dialog will be used to set a new password when an old one expired.</param>
    public PasswordDialog(MySqlWorkbenchConnection wbConnection, bool testConnection, bool passwordExpired)
    {
      _testConnection = testConnection;
      _passwordFlags = new PasswordDialogFlags(wbConnection);
      InitializeComponent();
      PasswordExpiredDialog = passwordExpired;
      WbConnection = wbConnection;
      UserValueLabel.Text = WbConnection.UserName;
      ConnectionValueLabel.Text = WbConnection.Name + @" - " + WbConnection.HostIdentifier;
      PasswordTextBox.Text = WbConnection.Password;
      SetDialogInterface();
    }

    #region Properties

    /// <summary>
    /// Gets a value indicating whether the dialog will be used to set a new password when an old one expired.
    /// </summary>
    public bool PasswordExpiredDialog { get; private set; }

    /// <summary>
    /// Gets a structure with data about the password operation.
    /// </summary>
    public PasswordDialogFlags PasswordFlags
    {
      get
      {
        return _passwordFlags;
      }
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
    }

    /// <summary>
    /// Gets the connection to a MySQL server instance selected by users
    /// </summary>
    public MySqlWorkbenchConnection WbConnection { get; private set; }

    #endregion Properties

    /// <summary>
    /// Shows the connection password dialog to users and returns the entered password.
    /// </summary>
    /// <param name="wbConnection">A <see cref="MySqlWorkbenchConnection"/> object representing the connection to a MySQL server instance selected by users.</param>
    /// <param name="testConnection">Flag indicating whether the connection is tested after setting the password.</param>
    /// <returns>A <see cref="PasswordDialogFlags"/> containing data about the operation.</returns>
    public static PasswordDialogFlags ShowConnectionPasswordDialog(MySqlWorkbenchConnection wbConnection, bool testConnection)
    {
      PasswordDialogFlags flags;
      using (PasswordDialog connectionPasswordDialog = new PasswordDialog(wbConnection, testConnection, false))
      {
        connectionPasswordDialog.ShowDialog();
        flags = connectionPasswordDialog.PasswordFlags;
      }

      return flags;
    }

    /// <summary>
    /// Shows the connection password dialog to users and returns the entered password.
    /// </summary>
    /// <param name="wbConnection">A <see cref="MySqlWorkbenchConnection"/> object representing the connection to a MySQL server instance selected by users.</param>
    /// <param name="testConnection">Flag indicating whether the connection is tested after setting the password.</param>
    /// <returns>A <see cref="PasswordDialogFlags"/> containing data about the operation.</returns>
    public static PasswordDialogFlags ShowExpiredPasswordDialog(MySqlWorkbenchConnection wbConnection, bool testConnection)
    {
      PasswordDialogFlags flags;
      using (PasswordDialog connectionPasswordDialog = new PasswordDialog(wbConnection, testConnection, true))
      {
        connectionPasswordDialog.ShowDialog();
        flags = connectionPasswordDialog.PasswordFlags;
      }

      return flags;
    }

    /// <summary>
    /// Event delegate method fired when the <see cref="PasswordChangedTimer"/> timer elapses.
    /// </summary>
    /// <param name="sender">Sender object.</param>
    /// <param name="e">Event arguments.</param>
    private void PasswordChangedTimer_Tick(object sender, EventArgs e)
    {
      TextBox passwordTextBox = null;
      if (PasswordTextBox.Focused)
      {
        passwordTextBox = PasswordTextBox;
      }
      else if (NewPasswordTextBox.Focused)
      {
        passwordTextBox = NewPasswordTextBox;
      }
      else if (ConfirmTextBox.Focused)
      {
        passwordTextBox = ConfirmTextBox;
      }

      PasswordTextBoxValidated(passwordTextBox, EventArgs.Empty);
    }

    /// <summary>
    /// Event delegate method fired when the <see cref="PasswordDialog"/> form is closing.
    /// </summary>
    /// <param name="sender">Sender object.</param>
    /// <param name="e">Event arguments.</param>
    private void PasswordDialog_FormClosing(object sender, FormClosingEventArgs e)
    {
      if (DialogResult == DialogResult.Cancel)
      {
        _passwordFlags.Cancelled = true;
        return;
      }

      if (PasswordExpiredDialog)
      {
        // Check if the new password and its confirmation match, otherwise notify the user and exit.
        if (NewPasswordTextBox.Text != ConfirmTextBox.Text)
        {
          InfoDialog.ShowErrorDialog(Properties.Resources.PasswordResetErrorTitleText, Properties.Resources.PasswordsMismatchErrorText);
          e.Cancel = true;
          return;
        }

        // Reset the password and if the reset is successful assign the new password to the local connection.
        WbConnection.Password = PasswordTextBox.Text;
        try
        {
          WbConnection.ResetPassword(ConfirmTextBox.Text);
        }
        catch (Exception ex)
        {
          MySqlSourceTrace.WriteAppErrorToLog(ex);
          InfoDialog.ShowErrorDialog(Properties.Resources.PasswordResetErrorTitleText, Properties.Resources.PasswordResetErrorDetailText);
          _passwordFlags.Cancelled = true;
          return;
        }

        WbConnection.Password = ConfirmTextBox.Text;
      }
      else
      {
        WbConnection.Password = PasswordTextBox.Text;
      }

      _passwordFlags.NewPassword = WbConnection.Password;
      bool connectionSuccessful = false;
      if (_testConnection)
      {
        // Test the connection and if not successful revert the password to the one before the dialog was shown to the user.
        TestConnectionResult connectionResult = WbConnection.TestConnectionAndReturnResult(false);
        _passwordFlags.ConnectionResult = connectionResult;
        switch(connectionResult)
        {
          case TestConnectionResult.ConnectionSuccess:
          case TestConnectionResult.PasswordReset:
            connectionSuccessful = true;

            // If the pasword was reset within the connection test, then set it again in the new password flag.
            if (connectionResult == TestConnectionResult.PasswordReset)
            {
              _passwordFlags.NewPassword = WbConnection.Password;
            }

            break;

          case TestConnectionResult.PasswordExpired:
            // This status is set if the password was expired, and the dialog shown to the user to reset the password was cancelled, so exit.
            return;
        }
      }

      // If the connection was successful and the user chose to store the password, save it in the password vault.
      if (!StorePasswordSecurely || !connectionSuccessful || string.IsNullOrEmpty(WbConnection.Password))
      {
        return;
      }

      string storedPassword = MySqlWorkbenchPasswordVault.FindPassword(WbConnection.HostIdentifier, WbConnection.UserName);
      if (storedPassword == null || storedPassword != WbConnection.Password)
      {
        MySqlWorkbenchPasswordVault.StorePassword(WbConnection.HostIdentifier, WbConnection.UserName, WbConnection.Password);
      }
    }

    /// <summary>
    /// Event delegate method fired when the <see cref="PasswordTextBox"/> text changes.
    /// </summary>
    /// <param name="sender">Sender object.</param>
    /// <param name="e">Event arguments.</param>
    private void PasswordTextBoxTextChanged(object sender, EventArgs e)
    {
      PasswordChangedTimer.Stop();
      PasswordChangedTimer.Start();
    }

    /// <summary>
    /// Event delegate method fired when the <see cref="PasswordTextBox"/> is validated.
    /// </summary>
    /// <param name="sender">Sender object.</param>
    /// <param name="e">Event arguments.</param>
    private void PasswordTextBoxValidated(object sender, EventArgs e)
    {
      if (sender == null)
      {
        return;
      }

      TextBox passwordTextBox = sender as TextBox;
      if (passwordTextBox == null)
      {
        return;
      }

      PasswordChangedTimer.Stop();
      passwordTextBox.Text = passwordTextBox.Text.Trim();
      DialogOKButton.Enabled = PasswordTextBox.TextLength > 0 && (!PasswordExpiredDialog || NewPasswordTextBox.TextLength > 0 && ConfirmTextBox.TextLength > 0);
    }

    /// <summary>
    /// Sets the dialog interface to use it to enter connection passwords or to enter a new password after an old one expired.
    /// </summary>
    private void SetDialogInterface()
    {
      Text = PasswordExpiredDialog ? Properties.Resources.ExpiredPasswordWindowTitleText : Properties.Resources.ConnectionPasswordWindowTitleText;
      EnterPasswordLabel.Text = PasswordExpiredDialog ? Properties.Resources.ExpiredPasswordLabelText : Properties.Resources.ConnectionPasswordLabelText;
      Height = PasswordExpiredDialog ? EXPANDED_DIALOG_HEIGHT : REGULAR_DIALOG_HEIGHT;
      EnterPasswordLabel.Height = PasswordExpiredDialog ? EnterPasswordLabel.Height : EnterPasswordLabel.Height / 2;
      EnterPasswordLabel.Location = new Point(EnterPasswordLabel.Location.X, EnterPasswordLabel.Location.Y + (PasswordExpiredDialog ? 0 : TOP_LABEL_VERTICAL_DELTA));
      PasswordTextBox.ReadOnly = PasswordExpiredDialog;
      NewPasswordLabel.Visible = PasswordExpiredDialog;
      NewPasswordTextBox.Visible = PasswordExpiredDialog;
      ConfirmLabel.Visible = PasswordExpiredDialog;
      ConfirmTextBox.Visible = PasswordExpiredDialog;
      PasswordLabel.Text = PasswordExpiredDialog ? Properties.Resources.OldPasswordLabelText : Properties.Resources.PasswordLabelText;
      StorePasswordSecurelyCheckBox.Location = PasswordExpiredDialog ?  StorePasswordSecurelyCheckBox.Location : NewPasswordTextBox.Location;
      DialogOKButton.Enabled = PasswordTextBox.Text.Trim().Length > 0;
    }
  }

  /// <summary>
  /// Specifies flags about password-related operations.
  /// </summary>
  public struct PasswordDialogFlags
  {
    /// <summary>
    /// Initializes a new instance of the <see cref="PasswordDialogFlags"/> struct.
    /// </summary>
    /// <param name="wbConnection"></param>
    public PasswordDialogFlags(MySqlWorkbenchConnection wbConnection)
    {
      Cancelled = false;
      ConnectionResult = TestConnectionResult.None;
      NewPassword = null;
      OldPassword = wbConnection.Password;
    }

    /// <summary>
    /// Flag indicating whether the operation (password connection or reset password) was cancelled by the user.
    /// </summary>
    public bool Cancelled;

    /// <summary>
    /// Indicates the result of a connection test.
    /// </summary>
    public TestConnectionResult ConnectionResult;

    /// <summary>
    /// Gets a value indicating whether the connection was made sucessfully or if the password was just reset by the user.
    /// </summary>
    public bool ConnectionSuccess
    {
      get
      {
        return ConnectionResult == TestConnectionResult.ConnectionSuccess || ConnectionResult == TestConnectionResult.PasswordReset;
      }
    }

    /// <summary>
    /// The new password entered by the user.
    /// </summary>
    public string NewPassword;

    /// <summary>
    /// The original password provided to the <see cref="PasswordDialog"/> .
    /// </summary>
    public string OldPassword;

    /// <summary>
    /// Gets a value indicating whether the connection could not be made because of a wrong password.
    /// </summary>
    public bool WrongPassword
    {
      get
      {
        return ConnectionResult == TestConnectionResult.WrongPassword;
      }
    }
  }
}