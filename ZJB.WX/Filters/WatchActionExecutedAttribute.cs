using System;
using System.Web.Http.Filters;

namespace ZJB.WX.Filters
{
    public class WatchActionExecutedAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(System.Web.Http.Controllers.HttpActionContext actionContext)
        {
            actionContext.Request.Properties["ActionStartTime"] = DateTime.Now;
            base.OnActionExecuting(actionContext);
        }

        public override void OnActionExecuted(HttpActionExecutedContext actionExecutedContext)
        {
            if (actionExecutedContext.Exception == null)
            {
                DateTime startTime = Convert.ToDateTime(actionExecutedContext.Request.Properties["ActionStartTime"]);
                double millisecond = DateTime.Now.Subtract(startTime).TotalMilliseconds;
                actionExecutedContext.Response.Headers.Add("Execute-Time", millisecond.ToString());
            }
            base.OnActionExecuted(actionExecutedContext);
        }
    }

}