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
        private int _oldselect_index = -1;

        private bool textBoxURLChanged = false;
        private bool buttonlongpress = false;

        delegate void DelegantTask();
        
        public SubscribeForm(ShadowsocksController controller)
        {
            this.Font = System.Drawing.SystemFonts.MessageBoxFont;
            InitializeComponent();

            this.Icon = Icon.FromHandle(Resources.ssw128.GetHicon());
            this.controller = controller;

            UpdateTexts();
            //controller.ConfigChanged += controller_ConfigChanged;

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
            ButtonAdd.Text = I18N.GetString(ButtonAdd.Text);
            ButtonDel.Text = I18N.GetString(ButtonDel.Text);
            ButtonOK.Text = I18N.GetString(ButtonOK.Text);
            ButtonCancel.Text = I18N.GetString(ButtonCancel.Text);
            labelLastUpdate.Text = I18N.GetString(labelLastUpdate.Text);
            labelUseProxy.Text = I18N.GetString(labelUseProxy.Text);
        }

        private void SubscribeForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            UpdateFreeNode.SubscribeFailureItemsArrayListInt = null;

            //controller.ConfigChanged -= controller_ConfigChanged;
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

        //private void controller_ConfigChanged(object sender, EventArgs e)
        //{
        //    //if (this.InvokeRequired)
        //    //    return;
        //    LoadCurrentConfiguration();
        //}

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
            int select_index = -1;
            checkBoxAutoUpdate.Checked = _modifiedConfiguration.nodeFeedAutoUpdate;
            checkBoxAutoLatency.Checked = _modifiedConfiguration.nodeFeedAutoLatency;
            checkBoxAutoUpdateUseProxy.Checked = _modifiedConfiguration.nodeFeedAutoUpdateUseProxy;
            checkBoxAutoUpdateTryUseProxy.Checked = _modifiedConfiguration.nodeFeedAutoUpdateTryUseProxy;
            UpdateList();
            UpdateSelected(select_index);
            //SetSelectIndex(select_index);
        }

        private int SaveAllSettings()
        {
            _modifiedConfiguration.nodeFeedAutoUpdate = checkBoxAutoUpdate.Checked;
            _modifiedConfiguration.nodeFeedAutoLatency = checkBoxAutoLatency.Checked;
            _modifiedConfiguration.nodeFeedAutoUpdateUseProxy = checkBoxAutoUpdateUseProxy.Checked;
            _modifiedConfiguration.nodeFeedAutoUpdateTryUseProxy = checkBoxAutoUpdateTryUseProxy.Checked;
            return 0;
        }

        private void ButtonCancel_Click(object sender, EventArgs e)
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

        private void ButtonOK_Click(object sender, EventArgs e)
        {
            //controller.ConfigChanged -= controller_ConfigChanged;
            this.FormClosed -= new System.Windows.Forms.FormClosedEventHandler(this.SubscribeForm_FormClosed);

            int select_index = listServerSubscribe.SelectedIndex;
            //不需管-1
            SaveSelected(select_index);
            if (SaveAllSettings() == -1)
            {
                return;
            }
            //controller.SaveServersConfig(_modifiedConfiguration);
            controller.SaveSubscribeConfig(_modifiedConfiguration);
            
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
            this.listServerSubscribe.BeginUpdate(); //数据更新，UI暂时挂起，直到EndUpdate绘制控件，可以有效避免闪烁并大大提高加载速度

            textBoxURLChanged = false;
            listServerSubscribe.Items.Clear();

            for (int i = 0; i < _modifiedConfiguration.serverSubscribes.Count; ++i)
            {
                string ItemStr = "";
                if (UpdateFreeNode.SubscribeFailureItemsArrayListInt != null && UpdateFreeNode.SubscribeFailureItemsArrayListInt[UpdateFreeNode.SubscribeFailureItemsArrayListInt.Length - 1] >= i)
                {
                    for (int j = 0; j < UpdateFreeNode.SubscribeFailureItemsArrayListInt.Length; j++)
                        if (i == UpdateFreeNode.SubscribeFailureItemsArrayListInt[j] - 1) 
                            ItemStr += "!";
                }

                ServerSubscribe ss = _modifiedConfiguration.serverSubscribes[i];
                ItemStr += ((i + 1).ToString() + "." + (String.IsNullOrEmpty(ss.Group) ? "   " : ss.Group + " - ")) + ss.URL;
                listServerSubscribe.Items.Add(ItemStr);
                
            }

            this.listServerSubscribe.EndUpdate();  //结束数据处理，UI界面一次性绘制。
        }

        private void SetSelectIndex(int index)
        {
            if (index >= -1 && index < _modifiedConfiguration.serverSubscribes.Count)
            {
                listServerSubscribe.SelectedIndexChanged -= listServerSubscribe_SelectedIndexChanged;
                listServerSubscribe.SelectedIndex = index;
                if (index == -1)
                    listServerSubscribe.SelectedItems.Clear();
                listServerSubscribe.SelectedIndexChanged += listServerSubscribe_SelectedIndexChanged;
            }
        }

        private void UpdateSelected(int index)
        {
            if(index==-1)
            {
                textBoxURL.ReadOnly = true;
                checkBoxUseProxy.Enabled = false;
                textBoxURL.Text = "";
                textBoxGroup.Text = "";
                textUpdate.Text = "(｢･ω･)｢";
                checkBoxUseProxy.Checked = false;
            }
            else if (index >= 0 && index < _modifiedConfiguration.serverSubscribes.Count)
            {
                textBoxURL.ReadOnly = false;
                checkBoxUseProxy.Enabled = true;
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
                if (checkBoxAutoUpdateUseProxy.Checked)
                    checkBoxUseProxy.Enabled = false;
                else
                    checkBoxUseProxy.Enabled = true;
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
                    textBoxURLChanged = true;
                }
                ss.UseProxy = checkBoxUseProxy.Checked;
            }
        }

        private void listServerSubscribe_SelectedIndexChanged(object sender, EventArgs e)
        {
            int select_index = listServerSubscribe.SelectedIndex;
            if (_oldselect_index == select_index || select_index==-1)
            {
                SaveSelected(_oldselect_index);
                if (_oldselect_index == -1 || select_index==-1)
                    return;
                _oldselect_index = -1;
                if(textBoxURLChanged)
                    UpdateList();
                UpdateSelected(-1);
                SetSelectIndex(-1);
            }
            else
            {
                //不需管-1
                SaveSelected(_oldselect_index);
                if (textBoxURLChanged)
                    UpdateList();
                UpdateSelected(select_index);
                SetSelectIndex(select_index);
                
                //if (listServerSubscribe.SelectedIndices.Count > 0)
                //{
                //    toolTip1.SetToolTip(listServerSubscribe, listServerSubscribe.Items[listServerSubscribe.SelectedIndex].ToString());
                //    toolTip1.Active = true;
                //}
                //else
                //{
                //    toolTip1.Active = false;
                //}
            }
        }

        private void ButtonAdd_Click(object sender, EventArgs e)
        {
            UpdateFreeNode.SubscribeFailureItemsArrayListInt = null;

            //不需管-1
            SaveSelected(_oldselect_index);
            int select_index = _modifiedConfiguration.serverSubscribes.Count;
            if (_oldselect_index >= 0 && _oldselect_index < _modifiedConfiguration.serverSubscribes.Count)
            {
                _modifiedConfiguration.serverSubscribes.Insert(_oldselect_index, new ServerSubscribe());
            }
            else
            {
                _oldselect_index = _modifiedConfiguration.serverSubscribes.Count;
                _modifiedConfiguration.serverSubscribes.Add(new ServerSubscribe());
            }
            UpdateList();
            UpdateSelected(_oldselect_index);
            SetSelectIndex(_oldselect_index);

            textBoxURL.Enabled = true;
        }

        private void ButtonDel_Click(object sender, EventArgs e)
        {
            UpdateFreeNode.SubscribeFailureItemsArrayListInt = null;

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
            if (checkBoxAutoUpdateUseProxy.Checked)
                checkBoxUseProxy.Enabled = false;
            else
                checkBoxUseProxy.Enabled = true;
        }

        private void checkBoxAutoUpdateTryUseProxy_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBoxAutoUpdateTryUseProxy.Checked && checkBoxAutoUpdateUseProxy.Checked)
                checkBoxAutoUpdateUseProxy.Checked = false;
        }

        private void ButtonDel_MouseDown(object sender, MouseEventArgs e)
        {
            timer_ButtonDel.Interval = 100;
            timer_ButtonDel.Enabled = true;
        }

        private void ButtonDel_MouseUp(object sender, MouseEventArgs e)
        {
            timer_ButtonDel.Enabled = false;
            if (buttonlongpress)
            {
                buttonlongpress = false;
                this.ButtonDel.Click += this.ButtonDel_Click;
            }
        }

        private void timer_ButtonDel_Tick(object sender, EventArgs e)
        {
            buttonlongpress = true;
            this.ButtonDel.Click -= this.ButtonDel_Click;
            ButtonDel_Click(null, null);
        }
    }
}
