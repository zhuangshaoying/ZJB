using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using ZJB.Api.BLL;
using ZJB.Api.Models;
using ZJB.Core.Injection;
using System.Net.Http;
using ZJB.Core.Utilities;

namespace System.Web.Http
{
    public static class ApiExtension
    {
    
    

        public static Int32 GetApiVersion(this ApiController api)
        {
            return Convert.ToInt32(HttpContext.Current.Request.RequestContext.RouteData.Values["version"]);
        }

        public static void SetCredential(this HttpRequestMessage request, Credential credential)
        {
            request.Properties["Credential"] = credential;
        }

        public static Credential GetCredential(this HttpRequestMessage request)
        {
            if (request!=null&&request.Properties.ContainsKey("Credential"))
                return request.Properties["Credential"] as Credential;

            return null;
        }

        public static Credential GetCredential(this ApiController api)
        {
            string token = HttpContext.Current.Request.Headers["Token"];
            if (string.IsNullOrEmpty(token) && HttpContext.Current.Request.Cookies["EnableApiDebug"] != null)
                token = HttpContext.Current.Request.QueryString["Token"];
            if (String.IsNullOrEmpty(token))
                return new Credential() { UserID = 0, ExpirationDate = DateTime.Now.Subtract(new TimeSpan(100, 0, 0, 0, 0)), Icon = null, Name = "未授权", Token = null };
            UserBll userDal = Container.Instance.Resolve<UserBll>();
            return userDal.GetCredentialByToken(token);
        }

    }
}