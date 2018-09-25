using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;
using log4net;
using ZJB.Api.BLL;
using ZJB.Api.Models;

namespace ZJB.Api.DAL
{
    public class ActionLogDal:BaseDal
    {
        public ActionLogDal()
            : base("WX")
        { }
        private ILog logger = LogManager.GetLogger("DynamicDal");
        private NCBaseRule ncBase = new NCBaseRule();
        #region 获取访问日志排行
        public List<ControllerActionMapModel> ActionLogTopStat(ActionLogTopStatReq parame,ref int totalSize)
        {
            DbCommand cmd = GetStoredProcCommand("P_ActionLog_StateList");
            AddInParameter(cmd, "@pi", DbType.Int32, parame.pi);
            AddInParameter(cmd, "@ps", DbType.Int32, parame.ps);
            AddInParameter(cmd, "@userId", DbType.Int32, parame.userId);
            AddInParameter(cmd, "@keyword", DbType.String, parame.keyword);
            AddOutParameter(cmd, "@totalSize", DbType.Int32, 4);
            DataSet ds = ExecuteDataSet(cmd);
            int.TryParse(cmd.Parameters["@totalSize"].Value.ToString(), out totalSize);
            List<ControllerActionMapModel> topList = new List<ControllerActionMapModel>();
            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                topList = BuildToModelList(ds.Tables[0].Select());
            }
            return topList;
        }
        #endregion
        #region 根据controller和action获取每日访问次数
        public List<StatModel> GetActionLog_StatByFunction(StatReq parame)
        {
            DbCommand cmd = GetStoredProcCommand("P_ActionLog_StatByFunction");
            AddInParameter(cmd, "@ps", DbType.Int32, parame.ps);
            AddInParameter(cmd, "@Controller", DbType.String, parame._Controller);
            AddInParameter(cmd, "@Action", DbType.String, parame._Action);
            AddInParameter(cmd, "@UserId", DbType.Int32, parame.UserId);
            AddInParameter(cmd, "@currentTime", DbType.Date, parame.currentTime);
            DataSet ds = ExecuteDataSet(cmd);
            List<StatModel> statList = new List<StatModel>();
            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    statList.Add(new StatModel()
                    {
                        Count = dr.Table.Columns.Contains("Counts") ? To<Int32>(dr, "Counts") : 0,
                        Time = dr.Table.Columns.Contains("Times") ? To<DateTime>(dr, "Times") : DateTime.Now
                    });
                }
            }
            return statList;
        }
        #endregion
        #region  私有方法
        private List<ControllerActionMapModel> BuildToModelList(IEnumerable<DataRow> rows)
        {
            return (from row in rows select BuildToModel(row)).ToList();
        }
        private ControllerActionMapModel BuildToModel(DataRow dr)
        {
            return new ControllerActionMapModel()
            {
                 AccessCount=dr.Table.Columns.Contains("AccessCount")?To<Int32>(dr,"AccessCount"):0,
                 Action = dr.Table.Columns.Contains("Action") ? To<string>(dr, "Action") : "",
                 Controller = dr.Table.Columns.Contains("Controller") ? To<string>(dr, "Controller") : "",
                 FunctionName = dr.Table.Columns.Contains("FunctionName") ? To<string>(dr, "FunctionName") : "",
                 Status = dr.Table.Columns.Contains("Status") ? To<Int32>(dr, "Status") : 0,
            };
        }
        #endregion
        #region Ip排行
       /// <summary>
       /// 
       /// </summary>
       /// <param name="parame"></param>
       /// <param name="totalSize"></param>
       /// <returns></returns>
        public List<StatModel> GetIpStatList(IpStatListReq parame, ref int totalSize)
        {
            DbCommand cmd = GetStoredProcCommand("P_ActionLog_IpStatList");
            AddInParameter(cmd, "@pi", DbType.Int32, parame.pi);
            AddInParameter(cmd, "@ps", DbType.Int32, parame.ps);
            AddInParameter(cmd, "@beginHour", DbType.Int32, parame.beginHour);
            AddInParameter(cmd, "@endHour", DbType.Int32, parame.endHour);
            AddInParameter(cmd, "@keyword", DbType.String, parame.keyword);
            AddOutParameter(cmd, "@totalSize", DbType.Int32, totalSize);
            DataSet ds = ExecuteDataSet(cmd);
            int.TryParse(cmd.Parameters["@totalSize"].Value.ToString(), out totalSize);
            List<StatModel> statList = new List<StatModel>();
            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    statList.Add(new StatModel() {
                        StatName = To<string>(dr, "IpAddress"),
                        Count = To<Int32>(dr, "Counts")
                    });
                }
            }
            return statList;
        }
        #endregion
        
    }
}
