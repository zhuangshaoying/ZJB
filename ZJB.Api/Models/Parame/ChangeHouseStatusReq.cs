using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ZJB.Api.Models
{
    public class ChangeHouseStatusReq
    {
      
        public byte? ChangeToStatus { get; set; }

        public int UserId { get; set; }

        public string HouseIdsStr { get; set; }
    }
}
