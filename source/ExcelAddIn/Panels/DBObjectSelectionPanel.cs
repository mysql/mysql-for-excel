using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using MySQL.Utility;
using MySQL.ExcelAddIn.Properties;

namespace MySQL.ExcelAddIn
{
  public partial class DBObjectSelectionPanel : UserControl
  {
    private MySqlWorkbenchConnection connection;

    public DBObjectSelectionPanel()
    {
      InitializeComponent();
    }

    public bool ExportDataActionEnabled
    {
      set { exportToNewTable.Enabled = value; }
      get { return exportToNewTable.Enabled; }
    }

    public void SetConnection(MySqlWorkbenchConnection connection)
    {
      this.connection = connection;
      lblConnectionName.Text = connection.Name;
      lblUserIP.Text = String.Format("User: {0}, IP: {1}", connection.UserName, connection.Host);
      PopulateList();
    }

    private void PopulateList()
    {
      foreach (TreeNode node in objectList.Nodes)
        node.Nodes.Clear();

      LoadTables();
      LoadViews();
      LoadRoutines();
    }

    private void LoadTables()
    {
      DataTable tables = Utilities.GetSchemaCollection(connection, "Tables", null, connection.Schema);

      TreeNode parent = objectList.Nodes[0];
      foreach (DataRow tableRow in tables.Rows)
      {
        string tableName = tableRow["TABLE_NAME"].ToString();
        string text = String.Format("{0}|{1}", tableName, String.Format("Engine: {0}", tableRow["ENGINE"].ToString()));

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
        string text = String.Format("{0}|{1}", viewName, String.Format("Updatable: {0}", viewRow["IS_UPDATABLE"].ToString()));

        TreeNode node = objectList.AddNode(parent, text);
        node.Tag = new DBObject(viewName, DBObjectType.View);
        node.ImageIndex = 1;
      }
    }

    private void LoadRoutines()
    {
      DataTable procs = Utilities.GetSchemaCollection(connection, "Procedures", null, connection.Schema);
      if (procs == null) return;

      TreeNode parent = objectList.Nodes[2];
      foreach (DataRow routineRow in procs.Rows)
      {
        string procName = routineRow["ROUTINE_NAME"].ToString();
        string type = routineRow["ROUTINE_TYPE"].ToString();
        string text = String.Format("{0}|{1}", procName, String.Format("Type: {0}", type));

        TreeNode node = objectList.AddNode(parent, text);
        node.Tag = new DBObject(procName, DBObjectType.Routine, (type == "PROCEDURE" ? RoutineType.Procedure : RoutineType.Function));
        node.ImageIndex = 2;
      }
    }

    private bool exportDataToTable(string appendToTableName)
    {
      (Parent as TaskPaneControl).AppendDataToTable(appendToTableName);
      return true;
    }

    private void objectList_AfterSelect(object sender, TreeViewEventArgs e)
    {
      DBObject o = null;
      if (e.Node != null && e.Node.Level > 0)
        o = e.Node.Tag as DBObject;

      importData.Enabled = o != null;
      editData.Enabled = o != null;
      appendData.Enabled = o != null && o.Type == DBObjectType.Table;
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
          importRoutine(dbo);
          break;
      }
    }

    private void importTableOrView(DBObject dbo)
    {
      ImportTableViewDialog importDialog = new ImportTableViewDialog(connection, dbo);
      DialogResult dr = importDialog.ShowDialog();
      if (dr == DialogResult.Cancel)
        return;
      if (importDialog.ImportDataTable == null)
      {
        string msg = String.Format(Resources.UnableToRetrieveData, dbo.Name);
        MessageBox.Show(msg, Resources.Error, MessageBoxButtons.OK, MessageBoxIcon.Error);
        return;
      }
      (Parent as TaskPaneControl).ImportDataToExcel(importDialog.ImportDataTable, importDialog.ImportHeaders);
    }

    private void importRoutine(DBObject dbo)
    {
      ImportRoutineDialog importDialog = new ImportRoutineDialog(connection, dbo);
      DialogResult dr = importDialog.ShowDialog();
      if (dr == DialogResult.Cancel)
        return;
      if (importDialog.ImportDataSet == null)
      {
        string msg = String.Format(Resources.UnableToRetrieveData, dbo.Name);
        MessageBox.Show(msg, Resources.Error, MessageBoxButtons.OK, MessageBoxIcon.Error);
        return;
      }
      (Parent as TaskPaneControl).ImportDataToExcel(importDialog.ImportDataSet, importDialog.ImportHeaders, importDialog.ImportType);
    }

    private void editData_Click(object sender, EventArgs e)
    {
      MessageBox.Show("Editing Data...");
    }

    private void appendData_Click(object sender, EventArgs e)
    {
      if ((objectList.SelectedNode.Tag as DBObject).Type == DBObjectType.Table)
        exportDataToTable(objectList.SelectedNode.Name);
    }


    private void exportToNewTable_Click(object sender, EventArgs e)
    {
      bool success = exportDataToTable(String.Empty);
      if (success)
        LoadTables();
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
      (Parent as TaskPaneControl).CloseConnection();
    }

  }

}
