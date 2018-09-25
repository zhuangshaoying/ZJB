using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ZJB.Api.Models.Parame
{
     public class HouseParame
     {
         public int HouseId { get; set; }
         public int UserId { get; set; }
         public int TradeType { get; set; }
         public int CityId { get; set; }
         public int Distrctid { get; set; }
         public int RegionId { get; set; }
         public int CommunityId { get; set; }
         public string CommunityName { get; set; }
         public int BuildType { get; set; }
         public decimal BuildArea { get; set; }
         public decimal UsedArea { get; set; }
         public string PointTo { get; set; }
         public decimal Price { get; set; }
         public string PriceUnit { get; set; }

         public int MinPrice { get; set; }
         public int MaxPrice { get; set; }
         public int MinArea { get; set; }
         public int MaxArea { get; set; }
         public int HouseOrder { get; set; }
         public int PageIndex { get; set; }
         public int PageSize { get; set; }
         public int CurFloor { get; set; }
         public int MaxFloor { get; set; }
         public int UsedYear { get; set; }
         public string FitmentStatus { get; set; }
         public int PicNum { get; set; }
         public string Title { get; set; }
         public string Note { get; set; }
         public string Address { get; set; }
         public string LookHouseTime { get; set; }
         public string HouseLabel { get; set; }
         public string Tag { get; set; }
         public string InternalNum { get; set; }
         public string CellLabel { get; set; }
         public string YiJuHua { get; set; }
         public int Room { get; set; }
         public int Hall { get; set; }
         public int Kitchen { get; set; }
         public int Toilet { get; set; }
         public int Balcony { get; set; }
         public string PayType { get; set; }
       
         public int Source { get; set; }
         public bool IsClone { get; set; }
         public Nullable<int> BeColneHouseId { get; set; }
         public string BeColneID { get; set; }
         public List<HouseImgParame> HouseImgs { get; set; }

         /// <summary>
         /// 房源批量导入 唯一标示
         /// </summary>
         public int IndexPoint { get; set; }


         public decimal UnitPrice { get; set; }

         public decimal LowPay { get; set; }

         public DateTime ExpireDay { get; set; }

         public int Status { get; set; }

         public string IP { get; set; }

         public DateTime PostTime { get; set; }

         public string HouseImgPath { get; set; }

         public DateTime AddDate { get; set; }

         public DateTime PushTime { get; set; }

         public DateTime DeleteTime { get; set; }
     }

    public class HouseImgParame
    {
        public string imageUrl { get; set; }
        public int imageType { get; set; }
        public string imgDescribe { get; set; }
        public bool IsCover { get; set; }
        /// <summary>
        /// 房源批量导入 唯一标示
        /// </summary>
        public int IndexPoint { get; set; }

        public int HouseID { get; set; }

        public string ImagePath { get; set; }

        public string ImagePos { get; set; }

        public int ImageType { get; set; }

        public int OrderID { get; set; }

        public DateTime AddTime { get; set; }

        public int Status { get; set; }

        public int CommunityID { get; set; }

        public int UserID { get; set; }
    }
    public class HouseInfoParame
    {
        public string BasicEquipHouse { get; set; }//住宅基础设施
        public string HouseType { get; set; }//房屋类别
        public string HouseSubType { get; set; } //住宅子类型
        public string HouseProperty { get; set; } //房屋产权
        public string LandYear { get; set; } //产权年限
        public string HouseStructure { get; set; }  //房屋结构
        public bool FiveYears { get; set; }//产证满二
        public bool OnlyHouse { get; set; } //唯一住房
        public string AdvEquip { get; set; } //配套设施
        /// <summary>
        /// 房源批量导入 唯一标示
        /// </summary>
        public int IndexPoint { get; set; }

        public int HouseID { get; set; }

        public string BasicEquip { get; set; }
    }

    public class VillaInfoParame
    {
        public string VillaType { get; set; }//别墅形式
        public string HallType { get; set; }//厅结构
        public string LandYear { get; set; } //产权年限
        public bool FiveYears { get; set; }//产证满二
        public bool OnlyHouse { get; set; } //唯一住房
        public bool Basement { get; set; } //地下室
        public bool Garden { get; set; }//花园
        public bool Garage { get; set; } //车库
        public bool ParkLot { get; set; } //停车位
        public string BasicEquip { get; set; }//配套设施
        public string AdvEquip{ get; set; }//室内设施
        /// <summary>
        /// 房源批量导入 唯一标示
        /// </summary>
        public int IndexPoint { get; set; }


        public int HouseID { get; set; }
    }
    public class ShopInfoParame
    {
        public string ShopType { get; set; }//商铺类型
        public string ShopStatus { get; set; }//商铺状态
        public string TargetField { get; set; } //目标业态
        public decimal Fee { get; set; } //物业费
        public bool Divide { get; set; }//是否分割
        public string BasicEquip{ get; set; } //配套设施
        /// <summary>
        /// 房源批量导入 唯一标示
        /// </summary>
        public int IndexPoint { get; set; }


        public int HouseID { get; set; }
    }
    public class OfficeInfoParame
    {
        public string OfficeType { get; set; }//写字楼类别
        public string OfficeLevel { get; set; }//写字楼级别
       public decimal Fee { get; set; } //物业费
        public bool Divide { get; set; }//是否分割
        public string BasicEquip{ get; set; } //配套设施
        /// <summary>
        /// 房源批量导入 唯一标示
        /// </summary>
        public int IndexPoint { get; set; }


        public int HouseID { get; set; }
    }
    public class FactoryInfoParame
    {
        public string FactoryType { get; set; }//厂房类别
        public string BasicEquip { get; set; }//基础设施

        /// <summary>
        /// 房源批量导入 唯一标示
        /// </summary>
        public int IndexPoint { get; set; }

        public int HouseID { get; set; }
    }
    


}
