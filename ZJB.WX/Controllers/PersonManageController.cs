using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ZJB.Api.Entity;
using ZJB.Api.Models;
using ZJB.Api.BLL;
using ZJB.Core.Utilities;
using ZJB.WX.Common.UserVerifiler;
using ZJB.WX.Models;
using System.Data.Objects;


namespace ZJB.WX.Controllers
{
    [Authorization]
    public class PersonManageController :BaseController 
    {
        //个人管理
        private UserTaskLogBll userTaskLog = new UserTaskLogBll();
        private NCBaseRule ncBase = new NCBaseRule();
        public ActionResult Index()
        {
            return View();
        }

        #region  站点管理
        /// <summary>
        /// 站点管理页面  GET: /PersonManage/SiteManageView
        /// </summary>
        /// <returns></returns>
        public ActionResult SiteManageView()
        {
            PublicUserModel loginUser = this.GetLoginUser();//当前用户

            SiteManageBll siteManageBll = new SiteManageBll();
            List<SiteManageModel> siteManageList = siteManageBll.GetSiteList(cityId: loginUser.CityID);//所有站点

            UserSiteManageBll userSiteManageBll = new UserSiteManageBll();
            List<UserSiteManageModel> userSiteManageList = userSiteManageBll.GetUserSiteListByUserId(loginUser.UserID);//用户关联的站点

            List<SiteManageViewModel> viewList = (from site in siteManageList
                                                  join userSite in userSiteManageList on site.SiteID equals userSite.SiteID into viewListTemp
                                                  from viewItem in viewListTemp.DefaultIfEmpty()
                                                  select new SiteManageViewModel()
                                                  {
                                                      CityID = site.CityID,
                                                      Logo = site.Logo,
                                                      SiteID = site.SiteID,
                                                      SiteName = site.SiteName,
                                                      Status = site.Status,
                                                      SiteStatus =viewItem==null?1:viewItem.SiteStatus,
                                                      SiteUserID=viewItem==null?0:viewItem.SiteUserID,
                                                      SiteUserName = viewItem==null?"":viewItem.SiteUserName,
                                                      UserID = viewItem==null?0:viewItem.UserID,
                                                      YunRefresh = site.YunRefresh,
                                                      LoginUrl=site.LoginUrl,
                                                      RegisterUrl = site.RegisterUrl
                                                  }).ToList();


            return View(viewList);
        }
        /// <summary>
        /// 修改密码界面
        /// </summary>
        /// <returns></returns>
        public ActionResult UpdatePasswordView()
        {
            return View();
        }
        
        /// <summary>
        /// 添加站点绑定
        /// </summary>
        /// <param name="parames"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult UserSiteAdd(AddUserSiteReq parames)
        {
            PublicUserModel loginUser = this.GetLoginUser();
            if (parames.webBasicId > 0)
            {
                bool flag = false;
                try
                {

                  flag= UserVerifier.CheckSite(parames.webBasicId, parames.loginName, parames.loginPwd);

                }
                catch (Exception)
                {

                }
                if(!flag)
                    return Json(new { msg = "帐号或密码错误!" });
                #region 添加
                UserSiteManageBll userSiteManageBll = new UserSiteManageBll();
                UserSiteManageModel userSiteManageModel = new UserSiteManageModel()
                {
                    SiteID = parames.webBasicId,
                    UserID = loginUser.UserID,
                    SiteStatus = 1,
                    SiteUserName = parames.loginName,
                    SiteUserPwd = parames.loginPwd
                };
                userSiteManageBll.UserSiteManageAdd(userSiteManageModel);
                #endregion
                #region 绑定网站帐号任务

                DoTask(loginUser.UserID, PointsEnum.First_BindSiteAccount);
                #endregion

                if (loginUser.VipType < 1)
                {
                    PublicUser user =
                        ncBase.CurrentEntities.PublicUser.Where(o => o.UserID == loginUser.UserID).FirstOrDefault();
                    if (user.IsNoNull())
                    {
                        VipType vipType=  ncBase.CurrentEntities.VipType.Where(o => o.VipTypeID == 2).FirstOrDefault();
                        user.VipType = vipType.IsNoNull() ? vipType.VipTypeID : 1;
                        user.MaxStock = vipType.IsNoNull() ? Convert.ToInt32(vipType.MaxStock) : 50;
                        user.PublishNum = vipType.IsNoNull() ? Convert.ToInt32(vipType.PublishNum) : 2;
                        ncBase.CurrentEntities.SaveChanges();

                    }
                }
                return Json(new { msg = "添加成功" });
            }
            else
            {

                return Json(new { msg = "添加失败" });
            }
        }
        /// <summary>
        /// 删除绑定站点
        /// </summary>
        /// <param name="SiteId"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult UserSiteDelete(int SiteId)
        {
            UserSiteManageBll userSiteManageBll = new UserSiteManageBll();
             int row=userSiteManageBll.DeleteUserSite(this.GetLoginUser().UserID,SiteId);
             return Json(row);
        }
        /// <summary>
        /// 修改绑定站点的密码
        /// </summary>
        /// <param name="SiteId"></param>
        /// <param name="loginPwd"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult UserSitePwdUpdate(int SiteId, string loginPwd)
        {
          
     
            UserSiteManageBll userSiteManageBll = new UserSiteManageBll();
            int row = userSiteManageBll.UpdateUserSitePwd(this.GetLoginUser().UserID, SiteId, loginPwd);
            return Json(row > 0 ? new { msg = "密码修改成功" } : new { msg = "密码修改失败" });
        }
        /// <summary>
        /// 查看绑定站点的密码
        /// </summary>
        /// <param name="siteId"></param>
        /// <param name="pwd">登陆密码</param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult UserSitePwdShow(int siteId, string userPwd)
        {
            UserBll userBll = new UserBll();
            PublicUserModel checkUser = userBll.PublicUserLogin(this.GetLoginUser().Name, userPwd,0);
            if (checkUser!=null&&checkUser.UserID > 0)
            {
                UserSiteManageBll userSiteManageBll = new UserSiteManageBll();
                List<UserSiteManageModel> userSiteManageList = userSiteManageBll.GetUserSiteListByUserId(checkUser.UserID);//用户关联的站点
                UserSiteManageModel userSiteItem = userSiteManageList.Where(s => s.SiteID == siteId).FirstOrDefault();
                if (userSiteItem != null)
                {
                    return Json(new { msg = "成功", loginName = userSiteItem.SiteUserName, loginPwd = userSiteItem.SiteUserPwd });
                }
                else
                {
                    return Json(new { msg = "失败", loginName = userSiteItem.SiteUserName, loginPwd = userSiteItem.SiteUserPwd });
                }
            }
            else {
                return Json(new { msg = "登陆密码错误" });
            }
        }
        public ActionResult SiteView(int userWebId)
        {
            int uid = this.GetLoginUser().UserID;
            SiteManage siteManage=new SiteManage();
            UserSiteManage userSiteManage = ncBase.CurrentEntities.UserSiteManage.Where(u => u.UserID == uid &&u.SiteID== userWebId).FirstOrDefault();
            if (userSiteManage.IsNoNull())
            {
                int siteId = userSiteManage.SiteID;
                siteManage = ncBase.CurrentEntities.SiteManage.Where(o => o.SiteID == siteId).FirstOrDefault();
                if (siteManage.IsNoNull() && !string.IsNullOrEmpty(siteManage.LoginHtml))
                {
                    var loginhtml = siteManage.LoginHtml;
                    loginhtml =
                        loginhtml.Replace("#loginusername#", userSiteManage.SiteUserName)
                            .Replace("#loginpassword#", CryptoUtility.TripleDESDecrypt(userSiteManage.SiteUserPwd));
                    siteManage.LoginHtml = loginhtml;
                }
                
            }

            return View(siteManage);
        }
        #endregion

        #region 修改密码
        /// <summary>
        /// 密码修改
        /// </summary>
        /// <param name="oldPwd"></param>
        /// <param name="newPwd"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult UserUpdatePwd(string oldPwd, string newPwd)
        {
            UserBll userBll = new UserBll();
            PublicUserModel checkUser = userBll.PublicUserLogin(this.GetLoginUser().Name, oldPwd, 0);
            if (checkUser != null && checkUser.UserID > 0)
            {
                int row= userBll.UpdateUserPassword(checkUser.UserID,newPwd);
                if (row>0)
                {
                    return Json(new { msg = "修改成功"});
                }
                else
                {
                    return Json(new { msg = "修改失败"});
                }
            }
            else
            {
                return Json(new { msg = "原始密码错误" });
            }
        }
        #endregion

        #region 站点验证
      
        [HttpPost]
        public JsonResult CheckSite(int siteId)
        {
           int uid = this.GetLoginUser().UserID;
            UserBll userBll = new UserBll();
            UserSiteManageBll userSiteManageBll = new UserSiteManageBll();
            List<UserSiteManageModel> userSiteManageList = userSiteManageBll.GetUserSiteListByUserId(this.GetLoginUser().UserID);//用户关联的站点
            UserSiteManageModel userSiteItem = userSiteManageList.Where(s => s.SiteID == siteId).FirstOrDefault();
            if (userSiteItem.IsNull())
                return Json(new { msg = "您还未绑定帐号！" });

            bool flag = false;
            try
            {
                flag = UserVerifier.CheckSite(siteId, userSiteItem.SiteUserName, userSiteItem.SiteUserPwd);
            }
            catch (Exception)
            {
              
            }
              
            if (flag)
            {

                UserSiteManage siteManage =
                    ncBase.CurrentEntities.UserSiteManage.Where(
                        o => o.SiteID == siteId && o.SiteUserName == userSiteItem.SiteUserName && o.UserID == uid).FirstOrDefault();
                if (siteManage.IsNoNull())
                {
                    siteManage.SiteStatus = BitConverter.GetBytes(1)[0];
                    siteManage.BanTime = DateTime.Now;
                    ncBase.CurrentEntities.SaveChanges();
                    return Json(new { msg = "账号验证成功并已经激活使用！" });
                }

                return Json(new { msg = "未知错误！" });
            }
            return Json(new { msg = "账号密码不正确！" });
       
        }
        #endregion


        #region 获取水印设置信息
        public JsonResult GetWaterMarkSeting()
        {
            PublicUserModel loginUser=this.GetLoginUser();
            P_WaterMark_Info_Result result= ncBase.CurrentEntities.P_WaterMark_Info(loginUser.UserID).FirstOrDefault();
            if (result.IsNull())
            {
                result = new P_WaterMark_Info_Result()
                {
                    UserId = loginUser.UserID,
                    Watermark = "/images/company/WX.png",
                    WaterMarkPosition = 0
                };
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region 保存水印设置
        [HttpPost]
        public JsonResult SaveWaterMark(int position)
        {
            PublicUserModel loginUser = this.GetLoginUser();
            PersonalSeting mySetting = ncBase.CurrentEntities.PersonalSeting.Where(o => o.UserId == loginUser.UserID).FirstOrDefault();
            if (mySetting.IsNoNull())
            {
                mySetting.WaterMarkPosition = position;
                ncBase.CurrentEntities.SaveChanges();
            }
            else {
                mySetting = new PersonalSeting
                {
                    UserId = loginUser.UserID,
                    WaterMarkPosition = position
                };
                ncBase.CurrentEntities.PersonalSeting.AddObject(mySetting);
                ncBase.CurrentEntities.SaveChanges();
            }
            return Json(mySetting.WaterMarkPosition);
        }
        #endregion 
    }
}
