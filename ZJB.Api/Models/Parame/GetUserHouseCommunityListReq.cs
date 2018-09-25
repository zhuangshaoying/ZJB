using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ZJB.Api.Models
{
    public class GetUserHouseCommunityListReq
    {
        public int PostType { get; set; }
        public int BudlingType { get; set; }
        public int BudlingStatus { get; set; }
        public int UserId { get; set; }
    }
}
