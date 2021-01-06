using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Diagnostics;
using System.Net;
using Microsoft.Win32;
using Shadowsocks.Controller;
using Shadowsocks.Model;
using Shadowsocks.Properties;
using ZXing.QrCode.Internal;
using Shadowsocks.Encryption;

namespace Shadowsocks.View
{
    public partial class ConfigForm : Form
    {
        private ShadowsocksController controller;
        private UpdateChecker updateChecker;

        private Settings settings;
        private int _oldSelectedIndex = -1;
        public string currentSelectedID { get { return _oldSelectedIndex == -1 ? null : settings.Servers[_oldSelectedIndex].id; } }
        private string currentServerID = null;

        private int displayItemsCount = 0;
        private bool lbServerMultiselected = false;

        public ConfigForm(ShadowsocksController _controller, UpdateChecker _updateChecker)
        {
            this.controller = _controller;
            this.updateChecker = _updateChecker;
            InitializeComponent();
            this.Icon = Icon.FromHandle(Resources.ssw128.GetHicon());
            if (updateChecker.LatestVersionURL == null)
                llbUpdate.Visible = false;
            this.lstServers.MouseWheel += lstServers_MouseWheel;
            this.DoubleBuffered = true;

            nudServerPort.Minimum = IPEndPoint.MinPort;
            nudServerPort.Maximum = IPEndPoint.MaxPort;
            nudUdpPort.Minimum = IPEndPoint.MinPort;
            nudUdpPort.Maximum = IPEndPoint.MaxPort;


            foreach (string name in EncryptorFactory.GetEncryptor())
            {
                EncryptorInfo info = EncryptorFactory.GetEncryptorInfo(name);
                if (info.display)
                    cmbEncryption.Items.Add(name);
            }


            controller.ConfigChanged += controller_ConfigChanged;

            UpdateTexts();
            LoadConfiguration();

            int dpi_mul = Util.Utils.GetDpiMul();
            DrawLogo(350 * dpi_mul / 4);

            if (!settings.isHideTips)
                picQRcode.Visible = true;

            displayItemsCount = (lstServers.Height - 4) / lstServers.ItemHeight;
            if (settings.Servers.Count > 21)
                displayItemsCount--;

            if (settings.index >-1 && settings.index < settings.Servers.Count)
                currentServerID = settings.Servers[settings.index].id;
        }

        private void UpdateTexts()
        {
            this.Text = I18N.GetString("Edit Servers") + "("
                + (controller.GetCurrentConfiguration().shareOverLan ? I18N.GetString("Any") : I18N.GetString("Local")) + ":" + controller.GetCurrentConfiguration().localPort.ToString()
                + " " + I18N.GetString("Version") + ":" + UpdateChecker.FullVersion
                + ")";

            btnAdd.Text = I18N.GetString("&Add");
            btnDelete.Text = I18N.GetString("&Delete");
            btnUp.Text = I18N.GetString("Up");
            btnDown.Text = I18N.GetString("Down");

            const string mark_str = "*";
            labelIP.Text = mark_str + I18N.GetString(labelIP.Text);//"Server IP");
            labelServerPort.Text = mark_str + I18N.GetString(labelServerPort.Text);//"Server Port");
            labelUDPPort.Text = I18N.GetString(labelUDPPort.Text);// "UDP Port");
            labelPassword.Text = mark_str + I18N.GetString(labelPassword.Text);// "Password");
            labelEncryption.Text = mark_str + I18N.GetString(labelEncryption.Text);// "Encryption");
            labelTCPProtocol.Text = mark_str + I18N.GetString(labelTCPProtocol.Text);
            labelObfs.Text = mark_str + I18N.GetString(labelObfs.Text);
            labelRemarks.Text = I18N.GetString(labelRemarks.Text);// "Remarks");

            chkAdvSetting.Text = I18N.GetString(chkAdvSetting.Text);
            labelTCPoverUDP.Text = I18N.GetString(labelTCPoverUDP.Text);
            labelUDPoverTCP.Text = I18N.GetString(labelUDPoverTCP.Text);
            labelProtocolParam.Text = I18N.GetString(labelProtocolParam.Text);
            labelObfsParam.Text = I18N.GetString(labelObfsParam.Text);
            labelObfsUDP.Text = I18N.GetString(labelObfsUDP.Text);
            labelNote.Text = I18N.GetString(labelNote.Text);
            chkTcpOverUdp.Text = I18N.GetString(chkTcpOverUdp.Text);
            chkUdpOverTcp.Text = I18N.GetString(chkUdpOverTcp.Text);
            CheckObfsUDP.Text = I18N.GetString(CheckObfsUDP.Text);
            chkSSRLink.Text = I18N.GetString(chkSSRLink.Text);
            for (int i = 0; i < cmbTcpProtocol.Items.Count; ++i)
            {
                cmbTcpProtocol.Items[i] = I18N.GetString(cmbTcpProtocol.Items[i].ToString());
            }

            ServerGroupBox.Text = I18N.GetString("Server");

            btnOK.Text = I18N.GetString(btnOK.Text);
            btnClose.Text = I18N.GetString(btnClose.Text);
            btnApply.Text = I18N.GetString(btnApply.Text);
            llbUpdate.MaximumSize = new Size(lstServers.Width, lstServers.Height);
            llbUpdate.Text = String.Format(I18N.GetString("New version {0} {1} available"), UpdateChecker.Name, updateChecker.LatestVersionNumber);
        }

        private delegate void delegateConfigChanged(Object obj, EventArgs e);
        private void controller_ConfigChanged(object sender, EventArgs e)
        {
            List<string> list = (List<string>)((object[])sender)[0];
            if (list.Contains("All") || list.Contains(this.Name))
            {
                if (this.InvokeRequired)
                {
                    delegateConfigChanged adelegateConfigChanged = new delegateConfigChanged(controller_ConfigChanged);
                    this.Invoke(adelegateConfigChanged, new object[] { sender, e });
                    return;
                }

                LoadConfiguration();
            }
        }

        private void LoadConfiguration()
        {
            string tmpselectedid = null;
            List<int> tmpselectedindices = null;
            int lstserversTopIndex = lstServers.TopIndex;
            if (_oldSelectedIndex != -1)
                tmpselectedid = settings.Servers[_oldSelectedIndex].id;
            else if (lstServers.SelectedIndices.Count > 0)
            {
                tmpselectedindices = new List<int>();
                foreach (int i in lstServers.SelectedIndices)
                    tmpselectedindices.Add(i);
            }

            settings = new Settings(controller.GetCurrentConfiguration());
            LoadServersList();

            if (tmpselectedid != null)
            {
                int index = -1;
                index = settings.Servers.FindIndex(t => t.id == tmpselectedid);
                if (index != -1)
                    SetSelectedIndex(new List<int>() { index });
                else
                {
                    SetSelectedIndex(new List<int>() { settings.index });
                }
                SetLstServersTopIndex(lstserversTopIndex);
            }
            else if (tmpselectedindices != null)
            {
                try
                {
                    lstServers.SelectedIndexChanged -= ServersListBox_SelectedIndexChanged;

                    if (!lbServerMultiselected)
                    {
                        SetSelectedIndex(new List<int>() { -1 });
                        LoadSelectedServer();
                    }
                    lbServerMultiselected = true;
                    btnAdd.Enabled = false;

                    foreach (int i in tmpselectedindices)
                        lstServers.SelectedIndex = i;
                }
                catch (Exception e)
                {

                }
                finally
                {
                    lstServers.SelectedIndexChanged += ServersListBox_SelectedIndexChanged;
                    SetLstServersTopIndex(lstserversTopIndex);
                }

            }
        }

        private int SaveOldSelectedServer()
        {
            try
            {
                if (_oldSelectedIndex <0 || _oldSelectedIndex >= settings.Servers.Count || !ServerGroupBox.Enabled)
                {
                    return 0; // no changes
                }
                Server server = new Server
                {
                    server = txtIP.Text.Trim(),
                    server_port = Convert.ToUInt16(nudServerPort.Value),
                    server_udp_port = Convert.ToUInt16(nudUdpPort.Value),
                    password = txtPassword.Text,
                    method = cmbEncryption.Text,
                    protocol = cmbTcpProtocol.Text,
                    protocolparam = txtProtocolParam.Text,
                    obfs = cmbObfs.Text,
                    obfsparam = txtObfsParam.Text,
                    remarks = txtRemarks.Text,
                    group = txtGroup.Text.Trim(),
                    udp_over_tcp = chkUdpOverTcp.Checked,
                    //latency = settings.Servers[_oldSelectedIndex].latency,
                    //obfs_udp = CheckObfsUDP.Checked,
                    id = settings.Servers[_oldSelectedIndex].id,// _SelectedID
                    enable = settings.Servers[_oldSelectedIndex].enable
                };
                Configuration.CheckServer(server);
                if (settings.Servers[_oldSelectedIndex].server != server.server
                    || settings.Servers[_oldSelectedIndex].server_port != server.server_port
                    || settings.Servers[_oldSelectedIndex].remarks_base64 != server.remarks_base64
                    || settings.Servers[_oldSelectedIndex].group != server.group
                    )
                {

                    if (!string.IsNullOrEmpty(server.group))
                        lstServers.Items[_oldSelectedIndex] = server.group + " - " + server.HiddenName();
                    else
                        lstServers.Items[_oldSelectedIndex] = "      " + server.HiddenName();
                }
                settings.Servers[_oldSelectedIndex].CopyBaseFrom(server);
                return 0;
            }
            catch (FormatException)
            {
                MessageBox.Show(I18N.GetString("Illegal port number format"));
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            return -1; // ERROR
        }

        private void DrawLogo(int width)
        {
            Bitmap drawArea = new Bitmap(width, width);
            using (Graphics g = Graphics.FromImage(drawArea))
            {
                g.Clear(Color.White);
                Bitmap ngnl = Resources.ngnl;
                g.DrawImage(ngnl, new Rectangle(0, 0, width, width));
                if (!settings.isHideTips)
                    g.DrawString(I18N.GetString("Click the 'Link' text box"), new Font("宋体", 12), new SolidBrush(Color.Black), new RectangleF(0, 0, 300, 300));
            }
            picQRcode.Image = drawArea;
        }

        private void GenQR(string ssconfig)
        {
            int dpi_mul = Util.Utils.GetDpiMul();
            int width = 350 * dpi_mul / 4;
            if (picQRcode.Visible)// 
            {
                string qrText = ssconfig;
                QRCode code = ZXing.QrCode.Internal.Encoder.encode(qrText, ErrorCorrectionLevel.M);
                ByteMatrix m = code.Matrix;
                int blockSize = Math.Max(width / (m.Width + 2), 1);
                Bitmap drawArea = new Bitmap(((m.Width + 2) * blockSize), ((m.Height + 2) * blockSize));
                using (Graphics g = Graphics.FromImage(drawArea))
                {
                    g.Clear(Color.White);
                    using (Brush b = new SolidBrush(Color.Black))
                    {
                        for (int row = 0; row < m.Width; row++)
                        {
                            for (int col = 0; col < m.Height; col++)
                            {
                                if (m[row, col] != 0)
                                {
                                    g.FillRectangle(b, blockSize * (row + 1), blockSize * (col + 1),
                                        blockSize, blockSize);
                                }
                            }
                        }
                    }
                    Bitmap ngnl = Resources.ngnl;
                    int div = 13, div_l = 5, div_r = 8;
                    int l = (m.Width * div_l + div - 1) / div * blockSize, r = (m.Width * div_r + div - 1) / div * blockSize;
                    g.DrawImage(ngnl, new Rectangle(l + blockSize, l + blockSize, r - l, r - l));
                }
                picQRcode.Image = drawArea;
                settings.isHideTips = true;
            }
            else
            {
                DrawLogo(picQRcode.Width);
            }
        }

        public void LoadSelectedServer()
        {
            if (_oldSelectedIndex > -1 && _oldSelectedIndex < settings.Servers.Count) 
            {
                Server server = settings.Servers[_oldSelectedIndex];

                txtIP.Text = server.server;
                nudServerPort.Value = server.server_port;
                nudUdpPort.Value = server.server_udp_port;
                txtPassword.Text = server.password;
                cmbEncryption.Text = server.method ?? "aes-256-cfb";
                if (string.IsNullOrEmpty(server.protocol))
                {
                    cmbTcpProtocol.Text = "origin";
                }
                else
                {
                    cmbTcpProtocol.Text = server.protocol ?? "origin";
                }
                string obfs_text = server.obfs ?? "plain";
                cmbObfs.Text = obfs_text;
                txtProtocolParam.Text = server.protocolparam;
                txtObfsParam.Text = server.obfsparam;
                txtRemarks.Text = server.remarks;
                txtGroup.Text = server.group;
                chkUdpOverTcp.Checked = server.udp_over_tcp;
                //CheckObfsUDP.Checked = server.obfs_udp;


                if (cmbTcpProtocol.Text == "origin"
                    && obfs_text == "plain"
                    && !chkUdpOverTcp.Checked
                    )
                {
                    chkAdvSetting.Checked = false;
                }

                if (chkSSRLink.Checked)
                {
                    tbSSRLink.Text = server.GetSSRLinkForServer();
                }
                else
                {
                    tbSSRLink.Text = server.GetSSLinkForServer();
                }

                if (chkTcpOverUdp.Checked || chkUdpOverTcp.Checked || server.server_udp_port != 0)
                {
                    chkAdvSetting.Checked = true;
                }

                Update_SSR_controls_Visable();
                UpdateObfsTextbox();
                GenQR(tbSSRLink.Text);
                ServerGroupBox.Enabled = true;

            }
            else
            {
                ServerGroupBox.Enabled = false;
            }
        }

        private void LoadServersList()
        {
            lstServers.BeginUpdate();
            lstServers.ClearSelected();
            lstServers.Items.Clear();
            foreach (Server s in settings.Servers)
            {
                if (!string.IsNullOrEmpty(s.group))
                    lstServers.Items.Add(s.group + " - " + s.HiddenName());
                else
                    lstServers.Items.Add("      " + s.HiddenName());
            }
            lstServers.EndUpdate();
        }

        public void SetSelectedIndex(List<int> listIndex)
        {
            if (listIndex.Count == 1)
            {
                if (listIndex[0] == -1)
                {
                    _oldSelectedIndex = -1;
                }
                else if (listIndex[0] > -1 && listIndex[0] < settings.Servers.Count)
                {
                    lstServers.ClearSelected();
                    _oldSelectedIndex = lstServers.SelectedIndex = listIndex[0];
                }
            }
            else if (listIndex.Count > 1) 
            {
                try
                {
                    lstServers.SelectedIndexChanged -= ServersListBox_SelectedIndexChanged;
                    lstServers.ClearSelected();

                    if (!lbServerMultiselected)
                    {
                        SetSelectedIndex(new List<int>() { -1 });
                        LoadSelectedServer();
                    }
                    lbServerMultiselected = true;
                    btnAdd.Enabled = false;

                    foreach (int i in listIndex)
                        lstServers.SelectedIndex = i;
                }
                catch (Exception e)
                {

                }
                finally
                {
                    lstServers.SelectedIndexChanged += ServersListBox_SelectedIndexChanged;
                }
                
            }
        }

        private void ServersListBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                lstServers.SelectedIndexChanged -= ServersListBox_SelectedIndexChanged;
                if (lstServers.SelectedItems.Count > 1)
                {
                    if (!lbServerMultiselected)
                    {
                        SetSelectedIndex(new List<int>() { -1 });
                        LoadSelectedServer();
                    }
                    lbServerMultiselected = true;
                    btnAdd.Enabled = false;

                    return;
                }

                if (_oldSelectedIndex == lstServers.SelectedIndex || lstServers.SelectedIndex == -1)
                {
                    // we are moving back to oldSelectedIndex or doing a force move
                    return;
                }

                int result = SaveOldSelectedServer();
                if (result == -1)
                {

                    lstServers.SelectedIndex = _oldSelectedIndex; // go back

                    return;
                }
                _oldSelectedIndex = lstServers.SelectedIndex;
                LoadSelectedServer();
                lbServerMultiselected = false;
                btnAdd.Enabled = true;

            }
            catch (Exception ex)
            {
                Logging.Log(LogLevel.Error, ex.Message);
            }
            finally
            {
                lstServers.SelectedIndexChanged += ServersListBox_SelectedIndexChanged;
            }
        }

        public void SetLstServersTopIndex(int index = 0)
        {
            if (index == 0)
                lstServers.TopIndex = _oldSelectedIndex > displayItemsCount / 2 ? _oldSelectedIndex - displayItemsCount / 2 + 1 : 0;
            else
                lstServers.TopIndex = index;
        }

        private void BtnAdd_Click(object sender, EventArgs e)
        {
            try
            {
                lstServers.SelectedIndexChanged -= ServersListBox_SelectedIndexChanged;
                if (SaveOldSelectedServer() == -1)
                {
                    return;
                }
                Server server = _oldSelectedIndex >= 0 && _oldSelectedIndex < settings.Servers.Count
                    ? settings.Servers[_oldSelectedIndex].CloneBase(true)
                    : Configuration.GetDefaultServer();
                settings.Servers.Insert(_oldSelectedIndex < 0 ? 0 : _oldSelectedIndex + 1, server);
                lstServers.Items.Insert(_oldSelectedIndex < 0 ? 0 : _oldSelectedIndex + 1, !string.IsNullOrEmpty(server.group) ? server.group + " - " + server.HiddenName() : "      " + server.HiddenName());

                if (_oldSelectedIndex + 2 == lstServers.Items.Count)
                    lstServers.TopIndex = lstServers.TopIndex + 1;
                if (_oldSelectedIndex < 0)
                {
                    SetSelectedIndex(new List<int>() { 0 });
                    LoadSelectedServer();
                }
                else
                    SetSelectedIndex(new List<int>() { ++_oldSelectedIndex });

            }
            catch (Exception ex)
            {
                Logging.Log(LogLevel.Error, ex.Message);
            }
            finally
            {
                lstServers.SelectedIndexChanged += ServersListBox_SelectedIndexChanged;
            }
        }

        private void BtnDelete_Click(object sender, EventArgs e)
        {
            try
            {
                lstServers.SelectedIndexChanged -= ServersListBox_SelectedIndexChanged;
                int[] array = null;
                lstServers.SelectedIndices.CopyTo(array = new int[lstServers.SelectedIndices.Count], 0);
                Array.Sort(array);
                for (int i = array.Length-1; i > -1; i--)
                {
                    int index = array[i];
                    if(index>-1 && index < lstServers.Items.Count)
                    {
                        settings.Servers.RemoveAt(index);
                        lstServers.Items.RemoveAt(index);
                    }
                }
                if (_oldSelectedIndex >= settings.Servers.Count)
                {
                    SetSelectedIndex(new List<int>() { settings.Servers.Count - 1 });
                }
                else
                    SetSelectedIndex(new List<int>() { _oldSelectedIndex });
                LoadSelectedServer();
                if (lbServerMultiselected)
                {
                    lbServerMultiselected = false;
                    btnAdd.Enabled = true;
                }

            }
            catch (Exception ex)
            {
                Logging.Log(LogLevel.Error, ex.Message);
            }
            finally
            {
                lstServers.SelectedIndexChanged += ServersListBox_SelectedIndexChanged;
                if (!timer_btnLongPress.LongPress)
                    lstServers.Focus();
            }
        }

        private void BtnOK_Click(object sender, EventArgs e)
        {
            controller.ConfigChanged -= controller_ConfigChanged;
            BtnApply_Click(true, null);
            this.Close();
        }

        private void BtnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void BtnApply_Click(object sender, EventArgs e)
        {
            try
            {
                if (SaveOldSelectedServer() == -1)
                {
                    return;
                }
                if (settings.Servers.Count == 0)
                {
                    MessageBox.Show(I18N.GetString("Please add at least one server"));
                    return;
                }
                int index = -1;
                if (currentServerID != null && settings.index < settings.Servers.Count)
                {
                    if (currentSelectedID != settings.Servers[settings.index].id)
                    {
                        index = settings.Servers.FindIndex(t => t.id == currentServerID);
                        if (index != -1 || (index = settings.Servers.FindIndex(t => t.enable)) != -1)
                            settings.index = index;
                        else
                            settings.index = 0;
                    }
                }
                else
                {
                    if ((index = settings.Servers.FindIndex(t => t.enable)) != -1)
                        settings.index = index;
                    else
                        settings.index = 0;
                }
                if (Util.TCPingManager.InProcessing)
                    Util.TCPingManager.StopTcping(this.Name);
                Configuration config = controller.GetCurrentConfiguration();
                settings.SaveTo(config);
                Configuration.Save(config);
                controller.JustReload();
                if (Util.TCPingManager.listTerminator!=null && Util.TCPingManager.listTerminator.Contains(this.Name))
                    Util.TCPingManager.RestoreTcping(this.Name);
            }
            catch (Exception ex)
            {
                Logging.LogUsefulException(ex);
            }
        }

        private void ConfigForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            controller.ConfigChanged -= controller_ConfigChanged;
        }

        private void BtnUp_Click(object sender, EventArgs e)
        {
            if (lstServers.SelectedIndices.Count == 0) 
                return;
            lstServers.BeginUpdate();
            try
            {
                lstServers.SelectedIndexChanged -= ServersListBox_SelectedIndexChanged;
                int[] array = new int[lstServers.SelectedIndices.Count];
                lstServers.SelectedIndices.CopyTo(array, 0);
                Array.Sort(array);
                if (array.Length == 1)
                {
                    int result = SaveOldSelectedServer();
                    if (result == -1)
                        return;
                }
                lstServers.ClearSelected();
                for(int i = 0; i < array.Length; i++)
                {
                    if (array[i] == 0 || (i > 0 && array[i] == array[i - 1] + 1)) 
                    {
                        lstServers.SelectedIndex = array[i];
                        continue;
                    }
                    settings.Servers.Reverse(array[i] - 1, 2);
                    var tmp = lstServers.Items[array[i]];
                    lstServers.Items[array[i]] = lstServers.Items[array[i]-1];
                    lstServers.Items[array[i]-1] = tmp;
                    lstServers.SelectedIndex = --array[i];
                    if (array.Length == 1)
                        _oldSelectedIndex--;
                }

            }
            catch (Exception ex)
            {
                Logging.Log(LogLevel.Error, ex.Message);
            }
            finally
            {
                lstServers.EndUpdate();
                lstServers.SelectedIndexChanged += ServersListBox_SelectedIndexChanged;
                if (!timer_btnLongPress.LongPress)
                    lstServers.Focus();
            }

        }

        private void BtnDown_Click(object sender, EventArgs e)
        {
            if (lstServers.SelectedIndices.Count == 0)
                return;
            lstServers.BeginUpdate();
            try
            {
                lstServers.SelectedIndexChanged -= ServersListBox_SelectedIndexChanged;
                int[] array = new int[lstServers.SelectedIndices.Count];
                lstServers.SelectedIndices.CopyTo(array, 0);
                Array.Sort(array);
                if (array.Length == 1)
                {
                    int result = SaveOldSelectedServer();
                    if (result == -1)
                        return;
                }
                lstServers.ClearSelected();
                for (int i = array.Length - 1; i > -1; i--) 
                {
                    if (array[i] == settings.Servers.Count - 1 || (i < array.Length - 1 && array[i] == array[i + 1] - 1))
                    {
                        lstServers.SelectedIndex = array[i];
                        continue;
                    }
                    settings.Servers.Reverse(array[i], 2);
                    var tmp = lstServers.Items[array[i]];
                    lstServers.Items[array[i]] = lstServers.Items[array[i] + 1];
                    lstServers.Items[array[i] + 1] = tmp;
                    lstServers.SelectedIndex = ++array[i];
                    if (array.Length == 1)
                        _oldSelectedIndex++;
                }
            }
            catch (Exception ex)
            {
                Logging.Log(LogLevel.Error, ex.Message);
            }
            finally
            {
                lstServers.EndUpdate();
                lstServers.SelectedIndexChanged += ServersListBox_SelectedIndexChanged;
                if (!timer_btnLongPress.LongPress)
                    lstServers.Focus();
            }
        }

        private void tbSSRLink_Enter(object sender, EventArgs e)
        {
            try
            {
                lstServers.SelectedIndexChanged -= ServersListBox_SelectedIndexChanged;
                int result = SaveOldSelectedServer();
                if (result == -1)
                    return;
                if (!picQRcode.Visible)
                {
                    picQRcode.Visible = true;
                    LoadSelectedServer();
                }
                tbSSRLink.SelectAll();
                Clipboard.SetDataObject(tbSSRLink.Text);

            }
            catch (Exception ex)
            {
                Logging.Log(LogLevel.Error, ex.Message);
            }
            finally
            {
                lstServers.SelectedIndexChanged += ServersListBox_SelectedIndexChanged;
            }
        }

        private void tbSSRLink_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == System.Windows.Forms.MouseButtons.Left)
            {
                ((TextBox)sender).SelectAll();
            }
        }

        private void LinkUpdate_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            System.Diagnostics.Process.Start(updateChecker.LatestVersionURL);
        }

        private void PasswordLabel_CheckedChanged(object sender, EventArgs e)
        {
            if (labelPassword.Checked)
            {
                txtPassword.UseSystemPasswordChar = false;
            }
            else
            {
                txtPassword.UseSystemPasswordChar = true;
            }
        }

        private void UpdateObfsTextbox()
        {
            try
            {
                Obfs.ObfsBase obfs = (Obfs.ObfsBase)Obfs.ObfsFactory.GetObfs(cmbObfs.Text);
                int[] properties = obfs.GetObfs()[cmbObfs.Text];
                if (properties[2] > 0)
                {
                    txtObfsParam.Enabled = true;
                }
                else
                {
                    txtObfsParam.Enabled = false;
                }
            }
            catch
            {
                txtObfsParam.Enabled = true;
            }
        }

        private void ObfsCombo_TextChanged(object sender, EventArgs e)
        {
            UpdateObfsTextbox();
        }

        private void checkSSRLink_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                lstServers.SelectedIndexChanged -= ServersListBox_SelectedIndexChanged;
                int result = SaveOldSelectedServer();
                if (result == -1)
                    return;
                LoadSelectedServer();

            }
            catch (Exception ex)
            {
                Logging.Log(LogLevel.Error, ex.Message);
            }
            finally
            {
                lstServers.SelectedIndexChanged += ServersListBox_SelectedIndexChanged;
            }

        }

        private void checkAdvSetting_CheckedChanged(object sender, EventArgs e)
        {
            Update_SSR_controls_Visable();
        }

        private void Update_SSR_controls_Visable()
        {
            SuspendLayout();
            if (chkAdvSetting.Checked)
            {
                labelUDPPort.Visible = true;
                nudUdpPort.Visible = true;
                //TCPoverUDPLabel.Visible = true;
                //CheckTCPoverUDP.Visible = true;
            }
            else
            {
                labelUDPPort.Visible = false;
                nudUdpPort.Visible = false;
                //TCPoverUDPLabel.Visible = false;
                //CheckTCPoverUDP.Visible = false;
            }
            if (chkAdvSetting.Checked)
            {
                labelUDPoverTCP.Visible = true;
                chkUdpOverTcp.Visible = true;
            }
            else
            {
                labelUDPoverTCP.Visible = false;
                chkUdpOverTcp.Visible = false;
            }
            ResumeLayout();
        }

        private void IPLabel_CheckedChanged(object sender, EventArgs e)
        {
            if (labelIP.Checked)
            {
                txtIP.UseSystemPasswordChar = false;
            }
            else
            {
                txtIP.UseSystemPasswordChar = true;
            }
        }

        private void btn_MouseDown(object sender, MouseEventArgs e)
        {
            Button btn = (Button)sender;
            if (sender.Equals(this.btnUp))
            {
                timer_btnLongPress.TriggerSource = btnUp.Name;
                timer_btnLongPress.Interval = 50;
            }
            else if (sender.Equals(this.btnDown))
            {
                timer_btnLongPress.TriggerSource = btnDown.Name;
                timer_btnLongPress.Interval = 50;
            }
            else if (sender.Equals(this.btnDelete))
            {
                timer_btnLongPress.TriggerSource = btnDelete.Name;
                timer_btnLongPress.Interval = 100;
            }
            timer_btnLongPress.Start();
        }
        private void btn_MouseUp(object sender, MouseEventArgs e)
        {
            timer_btnLongPress.Stop();
            if (timer_btnLongPress.LongPress)
            {
                timer_btnLongPress.LongPress = false;
                if (timer_btnLongPress.TriggerSource == btnUp.Name)
                    this.btnUp.Click += BtnUp_Click;
                else if (timer_btnLongPress.TriggerSource == btnDown.Name)
                    this.btnDown.Click += BtnDown_Click;
                else if (timer_btnLongPress.TriggerSource == btnDelete.Name)
                    this.btnDelete.Click += this.BtnDelete_Click;
                timer_btnLongPress.TriggerSource = null;
                lstServers.Focus();
            }
        }
        private void timer_btnLongPress_Tick(object sender, EventArgs e)
        {
            timer_btnLongPress.Stop();
            timer_btnLongPress.LongPress = true;
            if (timer_btnLongPress.TriggerSource == btnUp.Name)
            {
                this.btnUp.Click -= BtnUp_Click;
                BtnUp_Click(null, null);
            }
            else if (timer_btnLongPress.TriggerSource == btnDown.Name)
            {
                this.btnDown.Click -= BtnDown_Click;
                BtnDown_Click(null, null);
            }
            else if (timer_btnLongPress.TriggerSource == btnDelete.Name)
            {
                this.btnDelete.Click -= this.BtnDelete_Click;
                BtnDelete_Click(null, null);
            }
            this.timer_btnLongPress.Start();
        }

        private void lstServers_MouseWheel(object sender,MouseEventArgs e)
        {
            if (!this.lstServers.Bounds.Contains(e.Location))
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




        public class Settings
        {
            public List<Server> Servers;
            public int index;
            public bool isHideTips;

            public Settings(Configuration config)
            {
                index = config.index;
                isHideTips = config.isHideTips;
                Servers = new List<Server>();
                foreach (Server s in config.Servers)
                    Servers.Add(s.CloneBase());
            }

            public void SaveTo(Configuration config)
            {
                config.index = index;
                config.isHideTips = isHideTips;
                for(int i = 0; i < config.Servers.Count; i++)
                {
                    int index = Servers.FindIndex(t => t.id == config.Servers[i].id);
                    if (index != -1)
                    {
                        Servers[index].InheritDataFrom(config.Servers[i]);
                    }
                    else
                    {
                        config.Servers[i].enable = false;
                        config.Servers[i].GetConnections().CloseAll();
                    }
                }
                config.Servers = Servers;
            }
        }
    }
}