namespace MySQL.ForExcel
{
  partial class EditChangesDialog
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
      this.grdChanges = new System.Windows.Forms.DataGridView();
      this.btnClose = new System.Windows.Forms.Button();
      ((System.ComponentModel.ISupportInitialize)(this.grdChanges)).BeginInit();
      this.SuspendLayout();
      // 
      // grdChanges
      // 
      this.grdChanges.AllowUserToAddRows = false;
      this.grdChanges.AllowUserToDeleteRows = false;
      this.grdChanges.AllowUserToResizeColumns = false;
      this.grdChanges.AllowUserToResizeRows = false;
      this.grdChanges.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
      this.grdChanges.Location = new System.Drawing.Point(12, 12);
      this.grdChanges.MultiSelect = false;
      this.grdChanges.Name = "grdChanges";
      this.grdChanges.ReadOnly = true;
      this.grdChanges.RowHeadersVisible = false;
      this.grdChanges.Size = new System.Drawing.Size(665, 248);
      this.grdChanges.TabIndex = 0;
      // 
      // btnClose
      // 
      this.btnClose.DialogResult = System.Windows.Forms.DialogResult.Cancel;
      this.btnClose.Location = new System.Drawing.Point(602, 266);
      this.btnClose.Name = "btnClose";
      this.btnClose.Size = new System.Drawing.Size(75, 23);
      this.btnClose.TabIndex = 1;
      this.btnClose.Text = "Close";
      this.btnClose.UseVisualStyleBackColor = true;
      // 
      // EditChangesDialog
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.CancelButton = this.btnClose;
      this.ClientSize = new System.Drawing.Size(689, 293);
      this.ControlBox = false;
      this.Controls.Add(this.btnClose);
      this.Controls.Add(this.grdChanges);
      this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
      this.Name = "EditChangesDialog";
      this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
      this.Text = "EditChangesDialog";
      ((System.ComponentModel.ISupportInitialize)(this.grdChanges)).EndInit();
      this.ResumeLayout(false);

    }

    #endregion

    private System.Windows.Forms.DataGridView grdChanges;
    private System.Windows.Forms.Button btnClose;
  }
}