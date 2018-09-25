using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using log4net;
using ZJB.Api.BLL;
using ZJB.Api.Models;
using System.Data;
using ZJB.Core.Utilities;
namespace ZJB.Api.DAL
{
    public class PostManageDal : BaseDal
    {

        public PostManageDal()
            : base("WX")
        {

        }
        private ILog logger = LogManager.GetLogger("PostManageDal");
        private NCBaseRule ncBase = new NCBaseRule();


        #region 新增发布和预约发布

        public virtual int BatchPostManageAdd(int userId, int releaseType, string postList, List<PostLogModel> postLogList)
        {
            DbCommand cmd = GetStoredProcCommand("P_Api_PostManage_BatchAdd");
            StringBuilder postLogData = new StringBuilder();
            AddInParameter(cmd, "@userId", DbType.Int32, userId);
            AddInParameter(cmd, "@postData", DbType.Xml, postList);
            if (postLogList != null)
            {
                postLogData.Append(XmlUtility.Serialize(postLogList, Encoding.UTF8, "PostLogModelList"));
                AddInParameter(cmd, "@postLogData", DbType.Xml, postLogData.ToString());
            }
            AddInParameter(cmd, "@releaseType", DbType.Int32, releaseType);
            return ExecuteNonQuery(cmd);
        }

        #endregion
        #region 预约管理列表
        public virtual List<PostManageModel> GetAppointLogList(AppointLogListReq parame, ref int totalSize)
        {
            DbCommand cmd = GetStoredProcCommand("P_Api_GetAppointLogList");
            AddInParameter(cmd, "@userId", DbType.Int32, parame.userId);
            AddInParameter(cmd, "@time", DbType.Date, parame.time);
            AddInParameter(cmd, "@status", DbType.Int32, parame.status);
            AddInParameter(cmd, "@tradeType", DbType.Int32, parame.tradeType);
            AddInParameter(cmd, "@pi", DbType.Int32, parame.pi);
            AddInParameter(cmd, "@ps", DbType.Int32, parame.ps);
            AddOutParameter(cmd, "@totalSize", DbType.Int32, 4);
            DataSet ds = ExecuteDataSet(cmd);
            int.TryParse(cmd.Parameters["@totalSize"].Value.ToString(), out totalSize);
            List<PostManageModel> appointList=new List<PostManageModel>();
            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                return BuildToModelList(ds.Tables[0].Select());
            }
            return appointList;
        }

        private List<PostManageModel> BuildToModelList(IEnumerable<DataRow> rows)
        {
            return (from row in rows select BuildToModel(row)).ToList();
        }
        private PostManageModel BuildToModel(DataRow dr)
        {
            return new PostManageModel()
            {
                HouseID = dr.Table.Columns.Contains("HouseID") ? To<Int32>(dr, "HouseID") : 0,
                OrderSites = dr.Table.Columns.Contains("OrderSites") ? To<string>(dr, "OrderSites") : "",
                OrderStatus = dr.Table.Columns.Contains("OrderStatus") ? To<Int32>(dr, "OrderStatus") : 0,
                OrderTime = dr.Table.Columns.Contains("OrderTime") ? To<string>(dr, "OrderTime") : "",
                Title = dr.Table.Columns.Contains("Title") ? To<string>(dr, "Title") : "",
                AddTime = dr.Table.Columns.Contains("AddTime") ? To<DateTime>(dr, "AddTime") :DateTime.Now,
                AllSites = dr.Table.Columns.Contains("AllSites") ? To<string>(dr, "AllSites") : ""
                 
            };
        }
        #endregion
        #region 预约删除
        public virtual int DeleteAppoint(int userId, string houseIds)
        {
            DbCommand cmd = GetStoredProcCommand("P_Api_DeleteAppoint");
            AddInParameter(cmd, "@userId", DbType.Int32, userId);
            AddInParameter(cmd, "@houseIds", DbType.String, houseIds);
            return ExecuteNonQuery(cmd);
        }
        #endregion
    }
}
