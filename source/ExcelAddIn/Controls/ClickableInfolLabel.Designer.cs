namespace MySQL.ExcelAddIn.Controls
{
  partial class ClickableInfolLabel
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

    #region Component Designer generated code

    /// <summary> 
    /// Required method for Designer support - do not modify 
    /// the contents of this method with the code editor.
    /// </summary>
    private void InitializeComponent()
    {
      this.lblText = new System.Windows.Forms.Label();
      this.lblInfo1 = new System.Windows.Forms.Label();
      this.lblInfo2 = new System.Windows.Forms.Label();
      this.btnImage = new System.Windows.Forms.Button();
      this.SuspendLayout();
      // 
      // lblText
      // 
      this.lblText.AutoSize = true;
      this.lblText.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.lblText.Location = new System.Drawing.Point(57, 3);
      this.lblText.Name = "lblText";
      this.lblText.Size = new System.Drawing.Size(35, 16);
      this.lblText.TabIndex = 1;
      this.lblText.Text = "Text";
      // 
      // lblInfo1
      // 
      this.lblInfo1.AutoSize = true;
      this.lblInfo1.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.lblInfo1.ForeColor = System.Drawing.SystemColors.ControlDarkDark;
      this.lblInfo1.Location = new System.Drawing.Point(57, 24);
      this.lblInfo1.Name = "lblInfo1";
      this.lblInfo1.Size = new System.Drawing.Size(114, 14);
      this.lblInfo1.TabIndex = 2;
      this.lblInfo1.Text = "Contextual Information";
      // 
      // lblInfo2
      // 
      this.lblInfo2.AutoSize = true;
      this.lblInfo2.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.lblInfo2.ForeColor = System.Drawing.SystemColors.ControlDarkDark;
      this.lblInfo2.Location = new System.Drawing.Point(57, 38);
      this.lblInfo2.Name = "lblInfo2";
      this.lblInfo2.Size = new System.Drawing.Size(141, 14);
      this.lblInfo2.TabIndex = 3;
      this.lblInfo2.Text = "More Contextual Information";
      // 
      // btnImage
      // 
      this.btnImage.FlatAppearance.BorderSize = 0;
      this.btnImage.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
      this.btnImage.Location = new System.Drawing.Point(3, 3);
      this.btnImage.Name = "btnImage";
      this.btnImage.Size = new System.Drawing.Size(48, 48);
      this.btnImage.TabIndex = 4;
      this.btnImage.UseVisualStyleBackColor = false;
      // 
      // ClickableInfolLabel
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.Controls.Add(this.btnImage);
      this.Controls.Add(this.lblInfo2);
      this.Controls.Add(this.lblInfo1);
      this.Controls.Add(this.lblText);
      this.Name = "ClickableInfolLabel";
      this.Size = new System.Drawing.Size(201, 55);
      this.Load += new System.EventHandler(this.ClickableInfolLabel_Load);
      this.ResumeLayout(false);
      this.PerformLayout();

    }

    #endregion

    private System.Windows.Forms.Label lblText;
    private System.Windows.Forms.Label lblInfo1;
    private System.Windows.Forms.Label lblInfo2;
    private System.Windows.Forms.Button btnImage;
  }
}
