using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ZJB.Api.Models
{
    public class IpStatListReq
    {
        public IpStatListReq()
        {
            pi = 1;
            ps = 20;
            beginHour = 0;
            endHour = 24;
        }
        public int pi { get; set; }
        public int ps { get; set; }
        public int beginHour { get; set; }
        public int endHour { get; set; }
        public string keyword { get; set; }
    }
}
