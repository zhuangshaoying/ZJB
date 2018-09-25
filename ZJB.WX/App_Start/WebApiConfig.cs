using System.Web.Http;
using System.Web.Http.Dispatcher;
using ZJB.Core.Utilities;
using ZJB.WX.Filters;


namespace ZJB.WX
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{action}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );
            config.Formatters.Remove(config.Formatters.XmlFormatter);

            config.Routes.MapHttpRoute(
                name: "Api",
                routeTemplate: "{namespace}/{controller}/{action}/{id}",
                defaults: new { version = "1", id = RouteParameter.Optional }, constraints: new { version = @"^\d+$" }
            );



            config.Filters.Add(new WatchActionExecutedAttribute());
            config.Filters.Add(new ExceptionLogAttribute());
            config.Filters.Add(new ValidateRequestAttribute());
            config.IncludeErrorDetailPolicy = ConfigUtility.GetBoolValue("OnProduction")
                                                  ? IncludeErrorDetailPolicy.Never
                                                  : IncludeErrorDetailPolicy.Always;

           //  config.Services.Replace(typeof(IHttpControllerSelector), new NamespaceHttpControllerSelector(config));
        }
    }
}
