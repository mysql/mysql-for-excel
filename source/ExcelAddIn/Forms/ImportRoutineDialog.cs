using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using MySQL.Utility;

namespace MySQL.ExcelAddIn
{
  public partial class ImportRoutineDialog : Form
  {
    private MySqlWorkbenchConnection wbConnection;
    private DBObject importDBObject;

    public ImportRoutineDialog()
    {
      InitializeComponent();
    }
  }
}
