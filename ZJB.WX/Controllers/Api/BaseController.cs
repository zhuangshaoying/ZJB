using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using ZJB.Api.BLL;
using ZJB.Api.Models;
using ZJB.WX.Common;

namespace ZJB.WX.Controllers.Api
{
    public class BaseController : ApiController
    {
      
        #region 做任务
      
        public virtual void DoTask(int uid, PointsEnum pointsEnum,out int points)
        {
            points = 0;
               UserTaskLogBll userTaskLog = new UserTaskLogBll();

               Dictionary<string, string> userTaskInfo = TaskHelper.GetEveryDayTask(uid);
            int taskKey = (int)pointsEnum;
            if (userTaskInfo != null)
            {
                if (!userTaskInfo.ContainsKey(taskKey.ToString()))//没有任务状态
                {
                    userTaskInfo = TaskHelper.GetEveryDayTask(uid, true);
                }
                if (userTaskInfo != null && userTaskInfo.ContainsKey(taskKey.ToString()) && userTaskInfo[taskKey.ToString()] == "-1")//未做过
                {
                    points=userTaskLog.UserTaskLogAdd(uid, taskKey);
                    TaskHelper.SetEveryDayTask(uid, taskKey, 0);
                }
            }
     

        }
        #endregion
    }
}
