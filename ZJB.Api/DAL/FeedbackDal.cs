using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ZJB.Api.Models;
using System.Data;
using System.Data.Common;
namespace ZJB.Api.DAL
{
    class FeedbackDal:BaseDal
    {
        public FeedbackDal()
            : base("WX")
        { }
        #region 列表
        public virtual List<FeedbackModel> GetFeedbackList(FeedbackListReq parame,ref int totalSize)
        {
            DbCommand cmd = GetStoredProcCommand("P_Feedback_List");
            AddInParameter(cmd, "@pi", DbType.Int32, parame.pi);
            AddInParameter(cmd, "@ps", DbType.Int32, parame.ps);
            AddOutParameter(cmd, "@totalSize", DbType.Int32, 4);
            DataSet ds = ExecuteDataSet(cmd);
            List<FeedbackModel> feedbackList = new List<FeedbackModel>();
            int.TryParse(cmd.Parameters["@totalSize"].Value.ToString(), out totalSize);
            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                return BuildToModelList(ds.Tables[0].Select());
            }
            return feedbackList;
        }
        private List<FeedbackModel> BuildToModelList(IEnumerable<DataRow> rows)
        { 
            return (from row in rows select BuildToModel(row)).ToList();
        }
        private FeedbackModel BuildToModel(DataRow row)
        {
            return new FeedbackModel(){
               CreateTime=row.Table.Columns.Contains("CreateTime")?To<DateTime>(row,"CreateTime"):new DateTime(1970,1,1),
               FeedbackContent = row.Table.Columns.Contains("FeedbackContent") ? To<string>(row, "FeedbackContent") : "",
               FeedbackId = row.Table.Columns.Contains("FeedbackId") ? To<Int32>(row, "FeedbackId") :0,
               UserId = row.Table.Columns.Contains("UserId") ? To<Int32>(row, "UserId") : 0,
               IsReplay = row.Table.Columns.Contains("IsReplay") ? To<Int32>(row, "IsReplay") : 0,
               UserName = row.Table.Columns.Contains("Name") ? To<string>(row, "Name") : ""
            };
        }
        #endregion
    }
}
