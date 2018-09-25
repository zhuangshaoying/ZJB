using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using Senparc.Weixin;
using Senparc.Weixin.Exceptions;
using Senparc.Weixin.MP;
using Senparc.Weixin.MP.AdvancedAPIs;
using Senparc.Weixin.MP.AdvancedAPIs.OAuth;
using ZJB.Api.BLL;
using ZJB.Api.Entity;
using ZJB.Api.Models;
using HttpUtility = System.Web.HttpUtility;

namespace ZJB.WX.Controllers
{
    public class OAuth2Controller : BaseController
    {

        private string appId = ConfigurationManager.AppSettings["WeixinAppId"];
        private string secret = ConfigurationManager.AppSettings["WeixinAppSecret"];


        #region 授权相关设置

        private string STATE = "zhujia001";//授权标识
        private bool ReqietTryGetUser = true;//静雯模式试着取用户信息，如果是已关注，可取到用户信息
        /// <summary>
        /// 登陆地址设置
        /// </summary>
        private string baseUrl = "http://www.zhujia001.com/oauth2/";


        /// <summary>
        /// 注册用户,返回用户id
        /// </summary>
        /// <param name="openId"></param>
        /// <param name="userInfo"></param>
        /// <returns>用户id</returns>
        private string RegisterUser(string openId, OAuthUserInfo userInfo, out ELoginStatus status)
        {
            //todo 注册用户,返回用户id
            string loupanIds = "";
            string job = "";
            UserBll userBll = new UserBll();
            PublicUserModel user = userBll.GetUserByOpenId(openId);
            if (user == null)
            {

                user = new PublicUserModel();
                user.Name = userInfo.openid;
                user.Password = "ZJB2016";
                user.CityID = 592;
                user.DistrictId = 0;
                user.CompanyId = 0;
                user.StoreId = 0;
                user.RegionId = 0;
                user.Portrait = userInfo.headimgurl;//result.wxinfo.SelectToken("headimgurl").ToString();
                user.VipType = 2; //会员类型
                user.IP = Request.UserHostAddress;
                user.LastLoginIP = Request.UserHostAddress;
                user.NickName = userInfo.nickname;//wxinfo.SelectToken("nickname").ToString();
                user.EnrolnName = userInfo.nickname; //wxinfo.SelectToken("nickname").ToString();


                if (!string.IsNullOrEmpty(user.Name))
                {
                    int userid = userBll.addPublicUser(user);
                    if (userid == -1)
                    {
                        //return Json(new { status = 1, msg = "用户名已经存在" });
                    }
                    else if (userid == -2)
                    {
                        //return Json(new { status = 1, msg = "手机号已被注册" });
                    }
                    //if (userid > 0)//自动登录

                    userBll.addPublicUserThirdInfo(userid, 1, userInfo.openid,
                        userInfo.unionid);
                    user.UserID = userid;
                    status = ELoginStatus.Success;
                    return status == ELoginStatus.Success ? userid.ToString() : null;
                }
            }
            status = ELoginStatus.UserNotExist;
            return "";

        }

        private string RegisterUser(string openId, OAuthUserInfo userInfo)
        {
            ELoginStatus status = ELoginStatus.Success;
            return RegisterUser(openId, userInfo, out status);
        }



        private ActionResult RedirectToUrl(string url, string msg = "授权中……")
        {
            string jsTooltip = @"<style>
 .favorite {
    position: fixed;
    width: 130px;
    background-color: rgba(0,0,0,.7);
    border-radius: 5px;
    color: #fff;
    font-size: 16px;
    line-height: 1;
    text-align: center;
    padding: 16px 0;
    z-index: 9999;
    left: 50%;
    margin-left: -65px;
    top: 130px;
  }
</style>
<div id='collectSuccess' class='favorite' style='/* display: none; */'>正在登录中</div>";

            string toUrlJs = string.Format("<script>window.location='{0}'</script>", url);
            //Response.Write(jsTooltip);
            //Response.Flush();
            //Response.Write(toUrlJs);
            //Response.Flush();
            return Content(jsTooltip + toUrlJs, "text/html; charset=utf-8");
        }
        #endregion

        public ActionResult Index()
        {
            //此页面引导用户点击授权
            ViewData["UrlUserInfo"] = OAuthApi.GetAuthorizeUrl(appId, baseUrl + "UserInfoCallback", STATE, OAuthScope.snsapi_userinfo);
            ViewData["UrlBase"] = OAuthApi.GetAuthorizeUrl(appId, baseUrl + "BaseCallback", STATE, OAuthScope.snsapi_base);
            return View();
        }
        /// <summary>
        /// OAuth/WXLogin?referer=
        /// 转到这里进行授权 
        /// </summary>
        /// <param name="referer"></param>
        /// <returns></returns>
        public ActionResult WXLogin(string referer = null)
        {
            //静默模式登陆
            string toUrl = string.Format(baseUrl + "BaseCallback?referer={0}", HttpUtility.UrlEncode(referer ?? "/"));
            string url = OAuthApi.GetAuthorizeUrl(appId, toUrl, STATE, OAuthScope.snsapi_base);
            return Redirect(url);
        }
        /// <summary>
        /// 取得用户信息
        /// </summary>
        /// <param name="code"></param>
        /// <param name="state"></param>
        /// <returns></returns>
        public ActionResult UserInfoCallback(string code, string state)
        {
            if (string.IsNullOrEmpty(code))
            {
                return Content("您拒绝了授权！");
            }

            if (state != STATE)
            {
                //这里的state其实是会暴露给客户端的，验证能力很弱，这里只是演示一下
                //实际上可以存任何想传递的数据，比如用户ID，并且需要结合例如下面的Session["OAuthAccessToken"]进行验证
                return Content("验证失败！请从正规途径进入！");
            }

            //通过，用code换取access_token
            var result = OAuthApi.GetAccessToken(appId, secret, code);
            if (result.errcode != ReturnCode.请求成功)
            {
                return Content("错误：" + result.errmsg);
            }

            //下面2个数据也可以自己封装成一个类，储存在数据库中（建议结合缓存）
            //如果可以确保安全，可以将access_token存入用户的cookie中，每一个人的access_token是不一样的
            Session["OAuthAccessTokenStartTime"] = DateTime.Now;
            Session["OAuthAccessToken"] = result;

            //因为第一步选择的是OAuthScope.snsapi_userinfo，这里可以进一步获取用户详细信息
            try
            {
                OAuthUserInfo userInfo = OAuthApi.GetUserInfo(result.access_token, result.openid);
                if (string.IsNullOrEmpty(Request["referer"]))
                {
                    return View(userInfo);
                }
                if (userInfo == null || userInfo.nickname == null || result.openid != userInfo.openid)
                {
                    return Content("获取用户信息失败！");
                }
                //判断用户是否存存
                string userId = null;
                ELoginStatus status = ELoginStatus.Success;
                try
                {
                    userId = GetUserIdByOpenId(result.openid);

                    if (userId == null)
                    {
                        userId = RegisterUser(result.openid, userInfo, out status);
                    }
                }
                catch (Exception ex)
                {

                }
                if (!LoginByUserId(userId))
                {
                    return Content("登陆失败status:" + status.ToString() + " userId：" + userId);
                }
                string referer = Request["referer"] ?? "/";
                return RedirectToUrl(referer);
            }
            catch (ErrorJsonResultException ex)
            {
                return Content(ex.Message);
            }
        }

        /// <summary>
        /// 静默模式
        /// </summary>
        /// <param name="code"></param>
        /// <param name="state"></param>
        /// <returns></returns>
        public ActionResult BaseCallback(string code, string state)
        {

            if (string.IsNullOrEmpty(code))
            {
                return Content("您拒绝了授权！");
            }

            if (state != STATE)
            {
                //这里的state其实是会暴露给客户端的，验证能力很弱，这里只是演示一下
                //实际上可以存任何想传递的数据，比如用户ID，并且需要结合例如下面的Session["OAuthAccessToken"]进行验证
                return Content("验证失败！请从正规途径进入！");
            }

            //通过，用code换取access_token
            var result = OAuthApi.GetAccessToken(appId, secret, code);
            if (result.errcode != ReturnCode.请求成功)
            {
                return Content("错误：" + result.errmsg);
            }

            //下面2个数据也可以自己封装成一个类，储存在数据库中（建议结合缓存）
            //如果可以确保安全，可以将access_token存入用户的cookie中，每一个人的access_token是不一样的
            Session["OAuthAccessTokenStartTime"] = DateTime.Now;
            Session["OAuthAccessToken"] = result;

            //判断用户是否存存
            string userId = null;
            try
            {
                userId = GetUserIdByOpenId(result.openid);
            }
            catch (Exception ex)
            {

            }

            if (userId == null)
            {
                //因为这里还不确定用户是否关注本微信，所以只能试探性地获取一下
                OAuthUserInfo userInfo = null;
                try
                {
                    if (ReqietTryGetUser)
                    {
                        //已关注，可以得到详细信息
                        userInfo = OAuthApi.GetUserInfo(result.access_token, result.openid);

                    }

                    if (string.IsNullOrEmpty(Request["referer"]))
                    {
                        ViewData["ByBase"] = true;
                        return View("UserInfoCallback", userInfo);
                    }
                    else
                    {

                    }
                }
                catch (ErrorJsonResultException ex)
                {
                    //未关注，只能授权，无法得到详细信息
                    //这里的 ex.JsonResult 可能为："{\"errcode\":40003,\"errmsg\":\"invalid openid\"}"
                    //return Content("用户已授权，授权Token：" + result);
                }
                if (userInfo != null && result.openid == userInfo.openid && userInfo.nickname != null)
                {
                    userId = RegisterUser(result.openid, userInfo);
                }




            }
            string referer = Request["referer"] ?? "/";
            if (userId == null)
            {
                //授权登陆
                string toUrl = string.Format(baseUrl + "UserInfoCallback?referer={0}", HttpUtility.UrlEncode(referer));
                string url = OAuthApi.GetAuthorizeUrl(appId, toUrl, STATE, OAuthScope.snsapi_userinfo);
                return Redirect(url);
            }
            else
            {
                if (!LoginByUserId(userId))
                {
                    return Content("登陆失败 userId：" + userId);
                }
                //登陆用户
                return RedirectToUrl(referer);
            }
        }
    }
}