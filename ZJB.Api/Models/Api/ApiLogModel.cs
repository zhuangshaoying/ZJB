using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Newtonsoft.Json;
using ZJB.Api.Common;


namespace ZJB.Api.Models
{
    public class ApiLogModel
    {
        public int Id { get; set; }
        public string Server { get; set; }
        public string Host { get; set; }
        public string PathAndQuery { get; set; }
        public string Controller { get; set; }
        public string Action { get; set; }
        public string IMEI { get; set; }
        public string Device { get; set; }
        public string UserAgent { get; set; }
        public string Hash { get; set; }
        public string Token { get; set; }
        public string ValidationResult { get; set; }
        public string IpAddress { get; set; }
        public string MacAddress { get; set; }
        public string OsVersion { get; set; }
                [JsonConverter(typeof(DateTimeConverter))]
        public DateTime LogTime { get; set; }
    }
}