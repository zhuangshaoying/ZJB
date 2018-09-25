using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ZJB.Api.Models
{
    public class PostManageModel
    {
        public int HouseID { get; set; }
        public DateTime LastTime { get; set; }
        public DateTime AddTime { get; set; }
        public int SuccessCount { get; set; }
        public int FailCount { get; set; }
        public int Count { get; set; }

        public int PostStatus { get; set; }
        public string PostSites { get; set; }
       
        public int OrderStatus { get; set; }
        public string OrderSites { get; set; }
        public string OrderTime { get; set; }
        /// <summary>
        /// 所有发布过的站点
        /// </summary>
        public string AllSites { get; set; }
        #region 房源相关信息
        public string Title { get; set; }
        #endregion
    }
}
