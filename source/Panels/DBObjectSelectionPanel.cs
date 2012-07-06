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

namespace MySQL.ForExcel
{
  public partial class DBObjectSelectionPanel : UserControl
  {
    private MySqlWorkbenchConnection connection;
    private string filter;

    public DBObjectSelectionPanel()
    {
      InitializeComponent();
      objectList.AddNode(null, "Tables");
      objectList.AddNode(null, "Views");
      objectList.AddNode(null, "Procedures");
    }

    public bool ExportDataActionEnabled
    {
      set { exportToNewTableLabel.Enabled = value; }
      get { return exportToNewTableLabel.Enabled; }
    }

    public void SetConnection(MySqlWorkbenchConnection connection)
    {
      this.connection = connection;
      lblConnectionName.Text = connection.Name;
      lblUserIP.Text = String.Format("User: {0}, IP: {1}", connection.UserName, connection.Host);
      PopulateList();
      objectList_AfterSelect(null, null);
    }

    private void PopulateList()
    {
      foreach (TreeNode node in objectList.Nodes)
        node.Nodes.Clear();

      LoadTables();
      LoadViews();
      LoadProcedures();

      if (objectList.Nodes[0].GetNodeCount(true) > 0)
        objectList.Nodes[0].Expand();
    }

    private void LoadTables()
    {
      DataTable tables = Utilities.GetSchemaCollection(connection, "Tables", null, connection.Schema);

      TreeNode parent = objectList.Nodes[0];
      foreach (DataRow tableRow in tables.Rows)
      {
        string tableName = tableRow["TABLE_NAME"].ToString();

        // check our filter
        if (!String.IsNullOrEmpty(filter) && String.Compare(filter, tableName, true) != 0) continue;

        string text = tableName;

        TreeNode node = objectList.AddNode(parent, text);
        node.Tag = new DBObject(tableName, DBObjectType.Table);
        node.ImageIndex = 0;
      }
    }

    private void LoadViews()
    {
      DataTable views = Utilities.GetSchemaCollection(connection, "Views", null, connection.Schema);
      if (views == null) return;

      TreeNode parent = objectList.Nodes[1];
      foreach (DataRow viewRow in views.Rows)
      {
        string viewName = viewRow["TABLE_NAME"].ToString();

        // check our filter
        if (!String.IsNullOrEmpty(filter) && String.Compare(filter, viewName, true) != 0) continue;

        string text = viewName;

        TreeNode node = objectList.AddNode(parent, text);
        node.Tag = new DBObject(viewName, DBObjectType.View);
        node.ImageIndex = 1;
      }
    }

    private void LoadProcedures()
    {
      DataTable procs = Utilities.GetSchemaCollection(connection, "Procedures", null, connection.Schema, null, "PROCEDURE");
      if (procs == null) return;

      TreeNode parent = objectList.Nodes[2];
      foreach (DataRow procedureRow in procs.Rows)
      {
        string procName = procedureRow["ROUTINE_NAME"].ToString();

        // check our filter
        if (!String.IsNullOrEmpty(filter) && String.Compare(filter, procName, true) != 0) continue;

        string type = procedureRow["ROUTINE_TYPE"].ToString();
        string text = procName;

        TreeNode node = objectList.AddNode(parent, text);
        node.Tag = new DBObject(procName, DBObjectType.Routine, (type == "PROCEDURE" ? RoutineType.Procedure : RoutineType.Function));
        node.ImageIndex = 2;
      }
    }

    private void objectList_AfterSelect(object sender, TreeViewEventArgs e)
    {
      DBObject o = null;
      if (e != null && e.Node != null && e.Node.Level > 0)
        o = e.Node.Tag as DBObject;

      importDataLabel.Enabled = o != null;
      editDataLabel.Enabled = o != null;
      appendDataLabel.Enabled = o != null && o.Type == DBObjectType.Table;
    }

    private void importData_Click(object sender, EventArgs e)
    {
      if (objectList.SelectedNode == null)
        return;

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

    private void importTableOrView(DBObject dbo)
    {
      ImportTableViewForm importForm = new ImportTableViewForm(connection, dbo);
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
      ImportProcedureForm importProcedureForm = new ImportProcedureForm(connection, dbo, (Parent as TaskPaneControl).ActiveWorksheet);
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
      DBObject selDBObject = (objectList.SelectedNode.Tag as DBObject);
      if (selDBObject.Type == DBObjectType.Table)
        exportDataToTable(selDBObject);
    }

    private void exportToNewTable_Click(object sender, EventArgs e)
    {
      bool success = exportDataToTable(null);
      if (success)
      {
        objectList.Nodes[0].Nodes.Clear();
        LoadTables();
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
        filter = objectFilter.Text.Trim();
        PopulateList();
      }
    }

  }

}
