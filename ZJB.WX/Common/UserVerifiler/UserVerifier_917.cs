using System.IO;
using System.Net;
using System.Text;

namespace ZJB.WX.Common.UserVerifiler
{
    public class UserVerifier_917
    {
        private const string loginApi = "http://www.917.com/api/user.html";
        private const string loginParam = "act=webLogin&txtUserName={0}&txtPassword={1}";

        public static bool IsValidate(string username, string password, WebProxy proxy = null)
        {
            var webClient = new WebClient();
            webClient.Headers.Set("User-Agent",
                "Mozilla/5.0 (Windows NT 6.1; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/39.0.2171.95 Safari/537.36");
            webClient.DownloadData(loginApi);
            var request = (HttpWebRequest) WebRequest.Create(loginApi);
            if (proxy != null)
            {
                request.Proxy = proxy;
            }
            request.Method = "POST";
            request.Headers.Set("X-Requested-With", "XMLHttpRequest");
            request.AllowAutoRedirect = false;
            request.UserAgent =
                "Mozilla/5.0 (Windows NT 6.1; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/39.0.2171.95 Safari/537.36";
            request.ContentType = "application/x-www-form-urlencoded";
            Stream stream = request.GetRequestStream();
            byte[] formData = Encoding.UTF8.GetBytes(string.Format(loginParam, username, password));
            stream.Write(formData, 0, formData.Length);
            stream.Close();
            WebResponse response = request.GetResponse();
            Stream response_stream = response.GetResponseStream();
            if (response_stream == null)
            {
                return false;
            }
            var readStream = new StreamReader(response_stream);
            string html = readStream.ReadToEnd();
            return html.Contains("登录成功");
        }
    }
}