// 
// Copyright (c) 2012-2013, Oracle and/or its affiliates. All rights reserved.
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

namespace MySQL.ForExcel
{
  using System;
  using System.Diagnostics;
  using System.IO;
  using System.Windows.Forms;
  using MySQL.Utility;
  using MySQL.Utility.Forms;
  using Office = Microsoft.Office.Core;

  /// <summary>
  /// Represents the main MySQL for Excel Office add-in.
  /// </summary>
  public partial class ThisAddIn
  {
    /// <summary>
    /// The Add-In's pane width in pixels.
    /// </summary>
    private const int ADD_IN_PANE_WIDTH = 266;

    /// <summary>
    /// The string representation of the Escape key.
    /// </summary>
    private const string ESCAPE_KEY = "{ESC}";

    /// <summary>
    /// The title given to the assembly of the Add-In.
    /// </summary>
    private string _assemblyTitle;

    /// <summary>
    /// The pane control used by the Add-In to display its controls.
    /// </summary>
    private TaskPaneControl _taskPaneControl;

    /// <summary>
    /// The pane control added to the Excel's ribbon.
    /// </summary>
    private Microsoft.Office.Tools.CustomTaskPane _taskPaneValue;

    /// <summary>
    /// Gets the title given to the assembly of the Add-In.
    /// </summary>
    public string AssemblyTitle
    {
      get
      {
        if (string.IsNullOrEmpty(_assemblyTitle))
        {
          _assemblyTitle = AssemblyInfo.AssemblyTitle;
        }

        return _assemblyTitle;
      }
    }

    /// <summary>
    /// Gets the pane control added to the Excel's ribbon.
    /// </summary>
    public Microsoft.Office.Tools.CustomTaskPane TaskPane
    {
      get
      {
        return _taskPaneValue;
      }
    }

    /// <summary>
    /// Customizes the looks of the <see cref="MySQL.Utility.Forms.InfoDialog"/> form for MySQL for Excel.
    /// </summary>
    private void CustomizeInfoDialog()
    {
      InfoDialog.ApplicationName = AssemblyTitle;
      InfoDialog.SuccessLogo = Properties.Resources.MySQLforExcel_InfoDlg_Success_64x64;
      InfoDialog.ErrorLogo = Properties.Resources.MySQLforExcel_InfoDlg_Error_64x64;
      InfoDialog.WarningLogo = Properties.Resources.MySQLforExcel_InfoDlg_Warning_64x64;
      InfoDialog.InformationLogo = Properties.Resources.MySQLforExcel_Logo_64x64;
    }

    /// <summary>
    /// Initializes settings for the <see cref="MySqlWorkbenchConnectionsHelper"/> and <see cref="MySqlWorkbenchPasswordVault"/> classes.
    /// </summary>
    private void InitializeMySQLWorkbenchStaticSettings()
    {
      string applicationDataFolderPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
      MySqlWorkbench.ExternalApplicationName = AssemblyTitle;
      MySqlWorkbenchPasswordVault.ApplicationPasswordVaultFilePath = applicationDataFolderPath + @"\Oracle\MySQL for Excel\user_data.dat";
      MySqlWorkbench.ExternalApplicationConnectionsFilePath = applicationDataFolderPath + @"\Oracle\MySQL for Excel\connections.xml";
      MySQLSourceTrace.LogFilePath = applicationDataFolderPath + @"\Oracle\MySQL for Excel\MySQLForExcel.log";
      MySQLSourceTrace.SourceTraceClass = "MySQLForExcel";
    }

    /// <summary>
    /// Event delegate method fired when the <see cref="taskPaneControl"/> size changes.
    /// </summary>
    /// <param name="sender">Sender object.</param>
    /// <param name="e">Event arguments.</param>
    private void taskPaneControl_SizeChanged(object sender, EventArgs e)
    {
      if (_taskPaneValue == null || !_taskPaneValue.Visible)
      {
        return;
      }

      bool shouldResetWidth = _taskPaneValue.Width != ADD_IN_PANE_WIDTH && Application.Width >= ADD_IN_PANE_WIDTH;
      if (shouldResetWidth)
      {
        try
        {
          SendKeys.Send(ESCAPE_KEY);
          _taskPaneValue.Width = ADD_IN_PANE_WIDTH;
        }
        catch (Exception ex)
        {
          MySQLSourceTrace.WriteAppErrorToLog(ex);
        }
      }
    }

    /// <summary>
    /// Event delegate method fired when the <see cref="taskPaneValue"/> visible property value changes.
    /// </summary>
    /// <param name="sender">Sender object.</param>
    /// <param name="e">Sender object.</param>
    private void taskPaneValue_VisibleChanged(object sender, EventArgs e)
    {
      Globals.Ribbons.ManageTaskPaneRibbon.ShowTaskPaneRibbonToggleButton.Checked = _taskPaneValue.Visible;
    }

    /// <summary>
    /// Event delegate method fired when the <see cref="ThisAddIn"/> is closed.
    /// </summary>
    /// <param name="sender">Sender object.</param>
    /// <param name="e">Event arguments.</param>
    private void ThisAddIn_Shutdown(object sender, System.EventArgs e)
    {
      MySQLSourceTrace.WriteToLog(Properties.Resources.ShutdownMessage, SourceLevels.Information);
      _taskPaneControl.CloseAddIn(true);
    }

    /// <summary>
    /// Event delegate method fired when the <see cref="ThisAddIn"/> is started.
    /// </summary>
    /// <param name="sender">Sender object.</param>
    /// <param name="e">Event arguments.</param>
    private void ThisAddIn_Startup(object sender, System.EventArgs e)
    {
      try
      {
        //// Static initializations.
        System.Windows.Forms.Application.EnableVisualStyles();
        System.Windows.Forms.Application.SetCompatibleTextRenderingDefault(false);
        CustomizeInfoDialog();
        InitializeMySQLWorkbenchStaticSettings();

        //// Log the Add-In's Startup
        MySQLSourceTrace.WriteToLog(Properties.Resources.StartupMessage, SourceLevels.Information);

        //// Make sure the settings directory exists
        Directory.CreateDirectory(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + @"\Oracle\MySQL for Excel");

        //// This method is used to migrate all connections created with 1.0.6 (in a local connections file) to the Workbench connections file.
        MySqlWorkbench.MigrateExternalConnectionsToWorkbench();

        _taskPaneControl = new TaskPaneControl(Application);
        _taskPaneControl.Dock = DockStyle.Fill;
        _taskPaneControl.SizeChanged += new EventHandler(taskPaneControl_SizeChanged);
        _taskPaneValue = CustomTaskPanes.Add(_taskPaneControl, AssemblyTitle);
        _taskPaneValue.VisibleChanged += taskPaneValue_VisibleChanged;
        _taskPaneValue.DockPosition = Office.MsoCTPDockPosition.msoCTPDockPositionRight;
        _taskPaneValue.DockPositionRestrict = Office.MsoCTPDockPositionRestrict.msoCTPDockPositionRestrictNoHorizontal;
        _taskPaneValue.Width = ADD_IN_PANE_WIDTH;
      }
      catch (Exception ex)
      {
        MySQLSourceTrace.WriteAppErrorToLog(ex);
      }
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

    #endregion VSTO generated code
  }
}