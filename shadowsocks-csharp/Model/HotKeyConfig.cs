using System;

namespace Shadowsocks.Model
{
    /*
     * Format:
     *  <modifiers-combination>+<key>
     *
     */

    [Serializable]
    public class HotkeyConfig
    {
        public string SwitchProxyMode;
        public string SwitchLoadBalance;
        public string SwitchAllowLan;
        public string CallClipboardAndQRCodeScanning;
        //public string ShowLogs;
        public string ServerMoveUp;
        public string ServerMoveDown;
        public bool RegHotkeysAtStartup;

        public HotkeyConfig()
        {
            SwitchProxyMode = "";
            SwitchLoadBalance = "";
            SwitchAllowLan = "";
            CallClipboardAndQRCodeScanning = "";
            //ShowLogs = "";
            ServerMoveUp = "";
            ServerMoveDown = "";
            RegHotkeysAtStartup = false;
        }
    }
}