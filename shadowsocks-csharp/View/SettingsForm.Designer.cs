namespace Shadowsocks.View
{
    partial class SettingsForm
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
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
            this.lblBalance = new System.Windows.Forms.Label();
            this.cmbBalance = new System.Windows.Forms.ComboBox();
            this.chkAutoBan = new System.Windows.Forms.CheckBox();
            this.chkBalance = new System.Windows.Forms.CheckBox();
            this.chkAutoStartup = new System.Windows.Forms.CheckBox();
            this.chkBalanceInGroup = new System.Windows.Forms.CheckBox();
            this.gbxSocks5Proxy = new System.Windows.Forms.GroupBox();
            this.tableLayoutPanel9 = new System.Windows.Forms.TableLayoutPanel();
            this.lblS5Password = new System.Windows.Forms.Label();
            this.lblS5Username = new System.Windows.Forms.Label();
            this.txtS5Pass = new System.Windows.Forms.TextBox();
            this.lblS5Port = new System.Windows.Forms.Label();
            this.txtS5User = new System.Windows.Forms.TextBox();
            this.lblS5Server = new System.Windows.Forms.Label();
            this.nudS5Port = new System.Windows.Forms.NumericUpDown();
            this.txtS5Server = new System.Windows.Forms.TextBox();
            this.cmbProxyType = new System.Windows.Forms.ComboBox();
            this.chkSockProxy = new System.Windows.Forms.CheckBox();
            this.chkPacProxy = new System.Windows.Forms.CheckBox();
            this.lblUserAgent = new System.Windows.Forms.Label();
            this.txtUserAgent = new System.Windows.Forms.TextBox();
            this.tableLayoutPanel10 = new System.Windows.Forms.TableLayoutPanel();
            this.tableLayoutPanel3 = new System.Windows.Forms.TableLayoutPanel();
            this.btnApply = new System.Windows.Forms.Button();
            this.btnClose = new System.Windows.Forms.Button();
            this.btnOK = new System.Windows.Forms.Button();
            this.tableLayoutPanel5 = new System.Windows.Forms.TableLayoutPanel();
            this.lblReconnect = new System.Windows.Forms.Label();
            this.nudReconnect = new System.Windows.Forms.NumericUpDown();
            this.lblTtl = new System.Windows.Forms.Label();
            this.nudTTL = new System.Windows.Forms.NumericUpDown();
            this.lblTimeout = new System.Windows.Forms.Label();
            this.nudTimeout = new System.Windows.Forms.NumericUpDown();
            this.txtDNS = new System.Windows.Forms.TextBox();
            this.btnSetDefault = new System.Windows.Forms.Button();
            this.lblDNS = new System.Windows.Forms.Label();
            this.lblLocalDns = new System.Windows.Forms.Label();
            this.txtLocalDNS = new System.Windows.Forms.TextBox();
            this.gbxListen = new System.Windows.Forms.GroupBox();
            this.tableLayoutPanel4 = new System.Windows.Forms.TableLayoutPanel();
            this.txtAuthPass = new System.Windows.Forms.TextBox();
            this.lblAuthPass = new System.Windows.Forms.Label();
            this.txtAuthUser = new System.Windows.Forms.TextBox();
            this.lblAuthUser = new System.Windows.Forms.Label();
            this.chkShareOverLan = new System.Windows.Forms.CheckBox();
            this.nudProxyPort = new System.Windows.Forms.NumericUpDown();
            this.lblProxyPort = new System.Windows.Forms.Label();
            this.chkLoggingEnable = new System.Windows.Forms.CheckBox();
            this.tableLayoutPanel1.SuspendLayout();
            this.tableLayoutPanel2.SuspendLayout();
            this.gbxSocks5Proxy.SuspendLayout();
            this.tableLayoutPanel9.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudS5Port)).BeginInit();
            this.tableLayoutPanel10.SuspendLayout();
            this.tableLayoutPanel3.SuspendLayout();
            this.tableLayoutPanel5.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudReconnect)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudTTL)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudTimeout)).BeginInit();
            this.gbxListen.SuspendLayout();
            this.tableLayoutPanel4.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudProxyPort)).BeginInit();
            this.SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.AutoSize = true;
            this.tableLayoutPanel1.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.tableLayoutPanel1.ColumnCount = 2;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel1.Controls.Add(this.tableLayoutPanel2, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this.gbxSocks5Proxy, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.tableLayoutPanel10, 1, 2);
            this.tableLayoutPanel1.Controls.Add(this.gbxListen, 0, 2);
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Margin = new System.Windows.Forms.Padding(0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 3;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.Size = new System.Drawing.Size(574, 414);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // tableLayoutPanel2
            // 
            this.tableLayoutPanel2.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.tableLayoutPanel2.AutoSize = true;
            this.tableLayoutPanel2.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.tableLayoutPanel2.ColumnCount = 2;
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel2.Controls.Add(this.lblBalance, 0, 2);
            this.tableLayoutPanel2.Controls.Add(this.cmbBalance, 1, 2);
            this.tableLayoutPanel2.Controls.Add(this.chkAutoBan, 1, 4);
            this.tableLayoutPanel2.Controls.Add(this.chkBalance, 1, 1);
            this.tableLayoutPanel2.Controls.Add(this.chkAutoStartup, 1, 0);
            this.tableLayoutPanel2.Controls.Add(this.chkBalanceInGroup, 1, 3);
            this.tableLayoutPanel2.Controls.Add(this.chkLoggingEnable, 1, 5);
            this.tableLayoutPanel2.Location = new System.Drawing.Point(353, 3);
            this.tableLayoutPanel2.Name = "tableLayoutPanel2";
            this.tableLayoutPanel2.RowCount = 6;
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel2.Size = new System.Drawing.Size(205, 146);
            this.tableLayoutPanel2.TabIndex = 2;
            // 
            // lblBalance
            // 
            this.lblBalance.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.lblBalance.AutoSize = true;
            this.lblBalance.Location = new System.Drawing.Point(3, 55);
            this.lblBalance.Name = "lblBalance";
            this.lblBalance.Size = new System.Drawing.Size(46, 13);
            this.lblBalance.TabIndex = 12;
            this.lblBalance.Text = "Balance";
            this.lblBalance.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // cmbBalance
            // 
            this.cmbBalance.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.cmbBalance.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbBalance.FormattingEnabled = true;
            this.cmbBalance.Items.AddRange(new object[] {
            "OneByOne",
            "Random",
            "FastDownloadSpeed",
            "LowLatency",
            "LowException",
            "SelectedFirst",
            "Timer"});
            this.cmbBalance.Location = new System.Drawing.Point(55, 49);
            this.cmbBalance.Margin = new System.Windows.Forms.Padding(3, 3, 3, 7);
            this.cmbBalance.Name = "cmbBalance";
            this.cmbBalance.Size = new System.Drawing.Size(147, 21);
            this.cmbBalance.TabIndex = 14;
            // 
            // chkAutoBan
            // 
            this.chkAutoBan.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.chkAutoBan.AutoSize = true;
            this.chkAutoBan.Location = new System.Drawing.Point(55, 103);
            this.chkAutoBan.Name = "chkAutoBan";
            this.chkAutoBan.Size = new System.Drawing.Size(67, 17);
            this.chkAutoBan.TabIndex = 15;
            this.chkAutoBan.Text = "AutoBan";
            this.chkAutoBan.UseVisualStyleBackColor = true;
            // 
            // chkBalance
            // 
            this.chkBalance.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.chkBalance.AutoSize = true;
            this.chkBalance.Location = new System.Drawing.Point(55, 26);
            this.chkBalance.Name = "chkBalance";
            this.chkBalance.Size = new System.Drawing.Size(100, 17);
            this.chkBalance.TabIndex = 13;
            this.chkBalance.Text = "Enable balance";
            this.chkBalance.UseVisualStyleBackColor = true;
            // 
            // chkAutoStartup
            // 
            this.chkAutoStartup.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.chkAutoStartup.AutoSize = true;
            this.chkAutoStartup.Location = new System.Drawing.Point(55, 3);
            this.chkAutoStartup.Name = "chkAutoStartup";
            this.chkAutoStartup.Size = new System.Drawing.Size(88, 17);
            this.chkAutoStartup.TabIndex = 12;
            this.chkAutoStartup.Text = "Start on Boot";
            this.chkAutoStartup.UseVisualStyleBackColor = true;
            // 
            // chkBalanceInGroup
            // 
            this.chkBalanceInGroup.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.chkBalanceInGroup.AutoSize = true;
            this.chkBalanceInGroup.Location = new System.Drawing.Point(55, 80);
            this.chkBalanceInGroup.Name = "chkBalanceInGroup";
            this.chkBalanceInGroup.Size = new System.Drawing.Size(106, 17);
            this.chkBalanceInGroup.TabIndex = 15;
            this.chkBalanceInGroup.Text = "Balance in group";
            this.chkBalanceInGroup.UseVisualStyleBackColor = true;
            // 
            // gbxSocks5Proxy
            // 
            this.gbxSocks5Proxy.AutoSize = true;
            this.gbxSocks5Proxy.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.gbxSocks5Proxy.Controls.Add(this.tableLayoutPanel9);
            this.gbxSocks5Proxy.Location = new System.Drawing.Point(14, 0);
            this.gbxSocks5Proxy.Margin = new System.Windows.Forms.Padding(14, 0, 0, 0);
            this.gbxSocks5Proxy.Name = "gbxSocks5Proxy";
            this.gbxSocks5Proxy.Size = new System.Drawing.Size(323, 199);
            this.gbxSocks5Proxy.TabIndex = 0;
            this.gbxSocks5Proxy.TabStop = false;
            this.gbxSocks5Proxy.Text = "Remote proxy";
            // 
            // tableLayoutPanel9
            // 
            this.tableLayoutPanel9.AutoSize = true;
            this.tableLayoutPanel9.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.tableLayoutPanel9.ColumnCount = 2;
            this.tableLayoutPanel9.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel9.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel9.Controls.Add(this.lblS5Password, 0, 5);
            this.tableLayoutPanel9.Controls.Add(this.lblS5Username, 0, 4);
            this.tableLayoutPanel9.Controls.Add(this.txtS5Pass, 1, 5);
            this.tableLayoutPanel9.Controls.Add(this.lblS5Port, 0, 3);
            this.tableLayoutPanel9.Controls.Add(this.txtS5User, 1, 4);
            this.tableLayoutPanel9.Controls.Add(this.lblS5Server, 0, 2);
            this.tableLayoutPanel9.Controls.Add(this.nudS5Port, 1, 3);
            this.tableLayoutPanel9.Controls.Add(this.txtS5Server, 1, 2);
            this.tableLayoutPanel9.Controls.Add(this.cmbProxyType, 1, 1);
            this.tableLayoutPanel9.Controls.Add(this.chkSockProxy, 0, 0);
            this.tableLayoutPanel9.Controls.Add(this.chkPacProxy, 1, 0);
            this.tableLayoutPanel9.Controls.Add(this.lblUserAgent, 0, 6);
            this.tableLayoutPanel9.Controls.Add(this.txtUserAgent, 1, 6);
            this.tableLayoutPanel9.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel9.Location = new System.Drawing.Point(3, 16);
            this.tableLayoutPanel9.Name = "tableLayoutPanel9";
            this.tableLayoutPanel9.RowCount = 7;
            this.tableLayoutPanel9.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel9.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel9.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel9.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel9.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel9.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel9.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel9.Size = new System.Drawing.Size(317, 180);
            this.tableLayoutPanel9.TabIndex = 0;
            // 
            // lblS5Password
            // 
            this.lblS5Password.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.lblS5Password.AutoSize = true;
            this.lblS5Password.Location = new System.Drawing.Point(19, 134);
            this.lblS5Password.Name = "lblS5Password";
            this.lblS5Password.Size = new System.Drawing.Size(53, 13);
            this.lblS5Password.TabIndex = 5;
            this.lblS5Password.Text = "Password";
            // 
            // lblS5Username
            // 
            this.lblS5Username.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.lblS5Username.AutoSize = true;
            this.lblS5Username.Location = new System.Drawing.Point(17, 108);
            this.lblS5Username.Name = "lblS5Username";
            this.lblS5Username.Size = new System.Drawing.Size(55, 13);
            this.lblS5Username.TabIndex = 4;
            this.lblS5Username.Text = "Username";
            // 
            // txtS5Pass
            // 
            this.txtS5Pass.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.txtS5Pass.ImeMode = System.Windows.Forms.ImeMode.Off;
            this.txtS5Pass.Location = new System.Drawing.Point(78, 131);
            this.txtS5Pass.Name = "txtS5Pass";
            this.txtS5Pass.Size = new System.Drawing.Size(236, 20);
            this.txtS5Pass.TabIndex = 6;
            // 
            // lblS5Port
            // 
            this.lblS5Port.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.lblS5Port.AutoSize = true;
            this.lblS5Port.Location = new System.Drawing.Point(12, 82);
            this.lblS5Port.Name = "lblS5Port";
            this.lblS5Port.Size = new System.Drawing.Size(60, 13);
            this.lblS5Port.TabIndex = 1;
            this.lblS5Port.Text = "Server Port";
            // 
            // txtS5User
            // 
            this.txtS5User.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.txtS5User.ImeMode = System.Windows.Forms.ImeMode.Off;
            this.txtS5User.Location = new System.Drawing.Point(78, 105);
            this.txtS5User.Name = "txtS5User";
            this.txtS5User.Size = new System.Drawing.Size(236, 20);
            this.txtS5User.TabIndex = 5;
            // 
            // lblS5Server
            // 
            this.lblS5Server.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.lblS5Server.AutoSize = true;
            this.lblS5Server.Location = new System.Drawing.Point(21, 56);
            this.lblS5Server.Name = "lblS5Server";
            this.lblS5Server.Size = new System.Drawing.Size(51, 13);
            this.lblS5Server.TabIndex = 0;
            this.lblS5Server.Text = "Server IP";
            // 
            // nudS5Port
            // 
            this.nudS5Port.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.nudS5Port.ImeMode = System.Windows.Forms.ImeMode.Off;
            this.nudS5Port.Location = new System.Drawing.Point(78, 79);
            this.nudS5Port.Maximum = new decimal(new int[] {
            65535,
            0,
            0,
            0});
            this.nudS5Port.Name = "nudS5Port";
            this.nudS5Port.Size = new System.Drawing.Size(236, 20);
            this.nudS5Port.TabIndex = 4;
            // 
            // txtS5Server
            // 
            this.txtS5Server.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.txtS5Server.ImeMode = System.Windows.Forms.ImeMode.Off;
            this.txtS5Server.Location = new System.Drawing.Point(78, 53);
            this.txtS5Server.Name = "txtS5Server";
            this.txtS5Server.Size = new System.Drawing.Size(236, 20);
            this.txtS5Server.TabIndex = 3;
            // 
            // cmbProxyType
            // 
            this.cmbProxyType.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.cmbProxyType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbProxyType.FormattingEnabled = true;
            this.cmbProxyType.Items.AddRange(new object[] {
            "Socks5(support UDP)",
            "Http tunnel",
            "TCP Port tunnel"});
            this.cmbProxyType.Location = new System.Drawing.Point(78, 26);
            this.cmbProxyType.Name = "cmbProxyType";
            this.cmbProxyType.Size = new System.Drawing.Size(236, 21);
            this.cmbProxyType.TabIndex = 2;
            // 
            // chkSockProxy
            // 
            this.chkSockProxy.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.chkSockProxy.AutoSize = true;
            this.chkSockProxy.Location = new System.Drawing.Point(3, 3);
            this.chkSockProxy.Name = "chkSockProxy";
            this.chkSockProxy.Size = new System.Drawing.Size(69, 17);
            this.chkSockProxy.TabIndex = 0;
            this.chkSockProxy.Text = "Proxy On";
            this.chkSockProxy.UseVisualStyleBackColor = true;
            // 
            // chkPacProxy
            // 
            this.chkPacProxy.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.chkPacProxy.AutoSize = true;
            this.chkPacProxy.Location = new System.Drawing.Point(78, 3);
            this.chkPacProxy.Name = "chkPacProxy";
            this.chkPacProxy.Size = new System.Drawing.Size(163, 17);
            this.chkPacProxy.TabIndex = 1;
            this.chkPacProxy.Text = "PAC \"direct\" return this proxy";
            this.chkPacProxy.UseVisualStyleBackColor = true;
            // 
            // lblUserAgent
            // 
            this.lblUserAgent.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.lblUserAgent.AutoSize = true;
            this.lblUserAgent.Location = new System.Drawing.Point(12, 160);
            this.lblUserAgent.Name = "lblUserAgent";
            this.lblUserAgent.Size = new System.Drawing.Size(60, 13);
            this.lblUserAgent.TabIndex = 5;
            this.lblUserAgent.Text = "User Agent";
            // 
            // txtUserAgent
            // 
            this.txtUserAgent.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.txtUserAgent.ImeMode = System.Windows.Forms.ImeMode.Off;
            this.txtUserAgent.Location = new System.Drawing.Point(78, 157);
            this.txtUserAgent.Name = "txtUserAgent";
            this.txtUserAgent.Size = new System.Drawing.Size(236, 20);
            this.txtUserAgent.TabIndex = 7;
            // 
            // tableLayoutPanel10
            // 
            this.tableLayoutPanel10.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.tableLayoutPanel10.AutoSize = true;
            this.tableLayoutPanel10.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.tableLayoutPanel10.ColumnCount = 1;
            this.tableLayoutPanel10.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel10.Controls.Add(this.tableLayoutPanel3, 0, 2);
            this.tableLayoutPanel10.Controls.Add(this.tableLayoutPanel5, 0, 1);
            this.tableLayoutPanel10.Location = new System.Drawing.Point(340, 202);
            this.tableLayoutPanel10.Name = "tableLayoutPanel10";
            this.tableLayoutPanel10.RowCount = 3;
            this.tableLayoutPanel10.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel10.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel10.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel10.Size = new System.Drawing.Size(231, 209);
            this.tableLayoutPanel10.TabIndex = 3;
            // 
            // tableLayoutPanel3
            // 
            this.tableLayoutPanel3.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.tableLayoutPanel3.AutoSize = true;
            this.tableLayoutPanel3.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.tableLayoutPanel3.ColumnCount = 3;
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.tableLayoutPanel3.Controls.Add(this.btnApply, 2, 0);
            this.tableLayoutPanel3.Controls.Add(this.btnClose, 1, 0);
            this.tableLayoutPanel3.Controls.Add(this.btnOK, 0, 0);
            this.tableLayoutPanel3.Location = new System.Drawing.Point(3, 168);
            this.tableLayoutPanel3.Margin = new System.Windows.Forms.Padding(3, 3, 0, 3);
            this.tableLayoutPanel3.Name = "tableLayoutPanel3";
            this.tableLayoutPanel3.RowCount = 1;
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel3.Size = new System.Drawing.Size(228, 38);
            this.tableLayoutPanel3.TabIndex = 14;
            // 
            // btnApply
            // 
            this.btnApply.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.btnApply.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.btnApply.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btnApply.Font = new System.Drawing.Font("SimSun", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnApply.Location = new System.Drawing.Point(155, 3);
            this.btnApply.Margin = new System.Windows.Forms.Padding(3, 3, 3, 0);
            this.btnApply.Name = "btnApply";
            this.btnApply.Size = new System.Drawing.Size(70, 35);
            this.btnApply.TabIndex = 22;
            this.btnApply.Text = "Apply";
            this.btnApply.UseVisualStyleBackColor = true;
            this.btnApply.Click += new System.EventHandler(this.BtnApply_Click);
            // 
            // btnClose
            // 
            this.btnClose.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.btnClose.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.btnClose.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnClose.Font = new System.Drawing.Font("SimSun", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnClose.Location = new System.Drawing.Point(80, 3);
            this.btnClose.Margin = new System.Windows.Forms.Padding(3, 3, 0, 0);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(70, 35);
            this.btnClose.TabIndex = 22;
            this.btnClose.Text = "Close";
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.BtnClose_Click);
            // 
            // btnOK
            // 
            this.btnOK.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.btnOK.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.btnOK.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btnOK.Font = new System.Drawing.Font("SimSun", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnOK.Location = new System.Drawing.Point(3, 3);
            this.btnOK.Margin = new System.Windows.Forms.Padding(3, 3, 3, 0);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(70, 35);
            this.btnOK.TabIndex = 21;
            this.btnOK.Text = "OK";
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Click += new System.EventHandler(this.BtnOK_Click);
            // 
            // tableLayoutPanel5
            // 
            this.tableLayoutPanel5.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.tableLayoutPanel5.AutoSize = true;
            this.tableLayoutPanel5.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.tableLayoutPanel5.ColumnCount = 2;
            this.tableLayoutPanel5.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel5.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel5.Controls.Add(this.lblReconnect, 0, 4);
            this.tableLayoutPanel5.Controls.Add(this.nudReconnect, 1, 4);
            this.tableLayoutPanel5.Controls.Add(this.lblTtl, 0, 6);
            this.tableLayoutPanel5.Controls.Add(this.nudTTL, 1, 6);
            this.tableLayoutPanel5.Controls.Add(this.lblTimeout, 0, 5);
            this.tableLayoutPanel5.Controls.Add(this.nudTimeout, 1, 5);
            this.tableLayoutPanel5.Controls.Add(this.txtDNS, 1, 1);
            this.tableLayoutPanel5.Controls.Add(this.btnSetDefault, 1, 0);
            this.tableLayoutPanel5.Controls.Add(this.lblDNS, 0, 1);
            this.tableLayoutPanel5.Controls.Add(this.lblLocalDns, 0, 2);
            this.tableLayoutPanel5.Controls.Add(this.txtLocalDNS, 1, 2);
            this.tableLayoutPanel5.Location = new System.Drawing.Point(6, 0);
            this.tableLayoutPanel5.Margin = new System.Windows.Forms.Padding(0);
            this.tableLayoutPanel5.Name = "tableLayoutPanel5";
            this.tableLayoutPanel5.Padding = new System.Windows.Forms.Padding(3);
            this.tableLayoutPanel5.RowCount = 7;
            this.tableLayoutPanel5.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel5.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel5.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel5.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel5.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel5.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel5.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel5.Size = new System.Drawing.Size(218, 165);
            this.tableLayoutPanel5.TabIndex = 3;
            // 
            // lblReconnect
            // 
            this.lblReconnect.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.lblReconnect.AutoSize = true;
            this.lblReconnect.Location = new System.Drawing.Point(6, 90);
            this.lblReconnect.Name = "lblReconnect";
            this.lblReconnect.Size = new System.Drawing.Size(91, 13);
            this.lblReconnect.TabIndex = 3;
            this.lblReconnect.Text = "Reconnect Times";
            this.lblReconnect.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // nudReconnect
            // 
            this.nudReconnect.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.nudReconnect.ImeMode = System.Windows.Forms.ImeMode.Off;
            this.nudReconnect.Location = new System.Drawing.Point(103, 87);
            this.nudReconnect.Maximum = new decimal(new int[] {
            20,
            0,
            0,
            0});
            this.nudReconnect.Name = "nudReconnect";
            this.nudReconnect.Size = new System.Drawing.Size(109, 20);
            this.nudReconnect.TabIndex = 18;
            // 
            // lblTtl
            // 
            this.lblTtl.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.lblTtl.AutoSize = true;
            this.lblTtl.Location = new System.Drawing.Point(70, 142);
            this.lblTtl.Name = "lblTtl";
            this.lblTtl.Size = new System.Drawing.Size(27, 13);
            this.lblTtl.TabIndex = 3;
            this.lblTtl.Text = "TTL";
            this.lblTtl.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // nudTTL
            // 
            this.nudTTL.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.nudTTL.ImeMode = System.Windows.Forms.ImeMode.Off;
            this.nudTTL.Location = new System.Drawing.Point(103, 139);
            this.nudTTL.Maximum = new decimal(new int[] {
            600,
            0,
            0,
            0});
            this.nudTTL.Name = "nudTTL";
            this.nudTTL.Size = new System.Drawing.Size(109, 20);
            this.nudTTL.TabIndex = 20;
            // 
            // lblTimeout
            // 
            this.lblTimeout.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.lblTimeout.AutoSize = true;
            this.lblTimeout.Location = new System.Drawing.Point(49, 116);
            this.lblTimeout.Name = "lblTimeout";
            this.lblTimeout.Size = new System.Drawing.Size(48, 13);
            this.lblTimeout.TabIndex = 3;
            this.lblTimeout.Text = " Timeout";
            this.lblTimeout.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // nudTimeout
            // 
            this.nudTimeout.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.nudTimeout.ImeMode = System.Windows.Forms.ImeMode.Off;
            this.nudTimeout.Location = new System.Drawing.Point(103, 113);
            this.nudTimeout.Maximum = new decimal(new int[] {
            60,
            0,
            0,
            0});
            this.nudTimeout.Name = "nudTimeout";
            this.nudTimeout.Size = new System.Drawing.Size(109, 20);
            this.nudTimeout.TabIndex = 19;
            // 
            // txtDNS
            // 
            this.txtDNS.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.txtDNS.ImeMode = System.Windows.Forms.ImeMode.Off;
            this.txtDNS.Location = new System.Drawing.Point(103, 35);
            this.txtDNS.MaxLength = 0;
            this.txtDNS.Name = "txtDNS";
            this.txtDNS.Size = new System.Drawing.Size(109, 20);
            this.txtDNS.TabIndex = 17;
            this.txtDNS.WordWrap = false;
            // 
            // btnSetDefault
            // 
            this.btnSetDefault.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.btnSetDefault.Location = new System.Drawing.Point(103, 6);
            this.btnSetDefault.Name = "btnSetDefault";
            this.btnSetDefault.Size = new System.Drawing.Size(109, 23);
            this.btnSetDefault.TabIndex = 16;
            this.btnSetDefault.Text = "Set Default";
            this.btnSetDefault.UseVisualStyleBackColor = true;
            this.btnSetDefault.Click += new System.EventHandler(this.btnSetDefault_Click);
            // 
            // lblDNS
            // 
            this.lblDNS.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.lblDNS.AutoSize = true;
            this.lblDNS.Location = new System.Drawing.Point(67, 38);
            this.lblDNS.Name = "lblDNS";
            this.lblDNS.Size = new System.Drawing.Size(30, 13);
            this.lblDNS.TabIndex = 3;
            this.lblDNS.Text = "DNS";
            this.lblDNS.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblLocalDns
            // 
            this.lblLocalDns.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.lblLocalDns.AutoSize = true;
            this.lblLocalDns.Location = new System.Drawing.Point(42, 64);
            this.lblLocalDns.Name = "lblLocalDns";
            this.lblLocalDns.Size = new System.Drawing.Size(55, 13);
            this.lblLocalDns.TabIndex = 3;
            this.lblLocalDns.Text = "Local Dns";
            this.lblLocalDns.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // txtLocalDNS
            // 
            this.txtLocalDNS.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.txtLocalDNS.ImeMode = System.Windows.Forms.ImeMode.Off;
            this.txtLocalDNS.Location = new System.Drawing.Point(103, 61);
            this.txtLocalDNS.MaxLength = 0;
            this.txtLocalDNS.Name = "txtLocalDNS";
            this.txtLocalDNS.Size = new System.Drawing.Size(109, 20);
            this.txtLocalDNS.TabIndex = 17;
            this.txtLocalDNS.WordWrap = false;
            // 
            // gbxListen
            // 
            this.gbxListen.AutoSize = true;
            this.gbxListen.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.gbxListen.Controls.Add(this.tableLayoutPanel4);
            this.gbxListen.Location = new System.Drawing.Point(14, 199);
            this.gbxListen.Margin = new System.Windows.Forms.Padding(14, 0, 0, 0);
            this.gbxListen.Name = "gbxListen";
            this.gbxListen.Size = new System.Drawing.Size(309, 120);
            this.gbxListen.TabIndex = 1;
            this.gbxListen.TabStop = false;
            this.gbxListen.Text = "Local proxy";
            // 
            // tableLayoutPanel4
            // 
            this.tableLayoutPanel4.AutoSize = true;
            this.tableLayoutPanel4.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.tableLayoutPanel4.ColumnCount = 2;
            this.tableLayoutPanel4.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel4.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel4.Controls.Add(this.txtAuthPass, 1, 3);
            this.tableLayoutPanel4.Controls.Add(this.lblAuthPass, 0, 3);
            this.tableLayoutPanel4.Controls.Add(this.txtAuthUser, 1, 2);
            this.tableLayoutPanel4.Controls.Add(this.lblAuthUser, 0, 2);
            this.tableLayoutPanel4.Controls.Add(this.chkShareOverLan, 1, 0);
            this.tableLayoutPanel4.Controls.Add(this.nudProxyPort, 1, 1);
            this.tableLayoutPanel4.Controls.Add(this.lblProxyPort, 0, 1);
            this.tableLayoutPanel4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel4.Location = new System.Drawing.Point(3, 16);
            this.tableLayoutPanel4.Name = "tableLayoutPanel4";
            this.tableLayoutPanel4.RowCount = 4;
            this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel4.Size = new System.Drawing.Size(303, 101);
            this.tableLayoutPanel4.TabIndex = 0;
            // 
            // txtAuthPass
            // 
            this.txtAuthPass.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.txtAuthPass.ImeMode = System.Windows.Forms.ImeMode.Off;
            this.txtAuthPass.Location = new System.Drawing.Point(64, 78);
            this.txtAuthPass.Name = "txtAuthPass";
            this.txtAuthPass.Size = new System.Drawing.Size(236, 20);
            this.txtAuthPass.TabIndex = 11;
            // 
            // lblAuthPass
            // 
            this.lblAuthPass.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.lblAuthPass.AutoSize = true;
            this.lblAuthPass.Location = new System.Drawing.Point(5, 81);
            this.lblAuthPass.Name = "lblAuthPass";
            this.lblAuthPass.Size = new System.Drawing.Size(53, 13);
            this.lblAuthPass.TabIndex = 8;
            this.lblAuthPass.Text = "Password";
            this.lblAuthPass.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // txtAuthUser
            // 
            this.txtAuthUser.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.txtAuthUser.ImeMode = System.Windows.Forms.ImeMode.Off;
            this.txtAuthUser.Location = new System.Drawing.Point(64, 52);
            this.txtAuthUser.Name = "txtAuthUser";
            this.txtAuthUser.Size = new System.Drawing.Size(236, 20);
            this.txtAuthUser.TabIndex = 10;
            // 
            // lblAuthUser
            // 
            this.lblAuthUser.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.lblAuthUser.AutoSize = true;
            this.lblAuthUser.Location = new System.Drawing.Point(3, 55);
            this.lblAuthUser.Name = "lblAuthUser";
            this.lblAuthUser.Size = new System.Drawing.Size(55, 13);
            this.lblAuthUser.TabIndex = 5;
            this.lblAuthUser.Text = "Username";
            this.lblAuthUser.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // chkShareOverLan
            // 
            this.chkShareOverLan.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.chkShareOverLan.AutoSize = true;
            this.chkShareOverLan.Location = new System.Drawing.Point(64, 3);
            this.chkShareOverLan.Name = "chkShareOverLan";
            this.chkShareOverLan.Size = new System.Drawing.Size(132, 17);
            this.chkShareOverLan.TabIndex = 8;
            this.chkShareOverLan.Text = "Allow Clients from LAN";
            this.chkShareOverLan.UseVisualStyleBackColor = true;
            // 
            // nudProxyPort
            // 
            this.nudProxyPort.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.nudProxyPort.ImeMode = System.Windows.Forms.ImeMode.Off;
            this.nudProxyPort.Location = new System.Drawing.Point(64, 26);
            this.nudProxyPort.Maximum = new decimal(new int[] {
            65535,
            0,
            0,
            0});
            this.nudProxyPort.Name = "nudProxyPort";
            this.nudProxyPort.Size = new System.Drawing.Size(236, 20);
            this.nudProxyPort.TabIndex = 9;
            // 
            // lblProxyPort
            // 
            this.lblProxyPort.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.lblProxyPort.AutoSize = true;
            this.lblProxyPort.Location = new System.Drawing.Point(3, 29);
            this.lblProxyPort.Name = "lblProxyPort";
            this.lblProxyPort.Size = new System.Drawing.Size(55, 13);
            this.lblProxyPort.TabIndex = 3;
            this.lblProxyPort.Text = "Proxy Port";
            this.lblProxyPort.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // chkLoggingEnable
            // 
            this.chkLoggingEnable.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.chkLoggingEnable.AutoSize = true;
            this.chkLoggingEnable.Location = new System.Drawing.Point(55, 126);
            this.chkLoggingEnable.Name = "chkLoggingEnable";
            this.chkLoggingEnable.Size = new System.Drawing.Size(96, 17);
            this.chkLoggingEnable.TabIndex = 16;
            this.chkLoggingEnable.Text = "Enable logging";
            this.chkLoggingEnable.UseVisualStyleBackColor = true;
            // 
            // SettingsForm
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.AutoSize = true;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.CancelButton = this.btnClose;
            this.ClientSize = new System.Drawing.Size(1152, 708);
            this.Controls.Add(this.tableLayoutPanel1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "SettingsForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "SettingsForm";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.SettingsForm_FormClosed);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.tableLayoutPanel2.ResumeLayout(false);
            this.tableLayoutPanel2.PerformLayout();
            this.gbxSocks5Proxy.ResumeLayout(false);
            this.gbxSocks5Proxy.PerformLayout();
            this.tableLayoutPanel9.ResumeLayout(false);
            this.tableLayoutPanel9.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudS5Port)).EndInit();
            this.tableLayoutPanel10.ResumeLayout(false);
            this.tableLayoutPanel10.PerformLayout();
            this.tableLayoutPanel3.ResumeLayout(false);
            this.tableLayoutPanel5.ResumeLayout(false);
            this.tableLayoutPanel5.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudReconnect)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudTTL)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudTimeout)).EndInit();
            this.gbxListen.ResumeLayout(false);
            this.gbxListen.PerformLayout();
            this.tableLayoutPanel4.ResumeLayout(false);
            this.tableLayoutPanel4.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudProxyPort)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.GroupBox gbxSocks5Proxy;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel9;
        private System.Windows.Forms.TextBox txtS5Pass;
        private System.Windows.Forms.TextBox txtS5User;
        private System.Windows.Forms.Label lblS5Password;
        private System.Windows.Forms.Label lblS5Server;
        private System.Windows.Forms.Label lblS5Port;
        private System.Windows.Forms.TextBox txtS5Server;
        private System.Windows.Forms.NumericUpDown nudS5Port;
        private System.Windows.Forms.Label lblS5Username;
        private System.Windows.Forms.CheckBox chkSockProxy;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel10;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel3;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel5;
        private System.Windows.Forms.NumericUpDown nudProxyPort;
        private System.Windows.Forms.Label lblProxyPort;
        private System.Windows.Forms.Label lblReconnect;
        private System.Windows.Forms.NumericUpDown nudReconnect;
        private System.Windows.Forms.Label lblTtl;
        private System.Windows.Forms.NumericUpDown nudTTL;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
        private System.Windows.Forms.Label lblBalance;
        private System.Windows.Forms.ComboBox cmbBalance;
        private System.Windows.Forms.CheckBox chkAutoBan;
        private System.Windows.Forms.CheckBox chkBalance;
        private System.Windows.Forms.CheckBox chkAutoStartup;
        private System.Windows.Forms.CheckBox chkShareOverLan;
        private System.Windows.Forms.ComboBox cmbProxyType;
        private System.Windows.Forms.GroupBox gbxListen;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel4;
        private System.Windows.Forms.TextBox txtAuthPass;
        private System.Windows.Forms.Label lblAuthPass;
        private System.Windows.Forms.TextBox txtAuthUser;
        private System.Windows.Forms.Label lblAuthUser;
        private System.Windows.Forms.CheckBox chkPacProxy;
        private System.Windows.Forms.Label lblUserAgent;
        private System.Windows.Forms.TextBox txtUserAgent;
        private System.Windows.Forms.Label lblDNS;
        private System.Windows.Forms.TextBox txtDNS;
        private System.Windows.Forms.Label lblTimeout;
        private System.Windows.Forms.NumericUpDown nudTimeout;
        private System.Windows.Forms.Button btnSetDefault;
        private System.Windows.Forms.CheckBox chkBalanceInGroup;
        private System.Windows.Forms.Label lblLocalDns;
        private System.Windows.Forms.TextBox txtLocalDNS;
        private System.Windows.Forms.Button btnApply;
        private System.Windows.Forms.CheckBox chkLoggingEnable;
    }
}