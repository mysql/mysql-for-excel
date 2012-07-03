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
    public AppendNewColumnMappingDialog()
    {
      InitializeComponent();
    }

    public string ColumnMappingName { get; set; }

    private void btnOK_Click(object sender, EventArgs e)
    {
      ColumnMappingName = txtMappingName.Text.Trim();
    }
  }
}
