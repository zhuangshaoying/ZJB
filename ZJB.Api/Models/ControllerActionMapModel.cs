using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ZJB.Api.Models
{
    public class ControllerActionMapModel
    {
        public string Controller { get; set; }
        public string Action { get; set; }
        /// <summary>
        /// 是否重点功能
        /// </summary>
        public int Status { get; set; }
        public string FunctionName{get;set;}
        /// <summary>
        /// 访问次数
        /// </summary>
        public int AccessCount { get; set; }
    }
}
