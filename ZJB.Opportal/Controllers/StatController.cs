using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ZJB.Api.Models;
using ZJB.Api.BLL;
using ZJB.Opportal.Models;
using ZJB.Api.Entity;
namespace ZJB.Opportal.Controllers
{
    [Authorization]
    public class StatController : Controller
    {
        //
        // GET: /Stat/
        private UserBll userBll = new UserBll();
        private HouseBll houseBll = new HouseBll();
        private PostLogBll postLogBll = new PostLogBll();
        private ActionLogBll actionLogBll = new ActionLogBll();
        private System.Web.Script.Serialization.JavaScriptSerializer jss = new System.Web.Script.Serialization.JavaScriptSerializer();
        public ActionResult Index()
        {
            return View();
        }
        #region 每日统计
        /// <summary>
        /// 每日统计
        /// </summary>
        /// <returns></returns>
        public ActionResult EveryDayStat()
        {
            
            return View();
        }

        public JsonResult GetEveryDayStat(StatReq parame)
        {
            parame.currentTime = parame.currentTime.ToString() == "0001/1/1 0:00:00" ? DateTime.Now : parame.currentTime;
           
            parame.ps = parame.ps == 0 ? 10 : parame.ps;
            #region 最近日期
            string DateTimeList = string.Empty;
            if (parame.currentTime.CompareTo(DateTime.Now) > 0)
            {
                parame.currentTime = DateTime.Now;
            }
            List<DateTime> timeList = new List<DateTime>();
            for (int i = 0; i < parame.ps; i++)
            {
                timeList.Add(parame.currentTime.AddDays(i - parame.ps + 1));
            }
            #endregion
            List<StatModel> everyDayLoginStat = userBll.GetLoginStat(parame);
            List<StatModel> everyDayHouseAddStat = houseBll.GetHouseAddStat(parame);
            List<StatModel> everyDayPushHouseStat = postLogBll.GetPostStat(parame);
            EveryDayStatViewModel everyDayStat = new EveryDayStatViewModel();
            everyDayStat.UserLoginStat = (from itemTime in timeList
                                           join itemstat in everyDayLoginStat on itemTime.ToShortDateString() equals itemstat.Time.ToShortDateString() into temp
                                          from viewItem in temp.DefaultIfEmpty()
                                          select new
                                          {
                                              ShortDate = itemTime,
                                              Time = itemTime.ToString("MM-dd"),
                                              Count = viewItem == null ? 0 : viewItem.Count
                                          }).OrderBy(o => o.ShortDate);


            everyDayStat.HouseAddStat = (from itemTime in timeList
                                         join itemstat in everyDayHouseAddStat on itemTime.ToShortDateString() equals itemstat.Time.ToShortDateString() into temp
                                          from viewItem in temp.DefaultIfEmpty()
                                          select new
                                          {
                                              ShortDate = itemTime,
                                              Time = itemTime.ToString("MM-dd"),
                                              Count = viewItem == null ? 0 : viewItem.Count
                                          }).OrderBy(o => o.ShortDate);

            everyDayStat.PushHouseStat = (from itemTime in timeList
                                          join itemstat in everyDayPushHouseStat on itemTime.ToShortDateString() equals itemstat.Time.ToShortDateString() into temp
                                          from viewItem in temp.DefaultIfEmpty()
                                          select new
                                          {
                                              ShortDate = itemTime,
                                              Time = itemTime.ToString("MM-dd"),
                                              Count = viewItem == null ? 0 : viewItem.Count,
                                              SuccessCount = viewItem == null ? 0 : viewItem.SuccessCount,
                                              FailCount = viewItem == null ? 0 : viewItem.FailCount
                                          }).OrderBy(o => o.ShortDate);


            everyDayStat.timeList = timeList.Select(t=>new{Time=t.ToString("MM-dd")});
            return Json(everyDayStat,JsonRequestBehavior.AllowGet);
        }
        #endregion
        #region 站点分析 --总的发布分布 成功分布 失败分布
        public ActionResult SiteAnalyse()
        {
            return View();
        }
        public JsonResult GetSiteAnalyseData(StatReq parame)
        {
            List<StatModel> statList = postLogBll.GetSiteAnalyseData(parame);
            if(statList!=null&&statList.Count>0)
            {
                var resultList = statList.Select(s => new
                {
                    s.SiteName,
                    s.Count,
                    s.TipContent
                });
                return Json(new { data = resultList }, JsonRequestBehavior.AllowGet);
            }
            
            return Json(new { data = new List<StatModel>() }, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region 访问Ip排行列表
        public ActionResult IpAccessList(int ps = 20, int pi = 1,int bh=0,int eh=24, string keyWord="")
        {
            int totalSize = 0;
            List<StatModel> IpList = new List<StatModel>();
            IpStatListReq parame = new IpStatListReq()
            {
                 beginHour=bh,
                 endHour=eh,
                 keyword=keyWord,
                 ps = ps,
                 pi = pi
            };
            IpList = actionLogBll.GetIpStatList(parame, ref totalSize);
            ViewBag.Count = totalSize;
            ViewBag.PageIndex = pi;
            ViewBag.PageSize = ps;
            ViewBag.KeyWord = keyWord;
            ViewBag.BeginHour = bh;
            ViewBag.EndHour = eh;
            ViewBag.PageTotal = (int)Math.Ceiling((double)ViewBag.Count / ps);
            return View(IpList);
        }
        #endregion
        #region 用户访问统计
        public ActionResult UserAccessList(int ps = 100, int pi = 1, int bh = 0, int eh = 24, string IpAddress = "")
        {
            int totalSize = 0;
            List<PublicUserModel> userList = new List<PublicUserModel>();
            UserAccessListReq parame = new UserAccessListReq()
            {
                beginHour = bh,
                endHour = eh,
                IpAddress = IpAddress,
                ps = ps,
                pi = pi
            };
            userList = userBll.GetUserAccessStatList(parame, ref totalSize);
            ViewBag.Count = totalSize;
            ViewBag.PageIndex = pi;
            ViewBag.PageSize = ps;
            ViewBag.IpAddress = IpAddress;
            ViewBag.BeginHour = bh;
            ViewBag.EndHour = eh;
            ViewBag.PageTotal = (int)Math.Ceiling((double)ViewBag.Count / ps);
            return View(userList);
        }
        #endregion

        #region 站点失败类型统计

        #endregion
    }
}
