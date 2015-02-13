// Copyright (c) 2014-2015, Oracle and/or its affiliates. All rights reserved.
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
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;
using MySQL.Utility.Classes;
using OfficeCore = Microsoft.Office.Core;

namespace MySQL.ForExcel.Controls
{
  /// <summary>
  /// Defines a custom ribbon control for MySQL for Excel.
  /// </summary>
  [ComVisible(true)]
  public class MySqlRibbon : OfficeCore.IRibbonExtensibility
  {
    /// <summary>
    /// A reference to the custom ribbon UI.
    /// </summary>
    private OfficeCore.IRibbonUI _ribbon;

    /// <summary>
    /// Initializes a new instance of the <see cref="MySqlRibbon"/> class.
    /// </summary>
    public MySqlRibbon()
    {
    }

    /// <summary>
    /// Gets or sets a value indicating whether the toggle button with id MySqlForExcelGroup is pressed.
    /// </summary>
    public bool ShowMySqlForExcelPaneTogglePressed { get; set; }

    #region IRibbonExtensibility Members

    /// <summary>
    /// Loads the XML markup, either from an XML customization file or from XML markup embedded in the procedure, that customizes the Ribbon user interface.
    /// </summary>
    /// <param name="ribbonId">The ID for the RibbonX UI. For Word, Excel, PowerPoint, and Access, there is only one ID for each application. For Outlook, there will be one for each type of Inspector object.</param>
    /// <returns>The resource text for the <see cref="MySqlRibbon"/> ribbon.</returns>
    public string GetCustomUI(string ribbonId)
    {
      return GetResourceText("MySQL.ForExcel.Controls.MySqlRibbon.xml");
    }

    #endregion

    #region Ribbon Callbacks
    // Create callback methods here. For more information about adding callback methods, visit http://go.microsoft.com/fwlink/?LinkID=271226

    /// <summary>
    /// Callback method specified within the buttonPressed attribute of a ribbon control declared in the Ribbon.xml.
    /// </summary>
    /// <param name="control">A ribbon control.</param>
    /// <returns><c>true</c>if the given control is pressed, <c>false</c> otherwise.</returns>
    public bool GetButtonPressed(OfficeCore.IRibbonControl control)
    {
      switch (control.Id)
      {
        case "ShowMySqlForExcelPane":
          return ShowMySqlForExcelPaneTogglePressed;
      }

      return false;
    }

    /// <summary>
    /// Callback method specified within the getImage attribute of a ribbon control declared in the Ribbon.xml.
    /// </summary>
    /// <param name="control">A ribbon control.</param>
    /// <returns>The image to be used for the given ribbon control.</returns>
    public object GetControlImage(OfficeCore.IRibbonControl control)
    {
      switch (control.Id)
      {
        case "ShowMySqlForExcelPane":
          return Properties.Resources.MySQLforExcel_Logo_48x48;
      }

      return null;
    }

    /// <summary>
    /// Callback method specified within the onAction attribute of a ribbon control declared in the Ribbon.xml.
    /// </summary>
    /// <param name="control">A ribbon control.</param>
    /// <param name="buttonPressed">Flag indicating whether the toggle button is depressed.</param>
    public void OnClickMySqlForExcel(OfficeCore.IRibbonControl control, bool buttonPressed)
    {
      ShowMySqlForExcelPaneTogglePressed = buttonPressed;
      Microsoft.Office.Tools.CustomTaskPane taskPane = Globals.ThisAddIn.GetOrCreateActiveCustomPane();
      if (taskPane == null)
      {
        MySqlSourceTrace.WriteToLog(string.Format("Could not get or create a custom task pane for the active Excel window. Using Excel version {0}.", Globals.ThisAddIn.ExcelVersionNumber));
        return;
      }

      taskPane.Visible = buttonPressed;
      if (!buttonPressed)
      {
        Globals.ThisAddIn.CloseExcelPane(taskPane.Control as ExcelAddInPane);
      }
    }

    /// <summary>
    /// Callback method specified within the onAction attribute of the native Refresh control.
    /// </summary>
    /// <param name="control">The ribbon control.</param>
    /// <param name="cancelDefault">Flag indicating whether the native functionality is cancelled, by default its value is <c>true</c>.</param>
    public void OnCustomRefresh(OfficeCore.IRibbonControl control, ref bool cancelDefault)
    {
      if (!Globals.ThisAddIn.RefreshDataCustomFunctionality())
      {
        // Set the cancelDefault to false if the active Excel object is not ListObject tied to MySQL data so the standard refresh functionality is automatically called.
        cancelDefault = false;
      }
    }

    /// <summary>
    /// Callback method specified within the onAction attribute of the native Refresh All control.
    /// </summary>
    /// <param name="control">The ribbon control.</param>
    /// <param name="cancelDefault">Flag indicating whether the native functionality is cancelled, by default its value is <c>true</c>.</param>
    public void OnCustomRefreshAll(OfficeCore.IRibbonControl control, ref bool cancelDefault)
    {
      Globals.ThisAddIn.RefreshAllCustomFunctionality();
    }

    /// <summary>
    /// Event delegate method fired when the <see cref="MySqlRibbon"/> is loaded.
    /// </summary>
    /// <param name="ribbonUi">A reference to the custom ribbon UI.</param>
    public void Ribbon_Load(OfficeCore.IRibbonUI ribbonUi)
    {
      _ribbon = ribbonUi;
    }

    #endregion

    #region Helpers

    /// <summary>
    /// Gets the text of a given resource within the assembly.
    /// </summary>
    /// <param name="resourceName">A resource name.</param>
    /// <returns>The text of a given resource within the assembly.</returns>
    private static string GetResourceText(string resourceName)
    {
      var asm = Assembly.GetExecutingAssembly();
      string[] resourceNames = asm.GetManifestResourceNames();
      foreach (string t in resourceNames)
      {
        if (string.Compare(resourceName, t, StringComparison.OrdinalIgnoreCase) != 0)
        {
          continue;
        }

        var resourceStream = asm.GetManifestResourceStream(t);
        if (resourceStream == null)
        {
          continue;
        }

        using (var resourceReader = new StreamReader(resourceStream))
        {
          return resourceReader.ReadToEnd();
        }
      }

      return null;
    }

    #endregion

    /// <summary>
    /// Changes the toggle state of the control with id ShowMySqlForExcelPane defined in the Ribbon.xml.
    /// </summary>
    /// <param name="pressed"></param>
    public void ChangeShowMySqlForExcelPaneToggleState(bool pressed)
    {
      ShowMySqlForExcelPaneTogglePressed = pressed;
      _ribbon.InvalidateControl("ShowMySqlForExcelPane");
    }
  }
}
