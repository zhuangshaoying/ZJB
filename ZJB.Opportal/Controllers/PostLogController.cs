using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ZJB.Api.Models;
using ZJB.Api.BLL;
using ZJB.Core.Utilities;
namespace ZJB.Opportal.Controllers
{
    [Authorization]
    public class PostLogController : Controller
    {
        //
        // GET: /PostLog/
        private readonly PostLogBll postLogBll = new PostLogBll();
        public ActionResult Index()
        {
            return View();
        }

        #region 发布日志页面
        public ActionResult GetPostLogView(PostLogListParames parames=null)
        {
            ViewBag.parames = parames;
            return View();
        }
        #endregion

        #region 发布日志列表json数据
        public JsonResult GetPostLogList(PostLogListParames parames)
        {
            int totalSize = 0;
            int houseid = 0;
            int.TryParse(parames.title, out houseid);///支持房源编号 或者标题搜索用到
            parames.houseId = houseid;
            if (parames.houseId > 0)
            {
                parames.title = string.Empty;
            }
            List<PostLogModel> postLogList = postLogBll.GetPostLogList(parames, ref totalSize);
            if (postLogList.IsNoNull()&& postLogList.Count > 0)
            {
                var resultList = postLogList.Select(p => new
                {
                    DateTime = p.DateTime.IsNoNull()?p.DateTime.ToString():"",
                    ShortDateTime = p.DateTime.IsNoNull()?(p.DateTime.ToString("MM-dd HH:mm")):"",
                    ShortBetinTime = p.BeginTime.IsNoNull() ? (p.BeginTime.ToString("MM-dd HH:mm")) : "",
                    ID = p.ID,
                    InfoID = p.InfoID,
                    SiteID = p.SiteID,
                    Status = p.Status,
                    TargetID = p.TargetID,
                    UserID = p.UserID,
                    TargetUrl = p.TargetUrl,
                    IsOrder = p.IsOrder,
                    Msg = p.Msg,
                    SiteUserName = p.SiteUserName,
                    TradeType = p.TradeType,
                    BuildType = p.BuildType,
                    CommunityName = p.CommunityName,
                    BuildArea = p.BuildArea,
                    CurFloor = p.CurFloor,
                    MaxFloor = p.MaxFloor,
                    Price = p.Price,
                    PriceUnit = p.PriceUnit,
                    Title = p.Title,
                    SiteName = p.SiteName,
                    RealyMsg=p.RealyMsg
                });
                return Json(new { data = resultList, totalSize = totalSize }, JsonRequestBehavior.AllowGet);
            }
            return Json(new { data = postLogList, totalSize = totalSize }, JsonRequestBehavior.AllowGet);
        }
        /// <summary>
        /// 发布日志---房源查看
        /// </summary>
        /// <param name="parames"></param>
        /// <returns></returns>
        public JsonResult GetLogByHouses(PostLogListParames parames)
        {
            int totalSize = 0;
            parames.userId =0;
            List<PostLogModel> postLogList = postLogBll.GetPostLogGroupByHouseId(parames, ref totalSize);
            if (postLogList != null && postLogList.Count > 0)
            {
                var resultList = postLogList.Select(p => new
                {
                    InfoID = p.InfoID,
                    TargetUrl = p.TargetUrl,
                    CommunityName = p.CommunityName,
                    BuildArea = p.BuildArea,
                    CurFloor = p.CurFloor,
                    MaxFloor = p.MaxFloor,
                    Price = p.Price,
                    PriceUnit = p.PriceUnit,
                    Title = p.Title,
                    PostCount=p.PostCount
                });
                return Json(new { data = resultList, totalSize = totalSize }, JsonRequestBehavior.AllowGet);
            }
            return Json(new { data = postLogList, totalSize = totalSize }, JsonRequestBehavior.AllowGet);
 
        }
        /// <summary>
        /// 发布日志---网站查看
        /// </summary>
        /// <param name="parames"></param>
        /// <returns></returns>
        public JsonResult GetLogByWebSite()
        {
            int cityId = 592;
            List<string> timeList = new List<string>();
            for (int i = 6; i >= 0; i--)
            {
                DateTime diffTime = DateTime.Now.AddDays(-i);
                timeList.Add(diffTime.ToString("MM-dd"));
            }
            List<PostLogModel> postLogList = postLogBll.GetPostLogGroupByWebSiteAdmin(0, cityId);

            Dictionary<int?, PostLogModel> siteDic = new Dictionary<int?, PostLogModel>();
            if (postLogList != null && postLogList.Count > 0)
            {
                foreach (PostLogModel item in postLogList)
                {
                    if (siteDic.ContainsKey(item.SiteID))
                    {
                        siteDic[item.SiteID].TimeAllCount += item.PostCount;
                        if (item.DateTime != null && item.DateTime.ToString() != "0001/1/1 0:00:00")
                        {
                            siteDic[item.SiteID].SiteTimeLogList.Add(new siteTimeLog()
                            {
                                count = item.PostCount,
                                time = item.DateTime
                            });
                        }
                    }
                    else
                    {
                        PostLogModel logModel = new PostLogModel();
                        logModel.SiteID = item.SiteID;
                        logModel.SiteName = item.SiteName;
                        logModel.Logo = item.Logo;
                        logModel.SiteTimeLogList = new List<siteTimeLog>(); ;
                        logModel.TimeAllCount = item.PostCount;
                        if (item.DateTime != null && item.DateTime.ToString() != "0001/1/1 0:00:00")
                        {
                            logModel.SiteTimeLogList.Add(new siteTimeLog()
                            {
                                count = item.PostCount,
                                time = item.DateTime
                            });
                        }
                        siteDic.Add(item.SiteID, logModel);
                    }
                }
                List<PostLogModel> resultList = new List<PostLogModel>();
                foreach (PostLogModel item in siteDic.Values)
                {
                    resultList.Add(item);
                }
                if (postLogList != null && postLogList.Count > 0)
                {
                    var varResultList = resultList.Select(p => new
                    {
                        p.SiteID,
                        p.SiteName,
                        p.Logo,
                        p.TimeAllCount,
                        SiteTimeLogList = p.SiteTimeLogList.Select(t => new
                        {
                            time = t.time.ToString("MM-dd"),
                            count = t.count
                        })
                    });
                    return Json(new { data = varResultList, timeList = timeList, totalSize = 0 }, JsonRequestBehavior.AllowGet);
                }
            }
            return Json(new { data = new List<PostLogModel>(), timeList = timeList,totalSize = 0 }, JsonRequestBehavior.AllowGet);
        }
        #endregion
    }
}
