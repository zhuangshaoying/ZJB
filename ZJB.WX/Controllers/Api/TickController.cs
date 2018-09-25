using System.Collections.Generic;
using System.Web.Http;
using ZJB.WX.Controllers.Api.TickRules;
using ZJB.WX.Filters;
using ZJB.WX.Models;

namespace ZJB.WX.Controllers.Api
{
    public class TickController : ApiController
    {
        [HttpPost]
        [Token]
        [IgnoreValidate]
        public TickResponse FetchTask([FromBody]ClientInfo clientInfo)
        {
            var credential = this.GetCredential();

            var rules = GetRules();

            foreach (var rule in rules)
            {
                var response = rule.GetResponse(credential, clientInfo);
                if (response != null)
                    return response;
            }

            return new TickResponse {Action = Action.Nothing, Data = null};
        }

        [HttpPost]
        [Token]
        [IgnoreValidate]
        public TickResponse CheckUpdate([FromBody]ClientInfo clientInfo)
        {
            var credential = this.GetCredential();

            var update = new UpdateAppRule();

            var response = update.GetResponse(credential, clientInfo);
            if (response != null)
                return response;
            return new TickResponse { Action = Action.Nothing, Data = null };
        }

        [HttpPost]
        [Token]
        public object ReportForwardResult(string id)
        {
            byte[] data = Request.Content.ReadAsByteArrayAsync().Result;
            //TODO 
            return new {data.Length};
        }

        private IEnumerable<ITickRule> GetRules()
        {
            yield return new LoadAssemblyRule();
            yield return new ForwardRequestRule();
            yield return new UpdateAppRule();
            yield return new NewHouseInfoRule();
        }
    }

    public class ClientInfo
    {
        public string NetVersion { get; set; }
        public string OSVersion { get; set; }
        public string AppVersion { get; set; }
        public string LastHouseInfoId { get; set; }
        public string UserId { get; set; }
    }
}