using System;

namespace ZJB.Core.Logging
{
    [Serializable]
    public class LogEntry
    {
        public string Message { get; set; }

        public int ThreadId { get; set; }

        public DateTime LogTime { get; set; }

        public ExceptionMessage ExceptionMessage { get; set; }
    }
}
