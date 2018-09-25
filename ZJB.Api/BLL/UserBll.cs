using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Text.RegularExpressions;
using System.Configuration;
using System.Web.Configuration;
using log4net;
using ZJB.Api.Common;
using ZJB.Api.DAL;
using ZJB.Core.Injection;
using ZJB.Api.Models;


namespace ZJB.Api.BLL
{
    public class UserBll
    {
        private readonly UserDal userDal = Container.Instance.Resolve<UserDal>();


        #region 用户注册
        /// <summary>
        /// 用户注册
        /// </summary>
        /// <param name="entity"></param>
        /// <returns>UserID大于0：成功 ，小于0：各种失败</returns>
        public virtual int addPublicUser(PublicUserModel entity)
        {
            return userDal.addPublicUser(entity);
        }

        public virtual int UpdayePublicUserTel(int UserID, string Tel)
        {
            return userDal.UpdayePublicUserTel(UserID, Tel);
        }

        //INSERT INTO PublicUserThirdInfo(UserID,AppID,OpenID,UnionID) VALUES() 
        public virtual int addPublicUserThirdInfo(int UserID, int AppID, string OpenID, string UnionID)
        {
            return userDal.addPublicUserThirdInfo(UserID, AppID, OpenID, UnionID);
        }
        #endregion

        #region 用户登录
        /// <summary>
        /// 用户登录
        /// </summary>
        /// <param name="loginName">手机号或者用户名登陆</param>
        /// <param name="pwd">未加密的密码</param>
        /// <param name="isAddLog">是否记录登陆明细 默认是</param>
        /// <param name="userid">想要登录的用户ID 仅限超管使用</param>
        /// <returns></returns>
        public virtual PublicUserModel PublicUserLogin(string loginName, string pwd)
        {
            int isAddLog = 1;
            int userid = 0;
            return userDal.PublicUserLogin(loginName, pwd, isAddLog, userid);
        }
        public virtual PublicUserModel PublicUserLoginByToken(string token)
        {
            return userDal.PublicUserLoginByToken(token);
        }
        public virtual PublicUserModel PublicUserLogin(string loginName, string pwd, int isAddLog)
        {

            int userid = 0;
            return userDal.PublicUserLogin(loginName, pwd, isAddLog, userid);
        }
        public virtual PublicUserModel PublicUserLogin(string loginName, string pwd, int isAddLog, int userid)
        {


            return userDal.PublicUserLogin(loginName, pwd, isAddLog, userid);
        }
        public virtual PublicUserModel PublicUserAdminLogin(string loginName, string pwd, int isAddLog = 1)
        {
            return userDal.PublicUserAdminLogin(loginName, pwd, isAddLog);
        }

        public virtual Credential PostLogin(string loginName, string password, int loginType, out int returnValue)
        {
            return userDal.PostLogin(loginName, password, loginType, out returnValue);
        }
        public virtual Credential PostLoginByToken(string token)
        {
            return userDal.PostLoginByToken(token);
        }
        #endregion

        #region  修改密码
        /// <summary>
        /// 修改密码
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="pwd"></param>
        /// <returns></returns>
        public virtual int UpdateUserPassword(int userId, string pwd)
        {
            return userDal.UpdateUserPassword(userId, pwd);
        }
        #endregion

        #region 获取用户信息

        public virtual PublicUserModel GetUserById(int userid)
        {
            return userDal.GetUserById(userid);
        }
        public virtual PublicUserModel GetUserByOpenId(string openid)
        {
            return userDal.GetUserByOpenId(openid);
        }
        public virtual string GetUserName(int uid)
        {

            return userDal.GetUserName(uid);
        }
        public virtual PublicUserModel getUserByTel(string tel)
        {
            return userDal.getUserByTel(tel);
        }
        public virtual PublicUserModel getUserByName(string name)
        {
            return userDal.getUserByName(name);
        }

        /// <summary>
        /// 获取所有管理员openid信息
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public virtual  List<PublicUserModel> getadminUserOpenid()
        {
            return userDal.getadminUserOpenid();
        }

        #endregion

        #region 使用简报
        /// <summary>
        /// 使用简报
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public virtual UserReportModel GetUserReport(int userId)
        {
            return userDal.GetUserReport(userId);
        }
        #endregion

        #region 管理后台 每日用户登陆统计
        public virtual List<StatModel> GetLoginStat(StatReq parame)
        {
            return userDal.GetLoginStat(parame);
        }
        #endregion 

        #region 每天访问登陆后的首页就增加一次统计
        public virtual int HomeIndexAccessStat(int userId, string name)
        {
            return userDal.HomeIndexAccessStat(userId, name);
        }
        #endregion

        #region  获取用户凭证
        /// <summary>
        /// 获取用户凭证
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        public Credential GetCredentialByToken(string token)
        {
            return userDal.GetCredentialByToken(token);
        }
        #endregion

        #region 首页 是否签到 和签到天数
        public UserTaskSignStat UserSingStat(int userId)
        {
            return userDal.UserSingStat(userId);
        }
        #endregion

        #region 用户访问排行
        /// <summary>
        /// 
        /// </summary>
        /// <param name="parame"></param>
        /// <param name="totalSize"></param>
        /// <returns></returns>
        public List<PublicUserModel> GetUserAccessStatList(UserAccessListReq parame, ref int totalSize)
        {
            return userDal.GetUserAccessStatList(parame, ref totalSize);

        }
        #endregion

        #region  发送验证码
        public int PostGenerateCaptcha(string phone, string content, string captcha, int type)
        {
            SmsApi smsApi = new SmsApi();
            Parallel.Invoke(() => smsApi.SendVerificationCode(phone, content, "【房产盒子】"));
            return userDal.AddCaptcha(phone, captcha, DateTime.Now.AddMinutes(20), type);
        }
        #endregion

        #region  验证验证码
        public int CheckCaptcha(string phone, string captcha, int type)
        {

            return userDal.CheckCaptcha(phone, captcha, type);
        }
        #endregion

        #region 用户积分操作
        public virtual int AddUserPoint(int uid, int points, string pointDesc)
        {
            return userDal.AddUserPoint(uid, points, pointDesc);
        }
        #endregion 

        #region 获取用户的相关的总店门店相关信息
        public List<CompanyModel> GetUserCompanyList(int userId)
        {
            return userDal.GetUserCompanyList(userId);
        }
        #endregion

        #region 今日签到排行
        public List<UserSignTop> GetTodayUserSignTop(int pi, int ps, ref int totalSize)
        {
            return userDal.GetTodayUserSignTop(pi, ps, ref totalSize);
        }
        public SignRightStat GetSignRightStat(int userId)
        {
            return userDal.GetSignRightStat(userId);
        }
        #endregion

        #region 获取邀请排行榜
        public List<UserSignTop> GetInviteList(int pi, int ps, ref int totalSize)
        {
            return userDal.GetInviteList(pi, ps, ref totalSize);
        }
        #endregion


        #region 获取通讯录
        /// <summary>
        /// 
        /// </summary>
        /// <param name="parame"></param>
        /// <param name="totalSize"></param>
        /// <returns></returns>
        public List<PublicUserModel> GetUserContacts(string keyword, int companyId, int storeId, string letter, int pi, int ps, ref int totalSize)
        {
            return userDal.GetUserContacts(keyword, companyId, storeId, pi, ps, letter, ref totalSize);

        }
        #endregion
    }

}