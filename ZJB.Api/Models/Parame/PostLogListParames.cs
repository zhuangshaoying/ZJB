using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ZJB.Api.Models
{
     public class PostLogListParames
    {
         public int houseId { get; set; }
         public int userId { get; set; }
         public DateTime? time { get; set; }
         public int siteId { get; set; }
         public int? status { get; set; }
         public string communityName { get; set; }
         public string title { get; set; }
         public int pageIndex { get; set; }
         public int pageSize { get; set; }

    }
}
