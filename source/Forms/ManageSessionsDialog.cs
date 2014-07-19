// Copyright (c) 2014, Oracle and/or its affiliates. All rights reserved.
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
using System.Globalization;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using MySQL.ForExcel.Classes;
using MySQL.ForExcel.Interfaces;
using MySQL.Utility.Forms;
using System.Drawing;

namespace MySQL.ForExcel.Forms
{
  /// <summary>
  /// Shows the list of MySQL for Excel Edit and Import stored connection information in a list to cherry pick and delete the ones that are no longer needed.
  /// </summary>
  public partial class ManageConnectionInfosDialog : AutoStyleableBaseDialog
  {
    /// <summary>
    /// Initializes a new instance of the <see cref="ManageConnectionInfosDialog"/> class.
    /// </summary>
    public ManageConnectionInfosDialog()
    {
      ConnectionInfosToDelete = new List<IConnectionInfo>();
      InitializeComponent();
      LoadListViewWithStoredConnectionInfos();
    }

    /// <summary>
    /// List of the sessions to delete.
    /// </summary>
    public List<IConnectionInfo> ConnectionInfosToDelete { get; private set; }

    /// <summary>
    /// Handles the Click event of the DeleteSelectedButton control.
    /// </summary>
    /// <param name="sender">The source of the event.</param>
    /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
    private void DeleteSelectedButton_Click(object sender, EventArgs e)
    {
      ConnectionInfosListView.Items.Cast<ListViewItem>().Where(item => item.Checked).ToList().ForEach(item => ConnectionInfosToDelete.Add(item.Tag as IConnectionInfo));
    }

    /// <summary>
    /// Loads the ListView with stored sessions.
    /// </summary>
    private void LoadListViewWithStoredConnectionInfos()
    {
      var allConnectionInfos = new List<IConnectionInfo>();
      allConnectionInfos.AddRange(Globals.ThisAddIn.EditConnectionInfos);
      allConnectionInfos.AddRange(Globals.ThisAddIn.StoredImportConnectionInfos);
      ConnectionInfosListView.Groups.Clear();

      foreach (var connectionInfo in allConnectionInfos)
      {
        var listViewItem = ConnectionInfosListView.Items.Add(connectionInfo.GetHashCode().ToString(CultureInfo.InvariantCulture), String.Empty, 0);
        if (connectionInfo.GetType() == typeof(ImportConnectionInfo))
        {
          listViewItem.SubItems.Add("Import");
          listViewItem.SubItems.Add(((ImportConnectionInfo)connectionInfo).ExcelTableName);
        }
        else
        {
          listViewItem.SubItems.Add("Edit");
          listViewItem.SubItems.Add(((EditConnectionInfo)connectionInfo).SchemaName + "." + ((EditConnectionInfo)connectionInfo).TableName);
        }

        listViewItem.SubItems.Add(connectionInfo.LastAccess.ToString(CultureInfo.InvariantCulture));
        listViewItem.Tag = connectionInfo;

        //Get the list view group or create it.
        ListViewGroup listViewGroup = ConnectionInfosListView.Groups.Cast<ListViewGroup>().FirstOrDefault(g => g.Name == connectionInfo.WorkbookGuid);
        if (listViewGroup == null)
        {
          listViewGroup = new ListViewGroup(connectionInfo.WorkbookGuid, connectionInfo.WorkbookFilePath);
          ConnectionInfosListView.Groups.Add(listViewGroup);
        }

        //Set the ListViewItem group
        listViewItem.Group = listViewGroup;

        //If the current session is from the active workbook, set its font in bold.
        var bold = new System.Drawing.Font(listViewItem.Font, listViewItem.Font.Style | FontStyle.Bold);
        var regular = new System.Drawing.Font(listViewItem.Font, listViewItem.Font.Style | FontStyle.Regular);
        listViewItem.Font = Globals.ThisAddIn.Application.ActiveWorkbook.GetOrCreateId() == connectionInfo.WorkbookGuid ? bold : regular;

        //Set the session's font in red if the worbooks it belongs to is not found in the system.
        listViewItem.ForeColor = File.Exists(connectionInfo.WorkbookFilePath) ? Color.Black : Color.Red;
      }

      ConnectionInfosListView.OwnerDraw = false;
    }

    /// <summary>
    /// Event delegate method fired when the <see cref="SelectNoneToolStripMenuItem"/> context menu item is clicked.
    /// </summary>
    /// <param name="sender">Sender object.</param>
    /// <param name="e">Event arguments.</param>
    private void SelectNoneToolStripMenuItem_Click(object sender, EventArgs e)
    {
      ConnectionInfosListView.Items.Cast<ListViewItem>().ToList().ForEach(item => item.Checked = false);
    }

    /// <summary>
    /// Event delegate method fired when the <see cref="SelectWorkbookConnectionInfosToolStripMenuItem"/> context menu item is clicked.
    /// </summary>
    /// <param name="sender">Sender object.</param>
    /// <param name="e">Event arguments.</param>
    private void SelectWorkbookConnectionInfosToolStripMenuItem_Click(object sender, EventArgs e)
    {
      var currentWorkbookGuid = ((IConnectionInfo)ConnectionInfosListView.FocusedItem.Tag).WorkbookGuid;
      ConnectionInfosListView.Items.Cast<ListViewItem>().Where(item => ((IConnectionInfo)item.Tag).WorkbookGuid == currentWorkbookGuid).ToList().ForEach(item => item.Checked = true);
    }
  }
}