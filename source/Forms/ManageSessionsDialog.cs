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
  /// Shows the list of MySQL for Excel Edit and Import stored sessions in a list to cherry pick and delete the ones that are no longer needed.
  /// </summary>
  public partial class ManageSessionsDialog : AutoStyleableBaseDialog
  {
    /// <summary>
    /// Initializes a new instance of the <see cref="ManageSessionsDialog"/> class.
    /// </summary>
    public ManageSessionsDialog()
    {
      SessionsToDelete = new List<ISessionInfo>();
      InitializeComponent();
      LoadListViewWithStoredSessions();
    }

    /// <summary>
    /// List of the sessions to delete.
    /// </summary>
    public List<ISessionInfo> SessionsToDelete { get; private set; }

    /// <summary>
    /// Handles the Click event of the DeleteSelectedButton control.
    /// </summary>
    /// <param name="sender">The source of the event.</param>
    /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
    private void DeleteSelectedButton_Click(object sender, EventArgs e)
    {
      SessionsListView.Items.Cast<ListViewItem>().Where(item => item.Checked).ToList().ForEach(item => SessionsToDelete.Add(item.Tag as ISessionInfo));
    }

    /// <summary>
    /// Loads the ListView with stored sessions.
    /// </summary>
    private void LoadListViewWithStoredSessions()
    {
      var allSessions = new List<ISessionInfo>();
      allSessions.AddRange(Globals.ThisAddIn.StoredEditSessions);
      allSessions.AddRange(Globals.ThisAddIn.StoredImportSessions);
      SessionsListView.Groups.Clear();

      foreach (var session in allSessions)
      {
        var listViewItem = SessionsListView.Items.Add(session.GetHashCode().ToString(CultureInfo.InvariantCulture), String.Empty, 0);
        if (session.GetType() == typeof(ImportSessionInfo))
        {
          listViewItem.SubItems.Add("Import");
          listViewItem.SubItems.Add(((ImportSessionInfo)session).ExcelTableName);
        }
        else
        {
          listViewItem.SubItems.Add("Edit");
          listViewItem.SubItems.Add(((EditSessionInfo)session).SchemaName + "." + ((EditSessionInfo)session).TableName);
        }

        listViewItem.SubItems.Add(session.LastAccess.ToString(CultureInfo.InvariantCulture));
        listViewItem.Tag = session;

        //Get the list view group or create it.
        ListViewGroup listViewGroup = SessionsListView.Groups.Cast<ListViewGroup>().FirstOrDefault(g => g.Name == session.WorkbookGuid);
        if (listViewGroup == null)
        {
          listViewGroup = new ListViewGroup(session.WorkbookGuid, session.WorkbookFilePath);
          SessionsListView.Groups.Add(listViewGroup);
        }

        //Set the ListViewItem group
        listViewItem.Group = listViewGroup;

        //If the current session is from the active workbook, set its font in bold.
        var bold = new System.Drawing.Font(listViewItem.Font, listViewItem.Font.Style | FontStyle.Bold);
        var regular = new System.Drawing.Font(listViewItem.Font, listViewItem.Font.Style | FontStyle.Regular);
        listViewItem.Font = Globals.ThisAddIn.Application.ActiveWorkbook.GetOrCreateId() == session.WorkbookGuid ? bold : regular;

        //Set the session's font in red if the worbooks it belongs to is not found in the system.
        listViewItem.ForeColor = File.Exists(session.WorkbookFilePath) ? Color.Black : Color.Red;
      }

      SessionsListView.OwnerDraw = false;
    }

    /// <summary>
    /// Event delegate method fired when the <see cref="SelectNoneToolStripMenuItem"/> context menu item is clicked.
    /// </summary>
    /// <param name="sender">Sender object.</param>
    /// <param name="e">Event arguments.</param>
    private void SelectNoneToolStripMenuItem_Click(object sender, EventArgs e)
    {
      SessionsListView.Items.Cast<ListViewItem>().ToList().ForEach(item => item.Checked = false);
    }

    /// <summary>
    /// Event delegate method fired when the <see cref="SelectWorkbookSessionsToolStripMenuItem"/> context menu item is clicked.
    /// </summary>
    /// <param name="sender">Sender object.</param>
    /// <param name="e">Event arguments.</param>
    private void SelectWorkbookSessionsToolStripMenuItem_Click(object sender, EventArgs e)
    {
      var currentWorkbookGuid = ((ISessionInfo)SessionsListView.FocusedItem.Tag).WorkbookGuid;
      SessionsListView.Items.Cast<ListViewItem>().Where(item => ((ISessionInfo)item.Tag).WorkbookGuid == currentWorkbookGuid).ToList().ForEach(item => item.Checked = true);
    }
  }
}