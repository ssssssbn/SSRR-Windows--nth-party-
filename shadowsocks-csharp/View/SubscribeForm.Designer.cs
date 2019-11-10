namespace Shadowsocks.View
{
    partial class SubscribeForm
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
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.chkBJoinUpdate = new System.Windows.Forms.CheckBox();
            this.labelURL = new System.Windows.Forms.Label();
            this.labelGroupName = new System.Windows.Forms.Label();
            this.tBURL = new System.Windows.Forms.TextBox();
            this.tBGroup = new System.Windows.Forms.TextBox();
            this.labelLastUpdate = new System.Windows.Forms.Label();
            this.tBUpdate = new System.Windows.Forms.TextBox();
            this.labelDontUseProxy = new System.Windows.Forms.Label();
            this.chkBDontUseProxy = new System.Windows.Forms.CheckBox();
            this.chkBUseProxy = new System.Windows.Forms.CheckBox();
            this.labelUseProxy = new System.Windows.Forms.Label();
            this.labelJoinUpdate = new System.Windows.Forms.Label();
            this.checkBoxAutoUpdate = new System.Windows.Forms.CheckBox();
            this.btnApply = new System.Windows.Forms.Button();
            this.btnOK = new System.Windows.Forms.Button();
            this.btnClose = new System.Windows.Forms.Button();
            this.btnUp = new System.Windows.Forms.Button();
            this.btnDown = new System.Windows.Forms.Button();
            this.btnDeleteIncludeServers = new System.Windows.Forms.Button();
            this.btnAdd = new System.Windows.Forms.Button();
            this.btnDelete = new System.Windows.Forms.Button();
            this.chkBSortServersBySubscriptionsOrder = new System.Windows.Forms.CheckBox();
            this.lblevery2 = new System.Windows.Forms.Label();
            this.lblhours2 = new System.Windows.Forms.Label();
            this.cmbAutoLatencyInterval = new System.Windows.Forms.ComboBox();
            this.lblevery1 = new System.Windows.Forms.Label();
            this.lblhours1 = new System.Windows.Forms.Label();
            this.cmbAutoUpdateInterval = new System.Windows.Forms.ComboBox();
            this.checkBoxAutoUpdateUseProxy = new System.Windows.Forms.CheckBox();
            this.checkBoxAutoLatency = new System.Windows.Forms.CheckBox();
            this.checkBoxAutoUpdateTryUseProxy = new System.Windows.Forms.CheckBox();
            this.listServerSubscribe = new System.Windows.Forms.ListBox();
            this.cMSlistServerSubscribe = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.UpdateSelectedToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.UpdateSelectedUseProxyToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.tableLayoutPanel3 = new System.Windows.Forms.TableLayoutPanel();
            this.tableLayoutPanel6 = new System.Windows.Forms.TableLayoutPanel();
            this.GroupSubscribeDetail = new System.Windows.Forms.GroupBox();
            this.timer_btnLongPress = new Shadowsocks.Model.MyFormsTimer(this.components);
            this.tableLayoutPanel1.SuspendLayout();
            this.cMSlistServerSubscribe.SuspendLayout();
            this.tableLayoutPanel3.SuspendLayout();
            this.tableLayoutPanel6.SuspendLayout();
            this.GroupSubscribeDetail.SuspendLayout();
            this.SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.AutoSize = true;
            this.tableLayoutPanel1.ColumnCount = 2;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel1.Controls.Add(this.chkBJoinUpdate, 1, 3);
            this.tableLayoutPanel1.Controls.Add(this.labelURL, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.labelGroupName, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.tBURL, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this.tBGroup, 1, 1);
            this.tableLayoutPanel1.Controls.Add(this.labelLastUpdate, 0, 2);
            this.tableLayoutPanel1.Controls.Add(this.tBUpdate, 1, 2);
            this.tableLayoutPanel1.Controls.Add(this.labelDontUseProxy, 0, 5);
            this.tableLayoutPanel1.Controls.Add(this.chkBDontUseProxy, 1, 5);
            this.tableLayoutPanel1.Controls.Add(this.chkBUseProxy, 1, 4);
            this.tableLayoutPanel1.Controls.Add(this.labelUseProxy, 0, 4);
            this.tableLayoutPanel1.Controls.Add(this.labelJoinUpdate, 0, 3);
            this.tableLayoutPanel1.Location = new System.Drawing.Point(6, 19);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 6;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.Size = new System.Drawing.Size(276, 138);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // chkBJoinUpdate
            // 
            this.chkBJoinUpdate.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.chkBJoinUpdate.AutoSize = true;
            this.chkBJoinUpdate.Location = new System.Drawing.Point(87, 81);
            this.chkBJoinUpdate.Name = "chkBJoinUpdate";
            this.chkBJoinUpdate.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.chkBJoinUpdate.Size = new System.Drawing.Size(15, 14);
            this.chkBJoinUpdate.TabIndex = 2;
            this.chkBJoinUpdate.UseVisualStyleBackColor = true;
            this.chkBJoinUpdate.CheckedChanged += new System.EventHandler(this.CheckBox_CheckedChanged);
            // 
            // labelURL
            // 
            this.labelURL.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.labelURL.AutoSize = true;
            this.labelURL.Location = new System.Drawing.Point(52, 6);
            this.labelURL.Name = "labelURL";
            this.labelURL.Size = new System.Drawing.Size(29, 13);
            this.labelURL.TabIndex = 0;
            this.labelURL.Text = "URL";
            // 
            // labelGroupName
            // 
            this.labelGroupName.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.labelGroupName.AutoSize = true;
            this.labelGroupName.Location = new System.Drawing.Point(16, 32);
            this.labelGroupName.Name = "labelGroupName";
            this.labelGroupName.Size = new System.Drawing.Size(65, 13);
            this.labelGroupName.TabIndex = 2;
            this.labelGroupName.Text = "Group name";
            // 
            // tBURL
            // 
            this.tBURL.Location = new System.Drawing.Point(87, 3);
            this.tBURL.Name = "tBURL";
            this.tBURL.Size = new System.Drawing.Size(186, 20);
            this.tBURL.TabIndex = 0;
            this.tBURL.TextChanged += new System.EventHandler(this.TBURL_TextChanged);
            // 
            // tBGroup
            // 
            this.tBGroup.Location = new System.Drawing.Point(87, 29);
            this.tBGroup.Name = "tBGroup";
            this.tBGroup.Size = new System.Drawing.Size(186, 20);
            this.tBGroup.TabIndex = 1;
            this.tBGroup.TextChanged += new System.EventHandler(this.TBGroup_TextChanged);
            this.tBGroup.Leave += new System.EventHandler(this.TBGroup_Leave);
            // 
            // labelLastUpdate
            // 
            this.labelLastUpdate.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.labelLastUpdate.AutoSize = true;
            this.labelLastUpdate.Location = new System.Drawing.Point(16, 58);
            this.labelLastUpdate.Name = "labelLastUpdate";
            this.labelLastUpdate.Size = new System.Drawing.Size(65, 13);
            this.labelLastUpdate.TabIndex = 4;
            this.labelLastUpdate.Text = "Last Update";
            // 
            // tBUpdate
            // 
            this.tBUpdate.Location = new System.Drawing.Point(87, 55);
            this.tBUpdate.Name = "tBUpdate";
            this.tBUpdate.ReadOnly = true;
            this.tBUpdate.Size = new System.Drawing.Size(186, 20);
            this.tBUpdate.TabIndex = 5;
            // 
            // labelDontUseProxy
            // 
            this.labelDontUseProxy.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.labelDontUseProxy.AutoSize = true;
            this.labelDontUseProxy.Location = new System.Drawing.Point(3, 121);
            this.labelDontUseProxy.Name = "labelDontUseProxy";
            this.labelDontUseProxy.Size = new System.Drawing.Size(78, 13);
            this.labelDontUseProxy.TabIndex = 8;
            this.labelDontUseProxy.Text = "Dont use proxy";
            // 
            // chkBDontUseProxy
            // 
            this.chkBDontUseProxy.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.chkBDontUseProxy.AutoSize = true;
            this.chkBDontUseProxy.Location = new System.Drawing.Point(87, 121);
            this.chkBDontUseProxy.Name = "chkBDontUseProxy";
            this.chkBDontUseProxy.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.chkBDontUseProxy.Size = new System.Drawing.Size(15, 14);
            this.chkBDontUseProxy.TabIndex = 4;
            this.chkBDontUseProxy.UseVisualStyleBackColor = true;
            this.chkBDontUseProxy.CheckedChanged += new System.EventHandler(this.CheckBox_CheckedChanged);
            // 
            // chkBUseProxy
            // 
            this.chkBUseProxy.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.chkBUseProxy.AutoSize = true;
            this.chkBUseProxy.Location = new System.Drawing.Point(87, 101);
            this.chkBUseProxy.Name = "chkBUseProxy";
            this.chkBUseProxy.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.chkBUseProxy.Size = new System.Drawing.Size(15, 14);
            this.chkBUseProxy.TabIndex = 3;
            this.chkBUseProxy.UseVisualStyleBackColor = true;
            this.chkBUseProxy.CheckedChanged += new System.EventHandler(this.CheckBox_CheckedChanged);
            // 
            // labelUseProxy
            // 
            this.labelUseProxy.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.labelUseProxy.AutoSize = true;
            this.labelUseProxy.Location = new System.Drawing.Point(27, 101);
            this.labelUseProxy.Name = "labelUseProxy";
            this.labelUseProxy.Size = new System.Drawing.Size(54, 13);
            this.labelUseProxy.TabIndex = 7;
            this.labelUseProxy.Text = "Use proxy";
            // 
            // labelJoinUpdate
            // 
            this.labelJoinUpdate.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.labelJoinUpdate.AutoSize = true;
            this.labelJoinUpdate.Location = new System.Drawing.Point(17, 81);
            this.labelJoinUpdate.Name = "labelJoinUpdate";
            this.labelJoinUpdate.Size = new System.Drawing.Size(64, 13);
            this.labelJoinUpdate.TabIndex = 8;
            this.labelJoinUpdate.Text = "Join Update";
            // 
            // checkBoxAutoUpdate
            // 
            this.checkBoxAutoUpdate.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.checkBoxAutoUpdate.AutoSize = true;
            this.checkBoxAutoUpdate.Location = new System.Drawing.Point(3, 242);
            this.checkBoxAutoUpdate.Name = "checkBoxAutoUpdate";
            this.checkBoxAutoUpdate.Size = new System.Drawing.Size(84, 17);
            this.checkBoxAutoUpdate.TabIndex = 3;
            this.checkBoxAutoUpdate.Text = "Auto update";
            this.checkBoxAutoUpdate.UseVisualStyleBackColor = true;
            this.checkBoxAutoUpdate.CheckedChanged += new System.EventHandler(this.CheckBox_CheckedChanged);
            // 
            // btnApply
            // 
            this.btnApply.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.btnApply.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.btnApply.Enabled = false;
            this.btnApply.Location = new System.Drawing.Point(668, 238);
            this.btnApply.Name = "btnApply";
            this.tableLayoutPanel3.SetRowSpan(this.btnApply, 4);
            this.btnApply.Size = new System.Drawing.Size(105, 35);
            this.btnApply.TabIndex = 12;
            this.btnApply.Text = "Apply";
            this.btnApply.UseVisualStyleBackColor = true;
            this.btnApply.Click += new System.EventHandler(this.BtnApply_Click);
            // 
            // btnOK
            // 
            this.btnOK.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.btnOK.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.btnOK.Location = new System.Drawing.Point(446, 238);
            this.btnOK.Name = "btnOK";
            this.tableLayoutPanel3.SetRowSpan(this.btnOK, 4);
            this.btnOK.Size = new System.Drawing.Size(105, 35);
            this.btnOK.TabIndex = 10;
            this.btnOK.Text = "OK";
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Click += new System.EventHandler(this.BtnOK_Click);
            // 
            // btnClose
            // 
            this.btnClose.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.btnClose.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.btnClose.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnClose.Location = new System.Drawing.Point(557, 238);
            this.btnClose.Name = "btnClose";
            this.tableLayoutPanel3.SetRowSpan(this.btnClose, 4);
            this.btnClose.Size = new System.Drawing.Size(105, 35);
            this.btnClose.TabIndex = 11;
            this.btnClose.Text = "Close";
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.BtnClose_Click);
            // 
            // btnUp
            // 
            this.btnUp.AutoSize = true;
            this.btnUp.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.btnUp.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnUp.Location = new System.Drawing.Point(3, 3);
            this.btnUp.Name = "btnUp";
            this.btnUp.Size = new System.Drawing.Size(48, 23);
            this.btnUp.TabIndex = 0;
            this.btnUp.Text = "Up";
            this.btnUp.UseVisualStyleBackColor = true;
            this.btnUp.Click += new System.EventHandler(this.BtnUp_Click);
            this.btnUp.MouseDown += new System.Windows.Forms.MouseEventHandler(this.Btn_MouseDown);
            this.btnUp.MouseUp += new System.Windows.Forms.MouseEventHandler(this.Btn_MouseUp);
            // 
            // btnDown
            // 
            this.btnDown.AutoSize = true;
            this.btnDown.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.btnDown.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnDown.Location = new System.Drawing.Point(57, 3);
            this.btnDown.Name = "btnDown";
            this.btnDown.Size = new System.Drawing.Size(48, 23);
            this.btnDown.TabIndex = 1;
            this.btnDown.Text = "Down";
            this.btnDown.UseVisualStyleBackColor = true;
            this.btnDown.Click += new System.EventHandler(this.BtnDown_Click);
            this.btnDown.MouseDown += new System.Windows.Forms.MouseEventHandler(this.Btn_MouseDown);
            this.btnDown.MouseUp += new System.Windows.Forms.MouseEventHandler(this.Btn_MouseUp);
            // 
            // btnDeleteIncludeServers
            // 
            this.btnDeleteIncludeServers.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.btnDeleteIncludeServers.AutoSize = true;
            this.btnDeleteIncludeServers.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.btnDeleteIncludeServers.Location = new System.Drawing.Point(219, 3);
            this.btnDeleteIncludeServers.Name = "btnDeleteIncludeServers";
            this.btnDeleteIncludeServers.Size = new System.Drawing.Size(125, 23);
            this.btnDeleteIncludeServers.TabIndex = 4;
            this.btnDeleteIncludeServers.Text = "Delete(IncludeServers)";
            this.btnDeleteIncludeServers.UseVisualStyleBackColor = true;
            this.btnDeleteIncludeServers.Click += new System.EventHandler(this.BtnDeleteIncludeNode_Click);
            this.btnDeleteIncludeServers.MouseDown += new System.Windows.Forms.MouseEventHandler(this.Btn_MouseDown);
            this.btnDeleteIncludeServers.MouseUp += new System.Windows.Forms.MouseEventHandler(this.Btn_MouseUp);
            // 
            // btnAdd
            // 
            this.btnAdd.AutoSize = true;
            this.btnAdd.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.btnAdd.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnAdd.Location = new System.Drawing.Point(111, 3);
            this.btnAdd.Name = "btnAdd";
            this.btnAdd.Size = new System.Drawing.Size(48, 23);
            this.btnAdd.TabIndex = 2;
            this.btnAdd.Text = "&Add";
            this.btnAdd.UseVisualStyleBackColor = true;
            this.btnAdd.Click += new System.EventHandler(this.BtnAdd_Click);
            // 
            // btnDelete
            // 
            this.btnDelete.AutoSize = true;
            this.btnDelete.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.btnDelete.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnDelete.Location = new System.Drawing.Point(165, 3);
            this.btnDelete.Name = "btnDelete";
            this.btnDelete.Size = new System.Drawing.Size(48, 23);
            this.btnDelete.TabIndex = 3;
            this.btnDelete.Text = "&Delete";
            this.btnDelete.UseVisualStyleBackColor = true;
            this.btnDelete.Click += new System.EventHandler(this.BtnDelete_Click);
            this.btnDelete.MouseDown += new System.Windows.Forms.MouseEventHandler(this.Btn_MouseDown);
            this.btnDelete.MouseUp += new System.Windows.Forms.MouseEventHandler(this.Btn_MouseUp);
            // 
            // chkBSortServersBySubscriptionsOrder
            // 
            this.chkBSortServersBySubscriptionsOrder.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.chkBSortServersBySubscriptionsOrder.AutoSize = true;
            this.tableLayoutPanel3.SetColumnSpan(this.chkBSortServersBySubscriptionsOrder, 8);
            this.chkBSortServersBySubscriptionsOrder.Location = new System.Drawing.Point(3, 290);
            this.chkBSortServersBySubscriptionsOrder.Name = "chkBSortServersBySubscriptionsOrder";
            this.chkBSortServersBySubscriptionsOrder.Size = new System.Drawing.Size(182, 17);
            this.chkBSortServersBySubscriptionsOrder.TabIndex = 9;
            this.chkBSortServersBySubscriptionsOrder.Text = "Sort servers by subscription order";
            this.chkBSortServersBySubscriptionsOrder.UseVisualStyleBackColor = true;
            this.chkBSortServersBySubscriptionsOrder.CheckedChanged += new System.EventHandler(this.CheckBox_CheckedChanged);
            // 
            // lblevery2
            // 
            this.lblevery2.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.lblevery2.AutoSize = true;
            this.lblevery2.Location = new System.Drawing.Point(315, 244);
            this.lblevery2.Name = "lblevery2";
            this.lblevery2.Size = new System.Drawing.Size(33, 13);
            this.lblevery2.TabIndex = 0;
            this.lblevery2.Text = "every";
            // 
            // lblhours2
            // 
            this.lblhours2.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.lblhours2.AutoSize = true;
            this.lblhours2.Location = new System.Drawing.Point(401, 244);
            this.lblhours2.Name = "lblhours2";
            this.lblhours2.Size = new System.Drawing.Size(39, 13);
            this.lblhours2.TabIndex = 1;
            this.lblhours2.Text = "hour(s)";
            // 
            // cmbAutoLatencyInterval
            // 
            this.cmbAutoLatencyInterval.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.cmbAutoLatencyInterval.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbAutoLatencyInterval.FormattingEnabled = true;
            this.cmbAutoLatencyInterval.Items.AddRange(new object[] {
            "0.5",
            "1",
            "2",
            "3",
            "6",
            "12",
            "24"});
            this.cmbAutoLatencyInterval.Location = new System.Drawing.Point(354, 240);
            this.cmbAutoLatencyInterval.Name = "cmbAutoLatencyInterval";
            this.cmbAutoLatencyInterval.Size = new System.Drawing.Size(41, 21);
            this.cmbAutoLatencyInterval.TabIndex = 6;
            this.cmbAutoLatencyInterval.DropDownClosed += new System.EventHandler(this.Cmb_DropDownClosed);
            // 
            // lblevery1
            // 
            this.lblevery1.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.lblevery1.AutoSize = true;
            this.lblevery1.Location = new System.Drawing.Point(93, 244);
            this.lblevery1.Name = "lblevery1";
            this.lblevery1.Size = new System.Drawing.Size(33, 13);
            this.lblevery1.TabIndex = 0;
            this.lblevery1.Text = "every";
            // 
            // lblhours1
            // 
            this.lblhours1.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.lblhours1.AutoSize = true;
            this.lblhours1.Location = new System.Drawing.Point(179, 244);
            this.lblhours1.Name = "lblhours1";
            this.lblhours1.Size = new System.Drawing.Size(39, 13);
            this.lblhours1.TabIndex = 1;
            this.lblhours1.Text = "hour(s)";
            // 
            // cmbAutoUpdateInterval
            // 
            this.cmbAutoUpdateInterval.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.cmbAutoUpdateInterval.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbAutoUpdateInterval.FormattingEnabled = true;
            this.cmbAutoUpdateInterval.Items.AddRange(new object[] {
            "1",
            "3",
            "6",
            "12",
            "24",
            "48",
            "72"});
            this.cmbAutoUpdateInterval.Location = new System.Drawing.Point(132, 240);
            this.cmbAutoUpdateInterval.Name = "cmbAutoUpdateInterval";
            this.cmbAutoUpdateInterval.Size = new System.Drawing.Size(41, 21);
            this.cmbAutoUpdateInterval.TabIndex = 4;
            this.cmbAutoUpdateInterval.DropDownClosed += new System.EventHandler(this.Cmb_DropDownClosed);
            // 
            // checkBoxAutoUpdateUseProxy
            // 
            this.checkBoxAutoUpdateUseProxy.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.checkBoxAutoUpdateUseProxy.AutoSize = true;
            this.tableLayoutPanel3.SetColumnSpan(this.checkBoxAutoUpdateUseProxy, 4);
            this.checkBoxAutoUpdateUseProxy.Location = new System.Drawing.Point(3, 267);
            this.checkBoxAutoUpdateUseProxy.Name = "checkBoxAutoUpdateUseProxy";
            this.checkBoxAutoUpdateUseProxy.Size = new System.Drawing.Size(73, 17);
            this.checkBoxAutoUpdateUseProxy.TabIndex = 7;
            this.checkBoxAutoUpdateUseProxy.Text = "Use proxy";
            this.checkBoxAutoUpdateUseProxy.UseVisualStyleBackColor = true;
            this.checkBoxAutoUpdateUseProxy.CheckedChanged += new System.EventHandler(this.CheckBox_CheckedChanged);
            // 
            // checkBoxAutoLatency
            // 
            this.checkBoxAutoLatency.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.checkBoxAutoLatency.AutoSize = true;
            this.checkBoxAutoLatency.Location = new System.Drawing.Point(224, 242);
            this.checkBoxAutoLatency.Name = "checkBoxAutoLatency";
            this.checkBoxAutoLatency.Size = new System.Drawing.Size(85, 17);
            this.checkBoxAutoLatency.TabIndex = 5;
            this.checkBoxAutoLatency.Text = "Auto latency";
            this.checkBoxAutoLatency.UseVisualStyleBackColor = true;
            this.checkBoxAutoLatency.CheckedChanged += new System.EventHandler(this.CheckBox_CheckedChanged);
            // 
            // checkBoxAutoUpdateTryUseProxy
            // 
            this.checkBoxAutoUpdateTryUseProxy.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.checkBoxAutoUpdateTryUseProxy.AutoSize = true;
            this.tableLayoutPanel3.SetColumnSpan(this.checkBoxAutoUpdateTryUseProxy, 4);
            this.checkBoxAutoUpdateTryUseProxy.Location = new System.Drawing.Point(224, 267);
            this.checkBoxAutoUpdateTryUseProxy.Name = "checkBoxAutoUpdateTryUseProxy";
            this.checkBoxAutoUpdateTryUseProxy.Size = new System.Drawing.Size(89, 17);
            this.checkBoxAutoUpdateTryUseProxy.TabIndex = 8;
            this.checkBoxAutoUpdateTryUseProxy.Text = "Try use proxy";
            this.checkBoxAutoUpdateTryUseProxy.UseVisualStyleBackColor = true;
            this.checkBoxAutoUpdateTryUseProxy.CheckedChanged += new System.EventHandler(this.CheckBox_CheckedChanged);
            // 
            // listServerSubscribe
            // 
            this.listServerSubscribe.AllowDrop = true;
            this.tableLayoutPanel3.SetColumnSpan(this.listServerSubscribe, 8);
            this.listServerSubscribe.Dock = System.Windows.Forms.DockStyle.Fill;
            this.listServerSubscribe.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.listServerSubscribe.FormattingEnabled = true;
            this.listServerSubscribe.HorizontalScrollbar = true;
            this.listServerSubscribe.ItemHeight = 33;
            this.listServerSubscribe.Location = new System.Drawing.Point(0, 0);
            this.listServerSubscribe.Margin = new System.Windows.Forms.Padding(0);
            this.listServerSubscribe.Name = "listServerSubscribe";
            this.listServerSubscribe.SelectionMode = System.Windows.Forms.SelectionMode.MultiExtended;
            this.listServerSubscribe.Size = new System.Drawing.Size(443, 202);
            this.listServerSubscribe.TabIndex = 0;
            this.listServerSubscribe.DrawItem += new System.Windows.Forms.DrawItemEventHandler(this.listServerSubscribe_DrawItem);
            this.listServerSubscribe.SelectedIndexChanged += new System.EventHandler(this.listServerSubscribe_SelectedIndexChanged);
            this.listServerSubscribe.MouseUp += new System.Windows.Forms.MouseEventHandler(this.listServerSubscribe_MouseUp);
            // 
            // cMSlistServerSubscribe
            // 
            this.cMSlistServerSubscribe.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.UpdateSelectedToolStripMenuItem,
            this.UpdateSelectedUseProxyToolStripMenuItem});
            this.cMSlistServerSubscribe.Name = "cMSlistServerSubscribe";
            this.cMSlistServerSubscribe.Size = new System.Drawing.Size(219, 48);
            // 
            // UpdateSelectedToolStripMenuItem
            // 
            this.UpdateSelectedToolStripMenuItem.Name = "UpdateSelectedToolStripMenuItem";
            this.UpdateSelectedToolStripMenuItem.Size = new System.Drawing.Size(218, 22);
            this.UpdateSelectedToolStripMenuItem.Text = "Update Selected";
            this.UpdateSelectedToolStripMenuItem.Click += new System.EventHandler(this.UpdateSelectedToolStripMenuItem_Click);
            // 
            // UpdateSelectedUseProxyToolStripMenuItem
            // 
            this.UpdateSelectedUseProxyToolStripMenuItem.Name = "UpdateSelectedUseProxyToolStripMenuItem";
            this.UpdateSelectedUseProxyToolStripMenuItem.Size = new System.Drawing.Size(218, 22);
            this.UpdateSelectedUseProxyToolStripMenuItem.Text = "Update Selected(Use Proxy)";
            this.UpdateSelectedUseProxyToolStripMenuItem.Click += new System.EventHandler(this.UpdateSelectedUseProxyToolStripMenuItem_Click);
            // 
            // tableLayoutPanel3
            // 
            this.tableLayoutPanel3.AutoSize = true;
            this.tableLayoutPanel3.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.tableLayoutPanel3.ColumnCount = 11;
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel3.Controls.Add(this.btnApply, 10, 1);
            this.tableLayoutPanel3.Controls.Add(this.chkBSortServersBySubscriptionsOrder, 0, 4);
            this.tableLayoutPanel3.Controls.Add(this.btnClose, 9, 1);
            this.tableLayoutPanel3.Controls.Add(this.btnOK, 8, 1);
            this.tableLayoutPanel3.Controls.Add(this.listServerSubscribe, 0, 0);
            this.tableLayoutPanel3.Controls.Add(this.checkBoxAutoUpdateTryUseProxy, 4, 3);
            this.tableLayoutPanel3.Controls.Add(this.checkBoxAutoUpdateUseProxy, 0, 3);
            this.tableLayoutPanel3.Controls.Add(this.tableLayoutPanel6, 0, 1);
            this.tableLayoutPanel3.Controls.Add(this.lblhours2, 7, 2);
            this.tableLayoutPanel3.Controls.Add(this.GroupSubscribeDetail, 8, 0);
            this.tableLayoutPanel3.Controls.Add(this.cmbAutoLatencyInterval, 6, 2);
            this.tableLayoutPanel3.Controls.Add(this.lblevery2, 5, 2);
            this.tableLayoutPanel3.Controls.Add(this.checkBoxAutoUpdate, 0, 2);
            this.tableLayoutPanel3.Controls.Add(this.lblevery1, 1, 2);
            this.tableLayoutPanel3.Controls.Add(this.checkBoxAutoLatency, 4, 2);
            this.tableLayoutPanel3.Controls.Add(this.lblhours1, 3, 2);
            this.tableLayoutPanel3.Controls.Add(this.cmbAutoUpdateInterval, 2, 2);
            this.tableLayoutPanel3.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel3.Margin = new System.Windows.Forms.Padding(0);
            this.tableLayoutPanel3.Name = "tableLayoutPanel3";
            this.tableLayoutPanel3.RowCount = 5;
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel3.Size = new System.Drawing.Size(776, 310);
            this.tableLayoutPanel3.TabIndex = 0;
            // 
            // tableLayoutPanel6
            // 
            this.tableLayoutPanel6.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.tableLayoutPanel6.AutoSize = true;
            this.tableLayoutPanel6.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.tableLayoutPanel6.ColumnCount = 5;
            this.tableLayoutPanel3.SetColumnSpan(this.tableLayoutPanel6, 8);
            this.tableLayoutPanel6.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanel6.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanel6.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanel6.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanel6.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel6.Controls.Add(this.btnDeleteIncludeServers, 4, 0);
            this.tableLayoutPanel6.Controls.Add(this.btnDelete, 3, 0);
            this.tableLayoutPanel6.Controls.Add(this.btnAdd, 2, 0);
            this.tableLayoutPanel6.Controls.Add(this.btnDown, 1, 0);
            this.tableLayoutPanel6.Controls.Add(this.btnUp, 0, 0);
            this.tableLayoutPanel6.Location = new System.Drawing.Point(48, 205);
            this.tableLayoutPanel6.Name = "tableLayoutPanel6";
            this.tableLayoutPanel6.RowCount = 1;
            this.tableLayoutPanel6.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel6.Size = new System.Drawing.Size(347, 29);
            this.tableLayoutPanel6.TabIndex = 2;
            // 
            // GroupSubscribeDetail
            // 
            this.GroupSubscribeDetail.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.GroupSubscribeDetail.AutoSize = true;
            this.GroupSubscribeDetail.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.tableLayoutPanel3.SetColumnSpan(this.GroupSubscribeDetail, 3);
            this.GroupSubscribeDetail.Controls.Add(this.tableLayoutPanel1);
            this.GroupSubscribeDetail.Location = new System.Drawing.Point(465, 3);
            this.GroupSubscribeDetail.Name = "GroupSubscribeDetail";
            this.GroupSubscribeDetail.Size = new System.Drawing.Size(288, 176);
            this.GroupSubscribeDetail.TabIndex = 1;
            this.GroupSubscribeDetail.TabStop = false;
            this.GroupSubscribeDetail.Text = "Subscribe Detail";
            // 
            // timer_btnLongPress
            // 
            this.timer_btnLongPress.Tick += new System.EventHandler(this.timer_btnLongPress_Tick);
            // 
            // SubscribeForm
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.AutoSize = true;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.CancelButton = this.btnClose;
            this.ClientSize = new System.Drawing.Size(983, 598);
            this.Controls.Add(this.tableLayoutPanel3);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.Name = "SubscribeForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Subscribe Settings";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.SubscribeForm_FormClosing);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.cMSlistServerSubscribe.ResumeLayout(false);
            this.tableLayoutPanel3.ResumeLayout(false);
            this.tableLayoutPanel3.PerformLayout();
            this.tableLayoutPanel6.ResumeLayout(false);
            this.tableLayoutPanel6.PerformLayout();
            this.GroupSubscribeDetail.ResumeLayout(false);
            this.GroupSubscribeDetail.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Label labelURL;
        private System.Windows.Forms.Label labelGroupName;
        private System.Windows.Forms.TextBox tBURL;
        private System.Windows.Forms.TextBox tBGroup;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.CheckBox checkBoxAutoUpdate;
        private System.Windows.Forms.CheckBox checkBoxAutoUpdateUseProxy;
        private System.Windows.Forms.ListBox listServerSubscribe;
        private System.Windows.Forms.Button btnAdd;
        private System.Windows.Forms.Button btnDelete;
        private System.Windows.Forms.Label labelLastUpdate;
        private System.Windows.Forms.TextBox tBUpdate;
        private System.Windows.Forms.CheckBox chkBUseProxy;
        private System.Windows.Forms.Label labelUseProxy;
        private System.Windows.Forms.CheckBox checkBoxAutoUpdateTryUseProxy;
        private System.Windows.Forms.CheckBox checkBoxAutoLatency;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel3;
        private System.Windows.Forms.Label lblevery1;
        private System.Windows.Forms.Label lblhours1;
        private System.Windows.Forms.ComboBox cmbAutoUpdateInterval;
        private System.Windows.Forms.Label lblevery2;
        private System.Windows.Forms.Label lblhours2;
        private System.Windows.Forms.ComboBox cmbAutoLatencyInterval;
        private System.Windows.Forms.Button btnApply;
        private System.Windows.Forms.ContextMenuStrip cMSlistServerSubscribe;
        private System.Windows.Forms.ToolStripMenuItem UpdateSelectedToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem UpdateSelectedUseProxyToolStripMenuItem;
        private System.Windows.Forms.CheckBox chkBDontUseProxy;
        private System.Windows.Forms.Label labelDontUseProxy;
        private System.Windows.Forms.Button btnDeleteIncludeServers;
        private System.Windows.Forms.CheckBox chkBJoinUpdate;
        private System.Windows.Forms.Label labelJoinUpdate;
        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.CheckBox chkBSortServersBySubscriptionsOrder;
        private System.Windows.Forms.Button btnUp;
        private System.Windows.Forms.Button btnDown;
        private System.Windows.Forms.GroupBox GroupSubscribeDetail;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel6;
        private Model.MyFormsTimer timer_btnLongPress;
    }
}