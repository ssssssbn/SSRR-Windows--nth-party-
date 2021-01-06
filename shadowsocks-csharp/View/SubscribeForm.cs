using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using Shadowsocks.Controller;
using Shadowsocks.Model;
using Shadowsocks.Properties;

namespace Shadowsocks.View
{
    public partial class SubscribeForm : Form
    {
        private ShadowsocksController controller;
        private Settings settings;
        private int _oldselect_index = -1;

        private bool lbServerSubscribeMultiselected = false;
        private bool configChanged = false;

        private List<KeyInfoOfTheSubscription> KIOTS;
        private List<int> listnewUpdateAboutSubscribes;
        private List<DelSubscribes> listDelSubscribes;

        private CurrentlyEdited currentlyEdited;

        private Mutex mtExec;
        private bool allowCheck = false;

        public SubscribeForm(ShadowsocksController _controller)
        {
            this.Font = System.Drawing.SystemFonts.MessageBoxFont;
            InitializeComponent();
            listServerSubscribe.MouseWheel += listServerSubscribe_MouseWheel;
            KIOTS = new List<KeyInfoOfTheSubscription>();
            listnewUpdateAboutSubscribes = new List<int>();
            listDelSubscribes = new List<DelSubscribes>();
            mtExec = new Mutex();

            this.DoubleBuffered = true;

            this.Icon = Icon.FromHandle(Resources.ssw128.GetHicon());
            this.controller = _controller;

            UpdateTexts();

            controller.ConfigChanged += controller_ConfigChanged;

            LoadConfiguration();
            allowCheck = true;
        }

        private void UpdateTexts()
        {
            this.Text = I18N.GetString(this.Text);
            GroupSubscribeDetail.Text = I18N.GetString(GroupSubscribeDetail.Text);
            labelURL.Text = I18N.GetString(labelURL.Text);
            labelGroupName.Text = I18N.GetString(labelGroupName.Text);
            checkBoxAutoUpdate.Text = I18N.GetString(checkBoxAutoUpdate.Text);
            lblevery1.Text = I18N.GetString(lblevery1.Text);
            lblhours1.Text = I18N.GetString(lblhours1.Text);
            lblevery2.Text = I18N.GetString(lblevery2.Text);
            lblhours2.Text = I18N.GetString(lblhours2.Text);
            checkBoxAutoLatency.Text = I18N.GetString(checkBoxAutoLatency.Text);
            checkBoxAutoUpdateUseProxy.Text = I18N.GetString(checkBoxAutoUpdateUseProxy.Text);
            checkBoxAutoUpdateTryUseProxy.Text = I18N.GetString(checkBoxAutoUpdateTryUseProxy.Text);
            chkBSortServersBySubscriptionsOrder.Text = I18N.GetString(chkBSortServersBySubscriptionsOrder.Text);
            btnAdd.Text = I18N.GetString(btnAdd.Text);
            btnDelete.Text = I18N.GetString(btnDelete.Text);
            btnDeleteIncludeServers.Text = I18N.GetString(btnDeleteIncludeServers.Text);
            btnUp.Text = I18N.GetString(btnUp.Text);
            btnDown.Text = I18N.GetString(btnDown.Text);
            btnOK.Text = I18N.GetString(btnOK.Text);
            btnClose.Text = I18N.GetString(btnClose.Text);
            btnApply.Text = I18N.GetString(btnApply.Text);
            labelLastUpdate.Text = I18N.GetString(labelLastUpdate.Text);
            labelJoinUpdate.Text = I18N.GetString(labelJoinUpdate.Text);
            labelUseProxy.Text = I18N.GetString(labelUseProxy.Text);
            labelDontUseProxy.Text = I18N.GetString(labelDontUseProxy.Text);

            UpdateSelectedToolStripMenuItem.Text = I18N.GetString(UpdateSelectedToolStripMenuItem.Text);
            UpdateSelectedUseProxyToolStripMenuItem.Text = I18N.GetString(UpdateSelectedUseProxyToolStripMenuItem.Text);
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
                mtExec.WaitOne();
                MergeConfig();
                mtExec.ReleaseMutex();
            }
        }

        private void MergeConfig()
        {
            try
            {
                allowCheck = false;
                Configuration config = controller.GetCurrentConfiguration();

                StoreCurrentlyEdited();
                List<ServerSubscribe> tmpss = new List<ServerSubscribe>();

                if (!btnApply.Enabled)
                {
                    tmpss = settings.serverSubscribes;

                    settings = new Settings(config);
                    int serverSubscribesCount = settings.serverSubscribes.Count;
                    if (serverSubscribesCount > 0)
                        for (int i = 0; i < serverSubscribesCount; i++)
                        {
                            if (!CompareServerSubscribe(settings.serverSubscribes[i], tmpss[i]) && listnewUpdateAboutSubscribes.FindIndex(t => t == i) < 0)
                            {
                                listnewUpdateAboutSubscribes.Add(i);
                            }
                        }
                }
                else
                {
                    tmpss = settings.serverSubscribes;

                    settings = new Settings(config);
                    List<ServerSubscribe> tmp = settings.serverSubscribes;
                    settings.serverSubscribes = tmpss;
                    tmpss = tmp;

                    //删除非法重复订阅
                    for (int i = 0; i < tmpss.Count; i++)
                    {
                        if (tmpss[i].URL == "")
                        {
                            tmpss.RemoveAt(i--);
                            continue;
                        }
                        List<string> listrepeated = Util.Utils.IsServerSubscriptionRepeat(tmpss, tmpss[i].URL);
                        foreach (string id in listrepeated)
                        {
                            tmpss.RemoveAt(tmpss.FindIndex(t => t.id == id));
                            i--;
                        }
                    }


                    //查取删除订阅
                    for (int i = 0; i < tmpss.Count; i++)
                    {
                        int index = -1;
                        index = settings.serverSubscribes.FindIndex(t => t.URL == tmpss[i].id);
                        if (index > -1)
                        {
                            if (settings.serverSubscribes[index].LastUpdateTime != tmpss[i].LastUpdateTime)
                            {
                                settings.serverSubscribes[index].Group = tmpss[i].Group;
                                settings.serverSubscribes[index].LastUpdateTime = tmpss[i].LastUpdateTime;
                                if (listnewUpdateAboutSubscribes.FindIndex(t => t == index) < 0)
                                    listnewUpdateAboutSubscribes.Add(index);
                            }
                        }
                    }
                }

                UpdateList();
                UpdateSelected(currentlyEdited.selectindex);
                try
                {
                    listServerSubscribe.SelectedIndexChanged -= listServerSubscribe_SelectedIndexChanged;
                    if (btnApply.Enabled)
                    {
                        RestoreCurrentlyEdited(2);
                    }
                    else
                        RestoreCurrentlyEdited(1);

                }
                catch (Exception e)
                {
                    throw e;
                }
                finally
                {
                    listServerSubscribe.SelectedIndexChanged += listServerSubscribe_SelectedIndexChanged;
                }
            }
            catch (Exception e)
            {
                Logging.LogUsefulException(e);
                BtnClose_Click(null, null);
            }
            finally
            {
                allowCheck = true;
                chkSettingChanged();
            }
            //}

        }

        private void LoadConfiguration()
        {
            settings = new Settings(controller.GetCurrentConfiguration());
            if (settings.serverSubscribes.Count > 6)
                listServerSubscribe.Height = 11 * listServerSubscribe.ItemHeight + 4;
            LoadAllSettings();
        }

        private void LoadAllSettings()
        {
            int select_index = -1;
            checkBoxAutoUpdate.Checked = settings.nodeFeedAutoUpdate;
            cmbAutoUpdateInterval.SelectedIndex = cmbAutoUpdateInterval.FindStringExact(settings.nodeFeedAutoUpdateInterval.ToString());
            checkBoxAutoLatency.Checked = settings.nodeFeedAutoLatency;
            cmbAutoLatencyInterval.SelectedIndex = cmbAutoLatencyInterval.FindStringExact(((float)settings.nodeFeedAutoLatencyInterval / 60).ToString());
            checkBoxAutoUpdateUseProxy.Checked = settings.nodeFeedAutoUpdateUseProxy;
            checkBoxAutoUpdateTryUseProxy.Checked = settings.nodeFeedAutoUpdateTryUseProxy;
            chkBSortServersBySubscriptionsOrder.Checked = settings.sortServersBySubscriptionsOrder;
            UpdateList();
            UpdateSelected(select_index);
        }

        private void SaveAllSettings()
        {
            Configuration newconfig = controller.GetCurrentConfiguration();
            settings.SaveTo(newconfig, ref listDelSubscribes);

            Configuration.Save(newconfig);
            controller.JustReload();
        }

        private void BtnClose_Click(object sender, EventArgs e)
        {
            mtExec.WaitOne();
            mtExec.ReleaseMutex();
            this.Close();
        }

        private void BtnOK_Click(object sender, EventArgs e)
        {
            mtExec.WaitOne();
            allowCheck = false;
            if (btnApply.Enabled)
                BtnApply_Click(true, null);

            if (checkBoxAutoUpdate.Checked && configChanged)
            {
                configChanged = false;
                controller.InvokeUpdateNodeFromSubscribeForm(null, null);
            }

            allowCheck = true;
            mtExec.ReleaseMutex();
            this.Close();
        }

        private void BtnApply_Click(object sender, EventArgs e)
        {
            if (typeof(bool) != sender.GetType())
            {
                mtExec.WaitOne();
                allowCheck = false;
            }
            controller.ConfigChanged -= controller_ConfigChanged;
            try
            {
                SaveAllSettings();
                btnApply.Enabled = false;
            }
            catch (Exception ex)
            {
                Logging.Log(LogLevel.Error, String.Format("Operation {0} error:" + System.Environment.NewLine + "{1}", sender != null ? (typeof(bool) == sender.GetType() ? btnOK.Name : btnApply.Name) : btnApply.Name, ex.Message));
            }
            finally
            {
                controller.ConfigChanged += controller_ConfigChanged;
                if (typeof(bool) != sender.GetType())
                {
                    allowCheck = true;
                    mtExec.ReleaseMutex();
                }
            }
        }

        private void UpdateList()
        {
            this.listServerSubscribe.BeginUpdate(); //数据更新，UI暂时挂起，直到EndUpdate绘制控件，可以有效避免闪烁并大大提高加载速度

            // Create a Graphics object to use when determining the size of the largest item in the ListBox.
            Graphics g = listServerSubscribe.CreateGraphics();

            listServerSubscribe.Items.Clear();
            KIOTS.Clear();

            for (int i = 0; i < settings.serverSubscribes.Count; ++i)
            {

                ServerSubscribe ss = settings.serverSubscribes[i];
                ss.index = i;
                listServerSubscribe.Items.Add(GenerateListInfo(g, ss));

                KeyInfoOfTheSubscription kiots;
                kiots.Name = ss.Group;
                kiots.Host = Util.Utils.GetHostFromUrl(ss.URL);
                KIOTS.Add(kiots);
            }

            this.listServerSubscribe.EndUpdate();  //结束数据处理，UI界面一次性绘制。
        }

        private string GenerateListInfo(Graphics g, ServerSubscribe ss)
        {
            int hzSize = 0;
            int ItemHeight = 0;
            StringBuilder sb = new StringBuilder();
            sb.Append(((ss.index + 1).ToString() + "." + ss.Group + System.Environment.NewLine + "    ") + ss.URL);
            hzSize = (int)g.MeasureString(sb.ToString(), listServerSubscribe.Font).Width;
            if (listServerSubscribe.HorizontalExtent < hzSize)
                listServerSubscribe.HorizontalExtent = hzSize;
            ItemHeight = (int)g.MeasureString(sb.ToString(), listServerSubscribe.Font).Height;
            if (listServerSubscribe.ItemHeight < ItemHeight)
                listServerSubscribe.ItemHeight = ItemHeight;
            return sb.ToString();
        }

        private void UpdateSelected(int index)
        {
            if (index < 0)
            {
                tBURL.Enabled = false;
                tBGroup.Enabled = false;
                tBUpdate.Enabled = false;
                chkBJoinUpdate.Enabled = false;
                chkBUseProxy.Enabled = false;
                chkBDontUseProxy.Enabled = false;
                tBURL.Text = "";
                tBGroup.Text = "";
                tBUpdate.Text = "(｢･ω･)｢";
                chkBJoinUpdate.Checked = false;
                chkBUseProxy.Checked = false;
                chkBDontUseProxy.Checked = false;
                _oldselect_index = -1;
            }
            else if (index >= 0 && index < settings.serverSubscribes.Count)
            {
                tBURL.Enabled = true;
                tBGroup.Enabled = true;
                tBUpdate.Enabled = true;
                chkBJoinUpdate.Enabled = true;
                chkBUseProxy.Enabled = true;
                chkBDontUseProxy.Enabled = true;
                ServerSubscribe ss = settings.serverSubscribes[index];
                tBURL.Text = ss.URL;
                tBGroup.Text = ss.Group;
                _oldselect_index = index;
                if (ss.LastUpdateTime != 0)
                {
                    DateTime now = new DateTime(1970, 1, 1, 0, 0, 0);
                    now = now.AddSeconds(ss.LastUpdateTime);
                    tBUpdate.Text = now.ToLongDateString() + " " + now.ToLongTimeString();
                }
                else
                {
                    tBUpdate.Text = "(｢･ω･)｢";
                }
                chkBJoinUpdate.Checked = ss.JoinUpdate;
                chkBUseProxy.Checked = ss.UseProxy;
                chkBDontUseProxy.Checked = ss.DontUseProxy;
            }
            if (index > -2 && index < settings.serverSubscribes.Count)
            {
                listServerSubscribe.SelectedIndexChanged -= listServerSubscribe_SelectedIndexChanged;
                if (index == -1)
                    listServerSubscribe.SelectedItems.Clear();
                else
                    listServerSubscribe.SelectedIndex = index;
                listServerSubscribe.SelectedIndexChanged += listServerSubscribe_SelectedIndexChanged;
            }

        }

        private void listServerSubscribe_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!allowCheck)
                return;
            //新变化
            bool needrefresh = false;
            if (listServerSubscribe.SelectedItems.Count > 0)
            {
                foreach (int i in listServerSubscribe.SelectedIndices)
                {
                    int index = listnewUpdateAboutSubscribes.FindIndex(t => t == i);
                    if (index > -1)
                    {
                        listnewUpdateAboutSubscribes.RemoveAt(index);
                        needrefresh = true;
                    }
                }
            }
            if (needrefresh)
                listServerSubscribe.Invalidate(true);


            mtExec.WaitOne();
            allowCheck = false;

            try
            {
                if (listServerSubscribe.SelectedItems.Count > 1)
                {
                    if (!lbServerSubscribeMultiselected)
                    {
                        UpdateSelected(-2);
                    }
                    lbServerSubscribeMultiselected = true;
                    btnAdd.Enabled = false;

                    return;
                }
                int select_index = listServerSubscribe.SelectedIndex;
                if (select_index >= 0 && select_index < listServerSubscribe.Items.Count)
                {
                    if (_oldselect_index == select_index)
                    {
                        return;
                    }
                    else
                    {
                        UpdateSelected(select_index);
                    }
                    lbServerSubscribeMultiselected = false;
                    btnAdd.Enabled = true;
                }
            }
            catch (Exception ex)
            {
                Logging.Log(LogLevel.Error, String.Format("Operation {0} error:" + System.Environment.NewLine + "{1}", "SelectedIndexChanged", ex.Message));
            }
            finally
            {
                allowCheck = true;
                mtExec.ReleaseMutex();
            }
        }

        private void BtnAdd_Click(object sender, EventArgs e)
        {
            mtExec.WaitOne();
            allowCheck = false;
            try
            {
                if (_oldselect_index >= 0 && _oldselect_index < settings.serverSubscribes.Count)
                {
                    settings.serverSubscribes.Insert(_oldselect_index, Configuration.CopySubscribes(settings.serverSubscribes[_oldselect_index]));
                    _oldselect_index++;
                }
                else
                {
                    _oldselect_index = settings.serverSubscribes.Count;
                    ServerSubscribe ss = null;
                    while (ss == null || settings.serverSubscribes.FindIndex(t => t.id == ss.id) != -1)
                        ss = new ServerSubscribe();
                    settings.serverSubscribes.Add(ss);
                }
                int lsstopindex = listServerSubscribe.TopIndex;
                UpdateList();
                int displayitemscount = (listServerSubscribe.Height - 4) / listServerSubscribe.ItemHeight - 1;
                if (settings.serverSubscribes.Count > displayitemscount)
                {
                    int tmp = settings.serverSubscribes.Count - (settings.serverSubscribes.Count == displayitemscount ? displayitemscount - 3 : displayitemscount - 2);
                    if (_oldselect_index < tmp)
                        listServerSubscribe.TopIndex = lsstopindex;
                    else
                        listServerSubscribe.TopIndex = tmp;
                }

                UpdateSelected(_oldselect_index);
            }
            catch (Exception ex)
            {
                Logging.Log(LogLevel.Error, String.Format("Operation {0} error:" + System.Environment.NewLine + "{1}", btnAdd.Name, ex.Message));
            }
            finally
            {
                allowCheck = true;
                chkSettingChanged();
                if (!timer_btnLongPress.LongPress)
                    listServerSubscribe.Focus();
                mtExec.ReleaseMutex();
            }
        }
        private void BtnDelete_Click(object sender, EventArgs e)
        {
            int lssic = listServerSubscribe.SelectedItems.Count;
            if (lssic < 1)
                return;

            mtExec.WaitOne();
            allowCheck = false;
            try
            {
                int select_index = -1;
                bool ischanged = false;
                for (int j = lssic - 1; j > -1; j--)
                {
                    select_index = listServerSubscribe.SelectedIndices[j];
                    if (select_index >= 0 && select_index < settings.serverSubscribes.Count)
                    {
                        if (sender != null && typeof(bool) == sender.GetType())
                        {
                            listDelSubscribes.Add(new DelSubscribes(settings.serverSubscribes[select_index].id, true));
                        }
                        else
                            listDelSubscribes.Add(new DelSubscribes(settings.serverSubscribes[select_index].id));

                        settings.serverSubscribes.RemoveAt(select_index);
                        if (select_index >= settings.serverSubscribes.Count)
                        {
                            select_index = settings.serverSubscribes.Count - 1;
                        }
                        ischanged = true;
                    }
                }
                if (ischanged)
                {
                    int tmpTopIndex = listServerSubscribe.TopIndex;
                    UpdateList();
                    listServerSubscribe.TopIndex = tmpTopIndex;
                    UpdateSelected(select_index);
                }

                if (lbServerSubscribeMultiselected)
                {
                    lbServerSubscribeMultiselected = false;
                    btnAdd.Enabled = true;
                    UpdateSelected(-1);
                }
            }
            catch (Exception ex)
            {
                Logging.Log(LogLevel.Error, String.Format("Operation {0} error:" + System.Environment.NewLine + "{1}", sender != null ? (typeof(bool) == sender.GetType() ? btnDeleteIncludeServers.Name : btnDelete.Name) : btnDelete.Name, ex.Message));
            }
            finally
            {
                allowCheck = true;
                chkSettingChanged();
                if (!timer_btnLongPress.LongPress)
                    listServerSubscribe.Focus();
                mtExec.ReleaseMutex();
            }
        }
        private void BtnDeleteIncludeNode_Click(object sender, EventArgs e)
        {
            BtnDelete_Click(true, null);
        }
        private void BtnUp_Click(object sender, EventArgs e)
        {
            if (listServerSubscribe.SelectedIndices.Count == 0) 
                return;
            mtExec.WaitOne();
            allowCheck = false;
            listServerSubscribe.BeginUpdate();
            try
            {
                listServerSubscribe.SelectedIndexChanged -= listServerSubscribe_SelectedIndexChanged;

                int[] array = new int[listServerSubscribe.SelectedIndices.Count];
                listServerSubscribe.SelectedIndices.CopyTo(array, 0);
                Array.Sort(array);
                listServerSubscribe.ClearSelected();
                for (int i = 0; i < array.Length; i++)
                {
                    if (array[i] == 0 || (i > 0 && array[i] == array[i - 1] + 1))
                    {
                        listServerSubscribe.SelectedIndex = array[i];
                        continue;
                    }
                    int sncurrent = settings.serverSubscribes[array[i]].index;
                    int snnext = settings.serverSubscribes[array[i] - 1].index;
                    settings.serverSubscribes[array[i]].index = snnext;
                    settings.serverSubscribes[array[i] - 1].index = sncurrent;
                    settings.serverSubscribes.Reverse(array[i] - 1, 2);
                    listServerSubscribe.Items[array[i]] = GenerateListInfo(listServerSubscribe.CreateGraphics(), settings.serverSubscribes[array[i]]);
                    listServerSubscribe.Items[array[i] - 1] = GenerateListInfo(listServerSubscribe.CreateGraphics(), settings.serverSubscribes[array[i] - 1]);
                    listServerSubscribe.SelectedIndex = --array[i];
                    if (array.Length == 1)
                        _oldselect_index--;
                }
            }
            catch (Exception ex)
            {
                Logging.Log(LogLevel.Error, String.Format("Operation {0} error:" + System.Environment.NewLine + "{1}", btnUp.Name, ex.Message));
            }
            finally
            {
                listServerSubscribe.EndUpdate();
                listServerSubscribe.SelectedIndexChanged += listServerSubscribe_SelectedIndexChanged;
                allowCheck = true;
                chkSettingChanged();
                if (!timer_btnLongPress.LongPress)
                    listServerSubscribe.Focus();
                mtExec.ReleaseMutex();
            }
        }

        private void BtnDown_Click(object sender, EventArgs e)
        {
            if (listServerSubscribe.SelectedIndices.Count == 0)
                return;
            mtExec.WaitOne();
            allowCheck = false;
            listServerSubscribe.BeginUpdate();
            try
            {
                listServerSubscribe.SelectedIndexChanged -= listServerSubscribe_SelectedIndexChanged;

                int[] array = new int[listServerSubscribe.SelectedIndices.Count];
                listServerSubscribe.SelectedIndices.CopyTo(array, 0);
                Array.Sort(array);
                listServerSubscribe.ClearSelected();
                for (int i = array.Length - 1; i > -1; i--)
                {
                    if (array[i] == settings.serverSubscribes.Count - 1 || (i < array.Length - 1 && array[i] == array[i + 1] - 1))
                    {
                        listServerSubscribe.SelectedIndex = array[i];
                        continue;
                    }
                    int sncurrent = settings.serverSubscribes[array[i]].index;
                    int snnext = settings.serverSubscribes[array[i] + 1].index;
                    settings.serverSubscribes[array[i]].index = snnext;
                    settings.serverSubscribes[array[i] + 1].index = sncurrent;
                    settings.serverSubscribes.Reverse(array[i], 2);
                    listServerSubscribe.Items[array[i]] = GenerateListInfo(listServerSubscribe.CreateGraphics(), settings.serverSubscribes[array[i]]);
                    listServerSubscribe.Items[array[i] + 1] = GenerateListInfo(listServerSubscribe.CreateGraphics(), settings.serverSubscribes[array[i] + 1]);

                    listServerSubscribe.SelectedIndex = ++array[i];
                    if (array.Length == 1)
                        _oldselect_index++;
                }
            }
            catch (Exception ex)
            {
                Logging.Log(LogLevel.Error, String.Format("Operation {0} error:" + System.Environment.NewLine + "{1}", btnDown.Name, ex.Message));
            }
            finally
            {
                listServerSubscribe.EndUpdate();
                listServerSubscribe.SelectedIndexChanged += listServerSubscribe_SelectedIndexChanged;
                allowCheck = true;
                chkSettingChanged();
                if (!timer_btnLongPress.LongPress)
                    listServerSubscribe.Focus();
                mtExec.ReleaseMutex();
            }
        }
        private void Btn_MouseDown(object sender, MouseEventArgs e)
        {
            Button btn = (Button)sender;
            if (sender.Equals(this.btnUp))
            {
                timer_btnLongPress.TriggerSource = btnUp.Name;
            }
            else if (sender.Equals(this.btnDown))
            {
                timer_btnLongPress.TriggerSource = btnDown.Name;
            }
            else if (sender.Equals(this.btnDelete))
            {
                timer_btnLongPress.TriggerSource = btnDelete.Name;
            }
            else if (sender.Equals(this.btnDeleteIncludeServers))
            {
                timer_btnLongPress.TriggerSource = btnDeleteIncludeServers.Name;
            }
            timer_btnLongPress.Interval = 100;
            timer_btnLongPress.Start();
        }
        private void Btn_MouseUp(object sender, MouseEventArgs e)
        {
            timer_btnLongPress.Stop();
            if (timer_btnLongPress.LongPress)
            {
                timer_btnLongPress.LongPress = false;
                if (timer_btnLongPress.TriggerSource == btnUp.Name)
                    this.btnUp.Click += BtnUp_Click;
                else if (timer_btnLongPress.TriggerSource == btnDown.Name)
                    this.btnDown.Click += BtnDown_Click;
                else if (timer_btnLongPress.TriggerSource == btnDelete.Name)
                    this.btnDelete.Click += this.BtnDelete_Click;
                else if (timer_btnLongPress.TriggerSource == btnDeleteIncludeServers.Name)
                    this.btnDeleteIncludeServers.Click += this.BtnDeleteIncludeNode_Click;
                timer_btnLongPress.TriggerSource = null;
                listServerSubscribe.Focus();
            }
        }
        private void timer_btnLongPress_Tick(object sender, EventArgs e)
        {
            timer_btnLongPress.Stop();
            timer_btnLongPress.LongPress = true;
            if (timer_btnLongPress.TriggerSource == btnUp.Name)
            {
                this.btnUp.Click -= BtnUp_Click;
                BtnUp_Click(null, null);
            }
            else if (timer_btnLongPress.TriggerSource == btnDown.Name)
            {
                this.btnDown.Click -= BtnDown_Click;
                BtnDown_Click(null, null);
            }
            else if (timer_btnLongPress.TriggerSource == btnDelete.Name)
            {
                this.btnDelete.Click -= this.BtnDelete_Click;
                BtnDelete_Click(null, null);
            }
            else if (timer_btnLongPress.TriggerSource == btnDeleteIncludeServers.Name)
            {
                this.btnDeleteIncludeServers.Click -= this.BtnDeleteIncludeNode_Click;
                BtnDelete_Click(true, null);
            }
            this.timer_btnLongPress.Start();
        }

        private void chkSettingChanged()
        {
            if (!allowCheck)
                return;
            btnApply.Enabled = isSettingChanged();
            if (btnApply.Enabled)
                configChanged = true;
            else
                configChanged = false;
        }
        private bool isSettingChanged()
        {
            Configuration config = controller.GetCurrentConfiguration();
            if (config.serverSubscribes.Count != settings.serverSubscribes.Count)
                return true;


            if (checkBoxAutoUpdate.Checked ^ config.nodeFeedAutoUpdate)
                return true;
            if (int.Parse(cmbAutoUpdateInterval.SelectedItem.ToString()) != config.nodeFeedAutoUpdateInterval)
                return true;
            if (checkBoxAutoLatency.Checked ^ config.nodeFeedAutoLatency)
                return true;
            if ((int)(float.Parse(cmbAutoLatencyInterval.SelectedItem.ToString()) * 60) != config.nodeFeedAutoLatencyInterval)
                return true;
            if (checkBoxAutoUpdateUseProxy.Checked ^ config.nodeFeedAutoUpdateUseProxy)
                return true;
            if (checkBoxAutoUpdateTryUseProxy.Checked ^ config.nodeFeedAutoUpdateTryUseProxy)
                return true;
            if (chkBSortServersBySubscriptionsOrder.Checked ^ config.sortServersBySubscriptionsOrder)
                return true;


            for (int i = 0; i < settings.serverSubscribes.Count; i++)
            {
                if (!CompareServerSubscribe(config.serverSubscribes[i], settings.serverSubscribes[i]))
                    return true;
            }

            listDelSubscribes.Clear();
            return false;
        }


        private void listServerSubscribe_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                int index = listServerSubscribe.IndexFromPoint(e.X, e.Y);
                if (index < 0)
                    ;
                else if (listServerSubscribe.SelectedItems.Count == 0)
                {
                    listServerSubscribe.SelectedIndex = index;
                }
                else if (listServerSubscribe.SelectedItems.Count == 1 && listServerSubscribe.SelectedIndex != index)
                {
                    listServerSubscribe.SelectedIndexChanged -= listServerSubscribe_SelectedIndexChanged;
                    listServerSubscribe.SelectedItems.Clear();
                    listServerSubscribe.SelectedIndexChanged += listServerSubscribe_SelectedIndexChanged;
                    listServerSubscribe.SelectedIndex = index;
                }

                if (listServerSubscribe.SelectedItems.Count > 0)
                {
                    UpdateSelectedToolStripMenuItem.Enabled = true;
                    UpdateSelectedUseProxyToolStripMenuItem.Enabled = true;
                }
                else
                {
                    UpdateSelectedToolStripMenuItem.Enabled = false;
                    UpdateSelectedUseProxyToolStripMenuItem.Enabled = false;
                }

                cMSlistServerSubscribe.Show(listServerSubscribe, e.X, e.Y);
            }

        }
        private void UpdateSelectedToolStripMenuItem_Click(object sender, EventArgs e)
        {
            mtExec.WaitOne();
            UpdateSelectedSubscribes(false);
            mtExec.ReleaseMutex();
        }
        private void UpdateSelectedUseProxyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            mtExec.WaitOne();
            UpdateSelectedSubscribes(true);
            mtExec.ReleaseMutex();
        }
        private void UpdateSelectedSubscribes(bool useproxy)
        {
            if (btnApply.Enabled)
            {
                MessageBox.Show(I18N.GetString("Changes must be applied before execution."), I18N.GetString("Tips"));
                return;
            }
            int[] tmp;
            listServerSubscribe.SelectedIndices.CopyTo(tmp = new int[listServerSubscribe.SelectedIndices.Count], 0);
            controller.InvokeUpdateNodeFromSubscribeForm(new object[] { tmp, useproxy }, null);
        }


        private void StoreCurrentlyEdited(bool InEditing = false)
        {
            currentlyEdited.InEditing = InEditing;

            currentlyEdited.listServerSubscribe_TopIndex = listServerSubscribe.TopIndex;
            currentlyEdited.selectindex = listServerSubscribe.SelectedIndex;
            if (currentlyEdited.selecteditems == null)
                currentlyEdited.selecteditems = new List<int>();
            if (listServerSubscribe.SelectedIndices.Count > 0)
            {
                currentlyEdited.selecteditems.Clear();
                foreach (int i in listServerSubscribe.SelectedIndices)
                    currentlyEdited.selecteditems.Add(i);
            }

            currentlyEdited.txttBURL = tBURL.Text;
            if (tBURL.Focused)
            {
                currentlyEdited.txttBfocused = true;
                currentlyEdited.txttBURLCursorPos = tBURL.SelectionStart;
                currentlyEdited.txttBURLSelectionLength = tBURL.SelectionLength;
            }
            currentlyEdited.chkJoinUpdate = chkBJoinUpdate.Checked;
            currentlyEdited.chkUseProxy = chkBUseProxy.Checked;
            currentlyEdited.chkDontUseProxy = chkBDontUseProxy.Checked;
        }
        private void RestoreCurrentlyEdited(int Restorelevel = 0)
        {
            if (currentlyEdited.selectindex > -1 && currentlyEdited.selectindex < listServerSubscribe.Items.Count)
            {
                if (currentlyEdited.selecteditems != null && currentlyEdited.selecteditems.Count > 1)
                {
                    foreach (int i in currentlyEdited.selecteditems)
                    {
                        if (i < listServerSubscribe.Items.Count)
                            listServerSubscribe.SelectedIndex = i;
                    }
                    lbServerSubscribeMultiselected = true;
                }
                else
                    lbServerSubscribeMultiselected = false;
                if (currentlyEdited.selectindex < listServerSubscribe.Items.Count)
                    listServerSubscribe.SelectedIndex = currentlyEdited.selectindex;
            }
            listServerSubscribe.TopIndex = currentlyEdited.listServerSubscribe_TopIndex;

            if (Restorelevel == 1)
                return;


            if (currentlyEdited.selectindex > -1 && currentlyEdited.selectindex < listServerSubscribe.Items.Count)
            {
                tBURL.Text = currentlyEdited.txttBURL;
                chkBJoinUpdate.Checked = currentlyEdited.chkJoinUpdate;
                chkBUseProxy.Checked = currentlyEdited.chkUseProxy;
                chkBDontUseProxy.Checked = currentlyEdited.chkDontUseProxy;
                if (currentlyEdited.InEditing && currentlyEdited.txttBfocused)
                {
                    tBURL.Focus();
                    if (currentlyEdited.txttBURLSelectionLength > 0)
                        tBURL.Select(currentlyEdited.txttBURLCursorPos, currentlyEdited.txttBURLSelectionLength);
                    else
                        tBURL.SelectionStart = currentlyEdited.txttBURLCursorPos;
                }
            }

            if (Restorelevel == 2)
                return;
        }


        private void listServerSubscribe_DrawItem(object sender, DrawItemEventArgs e)
        {

            if (e.Index >= 0)
            {
                e.DrawBackground();
                Brush mybsh = Brushes.Black;

                //string[] strarray = listServerSubscribe.Items[e.Index].ToString().Split(new string[] { "\r\n    " }, StringSplitOptions.RemoveEmptyEntries);
                string url = (e.Index > -1 && e.Index < settings.serverSubscribes.Count) ? settings.serverSubscribes[e.Index].URL : "";

                if (listnewUpdateAboutSubscribes.Contains(e.Index))
                    mybsh = Brushes.Green;

                if (e.Index > 0)
                {
                    List<string> listrepeated = Util.Utils.IsServerSubscriptionRepeat(settings.serverSubscribes, settings.serverSubscribes[e.Index].URL);
                    if (listrepeated.Count > 0 && listrepeated.FindIndex(t => t == settings.serverSubscribes[e.Index].id) > -1)   
                    {
                        mybsh = Brushes.DarkOrange;
                    }
                }

                if (settings.serverSubscribes[e.Index].URL == "")
                    mybsh = Brushes.Red;
                else if (url != "" && UpdateSubscribeManager.listSubscribeFailureLinks != null)
                {
                    if (UpdateSubscribeManager.listSubscribeFailureLinks.FindIndex(t => t == url) != -1)
                    {
                        mybsh = Brushes.Red;
                    }
                }

                // 焦点框
                e.DrawFocusRectangle();
                //文本 
                e.Graphics.DrawString(listServerSubscribe.Items[e.Index].ToString(), e.Font, mybsh, e.Bounds, StringFormat.GenericDefault);
            }
        }

        private void listServerSubscribe_MouseWheel(object sender, MouseEventArgs e)
        {
            if (cMSlistServerSubscribe.Visible || !listServerSubscribe.Bounds.Contains(e.Location))
            {
                if (e is HandledMouseEventArgs h)
                {
                    h.Handled = true;
                }
            }
            else
            {
                if (e is HandledMouseEventArgs h)
                {
                    h.Handled = false;
                }
            }
        }

        private bool CompareServerSubscribe(ServerSubscribe ss1, ServerSubscribe ss2)
        {
            if (ss1.id != ss2.id)
                return false;
            if (ss1.URL != ss2.URL)
                return false;
            if (ss1.Group != ss2.Group)
                return false;
            if (ss1.groupUserDefine ^ ss2.groupUserDefine)
                return false;
            if (ss1.JoinUpdate ^ ss2.JoinUpdate)
                return false;
            if (ss1.UseProxy ^ ss2.UseProxy)
                return false;
            if (ss1.DontUseProxy ^ ss2.DontUseProxy)
                return false;
            if (ss1.LastUpdateTime != ss2.LastUpdateTime)
                return false;
            return true;
        }

        private void TBURL_TextChanged(object sender, EventArgs e)
        {
            if (!allowCheck || listServerSubscribe.SelectedItems.Count != 1 || !(_oldselect_index > -1 && _oldselect_index < listServerSubscribe.Items.Count))
                return;
            mtExec.WaitOne();
            allowCheck = false;
            ServerSubscribe ss = settings.serverSubscribes[_oldselect_index];
            ss.URL = tBURL.Text;


            ServerSubscribe readonlySubscribe = null;
            foreach (ServerSubscribe tmpss in controller.GetCurrentConfiguration().serverSubscribes)
                if (settings.serverSubscribes[_oldselect_index].id == tmpss.id)
                {
                    readonlySubscribe = tmpss;
                    break;
                }
            if (readonlySubscribe == null)
            {
                if (!String.IsNullOrEmpty(tBGroup.Text))
                    ss.groupUserDefine = true;
            }
            else
            {
                if(readonlySubscribe.Host != ss.Host)
                {
                    if (!String.IsNullOrEmpty(tBGroup.Text))
                        ss.groupUserDefine = true;
                    else
                        ss.groupUserDefine = false;
                }
                else
                {
                    if (String.IsNullOrEmpty(tBGroup.Text))
                    {
                        if (readonlySubscribe.groupUserDefine) 
                        {
                            ss.groupUserDefine = false;
                        }
                        else
                        {
                            tBGroup.Text = readonlySubscribe.Group;
                            ss.Group = readonlySubscribe.Group;
                            ss.groupUserDefine = readonlySubscribe.groupUserDefine;
                        }
                    }
                    else
                    {
                        if (tBGroup.Text == readonlySubscribe.Group)
                            ss.groupUserDefine = readonlySubscribe.groupUserDefine;
                        else
                            ss.groupUserDefine = true;
                    }
                }
            }

            if (readonlySubscribe != null && readonlySubscribe.Host == ss.Host && readonlySubscribe.LastUpdateTime != 0) 
            {
                DateTime now = new DateTime(1970, 1, 1, 0, 0, 0);
                now = now.AddSeconds(readonlySubscribe.LastUpdateTime);
                tBUpdate.Text = now.ToLongDateString() + " " + now.ToLongTimeString();
                ss.LastUpdateTime = readonlySubscribe.LastUpdateTime;
            }
            else
            {
                tBUpdate.Text = "(｢･ω･)｢";
                ss.LastUpdateTime = 0;
            }
            listServerSubscribe.Items[_oldselect_index] = GenerateListInfo(listServerSubscribe.CreateGraphics(), ss);

            allowCheck = true;
            chkSettingChanged();
            mtExec.ReleaseMutex();
        }
        private void TBGroup_TextChanged(object sender, EventArgs e)
        {
            if (!allowCheck || listServerSubscribe.SelectedItems.Count != 1 || !(_oldselect_index > -1 && _oldselect_index < listServerSubscribe.Items.Count))
                return;
            mtExec.WaitOne();
            allowCheck = false;
            int index = controller.GetCurrentConfiguration().serverSubscribes.FindIndex(t => t.id == settings.serverSubscribes[_oldselect_index].id);
            ServerSubscribe readonlySubscribe = index != -1 ? controller.GetCurrentConfiguration().serverSubscribes[index] : null;

            ServerSubscribe ss = settings.serverSubscribes[_oldselect_index];
            ss.Group = tBGroup.Text;

            if (!String.IsNullOrEmpty(tBGroup.Text) && readonlySubscribe != null && readonlySubscribe.Group != ss.Group) 
                ss.groupUserDefine = true;
            else
            {
                ss.groupUserDefine = false;
            }
            listServerSubscribe.Items[_oldselect_index] = GenerateListInfo(listServerSubscribe.CreateGraphics(), ss);

            allowCheck = true;
            chkSettingChanged();
            mtExec.ReleaseMutex();
        }
        private void TBGroup_Leave(object sender, EventArgs e)
        {
            if (!allowCheck || listServerSubscribe.SelectedItems.Count != 1 || !(_oldselect_index > -1 && _oldselect_index < listServerSubscribe.Items.Count))
                return;
            mtExec.WaitOne();
            allowCheck = false;

            if (String.IsNullOrEmpty(tBGroup.Text))
            {
                ServerSubscribe ss = settings.serverSubscribes[_oldselect_index];
                int index = controller.GetCurrentConfiguration().serverSubscribes.FindIndex(t => t.id == settings.serverSubscribes[_oldselect_index].id);
                ServerSubscribe readonlySubscribe = index != -1 ? controller.GetCurrentConfiguration().serverSubscribes[index] : null;
                if (readonlySubscribe != null && !String.IsNullOrEmpty(readonlySubscribe.Group) && readonlySubscribe.Host == ss.Host)  
                {
                    tBGroup.Text = readonlySubscribe.Group;
                    ss.Group = tBGroup.Text;
                    listServerSubscribe.Items[_oldselect_index] = GenerateListInfo(listServerSubscribe.CreateGraphics(), ss);
                }
            }

            allowCheck = true;
            chkSettingChanged();
            mtExec.ReleaseMutex();
        }
        private void CheckBox_CheckedChanged(object sender, EventArgs e)
        {
            if (!allowCheck)
                return;
            mtExec.WaitOne();
            allowCheck = false;
            string chkbName = ((CheckBox)sender).Name;
            try
            {
                if (chkbName == checkBoxAutoUpdate.Name)
                {
                    settings.nodeFeedAutoUpdate = checkBoxAutoUpdate.Checked;
                    return;
                }
                else if (chkbName == checkBoxAutoLatency.Name)
                {
                    settings.nodeFeedAutoLatency = checkBoxAutoLatency.Checked;
                    return;
                }
                else if (chkbName == checkBoxAutoUpdateUseProxy.Name)
                {
                    settings.nodeFeedAutoUpdateUseProxy = checkBoxAutoUpdateUseProxy.Checked;
                    if (checkBoxAutoUpdateUseProxy.Checked && checkBoxAutoUpdateTryUseProxy.Checked)
                    {
                        checkBoxAutoUpdateTryUseProxy.Checked = false;
                        settings.nodeFeedAutoUpdateTryUseProxy = checkBoxAutoUpdateTryUseProxy.Checked;
                    }
                    return;
                }
                else if (chkbName == checkBoxAutoUpdateTryUseProxy.Name)
                {
                    settings.nodeFeedAutoUpdateTryUseProxy = checkBoxAutoUpdateTryUseProxy.Checked;
                    if (checkBoxAutoUpdateTryUseProxy.Checked && checkBoxAutoUpdateUseProxy.Checked)
                    {
                        checkBoxAutoUpdateUseProxy.Checked = false;
                        settings.nodeFeedAutoUpdateUseProxy = checkBoxAutoUpdateUseProxy.Checked;
                    }
                    return;
                }

                if (listServerSubscribe.SelectedItems.Count != 1 || !(_oldselect_index > -1 && _oldselect_index < listServerSubscribe.Items.Count))
                    return;
                ServerSubscribe ss = settings.serverSubscribes[_oldselect_index];
                if (chkbName == chkBJoinUpdate.Name)
                    ss.JoinUpdate = chkBJoinUpdate.Checked;
                else if (chkbName == chkBUseProxy.Name)
                {
                    ss.UseProxy = chkBUseProxy.Checked;
                    if (chkBUseProxy.Checked && chkBDontUseProxy.Checked)
                    {
                        chkBDontUseProxy.Checked = false;
                        ss.DontUseProxy = chkBDontUseProxy.Checked;
                    }
                }
                else if (chkbName == chkBDontUseProxy.Name)
                {
                    ss.DontUseProxy = chkBDontUseProxy.Checked;
                    if (chkBDontUseProxy.Checked && chkBUseProxy.Checked)
                    {
                        chkBUseProxy.Checked = false;
                        ss.UseProxy = chkBUseProxy.Checked;
                    }
                }
                else
                    Logging.Log(LogLevel.Error, "Subscribe CheckBox Trigger unknown");
            }
            catch (Exception ex)
            {
                Logging.Log(LogLevel.Error, String.Format("Operation {0} error:" + System.Environment.NewLine + "{1}", chkbName, ex.Message));
            }
            finally
            {
                allowCheck = true;
                chkSettingChanged();
                mtExec.ReleaseMutex();
            }
        }

        private void Cmb_DropDownClosed(object sender,EventArgs e)
        {
            if (!allowCheck)
                return;
            mtExec.WaitOne();
            allowCheck = false;
            string cmbName = ((ComboBox)sender).Name;
            try
            {
                int tmp;
                if (cmbName == cmbAutoUpdateInterval.Name)
                {
                    tmp = int.Parse(cmbAutoUpdateInterval.SelectedItem.ToString());
                        if (settings.nodeFeedAutoUpdateInterval != tmp)
                    {
                        settings.nodeFeedAutoUpdateInterval = tmp;
                    }
                }
                else if (cmbName == cmbAutoLatencyInterval.Name)
                {
                    tmp = (int)(float.Parse(cmbAutoLatencyInterval.SelectedItem.ToString()) * 60);
                    if (settings.nodeFeedAutoLatencyInterval != tmp)
                    {
                        settings.nodeFeedAutoLatencyInterval = tmp;
                    }
                }

            }
            catch (Exception ex)
            {
                Logging.Log(LogLevel.Error, String.Format("Operation {0} error:" + System.Environment.NewLine + "{1}", cmbName, ex.Message));
            }
            finally
            {
                allowCheck = true;
                chkSettingChanged();
                mtExec.ReleaseMutex();
            }

        }
        private void SubscribeForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            controller.ConfigChanged -= controller_ConfigChanged;
            listDelSubscribes.Clear();
            listDelSubscribes = null;
            mtExec.Dispose();
            mtExec = null;
        }


        public class Settings
        {
            public List<ServerSubscribe> serverSubscribes;
            public bool nodeFeedAutoUpdate;
            public int nodeFeedAutoUpdateInterval;
            public bool nodeFeedAutoLatency;
            public int nodeFeedAutoLatencyInterval;
            public bool nodeFeedAutoUpdateUseProxy;
            public bool nodeFeedAutoUpdateTryUseProxy;
            public bool sortServersBySubscriptionsOrder;

            public UInt64 LastnodeFeedAutoLatency;
            public UInt64 LastUpdateSubscribesTime;
            public Settings(Configuration config)
            {
                serverSubscribes = new List<ServerSubscribe>();
                foreach (ServerSubscribe ss in config.serverSubscribes)
                    serverSubscribes.Add(ss.Clone());
                nodeFeedAutoUpdate = config.nodeFeedAutoUpdate;
                nodeFeedAutoUpdateInterval = config.nodeFeedAutoUpdateInterval;
                nodeFeedAutoLatency = config.nodeFeedAutoLatency;
                nodeFeedAutoLatencyInterval = config.nodeFeedAutoLatencyInterval;
                nodeFeedAutoUpdateUseProxy = config.nodeFeedAutoUpdateUseProxy;
                nodeFeedAutoUpdateTryUseProxy = config.nodeFeedAutoUpdateTryUseProxy;
                sortServersBySubscriptionsOrder = config.sortServersBySubscriptionsOrder;
                LastnodeFeedAutoLatency = config.LastnodeFeedAutoLatency;
                LastUpdateSubscribesTime = config.LastUpdateSubscribesTime;
            }

            public void SaveTo(Configuration config, ref List<DelSubscribes> listDelSubscribes)
            {
                config.nodeFeedAutoUpdate = nodeFeedAutoUpdate;
                config.nodeFeedAutoUpdateInterval = nodeFeedAutoUpdateInterval;
                config.nodeFeedAutoLatency = nodeFeedAutoLatency;
                config.nodeFeedAutoLatencyInterval = nodeFeedAutoLatencyInterval;
                config.nodeFeedAutoUpdateUseProxy = nodeFeedAutoUpdateUseProxy;
                config.nodeFeedAutoUpdateTryUseProxy = nodeFeedAutoUpdateTryUseProxy;
                config.sortServersBySubscriptionsOrder = sortServersBySubscriptionsOrder;
                config.LastnodeFeedAutoLatency = LastnodeFeedAutoLatency;
                config.LastUpdateSubscribesTime = LastUpdateSubscribesTime;

                string currentserverid = config.Servers[config.index].id;
                List<Server> allServers = config.Servers;
                //change group name
                string invalidservergroupname = I18N.GetString("Server subscription did not return a valid server.");
                foreach (ServerSubscribe ss in serverSubscribes)
                {
                    int index = config.serverSubscribes.FindIndex(t => t.id == ss.id);
                    if (index != -1)
                    {
                        string oldurl = config.serverSubscribes[index].URL;
                        string oldgroup = config.serverSubscribes[index].Group;
                        //delete invalid server if url changed or update invalid server remark if group name changed
                        if(!String.IsNullOrEmpty(oldurl) && !String.IsNullOrEmpty(oldgroup) && !String.IsNullOrEmpty(ss.URL) && !String.IsNullOrEmpty(ss.Group) && (oldurl != ss.URL || oldgroup != ss.Group))
                        {
                            if (oldurl != ss.URL)
                            {
                                //delete invalid server if url changed
                                while ((index = allServers.FindIndex(t => t.group == invalidservergroupname && (t.remarks == oldgroup + " " + oldurl || t.remarks == oldurl))) != -1)
                                {
                                    allServers.RemoveAt(index);
                                }
                            }
                            else if (oldgroup != ss.Group)
                            {
                                //update invalid server remark if group name changed
                                while ((index = allServers.FindIndex(t => t.group == invalidservergroupname && (t.remarks == oldgroup + " " + oldurl || t.remarks == oldurl))) != -1)
                                {
                                    allServers[index].remarks = (String.IsNullOrEmpty(ss.Group) ? "" : ss.Group + " ") + ss.URL;
                                }
                            }
                        }
                        //update server group if gourp name changed
                        if (ss.groupUserDefine && !String.IsNullOrEmpty(ss.Group) && !String.IsNullOrEmpty(oldgroup) && ss.Group != oldgroup)
                        {
                            //index = -1;
                            while ((index = allServers.FindIndex(t => t.group == oldgroup)) != -1)
                            {
                                allServers[index].group = ss.Group;
                            };
                        }
                    }
                }

                //del(if need) node
                if (listDelSubscribes.Count > 0)
                {
                    foreach (DelSubscribes ds in listDelSubscribes)
                    {
                        int index = config.serverSubscribes.FindIndex(t => t.id == ds.id);
                        if (index !=-1 && ds.delnode)
                        {
                            string groupname = config.serverSubscribes[index].Group;
                            if (groupname != "")
                            {
                                int serverindex = -1;
                                while ((serverindex = allServers.FindIndex(t => t.group == groupname)) != -1)
                                {
                                    allServers[serverindex].GetConnections().CloseAll();
                                    allServers.RemoveAt(serverindex);
                                }
                            }
                            string Url = config.serverSubscribes[index].URL;
                            while ((index = allServers.FindIndex(t => t.group == invalidservergroupname && (t.remarks == groupname + " " + Url || t.remarks == Url))) != -1) 
                            {
                                allServers.RemoveAt(index);
                            }
                        }

                    }
                    listDelSubscribes.Clear();

                }

                int currentservernewindex = config.Servers.FindIndex(t => t.id == currentserverid);
                if (currentservernewindex != -1 || (currentservernewindex = config.Servers.FindIndex(t => t.enable)) != -1)
                    config.index = currentservernewindex;
                else
                    config.index = 0;

                config.serverSubscribes.Clear();
                foreach (ServerSubscribe ss in serverSubscribes)
                    config.serverSubscribes.Add(ss.Clone());

                if (config.sortServersBySubscriptionsOrder)
                    Util.Utils.SortServers(config);

            }
        }
        public class DelSubscribes
        {
            public string id;
            public bool delnode;
            public DelSubscribes(string _id, bool _delnode = false)
            {
                id = _id;
                delnode = _delnode;
            }
        }
        public struct CurrentlyEdited
        {
            public bool InEditing;

            public int listServerSubscribe_TopIndex;
            public List<int> selecteditems;
            public int selectindex;
            public string txttBURL;
            public bool txttBfocused;
            public int txttBURLCursorPos;
            public int txttBURLSelectionLength;
            public bool chkJoinUpdate;
            public bool chkUseProxy;
            public bool chkDontUseProxy;
        }
        public struct KeyInfoOfTheSubscription
        {
            public string Name;
            public string Host;
        }
    }
}
