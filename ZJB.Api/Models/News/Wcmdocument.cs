using System;

 namespace ZJB.Api.Model
{
     /// <summary>
     /// 文章内容实体类
     /// </summary>
     public class WcmDocument
     {  
         #region Model
         private int _docid;
         private int _docchannel;
         private int _docversion;
         private int _doctype;
         private string _doctitle;
         private int _docsource;
         private int _docsecurity;
         private int _docstatus;
         private int? _dockind;
         private string _doccontent;
         private string _dochtmlcon;
         private string _docabstract;
         private string _dockeywords;
         private string _docrelwords;
         private string _docpeople;
         private string _docplace;
         private string _docauthor;
         private string _doceditor;
         private string _docauditor;
         private int? _docoutupid;
         private DateTime? _docvalid;
         private string _docpuburl;
         private DateTime _docpubtime = DateTime.Now;
         private DateTime _docreltime = DateTime.Now;
         private string _cruser;
         private DateTime? _crtime;
         private int _docwordscount;
         private int? _docpro;
         private int _rightdefined;
         private string _titlecolor;
      
         private int? _schedule;
         private int _docflag;
         private string _editor;
         private string _attribute;
         private int _hitscount;
         private string _docpubhtmlcon;
         private string _subdoctitle;
         private int? _attachpic;
         private string _doclink;
         private string _docfilename;
         private string _lpname;
         private string _discount;
         private string _price;
         private string _zy;
         private int? _lparea;
         private int? _ispic;
         private int? _isvideo;
         private int? _xqid;
         private int? _lpid;
         private int? _ismood;
         private string _kdqm;
         private string _kdbj;
         private string _xwqm;
         private string _xwbj;
         private DateTime? _replaytime;
         private string _videourl;
         private int? _bbszd;
         private int _replaynum;
         private string _appfile;
         private string _channelname;
         private string _crusertype;
         private string _sjtitle;
#endregion


         #region
         /// <summary>
         /// 手机标题
         /// </summary>
         public string SjTitle { get; set; }

         /// <summary>
         /// 新闻ID
         /// </summary>
         public int DOCID
         {
             set { _docid = value; }
             get { return _docid; }
         }
         /// <summary>
         /// 新闻所属频道ID
         /// </summary>
         public int DOCCHANNEL
         {
             set { _docchannel = value; }
             get { return _docchannel; }
         }
         /// <summary>
         /// 文档的版本号
         /// </summary>
         public int DOCVERSION
         {
             set { _docversion = value; }
             get { return _docversion; }
         }
         /// <summary>
         /// 新闻类型
         /// </summary>
         public int DOCTYPE
         {
             set { _doctype = value; }
             get { return _doctype; }
         }
         /// <summary>
         /// 新闻标题
         /// </summary>
         public string DOCTITLE
         {
             set { _doctitle = value; }
             get { return _doctitle; }
         }
         /// <summary>
         /// 文档的来源编号
         /// </summary>
         public int DOCSOURCE
         {
             set { _docsource = value; }
             get { return _docsource; }
         }
         /// <summary>
         /// 文档的安全级别编号
         /// </summary>
         public int DOCSECURITY
         {
             set { _docsecurity = value; }
             get { return _docsecurity; }
         }
         /// <summary>
         /// 新闻状态
         /// </summary>
         public int DOCSTATUS
         {
             set { _docstatus = value; }
             get { return _docstatus; }
         }
         /// <summary>
         /// 文档的分类编号
         /// </summary>
         public int? DOCKIND
         {
             set { _dockind = value; }
             get { return _dockind; }
         }
         /// <summary>
         /// 新闻文本内容
         /// </summary>
         public string DOCCONTENT
         {
             set { _doccontent = value; }
             get { return _doccontent; }
         }
         /// <summary>
         ///  新闻HTML内容
         /// </summary>
         public string DOCHTMLCON
         {
             set { _dochtmlcon = value; }
             get { return _dochtmlcon; }
         }
         /// <summary>
         ///  新闻摘要
         /// </summary>
         public string DOCABSTRACT
         {
             set { _docabstract = value; }
             get { return _docabstract; }
         }
         /// <summary>
         /// 新闻关键字
         /// </summary>
         public string DOCKEYWORDS
         {
             set { _dockeywords = value; }
             get { return _dockeywords; }
         }
         /// <summary>
         /// 文档的相关词
         /// </summary>
         public string DOCRELWORDS
         {
             set { _docrelwords = value; }
             get { return _docrelwords; }
         }
         /// <summary>
         /// 文档涉及的人物
         /// </summary>
         public string DOCPEOPLE
         {
             set { _docpeople = value; }
             get { return _docpeople; }
         }
         /// <summary>
         /// 文档涉及的地点
         /// </summary>
         public string DOCPLACE
         {
             set { _docplace = value; }
             get { return _docplace; }
         }
         /// <summary>
         /// 新闻作者
         /// </summary>
         public string DOCAUTHOR
         {
             set { _docauthor = value; }
             get { return _docauthor; }
         }
         /// <summary>
         ///  新闻编辑
         /// </summary>
         public string DOCEDITOR
         {
             set { _doceditor = value; }
             get { return _doceditor; }
         }
         /// <summary>
         /// 新闻审核人
         /// </summary>
         public string DOCAUDITOR
         {
             set { _docauditor = value; }
             get { return _docauditor; }
         }
         /// <summary>
         /// 文档的输入编号
         /// </summary>
         public int? DOCOUTUPID
         {
             set { _docoutupid = value; }
             get { return _docoutupid; }
         }
         /// <summary>
         /// 文档的有效时间
         /// </summary>
         public DateTime? DOCVALID
         {
             set { _docvalid = value; }
             get { return _docvalid; }
         }
         /// <summary>
         /// 新闻发布地址
         /// </summary>
         public string DOCPUBURL
         {
             set { _docpuburl = value; }
             get { return _docpuburl; }
         }
         /// <summary>
         /// 新闻发布时间
         /// </summary>
         public DateTime DOCPUBTIME
         {
             set { _docpubtime = value; }
             get { return _docpubtime; }
         }
         /// <summary>
         /// 新闻更新时间
         /// </summary>
         public DateTime DOCRELTIME
         {
             set { _docreltime = value; }
             get { return _docreltime; }
         }
         /// <summary>
         /// 新闻创建者
         /// </summary>
         public string CRUSER
         {
             set { _cruser = value; }
             get { return _cruser; }
         }
         /// <summary>
         /// 文档的创建时间
         /// </summary>
         public DateTime? CRTIME
         {
             set { _crtime = value; }
             get { return _crtime; }
         }
         /// <summary>
         /// 文档的正文字数统计
         /// </summary>
         public int DOCWORDSCOUNT
         {
             set { _docwordscount = value; }
             get { return _docwordscount; }
         }
         /// <summary>
         /// 文档的属性
         /// </summary>
         public int? DOCPRO
         {
             set { _docpro = value; }
             get { return _docpro; }
         }
         /// <summary>
         ///文档的独立权限设置
         /// </summary>
         public int RIGHTDEFINED
         {
             set { _rightdefined = value; }
             get { return _rightdefined; }
         }
         /// <summary>
         /// 文档的标题颜色
         /// </summary>
         public string TITLECOLOR
         {
             set { _titlecolor = value; }
             get { return _titlecolor; }
         }
         /// <summary>
         /// 文档的计划发布任务编号
         /// </summary>
         public int? SCHEDULE
         {
             set { _schedule = value; }
             get { return _schedule; }
         }
         /// <summary>
         /// 文档的标记
         /// </summary>
         public int DOCFLAG
         {
             set { _docflag = value; }
             get { return _docflag; }
         }
         /// <summary>
         /// 文档编辑人员
         /// </summary>
         public string EDITOR
         {
             set { _editor = value; }
             get { return _editor; }
         }
         /// <summary>
         /// 文档的附加属性
         /// </summary>
         public string ATTRIBUTE
         {
             set { _attribute = value; }
             get { return _attribute; }
         }
         /// <summary>
         /// 点击率
         /// </summary>
         public int HitsCount
         {
             set { _hitscount = value; }
             get { return _hitscount; }
         }
         /// <summary>
         /// 发布的正文
         /// </summary>
         public string DOCPUBHTMLCON
         {
             set { _docpubhtmlcon = value; }
             get { return _docpubhtmlcon; }
         }
         /// <summary>
         /// 副标题
         /// </summary>
         public string SubDocTitle
         {
             set { _subdoctitle = value; }
             get { return _subdoctitle; }
         }
         /// <summary>
         /// 是否附属图片
         /// </summary>
         public int? ATTACHPIC
         {
             set { _attachpic = value; }
             get { return _attachpic; }
         }
         /// <summary>
         /// 文档链接地址
         /// </summary>
         public string DOCLINK
         {
             set { _doclink = value; }
             get { return _doclink; }
         }
         /// <summary>
         /// 外部文件名
         /// </summary>
         public string DOCFILENAME
         {
             set { _docfilename = value; }
             get { return _docfilename; }
         }
        
         /// <summary>
         /// 相关楼盘名
         /// </summary>
         public string LPNAME
         {
             set { _lpname = value; }
             get { return _lpname; }
         }
         
         /// <summary>
         /// 优惠
         /// </summary>
         public string DISCOUNT
         {
             set { _discount = value; }
             get { return _discount; }
         }
         /// <summary>
         /// 价格
         /// </summary>
         public string PRICE
         {
             set { _price = value; }
             get { return _price; }
         }
         /// <summary>
         /// 相关楼盘区域
         /// </summary>
         public int? LPAREA
         {
             set { _lparea = value; }
             get { return _lparea; }
         }
         /// <summary>
         /// 是否有图
         /// </summary>
         public int? isPic
         {
             set { _ispic = value; }
             get { return _ispic; }
         }
         /// <summary>
         /// 是否有视频
         /// </summary>
         public int? isVideo
         {
             set { _isvideo = value; }
             get { return _isvideo; }
         }
         /// <summary>
         /// 小区编号
         /// </summary>
         public int? XQID
         {
             set { _xqid = value; }
             get { return _xqid; }
         }
         /// <summary>
         /// 相关楼盘编号
         /// </summary>
         public int? LPID
         {
             set { _lpid = value; }
             get { return _lpid; }
         }
         /// <summary>
         /// 是否有心情选项
         /// </summary>
         public int? isMood
         {
             set { _ismood = value; }
             get { return _ismood; }
         }
         /// <summary>
         /// 快递签名
         /// </summary>
         public string KDQM
         {
             set { _kdqm = value; }
             get { return _kdqm; }
         }
         /// <summary>
         /// 快递编辑
         /// </summary>
         public string KDBJ
         {
             set { _kdbj = value; }
             get { return _kdbj; }
         }
         /// <summary>
         /// 新闻签名
         /// </summary>
         public string XWQM
         {
             set { _xwqm = value; }
             get { return _xwqm; }
         }
         /// <summary>
         /// 新闻编辑
         /// </summary>
         public string XWBJ
         {
             set { _xwbj = value; }
             get { return _xwbj; }
         }
         /// <summary>
         /// 最新回复时间
         /// </summary>
         public DateTime? REPLAYTIME
         {
             set { _replaytime = value; }
             get { return _replaytime; }
         }
         /// <summary>
         /// 视频地址
         /// </summary>
         public string VIDEOURL
         {
             set { _videourl = value; }
             get { return _videourl; }
         }
         /// <summary>
         /// bbs论坛新闻至顶用
         /// </summary>
         public int? bbszd
         {
             set { _bbszd = value; }
             get { return _bbszd; }
         }

         public int replaynum
         {
             set { _replaynum = value; }
             get { return _replaynum; }
         }

         public string APPFILE {
             set { _appfile = value; }
             get { return _appfile; }
         }

         public string CHANNELNAME
         {
             set { _channelname = value; }
             get { return _channelname; }
         }

         public string CRUSERTYPE
         {
             set { _crusertype = value; }
             get { return _crusertype; }
         }

         #endregion Model

     }

    public class MobileNews
    {
       
        public int NewsId { get; set; }
        public string ShowImg { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public int Hits { get; set; }
        public int Pls { get; set; }
        public double Replaytime { get; set; }
        public string CommentHtml { get; set; }

        public DateTime CRTIME { get; set; }

        public MobileNews()
        {
            NewsId = 0;
            ShowImg = "";
            Title = "";
            Hits = 0;
            Pls = 0;
            Replaytime =0;
            Content = "";
            CommentHtml = "";
        }
    }

    /// <summary>
    /// 快递实体类
    /// </summary>
    public class KuaidiInfo
     {
         /// <summary>
         /// 新闻标题
         /// </summary>
         public string Title { get; set; }
         /// <summary>
         /// 新闻内容(去除Html标签的)
         /// </summary>
         public string Content { get; set; }
         /// <summary>
         /// 新闻内容(包含Html标签)
         /// </summary>
         public string HtmlContent { get; set; }
         /// <summary>
         /// 新闻Id
         /// </summary>
         public string Id { get; set; }
         /// <summary>
         /// 作者
         /// </summary>
         public string Author { get; set; }
         /// <summary>
         /// 发布时间
         /// </summary>
         public string Reltime { get; set; }
         /// <summary>
         /// 新闻路径
         /// </summary>
         public string NewsUrl { get; set; }
         /// <summary>
         /// 楼盘Id
         /// </summary>
         public int LoupanId { get; set; }
         /// <summary>
         /// 楼盘名
         /// </summary>
         public string LoupanName { get; set; }
         /// <summary>
         /// 评论数量
         /// </summary>
         public int ReplyCount { get; set; }
         /// <summary>
         /// 新闻浏览量
         /// </summary>
         public int PageView { get; set; }
         /// <summary>
         /// 新闻的第一张图
         /// </summary>
         public string NewsFirstImages { get; set; }
         /// <summary>
         /// 发布人
         /// </summary>
         public string Issuer { get; set; }
     }


    /// <summary>
    /// 频道实体类
    /// </summary>
    public class DocChannel
    {
        /// <summary>
        /// 频道编号
        /// </summary>
        public int Channelid { get; set; }

        /// <summary>
        /// 所属分站
        /// </summary>
        public SiteID SiteID { get; set; }

        /// <summary>
        /// 频道名称
        /// </summary>
        public string ChnlName { get; set; }
        /// <summary>
        /// 频道描述
        /// </summary>
        public string ChnlDes { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CrateTime { get; set; }

        /// <summary>
        /// 创建者
        /// </summary>
        public string CrateUser { get; set; }
    }

    public enum SiteID
    {
        /// <summary>
        /// 厦门
        /// </summary>
        XM = 1,
        /// <summary>
        /// 漳州
        /// </summary>
        ZZ = 2,
        /// <summary>
        /// 泉州
        /// </summary>
        QZ = 3,
        /// <summary>
        /// 福州
        /// </summary>
        FZ = 4,
        /// <summary>
        /// 龙岩
        /// </summary>
        LY = 5,
        /// <summary>
        /// 莆田
        /// </summary>
        PT = 6,
        /// <summary>
        /// 三明
        /// </summary>
        SM = 7,
        /// <summary>
        /// 南平
        /// </summary>
        NP = 8,
        /// <summary>
        /// 宁德
        /// </summary>
        ND = 9
    }
    public enum newsOrder
    {
        /// <summary>
        /// 按发布时间
        /// </summary>
        RelTimeDesc = 1,
        /// <summary>
        /// 点击数
        /// </summary>
        HitsDesc = 2,
        /// <summary>
        /// 回复数
        /// </summary>
        ReplayNumDesc = 3,
        /// <summary>
        /// TRS排序
        /// </summary>
        Docorderpri = 4,
        CRTIME = 0,
    }
}
