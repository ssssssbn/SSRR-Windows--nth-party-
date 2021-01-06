using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Shadowsocks.Controller;
using Shadowsocks.Model;
using Shadowsocks.Properties;
using System.Threading;
using System.Runtime.InteropServices;
using Shadowsocks.Util;

namespace Shadowsocks.View
{
    public partial class ServerLogForm : Form
    {

        //通过窗口名称查找窗口句柄
        [DllImport("User32.dll ", EntryPoint = "FindWindow")]
        private static extern IntPtr FindWindow(string lpClassName, string lpWindowName);//关键方法
        //通过窗口句柄判断窗口是否最小化
        [DllImport("user32.dll")]
        public static extern bool IsIconic(IntPtr hWnd);


        private APoint TopLeftPoint = new APoint();

        class DoubleBufferListView : DataGridView
        {
            public DoubleBufferListView()
            {
                SetStyle(ControlStyles.DoubleBuffer
                        | ControlStyles.OptimizedDoubleBuffer
                        | ControlStyles.UserPaint
                        | ControlStyles.AllPaintingInWmPaint
                        , true);
                UpdateStyles();
            }
        }

        private ShadowsocksController controller;
        //private ContextMenu contextMenu1;
        private MenuItem topmostItem;
        //private MenuItem clearItem;
        private List<int> listOrder = new List<int>();
        private int lastRefreshIndex = 0;
        private bool firstDispley = true;
        private bool rowChange = false;
        private int updatePause = 0;
        private int updateTick = 0;
        private int updateSize = 0;
        private int pendingUpdate = 0;
        private string title_perfix = "";
        private ServerSpeedLogShow[] ServerSpeedLogList;
        private Thread workerThread;
        private AutoResetEvent workerEvent = new AutoResetEvent(false);

        private MyTimer mtimerDataGridServerCellClick;

        private const long timeout = 99999;

        public ServerLogForm(ShadowsocksController controller)
        {
            this.controller = controller;
            try
            {
                this.Icon = Icon.FromHandle((new Bitmap("icon.png")).GetHicon());
                title_perfix = System.Windows.Forms.Application.StartupPath;
                if (title_perfix.Length > 20)
                    title_perfix = title_perfix.Substring(0, 20);
            }
            catch
            {
                this.Icon = Icon.FromHandle(Resources.ssw128.GetHicon());
            }
            this.Font = System.Drawing.SystemFonts.MessageBoxFont;
            InitializeComponent();

            ServerDataGrid.MouseWheel += ServerDataGrid_MouseWheel;

            this.Width = 810;
            int dpi_mul = Util.Utils.GetDpiMul();

            Configuration config = controller.GetCurrentConfiguration();
            if (config.Servers.Count < 8)
            {
                this.Height = 300 * dpi_mul / 4;
            }
            else if (config.Servers.Count < 20)
            {
                this.Height = (300 + (config.Servers.Count - 8) * 16) * dpi_mul / 4;
            }
            else
            {
                this.Height = 500 * dpi_mul / 4;
            }
            UpdateTexts();
            UpdateLog();

            this.Menu = new MainMenu(new MenuItem[] {
                CreateMenuGroup("&Control", new MenuItem[] {
                    CreateMenuItem("&Disconnect direct connections", new EventHandler(this.DisconnectForward_Click)),
                    CreateMenuItem("Disconnect &All", new EventHandler(this.Disconnect_Click)),
                    new MenuItem("-"),
                    CreateMenuItem("Clear &MaxSpeed", new EventHandler(this.ClearMaxSpeed_Click)),
                    /*clearItem = */CreateMenuItem("&Clear", new EventHandler(this.ClearItem_Click)),
                    new MenuItem("-"),
                    CreateMenuItem("Clear &Selected Total", new EventHandler(this.ClearSelectedTotal_Click)),
                    CreateMenuItem("Clear &Total", new EventHandler(this.ClearTotal_Click)),
                }),
                CreateMenuGroup("Port &out", new MenuItem[] {
                    CreateMenuItem("Copy current link", new EventHandler(this.copyLinkItem_Click)),
                    CreateMenuItem("Copy current group links", new EventHandler(this.copyGroupLinkItem_Click)),
                    CreateMenuItem("Copy all enable links", new EventHandler(this.copyEnableLinksItem_Click)),
                    CreateMenuItem("Copy all links", new EventHandler(this.copyLinksItem_Click)),
                }),
                CreateMenuGroup("&Window", new MenuItem[] {
                    CreateMenuItem("Auto &size", new EventHandler(this.autosizeItem_Click)),
                    this.topmostItem = CreateMenuItem("Always On &Top", new EventHandler(this.topmostItem_Click)),
                }),
            });
            
            if(config.IsServerLogFormTopmost)
                topmostItem.Checked = this.TopMost = true;

            controller.ConfigChanged += controller_ConfigChanged;

            for (int i = 0; i < ServerDataGrid.Columns.Count; ++i)
            {
                ServerDataGrid.Columns[i].Width = ServerDataGrid.Columns[i].Width * dpi_mul / 4;
            }

            ServerDataGrid.RowTemplate.Height = 20 * dpi_mul / 4;
            int width = 0;
            for (int i = 0; i < ServerDataGrid.Columns.Count; ++i)
            {
                if (!ServerDataGrid.Columns[i].Visible)
                    continue;
                width += ServerDataGrid.Columns[i].Width;
            }
            this.Width = width + SystemInformation.VerticalScrollBarWidth + (this.Width - this.ClientSize.Width) + 1;
            ServerDataGrid.AutoResizeColumnHeadersHeight();
        }

        private void ServerDataGrid_MouseWheel(object sender, MouseEventArgs e)
        {
            if (!ServerDataGrid.Bounds.Contains(e.Location))
            {
                if (e is HandledMouseEventArgs h)
                {
                    h.Handled = true;
                }
            }
            else
            {
                if (e is HandledMouseEventArgs h)
                {
                    h.Handled = false;
                }
            }

        }

        private MenuItem CreateMenuGroup(string text, MenuItem[] items)
        {
            return new MenuItem(I18N.GetString(text), items);
        }

        private MenuItem CreateMenuItem(string text, EventHandler click)
        {
            return new MenuItem(I18N.GetString(text), click);
        }

        private void UpdateTitle()
        {
            this.Text = title_perfix + I18N.GetString("ServerLog") + "("
                + (controller.GetCurrentConfiguration().shareOverLan ? I18N.GetString("Any") : I18N.GetString("Local")) + ":" + controller.GetCurrentConfiguration().localPort.ToString()
                + "(" + Model.Server.GetForwardServerRef().GetConnections().Count.ToString()+ ")"
                + " " + I18N.GetString("Version") + UpdateChecker.FullVersion
                + ")";
        }
        private void UpdateTexts()
        {
            UpdateTitle();
            for (int i = 0; i < ServerDataGrid.Columns.Count; ++i)
            {
                ServerDataGrid.Columns[i].HeaderText = I18N.GetString(ServerDataGrid.Columns[i].HeaderText);
            }
        }

        private void controller_ConfigChanged(object sender, EventArgs e)
        {
            List<string> list = (List<string>)((object[])sender)[0];
            if (list.Contains("All") || list.Contains(this.Name))
            {
                UpdateTitle();
            }
        }
        
        private string FormatBytes(long bytes)
        {
            const long K = 1024L;
            const long M = K * 1024L;
            const long G = M * 1024L;
            const long T = G * 1024L;
            const long P = T * 1024L;
            const long E = P * 1024L;

            if (bytes >= M * 990)
            {
                if (bytes >= G * 990)
                {
                    if (bytes >= P * 990)
                        return (bytes / (double)E).ToString("F3") + "E";
                    if (bytes >= T * 990)
                        return (bytes / (double)P).ToString("F3") + "P";
                    return (bytes / (double)T).ToString("F3") + "T";
                }
                else
                {
                    if (bytes >= G * 99)
                        return (bytes / (double)G).ToString("F2") + "G";
                    if (bytes >= G * 9)
                        return (bytes / (double)G).ToString("F3") + "G";
                    return (bytes / (double)G).ToString("F4") + "G";
                }
            }
            else
            {
                if (bytes >= K * 990)
                {
                    if (bytes >= M * 100)
                        return (bytes / (double)M).ToString("F1") + "M";
                    if (bytes > M * 9.9)
                        return (bytes / (double)M).ToString("F2") + "M";
                    return (bytes / (double)M).ToString("F3") + "M";
                }
                else
                {
                    if (bytes > K * 99)
                        return (bytes / (double)K).ToString("F0") + "K";
                    if (bytes > 900)
                        return (bytes / (double)K).ToString("F1") + "K";
                    return bytes.ToString();
                }
            }
        }

        public bool SetBackColor(DataGridViewCell cell, Color newColor)
        {
            if (cell.Style.BackColor != newColor)
            {
                cell.Style.BackColor = newColor;
                rowChange = true;
                return true;
            }
            return false;
        }
        public bool SetCellToolTipText(DataGridViewCell cell, string newString)
        {
            if (cell.ToolTipText != newString)
            {
                cell.ToolTipText = newString;
                rowChange = true;
                return true;
            }
            return false;
        }
        public bool SetCellText(DataGridViewCell cell, string newString)
        {
            if ((string)cell.Value != newString)
            {
                cell.Value = newString;
                rowChange = true;
                return true;
            }
            return false;
        }
        public bool SetCellText(DataGridViewCell cell, long newInteger)
        {
            if ((string)cell.Value != newInteger.ToString())
            {
                cell.Value = newInteger.ToString();
                rowChange = true;
                return true;
            }
            return false;
        }
        byte ColorMix(byte a, byte b, double alpha)
        {
            return (byte)(b * alpha + a * (1 - alpha));
        }
        Color ColorMix(Color a, Color b, double alpha)
        {
            return Color.FromArgb(ColorMix(a.R, b.R, alpha),
                ColorMix(a.G, b.G, alpha),
                ColorMix(a.B, b.B, alpha));
        }
        public void UpdateLogThread()
        {
            try
            {
                while (workerThread != null)
                {
                    Configuration config = controller.GetCurrentConfiguration();
                    ServerSpeedLogShow[] _ServerSpeedLogList = new ServerSpeedLogShow[config.Servers.Count];
                    for (int i = 0; i < config.Servers.Count && i < _ServerSpeedLogList.Length; ++i)
                    {
                            _ServerSpeedLogList[i] = config.Servers[i].ServerSpeedLog().Translate();
                    }
                    ServerSpeedLogList = _ServerSpeedLogList;

                    workerEvent.WaitOne();
                }
            }
            catch (Exception e)
            {
                Logging.Log(LogLevel.Error, e.Message);
            }
        }
        public void UpdateLog()
        {
            if (workerThread == null)
            {
                workerThread = new Thread(this.UpdateLogThread);
                workerThread.Start();
            }
            else
            {
                workerEvent.Set();
            }
        }

        public void RefreshLog()
        {
            if (ServerSpeedLogList == null)
                return;

            int last_rowcount = ServerDataGrid.RowCount;
            Configuration config = controller.GetCurrentConfiguration();
            if (listOrder.Count > config.Servers.Count)
            {
                listOrder.RemoveRange(config.Servers.Count, listOrder.Count - config.Servers.Count);
            }
            while (listOrder.Count < config.Servers.Count)
            {
                listOrder.Add(0);
            }
            while (ServerDataGrid.RowCount < config.Servers.Count && ServerDataGrid.RowCount < ServerSpeedLogList.Length)
            {
                ServerDataGrid.Rows.Add();
                int id = ServerDataGrid.RowCount - 1;
                ServerDataGrid[0, id].Value = id;
            }
            if (ServerDataGrid.RowCount > config.Servers.Count)
            {
                for (int list_index = 0; list_index < ServerDataGrid.RowCount; ++list_index)
                {
                    DataGridViewCell id_cell = ServerDataGrid[0, list_index];
                    int id = (int)id_cell.Value;
                    if (id >= config.Servers.Count)
                    {
                        ServerDataGrid.Rows.RemoveAt(list_index);
                        --list_index;
                    }
                }
            }
            int displayBeginIndex = ServerDataGrid.FirstDisplayedScrollingRowIndex;
            int displayEndIndex = displayBeginIndex + ServerDataGrid.DisplayedRowCount(true);
            try
            {
                for (int list_index = (lastRefreshIndex >= ServerDataGrid.RowCount) ? 0 : lastRefreshIndex, rowChangeCnt = 0;
                    list_index < ServerDataGrid.RowCount && rowChangeCnt <= 100;
                    ++list_index)
                {
                    lastRefreshIndex = list_index + 1;

                    DataGridViewCell id_cell = ServerDataGrid[0, list_index];
                    int id = (int)id_cell.Value;
                    Server server = config.Servers[id];
                    ServerSpeedLogShow serverSpeedLog = ServerSpeedLogList[id];
                    listOrder[id] = list_index;
                    rowChange = false;
                    for (int curcol = 0; curcol < ServerDataGrid.Columns.Count; ++curcol)
                    {
                        if (!firstDispley
                            && (ServerDataGrid.SortedColumn == null || ServerDataGrid.SortedColumn.Index != curcol)
                            && (list_index < displayBeginIndex || list_index >= displayEndIndex))
                            continue;
                        DataGridViewCell cell = ServerDataGrid[curcol, list_index];
                        string columnName = ServerDataGrid.Columns[curcol].Name;
                        // Server
                        if (columnName == "Server")
                        {
                            if (config.index == id)
                                SetBackColor(cell, Color.Cyan);
                            else
                                SetBackColor(cell, Color.White);
                            SetCellText(cell, server.FriendlyName());
                        }
                        if (columnName == "Group")
                        {
                            SetCellText(cell, server.group);
                        }
                        // Enable
                        if (columnName == "Enable")
                        {
                            if (server.enable) 
                                SetBackColor(cell, Color.White);
                            else
                                SetBackColor(cell, Color.Red);
                        }
                        // TotalConnectTimes
                        else if (columnName == "TotalConnect")
                        {
                            SetCellText(cell, serverSpeedLog.totalConnectTimes);
                        }
                        // TotalConnecting
                        else if (columnName == "Connecting")
                        {
                            long connections = serverSpeedLog.totalConnectTimes - serverSpeedLog.totalDisconnectTimes;
                            Color[] colList = new Color[5] { Color.White, Color.LightGreen, Color.Yellow, Color.Red, Color.Red };
                            long[] bytesList = new long[5] { 0, 16, 32, 64, 65536 };
                            for (int i = 1; i < colList.Length; ++i)
                            {
                                if (connections < bytesList[i])
                                {
                                    SetBackColor(cell,
                                        ColorMix(colList[i - 1],
                                            colList[i],
                                            (double)(connections - bytesList[i - 1]) / (bytesList[i] - bytesList[i - 1])
                                        )
                                        );
                                    break;
                                }
                            }
                            SetCellText(cell, serverSpeedLog.totalConnectTimes - serverSpeedLog.totalDisconnectTimes);
                            
                        }
                        // AvgConnectTime
                        else if (columnName == "AvgLatency")
                        {
                            if (serverSpeedLog.avgConnectTime > 0)
                                SetCellText(cell, serverSpeedLog.avgConnectTime / 1000);
                            else
                                SetCellText(cell, "-");
                        }
                        // AvgDownSpeed
                        else if (columnName == "AvgDownSpeed")
                        {
                            long avgBytes = serverSpeedLog.avgDownloadBytes;
                            string valStr = FormatBytes(avgBytes);
                            Color[] colList = new Color[6] { Color.White, Color.LightGreen, Color.Yellow, Color.Pink, Color.Red, Color.Red };
                            long[] bytesList = new long[6] { 0, 1024 * 64, 1024 * 512, 1024 * 1024 * 4, 1024 * 1024 * 16, 1024L * 1024 * 1024 * 1024 };
                            for (int i = 1; i < colList.Length; ++i)
                            {
                                if (avgBytes < bytesList[i])
                                {
                                    SetBackColor(cell,
                                        ColorMix(colList[i - 1],
                                            colList[i],
                                            (double)(avgBytes - bytesList[i - 1]) / (bytesList[i] - bytesList[i - 1])
                                        )
                                        );
                                    break;
                                }
                            }
                            SetCellText(cell, valStr);
                        }
                        // MaxDownSpeed
                        else if (columnName == "MaxDownSpeed")
                        {
                            long maxBytes = serverSpeedLog.maxDownloadBytes;
                            string valStr = FormatBytes(maxBytes);
                            Color[] colList = new Color[6] { Color.White, Color.LightGreen, Color.Yellow, Color.Pink, Color.Red, Color.Red };
                            long[] bytesList = new long[6] { 0, 1024 * 64, 1024 * 512, 1024 * 1024 * 4, 1024 * 1024 * 16, 1024 * 1024 * 1024 };
                            for (int i = 1; i < colList.Length; ++i)
                            {
                                if (maxBytes < bytesList[i])
                                {
                                    SetBackColor(cell,
                                        ColorMix(colList[i - 1],
                                            colList[i],
                                            (double)(maxBytes - bytesList[i - 1]) / (bytesList[i] - bytesList[i - 1])
                                        )
                                        );
                                    break;
                                }
                            }
                            SetCellText(cell, valStr);
                        }
                        // AvgUpSpeed
                        else if (columnName == "AvgUpSpeed")
                        {
                            long avgBytes = serverSpeedLog.avgUploadBytes;
                            string valStr = FormatBytes(avgBytes);
                            Color[] colList = new Color[6] { Color.White, Color.LightGreen, Color.Yellow, Color.Pink, Color.Red, Color.Red };
                            long[] bytesList = new long[6] { 0, 1024 * 64, 1024 * 512, 1024 * 1024 * 4, 1024 * 1024 * 16, 1024L * 1024 * 1024 * 1024 };
                            for (int i = 1; i < colList.Length; ++i)
                            {
                                if (avgBytes < bytesList[i])
                                {
                                    SetBackColor(cell,
                                        ColorMix(colList[i - 1],
                                            colList[i],
                                            (double)(avgBytes - bytesList[i - 1]) / (bytesList[i] - bytesList[i - 1])
                                        )
                                        );
                                    break;
                                }
                            }
                            SetCellText(cell, valStr);
                        }
                        // MaxUpSpeed
                        else if (columnName == "MaxUpSpeed")
                        {
                            long maxBytes = serverSpeedLog.maxUploadBytes;
                            string valStr = FormatBytes(maxBytes);
                            Color[] colList = new Color[6] { Color.White, Color.LightGreen, Color.Yellow, Color.Pink, Color.Red, Color.Red };
                            long[] bytesList = new long[6] { 0, 1024 * 64, 1024 * 512, 1024 * 1024 * 4, 1024 * 1024 * 16, 1024 * 1024 * 1024 };
                            for (int i = 1; i < colList.Length; ++i)
                            {
                                if (maxBytes < bytesList[i])
                                {
                                    SetBackColor(cell,
                                        ColorMix(colList[i - 1],
                                            colList[i],
                                            (double)(maxBytes - bytesList[i - 1]) / (bytesList[i] - bytesList[i - 1])
                                        )
                                        );
                                    break;
                                }
                            }
                            SetCellText(cell, valStr);
                        }
                        // TotalUploadBytes
                        else if (columnName == "Upload")
                        {
                            string valStr = FormatBytes(serverSpeedLog.totalUploadBytes);
                            string fullVal = serverSpeedLog.totalUploadBytes.ToString();
                            if (cell.ToolTipText != fullVal)
                            {
                                if (fullVal == "0")
                                    SetBackColor(cell, Color.FromArgb(0xf4, 0xff, 0xf4));
                                else
                                {
                                    SetBackColor(cell, Color.LightGreen);
                                    cell.Tag = 8;
                                }
                            }
                            else if (cell.Tag != null)
                            {
                                cell.Tag = (int)cell.Tag - 1;
                                if ((int)cell.Tag == 0) SetBackColor(cell, Color.FromArgb(0xf4, 0xff, 0xf4));
                            }
                            SetCellToolTipText(cell, fullVal);
                            SetCellText(cell, valStr);
                        }
                        // TotalDownloadBytes
                        else if (columnName == "Download")
                        {
                            string valStr = FormatBytes(serverSpeedLog.totalDownloadBytes);
                            string fullVal = serverSpeedLog.totalDownloadBytes.ToString();
                            if (cell.ToolTipText != fullVal)
                            {
                                if (fullVal == "0")
                                    SetBackColor(cell, Color.FromArgb(0xff, 0xf0, 0xf0));
                                else
                                {
                                    SetBackColor(cell, Color.LightGreen);
                                    cell.Tag = 8;
                                }
                            }
                            else if (cell.Tag != null)
                            {
                                cell.Tag = (int)cell.Tag - 1;
                                if ((int)cell.Tag == 0) SetBackColor(cell, Color.FromArgb(0xff, 0xf0, 0xf0));
                            }
                            SetCellToolTipText(cell, fullVal);
                            SetCellText(cell, valStr);
                        }
                        else if (columnName == "DownloadRaw")
                        {
                            string valStr = FormatBytes(serverSpeedLog.totalDownloadRawBytes);
                            string fullVal = serverSpeedLog.totalDownloadRawBytes.ToString();
                            if (cell.ToolTipText != fullVal)
                            {
                                if (fullVal == "0")
                                    SetBackColor(cell, Color.FromArgb(0xff, 0x80, 0x80));
                                else
                                {
                                    SetBackColor(cell, Color.LightGreen);
                                    cell.Tag = 8;
                                }
                            }
                            else if (cell.Tag != null)
                            {
                                cell.Tag = (int)cell.Tag - 1;
                                if ((int)cell.Tag == 0)
                                {
                                    if (fullVal == "0")
                                        SetBackColor(cell, Color.FromArgb(0xff, 0x80, 0x80));
                                    else
                                        SetBackColor(cell, Color.FromArgb(0xf0, 0xf0, 0xff));
                                }
                            }
                            SetCellToolTipText(cell, fullVal);
                            SetCellText(cell, valStr);
                        }
                        // ErrorConnectTimes
                        else if (columnName == "ConnectError")
                        {
                            long val = serverSpeedLog.errorConnectTimes + serverSpeedLog.errorDecodeTimes;
                            Color col = Color.FromArgb(255, (byte)Math.Max(0, 255 - val * 2.5), (byte)Math.Max(0, 255 - val * 2.5));
                            SetBackColor(cell, col);
                            SetCellText(cell, val);
                        }
                        // ErrorTimeoutTimes
                        else if (columnName == "ConnectTimeout")
                        {
                            SetCellText(cell, serverSpeedLog.errorTimeoutTimes);
                        }
                        // ErrorTimeoutTimes
                        else if (columnName == "ConnectEmpty")
                        {
                            long val = serverSpeedLog.errorEmptyTimes;
                            Color col = Color.FromArgb(255, (byte)Math.Max(0, 255 - val * 8), (byte)Math.Max(0, 255 - val * 8));
                            SetBackColor(cell, col);
                            SetCellText(cell, val);
                        }
                        // ErrorContinurousTimes
                        else if (columnName == "Continuous")
                        {
                            long val = serverSpeedLog.errorContinurousTimes;
                            Color col = Color.FromArgb(255, (byte)Math.Max(0, 255 - val * 8), (byte)Math.Max(0, 255 - val * 8));
                            SetBackColor(cell, col);
                            SetCellText(cell, val);
                        }
                        // ErrorPersent
                        else if (columnName == "ErrorPercent")
                        {
                            if (serverSpeedLog.errorLogTimes + serverSpeedLog.totalConnectTimes - serverSpeedLog.totalDisconnectTimes > 0)
                            {
                                double percent = (serverSpeedLog.errorConnectTimes
                                    + serverSpeedLog.errorTimeoutTimes
                                    + serverSpeedLog.errorDecodeTimes)
                                    * 100.00
                                    / (serverSpeedLog.errorLogTimes + serverSpeedLog.totalConnectTimes - serverSpeedLog.totalDisconnectTimes);
                                SetBackColor(cell, Color.FromArgb(255, (byte)(255 - percent * 2), (byte)(255 - percent * 2)));
                                SetCellText(cell, percent.ToString("F0") + "%");
                            }
                            else
                            {
                                SetBackColor(cell, Color.White);
                                SetCellText(cell, "-");
                            }
                        }
                    }
                    if (rowChange && list_index >= displayBeginIndex && list_index < displayEndIndex)
                        rowChangeCnt++;
                }
            }
            catch
            {

            }
            UpdateTitle();
            if (ServerDataGrid.SortedColumn != null)
            {
                ServerDataGrid.Sort(ServerDataGrid.SortedColumn, (ListSortDirection)((int)ServerDataGrid.SortOrder - 1));
            }
            if (last_rowcount == 0 && config.index >= 0 && config.index < ServerDataGrid.RowCount)
            {
                ServerDataGrid[0, config.index].Selected = true;
            }
            if (firstDispley)
            {
                ServerDataGrid.FirstDisplayedScrollingRowIndex = Math.Max(0, config.index - ServerDataGrid.DisplayedRowCount(true) / 2);
                firstDispley = false;
            }
        }

        private void autosizeColumns()
        {
            for (int i = 0; i < ServerDataGrid.Columns.Count; ++i)
            {
                string name = ServerDataGrid.Columns[i].Name;
                if (name == "AvgLatency"
                    || name == "AvgDownSpeed"
                    || name == "MaxDownSpeed"
                    || name == "AvgUpSpeed"
                    || name == "MaxUpSpeed"
                    || name == "Upload"
                    || name == "Download"
                    || name == "DownloadRaw"
                    || name == "Group"
                    || name == "Connecting"
                    || name == "ErrorPercent"
                    || name == "ConnectError"
                    || name == "ConnectTimeout"
                    || name == "Continuous"
                    || name == "ConnectEmpty"
                    )
                {
                    if (ServerDataGrid.Columns[i].Width <= 2)
                        continue;
                    ServerDataGrid.AutoResizeColumn(i, DataGridViewAutoSizeColumnMode.AllCellsExceptHeader);
                    if (name == "AvgLatency"
                        || name == "Connecting"
                        || name == "AvgDownSpeed"
                        || name == "MaxDownSpeed"
                        || name == "AvgUpSpeed"
                        || name == "MaxUpSpeed"
                        )
                    {
                        ServerDataGrid.Columns[i].MinimumWidth = ServerDataGrid.Columns[i].Width;
                    }
                }
            }
            int width = 0;
            for (int i = 0; i < ServerDataGrid.Columns.Count; ++i)
            {
                if (!ServerDataGrid.Columns[i].Visible)
                    continue;
                width += ServerDataGrid.Columns[i].Width;
            }
            this.Width = width + SystemInformation.VerticalScrollBarWidth + (this.Width - this.ClientSize.Width) + 1;
            ServerDataGrid.AutoResizeColumnHeadersHeight();
        }

        private void autosizeItem_Click(object sender, EventArgs e)
        {
            autosizeColumns();
        }

        private void copyLinkItem_Click(object sender, EventArgs e)
        {
            Configuration config = controller.GetCurrentConfiguration();
            if (config.index >= 0 && config.index < config.Servers.Count)
            {
                try
                {
                    string link = config.Servers[config.index].GetSSRLinkForServer();
                    Clipboard.SetText(link);
                }
                catch { }
            }
        }

        private void copyGroupLinkItem_Click(object sender, EventArgs e)
        {
            Configuration config = controller.GetCurrentConfiguration();
            if (config.index >= 0 && config.index < config.Servers.Count)
            {
                string group = config.Servers[config.index].group;
                string link = "";
                for (int index = 0; index < config.Servers.Count; ++index)
                {
                    if (config.Servers[index].group != group)
                        continue;
                    link += config.Servers[index].GetSSRLinkForServer() + System.Environment.NewLine;
                }
                try
                {
                    Clipboard.SetText(link);
                }
                catch { }
            }
        }

        private void copyEnableLinksItem_Click(object sender, EventArgs e)
        {
            Configuration config = controller.GetCurrentConfiguration();
            string link = "";
            for (int index = 0; index < config.Servers.Count; ++index)
            {
                if (!config.Servers[index].enable)
                    continue;
                link += config.Servers[index].GetSSRLinkForServer() + System.Environment.NewLine;
            }
            try
            {
                Clipboard.SetText(link);
            }
            catch { }
        }

        private void copyLinksItem_Click(object sender, EventArgs e)
        {
            Configuration config = controller.GetCurrentConfiguration();
            string link = "";
            for (int index = 0; index < config.Servers.Count; ++index)
            {
                link += config.Servers[index].GetSSRLinkForServer() + System.Environment.NewLine;
            }
            try
            {
                Clipboard.SetText(link);
            }
            catch { }
        }

        private void topmostItem_Click(object sender, EventArgs e)
        {
            topmostItem.Checked = !topmostItem.Checked;
            this.TopMost = topmostItem.Checked;
        }

        private void DisconnectForward_Click(object sender, EventArgs e)
        {
            Model.Server.GetForwardServerRef().GetConnections().CloseAll();
        }

        private void Disconnect_Click(object sender, EventArgs e)
        {
            controller.DisconnectAllConnections();
        }

        private void ClearMaxSpeed_Click(object sender, EventArgs e)
        {
            Configuration config = controller.GetCurrentConfiguration();
            foreach (Server server in config.Servers)
            {
                server.ServerSpeedLog().ClearMaxSpeed();
            }
        }

        private void ClearSelectedTotal_Click(object sender, EventArgs e)
        {
            Configuration config = controller.GetCurrentConfiguration();
            if (config.index >= 0 && config.index < config.Servers.Count)
            {
                try
                {
                    controller.ClearTransferTotal(config.Servers[config.index].server);
                }
                catch { }
            }
        }

        private void ClearTotal_Click(object sender, EventArgs e)
        {
            Configuration config = controller.GetCurrentConfiguration();
            foreach (Server server in config.Servers)
            {
                controller.ClearTransferTotal(server.server);
            }
        }

        private void ClearItem_Click(object sender, EventArgs e)
        {
            Configuration config = controller.GetCurrentConfiguration();
            foreach (Server server in config.Servers)
            {
                server.ServerSpeedLog().Clear();
            }
        }

        private void timer_Tick(object sender, EventArgs e)
        {
            if (updatePause > 0)
            {
                updatePause -= 1;
                return;
            }
            if (this.WindowState == FormWindowState.Minimized)
            {
                if (++pendingUpdate < 40)
                {
                    return;
                }
            }
            else
            {
                ++updateTick;
            }
            pendingUpdate = 0;
            RefreshLog();
            UpdateLog();
            if (updateSize > 1) --updateSize;
            if (updateTick == 2 || updateSize == 1)
            {
                updateSize = 0;
                //autosizeColumns();
            }
        }

        private void ServerDataGrid_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right && e.RowIndex >= 0)
            {
                int id = (int)ServerDataGrid[0, e.RowIndex].Value;
                string columnName = ServerDataGrid.Columns[e.ColumnIndex].Name;
                Configuration config = controller.GetCurrentConfiguration();
                if (columnName == "Server")
                {
                    Disconnect_Click(null, null);
                    //controller.DisconnectAllConnections();
                    controller.SelectServerIndex(id);
                }
                // AvgConnectTime
                else if (columnName == "AvgLatency")
                {
                    string GroupName = config.Servers[id].group;
                    List<string> tmplist = new List<string>();
                    for(int i = 0; i < config.Servers.Count; i++)
                    {
                        Server server = config.Servers[i];
                        if (GroupName == server.group)
                        {
                            tmplist.Add(server.id);
                        }
                    }
                    Util.TCPingManager.StartTcping(controller, tmplist);
                }
                ServerDataGrid[0, e.RowIndex].Selected = true;
            }
        }

        private void ServerDataGrid_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                int id = (int)ServerDataGrid[0, e.RowIndex].Value;
                string columnName = ServerDataGrid.Columns[e.ColumnIndex].Name;
                Configuration config = controller.GetCurrentConfiguration();
                if (columnName == "Server")
                {
                    if (mtimerDataGridServerCellClick == null)
                    {
                        mtimerDataGridServerCellClick = new MyTimer(200);
                        mtimerDataGridServerCellClick.AutoReset = false;
                        //mtimerDataGridServerCellClick.Elapsed -= DataGridServer_CellClick;
                        mtimerDataGridServerCellClick.Elapsed += DataGridServer_CellClick;
                    }
                    if (mtimerDataGridServerCellClick.Enabled)
                        mtimerDataGridServerCellClick.Stop();
                    mtimerDataGridServerCellClick.obj = new object[] { id };
                    mtimerDataGridServerCellClick.Start();

                }
                else if (columnName == "Group")
                {
                    Server cur_server = config.Servers[id];
                    string group = cur_server.group;
                    if (!string.IsNullOrEmpty(group))
                    {
                        bool enable = !cur_server.enable;
                        bool needreload = false;
                        foreach (Server server in config.Servers)
                        {
                            if (server.group == group)
                            {
                                if (server.enable != enable)
                                {
                                    server.enable = enable;
                                    if(!enable && server.GetConnections().Count > 0)
                                    {
                                        server.GetConnections().CloseAll();
                                        if (server.id==config.Servers[id].id)
                                            needreload = true;
                                    }
                                }
                            }
                        }
                        controller.SelectServerIndex(config.index, needreload);
                    }
                }
                else if (columnName == "Enable")
                {
                    Server server = config.Servers[id];
                    server.enable = !server.enable;
                    if (!server.enable && server.GetConnections().Count > 0) 
                        server.GetConnections().CloseAll();
                    controller.SelectServerIndex(config.index, config.index == id ? true : false);
                }
                // AvgConnectTime
                else if (columnName == "AvgLatency")
                {
                    Util.TCPingManager.StartTcping(controller, config.Servers[id].id);
                }
                ServerDataGrid[0, e.RowIndex].Selected = true;
            }
        }
        /// <summary>
        /// 代理委托
        /// </summary>
        private delegate void AddItemDelegate(object sender, EventArgs e);

        /// <summary>
        /// 数据绑定
        /// </summary>
        public void DataGridServer_CellClick(object sender, EventArgs e)
        {
            try
            {
                if (this.InvokeRequired)
                {
                    AddItemDelegate m_SetProgressProxy = new AddItemDelegate(DataGridServer_CellClick);
                    this.Invoke(m_SetProgressProxy, sender, e);
                }
                else
                {
                    object[] obj = ((MyTimer)sender).obj;
                    //controller.DisconnectAllConnections();
                    Disconnect_Click(null, null);
                    controller.SelectServerIndex((int)obj[0]);
                }
            }
            catch (Exception ex)
            {
                Logging.LogUsefulException(ex);
            }
        }

        private void ServerDataGrid_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (mtimerDataGridServerCellClick != null && mtimerDataGridServerCellClick.Enabled) 
            {
                mtimerDataGridServerCellClick.Stop();
            }

            if (e.RowIndex >= 0)
            {
                int id = (int)ServerDataGrid[0, e.RowIndex].Value;
                if (ServerDataGrid.Columns[e.ColumnIndex].Name == "ID")
                {
                    controller.ShowConfigForm(id);
                }
                if (ServerDataGrid.Columns[e.ColumnIndex].Name == "Server")
                {
                    controller.ShowConfigForm(id);
                }
                if (ServerDataGrid.Columns[e.ColumnIndex].Name == "Connecting")
                {
                    Configuration config = controller.GetCurrentConfiguration();
                    Server server = config.Servers[id];
                    server.GetConnections().CloseAll();
                }
                if (ServerDataGrid.Columns[e.ColumnIndex].Name == "MaxDownSpeed" || ServerDataGrid.Columns[e.ColumnIndex].Name == "MaxUpSpeed")
                {
                    Configuration config = controller.GetCurrentConfiguration();
                    config.Servers[id].ServerSpeedLog().ClearMaxSpeed();
                }
                if (ServerDataGrid.Columns[e.ColumnIndex].Name == "Upload" || ServerDataGrid.Columns[e.ColumnIndex].Name == "Download")
                {
                    Configuration config = controller.GetCurrentConfiguration();
                    config.Servers[id].ServerSpeedLog().ClearTrans();
                }
                if (ServerDataGrid.Columns[e.ColumnIndex].Name == "DownloadRaw")
                {
                    Configuration config = controller.GetCurrentConfiguration();
                    config.Servers[id].ServerSpeedLog().Clear();
                    config.Servers[id].enable = true;
                }
                if (ServerDataGrid.Columns[e.ColumnIndex].Name == "ConnectError"
                    || ServerDataGrid.Columns[e.ColumnIndex].Name == "ConnectTimeout"
                    || ServerDataGrid.Columns[e.ColumnIndex].Name == "ConnectEmpty"
                    || ServerDataGrid.Columns[e.ColumnIndex].Name == "Continuous"
                    )
                {
                    Configuration config = controller.GetCurrentConfiguration();
                    config.Servers[id].ServerSpeedLog().ClearError();
                    config.Servers[id].enable = true;
                }
                ServerDataGrid[0, e.RowIndex].Selected = true;
            }
        }

        private void ServerLogForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            Configuration config = controller.GetCurrentConfiguration();
            config.ServerLogFormLocation = TopLeftPoint;
            if (this.TopMost ^ config.IsServerLogFormTopmost)
            {
                config.IsServerLogFormTopmost = this.TopMost;
            }
            //for (int i = 0; i < ServerSpeedLogList.Length; i++)
            //    if (ServerSpeedLogList[i].avgConnectTime > 0)
            //        config.Servers[i].latency = (int)ServerSpeedLogList[i].avgConnectTime / 1000;
            Configuration.Save(config);

            controller.ConfigChanged -= controller_ConfigChanged;

            Thread thread = workerThread;
            workerThread = null;
            while (thread.IsAlive)
            {
                workerEvent.Set();
                Thread.Sleep(10);
            }
        }

        private long Str2Long(String str)
        {
            if (str == "-") return timeout;
            if (str.LastIndexOf('K') > 0)
            {
                Double ret = Convert.ToDouble(str.Substring(0, str.LastIndexOf('K')));
                return (long)(ret * 1024);
            }
            if (str.LastIndexOf('M') > 0)
            {
                Double ret = Convert.ToDouble(str.Substring(0, str.LastIndexOf('M')));
                return (long)(ret * 1024 * 1024);
            }
            if (str.LastIndexOf('G') > 0)
            {
                Double ret = Convert.ToDouble(str.Substring(0, str.LastIndexOf('G')));
                return (long)(ret * 1024 * 1024 * 1024);
            }
            if (str.LastIndexOf('T') > 0)
            {
                Double ret = Convert.ToDouble(str.Substring(0, str.LastIndexOf('T')));
                return (long)(ret * 1024 * 1024 * 1024 * 1024);
            }
            try
            {
                Double ret = Convert.ToDouble(str);
                return (long)ret;
            }
            catch
            {
                return timeout;
            }
        }

        private void ServerDataGrid_SortCompare(object sender, DataGridViewSortCompareEventArgs e)
        {
            //e.SortResult = 0;
            if (e.Column.Name == "Server" || e.Column.Name == "Group")
            {
                e.SortResult = System.String.Compare(Convert.ToString(e.CellValue1), Convert.ToString(e.CellValue2));
                e.Handled = true;
            }
            else if (e.Column.Name == "ID"
                || e.Column.Name == "TotalConnect"
                || e.Column.Name == "Connecting"
                || e.Column.Name == "ConnectError"
                || e.Column.Name == "ConnectTimeout"
                || e.Column.Name == "Continuous"
                )
            {
                Int32 v1 = Convert.ToInt32(e.CellValue1);
                Int32 v2 = Convert.ToInt32(e.CellValue2);
                e.SortResult = (v1 == v2 ? 0 : (v1 < v2 ? -1 : 1));
            }
            else if (e.Column.Name == "ErrorPercent")
            {
                String s1 = Convert.ToString(e.CellValue1);
                String s2 = Convert.ToString(e.CellValue2);
                Int32 v1 = s1.Length <= 1 ? 0 : Convert.ToInt32(Convert.ToDouble(s1.Substring(0, s1.Length - 1)) * 100);
                Int32 v2 = s2.Length <= 1 ? 0 : Convert.ToInt32(Convert.ToDouble(s2.Substring(0, s2.Length - 1)) * 100);
                e.SortResult = v1 == v2 ? 0 : v1 < v2 ? -1 : 1;
            }
            else if (e.Column.Name == "AvgLatency"
                || e.Column.Name == "AvgDownSpeed"
                || e.Column.Name == "MaxDownSpeed"
                || e.Column.Name == "AvgUpSpeed"
                || e.Column.Name == "MaxUpSpeed"
                || e.Column.Name == "Upload"
                || e.Column.Name == "Download"
                || e.Column.Name == "DownloadRaw"
                )
            {
                String s1 = Convert.ToString(e.CellValue1);
                String s2 = Convert.ToString(e.CellValue2);
                long v1 = Str2Long(s1);
                long v2 = Str2Long(s2);
                e.SortResult = (v1 == v2 ? 0 : (v1 < v2 ? -1 : 1));
            }
            if (e.SortResult == 0)
            {
                int v1 = listOrder[Convert.ToInt32(ServerDataGrid[0, e.RowIndex1].Value)];
                int v2 = listOrder[Convert.ToInt32(ServerDataGrid[0, e.RowIndex2].Value)];
                e.SortResult = (v1 == v2 ? 0 : (v1 < v2 ? -1 : 1));
                if (e.SortResult != 0 && ServerDataGrid.SortOrder == SortOrder.Descending)
                {
                    e.SortResult = -e.SortResult;
                }
            }
            if (e.SortResult != 0)
            {
                e.Handled = true;
            }
        }

        private void ServerLogForm_Move(object sender, EventArgs e)
        {
            updatePause = 0;
        }

        protected override void WndProc(ref Message message)
        {
            const int WM_SIZING = 532;
            const int WM_MOVING = 534;
            const int WM_SYSCOMMAND = 0x0112;
            const int SC_MINIMIZE = 0xF020;
            switch (message.Msg)
            {
                case WM_SIZING:
                case WM_MOVING:
                    updatePause = 2;
                    break;
                case WM_SYSCOMMAND:
                    if ((int)message.WParam == SC_MINIMIZE)
                    {
                        Util.Utils.ReleaseMemory(true);
                    }
                    break;
            }
            base.WndProc(ref message);
        }

        private void ServerLogForm_ResizeEnd(object sender, EventArgs e)
        {
            updatePause = 0;

            int width = 0;
            for (int i = 0; i < ServerDataGrid.Columns.Count; ++i)
            {
                if (!ServerDataGrid.Columns[i].Visible)
                    continue;
                width += ServerDataGrid.Columns[i].Width;
            }
            width += SystemInformation.VerticalScrollBarWidth + (this.Width - this.ClientSize.Width) + 1;
            ServerDataGrid.Columns[2].Width += this.Width - width;
        }

        private void ServerDataGrid_ColumnWidthChanged(object sender, DataGridViewColumnEventArgs e)
        {
            int width = 0;
            for (int i = 0; i < ServerDataGrid.Columns.Count; ++i)
            {
                if (!ServerDataGrid.Columns[i].Visible)
                    continue;
                width += ServerDataGrid.Columns[i].Width;
            }
            this.Width = width + SystemInformation.VerticalScrollBarWidth + (this.Width - this.ClientSize.Width) + 1;
            ServerDataGrid.AutoResizeColumnHeadersHeight();
        }

        private void ServerLogForm_LocationChanged(object sender, EventArgs e)
        {
            if (!IsIconic(FindWindow(null, this.Text)))
            {
                TopLeftPoint.X = this.Location.X;
                TopLeftPoint.Y = this.Location.Y;
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

    }
}
