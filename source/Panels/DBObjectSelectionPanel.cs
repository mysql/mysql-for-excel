// 
// Copyright (c) 2012, Oracle and/or its affiliates. All rights reserved.
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

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using MySQL.Utility;
using MySQL.ForExcel.Properties;
using Excel = Microsoft.Office.Interop.Excel;

namespace MySQL.ForExcel
{
  public partial class DBObjectSelectionPanel : AutoStyleableBasePanel
  {
    private MySqlWorkbenchConnection connection;
    private string filter;
    private bool currentExcelSelectionContainsData = false;

    public DBObject CurrentSelectedDBObject
    {
      get
      {
        if (objectList.Nodes.Count > 0 && objectList.SelectedNode != null && objectList.SelectedNode.Level > 0)
          return (objectList.SelectedNode.Tag as DBObject);
        else
          return null;
      }
    }

    public DBObjectSelectionPanel()
    {
      InitializeComponent();

      InheritFontToControlsExceptionList.Add("exportToNewTableLabel");
      InheritFontToControlsExceptionList.Add("selectDatabaseObjectLabel");
      InheritFontToControlsExceptionList.Add("importDataLabel");
      InheritFontToControlsExceptionList.Add("editDataLabel");
      InheritFontToControlsExceptionList.Add("appendDataLabel");

      objectList.AddNode(null, "Tables");
      objectList.AddNode(null, "Views");
      objectList.AddNode(null, "Procedures");
    }

    public bool ExcelSelectionContainsData
    {
      set
      {
        currentExcelSelectionContainsData = value;
        exportToNewTableLabel.Enabled = value;
        appendDataLabel.Enabled = value && CurrentSelectedDBObject != null && CurrentSelectedDBObject.Type == DBObjectType.Table;
      }
    }

    public void SetConnection(MySqlWorkbenchConnection connection)
    {
      this.connection = connection;
      lblConnectionName.Text = connection.Name;
      lblUserIP.Text = String.Format("User: {0}, IP: {1}", connection.UserName, connection.Host);
      PopulateList();
      objectList_AfterSelect(null, null);
    }

    private void refreshDatabaseObjectsToolStripMenuItem_Click(object sender, EventArgs e)
    {
      try
      {
        PopulateList();
        objectList_AfterSelect(null, null);
      }
      catch (Exception ex)
      {
        InfoDialog dialog = new InfoDialog(false, ex.Message, null);
        dialog.ShowDialog();
      }
    }

    private void PopulateList()
    {
      foreach (TreeNode node in objectList.Nodes)
        node.Nodes.Clear();

      LoadDataObjects(DBObjectType.Table);
      LoadDataObjects(DBObjectType.View);
      LoadDataObjects(DBObjectType.Routine);

      if (objectList.Nodes[0].GetNodeCount(true) > 0)
        objectList.Nodes[0].Expand();
    }

    private void LoadDataObjects(DBObjectType dataObjectType)
    {
      DataTable objs = new DataTable();
      TreeNode parent = new TreeNode();

      string objectName;
      if (dataObjectType == DBObjectType.Routine)
      {
        objs = MySQLDataUtilities.GetSchemaCollection(connection, "Procedures", null, connection.Schema, null, "PROCEDURE");
        objectName = "ROUTINE_NAME";
        parent = objectList.Nodes[2];
      }
      else
      {
        objs = MySQLDataUtilities.GetSchemaCollection(connection, dataObjectType.ToString() + "s", null, connection.Schema);
        objectName = "TABLE_NAME";
        parent = objectList.Nodes[(int)dataObjectType];
      }

      if (objs == null) return;

      foreach (DataRow dataRow in objs.Rows)
      {
        string dataName = dataRow[objectName].ToString();

        // check our filter
        if (!String.IsNullOrEmpty(filter) && !dataName.ToUpper().Contains(filter)) continue;

        string text = dataName;

        TreeNode node = objectList.AddNode(parent, text);
        if (dataObjectType == DBObjectType.Routine)
          node.Tag = new DBObject(dataName, dataObjectType,
            ((dataRow["ROUTINE_TYPE"].ToString()) == "PROCEDURE" ? RoutineType.Procedure : RoutineType.Function));
        else
          node.Tag = new DBObject(dataName, dataObjectType);
        node.ImageIndex = (int)dataObjectType;
      }
    }

    private void objectList_AfterSelect(object sender, TreeViewEventArgs e)
    {
      DBObject obj = CurrentSelectedDBObject;

      importDataLabel.Enabled = obj != null;
      editDataLabel.Enabled = obj != null && obj.Type == DBObjectType.Table;
      appendDataLabel.Enabled = obj != null && obj.Type == DBObjectType.Table && currentExcelSelectionContainsData;
    }

    private void importData_Click(object sender, EventArgs e)
    {
      if (objectList.SelectedNode == null)
        return;

      try
      {
        DBObject dbo = objectList.SelectedNode.Tag as DBObject;
        switch (dbo.Type)
        {
          case DBObjectType.Table:
          case DBObjectType.View:
            importTableOrView(dbo);
            break;
          case DBObjectType.Routine:
            importProcedure(dbo);
            break;
        }
      }
      catch (Exception ex)
      {
        InfoDialog dialg = new InfoDialog(false, ex.Message, null);
        dialg.ShowDialog();
      }
    }

    private void importTableOrView(DBObject dbo)
    {
      ImportTableViewForm importForm = new ImportTableViewForm(connection, dbo, (Parent as TaskPaneControl).ActiveWorksheet.Name, (Parent as TaskPaneControl).ActiveWorkbook.Excel8CompatibilityMode);
      DialogResult dr = importForm.ShowDialog();
      if (dr == DialogResult.Cancel)
        return;
      if (importForm.ImportDataTable == null)
      {
        string msg = String.Format(Resources.UnableToRetrieveData, dbo.Name);
        MessageBox.Show(msg, Resources.Error, MessageBoxButtons.OK, MessageBoxIcon.Error);
        return;
      }
      (Parent as TaskPaneControl).ImportDataToExcel(importForm.ImportDataTable, importForm.ImportHeaders);
    }

    private void importProcedure(DBObject dbo)
    {
      ImportProcedureForm importProcedureForm = new ImportProcedureForm(connection, dbo, (Parent as TaskPaneControl).ActiveWorksheet.Name, (Parent as TaskPaneControl).ActiveWorkbook.Excel8CompatibilityMode);
      DialogResult dr = importProcedureForm.ShowDialog();
      if (dr == DialogResult.Cancel)
        return;
      if (importProcedureForm.ImportDataSet == null)
      {
        string msg = String.Format(Resources.UnableToRetrieveData, dbo.Name);
        MessageBox.Show(msg, Resources.Error, MessageBoxButtons.OK, MessageBoxIcon.Error);
        return;
      }
      (Parent as TaskPaneControl).ImportDataToExcel(importProcedureForm.ImportDataSet, importProcedureForm.ImportHeaders, importProcedureForm.ImportType, importProcedureForm.SelectedResultSet);
    }

    private bool exportDataToTable(DBObject appendToTable)
    {
      return (Parent as TaskPaneControl).AppendDataToTable(appendToTable);
    }

    private void appendData_Click(object sender, EventArgs e)
    {
      if (objectList.SelectedNode == null)
        return;
      try
      {
        DBObject selDBObject = (objectList.SelectedNode.Tag as DBObject);
        if (selDBObject.Type == DBObjectType.Table)
          exportDataToTable(selDBObject);
      }
      catch (Exception ex)
      {
        InfoDialog dialog = new InfoDialog(false, ex.Message, null);
        dialog.ShowDialog();
      }
    }

    private void exportToNewTable_Click(object sender, EventArgs e)
    {
      bool success = exportDataToTable(null);
      if (success)
      {
        objectList.Nodes[0].Nodes.Clear();
        LoadDataObjects(DBObjectType.Table);
        objectList_AfterSelect(objectList, new TreeViewEventArgs(null));
      }
    }

    private void editData_Click(object sender, EventArgs e)
    {
      DBObject selDBObject = (objectList.SelectedNode != null ? objectList.SelectedNode.Tag as DBObject : null);
      if (selDBObject == null || selDBObject.Type != DBObjectType.Table)
        return;
      (Parent as TaskPaneControl).EditTableData(selDBObject);
    }

    private void btnHelp_Click(object sender, EventArgs e)
    {
      MessageBox.Show("Showing Help...");
    }

    private void btnBack_Click(object sender, EventArgs e)
    {
      (Parent as TaskPaneControl).CloseSchema();
    }

    private void btnClose_Click(object sender, EventArgs e)
    {
      (Parent as TaskPaneControl).CloseAddIn();
    }

    private void objectFilter_KeyDown(object sender, KeyEventArgs e)
    {
      if (e.KeyCode == Keys.Enter)
      {
        filter = objectFilter.Text.Trim().ToUpper();
        try
        {
          PopulateList();
        }
        catch (Exception ex)
        {
          InfoDialog dialog = new InfoDialog(false, ex.Message, null);
          dialog.ShowDialog();
        }
      }
    }

    private void label_Paint(object sender, PaintEventArgs e)
    {
      Label label = sender as Label;
      // Set a rectangle size with same width and larger height than label's
      SizeF layoutSize = new SizeF(label.Width, label.Height + 1);
      // Get the actual size of rectangle needed for all of text.
      SizeF fullSize = e.Graphics.MeasureString(label.Text, label.Font, layoutSize);
      // Set a tooltip if not all text fits in label's size.
      if (fullSize.Width > label.Width || fullSize.Height > label.Height)
      {
        labelsToolTip.SetToolTip(label, label.Text);
      }
      else
      {
        labelsToolTip.SetToolTip(label, null);
      }
    }

  }

}
