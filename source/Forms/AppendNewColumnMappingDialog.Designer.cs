namespace MySQL.ForExcel
{
  partial class AppendNewColumnMappingDialog
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
      this.txtMappingName = new System.Windows.Forms.TextBox();
      this.picLogo = new System.Windows.Forms.PictureBox();
      this.lblColumnMappingName = new System.Windows.Forms.Label();
      this.lblMappingName = new System.Windows.Forms.Label();
      this.lblInstructions = new System.Windows.Forms.Label();
      this.contentAreaPanel.SuspendLayout();
      this.commandAreaPanel.SuspendLayout();
      ((System.ComponentModel.ISupportInitialize)(this.picLogo)).BeginInit();
      this.SuspendLayout();
      // 
      // contentAreaPanel
      // 
      this.contentAreaPanel.BackColor = System.Drawing.SystemColors.Window;
      this.contentAreaPanel.Controls.Add(this.lblMappingName);
      this.contentAreaPanel.Controls.Add(this.lblInstructions);
      this.contentAreaPanel.Controls.Add(this.lblColumnMappingName);
      this.contentAreaPanel.Controls.Add(this.picLogo);
      this.contentAreaPanel.Controls.Add(this.txtMappingName);
      this.contentAreaPanel.Size = new System.Drawing.Size(514, 135);
      // 
      // commandAreaPanel
      // 
      this.commandAreaPanel.Controls.Add(this.btnOK);
      this.commandAreaPanel.Controls.Add(this.btnCancel);
      this.commandAreaPanel.Location = new System.Drawing.Point(0, 136);
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
      // txtMappingName
      // 
      this.txtMappingName.Location = new System.Drawing.Point(186, 90);
      this.txtMappingName.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
      this.txtMappingName.Name = "txtMappingName";
      this.txtMappingName.Size = new System.Drawing.Size(316, 20);
      this.txtMappingName.TabIndex = 1;
      this.txtMappingName.TextChanged += new System.EventHandler(this.txtMappingName_TextChanged);
      // 
      // picLogo
      // 
      this.picLogo.Image = global::MySQL.ForExcel.Properties.Resources.MySQLforExcel_Input_64x64;
      this.picLogo.Location = new System.Drawing.Point(14, 14);
      this.picLogo.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
      this.picLogo.Name = "picLogo";
      this.picLogo.Size = new System.Drawing.Size(64, 64);
      this.picLogo.TabIndex = 11;
      this.picLogo.TabStop = false;
      // 
      // lblColumnMappingName
      // 
      this.lblColumnMappingName.AutoSize = true;
      this.lblColumnMappingName.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.lblColumnMappingName.ForeColor = System.Drawing.Color.Navy;
      this.lblColumnMappingName.Location = new System.Drawing.Point(84, 23);
      this.lblColumnMappingName.Name = "lblColumnMappingName";
      this.lblColumnMappingName.Size = new System.Drawing.Size(202, 18);
      this.lblColumnMappingName.TabIndex = 0;
      this.lblColumnMappingName.Text = "New Column Mapping Name:";
      // 
      // lblMappingName
      // 
      this.lblMappingName.AutoSize = true;
      this.lblMappingName.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.lblMappingName.Location = new System.Drawing.Point(84, 91);
      this.lblMappingName.Name = "lblMappingName";
      this.lblMappingName.Size = new System.Drawing.Size(96, 15);
      this.lblMappingName.TabIndex = 13;
      this.lblMappingName.Text = "Mapping Name:";
      // 
      // lblInstructions
      // 
      this.lblInstructions.AutoSize = true;
      this.lblInstructions.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.lblInstructions.Location = new System.Drawing.Point(84, 45);
      this.lblInstructions.Name = "lblInstructions";
      this.lblInstructions.Size = new System.Drawing.Size(283, 15);
      this.lblInstructions.TabIndex = 12;
      this.lblInstructions.Text = "Please enter a name for the new column mapping.";
      // 
      // AppendNewColumnMappingDialog
      // 
      this.AcceptButton = this.btnOK;
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.CancelButton = this.btnCancel;
      this.ClientSize = new System.Drawing.Size(514, 182);
      this.CommandAreaHeight = 45;
      this.MainInstructionLocation = new System.Drawing.Point(13, 13);
      this.MainInstructionLocationOffset = new System.Drawing.Size(-10, 10);
      this.Name = "AppendNewColumnMappingDialog";
      this.Text = "Create New Schema";
      this.contentAreaPanel.ResumeLayout(false);
      this.contentAreaPanel.PerformLayout();
      this.commandAreaPanel.ResumeLayout(false);
      ((System.ComponentModel.ISupportInitialize)(this.picLogo)).EndInit();
      this.ResumeLayout(false);

    }

    #endregion

    private System.Windows.Forms.Button btnOK;
    private System.Windows.Forms.Button btnCancel;
    private System.Windows.Forms.TextBox txtMappingName;
    private System.Windows.Forms.Label lblColumnMappingName;
    private System.Windows.Forms.PictureBox picLogo;
    private System.Windows.Forms.Label lblMappingName;
    private System.Windows.Forms.Label lblInstructions;
  }
}