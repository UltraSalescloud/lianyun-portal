using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Lianyun.UST.Infrastructure.Logging
{
    public interface ILogger
    {
        void Debug(object source, object message);
        void Debug(object source, object message, Exception exception);
        void Debug(Type source, object message);
        void Debug(Type source, object message, Exception exception);
        void EnsureInitialized();

        void Error(object message);
        void Error(object source, object message);
        void Error(object source, object message, Exception exception);
        void Error(Type source, object message);
        void Error(Type source, object message, Exception exception);
        void Fatal(object source, object message);
        void Fatal(object source, object message, Exception exception);
        void Fatal(Type source, object message);
        void Fatal(Type source, object message, Exception exception);
        void Info(object source, object message);
        void Info(object source, object message, Exception exception);
        void Info(Type source, object message);
        void Info(Type source, object message, Exception exception);
        string SerializeException(Exception e);
        void Warn(object source, object message);
        void Warn(object source, object message, Exception exception);
        void Warn(Type source, object message);
        void Warn(Type source, object message, Exception exception);
    }
}
