using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZJB.Api.Models
{
    public class GroupModel
    {
        public int Id { get; set; }
        public string GroupName { get; set; }
        public string Description { get; set; }
        public string Icon { get; set; }
        public int ShowType { get; set; }
        public int MemberCount { get; set; }
        public int MemberStatus { get; set; }
        public int MemberType { get; set; }
    }
}
