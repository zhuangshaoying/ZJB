using System;
using System.ComponentModel;

namespace ZJB.Api.Models
{
    /// <summary>
    /// 楼盘排序
    /// </summary>
    public enum LouPanOrder
    {
        /// <summary>
        /// 楼盘修改时间倒序排序
        /// </summary>
        UpdateTimeDesc = 0,
        /// <summary>
        /// 点击率倒序排序
        /// </summary>
        HitsDesc = 1,
        /// <summary>
        /// 开盘时间倒序排序
        /// </summary>
        OpeningDateDesc = 2
     
      
    }
}
