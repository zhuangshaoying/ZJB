using ZJB.Api.Models;
using ZJB.Core.Utilities;
using ZJB.WX.Models;
using Action = ZJB.WX.Models.Action;

namespace ZJB.WX.Controllers.Api.TickRules
{
    public class NewHouseInfoRule:ITickRule
    {
        public TickResponse GetResponse(Credential credential, ClientInfo clientInfo)
        {
            long count = RedisHelper.CountQueue("HouseRemind_" + clientInfo.UserId);
            if (count == 0) return null;
            return new TickResponse
            {
                Action = Action.NewHouseInfo,
                Data = new
                {
                    Total = count
                }
            };
        }
    }
}