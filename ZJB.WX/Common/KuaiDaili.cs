using System;
using System.Net;
using ZJB.Core.Utilities;

namespace ZJB.WX.Common
{
    internal class KuaiDaili
    {
        private const string api = "http://www.kuaidaili.com/api/getproxy/?orderid=932174366195957&num={0}" +
                                   "&area=%E7%A6%8F%E5%BB%BA&browser=1&protocol=1&method=2&sort=0&sep=1&an_ha=1";

        private static long lastInitTime;

        public static WebProxy GetProxy()
        {
            long now =  DateTimeUtility.ToUnixTime(DateTime.Now);
            if (now - lastInitTime > 300) //代理有时效性
            {
                InitProxys();
            }
            string value = RedisHelper.OutQueue("KuaiDailiFujian");
            if (string.IsNullOrEmpty(value))
            {
                InitProxys();
                value = RedisHelper.OutQueue("KuaiDailiFujian");
            }
            if (string.IsNullOrEmpty(value))
            {
                throw new WebException("(KDL)获取代理失败！");
            }
            string[] values = value.Replace("\r", "").Split(':');
            try
            {
                var proxy = new WebProxy(values[0], int.Parse(values[1]));
                return proxy;
            }
            catch (Exception e)
            {
                throw new Exception("values:" + values + "\n" + e.StackTrace);
            }
        }

        private static void InitProxys()
        {
            lastInitTime = DateTimeUtility.ToUnixTime(DateTime.Now);
            RedisHelper.ClearQueue("KuaiDailiFujian");
            string[] kuaidailis = GetKuaiDailis();
            foreach (string proxy in kuaidailis)
            {
                RedisHelper.Inqueue("KuaiDailiFujian", proxy);
            }
        }

        private static string[] GetKuaiDailis()
        {
            string ret = HttpUtility.GetString(string.Format(api, 5));
            return ret.Split('\n');
        }
    }
}