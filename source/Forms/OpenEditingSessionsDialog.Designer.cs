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
  partial class OpenEditingSessionsDialog
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
      this.OperationSummaryLabel = new System.Windows.Forms.Label();
      this.LogoPictureBox = new System.Windows.Forms.PictureBox();
      this.RevertDataLabel = new System.Windows.Forms.Label();
      this.OpenButton = new System.Windows.Forms.Button();
      this.PersistButton = new System.Windows.Forms.Button();
      this.DiscardButton = new System.Windows.Forms.Button();
      this.OperationSummaryWarningLabel = new System.Windows.Forms.Label();
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
      this.ContentAreaPanel.Controls.Add(this.OperationSummaryWarningLabel);
      this.ContentAreaPanel.Controls.Add(this.OperationSummaryLabel);
      this.ContentAreaPanel.Controls.Add(this.LogoPictureBox);
      this.ContentAreaPanel.Controls.Add(this.RevertDataLabel);
      this.ContentAreaPanel.Size = new System.Drawing.Size(484, 151);
      // 
      // CommandAreaPanel
      // 
      this.CommandAreaPanel.Controls.Add(this.DiscardButton);
      this.CommandAreaPanel.Controls.Add(this.PersistButton);
      this.CommandAreaPanel.Controls.Add(this.OpenButton);
      this.CommandAreaPanel.Location = new System.Drawing.Point(0, 106);
      this.CommandAreaPanel.Size = new System.Drawing.Size(484, 45);
      // 
      // OperationSummaryLabel
      // 
      this.OperationSummaryLabel.AutoSize = true;
      this.OperationSummaryLabel.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.OperationSummaryLabel.Location = new System.Drawing.Point(92, 56);
      this.OperationSummaryLabel.Name = "OperationSummaryLabel";
      this.OperationSummaryLabel.Size = new System.Drawing.Size(283, 15);
      this.OperationSummaryLabel.TabIndex = 26;
      this.OperationSummaryLabel.Text = "There are saved sessions, what would you like to do?";
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
      // RevertDataLabel
      // 
      this.RevertDataLabel.AutoSize = true;
      this.RevertDataLabel.Font = new System.Drawing.Font("Segoe UI", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.RevertDataLabel.ForeColor = System.Drawing.Color.Navy;
      this.RevertDataLabel.Location = new System.Drawing.Point(91, 29);
      this.RevertDataLabel.Name = "RevertDataLabel";
      this.RevertDataLabel.Size = new System.Drawing.Size(199, 20);
      this.RevertDataLabel.TabIndex = 25;
      this.RevertDataLabel.Text = "Open Saved Editing Sessions";
      // 
      // OpenButton
      // 
      this.OpenButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
      this.OpenButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
      this.OpenButton.Location = new System.Drawing.Point(397, 11);
      this.OpenButton.Name = "OpenButton";
      this.OpenButton.Size = new System.Drawing.Size(75, 23);
      this.OpenButton.TabIndex = 0;
      this.OpenButton.Text = "Open";
      this.OpenButton.UseVisualStyleBackColor = true;
      this.OpenButton.Click += new System.EventHandler(this.OpenButton_Click);
      // 
      // PersistButton
      // 
      this.PersistButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
      this.PersistButton.DialogResult = System.Windows.Forms.DialogResult.OK;
      this.PersistButton.Location = new System.Drawing.Point(316, 11);
      this.PersistButton.Name = "PersistButton";
      this.PersistButton.Size = new System.Drawing.Size(75, 23);
      this.PersistButton.TabIndex = 1;
      this.PersistButton.Text = "Persist";
      this.PersistButton.UseVisualStyleBackColor = true;
      this.PersistButton.Click += new System.EventHandler(this.PersistButton_Click);
      // 
      // DiscardButton
      // 
      this.DiscardButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
      this.DiscardButton.DialogResult = System.Windows.Forms.DialogResult.OK;
      this.DiscardButton.Location = new System.Drawing.Point(235, 11);
      this.DiscardButton.Name = "DiscardButton";
      this.DiscardButton.Size = new System.Drawing.Size(75, 23);
      this.DiscardButton.TabIndex = 2;
      this.DiscardButton.Text = "Discard";
      this.DiscardButton.UseVisualStyleBackColor = true;
      this.DiscardButton.Click += new System.EventHandler(this.DiscardButton_Click);
      // 
      // OperationSummaryWarningLabel
      // 
      this.OperationSummaryWarningLabel.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.OperationSummaryWarningLabel.Location = new System.Drawing.Point(92, 56);
      this.OperationSummaryWarningLabel.Name = "OperationSummaryWarningLabel";
      this.OperationSummaryWarningLabel.Size = new System.Drawing.Size(380, 37);
      this.OperationSummaryWarningLabel.TabIndex = 28;
      this.OperationSummaryWarningLabel.Text = "Saved sessions belong to schema \'{0}\' and cannot be opened, the current schema \'{" +
    "1}\' contains active sessions.";
      this.OperationSummaryWarningLabel.Visible = false;
      // 
      // OpenEditingSessionsDialog
      // 
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
      this.CancelButton = this.OpenButton;
      this.ClientSize = new System.Drawing.Size(484, 151);
      this.CommandAreaVisible = true;
      this.FootnoteAreaHeight = 0;
      this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Sizable;
      this.MaximumSize = new System.Drawing.Size(500, 190);
      this.MinimumSize = new System.Drawing.Size(500, 190);
      this.Name = "OpenEditingSessionsDialog";
      this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
      this.Text = "MySQL for Excel";
      this.ContentAreaPanel.ResumeLayout(false);
      this.ContentAreaPanel.PerformLayout();
      this.CommandAreaPanel.ResumeLayout(false);
      ((System.ComponentModel.ISupportInitialize)(this.LogoPictureBox)).EndInit();
      this.ResumeLayout(false);

    }

    #endregion

    private System.Windows.Forms.Label OperationSummaryLabel;
    private System.Windows.Forms.PictureBox LogoPictureBox;
    private System.Windows.Forms.Label RevertDataLabel;
    private System.Windows.Forms.Button OpenButton;
    private System.Windows.Forms.Button DiscardButton;
    private System.Windows.Forms.Button PersistButton;
    private System.Windows.Forms.Label OperationSummaryWarningLabel;
  }
}