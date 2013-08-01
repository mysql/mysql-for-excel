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
  partial class NewSchemaDialog
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
      this.DialogOKButton = new System.Windows.Forms.Button();
      this.DialogCancelButton = new System.Windows.Forms.Button();
      this.SchemaNameTextBox = new System.Windows.Forms.TextBox();
      this.LogoPictureBox = new System.Windows.Forms.PictureBox();
      this.NewSchemaNameLabel = new System.Windows.Forms.Label();
      this.InstructionsLabel = new System.Windows.Forms.Label();
      this.SchemaNameLabel = new System.Windows.Forms.Label();
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
      this.ContentAreaPanel.Controls.Add(this.SchemaNameLabel);
      this.ContentAreaPanel.Controls.Add(this.InstructionsLabel);
      this.ContentAreaPanel.Controls.Add(this.NewSchemaNameLabel);
      this.ContentAreaPanel.Controls.Add(this.LogoPictureBox);
      this.ContentAreaPanel.Controls.Add(this.SchemaNameTextBox);
      this.ContentAreaPanel.Size = new System.Drawing.Size(514, 182);
      // 
      // CommandAreaPanel
      // 
      this.CommandAreaPanel.Controls.Add(this.DialogOKButton);
      this.CommandAreaPanel.Controls.Add(this.DialogCancelButton);
      this.CommandAreaPanel.Location = new System.Drawing.Point(0, 137);
      this.CommandAreaPanel.Size = new System.Drawing.Size(514, 45);
      // 
      // DialogOKButton
      // 
      this.DialogOKButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
      this.DialogOKButton.DialogResult = System.Windows.Forms.DialogResult.OK;
      this.DialogOKButton.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.DialogOKButton.Location = new System.Drawing.Point(346, 11);
      this.DialogOKButton.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
      this.DialogOKButton.Name = "DialogOKButton";
      this.DialogOKButton.Size = new System.Drawing.Size(75, 23);
      this.DialogOKButton.TabIndex = 0;
      this.DialogOKButton.Text = "OK";
      this.DialogOKButton.UseVisualStyleBackColor = true;
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
      // SchemaNameTextBox
      // 
      this.SchemaNameTextBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.SchemaNameTextBox.Location = new System.Drawing.Point(183, 88);
      this.SchemaNameTextBox.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
      this.SchemaNameTextBox.Name = "SchemaNameTextBox";
      this.SchemaNameTextBox.Size = new System.Drawing.Size(319, 21);
      this.SchemaNameTextBox.TabIndex = 3;
      this.SchemaNameTextBox.TextChanged += new System.EventHandler(this.SchemaNameTextBox_TextChanged);
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
      // NewSchemaNameLabel
      // 
      this.NewSchemaNameLabel.AutoSize = true;
      this.NewSchemaNameLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.NewSchemaNameLabel.ForeColor = System.Drawing.Color.Navy;
      this.NewSchemaNameLabel.Location = new System.Drawing.Point(84, 23);
      this.NewSchemaNameLabel.Name = "NewSchemaNameLabel";
      this.NewSchemaNameLabel.Size = new System.Drawing.Size(145, 18);
      this.NewSchemaNameLabel.TabIndex = 0;
      this.NewSchemaNameLabel.Text = "New Schema Name:";
      // 
      // InstructionsLabel
      // 
      this.InstructionsLabel.AutoSize = true;
      this.InstructionsLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.InstructionsLabel.Location = new System.Drawing.Point(84, 45);
      this.InstructionsLabel.Name = "InstructionsLabel";
      this.InstructionsLabel.Size = new System.Drawing.Size(234, 15);
      this.InstructionsLabel.TabIndex = 1;
      this.InstructionsLabel.Text = "Please enter a name for the new schema.";
      // 
      // SchemaNameLabel
      // 
      this.SchemaNameLabel.AutoSize = true;
      this.SchemaNameLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.SchemaNameLabel.Location = new System.Drawing.Point(84, 91);
      this.SchemaNameLabel.Name = "SchemaNameLabel";
      this.SchemaNameLabel.Size = new System.Drawing.Size(93, 15);
      this.SchemaNameLabel.TabIndex = 2;
      this.SchemaNameLabel.Text = "Schema Name:";
      // 
      // NewSchemaDialog
      // 
      this.AcceptButton = this.DialogOKButton;
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
      this.CancelButton = this.DialogCancelButton;
      this.ClientSize = new System.Drawing.Size(514, 182);
      this.CommandAreaVisible = true;
      this.FootnoteAreaHeight = 0;
      this.MainInstructionLocation = new System.Drawing.Point(13, 13);
      this.MainInstructionLocationOffset = new System.Drawing.Size(-10, 10);
      this.Name = "NewSchemaDialog";
      this.Text = "MySQL for Excel";
      this.ContentAreaPanel.ResumeLayout(false);
      this.ContentAreaPanel.PerformLayout();
      this.CommandAreaPanel.ResumeLayout(false);
      ((System.ComponentModel.ISupportInitialize)(this.LogoPictureBox)).EndInit();
      this.ResumeLayout(false);

    }

    #endregion

    private System.Windows.Forms.Button DialogOKButton;
    private System.Windows.Forms.Button DialogCancelButton;
    private System.Windows.Forms.TextBox SchemaNameTextBox;
    private System.Windows.Forms.Label NewSchemaNameLabel;
    private System.Windows.Forms.PictureBox LogoPictureBox;
    private System.Windows.Forms.Label SchemaNameLabel;
    private System.Windows.Forms.Label InstructionsLabel;
  }
}