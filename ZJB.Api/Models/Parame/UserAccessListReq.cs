using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ZJB.Api.Models
{
   public class UserAccessListReq
    {
       public UserAccessListReq()
        {
            pi = 1;
            ps = 100;
            beginHour = 0;
            endHour = 24;
        }
        public int pi { get; set; }
        public int ps { get; set; }
        public int beginHour { get; set; }
        public int endHour { get; set; }
        public string IpAddress { get; set; }
    }
}
