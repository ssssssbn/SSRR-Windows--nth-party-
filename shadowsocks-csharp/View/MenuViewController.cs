using Shadowsocks.Controller;
using Shadowsocks.Model;
using Shadowsocks.Properties;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;
using System.Runtime.InteropServices;

using ZXing;
using ZXing.Common;
using ZXing.QrCode;
using System.Threading;
using System.Text.RegularExpressions;
using Shadowsocks.Util;
using System.Net.Sockets;
using System.Net;
using System.Linq;

using Shadowsocks.Controller.Hotkeys;
using System.Text;
using Microsoft.Win32;

namespace Shadowsocks.View {
    public class EventParams {
        public object _sender;
        public EventArgs _e;

        public EventParams(object sender, EventArgs e) {
            _sender = sender;
            _e = e;
        }
    }

    public class MenuViewController {
        // yes this is just a menu view controller
        // when config form is closed, it moves away from RAM
        // and it should just do anything related to the config form

        private const string Name = "MenuViewController";
        private ShadowsocksController _controller;
        private UpdateChecker updateChecker;
        private UpdateFreeNode updateFreeNodeChecker;
        private UpdateSubscribeManager updateSubscribeManager;

        private NotifyIcon _notifyIcon;
        private ContextMenu contextMenu1;

        private MenuItem noModifyItem;
        private MenuItem enableItem;
        private MenuItem PACModeItem;
        private MenuItem globalModeItem;
        private MenuItem modeItem;

        private MenuItem ruleBypassLan;
        private MenuItem ruleBypassLanAndChina;
        private MenuItem ruleBypassLanAndNotChina;
        private MenuItem ruleUser;
        private MenuItem ruleDisableBypass;

        private MenuItem SeperatorItem;
        private MenuItem ServersItem;
        private MenuItem SelectedEnableBalanceItem;
        private MenuItem sameHostForSameTargetItem;
        private MenuItem UpdateItem;
        private MenuItem hotKeyItem;
        private ConfigForm configForm;
        private SettingsForm settingsForm;
        private ServerLogForm serverLogForm;
        private PortSettingsForm portMapForm;
        private SubscribeForm subScribeForm;
        private LogForm logForm;
        private HotkeySettingsForm hotkeySettingsForm;
        private string _urlToOpen;
        private System.Timers.Timer timerDisconnectCurrent;
        private const double timerDisconnectCurrentInterval = 1000.0 * 1;
        private System.Timers.Timer timerCheckUpdate;
        private double timerCheckSubscriptionsUpdateInterval;
        private System.Timers.Timer timerUpdateLatency;
        private double timerUpdateLatencyInterval;
        //private System.Timers.Timer timerCheckSystemTime;

        public bool configfrom_open = false;
        public bool subScribeForm_open = false;
        private List<EventParams> eventList = new List<EventParams>();
        
        public static AppBarForm appbarform;
        private Mutex mtupdatenode = new Mutex();

        //[DllImport("user32.dll", CharSet = CharSet.Auto)]
        //static extern bool DestroyIcon(IntPtr handle);

        public MenuViewController(ShadowsocksController controller)
        {
            _controller = controller;

            LoadMenu();

            _controller.ToggleModeChanged += controller_ToggleModeChanged;
            _controller.ToggleRuleModeChanged += controller_ToggleRuleModeChanged;
            _controller.ConfigChanged += controller_ConfigChanged;
            _controller.PACFileReadyToOpen += controller_FileReadyToOpen;
            _controller.UserRuleFileReadyToOpen += controller_FileReadyToOpen;
            _controller.Errored += controller_Errored;
            _controller.UpdatePACFromGFWListCompleted += controller_UpdatePACFromGFWListCompleted;
            _controller.UpdatePACFromGFWListError += controller_UpdatePACFromGFWListError;
            _controller.ShowConfigFormEvent += Config_Click;
            _controller.UpdateNodeFromSubscribeForm += controller_UpdateNodeFromSubscribeForm;

            _notifyIcon = new NotifyIcon();
            UpdateTrayIcon();
            _notifyIcon.Visible = true;
            _notifyIcon.ContextMenu = contextMenu1;
            _notifyIcon.MouseClick += notifyIcon_Click;

            updateChecker = new UpdateChecker();
            updateChecker.NewVersionFound += updateChecker_NewVersionFound;

            updateFreeNodeChecker = new UpdateFreeNode();
            updateFreeNodeChecker.NewFreeNodeFound += updateFreeNodeChecker_NewFreeNodeFound;

            updateSubscribeManager = new UpdateSubscribeManager(updateFreeNodeChecker);

            LoadCurrentConfiguration();

            if (timerDisconnectCurrent == null)
            {
                timerDisconnectCurrent = new System.Timers.Timer(timerDisconnectCurrentInterval);
                timerDisconnectCurrent.AutoReset = false;
                timerDisconnectCurrent.Elapsed += timerDisconnectCurrent_Elapsed;
            }

        }

        public void initTimers()
        {
            //interval will be changed as needed

            if (timerCheckUpdate == null)
            {
                timerCheckUpdate = new System.Timers.Timer();
                timerCheckUpdate.AutoReset = false;
                timerCheckUpdate.Elapsed += timerCheckSubscriptionsUpdate_Elapsed;
            }
            if (timerUpdateLatency == null)
            {
                timerUpdateLatency = new System.Timers.Timer();
                timerUpdateLatency.AutoReset = false;
                timerUpdateLatency.Elapsed += timerUpdateLatency_Elapsed;
            }

            if (_controller.GetCurrentConfiguration().nodeFeedAutoUpdate)
            {
                if (!timerCheckUpdate.Enabled)
                {
                    double timenow = Math.Floor(DateTime.Now.Subtract(new DateTime(1970, 1, 1, 0, 0, 0)).TotalSeconds);
                    double timeLast = _controller.GetCurrentConfiguration().LastUpdateSubscribesTime;
                    double timespan = timenow - timeLast;

                    if(timenow > timeLast)
                    {
                        if (timespan > timerCheckSubscriptionsUpdateInterval)
                            timerCheckUpdate.Interval = 1000.0 * 10;
                        else
                            timerCheckUpdate.Interval = 1000.0 * (timerCheckSubscriptionsUpdateInterval - timespan);
                    }
                    else
                        timerCheckUpdate.Interval = 1000.0 * timerCheckSubscriptionsUpdateInterval;
                    Logging.Debug("timerCheckUpdate started");
                    timerCheckUpdate.Start();

                    if (_controller.GetCurrentConfiguration().nodeFeedAutoLatency && timerCheckUpdate.Interval != 1000.0 * 10)
                    {
                        if (!timerUpdateLatency.Enabled)
                        {
                            timeLast = _controller.GetCurrentConfiguration().LastnodeFeedAutoLatency;
                            timespan = timenow - timeLast;
                            if (timenow > timeLast)
                            {
                                if (timespan > timerUpdateLatencyInterval)
                                    timerUpdateLatency.Interval = 1000.0 * 10;
                                else
                                    timerUpdateLatency.Interval = 1000.0 * (timerUpdateLatencyInterval - timespan);
                            }
                            else
                                timerCheckUpdate.Interval = 1000.0 * timerUpdateLatencyInterval;

                            Logging.Debug("timerUpdateLatency started");
                            timerUpdateLatency.Start();
                        }
                    }
                    else
                        timerUpdateLatency.Stop();
                }
            }
            else
            {
                timerCheckUpdate.Stop();
                if (_controller.GetCurrentConfiguration().nodeFeedAutoLatency)
                {
                    if (!timerUpdateLatency.Enabled)
                    {
                        double timespan = (double)((UInt64)Math.Floor(DateTime.Now.Subtract(new DateTime(1970, 1, 1, 0, 0, 0)).TotalSeconds) - _controller.GetCurrentConfiguration().LastnodeFeedAutoLatency);
                        if (timespan > timerUpdateLatencyInterval)
                            timerUpdateLatency.Interval = 1000.0 * 10;
                        else
                            timerUpdateLatency.Interval = 1000.0 * (timerUpdateLatencyInterval - timespan);
                        Logging.Debug("timerUpdateLatency started");
                        timerUpdateLatency.Start();
                    }
                }
                else
                    timerUpdateLatency.Stop();

            }
        }

        public void stopTimers()
        {
            if (timerCheckUpdate != null && timerCheckUpdate.Enabled)
                timerCheckUpdate.Stop();
            if (timerUpdateLatency != null && timerUpdateLatency.Enabled)
                timerUpdateLatency.Stop();
            //if (timerCheckSystemTime != null && timerCheckSystemTime.Enabled)
            //    timerCheckSystemTime.Stop();
        }

        public void ShownotifyIcontext()
        {
            ShowBalloonTip(I18N.GetString("Current status"), _notifyIcon.Text, ToolTipIcon.Info);
        }
        
        public void ShowTextByNotifyIconBalloon(string title, string content, ToolTipIcon icon)
        {
            ShowBalloonTip(I18N.GetString(title), I18N.GetString(content), icon);
        }

        private void timerDisconnectCurrent_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            _controller.DisconnectAllConnections();
        }

        private void timerCheckSubscriptionsUpdate_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            try
            {
                if (timerCheckUpdate != null)
                {
                    timerCheckUpdate.Interval = 1000.0 * timerCheckSubscriptionsUpdateInterval;
                }

                Configuration cfg = _controller.GetCurrentConfiguration();
                double timenow = Math.Floor(DateTime.Now.Subtract(new DateTime(1970, 1, 1, 0, 0, 0)).TotalSeconds);
                double timeLast = cfg.LastUpdateSubscribesTime;
                if (timenow <= timeLast)
                    return;

                if (Utils.IsConnectionAvailable()) 
                {
                    if (cfg.serverSubscribes.Count > 0)
                        CheckNodeUpdate(null, cfg.nodeFeedAutoUpdateUseProxy && !cfg.isDefaultConfig(), false);
                }
                else
                    throw new Exception("network error");

            }
            catch (Exception ex)
            {
                string err = null;

                if (ex.Message == "network error")
                    err = "There seems to be a problem with the network connection.";
                //else if (ex.Message == "system time incorrect")
                //    err = "The system time seems to be incorrect.";
                else
                    err = ex.Message + System.Environment.NewLine;
                ShowBalloonTip(I18N.GetString("Error"), I18N.GetString(err), ToolTipIcon.Error);
            }
            finally
            {
                if (_controller.GetCurrentConfiguration().nodeFeedAutoUpdate && !timerCheckUpdate.Enabled)
                    timerCheckUpdate.Start();
            }
        }

        //private void TimerCheckSystemTime_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        //{
        //    try 
        //    {
        //        timerCheckSystemTime.Interval = 1000.0 * 60 * 60 * 24;
        //        double localInternationalUnixTimeStamp = Util.Utils.GetLocalInternationalTime().Subtract(new DateTime(1970, 1, 1, 0, 0, 0)).TotalSeconds;
        //        double systemUnixTimeStamp = DateTime.Now.Subtract(new DateTime(1970, 1, 1, 0, 0, 0)).TotalSeconds;
        //        if (Math.Abs(systemUnixTimeStamp - localInternationalUnixTimeStamp) < 30)
        //            Program.IsSystemTimeCorrectFlag = 1;
        //        else
        //            Program.IsSystemTimeCorrectFlag = 0;
        //    }
        //    catch (Exception ex)
        //    {
        //        Logging.Log(LogLevel.Error, ex.Message);
        //        Program.IsSystemTimeCorrectFlag = 0;
        //    }
        //    finally
        //    {
        //        timerCheckSystemTime.Start();
        //    }
        //}

        private void timerUpdateLatency_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            timerUpdateLatency.Interval = 1000.0 * timerUpdateLatencyInterval;

            if (!Util.TCPingManager.Enabled)
            {
                ShowBalloonTip(I18N.GetString("Tips"), I18N.GetString("The Latency test is temporarily disabled by other operations such as updating subscriptions."), ToolTipIcon.Info);
                return;
            }

            Configuration config = _controller.GetCurrentConfiguration();
            try
            {
                double timenow = Math.Floor(DateTime.Now.Subtract(new DateTime(1970, 1, 1, 0, 0, 0)).TotalSeconds);
                double timeLast = config.LastnodeFeedAutoLatency;
                if (timenow <= timeLast)
                    return;
                //while (Program.IsSystemTimeCorrectFlag == -1)
                //{
                //    Thread.Sleep(100);
                //}
                //if (Program.IsSystemTimeCorrectFlag == 0)
                //    throw new Exception("system time incorrect");

                int serverscount = config.Servers.Count;
                List<string> tcpinglist = new List<string>();
                for (int i = 0; i < serverscount; i++)
                {
                    tcpinglist.Add(config.Servers[i].id);
                }
                Logging.Debug("trigger auto latency");
                Util.TCPingManager.StartTcping(_controller, tcpinglist);
            }
            catch (Exception ex)
            {
                //if (ex.Message == "system time incorrect")
                //    ShowBalloonTip(I18N.GetString("Error"), I18N.GetString(ex.Message), ToolTipIcon.Error);
                //else
                    timerUpdateLatency.Interval = 1000.0 * 10;
                Logging.Log(LogLevel.Error, /*"ServerID:" + ServerID.ToString() + "." +*/ ex.Message);
            }
            finally
            {
                if (config.nodeFeedAutoLatency)
                {
                    timerUpdateLatency.Start();
                }

            }
        }

        void controller_Errored(object sender, System.IO.ErrorEventArgs e) {
            string error_message = e.GetException().ToString();
            if (error_message.StartsWith("System.Exception: " + I18N.GetString("Port already in use")))
                error_message += System.Environment.NewLine + System.Environment.NewLine + I18N.GetString("Go to the Local Agents area of ​​the Options Settings window to set up an available port for the local port.");
            MessageBox.Show(error_message, String.Format(I18N.GetString("Shadowsocks Error: {0}"), e.GetException().Message));
        }

        private void UpdateTrayIcon() {
            int dpi = 96;
            using (Graphics graphics = Graphics.FromHwnd(IntPtr.Zero)) {
                dpi = (int)graphics.DpiX;
            }
            Configuration config = _controller.GetCurrentConfiguration();
            bool enabled = config.sysProxyMode != (int)ProxyMode.NoModify && config.sysProxyMode != (int)ProxyMode.Direct;
            string balancemode = "";
            if (config.enableBalance)
            {
                balancemode = I18N.GetString(config.balanceAlgorithm);
            }
            string server;
            int server_current = config.index;
            if (config.Servers[server_current].remarks == "")
                server = I18N.GetString("No Remarks");
            else
                server = config.Servers[server_current].remarks;
            if (server.Length > 30)
                server = server.Substring(0, 30) + "...";
            bool global = config.sysProxyMode == (int)ProxyMode.Global;
            bool enableBalance = config.enableBalance;

            try {
                Bitmap icon = new Bitmap("icon.png");
                Icon newIcon = Icon.FromHandle(icon.GetHicon());
                icon.Dispose();
                _notifyIcon.Icon = newIcon;
            }
            catch {
                Bitmap icon = null;
                if (dpi < 97) {
                    icon = Resources.ss16;
                }
                else if (dpi < 121) {
                    icon = Resources.ss20;
                }
                else {
                    icon = Resources.ss24;
                }
                double mul_a = 1.0, mul_r = 1.0, mul_g = 1.0, mul_b = 1.0;
                if (!enabled) {
                    mul_g = 0.4;
                }
                else if (!global) {
                    mul_b = 0.4;
                    mul_g = 0.8;
                }
                if (!enableBalance) {
                    mul_r = 0.4;
                }

                Bitmap iconCopy = new Bitmap(icon);
                for (int x = 0; x < iconCopy.Width; x++)
                {
                    for (int y = 0; y < iconCopy.Height; y++)
                    {
                        Color color = icon.GetPixel(x, y);
                        iconCopy.SetPixel(x, y,
                            Color.FromArgb((byte)(color.A * mul_a),
                            ((byte)(color.R * mul_r)),
                            ((byte)(color.G * mul_g)),
                            ((byte)(color.B * mul_b))));
                    }
                }
                Icon newIcon = Icon.FromHandle(iconCopy.GetHicon());
                icon.Dispose();
                iconCopy.Dispose();

                _notifyIcon.Icon = newIcon;
            }

            // we want to show more details but notify icon title is limited to 63 characters
            string text = (enabled ?
                    (global ? I18N.GetString("Global") : I18N.GetString("PAC")) :
                    I18N.GetString("Disable system proxy"))
                    ;
            if (enableBalance)
                text +=
                    System.Environment.NewLine
                    + balancemode;
            text +=
                 System.Environment.NewLine
                + server
                + System.Environment.NewLine
                + String.Format(I18N.GetString("Running: Port {0}"), config.localPort) // this feedback is very important because they need to know Shadowsocks is running
                ;
            _notifyIcon.Text = text.Substring(0, Math.Min(63, text.Length));
        }
        
        private MenuItem CreateMenuItem(string text, EventHandler click) {
            return new MenuItem(I18N.GetString(text), click);
        }

        private MenuItem CreateMenuGroup(string text, MenuItem[] items) {
            return new MenuItem(I18N.GetString(text), items);
        }

        private void LoadMenu() {
            contextMenu1 = new ContextMenu(new MenuItem[] {
                modeItem = CreateMenuGroup("Mode", new MenuItem[] {
                    enableItem = CreateMenuItem("Disable system proxy", new EventHandler(DirectItem_Click)),
                    PACModeItem = CreateMenuItem("PAC", new EventHandler(PACModeItem_Click)),
                    globalModeItem = CreateMenuItem("Global", new EventHandler(GlobalModeItem_Click)),
                    new MenuItem("-"),
                    noModifyItem = CreateMenuItem("No modify system proxy", new EventHandler(NoModifyItem_Click))
                }),
                CreateMenuGroup("PAC ", new MenuItem[] {
                    CreateMenuItem("Update local PAC from GFWList", new EventHandler(UpdatePACFromGFWListItem_Click)),
                    new MenuItem("-"),
                    CreateMenuItem("Update local PAC from Lan IP list", new EventHandler(UpdatePACFromLanIPListItem_Click)),
                    CreateMenuItem("Update local PAC from Chn White list", new EventHandler(UpdatePACFromCNWhiteListItem_Click)),
                    CreateMenuItem("Update local PAC from Chn IP list", new EventHandler(UpdatePACFromCNIPListItem_Click)),
                    new MenuItem("-"),
                    CreateMenuItem("Update local PAC from Chn Only list", new EventHandler(UpdatePACFromCNOnlyListItem_Click)),
                    new MenuItem("-"),
                    CreateMenuItem("Copy PAC URL", new EventHandler(CopyPACURLItem_Click)),
                    CreateMenuItem("Edit local PAC file...", new EventHandler(EditPACFileItem_Click)),
                    CreateMenuItem("Edit user rule for GFWList...", new EventHandler(EditUserRuleFileForGFWListItem_Click)),
                }),
                CreateMenuGroup("Proxy rule", new MenuItem[] {
                    ruleBypassLan = CreateMenuItem("Bypass LAN", new EventHandler(RuleBypassLanItem_Click)),
                    ruleBypassLanAndChina = CreateMenuItem("Bypass LAN && China", new EventHandler(RuleBypassLanAndChinaItem_Click)),
                    ruleBypassLanAndNotChina = CreateMenuItem("Bypass LAN && not China", new EventHandler(RuleBypassLanAndNotChinaItem_Click)),
                    ruleUser = CreateMenuItem("User custom", new EventHandler(RuleUserItem_Click)),
                    new MenuItem("-"),
                    ruleDisableBypass = CreateMenuItem("Disable bypass", new EventHandler(RuleBypassDisableItem_Click)),
                }),
                new MenuItem("-"),
                ServersItem = CreateMenuGroup("Servers", new MenuItem[] {
                    SeperatorItem = new MenuItem("-"),
                    CreateMenuItem("Edit servers...", new EventHandler(Config_Click)),
                    new MenuItem("-"),
                    CreateMenuItem("Import from file...", new EventHandler(Import_Click)),
                    CreateMenuItem("Import from screen QRCode...", new EventHandler(ScanQRCodeItem_Click)),
                    CreateMenuItem("Import from clipboard SSR links...", new EventHandler(ScanClipboardAddressItem_Click)),
                    new MenuItem("-"),
                    sameHostForSameTargetItem = CreateMenuItem("Same host for same address", new EventHandler(SelectSameHostForSameTargetItem_Click)),
                    new MenuItem("-"),
                    CreateMenuItem("Server statistic...", new EventHandler(ShowServerLogItem_Click)),
                    CreateMenuItem("Disconnect current", new EventHandler(DisconnectCurrent_Click)),
                }),
                CreateMenuGroup("Servers Subscribe", new MenuItem[] {
                    CreateMenuItem("Subscribe setting...", new EventHandler(SubscribeSetting_Click)),
                    CreateMenuItem("Update subscribe SSR node", new EventHandler(CheckNodeUpdate_Click)),
                    CreateMenuItem("Update subscribe SSR node(use proxy)", new EventHandler(CheckNodeUpdateUseProxy_Click)),
                }),
                SelectedEnableBalanceItem = CreateMenuItem("Load balance", new EventHandler(SelectRandomItem_Click)),
                CreateMenuItem("Global settings...", new EventHandler(Setting_Click)),
                CreateMenuItem("Port settings...", new EventHandler(ShowPortMapItem_Click)),
                new MenuItem("-"),
                CreateMenuItem("Update latency", new EventHandler(UpdateLatency_Click)),
                hotKeyItem = CreateMenuItem("Edit Hotkeys...", new EventHandler(hotKeyItem_Click)),
                CreateMenuItem("Reset password...", new EventHandler(ResetPasswordItem_Click)),
                //CreateMenuItem("Gen custom QRCode...", new EventHandler(showURLFromQRCode)),
                //CreateMenuItem("Show logs...", new EventHandler(ShowLogItem_Click)),
                UpdateItem = CreateMenuItem("Update available", new EventHandler(UpdateItem_Clicked)),
                CreateMenuGroup("Help", new MenuItem[] {
                    CreateMenuItem("Check update", new EventHandler(CheckUpdate_Click)),
                    CreateMenuItem("Show logs...", new EventHandler(ShowLogItem_Click)),
                //    CreateMenuItem("Open wiki...", new EventHandler(OpenWiki_Click)),
                //    CreateMenuItem("Feedback...", new EventHandler(FeedbackItem_Click)),
                //    new MenuItem("-"),
                    CreateMenuItem("Gen custom QRCode...", new EventHandler(showURLFromQRCode)),
                //    new MenuItem("-"),
                    CreateMenuItem("About...", new EventHandler(AboutItem_Click)),
                //    CreateMenuItem("Donate...", new EventHandler(DonateItem_Click)),
                }),
                new MenuItem("-"),
                CreateMenuItem("Quit", new EventHandler(Quit_Click))
            });
            UpdateItem.Visible = false;
        }

        private void controller_ConfigChanged(object sender, EventArgs e)
        {
            List<string> list = (List<string>)(((object[])sender)[0]);
            if(list.Contains("All") || list.Contains(Name))
            {
                LoadCurrentConfiguration();
                UpdateTrayIcon();
            }
        }

        private void controller_ToggleModeChanged(object sender, EventArgs e) {
            Configuration config = _controller.GetCurrentConfiguration();
            UpdateSysProxyMode(config);
            timerDisconnectCurrent.Stop();
            if (config.sysProxyMode == (int)ProxyMode.Direct)
                timerDisconnectCurrent.Start();
        }

        private void controller_ToggleRuleModeChanged(object sender, EventArgs e) {
            Configuration config = _controller.GetCurrentConfiguration();
            UpdateProxyRule(config);
        }

        void controller_FileReadyToOpen(object sender, ShadowsocksController.PathEventArgs e) {
            string argument = @"/select, " + e.Path;

            System.Diagnostics.Process.Start("explorer.exe", argument);
        }

        void ShowBalloonTip(string title, string content, ToolTipIcon icon, /*int timeout, */int type = 0)
        {
            _notifyIcon.BalloonTipClicked -= newVersionFound_BalloonTipClicked;
            _notifyIcon.BalloonTipClicked -= UpdateSubscriptionErrorsOrDuplicates_BalloonTipClicked;

            if (type == 1)//new version found
            {
                _notifyIcon.BalloonTipClicked += newVersionFound_BalloonTipClicked;
            }
            else if (type == 2)//Update subscription has errors or duplicates
            {
                _notifyIcon.BalloonTipClicked += UpdateSubscriptionErrorsOrDuplicates_BalloonTipClicked;
            }
            if (!Program.SystemInFullScreenMode && _notifyIcon != null)
            {
                _notifyIcon.BalloonTipTitle = title;
                _notifyIcon.BalloonTipText = content;
                _notifyIcon.BalloonTipIcon = icon;
                _notifyIcon.ShowBalloonTip(0);
            }
        }

        void controller_UpdatePACFromGFWListError(object sender, System.IO.ErrorEventArgs e) {
            GFWListUpdater updater = (GFWListUpdater)sender;
            ShowBalloonTip(I18N.GetString("Failed to update PAC file"), e.GetException().Message, ToolTipIcon.Error);
            Logging.LogUsefulException(e.GetException());
        }

        void controller_UpdatePACFromGFWListCompleted(object sender, GFWListUpdater.ResultEventArgs e) {
            GFWListUpdater updater = (GFWListUpdater)sender;
            string result = e.Success ?
                (updater.update_type <= 1 ? I18N.GetString("PAC updated") : I18N.GetString("Domain white list list updated"))
                : I18N.GetString("No updates found. Please report to GFWList if you have problems with it.");
            ShowBalloonTip(I18N.GetString("Shadowsocks"), result, ToolTipIcon.Info);
        }

        void updateFreeNodeChecker_NewFreeNodeFound(object sender, EventArgs e)
        {
            object[] obj = (object[])sender;
            if (obj.Length > 3)
            {
                while (true)
                {
                    if (updateFreeNodeChecker.Interrupt)
                    {
                        mtupdatenode.WaitOne();
                        updateFreeNodeChecker.mtmwcRunningCheck.WaitOne(-1);
                        updateFreeNodeChecker.RenewNodeCount--;
                        Logging.Debug(String.Format("interrupt RenewNodeCount {0}", updateFreeNodeChecker.RenewNodeCount));
                        if (updateFreeNodeChecker.RenewNodeCount==0 && updateFreeNodeChecker.mwcRunningCount==0)
                            updateSubscribeManager.InProcessing = false;
                        updateFreeNodeChecker.mtmwcRunningCheck.ReleaseMutex();
                        mtupdatenode.ReleaseMutex();
                        return;
                    }
                    mtupdatenode.WaitOne();
                    updateFreeNodeChecker.mtmwcRunningCheck.WaitOne(-1);
                    if (updateFreeNodeChecker.RenewNodeCount == 0)
                    {
                        updateFreeNodeChecker.mtmwcRunningCheck.ReleaseMutex();
                        break;
                    }
                    updateFreeNodeChecker.mtmwcRunningCheck.ReleaseMutex();
                    mtupdatenode.ReleaseMutex();
                }
                UpdateFreeNodeEnd(sender);
                Logging.Debug(String.Format("end RenewNodeCount {0}", updateFreeNodeChecker.RenewNodeCount));
                mtupdatenode.ReleaseMutex();
                return;
            }


            string ReceivedNode = (string)obj[0];
            ServerSubscribe task = (ServerSubscribe)obj[1];
            bool resultstatus = (bool)obj[2];

            try
            {
                mtupdatenode.WaitOne();
                if (updateSubscribeManager.Interrupt)
                    throw new Exception("Interrupt");
                if (configfrom_open)
                {
                    if (timerCheckUpdate != null && timerCheckUpdate.Enabled)
                        timerCheckUpdate.Stop();
                    eventList.Add(new EventParams(sender, e));
                    return;
                }

                ReceivedNode = ReceivedNode.Trim('\r', '\n', ' ');


                string lastGroup = null;
                Configuration config = _controller.GetCurrentConfiguration();
                if (resultstatus)
                {
                    List<string> urls = new List<string>();
                    Server selected_server = null;
                    int max_node_num = 0;
                    if (config.index >= 0 && config.index < config.Servers.Count)
                    {
                        selected_server = config.Servers[config.index];
                    }
                    if (!String.IsNullOrEmpty(ReceivedNode))
                    {
                        try
                        {
                            ReceivedNode = Util.Base64.DecodeBase64(ReceivedNode);
                        }
                        catch
                        {
                            ReceivedNode = "";
                        }


                        Match match_maxnum = Regex.Match(ReceivedNode, "^MAX=([0-9]+)");
                        if (match_maxnum.Success)
                        {
                            try
                            {
                                max_node_num = Convert.ToInt32(match_maxnum.Groups[1].Value, 10);
                            }
                            catch
                            {

                            }
                        }
                        URL_Split(ReceivedNode, ref urls);
                        for (int i = urls.Count - 1; i >= 0; --i)
                        {
                            if (!urls[i].StartsWith("ssr"))
                                urls.RemoveAt(i);
                        }
                    }
                    string invalidservergroupname = I18N.GetString("Server subscription did not return a valid server.");
                    if (urls.Count > 0)
                    {
                        bool keep_selected_server = false; // set 'false' if import all nodes
                        if (max_node_num <= 0 || max_node_num >= urls.Count)
                        {
                            urls.Reverse();
                        }
                        else
                        {
                            Random r = new Random();
                            Util.Utils.Shuffle(urls, r);
                            urls.RemoveRange(max_node_num, urls.Count - max_node_num);
                            if (!config.isDefaultConfig())
                                keep_selected_server = true;
                        }
                        string curGroup = null;
                        int subscribeindex = config.serverSubscribes.FindIndex(t => t.id == task.id);
                        foreach (string url in urls)
                        {
                            try // try get group name
                            {
                                Server server = new Server(url, null);
                                if (server.enable && !String.IsNullOrEmpty(server.group))
                                {
                                    curGroup = server.group;
                                    break;
                                }
                            }
                            catch { }
                        }
                        if (String.IsNullOrEmpty(curGroup))
                        {
                            curGroup = task.Group;
                        }
                        else
                        {
                            if(!String.IsNullOrEmpty(task.Group))
                                while (true)
                                {
                                    if (task.groupUserDefine && curGroup == task.Group)
                                    {
                                        task.groupUserDefine = false;
                                        if (subscribeindex != -1)
                                            config.serverSubscribes[subscribeindex].groupUserDefine = false;
                                        continue;
                                    }
                                    if (!task.groupUserDefine && curGroup != task.Group)
                                    {
                                        int index = -1;
                                        while ((index = config.Servers.FindIndex(t => t.group == task.Group)) != -1)
                                        {
                                            config.Servers[index].group = curGroup;
                                        }
                                        task.Group = curGroup;
                                        if (subscribeindex != -1)
                                        {
                                            config.serverSubscribes[subscribeindex].Group = curGroup;
                                        }
                                        continue;
                                    }
                                    if (task.groupUserDefine)
                                    {
                                        curGroup = task.Group;
                                    }

                                    break;
                                }
                            else
                            {
                                task.Group = curGroup;
                                if (subscribeindex != -1)
                                    config.serverSubscribes[subscribeindex].Group = curGroup;
                            }

                        }
                        lastGroup = task.Group;

                        if (String.IsNullOrEmpty(curGroup))
                        {
                            curGroup = task.URL;
                        }
                        if (String.IsNullOrEmpty(lastGroup))
                        {
                            lastGroup = curGroup;
                        }

                        if (keep_selected_server && selected_server != null && selected_server.group == curGroup)
                        {
                            bool match = false;
                            for (int i = 0; i < urls.Count; ++i)
                            {
                                try
                                {
                                    Server server = new Server(urls[i], null);
                                    if (server.enable && selected_server.isMatchServer(server))
                                    {
                                        match = true;
                                        break;
                                    }
                                }
                                catch { }
                            }
                            if (!match)
                            {
                                urls.RemoveAt(0);
                                urls.Add(selected_server.GetSSRLinkForServer());
                            }
                        }

                        // import all, find difference
                        {
                            Dictionary<string, Server> old_servers = new Dictionary<string, Server>();
                            Dictionary<string, Server> old_insert_servers = new Dictionary<string, Server>();
                            if (!String.IsNullOrEmpty(lastGroup))
                            {
                                for (int i = config.Servers.Count - 1; i >= 0; --i)
                                {
                                    if (lastGroup == config.Servers[i].group)
                                    {
                                        old_servers[config.Servers[i].id] = config.Servers[i];
                                    }
                                }
                            }
                            foreach (string url in urls)
                            {
                                try
                                {
                                    Server server = new Server(url, curGroup);
                                    if (!server.enable)
                                        continue;
                                    bool match = false;
                                    if (!match)
                                    {
                                        foreach (KeyValuePair<string, Server> pair in old_insert_servers)
                                        {
                                            if (server.isMatchServer(pair.Value))
                                            {
                                                match = true;
                                                break;
                                            }
                                        }
                                    }
                                    old_insert_servers[server.id] = server;
                                    if (!match)
                                    {
                                        foreach (KeyValuePair<string, Server> pair in old_servers)
                                        {
                                            if (server.isMatchServer(pair.Value))
                                            {
                                                match = true;
                                                old_servers.Remove(pair.Key);
                                                pair.Value.CopyServerInfo(server);
                                                //++count;
                                                break;
                                            }
                                        }
                                    }
                                    if (!match)
                                    {
                                        int insert_index = -1;
                                        if ((insert_index = config.Servers.FindLastIndex(t => t.group == curGroup) + 1) == 0)
                                            insert_index = config.Servers.Count;
                                        config.Servers.Insert(insert_index, server);
                                        //++count;
                                    }
                                }
                                catch { }
                            }
                            foreach (KeyValuePair<string, Server> pair in old_servers)
                            {
                                int index = config.Servers.FindIndex(t => t.id == pair.Key);
                                if(index!=-1)
                                    config.Servers.RemoveAt(index);
                            }
                        }
                        int invalidserverindex = -1;
                        while ((invalidserverindex = config.Servers.FindIndex(t => t.group == invalidservergroupname && (t.remarks == task.Group + " " + task.URL || t.remarks == task.URL))) != -1) 
                        {
                            config.Servers.RemoveAt(invalidserverindex);
                        }

                    }
                    else if (task.Group != "")
                    {
                        lastGroup = task.Group;
                        int index = -1;
                        while ((index = config.Servers.FindIndex(t => t.group == lastGroup)) != -1) 
                        {
                            config.Servers.RemoveAt(index);
                        }
                        int firstinvalidserverindex = config.Servers.FindIndex(t => t.group == invalidservergroupname && (t.remarks == task.Group + " " + task.URL || t.remarks == task.URL));
                        while (firstinvalidserverindex != -1 && (index = config.Servers.FindLastIndex(t => t.group == invalidservergroupname && (t.remarks == task.Group + " " + task.URL || t.remarks == task.URL))) != -1 && index != firstinvalidserverindex) 
                        {
                            config.Servers.RemoveAt(index);
                        }

                        if (firstinvalidserverindex == -1)
                        {
                            Server server = new Server();
                            server.group = invalidservergroupname;
                            server.remarks = (String.IsNullOrEmpty(task.Group) ? "" : task.Group + " ") + task.URL;
                            server.enable = false;
                            config.Servers.Insert(config.Servers.FindIndex(t => t.group != invalidservergroupname), server);
                        }
                    }
                    else
                    {
                        lastGroup = (task.index + 1).ToString();
                        int firstinvalidserverindex = config.Servers.FindIndex(t => t.group == invalidservergroupname && t.remarks == (String.IsNullOrEmpty(task.Group) ? "" : task.Group + " ") + task.URL);
                        int index = -1;
                        while (firstinvalidserverindex != -1 && firstinvalidserverindex != (index = config.Servers.FindLastIndex(t => t.group == invalidservergroupname && t.remarks == (String.IsNullOrEmpty(task.Group) ? "" : task.Group + " ") + task.URL))) 
                        {
                            config.Servers.RemoveAt(index);
                        }
                        if (firstinvalidserverindex == -1)
                        {
                            Server server = new Server();
                            server.group = invalidservergroupname;
                            server.remarks = (String.IsNullOrEmpty(task.Group) ? "" : task.Group + " ") + task.URL;
                            server.enable = false;
                            config.Servers.Insert(config.Servers.FindIndex(t => t.group != invalidservergroupname), server);
                        }
                    }

                    int newselectedserverindex = -1;
                    if (selected_server != null && config.index < config.Servers.Count) 
                    {
                        if (selected_server.id != config.Servers[config.index].id)
                        {
                            newselectedserverindex = config.Servers.FindIndex(t => t.id == selected_server.id);
                            if (newselectedserverindex != -1 || (newselectedserverindex = config.Servers.FindIndex(t => t.enable)) != -1)
                                config.index = newselectedserverindex;
                            else
                                config.index = 0;
                        }
                    }
                    else
                    {
                        if ((newselectedserverindex = config.Servers.FindIndex(t => t.enable)) != -1) 
                            config.index = newselectedserverindex;
                        else
                            config.index = 0;
                    }
                    _controller.SaveServersConfig(config, false);
                    if (updateFreeNodeChecker.notify)
                        ShowBalloonTip(I18N.GetString("Success"),
                            string.Format(I18N.GetString("Update subscribe {0} success"), lastGroup), ToolTipIcon.Info);
                }
                else
                {
                    if (lastGroup == null)
                    {
                        if (task.Group != "")
                            lastGroup = task.Group;
                        else
                        {
                            lastGroup = (task.index + 1).ToString();
                        }
                    }
                    ShowBalloonTip(I18N.GetString("Tips"), String.Format(I18N.GetString("Update subscribe {0} failure"), lastGroup), ToolTipIcon.Info);
                }

                if (task.index > -1 && task.index < config.serverSubscribes.Count) 
                {
                    config.serverSubscribes[task.index].LastUpdateTime = (UInt64)Math.Floor(DateTime.Now.Subtract(new DateTime(1970, 1, 1, 0, 0, 0)).TotalSeconds);
                }
            }
            catch (Exception ex)
            {
                if (ex.Message == "Interrupt")
                {
                    updateFreeNodeChecker.mtmwcRunningCheck.WaitOne(-1);
                    if (updateFreeNodeChecker.RenewNodeCount == 1)
                    {
                        _controller.SaveServersConfig(_controller.GetCurrentConfiguration());
                    }
                    updateFreeNodeChecker.mtmwcRunningCheck.ReleaseMutex();
                }
                else
                    Logging.Log(LogLevel.Error, String.Format("Subscribes {0} error" + System.Environment.NewLine + "{1}", task != null ? (task.index + 1).ToString() : "reference error", ex.Message));
            }
            finally
            {
                updateFreeNodeChecker.RenewNodeCount--;
                Logging.Debug(String.Format("task {0}  RenewNodeCount {1}", task.index + 1, updateFreeNodeChecker.RenewNodeCount));
                mtupdatenode.ReleaseMutex();
            }
        }

        private void UpdateFreeNodeEnd(object _obj)
        {
            object[] obj = (object[])_obj;
            List<ServerSubscribe> AllTask = (List<ServerSubscribe>)obj[0];
            bool AllFailed = (bool)obj[1];
            List<string> listrepeatedstr = (List<string>)obj[2];
            List<string> listfailedstr = (List<string>)obj[3];

            Configuration config = _controller.GetCurrentConfiguration();
            config.LastUpdateSubscribesTime = (UInt64)Math.Floor(DateTime.Now.Subtract(new DateTime(1970, 1, 1, 0, 0, 0)).TotalSeconds);

            if (config.sortServersBySubscriptionsOrder)
                Util.Utils.SortServers(config);



            StringBuilder sbfinalmsg = new StringBuilder();
            StringBuilder sbrepeatmsg = new StringBuilder();
            StringBuilder sbfailmsg = new StringBuilder();
            StringBuilder sbfaillinks = new StringBuilder();

            int index = -1;
            List<int> listrepeatedint = new List<int>();
            foreach (string id in listrepeatedstr)
            {
                index = config.serverSubscribes.FindIndex(t => t.id == id);
                if (index != -1)
                    listrepeatedint.Add(index);
                else
                    sbrepeatmsg.Append((sbrepeatmsg.Length == 0 ? "" : ",") + id);
            }
            for (int i = 0; i < listrepeatedint.Count; i++)
            {
                if (i > 0 && i < listrepeatedint.Count - 1 && listrepeatedint[i - 1] + 1 == listrepeatedint[i] && listrepeatedint[i] + 1 == listrepeatedint[i + 1])
                {
                    if (sbrepeatmsg.Length > 0 && sbrepeatmsg[sbrepeatmsg.Length - 1] != '~') 
                    {
                        sbrepeatmsg.Append("~");
                    }
                    continue;
                }
                sbrepeatmsg.Append(((sbrepeatmsg.Length == 0 || sbrepeatmsg[sbrepeatmsg.Length - 1] == '~') ? "" : ",") + (index + 1).ToString());
            }
            index = -1;
            List<int> listfailedint = new List<int>();
            foreach (string id in listfailedstr)
            {
                index = config.serverSubscribes.FindIndex(t => t.id == id);
                if (index != -1)
                    listfailedint.Add(index);
                else
                    sbfailmsg.Append((sbfailmsg.Length == 0 ? "" : ",") + id);
            }
            listfailedint.Sort();
            for (int i = 0; i < listfailedint.Count; i++)
            {
                sbfaillinks.Append((sbfaillinks.Length == 0 ? "" : ",") + config.serverSubscribes[listfailedint[i]].URL);
                if (i > 0 && i < listfailedint.Count - 1 && listfailedint[i - 1] + 1 == listfailedint[i] && listfailedint[i] + 1 == listfailedint[i + 1]) 
                {
                    if (sbfailmsg.Length > 0 && sbfailmsg[sbfailmsg.Length - 1] != '~') 
                    {
                        sbfailmsg.Append("~");
                    }
                    continue;
                }
                sbfailmsg.Append(((sbfailmsg.Length == 0 || sbfailmsg[sbfailmsg.Length - 1] == '~') ? "" : ",") + (listfailedint[i] + 1).ToString());
            }

            if (!updateSubscribeManager.UpdateAll)
            {
                List<string> listfaillinks = new List<string>(sbfaillinks.ToString().Split(','));
                foreach (ServerSubscribe ss in AllTask)
                {
                    string url = ss.URL;
                    int index1 = listfaillinks.FindIndex(t => t == url);
                    int index2 = UpdateSubscribeManager.listSubscribeFailureLinks.FindIndex(t => t == url);
                    if (-1 == index1)
                    {
                        if (-1 != index2)
                            UpdateSubscribeManager.listSubscribeFailureLinks.RemoveAt(index2);
                    }
                    else
                    {
                        if (-1 == index2)
                            UpdateSubscribeManager.listSubscribeFailureLinks.Add(listfaillinks[index1]);
                    }
                }
            }

            if (sbrepeatmsg.Length > 0 || sbfailmsg.Length > 0)
            {
                if (sbfailmsg.Length > 0)
                {
                    if (sbfaillinks.Length > 0)
                    {
                        if (updateSubscribeManager.UpdateAll)
                            UpdateSubscribeManager.listSubscribeFailureLinks = new List<string>(sbfaillinks.ToString().Split(','));
                    }
                    if (AllFailed)
                    {
                        sbfailmsg.Clear();
                        sbfailmsg.Append(I18N.GetString("All"));
                    }
                    sbfinalmsg.Append(String.Format(I18N.GetString("Update subscribe {0} failure"), sbfailmsg.ToString()));
                }

                if (sbrepeatmsg.Length > 0)
                {
                    sbfinalmsg.Append((sbfinalmsg.Length == 0 ? "" : ",") + String.Format(I18N.GetString("Subscribe {0} repeat"), sbrepeatmsg.ToString()));
                }
                Logging.Log(LogLevel.Info, sbfinalmsg.ToString());
                ShowBalloonTip(I18N.GetString("Subscribe"), sbfinalmsg.ToString(), ToolTipIcon.Info, 2);

            }
            else
            {
                if (updateSubscribeManager.UpdateAll/* updateFreeNodeChecker.result.AllSubscribes*/)
                    sbfinalmsg.Append(I18N.GetString("Update all subscribe success"));
                else
                    sbfinalmsg.Append(I18N.GetString("Update selected subscribe success"));

                if (updateSubscribeManager.notify)
                    ShowBalloonTip(I18N.GetString("Subscribe"), sbfinalmsg.ToString(), ToolTipIcon.Info);
            }

            Configuration.Save(config);
            _controller.JustReload();
            updateSubscribeManager.InProcessing = false;
            Logging.Log(LogLevel.Info, "Update server subscribes succeed.");

            if (config.nodeFeedAutoLatency)
            {
                Util.TCPingManager.Enabled = true;
                Util.TCPingManager.Interrupt = false;
                Util.TCPingManager.listTerminator.Clear();
                Logging.Debug("Terminator :cleared");
                if (!updateSubscribeManager.UpdateAll)
                {
                    List<string> tmplist = new List<string>();
                    foreach(ServerSubscribe ss in AllTask)
                    {
                        string groupname = ss.Group;
                        if (groupname == "")
                            continue;
                        for (int i = 0; i < config.Servers.Count; i++)
                        {
                            Server server = config.Servers[i];
                            if(groupname == server.group)
                            {
                                tmplist.Add(server.id);
                            }
                        }

                    }
                    Util.TCPingManager.StartTcping(_controller, tmplist);
                    timerUpdateLatency.Interval = 1000.0 * timerUpdateLatencyInterval;

                }
                else
                {
                    timerUpdateLatency.Interval = 1000.0;
                }
                timerUpdateLatency.Start();
            }
            else 
            {
                ResumeLatencyTest("Updating Subscriptions");
            }
        }

        private void ResumeOperation()
        {

        }

        private void ResumeLatencyTest(string Restorer)
        {
            Util.TCPingManager.RestoreTcping(Restorer);
            if (_controller.GetCurrentConfiguration().nodeFeedAutoLatency && !timerUpdateLatency.Enabled)
                timerUpdateLatency.Start();
        }

        void UpdateSubscriptionErrorsOrDuplicates_BalloonTipClicked(object sender, EventArgs e)
        {
            _notifyIcon.BalloonTipClicked -= UpdateSubscriptionErrorsOrDuplicates_BalloonTipClicked;
            SubscribeSetting_Click(null, null);
        }

        void updateChecker_NewVersionFound(object sender, EventArgs e)
        {
            double version;
            if(double.TryParse(updateChecker.LatestVersionNumber, out version))
            {
                switch (version)
                {
                    case -1:
                        Logging.Log(LogLevel.Info, "connect to update server error.");
                        ShowBalloonTip(I18N.GetString("Check update"), I18N.GetString("Connecting update server error."), ToolTipIcon.Info);
                        break;
                    case 0:
                        Logging.Log(LogLevel.Info, "No new version found.");
                        if (updateSubscribeManager.notify)
                            ShowBalloonTip(I18N.GetString("Check update"), I18N.GetString("No new version found."), ToolTipIcon.Info);
                        break;
                    default:
                        break;
                }
            }
            else
            {
                if (!UpdateItem.Visible)
                {
                    UpdateItem.Visible = true;
                    UpdateItem.Text = String.Format(I18N.GetString("New version {0} available"), updateChecker.LatestVersionNumber);
                }
                Logging.Log(LogLevel.Info, String.Format("New version {0} found.", updateChecker.LatestVersionNumber));
                ShowBalloonTip(String.Format(I18N.GetString("New version {0} available"), updateChecker.LatestVersionNumber)/*String.Format(I18N.GetString("{0} {1} Update Found"), updateChecker.LatestVersionNumber)*/,
                    I18N.GetString("Click to download"), ToolTipIcon.Info, 1);
            }
        }
        
        void UpdateItem_Clicked(object sender, EventArgs e)
        {
            Process.Start(updateChecker.LatestVersionURL);
        }

        void newVersionFound_BalloonTipClicked(object sender, EventArgs e)
        {
            _notifyIcon.BalloonTipClicked -= newVersionFound_BalloonTipClicked;
            UpdateItem_Clicked(null, null);
        }

        private void UpdateSysProxyMode(Configuration config) {
            noModifyItem.Checked = config.sysProxyMode == (int)ProxyMode.NoModify;
            enableItem.Checked = config.sysProxyMode == (int)ProxyMode.Direct;
            PACModeItem.Checked = config.sysProxyMode == (int)ProxyMode.Pac;
            globalModeItem.Checked = config.sysProxyMode == (int)ProxyMode.Global;
        }

        private void UpdateProxyRule(Configuration config) {
            ruleDisableBypass.Checked = config.proxyRuleMode == (int)ProxyRuleMode.Disable;
            ruleBypassLan.Checked = config.proxyRuleMode == (int)ProxyRuleMode.BypassLan;
            ruleBypassLanAndChina.Checked = config.proxyRuleMode == (int)ProxyRuleMode.BypassLanAndChina;
            ruleBypassLanAndNotChina.Checked = config.proxyRuleMode == (int)ProxyRuleMode.BypassLanAndNotChina;
            ruleUser.Checked = config.proxyRuleMode == (int)ProxyRuleMode.UserCustom;
        }

        private void LoadCurrentConfiguration() {
            Configuration config = _controller.GetCurrentConfiguration();
            UpdateServersMenu();
            UpdateSysProxyMode(config);

            UpdateProxyRule(config);

            SelectedEnableBalanceItem.Checked = config.enableBalance;
            sameHostForSameTargetItem.Checked = config.sameHostForSameTarget;

            timerCheckSubscriptionsUpdateInterval = 60 * 60 * config.nodeFeedAutoUpdateInterval;
            timerUpdateLatencyInterval = 60 * config.nodeFeedAutoLatencyInterval;
            initTimers();

            Logging.Debug("timerCheckSubscriptionsUpdateInterval=" + config.nodeFeedAutoUpdateInterval.ToString() + ",timerUpdateLatencyInterval=" + ((float)config.nodeFeedAutoLatencyInterval / 60).ToString());
        }

        private void UpdateServersMenu() {
            var items = ServersItem.MenuItems;
            while (items[0] != SeperatorItem) {
                items.RemoveAt(0);
            }

            Configuration configuration = _controller.GetCurrentConfiguration();
            SortedDictionary<string, MenuItem> group = new SortedDictionary<string, MenuItem>();
            const string def_group = "!(no group)";
            string select_group = "";
            for (int i = 0; i < configuration.Servers.Count; i++) {
                string group_name;
                Server server = configuration.Servers[i];
                if (string.IsNullOrEmpty(server.group))
                    group_name = def_group;
                else
                    group_name = server.group;

                string latency;
                if (server.latency == Server.LATENCY_TESTING)
                {
                    latency = "[testing]";
                }
                else if (server.latency == Server.LATENCY_ERROR)
                {
                    latency = "[error]";
                }
                else if (server.latency == Server.LATENCY_PENDING)
                {
                    latency = "[pending]";
                }
                else
                {
                    latency = "[" + server.latency.ToString() + "ms]";
                }
                MenuItem item = new MenuItem(latency + " " + server.FriendlyName());
                item.Tag = i;
                item.Click += AServerItem_Click;
                if (configuration.index == i) {
                    item.Checked = true;
                    select_group = group_name;
                }

                if (group.ContainsKey(group_name)) {
                    group[group_name].MenuItems.Add(item);
                }
                else {
                    group[group_name] = new MenuItem(group_name, new MenuItem[1] { item });
                }
            }
            {
                int i = 0;
                foreach (KeyValuePair<string, MenuItem> pair in group) {
                    if (pair.Key == def_group) {
                        pair.Value.Text = I18N.GetString("(empty group)");
                    }
                    if (pair.Key == select_group) {
                        pair.Value.Text = "● " + pair.Value.Text;
                    }
                    else {
                        pair.Value.Text = "　" + pair.Value.Text;
                    }
                    items.Add(i, pair.Value);
                    ++i;
                }
            }
        }

        private void ShowConfigForm(List<int> listNewNode = null)
        {
            mtupdatenode.WaitOne(-1);
            mtupdatenode.ReleaseMutex();

            if (configForm != null)
            {
                configForm.Activate();
                configForm.BringToFront();
            }
            else
            {
                configfrom_open = true;
                configForm = new ConfigForm(_controller, updateChecker/*, addNode ? -1 : -2*/);
                configForm.Show();
                configForm.Activate();
                configForm.BringToFront();
                configForm.FormClosed += configForm_FormClosed;
            }
            if (listNewNode == null)
            {
                configForm.SetSelectedIndex(new List<int>() { _controller.GetCurrentConfiguration().index });
                configForm.SetLstServersTopIndex();
            }
            else
            {
                configForm.SetSelectedIndex(listNewNode);
                if (listNewNode.Count == 1)
                    configForm.SetLstServersTopIndex();
                else if (listNewNode.Count > 1)
                    configForm.SetLstServersTopIndex(listNewNode[0] - 1);
            }

        }

        private void ShowSettingForm() {
            if (settingsForm != null) {
                settingsForm.Activate();
                settingsForm.BringToFront();
            }
            else {
                settingsForm = new SettingsForm(_controller);
                settingsForm.Show();
                settingsForm.Activate();
                settingsForm.BringToFront();
                settingsForm.FormClosed += settingsForm_FormClosed;
            }
        }

        private void ShowPortMapForm() {
            if (portMapForm != null) {
                portMapForm.Activate();
                portMapForm.BringToFront();
                portMapForm.Update();
                if (portMapForm.WindowState == FormWindowState.Minimized) {
                    portMapForm.WindowState = FormWindowState.Normal;
                }
            }
            else {
                portMapForm = new PortSettingsForm(_controller);
                portMapForm.Show();
                portMapForm.Activate();
                portMapForm.BringToFront();
                portMapForm.FormClosed += portMapForm_FormClosed;
            }
        }

        private bool IsOutOfScreen(APoint formLocation)
        {
            if (formLocation.X < 0 || formLocation.Y < 0 || formLocation.X > Screen.PrimaryScreen.Bounds.Width || formLocation.Y > Screen.PrimaryScreen.Bounds.Height)
                return true;
            else
                return false;
        }

        private void ShowServerLogForm() {
            if (serverLogForm != null) {
                serverLogForm.Activate();
                serverLogForm.BringToFront();
                serverLogForm.Update();
                if (serverLogForm.WindowState == FormWindowState.Minimized) {
                    serverLogForm.WindowState = FormWindowState.Normal;
                }
            }
            else {
              Configuration config = _controller.GetCurrentConfiguration();
                serverLogForm = new ServerLogForm(_controller);
                serverLogForm.Show();
                if (config.ServerLogFormLocation != null && !IsOutOfScreen(config.ServerLogFormLocation))
                    serverLogForm.Location = (Point)new Size(config.ServerLogFormLocation.X, config.ServerLogFormLocation.Y);
                serverLogForm.Activate();
                serverLogForm.BringToFront();
                serverLogForm.FormClosed += serverLogForm_FormClosed;
            }
        }

        private void ShowGlobalLogForm() {
            if (logForm != null) {
                logForm.Activate();
                logForm.BringToFront();
                logForm.Update();
                if (logForm.WindowState == FormWindowState.Minimized) {
                    logForm.WindowState = FormWindowState.Normal;
                }
            }
            else {
                logForm = new LogForm(_controller);
                logForm.Show();
                logForm.Activate();
                logForm.BringToFront();
                logForm.FormClosed += globalLogForm_FormClosed;
            }
        }

        private void ShowSubscribeSettingForm() {
            if (subScribeForm != null) {
                subScribeForm.Activate();
                subScribeForm.BringToFront();
                subScribeForm.Update();
                if (subScribeForm.WindowState == FormWindowState.Minimized) {
                    subScribeForm.WindowState = FormWindowState.Normal;
                }
            }
            else
            {
                subScribeForm_open = true;
                subScribeForm = new SubscribeForm(_controller);
                subScribeForm.Show();
                subScribeForm.Activate();
                subScribeForm.BringToFront();
                subScribeForm.FormClosed += subScribeForm_FormClosed;
            }
        }

        void configForm_FormClosed(object sender, FormClosedEventArgs e) {
            IDataObject iData = Clipboard.GetDataObject();
            if (iData.GetDataPresent(DataFormats.Text))
            {
                string str = (string)iData.GetData(DataFormats.Text);
                if(str.StartsWith("ss://",StringComparison.OrdinalIgnoreCase) || str.StartsWith("ssr://", StringComparison.OrdinalIgnoreCase))
                    Clipboard.Clear();
            }
            configfrom_open = false;
            configForm.Dispose();
            configForm = null;
            Utils.ReleaseMemory(true);
            if (eventList.Count > 0)
            {
                foreach (EventParams p in eventList)
                {
                    updateFreeNodeChecker_NewFreeNodeFound(p._sender, p._e);
                }
                eventList.Clear();
                if (_controller.GetCurrentConfiguration().nodeFeedAutoUpdate && !timerCheckUpdate.Enabled) timerCheckUpdate.Start();
            }
        }

        void settingsForm_FormClosed(object sender, FormClosedEventArgs e) {
                settingsForm.Dispose();
                settingsForm = null;
                Utils.ReleaseMemory(true);
        }

        void serverLogForm_FormClosed(object sender, FormClosedEventArgs e) {
            serverLogForm.Dispose();
            serverLogForm = null;
            Util.Utils.ReleaseMemory(true);
        }

        void portMapForm_FormClosed(object sender, FormClosedEventArgs e) {
            portMapForm.Dispose();
            portMapForm = null;
            Util.Utils.ReleaseMemory(true);
        }

        void globalLogForm_FormClosed(object sender, FormClosedEventArgs e) {
            logForm.Dispose();
            logForm = null;
            Util.Utils.ReleaseMemory(true);
        }

        void subScribeForm_FormClosed(object sender, FormClosedEventArgs e) {
            if (subScribeForm != null)
            {
                subScribeForm.Dispose();
                subScribeForm = null;
            }
            subScribeForm_open = false;
        }

        private void Config_Click(object sender, EventArgs e) {
            if (typeof(int) == sender.GetType()) {
                ShowConfigForm(new List<int>() { (int)sender });
            }
            else {
                ShowConfigForm();
            }
        }

        private void Import_Click(object sender, EventArgs e) {
            using (OpenFileDialog dlg = new OpenFileDialog()) {
                dlg.InitialDirectory = System.Windows.Forms.Application.StartupPath;
                if (dlg.ShowDialog() == DialogResult.OK) {
                    string name = dlg.FileName;
                    Configuration cfg = Configuration.LoadFile(name);
                    if (cfg.Servers.Count == 1 && cfg.Servers[0].server == Configuration.GetDefaultServer().server) {
                        MessageBox.Show("Load config file failed", "ShadowsocksR");
                    }
                    else {
                        _controller.MergeConfiguration(cfg);
                        LoadCurrentConfiguration();
                    }
                }
            }
        }

        private void Setting_Click(object sender, EventArgs e) {
            ShowSettingForm();
        }

        private void Quit() {
            SystemEvents.SessionEnding -= Quit_Click;
            Util.TCPingManager.Dispose();
            if (updateSubscribeManager.InProcessing)
                updateSubscribeManager.Dispose();
            HotKeys.Destroy();
            if (appbarform != null)
            {
                appbarform.RegAppBar(true);
                appbarform.Dispose();
                appbarform = null;
            }
            if (configForm != null)
            {
                eventList.Clear();
                configForm.Close();
                configForm = null;
                configfrom_open = false;
            }
            if (serverLogForm != null) {
                serverLogForm.Close();
                serverLogForm = null;
            }
            if (hotkeySettingsForm != null)
            {
                hotkeySettingsForm.Close();
                hotkeySettingsForm = null;
            }
            if (subScribeForm != null)
            {
                eventList.Clear();
                subScribeForm.Close();
                subScribeForm = null;
            }
            if (timerCheckUpdate != null)
            {
                timerCheckUpdate.Stop();
                timerCheckUpdate.Elapsed -= timerCheckSubscriptionsUpdate_Elapsed;
                timerCheckUpdate = null;
            }
            if (timerUpdateLatency != null)
            {
                timerUpdateLatency.Stop();
                timerUpdateLatency.Elapsed -= timerUpdateLatency_Elapsed;
                timerUpdateLatency = null;
            }
            //if(timerCheckSystemTime!=null)
            //{
            //    timerCheckSystemTime.Stop();
            //    timerCheckSystemTime.Elapsed -= TimerCheckSystemTime_Elapsed;
            //    timerCheckSystemTime = null;
            //}
            if (_notifyIcon != null)
            {
                _notifyIcon.Visible = false;
                _notifyIcon = null;
            }
            if (_controller != null)
                _controller.Stop();
            Application.Exit();
        }

        public void Quit_Click(object sender, EventArgs e)
        {
            Quit();
        }

        private void OpenWiki_Click(object sender, EventArgs e) {
            Process.Start("https://github.com/shadowsocksrr/shadowsocks-rss/wiki");
        }

        private void FeedbackItem_Click(object sender, EventArgs e) {
            Process.Start("https://github.com/shadowsocksrr/shadowsocksr-csharp/issues/new");
        }

        private void ResetPasswordItem_Click(object sender, EventArgs e) {
            ResetPassword dlg = new ResetPassword();
            dlg.Show();
            dlg.Activate();
            dlg.BringToFront();
        }

        private void AboutItem_Click(object sender, EventArgs e)
        {
            Process.Start("https://github.com/githubzgr/SSRR-Windows--nth-party-");
            //Process.Start("https://github.com/SoDa-GitHub/shadowsocksrr-csharp");
        }

        private void DonateItem_Click(object sender, EventArgs e) {
            Process.Start("https://github.com/SoDa-GitHub/shadowsocksrr-csharp/blob/master/donate.jpg?raw=true");
        }

        [DllImport("user32.dll")]
        private static extern short GetAsyncKeyState(System.Windows.Forms.Keys vKey);

        private void notifyIcon_Click(object sender, MouseEventArgs e) {
            if (e.Button == MouseButtons.Left) {
                int SCA_key = GetAsyncKeyState(Keys.ShiftKey) < 0 ? 1 : 0;
                SCA_key |= GetAsyncKeyState(Keys.ControlKey) < 0 ? 2 : 0;
                SCA_key |= GetAsyncKeyState(Keys.Menu) < 0 ? 4 : 0;
                if (SCA_key == 2) {
                    ShowServerLogForm();
                }
                else if (SCA_key == 1) {
                    ShowSettingForm();
                }
                else if (SCA_key == 4) {
                    ShowPortMapForm();
                }
                else {
                    ShowConfigForm();
                }
            }
            else if (e.Button == MouseButtons.Middle) {
                ShowServerLogForm();
            }
        }

        private void NoModifyItem_Click(object sender, EventArgs e) {
            _controller.ToggleMode(ProxyMode.NoModify);
        }

        private void DirectItem_Click(object sender, EventArgs e) {
            _controller.ToggleMode(ProxyMode.Direct);
            DisconnectCurrent_Click(null, null);
        }

        private void PACModeItem_Click(object sender, EventArgs e)
        {
            _controller.ToggleMode(ProxyMode.Pac);
        }

        private void GlobalModeItem_Click(object sender, EventArgs e) {
            _controller.ToggleMode(ProxyMode.Global);
        }

        private void RuleBypassLanItem_Click(object sender, EventArgs e) {
            _controller.ToggleRuleMode((int)ProxyRuleMode.BypassLan);
        }

        private void RuleBypassLanAndChinaItem_Click(object sender, EventArgs e) {
            _controller.ToggleRuleMode((int)ProxyRuleMode.BypassLanAndChina);
        }

        private void RuleBypassLanAndNotChinaItem_Click(object sender, EventArgs e) {
            _controller.ToggleRuleMode((int)ProxyRuleMode.BypassLanAndNotChina);
        }

        private void RuleUserItem_Click(object sender, EventArgs e) {
            _controller.ToggleRuleMode((int)ProxyRuleMode.UserCustom);
        }

        private void RuleBypassDisableItem_Click(object sender, EventArgs e) {
            _controller.ToggleRuleMode((int)ProxyRuleMode.Disable);
        }

        private void SelectRandomItem_Click(object sender, EventArgs e)
        {
            SelectedEnableBalanceItem.Checked = !SelectedEnableBalanceItem.Checked;
            _controller.ToggleEnableBalance(SelectedEnableBalanceItem.Checked);
        }

        private void SelectSameHostForSameTargetItem_Click(object sender, EventArgs e) {
            sameHostForSameTargetItem.Checked = !sameHostForSameTargetItem.Checked;
            _controller.ToggleSameHostForSameTargetRandom(sameHostForSameTargetItem.Checked);
        }

        private void CopyPACURLItem_Click(object sender, EventArgs e) {
            try {
                Configuration config = _controller.GetCurrentConfiguration();
                string pacUrl;
                pacUrl = "http://127.0.0.1:" + config.localPort.ToString() + "/pac?" + "auth=" + config.localAuthPassword + "&t=" + Util.Utils.GetTimestamp(DateTime.Now);
                Clipboard.SetText(pacUrl);
            }
            catch {

            }
        }

        private void EditPACFileItem_Click(object sender, EventArgs e) {
            _controller.TouchPACFile();
        }

        private void UpdatePACFromGFWListItem_Click(object sender, EventArgs e)//更新PAC为 GFWList
        {
            _controller.UpdatePACTo(GFWListUpdater.Templates.ss_gfw);//UpdatePACFromGFWList();
        }

        private void UpdatePACFromLanIPListItem_Click(object sender, EventArgs e)//更新PAC为 绕过局域网IP
        {
            _controller.UpdatePACTo(GFWListUpdater.Templates.ss_lanip);//UpdatePACFromOnlinePac("https://raw.githubusercontent.com/shadowsocksrr/breakwa11.github.io/master/ssr/ss_lanip.pac");
        }

        private void UpdatePACFromCNWhiteListItem_Click(object sender, EventArgs e)//更新PAC为 绕过大陆常见域名列表
        {
            _controller.UpdatePACTo(GFWListUpdater.Templates.ss_white);//UpdatePACFromOnlinePac("https://raw.githubusercontent.com/shadowsocksrr/breakwa11.github.io/master/ssr/ss_white.pac");
        }

        private void UpdatePACFromCNOnlyListItem_Click(object sender, EventArgs e)//更新PAC为 绕过大陆IP
        {
            _controller.UpdatePACTo(GFWListUpdater.Templates.ss_white_r);//UpdatePACFromOnlinePac("https://raw.githubusercontent.com/shadowsocksrr/breakwa11.github.io/master/ssr/ss_white_r.pac");
        }

        private void UpdatePACFromCNIPListItem_Click(object sender, EventArgs e)//更新PAC为 代理大陆常见域名（回国）
        {
            _controller.UpdatePACTo(GFWListUpdater.Templates.ss_cnip);//UpdatePACFromOnlinePac("https://raw.githubusercontent.com/shadowsocksrr/breakwa11.github.io/master/ssr/ss_cnip.pac");
        }

        private void EditUserRuleFileForGFWListItem_Click(object sender, EventArgs e) {
            _controller.TouchUserRuleFile();
        }

        private void AServerItem_Click(object sender, EventArgs e) {
            MenuItem item = (MenuItem)sender;
            _controller.DisconnectAllConnections();
            _controller.SelectServerIndex((int)item.Tag);
        }

        private void CheckUpdate_Click(object sender, EventArgs e)
        {
            updateSubscribeManager.notify = true;
            updateChecker.CheckUpdate(_controller.GetCurrentConfiguration());
        }

        private void CheckNodeUpdate_Click(object sender, EventArgs e)
        {
            if (_controller.GetCurrentConfiguration().serverSubscribes.Count == 0)
            {
                ShowBalloonTip(I18N.GetString("Tips"), I18N.GetString("Please add a server subscription and try again."), ToolTipIcon.Info);
                return;
            }
            if (CheckEnvironments())
            {
                if (timerCheckUpdate.Enabled)
                    timerCheckUpdate.Stop();
                CheckNodeUpdate(sender, false, true);
                timerCheckUpdate.Interval = 1000.0 * timerCheckSubscriptionsUpdateInterval;
                if (_controller.GetCurrentConfiguration().nodeFeedAutoUpdate && !timerCheckUpdate.Enabled)
                    timerCheckUpdate.Start();
            }
        }

        private void CheckNodeUpdateUseProxy_Click(object sender, EventArgs e)
        {
            if (_controller.GetCurrentConfiguration().serverSubscribes.Count == 0)
            {
                ShowBalloonTip(I18N.GetString("Tips"), I18N.GetString("Please add a server subscription and try again."), ToolTipIcon.Info);
                return;
            }
            if (CheckEnvironments())
            {
                if (timerCheckUpdate.Enabled)
                    timerCheckUpdate.Stop();
                CheckNodeUpdate(sender, true, true);
                timerCheckUpdate.Interval = 1000.0 * timerCheckSubscriptionsUpdateInterval;
                if (_controller.GetCurrentConfiguration().nodeFeedAutoUpdate && !timerCheckUpdate.Enabled)
                    timerCheckUpdate.Start();
            }
        }

        private void controller_UpdateNodeFromSubscribeForm(object sender,EventArgs e)
        {
            Object[] objlist = (object[])sender;
            if (objlist == null)
            {
                if (eventList.Count > 0)
                {
                    eventList.Clear();
                    updateSubscribeManager.InterruptUpdateFreeNode();
                }
                if (timerCheckUpdate.Enabled)
                    timerCheckUpdate.Stop();
                timerCheckUpdate.Interval = 1000.0;
                timerCheckUpdate.Start();
                return;
            }
            if (!(bool)objlist[1])
                CheckNodeUpdate_Click(objlist[0], null);
            else
                CheckNodeUpdateUseProxy_Click(objlist[0], null);
        }

        private bool CheckEnvironments(int type = 0)
        {
            if (!Utils.IsConnectionAvailable())
            {
                ShowBalloonTip(I18N.GetString("Error"), I18N.GetString("There seems to be a problem with the network connection."), ToolTipIcon.Error);
                return false;
            }
            if (type == 1)
                return true;
            if (configfrom_open)
            {
                ShowBalloonTip(I18N.GetString("Tips"), I18N.GetString("Close the configuration form before updating."), ToolTipIcon.Info);
                return false;
            }
            return true;
        }

        private void CheckNodeUpdate(object obj, bool proxy, bool notify)
        {
            if (timerUpdateLatency.Enabled)
                timerUpdateLatency.Stop();
            Util.TCPingManager.StopTcping("Updating Subscriptions");

            int[] indexes;
            if (obj != null && typeof(int[]) == obj.GetType()) 
                indexes = (int[])obj;
            else
                indexes = new int[0];
            Thread th = new Thread(new ParameterizedThreadStart(thupdateSubscribeManager_CreateTask));
            if (!proxy)
            {
                if (updateSubscribeManager.InProcessing && updateSubscribeManager.use_proxy)
                {
                    if (eventList.Count > 0)
                        eventList.Clear();
                    updateSubscribeManager.InterruptUpdateFreeNode();
                }
                th.Start(new object[] { indexes, false, notify });
            }
            else
            {
                if (updateSubscribeManager.InProcessing && !updateSubscribeManager.use_proxy)
                {
                    if (eventList.Count > 0)
                        eventList.Clear();
                    updateSubscribeManager.InterruptUpdateFreeNode();
                }
                th.Start(new object[] { indexes, true, notify });
            }
        }

        private void thupdateSubscribeManager_CreateTask(object _obj)
        {
            object[] obj = (object[])_obj;
            if(!updateSubscribeManager.InProcessing)
                updateSubscribeManager.CreateTask(_controller.GetCurrentConfiguration(), (int[])obj[0], (bool)obj[1], (bool)obj[2]);
            else
                ShowBalloonTip(I18N.GetString("Tips"), I18N.GetString("Updating server subscription."), ToolTipIcon.Info);
        }

        private void ShowLogItem_Click(object sender, EventArgs e) {
            ShowGlobalLogForm();
        }

        private void ShowPortMapItem_Click(object sender, EventArgs e) {
            ShowPortMapForm();
        }

        private void ShowServerLogItem_Click(object sender, EventArgs e) {
            ShowServerLogForm();
        }

        private void SubscribeSetting_Click(object sender, EventArgs e) {
            ShowSubscribeSettingForm();
        }

        private void DisconnectCurrent_Click(object sender, EventArgs e)
        {
            _controller.DisconnectAllConnections();
        }

        private void DisconnectIfUnenableOrNonrandom(object sender, EventArgs e)
        {
            Configuration config = _controller.GetCurrentConfiguration();
            for (int id = 0; id < config.Servers.Count; ++id)
            {
                Server server = config.Servers[id];
                if(!config.enableBalance || !server.enable)
                    server.GetConnections().CloseAll();
            }
        }

        private void URL_Split(string text, ref List<string> out_urls) {
            if (String.IsNullOrEmpty(text)) {
                return;
            }
            int ss_index = text.IndexOf("ss://", 1, StringComparison.OrdinalIgnoreCase);
            int ssr_index = text.IndexOf("ssr://", 1, StringComparison.OrdinalIgnoreCase);
            int index = ss_index;
            if (index == -1 || index > ssr_index && ssr_index != -1) index = ssr_index;
            if (index == -1) {
                out_urls.Insert(0, text);
            }
            else {
                out_urls.Insert(0, text.Substring(0, index));
                URL_Split(text.Substring(index), ref out_urls);
            }
        }

        private bool ScanClipboardAddress(bool IsShowBalloonTip = true) {
            try
            {
                IDataObject iData = Clipboard.GetDataObject();
                if (iData.GetDataPresent(DataFormats.Text)) {
                    List<string> urls = new List<string>();
                    URL_Split((string)iData.GetData(DataFormats.Text), ref urls);
                    List<int> listindices = new List<int>();
                    List<Server> Allservers = _controller.GetCurrentConfiguration().Servers;
                    for (int i = 0; i < urls.Count; i++) 
                    {
                        Server server = new Server(urls[i], null);
                        if (!server.enable)
                            continue;
                        int index = -1;
                        if (i != urls.Count - 1)
                            index = _controller.AddServer(server, configfrom_open ? configForm.currentSelectedID : null, false);
                        else
                            index = _controller.AddServer(server, configfrom_open ? configForm.currentSelectedID : null);
                        if (index != -1 && listindices.FindIndex(t => t == index + i) == -1)  
                        {
                            listindices.Add(index + listindices.Count);
                        }
                    }
                    if (listindices.Count> 0)
                    {
                        listindices.Sort();
                        ShowConfigForm(listindices);
                        Clipboard.Clear();
                        return true;
                    }
                }
                if (IsShowBalloonTip)
                    ShowBalloonTip(I18N.GetString("Tips"), I18N.GetString("No new SS(R) links in clipboard."), ToolTipIcon.Error);
            }
            catch (Exception e)
            {
                Logging.LogUsefulException(e);
            }
            return false;
        }

        private void ScanClipboardAddressItem_Click(object sender, EventArgs e)
        {
            ScanClipboardAddress();
        }

        private bool ScanQRCode(Screen screen, Bitmap fullImage, Rectangle cropRect, out string url, out Rectangle rect) {
            using (Bitmap target = new Bitmap(cropRect.Width, cropRect.Height)) {
                using (Graphics g = Graphics.FromImage(target)) {
                    g.DrawImage(fullImage, new Rectangle(0, 0, cropRect.Width, cropRect.Height),
                                    cropRect,
                                    GraphicsUnit.Pixel);
                }
                var source = new BitmapLuminanceSource(target);
                var bitmap = new BinaryBitmap(new HybridBinarizer(source));
                QRCodeReader reader = new QRCodeReader();
                var result = reader.decode(bitmap);
                if (result != null) {
                    url = result.Text;
                    double minX = Int32.MaxValue, minY = Int32.MaxValue, maxX = 0, maxY = 0;
                    foreach (ResultPoint point in result.ResultPoints) {
                        minX = Math.Min(minX, point.X);
                        minY = Math.Min(minY, point.Y);
                        maxX = Math.Max(maxX, point.X);
                        maxY = Math.Max(maxY, point.Y);
                    }
                    rect = new Rectangle(cropRect.Left + (int)minX, cropRect.Top + (int)minY, (int)(maxX - minX), (int)(maxY - minY));
                    return true;
                }
            }
            url = "";
            rect = new Rectangle();
            return false;
        }

        private bool ScanQRCodeStretch(Screen screen, Bitmap fullImage, Rectangle cropRect, double mul, out string url, out Rectangle rect) {
            using (Bitmap target = new Bitmap((int)(cropRect.Width * mul), (int)(cropRect.Height * mul))) {
                using (Graphics g = Graphics.FromImage(target)) {
                    g.DrawImage(fullImage, new Rectangle(0, 0, target.Width, target.Height),
                                    cropRect,
                                    GraphicsUnit.Pixel);
                }
                var source = new BitmapLuminanceSource(target);
                var bitmap = new BinaryBitmap(new HybridBinarizer(source));
                QRCodeReader reader = new QRCodeReader();
                var result = reader.decode(bitmap);
                if (result != null) {
                    url = result.Text;
                    double minX = Int32.MaxValue, minY = Int32.MaxValue, maxX = 0, maxY = 0;
                    foreach (ResultPoint point in result.ResultPoints) {
                        minX = Math.Min(minX, point.X);
                        minY = Math.Min(minY, point.Y);
                        maxX = Math.Max(maxX, point.X);
                        maxY = Math.Max(maxY, point.Y);
                    }
                    rect = new Rectangle(cropRect.Left + (int)(minX / mul), cropRect.Top + (int)(minY / mul), (int)((maxX - minX) / mul), (int)((maxY - minY) / mul));
                    return true;
                }
            }
            url = "";
            rect = new Rectangle();
            return false;
        }

        private Rectangle GetScanRect(int width, int height, int index, out double stretch) {
            stretch = 1;
            if (index < 5) {
                const int div = 5;
                int w = width * 3 / div;
                int h = height * 3 / div;
                Point[] pt = new Point[5] {
                    new Point(1, 1),

                    new Point(0, 0),
                    new Point(0, 2),
                    new Point(2, 0),
                    new Point(2, 2),
                };
                return new Rectangle(pt[index].X * width / div, pt[index].Y * height / div, w, h);
            }
            {
                const int base_index = 5;
                if (index < base_index + 6) {
                    double[] s = new double[] {
                        1,
                        2,
                        3,
                        4,
                        6,
                        8
                    };
                    stretch = 1 / s[index - base_index];
                    return new Rectangle(0, 0, width, height);
                }
            }
            {
                const int base_index = 11;
                if (index < base_index + 8) {
                    const int hdiv = 7;
                    const int vdiv = 5;
                    int w = width * 3 / hdiv;
                    int h = height * 3 / vdiv;
                    Point[] pt = new Point[8] {
                        new Point(1, 1),
                        new Point(3, 1),

                        new Point(0, 0),
                        new Point(0, 2),

                        new Point(2, 0),
                        new Point(2, 2),

                        new Point(4, 0),
                        new Point(4, 2),
                    };
                    return new Rectangle(pt[index - base_index].X * width / hdiv, pt[index - base_index].Y * height / vdiv, w, h);
                }
            }
            return new Rectangle(0, 0, 0, 0);
        }

        private bool ScanScreenQRCode(bool ss_only, bool IsShowBalloonTip = true) {
            foreach (Screen screen in Screen.AllScreens)
            {
                Point screen_size = Util.Utils.GetScreenPhysicalSize();
                using (Bitmap fullImage = new Bitmap(screen_size.X,
                                                screen_size.Y)) {
                    using (Graphics g = Graphics.FromImage(fullImage)) {
                        g.CopyFromScreen(screen.Bounds.X,
                                         screen.Bounds.Y,
                                         0, 0,
                                         fullImage.Size,
                                         CopyPixelOperation.SourceCopy);
                    }
                    bool decode_fail = false;
                    for (int i = 0; i < 100; i++)
                    {
                        double stretch;
                        Rectangle cropRect = GetScanRect(fullImage.Width, fullImage.Height, i, out stretch);
                        if (cropRect.Width == 0)
                            break;

                        string url;
                        Rectangle rect;
                        if (stretch == 1 ? ScanQRCode(screen, fullImage, cropRect, out url, out rect) : ScanQRCodeStretch(screen, fullImage, cropRect, stretch, out url, out rect))
                        {
                            QRCodeSplashForm splash = new QRCodeSplashForm();
                            Server server = new Server(url, null);
                            if (!server.enable)
                            {
                                decode_fail = true;
                                break;
                            }
                                int newNodeIndex = _controller.AddServer(server, configfrom_open ? configForm.currentSelectedID : null);
                                if (newNodeIndex != -1)
                                {
                                    splash.FormClosed += (s, e) =>
                                    {
                                        ShowConfigForm(new List<int>() { newNodeIndex });
                                    };
                                }
                                else if (!ss_only)
                                {
                                    _urlToOpen = url;
                                    splash.FormClosed += showURLFromQRCode;
                                }
                                else
                                {
                                    decode_fail = true;
                                    continue;
                                }
                            splash.Location = new Point(screen.Bounds.X, screen.Bounds.Y);
                            double dpi = Screen.PrimaryScreen.Bounds.Width / (double)screen_size.X;
                            splash.TargetRect = new Rectangle(
                                (int)(rect.Left * dpi + screen.Bounds.X),
                                (int)(rect.Top * dpi + screen.Bounds.Y),
                                (int)(rect.Width * dpi),
                                (int)(rect.Height * dpi));
                            splash.Size = new Size(fullImage.Width, fullImage.Height);
                            splash.Show();
                            return true;
                        }
                    }
                    if (decode_fail)
                    {
                        if (IsShowBalloonTip)
                            ShowBalloonTip(I18N.GetString("Tips"), I18N.GetString("Failed to decode QRCode"), ToolTipIcon.Error);
                        return false;
                    }
                }
            }
            if (IsShowBalloonTip) 
                ShowBalloonTip(I18N.GetString("Tips"), I18N.GetString("No QRCode found. Try to zoom in or move it to the center of the screen."), ToolTipIcon.Error);
            return false;
        }

        private void ScanQRCodeItem_Click(object sender, EventArgs e) {
            ScanScreenQRCode(false);
        }

        void openURLFromQRCode(object sender, FormClosedEventArgs e) {
            Process.Start(_urlToOpen);
        }
        
        private void ShowHotKeySettingsForm()
        {
            if (!HotKeys._Enabled)
                HotKeys.Init();
            HotKeys.StophotKeyManager();
            if (hotkeySettingsForm == null)
            {
                hotkeySettingsForm = new HotkeySettingsForm(_controller);
                hotkeySettingsForm.FormClosed += hotkeySettingsForm_FormClosed;
                hotkeySettingsForm.Show();
            }
            HotkeySettingsForm._IsModified = false;
            hotkeySettingsForm.Activate();
            hotkeySettingsForm.BringToFront();
        }

        void hotkeySettingsForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (HotKeys._Enabled)
                HotKeys.Init();
            hotkeySettingsForm.Dispose();
            hotkeySettingsForm = null;

            Utils.ReleaseMemory(true);
        }

        private void UpdateLatency_Click(object sender, EventArgs e)
        {
            if (!CheckEnvironments(1)) 
                return;
            if (timerUpdateLatency.Enabled)
                timerUpdateLatency.Stop();
            timerUpdateLatency.Interval = 1000.0;
            timerUpdateLatency.Start();
        }

        private void hotKeyItem_Click(object sender, EventArgs e)
        {
            ShowHotKeySettingsForm();
        }

        void showURLFromQRCode() {
            ShowTextForm dlg = new ShowTextForm("QRCode", _urlToOpen);
            dlg.Show();
            dlg.Activate();
            dlg.BringToFront();
        }

        void showURLFromQRCode(object sender, FormClosedEventArgs e) {
            showURLFromQRCode();
        }

        void showURLFromQRCode(object sender, System.EventArgs e) {
            showURLFromQRCode();
        }

        public void ShowLogForm_HotKey()
        {
            ShowGlobalLogForm();
        }

        public void CallClipboardAndQRCodeScanning_HotKey()
        {
            if(!ScanClipboardAddress(false) && !ScanScreenQRCode(false, false))
                ShowBalloonTip(I18N.GetString("Tips"), I18N.GetString("No new SS(R) links in clipboard or new QRCode on screen."), ToolTipIcon.Info);
        }
        
        public void DirectItem_Click()
        {
            DirectItem_Click(null, null);
        }

        public void PACModeItem_Click()
        {
            PACModeItem_Click(null, null);
        }

        public void GlobalModeItem_Click()
        {
            GlobalModeItem_Click(null, null);
        }

    }
}
