using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shadowsocks.Model
{
    public partial class MyFormsTimer : System.Windows.Forms.Timer
    {
        public string TriggerSource = null;
        public bool LongPress = false;
        public MyFormsTimer()
        {
            InitializeComponent();
        }

        public MyFormsTimer(IContainer container)
        {
            container.Add(this);

            InitializeComponent();
        }
    }
}
