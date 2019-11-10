namespace Shadowsocks.View
{
    partial class PortSettingsForm
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
            this.listPorts = new System.Windows.Forms.ListBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
            this.labelType = new System.Windows.Forms.Label();
            this.labelID = new System.Windows.Forms.Label();
            this.labelAddr = new System.Windows.Forms.Label();
            this.labelPort = new System.Windows.Forms.Label();
            this.checkEnable = new System.Windows.Forms.CheckBox();
            this.textAddr = new System.Windows.Forms.TextBox();
            this.NumTargetPort = new System.Windows.Forms.NumericUpDown();
            this.comboBoxType = new System.Windows.Forms.ComboBox();
            this.comboServers = new System.Windows.Forms.ComboBox();
            this.labelLocal = new System.Windows.Forms.Label();
            this.NumLocalPort = new System.Windows.Forms.NumericUpDown();
            this.label1 = new System.Windows.Forms.Label();
            this.textRemarks = new System.Windows.Forms.TextBox();
            this.tableLayoutPanel3 = new System.Windows.Forms.TableLayoutPanel();
            this.btnApply = new System.Windows.Forms.Button();
            this.btnOK = new System.Windows.Forms.Button();
            this.btnClose = new System.Windows.Forms.Button();
            this.tableLayoutPanel4 = new System.Windows.Forms.TableLayoutPanel();
            this.btnAdd = new System.Windows.Forms.Button();
            this.btnDelete = new System.Windows.Forms.Button();
            this.tableLayoutPanel1.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.tableLayoutPanel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.NumTargetPort)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.NumLocalPort)).BeginInit();
            this.tableLayoutPanel3.SuspendLayout();
            this.tableLayoutPanel4.SuspendLayout();
            this.SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.AutoSize = true;
            this.tableLayoutPanel1.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.tableLayoutPanel1.ColumnCount = 2;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel1.Controls.Add(this.listPorts, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.groupBox1, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this.tableLayoutPanel3, 1, 1);
            this.tableLayoutPanel1.Controls.Add(this.tableLayoutPanel4, 0, 1);
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Margin = new System.Windows.Forms.Padding(0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 2;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.Size = new System.Drawing.Size(646, 332);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // listPorts
            // 
            this.listPorts.Dock = System.Windows.Forms.DockStyle.Fill;
            this.listPorts.FormattingEnabled = true;
            this.listPorts.Location = new System.Drawing.Point(3, 3);
            this.listPorts.Name = "listPorts";
            this.listPorts.Size = new System.Drawing.Size(144, 238);
            this.listPorts.TabIndex = 0;
            this.listPorts.SelectedIndexChanged += new System.EventHandler(this.listPorts_SelectedIndexChanged);
            // 
            // groupBox1
            // 
            this.groupBox1.AutoSize = true;
            this.groupBox1.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.groupBox1.Controls.Add(this.tableLayoutPanel2);
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox1.Location = new System.Drawing.Point(153, 3);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(490, 238);
            this.groupBox1.TabIndex = 2;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Map Setting";
            // 
            // tableLayoutPanel2
            // 
            this.tableLayoutPanel2.AutoSize = true;
            this.tableLayoutPanel2.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.tableLayoutPanel2.ColumnCount = 2;
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel2.Controls.Add(this.labelType, 0, 1);
            this.tableLayoutPanel2.Controls.Add(this.labelID, 0, 2);
            this.tableLayoutPanel2.Controls.Add(this.labelAddr, 0, 4);
            this.tableLayoutPanel2.Controls.Add(this.labelPort, 0, 5);
            this.tableLayoutPanel2.Controls.Add(this.checkEnable, 1, 0);
            this.tableLayoutPanel2.Controls.Add(this.textAddr, 1, 4);
            this.tableLayoutPanel2.Controls.Add(this.NumTargetPort, 1, 5);
            this.tableLayoutPanel2.Controls.Add(this.comboBoxType, 1, 1);
            this.tableLayoutPanel2.Controls.Add(this.comboServers, 1, 2);
            this.tableLayoutPanel2.Controls.Add(this.labelLocal, 0, 3);
            this.tableLayoutPanel2.Controls.Add(this.NumLocalPort, 1, 3);
            this.tableLayoutPanel2.Controls.Add(this.label1, 0, 6);
            this.tableLayoutPanel2.Controls.Add(this.textRemarks, 1, 6);
            this.tableLayoutPanel2.Location = new System.Drawing.Point(6, 19);
            this.tableLayoutPanel2.Name = "tableLayoutPanel2";
            this.tableLayoutPanel2.RowCount = 7;
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel2.Size = new System.Drawing.Size(478, 181);
            this.tableLayoutPanel2.TabIndex = 0;
            // 
            // labelType
            // 
            this.labelType.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.labelType.AutoSize = true;
            this.labelType.Location = new System.Drawing.Point(35, 30);
            this.labelType.Name = "labelType";
            this.labelType.Size = new System.Drawing.Size(31, 13);
            this.labelType.TabIndex = 0;
            this.labelType.Text = "Type";
            // 
            // labelID
            // 
            this.labelID.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.labelID.AutoSize = true;
            this.labelID.Location = new System.Drawing.Point(14, 57);
            this.labelID.Name = "labelID";
            this.labelID.Size = new System.Drawing.Size(52, 13);
            this.labelID.TabIndex = 0;
            this.labelID.Text = "Server ID";
            // 
            // labelAddr
            // 
            this.labelAddr.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.labelAddr.AutoSize = true;
            this.labelAddr.Location = new System.Drawing.Point(3, 109);
            this.labelAddr.Name = "labelAddr";
            this.labelAddr.Size = new System.Drawing.Size(63, 13);
            this.labelAddr.TabIndex = 0;
            this.labelAddr.Text = "Target Addr";
            // 
            // labelPort
            // 
            this.labelPort.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.labelPort.AutoSize = true;
            this.labelPort.Location = new System.Drawing.Point(6, 135);
            this.labelPort.Name = "labelPort";
            this.labelPort.Size = new System.Drawing.Size(60, 13);
            this.labelPort.TabIndex = 0;
            this.labelPort.Text = "Target Port";
            // 
            // checkEnable
            // 
            this.checkEnable.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.checkEnable.AutoSize = true;
            this.checkEnable.Location = new System.Drawing.Point(72, 3);
            this.checkEnable.Name = "checkEnable";
            this.checkEnable.Size = new System.Drawing.Size(59, 17);
            this.checkEnable.TabIndex = 3;
            this.checkEnable.Text = "Enable";
            this.checkEnable.UseVisualStyleBackColor = true;
            // 
            // textAddr
            // 
            this.textAddr.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.textAddr.Location = new System.Drawing.Point(72, 106);
            this.textAddr.Name = "textAddr";
            this.textAddr.Size = new System.Drawing.Size(403, 20);
            this.textAddr.TabIndex = 7;
            // 
            // NumTargetPort
            // 
            this.NumTargetPort.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.NumTargetPort.Location = new System.Drawing.Point(72, 132);
            this.NumTargetPort.Maximum = new decimal(new int[] {
            65535,
            0,
            0,
            0});
            this.NumTargetPort.Name = "NumTargetPort";
            this.NumTargetPort.Size = new System.Drawing.Size(403, 20);
            this.NumTargetPort.TabIndex = 8;
            // 
            // comboBoxType
            // 
            this.comboBoxType.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.comboBoxType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxType.FormattingEnabled = true;
            this.comboBoxType.Items.AddRange(new object[] {
            "Port Forward",
            "Force Proxy",
            "Proxy With Rule"});
            this.comboBoxType.Location = new System.Drawing.Point(72, 26);
            this.comboBoxType.Name = "comboBoxType";
            this.comboBoxType.Size = new System.Drawing.Size(403, 21);
            this.comboBoxType.TabIndex = 4;
            this.comboBoxType.SelectedIndexChanged += new System.EventHandler(this.comboBoxType_SelectedIndexChanged);
            // 
            // comboServers
            // 
            this.comboServers.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.comboServers.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboServers.FormattingEnabled = true;
            this.comboServers.Location = new System.Drawing.Point(72, 53);
            this.comboServers.Name = "comboServers";
            this.comboServers.Size = new System.Drawing.Size(403, 21);
            this.comboServers.TabIndex = 5;
            // 
            // labelLocal
            // 
            this.labelLocal.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.labelLocal.AutoSize = true;
            this.labelLocal.Location = new System.Drawing.Point(11, 83);
            this.labelLocal.Name = "labelLocal";
            this.labelLocal.Size = new System.Drawing.Size(55, 13);
            this.labelLocal.TabIndex = 0;
            this.labelLocal.Text = "Local Port";
            // 
            // NumLocalPort
            // 
            this.NumLocalPort.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.NumLocalPort.Location = new System.Drawing.Point(72, 80);
            this.NumLocalPort.Maximum = new decimal(new int[] {
            65535,
            0,
            0,
            0});
            this.NumLocalPort.Name = "NumLocalPort";
            this.NumLocalPort.Size = new System.Drawing.Size(403, 20);
            this.NumLocalPort.TabIndex = 6;
            // 
            // label1
            // 
            this.label1.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(17, 161);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(49, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Remarks";
            // 
            // textRemarks
            // 
            this.textRemarks.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.textRemarks.Location = new System.Drawing.Point(72, 158);
            this.textRemarks.Name = "textRemarks";
            this.textRemarks.Size = new System.Drawing.Size(403, 20);
            this.textRemarks.TabIndex = 9;
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
            this.tableLayoutPanel3.Controls.Add(this.btnOK, 0, 0);
            this.tableLayoutPanel3.Controls.Add(this.btnClose, 1, 0);
            this.tableLayoutPanel3.Location = new System.Drawing.Point(228, 267);
            this.tableLayoutPanel3.Name = "tableLayoutPanel3";
            this.tableLayoutPanel3.RowCount = 1;
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel3.Size = new System.Drawing.Size(339, 41);
            this.tableLayoutPanel3.TabIndex = 3;
            // 
            // btnApply
            // 
            this.btnApply.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.btnApply.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnApply.Location = new System.Drawing.Point(229, 3);
            this.btnApply.Name = "btnApply";
            this.btnApply.Size = new System.Drawing.Size(107, 35);
            this.btnApply.TabIndex = 11;
            this.btnApply.Text = "Apply";
            this.btnApply.UseVisualStyleBackColor = true;
            this.btnApply.Click += new System.EventHandler(this.BtnApply_Click);
            // 
            // btnOK
            // 
            this.btnOK.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.btnOK.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnOK.Location = new System.Drawing.Point(3, 3);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(107, 35);
            this.btnOK.TabIndex = 10;
            this.btnOK.Text = "OK";
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Click += new System.EventHandler(this.BtnOK_Click);
            // 
            // btnClose
            // 
            this.btnClose.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.btnClose.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnClose.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnClose.Location = new System.Drawing.Point(116, 3);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(107, 35);
            this.btnClose.TabIndex = 11;
            this.btnClose.Text = "Close";
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.BtnClose_Click);
            // 
            // tableLayoutPanel4
            // 
            this.tableLayoutPanel4.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.tableLayoutPanel4.AutoSize = true;
            this.tableLayoutPanel4.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.tableLayoutPanel4.ColumnCount = 1;
            this.tableLayoutPanel4.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel4.Controls.Add(this.btnAdd, 0, 0);
            this.tableLayoutPanel4.Controls.Add(this.btnDelete, 0, 1);
            this.tableLayoutPanel4.Location = new System.Drawing.Point(19, 247);
            this.tableLayoutPanel4.Name = "tableLayoutPanel4";
            this.tableLayoutPanel4.RowCount = 2;
            this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel4.Size = new System.Drawing.Size(111, 82);
            this.tableLayoutPanel4.TabIndex = 1;
            // 
            // btnAdd
            // 
            this.btnAdd.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.btnAdd.Location = new System.Drawing.Point(3, 3);
            this.btnAdd.Name = "btnAdd";
            this.btnAdd.Size = new System.Drawing.Size(105, 35);
            this.btnAdd.TabIndex = 1;
            this.btnAdd.Text = "&Add";
            this.btnAdd.UseVisualStyleBackColor = true;
            this.btnAdd.Click += new System.EventHandler(this.btnAdd_Click);
            // 
            // btnDelete
            // 
            this.btnDelete.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.btnDelete.Location = new System.Drawing.Point(3, 44);
            this.btnDelete.Name = "btnDelete";
            this.btnDelete.Size = new System.Drawing.Size(105, 35);
            this.btnDelete.TabIndex = 2;
            this.btnDelete.Text = "&Delete";
            this.btnDelete.UseVisualStyleBackColor = true;
            this.btnDelete.Click += new System.EventHandler(this.btnDelete_Click);
            // 
            // PortSettingsForm
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.AutoSize = true;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.CancelButton = this.btnClose;
            this.ClientSize = new System.Drawing.Size(1326, 688);
            this.Controls.Add(this.tableLayoutPanel1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Name = "PortSettingsForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Port Settings";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.PortMapForm_FormClosed);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.tableLayoutPanel2.ResumeLayout(false);
            this.tableLayoutPanel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.NumTargetPort)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.NumLocalPort)).EndInit();
            this.tableLayoutPanel3.ResumeLayout(false);
            this.tableLayoutPanel4.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.ListBox listPorts;
        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
        private System.Windows.Forms.Label labelType;
        private System.Windows.Forms.Label labelID;
        private System.Windows.Forms.Label labelAddr;
        private System.Windows.Forms.Label labelPort;
        private System.Windows.Forms.CheckBox checkEnable;
        private System.Windows.Forms.TextBox textAddr;
        private System.Windows.Forms.NumericUpDown NumTargetPort;
        private System.Windows.Forms.ComboBox comboBoxType;
        private System.Windows.Forms.ComboBox comboServers;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel3;
        private System.Windows.Forms.Label labelLocal;
        private System.Windows.Forms.NumericUpDown NumLocalPort;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel4;
        private System.Windows.Forms.Button btnAdd;
        private System.Windows.Forms.Button btnDelete;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox textRemarks;
        private System.Windows.Forms.Button btnApply;
    }
}