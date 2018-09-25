using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ZJB.Api.Entity;

namespace ZJB.WX.Models
{
    public class HouseModels
    {
        public HouseBasicInfo HouseBasicInfo { get; set; }
        public HouseInfo HouseInfo { get; set; }
        public VillaInfo VillaInfo { get; set; }
        public ShopInfo ShopInfo { get; set; }
        public OfficeInfo OfficeInfo { get; set; }
        public  FactoryInfo  FactoryInfo { get; set; }
        public List<HouseImage> HouseImages { get; set; }
        public VPublicUser UserInfo  { get; set; }
        public Community Community { get; set; }
    }

    public class NewHouseNoticeModel
    {
        /// <summary>
        /// 采集ID
        /// </summary>
        public string id { get; set; }
        /// <summary>
        /// 标题
        /// </summary>
        public string t { get; set; }
        /// <summary>
        /// 表名
        /// </summary>
        public string c { get; set; }
        /// <summary>
        /// 时间
        /// </summary>
        public DateTime d { get; set; }
        /// <summary>
        /// 来源
        /// </summary>
        public string s { get; set; }
        /// <summary>
        /// 租售类型
        /// </summary>
        public string tt { get; set; }
    }
}