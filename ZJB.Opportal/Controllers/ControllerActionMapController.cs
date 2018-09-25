using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ZJB.Api.BLL;
using ZJB.Api.Entity;
using ZJB.Core.Utilities;
using ZJB.Api.Models;
using ZJB.Opportal.Models;
namespace ZJB.Opportal.Controllers
{
    public class ControllerActionMapController : Controller
    {
        //
        // GET: /ControllerActionMap/
        private NCBaseRule ncBase = new NCBaseRule();
        private UserBll userBll = new UserBll();
        private ActionLogBll actionLogBll = new ActionLogBll();
        /// <summary>
        /// ActionLog 首页
        /// </summary>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="keyWord"></param>
        /// <returns></returns>
        public ActionResult Index(int pageIndex = 1, int pageSize = 20, string keyWord = "",int userId=0)
        {
            int totalSize = 0;
            string userName = string.Empty;
            ActionLogTopStatReq parame=new ActionLogTopStatReq(){
             keyword=keyWord,
             pi=pageIndex,
             ps=pageSize,
             userId=userId
            };
            if (userId > 0)
            {
                PublicUserModel user = userBll.GetUserById(userId);
                if (user.IsNoNull())
                {
                    userName = user.Name;
                }
            }
            List<ControllerActionMapModel> stateList= actionLogBll.ActionLogTopStat(parame, ref totalSize);
            ViewBag.Count = totalSize;
            ViewBag.PageIndex = pageIndex;
            ViewBag.PageSize = pageSize;
            ViewBag.KeyWord = keyWord;
            ViewBag.UserId = userId;
            ViewBag.UserName = userName;
            ViewBag.PageTotal = (int)Math.Ceiling((double)ViewBag.Count / pageSize);
            return View(stateList);
        }
        /// <summary>
        /// 添加或者修改功能描述 
        /// </summary>
        /// <param name="newItem"></param>
        /// <returns></returns>
        public JsonResult EditControlActionMap(ControlActionMap newItem)
        {
            ControlActionMap itemMap = ncBase.CurrentEntities.ControlActionMap.Where(m => m.Controller == newItem.Controller && m.Action == newItem.Action).FirstOrDefault();
            if (itemMap.IsNoNull())
            {
                itemMap.Status =1;
                itemMap.FunctionName = newItem.FunctionName;
                ncBase.CurrentEntities.SaveChanges();
            }
            else {
                newItem.Status = 1;
                ncBase.CurrentEntities.ControlActionMap.AddObject(newItem);
                ncBase.CurrentEntities.SaveChanges();
            }
            return Json(new { status=1});
        }
        /// <summary>
        /// 改变功能描述的状态（是否重点功能）
        /// </summary>
        /// <param name="newItem"></param>
        /// <returns></returns>
        public JsonResult EditControlActionMapStatus(ControlActionMap newItem)
        {
            ControlActionMap itemMap = ncBase.CurrentEntities.ControlActionMap.Where(m => m.Controller == newItem.Controller && m.Action == newItem.Action).FirstOrDefault();
            if (itemMap.IsNoNull())
            {
                itemMap.Status = newItem.Status;
                ncBase.CurrentEntities.SaveChanges();
                return Json(new { status = 1 });
            }
            return Json(new { status = 0 });
        }
        #region 通过controller和anction 获取每天访问次数
        /// <summary>
        /// 通过controller和anction 获取每天访问次数
        /// </summary>
        /// <returns></returns>
        public ActionResult ActionLogStatEveryDayByFunction(string _controller,string _action,int userId=0)
        {
            string functionName = string.Empty;

            ControlActionMap item = ncBase.CurrentEntities.ControlActionMap.Where(m => m.Controller == _controller && m.Action == _action).FirstOrDefault();
            if (item.IsNoNull())
            {
                functionName = item.FunctionName;
            }
            if (string.IsNullOrEmpty(functionName)) {
                functionName = _controller + "/" + _action;
            }
            string userName = string.Empty;
            if (userId > 0)
            {
                PublicUserModel user = userBll.GetUserById(userId);
                if (user.IsNoNull())
                {
                    userName = user.Name;
                }
            }
            ViewBag.FunctionName = functionName;
            ViewBag.Controller = _controller;
            ViewBag.Action = _action;
            ViewBag.UserId = userId;
            ViewBag.UserName = userName;
            return View();
           
        }
        public JsonResult GetActionLogStatEveryDayByFunction(StatReq parame)
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
            List<StatModel> everyDayFunctionStat = actionLogBll.GetActionLog_StatByFunction(parame);
            EveryDayStatViewModel everyDayStat = new EveryDayStatViewModel();
            everyDayStat.FunctionStat = (from itemTime in timeList
                        join itemstat in everyDayFunctionStat on itemTime.ToShortDateString() equals itemstat.Time.ToShortDateString() into temp
                        from viewItem in temp.DefaultIfEmpty()
                        select new
                        {
                            ShortDate = itemTime,
                            Time = itemTime.ToString("MM-dd"),
                            Count = viewItem == null ? 0 : viewItem.Count
                        }).OrderBy(o => o.ShortDate);
            everyDayStat.timeList = timeList.Select(t => new { Time = t.ToString("MM-dd") });
            return Json(everyDayStat, JsonRequestBehavior.AllowGet);
        }
        #endregion
    }
}
