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
      this.pictureBox1 = new System.Windows.Forms.PictureBox();
      this.lblEnterPassword = new System.Windows.Forms.Label();
      this.lblConnection = new System.Windows.Forms.Label();
      this.lblUser = new System.Windows.Forms.Label();
      this.lblPassword = new System.Windows.Forms.Label();
      this.txtPassword = new System.Windows.Forms.TextBox();
      this.lblConnectionValue = new System.Windows.Forms.Label();
      this.lblUserValue = new System.Windows.Forms.Label();
      this.btnCancel = new System.Windows.Forms.Button();
      this.btnOK = new System.Windows.Forms.Button();
      ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
      this.SuspendLayout();
      // 
      // pictureBox1
      // 
      this.pictureBox1.Image = global::MySQL.ForExcel.Properties.Resources.mysql_header_img;
      this.pictureBox1.Location = new System.Drawing.Point(14, 15);
      this.pictureBox1.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
      this.pictureBox1.Name = "pictureBox1";
      this.pictureBox1.Size = new System.Drawing.Size(79, 116);
      this.pictureBox1.TabIndex = 0;
      this.pictureBox1.TabStop = false;
      // 
      // lblEnterPassword
      // 
      this.lblEnterPassword.AutoSize = true;
      this.lblEnterPassword.Location = new System.Drawing.Point(99, 15);
      this.lblEnterPassword.Name = "lblEnterPassword";
      this.lblEnterPassword.Size = new System.Drawing.Size(273, 16);
      this.lblEnterPassword.TabIndex = 0;
      this.lblEnterPassword.Text = "Please enter the password for the connection:";
      // 
      // lblConnection
      // 
      this.lblConnection.AutoSize = true;
      this.lblConnection.Location = new System.Drawing.Point(126, 40);
      this.lblConnection.Name = "lblConnection";
      this.lblConnection.Size = new System.Drawing.Size(77, 16);
      this.lblConnection.TabIndex = 1;
      this.lblConnection.Text = "Connection:";
      // 
      // lblUser
      // 
      this.lblUser.AutoSize = true;
      this.lblUser.Location = new System.Drawing.Point(164, 67);
      this.lblUser.Name = "lblUser";
      this.lblUser.Size = new System.Drawing.Size(39, 16);
      this.lblUser.TabIndex = 3;
      this.lblUser.Text = "User:";
      // 
      // lblPassword
      // 
      this.lblPassword.AutoSize = true;
      this.lblPassword.Location = new System.Drawing.Point(134, 92);
      this.lblPassword.Name = "lblPassword";
      this.lblPassword.Size = new System.Drawing.Size(69, 16);
      this.lblPassword.TabIndex = 5;
      this.lblPassword.Text = "Password:";
      // 
      // txtPassword
      // 
      this.txtPassword.Location = new System.Drawing.Point(212, 92);
      this.txtPassword.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
      this.txtPassword.Name = "txtPassword";
      this.txtPassword.Size = new System.Drawing.Size(315, 22);
      this.txtPassword.TabIndex = 6;
      this.txtPassword.UseSystemPasswordChar = true;
      // 
      // lblConnectionValue
      // 
      this.lblConnectionValue.AutoSize = true;
      this.lblConnectionValue.Location = new System.Drawing.Point(209, 40);
      this.lblConnectionValue.Name = "lblConnectionValue";
      this.lblConnectionValue.Size = new System.Drawing.Size(22, 16);
      this.lblConnectionValue.TabIndex = 2;
      this.lblConnectionValue.Text = "??";
      // 
      // lblUserValue
      // 
      this.lblUserValue.AutoSize = true;
      this.lblUserValue.Location = new System.Drawing.Point(209, 67);
      this.lblUserValue.Name = "lblUserValue";
      this.lblUserValue.Size = new System.Drawing.Size(22, 16);
      this.lblUserValue.TabIndex = 4;
      this.lblUserValue.Text = "??";
      // 
      // btnCancel
      // 
      this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
      this.btnCancel.Location = new System.Drawing.Point(440, 132);
      this.btnCancel.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
      this.btnCancel.Name = "btnCancel";
      this.btnCancel.Size = new System.Drawing.Size(87, 28);
      this.btnCancel.TabIndex = 8;
      this.btnCancel.Text = "Cancel";
      this.btnCancel.UseVisualStyleBackColor = true;
      // 
      // btnOK
      // 
      this.btnOK.DialogResult = System.Windows.Forms.DialogResult.OK;
      this.btnOK.Location = new System.Drawing.Point(346, 132);
      this.btnOK.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
      this.btnOK.Name = "btnOK";
      this.btnOK.Size = new System.Drawing.Size(87, 28);
      this.btnOK.TabIndex = 7;
      this.btnOK.Text = "OK";
      this.btnOK.UseVisualStyleBackColor = true;
      // 
      // PasswordDialog
      // 
      this.AcceptButton = this.btnOK;
      this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 16F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.CancelButton = this.btnCancel;
      this.ClientSize = new System.Drawing.Size(547, 181);
      this.ControlBox = false;
      this.Controls.Add(this.btnOK);
      this.Controls.Add(this.btnCancel);
      this.Controls.Add(this.lblUserValue);
      this.Controls.Add(this.lblConnectionValue);
      this.Controls.Add(this.txtPassword);
      this.Controls.Add(this.lblPassword);
      this.Controls.Add(this.lblUser);
      this.Controls.Add(this.lblConnection);
      this.Controls.Add(this.lblEnterPassword);
      this.Controls.Add(this.pictureBox1);
      this.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
      this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
      this.Name = "PasswordDialog";
      this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
      this.Text = "Connection Password";
      ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
      this.ResumeLayout(false);
      this.PerformLayout();

    }

    #endregion

    private System.Windows.Forms.PictureBox pictureBox1;
    private System.Windows.Forms.Label lblEnterPassword;
    private System.Windows.Forms.Label lblConnection;
    private System.Windows.Forms.Label lblUser;
    private System.Windows.Forms.Label lblPassword;
    private System.Windows.Forms.TextBox txtPassword;
    private System.Windows.Forms.Label lblConnectionValue;
    private System.Windows.Forms.Label lblUserValue;
    private System.Windows.Forms.Button btnCancel;
    private System.Windows.Forms.Button btnOK;
  }
}