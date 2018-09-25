using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ZJB.Api.Models
{
    public class StatReq
    {
        public int ps { get; set; }
        public DateTime currentTime { get; set; }
        /// <summary>
        /// 开始时间
        /// </summary>
        public DateTime? startTime { get; set; }
        /// <summary>
        /// 截止时间
        /// </summary>
        public DateTime? endTime { get; set; }
        /// <summary>
        /// 0 所有 1成功 -1 失败
        /// </summary>
        public int status { get; set; }
        /// <summary>
        ///  
        /// </summary>
        public string _Controller { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string _Action { get; set; }
        public int UserId { get; set; }
    }

}
