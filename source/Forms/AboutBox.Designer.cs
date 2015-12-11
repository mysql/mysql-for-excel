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
      this.InstallerVersionLabel = new System.Windows.Forms.Label();
      this.ExcelVersionLabel = new System.Windows.Forms.Label();
      this.SuspendLayout();
      // 
      // InstallerVersionLabel
      // 
      this.InstallerVersionLabel.AutoSize = true;
      this.InstallerVersionLabel.BackColor = System.Drawing.Color.Transparent;
      this.InstallerVersionLabel.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.InstallerVersionLabel.ForeColor = System.Drawing.Color.White;
      this.InstallerVersionLabel.Location = new System.Drawing.Point(102, 136);
      this.InstallerVersionLabel.Name = "InstallerVersionLabel";
      this.InstallerVersionLabel.Size = new System.Drawing.Size(114, 15);
      this.InstallerVersionLabel.TabIndex = 5;
      this.InstallerVersionLabel.Text = "MySQL Installer 1.4";
      this.InstallerVersionLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
      // 
      // ExcelVersionLabel
      // 
      this.ExcelVersionLabel.AutoSize = true;
      this.ExcelVersionLabel.BackColor = System.Drawing.Color.Transparent;
      this.ExcelVersionLabel.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.ExcelVersionLabel.ForeColor = System.Drawing.Color.White;
      this.ExcelVersionLabel.Location = new System.Drawing.Point(102, 117);
      this.ExcelVersionLabel.Name = "ExcelVersionLabel";
      this.ExcelVersionLabel.Size = new System.Drawing.Size(128, 15);
      this.ExcelVersionLabel.TabIndex = 4;
      this.ExcelVersionLabel.Text = "MySQL for Excel 1.1.x";
      this.ExcelVersionLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
      // 
      // AboutBox
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.BackgroundImage = global::MySQL.ForExcel.Properties.Resources.SplashScreenExcel;
      this.ClientSize = new System.Drawing.Size(560, 322);
      this.Controls.Add(this.InstallerVersionLabel);
      this.Controls.Add(this.ExcelVersionLabel);
      this.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
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
      this.PerformLayout();

    }

    #endregion

    private System.Windows.Forms.Label InstallerVersionLabel;
    private System.Windows.Forms.Label ExcelVersionLabel;

  }
}
