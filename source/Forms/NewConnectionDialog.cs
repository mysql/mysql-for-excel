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
using System.Windows.Forms;
using MySql.Data.MySqlClient;
using MySQL.Utility;
using System.Drawing;

namespace MySQL.ForExcel
{
  public partial class NewConnectionDialog : AutoStyleableBaseDialog
  {
    private MySqlWorkbenchConnection WBconn;

    public NewConnectionDialog()
    {
      InitializeComponent();
      WBconn = new MySqlWorkbenchConnection();
      connectionMethod.SelectedIndex = 0;
      bindingSource.DataSource = WBconn;
    }

    public MySqlWorkbenchConnection NewConnection { get; private set; }

    private void okButton_Click(object sender, EventArgs e)
    {
      if (!ValidateConnection())
      {
        DialogResult = DialogResult.None;
        return;
      }
      NewConnection = WBconn;
    }

    private bool ValidateConnection()
    {
      bool result = false;

      int validPort = 0;
      if (int.TryParse(WBconn.Port.ToString(), out validPort))
        result = true;
      else
        return false;

      //TODO: Need to add More Validations
      return result;
    }

    public bool TryOpenConnection(MySqlConnectionStringBuilder connectionString)
    {
      MySqlConnection WinAuthconn = new MySqlConnection(connectionString.ConnectionString + "; Integrated Security=True");
      MySqlConnection conn = new MySqlConnection(connectionString.ConnectionString);
      MySqlConnection connPass = conn;

      try
      {
        conn.Open();
        return true;
      }
      catch (MySqlException)
      {
        try
        {
          WinAuthconn.Open();
          return true;
        }
        catch (MySqlException)
        {
          try
          {
            PasswordDialog pwdDialog = new PasswordDialog(WBconn.HostIdentifier, WBconn.UserName);
            if (pwdDialog.ShowDialog(this) == DialogResult.Cancel) return false;
            connectionString.Password = pwdDialog.PasswordText;
            pwdDialog.Dispose();
            connPass = new MySqlConnection(connectionString.ConnectionString);
            connPass.Open();
            return true;
          }
          catch
          {
            throw;
          }
        }
      }
      finally
      {
        WinAuthconn.Close();
        conn.Close();
        connPass.Close();
      }
    }

    private void testButton_Click(object sender, EventArgs e)
    {      
      MySqlConnectionStringBuilder testConn = new MySqlConnectionStringBuilder();
      testConn.Server = WBconn.Host;
      testConn.Port = (uint)WBconn.Port;
      testConn.UserID = WBconn.UserName;
      testConn.ConnectionProtocol = (WBconn.DriverType == MySqlWorkbenchConnectionType.Tcp) ? MySqlConnectionProtocol.Tcp : MySqlConnectionProtocol.NamedPipe;
      string testHostName, testUserName;
      testHostName = (WBconn.Host == string.Empty) ? "localhost" : WBconn.Host;
      testUserName = WBconn.UserName;

      InfoDialog infoDialog = new InfoDialog(false, String.Format(Properties.Resources.ConnectionDataDisplayFailed, testHostName, testConn.Port, testUserName), string.Empty);

      try
      {
        if (!TryOpenConnection(testConn)) return;
        infoDialog = new InfoDialog(true, String.Format(Properties.Resources.ConnectionDataDisplaySuccess, testHostName, testConn.Port, testUserName), string.Empty);
      }
      catch (Exception ex)
      {
        infoDialog.OperationDetailsText = ex.Message;
        MiscUtilities.GetSourceTrace().WriteError("Application Exception - " + (ex.Message + " " + ex.InnerException), 1);
      }
      infoDialog.WordWrapDetails = true;
      infoDialog.ShowDialog();
      infoDialog.Dispose();
    }

    private void connectionName_TextChanged(object sender, EventArgs e)
    {
      okButton.Enabled = (connectionName.TextLength > 0);
    }

    private void connectionMethod_SelectedIndexChanged(object sender, EventArgs e)
    {
      bool standardConnection = (connectionMethod.SelectedIndex == 0);

      if (connectionMethod.SelectedIndex == 0)
      {
        WBconn.Host = "127.0.0.1";
        WBconn.DriverType = MySqlWorkbenchConnectionType.Tcp;
      }
      else if (connectionMethod.SelectedIndex == 1)
      {
        WBconn.DriverType = MySqlWorkbenchConnectionType.NamedPipes;
        WBconn.Host = "";
        labelPromptSocket.Location = new Point(labelPromptSocket.Location.X, labelPromptHostName.Location.Y);
        labelHelpSocket.Location = new Point(labelHelpSocket.Location.X, labelHelpHostName.Location.Y);
        socketPath.Location = new Point(socketPath.Location.X, hostName.Location.Y);
      }

      WBconn.Port = 3306;
      WBconn.UserName = "root";
      WBconn.Schema = "";

      labelPromptHostName.Visible = standardConnection;
      hostName.Enabled = standardConnection;
      hostName.Visible = standardConnection;
      labelHelpHostName.Visible = standardConnection;

      labelPromptPort.Visible = standardConnection;
      port.Enabled = standardConnection;
      port.Visible = standardConnection;

      useCompression.Enabled = standardConnection;
      labelCompression.Visible = standardConnection;

      labelPromptSocket.Visible = !standardConnection;
      socketPath.Enabled = !standardConnection;
      socketPath.Visible = !standardConnection;
      labelHelpSocket.Visible = !standardConnection;

      bindingSource.ResetCurrentItem();
    }
  }
}
