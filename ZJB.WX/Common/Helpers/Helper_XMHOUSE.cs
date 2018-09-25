using System.Collections.Specialized;
using System.Net;
using System.Text.RegularExpressions;
using ZJB.Core.Utilities;

namespace ZJB.WX.Common.xms
{
    internal abstract class Helper_XMhouse
    {
        public static string GetValidate(string url, string cookie, out string viewState)
        {
            string html = HttpUtility.GetString(url, GetHeaders(cookie));
            viewState = GetViewState(html);
            string eventValidation = GetEventValidation(html);
            return eventValidation;
        }

        public static NameValueCollection GetHeaders(string cookie)
        {
            return new NameValueCollection
            {
                {
                    "Referer",
                    "http://my.xmhouse.com/"
                },
                {"Content-Type", "application/x-www-form-urlencoded"},
                {"Accept", "text/html,application/xhtml+xml,application/xml;q=0.9,*/*;q=0.8"},
                {"Accept-Encoding", "deflate"},
                {"Accept-Charset", "utf-8, iso-8859-1, utf-16, *;q=0.7"},
                {"Cookie", cookie},
                {"User-Agent", "Mozilla/5.0 (Windows NT 6.1; WOW64; rv:35.0) Gecko/20100101 Firefox/35.0_Crawl"},
            };
        }

        public static string GetViewState(string html)
        {
            Match rm = Regex.Match(html, "id=\"__VIEWSTATE\" value=\"(.*?)\"");
            return rm.Success ? rm.Groups[1].Value : null;
        }

        private static string GetEventValidation(string html)
        {
            Match rm = Regex.Match(html, "id=\"__EVENTVALIDATION\" value=\"(.*?)\"");
            return rm.Success ? rm.Groups[1].Value : null;
        }

        public static WebHeaderCollection GetWebHeaderCollection()
        {
            NameValueCollection headers = GetHeaders("");
            var headerCollection = new WebHeaderCollection();
            if (headers == null) return headerCollection;
            foreach (string key in headers.Keys)
            {
                headerCollection.Set(key, headers[key]);
            }
            return headerCollection;
        }
    }
}