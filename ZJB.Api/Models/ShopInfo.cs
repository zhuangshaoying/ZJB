using System;
namespace ZJB.Api.Models
{
	/// <summary>
	/// 商铺信息
	/// </summary>
	
	public partial class ShopInfoModel
	{
        public ShopInfoModel()
		{}
		#region Model
		private int _houseid;
		private string _shoptype="";
		private string _shopstatus="";
		private string _targetfield="";
		private decimal? _fee=0M;
		private bool _divide= true;
		private string _basicequip="";
		/// <summary>
		/// 
		/// </summary>
		public int HouseID
		{
			set{ _houseid=value;}
			get{return _houseid;}
		}
		/// <summary>
		/// 商铺类型
		/// </summary>
		public string ShopType
		{
			set{ _shoptype=value;}
			get{return _shoptype;}
		}
		/// <summary>
		/// 商铺状态
		/// </summary>
		public string ShopStatus
		{
			set{ _shopstatus=value;}
			get{return _shopstatus;}
		}
		/// <summary>
		/// 目标业态
		/// </summary>
		public string TargetField
		{
			set{ _targetfield=value;}
			get{return _targetfield;}
		}
		/// <summary>
		/// 物业费
		/// </summary>
		public decimal? Fee
		{
			set{ _fee=value;}
			get{return _fee;}
		}
		/// <summary>
		/// 是否分割
		/// </summary>
		public bool Divide
		{
			set{ _divide=value;}
			get{return _divide;}
		}
		/// <summary>
		/// 配套设施
		/// </summary>
		public string BasicEquip
		{
			set{ _basicequip=value;}
			get{return _basicequip;}
		}
		#endregion Model

	}
}

