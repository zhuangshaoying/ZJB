using System;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ZJB.Api.Models;
using ZJB.Web.Utilities;
using ZJB.Api.Entity;
using ZJB.Api.BLL;

namespace ZJB.WX.Filters
{

        public class NoCacheGlobalActionFilter : ActionFilterAttribute
        {
            public override void OnResultExecuted(ResultExecutedContext filterContext)
            {

                HttpCachePolicyBase cache = filterContext.HttpContext.Response.Cache;
                cache.SetCacheability(HttpCacheability.NoCache);

                base.OnResultExecuted(filterContext);
            }
        }
    
}