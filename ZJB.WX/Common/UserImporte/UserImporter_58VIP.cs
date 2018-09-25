using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using Newtonsoft.Json;
using ZJB.Core.Utilities;
using ZJB.WX.Common.UserImporte.Models;

namespace ZJB.WX.Common.UserImporte
{
    public class UserImporter_58VIP : User_Importer
    {
        private const string loginApi = "https://passport.58.com/douilogin";
        private const string mobileLoginApi = "https://passport.58.com/douimobilelogin";
        private const string getVipInfo = "http://web.bangbang.58.com/vipinfo/wltInfo";
        private const string getcontactUrl = "http://web.bangbang.58.com/house/getcontactandphone?uid={0}";
        private const string my58Url = "http://my.58.com/{0}";

        protected override string version()
        {
            return "1.1";
        }

        public override ImportedUser Run(string uname, string pwd)
        {
            string uid;
            var result = new ImportedUser();
            string cookie = Login58Vip(uname, pwd, out uid);
            if (!string.IsNullOrEmpty(cookie))
            {
                string company;
                List<Wltinfo> wltInfos = GetWltinfoList(cookie, out company);

                if (wltInfos == null || wltInfos.Count == 0)
                {
                    //无对应权限
                    return null;
                }
                ContactInfo info = GetContactInfo(cookie, uid);
                if (info == null) return null;
                result.Company = company;
                result.UserId = uid;
                result.UserName = info.ContactName;
                result.Tel = info.PhoneNum;
                result.Portrait = GetUserIcon(uid);
            }
            return result;
        }

        private string Login58Vip(string username, string password, out string uid)
        {
            uid = "";
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
            //            var webClient = new WebClient {Headers = GetWebHeaderCollection(), Proxy = new WebProxy("localhost", 8888)};
            byte[] bytes = webClient.UploadValues(api, "POST", data);
            string response = Encoding.UTF8.GetString(bytes);
            if (!response.Contains("success")) return null;
            WebHeaderCollection response_headers = webClient.ResponseHeaders;
            string cookie = response_headers.Get("Set-Cookie");

            //            Match m = Regex.Match(cookie, "PPU=\"(.*?)\";");
            //            if (!m.Success) return null;
            //            cookie = "PPU=" + m.Groups[1].Value + ";id58=1;";

            if (!cookie.Contains("PPU="))
            {
                return null;
            }
            cookie = Regex.Replace(cookie, ";.*?,", ";") + ";id58=1";
            uid = Regex.Match(cookie, "UID=(\\d+)").Groups[1].Value;
            cookie += ";uid=" + uid;
            return cookie;
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
//                    "http://p.webapp.58.com/606/8/s5?s5&topcate=house&currentcate=zufang&id=8&location=606,,&geotype=baidu&geoia=,&formatsource=listpublish&os=android&ver=npost"
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
//                {"Accept-Language", "zh-CN, en-US"},
                {"Accept-Charset", "utf-8, iso-8859-1, utf-16, *;q=0.7"},
                {"Cookie", cookie},
            };
        }


        private string GetUserIcon(string userId)
        {
            string url = string.Format(my58Url, userId);
            string result = HttpUtility.GetString(url, Encoding.UTF8);
            try
            {
                string icon = Regex.Match(result, "<span class=\"tx\"><img src=\"(.*?)\"").Groups[1].Value;
                return icon;
            }
            catch (Exception)
            {
                return null;
            }
        }

        private ContactInfo GetContactInfo(string cookie, string userId)
        {
            var collection = new NameValueCollection
            {
                {"Cookie", cookie}
            };
            string url = string.Format(getcontactUrl, userId);
            string result = HttpUtility.GetString(url, Encoding.UTF8, collection);
            try
            {
                var response = JsonConvert.DeserializeObject<ContactInfo>(result);
                return response;
            }
            catch (Exception)
            {
                return null;
            }
        }

        private List<Wltinfo> GetWltinfoList(string cookie, out string company)
        {
            var collection = new NameValueCollection
            {
                {"Cookie", cookie}
            };
            string result = HttpUtility.GetString(getVipInfo, collection);
            company = "";
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
                company = r.company;
                return r.wltInfo.ToList();
            }
            catch (Exception)
            {
                return null;
            }
        }
    }
}