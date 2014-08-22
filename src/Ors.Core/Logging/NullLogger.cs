using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ors.Core.Logging
{
    public class NullLogger:ILogger
    {
        public bool IsEnabled(LogLevel level)
        {
            return false;
        }

        void ILogger.LogFormat(LogLevel level, Exception exception, string format, params object[] args)
        {
            
        }

        bool ILogger.IsEnabled(LogLevel level)
        {
            return this.IsEnabled(level);
        }

        void ILogger.LogFormat(LogLevel level, string format, params object[] args)
        {
            
        }


        void ILogger.Log(LogLevel level, object message)
        {
            
        }

        void ILogger.Log(LogLevel level, object messsage, Exception exception)
        {
            
        }
    }
}
