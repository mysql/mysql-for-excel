using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace MySQL.ExcelAddIn
{
  public partial class DBObjectSelectionPanel : UserControl
  {
    private DBObject selectedDBObject;
    public MySQLSchemaInfo SchemaInfo { set; private get; }
    public bool ExportDataActionEnabled
    {
      set { infExportDataNewTable.PictureEnabled = value; }
      get { return infExportDataNewTable.PictureEnabled; }
    }
    public delegate bool DBObjectSelectionPanelLeavingHandler(object sender, DBObjectSelectionPanelLeavingArgs args);
    public event DBObjectSelectionPanelLeavingHandler DBObjectSelectionPanelLeaving;

    public DBObjectSelectionPanel()
    {
      InitializeComponent();
      Utilities.SetDoubleBuffered(lisDBObjects);
    }

    private void resetDBObjectSelectionPanel()
    {
      lisDBObjects.Items.Clear();
      lblConnectionName.Text = "Connection Name";
      lblUserIP.Text = "User: ??, IP: ??";

      lisDBObjects.Groups["grpTables"].Header = String.Format("Tables ({0})",
                                                              SchemaInfo.TablesTable.Rows.Count);
      foreach (DataRow tableRow in SchemaInfo.TablesTable.Rows)
      {
        string tableName = tableRow["TABLE_NAME"].ToString();
        string[] tileItems = new string[] { tableName, String.Format("Engine: {0}", tableRow["ENGINE"].ToString()) };
        ListViewItem lvi = new ListViewItem(tileItems, 0, lisDBObjects.Groups["grpTables"]);
        lvi.Name = tableName;
        lvi.Font = new Font("Arial", 8, FontStyle.Regular);
        lisDBObjects.Items.Add(lvi);
      }

      lisDBObjects.Groups["grpViews"].Header = String.Format("Views ({0})",
                                                             SchemaInfo.ViewsTable.Rows.Count);
      foreach (DataRow viewRow in SchemaInfo.ViewsTable.Rows)
      {
        string viewName = viewRow["TABLE_NAME"].ToString();
        string[] tileItems = new string[] { viewName, String.Format("Updatable: {0}", viewRow["IS_UPDATABLE"].ToString()) };
        ListViewItem lvi = new ListViewItem(tileItems, 1, lisDBObjects.Groups["grpViews"]);
        lvi.Name = viewName;
        lvi.Font = new Font("Arial", 8, FontStyle.Regular);
        lisDBObjects.Items.Add(lvi);
      }

      lisDBObjects.Groups["grpRoutines"].Header = String.Format("Routines ({0})",
                                                             SchemaInfo.RoutinesTable.Rows.Count);
      foreach (DataRow routineRow in SchemaInfo.RoutinesTable.Rows)
      {
        string routineName = routineRow["ROUTINE_NAME"].ToString();
        string[] tileItems = new string[] { routineName, String.Format("Type: {0}", routineRow["ROUTINE_TYPE"].ToString()) };
        ListViewItem lvi = new ListViewItem(tileItems, 1, lisDBObjects.Groups["grpRoutines"]);
        lvi.Name = routineName;
        lvi.Font = new Font("Arial", 8, FontStyle.Regular);
        lisDBObjects.Items.Add(lvi);
      }

      if (SchemaInfo.ConnectionData != null)
      {
        lblConnectionName.Text = SchemaInfo.ConnectionData.Name;
        lblUserIP.Text = SchemaInfo.ConnectionData.GluedConnection;
      }
    }

    private bool importDataToExcel()
    {
      bool success = false;

      if (selectedDBObject != null)
      {
        switch (selectedDBObject.Type)
        {
          case DBObjectType.Table:
            MessageBox.Show("Importing Data From Table...");
            DataTable data = SchemaInfo.GetTableData(selectedDBObject.Name, null, String.Empty);
            success = OnDBObjectSelectionPanelLeaving(new DBObjectSelectionPanelLeavingArgs(DBObjectSelectionPanelLeavingAction.Import, data));
            break;
          case DBObjectType.View:
            MessageBox.Show("Importing Data From View...");
            break;
          case DBObjectType.Routine:
            MessageBox.Show("Importing Data From Routine...");
            break;
        }
      }

      return success;
    }

    private bool exportDataToTable(string appendToTableName)
    {
      bool success = false;
      DBObjectSelectionPanelLeavingArgs args;

      if (appendToTableName != null && appendToTableName != String.Empty)
        args = new DBObjectSelectionPanelLeavingArgs(DBObjectSelectionPanelLeavingAction.Append, appendToTableName);
      else
        args = new DBObjectSelectionPanelLeavingArgs(DBObjectSelectionPanelLeavingAction.Export, String.Empty);
      success = OnDBObjectSelectionPanelLeaving(args);
      return success;
    }

    protected virtual bool OnDBObjectSelectionPanelLeaving(DBObjectSelectionPanelLeavingArgs args)
    {
      bool success = false;
      if (DBObjectSelectionPanelLeaving != null)
        success = DBObjectSelectionPanelLeaving(this, args);
      lisDBObjects.SelectedItems.Clear();
      return success;
    }

    private void lisDBObjects_ItemSelectionChanged(object sender, ListViewItemSelectionChangedEventArgs e)
    {
      if (lisDBObjects.SelectedItems.Count > 0 && !e.Item.Equals(lisDBObjects.SelectedItems[0]))
        return;

      if (e.IsSelected)
      {
        selectedDBObject = new DBObject();
        selectedDBObject.Name = e.Item.Name;
        switch (e.Item.Group.Name)
        {
          case "grpTables":
            selectedDBObject.Type = DBObjectType.Table;
            break;
          case "grpViews":
            selectedDBObject.Type = DBObjectType.View;
            break;
          case "grpRoutines":
            selectedDBObject.Type = DBObjectType.Routine;
            break;
        }
      }
      else
        selectedDBObject = null;

      infImportData.PictureEnabled = e.IsSelected;
      infEditData.PictureEnabled = e.IsSelected;
      infAppendData.PictureEnabled = e.IsSelected && (selectedDBObject.Type == DBObjectType.Table);
    }

    private void DBObjectSelectionPanel_VisibleChanged(object sender, EventArgs e)
    {
      if (this.Visible)
      {
        resetDBObjectSelectionPanel();
        infImportData.PictureEnabled = false;
        infEditData.PictureEnabled = false;
        infAppendData.PictureEnabled = false;
      }
    }

    private void infImportData_PictureClick(object sender, EventArgs e)
    {
      importDataToExcel();
    }

    private void infEditData_PictureClick(object sender, EventArgs e)
    {
      MessageBox.Show("Editing Data...");
    }

    private void infAppendData_PictureClick(object sender, EventArgs e)
    {
      MessageBox.Show("Appending Data...");
    }

    private void infExportDataNewTable_PictureClick(object sender, EventArgs e)
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
      OnDBObjectSelectionPanelLeaving(new DBObjectSelectionPanelLeavingArgs(DBObjectSelectionPanelLeavingAction.Back));
    }

    private void btnClose_Click(object sender, EventArgs e)
    {
      OnDBObjectSelectionPanelLeaving(new DBObjectSelectionPanelLeavingArgs(DBObjectSelectionPanelLeavingAction.Close));
    }

    private void dbObjectsContextMenu_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
    {
      if (selectedDBObject == null)
        return;
      switch(e.ClickedItem.Name)
      {
        case "importDataToolStripMenuItem":
          importDataToExcel();
          break;
        case "editDataToolStripMenuItem":
          break;
        case "appendDataToolStripMenuItem":
          exportDataToTable(e.ClickedItem.Name);
          break;
      }
    }

    private void dbObjectsContextMenu_Opening(object sender, CancelEventArgs e)
    {
      if (selectedDBObject == null)
      {
        e.Cancel = true;
        return;
      }
      dbObjectsContextMenu.Items["appendDataToolStripMenuItem"].Visible = selectedDBObject.Type == DBObjectType.Table;
    }
  }

  public enum DBObjectSelectionPanelLeavingAction { Back, Close, Import, Edit, Append, Export };

  public class DBObjectSelectionPanelLeavingArgs : EventArgs
  {
    private DBObjectSelectionPanelLeavingAction selectedAction;
    private DataTable dataForExcel;
    private string appendToTable;

    public DBObjectSelectionPanelLeavingAction SelectedAction
    {
      get { return selectedAction; }
    }
    public DataTable DataForExcel
    {
      get { return dataForExcel; }
    }
    public string AppendToTable
    {
      get { return appendToTable; }
    }

    public DBObjectSelectionPanelLeavingArgs(DBObjectSelectionPanelLeavingAction selAction, DataTable data)
    {
      selectedAction = selAction;
      dataForExcel = data;
      appendToTable = String.Empty;
    }

    public DBObjectSelectionPanelLeavingArgs(DBObjectSelectionPanelLeavingAction selAction, string appendToTableName)
    {
      selectedAction = selAction;
      dataForExcel = null;
      appendToTable = appendToTableName;
    }

    public DBObjectSelectionPanelLeavingArgs(DBObjectSelectionPanelLeavingAction selAction)
    {
      selectedAction = selAction;
      dataForExcel = null;
      appendToTable = String.Empty;
    }
  }

}
