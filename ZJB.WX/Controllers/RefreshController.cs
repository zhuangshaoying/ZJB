using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ZJB.Api.BLL;
using ZJB.Api.Models;
using ZJB.Api.Entity;
using ZJB.Core.Utilities;
using ZJB.Api.BLL;
namespace ZJB.WX.Controllers
{
    [Authorization]
    public class RefreshController : Controller
    {
        //
        // GET: /Refresh/
        private NCBaseRule ncBase = new NCBaseRule();
        private RefreshBll refreshBll = new RefreshBll();
        public ActionResult Index()
        {
            return View();
        }
        public JsonResult GetRefreshWeb()
        {
            PublicUserModel loginUser=this.GetLoginUser();
            var result = refreshBll.GetUserRefreshWeb(loginUser.UserID).Select(s=>new { 
             s.SiteID,
             s.Logo,
             s.State,
             SiteUserName = string.IsNullOrEmpty(s.SiteUserName) ? "缺少账号" : s.SiteUserName
            });
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public JsonResult RefreshPlanState(int siteId=0)
        {
            PublicUserModel loginUser=this.GetLoginUser();
            RefreshPlan planItem = new RefreshPlan()
            {
                UserId=loginUser.UserID,
                SiteId=siteId
            };
            int planId = refreshBll.RefreshPlanState(planItem);
            string msg =string.Empty;
            switch (planId)
            {
                case -1:
                    msg = "不支持云刷新";
                    break;
                case -2:
                    msg = "没有绑定账号";
                    break;
                default:
                    msg = "操作成功";
                    break;
            }
            return Json(new { status = planId, msg = msg });
        }
        #region 刷新日志列表
        public JsonResult RefreshLogList(RefreshLogListReq parame)
        {
          //  SiteManage yunRefreshSite = ncBase.CurrentEntities.SiteManage.Where(s => s.YunRefresh == true && s.Status == 1).FirstOrDefault();
          //  if (yunRefreshSite.IsNoNull())
          //  {
                PublicUserModel loginUser = this.GetLoginUser();
                parame.UserId = loginUser.UserID;
                int totalSize = 0;
                List<RefreshLogModel> refreshLogList = refreshBll.RefreshLogList(parame, ref totalSize);
                if (refreshLogList != null && refreshLogList.Count > 0)
                {
                    var resultList = refreshLogList.Select(l => new
                    {
                        l.BuildType,
                        DateTime = ((DateTime)l.DateTime).ToString("MM-dd HH:mm"),
                        l.Houses,
                        l.Id,
                        Msg = l.Status == -99 ? l.Msg : l.ClientMsg,
                        l.PlanId,
                        l.PlanNo,
                        l.RefreshMode,
                        l.SiteId,
                        l.SiteName,
                        l.SiteUserName,
                        l.Status,
                        l.UserID,
                        l.RefreshNum
                    });
                    return Json(new { data = resultList, totalSize = totalSize, openRefresh = 1 },JsonRequestBehavior.AllowGet);
                }
                return Json(new { data = refreshLogList, totalSize = totalSize, openRefresh = 1 }, JsonRequestBehavior.AllowGet);
           // }
           // return Json(new { openRefresh = 0 }, JsonRequestBehavior.AllowGet);//openRefresh=0 未有站点开放云刷新
           
        }
        #endregion
        #region 获取可云刷新的站点
        public JsonResult GetEnableRefreshSite()
        {
            List<SiteManage> sitelist = ncBase.CurrentEntities.SiteManage.Where(s => s.Status == 1 && s.YunRefresh == true).ToList();
            if (sitelist.IsNoNull())
            {
                var resultList = sitelist.Select(s=>new
                {
                    s.SiteID,
                    s.SiteName
                });
                return Json(resultList,JsonRequestBehavior.AllowGet);
            }
            return Json(new List<SiteManage>(), JsonRequestBehavior.AllowGet);
        }
        #endregion
        #region  刷新设置 页面
        public ActionResult Refresh_SetView(int SiteId)
        {
            ViewBag.SiteId = SiteId;
            return View();
        }
        #endregion

        #region 刷新设置
        [HttpPost]
        public JsonResult RefreshSet(RefreshPlan item)
        {
            PublicUserModel loginUser = this.GetLoginUser();
            item.UserId = loginUser.UserID;
            int intervalTime = (int)item.IntervalTime;
            int beginHour=0,beginMinute=0,endHour=0,endMinute=0;
            int.TryParse(item.BeginHour.ToString(), out beginHour);
            int.TryParse(item.BeginMinute.ToString(), out beginMinute);
            int.TryParse(item.EndHour.ToString(), out endHour);
            int.TryParse(item.EndMinute.ToString(), out endMinute);
            TimeSpan beginTp = new TimeSpan(beginHour, beginMinute, 0);
            TimeSpan endTp = new TimeSpan(endHour, endMinute, 0);
            TimeSpan diffTime = endTp - beginTp;
            List<RefreshDetail> detailList = new List<RefreshDetail>();
            if (diffTime.TotalMinutes <= 0)
            {
                return Json(new { status = -1, msg = "结束时间必须大于开始时间" });
            }
            if (intervalTime < 1)
            {
                return Json(new { status = -1, msg = "刷新间隔必须大于1分钟" });
            }
            if (diffTime.TotalMinutes < intervalTime)
            {
                return Json(new { status = -1, msg = "设置的开始和结束间隔时间不能小于刷新间隔时间" });
            }
            if (item.CountPerTime <= 0)
            {
                return Json(new { status = -1, msg = "刷新次数必须大于0" });
            }
            TimeSpan detailTime = beginTp;
            for (int i = 0; i <= diffTime.TotalMinutes / intervalTime; i++)
            {
                detailList.Add(new RefreshDetail()
                {
                    BuildType = item.BuildType,
                    CountPerTime = item.CountPerTime,
                    Hour = detailTime.Hours,
                    Minute = detailTime.Minutes,
                    TradeType = item.TradeType
                });
                detailTime = detailTime.Add(new TimeSpan(0, intervalTime, 0));
            }
            int result = refreshBll.RefreshDetailAdd(item, detailList);
            string msg = string.Empty;
            if (result > 0)
            {
                msg = "添加成功";
            }
            if (result == -1)
            {
                msg = "计划已达上限，请先删除已有计划";
            }
            return Json(new { status = result, msg = msg});        
         
      
            
        }
        #endregion
        #region 刷新计划删除
        [HttpPost]
        public JsonResult RefreshSetDelete(string planIds)
        {
            PublicUserModel loginUser = this.GetLoginUser();
            int result = refreshBll.RefreshSetDelete(loginUser.UserID, planIds);
            return Json(new { status=result});
        }
        #endregion
        #region 刷新设置列表
        [HttpGet]
        public JsonResult GetRefreshSetList(int siteId)
        { 
            PublicUserModel loginUser=this.GetLoginUser();
            List<RefreshPlan> refreshList = ncBase.CurrentEntities.RefreshPlan.Where(u => u.UserId == loginUser.UserID && u.SiteId == siteId).ToList();
            if (refreshList.IsNoNull())
            {
                var resultList = refreshList.Select(p => new
                {
                    p.SiteId,
                    p.PlanId,
                    p.BeginHour,
                    p.BeginMinute,
                    p.EndHour,
                    p.EndMinute,
                    p.TradeType,
                    p.BuildType,
                    p.IntervalTime,
                    p.CountPerTime
                });

                return Json(new { data = resultList, totalSize = resultList.Count() },JsonRequestBehavior.AllowGet);
            }
            return Json(new { data = refreshList, totalSize = 0}, JsonRequestBehavior.AllowGet);
        }
        #endregion 
         
    }
}
