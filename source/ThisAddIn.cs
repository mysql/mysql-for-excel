using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Excel = Microsoft.Office.Interop.Excel;
using Office = Microsoft.Office.Core;
using Microsoft.Office.Tools.Excel;
using System.Windows.Forms;

namespace MySQL.ForExcel
{
  public partial class ThisAddIn
  {
    private const int paneWidth = 260;
    private const int minPaneHeight = 500;
    private TaskPaneControl taskPaneControl;
    private Microsoft.Office.Tools.CustomTaskPane taskPaneValue;

    public Microsoft.Office.Tools.CustomTaskPane TaskPane
    {
      get { return this.taskPaneValue; }
    }

    private void ThisAddIn_Startup(object sender, System.EventArgs e)
    {
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
      if (taskPaneValue != null && taskPaneValue.Visible && (taskPaneValue.Width != paneWidth || taskPaneValue.Height < minPaneHeight))
      {
        SendKeys.Send("{ESC}");
        if (taskPaneValue.Width != paneWidth)
          taskPaneValue.Width = paneWidth;
        if (taskPaneValue.Height < minPaneHeight)
          taskPaneValue.Height = minPaneHeight;
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
