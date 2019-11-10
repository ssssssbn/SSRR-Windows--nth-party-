namespace Shadowsocks.View
{
    partial class ConfigForm
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.panel2 = new System.Windows.Forms.Panel();
            this.btnDelete = new System.Windows.Forms.Button();
            this.btnAdd = new System.Windows.Forms.Button();
            this.picQRcode = new System.Windows.Forms.PictureBox();
            this.lstServers = new System.Windows.Forms.ListBox();
            this.llbUpdate = new System.Windows.Forms.LinkLabel();
            this.btnDown = new System.Windows.Forms.Button();
            this.btnUp = new System.Windows.Forms.Button();
            this.ServerGroupBox = new System.Windows.Forms.GroupBox();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.labelIP = new System.Windows.Forms.CheckBox();
            this.labelServerPort = new System.Windows.Forms.Label();
            this.labelPassword = new System.Windows.Forms.CheckBox();
            this.labelEncryption = new System.Windows.Forms.Label();
            this.labelTCPProtocol = new System.Windows.Forms.Label();
            this.labelProtocolParam = new System.Windows.Forms.Label();
            this.labelObfs = new System.Windows.Forms.Label();
            this.labelObfsParam = new System.Windows.Forms.Label();
            this.labelRemarks = new System.Windows.Forms.Label();
            this.labelGroup = new System.Windows.Forms.Label();
            this.chkSSRLink = new System.Windows.Forms.CheckBox();
            this.chkAdvSetting = new System.Windows.Forms.CheckBox();
            this.labelUDPPort = new System.Windows.Forms.Label();
            this.labelTCPoverUDP = new System.Windows.Forms.Label();
            this.labelUDPoverTCP = new System.Windows.Forms.Label();
            this.labelObfsUDP = new System.Windows.Forms.Label();
            this.txtIP = new System.Windows.Forms.TextBox();
            this.nudServerPort = new System.Windows.Forms.NumericUpDown();
            this.txtPassword = new System.Windows.Forms.TextBox();
            this.cmbEncryption = new System.Windows.Forms.ComboBox();
            this.cmbTcpProtocol = new System.Windows.Forms.ComboBox();
            this.txtProtocolParam = new System.Windows.Forms.TextBox();
            this.cmbObfs = new System.Windows.Forms.ComboBox();
            this.txtObfsParam = new System.Windows.Forms.TextBox();
            this.txtRemarks = new System.Windows.Forms.TextBox();
            this.txtGroup = new System.Windows.Forms.TextBox();
            this.tbSSRLink = new System.Windows.Forms.TextBox();
            this.labelNote = new System.Windows.Forms.Label();
            this.nudUdpPort = new System.Windows.Forms.NumericUpDown();
            this.chkTcpOverUdp = new System.Windows.Forms.CheckBox();
            this.chkUdpOverTcp = new System.Windows.Forms.CheckBox();
            this.CheckObfsUDP = new System.Windows.Forms.CheckBox();
            this.btnClose = new System.Windows.Forms.Button();
            this.btnOK = new System.Windows.Forms.Button();
            this.tableLayoutPanel3 = new System.Windows.Forms.TableLayoutPanel();
            this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
            this.tableLayoutPanel4 = new System.Windows.Forms.TableLayoutPanel();
            this.btnApply = new System.Windows.Forms.Button();
            this.timer_btnLongPress = new Shadowsocks.Model.MyFormsTimer(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.picQRcode)).BeginInit();
            this.ServerGroupBox.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudServerPort)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudUdpPort)).BeginInit();
            this.tableLayoutPanel3.SuspendLayout();
            this.tableLayoutPanel2.SuspendLayout();
            this.tableLayoutPanel4.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel2
            // 
            this.panel2.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.panel2.AutoSize = true;
            this.panel2.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.panel2.Location = new System.Drawing.Point(902, 187);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(0, 0);
            this.panel2.TabIndex = 1;
            // 
            // btnDelete
            // 
            this.btnDelete.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.btnDelete.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.btnDelete.Location = new System.Drawing.Point(125, 277);
            this.btnDelete.Margin = new System.Windows.Forms.Padding(0);
            this.btnDelete.Name = "btnDelete";
            this.btnDelete.Size = new System.Drawing.Size(120, 34);
            this.btnDelete.TabIndex = 2;
            this.btnDelete.Text = "&Delete";
            this.btnDelete.UseVisualStyleBackColor = true;
            this.btnDelete.Click += new System.EventHandler(this.BtnDelete_Click);
            this.btnDelete.MouseDown += new System.Windows.Forms.MouseEventHandler(this.btn_MouseDown);
            this.btnDelete.MouseUp += new System.Windows.Forms.MouseEventHandler(this.btn_MouseUp);
            // 
            // btnAdd
            // 
            this.btnAdd.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.btnAdd.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.btnAdd.Location = new System.Drawing.Point(0, 277);
            this.btnAdd.Margin = new System.Windows.Forms.Padding(0);
            this.btnAdd.Name = "btnAdd";
            this.btnAdd.Size = new System.Drawing.Size(120, 34);
            this.btnAdd.TabIndex = 1;
            this.btnAdd.Text = "&Add";
            this.btnAdd.UseVisualStyleBackColor = true;
            this.btnAdd.Click += new System.EventHandler(this.BtnAdd_Click);
            // 
            // picQRcode
            // 
            this.picQRcode.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.picQRcode.BackColor = System.Drawing.SystemColors.Control;
            this.picQRcode.Location = new System.Drawing.Point(607, 125);
            this.picQRcode.Margin = new System.Windows.Forms.Padding(4);
            this.picQRcode.Name = "picQRcode";
            this.picQRcode.Size = new System.Drawing.Size(260, 200);
            this.picQRcode.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.picQRcode.TabIndex = 13;
            this.picQRcode.TabStop = false;
            this.picQRcode.Visible = false;
            // 
            // lstServers
            // 
            this.lstServers.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.tableLayoutPanel3.SetColumnSpan(this.lstServers, 2);
            this.lstServers.HorizontalScrollbar = true;
            this.lstServers.Location = new System.Drawing.Point(0, 0);
            this.lstServers.Margin = new System.Windows.Forms.Padding(0);
            this.lstServers.Name = "lstServers";
            this.lstServers.SelectionMode = System.Windows.Forms.SelectionMode.MultiExtended;
            this.lstServers.Size = new System.Drawing.Size(250, 277);
            this.lstServers.TabIndex = 0;
            this.lstServers.SelectedIndexChanged += new System.EventHandler(this.ServersListBox_SelectedIndexChanged);
            // 
            // llbUpdate
            // 
            this.llbUpdate.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.llbUpdate.AutoSize = true;
            this.llbUpdate.Location = new System.Drawing.Point(72, 467);
            this.llbUpdate.Margin = new System.Windows.Forms.Padding(5);
            this.llbUpdate.Name = "llbUpdate";
            this.llbUpdate.Size = new System.Drawing.Size(111, 13);
            this.llbUpdate.TabIndex = 2;
            this.llbUpdate.TabStop = true;
            this.llbUpdate.Text = "New version available";
            this.llbUpdate.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.LinkUpdate_LinkClicked);
            // 
            // btnDown
            // 
            this.btnDown.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.btnDown.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.btnDown.Location = new System.Drawing.Point(125, 311);
            this.btnDown.Margin = new System.Windows.Forms.Padding(0);
            this.btnDown.Name = "btnDown";
            this.btnDown.Size = new System.Drawing.Size(120, 34);
            this.btnDown.TabIndex = 4;
            this.btnDown.Text = "Down";
            this.btnDown.UseVisualStyleBackColor = true;
            this.btnDown.Click += new System.EventHandler(this.BtnDown_Click);
            this.btnDown.MouseDown += new System.Windows.Forms.MouseEventHandler(this.btn_MouseDown);
            this.btnDown.MouseUp += new System.Windows.Forms.MouseEventHandler(this.btn_MouseUp);
            // 
            // btnUp
            // 
            this.btnUp.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.btnUp.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.btnUp.Location = new System.Drawing.Point(0, 311);
            this.btnUp.Margin = new System.Windows.Forms.Padding(0);
            this.btnUp.Name = "btnUp";
            this.btnUp.Size = new System.Drawing.Size(120, 34);
            this.btnUp.TabIndex = 3;
            this.btnUp.Text = "Up";
            this.btnUp.UseVisualStyleBackColor = true;
            this.btnUp.Click += new System.EventHandler(this.BtnUp_Click);
            this.btnUp.MouseDown += new System.Windows.Forms.MouseEventHandler(this.btn_MouseDown);
            this.btnUp.MouseUp += new System.Windows.Forms.MouseEventHandler(this.btn_MouseUp);
            // 
            // ServerGroupBox
            // 
            this.ServerGroupBox.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.ServerGroupBox.AutoSize = true;
            this.ServerGroupBox.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.ServerGroupBox.Controls.Add(this.tableLayoutPanel1);
            this.ServerGroupBox.Location = new System.Drawing.Point(259, 3);
            this.ServerGroupBox.Name = "ServerGroupBox";
            this.ServerGroupBox.Size = new System.Drawing.Size(341, 444);
            this.ServerGroupBox.TabIndex = 1;
            this.ServerGroupBox.TabStop = false;
            this.ServerGroupBox.Text = "Server";
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.AutoSize = true;
            this.tableLayoutPanel1.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.tableLayoutPanel1.ColumnCount = 2;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel1.Controls.Add(this.labelIP, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.labelServerPort, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.labelPassword, 0, 2);
            this.tableLayoutPanel1.Controls.Add(this.labelEncryption, 0, 3);
            this.tableLayoutPanel1.Controls.Add(this.labelTCPProtocol, 0, 4);
            this.tableLayoutPanel1.Controls.Add(this.labelProtocolParam, 0, 5);
            this.tableLayoutPanel1.Controls.Add(this.labelObfs, 0, 6);
            this.tableLayoutPanel1.Controls.Add(this.labelObfsParam, 0, 7);
            this.tableLayoutPanel1.Controls.Add(this.labelRemarks, 0, 8);
            this.tableLayoutPanel1.Controls.Add(this.labelGroup, 0, 9);
            this.tableLayoutPanel1.Controls.Add(this.chkSSRLink, 0, 10);
            this.tableLayoutPanel1.Controls.Add(this.chkAdvSetting, 0, 11);
            this.tableLayoutPanel1.Controls.Add(this.labelUDPPort, 0, 12);
            this.tableLayoutPanel1.Controls.Add(this.labelTCPoverUDP, 0, 13);
            this.tableLayoutPanel1.Controls.Add(this.labelUDPoverTCP, 0, 14);
            this.tableLayoutPanel1.Controls.Add(this.labelObfsUDP, 0, 15);
            this.tableLayoutPanel1.Controls.Add(this.txtIP, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this.nudServerPort, 1, 1);
            this.tableLayoutPanel1.Controls.Add(this.txtPassword, 1, 2);
            this.tableLayoutPanel1.Controls.Add(this.cmbEncryption, 1, 3);
            this.tableLayoutPanel1.Controls.Add(this.cmbTcpProtocol, 1, 4);
            this.tableLayoutPanel1.Controls.Add(this.txtProtocolParam, 1, 5);
            this.tableLayoutPanel1.Controls.Add(this.cmbObfs, 1, 6);
            this.tableLayoutPanel1.Controls.Add(this.txtObfsParam, 1, 7);
            this.tableLayoutPanel1.Controls.Add(this.txtRemarks, 1, 8);
            this.tableLayoutPanel1.Controls.Add(this.txtGroup, 1, 9);
            this.tableLayoutPanel1.Controls.Add(this.tbSSRLink, 1, 10);
            this.tableLayoutPanel1.Controls.Add(this.labelNote, 1, 11);
            this.tableLayoutPanel1.Controls.Add(this.nudUdpPort, 1, 12);
            this.tableLayoutPanel1.Controls.Add(this.chkTcpOverUdp, 1, 13);
            this.tableLayoutPanel1.Controls.Add(this.chkUdpOverTcp, 1, 14);
            this.tableLayoutPanel1.Controls.Add(this.CheckObfsUDP, 1, 15);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(3, 16);
            this.tableLayoutPanel1.Margin = new System.Windows.Forms.Padding(0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.Padding = new System.Windows.Forms.Padding(3);
            this.tableLayoutPanel1.RowCount = 16;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.Size = new System.Drawing.Size(335, 425);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // labelIP
            // 
            this.labelIP.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.labelIP.AutoSize = true;
            this.labelIP.Location = new System.Drawing.Point(20, 7);
            this.labelIP.Name = "labelIP";
            this.labelIP.Size = new System.Drawing.Size(70, 17);
            this.labelIP.TabIndex = 0;
            this.labelIP.Text = "Server IP";
            this.labelIP.UseVisualStyleBackColor = true;
            this.labelIP.CheckedChanged += new System.EventHandler(this.IPLabel_CheckedChanged);
            // 
            // labelServerPort
            // 
            this.labelServerPort.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.labelServerPort.AutoSize = true;
            this.labelServerPort.Location = new System.Drawing.Point(30, 35);
            this.labelServerPort.Name = "labelServerPort";
            this.labelServerPort.Size = new System.Drawing.Size(60, 13);
            this.labelServerPort.TabIndex = 8;
            this.labelServerPort.Text = "Server Port";
            // 
            // labelPassword
            // 
            this.labelPassword.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.labelPassword.AutoSize = true;
            this.labelPassword.Location = new System.Drawing.Point(18, 59);
            this.labelPassword.Name = "labelPassword";
            this.labelPassword.Size = new System.Drawing.Size(72, 17);
            this.labelPassword.TabIndex = 3;
            this.labelPassword.Text = "Password";
            this.labelPassword.UseVisualStyleBackColor = true;
            this.labelPassword.CheckedChanged += new System.EventHandler(this.PasswordLabel_CheckedChanged);
            // 
            // labelEncryption
            // 
            this.labelEncryption.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.labelEncryption.AutoSize = true;
            this.labelEncryption.Location = new System.Drawing.Point(33, 90);
            this.labelEncryption.Name = "labelEncryption";
            this.labelEncryption.Size = new System.Drawing.Size(57, 13);
            this.labelEncryption.TabIndex = 12;
            this.labelEncryption.Text = "Encryption";
            // 
            // labelTCPProtocol
            // 
            this.labelTCPProtocol.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.labelTCPProtocol.AutoSize = true;
            this.labelTCPProtocol.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(0)))), ((int)(((byte)(192)))));
            this.labelTCPProtocol.Location = new System.Drawing.Point(44, 121);
            this.labelTCPProtocol.Margin = new System.Windows.Forms.Padding(3, 5, 3, 5);
            this.labelTCPProtocol.Name = "labelTCPProtocol";
            this.labelTCPProtocol.Size = new System.Drawing.Size(46, 13);
            this.labelTCPProtocol.TabIndex = 14;
            this.labelTCPProtocol.Text = "Protocol";
            // 
            // labelProtocolParam
            // 
            this.labelProtocolParam.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.labelProtocolParam.AutoSize = true;
            this.labelProtocolParam.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(0)))), ((int)(((byte)(192)))));
            this.labelProtocolParam.Location = new System.Drawing.Point(12, 149);
            this.labelProtocolParam.Name = "labelProtocolParam";
            this.labelProtocolParam.Size = new System.Drawing.Size(78, 13);
            this.labelProtocolParam.TabIndex = 16;
            this.labelProtocolParam.Text = "Protocol param";
            // 
            // labelObfs
            // 
            this.labelObfs.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.labelObfs.AutoSize = true;
            this.labelObfs.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(0)))), ((int)(((byte)(192)))));
            this.labelObfs.Location = new System.Drawing.Point(61, 178);
            this.labelObfs.Name = "labelObfs";
            this.labelObfs.Size = new System.Drawing.Size(29, 13);
            this.labelObfs.TabIndex = 18;
            this.labelObfs.Text = "Obfs";
            // 
            // labelObfsParam
            // 
            this.labelObfsParam.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.labelObfsParam.AutoSize = true;
            this.labelObfsParam.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(0)))), ((int)(((byte)(192)))));
            this.labelObfsParam.Location = new System.Drawing.Point(29, 206);
            this.labelObfsParam.Name = "labelObfsParam";
            this.labelObfsParam.Size = new System.Drawing.Size(61, 13);
            this.labelObfsParam.TabIndex = 20;
            this.labelObfsParam.Text = "Obfs param";
            // 
            // labelRemarks
            // 
            this.labelRemarks.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.labelRemarks.AutoSize = true;
            this.labelRemarks.Location = new System.Drawing.Point(41, 232);
            this.labelRemarks.Name = "labelRemarks";
            this.labelRemarks.Size = new System.Drawing.Size(49, 13);
            this.labelRemarks.TabIndex = 22;
            this.labelRemarks.Text = "Remarks";
            // 
            // labelGroup
            // 
            this.labelGroup.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.labelGroup.AutoSize = true;
            this.labelGroup.Location = new System.Drawing.Point(54, 258);
            this.labelGroup.Name = "labelGroup";
            this.labelGroup.Size = new System.Drawing.Size(36, 13);
            this.labelGroup.TabIndex = 24;
            this.labelGroup.Text = "Group";
            // 
            // chkSSRLink
            // 
            this.chkSSRLink.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.chkSSRLink.AutoSize = true;
            this.chkSSRLink.Checked = true;
            this.chkSSRLink.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkSSRLink.Location = new System.Drawing.Point(19, 282);
            this.chkSSRLink.Name = "chkSSRLink";
            this.chkSSRLink.Size = new System.Drawing.Size(71, 17);
            this.chkSSRLink.TabIndex = 12;
            this.chkSSRLink.Text = "SSR Link";
            this.chkSSRLink.UseVisualStyleBackColor = true;
            this.chkSSRLink.CheckedChanged += new System.EventHandler(this.checkSSRLink_CheckedChanged);
            // 
            // chkAdvSetting
            // 
            this.chkAdvSetting.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.chkAdvSetting.AutoSize = true;
            this.chkAdvSetting.Location = new System.Drawing.Point(6, 307);
            this.chkAdvSetting.Name = "chkAdvSetting";
            this.chkAdvSetting.Size = new System.Drawing.Size(84, 17);
            this.chkAdvSetting.TabIndex = 14;
            this.chkAdvSetting.Text = "Adv. Setting";
            this.chkAdvSetting.UseVisualStyleBackColor = true;
            this.chkAdvSetting.CheckedChanged += new System.EventHandler(this.checkAdvSetting_CheckedChanged);
            // 
            // labelUDPPort
            // 
            this.labelUDPPort.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.labelUDPPort.AutoSize = true;
            this.labelUDPPort.Location = new System.Drawing.Point(38, 333);
            this.labelUDPPort.Margin = new System.Windows.Forms.Padding(3, 5, 3, 5);
            this.labelUDPPort.Name = "labelUDPPort";
            this.labelUDPPort.Size = new System.Drawing.Size(52, 13);
            this.labelUDPPort.TabIndex = 30;
            this.labelUDPPort.Text = "UDP Port";
            this.labelUDPPort.Visible = false;
            // 
            // labelTCPoverUDP
            // 
            this.labelTCPoverUDP.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.labelTCPoverUDP.AutoSize = true;
            this.labelTCPoverUDP.Location = new System.Drawing.Point(12, 358);
            this.labelTCPoverUDP.Margin = new System.Windows.Forms.Padding(3, 5, 3, 5);
            this.labelTCPoverUDP.Name = "labelTCPoverUDP";
            this.labelTCPoverUDP.Size = new System.Drawing.Size(78, 13);
            this.labelTCPoverUDP.TabIndex = 32;
            this.labelTCPoverUDP.Text = "TCP over UDP";
            this.labelTCPoverUDP.Visible = false;
            // 
            // labelUDPoverTCP
            // 
            this.labelUDPoverTCP.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.labelUDPoverTCP.AutoSize = true;
            this.labelUDPoverTCP.Location = new System.Drawing.Point(12, 381);
            this.labelUDPoverTCP.Margin = new System.Windows.Forms.Padding(3, 5, 3, 5);
            this.labelUDPoverTCP.Name = "labelUDPoverTCP";
            this.labelUDPoverTCP.Size = new System.Drawing.Size(78, 13);
            this.labelUDPoverTCP.TabIndex = 34;
            this.labelUDPoverTCP.Text = "UDP over TCP";
            this.labelUDPoverTCP.Visible = false;
            // 
            // labelObfsUDP
            // 
            this.labelObfsUDP.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.labelObfsUDP.AutoSize = true;
            this.labelObfsUDP.Location = new System.Drawing.Point(35, 404);
            this.labelObfsUDP.Margin = new System.Windows.Forms.Padding(3, 5, 3, 5);
            this.labelObfsUDP.Name = "labelObfsUDP";
            this.labelObfsUDP.Size = new System.Drawing.Size(55, 13);
            this.labelObfsUDP.TabIndex = 36;
            this.labelObfsUDP.Text = "Obfs UDP";
            this.labelObfsUDP.Visible = false;
            // 
            // txtIP
            // 
            this.txtIP.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.txtIP.Location = new System.Drawing.Point(96, 6);
            this.txtIP.MaxLength = 512;
            this.txtIP.Name = "txtIP";
            this.txtIP.Size = new System.Drawing.Size(233, 20);
            this.txtIP.TabIndex = 1;
            this.txtIP.UseSystemPasswordChar = true;
            this.txtIP.WordWrap = false;
            // 
            // nudServerPort
            // 
            this.nudServerPort.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.nudServerPort.Location = new System.Drawing.Point(96, 32);
            this.nudServerPort.Maximum = new decimal(new int[] {
            65535,
            0,
            0,
            0});
            this.nudServerPort.Name = "nudServerPort";
            this.nudServerPort.Size = new System.Drawing.Size(233, 20);
            this.nudServerPort.TabIndex = 2;
            // 
            // txtPassword
            // 
            this.txtPassword.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.txtPassword.Location = new System.Drawing.Point(96, 58);
            this.txtPassword.MaxLength = 256;
            this.txtPassword.Name = "txtPassword";
            this.txtPassword.Size = new System.Drawing.Size(233, 20);
            this.txtPassword.TabIndex = 4;
            this.txtPassword.UseSystemPasswordChar = true;
            this.txtPassword.WordWrap = false;
            // 
            // cmbEncryption
            // 
            this.cmbEncryption.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.cmbEncryption.FormattingEnabled = true;
            this.cmbEncryption.Location = new System.Drawing.Point(96, 84);
            this.cmbEncryption.Margin = new System.Windows.Forms.Padding(3, 3, 3, 7);
            this.cmbEncryption.Name = "cmbEncryption";
            this.cmbEncryption.Size = new System.Drawing.Size(233, 21);
            this.cmbEncryption.TabIndex = 5;
            // 
            // cmbTcpProtocol
            // 
            this.cmbTcpProtocol.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.cmbTcpProtocol.FormattingEnabled = true;
            this.cmbTcpProtocol.Items.AddRange(new object[] {
            "origin",
            "verify_deflate",
            "auth_sha1_v4",
            "auth_aes128_md5",
            "auth_aes128_sha1",
            "auth_chain_a",
            "auth_chain_b",
            "auth_chain_c",
            "auth_chain_d",
            "auth_chain_e",
            "auth_chain_f",
            "auth_akarin_rand",
            "auth_akarin_spec_a"});
            this.cmbTcpProtocol.Location = new System.Drawing.Point(96, 115);
            this.cmbTcpProtocol.Margin = new System.Windows.Forms.Padding(3, 3, 3, 7);
            this.cmbTcpProtocol.Name = "cmbTcpProtocol";
            this.cmbTcpProtocol.Size = new System.Drawing.Size(233, 21);
            this.cmbTcpProtocol.TabIndex = 6;
            // 
            // txtProtocolParam
            // 
            this.txtProtocolParam.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.txtProtocolParam.Location = new System.Drawing.Point(96, 146);
            this.txtProtocolParam.Name = "txtProtocolParam";
            this.txtProtocolParam.Size = new System.Drawing.Size(233, 20);
            this.txtProtocolParam.TabIndex = 7;
            // 
            // cmbObfs
            // 
            this.cmbObfs.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.cmbObfs.FormattingEnabled = true;
            this.cmbObfs.Items.AddRange(new object[] {
            "plain",
            "http_simple",
            "http_post",
            "random_head",
            "tls1.2_ticket_auth",
            "tls1.2_ticket_fastauth"});
            this.cmbObfs.Location = new System.Drawing.Point(96, 172);
            this.cmbObfs.Margin = new System.Windows.Forms.Padding(3, 3, 3, 7);
            this.cmbObfs.Name = "cmbObfs";
            this.cmbObfs.Size = new System.Drawing.Size(233, 21);
            this.cmbObfs.TabIndex = 8;
            this.cmbObfs.TextChanged += new System.EventHandler(this.ObfsCombo_TextChanged);
            // 
            // txtObfsParam
            // 
            this.txtObfsParam.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.txtObfsParam.Location = new System.Drawing.Point(96, 203);
            this.txtObfsParam.Name = "txtObfsParam";
            this.txtObfsParam.Size = new System.Drawing.Size(233, 20);
            this.txtObfsParam.TabIndex = 9;
            // 
            // txtRemarks
            // 
            this.txtRemarks.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.txtRemarks.Location = new System.Drawing.Point(96, 229);
            this.txtRemarks.MaxLength = 256;
            this.txtRemarks.Name = "txtRemarks";
            this.txtRemarks.Size = new System.Drawing.Size(233, 20);
            this.txtRemarks.TabIndex = 10;
            this.txtRemarks.WordWrap = false;
            // 
            // txtGroup
            // 
            this.txtGroup.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.txtGroup.Location = new System.Drawing.Point(96, 255);
            this.txtGroup.MaxLength = 64;
            this.txtGroup.Name = "txtGroup";
            this.txtGroup.Size = new System.Drawing.Size(233, 20);
            this.txtGroup.TabIndex = 11;
            this.txtGroup.WordWrap = false;
            // 
            // tbSSRLink
            // 
            this.tbSSRLink.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.tbSSRLink.Location = new System.Drawing.Point(96, 281);
            this.tbSSRLink.Name = "tbSSRLink";
            this.tbSSRLink.ReadOnly = true;
            this.tbSSRLink.Size = new System.Drawing.Size(233, 20);
            this.tbSSRLink.TabIndex = 13;
            this.tbSSRLink.WordWrap = false;
            this.tbSSRLink.MouseClick += new System.Windows.Forms.MouseEventHandler(this.tbSSRLink_MouseClick);
            this.tbSSRLink.Enter += new System.EventHandler(this.tbSSRLink_Enter);
            // 
            // labelNote
            // 
            this.labelNote.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.labelNote.AutoSize = true;
            this.labelNote.Location = new System.Drawing.Point(98, 309);
            this.labelNote.Margin = new System.Windows.Forms.Padding(5);
            this.labelNote.Name = "labelNote";
            this.labelNote.Size = new System.Drawing.Size(149, 13);
            this.labelNote.TabIndex = 29;
            this.labelNote.Text = "NOT all server support belows";
            // 
            // nudUdpPort
            // 
            this.nudUdpPort.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.nudUdpPort.Location = new System.Drawing.Point(96, 330);
            this.nudUdpPort.Maximum = new decimal(new int[] {
            65535,
            0,
            0,
            0});
            this.nudUdpPort.Name = "nudUdpPort";
            this.nudUdpPort.Size = new System.Drawing.Size(233, 20);
            this.nudUdpPort.TabIndex = 15;
            this.nudUdpPort.Visible = false;
            // 
            // chkTcpOverUdp
            // 
            this.chkTcpOverUdp.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.chkTcpOverUdp.AutoSize = true;
            this.chkTcpOverUdp.Location = new System.Drawing.Point(96, 356);
            this.chkTcpOverUdp.Name = "chkTcpOverUdp";
            this.chkTcpOverUdp.Size = new System.Drawing.Size(166, 17);
            this.chkTcpOverUdp.TabIndex = 16;
            this.chkTcpOverUdp.Text = "TCP over TCP if not checked";
            this.chkTcpOverUdp.UseVisualStyleBackColor = true;
            this.chkTcpOverUdp.Visible = false;
            // 
            // chkUdpOverTcp
            // 
            this.chkUdpOverTcp.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.chkUdpOverTcp.AutoSize = true;
            this.chkUdpOverTcp.Location = new System.Drawing.Point(96, 379);
            this.chkUdpOverTcp.Name = "chkUdpOverTcp";
            this.chkUdpOverTcp.Size = new System.Drawing.Size(170, 17);
            this.chkUdpOverTcp.TabIndex = 17;
            this.chkUdpOverTcp.Text = "UDP over UDP if not checked";
            this.chkUdpOverTcp.UseVisualStyleBackColor = true;
            this.chkUdpOverTcp.Visible = false;
            // 
            // CheckObfsUDP
            // 
            this.CheckObfsUDP.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.CheckObfsUDP.AutoSize = true;
            this.CheckObfsUDP.Location = new System.Drawing.Point(96, 402);
            this.CheckObfsUDP.Name = "CheckObfsUDP";
            this.CheckObfsUDP.Size = new System.Drawing.Size(131, 17);
            this.CheckObfsUDP.TabIndex = 18;
            this.CheckObfsUDP.Text = "Recommend checked";
            this.CheckObfsUDP.UseVisualStyleBackColor = true;
            this.CheckObfsUDP.Visible = false;
            // 
            // btnClose
            // 
            this.btnClose.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.btnClose.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.btnClose.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnClose.Location = new System.Drawing.Point(114, 3);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(105, 35);
            this.btnClose.TabIndex = 99;
            this.btnClose.Text = "Close";
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.BtnClose_Click);
            // 
            // btnOK
            // 
            this.btnOK.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.btnOK.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.btnOK.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btnOK.Location = new System.Drawing.Point(3, 3);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(105, 35);
            this.btnOK.TabIndex = 98;
            this.btnOK.Text = "OK";
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Click += new System.EventHandler(this.BtnOK_Click);
            // 
            // tableLayoutPanel3
            // 
            this.tableLayoutPanel3.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.tableLayoutPanel3.AutoSize = true;
            this.tableLayoutPanel3.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.tableLayoutPanel3.ColumnCount = 2;
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel3.Controls.Add(this.lstServers, 0, 0);
            this.tableLayoutPanel3.Controls.Add(this.btnDown, 1, 2);
            this.tableLayoutPanel3.Controls.Add(this.btnAdd, 0, 1);
            this.tableLayoutPanel3.Controls.Add(this.btnDelete, 1, 1);
            this.tableLayoutPanel3.Controls.Add(this.btnUp, 0, 2);
            this.tableLayoutPanel3.Location = new System.Drawing.Point(3, 3);
            this.tableLayoutPanel3.Name = "tableLayoutPanel3";
            this.tableLayoutPanel3.RowCount = 3;
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel3.Size = new System.Drawing.Size(250, 345);
            this.tableLayoutPanel3.TabIndex = 0;
            // 
            // tableLayoutPanel2
            // 
            this.tableLayoutPanel2.AutoSize = true;
            this.tableLayoutPanel2.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.tableLayoutPanel2.ColumnCount = 3;
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel2.Controls.Add(this.tableLayoutPanel4, 1, 1);
            this.tableLayoutPanel2.Controls.Add(this.tableLayoutPanel3, 0, 0);
            this.tableLayoutPanel2.Controls.Add(this.ServerGroupBox, 1, 0);
            this.tableLayoutPanel2.Controls.Add(this.picQRcode, 2, 0);
            this.tableLayoutPanel2.Controls.Add(this.llbUpdate, 0, 1);
            this.tableLayoutPanel2.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel2.Margin = new System.Windows.Forms.Padding(0);
            this.tableLayoutPanel2.Name = "tableLayoutPanel2";
            this.tableLayoutPanel2.RowCount = 2;
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.tableLayoutPanel2.Size = new System.Drawing.Size(871, 497);
            this.tableLayoutPanel2.TabIndex = 0;
            // 
            // tableLayoutPanel4
            // 
            this.tableLayoutPanel4.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.tableLayoutPanel4.AutoSize = true;
            this.tableLayoutPanel4.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.tableLayoutPanel4.ColumnCount = 3;
            this.tableLayoutPanel4.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel4.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel4.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel4.Controls.Add(this.btnOK, 0, 0);
            this.tableLayoutPanel4.Controls.Add(this.btnApply, 2, 0);
            this.tableLayoutPanel4.Controls.Add(this.btnClose, 1, 0);
            this.tableLayoutPanel4.Location = new System.Drawing.Point(263, 453);
            this.tableLayoutPanel4.Name = "tableLayoutPanel4";
            this.tableLayoutPanel4.RowCount = 1;
            this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel4.Size = new System.Drawing.Size(333, 41);
            this.tableLayoutPanel4.TabIndex = 3;
            // 
            // btnApply
            // 
            this.btnApply.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.btnApply.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.btnApply.Location = new System.Drawing.Point(225, 3);
            this.btnApply.Name = "btnApply";
            this.btnApply.Size = new System.Drawing.Size(105, 35);
            this.btnApply.TabIndex = 100;
            this.btnApply.Text = "Apply";
            this.btnApply.UseVisualStyleBackColor = true;
            this.btnApply.Click += new System.EventHandler(this.BtnApply_Click);
            // 
            // timer_btnLongPress
            // 
            this.timer_btnLongPress.Interval = 50;
            this.timer_btnLongPress.Tick += new System.EventHandler(this.timer_btnLongPress_Tick);
            // 
            // ConfigForm
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.AutoSize = true;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.BackColor = System.Drawing.SystemColors.Control;
            this.CancelButton = this.btnClose;
            this.ClientSize = new System.Drawing.Size(1916, 805);
            this.Controls.Add(this.tableLayoutPanel2);
            this.Controls.Add(this.panel2);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "ConfigForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "0";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.ConfigForm_FormClosed);
            ((System.ComponentModel.ISupportInitialize)(this.picQRcode)).EndInit();
            this.ServerGroupBox.ResumeLayout(false);
            this.ServerGroupBox.PerformLayout();
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudServerPort)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudUdpPort)).EndInit();
            this.tableLayoutPanel3.ResumeLayout(false);
            this.tableLayoutPanel2.ResumeLayout(false);
            this.tableLayoutPanel2.PerformLayout();
            this.tableLayoutPanel4.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Button btnDelete;
        private System.Windows.Forms.Button btnAdd;
        private System.Windows.Forms.ListBox lstServers;
        private System.Windows.Forms.Button btnDown;
        private System.Windows.Forms.Button btnUp;
        private System.Windows.Forms.PictureBox picQRcode;
        private System.Windows.Forms.LinkLabel llbUpdate;
        private System.Windows.Forms.GroupBox ServerGroupBox;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.ComboBox cmbObfs;
        private System.Windows.Forms.Label labelObfs;
        private System.Windows.Forms.Label labelServerPort;
        private System.Windows.Forms.TextBox txtIP;
        private System.Windows.Forms.NumericUpDown nudServerPort;
        private System.Windows.Forms.TextBox txtPassword;
        private System.Windows.Forms.Label labelEncryption;
        private System.Windows.Forms.ComboBox cmbEncryption;
        private System.Windows.Forms.TextBox tbSSRLink;
        private System.Windows.Forms.TextBox txtRemarks;
        private System.Windows.Forms.Label labelObfsUDP;
        private System.Windows.Forms.CheckBox CheckObfsUDP;
        private System.Windows.Forms.Label labelTCPProtocol;
        private System.Windows.Forms.Label labelUDPoverTCP;
        private System.Windows.Forms.CheckBox chkUdpOverTcp;
        private System.Windows.Forms.Label labelNote;
        private System.Windows.Forms.CheckBox labelPassword;
        private System.Windows.Forms.Label labelTCPoverUDP;
        private System.Windows.Forms.CheckBox chkTcpOverUdp;
        private System.Windows.Forms.ComboBox cmbTcpProtocol;
        private System.Windows.Forms.Label labelObfsParam;
        private System.Windows.Forms.TextBox txtObfsParam;
        private System.Windows.Forms.Label labelGroup;
        private System.Windows.Forms.TextBox txtGroup;
        private System.Windows.Forms.CheckBox chkAdvSetting;
        private System.Windows.Forms.Label labelUDPPort;
        private System.Windows.Forms.NumericUpDown nudUdpPort;
        private System.Windows.Forms.CheckBox chkSSRLink;
        private System.Windows.Forms.Label labelRemarks;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.Label labelProtocolParam;
        private System.Windows.Forms.TextBox txtProtocolParam;
        private System.Windows.Forms.CheckBox labelIP;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel3;
        private Model.MyFormsTimer timer_btnLongPress;
        private System.Windows.Forms.Button btnApply;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel4;
    }
}

