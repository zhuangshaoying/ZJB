using System.Web;

namespace ZJB.Web.PermanentlyMoved
{
    public class PermanentlyMovedUtility
    {
        public static void Redirect(string url)
        {
            HttpResponse response = HttpContext.Current.Response;
            response.Clear();
            response.StatusCode = 301;
            response.Status = "301 Moved Permanently";
            response.AddHeader("Location", url);
            response.End();
        }
    }
}
