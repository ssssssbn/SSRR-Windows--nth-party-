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
            this.btnClose = new System.Windows.Forms.Button();
            this.btnOKAndEnable = new System.Windows.Forms.Button();
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
            this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
            this.btnDisableAll = new System.Windows.Forms.Button();
            this.tableLayoutPanel3 = new System.Windows.Forms.TableLayoutPanel();
            this.tableLayoutPanel1.SuspendLayout();
            this.tableLayoutPanel2.SuspendLayout();
            this.tableLayoutPanel3.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnClose
            // 
            this.btnClose.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.btnClose.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.btnClose.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnClose.Font = new System.Drawing.Font("Microsoft YaHei", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnClose.Location = new System.Drawing.Point(301, 3);
            this.btnClose.Name = "btnClose";
            this.btnClose.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.btnClose.Size = new System.Drawing.Size(105, 35);
            this.btnClose.TabIndex = 0;
            this.btnClose.Text = "Close";
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.BtnClose_Click);
            // 
            // btnOKAndEnable
            // 
            this.btnOKAndEnable.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.btnOKAndEnable.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.btnOKAndEnable.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btnOKAndEnable.Font = new System.Drawing.Font("Microsoft YaHei", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnOKAndEnable.Location = new System.Drawing.Point(190, 3);
            this.btnOKAndEnable.Name = "btnOKAndEnable";
            this.btnOKAndEnable.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.btnOKAndEnable.Size = new System.Drawing.Size(105, 35);
            this.btnOKAndEnable.TabIndex = 1;
            this.btnOKAndEnable.Text = "OK And Enable";
            this.btnOKAndEnable.UseVisualStyleBackColor = true;
            this.btnOKAndEnable.Click += new System.EventHandler(this.BtnOKAndEnable_Click);
            // 
            // btnRegisterAll
            // 
            this.btnRegisterAll.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.btnRegisterAll.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.btnRegisterAll.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnRegisterAll.Font = new System.Drawing.Font("Microsoft YaHei", 9F);
            this.btnRegisterAll.Location = new System.Drawing.Point(3, 3);
            this.btnRegisterAll.Name = "btnRegisterAll";
            this.btnRegisterAll.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.btnRegisterAll.Size = new System.Drawing.Size(70, 35);
            this.btnRegisterAll.TabIndex = 3;
            this.btnRegisterAll.Text = "Reg All";
            this.btnRegisterAll.UseVisualStyleBackColor = true;
            this.btnRegisterAll.Visible = false;
            this.btnRegisterAll.Click += new System.EventHandler(this.RegisterAllButton_Click);
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.tableLayoutPanel1.AutoSize = true;
            this.tableLayoutPanel1.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
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
            this.tableLayoutPanel1.Controls.Add(this.SwitchProxyModeTextBox, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this.SwitchLoadBalanceTextBox, 1, 1);
            this.tableLayoutPanel1.Controls.Add(this.SwitchAllowLanTextBox, 1, 2);
            this.tableLayoutPanel1.Controls.Add(this.CallClipboardAndQRCodeScanningTextBox, 1, 3);
            this.tableLayoutPanel1.Controls.Add(this.ServerMoveUpTextBox, 1, 4);
            this.tableLayoutPanel1.Controls.Add(this.ServerMoveDownTextBox, 1, 5);
            this.tableLayoutPanel1.Controls.Add(this.RegHotkeysAtStartupCheckBox, 1, 6);
            this.tableLayoutPanel1.Location = new System.Drawing.Point(3, 3);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 7;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.Size = new System.Drawing.Size(497, 218);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // RegHotkeysAtStartupLabel
            // 
            this.RegHotkeysAtStartupLabel.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.RegHotkeysAtStartupLabel.AutoSize = true;
            this.RegHotkeysAtStartupLabel.Location = new System.Drawing.Point(105, 192);
            this.RegHotkeysAtStartupLabel.Margin = new System.Windows.Forms.Padding(8, 0, 8, 0);
            this.RegHotkeysAtStartupLabel.Name = "RegHotkeysAtStartupLabel";
            this.RegHotkeysAtStartupLabel.Size = new System.Drawing.Size(165, 20);
            this.RegHotkeysAtStartupLabel.TabIndex = 12;
            this.RegHotkeysAtStartupLabel.Text = "Reg Hotkeys At Startup";
            this.RegHotkeysAtStartupLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // SwitchProxyModeLabel
            // 
            this.SwitchProxyModeLabel.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.SwitchProxyModeLabel.AutoSize = true;
            this.SwitchProxyModeLabel.Location = new System.Drawing.Point(132, 5);
            this.SwitchProxyModeLabel.Margin = new System.Windows.Forms.Padding(8, 0, 8, 0);
            this.SwitchProxyModeLabel.Name = "SwitchProxyModeLabel";
            this.SwitchProxyModeLabel.Size = new System.Drawing.Size(138, 20);
            this.SwitchProxyModeLabel.TabIndex = 0;
            this.SwitchProxyModeLabel.Text = "Switch Proxy Mode";
            this.SwitchProxyModeLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // SwitchLoadBalanceLabel
            // 
            this.SwitchLoadBalanceLabel.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.SwitchLoadBalanceLabel.AutoSize = true;
            this.SwitchLoadBalanceLabel.Location = new System.Drawing.Point(123, 36);
            this.SwitchLoadBalanceLabel.Margin = new System.Windows.Forms.Padding(8, 0, 8, 0);
            this.SwitchLoadBalanceLabel.Name = "SwitchLoadBalanceLabel";
            this.SwitchLoadBalanceLabel.Size = new System.Drawing.Size(147, 20);
            this.SwitchLoadBalanceLabel.TabIndex = 2;
            this.SwitchLoadBalanceLabel.Text = "Switch Load Balance";
            this.SwitchLoadBalanceLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // SwitchAllowLanLabel
            // 
            this.SwitchAllowLanLabel.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.SwitchAllowLanLabel.AutoSize = true;
            this.SwitchAllowLanLabel.Location = new System.Drawing.Point(108, 67);
            this.SwitchAllowLanLabel.Margin = new System.Windows.Forms.Padding(8, 0, 8, 0);
            this.SwitchAllowLanLabel.Name = "SwitchAllowLanLabel";
            this.SwitchAllowLanLabel.Size = new System.Drawing.Size(162, 20);
            this.SwitchAllowLanLabel.TabIndex = 4;
            this.SwitchAllowLanLabel.Text = "Switch Share Over LAN";
            this.SwitchAllowLanLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // CallClipboardAndQRCodeScanningLabel
            // 
            this.CallClipboardAndQRCodeScanningLabel.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.CallClipboardAndQRCodeScanningLabel.AutoSize = true;
            this.CallClipboardAndQRCodeScanningLabel.Location = new System.Drawing.Point(8, 98);
            this.CallClipboardAndQRCodeScanningLabel.Margin = new System.Windows.Forms.Padding(8, 0, 8, 0);
            this.CallClipboardAndQRCodeScanningLabel.Name = "CallClipboardAndQRCodeScanningLabel";
            this.CallClipboardAndQRCodeScanningLabel.Size = new System.Drawing.Size(262, 20);
            this.CallClipboardAndQRCodeScanningLabel.TabIndex = 6;
            this.CallClipboardAndQRCodeScanningLabel.Text = "Call Clipboard And QRCode Scanning";
            this.CallClipboardAndQRCodeScanningLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // ServerMoveUpLabel
            // 
            this.ServerMoveUpLabel.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.ServerMoveUpLabel.AutoSize = true;
            this.ServerMoveUpLabel.Location = new System.Drawing.Point(118, 129);
            this.ServerMoveUpLabel.Margin = new System.Windows.Forms.Padding(8, 0, 8, 0);
            this.ServerMoveUpLabel.Name = "ServerMoveUpLabel";
            this.ServerMoveUpLabel.Size = new System.Drawing.Size(152, 20);
            this.ServerMoveUpLabel.TabIndex = 8;
            this.ServerMoveUpLabel.Text = "Switch To Prev Server";
            this.ServerMoveUpLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // ServerMoveDownLabel
            // 
            this.ServerMoveDownLabel.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.ServerMoveDownLabel.AutoSize = true;
            this.ServerMoveDownLabel.Location = new System.Drawing.Point(116, 160);
            this.ServerMoveDownLabel.Margin = new System.Windows.Forms.Padding(8, 0, 8, 0);
            this.ServerMoveDownLabel.Name = "ServerMoveDownLabel";
            this.ServerMoveDownLabel.Size = new System.Drawing.Size(154, 20);
            this.ServerMoveDownLabel.TabIndex = 10;
            this.ServerMoveDownLabel.Text = "Switch To Next Server";
            this.ServerMoveDownLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // SwitchProxyModeTextBox
            // 
            this.SwitchProxyModeTextBox.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.SwitchProxyModeTextBox.Location = new System.Drawing.Point(281, 3);
            this.SwitchProxyModeTextBox.Margin = new System.Windows.Forms.Padding(3, 3, 16, 3);
            this.SwitchProxyModeTextBox.Name = "SwitchProxyModeTextBox";
            this.SwitchProxyModeTextBox.ReadOnly = true;
            this.SwitchProxyModeTextBox.Size = new System.Drawing.Size(200, 25);
            this.SwitchProxyModeTextBox.TabIndex = 1;
            this.SwitchProxyModeTextBox.KeyDown += new System.Windows.Forms.KeyEventHandler(this.HotkeyDown);
            this.SwitchProxyModeTextBox.KeyUp += new System.Windows.Forms.KeyEventHandler(this.HotkeyUp);
            // 
            // SwitchLoadBalanceTextBox
            // 
            this.SwitchLoadBalanceTextBox.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.SwitchLoadBalanceTextBox.Location = new System.Drawing.Point(281, 34);
            this.SwitchLoadBalanceTextBox.Margin = new System.Windows.Forms.Padding(3, 3, 16, 3);
            this.SwitchLoadBalanceTextBox.Name = "SwitchLoadBalanceTextBox";
            this.SwitchLoadBalanceTextBox.ReadOnly = true;
            this.SwitchLoadBalanceTextBox.Size = new System.Drawing.Size(200, 25);
            this.SwitchLoadBalanceTextBox.TabIndex = 3;
            this.SwitchLoadBalanceTextBox.KeyDown += new System.Windows.Forms.KeyEventHandler(this.HotkeyDown);
            this.SwitchLoadBalanceTextBox.KeyUp += new System.Windows.Forms.KeyEventHandler(this.HotkeyUp);
            // 
            // SwitchAllowLanTextBox
            // 
            this.SwitchAllowLanTextBox.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.SwitchAllowLanTextBox.Location = new System.Drawing.Point(281, 65);
            this.SwitchAllowLanTextBox.Margin = new System.Windows.Forms.Padding(3, 3, 16, 3);
            this.SwitchAllowLanTextBox.Name = "SwitchAllowLanTextBox";
            this.SwitchAllowLanTextBox.ReadOnly = true;
            this.SwitchAllowLanTextBox.Size = new System.Drawing.Size(200, 25);
            this.SwitchAllowLanTextBox.TabIndex = 5;
            this.SwitchAllowLanTextBox.KeyDown += new System.Windows.Forms.KeyEventHandler(this.HotkeyDown);
            this.SwitchAllowLanTextBox.KeyUp += new System.Windows.Forms.KeyEventHandler(this.HotkeyUp);
            // 
            // CallClipboardAndQRCodeScanningTextBox
            // 
            this.CallClipboardAndQRCodeScanningTextBox.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.CallClipboardAndQRCodeScanningTextBox.Location = new System.Drawing.Point(281, 96);
            this.CallClipboardAndQRCodeScanningTextBox.Margin = new System.Windows.Forms.Padding(3, 3, 16, 3);
            this.CallClipboardAndQRCodeScanningTextBox.Name = "CallClipboardAndQRCodeScanningTextBox";
            this.CallClipboardAndQRCodeScanningTextBox.ReadOnly = true;
            this.CallClipboardAndQRCodeScanningTextBox.Size = new System.Drawing.Size(200, 25);
            this.CallClipboardAndQRCodeScanningTextBox.TabIndex = 7;
            this.CallClipboardAndQRCodeScanningTextBox.KeyDown += new System.Windows.Forms.KeyEventHandler(this.HotkeyDown);
            this.CallClipboardAndQRCodeScanningTextBox.KeyUp += new System.Windows.Forms.KeyEventHandler(this.HotkeyUp);
            // 
            // ServerMoveUpTextBox
            // 
            this.ServerMoveUpTextBox.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.ServerMoveUpTextBox.Location = new System.Drawing.Point(281, 127);
            this.ServerMoveUpTextBox.Margin = new System.Windows.Forms.Padding(3, 3, 16, 3);
            this.ServerMoveUpTextBox.Name = "ServerMoveUpTextBox";
            this.ServerMoveUpTextBox.ReadOnly = true;
            this.ServerMoveUpTextBox.Size = new System.Drawing.Size(200, 25);
            this.ServerMoveUpTextBox.TabIndex = 9;
            this.ServerMoveUpTextBox.KeyDown += new System.Windows.Forms.KeyEventHandler(this.HotkeyDown);
            this.ServerMoveUpTextBox.KeyUp += new System.Windows.Forms.KeyEventHandler(this.HotkeyUp);
            // 
            // ServerMoveDownTextBox
            // 
            this.ServerMoveDownTextBox.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.ServerMoveDownTextBox.Location = new System.Drawing.Point(281, 158);
            this.ServerMoveDownTextBox.Margin = new System.Windows.Forms.Padding(3, 3, 16, 3);
            this.ServerMoveDownTextBox.Name = "ServerMoveDownTextBox";
            this.ServerMoveDownTextBox.ReadOnly = true;
            this.ServerMoveDownTextBox.Size = new System.Drawing.Size(200, 25);
            this.ServerMoveDownTextBox.TabIndex = 11;
            this.ServerMoveDownTextBox.KeyDown += new System.Windows.Forms.KeyEventHandler(this.HotkeyDown);
            this.ServerMoveDownTextBox.KeyUp += new System.Windows.Forms.KeyEventHandler(this.HotkeyUp);
            // 
            // RegHotkeysAtStartupCheckBox
            // 
            this.RegHotkeysAtStartupCheckBox.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.RegHotkeysAtStartupCheckBox.AutoSize = true;
            this.RegHotkeysAtStartupCheckBox.Location = new System.Drawing.Point(281, 195);
            this.RegHotkeysAtStartupCheckBox.Margin = new System.Windows.Forms.Padding(3, 9, 9, 9);
            this.RegHotkeysAtStartupCheckBox.Name = "RegHotkeysAtStartupCheckBox";
            this.RegHotkeysAtStartupCheckBox.Size = new System.Drawing.Size(15, 14);
            this.RegHotkeysAtStartupCheckBox.TabIndex = 13;
            this.RegHotkeysAtStartupCheckBox.UseVisualStyleBackColor = true;
            // 
            // tableLayoutPanel2
            // 
            this.tableLayoutPanel2.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.tableLayoutPanel2.AutoSize = true;
            this.tableLayoutPanel2.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.tableLayoutPanel2.ColumnCount = 4;
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel2.Controls.Add(this.btnRegisterAll, 0, 0);
            this.tableLayoutPanel2.Controls.Add(this.btnClose, 3, 0);
            this.tableLayoutPanel2.Controls.Add(this.btnOKAndEnable, 2, 0);
            this.tableLayoutPanel2.Controls.Add(this.btnDisableAll, 1, 0);
            this.tableLayoutPanel2.Location = new System.Drawing.Point(47, 227);
            this.tableLayoutPanel2.Name = "tableLayoutPanel2";
            this.tableLayoutPanel2.RowCount = 1;
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel2.Size = new System.Drawing.Size(409, 41);
            this.tableLayoutPanel2.TabIndex = 1;
            // 
            // btnDisableAll
            // 
            this.btnDisableAll.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.btnDisableAll.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.btnDisableAll.Location = new System.Drawing.Point(79, 3);
            this.btnDisableAll.Name = "btnDisableAll";
            this.btnDisableAll.Size = new System.Drawing.Size(105, 35);
            this.btnDisableAll.TabIndex = 2;
            this.btnDisableAll.Text = "Disable All";
            this.btnDisableAll.UseVisualStyleBackColor = true;
            this.btnDisableAll.Click += new System.EventHandler(this.BtnDisableAll_Click);
            // 
            // tableLayoutPanel3
            // 
            this.tableLayoutPanel3.AutoSize = true;
            this.tableLayoutPanel3.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.tableLayoutPanel3.ColumnCount = 1;
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel3.Controls.Add(this.tableLayoutPanel2, 0, 1);
            this.tableLayoutPanel3.Controls.Add(this.tableLayoutPanel1, 0, 0);
            this.tableLayoutPanel3.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel3.Margin = new System.Windows.Forms.Padding(0);
            this.tableLayoutPanel3.Name = "tableLayoutPanel3";
            this.tableLayoutPanel3.RowCount = 2;
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel3.Size = new System.Drawing.Size(503, 271);
            this.tableLayoutPanel3.TabIndex = 0;
            // 
            // HotkeySettingsForm
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.AutoSize = true;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.CancelButton = this.btnOKAndEnable;
            this.ClientSize = new System.Drawing.Size(784, 562);
            this.Controls.Add(this.tableLayoutPanel3);
            this.Font = new System.Drawing.Font("Microsoft YaHei", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "HotkeySettingsForm";
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Edit Hotkeys...";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.HotkeySettingsForm_FormClosed);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.tableLayoutPanel2.ResumeLayout(false);
            this.tableLayoutPanel3.ResumeLayout(false);
            this.tableLayoutPanel3.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Label SwitchProxyModeLabel;
        private System.Windows.Forms.Label SwitchLoadBalanceLabel;
        private System.Windows.Forms.Label SwitchAllowLanLabel;
        private System.Windows.Forms.Label CallClipboardAndQRCodeScanningLabel;
        private System.Windows.Forms.Label ServerMoveUpLabel;
        private System.Windows.Forms.Label ServerMoveDownLabel;
        private System.Windows.Forms.Button btnOKAndEnable;
        private System.Windows.Forms.Button btnClose;
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
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel3;
        private System.Windows.Forms.Button btnDisableAll;
    }
}