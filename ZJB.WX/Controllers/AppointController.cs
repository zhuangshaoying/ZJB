using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ZJB.Api.BLL;
using ZJB.Api.Models;
using ZJB.Api.Entity;
namespace ZJB.WX.Controllers
{
    [Authorization]
    public class AppointController : Controller
    {
        //
        // GET: /AppointManage/
        private readonly PostManageBll postManageBll = new PostManageBll();
        private NCBaseRule ncBase = new NCBaseRule();
        public ActionResult Index()
        {
            return View();
        }

        #region 预约日志页面
        public ActionResult AppointLogView(int tradeType = 1)
        {
            ViewBag.tradeType = tradeType;
            return View();
        }
        #endregion
        #region 预约日志列表json数据
        public JsonResult GetAppointList(AppointLogListReq parame)
        {
            int totalSize = 0;
            parame.userId = this.GetLoginUser().UserID;
            List<PostManageModel> appointList = new List<PostManageModel>();
            appointList = postManageBll.GetAppointLogList(parame,ref totalSize);
            List<SiteManage> siteDic = ncBase.CurrentEntities.SiteManage.ToList();
            if (appointList != null && appointList.Count > 0)
            {
                var resultList = appointList.Select(p => new
                {
                    p.Title,
                    p.HouseID,
                    AddTime=p.AddTime.ToString(),
                    p.OrderStatus,
                    OrderSites = siteDic!=null?OrderSitesList(siteDic, p.OrderSites):null,
                    OrderTime = OrderTimesList(p.OrderTime)
                });
                return Json(new { data = resultList ,totalSize=totalSize},JsonRequestBehavior.AllowGet);
            }
            else
                return Json(new { data = new List<PostManageModel>(), totalSize = totalSize }, JsonRequestBehavior.AllowGet);
            
        }
        private List<string> OrderTimesList(string times)
        {
            List<string> timesList = times.Split(',').ToList();
            List<string> resultList = new List<string>();
            if (timesList != null && timesList.Count > 0)
            {
                foreach (string item in timesList)
                {
                    long longtime=0;
                    long.TryParse(item,out longtime);
                    resultList.Add(ZJB.Core.Utilities.DateTimeUtility.FromUnixTime(longtime/1000).ToString("yyyy-MM-dd HH:mm"));
                }
            }
            return resultList;
        }
        private List<string> OrderSitesList(List<SiteManage> siteList, string sites)
        {
            List<string> sitesList = sites.Split(',').ToList();
            List<string> resultList = new List<string>();
            if (sitesList != null && sitesList.Count > 0)
            {
                foreach (string item in sitesList)
                {
                    int siteId = 0;
                    int.TryParse(item,out siteId);
                    if (siteId > 0 && siteList.Where(s => s.SiteID == siteId).FirstOrDefault()!=null)
                    {
                        resultList.Add(siteList.Where(s=>s.SiteID==siteId).FirstOrDefault().SiteName);
                    }
                }
            }
            return resultList;
        }
        #endregion

        #region 删除预约
        [HttpPost]
        public JsonResult DeleteAppoint()
        {
            string HouseIds = "";
            if (Request.Form["HouseIds"] != null && Request.Form["HouseIds"].ToString() != "")
            {
                HouseIds = Request.Form["HouseIds"].ToString();
            }
            int rows = postManageBll.DeleteAppoint(this.GetLoginUser().UserID, HouseIds);
            return Json(new { status = rows });
        }
        #endregion
    }
}
