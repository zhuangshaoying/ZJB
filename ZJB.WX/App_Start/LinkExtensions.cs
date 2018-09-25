using System.Collections.Generic;
using System.Web.Mvc.Html;
using System.Linq;
using ZJB.Api;
using ZJB.WX.Models;

namespace System.Web.Mvc
{
    public static class LinkExtensions
    {
        public static MvcHtmlString RouteLink(this HtmlHelper helper, string linkText, HouseParameter sellParameter, Action<HouseParameter> op, string normalClass, string equalClass)
        {
            HouseParameter clone = (HouseParameter)sellParameter.Clone();
         
            op(clone);

            var htmlAttribute = new { @class = (clone.Equals(sellParameter) ? equalClass : normalClass) };

            return helper.RouteLink(linkText, "Map_HouseList", clone, htmlAttribute);
        }

        public static MvcHtmlString RouteLink(this HtmlHelper helper, string linkText, HouseParameter sellParameter, int defaultOrder, string defaultClass, int oppositeOrder, string oppositeClass)
        {
            HouseParameter clone = (HouseParameter)sellParameter.Clone();

            clone.OrderBy = (clone.OrderBy == defaultOrder) ? oppositeOrder : defaultOrder;

            var htmlAttribute = new { @class = (clone.OrderBy == defaultOrder) ? oppositeClass : defaultClass };


            return helper.RouteLink(linkText, "Map_HouseList", clone, htmlAttribute);
        }

        public static MvcHtmlString RouteLinkForRegion(this HtmlHelper helper, string linkText, HouseParameter sellParameter, int distrct, string normalClass, string equalClass)
        {
            HouseParameter clone = (HouseParameter)sellParameter.Clone();

            clone.Distrct = distrct;
            clone.Region = null;
            clone.Page = null;
            string regionClass = distrct==sellParameter.Distrct
                                     ? equalClass
                                     : normalClass;

            return helper.RouteLink(linkText, "Map_HouseList", clone, new { @class = regionClass });
        }
    }
}