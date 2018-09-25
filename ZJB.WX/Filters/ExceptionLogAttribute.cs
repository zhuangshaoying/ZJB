using System.Web.Http.Filters;
using log4net;

namespace ZJB.WX.Filters
{
    public class ExceptionLogAttribute : ExceptionFilterAttribute
    {
        private ILog logger = LogManager.GetLogger("ExceptionLog");

        public override void OnException(HttpActionExecutedContext actionExecutedContext)
        {
            logger.Error(actionExecutedContext.Exception.Message, actionExecutedContext.Exception);
            base.OnException(actionExecutedContext);
        }
    }
}