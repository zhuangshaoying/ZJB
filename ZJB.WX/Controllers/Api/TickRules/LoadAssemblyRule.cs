using System;
using System.IO;
using ZJB.Api.Models;
using ZJB.WX.Models;
using Action = ZJB.WX.Models.Action;

namespace ZJB.WX.Controllers.Api.TickRules
{
    public class LoadAssemblyRule : ITickRule
    {
        public TickResponse GetResponse(Credential credential, ClientInfo clientInfo)
        {
            return null;
            return new TickResponse
            {
                Action = Action.LoadAssembly,
                Data = new { AssemblyToLoad = Convert.ToBase64String(File.ReadAllBytes(@"D:\Projects\ZJB.WX2\TestDll\bin\Debug\TestDll.dll")) }
            };
        }
    }
}