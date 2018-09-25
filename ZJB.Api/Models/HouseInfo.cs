using System;
namespace ZJB.Api.Models
{
	/// <summary>
	/// 住宅信息
	/// </summary>
	
	public partial class HouseInfoModel
	{
        public HouseInfoModel()
		{}
		#region Model
		private int _houseid;
		private string _housetype="";
		private string _housesubtype="";
		private string _houseproperty="";
		private string _landyear="";
		private string _housestructure="";
		private bool _fiveyears= true;
		private bool _onlyhouse= true;
		private string _basicequip="";
		private string _advequip="";
		/// <summary>
		/// 
		/// </summary>
		public int HouseID
		{
			set{ _houseid=value;}
			get{return _houseid;}
		}
		/// <summary>
		/// 房屋类别
		/// </summary>
		public string HouseType
		{
			set{ _housetype=value;}
			get{return _housetype;}
		}
		/// <summary>
		/// 住宅子类型
		/// </summary>
		public string HouseSubType
		{
			set{ _housesubtype=value;}
			get{return _housesubtype;}
		}
		/// <summary>
		/// 房屋产权
		/// </summary>
		public string HouseProperty
		{
			set{ _houseproperty=value;}
			get{return _houseproperty;}
		}
		/// <summary>
		/// 产权年限
		/// </summary>
		public string LandYear
		{
			set{ _landyear=value;}
			get{return _landyear;}
		}
		/// <summary>
		/// 房屋结构
		/// </summary>
		public string HouseStructure
		{
			set{ _housestructure=value;}
			get{return _housestructure;}
		}
		/// <summary>
		/// 产证满二
		/// </summary>
		public bool FiveYears
		{
			set{ _fiveyears=value;}
			get{return _fiveyears;}
		}
		/// <summary>
		/// 唯一住房
		/// </summary>
		public bool OnlyHouse
		{
			set{ _onlyhouse=value;}
			get{return _onlyhouse;}
		}
		/// <summary>
		/// 基础设施
		/// </summary>
		public string BasicEquip
		{
			set{ _basicequip=value;}
			get{return _basicequip;}
		}
		/// <summary>
		/// 配套设施
		/// </summary>
		public string AdvEquip
		{
			set{ _advequip=value;}
			get{return _advequip;}
		}
		#endregion Model

	}
}

