namespace MySQL.ForExcel
{
  partial class NewConnectionDialog
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
      this.label1 = new System.Windows.Forms.Label();
      this.connectionName = new System.Windows.Forms.TextBox();
      this.label2 = new System.Windows.Forms.Label();
      this.connectionType = new System.Windows.Forms.Label();
      this.label4 = new System.Windows.Forms.Label();
      this.connectionMethod = new System.Windows.Forms.ComboBox();
      this.tabControl1 = new System.Windows.Forms.TabControl();
      this.parametersPage = new System.Windows.Forms.TabPage();
      this.label10 = new System.Windows.Forms.Label();
      this.label9 = new System.Windows.Forms.Label();
      this.defaultSchema = new System.Windows.Forms.TextBox();
      this.label8 = new System.Windows.Forms.Label();
      this.label7 = new System.Windows.Forms.Label();
      this.port = new System.Windows.Forms.TextBox();
      this.label6 = new System.Windows.Forms.Label();
      this.userName = new System.Windows.Forms.TextBox();
      this.label5 = new System.Windows.Forms.Label();
      this.hostName = new System.Windows.Forms.TextBox();
      this.label3 = new System.Windows.Forms.Label();
      this.advancedPage = new System.Windows.Forms.TabPage();
      this.label13 = new System.Windows.Forms.Label();
      this.label12 = new System.Windows.Forms.Label();
      this.label11 = new System.Windows.Forms.Label();
      this.useSSL = new System.Windows.Forms.CheckBox();
      this.useANSI = new System.Windows.Forms.CheckBox();
      this.useCompression = new System.Windows.Forms.CheckBox();
      this.okButton = new System.Windows.Forms.Button();
      this.cancelButton = new System.Windows.Forms.Button();
      this.testButton = new System.Windows.Forms.Button();
      this.tabControl1.SuspendLayout();
      this.parametersPage.SuspendLayout();
      this.advancedPage.SuspendLayout();
      this.SuspendLayout();
      // 
      // label1
      // 
      this.label1.AutoSize = true;
      this.label1.Location = new System.Drawing.Point(18, 23);
      this.label1.Name = "label1";
      this.label1.Size = new System.Drawing.Size(110, 15);
      this.label1.TabIndex = 0;
      this.label1.Text = "Connection Name:";
      // 
      // connectionName
      // 
      this.connectionName.Location = new System.Drawing.Point(135, 20);
      this.connectionName.Name = "connectionName";
      this.connectionName.Size = new System.Drawing.Size(373, 21);
      this.connectionName.TabIndex = 1;
      // 
      // label2
      // 
      this.label2.AutoSize = true;
      this.label2.Location = new System.Drawing.Point(527, 23);
      this.label2.Name = "label2";
      this.label2.Size = new System.Drawing.Size(177, 15);
      this.label2.TabIndex = 2;
      this.label2.Text = "Type a name for the connection";
      // 
      // connectionType
      // 
      this.connectionType.AutoSize = true;
      this.connectionType.Location = new System.Drawing.Point(527, 55);
      this.connectionType.Name = "connectionType";
      this.connectionType.Size = new System.Drawing.Size(222, 15);
      this.connectionType.TabIndex = 4;
      this.connectionType.Text = "Method to use to connect to the RDBMS";
      // 
      // label4
      // 
      this.label4.AutoSize = true;
      this.label4.Location = new System.Drawing.Point(18, 55);
      this.label4.Name = "label4";
      this.label4.Size = new System.Drawing.Size(116, 15);
      this.label4.TabIndex = 3;
      this.label4.Text = "Connection Method:";
      // 
      // connectionMethod
      // 
      this.connectionMethod.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
      this.connectionMethod.FormattingEnabled = true;
      this.connectionMethod.Items.AddRange(new object[] {
            "Standard (TCP/IP)",
            "Local Socket/Pipe",
            "Standard TCP/IP over SSH"});
      this.connectionMethod.Location = new System.Drawing.Point(135, 52);
      this.connectionMethod.Name = "connectionMethod";
      this.connectionMethod.Size = new System.Drawing.Size(373, 22);
      this.connectionMethod.TabIndex = 5;
      // 
      // tabControl1
      // 
      this.tabControl1.Controls.Add(this.parametersPage);
      this.tabControl1.Controls.Add(this.advancedPage);
      this.tabControl1.Location = new System.Drawing.Point(12, 89);
      this.tabControl1.Name = "tabControl1";
      this.tabControl1.SelectedIndex = 0;
      this.tabControl1.Size = new System.Drawing.Size(758, 373);
      this.tabControl1.TabIndex = 6;
      // 
      // parametersPage
      // 
      this.parametersPage.Controls.Add(this.label10);
      this.parametersPage.Controls.Add(this.label9);
      this.parametersPage.Controls.Add(this.defaultSchema);
      this.parametersPage.Controls.Add(this.label8);
      this.parametersPage.Controls.Add(this.label7);
      this.parametersPage.Controls.Add(this.port);
      this.parametersPage.Controls.Add(this.label6);
      this.parametersPage.Controls.Add(this.userName);
      this.parametersPage.Controls.Add(this.label5);
      this.parametersPage.Controls.Add(this.hostName);
      this.parametersPage.Controls.Add(this.label3);
      this.parametersPage.Font = new System.Drawing.Font("Arial", 8.5F);
      this.parametersPage.Location = new System.Drawing.Point(4, 23);
      this.parametersPage.Name = "parametersPage";
      this.parametersPage.Padding = new System.Windows.Forms.Padding(3);
      this.parametersPage.Size = new System.Drawing.Size(750, 346);
      this.parametersPage.TabIndex = 0;
      this.parametersPage.Text = "Parameters";
      this.parametersPage.UseVisualStyleBackColor = true;
      // 
      // label10
      // 
      this.label10.AutoSize = true;
      this.label10.Location = new System.Drawing.Point(445, 82);
      this.label10.Name = "label10";
      this.label10.Size = new System.Drawing.Size(273, 15);
      this.label10.TabIndex = 17;
      this.label10.Text = "The schema that will be used as default schema";
      // 
      // label9
      // 
      this.label9.AutoSize = true;
      this.label9.Location = new System.Drawing.Point(445, 55);
      this.label9.Name = "label9";
      this.label9.Size = new System.Drawing.Size(189, 15);
      this.label9.TabIndex = 16;
      this.label9.Text = "Name of the user to connect with.";
      // 
      // defaultSchema
      // 
      this.defaultSchema.Location = new System.Drawing.Point(119, 78);
      this.defaultSchema.Name = "defaultSchema";
      this.defaultSchema.Size = new System.Drawing.Size(317, 21);
      this.defaultSchema.TabIndex = 15;
      // 
      // label8
      // 
      this.label8.AutoSize = true;
      this.label8.Location = new System.Drawing.Point(14, 82);
      this.label8.Name = "label8";
      this.label8.Size = new System.Drawing.Size(98, 15);
      this.label8.TabIndex = 14;
      this.label8.Text = "Default Schema:";
      // 
      // label7
      // 
      this.label7.AutoSize = true;
      this.label7.Location = new System.Drawing.Point(445, 22);
      this.label7.Name = "label7";
      this.label7.Size = new System.Drawing.Size(287, 15);
      this.label7.TabIndex = 13;
      this.label7.Text = "Name or IP address of the server host - TCP/IP port";
      // 
      // port
      // 
      this.port.Location = new System.Drawing.Point(312, 20);
      this.port.Name = "port";
      this.port.Size = new System.Drawing.Size(124, 21);
      this.port.TabIndex = 12;
      // 
      // label6
      // 
      this.label6.AutoSize = true;
      this.label6.Location = new System.Drawing.Point(276, 25);
      this.label6.Name = "label6";
      this.label6.Size = new System.Drawing.Size(32, 15);
      this.label6.TabIndex = 11;
      this.label6.Text = "Port:";
      // 
      // userName
      // 
      this.userName.Location = new System.Drawing.Point(119, 49);
      this.userName.Name = "userName";
      this.userName.Size = new System.Drawing.Size(317, 21);
      this.userName.TabIndex = 10;
      // 
      // label5
      // 
      this.label5.AutoSize = true;
      this.label5.Location = new System.Drawing.Point(15, 55);
      this.label5.Name = "label5";
      this.label5.Size = new System.Drawing.Size(69, 15);
      this.label5.TabIndex = 9;
      this.label5.Text = "Username:";
      // 
      // hostName
      // 
      this.hostName.Location = new System.Drawing.Point(119, 20);
      this.hostName.Name = "hostName";
      this.hostName.Size = new System.Drawing.Size(151, 21);
      this.hostName.TabIndex = 8;
      // 
      // label3
      // 
      this.label3.AutoSize = true;
      this.label3.Location = new System.Drawing.Point(14, 25);
      this.label3.Name = "label3";
      this.label3.Size = new System.Drawing.Size(68, 15);
      this.label3.TabIndex = 7;
      this.label3.Text = "Hostname:";
      // 
      // advancedPage
      // 
      this.advancedPage.Controls.Add(this.label13);
      this.advancedPage.Controls.Add(this.label12);
      this.advancedPage.Controls.Add(this.label11);
      this.advancedPage.Controls.Add(this.useSSL);
      this.advancedPage.Controls.Add(this.useANSI);
      this.advancedPage.Controls.Add(this.useCompression);
      this.advancedPage.Location = new System.Drawing.Point(4, 23);
      this.advancedPage.Name = "advancedPage";
      this.advancedPage.Padding = new System.Windows.Forms.Padding(3);
      this.advancedPage.Size = new System.Drawing.Size(750, 346);
      this.advancedPage.TabIndex = 1;
      this.advancedPage.Text = "Advanced";
      this.advancedPage.UseVisualStyleBackColor = true;
      // 
      // label13
      // 
      this.label13.AutoSize = true;
      this.label13.Location = new System.Drawing.Point(420, 91);
      this.label13.Name = "label13";
      this.label13.Size = new System.Drawing.Size(201, 15);
      this.label13.TabIndex = 19;
      this.label13.Text = "This option turns on SSL encryption";
      // 
      // label12
      // 
      this.label12.AutoSize = true;
      this.label12.Location = new System.Drawing.Point(420, 57);
      this.label12.Name = "label12";
      this.label12.Size = new System.Drawing.Size(314, 15);
      this.label12.TabIndex = 18;
      this.label12.Text = "If enabled this option overwrites the server side settings.";
      // 
      // label11
      // 
      this.label11.AutoSize = true;
      this.label11.Location = new System.Drawing.Point(420, 28);
      this.label11.Name = "label11";
      this.label11.Size = new System.Drawing.Size(218, 15);
      this.label11.TabIndex = 17;
      this.label11.Text = "Select this option for WAN connections";
      // 
      // useSSL
      // 
      this.useSSL.AutoSize = true;
      this.useSSL.Enabled = false;
      this.useSSL.Location = new System.Drawing.Point(87, 90);
      this.useSSL.Name = "useSSL";
      this.useSSL.Size = new System.Drawing.Size(136, 19);
      this.useSSL.TabIndex = 2;
      this.useSSL.Text = "Use SSL if available";
      this.useSSL.UseVisualStyleBackColor = true;
      // 
      // useANSI
      // 
      this.useANSI.AutoSize = true;
      this.useANSI.Enabled = false;
      this.useANSI.Location = new System.Drawing.Point(87, 56);
      this.useANSI.Name = "useANSI";
      this.useANSI.Size = new System.Drawing.Size(223, 19);
      this.useANSI.TabIndex = 1;
      this.useANSI.Text = "Use ANSI quotes to quote identifiers";
      this.useANSI.UseVisualStyleBackColor = true;
      // 
      // useCompression
      // 
      this.useCompression.AutoSize = true;
      this.useCompression.Location = new System.Drawing.Point(87, 24);
      this.useCompression.Name = "useCompression";
      this.useCompression.Size = new System.Drawing.Size(175, 19);
      this.useCompression.TabIndex = 0;
      this.useCompression.Text = "Use Compression protocol";
      this.useCompression.UseVisualStyleBackColor = true;
      // 
      // okButton
      // 
      this.okButton.DialogResult = System.Windows.Forms.DialogResult.OK;
      this.okButton.Location = new System.Drawing.Point(680, 474);
      this.okButton.Name = "okButton";
      this.okButton.Size = new System.Drawing.Size(90, 28);
      this.okButton.TabIndex = 7;
      this.okButton.Text = "OK";
      this.okButton.UseVisualStyleBackColor = true;
      this.okButton.Click += new System.EventHandler(this.okButton_Click);
      // 
      // cancelButton
      // 
      this.cancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
      this.cancelButton.Location = new System.Drawing.Point(584, 474);
      this.cancelButton.Name = "cancelButton";
      this.cancelButton.Size = new System.Drawing.Size(90, 28);
      this.cancelButton.TabIndex = 8;
      this.cancelButton.Text = "Cancel";
      this.cancelButton.UseVisualStyleBackColor = true;
      // 
      // testButton
      // 
      this.testButton.Location = new System.Drawing.Point(464, 473);
      this.testButton.Name = "testButton";
      this.testButton.Size = new System.Drawing.Size(114, 28);
      this.testButton.TabIndex = 9;
      this.testButton.Text = "Test Connection";
      this.testButton.UseVisualStyleBackColor = true;
      // 
      // NewConnectionDialog
      // 
      this.AcceptButton = this.okButton;
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 14F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.CancelButton = this.cancelButton;
      this.ClientSize = new System.Drawing.Size(782, 516);
      this.Controls.Add(this.testButton);
      this.Controls.Add(this.cancelButton);
      this.Controls.Add(this.okButton);
      this.Controls.Add(this.tabControl1);
      this.Controls.Add(this.connectionMethod);
      this.Controls.Add(this.connectionType);
      this.Controls.Add(this.label4);
      this.Controls.Add(this.label2);
      this.Controls.Add(this.connectionName);
      this.Controls.Add(this.label1);
      this.Font = new System.Drawing.Font("Arial", 8.5F);
      this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
      this.MaximizeBox = false;
      this.MinimizeBox = false;
      this.Name = "NewConnectionDialog";
      this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
      this.Text = "Setup New Connection";
      this.tabControl1.ResumeLayout(false);
      this.parametersPage.ResumeLayout(false);
      this.parametersPage.PerformLayout();
      this.advancedPage.ResumeLayout(false);
      this.advancedPage.PerformLayout();
      this.ResumeLayout(false);
      this.PerformLayout();

    }

    #endregion

    private System.Windows.Forms.Label label1;
    private System.Windows.Forms.TextBox connectionName;
    private System.Windows.Forms.Label label2;
    private System.Windows.Forms.Label connectionType;
    private System.Windows.Forms.Label label4;
    private System.Windows.Forms.ComboBox connectionMethod;
    private System.Windows.Forms.TabControl tabControl1;
    private System.Windows.Forms.TabPage parametersPage;
    private System.Windows.Forms.TextBox defaultSchema;
    private System.Windows.Forms.Label label8;
    private System.Windows.Forms.Label label7;
    private System.Windows.Forms.TextBox port;
    private System.Windows.Forms.Label label6;
    private System.Windows.Forms.TextBox userName;
    private System.Windows.Forms.Label label5;
    private System.Windows.Forms.TextBox hostName;
    private System.Windows.Forms.Label label3;
    private System.Windows.Forms.TabPage advancedPage;
    private System.Windows.Forms.Button okButton;
    private System.Windows.Forms.Button cancelButton;
    private System.Windows.Forms.Button testButton;
    private System.Windows.Forms.Label label10;
    private System.Windows.Forms.Label label9;
    private System.Windows.Forms.CheckBox useSSL;
    private System.Windows.Forms.CheckBox useANSI;
    private System.Windows.Forms.CheckBox useCompression;
    private System.Windows.Forms.Label label13;
    private System.Windows.Forms.Label label12;
    private System.Windows.Forms.Label label11;
  }
}