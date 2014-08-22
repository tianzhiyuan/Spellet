using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Ors.Core.Logging;
using log4net;
using log4net.Appender;
using log4net.Config;
using log4net.Layout;

namespace Ors.Core.Log4Net
{
    public class Log4NetLogger : ILogger
    {
        private readonly log4net.ILog _logger;
        public string LogName { get; set; }
        public Log4NetLogger(string configFile)
        {
            var file = new FileInfo(configFile);
            if (!file.Exists)
            {
                file = new FileInfo(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, configFile));
            }
            if (file.Exists)
            {
                XmlConfigurator.ConfigureAndWatch(file);
            }
            else
            {
                BasicConfigurator.Configure(new ConsoleAppender { Layout = new PatternLayout() });
            }
            _logger = LogManager.GetCurrentLoggers()[0];
        }
        public void LogFormat(LogLevel level, Exception exception, string format, params object[] args)
        {
            switch (level)
            {
                case LogLevel.Debug:
                    _logger.Debug(string.Format(format, args), exception);
                    break;
                case LogLevel.Error:
                    _logger.Error(string.Format(format, args), exception);
                    break;
                case LogLevel.Fatal:
                    _logger.Fatal(string.Format(format, args), exception);
                    break;
                case LogLevel.Info:
                    _logger.Info(string.Format(format, args), exception);
                    break;
                case LogLevel.Warn:
                    _logger.Warn(string.Format(format, args), exception);
                    break;
            }
        }

        public bool IsEnabled(LogLevel level)
        {
            switch (level)
            {
                case LogLevel.Debug:
                    return _logger.IsDebugEnabled;
                case LogLevel.Error:
                    return _logger.IsErrorEnabled;
                case LogLevel.Fatal:
                    return _logger.IsFatalEnabled;
                case LogLevel.Info:
                    return _logger.IsInfoEnabled;
                case LogLevel.Warn:
                    return _logger.IsWarnEnabled;
            }
            return false;
        }


        public void LogFormat(LogLevel level, string format, params object[] args)
        {
            switch (level)
            {
                case LogLevel.Debug:
                    _logger.Debug(string.Format(format, args));
                    break;
                case LogLevel.Error:
                    _logger.Error(string.Format(format, args));
                    break;
                case LogLevel.Fatal:
                    _logger.Fatal(string.Format(format, args));
                    break;
                case LogLevel.Info:
                    _logger.Info(string.Format(format, args));
                    break;
                case LogLevel.Warn:
                    _logger.Warn(string.Format(format, args));
                    break;
            }
        }

        public void Log(LogLevel level, object message)
        {
            switch (level)
            {
                case LogLevel.Debug:
                    _logger.Debug(message);
                    break;
                case LogLevel.Error:
                    _logger.Error(message);
                    break;
                case LogLevel.Fatal:
                    _logger.Fatal(message);
                    break;
                case LogLevel.Info:
                    _logger.Info(message);
                    break;
                case LogLevel.Warn:
                    _logger.Warn(message);
                    break;
            }
        }

        public void Log(LogLevel level, object message, Exception exception)
        {
            switch (level)
            {
                case LogLevel.Debug:
                    _logger.Debug(message, exception);
                    break;
                case LogLevel.Error:
                    _logger.Error(message, exception);
                    break;
                case LogLevel.Fatal:
                    _logger.Fatal(message, exception);
                    break;
                case LogLevel.Info:
                    _logger.Info(message, exception);
                    break;
                case LogLevel.Warn:
                    _logger.Warn(message, exception);
                    break;
            }
        }
    }
}
