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
  partial class RestoreEditSessionsDialog
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
      this.LogoPictureBox = new System.Windows.Forms.PictureBox();
      this.RestoreEditSessionsTitleLabel = new System.Windows.Forms.Label();
      this.RestoreButton = new System.Windows.Forms.Button();
      this.NothingButton = new System.Windows.Forms.Button();
      this.DeleteButton = new System.Windows.Forms.Button();
      this.RestoreEditSessionsDetailLabel = new System.Windows.Forms.Label();
      this.ContentAreaPanel.SuspendLayout();
      this.CommandAreaPanel.SuspendLayout();
      ((System.ComponentModel.ISupportInitialize)(this.LogoPictureBox)).BeginInit();
      this.SuspendLayout();
      // 
      // FootnoteAreaPanel
      // 
      this.FootnoteAreaPanel.Location = new System.Drawing.Point(0, 292);
      this.FootnoteAreaPanel.Size = new System.Drawing.Size(634, 0);
      // 
      // ContentAreaPanel
      // 
      this.ContentAreaPanel.Controls.Add(this.RestoreEditSessionsDetailLabel);
      this.ContentAreaPanel.Controls.Add(this.LogoPictureBox);
      this.ContentAreaPanel.Controls.Add(this.RestoreEditSessionsTitleLabel);
      this.ContentAreaPanel.Size = new System.Drawing.Size(484, 151);
      // 
      // CommandAreaPanel
      // 
      this.CommandAreaPanel.Controls.Add(this.DeleteButton);
      this.CommandAreaPanel.Controls.Add(this.NothingButton);
      this.CommandAreaPanel.Controls.Add(this.RestoreButton);
      this.CommandAreaPanel.Location = new System.Drawing.Point(0, 106);
      this.CommandAreaPanel.Size = new System.Drawing.Size(484, 45);
      // 
      // LogoPictureBox
      // 
      this.LogoPictureBox.Image = global::MySQL.ForExcel.Properties.Resources.MySQLforExcel_Logo_64x64;
      this.LogoPictureBox.Location = new System.Drawing.Point(21, 22);
      this.LogoPictureBox.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
      this.LogoPictureBox.Name = "LogoPictureBox";
      this.LogoPictureBox.Size = new System.Drawing.Size(64, 64);
      this.LogoPictureBox.TabIndex = 27;
      this.LogoPictureBox.TabStop = false;
      // 
      // RestoreEditSessionsTitleLabel
      // 
      this.RestoreEditSessionsTitleLabel.AutoSize = true;
      this.RestoreEditSessionsTitleLabel.Font = new System.Drawing.Font("Segoe UI", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.RestoreEditSessionsTitleLabel.ForeColor = System.Drawing.Color.Navy;
      this.RestoreEditSessionsTitleLabel.Location = new System.Drawing.Point(91, 22);
      this.RestoreEditSessionsTitleLabel.Name = "RestoreEditSessionsTitleLabel";
      this.RestoreEditSessionsTitleLabel.Size = new System.Drawing.Size(192, 20);
      this.RestoreEditSessionsTitleLabel.TabIndex = 25;
      this.RestoreEditSessionsTitleLabel.Text = "Restore Saved Edit Sessions";
      // 
      // RestoreButton
      // 
      this.RestoreButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
      this.RestoreButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
      this.RestoreButton.Location = new System.Drawing.Point(397, 11);
      this.RestoreButton.Name = "RestoreButton";
      this.RestoreButton.Size = new System.Drawing.Size(75, 23);
      this.RestoreButton.TabIndex = 0;
      this.RestoreButton.Text = "Restore";
      this.RestoreButton.UseVisualStyleBackColor = true;
      this.RestoreButton.Click += new System.EventHandler(this.OpenButton_Click);
      // 
      // NothingButton
      // 
      this.NothingButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
      this.NothingButton.DialogResult = System.Windows.Forms.DialogResult.OK;
      this.NothingButton.Location = new System.Drawing.Point(316, 11);
      this.NothingButton.Name = "NothingButton";
      this.NothingButton.Size = new System.Drawing.Size(75, 23);
      this.NothingButton.TabIndex = 1;
      this.NothingButton.Text = "Nothing";
      this.NothingButton.UseVisualStyleBackColor = true;
      this.NothingButton.Click += new System.EventHandler(this.PersistButton_Click);
      // 
      // DeleteButton
      // 
      this.DeleteButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
      this.DeleteButton.DialogResult = System.Windows.Forms.DialogResult.OK;
      this.DeleteButton.Location = new System.Drawing.Point(235, 11);
      this.DeleteButton.Name = "DeleteButton";
      this.DeleteButton.Size = new System.Drawing.Size(75, 23);
      this.DeleteButton.TabIndex = 2;
      this.DeleteButton.Text = "Delete";
      this.DeleteButton.UseVisualStyleBackColor = true;
      this.DeleteButton.Click += new System.EventHandler(this.DiscardButton_Click);
      // 
      // RestoreEditSessionsDetailLabel
      // 
      this.RestoreEditSessionsDetailLabel.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.RestoreEditSessionsDetailLabel.Location = new System.Drawing.Point(95, 49);
      this.RestoreEditSessionsDetailLabel.Name = "RestoreEditSessionsDetailLabel";
      this.RestoreEditSessionsDetailLabel.Size = new System.Drawing.Size(377, 37);
      this.RestoreEditSessionsDetailLabel.TabIndex = 28;
      this.RestoreEditSessionsDetailLabel.Text = "The Excel workbook being opened contains saved Edit sessions.\r\nWhat do you want t" +
    "o do with them?";
      // 
      // RestoreEditSessionsDialog
      // 
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
      this.CancelButton = this.RestoreButton;
      this.ClientSize = new System.Drawing.Size(484, 151);
      this.CommandAreaVisible = true;
      this.FootnoteAreaHeight = 0;
      this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Sizable;
      this.MaximumSize = new System.Drawing.Size(500, 190);
      this.MinimumSize = new System.Drawing.Size(500, 190);
      this.Name = "RestoreEditSessionsDialog";
      this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
      this.Text = "MySQL for Excel";
      this.ContentAreaPanel.ResumeLayout(false);
      this.ContentAreaPanel.PerformLayout();
      this.CommandAreaPanel.ResumeLayout(false);
      ((System.ComponentModel.ISupportInitialize)(this.LogoPictureBox)).EndInit();
      this.ResumeLayout(false);

    }

    #endregion

    private System.Windows.Forms.PictureBox LogoPictureBox;
    private System.Windows.Forms.Label RestoreEditSessionsTitleLabel;
    private System.Windows.Forms.Button RestoreButton;
    private System.Windows.Forms.Button DeleteButton;
    private System.Windows.Forms.Button NothingButton;
    private System.Windows.Forms.Label RestoreEditSessionsDetailLabel;
  }
}