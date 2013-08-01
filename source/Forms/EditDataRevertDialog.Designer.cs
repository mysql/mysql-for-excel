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
  partial class EditDataRevertDialog
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
      this.OperationSummaryLabel = new System.Windows.Forms.Label();
      this.LogoPictureBox = new System.Windows.Forms.PictureBox();
      this.RevertDataLabel = new System.Windows.Forms.Label();
      this.DialogCancelButton = new System.Windows.Forms.Button();
      this.RevertDataButton = new System.Windows.Forms.Button();
      this.RefreshDataButton = new System.Windows.Forms.Button();
      this.ContentAreaPanel.SuspendLayout();
      this.CommandAreaPanel.SuspendLayout();
      ((System.ComponentModel.ISupportInitialize)(this.LogoPictureBox)).BeginInit();
      this.SuspendLayout();
      // 
      // FootnoteAreaPanel
      // 
      this.FootnoteAreaPanel.Location = new System.Drawing.Point(0, 71);
      this.FootnoteAreaPanel.Size = new System.Drawing.Size(484, 0);
      // 
      // ContentAreaPanel
      // 
      this.ContentAreaPanel.Controls.Add(this.OperationSummaryLabel);
      this.ContentAreaPanel.Controls.Add(this.LogoPictureBox);
      this.ContentAreaPanel.Controls.Add(this.RevertDataLabel);
      this.ContentAreaPanel.Size = new System.Drawing.Size(484, 151);
      // 
      // CommandAreaPanel
      // 
      this.CommandAreaPanel.Controls.Add(this.RefreshDataButton);
      this.CommandAreaPanel.Controls.Add(this.RevertDataButton);
      this.CommandAreaPanel.Controls.Add(this.DialogCancelButton);
      this.CommandAreaPanel.Location = new System.Drawing.Point(0, 106);
      this.CommandAreaPanel.Size = new System.Drawing.Size(484, 45);
      // 
      // OperationSummaryLabel
      // 
      this.OperationSummaryLabel.AutoSize = true;
      this.OperationSummaryLabel.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.OperationSummaryLabel.Location = new System.Drawing.Point(92, 56);
      this.OperationSummaryLabel.Name = "OperationSummaryLabel";
      this.OperationSummaryLabel.Size = new System.Drawing.Size(341, 30);
      this.OperationSummaryLabel.TabIndex = 26;
      this.OperationSummaryLabel.Text = "Reverting changes or refreshing data from the DB will cause\r\nyour changes to be l" +
    "ost. Click on the buttons below to proceed.";
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
      this.RevertDataLabel.Size = new System.Drawing.Size(87, 20);
      this.RevertDataLabel.TabIndex = 25;
      this.RevertDataLabel.Text = "Revert Data";
      // 
      // DialogCancelButton
      // 
      this.DialogCancelButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
      this.DialogCancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
      this.DialogCancelButton.Location = new System.Drawing.Point(397, 11);
      this.DialogCancelButton.Name = "DialogCancelButton";
      this.DialogCancelButton.Size = new System.Drawing.Size(75, 23);
      this.DialogCancelButton.TabIndex = 0;
      this.DialogCancelButton.Text = "Cancel";
      this.DialogCancelButton.UseVisualStyleBackColor = true;
      // 
      // RevertDataButton
      // 
      this.RevertDataButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
      this.RevertDataButton.DialogResult = System.Windows.Forms.DialogResult.OK;
      this.RevertDataButton.Location = new System.Drawing.Point(235, 11);
      this.RevertDataButton.Name = "RevertDataButton";
      this.RevertDataButton.Size = new System.Drawing.Size(156, 23);
      this.RevertDataButton.TabIndex = 1;
      this.RevertDataButton.Text = "Revert Changed Data";
      this.RevertDataButton.UseVisualStyleBackColor = true;
      this.RevertDataButton.Click += new System.EventHandler(this.RevertDataButton_Click);
      // 
      // RefreshDataButton
      // 
      this.RefreshDataButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
      this.RefreshDataButton.DialogResult = System.Windows.Forms.DialogResult.OK;
      this.RefreshDataButton.Location = new System.Drawing.Point(85, 11);
      this.RefreshDataButton.Name = "RefreshDataButton";
      this.RefreshDataButton.Size = new System.Drawing.Size(144, 23);
      this.RefreshDataButton.TabIndex = 2;
      this.RefreshDataButton.Text = "Refresh Data from DB";
      this.RefreshDataButton.UseVisualStyleBackColor = true;
      this.RefreshDataButton.Click += new System.EventHandler(this.RefreshDataButton_Click);
      // 
      // EditDataRevertDialog
      // 
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
      this.CancelButton = this.DialogCancelButton;
      this.ClientSize = new System.Drawing.Size(484, 151);
      this.CommandAreaVisible = true;
      this.FootnoteAreaHeight = 0;
      this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Sizable;
      this.MaximumSize = new System.Drawing.Size(500, 190);
      this.MinimumSize = new System.Drawing.Size(500, 190);
      this.Name = "EditDataRevertDialog";
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
    private System.Windows.Forms.Button DialogCancelButton;
    private System.Windows.Forms.Button RefreshDataButton;
    private System.Windows.Forms.Button RevertDataButton;
  }
}