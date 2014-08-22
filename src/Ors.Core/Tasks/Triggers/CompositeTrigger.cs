using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ors.Core.Extensions;

namespace Ors.Core.Tasks.Triggers
{
    public class CompositeTrigger:ITrigger
    {
        public ITrigger[] Triggers { get; set; }
        event TriggerHandle ITrigger.Triggered
        {
            add 
            { 
                var triggers = Triggers;
                if (triggers.IsNullOrEmpty()) return;
                foreach (var trigger in triggers)
                {
                    trigger.Triggered += value;
                }
            }
            remove
            {
                var triggers = Triggers;
                if (triggers.IsNullOrEmpty()) return;
                foreach (var trigger in triggers)
                {
                    trigger.Triggered -= value;
                }
            }
        }



        void ITrigger.Init()
        {
            if (Triggers.IsNullOrEmpty()) return;
            foreach (var trigger in Triggers)
            {
                trigger.Init();
            }
        }
    }
}
