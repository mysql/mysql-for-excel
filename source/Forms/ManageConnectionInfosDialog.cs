// Copyright (c) 2014, 2017, Oracle and/or its affiliates. All rights reserved.
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
using MySQL.ForExcel.Properties;
using MySql.Utility.Forms;
using System.Drawing;

namespace MySQL.ForExcel.Forms
{
  /// <summary>
  /// Shows the list of MySQL for Excel Edit and Import stored connection information in a list to cherry pick and delete the ones that are no longer needed.
  /// </summary>
  public partial class ManageConnectionInfosDialog : AutoStyleableBaseDialog
  {
    #region Fields

    /// <summary>
    /// A list of <see cref="IConnectionInfo"/> objects related to Excel Workbooks that no longer exist.
    /// </summary>
    private List<IConnectionInfo> _connectionInfosWithNonExistentWorkbook;

    #endregion Fields

    /// <summary>
    /// Initializes a new instance of the <see cref="ManageConnectionInfosDialog"/> class.
    /// </summary>
    public ManageConnectionInfosDialog()
    {
      _connectionInfosWithNonExistentWorkbook = GetConnectionInfosWithNonExistentWorkbook();

      InitializeComponent();

      ConnectionInfosToDelete = new List<IConnectionInfo>();
      RefreshControlValues();
      LoadListViewWithStoredConnectionInfos();
    }

    #region Properties

    /// <summary>
    /// Gets the number of days used to select <see cref="IConnectionInfo"/> objects with a last access older than those days.
    /// </summary>
    public int ConnectionInfosLastAccessDays { get; private set; }

    /// <summary>
    /// Gets a list of the connection information entries to delete.
    /// </summary>
    public List<IConnectionInfo> ConnectionInfosToDelete { get; private set; }

    /// <summary>
    /// Gets a value indicating whether <see cref="IConnectionInfo"/> objects should be automatically deleted if they are related to a Workbook that is no longer found.
    /// </summary>
    public bool DeleteAutomaticallyOrphanedConnectionInfos { get; private set; }

    #endregion Properties

    /// <summary>
    /// Gets a list of <see cref="IConnectionInfo"/> objects related to Excel Workbooks that no longer exist.
    /// </summary>
    /// <returns>A list of <see cref="IConnectionInfo"/> objects related to Excel Workbooks that no longer exist.</returns>
    public static List<IConnectionInfo> GetConnectionInfosWithNonExistentWorkbook()
    {
      var allConnectionInfos = new List<IConnectionInfo>();
      allConnectionInfos.AddRange(WorkbookConnectionInfos.UserSettingsEditConnectionInfos);
      allConnectionInfos.AddRange(WorkbookConnectionInfos.UserSettingsImportConnectionInfos);
      var orphanedConnectionInfos = allConnectionInfos.Where(connectionInfo => !File.Exists(connectionInfo.WorkbookFilePath)).ToList();
      return orphanedConnectionInfos;
    }

    /// <summary>
    /// Refreshes the dialog controls' values.
    /// </summary>
    /// <param name="useDefaultValues">Controls are set to their default values if <c>true</c>. Current stored values in application settings are used otherwise.</param>
    public void RefreshControlValues(bool useDefaultValues = false)
    {
      var settings = Settings.Default;
      if (useDefaultValues)
      {
        ConnectionInfosLastAccessDays = settings.GetPropertyDefaultValueByName<int>("ConnectionInfosLastAccessDays");
        DeleteAutomaticallyOrphanedConnectionInfos = settings.GetPropertyDefaultValueByName<bool>("DeleteAutomaticallyOrphanedConnectionInfos");
      }
      else
      {
        ConnectionInfosLastAccessDays = Settings.Default.ConnectionInfosLastAccessDays;
        DeleteAutomaticallyOrphanedConnectionInfos = Settings.Default.DeleteAutomaticallyOrphanedConnectionInfos;
      }
    }

    /// <summary>
    /// Loads the ListView with stored sessions.
    /// </summary>
    private void LoadListViewWithStoredConnectionInfos()
    {
      var currentWorkbookId = Globals.ThisAddIn.ActiveWorkbook.GetOrCreateId();
      var allConnectionInfos = new List<IConnectionInfo>();
      allConnectionInfos.AddRange(WorkbookConnectionInfos.UserSettingsEditConnectionInfos);
      allConnectionInfos.AddRange(WorkbookConnectionInfos.UserSettingsImportConnectionInfos);
      ConnectionInfosListView.Groups.Clear();

      foreach (var connectionInfo in allConnectionInfos)
      {
        var listViewItem = ConnectionInfosListView.Items.Add(connectionInfo.GetHashCode().ToString(CultureInfo.InvariantCulture), string.Empty, 0);
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
        var listViewGroup = ConnectionInfosListView.Groups.Cast<ListViewGroup>().FirstOrDefault(g => g.Name == connectionInfo.WorkbookGuid);
        if (listViewGroup == null)
        {
          listViewGroup = new ListViewGroup(connectionInfo.WorkbookGuid, connectionInfo.WorkbookFilePath);
          ConnectionInfosListView.Groups.Add(listViewGroup);
        }

        //Set the ListViewItem group
        listViewItem.Group = listViewGroup;

        //If the current session is from the active workbook, set its font in bold.
        listViewItem.Font = currentWorkbookId == connectionInfo.WorkbookGuid
          ? new Font(listViewItem.Font, listViewItem.Font.Style | FontStyle.Bold)
          : new Font(listViewItem.Font, listViewItem.Font.Style | FontStyle.Regular);

        //Set the session's font in red if the worbooks it belongs to is not found in the system.
        listViewItem.ForeColor = _connectionInfosWithNonExistentWorkbook.Any(ci => ci.Equals(connectionInfo)) ? Color.Red : Color.Black;
      }
    }

    /// <summary>
    /// Event delegate method fired when the <see cref="ManageConnectionInfosDialog"/> is being closed.
    /// </summary>
    /// <param name="sender">Sender object.</param>
    /// <param name="e">Event arguments.</param>
    private void ManageConnectionInfosDialog_FormClosing(object sender, FormClosingEventArgs e)
    {
      if (DialogResult == DialogResult.Cancel)
      {
        return;
      }

      ConnectionInfosToDelete.Clear();
      ConnectionInfosListView.Items.Cast<ListViewItem>().Where(item => item.Checked).ToList().ForEach(item => ConnectionInfosToDelete.Add(item.Tag as IConnectionInfo));
      ConnectionInfosLastAccessDays = (int) SelectConnectionInfosNumericUpDown.Value;
      DeleteAutomaticallyOrphanedConnectionInfos = DeleteOrphanedInfosCheckBox.Checked;
    }

    /// <summary>
    /// Event delegate method fired when the <see cref="ManageConnectionInfosDialog"/> is shown.
    /// </summary>
    /// <param name="sender">Sender object.</param>
    /// <param name="e">Event arguments.</param>
    private void ManageConnectionInfosDialog_Shown(object sender, EventArgs e)
    {
      UpdateControlValues();
    }

    /// <summary>
    /// Event delegate method fired when the <see cref="SelectAllToolStripMenuItem"/> context menu item is clicked.
    /// </summary>
    /// <param name="sender">Sender object.</param>
    /// <param name="e">Event arguments.</param>
    private void SelectAllToolStripMenuItem_Click(object sender, EventArgs e)
    {
      ConnectionInfosListView.Items.Cast<ListViewItem>().ToList().ForEach(item => item.Checked = true);
    }

    /// <summary>
    /// Event delegate method fired when the <see cref="SelectConnectionInfosLinkLabel"/> link label is clicked.
    /// </summary>
    /// <param name="sender">Sender object.</param>
    /// <param name="e">Event arguments.</param>
    private void SelectConnectionInfosLinkLabel_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
    {
      var accessLimitDate = DateTime.Today.Subtract(new TimeSpan(ConnectionInfosLastAccessDays, 0, 0, 0));
      foreach (var listViewItem in
                from ListViewItem listViewItem in ConnectionInfosListView.Items
                let connectionInfo = listViewItem.Tag as IConnectionInfo
                where connectionInfo != null
                where connectionInfo.LastAccess < accessLimitDate
                select listViewItem)
      {
        listViewItem.Checked = true;
      }
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

    /// <summary>
    /// Event delegate method fired when the <see cref="SelectWorkbookNotFoundToolStripMenuItem"/> context menu item is clicked.
    /// </summary>
    /// <param name="sender">Sender object.</param>
    /// <param name="e">Event arguments.</param>
    private void SelectWorkbookNotFoundToolStripMenuItem_Click(object sender, EventArgs e)
    {
      foreach (var listViewItem in
                from ListViewItem listViewItem in ConnectionInfosListView.Items
                let connectionInfo = listViewItem.Tag as IConnectionInfo
                where connectionInfo != null
                where _connectionInfosWithNonExistentWorkbook.Any(ci => ci.Equals(connectionInfo))
                select listViewItem)
      {
        listViewItem.Checked = true;
      }
    }

    /// <summary>
    /// Event delegate method fired when the <see cref="TablesViewsContextMenuStrip"/> context menu is being opened.
    /// </summary>
    /// <param name="sender">Sender object.</param>
    /// <param name="e">Event arguments.</param>
    private void TablesViewsContextMenuStrip_Opening(object sender, System.ComponentModel.CancelEventArgs e)
    {
      int checkedCount = ConnectionInfosListView.CheckedItems.Count;
      int itemsCount = ConnectionInfosListView.Items.Count;
      SelectAllToolStripMenuItem.Visible = checkedCount < itemsCount;
      SelectNoneToolStripMenuItem.Visible = checkedCount > 0;
    }

    /// <summary>
    /// Updates the control values based on the dialog saved properties.
    /// </summary>
    private void UpdateControlValues()
    {
      if (ConnectionInfosToDelete != null)
      {
        foreach (ListViewItem listViewItem in ConnectionInfosListView.Items)
        {
          var connectionInfo = listViewItem.Tag as IConnectionInfo;
          if (connectionInfo == null)
          {
            continue;
          }

          listViewItem.Checked = ConnectionInfosToDelete.Any(ci => ci.Equals(connectionInfo));
        }
      }

      SelectConnectionInfosNumericUpDown.Value = ConnectionInfosLastAccessDays;
      DeleteOrphanedInfosCheckBox.Checked = DeleteAutomaticallyOrphanedConnectionInfos;
    }
  }
}