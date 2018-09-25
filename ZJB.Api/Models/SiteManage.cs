using System;
namespace ZJB.Api.Models
{
	/// <summary>
	/// 站点清单
	/// </summary>
	
	public partial class SiteManageModel
	{
        public SiteManageModel()
		{}
		#region Model
		private int _siteid;
		private string _sitename;
		private int? _cityid=592;
		private string _logo="";
		private int? _status=1;
		private bool? _yunrefresh= true;
        private string _loginUrl;
        private string _registerUrl;
		/// <summary>
		/// 
		/// </summary>
		public int SiteID
		{
			set{ _siteid=value;}
			get{return _siteid;}
		}
		/// <summary>
		/// 站点名称
		/// </summary>
		public string SiteName
		{
			set{ _sitename=value;}
			get{return _sitename;}
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
		/// Logo
		/// </summary>
		public string Logo
		{
			set{ _logo=value;}
			get{return _logo;}
		}
		/// <summary>
		/// 站点状态  1 可用 0不可用
		/// </summary>
		public int? Status
		{
			set{ _status=value;}
			get{return _status;}
		}
		/// <summary>
		/// 是否可以云刷新
		/// </summary>
		public bool? YunRefresh
		{
			set{ _yunrefresh=value;}
			get{return _yunrefresh;}
		}
        /// <summary>
        /// 登录URL
        /// </summary>
        public string LoginUrl
        {
            set { _loginUrl = value; }
            get { return _loginUrl; }
        }
        /// <summary>
        /// 注册Url
        /// </summary>
        public string RegisterUrl
        {
            set { _registerUrl = value; }
            get { return _registerUrl; }
        }
		#endregion Model
        public string SiteUserName { get; set; }
        public int State { get; set; }
	}
}

