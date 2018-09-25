using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Web.Mvc;
using ZJB.Api.BLL;
using ZJB.Api.Models;
using ZJB.Core.Utilities;

namespace ZJB.WX.Common
{
    public static class TaskHelper
    {

        /// <summary>
        /// 获取每日任务状态
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="refresh">是否直接读库</param>
        /// <returns></returns>
        public static Dictionary<string, string> GetEveryDayTask( int userId, bool refresh = false)
        {
            string hashId = "HeziTasks_" + userId;
            //RedisHelper.ClearQueue(hashId);
            Dictionary<string, string> userTaskListDic = refresh ? new Dictionary<string, string>() : RedisHelper.GetAllEntriesFromHash(hashId);
            if (userTaskListDic.Count <= 0)
            {
                UserTaskLogBll userTaskBll = new UserTaskLogBll();
                List<UserTaskModel> userTaskList = userTaskBll.GetUserTask_EveryDay(userId);
                if (userTaskList.IsNoNull())
                {
                    userTaskListDic = ResetEverydayTask(hashId, userTaskList);
                }
            }
            return userTaskListDic;
        }
        private static Dictionary<string, string> ResetEverydayTask(string hashId, List<UserTaskModel> userTaskList)
        {
            Dictionary<string, string> userTaskListDic = new Dictionary<string, string>();
            RedisHelper.ClearQueue(hashId);
            foreach (UserTaskModel item in userTaskList)
            {
                if (!userTaskListDic.ContainsKey(item.TaskId.ToString()))
                {
                    userTaskListDic.Add(item.TaskId.ToString(), item.TaskStatus.ToString());
                }
                RedisHelper.SetHash(hashId, item.TaskId.ToString(), item.TaskStatus.ToString());
            }
            //DateTime expireTime = DateTime.Now.ToUniversalTime().AddMinutes(1);
            DateTime expireTime = DateTime.Today.ToUniversalTime().AddDays(1);
            RedisHelper.SetHashExpire(hashId, expireTime);
            return userTaskListDic;
        }

        /// <summary>
        /// 更改任务状态
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="TaskStatus">0是做过 -1没做过</param>
        public static void SetEveryDayTask( int userId, int taskId, int TaskStatus)
        {
            string hashId = "HeziTasks_" + userId;
            RedisHelper.SetHash(hashId, taskId.ToString(), TaskStatus.ToString());
            DateTime expireTime = DateTime.Today.ToUniversalTime().AddDays(1);
            RedisHelper.SetHashExpire(hashId, expireTime);
        }

        
    }
}