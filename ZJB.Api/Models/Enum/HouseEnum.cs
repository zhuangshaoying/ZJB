using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ZJB.Api.Models
{
    public enum BuildingType
    {
        Other = 0,
        House = 1,  //住宅
        Villa = 2, //别墅
        Shop = 3,//商铺
        Office = 4,//写字楼
        Factory = 5,//厂房

    }
    public enum TradeType
    {
        Other = 0,
        Sell = 1,  //出售
        QiuGou = 2, //求购
        Rent = 3,  //出租
        QiuZu = 4, //求租


    }

    public enum HouseStatus
    {
        Other = 0,
        Release = 1,  //发布中
        Draft = 2, //草稿箱
        Deleted = 3,  //删除

    }

    public enum SourceType
    {
        Web = 0, // 
        Ios = 1,  //
        Android = 2, //
        Pc = 3,  //

    }
    public enum Message
    {
        Interview = 1,//直约业主
        Consult = 2,//谈价磋商

    }
}
