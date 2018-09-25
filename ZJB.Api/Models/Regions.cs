using System;
namespace ZJB.Api.Models
{
	/// <summary>
	/// 行政区域表
	/// </summary>
	
	public partial class RegionsModel
	{
        public RegionsModel()
		{}
		#region Model
		private int _regionid;
		private string _name="";
		private string _spell="";
		private string _shortspell="";
		private int _displayorder=0;
		private int _parentid=0;
		private int _layer=0;
		private int _distrctid=0;
		private string _distrctname="";
		private int _cityid=0;
		private string _cityname="";
        private int _ishot = 0;
        /// <summary>
        /// 区域id
        /// </summary>
        public int RegionID
		{
			set{ _regionid=value;}
			get{return _regionid;}
		}
		/// <summary>
		/// 名称
		/// </summary>
		public string Name
		{
			set{ _name=value;}
			get{return _name;}
		}
		/// <summary>
		/// 拼写
		/// </summary>
		public string Spell
		{
			set{ _spell=value;}
			get{return _spell;}
		}
		/// <summary>
		/// 简拼
		/// </summary>
		public string ShortSpell
		{
			set{ _shortspell=value;}
			get{return _shortspell;}
		}
		/// <summary>
		/// 排序
		/// </summary>
		public int DisplayOrder
		{
			set{ _displayorder=value;}
			get{return _displayorder;}
		}
		/// <summary>
		/// 父id
		/// </summary>
		public int ParentID
		{
			set{ _parentid=value;}
			get{return _parentid;}
		}
		/// <summary>
		/// 级别
		/// </summary>
		public int Layer
		{
			set{ _layer=value;}
			get{return _layer;}
		}
		/// <summary>
		/// 省id
		/// </summary>
		public int DistrctID
		{
			set{ _distrctid=value;}
			get{return _distrctid;}
		}
		/// <summary>
		/// 省名称
		/// </summary>
		public string DistrctName
		{
			set{ _distrctname=value;}
			get{return _distrctname;}
		}
		/// <summary>
		/// 市id
		/// </summary>
		public int CityID
		{
			set{ _cityid=value;}
			get{return _cityid;}
		}
		/// <summary>
		/// 市名称
		/// </summary>
		public string CityName
		{
			set{ _cityname=value;}
			get{return _cityname;}
		}
        public int IsHot
        {
            set { _ishot = value; }
            get { return _ishot; }
        }
        #endregion Model

    }
}

