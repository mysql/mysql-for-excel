// Copyright (c) 2012-2015, Oracle and/or its affiliates. All rights reserved.
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
using MySQL.Utility.Classes.MySQLInstaller;
using MySQL.Utility.Classes.MySQLWorkbench;
using MySQL.Utility.Forms;
using ExcelInterop = Microsoft.Office.Interop.Excel;
using ExcelTools = Microsoft.Office.Tools.Excel;
using OfficeTools = Microsoft.Office.Tools;
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
    /// The Excel major version number corresponding to Excel 2007.
    /// </summary>
    public const int EXCEL_2007_VERSION_NUMBER = 12;

    /// <summary>
    /// The Excel major version number corresponding to Excel 2010.
    /// </summary>
    public const int EXCEL_2010_VERSION_NUMBER = 14;

    /// <summary>
    /// The Excel major version number corresponding to Excel 2013.
    /// </summary>
    public const int EXCEL_2013_VERSION_NUMBER = 15;

    #endregion Constants

    #region Fields

    /// <summary>
    /// A dictionary containing subsets of the <see cref="EditConnectionInfo"/> list filtered by <see cref="ExcelInterop.Workbook"/>.
    /// </summary>
    private Dictionary<string, List<EditConnectionInfo>> _editConnectionInfosByWorkbook;

    /// <summary>
    /// The name of the last deactivated Excel <see cref="ExcelInterop.Worksheet"/>.
    /// </summary>
    private string _lastDeactivatedSheetName;

    /// <summary>
    /// True while restoring existing <see cref="EditConnectionInfo"/> objects for the current workbook, avoiding unwanted actions to be raised during the process.
    /// </summary>
    private bool _restoringExistingConnectionInfo;

    #endregion Fields

    #region Properties

    /// <summary>
    /// Gets the <see cref="Microsoft.Office.Tools.CustomTaskPane"/> contained in the active Excel window.
    /// </summary>
    public OfficeTools.CustomTaskPane ActiveCustomPane
    {
      get
      {
        OfficeTools.CustomTaskPane addInPane = CustomTaskPanes.FirstOrDefault(ctp =>
        {
          bool isParentWindowActiveExcelWindow;
          if (ExcelVersionNumber >= EXCEL_2013_VERSION_NUMBER)
          {
            // If running on Excel 2013 or later a MDI is used for the windows so the active custom pane is matched with its window and the application active window.
            ExcelInterop.Window paneWindow = null;
            try
            {
              // This assignment is intentionally inside a try block because when an Excel window has been previously closed this property (ActiveCustomPane)
              // is called before the CustomTaskPane linked to the closed Excel window is removed from the collection, so the ctp.Window can throw an Exception.
              // A null check is not enough.
              paneWindow = ctp.Window as ExcelInterop.Window;
            }
            catch
            {
            }

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
    /// Gets the active <see cref="ExcelInterop.Workbook"/> or creates one if there is no active one.
    /// </summary>
    public ExcelInterop.Workbook ActiveWorkbook
    {
      get
      {
        return Application.ActiveWorkbook ?? Application.Workbooks.Add(1);
      }
    }

    /// <summary>
    /// Gets a subset of the <see cref="EditConnectionInfos"/> objects listing only those associated to the active <see cref="ExcelInterop.Workbook"/>.
    /// </summary>
    public List<EditConnectionInfo> ActiveWorkbookEditConnectionInfos
    {
      get
      {
        return GetWorkbookEditConnectionInfos(ActiveWorkbook);
      }
    }

    /// <summary>
    /// Gets a subset of the <see cref="StoredImportConnectionInfos"/> objects listing only those assocaiated to the active <see cref="ExcelInterop.Workbook"/>.
    /// </summary>
    public List<ImportConnectionInfo> ActiveWorkbookImportConnectionInfos
    {
      get
      {
        var workbookId = ActiveWorkbook.GetOrCreateId();
        return GetWorkbookImportConnectionInfos(workbookId);
      }
    }

    /// <summary>
    /// Gets a subset of the <see cref="StoredImportConnectionInfos"/> listing only those assocaiated to the active <see cref="ExcelInterop.Worksheet"/>.
    /// </summary>
    public List<ImportConnectionInfo> ActiveWorksheetImportConnectionInfos
    {
      get
      {
        var workbookId = ActiveWorkbook.GetOrCreateId();
        ExcelInterop.Worksheet worksheet = ActiveWorkbook.ActiveSheet;
        return GetWorkSheetImportConnectionInfos(workbookId, worksheet.Name);
      }
    }

    /// <summary>
    /// Gets the title given to the assembly of the Add-In.
    /// </summary>
    public string AssemblyTitle { get; private set; }

    /// <summary>
    /// Gets the custom ribbon defined by this add-in.
    /// </summary>
    public MySqlRibbon CustomMySqlRibbon { get; private set; }

    /// <summary>
    /// Gets a list of <see cref="EditConnectionInfo"/> objects saved to disk.
    /// </summary>
    public List<EditConnectionInfo> EditConnectionInfos
    {
      get
      {
        return Settings.Default.EditConnectionInfosList ?? (Settings.Default.EditConnectionInfosList = new List<EditConnectionInfo>());
      }
    }

    /// <summary>
    /// Gets a list with all the Excel panes instantiated in the Excel window, stored it to dispose of them when needed.
    /// </summary>
    public List<ExcelAddInPane> ExcelPanesList { get; private set; }

    /// <summary>
    /// Gets the version for <see cref="ExcelInterop.PivotTable"/> objects creation.
    /// </summary>
    public ExcelInterop.XlPivotTableVersionList ExcelPivotTableVersion
    {
      get
      {
        switch (ExcelVersionNumber)
        {
          case EXCEL_2013_VERSION_NUMBER:
            return ExcelInterop.XlPivotTableVersionList.xlPivotTableVersion15;

          case EXCEL_2010_VERSION_NUMBER:
            return ExcelInterop.XlPivotTableVersionList.xlPivotTableVersion14;

          default:
            return ExcelInterop.XlPivotTableVersionList.xlPivotTableVersion12;
        }
      }
    }

    /// <summary>
    /// Gets the MS Excel major version number.
    /// </summary>
    public int ExcelVersionNumber { get; private set; }

    /// <summary>
    /// Gets or sets a value indicating whether the detection of contents for a cell selection should be skipped.
    /// </summary>
    /// <remarks>Used when a cell selection is being done programatically and not by the user.</remarks>
    public bool SkipSelectedDataContentsDetection { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether the <see cref="ExcelInterop.Worksheet.Change"/> event should be skipped.
    /// </summary>
    /// <remarks>Used when a cell's value is being set programatically and not by the user.</remarks>
    public bool SkipWorksheetChangeEvent { get; set; }

    /// <summary>
    /// Gets a list of <see cref="ImportConnectionInfo"/> objects saved to disk.
    /// </summary>
    public List<ImportConnectionInfo> StoredImportConnectionInfos
    {
      get
      {
        return Settings.Default.ImportConnectionInfosList ?? (Settings.Default.ImportConnectionInfosList = new List<ImportConnectionInfo>());
      }
    }

    /// <summary>
    /// Gets or sets a value indicating whether a temporary hidden <see cref="ExcelInterop.Worksheet"/> is being used by a <see cref="TempRange"/> instance.
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

      try
      {
        ExcelPanesList.Remove(excelPane);
        if (ExcelPanesList.Count == 0)
        {
          ExcelAddInPanesClosed();
        }

        excelPane.Dispose();
        OfficeTools.CustomTaskPane customPane = CustomTaskPanes.FirstOrDefault(ctp => ctp.Control is ExcelAddInPane && ctp.Control == excelPane);
        if (customPane != null)
        {
          CustomTaskPanes.Remove(customPane);
          customPane.Dispose();
        }
      }
      catch (Exception ex)
      {
        MySqlSourceTrace.WriteAppErrorToLog(ex);
      }
    }

    /// <summary>
    /// Closes and removes all <see cref="EditConnectionInfo" /> associated to the given <see cref="ExcelInterop.Workbook"/>.
    /// </summary>
    /// <param name="workbook">The <see cref="ExcelInterop.Workbook"/> associated to the <see cref="EditConnectionInfo" /> objects to close.</param>
    public void CloseWorkbookEditConnectionInfos(ExcelInterop.Workbook workbook)
    {
      if (workbook == null)
      {
        return;
      }

      var workbookConnectionInfos = GetWorkbookEditConnectionInfos(workbook);
      var connectionInfosToFreeResources = workbookConnectionInfos.FindAll(connectionInfo => connectionInfo.EditDialog != null && connectionInfo.EditDialog.WorkbookName == workbook.Name);
      foreach (var connectionInfo in connectionInfosToFreeResources)
      {
        // The Close method is both closing the dialog and removing itself from the collection of <see cref="EditConnectionInfo" /> objects.
        connectionInfo.EditDialog.Close();
      }
    }

    /// <summary>
    /// Gets the custom task pane in the active window, if not found creates it.
    /// </summary>
    /// <returns>the active or newly created <see cref="Microsoft.Office.Tools.CustomTaskPane"/> object.</returns>
    public OfficeTools.CustomTaskPane GetOrCreateActiveCustomPane()
    {
      OfficeTools.CustomTaskPane activeCustomPane = ActiveCustomPane;

      // If there is no custom pane associated to the Excel Add-In in the active window, create one.
      if (activeCustomPane != null)
      {
        return activeCustomPane;
      }

      Application.Cursor = ExcelInterop.XlMousePointer.xlWait;
      if (ExcelPanesList == null)
      {
        ExcelPanesList = new List<ExcelAddInPane>();
      }

      // Determine if this is the first run of the Add-In by checking if there are no Excel panes in the collection.
      // This must be done at this point of the code, before the lines below that create an Excel pane.
      bool firstRun = ExcelPanesList.Count == 0;
      if (firstRun)
      {
        // Migrate all Workbench connections created with version 1.0.6 (in a local connections file) to the Workbench connections file.
        MySqlWorkbench.MigrateExternalConnectionsToWorkbench();
      }

      // Instantiate the Excel Add-In pane to attach it to the Excel's custom task pane.
      // Note that in Excel 2007 and 2010 a MDI model is used so only a single Excel pane is instantiated, whereas in Excel 2013 and greater
      //  a SDI model is used instead, so an Excel pane is instantiated for each custom task pane appearing in each Excel window.
      var excelPane = new ExcelAddInPane { Dock = DockStyle.Fill };
      excelPane.SizeChanged += ExcelPane_SizeChanged;
      ExcelPanesList.Add(excelPane);

      // Create a new custom task pane and initialize it.
      activeCustomPane = CustomTaskPanes.Add(excelPane, AssemblyTitle);
      activeCustomPane.VisibleChanged += CustomTaskPaneVisibleChanged;
      activeCustomPane.DockPosition = OfficeCore.MsoCTPDockPosition.msoCTPDockPositionRight;
      activeCustomPane.DockPositionRestrict = OfficeCore.MsoCTPDockPositionRestrict.msoCTPDockPositionRestrictNoHorizontal;
      activeCustomPane.Width = ADD_IN_PANE_WIDTH;

      // First run if no Excel panes have been opened yet.
      if (firstRun)
      {
        ExcelAddInPaneFirstRun();
      }

      Application.Cursor = ExcelInterop.XlMousePointer.xlDefault;
      return activeCustomPane;
    }

    /// <summary>
    /// Refreshes the data in all <see cref="ExcelInterop.ListObject"/> and <see cref="ExcelInterop.PivotTable"/> objects in every <see cref="ExcelInterop.Worksheet"/> of the active <see cref="ExcelInterop.Workbook"/>.
    /// </summary>
    public void RefreshAllCustomFunctionality()
    {
      foreach (ExcelInterop.Worksheet worksheet in ActiveWorkbook.Worksheets)
      {
        if (!worksheet.RefreshAllListObjects())
        {
          break;
        }

        if (!worksheet.RefreshAllPivotTables())
        {
          break;
        }
      }
    }

    /// <summary>
    /// Attempts to refresh the MySQL data tied to the <see cref="ExcelInterop.ListObject"/> of the active Excell cell.
    /// </summary>
    /// <returns><c>true</c> if the active <see cref="ExcelInterop.ListObject"/> has a related <see cref="ImportConnectionInfo"/> and was refreshed, <c>false</c> otherwise.</returns>
    public bool RefreshDataCustomFunctionality()
    {
      var listObject = Application.ActiveCell.ListObject;
      return listObject.RefreshMySqlData();
    }

    /// <summary>
    /// Creates and returns a new instance of the <see cref="MySqlRibbon"/> class.
    /// </summary>
    /// <returns>A new instance of the <see cref="MySqlRibbon"/> class.</returns>
    protected override Microsoft.Office.Core.IRibbonExtensibility CreateRibbonExtensibilityObject()
    {
      CustomMySqlRibbon = new MySqlRibbon();
      return CustomMySqlRibbon;
    }

    /// <summary>
    /// Event delegate method fired when an Excel <see cref="ExcelInterop.Worksheet"/> is activated.
    /// </summary>
    /// <param name="workSheet">A <see cref="ExcelInterop.Worksheet"/> object.</param>
    private void Application_SheetActivate(object workSheet)
    {
      if (ActiveExcelPane == null || UsingTempWorksheet)
      {
        return;
      }

      ExcelInterop.Worksheet activeSheet = workSheet as ExcelInterop.Worksheet;
      if (!activeSheet.IsVisible())
      {
        return;
      }

      if (_lastDeactivatedSheetName.Length > 0 && !ActiveWorkbook.WorksheetExists(_lastDeactivatedSheetName))
      {
        // Worksheet was deleted and the Application_SheetBeforeDelete did not run, user is running Excel 2010 or earlier.
        CloseMissingWorksheetEditConnectionInfo(ActiveWorkbook, _lastDeactivatedSheetName);
      }

      ChangeEditDialogVisibility(activeSheet, true);
    }

    /// <summary>
    /// Event delegate method fired before an Excel <see cref="ExcelInterop.Worksheet"/> is deleted.
    /// </summary>
    /// <param name="workSheet">A <see cref="ExcelInterop.Worksheet"/> object.</param>
    private void Application_SheetBeforeDelete(object workSheet)
    {
      if (ActiveExcelPane == null || UsingTempWorksheet)
      {
        return;
      }

      ExcelInterop.Worksheet activeSheet = workSheet as ExcelInterop.Worksheet;
      if (!activeSheet.IsVisible())
      {
        return;
      }

      CloseWorksheetEditConnectionInfo(activeSheet);

      // If the _lastDeactivatedSheetName is not empty it means a deactivated sheet may have been deleted, if this method ran it means the user is running
      // Excel 2013 or later where this method is supported, so we need to clean the _lastDeactivatedSheetName.
      if (_lastDeactivatedSheetName.Length > 0)
      {
        _lastDeactivatedSheetName = string.Empty;
      }
    }

    /// <summary>
    /// Event delegate method fired when the contents of the current selection of Excel cells in a given <see cref="ExcelInterop.Worksheet"/> change.
    /// </summary>
    /// <param name="workSheet">A <see cref="ExcelInterop.Worksheet"/> object.</param>
    /// <param name="targetRange">A selection of Excel cells.</param>
    private void Application_SheetChange(object workSheet, ExcelInterop.Range targetRange)
    {
      if (ActiveExcelPane == null || SkipWorksheetChangeEvent || UsingTempWorksheet)
      {
        return;
      }

      ExcelInterop.Worksheet activeSheet = workSheet as ExcelInterop.Worksheet;
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
    /// Event delegate method fired when an Excel <see cref="ExcelInterop.Worksheet"/> is deactivated.
    /// </summary>
    /// <param name="workSheet">A <see cref="ExcelInterop.Worksheet"/> object.</param>
    private void Application_SheetDeactivate(object workSheet)
    {
      if (ActiveExcelPane == null || UsingTempWorksheet)
      {
        return;
      }

      ExcelInterop.Worksheet deactivatedSheet = workSheet as ExcelInterop.Worksheet;
      if (!deactivatedSheet.IsVisible())
      {
        return;
      }

      _lastDeactivatedSheetName = deactivatedSheet != null ? deactivatedSheet.Name : string.Empty;
      ChangeEditDialogVisibility(deactivatedSheet, false);
    }

    /// <summary>
    /// Event delegate method fired when the selection of Excel cells in a given <see cref="ExcelInterop.Worksheet"/> changes.
    /// </summary>
    /// <param name="workSheet">A <see cref="ExcelInterop.Worksheet"/> object.</param>
    /// <param name="targetRange">The new selection of Excel cells.</param>
    private void Application_SheetSelectionChange(object workSheet, ExcelInterop.Range targetRange)
    {
      if (ActiveExcelPane == null || UsingTempWorksheet)
      {
        return;
      }

      var activeSheet = workSheet as ExcelInterop.Worksheet;
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
    private void Application_WindowActivate(ExcelInterop.Workbook workbook, ExcelInterop.Window window)
    {
      // Verify the collection of custom task panes to dispose of custom task panes pointing to closed (invalid) windows.
      bool disposePane = false;
      foreach (OfficeTools.CustomTaskPane customPane in CustomTaskPanes.Where(customPane => customPane.Control is ExcelAddInPane))
      {
        try
        {
          // Do NOT remove the following line although the customPaneWindow variable is not used in the method the casting
          // of the customPane.Window is needed to determine if the window is still valid and has not been disposed of.
          var customPaneWindow = customPane.Window as ExcelInterop.Window;
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
      CustomMySqlRibbon.ChangeShowMySqlForExcelPaneToggleState(ActiveCustomPane != null && ActiveCustomPane.Visible);
    }

    /// <summary>
    /// Event delegate method fired when a <see cref="ExcelInterop.Workbook"/> is activated.
    /// </summary>
    /// <param name="workBook">A <see cref="ExcelInterop.Workbook"/> object.</param>
    private void Application_WorkbookActivate(object workBook)
    {
      if (ActiveExcelPane == null)
      {
        return;
      }

      ExcelInterop.Workbook activeWorkbook = workBook as ExcelInterop.Workbook;
      ExcelInterop.Worksheet activeSheet = activeWorkbook != null ? activeWorkbook.ActiveSheet as ExcelInterop.Worksheet : null;
      ChangeEditDialogVisibility(activeSheet, true);
      ActiveExcelPane.RefreshDbObjectPanelActionLabelsEnabledStatus();
    }

    /// <summary>
    /// Event delegate method fired after an Excel <see cref="ExcelInterop.Workbook"/> is saved to disk.
    /// </summary>
    /// <param name="workbook">A <see cref="ExcelInterop.Workbook"/> object.</param>
    /// <param name="success">Flag indicating whether the save operation was successful.</param>
    private void Application_WorkbookAfterSave(ExcelInterop.Workbook workbook, bool success)
    {
      var workbookId = workbook.GetOrCreateId();
      var workbookEditConnectionInfos = GetWorkbookEditConnectionInfos(workbook);

      // Protect all worksheets with an active editing table.
      foreach (var activeEditConnectionInfo in workbookEditConnectionInfos)
      {
        if (activeEditConnectionInfo.EditDialog != null)
        {
          activeEditConnectionInfo.EditDialog.ProtectWorksheet();
        }

        if (!success)
        {
          continue;
        }

        // Add new EditConnectionInfo in memory collection to serialized collection
        activeEditConnectionInfo.LastAccess = DateTime.Now;
        activeEditConnectionInfo.WorkbookFilePath = workbook.FullName;
        if (!EditConnectionInfos.Contains(activeEditConnectionInfo))
        {
          EditConnectionInfos.Add(activeEditConnectionInfo);
        }
      }

      // Add back the localized date format strings to this workbook.
      workbook.AddLocalizedDateFormatStringsAsNames();

      if (!success)
      {
        workbook.Saved = false;
        return;
      }

      // Scrubbing of invalid ImportConnectionInfo and setting last access time.
      RemoveInvalidImportConnectionInformation();
      foreach (var activeImportConnectionInfo in ActiveWorkbookImportConnectionInfos)
      {
        activeImportConnectionInfo.LastAccess = DateTime.Now;
        activeImportConnectionInfo.WorkbookName = workbook.Name;
        activeImportConnectionInfo.WorkbookFilePath = workbook.FullName;
      }

      // Remove deleted EditConnectionInfo objects from memory collection also from serialized collection
      foreach (var storedConnectionInfo in EditConnectionInfos.FindAll(storedConnectionInfo => string.Equals(storedConnectionInfo.WorkbookGuid, workbookId, StringComparison.InvariantCulture) && !workbookEditConnectionInfos.Exists(wbConnectionInfo => wbConnectionInfo.HasSameWorkbookAndTable(storedConnectionInfo))))
      {
        EditConnectionInfos.Remove(storedConnectionInfo);
      }

      MiscUtilities.SaveSettings();
      workbook.Saved = true;
    }

    /// <summary>
    /// Method that overrides the default program flow on Excel 2007 since it doesn't exist an WorkbookAfterSave event in this version compared to 2010 and superior versions of Excel.
    /// More info about this topic can be found at http://msdn.microsoft.com/en-us/library/office/ff836466(v=office.15).aspx" />
    /// </summary>
    /// <param name="workbook">A <see cref="ExcelInterop.Workbook"/> object.</param>
    /// <param name="saveAsUi">Flag indicating whether the Save As dialog was displayed.</param>
    private void Application_WorkbookAfterSave2007(ExcelInterop.Workbook workbook, bool saveAsUi)
    {
      Application.EnableEvents = false; //Stops beforesave event from re-running
      bool triggerAfterSave = true;

      try
      {
        if (saveAsUi)
        {
          var saveAsDialog = Application.Dialogs[ExcelInterop.XlBuiltInDialog.xlDialogSaveAs];
          triggerAfterSave = saveAsDialog.Show(workbook.Name, Application.DefaultSaveFormat, null, true, null, false);
        }
        else
        {
          workbook.Save();
        }
      }
      catch (Exception ex)
      {
        MySqlSourceTrace.WriteAppErrorToLog(ex);
        triggerAfterSave = false;
      }

      Application.EnableEvents = true;
      if (triggerAfterSave)
      {
        Application_WorkbookAfterSave(workbook, saveAsUi);
      }
    }

    /// <summary>
    /// Event delegate method fired when a new <see cref="ExcelInterop.Workbook"/> is created.
    /// </summary>
    /// <param name="workbook">The <see cref="ExcelInterop.Workbook"/> being created.</param>
    private void Application_WorkbookNewWorkbook(Microsoft.Office.Interop.Excel.Workbook workbook)
    {
      InitializeWorkbook(workbook);
    }

    /// <summary>
    /// Removes invalid import connection information from the collection.
    /// </summary>
    private void RemoveInvalidImportConnectionInformation()
    {
      var invalidConnectionInfos = new List<ImportConnectionInfo>();
      foreach (var importConnectionInfo in ActiveWorkbookImportConnectionInfos)
      {
        try
        {
          // DO NOT REMOVE this line. If the excel table is invalid, accessing it will throw an exception.
          var excelTableComment = importConnectionInfo.ExcelTable.Comment;
        }
        catch
        {
          // The importConnectionInfo's list object was moved to another worksheet or when its columns had been deleted or the reference to it no longer exists.
          invalidConnectionInfos.Add(importConnectionInfo);
        }
      }

      // Dispose of ImportConnectionInfo objects that are no longer valid for the current workbook.
      if (invalidConnectionInfos.Count > 0)
      {
        invalidConnectionInfos.ForEach(invalidSession => invalidSession.ExcelTable.DeleteSafely(false));
        invalidConnectionInfos.ForEach(invalidSession => StoredImportConnectionInfos.Remove(invalidSession));
      }
    }

    /// <summary>
    /// Event delegate method fired before a <see cref="ExcelInterop.Workbook"/> is closed.
    /// </summary>
    /// <param name="workbook">A <see cref="ExcelInterop.Workbook"/> object.</param>
    /// <param name="cancel">Flag indicating whether the user cancelled the closing event.</param>
    private void Application_WorkbookBeforeClose(ExcelInterop.Workbook workbook, ref bool cancel)
    {
      if (ActiveExcelPane == null)
      {
        return;
      }

      bool wasAlreadySaved = workbook.Saved;
      if (!wasAlreadySaved)
      {
        // Cleanup and close ImportConnectionInfo objects from the closing workbook.
        RemoveInvalidImportConnectionInformation();
        foreach (var importConnectionInfo in ActiveWorkbookImportConnectionInfos)
        {
          importConnectionInfo.Dispose();
        }

        switch (MessageBox.Show(string.Format(Resources.WorkbookSavingDetailText, workbook.Name), Application.Name, MessageBoxButtons.YesNoCancel, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button1))
        {
          case DialogResult.Yes:
            for (int retry = 1; retry <= 3 && !wasAlreadySaved; retry++)
            {
              try
              {
                if (workbook.IsNew())
                {
                  // The workbook is being saved for the very first time, so show the Save As dialog to users which will save the Workbook where the user wants to.
                  Application.Dialogs[ExcelInterop.XlBuiltInDialog.xlDialogSaveAs].Show(workbook.Name);
                }
                else
                {
                  // The workbook has been saved before, so just overwrite it.
                  workbook.Save();
                }

                wasAlreadySaved = true;
              }
              catch (Exception ex)
              {
                var errorTitle = retry <= 3 ? Resources.WorkbookSaveErrorText : Resources.WorkbookSaveErrorFinalText;
                MiscUtilities.ShowCustomizedErrorDialog(errorTitle, ex.Message, true);
                MySqlSourceTrace.WriteToLog(errorTitle);
                MySqlSourceTrace.WriteAppErrorToLog(ex);
              }
            }

            cancel = !wasAlreadySaved;
            break;

          case DialogResult.No:
            wasAlreadySaved = true;
            break;

          case DialogResult.Cancel:
            cancel = true;
            return;
        }
      }

      CloseWorkbookEditConnectionInfos(workbook);
      if (wasAlreadySaved)
      {
        workbook.Saved = true;
      }

      // Remove the EditConnectionInfo objects for the workbook being closed from the dictionary.
      _editConnectionInfosByWorkbook.Remove(workbook.GetOrCreateId());
    }

    /// <summary>
    /// Event delegate method fired before an Excel <see cref="ExcelInterop.Workbook"/> is saved to disk.
    /// </summary>
    /// <param name="workbook">A <see cref="ExcelInterop.Workbook"/> object.</param>
    /// <param name="saveAsUi">Flag indicating whether the Save As dialog was displayed.</param>
    /// <param name="cancel">Flag indicating whether the user cancelled the saving event.</param>
    private void Application_WorkbookBeforeSave(ExcelInterop.Workbook workbook, bool saveAsUi, ref bool cancel)
    {
      var workbookConnectionInfos = GetWorkbookEditConnectionInfos(workbook);

      // Unprotect all worksheets with an active editing table.
      foreach (var activeEditConnectionInfo in workbookConnectionInfos.Where(activeEditConnectionInfo => activeEditConnectionInfo.EditDialog != null))
      {
        activeEditConnectionInfo.EditDialog.UnprotectWorksheet();
      }

      //The WorkbookAfterSave event in Excel 2007 does not exist so we need to sligthly alter the program flow to overcome this limitation.
      if (ExcelVersionNumber <= EXCEL_2007_VERSION_NUMBER)
      {
        cancel = true; //Cancels the users original save command request in order to execute the following code override.
        Application_WorkbookAfterSave2007(workbook, saveAsUi);
      }

      // Remove the localized date format strings from this workbook, since they can't be saved in a macro-free (normal) workbook.
      workbook.RemoveLocalizedDateFormatNames();
    }

    /// <summary>
    /// Event delegate method fired when an Excel <see cref="ExcelInterop.Workbook"/> is deactivated.
    /// </summary>
    /// <param name="workbook">A <see cref="ExcelInterop.Workbook"/> object.</param>
    private void Application_WorkbookDeactivate(object workbook)
    {
      if (ActiveExcelPane == null)
      {
        return;
      }

      // Hide editDialogs from deactivated Workbook
      ExcelInterop.Workbook deactivatedWorkbook = workbook as ExcelInterop.Workbook;
      if (deactivatedWorkbook == null)
      {
        return;
      }

      foreach (ExcelInterop.Worksheet wSheet in deactivatedWorkbook.Worksheets)
      {
        ChangeEditDialogVisibility(wSheet, false);
      }
    }

    /// <summary>
    /// Event delegate method fired when a <see cref="ExcelInterop.Workbook"/> is opened.
    /// </summary>
    /// <param name="workbook">The <see cref="ExcelInterop.Workbook"/> being opened.</param>
    private void Application_WorkbookOpen(ExcelInterop.Workbook workbook)
    {
      InitializeWorkbook(workbook);
    }

    /// <summary>
    /// Shows or hides an Edit dialog associated to the given <see cref="ExcelInterop.Worksheet"/>.
    /// </summary>
    /// <param name="workSheet">A <see cref="ExcelInterop.Worksheet"/> object.</param>
    /// <param name="show">Flag indicating if the dialog will be shown or hidden.</param>
    private void ChangeEditDialogVisibility(ExcelInterop.Worksheet workSheet, bool show)
    {
      if (workSheet == null)
      {
        return;
      }

      var parentWorkbook = workSheet.Parent as ExcelInterop.Workbook;
      if (parentWorkbook == null)
      {
        return;
      }

      var workbookConnectionInfos = GetWorkbookEditConnectionInfos(parentWorkbook);
      if (workbookConnectionInfos.Count == 0 || _restoringExistingConnectionInfo)
      {
        return;
      }

      var activeEditConnectionInfo = workbookConnectionInfos.GetActiveEditConnectionInfo(workSheet);
      if (activeEditConnectionInfo == null)
      {
        return;
      }

      if (show)
      {
        activeEditConnectionInfo.EditDialog.ShowDialog();
      }
      else
      {
        activeEditConnectionInfo.EditDialog.Hide();
      }
    }

    /// <summary>
    /// Closes and removes the <see cref="EditConnectionInfo"/> associated to the given <see cref="ExcelInterop.Worksheet"/>.
    /// </summary>
    /// <param name="workbook">An <see cref="ExcelInterop.Workbook"/>.</param>
    /// <param name="missingWorksheetName">The name of the <see cref="ExcelInterop.Worksheet"/> that no longer exists and that is associated to the <see cref="EditConnectionInfo"/> to close.</param>
    private void CloseMissingWorksheetEditConnectionInfo(ExcelInterop.Workbook workbook, string missingWorksheetName)
    {
      if (workbook == null || missingWorksheetName.Length == 0)
      {
        return;
      }

      var workbookConnectionInfos = GetWorkbookEditConnectionInfos(workbook);
      var wsConnectionInfo = workbookConnectionInfos.FirstOrDefault(connectionInfo => !connectionInfo.EditDialog.EditingWorksheetExists);
      if (wsConnectionInfo == null)
      {
        return;
      }

      wsConnectionInfo.EditDialog.Close();
    }

    /// <summary>
    /// Closes and removes the <see cref="EditConnectionInfo"/> associated to the given <see cref="ExcelInterop.Worksheet"/>.
    /// </summary>
    /// <param name="worksheet">The <see cref="ExcelInterop.Worksheet"/> associated to the <see cref="EditConnectionInfo"/> to close.</param>
    private void CloseWorksheetEditConnectionInfo(ExcelInterop.Worksheet worksheet)
    {
      if (worksheet == null)
      {
        return;
      }

      ExcelInterop.Workbook parentWorkbook = worksheet.Parent as ExcelInterop.Workbook;
      if (parentWorkbook == null)
      {
        return;
      }

      var workbookConnectionInfos = GetWorkbookEditConnectionInfos(parentWorkbook);
      var wsConnectionInfo = workbookConnectionInfos.FirstOrDefault(connectionInfo => string.Equals(connectionInfo.EditDialog.WorkbookName, parentWorkbook.Name, StringComparison.InvariantCulture) && string.Equals(connectionInfo.EditDialog.WorksheetName, worksheet.Name, StringComparison.InvariantCulture));
      if (wsConnectionInfo == null)
      {
        return;
      }

      wsConnectionInfo.EditDialog.Close();
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
    /// Customizes the looks of the <see cref="MySQL.Utility.Forms.InfoDialog"/> form for MySQL for ExcelInterop.
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
    /// Event delegate method fired when the <see cref="OfficeTools.CustomTaskPane"/> visible property value changes.
    /// </summary>
    /// <param name="sender">Sender object.</param>
    /// <param name="e">Sender object.</param>
    private void CustomTaskPaneVisibleChanged(object sender, EventArgs e)
    {
      OfficeTools.CustomTaskPane customTaskPane = sender as OfficeTools.CustomTaskPane;
      CustomMySqlRibbon.ChangeShowMySqlForExcelPaneToggleState(customTaskPane != null && customTaskPane.Visible);
    }

    /// <summary>
    /// Deletes automatically saved connection information entries with non-existent Excel Workbooks.
    /// </summary>
    /// <param name="logOperation">Flag indicating whether this operation is written in the application log.</param>
    private void DeleteConnectionInfosWithNonExistentWorkbook(bool logOperation)
    {
      if (!Settings.Default.DeleteAutomaticallyOrphanedConnectionInfos)
      {
        return;
      }

      var orphanedConnectionInfos = ManageConnectionInfosDialog.GetConnectionInfosWithNonExistentWorkbook();
      if (orphanedConnectionInfos == null)
      {
        return;
      }

      if (logOperation)
      {
        MySqlSourceTrace.WriteToLog(Resources.DeletingConnectionInfosWithNonExistentWorkbook, SourceLevels.Information);
      }

      foreach (var connectionInfo in orphanedConnectionInfos)
      {
        if (connectionInfo.GetType() == typeof(EditConnectionInfo))
        {
          Globals.ThisAddIn.EditConnectionInfos.Remove(connectionInfo as EditConnectionInfo);
        }
        else
        {
          Globals.ThisAddIn.StoredImportConnectionInfos.Remove(connectionInfo as ImportConnectionInfo);
        }
      }

      MiscUtilities.SaveSettings();
    }

    /// <summary>
    /// Delete the closed workbook's <see cref="EditConnectionInfo"/> objects from the settings file.
    /// </summary>
    private void DeleteCurrentWorkbookEditConnectionInfos(ExcelInterop.Workbook workbook)
    {
      if (workbook == null || string.IsNullOrEmpty(workbook.GetOrCreateId()))
      {
        return;
      }

      // Remove all EditConnectionInfo objects from the current workbook.
      var workbookConnectionInfos = GetWorkbookEditConnectionInfos(workbook);
      foreach (var connectionInfo in workbookConnectionInfos)
      {
        EditConnectionInfos.Remove(connectionInfo);
      }

      Settings.Default.Save();
      if (workbookConnectionInfos.Count > 0)
      {
        _editConnectionInfosByWorkbook.Remove(workbook.GetOrCreateId());
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
      OfficeTools.CustomTaskPane customTaskPane = CustomTaskPanes.FirstOrDefault(ctp => ctp.Control == excelPane);
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
      _editConnectionInfosByWorkbook = new Dictionary<string, List<EditConnectionInfo>>(EditConnectionInfos.Count);
      _lastDeactivatedSheetName = string.Empty;
      _restoringExistingConnectionInfo = false;
      SkipSelectedDataContentsDetection = false;
      SkipWorksheetChangeEvent = false;

      // Subscribe to Excel events
      SetupExcelEvents(true);

      // Create custom MySQL Excel table style and localized date format strings in the active workbook if it exists.
      InitializeWorkbook(ActiveWorkbook);

      // Automatically delete ConnectionInfos that have a non-existent Excel Workbook.
      DeleteConnectionInfosWithNonExistentWorkbook(true);

      // Restore EditConnectionInfos
      ShowOpenEditConnectionInfosDialog(ActiveWorkbook);
    }

    /// <summary>
    /// Performs clean-up code that must be done after all Excel panes have been closed by the user.
    /// </summary>
    private void ExcelAddInPanesClosed()
    {
      // Unsubscribe from Excel events
      SetupExcelEvents(false);
      if (_editConnectionInfosByWorkbook != null)
      {
        _editConnectionInfosByWorkbook.Clear();

      }
    }

    /// <summary>
    /// Initializes settings for the <see cref="MySqlWorkbench"/> and <see cref="MySqlWorkbenchPasswordVault"/> classes.
    /// </summary>
    private void InitializeMySqlWorkbenchStaticSettings()
    {
      string applicationDataFolderPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
      MySqlSourceTrace.LogFilePath = applicationDataFolderPath + @"\Oracle\MySQL for Excel\MySQLForExcelInterop.log";
      MySqlSourceTrace.SourceTraceClass = "MySQLForExcel";
      MySqlWorkbench.ExternalApplicationName = AssemblyTitle;
      MySqlWorkbenchPasswordVault.ApplicationPasswordVaultFilePath = applicationDataFolderPath + @"\Oracle\MySQL for Excel\user_data.dat";
      MySqlWorkbench.ExternalConnections.CreateDefaultConnections = !MySqlWorkbench.ConnectionsFileExists && MySqlWorkbench.Connections.Count == 0;
      MySqlWorkbench.ExternalApplicationConnectionsFilePath = applicationDataFolderPath + @"\Oracle\MySQL for Excel\connections.xml";
    }

    /// <summary>
    /// Method used to initialize a <see cref="ExcelInterop.Workbook" /> when it is opened or created.
    /// </summary>
    /// <param name="workbook">The <see cref="ExcelInterop.Workbook" /> being opened.</param>
    private void InitializeWorkbook(ExcelInterop.Workbook workbook)
    {
      if (workbook == null)
      {
        return;
      }

      // Add the custom MySQL table style (for Excel tables) and localized date format strings to this workbook.
      workbook.CreateMySqlTableStyle();
      workbook.AddLocalizedDateFormatStringsAsNames();

      // When it is a new workbook it won't have any IConnectionInfo object related to it, so we could skip the rest of the method altogether.
      if (workbook.IsNew())
      {
        return;
      }

      RestoreImportConnectionInfos(workbook);
      if (ActiveExcelPane == null)
      {
        return;
      }

      ShowOpenEditConnectionInfosDialog(workbook);
    }

    /// <summary>
    /// Gets a subset of the <see cref="EditConnectionInfos"/> listing only those assocaiated to the given <see cref="ExcelInterop.Workbook"/>.
    /// </summary>
    /// <param name="workbook">A <see cref="ExcelInterop.Workbook"/> with active <see cref="EditConnectionInfo"/> objects.</param>
    /// <returns>A subset of the <see cref="EditConnectionInfos"/> listing only those assocaiated to the given <see cref="ExcelInterop.Workbook"/></returns>
    private List<EditConnectionInfo> GetWorkbookEditConnectionInfos(ExcelInterop.Workbook workbook)
    {
      List<EditConnectionInfo> workbookConnectionInfos = null;
      string workbookId = workbook.GetOrCreateId();
      if (_editConnectionInfosByWorkbook != null && !string.IsNullOrEmpty(workbookId))
      {
        if (_editConnectionInfosByWorkbook.ContainsKey(workbookId))
        {
          workbookConnectionInfos = _editConnectionInfosByWorkbook[workbookId];
        }
        else
        {
          workbookConnectionInfos = EditConnectionInfos.FindAll(connectionInfo => string.Equals(connectionInfo.WorkbookGuid, workbookId, StringComparison.InvariantCulture));
          _editConnectionInfosByWorkbook.Add(workbookId, workbookConnectionInfos);
        }
      }

      return workbookConnectionInfos ?? new List<EditConnectionInfo>();
    }

    /// <summary>
    /// Gets a subset of the <see cref="StoredImportConnectionInfos" /> listing only those assocaiated to the given <see cref="ExcelInterop.Workbook" />.
    /// </summary>
    /// <param name="workbookId">Workbook Id to match the sub set of ImportConnectionInfos to.</param>
    /// <returns> A subset of the <see cref="StoredImportConnectionInfos" /> listing only those assocaiated to the given <see cref="ExcelInterop.Workbook" /></returns>
    private List<ImportConnectionInfo> GetWorkbookImportConnectionInfos(string workbookId)
    {
      return StoredImportConnectionInfos.FindAll(connectionInfo => string.Equals(connectionInfo.WorkbookGuid, workbookId, StringComparison.InvariantCulture));
    }

    /// <summary>
    /// Gets a subset of the <see cref="StoredImportConnectionInfos" /> listing only those assocaiated to the given <see cref="ExcelInterop.Worksheet" />.
    /// </summary>
    /// <param name="workbookId">Workbook Id to match the sub set of <see cref="ImportConnectionInfo" /> to.</param>
    /// <param name="worksheetName">Worksheet Name to match the sub set of <see cref="ImportConnectionInfo" /> to.</param>
    /// <returns>A subset of the <see cref="StoredImportConnectionInfos" /> listing only those assocaiated to the given <see cref="ExcelInterop.Worksheet" /></returns>
    private List<ImportConnectionInfo> GetWorkSheetImportConnectionInfos(string workbookId, string worksheetName)
    {
      List<ImportConnectionInfo> worksheetConnectionInfos = GetWorkbookImportConnectionInfos(workbookId);
      return worksheetConnectionInfos.FindAll(connectionInfo => string.Equals(connectionInfo.WorksheetName, worksheetName, StringComparison.InvariantCulture));
    }

    /// <summary>
    /// Opens an <see cref="EditDataDialog"/> for each <see cref="EditConnectionInfo" />.
    /// </summary>
    /// <param name="workbook">The workbook.</param>
    private void OpenEditConnectionInfosOfTables(ExcelInterop.Workbook workbook)
    {
      if (workbook == null)
      {
        return;
      }

      var workbookConnectionInfos = GetWorkbookEditConnectionInfos(workbook);
      if (workbookConnectionInfos.Count == 0)
      {
        return;
      }

      var missingTables = new List<string>();
      _restoringExistingConnectionInfo = true;
      foreach (var connectionInfos in workbookConnectionInfos)
      {
        var editTableObject = ActiveExcelPane.LoadedTables.FirstOrDefault(dbo => string.Equals(dbo.Name, connectionInfos.TableName, StringComparison.InvariantCulture));
        if (editTableObject == null)
        {
          missingTables.Add(connectionInfos.TableName);
          continue;
        }

        ActiveExcelPane.EditTableData(editTableObject, true, workbook);
      }

      if (workbookConnectionInfos.Count - missingTables.Count > 0)
      {
        ActiveExcelPane.ActiveEditDialog.ShowDialog();
      }

      _restoringExistingConnectionInfo = false;

      // If no errors were found at the opening process do not display the warning dialog at the end.
      if (missingTables.Count <= 0)
      {
        return;
      }

      var errorMessage = new StringBuilder();
      if (missingTables.Count > 0)
      {
        errorMessage.AppendLine(Resources.RestoreConnectionInfosMissingTablesMessage);
        foreach (var table in missingTables)
        {
          errorMessage.AppendLine(table);
        }
      }

      MiscUtilities.ShowCustomizedInfoDialog(InfoDialog.InfoType.Warning, Resources.RestoreConnectionInfosWarningMessage, errorMessage.ToString());
    }

    /// <summary>
    /// Attempts to open a <see cref="MySqlWorkbenchConnection"/> from an Editing table.
    /// </summary>
    /// <param name="connectionInfoConnection">The <see cref="MySqlWorkbenchConnection"/> the <see cref="EditConnectionInfo" /> uses.</param>
    /// <returns><c>true</c> if the connection was successfully opened, <c>false</c> otherwise.</returns>
    private bool OpenConnectionForSavedEditConnectionInfo(MySqlWorkbenchConnection connectionInfoConnection)
    {
      var connectionResult = ActiveExcelPane.OpenConnection(connectionInfoConnection, false);
      if (connectionResult.Cancelled)
      {
        return false;
      }

      if (connectionResult.ConnectionSuccess)
      {
        return true;
      }

      InfoDialog.ShowDialog(InfoDialogProperties.GetWarningDialogProperties(Resources.RestoreConnectionInfosOpenConnectionErrorTitle, Resources.RestoreConnectionInfosOpenConnectionErrorDetail));
      return false;
    }

    /// <summary>
    /// Attempts to open a <see cref="MySqlWorkbenchConnection"/> from an Editing table.
    /// </summary>
    /// <param name="connectionInfo">A saved <see cref="EditConnectionInfo"/> object.</param>
    /// <param name="workbook">A <see cref="ExcelInterop.Workbook"/> object related to the <see cref="EditConnectionInfo" />.</param>
    /// <returns>The opened <see cref="MySqlWorkbenchConnection"/>.</returns>
    private MySqlWorkbenchConnection OpenConnectionForSavedEditConnectionInfo(EditConnectionInfo connectionInfo, ExcelInterop.Workbook workbook)
    {
      if (connectionInfo == null || workbook == null)
      {
        return null;
      }

      // Check if connection in stored the <see cref="EditConnectionInfo" /> still exists in the collection of Workbench connections.
      var wbConnectionInfoConnection = MySqlWorkbench.Connections.GetConnectionForId(connectionInfo.ConnectionId);
      DialogResult dialogResult;
      if (wbConnectionInfoConnection == null)
      {
        dialogResult = MiscUtilities.ShowCustomizedWarningDialog(Resources.RestoreConnectionInfosOpenConnectionErrorTitle, Resources.RestoreConnectionInfosWBConnectionNoLongerExistsFailedDetail);
        if (dialogResult == DialogResult.Yes)
        {
          DeleteCurrentWorkbookEditConnectionInfos(workbook);
        }

        return null;
      }

      wbConnectionInfoConnection.AllowZeroDateTimeValues = true;
      if (ActiveExcelPane.WbConnection == null)
      {
        // If the connection in the active pane is null it means an active connection does not exist, so open a connection.
        if (!OpenConnectionForSavedEditConnectionInfo(wbConnectionInfoConnection))
        {
          return null;
        }
      }
      else if (!string.Equals(wbConnectionInfoConnection.HostIdentifier, ActiveExcelPane.WbConnection.HostIdentifier, StringComparison.InvariantCulture))
      {
        // If the stored connection points to a different host as the current connection, ask the user if he wants to open a new connection only if there are active Edit dialogs.
        if (_editConnectionInfosByWorkbook.Count > 1)
        {
          var dialogProperties = InfoDialogProperties.GetYesNoDialogProperties(
            InfoDialog.InfoType.Warning,
            Resources.RestoreConnectionInfosTitle,
            Resources.RestoreConnectionInfosOpenConnectionCloseEditDialogsDetail,
            null,
            Resources.RestoreConnectionInfosOpenConnectionCloseEditDialogsMoreInfo);
          if (InfoDialog.ShowDialog(dialogProperties).DialogResult == DialogResult.No)
          {
            return null;
          }

          ActiveExcelPane.CloseSchema(false, false);
          ActiveExcelPane.CloseConnection(false);
        }

        if (!OpenConnectionForSavedEditConnectionInfo(wbConnectionInfoConnection))
        {
          return null;
        }
      }

      return ActiveExcelPane.WbConnection;
    }

    /// <summary>
    /// Processes the missing connections to either create and assign them a new connection or disconnect their excel tables.
    /// </summary>
    /// <param name="missingConnectionInfoConnections">A list of <see cref="ImportConnectionInfo" /> objects which connection is not found.</param>
    /// <param name="workbook">The <see cref="Microsoft.Office.Interop.Excel.Workbook" /> the list of <see cref="ImportConnectionInfo" /> belong to.</param>
    private void ProcessMissingConnectionInfoWorkbenchConnections(List<ImportConnectionInfo> missingConnectionInfoConnections, ExcelInterop.Workbook workbook)
    {
      if (missingConnectionInfoConnections.Count <= 0)
      {
        return;
      }

      var moreInfoText = MySqlWorkbench.IsRunning
        ? Resources.UnableToAddConnectionsWhenWBRunning + Environment.NewLine + Resources.ImportConnectionInfosMissingConnectionsMoreInfo
        : Resources.ImportConnectionInfosMissingConnectionsMoreInfo;
      var stringBuilder = new StringBuilder(moreInfoText);
      List<string> missingHostIds = missingConnectionInfoConnections.Select(i => i.HostIdentifier).Distinct().ToList();
      foreach (var missingHostId in missingHostIds)
      {
        stringBuilder.Append(Environment.NewLine);
        stringBuilder.Append(missingHostId);
      }

      var dialogProperties = InfoDialogProperties.GetWarningDialogProperties(
        Resources.ImportConnectionInfosMissingConnectionsTitle,
        Resources.ImportConnectionInfosMissingConnectionsDetail,
        null,
        stringBuilder.ToString());
      dialogProperties.ButtonsProperties = new InfoButtonsProperties(InfoButtonsProperties.ButtonsLayoutType.Generic3Buttons)
      {
        Button1Text = Resources.CreateButtonText,
        Button1DialogResult = DialogResult.OK,
        Button2Text = Resources.DeleteAllButtonText,
        Button2DialogResult = DialogResult.Cancel,
        Button3Text = Resources.WorkOfflineButtonText,
        Button3DialogResult = DialogResult.Abort
      };
      dialogProperties.WordWrapMoreInfo = false;
      switch (InfoDialog.ShowDialog(dialogProperties).DialogResult)
      {
        case DialogResult.OK:
          // If Workbench is running we can't add new connections, so we ask the user to close it. if he still decides not to do so we disconnect all excel tables to work offline.
          var workbenchWarningDialogResult = DialogResult.None;
          while (MySqlWorkbench.IsRunning && workbenchWarningDialogResult != DialogResult.Cancel)
          {
            workbenchWarningDialogResult = InfoDialog.ShowDialog(InfoDialogProperties.GetErrorDialogProperties(
              Resources.OperationErrorTitle,
              Resources.UnableToAddConnectionsWhenWBRunning,
              Resources.CloseWBAdviceToAdd,
              Resources.CloseWBAdviceToAdd)).DialogResult;
          }

          if (workbenchWarningDialogResult == DialogResult.Cancel)
          {
            missingConnectionInfoConnections.ForEach(connectionInfo => connectionInfo.ExcelTable.Unlink());
            break;
          }

          List<string> missingConnectionIds = missingConnectionInfoConnections.Select(i => i.ConnectionId).Distinct().ToList();
          foreach (var missingConnectionId in missingConnectionIds)
          {
            //Fill the new connection with the old HostIdentifier information for the New Connection Dialog if available;
            var missingConnectionconnectionInfo = missingConnectionInfoConnections.FirstOrDefault(s => s.ConnectionId == missingConnectionId);
            //Create the new connection and assign it to all corresponding connectionInfos.
            using (var newConnectionDialog = new MySqlWorkbenchConnectionDialog(null))
            {
              //If the HostIdentifier is set, we use it to fill in the blanks for the new connection in the dialog.
              if (missingConnectionconnectionInfo != null && !string.IsNullOrEmpty(missingConnectionconnectionInfo.HostIdentifier))
              {
                var hostIdArray = missingConnectionconnectionInfo.HostIdentifier.ToLower().Replace("mysql@", string.Empty).Split(':').ToArray();
                var host = hostIdArray.Length > 0 ? hostIdArray[0] : string.Empty;
                var portString = hostIdArray.Length > 1 ? hostIdArray[1] : string.Empty;
                uint port;
                uint.TryParse(portString, out port);
                newConnectionDialog.WorkbenchConnection.Host = host;
                newConnectionDialog.WorkbenchConnection.Port = port;
              }

              var result = newConnectionDialog.ShowDialog();
              //For each connectionInfo that is pointing to the same connection
              foreach (var connectionInfo in missingConnectionInfoConnections.Where(connectionInfo => connectionInfo.ConnectionId == missingConnectionId).ToList())
              {
                if (result == DialogResult.OK)
                {
                  //If the connection was created we reassign every corresponding connectionInfo of this set to it.
                  connectionInfo.ConnectionId = newConnectionDialog.WorkbenchConnection.Id;
                  connectionInfo.Restore(workbook);
                  MiscUtilities.SaveSettings();
                }
                else
                {
                  //If the user cancels the creation of a new connection for this set of connectionInfos, we just need to disconnect their Excel Tables.
                  connectionInfo.ExcelTable.Unlink();
                }
              }
            }
          }
          break;
        case DialogResult.Cancel:
          foreach (var connectionInfo in missingConnectionInfoConnections)
          {
            connectionInfo.ExcelTable.Unlink();
            StoredImportConnectionInfos.Remove(connectionInfo);
          }
          break;
        case DialogResult.Abort: //The user selected Work offline so we will disconnect every invalid connectionInfo.
          missingConnectionInfoConnections.ForEach(connectionInfo => connectionInfo.ExcelTable.Unlink());
          break;
      }
    }

    ///  <summary>
    /// Restores saved <see cref="EditConnectionInfo"/> objects from the given <see cref="ExcelInterop.Workbook"/>.
    /// </summary>
    /// <param name="workbook">An <see cref="ExcelInterop.Workbook"/> with saved <see cref="EditConnectionInfo"/> objects.</param>
    private void RestoreEditConnectionInfos(ExcelInterop.Workbook workbook)
    {
      if (workbook == null || ActiveExcelPane == null || _editConnectionInfosByWorkbook.ContainsKey(workbook.GetOrCreateId()))
      {
        return;
      }

      // Add the EditConnectionInfo objects for the workbook being opened to the dictionary.
      // The GetWorkbookEditConnectionInfos method will add the EditConnectionInfo objects related to the workbook it if they haven't been added.
      var workbookEditConnectionInfos = GetWorkbookEditConnectionInfos(workbook);
      if (!Settings.Default.EditSessionsRestoreWhenOpeningWorkbook || workbookEditConnectionInfos.Count == 0)
      {
        return;
      }

      // Open the connection from the EditConnectionInfo, check also if the current connection can be used to avoid opening a new one.
      var currenConnection = ActiveExcelPane.WbConnection;
      var firstConnectionInfo = workbookEditConnectionInfos[0];
      var currentSchema = currenConnection != null ? currenConnection.Schema : string.Empty;
      var connectionInfoConnection = OpenConnectionForSavedEditConnectionInfo(firstConnectionInfo, workbook);
      if (connectionInfoConnection == null)
      {
        return;
      }

      // Close the current schema if the current connection is being reused but the EditConnectionInfo's schema is different
      bool connectionReused = connectionInfoConnection.Equals(currenConnection);
      bool openSchema = !connectionReused;
      if (connectionReused && !string.Equals(currentSchema, firstConnectionInfo.SchemaName, StringComparison.InvariantCulture))
      {
        if (!ActiveExcelPane.CloseSchema(true, false))
        {
          return;
        }

        openSchema = true;
      }

      if (openSchema)
      {
        // Verify if the EditConnectionInfo's schema to be opened still exists in the connected MySQL server
        if (!ActiveExcelPane.LoadedSchemas.Exists(schemaObj => schemaObj.Name == firstConnectionInfo.SchemaName))
        {
          var errorMessage = string.Format(Resources.RestoreConnectionInfosSchemaNoLongerExistsFailed, connectionInfoConnection.HostIdentifier, connectionInfoConnection.Schema);
          MiscUtilities.ShowCustomizedInfoDialog(InfoDialog.InfoType.Error, errorMessage);
          return;
        }

        // Open the EditConnectionInfo's schema
        ActiveExcelPane.OpenSchema(firstConnectionInfo.SchemaName, true);
      }

      // Open the EditConnectionInfo for each of the tables being edited
      OpenEditConnectionInfosOfTables(workbook);
    }

    /// <summary>
    /// Restores the <see cref="ImportConnectionInfo"/>s that are tied to the given <see cref="ExcelInterop.Workbook"/>.
    /// </summary>
    /// <param name="workbook">A <see cref="ExcelInterop.Workbook"/> object.</param>
    private void RestoreImportConnectionInfos(ExcelInterop.Workbook workbook)
    {
      if (workbook == null)
      {
        return;
      }

      var workbookId = workbook.GetOrCreateId();
      var importConnectionInfos = GetWorkbookImportConnectionInfos(workbookId);
      if (importConnectionInfos == null)
      {
        return;
      }

      foreach (ImportConnectionInfo connectionInfo in importConnectionInfos)
      {
        connectionInfo.Restore(workbook);
      }

      // Verify missing connections and ask the user for action to take?
      ProcessMissingConnectionInfoWorkbenchConnections(importConnectionInfos.Where(connectionInfo => connectionInfo.ConnectionInfoError == ImportConnectionInfo.ConnectionInfoErrorType.WorkbenchConnectionDoesNotExist).ToList(), workbook);
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

        ((ExcelInterop.AppEvents_Event)Application).NewWorkbook += Application_WorkbookNewWorkbook;
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
      }
      else
      {
        if (ExcelVersionNumber >= EXCEL_2013_VERSION_NUMBER)
        {
          Application.WindowActivate -= Application_WindowActivate;
        }

        ((ExcelInterop.AppEvents_Event)Application).NewWorkbook -= Application_WorkbookNewWorkbook;
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
      }
    }

    /// <summary>
    /// Shows a dialog to the users to decide what to do with saved <see cref="EditConnectionInfo"/> objects.
    /// </summary>
    /// <param name="workbook">The <see cref="ExcelInterop.Workbook"/> that may contain saved <see cref="EditConnectionInfo"/> objects.</param>
    private void ShowOpenEditConnectionInfosDialog(ExcelInterop.Workbook workbook)
    {
      if (workbook == null)
      {
        return;
      }

      var workbookId = workbook.GetOrCreateId();
      if (!EditConnectionInfos.Exists(connectionInfo => connectionInfo.WorkbookGuid == workbookId) || _editConnectionInfosByWorkbook.ContainsKey(workbookId))
      {
        return;
      }

      var infoProperties = InfoDialogProperties.GetWarningDialogProperties(Resources.RestoreEditConnectionInfoTitle, Resources.RestoreEditConnectionInfoDetail);
      infoProperties.ButtonsProperties.LayoutType = InfoButtonsProperties.ButtonsLayoutType.Generic3Buttons;
      infoProperties.ButtonsProperties.Button1Text = Resources.RestoreButtonText;
      infoProperties.ButtonsProperties.Button1DialogResult = DialogResult.Yes;
      infoProperties.ButtonsProperties.Button2Text = Resources.WorkOfflineButtonText;
      infoProperties.ButtonsProperties.Button2DialogResult = DialogResult.Cancel;
      infoProperties.ButtonsProperties.Button3Text = Resources.DeleteButtonText;
      infoProperties.ButtonsProperties.Button3DialogResult = DialogResult.Abort;
      infoProperties.WordWrapMoreInfo = false;
      var infoResult = InfoDialog.ShowDialog(infoProperties);
      switch (infoResult.DialogResult)
      {
        case DialogResult.Abort:
          // Discard: Do not open any and delete all saved EditConnectionInfo objects for the current workbook.
          DeleteCurrentWorkbookEditConnectionInfos(workbook);
          break;

        case DialogResult.Yes:
          // Attempt to restore EditConnectionInfo objects for the workbook being opened
          RestoreEditConnectionInfos(workbook);
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
      if (ExcelPanesList != null)
      {
        foreach (var excelPane in ExcelPanesList)
        {
          excelPane.Dispose();
        }
      }

      ExcelAddInPanesClosed();
      MySqlSourceTrace.WriteToLog(Resources.ShutdownMessage, SourceLevels.Information);

      // Unsubscribe events tracked even when no Excel panes are open.
      Application.WorkbookOpen -= Application_WorkbookOpen;

      // Dispose (close) all ImportConnectionInfo object
      if (ActiveWorkbookImportConnectionInfos != null && ActiveWorkbookImportConnectionInfos.Count > 0)
      {
        foreach (var importConnectionInfo in ActiveWorkbookImportConnectionInfos)
        {
          importConnectionInfo.Dispose();
        }
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
        // Static initializations.
        System.Windows.Forms.Application.EnableVisualStyles();
        System.Windows.Forms.Application.SetCompatibleTextRenderingDefault(false);
        CustomizeInfoDialog();
        InitializeMySqlWorkbenchStaticSettings();
        MySqlInstaller.LoadData();
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

        // Subscribe events tracked even when no Excel panes are open.
        Application.WorkbookOpen += Application_WorkbookOpen;
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