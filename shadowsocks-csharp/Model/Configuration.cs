using Shadowsocks.Controller;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Text;
using System.Windows.Forms;
using System.Threading;
using Shadowsocks.Encryption;
using Newtonsoft.Json;
using System.Runtime.InteropServices;
using Shadowsocks.Util;

namespace Shadowsocks.Model
{
    public class UriVisitTime : IComparable
    {
        public DateTime visitTime;
        public string uri;
        public int index;

        public int CompareTo(object other)
        {
            if (!(other is UriVisitTime))
                throw new InvalidOperationException("CompareTo: Not a UriVisitTime");
            if (Equals(other))
                return 0;
            return visitTime.CompareTo(((UriVisitTime)other).visitTime);
        }

    }

    public enum PortMapType : int
    {
        Forward = 0,
        ForceProxy,
        RuleProxy
    }

    public enum ProxyRuleMode : int
    {
        Disable = 0,
        BypassLan,
        BypassLanAndChina,
        BypassLanAndNotChina,
        UserCustom = 16,
    }

    [Serializable]
    public class PortMapConfig
    {
        public bool enable;
        public PortMapType type;
        public string id;
        public string server_addr;
        public int server_port;
        public string remarks;

        public PortMapConfig Clone()
        {
            PortMapConfig pmc = new PortMapConfig();
            pmc.enable = enable;
            pmc.type = type;
            pmc.id = id;
            pmc.server_addr = server_addr;
            pmc.server_port = server_port;
            pmc.remarks = remarks;
            return pmc;
        }
    }

    public class PortMapConfigCache
    {
        public PortMapType type;
        public string id;
        public Server server;
        public string server_addr;
        public int server_port;
    }

    [Serializable]
    public class ServerSubscribe
    {
        public static string DEFAULT_FEED_SUBSCRIBES_URL { get { return "https://raw.githubusercontent.com/githubzgr/subscribes/master/subscribes_base64"; } } 
        public static string DEFAULT_FEED_URL { get{return "https://raw.githubusercontent.com/shadowsocksrr/breakwa11.github.io/master/free/freenodeplain.txt"; } }
        //private static string OLD_DEFAULT_FEED_URL = "https://raw.githubusercontent.com/shadowsocksrr/breakwa11.github.io/master/free/freenode.txt";

        public string id;
        private string url;
        public string Group = "";
        public UInt64 LastUpdateTime = 0;

        public bool JoinUpdate = true;
        public bool UseProxy = false;
        public bool DontUseProxy = false;

        private string host = "";
        public bool groupUserDefine = false;
        public int index = -1;

        public ServerSubscribe()
        {
            URL = "";
            if(String.IsNullOrEmpty(id))
            {
                byte[] id = new byte[16];
                Util.Utils.RandBytes(id, id.Length);
                this.id = BitConverter.ToString(id).Replace("-", "");
            }
        }
        public string URL
        {
            get { return this.url; }
            set
            {
                this.url = value == "" ? DEFAULT_FEED_URL : value;
                host = Utils.GetHostFromUrl(this.url);
            }
        }
        //public string ID
        //{
        //    get { return this.id; }
        //}

        public string Host
        {
            get { return this.host; }
        }

        public ServerSubscribe Clone()
        {
            ServerSubscribe ss = new ServerSubscribe();
            ss.URL = URL;
            ss.Group = Group;
            ss.LastUpdateTime = LastUpdateTime;
            ss.JoinUpdate = JoinUpdate;
            ss.UseProxy = UseProxy;
            ss.DontUseProxy = DontUseProxy;

            ss.id = id;
            ss.host = host;
            ss.groupUserDefine = groupUserDefine;
            return ss;
        }
    }
    
    public class GlobalConfiguration
    {
        public static string config_password = "";
    }

    [Serializable()]
    class ConfigurationException : System.Exception
    {
        public ConfigurationException() : base() { }
        public ConfigurationException(string message) : base(message) { }
        public ConfigurationException(string message, System.Exception inner) : base(message, inner) { }
        protected ConfigurationException(System.Runtime.Serialization.SerializationInfo info,
            System.Runtime.Serialization.StreamingContext context)
        { }
    }
    [Serializable()]
    class ConfigurationWarning : System.Exception
    {
        public ConfigurationWarning() : base() { }
        public ConfigurationWarning(string message) : base(message) { }
        public ConfigurationWarning(string message, System.Exception inner) : base(message, inner) { }
        protected ConfigurationWarning(System.Runtime.Serialization.SerializationInfo info,
            System.Runtime.Serialization.StreamingContext context)
        { }
    }

    [Serializable]
    public class Configuration
    {
        public List<Server> Servers;
        public int index;
        public bool enableBalance;
        public int sysProxyMode;
        public bool shareOverLan;
        public int localPort;
        public string localAuthPassword;

        public string localDnsServer;
        public string dnsServer;
        public int reconnectTimes;
        public string balanceAlgorithm;
        public bool randomInGroup;
        public int TTL;
        public int connectTimeout;

        public int proxyRuleMode;

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
        public bool sameHostForSameTarget;

        private int keepVisitTime;

        public bool isHideTips;
        public bool IsServerLogFormTopmost;
        public APoint ServerLogFormLocation;

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

        public Dictionary<string, string> token = new Dictionary<string, string>();
        public Dictionary<string, PortMapConfig> portMap = new Dictionary<string, PortMapConfig>();

        public HotkeyConfig hotkey;

        private Dictionary<int, ServerSelectStrategy> serverStrategyMap = new Dictionary<int, ServerSelectStrategy>();
        private Dictionary<int, PortMapConfigCache> portMapCache = new Dictionary<int, PortMapConfigCache>();
        private LRUCache<string, UriVisitTime> uricache = new LRUCache<string, UriVisitTime>(180);

        private static string CONFIG_FILE = "gui-config.json";
        private static string CONFIG_FILE_BACKUP = "gui-config.json.backup";


        public static void SetPassword(string password)
        {
            GlobalConfiguration.config_password = password;
        }

        public static bool SetPasswordTry(string old_password, string password)
        {
            if (old_password != GlobalConfiguration.config_password)
                return false;
            return true;
        }

        public bool KeepCurrentServer(int localPort, string targetAddr, string id)
        {
            if (sameHostForSameTarget && targetAddr != null)
            {
                lock (serverStrategyMap)
                {
                    if (!serverStrategyMap.ContainsKey(localPort))
                        serverStrategyMap[localPort] = new ServerSelectStrategy();
                    ServerSelectStrategy serverStrategy = serverStrategyMap[localPort];

                    if (uricache.ContainsKey(targetAddr))
                    {
                        UriVisitTime visit = uricache.Get(targetAddr);
                        int index = -1;
                        for (int i = 0; i < Servers.Count; ++i)
                        {
                            if (Servers[i].id == id)
                            {
                                index = i;
                                break;
                            }
                        }
                        if (index >= 0 && visit.index == index && Servers[index].enable)
                        {
                            uricache.Del(targetAddr);
                            return true;
                        }
                    }
                }
            }
            return false;
        }

        public Server GetCurrentServer(int localPort, ServerSelectStrategy.FilterFunc filter, string targetAddr = null, bool cfgRandom = false, bool usingRandom = false, bool forceRandom = false)
        {
            lock (serverStrategyMap)
            {
                if (!serverStrategyMap.ContainsKey(localPort))
                    serverStrategyMap[localPort] = new ServerSelectStrategy();
                ServerSelectStrategy serverStrategy = serverStrategyMap[localPort];

                uricache.SetTimeout(keepVisitTime);
                uricache.Sweep();
                if (sameHostForSameTarget && !forceRandom && targetAddr != null && uricache.ContainsKey(targetAddr))
                {
                    UriVisitTime visit = uricache.Get(targetAddr);
                    if (visit.index < Servers.Count && Servers[visit.index].enable && Servers[visit.index].ServerSpeedLog().ErrorContinurousTimes == 0)
                    {
                        uricache.Del(targetAddr);
                        return Servers[visit.index];
                    }
                }
                if (forceRandom)
                {
                    int index;
                    if (filter == null && randomInGroup)
                    {
                        index = serverStrategy.Select(Servers, this.index, balanceAlgorithm, delegate (Server server, Server selServer)
                        {
                            if (selServer != null)
                                return selServer.group == server.group;
                            return false;
                        }, true);
                    }
                    else
                    {
                        index = serverStrategy.Select(Servers, this.index, balanceAlgorithm, filter, true);
                    }
                    if (index == -1) return GetErrorServer();
                    return Servers[index];
                }
                else if (usingRandom && cfgRandom)
                {
                    int index;
                    if (filter == null && randomInGroup)
                    {
                        index = serverStrategy.Select(Servers, this.index, balanceAlgorithm, delegate (Server server, Server selServer)
                        {
                            if (selServer != null)
                                return selServer.group == server.group;
                            return false;
                        });
                    }
                    else
                    {
                        index = serverStrategy.Select(Servers, this.index, balanceAlgorithm, filter);
                    }
                    if (index == -1) return GetErrorServer();
                    if (targetAddr != null)
                    {
                        UriVisitTime visit = new UriVisitTime();
                        visit.uri = targetAddr;
                        visit.index = index;
                        visit.visitTime = DateTime.Now;
                        uricache.Set(targetAddr, visit);
                    }
                    return Servers[index];
                }
                else
                {
                    if (index >= 0 && index < Servers.Count)
                    {
                        int selIndex = index;
                        if (usingRandom)
                        {
                            for (int i = 0; i < Servers.Count; ++i)
                            {
                                if (Servers[selIndex].enable)
                                {
                                    break;
                                }
                                else
                                {
                                    selIndex = (selIndex + 1) % Servers.Count;
                                }
                            }
                        }

                        if (targetAddr != null)
                        {
                            UriVisitTime visit = new UriVisitTime();
                            visit.uri = targetAddr;
                            visit.index = selIndex;
                            visit.visitTime = DateTime.Now;
                            uricache.Set(targetAddr, visit);
                        }
                        return Servers[selIndex];
                    }
                    else
                    {
                        return GetErrorServer();
                    }
                }
            }
        }

        public void FlushPortMapCache()
        {
            portMapCache = new Dictionary<int, PortMapConfigCache>();
            Dictionary<string, Server> id2server = new Dictionary<string, Server>();
            Dictionary<string, int> server_group = new Dictionary<string, int>();
            foreach (Server s in Servers)
            {
                id2server[s.id] = s;
                if (!string.IsNullOrEmpty(s.group))
                {
                    server_group[s.group] = 1;
                }
            }
            foreach (KeyValuePair<string, PortMapConfig> pair in portMap)
            {
                int key = 0;
                PortMapConfig pm = pair.Value;
                if (!pm.enable)
                    continue;
                if (id2server.ContainsKey(pm.id) || server_group.ContainsKey(pm.id) || pm.id == null || pm.id.Length == 0)
                { }
                else
                    continue;
                try
                {
                    key = int.Parse(pair.Key);
                }
                catch (FormatException)
                {
                    continue;
                }
                portMapCache[key] = new PortMapConfigCache
                {
                    type = pm.type,
                    id = pm.id,
                    server = id2server.ContainsKey(pm.id) ? id2server[pm.id] : null,
                    server_addr = pm.server_addr,
                    server_port = pm.server_port
                };
            }
            lock (serverStrategyMap)
            {
                List<int> remove_ports = new List<int>();
                foreach (KeyValuePair<int, ServerSelectStrategy> pair in serverStrategyMap)
                {
                    if (portMapCache.ContainsKey(pair.Key)) continue;
                    remove_ports.Add(pair.Key);
                }
                foreach (int port in remove_ports)
                {
                    serverStrategyMap.Remove(port);
                }
                if (!portMapCache.ContainsKey(localPort))
                    serverStrategyMap.Remove(localPort);
            }

            uricache.Clear();
        }

        public Dictionary<int, PortMapConfigCache> GetPortMapCache()
        {
            return portMapCache;
        }

        public static void CheckServer(Server server)
        {
            CheckPort(server.server_port);
            //if (server.server_udp_port != 0)
            CheckPort(server.server_udp_port);
            try
            {
                CheckPassword(server.password);
            }
            catch (ConfigurationWarning cw)
            {
                server.password = "";
                MessageBox.Show(cw.Message, cw.Message, MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            CheckServer(server.server);
        }

        public Configuration()
        {
            index = 0;
            localPort = 1080;

            reconnectTimes = 2;
            keepVisitTime = 180;
            connectTimeout = 5;
            dnsServer = "";
            localDnsServer = "";

            balanceAlgorithm = "LowException";
            enableBalance = false;
            autoBan = false;
            enableLogging = true;
            sysProxyMode = (int)ProxyMode.NoModify;
            proxyRuleMode = (int)ProxyRuleMode.Disable;
            IsServerLogFormTopmost = false;
            ServerLogFormLocation = new APoint();

            serverSubscribes = new List<ServerSubscribe>();
            nodeFeedAutoUpdate = true;
            nodeFeedAutoUpdateInterval = 3;
            nodeFeedAutoLatency = false;
            nodeFeedAutoLatencyInterval = 30;
            nodeFeedAutoUpdateUseProxy = false;
            nodeFeedAutoUpdateTryUseProxy = false;
            sortServersBySubscriptionsOrder = true;
            LastnodeFeedAutoLatency = 0;
            LastUpdateSubscribesTime = 0;

            Servers = new List<Server>()
            {
                GetDefaultServer()
            };

            hotkey = new HotkeyConfig();
        }

        public void CopyFrom(Configuration config)
        {
            //Servers.Clear();
            //foreach(Server s in config.Servers)
            //{
            //    Servers.Add(s.Clone());
            //    //Servers[Servers.Count - 1].CopyFrom(s);
            //}
            Servers = config.Servers;
            index = config.index;
            enableBalance = config.enableBalance;
            sysProxyMode = config.sysProxyMode;
            shareOverLan = config.shareOverLan;
            localPort = config.localPort;
            reconnectTimes = config.reconnectTimes;
            balanceAlgorithm = config.balanceAlgorithm;
            randomInGroup = config.randomInGroup;
            TTL = config.TTL;
            connectTimeout = config.connectTimeout;
            dnsServer = config.dnsServer;
            localDnsServer = config.localDnsServer;
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
            sameHostForSameTarget = config.sameHostForSameTarget;
            keepVisitTime = config.keepVisitTime;
            isHideTips = config.isHideTips;
            nodeFeedAutoUpdate = config.nodeFeedAutoUpdate;
            nodeFeedAutoUpdateInterval = config.nodeFeedAutoUpdateInterval;
            nodeFeedAutoLatency = config.nodeFeedAutoLatency;
            nodeFeedAutoLatencyInterval = config.nodeFeedAutoLatencyInterval;
            nodeFeedAutoUpdateUseProxy = config.nodeFeedAutoUpdateUseProxy;
            nodeFeedAutoUpdateTryUseProxy = config.nodeFeedAutoUpdateTryUseProxy;
            sortServersBySubscriptionsOrder = config.sortServersBySubscriptionsOrder;
            LastnodeFeedAutoLatency = config.LastnodeFeedAutoLatency;
            LastUpdateSubscribesTime = config.LastUpdateSubscribesTime;
            //serverSubscribeSetting = serverSubscribeSetting.Clone();
            serverSubscribes = config.serverSubscribes;
            //serverSubscribes.Clear();
            //foreach (ServerSubscribe ss in config.serverSubscribes)
            //{
            //    serverSubscribes.Add(ss.Clone());
            //}
        }
        public string getMemory(object o) // 获取引用类型的内存地址方法    
        {
            GCHandle h = GCHandle.Alloc(o, GCHandleType.WeakTrackResurrection);

            IntPtr addr = GCHandle.ToIntPtr(h);

            return "0x" + addr.ToString("X");
        }

        public void FixConfiguration()
        {
            if (localPort == 0)
            {
                localPort = 1080;
            }
            if (keepVisitTime == 0)
            {
                keepVisitTime = 180;
            }
            if (portMap == null)
            {
                portMap = new Dictionary<string, PortMapConfig>();
            }
            if (token == null)
            {
                token = new Dictionary<string, string>();
            }
            if (connectTimeout == 0)
            {
                connectTimeout = 10;
                reconnectTimes = 2;
                TTL = 180;
                keepVisitTime = 180;
            }
            if (localAuthPassword == null || localAuthPassword.Length < 16)
            {
                localAuthPassword = randString(20);
            }

            Dictionary<string, int> id = new Dictionary<string, int>();
            if (index < 0 || index >= Servers.Count) index = 0;
            foreach (Server server in Servers)
            {
                if (id.ContainsKey(server.id))
                {
                    byte[] new_id = new byte[16];
                    Util.Utils.RandBytes(new_id, new_id.Length);
                    server.id = BitConverter.ToString(new_id).Replace("-", "");
                }
                else
                {
                    id[server.id] = 0;
                }
            }
        }

        private static string randString(int len)
        {
            string set = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789-_";
            string ret = "";
            Random random = new Random();
            for (int i = 0; i < len; ++i)
            {
                ret += set[random.Next(set.Length)];
            }
            return ret;
        }

        public static Configuration LoadFile(string filename)
        {
            try
            {
                string configContent = File.ReadAllText(filename);
                return Load(configContent);
            }
            catch (Exception e)
            {
                if (!(e is FileNotFoundException))
                {
                    Console.WriteLine(e);
                }
                return new Configuration();
            }
        }

        public static Configuration Load()
        {
            return LoadFile(CONFIG_FILE);
        }

        public static void Save(Configuration config)
        {
            if (config.index < 0 || config.index >= config.Servers.Count)
            {
                int newindex = config.Servers.FindIndex(t => t.enable);
                if (newindex != -1)
                    config.index = newindex;
                else
                    config.index = 0;
            }
            try
            {
                string jsonString = SimpleJson.SimpleJson.SerializeObject(config);
                if (GlobalConfiguration.config_password.Length > 0)
                {
                    IEncryptor encryptor = EncryptorFactory.GetEncryptor("aes-256-cfb", GlobalConfiguration.config_password, false);
                    byte[] cfg_data = UTF8Encoding.UTF8.GetBytes(jsonString);
                    byte[] cfg_encrypt = new byte[cfg_data.Length + 128];
                    int data_len = 0;
                    const int buffer_size = 32768;
                    byte[] input = new byte[buffer_size];
                    byte[] ouput = new byte[buffer_size + 128];
                    for (int start_pos = 0; start_pos < cfg_data.Length; start_pos += buffer_size)
                    {
                        int len = Math.Min(cfg_data.Length - start_pos, buffer_size);
                        int out_len;
                        Buffer.BlockCopy(cfg_data, start_pos, input, 0, len);
                        encryptor.Encrypt(input, len, ouput, out out_len);
                        Buffer.BlockCopy(ouput, 0, cfg_encrypt, data_len, out_len);
                        data_len += out_len;
                    }
                    jsonString = System.Convert.ToBase64String(cfg_encrypt, 0, data_len);
                }
                using (StreamWriter sw = new StreamWriter(File.Open(CONFIG_FILE, FileMode.Create)))
                {
                    sw.Write(jsonString);
                    sw.Flush();
                }

                if (File.Exists(CONFIG_FILE_BACKUP))
                {
                    DateTime dt = File.GetLastWriteTimeUtc(CONFIG_FILE_BACKUP);
                    DateTime now = DateTime.Now;
                    if ((now - dt).TotalHours > 4)
                    {
                        File.Copy(CONFIG_FILE, CONFIG_FILE_BACKUP, true);
                    }
                }
                else
                {
                    File.Copy(CONFIG_FILE, CONFIG_FILE_BACKUP, true);
                }
            }
            catch (IOException e)
            {
                Console.Error.WriteLine(e);
            }
        }

        public static Configuration Load(string config_str)
        {
            try
            {
                if (GlobalConfiguration.config_password.Length > 0)
                {
                    byte[] cfg_encrypt = System.Convert.FromBase64String(config_str);
                    IEncryptor encryptor = EncryptorFactory.GetEncryptor("aes-256-cfb", GlobalConfiguration.config_password, false);
                    byte[] cfg_data = new byte[cfg_encrypt.Length];
                    int data_len = 0;
                    const int buffer_size = 32768;
                    byte[] input = new byte[buffer_size];
                    byte[] ouput = new byte[buffer_size + 128];
                    for (int start_pos = 0; start_pos < cfg_encrypt.Length; start_pos += buffer_size)
                    {
                        int len = Math.Min(cfg_encrypt.Length - start_pos, buffer_size);
                        int out_len;
                        Buffer.BlockCopy(cfg_encrypt, start_pos, input, 0, len);
                        encryptor.Decrypt(input, len, ouput, out out_len);
                        Buffer.BlockCopy(ouput, 0, cfg_data, data_len, out_len);
                        data_len += out_len;
                    }
                    config_str = UTF8Encoding.UTF8.GetString(cfg_data, 0, data_len);
                }
            }
            catch
            {

            }
            try
            {
                Configuration config = SimpleJson.SimpleJson.DeserializeObject<Configuration>(config_str, new JsonSerializerStrategy());
                config.FixConfiguration();
                return config;
            }
            catch
            {
            }
            return null;
        }

        public static Server GetDefaultServer()
        {
            return new Server();
        }

        public bool isDefaultConfig()
        {
            if (Servers.Count == 1 && Servers[0].server == Configuration.GetDefaultServer().server)
                return true;
            return false;
        }

        //public static Server CopyServer(Server server)
        //{
        //    Server s = new Server();
        //    s.server = server.server;
        //    s.server_port = server.server_port;
        //    s.method = server.method;
        //    s.protocol = server.protocol;
        //    s.protocolparam = server.protocolparam ?? "";
        //    s.obfs = server.obfs;
        //    s.obfsparam = server.obfsparam ?? "";
        //    s.password = server.password;
        //    s.remarks = server.remarks;
        //    s.group = server.group;
        //    s.udp_over_tcp = server.udp_over_tcp;
        //    s.server_udp_port = server.server_udp_port;
        //    return s;
        //}

        public static ServerSubscribe CopySubscribes(ServerSubscribe sss)
        {
            ServerSubscribe ss = new ServerSubscribe();
            ss.URL = sss.URL;
            ss.Group = sss.Group;
            ss.LastUpdateTime = sss.LastUpdateTime;
            ss.UseProxy = sss.UseProxy;
            return ss;
        }

        public static Server GetErrorServer()
        {
            Server server = new Server();
            server.server = "invalid";
            return server;
        }

        public static void CheckPort(int port)
        {
            if (port < 0 || port > 65535)
            {
                throw new ConfigurationException(I18N.GetString("Port out of range"));
            }
        }

        private static void CheckPassword(string password)
        {
            if (string.IsNullOrEmpty(password))
            {
                throw new ConfigurationWarning(I18N.GetString("Password are blank"));
                //throw new ConfigurationException(I18N.GetString("Password can not be blank"));
            }
        }

        private static void CheckServer(string server)
        {
            if (string.IsNullOrEmpty(server))
            {
                throw new ConfigurationException(I18N.GetString("Server IP can not be blank"));
            }
        }

        private class JsonSerializerStrategy : SimpleJson.PocoJsonSerializerStrategy
        {
            // convert string to int
            public override object DeserializeObject(object value, Type type)
            {
                if (type == typeof(Int32) && value.GetType() == typeof(string))
                {
                    return Int32.Parse(value.ToString());
                }
                return base.DeserializeObject(value, type);
            }
        }

        //public int GetNodeFeedAutoUpdateInterval()
        //{
        //    return 
        //    switch (nodeFeedAutoUpdateIntervalIndex)
        //    {
        //        case 0:
        //            return 1;
        //        case 1:
        //            return 3;
        //    }
        //}

        //public int GetNodeFeedAutoLatencyInterval()
        //{

        //}
    }

    [Serializable]
    public class ServerTrans
    {
        public long latency;
        public long totalUploadBytes;
        public long totalDownloadBytes;

        //void AddUpload(long bytes)
        //{
        //    //lock (this)
        //    {
        //        totalUploadBytes += bytes;
        //    }
        //}
        //void AddDownload(long bytes)
        //{
        //    //lock (this)
        //    {
        //        totalDownloadBytes += bytes;
        //    }
        //}
    }

    [Serializable]
    public class ServerTransferTotal
    {
        private static string LOG_FILE = "transfer_log.json";

        public Dictionary<string, object> servers = new Dictionary<string, object>();
        private int saveCounter;
        private DateTime saveTime;

        public static ServerTransferTotal Load()
        {
            try
            {
                string config_str = File.ReadAllText(LOG_FILE);
                ServerTransferTotal config = new ServerTransferTotal();
                try
                {
                    if (GlobalConfiguration.config_password.Length > 0)
                    {
                        byte[] cfg_encrypt = System.Convert.FromBase64String(config_str);
                        IEncryptor encryptor = EncryptorFactory.GetEncryptor("aes-256-cfb", GlobalConfiguration.config_password, false);
                        byte[] cfg_data = new byte[cfg_encrypt.Length];
                        int data_len;
                        encryptor.Decrypt(cfg_encrypt, cfg_encrypt.Length, cfg_data, out data_len);
                        config_str = UTF8Encoding.UTF8.GetString(cfg_data, 0, data_len);
                    }
                }
                catch
                {

                }
                config.servers = SimpleJson.SimpleJson.DeserializeObject<Dictionary<string, object>>(config_str, new JsonSerializerStrategy());
                config.Init();
                return config;
            }
            catch (Exception e)
            {
                if (!(e is FileNotFoundException))
                {
                    Console.WriteLine(e);
                }
                return new ServerTransferTotal();
            }
        }

        public void Init()
        {
            saveCounter = 256;
            saveTime = DateTime.Now;
            if (servers == null)
                servers = new Dictionary<string, object>();
        }

        public static void Save(ServerTransferTotal config)
        {
            try
            {
                using (StreamWriter sw = new StreamWriter(File.Open(LOG_FILE, FileMode.Create)))
                {
                    string jsonString = SimpleJson.SimpleJson.SerializeObject(config.servers);
                    if (GlobalConfiguration.config_password.Length > 0)
                    {
                        IEncryptor encryptor = EncryptorFactory.GetEncryptor("aes-256-cfb", GlobalConfiguration.config_password, false);
                        byte[] cfg_data = UTF8Encoding.UTF8.GetBytes(jsonString);
                        byte[] cfg_encrypt = new byte[cfg_data.Length + 128];
                        int data_len;
                        encryptor.Encrypt(cfg_data, cfg_data.Length, cfg_encrypt, out data_len);
                        jsonString = System.Convert.ToBase64String(cfg_encrypt, 0, data_len);
                    }
                    sw.Write(jsonString);
                    sw.Flush();
                }
            }
            catch (IOException e)
            {
                Console.Error.WriteLine(e);
            }
        }

        public void Clear(string server)
        {
            lock (servers)
            {
                if (servers.ContainsKey(server))
                {
                    ((ServerTrans)servers[server]).totalUploadBytes = 0;
                    ((ServerTrans)servers[server]).totalDownloadBytes = 0;
                }
            }
        }

        public void SetLatency(string server, Int64 size)
        {
            lock (servers)
            {
                if (!servers.ContainsKey(server))
                    servers.Add(server, new ServerTrans());
                ((ServerTrans)servers[server]).latency = size;
            }
            if (--saveCounter <= 0)
            {
                saveCounter = 256;
                if ((DateTime.Now - saveTime).TotalMinutes > 10)
                {
                    lock (servers)
                    {
                        Save(this);
                        saveTime = DateTime.Now;
                    }
                }
            }
        }
        public void AddtotalUpload(string server, Int64 size)
        {
            lock (servers)
            {
                if (!servers.ContainsKey(server))
                    servers.Add(server, new ServerTrans());
                ((ServerTrans)servers[server]).totalUploadBytes += size;
            }
            if (--saveCounter <= 0)
            {
                saveCounter = 256;
                if ((DateTime.Now - saveTime).TotalMinutes > 10)
                {
                    lock (servers)
                    {
                        Save(this);
                        saveTime = DateTime.Now;
                    }
                }
            }
        }
        public void AddtotalDownload(string server, Int64 size)
        {
            lock (servers)
            {
                if (!servers.ContainsKey(server))
                    servers.Add(server, new ServerTrans());
                ((ServerTrans)servers[server]).totalDownloadBytes += size;
            }
            if (--saveCounter <= 0)
            {
                saveCounter = 256;
                if ((DateTime.Now - saveTime).TotalMinutes > 10)
                {
                    lock (servers)
                    {
                        Save(this);
                        saveTime = DateTime.Now;
                    }
                }
            }
        }

        private class JsonSerializerStrategy : SimpleJson.PocoJsonSerializerStrategy
        {
            public override object DeserializeObject(object value, Type type)
            {
                if (type == typeof(Int64) && value.GetType() == typeof(string))
                {
                    return Int64.Parse(value.ToString());
                }
                else if (type == typeof(object))
                {
                    return base.DeserializeObject(value, typeof(ServerTrans));
                }
                return base.DeserializeObject(value, type);
            }
        }
    }


    [Serializable]
    public class APoint
    {
        public int X;
        public int Y;

        public APoint()
        {
            X = -1;
            Y = -1;
        }
    }
}
