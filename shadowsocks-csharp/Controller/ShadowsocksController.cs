using System.IO;
using Shadowsocks.Model;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Net.Sockets;
using System.Net;
using Shadowsocks.Controller.Hotkeys;
using Shadowsocks.Util;

namespace Shadowsocks.Controller
{
    public enum ProxyMode
    {
        NoModify,
        Direct,
        Pac,
        Global,
    }

    public class ShadowsocksController
    {
        // controller:
        // handle user actions
        // manipulates UI
        // interacts with low level logic

        private Listener _listener;
        private List<Listener> _port_map_listener;
        private PACServer _pacServer;
        private Configuration _config;
        private ServerTransferTotal _transfer;
        public IPRangeSet _rangeSet;
#if !_CONSOLE
        private HttpProxyRunner polipoRunner;
#endif
        private GFWListUpdater gfwListUpdater;
        private bool stopped = false;
        private bool firstRun = true;

        //private System.Timers.Timer timerUpdateLatency;


        public class PathEventArgs : EventArgs
        {
            public string Path;
        }

        public event EventHandler ConfigChanged;
        public event EventHandler ToggleModeChanged;
        public event EventHandler ToggleRuleModeChanged;
        //public event EventHandler ShareOverLANStatusChanged;
        public event EventHandler ShowConfigFormEvent;

        public event EventHandler UpdateNodeFromSubscribeForm;

        // when user clicked Edit PAC, and PAC file has already created
        public event EventHandler<PathEventArgs> PACFileReadyToOpen;
        public event EventHandler<PathEventArgs> UserRuleFileReadyToOpen;

        public event EventHandler<GFWListUpdater.ResultEventArgs> UpdatePACFromGFWListCompleted;

        public event ErrorEventHandler UpdatePACFromGFWListError;

        public event ErrorEventHandler Errored;

        public static Configuration tmpconfig;

        public ShadowsocksController()
        {
            _config = Configuration.Load();
            _transfer = ServerTransferTotal.Load();

            foreach (Server server in _config.Servers)
            {
                if (_transfer.servers.ContainsKey(server.server))
                {
                    ServerSpeedLog log = new ServerSpeedLog(((ServerTrans)_transfer.servers[server.server]).totalUploadBytes, ((ServerTrans)_transfer.servers[server.server]).totalDownloadBytes);
                    server.SetServerSpeedLog(log);
                }
            }
        }

        public void Start(bool regHotkeys = true)
        {
            Reload();
            if (regHotkeys && _config.hotkey.RegHotkeysAtStartup)
            {
                HotKeys.Init();
                HotkeyReg.RegAllHotkeys();
            }
        }

        protected void ReportError(Exception e)
        {
            Errored?.Invoke(this, new ErrorEventArgs(e));
        }

        public void ReloadIPRange()
        {
            _rangeSet = new IPRangeSet();
            _rangeSet.LoadChn();
            if (_config.proxyRuleMode == (int)ProxyRuleMode.BypassLanAndNotChina)
            {
                _rangeSet.Reverse();
            }
        }

        public Configuration GetCurrentConfiguration()
        {
            return _config;
        }

        private int FindFirstMatchServer(Server server, List<Server> servers)
        {
            for (int i = 0; i < servers.Count; ++i)
            {
                if (server.isMatchServer(servers[i]))
                {
                    return i;
                }
            }
            return -1;
        }

        public void AppendConfiguration(Configuration mergeConfig, List<Server> servers)
        {
            if (servers != null)
            {
                for (int j = 0; j < servers.Count; ++j)
                {
                    if (FindFirstMatchServer(servers[j], mergeConfig.Servers) == -1)
                    {
                        mergeConfig.Servers.Add(servers[j]);
                    }
                }
            }
        }

        public void MergeConfiguration(Configuration mergeConfig)
        {
            AppendConfiguration(_config, mergeConfig.Servers);
            SaveConfig(_config);
        }

        public bool SaveServersConfig(string config)
        {
            Configuration new_cfg = Configuration.Load(config);
            if (new_cfg != null)
            {
                SaveServersConfig(new_cfg);
                return true;
            }
            return false;
        }

        public void SaveServersConfig(Configuration config, bool reload = true)
        {
            _config.CopyFrom(config);
            SelectServerIndex(_config.index, reload);
        }

        public void SavePortConfig(Dictionary<string, PortMapConfig> portmap)
        {
            _config.portMap = portmap;
            SaveConfig(_config);
        }

        public int AddServer(Server s, string currentEditServerID = null, bool reload = true)
        {
            try
            {
                string currentserverid = _config.Servers[_config.index].id;
                int index = -1;
                if (!String.IsNullOrEmpty(currentEditServerID))
                {
                    index = _config.Servers.FindIndex(t => t.id == currentEditServerID);
                }
                index++;
                _config.Servers.Insert(index, s);

                if (index > -1 && index <= _config.index)
                {
                    int newindex = _config.Servers.FindIndex(t => t.id == currentserverid);
                    if (newindex != -1 || (newindex = _config.Servers.FindIndex(t => t.enable)) != -1)
                        _config.index = newindex;
                    else
                        _config.index = 0;
                }
                if (reload)
                    SaveConfig(_config);
                return index;
            }
            catch (Exception e)
            {
                Logging.LogUsefulException(e);
            }
            return -1;
        }

        public void ToggleMode(ProxyMode mode)
        {
            _config.sysProxyMode = (int)mode;
            SaveConfig(_config);
            ToggleModeChanged?.Invoke(this, new EventArgs());
        }

        public void ToggleRuleMode(int mode)
        {
            _config.proxyRuleMode = mode;
            SaveConfig(_config);
            ToggleRuleModeChanged?.Invoke(this, new EventArgs());
        }

        public void ToggleEnableBalance(bool enabled)
        {
            _config.enableBalance = enabled;
            SaveConfig(_config);
        }

        public void ToggleShareOverLAN(bool enabled)
        {
            _config.shareOverLan = enabled;
            SaveConfig(_config);
        }

        public void ToggleSameHostForSameTargetRandom(bool enabled)
        {
            _config.sameHostForSameTarget = enabled;
            SaveConfig(_config);
        }

        public void SelectServerIndex(int index, bool reload = true)
        {
            _config.index = index;
            SaveConfig(_config, reload);
        }

        public void Stop()
        {
            if (stopped)
            {
                return;
            }
            stopped = true;

            if (_port_map_listener != null)
            {
                foreach (Listener l in _port_map_listener)
                {
                    l.Stop();
                }
                _port_map_listener = null;
            }
            if (_listener != null)
            {
                _listener.Stop();
            }
#if !_CONSOLE
            if (polipoRunner != null)
            {
                polipoRunner.Stop();
            }
            if (_config.sysProxyMode != (int)ProxyMode.NoModify && _config.sysProxyMode != (int)ProxyMode.Direct)
            {
                SystemProxy.Update(_config, true);
            }
#endif
            ServerTransferTotal.Save(_transfer);
        }

        public void ClearTransferTotal(string server_addr)
        {
            _transfer.Clear(server_addr);
            foreach (Server server in _config.Servers)
            {
                if (server.server == server_addr)
                {
                    if (_transfer.servers.ContainsKey(server.server))
                    {
                        server.ServerSpeedLog().ClearTrans();
                    }
                }
            }
        }

        public void TouchPACFile()
        {
            string pacFilename = _pacServer.TouchPACFile();
            if (PACFileReadyToOpen != null)
            {
                PACFileReadyToOpen(this, new PathEventArgs() { Path = pacFilename });
            }
        }

        public void TouchUserRuleFile()
        {
            string userRuleFilename = _pacServer.TouchUserRuleFile();
            if (UserRuleFileReadyToOpen != null)
            {
                UserRuleFileReadyToOpen(this, new PathEventArgs() { Path = userRuleFilename });
            }
        }

        public void UpdatePACTo(GFWListUpdater.Templates templates)
        {
            if (gfwListUpdater != null)
            {
                gfwListUpdater.UpdatePACTo(_config, templates);
            }
        }

        protected void Reload()
        {
            if (_config.enableLogging)
            {
                Logging.OpenLogFile();
            }
            else
            {
                Logging.CloseLogFile();
            }
            if (_port_map_listener != null)
            {
                foreach (Listener l in _port_map_listener)
                {
                    l.Stop();
                }
                _port_map_listener = null;
            }
            // some logic in configuration updated the config when saving, we need to read it again
            _config.FlushPortMapCache();
            ReloadIPRange();

            HostMap hostMap = new HostMap();
            hostMap.LoadHostFile();
            HostMap.Instance().Clear(hostMap);

#if !_CONSOLE
            if (polipoRunner == null)
            {
                polipoRunner = new HttpProxyRunner();
            }
#endif
            if (_pacServer == null)
            {
                _pacServer = new PACServer();
                _pacServer.PACFileChanged += pacServer_PACFileChanged;
            }
            _pacServer.UpdateConfiguration(_config);
            if (gfwListUpdater == null)
            {
                gfwListUpdater = new GFWListUpdater();
                gfwListUpdater.UpdateCompleted += pacServer_PACUpdateCompleted;
                gfwListUpdater.Error += pacServer_PACUpdateError;
            }

            // don't put polipoRunner.Start() before pacServer.Stop()
            // or bind will fail when switching bind address from 0.0.0.0 to 127.0.0.1
            // though UseShellExecute is set to true now
            // http://stackoverflow.com/questions/10235093/socket-doesnt-close-after-application-exits-if-a-launched-process-is-open
            //bool _firstRun = firstRun;
            //for (int i = 1; i <= 5; ++i)
            {
                //_firstRun = false;
                try
                {
                    if (_listener != null && !_listener.isConfigChange(_config))
                    {
                        Local local = new Local(_config, _transfer, _rangeSet);
                        _listener.GetServices()[0] = local;
#if !_CONSOLE
                        if (polipoRunner.HasExited())
                        {
                            polipoRunner.Stop();
                            polipoRunner.Start(_config);

                            _listener.GetServices()[3] = new HttpPortForwarder(polipoRunner.RunningPort, _config);
                        }
#endif
                    }
                    else
                    {
                        if (_listener != null)
                        {
                            _listener.Stop();
                            _listener = null;
                        }

#if !_CONSOLE
                        polipoRunner.Stop();
                        polipoRunner.Start(_config);
#endif

                        Local local = new Local(_config, _transfer, _rangeSet);
                        List<Listener.Service> services = new List<Listener.Service>();
                        services.Add(local);
                        services.Add(_pacServer);
                        services.Add(new APIServer(this, _config));
#if !_CONSOLE
                        services.Add(new HttpPortForwarder(polipoRunner.RunningPort, _config));
#endif
                        _listener = new Listener(services);
                        _listener.Start(_config, 0);
                    }
                    //break;
                }
                catch (Exception e)
                {
                    // translate Microsoft language into human language
                    // i.e. An attempt was made to access a socket in a way forbidden by its access permissions => Port already in use
                    if (e is SocketException)
                    {
                        SocketException se = (SocketException)e;
                        if (se.SocketErrorCode == SocketError.AccessDenied)
                        {
                            e = new Exception(I18N.GetString("Port already in use") + string.Format(" {0}", _config.localPort), e);
                        }
                    }
                    Logging.LogUsefulException(e);
                    //if (firstRun)
                    {
                        ReportError(e);
                        //break;
                    }
                    //else
                    //{
                    //    Thread.Sleep(1000 * i * i);
                    //}
                    if (_listener != null)
                    {
                        _listener.Stop();
                        _listener = null;
                    }
                }
            }

            _port_map_listener = new List<Listener>();
            foreach (KeyValuePair<int, PortMapConfigCache> pair in _config.GetPortMapCache())
            {
                try
                {
                    Local local = new Local(_config, _transfer, _rangeSet);
                    List<Listener.Service> services = new List<Listener.Service>();
                    services.Add(local);
                    Listener listener = new Listener(services);
                    listener.Start(_config, pair.Key);
                    _port_map_listener.Add(listener);
                }
                catch (Exception e)
                {
                    // translate Microsoft language into human language
                    // i.e. An attempt was made to access a socket in a way forbidden by its access permissions => Port already in use
                    if (e is SocketException)
                    {
                        SocketException se = (SocketException)e;
                        if (se.SocketErrorCode == SocketError.AccessDenied)
                        {
                            e = new Exception(I18N.GetString("Port already in use") + string.Format(" {0}", pair.Key), e);
                        }
                    }
                    Logging.LogUsefulException(e);
                    ReportError(e);
                }
            }

            //ConfigChanged?.Invoke(this, new EventArgs());
            if (!firstRun)
                InvokeConfigChanged(new object[] { new List<string>() { "All" } }, new EventArgs());

            UpdateSystemProxy();
            Util.Utils.ReleaseMemory(true);
            firstRun = false;
        }


        protected void SaveConfig(Configuration newConfig, bool reload = true)
        {
            Configuration.Save(newConfig);
            if(reload)
                Reload();
        }

        public void JustReload()
        {
            Reload();
        }


        private void UpdateSystemProxy()
        {
#if !_CONSOLE
            if (_config.sysProxyMode != (int)ProxyMode.NoModify)
            {
                SystemProxy.Update(_config, false);
            }
#endif
        }

        private void pacServer_PACFileChanged(object sender, EventArgs e)
        {
            UpdateSystemProxy();
        }

        private void pacServer_PACUpdateCompleted(object sender, GFWListUpdater.ResultEventArgs e)
        {
            if (UpdatePACFromGFWListCompleted != null)
                UpdatePACFromGFWListCompleted(sender, e);
        }

        private void pacServer_PACUpdateError(object sender, ErrorEventArgs e)
        {
            if (UpdatePACFromGFWListError != null)
                UpdatePACFromGFWListError(sender, e);
        }

        public void ShowConfigForm(int index)
        {
            //if (ShowConfigFormEvent != null)
            //{
                ShowConfigFormEvent?.Invoke(index, new EventArgs());
            //}
        }

        /// <summary>
        /// Disconnect all connections from the remote host.
        /// </summary>
        public void DisconnectAllConnections()
        {
            List<Server> AllServers = GetCurrentConfiguration().Servers;
            foreach(Server s in AllServers)
            {
                s.GetConnections().CloseAll();
            }
        }
        

        public void SaveHotkeyConfig(HotkeyConfig newConfig)
        {
            _config.hotkey = newConfig;
            SaveConfig(_config, false);
            //ConfigChanged?.Invoke(this, new EventArgs());
            InvokeConfigChanged(new object[] { new List<string>() { "HotkeySettingsForm" } }, new EventArgs());
        }

        public void SaveTimerUpdateLatency()
        {
            _config.LastnodeFeedAutoLatency = (UInt64)Math.Floor(DateTime.Now.Subtract(new DateTime(1970, 1, 1, 0, 0, 0)).TotalSeconds);
            SaveConfig(_config, false);
            InvokeConfigChanged(new object[] { new List<string>() { "MenuViewController" } }, new EventArgs());
        }

        public void InvokeUpdateNodeFromSubscribeForm(object obj, EventArgs e)
        {
            UpdateNodeFromSubscribeForm?.Invoke(obj, e);
        }

        public void InvokeConfigChanged(object sender,EventArgs e)
        {
            ConfigChanged?.Invoke(sender, e);
        }

    }
}
