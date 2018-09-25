using System;
using System.Activities.Statements;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using MongoDB.Bson;
using Newtonsoft.Json.Linq;
using Senparc.Weixin.MP.AdvancedAPIs.TemplateMessage;
using ZJB.Api.BLL;
using ZJB.Api.Common;
using ZJB.Api.Entity;
using ZJB.Api.Models;
using ZJB.Core.Utilities;
using ZJB.Pager;
using ZJB.Web.Utilities;
using ZJB.WX.Common;
using ZetaHtmlCompressor.Internal;

namespace ZJB.WX.Controllers
{
    public class EsfController : BaseController
    {
        protected readonly SmsApi smsApi = new SmsApi();
        private NCBaseRule ncBase = new NCBaseRule();
        private UserBll userBll = new UserBll();
        private HouseBll houseBll = new HouseBll();
        private WeiXinPublic wxPublic = new WeiXinPublic();
        private NoticeBll noticeBll = new NoticeBll();
        private string s_appid = ConfigurationManager.AppSettings["WeixinAppId"]; //服务号
        private string wx_secret = ConfigurationManager.AppSettings["WeixinAppSecret"];

     
        private string OrderTemp = "r-B4SS2pm7KMynsriSAQlNf045mO80l4h1rfiboxacM";
        /// <summary>
        /// 直约业主
        /// </summary>
        private string BespokeTemp = "hNBPR6KYN_5I1_w8pbihKmRRTwMM-pba0AYo3sN8POQ";



        public ActionResult Index()
        {
            //return RedirectToAction("Delegate", "Esf");
            HouseListReq parames = new HouseListReq();
            parames.postType = 0;
            parames.buildingType = 0;
            parames.buildingStatus = 1;
            parames.cell = "";
            parames.sort = 7;
            parames.houseId = 0;
            parames.title = "";
            parames.tags = "2";
            parames.page = 1;
            parames.pageSize = 20;
            parames.userId = 0;
            parames.collectuserid = 0;
            int totalSize = 0;
            ViewBag.Esflist = houseBll.GetEsfHouseList(parames, ref totalSize);
            @ViewBag.TotalSize = totalSize;
            return View();
        }

        #region 发布房源
        /// <summary>
        /// 委托
        /// </summary>
        /// <returns></returns>
        public ActionResult Delegate()
        {
            return View();
        }


        /// <summary>
        /// 发布页面
        /// </summary>
        /// <returns></returns>
        [Authorization]
        public ActionResult Publish(int posttype = 1, int buildingType = 1)
        {

            string tel = "";

            var user = userBll.GetUserById(this.GetLoginUser().UserID);
            if (user.IsNoNull())
            {
                if (string.IsNullOrEmpty(user.Tel) || string.IsNullOrWhiteSpace(user.Tel))
                {
                    return RedirectToAction("Tel", "Esf");
                }
                tel = user.Tel;
            }

            ViewBag.Tel = tel;
            ViewBag.Posttype = posttype;
            ViewBag.BuildingType = buildingType;
            return View();
        }
        #endregion

        #region 手机验证

        /// <summary>
        /// 手机验证
        /// </summary>
        /// <returns></returns>
        public ActionResult Tel()
        {
            var user = this.GetLoginUser();
            return View();
        }

        /// <summary>
        /// 验证码
        /// </summary>
        /// <param name="tel"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult IdentifyingCode(string tel)
        {
            Random rad = new Random(); //实例化随机数产生器rad；
            int code = rad.Next(1000, 10000);
            Session["code"] = code;
            noticeBll.SendUserCode(tel, code.ToString());
            return JsonReturnValue(new { Message = "请耐心等待验证码" }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult AddTel(string tel, string code)
        {
            string msg = "";
            int isuccess = 0;
            if (Session["code"].IsNoNull() && Session["code"].ToString() == code)
            {
                isuccess = 1;
                var user = this.GetLoginUser();
                if (user != null && user.UserID > 0)
                {
                    isuccess = userBll.UpdayePublicUserTel(user.UserID, tel);
                    String saveKey = System.Configuration.ConfigurationManager.AppSettings["AuthSaveKey"];

                    if (String.IsNullOrEmpty(saveKey))
                    {
                        saveKey = "WXLoginedUser";
                    }
                    Session[saveKey] = userBll.GetUserById(this.GetLoginUser().UserID);
                    HttpCookie loginUserCookie = new HttpCookie(saveKey,
                        CryptoUtility.TripleDESEncrypt(user.UserID.ToString()));
                    loginUserCookie.Expires = DateTime.Now.AddDays(10);
                    Response.Cookies.Add(loginUserCookie);
                }
                msg = "绑定成功";
            }
            else
            {
                msg = "验证码错误！";
            }
            return JsonReturnValue(new { Success = isuccess, Message = msg }, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region 直约业主
        /// <summary>
        /// 直约业主(用户电话号码没有填写)
        /// </summary>
        /// <param name="houseid"></param>
        /// <param name="tel">电话号码</param>
        /// <param name="code">验证码</param>
        /// <param name="tpye">类型：0：申请1：带看</param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult AddInterview(int houseid, int userid, string tel, string code, string buildarea = "",
            string huxing = "", string communityname = "", string oid = "", int tpye = 0)
        {
            string msg = "";
            string isuccess = "0";
            if (Session["code"].IsNoNull() && Session["code"].ToString() == code)
            {
                //isuccess = 1;
                var user = this.GetLoginUser();
                if (user != null && user.UserID > 0)
                {
                    isuccess = houseBll.SetInterview(houseid, user.UserID, tpye, tel);
                    String saveKey = System.Configuration.ConfigurationManager.AppSettings["AuthSaveKey"];


                    #region 给管理人员推送消息

                    var adminlist = userBll.getadminUserOpenid();
                    foreach (PublicUserModel useradmin in adminlist)
                    {

                        if (!string.IsNullOrEmpty(useradmin.OpenID) && !string.IsNullOrWhiteSpace(useradmin.OpenID))
                        {
                            try
                            {
                                wxPublic.WxPushMessage(useradmin.OpenID, "您好，您有一条新的看房预约消息。",
                                    communityname + "-" + huxing.Replace(" ", "") + "-" + buildarea + ""
                                    , user.NickName + "(" + user.Tel + ")", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), "", "http://www.zhujia001.com/Esf/Bespoke?houseid=" + houseid);
                            }
                            catch (Exception)
                            {
                                //WX.
                            }
                        }
                    }

                    #endregion

                    if (!string.IsNullOrEmpty(oid) && !string.IsNullOrWhiteSpace(oid))
                    {
                        #region 推送消息
                        HouseMessage message = new HouseMessage();
                        message.UserID = userid;
                        message.HouseID = houseid;
                        message.Type = (int)Message.Interview;
                        message.Message = "您好：您有一条新的看房预约消息。(" + communityname + "-" + huxing.Replace(" ", "") + "-" + buildarea + ")";
                        message.IsRead = 0;
                        message.AddDate = DateTime.Now;
                        ncBase.CurrentEntities.AddToHouseMessage(message);
                        ncBase.CurrentEntities.SaveChanges();
                        #endregion
                        wxPublic.WxPushMessage(oid, "您好，您有一条新的看房预约消息。",
                            communityname + "-" + huxing.Replace(" ", "") + "-" + buildarea + ""
                            , user.NickName + "(" + user.Tel + ")", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), "", "http://www.zhujia001.com/Esf/Bespoke?peruse=1&houseid=" + houseid);

                    }

                    if (String.IsNullOrEmpty(saveKey))
                    {
                        saveKey = "WXLoginedUser";
                    }
                    Session[saveKey] = userBll.GetUserById(user.UserID);
                    HttpCookie loginUserCookie = new HttpCookie(saveKey,
                        CryptoUtility.TripleDESEncrypt(user.UserID.ToString()));
                    loginUserCookie.Expires = DateTime.Now.AddDays(10);
                    Response.Cookies.Add(loginUserCookie);
                }
                if (isuccess == "-1")
                {
                    msg = "该房源已预约且有效，不能再次预约！";
                }
                else
                {
                    msg = "预约成功！";
                }

            }
            else
            {
                msg = "验证码错误！";
            }

            return JsonReturnValue(new { Success = isuccess, Message = msg }, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 直约业主（用户已经有电话号码）
        /// </summary>
        /// <param name="houseid"></param>
        /// <param name="tpye">类型：0：申请1：带看</param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult SetInterview(int houseid, int userid, string buildarea = "",
            string huxing = "", string communityname = "", string oid = "", int tpye = 0)
        {
            string msg = "";
            string isuccess = "0";
            //if (Session["code"].IsNoNull() && Session["code"].ToString() == code)
            {
                //isuccess = 1;
                var user = userBll.GetUserById(this.GetLoginUser().UserID);
                if (user != null && user.UserID > 0)
                {
                    isuccess = houseBll.SetInterview(houseid, user.UserID, tpye, user.Tel);
                    String saveKey = System.Configuration.ConfigurationManager.AppSettings["AuthSaveKey"];


                    #region 给管理人员推送消息

                    var adminlist = userBll.getadminUserOpenid();
                    foreach (PublicUserModel useradmin in adminlist)
                    {

                        if (!string.IsNullOrEmpty(useradmin.OpenID) && !string.IsNullOrWhiteSpace(useradmin.OpenID))
                        {
                            try
                            {
                                wxPublic.WxPushMessage(useradmin.OpenID, "您好，您有一条新的看房预约消息。",
                                    communityname + "-" + huxing.Replace(" ", "") + "-" + buildarea + ""
                                    , user.NickName + "(" + user.Tel + ")", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), "", "http://www.zhujia001.com/Esf/Bespoke?houseid=" + houseid);
                            }
                            catch (Exception)
                            {
                                //WX.
                            }
                        }
                    }



                    if (!string.IsNullOrEmpty(oid) && !string.IsNullOrWhiteSpace(oid))
                    {
                        #region 推送消息
                        HouseMessage message = new HouseMessage();
                        message.UserID = userid;
                        message.HouseID = houseid;
                        message.Type = (int)Message.Interview;
                        message.Message = "您好：您有一条新的看房预约消息。(" + communityname + "-" + huxing.Replace(" ", "") + "-" + buildarea + ")";
                        message.IsRead = 0;
                        message.AddDate = DateTime.Now;
                        ncBase.CurrentEntities.AddToHouseMessage(message);
                        ncBase.CurrentEntities.SaveChanges();
                        #endregion
                        wxPublic.WxPushMessage(oid, "您好，您有一条新的看房预约消息。",
                                communityname + "-" + huxing.Replace(" ", "") + "-" + buildarea + ""
                                , user.NickName + "(" + user.Tel + ")", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), "", "http://www.zhujia001.com/Esf/Bespoke?peruse=1&houseid=" + houseid);

                    }

                    #endregion

                    if (String.IsNullOrEmpty(saveKey))
                    {
                        saveKey = "WXLoginedUser";
                    }
                    Session[saveKey] = userBll.GetUserById(user.UserID);
                    HttpCookie loginUserCookie = new HttpCookie(saveKey,
                        CryptoUtility.TripleDESEncrypt(user.UserID.ToString()));
                    loginUserCookie.Expires = DateTime.Now.AddDays(10);
                    Response.Cookies.Add(loginUserCookie);
                }
                if (isuccess == "-1")
                {
                    msg = "该房源已预约且有效，不能再次预约！";
                }
                else
                {
                    msg = "预约成功！";
                }

            }
            //else
            //{
            //    msg = "验证码错误！";
            //}

            return JsonReturnValue(new { Success = isuccess, Message = msg }, JsonRequestBehavior.AllowGet);
        }

      
        [HttpPost]
        public ActionResult AddConsult(int houseid, string title, string tel, double price, int type = 0)
        {
            string msg = "";
            string isuccess = "0";
            var user = userBll.GetUserById(this.GetLoginUser().UserID);
            if (user != null && user.UserID > 0)
            {
                isuccess = houseBll.AddConsult(houseid, user.UserID, type, tel, price);
                String saveKey = System.Configuration.ConfigurationManager.AppSettings["AuthSaveKey"];

                #region 给管理人员推送消息

                var adminlist = userBll.getadminUserOpenid();
                foreach (PublicUserModel useradmin in adminlist)
                {

                    if (!string.IsNullOrEmpty(useradmin.OpenID) && !string.IsNullOrWhiteSpace(useradmin.OpenID))
                    {
                        try
                        {
                            wxPublic.WxPushMessage(useradmin.OpenID, "您好，您有一条新的房源报价消息", "对" + title + "给出报价"
                                , user.NickName + "(" + user.Tel + ")", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), "",
                                "http://www.zhujia001.com/Esf/Consult?houseid=" + houseid);
                        }
                        catch (Exception)
                        {
                            //WX.
                        }
                    }
                }

                #endregion

                if (String.IsNullOrEmpty(saveKey))
                {
                    saveKey = "WXLoginedUser";
                }
                Session[saveKey] = userBll.GetUserById(user.UserID);
                HttpCookie loginUserCookie = new HttpCookie(saveKey,
                    CryptoUtility.TripleDESEncrypt(user.UserID.ToString()));
                loginUserCookie.Expires = DateTime.Now.AddDays(10);
                Response.Cookies.Add(loginUserCookie);
            }
            msg = "提交成功！";
            return JsonReturnValue(new
            {
                Success = isuccess,
                Message = msg
            }, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region 获取地铁信息
        [HttpPost]
        public ActionResult Metro(int cityid, int line)
        {
            RegionsBll regionsBll = new RegionsBll();
            var list = ncBase.CurrentEntities.Metro.Where(a => a.Status == 1 && a.SiteId == cityid && a.Line == line);
            return JsonReturnValue(new { list = list }, JsonRequestBehavior.AllowGet);

        }
        #endregion

        #region 获取行政区域
        /// <summary>
        /// 获取地区区域
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Regions(int cityid, int distrctid)
        {
            RegionsBll regionsBll = new RegionsBll();
            var list = regionsBll.GetRegionList(cityid).Where(a => a.Layer == 3 && a.DistrctID == distrctid);
            return JsonReturnValue(new { list = list }, JsonRequestBehavior.AllowGet);

        }
        #endregion

        #region 发布房源 图片上传
        [Authorization]
        public ActionResult Pictures(int houseid)
        {
            ViewBag.UserID = this.GetLoginUser().UserID;
            if (houseid == 0)
            {
                return RedirectToAction("List");
            }
            HouseListReq parames = new HouseListReq();
            parames.postType = 0;
            parames.buildingType = 0;
            parames.buildingStatus = 0;
            parames.cell = "";
            parames.sort = 7;
            parames.houseId = houseid;
            parames.title = "";
            parames.tags = "";
            parames.page = 1;
            parames.pageSize = 1;
            parames.userId = 0;
            int totalSize = 0;
            //ViewBag.EsfInfo = houseBll.GetEsfHouseList(parames, ref totalSize);

            var user = this.GetLoginUser();
            var list = houseBll.GetEsfHouseList(parames, ref totalSize).ToList()[0];


            return View(list);
        }



        [HttpPost]
        public ActionResult AddHouseImage(int houseid, string imgurls, int imgtype = 1, int communityid = 0, int userid = 0)
        {
            int isuccess = houseBll.SetHouseImages(houseid, imgurls, imgtype, communityid, userid);
            return JsonReturnValue(new { Success = isuccess }, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region 房源发布结果
        /// <summary>
        /// 发布结果
        /// </summary>
        /// <returns></returns>
        public ActionResult Complete(int hid = 0, int type = 1)
        {
            ViewBag.Hid = hid;
            ViewBag.Type = type;
            return View();
        }
        #endregion

        #region 委托协议
        /// <summary>
        /// 委托协议
        /// </summary>
        /// <returns></returns>
        public ActionResult Protocol()
        {
            return View();
        }
        #endregion

        #region 出售 出租 录入 表单提交

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult OperateHouse(FormCollection form)
        {
            PublicUserModel loginUser = this.GetLoginUser();

            #region 基本信息

            int editType = 0;
            int.TryParse(form["editType"], out editType); //发布类型 0 正在发布  1 克隆

            int houseId = 0, buildingType, city, uid = loginUser.UserID;
            int.TryParse(form["houseId"], out houseId); //房源ID

            if (editType > 0)
            {
                houseId = 0; // 克隆 作为新增房源
            }
            int chouseId = 0;
            int.TryParse(form["chouseId"], out chouseId); //被克隆房源ID
            int.TryParse(form["buildingType"], out buildingType); //房屋类型
            int tradeType;
            int.TryParse(form["postType"], out tradeType); //发布类型
            int isChange = 0;
            int.TryParse(form["isChange"], out isChange); //表单是否被修改

            int.TryParse(form["city"], out city); //城市ID
            int communityId;
            int.TryParse(form["cellCode"], out communityId); // 小区ID
            string communityName = form["cell"]; //小区名
            if (communityId < 1)
                return Content("请选择我们提供的一个小区");

            int distrct;
            int.TryParse(form["distrct"], out distrct); //行政区

            int region;
            int.TryParse(form["area"], out region); //路段

            string address = form["addr"]; //地址

            double buildArea, usedArea;
            double.TryParse(form["houseArea"], out buildArea); //建筑面积
            double.TryParse(form["areaUsed"], out usedArea); //使用面积
            if (usedArea > buildArea && buildingType != 2) //别墅可以使用面积>建筑面积
                return Content("建筑面积必须大于使用面积");
            int room, hall, kitchen, toilet, balcony;
            int.TryParse(form["room"], out room); //房
            int.TryParse(form["hall"], out hall); //厅
            int.TryParse(form["kitchen"], out kitchen); //厨房
            int.TryParse(form["toilet"], out toilet); //卫生间
            int.TryParse(form["balcony"], out balcony); //阳台

            double price;
            double.TryParse(form["price"], out price); //价格

            double lowpay = 0; //最低首付


            string priceUnit; //价格单位
            double unitPrice = 0;
            string firstinfo = "";

            switch (tradeType)
            {
                case (int)TradeType.Sell:
                    priceUnit = "万";
                    unitPrice = buildArea > 0 ? price * 10000 / buildArea : 0; //出售计算单价
                    lowpay = price * 10000 * 0.30; //最低首付计算
                    firstinfo = "您好，有一条新的出售房源消息。";

                    break;
                case (int)TradeType.Rent:
                    firstinfo = "您好，有一条新的出租房源消息。";

                    switch (buildingType)
                    {
                        case (int)BuildingType.House:
                        case (int)BuildingType.Villa:
                            priceUnit = "元/月";
                            break;
                        default:
                            priceUnit = form["priceType"];
                            break;
                    }
                    break;

                default:
                    priceUnit = form["priceType"];
                    break;
            }

            string payType = form["payType"].IsNoNull() ? form["payType"] : ""; //支付方式

            int usedYear = 0;
            int.TryParse(form["ddlUsedYear"], out usedYear); //建造年代

            Int16 curFloor = 0;
            Int16.TryParse(form["curFloor"], out curFloor); //所在楼层

            Int16 maxFloor = 0;
            Int16.TryParse(form["maxFloor"], out maxFloor); //总层数

            if (curFloor > maxFloor)
                return Content("总层数必须大于所在楼层");

            string pointTo = form["pointTo"]; //朝向

            string lookTime = form["lookTime"]; //看房时间
            string fitmentStatus = form["fitmentStatus"]; //装修程度
            string internalNum = form["internalNum"]; //内部编号
            string cellLabel = form["cellLabel"]; //小区特色
            string houseLabel = form["houseLabel"]; //房源标签
            string yijuhua = form["yijuhua"]; //一句话广告 
            string video = form["video"]; //视频 
            string title = form["title"].IsNoNull() ? form["title"] : communityName; //房源标题

            string contacts = form["contacts"].IsNoNull() ? form["contacts"] : loginUser.NickName; //委托人
            string tel = form["tel"].IsNoNull() ? form["tel"] : loginUser.Tel; //委托人联系方式
            var compressor = new HtmlCompressor();
            compressor.setRemoveStyleAttributes(true);

            string houseDescribe = form["houseDescribe"].IsNoNull() ? compressor.compress(form["houseDescribe"]) : "";
            //房源描述
            string houseDescribeDelHtml = StringUtility.DelHtml(houseDescribe);
            if (houseDescribeDelHtml.Length > 3000)
                return Content("房源描述的字数为30-3000个字！");

            string imgUrlCover = ""; //房源图封面

            string imgUrl = form["imgUrl"].IsNoNull() ? form["imgUrl"] : ""; //房源图 
            string imageType = form["imageType"].IsNoNull() ? form["imageType"] : ""; //房源图类型
            string imgDescribe = "," + (form["imgDescribe"].IsNoNull() ? form["imgDescribe"] : ""); //房源图说明

            string[] imgUrlLists = imgUrl.Split(',');
            string[] imageTypeLists = imageType.Split(',');
            string[] imgDescribeLists = imgDescribe.Split(',');

            if (imgUrlLists.Length > 0 && !string.IsNullOrEmpty(imgUrlLists[0]))
            {
                imgUrlCover = imgUrlLists[0];
            }


            HouseBasicInfo houseBasicInfo = new HouseBasicInfo();


            if (houseId > 0)
            {
                houseBasicInfo = ncBase.CurrentEntities.HouseBasicInfo.Where(o => o.HouseID == houseId).FirstOrDefault();
                if (houseBasicInfo.IsNull())
                {
                    return Content("该房源不存在");
                }
            }
            Community communityItem =
                ncBase.CurrentEntities.Community.Where(o => o.CommunityID == communityId).FirstOrDefault();
            if (communityItem.IsNoNull() && string.IsNullOrEmpty(communityItem.Name))
            {
                communityName = communityItem.Name;
            }
            houseBasicInfo.UserID = uid; //用户ID
            houseBasicInfo.TradeType = BitConverter.GetBytes(tradeType)[0];
            houseBasicInfo.CityID = city;
            houseBasicInfo.Distrctid = distrct;
            houseBasicInfo.RegionID = region;
            houseBasicInfo.CommunityID = communityId;
            houseBasicInfo.CommunityName = communityName;
            houseBasicInfo.BuildType = BitConverter.GetBytes(buildingType)[0];
            houseBasicInfo.BuildArea = Convert.ToDecimal(buildArea);
            houseBasicInfo.UsedArea = Convert.ToDecimal(usedArea);
            ;
            houseBasicInfo.PointTo = pointTo;
            houseBasicInfo.UnitPrice = Convert.ToDecimal(unitPrice);
            houseBasicInfo.Price = Convert.ToDecimal(price);
            houseBasicInfo.PriceUnit = priceUnit;
            houseBasicInfo.CurFloor = curFloor;
            houseBasicInfo.MaxFloor = maxFloor;
            houseBasicInfo.UsedYear = usedYear;
            houseBasicInfo.ExpireDay = DateTime.Now.AddDays(30);
            houseBasicInfo.FitmentStatus = fitmentStatus;
            int picNum = imgUrlLists.Length > 1 ? imgUrlLists.Length - 1 : 0;
            houseBasicInfo.PicNum = BitConverter.GetBytes(picNum)[0];
            houseBasicInfo.Title = title;
            houseBasicInfo.Note = houseDescribe;
            houseBasicInfo.Status = BitConverter.GetBytes((int)HouseStatus.Draft)[0];
            houseBasicInfo.IP = IpUtility.GetIp();
            houseBasicInfo.PostTime = DateTime.Now;
            houseBasicInfo.Address = address;
            houseBasicInfo.LookHouseTime = lookTime;
            houseBasicInfo.HouseLabel = houseLabel;
            houseBasicInfo.InternalNum = internalNum;
            houseBasicInfo.CellLabel = cellLabel;
            houseBasicInfo.YiJuHua = yijuhua;
            houseBasicInfo.Video = video;
            houseBasicInfo.Room = BitConverter.GetBytes(room)[0];
            houseBasicInfo.Hall = BitConverter.GetBytes(hall)[0];
            houseBasicInfo.Kitchen = BitConverter.GetBytes(kitchen)[0];
            houseBasicInfo.Toilet = BitConverter.GetBytes(toilet)[0];
            houseBasicInfo.Balcony = BitConverter.GetBytes(balcony)[0];
            houseBasicInfo.PayType = payType;
            houseBasicInfo.HouseImgPath = imgUrlCover;
            houseBasicInfo.LowPay = Convert.ToDecimal(lowpay);
            houseBasicInfo.Source = (int)SourceType.Web;
            houseBasicInfo.Tel = tel;
            houseBasicInfo.Contacts = contacts;
            if (houseId.Equals(0)) //添加操作时候
            {

                if (editType == 1)
                {
                    houseBasicInfo.IsClone = true;
                }
                else
                {
                    houseBasicInfo.IsClone = false;
                }

                houseBasicInfo.BeColneHouseID = chouseId; //被克隆房源ID
                houseBasicInfo.Tag = "";
                houseBasicInfo.AddDate = DateTime.Now;
                houseBasicInfo.PushTime = Convert.ToDateTime("1900-1-1");
                houseBasicInfo.DeleteTime = Convert.ToDateTime("1900-1-1");
                ncBase.CurrentEntities.AddToHouseBasicInfo(houseBasicInfo);
            }
            ncBase.CurrentEntities.SaveChanges();

            houseId = houseBasicInfo.HouseID;

            #endregion

            #region 添加房源图片

            if (imgUrlLists.Length > 0 && imgUrlLists.Length == imageTypeLists.Length &&
                (imgUrlLists.Length == imgDescribeLists.Length) && isChange == 1)
            {
                HouseBll houseBll = new HouseBll();
                houseBll.DelHouseImageByHouseID(houseId, uid);
                for (int i = 0; i < imgUrlLists.Length; i++)
                {
                    bool value = false;
                    if (imgUrlLists.Length > 1)
                    {
                        for (int j = i + 1; j < imgUrlLists.Length; j++)
                        {
                            if (i != j && imgUrlLists[i] == imgUrlLists[j])
                            {
                                value = true;
                            }
                        }
                    }

                    if (value == false && !string.IsNullOrEmpty(imgUrlLists[i]))
                    {
                        bool isCover = false;
                        if (imgUrlCover.Equals(imgUrlLists[i]))
                            isCover = true;
                        HouseImage houseImage = new HouseImage();
                        houseImage.HouseID = houseId;
                        houseImage.ImagePath = imgUrlLists[i];
                        houseImage.ImagePos = imgDescribeLists[i];
                        houseImage.ImageType = HouseUtility.GetHouseImgType(imageTypeLists[i]);
                        houseImage.IsCover = isCover;
                        houseImage.OrderID = i;
                        houseImage.CommunityID = communityId;
                        houseImage.UserID = uid;
                        houseImage.AddTime = DateTime.Now;
                        houseImage.Status = BitConverter.GetBytes(1)[0];
                        ;
                        ncBase.CurrentEntities.AddToHouseImage(houseImage);
                        ncBase.CurrentEntities.SaveChanges();
                    }
                }
            }

            #endregion

            bool isAdd = false;

            #region 房屋类型扩展信息

            switch (buildingType)
            {
                case (int)BuildingType.House:

                    #region 住宅信息

                    string basicEquipHouse = form["basicEquip"].IsNoNull() ? form["basicEquip"] : ""; //住宅基础设施
                    string houseType = form["houseType"].IsNoNull() ? form["houseType"] : ""; //房屋类别
                    string houseSubType = form["houseSubType"].IsNoNull() ? form["houseSubType"] : ""; //住宅子类型
                    string houseProperty = form["houseProperty"].IsNoNull() ? form["houseProperty"] : ""; //房屋产权
                    string landYear = form["landYear"].IsNoNull() ? form["landYear"] : ""; //产权年限
                    string houseStructure = form["houseStructure"].IsNoNull() ? form["houseStructure"] : ""; //房屋结构
                    bool fiveYears = form["fiveYears"].IsNoNull() && form["fiveYears"] == "1"; //产证满二
                    bool onlyHouse = form["onlyHouse"].IsNoNull() && form["onlyHouse"] == "1"; //唯一住房
                    string advEquip = form["advEquip"].IsNoNull() ? form["advEquip"] : ""; //配套设施

                    HouseInfo houseInfo =
                        ncBase.CurrentEntities.HouseInfo.Where(o => o.HouseID.Equals(houseId)).FirstOrDefault();
                    if (houseInfo.IsNull())
                    {
                        houseInfo = new HouseInfo();
                        isAdd = true; //标记为新增加
                    }

                    houseInfo.HouseID = houseId;
                    houseInfo.HouseType = houseType;
                    houseInfo.HouseSubType = houseSubType;
                    houseInfo.HouseProperty = houseProperty;
                    houseInfo.LandYear = landYear;
                    houseInfo.HouseStructure = houseStructure;
                    houseInfo.FiveYears = fiveYears;
                    houseInfo.OnlyHouse = onlyHouse;
                    houseInfo.BasicEquip = basicEquipHouse;
                    houseInfo.AdvEquip = advEquip;

                    if (isAdd)
                    {
                        ncBase.CurrentEntities.AddToHouseInfo(houseInfo);
                    }
                    ncBase.CurrentEntities.SaveChanges();

                    #endregion

                    break;

                case (int)BuildingType.Villa:

                    #region 别墅信息

                    string villaType = form["villaType"].IsNoNull() ? form["villaType"] : ""; //别墅形式
                    string hallType = form["hallType"].IsNoNull() ? form["hallType"] : ""; //厅结构
                    string landYear2 = form["landYear2"].IsNoNull() ? form["landYear2"] : ""; //产权年限
                    bool fiveYears2 = form["fiveYears2"].IsNoNull() && form["fiveYears2"] == "1"; //产证满二
                    bool onlyHouse2 = form["onlyHouse2"].IsNoNull() && form["onlyHouse2"] == "1"; //唯一住房
                    bool basement = form["basement"].IsNoNull() && form["basement"] == "1"; //地下室
                    bool garden = form["garden"].IsNoNull() && form["garden"] == "1"; //花园
                    bool garage = form["garage"].IsNoNull() && form["garage"] == "1"; //车库
                    bool parkLot = form["parkLot"].IsNoNull() && form["parkLot"] == "1"; //停车位
                    string basicEquip1 = form["basicEquip1"].IsNoNull() ? form["basicEquip1"] : ""; //配套设施
                    string advEquip1 = form["advEquip1"].IsNoNull() ? form["advEquip1"] : ""; //室内设施

                    VillaInfo villaInfo =
                        ncBase.CurrentEntities.VillaInfo.Where(o => o.HouseID.Equals(houseId)).FirstOrDefault();
                    if (villaInfo.IsNull())
                    {
                        villaInfo = new VillaInfo();
                        isAdd = true; //标记为新增加
                    }

                    villaInfo.HouseID = houseId;
                    villaInfo.VillaType = villaType;
                    villaInfo.HallType = hallType;
                    villaInfo.LandYear = landYear2;
                    villaInfo.OnlyHouse = onlyHouse2;
                    villaInfo.FiveYears = fiveYears2;
                    villaInfo.Basement = basement;
                    villaInfo.Garden = garden;
                    villaInfo.Garage = garage;
                    villaInfo.ParkLot = parkLot;
                    villaInfo.BasicEquip = basicEquip1;
                    villaInfo.AdvEquip = advEquip1;

                    if (isAdd)
                    {
                        ncBase.CurrentEntities.AddToVillaInfo(villaInfo);
                    }
                    ncBase.CurrentEntities.SaveChanges();

                    #endregion

                    break;

                case (int)BuildingType.Shop:

                    #region 商铺信息

                    string shopType = form["shopType"].IsNoNull() ? form["shopType"] : "";
                    ; //商铺类型
                    string shopStatus = form["shopStatus"].IsNoNull() ? form["shopStatus"] : "";
                    ; //商铺状态
                    string targetField = form["targetField"].IsNoNull() ? form["targetField"] : "";
                    ; //目标业态
                    decimal feeShop;
                    decimal.TryParse(form["fee2"], out feeShop); //物业费
                    bool divideShop = form["divide2"].IsNoNull() && form["divide2"] == "1"; //是否分割
                    string basicEquip2 = form["basicEquip2"].IsNoNull() ? form["basicEquip2"] : ""; //配套设施

                    ShopInfo shopInfo =
                        ncBase.CurrentEntities.ShopInfo.Where(o => o.HouseID.Equals(houseId)).FirstOrDefault();
                    if (shopInfo.IsNull())
                    {
                        shopInfo = new ShopInfo();
                        isAdd = true; //标记为新增加
                    }

                    shopInfo.HouseID = houseId;
                    shopInfo.ShopType = shopType;
                    shopInfo.ShopStatus = shopStatus;
                    shopInfo.TargetField = targetField;
                    shopInfo.Fee = feeShop;
                    shopInfo.Divide = divideShop;
                    shopInfo.BasicEquip = basicEquip2;


                    if (isAdd)
                    {
                        ncBase.CurrentEntities.AddToShopInfo(shopInfo);
                    }
                    ncBase.CurrentEntities.SaveChanges();

                    #endregion

                    break;

                case (int)BuildingType.Office:

                    #region 写字楼信息

                    string officeType = form["officeType"].IsNoNull() ? form["officeType"] : ""; //写字楼类别
                    string officeLevel = form["officeLevel"].IsNoNull() ? form["officeLevel"] : ""; //写字楼级别
                    decimal fee3;
                    decimal.TryParse(form["fee3"], out fee3); //物业费
                    bool divide3 = form["divide3"].IsNoNull() && form["divide3"] == "1"; //是否分割
                    string basicEquip3 = form["basicEquip3"].IsNoNull() ? form["basicEquip3"] : ""; //配套设施


                    OfficeInfo officeInfo =
                        ncBase.CurrentEntities.OfficeInfo.Where(o => o.HouseID.Equals(houseId)).FirstOrDefault();
                    if (officeInfo.IsNull())
                    {
                        officeInfo = new OfficeInfo();
                        isAdd = true; //标记为新增加
                    }

                    officeInfo.HouseID = houseId;
                    officeInfo.OfficeType = officeType;
                    officeInfo.OfficeLevel = officeLevel;
                    officeInfo.BasicEquip = basicEquip3;
                    officeInfo.Fee = fee3;
                    officeInfo.Divide = divide3;


                    if (isAdd)
                    {
                        ncBase.CurrentEntities.AddToOfficeInfo(officeInfo);
                    }
                    ncBase.CurrentEntities.SaveChanges();

                    #endregion

                    break;
                case (int)BuildingType.Factory:

                    #region 厂房信息

                    string factoryType = form["factoryType"].IsNoNull() ? form["factoryType"] : ""; //厂房类别

                    string basicEquip4 = form["basicEquip4"].IsNoNull() ? form["basicEquip4"] : ""; //基础设施


                    FactoryInfo factoryInfo =
                        ncBase.CurrentEntities.FactoryInfo.Where(o => o.HouseID.Equals(houseId)).FirstOrDefault();
                    if (factoryInfo.IsNull())
                    {
                        factoryInfo = new FactoryInfo();
                        isAdd = true; //标记为新增加
                    }

                    factoryInfo.HouseID = houseId;
                    factoryInfo.BasicEquip = basicEquip4;
                    factoryInfo.FactoryType = factoryType;

                    if (isAdd)
                    {
                        ncBase.CurrentEntities.AddToFactoryInfo(factoryInfo);
                    }
                    ncBase.CurrentEntities.SaveChanges();

                    #endregion

                    break;
            }

            #endregion

            #region 给管理人员推送消息

            var adminlist = userBll.getadminUserOpenid();
            foreach (PublicUserModel adminuser in adminlist)
            {

                if (!string.IsNullOrEmpty(adminuser.OpenID) && !string.IsNullOrWhiteSpace(adminuser.OpenID))
                {
                    try
                    {

                        var newTemplate = new QuestionTemplateData()
                        {
                            //first = new TemplateDataItem(firstinfo), //您好！您收到一条回复信息
                            //keyword1 = new TemplateDataItem(houseBasicInfo.CommunityName),
                            //keyword2 = new TemplateDataItem(houseBasicInfo.BuildArea + "㎡"),
                            //keyword3 = new TemplateDataItem(houseBasicInfo.Price + houseBasicInfo.PriceUnit),
                            //keyword4 = new TemplateDataItem(houseBasicInfo.Contacts),
                            //remark = new TemplateDataItem("速去查看详情"),
                            first = new TemplateDataItem(firstinfo), //您好！您收到一条回复信息
                            keyword1 = new TemplateDataItem(houseBasicInfo.InternalNum),
                            keyword2 = new TemplateDataItem(houseBasicInfo.Title),
                            keyword3 = new TemplateDataItem(houseBasicInfo.AddDate.ToString()),
                            keyword4 = new TemplateDataItem(HDictionary.Instance.TradeType(TradeType.Sell.ToString())),
                            remark = new TemplateDataItem("速去查看详情"),
                        };
                        Task.Factory.StartNew(() => wxPublic.SendOrderStatusChangeMessag(s_appid, wx_secret, adminuser.OpenID, OrderTemp, newTemplate,
                            "http://www.zhujia001.com/Esf/Detail?houseid=" + houseBasicInfo.HouseID));
                    }
                    catch (Exception)
                    {
                        //WX.
                    }
                }
            }

            #endregion

            if (houseId > 0)
            {
                HouseBasicInfoShare itemShare =
                    ncBase.CurrentEntities.HouseBasicInfoShare.Where(o => o.ShareHouseId == chouseId).FirstOrDefault();
                if (itemShare.IsNoNull())
                {
                    itemShare.ShareCount = itemShare.ShareCount + 1;
                    ncBase.CurrentEntities.SaveChanges();
                }
            }
            //return RedirectToAction("Complete",
            //    new { posttype = tradeType, buildType = buildingType, editType = editType, buildingId = chouseId });
            return RedirectToAction("Pictures",
            new { houseid = houseId });
            // return JsonReturnValue(new { Message = "房源提交成功", id = houseId, }, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region 发布的房源列表页
        /// <summary>
        /// 委托管理
        /// </summary>
        /// <returns></returns>
        [Authorization]
        public ActionResult DelegateMng()
        {
            ViewBag.UserId = this.GetLoginUser().UserID;
            return View();
        }
        #endregion

        #region 发布的房源详细页

        /// <summary>
        /// 委托管理详细
        /// </summary>
        /// <returns></returns>
        /// 
        [Authorization]
        public ActionResult Detail(int houseid = 0)
        {
            if (houseid == 0)
            {
                return RedirectToAction("List");
            }
            HouseListReq parames = new HouseListReq();
            parames.postType = 0;
            parames.buildingType = 0;
            parames.buildingStatus = 0;
            parames.cell = "";
            parames.sort = 7;
            parames.houseId = houseid;
            parames.title = "";
            parames.tags = "";
            parames.page = 1;
            parames.pageSize = 1;
            parames.userId = 0;
            int totalSize = 0;
            //ViewBag.EsfInfo = houseBll.GetEsfHouseList(parames, ref totalSize);

            var user = this.GetLoginUser();
            var admin =
                ncBase.CurrentEntities.PublicUserManager.Where(o => o.UserID == user.UserID && o.Status == 1).ToList();
            var list = houseBll.GetEsfHouseList(parames, ref totalSize).ToList()[0];

            if ((admin.IsNoNull() && admin.Count > 0) || list.UserID == user.UserID)
            {
                ViewBag.Interview = houseBll.GetConsultInterview(houseid);
            }
            else
            {
                return RedirectToAction("List");
            }

            return View(list);
        }

        #endregion

        #region 房源列表
        /// <summary>
        /// 房源列表
        /// </summary>
        /// <returns></returns>
        public ActionResult List(int cityid = 592, string cell = "", int tradetype = 1, int ismetro = 0)
        {

            HouseListReq parames = new HouseListReq();
            parames.cell = cell;
            parames.cityId = cityid;
            parames.postType = tradetype;
            parames.ismetro = ismetro;

            RegionsBll regionsBll = new RegionsBll();
            var Regions = regionsBll.GetRegionList(cityid);
            ViewBag.RegionsArea = Regions.Where(a => a.Layer == 2); //行政区域
            return View(parames);
        }

        public ActionResult ZuList()
        {

            HouseListReq parames = new HouseListReq();
            parames.postType = 3;
            parames.buildingType = 0;
            parames.buildingStatus = 1;
            parames.cell = "";
            parames.sort = 7;
            parames.houseId = 0;
            parames.title = "";
            parames.tags = "";
            parames.page = 1;
            parames.pageSize = 20;
            parames.userId = 0;
            parames.collectuserid = 0;
            int totalSize = 0;
            ViewBag.Esflist = houseBll.GetEsfHouseList(parames, ref totalSize);
            @ViewBag.TotalSize = totalSize;
            return View();
        }

        [HttpPost]
        public ActionResult GetEsfList(int tradetype = 0, int buildtype = 0, int status = 1, string cell = "",
          int sort = 7, int houseId = 0,
          string title = "", string tags = "", int pageIndex = 1, int pageSize = 10, int userId = 0, int regionid = 0,
          int districtid = 0, int room = 0,
          int minprice = 0, int maxprice = 0, int line = 0, int metroid = 0, int ismetro = 0, int iscollect = 0, int collectuserid = 0, int IsBrowse = 0)
        {
            HouseListReq parames = new HouseListReq();
            parames.postType = tradetype;
            parames.buildingType = buildtype;
            parames.buildingStatus = status;
            parames.cell = cell;
            parames.sort = sort;
            parames.houseId = houseId;
            parames.title = title;
            parames.tags = tags;
            parames.page = pageIndex;
            parames.pageSize = pageSize;
            parames.userId = userId;
            parames.roomType = room;
            parames.districtId = districtid;
            parames.regionId = regionid;
            parames.maxPrice = maxprice;
            parames.minPrice = minprice;
            parames.collectuserid = collectuserid;
            parames.ismetro = ismetro;
            parames.line = line;
            parames.metroid = metroid;
            parames.iscollect = iscollect;
            parames.IsBrowse = IsBrowse;

            int totalSize = 0;

            var list = houseBll.GetEsfHouseList(parames, ref totalSize);
            return JsonReturnValue(new { totalSize = totalSize, list = list }, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region 房源详细页
        [Authorization]
        public ActionResult EsfDetail(int houseid = 0)
        {

            if (houseid < 1)
            {
                return RedirectToAction("Index", "Esf");
            }
            var user = this.GetLoginUser();
            ViewBag.UserInfo = user;
            var userid = user.UserID;
            HouseListReq parames = new HouseListReq();
            parames.postType = 0;
            parames.buildingType = 0;
            parames.buildingStatus = 1;
            parames.cell = "";
            parames.sort = 7;
            parames.houseId = houseid;
            parames.title = "";
            parames.tags = "";
            parames.page = 1;
            parames.pageSize = 1;
            parames.userId = 0;
            parames.collectuserid = userid;
            int totalSize = 0;
            var list = houseBll.GetEsfHouseList(parames, ref totalSize);
            if (list.IsNoNull() && list.Count > 0)
            {
                //houseBll.SetHouseHits(houseid);

                var img = ncBase.CurrentEntities.HouseImage.Where(o => o.HouseID == houseid && o.Status == 1).ToList();
                ViewBag.Images = img;
                ViewBag.ImgSum = img.Count;
                parames.houseId = 0;
                parames.pageSize = 5;
                parames.districtId = list[0].Distrctid;
                //  parames.cell = list[0].CommunityName;
                //var CommunityHouse =
                //    houseBll.GetEsfHouseList(parames, ref totalSize).Where(a => a.HouseID != houseid).ToList();
                //if (CommunityHouse.IsNoNull() && CommunityHouse.Count > 0)
                //{
                //    ViewBag.CommunityHouse = CommunityHouse[0];
                //}
                parames.postType = list[0].TradeType;
                ViewBag.CommunityHouse =
                    houseBll.GetEsfHouseList(parames, ref totalSize).Where(a => a.HouseID != houseid);
            }
            else
            {
                return RedirectToAction("Index", "Esf");
            }

            houseBll.AddBrowse(houseid, userid, 1);
            int readnum = 0;
            var Browse = houseBll.GetBrowse(houseid, 1, 20, ref readnum);
            readnum = list[0].Hits;
            if (Browse.Count < 20 && readnum > Browse.Count)
            {
                const string vestKey = "userpicKey";
                JObject pic_list = null;
                if (HttpContext.Cache[vestKey] == null)
                {
                    string jsPath = HttpContext.Server.MapPath("/Scripts/Esf/userpic.js");//你要保证文件名及路径正确。
                    string jsContent = System.IO.File.ReadAllText(jsPath, System.Text.Encoding.UTF8);
                    jsContent = jsContent.Replace("var", "").Replace("pic_json", "").Replace("=", "");
                    pic_list = JObject.Parse(jsContent);
                    if (pic_list != null)
                    {
                        HttpContext.Cache.Insert(vestKey, pic_list, null, DateTime.Now.AddMinutes(5), TimeSpan.Zero);
                    }
                }
                else
                {
                    pic_list = HttpContext.Cache[vestKey] as JObject;
                }
                int imax = readnum > 20 ? 20 : readnum;
                for (int i = Browse.Count; i < imax; i++)
                {
                    Random ran = new Random();
                    int image = ran.Next(1, 7800) + i;
                    var userImg = pic_list.SelectToken("list")[image].SelectToken("pic").ToString();
                    Browse.Add(new PublicUserModel()
                    {
                        Portrait = userImg
                    });
                }
            }

            ViewBag.Browse = Browse;
            ViewBag.ReadNum = readnum;

            ViewBag.Tel = userBll.GetUserById(userid).Tel;
            return View(list[0]);
        }
        #endregion

        #region 小区详细页

        [Authorization]
        public ActionResult CommunityDetail(int id)
        {
            var data = ncBase.CurrentEntities.Community.FirstOrDefault(p => p.CommunityID == id);

            if (data == null)
            {
                return Content("小区不存在！");
            }

            int countesf = ncBase.CurrentEntities.HouseBasicInfo.Where(o => o.CommunityID == id && o.Status == 1 && o.TradeType == 1).Count();
            this.ViewBag.HouseCount = countesf;
            this.ViewBag.HistoryPriceList = houseBll.GetLpHistoryPriceList(id);

            return View(data);

        }
        #endregion

        #region 磋商报价
        /// <summary>
        /// 磋商报价
        /// </summary>
        /// <param name="houseid"></param>
        /// <returns></returns>
        [Authorization]
        public ActionResult Consult(int houseid = 0)
        {
            var user = this.GetLoginUser();
            var admin =
                ncBase.CurrentEntities.PublicUserManager.Where(o => o.UserID == user.UserID && o.Status == 1).ToList();
            if (admin.IsNoNull() && admin.Count > 0)
            {
                ViewBag.Interview = houseBll.GetConsult(houseid);
            }
            else
            {
                return RedirectToAction("List");
            }
            return View();
        }
        #endregion

        #region 设置收藏

        [HttpPost]
        public ActionResult SetCollect(int houseid, int type = 1)
        {
            houseBll.SetCollect(houseid, this.GetLoginUser().UserID, type);
            return JsonReturnValue(new { success = 1 }, JsonRequestBehavior.AllowGet);

        }
        #endregion

        #region 个人中心
        [Authorization]
        public ActionResult Me()
        {
            var user = this.GetLoginUser();
            ViewBag.MessNum = ncBase.CurrentEntities.HouseMessage.Where(o => o.IsRead == 0 && o.UserID == user.UserID).Count();
            return View(user);
        }
        #endregion

        #region 我的收藏
        [Authorization]
        public ActionResult Collect()
        {
            ViewBag.UserId = this.GetLoginUser().UserID;

            return View();
        }
        #endregion

        #region 我的浏览记录
        [Authorization]
        public ActionResult Browse()
        {
            ViewBag.UserId = this.GetLoginUser().UserID;

            return View();
        }
        #endregion

        #region 举报虚假
        public ActionResult Accusation(int tpye = 1, int houseid = 0)
        {
            var user = this.GetLoginUser();
            ViewBag.UserId = user.IsNoNull() ? 0 : user.UserID;
            ViewBag.Tel = user.Tel.IsNull() ? "" : user.Tel; ;
            ViewBag.HouseId = houseid;
            ViewBag.Tpye = tpye;
            var acc = ncBase.CurrentEntities.Accusation.Where(o => o.Tpye == tpye && o.Status == 1).ToList();
            return View(acc);
        }

        /// <summary>
        /// 房源举报
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult AddJuBao(int aid = 0, int houseid = 0, int type = 1, string contents = "", string tel = "", string code = "")
        {
            int isuccess = 0;
            string msg = "举报成功!!";
            if ((Session["code"].IsNoNull() && Session["code"].ToString() == code) || (code == "true"))
            {
                int userid = 0;
                isuccess = houseBll.AddAccusationLog(aid, userid, houseid, type, contents, tel);
                if (isuccess < 1)
                {
                    msg = "举报失败!!";
                }
            }
            else
            {
                msg = "验证码错误!!";
            }
            return JsonReturnValue(new
            {
                Success = isuccess,
                Message = msg
            }, JsonRequestBehavior.AllowGet);

        }
        #endregion

        #region 我的消息
        [Authorization]
        public ActionResult MeMessage()
        {
            var user = this.GetLoginUser();

            ViewBag.UserID = user.UserID;
            return View();
        }
        #endregion

        #region 获取我的消息
        /// <summary>
        /// 获取我的消息
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult GetMeMessage(int pageIndex = 1, int pageSize = 10, int userId = 0, int isread = 0)
        {
            var mes = ncBase.CurrentEntities.HouseMessage.Where(o => o.IsRead == isread && o.UserID == userId).OrderByDescending(o => o.AddDate);
            int totalSize = mes.Count();
            return JsonReturnValue(new { totalSize = totalSize, list = mes.ToPagedList(pageIndex, pageSize) }, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region 房源报价详情
        /// <summary>
        /// 
        /// </summary>
        /// <param name="houseid"></param>
        /// <returns></returns>
        public ActionResult Bespoke(int houseid = 0, int peruse = 0)
        {
            var user = this.GetLoginUser();
            var admin =
                ncBase.CurrentEntities.PublicUserManager.Where(o => o.UserID == user.UserID && o.Status == 1).ToList();
            var Interview = houseBll.GetInterview(houseid);
            if (admin.IsNoNull() && admin.Count > 0)
            {
                ViewBag.Interview = Interview;
            }
            else
            {

                if (Interview != null && Interview.Tables.Count > 0 &&
                    Interview.Tables[0].Rows.Count > 0 &&
                    Interview.Tables[0].Rows[0]["UserID"].ToString() == user.UserID.ToString())
                {
                    if (peruse > 0)
                    {
                        var mess = ncBase.CurrentEntities.HouseMessage.Where(o => o.HouseID == houseid && o.UserID == user.UserID);
                        if (mess.IsNoNull())
                        {
                            foreach (HouseMessage ms in mess)
                            {
                                var updatemes = ncBase.CurrentEntities.HouseMessage.Where(o => o.HouseID == houseid && o.UserID == user.UserID && o.ID == ms.ID).FirstOrDefault();
                                updatemes.IsRead = 1;

                            }
                            ncBase.CurrentEntities.SaveChanges();
                        }
                    }

                    ViewBag.Interview = Interview;
                    return View();
                }

                return RedirectToAction("List");
            }

            return View();
        }
        #endregion

        #region 报备客户
        [Authorization]
        public ActionResult AddSales(int hid = 0)
        {
            ICacheManager cache = CacheFactory.GetInstance();
            //Credential loginUser = this.GetLoginUser();
            cache.Remove(string.Format(CacheItemConstant.UserModelItem, this.GetLoginUser().UserID));
            var loginUser = this.GetLoginUser();
            if (loginUser.VipType != 99)
            {
                return Redirect("/esf/EsfDetail?houseid="+hid);
            }
      
            ViewBag.Hid = hid;
   
            return View(loginUser);
        }

        [HttpPost]
        [Authorization]
        public ActionResult AddBaoBei(int hid = 0,  string FName = "", string FTel = "", decimal Price=0,
                                                int sex = 1,  string AppointmentTime = "", string Notes = "")
        {
            var user = this.GetLoginUser();
            string jsons = string.Empty;
            try
            {
                string sexname;
                if (sex == 1)
                {
                    sexname = "先生";
                }
                else
                {
                    sexname = "女士";
                }
                if (string.IsNullOrEmpty(FName))
                {
                    jsons = "{\"success\":\"-2\",\"msg\":\"客户姓名不能为空！！\"}";
                }
                else if (string.IsNullOrEmpty(FTel))
                {
                    jsons = "{\"success\":\"-2\",\"msg\":\"电话不能为空！！\"}";
                }
                else
                {
                    int cfid = 0;
                    var isuccess = houseBll.AddBaoBei(hid, FName, user.UserID, 
                                                      FTel, Price,   sex, AppointmentTime, Notes);
                    if (isuccess > 0)
                    {
                      
                        jsons = "{\"success\":\"1\",\"msg\":\"\"}";
                    }
                    else
                    {
                       
                        jsons = "{\"success\":\"" + isuccess + "\",\"error\":\"\"}";
                    }
                }
            }
            catch (Exception ex)
            {
                jsons = "{\"success\":\"0\",\"msg\":\"" + ex.ToString() + "\"}";
                throw;
            }
            Response.ContentType = "text/plain";
            string callbackFunName = Request["success_jsonpCallback"];
            string json = "[" + jsons + "]";
            json = string.Format("{0}({1})", Request["success_jsonpCallback"], json);
            return Content(json);
        }


        #endregion

        #region 我的客户列表
        [Authorization]
        public ActionResult BaoBeiList(int st = 0)
        {
            var user = this.GetLoginUser();
            ViewBag.State = st;
            return View(user);
        }

        [Authorization]
        public JsonResult GetBaoBeiList(int state = 0, int pageindex = 1, int pagesize = 10)
        {

            
            var user = this.GetLoginUser();
            int userid = user.UserID;
            var list = houseBll.GetBaobeiList(userid: userid, state: state, pageindex: pageindex, pagesize: pagesize);
            return Json(list);
        } 
        #endregion

        #region 我的客户详细
       [Authorization]
        public ActionResult BaoBeiDetail(int tjid)
        {
            var user = this.GetLoginUser();
            
            return View();
        }
        #endregion

        #region 状态操作
        /*  
          /// <summary>
          /// 
          /// </summary>
          /// <param name="id"></param>
          /// <param name="type">1.为推荐审核，2</param>
          /// <returns></returns>
          [Authorization]
          public ActionResult BBOperate(int id = 0, int type = 1)
          {
              //Credential loginUser = this.GetLoginUser();
              //cache.Remove(string.Format(CacheItemConstant.UserModelItem, this.GetLoginUser().UserID));
              Credential loginUser = user.GetCredentialByUserId(this.GetLoginUser().UserID);
              var flag = 0;
              var ds = loupanBll.SetUserRelation(loginUser.UserID, lpid);
              if (loginUser.UserType != "3" && !(ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0))
              {
                  if (!(type == 2))
                  {
                      return Redirect("/User/me");
                  }
              }
              else
              {
                  flag = 1;
              }
              if (type == 2)
              {
                  ViewBag.Visit = tuijin.GetTuiJianVisit(tjid);
              }
              if (type == 3)
              {
                  ViewBag.PayDeposit = tuijin.GetTuiJianPayDeposit(tjid);
              }
              ViewBag.Tjid = tjid;
              ViewBag.Type = type;
              ViewBag.UserName = loginUser.UserName;
              ViewBag.Flag = flag;
              return View(loginUser);
          }
          */
        #endregion

    }
}
