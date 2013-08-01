// 
// Copyright (c) 2012-2013, Oracle and/or its affiliates. All rights reserved.
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
      this.DataRibbonTab = this.Factory.CreateRibbonTab();
      this.MySQLExcelAddInRibbonGroup = this.Factory.CreateRibbonGroup();
      this.ShowTaskPaneRibbonToggleButton = this.Factory.CreateRibbonToggleButton();
      this.DataRibbonTab.SuspendLayout();
      this.MySQLExcelAddInRibbonGroup.SuspendLayout();
      // 
      // DataRibbonTab
      // 
      this.DataRibbonTab.ControlId.ControlIdType = Microsoft.Office.Tools.Ribbon.RibbonControlIdType.Office;
      this.DataRibbonTab.ControlId.OfficeId = "TabData";
      this.DataRibbonTab.Groups.Add(this.MySQLExcelAddInRibbonGroup);
      this.DataRibbonTab.Label = "TabData";
      this.DataRibbonTab.Name = "DataRibbonTab";
      // 
      // MySQLExcelAddInRibbonGroup
      // 
      this.MySQLExcelAddInRibbonGroup.Items.Add(this.ShowTaskPaneRibbonToggleButton);
      this.MySQLExcelAddInRibbonGroup.Label = "Database";
      this.MySQLExcelAddInRibbonGroup.Name = "MySQLExcelAddInRibbonGroup";
      // 
      // ShowTaskPaneRibbonToggleButton
      // 
      this.ShowTaskPaneRibbonToggleButton.ControlSize = Microsoft.Office.Core.RibbonControlSize.RibbonControlSizeLarge;
      this.ShowTaskPaneRibbonToggleButton.Image = global::MySQL.ForExcel.Properties.Resources.MySQLforExcel_Logo_48x48;
      this.ShowTaskPaneRibbonToggleButton.Label = "MySQL for Excel";
      this.ShowTaskPaneRibbonToggleButton.Name = "ShowTaskPaneRibbonToggleButton";
      this.ShowTaskPaneRibbonToggleButton.ShowImage = true;
      this.ShowTaskPaneRibbonToggleButton.Click += new Microsoft.Office.Tools.Ribbon.RibbonControlEventHandler(this.ShowTaskPaneRibbonToggleButton_Click);
      // 
      // ManageTaskPaneRibbon
      // 
      this.Name = "ManageTaskPaneRibbon";
      this.RibbonType = "Microsoft.Excel.Workbook";
      this.Tabs.Add(this.DataRibbonTab);
      this.Load += new Microsoft.Office.Tools.Ribbon.RibbonUIEventHandler(this.ManageTaskPaneRibbon_Load);
      this.DataRibbonTab.ResumeLayout(false);
      this.DataRibbonTab.PerformLayout();
      this.MySQLExcelAddInRibbonGroup.ResumeLayout(false);
      this.MySQLExcelAddInRibbonGroup.PerformLayout();

    }

    #endregion

    internal Microsoft.Office.Tools.Ribbon.RibbonTab DataRibbonTab;
    internal Microsoft.Office.Tools.Ribbon.RibbonGroup MySQLExcelAddInRibbonGroup;
    internal Microsoft.Office.Tools.Ribbon.RibbonToggleButton ShowTaskPaneRibbonToggleButton;
  }

  partial class ThisRibbonCollection
  {
    internal ManageTaskPaneRibbon ManageTaskPaneRibbon
    {
      get { return this.GetRibbon<ManageTaskPaneRibbon>(); }
    }
  }
}
