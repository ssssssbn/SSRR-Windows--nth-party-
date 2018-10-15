using System;
using System.Drawing;
using System.Windows.Forms;
using Shadowsocks.Model;
using Shadowsocks.Properties;
using Shadowsocks.Controller;

namespace Shadowsocks.View
{
    public partial class ResetPassword : Form
    {
        public ResetPassword()
        {
            InitializeComponent();
            this.Icon = Icon.FromHandle(Resources.ssw128.GetHicon());
            this.Text = I18N.GetString(this.Text);//"ResetPassword"
            this.label4.Text = I18N.GetString(this.label4.Text);
            this.label2.Text = I18N.GetString(this.label2.Text);
            this.label1.Text = I18N.GetString(this.label1.Text);
            this.label3.Text = I18N.GetString(this.label3.Text);
            this.buttonOK.Text = I18N.GetString(this.buttonOK.Text);
        }

        private void buttonOK_Click(object sender, EventArgs e)
        {
            if (textPassword.Text == textPassword2.Text && Configuration.SetPasswordTry(textOld.Text, textPassword.Text))
            {
                Configuration cfg = Configuration.Load();
                Configuration.SetPassword(textPassword.Text);
                Configuration.Save(cfg);
                Close();
            }
            else
            {
                MessageBox.Show(I18N.GetString("Password NOT match"), I18N.GetString("Error"), MessageBoxButtons.OK);
            }
        }

        private void ResetPassword_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                if (textOld.Focused)
                {
                    textPassword.Focus();
                }
                else if (textPassword.Focused)
                {
                    textPassword2.Focus();
                }
                else
                {
                    buttonOK_Click(this, new EventArgs());
                }
            }
        }
    }
}
