using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ZJB.Api.Models
{
    public class UserReportModel
    {
        /// <summary>
        /// 房源数量
        /// </summary>
        public int HouseCount { get; set; }
        /// <summary>
        /// 今日新增房源数量
        /// </summary>
        public int TodayHouseCount { get; set; }
        /// <summary>
        /// 今日发布数量
        /// </summary>
        public int TodayPostCount { get; set; }
        /// <summary>
        /// 绑定网站个数
        /// </summary>
        public int UserSiteCount { get; set; }
        /// <summary>
        /// 异常账号个数
        /// </summary>
        public int ErrorUserSiteCount { get; set; }
    }
}
