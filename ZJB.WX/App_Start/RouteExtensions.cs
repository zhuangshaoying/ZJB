using System.Linq;
using System.Web.Routing;

namespace System.Web.Mvc
{
    public static class RouteExtensions
    {
        public static Route MapRoute(this RouteCollection routes, string name, string domain, string url, object defaults, object constraints, string[] namespaces)
        {
            if (routes == null)
            {
                throw new ArgumentNullException("routes");
            }
            if (url == null)
            {
                throw new ArgumentNullException("url");
            }

            HouseRoute route = new HouseRoute(url, defaults, new MvcRouteHandler())
            {
                Defaults = new RouteValueDictionary(defaults),
                Constraints = new RouteValueDictionary(constraints),
                DataTokens = new RouteValueDictionary()
            };

            if ((namespaces != null) && (namespaces.Length > 0))
            {
                route.DataTokens["Namespaces"] = namespaces;
            }

            routes.Add(name, route);

            return route;
        }

        public static Route MapRoute(this AreaRegistrationContext context, string name, string domain, string url, object defaults, object constraints , string[] namespaces )
        {
       
            if (namespaces == null && context.Namespaces != null)
            {
                namespaces = context.Namespaces.ToArray();
            }

            Route route = context.Routes.MapRoute(name, domain, url, defaults, constraints, namespaces);
            route.DataTokens["area"] = context.AreaName;

            bool useNamespaceFallback = (namespaces == null || namespaces.Length == 0);
            route.DataTokens["UseNamespaceFallback"] = useNamespaceFallback;

            return route;
        }
    }
}
