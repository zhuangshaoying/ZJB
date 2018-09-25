using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Configuration;
using System.Web.Http;
using log4net;
using ZJB.Api.BLL;
using ZJB.Api.Entity;
using ZJB.Api.Models;
using ZJB.Core.Injection;
using ZJB.Core.Utilities;
using ZJB.WX.Common;
using ZJB.WX.Common.UserVerifiler;
using ZJB.WX.Filters;

using ZJB.WX.Models.Client;


namespace ZJB.WX.Controllers.Api
{
    public class UserController : BaseController
    {
        private ILog logger = LogManager.GetLogger("UserLogger");
        private readonly UserBll userBll = Container.Instance.Resolve<UserBll>();

        private UserTaskLogBll userTaskLog = new UserTaskLogBll();
        private NCBaseRule ncBase = new NCBaseRule();

        #region 用户登录
        /// <summary>
        /// 用户登陆  
        /// </summary>
        /// <returns>json字符串</returns>
        [HttpPost]
        [IgnoreValidate]
        public ApiResponse Login([FromBody]LoginReq model)
        {
            int loginType = 0;
            if (HttpContext.Current.Request.UserAgent.Contains("IPHONE"))
            {
                loginType = (int)SourceType.Ios; ;  // ios 登录
            }
            else if (HttpContext.Current.Request.UserAgent.Contains("ANDROID"))
            {
                loginType = (int)SourceType.Android;  // android 登录
            }
            else if (HttpContext.Current.Request.UserAgent.Contains("PC_Ver"))
            {
                loginType = (int)SourceType.Pc;  // Pc客户端 登录
            }

            #region Token登录
            if (!string.IsNullOrEmpty(model.Token))
            {
                var userByToken = userBll.PostLoginByToken(model.Token);
                var result = new
                {
                    userByToken.UserID,
                    userByToken.Name,
                    userByToken.Phone,
                    userByToken.Token,
                    userByToken.Icon,
                    ExpirationDate = DateTimeUtility.ToUnixTime(userByToken.ExpirationDate),
                    userByToken.CityId,
                    userByToken.Email,
                    GainPoints = 0,
                    GainPointsMsg = ""
                };
                return new ApiResponse(Metas.SUCCESS, result);
            }
#endregion
            int loginStatus;
            var user = userBll.PostLogin(model.UserName, model.Password, loginType, out loginStatus);

            if (user != null)
            {
                int gainPoints=0;
                string gainPointsMsg="";

                
                #region APP登录加分

                switch (loginType)
                {
                    case (int)SourceType.Ios:
                    case (int)SourceType.Android:
                         DoTask(user.UserID, PointsEnum.First_Login_App, out gainPoints);
                        gainPointsMsg = "完成“APP客户端登录”任务"; break;
                    case (int)SourceType.Pc:
                        DoTask(user.UserID, PointsEnum.First_Login_PC, out gainPoints);
                        gainPointsMsg = "完成“Pc客户端登录”任务"; break;

                }
               
              
                #endregion

                var result = new
                {
                    user.UserID,
                    user.Name,
                    user.Phone,
                    user.Token,
                    user.Icon,
                    ExpirationDate=DateTimeUtility.ToUnixTime(user.ExpirationDate),
                    user.CityId,
                    user.Email,
                    GainPoints = gainPoints,
                    GainPointsMsg = gainPointsMsg
                };
                return new ApiResponse(Metas.SUCCESS, result);
            }

            switch (loginStatus)
            {
                case 1: return new ApiResponse(Metas.ACCOUNT_OR_PASSWORD_WRONG); 
            }

            return new ApiResponse(Metas.UNKNOWN_ERROR);
        }
        #endregion

        #region 站点管理
        /// <summary>
        /// 站点管理  
        /// </summary>
        /// <returns>json字符串</returns>
        [HttpGet]
        [Token]
        public ApiResponse SiteManage(int cityId=592)
        {
            int userId = GetCurrentUserId();
            SiteManageBll siteManageBll = new SiteManageBll();
            List<SiteManageModel> siteManageList = siteManageBll.GetSiteList(cityId: cityId);//所有站点

            UserSiteManageBll userSiteManageBll = new UserSiteManageBll();
            List<UserSiteManageModel> userSiteManageList = userSiteManageBll.GetUserSiteListByUserId(userId);//用户关联的站点

           var viewList = (from site in siteManageList
                                                  join userSite in userSiteManageList on site.SiteID equals userSite.SiteID into viewListTemp
                                                  from viewItem in viewListTemp.DefaultIfEmpty()
                                                  select new 
                                                  {
                                                      CityID = site.CityID,
                                                      Logo = "http://"+ HttpContext.Current.Request.Url.Host + "/"+site.Logo,
                                                      SiteID = site.SiteID,
                                                      SiteName = site.SiteName,
                                                      Status = site.Status,
                                                      SiteStatus = viewItem == null ? 1 : viewItem.SiteStatus,
                                                      SiteUserID = viewItem == null ? 0 : viewItem.SiteUserID,
                                                      SiteUserName = viewItem == null ? "" : viewItem.SiteUserName,
                                                      UserID = viewItem == null ? 0 : viewItem.UserID,
                                                      YunRefresh = site.YunRefresh,
                                                      RegisterUrl = site.RegisterUrl
                                                  }).ToList();

            return new ApiResponse(Metas.SUCCESS, viewList);
        }
        #endregion

        #region 添加站点
        /// <summary>
        /// 添加站点  
        /// </summary>
        /// <returns>json字符串</returns>
        [HttpPost]
        [Token]
        public ApiResponse UserSiteManageAdd([FromBody]SiteUserReq model)
        {
            int gainPoints = 0;
            string gainPointsMsg = "";
            if (model.SiteID > 0)
            {
                bool flag = false;
                try
                {
                    flag = UserVerifier.CheckSite(model.SiteID, model.UserName, model.Password);
                }
                catch (Exception)
                {
                    
                  
                }
           
                if (!flag)
                    return new ApiResponse(Metas.ACCOUNT_OR_PASSWORD_WRONG); 
                UserSiteManageBll userSiteManageBll = new UserSiteManageBll();
                UserSiteManageModel userSiteManageModel = new UserSiteManageModel()
                {
                    SiteID = model.SiteID,
                    UserID = GetCurrentUserId(),
                    SiteStatus = 1,
                    SiteUserName = model.UserName,
                    SiteUserPwd = model.Password
                };
                int row=  userSiteManageBll.UserSiteManageAdd(userSiteManageModel);

                if (row > 0)
                {
         
                    DoTask(GetCurrentUserId(), PointsEnum.First_BindSiteAccount, out gainPoints);
                    gainPointsMsg = "完成“绑定网站帐号”任务";
                    var result = new
                    {
                        GainPoints = gainPoints,
                        GainPointsMsg = gainPointsMsg
                    };
                    return new ApiResponse(Metas.SUCCESS, result);
                }
                else
                {
                    return new ApiResponse(Metas.SITE_EXISTS);
                }
            }
            else
            {

                return new ApiResponse(Metas.SITE_NOT_EXISTS);  // Todo：这边没有判断站点是否存在、关闭
            }
        }
        #endregion

        #region 删除站点
        /// <summary>
        /// 删除站点  
        /// </summary>
        /// <returns>json字符串</returns>
        [HttpPost]
        [Token]
        public ApiResponse UserSiteDelete([FromBody]SiteUserReq model)
        {
            UserSiteManageBll userSiteManageBll = new UserSiteManageBll();
            userSiteManageBll.DeleteUserSite(GetCurrentUserId(), model.SiteID);
            return new ApiResponse(Metas.SUCCESS);
        }
        #endregion

        #region 修改绑定站点的密码
        /// <summary>
        /// 修改绑定站点的密码  
        /// </summary>
        /// <returns>json字符串</returns>
        [HttpPost]
        [Token]
        public ApiResponse UserSiteUpdatePwd([FromBody]SiteUserReq model)
        {
            UserSiteManageBll userSiteManageBll = new UserSiteManageBll();
            int row = userSiteManageBll.UpdateUserSitePwd(GetCurrentUserId(), model.SiteID, model.Password);
            return new ApiResponse(Metas.SUCCESS);
        }
        #endregion

        #region 查看绑定站点的密码
        /// <summary>
        /// 查看绑定站点的密码  
        /// </summary>
        /// <returns>json字符串</returns>
        [HttpPost]
        [Token]
        public ApiResponse UserSiteShowPwd([FromBody]SiteUserReq model)
        {
            var credential = Request.GetCredential();
            string userName = "";
            if (credential != null)
            {
                userName = credential.Name;
            }
            UserBll userBll = new UserBll();
            PublicUserModel checkUser = userBll.PublicUserLogin(userName, model.Password, 0);
            if (checkUser != null && checkUser.UserID > 0)
            {
                UserSiteManageBll userSiteManageBll = new UserSiteManageBll();
                List<UserSiteManageModel> userSiteManageList = userSiteManageBll.GetUserSiteListByUserId(checkUser.UserID);//用户关联的站点
                UserSiteManageModel userSiteItem = userSiteManageList.Where(s => s.SiteID == model.SiteID).FirstOrDefault();
                if (userSiteItem.IsNoNull())
                {
                    var list = new
                    {
                        userSiteItem.SiteUserName,
                        userSiteItem.SiteUserPwd
                    };
                    return new ApiResponse(Metas.SUCCESS, list);
                }
                else
                {
                    return new ApiResponse(Metas.SUCCESS);
                }
            }
            else
            {
                return new ApiResponse(Metas.PASSWORD_WRONG);
            }

        }
        #endregion

        #region 经纪人个人信息

        [HttpGet]
        [Token]
        public ApiResponse BrokerInfo()
        {
            int userId = GetCurrentUserId();
       
            VPublicUser vPublicUser = ncBase.CurrentEntities.VPublicUser.Where(o => o.UserID == userId).FirstOrDefault();

            var brokerInfo = vPublicUser.IsNoNull()
                ? new
                {
                    vPublicUser.UserID,
                    vPublicUser.VipType,
                    vPublicUser.Name,
                    vPublicUser.Tel,
                    vPublicUser.VipTypeName,
                    vPublicUser.CityID,
                    vPublicUser.CityName,
                    CompanyID=vPublicUser.CompanyId,
                    vPublicUser.CompanyName,
                    StoreID=vPublicUser.StoreId,
                    vPublicUser.StoreName,
                    vPublicUser.Email,
                    vPublicUser.EnrolnName,
                    vPublicUser.Points,
                    vPublicUser.Portrait,
                }
                : null;

            return new ApiResponse(Metas.SUCCESS, brokerInfo);
        }
        #endregion

        #region 修改Email

        [HttpPost]
        [Token]
        public ApiResponse EditEmail([FromBody]UserReq model)
        {
            int userId = GetCurrentUserId();

            if (string.IsNullOrEmpty(model.Email))
            {
                return new ApiResponse(Metas.EMAIL_NULL);
            }
            PublicUser emailUser = ncBase.CurrentEntities.PublicUser.Where(u => u.Email == model.Email).FirstOrDefault();
            if (emailUser.IsNoNull())
            {
                return new ApiResponse(Metas.EMAIL_EXISTS);
            }
            #region 直接修改
        
            PublicUser loginUser = ncBase.CurrentEntities.PublicUser.Where(u => u.UserID == userId).FirstOrDefault();
            loginUser.Email = model.Email;
            ncBase.CurrentEntities.SaveChanges();
            #endregion

            return new ApiResponse(Metas.SUCCESS);
        }
        #endregion

        #region 修改头像

        [HttpPost]
        [Token]
        public ApiResponse EditPortrait([FromBody]UserReq model)
        {
            int userId = GetCurrentUserId();
            int gainPoints = 0;
            string gainPointsMsg = "";
            if (string.IsNullOrEmpty(model.Portrait))
            {
                return new ApiResponse(Metas.Portrait_NULL);
            }
            PublicUser thisUser = ncBase.CurrentEntities.PublicUser.Where(o => o.UserID == userId).FirstOrDefault();
            if (thisUser.IsNoNull())
            {
                thisUser.Portrait = model.Portrait;
                ncBase.CurrentEntities.SaveChanges();

                DoTask(userId, PointsEnum.First_UploadHead, out gainPoints);
                gainPointsMsg = "完成“有头有脸”任务";
            }

            var result = new
            {
                GainPoints = gainPoints,
                GainPointsMsg = gainPointsMsg
            };
            return new ApiResponse(Metas.SUCCESS, result);
        }
        #endregion

        #region 修改真实姓名

        [HttpPost]
        [Token]
        public ApiResponse EditEnrolnName([FromBody]UserReq model)
        {
            int userId = GetCurrentUserId();
            int gainPoints = 0;
            string gainPointsMsg = "";
            if (string.IsNullOrEmpty(model.EnrolnName))
                return new ApiResponse(Metas.Info_NULL);
            if (model.EnrolnName.Length < 2 || model.EnrolnName.Length > 6)
                return new ApiResponse(Metas.EnrolnName_LengthError);
            PublicUser thisUser = ncBase.CurrentEntities.PublicUser.Where(o => o.UserID == userId).FirstOrDefault();
            if (thisUser.IsNoNull())
            {
                thisUser.EnrolnName = model.EnrolnName;
                ncBase.CurrentEntities.SaveChanges();
              
                DoTask(userId, PointsEnum.First_EnrolnName, out gainPoints);
                 gainPointsMsg = "完成“修改真实姓名”任务";
            }

            var result = new
            {
                GainPoints = gainPoints,
                GainPointsMsg = gainPointsMsg
            };
            return new ApiResponse(Metas.SUCCESS, result);
        }
        #endregion

        #region 修改密码

        [HttpPost]
        [Token]
        public ApiResponse EditPwd([FromBody]UserReq model)
        {
            
            if(string.IsNullOrEmpty(model.NewPwd)||model.NewPwd.Length<6)
                return new ApiResponse(Metas.PwdLength_Wrong);
            var credential = Request.GetCredential();
            int userId = 0;
            string userName = "";
            if (credential != null)
            {
                userId = credential.UserID;
                userName = credential.Name;
            }
            UserBll userBll = new UserBll();
            PublicUserModel checkUser = userBll.PublicUserLogin(userName, model.OldPwd, 0);
            if (checkUser != null && checkUser.UserID > 0)
            {
                int row = userBll.UpdateUserPassword(checkUser.UserID, model.NewPwd);
                if (row > 0)
                {
                    return new ApiResponse(Metas.SUCCESS);
                }
                else
                {
                    return new ApiResponse(Metas.UNKNOWN_ERROR);
                }
            }
            else
            {
                return new ApiResponse(Metas.PASSWORD_WRONG);
           
            }

            
        }
        #endregion

        #region 生成验证码
        /// <summary>
        /// 生成验证码 
        /// </summary>
        /// <returns>json字符串</returns>
        [HttpPost]
        [IgnoreValidate]
        public ApiResponse GenerateCaptcha([FromBody]UserReq model)
        {
            string captcha = StringUtility.GetValiCode();
            string content = String.Format("{0}(房产盒子验证码)", captcha);
            userBll.PostGenerateCaptcha(model.Phone, content, captcha,model.Type);
            return new ApiResponse(Metas.SUCCESS);
        }

        #endregion

        #region 修改手机
        /// <summary>
        /// 修改手机 
        /// </summary>
        /// <returns>json字符串</returns>
        [HttpPost]
        [Token]
        public ApiResponse EditMobile([FromBody]UserReq model)
        {
            if (string.IsNullOrEmpty(model.Phone))
            {
                return new ApiResponse(Metas.Phone_NULL);
            
            }

            if (string.IsNullOrEmpty(model.Captcha) || userBll.CheckCaptcha(model.Phone,model.Captcha,model.Type)==0)
            {
                return new ApiResponse(Metas.Captcha_Wrong);
            }
            PublicUser telUser = ncBase.CurrentEntities.PublicUser.Where(u => u.Tel == model.Phone).FirstOrDefault();
            if (telUser.IsNoNull())
            {
                return new ApiResponse(Metas.Phone_EXISTS);
            }
            int userId = GetCurrentUserId();
            PublicUser loginUser = ncBase.CurrentEntities.PublicUser.Where(u => u.UserID == userId).FirstOrDefault();
            loginUser.Tel = model.Phone;
            ncBase.CurrentEntities.SaveChanges();
            
            return new ApiResponse(Metas.SUCCESS);
        }

        #endregion

        #region 意见反馈

        [HttpPost]
        [Token]
        public ApiResponse AddFeedback([FromBody]FeedbackReq model)
        {
            int userId = GetCurrentUserId();

            if (string.IsNullOrEmpty(model.Content))
            {
                return new ApiResponse(Metas.Info_NULL);
            }
            Feedback feedback= new Feedback();
            feedback.FeedbackContent = model.Content;
            feedback.IsReplay = false;
            feedback.UserId = userId;
            feedback.CreateTime =DateTime.Now;
            ncBase.CurrentEntities.Feedback.AddObject(feedback);
            ncBase.CurrentEntities.SaveChanges();

            return new ApiResponse(Metas.SUCCESS);
        }
        #endregion

        #region 获取任务列表

        [HttpGet]
        [Token]
        public ApiResponse GetTask(int type = 0)
        {
            int userId = GetCurrentUserId();

            List<UserTaskModel> taskList = new List<UserTaskModel>();
            if (type == 0) //未完成
            {
                taskList = userTaskLog.GetNoCompleteTask(userId);
            }
            else if (type == 1) //已完成
            {
                taskList = userTaskLog.GetCompleteTask(userId);
            }
            UserTaskStat stat = userTaskLog.Get_UserTask_Stat(userId);
            var result = new
            {
                stat.PointsCount,
                stat.UserPoints,
                stat.CompleteCount,
                TaskList = taskList.IsNoNull()
                    ? taskList.Where(o => o.AppUrl!=null&&o.AppUrl != "").Select(p => new
                    {
                        p.TaskId,
                        p.TaskName,
                        p.TaskDescription,
                        p.Points,
                        TaskType = p.Type,
                        CurNum = p.TaskStatus > 0 ? 1 : 0,
                        MaxNum = 1,
                        p.AppUrl
                    })
                    : null
            };
       

    return new ApiResponse(Metas.SUCCESS, result);
        }
        #endregion

        #region 执行完成任务

        [HttpGet]
        [Token]
        public ApiResponse DoTaskByTaskId(int taskId = 0)
        {
            int userId = GetCurrentUserId();
            PointsEnum pointsEnum = (PointsEnum) taskId;
            if(pointsEnum.IsNull())
            return new ApiResponse(Metas.SUCCESS);
            int gainPoints = 0;
           string   gainPointsMsg = "完成任务";
           DoTask(userId, pointsEnum, out gainPoints);
            var result = new
            {
               GainPoints= gainPoints,
               GainPointsMsg=gainPointsMsg
            };
            return new ApiResponse(Metas.SUCCESS, result);
        }
        #endregion

        #region 每日签到
        /// <summary>
        /// 每日签到 
        /// </summary>
        /// <returns>json字符串</returns>
        [HttpGet]
        [Token]
        public ApiResponse DaySign()
        {
              int userId = GetCurrentUserId();
            int result = 0;
            int gainPoints = 0;
            string gainPointsMsg = "";
            int taskKey = 0;
            int timeOut = 4;//四小时时间
            DateTime nowTime=DateTime.Now;
            #region 判断时间
            if (nowTime.Hour >= 8 && nowTime.Hour < 8+timeOut)
            {
                taskKey = (int)PointsEnum.EveryDay_Sign_8;
            }
            else if (nowTime.Hour >= 15 &&nowTime.Hour < 15 + timeOut)
            {
                taskKey = (int)PointsEnum.EveryDay_Sign_16;
            }
            #endregion

            
            if (taskKey > 0)
            {
                int addResult = 0;
                Dictionary<string, string> userTaskInfo = TaskHelper.GetEveryDayTask(userId);
                if (userTaskInfo != null)
                {
                    if (!userTaskInfo.ContainsKey(taskKey.ToString()))//没有任务状态
                    {
                        userTaskInfo = TaskHelper.GetEveryDayTask(userId, true);
                    }
                    if (userTaskInfo != null)
                    {
                        if (userTaskInfo.ContainsKey(taskKey.ToString()) && userTaskInfo[taskKey.ToString()] == "-1")//未做过
                        {
                            addResult = userTaskLog.UserTaskLogAdd(userId, taskKey);
                            TaskHelper.SetEveryDayTask(userId, taskKey, 0);
                        }
                    }
                }
                if (addResult > 0)
                {
                    result = userTaskLog.UserTaskLogDraw(userId, taskKey);
                    #region 结果
                    if (result <= 0)
                    {
                        switch (result)
                        {
                            case -1:
                                return new ApiResponse(Metas.Task_Null);
                        
                            case -2:
                            case -3:
                                return new ApiResponse(Metas.Sign_EXISTS);
                         
                            default:
                                return new ApiResponse(Metas.Sign_EXISTS);
                             
                        }
                    }
                    else
                    {
                        gainPoints = result;
                        gainPointsMsg = "成功领取+" + result + "积分";
                    }
                    #endregion
                }
                else
                {
                    #region 结果

                    switch (addResult)
                    {
                        case -1:
                            return new ApiResponse(Metas.Task_Null);
                       
                        case -2:
                        case -3:
                            return new ApiResponse(Metas.Sign_EXISTS);
                       
                        default:
                            return new ApiResponse(Metas.Sign_EXISTS); 
                         
                    }
                    #endregion
                }
            }
            else
            {
                return new ApiResponse(Metas.Appointed_Time); 
                gainPointsMsg = "";
            }
            var response = new
            {
                GainPoints = gainPoints,
                GainPointsMsg = gainPointsMsg
            };
            return new ApiResponse(Metas.SUCCESS, response);
        }

        #endregion

        #region 私有方法
        /// <summary>
        /// 获取当前登录的用户ID
        /// </summary>
        /// <returns></returns>
        private int GetCurrentUserId()
        {
            var credential = Request.GetCredential();
            int userId = 0;
            if (credential != null)
            {
                userId = credential.UserID;
            }
            return userId;
        }
        #endregion
    }
}
