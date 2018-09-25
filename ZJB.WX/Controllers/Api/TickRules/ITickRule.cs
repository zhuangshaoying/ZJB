using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ZJB.Api.Models;
using ZJB.WX.Models;

namespace ZJB.WX.Controllers.Api.TickRules
{
    public interface ITickRule
    {
        TickResponse GetResponse(Credential credential,ClientInfo clientInfo);
    }
}