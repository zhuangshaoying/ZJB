using System;

namespace ZJB.Core.Logging
{
    [Serializable]
    public class ExceptionMessage
    {
        private const int MaxInnerException = 5;
        public string Name { get; set; }
        public string Message { get; set; }
        public string StackTrace { get; set; }
        public string TargetSite { get; set; }
        public string Source { get; set; }
        public ExceptionMessage InnerExceptionMessage { get; set; }

        public ExceptionMessage()
        {
            
        }

        public ExceptionMessage(Exception exception)
            : this(exception, 0)
        {

        }

        private ExceptionMessage(Exception exception, int innerLevel)
        {
            Name = exception.GetType().FullName;
            Message = exception.Message;
            StackTrace = exception.StackTrace;
            TargetSite = exception.TargetSite == null ? string.Empty : exception.TargetSite.ToString();
            Source = exception.Source;

            if (exception.InnerException != null && innerLevel<MaxInnerException)
            {
                InnerExceptionMessage = new ExceptionMessage(exception.InnerException, innerLevel + 1);
            }
        }
    }
}
