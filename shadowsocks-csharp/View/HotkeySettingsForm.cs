using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Shadowsocks.Controller;
using Shadowsocks.Model;
using Shadowsocks.Properties;
using Shadowsocks.Util;
using static Shadowsocks.Controller.HotkeyReg;

namespace Shadowsocks.View
{
    public partial class HotkeySettingsForm : Form
    {
        public static bool _IsModified = false;
        private HotkeyConfig _TempHotkeyConfig;
        private readonly ShadowsocksController _controller;

        // this is a copy of hotkey configuration that we are working on
        private HotkeyConfig _modifiedHotkeyConfig;

        public HotkeySettingsForm(ShadowsocksController controller)
        {
            InitializeComponent();
            UpdateTexts();
            Icon = Icon.FromHandle(Resources.ssw128.GetHicon());

            _controller = controller;
            _controller.ConfigChanged += controller_ConfigChanged;

            LoadCurrentConfiguration();
        }

        //public void RegOrUnregAppBar(bool registered)
        //{
        //    RegAppBar(registered);
        //}

        private void UpdateTexts()
        {
            // I18N stuff
            SwitchProxyModeLabel.Text = I18N.GetString(SwitchProxyModeLabel.Text);
            SwitchLoadBalanceLabel.Text = I18N.GetString(SwitchLoadBalanceLabel.Text);
            SwitchAllowLanLabel.Text = I18N.GetString(SwitchAllowLanLabel.Text);
            CallClipboardAndQRCodeScanningLabel.Text = I18N.GetString(CallClipboardAndQRCodeScanningLabel.Text);
            ServerMoveUpLabel.Text = I18N.GetString(ServerMoveUpLabel.Text);
            ServerMoveDownLabel.Text = I18N.GetString(ServerMoveDownLabel.Text);
            RegHotkeysAtStartupLabel.Text = I18N.GetString(RegHotkeysAtStartupLabel.Text);
            btnOK.Text = I18N.GetString(btnOK.Text);
            btnCancel.Text = I18N.GetString(btnCancel.Text);
            btnRegisterAll.Text = I18N.GetString(btnRegisterAll.Text);
            this.Text = I18N.GetString(this.Text);
            //SwitchAllowLanLabel.Text = I18N.GetString("Switch share over LAN");
            //ShowLogsLabel.Text = I18N.GetString("Show Logs");
            //ServerMoveUpLabel.Text = I18N.GetString("Switch to prev server");
            //ServerMoveDownLabel.Text = I18N.GetString("Switch to next server");
            //RegHotkeysAtStartupLabel.Text = I18N.GetString("Reg Hotkeys At Startup");
            //btnOK.Text = I18N.GetString("OK");
            //btnCancel.Text = I18N.GetString("Cancel");
            //btnRegisterAll.Text = I18N.GetString("Reg All");
            //this.Text = I18N.GetString("Edit Hotkeys...");
        }

        private void controller_ConfigChanged(object sender, EventArgs e)
        {
            LoadCurrentConfiguration();
        }

        private void LoadCurrentConfiguration()
        {
            _TempHotkeyConfig = _modifiedHotkeyConfig = _controller.GetCurrentConfiguration().hotkey;
                //GetConfiguration().hotkey;
            SetConfigToUI(_modifiedHotkeyConfig);
        }

        private void SetConfigToUI(HotkeyConfig config)
        {
            SwitchProxyModeTextBox.Text = config.SwitchProxyMode;
            SwitchLoadBalanceTextBox.Text = config.SwitchLoadBalance;
            SwitchAllowLanTextBox.Text = config.SwitchAllowLan;
            CallClipboardAndQRCodeScanningTextBox.Text = config.CallClipboardAndQRCodeScanning;
            ServerMoveUpTextBox.Text = config.ServerMoveUp;
            ServerMoveDownTextBox.Text = config.ServerMoveDown;
            RegHotkeysAtStartupCheckBox.Checked = config.RegHotkeysAtStartup;
        }

        private void SaveConfig()
        {
            _controller.SaveHotkeyConfig(_modifiedHotkeyConfig);
        }

        private HotkeyConfig GetConfigFromUI()
        {
            return new HotkeyConfig
            {
                SwitchProxyMode = SwitchProxyModeTextBox.Text,
                SwitchLoadBalance = SwitchLoadBalanceTextBox.Text,
                SwitchAllowLan = SwitchAllowLanTextBox.Text,
                CallClipboardAndQRCodeScanning = CallClipboardAndQRCodeScanningTextBox.Text,
                ServerMoveUp = ServerMoveUpTextBox.Text,
                ServerMoveDown = ServerMoveDownTextBox.Text,
                RegHotkeysAtStartup = RegHotkeysAtStartupCheckBox.Checked
            };
        }

        /// <summary>
        /// Capture hotkey - Press key
        /// </summary>
        private void HotkeyDown(object sender, KeyEventArgs e)
        {
            StringBuilder sb = new StringBuilder();
            //Combination key only
            if (e.Modifiers != 0)
            {
                // XXX: Hotkey parsing depends on the sequence, more specifically, ModifierKeysConverter.
                // Windows key is reserved by operating system, we deny this key.
                if (e.Control)
                {
                    sb.Append("Ctrl+");
                }
                if (e.Alt)
                {
                    sb.Append("Alt+");
                }
                if (e.Shift)
                {
                    sb.Append("Shift+");
                }

                Keys keyvalue = (Keys)e.KeyValue;
                if ((keyvalue >= Keys.PageUp && keyvalue <= Keys.Down) ||
                    (keyvalue >= Keys.A && keyvalue <= Keys.Z) ||
                    (keyvalue >= Keys.F1 && keyvalue <= Keys.F12))
                {
                    sb.Append(e.KeyCode);
                }
                else if (keyvalue >= Keys.D0 && keyvalue <= Keys.D9)
                {
                    sb.Append('D').Append((char)e.KeyValue);
                }
                else if (keyvalue >= Keys.NumPad0 && keyvalue <= Keys.NumPad9)
                {
                    sb.Append("NumPad").Append((char)(e.KeyValue - 48));
                }
            }
            ((TextBox)sender).Text = sb.ToString();
        }

        /// <summary>
        /// Capture hotkey - Release key
        /// </summary>
        private void HotkeyUp(object sender, KeyEventArgs e)
        {
            var tb = (TextBox)sender;
            var content = tb.Text.TrimEnd();
            if (content.Length >= 1 && content[content.Length - 1] == '+')
            {
                tb.Text = "";
            }
            if (tb.Text != "") 
            {
                HotkeySettingsForm._IsModified = true;
                TryRegAllHotkeys();
            }
        }

        private void CancelButton_Click(object sender, EventArgs e)
        {
            if(_IsModified)
                RegisterAllHotkeys(_TempHotkeyConfig);
            this.Close();
        }

        private void OKButton_Click(object sender, EventArgs e)
        {
            _modifiedHotkeyConfig = GetConfigFromUI();
            // try to register, notify to change settings if failed
            if (!RegisterAllHotkeys(_modifiedHotkeyConfig))
            {
                if (MenuViewController.appbarform != null)
                {
                    MenuViewController.appbarform.RegAppBar(true);
                    MenuViewController.appbarform.Dispose();
                    MenuViewController.appbarform = null;
                    Utils.ReleaseMemory(true);
                }

                //IsRegHotkeys = false;
                MessageBox.Show(I18N.GetString("Register hotkey failed"));
            }
            else
            {
                if (MenuViewController.appbarform == null)
                    MenuViewController.appbarform = new AppBarForm();
            }
                
            //IsRegHotkeys = true;
            // All check passed, saving
            SaveConfig();
            this.Close();
        }

        private void RegisterAllButton_Click(object sender, EventArgs e)
        {
            _modifiedHotkeyConfig = GetConfigFromUI();
            RegisterAllHotkeys(_modifiedHotkeyConfig);
        }

        private bool RegisterAllHotkeys(HotkeyConfig hotkeyConfig)
        {
            return
            RegHotkeyFromString(hotkeyConfig.SwitchProxyMode, "SwitchProxyModeCallback", result => HandleRegResult(hotkeyConfig.SwitchProxyMode, SwitchProxyModeLabel, result))
            && RegHotkeyFromString(hotkeyConfig.SwitchLoadBalance, "SwitchLoadBalanceCallback", result => HandleRegResult(hotkeyConfig.SwitchLoadBalance, SwitchLoadBalanceLabel, result))
            && RegHotkeyFromString(hotkeyConfig.SwitchAllowLan, "SwitchAllowLanCallback", result => HandleRegResult(hotkeyConfig.SwitchAllowLan, SwitchAllowLanLabel, result))
            && RegHotkeyFromString(hotkeyConfig.CallClipboardAndQRCodeScanning, "ClipboardAndQRCodeScanningCallback", result => HandleRegResult(hotkeyConfig.CallClipboardAndQRCodeScanning, CallClipboardAndQRCodeScanningLabel, result))
            //&& RegHotkeyFromString(hotkeyConfig.ShowLogs, "ShowLogsCallback", result => HandleRegResult(hotkeyConfig.ShowLogs, CallClipboardAndQRCodeScanningLabel, result))
            && RegHotkeyFromString(hotkeyConfig.ServerMoveUp, "ServerMoveUpCallback", result => HandleRegResult(hotkeyConfig.ServerMoveUp, ServerMoveUpLabel, result))
            && RegHotkeyFromString(hotkeyConfig.ServerMoveDown, "ServerMoveDownCallback", result => HandleRegResult(hotkeyConfig.ServerMoveDown, ServerMoveDownLabel, result))
            ;
        }

        private void HandleRegResult(string hotkeyStr, Label label, RegResult result)
        {
            switch (result)
            {
                case RegResult.ParseError:
                    MessageBox.Show(string.Format(I18N.GetString("Cannot parse hotkey: {0}"), hotkeyStr));
                    break;
                case RegResult.UnregSuccess:
                    label.ResetBackColor();
                    break;
                case RegResult.RegSuccess:
                    label.BackColor = Color.Green;
                    break;
                case RegResult.RegFailure:
                    label.BackColor = Color.Red;
                    break;
                default:
                    break;
            }
        }

        private void TryRegAllHotkeys()
        {
            _modifiedHotkeyConfig = GetConfigFromUI();
            RegHotkeyFromString(_modifiedHotkeyConfig.SwitchProxyMode, "SwitchProxyModeCallback", result => HandleRegResult(_modifiedHotkeyConfig.SwitchProxyMode, SwitchProxyModeLabel, result));
            RegHotkeyFromString(_modifiedHotkeyConfig.SwitchLoadBalance, "SwitchLoadBalanceCallback", result => HandleRegResult(_modifiedHotkeyConfig.SwitchLoadBalance, SwitchLoadBalanceLabel, result));
            RegHotkeyFromString(_modifiedHotkeyConfig.SwitchAllowLan, "SwitchAllowLanCallback", result => HandleRegResult(_modifiedHotkeyConfig.SwitchAllowLan, SwitchAllowLanLabel, result));
            RegHotkeyFromString(_modifiedHotkeyConfig.CallClipboardAndQRCodeScanning, "ClipboardAndQRCodeScanningCallback", result => HandleRegResult(_modifiedHotkeyConfig.CallClipboardAndQRCodeScanning, CallClipboardAndQRCodeScanningLabel, result));
            //RegHotkeyFromString(_modifiedHotkeyConfig.ShowLogs, "ShowLogsCallback", result => HandleRegResult(_modifiedHotkeyConfig.ShowLogs, CallClipboardAndQRCodeScanningLabel, result));
            RegHotkeyFromString(_modifiedHotkeyConfig.ServerMoveUp, "ServerMoveUpCallback", result => HandleRegResult(_modifiedHotkeyConfig.ServerMoveUp, ServerMoveUpLabel, result));
            RegHotkeyFromString(_modifiedHotkeyConfig.ServerMoveDown, "ServerMoveDownCallback", result => HandleRegResult(_modifiedHotkeyConfig.ServerMoveDown, ServerMoveDownLabel, result));
        }

        private void HotkeySettingsForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            _controller.ConfigChanged -= controller_ConfigChanged;
        }
    }
}