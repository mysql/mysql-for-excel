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
  public partial class NewSchemaDialog : AutoStyleableBaseDialog
  {
    public string SchemaName
    {
      get { return txtSchemaName.Text.Trim(); }
      set { txtSchemaName.Text = value; }
    }

    public NewSchemaDialog()
    {
      InitializeComponent();
    }

    private void txtSchemaName_TextChanged(object sender, EventArgs e)
    {
      btnOK.Enabled = SchemaName.Length > 0;
    }
  }
}
