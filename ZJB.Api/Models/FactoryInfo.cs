using System;
namespace ZJB.Api.Models
{
	/// <summary>
	/// 厂房信息
	/// </summary>
	
	public partial class FactoryInfoModel
	{
        public FactoryInfoModel()
		{}
		#region Model
		private int _houseid;
		private string _factorytype="";
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
		/// 厂房类型
		/// </summary>
		public string FactoryType
		{
			set{ _factorytype=value;}
			get{return _factorytype;}
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

