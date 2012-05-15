using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using MySQL.Utility;

namespace MySQL.ExcelAddIn
{
  public partial class NewConnectionDialog : Form
  {
    private MySqlWorkbenchConnection connection;

    public NewConnectionDialog()
    {
      InitializeComponent();
      connection = new MySqlWorkbenchConnection();
      connectionName.DataBindings.Add(new Binding("Text", connection, "Name"));
      hostName.DataBindings.Add(new Binding("Text", connection, "Host"));
      userName.DataBindings.Add(new Binding("Text", connection, "UserName"));
      //TODO: we need to setup more bindings
    }

    public MySqlWorkbenchConnection NewConnection { get; private set; }

    private void okButton_Click(object sender, EventArgs e)
    {
      if (!ValidateAndSave())
      {
        DialogResult = DialogResult.None;
        return;
      }

      NewConnection = connection;
    }

    private bool ValidateAndSave()
    {
      // we need to validate things like port
      return true;
    }
  }
}
