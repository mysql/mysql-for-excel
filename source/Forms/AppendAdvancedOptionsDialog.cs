using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace MySQL.ForExcel
{
  public partial class AppendAdvancedOptionsDialog : AutoStyleableBaseDialog
  {

    private MySQLColumnMappingList mappings;
    private MySQLColumnMapping selectedMapping;

    public AppendAdvancedOptionsDialog()
    {
      InitializeComponent();

      chkDoNotPerformAutoMap.Checked = Properties.Settings.Default.AppendPerformAutoMap;
      chkAutoStoreColumnMapping.Checked = Properties.Settings.Default.AppendAutoStoreColumnMapping;
      chkReloadColumnMapping.Checked = Properties.Settings.Default.AppendReloadColumnMapping;
      chkUseFormattedValues.Checked = Properties.Settings.Default.AppendUseFormattedValues;
      mappings = new MySQLColumnMappingList();
      RefreshMappingList();
    }

    private void btnAccept_Click(object sender, EventArgs e)
    {
      Properties.Settings.Default.AppendPerformAutoMap = chkDoNotPerformAutoMap.Checked;
      Properties.Settings.Default.AppendAutoStoreColumnMapping = chkAutoStoreColumnMapping.Checked;
      Properties.Settings.Default.AppendReloadColumnMapping = chkReloadColumnMapping.Checked;
      Properties.Settings.Default.AppendUseFormattedValues = chkUseFormattedValues.Checked;
      MiscUtilities.SaveSettings();
      DialogResult = DialogResult.OK;
      Close();
    }


    private void RefreshMappingList()
    {
      lstMappings.Items.Clear();

      foreach (var item in mappings.UserColumnMappingsList)
      {
        ListViewItem itemList =  new ListViewItem();
        itemList.Text = string.Format("{0} ({1}.{2})", item.Name, item.SchemaName, item.TableName);
        itemList.SubItems.Add("");
        itemList.Tag = item;
        lstMappings.Items.Add(itemList);
      }

      if (lstMappings.Items.Count > 0)
      {
        lstMappings.Items[0].Selected = true;                
      }
      else
      {
        btnDelete.Enabled = false;
        btnRenameMapping.Enabled = false;      
      }
    }

    private void btnDelete_Click(object sender, EventArgs e)
    {
      if (selectedMapping == null) return;

      mappings.Remove(selectedMapping);
      RefreshMappingList();
    }

    private void lstMappings_SelectedIndexChanged(object sender, EventArgs e)
    {
      btnDelete.Enabled = lstMappings.SelectedItems.Count > 0;      
      selectedMapping = lstMappings.SelectedItems.Count > 0 ? lstMappings.SelectedItems[0].Tag as MySQLColumnMapping : null;
      btnRenameMapping.Enabled = selectedMapping != null;
    }

    private void btnRenameMapping_Click(object sender, EventArgs e)
    {
      if (selectedMapping == null)
        return;
      var indexForName = 1;
      
      string proposedMappingName = String.Empty;
      do
      {
        proposedMappingName = String.Format("{0}Mapping{1}", selectedMapping.TableName, (indexForName > 1 ? indexForName.ToString() : String.Empty));
        indexForName++;
      }
      while (mappings.UserColumnMappingsList.Any(mapping => mapping.Name == proposedMappingName));

      var newColumnMappingName = new AppendNewColumnMappingDialog(proposedMappingName);
      DialogResult dr = newColumnMappingName.ShowDialog();
      if (dr == DialogResult.Cancel)
        return;

      var newName = newColumnMappingName.ColumnMappingName;
       
      newColumnMappingName.Dispose();

      // show error if name already exists
      if (mappings.UserColumnMappingsList.Where(t => t.Name.Equals(newName)).Count() > 0)
      {
        InfoDialog infoDialog = new InfoDialog(false, "Name is already in use", String.Format(@"Description Error: \""{0}\""", "Please try a different name for the mapping."));
        infoDialog.ShowDialog();
        return;      
      }

      mappings.Rename(selectedMapping, newName);
      RefreshMappingList();
      ListViewItem item = lstMappings.FindItemWithText(string.Format("{0} ({1}.{2})", newName, selectedMapping.SchemaName, selectedMapping.TableName));
      if (item != null)
      {
        lstMappings.Items[item.Index].Selected = true;
      }
      lstMappings.Focus();
    }    
  }
}
