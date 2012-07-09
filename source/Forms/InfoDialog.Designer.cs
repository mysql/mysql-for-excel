namespace MySQL.ForExcel
{
  partial class InfoDialog
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
      this.btnShowDetails = new System.Windows.Forms.Button();
      this.picLogo = new System.Windows.Forms.PictureBox();
      this.lblOperationStatus = new System.Windows.Forms.Label();
      this.lblOperationSummary = new System.Windows.Forms.Label();
      this.lblPressButton = new System.Windows.Forms.Label();
      this.txtDetails = new System.Windows.Forms.TextBox();
      this.contentAreaPanel.SuspendLayout();
      this.commandAreaPanel.SuspendLayout();
      ((System.ComponentModel.ISupportInitialize)(this.picLogo)).BeginInit();
      this.SuspendLayout();
      // 
      // contentAreaPanel
      // 
      this.contentAreaPanel.BackColor = System.Drawing.SystemColors.Window;
      this.contentAreaPanel.Controls.Add(this.txtDetails);
      this.contentAreaPanel.Controls.Add(this.lblPressButton);
      this.contentAreaPanel.Controls.Add(this.lblOperationSummary);
      this.contentAreaPanel.Controls.Add(this.picLogo);
      this.contentAreaPanel.Controls.Add(this.lblOperationStatus);
      this.contentAreaPanel.Size = new System.Drawing.Size(564, 266);
      // 
      // commandAreaPanel
      // 
      this.commandAreaPanel.Controls.Add(this.btnShowDetails);
      this.commandAreaPanel.Controls.Add(this.btnOK);
      this.commandAreaPanel.Location = new System.Drawing.Point(0, 266);
      this.commandAreaPanel.Size = new System.Drawing.Size(564, 45);
      // 
      // btnOK
      // 
      this.btnOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
      this.btnOK.Location = new System.Drawing.Point(477, 11);
      this.btnOK.Name = "btnOK";
      this.btnOK.Size = new System.Drawing.Size(75, 23);
      this.btnOK.TabIndex = 1;
      this.btnOK.Text = "OK";
      this.btnOK.UseVisualStyleBackColor = true;
      this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
      // 
      // btnShowDetails
      // 
      this.btnShowDetails.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
      this.btnShowDetails.Location = new System.Drawing.Point(12, 11);
      this.btnShowDetails.Name = "btnShowDetails";
      this.btnShowDetails.Size = new System.Drawing.Size(109, 23);
      this.btnShowDetails.TabIndex = 0;
      this.btnShowDetails.Text = "Show Details";
      this.btnShowDetails.UseVisualStyleBackColor = true;
      this.btnShowDetails.Click += new System.EventHandler(this.btnShowDetails_Click);
      // 
      // picLogo
      // 
      this.picLogo.Location = new System.Drawing.Point(21, 22);
      this.picLogo.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
      this.picLogo.Name = "picLogo";
      this.picLogo.Size = new System.Drawing.Size(64, 64);
      this.picLogo.TabIndex = 24;
      this.picLogo.TabStop = false;
      // 
      // lblOperationStatus
      // 
      this.lblOperationStatus.AutoSize = true;
      this.lblOperationStatus.Font = new System.Drawing.Font("Segoe UI", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.lblOperationStatus.ForeColor = System.Drawing.Color.Navy;
      this.lblOperationStatus.Location = new System.Drawing.Point(91, 29);
      this.lblOperationStatus.Name = "lblOperationStatus";
      this.lblOperationStatus.Size = new System.Drawing.Size(236, 20);
      this.lblOperationStatus.TabIndex = 0;
      this.lblOperationStatus.Text = "Operation Completed Successfully";
      // 
      // lblOperationSummary
      // 
      this.lblOperationSummary.AutoSize = true;
      this.lblOperationSummary.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.lblOperationSummary.Location = new System.Drawing.Point(92, 56);
      this.lblOperationSummary.Name = "lblOperationSummary";
      this.lblOperationSummary.Size = new System.Drawing.Size(145, 15);
      this.lblOperationSummary.TabIndex = 1;
      this.lblOperationSummary.Text = "Operation was performed.";
      // 
      // lblPressButton
      // 
      this.lblPressButton.AutoSize = true;
      this.lblPressButton.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.lblPressButton.Location = new System.Drawing.Point(92, 73);
      this.lblPressButton.Name = "lblPressButton";
      this.lblPressButton.Size = new System.Drawing.Size(120, 15);
      this.lblPressButton.TabIndex = 2;
      this.lblPressButton.Text = "Press OK to continue.";
      // 
      // txtDetails
      // 
      this.txtDetails.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
      this.txtDetails.BackColor = System.Drawing.SystemColors.Window;
      this.txtDetails.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.txtDetails.Location = new System.Drawing.Point(95, 102);
      this.txtDetails.Multiline = true;
      this.txtDetails.Name = "txtDetails";
      this.txtDetails.ReadOnly = true;
      this.txtDetails.ScrollBars = System.Windows.Forms.ScrollBars.Both;
      this.txtDetails.Size = new System.Drawing.Size(373, 140);
      this.txtDetails.TabIndex = 3;
      this.txtDetails.WordWrap = false;
      // 
      // InfoDialog
      // 
      this.AcceptButton = this.btnOK;
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.ClientSize = new System.Drawing.Size(564, 312);
      this.CommandAreaHeight = 45;
      this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Sizable;
      this.MinimumSize = new System.Drawing.Size(580, 350);
      this.Name = "InfoDialog";
      this.Text = "MySQL for Excel";
      this.contentAreaPanel.ResumeLayout(false);
      this.contentAreaPanel.PerformLayout();
      this.commandAreaPanel.ResumeLayout(false);
      ((System.ComponentModel.ISupportInitialize)(this.picLogo)).EndInit();
      this.ResumeLayout(false);

    }

    #endregion

    private System.Windows.Forms.Button btnOK;
    private System.Windows.Forms.Button btnShowDetails;
    private System.Windows.Forms.Label lblOperationSummary;
    private System.Windows.Forms.PictureBox picLogo;
    private System.Windows.Forms.Label lblOperationStatus;
    private System.Windows.Forms.Label lblPressButton;
    private System.Windows.Forms.TextBox txtDetails;
  }
}