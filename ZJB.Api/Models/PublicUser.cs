using System;
using System.Collections.Generic;
namespace ZJB.Api.Models
{
    /// <summary>
    /// 用户表
    /// </summary>
    [Serializable()]
    public partial class PublicUserModel
    {
        public PublicUserModel()
        {
            CompleteTask_First = new List<int>();
        }
        #region Model
        private int _userid;
        private string _name;
        private string _password;
        private string _portrait = "";
        private string _molblie;
        private string _tel;
        private string _nickname;
        private string _enrolnname;
        private int? _sex = 1;
        private string _address;
        private string _email;
        private string _qq;
        private string _ip;
        private int? _status = 1;
        private DateTime? _registertime = DateTime.Now;
        private int? _logintimes = 0;
        private string _lastloginip;
        private DateTime? _lastlogintime = DateTime.Now;
        private string _remarks = "";

        /// <summary>
        /// 
        /// </summary>
        public int UserID
        {
            set { _userid = value; }
            get { return _userid; }
        }
        /// <summary>
        /// 用户名
        /// </summary>
        public string Name
        {
            set { _name = value; }
            get { return _name; }
        }
        /// <summary>
        /// 密码
        /// </summary>
        public string Password
        {
            set { _password = value; }
            get { return _password; }
        }
        /// <summary>
        /// 头像
        /// </summary>
        public string Portrait
        {
            set { _portrait = value; }
            get { return _portrait; }
        }
        /// <summary>
        /// 手机
        /// </summary>
        public string Molblie
        {
            set { _molblie = value; }
            get { return _molblie; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string Tel
        {
            set { _tel = value; }
            get { return _tel; }
        }
        /// <summary>
        /// 昵称
        /// </summary>
        public string NickName
        {
            set { _nickname = value; }
            get { return _nickname; }
        }
        /// <summary>
        /// 真实名称
        /// </summary>
        public string EnrolnName
        {
            set { _enrolnname = value; }
            get { return _enrolnname; }
        }
        /// <summary>
        /// 性别  1男 2女 0保密
        /// </summary>
        public int? Sex
        {
            set { _sex = value; }
            get { return _sex; }
        }
        /// <summary>
        /// 通讯地址
        /// </summary>
        public string Address
        {
            set { _address = value; }
            get { return _address; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string Email
        {
            set { _email = value; }
            get { return _email; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string QQ
        {
            set { _qq = value; }
            get { return _qq; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string IP
        {
            set { _ip = value; }
            get { return _ip; }
        }
        /// <summary>
        /// 用户状态(0:未审核1:审核 2:冻结)
        /// </summary>
        public int? Status
        {
            set { _status = value; }
            get { return _status; }
        }
        /// <summary>
        /// 注册时间
        /// </summary>
        public DateTime? RegisterTime
        {
            set { _registertime = value; }
            get { return _registertime; }
        }
        /// <summary>
        /// 登录次数
        /// </summary>
        public int? LoginTimes
        {
            set { _logintimes = value; }
            get { return _logintimes; }
        }
        /// <summary>
        /// 最后登录的IP
        /// </summary>
        public string LastLoginIP
        {
            set { _lastloginip = value; }
            get { return _lastloginip; }
        }
        /// <summary>
        /// 最后登录时间
        /// </summary>
        public DateTime? LastLoginTime
        {
            set { _lastlogintime = value; }
            get { return _lastlogintime; }
        }

        public string Remarks
        {
            set { _remarks = value; }
            get { return _remarks; }
        }

        #endregion Model


        public int? CompanyId { get; set; }

        public int? StoreId { get; set; }

        public int CityID { get; set; }

        public int PublishNum { get; set; }

        public int MaxStock { get; set; }

        public int VipType { get; set; }

        public DateTime? ExpirationTime { get; set; }

        public int? DistrictId { get; set; }

        public int? RegionId { get; set; }
        /// <summary>
        /// 已完成的新手任务
        /// </summary>
        public List<int> CompleteTask_First { get; set; }

        public int AccessCount { get; set; }
        public string CompanyName { get; set; }
        public string StoreName { get; set; }
        public string VipTypeName { get; set; }

        public int Points { get; set; }

        public string OpenID { get; set; }
        /// <summary>
        /// 聊天连接ID
        /// </summary>
        public string ConnectionId { get; set; }
    }
}

