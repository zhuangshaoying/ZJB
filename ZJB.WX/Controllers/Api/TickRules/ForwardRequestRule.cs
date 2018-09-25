using System;
using System.Text;
using ZJB.Api.Models;
using ZJB.WX.Models;
using Action = ZJB.WX.Models.Action;

namespace ZJB.WX.Controllers.Api.TickRules
{
    public class ForwardRequestRule : ITickRule
    {
        public TickResponse GetResponse(Credential credential, ClientInfo clientInfo)
        {
            
            return null;
            return new TickResponse
            {
                Action = Action.ForwardRequest,
                Data = new
                {
                    Id = "aaaaa",
                    HttpMethod = "POST",
                    Url = "http://fchezi.com/User/DoLogin",
                    Cookie = "",
                    PostData =
                        Convert.ToBase64String(
                            Encoding.UTF8.GetBytes("phone=1333333333&userPwd=asddddd&id=0")),
                    ShouldReport = true
                }
            };
        }
    }
}