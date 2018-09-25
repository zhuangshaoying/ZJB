using System;
namespace ZJB.Api.Models
{
	/// <summary>
	/// 房源图片
	/// </summary>
	
	public partial class HouseImageModel
	{
        public HouseImageModel()
		{}
		#region Model
		private int _houseimageid;
		private int? _imagetype=1;
		private int _houseid;
		private int? _communityid=0;
		private int _userid;
		private string _imagepath;
		private string _imagepos;
		private DateTime? _addtime= DateTime.Now;
		private bool _iscover= false;
		private int? _orderid=0;
		private int? _status=1;
		/// <summary>
		/// 自动编号
		/// </summary>
		public int HouseImageID
		{
			set{ _houseimageid=value;}
			get{return _houseimageid;}
		}
		/// <summary>
		/// 图片类型  1室内图 2房型图 3小区图
		/// </summary>
		public int? ImageType
		{
			set{ _imagetype=value;}
			get{return _imagetype;}
		}
		/// <summary>
		/// 对应信息ID
		/// </summary>
		public int HouseID
		{
			set{ _houseid=value;}
			get{return _houseid;}
		}
		/// <summary>
		/// 小区ID
		/// </summary>
		public int? CommunityID
		{
			set{ _communityid=value;}
			get{return _communityid;}
		}
		/// <summary>
		/// 用户编号
		/// </summary>
		public int UserID
		{
			set{ _userid=value;}
			get{return _userid;}
		}
		/// <summary>
		/// 图片路径
		/// </summary>
		public string ImagePath
		{
			set{ _imagepath=value;}
			get{return _imagepath;}
		}
		/// <summary>
		/// 图片说明
		/// </summary>
		public string ImagePos
		{
			set{ _imagepos=value;}
			get{return _imagepos;}
		}
		/// <summary>
		/// 发布时间
		/// </summary>
		public DateTime? AddTime
		{
			set{ _addtime=value;}
			get{return _addtime;}
		}
		/// <summary>
		/// 是否封面图
		/// </summary>
		public bool IsCover
		{
			set{ _iscover=value;}
			get{return _iscover;}
		}
		/// <summary>
		/// 排序ID
		/// </summary>
		public int? OrderID
		{
			set{ _orderid=value;}
			get{return _orderid;}
		}
		/// <summary>
		/// 状态  1显示 0删除状态
		/// </summary>
		public int? Status
		{
			set{ _status=value;}
			get{return _status;}
		}
		#endregion Model

	}
}

