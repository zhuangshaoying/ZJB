using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using ZJB.Api.Models;
using ZJB.Core.Utilities;
using ZJB.WX.Filters;
using ZJB.WX.Models;
using Action = ZJB.WX.Models.Action;

namespace ZJB.WX.Controllers.Api.TickRules
{
    public class UpdateAppRule : ITickRule
    {
        private  AcceptedClients clients;

        public TickResponse GetResponse(Credential credential, ClientInfo clientInfo)
        {
            try
            {
                var request = HttpContext.Current.Request;
                if (clients == null || clients.Clients == null || clients.Clients.Count == 0)
                {
                    string config = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"Config/AcceptedClients.config");
                    using (var reader = new StreamReader(config, Encoding.UTF8))
                    {
                        string xml = reader.ReadToEnd();
                        clients = XmlUtility.Deserialize<AcceptedClients>(xml);
                    }
                }
                string userAgent = request.UserAgent;
                if (string.IsNullOrEmpty(request.UserAgent))
                {
                    userAgent = request.Headers["User-Agent"];
                }
                var client = clients.Clients.FirstOrDefault(
                    x =>
                        string.Equals(x.UserAgent, userAgent,
                            StringComparison.OrdinalIgnoreCase));

                if (client.UpdateVersion == clientInfo.AppVersion)
                {
                    return null;
                }
                //先做强制升级的
                return new TickResponse
                {
                    Action = Action.UpdateApp,
                    Data = new
                    {
                        Version = client.UpdateVersion,
                        Message = "强制升级",
                        Force = true,
                        Url = "http://fchezi.com/download/update.zip"
                    }
                };
            }
            catch (Exception e)
            {
                return null;
            }
        }
    }
}