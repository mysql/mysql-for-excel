// Copyright (c) 2012, 2013, Oracle and/or its affiliates. All rights reserved.
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
  partial class AppendNewColumnMappingDialog
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
      this.OKButton = new System.Windows.Forms.Button();
      this.DialogCancelButton = new System.Windows.Forms.Button();
      this.MappingNameTextBox = new System.Windows.Forms.TextBox();
      this.LogoPictureBox = new System.Windows.Forms.PictureBox();
      this.ColumnMappingNameLabel = new System.Windows.Forms.Label();
      this.MappingNameLabel = new System.Windows.Forms.Label();
      this.InstructionsLabel = new System.Windows.Forms.Label();
      this.ContentAreaPanel.SuspendLayout();
      this.CommandAreaPanel.SuspendLayout();
      ((System.ComponentModel.ISupportInitialize)(this.LogoPictureBox)).BeginInit();
      this.SuspendLayout();
      // 
      // FootnoteAreaPanel
      // 
      this.FootnoteAreaPanel.Location = new System.Drawing.Point(0, 102);
      this.FootnoteAreaPanel.Size = new System.Drawing.Size(514, 0);
      // 
      // ContentAreaPanel
      // 
      this.ContentAreaPanel.Controls.Add(this.MappingNameLabel);
      this.ContentAreaPanel.Controls.Add(this.InstructionsLabel);
      this.ContentAreaPanel.Controls.Add(this.ColumnMappingNameLabel);
      this.ContentAreaPanel.Controls.Add(this.LogoPictureBox);
      this.ContentAreaPanel.Controls.Add(this.MappingNameTextBox);
      this.ContentAreaPanel.Size = new System.Drawing.Size(514, 182);
      // 
      // CommandAreaPanel
      // 
      this.CommandAreaPanel.Controls.Add(this.OKButton);
      this.CommandAreaPanel.Controls.Add(this.DialogCancelButton);
      this.CommandAreaPanel.Location = new System.Drawing.Point(0, 137);
      this.CommandAreaPanel.Size = new System.Drawing.Size(514, 45);
      // 
      // OKButton
      // 
      this.OKButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
      this.OKButton.DialogResult = System.Windows.Forms.DialogResult.OK;
      this.OKButton.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.OKButton.Location = new System.Drawing.Point(346, 11);
      this.OKButton.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
      this.OKButton.Name = "OKButton";
      this.OKButton.Size = new System.Drawing.Size(75, 23);
      this.OKButton.TabIndex = 0;
      this.OKButton.Text = "OK";
      this.OKButton.UseVisualStyleBackColor = true;
      // 
      // DialogCancelButton
      // 
      this.DialogCancelButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
      this.DialogCancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
      this.DialogCancelButton.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.DialogCancelButton.Location = new System.Drawing.Point(427, 11);
      this.DialogCancelButton.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
      this.DialogCancelButton.Name = "DialogCancelButton";
      this.DialogCancelButton.Size = new System.Drawing.Size(75, 23);
      this.DialogCancelButton.TabIndex = 1;
      this.DialogCancelButton.Text = "Cancel";
      this.DialogCancelButton.UseVisualStyleBackColor = true;
      // 
      // MappingNameTextBox
      // 
      this.MappingNameTextBox.Location = new System.Drawing.Point(186, 90);
      this.MappingNameTextBox.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
      this.MappingNameTextBox.Name = "MappingNameTextBox";
      this.MappingNameTextBox.Size = new System.Drawing.Size(316, 20);
      this.MappingNameTextBox.TabIndex = 1;
      this.MappingNameTextBox.TextChanged += new System.EventHandler(this.MappingNameTextBox_TextChanged);
      // 
      // LogoPictureBox
      // 
      this.LogoPictureBox.Image = global::MySQL.ForExcel.Properties.Resources.MySQLforExcel_Input_64x64;
      this.LogoPictureBox.Location = new System.Drawing.Point(14, 14);
      this.LogoPictureBox.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
      this.LogoPictureBox.Name = "LogoPictureBox";
      this.LogoPictureBox.Size = new System.Drawing.Size(64, 64);
      this.LogoPictureBox.TabIndex = 11;
      this.LogoPictureBox.TabStop = false;
      // 
      // ColumnMappingNameLabel
      // 
      this.ColumnMappingNameLabel.AutoSize = true;
      this.ColumnMappingNameLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.ColumnMappingNameLabel.ForeColor = System.Drawing.Color.Navy;
      this.ColumnMappingNameLabel.Location = new System.Drawing.Point(84, 23);
      this.ColumnMappingNameLabel.Name = "ColumnMappingNameLabel";
      this.ColumnMappingNameLabel.Size = new System.Drawing.Size(168, 18);
      this.ColumnMappingNameLabel.TabIndex = 0;
      this.ColumnMappingNameLabel.Text = "Column Mapping Name:";
      // 
      // MappingNameLabel
      // 
      this.MappingNameLabel.AutoSize = true;
      this.MappingNameLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.MappingNameLabel.Location = new System.Drawing.Point(84, 91);
      this.MappingNameLabel.Name = "MappingNameLabel";
      this.MappingNameLabel.Size = new System.Drawing.Size(96, 15);
      this.MappingNameLabel.TabIndex = 13;
      this.MappingNameLabel.Text = "Mapping Name:";
      // 
      // InstructionsLabel
      // 
      this.InstructionsLabel.AutoSize = true;
      this.InstructionsLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.InstructionsLabel.Location = new System.Drawing.Point(84, 45);
      this.InstructionsLabel.Name = "InstructionsLabel";
      this.InstructionsLabel.Size = new System.Drawing.Size(257, 15);
      this.InstructionsLabel.TabIndex = 12;
      this.InstructionsLabel.Text = "Please enter a name for the column mapping.";
      // 
      // AppendNewColumnMappingDialog
      // 
      this.AcceptButton = this.OKButton;
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
      this.CancelButton = this.DialogCancelButton;
      this.ClientSize = new System.Drawing.Size(514, 182);
      this.CommandAreaVisible = true;
      this.FootnoteAreaHeight = 0;
      this.MainInstructionLocation = new System.Drawing.Point(13, 13);
      this.MainInstructionLocationOffset = new System.Drawing.Size(-10, 10);
      this.Name = "AppendNewColumnMappingDialog";
      this.Text = "MySQL for Excel";
      this.ContentAreaPanel.ResumeLayout(false);
      this.ContentAreaPanel.PerformLayout();
      this.CommandAreaPanel.ResumeLayout(false);
      ((System.ComponentModel.ISupportInitialize)(this.LogoPictureBox)).EndInit();
      this.ResumeLayout(false);

    }

    #endregion

    private System.Windows.Forms.Button OKButton;
    private System.Windows.Forms.Button DialogCancelButton;
    private System.Windows.Forms.TextBox MappingNameTextBox;
    private System.Windows.Forms.Label ColumnMappingNameLabel;
    private System.Windows.Forms.PictureBox LogoPictureBox;
    private System.Windows.Forms.Label MappingNameLabel;
    private System.Windows.Forms.Label InstructionsLabel;
  }
}