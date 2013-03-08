//
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
//

namespace MySQL.ForExcel
{
  using System;
  using System.ComponentModel;
  using System.Data;
  using System.Drawing;
  using System.Windows.Forms;
  using MySQL.ForExcel.Properties;
  using MySQL.Utility;
  using Excel = Microsoft.Office.Interop.Excel;

  /// <summary>
  /// Third panel shown to users within the Add-In's <see cref="TaskPaneControl"/> where DB objects are managed.
  /// </summary>
  public partial class DBObjectSelectionPanel : AutoStyleableBasePanel
  {
    /// <summary>
    /// Flag indicating if the currently selected Excel range contains any data.
    /// </summary>
    private bool _excelSelectionContainsData;

    /// <summary>
    /// A <see cref="MySqlWorkbenchConnection"/> object representing the connection to a MySQL server instance selected by users.
    /// </summary>
    private MySqlWorkbenchConnection _wbConnection;

    /// <summary>
    /// Initializes a new instance of the <see cref="DBObjectSelectionPanel"/> class.
    /// </summary>
    public DBObjectSelectionPanel()
    {
      _excelSelectionContainsData = false;
      _wbConnection = null;
      Filter = string.Empty;
      InitializeComponent();

      InheritFontToControlsExceptionList.Add(ExportToNewTableHotLabel.Name);
      InheritFontToControlsExceptionList.Add(SelectDatabaseObjectHotLabel.Name);
      InheritFontToControlsExceptionList.Add(ImportDataHotLabel.Name);
      InheritFontToControlsExceptionList.Add(EditDataHotLabel.Name);
      InheritFontToControlsExceptionList.Add(AppendDataHotLabel.Name);

      DBObjectList.AddNode(null, "Tables");
      DBObjectList.AddNode(null, "Views");
      DBObjectList.AddNode(null, "Procedures");
    }

    #region Properties

    /// <summary>
    /// Gets the currently selected database object from the ones in the DB objects selection list.
    /// </summary>
    [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    public DBObject CurrentSelectedDBObject
    {
      get
      {
        if (DBObjectList.Nodes.Count > 0 && DBObjectList.SelectedNode != null && DBObjectList.SelectedNode.Level > 0)
        {
          return (DBObjectList.SelectedNode.Tag as DBObject);
        }
        else
        {
          return null;
        }
      }
    }

    /// <summary>
    /// Gets or sets a value indicating whether the currently selected Excel range contains any data.
    /// </summary>
    [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    public bool ExcelSelectionContainsData
    {
      get
      {
        return _excelSelectionContainsData;
      }

      set
      {
        _excelSelectionContainsData = value;
        ExportToNewTableHotLabel.Enabled = value;
        AppendDataHotLabel.Enabled = value && CurrentSelectedDBObject != null && CurrentSelectedDBObject.Type == DBObjectType.Table;
      }
    }

    /// <summary>
    /// Gets a string containing the filter to apply to the schemas list.
    /// </summary>
    [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    public string Filter { get; private set; }

    /// <summary>
    /// Gets a <see cref="MySqlWorkbenchConnection"/> object representing the connection to a MySQL server instance selected by users.
    /// </summary>
    [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    public MySqlWorkbenchConnection WBConnection
    { 
      get
      {
        return _wbConnection;
      }

      set
      {
        _wbConnection = value;
        ConnectionNameLabel.Text = _wbConnection.Name;
        UserIPLabel.Text = String.Format("User: {0}, IP: {1}", _wbConnection.UserName, _wbConnection.Host);
        RefreshDBObjectsList();
        DBObjectList_AfterSelect(null, null);
      }
    }

    #endregion Properties

    /// <summary>
    /// Refreshes the availability of action labels linked to the currently selected DB object.
    /// </summary>
    /// <param name="tableName">Name of the table with status update.</param>
    /// <param name="editActive">Flag indicating if the Edit Data action is enabled on the currently selected object.</param>
    public void RefreshActionLabelsEnabledStatus(string tableName, bool editActive)
    {
      DBObject dbObj = CurrentSelectedDBObject;
      if (dbObj == null || (!string.IsNullOrEmpty(tableName) && dbObj.Name != tableName))
      {
        return;
      }

      editActive = dbObj.Type == DBObjectType.Table && (Parent as TaskPaneControl).TableHasEditOnGoing(CurrentSelectedDBObject.Name);
      ImportDataHotLabel.Enabled = true;
      EditDataHotLabel.Enabled = dbObj.Type == DBObjectType.Table && !editActive;
      AppendDataHotLabel.Enabled = dbObj.Type == DBObjectType.Table && ExcelSelectionContainsData;
    }

    /// <summary>
    /// Event delegate method fired when <see cref="AppendDataHotLabel"/> is clicked.
    /// </summary>
    /// <param name="sender">Sender object.</param>
    /// <param name="e">Event arguments.</param>
    private void AppendDataHotLabel_Click(object sender, EventArgs e)
    {
      if (DBObjectList.SelectedNode == null)
      {
        return;
      }

      try
      {
        DBObject selDBObject = (DBObjectList.SelectedNode.Tag as DBObject);
        if (selDBObject.Type == DBObjectType.Table)
        {
          ExportDataToTable(selDBObject);
        }
      }
      catch (Exception ex)
      {
        InfoDialog errorDialog = new InfoDialog(false, ex.Message, null);
        errorDialog.WordWrapDetails = true;
        errorDialog.ShowDialog();
        MiscUtilities.WriteAppErrorToLog(ex);
      }
      finally
      {
        if (Cursor == Cursors.WaitCursor)
        {
          Cursor = Cursors.Default;
        }
      }
    }

    /// <summary>
    /// Event delegate method fired when <see cref="BackButton"/> is clicked.
    /// </summary>
    /// <param name="sender">Sender object.</param>
    /// <param name="e">Event arguments.</param>
    private void BackButton_Click(object sender, EventArgs e)
    {
      (Parent as TaskPaneControl).CloseSchema();
    }

    /// <summary>
    /// Event delegate method fired when <see cref="CloseButton"/> is clicked.
    /// </summary>
    /// <param name="sender">Sender object.</param>
    /// <param name="e">Event arguments.</param>
    private void CloseButton_Click(object sender, EventArgs e)
    {
      (Parent as TaskPaneControl).CloseAddIn();
    }

    /// <summary>
    /// Event delegate method fired after a node in the <see cref="DBObjectList"/> is selected.
    /// </summary>
    /// <param name="sender">Sender object.</param>
    /// <param name="e">Event arguments.</param>
    private void DBObjectList_AfterSelect(object sender, TreeViewEventArgs e)
    {
      RefreshActionLabelsEnabledStatus(null, false);
    }

    /// <summary>
    /// Event delegate method fired when a key is pressed within the <see cref="DBObjectsFilter"/> control.
    /// </summary>
    /// <param name="sender">Sender object.</param>
    /// <param name="e">Event arguments.</param>
    private void DBObjectsFilter_KeyDown(object sender, KeyEventArgs e)
    {
      if (e.KeyCode == Keys.Enter)
      {
        Filter = DBObjectsFilter.Text.Trim().ToUpper();
        try
        {
          RefreshDBObjectsList();
        }
        catch (Exception ex)
        {
          InfoDialog errorDialog = new InfoDialog(false, ex.Message, null);
          errorDialog.WordWrapDetails = true;
          errorDialog.ShowDialog();
          MiscUtilities.WriteAppErrorToLog(ex);
        }
      }
    }

    /// <summary>
    /// Event delegate method fired when <see cref="EditDataHotLabel"/> is clicked.
    /// </summary>
    /// <param name="sender">Sender object.</param>
    /// <param name="e">Event arguments.</param>
    private void EditDataHotLabel_Click(object sender, EventArgs e)
    {
      DBObject selDBObject = (DBObjectList.SelectedNode != null ? DBObjectList.SelectedNode.Tag as DBObject : null);
      if (selDBObject == null || selDBObject.Type != DBObjectType.Table)
      {
        return;
      }

      bool editActivated = (Parent as TaskPaneControl).EditTableData(selDBObject);
      if (editActivated)
      {
        EditDataHotLabel.Enabled = false;
      }
    }

    /// <summary>
    /// Exports currently selected Excel data to a new MySQL table or appends it to an existing MySQL table.
    /// </summary>
    /// <param name="appendToTable">Table to append the data to, if null exports to a new table.</param>
    /// <returns><see cref="true"/> if data was exported/appended successfully, <see cref="false"/> otherwise.</returns>
    private bool ExportDataToTable(DBObject appendToTable)
    {
      return (Parent as TaskPaneControl).AppendDataToTable(appendToTable);
    }

    /// <summary>
    /// Event delegate method fired when <see cref="ExportToNewTableHotLabel"/> is clicked.
    /// </summary>
    /// <param name="sender">Sender object.</param>
    /// <param name="e">Event arguments.</param>
    private void ExportToNewTableHotLabel_Click(object sender, EventArgs e)
    {
      bool success = ExportDataToTable(null);
      if (success)
      {
        DBObjectList.Nodes[0].Nodes.Clear();
        LoadDataObjects(DBObjectType.Table);
        DBObjectList_AfterSelect(DBObjectList, new TreeViewEventArgs(null));
      }
    }

    /// <summary>
    /// Event delegate method fired when <see cref="HelpButton"/> is clicked.
    /// </summary>
    /// <param name="sender">Sender object.</param>
    /// <param name="e">Event arguments.</param>
    private void HelpButton_Click(object sender, EventArgs e)
    {
      MessageBox.Show("Showing Help...");
    }

    /// <summary>
    /// Event delegate method fired when <see cref="ImportDataHotLabel"/> is clicked.
    /// </summary>
    /// <param name="sender">Sender object.</param>
    /// <param name="e">Event arguments.</param>
    private void ImportDataHotLabel_Click(object sender, EventArgs e)
    {
      if (DBObjectList.SelectedNode == null)
      {
        return;
      }

      TaskPaneControl parentTaskPane = (Parent as TaskPaneControl);
      if (parentTaskPane == null)
      {
        return;
      }

      DBObject dbo = DBObjectList.SelectedNode.Tag as DBObject;
      if (dbo == null)
      {
        return;
      }

      if (parentTaskPane.ActiveWorksheetInEditMode)
      {
        WarningDialog wDiag = new WarningDialog(Resources.WorkSheetInEditModeWarningTitle, Resources.WorkSheetInEditModeWarningDetail);
        DialogResult dr = wDiag.ShowDialog();
        if (dr != DialogResult.Yes)
        {
          return;
        }

        Excel.Worksheet newWorksheet = parentTaskPane.CreateNewWorksheet(dbo.Name, true);
        if (newWorksheet == null)
        {
          return;
        }
      }

      try
      {
        switch (dbo.Type)
        {
          case DBObjectType.Table:
          case DBObjectType.View:
            ImportTableOrView(dbo);
            break;

          case DBObjectType.Routine:
            ImportProcedure(dbo);
            break;
        }
      }
      catch (Exception ex)
      {
        InfoDialog errorDialog = new InfoDialog(false, Properties.Resources.ImportDataErrorTitle, ex.Message);
        errorDialog.WordWrapDetails = true;
        errorDialog.ShowDialog();
        MiscUtilities.WriteAppErrorToLog(ex);
      }
    }

    /// <summary>
    /// Imports data from the selected procedure DB object to the current Excel worksheet.
    /// </summary>
    /// <param name="dbo">DB object.</param>
    private void ImportProcedure(DBObject dbo)
    {
      ImportProcedureForm importProcedureForm = new ImportProcedureForm(WBConnection, dbo, (Parent as TaskPaneControl).ActiveWorksheet.Name, (Parent as TaskPaneControl).ActiveWorkbook.Excel8CompatibilityMode);
      DialogResult dr = importProcedureForm.ShowDialog();
      if (dr == DialogResult.Cancel)
      {
        return;
      }

      if (importProcedureForm.ImportDataSet == null)
      {
        InfoDialog errorDialog = new InfoDialog(false, String.Format(Resources.UnableToRetrieveData, dbo.Type.ToString().ToLowerInvariant(), dbo.Name), null);
        errorDialog.ShowDialog();
        return;
      }

      (Parent as TaskPaneControl).ImportDataToExcel(importProcedureForm.ImportDataSet, importProcedureForm.ImportHeaders, importProcedureForm.ImportType, importProcedureForm.SelectedResultSet);
    }

    /// <summary>
    /// Imports data from the selected table or view DB object to the current Excel worksheet.
    /// </summary>
    /// <param name="dbo">DB object.</param>
    private void ImportTableOrView(DBObject dbo)
    {
      var taskPaneControl = (TaskPaneControl)Parent;
      ImportTableViewForm importForm = new ImportTableViewForm(WBConnection, dbo, taskPaneControl.ActiveWorkbook.ActiveSheet.Name, taskPaneControl.ActiveWorkbook.Excel8CompatibilityMode, false);

      DialogResult dr = importForm.ShowDialog();
      if (dr == DialogResult.Cancel)
      {
        return;
      }

      if (importForm.ImportDataTable == null)
      {
        InfoDialog errorDialog = new InfoDialog(false, String.Format(Resources.UnableToRetrieveData, dbo.Type.ToString().ToLowerInvariant(), dbo.Name), null);
        errorDialog.ShowDialog();
        return;
      }

      (Parent as TaskPaneControl).ImportDataToExcel(importForm.ImportDataTable, importForm.ImportHeaders);
    }

    /// <summary>
    /// Event delegate method fired when a <see cref="Label"/> control is being painted.
    /// </summary>
    /// <param name="sender">A <see cref="Label"/> control object.</param>
    /// <param name="e">Event aruments.</param>
    private void Label_Paint(object sender, PaintEventArgs e)
    {
      Label label = sender as Label;

      //// Set a rectangle size with same width and larger height than label's
      SizeF layoutSize = new SizeF(label.Width, label.Height + 1);

      //// Get the actual size of rectangle needed for all of text.
      SizeF fullSize = e.Graphics.MeasureString(label.Text, label.Font, layoutSize);

      //// Set a tooltip if not all text fits in label's size.
      if (fullSize.Width > label.Width || fullSize.Height > label.Height)
      {
        LabelsToolTip.SetToolTip(label, label.Text);
      }
      else
      {
        LabelsToolTip.SetToolTip(label, null);
      }
    }

    /// <summary>
    /// Fetches all DB object names of the given type from the current connection and loads them in the <see cref="DBObjectsList"/> tree.
    /// </summary>
    /// <param name="dataObjectType">Type of DB object to load.</param>
    private void LoadDataObjects(DBObjectType dataObjectType)
    {
      DataTable objs = new DataTable();
      TreeNode parent = new TreeNode();

      string objectName;
      if (dataObjectType == DBObjectType.Routine)
      {
        objs = MySQLDataUtilities.GetSchemaCollection(WBConnection, "Procedures", null, WBConnection.Schema, null, "PROCEDURE");
        objectName = "ROUTINE_NAME";
        parent = DBObjectList.Nodes[2];
      }
      else
      {
        objs = MySQLDataUtilities.GetSchemaCollection(WBConnection, dataObjectType.ToString() + "s", null, WBConnection.Schema);
        objectName = "TABLE_NAME";
        parent = DBObjectList.Nodes[(int)dataObjectType];
      }

      if (objs == null)
      {
        return;
      }

      foreach (DataRow dataRow in objs.Rows)
      {
        string dataName = dataRow[objectName].ToString();

        //// Check our filter
        if (!String.IsNullOrEmpty(Filter) && !dataName.ToUpper().Contains(Filter))
        {
          continue;
        }

        string text = dataName;
        TreeNode node = DBObjectList.AddNode(parent, text);
        if (dataObjectType == DBObjectType.Routine)
        {
          node.Tag = new DBObject(dataName, dataObjectType,
            ((dataRow["ROUTINE_TYPE"].ToString()) == "PROCEDURE" ? RoutineType.Procedure : RoutineType.Function));
        }
        else
        {
          node.Tag = new DBObject(dataName, dataObjectType);
        }

        node.ImageIndex = (int)dataObjectType;
      }
    }
    /// <summary>
    /// Refreshes the DB objects list control with current objects in the connected schema.
    /// </summary>
    private void RefreshDBObjectsList()
    {
      foreach (TreeNode node in DBObjectList.Nodes)
      {
        node.Nodes.Clear();
      }

      LoadDataObjects(DBObjectType.Table);
      LoadDataObjects(DBObjectType.View);
      LoadDataObjects(DBObjectType.Routine);

      if (DBObjectList.Nodes[0].GetNodeCount(true) > 0)
      {
        DBObjectList.Nodes[0].Expand();
      }
    }

    /// <summary>
    /// Event delegate method fired when <see cref="RefreshDatabaseObjectsToolStripMenuItem"/> is clicked.
    /// </summary>
    /// <param name="sender">Sender object.</param>
    /// <param name="e">Event arguments.</param>
    private void RefreshDatabaseObjectsToolStripMenuItem_Click(object sender, EventArgs e)
    {
      try
      {
        RefreshDBObjectsList();
        DBObjectList_AfterSelect(null, null);
      }
      catch (Exception ex)
      {
        InfoDialog errorDialog = new InfoDialog(false, Resources.RefreshDBObjectsErrorTitle, ex.Message);
        errorDialog.WordWrapDetails = true;
        errorDialog.ShowDialog();
        MiscUtilities.WriteAppErrorToLog(ex);
      }
    }
  }
}