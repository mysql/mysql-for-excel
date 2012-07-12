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
        lstMappings.Focus();
        btnDelete.Enabled = true;        
      }

      btnRenameMapping.Enabled = false;      
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

      if (selectedMapping != null)
      {
        txtNewName.Text = selectedMapping.Name;
        txtNewName.SelectionStart = 0;
        txtNewName.SelectionLength = txtNewName.Text.Length;

        if (selectedMapping.Name.Equals(txtNewName.Text))
          btnRenameMapping.Enabled = false;
      }
    }

    private void btnRenameMapping_Click(object sender, EventArgs e)
    {
      if (selectedMapping == null) return;

      // show error if name already exists
      if (mappings.UserColumnMappingsList.Where(t => t.Name.Equals(txtNewName.Text)).Count() > 0)
      {
        lblMappingNameWarning.Visible = true;
        picMappingNameWarning.Visible = true;        
        txtNewName.SelectionStart = 0;
        txtNewName.SelectionLength = txtNewName.Text.Length;
        txtNewName.Focus();
        return;      
      }      
      mappings.Rename(selectedMapping, txtNewName.Text);
      RefreshMappingList();
      ListViewItem item = lstMappings.FindItemWithText(string.Format("{0} ({1}.{2})", txtNewName.Text , selectedMapping.SchemaName, selectedMapping.TableName));
      if (item != null)
      {
        lstMappings.Items[item.Index].Selected = true;
      }
      lstMappings.Focus();
    }

    private void txtName_OnTextChanged(object sender, EventArgs e)
    {
      if (!txtNewName.Text.Equals(selectedMapping.Name))
        btnRenameMapping.Enabled = true;
    }

    private void txtName_GotFocus(object sender, EventArgs e)
    {            
      txtNewName.SelectionStart = 0;
      txtNewName.SelectionLength = txtNewName.Text.Length;
    }
  }
}
