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
      this.btnOK = new System.Windows.Forms.Button();
      this.btnCancel = new System.Windows.Forms.Button();
      this.txtSchemaName = new System.Windows.Forms.TextBox();
      this.picLogo = new System.Windows.Forms.PictureBox();
      this.lblNewSchemaName = new System.Windows.Forms.Label();
      this.lblInstructions = new System.Windows.Forms.Label();
      this.lblSchemaName = new System.Windows.Forms.Label();
      this.contentAreaPanel.SuspendLayout();
      this.commandAreaPanel.SuspendLayout();
      ((System.ComponentModel.ISupportInitialize)(this.picLogo)).BeginInit();
      this.SuspendLayout();
      // 
      // contentAreaPanel
      // 
      this.contentAreaPanel.BackColor = System.Drawing.SystemColors.Window;
      this.contentAreaPanel.Controls.Add(this.lblSchemaName);
      this.contentAreaPanel.Controls.Add(this.lblInstructions);
      this.contentAreaPanel.Controls.Add(this.lblNewSchemaName);
      this.contentAreaPanel.Controls.Add(this.picLogo);
      this.contentAreaPanel.Controls.Add(this.txtSchemaName);
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
      // txtSchemaName
      // 
      this.txtSchemaName.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.txtSchemaName.Location = new System.Drawing.Point(183, 95);
      this.txtSchemaName.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
      this.txtSchemaName.Name = "txtSchemaName";
      this.txtSchemaName.Size = new System.Drawing.Size(319, 21);
      this.txtSchemaName.TabIndex = 3;
      this.txtSchemaName.TextChanged += new System.EventHandler(this.txtSchemaName_TextChanged);
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
      // lblNewSchemaName
      // 
      this.lblNewSchemaName.AutoSize = true;
      this.lblNewSchemaName.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.lblNewSchemaName.ForeColor = System.Drawing.Color.Navy;
      this.lblNewSchemaName.Location = new System.Drawing.Point(84, 23);
      this.lblNewSchemaName.Name = "lblNewSchemaName";
      this.lblNewSchemaName.Size = new System.Drawing.Size(145, 18);
      this.lblNewSchemaName.TabIndex = 0;
      this.lblNewSchemaName.Text = "New Schema Name:";
      // 
      // lblInstructions
      // 
      this.lblInstructions.AutoSize = true;
      this.lblInstructions.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.lblInstructions.Location = new System.Drawing.Point(84, 63);
      this.lblInstructions.Name = "lblInstructions";
      this.lblInstructions.Size = new System.Drawing.Size(234, 15);
      this.lblInstructions.TabIndex = 1;
      this.lblInstructions.Text = "Please enter a name for the new schema.";
      // 
      // lblSchemaName
      // 
      this.lblSchemaName.AutoSize = true;
      this.lblSchemaName.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.lblSchemaName.Location = new System.Drawing.Point(84, 98);
      this.lblSchemaName.Name = "lblSchemaName";
      this.lblSchemaName.Size = new System.Drawing.Size(93, 15);
      this.lblSchemaName.TabIndex = 2;
      this.lblSchemaName.Text = "Schema Name:";
      // 
      // NewSchemaDialog
      // 
      this.AcceptButton = this.btnOK;
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.CancelButton = this.btnCancel;
      this.ClientSize = new System.Drawing.Size(514, 182);
      this.CommandAreaHeight = 45;
      this.MainInstructionLocation = new System.Drawing.Point(13, 13);
      this.MainInstructionLocationOffset = new System.Drawing.Size(-10, 10);
      this.Name = "NewSchemaDialog";
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
    private System.Windows.Forms.TextBox txtSchemaName;
    private System.Windows.Forms.Label lblNewSchemaName;
    private System.Windows.Forms.PictureBox picLogo;
    private System.Windows.Forms.Label lblSchemaName;
    private System.Windows.Forms.Label lblInstructions;
  }
}