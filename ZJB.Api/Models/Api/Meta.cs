
using System.Runtime.Serialization;
namespace ZJB.Api.Models
{
    [DataContract]
    public class Meta
    {
        [DataMember(Name = "status")]
        public int Status { get; set; }
        [DataMember(Name = "msg")]
        public string Message { get; set; }
    }
}
