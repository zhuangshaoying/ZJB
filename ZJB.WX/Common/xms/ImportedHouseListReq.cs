using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ZJB.WX.Common.xms
{
    public class ImportedHouseListReq
    {
        public ImportedHouseListReq()
        {
            PageIndex=1;
            PageSize = 30;
        }
        public int UserId { get; set; }
        public int SiteId { get; set; }
        public string SiteUserName { get; set; }
        public int PageIndex { get; set; }
        public int PageSize { get; set; }
        public int CommunityId { get; set; }
        public int TradeType { get; set; }
        public int BuildingType { get; set; }
        public int status { get; set; }
    }
}