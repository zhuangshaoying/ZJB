using System.Net;
using System.Text;

namespace ZJB.WX.Common.UserVerifiler
{
    public class UserVerifier_Anjuke
    {
        private const string loginApi = "http://member.anjuke.com/api/login/submit?username={0}&password={1}" +
                                        "&remember=true&callback=window.user.callbackDetail";

        public static bool IsValidate(string username, string password)
        {
            var webClient = new WebClient();
            webClient.Headers.Set("User-Agent",
                "Mozilla/5.0 (Windows NT 6.1; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/39.0.2171.95 Safari/537.36");
            byte[] bytes = webClient.DownloadData(string.Format(loginApi, username, password));
            string response = Encoding.UTF8.GetString(bytes);
            return response.Contains("{\"status\":1");
        }
    }
}