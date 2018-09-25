using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Text.RegularExpressions;
using System.Configuration;
using System.Web.Configuration;
using log4net;
using ZJB.Api.Common;
using ZJB.Api.DAL;
using ZJB.Api.Model;
using ZJB.Core.Injection;
using ZJB.Api.Models;


namespace ZJB.Api.BLL
{
    public class WxBll
    {
        private readonly WxDal wxDal = Container.Instance.Resolve<WxDal>();


        /// <summary>
        /// 获取关键词
        /// </summary>
        /// <returns></returns>
        public virtual List<KeyWordReply> GetKeyWordReply(string keyword)
        {
            return wxDal.GetKeyWordReply(keyword);
        }
    }
}