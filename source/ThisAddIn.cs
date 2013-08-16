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
  using System.Collections.Generic;
  using System.Diagnostics;
  using System.Globalization;
  using System.IO;
  using System.Linq;
  using System.Windows.Forms;
  using MySQL.Utility;
  using MySQL.Utility.Forms;
  using Excel = Microsoft.Office.Interop.Excel;
  using Office = Microsoft.Office.Tools;
  using OfficeCore = Microsoft.Office.Core;

  /// <summary>
  /// Represents the main MySQL for Excel Office add-in.
  /// </summary>
  public partial class ThisAddIn
  {
    #region Constants

    /// <summary>
    /// The Add-In's pane width in pixels.
    /// </summary>
    public const int ADD_IN_PANE_WIDTH = 266;

    /// <summary>
    /// The string representation of the Escape key.
    /// </summary>
    public const string ESCAPE_KEY = "{ESC}";

    /// <summary>
    /// The Excel major version number corresponding to Excel 2013.
    /// </summary>
    public const int EXCEL_2013_VERSION_NUMBER = 15;

    #endregion Constants

    #region Properties

    /// <summary>
    /// Gets the <see cref="Microsoft.Office.Tools.CustomTaskPane"/> contained in the active Excel window.
    /// </summary>
    public Office.CustomTaskPane ActiveCustomPane
    {
      get
      {
        Office.CustomTaskPane addInPane = CustomTaskPanes.FirstOrDefault(ctp =>
          {
            Excel.Window paneWindow = ctp.Window as Excel.Window;
            bool isPanWindow = paneWindow.Hwnd == Application.ActiveWindow.Hwnd;
            bool isExcelAddin = ctp.Control is ExcelAddInPane;
            return isPanWindow && isExcelAddin;
          });

        return addInPane;
      }
    }

    /// <summary>
    /// Gets the pane containing the MySQL for Excel add-in contained in the custom task pane shown in the active window.
    /// </summary>
    public ExcelAddInPane ActiveExcelPane
    {
      get
      {
        return ActiveCustomPane != null ? ActiveCustomPane.Control as ExcelAddInPane : null;
      }
    }

    /// <summary>
    /// Gets the title given to the assembly of the Add-In.
    /// </summary>
    public string AssemblyTitle { get; private set; }

    /// <summary>
    /// Gets a list with all the Excel panes instantiated in the Excel session, stored it to dispose of them when needed.
    /// </summary>
    public List<ExcelAddInPane> ExcelPanesList { get; private set; }

    /// <summary>
    /// Gets the MS Excel major version number.
    /// </summary>
    public int ExcelVersionNumber { get; private set; }

    #endregion Properties

    /// <summary>
    /// Closes the Excel Add-In pane and its related custom task pane.
    /// </summary>
    /// <param name="excelPane">The Excel pane to close.</param>
    public void CloseExcelPane(ExcelAddInPane excelPane)
    {
      if (excelPane == null)
      {
        return;
      }

      Office.CustomTaskPane customPane = CustomTaskPanes.FirstOrDefault(ctp => ctp.Control is ExcelAddInPane && ctp.Control == excelPane);
      ExcelPanesList.Remove(excelPane);
      excelPane.Dispose();
      if (customPane != null)
      {
        customPane.Dispose();
      }

      CustomTaskPanes.Remove(customPane);
    }

    /// <summary>
    /// Gets the custom task pane in the active window, if not found creates it.
    /// </summary>
    /// <returns>the active or newly created <see cref="Microsoft.Office.Tools.CustomTaskPane"/> object.</returns>
    public Office.CustomTaskPane GetOrCreateActiveCustomPane()
    {
      Office.CustomTaskPane activeCustomPane = ActiveCustomPane;

      //// If there is no custom pane associated to the Excel Add-In in the active window, create one.
      if (activeCustomPane == null)
      {
        Application.Cursor = Excel.XlMousePointer.xlWait;
        if (ExcelPanesList == null)
        {
          ExcelPanesList = new List<ExcelAddInPane>();
        }

        //// Instantiate the Excel Add-In pane to attach it to the Excel's custom task pane.
        //// Note that in Excel 2007 and 2010 a MDI model is used so only a single Excel pane is instantiated, whereas in Excel 2013 and greater
        ////  a SDI model is used instead, so an Excel pane is instantiated for each custom task pane appearing in each Excel window.
        ExcelAddInPane excelPane = new ExcelAddInPane(Application);
        excelPane.Dock = DockStyle.Fill;
        excelPane.SizeChanged += new EventHandler(ExcelPane_SizeChanged);
        ExcelPanesList.Add(excelPane);

        //// Create a new custom task pane and initialize it.
        activeCustomPane = CustomTaskPanes.Add(excelPane, AssemblyTitle);
        activeCustomPane.VisibleChanged += CustomTaskPaneVisibleChanged;
        activeCustomPane.DockPosition = OfficeCore.MsoCTPDockPosition.msoCTPDockPositionRight;
        activeCustomPane.DockPositionRestrict = OfficeCore.MsoCTPDockPositionRestrict.msoCTPDockPositionRestrictNoHorizontal;
        activeCustomPane.Width = ADD_IN_PANE_WIDTH;

        Application.Cursor = Excel.XlMousePointer.xlDefault;
      }

      return activeCustomPane;
    }

    /// <summary>
    /// Event delegate method fired when an Excel window is activated.
    /// </summary>
    /// <param name="Wb">The Excel workbook tied to the activated window.</param>
    /// <param name="Wn">The activated Excel window.</param>
    private void Application_WindowActivate(Excel.Workbook Wb, Excel.Window Wn)
    {
      //// Verify the collection of custom task panes to dispose of custom task panes pointing to closed (invalid) windows.
      bool disposePane = false;
      foreach (Office.CustomTaskPane customPane in CustomTaskPanes)
      {
        if (!(customPane.Control is ExcelAddInPane))
        {
          //// If a custom task pane has been created for a different Add-In then skip it.
          continue;
        }

        try
        {
          Excel.Window customPaneWindow = customPane.Window as Excel.Window;
        }
        catch
        {
          //// If an error ocurred trying to access the custom task pane window, it means its window is no longer valid
          ////  or in other words, it has been closed. There is no other way to find out if a windows was closed
          ////  (similar to the way we find out if a Worksheet has been closed as there are no events for that).
          disposePane = true;
        }

        if (disposePane)
        {
          ExcelAddInPane excelPane = customPane.Control as ExcelAddInPane;
          CloseExcelPane(excelPane);
          break;
        }
      }

      //// Synchronize the MySQL for Excel toggle button state of the currently activated window.
      Office.Ribbon.RibbonControl ribbonControl = Globals.Ribbons.ManageTaskPaneRibbon.MySQLExcelAddInRibbonGroup.Items.FirstOrDefault(rc => rc.Name == "ShowTaskPaneRibbonToggleButton");
      if (ribbonControl != null && ribbonControl is Office.Ribbon.RibbonToggleButton)
      {
        Office.Ribbon.RibbonToggleButton toggleButton = ribbonControl as Office.Ribbon.RibbonToggleButton;
        toggleButton.Checked = ActiveCustomPane != null && ActiveCustomPane.Visible;
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
    /// Event delegate method fired when the <see cref="taskPaneValue"/> visible property value changes.
    /// </summary>
    /// <param name="sender">Sender object.</param>
    /// <param name="e">Sender object.</param>
    private void CustomTaskPaneVisibleChanged(object sender, EventArgs e)
    {
      Office.CustomTaskPane customTaskPane = sender as Office.CustomTaskPane;
      Globals.Ribbons.ManageTaskPaneRibbon.ShowTaskPaneRibbonToggleButton.Checked = customTaskPane.Visible;
    }

    /// <summary>
    /// Event delegate method fired when the <see cref="ExcelPane"/> size changes.
    /// </summary>
    /// <param name="sender">Sender object.</param>
    /// <param name="e">Event arguments.</param>
    private void ExcelPane_SizeChanged(object sender, EventArgs e)
    {
      ExcelAddInPane excelPane = sender as ExcelAddInPane;

      //// Find the parent Custom Task Pane
      Office.CustomTaskPane customTaskPane = CustomTaskPanes.FirstOrDefault(ctp => ctp.Control == excelPane);
      if (customTaskPane == null || !customTaskPane.Visible)
      {
        return;
      }

      //// Since there is no way to restrict the resizing of a custom task pane, cancel the resizing as soon as a
      ////  user attempts to resize the pane.
      bool shouldResetWidth = customTaskPane.Width != ADD_IN_PANE_WIDTH && Application.Width >= ADD_IN_PANE_WIDTH;
      if (shouldResetWidth)
      {
        try
        {
          SendKeys.Send(ESCAPE_KEY);
          customTaskPane.Width = ADD_IN_PANE_WIDTH;
        }
        catch (Exception ex)
        {
          MySQLSourceTrace.WriteAppErrorToLog(ex);
        }
      }
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
    /// Event delegate method fired when the <see cref="ThisAddIn"/> is closed.
    /// </summary>
    /// <param name="sender">Sender object.</param>
    /// <param name="e">Event arguments.</param>
    private void ThisAddIn_Shutdown(object sender, System.EventArgs e)
    {
      MySQLSourceTrace.WriteToLog(Properties.Resources.ShutdownMessage, SourceLevels.Information);

      //// Close all Excel panes created
      foreach (ExcelAddInPane excelPane in ExcelPanesList)
      {
        excelPane.Dispose();
      }
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
        //// Make sure the settings directory exists
        Directory.CreateDirectory(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + @"\Oracle\MySQL for Excel");

        //// Static initializations.
        System.Windows.Forms.Application.EnableVisualStyles();
        System.Windows.Forms.Application.SetCompatibleTextRenderingDefault(false);
        CustomizeInfoDialog();
        InitializeMySQLWorkbenchStaticSettings();
        AssemblyTitle = AssemblyInfo.AssemblyTitle;

        //// Log the Add-In's Startup
        MySQLSourceTrace.WriteToLog(Properties.Resources.StartupMessage, SourceLevels.Information);

        //// Detect Excel version.
        int pointPos = Application.Version.IndexOf('.');
        string majorVersionText = pointPos >= 0 ? Application.Version.Substring(0, pointPos) : Application.Version;
        ExcelVersionNumber = Int32.Parse(majorVersionText, CultureInfo.InvariantCulture);

        //// This method is used to migrate all connections created with 1.0.6 (in a local connections file) to the Workbench connections file.
        MySqlWorkbench.MigrateExternalConnectionsToWorkbench();

        //// If the Excel version corresponds to Excel 2013 or greater we need to monitoring the Excel windows activation and deactivation
        ////  in order to synchronize the Add-In's toggle button state and dispose custom task panes when its parent window closes.
        if (ExcelVersionNumber >= EXCEL_2013_VERSION_NUMBER)
        {
          Application.WindowActivate += Application_WindowActivate;
        }
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