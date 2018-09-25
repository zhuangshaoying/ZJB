using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ZJB.Api.Models
{
    public class DynamicListReq
    {
        public DynamicListReq()
        {
            LastTime = new DateTime(9999, 1, 1);
            PageSize = 10;
            IsGetSupport = 0;
            FirstComming = 1;
        }
        public int UserId { get; set; }
        public DateTime LastTime { get; set; }
        public int PageSize { get; set; }
        public int CityId { get; set; }
        public int FirstComming { get; set; }
        /// <summary>
        /// 是否获取赞列表
        /// </summary>
        public int IsGetSupport { get; set; }
    }
}
