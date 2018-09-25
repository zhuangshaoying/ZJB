using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ZJB.Api.BLL;
using ZJB.Api.Entity;
using System.Data;
using System.Data.Common;
using log4net;
using ZJB.Api.Models;
namespace ZJB.Api.DAL
{
    public class RefreshDal:BaseDal
    {
        public RefreshDal()
            : base("WX")
        { }
        private ILog logger = LogManager.GetLogger("RefreshDal");
        private NCBaseRule ncBase = new NCBaseRule();
        #region 刷新设置
        /// <summary>
        /// 刷新计划的开关 
        /// </summary>
        /// <param name="item"></param>
        /// <returns>大于0 成功 -1:不支持云刷新,-2:没有绑定账号</returns>
        public virtual int RefreshPlanState(RefreshPlan item)
        {
            DbCommand cmd = GetStoredProcCommand("P_RefreshPlan_State");
            AddInParameter(cmd, "@UserId", DbType.Int32, item.UserId);
            AddInParameter(cmd, "@SiteId", DbType.Int32, item.SiteId);
            AddOutParameter(cmd, "@planId", DbType.Int32, 4);
            ExecuteNonQuery(cmd);
            int resultId=0;
            int.TryParse(cmd.Parameters["@planId"].Value.ToString(), out resultId);
            return resultId;
        }
        #endregion
        #region 刷新日志列表
        /// <summary>
        /// 
        /// </summary>
        /// <param name="parame"></param>
        /// <param name="totalSize"></param>
        /// <returns></returns>
        public virtual List<RefreshLogModel> RefreshLogList(RefreshLogListReq parame,ref int totalSize)
        {
            DbCommand cmd = GetStoredProcCommand("P_Refresh_LogList");
            AddInParameter(cmd, "@pi", DbType.Int32, parame.pi);
            AddInParameter(cmd, "@ps", DbType.Int32, parame.ps);
            AddInParameter(cmd, "@UserId", DbType.Int32, parame.UserId);
            AddInParameter(cmd, "@SiteId", DbType.Int32, parame.SiteId);
            AddInParameter(cmd, "@SiteUserName", DbType.String, parame.SiteUserName);
            AddInParameter(cmd, "@ViewData", DbType.Int32, parame.ViewData);
            AddInParameter(cmd, "@RefreshMode", DbType.Int32, parame.RefreshMode);
            AddInParameter(cmd, "@PlanNo", DbType.Int32, parame.PlanNo);
            AddInParameter(cmd, "@Time", DbType.Date, parame.Time);
            AddOutParameter(cmd, "@totalSize", DbType.Int32, 4);
            DataSet ds= ExecuteDataSet(cmd);
            List<RefreshLogModel> refreshLogList = new List<RefreshLogModel>();
            int.TryParse(cmd.Parameters["@totalSize"].Value.ToString(), out totalSize);
            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                refreshLogList = BuildToModelList(ds.Tables[0].Select());
            }
            return refreshLogList;
        }
        #endregion
        #region 刷新设置
        public virtual int RefreshDetailAdd(RefreshPlan plan, List<RefreshDetail> details)
        {
            DbCommand cmd = GetStoredProcCommand("P_Refresh_DetailAdd");
            AddInParameter(cmd, "@PlanId", DbType.Int32, plan.PlanId);
            AddInParameter(cmd, "@UserId", DbType.Int32, plan.UserId);
            AddInParameter(cmd, "@TradeType", DbType.Int32, plan.TradeType);
            AddInParameter(cmd, "@BuildType", DbType.Int32, plan.BuildType);
            AddInParameter(cmd, "@BeginHour", DbType.Int32, plan.BeginHour);
            AddInParameter(cmd, "@BeginMinute", DbType.Int32, plan.BeginMinute);
            AddInParameter(cmd, "@EndHour", DbType.Int32, plan.EndHour);
            AddInParameter(cmd, "@EndMinute", DbType.Int32, plan.EndMinute);
            AddInParameter(cmd, "@IntervalTime", DbType.Int32, plan.IntervalTime);
            AddInParameter(cmd, "@CountPerTime", DbType.Int32, plan.CountPerTime);
            AddInParameter(cmd, "@SiteId", DbType.Int32, plan.SiteId);
            AddInParameter(cmd,"@RefreshDetail",DbType.Xml,ZJB.Core.Utilities.XmlUtility.Serialize(details,Encoding.UTF8,"RefreshDetailList"));
            AddReturnParameter(cmd,"@ReturnValue",DbType.Int32);
            ExecuteNonQuery(cmd);
            int result=0;
            int.TryParse(cmd.Parameters["@ReturnValue"].Value.ToString(),out result);
           return result;
            
        }
        #endregion
        #region 刷新删除
        public virtual int RefreshSetDelete(int userId, string planIds)
        {
            DbCommand cmd = GetStoredProcCommand("P_RefreshDelete");
            AddInParameter(cmd, "@UserId", DbType.Int32, userId);
            AddInParameter(cmd, "@PlanIds", DbType.String, planIds);
            return ExecuteNonQuery(cmd);
        }
        #endregion
        #region 获取用户相关的可云刷新的站点
        public List<SiteManageModel> GetUserRefreshWeb(int userId)
        {
            DbCommand cmd = GetStoredProcCommand("P_GetRefreshWeb");
            AddInParameter(cmd, "@UserId", DbType.Int32, userId);
            DataSet ds = ExecuteDataSet(cmd);
            List<SiteManageModel> siteList=new List<SiteManageModel>();
            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                siteList=ToSiteManageModelList(ds.Tables[0].Select());
            }
            return siteList;
        }
        private List<SiteManageModel> ToSiteManageModelList(IEnumerable<DataRow> rows)
        {
            return (from dr in rows select ToSiteManageModel(dr)).ToList();
        }
        private SiteManageModel ToSiteManageModel(DataRow dr)
        {
            return new SiteManageModel
            {
                SiteID =dr.Table.Columns.Contains("SiteID")? To<Int32>(dr, "SiteID"):0,
                Logo = dr.Table.Columns.Contains("Logo") ? To<string>(dr, "Logo") : "",
                SiteUserName = dr.Table.Columns.Contains("SiteUserName") ? To<string>(dr, "SiteUserName") : "",
                State = dr.Table.Columns.Contains("State") ? To<Int32>(dr, "State") : 0,
            };
        }
        #endregion
        #region 私有方法
        private List<RefreshLogModel> BuildToModelList(IEnumerable<DataRow> rows)
        {
            return (from row in rows select BuildToModel(row)).ToList();
        }
        private RefreshLogModel BuildToModel(DataRow dr)
        {
            return new RefreshLogModel()
            {
                BuildType = dr.Table.Columns.Contains("BuildType")?To<Int32>(dr, "BuildType"):0,
                DateTime = dr.Table.Columns.Contains("DateTime") ? To<DateTime>(dr, "DateTime") : new DateTime(1970,1,1),
                Houses = dr.Table.Columns.Contains("Houses") ? To<string>(dr, "Houses") :"",
                Id = dr.Table.Columns.Contains("Id") ? To<Int32>(dr, "Id") : 0,
                Msg = dr.Table.Columns.Contains("Msg") ? To<string>(dr, "Msg") : "",
                ClientMsg = dr.Table.Columns.Contains("ClientMsg") ? To<string>(dr, "ClientMsg") : "",
                PlanId = dr.Table.Columns.Contains("PlanId") ? To<Int32>(dr, "PlanId") : 0,
                PlanNo = dr.Table.Columns.Contains("PlanNo") ? To<Int32>(dr, "PlanNo") : 0,
                RefreshMode = dr.Table.Columns.Contains("RefreshMode") ? To<Int32>(dr, "RefreshMode") : 0,
                SiteId = dr.Table.Columns.Contains("SiteId") ? To<Int32>(dr, "SiteId") : 0,
                SiteName = dr.Table.Columns.Contains("SiteName") ? To<string>(dr, "SiteName") : "",
                SiteUserName = dr.Table.Columns.Contains("SiteUserName") ? To<string>(dr, "SiteUserName") : "",
                Status = dr.Table.Columns.Contains("Status") ? To<Int32>(dr, "Status") : 0,
                UserID = dr.Table.Columns.Contains("UserID") ? To<Int32>(dr, "UserID") : 0,
                RefreshNum = dr.Table.Columns.Contains("RefreshNum") ? To<Int32>(dr, "RefreshNum") : 0,
            };
        }
        #endregion
    }
}
