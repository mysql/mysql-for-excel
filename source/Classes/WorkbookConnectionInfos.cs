// Copyright (c) 2017, 2019, Oracle and/or its affiliates. All rights reserved.
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
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Xml.Serialization;
using Microsoft.Office.Core;
using MySql.Utility.Classes;
using MySql.Utility.Classes.Logging;
using MySql.Utility.Classes.MySqlWorkbench;
using MySql.Utility.Forms;
using MySQL.ForExcel.Controls;
using MySQL.ForExcel.Forms;
using MySQL.ForExcel.Properties;
using ExcelInterop = Microsoft.Office.Interop.Excel;

namespace MySQL.ForExcel.Classes
{
  /// <summary>
  /// Contains functionality to store and retrieve connection information for Import and Edit Data operations persisted when saving a <see cref="ExcelInterop.Workbook"/>.
  /// </summary>
  [Serializable]
  public class WorkbookConnectionInfos
  {
    /// <summary>
    /// Initializes the <see cref="WorkbookConnectionInfos"/> class.
    /// </summary>
    static WorkbookConnectionInfos()
    {
      ConnectionInfosByWorkbook = new Dictionary<string, WorkbookConnectionInfos>();
    }

    /// <summary>
    /// DO NOT REMOVE. Default constructor required for serialization-deserialization.
    /// </summary>
    public WorkbookConnectionInfos()
    {
      EditConnectionInfos = new List<EditConnectionInfo>();
      EditConnectionInfosXmlPartId = null;
      ImportConnectionInfos = new List<ImportConnectionInfo>();
      ImportConnectionInfosXmlPartId = null;
      LoadDone = false;
      MigratedConnectionInfosFromSettingsFileToXmlParts = false;
      Storage = ConnectionInfosStorageType.UserSettingsFile;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="WorkbookConnectionInfos"/> class.
    /// </summary>
    /// <param name="workbook">An <see cref="ExcelInterop.Workbook"/>.</param>
    public WorkbookConnectionInfos(ExcelInterop.Workbook workbook)
      : this()
    {
      if (workbook == null)
      {
        return;
      }

      Storage = workbook.GetConnectionInfosStorageType();
      GetPropertiesFromWorkbook(workbook);
    }

    #region Enums

    /// <summary>
    /// Specifies identifiers to indicate the storage where connection information for Import and Edit Data operations is persisted.
    /// </summary>
    public enum ConnectionInfosStorageType
    {
      /// <summary>
      /// Persisted in the user settings file located in the application data directory, not portable.
      /// </summary>
      UserSettingsFile,

      /// <summary>
      /// Persisted in the <see cref="ExcelInterop.Workbook"/> as custom XML parts, which is portable.
      /// </summary>
      WorkbookXmlParts
    }

    #endregion Enums

    #region Properties

    /// <summary>
    /// Gets a dictionary containing <see cref="WorkbookConnectionInfos"/> instances per each open Workbook.
    /// </summary>
    [XmlIgnore]
    public static Dictionary<string, WorkbookConnectionInfos> ConnectionInfosByWorkbook { get; }

    /// <summary>
    /// Gets a list of <see cref="EditConnectionInfo"/> objects saved to the user settings file.
    /// </summary>
    [XmlIgnore]
    public static List<EditConnectionInfo> UserSettingsEditConnectionInfos => Settings.Default.EditConnectionInfosList ?? (Settings.Default.EditConnectionInfosList = new List<EditConnectionInfo>());

    /// <summary>
    /// Gets a list of <see cref="ImportConnectionInfo"/> objects saved to user settings file.
    /// </summary>
    [XmlIgnore]
    public static List<ImportConnectionInfo> UserSettingsImportConnectionInfos => Settings.Default.ImportConnectionInfosList ?? (Settings.Default.ImportConnectionInfosList = new List<ImportConnectionInfo>());

    /// <summary>
    /// Gets or sets a list of <see cref="EditConnectionInfo"/> objects saved to disk.
    /// </summary>
    [XmlIgnore]
    public List<EditConnectionInfo> EditConnectionInfos { get; private set; }

    /// <summary>
    /// Gets or sets the ID of the <see cref="CustomXMLPart"/> containing a serialized list of <see cref="EditConnectionInfo"/> instances.
    /// </summary>
    [XmlAttribute]
    public string EditConnectionInfosXmlPartId { get; set; }

    /// <summary>
    /// Gets or sets a list of <see cref="ImportConnectionInfo"/> objects saved to disk.
    /// </summary>
    [XmlIgnore]
    public List<ImportConnectionInfo> ImportConnectionInfos { get; private set; }

    /// <summary>
    /// Gets or sets the ID of the <see cref="CustomXMLPart"/> containing a serialized list of <see cref="ImportConnectionInfo"/> instances.
    /// </summary>
    [XmlAttribute]
    public string ImportConnectionInfosXmlPartId { get; set; }

    /// <summary>
    /// Gets a value indicating whether the load of connection information has already been performed.
    /// </summary>
    [XmlIgnore]
    public bool LoadDone { get; private set; }

    /// <summary>
    /// Gets a value indicating whether connection information was migrated from the user settings file to custom XML parts.
    /// </summary>
    [XmlIgnore]
    public bool MigratedConnectionInfosFromSettingsFileToXmlParts { get; private set; }

    /// <summary>
    /// Gets or sets the <see cref="ConnectionInfosStorageType"/> to identify where connection information is persisted.
    /// </summary>
    [XmlAttribute]
    public ConnectionInfosStorageType Storage { get; set; }

    #endregion Properties

    /// <summary>
    /// Closes and removes the <see cref="EditConnectionInfo"/> associated to the given <see cref="ExcelInterop.Worksheet"/>.
    /// </summary>
    /// <param name="workbook">An <see cref="ExcelInterop.Workbook"/>.</param>
    /// <param name="missingWorksheetName">The name of the <see cref="ExcelInterop.Worksheet"/> that no longer exists and that is associated to the <see cref="EditConnectionInfo"/> to close.</param>
    public static void CloseMissingWorksheetEditConnectionInfo(ExcelInterop.Workbook workbook, string missingWorksheetName)
    {
      if (workbook == null || string.IsNullOrEmpty(missingWorksheetName))
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
    /// Closes and removes all <see cref="EditConnectionInfo" /> associated to the given <see cref="ExcelInterop.Workbook"/>.
    /// </summary>
    /// <param name="workbook">The <see cref="ExcelInterop.Workbook"/> associated to the <see cref="EditConnectionInfo" /> objects to close.</param>
    public static void CloseWorkbookEditConnectionInfos(ExcelInterop.Workbook workbook)
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
    /// Closes and removes the <see cref="EditConnectionInfo"/> associated to the given <see cref="ExcelInterop.Worksheet"/>.
    /// </summary>
    /// <param name="worksheet">The <see cref="ExcelInterop.Worksheet"/> associated to the <see cref="EditConnectionInfo"/> to close.</param>
    public static void CloseWorksheetEditConnectionInfo(ExcelInterop.Worksheet worksheet)
    {
      if (!(worksheet?.Parent is ExcelInterop.Workbook parentWorkbook))
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
    /// Deletes automatically saved connection information entries with non-existent Excel <see cref="ExcelInterop.Workbook"/>s.
    /// </summary>
    /// <param name="logOperation">Flag indicating whether this operation is written in the application log.</param>
    public static void DeleteUserSettingsConnectionInfosWithNonExistentWorkbook(bool logOperation)
    {
      if (!Settings.Default.DeleteAutomaticallyOrphanedConnectionInfos)
      {
        return;
      }

      var orphanedConnectionInfos = ManageConnectionInfosDialog.GetConnectionInfosWithNonExistentWorkbook();
      if (orphanedConnectionInfos == null || orphanedConnectionInfos.Count == 0)
      {
        return;
      }

      if (logOperation)
      {
        Logger.LogInformation(Resources.DeletingConnectionInfosWithNonExistentWorkbook);
      }

      foreach (var connectionInfo in orphanedConnectionInfos)
      {
        if (connectionInfo.GetType() == typeof(EditConnectionInfo))
        {
          UserSettingsEditConnectionInfos.Remove(connectionInfo as EditConnectionInfo);
        }
        else
        {
          UserSettingsImportConnectionInfos.Remove(connectionInfo as ImportConnectionInfo);
        }
      }

      MiscUtilities.SaveSettings();
    }

    /// <summary>
    /// Disposes all <see cref="ImportConnectionInfo"/> instances related to the given <see cref="ExcelInterop.Workbook"/>.
    /// </summary>
    /// <param name="workbook">A <see cref="ExcelInterop.Workbook"/> instance.</param>
    public static void DisposeWorkbookImportConnectionInfos(ExcelInterop.Workbook workbook)
    {
      if (workbook == null)
      {
        return;
      }

      var workbookImportConnectionInfos = GetWorkbookImportConnectionInfos(workbook);
      foreach (var importConnectionInfo in workbookImportConnectionInfos)
      {
        importConnectionInfo.Dispose();
      }
    }

    /// <summary>
    /// Gets a <see cref="WorkbookConnectionInfos"/> associated to the given <see cref="ExcelInterop.Workbook"/>.
    /// </summary>
    /// <param name="workbook">A <see cref="ExcelInterop.Workbook"/> instance.</param>
    /// <param name="loadIfNotPresent">Loads the connection infos if the workbook is not in the main <see cref="ConnectionInfosByWorkbook"/> collection.</param>
    /// <returns>A <see cref="WorkbookConnectionInfos"/> associated to the given <see cref="ExcelInterop.Workbook"/>.</returns>
    public static WorkbookConnectionInfos GetWorkbookConnectionInfos(ExcelInterop.Workbook workbook, bool loadIfNotPresent = true)
    {
      if (workbook == null)
      {
        return null;
      }

      var workbookId = workbook.GetOrCreateId();
      if (string.IsNullOrEmpty(workbookId))
      {
        return null;
      }

      var workbookConnectionsInfos = ConnectionInfosByWorkbook.ContainsKey(workbookId)
        ? ConnectionInfosByWorkbook[workbookId]
        : null;

      if (workbookConnectionsInfos != null
          || !loadIfNotPresent)
      {
        return workbookConnectionsInfos;
      }

      workbookConnectionsInfos = new WorkbookConnectionInfos(workbook);
      workbookConnectionsInfos.Load(workbook);
      if (ConnectionInfosByWorkbook.ContainsKey(workbookId))
      {
        ConnectionInfosByWorkbook[workbookId] = workbookConnectionsInfos;
      }
      else
      {
        ConnectionInfosByWorkbook.Add(workbookId, workbookConnectionsInfos);
      }

      return workbookConnectionsInfos;
    }

    /// <summary>
    /// Gets a subset of the <see cref="EditConnectionInfo"/> listing only those associated to the given <see cref="ExcelInterop.Workbook"/>.
    /// </summary>
    /// <param name="workbook">A <see cref="ExcelInterop.Workbook"/> with active <see cref="EditConnectionInfo"/> objects.</param>
    /// <returns>A subset of the <see cref="EditConnectionInfo"/> listing only those associated to the given <see cref="ExcelInterop.Workbook"/></returns>
    public static List<EditConnectionInfo> GetWorkbookEditConnectionInfos(ExcelInterop.Workbook workbook)
    {
      var workbookConnectionInfos = GetWorkbookConnectionInfos(workbook);
      return workbookConnectionInfos == null
        ? new List<EditConnectionInfo>()
        : workbookConnectionInfos.EditConnectionInfos;
    }

    /// <summary>
    /// Gets a subset of the <see cref="ImportConnectionInfo" /> listing only those associated to the given <see cref="ExcelInterop.Workbook" />.
    /// </summary>
    /// <param name="workbook">A <see cref="ExcelInterop.Workbook"/> with active <see cref="ImportConnectionInfo"/> objects.</param>
    /// <returns> A subset of the <see cref="ImportConnectionInfo" /> listing only those associated to the given <see cref="ExcelInterop.Workbook" /></returns>
    public static List<ImportConnectionInfo> GetWorkbookImportConnectionInfos(ExcelInterop.Workbook workbook)
    {
      var workbookConnectionInfos = GetWorkbookConnectionInfos(workbook);
      return workbookConnectionInfos == null
        ? new List<ImportConnectionInfo>()
        : workbookConnectionInfos.ImportConnectionInfos;
    }

    /// <summary>
    /// Attempts to open a <see cref="MySqlWorkbenchConnection"/> from an Editing table.
    /// </summary>
    /// <param name="editConnectionInfo">A saved <see cref="EditConnectionInfo"/> object.</param>
    /// <param name="workbook">A <see cref="ExcelInterop.Workbook"/> instance.</param>
    /// <returns>The opened <see cref="MySqlWorkbenchConnection"/>.</returns>
    public static MySqlWorkbenchConnection OpenConnectionForSavedEditConnectionInfo(EditConnectionInfo editConnectionInfo, ExcelInterop.Workbook workbook)
    {
      if (editConnectionInfo == null || workbook == null)
      {
        return null;
      }

      // Check if the editConnectionInfo still exists in the collection of Workbench connections.
      var wbConnectionInfoConnection = MySqlWorkbench.Connections.GetConnectionForId(editConnectionInfo.ConnectionId);
      if (wbConnectionInfoConnection == null)
      {
        var dialogResult = MiscUtilities.ShowCustomizedWarningDialog(Resources.RestoreConnectionInfosOpenConnectionErrorTitle, Resources.RestoreConnectionInfosWBConnectionNoLongerExistsFailedDetail);
        if (dialogResult == DialogResult.Yes)
        {
          RemoveAllEditConnectionInfos(workbook);
        }

        return null;
      }

      wbConnectionInfoConnection.SetAdditionalConnectionProperties();
      var activeExcelPane = Globals.ThisAddIn.ActiveExcelPane;
      if (activeExcelPane == null)
      {
        return null;
      }

      if (activeExcelPane.WbConnection == null)
      {
        // If the connection in the active pane is null it means an active connection does not exist, so open a connection.
        if (!OpenConnectionForSavedConnectionInfo(wbConnectionInfoConnection, activeExcelPane))
        {
          return null;
        }
      }
      else if (!string.Equals(wbConnectionInfoConnection.HostIdentifier, activeExcelPane.WbConnection.HostIdentifier, StringComparison.InvariantCulture))
      {
        // If the stored connection points to a different host as the current connection, ask the user if he wants to open a new connection only if there are active Edit dialogs.
        var workbookId = workbook.GetOrCreateId();
        if (ConnectionInfosByWorkbook.Count(ci => ci.Key != workbookId && ci.Value.EditConnectionInfos.Count > 0) > 0)
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

          activeExcelPane.CloseSchema(false, false);
          activeExcelPane.CloseConnection(false);
        }

        if (!OpenConnectionForSavedConnectionInfo(wbConnectionInfoConnection, activeExcelPane))
        {
          return null;
        }
      }

      return activeExcelPane.WbConnection;
    }

    /// <summary>
    /// Opens an <see cref="EditDataDialog"/> for each <see cref="EditConnectionInfo" />.
    /// </summary>
    /// <param name="workbook">The workbook.</param>
    public static void OpenEditConnectionInfosOfTables(ExcelInterop.Workbook workbook)
    {
      if (workbook == null)
      {
        return;
      }

      var activeExcelPane = Globals.ThisAddIn.ActiveExcelPane;
      if (activeExcelPane == null)
      {
        return;
      }

      var workbookEditConnectionInfos = GetWorkbookEditConnectionInfos(workbook);
      if (workbookEditConnectionInfos.Count == 0)
      {
        return;
      }

      var missingTables = new List<string>();
      Globals.ThisAddIn.RestoringExistingConnectionInfo = true;
      foreach (var connectionInfos in workbookEditConnectionInfos)
      {
        var editTableObject = activeExcelPane.LoadedTables.FirstOrDefault(dbo => string.Equals(dbo.Name, connectionInfos.TableName, StringComparison.InvariantCulture));
        if (editTableObject == null)
        {
          missingTables.Add(connectionInfos.TableName);
          continue;
        }

        activeExcelPane.EditTableData(editTableObject, true, workbook);
      }

      if (workbookEditConnectionInfos.Count - missingTables.Count > 0)
      {
        activeExcelPane.ActiveEditDialog.ShowDialog();
      }

      Globals.ThisAddIn.RestoringExistingConnectionInfo = false;

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
    /// Delete all <see cref="EditConnectionInfo"/> objects related to the given <see cref="ExcelInterop.Workbook"/>.
    /// </summary>
    /// <param name="workbook">A <see cref="ExcelInterop.Workbook"/> instance.</param>
    public static void RemoveAllEditConnectionInfos(ExcelInterop.Workbook workbook)
    {
      if (workbook == null)
      {
        return;
      }

      var workbookConnectionInfos = GetWorkbookConnectionInfos(workbook);

      // Remove all EditConnectionInfo objects from the current workbook.
      if (workbookConnectionInfos != null)
      {
        workbookConnectionInfos.EditConnectionInfos.Clear();
        workbookConnectionInfos.Save(workbook);
      }
    }

    /// <summary>
    /// Removes the <see cref="EditConnectionInfo"/> in the active <see cref="ExcelInterop.Workbook"/> that contains the given <see cref="EditDataDialog"/>.
    /// </summary>
    /// <param name="editDialog">An <see cref="EditDataDialog"/> instance.</param>
    public static void RemoveEditConnectionInfoWithEditDialog(EditDataDialog editDialog)
    {
      if (editDialog == null)
      {
        return;
      }

      var activeWorkbookEditConnectionInfos = GetWorkbookEditConnectionInfos(Globals.ThisAddIn.ActiveWorkbook);
      var editConnectionInfoToRemove = activeWorkbookEditConnectionInfos.FirstOrDefault(ac => ac.EditDialog.Equals(editDialog));
      if (editConnectionInfoToRemove != null)
      {
        activeWorkbookEditConnectionInfos.Remove(editConnectionInfoToRemove);
      }
    }

    /// <summary>
    /// Removes invalid import connection information from the collection.
    /// </summary>
    /// <param name="workbook">A <see cref="ExcelInterop.Workbook"/> instance.</param>
    public static void RemoveInvalidImportConnectionInfos(ExcelInterop.Workbook workbook)
    {
      if (workbook == null)
      {
        return;
      }

      var workbookImportConnectionInfos = GetWorkbookImportConnectionInfos(workbook);
      var invalidConnectionInfos = new List<ImportConnectionInfo>();
      foreach (var importConnectionInfo in workbookImportConnectionInfos)
      {
        try
        {
          // DO NOT REMOVE this line. If the excel table is invalid, accessing it will throw an exception.
          // ReSharper disable once UnusedVariable
          var excelTableComment = importConnectionInfo.ExcelTable.Comment;
          importConnectionInfo.LastAccess = DateTime.Now;
        }
        catch
        {
          // The importConnectionInfo's list object was moved to another worksheet or when its columns had been deleted or the reference to it no longer exists.
          invalidConnectionInfos.Add(importConnectionInfo);
        }
      }

      // Delete ImportConnectionInfo objects that are no longer valid for the current workbook.
      if (invalidConnectionInfos.Count > 0)
      {
        invalidConnectionInfos.ForEach(invalidSession => invalidSession.ExcelTable.DeleteSafely(false));
        invalidConnectionInfos.ForEach(invalidSession => workbookImportConnectionInfos.Remove(invalidSession));
      }
    }

    /// <summary>
    /// Removes migrated <see cref="ImportConnectionInfo"/>s and <see cref="EditConnectionInfo"/>s from the user settings file.
    /// </summary>
    /// <param name="workbook">A <see cref="ExcelInterop.Workbook"/> instance.</param>
    public static void RemoveMigratedConnectionInfosFromSettingsFile(ExcelInterop.Workbook workbook)
    {
      if (workbook == null)
      {
        return;
      }

      var workbookConnectionInfos = GetWorkbookConnectionInfos(workbook);
      if (workbookConnectionInfos == null || !workbookConnectionInfos.MigratedConnectionInfosFromSettingsFileToXmlParts)
      {
        return;
      }

      var workbookId = workbook.GetOrCreateId();

      // Remove migrated ImportConnectionInfos
      UserSettingsImportConnectionInfos.RemoveAll(ici => ici.WorkbookGuid == workbookId && workbookConnectionInfos.ImportConnectionInfos.Contains(ici));

      // Remove migrated EditConnectionInfos
      UserSettingsEditConnectionInfos.RemoveAll(eci => eci.WorkbookGuid == workbookId && workbookConnectionInfos.EditConnectionInfos.Contains(eci));

      MiscUtilities.SaveSettings();
    }

    ///  <summary>
    /// Restores saved <see cref="EditConnectionInfo"/> objects from the given <see cref="ExcelInterop.Workbook"/>.
    /// </summary>
    /// <param name="workbook">An <see cref="ExcelInterop.Workbook"/> with saved <see cref="EditConnectionInfo"/> objects.</param>
    public static void RestoreEditConnectionInfos(ExcelInterop.Workbook workbook)
    {
      var activeExcelPane = Globals.ThisAddIn.ActiveExcelPane;
      if (workbook == null || activeExcelPane == null || !ConnectionInfosByWorkbook.ContainsKey(workbook.GetOrCreateId()))
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
      var currentConnection = activeExcelPane.WbConnection;
      var firstConnectionInfo = workbookEditConnectionInfos[0];
      var currentSchema = currentConnection != null ? currentConnection.Schema : string.Empty;
      var connectionInfoConnection = OpenConnectionForSavedEditConnectionInfo(firstConnectionInfo, workbook);
      if (connectionInfoConnection == null)
      {
        return;
      }

      // Close the current schema if the current connection is being reused but the EditConnectionInfo's schema is different
      var connectionReused = connectionInfoConnection.Equals(currentConnection);
      var openSchema = !connectionReused;
      if (connectionReused && !string.Equals(currentSchema, firstConnectionInfo.SchemaName, StringComparison.InvariantCulture))
      {
        if (!activeExcelPane.CloseSchema(true, false))
        {
          return;
        }

        openSchema = true;
      }

      if (openSchema)
      {
        // Verify if the EditConnectionInfo's schema to be opened still exists in the connected MySQL server
        if (!activeExcelPane.LoadedSchemas.Exists(schemaObj => schemaObj.Name == firstConnectionInfo.SchemaName))
        {
          var errorMessage = string.Format(Resources.RestoreConnectionInfosSchemaNoLongerExistsFailed, connectionInfoConnection.HostIdentifier, connectionInfoConnection.Schema);
          MiscUtilities.ShowCustomizedInfoDialog(InfoDialog.InfoType.Error, errorMessage);
          return;
        }

        // Open the EditConnectionInfo's schema
        activeExcelPane.OpenSchema(firstConnectionInfo.SchemaName, true);
      }

      // Open the EditConnectionInfo for each of the tables being edited
      OpenEditConnectionInfosOfTables(workbook);
    }

    /// <summary>
    /// Restores the <see cref="ImportConnectionInfo"/>s that are tied to the given <see cref="ExcelInterop.Workbook"/>.
    /// </summary>
    /// <param name="workbook">A <see cref="ExcelInterop.Workbook"/> object.</param>
    public static void RestoreImportConnectionInfos(ExcelInterop.Workbook workbook)
    {
      if (workbook == null)
      {
        return;
      }

      var importConnectionInfos = GetWorkbookImportConnectionInfos(workbook);
      if (importConnectionInfos == null)
      {
        return;
      }

      foreach (var connectionInfo in importConnectionInfos)
      {
        connectionInfo.Restore(workbook);
      }

      // Verify missing connections and ask the user for action to take?
      ProcessMissingConnectionInfoWorkbenchConnections(importConnectionInfos, workbook);
    }

    /// <summary>
    /// Saves connection information for Import and Edit Data operations in the storage defined in <see cref="Storage"/>.
    /// </summary>
    /// <param name="workbook">An <see cref="ExcelInterop.Workbook"/> instance.</param>
    public static void SaveForWorkbook(ExcelInterop.Workbook workbook)
    {
      var workbookConnectionInfos = GetWorkbookConnectionInfos(workbook);
      workbookConnectionInfos?.Save(workbook);
    }

    /// <summary>
    /// Loads connection information for Import and Edit Data operations from the storage defined in <see cref="Storage"/>.
    /// </summary>
    /// <param name="workbook">An <see cref="ExcelInterop.Workbook"/> instance.</param>
    public void Load(ExcelInterop.Workbook workbook)
    {
      if (LoadDone)
      {
        return;
      }

      // Attempt to move ConnectionInfo objects from settings file to the workbook custom properties
      MigrateConnectionInfosFromSettingsFileToCustomProperties(workbook);

      // Load  information
      LoadEditConnectionInfos(workbook);
      LoadImportConnectionInfos(workbook);
      LoadDone = true;
    }

    /// <summary>
    /// Saves connection information for Import and Edit Data operations in the storage defined in <see cref="Storage"/>.
    /// </summary>
    /// <param name="workbook">An <see cref="ExcelInterop.Workbook"/> instance.</param>
    public void Save(ExcelInterop.Workbook workbook)
    {
      SaveEditConnectionInfos(workbook);
      SaveImportConnectionInfos(workbook);
      if (Storage == ConnectionInfosStorageType.UserSettingsFile)
      {
        MiscUtilities.SaveSettings();
      }

      if (workbook == null)
      {
        return;
      }

      SavePropertiesIntoWorkbook(workbook);
    }

    /// <summary>
    /// Attempts to open a <see cref="MySqlWorkbenchConnection"/> from an Editing table.
    /// </summary>
    /// <param name="connectionInfoConnection">The <see cref="MySqlWorkbenchConnection"/> the <see cref="EditConnectionInfo" /> uses.</param>
    /// <param name="activeExcelPane">The pane containing the MySQL for Excel add-in contained in the custom task pane shown in the active window.</param>
    /// <returns><c>true</c> if the connection was successfully opened, <c>false</c> otherwise.</returns>
    private static bool OpenConnectionForSavedConnectionInfo(MySqlWorkbenchConnection connectionInfoConnection, ExcelAddInPane activeExcelPane)
    {
      if (activeExcelPane == null)
      {
        return false;
      }

      var connectionResult = activeExcelPane.OpenConnection(connectionInfoConnection, false);
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
    /// Processes the missing connections to either create and assign them a new connection or disconnect their excel tables.
    /// </summary>
    /// <param name="workbookImportConnectionInfos">A list of <see cref="ImportConnectionInfo" /> objects which connection is not found.</param>
    /// <param name="workbook">The <see cref="ExcelInterop.Workbook" /> the list of <see cref="ImportConnectionInfo" /> belong to.</param>
    private static void ProcessMissingConnectionInfoWorkbenchConnections(List<ImportConnectionInfo> workbookImportConnectionInfos, ExcelInterop.Workbook workbook)
    {
      if (workbook == null || workbookImportConnectionInfos == null || workbookImportConnectionInfos.Count == 0)
      {
        return;
      }

      var missingConnectionInfoConnections = workbookImportConnectionInfos.Where(connectionInfo => connectionInfo.ConnectionInfoError == ImportConnectionInfo.ConnectionInfoErrorType.WorkbenchConnectionDoesNotExist).ToList();
      if (missingConnectionInfoConnections.Count <= 0)
      {
        return;
      }

      var moreInfoText = MySqlWorkbench.IsRunning
        ? Resources.UnableToAddConnectionsWhenWBRunning + Environment.NewLine + Resources.ImportConnectionInfosMissingConnectionsMoreInfo
        : Resources.ImportConnectionInfosMissingConnectionsMoreInfo;
      var stringBuilder = new StringBuilder(moreInfoText);
      var missingHostIds = missingConnectionInfoConnections.Select(i => i.HostIdentifier).Distinct().ToList();
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
      dialogProperties.CommandAreaProperties = new CommandAreaProperties(CommandAreaProperties.ButtonsLayoutType.Generic3Buttons)
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

          var missingConnectionIds = missingConnectionInfoConnections.Select(i => i.ConnectionId).Distinct().ToList();
          foreach (var missingConnectionId in missingConnectionIds)
          {
            // Fill the new connection with the old HostIdentifier information for the New Connection Dialog if available;
            var missingConnectionInfo = missingConnectionInfoConnections.FirstOrDefault(s => s.ConnectionId == missingConnectionId);
            // Create the new connection and assign it to all corresponding connectionInfos.
            using (var newConnectionDialog = new MySqlWorkbenchConnectionDialog(null, false))
            {
              //If the HostIdentifier is set, we use it to fill in the blanks for the new connection in the dialog.
              if (missingConnectionInfo != null && !string.IsNullOrEmpty(missingConnectionInfo.HostIdentifier))
              {
                var hostIdArray = missingConnectionInfo.HostIdentifier.ToLower().Replace("mysql@", string.Empty).Split(':').ToArray();
                var host = hostIdArray.Length > 0 ? hostIdArray[0] : string.Empty;
                var portString = hostIdArray.Length > 1 ? hostIdArray[1] : string.Empty;
                uint.TryParse(portString, out var port);
                newConnectionDialog.WorkbenchConnection.Host = host;
                newConnectionDialog.WorkbenchConnection.Port = port;
              }

              var result = newConnectionDialog.ShowDialog();
              // For each connectionInfo that is pointing to the same connection
              foreach (var connectionInfo in missingConnectionInfoConnections.Where(connectionInfo => connectionInfo.ConnectionId == missingConnectionId).ToList())
              {
                if (result == DialogResult.OK)
                {
                  // If the connection was created we reassign every corresponding connectionInfo of this set to it.
                  connectionInfo.ConnectionId = newConnectionDialog.WorkbenchConnection.Id;
                  connectionInfo.Restore(workbook);
                  MiscUtilities.SaveSettings();
                }
                else
                {
                  // If the user cancels the creation of a new connection for this set of connectionInfos, we just need to disconnect their Excel Tables.
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
            workbookImportConnectionInfos.Remove(connectionInfo);
          }
          break;

        case DialogResult.Abort:
          // The user selected Work offline so we will disconnect every invalid connectionInfo.
          missingConnectionInfoConnections.ForEach(connectionInfo => connectionInfo.ExcelTable.Unlink());
          break;
      }
    }

    /// <summary>
    /// Retrieves values of <see cref="WorkbookConnectionInfos"/> properties related to the given <see cref="ExcelInterop.Workbook"/> from its document properties.
    /// </summary>
    /// <param name="workbook">An <see cref="ExcelInterop.Workbook"/>.</param>
    private void GetPropertiesFromWorkbook(ExcelInterop.Workbook workbook)
    {
      if (workbook == null)
      {
        return;
      }

      var propertyValue = workbook.LoadStringDocumentProperty(nameof(EditConnectionInfosXmlPartId));
      if (propertyValue != null)
      {
        EditConnectionInfosXmlPartId = propertyValue;
      }

      propertyValue = workbook.LoadStringDocumentProperty(nameof(ImportConnectionInfosXmlPartId));
      if (propertyValue != null)
      {
        ImportConnectionInfosXmlPartId = propertyValue;
      }

      propertyValue = workbook.LoadStringDocumentProperty("ConnectionInfosStorage");
      if (propertyValue == null)
      {
        return;
      }

      if (Enum.TryParse(propertyValue, out ConnectionInfosStorageType storage))
      {
        Storage = storage;
      }
    }

    /// <summary>
    /// Loads the value of the <see cref="EditConnectionInfos"/> property from the storage defined in <see cref="Storage"/>.
    /// </summary>
    /// <param name="workbook">An <see cref="ExcelInterop.Workbook"/> instance.</param>
    private void LoadEditConnectionInfos(ExcelInterop.Workbook workbook)
    {
      switch (Storage)
      {
        case ConnectionInfosStorageType.UserSettingsFile:
          var workbookId = workbook.GetOrCreateId();
          EditConnectionInfos = UserSettingsEditConnectionInfos.FindAll(editConnectionInfo => editConnectionInfo.WorkbookGuid == workbookId);
          break;

        case ConnectionInfosStorageType.WorkbookXmlParts:
          if (workbook == null || string.IsNullOrEmpty(EditConnectionInfosXmlPartId))
          {
            return;
          }

          var customXmlPart = workbook.CustomXMLParts.SelectByID(EditConnectionInfosXmlPartId);
          if (customXmlPart == null)
          {
            return;
          }

          EditConnectionInfos = customXmlPart.XML.Deserialize<List<EditConnectionInfo>>();
          break;
      }
    }

    /// <summary>
    /// Loads the value of the <see cref="ImportConnectionInfos"/> property from the storage defined in <see cref="Storage"/>.
    /// </summary>
    /// <param name="workbook">An <see cref="ExcelInterop.Workbook"/> instance.</param>
    private void LoadImportConnectionInfos(ExcelInterop.Workbook workbook)
    {
      switch (Storage)
      {
        case ConnectionInfosStorageType.UserSettingsFile:
          var workbookId = workbook.GetOrCreateId();
          ImportConnectionInfos = UserSettingsImportConnectionInfos.FindAll(importConnectionInfo => importConnectionInfo.WorkbookGuid == workbookId);
          break;

        case ConnectionInfosStorageType.WorkbookXmlParts:
          if (workbook == null || string.IsNullOrEmpty(ImportConnectionInfosXmlPartId))
          {
            return;
          }

          var customXmlPart = workbook.CustomXMLParts.SelectByID(ImportConnectionInfosXmlPartId);
          if (customXmlPart == null)
          {
            return;
          }

          ImportConnectionInfos = customXmlPart.XML.Deserialize<List<ImportConnectionInfo>>();
          break;
      }
    }

    /// <summary>
    /// Migrates connection information for Import and Edit Data operations from the user settings file to Workbook custom properties if possible.
    /// </summary>
    /// <param name="workbook">A <see cref="ExcelInterop.Workbook"/> instance.</param>
    private void MigrateConnectionInfosFromSettingsFileToCustomProperties(ExcelInterop.Workbook workbook)
    {
      if (workbook == null
          || MigratedConnectionInfosFromSettingsFileToXmlParts
          || Storage == ConnectionInfosStorageType.WorkbookXmlParts
          || !workbook.SupportsXmlParts())
      {
        return;
      }

      var workbookId = workbook.GetOrCreateId();
      var userSettingsWorkbookImportConnectionInfos = UserSettingsImportConnectionInfos.FindAll(ici => ici.WorkbookGuid == workbookId);
      var userSettingsWorkbookEditConnectionInfos = UserSettingsEditConnectionInfos.FindAll(eci => eci.WorkbookGuid == workbookId);
      if (userSettingsWorkbookImportConnectionInfos.Count + userSettingsWorkbookEditConnectionInfos.Count == 0)
      {
        // Nothing to migrate, but let's ensure from now on data is stored in XML parts.
        Storage = ConnectionInfosStorageType.WorkbookXmlParts;
        return;
      }

      var dialogProperties = InfoDialogProperties.GetYesNoDialogProperties(
            InfoDialog.InfoType.Warning,
            Resources.MoveConnectionsInfoFromSettingsFileToXmlPartsTitle,
            Resources.MoveConnectionsInfoFromSettingsFileToXmlPartsDetail,
            Resources.MoveConnectionsInfoFromSettingsFileToXmlPartsSubDetail,
            Resources.MoveConnectionsInfoFromSettingsFileToXmlPartsMoreInfo);
      dialogProperties.WordWrapMoreInfo = true;
      if (InfoDialog.ShowDialog(dialogProperties).DialogResult == DialogResult.No)
      {
        Storage = ConnectionInfosStorageType.UserSettingsFile;
        return;
      }

      var migratedConnectionInfosCount = 0;
      Storage = ConnectionInfosStorageType.WorkbookXmlParts;

      // Migrate ImportConnectionInfos
      foreach (var importConnectionInfo in userSettingsWorkbookImportConnectionInfos)
      {
        if (ImportConnectionInfos.Contains(importConnectionInfo))
        {
          continue;
        }

        ImportConnectionInfos.Add(importConnectionInfo);
        UserSettingsImportConnectionInfos.Remove(importConnectionInfo);
        migratedConnectionInfosCount++;
      }

      // Migrate EditConnectionInfos
      foreach (var editConnectionInfo in userSettingsWorkbookEditConnectionInfos)
      {
        if (EditConnectionInfos.Contains(editConnectionInfo))
        {
          continue;
        }

        EditConnectionInfos.Add(editConnectionInfo);
        UserSettingsEditConnectionInfos.Remove(editConnectionInfo);
        migratedConnectionInfosCount++;
      }

      MigratedConnectionInfosFromSettingsFileToXmlParts = migratedConnectionInfosCount > 0;
    }

    /// <summary>
    /// Saves the value of the <see cref="EditConnectionInfos"/> property in the storage defined in <see cref="Storage"/>.
    /// </summary>
    /// <param name="workbook">An <see cref="ExcelInterop.Workbook"/> instance.</param>
    private void SaveEditConnectionInfos(ExcelInterop.Workbook workbook)
    {
      switch (Storage)
      {
        case ConnectionInfosStorageType.UserSettingsFile:
          UserSettingsEditConnectionInfos.RemoveAll(eci => !EditConnectionInfos.Contains(eci));
          foreach (var eci in EditConnectionInfos)
          {
            if (UserSettingsEditConnectionInfos.Contains(eci))
            {
              continue;
            }

            UserSettingsEditConnectionInfos.Add(eci);
          }
          break;

        case ConnectionInfosStorageType.WorkbookXmlParts:
          if (workbook == null)
          {
            return;
          }

          var customXmlPart = string.IsNullOrEmpty(EditConnectionInfosXmlPartId)
            ? null
            : workbook.CustomXMLParts.SelectByID(EditConnectionInfosXmlPartId);
          customXmlPart?.Delete();
          if (EditConnectionInfos.Count > 0)
          {
            EditConnectionInfosXmlPartId = workbook.CustomXMLParts.Add(EditConnectionInfos.Serialize()).Id;
          }
          break;
      }
    }

    /// <summary>
    /// Saves the value of the <see cref="ImportConnectionInfos"/> property in the storage defined in <see cref="Storage"/>.
    /// </summary>
    /// <param name="workbook">An <see cref="ExcelInterop.Workbook"/> instance.</param>
    private void SaveImportConnectionInfos(ExcelInterop.Workbook workbook)
    {
      switch (Storage)
      {
        case ConnectionInfosStorageType.UserSettingsFile:
          UserSettingsImportConnectionInfos.RemoveAll(ici => !ImportConnectionInfos.Contains(ici));
          foreach (var ici in ImportConnectionInfos)
          {
            if (UserSettingsImportConnectionInfos.Contains(ici))
            {
              continue;
            }

            UserSettingsImportConnectionInfos.Add(ici);
          }
          break;

        case ConnectionInfosStorageType.WorkbookXmlParts:
          if (workbook == null)
          {
            return;
          }

          var customXmlPart = string.IsNullOrEmpty(ImportConnectionInfosXmlPartId)
            ? null
            : workbook.CustomXMLParts.SelectByID(ImportConnectionInfosXmlPartId);
          customXmlPart?.Delete();
          if (ImportConnectionInfos.Count > 0)
          {
            ImportConnectionInfosXmlPartId = workbook.CustomXMLParts.Add(ImportConnectionInfos.Serialize()).Id;
          }
          break;
      }
    }

    /// <summary>
    /// Saves values of <see cref="WorkbookConnectionInfos"/> properties related to the given <see cref="ExcelInterop.Workbook"/> into its document properties.
    /// </summary>
    /// <param name="workbook">An <see cref="ExcelInterop.Workbook"/>.</param>
    private void SavePropertiesIntoWorkbook(ExcelInterop.Workbook workbook)
    {
      if (workbook == null)
      {
        return;
      }

      workbook.SaveStringDocumentProperty(nameof(EditConnectionInfosXmlPartId), EditConnectionInfosXmlPartId);
      workbook.SaveStringDocumentProperty(nameof(ImportConnectionInfosXmlPartId), ImportConnectionInfosXmlPartId);
      workbook.SaveStringDocumentProperty("ConnectionInfosStorage", Storage.ToString());
    }
  }
}