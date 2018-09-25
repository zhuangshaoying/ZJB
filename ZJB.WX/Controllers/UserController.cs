using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Senparc.Weixin.MP;
using Senparc.Weixin.MP.AdvancedAPIs;
using ZJB.WX.Common.ZJB;
using ZJB.WX.Filters;
using ZJB.Api.BLL;
using ZJB.Api.Common;
using ZJB.Api.Entity;
using ZJB.Api.Models;
using ZJB.Core.Utilities;

using ZJB.WX.Models;
using HttpUtility = System.Web.HttpUtility;

namespace ZJB.WX.Controllers
{

    [Authorization]
    public class UserController : BaseController
    {
        private NCBaseRule ncBase = new NCBaseRule();
        private UserTaskLogBll userTaskLog = new UserTaskLogBll();
        protected readonly SmsApi smsApi = new SmsApi();
        private string WeixinAppId = ConfigurationManager.AppSettings["WeixinAppId"]; 
        private string WeixinAppSecret = ConfigurationManager.AppSettings["WeixinAppSecret"];
        private NoticeBll noticeBll = new NoticeBll();

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Verify(string tourl="", string code="")
        {

            if (string.IsNullOrEmpty(tourl))
                tourl = "http://" + Request.Url.Host;
          

            if (!string.IsNullOrEmpty(code))
            { 
               var backUrl = tourl + "?code=" + code;
                return redirectToUrl(backUrl);
            
            }

            string urlBase = OAuthApi.GetAuthorizeUrl(WeixinAppId, "http://" + Request.Url.Host + "/user/verify?tourl=" + tourl, "zhujia001", OAuthScope.snsapi_userinfo);
    
            return redirectToUrl(urlBase);

        }

        
        //
        // GET: /User/Login
        [AllowAnonymous]
        public ActionResult Login(int Id = 0, string tourl = "",int pc=0)
        {

            if (!(Request.UserAgent != null &&
                                      Request.UserAgent.ToLower().Contains("micromessenger")))
            {            
                tourl = "/QRCode/index?url=" + "http://www.zhujia001.com" + tourl;
            }
           


            PublicUserModel loginUser = this.GetLoginUser();
            if (loginUser != null && loginUser.UserID > 0)
            {

                return Redirect("/Home");
            }
            else if( pc==0)  
                return Redirect(tourl);

            ViewBag.UserId = Id;
            return View();
        }

        [AllowAnonymous]
        public ActionResult WXUrlLogin(string tourl = "", string openid = "", string wxresult = "")
        {

            //if (Session["Passport"] != null)
            //{
            //    var ucPassport = (UcPassport)Session["Passport"];
            //    if (ucPassport != null && ucPassport.IsLogin)
            //    {
            //        Register(ucPassport);
            //        return redirectToUrl(tourl);
            //    }
            //}
            //else
            //{
            JObject wxinfo = JObject.Parse(wxresult);
            UserBll userBll = new UserBll();
            PublicUserModel user = userBll.GetUserByOpenId(wxinfo.SelectToken("openid").ToString());
            if (user == null)
            {
                if (wxinfo == null)
                {
                    return Register("/");
                }
                user = new PublicUserModel();
                user.Name = wxinfo.SelectToken("openid").ToString();
                user.Password = "ZJB2016";
                user.CityID = 592;
                user.DistrictId = 0;
                user.CompanyId = 0;
                user.StoreId = 0;
                user.RegionId = 0;
                user.Portrait = wxinfo.SelectToken("headimgurl").ToString();
                user.VipType = 2; //会员类型
                user.IP = Request.UserHostAddress;
                user.LastLoginIP = Request.UserHostAddress;
                user.NickName = wxinfo.SelectToken("nickname").ToString();
                user.EnrolnName = wxinfo.SelectToken("nickname").ToString();


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

                    userBll.addPublicUserThirdInfo(userid, 1, wxinfo.SelectToken("openid").ToString(),
                        wxinfo.SelectToken("unionid").ToString());
                    user.UserID = userid;
                }
            }
            //PublicUserModel newUser = userBll.PublicUserLogin(ucPassport.UserName, ucPassport.PassWord);
            PublicUserModel newUser = user;
            String saveKey = System.Configuration.ConfigurationManager.AppSettings["AuthSaveKey"];

            if (String.IsNullOrEmpty(saveKey))
            {
                saveKey = "WXLoginedUser";
            }
            Session[saveKey] = newUser;
            HttpCookie loginUserCookie = new HttpCookie(saveKey, CryptoUtility.TripleDESEncrypt(newUser.UserID.ToString()));
            loginUserCookie.Expires = DateTime.Now.AddDays(10);
            Response.Cookies.Add(loginUserCookie);

            //}
            //else
            //    Register("/");
            if (string.IsNullOrEmpty(tourl))
            {
                tourl = "/";
            }
            if (!string.IsNullOrEmpty(tourl))
            {
                return redirectToUrl(tourl);
            }
            return View();
        }



        /// <summary>
        /// 微信登录
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        [AllowAnonymous]
        public ActionResult WXLogin(string code = "")
        {

            //if (Session["Passport"] != null)
            //{
            //    var ucPassport = (UcPassport)Session["Passport"];
            //    if (ucPassport != null && ucPassport.IsLogin)
            //    {
            //        Register(ucPassport);
            //        return redirectToUrl(tourl);
            //    }
            //}
            //else
            //{
            var tourl = "";
            if (Session["wx_backurl"] != null)
            {
                tourl = Session["wx_backurl"].ToString();
            }
            else
            {
                tourl = "/";

            }
            //JObject wxinfo = JObject.Parse(wxresult);
            var result = OAuthApi.GetAccessToken(WeixinAppId, WeixinAppSecret, code);
            var wxinfo = OAuthApi.GetUserInfo(result.access_token, result.openid);
            UserBll userBll = new UserBll();
            PublicUserModel user = userBll.GetUserByOpenId(result.openid);
            if (user == null)
            {
                if (result == null)
                {
                    return Register("/");
                }
                user = new PublicUserModel();
                user.Name = result.openid;
                user.Password = "ZJB2016";
                user.CityID = 592;
                user.DistrictId = 0;
                user.CompanyId = 0;
                user.StoreId = 0;
                user.RegionId = 0;
                user.Portrait = wxinfo.headimgurl;//result.wxinfo.SelectToken("headimgurl").ToString();
                user.VipType = 2; //会员类型
                user.IP = Request.UserHostAddress;
                user.LastLoginIP = Request.UserHostAddress;
                user.NickName = wxinfo.nickname;//wxinfo.SelectToken("nickname").ToString();
                user.EnrolnName = wxinfo.nickname; //wxinfo.SelectToken("nickname").ToString();


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

                    userBll.addPublicUserThirdInfo(userid, 1, wxinfo.openid,
                 wxinfo.unionid);
                    user.UserID = userid;
                }
            }
            //PublicUserModel newUser = userBll.PublicUserLogin(ucPassport.UserName, ucPassport.PassWord);
            PublicUserModel newUser = user;
            String saveKey = System.Configuration.ConfigurationManager.AppSettings["AuthSaveKey"];

            if (String.IsNullOrEmpty(saveKey))
            {
                saveKey = "WXLoginedUser";
            }
            Session[saveKey] = newUser;
            HttpCookie loginUserCookie = new HttpCookie(saveKey, CryptoUtility.TripleDESEncrypt(newUser.UserID.ToString()));
            loginUserCookie.Expires = DateTime.Now.AddDays(10);
            Response.Cookies.Add(loginUserCookie);

            //}
            //else
            //    Register("/");
            //if (string.IsNullOrEmpty(tourl))
            //{
            //    tourl = "/";
            //}
            if (!string.IsNullOrEmpty(tourl))
            {
                return redirectToUrl(tourl);
            }
            return View();
        }




        private ActionResult redirectToUrl(string tourl)
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

            string toUrlJs = string.Format("<script>window.location='{0}'</script>", tourl);
            //Response.Write(jsTooltip);
            //Response.Flush();
            //Response.Write(toUrlJs);
            //Response.Flush();
            return Content(jsTooltip + toUrlJs, "text/html; charset=utf-8");
        }


        [AllowAnonymous]
        public ActionResult Register(string code = "")
        {

            InvitationCode invitationCode = new InvitationCode();
            //PublicUserModel loginUser = this.GetLoginUser();
            //if (loginUser != null && loginUser.UserID > 0)
            //{
            //    return Redirect("/Home");
            //}

            List<Regions> regionList = ncBase.CurrentEntities.Regions.Where(r => r.Layer == 1).ToList();
            ViewData["RegionList"] = regionList;

            if (ConfigUtility.GetBoolValue("InvitationRegister") || !string.IsNullOrEmpty(code))
            {
                invitationCode =
                  ncBase.CurrentEntities.InvitationCode.Where(o => o.Code == code && o.IsUsed == false)
                      .FirstOrDefault();
                if (invitationCode.IsNull())
                {
                    return RedirectToAction("InvitationRegister", "User");
                }
            }

            return View(invitationCode);
        }
        [AllowAnonymous]
        public ActionResult InvitationRegister()
        {

            return View();
        }
        #region 开放用户注册
        [AllowAnonymous]
        [HttpPost]
        public ActionResult Invite(string from, string info)
        {
            UserModels userModels = new UserModels();
            bool existTel = false;
            PublicUser publicUser = new PublicUser();
            if (String.IsNullOrEmpty(info))
            {
                ViewBag.Status = -1;
                return View(userModels);
                // return Content("房产盒子提示：无效的参数！");
            }
            ViewBag.Info = info;
            ViewBag.From = from;
            info = CryptoUtility.TripleDESDecrypt(info);
            userModels = JsonConvert.DeserializeObject<UserModels>(info);
            if (userModels.IsNull())
            {
                ViewBag.Status = -1;
                return View(userModels);
            }
            ViewBag.Uid = userModels.InviteUid > 0 ? CryptoUtility.TripleDESEncrypt(Convert.ToString(userModels.InviteUid)) : "";
            string tel = string.IsNullOrEmpty(userModels.Tel) ? "" : StringUtility.StripWhiteSpace(userModels.Tel.Replace("-", ""), false);
            if (tel.StartsWith("0"))
                tel = tel.Substring(1);
            userModels.Tel = tel;
            if (!StringUtility.ValidateMobile(userModels.Tel))
            {
                ViewBag.Status = -2;
                return View(userModels);
                //  return Content("房产盒子提示：联合网用户手机号码格式不正确！");
            }
            if (string.IsNullOrEmpty(userModels.UserName))
            {
                ViewBag.Status = -3;
                return View(userModels);
                //  return Content("房产盒子提示：用户名不能为空!");
            }
            if (string.IsNullOrEmpty(userModels.Company) && string.IsNullOrEmpty(userModels.Store))
            {
                ViewBag.Status = -4;
                return View(userModels);
                // return Content("房产盒子提示：仅开放联合网房产经纪人注册!详情联系房产客服！");
            }
            publicUser =
               ncBase.CurrentEntities.PublicUser.Where(o => o.Tel == userModels.Tel).FirstOrDefault();
            if (publicUser.IsNoNull())
            {
                ViewBag.Status = -5;
                return View(userModels);

            }
            publicUser =
             ncBase.CurrentEntities.PublicUser.Where(o => o.Name == userModels.UserName).FirstOrDefault();
            if (publicUser.IsNoNull())
            {
                userModels.UserName = userModels.UserName + "_" + userModels.UserId;
            }
            ViewBag.Status = 0;
            return View(userModels);
        }

        /// <summary>
        /// 注册提交
        /// </summary>
        /// <returns></returns>
        [AllowAnonymous]
        public JsonResult InviteRegister(int cityId, string info, string password, string from = "")
        {

            if (String.IsNullOrEmpty(info))
            {
                return Json(new { status = -1, msg = "无效的参数" });

            }
            UserModels userModels = new UserModels();
            info = CryptoUtility.TripleDESDecrypt(info);
            userModels = JsonConvert.DeserializeObject<UserModels>(info);
            string tel = string.IsNullOrEmpty(userModels.Tel) ? "" : StringUtility.StripWhiteSpace(userModels.Tel.Replace("-", ""), false);
            if (tel.StartsWith("0"))
                tel = tel.Substring(1);
            userModels.Tel = tel;
            if (!StringUtility.ValidateMobile(userModels.Tel))
            {
                return Json(new { status = -1, msg = "手机号码格式不正确" });

            }
            PublicUser publicUser =
               ncBase.CurrentEntities.PublicUser.Where(o => o.Tel == userModels.Tel).FirstOrDefault();

            if (publicUser.IsNoNull())
            {
                return Json(new { status = -1, msg = "该手机号码已经注册过了" });
            }
            publicUser =
           ncBase.CurrentEntities.PublicUser.Where(o => o.Name == userModels.UserName).FirstOrDefault();
            string userName = userModels.UserName;
            if (publicUser.IsNoNull())
            {
                userModels.UserName = userModels.UserName + "_" + userModels.UserId + StringUtility.GetValiCode();
            }
            PublicUserModel user = new PublicUserModel();

            string strCompany = string.IsNullOrEmpty(userModels.Company) ? userModels.Store : userModels.Company;
            string strStore = string.IsNullOrEmpty(userModels.Company) ? "" : userModels.Store;
            Company company = ncBase.CurrentEntities.Company.Where(o => o.Name == strCompany && o.CityID == cityId).FirstOrDefault();
            if (company.IsNull())
            {
                company = new Company();
                company.Name = strCompany;
                company.Tel = "";
                company.Address = "";
                company.CityID = cityId;
                ncBase.CurrentEntities.AddToCompany(company);
                ncBase.CurrentEntities.SaveChanges();
            }


            CompanyStore companyStore = ncBase.CurrentEntities.CompanyStore.Where(o => o.StoreName == strStore && o.CompanyId == company.CompanyId).FirstOrDefault();
            if (!string.IsNullOrEmpty(strStore) && companyStore.IsNull())
            {
                companyStore = new CompanyStore();
                companyStore.Address = "";
                companyStore.CityID = cityId;
                companyStore.CompanyId = company.CompanyId;
                companyStore.StoreName = strStore;
                companyStore.Tel = "";
                ncBase.CurrentEntities.AddToCompanyStore(companyStore);
                ncBase.CurrentEntities.SaveChanges();
            }
            user.Tel = userModels.Tel;
            user.Name = userModels.UserName;
            user.Password = password;
            user.CityID = cityId;
            user.DistrictId = 0;
            user.CompanyId = company.CompanyId;
            user.StoreId = companyStore.IsNoNull() ? companyStore.StoreId : 0;
            user.RegionId = 0;
            user.VipType = 2; //会员类型
            user.Portrait = userModels.Portrait;
            user.Remarks = !string.IsNullOrEmpty(from) ? "From:" + from : "";
            user.IP = Request.UserHostAddress;
            user.LastLoginIP = Request.UserHostAddress;
            user.NickName = userName;
            user.EnrolnName = userName;

            #region 表单验证
            if (string.IsNullOrEmpty(user.Tel))
            {
                return Json(new { status = 1, msg = "手机号不能为空" });
            }

            if (string.IsNullOrEmpty(user.Name))
            {
                return Json(new { status = 1, msg = "用户名不能为空" });
            }
            if (string.IsNullOrEmpty(user.Password) || (user.Password != null && user.Password.Length < 6))
            {
                return Json(new { status = 1, msg = "密码至少6位数" });
            }
            if (user.CityID <= 0)
            {
                return Json(new { status = 1, msg = "所属城市必填" });
            }
            if (user.CompanyId <= 0)
            {
                return Json(new { status = 1, msg = "所属公司必填" });
            }

            #endregion

            UserBll userBll = new UserBll();
            int userid = userBll.addPublicUser(user);
            if (userid == -1)
            {
                return Json(new { status = 1, msg = "用户名已经存在" });
            }
            else if (userid == -2)
            {
                return Json(new { status = 1, msg = "手机号已被注册" });
            }
            if (userid > 0)//自动登录
            {
                try
                {

                    if (userModels.InviteUid.IsNoNull() && userModels.InviteUid > 0)
                    {
                        PublicUser publicUserInvite =
                            ncBase.CurrentEntities.PublicUser.Where(
                                o => o.UserID == userModels.InviteUid && o.Status == 1).FirstOrDefault();
                        if (publicUserInvite.IsNoNull())
                        {
                            int points = publicUserInvite.Points;
                            publicUserInvite.Points = points + 200;
                            ncBase.CurrentEntities.SaveChanges();

                            PublicUserPointsLog publicUserPointsLog = new PublicUserPointsLog();
                            publicUserPointsLog.Points = 200;
                            publicUserPointsLog.UserId = publicUserInvite.UserID;
                            publicUserPointsLog.AddTime = DateTime.Now;
                            publicUserPointsLog.CurrentPoints = points + 200;
                            publicUserPointsLog.Description = "手机用户" + user.Tel + "接受邀请注册加分";
                            ncBase.CurrentEntities.AddToPublicUserPointsLog(publicUserPointsLog);
                            ncBase.CurrentEntities.SaveChanges();
                            smsApi.SendSms(publicUserInvite.Tel, "手机用户" + user.Tel + "成功通过您的邀请链接注册房产盒子，系统赠送您200积分！", (Purpose)8, "【房产盒子】");

                        }
                    }
                    SendResult sendResult = smsApi.SendSms(user.Tel, "恭喜您成功开通房产盒子，帐号：" + user.Tel + " 密码：" + user.Password, (Purpose)8, "【房产盒子】");
                }
                catch (Exception)
                {

                    throw;
                }
                if (Session[user.Tel] != null)
                {
                    Session[user.Tel] = null;
                    Session.Remove(user.Tel);
                }
                PublicUserModel newUser = userBll.PublicUserLogin(user.Name, user.Password);
                String saveKey = System.Configuration.ConfigurationManager.AppSettings["AuthSaveKey"];
                if (String.IsNullOrEmpty(saveKey))
                {
                    saveKey = "WXLoginedUser";
                }
                Session[saveKey] = newUser;
                HttpCookie loginUserCookie = new HttpCookie(saveKey, CryptoUtility.TripleDESEncrypt(newUser.UserID.ToString()));
                loginUserCookie.Expires = DateTime.Now.AddDays(10);
                HttpContext.Response.Cookies.Add(loginUserCookie);
            }
            return Json(new { status = 0, msg = "注册成功" });


        }
        #endregion
        public ActionResult LoginOut()
        {
            String saveKey = System.Configuration.ConfigurationManager.AppSettings["AuthSaveKey"];
            if (String.IsNullOrEmpty(saveKey))
            {
                saveKey = "WXLoginedUser";
            }
            if (Session[saveKey] != null)
            {
                Session[saveKey] = null;
                Session.Remove(saveKey);
            }
            if (Request.Cookies[saveKey] != null)
            {
                Request.Cookies[saveKey].Expires = DateTime.Now.AddDays(-1);
                Response.Cookies.Add(Request.Cookies[saveKey]);
            }

            return View();


        }

        /// <summary>
        /// 登陆提交
        /// </summary>
        /// <param name="phone">用户名</param>
        /// <param name="userPwd">密码</param>
        /// <param name="id">用户ID</param>
        /// <returns></returns>
        [AllowAnonymous]
        public ActionResult DoLogin(string phone, string userPwd, int id = 0, string token = "", string tourl = "")
        {
            UserBll userApi = new UserBll();
            PublicUserModel user = new PublicUserModel();
            if (!string.IsNullOrEmpty(token))
            {
                user = userApi.PublicUserLoginByToken(token);
            }
            else
            {
                user = userApi.PublicUserLogin(phone, userPwd, 1, id);

            }

            if (user != null && user.UserID > 0)
            {
                String saveKey = System.Configuration.ConfigurationManager.AppSettings["AuthSaveKey"];
                if (String.IsNullOrEmpty(saveKey))
                {
                    saveKey = "WXLoginedUser";
                }
                Session[saveKey] = user;
                HttpCookie loginUserCookie = new HttpCookie(saveKey, CryptoUtility.TripleDESEncrypt(user.UserID.ToString()));
                loginUserCookie.Expires = DateTime.Now.AddDays(10);
                HttpContext.Response.Cookies.Add(loginUserCookie);
                if (!string.IsNullOrEmpty(tourl))
                    return new RedirectResult(HttpUtility.UrlDecode(tourl));
                return Json(new { status = 0 }, JsonRequestBehavior.AllowGet);
            }
            if (!string.IsNullOrEmpty(tourl))
                return new RedirectResult("/m/u/login");
            return Json(new { status = 1, msg = "用户名或者密码错误" }, JsonRequestBehavior.AllowGet);
        }
        /// <summary>
        /// 注册提交
        /// </summary>
        /// <param name="tel"></param>
        /// <param name="code"></param>
        /// <param name="name"></param>
        /// <param name="pwd"></param>
        /// <returns></returns>
        [AllowAnonymous]
        public JsonResult DoRegister(PublicUserModel user, string code, string invitationCode)
        {
            bool isNeedCode = true;//是否需要手机短信验证码

            InvitationCode invitationEntity = new InvitationCode();
            #region 是否需要邀请码验证
            if (ConfigUtility.GetBoolValue("InvitationRegister") || !string.IsNullOrEmpty(invitationCode))
            {
                #region 判断 如果邀请码对应的手机号存在 则无需手机短信验证码 否则需要手机短信验证码

                invitationEntity = ncBase.CurrentEntities.InvitationCode.Where(i => i.Code == invitationCode && i.IsUsed == false).FirstOrDefault();
                if (invitationEntity.IsNull())
                {
                    return Json(new { status = 1, msg = "邀请码不存在或已被使用" });
                }
                user.VipType = (int)invitationEntity.VipType;
                if (!string.IsNullOrEmpty(invitationEntity.BindTel))
                {
                    user.Tel = invitationEntity.BindTel;  //邀请码有绑定手机号码，防止填入其他手机号码
                    isNeedCode = false;
                }
            }
            #endregion
            #endregion

            #region 表单验证
            if (string.IsNullOrEmpty(user.Tel))
            {
                return Json(new { status = 1, msg = "手机号不能为空" });
            }
            if (isNeedCode)
            {
                if (string.IsNullOrEmpty(code) || Session["Phone_Code" + user.Tel] == null || (code != "" && Session["Phone_Code" + user.Tel] != null && code != Session["Phone_Code" + user.Tel].ToString()))
                {
                    return Json(new { status = 1, msg = "验证码错误" });
                }
            }
            if (string.IsNullOrEmpty(user.Name))
            {
                return Json(new { status = 1, msg = "用户名不能为空" });
            }
            if (string.IsNullOrEmpty(user.Password) || (user.Password != null && user.Password.Length < 6))
            {
                return Json(new { status = 1, msg = "密码至少6位数" });
            }
            if (user.CityID <= 0)
            {
                return Json(new { status = 1, msg = "所属城市必填" });
            }
            if (user.CompanyId <= 0)
            {
                return Json(new { status = 1, msg = "所属公司必填" });
            }
            if (user.StoreId <= 0)
            {
                if (user.DistrictId <= 0 || user.RegionId <= 0)
                {
                    return Json(new { status = 1, msg = "所属分店必填" });
                }
            }
            #endregion
            user.IP = Request.UserHostAddress;
            user.LastLoginIP = Request.UserHostAddress;
            user.VipType = 0; //待审用户
            UserBll userBll = new UserBll();
            int userid = userBll.addPublicUser(user);
            if (userid == -1)
            {
                return Json(new { status = 1, msg = "用户名已经存在" });
            }
            else if (userid == -2)
            {
                return Json(new { status = 1, msg = "手机号已被注册" });
            }
            if (userid > 0)//自动登录
            {

                #region 邀请码状态更改
                if (ConfigUtility.GetBoolValue("InvitationRegister"))
                {
                    invitationEntity.IsUsed = true;
                    invitationEntity.UsedTime = DateTime.Now;
                    invitationEntity.UsedUid = userid;
                    ncBase.CurrentEntities.SaveChanges();
                }
                #endregion

                if (Session[user.Tel] != null)
                {
                    Session[user.Tel] = null;
                    Session.Remove(user.Tel);
                }
                PublicUserModel newUser = userBll.PublicUserLogin(user.Name, user.Password);
                String saveKey = System.Configuration.ConfigurationManager.AppSettings["AuthSaveKey"];
                if (String.IsNullOrEmpty(saveKey))
                {
                    saveKey = "WXLoginedUser";
                }
                Session[saveKey] = newUser;
                HttpCookie loginUserCookie = new HttpCookie(saveKey, CryptoUtility.TripleDESEncrypt(newUser.UserID.ToString()));
                loginUserCookie.Expires = DateTime.Now.AddDays(10);
                HttpContext.Response.Cookies.Add(loginUserCookie);
            }
            return Json(new { status = 0, msg = "注册成功" });
        }
        [AllowAnonymous]
        public JsonResult UserUpdatePwd(PublicUserModel user, string code)
        {

            InvitationCode invitationEntity = new InvitationCode();


            #region 表单验证
            if (string.IsNullOrEmpty(user.Tel))
            {
                return Json(new { status = 1, msg = "手机号不能为空" });
            }

            if (string.IsNullOrEmpty(code) || Session["Phone_Code" + user.Tel] == null || (code != "" && Session["Phone_Code" + user.Tel] != null && code != Session["Phone_Code" + user.Tel].ToString()))
            {
                return Json(new { status = 1, msg = "验证码错误" });
            }


            if (string.IsNullOrEmpty(user.Password) || (user.Password != null && user.Password.Length < 6))
            {
                return Json(new { status = 1, msg = "密码至少6位数" });
            }


            #endregion
            UserBll userBll = new UserBll();
            PublicUserModel checkUser = userBll.getUserByTel(user.Tel);
            if (checkUser != null && checkUser.UserID > 0)
            {
                int row = userBll.UpdateUserPassword(checkUser.UserID, user.Password);
                if (row > 0)
                {
                    return Json(new { status = 0, msg = "修改成功" });
                }
                else
                {
                    return Json(new { status = 1, msg = "修改失败" });
                }
            }

            return Json(new { status = 1, msg = "帐号不存在" });



        }

        [HttpPost]
        [AllowAnonymous]
        public JsonResult GetPhoneCode(string tel)
        {
            string PHONECODE_CACHE = string.Empty;
            PHONECODE_CACHE = string.Format("PHONECODE_CACHE_{0}", tel);
            Random random = new Random();
            string strCode = random.Next(100000, 999999) + "";
            Session["Phone_Code" + tel] = strCode;
            if (System.Web.HttpContext.Current.Cache[PHONECODE_CACHE] == null)//重复发送验证
            {
                string smsContent = System.Configuration.ConfigurationManager.AppSettings["yzmContent"];



              
              
              var sendResult= noticeBll.SendUserCode(tel, strCode);

                SendsmsLog sendsmsLog = new SendsmsLog();
                sendsmsLog.Tel = tel;
                sendsmsLog.SmsContent = string.Format(smsContent, strCode);
                sendsmsLog.Sender = "住家帮小秘";
                sendsmsLog.SendTime = DateTime.Now;
                sendsmsLog.SendResult = sendResult;
                ncBase.CurrentEntities.AddToSendsmsLog(sendsmsLog);
                ncBase.CurrentEntities.SaveChanges();

                System.Web.HttpContext.Current.Cache.Insert(PHONECODE_CACHE, tel, null, DateTime.Now.AddMinutes(2), System.Web.Caching.Cache.NoSlidingExpiration);
            
                    return Json(new { status = 0 });
              
            }
            else
            {
                return Json(new { status = 0 });
            }
        }
        /// <summary>
        /// 判断手机号是否存在
        /// </summary>
        /// <param name="tel"></param>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpPost]
        public JsonResult CheckTelExists(string tel)
        {
            if (tel != null && tel != "")
            {
                UserBll userBll = new UserBll();
                PublicUserModel user = userBll.getUserByTel(tel);
                if (user != null && user.UserID > 0)
                {
                    return Json(new { exists = 1 });
                }
                return Json(new { exists = 0 });
            }
            return Json(new { exists = 1 });
        }
        /// <summary>
        /// 判断用户名是否存在
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpPost]
        public JsonResult CheckNameExists(string name)
        {
            if (name != null && name != "")
            {
                UserBll userBll = new UserBll();
                PublicUserModel user = userBll.getUserByName(name);
                if (user != null && user.UserID > 0)
                {
                    return Json(new { exists = 1 });
                }
                return Json(new { exists = 0 });
            }
            return Json(new { exists = 1 });
        }

        /// <summary>
        /// 判断邀请码是否被使用
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpPost]
        public JsonResult CheckInvitation(string code)
        {
            if (!string.IsNullOrEmpty(code))
            {
                InvitationCode invitationCode =
                    ncBase.CurrentEntities.InvitationCode.Where(o => o.Code == code && o.IsUsed == false)
                        .FirstOrDefault();

                if (invitationCode.IsNoNull())
                {
                    return Json(new { exists = 1 });
                }
                return Json(new { exists = 0 });
            }
            return Json(new { exists = 0 });
        }

        [AllowAnonymous]

        public JsonResult GetUserCompanyByKey(string act, string key, int cityId = 0, int companyId = 0)
        {
            if (act == "companyStore")
            {
                List<CompanyStore> companyList = ncBase.CurrentEntities.CompanyStore.Where(c => c.StoreName.Contains(key) && c.CompanyId == companyId && c.CityID == cityId && c.Status == 1).ToList();
                if (companyList != null && companyList.Count > 0)
                {
                    var resultList = companyList.Select(c => new
                    {
                        Name = c.StoreName,
                        CompanyId = c.StoreId
                    }).Take(10);
                    return Json(new { data = resultList }, JsonRequestBehavior.AllowGet);
                }

            }
            else
            {
                List<Company> companyList = ncBase.CurrentEntities.Company.Where(c => c.Name.Contains(key) && c.CityID == cityId && c.Status == 1).ToList();
                if (!(companyList != null && companyList.Count > 0))
                {

                    key = "自由经纪人";
                    companyList = ncBase.CurrentEntities.Company.Where(c => c.Name == key && c.CityID == cityId && c.Status == 1).ToList();
                }

                if (companyList != null && companyList.Count > 0)
                {
                    var resultList = companyList.Select(c => new
                    {
                        c.Name,
                        c.CompanyId
                    }).Take(10);
                    return Json(new { data = resultList }, JsonRequestBehavior.AllowGet);
                }
            }
            return Json(new { data = "" }, JsonRequestBehavior.AllowGet);
        }


        [AllowAnonymous]
        public JsonResult GetRegionList(int cityId = 0, int districtId = 0)
        {
            List<Regions> regionList = ncBase.CurrentEntities.Regions.Where(r => r.CityID == cityId && r.DistrctID == districtId).ToList();
            if (regionList != null && regionList.Count > 0)
            {
                var resultList = regionList.Select(r => new
                {
                    r.RegionID,
                    r.Name
                });
                return Json(new { data = resultList }, JsonRequestBehavior.AllowGet);
            }
            return Json(new { data = "" }, JsonRequestBehavior.AllowGet);
        }

        #region 个人信息

        public ActionResult BrokerInfo()
        {
            var userId = this.GetLoginUser().UserID;
            VPublicUser vPublicUser = ncBase.CurrentEntities.VPublicUser.Where(o => o.UserID == userId).FirstOrDefault();

            return View(vPublicUser);
        }
        public ActionResult UserInfo(int uid = 0)
        {

            VPublicUser vPublicUser = ncBase.CurrentEntities.VPublicUser.Where(o => o.UserID == uid).FirstOrDefault();

            return View(vPublicUser);
        }
        [AllowAnonymous]
        public ActionResult Forgot()
        {

            return View();
        }
        public ActionResult EditMobile()
        {
            var userId = this.GetLoginUser().UserID;
            VPublicUser vPublicUser = ncBase.CurrentEntities.VPublicUser.Where(o => o.UserID == userId).FirstOrDefault();
            return View(vPublicUser);
        }
        [HttpPost]
        public JsonResult EditMobile(string tel, string code)
        {
            if (string.IsNullOrEmpty(tel))
            {
                return Json(new { status = 1, msg = "手机号不能为空" });
            }

            if (string.IsNullOrEmpty(code) || Session["Phone_Code" + tel] == null || (code != "" && Session["Phone_Code" + tel] != null && code != Session["Phone_Code" + tel].ToString()))
            {
                return Json(new { status = 1, msg = "验证码错误" });
            }
            PublicUser telUser = ncBase.CurrentEntities.PublicUser.Where(u => u.Tel == tel).FirstOrDefault();
            if (telUser.IsNoNull())
            {
                return Json(new { status = 1, msg = "修改失败,该手机号码已被使用" });
            }
            int userId = this.GetLoginUser().UserID;
            PublicUser loginUser = ncBase.CurrentEntities.PublicUser.Where(u => u.UserID == userId).FirstOrDefault();
            loginUser.Tel = tel;
            ncBase.CurrentEntities.SaveChanges();
            return Json(new { status = 0, msg = "修改成功" });
        }
        public ActionResult BrokerEmail()
        {
            var userId = this.GetLoginUser().UserID;
            VPublicUser vPublicUser = ncBase.CurrentEntities.VPublicUser.Where(o => o.UserID == userId).FirstOrDefault();
            return View(vPublicUser);
        }
        [HttpPost]
        public JsonResult EditEmail(string email)
        {
            if (string.IsNullOrEmpty(email))
            {
                return Json(new { status = 1, msg = "邮箱不能为空" });
            }
            PublicUser emailUser = ncBase.CurrentEntities.PublicUser.Where(u => u.Email == email).FirstOrDefault();
            if (emailUser.IsNoNull())
            {
                return Json(new { status = 1, msg = "修改失败,该邮箱已被使用" });
            }
            PublicUserModel user = this.GetLoginUser();
            int userId = this.GetLoginUser().UserID;
            //#region 直接修改
            //PublicUserModel user = this.GetLoginUser();
            //int userId = this.GetLoginUser().UserID;
            //PublicUser loginUser = ncBase.CurrentEntities.PublicUser.Where(u => u.UserID == userId).FirstOrDefault();
            //loginUser.Email = email;
            //ncBase.CurrentEntities.SaveChanges();
            //#endregion
            #region 发送邮箱进行验证后，在验证页面进行修改
            string md5Code = ZJB.Core.Utilities.StringUtility.ToMd5String(userId.ToString() + email + DateTime.Now.ToString());

            EmailEditValidCode ValidRecord = ncBase.CurrentEntities.EmailEditValidCode.Where(e => e.UserId == userId).FirstOrDefault();
            if (ValidRecord.IsNoNull())
            {
                ValidRecord.NewEmail = email;
                ValidRecord.Status = 0;
                ValidRecord.AddTime = DateTime.Now;
                ValidRecord.Code = md5Code;
                ncBase.CurrentEntities.SaveChanges();
            }
            else
            {
                EmailEditValidCode newValid = new EmailEditValidCode()
                {
                    AddTime = DateTime.Now,
                    Code = md5Code,
                    NewEmail = email,
                    Status = 0,
                    UserId = userId,
                };
                ncBase.CurrentEntities.EmailEditValidCode.AddObject(newValid);
                ncBase.CurrentEntities.SaveChanges();
            }
            ZJB.Core.Utilities.EmailSender emailHelper = new EmailSender();
            string title = "修改邮箱验证";
            string url = "http://" + Request.Url.Host + "/User/DoChangeEmail" + "?validCode=" + md5Code;
            string updateUrl = "<a target='_blank' href=\"" + url + "\">前往修改</a>";
            string content = string.Format(ConfigUtility.GetValue("ChangeEamilConent"), user.Name, updateUrl);
            SendEmail(title, content, email);
            #endregion
            return Json(new { status = 0, msg = "邮箱验证已发送到您新的邮箱,请查收" });
        }
        public ActionResult BrokerHeadImg()
        {
            var userId = this.GetLoginUser().UserID;
            VPublicUser vPublicUser = ncBase.CurrentEntities.VPublicUser.Where(o => o.UserID == userId).FirstOrDefault();
            return View(vPublicUser);
        }
        [HttpPost]
        public JsonResult HeadImgSave(string url)
        {
            if (!string.IsNullOrEmpty(url))
            {
                PublicUserModel loginUser = this.GetLoginUser();
                int userId = loginUser.UserID;
                PublicUser thisUser = ncBase.CurrentEntities.PublicUser.Where(o => o.UserID == userId).FirstOrDefault();
                if (thisUser.IsNoNull())
                {
                    thisUser.Portrait = url;
                    ncBase.CurrentEntities.SaveChanges();
                }

                DoTask(userId, PointsEnum.First_UploadHead);
                return Json(new { status = 1, msg = "修改成功" });
            }
            else
            {
                return Json(new { status = -1, msg = "未获取到图片" });
            }
        }

        public ActionResult BrokerEnrolnName()
        {
            var userId = this.GetLoginUser().UserID;
            VPublicUser vPublicUser = ncBase.CurrentEntities.VPublicUser.Where(o => o.UserID == userId).FirstOrDefault();



            return View(vPublicUser);
        }
        [HttpPost]
        public JsonResult EnrolnNameSave(string name)
        {
            if (!string.IsNullOrEmpty(name))
            {
                if (name.Length < 2 || name.Length > 6)
                    return Json(new { status = -1, msg = "真实姓名长度不符" });
                int userId = this.GetLoginUser().UserID;
                PublicUser thisUser = ncBase.CurrentEntities.PublicUser.Where(o => o.UserID == userId).FirstOrDefault();
                if (thisUser.IsNoNull())
                {
                    thisUser.EnrolnName = name;
                    ncBase.CurrentEntities.SaveChanges();
                }
                DoTask(userId, PointsEnum.First_EnrolnName); //做任务

                return Json(new { status = 0, msg = "修改成功" });
            }
            else
            {
                return Json(new { status = -1, msg = "真实姓名不能为空" });
            }
        }
        #endregion

        #region 修改邮箱操作
        public ActionResult DoChangeEmail(string validCode)
        {
            PublicUserModel user = this.GetLoginUser();
            int status = 0;
            EmailEditValidCode item = ncBase.CurrentEntities.EmailEditValidCode.Where(e => e.Code == validCode && e.Status == 0 && e.UserId == user.UserID).OrderByDescending(e => e.AddTime).FirstOrDefault();
            if (item.IsNoNull())
            {
                PublicUser thisUser = ncBase.CurrentEntities.PublicUser.Where(u => u.UserID == user.UserID).FirstOrDefault();
                if (thisUser.IsNoNull())
                {
                    thisUser.Email = item.NewEmail;
                    item.Status = 1;
                    ncBase.CurrentEntities.SaveChanges();
                    status = 1;
                }
                else
                {
                    status = -100;//用户不存在
                }
            }
            else
            {
                status = -99;//不是有效的邮箱验证
            }
            ViewBag.status = status;
            return View();
        }
        #endregion

        #region 发送邮件
        /// <summary>
        /// 发送邮件
        /// </summary>
        /// <param name="title">邮件标题</param>
        /// <param name="content">邮件内容</param>
        /// <param name="fromto">发到哪</param>
        public void SendEmail(string title, string content, string fromto)
        {
            EmailSender emailSender = new EmailSender
            {
                MailFrom = ConfigUtility.GetValue("MailFrom"),
                EnableSsl = Convert.ToBoolean(ConfigUtility.GetValue("EnableSsl")),
                Port = Convert.ToInt32(ConfigUtility.GetValue("Port")),
                SmtpServer = ConfigUtility.GetValue("SmtpServer"),
                IsPlainText = false,
                MailTo = fromto,
                DisplayName = "房产盒子"
            };
            emailSender.MailPriority = System.Net.Mail.MailPriority.Normal;

            emailSender.Send(title, content);


        }
        #endregion


        #region 积分日志


        public ActionResult PointsLog(int pageIndex = 1, int pageSize = 20, string keyWord = "")
        {

            var userId = this.GetLoginUser().UserID;
            string sqlwhere = "it.userId=" + userId;
            List<PublicUserPointsLog> publicUserPointsLog =
            ncBase.CurrentEntities.PublicUserPointsLog.Where(sqlwhere)
                .OrderByDescending(o => o.Id)
                .Skip(pageSize * (pageIndex - 1))
                .Take(pageSize)
                .ToList();
            ViewBag.Count = ncBase.CurrentEntities.PublicUserPointsLog.Where(sqlwhere).Count();

            ViewBag.PageIndex = pageIndex;
            ViewBag.PageSize = pageSize;
            ViewBag.KeyWord = keyWord;
            ViewBag.PageTotal = (int)Math.Ceiling((double)ViewBag.Count / pageSize);


            return View(publicUserPointsLog);
        }
        #endregion

        #region 用户冻结页面
        [IgnoreValidateAttribute]
        public ActionResult Freeze()
        {
            PublicUserModel loginUser = this.GetLoginUser();
            //if (loginUser.Points > 0) {
            // return Redirect("/Home");
            // }
            return View(loginUser);
        }
        #endregion

        #region 抽奖
        [IgnoreValidateAttribute]
        public ActionResult Lottery()
        {
            PublicUserModel loginUser = this.GetLoginUser();
            UserSiteManageBll userSiteManageBll = new UserSiteManageBll();
            List<UserSiteManageModel> userSiteManageList = userSiteManageBll.GetUserSiteListByUserId(loginUser.UserID);//用户关联的站点
            UserSiteManageModel userSiteItem = userSiteManageList.Where(s => s.SiteID == 1).FirstOrDefault();
            bool hasZJB = true;
            if (userSiteItem.IsNull())
                hasZJB = false;
            ViewBag.HasZJB = hasZJB;
            return View(loginUser);
        }
        [IgnoreValidateAttribute]
        public ActionResult LotteryHandler()
        {
            PublicUserModel loginUser = this.GetLoginUser();
            UserSiteManageBll userSiteManageBll = new UserSiteManageBll();
            List<UserSiteManageModel> userSiteManageList = userSiteManageBll.GetUserSiteListByUserId(loginUser.UserID);//用户关联的站点
            UserSiteManageModel userSiteItem = userSiteManageList.Where(s => s.SiteID == 1).FirstOrDefault();
            if (userSiteItem.IsNull())

                return Json(new { Code = -5, Angle = 0 }, JsonRequestBehavior.AllowGet);

            var json = new Lottery_ZJB().Lottery(userSiteItem.SiteUserName, userSiteItem.SiteUserPwd);

            return Content(json);

        }
        #endregion

        #region 通讯录
        public ActionResult UserContacts()
        {
            PublicUserModel thisUser = this.GetLoginUser();
            int companyid = thisUser.CompanyId ?? 0;
            Company company = ncBase.CurrentEntities.Company.Where(o => o.CompanyId == companyid).FirstOrDefault();
            thisUser.CompanyName = company.IsNoNull() ? company.Name : "";
            return View(thisUser);
        }

        [HttpGet]
        public JsonResult GetUserContacts(string type = "list", string letter = "", int lastId = 0, long lastTime = 0, int pageSize = 10, int pageIndex = 1, string keyword = "")
        {
            PublicUserModel thisUser = this.GetLoginUser();
            string userName = thisUser.Name;
            int userId = thisUser.UserID;
            UserBll userBll = new UserBll();

            int totalSize = 0;

            List<PublicUserModel> userModels = userBll.GetUserContacts(keyword, thisUser.CompanyId ?? 0, 0, letter, pageIndex, pageSize, ref totalSize);

            var resultList = userModels.Select(h => new
            {

                h.CityID,
                h.UserID,
                Name = h.Name,
                h.EnrolnName,
                h.Email,
                h.Tel,
                h.CompanyName,
                h.StoreName,
                h.Points,
                h.Portrait

            });
            return Json(new { data = resultList, totalCount = totalSize }, JsonRequestBehavior.AllowGet);
        }
        #endregion


    }
}
