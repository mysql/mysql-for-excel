// Copyright (c) 2013, Oracle and/or its affiliates. All rights reserved.
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
      this.ContentAreaPanel.Size = new System.Drawing.Size(544, 301);
      // 
      // CommandAreaPanel
      // 
      this.CommandAreaPanel.Controls.Add(this.DialogAcceptButton);
      this.CommandAreaPanel.Controls.Add(this.DialogCancelButton);
      this.CommandAreaPanel.Location = new System.Drawing.Point(0, 256);
      this.CommandAreaPanel.Size = new System.Drawing.Size(544, 45);
      // 
      // DialogAcceptButton
      // 
      this.DialogAcceptButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
      this.DialogAcceptButton.DialogResult = System.Windows.Forms.DialogResult.OK;
      this.DialogAcceptButton.Location = new System.Drawing.Point(366, 11);
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
      this.DialogCancelButton.Location = new System.Drawing.Point(447, 11);
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
      this.EscapeFormulaValuesCheckBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.EscapeFormulaValuesCheckBox.Location = new System.Drawing.Point(53, 110);
      this.EscapeFormulaValuesCheckBox.Name = "EscapeFormulaValuesCheckBox";
      this.EscapeFormulaValuesCheckBox.Size = new System.Drawing.Size(443, 19);
      this.EscapeFormulaValuesCheckBox.TabIndex = 5;
      this.EscapeFormulaValuesCheckBox.Text = "Escape text values that start with \"=\" so Excel does not treat them as formulas";
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
      this.PreviewRowsQuantity1Label.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.PreviewRowsQuantity1Label.Location = new System.Drawing.Point(50, 85);
      this.PreviewRowsQuantity1Label.Name = "PreviewRowsQuantity1Label";
      this.PreviewRowsQuantity1Label.Size = new System.Drawing.Size(71, 15);
      this.PreviewRowsQuantity1Label.TabIndex = 2;
      this.PreviewRowsQuantity1Label.Text = "Use the first";
      // 
      // PreviewRowsQuantity2Label
      // 
      this.PreviewRowsQuantity2Label.AutoSize = true;
      this.PreviewRowsQuantity2Label.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.PreviewRowsQuantity2Label.Location = new System.Drawing.Point(185, 85);
      this.PreviewRowsQuantity2Label.Name = "PreviewRowsQuantity2Label";
      this.PreviewRowsQuantity2Label.Size = new System.Drawing.Size(223, 15);
      this.PreviewRowsQuantity2Label.TabIndex = 4;
      this.PreviewRowsQuantity2Label.Text = "rows to preview the MySQL table\'s data.";
      // 
      // PreviewRowsQuantityNumericUpDown
      // 
      this.PreviewRowsQuantityNumericUpDown.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
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
      this.PreviewRowsQuantityNumericUpDown.Size = new System.Drawing.Size(52, 21);
      this.PreviewRowsQuantityNumericUpDown.TabIndex = 3;
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
      this.CreateExcelTableCheckbox.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.CreateExcelTableCheckbox.Location = new System.Drawing.Point(53, 178);
      this.CreateExcelTableCheckbox.Name = "CreateExcelTableCheckbox";
      this.CreateExcelTableCheckbox.Size = new System.Drawing.Size(334, 19);
      this.CreateExcelTableCheckbox.TabIndex = 8;
      this.CreateExcelTableCheckbox.Text = "Create an Excel table for the imported MySQL table data.";
      this.CreateExcelTableCheckbox.UseVisualStyleBackColor = false;
      this.CreateExcelTableCheckbox.CheckedChanged += new System.EventHandler(this.CreateExcelTableCheckbox_CheckedChanged);
      // 
      // ExcelTableOptionsLabel
      // 
      this.ExcelTableOptionsLabel.AutoSize = true;
      this.ExcelTableOptionsLabel.BackColor = System.Drawing.Color.Transparent;
      this.ExcelTableOptionsLabel.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.ExcelTableOptionsLabel.ForeColor = System.Drawing.SystemColors.ControlText;
      this.ExcelTableOptionsLabel.Location = new System.Drawing.Point(24, 150);
      this.ExcelTableOptionsLabel.Name = "ExcelTableOptionsLabel";
      this.ExcelTableOptionsLabel.Size = new System.Drawing.Size(123, 17);
      this.ExcelTableOptionsLabel.TabIndex = 6;
      this.ExcelTableOptionsLabel.Text = "Excel Table Options";
      // 
      // UseStyle1Label
      // 
      this.UseStyle1Label.AutoSize = true;
      this.UseStyle1Label.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.UseStyle1Label.Location = new System.Drawing.Point(70, 205);
      this.UseStyle1Label.Name = "UseStyle1Label";
      this.UseStyle1Label.Size = new System.Drawing.Size(56, 15);
      this.UseStyle1Label.TabIndex = 9;
      this.UseStyle1Label.Text = "Use style";
      // 
      // UseStyleComboBox
      // 
      this.UseStyleComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
      this.UseStyleComboBox.FormattingEnabled = true;
      this.UseStyleComboBox.Location = new System.Drawing.Point(132, 204);
      this.UseStyleComboBox.Name = "UseStyleComboBox";
      this.UseStyleComboBox.Size = new System.Drawing.Size(225, 21);
      this.UseStyleComboBox.TabIndex = 10;
      // 
      // UseStyle2Label
      // 
      this.UseStyle2Label.AutoSize = true;
      this.UseStyle2Label.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.UseStyle2Label.Location = new System.Drawing.Point(363, 205);
      this.UseStyle2Label.Name = "UseStyle2Label";
      this.UseStyle2Label.Size = new System.Drawing.Size(133, 15);
      this.UseStyle2Label.TabIndex = 11;
      this.UseStyle2Label.Text = "for the new Excel table.";
      // 
      // ImportAdvancedOptionsDialog
      // 
      this.AcceptButton = this.DialogAcceptButton;
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
      this.CancelButton = this.DialogCancelButton;
      this.ClientSize = new System.Drawing.Size(544, 301);
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
  }
}