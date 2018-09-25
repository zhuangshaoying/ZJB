using System.Collections.Generic;
using System.Web.Routing;
using System.Text.RegularExpressions;

namespace System.Web.Mvc
{
    public class HouseRoute : Route
    {
        private Regex pathRegex;

        public string Area { get; set; }

        private string urlWithRegex;

        private static string RemoveRegex(string url)
        {
            return url.Replace("(", string.Empty).Replace(")?", string.Empty);
        }

        public HouseRoute( string url, RouteValueDictionary defaults)
            : base(RemoveRegex(url), defaults, new MvcRouteHandler())
        {
          
            urlWithRegex = url;
        }

        public HouseRoute( string url, RouteValueDictionary defaults, IRouteHandler routeHandler)
            : base(RemoveRegex(url), defaults, routeHandler)
        {
            
            urlWithRegex = url;
        }

        public HouseRoute( string url, object defaults)
            : base(RemoveRegex(url), new RouteValueDictionary(defaults), new MvcRouteHandler())
        {
          
            urlWithRegex = url;
        }

        public HouseRoute( string url, object defaults, IRouteHandler routeHandler)
            : base(RemoveRegex(url), new RouteValueDictionary(defaults), routeHandler)
        {
           
            urlWithRegex = url;
        }

        private string GetHost(HttpContextBase httpContext)
        {
            string requestDomain = httpContext.Request.Headers["host"];
            if (!string.IsNullOrEmpty(requestDomain))
            {
                if (requestDomain.IndexOf(":") > 0)
                {
                    requestDomain = requestDomain.Substring(0, requestDomain.IndexOf(":"));
                }
            }
            else
            {
                requestDomain = httpContext.Request.Url.Host;
            }

            return requestDomain;
        }
        public override RouteData GetRouteData(HttpContextBase httpContext)
        {
            pathRegex = CreateRegex(urlWithRegex);

            string requestDomain = GetHost(httpContext);


            RouteData data = null;
            Match pathMatch = null;


            if (requestDomain.StartsWith(Area + ".", StringComparison.OrdinalIgnoreCase))
            {
                string requestPath = httpContext.Request.Url.AbsolutePath.Substring(1) ;

                pathMatch = pathRegex.Match(requestPath);


            }
            else
            {

                string requestPath = httpContext.Request.Url.AbsolutePath.Substring(1);

                if (requestPath.StartsWith(Area + "/", StringComparison.OrdinalIgnoreCase) || requestPath.Equals(Area, StringComparison.OrdinalIgnoreCase))
                {
                    requestPath = requestPath.Remove(0, Area.Length);
                    if (requestPath.StartsWith("/"))
                        requestPath = requestPath.Remove(0, 1);

                    pathMatch = pathRegex.Match(requestPath);
                }

            }
            if (pathMatch != null && pathMatch.Success)
            {
                data = new RouteData(this, RouteHandler);

                if (Defaults != null)
                {
                    foreach (KeyValuePair<string, object> item in Defaults)
                    {
                        data.Values[item.Key] = item.Value;
                    }
                }

                for (int i = 1; i < pathMatch.Groups.Count; i++)
                {
                    Group group = pathMatch.Groups[i];
                    if (group.Success)
                    {
                        string key = pathRegex.GroupNameFromNumber(i);

                        if (!string.IsNullOrEmpty(key) && !char.IsNumber(key, 0))
                        {
                            if (!string.IsNullOrEmpty(group.Value))
                            {
                                data.Values[key] = HttpUtility.UrlDecode(group.Value);
                            }
                        }
                    }
                }

                foreach (var item in this.DataTokens)
                {
                    data.DataTokens.Add(item.Key, item.Value);
                }

                if (!this.ProcessConstraints(httpContext, data.Values, RouteDirection.IncomingRequest))
                {
                    return null;
                }
            }
            return data;
        }


        private bool ProcessConstraints(HttpContextBase httpContext, RouteValueDictionary values, RouteDirection routeDirection)
        {
            if (this.Constraints != null)
            {
                foreach (KeyValuePair<string, object> current in this.Constraints)
                {
                    if (!this.ProcessConstraint(httpContext, current.Value, current.Key, values, routeDirection))
                    {
                        return false;
                    }
                }
                return true;
            }
            return true;
        }


        public override VirtualPathData GetVirtualPath(RequestContext requestContext, RouteValueDictionary values)
        {
            RouteValueDictionary routeValue = RemoveDomainTokens(values);
            string requestDomain = GetHost(requestContext.HttpContext); ;


            VirtualPathData virtualPathData = base.GetVirtualPath(requestContext, routeValue);

            if (virtualPathData == null) return null;

            if (!requestDomain.StartsWith(Area + ".", StringComparison.OrdinalIgnoreCase))
            {
                virtualPathData.VirtualPath = Area + "/" + virtualPathData.VirtualPath;
            }

            return virtualPathData;
        }

        public DomainData GetDomainData(RequestContext requestContext, RouteValueDictionary values)
        {
            string hostname = Area;
            foreach (KeyValuePair<string, object> pair in values)
            {
                hostname = hostname.Replace("{" + pair.Key + "}", pair.Value.ToString());
            }

            return new DomainData
            {
                Protocol = "http",
                HostName = hostname,
                Fragment = ""
            };
        }

        private Regex CreateRegex(string source)
        {
            source = source.Replace("/", @"\/?");
            source = source.Replace(".", @"\.?");
            source = source.Replace("-", @"\-?");
            source = source.Replace("{", @"(?<");
            source = source.Replace("}", @">([a-zA-Z0-9%]*))");

            return new Regex("^" + source + "$", RegexOptions.IgnoreCase);
        }

        private RouteValueDictionary RemoveDomainTokens(RouteValueDictionary values)
        {
            Regex tokenRegex = new Regex(@"({[a-zA-Z0-9]*})*-?\.?\/?({[a-zA-Z0-9]*})*-?\.?\/?({[a-zA-Z0-9]*})*-?\.?\/?({[a-zA-Z0-9]*})*-?\.?\/?({[a-zA-Z0-9]*})*-?\.?\/?({[a-zA-Z0-9]*})*-?\.?\/?({[a-zA-Z0-9]*})*-?\.?\/?({[a-zA-Z0-9]*})*-?\.?\/?({[a-zA-Z0-9]*})*-?\.?\/?({[a-zA-Z0-9]*})*-?\.?\/?({[a-zA-Z0-9]*})*-?\.?\/?({[a-zA-Z0-9]*})*-?\.?\/?");
            Match tokenMatch = tokenRegex.Match(Area);
            for (int i = 0; i < tokenMatch.Groups.Count; i++)
            {
                Group group = tokenMatch.Groups[i];
                if (group.Success)
                {
                    string key = group.Value.Replace("{", "").Replace("}", "");
                    if (values.ContainsKey(key))
                        values.Remove(key);
                }
            }

            return values;
        }
    }
    public class DomainData
    {
        public string Protocol { get; set; }
        public string HostName { get; set; }
        public string Fragment { get; set; }
    }
}
