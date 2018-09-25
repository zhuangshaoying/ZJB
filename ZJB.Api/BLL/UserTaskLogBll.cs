using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ZJB.Api.DAL;
using ZJB.Core.Injection;
using ZJB.Api.Entity;
using ZJB.Api.Models;
namespace ZJB.Api.BLL
{
    public class UserTaskLogBll
    {

        private readonly UserTaskLogDal userTaskLogDal = Container.Instance.Resolve<UserTaskLogDal>();
        #region -增加任务记录
        /// <summary>
        /// -增加任务记录
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="taskId"></param>
        /// <returns></returns>
        public virtual int UserTaskLogAdd(int userId, int taskId,string note="")
        {
            return userTaskLogDal.UserTaskLogAdd(userId, taskId, note);
        }
        #endregion
        #region　完成任务领取奖励
        /// <summary>
        /// 
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="taskLogId">任务日志的id UserTaskLog</param>
        /// <returns></returns>
        public virtual int UserTaskLogDraw(int userId, int taskId)
        {
            return userTaskLogDal.UserTaskLogDraw(userId, taskId);
        }
        #endregion
        #region 获取未完成的任务(新手任务)
        public List<int> GetCompleteTask_First(int userid)
        {
            return userTaskLogDal.GetCompleteTask_First(userid);
        }
        #endregion
        #region 获取今日任务
        /// <summary>
        /// 获取今日任务-未完成
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public List<UserTaskModel> GetNoCompleteTask(int userId)
        {
            return userTaskLogDal.GetNoCompleteTask(userId);
        }
         /// <summary>
        /// 获取今日任务-已完成
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public List<UserTaskModel> GetCompleteTask(int userId)
        {
            return userTaskLogDal.GetCompleteTask(userId);
        }
        #endregion
        #region  任务情况统计
        public UserTaskStat Get_UserTask_Stat(int userId)
        {
            return userTaskLogDal.Get_UserTask_Stat(userId);
        }
        #endregion

        #region 今日任务 --所有 包括 完成和未完成
        public List<UserTaskModel> GetUserTask_EveryDay(int userId)
        {
            return userTaskLogDal.GetUserTask_EveryDay(userId);
        }
        #endregion
    }
}
