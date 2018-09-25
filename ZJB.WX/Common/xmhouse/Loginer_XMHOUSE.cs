using System.Collections.Specialized;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using ZJB.Core.Utilities;


namespace ZJB.WX.Common.xms
{
    public class Loginer_ZJB
    {
        private const string loginApi = "http://passport.ZJB.com/WebLogin.aspx";
        //        protected abstract string DoPost(P_Poster_GetHouseBasicInfo_Result basicInfo, PublicUser publicUser,
        //            string cookie, int siteId);


       
        internal static string Login(string username, string password)
        {
            string viewState =
                Helper_ZJB.GetViewState(HttpUtility.GetString("http://passport.ZJB.com/",
                    Helper_ZJB.GetWebHeaderCollection()));
            if (viewState == null)
            {
                return null;
            }
            //            Console.WriteLine("Login");
            var data = new NameValueCollection
            {
                {"__VIEWSTATE", viewState},
                {"ZJBUserName", username},
                {"XmmhousePassword", password},
                {"XmmhouseCheckCode", "8430"},
                {"toUrl", ""},
                {"SaveState", "on"},
            };
            var webClient = new WebClient { Headers = Helper_ZJB.GetWebHeaderCollection() };
            byte[] bytes = webClient.UploadValues(loginApi, "POST", data);
            string response = Encoding.UTF8.GetString(bytes);
            if (!response.Contains("location.href='http://my.ZJB.com/User_AllUser.aspx';},2000")) return null;
            WebHeaderCollection response_headers = webClient.ResponseHeaders;
            string cookie = response_headers.Get("Set-Cookie");

            Match m = Regex.Match(cookie, "ASP\\.NET_SessionId=(.*?);");
            if (!m.Success) return null;

            string uc1 = Regex.Match(response, "src=\"(http://bbs.ZJB.com/api/uc.php.*?)\"").Groups[1].Value;
            //            string uc2 = Regex.Match(response, "src=\"(http://passport.ZJB.com/api/Uc.php.*?)\"").Groups[1].Value;

            var webClient2 = new WebClient { Headers = Helper_ZJB.GetWebHeaderCollection() };
            //            webClient2.Headers = 
            webClient2.DownloadData(uc1);
            string cookie2 = webClient2.ResponseHeaders.Get("Set-Cookie");
            //            cookie2 = Regex.Match(cookie2, "(cdb_activationauth=.*?;)").Groups[1].Value + "cdb_cookietime=2592000; cdb_loginuser=" + username + ";";
            //            cookie2 = Regex.Match(cookie2, "(cdb_activationauth=.*?;)").Groups[1].Value + ";";
            if (cookie2 != null)
            {
                cookie2 = Regex.Replace(cookie2, "([^=]+=deleted; )?(?:expires|path)=.*?domain=.*?com,?", "");
            }
            //HttpUtility.GetString(uc2);
            cookie = "ASP.NET_SessionId=" + m.Groups[1].Value + ";" + cookie2;
            return cookie;
        }

//        private static WebHeaderCollection GetWebHeaderCollection()
//        {
//            NameValueCollection headers = GetHeaders("");
//            var headerCollection = new WebHeaderCollection();
//            if (headers == null) return headerCollection;
//            foreach (string key in headers.Keys)
//            {
//                headerCollection.Set(key, headers[key]);
//            }
//            return headerCollection;
//        }
//
//        private static string GetViewState(string html)
//        {
//            Match rm = Regex.Match(html, "id=\"__VIEWSTATE\" value=\"(.*?)\"");
//            return rm.Success ? rm.Groups[1].Value : null;
//        }

//        private static NameValueCollection GetHeaders(string cookie)
//        {
//            return new NameValueCollection
//            {
//                {
//                    "Referer",
//                    ""
//                },
//                {"Content-Type", "application/x-www-form-urlencoded"},
//                {"Accept", "text/html,application/xhtml+xml,application/xml;q=0.9,*/*;q=0.8"},
//                {"Accept-Encoding", "deflate"},
//                {"Accept-Charset", "utf-8, iso-8859-1, utf-16, *;q=0.7"},
//                {"Cookie", cookie},
//                {"User-Agent", "Mozilla/5.0 (Windows NT 6.1; WOW64; Trident/7.0; rv:11.0) like Gecko"},
//           };
//        }
    }
}