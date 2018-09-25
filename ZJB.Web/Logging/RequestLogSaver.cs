using System.Configuration;
using System.IO;
using System.Text;
using ZJB.Core.Injection;
using ZJB.Core.Utilities;
using System;

namespace ZJB.Web.Logging
{
    public class RequestLogSaver
    {
        protected ErrorLogData ErrorLogData
        {
            get
            {
                return Container.Instance.Resolve<ErrorLogData>();
            }
        }

        private string logFolder
        {
            get
            {
                string path = ConfigurationManager.AppSettings["LogPath"];

                if (path.StartsWith(@"\"))
                {
                    path = AppDomain.CurrentDomain.BaseDirectory + path;
                }
                return path;
            }
        }

        public virtual void Save(RequestLog log)
        {
            SaveFile(log);
            SaveDatabase(log);
        }

        private void SaveDatabase(RequestLog log)
        {
            string exception = string.Empty;
            string notes = string.Empty;

            if(log.ExceptionMessage!=null)
            {
                exception = log.ExceptionMessage.Name;
                notes = log.ExceptionMessage.Message;
            }
            else
            {
                notes = log.ExecutionTime.ToString();
            }

            ErrorLog errorLog = new ErrorLog
                                    {
                                        Exception = exception,
                                        Host = log.Host,
                                        LogPath = log.LogPath,
                                        Notes = notes,
                                        Path = log.Path,
                                        Query = log.Query,
                                        Server = log.Server
                                    };

            ErrorLogData.SaveErrorLog(errorLog);
        }

        private void SaveFile(RequestLog log)
        {
            Encoding encoding = Encoding.UTF8;
            string xmlString = XmlUtility.Serialize(log, encoding);

            string path = Path.GetDirectoryName(log.LogPath);

            CreateFolder(logFolder + path);

            using (StreamWriter writer = new StreamWriter(logFolder + log.LogPath, false, encoding))
            {
                writer.Write(xmlString);
            }
        }

        private void CreateFolder(string folder)
        {
            if (!Directory.Exists(folder))
            {
                Directory.CreateDirectory(folder);
            }
        }
    }
}
