using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace ZJB.Api.Models
{

    public class HouseListReq
    {
        /// <summary>
        /// 信息类型 1 出售 2 求购 3 出租 4求租
        /// </summary>
        public int postType { get; set; }

        /// <summary>
        /// 房屋类型（如 写字楼）
        /// </summary>
        public int buildingType { get; set; }
        /// <summary>
        /// 信息状态:1 发布中 2 草稿箱 3 删除状态
        /// </summary>
        public int? buildingStatus { get; set; }
        /// <summary>
        /// 小区名称
        /// </summary>
        public string cell { get; set; }
        /// <summary>
        /// 排序
        /// </summary>
        public int sort { get; set; }

        /// <summary>
        /// 房源id
        /// </summary>
        public int houseId { get; set; }
        /// <summary>
        /// 标题
        /// </summary>
        public string title { get; set; }
        /// <summary>
        /// 标签
        /// </summary>
        public string tags { get; set; }
        /// <summary>
        /// 当前页
        /// </summary>
        public int page { get; set; }
        /// <summary>
        /// 每页最大数
        /// </summary>
        public int pageSize { get; set; }
        /// <summary>
        /// 用户id
        /// </summary>
        public int userId { get; set; }

        /// <summary>
        /// 城市id
        /// </summary>
        public int cityId { get; set; }
        /// <summary>
        /// 行政区id
        /// </summary>
        public int districtId { get; set; }
        /// <summary>
        /// 路段id
        /// </summary>
        public int regionId { get; set; }
        /// <summary>
        /// x室
        /// </summary>
        public int roomType { get; set; }
        /// <summary>
        /// 最小价格
        /// </summary>
        public double minPrice { get; set; }
        /// <summary>
        /// 最大价格
        /// </summary>
        public double maxPrice { get; set; }
        /// <summary>
        /// 网站来源
        /// </summary>
        public int siteId { get; set; }
        /// <summary>
        /// 网站名称
        /// </summary>
        public string webName { get; set; }

        public int shareCompanyId { get; set; }

        public int shareStoreId { get; set; }
        public string tel { get; set; }
        /// <summary>
        /// 收藏用户userid
        /// </summary>
        public int collectuserid { get; set; }

        /// <summary>
        /// 地铁线路
        /// </summary>
        public int line { get; set; }
        /// <summary>
        /// 地铁站点
        /// </summary>
        public int metroid { get; set; }
        /// <summary>
        /// 是否显示地铁：0为显示所有的,1只显示地铁房
        /// </summary>
        public int ismetro { get; set; }

        /// <summary>
        /// 是否只显示收藏：0为显示所有的,1只显示用户收藏
        /// </summary>
        public int iscollect { get; set; }



        /// <summary>
        /// 是否只显示浏览
        /// </summary>
        public int IsBrowse { get; set; }
    }
}