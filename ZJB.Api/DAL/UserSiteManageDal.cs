using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using log4net;
using ZJB.Api.BLL;
using ZJB.Api.Entity;
using ZJB.Api.Models;
using System.Data.Common;
using System.Data;
using ZJB.Core.Utilities;
namespace ZJB.Api.DAL
{
    public  class UserSiteManageDal:BaseDal
    {
        public UserSiteManageDal()
            : base("WX")
        { }
        private ILog logger = LogManager.GetLogger("UserSiteManageDal");
        private NCBaseRule ncBase = new NCBaseRule();
        #region 获取用户站点列表
        /// <summary>
        /// 根据用户id获取用户关联的站点
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public virtual List<UserSiteManageModel> GetUserSiteListByUserId(int userId)
        {
            List<UserSiteManage> userSiteManageList = ncBase.CurrentEntities.UserSiteManage.Where(u => u.UserID == userId).ToList();
            return EFToModelList(userSiteManageList);
        }
        #endregion

        #region 用户绑定站点
        public virtual int UserSiteManageAdd(UserSiteManageModel userSiteManageModel)
        {
            UserSiteManage userSiteManage =
                ncBase.CurrentEntities.UserSiteManage.Where(
                    o => o.SiteID == userSiteManageModel.SiteID && o.UserID == userSiteManageModel.UserID)
                    .FirstOrDefault();

            if (userSiteManage.IsNoNull())
            {
                return 0;
            }
            userSiteManage   = new UserSiteManage()
            {
                SiteID = userSiteManageModel.SiteID,
                UserID = userSiteManageModel.UserID,
                SiteUserName = userSiteManageModel.SiteUserName,
                SiteUserPwd = CryptoUtility.TripleDESEncrypt(userSiteManageModel.SiteUserPwd),
                SiteStatus = 1,
                AddTime = DateTime.Now
            };
            ncBase.CurrentEntities.UserSiteManage.AddObject(userSiteManage);
            return ncBase.CurrentEntities.SaveChanges();
        }
        #endregion

        #region 删除绑定
        public virtual int DeleteUserSite(int userId, int siteId)
        {
            DbCommand cmd = GetStoredProcCommand("P_Api_DeleteUserSite");
            AddInParameter(cmd, "@userId", DbType.Int32, userId);
            AddInParameter(cmd, "@siteId", DbType.Int32, siteId);
             return ExecuteNonQuery(cmd);
        }
        #endregion

        #region 修改绑定密码
        public virtual int UpdateUserSitePwd(int userId, int siteId, string pwd)
        {
            DbCommand cmd = GetStoredProcCommand("P_Api_UpdateUserSitePwd");
            AddInParameter(cmd, "@userId", DbType.Int32, userId);
            AddInParameter(cmd, "@siteId", DbType.Int32, siteId);
            AddInParameter(cmd, "@pwd", DbType.String, CryptoUtility.TripleDESEncrypt(pwd));
            return ExecuteNonQuery(cmd);
        }

        #endregion

        #region EF关系隐射
        private List<UserSiteManageModel> EFToModelList(List<UserSiteManage> userSiteManageList)
        {
            return (from item in userSiteManageList select EFToModel(item)).ToList();
        }
        private UserSiteManageModel EFToModel(UserSiteManage userSiteManage)
        {
            return new UserSiteManageModel()
            {
                AddTime = userSiteManage.AddTime,
                SiteID = userSiteManage.SiteID,
                SiteStatus = userSiteManage.SiteStatus,
                SiteUserName = userSiteManage.SiteUserName,
                SiteUserPwd = CryptoUtility.TripleDESDecrypt(userSiteManage.SiteUserPwd),
                UserID = userSiteManage.UserID,
                SiteUserID = userSiteManage.SiteUserID,
                BanTime = userSiteManage.BanTime,
                BanText = userSiteManage.BanText,
                LimitOperation = userSiteManage.LimitOperation,
                RepeatOperation = userSiteManage.RepeatOperation,
                PlaceOperation = userSiteManage.PlaceOperation
            };
        }
        #endregion
    }
}
