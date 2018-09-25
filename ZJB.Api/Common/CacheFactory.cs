using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ZJB.Api.Common
{
    public class CacheFactory
    {
        public static ICacheManager GetInstance()
        {
            return new MemoryCacheManager();
        }
    }
}
