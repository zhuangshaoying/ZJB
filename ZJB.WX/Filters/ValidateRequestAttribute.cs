using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;
using System.Xml;
using System.Xml.Serialization;
using ZJB.Api.BLL;
using ZJB.Api.Common;
using ZJB.Api.Models;
using ZJB.Core.Injection;
using ZJB.Core.Utilities;
using ZJB.Web.Utilities;

namespace ZJB.WX.Filters
{
    public class ValidateRequestAttribute : ActionFilterAttribute
    {
        private static AcceptedClients clients;
        private readonly ApiBll apiBll = Container.Instance.Resolve<ApiBll>();

        public override void OnActionExecuting(HttpActionContext actionContext)
        {
            bool ignoreValidate = (actionContext.ActionDescriptor.GetCustomAttributes<IgnoreValidateAttribute>().Count > 0);

            HttpRequest request = HttpContext.Current.Request;
            SpecialLog(request);
            ApiLogModel log = BuildApiLog(request);
            log.Controller = actionContext.ControllerContext.ControllerDescriptor.ControllerName;
            log.Action = actionContext.ActionDescriptor.ActionName;
            CheckResult result = ignoreValidate ? CheckResult.FROM_ZJB : Check(request);
            log.ValidationResult = result.ToString();
            if (result == CheckResult.INVALID_HASH || result == CheckResult.INVALID_USERAGENT)
            {
                apiBll.AddApiLog(log);
                actionContext.Response = actionContext.Request.CreateResponse(HttpStatusCode.Forbidden,
                                                                          new ApiResponse(Metas.INVALID_PERMISSION, null));
                return;
            }
            apiBll.AddApiLog(log);
        }

        private static void SpecialLog(HttpRequest request)
        {
            foreach (string key in request.Headers.Keys)
            {
                if (!CacheFactory.GetInstance().IsSet(key)) continue;

                string value = CacheFactory.GetInstance().Get<string>(key);

                if (string.IsNullOrEmpty(value)) continue;

                if (value.Equals(request.Headers[key], StringComparison.OrdinalIgnoreCase))
                {
                    HttpContext.Current.Items["SpecialLog"] = string.Format("{0}={1}", key, value);
                    break;
                }
            }
        }

        private static ApiLogModel BuildApiLog(HttpRequest request)
        {
            ApiLogModel log = new ApiLogModel()
            {
                Hash = request.Headers["Hash"],
                Token = request.Headers["Token"],
                Host = request.Url.Host,
                PathAndQuery = request.Url.PathAndQuery,
                Server = Environment.MachineName,
                UserAgent = request.UserAgent,
                IpAddress = IpUtility.GetIp(),
                IMEI = request.Headers["IMEI"],
                Device = request.Headers["Device"],
                OsVersion = request.Headers["OsVersion"],
                MacAddress = request.Headers["MacAddress"]
            };
            return log;
        }

        private static Client GetClient(HttpRequest request)
        {
            if (clients == null)
            {
                string config = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"Config/AcceptedClients.config");
                using (StreamReader reader = new StreamReader(config, Encoding.UTF8))
                {
                    string xml = reader.ReadToEnd();
                    clients = XmlUtility.Deserialize<AcceptedClients>(xml);
                }
            }
            return clients.Clients.FirstOrDefault(
                 x =>
                 string.Equals(x.UserAgent, request.UserAgent,
                               StringComparison.OrdinalIgnoreCase));
        }

        private static CheckResult Check(HttpRequest request)
        {
            if (request.Cookies["EnableApiDebug"] != null)
            {
                return CheckResult.API_DEBUG;
            }
            //if (IpUtility.GetIp() == "::1" || IpUtility.GetIp().Contains("192.168."))
            //{
            //    return CheckResult.API_DEBUG;
            //}
            Client client = GetClient(request);
            string hash = request.Headers["Hash"];
            string pathAndQuery = request.Url.PathAndQuery;
            if (client == null)
            {
                return CheckResult.INVALID_USERAGENT;
            }
            else
            {
                MD5 md5 = new MD5CryptoServiceProvider();
                string toHash = pathAndQuery + client.Salt;
                string hashString = StringUtility.ToMd5String(toHash);
                if (!hashString.Equals(hash, StringComparison.OrdinalIgnoreCase))
                {
                    return CheckResult.INVALID_HASH;
                }
            }

            return CheckResult.VALID;
        }

        enum CheckResult
        {
            API_DEBUG,
            INVALID_USERAGENT,
            INVALID_HASH,
            VALID,
            FROM_ZJB
        }
    }


    public class AcceptedClients
    {
        [XmlElement("Client")]
        public List<Client> Clients { get; set; }
    }

    public class Client
    {
        [XmlAttribute]
        public string UserAgent { get; set; }
        [XmlAttribute]
        public string Salt { get; set; }
        [XmlAttribute]
        public string AppName { get; set; }
        [XmlAttribute]
        public string OS { get; set; }
        [XmlAttribute]
        public string Version { get; set; }
        [XmlAttribute]
        public int InternalVer { get; set; }
        [XmlAttribute]
        public int Update { get; set; }
        [XmlAttribute]
        public string UpdateUrl { get; set; }
        [XmlAttribute]
        public string ForceVer { get; set; }
        [XmlAttribute]
        public string PublishDate { get; set; }
        [XmlAttribute]
        public string ChangeLog { get; set; }
        [XmlAttribute]
        public string FileName { get; set; }
        [XmlAttribute]
        public string FileSize { get; set; }
        
        [XmlAttribute]
        public string UpdateVersion { get; set; }
    }
}