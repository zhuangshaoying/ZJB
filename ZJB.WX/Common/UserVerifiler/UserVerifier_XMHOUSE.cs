using System.Collections.Specialized;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using ZJB.Core.Utilities;
using ZJB.WX.Common.xms;

namespace ZJB.WX.Common.UserVerifiler
{
    public class UserVerifier_ZJB
    {
        private const string loginApi = "http://passport.ZJB.com/WebLogin.aspx";

        internal static bool IsValidate(string username, string password)
        {
            string viewState =
               Helper_ZJB.GetViewState(HttpUtility.GetString("http://passport.ZJB.com/",
                    Helper_ZJB.GetWebHeaderCollection()));
            if (viewState == null)
            {
                return false;
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
            if (!response.Contains("location.href='http://my.ZJB.com/User_AllUser.aspx';},2000")) return false;
            WebHeaderCollection response_headers = webClient.ResponseHeaders;
            string cookie = response_headers.Get("Set-Cookie");
            return cookie.Contains("NET_SessionId");
        }


     
   


     
    }
}