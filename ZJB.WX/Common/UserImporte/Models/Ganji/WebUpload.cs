namespace ZJB.WX.Common.UserImporte.Models.Ganji
{
    internal class WebUpload
    {
        public Image[][] Property1 { get; set; }
    }

    public class Image
    {
        public string image { get; set; }
        public string thumb_image { get; set; }
        public string id { get; set; }
        public string description { get; set; }
    }
}