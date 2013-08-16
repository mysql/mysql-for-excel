// Copyright (c) 2013, Oracle and/or its affiliates. All rights reserved.
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

namespace ExcelCustomAction
{
  using System;
  using System.IO;
  using System.Reflection;
  using System.Security.AccessControl;
  using Microsoft.Deployment.WindowsInstaller;
  using Microsoft.Win32;

  /// <summary>
  /// Contains custom actions exposed to the MySQL for Excel WiX installer to support custom installation pieces.
  /// </summary>
  public class CustomActions
  {
    /// <summary>
    /// The location of the MySQL for Excel registry key from the HK_CURRENT_USER or HK_LOCAL MACHINE root.
    /// </summary>
    public const string MYSQL_FOR_EXCEL_REGISTRY_KEY_LOCATION = @"SOFTWARE\Microsoft\Office\Excel\Addins\MySQL.ForExcel";

    /// <summary>
    /// The location of the Office registry key from the HK_CURRENT_USER or HK_LOCAL MACHINE root.
    /// </summary>
    public const string MS_OFFICE_REGISTRY_KEY_LOCATION = @"SOFTWARE\Microsoft\Office";

    /// <summary>
    /// Specifies identifiers to indicate the type of registry action to do by the installer.
    /// </summary>
    public enum AddInRegistryAction
    {
      /// <summary>
      /// Installs (adds) registry keys.
      /// </summary>
      Install,

      /// <summary>
      /// Removes registry keys.
      /// </summary>
      Remove
    }

    /// <summary>
    /// Specifies identifiers to indicate the registry view accessed by the installer.
    /// </summary>
    public enum AddInRegistryView
    {
      /// <summary>
      /// Registry view for 32-bit applications.
      /// </summary>
      Registry32,

      /// <summary>
      /// Registry view for 64-bit applications.
      /// </summary>
      Registry64
    }

    /// <summary>
    /// Installs (adds) the MySQL for Excel needed registry keys in the 32-bit registry view.
    /// </summary>
    /// <param name="session">The installer's session object.</param>
    /// <returns>The result of the custom action.</returns>
    [CustomAction]
    public static ActionResult InstallAddIn32(Session session)
    {
      return PerformActionOnRegistryView(AddInRegistryAction.Install, AddInRegistryView.Registry32, session);
    }

    /// <summary>
    /// Installs (adds) the MySQL for Excel needed registry keys in the 64-bit registry view.
    /// </summary>
    /// <param name="session">The installer's session object.</param>
    /// <returns>The result of the custom action.</returns>
    [CustomAction]
    public static ActionResult InstallAddIn64(Session session)
    {
      return PerformActionOnRegistryView(AddInRegistryAction.Install, AddInRegistryView.Registry64, session);
    }

    /// <summary>
    /// Removes the MySQL for Excel installed registry keys from the 32-bit registry view.
    /// </summary>
    /// <param name="session">The installer's session object.</param>
    /// <returns>The result of the custom action.</returns>
    [CustomAction]
    public static ActionResult RemoveAddIn32(Session session)
    {
      return PerformActionOnRegistryView(AddInRegistryAction.Remove, AddInRegistryView.Registry32, session);
    }

    /// <summary>
    /// Removes the MySQL for Excel installed registry keys from the 64-bit registry view.
    /// </summary>
    /// <param name="session">The installer's session object.</param>
    /// <returns>The result of the custom action.</returns>
    [CustomAction]
    public static ActionResult RemoveAddIn64(Session session)
    {
      return PerformActionOnRegistryView(AddInRegistryAction.Remove, AddInRegistryView.Registry64, session);
    }

    /// <summary>
    /// Retrieves the title and description from the MySQL for Excel assembly information.
    /// </summary>
    /// <param name="excelDll">The MySQL for Excel assembly.</param>
    /// <returns>An array of 2 elements containing the title and description.</returns>
    private static string[] GetMySqlForExelDllTitleAndDescription(string excelDll)
    {
      string[] titleAndDescription = null;
      if (string.IsNullOrEmpty(excelDll) || !File.Exists(excelDll))
      {
        throw new ExcellDynamicLibraryNotFoundException();
      }

      titleAndDescription = new string[2];
      Assembly excelAssembly = Assembly.LoadFile(excelDll);
      object[] assemblyAttributes = excelAssembly.GetCustomAttributes(typeof(AssemblyTitleAttribute), true);
      titleAndDescription[0] = (assemblyAttributes[0] as AssemblyTitleAttribute).Title;
      assemblyAttributes = excelAssembly.GetCustomAttributes(typeof(AssemblyDescriptionAttribute), true);
      titleAndDescription[1] = (assemblyAttributes[0] as AssemblyDescriptionAttribute).Description;
      return titleAndDescription;
    }

    /// <summary>
    /// Performs an installation or removal of the MySQL for Excel registry keys in the specified registry view.
    /// </summary>
    /// <param name="addInRegistryAction">The action to perform (install or remove).</param>
    /// <param name="addInRegistryView">The targeted registry view (32-bit or 64-bit).</param>
    /// <param name="session">The installer's session object.</param>
    /// <returns>The result of the custom action.</returns>
    private static ActionResult PerformActionOnRegistryView(AddInRegistryAction addInRegistryAction, AddInRegistryView addInRegistryView, Session session)
    {
      string actionText = addInRegistryAction.ToString();
      string viewText = addInRegistryView.ToString();
      session.Log(string.Format("Starting custom action for action {0} and registry view {1}.", actionText, viewText));

      try
      {
        RegistryView registryView = addInRegistryView == AddInRegistryView.Registry32 ? RegistryView.Registry32 : RegistryView.Registry64;
        RegistryKey baseKey = RegistryKey.OpenBaseKey(RegistryHive.LocalMachine, registryView);
        RegistryKey officeKey = baseKey.OpenSubKey(MS_OFFICE_REGISTRY_KEY_LOCATION, RegistryKeyPermissionCheck.ReadWriteSubTree, RegistryRights.FullControl);
        RegistryKey excelKey = null;

        switch (addInRegistryAction)
        {
          case AddInRegistryAction.Install:
            //// Retrieve the MySQL for Excel DLL and VSTO file install locations needed for the registry installation.
            string excelDll = session.CustomActionData["AddInDllLocation"];
            string vstoFile = session.CustomActionData["VstoFileLocation"];
            string[] excelDllTitleAndDesription = GetMySqlForExelDllTitleAndDescription(excelDll);
            ValidateVstoFile(vstoFile);

            //// If the files were found proceed to create the registry keys.
            excelKey = baseKey.CreateSubKey(MYSQL_FOR_EXCEL_REGISTRY_KEY_LOCATION);
            if (excelKey != null)
            {
              excelKey.SetValue("Description", excelDllTitleAndDesription[1], RegistryValueKind.String);
              excelKey.SetValue("FriendlyName", excelDllTitleAndDesription[0], RegistryValueKind.String);
              excelKey.SetValue("LoadBehavior", 3, RegistryValueKind.DWord);
              excelKey.SetValue("Manifest", vstoFile + "|vstolocal", RegistryValueKind.String);
              excelKey.Close();
            }
            break;

          case AddInRegistryAction.Remove:
            if (officeKey != null)
            {
              //// Remove the installed keys but also remove the container Excel and Addins keys if no more add-ins are present.
              excelKey = officeKey.OpenSubKey("Excel", true);
              if (excelKey != null)
              {
                RegistryKey addinsKey = excelKey.OpenSubKey("Addins", true);
                addinsKey.DeleteSubKeyTree("MySQL.ForExcel");
                if (addinsKey.SubKeyCount == 0 && addinsKey.ValueCount == 0)
                {
                  addinsKey.Close();
                  excelKey.DeleteSubKey("Addins");
                  if (excelKey.SubKeyCount == 0 && excelKey.ValueCount == 0)
                  {
                    excelKey.Close();
                    officeKey.DeleteSubKey("Excel");
                    officeKey.Close();
                  }
                }
              }
            }
            break;
        }

        session.Log(string.Format("Successfully performed action {0} for the add-in in the registry view: {1}.", actionText, viewText));
      }
      catch (Exception ex)
      {
        session.Log(ex.Message);
        return ActionResult.Failure;
      }

      return ActionResult.Success;
    }

    /// <summary>
    /// Validates that the MySQL for Excel VSTO file that will be mapped in the registry really exists.
    /// </summary>
    /// <param name="vstoFile">The MySQL for Excel VSTO file.</param>
    private static void ValidateVstoFile(string vstoFile)
    {
      if (string.IsNullOrEmpty(vstoFile) || !File.Exists(vstoFile))
      {
        throw new VstoDeploymentManifestNotFoundException();
      }
    }
  }
}