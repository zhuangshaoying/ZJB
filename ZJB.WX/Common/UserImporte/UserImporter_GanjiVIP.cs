//using Plugins.Helper;
//using Plugins.Loginer;
using System;
using System.Collections.Specialized;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using Newtonsoft.Json;
using ZJB.Core.Utilities;
using ZJB.WX.Common.UserImporte.Models;
using ZJB.WX.Common.UserImporte.Models.Ganji;

namespace ZJB.WX.Common.UserImporte
{
    public  class UserImporter_GanjiVIP : User_Importer
    {
        private const string basiApi = "http://mobds.ganji.cn/datashare/";
        private const string prefixTx = "http://image.ganjistatic1.com/";
        protected override string version()
        {
            return "1.1";
        }

        public override ImportedUser Run(string uname, string pwd)
        {
            string cookie = Login(uname, pwd);
            ImportedUser result = new ImportedUser();
            if (cookie == null)
            {
                return null;
            }
            string LoginId = Regex.Match(cookie, "LoginId=(\\d+);").Groups[1].Value;
            var data = new NameValueCollection
            {
                {"feeType", "1"},
                {"cityScriptIndex", "1001"},
                {"pg", "2"},
                {"categoryId", "7"},
                {"ucUserId", LoginId},
                {"pi", "1"},
            };
            string json = AppPost(basiApi, data, cookie, null, "GetUserPrivilegeByUserId");
            try
            {
                
                UserInfo userInfo = JsonConvert.DeserializeObject<UserInfo>(json);
                string touXiang = userInfo.baseInfo.avatar.Contains("http://") ? userInfo.baseInfo.avatar : (prefixTx + userInfo.baseInfo.avatar);
                result.Company = userInfo.baseInfo.companyName;
                result.UserId = LoginId;
                result.UserName = userInfo.realName;
                result.Portrait = touXiang;
                result.Tel = userInfo.phone;
                return result;
            }
            catch (Exception)
            {
                return null;
            }
        }
        private const string loginApi = "http://mobds.ganji.cn/users/?password={0}&loginName={1}";

        private static string Login(string username, string password, WebProxy proxy = null)
        {
            try
            {
                var webClient = new WebClient { Headers = GetWebHeaderCollection("userLogin") };
                //                proxy = new WebProxy("localhost",8888);
                if (proxy != null)
                {
                    webClient.Proxy = proxy;
                }
                byte[] bytes = webClient.DownloadData(string.Format(loginApi, password, username));
                string response = Encoding.UTF8.GetString(bytes);
                if (!response.Contains("LoginId")) return response;
                WebHeaderCollection response_headers = webClient.ResponseHeaders;
                string token = response_headers.Get("Token");
                string sessionId = response_headers.Get("sessionId");

                Match m = Regex.Match(response, "LoginId\":(\\d+).*?wapSessionId\":\"(.*?)\"");
                if (!m.Success) return null;
                string cookie = "Token=" + token + ";sessionId=" + sessionId + ";LoginId=" + m.Groups[1].Value +
                                ";wapSessionId=" + m.Groups[2].Value + ";";
                return cookie;
            }
            catch (WebException)
            {
                return "WebException";
            }
        }
        private static WebHeaderCollection GetWebHeaderCollection(string gjinterface)
        {
            NameValueCollection headers = GetAppHeaders(gjinterface);
            var headerCollection = new WebHeaderCollection();
            if (headers == null) return headerCollection;
            foreach (string key in headers.Keys)
            {
                headerCollection.Set(key, headers[key]);
            }
            return headerCollection;
        }
        private static NameValueCollection GetAppHeaders(string gjinterface, string token = null)
        {
            var headers = new NameValueCollection
            {
                {"customerId", "801"}, //
                {"clientAgent", "Sony Xperia"}, //
                {"versionId", "5.11.0"}, //
                {"model", "Generic/AnyPhone"}, //
                {"userId", "E8D394B20432BC781A58BC2C2BFAC3AB"}, //
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

        private static string AppPost(string postApi, NameValueCollection data, string cookie, WebProxy proxy = null,
         string interfaca = null)
        {
            string token = Regex.Match(cookie, "Token=(\\w+);").Groups[1].Value;
            var headers = GetAppHeaders(interfaca, token);
            //            proxy = new WebProxy("localhost", 8888);
            string response = HttpUtility.PostString(postApi, data, headers, proxy);
            return response;
        }
    }
}
