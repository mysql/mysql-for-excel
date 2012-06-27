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
      this.btnCancel = new System.Windows.Forms.Button();
      this.btnOK = new System.Windows.Forms.Button();
      this.ConnectionPanel = new System.Windows.Forms.Panel();
      this.lblUserValue = new System.Windows.Forms.Label();
      this.lblConnectionValue = new System.Windows.Forms.Label();
      this.txtPassword = new System.Windows.Forms.TextBox();
      this.lblPassword = new System.Windows.Forms.Label();
      this.lblUser = new System.Windows.Forms.Label();
      this.lblConnection = new System.Windows.Forms.Label();
      this.lblEnterPassword = new System.Windows.Forms.Label();
      this.picLogo = new System.Windows.Forms.PictureBox();
      this.ConnectionPanel.SuspendLayout();
      ((System.ComponentModel.ISupportInitialize)(this.picLogo)).BeginInit();
      this.SuspendLayout();
      // 
      // btnCancel
      // 
      this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
      this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
      this.btnCancel.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.btnCancel.Location = new System.Drawing.Point(457, 156);
      this.btnCancel.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
      this.btnCancel.Name = "btnCancel";
      this.btnCancel.Size = new System.Drawing.Size(75, 23);
      this.btnCancel.TabIndex = 8;
      this.btnCancel.Text = "Cancel";
      this.btnCancel.UseVisualStyleBackColor = true;
      // 
      // btnOK
      // 
      this.btnOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
      this.btnOK.DialogResult = System.Windows.Forms.DialogResult.OK;
      this.btnOK.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.btnOK.Location = new System.Drawing.Point(376, 156);
      this.btnOK.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
      this.btnOK.Name = "btnOK";
      this.btnOK.Size = new System.Drawing.Size(75, 23);
      this.btnOK.TabIndex = 7;
      this.btnOK.Text = "OK";
      this.btnOK.UseVisualStyleBackColor = true;
      // 
      // ConnectionPanel
      // 
      this.ConnectionPanel.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
      this.ConnectionPanel.BackColor = System.Drawing.SystemColors.Window;
      this.ConnectionPanel.Controls.Add(this.lblUserValue);
      this.ConnectionPanel.Controls.Add(this.lblConnectionValue);
      this.ConnectionPanel.Controls.Add(this.txtPassword);
      this.ConnectionPanel.Controls.Add(this.lblPassword);
      this.ConnectionPanel.Controls.Add(this.lblUser);
      this.ConnectionPanel.Controls.Add(this.lblConnection);
      this.ConnectionPanel.Controls.Add(this.lblEnterPassword);
      this.ConnectionPanel.Controls.Add(this.picLogo);
      this.ConnectionPanel.Location = new System.Drawing.Point(0, 0);
      this.ConnectionPanel.Name = "ConnectionPanel";
      this.ConnectionPanel.Size = new System.Drawing.Size(547, 149);
      this.ConnectionPanel.TabIndex = 9;
      // 
      // lblUserValue
      // 
      this.lblUserValue.AutoSize = true;
      this.lblUserValue.Location = new System.Drawing.Point(215, 74);
      this.lblUserValue.Name = "lblUserValue";
      this.lblUserValue.Size = new System.Drawing.Size(20, 17);
      this.lblUserValue.TabIndex = 12;
      this.lblUserValue.Text = "??";
      // 
      // lblConnectionValue
      // 
      this.lblConnectionValue.AutoSize = true;
      this.lblConnectionValue.Location = new System.Drawing.Point(215, 45);
      this.lblConnectionValue.Name = "lblConnectionValue";
      this.lblConnectionValue.Size = new System.Drawing.Size(20, 17);
      this.lblConnectionValue.TabIndex = 10;
      this.lblConnectionValue.Text = "??";
      // 
      // txtPassword
      // 
      this.txtPassword.Location = new System.Drawing.Point(218, 101);
      this.txtPassword.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
      this.txtPassword.Name = "txtPassword";
      this.txtPassword.Size = new System.Drawing.Size(315, 25);
      this.txtPassword.TabIndex = 14;
      this.txtPassword.UseSystemPasswordChar = true;
      // 
      // lblPassword
      // 
      this.lblPassword.AutoSize = true;
      this.lblPassword.Location = new System.Drawing.Point(140, 101);
      this.lblPassword.Name = "lblPassword";
      this.lblPassword.Size = new System.Drawing.Size(67, 17);
      this.lblPassword.TabIndex = 13;
      this.lblPassword.Text = "Password:";
      // 
      // lblUser
      // 
      this.lblUser.AutoSize = true;
      this.lblUser.Location = new System.Drawing.Point(170, 74);
      this.lblUser.Name = "lblUser";
      this.lblUser.Size = new System.Drawing.Size(38, 17);
      this.lblUser.TabIndex = 11;
      this.lblUser.Text = "User:";
      // 
      // lblConnection
      // 
      this.lblConnection.AutoSize = true;
      this.lblConnection.Location = new System.Drawing.Point(132, 45);
      this.lblConnection.Name = "lblConnection";
      this.lblConnection.Size = new System.Drawing.Size(76, 17);
      this.lblConnection.TabIndex = 9;
      this.lblConnection.Text = "Connection:";
      // 
      // lblEnterPassword
      // 
      this.lblEnterPassword.AutoSize = true;
      this.lblEnterPassword.Location = new System.Drawing.Point(105, 19);
      this.lblEnterPassword.Name = "lblEnterPassword";
      this.lblEnterPassword.Size = new System.Drawing.Size(275, 17);
      this.lblEnterPassword.TabIndex = 7;
      this.lblEnterPassword.Text = "Please enter the password for the connection:";
      // 
      // picLogo
      // 
      this.picLogo.Image = global::MySQL.ForExcel.Properties.Resources.MySQLforExcel_Logo_64x64;
      this.picLogo.Location = new System.Drawing.Point(19, 19);
      this.picLogo.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
      this.picLogo.Name = "picLogo";
      this.picLogo.Size = new System.Drawing.Size(52, 82);
      this.picLogo.TabIndex = 8;
      this.picLogo.TabStop = false;
      // 
      // PasswordDialog
      // 
      this.AcceptButton = this.btnOK;
      this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 17F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.CancelButton = this.btnCancel;
      this.ClientSize = new System.Drawing.Size(544, 187);
      this.ControlBox = false;
      this.Controls.Add(this.ConnectionPanel);
      this.Controls.Add(this.btnOK);
      this.Controls.Add(this.btnCancel);
      this.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
      this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
      this.Name = "PasswordDialog";
      this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
      this.Text = "Connection Password";
      this.ConnectionPanel.ResumeLayout(false);
      this.ConnectionPanel.PerformLayout();
      ((System.ComponentModel.ISupportInitialize)(this.picLogo)).EndInit();
      this.ResumeLayout(false);

    }

    #endregion

    private System.Windows.Forms.Button btnCancel;
    private System.Windows.Forms.Button btnOK;
    private System.Windows.Forms.Panel ConnectionPanel;
    private System.Windows.Forms.Label lblUserValue;
    private System.Windows.Forms.Label lblConnectionValue;
    private System.Windows.Forms.TextBox txtPassword;
    private System.Windows.Forms.Label lblPassword;
    private System.Windows.Forms.Label lblUser;
    private System.Windows.Forms.Label lblConnection;
    private System.Windows.Forms.Label lblEnterPassword;
    private System.Windows.Forms.PictureBox picLogo;
  }
}