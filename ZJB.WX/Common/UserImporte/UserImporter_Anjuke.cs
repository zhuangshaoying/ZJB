using System;
using System.Collections.Specialized;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using ZJB.Core.Utilities;
using ZJB.WX.Common.UserImporte.Models;

namespace ZJB.WX.Common.UserImporte
{
    internal class UserImporter_Anjuke : User_Importer
    {
        private const string userInfoApi = "http://my.anjuke.com/ajkbroker/broker/brokerinfo";

        private const string loginApi = "http://member.anjuke.com/api/login/submit?username={0}&password={1}" +
                                        "&remember=true&callback=window.user.callbackDetail";

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
            string html = HttpUtility.GetString(userInfoApi, GetHeaders(cookie));
            const string regex = "个人头像.*?src=\"(?<icon>.*?)\"[\\w\\W]*?" +
                                 "用户名：</strong>(?<userid>.*?)<[\\w\\W]*?" + "真实姓名：</strong>(?<truename>.*?)<[\\w\\W]*?" +
                                 "手机号码：</strong>(?<tel>\\d+)[\\w\\W]*?" +
                                 "电子邮箱：</strong>(?<email>.*?)<[\\w\\W]*?" +
                                 "所在城市：</strong>(?<city>.*?)<[\\w\\W]*?" + "工作区域：</strong>(?<area>.*?)<[\\w\\W]*?" +
                                 "所属公司：</strong>(?<company>.*?)<[\\w\\W]*?" + "所属门店：</strong>(?<store>.*?)<";
            Match m = Regex.Match(html, regex);

            if (!m.Success)
            {
                return null;
            }

            string icon = m.Groups["icon"].Value;
            string userid = m.Groups["userid"].Value;
            string truename = m.Groups["truename"].Value;
            string tel = m.Groups["tel"].Value;
            string city = m.Groups["city"].Value;
            string company = m.Groups["company"].Value;
            string store = m.Groups["store"].Value;
            return new ImportedUser
            {
                City = city,
                Company = company,
                Store = store,
                Tel = tel,
                UserId = "Ajk_" + userid,
                UserName = truename,
                Portrait = icon
            };
        }

        public static string Login(string username, string password)
        {
            var webClient = new WebClient();
            webClient.Headers.Set("User-Agent",
                "Mozilla/5.0 (Windows NT 6.1; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/39.0.2171.95 Safari/537.36");
            byte[] bytes = webClient.DownloadData(string.Format(loginApi, username, password));
            string response = Encoding.UTF8.GetString(bytes);
            if (!response.Contains("{\"status\":1")) return null;
            string cookie = webClient.ResponseHeaders.Get("Set-Cookie");
            MatchCollection ms = Regex.Matches(response, "(http%3A%2F%2Fmy\\.anjuke\\.com%2Fuser%2Fverify.*?)\"");
            string tmp =
                Regex.Replace(cookie, "([^=]+=deleted; )?(?:expires|path)=.*?domain=.*?com,?", "")
                    .Replace(" httponly", "");
            foreach (Match m in ms)
            {
                string url = Uri.UnescapeDataString(m.Groups[1].Value).Replace("\\", "");
                webClient.Headers.Set("Cookie", tmp);
                webClient.Headers.Set("User-Agent",
                    "Mozilla/5.0 (Windows NT 6.1; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/39.0.2171.95 Safari/537.36");
                webClient.DownloadData(url);
                tmp = webClient.ResponseHeaders.Get("Set-Cookie");
                tmp =
                    Regex.Replace(tmp, "^aQQ_ajkauthinfos=.*?;|([^=]+=deleted; )?(?:expires|path)=.*?domain=.*?com,?",
                        "").Replace(" httponly", "");

                url = "http://my.anjuke.com/user/broker/brokerhome";
                webClient.Headers.Set("Cookie", tmp);
                webClient.Headers.Set("User-Agent",
                    "Mozilla/5.0 (Windows NT 6.1; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/39.0.2171.95 Safari/537.36");
                webClient.DownloadData(url);
                string _cookie2 = webClient.ResponseHeaders.Get("Set-Cookie");
                if (_cookie2 != null)
                {
                    cookie = Regex.Replace(_cookie2, "([^=]+=deleted; )?(?:expires|path)=.*?domain=.*?com,?", "");
                }
            }
            return cookie;
        }

        private static NameValueCollection GetHeaders(string cookie)
        {
            return new NameValueCollection
            {
                {"Cookie", cookie},
                {
                    "User-Agent",
                    "Mozilla/5.0 (Windows NT 6.1; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/39.0.2171.95 Safari/537.36"
                },
            };
        }
    }
}