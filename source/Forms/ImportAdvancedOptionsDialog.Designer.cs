// Copyright (c) 2013, 2015, Oracle and/or its affiliates. All rights reserved.
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
      this.DialogAcceptButton = new System.Windows.Forms.Button();
      this.DialogCancelButton = new System.Windows.Forms.Button();
      this.EscapeFormulaValuesCheckBox = new System.Windows.Forms.CheckBox();
      this.AdvancedImportOptionsLabel = new System.Windows.Forms.Label();
      this.PreviewRowsQuantity1Label = new System.Windows.Forms.Label();
      this.PreviewRowsQuantity2Label = new System.Windows.Forms.Label();
      this.PreviewRowsQuantityNumericUpDown = new System.Windows.Forms.NumericUpDown();
      this.GeneralOptionsLabel = new System.Windows.Forms.Label();
      this.CreateExcelTableCheckbox = new System.Windows.Forms.CheckBox();
      this.ExcelTableOptionsLabel = new System.Windows.Forms.Label();
      this.UseStyle1Label = new System.Windows.Forms.Label();
      this.UseStyleComboBox = new System.Windows.Forms.ComboBox();
      this.UseStyle2Label = new System.Windows.Forms.Label();
      this.PrefixExcelTablesTextBox = new System.Windows.Forms.TextBox();
      this.PrefixExcelTablesCheckBox = new System.Windows.Forms.CheckBox();
      this.HelpToolTip = new System.Windows.Forms.ToolTip(this.components);
      this.FormatLongDatesTextBox = new System.Windows.Forms.TextBox();
      this.FormatShortDatesTextBox = new System.Windows.Forms.TextBox();
      this.FormatTimeTextBox = new System.Windows.Forms.TextBox();
      this.ResetToDefaultsButton = new System.Windows.Forms.Button();
      this.FormattingOptionsLabel = new System.Windows.Forms.Label();
      this.FormatLongDatesLabel = new System.Windows.Forms.Label();
      this.FormatShortDatesLabel = new System.Windows.Forms.Label();
      this.FormatTimeLabel = new System.Windows.Forms.Label();
      this.ContentAreaPanel.SuspendLayout();
      this.CommandAreaPanel.SuspendLayout();
      ((System.ComponentModel.ISupportInitialize)(this.PreviewRowsQuantityNumericUpDown)).BeginInit();
      this.SuspendLayout();
      // 
      // FootnoteAreaPanel
      // 
      this.FootnoteAreaPanel.Location = new System.Drawing.Point(0, 292);
      this.FootnoteAreaPanel.Size = new System.Drawing.Size(634, 0);
      // 
      // ContentAreaPanel
      // 
      this.ContentAreaPanel.Controls.Add(this.FormatTimeLabel);
      this.ContentAreaPanel.Controls.Add(this.FormatTimeTextBox);
      this.ContentAreaPanel.Controls.Add(this.FormatShortDatesLabel);
      this.ContentAreaPanel.Controls.Add(this.FormatShortDatesTextBox);
      this.ContentAreaPanel.Controls.Add(this.FormatLongDatesTextBox);
      this.ContentAreaPanel.Controls.Add(this.FormatLongDatesLabel);
      this.ContentAreaPanel.Controls.Add(this.FormattingOptionsLabel);
      this.ContentAreaPanel.Controls.Add(this.PrefixExcelTablesCheckBox);
      this.ContentAreaPanel.Controls.Add(this.PrefixExcelTablesTextBox);
      this.ContentAreaPanel.Controls.Add(this.UseStyle2Label);
      this.ContentAreaPanel.Controls.Add(this.UseStyleComboBox);
      this.ContentAreaPanel.Controls.Add(this.UseStyle1Label);
      this.ContentAreaPanel.Controls.Add(this.CreateExcelTableCheckbox);
      this.ContentAreaPanel.Controls.Add(this.ExcelTableOptionsLabel);
      this.ContentAreaPanel.Controls.Add(this.PreviewRowsQuantityNumericUpDown);
      this.ContentAreaPanel.Controls.Add(this.PreviewRowsQuantity2Label);
      this.ContentAreaPanel.Controls.Add(this.PreviewRowsQuantity1Label);
      this.ContentAreaPanel.Controls.Add(this.AdvancedImportOptionsLabel);
      this.ContentAreaPanel.Controls.Add(this.EscapeFormulaValuesCheckBox);
      this.ContentAreaPanel.Controls.Add(this.GeneralOptionsLabel);
      this.ContentAreaPanel.Size = new System.Drawing.Size(544, 461);
      // 
      // CommandAreaPanel
      // 
      this.CommandAreaPanel.Controls.Add(this.ResetToDefaultsButton);
      this.CommandAreaPanel.Controls.Add(this.DialogAcceptButton);
      this.CommandAreaPanel.Controls.Add(this.DialogCancelButton);
      this.CommandAreaPanel.Location = new System.Drawing.Point(0, 416);
      this.CommandAreaPanel.Size = new System.Drawing.Size(544, 45);
      // 
      // DialogAcceptButton
      // 
      this.DialogAcceptButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
      this.DialogAcceptButton.DialogResult = System.Windows.Forms.DialogResult.OK;
      this.DialogAcceptButton.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.DialogAcceptButton.Location = new System.Drawing.Point(376, 11);
      this.DialogAcceptButton.Name = "DialogAcceptButton";
      this.DialogAcceptButton.Size = new System.Drawing.Size(75, 23);
      this.DialogAcceptButton.TabIndex = 0;
      this.DialogAcceptButton.Text = "Accept";
      this.DialogAcceptButton.UseVisualStyleBackColor = true;
      // 
      // DialogCancelButton
      // 
      this.DialogCancelButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
      this.DialogCancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
      this.DialogCancelButton.Font = new System.Drawing.Font("Segoe UI", 8.25F);
      this.DialogCancelButton.Location = new System.Drawing.Point(457, 11);
      this.DialogCancelButton.Name = "DialogCancelButton";
      this.DialogCancelButton.Size = new System.Drawing.Size(75, 23);
      this.DialogCancelButton.TabIndex = 1;
      this.DialogCancelButton.Text = "Cancel";
      this.DialogCancelButton.UseVisualStyleBackColor = true;
      // 
      // EscapeFormulaValuesCheckBox
      // 
      this.EscapeFormulaValuesCheckBox.AutoSize = true;
      this.EscapeFormulaValuesCheckBox.BackColor = System.Drawing.Color.Transparent;
      this.EscapeFormulaValuesCheckBox.Font = new System.Drawing.Font("Segoe UI", 9F);
      this.EscapeFormulaValuesCheckBox.Location = new System.Drawing.Point(53, 112);
      this.EscapeFormulaValuesCheckBox.Name = "EscapeFormulaValuesCheckBox";
      this.EscapeFormulaValuesCheckBox.Size = new System.Drawing.Size(432, 19);
      this.EscapeFormulaValuesCheckBox.TabIndex = 5;
      this.EscapeFormulaValuesCheckBox.Text = "Escape text values that start with \"=\" so Excel does not treat them as formulas";
      this.HelpToolTip.SetToolTip(this.EscapeFormulaValuesCheckBox, "When checked the equals signs found at the start of text values in the importing " +
        "MySQL data are removed so they are not treated as formulas by Excel.");
      this.EscapeFormulaValuesCheckBox.UseVisualStyleBackColor = false;
      // 
      // AdvancedImportOptionsLabel
      // 
      this.AdvancedImportOptionsLabel.AutoSize = true;
      this.AdvancedImportOptionsLabel.Font = new System.Drawing.Font("Segoe UI", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.AdvancedImportOptionsLabel.ForeColor = System.Drawing.Color.Navy;
      this.AdvancedImportOptionsLabel.Location = new System.Drawing.Point(17, 17);
      this.AdvancedImportOptionsLabel.Name = "AdvancedImportOptionsLabel";
      this.AdvancedImportOptionsLabel.Size = new System.Drawing.Size(180, 20);
      this.AdvancedImportOptionsLabel.TabIndex = 0;
      this.AdvancedImportOptionsLabel.Text = "Advanced Import Options";
      // 
      // PreviewRowsQuantity1Label
      // 
      this.PreviewRowsQuantity1Label.AutoSize = true;
      this.PreviewRowsQuantity1Label.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.PreviewRowsQuantity1Label.Location = new System.Drawing.Point(50, 85);
      this.PreviewRowsQuantity1Label.Name = "PreviewRowsQuantity1Label";
      this.PreviewRowsQuantity1Label.Size = new System.Drawing.Size(69, 15);
      this.PreviewRowsQuantity1Label.TabIndex = 2;
      this.PreviewRowsQuantity1Label.Text = "Use the first";
      // 
      // PreviewRowsQuantity2Label
      // 
      this.PreviewRowsQuantity2Label.AutoSize = true;
      this.PreviewRowsQuantity2Label.Font = new System.Drawing.Font("Segoe UI", 9F);
      this.PreviewRowsQuantity2Label.Location = new System.Drawing.Point(185, 85);
      this.PreviewRowsQuantity2Label.Name = "PreviewRowsQuantity2Label";
      this.PreviewRowsQuantity2Label.Size = new System.Drawing.Size(217, 15);
      this.PreviewRowsQuantity2Label.TabIndex = 4;
      this.PreviewRowsQuantity2Label.Text = "rows to preview the MySQL table\'s data.";
      // 
      // PreviewRowsQuantityNumericUpDown
      // 
      this.PreviewRowsQuantityNumericUpDown.Font = new System.Drawing.Font("Segoe UI", 9F);
      this.PreviewRowsQuantityNumericUpDown.Location = new System.Drawing.Point(127, 83);
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
      this.PreviewRowsQuantityNumericUpDown.TabIndex = 3;
      this.HelpToolTip.SetToolTip(this.PreviewRowsQuantityNumericUpDown, "Limits the data preview to the given number of Excel data rows.");
      this.PreviewRowsQuantityNumericUpDown.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
      // 
      // GeneralOptionsLabel
      // 
      this.GeneralOptionsLabel.AutoSize = true;
      this.GeneralOptionsLabel.BackColor = System.Drawing.Color.Transparent;
      this.GeneralOptionsLabel.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.GeneralOptionsLabel.ForeColor = System.Drawing.SystemColors.ControlText;
      this.GeneralOptionsLabel.Location = new System.Drawing.Point(24, 56);
      this.GeneralOptionsLabel.Name = "GeneralOptionsLabel";
      this.GeneralOptionsLabel.Size = new System.Drawing.Size(103, 17);
      this.GeneralOptionsLabel.TabIndex = 1;
      this.GeneralOptionsLabel.Text = "General Options";
      // 
      // CreateExcelTableCheckbox
      // 
      this.CreateExcelTableCheckbox.AutoSize = true;
      this.CreateExcelTableCheckbox.BackColor = System.Drawing.Color.Transparent;
      this.CreateExcelTableCheckbox.Font = new System.Drawing.Font("Segoe UI", 9F);
      this.CreateExcelTableCheckbox.Location = new System.Drawing.Point(53, 302);
      this.CreateExcelTableCheckbox.Name = "CreateExcelTableCheckbox";
      this.CreateExcelTableCheckbox.Size = new System.Drawing.Size(294, 19);
      this.CreateExcelTableCheckbox.TabIndex = 14;
      this.CreateExcelTableCheckbox.Text = "Create an Excel table for the imported MySQL data.";
      this.HelpToolTip.SetToolTip(this.CreateExcelTableCheckbox, "When checked an Excel table (previously known as Excel lists) will be created for" +
        " the imported data.");
      this.CreateExcelTableCheckbox.UseVisualStyleBackColor = false;
      this.CreateExcelTableCheckbox.CheckedChanged += new System.EventHandler(this.CreateExcelTableCheckbox_CheckedChanged);
      // 
      // ExcelTableOptionsLabel
      // 
      this.ExcelTableOptionsLabel.AutoSize = true;
      this.ExcelTableOptionsLabel.BackColor = System.Drawing.Color.Transparent;
      this.ExcelTableOptionsLabel.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.ExcelTableOptionsLabel.ForeColor = System.Drawing.SystemColors.ControlText;
      this.ExcelTableOptionsLabel.Location = new System.Drawing.Point(24, 274);
      this.ExcelTableOptionsLabel.Name = "ExcelTableOptionsLabel";
      this.ExcelTableOptionsLabel.Size = new System.Drawing.Size(123, 17);
      this.ExcelTableOptionsLabel.TabIndex = 13;
      this.ExcelTableOptionsLabel.Text = "Excel Table Options";
      // 
      // UseStyle1Label
      // 
      this.UseStyle1Label.AutoSize = true;
      this.UseStyle1Label.Font = new System.Drawing.Font("Segoe UI", 9F);
      this.UseStyle1Label.Location = new System.Drawing.Point(70, 330);
      this.UseStyle1Label.Name = "UseStyle1Label";
      this.UseStyle1Label.Size = new System.Drawing.Size(53, 15);
      this.UseStyle1Label.TabIndex = 15;
      this.UseStyle1Label.Text = "Use style";
      // 
      // UseStyleComboBox
      // 
      this.UseStyleComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
      this.UseStyleComboBox.Font = new System.Drawing.Font("Segoe UI", 9F);
      this.UseStyleComboBox.FormattingEnabled = true;
      this.UseStyleComboBox.Location = new System.Drawing.Point(127, 327);
      this.UseStyleComboBox.Name = "UseStyleComboBox";
      this.UseStyleComboBox.Size = new System.Drawing.Size(225, 23);
      this.UseStyleComboBox.TabIndex = 16;
      this.HelpToolTip.SetToolTip(this.UseStyleComboBox, "You can quickly format table data by applying a predefined or custom table style." +
        "");
      // 
      // UseStyle2Label
      // 
      this.UseStyle2Label.AutoSize = true;
      this.UseStyle2Label.Font = new System.Drawing.Font("Segoe UI", 9F);
      this.UseStyle2Label.Location = new System.Drawing.Point(358, 330);
      this.UseStyle2Label.Name = "UseStyle2Label";
      this.UseStyle2Label.Size = new System.Drawing.Size(128, 15);
      this.UseStyle2Label.TabIndex = 17;
      this.UseStyle2Label.Text = "for the new Excel table.";
      // 
      // PrefixExcelTablesTextBox
      // 
      this.PrefixExcelTablesTextBox.Font = new System.Drawing.Font("Segoe UI", 9F);
      this.PrefixExcelTablesTextBox.Location = new System.Drawing.Point(321, 356);
      this.PrefixExcelTablesTextBox.Name = "PrefixExcelTablesTextBox";
      this.PrefixExcelTablesTextBox.Size = new System.Drawing.Size(169, 23);
      this.PrefixExcelTablesTextBox.TabIndex = 19;
      // 
      // PrefixExcelTablesCheckBox
      // 
      this.PrefixExcelTablesCheckBox.AutoSize = true;
      this.PrefixExcelTablesCheckBox.Font = new System.Drawing.Font("Segoe UI", 9F);
      this.PrefixExcelTablesCheckBox.Location = new System.Drawing.Point(73, 358);
      this.PrefixExcelTablesCheckBox.Name = "PrefixExcelTablesCheckBox";
      this.PrefixExcelTablesCheckBox.Size = new System.Drawing.Size(242, 19);
      this.PrefixExcelTablesCheckBox.TabIndex = 18;
      this.PrefixExcelTablesCheckBox.Text = "Prefix Excel tables with the following text:";
      this.HelpToolTip.SetToolTip(this.PrefixExcelTablesCheckBox, "When checked the specified text will be used to prefix the names of created Excel" +
        " tables.");
      this.PrefixExcelTablesCheckBox.UseVisualStyleBackColor = true;
      this.PrefixExcelTablesCheckBox.CheckedChanged += new System.EventHandler(this.PrefixExcelTablesCheckBox_CheckedChanged);
      // 
      // HelpToolTip
      // 
      this.HelpToolTip.AutoPopDelay = 5000;
      this.HelpToolTip.InitialDelay = 1000;
      this.HelpToolTip.ReshowDelay = 100;
      // 
      // FormatLongDatesTextBox
      // 
      this.FormatLongDatesTextBox.Font = new System.Drawing.Font("Segoe UI", 9F);
      this.FormatLongDatesTextBox.Location = new System.Drawing.Point(361, 176);
      this.FormatLongDatesTextBox.Name = "FormatLongDatesTextBox";
      this.FormatLongDatesTextBox.Size = new System.Drawing.Size(124, 23);
      this.FormatLongDatesTextBox.TabIndex = 8;
      this.HelpToolTip.SetToolTip(this.FormatLongDatesTextBox, "The Excel format string to be used for DateTime and TimeStamp MySQL data.\r\nA long" +
        " date format is suitable in this case.");
      // 
      // FormatShortDatesTextBox
      // 
      this.FormatShortDatesTextBox.Font = new System.Drawing.Font("Segoe UI", 9F);
      this.FormatShortDatesTextBox.Location = new System.Drawing.Point(247, 205);
      this.FormatShortDatesTextBox.Name = "FormatShortDatesTextBox";
      this.FormatShortDatesTextBox.Size = new System.Drawing.Size(124, 23);
      this.FormatShortDatesTextBox.TabIndex = 10;
      this.HelpToolTip.SetToolTip(this.FormatShortDatesTextBox, "The Excel format string to be used for Date MySQL data.\r\nA short date format is s" +
        "uitable in this case.");
      // 
      // FormatTimeTextBox
      // 
      this.FormatTimeTextBox.Font = new System.Drawing.Font("Segoe UI", 9F);
      this.FormatTimeTextBox.Location = new System.Drawing.Point(250, 234);
      this.FormatTimeTextBox.Name = "FormatTimeTextBox";
      this.FormatTimeTextBox.Size = new System.Drawing.Size(124, 23);
      this.FormatTimeTextBox.TabIndex = 12;
      this.HelpToolTip.SetToolTip(this.FormatTimeTextBox, "The Excel format string to be used for Time MySQL data.");
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
      // FormattingOptionsLabel
      // 
      this.FormattingOptionsLabel.AutoSize = true;
      this.FormattingOptionsLabel.BackColor = System.Drawing.Color.Transparent;
      this.FormattingOptionsLabel.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.FormattingOptionsLabel.ForeColor = System.Drawing.SystemColors.ControlText;
      this.FormattingOptionsLabel.Location = new System.Drawing.Point(24, 150);
      this.FormattingOptionsLabel.Name = "FormattingOptionsLabel";
      this.FormattingOptionsLabel.Size = new System.Drawing.Size(121, 17);
      this.FormattingOptionsLabel.TabIndex = 6;
      this.FormattingOptionsLabel.Text = "Formatting Options";
      // 
      // FormatLongDatesLabel
      // 
      this.FormatLongDatesLabel.AutoSize = true;
      this.FormatLongDatesLabel.Font = new System.Drawing.Font("Segoe UI", 9F);
      this.FormatLongDatesLabel.Location = new System.Drawing.Point(50, 179);
      this.FormatLongDatesLabel.Name = "FormatLongDatesLabel";
      this.FormatLongDatesLabel.Size = new System.Drawing.Size(305, 15);
      this.FormatLongDatesLabel.TabIndex = 7;
      this.FormatLongDatesLabel.Text = "Excel number format for DateTime and TimeStamp data:";
      // 
      // FormatShortDatesLabel
      // 
      this.FormatShortDatesLabel.AutoSize = true;
      this.FormatShortDatesLabel.Font = new System.Drawing.Font("Segoe UI", 9F);
      this.FormatShortDatesLabel.Location = new System.Drawing.Point(50, 208);
      this.FormatShortDatesLabel.Name = "FormatShortDatesLabel";
      this.FormatShortDatesLabel.Size = new System.Drawing.Size(191, 15);
      this.FormatShortDatesLabel.TabIndex = 9;
      this.FormatShortDatesLabel.Text = "Excel number format for Date data:";
      // 
      // FormatTimeLabel
      // 
      this.FormatTimeLabel.AutoSize = true;
      this.FormatTimeLabel.Font = new System.Drawing.Font("Segoe UI", 9F);
      this.FormatTimeLabel.Location = new System.Drawing.Point(50, 237);
      this.FormatTimeLabel.Name = "FormatTimeLabel";
      this.FormatTimeLabel.Size = new System.Drawing.Size(194, 15);
      this.FormatTimeLabel.TabIndex = 11;
      this.FormatTimeLabel.Text = "Excel number format for Time data:";
      // 
      // ImportAdvancedOptionsDialog
      // 
      this.AcceptButton = this.DialogAcceptButton;
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.AutoSize = true;
      this.CancelButton = this.DialogCancelButton;
      this.ClientSize = new System.Drawing.Size(544, 461);
      this.CommandAreaVisible = true;
      this.FootnoteAreaHeight = 0;
      this.MainInstructionLocation = new System.Drawing.Point(13, 21);
      this.Name = "ImportAdvancedOptionsDialog";
      this.Text = "Advanced Options";
      this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.ImportAdvancedOptionsDialog_FormClosing);
      this.ContentAreaPanel.ResumeLayout(false);
      this.ContentAreaPanel.PerformLayout();
      this.CommandAreaPanel.ResumeLayout(false);
      ((System.ComponentModel.ISupportInitialize)(this.PreviewRowsQuantityNumericUpDown)).EndInit();
      this.ResumeLayout(false);

    }

    #endregion

    private System.Windows.Forms.Button DialogAcceptButton;
    private System.Windows.Forms.Button DialogCancelButton;
    private System.Windows.Forms.CheckBox EscapeFormulaValuesCheckBox;
    private System.Windows.Forms.Label AdvancedImportOptionsLabel;
    private System.Windows.Forms.NumericUpDown PreviewRowsQuantityNumericUpDown;
    private System.Windows.Forms.Label PreviewRowsQuantity2Label;
    private System.Windows.Forms.Label PreviewRowsQuantity1Label;
    private System.Windows.Forms.Label GeneralOptionsLabel;
    private System.Windows.Forms.CheckBox CreateExcelTableCheckbox;
    private System.Windows.Forms.Label ExcelTableOptionsLabel;
    private System.Windows.Forms.Label UseStyle1Label;
    private System.Windows.Forms.ComboBox UseStyleComboBox;
    private System.Windows.Forms.Label UseStyle2Label;
    private System.Windows.Forms.CheckBox PrefixExcelTablesCheckBox;
    private System.Windows.Forms.TextBox PrefixExcelTablesTextBox;
    private System.Windows.Forms.ToolTip HelpToolTip;
    private System.Windows.Forms.Button ResetToDefaultsButton;
    private System.Windows.Forms.Label FormattingOptionsLabel;
    private System.Windows.Forms.Label FormatShortDatesLabel;
    private System.Windows.Forms.TextBox FormatShortDatesTextBox;
    private System.Windows.Forms.TextBox FormatLongDatesTextBox;
    private System.Windows.Forms.Label FormatLongDatesLabel;
    private System.Windows.Forms.Label FormatTimeLabel;
    private System.Windows.Forms.TextBox FormatTimeTextBox;
  }
}