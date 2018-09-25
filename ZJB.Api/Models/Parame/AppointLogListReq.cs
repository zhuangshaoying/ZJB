using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ZJB.Api.Models
{
    public class AppointLogListReq
    {
        public int pi { get; set; }
        public int ps { get; set; }
        public int userId { get; set; }
        public int tradeType { get; set; }
        public int? status { get; set; }
        public DateTime? time { get; set; }
    }
}
