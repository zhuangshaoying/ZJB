using System;
using System.Collections.Generic;
using Microsoft.Practices.EnterpriseLibrary.Caching;
using Microsoft.Practices.EnterpriseLibrary.Caching.BackingStoreImplementations;
using Microsoft.Practices.EnterpriseLibrary.Caching.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Caching.Expirations;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using ZJB.Core.Injection;

namespace ZJB.Core.Cache
{
    public class LocalCacheHandler<T> : CacheHandlerBase where T:CacheGroupBase
    {
        private Dictionary<string,ICacheManager> cacheManagers = new Dictionary<string, ICacheManager>();
        private object cacheLock = new object();

        protected override object GetItem(string paramString)
        {
            ICacheManager cacheManager = GetCacheManager();
            return cacheManager.GetData(Key + paramString);
        }

        protected override void SaveItem(string paramString, object item)
        {
            ICacheManager cacheManager = GetCacheManager();
            cacheManager.Add(Key + paramString, item, CacheItemPriority.Normal, null, new SlidingTime(TimeSpan.FromSeconds(ExpirationSeconds)));
        }

        protected override bool Contains(string paramString)
        {

            ICacheManager cacheManager = GetCacheManager();
            return cacheManager.Contains(Key + paramString);
        }

        private DictionaryConfigurationSource GetConfigurationSource()
        {
            DictionaryConfigurationSource configurationSource = new DictionaryConfigurationSource();

            CacheManagerSettings cachingConfiguration = new CacheManagerSettings();
            cachingConfiguration.DefaultCacheManager = Group;

            Type nullBackingStoreType = Type.GetType(new NullBackingStore().GetType().AssemblyQualifiedName);
            CacheStorageData cacheStorageData = new CacheStorageData("CACHE", nullBackingStoreType);
            cachingConfiguration.BackingStores.Add(cacheStorageData);


            CacheManagerData cacheManagerData = new CacheManagerData(Group, ExpirationSeconds, 10000, 5000, "CACHE");
            cachingConfiguration.CacheManagers.Add(cacheManagerData);

            configurationSource.Add(CacheManagerSettings.SectionName, cachingConfiguration);
            return configurationSource;
        }

        private ICacheManager GetCacheManager()
        {
            ICacheManager cacheManager;
            lock (cacheLock)
            {
                if (!cacheManagers.ContainsKey(Group))
                {
                    CacheManagerFactory cacheManagerFactory = new CacheManagerFactory(GetConfigurationSource());
                    cacheManagers.Add(Group, cacheManagerFactory.Create(Group));
                }
                cacheManager = cacheManagers[Group];
            }
            return cacheManager;
        }
    }
}
