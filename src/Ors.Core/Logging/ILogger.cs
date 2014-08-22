using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ors.Core.Logging
{
    public interface ILogger
    {
        void LogFormat(LogLevel level, Exception exception, string format, params object[] args);
        bool IsEnabled(LogLevel level);
        void LogFormat(LogLevel level, string format, params object[] args);
        void Log(LogLevel level, object message);
        void Log(LogLevel level, object messsage, Exception exception);
    }
}
