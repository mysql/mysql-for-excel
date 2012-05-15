namespace MySQL.ExcelAddIn
{
  partial class TaskPaneControl
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
      this.welcomePanel1 = new MySQL.ExcelAddIn.WelcomePanel();
      this.dbObjectSelectionPanel1 = new MySQL.ExcelAddIn.DBObjectSelectionPanel();
      this.schemaSelectionPanel1 = new MySQL.ExcelAddIn.SchemaSelectionPanel();
      this.SuspendLayout();
      // 
      // welcomePanel1
      // 
      this.welcomePanel1.Dock = System.Windows.Forms.DockStyle.Fill;
      this.welcomePanel1.Location = new System.Drawing.Point(0, 0);
      this.welcomePanel1.Name = "welcomePanel1";
      this.welcomePanel1.Size = new System.Drawing.Size(284, 651);
      this.welcomePanel1.TabIndex = 0;
      // 
      // dbObjectSelectionPanel1
      // 
      this.dbObjectSelectionPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
      this.dbObjectSelectionPanel1.Location = new System.Drawing.Point(0, 0);
      this.dbObjectSelectionPanel1.Name = "dbObjectSelectionPanel1";
      this.dbObjectSelectionPanel1.Size = new System.Drawing.Size(284, 651);
      this.dbObjectSelectionPanel1.TabIndex = 2;
      // 
      // schemaSelectionPanel1
      // 
      this.schemaSelectionPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
      this.schemaSelectionPanel1.Location = new System.Drawing.Point(0, 0);
      this.schemaSelectionPanel1.Name = "schemaSelectionPanel1";
      this.schemaSelectionPanel1.Size = new System.Drawing.Size(284, 651);
      this.schemaSelectionPanel1.TabIndex = 1;
      // 
      // TaskPaneControl
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.Controls.Add(this.welcomePanel1);
      this.Controls.Add(this.dbObjectSelectionPanel1);
      this.Controls.Add(this.schemaSelectionPanel1);
      this.Name = "TaskPaneControl";
      this.Size = new System.Drawing.Size(284, 651);
      this.ResumeLayout(false);

    }

    #endregion

    private WelcomePanel welcomePanel1;
    private SchemaSelectionPanel schemaSelectionPanel1;
    private DBObjectSelectionPanel dbObjectSelectionPanel1;

  }
}
