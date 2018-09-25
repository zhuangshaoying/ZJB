using System;
using System.IO;
using log4net;

namespace ZJB.Core.Logging
{
    public class Logger
    {
        private ILog logger;

        static Logger()
        {
            string config = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "log4net.xml");
            log4net.Config.XmlConfigurator.Configure(new FileInfo(config));
        }

        public Logger(string name)
        {
            logger = LogManager.GetLogger(name);
        }

        public Logger(Type type)
        {
            logger = LogManager.GetLogger(type);
        }

        public void Debug(string message)
        {
            logger.Debug(message);
        }

        public void Error(string message, Exception exception)
        {
            logger.Error(message, exception);
        }
    }
}
