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
  public partial class AppendAdvancedOptionsDialog : AutoStyleableBaseDialog
  {
    public AppendAdvancedOptionsDialog()
    {
      InitializeComponent();

      chkDoNotPerformAutoMap.Checked = Properties.Settings.Default.AppendPerformAutoMap;
      //chkAutoStoreColumnMapping.Checked = Properties.Settings.Default.AppendAutoStoreColumnMapping;
      //chkReloadColumnMapping.Checked = Properties.Settings.Default.AppendReloadColumnMapping;
      chkUseFormattedValues.Checked = Properties.Settings.Default.AppendUseFormattedValues;
    }

    private void btnAccept_Click(object sender, EventArgs e)
    {
      Properties.Settings.Default.AppendPerformAutoMap = chkDoNotPerformAutoMap.Checked;
      //Properties.Settings.Default.AppendAutoStoreColumnMapping = chkAutoStoreColumnMapping.Checked;
      //Properties.Settings.Default.AppendReloadColumnMapping = chkReloadColumnMapping.Checked;
      Properties.Settings.Default.AppendUseFormattedValues = chkUseFormattedValues.Checked;
      DialogResult = DialogResult.OK;
      Close();
    }

  }
}
