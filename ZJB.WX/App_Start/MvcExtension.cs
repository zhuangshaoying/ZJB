using System.Net.Http;
using ZJB.Api.Models;
using System.Collections.Generic;
using ZJB.Core.Utilities;
using Newtonsoft.Json;
using ZJB.WX.Models;
using ZJB.Api.Entity;
using ZJB.Api.BLL;
using System.Linq;
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


            if (context.Session != null && context.Session[saveKey] != null)
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

        //public static PublicUserModel GetLoginUser(this Controller context)
        //{
        //    String saveKey = System.Configuration.ConfigurationManager.AppSettings["AuthSaveKey"];
        //    if (String.IsNullOrEmpty(saveKey))
        //    {
        //        saveKey = "WXLoginedUser";
        //    }
        //    bool hasCookie = false;
        //    if (context.Request.Cookies[saveKey] != null)
        //    {
        //        hasCookie = true;
        //        if (context.Session != null && context.Session[saveKey] != null)
        //        {
        //            int userId = 0;
        //            int.TryParse(ZJB.Core.Utilities.CryptoUtility.TripleDESDecrypt(context.Request.Cookies[saveKey].Value), out userId);
        //            PublicUserModel user = (PublicUserModel)context.Session[saveKey];
        //            var ucPassport = (UcPassport)context.Session["Passport"];
        //            if (user != null && user.UserID == userId && ucPassport != null && ucPassport.UserName == user.Name)
        //            {
        //                return user;
        //            }
        //        }
        //    }
        //    if (context.Session["Passport"] != null)
        //    {
        //        var ucPassport = (UcPassport)context.Session["Passport"];
        //        if(ucPassport!=null && ucPassport.IsLogin)
        //        {
        //            return Register(context, ucPassport);
        //        }
        //    }
        //    else
        //    {
        //        if (hasCookie)
        //        {
        //            context.Request.Cookies[saveKey].Expires = DateTime.Now.AddDays(-1);
        //            context.Response.Cookies.Add(context.Request.Cookies[saveKey]);
        //        }
        //    }
        //    return null;
        //}


        private static void SignOut(this Controller context)
        {
            String saveKey = System.Configuration.ConfigurationManager.AppSettings["AuthSaveKey"];
            if (String.IsNullOrEmpty(saveKey))
            {
                saveKey = "WXLoginedUser";
            }
            if (context.Session[saveKey] != null)
            {
                context.Session[saveKey] = null;
                context.Session.Remove(saveKey);
            }
            if (context.Request.Cookies[saveKey] != null)
            {
                context.Request.Cookies[saveKey].Expires = DateTime.Now.AddDays(-1);
                context.Response.Cookies.Add(context.Request.Cookies[saveKey]);
            }
            if (context.Session["Passport"] != null)
            {
                context.Session.Remove("Passport");
            }
            if (context.Session != null)
            {
                context.SignOut();
            }
        }
    }
}