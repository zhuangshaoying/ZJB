using System;
using System.Data;
using System.Data.Common;
using System.Text;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Data;
using ZJB.Core.Logging;
using ZJB.Core.Utilities;
using System.Diagnostics;

namespace ZJB.Core.Data
{
    public abstract class BaseData
    {
        private static readonly string DataBaseTimeoutKey = "DataBaseTimeout";

        private Logger logger;

        private int timeout = 30;

        private Database database;

        protected BaseData()
            : this(null)
        { }

        protected BaseData(string connectionStringName)
        {
            logger = new Logger(GetType());

            int? dbTimeout = ConfigUtility.GetNullableIntValue(DataBaseTimeoutKey);

            if (dbTimeout.HasValue)
            {
                timeout = dbTimeout.Value;
            }

            if (string.IsNullOrEmpty(connectionStringName))
                database = DatabaseFactory.CreateDatabase();
            else
                database = DatabaseFactory.CreateDatabase(connectionStringName);
        }

        protected BaseData(string connectionStringName, IConfigurationSource configurationSource)
        {
            logger = new Logger(GetType());

            int? dbTimeout = ConfigUtility.GetNullableIntValue(DataBaseTimeoutKey);

            if (dbTimeout.HasValue)
            {
                timeout = dbTimeout.Value;
            }

            DatabaseProviderFactory databaseProviderFactory = new DatabaseProviderFactory(configurationSource); 

            if (string.IsNullOrEmpty(connectionStringName))
                database = databaseProviderFactory.CreateDefault();
            else
                database = databaseProviderFactory.Create(connectionStringName);
        }

        protected DbCommand GetStoredProcCommand(string spName)
        {
            return database.GetStoredProcCommand(spName);
        }

        protected DbCommand GetSqlStringCommand(string query)
        {
            return database.GetSqlStringCommand(query);
        }

        protected void AddInParameter(DbCommand dbCommand, string name, DbType dbType, object value)
        {
            database.AddInParameter(dbCommand, name, dbType, value);
        }

        protected void AddOutParameter(DbCommand dbCommand, string name, DbType dbType, int size)
        {
            if (!name.StartsWith("@"))
                name = "@" + name;

            database.AddOutParameter(dbCommand, name, dbType, size);
        }


        protected void AddReturnParameter(DbCommand dbCommand, string name, DbType dbType)
        {
            database.AddParameter(dbCommand, name, dbType, ParameterDirection.ReturnValue, string.Empty, DataRowVersion.Default, null);
        }

        protected virtual int ExecuteNonQuery(DbCommand dbCommand)
        {
            int result = 0;

            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            try
            {
                dbCommand.CommandTimeout = timeout;
                logger.Debug("Command: " + dbCommand.CommandText + "; Parameters: " + GetParams(dbCommand));
                result = database.ExecuteNonQuery(dbCommand);
                return result;
            }
            finally
            {
                stopwatch.Stop();
                logger.Debug(string.Format("ExecuteNonQuery Complete. Result = {0}, cost = {1} ms.", result, stopwatch.ElapsedMilliseconds));
            }
        }

        protected virtual DataSet ExecuteDataSet(DbCommand dbCommand)
        {
            DataSet dataSet = null;

            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            try
            {
                dbCommand.CommandTimeout = timeout;
                logger.Debug("Command: " + dbCommand.CommandText + "; Parameters: " + GetParams(dbCommand));
                dataSet = database.ExecuteDataSet(dbCommand);
                return dataSet;
            }
            finally
            {
                stopwatch.Stop();
                logger.Debug(string.Format("ExecuteDataSet Complete. Result = {0}, cost = {1} ms.",
                                           GetDataSetInfo(dataSet), stopwatch.ElapsedMilliseconds));
            }
        }

        private string GetDataSetInfo(DataSet dataSet)
        {
            StringBuilder buffer = new StringBuilder();

            if (dataSet == null)
                return string.Empty;

            buffer.AppendFormat("{{Table count={0}.", dataSet.Tables.Count);
            foreach (DataTable dataTable in dataSet.Tables)
            {
                buffer.AppendFormat("{{Table name={0}, row count={1}.}}", dataTable.TableName, dataTable.Rows.Count);

            }
            buffer.Append("}");

            return buffer.ToString();
        }


        protected virtual object ExecuteScalar(DbCommand dbCommand)
        {
            object result = null; 

            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            try
            {
                dbCommand.CommandTimeout = timeout;
                logger.Debug("Command: " + dbCommand.CommandText + "; Parameters: " + GetParams(dbCommand));
                result = database.ExecuteScalar(dbCommand);
                return result;
            }
            finally
            {
                stopwatch.Stop();
                logger.Debug(string.Format("ExecuteScalar Complete. Result = {0}, cost = {1} ms.",
                                           result ?? "NULL", stopwatch.ElapsedMilliseconds));
            }
        }


        protected string GetParams(DbCommand dbCommand)
        {
            StringBuilder buffer = new StringBuilder();
            try
            {
                DbParameterCollection parameters = dbCommand.Parameters;
                foreach (DbParameter dbParameter in parameters)
                {
                    buffer.Append(GetParameterExpression(dbParameter)).Append(",");
                }
                if (buffer.Length > 0)
                    buffer.Remove(buffer.Length - 1, 1);
            }
            catch (Exception exception)
            {
                logger.Error("Can't get parametes.", exception);
            }

            return buffer.ToString();
        }

        private string GetParameterExpression(DbParameter dbParameter)
        {
            string expression;

            if (dbParameter.Direction == ParameterDirection.Input || dbParameter.Direction == ParameterDirection.InputOutput)
            {
                switch (dbParameter.DbType)
                {
                    case DbType.Currency:
                    case DbType.Decimal:
                    case DbType.Double:
                    case DbType.Int16:
                    case DbType.Int32:
                    case DbType.Int64:
                    case DbType.Single:
                    case DbType.UInt16:
                    case DbType.UInt32:
                    case DbType.UInt64:
                    case DbType.VarNumeric:
                        expression = string.Format("{0}={1}", dbParameter.ParameterName, dbParameter.Value);
                        break;
                    default:
                        expression = string.Format("{0}='{1}'", dbParameter.ParameterName, dbParameter.Value);
                        break;
                }
            }
            else
            {
                expression = string.Format("out {0}", dbParameter.ParameterName);
            }

            return expression;
        }

        public static object GetParameterValue(object value)
        {
            if (value == null)
            {
                return DBNull.Value;
            }
            if (value is string && string.IsNullOrEmpty((string)value))
            {
                return DBNull.Value;
            }
            if (value is bool)
            {
                return ((bool)value) ? "Y" : "N";
            }
           
            return value;
        }

        protected static T GetOutputParameter<T>(DbCommand command, string parameterName)
        {
            if (!parameterName.StartsWith("@"))
                parameterName = "@" + parameterName;

            object value = command.Parameters[parameterName].Value;
            if (value == DBNull.Value)
            {
                return default(T);
            }
            return (T)value;
        }

        protected static T To<T>(DataRow dataRow, string columnName)
        {
            if (dataRow.IsNull(columnName))
            {
                return default(T);
            }
            object value = dataRow[columnName];

            if (typeof(T) == typeof(string) && value == null)
                return (T)Convert.ChangeType(string.Empty, typeof(T));

            if (typeof(T) == typeof(char) || typeof(T) == typeof(char?))
            {
                value = ((string)value)[0];
                return (T)Convert.ChangeType(value, typeof(T));
            }

            if (value is T)
            {
                return (T)value;
            }

            if ((value is int || value is short || value is char || value is byte) && typeof(T).IsEnum)
            {
                return (T)Convert.ChangeType(value, Enum.GetUnderlyingType(typeof(T)));
            }

            if (typeof (T).IsGenericType && typeof (T).GetGenericTypeDefinition() == typeof (Nullable<>))
            {
                return (T) Convert.ChangeType(value, typeof (T).GetGenericArguments()[0]);
            }

            return (T)Convert.ChangeType(value, typeof(T));
        }
    }
}
