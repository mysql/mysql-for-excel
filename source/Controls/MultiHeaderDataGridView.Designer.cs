namespace MySQL.ExcelAddIn
{
  partial class MultiHeaderDataGridView
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
      this.grdView = new System.Windows.Forms.DataGridView();
      ((System.ComponentModel.ISupportInitialize)(this.grdView)).BeginInit();
      this.SuspendLayout();
      // 
      // grdView
      // 
      this.grdView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
      this.grdView.Dock = System.Windows.Forms.DockStyle.Fill;
      this.grdView.Location = new System.Drawing.Point(0, 0);
      this.grdView.Name = "grdView";
      this.grdView.Size = new System.Drawing.Size(150, 150);
      this.grdView.TabIndex = 0;
      this.grdView.ColumnWidthChanged += new System.Windows.Forms.DataGridViewColumnEventHandler(this.grdView_ColumnWidthChanged);
      this.grdView.DataBindingComplete += new System.Windows.Forms.DataGridViewBindingCompleteEventHandler(this.grdView_DataBindingComplete);
      this.grdView.Scroll += new System.Windows.Forms.ScrollEventHandler(this.grdView_Scroll);
      this.grdView.Paint += new System.Windows.Forms.PaintEventHandler(this.grdView_Paint);
      // 
      // MultiHeaderDataGridView
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.Controls.Add(this.grdView);
      this.Name = "MultiHeaderDataGridView";
      ((System.ComponentModel.ISupportInitialize)(this.grdView)).EndInit();
      this.ResumeLayout(false);

    }

    #endregion

    private System.Windows.Forms.DataGridView grdView;
  }
}
