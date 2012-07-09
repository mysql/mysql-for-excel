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
  public partial class InfoDialog : AutoStyleableBaseDialog
  {
    private const int COLLAPSED_HEIGHT = 215;
    private const int EXPANDED_HEIGHT = 350;

    public InfoDialog(bool operationSuccessful, string operationSummary, string operationDetails)
    {
      InitializeComponent();
      picLogo.Image = (operationSuccessful ? Properties.Resources.MySQLforExcel_InfoDlg_Success_64x64 : Properties.Resources.MySQLforExcel_InfoDlg_Error_64x64);
      lblOperationStatus.Text = (operationSuccessful ? "Operation Completed Successfully" : "An Error Ocurred");
      lblOperationSummary.Text = operationSummary;
      txtDetails.Text = operationDetails;
      btnOK.Text = (operationSuccessful ? "OK" : "Back");
      btnOK.DialogResult = (operationSuccessful ? DialogResult.OK : DialogResult.Cancel);
      ChangeHeight(false);
    }

    private void ChangeHeight(bool expand)
    {
      if (expand)
      {
        txtDetails.Visible = true;
        MaximumSize = MinimumSize = new Size(Width, EXPANDED_HEIGHT);
        Height = EXPANDED_HEIGHT;
      }
      else
      {
        txtDetails.Visible = false;
        MaximumSize = MinimumSize = new Size(Width, COLLAPSED_HEIGHT);
        Height = COLLAPSED_HEIGHT;
      }
    }

    private void btnShowDetails_Click(object sender, EventArgs e)
    {
      ChangeHeight(Height == COLLAPSED_HEIGHT);
    }

    private void btnOK_Click(object sender, EventArgs e)
    {
      DialogResult = btnOK.DialogResult;
    }
  }
}
