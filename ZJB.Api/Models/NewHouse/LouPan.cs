using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using ZJB.Api.Models;


namespace ZJB.Api.Model
{

    /// <summary>
    /// 楼盘实体类
    /// </summary>
    [Serializable]
    public class LouPan
    {
        #region Model
        private int _id;
        private int? _siteid = 0;
        private string _name;
        private int? _identified = 0;
        private string _type;
        private string _buildingtype;
        private string _provincecode;
        private string _provincename;
        private string _citycode;
        private string _cityname;
        private string _regionname;
        private string _regioncode;
        private string _forumname;
        private string _forumcode;
        private string _address;
        private string _intro;
        private decimal _pricechange = 0M;
        private decimal _beforeprice = 0M;
        private string _price;
        private string _price_avg;
        private string _price_max;
        private string _creater;
        private string _creater_lic;
        private string _fazhan;
        private string _touzi;
        private string _saleaddress;
        private DateTime? _opendate;
        private DateTime? _incomedate;
        private string _opendetail;
        private string _incomedateDetail;
        private string _rate;
        private string _green_rate;
        private string _floor;
        private string _traffic;
        private string _utilites;
        private DateTime? _updatetime;
        private string _ipname;
        private int? _hits = 0;
        private string _pingyin;
        private string _area;
        private string _square;
        private string _videourl;
        private string _housetotal;
        private string _taofangtotal;
        private string _chengjiaototal;
        private string _xsarea;
        private string _chengjiaopercent;
        private string _daili;
        private string _saletel;
        private string _gouwuzhongxin;
        private string _school;
        private string _hospital;
        private string _post;
        private string _nursery;
        private string _bank;
        private string _seaorlake;
        private int? _thinkview = 0;
        private int? _goneview = 0;
        private string _loupan_type;
        private string _wuyecompany;
        private string _jianzhudanwei;
        private string _ip;
        private string _mapnid;
        private int? _zoneid = 0;
        private decimal? _lat;
        private decimal? _lng;
        private int? _newoldsign = 0;
        private int? _tuijian = 0;
        private DateTime? _tjdatetime;
        private int? _redsign = 0;
        private int? _status = 0;
        private int? _publicuserid = 0;
        private string _domains;
        private int? _display = 0;
        private int? _istop = 0;
        private DateTime? _dengjitime;
        private string _operator;
        private int? _queuetop = 0;
        private string _priceold;
        private string _neighborid;
        private string _initial;
        private string _floorhigh;
        private string _huxing_min;
        private string _huxing_max;
        private decimal? _area_min = 0M;
        private decimal? _area_max = 0M;
        private decimal _price_ref = 0M;
        private string _spjiage;
        private string _jiawei;
        private string _bbsurl;
        private string _one_pay;
        private string _jie_pay;
        private string _linkname;
        private string _linktel;
        private string _linkpost;
        private string _creater_intro;
        private string _daili_intro;
        private string _jianzhu_intro;
        private string _wuye_intro;
        private string _kgdate;
        private string _incomedate_first;
        private string _vagueopendate;
        private string _xianzhuang;
        private string _incomedate_all;
        private string _jgdate;
        private int? _shoptotal = 0;
        private decimal? _car_upprice;
        private string _car_num = "0";
        private string _car_upnum = "0";
        private decimal? _car_uppriceavg = 0M;
        private decimal? _car_downprice = 0M;
        private string _car_downnum = "0";
        private decimal? _car_downpriceavg;
        private string _xiaoshou_card;
        private string _xiaoshou_no;
        private string _guihua_card;
        private string _guihua_no;
        private string _jsyd_card;
        private string _jsyd_no;
        private string _jzydkg_card;
        private string _jzydkg_no;
        private string _jsgczl_card;
        private string _jsgczl_no;
        private string _zzzl_card;
        private string _zzzl_no;
        private string _zzsy_card;
        private string _zzsy_no;
        private string _jzcl_card;
        private string _jzcl_no;
        private string _otherseaorlake;
        private string _yhtel;
        private string _yhprice;
        private string _yhnote;
        private int? _yhorder = 0;
        private string _yhpic;
        private DateTime _yhendtime = DateTime.Now;
        private string _realprice;
        private DateTime? _regtime;
        private string _regip;
        private string _bannerpic;
        private string _bannergif;
        private bool _isvilla = false;
        private DateTime? _specificopeningdate;
        private string _fitment;
        private int? _extension;
        private string _pool;
        private int? _bbsid;
        private int? _uid;
        private string _fromxmjydj;
        private int? _fromxmjydjiscollection = 0;
        private DateTime _zddate;
        private string _fengmian;
        private int _rownumber = 0;
        private LouPanOrder _orderby = LouPanOrder.UpdateTimeDesc;
        private int _askcount;
        private int _tuangoucount;
        private DateTime? _bannerPicStartTime;
        private DateTime? _bannerPicEndTime;
        /// <summary>
        /// 区域推荐
        /// </summary>
        public int _quYuTj = 0;
        private string _areaRank { get; set; }

        /// <summary>
        /// 楼盘编号
        /// </summary>
        public int loupan_Id
        {
            set { _id = value; }
            get { return _id; }
        }
        /// <summary>
        /// 分站编号
        /// </summary>
        public int? siteId
        {
            set { _siteid = value; }
            get { return _siteid; }
        }
        /// <summary>
        /// 楼盘名
        /// </summary>
        public string loupan_Name
        {
            set { _name = value; }
            get { return _name; }
        }
        /// <summary>
        /// 审核状态
        /// </summary>
        public int? identified
        {
            set { _identified = value; }
            get { return _identified; }
        }
        /// <summary>
        /// 物业状态
        /// </summary>
        public string wuyeStatus
        {
            set { _type = value; }
            get { return _type; }
        }
        /// <summary>
        /// 建筑类型
        /// </summary>
        public string buildingType
        {
            set { _buildingtype = value; }
            get { return _buildingtype; }
        }
        /// <summary>
        /// 省级编号
        /// </summary>
        public string provinceCode
        {
            set { _provincecode = value; }
            get { return _provincecode; }
        }
        /// <summary>
        /// 省级名称
        /// </summary>
        public string provinceName
        {
            set { _provincename = value; }
            get { return _provincename; }
        }
        /// <summary>
        /// 城市编号
        /// </summary>
        public string cityCode
        {
            set { _citycode = value; }
            get { return _citycode; }
        }
        /// <summary>
        /// 城市名称
        /// </summary>
        public string cityName
        {
            set { _cityname = value; }
            get { return _cityname; }
        }
        /// <summary>
        /// 区域名称
        /// </summary>
        public string regionName
        {
            set { _regionname = value; }
            get { return _regionname; }
        }
        /// <summary>
        /// 片区编号
        /// </summary>
        public string DistrictCode { get; set; }

        /// <summary>
        /// 区域编号
        /// </summary>
        public string regionCode
        {
            set { _regioncode = value; }
            get { return _regioncode; }
        }
        /// <summary>
        /// 版块名称
        /// </summary>
        public string forumName
        {
            set { _forumname = value; }
            get { return _forumname; }
        }
        /// <summary>
        /// 版块编号
        /// </summary>
        public string forumCode
        {
            set { _forumcode = value; }
            get { return _forumcode; }
        }
        /// <summary>
        /// 项目地址
        /// </summary>
        public string address
        {
            set { _address = value; }
            get { return _address; }
        }
        /// <summary>
        /// 项目介绍
        /// </summary>
        public string intro
        {
            set { _intro = value; }
            get { return _intro; }
        }
        /// <summary>
        /// 价格浮动比率(同比上月)
        /// </summary>
        public decimal priceChange
        {
            set { _pricechange = value; }
            get { return _pricechange; }
        }
        /// <summary>
        /// 上个月的价格(ref参考价)
        /// </summary>
        public decimal beforePrice
        {
            set { _beforeprice = value; }
            get { return _beforeprice; }
        }
        /// <summary>
        /// 起 价
        /// </summary>
        public string price_Qijia
        {
            set { _price = value; }
            get { return _price; }
        }
        /// <summary>
        /// 均 价
        /// </summary>
        public string price_Avg
        {
            set { _price_avg = value; }
            get { return _price_avg; }
        }
        /// <summary>
        /// 最 高 价
        /// </summary>
        public string price_Max
        {
            set { _price_max = value; }
            get { return _price_max; }
        }
        /// <summary>
        /// 开发商
        /// </summary>
        public string creater
        {
            set { _creater = value; }
            get { return _creater; }
        }
        /// <summary>
        /// 预售证
        /// </summary>
        public string creater_Lic
        {
            set { _creater_lic = value; }
            get { return _creater_lic; }
        }
        /// <summary>
        /// 发展商
        /// </summary>
        public string fazhan
        {
            set { _fazhan = value; }
            get { return _fazhan; }
        }
        /// <summary>
        /// 投资商
        /// </summary>
        public string touzi
        {
            set { _touzi = value; }
            get { return _touzi; }
        }
        /// <summary>
        /// 售楼处地址
        /// </summary>
        public string saleAddress
        {
            set { _saleaddress = value; }
            get { return _saleaddress; }
        }
        /// <summary>
        ///  开盘日期
        /// </summary>
        public DateTime? openDate
        {
            set { _opendate = value; }
            get { return _opendate; }
        }
        /// <summary>
        /// 入住时间 作为 本月入住 栏目参考时间
        /// </summary>
        public DateTime? ruzhuDate
        {
            set { _incomedate = value; }
            get { return _incomedate; }
        }

        /// <summary>
        ///  开盘详情
        /// </summary>
        public string OpenDateDetail
        {
            set { _opendetail = value; }
            get { return _opendetail; }
        }
        /// <summary>
        /// 入住详情
        /// </summary>
        public string RuzhuDateDetail
        {
            set { _incomedateDetail = value; }
            get { return _incomedateDetail; }
        }

        /// <summary>
        /// 容 积 率
        /// </summary>
        public string rongji_Rate
        {
            set { _rate = value; }
            get { return _rate; }
        }
        /// <summary>
        /// 绿 化 率
        /// </summary>
        public string lvhua_Rate
        {
            set { _green_rate = value; }
            get { return _green_rate; }
        }
        /// <summary>
        /// 建筑规划层数
        /// </summary>
        public string guiHua_Floors
        {
            set { _floor = value; }
            get { return _floor; }
        }
        /// <summary>
        /// 交通工具
        /// </summary>
        public string traffic
        {
            set { _traffic = value; }
            get { return _traffic; }
        }
        /// <summary>
        /// 小区内配套设施
        /// </summary>
        public string utilites
        {
            set { _utilites = value; }
            get { return _utilites; }
        }
        /// <summary>
        /// 更新时间
        /// </summary>
        public DateTime? updateTime
        {
            set { _updatetime = value; }
            get { return _updatetime; }
        }
        /// <summary>
        /// ip地址名
        /// </summary>
        public string ipName
        {
            set { _ipname = value; }
            get { return _ipname; }
        }
        /// <summary>
        /// 楼盘点击数
        /// </summary>
        public int? hits
        {
            set { _hits = value; }
            get { return _hits; }
        }
        /// <summary>
        /// 楼盘名 拼音 首字母 
        /// </summary>
        public string pingYin
        {
            set { _pingyin = value; }
            get { return _pingyin; }
        }
        /// <summary>
        /// 总建筑面积
        /// </summary>
        public string allArea_JianZhu
        {
            set { _area = value; }
            get { return _area; }
        }
        /// <summary>
        /// 总占地面积
        /// </summary>
        public string allArea_ZhanDi
        {
            set { _square = value; }
            get { return _square; }
        }
        /// <summary>
        /// 视频地址
        /// </summary>
        public string videoUrl
        {
            set { _videourl = value; }
            get { return _videourl; }
        }
        /// <summary>
        /// 住宅幢数
        /// </summary>
        public string houseTotals
        {
            set { _housetotal = value; }
            get { return _housetotal; }
        }
        /// <summary>
        /// 住宅总套数
        /// </summary>
        public string taoFangTotals
        {
            set { _taofangtotal = value; }
            get { return _taofangtotal; }
        }
        /// <summary>
        /// 成交套数
        /// </summary>
        public string chengJiaoTotals
        {
            set { _chengjiaototal = value; }
            get { return _chengjiaototal; }
        }
        /// <summary>
        /// 销售面积
        /// </summary>
        public string allArea_XiaoShou
        {
            set { _xsarea = value; }
            get { return _xsarea; }
        }
        /// <summary>
        /// 成交比例
        /// </summary>
        public string chengJiaoPercent
        {
            set { _chengjiaopercent = value; }
            get { return _chengjiaopercent; }
        }
        /// <summary>
        /// 代理商
        /// </summary>
        public string daiLi
        {
            set { _daili = value; }
            get { return _daili; }
        }
        /// <summary>
        /// 销售热线
        /// </summary>
        public string saleTel
        {
            set { _saletel = value; }
            get { return _saletel; }
        }
        /// <summary>
        /// 购物中心
        /// </summary>
        public string gouWuZhongXin
        {
            set { _gouwuzhongxin = value; }
            get { return _gouwuzhongxin; }
        }
        /// <summary>
        /// 学校
        /// </summary>
        public string school
        {
            set { _school = value; }
            get { return _school; }
        }
        /// <summary>
        /// 医院
        /// </summary>
        public string hospital
        {
            set { _hospital = value; }
            get { return _hospital; }
        }
        /// <summary>
        /// 邮 政 局
        /// </summary>
        public string post
        {
            set { _post = value; }
            get { return _post; }
        }
        /// <summary>
        /// 幼 儿 园
        /// </summary>
        public string nursery
        {
            set { _nursery = value; }
            get { return _nursery; }
        }
        /// <summary>
        /// 银 行
        /// </summary>
        public string bank
        {
            set { _bank = value; }
            get { return _bank; }
        }
        /// <summary>
        /// 景 观
        /// </summary>
        public string seaOrLake
        {
            set { _seaorlake = value; }
            get { return _seaorlake; }
        }
        /// <summary>
        /// 想看该楼盘人数
        /// </summary>
        public int? thinkView
        {
            set { _thinkview = value; }
            get { return _thinkview; }
        }
        /// <summary>
        /// 看过该楼盘人数
        /// </summary>
        public int? goneView
        {
            set { _goneview = value; }
            get { return _goneview; }
        }
        /// <summary>
        /// 物业类型
        /// </summary>
        public string loupan_Type
        {
            set { _loupan_type = value; }
            get { return _loupan_type; }
        }
        /// <summary>
        /// 物业管理
        /// </summary>
        public string wuYeCompany
        {
            set { _wuyecompany = value; }
            get { return _wuyecompany; }
        }
        /// <summary>
        /// 建筑单位
        /// </summary>
        public string jianZhuDanWei
        {
            set { _jianzhudanwei = value; }
            get { return _jianzhudanwei; }
        }
        /// <summary>
        /// ip
        /// </summary>
        public string ip
        {
            set { _ip = value; }
            get { return _ip; }
        }
        /// <summary>
        /// 地图坐标(经纬结合)
        /// </summary>
        public string mapNid
        {
            set { _mapnid = value; }
            get { return _mapnid; }
        }
        /// <summary>
        /// 小区ID
        /// </summary>
        public int? zoneID
        {
            set { _zoneid = value; }
            get { return _zoneid; }
        }
        /// <summary>
        /// 经度
        /// </summary>
        public decimal? lat
        {
            set { _lat = value; }
            get { return _lat; }
        }
        /// <summary>
        /// 纬度
        /// </summary>
        public decimal? lng
        {
            set { _lng = value; }
            get { return _lng; }
        }
        /// <summary>
        /// 是否新盘
        /// </summary>
        public int? newOldSign
        {
            set { _newoldsign = value; }
            get { return _newoldsign; }
        }
        /// <summary>
        /// 推荐楼盘
        /// </summary>
        public int? tuiJian
        {
            set { _tuijian = value; }
            get { return _tuijian; }
        }
        /// <summary>
        ///  推荐楼盘的操作时间
        /// </summary>
        public DateTime? tjDateTime
        {
            set { _tjdatetime = value; }
            get { return _tjdatetime; }
        }
        /// <summary>
        /// 红色标题
        /// </summary>
        public int? redSign
        {
            set { _redsign = value; }
            get { return _redsign; }
        }
        /// <summary>
        /// 楼盘现况
        /// </summary>
        public int? status
        {
            set { _status = value; }
            get { return _status; }
        }
        /// <summary>
        /// 所属开发商
        /// </summary>
        public int? publicUserID
        {
            set { _publicuserid = value; }
            get { return _publicuserid; }
        }
        /// <summary>
        /// 2级域名
        /// </summary>
        public string domains
        {
            set { _domains = value; }
            get { return _domains; }
        }
        /// <summary>
        /// 是否显示?
        /// </summary>
        public int? display
        {
            set { _display = value; }
            get { return _display; }
        }
        /// <summary>
        /// 是否热门
        /// </summary>
        public int? istop
        {
            set { _istop = value; }
            get { return _istop; }
        }
        /// <summary>
        /// 登记时间
        /// </summary>
        public DateTime? dengJiTime
        {
            set { _dengjitime = value; }
            get { return _dengjitime; }
        }
        /// <summary>
        /// 操作者
        /// </summary>
        public string admin
        {
            set { _operator = value; }
            get { return _operator; }
        }
        /// <summary>
        /// 排序优先
        /// </summary>
        public int? queueTop
        {
            set { _queuetop = value; }
            get { return _queuetop; }
        }
        /// <summary>
        /// 历史价格
        /// </summary>
        public string priceHistory
        {
            set { _priceold = value; }
            get { return _priceold; }
        }
        /// <summary>
        /// 邻近楼盘ID
        /// </summary>
        public string neighborID
        {
            set { _neighborid = value; }
            get { return _neighborid; }
        }
        /// <summary>
        /// 楼盘首字音序字母
        /// </summary>
        public string initial
        {
            set { _initial = value; }
            get { return _initial; }
        }
        /// <summary>
        /// 层高
        /// </summary>
        public string floorHigh
        {
            set { _floorhigh = value; }
            get { return _floorhigh; }
        }
        /// <summary>
        /// 最小户型
        /// </summary>
        public string huxing_Min
        {
            set { _huxing_min = value; }
            get { return _huxing_min; }
        }
        /// <summary>
        /// 最大户型
        /// </summary>
        public string huxing_Max
        {
            set { _huxing_max = value; }
            get { return _huxing_max; }
        }
        /// <summary>
        /// 最小面积
        /// </summary>
        public decimal? area_Min
        {
            set { _area_min = value; }
            get { return _area_min; }
        }
        /// <summary>
        /// 最大面积
        /// </summary>
        public decimal? area_Max
        {
            set { _area_max = value; }
            get { return _area_max; }
        }
        /// <summary>
        /// 参考价
        /// </summary>
        public decimal price_Ref
        {
            set { _price_ref = value; }
            get { return _price_ref; }
        }
        /// <summary>
        /// 商铺价格
        /// </summary>
        public string jiaGe_ShangPu
        {
            set { _spjiage = value; }
            get { return _spjiage; }
        }
        /// <summary>
        /// 价位
        /// </summary>
        public string jiaWei
        {
            set { _jiawei = value; }
            get { return _jiawei; }
        }
        /// <summary>
        /// 业主论坛地址
        /// </summary>
        public string bbsUrl
        {
            set { _bbsurl = value; }
            get { return _bbsurl; }
        }
        /// <summary>
        /// 一次性付款
        /// </summary>
        public string one_Pay
        {
            set { _one_pay = value; }
            get { return _one_pay; }
        }
        /// <summary>
        /// 按 揭
        /// </summary>
        public string anJie_Pay
        {
            set { _jie_pay = value; }
            get { return _jie_pay; }
        }
        /// <summary>
        /// 联系人姓名
        /// </summary>
        public string linkName
        {
            set { _linkname = value; }
            get { return _linkname; }
        }
        /// <summary>
        /// 联系人电话
        /// </summary>
        public string linkTel
        {
            set { _linktel = value; }
            get { return _linktel; }
        }
        /// <summary>
        /// 联系人职位
        /// </summary>
        public string linkPost
        {
            set { _linkpost = value; }
            get { return _linkpost; }
        }
        /// <summary>
        /// 开发商简介
        /// </summary>
        public string creater_Intro
        {
            set { _creater_intro = value; }
            get { return _creater_intro; }
        }
        /// <summary>
        /// 代理商简介
        /// </summary>
        public string daiLi_Intro
        {
            set { _daili_intro = value; }
            get { return _daili_intro; }
        }
        /// <summary>
        /// 建筑单位简介
        /// </summary>
        public string jianZhu_Intro
        {
            set { _jianzhu_intro = value; }
            get { return _jianzhu_intro; }
        }
        /// <summary>
        /// 物业管理简介
        /// </summary>
        public string wuYe_Intro
        {
            set { _wuye_intro = value; }
            get { return _wuye_intro; }
        }
        /// <summary>
        /// 开工日期
        /// </summary>
        public string kaiGong_Date
        {
            set { _kgdate = value; }
            get { return _kgdate; }
        }
        /// <summary>
        /// 最早入住时间
        /// </summary>
        public string incomeDate_First
        {
            set { _incomedate_first = value; }
            get { return _incomedate_first; }
        }
        /// <summary>
        /// 模糊开盘日期
        /// </summary>
        public string vagueOpenDate
        {
            set { _vagueopendate = value; }
            get { return _vagueopendate; }
        }
        /// <summary>
        /// 目前工程状况
        /// </summary>
        public string xianZhuang
        {
            set { _xianzhuang = value; }
            get { return _xianzhuang; }
        }
        /// <summary>
        /// 全面入住时间
        /// </summary>
        public string incomeDate_All
        {
            set { _incomedate_all = value; }
            get { return _incomedate_all; }
        }
        /// <summary>
        /// 竣工日期
        /// </summary>
        public string junGong_Date
        {
            set { _jgdate = value; }
            get { return _jgdate; }
        }
        /// <summary>
        /// 店面总套数
        /// </summary>
        public int? dianMian_Sum
        {
            set { _shoptotal = value; }
            get { return _shoptotal; }
        }
        /// <summary>
        /// 地上车位租价
        /// </summary>
        public decimal? diShangCheWei_Rent
        {
            set { _car_upprice = value; }
            get { return _car_upprice; }
        }
        /// <summary>
        /// 车位总数
        /// </summary>
        public string cheWei_Sum
        {
            set { _car_num = value; }
            get { return _car_num; }
        }
        /// <summary>
        /// 地上车位数量
        /// </summary>
        public string diShangCheWei_Sum
        {
            set { _car_upnum = value; }
            get { return _car_upnum; }
        }
        /// <summary>
        /// 地上车位均价
        /// </summary>
        public decimal? diShangCheWei_AvgPrice
        {
            set { _car_uppriceavg = value; }
            get { return _car_uppriceavg; }
        }
        /// <summary>
        /// 地下车位租价
        /// </summary>
        public decimal? diXiaCheWei_Rent
        {
            set { _car_downprice = value; }
            get { return _car_downprice; }
        }
        /// <summary>
        /// 地下车位数量
        /// </summary>
        public string diXiaCheWei_Sum
        {
            set { _car_downnum = value; }
            get { return _car_downnum; }
        }
        /// <summary>
        ///  地下车位均价
        /// </summary>
        public decimal? diXiaCheWei_AvgPrice
        {
            set { _car_downpriceavg = value; }
            get { return _car_downpriceavg; }
        }
        /// <summary>
        /// 销售许可证号码(0,无;1,有,2,审批中)
        /// </summary>
        public string xiaoShou_Card
        {
            set { _xiaoshou_card = value; }
            get { return _xiaoshou_card; }
        }
        /// <summary>
        ///  销售许可证号码
        /// </summary>
        public string xiaoShou_NO
        {
            set { _xiaoshou_no = value; }
            get { return _xiaoshou_no; }
        }
        /// <summary>
        ///  建设工程规划许可证：(0,无;1,有,2,审批中)
        /// </summary>
        public string guiHua_Card
        {
            set { _guihua_card = value; }
            get { return _guihua_card; }
        }
        /// <summary>
        /// 建设工程规划许可证：
        /// </summary>
        public string guiHua_NO
        {
            set { _guihua_no = value; }
            get { return _guihua_no; }
        }
        /// <summary>
        /// 建设用地许可证：(0,无;1,有,2,审批中)
        /// </summary>
        public string jianSheYongDi_Card
        {
            set { _jsyd_card = value; }
            get { return _jsyd_card; }
        }
        /// <summary>
        /// 建设用地许可证：
        /// </summary>
        public string jianSheYongDi_NO
        {
            set { _jsyd_no = value; }
            get { return _jsyd_no; }
        }
        /// <summary>
        /// 建筑用地开工证(0,无;1,有,2,审批中)
        /// </summary>
        public string jianZhuYongDiKaiGong_Card
        {
            set { _jzydkg_card = value; }
            get { return _jzydkg_card; }
        }
        /// <summary>
        /// 建筑用地开工证
        /// </summary>
        public string jianZhuYongDiKaiGong_NO
        {
            set { _jzydkg_no = value; }
            get { return _jzydkg_no; }
        }
        /// <summary>
        /// 建设工程质量合格证书(0,无;1,有,2,审批中)
        /// </summary>
        public string jianSheGongChengZhiLiang_Card
        {
            set { _jsgczl_card = value; }
            get { return _jsgczl_card; }
        }
        /// <summary>
        /// 建设工程质量合格证书
        /// </summary>
        public string jianSheGongChengZhiLiang_NO
        {
            set { _jsgczl_no = value; }
            get { return _jsgczl_no; }
        }
        /// <summary>
        /// 住宅质量保证书(0,无;1,有,2,审批中)
        /// </summary>
        public string zhuZhaiZhiLiang_Card
        {
            set { _zzzl_card = value; }
            get { return _zzzl_card; }
        }
        /// <summary>
        /// 住宅质量保证书号码 
        /// </summary>
        public string zhuZhaiZhiLiang_NO
        {
            set { _zzzl_no = value; }
            get { return _zzzl_no; }
        }
        /// <summary>
        /// 住宅使用说明书(0,无;1,有,2,审批中)
        /// </summary>
        public string zhuZhaiShiYong_Card
        {
            set { _zzsy_card = value; }
            get { return _zzsy_card; }
        }
        /// <summary>
        /// 住宅使用说明书编号
        /// </summary>
        public string zhuZhaiShiYong_NO
        {
            set { _zzsy_no = value; }
            get { return _zzsy_no; }
        }
        /// <summary>
        /// 建筑测量书(0,无;1,有,2,审批中)
        /// </summary>
        public string jianZhuCeLiang_Card
        {
            set { _jzcl_card = value; }
            get { return _jzcl_card; }
        }
        /// <summary>
        /// 建筑测量书
        /// </summary>
        public string jianZhuCeLiang_NO
        {
            set { _jzcl_no = value; }
            get { return _jzcl_no; }
        }
        /// <summary>
        /// 其他景观
        /// </summary>
        public string OtherSeaOrLake
        {
            set { _otherseaorlake = value; }
            get { return _otherseaorlake; }
        }
        /// <summary>
        /// 优惠电话
        /// </summary>
        public string youHui_Tel
        {
            set { _yhtel = value; }
            get { return _yhtel; }
        }
        /// <summary>
        /// 优惠价格
        /// </summary>
        public string youHui_Price
        {
            set { _yhprice = value; }
            get { return _yhprice; }
        }
        /// <summary>
        /// 优惠备注
        /// </summary>
        public string youHui_Note
        {
            set { _yhnote = value; }
            get { return _yhnote; }
        }
        /// <summary>
        /// 优惠排序
        /// </summary>
        public int? youHui_Order
        {
            set { _yhorder = value; }
            get { return _yhorder; }
        }
        /// <summary>
        /// 优惠图片
        /// </summary>
        public string youHui_Pic
        {
            set { _yhpic = value; }
            get { return _yhpic; }
        }
        /// <summary>
        /// 优惠结束时间
        /// </summary>
        public DateTime youhui_EndTime
        {
            set { _yhendtime = value; }
            get { return _yhendtime; }
        }
        /// <summary>
        ///  具体价格
        /// </summary>
        public string realPrice
        {
            set { _realprice = value; }
            get { return _realprice; }
        }
        /// <summary>
        /// 注册时间
        /// </summary>
        public DateTime? regTime
        {
            set { _regtime = value; }
            get { return _regtime; }
        }
        /// <summary>
        /// 注册ip
        /// </summary>
        public string regIP
        {
            set { _regip = value; }
            get { return _regip; }
        }
        /// <summary>
        /// Banner图
        /// </summary>
        public string bannerPic
        {
            set { _bannerpic = value; }
            get { return _bannerpic; }
        }
        /// <summary>
        /// Banner图Gif
        /// </summary>
        public string bannerGif
        {
            set { _bannergif = value; }
            get { return _bannergif; }
        }
        /// <summary>
        /// 是否别墅
        /// </summary>
        public bool isVilla
        {
            set { _isvilla = value; }
            get { return _isvilla; }
        }
        /// <summary>
        /// 具体开盘时间
        /// </summary>
        public DateTime? juTi_OpeningDate
        {
            set { _specificopeningdate = value; }
            get { return _specificopeningdate; }
        }
        /// <summary>
        /// 装修程度
        /// </summary>
        public string zhuangXiuChengDu
        {
            set { _fitment = value; }
            get { return _fitment; }
        }
        /// <summary>
        /// 400分机号
        /// </summary>
        public int? extension
        {
            set { _extension = value; }
            get { return _extension; }
        }
        /// <summary>
        /// 公摊
        /// </summary>
        public string gongTan
        {
            set { _pool = value; }
            get { return _pool; }
        }
        /// <summary>
        /// 论坛板块ID
        /// </summary>
        public int? BBSID
        {
            set { _bbsid = value; }
            get { return _bbsid; }
        }
        /// <summary>
        /// 微博ID
        /// </summary>
        public int? UID
        {
            set { _uid = value; }
            get { return _uid; }
        }

        /// <summary>
        /// 置顶时间
        /// </summary>
        public DateTime ZDdate
        {
            set { _zddate = value; }
            get { return _zddate; }
        }
        /// <summary>
        /// 封面图
        /// </summary>
        public string fengMian
        {
            set { _fengmian = value; }
            get { return _fengmian; }
        }
        /// <summary>
        /// 排序ID
        /// </summary>
        public int rowNumber
        {
            set { _rownumber = value; }
            get { return _rownumber; }
        }

        /// <summary>
        /// 排序方式
        /// </summary>
        public LouPanOrder Orderby
        {
            set { _orderby = value; }
            get { return _orderby; }
        }
        /// <summary>
        /// 问房数
        /// </summary>
        public int AskCount
        {
            set { _askcount = value; }
            get { return _askcount; }
        }
        /// <summary>
        /// 团购数
        /// </summary>
        public int TuanGouCount
        {
            set { _tuangoucount = value; }
            get { return _tuangoucount; }
        }
        /// <summary>
        /// BannerPicStartTime
        /// </summary>
        public DateTime? BannerPicStartTime
        {
            set { _bannerPicStartTime = value; }
            get { return _bannerPicStartTime; }
        }
        /// <summary>
        /// BannerPicEndTime
        /// </summary>
        public DateTime? BannerPicEndTime
        {
            set { _bannerPicEndTime = value; }
            get { return _bannerPicEndTime; }
        }
        /// <summary>
        /// 区域排名
        /// </summary>
        public string AreaRank { get; set; }

        /// <summary>
        /// 物业费
        /// </summary>
        public string WuyeMoney { get; set; }


        /// <summary>
        /// 产权年限
        /// </summary>
        public string PropertyYear { get; set; }

        /// <summary>
        /// 首页推荐
        /// </summary>
        public int Sytj { get; set; }

        /// <summary>
        /// 优惠动态
        /// </summary>
        public string Discount { get; set; }

        /// <summary>
        /// 优惠标题
        /// </summary>
        public string ShortDiscount { get; set; }

        /// <summary>
        /// 公交
        /// </summary>
        public string RoadName { get; set; }

        /// <summary>
        /// 楼吧楼盘id
        /// </summary>
        public int LbLoupanId { get; set; }
        
        /// <summary>
        /// 小图封面图
        /// </summary>
        public string SmallImgFenMian { get; set; }

        /// <summary>
        /// 审核人
        /// </summary>
        public string Checker { get; set; }
        /// <summary>
        /// 优惠开始时间
        /// </summary>
        public string DisCountBeginTime { get; set; }
        /// <summary>
        /// 优惠结束时间
        /// </summary>
        public string DisCountEndTime { get; set; }
        /// <summary>
        /// 房源
        /// </summary>
        public string HousenNum { get; set; }

        /// <summary>
        /// 佣金
        /// </summary>
        public string Yongjin { get; set; }

        /// <summary>
        /// 特色
        /// </summary>
        public string TagName { get; set; }

        /// <summary>
        /// 佣金推荐
        /// </summary>
        public int YongJinTj { get; set; }

        /// <summary>
        /// 成交数量
        /// </summary>
        public int DealCount { get; set; }

        /// <summary>
        /// 预定数量
        /// </summary>
        public int PayDepositCount { get; set; }

        /// <summary>
        /// 推荐数量
        /// </summary>
        public int TuiJianCount { get; set; }

        /// <summary>
        /// 佣金排序
        /// </summary>
        public int YongJinOrder { get; set; }

        /// <summary>
        /// 价格单位
        /// </summary>
        public string PriceUnit { get; set; }

        /// <summary>
        /// 佣金优惠结束时间
        /// </summary>
        public DateTime? YhEndTime { get; set; }

        /// <summary>
        /// 佣金优惠结束剩余的秒数
        /// </summary>
        public string YhRemainSeconds { get; set; }

        /// <summary>
        /// 算距离
        /// </summary>
        public int Distance { get; set; }

        /// <summary>
        /// 是否显示街景
        /// </summary>
        public int JieJingDisplay { get; set; }
        /// <summary>
        /// 街景参数Pano设置
        /// </summary>
        public string Pano { get; set; }
        /// <summary>
        /// 街景参数Heading设置
        /// </summary>
        public int Heading { get; set; }
        /// <summary>
        /// 街景参数Zoom设置
        /// </summary>
        public int Zoom { get; set; }
        /// <summary>
        /// 街景参数Pitch设置
        /// </summary>
        public int Pitch { get; set; }
        /// <summary>
        /// 二维码
        /// </summary>
        public string ErWeiCode { get; set; }

        /// <summary>
        /// 区域推荐
        /// </summary>
        public int QuYuTj { get; set; }

        /// <summary>
        /// 点评数
        /// </summary>
        public int DpCount { get; set; }

        /// <summary>
        /// 分数
        /// </summary>
        public int Score { get; set; }

        #endregion Model
    }


    

   
}
