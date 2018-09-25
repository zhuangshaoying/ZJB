using System.Collections.Generic;
using System.Xml.Serialization;

namespace ZJB.Web.SiteGuard
{
    [XmlRoot("SiteGuardRules")]
    public class SiteGuardRules : List<SiteGuardRule>
    {

    }

    public class SiteGuardRule
    {
        public int TimePeriod { get; set; }
        public int MaxRequests { get; set; }
        public string HttpMethod { get; set; }
        public int BlockTime { get; set; }
        public BlockType BlockType { get; set; }
        public string Path { get; set; }
        public string Message { get; set; }
    }

    public enum BlockType
    {
        Forbidden,
        NotFound,
        CloseConnection
    }
}
