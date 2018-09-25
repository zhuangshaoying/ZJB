using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ZJB.Core.Cache;

namespace ZJB.Api.Common
{
    public class CacheTimeGroup : CacheGroupBase
    {
        public override int ExpirationSeconds
        {
            get { return 24 * 3600; }
        }
    }
}
