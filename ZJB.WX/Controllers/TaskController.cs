using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using LifeCycleLogging;
using log4net;
using ZJB.Api.Models;
using ZJB.Api.BLL;
using ZJB.WX.Common;
using ZJB.WX.Filters;

namespace ZJB.WX.Controllers
{
    [Authorization]
    [ActionLog(CheckPoints = false)]
    public class TaskController : Controller
    {
        //
        // GET: /Task/
        private UserTaskLogBll userTaskBll = new UserTaskLogBll();
        private ILog logger =  LogManager.GetLogger(string.Empty);
         
        public ActionResult Index()
        {
            PublicUserModel loginUser=this.GetLoginUser();
            UserTaskStat stat= userTaskBll.Get_UserTask_Stat(loginUser.UserID);
            ViewBag.UserName = loginUser.Name;
            ViewBag.Portrait = loginUser.Portrait;
            return View(stat);
        }
        public JsonResult GetUserTask(int type = 0)
        {
            PublicUserModel loginUser = this.GetLoginUser();
            List<UserTaskModel> taskList = new List<UserTaskModel>();
            if (type == 0)//未完成
            {
                taskList=userTaskBll.GetNoCompleteTask(loginUser.UserID);
            }
            else if (type == 1)//已完成
            {
                taskList=userTaskBll.GetCompleteTask(loginUser.UserID);
            }
            return Json(new { data=taskList},JsonRequestBehavior.AllowGet);
        }
        /// <summary>
        /// 领取任务奖励
        /// </summary>
        /// <param name="taskId"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult UserTaskDraw(int taskId)
        {
            PublicUserModel loginUser = this.GetLoginUser();
            int result= userTaskBll.UserTaskLogDraw(loginUser.UserID, taskId);
            string msg = string.Empty;
            if (result <= 0)
            {
                switch (result)
                {
                    case -1:
                        msg = "任务不存在";
                        break;
                    case -2:
                    case -3:
                        msg = "已经签到";
                        break;
                    default:
                        msg = "未知错误";
                        break;
                }
            }
            else
            {
                msg = "领取成功+" + result + "积分";
            }
            return Json(new { status = result, msg = msg });
        }
        #region 签到
        /// <summary>
        /// 签到任务
        /// </summary>
        /// <returns></returns>
        public JsonResult EveryDaySign()
        {
            PublicUserModel loginUser = this.GetLoginUser();
            int result = 0;
            string msg = string.Empty;
            int taskKey = 0;
            int timeOut = 4;//四小时时间
            DateTime nowTime=DateTime.Now;
          //  return Json(new { status = nowTime.ToString(), msg = msg });
            #region 判断时间
            if (nowTime.Hour >= 8 && nowTime.Hour < 8+timeOut)
            {
                taskKey = (int)PointsEnum.EveryDay_Sign_8;
            }
            else if (nowTime.Hour >= 15 &&nowTime.Hour < 15 + timeOut)
            {
                taskKey = (int)PointsEnum.EveryDay_Sign_16;
            }
            #endregion

            
            if (taskKey > 0)
            {
                int addResult = 0;
                Dictionary<string, string> userTaskInfo = TaskHelper.GetEveryDayTask(loginUser.UserID);
                if (userTaskInfo != null)
                {
                    if (!userTaskInfo.ContainsKey(taskKey.ToString()))//没有任务状态
                    {
                
                     //   logger.Error("1",new Exception("重写了！" + loginUser.UserID));
                        userTaskInfo = TaskHelper.GetEveryDayTask(loginUser.UserID, true);
                    }
                    if (userTaskInfo != null)
                    {
                     //    logger.Error("2",new Exception("成功了！" + loginUser.UserID + " | " + taskKey + ":" + userTaskInfo[taskKey.ToString()]));
                        if (userTaskInfo.ContainsKey(taskKey.ToString()) && userTaskInfo[taskKey.ToString()] == "-1")//未做过
                        {
                           
                            addResult = userTaskBll.UserTaskLogAdd(loginUser.UserID, taskKey);
                            TaskHelper.SetEveryDayTask(loginUser.UserID, taskKey, 0);
                        }
                        else
                        {
                        //     logger.Error("3",new Exception("失败！" + loginUser.UserID + " | " + addResult));
                      
                            addResult = -2;
                        }
                    }
                   
                }
                if (addResult > 0)
                {
                 
                    result = userTaskBll.UserTaskLogDraw(loginUser.UserID, taskKey);
                    #region 结果
                    if (result <= 0)
                    {
                        logger.Debug("result:" + result);
                        switch (result)
                        {
                            case -1:
                                msg = "任务不存在";
                                break;
                            case -2:
                            case -3:
                                msg = "今日已签到";
                                break;
                            default:
                                msg = "未知错误";
                                break;
                        }
                    }
                    else
                    {
                        msg = "成功领取+" + result + "积分";
                    }
                    #endregion
                }
                else
                {
                    #region 结果

                    switch (addResult)
                    {
                        case -1:
                            msg = "任务不存在";
                            break;
                        case -2:
                        case -3:
                            msg = "今日已签到"; logger.Debug("addResult:" + addResult);
                            break;
                        default:
                            msg = "未知错误";
                            break;
                    }
                    #endregion
                }
            }
            else
            {
                result = -1;
                msg = "领取时间未到";
            }
            return Json(new { status = result, msg = msg });

        }
        #endregion

        public JsonResult test()
        {
            string hashId = "HeziTasks_" + 1000021;
            ZJB.Core.Utilities.RedisHelper.ClearQueue(hashId);
            return Json("s", JsonRequestBehavior.AllowGet);
        }
    }
}
