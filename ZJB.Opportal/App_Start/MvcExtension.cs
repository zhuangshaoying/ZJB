using System.Net.Http;
using ZJB.Api.Models;
namespace System.Web.Mvc
{
    public static class MvcExtension
    {

        public static PublicUserModel GetLoginUser(this Controller context)
        {
            String saveKey = System.Configuration.ConfigurationManager.AppSettings["AuthSaveKey"];
            if (String.IsNullOrEmpty(saveKey))
            {
                saveKey = "WXLoginedUser";
            }
            if (context.Session!=null&&context.Session[saveKey] != null)
            {
                return (PublicUserModel)context.Session[saveKey];
            }
            else if (context.Request.Cookies[saveKey] != null)
            {
                int userId = 0;
                int.TryParse(ZJB.Core.Utilities.CryptoUtility.TripleDESDecrypt(context.Request.Cookies[saveKey].Value), out userId);
                ZJB.Api.BLL.UserBll userBll = new ZJB.Api.BLL.UserBll();
                return userBll.GetUserById(userId);
            }
            return null;
        }
    }
}