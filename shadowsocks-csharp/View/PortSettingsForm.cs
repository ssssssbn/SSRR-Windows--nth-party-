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
    public partial class PortSettingsForm : Form
    {
        private ShadowsocksController controller;
        //private Configuration _modifiedConfiguration;
        private Dictionary<string, PortMapConfig> portMap;
        private List<Server> readonlyAllServers;
        private int _oldSelectedIndex = -1;

        public PortSettingsForm(ShadowsocksController controller)
        {
            InitializeComponent();
            this.Icon = Icon.FromHandle(Resources.ssw128.GetHicon());
            this.controller = controller;
            controller.ConfigChanged += controller_ConfigChanged;

            UpdateTexts();

            comboBoxType.DisplayMember = "Text";
            comboBoxType.ValueMember = "Value";
            var items = new[]
            {
                new {Text = I18N.GetString("Port Forward"), Value = PortMapType.Forward},
                new {Text = I18N.GetString("Force Proxy"), Value = PortMapType.ForceProxy},
                new {Text = I18N.GetString("Proxy With Rule"), Value = PortMapType.RuleProxy}
            };
            comboBoxType.DataSource = items;

            LoadCurrentConfiguration();
        }

        private void UpdateTexts()
        {
            this.Text = I18N.GetString("Port Settings");
            groupBox1.Text = I18N.GetString("Map Setting");
            labelType.Text = I18N.GetString("Type");
            labelID.Text = I18N.GetString("Server ID");
            labelAddr.Text = I18N.GetString("Target Addr");
            labelPort.Text = I18N.GetString("Target Port");
            checkEnable.Text = I18N.GetString("Enable");
            labelLocal.Text = I18N.GetString("Local Port");
            label1.Text = I18N.GetString("Remarks");
            btnOK.Text = I18N.GetString(btnOK.Text);
            btnClose.Text = I18N.GetString(btnClose.Text);
            btnApply.Text = I18N.GetString(btnApply.Text);
            btnAdd.Text = I18N.GetString("&Add");
            btnDelete.Text = I18N.GetString("&Delete");
        }

        private void controller_ConfigChanged(object sender, EventArgs e)
        {
            List<string> list = (List<string>)((object[])sender)[0];
            if (list.Contains("All") || list.Contains(this.Name))
            {
                LoadCurrentConfiguration();
            }
        }

        private void PortMapForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            controller.ConfigChanged -= controller_ConfigChanged;
        }

        private void LoadCurrentConfiguration()
        {
            readonlyAllServers = controller.GetCurrentConfiguration().Servers;
            portMap = new Dictionary<string, PortMapConfig>();
            foreach (KeyValuePair<string, PortMapConfig> kvp in controller.GetCurrentConfiguration().portMap)
                portMap.Add(kvp.Key, kvp.Value.Clone());
            LoadPortSetting();
            LoadSelectedServer();
        }

        private void BtnOK_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void BtnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void BtnApply_Click(object sender, EventArgs e)
        {
            SaveSelectedServer();
            controller.SavePortConfig(portMap);

        }

        private void LoadPortSetting()
        {
            comboServers.Items.Clear();
            comboServers.Items.Add("");
            Dictionary<string, int> server_group = new Dictionary<string, int>();
            foreach (Server s in readonlyAllServers)
            {
                if (!string.IsNullOrEmpty(s.group) && !server_group.ContainsKey(s.group))
                {
                    comboServers.Items.Add("#" + s.group);
                    server_group[s.group] = 1;
                }
            }
            foreach (Server s in readonlyAllServers)
            {
                comboServers.Items.Add(GetDisplayText(s));
            }
            listPorts.Items.Clear();
            int[] list = new int[portMap.Count];
            int list_index = 0;
            foreach (KeyValuePair<string, PortMapConfig> it in portMap)
            {
                try
                {
                    list[list_index] = int.Parse(it.Key);
                }
                catch (FormatException)
                {

                }
                list_index += 1;
            }
            Array.Sort(list);
            for (int i = 0; i < list.Length; ++i)
            {
                string remarks = "";
                remarks = ((PortMapConfig)portMap[list[i].ToString()]).remarks ?? "";
                listPorts.Items.Add(list[i].ToString() + "    " + remarks);
            }
            _oldSelectedIndex = -1;
            if (listPorts.Items.Count > 0)
            {
                listPorts.SelectedIndex = 0;
            }
        }

        private string ServerListText2Key(string text)
        {
            if (text != null)
            {
                int pos = text.IndexOf(' ');
                if (pos > 0)
                    return text.Substring(0, pos);
            }
            return text;
        }

        private void SaveSelectedServer()
        {
            if (_oldSelectedIndex != -1)
            {
                bool reflash_list = false;
                string key = _oldSelectedIndex.ToString();
                if (key != NumLocalPort.Text)
                {
                    if (portMap.ContainsKey(key))
                    {
                        portMap.Remove(key);
                    }
                    reflash_list = true;
                    key = NumLocalPort.Text;
                    try
                    {
                        _oldSelectedIndex = int.Parse(key);
                    }
                    catch (FormatException)
                    {
                        _oldSelectedIndex = 0;
                    }
                }
                if (!portMap.ContainsKey(key))
                {
                    portMap[key] = new PortMapConfig();
                }
                PortMapConfig cfg = portMap[key] as PortMapConfig;

                cfg.enable = checkEnable.Checked;
                cfg.type = (PortMapType) comboBoxType.SelectedValue;
                cfg.id = GetID(comboServers.Text);
                cfg.server_addr = textAddr.Text;
                if (cfg.remarks != textRemarks.Text)
                {
                    reflash_list = true;
                }
                cfg.remarks = textRemarks.Text;
                cfg.server_port = Convert.ToInt32(NumTargetPort.Value);
                if (reflash_list)
                {
                    LoadPortSetting();
                }
            }
        }

        private void LoadSelectedServer()
        {
            string key = ServerListText2Key((string)listPorts.SelectedItem);
            Dictionary<string, int> server_group = new Dictionary<string, int>();
            foreach (Server s in readonlyAllServers)
            {
                if (!string.IsNullOrEmpty(s.group) && !server_group.ContainsKey(s.group))
                {
                    server_group[s.group] = 1;
                }
            }
            if (key != null && portMap.ContainsKey(key))
            {
                PortMapConfig cfg = portMap[key] as PortMapConfig;

                checkEnable.Checked = cfg.enable;
                comboBoxType.SelectedValue = cfg.type;
                string text = GetIDText(cfg.id);
                if (text.Length == 0 && server_group.ContainsKey(cfg.id))
                {
                    text = "#" + cfg.id;
                }
                comboServers.Text = text;
                NumLocalPort.Text = key;
                textAddr.Text = cfg.server_addr;
                NumTargetPort.Value = cfg.server_port;
                textRemarks.Text = cfg.remarks ?? "";

                try
                {
                    _oldSelectedIndex = int.Parse(key);
                }
                catch (FormatException)
                {
                    _oldSelectedIndex = 0;
                }
            }
        }

        private string GetID(string text)
        {
            if (text.IndexOf('#') >= 0)
            {
                return text.Substring(text.IndexOf('#') + 1);
            }
            return text;
        }

        private string GetDisplayText(Server s)
        {
            return (!string.IsNullOrEmpty(s.group) ? s.group + " - " : "    - ") + s.FriendlyName() + "        #" + s.id;
        }

        private string GetIDText(string id)
        {
            foreach (Server s in readonlyAllServers)
            {
                if (id == s.id)
                {
                    return GetDisplayText(s);
                }
            }
            return "";
        }

        private void listPorts_SelectedIndexChanged(object sender, EventArgs e)
        {
            SaveSelectedServer();
            LoadSelectedServer();
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            SaveSelectedServer();
            string key = "0";
            if (!portMap.ContainsKey(key))
            {
                portMap[key] = new PortMapConfig();
            }
            PortMapConfig cfg = portMap[key] as PortMapConfig;

            cfg.enable = checkEnable.Checked;
            cfg.type = (PortMapType) comboBoxType.SelectedValue;
            cfg.id = GetID(comboServers.Text);
            cfg.server_addr = textAddr.Text;
            cfg.remarks = textRemarks.Text;
            cfg.server_port = Convert.ToInt32(NumTargetPort.Value);

            _oldSelectedIndex = -1;
            LoadPortSetting();
            LoadSelectedServer();
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            string key = _oldSelectedIndex.ToString();
            if (portMap.ContainsKey(key))
            {
                portMap.Remove(key);
            }
            _oldSelectedIndex = -1;
            LoadPortSetting();
            LoadSelectedServer();
        }

        private void comboBoxType_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBoxType.SelectedIndex == 0)
            {
                textAddr.ReadOnly = false;
                NumTargetPort.ReadOnly = false;
                NumTargetPort.Increment = 1;
            }
            else
            {
                textAddr.ReadOnly = true;
                NumTargetPort.ReadOnly = true;
                NumTargetPort.Increment = 0;
            }
        }
    }
}
