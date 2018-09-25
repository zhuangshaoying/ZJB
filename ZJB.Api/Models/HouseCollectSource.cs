using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MongoDB.Bson.Serialization.Attributes;

namespace ZJB.Api.Models
{
    public  class HouseCollectSource
    {
        [BsonId]
        public string id { get; set; }
        public string Source { get; set; }
        public int City { get; set; }
    }
}
