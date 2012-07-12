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

    public PasswordDialog(string ConnectionName, string User)
    {
      InitializeComponent();
      if (ConnectionName == null)
        lblConnectionValue.Text = "New";
      else
        lblConnectionValue.Text = ConnectionName;
      if (User == null)
        lblUserValue.Text = "guest";
      else
        lblUserValue.Text = User;
    }
  }
}
