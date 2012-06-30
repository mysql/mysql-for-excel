namespace MySQL.ForExcel
{
  partial class AutoStyleableBaseDialog
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
      this.contentAreaPanel = new System.Windows.Forms.Panel();
      this.commandAreaPanel = new System.Windows.Forms.Panel();
      this.footNoteAreaPanel = new System.Windows.Forms.Panel();
      this.SuspendLayout();
      // 
      // contentAreaPanel
      // 
      this.contentAreaPanel.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
      this.contentAreaPanel.Location = new System.Drawing.Point(0, 0);
      this.contentAreaPanel.Name = "contentAreaPanel";
      this.contentAreaPanel.Size = new System.Drawing.Size(634, 170);
      this.contentAreaPanel.TabIndex = 0;
      this.contentAreaPanel.Paint += new System.Windows.Forms.PaintEventHandler(this.contentAreaPanel_Paint);
      // 
      // commandAreaPanel
      // 
      this.commandAreaPanel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
      this.commandAreaPanel.Location = new System.Drawing.Point(0, 171);
      this.commandAreaPanel.Name = "commandAreaPanel";
      this.commandAreaPanel.Size = new System.Drawing.Size(634, 40);
      this.commandAreaPanel.TabIndex = 1;
      this.commandAreaPanel.Paint += new System.Windows.Forms.PaintEventHandler(this.commandAreaPanel_Paint);
      // 
      // footNoteAreaPanel
      // 
      this.footNoteAreaPanel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
      this.footNoteAreaPanel.Location = new System.Drawing.Point(0, 202);
      this.footNoteAreaPanel.Name = "footNoteAreaPanel";
      this.footNoteAreaPanel.Size = new System.Drawing.Size(634, 0);
      this.footNoteAreaPanel.TabIndex = 2;
      this.footNoteAreaPanel.Visible = false;
      this.footNoteAreaPanel.Paint += new System.Windows.Forms.PaintEventHandler(this.footNoteAreaPanel_Paint);
      // 
      // AutoStyleableBaseDialog
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.ClientSize = new System.Drawing.Size(634, 212);
      this.Controls.Add(this.footNoteAreaPanel);
      this.Controls.Add(this.commandAreaPanel);
      this.Controls.Add(this.contentAreaPanel);
      this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
      this.MaximizeBox = false;
      this.MinimizeBox = false;
      this.Name = "AutoStyleableBaseDialog";
      this.ShowIcon = false;
      this.ShowInTaskbar = false;
      this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
      this.Text = "AutoStyleableDialog";
      this.ResumeLayout(false);

    }

    #endregion

    private System.Windows.Forms.Panel footNoteAreaPanel;
    protected System.Windows.Forms.Panel contentAreaPanel;
    protected System.Windows.Forms.Panel commandAreaPanel;
  }
}