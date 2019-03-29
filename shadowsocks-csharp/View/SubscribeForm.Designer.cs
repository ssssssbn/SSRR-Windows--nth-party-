namespace Shadowsocks.View
{
    partial class SubscribeForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.labelURL = new System.Windows.Forms.Label();
            this.labelGroupName = new System.Windows.Forms.Label();
            this.textBoxURL = new System.Windows.Forms.TextBox();
            this.textBoxGroup = new System.Windows.Forms.TextBox();
            this.labelLastUpdate = new System.Windows.Forms.Label();
            this.textUpdate = new System.Windows.Forms.TextBox();
            this.labelUseProxy = new System.Windows.Forms.Label();
            this.checkBoxUseProxy = new System.Windows.Forms.CheckBox();
            this.checkBoxAutoUpdate = new System.Windows.Forms.CheckBox();
            this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
            this.ButtonOK = new System.Windows.Forms.Button();
            this.ButtonCancel = new System.Windows.Forms.Button();
            this.tableLayoutPanel3 = new System.Windows.Forms.TableLayoutPanel();
            this.tableLayoutPanel5 = new System.Windows.Forms.TableLayoutPanel();
            this.tableLayoutPanel4 = new System.Windows.Forms.TableLayoutPanel();
            this.ButtonAdd = new System.Windows.Forms.Button();
            this.ButtonDel = new System.Windows.Forms.Button();
            this.tableLayoutPanel6 = new System.Windows.Forms.TableLayoutPanel();
            this.checkBoxAutoUpdateUseProxy = new System.Windows.Forms.CheckBox();
            this.checkBoxAutoUpdateTryUseProxy = new System.Windows.Forms.CheckBox();
            this.checkBoxAutoLatency = new System.Windows.Forms.CheckBox();
            this.listServerSubscribe = new System.Windows.Forms.ListBox();
            this.timer_ButtonDel = new System.Windows.Forms.Timer(this.components);
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.tableLayoutPanel1.SuspendLayout();
            this.tableLayoutPanel2.SuspendLayout();
            this.tableLayoutPanel3.SuspendLayout();
            this.tableLayoutPanel5.SuspendLayout();
            this.tableLayoutPanel4.SuspendLayout();
            this.tableLayoutPanel6.SuspendLayout();
            this.SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.tableLayoutPanel1.AutoSize = true;
            this.tableLayoutPanel1.ColumnCount = 2;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel1.Controls.Add(this.labelURL, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.labelGroupName, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.textBoxURL, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this.textBoxGroup, 1, 1);
            this.tableLayoutPanel1.Controls.Add(this.labelLastUpdate, 0, 2);
            this.tableLayoutPanel1.Controls.Add(this.textUpdate, 1, 2);
            this.tableLayoutPanel1.Controls.Add(this.labelUseProxy, 0, 3);
            this.tableLayoutPanel1.Controls.Add(this.checkBoxUseProxy, 1, 3);
            this.tableLayoutPanel1.Location = new System.Drawing.Point(352, 3);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 4;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.Size = new System.Drawing.Size(269, 101);
            this.tableLayoutPanel1.TabIndex = 1;
            // 
            // labelURL
            // 
            this.labelURL.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.labelURL.AutoSize = true;
            this.labelURL.Location = new System.Drawing.Point(51, 7);
            this.labelURL.Name = "labelURL";
            this.labelURL.Size = new System.Drawing.Size(23, 12);
            this.labelURL.TabIndex = 0;
            this.labelURL.Text = "URL";
            // 
            // labelGroupName
            // 
            this.labelGroupName.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.labelGroupName.AutoSize = true;
            this.labelGroupName.Location = new System.Drawing.Point(9, 34);
            this.labelGroupName.Name = "labelGroupName";
            this.labelGroupName.Size = new System.Drawing.Size(65, 12);
            this.labelGroupName.TabIndex = 2;
            this.labelGroupName.Text = "Group name";
            // 
            // textBoxURL
            // 
            this.textBoxURL.Location = new System.Drawing.Point(80, 3);
            this.textBoxURL.Name = "textBoxURL";
            this.textBoxURL.Size = new System.Drawing.Size(186, 21);
            this.textBoxURL.TabIndex = 1;
            // 
            // textBoxGroup
            // 
            this.textBoxGroup.Location = new System.Drawing.Point(80, 30);
            this.textBoxGroup.Name = "textBoxGroup";
            this.textBoxGroup.ReadOnly = true;
            this.textBoxGroup.Size = new System.Drawing.Size(186, 21);
            this.textBoxGroup.TabIndex = 3;
            // 
            // labelLastUpdate
            // 
            this.labelLastUpdate.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.labelLastUpdate.AutoSize = true;
            this.labelLastUpdate.Location = new System.Drawing.Point(3, 61);
            this.labelLastUpdate.Name = "labelLastUpdate";
            this.labelLastUpdate.Size = new System.Drawing.Size(71, 12);
            this.labelLastUpdate.TabIndex = 4;
            this.labelLastUpdate.Text = "Last Update";
            // 
            // textUpdate
            // 
            this.textUpdate.Location = new System.Drawing.Point(80, 57);
            this.textUpdate.Name = "textUpdate";
            this.textUpdate.ReadOnly = true;
            this.textUpdate.Size = new System.Drawing.Size(186, 21);
            this.textUpdate.TabIndex = 5;
            // 
            // labelUseProxy
            // 
            this.labelUseProxy.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.labelUseProxy.AutoSize = true;
            this.labelUseProxy.Location = new System.Drawing.Point(15, 85);
            this.labelUseProxy.Name = "labelUseProxy";
            this.labelUseProxy.Size = new System.Drawing.Size(59, 12);
            this.labelUseProxy.TabIndex = 7;
            this.labelUseProxy.Text = "Use proxy";
            // 
            // checkBoxUseProxy
            // 
            this.checkBoxUseProxy.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.checkBoxUseProxy.AutoSize = true;
            this.checkBoxUseProxy.Location = new System.Drawing.Point(80, 84);
            this.checkBoxUseProxy.Name = "checkBoxUseProxy";
            this.checkBoxUseProxy.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.checkBoxUseProxy.Size = new System.Drawing.Size(15, 14);
            this.checkBoxUseProxy.TabIndex = 6;
            this.checkBoxUseProxy.UseVisualStyleBackColor = true;
            // 
            // checkBoxAutoUpdate
            // 
            this.checkBoxAutoUpdate.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.checkBoxAutoUpdate.AutoSize = true;
            this.checkBoxAutoUpdate.Location = new System.Drawing.Point(15, 3);
            this.checkBoxAutoUpdate.Name = "checkBoxAutoUpdate";
            this.checkBoxAutoUpdate.Size = new System.Drawing.Size(90, 16);
            this.checkBoxAutoUpdate.TabIndex = 2;
            this.checkBoxAutoUpdate.Text = "Auto update";
            this.checkBoxAutoUpdate.UseVisualStyleBackColor = true;
            // 
            // tableLayoutPanel2
            // 
            this.tableLayoutPanel2.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.tableLayoutPanel2.AutoSize = true;
            this.tableLayoutPanel2.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.tableLayoutPanel2.ColumnCount = 2;
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel2.Controls.Add(this.ButtonOK, 0, 0);
            this.tableLayoutPanel2.Controls.Add(this.ButtonCancel, 1, 0);
            this.tableLayoutPanel2.Location = new System.Drawing.Point(378, 293);
            this.tableLayoutPanel2.Name = "tableLayoutPanel2";
            this.tableLayoutPanel2.RowCount = 1;
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel2.Size = new System.Drawing.Size(216, 45);
            this.tableLayoutPanel2.TabIndex = 3;
            // 
            // ButtonOK
            // 
            this.ButtonOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.ButtonOK.Location = new System.Drawing.Point(3, 3);
            this.ButtonOK.Name = "ButtonOK";
            this.ButtonOK.Size = new System.Drawing.Size(102, 39);
            this.ButtonOK.TabIndex = 0;
            this.ButtonOK.Text = "OK";
            this.ButtonOK.UseVisualStyleBackColor = true;
            this.ButtonOK.Click += new System.EventHandler(this.ButtonOK_Click);
            // 
            // ButtonCancel
            // 
            this.ButtonCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.ButtonCancel.Location = new System.Drawing.Point(111, 3);
            this.ButtonCancel.Name = "ButtonCancel";
            this.ButtonCancel.Size = new System.Drawing.Size(102, 39);
            this.ButtonCancel.TabIndex = 1;
            this.ButtonCancel.Text = "Cancel";
            this.ButtonCancel.UseVisualStyleBackColor = true;
            this.ButtonCancel.Click += new System.EventHandler(this.ButtonCancel_Click);
            // 
            // tableLayoutPanel3
            // 
            this.tableLayoutPanel3.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tableLayoutPanel3.AutoSize = true;
            this.tableLayoutPanel3.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.tableLayoutPanel3.ColumnCount = 2;
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel3.Controls.Add(this.tableLayoutPanel5, 0, 1);
            this.tableLayoutPanel3.Controls.Add(this.tableLayoutPanel1, 1, 0);
            this.tableLayoutPanel3.Controls.Add(this.tableLayoutPanel2, 1, 1);
            this.tableLayoutPanel3.Controls.Add(this.listServerSubscribe, 0, 0);
            this.tableLayoutPanel3.Location = new System.Drawing.Point(12, 12);
            this.tableLayoutPanel3.Name = "tableLayoutPanel3";
            this.tableLayoutPanel3.RowCount = 2;
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel3.Size = new System.Drawing.Size(624, 341);
            this.tableLayoutPanel3.TabIndex = 0;
            // 
            // tableLayoutPanel5
            // 
            this.tableLayoutPanel5.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.tableLayoutPanel5.AutoSize = true;
            this.tableLayoutPanel5.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.tableLayoutPanel5.ColumnCount = 1;
            this.tableLayoutPanel5.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel5.Controls.Add(this.tableLayoutPanel4, 0, 0);
            this.tableLayoutPanel5.Controls.Add(this.tableLayoutPanel6, 0, 1);
            this.tableLayoutPanel5.Location = new System.Drawing.Point(63, 253);
            this.tableLayoutPanel5.Name = "tableLayoutPanel5";
            this.tableLayoutPanel5.RowCount = 2;
            this.tableLayoutPanel5.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel5.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel5.Size = new System.Drawing.Size(222, 85);
            this.tableLayoutPanel5.TabIndex = 2;
            // 
            // tableLayoutPanel4
            // 
            this.tableLayoutPanel4.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.tableLayoutPanel4.AutoSize = true;
            this.tableLayoutPanel4.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.tableLayoutPanel4.ColumnCount = 2;
            this.tableLayoutPanel4.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel4.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel4.Controls.Add(this.ButtonAdd, 0, 0);
            this.tableLayoutPanel4.Controls.Add(this.ButtonDel, 1, 0);
            this.tableLayoutPanel4.Location = new System.Drawing.Point(30, 3);
            this.tableLayoutPanel4.Name = "tableLayoutPanel4";
            this.tableLayoutPanel4.RowCount = 1;
            this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel4.Size = new System.Drawing.Size(162, 29);
            this.tableLayoutPanel4.TabIndex = 5;
            // 
            // ButtonAdd
            // 
            this.ButtonAdd.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.ButtonAdd.Location = new System.Drawing.Point(3, 3);
            this.ButtonAdd.Name = "ButtonAdd";
            this.ButtonAdd.Size = new System.Drawing.Size(75, 23);
            this.ButtonAdd.TabIndex = 0;
            this.ButtonAdd.Text = "&Add";
            this.ButtonAdd.UseVisualStyleBackColor = true;
            this.ButtonAdd.Click += new System.EventHandler(this.ButtonAdd_Click);
            // 
            // ButtonDel
            // 
            this.ButtonDel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.ButtonDel.Location = new System.Drawing.Point(84, 3);
            this.ButtonDel.Name = "ButtonDel";
            this.ButtonDel.Size = new System.Drawing.Size(75, 23);
            this.ButtonDel.TabIndex = 1;
            this.ButtonDel.Text = "&Delete";
            this.ButtonDel.UseVisualStyleBackColor = true;
            this.ButtonDel.Click += new System.EventHandler(this.ButtonDel_Click);
            this.ButtonDel.MouseDown += new System.Windows.Forms.MouseEventHandler(this.ButtonDel_MouseDown);
            this.ButtonDel.MouseUp += new System.Windows.Forms.MouseEventHandler(this.ButtonDel_MouseUp);
            // 
            // tableLayoutPanel6
            // 
            this.tableLayoutPanel6.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.tableLayoutPanel6.AutoSize = true;
            this.tableLayoutPanel6.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.tableLayoutPanel6.ColumnCount = 2;
            this.tableLayoutPanel6.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel6.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel6.Controls.Add(this.checkBoxAutoUpdateUseProxy, 0, 1);
            this.tableLayoutPanel6.Controls.Add(this.checkBoxAutoUpdateTryUseProxy, 1, 1);
            this.tableLayoutPanel6.Controls.Add(this.checkBoxAutoUpdate, 0, 0);
            this.tableLayoutPanel6.Controls.Add(this.checkBoxAutoLatency, 1, 0);
            this.tableLayoutPanel6.Location = new System.Drawing.Point(3, 38);
            this.tableLayoutPanel6.Name = "tableLayoutPanel6";
            this.tableLayoutPanel6.RowCount = 2;
            this.tableLayoutPanel6.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel6.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel6.Size = new System.Drawing.Size(216, 44);
            this.tableLayoutPanel6.TabIndex = 6;
            // 
            // checkBoxAutoUpdateUseProxy
            // 
            this.checkBoxAutoUpdateUseProxy.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.checkBoxAutoUpdateUseProxy.AutoSize = true;
            this.checkBoxAutoUpdateUseProxy.Location = new System.Drawing.Point(27, 25);
            this.checkBoxAutoUpdateUseProxy.Name = "checkBoxAutoUpdateUseProxy";
            this.checkBoxAutoUpdateUseProxy.Size = new System.Drawing.Size(78, 16);
            this.checkBoxAutoUpdateUseProxy.TabIndex = 4;
            this.checkBoxAutoUpdateUseProxy.Text = "Use proxy";
            this.checkBoxAutoUpdateUseProxy.UseVisualStyleBackColor = true;
            this.checkBoxAutoUpdateUseProxy.CheckedChanged += new System.EventHandler(this.checkBoxAutoUpdateUseProxy_CheckedChanged);
            // 
            // checkBoxAutoUpdateTryUseProxy
            // 
            this.checkBoxAutoUpdateTryUseProxy.AutoSize = true;
            this.checkBoxAutoUpdateTryUseProxy.Location = new System.Drawing.Point(111, 25);
            this.checkBoxAutoUpdateTryUseProxy.Name = "checkBoxAutoUpdateTryUseProxy";
            this.checkBoxAutoUpdateTryUseProxy.Size = new System.Drawing.Size(102, 16);
            this.checkBoxAutoUpdateTryUseProxy.TabIndex = 5;
            this.checkBoxAutoUpdateTryUseProxy.Text = "Try use proxy";
            this.checkBoxAutoUpdateTryUseProxy.UseVisualStyleBackColor = true;
            this.checkBoxAutoUpdateTryUseProxy.CheckedChanged += new System.EventHandler(this.checkBoxAutoUpdateTryUseProxy_CheckedChanged);
            // 
            // checkBoxAutoLatency
            // 
            this.checkBoxAutoLatency.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.checkBoxAutoLatency.AutoSize = true;
            this.checkBoxAutoLatency.Location = new System.Drawing.Point(111, 3);
            this.checkBoxAutoLatency.Name = "checkBoxAutoLatency";
            this.checkBoxAutoLatency.Size = new System.Drawing.Size(96, 16);
            this.checkBoxAutoLatency.TabIndex = 3;
            this.checkBoxAutoLatency.Text = "Auto latency";
            this.checkBoxAutoLatency.UseVisualStyleBackColor = true;
            // 
            // listServerSubscribe
            // 
            this.listServerSubscribe.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.listServerSubscribe.FormattingEnabled = true;
            this.listServerSubscribe.HorizontalScrollbar = true;
            this.listServerSubscribe.ItemHeight = 12;
            this.listServerSubscribe.Location = new System.Drawing.Point(3, 3);
            this.listServerSubscribe.Name = "listServerSubscribe";
            this.listServerSubscribe.Size = new System.Drawing.Size(343, 244);
            this.listServerSubscribe.TabIndex = 0;
            this.listServerSubscribe.SelectedIndexChanged += new System.EventHandler(this.listServerSubscribe_SelectedIndexChanged);
            // 
            // timer_ButtonDel
            // 
            this.timer_ButtonDel.Tick += new System.EventHandler(this.timer_ButtonDel_Tick);
            // 
            // SubscribeForm
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.AutoSize = true;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.ClientSize = new System.Drawing.Size(648, 365);
            this.Controls.Add(this.tableLayoutPanel3);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.Name = "SubscribeForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Subscribe Settings";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.SubscribeForm_FormClosed);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.tableLayoutPanel2.ResumeLayout(false);
            this.tableLayoutPanel3.ResumeLayout(false);
            this.tableLayoutPanel3.PerformLayout();
            this.tableLayoutPanel5.ResumeLayout(false);
            this.tableLayoutPanel5.PerformLayout();
            this.tableLayoutPanel4.ResumeLayout(false);
            this.tableLayoutPanel6.ResumeLayout(false);
            this.tableLayoutPanel6.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Label labelURL;
        private System.Windows.Forms.Label labelGroupName;
        private System.Windows.Forms.TextBox textBoxURL;
        private System.Windows.Forms.TextBox textBoxGroup;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
        private System.Windows.Forms.Button ButtonOK;
        private System.Windows.Forms.Button ButtonCancel;
        private System.Windows.Forms.CheckBox checkBoxAutoUpdate;
        private System.Windows.Forms.CheckBox checkBoxAutoUpdateUseProxy;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel3;
        private System.Windows.Forms.ListBox listServerSubscribe;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel4;
        private System.Windows.Forms.Button ButtonAdd;
        private System.Windows.Forms.Button ButtonDel;
        private System.Windows.Forms.Label labelLastUpdate;
        private System.Windows.Forms.TextBox textUpdate;
        private System.Windows.Forms.CheckBox checkBoxUseProxy;
        private System.Windows.Forms.Label labelUseProxy;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel5;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel6;
        private System.Windows.Forms.CheckBox checkBoxAutoUpdateTryUseProxy;
        private System.Windows.Forms.CheckBox checkBoxAutoLatency;
        private System.Windows.Forms.Timer timer_ButtonDel;
        private System.Windows.Forms.ToolTip toolTip1;
    }
}