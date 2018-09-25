using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ZJB.Api.DAL;
using ZJB.Core.Injection;
using ZJB.Api.Entity;
using ZJB.Api.Models;
namespace ZJB.Api.BLL
{
    public class RefreshBll
    {
        private readonly RefreshDal refrehsDal = Container.Instance.Resolve<RefreshDal>();
        #region 刷新设置
        /// <summary>
        /// 刷新计划的开关
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public virtual int RefreshPlanState(RefreshPlan item)
        {
            return refrehsDal.RefreshPlanState(item);
        }
        #endregion
        #region 刷新日志列表
        /// <summary>
        /// 
        /// </summary>
        /// <param name="parame"></param>
        /// <param name="totalSize"></param>
        /// <returns></returns>
        public virtual List<RefreshLogModel> RefreshLogList(RefreshLogListReq parame, ref int totalSize)
        {
            return refrehsDal.RefreshLogList(parame, ref totalSize);
        }
        #endregion

        #region 刷新设置
        public virtual int RefreshDetailAdd(RefreshPlan plan, List<RefreshDetail> details)
        {
            return refrehsDal.RefreshDetailAdd(plan, details);
        }
        #endregion
        #region 刷新删除
        public virtual int RefreshSetDelete(int userId, string planIds)
        {
            return refrehsDal.RefreshSetDelete(userId, planIds);
        }
        #endregion
        #region 获取用户相关的可云刷新的站点
        public List<SiteManageModel> GetUserRefreshWeb(int userId)
        {
            return refrehsDal.GetUserRefreshWeb(userId);
            
        }
        #endregion
    }
}
