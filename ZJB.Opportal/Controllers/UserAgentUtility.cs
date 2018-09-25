using System;
using System.Web;

namespace ZJB.Web.Utilities
{
    public class UserAgentUtility
    {
        public static bool IsIE9(string userAgent)
        {
            throw new NotImplementedException();
        }
        public static bool IsIE8(string userAgent)
        {
            throw new NotImplementedException();
        }
        public static bool IsIE7(string userAgent)
        {
            throw new NotImplementedException();
        }
        public static bool IsIE6(string userAgent)
        {
            throw new NotImplementedException();
        }
        public static bool IsIE(string userAgent)
        {
            throw new NotImplementedException();
        }
        public static bool IsFirefox(string userAgent)
        {
            throw new NotImplementedException();
        }
        public static bool IsChrome(string userAgent)
        {
            throw new NotImplementedException();
        }

        public static bool IsBaidu(string userAgent)
        {
            return userAgent.IndexOf("Baiduspider", StringComparison.OrdinalIgnoreCase)>=0;
        }
        public static bool IsGoogle(string userAgent)
        {
            return userAgent.IndexOf("Googlebot", StringComparison.OrdinalIgnoreCase) >= 0;
        }
        public static bool IsYahoo(string userAgent)
        {
            return userAgent.IndexOf("Yahoo! Slurp", StringComparison.OrdinalIgnoreCase) >= 0;
        }
        public static bool IsSoso(string userAgent)
        {
            return userAgent.IndexOf("Sosospider", StringComparison.OrdinalIgnoreCase) >= 0;
        }
        public static bool IsBing(string userAgent)
        {
            return userAgent.IndexOf("bingbot", StringComparison.OrdinalIgnoreCase) >= 0;
        }
        public static bool IsSogou(string userAgent)
        {
            return userAgent.IndexOf("Sogou web spider", StringComparison.OrdinalIgnoreCase) >= 0;
        }
        public static bool IsSearchEngine(HttpRequest request)
        {
            string userAgent = request.UserAgent;

            if (string.IsNullOrEmpty(userAgent))
                return false;

            if (request.HttpMethod.Equals("POST", StringComparison.OrdinalIgnoreCase) && !(userAgent.IndexOf("Crawl", StringComparison.OrdinalIgnoreCase) >= 0))
                return false;

            return userAgent.IndexOf("spider", StringComparison.OrdinalIgnoreCase) >= 0
                || userAgent.IndexOf("bot", StringComparison.OrdinalIgnoreCase) >= 0
                || userAgent.IndexOf("Slurp", StringComparison.OrdinalIgnoreCase) >= 0
                || userAgent.IndexOf("Crawl", StringComparison.OrdinalIgnoreCase) >= 0;
        }
    }
}
