using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.Common;
using System.Text.RegularExpressions;
using ZJB.Api.Model;
using ZJB.Core.Data;


namespace ZJB.Api.DAL
{
    public class WcmdocumentData : BaseDal
    {
        public WcmdocumentData()
            : base("DBTRSWCM")
        {
        }

        /// <summary>
        /// 根据新闻ID获取新闻详细内容
        /// </summary>
        /// <param name="docId">新闻ID</param>
        /// <returns></returns>
        public virtual WcmDocument GetWcmDocumentDetailById(int docId)
        {

            DbCommand dbcommand = GetStoredProcCommand("P_Api_News_GetWcmDocumentDetailById");
            AddInParameter(dbcommand, "@docid", DbType.Int32, docId);
            DataSet objDs = ExecuteDataSet(dbcommand);
            if (objDs == null || objDs.Tables.Count == 0 || objDs.Tables[0].Rows.Count == 0)
                return new WcmDocument();
            return BuildSingleWcmDocument(objDs);
        }

        /// <summary>
        /// 根据新闻ID获取新闻详细内容
        /// </summary>
        /// <param name="docId">新闻ID</param>
        /// <returns></returns>
        public virtual WcmDocument GetAdminWcmDocumentDetailByNewsId(int docId)
        {

            DbCommand dbcommand = GetStoredProcCommand("P_Api_GetWcmDocumentDetailById");
            AddInParameter(dbcommand, "@docid", DbType.Int32, docId);
            DataSet objDs = ExecuteDataSet(dbcommand);
            if (objDs == null || objDs.Tables.Count == 0 || objDs.Tables[0].Rows.Count == 0)
                return new WcmDocument();
            return BuildSingleWcmDocument(objDs);
        }

        /// <summary>
        /// 获取单独某一条新闻的内容
        /// </summary>
        /// <param name="objDs"></param>
        /// <returns></returns>
        private WcmDocument BuildSingleWcmDocument(DataSet objDs)
        {
            WcmDocument wcmDocument = new WcmDocument();
            DataRow row = objDs.Tables[0].Rows[0];
            wcmDocument.DOCID = To<int>(row, "DOCID");
            wcmDocument.DOCTITLE = To<string>(row, "DOCTITLE");
            wcmDocument.SubDocTitle = To<string>(row, "SubDocTitle");
            wcmDocument.DOCCONTENT = To<string>(row, "DOCCONTENT");
            wcmDocument.DOCHTMLCON = To<string>(row, "DOCHTMLCON");
            wcmDocument.DOCPUBTIME = row.Table.Columns.Contains("DOCPUBTIME") ? To<DateTime>(row, "DOCPUBTIME") : DateTime.Now;
            wcmDocument.DOCRELTIME = row.Table.Columns.Contains("DOCRELTIME") ? To<DateTime>(row, "DOCRELTIME") : DateTime.Now;
            wcmDocument.DOCKEYWORDS = To<string>(row, "DOCKEYWORDS");
            wcmDocument.TITLECOLOR = To<string>(row, "TITLECOLOR");
            wcmDocument.HitsCount = row.Table.Columns.Contains("TITLECOLOR1") ? To<int>(row, "TITLECOLOR1") : 0;
            wcmDocument.VIDEOURL = row.Table.Columns.Contains("VIDEOURL") ? To<string>(row, "VIDEOURL") : "";
            wcmDocument.KDBJ = row.Table.Columns.Contains("KDBJ") ? To<string>(row, "KDBJ") : ""; 
            wcmDocument.KDQM = row.Table.Columns.Contains("KDQM") ? To<string>(row, "KDQM") : ""; 
            wcmDocument.XWBJ = row.Table.Columns.Contains("XWBJ") ? To<string>(row, "XWBJ") : ""; 
            wcmDocument.XWQM = row.Table.Columns.Contains("XWQM") ? To<string>(row, "XWQM") : "";
            wcmDocument.CRUSERTYPE = row.Table.Columns.Contains("CRUSERTYPE") ? To<string>(row, "CRUSERTYPE") : "";
            wcmDocument.replaynum = row.Table.Columns.Contains("ReplyCount") ? To<int>(row, "ReplyCount") : 0;
            wcmDocument.LPID = row.Table.Columns.Contains("LPID") ? To<int>(row, "LPID") : 0;
            wcmDocument.DOCPUBURL = To<string>(row, "DOCPUBURL");
            wcmDocument.DOCCHANNEL =
                row.Table.Columns.Contains("DOCCHANNEL") ? To<int>(row, "DOCCHANNEL") : 0;
            return wcmDocument;
        }

        /// <summary>
        /// 根据具体频道ID获取新闻内容
        /// </summary>
        /// <param name="chanleId">频道ID</param>
        /// <returns></returns>
        public virtual List<WcmDocument> GetWcmDocumentListByChanleId(int chanleId, int topNum = 0)
        {

            return GetWcmDocumentListByChanleId(chanleId.ToString(), topNum, "");
        }

        /// <summary>
        /// 获取新闻列表
        /// </summary>
        /// <param name="chanleIds">频道ID</param>
        /// <param name="topNum">条数</param>
        /// <param name="keyWord">标题搜索关键字</param>
        /// <param name="orderId">排序编号</param>
        /// <param name="days">获取新闻天数</param>
        /// <returns>结果列表</returns>
        public virtual List<WcmDocument> GetWcmDocumentListByChanleId(string chanleIds, int topNum,
                                                                      string keyWord = "",
                                                                      newsOrder orderId = newsOrder.RelTimeDesc,
                                                                      int days = 0)
        {
            return GetWcmDocumentListByChanleId(chanleIds, 1, topNum, keyWord, orderId, days);

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="chanleids"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="keyWord"></param>
        /// <param name="order"></param>
        /// <param name="days"></param>
        /// <returns></returns>
        public virtual List<WcmDocument> GetWcmDocumentListByChanleId(string chanleids, int pageIndex, int pageSize,
                                                                      string keyWord = "",
                                                                      newsOrder order = newsOrder.RelTimeDesc,
                                                                      int days = 0)
        {
            int count = 0;
            return GetWcmDocumentListByChanleId(chanleids, pageIndex, pageSize, ref count, keyWord, order, days);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="chanleids"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="count"></param>
        /// <param name="keyWord"></param>
        /// <param name="order"></param>
        /// <param name="days"></param>
        /// <returns></returns>
        public virtual List<WcmDocument> GetWcmDocumentListByChanleId(string chanleids, int pageIndex, int pageSize,
                                                                      ref int count,
                                                                      string keyWord = "",
                                                                      newsOrder order = newsOrder.RelTimeDesc,
                                                                      int days = 0)
        {
            DbCommand dbcommand = GetStoredProcCommand("P_Api_News_GetWcmDocumentList");

            AddInParameter(dbcommand, "@pageindex", DbType.Int32, pageIndex);
            AddInParameter(dbcommand, "@pagesize", DbType.Int32, pageSize);
            AddInParameter(dbcommand, "@chanleid", DbType.String, chanleids);
            AddInParameter(dbcommand, "@keyword", DbType.String, keyWord);
            AddInParameter(dbcommand, "@orderId", DbType.Int32, (int)order);
            AddInParameter(dbcommand, "@days", DbType.Int32, days);
            AddOutParameter(dbcommand, "@count", DbType.Int32, count);
            DataSet objDs = ExecuteDataSet(dbcommand);
            count = GetOutputParameter<int>(dbcommand, "@count");
            if (objDs == null || objDs.Tables.Count == 0 || objDs.Tables[0].Rows.Count == 0)
                return new List<WcmDocument>();
            return BuildWcmDocumentList(objDs.Tables[0]);
        }

       
        public virtual List<WcmDocument> GetWcmDocumentListByChanleIdRemoveCount(string chanleids,int pageSize,
                                                                      newsOrder order = newsOrder.Docorderpri,
                                                                      int days = 0)
        {
            DbCommand dbcommand = GetStoredProcCommand("P_Api_News_GetWcmDocumentListByChanleId");

            AddInParameter(dbcommand, "@pagesize", DbType.Int32, pageSize);
            AddInParameter(dbcommand, "@chanleid", DbType.String, chanleids);
            AddInParameter(dbcommand, "@orderId", DbType.Int32, (int)order);
            AddInParameter(dbcommand, "@days", DbType.Int32, days);
            DataSet objDs = ExecuteDataSet(dbcommand);
           
            if (objDs == null || objDs.Tables.Count == 0 || objDs.Tables[0].Rows.Count == 0)
                return new List<WcmDocument>();
            return BuildWcmDocumentList(objDs.Tables[0]);
        }


        /// <summary>
        /// 手机版首页推荐新闻
        /// </summary>
        /// <returns></returns>
        public virtual List<WcmDocument> GetMoblieTuiJianNewsList()
        {
            DbCommand dbcommand = GetStoredProcCommand("GetMoblieTuiJianNewsList");
            DataSet objDs = ExecuteDataSet(dbcommand);
            if (objDs == null || objDs.Tables.Count == 0 || objDs.Tables[0].Rows.Count == 0)
                return new List<WcmDocument>();
            return BuildWcmDocumentList(objDs.Tables[0]);
        }

        /// <summary>
        /// 获取新闻列表
        /// </summary>
        /// <param name="chanleIds">频道ID</param>
        /// <param name="topNum">条数</param>
        /// <param name="keyWord">标题搜索关键字</param>
        /// <returns>结果列表</returns>
        public virtual List<WcmDocument> GetWcmDocumentListByChanleId(string chanleIds, int topNum = 0,
                                                                      string keyWord = "")
        {
            newsOrder orderId = newsOrder.RelTimeDesc;
            int days = 0;
            return GetWcmDocumentListByChanleId(chanleIds, topNum, keyWord, orderId, days);
        }



        public virtual List<WcmDocument> GetWcmDocument(string channelIds, int pageIndex, int pageSize, ref int count,
                                                        DateTime dateTime, string keyWord, newsOrder order)
        {
            DbCommand dbcommand = GetStoredProcCommand("P_Api_News_GetWcmDocuments");

            AddInParameter(dbcommand, "@pageindex", DbType.Int32, pageIndex);
            AddInParameter(dbcommand, "@pagesize", DbType.Int32, pageSize);
            AddInParameter(dbcommand, "@chanleid", DbType.String, channelIds);
            AddInParameter(dbcommand, "@keyword", DbType.String, keyWord);
            AddInParameter(dbcommand, "@orderId", DbType.Int32, (int)order);
            AddInParameter(dbcommand, "@datetime", DbType.DateTime, dateTime);
            DataSet objDs = ExecuteDataSet(dbcommand);
            if (objDs == null || objDs.Tables.Count == 0 || objDs.Tables[0].Rows.Count == 0)
                return new List<WcmDocument>();
            count = (int)objDs.Tables[0].Rows[0]["allcount"];
            return BuildWcmDocumentListV2(objDs.Tables[0]);
        }


        private List<WcmDocument> BuildWcmDocumentListV2(DataTable dataTale)
        {
            return (from DataRow row in dataTale.Rows
                    select new WcmDocument
                        {
                            DOCID = To<int>(row, "DOCID"),
                            DOCTITLE = To<string>(row, "DOCTITLE"),
                            SubDocTitle = To<string>(row, "SubDocTitle"),
                            TITLECOLOR = To<string>(row, "TITLECOLOR"),
                            DOCABSTRACT = To<string>(row, "DOCABSTRACT"),
                            DOCRELTIME = To<DateTime>(row, "DOCRELTIME"),
                            DOCPUBURL = To<string>(row, "docpuburl"),
                            isPic = To<int>(row, "isPic"),
                            isVideo = To<int>(row, "isVideo"),
                            HitsCount = To<int>(row, "HitsCount"),
                            replaynum = To<int>(row, "ReplyCount"),
                            DOCCONTENT = To<string>(row, "DOCCONTENT"),
                            APPFILE = To<string>(row, "APPFILE"),
                            DOCHTMLCON = To<string>(row, "DOCHTMLCON"),
                            DOCCHANNEL = To<int>(row, "DOCCHANNEL"),
                            CRUSER = To<string>(row, "CRUSER"),
                            CHANNELNAME = To<string>(row, "CHNLNAME"),
                            DOCKEYWORDS = To<string>(row, "DOCKEYWORDS"),
                            SjTitle = To<string>(row, "HNSOURCE")
                        }).ToList();
        }


        private List<WcmDocument> BuildWcmDocumentList(DataTable dataTale)
        {
            List<WcmDocument> wcmDocuments = new List<WcmDocument>();
            foreach (DataRow row in dataTale.Rows)
            {
                WcmDocument wcmDocument = new WcmDocument();
                wcmDocument.DOCID = To<int>(row, "DOCID");
                wcmDocument.DOCTITLE = To<string>(row, "DOCTITLE");
                wcmDocument.SubDocTitle = To<string>(row, "SubDocTitle");
                wcmDocument.TITLECOLOR = To<string>(row, "TITLECOLOR");
                wcmDocument.DOCABSTRACT = To<string>(row, "DOCABSTRACT");
                wcmDocument.DOCRELTIME = To<DateTime>(row, "DOCRELTIME");
                wcmDocument.DOCPUBTIME = row.Table.Columns.Contains("DOCPUBTIME") ? To<DateTime>(row, "DOCPUBTIME") : DateTime.Now;
                wcmDocument.CRTIME = row.Table.Columns.Contains("CRTIME") ? To<DateTime>(row, "CRTIME") : DateTime.Now;
                
                wcmDocument.DOCPUBURL = To<string>(row, "docpuburl");
                wcmDocument.isPic = To<int>(row, "isPic");
                wcmDocument.isVideo = To<int>(row, "isVideo");
                wcmDocument.HitsCount = To<int>(row, "HitsCount");
                wcmDocument.replaynum = To<int>(row, "ReplyCount");
                wcmDocument.DOCCONTENT = To<string>(row, "DOCCONTENT");
                wcmDocument.APPFILE = row.Table.Columns.Contains("APPFILE") ? To<string>(row, "APPFILE") :"";
                wcmDocument.DOCHTMLCON = To<string>(row, "DOCHTMLCON");
                wcmDocument.DOCCHANNEL = To<int>(row, "DOCCHANNEL");
                wcmDocument.CRUSER = To<string>(row, "CRUSER");
                wcmDocument.CHANNELNAME = row.Table.Columns.Contains("CHNLNAME") ? To<string>(row, "CHNLNAME") : "";
                wcmDocument.DOCKEYWORDS = To<string>(row, "DOCKEYWORDS");
                wcmDocuments.Add(wcmDocument);
            }
            return wcmDocuments;
        }

        /// <summary>
        /// 查询楼盘快递
        /// </summary>
        /// <param name="loupanId">楼盘Id</param>
        /// <param name="pageIndex">分页页码</param>
        /// <param name="pageSize">分页条数</param>
        /// <param name="count">符合条件的总条数</param>
        /// <returns></returns>
        public virtual List<KuaidiInfo> GetKuaidiList(int loupanId, int pageIndex, int pageSize, ref int count)
        {
            List<KuaidiInfo> list = new List<KuaidiInfo>();
            DbCommand cmd = GetStoredProcCommand("p_GetKuaidiListByLoupanId");
            AddInParameter(cmd, "pagesize", DbType.Int32, pageSize);
            AddInParameter(cmd, "pageindex", DbType.Int32, pageIndex);
            AddInParameter(cmd, "loupanId", DbType.String, loupanId);
            DataSet ds = ExecuteDataSet(cmd);
            if (ds == null || ds.Tables.Count == 0 || ds.Tables[0].Rows.Count == 0)
                return new List<KuaidiInfo>();
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                var kd = new KuaidiInfo
                    {
                        Id = To<string>(dr, "id"),
                        Reltime = To<string>(dr, "DocReltime"),
                        LoupanName = To<string>(dr, "loupanName"),
                        Author = To<string>(dr, "docauthor"),
                        HtmlContent = To<string>(dr, "DOCHTMLCON"),
                        NewsUrl = To<string>(dr, "docpuburl"),
                        Content = To<string>(dr, "Doccontent"),
                        LoupanId = To<int>(dr, "LoupanId"),
                        Title = To<string>(dr, "Doctitle"),
                        ReplyCount = To<int>(dr, "ReplyCount"),
                        PageView = To<int>(dr, "hitsCount"),
                        Issuer = To<string>(dr, "CRUSER"),

                    };
                list.Add(kd);
                count = To<int>(dr, "total");
            }
            return list;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public virtual DocChannel GetChannelyId(int id)
        {
            DbCommand cmd = GetStoredProcCommand("p_GetChannelById");
            AddInParameter(cmd, "@id", DbType.Int32, id);
            DataSet objDs = ExecuteDataSet(cmd);
            if (objDs == null && objDs.Tables[0].Rows.Count <= 0)
                return new DocChannel();
            return BuildDocChannel(objDs).FirstOrDefault();

        }

        private IEnumerable<DocChannel> BuildDocChannel(DataSet objDs)
        {
            return (from DataRow row in objDs.Tables[0].Rows
                    select new DocChannel
                        {
                            Channelid = To<int>(row, "CHANNELID"),
                            ChnlName = To<string>(row, "CHNLNAME"),
                            ChnlDes = To<string>(row, "CHNLDESC"),
                            CrateTime = To<DateTime>(row, "CRTIME"),
                            CrateUser = To<string>(row, "CRUSER"),
                            SiteID = (SiteID)To<int>(row, "SITEID")
                        }).ToList();
        }

        /// <summary>
        /// 验证是否楼盘快递发布人
        /// </summary>
        /// <param name="docId"></param>
        /// <param name="loupanId"></param>
        /// <param name="issuer">发布人</param>
        /// <returns></returns>
        public virtual bool CheckIssuer(int docId, int loupanId, string issuer)
        {
            DbCommand cmd =
                GetSqlStringCommand(
                    " SELECT Cruser ,tt.lpid FROM wcmdocument t LEFT JOIN  dbo.SearchPic  tt ON  tt.docid=t.docid WHERE t.docid =@docId  AND tt.lpid =@loupanId ");
            AddInParameter(cmd, "docId", DbType.Int32, docId);
            AddInParameter(cmd, "loupanId", DbType.Int32, loupanId);
            DataSet ds = ExecuteDataSet(cmd);
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                string cruser = To<string>(dr, "Cruser");
                if (cruser.Equals(issuer))
                    return true;
            }
            return false;
        }

        public List<WcmDocument> GetWcmdocumentsByIds(string ids)
        {
            DbCommand cmd = GetStoredProcCommand("p_GetWcmdocumentsByIds");
            AddInParameter(cmd, "@ids", DbType.String, ids);
            DataSet obj = ExecuteDataSet(cmd);
            if (obj == null || obj.Tables[0].Rows.Count <= 0)
                return new List<WcmDocument>();
            return BuildWcmDocumentList(obj.Tables[0]);
        }


        /// <summary>
        /// 获取相关新闻
        /// </summary>
        /// <param name="channelid"></param>
        /// <param name="docid"></param>
        /// <param name="keyword"></param>
        /// <returns></returns>
        public virtual List<WcmDocument> GetRelateNewsList(int channelid, int docid, string keyword)
        {
            var wcmdocmentList = new List<WcmDocument>();
            DbCommand dbCommand = GetStoredProcCommand("P_Api_GetRelateNewsList");
            AddInParameter(dbCommand, "@channel", DbType.Int32, channelid);
            AddInParameter(dbCommand, "@docid", DbType.Int32, docid);
            AddInParameter(dbCommand, "@keywords", DbType.String, keyword);
            DataSet dataSet = ExecuteDataSet(dbCommand);
            if (dataSet == null || dataSet.Tables.Count == 0 || dataSet.Tables[0].Rows.Count == 0)
                return new List<WcmDocument>();
            else
            {
                wcmdocmentList.AddRange(from DataRow dataRow in dataSet.Tables[0].Rows
                                        select new WcmDocument()
                                            {
                                                DOCID = To<int>(dataRow, "docid"),
                                                DOCPUBURL = To<string>(dataRow, "docpuburl"),
                                                DOCTITLE = To<string>(dataRow, "doctitle"),
                                                DOCRELTIME = To<DateTime>(dataRow, "docreltime"),
                                                DOCCONTENT = To<string>(dataRow, "DOCCONTENT")
                                            });
                return wcmdocmentList;
            }
        }

       

        /// <summary>
        /// 添加新闻点击数
        /// </summary>
        /// <returns></returns>
        public virtual int AddHisCount(int docid,int hits)
        {
            DbCommand cmd = GetStoredProcCommand("P_Api_AddNewsHitCount");
            AddInParameter(cmd, "@docid", DbType.String, docid);
            AddInParameter(cmd, "@hits", DbType.String, hits);
            return ExecuteNonQuery(cmd);
        }

        public virtual Dictionary<int, string> GetPicDic(List<int> docidlist)
        {
            var buffer = new StringBuilder();
            foreach (int docid in docidlist)
            {
                buffer.AppendFormat("{0},", docid);
            }
            var result = new Dictionary<int, string>();
            if (buffer.Length == 0)
                return result;

            buffer.Remove(buffer.Length - 1, 1);

            string strSql = string.Format("SELECT APPFILE,APPDOCID FROM dbo.WCMAPPENDIX WHERE APPDOCID IN ({0})", buffer);
            var cmd = GetSqlStringCommand(strSql);
            var dataSet = ExecuteDataSet(cmd);
            if (dataSet == null || dataSet.Tables.Count == 0)
                return result;

            foreach (DataRow row in dataSet.Tables[0].Rows)
            {
                var imageurl = To<string>(row, "APPFILE");
                var docid = To<int>(row, "APPDOCID");
                result.Add(docid, imageurl);
            }

            return result;

        }

      
        /// <summary>
        /// 根据新闻频道、日期获取所有的新闻编号
        /// </summary>
        /// <param name="channelId"></param>
        /// <param name="beginDt"></param>
        /// <param name="endDt"></param>
        /// <returns></returns>
        public virtual List<int> GetNewsDocIdListByDayTj(int channelId, DateTime beginDt, DateTime endDt)
        {
            var docidList = new List<int>();
            DbCommand cmd = GetStoredProcCommand("P_Api_GetNewsDocIdListByDayTj");
            AddInParameter(cmd, "@channelId", DbType.String, channelId);
            AddInParameter(cmd, "@beginDt", DbType.DateTime, beginDt);
            AddInParameter(cmd, "@endDt", DbType.DateTime, endDt);
            DataSet dataSet = ExecuteDataSet(cmd);
            if (dataSet != null && dataSet.Tables.Count > 0 && dataSet.Tables[0].Rows.Count > 0)
            {
                docidList.AddRange(from DataRow dataRow in dataSet.Tables[0].Rows select To<int>(dataRow, "DOCID"));
                return docidList;
            }
            return null;
        }

        /// <summary>
        /// 获取点击数
        /// </summary>
        /// <param name="docidlist"></param>
        /// <returns></returns>
        public virtual Dictionary<int, int> GetHisCounts(List<int> docidlist)
        {
            var buffer = new StringBuilder();
            foreach (int docid in docidlist)
            {
                buffer.AppendFormat("{0},", docid);
            }
            var result = new Dictionary<int, int>();
            if (buffer.Length == 0)
                return result;

            buffer.Remove(buffer.Length - 1, 1);

            string strSql = string.Format("SELECT HitsCount,DOCID FROM dbo.WCMDOCUMENT WHERE DOCID IN  ({0})", buffer);
            var cmd = GetSqlStringCommand(strSql);
            var dataSet = ExecuteDataSet(cmd);
            if (dataSet == null || dataSet.Tables.Count == 0)
                return result;

            foreach (DataRow row in dataSet.Tables[0].Rows)
            {
                var hiscount = To<int>(row, "HitsCount");
                var docid = To<int>(row, "DOCID");
                result.Add(docid, hiscount);
            }

            return result;

        }        /// <summary>
        /// wenda
        /// </summary>
        /// <param name="name"></param>
        /// <param name="userpic"></param>
        /// <param name="sex"></param>
        /// <param name="openid"></param>
        /// <returns></returns>
        public virtual DataSet GetWCMDOCUMENT(string DOCCHANNEL, string DOCKEYWORDS, int pageindex, int pagesize, ref int irows)
        {
            int id = 0;
            DbCommand dbcommand
                = GetStoredProcCommand("GetWCMDOCUMENT");

            AddInParameter(dbcommand, "@DOCCHANNEL", DbType.String, DOCCHANNEL);
            AddInParameter(dbcommand, "@DOCKEYWORDS", DbType.String, DOCKEYWORDS);

            AddInParameter(dbcommand, "@pageIndex", DbType.Int32, pageindex);
            AddInParameter(dbcommand, "@pageSize", DbType.Int32, pagesize);
            AddOutParameter(dbcommand, "@rows", DbType.Int32, irows);

            DataSet dataSet = ExecuteDataSet(dbcommand);
            int.TryParse(dbcommand.Parameters["@rows"].Value.ToString(), out irows);

            return dataSet;
        }
        /// wenda
        /// </summary>
        /// <param name="name"></param>
        /// <param name="userpic"></param>
        /// <param name="sex"></param>
        /// <param name="openid"></param>
        /// <returns></returns>
        public virtual DataSet GetWCMDOCUMENTNews(string title, int pageindex, int pagesize, ref int irows)
        {
            int id = 0;
            DbCommand dbcommand
                = GetStoredProcCommand("GetWCMDOCUMENTNews");

            AddInParameter(dbcommand, "@doctitle", DbType.String, title);

            AddInParameter(dbcommand, "@pageIndex", DbType.Int32, pageindex);
            AddInParameter(dbcommand, "@pageSize", DbType.Int32, pagesize);
            AddOutParameter(dbcommand, "@rows", DbType.Int32, irows);

            DataSet dataSet = ExecuteDataSet(dbcommand);
            int.TryParse(dbcommand.Parameters["@rows"].Value.ToString(), out irows);

            return dataSet;
        }
    }
}

