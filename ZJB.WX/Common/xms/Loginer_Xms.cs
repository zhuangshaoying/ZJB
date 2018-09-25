using System;
using System.IO;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using ZJB.Core.Logging;


namespace ZJB.WX.Common.xms
{
    public class Loginer_Xms
    {
        private const string loginApi = "http://xms.4846.com/ajax/login.do";

        private const string loginParam =
            "phone={0}&userPwd={1}";
       
        public static string Login(string username, string password, WebProxy proxy = null)
        {
           
            try
            {
                System.Net.ServicePointManager.Expect100Continue = false;
                var request = (HttpWebRequest)WebRequest.Create(loginApi);
                request.Method = "POST";
                request.AllowAutoRedirect = false;
                request.UserAgent =
                    "Mozilla/5.0 (Windows NT 6.1; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/39.0.2171.95 Safari/537.36";
                request.ContentType = "application/x-www-form-urlencoded";
                if (proxy != null)
                {
                    request.Proxy = proxy;
                }
                Stream stream = request.GetRequestStream();
                byte[] formData = Encoding.UTF8.GetBytes(string.Format(loginParam, username, password));
                stream.Write(formData, 0, formData.Length);
                stream.Close();
                WebHeaderCollection response_headers = request.GetResponse().Headers;

                string cookie = response_headers.Get("Set-Cookie");
                if (!cookie.Contains("userInfo")) return null;
                Match m = Regex.Match(cookie, "(userInfo=.*?;)");
                if (!m.Success) return null;
                cookie = m.Groups[1].Value;
                return cookie;
            }
            catch (Exception e)
            {
                return e.Message;
            }
        }
     
    }
}