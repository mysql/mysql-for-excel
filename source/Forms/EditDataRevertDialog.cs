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
  public partial class EditDataRevertDialog : AutoStyleableBaseDialog
  {
    public enum EditUndoAction { RefreshData, RevertData };
    public EditUndoAction SelectedAction { get; private set; }

    public EditDataRevertDialog(bool autoCommitEnabled)
    {
      InitializeComponent();
      btnRevert.Enabled = !autoCommitEnabled;
    }

    private void btnRevert_Click(object sender, EventArgs e)
    {
      SelectedAction = EditUndoAction.RevertData;
    }

    private void btnRefreshData_Click(object sender, EventArgs e)
    {
      SelectedAction = EditUndoAction.RefreshData;
    }
  }
}
