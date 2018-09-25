using System;
namespace ZJB.Api.Models
{
	/// <summary>
	/// 小区
	/// </summary>
	
	public partial class CommunityModel
	{
        public CommunityModel()
		{}
		#region Model
		private int _communityid;
		private string _name;
		private DateTime? _adddate= DateTime.Now;
		private decimal? _sellprice=0M;
		private decimal? _rentprice=0M;
		private DateTime? _lastposttime= DateTime.Now;
		private int? _cityid;
		private string _cityname;
		private int? _distrctid;
		private string _distrctname="";
		private string _address;
		private string _traffic;
		private int? _buildtype;
		private string _completedate;
		private string _carnum;
		private string _allarea;
		private string _rongji;
		private string _kaifashang;
		private string _lvhua;
		private string _wuyecompany;
		private int? _personnum;
		private decimal? _wuyeprice;
		private string _description;
		private string _peitao;
		private int? _status=0;
		private string _lat;
		private string _lng;
		/// <summary>
		/// 
		/// </summary>
		public int CommunityID
		{
			set{ _communityid=value;}
			get{return _communityid;}
		}
		/// <summary>
		/// 小区名称
		/// </summary>
		public string Name
		{
			set{ _name=value;}
			get{return _name;}
		}
		/// <summary>
		/// 添加时间
		/// </summary>
		public DateTime? AddDate
		{
			set{ _adddate=value;}
			get{return _adddate;}
		}
		/// <summary>
		/// 当前出售价格
		/// </summary>
		public decimal? SellPrice
		{
			set{ _sellprice=value;}
			get{return _sellprice;}
		}
		/// <summary>
		/// 当前出租价格
		/// </summary>
		public decimal? RentPrice
		{
			set{ _rentprice=value;}
			get{return _rentprice;}
		}
		/// <summary>
		/// 最后修改时间
		/// </summary>
		public DateTime? LastPostTime
		{
			set{ _lastposttime=value;}
			get{return _lastposttime;}
		}
		/// <summary>
		/// 城市ID
		/// </summary>
		public int? CityID
		{
			set{ _cityid=value;}
			get{return _cityid;}
		}
		/// <summary>
		/// 城市名称
		/// </summary>
		public string CityName
		{
			set{ _cityname=value;}
			get{return _cityname;}
		}
		/// <summary>
		/// 行政区ID
		/// </summary>
		public int? Distrctid
		{
			set{ _distrctid=value;}
			get{return _distrctid;}
		}
		/// <summary>
		/// 行政区名称
		/// </summary>
		public string DistrctName
		{
			set{ _distrctname=value;}
			get{return _distrctname;}
		}
		/// <summary>
		/// 地址
		/// </summary>
		public string Address
		{
			set{ _address=value;}
			get{return _address;}
		}
		/// <summary>
		/// 交通
		/// </summary>
		public string Traffic
		{
			set{ _traffic=value;}
			get{return _traffic;}
		}
		/// <summary>
		/// 房屋类型
		/// </summary>
		public int? BuildType
		{
			set{ _buildtype=value;}
			get{return _buildtype;}
		}
		/// <summary>
		/// 竣工日期
		/// </summary>
		public string CompleteDate
		{
			set{ _completedate=value;}
			get{return _completedate;}
		}
		/// <summary>
		/// 停车位
		/// </summary>
		public string CarNum
		{
			set{ _carnum=value;}
			get{return _carnum;}
		}
		/// <summary>
		/// 总面积
		/// </summary>
		public string AllArea
		{
			set{ _allarea=value;}
			get{return _allarea;}
		}
		/// <summary>
		/// 容积率
		/// </summary>
		public string RongJi
		{
			set{ _rongji=value;}
			get{return _rongji;}
		}
		/// <summary>
		/// 开发商
		/// </summary>
		public string KaiFaShang
		{
			set{ _kaifashang=value;}
			get{return _kaifashang;}
		}
		/// <summary>
		/// 绿化率
		/// </summary>
		public string LvHua
		{
			set{ _lvhua=value;}
			get{return _lvhua;}
		}
		/// <summary>
		/// 物业公司
		/// </summary>
		public string WuyeCompany
		{
			set{ _wuyecompany=value;}
			get{return _wuyecompany;}
		}
		/// <summary>
		/// 总户数
		/// </summary>
		public int? PersonNum
		{
			set{ _personnum=value;}
			get{return _personnum;}
		}
		/// <summary>
		/// 物业管理费
		/// </summary>
		public decimal? WuyePrice
		{
			set{ _wuyeprice=value;}
			get{return _wuyeprice;}
		}
		/// <summary>
		/// 小区介绍
		/// </summary>
		public string Description
		{
			set{ _description=value;}
			get{return _description;}
		}
		/// <summary>
		/// 小区配套
		/// </summary>
		public string PeiTao
		{
			set{ _peitao=value;}
			get{return _peitao;}
		}
		/// <summary>
		/// 审核状态 1显示 0隐藏
		/// </summary>
		public int? Status
		{
			set{ _status=value;}
			get{return _status;}
		}
		/// <summary>
		/// 纬度
		/// </summary>
		public string Lat
		{
			set{ _lat=value;}
			get{return _lat;}
		}
		/// <summary>
		/// 经度
		/// </summary>
		public string Lng
		{
			set{ _lng=value;}
			get{return _lng;}
		}
		#endregion Model

	}
}

