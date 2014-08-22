using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ors.Core.Tasks
{
    public interface ITask
    {
        ITrigger Trigger { get; set; }
        void Execute(TaskContext context);
        string Name { get; set; }
        bool Single { get; set; }
        
    }
}
