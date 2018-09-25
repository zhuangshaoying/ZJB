using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Senparc.Weixin.MP.Helpers;

namespace ZJB.WX.Controllers
{
    public class PublicController : Controller
    {
        //
        // GET: /Public/

        private string appId = Core.Utilities.ConfigUtility.GetValue("WeixinAppId");
        private string secret = ZJB.Core.Utilities.ConfigUtility.GetValue("WeixinAppSecret");
        public ActionResult Index()
        {
            return View();
        }


        public JsonResult GetWxShareData(string url="")
        {
            var urlReferrer = string.IsNullOrEmpty(url)? Request.UrlReferrer != null ? Request.UrlReferrer.ToString() : Request.Url.AbsoluteUri:url;
            var jssdkUiPackage = JSSDKHelper.GetJsSdkUiPackage(appId, secret, urlReferrer);
           
            return Json(jssdkUiPackage);
        }
        
    }
}
