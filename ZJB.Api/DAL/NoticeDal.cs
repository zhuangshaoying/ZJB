using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using log4net;
using ZJB.Api.BLL;
using ZJB.Api.Entity;
using System.Data;
using System.Data.Common;
using ZJB.Api.Models;
namespace ZJB.Api.DAL
{
    public class NoticeDal:BaseDal
    {
        public NoticeDal()
            : base("WX")
        { }
        private ILog logger = LogManager.GetLogger("NoticeDal");
        private NCBaseRule ncBase = new NCBaseRule();
        /// <summary>
        /// 获取用户未读的公告信息
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public virtual List<Notice> GetNotReadNoticeList(GetNotReadNoticeListReq parame,ref int totalSize)
        {
            DbCommand cmd = GetStoredProcCommand("P_Api_GetNotReadNoticeTipList");
            AddInParameter(cmd, "@userId", DbType.Int32, parame.userId);
            AddInParameter(cmd, "@pi", DbType.Int32, parame.pi);
            AddInParameter(cmd, "@ps", DbType.Int32, parame.ps);
            AddOutParameter(cmd, "@totalSize", DbType.Int32, 4);
            DataSet ds= ExecuteDataSet(cmd);
            int.TryParse(cmd.Parameters["@totalSize"].Value.ToString(), out totalSize);
            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
               return BuildToModelList(ds.Tables[0].Select());
            }
            return new List<Notice>();
        }
        private List<Notice> BuildToModelList(IEnumerable<DataRow> rows)
        {
            return (from row in rows select BuildToModel(row)).ToList();
        }
        private Notice BuildToModel(DataRow dr)
        {
            return new Notice()
            {
                CreateTime = dr.Table.Columns.Contains("CreateTime")?To<DateTime>(dr, "CreateTime"):DateTime.Now,
                Type = dr.Table.Columns.Contains("Type")?To<Int32>(dr, "Type"):0,
                Hits = dr.Table.Columns.Contains("Hits") ? To<Int32>(dr, "Hits") : 0,
                NoticeContent = dr.Table.Columns.Contains("NoticeContent") ?To<string>(dr, "NoticeContent"):"",
                NoticeId = dr.Table.Columns.Contains("NoticeId") ? To<Int32>(dr, "NoticeId") : 0,
                Publisher = dr.Table.Columns.Contains("Publisher") ? To<string>(dr, "Publisher") : "",
                Title = dr.Table.Columns.Contains("Title") ? To<string>(dr, "Title") : "",
                CreateUserId = dr.Table.Columns.Contains("CreateUserId") ? To<Int32>(dr, "CreateUserId") : 0,
                FeedbackId = dr.Table.Columns.Contains("FeedbackId") ? To<Int32>(dr, "FeedbackId") : 0
            };
        }

        public virtual List<Notice> GetNoticeList(GetNoticeListReq parame, ref int totalSize)
        {
            DbCommand cmd = GetStoredProcCommand("P_Api_GetNoticeList");
            AddInParameter(cmd, "@userId", DbType.Int32, parame.userId);
            AddInParameter(cmd, "@pi", DbType.Int32, parame.pi);
            AddInParameter(cmd, "@ps", DbType.Int32, parame.ps);
            AddOutParameter(cmd, "@totalSize", DbType.Int32, 4);
            DataSet ds = ExecuteDataSet(cmd);
            int.TryParse(cmd.Parameters["@totalSize"].Value.ToString(), out totalSize);
            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                return BuildToModelList(ds.Tables[0].Select());
            }
            return new List<Notice>();
        }
        /// <summary>
        /// 设置为已读状态
        /// </summary>
        /// <param name="noticeId"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        public virtual int NoticeSetIsRead(int noticeId, int userId)
        {
            DbCommand cmd = GetStoredProcCommand("P_Api_NoticeLog_SetIsRead");
            AddInParameter(cmd, "@userId", DbType.Int32, userId);
            AddInParameter(cmd, "@noticeId", DbType.Int32, noticeId);
            return ExecuteNonQuery(cmd);
        }

        #region 根据反馈id串 获取回复内容
        public List<Notice> GetReplayListByIds(string feedbackIds)
        {
            DbCommand cmd = GetStoredProcCommand("P_Notice_ReplayList");
            AddInParameter(cmd, "@feedbackIds", DbType.String, feedbackIds);
            DataSet ds = ExecuteDataSet(cmd);
            List<Notice> replayList = new List<Notice>();
            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                return BuildToModelList(ds.Tables[0].Select());
            }
            return replayList;
        }
        #endregion
        /// <summary>
        /// 站内通知 如果type设置为0 不会显示再公告上，只有指定的receiverId私人才收的到
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="receiverId">不填表示群发</param>
        /// <returns></returns>
        public virtual int NoticeAdd(Notice entity,int receiverId=0)
        {
            if (entity.Type == 0 && receiverId == 0)//标记为私人通知 但是接收人id为0
                return 0;
            DbCommand cmd = GetStoredProcCommand("P_Notice_Add");
            AddInParameter(cmd, "@Title", DbType.String, entity.Title);
            AddInParameter(cmd, "@NoticeContent", DbType.String, entity.NoticeContent);
            AddInParameter(cmd, "@Type", DbType.Int32, entity.Type);
            AddInParameter(cmd, "@Publisher", DbType.String, entity.Publisher);
            AddInParameter(cmd, "@CreateUserId", DbType.Int32, entity.CreateUserId);
            AddInParameter(cmd, "@ReceiverId", DbType.Int32, receiverId);
            return ExecuteNonQuery(cmd);
        }
    }
}
