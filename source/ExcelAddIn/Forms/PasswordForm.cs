using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace MySQL.ExcelAddIn
{
  public partial class PasswordForm : Form
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

    public PasswordForm()
    {
      InitializeComponent();
    }
  }
}
