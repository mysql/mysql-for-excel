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
      // we need to validate things like port
      int validPort = 0;
      if (int.TryParse(WBconn.Port.ToString(), out validPort))
        result = true;
      else
        return false;
      //More Validations
      return result;
    }

    public MySqlConnection TryOpenConnection(MySqlConnectionStringBuilder connectionString)
    {
      MySqlConnection conn = new MySqlConnection(connectionString.ConnectionString);

      try
      {
        conn.Open();
      }
      catch (MySqlException)
      {
        PasswordDialog pwdDialog = new PasswordDialog(WBconn.Name, WBconn.UserName);
        if (pwdDialog.ShowDialog(this) == DialogResult.OK)
        {
          connectionString.Password = pwdDialog.PasswordText;
          conn = new MySqlConnection(connectionString.ConnectionString);
          conn.Open();
        }
        pwdDialog.Dispose();
      }
      return conn;
    }

    private void testButton_Click(object sender, EventArgs e)
    {
      MySqlConnectionStringBuilder testConn = new MySqlConnectionStringBuilder();
      testConn.Server = WBconn.Host;
      testConn.Port = (uint)WBconn.Port;
      testConn.UserID = WBconn.UserName;
      InfoDialog infoDialog = new InfoDialog(false, String.Format(Properties.Resources.ConnectionDataDisplayFailed, testConn.Server, testConn.Port, testConn.UserID), String.Format(Properties.Resources.ConnectionFailed, testConn.Server, testConn.Port));

      try
      {
        if (TryOpenConnection(testConn) != null)
        {
          infoDialog = new InfoDialog(true, String.Format(Properties.Resources.ConnectionDataDisplaySuccess, testConn.Server, testConn.Port, testConn.UserID), Properties.Resources.ConnectionSuccessfull);
          infoDialog.OperationSummarySubText = Properties.Resources.ConnectionSuccessfull;
        }
      }
      catch (Exception ex)
      {
        infoDialog.OperationSummarySubText = ex.Message;
      }
      finally
      {
        infoDialog.WordWrapDetails = true;
        infoDialog.ShowDialog();
        infoDialog.Dispose();
      }
    }

    private void connectionName_TextChanged(object sender, EventArgs e)
    {
      okButton.Enabled = (connectionName.TextLength > 0);
    }

    private void connectionMethod_SelectedIndexChanged(object sender, EventArgs e)
    {
      if (connectionMethod.SelectedIndex == 0)
        WBconn.Host = "127.0.0.1";
      else
      {
        WBconn.Host = "";
        labelPromptSocket.Location = new Point(labelPromptSocket.Location.X, labelPromptHostName.Location.Y);
        labelHelpSocket.Location = new Point(labelHelpSocket.Location.X, labelHelpHostName.Location.Y);
        socketPath.Location = new Point(socketPath.Location.X, hostName.Location.Y);
      }

      WBconn.Port = 3306;
      WBconn.UserName = "root";
      WBconn.Schema = "";

      labelHelpHostName.Visible = (connectionMethod.SelectedIndex == 0);
      labelPromptHostName.Enabled = (connectionMethod.SelectedIndex == 0);
      labelPromptHostName.Visible = (connectionMethod.SelectedIndex == 0);
      labelHelpHostName.Visible = (connectionMethod.SelectedIndex == 0);
      labelPromptPort.Visible = (connectionMethod.SelectedIndex == 0);
      labelPromptPort.Enabled = (connectionMethod.SelectedIndex == 0);
      port.Enabled = (connectionMethod.SelectedIndex == 0);
      port.Visible = (connectionMethod.SelectedIndex == 0);

      labelPromptSocket.Visible = !(connectionMethod.SelectedIndex == 0);
      labelHelpSocket.Visible = !(connectionMethod.SelectedIndex == 0);
      socketPath.Enabled = !(connectionMethod.SelectedIndex == 0);
      socketPath.Visible = !(connectionMethod.SelectedIndex == 0);

      useCompression.Enabled = !(connectionMethod.SelectedIndex == 0);
      labelCompression.Visible = !(connectionMethod.SelectedIndex == 0);

      bindingSource.ResetCurrentItem();
    }

    private void NewConnectionDialog_Load(object sender, EventArgs e)
    {

    }
  }
}
