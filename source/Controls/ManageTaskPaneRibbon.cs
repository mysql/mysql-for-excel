using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Office.Tools.Ribbon;
using System.Windows.Forms;

namespace MySQL.ForExcel
{
  public partial class ManageTaskPaneRibbon
  {
    private void ManageTaskPaneRibbon_Load(object sender, RibbonUIEventArgs e)
    {

    }

    private void togShowTaskPane_Click(object sender, RibbonControlEventArgs e)
    {
      bool enableAddIn = ((RibbonToggleButton)sender).Checked;
      Globals.ThisAddIn.TaskPane.Visible = enableAddIn;
      if (!enableAddIn)
      {
        TaskPaneControl tpc = Globals.ThisAddIn.TaskPane as TaskPaneControl;
        if (tpc != null)
          tpc.CloseAddIn();
      }
    }

  }
}
