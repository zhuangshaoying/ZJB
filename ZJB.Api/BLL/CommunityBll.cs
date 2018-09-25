using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Common;
using System.Data;
using System.Text;
using log4net;
using ZJB.Api.BLL;
using ZJB.Api.DAL;
using ZJB.Api.Entity;
using ZJB.Api.Models;
using ZJB.Core.Injection;
using ZJB.Core.Utilities;
using ZJB.Web.Utilities;

namespace ZJB.Api.BLL
{
    public class CommunityBll
    {

        private readonly CommunityDal apiDal = Container.Instance.Resolve<CommunityDal>();
        
        
        public virtual List<Community> GetCommunityList(string keyword,int pagesize)
        {
            return apiDal.GetCommunityList(keyword, pagesize);
        }
    }
}