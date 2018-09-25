using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ZJB.Api.BLL;
using ZJB.Api.Common;
using ZJB.Api.Models;
using ZJB.Core.Utilities;
using ZJB.WX.Common;

namespace ZJB.WX.Controllers
{
    public class BaseController : Controller
    {
        private readonly ICacheManager cache = CacheFactory.GetInstance();
        UserBll _userBll = new UserBll();
        public JsonResult JsonReturnValue(object data, JsonRequestBehavior jsonRequestBehavior)
        {
            if (Request.AcceptTypes != null && !Request.AcceptTypes.Contains("application/json"))
                return Json(data, "text/plain", jsonRequestBehavior);
            return Json(data, jsonRequestBehavior);
        }
        #region 做任务

        public virtual void DoTask(int uid, PointsEnum pointsEnum)
        {
            int points = 0;
            DoTask(uid, pointsEnum, out points);
        }

        public virtual void DoTask(int uid, PointsEnum pointsEnum, out int points)
        {
            points = 0;
               UserTaskLogBll userTaskLog = new UserTaskLogBll();

               Dictionary<string, string> userTaskInfo = TaskHelper.GetEveryDayTask(uid);
            int taskKey = (int)pointsEnum;
            if (userTaskInfo != null)
            {
                if (!userTaskInfo.ContainsKey(taskKey.ToString()))//没有任务状态
                {
                    userTaskInfo = TaskHelper.GetEveryDayTask(uid, true);
                }
                if (userTaskInfo != null && userTaskInfo.ContainsKey(taskKey.ToString()) && userTaskInfo[taskKey.ToString()] == "-1")//未做过
                {
                    userTaskLog.UserTaskLogAdd(uid, taskKey);
                    TaskHelper.SetEveryDayTask(uid, taskKey, 0);
                }
            }
     

        }
        #endregion


        /// <summary>
        /// 根据userId进行登陆
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        protected virtual bool LoginByUserId(string userId)
        {
            //todo 根据userId进行登陆

            int _userId = 0;
            int.TryParse(userId, out _userId);
            if (_userId < 1)
                return false;
            cache.Remove(string.Format(CacheItemConstant.UserModelItem, userId));
            var user = _userBll.GetUserById(_userId);
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


            return true;
        }

        /// <summary>
        /// 根据openid取得用户id
        /// </summary>
        /// <param name="openId"></param>
        /// <returns></returns>
        protected virtual string GetUserIdByOpenId(string openId)
        {
            //todo 根据openid取得用户id
            var user = _userBll.GetUserByOpenId(openId);



            return user != null ? Convert.ToString(user.UserID) : null;


        }
    }
}
