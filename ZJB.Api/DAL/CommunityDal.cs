using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Common;
using System.Data;
using System.Text;
using log4net;
using ZJB.Api.BLL;
using ZJB.Api.Entity;
using ZJB.Api.Models;
using ZJB.Core.Utilities;
using ZJB.Web.Utilities;

namespace ZJB.Api.DAL
{
    public class CommunityDal : BaseDal
    {
        public CommunityDal()
            : base("WX")
        { }
        private ILog logger = LogManager.GetLogger("CommunityDal");
        private NCBaseRule ncBase = new NCBaseRule();

        public virtual List<Community> GetCommunityList(string keyword,int pagesize)
        {

          return ncBase.CurrentEntities.Community.Where(o =>o.CityID==592&& o.Name.Contains(keyword)&&o.Status==1).OrderByDescending(o=>o.Recommend).Take(pagesize).ToList();
        }


    }
}