using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using log4net;
using ZJB.Api.BLL;
using ZJB.Api.Entity;
using System.Data;
using System.Data.Common;
using ZJB.Web.Utilities;
using ZJB.Core.Utilities;
using ZJB.Api.Models;
namespace ZJB.Api.DAL
{
    public class DynamicDal:BaseDal
    {
        public DynamicDal()
            : base("WX")
        { }
        private ILog logger = LogManager.GetLogger("DynamicDal");
        private NCBaseRule ncBase = new NCBaseRule();
        #region 发布动态和回复
        /// <summary>
        /// 发布动态
        /// </summary>
        /// <param name="item"></param>
        /// <returns>返回动态id 小于0 则失败</returns>
        public virtual int DynamicAdd(DynamicModel item)
        {
            DbCommand cmd = GetStoredProcCommand("P_Dynamic_Add");
            AddInParameter(cmd, "@UserId", DbType.Int32, item.UserId);
            AddInParameter(cmd, "@Type", DbType.Int32, item.Type);
            AddInParameter(cmd, "@Title", DbType.String, item.Title);
            AddInParameter(cmd, "@DynamicContent", DbType.String, item.DynamicContent);
            AddInParameter(cmd, "@Ip", DbType.String, IpUtility.GetIp());
            AddInParameter(cmd, "@Lat", DbType.Decimal, item.Lat);
            AddInParameter(cmd, "@Lng", DbType.Decimal, item.Lng);
            AddInParameter(cmd, "@CityId", DbType.Int32, item.CityId);
            AddInParameter(cmd, "@Location", DbType.String, item.Location);
            if (item.ImageList!=null&&item.ImageList.Count > 0)
            {
                AddInParameter(cmd, "@Images", DbType.Xml, XmlUtility.Serialize(item.ImageList, Encoding.UTF8, "Images"));
            }
            AddInParameter(cmd, "@Visible", DbType.Int32, item.Visible);
            AddInParameter(cmd, "@IsMentionAll", DbType.Int32, item.IsMentionAll);
            AddOutParameter(cmd, "@DynamicId", DbType.Int32, 4);
            ExecuteNonQuery(cmd);
            int outId = 0;
            int.TryParse(cmd.Parameters["@DynamicId"].Value.ToString(), out outId);
            return outId;
        }
       /// <summary>
        /// 发布回复
       /// </summary>
       /// <param name="item"></param>
       /// <returns></returns>
        public virtual int DynamicCommentAdd(DynamicModel item)
        {
            DbCommand cmd = GetStoredProcCommand("P_DynamicComment_Add");
            AddInParameter(cmd, "@UserId", DbType.Int32, item.UserId);
            AddInParameter(cmd, "@DynamicId", DbType.Int32, item.Id);
            AddInParameter(cmd, "@Comment", DbType.String, item.DynamicContent);
            AddInParameter(cmd, "@DynamicCommentId", DbType.Int32, item.ReplyCommentId);
            AddOutParameter(cmd, "@outid", DbType.Int32, 4);
            if (item.ImageList != null && item.ImageList.Count > 0)
            {
                AddInParameter(cmd, "@Images", DbType.Xml, XmlUtility.Serialize(item.ImageList, Encoding.UTF8, "Images"));
            }
            ExecuteNonQuery(cmd);
            int outId = 0;
            int.TryParse(cmd.Parameters["@outid"].Value.ToString(), out outId);
            return outId;
        }
        #endregion
        #region 动态列表和回复列表
        /// <summary>
        /// 动态列表
        /// </summary>
        /// <param name="parame"></param>
        /// <returns></returns>
        public virtual List<DynamicModel> DynamicList(DynamicListReq parame)
        {
            DbCommand cmd = GetStoredProcCommand("P_Dynamic_List");
            AddInParameter(cmd, "@UserId", DbType.Int32, parame.UserId);
            AddInParameter(cmd, "@CityId", DbType.Int32, parame.CityId);
            AddInParameter(cmd, "@LastTime", DbType.DateTime, parame.LastTime);
            AddInParameter(cmd, "@IsGetSupport", DbType.Int32, parame.IsGetSupport);
            AddInParameter(cmd, "@PageSize", DbType.Int32, parame.PageSize);
            AddInParameter(cmd, "FirstComming", DbType.Int32, parame.FirstComming);
            List<DynamicModel> dynamicList = new List<DynamicModel>();
            List<DynamicSupportModel> dynamicSupportList = new List<DynamicSupportModel>();
            DataSet ds = ExecuteDataSet(cmd);
            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
             
               if (parame.IsGetSupport == 1 && ds.Tables.Count>1&& ds.Tables[1].Rows.Count > 0)///赞列表
               {
                   dynamicSupportList = BuildToSupportModelList(ds.Tables[1].Select());
               }
               dynamicList = BuildToModelList(ds.Tables[0].Select(),dynamicSupportList);
            }
            return dynamicList;
        }
        /// <summary>
        /// 回复列表
        /// </summary>
        /// <param name="dynamicIds"></param>
        /// <returns></returns>
        public virtual List<DynamicModel> DynamicReplayListBydynamicIds(string dynamicIds, int pageSize=5,int lastId=0)
        {
            DbCommand cmd = GetStoredProcCommand("P_Dynamic_ReplayList");
            AddInParameter(cmd, "@dynamicIds", DbType.String, dynamicIds);
            AddInParameter(cmd, "@pageSize", DbType.Int32, pageSize);
            AddInParameter(cmd, "@lastId", DbType.Int32, lastId);
            List<DynamicModel> dynamicList = new List<DynamicModel>();
            DataSet ds = ExecuteDataSet(cmd);
            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                return BuildToModelList(ds.Tables[0].Select());
            }
            return dynamicList;
        }
        private List<DynamicModel> BuildToModelList(IEnumerable<DataRow> rows,List<DynamicSupportModel> supportList=null)
        {
            return (from row in rows select BuildToModel(row,supportList)).ToList();
        }
        private DynamicModel BuildToModel(DataRow row, List<DynamicSupportModel> supportList)
        {
            int dynamicId=row.Table.Columns.Contains("Id") ? To<Int32>(row, "Id") : 0;
            return new DynamicModel()
            {
                Abstract = row.Table.Columns.Contains("Abstract") ? To<string>(row, "Abstract") : "",
                AddTime = row.Table.Columns.Contains("AddTime") ? To<DateTime>(row, "AddTime") : DateTime.Now,
                CityId = row.Table.Columns.Contains("CityId") ? To<Int32>(row, "CityId") : 0,
                CityName = row.Table.Columns.Contains("CityName") ? To<string>(row, "CityName") : "",
                ClickNum = row.Table.Columns.Contains("ClickNum") ? To<Int32>(row, "ClickNum") : 0,
                CommentNum = row.Table.Columns.Contains("CommentNum") ? To<Int32>(row, "CommentNum") : 0,
                DynamicContent = row.Table.Columns.Contains("DynamicContent") ? To<string>(row, "DynamicContent") : "",
                Id = dynamicId,
                Ip = row.Table.Columns.Contains("Ip") ? To<string>(row, "Ip") : "",
                IsEssence = row.Table.Columns.Contains("IsEssence") ? To<bool>(row, "IsEssence") : false,
                IsHelp = row.Table.Columns.Contains("IsHelp") ? To<bool>(row, "IsHelp") : false,
                IsHot = row.Table.Columns.Contains("IsHot") ? To<bool>(row, "IsHot") : false,
                IsMentionAll = row.Table.Columns.Contains("IsMentionAll") ? To<bool>(row, "IsMentionAll") : false,
                IsTop = row.Table.Columns.Contains("IsTop") ? To<bool>(row, "IsTop") : false,
                LastCommentId = row.Table.Columns.Contains("LastCommentId") ? To<Int32>(row, "LastCommentId") : 0,
                LastCommentTime = row.Table.Columns.Contains("LastCommentTime") ? To<DateTime>(row, "LastCommentTime") : DateTime.Now,
                Lat = row.Table.Columns.Contains("Lat") ? To<decimal>(row, "Lat") : 0,
                Lng = row.Table.Columns.Contains("Lng") ? To<decimal>(row, "Lng") : 0,
                Location = row.Table.Columns.Contains("Location") ? To<string>(row, "Location") : "",
                Operator = row.Table.Columns.Contains("Operator") ? To<Int32>(row, "Operator") : 0,
                ShareNum = row.Table.Columns.Contains("ShareNum") ? To<Int32>(row, "ShareNum") : 0,
                State = row.Table.Columns.Contains("State") ? To<Int32>(row, "State") : 0,
                Title = row.Table.Columns.Contains("Title") ? To<string>(row, "Title") : "",
                TopTime = row.Table.Columns.Contains("TopTime") ? To<DateTime>(row, "TopTime") : DateTime.Now,
                Type = row.Table.Columns.Contains("Type") ? To<Int32>(row, "Type") : 0,
                UserId = row.Table.Columns.Contains("UserId") ? To<Int32>(row, "UserId") : 0,
                Visible = row.Table.Columns.Contains("Visible") ? To<Int32>(row, "Visible") : 0,
                VoteId = row.Table.Columns.Contains("VoteId") ? To<Int32>(row, "VoteId") : 0,
                UserName = row.Table.Columns.Contains("UserName") ? To<string>(row, "UserName") : "",
                Portrait = row.Table.Columns.Contains("Portrait") ? To<string>(row, "Portrait") : "",
                ReplayId = row.Table.Columns.Contains("ReplayId") ? To<Int32>(row, "ReplayId") : 0,
                ReplayUserId = row.Table.Columns.Contains("ReplayUserId") ? To<Int32>(row, "ReplayUserId") : 0,
                ReplyCommentId = row.Table.Columns.Contains("ReplyCommentId") ? To<Int32>(row, "ReplyCommentId") : 0,
                ReplayUserName = row.Table.Columns.Contains("ReplayName") ? To<string>(row, "ReplayName") : "",
                SupportList = supportList!=null?supportList.Where(s => s.DynamicId == dynamicId).ToList():new List<DynamicSupportModel>(),
                CompanyName = row.Table.Columns.Contains("CompanyName") ? To<string>(row, "CompanyName") : ""
            };
        }

        private List<DynamicSupportModel> BuildToSupportModelList(IEnumerable<DataRow> rows)
        {
            return (from row in rows select BuildToSupportModel(row)).ToList();
        }
        private DynamicSupportModel BuildToSupportModel(DataRow dr)
        {
            return new DynamicSupportModel()
            {
                AddTime = dr.Table.Columns.Contains("AddTime")?To<DateTime>(dr, "AddTime"):DateTime.Now,
                DynamicId =  dr.Table.Columns.Contains("DynamicId")?To<Int32>(dr, "DynamicId"):0,
                Id =  dr.Table.Columns.Contains("Id")?To<Int32>(dr, "Id"):0,
                Status =  dr.Table.Columns.Contains("Status")?To<Int32>(dr, "Status"):0,
                UserId =  dr.Table.Columns.Contains("UserId")?To<Int32>(dr, "UserId"):0,
                UserName =  dr.Table.Columns.Contains("UserName")?To<string>(dr, "UserName"):"",
            };
        }

        #endregion
        #region 动态的图片列表
        /// <summary>
        /// 动态的图片列表
        /// </summary>
        /// <param name="synamicIds"></param>
        /// <returns></returns>
        public virtual List<DynamicImage> DynamicImageListBydynamicIds(List<int> dynamicIds)
        {
            return (from item in ncBase.CurrentEntities.DynamicImage
                    from id in dynamicIds
                    where item.DynamicId == id
                    select item
                        ).ToList();
            
        }
        
        #endregion
        #region 赞和取消赞
        public virtual int DynamicSupportAdd(int userId, int dynamicId, int status)
        {
            DynamicSupport item = ncBase.CurrentEntities.DynamicSupport.Where(s => s.UserId == userId && s.DynamicId == dynamicId).FirstOrDefault();
            
            if (item.IsNoNull())
            {
                if (status < 0 && status>1)
                {
                    item.STATUS = item.STATUS == 1 ? 0 : 1;
                }
                else
                {
                    item.STATUS = status;
                }
                item.AddTime = DateTime.Now;
                if (item.STATUS == 1)
                {
                    Dynamic dynamicItem = ncBase.CurrentEntities.Dynamic.Where(d => d.Id == dynamicId).FirstOrDefault();
                    if (item.IsNoNull())
                    {
                        dynamicItem.LastCommentTime = DateTime.Now;
                    }
                }
                ncBase.CurrentEntities.SaveChanges();
            }
            else
            {
                item = new DynamicSupport()
                {
                    UserId = userId,
                    DynamicId = dynamicId,
                    AddTime = DateTime.Now,
                    STATUS = 1
                };
                ncBase.CurrentEntities.DynamicSupport.AddObject(item);
                ncBase.CurrentEntities.SaveChanges();
            }
            return item.Id;
        }
        #endregion
        #region 置顶和取消置顶
        /// <summary>
        /// 
        /// </summary>
        /// <param name="lybid"></param>
        /// <param name="otype">1是取消 0是置顶</param>
        /// <param name="days">天数</param>
        /// <returns></returns>
        public virtual int DynamicSetTop(int lybid, int otype, int days)
        {
            DbCommand cmd = GetStoredProcCommand("P_Dynamic_SetTop");
            AddInParameter(cmd, "@lybid", DbType.Int32, lybid);
            AddInParameter(cmd, "@otype", DbType.Int32, otype);
            AddInParameter(cmd, "@days", DbType.Int32, days);
            List<DynamicModel> dynamicList = new List<DynamicModel>();
            return ExecuteNonQuery(cmd);
        }
        #endregion
    }
}
