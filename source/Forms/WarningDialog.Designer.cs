namespace MySQL.ForExcel
{
  partial class WarningDialog
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
      this.btnYes = new System.Windows.Forms.Button();
      this.picLogo = new System.Windows.Forms.PictureBox();
      this.lblWarningTitle = new System.Windows.Forms.Label();
      this.lblWarningText = new System.Windows.Forms.Label();
      this.btnNo = new System.Windows.Forms.Button();
      this.contentAreaPanel.SuspendLayout();
      this.commandAreaPanel.SuspendLayout();
      ((System.ComponentModel.ISupportInitialize)(this.picLogo)).BeginInit();
      this.SuspendLayout();
      // 
      // contentAreaPanel
      // 
      this.contentAreaPanel.BackColor = System.Drawing.SystemColors.Window;
      this.contentAreaPanel.Controls.Add(this.lblWarningText);
      this.contentAreaPanel.Controls.Add(this.picLogo);
      this.contentAreaPanel.Controls.Add(this.lblWarningTitle);
      this.contentAreaPanel.Size = new System.Drawing.Size(484, 108);
      // 
      // commandAreaPanel
      // 
      this.commandAreaPanel.Controls.Add(this.btnNo);
      this.commandAreaPanel.Controls.Add(this.btnYes);
      this.commandAreaPanel.Location = new System.Drawing.Point(0, 108);
      this.commandAreaPanel.Size = new System.Drawing.Size(484, 45);
      // 
      // btnYes
      // 
      this.btnYes.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
      this.btnYes.Location = new System.Drawing.Point(316, 11);
      this.btnYes.Name = "btnYes";
      this.btnYes.Size = new System.Drawing.Size(75, 23);
      this.btnYes.TabIndex = 1;
      this.btnYes.Text = "Yes";
      this.btnYes.UseVisualStyleBackColor = true;
      // 
      // picLogo
      // 
      this.picLogo.Image = global::MySQL.ForExcel.Properties.Resources.MySQLforExcel_InfoDlg_Warning_64x64;
      this.picLogo.Location = new System.Drawing.Point(21, 22);
      this.picLogo.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
      this.picLogo.Name = "picLogo";
      this.picLogo.Size = new System.Drawing.Size(64, 64);
      this.picLogo.TabIndex = 24;
      this.picLogo.TabStop = false;
      // 
      // lblWarningTitle
      // 
      this.lblWarningTitle.AutoSize = true;
      this.lblWarningTitle.Font = new System.Drawing.Font("Segoe UI", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.lblWarningTitle.ForeColor = System.Drawing.Color.Navy;
      this.lblWarningTitle.Location = new System.Drawing.Point(91, 22);
      this.lblWarningTitle.Name = "lblWarningTitle";
      this.lblWarningTitle.Size = new System.Drawing.Size(98, 20);
      this.lblWarningTitle.TabIndex = 0;
      this.lblWarningTitle.Text = "Warning Title";
      // 
      // lblWarningText
      // 
      this.lblWarningText.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.lblWarningText.Location = new System.Drawing.Point(92, 50);
      this.lblWarningText.Name = "lblWarningText";
      this.lblWarningText.Size = new System.Drawing.Size(363, 36);
      this.lblWarningText.TabIndex = 1;
      this.lblWarningText.Text = "Warning Details Text";
      // 
      // btnNo
      // 
      this.btnNo.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
      this.btnNo.Location = new System.Drawing.Point(397, 11);
      this.btnNo.Name = "btnNo";
      this.btnNo.Size = new System.Drawing.Size(75, 23);
      this.btnNo.TabIndex = 2;
      this.btnNo.Text = "No";
      this.btnNo.UseVisualStyleBackColor = true;
      // 
      // WarningDialog
      // 
      this.AcceptButton = this.btnYes;
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
      this.CancelButton = this.btnNo;
      this.ClientSize = new System.Drawing.Size(484, 154);
      this.CommandAreaHeight = 45;
      this.Name = "WarningDialog";
      this.Text = "MySQL for Excel";
      this.contentAreaPanel.ResumeLayout(false);
      this.contentAreaPanel.PerformLayout();
      this.commandAreaPanel.ResumeLayout(false);
      ((System.ComponentModel.ISupportInitialize)(this.picLogo)).EndInit();
      this.ResumeLayout(false);

    }

    #endregion

    private System.Windows.Forms.Button btnYes;
    private System.Windows.Forms.Label lblWarningText;
    private System.Windows.Forms.PictureBox picLogo;
    private System.Windows.Forms.Label lblWarningTitle;
    private System.Windows.Forms.Button btnNo;
  }
}