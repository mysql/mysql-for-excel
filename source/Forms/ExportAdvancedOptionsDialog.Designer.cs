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
  partial class ExportAdvancedOptionsDialog
  {
    /// <summary>
    /// Required designer variable.
    /// </summary>
    private System.ComponentModel.IContainer components = null;

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

    #region Windows Form Designer generated code

    /// <summary>
    /// Required method for Designer support - do not modify
    /// the contents of this method with the code editor.
    /// </summary>
    private void InitializeComponent()
    {
      this.DialogAcceptButton = new System.Windows.Forms.Button();
      this.DialogCancelButton = new System.Windows.Forms.Button();
      this.ShowCopySQLButtonCheckBox = new System.Windows.Forms.CheckBox();
      this.OtherOptionsLabel = new System.Windows.Forms.Label();
      this.UseFormattedValuesCheckBox = new System.Windows.Forms.CheckBox();
      this.FieldDataOptionsLabel = new System.Windows.Forms.Label();
      this.AutoAllowEmptyNonIndexColumnsCheckBox = new System.Windows.Forms.CheckBox();
      this.AutoIndexIntColumnsCheckBox = new System.Windows.Forms.CheckBox();
      this.AddBufferToVarcharCheckBox = new System.Windows.Forms.CheckBox();
      this.DetectDatatypeCheckBox = new System.Windows.Forms.CheckBox();
      this.ColumnDatatypeOptionsLabel = new System.Windows.Forms.Label();
      this.AdvancedExportOptionsLabel = new System.Windows.Forms.Label();
      this.PreviewRowsQuantity1Label = new System.Windows.Forms.Label();
      this.PreviewRowsQuantity2Label = new System.Windows.Forms.Label();
      this.PreviewRowsQuantityNumericUpDown = new System.Windows.Forms.NumericUpDown();
      this.RemoveEmptyColumnsCheckBox = new System.Windows.Forms.CheckBox();
      this.ContentAreaPanel.SuspendLayout();
      this.CommandAreaPanel.SuspendLayout();
      ((System.ComponentModel.ISupportInitialize)(this.PreviewRowsQuantityNumericUpDown)).BeginInit();
      this.SuspendLayout();
      // 
      // FootnoteAreaPanel
      // 
      this.FootnoteAreaPanel.Location = new System.Drawing.Point(0, 319);
      this.FootnoteAreaPanel.Size = new System.Drawing.Size(544, 0);
      // 
      // ContentAreaPanel
      // 
      this.ContentAreaPanel.Controls.Add(this.RemoveEmptyColumnsCheckBox);
      this.ContentAreaPanel.Controls.Add(this.PreviewRowsQuantityNumericUpDown);
      this.ContentAreaPanel.Controls.Add(this.PreviewRowsQuantity2Label);
      this.ContentAreaPanel.Controls.Add(this.PreviewRowsQuantity1Label);
      this.ContentAreaPanel.Controls.Add(this.AdvancedExportOptionsLabel);
      this.ContentAreaPanel.Controls.Add(this.ShowCopySQLButtonCheckBox);
      this.ContentAreaPanel.Controls.Add(this.OtherOptionsLabel);
      this.ContentAreaPanel.Controls.Add(this.UseFormattedValuesCheckBox);
      this.ContentAreaPanel.Controls.Add(this.FieldDataOptionsLabel);
      this.ContentAreaPanel.Controls.Add(this.AutoAllowEmptyNonIndexColumnsCheckBox);
      this.ContentAreaPanel.Controls.Add(this.AutoIndexIntColumnsCheckBox);
      this.ContentAreaPanel.Controls.Add(this.AddBufferToVarcharCheckBox);
      this.ContentAreaPanel.Controls.Add(this.DetectDatatypeCheckBox);
      this.ContentAreaPanel.Controls.Add(this.ColumnDatatypeOptionsLabel);
      this.ContentAreaPanel.Size = new System.Drawing.Size(544, 399);
      // 
      // CommandAreaPanel
      // 
      this.CommandAreaPanel.Controls.Add(this.DialogAcceptButton);
      this.CommandAreaPanel.Controls.Add(this.DialogCancelButton);
      this.CommandAreaPanel.Location = new System.Drawing.Point(0, 354);
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
      // ShowCopySQLButtonCheckBox
      // 
      this.ShowCopySQLButtonCheckBox.AutoSize = true;
      this.ShowCopySQLButtonCheckBox.BackColor = System.Drawing.Color.Transparent;
      this.ShowCopySQLButtonCheckBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.ShowCopySQLButtonCheckBox.Location = new System.Drawing.Point(53, 344);
      this.ShowCopySQLButtonCheckBox.Name = "ShowCopySQLButtonCheckBox";
      this.ShowCopySQLButtonCheckBox.Size = new System.Drawing.Size(152, 19);
      this.ShowCopySQLButtonCheckBox.TabIndex = 13;
      this.ShowCopySQLButtonCheckBox.Text = "Show Copy SQL Button";
      this.ShowCopySQLButtonCheckBox.UseVisualStyleBackColor = false;
      this.ShowCopySQLButtonCheckBox.Visible = false;
      // 
      // OtherOptionsLabel
      // 
      this.OtherOptionsLabel.AutoSize = true;
      this.OtherOptionsLabel.BackColor = System.Drawing.Color.Transparent;
      this.OtherOptionsLabel.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.OtherOptionsLabel.ForeColor = System.Drawing.SystemColors.ControlText;
      this.OtherOptionsLabel.Location = new System.Drawing.Point(24, 290);
      this.OtherOptionsLabel.Name = "OtherOptionsLabel";
      this.OtherOptionsLabel.Size = new System.Drawing.Size(91, 17);
      this.OtherOptionsLabel.TabIndex = 11;
      this.OtherOptionsLabel.Text = "Other Options";
      // 
      // UseFormattedValuesCheckBox
      // 
      this.UseFormattedValuesCheckBox.AutoSize = true;
      this.UseFormattedValuesCheckBox.BackColor = System.Drawing.Color.Transparent;
      this.UseFormattedValuesCheckBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.UseFormattedValuesCheckBox.Location = new System.Drawing.Point(53, 253);
      this.UseFormattedValuesCheckBox.Name = "UseFormattedValuesCheckBox";
      this.UseFormattedValuesCheckBox.Size = new System.Drawing.Size(141, 19);
      this.UseFormattedValuesCheckBox.TabIndex = 10;
      this.UseFormattedValuesCheckBox.Text = "Use formatted values";
      this.UseFormattedValuesCheckBox.UseVisualStyleBackColor = false;
      // 
      // FieldDataOptionsLabel
      // 
      this.FieldDataOptionsLabel.AutoSize = true;
      this.FieldDataOptionsLabel.BackColor = System.Drawing.Color.Transparent;
      this.FieldDataOptionsLabel.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.FieldDataOptionsLabel.ForeColor = System.Drawing.SystemColors.ControlText;
      this.FieldDataOptionsLabel.Location = new System.Drawing.Point(24, 224);
      this.FieldDataOptionsLabel.Name = "FieldDataOptionsLabel";
      this.FieldDataOptionsLabel.Size = new System.Drawing.Size(116, 17);
      this.FieldDataOptionsLabel.TabIndex = 9;
      this.FieldDataOptionsLabel.Text = "Field Data Options";
      // 
      // AutoAllowEmptyNonIndexColumnsCheckBox
      // 
      this.AutoAllowEmptyNonIndexColumnsCheckBox.AutoSize = true;
      this.AutoAllowEmptyNonIndexColumnsCheckBox.BackColor = System.Drawing.Color.Transparent;
      this.AutoAllowEmptyNonIndexColumnsCheckBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.AutoAllowEmptyNonIndexColumnsCheckBox.Location = new System.Drawing.Point(53, 185);
      this.AutoAllowEmptyNonIndexColumnsCheckBox.Name = "AutoAllowEmptyNonIndexColumnsCheckBox";
      this.AutoAllowEmptyNonIndexColumnsCheckBox.Size = new System.Drawing.Size(436, 19);
      this.AutoAllowEmptyNonIndexColumnsCheckBox.TabIndex = 8;
      this.AutoAllowEmptyNonIndexColumnsCheckBox.Text = "Automatically check the Allow Empty checkbox for columns without an index";
      this.AutoAllowEmptyNonIndexColumnsCheckBox.UseVisualStyleBackColor = false;
      // 
      // AutoIndexIntColumnsCheckBox
      // 
      this.AutoIndexIntColumnsCheckBox.AutoSize = true;
      this.AutoIndexIntColumnsCheckBox.BackColor = System.Drawing.Color.Transparent;
      this.AutoIndexIntColumnsCheckBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.AutoIndexIntColumnsCheckBox.Location = new System.Drawing.Point(53, 160);
      this.AutoIndexIntColumnsCheckBox.Name = "AutoIndexIntColumnsCheckBox";
      this.AutoIndexIntColumnsCheckBox.Size = new System.Drawing.Size(349, 19);
      this.AutoIndexIntColumnsCheckBox.TabIndex = 7;
      this.AutoIndexIntColumnsCheckBox.Text = "Automatically check the Index checkbox for Integer columns";
      this.AutoIndexIntColumnsCheckBox.UseVisualStyleBackColor = false;
      // 
      // AddBufferToVarcharCheckBox
      // 
      this.AddBufferToVarcharCheckBox.AutoSize = true;
      this.AddBufferToVarcharCheckBox.BackColor = System.Drawing.Color.Transparent;
      this.AddBufferToVarcharCheckBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.AddBufferToVarcharCheckBox.Location = new System.Drawing.Point(73, 135);
      this.AddBufferToVarcharCheckBox.Name = "AddBufferToVarcharCheckBox";
      this.AddBufferToVarcharCheckBox.Size = new System.Drawing.Size(431, 19);
      this.AddBufferToVarcharCheckBox.TabIndex = 6;
      this.AddBufferToVarcharCheckBox.Text = "Add additional buffer to VARCHAR length (round up to 12, 25, 45, 125, 255)";
      this.AddBufferToVarcharCheckBox.UseVisualStyleBackColor = false;
      // 
      // DetectDatatypeCheckBox
      // 
      this.DetectDatatypeCheckBox.AutoSize = true;
      this.DetectDatatypeCheckBox.BackColor = System.Drawing.Color.Transparent;
      this.DetectDatatypeCheckBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.DetectDatatypeCheckBox.Location = new System.Drawing.Point(53, 110);
      this.DetectDatatypeCheckBox.Name = "DetectDatatypeCheckBox";
      this.DetectDatatypeCheckBox.Size = new System.Drawing.Size(418, 19);
      this.DetectDatatypeCheckBox.TabIndex = 5;
      this.DetectDatatypeCheckBox.Text = "Analyze and try to detect correct datatype based on column field contents";
      this.DetectDatatypeCheckBox.UseVisualStyleBackColor = false;
      this.DetectDatatypeCheckBox.CheckedChanged += new System.EventHandler(this.DetectDatatypeCheckBox_CheckedChanged);
      // 
      // ColumnDatatypeOptionsLabel
      // 
      this.ColumnDatatypeOptionsLabel.AutoSize = true;
      this.ColumnDatatypeOptionsLabel.BackColor = System.Drawing.Color.Transparent;
      this.ColumnDatatypeOptionsLabel.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.ColumnDatatypeOptionsLabel.ForeColor = System.Drawing.SystemColors.ControlText;
      this.ColumnDatatypeOptionsLabel.Location = new System.Drawing.Point(24, 56);
      this.ColumnDatatypeOptionsLabel.Name = "ColumnDatatypeOptionsLabel";
      this.ColumnDatatypeOptionsLabel.Size = new System.Drawing.Size(158, 17);
      this.ColumnDatatypeOptionsLabel.TabIndex = 1;
      this.ColumnDatatypeOptionsLabel.Text = "Column Datatype Options";
      // 
      // AdvancedExportOptionsLabel
      // 
      this.AdvancedExportOptionsLabel.AutoSize = true;
      this.AdvancedExportOptionsLabel.Font = new System.Drawing.Font("Segoe UI", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.AdvancedExportOptionsLabel.ForeColor = System.Drawing.Color.Navy;
      this.AdvancedExportOptionsLabel.Location = new System.Drawing.Point(17, 17);
      this.AdvancedExportOptionsLabel.Name = "AdvancedExportOptionsLabel";
      this.AdvancedExportOptionsLabel.Size = new System.Drawing.Size(178, 20);
      this.AdvancedExportOptionsLabel.TabIndex = 0;
      this.AdvancedExportOptionsLabel.Text = "Advanced Export Options";
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
      this.PreviewRowsQuantity2Label.Size = new System.Drawing.Size(285, 15);
      this.PreviewRowsQuantity2Label.TabIndex = 4;
      this.PreviewRowsQuantity2Label.Text = "Excel data rows to preview and calculate datatypes.";
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
      // RemoveEmptyColumnsCheckBox
      // 
      this.RemoveEmptyColumnsCheckBox.AutoSize = true;
      this.RemoveEmptyColumnsCheckBox.BackColor = System.Drawing.Color.Transparent;
      this.RemoveEmptyColumnsCheckBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.RemoveEmptyColumnsCheckBox.Location = new System.Drawing.Point(53, 319);
      this.RemoveEmptyColumnsCheckBox.Name = "RemoveEmptyColumnsCheckBox";
      this.RemoveEmptyColumnsCheckBox.Size = new System.Drawing.Size(445, 19);
      this.RemoveEmptyColumnsCheckBox.TabIndex = 12;
      this.RemoveEmptyColumnsCheckBox.Text = "Remove columns that contain no data, otherwise just flag them as \"Excluded\"";
      this.RemoveEmptyColumnsCheckBox.UseVisualStyleBackColor = false;
      // 
      // ExportAdvancedOptionsDialog
      // 
      this.AcceptButton = this.DialogAcceptButton;
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
      this.CancelButton = this.DialogCancelButton;
      this.ClientSize = new System.Drawing.Size(544, 399);
      this.CommandAreaVisible = true;
      this.FootnoteAreaHeight = 0;
      this.MainInstructionLocation = new System.Drawing.Point(13, 21);
      this.Name = "ExportAdvancedOptionsDialog";
      this.Text = "Advanced Options";
      this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.ExportAdvancedOptionsDialog_FormClosing);
      this.ContentAreaPanel.ResumeLayout(false);
      this.ContentAreaPanel.PerformLayout();
      this.CommandAreaPanel.ResumeLayout(false);
      ((System.ComponentModel.ISupportInitialize)(this.PreviewRowsQuantityNumericUpDown)).EndInit();
      this.ResumeLayout(false);

    }

    #endregion

    private System.Windows.Forms.Button DialogAcceptButton;
    private System.Windows.Forms.Button DialogCancelButton;
    private System.Windows.Forms.CheckBox ShowCopySQLButtonCheckBox;
    private System.Windows.Forms.Label OtherOptionsLabel;
    private System.Windows.Forms.CheckBox UseFormattedValuesCheckBox;
    private System.Windows.Forms.Label FieldDataOptionsLabel;
    private System.Windows.Forms.CheckBox AutoAllowEmptyNonIndexColumnsCheckBox;
    private System.Windows.Forms.CheckBox AutoIndexIntColumnsCheckBox;
    private System.Windows.Forms.CheckBox AddBufferToVarcharCheckBox;
    private System.Windows.Forms.CheckBox DetectDatatypeCheckBox;
    private System.Windows.Forms.Label ColumnDatatypeOptionsLabel;
    private System.Windows.Forms.Label AdvancedExportOptionsLabel;
    private System.Windows.Forms.NumericUpDown PreviewRowsQuantityNumericUpDown;
    private System.Windows.Forms.Label PreviewRowsQuantity2Label;
    private System.Windows.Forms.Label PreviewRowsQuantity1Label;
    private System.Windows.Forms.CheckBox RemoveEmptyColumnsCheckBox;
  }
}