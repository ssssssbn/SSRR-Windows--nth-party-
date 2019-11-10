using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shadowsocks.Model
{
    public class MyTimer:System.Timers.Timer
    {
        public object[] obj;
        public MyTimer()
        {
        }
        public MyTimer(double Interval)
        {
            this.Interval = Interval;
        }
    }
}
