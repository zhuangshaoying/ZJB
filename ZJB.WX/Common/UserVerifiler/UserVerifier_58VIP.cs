using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using Newtonsoft.Json;
using ZJB.Core.Utilities;

namespace ZJB.WX.Common.UserVerifiler
{
    internal class UserVerifier_58VIP
    {
        private const string loginApi = "https://passport.58.com/douilogin";
        private const string mobileLoginApi = "https://passport.58.com/douimobilelogin";
        private const string getVipInfo = "http://web.bangbang.58.com/vipinfo/wltInfo";

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
            if (!cookie.Contains("PPU="))
            {
                return false;
            }
            string uid = Regex.Match(cookie, "UID=(\\d+)").Groups[1].Value;
            cookie = CookieParser.ParseSetCookie(cookie);
            cookie += ";uid=" + uid;
            List<Wltinfo> wltInfos = GetWltinfoList(cookie);
            return wltInfos != null;
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

        public static List<Wltinfo> GetWltinfoList(string cookie)
        {
            var collection = new NameValueCollection
            {
                {"Cookie", cookie}
            };
            string result = HttpUtility.GetString(getVipInfo, collection);
            try
            {
                var response = JsonConvert.DeserializeObject<VipResponse>(result);
                if (response == null || string.IsNullOrWhiteSpace(response.respData))
                {
                    return null;
                }
                var r = JsonConvert.DeserializeObject<WltInfoResponse>(response.respData);
                if (r == null || r.wltInfo == null || r.wltInfo.Length == 0)
                {
                    return null;
                }
                return r.wltInfo.ToList();
            }
            catch (Exception e)
            {
                return null;
            }
        }

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
//            public Bizparamsobj bizParamsObj { get; set; }
//            public Configparamsobj configParamsObj { get; set; }
            public long priceItemId { get; set; }
        }
    }
}