using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZJB.Api.Models.Community
{
    public class SimilarCommunity
    {
        public string CommunityID { get; set; }
        public string Name { get; set; }
        public decimal Level { get; set; }
        public string DistrictName { get; set; }

        public string RegionName { get; set; }
        
        public string Address { get; set; }
    }
}
