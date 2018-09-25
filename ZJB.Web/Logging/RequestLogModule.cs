using System;
using System.Web;
using ZJB.Core.Injection;
using ZJB.Core.Logging;
using ZJB.Core.Utilities;

namespace ZJB.Web.Logging
{
    public class RequestLogModule : IHttpModule
    {
        private RequestLog requestLog
        {
            get
            {
                return RequestLog.GetRequestLog();
            }
        }

        private RequestLogSaver RequestLogSaver
        {
            get
            {
                return Container.Instance.Resolve<RequestLogSaver>();
            }
        }

        private const string MaxExecutionTime = "MaxExecutionTime";

        public void Dispose()
        {

        }

        public void Init(HttpApplication application)
        {
            application.BeginRequest += BeginRequest;
            application.EndRequest += EndRequest;
            application.Error += Error;
        }


        void BeginRequest(object sender, EventArgs e)
        {
            HttpApplication application = sender as HttpApplication;

            if (application != null)
            {
                requestLog.Server = Environment.MachineName;
                requestLog.Headers = application.Context.Request.Headers;
                requestLog.Form = application.Context.Request.Form;
                requestLog.QueryString = application.Context.Request.QueryString;
                requestLog.Cookies = application.Context.Request.Cookies;
                requestLog.HttpMethod = application.Context.Request.HttpMethod;
                requestLog.UserHostName = application.Context.Request.UserHostName;
                requestLog.RawUrl = application.Context.Request.RawUrl;
                requestLog.Path = application.Context.Request.Path;
                requestLog.UserHostAddress = application.Context.Request.UserHostAddress;
                requestLog.IsAuthenticated = application.Context.Request.IsAuthenticated;
                requestLog.BeginTime = DateTime.Now;

                requestLog.Host = application.Context.Request.Url.Host;
                requestLog.Query = application.Context.Request.Url.Query;
            }
        }

        void EndRequest(object sender, EventArgs e)
        {
            HttpApplication application = sender as HttpApplication;

            if (application != null)
            {
                if(requestLog.ExceptionMessage == null && ForceLog(application.Request))
                {
                   requestLog.ExceptionMessage = new ExceptionMessage(new ForceLogException());
                }

                requestLog.EndTime = DateTime.Now;
                requestLog.ExecutionTime = (int)requestLog.EndTime.Subtract(requestLog.BeginTime).TotalMilliseconds;

                if (requestLog.ExceptionMessage != null
                    || ExceedMaxExecutionTime(requestLog))
                {
                    DateTime now = DateTime.Now;
                    requestLog.LogPath = string.Format(@"\{0}\{1}\{2}\{3}.xml", now.Year, now.Month, now.Day, Guid.NewGuid());


                    RequestLogSaver.Save(requestLog);
                }
            }
        }

        void Error(object sender, EventArgs e)
        {
            HttpApplication application = sender as HttpApplication;

            try
            {
                if (application != null)
                {
                    requestLog.EndTime = DateTime.Now;
                    requestLog.ExecutionTime = (int)requestLog.EndTime.Subtract(requestLog.BeginTime).TotalMilliseconds;

                    DateTime now = DateTime.Now;
                    requestLog.LogPath = string.Format(@"\{0}\{1}\{2}\{3}.xml", now.Year, now.Month, now.Day, Guid.NewGuid());

                    Exception exception = application.Server.GetLastError();

                    if (exception is HttpUnhandledException && exception.InnerException != null)
                        exception = exception.InnerException;

                    requestLog.ExceptionMessage = new ExceptionMessage(exception);

                    RequestLogSaver.Save(requestLog);

                    requestLog.ExceptionMessage = null;
                }
            }
            catch
            {
                //do nothing
            }
        }


        private bool ExceedMaxExecutionTime(RequestLog requestLog)
        {
            int? maxExecutionTime = ConfigUtility.GetNullableIntValue(MaxExecutionTime);

            if (maxExecutionTime.HasValue)
            {
                return requestLog.ExecutionTime > maxExecutionTime.Value;
            }

            return false;
        }

        private bool ForceLog(HttpRequest request)
        {
            HttpCookie cookie = request.Cookies["ForceLog"];
            if (cookie == null) return false;

            DateTime expire;

            if (DateTime.TryParse(cookie.Value, out expire))
                return expire > DateTime.Now;

            return false;

        }

    }

    public class ForceLogException : Exception
    {
    }
}
