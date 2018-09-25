using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using log4net;
using ZJB.Api.Models;
using System.Data;
using System.Data.Common;
namespace ZJB.Api.DAL
{
    public class PostLogDal:BaseDal
    {
        public PostLogDal()
            : base("WX")
        { }
        private ILog logger = LogManager.GetLogger("PostLogDal");

        /// <summary>
        /// 发布日志列表
        /// </summary>
        /// <returns></returns>
        public virtual List<PostLogModel> GetPostLogList(PostLogListParames parames,ref int totalSize)
        {
            DbCommand cmd = GetStoredProcCommand("P_Api_PostLogList");
            AddInParameter(cmd, "@communityName", DbType.String, parames.communityName);
            AddInParameter(cmd, "@houseId", DbType.Int32, parames.houseId);
            AddInParameter(cmd, "@pageIndex", DbType.Int32, parames.pageIndex);
            AddInParameter(cmd, "@pageSize", DbType.Int32, parames.pageSize);
            AddInParameter(cmd, "@siteId", DbType.Int32, parames.siteId);
            AddInParameter(cmd, "@status", DbType.Int32, parames.status);
            AddInParameter(cmd, "@time", DbType.Date, parames.time);
            AddInParameter(cmd, "@title", DbType.String, parames.title);
            AddInParameter(cmd, "@userId", DbType.Int32, parames.userId);
            AddOutParameter(cmd, "@totalSize", DbType.Int32, 4);
            DataSet ds = ExecuteDataSet(cmd);
            int.TryParse(cmd.Parameters["@totalSize"].Value.ToString(),out totalSize);
            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                return BuildModelToList(ds.Tables[0].Select());
            }
            return null;
        }
        /// <summary>
        /// 发布日志--房源查看统计
        /// </summary>
        /// <param name="parames"></param>
        /// <param name="totalSize"></param>
        /// <returns></returns>
        public virtual List<PostLogModel> GetPostLogGroupByHouseId(PostLogListParames parames, ref int totalSize)
        {
            DbCommand cmd = GetStoredProcCommand("P_Api_GetPostLogGroupByHouseId");
            AddInParameter(cmd, "@userId", DbType.Int32, parames.userId);
            AddInParameter(cmd, "@pageIndex", DbType.Int32, parames.pageIndex);
            AddInParameter(cmd, "@pageSize", DbType.Int32, parames.pageSize);
            AddOutParameter(cmd, "@totalSize", DbType.Int32, 4);
            DataSet ds = ExecuteDataSet(cmd);
            int.TryParse(cmd.Parameters["@totalSize"].Value.ToString(), out totalSize);
            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                return BuildModelToList(ds.Tables[0].Select());
            }
            return null;
        }
        /// <summary>
        /// 发布日志--网站查看统计
        /// </summary>
        /// <param name="parames"></param>
        /// <param name="totalSize"></param>
        /// <returns></returns>
        public virtual List<PostLogModel> GetPostLogGroupByWebSite(int userid)
        {
            DbCommand cmd = GetStoredProcCommand("P_Api_GetPostLogGroupByWebSite");
            AddInParameter(cmd, "@userId", DbType.Int32, userid);
            DataSet ds = ExecuteDataSet(cmd);
            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                return BuildModelToList(ds.Tables[0].Select());
            }
            return null;
        }
        /// <summary>
        /// 后台管理-发布日志--网站查看统计 --
        /// </summary>
        /// <param name="parames"></param>
        /// <param name="totalSize"></param>
        /// <returns></returns>
        public virtual List<PostLogModel> GetPostLogGroupByWebSiteAdmin(int userid,int cityId=592)
        {
            DbCommand cmd = GetStoredProcCommand("P_Api_GetPostLogGroupByWebSite_Admin");
            AddInParameter(cmd, "@userId", DbType.Int32, userid);
            AddInParameter(cmd, "@cityId", DbType.Int32, cityId);
            DataSet ds = ExecuteDataSet(cmd);
            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                return BuildModelToList(ds.Tables[0].Select());
            }
            return null;
        }
        #region 绑定实体
        private List<PostLogModel> BuildModelToList(IEnumerable<DataRow> rows)
        {
            return (from row in rows select BuildModel(row)).ToList();
        }
        private PostLogModel BuildModel(DataRow dr)
        {
            return new PostLogModel()
            {
                DateTime = dr.Table.Columns.Contains("DateTime") ? To<DateTime>(dr, "DateTime") : DateTime.Now,
                ID = dr.Table.Columns.Contains("ID") ? To<Int32>(dr, "ID") : 0,
                InfoID = dr.Table.Columns.Contains("InfoID") ? To<Int32>(dr, "InfoID") : 0,
                SiteID = dr.Table.Columns.Contains("SiteID") ? To<Int32>(dr, "SiteID") : 0,
                Logo = dr.Table.Columns.Contains("Logo") ? To<string>(dr, "Logo") : "",
                Status = dr.Table.Columns.Contains("Status") ? To<Int32>(dr, "Status") : 0,
                TargetID = dr.Table.Columns.Contains("TargetID") ? To<string>(dr, "TargetID") : "",
                UserID = dr.Table.Columns.Contains("UserID") ? To<Int32>(dr, "UserID") : 0,
                TargetUrl = dr.Table.Columns.Contains("TargetUrl") ? To<string>(dr, "TargetUrl") : "",
                IsOrder = dr.Table.Columns.Contains("IsOrder") ? To<Int32>(dr, "IsOrder") : 0,
                Msg = dr.Table.Columns.Contains("Description") ? To<string>(dr, "Description") : "",
                RealyMsg = dr.Table.Columns.Contains("Msg") ? To<string>(dr, "Msg") : "",
                SiteUserName = dr.Table.Columns.Contains("SiteUserName") ? To<string>(dr, "SiteUserName") : "",
                TradeType = dr.Table.Columns.Contains("TradeType") ? To<Int32>(dr, "TradeType") : 0,
                BuildType = dr.Table.Columns.Contains("BuildType") ? To<Int32>(dr, "BuildType") : 0,
                CommunityName = dr.Table.Columns.Contains("CommunityName") ? To<string>(dr, "CommunityName") : "",
                BuildArea = dr.Table.Columns.Contains("BuildArea") ? To<decimal>(dr, "BuildArea") : 0,
                CurFloor = dr.Table.Columns.Contains("CurFloor") ? To<Int32>(dr, "CurFloor") : 0,
                MaxFloor = dr.Table.Columns.Contains("MaxFloor") ? To<Int32>(dr, "MaxFloor") : 0,
                Price = dr.Table.Columns.Contains("Price") ? To<decimal>(dr, "Price") : 0,
                PriceUnit = dr.Table.Columns.Contains("PriceUnit") ? To<string>(dr, "PriceUnit") : "",
                Title = dr.Table.Columns.Contains("Title") ? To<string>(dr, "Title") : "",
                SiteName = dr.Table.Columns.Contains("SiteName") ? To<string>(dr, "SiteName") : "",
                PostCount = dr.Table.Columns.Contains("postCount") ? To<Int32>(dr, "postCount") : 0,
                BeginTime = dr.Table.Columns.Contains("BeginTime") ? To<DateTime>(dr, "BeginTime") : DateTime.Now,
                PostType = dr.Table.Columns.Contains("PostType") ? To<Int32>(dr, "PostType") : 0,
                ReRelease = dr.Table.Columns.Contains("ReRelease") ? To<Int32>(dr, "ReRelease") : 0,
            };
        }
        #endregion

        #region 后台管理 房源推送统计
        public virtual List<StatModel> GetPostStat(StatReq parame)
        {
            DbCommand cmd = GetStoredProcCommand("P_Api_GetPostLogStat");
            AddInParameter(cmd, "@ps", DbType.Int32, parame.ps);
            AddInParameter(cmd, "@currentTime", DbType.Date, parame.currentTime);
            DataSet ds = ExecuteDataSet(cmd);
            List<StatModel> statList = new List<StatModel>();
            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    statList.Add(new StatModel()
                    {
                        Count = dr.Table.Columns.Contains("Counts") ? To<Int32>(dr, "Counts") : 0,
                        SuccessCount = dr.Table.Columns.Contains("SuccessCount") ? To<Int32>(dr, "SuccessCount") : 0,
                        FailCount = dr.Table.Columns.Contains("FailCount") ? To<Int32>(dr, "FailCount") : 0,
                        Time = dr.Table.Columns.Contains("Times") ? To<DateTime>(dr, "Times") : DateTime.Now
                    });
                }
            }
            return statList;
        }
        #endregion
        #region 后台管理 站点分析 --总的发布分布 成功分布 失败分布
        public virtual List<StatModel> GetSiteAnalyseData(StatReq parame)
        {
            DbCommand cmd = GetStoredProcCommand("P_Stat_PushSite");
            AddInParameter(cmd, "@startTime", DbType.Date, parame.startTime);
            AddInParameter(cmd, "@endTime", DbType.Date, parame.endTime);
            AddInParameter(cmd, "@status", DbType.Int32, parame.status);
            DataSet ds = ExecuteDataSet(cmd);
            List<StatModel> statList = new List<StatModel>();
            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    statList.Add(new StatModel()
                    {
                        Count = dr.Table.Columns.Contains("Counts") ? To<Int32>(dr, "Counts") : 0,
                        SiteName = dr.Table.Columns.Contains("SiteName") ? To<string>(dr, "SiteName") : "",
                        TipContent = dr.Table.Columns.Contains("TipContent") ? To<string>(dr, "TipContent") : ""
                    });
                }
            }
            return statList;
        }
        #endregion
    }
}
