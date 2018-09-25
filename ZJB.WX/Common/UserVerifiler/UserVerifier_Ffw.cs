using System.Net;
using System.Text;

namespace ZJB.WX.Common.UserVerifiler
{
    public class UserVerifier_Ffw
    {
        public const string loginAPi =
          "http://www.ffw.com.cn/passport/login.php?account={0}&pwd={1}&jcallback=log3&jsonp=jsonp1429241760013&fwd=http%3A%2F%2Fqz.2s.ffw.com.cn%2F&_=1429241792798";

        public static bool IsValidate(string username, string password, WebProxy proxy = null)
        {
            var webClient = new WebClient();
            webClient.Headers.Set("User-Agent",
                "Mozilla/5.0 (Windows NT 6.1; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/39.0.2171.95 Safari/537.36");
            byte[] bytes = webClient.DownloadData(string.Format(loginAPi, username, password));
            string response = Encoding.UTF8.GetString(bytes);
            if (response.Contains("\\u9519\\u8bef"))
            {
                //用户名或密码错误
                return false;
            }
            if (!response.Contains("\"ret\":\"ok\"")) return false;
            string cookie = webClient.ResponseHeaders.Get("Set-Cookie");
            return true;
        }
    }
}
