using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ZJB.WX.Models.Client
{
    public class HouseReq
    {
        /// <summary>
        /// 信息类型 1 出售 2 求购 3 出租 4求租
        /// </summary>
        public int PostType { get; set; }

        /// <summary>
        /// 房屋类型（如 写字楼）
        /// </summary>
        public int BuildType { get; set; }
        /// <summary>
        /// 信息状态:1 发布中 2 草稿箱 3 删除状态
        /// </summary>
        public int? BuildingStatus { get; set; }
        /// <summary>
        /// 小区名称
        /// </summary>
        public string Community { get; set; }
        /// <summary>
        /// 排序
        /// </summary>
        public int Sort { get; set; }

        /// <summary>
        /// 房源id
        /// </summary>
        public int HouseId { get; set; }

         /// <summary>
        /// 房源id数组
        /// </summary>
        public string HouseIds { get; set; }
        
        /// <summary>
        /// 标题
        /// </summary>
        public string Title { get; set; }
        /// <summary>
        /// 标签
        /// </summary>
        public string Tags { get; set; }
        /// <summary>
        /// 当前页
        /// </summary>
        public int PageIndex { get; set; }
        /// <summary>
        /// 每页最大数
        /// </summary>
        public int PageSize { get; set; }
        /// <summary>
        /// 用户id
        /// </summary>
        public int UserId { get; set; }

        /// <summary>
        /// 城市id
        /// </summary>
        public int CityId { get; set; }
        /// <summary>
        /// 行政区id
        /// </summary>
        public int DistrictId { get; set; }
        /// <summary>
        /// 行政区
        /// </summary>
        public string DistrictName { get; set; }
        /// <summary>
        /// 路段id
        /// </summary>
        public int RegionId { get; set; }
        /// <summary>
        /// 路段
        /// </summary>
        public string RegionName { get; set; }
        /// <summary>
        /// <summary>
        /// x室
        /// </summary>
        public int Room{ get; set; }
        /// <summary>
        /// 最小价格
        /// </summary>
        public double MinPrice { get; set; }
        /// <summary>
        /// 最大价格
        /// </summary>
        public double MaxPrice { get; set; }
        /// <summary>
        /// 网站来源
        /// </summary>
        public int SiteId { get; set; }
        /// <summary>
        /// 网站名称
        /// </summary>
        public string WebName { get; set; }
        /// <summary>
        /// 网站ID数组
        /// </summary>
        public string WebSiteIds { get; set; }

        /// <summary>
        /// 预约时间时间戳数组
        /// </summary>
        public string OrderTime { get; set; }
        /// <summary>
        /// 发布类型 0 立即发布 1 预约发布
        /// </summary>
        private int _releaseType = 0;
        public int ReleaseType
        {
            set { _releaseType = value; }
            get { return _releaseType; }
        }

        private int _CommunityId = 0;
        public int CommunityId
        {
            set { _CommunityId = value; }
            get { return _CommunityId; }
        }

        /// <summary>
        /// 图片数量
        /// </summary>
        public int PicNum { get; set; }

         /// <summary>
        /// 发布人
        /// </summary>
        public string Publisher { get; set; }

        /// <summary>
        /// 来源
        /// </summary>
        public string Source { get; set; }

           /// <summary>
        /// Url
        /// </summary>
        public string Url { get; set; }

        
           /// <summary>
        /// 发布时间
        /// </summary>
        public long ReleaseTime { get; set; }
        
         /// <summary>
        /// 电话
        /// </summary>
        public string Tel { get; set; }
        
        /// <summary>
        /// 房源采集编号
        /// </summary>
        public string CollectId { get; set; }
        public string CommunityName { get; set; }
        
        public decimal BuildArea { get; set; }
        public decimal UsedArea { get; set; }
        public string PointTo { get; set; }
        public decimal Price { get; set; }
        public string PriceUnit { get; set; }
        public int CurFloor { get; set; }
        public int MaxFloor { get; set; }
        public int UsedYear { get; set; }
        public string FitmentStatus { get; set; }
        public string Note { get; set; }
        public string Address { get; set; }
        public string LookHouseTime { get; set; }
        public string HouseLabel { get; set; }
        public string Tag { get; set; }
        public string InternalNum { get; set; }
        public string CellLabel { get; set; }
        public string YiJuHua { get; set; }
        public int Hall { get; set; }
        public int Kitchen { get; set; }
        public int Toilet { get; set; }
        public int Balcony { get; set; }
        public string PayType { get; set; }
        public string RoomImages { get; set; }
        public string OutDoorImages { get; set; }
        public string FangXingImages { get; set; }

        public string HouseBasicEquip { get; set; }//住宅基础设施
        public string HouseType { get; set; }//房屋类别
        public string HouseSubType { get; set; } //住宅子类型
        public string HouseProperty { get; set; } //房屋产权
        public string LandYear { get; set; } //产权年限
        public string HouseStructure { get; set; }  //房屋结构
        public bool FiveYears { get; set; }//产证满二
        public bool OnlyHouse { get; set; } //唯一住房
        public string HouseAdvEquip { get; set; } //配套设施


        public string VillaType { get; set; }//别墅形式
        public string HallType { get; set; }//厅结构
        public bool Basement { get; set; } //地下室
        public bool Garden { get; set; }//花园
        public bool Garage { get; set; } //车库
        public bool ParkLot { get; set; } //停车位
        public string VillaBasicEquip { get; set; }//配套设施
        public string VillaAdvEquip { get; set; }//室内设施


        public string ShopType { get; set; }//商铺类型
        public string ShopStatus { get; set; }//商铺状态
        public string TargetField { get; set; } //目标业态
        public decimal ShopFee { get; set; } //物业费
        public bool ShopDivide { get; set; }//是否分割
        public string ShopBasicEquip { get; set; } //配套设施

        public string OfficeType { get; set; }//写字楼类别
        public string OfficeLevel { get; set; }//写字楼级别
        public decimal OfficeFee { get; set; } //物业费
        public bool OfficeDivide { get; set; }//是否分割
        public string OfficeBasicEquip { get; set; } //配套设施


        public string FactoryType { get; set; }//厂房类别
        public string FactoryBasicEquip { get; set; }//基础设施
  
    }


    public class HouseRemindReq
    {
        /// <summary>
        /// 关键字
        /// </summary>
        public string Keywords { get; set; }

        /// <summary>
        /// 类型
        /// </summary>
        public int tradeType { get; set; }
       
    }

}