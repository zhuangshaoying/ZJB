using System;
namespace ZJB.Api.Models
{
    /// <summary>
    /// 房源基础信息
    /// </summary>

    public partial class HouseBasicInfoModel
    {
        public HouseBasicInfoModel()
        { }
        #region Model
        private int _houseid;
        private int _userid;
        private int _tradetype = 0;
        private int _cityid;
        private int _distrctid = 0;
        private int? _regionid = 0;
        private int _communityid = 0;
        private string _communityname;
        private int _buildtype = 1;
        private decimal _buildarea = 0M;
        private decimal _usedarea;
        private string _pointto = "";
        private decimal? _unitprice = 0M;
        private decimal _price = 0M;
        private decimal? _lowpay = 0M;
        private string _priceunit = "万";
        private int _curfloor = 0;
        private int _maxfloor = 0;
        private int? _usedyear;
        private DateTime? _expireday = DateTime.Now;
        private string _fitmentstatus = "中等装修";
        private int? _picnum = 0;
        private string _title = "";
        private string _note;
        private int? _status = 0;
        private string _ip;
        private DateTime? _adddate = DateTime.Now;
        private DateTime? _posttime = DateTime.Now;
        private DateTime? _pushtime;
        private DateTime? _deletetime;
        private string _address = "";
        private string _lookhousetime = "电话预约";
        private string _houselabel = "";
        private string _tag;
        private string _internalnum = "";
        private string _celllabel = "";
        private string _yijuhua = "";
        private int _room = 1;
        private int _hall = 0;
        private int _kitchen = 0;
        private int _toilet = 0;
        private int _balcony = 0;
        private string _paytype = "";
        private string _houseimgpath = "";
        private string _userName = "";
        private string _userPic= "";
        //状态名称
        public string StatusName { set; get; }
        //地图坐标
        public string Lat { set; get; }
        //地图坐标
        public string Lng { set; get; }
        //周边配套
        public string PeiTao { set; get; }
        //交通
        public string Traffic { set; get; }
        //openid
        public string OpenId { set; get; }
        /// <summary>
        /// 收藏,1为收藏
        /// </summary>
        public int Collect { set; get; }
        /// <summary>
        /// 
        /// </summary>
        public string DistrctName { set; get; }

        public string RegionName { set; get; }
        /// <summary>
        /// 地铁站点
        /// </summary>
        public string SiteName { set; get; }
        /// <summary>
        /// 地铁几号线
        /// </summary>
        public int Line { set; get; }
        

        /// <summary>
        /// 信息编号
        /// </summary>
        public int HouseID
        {
            set { _houseid = value; }
            get { return _houseid; }
        }
        /// <summary>
        /// 用户ID号
        /// </summary>
        public int UserID
        {
            set { _userid = value; }
            get { return _userid; }
        }
        /// <summary>
        /// 信息类型 1 出售 2 求购 3 出租 4求租
        /// </summary>
        public int TradeType
        {
            set { _tradetype = value; }
            get { return _tradetype; }
        }
        /// <summary>
        /// 站点ID
        /// </summary>
        public int CityID
        {
            set { _cityid = value; }
            get { return _cityid; }
        }
        /// <summary>
        /// 行政区
        /// </summary>
        public int Distrctid
        {
            set { _distrctid = value; }
            get { return _distrctid; }
        }
        /// <summary>
        /// 区域ID 关联Regions表
        /// </summary>
        public int? RegionID
        {
            set { _regionid = value; }
            get { return _regionid; }
        }
        /// <summary>
        /// 
        /// </summary>
        public int CommunityID
        {
            set { _communityid = value; }
            get { return _communityid; }
        }
        /// <summary>
        /// 小区
        /// </summary>
        public string CommunityName
        {
            set { _communityname = value; }
            get { return _communityname; }
        }
        /// <summary>
        /// 房屋类型（如 写字楼）
        /// </summary>
        public int BuildType
        {
            set { _buildtype = value; }
            get { return _buildtype; }
        }
        /// <summary>
        /// 房屋面积(建筑面积)
        /// </summary>
        public decimal BuildArea
        {
            set { _buildarea = value; }
            get { return _buildarea; }
        }
        /// <summary>
        /// 使用面积
        /// </summary>
        public decimal UsedArea
        {
            set { _usedarea = value; }
            get { return _usedarea; }
        }
        /// <summary>
        /// 朝向
        /// </summary>
        public string PointTo
        {
            set { _pointto = value; }
            get { return _pointto; }
        }
        /// <summary>
        /// 单价(元/每平)
        /// </summary>
        public decimal? UnitPrice
        {
            set { _unitprice = value; }
            get { return _unitprice; }
        }
        /// <summary>
        /// 总价格
        /// </summary>
        public decimal Price
        {
            set { _price = value; }
            get { return _price; }
        }
        /// <summary>
        /// 最低首付
        /// </summary>
        public decimal? LowPay
        {
            set { _lowpay = value; }
            get { return _lowpay; }
        }
        /// <summary>
        /// 价格单位 
        /// </summary>
        public string PriceUnit
        {
            set { _priceunit = value; }
            get { return _priceunit; }
        }
        /// <summary>
        /// 所在层数
        /// </summary>
        public int CurFloor
        {
            set { _curfloor = value; }
            get { return _curfloor; }
        }
        /// <summary>
        /// 总层数

        /// </summary>
        public int MaxFloor
        {
            set { _maxfloor = value; }
            get { return _maxfloor; }
        }
        /// <summary>
        /// 使用年限
        /// </summary>
        public int? UsedYear
        {
            set { _usedyear = value; }
            get { return _usedyear; }
        }
        /// <summary>
        /// 到期时间
        /// </summary>
        public DateTime? ExpireDay
        {
            set { _expireday = value; }
            get { return _expireday; }
        }
        /// <summary>
        /// 装修程度
        /// </summary>
        public string FitmentStatus
        {
            set { _fitmentstatus = value; }
            get { return _fitmentstatus; }
        }
        /// <summary>
        /// 房源图片数量
        /// </summary>
        public int? PicNum
        {
            set { _picnum = value; }
            get { return _picnum; }
        }
        /// <summary>
        /// 房源标题
        /// </summary>
        public string Title
        {
            set { _title = value; }
            get { return _title; }
        }
        /// <summary>
        /// 信息说明
        /// </summary>
        public string Note
        {
            set { _note = value; }
            get { return _note; }
        }
        /// <summary>
        /// 信息状态:1 发布中 2 草稿箱 3 删除状态
        /// </summary>
        public int? Status
        {
            set { _status = value; }
            get { return _status; }
        }
        /// <summary>
        /// 添加时IP地址
        /// </summary>
        public string IP
        {
            set { _ip = value; }
            get { return _ip; }
        }
        /// <summary>
        /// 最初入库时间
        /// </summary>
        public DateTime? AddDate
        {
            set { _adddate = value; }
            get { return _adddate; }
        }
        /// <summary>
        /// 最新发布时间

        /// </summary>
        public DateTime? PostTime
        {
            set { _posttime = value; }
            get { return _posttime; }
        }

        public DateTime? PushTime
        {
            set { _pushtime = value; }
            get { return _pushtime; }
        }
        /// <summary>
        /// 删除时间
        /// </summary>
        public DateTime? DeleteTime
        {
            set { _deletetime = value; }
            get { return _deletetime; }
        }
        /// <summary>
        /// 地址
        /// </summary>
        public string Address
        {
            set { _address = value; }
            get { return _address; }
        }
        /// <summary>
        /// 看房时间
        /// </summary>
        public string LookHouseTime
        {
            set { _lookhousetime = value; }
            get { return _lookhousetime; }
        }
        /// <summary>
        /// 房源标签
        /// </summary>
        public string HouseLabel
        {
            set { _houselabel = value; }
            get { return _houselabel; }
        }
        /// <summary>
        /// 标签 0 不限 1放心 2急推 3新房 4急攻
        /// </summary>
        public string Tag
        {
            set { _tag = value; }
            get { return _tag; }
        }
        /// <summary>
        /// 内部编号
        /// </summary>
        public string InternalNum
        {
            set { _internalnum = value; }
            get { return _internalnum; }
        }
        /// <summary>
        /// 小区特色
        /// </summary>
        public string CellLabel
        {
            set { _celllabel = value; }
            get { return _celllabel; }
        }
        /// <summary>
        /// 一句话广告
        /// </summary>
        public string YiJuHua
        {
            set { _yijuhua = value; }
            get { return _yijuhua; }
        }
        /// <summary>
        /// 房
        /// </summary>
        public int Room
        {
            set { _room = value; }
            get { return _room; }
        }
        /// <summary>
        /// 厅
        /// </summary>
        public int Hall
        {
            set { _hall = value; }
            get { return _hall; }
        }
        /// <summary>
        /// 厨房
        /// </summary>
        public int Kitchen
        {
            set { _kitchen = value; }
            get { return _kitchen; }
        }
        /// <summary>
        /// 卫
        /// </summary>
        public int Toilet
        {
            set { _toilet = value; }
            get { return _toilet; }
        }
        /// <summary>
        /// 阳台
        /// </summary>
        public int Balcony
        {
            set { _balcony = value; }
            get { return _balcony; }
        }
        /// <summary>
        /// 支付方式
        /// </summary>
        public string PayType
        {
            set { _paytype = value; }
            get { return _paytype; }
        }
        /// <summary>
        /// 封面图
        /// </summary>
        public string HouseImgPath
        {
            set { _houseimgpath = value; }
            get { return _houseimgpath; }
        }
        public string UserName
        {
            set { _userName = value; }
            get { return _userName; }
        }
        public string UserPic
        {
            set { _userPic = value; }
            get { return _userPic; }
        }

        
        public int Hits { get; set; }
        public string PostSites { get; set; }
        public string OrderSites { get; set; }
        public int OrderStatus { get; set; }
        /// <summary>
        /// 网站数
        /// </summary>
        public int WebCount { get; set; }
        #endregion Model

        public int IsShare { get; set; }

        public string ShareTel { get; set; }

        public string ShareName { get; set; }

        public int ShareCompanyId { get; set; }

        public int ShareCompanyStoreId { get; set; }

        public string ShareCompanyName { get; set; }

        public string ShareCompanyStoreName { get; set; }

        public DateTime ShareExpireDay { get; set; }

        public int ShareCount { get; set; }
        public int ShareIsClone { get; set; }
        public int ShareUserId { get; set; }
        public int IsClone { get; set; }
        public int BeColneHouseID { get; set; }
        public string Tel { get; set; }
        public string Contacts { get; set; }
        /// <summary>
        /// 直约业主数
        /// </summary>
        public int InterviewNum { get; set; }
        /// <summary>
        /// 谈价人数
        /// </summary>
        public int ConsultNum { get; set; }
        
            
    }
    public class ImpHouseResultModel
    {
        public string BeCloneId { get; set; }
        public int HouseId { get; set; }
    }
}

