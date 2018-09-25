using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ZJB.Api.DAL;
using ZJB.Core.Injection;
using ZJB.Api.Models;
namespace ZJB.Api.BLL
{
    public class ActionLogBll
    {
        private readonly ActionLogDal actionLogDal = Container.Instance.Resolve<ActionLogDal>();

        #region 获取访问日志排行
        public List<ControllerActionMapModel> ActionLogTopStat(ActionLogTopStatReq parame, ref int totalSize)
        {
            return actionLogDal.ActionLogTopStat(parame,ref totalSize);
        }
        #endregion
        #region 根据controller和action获取每日访问次数
        public List<StatModel> GetActionLog_StatByFunction(StatReq parame)
        {
            return actionLogDal.GetActionLog_StatByFunction(parame);
        }
        #endregion
        #region Ip排行
        /// <summary>
        /// 
        /// </summary>
        /// <param name="parame"></param>
        /// <param name="totalSize"></param>
        /// <returns></returns>
        public List<StatModel> GetIpStatList(IpStatListReq parame,ref int totalSize)
        {
            return actionLogDal.GetIpStatList(parame, ref totalSize);
            
        }
        #endregion

    }
}
