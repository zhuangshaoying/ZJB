using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ZJB.Api.Entity;

namespace ZJB.WX.Models
{
    public class UserModels
    {
        public int UserId { get; set; }
        public string UserName { get; set; }
        public string Tel { get; set; }
        public string Company { get; set; }
        public string Store { get; set; }
        public string Portrait { get; set; }
        public int InviteUid { get; set; }
   
    }

   
}