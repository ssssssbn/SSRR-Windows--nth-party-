using Shadowsocks.Model;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml;
using System.Windows.Forms;

using Shadowsocks;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using System.Diagnostics;
using System.Timers;
using Shadowsocks.Util;

namespace Shadowsocks.Controller
{
    public class UpdateFreeNode
    {
        public const string Name = "ShadowsocksR";
        private const string User_Agent = @"Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/74.0.3729.131 Safari/537.36";

        public event EventHandler NewFreeNodeFound;
        private bool use_proxy;
        public bool notify;

        private Configuration config;
        private const int mwcMaxNum = 5;
        private MyWebClient[] mwc;
        private const double timeout = 5;
#if DEBUG
        public double[] timerarray = new double[mwcMaxNum];
#endif
        public bool InProcessing;
        public bool Interrupt;
        public int mwcRunningCount = 0;
        public List<ServerSubscribe> listServerSubscribes;
        public List<ServerSubscribe> listServerSubscribes_backup;
        public List<string> listRepeated;
        public List<string> listFailed;
        public Mutex mtListControl = new Mutex();
        public Mutex mtmwcRunningCheck = new Mutex();
        private bool AllFailed = true;
        public int RenewNodeCount = 0;

        public void CheckUpdate(Configuration _config, bool _use_proxy, bool _notify)
        {
            try
            {
                listServerSubscribes_backup = new List<ServerSubscribe>();
                foreach (ServerSubscribe ss in listServerSubscribes)
                {
                    listServerSubscribes_backup.Add(ss);
                }


                config = _config;
                use_proxy = _use_proxy;
                notify = _notify;

                if (mwc == null)
                    mwc = new MyWebClient[mwcMaxNum];
#if DEBUG
                Program.sw.Restart();
#endif
                mtListControl.WaitOne();
                int taskcount = listServerSubscribes.Count;
                if (taskcount > mwcMaxNum)
                    taskcount = mwcMaxNum;
                mtListControl.ReleaseMutex();

                Logging.Log(LogLevel.Info, "Update server subscribes start.");
                for (int i = 0; i < taskcount; i++)
                {
                    if (Interrupt)
                        return;

                    int mwcindex = -1;
                    for (int newmwcindex = 0; newmwcindex < mwcMaxNum; newmwcindex++)
                    {
                        if (mwc[newmwcindex] == null || mwc[newmwcindex].free)
                        {
                            mwcindex = newmwcindex;
                            if (mwc[mwcindex] == null)
                            {
                                mwc[mwcindex] = new MyWebClient(1000.0 * timeout);
                                mwc[mwcindex].mwcindex = mwcindex;
                                mwc[mwcindex].DownloadStringCompleted += mwc_DownloadStringCompleted;
                            }
                            break;
                        }
                    }
                    mtListControl.WaitOne();
                    if (mwcindex == -1 || listServerSubscribes.Count == 0)
                    {
                        mtListControl.ReleaseMutex();
                        return;
                    }
                    mtListControl.ReleaseMutex();

                    mwcRunningCount++;
                    RenewNodeCount++;
                    Logging.Debug(String.Format("mwcRunningCount {0}  RenewNodeCount {1}", mwcRunningCount, RenewNodeCount));
                    mwc[mwcindex].free = false;
                    Thread th = new Thread(new ParameterizedThreadStart(thStartDownloadString));
                    th.Start(new object[] { mwcindex });

                }


            }
            catch (Exception e)
            {
                Logging.LogUsefulException(e);
            }
        }

        private void thStartDownloadString(object _obj)
        {
            int mwcindex = (int)((object[])_obj)[0];
            if (Interrupt)
            {
                mwc[mwcindex].Dispose();
                mwc[mwcindex] = null;
                mtmwcRunningCheck.WaitOne(-1);
                mwcRunningCount--;
                RenewNodeCount--;
                Logging.Debug(String.Format("interrupt mwcRunningCount {0}  RenewNodeCount {1}", mwcRunningCount, RenewNodeCount));
                if (mwcRunningCount == 0 && RenewNodeCount == 0)
                    InProcessing = false;
                mtmwcRunningCheck.ReleaseMutex();
                return;
            }

            mwc[mwcindex].Headers.Add("User-Agent", String.IsNullOrEmpty(config.proxyUserAgent) ? User_Agent : config.proxyUserAgent);
            mwc[mwcindex].QueryString["rnd"] = Util.Utils.RandUInt32().ToString();

            mtListControl.WaitOne();
            if (listServerSubscribes.Count == 0)
            {
                mtmwcRunningCheck.WaitOne();
                mwcRunningCount--;
                RenewNodeCount--;
                Logging.Debug(String.Format("mwcRunningCount {0}  RenewNodeCount {1}", mwcRunningCount, RenewNodeCount));
                if (mwcRunningCount == 0)
                {
                    Thread th = new Thread(new ParameterizedThreadStart(thInvokeNewFeedNodeFound));
                    th.Start(new object[] { listServerSubscribes_backup, AllFailed, listRepeated, listFailed });
                }
                mtmwcRunningCheck.ReleaseMutex();
                mtListControl.ReleaseMutex();
                mwc[mwcindex].free = true;
                return;
            }
            mwc[mwcindex].task = listServerSubscribes[0];
            listServerSubscribes.RemoveAt(0);
            mtListControl.ReleaseMutex();


            mwc[mwcindex].Proxy = null;
                if (mwc[mwcindex].task.DontUseProxy)
                {
                    ;
                }
                else if (use_proxy || mwc[mwcindex].task.UseProxy)
                {
                    WebProxy proxy = new WebProxy(IPAddress.Loopback.ToString(), config.localPort);
                    if (!string.IsNullOrEmpty(config.authPass))
                    {
                        proxy.Credentials = new NetworkCredential(config.authUser, config.authPass);
                    }
                    mwc[mwcindex].Proxy = proxy;
                }
#if DEBUG
            timerarray[mwcindex] = Program.sw.Elapsed.TotalMilliseconds;
#endif
            mwc[mwcindex].timer.Start();
            mwc[mwcindex].DownloadStringAsync(new Uri(mwc[mwcindex].task.URL));
            Logging.Debug(String.Format("Subscribe {0} on MyWebClient {1} {2}using proxy start",
                mwc[mwcindex].task.index + 1,
                mwcindex + 1,
                mwc[mwcindex].task.DontUseProxy ? "not " : mwc[mwcindex].task.UseProxy ? "" : use_proxy ? "" : "not "));

        }

        private void mwc_DownloadStringCompleted(object sender,DownloadStringCompletedEventArgs e)
        {
            MyWebClient tmpmwc = (MyWebClient)sender;
            mwc[tmpmwc.mwcindex].timer.Stop();
            mwc[tmpmwc.mwcindex].timer.Interval = 1000.0 * timeout;

            if (Interrupt)
            {
                mwc[tmpmwc.mwcindex].Dispose();
                mwc[tmpmwc.mwcindex] = null;
                mtmwcRunningCheck.WaitOne(-1);
                mwcRunningCount--;
                RenewNodeCount--;
                Logging.Debug(String.Format("mwcRunningCount {0}  RenewNodeCount {1}", mwcRunningCount, RenewNodeCount));
                if (mwcRunningCount == 0 && RenewNodeCount == 0)
                    InProcessing = false;
                mtmwcRunningCheck.ReleaseMutex();
                return;
            }

            Thread th = null;
            bool status = false;
            StringBuilder sb = new StringBuilder();
            try
            {
                sb.Append(e.Result);
                status = true;
                AllFailed = false;
                RenewNodeCount++;
                Logging.Debug(String.Format("mwcRunningCount {0}  RenewNodeCount {1}", mwcRunningCount, RenewNodeCount));
                th = new Thread(new ParameterizedThreadStart(thInvokeNewFeedNodeFound));
                th.Start(new object[] { sb.ToString(), tmpmwc.task, status });

            }
            catch (Exception ex)
            {
                if (ex.Message.Contains("cancel"))
                {
                    mwcRunningCount--;
                    RenewNodeCount--;
                    Logging.Debug(String.Format("mwcRunningCount {0}  RenewNodeCount {1}", mwcRunningCount, RenewNodeCount));
                    return;
                }
#if DEBUG
                Logging.Debug(String.Format("Subscribe {0} on MyWebClient {1} {2}using proxy error in {3} s" + System.Environment.NewLine + "{4}",
                    tmpmwc.task.index + 1,
                    tmpmwc.mwcindex + 1,
                    tmpmwc.task.DontUseProxy ? "not " : tmpmwc.task.UseProxy ? "" : use_proxy ? "" : "not ",
                    (Program.sw.Elapsed.TotalMilliseconds - timerarray[tmpmwc.mwcindex]) / 1000.0,
                    ex.Message.Contains("Check InnerException for exception details.") ? ex.InnerException.Message : ex.Message));
#endif
                if (!tmpmwc.task.DontUseProxy && !tmpmwc.task.UseProxy && config.nodeFeedAutoUpdateTryUseProxy && !use_proxy)
                {
                    tmpmwc.task.UseProxy = true;
                    mtListControl.WaitOne();
                    listServerSubscribes.Insert(0, tmpmwc.task);
                    mtListControl.ReleaseMutex();

                    return;
                }
                if (listFailed != null)
                    listFailed.Add(tmpmwc.task.id);
                RenewNodeCount++;
                Logging.Debug(String.Format("mwcRunningCount {0}  RenewNodeCount {1}", mwcRunningCount, RenewNodeCount));
                if (ex.Message.Contains("Check InnerException for exception details.") && ex.InnerException.Message == "The request was aborted: The request was canceled.")
                    Logging.Log(LogLevel.Info, String.Format(I18N.GetString("Update subscribe {0} timeout"), tmpmwc.task.index + 1));

                th = new Thread(new ParameterizedThreadStart(thInvokeNewFeedNodeFound));
                th.Start(new object[] { sb.ToString(), tmpmwc.task, status });
            }
            finally
            {
#if DEBUG
                Logging.Debug(String.Format("Subscribe {0} on MyWebClient {1} {2}using proxy completed in {3} s",
                    tmpmwc.task.index + 1,
                    tmpmwc.mwcindex + 1,
                    tmpmwc.task.DontUseProxy ? "not " : tmpmwc.task.UseProxy ? "" : use_proxy ? "" : "not ",
                    (Program.sw.Elapsed.TotalMilliseconds - timerarray[tmpmwc.mwcindex]) / 1000.0));
                timerarray[tmpmwc.mwcindex] = 0;
#endif
                th = new Thread(new ParameterizedThreadStart(thStartDownloadString));
                th.Start(new object[] { tmpmwc.mwcindex });

            }
        }

        private void thInvokeNewFeedNodeFound(object _obj)
        {
            NewFreeNodeFound?.Invoke(_obj, new EventArgs());
        }

        public void Stop()
        {
            Logging.Debug("updater stopped start");
            if (mwc != null)
            {
                for (int i = 0; i < mwcMaxNum; i++)
                {
                    if (mwc[i] != null && mwc[i].IsBusy)
                    {
                        mwc[i].CancelAsync();
                        mwc[i].Dispose();
                        mwc[i] = null;
                    }
                }
                while (InProcessing) ;
                mwc = null;
            }
            if (listServerSubscribes != null)
            {
                listServerSubscribes.Clear();
                listServerSubscribes = null;
            }
            if (listServerSubscribes_backup != null)
            {
                listServerSubscribes_backup.Clear();
                listServerSubscribes_backup = null;
            }
            if (listRepeated != null)
            {
                listRepeated.Clear();
                listRepeated = null;
            }
            if (listFailed != null)
            {
                listFailed.Clear();
                listFailed = null;
            }
            mtListControl = new Mutex();
            mtmwcRunningCheck = new Mutex();
            Logging.Debug("updater stopped end");
        }

        public void Dispose()
        {
            Logging.Debug("updater dispose start");
            Stop();
            if(mtListControl!=null)
            {
                mtListControl.Dispose();
                mtListControl = null;
            }
            if(mtmwcRunningCheck != null)
            {
                mtmwcRunningCheck.Dispose();
                mtmwcRunningCheck = null;
            }
            Logging.Debug("updater dispose end");
        }
    }

    public class UpdateSubscribeManager
    {
        private UpdateFreeNode updater;
        public bool use_proxy;
        public bool notify;

        public static List<string> listSubscribeFailureLinks = new List<string>();
        public bool InProcessing { get { return updater.InProcessing; } set { updater.InProcessing = value; } }
        public bool Interrupt  { get { return updater.Interrupt; }set { updater.Interrupt = value; } }
        public bool UpdateAll = false;

        public UpdateSubscribeManager(UpdateFreeNode _updater)
        {
            updater = _updater;
        }
        public void CreateTask(Configuration config, int[] indexes, bool _use_proxy, bool _notify)
        {
            if (!updater.InProcessing)
            {
                updater.InProcessing = true;


                List<ServerSubscribe> serverSubscribes = new List<ServerSubscribe>();
                List<string> listRepeated = new List<string>();
                List<string> listFailed = new List<string>();

                use_proxy = _use_proxy;
                notify = _notify;
                UpdateAll = false;
                if (indexes == null || indexes.Length == 0)
                {
                    UpdateAll = true;
                    listSubscribeFailureLinks.Clear();
                    int ServerSubscribeCount = config.serverSubscribes.Count;
                    for (int i = 0; i < ServerSubscribeCount; ++i)
                    {
                        if (!config.serverSubscribes[i].JoinUpdate)
                            continue;
                        serverSubscribes.Add(config.serverSubscribes[i].Clone());
                        serverSubscribes[serverSubscribes.Count - 1].index = i;
                    }
                }
                else
                {
                    for (int i = 0; i < indexes.Length; i++)
                    {
                        serverSubscribes.Add(config.serverSubscribes[indexes[i]].Clone());
                        serverSubscribes[serverSubscribes.Count - 1].index= indexes[i];
                    }
                }

                for (int i = 0; i < serverSubscribes.Count; )
                {
                    if (serverSubscribes[i].URL == "")
                    {
                        if (!listFailed.Contains(serverSubscribes[i].id))
                            listFailed.Add(serverSubscribes[i].id);
                        serverSubscribes.RemoveAt(i);
                        continue;
                    }
                    if (Utils.IsServerSubscriptionRepeat(serverSubscribes, serverSubscribes[i].URL).Count > 0) 
                    {
                        if (!listRepeated.Contains(serverSubscribes[i].id))
                            listRepeated.Add(serverSubscribes[i].id);
                        serverSubscribes.RemoveAt(i);
                        continue;
                    }
                    i++;
                }

                if (serverSubscribes.Count == 0)
                {
                    updater.InProcessing = false;
                    return;
                }


                updater.mtListControl.WaitOne();
                updater.listServerSubscribes = serverSubscribes;
                updater.listRepeated = listRepeated;
                updater.listFailed = listFailed;
                updater.mtListControl.ReleaseMutex();
                updater.CheckUpdate(config, use_proxy, notify);
            }
        }
        
        public void InterruptUpdateFreeNode()
        {
            Interrupt = true;
            updater.Stop();
            Interrupt = false;
        }

        public void Dispose()
        {
            Interrupt = true;
            updater.Dispose();
        }
    }
    
}
