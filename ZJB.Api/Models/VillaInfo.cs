using System;
namespace ZJB.Api.Models
{
	/// <summary>
	/// 别墅信息
	/// </summary>
	
	public partial class VillaInfoModel
	{
        public VillaInfoModel()
		{}
		#region Model
		private int _houseid;
		private string _villatype="";
		private string _halltype="";
		private string _landyear="";
		private bool _onlyhouse= true;
		private bool _fiveyears= true;
		private bool _basement= true;
		private bool _garden= false;
		private bool _garage= true;
		private bool _parklot= true;
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
		/// 别墅形式
		/// </summary>
		public string VillaType
		{
			set{ _villatype=value;}
			get{return _villatype;}
		}
		/// <summary>
		/// 厅结构
		/// </summary>
		public string HallType
		{
			set{ _halltype=value;}
			get{return _halltype;}
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
		/// 唯一住房
		/// </summary>
		public bool OnlyHouse
		{
			set{ _onlyhouse=value;}
			get{return _onlyhouse;}
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
		/// 地下室
		/// </summary>
		public bool Basement
		{
			set{ _basement=value;}
			get{return _basement;}
		}
		/// <summary>
		/// 花园
		/// </summary>
		public bool Garden
		{
			set{ _garden=value;}
			get{return _garden;}
		}
		/// <summary>
		/// 车库
		/// </summary>
		public bool Garage
		{
			set{ _garage=value;}
			get{return _garage;}
		}
		/// <summary>
		/// 停车位
		/// </summary>
		public bool ParkLot
		{
			set{ _parklot=value;}
			get{return _parklot;}
		}
		/// <summary>
		/// 配套设施
		/// </summary>
		public string BasicEquip
		{
			set{ _basicequip=value;}
			get{return _basicequip;}
		}
		/// <summary>
		/// 室内设施
		/// </summary>
		public string AdvEquip
		{
			set{ _advequip=value;}
			get{return _advequip;}
		}
		#endregion Model

	}
}

