using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ZJB.Api.Models
{
    public class RefreshLogListReq
    {
        public RefreshLogListReq()
        {
            pi = 1;
            ps = 10;
        }
        public int pi { get; set; }
        public int ps { get; set; }
        public int UserId { get; set; }
        public int SiteId { get; set; }
        public string SiteUserName { get; set; }
        /// <summary>
        /// 刷新结果 0不限 1成功 -1失败
        /// </summary>
        public int ViewData { get; set; }
        /// <summary>
        /// 计划号
        /// </summary>
        public int PlanNo { get; set; }
        public DateTime? Time { get; set; }

        public int RefreshMode { get; set; }
    }
}
