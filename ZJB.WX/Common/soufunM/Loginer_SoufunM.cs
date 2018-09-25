
using ZJB.Core.Utilities;
namespace ZJB.WX.Common.xms
{
    public class Loginer_SoufunM
    {
        private const string loginApi = "http://agentappnew.3g.fang.com/http/agentservice.jsp?";
        private const string paramApi = "messagename=login&username={0}&pwd={1}";

        internal static string Login(string username, string password)
        {
            string response = HttpUtility.GetString(loginApi + string.Format(paramApi, username, password));
//            if (!response.Contains("登录成功")) return null;
            return response;
        }
    }
}