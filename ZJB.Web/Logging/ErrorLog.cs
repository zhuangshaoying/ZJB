using System;

namespace ZJB.Web.Logging
{
    public class ErrorLog
    {
        public int Id { get; set; }
        public string Server { get; set; }
        public string Host { get; set; }
        public string Path { get; set; }
        public string Query { get; set; }
        public string Exception { get; set; }
        public string LogPath { get; set; }
        public DateTime LogTime { get; set; }
        public string Notes { get; set; }
    }
}
