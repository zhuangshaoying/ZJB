using System.Web;
using System.Web.Mvc;
using ZJB.WX.Filters;

namespace ZJB.WX
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
            filters.Add(new ActionLogAttribute());
            filters.Add(new NoCacheGlobalActionFilter());
            
        }
    }
}