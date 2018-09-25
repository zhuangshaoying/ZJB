using System.Runtime.Serialization;
using ZJB.Core.Utilities;

namespace ZJB.Api.Models
{
    [DataContract]
    public class ApiResponse
    {
        public ApiResponse(Meta meta, dynamic response=null, int? totalSize = 0)
        {
          
            Meta = meta;
            Response = response;
            TotalSize = totalSize;
        }
        public ApiResponse()
        {
          
        }

        [DataMember(Name = "meta")]
       
        public Meta Meta { get; set; }

        [DataMember(Name = "response")]
        public dynamic Response { get; set; }

        [DataMember(Name = "totalsize")]
        public int? TotalSize { get; set; }
    }
}
