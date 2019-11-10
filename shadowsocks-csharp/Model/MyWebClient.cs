using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Shadowsocks.Model
{
    public class MyWebClient: System.Net.WebClient
    {
        public System.Timers.Timer timer;
        public int mwcindex;
        public ServerSubscribe task;
        public bool free = true;

        public MyWebClient(double timeout = 1000.0 * 30)
        {
            timer = new System.Timers.Timer(timeout);
            timer.Elapsed -= timer_Elapsed;
            timer.Elapsed += timer_Elapsed;
        }

        private void timer_Elapsed(object sender,EventArgs e)
        {
            this.CancelAsync();
        }

        public void DisposeTimer()
        {
            timer.Stop();
            timer.Dispose();
            task = null;
        }
    }
}
