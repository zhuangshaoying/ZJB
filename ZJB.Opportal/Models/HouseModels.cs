using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ZJB.Api.Entity;

namespace ZJB.Opportal.Models
{
    public class HouseModels
    {
        public HouseBasicInfo HouseBasicInfo { get; set; }
        public HouseInfo HouseInfo { get; set; }
        public VillaInfo VillaInfo { get; set; }
        public ShopInfo ShopInfo { get; set; }
        public OfficeInfo OfficeInfo { get; set; }
        public  FactoryInfo  FactoryInfo { get; set; }
        public List<HouseImage> HouseImages { get; set; }
       
    }
}