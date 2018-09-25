namespace ZJB.WX.Common.UserImporte.Models.Ganji
{
    internal class AppUpload
    {
    }

    public class GanjiResponse
    {
        public int Code { get; set; }
        public string Message { get; set; }
        public string Detail { get; set; }
        public string StatusCode { get; set; }
    }

    public class ImageResponse
    {
        public int error { get; set; }
        public string text { get; set; }
        public Info[] info { get; set; }
    }

    public class Info
    {
        public string url { get; set; }
        public string thumbUrl { get; set; }
        public int[] image_info { get; set; }
    }
}