using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ZJB.Api.Entity;
namespace ZJB.Api.Models
{
    public class DynamicModel
    {
        #region 字段
        /// <summary>
        /// Id
        /// </summary>		
        private int _id;
        public int Id
        {
            get { return _id; }
            set { _id = value; }
        }
        /// <summary>
        /// 发帖用户ID
        /// </summary>		
        private int _userid;
        public int UserId
        {
            get { return _userid; }
            set { _userid = value; }
        }
        /// <summary>
        /// Type
        /// </summary>		
        private int _type;
        public int Type
        {
            get { return _type; }
            set { _type = value; }
        }
        /// <summary>
        /// 是否置顶
        /// </summary>		
        private bool _istop;
        public bool IsTop
        {
            get { return _istop; }
            set { _istop = value; }
        }
        /// <summary>
        /// 是否热门
        /// </summary>		
        private bool _ishot;
        public bool IsHot
        {
            get { return _ishot; }
            set { _ishot = value; }
        }
        /// <summary>
        /// 是否精华
        /// </summary>		
        private bool _isessence;
        public bool IsEssence
        {
            get { return _isessence; }
            set { _isessence = value; }
        }
        /// <summary>
        /// 是否帮助
        /// </summary>		
        private bool _ishelp;
        public bool IsHelp
        {
            get { return _ishelp; }
            set { _ishelp = value; }
        }
        /// <summary>
        /// 标题
        /// </summary>		
        private string _title;
        public string Title
        {
            get { return _title; }
            set { _title = value; }
        }
        /// <summary>
        /// 帖子内容
        /// </summary>		
        private string _dynamiccontent;
        public string DynamicContent
        {
            get { return _dynamiccontent; }
            set { _dynamiccontent = value; }
        }
        /// <summary>
        /// 城市ID
        /// </summary>		
        private int _cityid;
        public int CityId
        {
            get { return _cityid; }
            set { _cityid = value; }
        }
        /// <summary>
        /// 城市名称
        /// </summary>		
        private string _cityname;
        public string CityName
        {
            get { return _cityname; }
            set { _cityname = value; }
        }
        /// <summary>
        /// Ip
        /// </summary>		
        private string _ip;
        public string Ip
        {
            get { return _ip; }
            set { _ip = value; }
        }
        /// <summary>
        /// 0 未审核或删除  1 正常 2 关键字过滤 3 管理员删帖
        /// </summary>		
        private int _state;
        public int State
        {
            get { return _state; }
            set { _state = value; }
        }
        /// <summary>
        /// 发帖时间
        /// </summary>		
        private DateTime _addtime;
        public DateTime AddTime
        {
            get { return _addtime; }
            set { _addtime = value; }
        }
        /// <summary>
        /// 点击量
        /// </summary>		
        private int _clicknum;
        public int ClickNum
        {
            get { return _clicknum; }
            set { _clicknum = value; }
        }
        /// <summary>
        /// 投票ID
        /// </summary>		
        private int _voteid;
        public int VoteId
        {
            get { return _voteid; }
            set { _voteid = value; }
        }
        /// <summary>
        /// 回复数
        /// </summary>		
        private int _commentnum;
        public int CommentNum
        {
            get { return _commentnum; }
            set { _commentnum = value; }
        }
        /// <summary>
        /// 最后回复的ID
        /// </summary>		
        private int _lastcommentid;
        public int LastCommentId
        {
            get { return _lastcommentid; }
            set { _lastcommentid = value; }
        }
        /// <summary>
        /// 最后回复时间
        /// </summary>		
        private DateTime _lastcommenttime;
        public DateTime LastCommentTime
        {
            get { return _lastcommenttime; }
            set { _lastcommenttime = value; }
        }
        /// <summary>
        /// 置顶到期时间
        /// </summary>		
        private DateTime _toptime;
        public DateTime TopTime
        {
            get { return _toptime; }
            set { _toptime = value; }
        }
        /// <summary>
        /// 纬度
        /// </summary>		
        private decimal _lat;
        public decimal Lat
        {
            get { return _lat; }
            set { _lat = value; }
        }
        /// <summary>
        /// 经度
        /// </summary>		
        private decimal _lng;
        public decimal Lng
        {
            get { return _lng; }
            set { _lng = value; }
        }
        /// <summary>
        /// 地理位置
        /// </summary>		
        private string _location;
        public string Location
        {
            get { return _location; }
            set { _location = value; }
        }
        /// <summary>
        /// 操作人ID
        /// </summary>		
        private int _operator;
        public int Operator
        {
            get { return _operator; }
            set { _operator = value; }
        }
        /// <summary>
        /// 摘要
        /// </summary>		
        private string _abstract;
        public string Abstract
        {
            get { return _abstract; }
            set { _abstract = value; }
        }
        /// <summary>
        /// 分享数量
        /// </summary>		
        private int _sharenum;
        public int ShareNum
        {
            get { return _sharenum; }
            set { _sharenum = value; }
        }
        /// <summary>
        /// 可见范围 0:公开，所以人可见 1:私密，仅自己可见 2:选中的人可见 3:选中的人不可见
        /// </summary>		
        private int _visible;
        public int Visible
        {
            get { return _visible; }
            set { _visible = value; }
        }
        /// <summary>
        /// 是否@全部人 1:是 0:否
        /// </summary>		
        private bool _ismentionall;
        public bool IsMentionAll
        {
            get { return _ismentionall; }
            set { _ismentionall = value; }
        }
        #endregion

        #region 其他表字段
        public List<DynamicImage> ImageList { get; set; }
        public List<DynamicSupportModel> SupportList { get; set; }
        public string UserName { get; set; }
        public string Portrait { get; set; }
        public string CompanyName { get; set; }
        /// <summary>
        /// 回复的主帖id
        /// </summary>
        public int ReplayId { get; set; }
        /// <summary>
        /// 回复的用户id
        /// </summary>
        public int ReplayUserId { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string ReplayUserName { get; set; }
        /// <summary>
        /// 回复的评论id
        /// </summary>
        public int ReplyCommentId { get; set; }
        #endregion

       
    }
}
