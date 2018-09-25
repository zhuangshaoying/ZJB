using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ZJB.Api.Models
{
    public class GetNotReadNoticeListReq
    {
        public int pi { get; set; }
        public int ps { get; set; }
        public int userId { get; set; }
    }
}
