using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ZJB.WX.Models
{
    public class ImpHouseReq
    {
        /// <summary>
        /// 信息类型 1 出售 2 求购 3 出租 4求租 
        /// </summary>
        public int postType { get; set; }
    }
}