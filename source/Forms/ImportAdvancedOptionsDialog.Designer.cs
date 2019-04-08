// Copyright (c) 2013, 2017, Oracle and/or its affiliates. All rights reserved.
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
  partial class ImportAdvancedOptionsDialog
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
      System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ImportAdvancedOptionsDialog));
      this.DialogAcceptButton = new System.Windows.Forms.Button();
      this.DialogCancelButton = new System.Windows.Forms.Button();
      this.HelpToolTip = new System.Windows.Forms.ToolTip(this.components);
      this.PreviewRowsQuantityNumericUpDown = new System.Windows.Forms.NumericUpDown();
      this.EscapeFormulaValuesCheckBox = new System.Windows.Forms.CheckBox();
      this.FormatTimeTextBox = new System.Windows.Forms.TextBox();
      this.FormatShortDatesTextBox = new System.Windows.Forms.TextBox();
      this.FormatLongDatesTextBox = new System.Windows.Forms.TextBox();
      this.PrefixExcelTablesCheckBox = new System.Windows.Forms.CheckBox();
      this.PrefixExcelTablesTextBox = new System.Windows.Forms.TextBox();
      this.UseStyleComboBox = new System.Windows.Forms.ComboBox();
      this.CreateExcelTableCheckbox = new System.Windows.Forms.CheckBox();
      this.FloatingPointDataAsDecimalCheckBox = new System.Windows.Forms.CheckBox();
      this.ResetToDefaultsButton = new System.Windows.Forms.Button();
      this.OptionsTabControl = new System.Windows.Forms.TabControl();
      this.GeneralTabPage = new System.Windows.Forms.TabPage();
      this.PreviewRowsQuantity2Label = new System.Windows.Forms.Label();
      this.PreviewRowsQuantity1Label = new System.Windows.Forms.Label();
      this.GeneralOptionsLabel = new System.Windows.Forms.Label();
      this.FormattingTabPage = new System.Windows.Forms.TabPage();
      this.FormatTimeLabel = new System.Windows.Forms.Label();
      this.FormatShortDatesLabel = new System.Windows.Forms.Label();
      this.FormatLongDatesLabel = new System.Windows.Forms.Label();
      this.FormattingOptionsLabel = new System.Windows.Forms.Label();
      this.ExcelTablesTabPage = new System.Windows.Forms.TabPage();
      this.UseStyle2Label = new System.Windows.Forms.Label();
      this.UseStyle1Label = new System.Windows.Forms.Label();
      this.ExcelTableOptionsLabel = new System.Windows.Forms.Label();
      this.ContentAreaPanel.SuspendLayout();
      this.CommandAreaPanel.SuspendLayout();
      ((System.ComponentModel.ISupportInitialize)(this.PreviewRowsQuantityNumericUpDown)).BeginInit();
      this.OptionsTabControl.SuspendLayout();
      this.GeneralTabPage.SuspendLayout();
      this.FormattingTabPage.SuspendLayout();
      this.ExcelTablesTabPage.SuspendLayout();
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
      this.ContentAreaPanel.Size = new System.Drawing.Size(551, 276);
      // 
      // CommandAreaPanel
      // 
      this.CommandAreaPanel.Controls.Add(this.ResetToDefaultsButton);
      this.CommandAreaPanel.Controls.Add(this.DialogAcceptButton);
      this.CommandAreaPanel.Controls.Add(this.DialogCancelButton);
      this.CommandAreaPanel.Location = new System.Drawing.Point(0, 231);
      this.CommandAreaPanel.Size = new System.Drawing.Size(551, 45);
      // 
      // DialogAcceptButton
      // 
      this.DialogAcceptButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
      this.DialogAcceptButton.DialogResult = System.Windows.Forms.DialogResult.OK;
      this.DialogAcceptButton.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.DialogAcceptButton.Location = new System.Drawing.Point(383, 11);
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
      this.DialogCancelButton.Font = new System.Drawing.Font("Segoe UI", 8.25F);
      this.DialogCancelButton.Location = new System.Drawing.Point(464, 11);
      this.DialogCancelButton.Name = "DialogCancelButton";
      this.DialogCancelButton.Size = new System.Drawing.Size(75, 23);
      this.DialogCancelButton.TabIndex = 1;
      this.DialogCancelButton.Text = "Cancel";
      this.DialogCancelButton.UseVisualStyleBackColor = true;
      // 
      // HelpToolTip
      // 
      this.HelpToolTip.AutomaticDelay = 2000;
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
      this.PreviewRowsQuantityNumericUpDown.TabIndex = 2;
      this.HelpToolTip.SetToolTip(this.PreviewRowsQuantityNumericUpDown, "Limits the data preview to the given number of Excel data rows.");
      this.PreviewRowsQuantityNumericUpDown.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
      // 
      // EscapeFormulaValuesCheckBox
      // 
      this.EscapeFormulaValuesCheckBox.AutoSize = true;
      this.EscapeFormulaValuesCheckBox.BackColor = System.Drawing.Color.Transparent;
      this.EscapeFormulaValuesCheckBox.Font = new System.Drawing.Font("Segoe UI", 9F);
      this.EscapeFormulaValuesCheckBox.Location = new System.Drawing.Point(37, 76);
      this.EscapeFormulaValuesCheckBox.Name = "EscapeFormulaValuesCheckBox";
      this.EscapeFormulaValuesCheckBox.Size = new System.Drawing.Size(432, 19);
      this.EscapeFormulaValuesCheckBox.TabIndex = 4;
      this.EscapeFormulaValuesCheckBox.Text = "Escape text values that start with \"=\" so Excel does not treat them as formulas";
      this.HelpToolTip.SetToolTip(this.EscapeFormulaValuesCheckBox, "When checked the equals signs found at the start of text values in the importing " +
        "MySQL data are removed so they are not treated as formulas by Excel.");
      this.EscapeFormulaValuesCheckBox.UseVisualStyleBackColor = false;
      // 
      // FormatTimeTextBox
      // 
      this.FormatTimeTextBox.Font = new System.Drawing.Font("Segoe UI", 9F);
      this.FormatTimeTextBox.Location = new System.Drawing.Point(234, 104);
      this.FormatTimeTextBox.Name = "FormatTimeTextBox";
      this.FormatTimeTextBox.Size = new System.Drawing.Size(124, 23);
      this.FormatTimeTextBox.TabIndex = 6;
      this.HelpToolTip.SetToolTip(this.FormatTimeTextBox, "The Excel format string to be used for Time MySQL data.");
      // 
      // FormatShortDatesTextBox
      // 
      this.FormatShortDatesTextBox.Font = new System.Drawing.Font("Segoe UI", 9F);
      this.FormatShortDatesTextBox.Location = new System.Drawing.Point(231, 75);
      this.FormatShortDatesTextBox.Name = "FormatShortDatesTextBox";
      this.FormatShortDatesTextBox.Size = new System.Drawing.Size(124, 23);
      this.FormatShortDatesTextBox.TabIndex = 4;
      this.HelpToolTip.SetToolTip(this.FormatShortDatesTextBox, "The Excel format string to be used for Date MySQL data.\r\nA short date format is s" +
        "uitable in this case.");
      // 
      // FormatLongDatesTextBox
      // 
      this.FormatLongDatesTextBox.Font = new System.Drawing.Font("Segoe UI", 9F);
      this.FormatLongDatesTextBox.Location = new System.Drawing.Point(345, 46);
      this.FormatLongDatesTextBox.Name = "FormatLongDatesTextBox";
      this.FormatLongDatesTextBox.Size = new System.Drawing.Size(124, 23);
      this.FormatLongDatesTextBox.TabIndex = 2;
      this.HelpToolTip.SetToolTip(this.FormatLongDatesTextBox, "The Excel format string to be used for DateTime and TimeStamp MySQL data.\r\nA long" +
        " date format is suitable in this case.");
      // 
      // PrefixExcelTablesCheckBox
      // 
      this.PrefixExcelTablesCheckBox.AutoSize = true;
      this.PrefixExcelTablesCheckBox.Font = new System.Drawing.Font("Segoe UI", 9F);
      this.PrefixExcelTablesCheckBox.Location = new System.Drawing.Point(57, 104);
      this.PrefixExcelTablesCheckBox.Name = "PrefixExcelTablesCheckBox";
      this.PrefixExcelTablesCheckBox.Size = new System.Drawing.Size(242, 19);
      this.PrefixExcelTablesCheckBox.TabIndex = 5;
      this.PrefixExcelTablesCheckBox.Text = "Prefix Excel tables with the following text:";
      this.HelpToolTip.SetToolTip(this.PrefixExcelTablesCheckBox, "When checked the specified text will be used to prefix the names of created Excel" +
        " tables.");
      this.PrefixExcelTablesCheckBox.UseVisualStyleBackColor = true;
      this.PrefixExcelTablesCheckBox.CheckedChanged += new System.EventHandler(this.PrefixExcelTablesCheckBox_CheckedChanged);
      // 
      // PrefixExcelTablesTextBox
      // 
      this.PrefixExcelTablesTextBox.Font = new System.Drawing.Font("Segoe UI", 9F);
      this.PrefixExcelTablesTextBox.Location = new System.Drawing.Point(305, 102);
      this.PrefixExcelTablesTextBox.Name = "PrefixExcelTablesTextBox";
      this.PrefixExcelTablesTextBox.Size = new System.Drawing.Size(169, 23);
      this.PrefixExcelTablesTextBox.TabIndex = 6;
      this.HelpToolTip.SetToolTip(this.PrefixExcelTablesTextBox, "A custom prefix to add to Excel tables created by MySQL for Excel.");
      // 
      // UseStyleComboBox
      // 
      this.UseStyleComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
      this.UseStyleComboBox.Font = new System.Drawing.Font("Segoe UI", 9F);
      this.UseStyleComboBox.FormattingEnabled = true;
      this.UseStyleComboBox.Location = new System.Drawing.Point(111, 73);
      this.UseStyleComboBox.Name = "UseStyleComboBox";
      this.UseStyleComboBox.Size = new System.Drawing.Size(188, 23);
      this.UseStyleComboBox.TabIndex = 3;
      this.HelpToolTip.SetToolTip(this.UseStyleComboBox, "You can quickly format table data by applying a predefined or custom table style." +
        "");
      // 
      // CreateExcelTableCheckbox
      // 
      this.CreateExcelTableCheckbox.AutoSize = true;
      this.CreateExcelTableCheckbox.BackColor = System.Drawing.Color.Transparent;
      this.CreateExcelTableCheckbox.Font = new System.Drawing.Font("Segoe UI", 9F);
      this.CreateExcelTableCheckbox.Location = new System.Drawing.Point(37, 48);
      this.CreateExcelTableCheckbox.Name = "CreateExcelTableCheckbox";
      this.CreateExcelTableCheckbox.Size = new System.Drawing.Size(294, 19);
      this.CreateExcelTableCheckbox.TabIndex = 1;
      this.CreateExcelTableCheckbox.Text = "Create an Excel table for the imported MySQL data.";
      this.HelpToolTip.SetToolTip(this.CreateExcelTableCheckbox, "When checked an Excel table (ListObject in VBA) will be created for the imported " +
        "data.");
      this.CreateExcelTableCheckbox.UseVisualStyleBackColor = false;
      this.CreateExcelTableCheckbox.CheckedChanged += new System.EventHandler(this.CreateExcelTableCheckbox_CheckedChanged);
      // 
      // FloatingPointDataAsDecimalCheckBox
      // 
      this.FloatingPointDataAsDecimalCheckBox.AutoSize = true;
      this.FloatingPointDataAsDecimalCheckBox.Location = new System.Drawing.Point(34, 133);
      this.FloatingPointDataAsDecimalCheckBox.Name = "FloatingPointDataAsDecimalCheckBox";
      this.FloatingPointDataAsDecimalCheckBox.Size = new System.Drawing.Size(326, 19);
      this.FloatingPointDataAsDecimalCheckBox.TabIndex = 7;
      this.FloatingPointDataAsDecimalCheckBox.Text = "Import all floating-point data using a DECIMAL data type";
      this.HelpToolTip.SetToolTip(this.FloatingPointDataAsDecimalCheckBox, resources.GetString("FloatingPointDataAsDecimalCheckBox.ToolTip"));
      this.FloatingPointDataAsDecimalCheckBox.UseVisualStyleBackColor = true;
      // 
      // ResetToDefaultsButton
      // 
      this.ResetToDefaultsButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
      this.ResetToDefaultsButton.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.ResetToDefaultsButton.Location = new System.Drawing.Point(12, 11);
      this.ResetToDefaultsButton.Name = "ResetToDefaultsButton";
      this.ResetToDefaultsButton.Size = new System.Drawing.Size(133, 23);
      this.ResetToDefaultsButton.TabIndex = 2;
      this.ResetToDefaultsButton.Text = "Reset to Defaults";
      this.ResetToDefaultsButton.UseVisualStyleBackColor = true;
      this.ResetToDefaultsButton.Click += new System.EventHandler(this.ResetToDefaultsButton_Click);
      // 
      // OptionsTabControl
      // 
      this.OptionsTabControl.Controls.Add(this.GeneralTabPage);
      this.OptionsTabControl.Controls.Add(this.FormattingTabPage);
      this.OptionsTabControl.Controls.Add(this.ExcelTablesTabPage);
      this.OptionsTabControl.Dock = System.Windows.Forms.DockStyle.Fill;
      this.OptionsTabControl.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.OptionsTabControl.Location = new System.Drawing.Point(0, 0);
      this.OptionsTabControl.Name = "OptionsTabControl";
      this.OptionsTabControl.SelectedIndex = 0;
      this.OptionsTabControl.Size = new System.Drawing.Size(551, 276);
      this.OptionsTabControl.TabIndex = 0;
      // 
      // GeneralTabPage
      // 
      this.GeneralTabPage.Controls.Add(this.PreviewRowsQuantityNumericUpDown);
      this.GeneralTabPage.Controls.Add(this.PreviewRowsQuantity2Label);
      this.GeneralTabPage.Controls.Add(this.PreviewRowsQuantity1Label);
      this.GeneralTabPage.Controls.Add(this.EscapeFormulaValuesCheckBox);
      this.GeneralTabPage.Controls.Add(this.GeneralOptionsLabel);
      this.GeneralTabPage.Location = new System.Drawing.Point(4, 24);
      this.GeneralTabPage.Name = "GeneralTabPage";
      this.GeneralTabPage.Padding = new System.Windows.Forms.Padding(3);
      this.GeneralTabPage.Size = new System.Drawing.Size(543, 248);
      this.GeneralTabPage.TabIndex = 0;
      this.GeneralTabPage.Text = "General";
      this.GeneralTabPage.UseVisualStyleBackColor = true;
      // 
      // PreviewRowsQuantity2Label
      // 
      this.PreviewRowsQuantity2Label.AutoSize = true;
      this.PreviewRowsQuantity2Label.Font = new System.Drawing.Font("Segoe UI", 9F);
      this.PreviewRowsQuantity2Label.Location = new System.Drawing.Point(169, 49);
      this.PreviewRowsQuantity2Label.Name = "PreviewRowsQuantity2Label";
      this.PreviewRowsQuantity2Label.Size = new System.Drawing.Size(217, 15);
      this.PreviewRowsQuantity2Label.TabIndex = 3;
      this.PreviewRowsQuantity2Label.Text = "rows to preview the MySQL table\'s data.";
      // 
      // PreviewRowsQuantity1Label
      // 
      this.PreviewRowsQuantity1Label.AutoSize = true;
      this.PreviewRowsQuantity1Label.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.PreviewRowsQuantity1Label.Location = new System.Drawing.Point(34, 49);
      this.PreviewRowsQuantity1Label.Name = "PreviewRowsQuantity1Label";
      this.PreviewRowsQuantity1Label.Size = new System.Drawing.Size(69, 15);
      this.PreviewRowsQuantity1Label.TabIndex = 1;
      this.PreviewRowsQuantity1Label.Text = "Use the first";
      // 
      // GeneralOptionsLabel
      // 
      this.GeneralOptionsLabel.AutoSize = true;
      this.GeneralOptionsLabel.BackColor = System.Drawing.Color.Transparent;
      this.GeneralOptionsLabel.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.GeneralOptionsLabel.ForeColor = System.Drawing.SystemColors.ControlText;
      this.GeneralOptionsLabel.Location = new System.Drawing.Point(8, 20);
      this.GeneralOptionsLabel.Name = "GeneralOptionsLabel";
      this.GeneralOptionsLabel.Size = new System.Drawing.Size(103, 17);
      this.GeneralOptionsLabel.TabIndex = 0;
      this.GeneralOptionsLabel.Text = "General Options";
      // 
      // FormattingTabPage
      // 
      this.FormattingTabPage.Controls.Add(this.FloatingPointDataAsDecimalCheckBox);
      this.FormattingTabPage.Controls.Add(this.FormatTimeLabel);
      this.FormattingTabPage.Controls.Add(this.FormatTimeTextBox);
      this.FormattingTabPage.Controls.Add(this.FormatShortDatesLabel);
      this.FormattingTabPage.Controls.Add(this.FormatShortDatesTextBox);
      this.FormattingTabPage.Controls.Add(this.FormatLongDatesTextBox);
      this.FormattingTabPage.Controls.Add(this.FormatLongDatesLabel);
      this.FormattingTabPage.Controls.Add(this.FormattingOptionsLabel);
      this.FormattingTabPage.Location = new System.Drawing.Point(4, 24);
      this.FormattingTabPage.Name = "FormattingTabPage";
      this.FormattingTabPage.Padding = new System.Windows.Forms.Padding(3);
      this.FormattingTabPage.Size = new System.Drawing.Size(543, 248);
      this.FormattingTabPage.TabIndex = 1;
      this.FormattingTabPage.Text = "Formatting";
      this.FormattingTabPage.UseVisualStyleBackColor = true;
      // 
      // FormatTimeLabel
      // 
      this.FormatTimeLabel.AutoSize = true;
      this.FormatTimeLabel.Font = new System.Drawing.Font("Segoe UI", 9F);
      this.FormatTimeLabel.Location = new System.Drawing.Point(34, 107);
      this.FormatTimeLabel.Name = "FormatTimeLabel";
      this.FormatTimeLabel.Size = new System.Drawing.Size(194, 15);
      this.FormatTimeLabel.TabIndex = 5;
      this.FormatTimeLabel.Text = "Excel number format for Time data:";
      // 
      // FormatShortDatesLabel
      // 
      this.FormatShortDatesLabel.AutoSize = true;
      this.FormatShortDatesLabel.Font = new System.Drawing.Font("Segoe UI", 9F);
      this.FormatShortDatesLabel.Location = new System.Drawing.Point(34, 78);
      this.FormatShortDatesLabel.Name = "FormatShortDatesLabel";
      this.FormatShortDatesLabel.Size = new System.Drawing.Size(191, 15);
      this.FormatShortDatesLabel.TabIndex = 3;
      this.FormatShortDatesLabel.Text = "Excel number format for Date data:";
      // 
      // FormatLongDatesLabel
      // 
      this.FormatLongDatesLabel.AutoSize = true;
      this.FormatLongDatesLabel.Font = new System.Drawing.Font("Segoe UI", 9F);
      this.FormatLongDatesLabel.Location = new System.Drawing.Point(34, 49);
      this.FormatLongDatesLabel.Name = "FormatLongDatesLabel";
      this.FormatLongDatesLabel.Size = new System.Drawing.Size(305, 15);
      this.FormatLongDatesLabel.TabIndex = 1;
      this.FormatLongDatesLabel.Text = "Excel number format for DateTime and TimeStamp data:";
      // 
      // FormattingOptionsLabel
      // 
      this.FormattingOptionsLabel.AutoSize = true;
      this.FormattingOptionsLabel.BackColor = System.Drawing.Color.Transparent;
      this.FormattingOptionsLabel.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.FormattingOptionsLabel.ForeColor = System.Drawing.SystemColors.ControlText;
      this.FormattingOptionsLabel.Location = new System.Drawing.Point(8, 20);
      this.FormattingOptionsLabel.Name = "FormattingOptionsLabel";
      this.FormattingOptionsLabel.Size = new System.Drawing.Size(121, 17);
      this.FormattingOptionsLabel.TabIndex = 0;
      this.FormattingOptionsLabel.Text = "Formatting Options";
      // 
      // ExcelTablesTabPage
      // 
      this.ExcelTablesTabPage.Controls.Add(this.PrefixExcelTablesCheckBox);
      this.ExcelTablesTabPage.Controls.Add(this.PrefixExcelTablesTextBox);
      this.ExcelTablesTabPage.Controls.Add(this.UseStyle2Label);
      this.ExcelTablesTabPage.Controls.Add(this.UseStyleComboBox);
      this.ExcelTablesTabPage.Controls.Add(this.UseStyle1Label);
      this.ExcelTablesTabPage.Controls.Add(this.CreateExcelTableCheckbox);
      this.ExcelTablesTabPage.Controls.Add(this.ExcelTableOptionsLabel);
      this.ExcelTablesTabPage.Location = new System.Drawing.Point(4, 24);
      this.ExcelTablesTabPage.Name = "ExcelTablesTabPage";
      this.ExcelTablesTabPage.Size = new System.Drawing.Size(543, 248);
      this.ExcelTablesTabPage.TabIndex = 2;
      this.ExcelTablesTabPage.Text = "Excel Tables";
      this.ExcelTablesTabPage.UseVisualStyleBackColor = true;
      // 
      // UseStyle2Label
      // 
      this.UseStyle2Label.AutoSize = true;
      this.UseStyle2Label.Font = new System.Drawing.Font("Segoe UI", 9F);
      this.UseStyle2Label.Location = new System.Drawing.Point(305, 76);
      this.UseStyle2Label.Name = "UseStyle2Label";
      this.UseStyle2Label.Size = new System.Drawing.Size(128, 15);
      this.UseStyle2Label.TabIndex = 4;
      this.UseStyle2Label.Text = "for the new Excel table.";
      // 
      // UseStyle1Label
      // 
      this.UseStyle1Label.AutoSize = true;
      this.UseStyle1Label.Font = new System.Drawing.Font("Segoe UI", 9F);
      this.UseStyle1Label.Location = new System.Drawing.Point(54, 76);
      this.UseStyle1Label.Name = "UseStyle1Label";
      this.UseStyle1Label.Size = new System.Drawing.Size(53, 15);
      this.UseStyle1Label.TabIndex = 2;
      this.UseStyle1Label.Text = "Use style";
      // 
      // ExcelTableOptionsLabel
      // 
      this.ExcelTableOptionsLabel.AutoSize = true;
      this.ExcelTableOptionsLabel.BackColor = System.Drawing.Color.Transparent;
      this.ExcelTableOptionsLabel.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.ExcelTableOptionsLabel.ForeColor = System.Drawing.SystemColors.ControlText;
      this.ExcelTableOptionsLabel.Location = new System.Drawing.Point(8, 20);
      this.ExcelTableOptionsLabel.Name = "ExcelTableOptionsLabel";
      this.ExcelTableOptionsLabel.Size = new System.Drawing.Size(122, 17);
      this.ExcelTableOptionsLabel.TabIndex = 0;
      this.ExcelTableOptionsLabel.Text = "Excel Table Options";
      // 
      // ImportAdvancedOptionsDialog
      // 
      this.AcceptButton = this.DialogAcceptButton;
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.AutoSize = true;
      this.CancelButton = this.DialogCancelButton;
      this.ClientSize = new System.Drawing.Size(551, 276);
      this.CommandAreaVisible = true;
      this.FootnoteAreaHeight = 0;
      this.MainInstructionLocation = new System.Drawing.Point(13, 21);
      this.Name = "ImportAdvancedOptionsDialog";
      this.Text = "Advanced Import Data Options";
      this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.ImportAdvancedOptionsDialog_FormClosing);
      this.ContentAreaPanel.ResumeLayout(false);
      this.CommandAreaPanel.ResumeLayout(false);
      ((System.ComponentModel.ISupportInitialize)(this.PreviewRowsQuantityNumericUpDown)).EndInit();
      this.OptionsTabControl.ResumeLayout(false);
      this.GeneralTabPage.ResumeLayout(false);
      this.GeneralTabPage.PerformLayout();
      this.FormattingTabPage.ResumeLayout(false);
      this.FormattingTabPage.PerformLayout();
      this.ExcelTablesTabPage.ResumeLayout(false);
      this.ExcelTablesTabPage.PerformLayout();
      this.ResumeLayout(false);

    }

    #endregion

    private System.Windows.Forms.Button DialogAcceptButton;
    private System.Windows.Forms.Button DialogCancelButton;
    private System.Windows.Forms.ToolTip HelpToolTip;
    private System.Windows.Forms.Button ResetToDefaultsButton;
    private System.Windows.Forms.TabControl OptionsTabControl;
    private System.Windows.Forms.TabPage GeneralTabPage;
    private System.Windows.Forms.TabPage FormattingTabPage;
    private System.Windows.Forms.TabPage ExcelTablesTabPage;
    private System.Windows.Forms.NumericUpDown PreviewRowsQuantityNumericUpDown;
    private System.Windows.Forms.Label PreviewRowsQuantity2Label;
    private System.Windows.Forms.Label PreviewRowsQuantity1Label;
    private System.Windows.Forms.CheckBox EscapeFormulaValuesCheckBox;
    private System.Windows.Forms.Label GeneralOptionsLabel;
    private System.Windows.Forms.Label FormatTimeLabel;
    private System.Windows.Forms.TextBox FormatTimeTextBox;
    private System.Windows.Forms.Label FormatShortDatesLabel;
    private System.Windows.Forms.TextBox FormatShortDatesTextBox;
    private System.Windows.Forms.TextBox FormatLongDatesTextBox;
    private System.Windows.Forms.Label FormatLongDatesLabel;
    private System.Windows.Forms.Label FormattingOptionsLabel;
    private System.Windows.Forms.CheckBox PrefixExcelTablesCheckBox;
    private System.Windows.Forms.TextBox PrefixExcelTablesTextBox;
    private System.Windows.Forms.Label UseStyle2Label;
    private System.Windows.Forms.ComboBox UseStyleComboBox;
    private System.Windows.Forms.Label UseStyle1Label;
    private System.Windows.Forms.CheckBox CreateExcelTableCheckbox;
    private System.Windows.Forms.Label ExcelTableOptionsLabel;
    private System.Windows.Forms.CheckBox FloatingPointDataAsDecimalCheckBox;
  }
}