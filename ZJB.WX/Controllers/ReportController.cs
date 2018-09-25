using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Builders;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ZJB.Api.BLL;
using ZJB.Api.Common;
using ZJB.Api.Models;
using ZJB.Core.Injection;
using ZJB.Core.Utilities;


namespace ZJB.WX.Controllers
{
    /// <summary>
    /// 消息报告
    /// </summary>
    public class ReportController : BaseController
    {
        //
        // GET: /Community/

        private NCBaseRule ncRule = Container.Instance.Resolve<NCBaseRule>();
        private HouseBll houseBll = new HouseBll();
        private WeiXinPublic wxPublic = new WeiXinPublic();
        public ActionResult ReportNewsByLogIdTest(string message)
        {
            List<string> openids = ConfigUtility.GetValue("Report.OpenIdListForNews").Split(";".ToArray(), StringSplitOptions.RemoveEmptyEntries).ToList();
            if (openids.Count == 0)
            {
                openids.Add("okW7twhyUaxwcPnKt93urudB40OI");
                openids.Add("okW7twgxMor7gtphIQuJnS396KfI");
            }
            wxPublic.WxPushMessage(openids.LastOrDefault(), "您好，您有一条新的新闻站点采集消息。"
                                    , keyword1: "0", keyword2: ""
                                    , keyword3: DateTime.Now.ToString("yyyy-MM-dd HH:mm"), keyword4: "", url: "http://lpsjadmin.ZJB.com/"
                                    , temp: "o0lKqQvcBXTDDr6K3NZZRi0jd6dXTPhGmv_KTBCUW9A", remark: message??"其它"
                                    );
            return Json(new { ok = 1 }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult ReportNewsByLogId(string logId)
        {

     
            List<string> openids = ConfigUtility.GetValue("Report.OpenIdListForNews").Split(";".ToArray(),StringSplitOptions.RemoveEmptyEntries).ToList();
            if (openids.Count == 0)
            {
                openids.Add("okW7twhyUaxwcPnKt93urudB40OI");
                openids.Add("okW7twgxMor7gtphIQuJnS396KfI");
            }
            var client = new MongoClient(ConfigUtility.GetValue("Report.NewsMongoDb") ??"mongodb://192.168.0.173");
            MongoDatabase NewsTempDatabase = client.GetServer().GetDatabase("Temp");
            MongoCollection<BsonDocument> tempCollection = NewsTempDatabase.GetCollection<BsonDocument>("NewsCrawler");
            MongoCollection<BsonDocument> logCollection = NewsTempDatabase.GetCollection("NewsCrawlerLog");
            var log = logCollection.FindOne(Query.EQ("_id",new ObjectId( logId)));
            //var log = new BsonDocument
            //    {
            //        {"_id", logId},
            //        {"siteUrls",jobRule.siteUrl},
            //        {"host",IP + " ## " + hostName},
            //        {"jobRule", JsonConvert.SerializeObject(jobRule)},
            //        {"startTime", start_time},
            //        {"endTime", end_time},
            //        {"usedTime", usedTime},
            //        {"newsUrls", JsonConvert.SerializeObject(urls)},
            //        {"urlSum", urls.Count},
            //        {"newsCrawled", newsCrawled}
            //    };
            if (log != null)
            {
                string sitrUrls = log["siteUrls"].ToString();
                string lastNewsTitle = log["lastNewsTitle"].ToString();
                string newsCrawled = log["newsCrawled"].ToString();
                var newsCrawler = tempCollection.FindOne(Query.EQ("_id", sitrUrls));
                string siteName = newsCrawler["siteName"].ToString();
                string cityId = newsCrawler["cityId"].ToString();
                lastNewsTitle = lastNewsTitle ?? "速去查看详情";
                if (cityId == "4")//厦门
                {
                    foreach (var openId in openids)
                    {
                        try
                        {
                            wxPublic.WxPushMessage(openId, "您好，您有一条新的新闻站点采集消息。"
                                    , keyword1: newsCrawled, keyword2: siteName
                                    , keyword3: DateTime.Now.ToString("yyyy-MM-dd HH:mm"),keyword4: "", url: "http://lpsjadmin.ZJB.com/LouBaNews/NewsIndex?newslogid="+logId
                                    , temp: "o0lKqQvcBXTDDr6K3NZZRi0jd6dXTPhGmv_KTBCUW9A", remark: lastNewsTitle
                                    );
                        }
                        catch (Exception)
                        {
                            //WX.
                        }
                    }
                }
            }
            return Json(new {ok=1 },JsonRequestBehavior.AllowGet);
        }
        public ActionResult ReportNewsByResult([System.Web.Http.FromBody] string content)
        {

            JObject datas = null;
            if (!string.IsNullOrEmpty(content))
            {
                datas = JObject.Parse(content);
            }
            if (datas != null)
            {
                string cityId = datas["cityId"].ToString();
                if (cityId == "4")//厦门
                {
                    List<string> openids = ConfigUtility.GetValue("Report.OpenIdListForNews").Split(";".ToArray(), StringSplitOptions.RemoveEmptyEntries).ToList();
                    if (openids.Count == 0)
                    {
                        openids.Add("okW7twhyUaxwcPnKt93urudB40OI");
                        openids.Add("okW7twgxMor7gtphIQuJnS396KfI");
                    }
                    string lastNewsTitle = datas["lastNewsTitle"].ToString();
                    string lastNewsUrl = datas["lastNewsUrl"].ToString();
                    string newsCrawled = datas["newsCrawled"].ToString();
                    string siteName = datas["siteName"].ToString();

                    lastNewsTitle = lastNewsTitle ?? "速去查看详情";
                    foreach (var openId in openids)
                    {
                        try
                        {
                            wxPublic.WxPushMessage(openId, "您好，您有一条新的新闻站点采集消息。"
                                    , keyword1: newsCrawled, keyword2: siteName
                                    , keyword3: DateTime.Now.ToString("yyyy-MM-dd HH:mm"), keyword4: "", url: "http://lpsjadmin.ZJB.com/LouBaNews/NewsIndex"
                                    , temp: "o0lKqQvcBXTDDr6K3NZZRi0jd6dXTPhGmv_KTBCUW9A", remark: lastNewsTitle
                                    );
                        }
                        catch (Exception)
                        {
                            //WX.
                        }
                    }
                }
            }
            return Json(new { ok = 1 }, JsonRequestBehavior.AllowGet);
        }


        #region news配置
        private static readonly string NewsConnectString = ZJB.Core.Utilities.ConfigUtility.GetValue("News.ConnStr");
        private static readonly string NewsDatabase = ZJB.Core.Utilities.ConfigUtility.GetValue("News.Database");
        private static readonly string NewsTableName = ZJB.Core.Utilities.ConfigUtility.GetValue("News.TableName");
        private static readonly string NewsToUsers = ZJB.Core.Utilities.ConfigUtility.GetValue("News.ToUsers");
        private static MongoCollection<BsonDocument> NewsCollection = null;
        #endregion

        #region CommunityPrice配置
        private static readonly string CommunityPriceConnectString = ZJB.Core.Utilities.ConfigUtility.GetValue("CommunityPrice.ConnStr");
        private static readonly string CommunityPriceDatabase = ZJB.Core.Utilities.ConfigUtility.GetValue("CommunityPrice.Database");
        private static readonly string CommunityPriceTableName = ZJB.Core.Utilities.ConfigUtility.GetValue("CommunityPrice.TableName");
        private static readonly string CommunityPriceToUsers = ZJB.Core.Utilities.ConfigUtility.GetValue("CommunityPrice.ToUsers");
        private static MongoCollection<BsonDocument> CommunityPriceCollection = null;
        private static Dictionary<int, List<string>> users = new Dictionary<int, List<string>>();
        #endregion
        #region 通用方法
        private static MongoCollection<BsonDocument> GetCollection(string connectString, string database, string tableName)
        {
            MongoClient client = new MongoClient(connectString);
            var db = client.GetServer().GetDatabase(database);
            var _collection = db.GetCollection<BsonDocument>(tableName);
            return _collection;
        }
        /// <summary>
        /// 发送消息
        /// </summary>
        /// <param name="type">1.新闻，2.小区房价，3.小区动态，4.小区</param>
        /// <param name="msg"></param>
        /// <param name="url"></param>
        private void SendMsg(int type, string msg, string url,string otherInfo="")
        {
            var userLst = users.ContainsKey(type)? users[type]:new List<string>();
            List<string> openIds = userLst.ToList();
            switch (type)
            {
                default:
                    break;
            }
            foreach (var openId in openIds)
            {
                wxPublic.WxPushMessage(openId, msg
                                    , keyword1: "", keyword2: ""
                                    , keyword3: DateTime.Now.ToString("yyyy-MM-dd HH:mm"), keyword4: "", url: url
                                    , temp: "o0lKqQvcBXTDDr6K3NZZRi0jd6dXTPhGmv_KTBCUW9A", remark: otherInfo
                                    );

            }
        }
        static ReportController()
        {
            users = new Dictionary<int, List<string>>();
            users.Add(1, (NewsToUsers??"").Split(';').ToList());
            users.Add(2, (CommunityPriceToUsers ?? "").Split(';').ToList());
            if (!string.IsNullOrEmpty(NewsConnectString))
            {
                NewsCollection = GetCollection(NewsConnectString, NewsDatabase, NewsTableName);
            }
            if (!string.IsNullOrEmpty(CommunityPriceConnectString))
            {
                CommunityPriceCollection = GetCollection(CommunityPriceConnectString, CommunityPriceDatabase, CommunityPriceTableName);
            }
        }
        #endregion
        public ActionResult Index()
        {
            return View();
        }
        #region news
        [HttpPost]
        public ActionResult News(JObject results)
        {
            string key = results.First.Value<string>();
            switch (key)
            {
                case "news_xm":
                    {
                        var items = (JArray)(results[key]);
                        foreach (JObject item in items)
                        {
                            var oldItem = GetResultForNews(item["地址"].ToString());
                            if (oldItem == null)
                            {
                                InsertDataForNews(key, oldItem);

                                string msg = string.Format(
                                    "采集到一条来自{0}的消息，标题:{1}", key, item["标题"], item["来源"]
                                    );
                                string url = string.Format("http://wx.linlishe.cn/report/shownews?url={0}", System.Web.HttpUtility.UrlEncode(item["地址"].ToString()));
                                SendMsg(1, msg, url);
                            }
                        }
                    }
                    break;
                case "results":
                default:
                    break;
            }
            var json = new { Success = true, Message = string.Empty };
            return Json(json);
        }

        /// <summary>
        /// 显示新闻
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public ActionResult ShowNews(string url)
        {
            var data = GetResultForNews(url);
            return View();
        }
        private JObject GetResultForNews(string url)
        {
            JObject result = null;
            var query = Query.EQ("地址", url);
            result = NewsCollection.FindOneAs<JObject>(query);
            return result;
        }

        private void InsertDataForNews(string site, JObject source)
        {
            NewsCollection.Insert(source);
        }
        #endregion

        #region communityPrice
        [HttpPost]
        public ActionResult CommunityPrice(JObject results)
        {
            string site = results.First.Value<string>();
            switch (site)
            {
                case "58":
                case "ajk":
                case "fang":
                    {
                        var items = (JArray)(results[site]);
                        foreach (JObject item in items)
                        {
                            var oldItem = GetResultForCommunityPrice(site, item["小区名称"].ToString());
                            if (oldItem != null)
                            {
                                if (oldItem["价格"].ToString() == item["价格"].ToString())
                                {
                                    oldItem = null;
                                }
                            }
                            if (oldItem == null)
                            {
                                InsertDataForCommunityPrice(site, oldItem);
                                string msg = string.Format(
                                    "[{1}]小区价格发生变化,最新价:{2}", site, item["小区名称"], item["价格"]
                                    );
                                string url = string.Format("http://wx.linlishe.cn/report/showCommunityPrice?site={0}&communityName={1}", System.Web.HttpUtility.UrlEncode(site), System.Web.HttpUtility.UrlEncode(item["小区"].ToString()));
                                SendMsg(1, msg, url);
                            }
                        }
                    }
                    break;
                case "results":
                default:
                    break;
            }
            var json = new { Success = true, Message = string.Empty };
            return Json(json);
        }
        /// <summary>
        /// 显示小区价格
        /// </summary>
        /// <param name="site"></param>
        /// <param name="communityName"></param>
        /// <returns></returns>
        public ActionResult ShowCommunityPrice(string site, string communityName)
        {
            var data = GetResultForCommunityPrice(site, communityName);
            return View(data);
        }
        private JObject GetResultForCommunityPrice(string site, string communityName)
        {
            JObject result = null;
            var query = Query.And(Query.EQ("小区名称", communityName), Query.EQ("网站", site));
            result = CommunityPriceCollection.FindOneAs<JObject>(query);

            return result;
        }


        private void InsertDataForCommunityPrice(string communityName, JObject source)
        {
            CommunityPriceCollection.Insert(source);
        }
        #endregion

    }
}
