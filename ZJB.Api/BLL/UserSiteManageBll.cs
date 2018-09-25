using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ZJB.Api.DAL;
using ZJB.Api.Models;
using ZJB.Core.Injection;

namespace ZJB.Api.BLL
{
    public class UserSiteManageBll
    {
        private readonly UserSiteManageDal userSiteManageDal = Container.Instance.Resolve<UserSiteManageDal>();
        public virtual List<UserSiteManageModel> GetUserSiteListByUserId(int userId)
        {
            return userSiteManageDal.GetUserSiteListByUserId(userId);
        }
        #region 绑定站点
        public virtual int UserSiteManageAdd(UserSiteManageModel userSiteManageModel)
        {
            return userSiteManageDal.UserSiteManageAdd(userSiteManageModel);
        }
        #endregion
        #region 删除绑定
        public virtual int DeleteUserSite(int userId,int siteId)
        {
            return userSiteManageDal.DeleteUserSite(userId, siteId);
        }
        #endregion

        #region 修改绑定密码
        public virtual int UpdateUserSitePwd(int userId, int siteId,string pwd)
        {
            return userSiteManageDal.UpdateUserSitePwd(userId, siteId, pwd);
        }
        
        #endregion


     
        
    }
}
