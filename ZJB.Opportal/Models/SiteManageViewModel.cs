using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ZJB.WX.Models
{
    public class SiteManageViewModel
    {
        public SiteManageViewModel()
		{}
		#region Model
		private int _siteid;
		private string _sitename;
		private int? _cityid=592;
		private string _logo="";
		private int? _status=1;
		private bool? _yunrefresh= true;

        private int _userid;
        private int? _siteuserid;
        private string _siteusername;
        private int? _sitestatus = 1;
        private DateTime? _addtime = DateTime.Now;

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
        /// 用户ID
        /// </summary>
        public int UserID
        {
            set { _userid = value; }
            get { return _userid; }
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
            set { _siteusername = value; }
            get { return _siteusername; }
        }
      
        /// <summary>
        /// 网站用户帐号状态 1正常状态  0不可用
        /// </summary>
        public int? SiteStatus
        {
            set { _sitestatus = value; }
            get { return _sitestatus; }
        }
        /// <summary>
        /// 操作时间
        /// </summary>
        public DateTime? AddTime
        {
            set { _addtime = value; }
            get { return _addtime; }
        }
		#endregion Model
    }
}