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
using System.Linq;
using System.Text;
using System.Windows.Forms;
using MySQL.ForExcel.Classes;
using MySQL.ForExcel.Properties;
using MySQL.Utility.Forms;
using ExcelInterop = Microsoft.Office.Interop.Excel;

namespace MySQL.ForExcel.Forms
{
  /// <summary>
  /// Previews the results of a procedure and lets users select rows to import to an Excel spreadsheet.
  /// </summary>
  public partial class ImportMultipleDialog : AutoStyleableBaseDialog
  {
    #region Fields

    /// <summary>
    /// The Tables or Views selected by users for import.
    /// </summary>
    private readonly List<DbView> _importTablesOrViews;

    /// <summary>
    /// Flag indicating whether Excel relationships can be created.
    /// </summary>
    private bool _importRelationshipsEnabled;

    /// <summary>
    /// The Tables related to objects selected by the users.
    /// </summary>
    private readonly List<DbView> _relatedTables;

    /// <summary>
    /// The full Table or View DB objects list contained in the current selected schema.
    /// </summary>
    private readonly List<DbView> _tableOrViews;

    #endregion Fields

    /// <summary>
    /// Initializes a new instance of the <see cref="ImportProcedureForm"/> class.
    /// </summary>
    /// <param name="tableOrViews">The full Table or View DB objects list contained in the current selected schema.</param>
    /// <param name="selectAllRelatedTables">Flag indicating whether all found related tables are selected by default.</param>
    public ImportMultipleDialog(List<DbView> tableOrViews, bool selectAllRelatedTables)
    {
      if (tableOrViews == null)
      {
        throw new ArgumentNullException("tableOrViews");
      }

      _tableOrViews = tableOrViews;
      _tableOrViews.ForEach(dbo => dbo.Excluded = false);
      _importTablesOrViews = _tableOrViews.Where(dbo => dbo.Selected).ToList();
      _relatedTables = new List<DbView>();

      InitializeComponent();

      SelectedTablesViewsLabel.Text += _importTablesOrViews.Count;
      RelatedTablesViewsLabel.Text += 0;
      SetWorkbookCompatibilityWarning();
      SetControlsEnabledState();
      ProcessSelectedTablesOrViews(selectAllRelatedTables);
    }

    #region Properties

    /// <summary>
    /// Gets a value indicating whether the Excel version is 2010 or lower.
    /// </summary>
    public static bool Excel2010OrLower
    {
      get
      {
        return Globals.ThisAddIn.ExcelVersionNumber < ThisAddIn.EXCEL_2013_VERSION_NUMBER;
      }
    }

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
        SetControlsEnabledState();
      }
    }

    /// <summary>
    /// Changes the checked status of all list view items on the related tables and views list.
    /// </summary>
    /// <param name="select">Flag indicating if the items are selected or not.</param>
    private void ChangeAllRelatedItemsSelection(bool select)
    {
      foreach (ListViewItem item in RelatedTablesListView.Items)
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
    /// Imports the selected MySQL tables data into new Excel worksheets.
    /// </summary>
    /// <returns><c>true</c> if the import is successful, <c>false</c> if errros were found during the import.</returns>
    private void ImportData()
    {
      Cursor = Cursors.WaitCursor;
      var relationshipsList = new List<MySqlDataRelationship>();

      // Import tables data in Excel worksheets
      var excelTablesDictionary = new Dictionary<string, ExcelInterop.ListObject>();
      var fullImportList = _importTablesOrViews.Concat(_relatedTables.Where(dbo => !dbo.Excluded)).ToList();
      foreach (var importTableOrView in fullImportList)
      {
        // Refresh import parameter values
        SetImportParameterValues(importTableOrView);

        // Import the table/view data into an Excel worksheet
        var importTuple = importTableOrView.ImportData();
        var excelTable = importTuple.Item2 as ExcelInterop.ListObject;
        var dbTable = importTableOrView as DbTable;
        if (excelTable == null || !_importRelationshipsEnabled || dbTable == null)
        {
          continue;
        }

        // Add relationships of the current importing
        excelTablesDictionary.Add(importTuple.Item1.TableName, excelTable);
        var importedTableNames = fullImportList.Where(dbo => dbo is DbTable).Select(dbo => dbo.Name).ToList();
        relationshipsList.AddRange(dbTable.Relationships.Where(rel => rel.ExistsAmongTablesInList(importedTableNames)));
      }

      // Create Excel relationships
      if (_importRelationshipsEnabled && relationshipsList.Count > 0)
      {
        var relationshipsCreationErrorBuilder = new StringBuilder(relationshipsList.Count * 200);
        foreach (var relationship in relationshipsList)
        {
          ExcelInterop.ListObject excelTable;
          ExcelInterop.ListObject relatedExcelTable;
          bool excelTableExists = excelTablesDictionary.TryGetValue(relationship.TableName, out excelTable);
          bool relatedExcelTableExists = excelTablesDictionary.TryGetValue(relationship.RelatedTableName, out relatedExcelTable);
          if (!excelTableExists || !relatedExcelTableExists)
          {
            if (relationshipsCreationErrorBuilder.Length > 0)
            {
              relationshipsCreationErrorBuilder.Append(Environment.NewLine);
            }

            relationshipsCreationErrorBuilder.Append(relationship.GetCreationErrorMessage(MySqlDataRelationship.CreationStatus.ModelTablesNotFound));
            continue;
          }

          var creationStatus = relationship.CreateExcelRelationship(excelTable.Name, relatedExcelTable.Name);
          if (creationStatus == MySqlDataRelationship.CreationStatus.Success)
          {
            continue;
          }

          if (relationshipsCreationErrorBuilder.Length > 0)
          {
            relationshipsCreationErrorBuilder.Append(Environment.NewLine);
          }

          relationshipsCreationErrorBuilder.Append(relationship.GetCreationErrorMessage(creationStatus));
        }

        if (relationshipsCreationErrorBuilder.Length > 0)
        {
          InfoDialog.ShowErrorDialog(Resources.ExcelRelationshipsCreationErrorTitle, Resources.ExcelRelationshipsCreationErrorDetail, null, relationshipsCreationErrorBuilder.ToString(), false);
          relationshipsCreationErrorBuilder.Clear();
        }

        excelTablesDictionary.Clear();
      }

      Cursor = Cursors.Default;
    }

    /// <summary>
    /// Event delegate method fired when the <see cref="ImportMultipleDialog"/> is closing.
    /// </summary>
    /// <param name="sender">Sender object.</param>
    /// <param name="e">Event arguments.</param>
    private void ImportMultipleDialog_FormClosing(object sender, FormClosingEventArgs e)
    {
      if (DialogResult == DialogResult.OK)
      {
        ImportData();
      }
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

      var previewTableOrView = listView.SelectedItems[0].Tag as DbView;
      if (previewTableOrView == null)
      {
        return;
      }

      using (var previewDialog = new PreviewTableViewDialog(previewTableOrView))
      {
        previewDialog.ShowDialog();
      }
    }

    /// <summary>
    /// Refreshes the list with tables and views.
    /// </summary>
    /// <param name="checkAllRelatedTables">Flag indicating whether all found related tables not in the original selection are checked by default.</param>
    private void ProcessSelectedTablesOrViews(bool checkAllRelatedTables)
    {
      var tablesInOriginalSelection = _importTablesOrViews.Where(dbo => dbo is DbTable).Select(dbo => dbo.Name).ToList();
      foreach (var tableOrView in _importTablesOrViews)
      {
        // Fill the selected tables and views list.
        var dbTable = tableOrView as DbTable;
        var lvi = TablesViewsListView.Items.Add(tableOrView.Name, tableOrView.Name, dbTable == null ? 1 : 0);
        lvi.SubItems.Add(string.Empty);
        lvi.Tag = tableOrView;
        if (dbTable == null)
        {
          continue;
        }

        // Get the related tables that are not in the original selection so we can fill the Related Tables list view.
        lvi.SubItems[1].Text = dbTable.RelatedObjectNames;
        var relationships = dbTable.Relationships.Where(rel => !tablesInOriginalSelection.Contains(rel.RelatedTableName)).ToList();
        if (relationships.Count == 0)
        {
          continue;
        }

        foreach (var relationship in relationships)
        {
          var relatedTable = _tableOrViews.FirstOrDefault(dbo => dbo.Name == relationship.RelatedTableName) as DbTable;
          if (relatedTable == null || _relatedTables.Contains(relatedTable))
          {
            continue;
          }

          // Fill the related tables and views list.
          _relatedTables.Add(relatedTable);
          lvi = RelatedTablesListView.Items.Add(relationship.RelatedTableName, relationship.RelatedTableName, 0);
          lvi.SubItems.Add(relatedTable.RelatedObjectNames);
          lvi.Tag = relatedTable;
          lvi.Checked = checkAllRelatedTables;
        }
      }
    }

    /// <summary>
    /// Event delegate method fired when the <see cref="RelatedTablesListView"/> gets an item checked.
    /// </summary>
    /// <param name="sender">Sender object.</param>
    /// <param name="e">Event arguments.</param>
    private void RelatedTablesViewsListView_ItemChecked(object sender, ItemCheckedEventArgs e)
    {
      RelatedTablesViewsLabel.Text = Resources.SelectedRelatedTablesAndViewsText + RelatedTablesListView.CheckedIndices.Count;
      var relatedDbObject = e.Item.Tag as DbObject;
      if (relatedDbObject == null)
      {
        return;
      }

      relatedDbObject.Excluded = !e.Item.Checked;
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
    /// Sets the enabled or disabled status of controls in the form.
    /// </summary>
    private void SetControlsEnabledState()
    {
      _importRelationshipsEnabled = !Excel2010OrLower && Settings.Default.ImportCreateExcelTable;
      AddSummaryFieldsCheckBox.Checked = Settings.Default.ImportCreateExcelTable && AddSummaryFieldsCheckBox.Checked;
      AddSummaryFieldsCheckBox.Enabled = Settings.Default.ImportCreateExcelTable;
      CreateExcelRelationshipsCheckBox.Checked = _importRelationshipsEnabled;
      CreateExcelRelationshipsCheckBox.Enabled = _importRelationshipsEnabled;
      WhyDisabledLinkLabel.Visible = !_importRelationshipsEnabled;
    }

    /// <summary>
    /// Sets the import parameter values into the given database object.
    /// This is needed before getting any data from it.
    /// </summary>
    private void SetImportParameterValues(DbView dbTableOrView)
    {
      dbTableOrView.ImportParameters.AddSummaryFields = AddSummaryFieldsCheckBox.Checked;
      dbTableOrView.ImportParameters.ColumnsNamesList = null;
      dbTableOrView.ImportParameters.CreatePivotTable = CreatePivotTableCheckBox.Checked;
      dbTableOrView.ImportParameters.FirstRowIndex = -1;
      dbTableOrView.ImportParameters.ForEditDataOperation = false;
      dbTableOrView.ImportParameters.IncludeColumnNames = true;
      dbTableOrView.ImportParameters.IntoNewWorksheet = true;
      dbTableOrView.ImportParameters.PivotTablePosition = MySqlDataTable.PivotTablePosition.Right;
      dbTableOrView.ImportParameters.RowsCount = -1;
    }

    /// <summary>
    /// Sets the warning about the active <see cref="ExcelInterop.Workbook"/> being in compatibility mode.
    /// </summary>
    private void SetWorkbookCompatibilityWarning()
    {
      bool workbookInCompatibilityMode = Globals.ThisAddIn.Application.ActiveWorkbook.Excel8CompatibilityMode;
      WorkbookInCompatibilityModeWarningLabel.Text = Resources.WorkbookInCompatibilityModeWarning;
      WorkbookInCompatibilityModeWarningLabel.Visible = workbookInCompatibilityMode;
      WorkbookInCompatibilityModeWarningPictureBox.Visible = workbookInCompatibilityMode;
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

    /// <summary>
    /// Event delegate method fired when the <see cref="WhyDisabledLinkLabel"/> is clicked.
    /// </summary>
    /// <param name="sender">Sender object.</param>
    /// <param name="e">Event arguments.</param>
    private void WhyDisabledLinkLabel_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
    {
      InfoDialog.ShowDialog(
        InfoDialog.DialogType.OkOnly,
        InfoDialog.InfoType.Info,
        Resources.ImportMultipleRelationshipsNotSupportedTitleText,
        Excel2010OrLower
          ? Resources.ImportMultipleRelationshipsNotSupportedExcelVersionWarningText
          : Resources.ImportMultipleRelationshipsNotSupportedNoExcelTablesWarningText);
    }
  }
}