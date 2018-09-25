using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using ZJB.Api.DAL;
using ZJB.Api.Model;
using ZJB.Core.Injection;
namespace ZJB.Api
{
    public class NewsBll
    {
        private WcmdocumentData WcmdocumentData = Container.Instance.Resolve<WcmdocumentData>();

        /// <summary>
        /// 根据新闻ID获取新闻详细内容
        /// </summary>
        /// <param name="docId">新闻ID</param>
        /// <returns>返回结果实体类</returns>
        public virtual WcmDocument GetWcmDocumentDetailById(int docId)
        {
            return WcmdocumentData.GetWcmDocumentDetailById(docId);
        }

        /// <summary>
        /// 根据新闻ID获取新闻详细内容
        /// </summary>
        /// <param name="docId">新闻ID</param>
        /// <returns></returns>
        public virtual WcmDocument GetAdminWcmDocumentDetailByNewsId(int docId)
        {
            return WcmdocumentData.GetAdminWcmDocumentDetailByNewsId(docId);
        }


        /// <summary>
        /// 根据具体频道ID获取新闻内容
        /// </summary>
        /// <param name="chanleId">频道ID</param>
        /// <param name="topNum">条数</param>
        /// <returns>返回结果列表</returns>
        public virtual List<WcmDocument> GetWcmDocumentListByChanleId(int chanleId, int topNum = 0)
        {
            return WcmdocumentData.GetWcmDocumentListByChanleId(chanleId, topNum);
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
            return WcmdocumentData.GetWcmDocumentListByChanleId(chanleIds, topNum, keyWord);
        }
          /// <summary>
        /// 手机版首页推荐新闻
        /// </summary>
        /// <returns></returns>
        public virtual List<WcmDocument> GetMoblieTuiJianNewsList()
        {
            return WcmdocumentData.GetMoblieTuiJianNewsList();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="chanleIds"></param>
        /// <param name="pageSize"></param>
        /// <param name="pageIndex"></param>
        /// <param name="count"></param>
        /// <param name="keyWord"></param>
        /// <param name="order"></param>
        /// <returns></returns>
        public virtual List<WcmDocument> GetWcmDocumentsListByChanleIds(string chanleIds, int pageIndex, int pageSize,
                                                                        ref int count, string keyWord = "",
                                                                        newsOrder order = newsOrder.Docorderpri)
        {
            return WcmdocumentData.GetWcmDocumentListByChanleId(chanleIds, pageIndex, pageSize, ref count, keyWord,
                                                                newsOrder.Docorderpri, 0);
        }

        /// <summary>
        /// 获取新闻列表 去除条数
        /// </summary>
        /// <param name="chanleids"></param>
        /// <param name="pageSize"></param>
        /// <param name="order"></param>
        /// <param name="days"></param>
        /// <returns></returns>
        public virtual List<WcmDocument> GetWcmDocumentListByChanleIdRemoveCount(string chanleids, int pageSize,
            newsOrder order = newsOrder.Docorderpri,
            int days = 0)
        {
            return WcmdocumentData.GetWcmDocumentListByChanleIdRemoveCount(chanleids,  pageSize, 
                                                                newsOrder.Docorderpri, 0);
        }


        /// <summary>
        /// 获取新闻列表
        /// </summary>
        /// <param name="chanleIds">频道ID</param>
        /// <param name="topNum">条数</param>
        /// <param name="keyWord">标题搜索关键字</param>
        /// <param name="oredrId">排序方式</param>
        /// <param name="days">天数范围</param>
        /// <returns>结果列表</returns>
        public virtual List<WcmDocument> GetWcmDocumentListByChanleId(string chanleIds, int topNum = 0,
                                                                      string keyWord = "",
                                                                      newsOrder oredrId = newsOrder.RelTimeDesc,
                                                                      int days = 0)
        {
            return WcmdocumentData.GetWcmDocumentListByChanleId(chanleIds, topNum, keyWord, oredrId, days);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="chanleIds"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="count"></param>
        /// <param name="dateTime"></param>
        /// <param name="keyWord"></param>
        /// <param name="order"></param>
        /// <returns></returns>
        public virtual List<WcmDocument> GetWcmDocument(string chanleIds, int pageIndex, int pageSize, ref int count,
                                                        DateTime dateTime, string keyWord,
                                                        newsOrder order = newsOrder.RelTimeDesc)
        {
            return WcmdocumentData.GetWcmDocument(chanleIds, pageIndex, pageSize, ref count, dateTime, keyWord, order);
        }
         

         
        /// <summary>
        /// 查询楼盘快递
        /// </summary>
        /// <param name="loupanId">楼盘Id</param>
        /// <param name="pageIndex">分页页码</param>
        /// <param name="pageSize">分页条数</param>
        /// <param name="count">符合条件的总条数</param>
        /// <returns></returns>
        public virtual List<KuaidiInfo> GetKuaidiListByLoupanId(int loupanId, ref int count, int pageIndex = 1,
                                                     int pageSize = 20)
        {
            List<KuaidiInfo> list = WcmdocumentData.GetKuaidiList(loupanId, pageIndex, pageSize, ref count);
            foreach (KuaidiInfo kuaidiInfo in list)
            {
                kuaidiInfo.NewsFirstImages = GetNewsFirstImagePath(kuaidiInfo.NewsUrl, kuaidiInfo.HtmlContent);
            }
            return list;
        }
        /// <summary>
        /// 获取新闻的第一张图片地址
        /// </summary>
        /// <param name="url">NewsUrl</param>
        /// <param name="dochtml">NewsHtmlContent</param>
        /// <returns></returns>
        public string GetNewsFirstImagePath(string url, string dochtml)
        {
            try
            {
                Regex rURL = new Regex(@"(?is)t20.*?\.htm");
                Regex r = new Regex(@"(?is)[^oldsrc]src=""(?<image>.*?)""");
                string httpPath = rURL.Replace(url, "");
                MatchCollection matchs = r.Matches(dochtml);
                foreach (Match match in matchs)
                {
                    string imgPath = match.Groups["image"].Value.Trim().ToLower();
                    if (imgPath.IndexOf(".jpg", System.StringComparison.Ordinal) > -1 || imgPath.IndexOf(".png", System.StringComparison.Ordinal) > -1
                       || imgPath.IndexOf(".jpeg", System.StringComparison.Ordinal) > -1)
                    {
                        if (imgPath.StartsWith("./"))
                        {
                            imgPath = imgPath.Replace("./", "");
                        }
                        return (httpPath + imgPath);
                    }
                }
                return "";
            }
            catch (Exception)
            {
                return "";
            }
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
            return WcmdocumentData.CheckIssuer(docId, loupanId, issuer);
        }

        /// <summary>
        /// 获取频道信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public virtual DocChannel GetChannelyId(int id)
        {
            return WcmdocumentData.GetChannelyId(id);
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
            return WcmdocumentData.GetRelateNewsList(channelid, docid, keyword);
        }

        /// <summary>
        /// 添加新闻点击数
        /// </summary>
        /// <returns></returns>
        public virtual int AddHisCount(int docid)
        {
            return WcmdocumentData.AddHisCount(docid,1);
        }
        /// <summary>
        /// 添加新闻点击数
        /// </summary>
        /// <param name="docid"></param>
        /// <param name="hits"></param>
        /// <returns></returns>
        public virtual int AddHisCount(int docid, int hits)
        {
            return WcmdocumentData.AddHisCount(docid,hits);
        }

        /// <summary>
        /// 获取新闻图片
        /// </summary>
        /// <param name="docidlist"></param>
        /// <returns></returns>
        public virtual Dictionary<int, string> GetPicDic(List<int> docidlist)
        {
            return WcmdocumentData.GetPicDic(docidlist);
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
            return WcmdocumentData.GetNewsDocIdListByDayTj(channelId, beginDt, endDt);
        }

        /// <summary>
        /// 获取点击数
        /// </summary>
        /// <param name="docidlist"></param>
        /// <returns></returns>
        public virtual Dictionary<int, int> GetHisCounts(List<int> docidlist)
        {
            return WcmdocumentData.GetHisCounts(docidlist);
        }
    }
}
