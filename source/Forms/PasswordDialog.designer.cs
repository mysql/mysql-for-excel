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
  partial class PasswordDialog
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
      this.components = new System.ComponentModel.Container();
      this.DialogOKButton = new System.Windows.Forms.Button();
      this.DialogCancelButton = new System.Windows.Forms.Button();
      this.UserValueLabel = new System.Windows.Forms.Label();
      this.ConnectionValueLabel = new System.Windows.Forms.Label();
      this.PasswordTextBox = new System.Windows.Forms.TextBox();
      this.PasswordLabel = new System.Windows.Forms.Label();
      this.UserLabel = new System.Windows.Forms.Label();
      this.ConnectionLabel = new System.Windows.Forms.Label();
      this.EnterPasswordLabel = new System.Windows.Forms.Label();
      this.LogoPictureBox = new System.Windows.Forms.PictureBox();
      this.StorePasswordSecurelyCheckBox = new System.Windows.Forms.CheckBox();
      this.PasswordChangedTimer = new System.Windows.Forms.Timer(this.components);
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
      this.ContentAreaPanel.Controls.Add(this.StorePasswordSecurelyCheckBox);
      this.ContentAreaPanel.Controls.Add(this.LogoPictureBox);
      this.ContentAreaPanel.Controls.Add(this.EnterPasswordLabel);
      this.ContentAreaPanel.Controls.Add(this.UserValueLabel);
      this.ContentAreaPanel.Controls.Add(this.PasswordTextBox);
      this.ContentAreaPanel.Controls.Add(this.ConnectionValueLabel);
      this.ContentAreaPanel.Controls.Add(this.ConnectionLabel);
      this.ContentAreaPanel.Controls.Add(this.UserLabel);
      this.ContentAreaPanel.Controls.Add(this.PasswordLabel);
      this.ContentAreaPanel.Size = new System.Drawing.Size(514, 226);
      // 
      // CommandAreaPanel
      // 
      this.CommandAreaPanel.Controls.Add(this.DialogOKButton);
      this.CommandAreaPanel.Controls.Add(this.DialogCancelButton);
      this.CommandAreaPanel.Location = new System.Drawing.Point(0, 181);
      this.CommandAreaPanel.Size = new System.Drawing.Size(514, 45);
      // 
      // DialogOKButton
      // 
      this.DialogOKButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
      this.DialogOKButton.DialogResult = System.Windows.Forms.DialogResult.OK;
      this.DialogOKButton.Enabled = false;
      this.DialogOKButton.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.DialogOKButton.Location = new System.Drawing.Point(346, 11);
      this.DialogOKButton.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
      this.DialogOKButton.Name = "DialogOKButton";
      this.DialogOKButton.Size = new System.Drawing.Size(75, 23);
      this.DialogOKButton.TabIndex = 0;
      this.DialogOKButton.Text = "OK";
      this.DialogOKButton.UseVisualStyleBackColor = true;
      this.DialogOKButton.Click += new System.EventHandler(this.DialogOKButton_Click);
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
      // UserValueLabel
      // 
      this.UserValueLabel.AutoSize = true;
      this.UserValueLabel.BackColor = System.Drawing.Color.Transparent;
      this.UserValueLabel.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.UserValueLabel.Location = new System.Drawing.Point(187, 87);
      this.UserValueLabel.Name = "UserValueLabel";
      this.UserValueLabel.Size = new System.Drawing.Size(17, 15);
      this.UserValueLabel.TabIndex = 4;
      this.UserValueLabel.Text = "??";
      // 
      // ConnectionValueLabel
      // 
      this.ConnectionValueLabel.AutoSize = true;
      this.ConnectionValueLabel.BackColor = System.Drawing.Color.Transparent;
      this.ConnectionValueLabel.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.ConnectionValueLabel.Location = new System.Drawing.Point(187, 64);
      this.ConnectionValueLabel.Name = "ConnectionValueLabel";
      this.ConnectionValueLabel.Size = new System.Drawing.Size(17, 15);
      this.ConnectionValueLabel.TabIndex = 2;
      this.ConnectionValueLabel.Text = "??";
      // 
      // PasswordTextBox
      // 
      this.PasswordTextBox.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.PasswordTextBox.Location = new System.Drawing.Point(187, 108);
      this.PasswordTextBox.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
      this.PasswordTextBox.Name = "PasswordTextBox";
      this.PasswordTextBox.Size = new System.Drawing.Size(315, 23);
      this.PasswordTextBox.TabIndex = 6;
      this.PasswordTextBox.UseSystemPasswordChar = true;
      this.PasswordTextBox.TextChanged += new System.EventHandler(this.PasswordTextBox_TextChanged);
      this.PasswordTextBox.Validated += new System.EventHandler(this.PasswordTextBox_Validated);
      // 
      // PasswordLabel
      // 
      this.PasswordLabel.AutoSize = true;
      this.PasswordLabel.BackColor = System.Drawing.Color.Transparent;
      this.PasswordLabel.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.PasswordLabel.Location = new System.Drawing.Point(103, 111);
      this.PasswordLabel.Name = "PasswordLabel";
      this.PasswordLabel.Size = new System.Drawing.Size(60, 15);
      this.PasswordLabel.TabIndex = 5;
      this.PasswordLabel.Text = "Password:";
      // 
      // UserLabel
      // 
      this.UserLabel.AutoSize = true;
      this.UserLabel.BackColor = System.Drawing.Color.Transparent;
      this.UserLabel.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.UserLabel.Location = new System.Drawing.Point(103, 87);
      this.UserLabel.Name = "UserLabel";
      this.UserLabel.Size = new System.Drawing.Size(33, 15);
      this.UserLabel.TabIndex = 3;
      this.UserLabel.Text = "User:";
      // 
      // ConnectionLabel
      // 
      this.ConnectionLabel.AutoSize = true;
      this.ConnectionLabel.BackColor = System.Drawing.Color.Transparent;
      this.ConnectionLabel.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.ConnectionLabel.Location = new System.Drawing.Point(103, 64);
      this.ConnectionLabel.Name = "ConnectionLabel";
      this.ConnectionLabel.Size = new System.Drawing.Size(72, 15);
      this.ConnectionLabel.TabIndex = 1;
      this.ConnectionLabel.Text = "Connection:";
      // 
      // EnterPasswordLabel
      // 
      this.EnterPasswordLabel.AutoSize = true;
      this.EnterPasswordLabel.Font = new System.Drawing.Font("Segoe UI", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.EnterPasswordLabel.ForeColor = System.Drawing.Color.Navy;
      this.EnterPasswordLabel.Location = new System.Drawing.Point(90, 27);
      this.EnterPasswordLabel.Name = "EnterPasswordLabel";
      this.EnterPasswordLabel.Size = new System.Drawing.Size(309, 20);
      this.EnterPasswordLabel.TabIndex = 0;
      this.EnterPasswordLabel.Text = "Please enter the password for the connection:";
      // 
      // LogoPictureBox
      // 
      this.LogoPictureBox.Image = global::MySQL.ForExcel.Properties.Resources.MySQLforExcel_Security;
      this.LogoPictureBox.Location = new System.Drawing.Point(20, 20);
      this.LogoPictureBox.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
      this.LogoPictureBox.Name = "LogoPictureBox";
      this.LogoPictureBox.Size = new System.Drawing.Size(64, 64);
      this.LogoPictureBox.TabIndex = 22;
      this.LogoPictureBox.TabStop = false;
      // 
      // StorePasswordSecurelyCheckBox
      // 
      this.StorePasswordSecurelyCheckBox.AutoSize = true;
      this.StorePasswordSecurelyCheckBox.Location = new System.Drawing.Point(190, 138);
      this.StorePasswordSecurelyCheckBox.Name = "StorePasswordSecurelyCheckBox";
      this.StorePasswordSecurelyCheckBox.Size = new System.Drawing.Size(147, 17);
      this.StorePasswordSecurelyCheckBox.TabIndex = 7;
      this.StorePasswordSecurelyCheckBox.Text = "Store password securely?";
      this.StorePasswordSecurelyCheckBox.UseVisualStyleBackColor = true;
      // 
      // PasswordChangedTimer
      // 
      this.PasswordChangedTimer.Interval = 800;
      this.PasswordChangedTimer.Tick += new System.EventHandler(this.PasswordChangedTimer_Tick);
      // 
      // PasswordDialog
      // 
      this.AcceptButton = this.DialogOKButton;
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
      this.CancelButton = this.DialogCancelButton;
      this.ClientSize = new System.Drawing.Size(514, 226);
      this.CommandAreaVisible = true;
      this.FootnoteAreaHeight = 0;
      this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
      this.MainInstructionLocation = new System.Drawing.Point(19, 19);
      this.MainInstructionLocationOffset = new System.Drawing.Size(-20, 5);
      this.Name = "PasswordDialog";
      this.Text = "Connection Password";
      this.ContentAreaPanel.ResumeLayout(false);
      this.ContentAreaPanel.PerformLayout();
      this.CommandAreaPanel.ResumeLayout(false);
      ((System.ComponentModel.ISupportInitialize)(this.LogoPictureBox)).EndInit();
      this.ResumeLayout(false);

    }

    #endregion

    private System.Windows.Forms.Button DialogOKButton;
    private System.Windows.Forms.Button DialogCancelButton;
    private System.Windows.Forms.Label UserValueLabel;
    private System.Windows.Forms.TextBox PasswordTextBox;
    private System.Windows.Forms.Label ConnectionValueLabel;
    private System.Windows.Forms.Label ConnectionLabel;
    private System.Windows.Forms.Label UserLabel;
    private System.Windows.Forms.Label PasswordLabel;
    private System.Windows.Forms.Label EnterPasswordLabel;
    private System.Windows.Forms.PictureBox LogoPictureBox;
    private System.Windows.Forms.CheckBox StorePasswordSecurelyCheckBox;
    private System.Windows.Forms.Timer PasswordChangedTimer;
  }
}