using System;
namespace ZJB.Api.Models
{
	/// <summary>
	/// 用户站点配置
	/// </summary>
	public partial class UserSiteManageModel
	{
        public UserSiteManageModel()
		{}
		#region Model
		private int _siteid;
		private int _userid;
		private string _siteusername;
		private string _siteuserpwd;
		private int? _sitestatus=1;
        private int? _siteuserid;
		private DateTime? _addtime= DateTime.Now;
		/// <summary>
		/// 站点ID
		/// </summary>
		public int SiteID
		{
			set{ _siteid=value;}
			get{return _siteid;}
		}
		/// <summary>
		/// 用户ID
		/// </summary>
		public int UserID
		{
			set{ _userid=value;}
			get{return _userid;}
		}
        /// <summary>
        /// 网站站点用户编号
        /// </summary>
        public int? SiteUserID
        {
            set { _siteuserid = value; }
            get { return _siteuserid; }
        }
		/// <summary>
		/// 网站站点用户名
		/// </summary>
		public string SiteUserName
		{
			set{ _siteusername=value;}
			get{return _siteusername;}
		}
		/// <summary>
		/// 站点密码
		/// </summary>
		public string SiteUserPwd
		{
			set{ _siteuserpwd=value;}
			get{return _siteuserpwd;}
		}
		/// <summary>
		/// 网站用户帐号状态 1正常状态  0不可用
		/// </summary>
		public int? SiteStatus
		{
			set{ _sitestatus=value;}
			get{return _sitestatus;}
		}
		/// <summary>
		/// 操作时间
		/// </summary>
		public DateTime? AddTime
		{
			set{ _addtime=value;}
			get{return _addtime;}
		}
        public DateTime? BanTime { get; set; }
        public string BanText { get; set; }
        public int? RepeatOperation { get; set; }
        public int? LimitOperation { get; set; }
        public int? PlaceOperation { get; set; }
		#endregion Model

	}
}

