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

namespace MySQL.ForExcel
{
  partial class ManageTaskPaneRibbon : Microsoft.Office.Tools.Ribbon.RibbonBase
  {
    /// <summary>
    /// Required designer variable.
    /// </summary>
    private System.ComponentModel.IContainer components = null;

    public ManageTaskPaneRibbon()
      : base(Globals.Factory.GetRibbonFactory())
    {
      InitializeComponent();
    }

    /// <summary> 
    /// Clean up any resources being used.
    /// </summary>
    /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
    protected override void Dispose(bool disposing)
    {
      if (disposing && (components != null))
      {
        components.Dispose();
      }
      base.Dispose(disposing);
    }

    #region Component Designer generated code

    /// <summary>
    /// Required method for Designer support - do not modify
    /// the contents of this method with the code editor.
    /// </summary>
    private void InitializeComponent()
    {
      this.tab1 = this.Factory.CreateRibbonTab();
      this.grpMySQLExcelAddIn = this.Factory.CreateRibbonGroup();
      this.togShowTaskPane = this.Factory.CreateRibbonToggleButton();
      this.tab1.SuspendLayout();
      this.grpMySQLExcelAddIn.SuspendLayout();
      // 
      // tab1
      // 
      this.tab1.ControlId.ControlIdType = Microsoft.Office.Tools.Ribbon.RibbonControlIdType.Office;
      this.tab1.ControlId.OfficeId = "TabData";
      this.tab1.Groups.Add(this.grpMySQLExcelAddIn);
      this.tab1.Label = "TabData";
      this.tab1.Name = "tab1";
      // 
      // grpMySQLExcelAddIn
      // 
      this.grpMySQLExcelAddIn.Items.Add(this.togShowTaskPane);
      this.grpMySQLExcelAddIn.Label = "Database";
      this.grpMySQLExcelAddIn.Name = "grpMySQLExcelAddIn";
      // 
      // togShowTaskPane
      // 
      this.togShowTaskPane.ControlSize = Microsoft.Office.Core.RibbonControlSize.RibbonControlSizeLarge;
      this.togShowTaskPane.Image = global::MySQL.ForExcel.Properties.Resources.MySQLforExcel_Logo_48x48;
      this.togShowTaskPane.Label = "MySQL for Excel";
      this.togShowTaskPane.Name = "togShowTaskPane";
      this.togShowTaskPane.ShowImage = true;
      this.togShowTaskPane.Click += new Microsoft.Office.Tools.Ribbon.RibbonControlEventHandler(this.togShowTaskPane_Click);
      // 
      // ManageTaskPaneRibbon
      // 
      this.Name = "ManageTaskPaneRibbon";
      this.RibbonType = "Microsoft.Excel.Workbook";
      this.Tabs.Add(this.tab1);
      this.Load += new Microsoft.Office.Tools.Ribbon.RibbonUIEventHandler(this.ManageTaskPaneRibbon_Load);
      this.tab1.ResumeLayout(false);
      this.tab1.PerformLayout();
      this.grpMySQLExcelAddIn.ResumeLayout(false);
      this.grpMySQLExcelAddIn.PerformLayout();

    }

    #endregion

    internal Microsoft.Office.Tools.Ribbon.RibbonTab tab1;
    internal Microsoft.Office.Tools.Ribbon.RibbonGroup grpMySQLExcelAddIn;
    internal Microsoft.Office.Tools.Ribbon.RibbonToggleButton togShowTaskPane;
  }

  partial class ThisRibbonCollection
  {
    internal ManageTaskPaneRibbon ManageTaskPaneRibbon
    {
      get { return this.GetRibbon<ManageTaskPaneRibbon>(); }
    }
  }
}
