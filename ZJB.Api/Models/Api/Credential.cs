using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using Newtonsoft.Json;
using ZJB.Api.Common;

namespace ZJB.Api.Models
{

    public class Credential
    {
        public string Name { get; set; }
        public string Token { get; set; }
        public int UserID { get; set; }
        public int CityId { get; set; }
        [JsonConverter(typeof (DateTimeConverter))]
        public DateTime ExpirationDate { get; set; }

        public string Icon { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }

 
 
       
        
     
    }

  
}