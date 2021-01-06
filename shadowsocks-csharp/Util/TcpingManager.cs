using Shadowsocks.Controller;
using Shadowsocks.Model;
using Shadowsocks.View;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Shadowsocks.Util
{
    public static class TCPingManager
    {
        private static ShadowsocksController controller;
        private static List<string> listTCPingList1;
        private static List<List<string>> listTCPingList2;
        private static List<List<string>> listTCPingList_backup;
        private static List<string> listInTCPing;
        public static List<string> listTerminator;
        private static Mutex mtListControl;
        private static Mutex mtThreadAliveCheck;

        private const int thLatencyTestMaxNum = 128;
        private static Thread[] thLatencyTest;

        public static int ThreadAliveCount = 0;
        public static bool Enabled = true;
        public static bool InProcessing = false;
        private static bool interrupt = false;
        public static bool Interrupt { 
            get { 
                return interrupt; 
            }
            set {
                bool tmp = value;
                if (!interrupt && tmp)
                {
                    Logging.Log(LogLevel.Info, "Update latency interrupted.");
                }
                else if(interrupt && !tmp)
                {
                    Logging.Log(LogLevel.Info, "Update latency continued.");
                }
                interrupt = tmp;
            } }

        public static void Init(ShadowsocksController _controller)
        {
            if (controller == null)
                controller = _controller;
            if (mtListControl == null)
                mtListControl = new Mutex(true);
            if (mtThreadAliveCheck == null)
                mtThreadAliveCheck = new Mutex();
            if (thLatencyTest == null)
                thLatencyTest = new Thread[thLatencyTestMaxNum];
            if (listTCPingList1 == null)
                listTCPingList1 = new List<string>();
            if (listTCPingList2 == null)
                listTCPingList2 = new List<List<string>>();
            if (listTCPingList_backup == null)
                listTCPingList_backup = new List<List<string>>();
            if (listInTCPing == null)
                listInTCPing = new List<string>();
        }

        public static void StartTcping(ShadowsocksController _controller, List<string> list)
        {
            if (list==null || list.Count <= 0)
                return;
            if (mtListControl != null)
                mtListControl.WaitOne(-1);
            Init(_controller);
            if (listTCPingList1.Count == 0 && listInTCPing.Count == 0) 
            {
                Logging.Debug("Update latency started.");
            }

            listTCPingList2.Add(new List<string>());
            listTCPingList_backup.Add(new List<string>());
            foreach (string str in list)
            {
                listTCPingList1.Add(str);
                listTCPingList2[listTCPingList2.Count-1].Add(str);
                listTCPingList_backup[listTCPingList_backup.Count - 1].Add(str);
            }
            mtListControl.ReleaseMutex();

            Thread thStart = new Thread(new ParameterizedThreadStart(Start));
            thStart.Start(new object[] { false });
        }

        public static void StartTcping(ShadowsocksController _controller, string str)
        {
            if (mtListControl != null)
                mtListControl.WaitOne(-1);
            Init(_controller); 
            if (listTCPingList1.Count == 0 && listInTCPing.Count == 0) 
            {
                Logging.Debug("Update latency started.");
            }

            listTCPingList2.Add(new List<string>());
            listTCPingList_backup.Add(new List<string>());
            listTCPingList1.Add(str);
            listTCPingList2[listTCPingList2.Count - 1].Add(str);
            listTCPingList_backup[listTCPingList_backup.Count - 1].Add(str);
            mtListControl.ReleaseMutex();

            Thread thStart = new Thread(new ParameterizedThreadStart(Start));
            thStart.Start(new object[] { false });
        }

        private static void Start(object IsRestore)
        {
            try
            {
                mtThreadAliveCheck.WaitOne();
                ThreadAliveCount++;
                mtThreadAliveCheck.ReleaseMutex();
                if (!Enabled)
                {
                    Interrupt = true;
                    //Logging.Log(LogLevel.Info, "Update latency interrupted.");
                    return;
                }
                InProcessing = true;

                mtListControl.WaitOne(-1);
                if ((bool)((object[])IsRestore)[0])
                {
                    listTCPingList1.Clear();
                    listTCPingList2.Clear();
                    listInTCPing.Clear();
                    foreach (List<string> liststr in listTCPingList_backup)
                    {
                        listTCPingList2.Add(new List<string>());
                        foreach(string str in liststr)
                        {
                            listTCPingList1.Add(str);
                            listTCPingList2[listTCPingList2.Count - 1].Add(str);
                        }
                    }
                }
                mtListControl.ReleaseMutex();


                mtListControl.WaitOne(-1);
                int thcount = listTCPingList1.Count;
                if (thcount > thLatencyTestMaxNum)
                    thcount = thLatencyTestMaxNum;
                mtListControl.ReleaseMutex();
                bool thAllBusy = false;
                for (int i = 0; i < thcount; i++) 
                {
                    if (Interrupt)
                    {
                        return;
                    }
                    if (thAllBusy)
                        return;

                    int thindex = -1;
                    for (int newthindex = 0; newthindex < thLatencyTestMaxNum; newthindex++)
                    {
                        if (Interrupt)
                            return;

                        if (thLatencyTest[newthindex] == null) 
                        {
                            thindex = newthindex;
                            break;
                        }
                        if (newthindex == thLatencyTestMaxNum - 1)
                            thAllBusy = true;
                    }
                    mtListControl.WaitOne(-1);
                    if (thindex == -1 || listTCPingList1.Count == 0)
                    {
                        mtListControl.ReleaseMutex();
                        return;
                    }
                    mtListControl.ReleaseMutex();

                    thLatencyTest[thindex] = new Thread(new ParameterizedThreadStart(TcpingLatencytest));
                    thLatencyTest[thindex].Start(new object[] { thindex });

                }
            }
            catch (Exception e)
            {
                if (e.Message != "Thread was being aborted.")
                    Logging.Log(LogLevel.Error, e.Message);
            }
            finally
            {
                mtThreadAliveCheck.WaitOne();
                ThreadAliveCount--;
                //Logging.Debug(String.Format("thread alive count {0}", ThreadAliveCount));
                if (ThreadAliveCount == 0)
                    InProcessing = false;
                mtThreadAliveCheck.ReleaseMutex();
            }
        }

        private static void TcpingLatencytest(object _obj)
        {
            object[] obj = (object[])_obj;
            int thindex = (int)obj[0];
            string ServerID = null;
            int index = -1;

            ThreadAliveCount++;
            try
            {
                //Logging.Debug(String.Format("thread alive count {0}", ThreadAliveCount));
                mtListControl.WaitOne(-1);
                while (true)
                {
                    if (listTCPingList1.Count == 0) 
                    {
                        break;
                    }
                    bool end = false;
                    for (int i = 0; i < listTCPingList1.Count; i++)
                    {
                        ServerID = listTCPingList1[i];
                        if (listInTCPing.FindIndex(t => t == ServerID) == -1)
                        {
                            listInTCPing.Add(ServerID);
                            listTCPingList1.Remove(ServerID);
                            break;
                        }
                        if (i == listTCPingList1.Count - 1)
                        {
                            end = true;
                            break;
                        }
                    }
                    if (end)
                        break;

                    List<Server> AllServers = controller.GetCurrentConfiguration().Servers;
                    index = AllServers.FindIndex(t => t.id == ServerID);
                    if (index != -1) 
                    {
                        mtListControl.ReleaseMutex();
                        AllServers[index].tcpingLatency();
                        mtListControl.WaitOne(-1);
                    }

                    for (int i = 0; i < listTCPingList2.Count; i++)
                    {
                        if (listTCPingList2[i].Remove(ServerID))
                        {
                            if (listTCPingList2[i].Count == 0)
                            {
                                listTCPingList2.RemoveAt(i);
                                listTCPingList_backup.RemoveAt(i);
                            }
                            break;
                        }
                    }
                    listInTCPing.Remove(ServerID);
                    ServerID = null;


                    if (Interrupt)
                    {
                        mtListControl.ReleaseMutex();
                        return;
                    }
                    //Logging.Debug(String.Format("Server {0} completed test  on Thread {1}", index, thindex));

                }

            }
            catch (Exception e)
            {
                if (e.Message == "Thread was being aborted.")
                {
                    return;
                }
                else
                {
                    Logging.Log(LogLevel.Error, String.Format("Server {0}" + System.Environment.NewLine + "{1}", !String.IsNullOrEmpty(ServerID) ? controller.GetCurrentConfiguration().Servers.FindIndex(t => t.id == ServerID) : -1, e.Message));

                }
            }
            finally
            {
                mtThreadAliveCheck.WaitOne();
                ThreadAliveCount--;
//Logging.Debug(String.Format("thread alive count {0}", ThreadAliveCount));
                if (ThreadAliveCount == 0)
                    InProcessing = false;
                mtThreadAliveCheck.ReleaseMutex();

                if (thLatencyTest!=null)
                    thLatencyTest[thindex]=null;
            }
            mtThreadAliveCheck.WaitOne(-1);
//            //ThreadAliveCount--;
//            Logging.Debug(String.Format("thread alive count {0}", ThreadAliveCount));
            if (ThreadAliveCount > 0)
            {
                mtThreadAliveCheck.ReleaseMutex();
                mtListControl.ReleaseMutex();
                return;
            }
            mtThreadAliveCheck.ReleaseMutex();

            if (Interrupt)
            {
                mtListControl.ReleaseMutex();
                mtThreadAliveCheck.WaitOne();
                if (ThreadAliveCount == 0)
                    InProcessing = false;
                mtThreadAliveCheck.ReleaseMutex();
                return;
            }

            if (listTCPingList1.Count == 0 && listInTCPing.Count == 0/*)*/ && Enabled && !Interrupt)
            {
                Logging.Debug("Update latency succeed.");
            }
            thLatencyTest = null;
            controller.SaveTimerUpdateLatency();
            Utils.ReleaseMemory(true);
            InProcessing = false;
            mtListControl.ReleaseMutex();

        }

        public static void StopTcping(string Terminator)
        {
            if (listTerminator == null)
                listTerminator = new List<string>();
            listTerminator.Add(Terminator);
            StringBuilder sb = new StringBuilder();
            foreach (string str in listTerminator)
                sb.Append((sb.Length == 0 ? "" : ",") + str);
            Logging.Debug(String.Format("Terminator :{0},{1}", listTerminator.Count, sb.ToString()));
            if (listTerminator.Count == 1 && InProcessing)
            {
                Enabled = false;
                Interrupt = true;
                if (Terminator != null)
                    Logging.Log(LogLevel.Info, String.Format("Update latency interrupted by {0} .", Terminator));
                if (thLatencyTest != null)
                    for (int i = 0; i < thLatencyTest.Length; i++)
                    {
                        if (thLatencyTest[i] != null && thLatencyTest[i].ThreadState == System.Threading.ThreadState.Running)
                            thLatencyTest[i].Abort();
                    }
            }
        }

        public static void RestoreTcping(string Restorer)
        {
            if (listTerminator != null)
            {
                listTerminator.Remove(Restorer);
                StringBuilder sb = new StringBuilder();
                if (listTerminator.Count != 0)
                    foreach (string str in listTerminator)
                        sb.Append((sb.Length == 0 ? "" : ",") + str);
                Logging.Debug(String.Format("Terminator :{0},{1}", listTerminator.Count, sb.Length == 0 ? "null" : sb.ToString()));
            }
            if ((listTerminator == null || listTerminator.Count == 0) && Interrupt) 
            {
                Enabled = true;
                Interrupt = false;
                Logging.Log(LogLevel.Info, String.Format("Update latency continued by {0} .", Restorer));
                Thread thStart = new Thread(new ParameterizedThreadStart(Start));
                thStart.Start(new object[] { true });
            }
        }

        public static void Dispose()
        {
            StopTcping("Dispose Self");
            while (InProcessing) ;
            if (listTCPingList1 != null)
            {
                listTCPingList1.Clear();
                listTCPingList1 = null;
            }
            if (listTCPingList2 != null)
            {
                for(int i = 0; i < listTCPingList2.Count; i++)
                {
                    if (listTCPingList2[i] != null)
                    {
                        listTCPingList2[i].Clear();
                    }
                }
                listTCPingList2.Clear();
                listTCPingList2 = null;
            }
            if (listTCPingList_backup != null)
            {
                for (int i = 0; i < listTCPingList_backup.Count; i++)
                {
                    if (listTCPingList_backup[i] != null)
                    {
                        listTCPingList_backup[i].Clear();
                    }
                }
                listTCPingList_backup.Clear();
                listTCPingList_backup = null;
            }
            if (listInTCPing != null)
            {
                listInTCPing.Clear();
                listInTCPing = null;
            }
            if(listTerminator!=null)
            {
                listTerminator.Clear();
                listTerminator = null;
            }
            if (thLatencyTest != null)
            {
                for (int i = 0; i < thLatencyTest.Length; i++)
                {
                    if (thLatencyTest[i] != null && thLatencyTest[i].IsAlive)
                        thLatencyTest[i].Abort();
                    thLatencyTest[i] = null;
                }
                thLatencyTest = null;
            }
            if (mtListControl != null)
            {
                mtListControl.Dispose();
                mtListControl = null;
            }
            if (mtThreadAliveCheck != null)
            {
                mtThreadAliveCheck.Dispose();
                mtThreadAliveCheck = null;
            }
            if (controller != null)
                controller = null;
        }
    }
}
