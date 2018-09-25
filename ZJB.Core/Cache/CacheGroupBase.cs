using System.Collections.Generic;

namespace ZJB.Core.Cache
{
    public abstract class CacheGroupBase 
    {
        public abstract int ExpirationSeconds{ get;}
    }
}
