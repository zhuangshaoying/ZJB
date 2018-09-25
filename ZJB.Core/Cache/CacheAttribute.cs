using System;
using Microsoft.Practices.Unity.InterceptionExtension;
using ZJB.Core.Injection;

namespace ZJB.Core.Cache
{
    [AttributeUsage(AttributeTargets.Method)]
    public class CacheAttribute : HandlerAttribute
    {
        public CacheAttribute(Type cacheHandlerType, string key)
        {
            if (!typeof(CacheHandlerBase).IsAssignableFrom(cacheHandlerType))
            {
                throw new CacheAttributeException(string.Format("{0} is not subclass of {1}", cacheHandlerType, typeof(CacheHandlerBase)));
            }
            this.cacheHandlerType = cacheHandlerType;
            this.key = key;
        }

        private string key;
        public string Key
        {
            get { return key; }
        }


        private Type cacheHandlerType;
        public Type CacheHandlerType 
        { 
            get
            {
                return cacheHandlerType;
            }
        }


        public override ICallHandler CreateHandler(Microsoft.Practices.Unity.IUnityContainer container)
        {
            Type cacheGroupType = cacheHandlerType.GetGenericArguments()[0];

            string id = string.Format("{0}|{1}", cacheHandlerType.Name, Key);

            CacheHandlerBase cacheHandler;

            if (Container.Instance.IsRegistered<CacheHandlerBase>(id))
            {
                cacheHandler = Container.Instance.Resolve<CacheHandlerBase>(id);
            }
            else
            {
                cacheHandler = (CacheHandlerBase)Activator.CreateInstance(cacheHandlerType);

                cacheHandler.Key = Key;

                cacheHandler.Group = cacheGroupType.Name;

                CacheGroupBase cacheGroup = (CacheGroupBase)Container.Instance.Resolve(cacheGroupType);

                cacheHandler.ExpirationSeconds = cacheGroup.ExpirationSeconds;

                Container.Instance.RegisterInstance<CacheHandlerBase>(cacheHandler, id);
            }

            return cacheHandler;
        }

    }

    public class CacheAttributeException : Exception
    {
        public CacheAttributeException(string message)
            : base(message)
        {

        }
    }
}
