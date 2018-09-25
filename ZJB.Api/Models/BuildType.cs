using System;
namespace ZJB.Api.Models
{
	/// <summary>
	/// 房屋类型
	/// </summary>
	public partial class BuildTypeModel
	{
        public BuildTypeModel()
		{}
		#region Model
		private int _buildtypeid;
		private string _buildtypename;
		/// <summary>
		/// 
		/// </summary>
		public int BuildTypeID
		{
			set{ _buildtypeid=value;}
			get{return _buildtypeid;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string BuildTypeName
		{
			set{ _buildtypename=value;}
			get{return _buildtypename;}
		}
		#endregion Model

	}
}

