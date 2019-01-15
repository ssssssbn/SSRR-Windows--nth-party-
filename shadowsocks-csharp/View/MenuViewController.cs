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

        private ShadowsocksController _controller;
        private UpdateChecker updateChecker;
        private UpdateFreeNode updateFreeNodeChecker;
        private UpdateSubscribeManager updateSubscribeManager;

        //private bool isBeingUsed = false;

        private NotifyIcon _notifyIcon;
        private ContextMenu contextMenu1;

        private MenuItem noModifyItem;
        private MenuItem enableItem;
        private MenuItem PACModeItem;
        private MenuItem globalModeItem;
        private MenuItem modeItem;

        private MenuItem ruleBypassLan;
        private MenuItem ruleBypassChina;
        private MenuItem ruleBypassNotChina;
        private MenuItem ruleUser;
        private MenuItem ruleDisableBypass;

        private MenuItem SeperatorItem;
        private MenuItem ServersItem;
        private MenuItem SelectRandomItem;
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
        //private System.Timers.Timer timerDetectVirus;
        private System.Timers.Timer timerDelayCheckUpdate;
        private System.Timers.Timer timerUpdateLatency;
        public bool UpdateLatencyInterrupt = false;

        private bool configfrom_open = false;
        private bool subScribeForm_open = false;
        private List<EventParams> eventList = new List<EventParams>();
        
        public static AppBarForm appbarform;
        public bool IsCopyLinksToClipboard = false;
        public bool UpdateLatencyInProgress = false;
        public bool UpdateFreeNodeInterrupt = false;
        //public static bool appbarformAtStart = false;

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

            _notifyIcon = new NotifyIcon();
            UpdateTrayIcon();
            _notifyIcon.Visible = true;
            _notifyIcon.ContextMenu = contextMenu1;
            _notifyIcon.MouseClick += notifyIcon1_Click;
            //_notifyIcon.MouseDoubleClick += notifyIcon1_DoubleClick;

            updateChecker = new UpdateChecker();
            updateChecker.NewVersionFound += updateChecker_NewVersionFound;

            updateFreeNodeChecker = new UpdateFreeNode();
            updateFreeNodeChecker.NewFreeNodeFound += updateFreeNodeChecker_NewFreeNodeFound;

            updateSubscribeManager = new UpdateSubscribeManager();

            LoadCurrentConfiguration();

            //this interval will change
            timerDelayCheckUpdate = new System.Timers.Timer(1000.0 * 10);
            timerDelayCheckUpdate.AutoReset = false;
            timerDelayCheckUpdate.Elapsed += timerDelayCheckUpdate_Elapsed;
            timerDelayCheckUpdate.Start();


            timerUpdateLatency = new System.Timers.Timer(1000.0 * 3);
            timerUpdateLatency.AutoReset = false;
            timerUpdateLatency.Elapsed += timerUpdateLatency_Elapsed;
            if(!_controller.GetCurrentConfiguration().nodeFeedAutoUpdate && _controller.GetCurrentConfiguration().nodeFeedAutoLatency)
            {
                timerUpdateLatency.Start();
            }
        }

        public void startCheckFreeNode()
        {
                timerDelayCheckUpdate.Interval = 1000.0;
                timerDelayCheckUpdate.Start();
        }

        public void startUpdateLatency()
        {
            timerUpdateLatency.Interval = 1000.0;
            timerUpdateLatency.Start();
        }

        public void ShownotifyIcontext()
        {
            ShowBalloonTip(I18N.GetString("Current status"), _notifyIcon.Text, ToolTipIcon.Info, 1);
        }
        
        public void ShowTextByNotifyIconBalloon(string title, string content, ToolTipIcon icon, int timeout)
        {
            ShowBalloonTip(I18N.GetString(title), I18N.GetString(content), icon, timeout);
        }

        private void timerDelayCheckUpdate_Elapsed(object sender, System.Timers.ElapsedEventArgs e) {
            if (timerDelayCheckUpdate != null) {
                if (timerDelayCheckUpdate.Interval <= 1000.0 * 30) {
                    timerDelayCheckUpdate.Interval = 1000.0 * 60 * 5;
                }
                else {
                    timerDelayCheckUpdate.Interval = 1000.0 * 60 * 60 * 2;
                }
            }
            //updateChecker.CheckUpdate(_controller.GetCurrentConfiguration());

            if (subScribeForm_open)
            {
                UpdateFreeNodeInterrupt = true;
                return;
            }

            Configuration cfg = _controller.GetCurrentConfiguration();
            if (cfg.nodeFeedAutoUpdate) //cfg.isDefaultConfig() ||
            {
                    updateSubscribeManager.CreateTask(_controller.GetCurrentConfiguration(), updateFreeNodeChecker, -1, cfg.nodeFeedAutoUpdateUseProxy && !cfg.isDefaultConfig(), false);//!cfg.isDefaultConfig()
            }
            timerDelayCheckUpdate.Start();
        }

        private void timerUpdateLatency_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            if (subScribeForm_open)
            {
                UpdateLatencyInterrupt = true;
                return;
            }
            if (UpdateLatencyInProgress)
            {
                ShowBalloonTip(I18N.GetString("Tips"), I18N.GetString("Latency test in progress."), ToolTipIcon.Info, 1);
                return;
            }
            else if (UpdateLatencyInterrupt)
            {
                ShowBalloonTip(I18N.GetString("Tips"), I18N.GetString("Updating server subscriptions, update latency process after completion."), ToolTipIcon.Info, 1);
                return;
            }
            UpdateLatencyInProgress = true;

            Configuration configuration = _controller.GetCurrentConfiguration();
            try
            {
                for (int i = 0; i < configuration.configs.Count; i++)
                {
                    if (updateSubscribeManager.IsInProgress())
                    {
                        UpdateLatencyInProgress = false;
                        if (i > 0)
                            Utils.ReleaseMemory(true);
                        return;
                    }
                    if(UpdateLatencyInterrupt || subScribeForm_open || configfrom_open)
                    {
                        UpdateLatencyInterrupt = true;
                        UpdateLatencyInProgress = false;
                        if (i > 0)
                            Utils.ReleaseMemory(true);
                        return;
                    }
                    //if (UpdateLatencyInterrupt)
                    //{
                    //    if (i > 0)
                    //        Utils.ReleaseMemory(true);
                    //    UpdateLatencyInProgress = false;
                    //    return;
                    //}
                    if (!configuration.configs[i].enable)
                    {
                        configuration.configs[i].latency = -1;
                        continue;
                    }
                    configuration.configs[i].tcpingLatency();
                }
                Utils.ReleaseMemory(true);
            }
            catch
            {
                timerUpdateLatency.Interval = 1000.0 * 10;
            }
            //_controller.SaveTimerUpdateLatency();
            UpdateServersMenu();
            if (configuration.nodeFeedAutoLatency)
            {
                timerUpdateLatency.Interval = 1000.0 * 60 * 30;
                timerUpdateLatency.Start();
            }
            UpdateLatencyInProgress = false;
        }

        void controller_Errored(object sender, System.IO.ErrorEventArgs e) {
            MessageBox.Show(e.GetException().ToString(), String.Format(I18N.GetString("Shadowsocks Error: {0}"), e.GetException().Message));
        }

        private void UpdateTrayIcon() {
            int dpi;
            using (Graphics graphics = Graphics.FromHwnd(IntPtr.Zero)) {
                dpi = (int)graphics.DpiX;
            }
            Configuration config = _controller.GetCurrentConfiguration();
            bool enabled = config.sysProxyMode != (int)ProxyMode.NoModify && config.sysProxyMode != (int)ProxyMode.Direct;
            string balancemode = "";
            if (config.random)
            {
                balancemode = I18N.GetString(config.balanceAlgorithm);
            }
            string server;
            int server_current = config.index;
            //else {
            //    int server_current = config.index;
            //    if (config.configs[server_current].remarks == "")
            //        server = I18N.GetString("No Remarks");
            //    else
            //        server = config.configs[server_current].remarks;
            //}
            if (config.configs[server_current].remarks == "")
                server = I18N.GetString("No Remarks");
            else
                server = config.configs[server_current].remarks;
            if (server.Length > 30)
                server = server.Substring(0, 30) + "...";
            bool global = config.sysProxyMode == (int)ProxyMode.Global;
            bool random = config.random;

            try {
                using (Bitmap icon = new Bitmap("icon.png")) {
                    _notifyIcon.Icon = Icon.FromHandle(icon.GetHicon());
                }
            }
            catch {
                Bitmap icon = null;
                if (dpi < 97) {
                    // dpi = 96;
                    icon = Resources.ss16;
                }
                else if (dpi < 121) {
                    // dpi = 120;
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
                if (!random) {
                    mul_r = 0.4;
                }

                using (Bitmap iconCopy = new Bitmap(icon)) {
                    for (int x = 0; x < iconCopy.Width; x++) {
                        for (int y = 0; y < iconCopy.Height; y++) {
                            Color color = icon.GetPixel(x, y);
                            iconCopy.SetPixel(x, y,
                                Color.FromArgb((byte)(color.A * mul_a),
                                ((byte)(color.R * mul_r)),
                                ((byte)(color.G * mul_g)),
                                ((byte)(color.B * mul_b))));
                        }
                    }
                    _notifyIcon.Icon = Icon.FromHandle(iconCopy.GetHicon());
                }
            }

            // we want to show more details but notify icon title is limited to 63 characters
            string text = (enabled ?
                    (global ? I18N.GetString("Global") : I18N.GetString("PAC")) :
                    I18N.GetString("Disable system proxy"))
                    ;
            if (random)
                text +=
                    "\r\n"
                    + balancemode;
            text +=
                 "\r\n"
                + server
                + "\r\n"
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
                    ruleBypassChina = CreateMenuItem("Bypass LAN && China", new EventHandler(RuleBypassChinaItem_Click)),
                    ruleBypassNotChina = CreateMenuItem("Bypass LAN && not China", new EventHandler(RuleBypassNotChinaItem_Click)),
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
                SelectRandomItem = CreateMenuItem("Load balance", new EventHandler(SelectRandomItem_Click)),
                CreateMenuItem("Global settings...", new EventHandler(Setting_Click)),
                CreateMenuItem("Port settings...", new EventHandler(ShowPortMapItem_Click)),
                new MenuItem("-"),
                CreateMenuItem("Update latency", new EventHandler(UpdateLatency_Click)),
                hotKeyItem = CreateMenuItem("Edit Hotkeys...", new EventHandler(hotKeyItem_Click)),
                CreateMenuItem("Reset password...", new EventHandler(ResetPasswordItem_Click)),
                CreateMenuItem("Gen custom QRCode...", new EventHandler(showURLFromQRCode)),
                CreateMenuItem("Show logs...", new EventHandler(ShowLogItem_Click)),
                UpdateItem = CreateMenuItem("Update available", new EventHandler(UpdateItem_Clicked)),
                //CreateMenuGroup("Help", new MenuItem[] {
                //    CreateMenuItem("Check update", new EventHandler(CheckUpdate_Click)),
                //    CreateMenuItem("Show logs...", new EventHandler(ShowLogItem_Click)),
                //    CreateMenuItem("Open wiki...", new EventHandler(OpenWiki_Click)),
                //    CreateMenuItem("Feedback...", new EventHandler(FeedbackItem_Click)),
                //    new MenuItem("-"),
                //    CreateMenuItem("Gen custom QRCode...", new EventHandler(showURLFromQRCode)),
                //    new MenuItem("-"),
                //    CreateMenuItem("About...", new EventHandler(AboutItem_Click)),
                //    CreateMenuItem("Donate...", new EventHandler(DonateItem_Click)),
                //}),
                new MenuItem("-"),
                CreateMenuItem("Quit", new EventHandler(Quit_Click))
            });
            UpdateItem.Visible = false;

            //Configuration config = _controller.GetCurrentConfiguration();
            //if (config.hotkey.RegHotkeysAtStartup == true && config.hotkey.SwitchSystemProxy != "")
            //{
            //    appbarform = new AppBarForm();
            //    appbarformAtStart = true;
            //}

        }

        private void controller_ConfigChanged(object sender, EventArgs e) {
            LoadCurrentConfiguration();
            UpdateTrayIcon();
        }

        private void controller_ToggleModeChanged(object sender, EventArgs e) {
            Configuration config = _controller.GetCurrentConfiguration();
            UpdateSysProxyMode(config);
            if (config.sysProxyMode == (int)ProxyMode.Direct)
                DisconnectCurrent_Click(null, null);
        }

        private void controller_ToggleRuleModeChanged(object sender, EventArgs e) {
            Configuration config = _controller.GetCurrentConfiguration();
            UpdateProxyRule(config);
        }

        void controller_FileReadyToOpen(object sender, ShadowsocksController.PathEventArgs e) {
            string argument = @"/select, " + e.Path;

            System.Diagnostics.Process.Start("explorer.exe", argument);
        }

        void ShowBalloonTip(string title, string content, ToolTipIcon icon, int timeout) {
            _notifyIcon.BalloonTipTitle = title;
            _notifyIcon.BalloonTipText = content;
            _notifyIcon.BalloonTipIcon = icon;
            _notifyIcon.ShowBalloonTip(timeout);
        }

        void controller_UpdatePACFromGFWListError(object sender, System.IO.ErrorEventArgs e) {
            GFWListUpdater updater = (GFWListUpdater)sender;
            ShowBalloonTip(I18N.GetString("Failed to update PAC file"), e.GetException().Message, ToolTipIcon.Error, 5000);
            Logging.LogUsefulException(e.GetException());
        }

        void controller_UpdatePACFromGFWListCompleted(object sender, GFWListUpdater.ResultEventArgs e) {
            GFWListUpdater updater = (GFWListUpdater)sender;
            string result = e.Success ?
                (updater.update_type <= 1 ? I18N.GetString("PAC updated") : I18N.GetString("Domain white list list updated"))
                : I18N.GetString("No updates found. Please report to GFWList if you have problems with it.");
            ShowBalloonTip(I18N.GetString("Shadowsocks"), result, ToolTipIcon.Info, 1000);
        }

        void updateFreeNodeChecker_NewFreeNodeFound(object sender, EventArgs e) {
            if (configfrom_open)
            {
                eventList.Clear();
                eventList.Add(new EventParams(sender, e));
                return;
            }
            if (subScribeForm_open)
            {
                timerDelayCheckUpdate.Stop();
                updateSubscribeManager.ClearTask();
                UpdateFreeNodeInterrupt = true;
                return;
            }
            if (UpdateFreeNodeInterrupt)
            {
                updateSubscribeManager.ClearTask();
                updateSubscribeManager.CreateTask(_controller.GetCurrentConfiguration(), updateFreeNodeChecker, -1, !updateSubscribeManager._use_proxy, true);
                UpdateFreeNodeInterrupt = false;
                return;
            }
            string lastGroup = null;
            int count = 0;
            if (!String.IsNullOrEmpty(updateFreeNodeChecker.FreeNodeResult)) {
                List<string> urls = new List<string>();
                updateFreeNodeChecker.FreeNodeResult = updateFreeNodeChecker.FreeNodeResult.TrimEnd('\r', '\n', ' ');
                Configuration config = _controller.GetCurrentConfiguration();
                Server selected_server = null;
                if (config.index >= 0 && config.index < config.configs.Count) {
                    selected_server = config.configs[config.index];
                }
                try {
                    updateFreeNodeChecker.FreeNodeResult = Util.Base64.DecodeBase64(updateFreeNodeChecker.FreeNodeResult);
                }
                catch {
                    updateFreeNodeChecker.FreeNodeResult = "";
                }
                int max_node_num = 0;

                Match match_maxnum = Regex.Match(updateFreeNodeChecker.FreeNodeResult, "^MAX=([0-9]+)");
                if (match_maxnum.Success) {
                    try {
                        max_node_num = Convert.ToInt32(match_maxnum.Groups[1].Value, 10);
                    }
                    catch {

                    }
                }
                URL_Split(updateFreeNodeChecker.FreeNodeResult, ref urls);
                for (int i = urls.Count - 1; i >= 0; --i) {
                    if (!urls[i].StartsWith("ssr"))
                        urls.RemoveAt(i);
                }
                if (urls.Count > 0) {
                    bool keep_selected_server = false; // set 'false' if import all nodes
                    if (max_node_num <= 0 || max_node_num >= urls.Count) {
                        urls.Reverse();
                    }
                    else {
                        Random r = new Random();
                        Util.Utils.Shuffle(urls, r);
                        urls.RemoveRange(max_node_num, urls.Count - max_node_num);
                        if (!config.isDefaultConfig())
                            keep_selected_server = true;
                    }
                    string curGroup = null;
                    foreach (string url in urls) {
                        try // try get group name
                        {
                            Server server = new Server(url, null);
                            if (!String.IsNullOrEmpty(server.group)) {
                                curGroup = server.group;
                                break;
                            }
                        }
                        catch { }
                    }
                    string subscribeURL = updateSubscribeManager.URL;
                    if (String.IsNullOrEmpty(curGroup)) {
                        curGroup = subscribeURL;
                    }
                    for (int i = 0; i < config.serverSubscribes.Count; ++i) {
                        if (subscribeURL == config.serverSubscribes[i].URL) {
                            lastGroup = config.serverSubscribes[i].Group;
                            config.serverSubscribes[i].Group = curGroup;
                            break;
                        }
                    }
                    if (String.IsNullOrEmpty(lastGroup)) {
                        lastGroup = curGroup;
                    }

                    if (keep_selected_server && selected_server.group == curGroup) {
                        bool match = false;
                        for (int i = 0; i < urls.Count; ++i) {
                            try {
                                Server server = new Server(urls[i], null);
                                if (selected_server.isMatchServer(server)) {
                                    match = true;
                                    break;
                                }
                            }
                            catch { }
                        }
                        if (!match) {
                            urls.RemoveAt(0);
                            urls.Add(selected_server.GetSSRLinkForServer());
                        }
                    }

                    // import all, find difference
                    {
                        Dictionary<string, Server> old_servers = new Dictionary<string, Server>();
                        Dictionary<string, Server> old_insert_servers = new Dictionary<string, Server>();
                        if (!String.IsNullOrEmpty(lastGroup)) {
                            for (int i = config.configs.Count - 1; i >= 0; --i) {
                                if (lastGroup == config.configs[i].group) {
                                    old_servers[config.configs[i].id] = config.configs[i];
                                }
                            }
                        }
                        foreach (string url in urls) {
                            try {
                                Server server = new Server(url, curGroup);
                                bool match = false;
                                if (!match) {
                                    foreach (KeyValuePair<string, Server> pair in old_insert_servers) {
                                        if (server.isMatchServer(pair.Value)) {
                                            match = true;
                                            break;
                                        }
                                    }
                                }
                                old_insert_servers[server.id] = server;
                                if (!match) {
                                    foreach (KeyValuePair<string, Server> pair in old_servers) {
                                        if (server.isMatchServer(pair.Value)) {
                                            match = true;
                                            old_servers.Remove(pair.Key);
                                            pair.Value.CopyServerInfo(server);
                                            ++count;
                                            break;
                                        }
                                    }
                                }
                                if (!match) {
                                    int insert_index = config.configs.Count;
                                    for (int index = config.configs.Count - 1; index >= 0; --index) {
                                        if (config.configs[index].group == curGroup) {
                                            insert_index = index + 1;
                                            break;
                                        }
                                    }
                                    config.configs.Insert(insert_index, server);
                                    ++count;
                                }
                            }
                            catch { }
                        }
                        foreach (KeyValuePair<string, Server> pair in old_servers) {
                            for (int i = config.configs.Count - 1; i >= 0; --i) {
                                if (config.configs[i].id == pair.Key) {
                                    config.configs.RemoveAt(i);
                                    break;
                                }
                            }
                        }
                        _controller.SaveServersConfig(config, false);
                    }
                    config = _controller.GetCurrentConfiguration();
                    if (selected_server != null) {
                        bool match = false;
                        for (int i = config.configs.Count - 1; i >= 0; --i) {
                            if (config.configs[i].id == selected_server.id) {
                                config.index = i;
                                match = true;
                                break;
                            }
                            else if (config.configs[i].group == selected_server.group) {
                                if (config.configs[i].isMatchServer(selected_server)) {
                                    config.index = i;
                                    match = true;
                                    break;
                                }
                            }
                        }
                        if (!match) {
                            config.index = config.configs.Count - 1;
                        }
                    }
                    else {
                        config.index = config.configs.Count - 1;
                    }
                    if (count > 0) {
                        for (int i = 0; i < config.serverSubscribes.Count; ++i) {
                            if (config.serverSubscribes[i].URL == updateFreeNodeChecker.subscribeTask.URL) {
                                config.serverSubscribes[i].LastUpdateTime = (UInt64)Math.Floor(DateTime.Now.Subtract(new DateTime(1970, 1, 1, 0, 0, 0)).TotalSeconds);
                            }
                        }
                    }
                    _controller.SaveServersConfig(config, false);
                }
            }
            
            if (count > 0)
            {
                updateFreeNodeChecker.countnum += 1;
                if (updateFreeNodeChecker.noitify)
                    ShowBalloonTip(I18N.GetString("Success"),
                        string.Format(I18N.GetString("Update subscribe {0} success"), lastGroup), ToolTipIcon.Info, 10000);
            }
            else
            {
                if (!updateFreeNodeChecker.trieduseproxy)
                {
                    updateFreeNodeChecker.countnum += 1;
                    updateFreeNodeChecker.countfailurenum += 1;
                    updateFreeNodeChecker.recordfailure();
                    if (lastGroup == null)
                    {
                        lastGroup = updateFreeNodeChecker.subscribeTask.Group;
                        //lastGroup = updateSubscribeManager.LastGroup;
                    }
                    ShowBalloonTip(I18N.GetString("Error"), String.Format(I18N.GetString("Update subscribe {0} failure"), lastGroup), ToolTipIcon.Info, 1);
                }
            }

            if (updateSubscribeManager.Next())
            {
            }
            else
            {
                _controller.JustReload();
                UpdateLatencyInterrupt = false;
                if (_controller.GetCurrentConfiguration().nodeFeedAutoLatency)
                {
                    timerUpdateLatency.Interval = 1000.0;
                    timerUpdateLatency.Start();
                }
                if (updateFreeNodeChecker.countfailure != "")
                {
                    if (updateFreeNodeChecker.countnum == updateFreeNodeChecker.countfailurenum)
                        updateFreeNodeChecker.countfailure = I18N.GetString("All");
                    ShowBalloonTip(I18N.GetString("Error"), String.Format(I18N.GetString("Update subscribe {0} failure"), updateFreeNodeChecker.countfailure), ToolTipIcon.Info, 1);
                }
                else
                {
                    if (updateFreeNodeChecker.noitify)
                        ShowBalloonTip(I18N.GetString("Success"), I18N.GetString("Update all subscribe ssuccess"), ToolTipIcon.Info, 1);
                }
                updateSubscribeManager.ClearTask();
            }

        }

        void updateChecker_NewVersionFound(object sender, EventArgs e) {
            if (string.IsNullOrEmpty(updateChecker.LatestVersionNumber)) {
                Logging.Log(LogLevel.Error, "connect to update server error");
            }
            else {
                if (!UpdateItem.Visible) {
                    ShowBalloonTip(String.Format(I18N.GetString("{0} {1} Update Found"), UpdateChecker.Name, updateChecker.LatestVersionNumber),
                        I18N.GetString("Click menu to download"), ToolTipIcon.Info, 10000);
                    _notifyIcon.BalloonTipClicked += notifyIcon1_BalloonTipClicked;

                    timerDelayCheckUpdate.Elapsed -= timerDelayCheckUpdate_Elapsed;
                    timerDelayCheckUpdate.Stop();
                    timerDelayCheckUpdate = null;
                }
                UpdateItem.Visible = true;
                UpdateItem.Text = String.Format(I18N.GetString("New version {0} {1} available"), UpdateChecker.Name, updateChecker.LatestVersionNumber);
            }
        }

        void UpdateItem_Clicked(object sender, EventArgs e) {
            Process.Start(updateChecker.LatestVersionURL);
        }

        void notifyIcon1_BalloonTipClicked(object sender, EventArgs e) {
            Process.Start(updateChecker.LatestVersionURL);
            _notifyIcon.BalloonTipClicked -= notifyIcon1_BalloonTipClicked;
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
            ruleBypassChina.Checked = config.proxyRuleMode == (int)ProxyRuleMode.BypassLanAndChina;
            ruleBypassNotChina.Checked = config.proxyRuleMode == (int)ProxyRuleMode.BypassLanAndNotChina;
            ruleUser.Checked = config.proxyRuleMode == (int)ProxyRuleMode.UserCustom;
        }

        private void LoadCurrentConfiguration() {
            Configuration config = _controller.GetCurrentConfiguration();
            UpdateServersMenu();
            UpdateSysProxyMode(config);

            UpdateProxyRule(config);

            SelectRandomItem.Checked = config.random;
            sameHostForSameTargetItem.Checked = config.sameHostForSameTarget;
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
            for (int i = 0; i < configuration.configs.Count; i++) {
                string group_name;
                Server server = configuration.configs[i];
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

        private void ShowConfigForm(bool addNode) {
            if (configForm != null) {
                configForm.Activate();
                if (addNode) {
                    Configuration cfg = _controller.GetCurrentConfiguration();
                    configForm.SetServerListSelectedIndex(cfg.index + 1);
                }
            }
            else {
                configfrom_open = true;
                configForm = new ConfigForm(_controller, updateChecker, addNode ? -1 : -2);
                configForm.Show();
                configForm.Activate();
                configForm.BringToFront();
                configForm.FormClosed += configForm_FormClosed;
            }
        }

        private void ShowConfigForm(int index) {
            if (configForm != null) {
                configForm.Activate();
            }
            else {
                configfrom_open = true;
                configForm = new ConfigForm(_controller, updateChecker, index);
                configForm.Show();
                configForm.Activate();
                configForm.BringToFront();
                configForm.FormClosed += configForm_FormClosed;
            }
        }

        private void ShowSettingForm() {
            if (settingsForm != null) {
                settingsForm.Activate();
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
            subScribeForm_open = true;
            if (subScribeForm != null) {
                subScribeForm.Activate();
                subScribeForm.Update();
                if (subScribeForm.WindowState == FormWindowState.Minimized) {
                    subScribeForm.WindowState = FormWindowState.Normal;
                }
            }
            else {
                subScribeForm = new SubscribeForm(_controller);
                subScribeForm.Show();
                subScribeForm.Activate();
                subScribeForm.BringToFront();
                subScribeForm.FormClosed += subScribeForm_FormClosed;
            }
        }

        void configForm_FormClosed(object sender, FormClosedEventArgs e) {
            if (IsCopyLinksToClipboard)
            {
                IsCopyLinksToClipboard = false;
                Clipboard.Clear();
            }
            configForm = null;
            configfrom_open = false;
            Utils.ReleaseMemory(true);
            if (eventList.Count > 0) {
                foreach (EventParams p in eventList) {
                    updateFreeNodeChecker_NewFreeNodeFound(p._sender, p._e);
                }
                eventList.Clear();
            }
        }

        void settingsForm_FormClosed(object sender, FormClosedEventArgs e) {
            settingsForm = null;
            Utils.ReleaseMemory(true);
        }

        void serverLogForm_FormClosed(object sender, FormClosedEventArgs e) {
            serverLogForm = null;
            Util.Utils.ReleaseMemory(true);
        }

        void portMapForm_FormClosed(object sender, FormClosedEventArgs e) {
            portMapForm = null;
            Util.Utils.ReleaseMemory(true);
        }

        void globalLogForm_FormClosed(object sender, FormClosedEventArgs e) {
            logForm = null;
            Util.Utils.ReleaseMemory(true);
        }

        void subScribeForm_FormClosed(object sender, FormClosedEventArgs e) {
            subScribeForm = null;
            subScribeForm_open = false;
            //timerUpdateLatency = new System.Timers.Timer(1000.0 * 3);
            //timerUpdateLatency.Elapsed += timerUpdateLatency_Elapsed;
            //timerUpdateLatency.Start();
        }

        private void Config_Click(object sender, EventArgs e) {
            if (typeof(int) == sender.GetType()) {
                ShowConfigForm((int)sender);
            }
            else {
                ShowConfigForm(false);
            }
        }

        private void Import_Click(object sender, EventArgs e) {
            using (OpenFileDialog dlg = new OpenFileDialog()) {
                dlg.InitialDirectory = System.Windows.Forms.Application.StartupPath;
                if (dlg.ShowDialog() == DialogResult.OK) {
                    string name = dlg.FileName;
                    Configuration cfg = Configuration.LoadFile(name);
                    if (cfg.configs.Count == 1 && cfg.configs[0].server == Configuration.GetDefaultServer().server) {
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
            if (_controller != null) 
                _controller.Stop();
            HotKeys.Destroy();
            if (appbarform != null)
            {
                appbarform.RegAppBar(true);
                appbarform.Dispose();
                appbarform = null;
            }
            if (configForm != null) {
                configForm.Close();
                configForm = null;
                configfrom_open = false;
            }
            if (serverLogForm != null) {
                serverLogForm.Close();
                serverLogForm = null;
            }
            if (timerDelayCheckUpdate != null) {
                timerDelayCheckUpdate.Elapsed -= timerDelayCheckUpdate_Elapsed;
                timerDelayCheckUpdate.Stop();
                timerDelayCheckUpdate = null;
            }
            if (timerUpdateLatency != null)
            {
                timerUpdateLatency.Elapsed -= timerUpdateLatency_Elapsed;
                timerUpdateLatency.Stop();
                timerUpdateLatency = null;
                _controller.SaveTimerUpdateLatency();
            }
            if (_notifyIcon != null)
                _notifyIcon.Visible = false;
            Application.Exit();
        }

        private void Quit_Click(object sender, EventArgs e) {
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
        }

        private void AboutItem_Click(object sender, EventArgs e) {
            Process.Start("https://github.com/SoDa-GitHub/shadowsocksrr-csharp");
        }

        private void DonateItem_Click(object sender, EventArgs e) {
            Process.Start("https://github.com/SoDa-GitHub/shadowsocksrr-csharp/blob/master/donate.jpg?raw=true");
        }

        [DllImport("user32.dll")]
        private static extern short GetAsyncKeyState(System.Windows.Forms.Keys vKey);

        private void notifyIcon1_Click(object sender, MouseEventArgs e) {
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
                    ShowConfigForm(false);
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

        private void RuleBypassChinaItem_Click(object sender, EventArgs e) {
            _controller.ToggleRuleMode((int)ProxyRuleMode.BypassLanAndChina);
        }

        private void RuleBypassNotChinaItem_Click(object sender, EventArgs e) {
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
            SelectRandomItem.Checked = !SelectRandomItem.Checked;
            _controller.ToggleSelectRandom(SelectRandomItem.Checked);
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

        private void UpdatePACFromGFWListItem_Click(object sender, EventArgs e) {
            _controller.UpdatePACFromGFWList();
        }

        private void UpdatePACFromLanIPListItem_Click(object sender, EventArgs e) {
            _controller.UpdatePACFromOnlinePac("https://raw.githubusercontent.com/shadowsocksrr/breakwa11.github.io/master/ssr/ss_lanip.pac");
        }

        private void UpdatePACFromCNWhiteListItem_Click(object sender, EventArgs e) {
            _controller.UpdatePACFromOnlinePac("https://raw.githubusercontent.com/shadowsocksrr/breakwa11.github.io/master/ssr/ss_white.pac");
        }

        private void UpdatePACFromCNOnlyListItem_Click(object sender, EventArgs e) {
            _controller.UpdatePACFromOnlinePac("https://raw.githubusercontent.com/shadowsocksrr/breakwa11.github.io/master/ssr/ss_white_r.pac");
        }

        private void UpdatePACFromCNIPListItem_Click(object sender, EventArgs e) {
            _controller.UpdatePACFromOnlinePac("https://raw.githubusercontent.com/shadowsocksrr/breakwa11.github.io/master/ssr/ss_cnip.pac");
        }

        private void EditUserRuleFileForGFWListItem_Click(object sender, EventArgs e) {
            _controller.TouchUserRuleFile();
        }

        private void AServerItem_Click(object sender, EventArgs e) {
            MenuItem item = (MenuItem)sender;
            _controller.SelectServerIndex((int)item.Tag);
            DisconnectCurrent_Click(null, null);
            //Configuration config = _controller.GetCurrentConfiguration();
            //for (int id = 0; id < config.configs.Count; ++id) {
            //    Server server = config.configs[id];
            //    server.GetConnections().CloseAll();
            //}
        }

        private void CheckUpdate_Click(object sender, EventArgs e) {
            updateChecker.CheckUpdate(_controller.GetCurrentConfiguration());
        }

        private void CheckNodeUpdate_Click(object sender, EventArgs e)
        {
            CheckNodeUpdate(false);
            //timerUpdateLatency.Stop();
            //UpdateLatencyInterrupt = true;
            //if (updateSubscribeManager.IsInProgress() && updateSubscribeManager._use_proxy == true)
            //    UpdateFreeNodeInterrupt = true;
            //else
            //    updateSubscribeManager.CreateTask(_controller.GetCurrentConfiguration(), updateFreeNodeChecker, -1, false, true);
        }

        private void CheckNodeUpdateUseProxy_Click(object sender, EventArgs e)
        {
            CheckNodeUpdate(true);
            //timerUpdateLatency.Stop();
            //UpdateLatencyInterrupt = true;
            //if (updateSubscribeManager.IsInProgress() && updateSubscribeManager._use_proxy == false)
            //    UpdateFreeNodeInterrupt = true;
            //else
            //    updateSubscribeManager.CreateTask(_controller.GetCurrentConfiguration(), updateFreeNodeChecker, -1, true, true);
        }

        private void CheckNodeUpdate(bool proxy)
        {
            if(subScribeForm_open)
            {
                ShowBalloonTip(I18N.GetString("Tips"), I18N.GetString("Close the server subscription settings window before updating."), ToolTipIcon.Info, 1);
                return;
            }
            timerUpdateLatency.Stop();
            UpdateLatencyInterrupt = true;
            if (!proxy)
            {
                if (updateSubscribeManager.IsInProgress() && updateSubscribeManager._use_proxy == true)
                    UpdateFreeNodeInterrupt = true;
                else
                    updateSubscribeManager.CreateTask(_controller.GetCurrentConfiguration(), updateFreeNodeChecker, -1, false, true);
            }
            else
            {
                if (updateSubscribeManager.IsInProgress() && updateSubscribeManager._use_proxy == false)
                    UpdateFreeNodeInterrupt = true;
                else
                    updateSubscribeManager.CreateTask(_controller.GetCurrentConfiguration(), updateFreeNodeChecker, -1, true, true);
            }
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

        private void DisconnectCurrent_Click(object sender, EventArgs e) {
            Configuration config = _controller.GetCurrentConfiguration();
            for (int id = 0; id < config.configs.Count; ++id) {
                Server server = config.configs[id];
                server.GetConnections().CloseAll();
            }
        }

        private void DisconnectIfUnenableOrNonrandom(object sender, EventArgs e)
        {
            Configuration config = _controller.GetCurrentConfiguration();
            for (int id = 0; id < config.configs.Count; ++id)
            {
                Server server = config.configs[id];
                if(!config.random || !server.isEnable())
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
            try {
                IDataObject iData = Clipboard.GetDataObject();
                if (iData.GetDataPresent(DataFormats.Text)) {
                    List<string> urls = new List<string>();
                    URL_Split((string)iData.GetData(DataFormats.Text), ref urls);
                    int count = 0;
                    foreach (string url in urls) {
                        if (!_controller.IsServerExisting(url))
                            if (_controller.AddServerBySSURL(url))
                                ++count;
                    }
                    if (count > 0)
                    {
                        ShowConfigForm(true);
                        Clipboard.Clear();
                        return true;
                    }
                }
            }
            catch
            {
            }
            if (IsShowBalloonTip)
                ShowBalloonTip(I18N.GetString("Tips"), I18N.GetString("No new SS(R) links in clipboard."), ToolTipIcon.Error, 3);
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
                    //rect = new Rectangle((int)minX, (int)minY, (int)(maxX - minX), (int)(maxY - minY));
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
                    //rect = new Rectangle((int)minX, (int)minY, (int)(maxX - minX), (int)(maxY - minY));
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
            Thread.Sleep(100);
            foreach (Screen screen in Screen.AllScreens) {
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
                    for (int i = 0; i < 100; i++) {
                        double stretch;
                        Rectangle cropRect = GetScanRect(fullImage.Width, fullImage.Height, i, out stretch);
                        if (cropRect.Width == 0)
                            break;

                        string url;
                        Rectangle rect;
                        if (stretch == 1 ? ScanQRCode(screen, fullImage, cropRect, out url, out rect) : ScanQRCodeStretch(screen, fullImage, cropRect, stretch, out url, out rect)) {
                            if (!_controller.IsServerExisting(url))
                            {
                                var success = _controller.AddServerBySSURL(url);
                                QRCodeSplashForm splash = new QRCodeSplashForm();
                                if (success)
                                {
                                    splash.FormClosed += splash_FormClosed;
                                }
                                else if (!ss_only)
                                {
                                    _urlToOpen = url;
                                    //if (url.StartsWith("http://") || url.StartsWith("https://"))
                                    //    splash.FormClosed += openURLFromQRCode;
                                    //else
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
                            else
                            {
                                if (IsShowBalloonTip)
                                    ShowBalloonTip(I18N.GetString("Tips"), I18N.GetString("No new QRCode on screen."), ToolTipIcon.Info, 1);
                                return false;
                            }
                        }
                    }
                    if (decode_fail) {
                        MessageBox.Show(I18N.GetString("Failed to decode QRCode"));
                    }
                }
            }
            if (IsShowBalloonTip) 
                ShowBalloonTip(I18N.GetString("Tips"), I18N.GetString("No QRCode found. Try to zoom in or move it to the center of the screen."), ToolTipIcon.Error,3);
            //MessageBox.Show(I18N.GetString("No QRCode found. Try to zoom in or move it to the center of the screen."));
            return false;
        }

        private void ScanQRCodeItem_Click(object sender, EventArgs e) {
            ScanScreenQRCode(false);
        }

        void splash_FormClosed(object sender, FormClosedEventArgs e) {
            ShowConfigForm(true);
        }

        void openURLFromQRCode(object sender, FormClosedEventArgs e) {
            Process.Start(_urlToOpen);
        }
        
        //public static void SethotkeySettingsForm(ShadowsocksController controller)
        //{
        //    hotkeySettingsForm = new HotkeySettingsForm(controller);
        //}

        private void ShowHotKeySettingsForm()
        {
            HotKeys.StophotKeyManager();
            if (hotkeySettingsForm == null)
            {
                hotkeySettingsForm = new HotkeySettingsForm(_controller);
                hotkeySettingsForm.Show();
                hotkeySettingsForm.FormClosed += hotkeySettingsForm_FormClosed;
            }
            HotkeySettingsForm._IsModified = false;
            hotkeySettingsForm.Activate();
        }

        void hotkeySettingsForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            HotKeys.Init();
            hotkeySettingsForm.Dispose();
            hotkeySettingsForm = null;

            //if (!appbarformAtStart)
            //{
            //    appbarform.RegAppBar(true);
            //    appbarform.Dispose();
            //    appbarform = null;
            //}

            Utils.ReleaseMemory(true);
        }

        private void UpdateLatency_Click(object sender, EventArgs e)
        {
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
                ShowBalloonTip(I18N.GetString("Tips"), I18N.GetString("No new SS(R) links in clipboard or new QRCode on screen."), ToolTipIcon.Info, 3);
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
