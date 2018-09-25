using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Newtonsoft.Json;
using ZJB.Api.BLL;
using ZJB.Api.Models;
using ZJB.Api.Entity;
namespace ZJB.Opportal.Controllers
{
  [Authorization]
    public class HomeController : Controller
    {

        private NCBaseRule ncBase = new NCBaseRule();
        private UserBll userBll = new UserBll();
        public ActionResult Index()
        {
           
            return View();
        }
        public ActionResult MainRight()
        {
           
            return View();
        }

        /// <summary>
        /// Ueditor
        /// </summary>
        /// <returns></returns>
        public ActionResult UeditorContro()
        {
            string jsonpCallback = Request["callback"],
             json = JsonConvert.SerializeObject(null);
            if (String.IsNullOrWhiteSpace(jsonpCallback))
            {
                return Content(json);

            }
            else
            {
                return Content(String.Format("{0}({1});", jsonpCallback, json));
            }

        }
    }
}
