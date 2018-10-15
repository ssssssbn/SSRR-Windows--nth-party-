using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using Shadowsocks.Controller.Hotkeys;

using CombinationKeyRecord;
using System.Runtime.InteropServices;

namespace Shadowsocks.View
{
    public partial class AppBarForm : Form
    {
        private IntPtr desktopHandle;
        private IntPtr shellHandle;
        int uCallBackMsg;

        public AppBarForm()
        {
            InitializeComponent();

            RegAppBar(false);
        }

        public void RegAppBar(bool registered)
        {
            APPBARDATA abd = new APPBARDATA();
            abd.cbSize = Marshal.SizeOf(abd);
            abd.hWnd = this.Handle;

            desktopHandle = APIWrapper.GetDesktopWindow();
            shellHandle = APIWrapper.GetShellWindow();
            if (!registered)
            {
                //register
                uCallBackMsg = APIWrapper.RegisterWindowMessage("APPBARMSG_CSDN_HELPER");
                abd.uCallbackMessage = uCallBackMsg;
                uint ret = APIWrapper.SHAppBarMessage((int)ABMsg.ABM_NEW, ref abd);
            }
            else
            {
                APIWrapper.SHAppBarMessage((int)ABMsg.ABM_REMOVE, ref abd);
            }
        }


        //重载窗口消息处理函数
        protected override void WndProc(ref System.Windows.Forms.Message m)
        {
            if (m.Msg == uCallBackMsg)
            {
                switch (m.WParam.ToInt32())
                {
                    case (int)ABNotify.ABN_FULLSCREENAPP:
                        {
                            IntPtr hWnd = APIWrapper.GetForegroundWindow();
                            //判断当前全屏的应用是否是桌面
                            if (hWnd.Equals(desktopHandle) || hWnd.Equals(shellHandle))
                            {
                                if (!HotKeys.IshotKeyManagerRunning)
                                    HotKeys.Init();
                                break;
                            }
                            //判断是否全屏
                            if ((int)m.LParam == 1)
                            {
                                if (HotKeys.IshotKeyManagerRunning)
                                    HotKeys.StophotKeyManager();
                            }
                            else
                            {
                                if (!HotKeys.IshotKeyManagerRunning)
                                    HotKeys.Init();
                            }
                            break;
                        }
                    default:
                        break;
                }
            }
            base.WndProc(ref m);
        }
    }
}
