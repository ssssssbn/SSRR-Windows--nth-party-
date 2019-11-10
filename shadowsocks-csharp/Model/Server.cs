using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;
#if !_CONSOLE
using SimpleJson;
#endif
using Shadowsocks.Controller;
using System.Text.RegularExpressions;
using System.Net;
using System.Net.Sockets;
using System.Linq;

namespace Shadowsocks.Model
{
    public class DnsBuffer
    {
        public IPAddress ip;
        public DateTime updateTime;
        public string host;
        public bool force_expired;
        public bool isExpired(string host)
        {
            if (updateTime == null) return true;
            if (this.host != host) return true;
            if (force_expired && (DateTime.Now - updateTime).TotalMinutes > 1) return true;
            return (DateTime.Now - updateTime).TotalMinutes > 30;
        }
        public void UpdateDns(string host, IPAddress ip)
        {
            updateTime = DateTime.Now;
            this.ip = new IPAddress(ip.GetAddressBytes());
            this.host = host;
            force_expired = false;
        }
    }

    public class Connections
    {
        private System.Collections.Generic.Dictionary<IHandler, Int32> sockets = new Dictionary<IHandler, int>();
        public bool AddRef(IHandler socket)
        {
            lock (this)
            {
                if (sockets.ContainsKey(socket))
                {
                    sockets[socket] += 1;
                }
                else
                {
                    sockets[socket] = 1;
                }
                return true;
            }
        }
        public bool DecRef(IHandler socket)
        {
            lock (this)
            {
                if (sockets.ContainsKey(socket))
                {
                    sockets[socket] -= 1;
                    if (sockets[socket] == 0)
                    {
                        sockets.Remove(socket);
                    }
                }
                else
                {
                    return false;
                }
                return true;
            }
        }
        public void CloseAll()
        {
            IHandler[] s;
            lock (this)
            {
                s = new IHandler[sockets.Count];
                sockets.Keys.CopyTo(s, 0);
            }
            foreach (IHandler handler in s)
            {
                try
                {
                    handler.Shutdown(true);
                }
                catch
                {
                    Logging.Error("handler Shutdown failed.");
                }
            }
        }
        public int Count
        {
            get
            {
                return sockets.Count;
            }
        }
    }

    [Serializable]
    public class Server
    {
        public string id;
        public string server;
        public ushort server_port;
        public ushort server_udp_port;
        public string password;
        public string method;
        public string protocol;
        public string protocolparam;
        public string obfs;
        public string obfsparam;
        public string remarks_base64;
        public string group;
        public bool enable;
        public bool udp_over_tcp;

        public long latency
        {
            get
            {
                if (serverSpeedLog != null)
                    return serverSpeedLog.AvgConnectTime / 1000;
                else
                    return -1;
            }
            set
            {
                if (serverSpeedLog != null)
                    serverSpeedLog.AvgConnectTime = value * 1000;
            }
        }

        public static int LATENCY_ERROR = -2;
        public static int LATENCY_PENDING = -1;
        public static int LATENCY_TESTING = 0;


        private object protocoldata;
        private object obfsdata;
        private ServerSpeedLog serverSpeedLog = new ServerSpeedLog();
        private DnsBuffer dnsBuffer = new DnsBuffer();
        private Connections Connections = new Connections();
        private static Server forwardServer = new Server();

        public void InheritDataFrom(Server Server)
        {
            protocoldata = Server.protocoldata;
            obfsdata = Server.obfsdata;
            serverSpeedLog = Server.serverSpeedLog;
            dnsBuffer = Server.dnsBuffer;
            Connections = Server.Connections;
            //enable = Server.enable;
        }

        public void CopyServerInfo(Server Server)
        {
            remarks = Server.remarks;
            group = Server.group;
        }

        public static Server GetForwardServerRef()
        {
            return forwardServer;
        }

        public void SetConnections(Connections Connections)
        {
            this.Connections = Connections;
        }

        public Connections GetConnections()
        {
            return Connections;
        }

        public DnsBuffer DnsBuffer()
        {
            return dnsBuffer;
        }

        public ServerSpeedLog ServerSpeedLog()
        {
            return serverSpeedLog;
        }
        public void SetServerSpeedLog(ServerSpeedLog log)
        {
            log.AvgConnectTime = serverSpeedLog.AvgConnectTime;
            serverSpeedLog = log;
        }

        public string remarks
        {
            get
            {
                if (remarks_base64.Length == 0)
                {
                    return string.Empty;
                }
                try
                {
                    return Util.Base64.DecodeUrlSafeBase64(remarks_base64);
                }
                catch (FormatException)
                {
                    var old = remarks_base64;
                    remarks = remarks_base64;
                    return old;
                }
            }
            set
            {
                remarks_base64 = Util.Base64.EncodeUrlSafeBase64(value);
            }
        }

        public string FriendlyName()
        {
            if (string.IsNullOrEmpty(server))
            {
                return I18N.GetString("New server");
            }
            if (string.IsNullOrEmpty(remarks_base64))
            {
                if (server.IndexOf(':') >= 0)
                {
                    return "[" + server + "]:" + server_port;
                }
                else
                {
                    return server + ":" + server_port;
                }
            }
            else
            {
                if (server.IndexOf(':') >= 0)
                {
                    return remarks + " ([" + server + "]:" + server_port + ")";
                }
                else
                {
                    return remarks + " (" + server + ":" + server_port + ")";
                }
            }
        }

        public string HiddenName(bool hide = true)
        {
            if (string.IsNullOrEmpty(server))
            {
                return I18N.GetString("New server");
            }
            string server_alter_name = server;
            if (hide)
            {
                server_alter_name = Util.ServerName.HideServerAddr(server);
            }
            if (string.IsNullOrEmpty(remarks_base64))
            {
                if (server.IndexOf(':') >= 0)
                {
                    return "[" + server_alter_name + "]:" + server_port;
                }
                else
                {
                    return server_alter_name + ":" + server_port;
                }
            }
            else
            {
                if (server.IndexOf(':') >= 0)
                {
                    return remarks + " ([" + server_alter_name + "]:" + server_port + ")";
                }
                else
                {
                    return remarks + " (" + server_alter_name + ":" + server_port + ")";
                }
            }
        }

        public void CopyBaseFrom(Server s)
        {
            id = s.id;
            server = s.server;
            server_port = s.server_port;
            server_udp_port = s.server_udp_port;
            password = s.password;
            method = s.method;
            protocol = s.protocol;
            protocolparam = s.protocolparam;
            obfs = s.obfs;
            obfsparam = s.obfsparam;
            remarks_base64 = s.remarks_base64;
            group = s.group;
            enable = s.enable;
            udp_over_tcp = s.udp_over_tcp;
            latency = s.latency;
        }
        public Server CloneBase(bool newserver=false)
        {
            Server ret = new Server();
            if (!newserver)
                ret.id = id;
            ret.server = server;
            ret.server_port = server_port;
            ret.server_udp_port = server_udp_port;
            ret.password = password;
            ret.method = method;
            ret.protocol = protocol;
            ret.protocolparam = protocolparam ?? "";
            ret.obfs = obfs;
            ret.obfsparam = obfsparam ?? "";
            ret.remarks_base64 = remarks_base64;
            ret.group = group;
            if (!newserver)
                ret.enable = enable;
            else
                ret.enable = true;
            ret.udp_over_tcp = udp_over_tcp;
            if (!newserver)
                ret.latency = latency;
            else
                ret.latency = -1;
            return ret;
        }

        public Server()
        {
            server = "server host";
            server_port = 8388;
            method = "aes-256-cfb";
            protocol = "origin";
            protocolparam = "";
            obfs = "plain";
            obfsparam = "";
            password = "0";
            remarks_base64 = "";
            group = "FreeSSR-public";
            udp_over_tcp = false;
            enable = true;
            latency = LATENCY_PENDING;
            byte[] id = new byte[16];
            Util.Utils.RandBytes(id, id.Length);
            this.id = BitConverter.ToString(id).Replace("-", "");
        }

        public Server(string ssURL, string force_group) : this()
        {
            try
            {
                if (ssURL.StartsWith("ss://", StringComparison.OrdinalIgnoreCase))
                {
                    ServerFromSS(ssURL, force_group);
                }
                else if (ssURL.StartsWith("ssr://", StringComparison.OrdinalIgnoreCase))
                {
                    ServerFromSSR(ssURL, force_group);
                }
                else
                {
                    throw new FormatException();
                }
            }
            catch (FormatException)
            {
                enable = false;
                group = "Invalid link format";
            }
        }

        public bool isMatchServer(Server server)
        {
            if (this.server == server.server
                && server_port == server.server_port
                && server_udp_port == server.server_udp_port
                && method == server.method
                && protocol == server.protocol
                && protocolparam == server.protocolparam
                && obfs == server.obfs
                && obfsparam == server.obfsparam
                && password == server.password
                && udp_over_tcp == server.udp_over_tcp
                )
                return true;
            return false;
        }

        private Dictionary<string, string> ParseParam(string param_str)
        {
            Dictionary<string, string> params_dict = new Dictionary<string, string>();
            string[] obfs_params = param_str.Split('&');
            foreach (string p in obfs_params)
            {
                if (p.IndexOf('=') > 0)
                {
                    int index = p.IndexOf('=');
                    string key, val;
                    key = p.Substring(0, index);
                    val = p.Substring(index + 1);
                    params_dict[key] = val;
                }
            }
            return params_dict;
        }

        public void ServerFromSSR(string ssrURL, string force_group)
        {
            // ssr://host:port:protocol:method:obfs:base64pass/?obfsparam=base64&remarks=base64&group=base64&udpport=0&uot=1
            Match ssr = Regex.Match(ssrURL, "ssr://([A-Za-z0-9_-]+)", RegexOptions.IgnoreCase);
            if (!ssr.Success)
                throw new FormatException();

            string data = Util.Base64.DecodeUrlSafeBase64(ssr.Groups[1].Value);
            Dictionary<string, string> params_dict = new Dictionary<string, string>();

            Match match = null;

            int param_start_pos = data.IndexOf("?");
            if (param_start_pos > 0)
            {
                params_dict = ParseParam(data.Substring(param_start_pos + 1));
                data = data.Substring(0, param_start_pos);
            }
            if (data.IndexOf("/") >= 0)
            {
                data = data.Substring(0, data.LastIndexOf("/"));
            }

            Regex UrlFinder = new Regex("^(.+):([^:]+):([^:]*):([^:]+):([^:]*):([^:]+)");
            match = UrlFinder.Match(data);

            if (match == null || !match.Success)
                throw new FormatException();

            server = match.Groups[1].Value;
            server_port = ushort.Parse(match.Groups[2].Value);
            protocol = match.Groups[3].Value.Length == 0 ? "origin" : match.Groups[3].Value;
            protocol = protocol.Replace("_compatible", "");
            method = match.Groups[4].Value;
            obfs = match.Groups[5].Value.Length == 0 ? "plain" : match.Groups[5].Value;
            obfs = obfs.Replace("_compatible", "");
            password = Util.Base64.DecodeStandardSSRUrlSafeBase64(match.Groups[6].Value);

            if (params_dict.ContainsKey("protoparam"))
            {
                protocolparam = Util.Base64.DecodeStandardSSRUrlSafeBase64(params_dict["protoparam"]);
            }
            if (params_dict.ContainsKey("obfsparam"))
            {
                obfsparam = Util.Base64.DecodeStandardSSRUrlSafeBase64(params_dict["obfsparam"]);
            }
            if (params_dict.ContainsKey("remarks"))
            {
                remarks = Util.Base64.DecodeStandardSSRUrlSafeBase64(params_dict["remarks"]);
            }
            if (params_dict.ContainsKey("group"))
            {
                group = Util.Base64.DecodeStandardSSRUrlSafeBase64(params_dict["group"]);
            }
            else
                group = "";
            if (params_dict.ContainsKey("uot"))
            {
                udp_over_tcp = int.Parse(params_dict["uot"]) != 0;
            }
            if (params_dict.ContainsKey("udpport"))
            {
                server_udp_port = ushort.Parse(params_dict["udpport"]);
            }
            if (!String.IsNullOrEmpty(force_group))
                group = force_group;
        }

        public void ServerFromSS(string ssURL, string force_group)
        {
            Server legacyServer = ParseLegacyURL(ssURL, force_group);
            if (legacyServer != null)   //legacy
            {
                protocol = legacyServer.protocol;
                method = legacyServer.method;
                password = legacyServer.password;
                server = legacyServer.server;
                server_port = legacyServer.server_port;
                group = legacyServer.group;
            }
            else
            {
                try
                {
                    Uri parsedUrl;
                    parsedUrl = new Uri(ssURL);
                    //}
                    //catch (UriFormatException)
                    //{
                    //    Logging.Info("UriFormatException");
                    //}
                    //Server server = new Server
                    //{
                    string tmp_remarks = parsedUrl.GetComponents(UriComponents.Fragment, UriFormat.Unescaped);
                    string tmp_server = parsedUrl.IdnHost;
                    ushort tmp_server_port = (ushort)parsedUrl.Port;
                    //};

                    // parse base64 UserInfo
                    string rawUserInfo = parsedUrl.GetComponents(UriComponents.UserInfo, UriFormat.Unescaped);
                    string base64 = rawUserInfo.Replace('-', '+').Replace('_', '/');    // Web-safe base64 to normal base64
                    string userInfo = "";
                    try
                    {
                        userInfo = Encoding.UTF8.GetString(Convert.FromBase64String(
                        base64.PadRight(base64.Length + (4 - base64.Length % 4) % 4, '=')));
                    }
                    catch (FormatException)
                    {
                        Logging.Info("FormatException");
                    }
                    string[] userInfoParts = userInfo.Split(new char[] { ':' }, 2);
                    if (userInfoParts.Length != 2)
                    {
                        Logging.Info("userInfoParts error:Length is not 2!");
                        return;
                    }
                    method = userInfoParts[0];
                    password = userInfoParts[1];

                    remarks = tmp_remarks;
                    server = tmp_server;
                    server_port = tmp_server_port;

                    if (!String.IsNullOrEmpty(force_group))
                        group = force_group;
                    else
                        group = "";
                }
                catch (Exception e)
                {
                    Logging.LogUsefulException(e);
                }
                //NameValueCollection queryParameters = HttpUtility.ParseQueryString(parsedUrl.Query);
                //string[] pluginParts = HttpUtility.UrlDecode(queryParameters["plugin"] ?? "").Split(new[] { ';' }, 2);
                //if (pluginParts.Length > 0)
                //{
                //    server.plugin = pluginParts[0] ?? "";
                //}

                //if (pluginParts.Length > 1)
                //{
                //    server.plugin_opts = pluginParts[1] ?? "";
                //}
            }
            //Regex UrlFinder = new Regex("^(?i)ss://([A-Za-z0-9+-/=_]+)(#(.+))?", RegexOptions.IgnoreCase),
            //    DetailsParser = new Regex("^((?<method>.+):(?<password>.*)@(?<hostname>.+?)" +
            //                          ":(?<port>\\d+?))$", RegexOptions.IgnoreCase);

                //var match = UrlFinder.Match(ssURL);
                //if (!match.Success)
                //    throw new FormatException();

                //var base64 = match.Groups[1].Value;
                //match = DetailsParser.Match(Encoding.UTF8.GetString(Convert.FromBase64String(
                //    base64.PadRight(base64.Length + (4 - base64.Length % 4) % 4, '='))));
                //protocol = "origin";
                //method = match.Groups["method"].Value;
                //password = match.Groups["password"].Value;
                //server = match.Groups["hostname"].Value;
                //server_port = int.Parse(match.Groups["port"].Value);
                //if (!String.IsNullOrEmpty(force_group))
                //    group = force_group;
                //else
                //    group = "";
            }

        private Server ParseLegacyURL(string ssURL, string force_group)
        {
            try
            {
                Regex UrlFinder = new Regex("^(?i)ss://([A-Za-z0-9+-/=_]+)(#(.+))?", RegexOptions.IgnoreCase),
                    DetailsParser = new Regex("^((?<method>.+):(?<password>.*)@(?<hostname>.+?)" +
                                          ":(?<port>\\d+?))$", RegexOptions.IgnoreCase);

                var match = UrlFinder.Match(ssURL);
                if (!match.Success)
                    throw new FormatException();

                Server server = new Server();
                var base64 = match.Groups[1].Value;
                match = DetailsParser.Match(Encoding.UTF8.GetString(Convert.FromBase64String(
                    base64.PadRight(base64.Length + (4 - base64.Length % 4) % 4, '='))));
                server.protocol = "origin";
                server.method = match.Groups["method"].Value;
                server.password = match.Groups["password"].Value;
                server.server = match.Groups["hostname"].Value;
                server.server_port = ushort.Parse(match.Groups["port"].Value);
                if (!String.IsNullOrEmpty(force_group))
                    server.group = force_group;
                else
                    server.group = "";
                return server;
            }
            catch
            {
                return null;
            }
        }

        public string GetSSLinkForServer()
        {
            string parts = method + ":" + password + "@" + server + ":" + server_port;
            string base64 = System.Convert.ToBase64String(Encoding.UTF8.GetBytes(parts)).Replace("=", "");
            return "ss://" + base64;
        }

        public string GetSSRLinkForServer()
        {
            string main_part = server + ":" + server_port + ":" + protocol + ":" + method + ":" + obfs + ":" + Util.Base64.EncodeUrlSafeBase64(password);
            string param_str = "obfsparam=" + Util.Base64.EncodeUrlSafeBase64(obfsparam ?? "");
            if (!string.IsNullOrEmpty(protocolparam))
            {
                param_str += "&protoparam=" + Util.Base64.EncodeUrlSafeBase64(protocolparam);
            }
            if (!string.IsNullOrEmpty(remarks))
            {
                param_str += "&remarks=" + Util.Base64.EncodeUrlSafeBase64(remarks);
            }
            if (!string.IsNullOrEmpty(group))
            {
                param_str += "&group=" + Util.Base64.EncodeUrlSafeBase64(group);
            }
            if (udp_over_tcp)
            {
                param_str += "&uot=" + "1";
            }
            if (server_udp_port > 0)
            {
                param_str += "&udpport=" + server_udp_port.ToString();
            }
            string base64 = Util.Base64.EncodeUrlSafeBase64(main_part + "/?" + param_str);
            return "ssr://" + base64;
        }

        //public bool isEnable()
        //{
        //    return enable;
        //}

        //public void setEnable(bool enable)
        //{
        //    this.enable = enable;
        //}

        public object getObfsData()
        {
            return this.obfsdata;
        }
        public void setObfsData(object data)
        {
            this.obfsdata = data;
        }

        public object getProtocolData()
        {
            return this.protocoldata;
        }
        public void setProtocolData(object data)
        {
            this.protocoldata = data;
        }

        public void tcpingLatency()
        {
            
            double latencies = -1;
            TcpClient sock = null;
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            try
            {
                Dns.GetHostAddresses(server);
            }
            catch (Exception)
            {
                stopwatch.Stop();
                latency = LATENCY_ERROR;
                return;
            }

            if (server.Contains(':'))
                sock = new TcpClient(AddressFamily.InterNetworkV6);
            else
                sock = new TcpClient(AddressFamily.InterNetwork);

            try
            {
                var result = sock.BeginConnect(server, server_port, null, null);
                if (result.AsyncWaitHandle.WaitOne(TimeSpan.FromSeconds(2)))
                {
                    latencies = stopwatch.Elapsed.TotalMilliseconds;
                }
            }
            catch (Exception e)
            {
                Logging.Log(LogLevel.Error, e.Message);
            }
            finally
            {
                stopwatch.Stop();
                sock.Close();
            }

            if (0 < latencies)
            {
                if (latencies < 1)
                    latencies = 1;
                latency = (long)latencies;
                //if (serverSpeedLog.AvgConnectTime != -1)
                //    serverSpeedLog.AddConnectTime(latency, true);
            }
            else
            {
                latency = LATENCY_ERROR;
            }
        }
    }
}
