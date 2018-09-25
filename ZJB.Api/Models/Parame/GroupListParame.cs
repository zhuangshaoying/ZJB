using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZJB.Api.Models
{
    public class GroupListParame
    {
        public int UserId { get; set; }
        /// <summary>
        /// //0 是所有群组列表 1带字母搜索的群组列表 2 我的群组列表
        /// </summary>
        public int SearchType { get; set; }
        public string KeyWord { get; set; }
        public int PageSize { get; set; }
        public int LastId { get; set; }
        public int Status { get; set; }
    }
}
