using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ZJB.Api.Models
{
    public  class GetNoticeListReq
    {
        public int userId { get; set; }
        public int pi { get; set; }
        public int ps { get; set; }
        public int type { get; set; }
    }
}
