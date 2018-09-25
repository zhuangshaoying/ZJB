using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ZJB.Api.Models
{
    public class QiniuToken
    {
        public string Token { get; set; }
        public double ExpirationTime { get; set; }

    }
}
