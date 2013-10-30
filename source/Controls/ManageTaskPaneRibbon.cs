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

using Microsoft.Office.Tools.Ribbon;
using MySQL.Utility.Classes;

namespace MySQL.ForExcel.Controls
{
  /// <summary>
  /// Attaches the add-in to a ribbon button.
  /// </summary>
  public partial class ManageTaskPaneRibbon
  {
    /// <summary>
    /// Event delegate method fired when the <see cref="ManageTaskPaneRibbon"/> is loaded.
    /// </summary>
    /// <param name="sender">Sender object.</param>
    /// <param name="e">Event arguments.</param>
    private void ManageTaskPaneRibbon_Load(object sender, RibbonUIEventArgs e)
    {
    }

    /// <summary>
    /// Event delegate method fired when the <see cref="ShowTaskPaneRibbonToggleButton"/> is clicked
    /// </summary>
    /// <param name="sender">Sender object.</param>
    /// <param name="e">Event arguments.</param>
    private void ShowTaskPaneRibbonToggleButton_Click(object sender, RibbonControlEventArgs e)
    {
      var ribbonToggleButton = sender as RibbonToggleButton;
      bool showAddIn = ribbonToggleButton != null && ribbonToggleButton.Checked;
      Microsoft.Office.Tools.CustomTaskPane taskPane = Globals.ThisAddIn.GetOrCreateActiveCustomPane();
      if (taskPane == null)
      {
        MySqlSourceTrace.WriteToLog(string.Format("Could not get or create a custom task pane for the active Excel window. Using Excel version {0}.", Globals.ThisAddIn.ExcelVersionNumber));
        return;
      }

      taskPane.Visible = showAddIn;
      if (!showAddIn)
      {
        Globals.ThisAddIn.CloseExcelPane(taskPane.Control as ExcelAddInPane);
      }
    }
  }
}