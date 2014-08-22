using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ors.Core.Logging;

namespace Ors.Core.Tasks
{
    public abstract class AbstractTask:ITask
    {
        public ILogger Logger { get; set; }
        public abstract void Execute(TaskContext context);
        public string Name { get; set; }
        public bool Single { get; set; }
        public ITrigger Trigger { get; set; }
        
        protected void Log(object obj, LogLevel level = LogLevel.Info)
        {
            var logger = Logger;
            if (logger == null) return;
            try
            {
                logger.Log( level, obj);
            }
            catch
            {
                
            }
        }
    }
}
