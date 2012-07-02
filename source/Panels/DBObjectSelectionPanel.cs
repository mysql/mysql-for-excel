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
      LoadRoutines();

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

        // check our filter
        if (!String.IsNullOrEmpty(filter) && String.Compare(filter, viewName, true) != 0) continue;

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

        // check our filter
        if (!String.IsNullOrEmpty(filter) && String.Compare(filter, procName, true) != 0) continue;

        string type = routineRow["ROUTINE_TYPE"].ToString();
        string text = String.Format("{0}|{1}", procName, String.Format("Type: {0}", type));

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
          importRoutine(dbo);
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

    private void importRoutine(DBObject dbo)
    {
      ImportRoutineForm importRoutineForm = new ImportRoutineForm(connection, dbo);
      DialogResult dr = importRoutineForm.ShowDialog();
      if (dr == DialogResult.Cancel)
        return;
      if (importRoutineForm.ImportDataSet == null)
      {
        string msg = String.Format(Resources.UnableToRetrieveData, dbo.Name);
        MessageBox.Show(msg, Resources.Error, MessageBoxButtons.OK, MessageBoxIcon.Error);
        return;
      }
      (Parent as TaskPaneControl).ImportDataToExcel(importRoutineForm.ImportDataSet, importRoutineForm.ImportHeaders, importRoutineForm.ImportType);
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
