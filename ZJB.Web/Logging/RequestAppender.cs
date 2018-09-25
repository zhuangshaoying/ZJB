using System;
using System.Threading;
using ZJB.Core.Logging;
using log4net.Appender;
using log4net.Core;

namespace ZJB.Web.Logging
{
    public class RequestAppender : AppenderSkeleton
    {
        protected override void Append(LoggingEvent loggingEvent)
        {
            RequestLog log = RequestLog.GetRequestLog();

            if (loggingEvent.ExceptionObject != null)
            {
                log.ExceptionMessage = new ExceptionMessage(loggingEvent.ExceptionObject);
            }

            LogEntry logEntry = BuildLogEntry(loggingEvent);

            log.LogEntrys.Add(logEntry);
        }

        protected virtual LogEntry BuildLogEntry(LoggingEvent loggingEvent)
        {
            Exception exception = loggingEvent.ExceptionObject;
            LogEntry logEntry = new LogEntry
            {
                ExceptionMessage = exception == null ? null : new ExceptionMessage(exception),
                LogTime = loggingEvent.TimeStamp,
                Message = loggingEvent.MessageObject.ToString(),
                ThreadId = Thread.CurrentThread.ManagedThreadId
            };
            return logEntry;
        }
    }
}
