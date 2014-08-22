using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ors.Core.Tasks
{
    public class TriggerEventArgs:EventArgs
    {
        public TaskContext Context { get; set; }
    }
    public delegate void TriggerHandle(object sender, TriggerEventArgs args);
    
    public interface ITrigger
    {
        event TriggerHandle Triggered;
        void Init();
    }
}
