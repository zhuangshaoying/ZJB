using System.Collections.Specialized;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using Newtonsoft.Json;
using ZJB.Core.Utilities;
using ZJB.WX.Common.UserImporte.Models;
using ZJB.WX.Common.xms;

namespace ZJB.WX.Common.UserImporte
{
    internal class UserImporter_ZJB : User_Importer
    {
        private const string userInfoApi = "http://my.ZJB.com/JJR/FzHzHelper/Invite.aspx";


        protected override string version()
        {
            return "1.1";
        }

        public override ImportedUser Run(string uname, string pwd)
        {
            string cookie = Login(uname, pwd);
            if (cookie == null)
            {
                return null;
            }
            string html = HttpUtility.GetString(userInfoApi, Helper_ZJB.GetHeaders(cookie));
            string info = Regex.Match(html, "name='info' value='(.*?)'").Groups[1].Value;
            string json = CryptoUtility.TripleDESDecrypt(info);
            return JsonConvert.DeserializeObject<ImportedUser>(json);
        }
        private const string loginApi = "http://passport.ZJB.com/WebLogin.aspx";
        //        protected abstract string DoPost(P_Poster_GetHouseBasicInfo_Result basicInfo, PublicUser publicUser,
        //            string cookie, int siteId);


        private static string Login(string username, string password)
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

       
    }
}