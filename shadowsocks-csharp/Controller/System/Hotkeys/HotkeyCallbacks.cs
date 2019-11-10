using System;
using System.Reflection;
using Shadowsocks.Model;
using Shadowsocks.View;
using Shadowsocks;

namespace Shadowsocks.Controller.Hotkeys
{
    public class HotkeyCallbacks
    {
        private Configuration _config;
        private readonly MenuViewController _viewController;

        private System.Timers.Timer timerTriggerLimit;

        public static void InitInstance()
        {
            if (Instance != null)
            {
                return;
            }

            Instance = new HotkeyCallbacks();
        }

        public static void Destroy()
        {
            if (Instance == null)
                return;
            Instance = new HotkeyCallbacks(false);
            Instance = null;
        }

        /// <summary>
        /// Create hotkey callback handler delegate based on callback name
        /// </summary>
        /// <param name="methodname"></param>
        /// <returns></returns>
        public static Delegate GetCallback(string methodname)
        {
            if (methodname.IsNullOrEmpty()) throw new ArgumentException(nameof(methodname));
            MethodInfo dynMethod = typeof(HotkeyCallbacks).GetMethod(methodname,
                BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.IgnoreCase);
            return dynMethod == null ? null : Delegate.CreateDelegate(typeof(HotKeys.HotKeyCallBackHandler), Instance, dynMethod);
        }

        #region Singleton 
        
        private static HotkeyCallbacks Instance { get; set; }

        private readonly ShadowsocksController _controller;

        private HotkeyCallbacks(bool enable = true)
        {
            if (enable)
            {
                _controller = Program.GetController();
                _viewController = Program.GetMenuViewController();
                if (timerTriggerLimit == null)
                {
                    timerTriggerLimit = new System.Timers.Timer(150.0);
                    timerTriggerLimit.AutoReset = false;
                }

            }
            else
            {
                _controller = null;
                _viewController = null;
                if (timerTriggerLimit != null)
                {
                    timerTriggerLimit.Stop();
                    timerTriggerLimit.Dispose();
                    timerTriggerLimit = null;
                }
            }
        }



        #endregion

        #region Callbacks

        private void SwitchProxyModeCallback()
        {
            if (timerTriggerLimit.Enabled)
            {
                return;
            }
            timerTriggerLimit.Start();

            _config = _controller.GetCurrentConfiguration();// Configuration.Load();
            //var config = _controller.GetConfiguration();
            switch (_config.sysProxyMode)
            {
                case (int)ProxyMode.Direct:
                    _controller.ToggleMode(ProxyMode.Pac);
                    break;
                case (int)ProxyMode.Pac:
                    _controller.ToggleMode(ProxyMode.Global);
                    break;
                case (int)ProxyMode.Global:
                    _controller.ToggleMode(ProxyMode.Direct);
                    break;
                case (int)ProxyMode.NoModify:
                    _controller.ToggleMode(ProxyMode.Direct);
                    break;
                default:
                    break;
            }
            _viewController.ShownotifyIcontext();

        }

        private void SwitchLoadBalanceCallback()
        {
            if (timerTriggerLimit.Enabled)
                return;
            timerTriggerLimit.Start();

            _config = _controller.GetCurrentConfiguration(); // Configuration.Load();
            _controller.ToggleEnableBalance(!_config.enableBalance);
            _viewController.ShownotifyIcontext();

            //bool enabled = _controller.GetConfiguration().enabled;
            //_controller.ToggleMode(ProxyMode.Direct);
            //_controller.ToggleEnable(!enabled);
            
        }

        private void SwitchAllowLanCallback()
        {
            if (timerTriggerLimit.Enabled)
                return;
            timerTriggerLimit.Start();

            _config = _controller.GetCurrentConfiguration(); //Configuration.Load();
            _controller.ToggleShareOverLAN(!_config.shareOverLan);
            if(!_config.shareOverLan)
                _viewController.ShowTextByNotifyIconBalloon(I18N.GetString("Tips"),I18N.GetString("Share Over LAN")+":"+I18N.GetString("On"),System.Windows.Forms.ToolTipIcon.Info);
            else
                _viewController.ShowTextByNotifyIconBalloon(I18N.GetString("Tips"), I18N.GetString("Share Over LAN") + ":" + I18N.GetString("Off"), System.Windows.Forms.ToolTipIcon.Info);
            
        }

        private void ClipboardAndQRCodeScanningCallback()
        {
            if (timerTriggerLimit.Enabled)
                return;
            timerTriggerLimit.Start();

            _viewController.CallClipboardAndQRCodeScanning_HotKey();
            
        }

        //private void ShowLogsCallback()
        //{
        //    //Program._viewController.ShowLogForm_HotKey();
        //}

        private void ServerMoveUpCallback()
        {
            if (timerTriggerLimit.Enabled)
                return;
            timerTriggerLimit.Start();

            int currIndex;
            int serverCount;
            GetCurrServerInfo(out currIndex, out serverCount);
            if (currIndex - 1 < 0)
            {
                // revert to last server
                currIndex = serverCount - 1;
            }
            else
            {
                currIndex -= 1;
            }
            _controller.SelectServerIndex(currIndex);
            _viewController.ShownotifyIcontext();
            
        }

        private void ServerMoveDownCallback()
        {
            if (timerTriggerLimit.Enabled)
                return;
            timerTriggerLimit.Start();

            int currIndex;
            int serverCount;
            GetCurrServerInfo(out currIndex, out serverCount);
            if (currIndex + 1 == serverCount)
            {
                // revert to first server
                currIndex = 0;
            }
            else
            {
                currIndex += 1;
            }
            _controller.SelectServerIndex(currIndex);
            _viewController.ShownotifyIcontext();
            
        }

        private void GetCurrServerInfo(out int currIndex, out int serverCount)
        {
            var currConfig = _controller.GetCurrentConfiguration();
            currIndex = currConfig.index;
            serverCount = currConfig.Servers.Count;
        }

        #endregion
    }
}
