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
      if (!ValidateAndSave())
      {
        DialogResult = DialogResult.None;
        return;
      }
      NewConnection = WBconn;
    }

    private bool ValidateAndSave()
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
      MySqlConnection conn = new MySqlConnection(connectionString.ConnectionString);

      try
      {
        conn.Open();
        return true;
      }
      catch (MySqlException)
      {
        PasswordDialog pwdDialog = new PasswordDialog(WBconn.Host, WBconn.Port, WBconn.UserName);
        if (pwdDialog.ShowDialog(this) == DialogResult.Cancel) return false;
        connectionString.Password = pwdDialog.PasswordText;
        pwdDialog.Dispose();
        conn = new MySqlConnection(connectionString.ConnectionString);
        try
        {
          conn.Open();
          return true;
        }
        catch
        {
          throw;
        }
      }
      finally
      {
        conn.Close();
      }
    }

    private void testButton_Click(object sender, EventArgs e)
    {      
      MySqlConnectionStringBuilder testConn = new MySqlConnectionStringBuilder();
      testConn.Server = WBconn.Host;
      testConn.Port = (uint)WBconn.Port;
      testConn.UserID = WBconn.UserName;
      string testHostName, testUserName;
      testHostName = (WBconn.Host == string.Empty) ? "localhost" : WBconn.Host;
      testUserName = (WBconn.UserName == string.Empty) ? "guest" : WBconn.UserName;

      InfoDialog infoDialog = new InfoDialog(false, String.Format(Properties.Resources.ConnectionDataDisplayFailed, testHostName, testConn.Port, testUserName), string.Empty);

      try
      {
        if (!TryOpenConnection(testConn)) return;
        infoDialog = new InfoDialog(true, String.Format(Properties.Resources.ConnectionDataDisplaySuccess, testHostName, testConn.Port, testUserName), string.Empty);
      }
      catch (Exception ex)
      {
        infoDialog.OperationDetailsText = ex.Message;
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
