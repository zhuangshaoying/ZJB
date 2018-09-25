using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ZJB.Api.Models
{
    public class DynamicSupportModel
    {
        public int Id { get; set; }
        public int DynamicId { get; set; }
        public int UserId { get; set; }
        public string UserName { get; set; }
        public int Status { get; set; }
        public DateTime AddTime { get; set; }
    }
}
