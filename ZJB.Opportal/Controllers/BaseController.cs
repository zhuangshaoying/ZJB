using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ZJB.Opportal.Controllers
{
    public class BaseController : Controller
    {
        
        public JsonResult JsonReturnValue(object data, JsonRequestBehavior jsonRequestBehavior)
        {
            if (Request.AcceptTypes != null && !Request.AcceptTypes.Contains("application/json"))
                return Json(data, "text/plain", jsonRequestBehavior);
            return Json(data, jsonRequestBehavior);
        }

    }
}
