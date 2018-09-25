using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ZJB.Api.Entity;
namespace ZJB.Api.Models
{
    public class RefreshLogModel:RefreshLog
    {
        public string SiteName { get; set; }
        public int PlanNo { get; set; }
        /// <summary>
        /// 用户看到的提示信息，原表的Msg只给后台管理员看
        /// </summary>
        public string ClientMsg { get; set; }
    }
}
