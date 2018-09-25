using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZJB.Api.Common
{
    public static class WebConvert
    {
        public static decimal ToDecial(string obj)
        {
            if (string.IsNullOrWhiteSpace(obj))
            {
                return 0;
            }
            return Convert.ToDecimal(obj);
        }
    }
}
