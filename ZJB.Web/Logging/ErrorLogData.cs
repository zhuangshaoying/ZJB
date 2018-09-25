using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using ZJB.Core.Data;

namespace ZJB.Web.Logging
{
    public class ErrorLogData : BaseData
    {
        public ErrorLogData()
            : base("LoggingConnection")
        {
        }

        public void SaveErrorLog(ErrorLog errorLog)
        {
            DbCommand cmd = GetStoredProcCommand("SaveErrorLog");
            AddInParameter(cmd, "Host", DbType.String, GetParameterValue(errorLog.Host));
            AddInParameter(cmd, "Exception", DbType.String, GetParameterValue(errorLog.Exception));
            AddInParameter(cmd, "LogPath", DbType.String, GetParameterValue(errorLog.LogPath));
            AddInParameter(cmd, "Path", DbType.String, GetParameterValue(errorLog.Path));
            AddInParameter(cmd, "Query", DbType.String, GetParameterValue(errorLog.Query));
            AddInParameter(cmd, "Server", DbType.String, GetParameterValue(errorLog.Server));
            AddInParameter(cmd, "Notes", DbType.String, GetParameterValue(errorLog.Notes));

            ExecuteNonQuery(cmd);
        }

        public virtual List<ErrorLog> GetErrorLogs(string host, string path, string exception, DateTime? beginTime, DateTime? endTime)
        {
            DbCommand cmd = GetStoredProcCommand("GetErrorLog");

            AddInParameter(cmd, "Host", DbType.String, GetParameterValue(host));

            AddInParameter(cmd, "Path", DbType.String, GetParameterValue(path));

            AddInParameter(cmd, "Exception", DbType.String, GetParameterValue(exception));

            if (beginTime.HasValue)
                AddInParameter(cmd, "BeginTime", DbType.DateTime, beginTime.Value);

            if (endTime.HasValue)
                AddInParameter(cmd, "EndTime", DbType.DateTime, endTime.Value);

            DataSet ds = ExecuteDataSet(cmd);

            if (ds == null || ds.Tables.Count == 0 || ds.Tables[0].Rows.Count == 0)
                return new List<ErrorLog>();

            return BuildErrorLogs(ds.Tables[0]);
        }

        private static List<ErrorLog> BuildErrorLogs(DataTable dt)
        {
            List<ErrorLog> errorLogs = new List<ErrorLog>();

            foreach (DataRow row in dt.Rows)
            {
                ErrorLog log = new ErrorLog();

                log.Exception = To<string>(row, "Exception");
                log.Host = To<string>(row, "Host");
                log.Id = To<int>(row, "Id");
                log.LogPath = To<string>(row, "LogPath");
                log.LogTime = To<DateTime>(row, "LogTime");
                log.Path = To<string>(row, "Path");
                log.Query = To<string>(row, "Query");
                log.Server = To<string>(row, "Server");
                log.Notes = To<string>(row, "Notes");

                errorLogs.Add(log);
            }

            return errorLogs;
        }
    }
}
