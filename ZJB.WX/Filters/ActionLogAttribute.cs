using System;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ZJB.Api.Models;
using ZJB.Web.Utilities;
using ZJB.Api.Entity;
using ZJB.Api.BLL;

namespace ZJB.WX.Filters
{
    public class ActionLogAttribute : ActionFilterAttribute
    {
        private bool _checkPoints = true;

        public bool CheckPoints
        {
            get { return _checkPoints; }
            set { _checkPoints = value; }
        }

       
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {

           // bool ignoreValidate = (filterContext.ActionDescriptor.GetCustomAttributes<IgnoreValidateAttribute>().Count > 0);

            var ignoreValidate = filterContext.ActionDescriptor.GetCustomAttributes(typeof(IgnoreValidateAttribute), false).Count() > 0;
            var checkUserType = filterContext.ActionDescriptor.GetCustomAttributes(typeof(CheckUserTypeAttribute), false).Count() > 0;
            
            if(ignoreValidate)
                return;
           
            NCBaseRule ncBase = new NCBaseRule();
            HttpRequest request = HttpContext.Current.Request;
            
            ActionLog log = BuildActionLog(request);
            
            log.Controller = filterContext.ActionDescriptor.ControllerDescriptor.ControllerName;
            log.Action = filterContext.ActionDescriptor.ActionName;
      
            var user = GetLoginUser(filterContext.HttpContext);
            if (user != null)
            {
                log.UserID = user.UserID;
                log.UserName = user.Name;
            }
            //TODO:
            //Save to database
            if(log.PathAndQuery!=null && log.PathAndQuery.Length > 250)
            {
                log.PathAndQuery = log.PathAndQuery.Substring(0, 250);
            }
            ncBase.CurrentEntities.ActionLog.AddObject(log);
            ncBase.CurrentEntities.SaveChanges();
            //if (user != null && user.Points <= 0 && log.Action != "User" && log.Action != "LoginOut" && CheckPoints)
            //{
            //    filterContext.HttpContext.Response.Redirect("/User/Freeze",true);
            //    return;

            if (user != null && string.IsNullOrEmpty(user.Tel) && log.Action != "User" && log.Action != "LoginOut" && checkUserType)
            {
                filterContext.HttpContext.Response.Redirect("/User/EditMobile", true);
                return;
            }

            //if (user != null && user.VipType < 1 && checkUserType)
            //{
            //    ZJB.Api.BLL.UserBll userBll = new ZJB.Api.BLL.UserBll();
            //    var newUser = userBll.GetUserById(user.UserID);
            //    if(newUser.VipType<1)
            //        filterContext.HttpContext.Response.Redirect("/PersonManage/SiteManageView", true);
            //    return;
            //}
            //Config in FilterConfig.RegisterGlobalFilters
        }

      
        private static ActionLog BuildActionLog(HttpRequest request)
        {
            ActionLog log = new ActionLog()
            {
                Host = request.Url.Host,
                PathAndQuery = request.Url.PathAndQuery,
                Server = Environment.MachineName,
                UserAgent = request.UserAgent,
                IpAddress = IpUtility.GetIp(),
                AccessTime = DateTime.Now,
                HttpMethod = request.HttpMethod
            };
            return log;
        }

        public static PublicUserModel GetLoginUser(HttpContextBase context)
        {
            String saveKey = System.Configuration.ConfigurationManager.AppSettings["AuthSaveKey"];
            ZJB.Api.BLL.UserBll userBll = new ZJB.Api.BLL.UserBll();
             PublicUserModel user=new PublicUserModel();
            if (String.IsNullOrEmpty(saveKey))
            {
                saveKey = "WXLoginedUser";
            }
            if (context.Session != null && context.Session[saveKey] != null)
            {
                 user = (PublicUserModel) context.Session[saveKey];
                if (user.Points < 1)
                {
                    user = userBll.GetUserById(user.UserID);
                    context.Session[saveKey] = user;
                }
                return user;
            }
            else if (context.Request.Cookies[saveKey] != null)
            {
                int userId = 0;
                int.TryParse(ZJB.Core.Utilities.CryptoUtility.TripleDESDecrypt(context.Request.Cookies[saveKey].Value), out userId);
               
                 user=userBll.GetUserById(userId);
                context.Session[saveKey]=user;
                return user;
            }
            return null;
        }
    }
}