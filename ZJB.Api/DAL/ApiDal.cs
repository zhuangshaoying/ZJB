using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Common;
using System.Data;
using System.Text;
using log4net;
using ZJB.Api.BLL;
using ZJB.Api.Entity;
using ZJB.Api.Models;
using ZJB.Core.Utilities;
using ZJB.Web.Utilities;

namespace ZJB.Api.DAL
{
    public class ApiDal : BaseDal
    {
        public ApiDal()
            : base("WX")
        { }
        private ILog logger = LogManager.GetLogger("ApiDal");
        private NCBaseRule ncBase = new NCBaseRule();

        public virtual int AddApiLog(ApiLogModel log)
        {
            DbCommand cmd = GetStoredProcCommand("P_Client_AddApiLog");
            AddInParameter(cmd, "Hash", DbType.String, GetParameterValue(log.Hash));
            AddInParameter(cmd, "Host", DbType.String, GetParameterValue(log.Host));
            AddInParameter(cmd, "ValidationResult", DbType.String, GetParameterValue(log.ValidationResult));
            AddInParameter(cmd, "PathAndQuery", DbType.String, GetParameterValue(log.PathAndQuery));
            AddInParameter(cmd, "Server", DbType.String, GetParameterValue(log.Server));
            AddInParameter(cmd, "Token", DbType.String, GetParameterValue(log.Token));
            AddInParameter(cmd, "UserAgent", DbType.String, GetParameterValue(log.UserAgent));
            AddInParameter(cmd, "Controller", DbType.String, GetParameterValue(log.Controller));
            AddInParameter(cmd, "Action", DbType.String, GetParameterValue(log.Action));
            AddInParameter(cmd, "IpAddress", DbType.String, GetParameterValue(log.IpAddress));
            AddInParameter(cmd, "IMEI", DbType.String, GetParameterValue(log.IMEI));
            AddInParameter(cmd, "Device", DbType.String, GetParameterValue(log.Device));
            AddInParameter(cmd, "OsVersion", DbType.String, GetParameterValue(log.OsVersion));
            AddInParameter(cmd, "MacAddress", DbType.String, GetParameterValue(log.MacAddress));
            AddOutParameter(cmd, "ApiLogId", DbType.Int32, 4);
            ExecuteNonQuery(cmd);
            return GetOutputParameter<Int32>(cmd, "@ApiLogId");
        }

    }
}