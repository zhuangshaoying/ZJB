using System;
using System.Net;
using System.Web;

namespace ZJB.Web.Utilities
{
    public static class CnzzUtility
    {
       
            private const string ImageDomain = "c.cnzz.com";
            public static string TrackPageView(int SiteId = 0)
            {
                HttpRequest request = HttpContext.Current.Request;
                string scheme = request != null ? request.IsSecureConnection ? "https" : "http" : "http";
                string referer = request != null && request.UrlReferrer != null && "" != request.UrlReferrer.ToString() ? request.UrlReferrer.ToString() : "";
                String rnd = new Random().Next(0x7fffffff).ToString();
                return scheme + "://" + ImageDomain + "/wapstat.php" + "?siteid=" + SiteId + "&r=" + HttpUtility.UrlEncode(referer) + "&rnd=" + rnd;
            }
      
    }

}
