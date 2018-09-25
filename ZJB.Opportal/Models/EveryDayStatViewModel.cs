using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ZJB.Api.Models;
namespace ZJB.Opportal.Models
{
    public class EveryDayStatViewModel
    {
        /// <summary>
        /// 每日登陆
        /// </summary>
        public dynamic UserLoginStat { get; set; }
        /// <summary>
        /// 每日新增
        /// </summary>
        public dynamic HouseAddStat { get; set; }
        /// <summary>
        /// 每日推送房源
        /// </summary>
        public dynamic PushHouseStat { get; set; }
        /// <summary>
        /// 每日功能访问
        /// </summary>
        public dynamic FunctionStat { get; set; }
        /// <summary>
        /// 时间列表
        /// </summary>
        public dynamic timeList { get; set; }
    }
}