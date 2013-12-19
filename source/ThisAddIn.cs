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

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using MySQL.ForExcel.Classes;
using MySQL.ForExcel.Controls;
using MySQL.ForExcel.Properties;
using MySQL.Utility.Classes;
using MySQL.Utility.Classes.MySQLWorkbench;
using MySQL.Utility.Forms;
using Excel = Microsoft.Office.Interop.Excel;
using Office = Microsoft.Office.Tools;
using OfficeCore = Microsoft.Office.Core;

namespace MySQL.ForExcel
{
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
          bool isParentWindowActiveExcelWindow;
          if (ExcelVersionNumber >= EXCEL_2013_VERSION_NUMBER)
          {
            // If running on Excel 2013 or later a MDI is used for the windows so the active custom pane is matched with its
            // window and the application active window.
            Excel.Window paneWindow = ctp.Window as Excel.Window;
            isParentWindowActiveExcelWindow = paneWindow != null && paneWindow.Hwnd == Application.ActiveWindow.Hwnd;
          }
          else
          {
            // If running on Excel 2007 or 2010 a SDI is used so the active custom pane is the first one of an Excel Add-In.
            isParentWindowActiveExcelWindow = true;
          }

          return isParentWindowActiveExcelWindow && ctp.Control is ExcelAddInPane;
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

    /// <summary>
    /// Gets a list of <see cref="EditSessionInfo"/> objects saved to disk.
    /// </summary>
    public List<EditSessionInfo> StoredEditSessions
    {
      get
      {
        return Settings.Default.EditSessionsList ?? (Settings.Default.EditSessionsList = new List<EditSessionInfo>());
      }
    }

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

      // If there is no custom pane associated to the Excel Add-In in the active window, create one.
      if (activeCustomPane != null)
      {
        return activeCustomPane;
      }

      Application.Cursor = Excel.XlMousePointer.xlWait;
      if (ExcelPanesList == null)
      {
        ExcelPanesList = new List<ExcelAddInPane>();
      }

      // Instantiate the Excel Add-In pane to attach it to the Excel's custom task pane.
      // Note that in Excel 2007 and 2010 a MDI model is used so only a single Excel pane is instantiated, whereas in Excel 2013 and greater
      //  a SDI model is used instead, so an Excel pane is instantiated for each custom task pane appearing in each Excel window.
      ExcelAddInPane excelPane = new ExcelAddInPane { Dock = DockStyle.Fill };
      excelPane.SizeChanged += ExcelPane_SizeChanged;
      ExcelPanesList.Add(excelPane);

      // Create a new custom task pane and initialize it.
      activeCustomPane = CustomTaskPanes.Add(excelPane, AssemblyTitle);
      activeCustomPane.VisibleChanged += CustomTaskPaneVisibleChanged;
      activeCustomPane.DockPosition = OfficeCore.MsoCTPDockPosition.msoCTPDockPositionRight;
      activeCustomPane.DockPositionRestrict = OfficeCore.MsoCTPDockPositionRestrict.msoCTPDockPositionRestrictNoHorizontal;
      activeCustomPane.Width = ADD_IN_PANE_WIDTH;

      // Create custom MySQL Excel table style in the active workbook if it exists
      Application.ActiveWorkbook.CreateMySqlTableStyle();

      Application.Cursor = Excel.XlMousePointer.xlDefault;
      return activeCustomPane;
    }

    /// <summary>
    /// Event delegate method fired when an Excel <see cref="Excel.Worksheet"/> is activated.
    /// </summary>
    /// <param name="workSheet">A <see cref="Excel.Worksheet"/> object.</param>
    private void Application_SheetActivate(object workSheet)
    {
      if (ActiveExcelPane == null)
      {
        return;
      }

      Excel.Worksheet activeSheet = workSheet as Excel.Worksheet;
      ActiveExcelPane.ChangeEditDialogVisibility(activeSheet, true);
    }

    /// <summary>
    /// Event delegate method fired before an Excel <see cref="Excel.Worksheet"/> is deleted.
    /// </summary>
    /// <param name="workSheet">A <see cref="Excel.Worksheet"/> object.</param>
    private void Application_SheetBeforeDelete(object workSheet)
    {
      if (ActiveExcelPane == null)
      {
        return;
      }

      Excel.Worksheet activeSheet = workSheet as Excel.Worksheet;
      ActiveExcelPane.CloseWorksheetEditSession(activeSheet);
    }

    /// <summary>
    /// Event delegate method fired when the contents of the current selection of Excel cells in a given <see cref="Excel.Worksheet"/> change.
    /// </summary>
    /// <param name="workSheet">A <see cref="Excel.Worksheet"/> object.</param>
    /// <param name="targetRange">A selection of Excel cells.</param>
    private void Application_SheetChange(object workSheet, Excel.Range targetRange)
    {
      if (ActiveExcelPane == null)
      {
        return;
      }

      ActiveExcelPane.UpdateExcelSelectedDataStatus(targetRange);
    }

    /// <summary>
    /// Event delegate method fired when an Excel <see cref="Excel.Worksheet"/> is deactivated.
    /// </summary>
    /// <param name="workSheet">A <see cref="Excel.Worksheet"/> object.</param>
    private void Application_SheetDeactivate(object workSheet)
    {
      if (ActiveExcelPane == null)
      {
        return;
      }

      Excel.Worksheet deactivatedSheet = workSheet as Excel.Worksheet;
      ActiveExcelPane.ChangeEditDialogVisibility(deactivatedSheet, false);
    }

    /// <summary>
    /// Event delegate method fired when the selection of Excel cells in a given <see cref="Excel.Worksheet"/> changes.
    /// </summary>
    /// <param name="workSheet">A <see cref="Excel.Worksheet"/> object.</param>
    /// <param name="targetRange">The new selection of Excel cells.</param>
    private void Application_SheetSelectionChange(object workSheet, Excel.Range targetRange)
    {
      if (ActiveExcelPane == null)
      {
        return;
      }

      ActiveExcelPane.UpdateExcelSelectedDataStatus(targetRange);
    }

    /// <summary>
    /// Event delegate method fired when an Excel window is activated.
    /// </summary>
    /// <param name="workbook">The Excel workbook tied to the activated window.</param>
    /// <param name="window">The activated Excel window.</param>
    private void Application_WindowActivate(Excel.Workbook workbook, Excel.Window window)
    {
      // Verify the collection of custom task panes to dispose of custom task panes pointing to closed (invalid) windows.
      bool disposePane = false;
      foreach (Office.CustomTaskPane customPane in CustomTaskPanes.Where(customPane => customPane.Control is ExcelAddInPane))
      {
        try
        {
          // Do NOT remove the following line although the customPaneWindow variable is not used in the method the casting
          // of the customPane.Window is needed to determine if the window is still valid and has not been disposed of.
          Excel.Window customPaneWindow = customPane.Window as Excel.Window;
        }
        catch
        {
          // If an error ocurred trying to access the custom task pane window, it means its window is no longer valid
          //  or in other words, it has been closed. There is no other way to find out if a windows was closed
          //  (similar to the way we find out if a Worksheet has been closed as there are no events for that).
          disposePane = true;
        }

        if (!disposePane)
        {
          continue;
        }

        ExcelAddInPane excelPane = customPane.Control as ExcelAddInPane;
        CloseExcelPane(excelPane);
        break;
      }

      // Synchronize the MySQL for Excel toggle button state of the currently activated window.
      Office.Ribbon.RibbonControl ribbonControl = Globals.Ribbons.ManageTaskPaneRibbon.MySQLExcelAddInRibbonGroup.Items.FirstOrDefault(rc => rc.Name == "ShowTaskPaneRibbonToggleButton");
      if (!(ribbonControl is Office.Ribbon.RibbonToggleButton))
      {
        return;
      }

      Office.Ribbon.RibbonToggleButton toggleButton = ribbonControl as Office.Ribbon.RibbonToggleButton;
      toggleButton.Checked = ActiveCustomPane != null && ActiveCustomPane.Visible;
    }

    /// <summary>
    /// Event delegate method fired when a <see cref="Excel.Workbook"/> is activated.
    /// </summary>
    /// <param name="workBook">A <see cref="Excel.Workbook"/> object.</param>
    private void Application_WorkbookActivate(object workBook)
    {
      if (ActiveExcelPane == null)
      {
        return;
      }

      Excel.Workbook activeWorkbook = workBook as Excel.Workbook;
      Excel.Worksheet activeSheet = activeWorkbook != null ? activeWorkbook.ActiveSheet as Excel.Worksheet : null;
      ActiveExcelPane.ChangeEditDialogVisibility(activeSheet, true);
    }

    /// <summary>
    /// Event delegate method fired before a <see cref="Excel.Workbook"/> is closed.
    /// </summary>
    /// <param name="workbook">A <see cref="Excel.Workbook"/> object.</param>
    /// <param name="cancel">Flag indicating whether the user cancelled the closing event.</param>
    private void Application_WorkbookBeforeClose(Excel.Workbook workbook, ref bool cancel)
    {
      if (ActiveExcelPane == null)
      {
        return;
      }

      bool flagAsSaved = false;
      if (!workbook.Saved)
      {
        switch (MessageBox.Show(string.Format(Resources.WorkbookSavingDetailText, workbook.Name), Application.Name, MessageBoxButtons.YesNoCancel, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button1))
        {
          case DialogResult.Yes:
            workbook.Save();
            break;

          case DialogResult.No:
            flagAsSaved = true;
            break;

          case DialogResult.Cancel:
            cancel = true;
            return;
        }
      }

      ActiveExcelPane.CloseWorkbookEditSessions(workbook);
      if (flagAsSaved)
      {
        workbook.Saved = true;
      }
    }

    /// <summary>
    /// Event delegate method fired after an Excel <see cref="Excel.Workbook"/> is saved to disk.
    /// </summary>
    /// <param name="workbook">A <see cref="Excel.Workbook"/> object.</param>
    /// <param name="success">Flag indicating whether the save operation was successful.</param>
    private void Application_WorkbookAfterSave(Excel.Workbook workbook, bool success)
    {
      if (ActiveExcelPane == null || ActiveExcelPane.WorkbookEditSessions == null || ActiveExcelPane.WorkbookEditSessions.Count <= 0)
      {
        return;
      }

      if (ActiveExcelPane.WorkbookEditSessions == null
          || !ActiveExcelPane.WorkbookEditSessions.Exists(session => session.EditDialog != null && session.EditDialog.WorkbookName == workbook.Name))
      {
        return;
      }

      foreach (var activeSession in workbook.Worksheets.Cast<Excel.Worksheet>().Select(worksheet => ActiveExcelPane.WorkbookEditSessions.GetActiveEditSession(worksheet)).Where(activeSession => activeSession != null))
      {
        activeSession.EditDialog.ProtectWorksheet();
      }

      workbook.Saved = true;
    }

    /// <summary>
    /// Event delegate method fired before an Excel <see cref="Excel.Workbook"/> is saved to disk.
    /// </summary>
    /// <param name="workbook">A <see cref="Excel.Workbook"/> object.</param>
    /// <param name="saveAsUi">Flag indicating whether the Save As dialog was displayed.</param>
    /// <param name="cancel">Flag indicating whether the user cancelled the saving event.</param>
    private void Application_WorkbookBeforeSave(Excel.Workbook workbook, bool saveAsUi, ref bool cancel)
    {
      if (ActiveExcelPane == null || ActiveExcelPane.WorkbookEditSessions == null || ActiveExcelPane.WorkbookEditSessions.Count <= 0)
      {
        return;
      }

      // Unprotect all worksheets with an active edit session.
      foreach (var activeSession in ActiveExcelPane.WorkbookEditSessions)
      {
        activeSession.EditDialog.UnprotectWorksheet();

        // Add new Edit sessions in memory collection to serialized collection
        if (!Settings.Default.EditSessionsList.Exists(session => session.HasSameWorkbookAndTable(activeSession)))
        {
          Settings.Default.EditSessionsList.Add(activeSession);
        }
      }

      // Remove deleted Edit sessions from memory collection also from serialized collection
      foreach (var deletedSession in Settings.Default.EditSessionsList.FindAll(deletedSession => !ActiveExcelPane.WorkbookEditSessions.Exists(session => session.HasSameWorkbookAndTable(deletedSession))))
      {
        Settings.Default.EditSessionsList.Remove(deletedSession);
      }

      Settings.Default.Save();
    }

    /// <summary>
    /// Event delegate method fired when an Excel <see cref="Excel.Workbook"/> is deactivated.
    /// </summary>
    /// <param name="workbook">A <see cref="Excel.Workbook"/> object.</param>
    private void Application_WorkbookDeactivate(object workbook)
    {
      if (ActiveExcelPane == null)
      {
        return;
      }

      // Hide editDialogs from deactivated Workbook
      Excel.Workbook deactivatedWorkbook = workbook as Excel.Workbook;
      if (deactivatedWorkbook == null)
      {
        return;
      }

      foreach (Excel.Worksheet wSheet in deactivatedWorkbook.Worksheets)
      {
        ActiveExcelPane.ChangeEditDialogVisibility(wSheet, false);
      }
    }

    /// <summary>
    /// Event delegate method fired when a <see cref="Excel.Workbook"/> is opened.
    /// </summary>
    /// <param name="workbook">The <see cref="Excel.Workbook"/> being opened.</param>
    private void Application_WorkbookOpen(Excel.Workbook workbook)
    {
      if (workbook == null)
      {
        return;
      }

      // Add the custom MySQL table style (for Excel tables) to this workbook.
      workbook.CreateMySqlTableStyle();

      if (ActiveExcelPane == null)
      {
        return;
      }

      // Attempt to restore Edit sessions for the workbook beeing opened
      ActiveExcelPane.RestoreEditSessions(workbook);
    }

    /// <summary>
    /// Converts the settings stored mappings property to the renamed MySqlColumnMapping class.
    /// </summary>
    private static void ConvertSettingsStoredMappingsCasing()
    {
      // Check if settings file exists, if it does not flag the conversion as done since it was not needed.
      MySqlForExcelSettings settings = new MySqlForExcelSettings();
      if (!File.Exists(settings.SettingsPath))
      {
        Settings.Default.ConvertedSettingsStoredMappingsCasing = true;
        return;
      }

      // Open the settings.config file for writing and convert the MySQLColumnMapping class to MySqlColumnMapping.
      try
      {
        bool converted = false;
        string settingsConfigText = File.ReadAllText(settings.SettingsPath, Encoding.Unicode);
        if (settingsConfigText.Contains("MySQLColumnMapping"))
        {
          settingsConfigText = settingsConfigText.Replace("MySQLColumnMapping", "MySqlColumnMapping");
          converted = true;
        }

        if (!converted)
        {
          return;
        }

        File.WriteAllText(settings.SettingsPath, settingsConfigText, Encoding.Unicode);
        Settings.Default.Reload();
        Settings.Default.ConvertedSettingsStoredMappingsCasing = true;
        Settings.Default.Save();
      }
      catch (Exception ex)
      {
        MySqlSourceTrace.WriteAppErrorToLog(ex);
      }
    }

    /// <summary>
    /// Customizes the looks of the <see cref="MySQL.Utility.Forms.InfoDialog"/> form for MySQL for Excel.
    /// </summary>
    private void CustomizeInfoDialog()
    {
      InfoDialog.ApplicationName = AssemblyTitle;
      InfoDialog.SuccessLogo = Resources.MySQLforExcel_InfoDlg_Success_64x64;
      InfoDialog.ErrorLogo = Resources.MySQLforExcel_InfoDlg_Error_64x64;
      InfoDialog.WarningLogo = Resources.MySQLforExcel_InfoDlg_Warning_64x64;
      InfoDialog.InformationLogo = Resources.MySQLforExcel_Logo_64x64;
    }

    /// <summary>
    /// Event delegate method fired when the <see cref="Office.CustomTaskPane"/> visible property value changes.
    /// </summary>
    /// <param name="sender">Sender object.</param>
    /// <param name="e">Sender object.</param>
    private static void CustomTaskPaneVisibleChanged(object sender, EventArgs e)
    {
      Office.CustomTaskPane customTaskPane = sender as Office.CustomTaskPane;
      Globals.Ribbons.ManageTaskPaneRibbon.ShowTaskPaneRibbonToggleButton.Checked = customTaskPane != null && customTaskPane.Visible;
    }

    /// <summary>
    /// Event delegate method fired when the <see cref="ExcelAddInPane"/> size changes.
    /// </summary>
    /// <param name="sender">Sender object.</param>
    /// <param name="e">Event arguments.</param>
    private void ExcelPane_SizeChanged(object sender, EventArgs e)
    {
      ExcelAddInPane excelPane = sender as ExcelAddInPane;

      // Find the parent Custom Task Pane
      Office.CustomTaskPane customTaskPane = CustomTaskPanes.FirstOrDefault(ctp => ctp.Control == excelPane);
      if (customTaskPane == null || !customTaskPane.Visible)
      {
        return;
      }

      // Since there is no way to restrict the resizing of a custom task pane, cancel the resizing as soon as a
      //  user attempts to resize the pane.
      bool shouldResetWidth = customTaskPane.Width != ADD_IN_PANE_WIDTH && Application.Width >= ADD_IN_PANE_WIDTH;
      if (!shouldResetWidth)
      {
        return;
      }

      try
      {
        SendKeys.Send(ESCAPE_KEY);
        customTaskPane.Width = ADD_IN_PANE_WIDTH;
      }
      catch (Exception ex)
      {
        MySqlSourceTrace.WriteAppErrorToLog(ex);
      }
    }

    /// <summary>
    /// Initializes settings for the <see cref="MySqlWorkbench"/> and <see cref="MySqlWorkbenchPasswordVault"/> classes.
    /// </summary>
    private void InitializeMySqlWorkbenchStaticSettings()
    {
      string applicationDataFolderPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
      MySqlWorkbench.ExternalApplicationName = AssemblyTitle;
      MySqlWorkbenchPasswordVault.ApplicationPasswordVaultFilePath = applicationDataFolderPath + @"\Oracle\MySQL for Excel\user_data.dat";
      MySqlWorkbench.ExternalApplicationConnectionsFilePath = applicationDataFolderPath + @"\Oracle\MySQL for Excel\connections.xml";
      MySqlSourceTrace.LogFilePath = applicationDataFolderPath + @"\Oracle\MySQL for Excel\MySQLForExcel.log";
      MySqlSourceTrace.SourceTraceClass = "MySQLForExcel";
    }

    /// <summary>
    /// Setups the excel events.
    /// </summary>
    /// <param name="subscribe">if set to <c>true</c> [subscribe].</param>
    private void SetupExcelEvents(bool subscribe)
    {
      if (subscribe)
      {
        // Excel version corresponds to Excel 2013 or greater.
        if (ExcelVersionNumber >= EXCEL_2013_VERSION_NUMBER)
        {
          //  Monitor the Excel windows activation and deactivation in order to synchronize the Add-In's toggle button state and dispose custom task panes when its parent window closes.
          Application.WindowActivate += Application_WindowActivate;
        }

        Application.SheetActivate += Application_SheetActivate;
        Application.SheetBeforeDelete += Application_SheetBeforeDelete;
        Application.SheetChange += Application_SheetChange;
        Application.SheetDeactivate += Application_SheetDeactivate;
        Application.SheetSelectionChange += Application_SheetSelectionChange;
        Application.WorkbookActivate += Application_WorkbookActivate;
        Application.WorkbookAfterSave += Application_WorkbookAfterSave;
        Application.WorkbookBeforeClose += Application_WorkbookBeforeClose;
        Application.WorkbookBeforeSave += Application_WorkbookBeforeSave;
        Application.WorkbookDeactivate += Application_WorkbookDeactivate;
        Application.WorkbookOpen += Application_WorkbookOpen;
      }
      else
      {
        if (ExcelVersionNumber >= EXCEL_2013_VERSION_NUMBER)
        {
          Application.WindowActivate -= Application_WindowActivate;
        }

        Application.SheetActivate -= Application_SheetActivate;
        Application.SheetBeforeDelete -= Application_SheetBeforeDelete;
        Application.SheetChange -= Application_SheetChange;
        Application.SheetDeactivate -= Application_SheetDeactivate;
        Application.SheetSelectionChange -= Application_SheetSelectionChange;
        Application.WorkbookActivate -= Application_WorkbookActivate;
        Application.WorkbookAfterSave -= Application_WorkbookAfterSave;
        Application.WorkbookBeforeClose -= Application_WorkbookBeforeClose;
        Application.WorkbookBeforeSave -= Application_WorkbookBeforeSave;
        Application.WorkbookDeactivate -= Application_WorkbookDeactivate;
        Application.WorkbookOpen -= Application_WorkbookOpen;
      }
    }

    /// <summary>
    /// Event delegate method fired when the <see cref="ThisAddIn"/> is closed.
    /// </summary>
    /// <param name="sender">Sender object.</param>
    /// <param name="e">Event arguments.</param>
    private void ThisAddIn_Shutdown(object sender, EventArgs e)
    {
      MySqlSourceTrace.WriteToLog(Resources.ShutdownMessage, SourceLevels.Information);

      // Close all Excel panes created
      if (ExcelPanesList == null)
      {
        return;
      }

      // Unsibscribe from Excel events
      SetupExcelEvents(false);

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
    private void ThisAddIn_Startup(object sender, EventArgs e)
    {
      try
      {
        // Make sure the settings directory exists
        Directory.CreateDirectory(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + @"\Oracle\MySQL for Excel");

        // Static initializations.
        System.Windows.Forms.Application.EnableVisualStyles();
        System.Windows.Forms.Application.SetCompatibleTextRenderingDefault(false);
        CustomizeInfoDialog();
        InitializeMySqlWorkbenchStaticSettings();
        AssemblyTitle = AssemblyInfo.AssemblyTitle;

        // Log the Add-In's Startup
        MySqlSourceTrace.WriteToLog(Resources.StartupMessage, SourceLevels.Information);

        // Detect Excel version.
        int pointPos = Application.Version.IndexOf('.');
        string majorVersionText = pointPos >= 0 ? Application.Version.Substring(0, pointPos) : Application.Version;
        ExcelVersionNumber = Int32.Parse(majorVersionText, CultureInfo.InvariantCulture);

        // Convert the StoredDataMappings setting's data type to MySql
        if (!Settings.Default.ConvertedSettingsStoredMappingsCasing)
        {
          ConvertSettingsStoredMappingsCasing();
        }

        // This method is used to migrate all connections created with 1.0.6 (in a local connections file) to the Workbench connections file.
        MySqlWorkbench.MigrateExternalConnectionsToWorkbench();

        // Subscribe to Excel events
        SetupExcelEvents(true);
      }
      catch (Exception ex)
      {
        MySqlSourceTrace.WriteAppErrorToLog(ex);
      }
    }

    #region VSTO generated code

    /// <summary>
    /// Required method for Designer support - do not modify
    /// the contents of this method with the code editor.
    /// </summary>
    private void InternalStartup()
    {
      Startup += ThisAddIn_Startup;
      Shutdown += ThisAddIn_Shutdown;
    }

    #endregion VSTO generated code
  }
}