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
using Excel = Microsoft.Office.Interop.Excel;
using Office = Microsoft.Office.Core;
using Microsoft.Office.Tools.Excel;
using System.Windows.Forms;
using System.IO;

namespace MySQL.ForExcel
{
  public partial class ThisAddIn
  {
    private const int paneWidth = 260;
    private TaskPaneControl taskPaneControl;
    private Microsoft.Office.Tools.CustomTaskPane taskPaneValue;

    public Microsoft.Office.Tools.CustomTaskPane TaskPane
    {
      get { return this.taskPaneValue; }
    }

    private void ThisAddIn_Startup(object sender, System.EventArgs e)
    {
      // make sure our settings dir exists
      string dir = String.Format(@"{0}\Oracle\MySQL for Excel", Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData));
      Directory.CreateDirectory(dir);

      taskPaneControl = new TaskPaneControl(Application);
      taskPaneControl.Dock = DockStyle.Fill;
      taskPaneControl.SizeChanged += new EventHandler(taskPaneControl_SizeChanged);
      taskPaneValue = CustomTaskPanes.Add(taskPaneControl, "MySQL for Excel");
      taskPaneValue.VisibleChanged += taskPaneValue_VisibleChanged;
      taskPaneValue.DockPosition = Office.MsoCTPDockPosition.msoCTPDockPositionRight;
      taskPaneValue.DockPositionRestrict = Office.MsoCTPDockPositionRestrict.msoCTPDockPositionRestrictNoHorizontal;
      taskPaneValue.Width = paneWidth;
    }

    void taskPaneControl_SizeChanged(object sender, EventArgs e)
    {
      if (taskPaneValue == null || !taskPaneValue.Visible)
        return;
      bool shouldResetWidth = taskPaneValue.Width != paneWidth && this.Application.Width >= paneWidth;
      if (shouldResetWidth)
      {
        try
        {
          SendKeys.Send("{ESC}");
          taskPaneValue.Width = paneWidth;
        }
        catch (Exception ex)
        {
          MiscUtilities.GetSourceTrace().WriteError("Application Exception - " + (ex.Message + " " + ex.InnerException), 1);
        }
      }
    }

    private void taskPaneValue_VisibleChanged(object sender, EventArgs e)
    {
      Globals.Ribbons.ManageTaskPaneRibbon.togShowTaskPane.Checked = taskPaneValue.Visible;
    }
    
    private void ThisAddIn_Shutdown(object sender, System.EventArgs e)
    {
      taskPaneControl.CloseAddIn();
    }

    #region VSTO generated code

    /// <summary>
    /// Required method for Designer support - do not modify
    /// the contents of this method with the code editor.
    /// </summary>
    private void InternalStartup()
    {
      this.Startup += new System.EventHandler(ThisAddIn_Startup);
      this.Shutdown += new System.EventHandler(ThisAddIn_Shutdown);
    }
        
    #endregion
  }
}
