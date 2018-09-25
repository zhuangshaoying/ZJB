using System;
using System.Net;
using System.Web;

namespace ZJB.Web.Utilities
{
    public static class IpUtility
    {
        public static string GetIp()
        {
            HttpRequest request = HttpContext.Current.Request;
            if (!string.IsNullOrEmpty(request.Headers["X-ARR-LOG-ID"]) && !string.IsNullOrEmpty(request.Headers["X-Forwarded-For"]))
            {
                return request.Headers["X-Forwarded-For"].Split(':')[0];
            }
            return request.UserHostAddress;
        }

        public static uint ToUInt32(string ipString)
        {
            IPAddress ip;
            if (IPAddress.TryParse(ipString, out ip))
            {
                byte[] bytes = ip.GetAddressBytes();
                Array.Reverse(bytes);
                return BitConverter.ToUInt32(bytes, 0);
            }

            return 0;
        }
    }

}
