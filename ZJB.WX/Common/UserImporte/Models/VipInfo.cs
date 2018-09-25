namespace ZJB.WX.Common.UserImporte.Models
{

    public class VipResponse
    {
        public int respCode { get; set; }
        public string respData { get; set; }
    }

    public class WltInfoResponse
    {
        public Wltinfo[] wltInfo { get; set; }
        public string company { get; set; }
    }

    public class Wltinfo
    {
        public long wltId { get; set; }
        public long userId { get; set; }
        public int productTypeid { get; set; }
        public int productId { get; set; }
        public long packageId { get; set; }
        public int[] dispcateList { get; set; }
        public int[] dispcityList { get; set; }
        public int[] dispAreaList { get; set; }
        public int[] dispBizAreaList { get; set; }
        public int state { get; set; }
        public long openDate { get; set; }
        public long endDate { get; set; }
        public bool isTrial { get; set; }
        public string orderId { get; set; }
        public Bizparamsobj bizParamsObj { get; set; }
        public Configparamsobj configParamsObj { get; set; }
        public long priceItemId { get; set; }
    }

    public class Bizparamsobj
    {
        public Map map { get; set; }
    }

    public class Map
    {
        public string _10 { get; set; }
        public string _8 { get; set; }
        public string _14 { get; set; }
        public string _11 { get; set; }
        public string _17 { get; set; }
        public string _16 { get; set; }
    }

    public class Configparamsobj
    {
        public Map1 map { get; set; }
    }
    public class Map1
    {
        public int bindphone { get; set; }
    }


    public class ContactInfo
    {
        public string ContactName { get; set; }
        public string PhoneNum { get; set; }
    }

}
