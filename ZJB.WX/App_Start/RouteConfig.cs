using System.Collections.Generic;
using System.Web.Mvc;
using System.Web.Routing;

namespace ZJB.WX
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

         
            //routes.MapRoute(
            //   name: "Map_HouseList",
            //     url: "{city}-{trade}/{*search}",
            //    defaults: new
            //    {
            //        controller = "House",
            //        action = "HouseList",
            //        city = UrlParameter.Optional,
            //        build = UrlParameter.Optional,
            //        trade = UrlParameter.Optional
            //    },
            //   constraints: new { trade = "^esf|^zf", build = @"\d+" }
              
            //);
            
         //   routes.MapRoute(
         //    "Map_HouseList",
         //    "{city}-{trade}/{*search}",
         //    new
         //    {
         //        controller = "House",
         //        action = "HouseList",
         //        city = "xm",
         //        trade = "esf",

         //    },
         //    new { controller = "^H.*", trade = "^esf|^zf" },
         //  null
         //);


            routes.MapRoute(
               name: "HouseCity",
               url: "city",
               defaults: new
               {
                   controller = "Home",
                   action ="City",
             

               },
                constraints:null, namespaces: null
            );

            routes.MapRoute(
                    name: "HouseView",
                    url: "v/{id}.html",
                    defaults: new
                     {
                         controller = "House",
                         action = "Detail",
                         id = UrlParameter.Optional

                     },
                     constraints:  new { id = @"\d+" },namespaces:null
                 );

            routes.MapAllParamRoute(  // MapRoute
                "Map_HouseList",
                "{city}-{trade}",
                new { controller = "House", action = "HouseList", city = "xm", trade = "esf" },
                new {  trade = "^esf|^zf" },
                new
                {
                    distrct = @"d(\d+)",
                    region = @"r(\d+)",
                    userType = @"u(\d+)",

                    layout = @"l(\d+)",
                    buildType = @"b(\d+)",
                    minPrice = @"minp(\d+)",
                    maxPrice = @"maxp(\d+)",
                    minArea = @"mina(\d+)",
                    maxArea = @"maxa(\d+)",
                    postTimeDay = @"pt(\d+)",
                    orderBy = @"o(\d+)",
                    page = @"p(\d+)"
                }
            );
            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}",
                defaults: new { controller = "Esf", action = "Index" }
            
            );
        }
    }
}