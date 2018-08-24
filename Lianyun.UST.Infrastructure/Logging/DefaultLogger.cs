using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using log4net;
using System.IO;
using log4net.Config;

namespace Lianyun.UST.Infrastructure.Logging
{

    public class DefaultLogger : ILogger
    {
        private Dictionary<Type, ILog> _loggers = new Dictionary<Type, ILog>();
        private bool _logInitialized = false;
        private object _lock = new object();

        public string SerializeException(Exception e)
        {
            return SerializeException(e, string.Empty);
        }

        private string SerializeException(Exception e, string exceptionMessage)
        {
            if (e == null) return string.Empty;

            exceptionMessage = string.Format(
                "{0}{1}{2}\n{3}",
                exceptionMessage,
                (exceptionMessage == string.Empty) ? string.Empty : "\n\n",
                e.Message,
                e.StackTrace);

            if (e.InnerException != null)
                exceptionMessage = SerializeException(e.InnerException, exceptionMessage);

            return exceptionMessage;
        }

        private ILog getLogger()
        {
            EnsureInitialized();
            lock (_lock)
            {               
                    ILog logger = LogManager.GetLogger("ErrorLog");                  
                    return logger;              
            }
        }
        private ILog getLogger(Type source)
        {
            EnsureInitialized();
            lock (_lock)
            {
                if (_loggers.ContainsKey(source))
                {
                    return _loggers[source];
                }
                else
                {
                    ILog logger = LogManager.GetLogger(source);
                    _loggers.Add(source, logger);
                    return logger;
                }
            }
        }
        /* Log a message object */

        public void Debug(object source, object message)
        {
            Debug(source.GetType(), message);
        }

        public void Debug(Type source, object message)
        {
            getLogger(source).Debug(message);
        }

        public void Info(object source, object message)
        {
            Info(source.GetType(), message);
        }

        public void Info(Type source, object message)
        {
            getLogger(source).Info(message);
        }

        public void Warn(object source, object message)
        {
            Warn(source.GetType(), message);
        }

        public void Warn(Type source, object message)
        {
            getLogger(source).Warn(message);
        }

        public void Error(object source, object message)
        {
            Error(source.GetType(), message);
        }

        public void Error(Type source, object message)
        {
            getLogger(source).Error(message);
        }
        public void Error(object message)
        {
            getLogger().Error(message);
        }

        public void Fatal(object source, object message)
        {
            Fatal(source.GetType(), message);
        }

        public void Fatal(Type source, object message)
        {
            getLogger(source).Fatal(message);
        }

        /* Log a message object and exception */

        public void Debug(object source, object message, Exception exception)
        {
            Debug(source.GetType(), message, exception);
        }

        public void Debug(Type source, object message, Exception exception)
        {
            getLogger(source).Debug(message, exception);
        }

        public void Info(object source, object message, Exception exception)
        {
            Info(source.GetType(), message, exception);
        }

        public void Info(Type source, object message, Exception exception)
        {
            getLogger(source).Info(message, exception);
        }

        public void Warn(object source, object message, Exception exception)
        {
            Warn(source.GetType(), message, exception);
        }

        public void Warn(Type source, object message, Exception exception)
        {
            getLogger(source).Warn(message, exception);
        }

        public void Error(object source, object message, Exception exception)
        {
            Error(source.GetType(), message, exception);
        }

        public void Error(Type source, object message, Exception exception)
        {
            getLogger(source).Error(message, exception);
        }

        public void Fatal(object source, object message, Exception exception)
        {
            Fatal(source.GetType(), message, exception);
        }

        public void Fatal(Type source, object message, Exception exception)
        {
            getLogger(source).Fatal(message, exception);
        }

        private void initialize()
        {
            string logFilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Log4Net.config");
            if (!File.Exists(logFilePath))
            {
                logFilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"bin\Log4Net.config");
            }

            XmlConfigurator.ConfigureAndWatch(new FileInfo(logFilePath));
        }

        public void EnsureInitialized()
        {
            if (!_logInitialized)
            {
                initialize();
                _logInitialized = true;
            }
        }
    }

}
