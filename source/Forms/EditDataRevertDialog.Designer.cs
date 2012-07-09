namespace MySQL.ForExcel
{
  partial class EditDataRevertDialog
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
      this.lblOperationSummary = new System.Windows.Forms.Label();
      this.picLogo = new System.Windows.Forms.PictureBox();
      this.lblRevertData = new System.Windows.Forms.Label();
      this.btnCancel = new System.Windows.Forms.Button();
      this.btnRevert = new System.Windows.Forms.Button();
      this.btnRefreshData = new System.Windows.Forms.Button();
      this.contentAreaPanel.SuspendLayout();
      this.commandAreaPanel.SuspendLayout();
      ((System.ComponentModel.ISupportInitialize)(this.picLogo)).BeginInit();
      this.SuspendLayout();
      // 
      // contentAreaPanel
      // 
      this.contentAreaPanel.BackColor = System.Drawing.SystemColors.Window;
      this.contentAreaPanel.Controls.Add(this.lblOperationSummary);
      this.contentAreaPanel.Controls.Add(this.picLogo);
      this.contentAreaPanel.Controls.Add(this.lblRevertData);
      this.contentAreaPanel.Size = new System.Drawing.Size(484, 106);
      // 
      // commandAreaPanel
      // 
      this.commandAreaPanel.Controls.Add(this.btnRefreshData);
      this.commandAreaPanel.Controls.Add(this.btnRevert);
      this.commandAreaPanel.Controls.Add(this.btnCancel);
      this.commandAreaPanel.Location = new System.Drawing.Point(0, 106);
      this.commandAreaPanel.Size = new System.Drawing.Size(484, 45);
      // 
      // lblOperationSummary
      // 
      this.lblOperationSummary.AutoSize = true;
      this.lblOperationSummary.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.lblOperationSummary.Location = new System.Drawing.Point(92, 56);
      this.lblOperationSummary.Name = "lblOperationSummary";
      this.lblOperationSummary.Size = new System.Drawing.Size(341, 30);
      this.lblOperationSummary.TabIndex = 26;
      this.lblOperationSummary.Text = "Reverting changes or refreshing data from the DB will cause\r\nyour changes to be l" +
    "ost. Click on the buttons below to proceed.";
      // 
      // picLogo
      // 
      this.picLogo.Image = global::MySQL.ForExcel.Properties.Resources.MySQLforExcel_Logo_64x64;
      this.picLogo.Location = new System.Drawing.Point(21, 22);
      this.picLogo.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
      this.picLogo.Name = "picLogo";
      this.picLogo.Size = new System.Drawing.Size(64, 64);
      this.picLogo.TabIndex = 27;
      this.picLogo.TabStop = false;
      // 
      // lblRevertData
      // 
      this.lblRevertData.AutoSize = true;
      this.lblRevertData.Font = new System.Drawing.Font("Segoe UI", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.lblRevertData.ForeColor = System.Drawing.Color.Navy;
      this.lblRevertData.Location = new System.Drawing.Point(91, 29);
      this.lblRevertData.Name = "lblRevertData";
      this.lblRevertData.Size = new System.Drawing.Size(87, 20);
      this.lblRevertData.TabIndex = 25;
      this.lblRevertData.Text = "Revert Data";
      // 
      // btnCancel
      // 
      this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
      this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
      this.btnCancel.Location = new System.Drawing.Point(397, 11);
      this.btnCancel.Name = "btnCancel";
      this.btnCancel.Size = new System.Drawing.Size(75, 23);
      this.btnCancel.TabIndex = 0;
      this.btnCancel.Text = "Cancel";
      this.btnCancel.UseVisualStyleBackColor = true;
      // 
      // btnRevert
      // 
      this.btnRevert.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
      this.btnRevert.DialogResult = System.Windows.Forms.DialogResult.OK;
      this.btnRevert.Location = new System.Drawing.Point(235, 11);
      this.btnRevert.Name = "btnRevert";
      this.btnRevert.Size = new System.Drawing.Size(156, 23);
      this.btnRevert.TabIndex = 1;
      this.btnRevert.Text = "Revert to Original Values";
      this.btnRevert.UseVisualStyleBackColor = true;
      this.btnRevert.Click += new System.EventHandler(this.btnRevert_Click);
      // 
      // btnRefreshData
      // 
      this.btnRefreshData.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
      this.btnRefreshData.DialogResult = System.Windows.Forms.DialogResult.OK;
      this.btnRefreshData.Location = new System.Drawing.Point(85, 11);
      this.btnRefreshData.Name = "btnRefreshData";
      this.btnRefreshData.Size = new System.Drawing.Size(144, 23);
      this.btnRefreshData.TabIndex = 2;
      this.btnRefreshData.Text = "Refresh Data from DB";
      this.btnRefreshData.UseVisualStyleBackColor = true;
      this.btnRefreshData.Click += new System.EventHandler(this.btnRefreshData_Click);
      // 
      // EditDataRevertDialog
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.CancelButton = this.btnCancel;
      this.ClientSize = new System.Drawing.Size(484, 152);
      this.CommandAreaHeight = 45;
      this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Sizable;
      this.MaximumSize = new System.Drawing.Size(500, 190);
      this.MinimumSize = new System.Drawing.Size(500, 190);
      this.Name = "EditDataRevertDialog";
      this.Text = "MySQL for Excel";
      this.contentAreaPanel.ResumeLayout(false);
      this.contentAreaPanel.PerformLayout();
      this.commandAreaPanel.ResumeLayout(false);
      ((System.ComponentModel.ISupportInitialize)(this.picLogo)).EndInit();
      this.ResumeLayout(false);

    }

    #endregion

    private System.Windows.Forms.Label lblOperationSummary;
    private System.Windows.Forms.PictureBox picLogo;
    private System.Windows.Forms.Label lblRevertData;
    private System.Windows.Forms.Button btnCancel;
    private System.Windows.Forms.Button btnRefreshData;
    private System.Windows.Forms.Button btnRevert;
  }
}