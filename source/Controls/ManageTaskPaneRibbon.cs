// 
// Copyright (c) 2012, Oracle and/or its affiliates. All rights reserved.
//
// This program is free software; you can redistribute it and/or
// modify it under the terms of the GNU General Public License as
// published by the Free Software Foundation; version 2 of the
// License.
// 
// This program is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the
// GNU General Public License for more details.
// 
// You should have received a copy of the GNU General Public License
// along with this program; if not, write to the Free Software
// Foundation, Inc., 51 Franklin St, Fifth Floor, Boston, MA
// 02110-1301  USA
//

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
