using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Web;
using System.Xml.Serialization;
using ZJB.Core.Logging;

namespace ZJB.Web.Logging
{
    [Serializable]
    public class RequestLog
    {
        public static readonly string RequestLogKey = "REQUEST_LOG_KEY";

        private static readonly string ViewStateException = "System.Web.UI.ViewStateException";

        private Dictionary<string, string> ignoreKeys
            = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase) { 
                { "__VIEWSTATE", string.Empty },
                { "__LASTFOCUS", string.Empty },
                { "__EVENTVALIDATION", string.Empty } 
            };

        public static RequestLog GetRequestLog()
        {
            if (!HttpContext.Current.Items.Contains(RequestLogKey))
            {
                RequestLog log = new RequestLog
                {
                    LogEntrys = new List<LogEntry>()
                };
                HttpContext.Current.Items[RequestLogKey] = log;
            }

            return HttpContext.Current.Items[RequestLogKey] as RequestLog;
        }

        public string Host { get; set; }
        public string Server { get; set; }
        public string RawUrl { get; set; }
        public string Path { get; set; }
        public string HttpMethod { get; set; }
        public string UserHostName { get; set; }
        public string UserHostAddress { get; set; }
        public bool IsAuthenticated { get; set; }
        public int ExecutionTime { get; set; }

        [XmlIgnore]
        public string Query { get; set; }

        [XmlIgnore]
        public string LogPath { get; set; }

        [XmlIgnore]
        public NameValueCollection Headers { get; set; }

        private List<string> headersWrapper;

        [XmlArray("Headers")]
        [XmlArrayItem("Item")]
        public List<string> HeadersWrapper
        {
            get
            {
                if (headersWrapper == null && Headers != null)
                {
                    headersWrapper = new List<string>();
                    foreach (string key in Headers.Keys)
                    {
                        headersWrapper.Add(string.Format("{0}={1}", key, Headers[key]));
                    }
                }
                return headersWrapper;
            }
            set { headersWrapper = value; }
        }


        [XmlIgnore]
        public NameValueCollection Form { get; set; }

        private List<string> formWrapper;
        [XmlArray("Form")]
        [XmlArrayItem("Item")]
        public List<string> FormWrapper
        {
            get
            {
                if (formWrapper == null && Form != null)
                {
                    formWrapper = new List<string>();
                    foreach (string key in Form.Keys)
                    {
                        if (ShouldLogFormItem(key))
                            formWrapper.Add(string.Format("{0}={1}", key, Form[key]));
                    }
                }
                return formWrapper;
            }
            set { formWrapper = value; }
        }

        private bool ShouldLogFormItem(string key)
        {
            if(!string.IsNullOrEmpty(key))
            {
                if (ignoreKeys.ContainsKey(key))
                {
                    bool isViewStateException = false;
                    ExceptionMessage exception = ExceptionMessage;
                    while (exception!=null)
                    {
                        if (ViewStateException.Equals(exception.Name, StringComparison.OrdinalIgnoreCase))
                        {
                            isViewStateException = true;
                            break;
                        }
                        exception = exception.InnerExceptionMessage;
                    }
                    return isViewStateException;
                }

                return true;
            }
            return false;
        }

        [XmlIgnore]
        public NameValueCollection QueryString { get; set; }

        private List<string> queryStringWrapper;

        [XmlArray("QueryString")]
        [XmlArrayItem("Item")]
        public List<string> QueryStringWrapper
        {
            get
            {
                if (queryStringWrapper == null && QueryString != null)
                {
                    queryStringWrapper = new List<string>();
                    foreach (string key in QueryString.Keys)
                    {
                        queryStringWrapper.Add(string.Format("{0}={1}", key, QueryString[key]));
                    }
                }
                return queryStringWrapper;
            }
            set { queryStringWrapper = value; }
        }

        [XmlIgnore]
        public HttpCookieCollection Cookies { get; set; }

        private List<string> cookiesWrapper;

        [XmlArray("Cookies")]
        [XmlArrayItem("Item")]
        public List<string> CookiesWrapper
        {
            get
            {
                if (cookiesWrapper == null && Cookies != null)
                {
                    cookiesWrapper = new List<string>();
                    foreach (string key in Cookies.Keys)
                    {
                        HttpCookie cookie = Cookies[key];
                        cookiesWrapper.Add(string.Format("{0}={1}", key, (cookie == null ? "NULL" : cookie.Value)));
                    }
                }
                return cookiesWrapper;
            }
            set { cookiesWrapper = value; }
        }


        public DateTime BeginTime { get; set; }
        public List<LogEntry> LogEntrys { get; set; }
        public DateTime EndTime { get; set; }
        public ExceptionMessage ExceptionMessage { get; set; }
    }
}
