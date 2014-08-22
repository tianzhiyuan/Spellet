using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ors.Core.Tasks
{
    public class TaskContext
    {
        public IDictionary<string, object> Objects { get; set; }
        public object Lock;
    }
}
