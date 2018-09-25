using System;

namespace ZJB.WX.Models
{
    public class ActionLog
    {
        public int? UserID { get; set; }
        public string UserName { get; set; }
        public string Host { get; set; }
        public string PathAndQuery { get; set; }
        public string Controller { get; set; }
        public string Action { get; set; }
        public string HttpMethod { get; set; }
        public string Server { get; set; }
        public string UserAgent { get; set; }
        public string IpAddress { get; set; }
        public DateTime AccessTime { get; set; }
    }
}