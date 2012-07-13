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
  public partial class AppendNewColumnMappingDialog : AutoStyleableBaseDialog
  {
    public string ColumnMappingName
    {
      get { return txtMappingName.Text.Trim(); }
      set { txtMappingName.Text = value; }
    }

    public AppendNewColumnMappingDialog(string proposedMappingName)
    {
      InitializeComponent();
      txtMappingName.Text = proposedMappingName;
      txtMappingName.SelectAll();
    }

    public AppendNewColumnMappingDialog() : this(String.Empty)
    {
    }

    private void txtMappingName_TextChanged(object sender, EventArgs e)
    {
      btnOK.Enabled = ColumnMappingName.Length > 0;
    }
  }
}
