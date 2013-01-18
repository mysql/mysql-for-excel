namespace MySQL.ForExcel
{
  partial class AboutBox
  {
    /// <summary>
    /// Required designer variable.
    /// </summary>
    private System.ComponentModel.IContainer components = null;

    /// <summary>
    /// Clean up any resources being used.
    /// </summary>
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
      this.version = new System.Windows.Forms.Label();
      this.installVersion = new System.Windows.Forms.Label();
      this.SuspendLayout();
      // 
      // version
      // 
      this.version.AutoSize = true;
      this.version.ForeColor = System.Drawing.SystemColors.ControlDark;
      this.version.Location = new System.Drawing.Point(478, 125);
      this.version.Name = "version";
      this.version.Size = new System.Drawing.Size(69, 13);
      this.version.TabIndex = 0;
      this.version.Text = "Version 1.1.1";
      this.version.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
      // 
      // installVersion
      // 
      this.installVersion.AutoSize = true;
      this.installVersion.ForeColor = System.Drawing.SystemColors.ControlDark;
      this.installVersion.Location = new System.Drawing.Point(448, 143);
      this.installVersion.Name = "installVersion";
      this.installVersion.Size = new System.Drawing.Size(99, 13);
      this.installVersion.TabIndex = 1;
      this.installVersion.Text = "MySQL Installer 1.1";
      this.installVersion.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
      // 
      // AboutBox
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.BackgroundImage = global::MySQL.ForExcel.Properties.Resources.SplashScreenExcel;
      this.ClientSize = new System.Drawing.Size(557, 271);
      this.Controls.Add(this.installVersion);
      this.Controls.Add(this.version);
      this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
      this.MaximizeBox = false;
      this.MinimizeBox = false;
      this.Name = "AboutBox";
      this.Padding = new System.Windows.Forms.Padding(9);
      this.ShowIcon = false;
      this.ShowInTaskbar = false;
      this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
      this.Text = "AboutBox";
      this.Click += new System.EventHandler(this.AboutBox_Click);
      this.ResumeLayout(false);
      this.PerformLayout();

    }

    #endregion

    private System.Windows.Forms.Label version;
    private System.Windows.Forms.Label installVersion;

  }
}
