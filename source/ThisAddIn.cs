// Copyright (c) 2012-2014, Oracle and/or its affiliates. All rights reserved.
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
using MySQL.ForExcel.Forms;
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

    #region Fields

    /// <summary>
    /// A dictionary containing subsets of the <see cref="StoredEditSessions"/> list containing only sessions of a <see cref="Excel.Workbook"/>.
    /// </summary>
    private Dictionary<string, List<EditSessionInfo>> _editSessionsByWorkbook;

    /// <summary>
    /// The name of the last deactivated Excel <see cref="Excel.Worksheet"/>.
    /// </summary>
    private string _lastDeactivatedSheetName;

    /// <summary>
    /// True while restoring existing sessions for the current workbook, avoiding unwanted actions to be raised during the process.
    /// </summary>
    private bool _restoringExistingSession;

    #endregion Fields

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
            isParentWindowActiveExcelWindow = paneWindow != null && Application.ActiveWindow != null && paneWindow.Hwnd == Application.ActiveWindow.Hwnd;
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
    /// Gets a subset of the <see cref="StoredEditSessions"/> list containing only sessions assocaiated to the active <see cref="Excel.Workbook"/>.
    /// </summary>
    public List<EditSessionInfo> ActiveWorkbookSessions
    {
      get
      {
        return GetWorkbookEditSessions(Application.ActiveWorkbook);
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
    /// Gets or sets a value indicating whether the detection of contents for a cell selection should be skipped.
    /// Used mainly when a cells selection is being done programatically and not by the user.
    /// </summary>
    public bool SkipSelectedDataContentsDetection { get; set; }

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

    /// <summary>
    /// Gets or sets a value indicating whether a temporary hidden <see cref="Excel.Worksheet"/> is being used by a <see cref="TempRange"/> instance.
    /// </summary>
    public bool UsingTempWorksheet { get; set; }

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
      if (ExcelPanesList.Count == 0)
      {
        ExcelAddInPanesClosed();
      }

      excelPane.Dispose();
      if (customPane != null)
      {
        customPane.Dispose();
      }

      CustomTaskPanes.Remove(customPane);
    }

    /// <summary>
    /// Closes and removes all Edit sessions associated to the given <see cref="Excel.Workbook"/>.
    /// </summary>
    /// <param name="workbook">The <see cref="Excel.Workbook"/> associated to the Edit sessions to close.</param>
    public void CloseWorkbookEditSessions(Excel.Workbook workbook)
    {
      if (workbook == null)
      {
        return;
      }

      var workbookSessions = GetWorkbookEditSessions(workbook);
      var sessionsToFreeResources = workbookSessions.FindAll(session => session.EditDialog != null && session.EditDialog.WorkbookName == workbook.Name);
      foreach (var session in sessionsToFreeResources)
      {
        // The Close method is both closing the dialog and removing itself from the collection of EditSessionInfo objects.
        session.EditDialog.Close();
      }
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

      bool firstRun = ExcelPanesList.Count == 0;

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

      // Create custom MySQL Excel table style and localized date format strings in the active workbook if it exists.
      Application.ActiveWorkbook.CreateMySqlTableStyle();
      Application.ActiveWorkbook.AddLocalizedDateFormatStringsAsNames();

      // First run if no Excel panes have been opened yet.
      if (firstRun)
      {
        ExcelAddInPaneFirstRun();
      }

      Application.Cursor = Excel.XlMousePointer.xlDefault;
      return activeCustomPane;
    }

    /// <summary>
    /// Event delegate method fired when an Excel <see cref="Excel.Worksheet"/> is activated.
    /// </summary>
    /// <param name="workSheet">A <see cref="Excel.Worksheet"/> object.</param>
    private void Application_SheetActivate(object workSheet)
    {
      if (ActiveExcelPane == null || UsingTempWorksheet)
      {
        return;
      }

      Excel.Worksheet activeSheet = workSheet as Excel.Worksheet;
      if (!activeSheet.IsVisible())
      {
        return;
      }

      if (_lastDeactivatedSheetName.Length > 0 && !Application.ActiveWorkbook.WorksheetExists(_lastDeactivatedSheetName))
      {
        // Worksheet was deleted and the Application_SheetBeforeDelete did not run, user is running Excel 2010 or earlier.
        CloseMissingWorksheetEditSession(Application.ActiveWorkbook, _lastDeactivatedSheetName);
      }

      ChangeEditDialogVisibility(activeSheet, true);
    }

    /// <summary>
    /// Event delegate method fired before an Excel <see cref="Excel.Worksheet"/> is deleted.
    /// </summary>
    /// <param name="workSheet">A <see cref="Excel.Worksheet"/> object.</param>
    private void Application_SheetBeforeDelete(object workSheet)
    {
      if (ActiveExcelPane == null || UsingTempWorksheet)
      {
        return;
      }

      Excel.Worksheet activeSheet = workSheet as Excel.Worksheet;
      if (!activeSheet.IsVisible())
      {
        return;
      }

      CloseWorksheetEditSession(activeSheet);

      // If the _lastDeactivatedSheetName is not empty it means a deactivated sheet may have been deleted, if this method ran it means the user is running
      // Excel 2013 or later where this method is supported, so we need to clean the _lastDeactivatedSheetName.
      if (_lastDeactivatedSheetName.Length > 0)
      {
        _lastDeactivatedSheetName = string.Empty;
      }
    }

    /// <summary>
    /// Event delegate method fired when the contents of the current selection of Excel cells in a given <see cref="Excel.Worksheet"/> change.
    /// </summary>
    /// <param name="workSheet">A <see cref="Excel.Worksheet"/> object.</param>
    /// <param name="targetRange">A selection of Excel cells.</param>
    private void Application_SheetChange(object workSheet, Excel.Range targetRange)
    {
      if (ActiveExcelPane == null || UsingTempWorksheet)
      {
        return;
      }

      Excel.Worksheet activeSheet = workSheet as Excel.Worksheet;
      if (!activeSheet.IsVisible())
      {
        return;
      }

      if (!SkipSelectedDataContentsDetection)
      {
        ActiveExcelPane.UpdateExcelSelectedDataStatus(targetRange);
      }
    }

    /// <summary>
    /// Event delegate method fired when an Excel <see cref="Excel.Worksheet"/> is deactivated.
    /// </summary>
    /// <param name="workSheet">A <see cref="Excel.Worksheet"/> object.</param>
    private void Application_SheetDeactivate(object workSheet)
    {
      if (ActiveExcelPane == null || UsingTempWorksheet)
      {
        return;
      }

      Excel.Worksheet deactivatedSheet = workSheet as Excel.Worksheet;
      if (!deactivatedSheet.IsVisible())
      {
        return;
      }

      _lastDeactivatedSheetName = deactivatedSheet != null ? deactivatedSheet.Name : string.Empty;
      ChangeEditDialogVisibility(deactivatedSheet, false);
    }

    /// <summary>
    /// Event delegate method fired when the selection of Excel cells in a given <see cref="Excel.Worksheet"/> changes.
    /// </summary>
    /// <param name="workSheet">A <see cref="Excel.Worksheet"/> object.</param>
    /// <param name="targetRange">The new selection of Excel cells.</param>
    private void Application_SheetSelectionChange(object workSheet, Excel.Range targetRange)
    {
      if (ActiveExcelPane == null || UsingTempWorksheet)
      {
        return;
      }

      Excel.Worksheet activeSheet = workSheet as Excel.Worksheet;
      if (!activeSheet.IsVisible())
      {
        return;
      }

      if (!SkipSelectedDataContentsDetection)
      {
        ActiveExcelPane.UpdateExcelSelectedDataStatus(targetRange);
      }
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
      ChangeEditDialogVisibility(activeSheet, true);
      ActiveExcelPane.RefreshDbObjectPanelActionLabelsEnabledStatus();
    }

    /// <summary>
    /// Event delegate method fired after an Excel <see cref="Excel.Workbook"/> is saved to disk.
    /// </summary>
    /// <param name="workbook">A <see cref="Excel.Workbook"/> object.</param>
    /// <param name="success">Flag indicating whether the save operation was successful.</param>
    private void Application_WorkbookAfterSave(Excel.Workbook workbook, bool success)
    {
      var workbookId = workbook.GetOrCreateId();
      var workbookSessions = GetWorkbookEditSessions(workbook);

      // Protect all worksheets with an active edit session.
      foreach (var activeSession in workbookSessions)
      {
        activeSession.WorkbookFilePath = workbook.FullName;
        if (activeSession.EditDialog != null)
        {
          activeSession.EditDialog.ProtectWorksheet();
        }

        var storedSession = StoredEditSessions.FirstOrDefault(session => session.HasSameWorkbookAndTable(activeSession));
        if (storedSession != null)
        {
          storedSession.WorkbookFilePath = workbook.FullName;
          continue;
        }

        // Add new Edit sessions in memory collection to serialized collection
        StoredEditSessions.Add(activeSession);
      }

      // Remove deleted Edit sessions from memory collection also from serialized collection
      foreach (var storedSession in StoredEditSessions.FindAll(storedSession => string.Equals(storedSession.WorkbookGuid, workbookId, StringComparison.InvariantCulture) && !workbookSessions.Exists(wbSession => wbSession.HasSameWorkbookAndTable(storedSession))))
      {
        StoredEditSessions.Remove(storedSession);
      }

      Settings.Default.Save();
      workbook.Saved = true;
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

      bool wasAlreadySaved = workbook.Saved;
      if (!wasAlreadySaved)
      {
        switch (MessageBox.Show(string.Format(Resources.WorkbookSavingDetailText, workbook.Name), Application.Name, MessageBoxButtons.YesNoCancel, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button1))
        {
          case DialogResult.Yes:
            workbook.Save();
            break;

          case DialogResult.No:
            wasAlreadySaved = true;
            break;

          case DialogResult.Cancel:
            cancel = true;
            return;
        }
      }

      CloseWorkbookEditSessions(workbook);
      if (wasAlreadySaved)
      {
        workbook.Saved = true;
      }

      // Remove the Edit sessions for the workbook being closed from the dictionary.
      _editSessionsByWorkbook.Remove(workbook.GetOrCreateId());
    }

    /// <summary>
    /// Event delegate method fired before an Excel <see cref="Excel.Workbook"/> is saved to disk.
    /// </summary>
    /// <param name="workbook">A <see cref="Excel.Workbook"/> object.</param>
    /// <param name="saveAsUi">Flag indicating whether the Save As dialog was displayed.</param>
    /// <param name="cancel">Flag indicating whether the user cancelled the saving event.</param>
    private void Application_WorkbookBeforeSave(Excel.Workbook workbook, bool saveAsUi, ref bool cancel)
    {
      var workbookSessions = GetWorkbookEditSessions(workbook);

      // Unprotect all worksheets with an active edit session.
      foreach (var activeSession in workbookSessions.Where(activeSession => activeSession.EditDialog != null))
      {
        activeSession.EditDialog.UnprotectWorksheet();
      }
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
        ChangeEditDialogVisibility(wSheet, false);
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

      // Add the custom MySQL table style (for Excel tables) and localized date format strings to this workbook.
      workbook.CreateMySqlTableStyle();
      workbook.AddLocalizedDateFormatStringsAsNames();

      if (ActiveExcelPane == null)
      {
        return;
      }

      ShowOpenEditSessionsDialog(workbook);
    }

    /// <summary>
    /// Shows or hides an Edit dialog associated to the given <see cref="Excel.Worksheet"/>.
    /// </summary>
    /// <param name="workSheet">A <see cref="Excel.Worksheet"/> object.</param>
    /// <param name="show">Flag indicating if the dialog will be shown or hidden.</param>
    private void ChangeEditDialogVisibility(Excel.Worksheet workSheet, bool show)
    {
      if (workSheet == null)
      {
        return;
      }

      var parentWorkbook = workSheet.Parent as Excel.Workbook;
      if (parentWorkbook == null)
      {
        return;
      }

      var workbookSessions = GetWorkbookEditSessions(parentWorkbook);
      if (workbookSessions.Count == 0 || _restoringExistingSession)
      {
        return;
      }

      var activeSession = workbookSessions.GetActiveEditSession(workSheet);
      if (activeSession == null)
      {
        return;
      }

      if (show)
      {
        activeSession.EditDialog.ShowDialog();
      }
      else
      {
        activeSession.EditDialog.Hide();
      }
    }

    /// <summary>
    /// Closes and removes the Edit session associated to the given <see cref="Excel.Worksheet"/>.
    /// </summary>
    /// <param name="workbook">An <see cref="Excel.Workbook"/>.</param>
    /// <param name="missingWorksheetName">The name of the <see cref="Excel.Worksheet"/> that no longer exists and that is associated to the Edit session to close.</param>
    private void CloseMissingWorksheetEditSession(Excel.Workbook workbook, string missingWorksheetName)
    {
      if (workbook == null || missingWorksheetName.Length == 0)
      {
        return;
      }

      var workbookSessions = GetWorkbookEditSessions(workbook);
      var wsSession = workbookSessions.FirstOrDefault(session => !session.EditDialog.EditingWorksheetExists);
      if (wsSession == null)
      {
        return;
      }

      wsSession.EditDialog.Close();
    }

    /// <summary>
    /// Closes and removes the Edit session associated to the given <see cref="Excel.Worksheet"/>.
    /// </summary>
    /// <param name="worksheet">The <see cref="Excel.Worksheet"/> associated to the Edit session to close.</param>
    private void CloseWorksheetEditSession(Excel.Worksheet worksheet)
    {
      if (worksheet == null)
      {
        return;
      }

      Excel.Workbook parentWorkbook = worksheet.Parent as Excel.Workbook;
      if (parentWorkbook == null)
      {
        return;
      }

      var workbookSessions = GetWorkbookEditSessions(parentWorkbook);
      var wsSession = workbookSessions.FirstOrDefault(session => string.Equals(session.EditDialog.WorkbookName, parentWorkbook.Name, StringComparison.InvariantCulture) && string.Equals(session.EditDialog.WorksheetName, worksheet.Name, StringComparison.InvariantCulture));
      if (wsSession == null)
      {
        return;
      }

      wsSession.EditDialog.Close();
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
    /// Delete the closed workbook's edit sessions from the settings file.
    /// </summary>
    private void DeleteCurrentWorkbookEditSessions(Excel.Workbook workbook)
    {
      if (workbook == null || string.IsNullOrEmpty(workbook.GetOrCreateId()))
      {
        return;
      }

      // Remove all sessions from the current workbook.
      var workbookSessions = GetWorkbookEditSessions(workbook);
      foreach (var session in workbookSessions)
      {
        StoredEditSessions.Remove(session);
      }

      Settings.Default.Save();
      if (workbookSessions.Count > 0)
      {
        _editSessionsByWorkbook.Remove(workbook.GetOrCreateId());
      }
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
    /// Performs initializations that must occur when the first Excel pane is opened by the user and not at the Add-In startup.
    /// </summary>
    private void ExcelAddInPaneFirstRun()
    {
      _editSessionsByWorkbook = new Dictionary<string, List<EditSessionInfo>>(StoredEditSessions.Count);
      _lastDeactivatedSheetName = string.Empty;
      _restoringExistingSession = false;
      SkipSelectedDataContentsDetection = false;

      // This method is used to migrate all connections created with 1.0.6 (in a local connections file) to the Workbench connections file.
      MySqlWorkbench.MigrateExternalConnectionsToWorkbench();

      // Subscribe to Excel events
      SetupExcelEvents(true);

      // Restore saved Edit sessons
      ShowOpenEditSessionsDialog(Application.ActiveWorkbook);
    }

    /// <summary>
    /// Performs clean-up code that must be done after all Excel panes have been closed by the user.
    /// </summary>
    private void ExcelAddInPanesClosed()
    {
      // Unsubscribe from Excel events
      SetupExcelEvents(false);
      _editSessionsByWorkbook.Clear();
    }

    /// <summary>
    /// Initializes settings for the <see cref="MySqlWorkbench"/> and <see cref="MySqlWorkbenchPasswordVault"/> classes.
    /// </summary>
    private void InitializeMySqlWorkbenchStaticSettings()
    {
      string applicationDataFolderPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
      MySqlWorkbench.ExternalApplicationName = AssemblyTitle;
      MySqlWorkbenchPasswordVault.ApplicationPasswordVaultFilePath = applicationDataFolderPath + @"\Oracle\MySQL for Excel\user_data.dat";
      MySqlWorkbench.ExternalConnections.CreateDefaultConnections = !MySqlWorkbench.IsInstalled && !File.Exists(MySqlWorkbench.ConnectionsFilePath) && MySqlWorkbench.Connections.Count == 0;
      MySqlWorkbench.ExternalApplicationConnectionsFilePath = applicationDataFolderPath + @"\Oracle\MySQL for Excel\connections.xml";
      MySqlSourceTrace.LogFilePath = applicationDataFolderPath + @"\Oracle\MySQL for Excel\MySQLForExcel.log";
      MySqlSourceTrace.SourceTraceClass = "MySQLForExcel";
    }

    /// <summary>
    /// Gets a subset of the <see cref="StoredEditSessions"/> list containing only sessions assocaiated to the given <see cref="Excel.Workbook"/>.
    /// </summary>
    /// <param name="workbook">A <see cref="Excel.Workbook"/> with active Edit sessions.</param>
    /// <returns>A subset of the <see cref="StoredEditSessions"/> list containing only sessions assocaiated to the given <see cref="Excel.Workbook"/></returns>
    private List<EditSessionInfo> GetWorkbookEditSessions(Excel.Workbook workbook)
    {
      List<EditSessionInfo> workbookSessions = null;
      string workbookId = workbook.GetOrCreateId();
      if (_editSessionsByWorkbook != null && !string.IsNullOrEmpty(workbookId))
      {
        if (_editSessionsByWorkbook.ContainsKey(workbookId))
        {
          workbookSessions = _editSessionsByWorkbook[workbookId];
        }
        else
        {
          workbookSessions = StoredEditSessions.FindAll(session => string.Equals(session.WorkbookGuid, workbookId, StringComparison.InvariantCulture));
          _editSessionsByWorkbook.Add(workbookId, workbookSessions);
        }
      }

      return workbookSessions ?? new List<EditSessionInfo>();
    }

    /// <summary>
    /// Opens an <see cref="EditDataDialog"/> representing a saved Edit session for each of the tables.
    /// </summary>
    /// <param name="workbook">The workbook.</param>
    private void OpenEditSessionsOfTables(Excel.Workbook workbook)
    {
      if (workbook == null)
      {
        return;
      }

      var workbookSessions = GetWorkbookEditSessions(workbook);
      if (workbookSessions.Count == 0)
      {
        return;
      }

      var missingTables = new List<string>();
      _restoringExistingSession = true;
      foreach (var session in workbookSessions)
      {
        DbObject sessionTableObject = ActiveExcelPane.LoadedTables.FirstOrDefault(dbo => string.Equals(dbo.Name, session.TableName, StringComparison.InvariantCulture));
        if (sessionTableObject == null)
        {
          missingTables.Add(session.TableName);
          continue;
        }

        ActiveExcelPane.EditTableData(sessionTableObject, true, workbook);
      }

      if (workbookSessions.Count - missingTables.Count > 0)
      {
        ActiveExcelPane.ActiveEditDialog.ShowDialog();
      }

      _restoringExistingSession = false;

      // If no errors were found at the opening sessions process do not display the warning message at the end.
      if (missingTables.Count <= 0)
      {
        return;
      }

      var errorMessage = new StringBuilder();
      if (missingTables.Count > 0)
      {
        errorMessage.AppendLine(Resources.RestoreSessionsMissingTablesMessage);
        foreach (var table in missingTables)
        {
          errorMessage.AppendLine(table);
        }
      }

      MiscUtilities.ShowCustomizedInfoDialog(InfoDialog.InfoType.Warning, Resources.RestoreSessionsWarningMessage, errorMessage.ToString());
    }

    /// <summary>
    /// Attempts to open a <see cref="MySqlWorkbenchConnection"/> of a saved session.
    /// </summary>
    /// <param name="sessionConection">A <see cref="MySqlWorkbenchConnection"/> of a stored session.</param>
    /// <returns><c>true</c> if the connection was successfully opened, <c>false</c> otherwise.</returns>
    private bool OpenConnectionForSavedSession(MySqlWorkbenchConnection sessionConection)
    {
      var connectionResult = ActiveExcelPane.OpenConnection(sessionConection, false);
      if (connectionResult.Cancelled)
      {
        return false;
      }

      if (connectionResult.ConnectionSuccess)
      {
        return true;
      }

      InfoDialog.ShowWarningDialog(Resources.RestoreSessionsOpenConnectionErrorTitle, Resources.RestoreSessionsOpenConnectionErrorDetail);
      return false;
    }

    /// <summary>
    /// Attempts to open a <see cref="MySqlWorkbenchConnection"/> of a saved session.
    /// </summary>
    /// <param name="session">A saved <see cref="EditSessionInfo"/> object.</param>
    /// <param name="workbook">A <see cref="Excel.Workbook"/> object related to the saved session.</param>
    /// <returns>The opened <see cref="MySqlWorkbenchConnection"/>.</returns>
    private MySqlWorkbenchConnection OpenConnectionForSavedSession(EditSessionInfo session, Excel.Workbook workbook)
    {
      if (session == null || workbook == null)
      {
        return null;
      }

      // Check if connection in stored session still exists in the collection of Workbench connections.
      var wbSessionConnection = MySqlWorkbench.Connections.GetConnectionForId(session.ConnectionId);
      DialogResult dialogResult;
      if (wbSessionConnection == null)
      {
        dialogResult = MiscUtilities.ShowCustomizedWarningDialog(Resources.RestoreSessionsOpenConnectionErrorTitle, Resources.RestoreSessionsWBConnectionNoLongerExistsFailedDetail);
        if (dialogResult == DialogResult.Yes)
        {
          DeleteCurrentWorkbookEditSessions(workbook);
        }

        return null;
      }

      if (ActiveExcelPane.WbConnection == null)
      {
        // If the connection in the active pane is null it means an active connection does not exist, so open a connection.
        if (!OpenConnectionForSavedSession(wbSessionConnection))
        {
          return null;
        }
      }
      else if (!string.Equals(wbSessionConnection.HostIdentifier, ActiveExcelPane.WbConnection.HostIdentifier, StringComparison.InvariantCulture))
      {
        // If the stored connection points to a different host as the current connection, ask the user if he wants to open a new connection only if there are active Edit dialogs.
        if (_editSessionsByWorkbook.Count > 1)
        {
          dialogResult = InfoDialog.ShowYesNoDialog(
            InfoDialog.InfoType.Warning,
            Resources.RestoreSessionsTitle,
            Resources.RestoreSessionsOpenConnectionCloseEditDialogsDetail,
            null,
            Resources.RestoreSessionsOpenConnectionCloseEditDialogsMoreInfo);
          if (dialogResult == DialogResult.No)
          {
            return null;
          }

          ActiveExcelPane.CloseSchema(false, false);
          ActiveExcelPane.CloseConnection(false);
        }

        if (!OpenConnectionForSavedSession(wbSessionConnection))
        {
          return null;
        }
      }

      return ActiveExcelPane.WbConnection;
    }

    ///  <summary>
    /// Restores saved Edit sessions from the given <see cref="Excel.Workbook"/>.
    /// </summary>
    /// <param name="workbook">An <see cref="Excel.Workbook"/> with saved Edit sessions.</param>
    private void RestoreEditSessions(Excel.Workbook workbook)
    {
      if (workbook == null || ActiveExcelPane == null || _editSessionsByWorkbook.ContainsKey(workbook.GetOrCreateId()))
      {
        return;
      }

      // Add the sessions for the workbook being opened to the dictionary of sessions.
      // The GetWorkbookEditSessions method will add the sessions related to the workbook it if they haven't been added.
      var workbookEditSessions = GetWorkbookEditSessions(workbook);
      if (!Settings.Default.EditSessionsRestoreWhenOpeningWorkbook || workbookEditSessions.Count == 0)
      {
        return;
      }

      // Open the connection from the session, check also if the current connection can be used to avoid opening a new one.
      var currenConnection = ActiveExcelPane.WbConnection;
      var firstSession = workbookEditSessions[0];
      var currentSchema = currenConnection != null ? currenConnection.Schema : string.Empty;
      var sessionConnection = OpenConnectionForSavedSession(firstSession, workbook);
      if (sessionConnection == null)
      {
        return;
      }

      // Close the current schema if the current connection is being reused but the session schema is different
      bool connectionReused = sessionConnection.Equals(currenConnection);
      bool openSchema = !connectionReused;
      if (connectionReused && !string.Equals(currentSchema, firstSession.SchemaName, StringComparison.InvariantCulture))
      {
        if (!ActiveExcelPane.CloseSchema(true, false))
        {
          return;
        }

        openSchema = true;
      }

      if (openSchema)
      {
        // Verify if the session schema to be opened still exists in the connected MySQL server
        if (!ActiveExcelPane.LoadedSchemas.Contains(firstSession.SchemaName))
        {
          var errorMessage = string.Format(Resources.RestoreSessionsSchemaNoLongerExistsFailed, sessionConnection.HostIdentifier, sessionConnection.Schema);
          MiscUtilities.ShowCustomizedInfoDialog(InfoDialog.InfoType.Error, errorMessage);
          return;
        }

        // Open the session schema
        ActiveExcelPane.OpenSchema(firstSession.SchemaName, true);
      }

      // Open the Edit sessions for each of the tables being edited
      OpenEditSessionsOfTables(workbook);
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
    /// Shows a <see cref="RestoreEditSessionsDialog"/> to the users to decide what to do with saved Edit sessions.
    /// </summary>
    /// <param name="workbook">The <see cref="Excel.Workbook"/> that may contain saved Edit sessions.</param>
    private void ShowOpenEditSessionsDialog(Excel.Workbook workbook)
    {
      if (workbook == null)
      {
        return;
      }

      var workbookId = workbook.GetOrCreateId();
      if (!StoredEditSessions.Exists(session => session.WorkbookGuid == workbookId) || _editSessionsByWorkbook.ContainsKey(workbookId))
      {
        return;
      }

      switch (RestoreEditSessionsDialog.ShowAndDispose())
      {
        case DialogResult.Abort:
          // Discard: Do not open any and delete all saved sessions for the current workbook.
          DeleteCurrentWorkbookEditSessions(workbook);
          break;

        case DialogResult.Yes:
          // Attempt to restore Edit sessions for the workbook beeing opened
          RestoreEditSessions(workbook);
          break;
      }
    }

    /// <summary>
    /// Event delegate method fired when the <see cref="ThisAddIn"/> is closed.
    /// </summary>
    /// <param name="sender">Sender object.</param>
    /// <param name="e">Event arguments.</param>
    private void ThisAddIn_Shutdown(object sender, EventArgs e)
    {
      // Close all Excel panes created
      if (ExcelPanesList == null)
      {
        return;
      }

      foreach (ExcelAddInPane excelPane in ExcelPanesList)
      {
        excelPane.Dispose();
      }

      ExcelAddInPanesClosed();
      MySqlSourceTrace.WriteToLog(Resources.ShutdownMessage, SourceLevels.Information);
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
        // Static initializations.
        System.Windows.Forms.Application.EnableVisualStyles();
        System.Windows.Forms.Application.SetCompatibleTextRenderingDefault(false);
        CustomizeInfoDialog();
        InitializeMySqlWorkbenchStaticSettings();
        AssemblyTitle = AssemblyInfo.AssemblyTitle;
        UsingTempWorksheet = false;

        // Make sure the settings directory exists
        Directory.CreateDirectory(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + @"\Oracle\MySQL for Excel");

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