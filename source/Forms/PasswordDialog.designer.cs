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
      this.btnOK = new System.Windows.Forms.Button();
      this.btnCancel = new System.Windows.Forms.Button();
      this.lblUserValue = new System.Windows.Forms.Label();
      this.lblConnectionValue = new System.Windows.Forms.Label();
      this.txtPassword = new System.Windows.Forms.TextBox();
      this.lblPassword = new System.Windows.Forms.Label();
      this.lblUser = new System.Windows.Forms.Label();
      this.lblConnection = new System.Windows.Forms.Label();
      this.lblEnterPassword = new System.Windows.Forms.Label();
      this.picLogo = new System.Windows.Forms.PictureBox();
      this.contentAreaPanel.SuspendLayout();
      this.commandAreaPanel.SuspendLayout();
      ((System.ComponentModel.ISupportInitialize)(this.picLogo)).BeginInit();
      this.SuspendLayout();
      // 
      // contentAreaPanel
      // 
      this.contentAreaPanel.BackColor = System.Drawing.SystemColors.Window;
      this.contentAreaPanel.Controls.Add(this.picLogo);
      this.contentAreaPanel.Controls.Add(this.lblEnterPassword);
      this.contentAreaPanel.Controls.Add(this.lblUserValue);
      this.contentAreaPanel.Controls.Add(this.txtPassword);
      this.contentAreaPanel.Controls.Add(this.lblConnectionValue);
      this.contentAreaPanel.Controls.Add(this.lblConnection);
      this.contentAreaPanel.Controls.Add(this.lblUser);
      this.contentAreaPanel.Controls.Add(this.lblPassword);
      this.contentAreaPanel.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
      this.contentAreaPanel.Size = new System.Drawing.Size(514, 145);
      // 
      // commandAreaPanel
      // 
      this.commandAreaPanel.Controls.Add(this.btnOK);
      this.commandAreaPanel.Controls.Add(this.btnCancel);
      this.commandAreaPanel.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
      this.commandAreaPanel.Location = new System.Drawing.Point(0, 145);
      this.commandAreaPanel.Size = new System.Drawing.Size(514, 45);
      // 
      // btnOK
      // 
      this.btnOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
      this.btnOK.DialogResult = System.Windows.Forms.DialogResult.OK;
      this.btnOK.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.btnOK.Location = new System.Drawing.Point(346, 11);
      this.btnOK.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
      this.btnOK.Name = "btnOK";
      this.btnOK.Size = new System.Drawing.Size(75, 23);
      this.btnOK.TabIndex = 0;
      this.btnOK.Text = "OK";
      this.btnOK.UseVisualStyleBackColor = true;
      // 
      // btnCancel
      // 
      this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
      this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
      this.btnCancel.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.btnCancel.Location = new System.Drawing.Point(427, 11);
      this.btnCancel.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
      this.btnCancel.Name = "btnCancel";
      this.btnCancel.Size = new System.Drawing.Size(75, 23);
      this.btnCancel.TabIndex = 1;
      this.btnCancel.Text = "Cancel";
      this.btnCancel.UseVisualStyleBackColor = true;
      // 
      // lblUserValue
      // 
      this.lblUserValue.AutoSize = true;
      this.lblUserValue.BackColor = System.Drawing.Color.Transparent;
      this.lblUserValue.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.lblUserValue.Location = new System.Drawing.Point(187, 87);
      this.lblUserValue.Name = "lblUserValue";
      this.lblUserValue.Size = new System.Drawing.Size(17, 15);
      this.lblUserValue.TabIndex = 4;
      this.lblUserValue.Text = "??";
      // 
      // lblConnectionValue
      // 
      this.lblConnectionValue.AutoSize = true;
      this.lblConnectionValue.BackColor = System.Drawing.Color.Transparent;
      this.lblConnectionValue.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.lblConnectionValue.Location = new System.Drawing.Point(187, 64);
      this.lblConnectionValue.Name = "lblConnectionValue";
      this.lblConnectionValue.Size = new System.Drawing.Size(17, 15);
      this.lblConnectionValue.TabIndex = 2;
      this.lblConnectionValue.Text = "??";
      // 
      // txtPassword
      // 
      this.txtPassword.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.txtPassword.Location = new System.Drawing.Point(187, 108);
      this.txtPassword.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
      this.txtPassword.Name = "txtPassword";
      this.txtPassword.Size = new System.Drawing.Size(315, 23);
      this.txtPassword.TabIndex = 6;
      this.txtPassword.UseSystemPasswordChar = true;
      // 
      // lblPassword
      // 
      this.lblPassword.AutoSize = true;
      this.lblPassword.BackColor = System.Drawing.Color.Transparent;
      this.lblPassword.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.lblPassword.Location = new System.Drawing.Point(103, 111);
      this.lblPassword.Name = "lblPassword";
      this.lblPassword.Size = new System.Drawing.Size(60, 15);
      this.lblPassword.TabIndex = 5;
      this.lblPassword.Text = "Password:";
      // 
      // lblUser
      // 
      this.lblUser.AutoSize = true;
      this.lblUser.BackColor = System.Drawing.Color.Transparent;
      this.lblUser.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.lblUser.Location = new System.Drawing.Point(103, 87);
      this.lblUser.Name = "lblUser";
      this.lblUser.Size = new System.Drawing.Size(33, 15);
      this.lblUser.TabIndex = 3;
      this.lblUser.Text = "User:";
      // 
      // lblConnection
      // 
      this.lblConnection.AutoSize = true;
      this.lblConnection.BackColor = System.Drawing.Color.Transparent;
      this.lblConnection.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.lblConnection.Location = new System.Drawing.Point(103, 64);
      this.lblConnection.Name = "lblConnection";
      this.lblConnection.Size = new System.Drawing.Size(72, 15);
      this.lblConnection.TabIndex = 1;
      this.lblConnection.Text = "Connection:";
      // 
      // lblEnterPassword
      // 
      this.lblEnterPassword.AutoSize = true;
      this.lblEnterPassword.Font = new System.Drawing.Font("Segoe UI", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.lblEnterPassword.ForeColor = System.Drawing.Color.Navy;
      this.lblEnterPassword.Location = new System.Drawing.Point(90, 27);
      this.lblEnterPassword.Name = "lblEnterPassword";
      this.lblEnterPassword.Size = new System.Drawing.Size(309, 20);
      this.lblEnterPassword.TabIndex = 0;
      this.lblEnterPassword.Text = "Please enter the password for the connection:";
      // 
      // picLogo
      // 
      this.picLogo.Image = global::MySQL.ForExcel.Properties.Resources.MySQLforExcel_Security;
      this.picLogo.Location = new System.Drawing.Point(20, 20);
      this.picLogo.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
      this.picLogo.Name = "picLogo";
      this.picLogo.Size = new System.Drawing.Size(64, 64);
      this.picLogo.TabIndex = 22;
      this.picLogo.TabStop = false;
      // 
      // PasswordDialog
      // 
      this.AcceptButton = this.btnOK;
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
      this.CancelButton = this.btnCancel;
      this.ClientSize = new System.Drawing.Size(514, 192);
      this.CommandAreaHeight = 45;
      this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
      this.MainInstructionLocation = new System.Drawing.Point(19, 19);
      this.MainInstructionLocationOffset = new System.Drawing.Size(-20, 5);
      this.Name = "PasswordDialog";
      this.Text = "Connection Password";
      this.contentAreaPanel.ResumeLayout(false);
      this.contentAreaPanel.PerformLayout();
      this.commandAreaPanel.ResumeLayout(false);
      ((System.ComponentModel.ISupportInitialize)(this.picLogo)).EndInit();
      this.ResumeLayout(false);

    }

    #endregion

    private System.Windows.Forms.Button btnOK;
    private System.Windows.Forms.Button btnCancel;
    private System.Windows.Forms.Label lblUserValue;
    private System.Windows.Forms.TextBox txtPassword;
    private System.Windows.Forms.Label lblConnectionValue;
    private System.Windows.Forms.Label lblConnection;
    private System.Windows.Forms.Label lblUser;
    private System.Windows.Forms.Label lblPassword;
    private System.Windows.Forms.Label lblEnterPassword;
    private System.Windows.Forms.PictureBox picLogo;
  }
}