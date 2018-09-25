using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ZJB.Api.Models
{
    public enum PointsEnum
    {
        /// <summary>
        /// 上传头像
        /// </summary>
        First_UploadHead = 1,
        /// <summary>
        /// 绑定网站帐号
        /// </summary>
        First_BindSiteAccount = 2,
        /// <summary>
        /// 录入第一套房源
        /// </summary>
        First_AddHouse=3,
        /// <summary>
        /// 发布第一套房源
        /// </summary>
        First_RealseHouse=4,
        /// <summary>
        /// 第一次浏览采集的房源
        /// </summary>
        First_ViewCollectHouse=5,
        /// <summary>
        /// 第一次在Pc客户端登录
        /// </summary>
        First_Login_PC=6,
        /// <summary>
        /// 在APP第一次分享自己的房源 
        /// </summary>
        First_ShareHouse_App=7,
        /// <summary>
        /// 盒子赠送积分，每日上午8：00发放，有4个小时领取时间。
        /// </summary>
        EveryDay_Sign_8=8,
        /// <summary>
        /// 盒子赠送积分，每日下午16：00发放，有4个小时领取时间。
        /// </summary>
        EveryDay_Sign_16 = 9,
        /// <summary>
        /// 浏览一条采集的房源
        /// </summary>
        EveryDay_ViewCollectHouse=10,
        /// <summary>
        /// 在PC客户端在线每个小时+2分
        /// </summary>
        EveryDay_Online_PC=11,
        /// <summary>
        /// App分享一次
        /// </summary>
        EveryDay_ShareHouse_App = 12,
        /// <summary>
        /// 建议被管理员采纳
        /// </summary>
        Special_GreatSuggess=13,
        /// <summary>
        /// 在APP客户端登录
        /// </summary>
        First_Login_App=14,
        /// <summary>
        /// 发布一条房源
        /// </summary>
        EveryDay_RealseHouse = 15,
        /// <summary>
        /// 填写真实姓名
        /// </summary>
        First_EnrolnName = 16,
        /// <summary>
        /// 房源共享
        /// </summary>
        First_HouseShare = 17,
        /// <summary>
        /// 提醒设置
        /// </summary>
        First_HouseRemind = 18,
        /// <summary>
        /// 第一次查看房源共享列表
        /// </summary>
        First_HouseShareView=19
    }
}
