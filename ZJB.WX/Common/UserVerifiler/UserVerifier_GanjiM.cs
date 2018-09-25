using System;
using System.Collections.Specialized;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using ZJB.Core.Utilities;

namespace ZJB.WX.Common.UserVerifiler
{
    public class UserVerifier_GanjiM
    {
        private const string loginApi = "http://mobds.ganji.cn/users/?password={0}&loginName={1}";

        public static NameValueCollection GetAppHeaders(string gjinterface, string userid, string token)
        {
            var headers = new NameValueCollection
            {
                {"customerId", "871"}, //
                {"clientAgent", "Sony Xperia"}, //
                {"versionId", "2.5.0"}, //
                {"model", "Generic/AnyPhone"}, //
                {"userId", userid}, //
                {"agency", "baidu02"}, //
                {"clientTest", "false"}, //
//                {"User-Agent", " Dalvik/1.6.0 (Linux; U; Android 4.2.2; Sony Xperia Build/JDQ39E)"}, //
                {"GjData-Version", "1.0"}, //
            };
            if (gjinterface != null)
            {
                headers.Add("interface", gjinterface);
                if (gjinterface == "UploadImages")
                {
                    headers.Add("contentformat", "bin");
                }
            }
            if (token != null)
            {
                headers.Add("Token", token);
            }
            return headers;
        }

        private static WebHeaderCollection GetWebHeaderCollection(string gjinterface, string userId)
        {
            NameValueCollection headers = GetAppHeaders(gjinterface, userId,
                null);
            var headerCollection = new WebHeaderCollection();
            if (headers == null) return headerCollection;
            foreach (string key in headers.Keys)
            {
                headerCollection.Set(key, headers[key]);
            }
            return headerCollection;
        }

        private static string getUserId(string username)
        {
            NameValueCollection headers = GetAppHeaders("userRegister", "",
                null);
            string base64Code =
                System.Convert.ToBase64String(Encoding.UTF8.GetBytes(username + new Random().Next(0, 1000)));
            headers.Add("androidId", base64Code);
            string json = HttpUtility.GetString("http://mobds.ganji.cn/datashare/", headers);
            return Regex.Match(json, "userid\":\"(\\w+)").Groups[1].Value;
        }

        public static bool IsValidate(string username, string password, WebProxy proxy = null)
        {
            string ganjiuserId = getUserId(username);
            var webClient = new WebClient {Headers = GetWebHeaderCollection("userLogin", ganjiuserId)};
            if (proxy != null)
            {
                webClient.Proxy = proxy;
            }
            byte[] bytes = webClient.DownloadData(string.Format(loginApi, password, username));
            string response = Encoding.UTF8.GetString(bytes);
            return response.Contains("LoginId");
        }
    }
}