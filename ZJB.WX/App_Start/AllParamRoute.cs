using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace ZJB.WX
{

    public static class AllParamRouteType
    {
        public static AllParamRoute MapAllParamRoute(this RouteCollection routes, string name, string url, object defaults, object constraints = null, object paramPattern = null)
        {
            var route = new AllParamRoute(url,
                new RouteValueDictionary(defaults),
                constraints != null ? new RouteValueDictionary(constraints) : null,
                new RouteValueDictionary(), //dataTokens
                new MvcRouteHandler(),
                paramPattern != null ? new RouteValueDictionary(paramPattern) : null
                );
            routes.Add(name, route);
            return route;
        }
    }
    public class AllParamRoute : Route
    {

        private static readonly string allparamsKey = "allparams";
        private RouteValueDictionary paramPattern = new RouteValueDictionary { { "zone", @"z(\d+)" }, { "prices", @"p(\d+)" }, { "key", @"k(\d+)" } };
        private string pattern = string.Empty;
        private List<string> keys = new List<string>();
        private List<string> keysFmt = new List<string>();

        private static string processAllParamUrl(string url)
        {
            string result = url;
            if (url.IndexOf(".") > -1)
            {
                result = result.Substring(0, url.IndexOf("."))
                    + "/{*" + allparamsKey + "}"
                    + result.Substring(url.IndexOf("."));
            }
            else
            {
                result += "/{*" + allparamsKey + "}";
            }
            return result;
        }
        public AllParamRoute(string url, RouteValueDictionary defaults, RouteValueDictionary constraints, RouteValueDictionary dataTokens, IRouteHandler routeHandler, RouteValueDictionary paramPattern = null)
            : base(processAllParamUrl(url), defaults, constraints, dataTokens, routeHandler)
        {
            this.paramPattern = paramPattern ?? this.paramPattern;
            if (this.paramPattern != null)
            {
                foreach (var item in paramPattern)
                {
                    if (item.Value == null)
                    {
                        continue;
                    }
                    if (pattern.Length > 0)
                    {
                        pattern += "|";
                    }
                    keys.Add(item.Key);
                    pattern += item.Value;
                    string keyFmt = item.Value.ToString();
                    if (keyFmt.Contains("(") && keyFmt.Contains(")"))
                    {
                        keyFmt = keyFmt.Substring(0, keyFmt.LastIndexOf("("))
                           + "{" + item.Key + "}"
                           + keyFmt.Substring(keyFmt.LastIndexOf(")") + 1)
                           ;
                    }
                    keysFmt.Add(keyFmt);
                }
            }
        }
        public override RouteData GetRouteData(HttpContextBase httpContext)
        {
            var result = base.GetRouteData(httpContext);
            if (result == null) return null;
            if (paramPattern == null || !result.Values.ContainsKey(allparamsKey))
            {
                return result;
            }
            string sourse = result.Values[allparamsKey] as string;
            if (string.IsNullOrEmpty(sourse))
            {
                return result;
            }
            var match = System.Text.RegularExpressions.Regex.Match(sourse, pattern);
            while (match != null && match.Success)
            {
                for (int k = 1; k < match.Groups.Count; k++)
                {
                    string matchValue = match.Groups[k].Value;
                    if (matchValue != null)
                    {
                        string key = keys[k - 1];
                        if (matchValue != string.Empty)
                        {
                            result.Values[key] = matchValue;
                        }
                    }
                }
                match = match.NextMatch();
            }
            return result;
        }

        public override VirtualPathData GetVirtualPath(RequestContext requestContext, RouteValueDictionary values)
        {
            var elegantValues = new RouteValueDictionary(values);
            bool hasParamPattern = false;
            foreach (var item in requestContext.RouteData.Values)
            {
                if (!elegantValues.ContainsKey(item.Key))
                {
                    elegantValues.Add(item.Key, item.Value);
                }
            }
            foreach (var item in paramPattern)
            {
                if (elegantValues.ContainsKey(item.Key))
                {
                    hasParamPattern = true;
                    break;
                }
            }
            if (hasParamPattern)
            {
                string allParam = string.Empty;
                int index = -1;
                foreach (var key in keys)
                {
                    index++;
                    if (values.ContainsKey(key))
                    {
                        object v = null;
                        values.TryGetValue(key, out v);
                        string value = v != null ? v.ToString() : null;
                        if (!string.IsNullOrEmpty(value))
                        {
                            allParam += keysFmt[index].Replace("{" + key + "}", value);
                        }
                        elegantValues.Remove(key);
                        //requestContext.RouteData.Values.Remove(key);
                    }
                }
                elegantValues[allparamsKey] = allParam;
            }
            else
            {
                elegantValues[allparamsKey] = "";
            }
            var result = base.GetVirtualPath(requestContext, elegantValues);
            return result;
        }

    }
}