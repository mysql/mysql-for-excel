// Copyright (c) 2013, 2019, Oracle and/or its affiliates. All rights reserved.
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

namespace MySQL.ForExcel.Forms
{
  partial class GlobalOptionsDialog
  {
    /// <summary>
    /// Required designer variable.
    /// </summary>
    private System.ComponentModel.IContainer components = null;

    /// <summary>
    /// Clean up any resources being used.
    /// </summary>
    /// <param name="disposing"><c>true</c> if managed resources should be disposed; otherwise, <c>false</c>.</param>
    protected override void Dispose(bool disposing)
    {
      if (disposing)
      {
        if (components != null)
        {
          components.Dispose();
        }

        if (_manageConnectionInfosDialog != null)
        {
          _manageConnectionInfosDialog.Dispose();
        }
      }

      base.Dispose(disposing);
    }

    #region Windows Form Designer generated code

    /// <summary>
    /// Required method for Designer support - do not modify
    /// the contents of this method with the code editor.
    /// </summary>
    private void InitializeComponent()
    {
      this.components = new System.ComponentModel.Container();
      System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(GlobalOptionsDialog));
      this.DialogAcceptButton = new System.Windows.Forms.Button();
      this.DialogCancelButton = new System.Windows.Forms.Button();
      this.ExcelToolTip = new System.Windows.Forms.ToolTip(this.components);
      this.MigrateWorkbenchConnectionsButton = new System.Windows.Forms.Button();
      this.QueryTimeoutNumericUpDown = new System.Windows.Forms.NumericUpDown();
      this.ConnectionTimeoutNumericUpDown = new System.Windows.Forms.NumericUpDown();
      this.ShowExecutedSqlQueryRadioButton = new System.Windows.Forms.RadioButton();
      this.PreviewSqlQueriesRadioButton = new System.Windows.Forms.RadioButton();
      this.NoSqlStatementsRadioButton = new System.Windows.Forms.RadioButton();
      this.ManageImportConnectionInfosButton = new System.Windows.Forms.Button();
      this.ShowingSidebarRadioButton = new System.Windows.Forms.RadioButton();
      this.OpeningWorkbookRadioButton = new System.Windows.Forms.RadioButton();
      this.ManageEditConnectionInfosButton = new System.Windows.Forms.Button();
      this.PreviewTableDataCheckBox = new System.Windows.Forms.CheckBox();
      this.CreateNewWorksheetsRadioButton = new System.Windows.Forms.RadioButton();
      this.ReuseWorksheetsRadioButton = new System.Windows.Forms.RadioButton();
      this.RestoreSavedEditSessionsCheckBox = new System.Windows.Forms.CheckBox();
      this.SpatialTextFormatComboBox = new System.Windows.Forms.ComboBox();
      this.ResetToDefaultsButton = new System.Windows.Forms.Button();
      this.OptionsTabControl = new System.Windows.Forms.TabControl();
      this.ConnectionsTabPage = new System.Windows.Forms.TabPage();
      this.AutomaticMigrationDelayValueLabel = new System.Windows.Forms.Label();
      this.AutomaticMigrationDelayLabel = new System.Windows.Forms.Label();
      this.QueryTimeout2Label = new System.Windows.Forms.Label();
      this.QueryTimeout1Label = new System.Windows.Forms.Label();
      this.ConnectionTimeout2Label = new System.Windows.Forms.Label();
      this.ConnectionTimeout1Label = new System.Windows.Forms.Label();
      this.ConnectionOptionsLabel = new System.Windows.Forms.Label();
      this.SqlQueriesTabPage = new System.Windows.Forms.TabPage();
      this.ShowQueriesOptionsPanel = new System.Windows.Forms.Panel();
      this.SqlQueriesLabel = new System.Windows.Forms.Label();
      this.SpatialDataTabPage = new System.Windows.Forms.TabPage();
      this.SpatialTextFormatLabel = new System.Windows.Forms.Label();
      this.SpatialDataLabel = new System.Windows.Forms.Label();
      this.ImportedTablesTabPage = new System.Windows.Forms.TabPage();
      this.RestoreImportedMySqlDataInExcelTablesLabel = new System.Windows.Forms.Label();
      this.ImportedTablesOptionsPanel = new System.Windows.Forms.Panel();
      this.ImportedTablesOptionsLabel = new System.Windows.Forms.Label();
      this.EditSessionsTabPage = new System.Windows.Forms.TabPage();
      this.ToleranceForFloatAndDoubleTextBox = new System.Windows.Forms.TextBox();
      this.ToleranceForFloatAndDoubleLabel = new System.Windows.Forms.Label();
      this.UseOptimisticUpdatesCheckBox = new System.Windows.Forms.CheckBox();
      this.EditSessionOptionsPanel = new System.Windows.Forms.Panel();
      this.EditSessionOptionsLabel = new System.Windows.Forms.Label();
      this.ContentAreaPanel.SuspendLayout();
      this.CommandAreaPanel.SuspendLayout();
      ((System.ComponentModel.ISupportInitialize)(this.QueryTimeoutNumericUpDown)).BeginInit();
      ((System.ComponentModel.ISupportInitialize)(this.ConnectionTimeoutNumericUpDown)).BeginInit();
      this.OptionsTabControl.SuspendLayout();
      this.ConnectionsTabPage.SuspendLayout();
      this.SqlQueriesTabPage.SuspendLayout();
      this.ShowQueriesOptionsPanel.SuspendLayout();
      this.SpatialDataTabPage.SuspendLayout();
      this.ImportedTablesTabPage.SuspendLayout();
      this.ImportedTablesOptionsPanel.SuspendLayout();
      this.EditSessionsTabPage.SuspendLayout();
      this.EditSessionOptionsPanel.SuspendLayout();
      this.SuspendLayout();
      // 
      // FootnoteAreaPanel
      // 
      this.FootnoteAreaPanel.Location = new System.Drawing.Point(0, 292);
      this.FootnoteAreaPanel.Size = new System.Drawing.Size(634, 0);
      // 
      // ContentAreaPanel
      // 
      this.ContentAreaPanel.Controls.Add(this.OptionsTabControl);
      this.ContentAreaPanel.Size = new System.Drawing.Size(541, 321);
      // 
      // CommandAreaPanel
      // 
      this.CommandAreaPanel.Controls.Add(this.ResetToDefaultsButton);
      this.CommandAreaPanel.Controls.Add(this.DialogAcceptButton);
      this.CommandAreaPanel.Controls.Add(this.DialogCancelButton);
      this.CommandAreaPanel.Location = new System.Drawing.Point(0, 276);
      this.CommandAreaPanel.Size = new System.Drawing.Size(541, 45);
      // 
      // DialogAcceptButton
      // 
      this.DialogAcceptButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
      this.DialogAcceptButton.DialogResult = System.Windows.Forms.DialogResult.OK;
      this.DialogAcceptButton.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.DialogAcceptButton.Location = new System.Drawing.Point(373, 11);
      this.DialogAcceptButton.Name = "DialogAcceptButton";
      this.DialogAcceptButton.Size = new System.Drawing.Size(75, 23);
      this.DialogAcceptButton.TabIndex = 0;
      this.DialogAcceptButton.Text = "Accept";
      this.DialogAcceptButton.UseVisualStyleBackColor = true;
      // 
      // DialogCancelButton
      // 
      this.DialogCancelButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
      this.DialogCancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
      this.DialogCancelButton.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.DialogCancelButton.Location = new System.Drawing.Point(454, 11);
      this.DialogCancelButton.Name = "DialogCancelButton";
      this.DialogCancelButton.Size = new System.Drawing.Size(75, 23);
      this.DialogCancelButton.TabIndex = 1;
      this.DialogCancelButton.Text = "Cancel";
      this.DialogCancelButton.UseVisualStyleBackColor = true;
      // 
      // ExcelToolTip
      // 
      this.ExcelToolTip.AutomaticDelay = 2000;
      // 
      // MigrateWorkbenchConnectionsButton
      // 
      this.MigrateWorkbenchConnectionsButton.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.MigrateWorkbenchConnectionsButton.Location = new System.Drawing.Point(37, 131);
      this.MigrateWorkbenchConnectionsButton.Name = "MigrateWorkbenchConnectionsButton";
      this.MigrateWorkbenchConnectionsButton.Size = new System.Drawing.Size(411, 23);
      this.MigrateWorkbenchConnectionsButton.TabIndex = 9;
      this.MigrateWorkbenchConnectionsButton.Text = "Migrate stored connections to MySQL Workbench now";
      this.ExcelToolTip.SetToolTip(this.MigrateWorkbenchConnectionsButton, "Migrates MySQL Server stored connections to the MySQL Workbench\'s connections fil" +
        "e.");
      this.MigrateWorkbenchConnectionsButton.UseVisualStyleBackColor = true;
      this.MigrateWorkbenchConnectionsButton.Click += new System.EventHandler(this.MigrateWorkbenchConnectionsButton_Click);
      // 
      // QueryTimeoutNumericUpDown
      // 
      this.QueryTimeoutNumericUpDown.Font = new System.Drawing.Font("Segoe UI", 9F);
      this.QueryTimeoutNumericUpDown.Location = new System.Drawing.Point(74, 74);
      this.QueryTimeoutNumericUpDown.Maximum = new decimal(new int[] {
            1000,
            0,
            0,
            0});
      this.QueryTimeoutNumericUpDown.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
      this.QueryTimeoutNumericUpDown.Name = "QueryTimeoutNumericUpDown";
      this.QueryTimeoutNumericUpDown.Size = new System.Drawing.Size(52, 23);
      this.QueryTimeoutNumericUpDown.TabIndex = 5;
      this.ExcelToolTip.SetToolTip(this.QueryTimeoutNumericUpDown, "Number of seconds to wait before a query sent to a MySQL server times out.\r\nOn ve" +
        "ry slow connections or when connecting to a slow computer it is advised to raise" +
        " this value.");
      this.QueryTimeoutNumericUpDown.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
      // 
      // ConnectionTimeoutNumericUpDown
      // 
      this.ConnectionTimeoutNumericUpDown.Font = new System.Drawing.Font("Segoe UI", 9F);
      this.ConnectionTimeoutNumericUpDown.Location = new System.Drawing.Point(74, 47);
      this.ConnectionTimeoutNumericUpDown.Maximum = new decimal(new int[] {
            1000,
            0,
            0,
            0});
      this.ConnectionTimeoutNumericUpDown.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
      this.ConnectionTimeoutNumericUpDown.Name = "ConnectionTimeoutNumericUpDown";
      this.ConnectionTimeoutNumericUpDown.Size = new System.Drawing.Size(52, 23);
      this.ConnectionTimeoutNumericUpDown.TabIndex = 2;
      this.ExcelToolTip.SetToolTip(this.ConnectionTimeoutNumericUpDown, "Number of seconds to wait before a connection to a MySQL server times out.\r\nOn ve" +
        "ry slow connections it is advised to raise this value.");
      this.ConnectionTimeoutNumericUpDown.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
      // 
      // ShowExecutedSqlQueryRadioButton
      // 
      this.ShowExecutedSqlQueryRadioButton.AutoSize = true;
      this.ShowExecutedSqlQueryRadioButton.Font = new System.Drawing.Font("Segoe UI", 9F);
      this.ShowExecutedSqlQueryRadioButton.Location = new System.Drawing.Point(32, 56);
      this.ShowExecutedSqlQueryRadioButton.Name = "ShowExecutedSqlQueryRadioButton";
      this.ShowExecutedSqlQueryRadioButton.Size = new System.Drawing.Size(312, 19);
      this.ShowExecutedSqlQueryRadioButton.TabIndex = 2;
      this.ShowExecutedSqlQueryRadioButton.TabStop = true;
      this.ShowExecutedSqlQueryRadioButton.Text = "Show executed SQL statements along with their results";
      this.ExcelToolTip.SetToolTip(this.ShowExecutedSqlQueryRadioButton, "SQL statements and their execution results are shown after they are sent to the s" +
        "erver.");
      this.ShowExecutedSqlQueryRadioButton.UseVisualStyleBackColor = true;
      // 
      // PreviewSqlQueriesRadioButton
      // 
      this.PreviewSqlQueriesRadioButton.AutoSize = true;
      this.PreviewSqlQueriesRadioButton.Font = new System.Drawing.Font("Segoe UI", 9F);
      this.PreviewSqlQueriesRadioButton.Location = new System.Drawing.Point(32, 33);
      this.PreviewSqlQueriesRadioButton.Name = "PreviewSqlQueriesRadioButton";
      this.PreviewSqlQueriesRadioButton.Size = new System.Drawing.Size(326, 19);
      this.PreviewSqlQueriesRadioButton.TabIndex = 1;
      this.PreviewSqlQueriesRadioButton.TabStop = true;
      this.PreviewSqlQueriesRadioButton.Text = "Preview SQL statements before they are sent to the server";
      this.ExcelToolTip.SetToolTip(this.PreviewSqlQueriesRadioButton, "SQL statements are shown, and can be modified, before they are sent to the server" +
        ".");
      this.PreviewSqlQueriesRadioButton.UseVisualStyleBackColor = true;
      // 
      // NoSqlStatementsRadioButton
      // 
      this.NoSqlStatementsRadioButton.AutoSize = true;
      this.NoSqlStatementsRadioButton.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.NoSqlStatementsRadioButton.Location = new System.Drawing.Point(32, 10);
      this.NoSqlStatementsRadioButton.Name = "NoSqlStatementsRadioButton";
      this.NoSqlStatementsRadioButton.Size = new System.Drawing.Size(270, 19);
      this.NoSqlStatementsRadioButton.TabIndex = 0;
      this.NoSqlStatementsRadioButton.TabStop = true;
      this.NoSqlStatementsRadioButton.Text = "Do not show SQL statements sent to the server";
      this.ExcelToolTip.SetToolTip(this.NoSqlStatementsRadioButton, "SQL statements are never shown, only the results of the executed queries are.");
      this.NoSqlStatementsRadioButton.UseVisualStyleBackColor = true;
      // 
      // ManageImportConnectionInfosButton
      // 
      this.ManageImportConnectionInfosButton.Font = new System.Drawing.Font("Segoe UI", 9F);
      this.ManageImportConnectionInfosButton.Location = new System.Drawing.Point(38, 124);
      this.ManageImportConnectionInfosButton.Name = "ManageImportConnectionInfosButton";
      this.ManageImportConnectionInfosButton.Size = new System.Drawing.Size(440, 23);
      this.ManageImportConnectionInfosButton.TabIndex = 3;
      this.ManageImportConnectionInfosButton.Text = "Manage connection information stored in the user settings file...";
      this.ExcelToolTip.SetToolTip(this.ManageImportConnectionInfosButton, "Allows you to select from all stored Import and Edit connection information to be" +
        " deleted once you press Accept.");
      this.ManageImportConnectionInfosButton.UseVisualStyleBackColor = true;
      this.ManageImportConnectionInfosButton.Click += new System.EventHandler(this.ManageConnectionInfosButton_Click);
      // 
      // ShowingSidebarRadioButton
      // 
      this.ShowingSidebarRadioButton.AutoSize = true;
      this.ShowingSidebarRadioButton.Font = new System.Drawing.Font("Segoe UI", 9F);
      this.ShowingSidebarRadioButton.Location = new System.Drawing.Point(52, 28);
      this.ShowingSidebarRadioButton.Name = "ShowingSidebarRadioButton";
      this.ShowingSidebarRadioButton.Size = new System.Drawing.Size(180, 19);
      this.ShowingSidebarRadioButton.TabIndex = 1;
      this.ShowingSidebarRadioButton.TabStop = true;
      this.ShowingSidebarRadioButton.Text = "Showing the Add-In\'s sidebar";
      this.ExcelToolTip.SetToolTip(this.ShowingSidebarRadioButton, "When checked, all MySQL data imported as Excel Tables is restored in the current " +
        "Excel workbook when the Add-In\'s sidebar is shown.");
      this.ShowingSidebarRadioButton.UseVisualStyleBackColor = true;
      // 
      // OpeningWorkbookRadioButton
      // 
      this.OpeningWorkbookRadioButton.AutoSize = true;
      this.OpeningWorkbookRadioButton.Font = new System.Drawing.Font("Segoe UI", 9F);
      this.OpeningWorkbookRadioButton.Location = new System.Drawing.Point(52, 5);
      this.OpeningWorkbookRadioButton.Name = "OpeningWorkbookRadioButton";
      this.OpeningWorkbookRadioButton.Size = new System.Drawing.Size(172, 19);
      this.OpeningWorkbookRadioButton.TabIndex = 0;
      this.OpeningWorkbookRadioButton.TabStop = true;
      this.OpeningWorkbookRadioButton.Text = "Opening an Excel workbook";
      this.ExcelToolTip.SetToolTip(this.OpeningWorkbookRadioButton, "When checked, all MySQL data imported as Excel Tables is restored when the Excel " +
        "workbook is opened even if the Add-In\'s sidebar has not been shown yet.");
      this.OpeningWorkbookRadioButton.UseVisualStyleBackColor = true;
      // 
      // ManageEditConnectionInfosButton
      // 
      this.ManageEditConnectionInfosButton.Font = new System.Drawing.Font("Segoe UI", 9F);
      this.ManageEditConnectionInfosButton.Location = new System.Drawing.Point(37, 200);
      this.ManageEditConnectionInfosButton.Name = "ManageEditConnectionInfosButton";
      this.ManageEditConnectionInfosButton.Size = new System.Drawing.Size(464, 23);
      this.ManageEditConnectionInfosButton.TabIndex = 7;
      this.ManageEditConnectionInfosButton.Text = "Manage connection information stored in the user settings file...";
      this.ExcelToolTip.SetToolTip(this.ManageEditConnectionInfosButton, "Allows you to select from all stored Import and Edit connection information to be" +
        " deleted once you press Accept.");
      this.ManageEditConnectionInfosButton.UseVisualStyleBackColor = true;
      this.ManageEditConnectionInfosButton.Click += new System.EventHandler(this.ManageConnectionInfosButton_Click);
      // 
      // PreviewTableDataCheckBox
      // 
      this.PreviewTableDataCheckBox.AutoSize = true;
      this.PreviewTableDataCheckBox.Font = new System.Drawing.Font("Segoe UI", 9F);
      this.PreviewTableDataCheckBox.Location = new System.Drawing.Point(37, 49);
      this.PreviewTableDataCheckBox.Name = "PreviewTableDataCheckBox";
      this.PreviewTableDataCheckBox.Size = new System.Drawing.Size(364, 19);
      this.PreviewTableDataCheckBox.TabIndex = 1;
      this.PreviewTableDataCheckBox.Text = "Preview MySQL table data before an Edit Data session is opened.";
      this.ExcelToolTip.SetToolTip(this.PreviewTableDataCheckBox, "When checked, the data of the selected MySQL table to edit is shown in a preview " +
        "dialog before the Edit Data session is opened.");
      this.PreviewTableDataCheckBox.UseVisualStyleBackColor = true;
      // 
      // CreateNewWorksheetsRadioButton
      // 
      this.CreateNewWorksheetsRadioButton.AutoSize = true;
      this.CreateNewWorksheetsRadioButton.Font = new System.Drawing.Font("Segoe UI", 9F);
      this.CreateNewWorksheetsRadioButton.Location = new System.Drawing.Point(52, 28);
      this.CreateNewWorksheetsRadioButton.Name = "CreateNewWorksheetsRadioButton";
      this.CreateNewWorksheetsRadioButton.Size = new System.Drawing.Size(355, 19);
      this.CreateNewWorksheetsRadioButton.TabIndex = 1;
      this.CreateNewWorksheetsRadioButton.TabStop = true;
      this.CreateNewWorksheetsRadioButton.Text = "Create new Excel worksheets for the restored Edit Data sessions";
      this.ExcelToolTip.SetToolTip(this.CreateNewWorksheetsRadioButton, "When restoring Edit Data sessions the data will be imported on new worksheets.");
      this.CreateNewWorksheetsRadioButton.UseVisualStyleBackColor = true;
      // 
      // ReuseWorksheetsRadioButton
      // 
      this.ReuseWorksheetsRadioButton.AutoSize = true;
      this.ReuseWorksheetsRadioButton.Font = new System.Drawing.Font("Segoe UI", 9F);
      this.ReuseWorksheetsRadioButton.Location = new System.Drawing.Point(52, 5);
      this.ReuseWorksheetsRadioButton.Name = "ReuseWorksheetsRadioButton";
      this.ReuseWorksheetsRadioButton.Size = new System.Drawing.Size(420, 19);
      this.ReuseWorksheetsRadioButton.TabIndex = 0;
      this.ReuseWorksheetsRadioButton.TabStop = true;
      this.ReuseWorksheetsRadioButton.Text = "Reuse Excel worksheets matching their names with the session table names";
      this.ExcelToolTip.SetToolTip(this.ReuseWorksheetsRadioButton, "When restoring Edit Data sessions the data will be imported on worksheets that ha" +
        "ve the same name as the MySQL table being edited.");
      this.ReuseWorksheetsRadioButton.UseVisualStyleBackColor = true;
      // 
      // RestoreSavedEditSessionsCheckBox
      // 
      this.RestoreSavedEditSessionsCheckBox.AutoSize = true;
      this.RestoreSavedEditSessionsCheckBox.Font = new System.Drawing.Font("Segoe UI", 9F);
      this.RestoreSavedEditSessionsCheckBox.Location = new System.Drawing.Point(37, 125);
      this.RestoreSavedEditSessionsCheckBox.Name = "RestoreSavedEditSessionsCheckBox";
      this.RestoreSavedEditSessionsCheckBox.Size = new System.Drawing.Size(374, 19);
      this.RestoreSavedEditSessionsCheckBox.TabIndex = 5;
      this.RestoreSavedEditSessionsCheckBox.Text = "Restore saved Edit Data sessions when opening an Excel workbook";
      this.ExcelToolTip.SetToolTip(this.RestoreSavedEditSessionsCheckBox, "When checked, Edit Data sessions that were active when an Excel workbook was save" +
        "d, are restored when the workbook is opened again.");
      this.RestoreSavedEditSessionsCheckBox.UseVisualStyleBackColor = true;
      this.RestoreSavedEditSessionsCheckBox.CheckedChanged += new System.EventHandler(this.RestoreSavedEditSessionsCheckBox_CheckedChanged);
      // 
      // SpatialTextFormatComboBox
      // 
      this.SpatialTextFormatComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
      this.SpatialTextFormatComboBox.FormattingEnabled = true;
      this.SpatialTextFormatComboBox.Location = new System.Drawing.Point(308, 48);
      this.SpatialTextFormatComboBox.Name = "SpatialTextFormatComboBox";
      this.SpatialTextFormatComboBox.Size = new System.Drawing.Size(168, 23);
      this.SpatialTextFormatComboBox.TabIndex = 2;
      this.ExcelToolTip.SetToolTip(this.SpatialTextFormatComboBox, resources.GetString("SpatialTextFormatComboBox.ToolTip"));
      // 
      // ResetToDefaultsButton
      // 
      this.ResetToDefaultsButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
      this.ResetToDefaultsButton.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.ResetToDefaultsButton.Location = new System.Drawing.Point(12, 11);
      this.ResetToDefaultsButton.Name = "ResetToDefaultsButton";
      this.ResetToDefaultsButton.Size = new System.Drawing.Size(128, 23);
      this.ResetToDefaultsButton.TabIndex = 2;
      this.ResetToDefaultsButton.Text = "Reset to Defaults";
      this.ResetToDefaultsButton.UseVisualStyleBackColor = true;
      this.ResetToDefaultsButton.Click += new System.EventHandler(this.ResetToDefaultsButton_Click);
      // 
      // OptionsTabControl
      // 
      this.OptionsTabControl.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
      this.OptionsTabControl.Controls.Add(this.ConnectionsTabPage);
      this.OptionsTabControl.Controls.Add(this.SqlQueriesTabPage);
      this.OptionsTabControl.Controls.Add(this.SpatialDataTabPage);
      this.OptionsTabControl.Controls.Add(this.ImportedTablesTabPage);
      this.OptionsTabControl.Controls.Add(this.EditSessionsTabPage);
      this.OptionsTabControl.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.OptionsTabControl.Location = new System.Drawing.Point(0, 0);
      this.OptionsTabControl.Name = "OptionsTabControl";
      this.OptionsTabControl.SelectedIndex = 0;
      this.OptionsTabControl.Size = new System.Drawing.Size(541, 281);
      this.OptionsTabControl.TabIndex = 0;
      // 
      // ConnectionsTabPage
      // 
      this.ConnectionsTabPage.Controls.Add(this.AutomaticMigrationDelayValueLabel);
      this.ConnectionsTabPage.Controls.Add(this.AutomaticMigrationDelayLabel);
      this.ConnectionsTabPage.Controls.Add(this.MigrateWorkbenchConnectionsButton);
      this.ConnectionsTabPage.Controls.Add(this.QueryTimeout2Label);
      this.ConnectionsTabPage.Controls.Add(this.QueryTimeoutNumericUpDown);
      this.ConnectionsTabPage.Controls.Add(this.QueryTimeout1Label);
      this.ConnectionsTabPage.Controls.Add(this.ConnectionTimeout2Label);
      this.ConnectionsTabPage.Controls.Add(this.ConnectionTimeoutNumericUpDown);
      this.ConnectionsTabPage.Controls.Add(this.ConnectionTimeout1Label);
      this.ConnectionsTabPage.Controls.Add(this.ConnectionOptionsLabel);
      this.ConnectionsTabPage.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.ConnectionsTabPage.Location = new System.Drawing.Point(4, 24);
      this.ConnectionsTabPage.Name = "ConnectionsTabPage";
      this.ConnectionsTabPage.Padding = new System.Windows.Forms.Padding(3);
      this.ConnectionsTabPage.Size = new System.Drawing.Size(533, 253);
      this.ConnectionsTabPage.TabIndex = 0;
      this.ConnectionsTabPage.Text = "Connections";
      this.ConnectionsTabPage.UseVisualStyleBackColor = true;
      // 
      // AutomaticMigrationDelayValueLabel
      // 
      this.AutomaticMigrationDelayValueLabel.AutoSize = true;
      this.AutomaticMigrationDelayValueLabel.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.AutomaticMigrationDelayValueLabel.ForeColor = System.Drawing.SystemColors.GrayText;
      this.AutomaticMigrationDelayValueLabel.Location = new System.Drawing.Point(303, 110);
      this.AutomaticMigrationDelayValueLabel.Name = "AutomaticMigrationDelayValueLabel";
      this.AutomaticMigrationDelayValueLabel.Size = new System.Drawing.Size(67, 15);
      this.AutomaticMigrationDelayValueLabel.TabIndex = 8;
      this.AutomaticMigrationDelayValueLabel.Text = "Delay Value";
      // 
      // AutomaticMigrationDelayLabel
      // 
      this.AutomaticMigrationDelayLabel.AutoSize = true;
      this.AutomaticMigrationDelayLabel.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.AutomaticMigrationDelayLabel.Location = new System.Drawing.Point(34, 110);
      this.AutomaticMigrationDelayLabel.Name = "AutomaticMigrationDelayLabel";
      this.AutomaticMigrationDelayLabel.Size = new System.Drawing.Size(263, 15);
      this.AutomaticMigrationDelayLabel.TabIndex = 7;
      this.AutomaticMigrationDelayLabel.Text = "Automatic connections migration delayed until: ";
      // 
      // QueryTimeout2Label
      // 
      this.QueryTimeout2Label.AutoSize = true;
      this.QueryTimeout2Label.Font = new System.Drawing.Font("Segoe UI", 9F);
      this.QueryTimeout2Label.Location = new System.Drawing.Point(132, 76);
      this.QueryTimeout2Label.Name = "QueryTimeout2Label";
      this.QueryTimeout2Label.Size = new System.Drawing.Size(316, 15);
      this.QueryTimeout2Label.TabIndex = 6;
      this.QueryTimeout2Label.Text = "seconds for a database query to execute before timing out.";
      // 
      // QueryTimeout1Label
      // 
      this.QueryTimeout1Label.AutoSize = true;
      this.QueryTimeout1Label.Font = new System.Drawing.Font("Segoe UI", 9F);
      this.QueryTimeout1Label.Location = new System.Drawing.Point(34, 76);
      this.QueryTimeout1Label.Name = "QueryTimeout1Label";
      this.QueryTimeout1Label.Size = new System.Drawing.Size(34, 15);
      this.QueryTimeout1Label.TabIndex = 4;
      this.QueryTimeout1Label.Text = "Wait ";
      // 
      // ConnectionTimeout2Label
      // 
      this.ConnectionTimeout2Label.AutoSize = true;
      this.ConnectionTimeout2Label.Font = new System.Drawing.Font("Segoe UI", 9F);
      this.ConnectionTimeout2Label.Location = new System.Drawing.Point(132, 49);
      this.ConnectionTimeout2Label.Name = "ConnectionTimeout2Label";
      this.ConnectionTimeout2Label.Size = new System.Drawing.Size(307, 15);
      this.ConnectionTimeout2Label.TabIndex = 3;
      this.ConnectionTimeout2Label.Text = "seconds for a connection to the server before timing out.";
      // 
      // ConnectionTimeout1Label
      // 
      this.ConnectionTimeout1Label.AutoSize = true;
      this.ConnectionTimeout1Label.Font = new System.Drawing.Font("Segoe UI", 9F);
      this.ConnectionTimeout1Label.Location = new System.Drawing.Point(34, 49);
      this.ConnectionTimeout1Label.Name = "ConnectionTimeout1Label";
      this.ConnectionTimeout1Label.Size = new System.Drawing.Size(34, 15);
      this.ConnectionTimeout1Label.TabIndex = 1;
      this.ConnectionTimeout1Label.Text = "Wait ";
      // 
      // ConnectionOptionsLabel
      // 
      this.ConnectionOptionsLabel.AutoSize = true;
      this.ConnectionOptionsLabel.BackColor = System.Drawing.Color.Transparent;
      this.ConnectionOptionsLabel.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.ConnectionOptionsLabel.ForeColor = System.Drawing.SystemColors.ControlText;
      this.ConnectionOptionsLabel.Location = new System.Drawing.Point(8, 20);
      this.ConnectionOptionsLabel.Name = "ConnectionOptionsLabel";
      this.ConnectionOptionsLabel.Size = new System.Drawing.Size(123, 17);
      this.ConnectionOptionsLabel.TabIndex = 0;
      this.ConnectionOptionsLabel.Text = "Connection Options";
      // 
      // SqlQueriesTabPage
      // 
      this.SqlQueriesTabPage.Controls.Add(this.ShowQueriesOptionsPanel);
      this.SqlQueriesTabPage.Controls.Add(this.SqlQueriesLabel);
      this.SqlQueriesTabPage.Location = new System.Drawing.Point(4, 24);
      this.SqlQueriesTabPage.Name = "SqlQueriesTabPage";
      this.SqlQueriesTabPage.Padding = new System.Windows.Forms.Padding(3);
      this.SqlQueriesTabPage.Size = new System.Drawing.Size(626, 224);
      this.SqlQueriesTabPage.TabIndex = 1;
      this.SqlQueriesTabPage.Text = "SQL Queries";
      this.SqlQueriesTabPage.UseVisualStyleBackColor = true;
      // 
      // ShowQueriesOptionsPanel
      // 
      this.ShowQueriesOptionsPanel.Controls.Add(this.ShowExecutedSqlQueryRadioButton);
      this.ShowQueriesOptionsPanel.Controls.Add(this.PreviewSqlQueriesRadioButton);
      this.ShowQueriesOptionsPanel.Controls.Add(this.NoSqlStatementsRadioButton);
      this.ShowQueriesOptionsPanel.Location = new System.Drawing.Point(6, 40);
      this.ShowQueriesOptionsPanel.Name = "ShowQueriesOptionsPanel";
      this.ShowQueriesOptionsPanel.Size = new System.Drawing.Size(427, 79);
      this.ShowQueriesOptionsPanel.TabIndex = 1;
      // 
      // SqlQueriesLabel
      // 
      this.SqlQueriesLabel.AutoSize = true;
      this.SqlQueriesLabel.BackColor = System.Drawing.Color.Transparent;
      this.SqlQueriesLabel.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.SqlQueriesLabel.ForeColor = System.Drawing.SystemColors.ControlText;
      this.SqlQueriesLabel.Location = new System.Drawing.Point(8, 20);
      this.SqlQueriesLabel.Name = "SqlQueriesLabel";
      this.SqlQueriesLabel.Size = new System.Drawing.Size(130, 17);
      this.SqlQueriesLabel.TabIndex = 0;
      this.SqlQueriesLabel.Text = "SQL Queries Options";
      // 
      // SpatialDataTabPage
      // 
      this.SpatialDataTabPage.Controls.Add(this.SpatialTextFormatComboBox);
      this.SpatialDataTabPage.Controls.Add(this.SpatialTextFormatLabel);
      this.SpatialDataTabPage.Controls.Add(this.SpatialDataLabel);
      this.SpatialDataTabPage.Location = new System.Drawing.Point(4, 24);
      this.SpatialDataTabPage.Name = "SpatialDataTabPage";
      this.SpatialDataTabPage.Padding = new System.Windows.Forms.Padding(3);
      this.SpatialDataTabPage.Size = new System.Drawing.Size(626, 224);
      this.SpatialDataTabPage.TabIndex = 2;
      this.SpatialDataTabPage.Text = "Spatial Data";
      this.SpatialDataTabPage.UseVisualStyleBackColor = true;
      // 
      // SpatialTextFormatLabel
      // 
      this.SpatialTextFormatLabel.AutoSize = true;
      this.SpatialTextFormatLabel.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.SpatialTextFormatLabel.Location = new System.Drawing.Point(35, 51);
      this.SpatialTextFormatLabel.Name = "SpatialTextFormatLabel";
      this.SpatialTextFormatLabel.Size = new System.Drawing.Size(264, 15);
      this.SpatialTextFormatLabel.TabIndex = 1;
      this.SpatialTextFormatLabel.Text = "Format to use when handling spatial data as text:";
      // 
      // SpatialDataLabel
      // 
      this.SpatialDataLabel.AutoSize = true;
      this.SpatialDataLabel.BackColor = System.Drawing.Color.Transparent;
      this.SpatialDataLabel.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.SpatialDataLabel.ForeColor = System.Drawing.SystemColors.ControlText;
      this.SpatialDataLabel.Location = new System.Drawing.Point(8, 20);
      this.SpatialDataLabel.Name = "SpatialDataLabel";
      this.SpatialDataLabel.Size = new System.Drawing.Size(128, 17);
      this.SpatialDataLabel.TabIndex = 0;
      this.SpatialDataLabel.Text = "Spatial Data Options";
      // 
      // ImportedTablesTabPage
      // 
      this.ImportedTablesTabPage.Controls.Add(this.ManageImportConnectionInfosButton);
      this.ImportedTablesTabPage.Controls.Add(this.RestoreImportedMySqlDataInExcelTablesLabel);
      this.ImportedTablesTabPage.Controls.Add(this.ImportedTablesOptionsPanel);
      this.ImportedTablesTabPage.Controls.Add(this.ImportedTablesOptionsLabel);
      this.ImportedTablesTabPage.Location = new System.Drawing.Point(4, 24);
      this.ImportedTablesTabPage.Name = "ImportedTablesTabPage";
      this.ImportedTablesTabPage.Size = new System.Drawing.Size(626, 224);
      this.ImportedTablesTabPage.TabIndex = 3;
      this.ImportedTablesTabPage.Text = "Imported Tables";
      this.ImportedTablesTabPage.UseVisualStyleBackColor = true;
      // 
      // RestoreImportedMySqlDataInExcelTablesLabel
      // 
      this.RestoreImportedMySqlDataInExcelTablesLabel.AutoSize = true;
      this.RestoreImportedMySqlDataInExcelTablesLabel.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.RestoreImportedMySqlDataInExcelTablesLabel.Location = new System.Drawing.Point(35, 51);
      this.RestoreImportedMySqlDataInExcelTablesLabel.Name = "RestoreImportedMySqlDataInExcelTablesLabel";
      this.RestoreImportedMySqlDataInExcelTablesLabel.Size = new System.Drawing.Size(278, 15);
      this.RestoreImportedMySqlDataInExcelTablesLabel.TabIndex = 1;
      this.RestoreImportedMySqlDataInExcelTablesLabel.Text = "Restore imported MySQL data in Excel Tables when:";
      // 
      // ImportedTablesOptionsPanel
      // 
      this.ImportedTablesOptionsPanel.Controls.Add(this.ShowingSidebarRadioButton);
      this.ImportedTablesOptionsPanel.Controls.Add(this.OpeningWorkbookRadioButton);
      this.ImportedTablesOptionsPanel.Location = new System.Drawing.Point(6, 67);
      this.ImportedTablesOptionsPanel.Name = "ImportedTablesOptionsPanel";
      this.ImportedTablesOptionsPanel.Size = new System.Drawing.Size(472, 51);
      this.ImportedTablesOptionsPanel.TabIndex = 2;
      // 
      // ImportedTablesOptionsLabel
      // 
      this.ImportedTablesOptionsLabel.AutoSize = true;
      this.ImportedTablesOptionsLabel.BackColor = System.Drawing.Color.Transparent;
      this.ImportedTablesOptionsLabel.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.ImportedTablesOptionsLabel.ForeColor = System.Drawing.SystemColors.ControlText;
      this.ImportedTablesOptionsLabel.Location = new System.Drawing.Point(8, 20);
      this.ImportedTablesOptionsLabel.Name = "ImportedTablesOptionsLabel";
      this.ImportedTablesOptionsLabel.Size = new System.Drawing.Size(153, 17);
      this.ImportedTablesOptionsLabel.TabIndex = 0;
      this.ImportedTablesOptionsLabel.Text = "Imported Tables Options";
      // 
      // EditSessionsTabPage
      // 
      this.EditSessionsTabPage.Controls.Add(this.ToleranceForFloatAndDoubleTextBox);
      this.EditSessionsTabPage.Controls.Add(this.ToleranceForFloatAndDoubleLabel);
      this.EditSessionsTabPage.Controls.Add(this.UseOptimisticUpdatesCheckBox);
      this.EditSessionsTabPage.Controls.Add(this.ManageEditConnectionInfosButton);
      this.EditSessionsTabPage.Controls.Add(this.PreviewTableDataCheckBox);
      this.EditSessionsTabPage.Controls.Add(this.EditSessionOptionsPanel);
      this.EditSessionsTabPage.Controls.Add(this.RestoreSavedEditSessionsCheckBox);
      this.EditSessionsTabPage.Controls.Add(this.EditSessionOptionsLabel);
      this.EditSessionsTabPage.Location = new System.Drawing.Point(4, 24);
      this.EditSessionsTabPage.Name = "EditSessionsTabPage";
      this.EditSessionsTabPage.Size = new System.Drawing.Size(533, 253);
      this.EditSessionsTabPage.TabIndex = 4;
      this.EditSessionsTabPage.Text = "Edit Sessions";
      this.EditSessionsTabPage.UseVisualStyleBackColor = true;
      // 
      // ToleranceForFloatAndDoubleTextBox
      // 
      this.ToleranceForFloatAndDoubleTextBox.Location = new System.Drawing.Point(409, 96);
      this.ToleranceForFloatAndDoubleTextBox.Name = "ToleranceForFloatAndDoubleTextBox";
      this.ToleranceForFloatAndDoubleTextBox.Size = new System.Drawing.Size(92, 23);
      this.ToleranceForFloatAndDoubleTextBox.TabIndex = 4;
      this.ExcelToolTip.SetToolTip(this.ToleranceForFloatAndDoubleTextBox, resources.GetString("ToleranceForFloatAndDoubleTextBox.ToolTip"));
      this.ToleranceForFloatAndDoubleTextBox.Validating += new System.ComponentModel.CancelEventHandler(this.ToleranceForFloatAndDoubleTextBox_Validating);
      // 
      // ToleranceForFloatAndDoubleLabel
      // 
      this.ToleranceForFloatAndDoubleLabel.AutoSize = true;
      this.ToleranceForFloatAndDoubleLabel.Location = new System.Drawing.Point(54, 99);
      this.ToleranceForFloatAndDoubleLabel.Name = "ToleranceForFloatAndDoubleLabel";
      this.ToleranceForFloatAndDoubleLabel.Size = new System.Drawing.Size(349, 15);
      this.ToleranceForFloatAndDoubleLabel.TabIndex = 3;
      this.ToleranceForFloatAndDoubleLabel.Text = "Tolerance for FLOAT and DOUBLE comparisons in WHERE clause:";
      // 
      // UseOptimisticUpdatesCheckBox
      // 
      this.UseOptimisticUpdatesCheckBox.AutoSize = true;
      this.UseOptimisticUpdatesCheckBox.Font = new System.Drawing.Font("Segoe UI", 8.25F);
      this.UseOptimisticUpdatesCheckBox.Location = new System.Drawing.Point(37, 74);
      this.UseOptimisticUpdatesCheckBox.Name = "UseOptimisticUpdatesCheckBox";
      this.UseOptimisticUpdatesCheckBox.Size = new System.Drawing.Size(271, 17);
      this.UseOptimisticUpdatesCheckBox.TabIndex = 2;
      this.UseOptimisticUpdatesCheckBox.Text = "Use optimistic updates on all Edit Data sessions";
      this.ExcelToolTip.SetToolTip(this.UseOptimisticUpdatesCheckBox, resources.GetString("UseOptimisticUpdatesCheckBox.ToolTip"));
      this.UseOptimisticUpdatesCheckBox.UseVisualStyleBackColor = true;
      this.UseOptimisticUpdatesCheckBox.CheckedChanged += new System.EventHandler(this.UseOptimisticUpdatesCheckBox_CheckedChanged);
      // 
      // EditSessionOptionsPanel
      // 
      this.EditSessionOptionsPanel.Controls.Add(this.CreateNewWorksheetsRadioButton);
      this.EditSessionOptionsPanel.Controls.Add(this.ReuseWorksheetsRadioButton);
      this.EditSessionOptionsPanel.Location = new System.Drawing.Point(5, 143);
      this.EditSessionOptionsPanel.Name = "EditSessionOptionsPanel";
      this.EditSessionOptionsPanel.Size = new System.Drawing.Size(496, 51);
      this.EditSessionOptionsPanel.TabIndex = 6;
      // 
      // EditSessionOptionsLabel
      // 
      this.EditSessionOptionsLabel.AutoSize = true;
      this.EditSessionOptionsLabel.BackColor = System.Drawing.Color.Transparent;
      this.EditSessionOptionsLabel.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.EditSessionOptionsLabel.ForeColor = System.Drawing.SystemColors.ControlText;
      this.EditSessionOptionsLabel.Location = new System.Drawing.Point(8, 20);
      this.EditSessionOptionsLabel.Name = "EditSessionOptionsLabel";
      this.EditSessionOptionsLabel.Size = new System.Drawing.Size(134, 17);
      this.EditSessionOptionsLabel.TabIndex = 0;
      this.EditSessionOptionsLabel.Text = "Edit Sesssion Options";
      // 
      // GlobalOptionsDialog
      // 
      this.AcceptButton = this.DialogAcceptButton;
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.CancelButton = this.DialogCancelButton;
      this.ClientSize = new System.Drawing.Size(541, 321);
      this.CommandAreaVisible = true;
      this.FootnoteAreaHeight = 0;
      this.MainInstructionLocation = new System.Drawing.Point(13, 21);
      this.MinimumSize = new System.Drawing.Size(537, 314);
      this.Name = "GlobalOptionsDialog";
      this.Text = "Global Options";
      this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.GlobalOptionsDialog_FormClosing);
      this.ContentAreaPanel.ResumeLayout(false);
      this.CommandAreaPanel.ResumeLayout(false);
      ((System.ComponentModel.ISupportInitialize)(this.QueryTimeoutNumericUpDown)).EndInit();
      ((System.ComponentModel.ISupportInitialize)(this.ConnectionTimeoutNumericUpDown)).EndInit();
      this.OptionsTabControl.ResumeLayout(false);
      this.ConnectionsTabPage.ResumeLayout(false);
      this.ConnectionsTabPage.PerformLayout();
      this.SqlQueriesTabPage.ResumeLayout(false);
      this.SqlQueriesTabPage.PerformLayout();
      this.ShowQueriesOptionsPanel.ResumeLayout(false);
      this.ShowQueriesOptionsPanel.PerformLayout();
      this.SpatialDataTabPage.ResumeLayout(false);
      this.SpatialDataTabPage.PerformLayout();
      this.ImportedTablesTabPage.ResumeLayout(false);
      this.ImportedTablesTabPage.PerformLayout();
      this.ImportedTablesOptionsPanel.ResumeLayout(false);
      this.ImportedTablesOptionsPanel.PerformLayout();
      this.EditSessionsTabPage.ResumeLayout(false);
      this.EditSessionsTabPage.PerformLayout();
      this.EditSessionOptionsPanel.ResumeLayout(false);
      this.EditSessionOptionsPanel.PerformLayout();
      this.ResumeLayout(false);

    }

    #endregion

    private System.Windows.Forms.Button DialogAcceptButton;
    private System.Windows.Forms.Button DialogCancelButton;
    private System.Windows.Forms.ToolTip ExcelToolTip;
    private System.Windows.Forms.Button ResetToDefaultsButton;
    private System.Windows.Forms.TabControl OptionsTabControl;
    private System.Windows.Forms.TabPage ConnectionsTabPage;
    private System.Windows.Forms.Label AutomaticMigrationDelayValueLabel;
    private System.Windows.Forms.Label AutomaticMigrationDelayLabel;
    private System.Windows.Forms.Button MigrateWorkbenchConnectionsButton;
    private System.Windows.Forms.Label QueryTimeout2Label;
    private System.Windows.Forms.NumericUpDown QueryTimeoutNumericUpDown;
    private System.Windows.Forms.Label QueryTimeout1Label;
    private System.Windows.Forms.Label ConnectionTimeout2Label;
    private System.Windows.Forms.NumericUpDown ConnectionTimeoutNumericUpDown;
    private System.Windows.Forms.Label ConnectionTimeout1Label;
    private System.Windows.Forms.Label ConnectionOptionsLabel;
    private System.Windows.Forms.TabPage SqlQueriesTabPage;
    private System.Windows.Forms.Panel ShowQueriesOptionsPanel;
    private System.Windows.Forms.RadioButton ShowExecutedSqlQueryRadioButton;
    private System.Windows.Forms.RadioButton PreviewSqlQueriesRadioButton;
    private System.Windows.Forms.RadioButton NoSqlStatementsRadioButton;
    private System.Windows.Forms.Label SqlQueriesLabel;
    private System.Windows.Forms.TabPage SpatialDataTabPage;
    private System.Windows.Forms.TabPage ImportedTablesTabPage;
    private System.Windows.Forms.Button ManageImportConnectionInfosButton;
    private System.Windows.Forms.Label RestoreImportedMySqlDataInExcelTablesLabel;
    private System.Windows.Forms.Panel ImportedTablesOptionsPanel;
    private System.Windows.Forms.RadioButton ShowingSidebarRadioButton;
    private System.Windows.Forms.RadioButton OpeningWorkbookRadioButton;
    private System.Windows.Forms.Label ImportedTablesOptionsLabel;
    private System.Windows.Forms.TabPage EditSessionsTabPage;
    private System.Windows.Forms.Button ManageEditConnectionInfosButton;
    private System.Windows.Forms.CheckBox PreviewTableDataCheckBox;
    private System.Windows.Forms.Panel EditSessionOptionsPanel;
    private System.Windows.Forms.RadioButton CreateNewWorksheetsRadioButton;
    private System.Windows.Forms.RadioButton ReuseWorksheetsRadioButton;
    private System.Windows.Forms.CheckBox RestoreSavedEditSessionsCheckBox;
    private System.Windows.Forms.Label EditSessionOptionsLabel;
    private System.Windows.Forms.Label SpatialDataLabel;
    private System.Windows.Forms.ComboBox SpatialTextFormatComboBox;
    private System.Windows.Forms.Label SpatialTextFormatLabel;
    private System.Windows.Forms.TextBox ToleranceForFloatAndDoubleTextBox;
    private System.Windows.Forms.Label ToleranceForFloatAndDoubleLabel;
    private System.Windows.Forms.CheckBox UseOptimisticUpdatesCheckBox;
  }
}