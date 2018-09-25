using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using log4net;
using ZJB.Api.BLL;
using ZJB.Api.Entity;
using ZJB.Api.Models;
namespace ZJB.Api.DAL
{
    public class RegionsDal:BaseDal
    {
        public RegionsDal():base("WX")
        { }
        private ILog logger = LogManager.GetLogger("RegionsDal");
        private NCBaseRule ncBase = new NCBaseRule();

        #region 获取所有的region列表
        public virtual List<RegionsModel> GetRegionList(int cityId)
        {
            List<Regions> regionsList = new List<Regions>();
            
            if(cityId>0)
              regionsList = ncBase.CurrentEntities.Regions.Where(p=>p.CityID==cityId).OrderByDescending(p => p.IsHot).ToList();
            else
              regionsList=ncBase.CurrentEntities.Regions.OrderByDescending(p=>p.IsHot).ToList();
            return EFToModelList(regionsList);
        }
        #endregion
        #region 隐射EF
        private List<RegionsModel> EFToModelList(List<Regions> regionsList)
        {
            return (from item in regionsList select EFToModel(item)).ToList();
        }
        private RegionsModel EFToModel(Regions regionItem)
        {
            return new RegionsModel()
            {
                CityID = regionItem.CityID,
                CityName = regionItem.CityName,
                DisplayOrder = regionItem.DisplayOrder,
                DistrctID = regionItem.DistrctID,
                DistrctName = regionItem.DistrctName,
                Layer = regionItem.Layer,
                Name = regionItem.Name,
                ParentID = regionItem.ParentID,
                RegionID = regionItem.RegionID,
                ShortSpell = regionItem.ShortSpell,
                Spell = regionItem.Spell
            };
        }
        #endregion
    }
}
