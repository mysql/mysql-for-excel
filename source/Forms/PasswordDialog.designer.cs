// Copyright (c) 2012-2014, Oracle and/or its affiliates. All rights reserved.
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
      if (disposing)
      {
        if (components != null)
        {
          components.Dispose();
        }

        // Set variables to null so this object does not hold references to them and the GC disposes of them sooner.
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
      this.NewPasswordTextBox = new System.Windows.Forms.TextBox();
      this.NewPasswordLabel = new System.Windows.Forms.Label();
      this.ConfirmTextBox = new System.Windows.Forms.TextBox();
      this.ConfirmLabel = new System.Windows.Forms.Label();
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
      this.ContentAreaPanel.Controls.Add(this.ConfirmTextBox);
      this.ContentAreaPanel.Controls.Add(this.ConfirmLabel);
      this.ContentAreaPanel.Controls.Add(this.NewPasswordTextBox);
      this.ContentAreaPanel.Controls.Add(this.NewPasswordLabel);
      this.ContentAreaPanel.Controls.Add(this.StorePasswordSecurelyCheckBox);
      this.ContentAreaPanel.Controls.Add(this.LogoPictureBox);
      this.ContentAreaPanel.Controls.Add(this.EnterPasswordLabel);
      this.ContentAreaPanel.Controls.Add(this.UserValueLabel);
      this.ContentAreaPanel.Controls.Add(this.PasswordTextBox);
      this.ContentAreaPanel.Controls.Add(this.ConnectionValueLabel);
      this.ContentAreaPanel.Controls.Add(this.ConnectionLabel);
      this.ContentAreaPanel.Controls.Add(this.UserLabel);
      this.ContentAreaPanel.Controls.Add(this.PasswordLabel);
      this.ContentAreaPanel.Size = new System.Drawing.Size(514, 286);
      // 
      // CommandAreaPanel
      // 
      this.CommandAreaPanel.Controls.Add(this.DialogOKButton);
      this.CommandAreaPanel.Controls.Add(this.DialogCancelButton);
      this.CommandAreaPanel.Location = new System.Drawing.Point(0, 241);
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
      this.UserValueLabel.Location = new System.Drawing.Point(193, 87);
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
      this.ConnectionValueLabel.Location = new System.Drawing.Point(193, 64);
      this.ConnectionValueLabel.Name = "ConnectionValueLabel";
      this.ConnectionValueLabel.Size = new System.Drawing.Size(17, 15);
      this.ConnectionValueLabel.TabIndex = 2;
      this.ConnectionValueLabel.Text = "??";
      // 
      // PasswordTextBox
      // 
      this.PasswordTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
      this.PasswordTextBox.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.PasswordTextBox.Location = new System.Drawing.Point(196, 110);
      this.PasswordTextBox.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
      this.PasswordTextBox.Name = "PasswordTextBox";
      this.PasswordTextBox.Size = new System.Drawing.Size(306, 23);
      this.PasswordTextBox.TabIndex = 6;
      this.PasswordTextBox.UseSystemPasswordChar = true;
      this.PasswordTextBox.TextChanged += new System.EventHandler(this.PasswordTextBoxTextChanged);
      this.PasswordTextBox.Validated += new System.EventHandler(this.PasswordTextBoxValidated);
      // 
      // PasswordLabel
      // 
      this.PasswordLabel.BackColor = System.Drawing.Color.Transparent;
      this.PasswordLabel.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.PasswordLabel.Location = new System.Drawing.Point(108, 113);
      this.PasswordLabel.Name = "PasswordLabel";
      this.PasswordLabel.Size = new System.Drawing.Size(82, 15);
      this.PasswordLabel.TabIndex = 5;
      this.PasswordLabel.Text = "Old Password:";
      this.PasswordLabel.TextAlign = System.Drawing.ContentAlignment.TopRight;
      // 
      // UserLabel
      // 
      this.UserLabel.AutoSize = true;
      this.UserLabel.BackColor = System.Drawing.Color.Transparent;
      this.UserLabel.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.UserLabel.Location = new System.Drawing.Point(154, 87);
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
      this.ConnectionLabel.Location = new System.Drawing.Point(115, 64);
      this.ConnectionLabel.Name = "ConnectionLabel";
      this.ConnectionLabel.Size = new System.Drawing.Size(72, 15);
      this.ConnectionLabel.TabIndex = 1;
      this.ConnectionLabel.Text = "Connection:";
      // 
      // EnterPasswordLabel
      // 
      this.EnterPasswordLabel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
      this.EnterPasswordLabel.Font = new System.Drawing.Font("Segoe UI", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.EnterPasswordLabel.ForeColor = System.Drawing.Color.Navy;
      this.EnterPasswordLabel.Location = new System.Drawing.Point(90, 20);
      this.EnterPasswordLabel.Name = "EnterPasswordLabel";
      this.EnterPasswordLabel.Size = new System.Drawing.Size(412, 44);
      this.EnterPasswordLabel.TabIndex = 0;
      this.EnterPasswordLabel.Text = "Connection password...";
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
      this.StorePasswordSecurelyCheckBox.Location = new System.Drawing.Point(196, 202);
      this.StorePasswordSecurelyCheckBox.Name = "StorePasswordSecurelyCheckBox";
      this.StorePasswordSecurelyCheckBox.Size = new System.Drawing.Size(147, 17);
      this.StorePasswordSecurelyCheckBox.TabIndex = 11;
      this.StorePasswordSecurelyCheckBox.Text = "Store password securely?";
      this.StorePasswordSecurelyCheckBox.UseVisualStyleBackColor = true;
      // 
      // PasswordChangedTimer
      // 
      this.PasswordChangedTimer.Interval = 500;
      this.PasswordChangedTimer.Tick += new System.EventHandler(this.PasswordChangedTimer_Tick);
      // 
      // NewPasswordTextBox
      // 
      this.NewPasswordTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
      this.NewPasswordTextBox.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.NewPasswordTextBox.Location = new System.Drawing.Point(196, 141);
      this.NewPasswordTextBox.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
      this.NewPasswordTextBox.Name = "NewPasswordTextBox";
      this.NewPasswordTextBox.Size = new System.Drawing.Size(306, 23);
      this.NewPasswordTextBox.TabIndex = 8;
      this.NewPasswordTextBox.UseSystemPasswordChar = true;
      this.NewPasswordTextBox.TextChanged += new System.EventHandler(this.PasswordTextBoxTextChanged);
      this.NewPasswordTextBox.Validated += new System.EventHandler(this.PasswordTextBoxValidated);
      // 
      // NewPasswordLabel
      // 
      this.NewPasswordLabel.AutoSize = true;
      this.NewPasswordLabel.BackColor = System.Drawing.Color.Transparent;
      this.NewPasswordLabel.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.NewPasswordLabel.Location = new System.Drawing.Point(103, 144);
      this.NewPasswordLabel.Name = "NewPasswordLabel";
      this.NewPasswordLabel.Size = new System.Drawing.Size(87, 15);
      this.NewPasswordLabel.TabIndex = 7;
      this.NewPasswordLabel.Text = "New Password:";
      // 
      // ConfirmTextBox
      // 
      this.ConfirmTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
      this.ConfirmTextBox.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.ConfirmTextBox.Location = new System.Drawing.Point(196, 172);
      this.ConfirmTextBox.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
      this.ConfirmTextBox.Name = "ConfirmTextBox";
      this.ConfirmTextBox.Size = new System.Drawing.Size(306, 23);
      this.ConfirmTextBox.TabIndex = 10;
      this.ConfirmTextBox.UseSystemPasswordChar = true;
      this.ConfirmTextBox.TextChanged += new System.EventHandler(this.PasswordTextBoxTextChanged);
      this.ConfirmTextBox.Validated += new System.EventHandler(this.PasswordTextBoxValidated);
      // 
      // ConfirmLabel
      // 
      this.ConfirmLabel.AutoSize = true;
      this.ConfirmLabel.BackColor = System.Drawing.Color.Transparent;
      this.ConfirmLabel.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.ConfirmLabel.Location = new System.Drawing.Point(133, 175);
      this.ConfirmLabel.Name = "ConfirmLabel";
      this.ConfirmLabel.Size = new System.Drawing.Size(54, 15);
      this.ConfirmLabel.TabIndex = 9;
      this.ConfirmLabel.Text = "Confirm:";
      // 
      // PasswordDialog
      // 
      this.AcceptButton = this.DialogOKButton;
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
      this.CancelButton = this.DialogCancelButton;
      this.ClientSize = new System.Drawing.Size(514, 286);
      this.CommandAreaVisible = true;
      this.FootnoteAreaHeight = 0;
      this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
      this.MainInstructionLocation = new System.Drawing.Point(19, 19);
      this.MainInstructionLocationOffset = new System.Drawing.Size(-20, 5);
      this.Name = "PasswordDialog";
      this.Text = "Connection Password";
      this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.PasswordDialog_FormClosing);
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
    private System.Windows.Forms.TextBox ConfirmTextBox;
    private System.Windows.Forms.Label ConfirmLabel;
    private System.Windows.Forms.TextBox NewPasswordTextBox;
    private System.Windows.Forms.Label NewPasswordLabel;
  }
}