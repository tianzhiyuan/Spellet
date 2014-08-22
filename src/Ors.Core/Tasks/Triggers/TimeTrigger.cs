using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Timers;
using System.Threading.Tasks;

namespace Ors.Core.Tasks.Triggers
{
    public class TimeTrigger : ITrigger
    {
        public event TriggerHandle Triggered;
        private Timer Timer;
        public int Seconds { get; set; }
        public int Interval { get { return this.Seconds*1000; } }
        
        protected void InitTimer()
        {
            var interval = this.Interval;
            if (interval <= 0) return;
            if (Timer == null)
            {
                Timer = new Timer(interval);
            }
            Timer.Elapsed += (sender, args) =>
                {
                    if (Triggered != null)
                    {
                        Triggered.Invoke(this, null);
                    }
                };
        }
        
        public void Init()
        {
            this.InitTimer();
        }
    }
}
