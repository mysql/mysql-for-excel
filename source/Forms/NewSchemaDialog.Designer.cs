namespace MySQL.ForExcel
{
  partial class NewSchemaDialog
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
      this.NewSchemaPanel = new System.Windows.Forms.Panel();
      this.schemaName = new System.Windows.Forms.TextBox();
      this.lblEnterPassword = new System.Windows.Forms.Label();
      this.picLogo = new System.Windows.Forms.PictureBox();
      this.NewSchemaPanel.SuspendLayout();
      ((System.ComponentModel.ISupportInitialize)(this.picLogo)).BeginInit();
      this.SuspendLayout();
      // 
      // btnCancel
      // 
      this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
      this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
      this.btnCancel.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.btnCancel.Location = new System.Drawing.Point(457, 119);
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
      this.btnOK.Location = new System.Drawing.Point(376, 119);
      this.btnOK.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
      this.btnOK.Name = "btnOK";
      this.btnOK.Size = new System.Drawing.Size(75, 23);
      this.btnOK.TabIndex = 7;
      this.btnOK.Text = "OK";
      this.btnOK.UseVisualStyleBackColor = true;
      this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
      // 
      // NewSchemaPanel
      // 
      this.NewSchemaPanel.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
      this.NewSchemaPanel.BackColor = System.Drawing.SystemColors.Window;
      this.NewSchemaPanel.Controls.Add(this.schemaName);
      this.NewSchemaPanel.Controls.Add(this.lblEnterPassword);
      this.NewSchemaPanel.Controls.Add(this.picLogo);
      this.NewSchemaPanel.Location = new System.Drawing.Point(0, 0);
      this.NewSchemaPanel.Name = "NewSchemaPanel";
      this.NewSchemaPanel.Size = new System.Drawing.Size(547, 106);
      this.NewSchemaPanel.TabIndex = 9;
      // 
      // schemaName
      // 
      this.schemaName.Location = new System.Drawing.Point(108, 46);
      this.schemaName.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
      this.schemaName.Name = "schemaName";
      this.schemaName.Size = new System.Drawing.Size(404, 25);
      this.schemaName.TabIndex = 9;
      // 
      // lblEnterPassword
      // 
      this.lblEnterPassword.AutoSize = true;
      this.lblEnterPassword.Location = new System.Drawing.Point(104, 25);
      this.lblEnterPassword.Name = "lblEnterPassword";
      this.lblEnterPassword.Size = new System.Drawing.Size(125, 17);
      this.lblEnterPassword.TabIndex = 8;
      this.lblEnterPassword.Text = "New Schema Name:";
      // 
      // picLogo
      // 
      this.picLogo.Image = global::MySQL.ForExcel.Properties.Resources.mysql_header_img;
      this.picLogo.Location = new System.Drawing.Point(12, 13);
      this.picLogo.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
      this.picLogo.Name = "picLogo";
      this.picLogo.Size = new System.Drawing.Size(71, 77);
      this.picLogo.TabIndex = 7;
      this.picLogo.TabStop = false;
      // 
      // NewSchemaDialog
      // 
      this.AcceptButton = this.btnOK;
      this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 17F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.CancelButton = this.btnCancel;
      this.ClientSize = new System.Drawing.Size(544, 155);
      this.ControlBox = false;
      this.Controls.Add(this.NewSchemaPanel);
      this.Controls.Add(this.btnOK);
      this.Controls.Add(this.btnCancel);
      this.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
      this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
      this.Name = "NewSchemaDialog";
      this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
      this.Text = "Create New Schema";
      this.NewSchemaPanel.ResumeLayout(false);
      this.NewSchemaPanel.PerformLayout();
      ((System.ComponentModel.ISupportInitialize)(this.picLogo)).EndInit();
      this.ResumeLayout(false);

    }

    #endregion

    private System.Windows.Forms.Button btnCancel;
    private System.Windows.Forms.Button btnOK;
    private System.Windows.Forms.Panel NewSchemaPanel;
    private System.Windows.Forms.TextBox schemaName;
    private System.Windows.Forms.Label lblEnterPassword;
    private System.Windows.Forms.PictureBox picLogo;
  }
}