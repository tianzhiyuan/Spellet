using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ors.Core.Logging
{
    public static class Extensions
    {
        
        public static void SafeLog(this ILogger logger, object obj)
        {
            if (logger == null) return;
            try
            {
                logger.Log(LogLevel.Info, obj);
            }
            catch
            {
                
            }
        }
        public static void Log(this ILogger logger, Exception ex)
        {
            logger.Log(LogLevel.Info, "", ex);
        }

        
        public static void Log(this ILogger logger, Exception ex, LogLevel level)
        {
            logger.Log(level, "", ex);
        }

        
    }
}
