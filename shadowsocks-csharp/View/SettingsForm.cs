using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Shadowsocks.Controller;
using Shadowsocks.Model;
using Shadowsocks.Properties;

namespace Shadowsocks.View
{
    public partial class SettingsForm : Form
    {
        private ShadowsocksController controller;
        private Settings settings;
        private Dictionary<int, string> _balanceIndexMap = new Dictionary<int, string>();

        public SettingsForm(ShadowsocksController controller)
        {
            this.Font = System.Drawing.SystemFonts.MessageBoxFont;
            InitializeComponent();

            this.Icon = Icon.FromHandle(Resources.ssw128.GetHicon());
            this.controller = controller;

            UpdateTexts();
            controller.ConfigChanged += controller_ConfigChanged;

            int dpi_mul = Util.Utils.GetDpiMul();
            LoadCurrentConfiguration();
        }

        private void SettingsForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            controller.ConfigChanged -= controller_ConfigChanged;
        }

        private void UpdateTexts()
        {
            this.Text = I18N.GetString("Global Settings") + "("
                + (controller.GetCurrentConfiguration().shareOverLan ? I18N.GetString("Any") : I18N.GetString("Local")) + ":" + controller.GetCurrentConfiguration().localPort.ToString()
                + I18N.GetString(" Version") + ":" + UpdateChecker.FullVersion
                + ")";

            gbxListen.Text = I18N.GetString(gbxListen.Text);
            chkShareOverLan.Text = I18N.GetString(chkShareOverLan.Text);
            lblProxyPort.Text = I18N.GetString(lblProxyPort.Text);// "Proxy Port");
            btnSetDefault.Text = I18N.GetString(btnSetDefault.Text);// "Set Default");
            lblLocalDns.Text = I18N.GetString(lblLocalDns.Text);// "Local Dns");
            lblReconnect.Text = I18N.GetString(lblReconnect.Text);// "Reconnect Times");
            lblTtl.Text = I18N.GetString(lblTtl.Text);// "TTL");
            lblTimeout.Text = I18N.GetString(lblTimeout.Text);

            chkAutoStartup.Text = I18N.GetString(chkAutoStartup.Text);
            chkBalance.Text = I18N.GetString(chkBalance.Text);
            chkAutoBan.Text = I18N.GetString(chkAutoBan.Text);// "AutoBan");
            chkLoggingEnable.Text = I18N.GetString(chkLoggingEnable.Text);

            gbxSocks5Proxy.Text = I18N.GetString(gbxSocks5Proxy.Text);
            chkPacProxy.Text = I18N.GetString(chkPacProxy.Text);
            chkSockProxy.Text = I18N.GetString(chkSockProxy.Text);// "Proxy On");
            lblS5Server.Text = I18N.GetString(lblS5Server.Text);// "Server IP");
            lblS5Port.Text = I18N.GetString(lblS5Port.Text);// "Server Port");
            //lblS5Server.Text = I18N.GetString("Server IP");
            //lblS5Port.Text = I18N.GetString("Server Port");
            lblS5Username.Text = I18N.GetString(lblS5Username.Text);// "Username");
            lblS5Password.Text = I18N.GetString(lblS5Password.Text);// "Password");
            lblUserAgent.Text = I18N.GetString(lblUserAgent.Text);// "User Agent");
            lblAuthUser.Text = I18N.GetString(lblAuthUser.Text);// "Username");
            lblAuthPass.Text = I18N.GetString(lblAuthPass.Text);// "Password");

            lblBalance.Text = I18N.GetString(lblBalance.Text);// "Enable balance");
            for (int i = 0; i < cmbProxyType.Items.Count; ++i)
            {
                cmbProxyType.Items[i] = I18N.GetString(cmbProxyType.Items[i].ToString());
            }
            chkBalanceInGroup.Text = I18N.GetString(chkBalanceInGroup.Text);//"Balance in group");
            for (int i = 0; i < cmbBalance.Items.Count; ++i)
            {
                _balanceIndexMap[i] = cmbBalance.Items[i].ToString();
                cmbBalance.Items[i] = I18N.GetString(cmbBalance.Items[i].ToString());
            }

            btnOK.Text = I18N.GetString(btnOK.Text);
            btnClose.Text = I18N.GetString(btnClose.Text);
            btnApply.Text = I18N.GetString(btnApply.Text);
        }

        private delegate void delegateConfigChanged(Object obj, EventArgs e);
        private void controller_ConfigChanged(object sender, EventArgs e)
        {
            List<string> list = (List<string>)((object[])sender)[0];
            if (list.Contains("All") || list.Contains(this.Name))
            {
                if (this.InvokeRequired)
                {
                    delegateConfigChanged adelegateConfigChanged = new delegateConfigChanged(controller_ConfigChanged);
                    this.Invoke(adelegateConfigChanged, new object[] { sender, e });
                    return;
                }
                LoadCurrentConfiguration();
            }
        }

        private void ShowWindow()
        {
            this.Opacity = 1;
            this.Show();
        }

        private int SaveOldSelectedServer()
        {
            try
            {
                int localPort = int.Parse(nudProxyPort.Text);
                Configuration.CheckPort(localPort);
                int ret = 0;
                settings.shareOverLan = chkShareOverLan.Checked;
                settings.localPort = localPort;
                settings.reconnectTimes = nudReconnect.Text.Length == 0 ? 0 : int.Parse(nudReconnect.Text);

                if (chkAutoStartup.Checked != AutoStartup.Check() && !AutoStartup.Set(chkAutoStartup.Checked))
                {
                    MessageBox.Show(I18N.GetString("Failed to update registry"));
                }
                settings.enableBalance = chkBalance.Checked;
                settings.balanceAlgorithm = cmbBalance.SelectedIndex >= 0 && cmbBalance.SelectedIndex < _balanceIndexMap.Count ? _balanceIndexMap[cmbBalance.SelectedIndex] : "OneByOne";
                settings.randomInGroup = chkBalanceInGroup.Checked;
                settings.TTL = Convert.ToInt32(nudTTL.Value);
                settings.connectTimeout = Convert.ToInt32(nudTimeout.Value);
                settings.dnsServer = txtDNS.Text;
                settings.localDnsServer = txtLocalDNS.Text;
                settings.proxyEnable = chkSockProxy.Checked;
                settings.pacDirectGoProxy = chkPacProxy.Checked;
                settings.proxyType = cmbProxyType.SelectedIndex;
                settings.proxyHost = txtS5Server.Text;
                settings.proxyPort = Convert.ToInt32(nudS5Port.Value);
                settings.proxyAuthUser = txtS5User.Text;
                settings.proxyAuthPass = txtS5Pass.Text;
                settings.proxyUserAgent = txtUserAgent.Text;
                settings.authUser = txtAuthUser.Text;
                settings.authPass = txtAuthPass.Text;

                settings.autoBan = chkAutoBan.Checked;
                settings.enableLogging = chkLoggingEnable.Checked;

                return ret;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            return -1; // ERROR
        }

        private void LoadSelectedServer()
        {
            chkShareOverLan.Checked = settings.shareOverLan;
            nudProxyPort.Value = settings.localPort;
            nudReconnect.Value = settings.reconnectTimes;

            chkAutoStartup.Checked = AutoStartup.Check();
            chkBalance.Checked = settings.enableBalance;
            int selectedIndex = 0;
            for (int i = 0; i < _balanceIndexMap.Count; ++i)
            {
                if (settings.balanceAlgorithm == _balanceIndexMap[i])
                {
                    selectedIndex = i;
                    break;
                }
            }
            cmbBalance.SelectedIndex = selectedIndex;
            chkBalanceInGroup.Checked = settings.randomInGroup;
            nudTTL.Value = settings.TTL;
            nudTimeout.Value = settings.connectTimeout;
            txtDNS.Text = settings.dnsServer;
            txtLocalDNS.Text = settings.localDnsServer;

            chkSockProxy.Checked = settings.proxyEnable;
            chkPacProxy.Checked = settings.pacDirectGoProxy;
            cmbProxyType.SelectedIndex = settings.proxyType;
            txtS5Server.Text = settings.proxyHost;
            nudS5Port.Value = settings.proxyPort;
            txtS5User.Text = settings.proxyAuthUser;
            txtS5Pass.Text = settings.proxyAuthPass;
            txtUserAgent.Text = settings.proxyUserAgent;
            txtAuthUser.Text = settings.authUser;
            txtAuthPass.Text = settings.authPass;

            chkAutoBan.Checked = settings.autoBan;
            chkLoggingEnable.Checked = settings.enableLogging;
        }

        private void LoadCurrentConfiguration()
        {
            settings = new Settings(controller.GetCurrentConfiguration());
            LoadSelectedServer();
        }

        private void BtnOK_Click(object sender, EventArgs e)
        {
            BtnApply_Click(null, null);
            this.Close();
        }

        private void BtnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void BtnApply_Click(object sender, EventArgs e)
        {
            controller.ConfigChanged -= controller_ConfigChanged;
            this.FormClosed -= new System.Windows.Forms.FormClosedEventHandler(this.SettingsForm_FormClosed);

            Configuration config = controller.GetCurrentConfiguration();
            if (SaveOldSelectedServer() == -1)
            {
                return;
            }
            else
            {
                settings.SaveTo(config);
            }
            Configuration.Save(config);
            controller.JustReload();
        }

        private void btnSetDefault_Click(object sender, EventArgs e)
        {
            if (chkSockProxy.Checked)
            {
                nudReconnect.Value = 4;
                nudTimeout.Value = 10;
                nudTTL.Value = 60;
            }
            else
            {
                nudReconnect.Value = 4;
                nudTimeout.Value = 5;
                nudTTL.Value = 60;
            }
        }

        public class Settings
        {
            public bool enableBalance;
            public bool shareOverLan;
            public int localPort;

            public string localDnsServer;
            public string dnsServer;
            public int reconnectTimes;
            public string balanceAlgorithm;
            public bool randomInGroup;
            public int TTL;
            public int connectTimeout;

            public bool proxyEnable;
            public bool pacDirectGoProxy;
            public int proxyType;
            public string proxyHost;
            public int proxyPort;
            public string proxyAuthUser;
            public string proxyAuthPass;
            public string proxyUserAgent;

            public string authUser;
            public string authPass;

            public bool autoBan;
            public bool enableLogging;

            public Settings(Configuration config)
            {
                enableBalance = config.enableBalance;
                shareOverLan = config.shareOverLan;
                localPort = config.localPort;
                localDnsServer = config.localDnsServer;
                dnsServer = config.dnsServer;
                reconnectTimes = config.reconnectTimes;
                balanceAlgorithm = config.balanceAlgorithm;
                randomInGroup = config.randomInGroup;
                TTL = config.TTL;
                connectTimeout = config.connectTimeout;
                proxyEnable = config.proxyEnable;
                pacDirectGoProxy = config.pacDirectGoProxy;
                proxyType = config.proxyType;
                proxyHost = config.proxyHost;
                proxyPort = config.proxyPort;
                proxyAuthUser = config.proxyAuthUser;
                proxyAuthPass = config.proxyAuthPass;
                proxyUserAgent = config.proxyUserAgent;
                authUser = config.authUser;
                authPass = config.authPass;
                autoBan = config.autoBan;
                enableLogging = config.enableLogging;
            }
            public void SaveTo(Configuration config)
            {
                config.enableBalance = enableBalance;
                config.shareOverLan = shareOverLan;
                config.localPort = localPort;
                config.localDnsServer = localDnsServer;
                config.dnsServer = dnsServer;
                config.reconnectTimes = reconnectTimes;
                config.balanceAlgorithm = balanceAlgorithm;
                config.randomInGroup = randomInGroup;
                config.TTL = TTL;
                config.connectTimeout = connectTimeout;
                config.proxyEnable = proxyEnable;
                config.pacDirectGoProxy = pacDirectGoProxy;
                config.proxyType = proxyType;
                config.proxyHost = proxyHost;
                config.proxyPort = proxyPort;
                config.proxyAuthUser = proxyAuthUser;
                config.proxyAuthPass = proxyAuthPass;
                config.proxyUserAgent = proxyUserAgent;
                config.authUser = authUser;
                config.authPass = authPass;
                config.autoBan = autoBan;
                config.enableLogging = enableLogging;
            }
        }
    }
}
