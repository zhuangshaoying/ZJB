using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ZJB.Api.DAL;
using ZJB.Api.Models;
using ZJB.Core.Injection;

namespace ZJB.Api.BLL
{
    public class SiteManageBll
    {
        private readonly SiteManageDal siteManageDal = Container.Instance.Resolve<SiteManageDal>();
        #region 获取站点列表
        /// <summary>
        /// 获取站点列表
        /// </summary>
        /// <returns></returns>
        public virtual List<SiteManageModel> GetSiteList(string buildType = "", int cityId = 592)
        {
            return siteManageDal.GetSiteList(buildType, cityId);
        }
        #endregion
    }
}
