using System;
using System.Collections.Generic;
namespace ZJB.Api.Models
{
	/// <summary>
	/// 写字楼信息
	/// </summary>
	
	public partial class PostLogModel
	{
        public PostLogModel()
		{}
		#region Model
		private int _id;
		private DateTime _datetime;
		private int? _userid;
		private int? _siteid;
		private int? _infoid;
		private string _targetid;
		private int? _status;
		/// <summary>
		/// 
		/// </summary>
		public int ID
		{
			set{ _id=value;}
			get{return _id;}
		}
		/// <summary>
		/// 执行时间
		/// </summary>
		public DateTime DateTime
		{
			set{ _datetime=value;}
			get{return _datetime;}
		}
		/// <summary>
		/// 用户id
		/// </summary>
		public int? UserID
		{
			set{ _userid=value;}
			get{return _userid;}
		}
		/// <summary>
		/// 站点id
		/// </summary>
		public int? SiteID
		{
			set{ _siteid=value;}
			get{return _siteid;}
		}
		/// <summary>
		/// 房源信息id
		/// </summary>
		public int? InfoID
		{
			set{ _infoid=value;}
			get{return _infoid;}
		}
		/// <summary>
		/// 发布成功后，目标站点的id
		/// </summary>
        public string TargetID
		{
			set{ _targetid=value;}
			get{return _targetid;}
		}
		/// <summary>
		/// 状态
		/// </summary>
		public int? Status
		{
			set{ _status=value;}
			get{return _status;}
		}
        /// <summary>
        ///  发布成功后，目标站点的房源链接地址
        /// </summary>
        public string TargetUrl { get; set; }
        public int IsOrder { get; set; }
        public string Msg { get; set; }
        /// <summary>
        /// 目标网站的账号
        /// </summary>
        public string SiteUserName { get; set; }

        public int PostType { get; set; }
		#endregion Model

      
       

        #region 房源信息
        public int TradeType { get; set; }

        public int BuildType { get; set; }

        public string CommunityName { get; set; }

        public decimal BuildArea { get; set; }

        public int CurFloor { get; set; }

        public int MaxFloor { get; set; }

        public decimal Price { get; set; }

        public string PriceUnit { get; set; }

        public string Title { get; set; }

        public int PostCount { get; set; }
        #endregion
        public string SiteName { get; set; }

        public string Logo { get; set; }

        public int TimeAllCount { get; set; }
        public List<siteTimeLog> SiteTimeLogList { get; set; }

        /// <summary>
        /// 后台看的错误信息
        /// </summary>
        public string RealyMsg { get; set; }

        public DateTime BeginTime { get; set; }

        public int RepeatOperation { get; set; }
        public int LimitOperation { get; set; }
        public int PlaceOperation { get; set; }
        public int ReRelease { get; set; }
    }
    public class siteTimeLog {
        public DateTime time { get; set; }
        public int count { get; set; }
    }
}

