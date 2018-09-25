using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Mvc;
using ZJB.Api.BLL;
using ZJB.Api.Entity;
using ZJB.Core.Utilities;

namespace ZJB.Opportal.Controllers
{
    [Authorization]
    public class ApisController : Controller
    {
        private NCBaseRule ncBase = new NCBaseRule();
        /// <summary>
        /// 接口列表
        /// </summary>
        /// <param name="type"></param>
        /// <param name="pi"></param>
        /// <param name="ps"></param>
        /// <returns></returns>
        public ActionResult ApiList(int type = 0, int pi = 1, int ps = 20)
        {
            int totalSize = 0;
            List<ApiWord> apiWords = new List<ApiWord>();

            apiWords = ncBase.CurrentEntities.ApiWord
                .Where(n =>type==0 || n.Type == type)
                .OrderByDescending(n => n.CreateTime)
                .Skip((pi - 1) * ps)
                .Take(ps).ToList();

            totalSize = ncBase.CurrentEntities.ApiWord
                .Where(n => type == 0 || n.Type == type).ToList().Count;

            ViewBag.TotalSize = totalSize;
            ViewBag.pi = pi;
            ViewBag.ps = ps;
            ViewBag.type = type;
            if (apiWords != null)
                return View(apiWords);
            return View(new List<ApiWord>());
        }

        /// <summary>
        /// 发布接口 页面
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public ActionResult ApiAdd(int apiId = 0, int type = 1,int act=0)
        {
            ApiWord apiWord = ncBase.CurrentEntities.ApiWord.Where(n => n.ApiWordId == apiId).FirstOrDefault();
            ViewBag.Act = act;
            return View(apiWord);
        }
        /// <summary>
        /// 添加新接口 
        /// </summary>
        /// <returns></returns>
        [System.Web.Mvc.HttpPost]
        [ValidateInput(false)]
        public JsonResult ApiNew(ApiWord entity)
        {
            entity.Publisher = this.GetLoginUser().Name;
          
            ApiWord apiWord =new ApiWord();
            if (entity.ApiWordId > 0)
            {
                 apiWord = ncBase.CurrentEntities.ApiWord.Where(n => n.ApiWordId == entity.ApiWordId).FirstOrDefault();
                if (apiWord.IsNoNull())//修改
                {
                    apiWord.Type = entity.Type;
                    apiWord.Title = entity.Title;
                    apiWord.ApiWordContent = entity.ApiWordContent;
                    apiWord.Publisher = entity.Publisher;
                    apiWord.Url = entity.Url;
                    apiWord.Method = entity.Method;
                    apiWord.IsLogin = entity.IsLogin;
                    ncBase.CurrentEntities.SaveChanges();
                    return Json(new { status = apiWord.ApiWordId });
                }
            }
            apiWord = new ApiWord();
            apiWord.Type = entity.Type;
            apiWord.Title = entity.Title;
            apiWord.ApiWordContent = entity.ApiWordContent;
            apiWord.Publisher = entity.Publisher;
            apiWord.Url = entity.Url;
            apiWord.Method = entity.Method;
            apiWord.CreateTime =DateTime.Now;
            apiWord.Hits =0;
            apiWord.IsLogin = entity.IsLogin;
            ncBase.CurrentEntities.AddToApiWord(apiWord);
            ncBase.CurrentEntities.SaveChanges();

            return Json(new { status = apiWord.ApiWordId });
        }
    }
}
