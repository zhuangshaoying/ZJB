using System.Text.RegularExpressions;
using ZJB.Core.Utilities;
using ZJB.WX.Common.UserImporte.Models;

namespace ZJB.WX.Common.UserImporte
{
    internal class UserImporter_Soufun : User_Importer
    {
        private const string loginApi = "http://agentappnew.3g.fang.com/http/agentservice.jsp?";
        private const string loginParamApi = "messagename=login&username={0}&pwd={1}";
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
            string icon = Regex.Match(cookie, "<photourl>(.*?)</photourl>").Groups[1].Value;
            string userid = Regex.Match(cookie, "<userid>(\\d+)</userid>").Groups[1].Value;
            string truename = Regex.Match(cookie, "<agentname>(.*?)</agentname>").Groups[1].Value;
            string tel = Regex.Match(cookie, "<mobilecode>(.*?)</mobilecode>").Groups[1].Value;
            string city = Regex.Match(cookie, "<city>(.*?)</city>").Groups[1].Value;
            string company = Regex.Match(cookie, "<comname>(.*?)</comname>").Groups[1].Value;
            return new ImportedUser
            {
                City = city,
                Company = company,
                Tel = tel,
                UserId = "Soufun_" + userid,
                UserName = truename,
                Portrait = icon
            };
        }

        private static string Login(string username, string password)
        {
            string response = HttpUtility.GetString(loginApi + string.Format(loginParamApi, username, password));
            if (!response.Contains("登录成功")) return null;
            return response;
        }
    }
}