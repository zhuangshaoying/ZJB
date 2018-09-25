using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ZJB.Api.Models;
using System.Data.Common;
using System.Data;
using ZJB.Api.Entity;
namespace ZJB.Api.DAL
{
    public class UserTaskLogDal:BaseDal
    {
        public UserTaskLogDal()
            : base("WX")
        { }
        #region 增加任务记录
        /// <summary>
        /// 完成任务-增加任务记录
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="taskId"></param>
        /// <returns></returns>
        public virtual int UserTaskLogAdd(int userId,int taskId,string Note)
        {
            DbCommand cmd = GetStoredProcCommand("P_UserTaskLog_Add");
            AddInParameter(cmd, "@UserId", DbType.Int32, userId);
            AddInParameter(cmd, "@TaskId", DbType.Int32, taskId);
            AddInParameter(cmd, "@Note", DbType.String, Note);
            AddReturnParameter(cmd, "@ReturnValue", DbType.Int32);
            ExecuteNonQuery(cmd);
            int result = 0;
            int.TryParse(cmd.Parameters["@ReturnValue"].Value.ToString(),out result);
            return result;
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
            DbCommand cmd = GetStoredProcCommand("P_UserTaskLog_Draw");
            AddInParameter(cmd, "@UserId", DbType.Int32, userId);
            AddInParameter(cmd, "@TaskId", DbType.Int32, taskId);
            AddReturnParameter(cmd, "@ReturnValue", DbType.Int32);
            ExecuteNonQuery(cmd);
            int result = 0;
            int.TryParse(cmd.Parameters["@ReturnValue"].Value.ToString(), out result);
            return result;
        }
        #endregion
        #region 获取未完成的任务(新手任务)
        public List<int> GetCompleteTask_First(int userid)
        {
            DbCommand cmd = GetStoredProcCommand("P_UserTaskLog_CompleteTask_First");
            AddInParameter(cmd, "@UserId", DbType.Int32, userid);
            DataSet ds= ExecuteDataSet(cmd);
            List<int> taskList = new List<int>();
            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    taskList.Add(To<int>(dr, "TaskId"));
                }
            }
            return taskList;
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
            DbCommand cmd = GetStoredProcCommand("P_UserTask_EveryDay_NoComplete");
            AddInParameter(cmd, "@UserId", DbType.Int32, userId);
            DataSet ds = ExecuteDataSet(cmd);
            List<UserTaskModel> taskList = new List<UserTaskModel>();
            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
               taskList= BuildToTaskModelList(ds.Tables[0].Select());
            }
            return taskList;

        }
        /// <summary>
        /// 获取今日任务-已完成
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public List<UserTaskModel> GetCompleteTask(int userId)
        {
            DbCommand cmd = GetStoredProcCommand("P_UserTask_EveryDay_Complete");
            AddInParameter(cmd, "@UserId", DbType.Int32, userId);
            DataSet ds = ExecuteDataSet(cmd);
            List<UserTaskModel> taskList = new List<UserTaskModel>();
            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
               taskList= BuildToTaskModelList(ds.Tables[0].Select());
            }
            return taskList;
            
        }
        #endregion

        #region 今日任务 --所有 包括 完成和未完成
        public List<UserTaskModel> GetUserTask_EveryDay(int userId)
        {
            DbCommand cmd = GetStoredProcCommand("P_UserTask_EveryDay");
            AddInParameter(cmd, "@UserId", DbType.Int32, userId);
            DataSet ds = ExecuteDataSet(cmd);
            List<UserTaskModel> userTaskList = new List<UserTaskModel>();
            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                userTaskList = BuildToTaskModelList(ds.Tables[0].Select());
            }
            return userTaskList;
        }
        #endregion
        #region  任务情况统计
        public UserTaskStat Get_UserTask_Stat(int userId)
        {
            DbCommand cmd = GetStoredProcCommand("P_UserTask_Stat");
            AddInParameter(cmd, "@userId", DbType.Int32, userId);
            DataSet ds = ExecuteDataSet(cmd);
            UserTaskStat userTaskStat = new UserTaskStat();
            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                userTaskStat.CompleteCount=To<int>(ds.Tables[0].Rows[0],"CompleteCount");
                userTaskStat.PointsCount = To<int>(ds.Tables[0].Rows[0], "PointsCount");
                userTaskStat.UserPoints = To<int>(ds.Tables[0].Rows[0], "UserPoints");
            }
            return userTaskStat;
        }
        #endregion
        #region 私有方法
        private List<UserTaskModel> BuildToTaskModelList(IEnumerable<DataRow> rows)
        {
            return (from row in rows select BuildToTaskModel(row)).ToList();
        }
        private UserTaskModel BuildToTaskModel(DataRow dr)
        {
            return new UserTaskModel()
            {
                AddTime = dr.Table.Columns.Contains("AddTime") ? To<DateTime>(dr, "AddTime") : new DateTime(1970, 1, 1),
                BeginTime = dr.Table.Columns.Contains("BeginTime") ? To<DateTime>(dr, "BeginTime") : new DateTime(1970, 1, 1),
                EndTime = dr.Table.Columns.Contains("EndTime") ? To<DateTime>(dr, "EndTime") : new DateTime(1970, 1, 1),
                Points = dr.Table.Columns.Contains("Points") ? To<Int32>(dr, "Points") : 0,
                State = dr.Table.Columns.Contains("State") ? To<Int32>(dr, "State") : 0,
                TaskDescription = dr.Table.Columns.Contains("TaskDescription") ? To<string>(dr, "TaskDescription") : "",
                TaskId = dr.Table.Columns.Contains("TaskId") ? To<Int32>(dr, "TaskId") : 0,
                TaskName = dr.Table.Columns.Contains("TaskName") ? To<string>(dr, "TaskName") : "",
                TaskStatus = dr.Table.Columns.Contains("TaskStatus") ? To<Int32>(dr, "TaskStatus") : -1,
                Type = dr.Table.Columns.Contains("Type") ? To<Int32>(dr, "Type") : 0,
                SpecialTaskCount = dr.Table.Columns.Contains("SpecialTaskCount") ? To<Int32>(dr, "SpecialTaskCount") : 0,
                TaskUrl = dr.Table.Columns.Contains("TaskUrl") ? To<string>(dr, "TaskUrl") : "",
                AppUrl = dr.Table.Columns.Contains("AppUrl") ? To<string>(dr, "AppUrl") : ""
            };
        }
        #endregion
    }
}
