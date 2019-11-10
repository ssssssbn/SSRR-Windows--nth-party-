using Shadowsocks.Controller;
using Shadowsocks.Util;
using System;
using System.IO;
using System.Threading;
using System.Windows.Forms;
using Microsoft.Win32;
using Shadowsocks.Model;
using System.Net;
using System.Diagnostics;
#if !_CONSOLE
using Shadowsocks.View;
#endif

namespace Shadowsocks
{
    static class Program
    {
        private static ShadowsocksController _controller;
#if !_CONSOLE
        private static MenuViewController _viewController { get; set; }
#endif
        public static bool SystemInFullScreenMode = false;
        //public static short IsSystemTimeCorrectFlag = -1;

#if DEBUG
        public static Stopwatch sw = new Stopwatch();
#endif
        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {

            if (Utils.IsVirusExist())
            {
                return;
            }
#if !_CONSOLE
                foreach (string arg in args)
            {
                if (arg == "--setautorun")
                {
                    if (!AutoStartup.Switch())
                    {
                        Environment.ExitCode = 1;
                    }
                    return;
                }
            }

            using (Mutex mutex = new Mutex(false, "Global\\ShadowsocksR_" + Application.StartupPath.GetHashCode()))
            {
                AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;
                Application.EnableVisualStyles();
                Application.ApplicationExit += Application_ApplicationExit;
                SystemEvents.PowerModeChanged += SystemEvents_PowerModeChanged;
                Application.SetCompatibleTextRenderingDefault(false);
                if (!mutex.WaitOne(0, false))
                {
                    MessageBox.Show(I18N.GetString("Find Shadowsocks icon in your notify tray.") + "\n" +
                        I18N.GetString("If you want to start multiple Shadowsocks, make a copy in another directory."),
                        I18N.GetString("ShadowsocksR is already running."));
                    return;
                }
#endif
                Directory.SetCurrentDirectory(Application.StartupPath);

#if !_CONSOLE
                int try_times = 0;
                while (Configuration.Load() == null)
                {
                    if (try_times >= 5)
                        return;
                    using (InputPassword dlg = new InputPassword())
                    {
                        if (dlg.ShowDialog() == DialogResult.OK)
                            Configuration.SetPassword(dlg.password);
                        else
                            return;
                    }
                    try_times += 1;
                }
                //if (try_times > 0)
                //    Logging.save_to_file = false;
#endif

                Logging.OpenLogFile();

                ServicePointManager.DefaultConnectionLimit = 512;
#if _DOTNET_4_0
                // Enable Modern TLS when .NET 4.5+ installed.
                if (EnvCheck.CheckDotNet45())
                    ServicePointManager.SecurityProtocol |= System.Net.SecurityProtocolType.Tls12;//(SecurityProtocolType)3072;
                if (EnvCheck.CheckDotNet471())
                    ServicePointManager.SecurityProtocol |= SecurityProtocolType.Tls13;
#endif
                ServicePointManager.SecurityProtocol |= SecurityProtocolType.Ssl3 | SecurityProtocolType.Tls;


                _controller = new ShadowsocksController();
                HostMap.Instance().LoadHostFile();

#if !_CONSOLE
                _viewController = new MenuViewController(_controller);
#endif
                _controller.Start();
                SystemEvents.SessionEnding += _viewController.Quit_Click;

#if !_CONSOLE

                foreach (string arg in args)
                {
                    if (arg == "-Direct")
                    {
                        _viewController.DirectItem_Click();
                        break;
                    }
                    else if(arg == "-Pac")
                    {
                        _viewController.PACModeItem_Click();
                        break;
                    }
                    else if(arg == "-Global")
                    {
                        _viewController.GlobalModeItem_Click();
                        break;
                    }
                }

                Application.Run();
            }
#else
            Console.ReadLine();
            _controller.Stop();
#endif
        }

        private static void SystemEvents_PowerModeChanged(object sender, PowerModeChangedEventArgs e)
        {
            switch (e.Mode)
            {
                case PowerModes.Resume:
                    Logging.Info("os wake up");
                    if (_controller != null)
                    {
                        System.Timers.Timer timer = new System.Timers.Timer(1000.0 * 5);
                        timer.Elapsed += Timer_Elapsed;
                        timer.AutoReset = false;
                        timer.Enabled = true;
                        timer.Start();
                    }
                    break;
                case PowerModes.Suspend:
                    if (_controller != null)
                    {
                        _controller.Stop();
                        _viewController.stopTimers();
                        Logging.Info("controller stopped");
                    }
                    Logging.Info("os suspend");
                    break;
            }
        }

        private static void Timer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            try
            {
                if (_controller != null)
                {
                    _controller.Start(false);
                    _viewController.initTimers();
                }
            }
            catch (Exception ex)
            {
                Logging.LogUsefulException(ex);
            }
            finally
            {
                try
                {
                    System.Timers.Timer timer = (System.Timers.Timer)sender;
                    timer.Enabled = false;
                    timer.Stop();
                    timer.Dispose();
                }
                catch (Exception ex)
                {
                    Logging.LogUsefulException(ex);
                }
            }
        }

        private static void Application_ApplicationExit(object sender, EventArgs e)
        {
            if (_controller != null) _controller.Stop();
            _controller = null;
        }

        private static int exited = 0;
        private static void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            if (Interlocked.Increment(ref exited) == 1)
            {
                Logging.Log(LogLevel.Error, e.ExceptionObject != null ? e.ExceptionObject.ToString() : "");
                MessageBox.Show(I18N.GetString("Unexpected error, ShadowsocksR will exit.") +
                    Environment.NewLine + (e.ExceptionObject != null ? e.ExceptionObject.ToString() : ""),
                    "Shadowsocks Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Application.Exit();
            }
        }

        public static ShadowsocksController GetController()
        {
            return _controller;
        }

        public static MenuViewController GetMenuViewController()
        {
            return _viewController;
        }
    }
}
