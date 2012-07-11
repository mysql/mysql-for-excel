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
  public partial class WarningDialog : AutoStyleableBaseDialog
  {
    public string WarningTitle
    {
      get { return lblWarningTitle.Text; }
      set { lblWarningTitle.Text = value; }
    }
    public string WarningText
    {
      get { return lblWarningText.Text; }
      set { lblWarningText.Text = value; }
    }

    public WarningDialog(string warningTitle, string warningText)
    {
      InitializeComponent();
      WarningTitle = warningTitle;
      WarningText = warningText;
    }

    public WarningDialog() : this("Warning Title", "Warning Details Text")
    {
    }
  }
}
