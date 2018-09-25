using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ZJB.Api.DAL;
using ZJB.Api.Models;
using ZJB.Core.Injection;

namespace ZJB.Api.BLL
{
    public class RegionsBll
    {
        private readonly RegionsDal regionDal = Container.Instance.Resolve<RegionsDal>();
        #region 获取所有的region列表
        public virtual List<RegionsModel> GetRegionList()
        {
            int cityId = 0;
            return regionDal.GetRegionList(cityId);
        }
        public virtual List<RegionsModel> GetRegionList(int cityId)
        {
            return regionDal.GetRegionList(cityId);
        }
        #endregion
    }
}
