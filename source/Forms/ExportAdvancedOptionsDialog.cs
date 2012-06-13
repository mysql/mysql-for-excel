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
  public partial class ExportAdvancedOptionsDialog : Form
  {
    public ExportAdvancedOptionsDialog()
    {
      InitializeComponent();

      chkDetectDatatype.Checked = Properties.Settings.Default.ExportDetectDatatype;
      chkAddBufferToVarchar.Checked = Properties.Settings.Default.ExportAddBufferToVarchar;
      chkAutoIndexIntColumns.Checked = Properties.Settings.Default.ExportAutoIndexIntColumns;
      chkAutoAllowEmptyNonIndexColumns.Checked = Properties.Settings.Default.ExportAutoAllowEmptyNonIndexColumns;
      chkUseFormattedValues.Checked = Properties.Settings.Default.ExportUseFormattedValues;
      chkShowCopySQLButton.Checked = Properties.Settings.Default.ExportShowCopySQLButton;
    }

    private void btnAccept_Click(object sender, EventArgs e)
    {
      Properties.Settings.Default.ExportDetectDatatype = chkDetectDatatype.Checked;
      Properties.Settings.Default.ExportAddBufferToVarchar = chkAddBufferToVarchar.Checked;
      Properties.Settings.Default.ExportAutoIndexIntColumns = chkAutoIndexIntColumns.Checked;
      Properties.Settings.Default.ExportAutoAllowEmptyNonIndexColumns = chkAutoAllowEmptyNonIndexColumns.Checked;
      Properties.Settings.Default.ExportUseFormattedValues = chkUseFormattedValues.Checked;
      Properties.Settings.Default.ExportShowCopySQLButton = chkShowCopySQLButton.Checked;
      DialogResult = DialogResult.OK;
      Close();
    }

  }
}
