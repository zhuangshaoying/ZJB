using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZJB.Api.Models.Community
{
    public class CommunityDetailModel
    {
        public ZJB.Api.Entity.Community Community { get; set; }
        public List<RegionsModel> CityList { get; set; }
        public List<RegionsModel> DistrctList { get; set; }
        public List<RegionsModel> RegionList { get; set; }
    }
}
