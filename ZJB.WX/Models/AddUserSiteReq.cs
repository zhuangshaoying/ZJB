using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ZJB.WX.Models
{
    public class AddUserSiteReq
    {
        /// <summary>
        /// 站点id
        /// </summary>
        public int webBasicId { get; set; }

        /// <summary>
        /// 用户名
        /// </summary>
        public string loginName { get; set; }

        /// <summary>
        /// 密码
        /// </summary>
        public string loginPwd { get; set; }
        /// <summary>
        /// 设置为默认 扩展用
        /// </summary>
        public bool isDefault { get; set; }
    }
}