using System.Collections.Specialized;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;

namespace ZJB.WX.Common.UserVerifiler
{
    internal class UserVerifier_58
    {
        private const string loginApi = "https://passport.58.com/douilogin";
        private const string mobileLoginApi = "https://passport.58.com/douimobilelogin";

        public static bool IsValidate(string username, string password)
        {
            bool isMobile = Regex.Match(username, "^\\d{11}$").Success;
            string api = isMobile ? mobileLoginApi : loginApi;
            string uname = isMobile ? "pptmobile" : "pptusername";
            string upass = isMobile ? "pptmobilepassword" : "pptpassword";
            var data = new NameValueCollection
            {
                {"domain", "58.com"},
                {"callback", "handleLoginResult"},
                {"sysIndex", "4"},
                {uname, username},
                {upass, password},
            };
            var webClient = new WebClient {Headers = GetWebHeaderCollection()};
            byte[] bytes = webClient.UploadValues(api, "POST", data);
            string response = Encoding.UTF8.GetString(bytes);
            if (!response.Contains("success")) return false;
            WebHeaderCollection response_headers = webClient.ResponseHeaders;
            string cookie = response_headers.Get("Set-Cookie");
            return cookie.Contains("PPU=");
        }

        private static WebHeaderCollection GetWebHeaderCollection()
        {
            NameValueCollection headers = GetHeaders("");
            var headerCollection = new WebHeaderCollection();
            if (headers == null) return headerCollection;
            foreach (string key in headers.Keys)
            {
                headerCollection.Set(key, headers[key]);
            }
            return headerCollection;
        }

        private static NameValueCollection GetHeaders(string cookie)
        {
            return new NameValueCollection
            {
                {
                    "Referer",
                    ""
                },
                {"Origin", "http://p.webapp.58.com"},
                {"Content-Type", "application/x-www-form-urlencoded"},
                {"Accept", "text/html,application/xhtml+xml,application/xml;q=0.9,*/*;q=0.8"},
                {"X-Requested-With", "com.wuba"},
                {
                    "User-Agent",
                    "Mozilla/5.0 (Linux; U; Android 4.3; zh-cn; Xperia Z - 4.3 - API 18 - 1080x1920_1 Build/JLS36G) AppleWebKit/534.30 (KHTML, like Gecko) Version/4.0 Mobile Safari/534.30"
                },
                {"Accept-Encoding", "deflate"},
                {"Accept-Charset", "utf-8, iso-8859-1, utf-16, *;q=0.7"},
                {"Cookie", cookie},
            };
        }
    }
}