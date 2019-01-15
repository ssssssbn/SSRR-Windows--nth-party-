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
    public partial class SubscribeForm : Form
    {
        private ShadowsocksController controller;
        // this is a copy of configuration that we are working on
        private Configuration _modifiedConfiguration;
        private int _oldselect_index;

        delegate void DelegantTask();

        public SubscribeForm(ShadowsocksController controller)
        {
            this.Font = System.Drawing.SystemFonts.MessageBoxFont;
            InitializeComponent();

            this.Icon = Icon.FromHandle(Resources.ssw128.GetHicon());
            this.controller = controller;

            UpdateTexts();
            controller.ConfigChanged += controller_ConfigChanged;

            LoadCurrentConfiguration();
        }

        private void UpdateTexts()
        {
            this.Text = I18N.GetString(this.Text);
            labelURL.Text = I18N.GetString(labelURL.Text);
            labelGroupName.Text = I18N.GetString(labelGroupName.Text);
            checkBoxAutoUpdate.Text = I18N.GetString(checkBoxAutoUpdate.Text);
            checkBoxAutoLatency.Text = I18N.GetString(checkBoxAutoLatency.Text);
            checkBoxAutoUpdateUseProxy.Text = I18N.GetString(checkBoxAutoUpdateUseProxy.Text);
            checkBoxAutoUpdateTryUseProxy.Text = I18N.GetString(checkBoxAutoUpdateTryUseProxy.Text);
            buttonOK.Text = I18N.GetString(buttonOK.Text);
            buttonCancel.Text = I18N.GetString(buttonCancel.Text);
            labelLastUpdate.Text = I18N.GetString(labelLastUpdate.Text);
            labelUseProxy.Text = I18N.GetString(labelUseProxy.Text);
        }

        private void SubscribeForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            controller.ConfigChanged -= controller_ConfigChanged;
            if (Program._viewController.UpdateFreeNodeInterrupt)
            {
                Program._viewController.UpdateFreeNodeInterrupt = false;
                Program._viewController.startCheckFreeNode();
            }
            else if (Program._viewController.UpdateLatencyInterrupt)
            {
                Program._viewController.UpdateLatencyInterrupt = false;
                Program._viewController.startUpdateLatency();
            }
        }

        private void controller_ConfigChanged(object sender, EventArgs e)
        {
            //if (this.InvokeRequired)
            //    return;
            LoadCurrentConfiguration();
        }

        private void LoadCurrentConfiguration()
        {
            _modifiedConfiguration = controller.GetConfiguration();
            LoadAllSettings();
            if (listServerSubscribe.Items.Count == 0)
            {
                textBoxURL.Enabled = false;
            }
            else
            {
                textBoxURL.Enabled = true;
            }
        }

        private void LoadAllSettings()
        {
            int select_index = 0;
            checkBoxAutoUpdate.Checked = _modifiedConfiguration.nodeFeedAutoUpdate;
            checkBoxAutoLatency.Checked = _modifiedConfiguration.nodeFeedAutoLatency;
            checkBoxAutoUpdateUseProxy.Checked = _modifiedConfiguration.nodeFeedAutoUpdateUseProxy;
            checkBoxAutoUpdateTryUseProxy.Checked = _modifiedConfiguration.nodeFeedAutoUpdateTryUseProxy;
            UpdateList();
            UpdateSelected(select_index);
            SetSelectIndex(select_index);
        }

        private int SaveAllSettings()
        {
            _modifiedConfiguration.nodeFeedAutoUpdate = checkBoxAutoUpdate.Checked;
            _modifiedConfiguration.nodeFeedAutoLatency = checkBoxAutoLatency.Checked;
            _modifiedConfiguration.nodeFeedAutoUpdateUseProxy = checkBoxAutoUpdateUseProxy.Checked;
            _modifiedConfiguration.nodeFeedAutoUpdateTryUseProxy = checkBoxAutoUpdateTryUseProxy.Checked;
            return 0;
        }

        private void buttonCancel_Click(object sender, EventArgs e)
        {
            //if (Program._viewController.UpdateFreeNodeInterrupt)
            //{
            //    Program._viewController.UpdateFreeNodeInterrupt = false;
            //    Program._viewController.subScribeFormCheckFreeNode();
            //}
            //else if (Program._viewController.UpdateLatencyInterrupt)
            //{
            //    Program._viewController.UpdateLatencyInterrupt = false;
            //    Program._viewController.subScribeFormUpdateLatency();
            //}
            this.Close();
        }

        private void buttonOK_Click(object sender, EventArgs e)
        {
            controller.ConfigChanged -= controller_ConfigChanged;
            this.FormClosed -= new System.Windows.Forms.FormClosedEventHandler(this.SubscribeForm_FormClosed);

            int select_index = listServerSubscribe.SelectedIndex;
            SaveSelected(select_index);
            if (SaveAllSettings() == -1)
            {
                return;
            }
            controller.SaveServersConfig(_modifiedConfiguration);
            
            if (Program._viewController.UpdateFreeNodeInterrupt || checkBoxAutoUpdate.Checked)
            {
                Program._viewController.UpdateFreeNodeInterrupt = false;
                Program._viewController.startCheckFreeNode();
            }
            else if (Program._viewController.UpdateLatencyInterrupt || checkBoxAutoLatency.Checked)
            {
                Program._viewController.UpdateLatencyInterrupt = false;
                Program._viewController.startUpdateLatency();
            }
            this.Close();
        }

        private void UpdateList()
        {
            listServerSubscribe.Items.Clear();
            for (int i = 0; i < _modifiedConfiguration.serverSubscribes.Count; ++i)
            {
                ServerSubscribe ss = _modifiedConfiguration.serverSubscribes[i];
                listServerSubscribe.Items.Add((String.IsNullOrEmpty(ss.Group) ? "    " : ss.Group + " - ") + ss.URL);
            }
        }

        private void SetSelectIndex(int index)
        {
            if (index >= 0 && index < _modifiedConfiguration.serverSubscribes.Count)
            {
                listServerSubscribe.SelectedIndex = index;
            }
        }

        private void UpdateSelected(int index)
        {
            if (index >= 0 && index < _modifiedConfiguration.serverSubscribes.Count)
            {
                ServerSubscribe ss = _modifiedConfiguration.serverSubscribes[index];
                textBoxURL.Text = ss.URL;
                textBoxGroup.Text = ss.Group;
                _oldselect_index = index;
                if (ss.LastUpdateTime != 0)
                {
                    DateTime now = new DateTime(1970, 1, 1, 0, 0, 0);
                    now = now.AddSeconds(ss.LastUpdateTime);
                    textUpdate.Text = now.ToLongDateString() + " " + now.ToLongTimeString();
                }
                else
                {
                    textUpdate.Text = "(｢･ω･)｢";
                }
                checkBoxUseProxy.Checked = ss.UseProxy;
            }
        }

        private void SaveSelected(int index)
        {
            if (index >= 0 && index < _modifiedConfiguration.serverSubscribes.Count)
            {
                ServerSubscribe ss = _modifiedConfiguration.serverSubscribes[index];
                if (ss.URL != textBoxURL.Text)
                {
                    ss.URL = textBoxURL.Text;
                    ss.Group = "";
                    ss.LastUpdateTime = 0;
                }
                ss.UseProxy = checkBoxUseProxy.Checked;
            }
        }

        private void listServerSubscribe_SelectedIndexChanged(object sender, EventArgs e)
        {
            int select_index = listServerSubscribe.SelectedIndex;
            if (_oldselect_index == select_index)
                return;

            SaveSelected(_oldselect_index);
            UpdateList();
            UpdateSelected(select_index);
            SetSelectIndex(select_index);
        }

        private void buttonAdd_Click(object sender, EventArgs e)
        {
            SaveSelected(_oldselect_index);
            int select_index = _modifiedConfiguration.serverSubscribes.Count;
            if (_oldselect_index >= 0 && _oldselect_index < _modifiedConfiguration.serverSubscribes.Count)
            {
                _modifiedConfiguration.serverSubscribes.Insert(select_index, new ServerSubscribe());
            }
            else
            {
                _modifiedConfiguration.serverSubscribes.Add(new ServerSubscribe());
            }
            UpdateList();
            UpdateSelected(select_index);
            SetSelectIndex(select_index);

            textBoxURL.Enabled = true;
        }

        private void buttonDel_Click(object sender, EventArgs e)
        {
            int select_index = listServerSubscribe.SelectedIndex;
            if (select_index >= 0 && select_index < _modifiedConfiguration.serverSubscribes.Count)
            {
                _modifiedConfiguration.serverSubscribes.RemoveAt(select_index);
                if (select_index >= _modifiedConfiguration.serverSubscribes.Count)
                {
                    select_index = _modifiedConfiguration.serverSubscribes.Count - 1;
                }
                UpdateList();
                UpdateSelected(select_index);
                SetSelectIndex(select_index);
            }
            if (listServerSubscribe.Items.Count == 0)
            {
                textBoxURL.Enabled = false;
            }
        }

        private void checkBoxAutoUpdateUseProxy_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBoxAutoUpdateUseProxy.Checked && checkBoxAutoUpdateTryUseProxy.Checked)
                checkBoxAutoUpdateTryUseProxy.Checked = false;
        }

        private void checkBoxAutoUpdateTryUseProxy_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBoxAutoUpdateTryUseProxy.Checked && checkBoxAutoUpdateUseProxy.Checked)
                checkBoxAutoUpdateUseProxy.Checked = false;
        }
    }
}
