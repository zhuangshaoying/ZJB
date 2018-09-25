using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ZJB.Api.Models
{
    public class StatModel
    {
        /// <summary>
        /// 统计名
        /// </summary>
        public string StatName { get; set; }
        /// <summary>
        /// 日期
        /// </summary>
        public DateTime Time { get; set; }
        /// <summary>
        /// 数量
        /// </summary>
        public int Count { get; set; }
        /// <summary>
        /// 成功数
        /// </summary>
        public int SuccessCount { get; set; }
        /// <summary>
        /// 失败数
        /// </summary>
        public int FailCount { get; set; }
        /// <summary>
        /// 网站名称
        /// </summary>
        public string SiteName { get; set; }
        /// <summary>
        /// 报表tip内容
        /// </summary>
        public string TipContent { get; set; }
    }
    public class UserTaskStat
    {
        /// <summary>
        /// 任务累计积分
        /// </summary>
        public int PointsCount { get; set; }
        /// <summary>
        /// 完成任务数
        /// </summary>
        public int CompleteCount { get; set; }
        /// <summary>
        /// 用户积分
        /// </summary>
        public int UserPoints { get; set; }
    }
    public class UserTaskSignStat {
        /// <summary>
        /// 今日是否签到一
        /// </summary>
        public int TodaySign { get; set; }
        /// <summary>
        /// 今日是否签到二
        /// </summary>
        public int TodaySign_2 { get; set; }
        /// <summary>
        /// 累计签到
        /// </summary>
        public int SignCount { get; set; }
        /// <summary>
        /// 所有用户总签到数
        /// </summary>
        public int AllSignCount { get; set; }
    }
    public class UserSignTop
    {
        public int UserId { get; set; }
        /// <summary>
        /// 用户名
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 头像
        /// </summary>
        public string Portrait { get; set; }
        public DateTime AddTime { get; set; }
        /// <summary>
        /// 累计签到x天
        /// </summary>
        public int SignCount { get; set; }
        /// <summary>
        /// 排行
        /// </summary>
        public int TopIndex { get; set; }
       
    }
    /// <summary>
    /// 今日签到右侧统计
    /// </summary>
    public class SignRightStat
    {
        public int TodaySignCount { get;set; }
        public int YesterdaySignCount { get;set; }
        /// <summary>
        /// 我；累计签到天数
        /// </summary>
        public int MySignCount { get; set; }
    }
}
