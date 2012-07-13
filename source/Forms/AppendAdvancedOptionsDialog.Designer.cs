namespace MySQL.ForExcel
{
  partial class AppendAdvancedOptionsDialog
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
      this.btnAccept = new System.Windows.Forms.Button();
      this.btnCancel = new System.Windows.Forms.Button();
      this.chkUseFormattedValues = new System.Windows.Forms.CheckBox();
      this.lblFieldDataOptions = new System.Windows.Forms.Label();
      this.chkReloadColumnMapping = new System.Windows.Forms.CheckBox();
      this.chkAutoStoreColumnMapping = new System.Windows.Forms.CheckBox();
      this.chkDoNotPerformAutoMap = new System.Windows.Forms.CheckBox();
      this.lblMappingOptions = new System.Windows.Forms.Label();
      this.lblAdvancedExportOptions = new System.Windows.Forms.Label();
      this.label1 = new System.Windows.Forms.Label();
      this.btnRenameMapping = new System.Windows.Forms.Button();
      this.btnDelete = new System.Windows.Forms.Button();
      this.lstMappings = new System.Windows.Forms.ListView();
      this.columnHeader1 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
      this.columnHeader2 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
      this.contentAreaPanel.SuspendLayout();
      this.commandAreaPanel.SuspendLayout();
      this.SuspendLayout();
      // 
      // contentAreaPanel
      // 
      this.contentAreaPanel.BackColor = System.Drawing.SystemColors.Window;
      this.contentAreaPanel.Controls.Add(this.lstMappings);
      this.contentAreaPanel.Controls.Add(this.btnDelete);
      this.contentAreaPanel.Controls.Add(this.btnRenameMapping);
      this.contentAreaPanel.Controls.Add(this.label1);
      this.contentAreaPanel.Controls.Add(this.lblAdvancedExportOptions);
      this.contentAreaPanel.Controls.Add(this.chkUseFormattedValues);
      this.contentAreaPanel.Controls.Add(this.lblFieldDataOptions);
      this.contentAreaPanel.Controls.Add(this.chkReloadColumnMapping);
      this.contentAreaPanel.Controls.Add(this.chkAutoStoreColumnMapping);
      this.contentAreaPanel.Controls.Add(this.chkDoNotPerformAutoMap);
      this.contentAreaPanel.Controls.Add(this.lblMappingOptions);
      this.contentAreaPanel.Size = new System.Drawing.Size(474, 435);
      // 
      // commandAreaPanel
      // 
      this.commandAreaPanel.Controls.Add(this.btnAccept);
      this.commandAreaPanel.Controls.Add(this.btnCancel);
      this.commandAreaPanel.Location = new System.Drawing.Point(0, 435);
      this.commandAreaPanel.Size = new System.Drawing.Size(474, 45);
      // 
      // btnAccept
      // 
      this.btnAccept.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
      this.btnAccept.DialogResult = System.Windows.Forms.DialogResult.OK;
      this.btnAccept.Location = new System.Drawing.Point(306, 11);
      this.btnAccept.Name = "btnAccept";
      this.btnAccept.Size = new System.Drawing.Size(75, 23);
      this.btnAccept.TabIndex = 0;
      this.btnAccept.Text = "Accept";
      this.btnAccept.UseVisualStyleBackColor = true;
      this.btnAccept.Click += new System.EventHandler(this.btnAccept_Click);
      // 
      // btnCancel
      // 
      this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
      this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
      this.btnCancel.Location = new System.Drawing.Point(387, 11);
      this.btnCancel.Name = "btnCancel";
      this.btnCancel.Size = new System.Drawing.Size(75, 23);
      this.btnCancel.TabIndex = 1;
      this.btnCancel.Text = "Cancel";
      this.btnCancel.UseVisualStyleBackColor = true;
      // 
      // chkUseFormattedValues
      // 
      this.chkUseFormattedValues.AutoSize = true;
      this.chkUseFormattedValues.BackColor = System.Drawing.Color.Transparent;
      this.chkUseFormattedValues.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.chkUseFormattedValues.Location = new System.Drawing.Point(53, 211);
      this.chkUseFormattedValues.Name = "chkUseFormattedValues";
      this.chkUseFormattedValues.Size = new System.Drawing.Size(141, 19);
      this.chkUseFormattedValues.TabIndex = 6;
      this.chkUseFormattedValues.Text = "Use formatted values";
      this.chkUseFormattedValues.UseVisualStyleBackColor = false;
      // 
      // lblFieldDataOptions
      // 
      this.lblFieldDataOptions.AutoSize = true;
      this.lblFieldDataOptions.BackColor = System.Drawing.Color.Transparent;
      this.lblFieldDataOptions.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.lblFieldDataOptions.ForeColor = System.Drawing.SystemColors.ControlText;
      this.lblFieldDataOptions.Location = new System.Drawing.Point(24, 182);
      this.lblFieldDataOptions.Name = "lblFieldDataOptions";
      this.lblFieldDataOptions.Size = new System.Drawing.Size(116, 17);
      this.lblFieldDataOptions.TabIndex = 5;
      this.lblFieldDataOptions.Text = "Field Data Options";
      // 
      // chkReloadColumnMapping
      // 
      this.chkReloadColumnMapping.AutoSize = true;
      this.chkReloadColumnMapping.BackColor = System.Drawing.Color.Transparent;
      this.chkReloadColumnMapping.Enabled = false;
      this.chkReloadColumnMapping.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.chkReloadColumnMapping.Location = new System.Drawing.Point(53, 131);
      this.chkReloadColumnMapping.Name = "chkReloadColumnMapping";
      this.chkReloadColumnMapping.Size = new System.Drawing.Size(390, 19);
      this.chkReloadColumnMapping.TabIndex = 3;
      this.chkReloadColumnMapping.Text = "Reload stored column mapping for the selected table automatically";
      this.chkReloadColumnMapping.UseVisualStyleBackColor = false;
      // 
      // chkAutoStoreColumnMapping
      // 
      this.chkAutoStoreColumnMapping.AutoSize = true;
      this.chkAutoStoreColumnMapping.BackColor = System.Drawing.Color.Transparent;
      this.chkAutoStoreColumnMapping.Enabled = false;
      this.chkAutoStoreColumnMapping.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.chkAutoStoreColumnMapping.Location = new System.Drawing.Point(53, 108);
      this.chkAutoStoreColumnMapping.Name = "chkAutoStoreColumnMapping";
      this.chkAutoStoreColumnMapping.Size = new System.Drawing.Size(343, 19);
      this.chkAutoStoreColumnMapping.TabIndex = 2;
      this.chkAutoStoreColumnMapping.Text = "Automatically store the column mapping for the given table";
      this.chkAutoStoreColumnMapping.UseVisualStyleBackColor = false;
      // 
      // chkDoNotPerformAutoMap
      // 
      this.chkDoNotPerformAutoMap.AutoSize = true;
      this.chkDoNotPerformAutoMap.BackColor = System.Drawing.Color.Transparent;
      this.chkDoNotPerformAutoMap.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.chkDoNotPerformAutoMap.Location = new System.Drawing.Point(53, 85);
      this.chkDoNotPerformAutoMap.Name = "chkDoNotPerformAutoMap";
      this.chkDoNotPerformAutoMap.Size = new System.Drawing.Size(303, 19);
      this.chkDoNotPerformAutoMap.TabIndex = 1;
      this.chkDoNotPerformAutoMap.Text = "Perform an automatic mapping when dialog opens";
      this.chkDoNotPerformAutoMap.UseVisualStyleBackColor = false;
      // 
      // lblMappingOptions
      // 
      this.lblMappingOptions.AutoSize = true;
      this.lblMappingOptions.BackColor = System.Drawing.Color.Transparent;
      this.lblMappingOptions.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.lblMappingOptions.ForeColor = System.Drawing.SystemColors.ControlText;
      this.lblMappingOptions.Location = new System.Drawing.Point(24, 56);
      this.lblMappingOptions.Name = "lblMappingOptions";
      this.lblMappingOptions.Size = new System.Drawing.Size(111, 17);
      this.lblMappingOptions.TabIndex = 0;
      this.lblMappingOptions.Text = "Mapping Options";
      // 
      // lblAdvancedExportOptions
      // 
      this.lblAdvancedExportOptions.AutoSize = true;
      this.lblAdvancedExportOptions.Font = new System.Drawing.Font("Segoe UI", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.lblAdvancedExportOptions.ForeColor = System.Drawing.Color.Navy;
      this.lblAdvancedExportOptions.Location = new System.Drawing.Point(17, 17);
      this.lblAdvancedExportOptions.Name = "lblAdvancedExportOptions";
      this.lblAdvancedExportOptions.Size = new System.Drawing.Size(224, 20);
      this.lblAdvancedExportOptions.TabIndex = 9;
      this.lblAdvancedExportOptions.Text = "Advanced Append Data Options";
      // 
      // label1
      // 
      this.label1.AutoSize = true;
      this.label1.BackColor = System.Drawing.Color.Transparent;
      this.label1.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.label1.ForeColor = System.Drawing.SystemColors.ControlText;
      this.label1.Location = new System.Drawing.Point(24, 249);
      this.label1.Name = "label1";
      this.label1.Size = new System.Drawing.Size(158, 17);
      this.label1.TabIndex = 10;
      this.label1.Text = "Stored Column Mappings";
      // 
      // btnRenameMapping
      // 
      this.btnRenameMapping.Enabled = false;
      this.btnRenameMapping.Location = new System.Drawing.Point(373, 290);
      this.btnRenameMapping.Name = "btnRenameMapping";
      this.btnRenameMapping.Size = new System.Drawing.Size(75, 25);
      this.btnRenameMapping.TabIndex = 12;
      this.btnRenameMapping.Text = "Rename";
      this.btnRenameMapping.UseVisualStyleBackColor = true;
      this.btnRenameMapping.Click += new System.EventHandler(this.btnRenameMapping_Click);
      // 
      // btnDelete
      // 
      this.btnDelete.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
      this.btnDelete.Enabled = false;
      this.btnDelete.Location = new System.Drawing.Point(373, 321);
      this.btnDelete.Name = "btnDelete";
      this.btnDelete.Size = new System.Drawing.Size(75, 23);
      this.btnDelete.TabIndex = 13;
      this.btnDelete.Text = "Delete";
      this.btnDelete.UseVisualStyleBackColor = true;
      this.btnDelete.Click += new System.EventHandler(this.btnDelete_Click);
      // 
      // lstMappings
      // 
      this.lstMappings.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader1,
            this.columnHeader2});
      this.lstMappings.FullRowSelect = true;
      this.lstMappings.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.None;
      this.lstMappings.Location = new System.Drawing.Point(53, 290);
      this.lstMappings.MultiSelect = false;
      this.lstMappings.Name = "lstMappings";
      this.lstMappings.Size = new System.Drawing.Size(314, 127);
      this.lstMappings.TabIndex = 43;
      this.lstMappings.UseCompatibleStateImageBehavior = false;
      this.lstMappings.View = System.Windows.Forms.View.Details;
      this.lstMappings.SelectedIndexChanged += new System.EventHandler(this.lstMappings_SelectedIndexChanged);
      // 
      // columnHeader1
      // 
      this.columnHeader1.Text = "";
      this.columnHeader1.Width = 265;
      // 
      // columnHeader2
      // 
      this.columnHeader2.Text = "";
      this.columnHeader2.Width = 0;
      // 
      // AppendAdvancedOptionsDialog
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.CancelButton = this.btnCancel;
      this.ClientSize = new System.Drawing.Size(474, 482);
      this.CommandAreaHeight = 45;
      this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Sizable;
      this.MainInstructionLocation = new System.Drawing.Point(13, 21);
      this.MaximumSize = new System.Drawing.Size(490, 520);
      this.MinimumSize = new System.Drawing.Size(490, 340);
      this.Name = "AppendAdvancedOptionsDialog";
      this.Text = "Advanced Options";
      this.contentAreaPanel.ResumeLayout(false);
      this.contentAreaPanel.PerformLayout();
      this.commandAreaPanel.ResumeLayout(false);
      this.ResumeLayout(false);

    }

    #endregion

    private System.Windows.Forms.Button btnAccept;
    private System.Windows.Forms.Button btnCancel;
    private System.Windows.Forms.CheckBox chkUseFormattedValues;
    private System.Windows.Forms.Label lblFieldDataOptions;
    private System.Windows.Forms.CheckBox chkReloadColumnMapping;
    private System.Windows.Forms.CheckBox chkAutoStoreColumnMapping;
    private System.Windows.Forms.CheckBox chkDoNotPerformAutoMap;
    private System.Windows.Forms.Label lblMappingOptions;
    private System.Windows.Forms.Label lblAdvancedExportOptions;
    private System.Windows.Forms.Button btnDelete;
    private System.Windows.Forms.Button btnRenameMapping;
    private System.Windows.Forms.Label label1;
    private System.Windows.Forms.ListView lstMappings;
    private System.Windows.Forms.ColumnHeader columnHeader1;
    private System.Windows.Forms.ColumnHeader columnHeader2;
  }
}