// Copyright (c) 2014, Oracle and/or its affiliates. All rights reserved.
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
  partial class MySqlScriptDialog
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
      if (disposing)
      {
        if (components != null)
        {
          components.Dispose();
        }

        // Set variables to null so this object does not hold references to them and the GC disposes of them sooner.
        _mySqlTable = null;
        _wbConnection = null;
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
      this.ApplyButton = new System.Windows.Forms.Button();
      this.DialogCancelButton = new System.Windows.Forms.Button();
      this.TitleLabel = new System.Windows.Forms.Label();
      this.QueryTextBox = new System.Windows.Forms.RichTextBox();
      this.QueryEditorContextMenuStrip = new System.Windows.Forms.ContextMenuStrip(this.components);
      this.ZoomInToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
      this.ZoomOutToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
      this.ZoomResetToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
      this.OriginalQueryButton = new System.Windows.Forms.Button();
      this.QueryChangedTimer = new System.Windows.Forms.Timer(this.components);
      this.QueryWarningLabel = new System.Windows.Forms.Label();
      this.QueryWarningPictureBox = new System.Windows.Forms.PictureBox();
      this.OriginalOperationsLabel = new System.Windows.Forms.Label();
      this.ContentAreaPanel.SuspendLayout();
      this.CommandAreaPanel.SuspendLayout();
      this.QueryEditorContextMenuStrip.SuspendLayout();
      ((System.ComponentModel.ISupportInitialize)(this.QueryWarningPictureBox)).BeginInit();
      this.SuspendLayout();
      // 
      // FootnoteAreaPanel
      // 
      this.FootnoteAreaPanel.Location = new System.Drawing.Point(0, 292);
      this.FootnoteAreaPanel.Size = new System.Drawing.Size(634, 0);
      // 
      // ContentAreaPanel
      // 
      this.ContentAreaPanel.Controls.Add(this.QueryTextBox);
      this.ContentAreaPanel.Controls.Add(this.QueryWarningLabel);
      this.ContentAreaPanel.Controls.Add(this.QueryWarningPictureBox);
      this.ContentAreaPanel.Controls.Add(this.TitleLabel);
      this.ContentAreaPanel.Controls.Add(this.OriginalOperationsLabel);
      this.ContentAreaPanel.Size = new System.Drawing.Size(884, 461);
      // 
      // CommandAreaPanel
      // 
      this.CommandAreaPanel.Controls.Add(this.OriginalQueryButton);
      this.CommandAreaPanel.Controls.Add(this.ApplyButton);
      this.CommandAreaPanel.Controls.Add(this.DialogCancelButton);
      this.CommandAreaPanel.Location = new System.Drawing.Point(0, 416);
      this.CommandAreaPanel.Size = new System.Drawing.Size(884, 45);
      // 
      // ApplyButton
      // 
      this.ApplyButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
      this.ApplyButton.Enabled = false;
      this.ApplyButton.Location = new System.Drawing.Point(716, 12);
      this.ApplyButton.Name = "ApplyButton";
      this.ApplyButton.Size = new System.Drawing.Size(75, 23);
      this.ApplyButton.TabIndex = 1;
      this.ApplyButton.Text = "Apply";
      this.ApplyButton.UseVisualStyleBackColor = true;
      this.ApplyButton.Click += new System.EventHandler(this.ApplyButton_Click);
      // 
      // DialogCancelButton
      // 
      this.DialogCancelButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
      this.DialogCancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
      this.DialogCancelButton.Location = new System.Drawing.Point(797, 12);
      this.DialogCancelButton.Name = "DialogCancelButton";
      this.DialogCancelButton.Size = new System.Drawing.Size(75, 23);
      this.DialogCancelButton.TabIndex = 2;
      this.DialogCancelButton.Text = "Cancel";
      this.DialogCancelButton.UseVisualStyleBackColor = true;
      this.DialogCancelButton.Click += new System.EventHandler(this.DialogCancelButton_Click);
      // 
      // TitleLabel
      // 
      this.TitleLabel.AutoSize = true;
      this.TitleLabel.Font = new System.Drawing.Font("Segoe UI", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.TitleLabel.ForeColor = System.Drawing.Color.Navy;
      this.TitleLabel.Location = new System.Drawing.Point(11, 19);
      this.TitleLabel.Name = "TitleLabel";
      this.TitleLabel.Size = new System.Drawing.Size(409, 20);
      this.TitleLabel.TabIndex = 0;
      this.TitleLabel.Text = "Review the SQL script to be applied on the MySQL database.";
      // 
      // QueryTextBox
      // 
      this.QueryTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
      this.QueryTextBox.AutoWordSelection = true;
      this.QueryTextBox.ContextMenuStrip = this.QueryEditorContextMenuStrip;
      this.QueryTextBox.DetectUrls = false;
      this.QueryTextBox.EnableAutoDragDrop = true;
      this.QueryTextBox.Font = new System.Drawing.Font("Consolas", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.QueryTextBox.HideSelection = false;
      this.QueryTextBox.Location = new System.Drawing.Point(12, 68);
      this.QueryTextBox.Name = "QueryTextBox";
      this.QueryTextBox.Size = new System.Drawing.Size(860, 317);
      this.QueryTextBox.TabIndex = 1;
      this.QueryTextBox.Text = "";
      this.QueryTextBox.WordWrap = false;
      this.QueryTextBox.TextChanged += new System.EventHandler(this.QueryTextBox_TextChanged);
      this.QueryTextBox.Validated += new System.EventHandler(this.QueryTextBox_Validated);
      // 
      // QueryEditorContextMenuStrip
      // 
      this.QueryEditorContextMenuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.ZoomInToolStripMenuItem,
            this.ZoomOutToolStripMenuItem,
            this.ZoomResetToolStripMenuItem});
      this.QueryEditorContextMenuStrip.Name = "QueryEditorContextMenuStrip";
      this.QueryEditorContextMenuStrip.Size = new System.Drawing.Size(138, 70);
      // 
      // ZoomInToolStripMenuItem
      // 
      this.ZoomInToolStripMenuItem.Image = global::MySQL.ForExcel.Properties.Resources.zoom_in;
      this.ZoomInToolStripMenuItem.Name = "ZoomInToolStripMenuItem";
      this.ZoomInToolStripMenuItem.Size = new System.Drawing.Size(137, 22);
      this.ZoomInToolStripMenuItem.Text = "Zoom In";
      this.ZoomInToolStripMenuItem.Click += new System.EventHandler(this.ZoomInToolStripMenuItem_Click);
      // 
      // ZoomOutToolStripMenuItem
      // 
      this.ZoomOutToolStripMenuItem.Image = global::MySQL.ForExcel.Properties.Resources.zoom_out;
      this.ZoomOutToolStripMenuItem.Name = "ZoomOutToolStripMenuItem";
      this.ZoomOutToolStripMenuItem.Size = new System.Drawing.Size(137, 22);
      this.ZoomOutToolStripMenuItem.Text = "Zoom Out";
      this.ZoomOutToolStripMenuItem.Click += new System.EventHandler(this.ZoomOutToolStripMenuItem_Click);
      // 
      // ZoomResetToolStripMenuItem
      // 
      this.ZoomResetToolStripMenuItem.Image = global::MySQL.ForExcel.Properties.Resources.zoom_reset;
      this.ZoomResetToolStripMenuItem.Name = "ZoomResetToolStripMenuItem";
      this.ZoomResetToolStripMenuItem.Size = new System.Drawing.Size(137, 22);
      this.ZoomResetToolStripMenuItem.Text = "Zoom Reset";
      this.ZoomResetToolStripMenuItem.Click += new System.EventHandler(this.ZoomResetToolStripMenuItem_Click);
      // 
      // OriginalQueryButton
      // 
      this.OriginalQueryButton.Enabled = false;
      this.OriginalQueryButton.Location = new System.Drawing.Point(12, 12);
      this.OriginalQueryButton.Name = "OriginalQueryButton";
      this.OriginalQueryButton.Size = new System.Drawing.Size(116, 23);
      this.OriginalQueryButton.TabIndex = 0;
      this.OriginalQueryButton.Text = "Original Query";
      this.OriginalQueryButton.UseVisualStyleBackColor = true;
      this.OriginalQueryButton.Click += new System.EventHandler(this.OriginalQueryButton_Click);
      // 
      // QueryChangedTimer
      // 
      this.QueryChangedTimer.Interval = 800;
      this.QueryChangedTimer.Tick += new System.EventHandler(this.QueryChangedTimer_Tick);
      // 
      // QueryWarningLabel
      // 
      this.QueryWarningLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
      this.QueryWarningLabel.AutoSize = true;
      this.QueryWarningLabel.BackColor = System.Drawing.Color.Transparent;
      this.QueryWarningLabel.Font = new System.Drawing.Font("Segoe UI", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.QueryWarningLabel.ForeColor = System.Drawing.Color.Red;
      this.QueryWarningLabel.Location = new System.Drawing.Point(35, 393);
      this.QueryWarningLabel.Name = "QueryWarningLabel";
      this.QueryWarningLabel.Size = new System.Drawing.Size(348, 12);
      this.QueryWarningLabel.TabIndex = 39;
      this.QueryWarningLabel.Text = "The highlighted statements exceed the MySQL server\'s maximum allowed packet value" +
    ",";
      this.QueryWarningLabel.Visible = false;
      // 
      // QueryWarningPictureBox
      // 
      this.QueryWarningPictureBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
      this.QueryWarningPictureBox.BackColor = System.Drawing.Color.Transparent;
      this.QueryWarningPictureBox.Image = global::MySQL.ForExcel.Properties.Resources.Warning;
      this.QueryWarningPictureBox.Location = new System.Drawing.Point(12, 388);
      this.QueryWarningPictureBox.Name = "QueryWarningPictureBox";
      this.QueryWarningPictureBox.Size = new System.Drawing.Size(20, 20);
      this.QueryWarningPictureBox.TabIndex = 40;
      this.QueryWarningPictureBox.TabStop = false;
      this.QueryWarningPictureBox.Visible = false;
      // 
      // OriginalOperationsLabel
      // 
      this.OriginalOperationsLabel.AutoSize = true;
      this.OriginalOperationsLabel.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.OriginalOperationsLabel.Location = new System.Drawing.Point(12, 48);
      this.OriginalOperationsLabel.Name = "OriginalOperationsLabel";
      this.OriginalOperationsLabel.Size = new System.Drawing.Size(530, 15);
      this.OriginalOperationsLabel.TabIndex = 41;
      this.OriginalOperationsLabel.Text = "Creating Table \'??\', Deleting ??, Inserting ??, Updating ?? row(s) with the follo" +
    "wing SQL statement(s):";
      // 
      // MySqlScriptDialog
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.CancelButton = this.DialogCancelButton;
      this.ClientSize = new System.Drawing.Size(884, 461);
      this.CommandAreaVisible = true;
      this.FootnoteAreaHeight = 0;
      this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Sizable;
      this.MinimumSize = new System.Drawing.Size(450, 300);
      this.Name = "MySqlScriptDialog";
      this.Text = "Review SQL Script";
      this.Controls.SetChildIndex(this.FootnoteAreaPanel, 0);
      this.Controls.SetChildIndex(this.ContentAreaPanel, 0);
      this.Controls.SetChildIndex(this.CommandAreaPanel, 0);
      this.ContentAreaPanel.ResumeLayout(false);
      this.ContentAreaPanel.PerformLayout();
      this.CommandAreaPanel.ResumeLayout(false);
      this.QueryEditorContextMenuStrip.ResumeLayout(false);
      ((System.ComponentModel.ISupportInitialize)(this.QueryWarningPictureBox)).EndInit();
      this.ResumeLayout(false);

    }

    #endregion

    private System.Windows.Forms.Button ApplyButton;
    private System.Windows.Forms.Button DialogCancelButton;
    private System.Windows.Forms.Label TitleLabel;
    private System.Windows.Forms.RichTextBox QueryTextBox;
    private System.Windows.Forms.Button OriginalQueryButton;
    private System.Windows.Forms.ContextMenuStrip QueryEditorContextMenuStrip;
    private System.Windows.Forms.ToolStripMenuItem ZoomInToolStripMenuItem;
    private System.Windows.Forms.ToolStripMenuItem ZoomOutToolStripMenuItem;
    private System.Windows.Forms.ToolStripMenuItem ZoomResetToolStripMenuItem;
    private System.Windows.Forms.Timer QueryChangedTimer;
    private System.Windows.Forms.Label QueryWarningLabel;
    private System.Windows.Forms.PictureBox QueryWarningPictureBox;
    private System.Windows.Forms.Label OriginalOperationsLabel;
  }
}