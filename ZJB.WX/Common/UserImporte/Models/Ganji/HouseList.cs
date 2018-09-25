namespace ZJB.WX.Common.UserImporte.Models.Ganji
{
    public class HouseList
    {
        public int errCode { get; set; }
        public string msg { get; set; }
        public Page page { get; set; }
    }

    public class Page
    {
        public string count { get; set; }
        public Post[] posts { get; set; }
    }

    public class Post
    {
        public string type { get; set; }
        public string house_id { get; set; }
        public string title { get; set; }
        public string puid { get; set; }
        public string account_id { get; set; }
        public string post_at { get; set; }
        public string person { get; set; }
        public string phone { get; set; }
        public string district_name { get; set; }
        public string street_name { get; set; }
        public string image_count { get; set; }
        public string price { get; set; }
        public Area area { get; set; }
        public string listing_status { get; set; }
        public string city { get; set; }
        public string images { get; set; }
        public string huxing_shi { get; set; }
        public string huxing_ting { get; set; }
        public string xiaoqu { get; set; }
        public Price_Format price_format { get; set; }
        public object yesterday_count { get; set; }
        public int history_count { get; set; }
        public string detail_url { get; set; }
        public Refresh refresh { get; set; }
        public string thumb_img { get; set; }
    }

    public class Area
    {
        public string u { get; set; }
        public int v { get; set; }
    }

    public class Price_Format
    {
        public string v { get; set; }
        public string u { get; set; }
    }

    public class Refresh
    {
        public int rightnow { get; set; }
        public int book { get; set; }
        public int left { get; set; }
        public int refreshed { get; set; }
        public int ordering { get; set; }
    }
}