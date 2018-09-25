namespace ZJB.WX.Common.UserImporte.Models.Ganji
{

    public class UserInfo
    {
        public string realName { get; set; }
        public string phone { get; set; }
        public Baseinfo baseInfo { get; set; }
    }

    public class Baseinfo
    {
        public string username { get; set; }
        public string name { get; set; }
        public string companyName { get; set; }
        public string avatar { get; set; }
        public string bang_year { get; set; }
        public string store_url { get; set; }
        public string creditScore { get; set; }
        public string cityId { get; set; }
        public string district_id { get; set; }
        public string street_id { get; set; }
        public bool ICImage { get; set; }
        public bool businessCardImage { get; set; }
        public string cityName { get; set; }
        public string district_name { get; set; }
        public string street_name { get; set; }
        public string customerName { get; set; }
        public string customerAddress { get; set; }
        public string customerLatlng { get; set; }
        public string agentUserContactNum { get; set; }
        public string contactPhone { get; set; }
        public string contactName { get; set; }
        public int assessmentRemind { get; set; }
    }
}
