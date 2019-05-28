// Copyright (c) 2012, 2017, Oracle and/or its affiliates. All rights reserved.
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
  partial class AppendAdvancedOptionsDialog
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
      if (disposing && (components != null))
      {
        components.Dispose();
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
      System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(AppendAdvancedOptionsDialog));
      this.DialogAcceptButton = new System.Windows.Forms.Button();
      this.DialogCancelButton = new System.Windows.Forms.Button();
      this.HelpToolTip = new System.Windows.Forms.ToolTip(this.components);
      this.ConfirmMappingOverwritingCheckBox = new System.Windows.Forms.CheckBox();
      this.ReloadColumnMappingCheckBox = new System.Windows.Forms.CheckBox();
      this.AutoStoreColumnMappingCheckBox = new System.Windows.Forms.CheckBox();
      this.DoNotPerformAutoMapCheckBox = new System.Windows.Forms.CheckBox();
      this.ShowDataTypesCheckBox = new System.Windows.Forms.CheckBox();
      this.PreviewRowsQuantityNumericUpDown = new System.Windows.Forms.NumericUpDown();
      this.UseFormattedValuesCheckBox = new System.Windows.Forms.CheckBox();
      this.GenerateMultipleInsertsCheckBox = new System.Windows.Forms.CheckBox();
      this.DisableTableIndexesCheckBox = new System.Windows.Forms.CheckBox();
      this.ReplaceDuplicatesRadioButton = new System.Windows.Forms.RadioButton();
      this.IgnoreDuplicatesRadioButton = new System.Windows.Forms.RadioButton();
      this.ErrorAndAbortRadioButton = new System.Windows.Forms.RadioButton();
      this.ResetToDefaultsButton = new System.Windows.Forms.Button();
      this.OptionsTabControl = new System.Windows.Forms.TabControl();
      this.ColumnsMappingTabPage = new System.Windows.Forms.TabPage();
      this.MappingOptionsLabel = new System.Windows.Forms.Label();
      this.StoredMappingsTabPage = new System.Windows.Forms.TabPage();
      this.MappingsListView = new System.Windows.Forms.ListView();
      this.columnHeader1 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
      this.columnHeader2 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
      this.DeleteMappingButton = new System.Windows.Forms.Button();
      this.RenameMappingButton = new System.Windows.Forms.Button();
      this.StoredColumnMappingsLabel = new System.Windows.Forms.Label();
      this.FieldDataTabPage = new System.Windows.Forms.TabPage();
      this.FieldDataOptionsLabel = new System.Windows.Forms.Label();
      this.PreviewRowsQuantity1Label = new System.Windows.Forms.Label();
      this.PreviewRowsQuantity2Label = new System.Windows.Forms.Label();
      this.SqlQueriesTabPage = new System.Windows.Forms.TabPage();
      this.DuplicateValuesOptionsPanel = new System.Windows.Forms.Panel();
      this.DuplicateConflictsLabel = new System.Windows.Forms.Label();
      this.SqlQueriesLabel = new System.Windows.Forms.Label();
      this.ContentAreaPanel.SuspendLayout();
      this.CommandAreaPanel.SuspendLayout();
      ((System.ComponentModel.ISupportInitialize)(this.PreviewRowsQuantityNumericUpDown)).BeginInit();
      this.OptionsTabControl.SuspendLayout();
      this.ColumnsMappingTabPage.SuspendLayout();
      this.StoredMappingsTabPage.SuspendLayout();
      this.FieldDataTabPage.SuspendLayout();
      this.SqlQueriesTabPage.SuspendLayout();
      this.DuplicateValuesOptionsPanel.SuspendLayout();
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
      this.ContentAreaPanel.Size = new System.Drawing.Size(484, 300);
      // 
      // CommandAreaPanel
      // 
      this.CommandAreaPanel.Controls.Add(this.ResetToDefaultsButton);
      this.CommandAreaPanel.Controls.Add(this.DialogAcceptButton);
      this.CommandAreaPanel.Controls.Add(this.DialogCancelButton);
      this.CommandAreaPanel.Location = new System.Drawing.Point(0, 255);
      this.CommandAreaPanel.Size = new System.Drawing.Size(484, 45);
      // 
      // DialogAcceptButton
      // 
      this.DialogAcceptButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
      this.DialogAcceptButton.DialogResult = System.Windows.Forms.DialogResult.OK;
      this.DialogAcceptButton.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.DialogAcceptButton.Location = new System.Drawing.Point(316, 11);
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
      this.DialogCancelButton.Location = new System.Drawing.Point(397, 11);
      this.DialogCancelButton.Name = "DialogCancelButton";
      this.DialogCancelButton.Size = new System.Drawing.Size(75, 23);
      this.DialogCancelButton.TabIndex = 1;
      this.DialogCancelButton.Text = "Cancel";
      this.DialogCancelButton.UseVisualStyleBackColor = true;
      // 
      // HelpToolTip
      // 
      this.HelpToolTip.AutoPopDelay = 5000;
      this.HelpToolTip.InitialDelay = 1000;
      this.HelpToolTip.ReshowDelay = 100;
      // 
      // ConfirmMappingOverwritingCheckBox
      // 
      this.ConfirmMappingOverwritingCheckBox.AutoSize = true;
      this.ConfirmMappingOverwritingCheckBox.BackColor = System.Drawing.Color.Transparent;
      this.ConfirmMappingOverwritingCheckBox.Font = new System.Drawing.Font("Segoe UI", 9F);
      this.ConfirmMappingOverwritingCheckBox.Location = new System.Drawing.Point(37, 120);
      this.ConfirmMappingOverwritingCheckBox.Name = "ConfirmMappingOverwritingCheckBox";
      this.ConfirmMappingOverwritingCheckBox.Size = new System.Drawing.Size(228, 19);
      this.ConfirmMappingOverwritingCheckBox.TabIndex = 10;
      this.ConfirmMappingOverwritingCheckBox.Text = "Confirm column mapping overwriting";
      this.HelpToolTip.SetToolTip(this.ConfirmMappingOverwritingCheckBox, resources.GetString("ConfirmMappingOverwritingCheckBox.ToolTip"));
      this.ConfirmMappingOverwritingCheckBox.UseVisualStyleBackColor = false;
      // 
      // ReloadColumnMappingCheckBox
      // 
      this.ReloadColumnMappingCheckBox.AutoSize = true;
      this.ReloadColumnMappingCheckBox.BackColor = System.Drawing.Color.Transparent;
      this.ReloadColumnMappingCheckBox.Font = new System.Drawing.Font("Segoe UI", 9F);
      this.ReloadColumnMappingCheckBox.Location = new System.Drawing.Point(37, 95);
      this.ReloadColumnMappingCheckBox.Name = "ReloadColumnMappingCheckBox";
      this.ReloadColumnMappingCheckBox.Size = new System.Drawing.Size(381, 19);
      this.ReloadColumnMappingCheckBox.TabIndex = 9;
      this.ReloadColumnMappingCheckBox.Text = "Reload stored column mapping for the selected table automatically";
      this.HelpToolTip.SetToolTip(this.ReloadColumnMappingCheckBox, "When checked if there is a stored mapping saved for the selected MySQL table, the" +
        " mapping will be applied automatically next time data is appended to that table." +
        "");
      this.ReloadColumnMappingCheckBox.UseVisualStyleBackColor = false;
      // 
      // AutoStoreColumnMappingCheckBox
      // 
      this.AutoStoreColumnMappingCheckBox.AutoSize = true;
      this.AutoStoreColumnMappingCheckBox.BackColor = System.Drawing.Color.Transparent;
      this.AutoStoreColumnMappingCheckBox.Font = new System.Drawing.Font("Segoe UI", 9F);
      this.AutoStoreColumnMappingCheckBox.Location = new System.Drawing.Point(37, 72);
      this.AutoStoreColumnMappingCheckBox.Name = "AutoStoreColumnMappingCheckBox";
      this.AutoStoreColumnMappingCheckBox.Size = new System.Drawing.Size(343, 19);
      this.AutoStoreColumnMappingCheckBox.TabIndex = 8;
      this.AutoStoreColumnMappingCheckBox.Text = "Automatically store the column mapping for the given table";
      this.HelpToolTip.SetToolTip(this.AutoStoreColumnMappingCheckBox, "When checked a column mapping is automatically saved for the selected MySQL table" +
        ".");
      this.AutoStoreColumnMappingCheckBox.UseVisualStyleBackColor = false;
      // 
      // DoNotPerformAutoMapCheckBox
      // 
      this.DoNotPerformAutoMapCheckBox.AutoSize = true;
      this.DoNotPerformAutoMapCheckBox.BackColor = System.Drawing.Color.Transparent;
      this.DoNotPerformAutoMapCheckBox.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.DoNotPerformAutoMapCheckBox.Location = new System.Drawing.Point(37, 49);
      this.DoNotPerformAutoMapCheckBox.Name = "DoNotPerformAutoMapCheckBox";
      this.DoNotPerformAutoMapCheckBox.Size = new System.Drawing.Size(296, 19);
      this.DoNotPerformAutoMapCheckBox.TabIndex = 7;
      this.DoNotPerformAutoMapCheckBox.Text = "Perform an automatic mapping when dialog opens";
      this.HelpToolTip.SetToolTip(this.DoNotPerformAutoMapCheckBox, resources.GetString("DoNotPerformAutoMapCheckBox.ToolTip"));
      this.DoNotPerformAutoMapCheckBox.UseVisualStyleBackColor = false;
      // 
      // ShowDataTypesCheckBox
      // 
      this.ShowDataTypesCheckBox.AutoSize = true;
      this.ShowDataTypesCheckBox.BackColor = System.Drawing.Color.Transparent;
      this.ShowDataTypesCheckBox.Font = new System.Drawing.Font("Segoe UI", 9F);
      this.ShowDataTypesCheckBox.Location = new System.Drawing.Point(37, 97);
      this.ShowDataTypesCheckBox.Name = "ShowDataTypesCheckBox";
      this.ShowDataTypesCheckBox.Size = new System.Drawing.Size(273, 19);
      this.ShowDataTypesCheckBox.TabIndex = 17;
      this.ShowDataTypesCheckBox.Text = "Show column data types above column names";
      this.HelpToolTip.SetToolTip(this.ShowDataTypesCheckBox, "If checked the data types of the source data and target MySQL table are shown abo" +
        "ve column names.\r\nWhen unchecked, data types are shown as tooltips on column nam" +
        "es.");
      this.ShowDataTypesCheckBox.UseVisualStyleBackColor = false;
      // 
      // PreviewRowsQuantityNumericUpDown
      // 
      this.PreviewRowsQuantityNumericUpDown.Font = new System.Drawing.Font("Segoe UI", 9F);
      this.PreviewRowsQuantityNumericUpDown.Location = new System.Drawing.Point(111, 47);
      this.PreviewRowsQuantityNumericUpDown.Maximum = new decimal(new int[] {
            1000,
            0,
            0,
            0});
      this.PreviewRowsQuantityNumericUpDown.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
      this.PreviewRowsQuantityNumericUpDown.Name = "PreviewRowsQuantityNumericUpDown";
      this.PreviewRowsQuantityNumericUpDown.Size = new System.Drawing.Size(52, 23);
      this.PreviewRowsQuantityNumericUpDown.TabIndex = 14;
      this.HelpToolTip.SetToolTip(this.PreviewRowsQuantityNumericUpDown, "Limits the data preview and data type automatic detection to the given number of " +
        "Excel data rows.");
      this.PreviewRowsQuantityNumericUpDown.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
      // 
      // UseFormattedValuesCheckBox
      // 
      this.UseFormattedValuesCheckBox.AutoSize = true;
      this.UseFormattedValuesCheckBox.BackColor = System.Drawing.Color.Transparent;
      this.UseFormattedValuesCheckBox.Font = new System.Drawing.Font("Segoe UI", 9F);
      this.UseFormattedValuesCheckBox.Location = new System.Drawing.Point(37, 72);
      this.UseFormattedValuesCheckBox.Name = "UseFormattedValuesCheckBox";
      this.UseFormattedValuesCheckBox.Size = new System.Drawing.Size(137, 19);
      this.UseFormattedValuesCheckBox.TabIndex = 16;
      this.UseFormattedValuesCheckBox.Text = "Use formatted values";
      this.HelpToolTip.SetToolTip(this.UseFormattedValuesCheckBox, "If checked it treats dates in Excel as such, otherwise it treats them as numbers." +
        "");
      this.UseFormattedValuesCheckBox.UseVisualStyleBackColor = false;
      // 
      // GenerateMultipleInsertsCheckBox
      // 
      this.GenerateMultipleInsertsCheckBox.AutoSize = true;
      this.GenerateMultipleInsertsCheckBox.Font = new System.Drawing.Font("Segoe UI", 9F);
      this.GenerateMultipleInsertsCheckBox.Location = new System.Drawing.Point(37, 49);
      this.GenerateMultipleInsertsCheckBox.Name = "GenerateMultipleInsertsCheckBox";
      this.GenerateMultipleInsertsCheckBox.Size = new System.Drawing.Size(280, 19);
      this.GenerateMultipleInsertsCheckBox.TabIndex = 16;
      this.GenerateMultipleInsertsCheckBox.Text = "Generate an INSERT statement for each data row";
      this.HelpToolTip.SetToolTip(this.GenerateMultipleInsertsCheckBox, resources.GetString("GenerateMultipleInsertsCheckBox.ToolTip"));
      this.GenerateMultipleInsertsCheckBox.UseVisualStyleBackColor = true;
      this.GenerateMultipleInsertsCheckBox.CheckedChanged += new System.EventHandler(this.GenerateMultipleInsertsCheckBox_CheckedChanged);
      // 
      // DisableTableIndexesCheckBox
      // 
      this.DisableTableIndexesCheckBox.AutoSize = true;
      this.DisableTableIndexesCheckBox.Font = new System.Drawing.Font("Segoe UI", 9F);
      this.DisableTableIndexesCheckBox.Location = new System.Drawing.Point(57, 74);
      this.DisableTableIndexesCheckBox.Name = "DisableTableIndexesCheckBox";
      this.DisableTableIndexesCheckBox.Size = new System.Drawing.Size(279, 19);
      this.DisableTableIndexesCheckBox.TabIndex = 17;
      this.DisableTableIndexesCheckBox.Text = "Disable table indexes to speed-up rows insertion";
      this.HelpToolTip.SetToolTip(this.DisableTableIndexesCheckBox, resources.GetString("DisableTableIndexesCheckBox.ToolTip"));
      this.DisableTableIndexesCheckBox.UseVisualStyleBackColor = true;
      // 
      // ReplaceDuplicatesRadioButton
      // 
      this.ReplaceDuplicatesRadioButton.AutoSize = true;
      this.ReplaceDuplicatesRadioButton.Font = new System.Drawing.Font("Segoe UI", 9F);
      this.ReplaceDuplicatesRadioButton.Location = new System.Drawing.Point(22, 49);
      this.ReplaceDuplicatesRadioButton.Name = "ReplaceDuplicatesRadioButton";
      this.ReplaceDuplicatesRadioButton.Size = new System.Drawing.Size(343, 19);
      this.ReplaceDuplicatesRadioButton.TabIndex = 2;
      this.ReplaceDuplicatesRadioButton.TabStop = true;
      this.ReplaceDuplicatesRadioButton.Text = "Replace the values in the old rows with the ones in new rows";
      this.HelpToolTip.SetToolTip(this.ReplaceDuplicatesRadioButton, resources.GetString("ReplaceDuplicatesRadioButton.ToolTip"));
      this.ReplaceDuplicatesRadioButton.UseVisualStyleBackColor = true;
      // 
      // IgnoreDuplicatesRadioButton
      // 
      this.IgnoreDuplicatesRadioButton.AutoSize = true;
      this.IgnoreDuplicatesRadioButton.Font = new System.Drawing.Font("Segoe UI", 9F);
      this.IgnoreDuplicatesRadioButton.Location = new System.Drawing.Point(22, 26);
      this.IgnoreDuplicatesRadioButton.Name = "IgnoreDuplicatesRadioButton";
      this.IgnoreDuplicatesRadioButton.Size = new System.Drawing.Size(262, 19);
      this.IgnoreDuplicatesRadioButton.TabIndex = 1;
      this.IgnoreDuplicatesRadioButton.TabStop = true;
      this.IgnoreDuplicatesRadioButton.Text = "Ignore rows with duplicate unique key values";
      this.HelpToolTip.SetToolTip(this.IgnoreDuplicatesRadioButton, resources.GetString("IgnoreDuplicatesRadioButton.ToolTip"));
      this.IgnoreDuplicatesRadioButton.UseVisualStyleBackColor = true;
      // 
      // ErrorAndAbortRadioButton
      // 
      this.ErrorAndAbortRadioButton.AutoSize = true;
      this.ErrorAndAbortRadioButton.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.ErrorAndAbortRadioButton.Location = new System.Drawing.Point(22, 3);
      this.ErrorAndAbortRadioButton.Name = "ErrorAndAbortRadioButton";
      this.ErrorAndAbortRadioButton.Size = new System.Drawing.Size(242, 19);
      this.ErrorAndAbortRadioButton.TabIndex = 0;
      this.ErrorAndAbortRadioButton.TabStop = true;
      this.ErrorAndAbortRadioButton.Text = "Error out and abort the append operation";
      this.HelpToolTip.SetToolTip(this.ErrorAndAbortRadioButton, "This is the default behavior, if rows containing duplicate values for unique keys" +
        " are found,\r\nthe MySQL Server will throw an error and the operation will be abor" +
        "ted.");
      this.ErrorAndAbortRadioButton.UseVisualStyleBackColor = true;
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
      this.OptionsTabControl.Controls.Add(this.ColumnsMappingTabPage);
      this.OptionsTabControl.Controls.Add(this.StoredMappingsTabPage);
      this.OptionsTabControl.Controls.Add(this.FieldDataTabPage);
      this.OptionsTabControl.Controls.Add(this.SqlQueriesTabPage);
      this.OptionsTabControl.Dock = System.Windows.Forms.DockStyle.Fill;
      this.OptionsTabControl.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.OptionsTabControl.Location = new System.Drawing.Point(0, 0);
      this.OptionsTabControl.Name = "OptionsTabControl";
      this.OptionsTabControl.SelectedIndex = 0;
      this.OptionsTabControl.Size = new System.Drawing.Size(484, 300);
      this.OptionsTabControl.TabIndex = 19;
      // 
      // ColumnsMappingTabPage
      // 
      this.ColumnsMappingTabPage.Controls.Add(this.ConfirmMappingOverwritingCheckBox);
      this.ColumnsMappingTabPage.Controls.Add(this.ReloadColumnMappingCheckBox);
      this.ColumnsMappingTabPage.Controls.Add(this.AutoStoreColumnMappingCheckBox);
      this.ColumnsMappingTabPage.Controls.Add(this.DoNotPerformAutoMapCheckBox);
      this.ColumnsMappingTabPage.Controls.Add(this.MappingOptionsLabel);
      this.ColumnsMappingTabPage.Location = new System.Drawing.Point(4, 24);
      this.ColumnsMappingTabPage.Name = "ColumnsMappingTabPage";
      this.ColumnsMappingTabPage.Padding = new System.Windows.Forms.Padding(3);
      this.ColumnsMappingTabPage.Size = new System.Drawing.Size(476, 272);
      this.ColumnsMappingTabPage.TabIndex = 0;
      this.ColumnsMappingTabPage.Text = "Columns Mapping";
      this.ColumnsMappingTabPage.UseVisualStyleBackColor = true;
      // 
      // MappingOptionsLabel
      // 
      this.MappingOptionsLabel.AutoSize = true;
      this.MappingOptionsLabel.BackColor = System.Drawing.Color.Transparent;
      this.MappingOptionsLabel.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.MappingOptionsLabel.ForeColor = System.Drawing.SystemColors.ControlText;
      this.MappingOptionsLabel.Location = new System.Drawing.Point(8, 20);
      this.MappingOptionsLabel.Name = "MappingOptionsLabel";
      this.MappingOptionsLabel.Size = new System.Drawing.Size(165, 17);
      this.MappingOptionsLabel.TabIndex = 6;
      this.MappingOptionsLabel.Text = "Columns Mapping Options";
      // 
      // StoredMappingsTabPage
      // 
      this.StoredMappingsTabPage.Controls.Add(this.MappingsListView);
      this.StoredMappingsTabPage.Controls.Add(this.DeleteMappingButton);
      this.StoredMappingsTabPage.Controls.Add(this.RenameMappingButton);
      this.StoredMappingsTabPage.Controls.Add(this.StoredColumnMappingsLabel);
      this.StoredMappingsTabPage.Location = new System.Drawing.Point(4, 24);
      this.StoredMappingsTabPage.Name = "StoredMappingsTabPage";
      this.StoredMappingsTabPage.Size = new System.Drawing.Size(476, 272);
      this.StoredMappingsTabPage.TabIndex = 3;
      this.StoredMappingsTabPage.Text = "Stored Mappings";
      this.StoredMappingsTabPage.UseVisualStyleBackColor = true;
      // 
      // MappingsListView
      // 
      this.MappingsListView.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
      this.MappingsListView.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader1,
            this.columnHeader2});
      this.MappingsListView.FullRowSelect = true;
      this.MappingsListView.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.None;
      this.MappingsListView.Location = new System.Drawing.Point(37, 50);
      this.MappingsListView.MultiSelect = false;
      this.MappingsListView.Name = "MappingsListView";
      this.MappingsListView.Size = new System.Drawing.Size(350, 163);
      this.MappingsListView.TabIndex = 20;
      this.MappingsListView.UseCompatibleStateImageBehavior = false;
      this.MappingsListView.View = System.Windows.Forms.View.Details;
      this.MappingsListView.SelectedIndexChanged += new System.EventHandler(this.MappingsListView_SelectedIndexChanged);
      // 
      // columnHeader1
      // 
      this.columnHeader1.Text = "";
      this.columnHeader1.Width = 265;
      // 
      // columnHeader2
      // 
      this.columnHeader2.Text = "";
      this.columnHeader2.Width = 0;
      // 
      // DeleteMappingButton
      // 
      this.DeleteMappingButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
      this.DeleteMappingButton.Enabled = false;
      this.DeleteMappingButton.Font = new System.Drawing.Font("Segoe UI", 9F);
      this.DeleteMappingButton.Location = new System.Drawing.Point(393, 79);
      this.DeleteMappingButton.Name = "DeleteMappingButton";
      this.DeleteMappingButton.Size = new System.Drawing.Size(75, 23);
      this.DeleteMappingButton.TabIndex = 22;
      this.DeleteMappingButton.Text = "Delete";
      this.DeleteMappingButton.UseVisualStyleBackColor = true;
      this.DeleteMappingButton.Click += new System.EventHandler(this.DeleteMappingButton_Click);
      // 
      // RenameMappingButton
      // 
      this.RenameMappingButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
      this.RenameMappingButton.Enabled = false;
      this.RenameMappingButton.Font = new System.Drawing.Font("Segoe UI", 9F);
      this.RenameMappingButton.Location = new System.Drawing.Point(393, 50);
      this.RenameMappingButton.Name = "RenameMappingButton";
      this.RenameMappingButton.Size = new System.Drawing.Size(75, 23);
      this.RenameMappingButton.TabIndex = 21;
      this.RenameMappingButton.Text = "Rename";
      this.RenameMappingButton.UseVisualStyleBackColor = true;
      this.RenameMappingButton.Click += new System.EventHandler(this.RenameMappingButton_Click);
      // 
      // StoredColumnMappingsLabel
      // 
      this.StoredColumnMappingsLabel.AutoSize = true;
      this.StoredColumnMappingsLabel.BackColor = System.Drawing.Color.Transparent;
      this.StoredColumnMappingsLabel.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.StoredColumnMappingsLabel.ForeColor = System.Drawing.SystemColors.ControlText;
      this.StoredColumnMappingsLabel.Location = new System.Drawing.Point(8, 20);
      this.StoredColumnMappingsLabel.Name = "StoredColumnMappingsLabel";
      this.StoredColumnMappingsLabel.Size = new System.Drawing.Size(158, 17);
      this.StoredColumnMappingsLabel.TabIndex = 19;
      this.StoredColumnMappingsLabel.Text = "Stored Column Mappings";
      // 
      // FieldDataTabPage
      // 
      this.FieldDataTabPage.Controls.Add(this.ShowDataTypesCheckBox);
      this.FieldDataTabPage.Controls.Add(this.FieldDataOptionsLabel);
      this.FieldDataTabPage.Controls.Add(this.UseFormattedValuesCheckBox);
      this.FieldDataTabPage.Controls.Add(this.PreviewRowsQuantityNumericUpDown);
      this.FieldDataTabPage.Controls.Add(this.PreviewRowsQuantity1Label);
      this.FieldDataTabPage.Controls.Add(this.PreviewRowsQuantity2Label);
      this.FieldDataTabPage.Location = new System.Drawing.Point(4, 24);
      this.FieldDataTabPage.Name = "FieldDataTabPage";
      this.FieldDataTabPage.Padding = new System.Windows.Forms.Padding(3);
      this.FieldDataTabPage.Size = new System.Drawing.Size(476, 272);
      this.FieldDataTabPage.TabIndex = 1;
      this.FieldDataTabPage.Text = "Field Data";
      this.FieldDataTabPage.UseVisualStyleBackColor = true;
      // 
      // FieldDataOptionsLabel
      // 
      this.FieldDataOptionsLabel.AutoSize = true;
      this.FieldDataOptionsLabel.BackColor = System.Drawing.Color.Transparent;
      this.FieldDataOptionsLabel.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.FieldDataOptionsLabel.ForeColor = System.Drawing.SystemColors.ControlText;
      this.FieldDataOptionsLabel.Location = new System.Drawing.Point(8, 20);
      this.FieldDataOptionsLabel.Name = "FieldDataOptionsLabel";
      this.FieldDataOptionsLabel.Size = new System.Drawing.Size(116, 17);
      this.FieldDataOptionsLabel.TabIndex = 12;
      this.FieldDataOptionsLabel.Text = "Field Data Options";
      // 
      // PreviewRowsQuantity1Label
      // 
      this.PreviewRowsQuantity1Label.AutoSize = true;
      this.PreviewRowsQuantity1Label.Font = new System.Drawing.Font("Segoe UI", 9F);
      this.PreviewRowsQuantity1Label.Location = new System.Drawing.Point(34, 49);
      this.PreviewRowsQuantity1Label.Name = "PreviewRowsQuantity1Label";
      this.PreviewRowsQuantity1Label.Size = new System.Drawing.Size(69, 15);
      this.PreviewRowsQuantity1Label.TabIndex = 13;
      this.PreviewRowsQuantity1Label.Text = "Use the first";
      // 
      // PreviewRowsQuantity2Label
      // 
      this.PreviewRowsQuantity2Label.AutoSize = true;
      this.PreviewRowsQuantity2Label.Font = new System.Drawing.Font("Segoe UI", 9F);
      this.PreviewRowsQuantity2Label.Location = new System.Drawing.Point(169, 49);
      this.PreviewRowsQuantity2Label.Name = "PreviewRowsQuantity2Label";
      this.PreviewRowsQuantity2Label.Size = new System.Drawing.Size(275, 15);
      this.PreviewRowsQuantity2Label.TabIndex = 15;
      this.PreviewRowsQuantity2Label.Text = "Excel data rows to preview and calculate datatypes.";
      // 
      // SqlQueriesTabPage
      // 
      this.SqlQueriesTabPage.Controls.Add(this.DuplicateValuesOptionsPanel);
      this.SqlQueriesTabPage.Controls.Add(this.DuplicateConflictsLabel);
      this.SqlQueriesTabPage.Controls.Add(this.GenerateMultipleInsertsCheckBox);
      this.SqlQueriesTabPage.Controls.Add(this.DisableTableIndexesCheckBox);
      this.SqlQueriesTabPage.Controls.Add(this.SqlQueriesLabel);
      this.SqlQueriesTabPage.Location = new System.Drawing.Point(4, 24);
      this.SqlQueriesTabPage.Name = "SqlQueriesTabPage";
      this.SqlQueriesTabPage.Size = new System.Drawing.Size(476, 272);
      this.SqlQueriesTabPage.TabIndex = 2;
      this.SqlQueriesTabPage.Text = "SQL Queries";
      this.SqlQueriesTabPage.UseVisualStyleBackColor = true;
      // 
      // DuplicateValuesOptionsPanel
      // 
      this.DuplicateValuesOptionsPanel.Controls.Add(this.ReplaceDuplicatesRadioButton);
      this.DuplicateValuesOptionsPanel.Controls.Add(this.IgnoreDuplicatesRadioButton);
      this.DuplicateValuesOptionsPanel.Controls.Add(this.ErrorAndAbortRadioButton);
      this.DuplicateValuesOptionsPanel.Location = new System.Drawing.Point(15, 119);
      this.DuplicateValuesOptionsPanel.Name = "DuplicateValuesOptionsPanel";
      this.DuplicateValuesOptionsPanel.Size = new System.Drawing.Size(427, 73);
      this.DuplicateValuesOptionsPanel.TabIndex = 19;
      // 
      // DuplicateConflictsLabel
      // 
      this.DuplicateConflictsLabel.AutoSize = true;
      this.DuplicateConflictsLabel.Font = new System.Drawing.Font("Segoe UI", 9F);
      this.DuplicateConflictsLabel.Location = new System.Drawing.Point(34, 101);
      this.DuplicateConflictsLabel.Name = "DuplicateConflictsLabel";
      this.DuplicateConflictsLabel.Size = new System.Drawing.Size(358, 15);
      this.DuplicateConflictsLabel.TabIndex = 18;
      this.DuplicateConflictsLabel.Text = "When new rows contain unique key values that duplicate old rows:";
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
      this.SqlQueriesLabel.TabIndex = 15;
      this.SqlQueriesLabel.Text = "SQL Queries Options";
      // 
      // AppendAdvancedOptionsDialog
      // 
      this.AcceptButton = this.DialogAcceptButton;
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.CancelButton = this.DialogCancelButton;
      this.ClientSize = new System.Drawing.Size(484, 300);
      this.CommandAreaVisible = true;
      this.FootnoteAreaHeight = 0;
      this.MainInstructionLocation = new System.Drawing.Point(13, 21);
      this.Name = "AppendAdvancedOptionsDialog";
      this.Text = "Advanced Append Data Options";
      this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.AppendAdvancedOptionsDialog_FormClosing);
      this.ContentAreaPanel.ResumeLayout(false);
      this.CommandAreaPanel.ResumeLayout(false);
      ((System.ComponentModel.ISupportInitialize)(this.PreviewRowsQuantityNumericUpDown)).EndInit();
      this.OptionsTabControl.ResumeLayout(false);
      this.ColumnsMappingTabPage.ResumeLayout(false);
      this.ColumnsMappingTabPage.PerformLayout();
      this.StoredMappingsTabPage.ResumeLayout(false);
      this.StoredMappingsTabPage.PerformLayout();
      this.FieldDataTabPage.ResumeLayout(false);
      this.FieldDataTabPage.PerformLayout();
      this.SqlQueriesTabPage.ResumeLayout(false);
      this.SqlQueriesTabPage.PerformLayout();
      this.DuplicateValuesOptionsPanel.ResumeLayout(false);
      this.DuplicateValuesOptionsPanel.PerformLayout();
      this.ResumeLayout(false);

    }

    #endregion

    private System.Windows.Forms.Button DialogAcceptButton;
    private System.Windows.Forms.Button DialogCancelButton;
    private System.Windows.Forms.ToolTip HelpToolTip;
    private System.Windows.Forms.Button ResetToDefaultsButton;
    private System.Windows.Forms.TabControl OptionsTabControl;
    private System.Windows.Forms.TabPage ColumnsMappingTabPage;
    private System.Windows.Forms.TabPage FieldDataTabPage;
    private System.Windows.Forms.TabPage StoredMappingsTabPage;
    private System.Windows.Forms.TabPage SqlQueriesTabPage;
    private System.Windows.Forms.CheckBox ConfirmMappingOverwritingCheckBox;
    private System.Windows.Forms.CheckBox ReloadColumnMappingCheckBox;
    private System.Windows.Forms.CheckBox AutoStoreColumnMappingCheckBox;
    private System.Windows.Forms.CheckBox DoNotPerformAutoMapCheckBox;
    private System.Windows.Forms.Label MappingOptionsLabel;
    private System.Windows.Forms.ListView MappingsListView;
    private System.Windows.Forms.ColumnHeader columnHeader1;
    private System.Windows.Forms.ColumnHeader columnHeader2;
    private System.Windows.Forms.Button DeleteMappingButton;
    private System.Windows.Forms.Button RenameMappingButton;
    private System.Windows.Forms.Label StoredColumnMappingsLabel;
    private System.Windows.Forms.CheckBox ShowDataTypesCheckBox;
    private System.Windows.Forms.Label FieldDataOptionsLabel;
    private System.Windows.Forms.CheckBox UseFormattedValuesCheckBox;
    private System.Windows.Forms.NumericUpDown PreviewRowsQuantityNumericUpDown;
    private System.Windows.Forms.Label PreviewRowsQuantity1Label;
    private System.Windows.Forms.Label PreviewRowsQuantity2Label;
    private System.Windows.Forms.CheckBox GenerateMultipleInsertsCheckBox;
    private System.Windows.Forms.CheckBox DisableTableIndexesCheckBox;
    private System.Windows.Forms.Label SqlQueriesLabel;
    private System.Windows.Forms.Label DuplicateConflictsLabel;
    private System.Windows.Forms.Panel DuplicateValuesOptionsPanel;
    private System.Windows.Forms.RadioButton ReplaceDuplicatesRadioButton;
    private System.Windows.Forms.RadioButton IgnoreDuplicatesRadioButton;
    private System.Windows.Forms.RadioButton ErrorAndAbortRadioButton;
  }
}