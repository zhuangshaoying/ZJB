using System;
namespace ZJB.Api.Models
{
	/// <summary>
	/// 写字楼信息
	/// </summary>
	
	public partial class OfficeInfoModel
	{
        public OfficeInfoModel()
		{}
		#region Model
		private int _houseid;
		private string _officetype="";
		private string _officelevel="";
		private string _basicequip="";
		private decimal? _fee=0M;
		private bool _divide= true;
		/// <summary>
		/// 
		/// </summary>
		public int HouseID
		{
			set{ _houseid=value;}
			get{return _houseid;}
		}
		/// <summary>
		/// 写字楼类别
		/// </summary>
		public string OfficeType
		{
			set{ _officetype=value;}
			get{return _officetype;}
		}
		/// <summary>
		/// 写字楼级别
		/// </summary>
		public string OfficeLevel
		{
			set{ _officelevel=value;}
			get{return _officelevel;}
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
		#endregion Model

	}
}

