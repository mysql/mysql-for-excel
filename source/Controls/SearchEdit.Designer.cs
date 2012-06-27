namespace MySQL.ForExcel
{
  partial class SearchEdit
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
      this.innerText = new System.Windows.Forms.TextBox();
      this.SuspendLayout();
      // 
      // innerText
      // 
      this.innerText.Anchor = System.Windows.Forms.AnchorStyles.None;
      this.innerText.BorderStyle = System.Windows.Forms.BorderStyle.None;
      this.innerText.Font = new System.Drawing.Font("Arial", 9F);
      this.innerText.Location = new System.Drawing.Point(38, 0);
      this.innerText.Name = "innerText";
      this.innerText.Size = new System.Drawing.Size(311, 14);
      this.innerText.TabIndex = 1;
      this.innerText.Enter += new System.EventHandler(this.innerText_Enter);
      this.innerText.KeyDown += new System.Windows.Forms.KeyEventHandler(this.innerText_KeyDown);
      this.innerText.Leave += new System.EventHandler(this.innerText_Leave);
      // 
      // SearchEdit
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.BackColor = System.Drawing.SystemColors.Window;
      this.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
      this.Controls.Add(this.innerText);
      this.Name = "SearchEdit";
      this.Size = new System.Drawing.Size(349, 15);
      this.Resize += new System.EventHandler(this.SearchEdit_Resize);
      this.ResumeLayout(false);
      this.PerformLayout();

    }

    #endregion

    private System.Windows.Forms.TextBox innerText;

  }
}
