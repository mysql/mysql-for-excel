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
      System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
      this.EditDataPanel = new System.Windows.Forms.Panel();
      this.chkAutoCommit = new System.Windows.Forms.CheckBox();
      this.chkRefreshFromDB = new System.Windows.Forms.CheckBox();
      this.grdPreview = new System.Windows.Forms.DataGridView();
      this.btnCommit = new System.Windows.Forms.Button();
      this.btnRevert = new System.Windows.Forms.Button();
      this.EditDataPanel.SuspendLayout();
      ((System.ComponentModel.ISupportInitialize)(this.grdPreview)).BeginInit();
      this.SuspendLayout();
      // 
      // EditDataPanel
      // 
      this.EditDataPanel.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
      this.EditDataPanel.BackColor = System.Drawing.SystemColors.Window;
      this.EditDataPanel.Controls.Add(this.chkAutoCommit);
      this.EditDataPanel.Controls.Add(this.chkRefreshFromDB);
      this.EditDataPanel.Controls.Add(this.grdPreview);
      this.EditDataPanel.Location = new System.Drawing.Point(0, 0);
      this.EditDataPanel.Name = "EditDataPanel";
      this.EditDataPanel.Size = new System.Drawing.Size(527, 302);
      this.EditDataPanel.TabIndex = 7;
      // 
      // chkAutoCommit
      // 
      this.chkAutoCommit.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
      this.chkAutoCommit.AutoSize = true;
      this.chkAutoCommit.Location = new System.Drawing.Point(370, 282);
      this.chkAutoCommit.Name = "chkAutoCommit";
      this.chkAutoCommit.Size = new System.Drawing.Size(142, 17);
      this.chkAutoCommit.TabIndex = 9;
      this.chkAutoCommit.Text = "Auto-Commit Changes";
      this.chkAutoCommit.UseVisualStyleBackColor = true;
      this.chkAutoCommit.CheckedChanged += new System.EventHandler(this.chkAutoCommit_CheckedChanged);
      // 
      // chkRefreshFromDB
      // 
      this.chkRefreshFromDB.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
      this.chkRefreshFromDB.AutoSize = true;
      this.chkRefreshFromDB.Checked = true;
      this.chkRefreshFromDB.CheckState = System.Windows.Forms.CheckState.Checked;
      this.chkRefreshFromDB.Location = new System.Drawing.Point(12, 282);
      this.chkRefreshFromDB.Name = "chkRefreshFromDB";
      this.chkRefreshFromDB.Size = new System.Drawing.Size(110, 17);
      this.chkRefreshFromDB.TabIndex = 11;
      this.chkRefreshFromDB.Text = "Refresh from DB";
      this.chkRefreshFromDB.UseVisualStyleBackColor = true;
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
      dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
      dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Window;
      dataGridViewCellStyle1.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.ControlText;
      dataGridViewCellStyle1.SelectionBackColor = System.Drawing.Color.Transparent;
      dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
      dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
      this.grdPreview.DefaultCellStyle = dataGridViewCellStyle1;
      this.grdPreview.Location = new System.Drawing.Point(12, 12);
      this.grdPreview.MultiSelect = false;
      this.grdPreview.Name = "grdPreview";
      this.grdPreview.ReadOnly = true;
      this.grdPreview.RowHeadersVisible = false;
      this.grdPreview.Size = new System.Drawing.Size(500, 253);
      this.grdPreview.TabIndex = 10;
      this.grdPreview.DataBindingComplete += new System.Windows.Forms.DataGridViewBindingCompleteEventHandler(this.grdPreview_DataBindingComplete);
      // 
      // btnCommit
      // 
      this.btnCommit.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
      this.btnCommit.Location = new System.Drawing.Point(370, 313);
      this.btnCommit.Name = "btnCommit";
      this.btnCommit.Size = new System.Drawing.Size(143, 23);
      this.btnCommit.TabIndex = 8;
      this.btnCommit.Text = "Commit Changes";
      this.btnCommit.UseVisualStyleBackColor = true;
      this.btnCommit.Click += new System.EventHandler(this.btnCommit_Click);
      // 
      // btnRevert
      // 
      this.btnRevert.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
      this.btnRevert.Location = new System.Drawing.Point(12, 313);
      this.btnRevert.Name = "btnRevert";
      this.btnRevert.Size = new System.Drawing.Size(110, 23);
      this.btnRevert.TabIndex = 7;
      this.btnRevert.Text = "Revert Data";
      this.btnRevert.UseVisualStyleBackColor = true;
      this.btnRevert.Click += new System.EventHandler(this.btnRevert_Click);
      // 
      // EditDataForm
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.ClientSize = new System.Drawing.Size(524, 348);
      this.Controls.Add(this.EditDataPanel);
      this.Controls.Add(this.btnCommit);
      this.Controls.Add(this.btnRevert);
      this.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.MaximizeBox = false;
      this.MinimizeBox = false;
      this.MinimumSize = new System.Drawing.Size(300, 300);
      this.Name = "EditDataForm";
      this.Opacity = 0.5D;
      this.ShowIcon = false;
      this.ShowInTaskbar = false;
      this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
      this.Text = "Edit Data - SheetName [Range]";
      this.TopMost = true;
      this.Activated += new System.EventHandler(this.EditDataForm_Activated);
      this.Deactivate += new System.EventHandler(this.EditDataForm_Deactivate);
      this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.EditDataForm_FormClosed);
      this.EditDataPanel.ResumeLayout(false);
      this.EditDataPanel.PerformLayout();
      ((System.ComponentModel.ISupportInitialize)(this.grdPreview)).EndInit();
      this.ResumeLayout(false);

    }

    #endregion

    private System.Windows.Forms.Panel EditDataPanel;
    private System.Windows.Forms.CheckBox chkAutoCommit;
    private System.Windows.Forms.CheckBox chkRefreshFromDB;
    private System.Windows.Forms.DataGridView grdPreview;
    private System.Windows.Forms.Button btnCommit;
    private System.Windows.Forms.Button btnRevert;

  }
}