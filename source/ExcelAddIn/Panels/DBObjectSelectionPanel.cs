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

    //private DBObject selectedDBObject;
    //public MySQLSchemaInfo SchemaInfo { set; private get; }
    
    public bool ExportDataActionEnabled
    {
      set { exportToNewTable.Enabled = value; }
      get { return exportToNewTable.Enabled; }
    }

    public DBObjectSelectionPanel()
    {
      InitializeComponent();
      Utilities.SetDoubleBuffered(lisDBObjects);
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
      lisDBObjects.Items.Clear();
      LoadTables();
      LoadViews();
      LoadRoutines();
    }

    private void LoadTables()
    {
      int counter = 0;
      DataTable tables = Utilities.GetSchemaCollection(connection, "Tables", null, connection.Schema);

      foreach (DataRow tableRow in tables.Rows)
      {
        string tableName = tableRow["TABLE_NAME"].ToString();
        string[] tileItems = new string[] { tableName, String.Format("Engine: {0}", tableRow["ENGINE"].ToString()) };
        ListViewItem lvi = new ListViewItem(tileItems, 0, lisDBObjects.Groups["grpTables"]);
        lvi.Name = tableName;
        lvi.Font = new Font("Arial", 8, FontStyle.Regular);
        lvi.Tag = new DBObject(tableName, DBObjectType.Table);
        lisDBObjects.Items.Add(lvi);
        counter++;
      }
      lisDBObjects.Groups["grpTables"].Header = String.Format("Tables ({0})", counter);
    }

    private void LoadViews()
    {
      int counter = 0;
      DataTable views = Utilities.GetSchemaCollection(connection, "Views", null, connection.Schema);
      if (views == null) return;
      foreach (DataRow viewRow in views.Rows)
      {
        string viewName = viewRow["TABLE_NAME"].ToString();
        string[] tileItems = new string[] { viewName, String.Format("Updatable: {0}", viewRow["IS_UPDATABLE"].ToString()) };
        ListViewItem lvi = new ListViewItem(tileItems, 1, lisDBObjects.Groups["grpViews"]);
        lvi.Name = viewName;
        lvi.Font = new Font("Arial", 8, FontStyle.Regular);
        lvi.Tag = new DBObject(viewName, DBObjectType.View);
        lisDBObjects.Items.Add(lvi);
        counter++;
      }
      lisDBObjects.Groups["grpViews"].Header = String.Format("Views ({0})", counter);
    }

    private void LoadRoutines()
    {
      int counter = 0;
      DataTable procs = Utilities.GetSchemaCollection(connection, "Procedures", null, connection.Schema);
      if (procs == null) return;

      foreach (DataRow routineRow in procs.Rows)
      {
        string routineName = routineRow["ROUTINE_NAME"].ToString();
        string type = routineRow["ROUTINE_TYPE"].ToString();
        if (type != "PROCEDURE") continue;
        string[] tileItems = new string[] { routineName, String.Format("Type: {0}", type) };
        ListViewItem lvi = new ListViewItem(tileItems, 1, lisDBObjects.Groups["grpRoutines"]);
        lvi.Name = routineName;
        lvi.Font = new Font("Arial", 8, FontStyle.Regular);
        lvi.Tag = new DBObject(routineName, DBObjectType.Routine);
        lisDBObjects.Items.Add(lvi);
      }
      lisDBObjects.Groups["grpRoutines"].Header = String.Format("Routines ({0})", counter);
    }

    private bool exportDataToTable(string appendToTableName)
    {
      //bool success = false;
      //DBObjectSelectionPanelLeavingArgs args;

      //if (appendToTableName != null && appendToTableName != String.Empty)
      //  args = new DBObjectSelectionPanelLeavingArgs(DBObjectSelectionPanelLeavingAction.Append, appendToTableName);
      //else
      //  args = new DBObjectSelectionPanelLeavingArgs(DBObjectSelectionPanelLeavingAction.Export, String.Empty);
      //success = OnDBObjectSelectionPanelLeaving(args);
      //return success;
      return true;
    }

    private void lisDBObjects_ItemSelectionChanged(object sender, ListViewItemSelectionChangedEventArgs e)
    {
      if (lisDBObjects.SelectedItems.Count > 0 && !e.Item.Equals(lisDBObjects.SelectedItems[0]))
        return;
      DBObject o = e.Item.Tag as DBObject;

      importData.Enabled = e.IsSelected;
      editData.Enabled = e.IsSelected;
      appendData.Enabled = e.IsSelected && o.Type == DBObjectType.Table; 
    }

    private void importData_Click(object sender, EventArgs e)
    {
      if (lisDBObjects.SelectedItems.Count != 1) return;

      DBObject dbo = lisDBObjects.SelectedItems[0].Tag as DBObject;
      DataTable dt = Utilities.GetDataFromDbObject(connection, dbo);
      if (dt == null)
      {
        string msg = String.Format(Resources.UnableToRetrieveData, dbo.Name);
        MessageBox.Show(msg, Resources.Error, MessageBoxButtons.OK, MessageBoxIcon.Error);
        return;
      }

      (Parent as TaskPaneControl).ImportDataToExcel(dt);
    }

    private void editData_Click(object sender, EventArgs e)
    {
      MessageBox.Show("Editing Data...");
    }

    private void appendData_Click(object sender, EventArgs e)
    {
      MessageBox.Show("Appending Data...");
    }


    private void exportToNewTable_Click(object sender, EventArgs e)
    {
      if (lisDBObjects.SelectedItems.Count > 0)
        exportDataToTable(lisDBObjects.SelectedItems[0].Name);
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
