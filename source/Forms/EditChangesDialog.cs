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
  public partial class EditChangesDialog : Form
  {
    public EditChangesDialog(DataTable changesTable)
    {
      InitializeComponent();

      grdChanges.DataSource = changesTable;
      foreach (DataGridViewColumn dgvc in grdChanges.Columns)
      {
        dgvc.SortMode = DataGridViewColumnSortMode.NotSortable;
      }
    }
  }
}
