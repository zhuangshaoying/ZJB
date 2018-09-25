using System;
using System.IO;
using System.Text;
using System.Web;
using System.Web.Caching;
using ZJB.Core.Logging;
using ZJB.Core.Utilities;
using ZJB.Web.Utilities;

namespace ZJB.Web.SiteGuard
{
    public class SiteGuardModule:IHttpModule
    {
        private string RULE_KEY = "RULE_KEY";
        private string configFile = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "SiteGuardRule.xml");
        private Logger logger = new Logger(typeof(SiteGuardModule));

        public void Dispose()
        {
        }

        public void Init(HttpApplication application)
        {
            application.BeginRequest += BeginRequest;
        }

        void BeginRequest(object sender, EventArgs e)
        {
            HttpApplication application = sender as HttpApplication;
            if(application==null) return;

            HttpContext context = application.Context;

            if (string.IsNullOrEmpty(context.Request.UserAgent))
            {
                Inspect(context, BlockType.Forbidden, string.Empty);
            }

            if (!UserAgentUtility.IsSearchEngine(context.Request))
            {
                SiteGuardRules rules = GetRules(context);

                foreach (var rule in rules)
                {
                    if(context.Request.Url.AbsolutePath.StartsWith(rule.Path, StringComparison.OrdinalIgnoreCase) && ("ALL".Equals(rule.HttpMethod,StringComparison.OrdinalIgnoreCase) ||  context.Request.HttpMethod.Equals(rule.HttpMethod,StringComparison.OrdinalIgnoreCase)))
                    {
                        string keyBase = rule.HttpMethod.ToLower() + "_" + rule.Path.ToLower() + "_" +
                                        IpUtility.GetIp();
                        string countKey = keyBase + "_Count";
                        string blockKey = keyBase + "_Block";

                        if (context.Cache[blockKey] != null)
                        {   
                            Inspect(context, rule.BlockType, rule.Message);
                            return;
                        }


                        object obj = context.Cache[countKey];
                        if (obj == null)
                        {
                            context.Cache.Add(countKey, new Counter { Count = 1 }, null, DateTime.Now.AddSeconds(rule.TimePeriod), Cache.NoSlidingExpiration, CacheItemPriority.AboveNormal, null);
                        }
                        else
                        {
                            Counter counter = (Counter)obj;

                            counter.Count++;

                            if (counter.Count > rule.MaxRequests)
                            {
                                context.Cache.Add(blockKey, true, null, DateTime.Now.AddSeconds(rule.BlockTime), Cache.NoSlidingExpiration, CacheItemPriority.AboveNormal, null);
                                Inspect(context, rule.BlockType, rule.Message);
                             }
                        }
                    }
                }
            }
        }

        internal void Inspect(HttpContext context, BlockType blockType, string message)
        {
            //logger.Error(message, new SiteGuardException(message));

            context.Response.Clear();

            if (blockType == BlockType.Forbidden)
            {
                context.Response.Status = "403 Forbidden";
                context.Response.StatusCode = 403;
            }
            else if (blockType == BlockType.NotFound)
            {

                context.Response.Status = "404 Not Found";
                context.Response.StatusCode = 404;
            }

            if (!string.IsNullOrEmpty(message))
                context.Response.Write(message);

            context.Response.End();
        }

        internal SiteGuardRules GetRules(HttpContext context)
        {
            var rules = context.Cache[RULE_KEY] as SiteGuardRules;

            if (rules == null)
            {
                using (StreamReader reader = new StreamReader(configFile, Encoding.UTF8))
                {
                    rules = XmlUtility.Deserialize<SiteGuardRules>(reader.ReadToEnd());
                }

                context.Cache.Insert(RULE_KEY, rules, new CacheDependency(configFile),
                                     Cache.NoAbsoluteExpiration, TimeSpan.Zero, CacheItemPriority.High, null);
            }

            return rules;
        }
    }

    class Counter
    {
        public int Count { get; set; }
    }
}
