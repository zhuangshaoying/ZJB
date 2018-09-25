﻿//------------------------------------------------------------------------------
// <auto-generated>
//    此代码是根据模板生成的。
//
//    手动更改此文件可能会导致应用程序中发生异常行为。
//    如果重新生成代码，则将覆盖对此文件的手动更改。
// </auto-generated>
//------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using MongoDB.Bson.Serialization.Attributes;
using ZJB.Api.Entity;

namespace ZJB.WX.Common.xms
{
    public partial class ImportedHouse
    {
        public ImportedHouse()
        {
            isMove = false;
            MoveStatus = 0;
        }

        [BsonId]
        public string HouseID { get; set; }
        public int UserID { get; set; }
        public byte TradeType { get; set; }
        public int CityID { get; set; }
        public int Distrctid { get; set; }
        public Nullable<int> RegionID { get; set; }
        public int CommunityID { get; set; }
        public string CommunityName { get; set; }
        public byte BuildType { get; set; }
        public decimal BuildArea { get; set; }
        public decimal UsedArea { get; set; }
        public string PointTo { get; set; }
        public Nullable<decimal> UnitPrice { get; set; }
        public decimal Price { get; set; }
        public string PriceUnit { get; set; }
        public int CurFloor { get; set; }
        public int MaxFloor { get; set; }
        public Nullable<int> UsedYear { get; set; }
//        public Nullable<System.DateTime> ExpireDay { get; set; }
  
        public string FitmentStatus { get; set; }
        public Nullable<byte> PicNum { get; set; }
        public string Title { get; set; }
        public string Note { get; set; }
        public Nullable<byte> Status { get; set; }
//        public string IP { get; set; }
//        public Nullable<System.DateTime> AddDate { get; set; }
      public Nullable<System.DateTime> PostTime { get; set; }
        public string Address { get; set; }
        public string LookHouseTime { get; set; }
        public string HouseLabel { get; set; }
//        public string Tag { get; set; }
//        public string InternalNum { get; set; }
        public string CellLabel { get; set; }
        public string YiJuHua { get; set; }
        public byte Room { get; set; }
        public byte Hall { get; set; }
        public byte Kitchen { get; set; }
        public byte Toilet { get; set; }
        public byte Balcony { get; set; }
        public string PayType { get; set; }
//        public string HouseImgPath { get; set; }
//        public Nullable<decimal> LowPay { get; set; }
//        public Nullable<System.DateTime> PushTime { get; set; }
//        public Nullable<System.DateTime> DeleteTime { get; set; }
//        public Nullable<int> Source { get; set; }
//        public Nullable<bool> IsClone { get; set; }
//        public Nullable<int> BeColneHouseID { get; set; }
        public HouseInfo houseInfo { get; set; }
        public FactoryInfo factoryInfo{ get; set; }
        public VillaInfo villaInfo { get; set; }
        public ShopInfo shopInfo { get; set; }
        public OfficeInfo officeInfo { get; set; }
        public string[] roomImages { get; set; }
        public string[] outdoorImages { get; set; }
        public string[] fangxingImages { get; set; }

        public int SiteId { get; set; }
        public string SiteUserName { get; set; }
        public bool isMove { get; set; }
        public int MoveStatus { get; set; }
        /// <summary>
        /// 导入后的房源id
        /// </summary>
        public int MoveHouseId { get; set; }
        public List<ImportedHouseRefUser> RefUser { get; set; }
    }
    public partial class ImportedHouseRefUser
    {
        public int UserID { get; set; }
        public int MoveStatus { get; set; }
        public int MoveHouseId { get; set; }
    }
}
