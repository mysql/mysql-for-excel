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
      this.components = new System.ComponentModel.Container();
      this.tabControl1 = new System.Windows.Forms.TabControl();
      this.parametersPage = new System.Windows.Forms.TabPage();
      this.labelHelpSocket = new System.Windows.Forms.Label();
      this.labelPromptSocket = new System.Windows.Forms.Label();
      this.socketPath = new System.Windows.Forms.TextBox();
      this.bindingSource = new System.Windows.Forms.BindingSource(this.components);
      this.labelHelpSchema = new System.Windows.Forms.Label();
      this.labelHelpUserName = new System.Windows.Forms.Label();
      this.defaultSchema = new System.Windows.Forms.TextBox();
      this.port = new System.Windows.Forms.TextBox();
      this.userName = new System.Windows.Forms.TextBox();
      this.hostName = new System.Windows.Forms.TextBox();
      this.LabelPromptSchema = new System.Windows.Forms.Label();
      this.labelHelpHostName = new System.Windows.Forms.Label();
      this.labelPromptPort = new System.Windows.Forms.Label();
      this.labelPromptUserName = new System.Windows.Forms.Label();
      this.labelPromptHostName = new System.Windows.Forms.Label();
      this.advancedPage = new System.Windows.Forms.TabPage();
      this.label21 = new System.Windows.Forms.Label();
      this.label20 = new System.Windows.Forms.Label();
      this.label19 = new System.Windows.Forms.Label();
      this.textBox4 = new System.Windows.Forms.TextBox();
      this.textBox3 = new System.Windows.Forms.TextBox();
      this.textBox2 = new System.Windows.Forms.TextBox();
      this.textBox1 = new System.Windows.Forms.TextBox();
      this.label18 = new System.Windows.Forms.Label();
      this.label17 = new System.Windows.Forms.Label();
      this.label16 = new System.Windows.Forms.Label();
      this.label15 = new System.Windows.Forms.Label();
      this.label14 = new System.Windows.Forms.Label();
      this.label13 = new System.Windows.Forms.Label();
      this.label12 = new System.Windows.Forms.Label();
      this.labelCompression = new System.Windows.Forms.Label();
      this.useSSL = new System.Windows.Forms.CheckBox();
      this.useANSI = new System.Windows.Forms.CheckBox();
      this.useCompression = new System.Windows.Forms.CheckBox();
      this.connectionMethod = new System.Windows.Forms.ComboBox();
      this.labelHelpMethod = new System.Windows.Forms.Label();
      this.labelPromptMethod = new System.Windows.Forms.Label();
      this.labelHelpName = new System.Windows.Forms.Label();
      this.connectionName = new System.Windows.Forms.TextBox();
      this.labelPromptName = new System.Windows.Forms.Label();
      this.testButton = new System.Windows.Forms.Button();
      this.cancelButton = new System.Windows.Forms.Button();
      this.okButton = new System.Windows.Forms.Button();
      this.contentAreaPanel.SuspendLayout();
      this.commandAreaPanel.SuspendLayout();
      this.tabControl1.SuspendLayout();
      this.parametersPage.SuspendLayout();
      ((System.ComponentModel.ISupportInitialize)(this.bindingSource)).BeginInit();
      this.advancedPage.SuspendLayout();
      this.SuspendLayout();
      // 
      // contentAreaPanel
      // 
      this.contentAreaPanel.BackColor = System.Drawing.SystemColors.Window;
      this.contentAreaPanel.Controls.Add(this.tabControl1);
      this.contentAreaPanel.Controls.Add(this.connectionMethod);
      this.contentAreaPanel.Controls.Add(this.labelHelpMethod);
      this.contentAreaPanel.Controls.Add(this.labelPromptMethod);
      this.contentAreaPanel.Controls.Add(this.labelHelpName);
      this.contentAreaPanel.Controls.Add(this.connectionName);
      this.contentAreaPanel.Controls.Add(this.labelPromptName);
      this.contentAreaPanel.Location = new System.Drawing.Point(3, 2);
      this.contentAreaPanel.Size = new System.Drawing.Size(793, 422);
      // 
      // commandAreaPanel
      // 
      this.commandAreaPanel.Controls.Add(this.testButton);
      this.commandAreaPanel.Controls.Add(this.cancelButton);
      this.commandAreaPanel.Controls.Add(this.okButton);
      this.commandAreaPanel.Location = new System.Drawing.Point(0, 425);
      this.commandAreaPanel.Size = new System.Drawing.Size(796, 45);
      // 
      // tabControl1
      // 
      this.tabControl1.Controls.Add(this.parametersPage);
      this.tabControl1.Controls.Add(this.advancedPage);
      this.tabControl1.Location = new System.Drawing.Point(12, 69);
      this.tabControl1.Name = "tabControl1";
      this.tabControl1.SelectedIndex = 0;
      this.tabControl1.Size = new System.Drawing.Size(769, 352);
      this.tabControl1.TabIndex = 2;
      // 
      // parametersPage
      // 
      this.parametersPage.Controls.Add(this.labelHelpSocket);
      this.parametersPage.Controls.Add(this.labelPromptSocket);
      this.parametersPage.Controls.Add(this.socketPath);
      this.parametersPage.Controls.Add(this.labelHelpSchema);
      this.parametersPage.Controls.Add(this.labelHelpUserName);
      this.parametersPage.Controls.Add(this.defaultSchema);
      this.parametersPage.Controls.Add(this.port);
      this.parametersPage.Controls.Add(this.userName);
      this.parametersPage.Controls.Add(this.hostName);
      this.parametersPage.Controls.Add(this.LabelPromptSchema);
      this.parametersPage.Controls.Add(this.labelHelpHostName);
      this.parametersPage.Controls.Add(this.labelPromptPort);
      this.parametersPage.Controls.Add(this.labelPromptUserName);
      this.parametersPage.Controls.Add(this.labelPromptHostName);
      this.parametersPage.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.parametersPage.Location = new System.Drawing.Point(4, 22);
      this.parametersPage.Name = "parametersPage";
      this.parametersPage.Padding = new System.Windows.Forms.Padding(3);
      this.parametersPage.Size = new System.Drawing.Size(761, 326);
      this.parametersPage.TabIndex = 0;
      this.parametersPage.Text = "Parameters";
      this.parametersPage.UseVisualStyleBackColor = true;
      // 
      // labelHelpSocket
      // 
      this.labelHelpSocket.AutoSize = true;
      this.labelHelpSocket.Location = new System.Drawing.Point(445, 131);
      this.labelHelpSocket.Name = "labelHelpSocket";
      this.labelHelpSocket.Size = new System.Drawing.Size(303, 15);
      this.labelHelpSocket.TabIndex = 13;
      this.labelHelpSocket.Text = "Path to local socket or pipe file. Leave empty for default.";
      this.labelHelpSocket.Visible = false;
      // 
      // labelPromptSocket
      // 
      this.labelPromptSocket.AutoSize = true;
      this.labelPromptSocket.Location = new System.Drawing.Point(7, 131);
      this.labelPromptSocket.Name = "labelPromptSocket";
      this.labelPromptSocket.Size = new System.Drawing.Size(100, 15);
      this.labelPromptSocket.TabIndex = 12;
      this.labelPromptSocket.Text = "Socket/Pipe Path:";
      this.labelPromptSocket.Visible = false;
      // 
      // socketPath
      // 
      this.socketPath.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.bindingSource, "Socket", true));
      this.socketPath.Location = new System.Drawing.Point(119, 128);
      this.socketPath.MaxLength = 484;
      this.socketPath.Name = "socketPath";
      this.socketPath.Size = new System.Drawing.Size(317, 23);
      this.socketPath.TabIndex = 4;
      this.socketPath.Visible = false;
      // 
      // bindingSource
      // 
      this.bindingSource.DataSource = typeof(MySQL.Utility.MySqlWorkbenchConnection);
      // 
      // labelHelpSchema
      // 
      this.labelHelpSchema.AutoSize = true;
      this.labelHelpSchema.Location = new System.Drawing.Point(445, 88);
      this.labelHelpSchema.Name = "labelHelpSchema";
      this.labelHelpSchema.Size = new System.Drawing.Size(258, 15);
      this.labelHelpSchema.TabIndex = 10;
      this.labelHelpSchema.Text = "The schema that will be used as default schema";
      // 
      // labelHelpUserName
      // 
      this.labelHelpUserName.AutoSize = true;
      this.labelHelpUserName.Location = new System.Drawing.Point(445, 55);
      this.labelHelpUserName.Name = "labelHelpUserName";
      this.labelHelpUserName.Size = new System.Drawing.Size(187, 15);
      this.labelHelpUserName.TabIndex = 7;
      this.labelHelpUserName.Text = "Name of the user to connect with.";
      // 
      // defaultSchema
      // 
      this.defaultSchema.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.bindingSource, "Schema", true));
      this.defaultSchema.Location = new System.Drawing.Point(119, 85);
      this.defaultSchema.Name = "defaultSchema";
      this.defaultSchema.Size = new System.Drawing.Size(317, 23);
      this.defaultSchema.TabIndex = 7;
      // 
      // port
      // 
      this.port.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.bindingSource, "Port", true));
      this.port.Location = new System.Drawing.Point(312, 19);
      this.port.Name = "port";
      this.port.Size = new System.Drawing.Size(124, 23);
      this.port.TabIndex = 5;
      // 
      // userName
      // 
      this.userName.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.bindingSource, "UserName", true));
      this.userName.Location = new System.Drawing.Point(119, 52);
      this.userName.MaxLength = 679;
      this.userName.Name = "userName";
      this.userName.Size = new System.Drawing.Size(317, 23);
      this.userName.TabIndex = 6;
      // 
      // hostName
      // 
      this.hostName.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.bindingSource, "Host", true));
      this.hostName.Location = new System.Drawing.Point(119, 19);
      this.hostName.MaxLength = 484;
      this.hostName.Name = "hostName";
      this.hostName.Size = new System.Drawing.Size(151, 23);
      this.hostName.TabIndex = 3;
      // 
      // LabelPromptSchema
      // 
      this.LabelPromptSchema.AutoSize = true;
      this.LabelPromptSchema.Location = new System.Drawing.Point(14, 89);
      this.LabelPromptSchema.Name = "LabelPromptSchema";
      this.LabelPromptSchema.Size = new System.Drawing.Size(93, 15);
      this.LabelPromptSchema.TabIndex = 8;
      this.LabelPromptSchema.Text = "Default Schema:";
      // 
      // labelHelpHostName
      // 
      this.labelHelpHostName.AutoSize = true;
      this.labelHelpHostName.Location = new System.Drawing.Point(445, 22);
      this.labelHelpHostName.Name = "labelHelpHostName";
      this.labelHelpHostName.Size = new System.Drawing.Size(276, 15);
      this.labelHelpHostName.TabIndex = 4;
      this.labelHelpHostName.Text = "Name or IP address of the server host - TCP/IP port";
      // 
      // labelPromptPort
      // 
      this.labelPromptPort.AutoSize = true;
      this.labelPromptPort.Location = new System.Drawing.Point(276, 23);
      this.labelPromptPort.Name = "labelPromptPort";
      this.labelPromptPort.Size = new System.Drawing.Size(32, 15);
      this.labelPromptPort.TabIndex = 2;
      this.labelPromptPort.Text = "Port:";
      // 
      // labelPromptUserName
      // 
      this.labelPromptUserName.AutoSize = true;
      this.labelPromptUserName.Location = new System.Drawing.Point(44, 58);
      this.labelPromptUserName.Name = "labelPromptUserName";
      this.labelPromptUserName.Size = new System.Drawing.Size(63, 15);
      this.labelPromptUserName.TabIndex = 5;
      this.labelPromptUserName.Text = "Username:";
      // 
      // labelPromptHostName
      // 
      this.labelPromptHostName.AutoSize = true;
      this.labelPromptHostName.Location = new System.Drawing.Point(42, 22);
      this.labelPromptHostName.Name = "labelPromptHostName";
      this.labelPromptHostName.Size = new System.Drawing.Size(65, 15);
      this.labelPromptHostName.TabIndex = 0;
      this.labelPromptHostName.Text = "Hostname:";
      // 
      // advancedPage
      // 
      this.advancedPage.Controls.Add(this.label21);
      this.advancedPage.Controls.Add(this.label20);
      this.advancedPage.Controls.Add(this.label19);
      this.advancedPage.Controls.Add(this.textBox4);
      this.advancedPage.Controls.Add(this.textBox3);
      this.advancedPage.Controls.Add(this.textBox2);
      this.advancedPage.Controls.Add(this.textBox1);
      this.advancedPage.Controls.Add(this.label18);
      this.advancedPage.Controls.Add(this.label17);
      this.advancedPage.Controls.Add(this.label16);
      this.advancedPage.Controls.Add(this.label15);
      this.advancedPage.Controls.Add(this.label14);
      this.advancedPage.Controls.Add(this.label13);
      this.advancedPage.Controls.Add(this.label12);
      this.advancedPage.Controls.Add(this.labelCompression);
      this.advancedPage.Controls.Add(this.useSSL);
      this.advancedPage.Controls.Add(this.useANSI);
      this.advancedPage.Controls.Add(this.useCompression);
      this.advancedPage.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.advancedPage.Location = new System.Drawing.Point(4, 22);
      this.advancedPage.Name = "advancedPage";
      this.advancedPage.Padding = new System.Windows.Forms.Padding(3);
      this.advancedPage.Size = new System.Drawing.Size(761, 326);
      this.advancedPage.TabIndex = 1;
      this.advancedPage.Text = "Advanced";
      this.advancedPage.UseVisualStyleBackColor = true;
      // 
      // label21
      // 
      this.label21.AutoSize = true;
      this.label21.Location = new System.Drawing.Point(420, 258);
      this.label21.Name = "label21";
      this.label21.Size = new System.Drawing.Size(323, 15);
      this.label21.TabIndex = 17;
      this.label21.Text = "Optional list of permissible ciphers to use for SSL encryption";
      // 
      // label20
      // 
      this.label20.AutoSize = true;
      this.label20.Location = new System.Drawing.Point(420, 216);
      this.label20.Name = "label20";
      this.label20.Size = new System.Drawing.Size(109, 15);
      this.label20.TabIndex = 16;
      this.label20.Text = "Path to Key for SSL.";
      // 
      // label19
      // 
      this.label19.AutoSize = true;
      this.label19.Location = new System.Drawing.Point(420, 175);
      this.label19.Name = "label19";
      this.label19.Size = new System.Drawing.Size(165, 15);
      this.label19.TabIndex = 15;
      this.label19.Text = "Path to Certificate File for SSL.";
      // 
      // textBox4
      // 
      this.textBox4.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.bindingSource, "SSLCipher", true));
      this.textBox4.Location = new System.Drawing.Point(83, 255);
      this.textBox4.Name = "textBox4";
      this.textBox4.Size = new System.Drawing.Size(325, 23);
      this.textBox4.TabIndex = 15;
      // 
      // textBox3
      // 
      this.textBox3.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.bindingSource, "SSLKey", true));
      this.textBox3.Location = new System.Drawing.Point(83, 208);
      this.textBox3.Name = "textBox3";
      this.textBox3.Size = new System.Drawing.Size(325, 23);
      this.textBox3.TabIndex = 14;
      // 
      // textBox2
      // 
      this.textBox2.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.bindingSource, "SSLCert", true));
      this.textBox2.Location = new System.Drawing.Point(83, 167);
      this.textBox2.Name = "textBox2";
      this.textBox2.Size = new System.Drawing.Size(325, 23);
      this.textBox2.TabIndex = 13;
      // 
      // textBox1
      // 
      this.textBox1.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.bindingSource, "SSLCA", true));
      this.textBox1.Location = new System.Drawing.Point(83, 127);
      this.textBox1.Name = "textBox1";
      this.textBox1.Size = new System.Drawing.Size(325, 23);
      this.textBox1.TabIndex = 12;
      // 
      // label18
      // 
      this.label18.AutoSize = true;
      this.label18.Location = new System.Drawing.Point(15, 263);
      this.label18.Name = "label18";
      this.label18.Size = new System.Drawing.Size(66, 15);
      this.label18.TabIndex = 10;
      this.label18.Text = "SSL Cipher:";
      // 
      // label17
      // 
      this.label17.AutoSize = true;
      this.label17.Location = new System.Drawing.Point(12, 216);
      this.label17.Name = "label17";
      this.label17.Size = new System.Drawing.Size(71, 15);
      this.label17.TabIndex = 9;
      this.label17.Text = "SSL Key File:";
      // 
      // label16
      // 
      this.label16.AutoSize = true;
      this.label16.Location = new System.Drawing.Point(3, 175);
      this.label16.Name = "label16";
      this.label16.Size = new System.Drawing.Size(80, 15);
      this.label16.TabIndex = 8;
      this.label16.Text = "SSL CERT File:";
      // 
      // label15
      // 
      this.label15.AutoSize = true;
      this.label15.Location = new System.Drawing.Point(420, 134);
      this.label15.Name = "label15";
      this.label15.Size = new System.Drawing.Size(218, 15);
      this.label15.TabIndex = 7;
      this.label15.Text = "Path to Certificate Authority File for SSL.";
      // 
      // label14
      // 
      this.label14.AutoSize = true;
      this.label14.Location = new System.Drawing.Point(15, 134);
      this.label14.Name = "label14";
      this.label14.Size = new System.Drawing.Size(68, 15);
      this.label14.TabIndex = 6;
      this.label14.Text = "SSL CA File:";
      // 
      // label13
      // 
      this.label13.AutoSize = true;
      this.label13.Location = new System.Drawing.Point(420, 84);
      this.label13.Name = "label13";
      this.label13.Size = new System.Drawing.Size(195, 15);
      this.label13.TabIndex = 5;
      this.label13.Text = "This option turns on SSL encryption";
      // 
      // label12
      // 
      this.label12.AutoSize = true;
      this.label12.Location = new System.Drawing.Point(420, 53);
      this.label12.Name = "label12";
      this.label12.Size = new System.Drawing.Size(301, 15);
      this.label12.TabIndex = 3;
      this.label12.Text = "If enabled this option overwrites the server side settings.";
      // 
      // labelCompression
      // 
      this.labelCompression.AutoSize = true;
      this.labelCompression.Location = new System.Drawing.Point(420, 26);
      this.labelCompression.Name = "labelCompression";
      this.labelCompression.Size = new System.Drawing.Size(215, 15);
      this.labelCompression.TabIndex = 1;
      this.labelCompression.Text = "Select this option for WAN connections";
      // 
      // useSSL
      // 
      this.useSSL.AutoSize = true;
      this.useSSL.DataBindings.Add(new System.Windows.Forms.Binding("Checked", this.bindingSource, "UseSSL", true));
      this.useSSL.Location = new System.Drawing.Point(87, 84);
      this.useSSL.Name = "useSSL";
      this.useSSL.Size = new System.Drawing.Size(125, 19);
      this.useSSL.TabIndex = 11;
      this.useSSL.Text = "Use SSL if available";
      this.useSSL.UseVisualStyleBackColor = true;
      // 
      // useANSI
      // 
      this.useANSI.AutoSize = true;
      this.useANSI.DataBindings.Add(new System.Windows.Forms.Binding("Checked", this.bindingSource, "UseAnsiQuotes", true));
      this.useANSI.Location = new System.Drawing.Point(87, 52);
      this.useANSI.Name = "useANSI";
      this.useANSI.Size = new System.Drawing.Size(216, 19);
      this.useANSI.TabIndex = 10;
      this.useANSI.Text = "Use ANSI quotes to quote identifiers";
      this.useANSI.UseVisualStyleBackColor = true;
      // 
      // useCompression
      // 
      this.useCompression.AutoSize = true;
      this.useCompression.DataBindings.Add(new System.Windows.Forms.Binding("Checked", this.bindingSource, "ClientCompress", true));
      this.useCompression.Location = new System.Drawing.Point(87, 22);
      this.useCompression.Name = "useCompression";
      this.useCompression.Size = new System.Drawing.Size(166, 19);
      this.useCompression.TabIndex = 9;
      this.useCompression.Text = "Use Compression protocol";
      this.useCompression.UseVisualStyleBackColor = true;
      // 
      // connectionMethod
      // 
      this.connectionMethod.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
      this.connectionMethod.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.connectionMethod.FormattingEnabled = true;
      this.connectionMethod.Items.AddRange(new object[] {
            "Standard (TCP/IP)",
            "Local Socket/Pipe"});
      this.connectionMethod.Location = new System.Drawing.Point(135, 40);
      this.connectionMethod.Name = "connectionMethod";
      this.connectionMethod.Size = new System.Drawing.Size(420, 23);
      this.connectionMethod.TabIndex = 1;
      this.connectionMethod.SelectedIndexChanged += new System.EventHandler(this.connectionMethod_SelectedIndexChanged);
      // 
      // labelHelpMethod
      // 
      this.labelHelpMethod.AutoSize = true;
      this.labelHelpMethod.BackColor = System.Drawing.Color.Transparent;
      this.labelHelpMethod.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.labelHelpMethod.Location = new System.Drawing.Point(561, 43);
      this.labelHelpMethod.Name = "labelHelpMethod";
      this.labelHelpMethod.Size = new System.Drawing.Size(220, 15);
      this.labelHelpMethod.TabIndex = 5;
      this.labelHelpMethod.Text = "Method to use to connect to the RDBMS";
      // 
      // labelPromptMethod
      // 
      this.labelPromptMethod.AutoSize = true;
      this.labelPromptMethod.BackColor = System.Drawing.Color.Transparent;
      this.labelPromptMethod.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.labelPromptMethod.Location = new System.Drawing.Point(12, 43);
      this.labelPromptMethod.Name = "labelPromptMethod";
      this.labelPromptMethod.Size = new System.Drawing.Size(117, 15);
      this.labelPromptMethod.TabIndex = 3;
      this.labelPromptMethod.Text = "Connection Method:";
      // 
      // labelHelpName
      // 
      this.labelHelpName.AutoSize = true;
      this.labelHelpName.BackColor = System.Drawing.Color.Transparent;
      this.labelHelpName.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.labelHelpName.Location = new System.Drawing.Point(561, 14);
      this.labelHelpName.Name = "labelHelpName";
      this.labelHelpName.Size = new System.Drawing.Size(176, 15);
      this.labelHelpName.TabIndex = 2;
      this.labelHelpName.Text = "Type a name for the connection";
      // 
      // connectionName
      // 
      this.connectionName.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.bindingSource, "Name", true));
      this.connectionName.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.connectionName.Location = new System.Drawing.Point(135, 11);
      this.connectionName.MaxLength = 358;
      this.connectionName.Name = "connectionName";
      this.connectionName.Size = new System.Drawing.Size(420, 23);
      this.connectionName.TabIndex = 0;
      this.connectionName.TextChanged += new System.EventHandler(this.connectionName_TextChanged);
      // 
      // labelPromptName
      // 
      this.labelPromptName.AutoSize = true;
      this.labelPromptName.BackColor = System.Drawing.Color.Transparent;
      this.labelPromptName.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.labelPromptName.Location = new System.Drawing.Point(22, 14);
      this.labelPromptName.Name = "labelPromptName";
      this.labelPromptName.Size = new System.Drawing.Size(107, 15);
      this.labelPromptName.TabIndex = 0;
      this.labelPromptName.Text = "Connection Name:";
      // 
      // testButton
      // 
      this.testButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
      this.testButton.Location = new System.Drawing.Point(494, 11);
      this.testButton.Name = "testButton";
      this.testButton.Size = new System.Drawing.Size(128, 23);
      this.testButton.TabIndex = 18;
      this.testButton.Text = "Test Connection";
      this.testButton.UseVisualStyleBackColor = true;
      this.testButton.Click += new System.EventHandler(this.testButton_Click);
      // 
      // cancelButton
      // 
      this.cancelButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
      this.cancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
      this.cancelButton.Location = new System.Drawing.Point(628, 11);
      this.cancelButton.Name = "cancelButton";
      this.cancelButton.Size = new System.Drawing.Size(75, 23);
      this.cancelButton.TabIndex = 17;
      this.cancelButton.Text = "Cancel";
      this.cancelButton.UseVisualStyleBackColor = true;
      // 
      // okButton
      // 
      this.okButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
      this.okButton.DialogResult = System.Windows.Forms.DialogResult.OK;
      this.okButton.Enabled = false;
      this.okButton.Location = new System.Drawing.Point(709, 11);
      this.okButton.Name = "okButton";
      this.okButton.Size = new System.Drawing.Size(75, 23);
      this.okButton.TabIndex = 16;
      this.okButton.Text = "OK";
      this.okButton.UseVisualStyleBackColor = true;
      this.okButton.Click += new System.EventHandler(this.okButton_Click);
      // 
      // NewConnectionDialog
      // 
      this.AcceptButton = this.okButton;
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
      this.CancelButton = this.cancelButton;
      this.ClientSize = new System.Drawing.Size(796, 471);
      this.CommandAreaHeight = 45;
      this.Name = "NewConnectionDialog";
      this.Text = "Setup New Connection";
      this.contentAreaPanel.ResumeLayout(false);
      this.contentAreaPanel.PerformLayout();
      this.commandAreaPanel.ResumeLayout(false);
      this.tabControl1.ResumeLayout(false);
      this.parametersPage.ResumeLayout(false);
      this.parametersPage.PerformLayout();
      ((System.ComponentModel.ISupportInitialize)(this.bindingSource)).EndInit();
      this.advancedPage.ResumeLayout(false);
      this.advancedPage.PerformLayout();
      this.ResumeLayout(false);

    }

    #endregion

    private System.Windows.Forms.TabControl tabControl1;
    private System.Windows.Forms.TabPage parametersPage;
    private System.Windows.Forms.Label labelHelpSchema;
    private System.Windows.Forms.Label labelHelpUserName;
    private System.Windows.Forms.TextBox defaultSchema;
    private System.Windows.Forms.TextBox port;
    private System.Windows.Forms.TextBox userName;
    private System.Windows.Forms.TextBox hostName;
    private System.Windows.Forms.Label LabelPromptSchema;
    private System.Windows.Forms.Label labelHelpHostName;
    private System.Windows.Forms.Label labelPromptPort;
    private System.Windows.Forms.Label labelPromptUserName;
    private System.Windows.Forms.Label labelPromptHostName;
    private System.Windows.Forms.TabPage advancedPage;
    private System.Windows.Forms.Label label13;
    private System.Windows.Forms.Label label12;
    private System.Windows.Forms.Label labelCompression;
    private System.Windows.Forms.CheckBox useSSL;
    private System.Windows.Forms.CheckBox useANSI;
    private System.Windows.Forms.CheckBox useCompression;
    private System.Windows.Forms.ComboBox connectionMethod;
    private System.Windows.Forms.Label labelHelpMethod;
    private System.Windows.Forms.Label labelPromptMethod;
    private System.Windows.Forms.Label labelHelpName;
    private System.Windows.Forms.TextBox connectionName;
    private System.Windows.Forms.Label labelPromptName;
    private System.Windows.Forms.Button testButton;
    private System.Windows.Forms.Button cancelButton;
    private System.Windows.Forms.Button okButton;
    private System.Windows.Forms.Label label21;
    private System.Windows.Forms.Label label20;
    private System.Windows.Forms.Label label19;
    private System.Windows.Forms.TextBox textBox4;
    private System.Windows.Forms.TextBox textBox3;
    private System.Windows.Forms.TextBox textBox2;
    private System.Windows.Forms.TextBox textBox1;
    private System.Windows.Forms.Label label18;
    private System.Windows.Forms.Label label17;
    private System.Windows.Forms.Label label16;
    private System.Windows.Forms.Label label15;
    private System.Windows.Forms.Label label14;
    private System.Windows.Forms.BindingSource bindingSource;
    private System.Windows.Forms.Label labelHelpSocket;
    private System.Windows.Forms.Label labelPromptSocket;
    private System.Windows.Forms.TextBox socketPath;
  }
}