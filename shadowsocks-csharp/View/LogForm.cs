using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;
using Shadowsocks.Controller;
using Shadowsocks.Model;
using Shadowsocks.Properties;

namespace Shadowsocks.View
{
    public partial class LogForm : Form
    {
        private readonly ShadowsocksController controller;

        private const int MaxReadSize = 65536;

        private string _currentLogFile;
        private string _currentLogFileName;
        private long _currentOffset;

        private bool enabled = true;


        public LogForm(ShadowsocksController _controller)
        {
            controller = _controller;

            InitializeComponent();
            controller.ConfigChanged += LogForm_Load;
            tbLog.MouseWheel += TbLog_MouseWheel;

            Icon = Icon.FromHandle(Resources.ssw128.GetHicon());

            UpdateTexts();
        }

        private void TbLog_MouseWheel(object sender, MouseEventArgs e)
        {
            if (!tbLog.Bounds.Contains(e.Location))
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

        private void UpdateTexts()
        {
            fileToolStripMenuItem.Text = I18N.GetString("&File");
            clearLogToolStripMenuItem.Text = I18N.GetString("Clear &log");
            showInExplorerToolStripMenuItem.Text = I18N.GetString("Show in &Explorer");
            closeToolStripMenuItem.Text = I18N.GetString("&Close");
            viewToolStripMenuItem.Text = I18N.GetString("&View");
            fontToolStripMenuItem.Text = I18N.GetString("&Font...");
            wrapTextToolStripMenuItem.Text = I18N.GetString("&Wrap Text");
            alwaysOnTopToolStripMenuItem.Text = I18N.GetString("&Always on top");
            Text = I18N.GetString("Log Viewer");
        }

        private void closeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void showInExplorerToolStripMenuItem_Click(object sender, EventArgs _)
        {
            try
            {
                string argument = "/n" + ",/select," + Logging.LogFile;
                System.Diagnostics.Process.Start("explorer.exe", argument);
            }
            catch (Exception e)
            {
                Logging.LogUsefulException(e);
            }
        }

        private delegate void delegateConfigChanged(Object obj, EventArgs e);
        private void LogForm_Load(object sender, EventArgs e)
        {
            List<string> list = null;
            if (sender.GetType() == typeof(List<string>))
                list = (List<string>)((object[])sender)[0];
            if (list == null || list.Contains("All") || list.Contains(this.Name)) 
            {
                if (this.InvokeRequired)
                {
                    delegateConfigChanged adelegateConfigChanged = new delegateConfigChanged(LogForm_Load);
                    this.Invoke(adelegateConfigChanged, new object[] { sender, e });
                    return;
                }

                if (controller.GetCurrentConfiguration().enableLogging)
                {
                    if (enabled) 
                    {
                        enabled = false;
                        _currentOffset = 0;
                        tbLog.Clear();
                        tbLog.AppendText("Logging is enabled." + System.Environment.NewLine);
                        ReadLog();
                        refreshTimer.Start();
                    }
                }
                else
                {
                    if (!enabled) 
                    {
                        enabled = true;
                        refreshTimer.Stop();
                        tbLog.Clear();
                        tbLog.AppendText("Logging is disabled, go to [Option Settings] to enable it." + System.Environment.NewLine);
                    }
                }
            }
        }

        private void ReadLog()
        {
            var newLogFile = Logging.LogFile;
            if (newLogFile != _currentLogFile)
            {
                _currentOffset = 0;
                _currentLogFile = newLogFile;
                _currentLogFileName = Logging.LogFileName;
            }

            try
            {
                using (
                    var reader =
                        new StreamReader(new FileStream(newLogFile, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
                )
                {
                    if (_currentOffset == 0)
                    {
                        var maxSize = reader.BaseStream.Length;
                        if (maxSize > MaxReadSize)
                        {
                            reader.BaseStream.Seek(-MaxReadSize, SeekOrigin.End);
                            reader.ReadLine();
                        }
                    }
                    else
                    {
                        reader.BaseStream.Seek(_currentOffset, SeekOrigin.Begin);
                    }

                    var txt = reader.ReadToEnd();
                    if (!string.IsNullOrEmpty(txt)) 
                    {
                        tbLog.AppendText(txt);
                    }

                    _currentOffset = reader.BaseStream.Position;
                }
            }
            catch (FileNotFoundException)
            {
            }
            catch (ArgumentNullException)
            {
            }

            Text = $@"{I18N.GetString("Log Viewer")} {_currentLogFileName}";
        }

        private void refreshTimer_Tick(object sender, EventArgs e)
        {
            ReadLog();
        }

        private void LogForm_Shown(object sender, EventArgs e)
        {
            tbLog.ScrollToCaret();
        }

        private void fontToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (FontDialog fontDialog = new FontDialog())
            {
                fontDialog.Font = tbLog.Font;
                if (fontDialog.ShowDialog() == DialogResult.OK)
                {
                    tbLog.Font = fontDialog.Font;
                }
            }
        }

        private void wrapTextToolStripMenuItem_Click(object sender, EventArgs e)
        {
            wrapTextToolStripMenuItem.Checked = !wrapTextToolStripMenuItem.Checked;
        }

        private void alwaysOnTopToolStripMenuItem_Click(object sender, EventArgs e)
        {
            alwaysOnTopToolStripMenuItem.Checked = !alwaysOnTopToolStripMenuItem.Checked;
        }

        private void wrapTextToolStripMenuItem_CheckedChanged(object sender, EventArgs e)
        {
            tbLog.WordWrap = wrapTextToolStripMenuItem.Checked;
        }

        private void alwaysOnTopToolStripMenuItem_CheckedChanged(object sender, EventArgs e)
        {
            TopMost = alwaysOnTopToolStripMenuItem.Checked;
        }

        private void clearLogToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Logging.Clear();
            _currentOffset = 0;
            tbLog.Clear();
        }

        private void LogForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            controller.ConfigChanged -= LogForm_Load;
        }
    }
}
