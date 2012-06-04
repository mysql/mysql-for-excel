namespace MySQL.ForExcel
{
  partial class EditDataForm
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
      System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
      this.btnRevert = new System.Windows.Forms.Button();
      this.btnCommit = new System.Windows.Forms.Button();
      this.chkAutoCommit = new System.Windows.Forms.CheckBox();
      this.lblRange = new System.Windows.Forms.Label();
      this.btnResizeForm = new System.Windows.Forms.Button();
      this.grdPreview = new System.Windows.Forms.DataGridView();
      this.chkRefreshFromDB = new System.Windows.Forms.CheckBox();
      ((System.ComponentModel.ISupportInitialize)(this.grdPreview)).BeginInit();
      this.SuspendLayout();
      // 
      // btnRevert
      // 
      this.btnRevert.Location = new System.Drawing.Point(15, 28);
      this.btnRevert.Name = "btnRevert";
      this.btnRevert.Size = new System.Drawing.Size(110, 23);
      this.btnRevert.TabIndex = 0;
      this.btnRevert.Text = "Revert Data";
      this.btnRevert.UseVisualStyleBackColor = true;
      this.btnRevert.Click += new System.EventHandler(this.btnRevert_Click);
      // 
      // btnCommit
      // 
      this.btnCommit.Location = new System.Drawing.Point(15, 57);
      this.btnCommit.Name = "btnCommit";
      this.btnCommit.Size = new System.Drawing.Size(110, 23);
      this.btnCommit.TabIndex = 1;
      this.btnCommit.Text = "Commit Changes";
      this.btnCommit.UseVisualStyleBackColor = true;
      this.btnCommit.Click += new System.EventHandler(this.btnCommit_Click);
      // 
      // chkAutoCommit
      // 
      this.chkAutoCommit.AutoSize = true;
      this.chkAutoCommit.Location = new System.Drawing.Point(131, 61);
      this.chkAutoCommit.Name = "chkAutoCommit";
      this.chkAutoCommit.Size = new System.Drawing.Size(130, 17);
      this.chkAutoCommit.TabIndex = 2;
      this.chkAutoCommit.Text = "Auto-Commit Changes";
      this.chkAutoCommit.UseVisualStyleBackColor = true;
      this.chkAutoCommit.CheckedChanged += new System.EventHandler(this.chkAutoCommit_CheckedChanged);
      // 
      // lblRange
      // 
      this.lblRange.AutoSize = true;
      this.lblRange.Location = new System.Drawing.Point(12, 9);
      this.lblRange.Name = "lblRange";
      this.lblRange.Size = new System.Drawing.Size(92, 13);
      this.lblRange.TabIndex = 3;
      this.lblRange.Text = "Editing Range: ??";
      // 
      // btnResizeForm
      // 
      this.btnResizeForm.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
      this.btnResizeForm.FlatAppearance.BorderSize = 0;
      this.btnResizeForm.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
      this.btnResizeForm.Location = new System.Drawing.Point(252, 80);
      this.btnResizeForm.Name = "btnResizeForm";
      this.btnResizeForm.Size = new System.Drawing.Size(20, 23);
      this.btnResizeForm.TabIndex = 4;
      this.btnResizeForm.Text = "˅";
      this.btnResizeForm.UseVisualStyleBackColor = true;
      this.btnResizeForm.Click += new System.EventHandler(this.btnResizeForm_Click);
      // 
      // grdPreview
      // 
      this.grdPreview.AllowUserToAddRows = false;
      this.grdPreview.AllowUserToDeleteRows = false;
      this.grdPreview.AllowUserToOrderColumns = true;
      this.grdPreview.AllowUserToResizeColumns = false;
      this.grdPreview.AllowUserToResizeRows = false;
      this.grdPreview.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
      this.grdPreview.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.AllCells;
      this.grdPreview.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
      dataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
      dataGridViewCellStyle4.BackColor = System.Drawing.SystemColors.Window;
      dataGridViewCellStyle4.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      dataGridViewCellStyle4.ForeColor = System.Drawing.SystemColors.ControlText;
      dataGridViewCellStyle4.SelectionBackColor = System.Drawing.Color.Transparent;
      dataGridViewCellStyle4.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
      dataGridViewCellStyle4.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
      this.grdPreview.DefaultCellStyle = dataGridViewCellStyle4;
      this.grdPreview.Location = new System.Drawing.Point(12, 86);
      this.grdPreview.MultiSelect = false;
      this.grdPreview.Name = "grdPreview";
      this.grdPreview.ReadOnly = true;
      this.grdPreview.RowHeadersVisible = false;
      this.grdPreview.Size = new System.Drawing.Size(500, 0);
      this.grdPreview.TabIndex = 5;
      this.grdPreview.DataBindingComplete += new System.Windows.Forms.DataGridViewBindingCompleteEventHandler(this.grdPreview_DataBindingComplete);
      // 
      // chkRefreshFromDB
      // 
      this.chkRefreshFromDB.AutoSize = true;
      this.chkRefreshFromDB.Checked = true;
      this.chkRefreshFromDB.CheckState = System.Windows.Forms.CheckState.Checked;
      this.chkRefreshFromDB.Location = new System.Drawing.Point(131, 32);
      this.chkRefreshFromDB.Name = "chkRefreshFromDB";
      this.chkRefreshFromDB.Size = new System.Drawing.Size(104, 17);
      this.chkRefreshFromDB.TabIndex = 6;
      this.chkRefreshFromDB.Text = "Refresh from DB";
      this.chkRefreshFromDB.UseVisualStyleBackColor = true;
      // 
      // EditDataForm
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.ClientSize = new System.Drawing.Size(524, 106);
      this.Controls.Add(this.chkRefreshFromDB);
      this.Controls.Add(this.grdPreview);
      this.Controls.Add(this.btnResizeForm);
      this.Controls.Add(this.lblRange);
      this.Controls.Add(this.chkAutoCommit);
      this.Controls.Add(this.btnCommit);
      this.Controls.Add(this.btnRevert);
      this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
      this.MaximizeBox = false;
      this.MinimizeBox = false;
      this.Name = "EditDataForm";
      this.Opacity = 0.5D;
      this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
      this.Text = "Edit Data";
      this.TopMost = true;
      this.Activated += new System.EventHandler(this.EditDataForm_Activated);
      this.Deactivate += new System.EventHandler(this.EditDataForm_Deactivate);
      this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.EditDataForm_FormClosed);
      ((System.ComponentModel.ISupportInitialize)(this.grdPreview)).EndInit();
      this.ResumeLayout(false);
      this.PerformLayout();

    }

    #endregion

    private System.Windows.Forms.Button btnRevert;
    private System.Windows.Forms.Button btnCommit;
    private System.Windows.Forms.CheckBox chkAutoCommit;
    private System.Windows.Forms.Label lblRange;
    private System.Windows.Forms.Button btnResizeForm;
    private System.Windows.Forms.DataGridView grdPreview;
    private System.Windows.Forms.CheckBox chkRefreshFromDB;
  }
}