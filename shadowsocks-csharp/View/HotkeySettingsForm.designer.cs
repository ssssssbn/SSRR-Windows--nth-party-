namespace Shadowsocks.View
{
    partial class HotkeySettingsForm
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
            System.Windows.Forms.FlowLayoutPanel flowLayoutPanel1;
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnOK = new System.Windows.Forms.Button();
            this.btnRegisterAll = new System.Windows.Forms.Button();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.RegHotkeysAtStartupLabel = new System.Windows.Forms.Label();
            this.SwitchProxyModeLabel = new System.Windows.Forms.Label();
            this.SwitchLoadBalanceLabel = new System.Windows.Forms.Label();
            this.SwitchAllowLanLabel = new System.Windows.Forms.Label();
            this.CallClipboardAndQRCodeScanningLabel = new System.Windows.Forms.Label();
            this.ServerMoveUpLabel = new System.Windows.Forms.Label();
            this.ServerMoveDownLabel = new System.Windows.Forms.Label();
            this.SwitchProxyModeTextBox = new System.Windows.Forms.TextBox();
            this.SwitchLoadBalanceTextBox = new System.Windows.Forms.TextBox();
            this.SwitchAllowLanTextBox = new System.Windows.Forms.TextBox();
            this.CallClipboardAndQRCodeScanningTextBox = new System.Windows.Forms.TextBox();
            this.ServerMoveUpTextBox = new System.Windows.Forms.TextBox();
            this.ServerMoveDownTextBox = new System.Windows.Forms.TextBox();
            this.RegHotkeysAtStartupCheckBox = new System.Windows.Forms.CheckBox();
            flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
            flowLayoutPanel1.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // flowLayoutPanel1
            // 
            this.tableLayoutPanel1.SetColumnSpan(flowLayoutPanel1, 2);
            flowLayoutPanel1.Controls.Add(this.btnCancel);
            flowLayoutPanel1.Controls.Add(this.btnOK);
            flowLayoutPanel1.Controls.Add(this.btnRegisterAll);
            flowLayoutPanel1.FlowDirection = System.Windows.Forms.FlowDirection.BottomUp;
            flowLayoutPanel1.Location = new System.Drawing.Point(0, 218);
            flowLayoutPanel1.Margin = new System.Windows.Forms.Padding(0);
            flowLayoutPanel1.Name = "flowLayoutPanel1";
            flowLayoutPanel1.Padding = new System.Windows.Forms.Padding(0, 0, 16, 0);
            flowLayoutPanel1.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            flowLayoutPanel1.Size = new System.Drawing.Size(475, 43);
            flowLayoutPanel1.TabIndex = 6;
            // 
            // btnCancel
            // 
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(333, 9);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.btnCancel.Size = new System.Drawing.Size(123, 31);
            this.btnCancel.TabIndex = 0;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.CancelButton_Click);
            // 
            // btnOK
            // 
            this.btnOK.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btnOK.Location = new System.Drawing.Point(204, 9);
            this.btnOK.Name = "btnOK";
            this.btnOK.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.btnOK.Size = new System.Drawing.Size(123, 31);
            this.btnOK.TabIndex = 1;
            this.btnOK.Text = "OK";
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Click += new System.EventHandler(this.OKButton_Click);
            // 
            // btnRegisterAll
            // 
            this.btnRegisterAll.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnRegisterAll.Location = new System.Drawing.Point(75, 9);
            this.btnRegisterAll.Name = "btnRegisterAll";
            this.btnRegisterAll.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.btnRegisterAll.Size = new System.Drawing.Size(123, 31);
            this.btnRegisterAll.TabIndex = 2;
            this.btnRegisterAll.Text = "Reg All";
            this.btnRegisterAll.UseVisualStyleBackColor = true;
            this.btnRegisterAll.Visible = false;
            this.btnRegisterAll.Click += new System.EventHandler(this.RegisterAllButton_Click);
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 2;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel1.Controls.Add(this.RegHotkeysAtStartupLabel, 0, 6);
            this.tableLayoutPanel1.Controls.Add(this.SwitchProxyModeLabel, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.SwitchLoadBalanceLabel, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.SwitchAllowLanLabel, 0, 2);
            this.tableLayoutPanel1.Controls.Add(this.CallClipboardAndQRCodeScanningLabel, 0, 3);
            this.tableLayoutPanel1.Controls.Add(this.ServerMoveUpLabel, 0, 4);
            this.tableLayoutPanel1.Controls.Add(this.ServerMoveDownLabel, 0, 5);
            this.tableLayoutPanel1.Controls.Add(flowLayoutPanel1, 0, 7);
            this.tableLayoutPanel1.Controls.Add(this.SwitchProxyModeTextBox, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this.SwitchLoadBalanceTextBox, 1, 1);
            this.tableLayoutPanel1.Controls.Add(this.SwitchAllowLanTextBox, 1, 2);
            this.tableLayoutPanel1.Controls.Add(this.CallClipboardAndQRCodeScanningTextBox, 1, 3);
            this.tableLayoutPanel1.Controls.Add(this.ServerMoveUpTextBox, 1, 4);
            this.tableLayoutPanel1.Controls.Add(this.ServerMoveDownTextBox, 1, 5);
            this.tableLayoutPanel1.Controls.Add(this.RegHotkeysAtStartupCheckBox, 1, 6);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 8;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 14.16726F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 14.16726F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 14.16726F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 14.16726F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 14.7784F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 14.38949F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 14.16309F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 40F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(491, 261);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // RegHotkeysAtStartupLabel
            // 
            this.RegHotkeysAtStartupLabel.AutoSize = true;
            this.RegHotkeysAtStartupLabel.Dock = System.Windows.Forms.DockStyle.Right;
            this.RegHotkeysAtStartupLabel.Location = new System.Drawing.Point(105, 187);
            this.RegHotkeysAtStartupLabel.Margin = new System.Windows.Forms.Padding(8, 0, 8, 0);
            this.RegHotkeysAtStartupLabel.Name = "RegHotkeysAtStartupLabel";
            this.RegHotkeysAtStartupLabel.Size = new System.Drawing.Size(165, 31);
            this.RegHotkeysAtStartupLabel.TabIndex = 16;
            this.RegHotkeysAtStartupLabel.Text = "Reg Hotkeys At Startup";
            this.RegHotkeysAtStartupLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // SwitchProxyModeLabel
            // 
            this.SwitchProxyModeLabel.AutoSize = true;
            this.SwitchProxyModeLabel.Dock = System.Windows.Forms.DockStyle.Right;
            this.SwitchProxyModeLabel.Location = new System.Drawing.Point(132, 0);
            this.SwitchProxyModeLabel.Margin = new System.Windows.Forms.Padding(8, 0, 8, 0);
            this.SwitchProxyModeLabel.Name = "SwitchProxyModeLabel";
            this.SwitchProxyModeLabel.Size = new System.Drawing.Size(138, 31);
            this.SwitchProxyModeLabel.TabIndex = 0;
            this.SwitchProxyModeLabel.Text = "Switch Proxy Mode";
            this.SwitchProxyModeLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // SwitchLoadBalanceLabel
            // 
            this.SwitchLoadBalanceLabel.AutoSize = true;
            this.SwitchLoadBalanceLabel.Dock = System.Windows.Forms.DockStyle.Right;
            this.SwitchLoadBalanceLabel.Location = new System.Drawing.Point(123, 31);
            this.SwitchLoadBalanceLabel.Margin = new System.Windows.Forms.Padding(8, 0, 8, 0);
            this.SwitchLoadBalanceLabel.Name = "SwitchLoadBalanceLabel";
            this.SwitchLoadBalanceLabel.Size = new System.Drawing.Size(147, 31);
            this.SwitchLoadBalanceLabel.TabIndex = 1;
            this.SwitchLoadBalanceLabel.Text = "Switch Load Balance";
            this.SwitchLoadBalanceLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // SwitchAllowLanLabel
            // 
            this.SwitchAllowLanLabel.AutoSize = true;
            this.SwitchAllowLanLabel.Dock = System.Windows.Forms.DockStyle.Right;
            this.SwitchAllowLanLabel.Location = new System.Drawing.Point(108, 62);
            this.SwitchAllowLanLabel.Margin = new System.Windows.Forms.Padding(8, 0, 8, 0);
            this.SwitchAllowLanLabel.Name = "SwitchAllowLanLabel";
            this.SwitchAllowLanLabel.Size = new System.Drawing.Size(162, 31);
            this.SwitchAllowLanLabel.TabIndex = 3;
            this.SwitchAllowLanLabel.Text = "Switch Share Over LAN";
            this.SwitchAllowLanLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // CallClipboardAndQRCodeScanningLabel
            // 
            this.CallClipboardAndQRCodeScanningLabel.AutoSize = true;
            this.CallClipboardAndQRCodeScanningLabel.Dock = System.Windows.Forms.DockStyle.Right;
            this.CallClipboardAndQRCodeScanningLabel.Location = new System.Drawing.Point(8, 93);
            this.CallClipboardAndQRCodeScanningLabel.Margin = new System.Windows.Forms.Padding(8, 0, 8, 0);
            this.CallClipboardAndQRCodeScanningLabel.Name = "CallClipboardAndQRCodeScanningLabel";
            this.CallClipboardAndQRCodeScanningLabel.Size = new System.Drawing.Size(262, 31);
            this.CallClipboardAndQRCodeScanningLabel.TabIndex = 4;
            this.CallClipboardAndQRCodeScanningLabel.Text = "Call Clipboard And QRCode Scanning";
            this.CallClipboardAndQRCodeScanningLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // ServerMoveUpLabel
            // 
            this.ServerMoveUpLabel.AutoSize = true;
            this.ServerMoveUpLabel.Dock = System.Windows.Forms.DockStyle.Right;
            this.ServerMoveUpLabel.Location = new System.Drawing.Point(118, 124);
            this.ServerMoveUpLabel.Margin = new System.Windows.Forms.Padding(8, 0, 8, 0);
            this.ServerMoveUpLabel.Name = "ServerMoveUpLabel";
            this.ServerMoveUpLabel.Size = new System.Drawing.Size(152, 32);
            this.ServerMoveUpLabel.TabIndex = 4;
            this.ServerMoveUpLabel.Text = "Switch To Prev Server";
            this.ServerMoveUpLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // ServerMoveDownLabel
            // 
            this.ServerMoveDownLabel.AutoSize = true;
            this.ServerMoveDownLabel.Dock = System.Windows.Forms.DockStyle.Right;
            this.ServerMoveDownLabel.Location = new System.Drawing.Point(116, 156);
            this.ServerMoveDownLabel.Margin = new System.Windows.Forms.Padding(8, 0, 8, 0);
            this.ServerMoveDownLabel.Name = "ServerMoveDownLabel";
            this.ServerMoveDownLabel.Size = new System.Drawing.Size(154, 31);
            this.ServerMoveDownLabel.TabIndex = 4;
            this.ServerMoveDownLabel.Text = "Switch To Next Server";
            this.ServerMoveDownLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // SwitchProxyModeTextBox
            // 
            this.SwitchProxyModeTextBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.SwitchProxyModeTextBox.Location = new System.Drawing.Point(281, 3);
            this.SwitchProxyModeTextBox.Margin = new System.Windows.Forms.Padding(3, 3, 16, 3);
            this.SwitchProxyModeTextBox.Name = "SwitchProxyModeTextBox";
            this.SwitchProxyModeTextBox.ReadOnly = true;
            this.SwitchProxyModeTextBox.Size = new System.Drawing.Size(291, 25);
            this.SwitchProxyModeTextBox.TabIndex = 8;
            this.SwitchProxyModeTextBox.KeyDown += new System.Windows.Forms.KeyEventHandler(this.HotkeyDown);
            this.SwitchProxyModeTextBox.KeyUp += new System.Windows.Forms.KeyEventHandler(this.HotkeyUp);
            // 
            // SwitchLoadBalanceTextBox
            // 
            this.SwitchLoadBalanceTextBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.SwitchLoadBalanceTextBox.Location = new System.Drawing.Point(281, 34);
            this.SwitchLoadBalanceTextBox.Margin = new System.Windows.Forms.Padding(3, 3, 16, 3);
            this.SwitchLoadBalanceTextBox.Name = "SwitchLoadBalanceTextBox";
            this.SwitchLoadBalanceTextBox.ReadOnly = true;
            this.SwitchLoadBalanceTextBox.Size = new System.Drawing.Size(291, 25);
            this.SwitchLoadBalanceTextBox.TabIndex = 7;
            this.SwitchLoadBalanceTextBox.KeyDown += new System.Windows.Forms.KeyEventHandler(this.HotkeyDown);
            this.SwitchLoadBalanceTextBox.KeyUp += new System.Windows.Forms.KeyEventHandler(this.HotkeyUp);
            // 
            // SwitchAllowLanTextBox
            // 
            this.SwitchAllowLanTextBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.SwitchAllowLanTextBox.Location = new System.Drawing.Point(281, 65);
            this.SwitchAllowLanTextBox.Margin = new System.Windows.Forms.Padding(3, 3, 16, 3);
            this.SwitchAllowLanTextBox.Name = "SwitchAllowLanTextBox";
            this.SwitchAllowLanTextBox.ReadOnly = true;
            this.SwitchAllowLanTextBox.Size = new System.Drawing.Size(291, 25);
            this.SwitchAllowLanTextBox.TabIndex = 10;
            this.SwitchAllowLanTextBox.KeyDown += new System.Windows.Forms.KeyEventHandler(this.HotkeyDown);
            this.SwitchAllowLanTextBox.KeyUp += new System.Windows.Forms.KeyEventHandler(this.HotkeyUp);
            // 
            // CallClipboardAndQRCodeScanningTextBox
            // 
            this.CallClipboardAndQRCodeScanningTextBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.CallClipboardAndQRCodeScanningTextBox.Location = new System.Drawing.Point(281, 96);
            this.CallClipboardAndQRCodeScanningTextBox.Margin = new System.Windows.Forms.Padding(3, 3, 16, 3);
            this.CallClipboardAndQRCodeScanningTextBox.Name = "CallClipboardAndQRCodeScanningTextBox";
            this.CallClipboardAndQRCodeScanningTextBox.ReadOnly = true;
            this.CallClipboardAndQRCodeScanningTextBox.Size = new System.Drawing.Size(291, 25);
            this.CallClipboardAndQRCodeScanningTextBox.TabIndex = 11;
            this.CallClipboardAndQRCodeScanningTextBox.KeyDown += new System.Windows.Forms.KeyEventHandler(this.HotkeyDown);
            this.CallClipboardAndQRCodeScanningTextBox.KeyUp += new System.Windows.Forms.KeyEventHandler(this.HotkeyUp);
            // 
            // ServerMoveUpTextBox
            // 
            this.ServerMoveUpTextBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ServerMoveUpTextBox.Location = new System.Drawing.Point(281, 127);
            this.ServerMoveUpTextBox.Margin = new System.Windows.Forms.Padding(3, 3, 16, 3);
            this.ServerMoveUpTextBox.Name = "ServerMoveUpTextBox";
            this.ServerMoveUpTextBox.ReadOnly = true;
            this.ServerMoveUpTextBox.Size = new System.Drawing.Size(291, 25);
            this.ServerMoveUpTextBox.TabIndex = 12;
            this.ServerMoveUpTextBox.KeyDown += new System.Windows.Forms.KeyEventHandler(this.HotkeyDown);
            this.ServerMoveUpTextBox.KeyUp += new System.Windows.Forms.KeyEventHandler(this.HotkeyUp);
            // 
            // ServerMoveDownTextBox
            // 
            this.ServerMoveDownTextBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ServerMoveDownTextBox.Location = new System.Drawing.Point(281, 159);
            this.ServerMoveDownTextBox.Margin = new System.Windows.Forms.Padding(3, 3, 16, 3);
            this.ServerMoveDownTextBox.Name = "ServerMoveDownTextBox";
            this.ServerMoveDownTextBox.ReadOnly = true;
            this.ServerMoveDownTextBox.Size = new System.Drawing.Size(291, 25);
            this.ServerMoveDownTextBox.TabIndex = 13;
            this.ServerMoveDownTextBox.KeyDown += new System.Windows.Forms.KeyEventHandler(this.HotkeyDown);
            this.ServerMoveDownTextBox.KeyUp += new System.Windows.Forms.KeyEventHandler(this.HotkeyUp);
            // 
            // RegHotkeysAtStartupCheckBox
            // 
            this.RegHotkeysAtStartupCheckBox.AutoSize = true;
            this.RegHotkeysAtStartupCheckBox.Location = new System.Drawing.Point(281, 196);
            this.RegHotkeysAtStartupCheckBox.Margin = new System.Windows.Forms.Padding(3, 9, 9, 9);
            this.RegHotkeysAtStartupCheckBox.Name = "RegHotkeysAtStartupCheckBox";
            this.RegHotkeysAtStartupCheckBox.Size = new System.Drawing.Size(15, 13);
            this.RegHotkeysAtStartupCheckBox.TabIndex = 17;
            this.RegHotkeysAtStartupCheckBox.UseVisualStyleBackColor = true;
            // 
            // HotkeySettingsForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.ClientSize = new System.Drawing.Size(491, 261);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Font = new System.Drawing.Font("微软雅黑", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "HotkeySettingsForm";
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Edit Hotkeys...";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.HotkeySettingsForm_FormClosed);
            flowLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Label SwitchProxyModeLabel;
        private System.Windows.Forms.Label SwitchLoadBalanceLabel;
        private System.Windows.Forms.Label SwitchAllowLanLabel;
        private System.Windows.Forms.Label CallClipboardAndQRCodeScanningLabel;
        private System.Windows.Forms.Label ServerMoveUpLabel;
        private System.Windows.Forms.Label ServerMoveDownLabel;
        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.TextBox CallClipboardAndQRCodeScanningTextBox;
        private System.Windows.Forms.TextBox SwitchAllowLanTextBox;
        private System.Windows.Forms.TextBox SwitchLoadBalanceTextBox;
        private System.Windows.Forms.TextBox SwitchProxyModeTextBox;
        private System.Windows.Forms.TextBox ServerMoveUpTextBox;
        private System.Windows.Forms.TextBox ServerMoveDownTextBox;
        private System.Windows.Forms.Button btnRegisterAll;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Label RegHotkeysAtStartupLabel;
        private System.Windows.Forms.CheckBox RegHotkeysAtStartupCheckBox;
    }
}