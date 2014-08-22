using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ors.Core.Extensions;
using Ors.Core.Logging;

namespace Ors.Core.Tasks
{
    public class Scheduler:ISheduler
    {
        public ILogger Logger { get; set; }
        public ITask[] Tasks { get; set; }
        public Exception LastError { get; private set; }
        protected void Log(object obj, LogLevel level = LogLevel.Info)
        {
            try
            {
                var logger = Logger;
                if (logger == null) return;
                logger.Log(level, obj);
            }
            catch
            {
                
            }
        }
        public void Start()
        {
            
            var tasks = Tasks;
            if (tasks.IsNullOrEmpty())
            {
                return;
            }
            foreach (var task in tasks)
            {
                var trigger = task.Trigger;
                if (trigger == null) continue;
                trigger.Init();
                trigger.Triggered += new TriggerHandle(CreateOnTrigger(task));
            }
        }

        private readonly IDictionary<string, Delegate> CachedHandlers = new Dictionary<string, Delegate>();
        private readonly IList<string> RunningSingletonTasks = new List<string>();
        private readonly object _lock = new object();
        protected Action<object, TriggerEventArgs> CreateOnTrigger(ITask task) 
        {
            var taskType = task.GetType();
            Delegate handler;
            if (!CachedHandlers.TryGetValue(taskType.FullName, out handler))
            {
                Action<object, TriggerEventArgs> h = (o, args) =>
                    {
                        var taskName = task.Name;
                        if (string.IsNullOrWhiteSpace(taskName)) taskName = this.GetType().Name;
                        Log(string.Format("Task [{0} Started.", taskName));
                        try
                        {
                            if (task.Single)
                            {
                                if (RunningSingletonTasks.Contains(taskName))
                                {
                                    Log(string.Format("Another task instance of {0} is still running.", taskName));
                                    return;
                                }
                                lock (_lock)
                                {
                                    if (RunningSingletonTasks.Contains(taskName))
                                    {
                                        Log(string.Format("Another task instance of {0} is still running.", taskName));
                                        return;
                                    }
                                    RunningSingletonTasks.Add(taskName);
                                }
                            }
                            
                            task.Execute(args.Context);
                            if (task.Single)
                            {
                                RunningSingletonTasks.Remove(taskName);
                            }
                            Log(string.Format("Task [{0} Ended.", taskName));
                        }
                        catch (Exception  error)
                        {
                            LastError = error;
                            Log(error, LogLevel.Error);
                        }
                        
                    };
                CachedHandlers.Add(taskType.FullName, h);
                handler = h;
            }
            return (Action<object, TriggerEventArgs>)handler;
        }
    }
}
