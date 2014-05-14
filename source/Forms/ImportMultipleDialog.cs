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
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Windows.Forms;
using MySQL.ForExcel.Classes;
using MySQL.ForExcel.Properties;
using MySQL.Utility.Classes;
using MySQL.Utility.Classes.MySQLWorkbench;
using MySQL.Utility.Forms;

namespace MySQL.ForExcel.Forms
{
  /// <summary>
  /// Previews the results of a procedure and lets users select rows to import to an Excel spreadsheet.
  /// </summary>
  public partial class ImportMultipleDialog : AutoStyleableBaseDialog
  {
    #region Fields

    /// <summary>
    /// The Table or View DB objects selected by users for import.
    /// </summary>
    private readonly List<DbObject> _importDbObjects;

    /// <summary>
    /// Flag indicating whether Excel relationships can and will be created.
    /// </summary>
    private readonly bool _importRelationships;

    /// <summary>
    /// The Table or View DB objects related to objects selected by the users.
    /// </summary>
    private readonly List<DbObject> _relatedDbObjects;

    /// <summary>
    /// The full Table or View DB objects list contained in the current selected schema.
    /// </summary>
    private readonly List<DbObject> _tableOrViewDbObjects;
    /// <summary>
    /// The connection to a MySQL server instance selected by users.
    /// </summary>
    private readonly MySqlWorkbenchConnection _wbConnection;

    /// <summary>
    /// Flag indicating whether the Excel workbook where data will be imported is open in compatibility mode.
    /// </summary>
    private readonly bool _workbookInCompatibilityMode;

    #endregion Fields

    /// <summary>
    /// Initializes a new instance of the <see cref="ImportProcedureForm"/> class.
    /// </summary>
    /// <param name="wbConnection">The connection to a MySQL server instance selected by users.</param>
    /// <param name="tableOrViewDbObjects">The full Table or View DB objects list contained in the current selected schema.</param>
    /// <param name="workbookInCompatibilityMode">Flag indicating whether the Excel workbook where data will be imported is open in compatibility mode.</param>
    public ImportMultipleDialog(MySqlWorkbenchConnection wbConnection, List<DbObject> tableOrViewDbObjects, bool workbookInCompatibilityMode)
    {
      if (wbConnection == null)
      {
        throw new ArgumentNullException("wbConnection");
      }

      if (tableOrViewDbObjects == null)
      {
        throw new ArgumentNullException("tableOrViewDbObjects");
      }

      _tableOrViewDbObjects = tableOrViewDbObjects;
      _importDbObjects = tableOrViewDbObjects.Where(dbObject => dbObject.Selected).ToList();
      _relatedDbObjects = new List<DbObject>(_importDbObjects.Count);
      ImportDataSet = null;
      _wbConnection = wbConnection;
      _workbookInCompatibilityMode = workbookInCompatibilityMode;
      bool excel2010OrLower = Globals.ThisAddIn.ExcelVersionNumber < ThisAddIn.EXCEL_2013_VERSION_NUMBER;
      _importRelationships = !excel2010OrLower && Settings.Default.ImportCreateExcelTable;

      InitializeComponent();

      TotalTablesViewsLabel.Text += _importDbObjects.Count;
      RelationshipsList = new List<MySqlDataRelationship>();

      // Set warnings.
      WorkbookInCompatibilityModeWarningLabel.Text = Resources.WorkbookInCompatibilityModeWarning;
      WorkbookInCompatibilityModeWarningLabel.Visible = workbookInCompatibilityMode;
      WorkbookInCompatibilityModeWarningPictureBox.Visible = workbookInCompatibilityMode;
      RelationshipsNotSupportedLabel.Text = excel2010OrLower
        ? Resources.ImportMultipleRelationshipsNotSupportedExcelVersionWarningText
        : Resources.ImportMultipleRelationshipsNotSupportedNoExcelTablesWarningText;
      RelationshipsNotSupportedPictureBox.Visible = !_importRelationships;
      RelationshipsNotSupportedLabel.Visible = !_importRelationships;

      ProcessSelectedDbObjects();
    }

    #region Properties

    /// <summary>
    /// Gets the <see cref="DataSet"/> containing tables with data from selected MySQL tables and views to be imported.
    /// </summary>
    [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    public DataSet ImportDataSet { get; private set; }

    /// <summary>
    /// A list of relationships for the imported tables and views.
    /// </summary>
    [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    public List<MySqlDataRelationship> RelationshipsList { get; private set; }

    /// <summary>
    /// Gets or sets the text associated with this control.
    /// </summary>
    public override sealed string Text
    {
      get
      {
        return base.Text;
      }

      set
      {
        base.Text = value;
      }
    }

    #endregion Properties

    /// <summary>
    /// Event delegate method fired when the <see cref="AdvancedOptionsButton"/> button is clicked.
    /// </summary>
    /// <param name="sender">Sender object.</param>
    /// <param name="e">Event arguments.</param>
    private void AdvancedOptionsButton_Click(object sender, EventArgs e)
    {
      using (var optionsDialog = new ImportAdvancedOptionsDialog())
      {
        optionsDialog.ShowDialog();
      }
    }

    /// <summary>
    /// Changes the checked status of all list view items on the related tables and views list.
    /// </summary>
    /// <param name="select">Flag indicating if the items are selected or not.</param>
    private void ChangeAllRelatedItemsSelection(bool select)
    {
      foreach (ListViewItem item in RelatedTablesViewsListView.Items)
      {
        item.Checked = select;
      }
    }

    /// <summary>
    /// Gets the owner <see cref="ListView"/> of a <see cref="ContextMenuStrip"/> control.
    /// </summary>
    /// <param name="toolStripMenuControl">An boxed object containing a <see cref="ContextMenuStrip"/> control.</param>
    /// <returns>The owner <see cref="ListView"/> of a <see cref="ContextMenuStrip"/> control.</returns>
    private ListView GetOwnerListViewControl(object toolStripMenuControl)
    {
      ContextMenuStrip ownerMenuStrip = null;
      if (toolStripMenuControl is ToolStripMenuItem)
      {
        var menuItem = toolStripMenuControl as ToolStripMenuItem;
        ownerMenuStrip = menuItem.Owner as ContextMenuStrip;
        if (ownerMenuStrip == null)
        {
          return null;
        }
      }
      else if (toolStripMenuControl is ContextMenuStrip)
      {
        ownerMenuStrip = toolStripMenuControl as ContextMenuStrip;
      }

      if (ownerMenuStrip == null)
      {
        return null;
      }

      var listView = ownerMenuStrip.SourceControl as ListView;
      return listView;
    }

    /// <summary>
    /// Event delegate method fired when the <see cref="ImportButton"/> button is clicked.
    /// </summary>
    /// <param name="sender">Sender object.</param>
    /// <param name="e">Event arguments.</param>
    private void ImportButton_Click(object sender, EventArgs e)
    {
      try
      {
        Cursor = Cursors.WaitCursor;
        ImportDataSet = new DataSet();
        var fullImportList = _importDbObjects.Concat(_relatedDbObjects);
        foreach (var importDbObject in fullImportList)
        {
          var mySqlTable = _wbConnection.CreateMySqlTable(false, importDbObject.Name, _workbookInCompatibilityMode, true);
          ImportDataSet.Tables.Add(mySqlTable);
        }
      }
      catch (Exception ex)
      {
        MiscUtilities.ShowCustomizedErrorDialog(Resources.ImportTableErrorTitle, ex.Message, true);
        ImportDataSet = null;
        MySqlSourceTrace.WriteAppErrorToLog(ex);
      }

      Cursor = Cursors.Default;
    }

    /// <summary>
    /// Event delegate method fired when the <see cref="ImportRelationshipsFromDbCheckBox"/> checked state changes.
    /// </summary>
    /// <param name="sender">Sender object.</param>
    /// <param name="e">Event arguments.</param>
    private void ImportRelationshipsFromDbCheckBox_CheckedChanged(object sender, EventArgs e)
    {
      RelatedTablesViewsListView.Enabled = ImportRelationshipsFromDbCheckBox.Checked;
      if (RelatedTablesViewsListView.Enabled)
      {
        return;
      }

      ChangeAllRelatedItemsSelection(false);
    }

    /// <summary>
    /// Event delegate method fired when the <see cref="PreviewDataToolStripMenuItem"/> context menu item is clicked.
    /// </summary>
    /// <param name="sender">Sender object.</param>
    /// <param name="e">Event arguments.</param>
    private void PreviewDataToolStripMenuItem_Click(object sender, EventArgs e)
    {
      var listView = GetOwnerListViewControl(sender);
      if (listView == null)
      {
        return;
      }

      if (listView.SelectedItems.Count < 0)
      {
        return;
      }

      var previewDbObject = listView.SelectedItems[0].Tag as DbObject;
      if (previewDbObject == null)
      {
        return;
      }

      using (var previewDialog = new PreviewTableViewDialog(_wbConnection, previewDbObject))
      {
        previewDialog.ShowDialog();
      }
    }

    /// <summary>
    /// Refreshes the list with tables and views.
    /// </summary>
    private void ProcessSelectedDbObjects()
    {
      foreach (var dbObject in _importDbObjects)
      {
        // Fill the selected tables and views list.
        var lvi = TablesViewsListView.Items.Add(dbObject.Name, dbObject.Name, dbObject.Type == DbObject.DbObjectType.Table ? 0 : 1);
        lvi.Tag = dbObject;

        // Get relationship for selected table or view.
        if (!_importRelationships)
        {
          continue;
        }

        var relationships = _wbConnection.GetRelationshipsFromForeignKeys(dbObject);
        if (relationships == null)
        {
          continue;
        }

        foreach (var relationship in relationships)
        {
          var relateDbObject = _tableOrViewDbObjects.FirstOrDefault(dbObj => dbObj.Name == relationship.RelatedTableOrViewName);
          if (relateDbObject == null)
          {
            continue;
          }

          relationship.Excluded = _importDbObjects.All(dbObj => dbObj.Name != relationship.RelatedTableOrViewName);
          RelationshipsList.Add(relationship);
          if (!relationship.Excluded)
          {
            continue;
          }

          // Fill the related tables and views list.
          if (!RelatedTablesViewsListView.Items.ContainsKey(relationship.RelatedTableOrViewName))
          {
            lvi = RelatedTablesViewsListView.Items.Add(relationship.RelatedTableOrViewName, relationship.RelatedTableOrViewName, relateDbObject.Type == DbObject.DbObjectType.Table ? 0 : 1);
            lvi.SubItems.Add(relationship.TableOrViewName);
            lvi.Tag = relateDbObject;
          }
          else if (lvi.SubItems.Count < 2 || !lvi.SubItems[1].Text.Split(',').Any(t => string.Equals(t.Trim(), relationship.TableOrViewName, StringComparison.InvariantCulture)))
          {
            lvi = RelatedTablesViewsListView.Items[relationship.RelatedTableOrViewName];
            lvi.SubItems[1].Text += @", " + relationship.TableOrViewName;
          }
        }
      }
    }

    /// <summary>
    /// Event delegate method fired when the <see cref="RelatedTablesViewsListView"/> gets an item checked.
    /// </summary>
    /// <param name="sender">Sender object.</param>
    /// <param name="e">Event arguments.</param>
    private void RelatedTablesViewsListView_ItemChecked(object sender, ItemCheckedEventArgs e)
    {
      var relatedDbObject = e.Item.Tag as DbObject;
      if (relatedDbObject == null)
      {
        return;
      }

      // Add or delete the checked or unchecked related DbObject to the related objects collection.
      bool dbObjectAlreadyOnList = _relatedDbObjects.Contains(relatedDbObject);
      if (e.Item.Checked && !dbObjectAlreadyOnList)
      {
        _relatedDbObjects.Add(relatedDbObject);
      }
      else if (!e.Item.Checked && dbObjectAlreadyOnList)
      {
        _relatedDbObjects.Remove(relatedDbObject);
      }

      // Flag the relation related to the checked or unchecked DbObject as Excluded.
      var relationship = RelationshipsList.FirstOrDefault(rel => rel.RelatedTableOrViewName == relatedDbObject.Name);
      if (relationship == null)
      {
        return;
      }

      relationship.Excluded = !e.Item.Checked;
    }

    /// <summary>
    /// Event delegate method fired when the <see cref="SelectAllToolStripMenuItem"/> context menu item is clicked.
    /// </summary>
    /// <param name="sender">Sender object.</param>
    /// <param name="e">Event arguments.</param>
    private void SelectAllToolStripMenuItem_Click(object sender, EventArgs e)
    {
      ChangeAllRelatedItemsSelection(true);
    }

    /// <summary>
    /// Event delegate method fired when the <see cref="SelectNoneToolStripMenuItem"/> context menu item is clicked.
    /// </summary>
    /// <param name="sender">Sender object.</param>
    /// <param name="e">Event arguments.</param>
    private void SelectNoneToolStripMenuItem_Click(object sender, EventArgs e)
    {
      ChangeAllRelatedItemsSelection(false);
    }

    /// <summary>
    /// Event delegate method fired when the <see cref="TablesViewsContextMenuStrip"/> context menu is being opened.
    /// </summary>
    /// <param name="sender">Sender object.</param>
    /// <param name="e">Event arguments.</param>
    private void TablesViewsContextMenuStrip_Opening(object sender, CancelEventArgs e)
    {
      var listView = GetOwnerListViewControl(sender);
      if (listView == null)
      {
        return;
      }

      switch (listView.Name)
      {
        case "TablesViewsListView":
          SelectAllToolStripMenuItem.Visible = false;
          SelectNoneToolStripMenuItem.Visible = false;
          break;

        case "RelatedTablesViewsListView":
          SelectAllToolStripMenuItem.Visible = true;
          SelectNoneToolStripMenuItem.Visible = true;
          break;
      }
    }
  }
}