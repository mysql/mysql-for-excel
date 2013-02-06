//
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
//

namespace MySQL.ForExcel
{
  partial class AboutBox
  {
    /// <summary>
    /// Required designer variable.
    /// </summary>
    private System.ComponentModel.IContainer components = null;

    /// <summary>
    /// Clean up any resources being used.
    /// </summary>
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
      this.lblExcelVersion = new System.Windows.Forms.Label();
      this.lblInstallerVersion = new System.Windows.Forms.Label();
      this.SuspendLayout();
      // 
      // lblExcelVersion
      // 
      this.lblExcelVersion.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
      this.lblExcelVersion.BackColor = System.Drawing.Color.Transparent;
      this.lblExcelVersion.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.lblExcelVersion.ForeColor = System.Drawing.SystemColors.ControlDarkDark;
      this.lblExcelVersion.Location = new System.Drawing.Point(455, 125);
      this.lblExcelVersion.Name = "lblExcelVersion";
      this.lblExcelVersion.Size = new System.Drawing.Size(96, 13);
      this.lblExcelVersion.TabIndex = 0;
      this.lblExcelVersion.Text = "Version 1.1.2";
      this.lblExcelVersion.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
      // 
      // lblInstallerVersion
      // 
      this.lblInstallerVersion.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
      this.lblInstallerVersion.BackColor = System.Drawing.Color.Transparent;
      this.lblInstallerVersion.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.lblInstallerVersion.ForeColor = System.Drawing.SystemColors.ControlDarkDark;
      this.lblInstallerVersion.Location = new System.Drawing.Point(431, 143);
      this.lblInstallerVersion.Name = "lblInstallerVersion";
      this.lblInstallerVersion.Size = new System.Drawing.Size(120, 13);
      this.lblInstallerVersion.TabIndex = 1;
      this.lblInstallerVersion.Text = "MySQL Installer 1.1";
      this.lblInstallerVersion.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
      // 
      // AboutBox
      // 
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
      this.BackgroundImage = global::MySQL.ForExcel.Properties.Resources.SplashScreenExcel;
      this.ClientSize = new System.Drawing.Size(557, 271);
      this.Controls.Add(this.lblInstallerVersion);
      this.Controls.Add(this.lblExcelVersion);
      this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
      this.MaximizeBox = false;
      this.MinimizeBox = false;
      this.Name = "AboutBox";
      this.Padding = new System.Windows.Forms.Padding(9);
      this.ShowIcon = false;
      this.ShowInTaskbar = false;
      this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
      this.Text = "AboutBox";
      this.Click += new System.EventHandler(this.AboutBox_Click);
      this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.AboutBox_KeyDown);
      this.ResumeLayout(false);

    }

    #endregion

    private System.Windows.Forms.Label lblExcelVersion;
    private System.Windows.Forms.Label lblInstallerVersion;
  }
}
