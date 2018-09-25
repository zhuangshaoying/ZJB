using System;
using System.Text;
using Microsoft.Practices.Unity.InterceptionExtension;
using ZJB.Core.Logging;

namespace ZJB.Core.Cache
{
    public abstract class CacheHandlerBase : ICallHandler
    {
        Logger logger = new Logger(typeof(CacheHandlerBase));

        public IMethodReturn Invoke(IMethodInvocation input, GetNextHandlerDelegate getNext)
        {
            string paramString = GetParameterString(input.Arguments);

            try
            {
                if (Contains(paramString))
                {
                    logger.Debug(string.Format("命中{0}缓存，[Group:{1}][Key:{2}][Params:{3}]", this.GetType(), Group, Key, paramString));
                    return input.CreateMethodReturn(GetItem(paramString), null);
                }

                IMethodReturn methodReturn = getNext()(input, getNext);
                if (methodReturn.ReturnValue != null)
                {
                    SaveItem(paramString, methodReturn.ReturnValue);
                    logger.Debug(string.Format("保存至{0}缓存，[Group:{1}][Key:{2}][Params:{3}]", this.GetType(), Group, Key, paramString));
                }
                return methodReturn;

            }
            catch (Exception ex)
            {
                string message = string.Format("{0}异常，[Group:{1}][Key:{2}][Params:{3}]", this.GetType(), Group, Key,
                                               paramString);
                logger.Error(message, new CacheException(message, ex));
                return getNext()(input, getNext);
            }
        }

        private static string GetParameterString(IParameterCollection parameterCollection)
        {
            StringBuilder buffer = new StringBuilder();

            for (int i = 0; i < parameterCollection.Count; i++)
            {
                buffer.AppendFormat("[{0}:{1}]", parameterCollection.ParameterName(i), parameterCollection[i]);
            }

            return buffer.ToString();
        }

        protected abstract object GetItem(string paramString);
        protected abstract void SaveItem(string paramString, object item);
        protected abstract bool Contains(string paramString);

        public string Key { get; set; }

        public string Group { get; set; }

        public int Order
        {
            get;
            set;
        }

        public int ExpirationSeconds { get; set; }
    }


    public class CacheException : Exception
    {
        public CacheException(string message)
            : base(message)
        {
        }
        public CacheException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}
