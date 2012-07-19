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
