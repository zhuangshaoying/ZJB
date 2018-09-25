using System;
using System.Activities.Expressions;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Web.Caching;
using System.Web.Mvc;
using System.Web.Routing;
using ZJB.Api.BLL;
using ZJB.Api.Common;
using ZJB.Api.Entity;
using ZJB.Api.Models;
using ZJB.Api.Models.Community;
using ZJB.Core.Utilities;
using ZJB.WX.Common;
using ZJB.WX.Common.xms;
using ZJB.WX.Filters;
using ZJB.WX.Models;
using ZJB.Pager;
using ZJB.Web.Utilities;
using ZetaHtmlCompressor.Internal;
using MongoDB.Driver;
using ZJB.Api.Models.Parame;
using System.Net;
namespace ZJB.WX.Controllers
{

    [Authorization]
    public class HouseController : BaseController
    {
        MogoHelper mogoHelper = new MogoHelper();
        private UserTaskLogBll userTaskLog = new UserTaskLogBll();
        private NCBaseRule ncBase = new NCBaseRule();
        private UserBll userBll = new UserBll();
        private RegionsBll regionBll = new RegionsBll();


        [AllowAnonymous]
        [ActionLog(CheckPoints = false)]
        [OutputCache(Duration = 60 * 60)]
        public ActionResult Index()
        {

            PublicUserModel loginUser = this.GetLoginUser();
            ViewData["LoginUser"] = loginUser;
            int num = 4;
            string sqlwhere = "it.Status=1 ";
            List<HouseBasicInfo> houseBasicInfos =
                ncBase.CurrentEntities.HouseBasicInfo.Where(sqlwhere)
                       .OrderByDescending(o => Guid.NewGuid())
                    .Take(num)
                    .ToList();
            return View(houseBasicInfos);
        }

        #region 房源列表
        [AllowAnonymous]
        [ActionLog(CheckPoints = false)]
        public ActionResult HouseList(HouseParameter houseParameter)
        {
            PublicUserModel loginUser = this.GetLoginUser();
            ViewData["LoginUser"] = loginUser;
            HouseBll houseBll = new HouseBll();
            int page = houseParameter.Page ?? 1;
            int pageSize = houseParameter.PageSize ?? 20;
            string cityName = "厦门";
            Regions region = ncBase.CurrentEntities.Regions.Where(o => o.ShortSpell == houseParameter.City).FirstOrDefault();
            int cityId = 0;
            if (region.IsNull())
            {
                cityId = 592;
            }
            else
            {
                cityName = region.Name;
                cityId = region.RegionID;
            }
            int tradeType = 1;
            switch (houseParameter.Trade)
            {
                case "esf":
                    tradeType = 1; break;
                case "zf":
                    tradeType = 3; break;
            }

            int rowsCount = 0;


            HouseParame houseParame = new HouseParame
            {
                CommunityId = 0,
                CityId = cityId,
                TradeType = tradeType,
                Distrctid = houseParameter.Distrct ?? 0,
                Title = houseParameter.KeyWord,
                RegionId = houseParameter.Region ?? 0,
                MinPrice = houseParameter.MinPrice ?? 0,
                MaxPrice = houseParameter.MaxPrice ?? 0,
                MinArea = houseParameter.MinArea ?? 0,
                MaxArea = houseParameter.MaxArea ?? 0,
                Room = houseParameter.Layout ?? 0,
                PointTo = "",
                UsedYear = 0,
                CurFloor = 0,
                Tag = "",
                HouseOrder = houseParameter.OrderBy ?? 0,
                UserId = 0,
                PageIndex = page,
                PageSize = pageSize,
                BuildType = houseParameter.BuildType ?? 1,
            };
            List<HouseBasicInfoModel> houseBasicInfos = houseBll.GetHouseBasicInfoList(houseParame, out rowsCount);//房源列表


            ViewData["rowsCount"] = rowsCount; //总条数
            PagedList<HouseBasicInfoModel> houseInfoList = new PagedList<HouseBasicInfoModel>(houseBasicInfos, page, pageSize, rowsCount);
            ViewBag.Parameter = houseParameter; //参数

            List<Regions> distrcs =
                ncBase.CurrentEntities.Regions.Where(o => o.CityID == cityId && o.Layer == 2).ToList();
            List<Regions> regions = new List<Regions>();

            if (houseParameter.Distrct > 0)
            {
                regions =
                ncBase.CurrentEntities.Regions.Where(o => o.CityID == cityId && o.DistrctID == houseParameter.Distrct && o.Layer == 3).ToList();
            }
            ViewBag.Regions = regions;
            ViewBag.Distrcs = distrcs;
            ViewBag.CityName = cityName;
            ViewData["TradeType"] = tradeType;
            return View(houseInfoList);


        }

        #endregion

        #region 房源详细页
        [AllowAnonymous]
        [ActionLog(CheckPoints = false)]
        public ActionResult Detail(int id)
        {
            PublicUserModel loginUser = this.GetLoginUser();
            ViewData["LoginUser"] = loginUser;
            HouseBasicInfo houseBasicInfo = new HouseBasicInfo();
            List<HouseImage> houseImages = new List<HouseImage>();
            HouseInfo houseInfo = new HouseInfo();
            VillaInfo villaInfo = new VillaInfo();
            ShopInfo shopInfo = new ShopInfo();
            OfficeInfo officeInfo = new OfficeInfo();
            FactoryInfo factoryInfo = new FactoryInfo();
            HouseModels houseModels = new HouseModels();
            VPublicUser userInfo = new VPublicUser();
            Community community = new Community();
            if (id > 0)
            {
                houseBasicInfo =
                   ncBase.CurrentEntities.HouseBasicInfo.Where(o => o.HouseID == id).FirstOrDefault();

                if (houseBasicInfo.IsNull()) //房屋类型不匹配 或者发布类型不匹配
                {
                    houseBasicInfo = new HouseBasicInfo();
                    houseModels.HouseBasicInfo = houseBasicInfo;
                    houseModels.HouseInfo = houseInfo;
                    houseModels.HouseImages = houseImages;
                    houseModels.VillaInfo = villaInfo;
                    houseModels.ShopInfo = shopInfo;
                    houseModels.OfficeInfo = officeInfo;
                    houseModels.FactoryInfo = factoryInfo;
                    houseModels.UserInfo = userInfo;
                    houseModels.Community = community;
                    ViewData["City"] = "xm";
                    return View(houseModels);
                }
                Regions region = ncBase.CurrentEntities.Regions.Where(o => o.RegionID == houseBasicInfo.CityID).FirstOrDefault();
                ViewData["City"] = region.IsNoNull() ? region.ShortSpell : "xm";


                int buildingType = houseBasicInfo.BuildType;
                houseImages =
                    ncBase.CurrentEntities.HouseImage.Where(o => o.HouseID == id && o.Status == 1).ToList();
                userInfo =
                  ncBase.CurrentEntities.VPublicUser.Where(o => o.UserID == houseBasicInfo.UserID).FirstOrDefault();
                community =
                    ncBase.CurrentEntities.Community.Where(o => o.CommunityID == houseBasicInfo.CommunityID)
                        .FirstOrDefault();
                switch (buildingType)
                {
                    case (int)BuildingType.House:
                        houseInfo = ncBase.CurrentEntities.HouseInfo.Where(o => o.HouseID.Equals(id)).FirstOrDefault();
                        if (houseInfo.IsNull()) houseInfo = new HouseInfo();
                        break;
                    case (int)BuildingType.Villa:
                        villaInfo = ncBase.CurrentEntities.VillaInfo.Where(o => o.HouseID.Equals(id)).FirstOrDefault();
                        if (villaInfo.IsNull()) villaInfo = new VillaInfo();
                        break;
                    case (int)BuildingType.Shop:
                        shopInfo = ncBase.CurrentEntities.ShopInfo.Where(o => o.HouseID.Equals(id)).FirstOrDefault();
                        if (shopInfo.IsNull()) shopInfo = new ShopInfo();
                        break;
                    case (int)BuildingType.Office:
                        officeInfo = ncBase.CurrentEntities.OfficeInfo.Where(o => o.HouseID.Equals(id)).FirstOrDefault();
                        if (officeInfo.IsNull()) officeInfo = new OfficeInfo();
                        break;
                    case (int)BuildingType.Factory:
                        factoryInfo = ncBase.CurrentEntities.FactoryInfo.Where(o => o.HouseID.Equals(id)).FirstOrDefault();
                        if (factoryInfo.IsNull()) factoryInfo = new FactoryInfo();
                        break;
                }
            }

            houseModels.HouseBasicInfo = houseBasicInfo;
            houseModels.HouseInfo = houseInfo;
            houseModels.HouseImages = houseImages;
            houseModels.VillaInfo = villaInfo;
            houseModels.ShopInfo = shopInfo;
            houseModels.OfficeInfo = officeInfo;
            houseModels.FactoryInfo = factoryInfo;
            houseModels.UserInfo = userInfo;
            houseModels.Community = community;
            houseBasicInfo.Hits = (houseBasicInfo.Hits ?? 0) + 1;
            ncBase.CurrentEntities.SaveChanges();
            return View(houseModels);

        }
        [AllowAnonymous]
        public ActionResult Mobile(int id)
        {
            HouseBasicInfo houseBasicInfo = new HouseBasicInfo();
            List<HouseImage> houseImages = new List<HouseImage>();
            HouseInfo houseInfo = new HouseInfo();
            VillaInfo villaInfo = new VillaInfo();
            ShopInfo shopInfo = new ShopInfo();
            OfficeInfo officeInfo = new OfficeInfo();
            FactoryInfo factoryInfo = new FactoryInfo();
            HouseModels houseModels = new HouseModels();
            VPublicUser userInfo = new VPublicUser();
            if (id > 0)
            {
                houseBasicInfo =
                   ncBase.CurrentEntities.HouseBasicInfo.Where(o => o.HouseID == id).FirstOrDefault();

                if (houseBasicInfo.IsNull()) //房屋类型不匹配 或者发布类型不匹配
                {
                    houseBasicInfo = new HouseBasicInfo();
                    houseModels.HouseBasicInfo = houseBasicInfo;
                    houseModels.HouseInfo = houseInfo;
                    houseModels.HouseImages = houseImages;
                    houseModels.VillaInfo = villaInfo;
                    houseModels.ShopInfo = shopInfo;
                    houseModels.OfficeInfo = officeInfo;
                    houseModels.FactoryInfo = factoryInfo;
                    houseModels.UserInfo = userInfo;
                    return View(houseModels);
                }
                int buildingType = houseBasicInfo.BuildType;

                userInfo =
                  ncBase.CurrentEntities.VPublicUser.Where(o => o.UserID == houseBasicInfo.UserID).FirstOrDefault();

                houseModels.HouseBasicInfo = houseBasicInfo;

                houseModels.UserInfo = userInfo;
            }
            return View(houseModels);
        }

        [AllowAnonymous]
        [OutputCache(Duration = 60 * 60)]
        [ActionLog(CheckPoints = false)]
        [IgnoreValidateAttribute]
        public ActionResult NearHouses(int communityId, int trade = 1, int id = 0, int num = 4)
        {
            string sqlwhere = "it.communityId=" + communityId + " and it.TradeType=" + trade + "and it.Status=1  and it.HouseID<>" + id;
            List<HouseBasicInfo> houseBasicInfos =
                ncBase.CurrentEntities.HouseBasicInfo.Where(sqlwhere)
                       .OrderByDescending(o => Guid.NewGuid())
                    .Take(num)
                    .ToList();
            return View(houseBasicInfos);
        }

        [AllowAnonymous]
        [ActionLog(CheckPoints = false)]
        [IgnoreValidateAttribute]
        [OutputCache(Duration = 60 * 60)]
        public ActionResult UserOtherHouses(int uid = 0, int id = 0, int num = 2)
        {
            string sqlwhere = "it.UserID=" + uid + " and it.Status=1  and it.HouseID<>" + id;
            List<HouseBasicInfo> houseBasicInfos =
                ncBase.CurrentEntities.HouseBasicInfo.Where(sqlwhere)
                       .OrderByDescending(o => Guid.NewGuid())
                    .Take(num)
                    .ToList();
            return View(houseBasicInfos);
        }
        #endregion

        #region 房源编辑页面
        /// <summary>
        /// 房源编辑页面
        /// </summary>
        /// <param name="posttype">信息类型 1 出售 2 求购 3 出租 4求租</param>
        /// <param name="buildingType">建筑类型 1 住宅 2 别墅 3 商铺 4写字楼 5厂房</param>
        /// <param name="buildingId">房源ID </param>
        /// <param name="editType">发布类型 0 正在发布 1  克隆</param>
        /// <param name="chouseId">被克隆房源ID</param>
        /// <returns></returns>
        [CheckUserType]
        public ActionResult GetHouse(int posttype = 1, int buildingType = 1, int buildingId = 0, int editType = 0, int chouseId = 0, int cloneSuccess = 0)
        {
            ViewBag.ShowColned = cloneSuccess;
            PublicUserModel loginUser = this.GetLoginUser();
            HouseModels houseModels = new HouseModels();
            if (posttype < 1 || posttype > 4) posttype = 1;
            if (buildingType < 1 || buildingType > 5) buildingType = 1;
            ViewBag.Posttype = posttype;
            ViewBag.EditType = editType;
            ViewBag.ChouseId = chouseId;
            ViewBag.BuildingType = buildingType;
            ViewData["LoginUser"] = loginUser;
            if (cloneSuccess == 1)
                return View(houseModels);
            else
            {
                HouseBasicInfo houseBasicInfo = new HouseBasicInfo();
                List<HouseImage> houseImages = new List<HouseImage>();
                HouseInfo houseInfo = new HouseInfo();
                VillaInfo villaInfo = new VillaInfo();
                ShopInfo shopInfo = new ShopInfo();
                OfficeInfo officeInfo = new OfficeInfo();
                FactoryInfo factoryInfo = new FactoryInfo();

                if (buildingId > 0)
                {
                    houseBasicInfo =
                       ncBase.CurrentEntities.HouseBasicInfo.Where(o => o.HouseID == buildingId && o.BuildType == buildingType && o.TradeType == posttype).FirstOrDefault();
                    byte bTradeType = BitConverter.GetBytes(posttype)[0];
                    if (houseBasicInfo.IsNull()) //房屋类型不匹配 或者发布类型不匹配
                    {

                        houseBasicInfo = new HouseBasicInfo();

                        houseModels.HouseBasicInfo = houseBasicInfo;
                        houseModels.HouseInfo = houseInfo;
                        houseModels.HouseImages = houseImages;
                        houseModels.VillaInfo = villaInfo;
                        houseModels.ShopInfo = shopInfo;
                        houseModels.OfficeInfo = officeInfo;
                        houseModels.FactoryInfo = factoryInfo;

                        return View(houseModels);
                    }

                    houseImages =
                        ncBase.CurrentEntities.HouseImage.Where(o => o.HouseID == buildingId && o.Status == 1).ToList();
                    if (houseImages.IsNoNull())
                    {
                        houseImages = houseImages.OrderBy(o => o.OrderID).ToList();
                    }

                    switch (buildingType)
                    {
                        case (int)BuildingType.House:
                            houseInfo = ncBase.CurrentEntities.HouseInfo.Where(o => o.HouseID.Equals(buildingId)).FirstOrDefault();
                            if (houseInfo.IsNull()) houseInfo = new HouseInfo();
                            break;
                        case (int)BuildingType.Villa:
                            villaInfo = ncBase.CurrentEntities.VillaInfo.Where(o => o.HouseID.Equals(buildingId)).FirstOrDefault();
                            if (villaInfo.IsNull()) villaInfo = new VillaInfo();
                            break;
                        case (int)BuildingType.Shop:
                            shopInfo = ncBase.CurrentEntities.ShopInfo.Where(o => o.HouseID.Equals(buildingId)).FirstOrDefault();
                            if (shopInfo.IsNull()) shopInfo = new ShopInfo();
                            break;
                        case (int)BuildingType.Office:
                            officeInfo = ncBase.CurrentEntities.OfficeInfo.Where(o => o.HouseID.Equals(buildingId)).FirstOrDefault();
                            if (officeInfo.IsNull()) officeInfo = new OfficeInfo();
                            break;
                        case (int)BuildingType.Factory:
                            factoryInfo = ncBase.CurrentEntities.FactoryInfo.Where(o => o.HouseID.Equals(buildingId)).FirstOrDefault();
                            if (factoryInfo.IsNull()) factoryInfo = new FactoryInfo();
                            break;
                    }
                }

                if (string.IsNullOrEmpty(houseBasicInfo.InternalNum))
                    houseBasicInfo.InternalNum = "WX" + DateTime.Now.ToString("yyyyMMdd") + StringUtility.GetValiCode(4);

                houseModels.HouseBasicInfo = houseBasicInfo;
                houseModels.HouseInfo = houseInfo;
                houseModels.HouseImages = houseImages;
                houseModels.VillaInfo = villaInfo;
                houseModels.ShopInfo = shopInfo;
                houseModels.OfficeInfo = officeInfo;
                houseModels.FactoryInfo = factoryInfo;

                CommunityDetailModel result = new CommunityDetailModel();
                List<RegionsModel> regionsList = regionBll.GetRegionList(loginUser.CityID);
                result.DistrctList = regionsList.FindAll(x => (x.Layer == 2));
                result.RegionList = regionsList.FindAll(x => (x.Layer == 3));
                ViewBag.RegionsModel = result;

                return View(houseModels);
            }
        }

        public ActionResult GetHouseLables()
        {
            return View();
        }
        public ActionResult GetHelp()
        {
            return View();
        }
        #endregion

        #region 个人房源列表



        [AllowAnonymous]
        [ActionLog(CheckPoints = false)]
        public ActionResult PersonHouseList(HouseParameter houseParameter)
        {

            HouseListReq parames = new HouseListReq();
            //PublicUserModel thisUser = this.GetLoginUser();
            int userId = 0;
            HouseBll houseBll = new HouseBll();
            parames.page = houseParameter.Page == 0 ? 1 : houseParameter.Page ?? 1;

            parames.pageSize = houseParameter.PageSize == 0 ? 10 : houseParameter.PageSize ?? 10;
            int totalSize = 0;
            var houseList = houseBll.GetHouseCollectList(parames, ref totalSize).ToList();//房源列表
            List<string> ids = new List<string>();
            if (houseList != null && houseList.Count() > 0)
            {
                ids = houseList.Select(v => v.Id).ToList();
            }
            var jjrTels = houseBll.GetJjrTels();
            MongoCursor<HouseCollectViewLog> viewList = houseBll.GetHouseCollectReadLogByIds(ids);
            List<HouseCollectInfo> resultList = new List<HouseCollectInfo>(houseList.Select(h => new HouseCollectInfo(h.Address, h.Balcony, h.BuildArea, h.BuildingType, h.CommunityName, h.CurFloor ?? 0, h.CityID, h.Distrctid, h.DistrctName, h.Hall, h.Id, h.Kitchen, h.MaxFloor ?? 0, h.PicNum ?? 0, h.Price, h.PriceUnit, h.Publisher, h.RegionID, h.RegionName, h.ReleaseTime, h.UpdateTime.IsNoNull() ? ((DateTime)h.UpdateTime).AddHours(8).ToString("MM-dd HH:mm") : "", h.Room, h.Source, h.Tel, h.Title, h.Toilet, h.TradeType, h.UpdateTime, h.Url, h.UpdateTime > h.ReleaseTime ? 1 : 0, viewList == null ? 0 : viewCount(viewList, h.Id), viewList == null ? 0 : isRead(viewList, h.Id, userId), jjrTels.Contains(h.Tel) ? 1 : 0)));
            int rowsCount = 1000;
            ViewData["rowsCount"] = rowsCount; //总条数
            PagedList<HouseCollectInfo> houseInfoList = new PagedList<HouseCollectInfo>(resultList, parames.page, parames.pageSize, rowsCount);
            ViewBag.Parameter = houseParameter; //参数
            ViewData["TradeType"] = 1;
            return View(houseInfoList);


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
            int.TryParse(form["editType"], out editType);  //发布类型 0 正在发布  1 克隆

            int houseId = 0, buildingType, city, uid = loginUser.UserID;
            int.TryParse(form["houseId"], out houseId);  //房源ID

            if (editType > 0)
            {
                houseId = 0;  // 克隆 作为新增房源
            }
            int chouseId = 0;
            int.TryParse(form["chouseId"], out chouseId);  //被克隆房源ID
            int.TryParse(form["buildingType"], out buildingType);  //房屋类型
            int tradeType;
            int.TryParse(form["postType"], out tradeType); //发布类型
            int isChange = 0;
            int.TryParse(form["isChange"], out isChange);  //表单是否被修改

            int.TryParse(form["city"], out city);  //城市ID
            int communityId;
            int.TryParse(form["cellCode"], out communityId); // 小区ID
            string communityName = form["cell"];  //小区名
            if (communityId < 1)
                return Content("请选择我们提供的一个小区");

            int distrct;
            int.TryParse(form["distrct"], out distrct);  //行政区

            int region;
            int.TryParse(form["area"], out region);    //路段

            string address = form["addr"];  //地址

            double buildArea, usedArea;
            double.TryParse(form["houseArea"], out buildArea);    //建筑面积
            double.TryParse(form["areaUsed"], out usedArea);    //使用面积
            if (usedArea > buildArea && buildingType != 2) //别墅可以使用面积>建筑面积
                return Content("建筑面积必须大于使用面积");
            int room, hall, kitchen, toilet, balcony;
            int.TryParse(form["room"], out room);    //房
            int.TryParse(form["hall"], out hall);    //厅
            int.TryParse(form["kitchen"], out kitchen);    //厨房
            int.TryParse(form["toilet"], out toilet);    //卫生间
            int.TryParse(form["balcony"], out balcony);    //阳台

            double price;
            double.TryParse(form["price"], out price);    //价格

            double lowpay = 0; //最低首付


            string priceUnit;  //价格单位
            double unitPrice = 0;

            switch (tradeType)
            {
                case (int)TradeType.Sell:
                    priceUnit = "万";
                    unitPrice = buildArea > 0 ? price * 10000 / buildArea : 0;  //出售计算单价
                    lowpay = price * 10000 * 0.30;  //最低首付计算
                    ; break;
                case (int)TradeType.Rent:
                    switch (buildingType)
                    {
                        case (int)BuildingType.House:
                        case (int)BuildingType.Villa:
                            priceUnit = "元/月"; break;
                        default: priceUnit = form["priceType"]; break;
                    }
                    break;

                default: priceUnit = form["priceType"]; break;
            }

            string payType = form["payType"].IsNoNull() ? form["payType"] : "";  //支付方式

            int usedYear = 0;
            int.TryParse(form["ddlUsedYear"], out usedYear);  //建造年代

            Int16 curFloor = 0;
            Int16.TryParse(form["curFloor"], out curFloor);  //所在楼层

            Int16 maxFloor = 0;
            Int16.TryParse(form["maxFloor"], out maxFloor);  //总层数

            if (curFloor > maxFloor)
                return Content("总层数必须大于所在楼层");

            string pointTo = form["pointTo"];  //朝向

            string lookTime = form["lookTime"];  //看房时间
            string fitmentStatus = form["fitmentStatus"];  //装修程度
            string internalNum = form["internalNum"];  //内部编号
            string cellLabel = form["cellLabel"];  //小区特色
            string houseLabel = form["houseLabel"];  //房源标签
            string yijuhua = form["yijuhua"];  //一句话广告 
            string video = form["video"];  //视频 
            string title = form["title"].IsNoNull() ? form["title"] : communityName;  //房源标题
          
            string contacts = form["contacts"].IsNoNull() ? form["contacts"] : loginUser.NickName;  //委托人
            string tel = form["tel"].IsNoNull() ? form["tel"] : loginUser.Tel;  //委托人联系方式
            var compressor = new HtmlCompressor();
            compressor.setRemoveStyleAttributes(true);

            string houseDescribe = form["houseDescribe"].IsNoNull() ? compressor.compress(form["houseDescribe"]) : ""; //房源描述
            string houseDescribeDelHtml = StringUtility.DelHtml(houseDescribe);
            if (houseDescribeDelHtml.Length < 30 || houseDescribeDelHtml.Length > 3000)
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
            Community communityItem = ncBase.CurrentEntities.Community.Where(o => o.CommunityID == communityId).FirstOrDefault();
            if (communityItem.IsNoNull() && string.IsNullOrEmpty(communityItem.Name))
            {
                communityName = communityItem.Name;
            }
            houseBasicInfo.UserID = uid;  //用户ID
            houseBasicInfo.TradeType = BitConverter.GetBytes(tradeType)[0];
            houseBasicInfo.CityID = city;
            houseBasicInfo.Distrctid = distrct;
            houseBasicInfo.RegionID = region;
            houseBasicInfo.CommunityID = communityId;
            houseBasicInfo.CommunityName = communityName;
            houseBasicInfo.BuildType = BitConverter.GetBytes(buildingType)[0];
            houseBasicInfo.BuildArea = Convert.ToDecimal(buildArea);
            houseBasicInfo.UsedArea = Convert.ToDecimal(usedArea); ;
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
            houseBasicInfo.Status = BitConverter.GetBytes((int)HouseStatus.Release)[0];
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
                        houseImage.Status = BitConverter.GetBytes(1)[0]; ;
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
                    string basicEquipHouse = form["basicEquip"].IsNoNull() ? form["basicEquip"] : "";  //住宅基础设施
                    string houseType = form["houseType"].IsNoNull() ? form["houseType"] : "";  //房屋类别
                    string houseSubType = form["houseSubType"].IsNoNull() ? form["houseSubType"] : "";  //住宅子类型
                    string houseProperty = form["houseProperty"].IsNoNull() ? form["houseProperty"] : "";  //房屋产权
                    string landYear = form["landYear"].IsNoNull() ? form["landYear"] : "";  //产权年限
                    string houseStructure = form["houseStructure"].IsNoNull() ? form["houseStructure"] : "";  //房屋结构
                    bool fiveYears = form["fiveYears"].IsNoNull() && form["fiveYears"] == "1";//产证满二
                    bool onlyHouse = form["onlyHouse"].IsNoNull() && form["onlyHouse"] == "1";  //唯一住房
                    string advEquip = form["advEquip"].IsNoNull() ? form["advEquip"] : "";  //配套设施

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

                    string villaType = form["villaType"].IsNoNull() ? form["villaType"] : "";  //别墅形式
                    string hallType = form["hallType"].IsNoNull() ? form["hallType"] : "";  //厅结构
                    string landYear2 = form["landYear2"].IsNoNull() ? form["landYear2"] : "";  //产权年限
                    bool fiveYears2 = form["fiveYears2"].IsNoNull() && form["fiveYears2"] == "1";//产证满二
                    bool onlyHouse2 = form["onlyHouse2"].IsNoNull() && form["onlyHouse2"] == "1";  //唯一住房
                    bool basement = form["basement"].IsNoNull() && form["basement"] == "1";//地下室
                    bool garden = form["garden"].IsNoNull() && form["garden"] == "1";  //花园
                    bool garage = form["garage"].IsNoNull() && form["garage"] == "1";//车库
                    bool parkLot = form["parkLot"].IsNoNull() && form["parkLot"] == "1";  //停车位
                    string basicEquip1 = form["basicEquip1"].IsNoNull() ? form["basicEquip1"] : "";  //配套设施
                    string advEquip1 = form["advEquip1"].IsNoNull() ? form["advEquip1"] : "";  //室内设施

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

                    string shopType = form["shopType"].IsNoNull() ? form["shopType"] : ""; ;  //商铺类型
                    string shopStatus = form["shopStatus"].IsNoNull() ? form["shopStatus"] : ""; ;  //商铺状态
                    string targetField = form["targetField"].IsNoNull() ? form["targetField"] : ""; ;  //目标业态
                    decimal feeShop;
                    decimal.TryParse(form["fee2"], out feeShop); //物业费
                    bool divideShop = form["divide2"].IsNoNull() && form["divide2"] == "1";//是否分割
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

                    string officeType = form["officeType"].IsNoNull() ? form["officeType"] : "";  //写字楼类别
                    string officeLevel = form["officeLevel"].IsNoNull() ? form["officeLevel"] : "";  //写字楼级别
                    decimal fee3;
                    decimal.TryParse(form["fee3"], out fee3);  //物业费
                    bool divide3 = form["divide3"].IsNoNull() && form["divide3"] == "1";//是否分割
                    string basicEquip3 = form["basicEquip3"].IsNoNull() ? form["basicEquip3"] : "";  //配套设施


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

                    string factoryType = form["factoryType"].IsNoNull() ? form["factoryType"] : "";  //厂房类别

                    string basicEquip4 = form["basicEquip4"].IsNoNull() ? form["basicEquip4"] : "";  //基础设施


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

         
            if (editType <= 0)
            {
                return RedirectToAction("GetHouseMainView", new { posttype = tradeType, buildType = buildingType });
            }
            else
            {
                if (houseId > 0)
                {
                    HouseBasicInfoShare itemShare = ncBase.CurrentEntities.HouseBasicInfoShare.Where(o => o.ShareHouseId == chouseId).FirstOrDefault();
                    if (itemShare.IsNoNull())
                    {
                        itemShare.ShareCount = itemShare.ShareCount + 1;
                        ncBase.CurrentEntities.SaveChanges();
                    }
                }
                return RedirectToAction("GetHouse", new { posttype = tradeType, buildType = buildingType, editType = editType, buildingId = chouseId, cloneSuccess = 1 });
            }
            // return JsonReturnValue(new { Message = "房源提交成功", id = houseId, }, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region 房源列表页面
        /// <summary>
        /// 
        /// </summary>
        /// <param name="posttype">信息类型 1 出售 2 求购 3 出租 4求租</param>
        /// <returns></returns>
        [CheckUserType]
        public ActionResult GetHouseMainView(int posttype = 1, int buildType = 1)
        {
            if (posttype == 0) posttype = 1;
            ViewBag.posttype = posttype;
            if (buildType == 0) buildType = 1;
            ViewBag.buildType = buildType;
            HouseBll houseBll = new HouseBll();
            List<HouseNumSumModel> sumList = houseBll.GetHouseNumSumData(this.GetLoginUser().UserID, posttype);
            return View(sumList);
        }
        #endregion

        #region 获取房源列表 json数据
        /// <summary>
        ///  获取房源列表 /Release/GetHouseList
        /// </summary>
        /// <param name="parames"></param>
        /// <returns></returns>
        public JsonResult GetHouseList(HouseListReq parames)
        {
            HouseBll houseBll = new HouseBll();
            RegionsBll regionBll = new RegionsBll();
            parames.userId = this.GetLoginUser().UserID;
            int houseid = 0;
            int.TryParse(parames.title, out houseid);//支持房源编号 或者标题搜索用到
            parames.page = parames.page == 0 ? 1 : parames.page;
            parames.houseId = houseid;
            if (parames.houseId > 0)
            {
                parames.title = string.Empty;
            }
            parames.pageSize = parames.pageSize == 0 ? 10 : parames.pageSize;
            int totalSize = 0;
            List<HouseBasicInfoModel> houseList = houseBll.GetHouseList(parames, ref totalSize);//房源列表

            List<RegionsModel> regionsList = regionBll.GetRegionList();

            var resultList = houseList.GroupJoin(
                regionsList,
                houseTable => houseTable.Distrctid,
                regionTable => regionTable.RegionID,
                (houseTable, regionTable) => new { houseTable, regionTable }
                ).Select(houseItem => new
                {
                    AddDate = houseItem.houseTable.AddDate.ToString(),
                    Address = houseItem.houseTable.Address,
                    Balcony = houseItem.houseTable.Balcony,
                    BuildArea = houseItem.houseTable.BuildArea,
                    BuildType = houseItem.houseTable.BuildType,
                    CellLabel = houseItem.houseTable.CellLabel,
                    CityID = houseItem.houseTable.CityID,
                    CommunityID = houseItem.houseTable.CommunityID,
                    CommunityName = houseItem.houseTable.CommunityName,
                    CurFloor = houseItem.houseTable.CurFloor,
                    Distrctid = houseItem.houseTable.Distrctid,
                    DistrctName = houseItem.regionTable.FirstOrDefault() != null ? houseItem.regionTable.FirstOrDefault().Name : "",
                    DeleteTime = houseItem.houseTable.DeleteTime.ToString(),
                    ExpireDay = houseItem.houseTable.ExpireDay.ToString(),
                    FitmentStatus = houseItem.houseTable.FitmentStatus,
                    Hall = houseItem.houseTable.Hall,
                    HouseID = houseItem.houseTable.HouseID,
                    HouseImgPath = houseItem.houseTable.HouseImgPath,
                    HouseLabel = houseItem.houseTable.HouseLabel,
                    InternalNum = houseItem.houseTable.InternalNum,
                    IP = houseItem.houseTable.IP,
                    Kitchen = houseItem.houseTable.Kitchen,
                    LookHouseTime = houseItem.houseTable.LookHouseTime,
                    MaxFloor = houseItem.houseTable.MaxFloor,
                    Note = houseItem.houseTable.Note,
                    PayType = houseItem.houseTable.PayType,
                    PicNum = houseItem.houseTable.PicNum,
                    PointTo = houseItem.houseTable.PointTo,
                    PostTime = houseItem.houseTable.PostTime.ToString(),
                    Price = houseItem.houseTable.Price,
                    PriceUnit = houseItem.houseTable.PriceUnit,
                    PushTime = houseItem.houseTable.PushTime.ToString(),
                    RegionID = houseItem.houseTable.RegionID,
                    Room = houseItem.houseTable.Room,
                    Status = houseItem.houseTable.Status,
                    Tag = houseItem.houseTable.Tag,
                    Title = houseItem.houseTable.Title,
                    Toilet = houseItem.houseTable.Toilet,
                    TradeType = houseItem.houseTable.TradeType,
                    UnitPrice = houseItem.houseTable.UnitPrice,
                    UsedArea = houseItem.houseTable.UsedArea,
                    UsedYear = houseItem.houseTable.UsedYear,
                    UserID = houseItem.houseTable.UserID,
                    YiJuHua = houseItem.houseTable.YiJuHua,
                    webCount = houseItem.houseTable.WebCount,
                    OrderStatus = houseItem.houseTable.OrderStatus,
                    BeColneHouseId = houseItem.houseTable.BeColneHouseID,
                    IsShare = houseItem.houseTable.IsShare,
                    Hits = houseItem.houseTable.Hits,
                    houseItem.houseTable.Tel,
                    houseItem.houseTable.Contacts
                }).ToList();
            return Json(new { data = resultList, totalSize = totalSize }, JsonRequestBehavior.AllowGet);
        }
        private int GetWebCount(string postSites, string orderSites)
        {
            List<string> postSiteList = postSites != null && postSites.Split(',') != null ? postSites.Split(',').ToList() : new List<string>();
            List<string> orderSiteList = orderSites != null && orderSites.Split(',') != null ? orderSites.Split(',').ToList() : new List<string>(); ;
            postSiteList.AddRange(orderSiteList);
            List<string> resultList = postSiteList.Union(orderSiteList).ToList();
            return resultList.Count;
        }
        #endregion

        #region 删除房源
        [HttpPost]
        public JsonResult DeleteHouses(ChangeHouseStatusReq parames)
        {
            PublicUserModel loginUser = this.GetLoginUser();

            if (Request.Form["HouseID"].ToString() != null && Request.Form["HouseID"].ToString() != "")
            {
                parames.HouseIdsStr = Request.Form["HouseID"].ToString();
                parames.UserId = loginUser.UserID;
                HouseBll houseBll = new HouseBll();
                int count = houseBll.ChangeHouseStatus(parames);
                return Json(new { msg = "成功", count = count });
            }
            else
            {
                return Json(new { msg = "无效参数", count = 0 });
            }
        }
        #endregion

        #region 用户房源小区 下拉页面
        public JsonResult UserHouseCommunityList(int postType = 1, int budlingType = 1, int budlingStatus = 1)
        {
            HouseBll houseBll = new HouseBll();
            List<CommunityModel> communityList = houseBll.GetUserHouseCommunityList(new GetUserHouseCommunityListReq()
            {
                BudlingType = budlingType,
                BudlingStatus = budlingStatus,
                PostType = postType,
                UserId = this.GetLoginUser().UserID
            });
            return Json(communityList, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region 房源标签
        public ActionResult UpdateHouseTagsView(string tags)
        {
            ViewBag.Tags = tags;
            return View();
        }
        [HttpPost]
        public JsonResult batchUpdateHouseTags(string tag)
        {
            string houseIds = string.Empty;
            if (Request.Form["HouseIds"] != null && Request.Form["HouseIds"].ToString() != "")
            {
                houseIds = Request.Form["HouseIds"].ToString();
            }
            HouseBll houseBll = new HouseBll();
            houseBll.UpdateHouseTags(this.GetLoginUser().UserID, houseIds, tag);
            return Json(new { msg = "ok" });

        }
        #endregion

        #region 获取可发布的站点列表
        /// <summary>
        /// 支持的建筑类型
        /// </summary>
        /// <param name="buildType"></param>
        /// <param name="isFilter">是否过滤状态不正常的</param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult GetEnableWebSite(int buildType = 0, int isFilter = 0)
        {
            PublicUserModel loginUser = this.GetLoginUser();//当前用户

            SiteManageBll siteManageBll = new SiteManageBll();
            List<SiteManageModel> siteManageList = siteManageBll.GetSiteList(buildType.ToString(), loginUser.CityID);//所有站点

            UserSiteManageBll userSiteManageBll = new UserSiteManageBll();
            List<UserSiteManageModel> userSiteManageList = userSiteManageBll.GetUserSiteListByUserId(loginUser.UserID);//用户关联的站点

            List<SiteManageViewModel> viewList = (from site in siteManageList
                                                  join userSite in userSiteManageList on site.SiteID equals userSite.SiteID into viewListTemp
                                                  from viewItem in viewListTemp.DefaultIfEmpty()
                                                  where viewItem != null && (viewItem.SiteStatus == 1 || isFilter == 0)
                                                  select new SiteManageViewModel()
                                                  {
                                                      SiteID = site.SiteID,
                                                      Logo = site.Logo,
                                                      SiteName = site.SiteName,
                                                      SiteUserName = viewItem != null ? viewItem.SiteUserName : "",
                                                      IsBan = viewItem != null && viewItem.BanTime > DateTime.Now ? 1 : 0,
                                                      BanText = viewItem != null ? viewItem.BanText : "",
                                                      BanTime = viewItem != null && viewItem.BanTime > DateTime.Now ? ((DateTime)viewItem.BanTime).ToString("yyyy-MM-dd") : "",
                                                      LimitOperation = viewItem.LimitOperation ?? 0,
                                                      RepeatOperation = viewItem.RepeatOperation ?? 0,
                                                      PlaceOperation = viewItem.PlaceOperation ?? 0
                                                  }).ToList();
            return Json(viewList);
        }

        #endregion

        #region 发布和预约发布
        /// <summary>
        /// 
        /// </summary>
        /// <param name="releaseType">0发布 1预约发布</param>
        /// <param name="buildCheck"></param>
        /// <param name="webCheck"></param>
        /// <param name="time"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult ReleaseHouses(List<string> buildCheck, List<string> webCheck, List<string> time, int releaseType = 0)
        {
            PublicUserModel loginUser = this.GetLoginUser();
            System.Web.Script.Serialization.JavaScriptSerializer jss = new System.Web.Script.Serialization.JavaScriptSerializer();
            string webSites = string.Empty;
            string times = string.Empty;
            string operateOption = string.Empty;
            List<PostLogModel> postLogOperation = new List<PostLogModel>();
            if (Request.Form["webCheck"] != null && Request.Form["webCheck"].ToString() != "")
            {
                webSites = Request.Form["webCheck"].ToString();
            }
            if (Request.Form["time"] != null && Request.Form["time"].ToString() != "")
            {
                times = Request.Form["time"].ToString();
            }
            if (Request.Form["WebOperate"] != null && Request.Form["WebOperate"].ToString() != "")
            {
                operateOption = Request.Form["WebOperate"].ToString();
                if (!string.IsNullOrEmpty(operateOption))
                {
                    string[] oList = operateOption.Split(',');
                    for (int i = 0; i < oList.Length; i++)
                    {
                        string[] option = oList[i].Split('_');
                        if (option.Length >= 3)
                        {
                            int siteId = 0;
                            int repeatOp = 0;
                            int limitOp = 0;
                            int placeOp = 0;
                            int.TryParse(option[0], out siteId);
                            int.TryParse(option[1], out repeatOp);
                            int.TryParse(option[2], out limitOp);
                            if (option.Length >= 4)
                            {
                                int.TryParse(option[3], out placeOp);
                            }
                            postLogOperation.Add(new PostLogModel()
                            {
                                SiteID = siteId,
                                RepeatOperation = repeatOp,
                                LimitOperation = limitOp,
                                PlaceOperation = placeOp
                            });

                        }
                    }
                }
            }
            PostManageBll postBll = new PostManageBll();
            List<PostManageModel> postList = new List<PostManageModel>();
            foreach (string houseId in buildCheck)
            {
                int hid = 0;
                int.TryParse(houseId, out hid);
                if (hid > 0)
                {
                    postList.Add(new PostManageModel()
                    {
                        HouseID = hid,
                        OrderTime = times,
                        PostSites = webSites,
                        OrderSites = webSites
                    });
                }
            }
            int rowCount = postBll.BatchPostManageAdd(loginUser.UserID, releaseType, postList, postLogOperation);
            
            return Json(rowCount);
        }
        /// <summary>
        /// 整合重复的站点
        /// </summary>
        /// <param name="allSite">新的站点</param>
        /// <param name="addSite">已有的站点</param>
        /// <returns>allSite</returns>
        private string DistinctSites(string addSite, int houseId, List<PostManageModel> oldList)
        {
            string result = string.Empty;
            PostManageModel item = oldList.Where(i => i.HouseID == houseId).FirstOrDefault();
            string allSite = item == null ? string.Empty : item.AllSites;
            List<string> allSiteList = string.IsNullOrEmpty(allSite) ? new List<string>() : allSite.Split(',').ToList();
            List<string> addSiteList = string.IsNullOrEmpty(addSite) ? new List<string>() : addSite.Split(',').ToList();
            allSiteList = allSiteList.Union(addSiteList).ToList();
            for (int i = 0; i < allSiteList.Count - 1; i++)
            {
                if (i >= allSiteList.Count - 1)
                {
                    result += allSiteList[i];
                }
                else
                {
                    result += allSiteList[i] + ",";
                }
            }
            return result;
        }
        #endregion

        #region 获取小区信息
        [AllowAnonymous]
        [HttpPost]
        [ValidateInput(false)]

        public JsonResult GetCellsByInput(string inputStr, int buildingType)
        {
            PublicUserModel loginUser = this.GetLoginUser();
            int cityId = loginUser.IsNull()?592:loginUser.CityID;
            List<VCommunity> conCommunities =
                ncBase.CurrentEntities.VCommunity.Where(o => o.AliasName.Contains(inputStr) && o.CityID == cityId && o.Status == 1).Take(10).ToList();
            if (conCommunities.IsNoNull())
            {
                dynamic cellls = conCommunities.Select(com =>
               new
               {
                   address = com.Address,
                   area = com.RegionID,
                   avgPrice = com.SellPrice,
                   cellCode = com.CommunityID,
                   cellName = com.AliasName,
                   completionDate = 0,
                   district = com.Distrctid
               });
                return Json(new CellResponse(cellls), JsonRequestBehavior.AllowGet);
            }

            return Json(new CellResponse(null), JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region 云刷新页面
        [CheckUserType]
        public ActionResult Refresh(int tabIndex = 0)
        {
            ViewBag.tabIndex = tabIndex;
            return View();
        }
        #endregion

        #region 云采集页面 和列表数据
        [CheckUserType]
        public ActionResult HouseCollect(int city = 592, int postType = 1)
        {
            int pi = 1;
            int ps = 10;
            ViewBag.tradeType = postType;
            ViewBag.pi = pi;
            ViewBag.ps = ps;
            ViewBag.city = city;
            PublicUserModel loginUser = this.GetLoginUser();
       
            return View();
        }



        public JsonResult GetHouseCollectList(HouseListReq parames)
        {

            PublicUserModel thisUser = this.GetLoginUser();
            string userName = thisUser.Name;
            int userId = thisUser.UserID;
            HouseBll houseBll = new HouseBll();
            parames.page = parames.page == 0 ? 1 : parames.page;
            if (parames.houseId > 0)
            {
                parames.title = string.Empty;
            }
            parames.pageSize = parames.pageSize == 0 ? 10 : parames.pageSize;
            int totalSize = 0;
            var houseList = houseBll.GetHouseCollectList(parames, ref totalSize).ToList();//房源列表
            List<string> ids = new List<string>();
            if (houseList != null && houseList.Count() > 0)
            {
                ids = houseList.Select(v => v.Id).ToList();
            }
            var jjrTels = houseBll.GetJjrTels();
            MongoCursor<HouseCollectViewLog> viewList = houseBll.GetHouseCollectReadLogByIds(ids);
            var resultList = houseList.Select(h => new
            {
                h.Address,
                h.Balcony,
                h.BuildArea,
                h.BuildingType,
                h.CommunityName,
                CurFloor = h.CurFloor ?? 0,
                h.CityID,
                h.Distrctid,
                h.DistrctName,
                h.Hall,
                h.Id,
                h.Kitchen,
                MaxFloor = h.MaxFloor ?? 0,
                PicNum = h.PicNum ?? 0,
                h.Price,
                h.PriceUnit,
                h.Publisher,
                h.RegionID,
                h.RegionName,
                h.ReleaseTime,
                AddDate = h.UpdateTime.IsNoNull() ? ((DateTime)h.UpdateTime).AddHours(8).ToString("MM-dd HH:mm") : "",
                h.Room,
                h.Source,
                h.Tel,
                h.Title,
                h.Toilet,
                h.TradeType,
                h.UpdateTime,
                h.Url,
                houseType = h.UpdateTime > h.ReleaseTime ? 1 : 0,// 0:新房源 ，1：刷新
                viewCount = viewList == null ? 0 : viewCount(viewList, h.Id),
                isRead = viewList == null ? 0 : isRead(viewList, h.Id, userId),
                isJjr = jjrTels.Contains(h.Tel) ? 1 : 0
            });
            return Json(new { data = resultList, totalSize = totalSize }, JsonRequestBehavior.AllowGet);
        }

        private int isRead(MongoCursor<HouseCollectViewLog> viewList, string id, int userId)
        {
            int isRead = 0;
            HouseCollectViewLog item = viewList.Where(v => v.id == id).FirstOrDefault();
            if (item != null)
            {
                isRead = item.Agents.Where(a => a.Id == userId).FirstOrDefault() == null ? 0 : 1;
            }
            return isRead;
        }
        private int viewCount(MongoCursor<HouseCollectViewLog> viewList, string id)
        {
            int count = 0;
            HouseCollectViewLog item = viewList.Where(v => v.id == id).FirstOrDefault();
            if (item != null)
            {
                count = item.Agents.Count;
            }
            return count;
        }
        public JsonResult GetHouseCollectSite(int cityId = 592)
        {
            HouseBll houseBll = new HouseBll();
            MongoCursor<HouseCollectSource> houseList = houseBll.GetHouseCollectSite(cityId);///
            return Json(new { data = houseList }, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult AddHouseCollectRead(string id)
        {
            PublicUserModel thisUser = this.GetLoginUser();
            string userName = thisUser.Name;
            int userId = thisUser.UserID;
            if (!string.IsNullOrEmpty(id))
            {
                HouseBll houseBll = new HouseBll();
                houseBll.HouseCollectReadAdd(id, userId, userName);
            }
            return Json(1);
        }
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Browser(HouseCrawler form)
        {
            string basicSiteUrl = "http://" + HttpContext.Request.Url.Host;
            string url = form.Url.IsNull() ? basicSiteUrl : form.Url;

            PublicUserModel loginUser = this.GetLoginUser();
            HouseCollection newCollect = ncBase.CurrentEntities.HouseCollection.Where(c => c.CollectId == form.Id && c.UserId == loginUser.UserID && c.CollectStatus == 1).FirstOrDefault();
            ViewBag.IsCollect = 0;
            if (newCollect.IsNoNull())
            {
                ViewBag.IsCollect = 1;
            }
            ViewBag.Url = url;
        
            return View(form);
        }
        [AllowAnonymous]
        [ActionLog(CheckPoints = false)]
        public ActionResult Browser(string hid = "", string tc = "")
        {
            string url = "http://" + HttpContext.Request.Url.Host;

            ViewBag.IsCollect = 0;
            if (string.IsNullOrEmpty(hid) || string.IsNullOrEmpty(tc))
            {
                ViewBag.Url = url;
                return View(new HouseCrawler());
            }

            string dbCollection = tc;
            HouseBll houseBll = new HouseBll();
            HouseCrawler form = houseBll.GetHouseCollect(hid, tc);


            PublicUserModel loginUser = this.GetLoginUser();

            if (form.IsNoNull())
            {
                houseBll.HouseCollectReadAdd(hid, loginUser.UserID, loginUser.Name);

                HouseCollection newCollect = ncBase.CurrentEntities.HouseCollection.Where(c => c.CollectId == form.Id && c.UserId == loginUser.UserID && c.CollectStatus == 1).FirstOrDefault();

                if (newCollect.IsNoNull())
                {
                    ViewBag.IsCollect = 1;
                }
                url = form.Url;
              
            }
            ViewBag.Url = url;

            return View(form);
        }
        public JsonResult AddHouseCollection(HouseCrawler houseData)
        {
            HouseCollection newCollect = new HouseCollection();
            PublicUserModel loginUser = this.GetLoginUser();
            newCollect = ncBase.CurrentEntities.HouseCollection.Where(c => c.CollectId == houseData.Id && c.UserId == loginUser.UserID).FirstOrDefault();
            byte balcony = 0;
            byte.TryParse(houseData.Balcony.ToString(), out balcony);
            decimal buildArea = 0;
            decimal.TryParse(houseData.BuildArea, out buildArea);
            byte hall = 0;
            byte.TryParse(houseData.Hall.ToString(), out hall);
            byte kitchen = 0;
            byte.TryParse(houseData.Kitchen.ToString(), out kitchen);
            decimal price = 0;
            decimal.TryParse(houseData.Price.ToString(), out price);
            byte room = 0;
            byte.TryParse(houseData.Room.ToString(), out room);
            byte toilet = 0;
            byte.TryParse(houseData.Toilet.ToString(), out toilet);
            if (newCollect.IsNull())
            {
                newCollect = new HouseCollection()
                {
                    Address = houseData.Address,
                    AddTime = DateTime.Now,
                    Balcony = balcony,
                    BuildArea = buildArea,
                    BuildingType = houseData.BuildingType,
                    CityId = houseData.CityID,
                    CollectId = houseData.Id,
                    CommunityName = houseData.CommunityName,
                    CurFloor = houseData.CurFloor,
                    Distrctid = houseData.Distrctid,
                    DistrctName = houseData.DistrctName,
                    Hall = hall,
                    Kitchen = kitchen,
                    MaxFloor = houseData.MaxFloor,
                    PicNum = houseData.PicNum,
                    Price = price,
                    PriceUnit = houseData.PriceUnit,
                    Publisher = houseData.Publisher,
                    RegionID = houseData.RegionID,
                    RegionName = houseData.RegionName,
                    ReleaseTime = houseData.ReleaseTime,
                    Room = room,
                    Source = houseData.Source,
                    Tel = houseData.Tel,
                    Title = houseData.Title,
                    Toilet = toilet,
                    TradeType = houseData.TradeType,
                    Url = houseData.Url,
                    UserId = loginUser.UserID,
                    CollectStatus = 1
                };
                ncBase.CurrentEntities.HouseCollection.AddObject(newCollect);
            }
            else
            {
                newCollect.Address = houseData.Address;
                newCollect.AddTime = DateTime.Now;
                newCollect.Balcony = balcony;
                newCollect.BuildArea = buildArea;
                newCollect.BuildingType = houseData.BuildingType;
                newCollect.CityId = houseData.CityID;
                newCollect.CollectId = houseData.Id;
                newCollect.CommunityName = houseData.CommunityName;
                newCollect.CurFloor = houseData.CurFloor;
                newCollect.Distrctid = houseData.Distrctid;
                newCollect.DistrctName = houseData.DistrctName;
                newCollect.Hall = hall;
                newCollect.Kitchen = kitchen;
                newCollect.MaxFloor = houseData.MaxFloor;
                newCollect.PicNum = houseData.PicNum;
                newCollect.Price = price;
                newCollect.PriceUnit = houseData.PriceUnit;
                newCollect.Publisher = houseData.Publisher;
                newCollect.RegionID = houseData.RegionID;
                newCollect.RegionName = houseData.RegionName;
                newCollect.ReleaseTime = houseData.ReleaseTime;
                newCollect.Room = room;
                newCollect.Source = houseData.Source;
                newCollect.Tel = houseData.Tel;
                newCollect.Title = houseData.Title;
                newCollect.Toilet = toilet;
                newCollect.TradeType = houseData.TradeType;
                newCollect.Url = houseData.Url;
                newCollect.UserId = loginUser.UserID;
                newCollect.CollectStatus = 1;
            }
            ncBase.CurrentEntities.SaveChanges();
            return Json(new { status = newCollect.Id, data = houseData.ReleaseTime.ToString() });
        }
        #endregion

        #region 房源共享
        [CheckUserType]
        public ActionResult ShareBuilding()
        {
            int uid = this.GetLoginUser().UserID;
            ViewBag.IsDebug = (uid <= 1000021 || uid == 1000160);
            return View();
        }
        public JsonResult GetShareHouseList(HouseListReq parame, int isFirst)
        {
            PublicUserModel loginUser = this.GetLoginUser();
            List<CompanyModel> companyList = new List<CompanyModel>();
            List<Regions> regionList = new List<Regions>();
            if (isFirst == 1)
            {
                companyList = userBll.GetUserCompanyList(loginUser.UserID);
                regionList = ncBase.CurrentEntities.Regions.Where(r => r.CityID == loginUser.CityID && r.DistrctID == 0).ToList();
            }
            #region 获取房源列表
            HouseBll houseBll = new HouseBll();
            parame.userId = loginUser.UserID;
            int totalSize = 0;
            List<HouseBasicInfoModel> shareHouseList = houseBll.GetHouseShareList(parame, ref totalSize);
            var shareHouse = shareHouseList.Select(h => new
            {
                h.BuildArea,
                h.BuildType,
                h.CommunityName,
                h.CurFloor,
                h.CityID,
                h.Distrctid,
                h.Hall,
                HouseId = h.HouseID,
                h.Kitchen,
                h.MaxFloor,
                PicNum = h.PicNum ?? 0,
                h.Price,
                h.PriceUnit,
                h.ShareCompanyId,
                h.ShareCompanyName,
                h.ShareCompanyStoreId,
                h.ShareCompanyStoreName,
                h.ShareName,
                h.ShareTel,
                h.Room,
                h.Title,
                h.Toilet,
                h.TradeType,
                h.HouseImgPath,
                h.ShareCount,
                h.ShareIsClone,
                ShareExpireDay = h.ShareExpireDay.ToString("yyyy-MM-dd"),
                IsMyShare = h.UserID == loginUser.UserID ? 1 : 0,
                h.Tag
            });
            #endregion
            #region 区域
            var districtData = regionList.Select(o => new
            {
                o.Name,
                o.RegionID
            });
            #endregion

          
            return Json(new { allStores = companyList, buildings = shareHouse, districts = districtData, totalSize = totalSize }, JsonRequestBehavior.AllowGet);

        }
        public ActionResult GetShareBuilding()
        {
            string json =
                "{\"allStores\":[{\"orgId\":\"592009\",\"parentId\":\"592\",\"orgName\":\"丹厦房产\",\"pinyin\":null,\"shouzimu\":null},{\"orgId\":\"592009025\",\"parentId\":\"592009\",\"orgName\":\"综合战区\",\"pinyin\":null,\"shouzimu\":null},{\"orgId\":\"592009025004\",\"parentId\":\"592009025\",\"orgName\":\"祥东战区\",\"pinyin\":null,\"shouzimu\":null},{\"orgId\":\"592009025004003\",\"parentId\":\"592009025004\",\"orgName\":\"东方瑞士店\",\"pinyin\":null,\"shouzimu\":null}],\"pageCount\":9,\"buildings\":[{\"buildingId\":389249,\"userId\":5781,\"postType\":0,\"buildingType\":1,\"title\":\"禾祥西 豆仔苑、读公园，一中、南北通透、精装三房、换房急售\",\"distrct\":59201,\"cell\":\"豆仔苑\",\"houseArea\":110.00,\"price\":280.00,\"room\":3,\"hall\":2,\"toilet\":2,\"curFloor\":3,\"maxFloor\":7,\"orgId\":\"592009031002002\",\"mutilFlag\":1,\"shareOrgId\":\"592009\",\"expDate\":\"201501040000000\",\"cloneNum\":7,\"tags\":\"\",\"contacter\":\"李民\",\"phone\":\"13799750733\",\"imgUrl\":\"http://img2.xmssoft.com/592/IMG_I/20141227/10/19abf6e0ca6552773f97b6988f57db47.jpg\"},{\"buildingId\":676803,\"userId\":2841,\"postType\":0,\"buildingType\":2,\"title\":\"台湾山庄 独栋别墅，华岳山庄边，可翻建，急售大降200万\",\"distrct\":59201,\"cell\":\"台湾山庄\",\"houseArea\":431.00,\"price\":2150.00,\"room\":8,\"hall\":5,\"toilet\":4,\"curFloor\":1,\"maxFloor\":3,\"orgId\":\"592009026003005\",\"mutilFlag\":1,\"shareOrgId\":\"592009\",\"expDate\":\"201501040000000\",\"cloneNum\":114,\"tags\":\"hot\",\"contacter\":\"吴炎兴\",\"phone\":\"13459016780\",\"imgUrl\":\"http://img2.xmssoft.com/592/IMG_I/20141022/09/94ecae1368db27b324b05678891ccf7f.jpg\"},{\"buildingId\":1181884,\"userId\":23728,\"postType\":0,\"buildingType\":1,\"title\":\"宏辉大厦三房 读群惠小学 中学划片一中双十 免营业税 拎包入住\",\"distrct\":59201,\"cell\":\"宏辉大厦\",\"houseArea\":143.80,\"price\":460.00,\"room\":3,\"hall\":2,\"toilet\":2,\"curFloor\":12,\"maxFloor\":21,\"orgId\":\"592009031001001\",\"mutilFlag\":1,\"shareOrgId\":\"592009\",\"expDate\":\"201501040000000\",\"cloneNum\":1,\"tags\":\"\",\"contacter\":\"欧阳艳\",\"phone\":\"13799250278\",\"imgUrl\":\"http://img3.xmssoft.com/592/IMG_I/20141205/17/6dd5c292a884911139a9899de179fb57.jpg\"},{\"buildingId\":1184095,\"userId\":5781,\"postType\":0,\"buildingType\":1,\"title\":\"思北 信隆城 高层南北通透、急售四房 邻华侨海景城、国贸金海岸\",\"distrct\":59201,\"cell\":\"国贸信隆城\",\"houseArea\":152.70,\"price\":520.00,\"room\":4,\"hall\":2,\"toilet\":2,\"curFloor\":18,\"maxFloor\":28,\"orgId\":\"592009031002002\",\"mutilFlag\":1,\"shareOrgId\":\"592009\",\"expDate\":\"201501040000000\",\"cloneNum\":7,\"tags\":\"\",\"contacter\":\"李民\",\"phone\":\"13799750733\",\"imgUrl\":\"http://img2.xmssoft.com/592/IMG_I/20141028/22/2d540245c60742bd5e6fbe177e4fbb88.jpg\"},{\"buildingId\":1241094,\"userId\":21320,\"postType\":0,\"buildingType\":1,\"title\":\"莲花路口最新楼盘 信和中央广场 电梯高层 南北通透 业主降价急售\",\"distrct\":59201,\"cell\":\"信和中央广场\",\"houseArea\":157.17,\"price\":600.00,\"room\":3,\"hall\":2,\"toilet\":2,\"curFloor\":21,\"maxFloor\":30,\"orgId\":\"592009002008\",\"mutilFlag\":1,\"shareOrgId\":\"592009\",\"expDate\":\"201501040000000\",\"cloneNum\":27,\"tags\":\"secure,hot\",\"contacter\":\"刘少华\",\"phone\":\"18359712699\",\"imgUrl\":\"http://img2.xmssoft.com/592/IMG_O/20141218/17/d1ff4da043212fb6bfe11a1a854c1332.jpg\"},{\"buildingId\":1308462,\"userId\":21320,\"postType\":0,\"buildingType\":1,\"title\":\"鲁能领秀城 百万豪装 拎包入住 一路降价 只为成交 全朝南户型！\",\"distrct\":59201,\"cell\":\"领秀城\",\"houseArea\":193.15,\"price\":625.00,\"room\":4,\"hall\":2,\"toilet\":3,\"curFloor\":8,\"maxFloor\":32,\"orgId\":\"592009002008\",\"mutilFlag\":1,\"shareOrgId\":\"592009\",\"expDate\":\"201501040000000\",\"cloneNum\":38,\"tags\":\"secure\",\"contacter\":\"刘少华\",\"phone\":\"18359712699\",\"imgUrl\":\"http://img2.xmssoft.com/592/IMG_I/20141214/16/6a63111588578ce939e65603e7452319.jpg\"},{\"buildingId\":1330356,\"userId\":20153,\"postType\":0,\"buildingType\":1,\"title\":\"金榜公园旁 鲁能领秀城 豪装朝南3房 业主急售 全盘最低！\",\"distrct\":59201,\"cell\":\"领秀城\",\"houseArea\":157.00,\"price\":538.00,\"room\":3,\"hall\":2,\"toilet\":3,\"curFloor\":10,\"maxFloor\":30,\"orgId\":\"592009002006\",\"mutilFlag\":1,\"shareOrgId\":\"592009\",\"expDate\":\"201501040000000\",\"cloneNum\":43,\"tags\":\"secure,hot,focus\",\"contacter\":\"王东全\",\"phone\":\"18659255351\",\"imgUrl\":\"http://img2.xmssoft.com/592/IMG_I/20141215/21/b7c0a9a31bbc7b61fa495c64686a3ffa.jpg\"},{\"buildingId\":1393160,\"userId\":7585,\"postType\":0,\"buildingType\":1,\"title\":\"富山省优槟榔名校框架房《湖明新村》高档装修，带露台，正南北\",\"distrct\":59201,\"cell\":\"湖明新村\",\"houseArea\":76.39,\"price\":225.00,\"room\":2,\"hall\":2,\"toilet\":2,\"curFloor\":4,\"maxFloor\":7,\"orgId\":\"592009031003003\",\"mutilFlag\":1,\"shareOrgId\":\"592009\",\"expDate\":\"201501040000000\",\"cloneNum\":5,\"tags\":\"secure,hot,new\",\"contacter\":\"韩梅华\",\"phone\":\"13720891507\",\"imgUrl\":\"http://img3.xmssoft.com/592/IMG_I/20141116/14/091c4727fd2687db0099046cbfbc736d.jpg\"},{\"buildingId\":1499545,\"userId\":10473,\"postType\":0,\"buildingType\":1,\"title\":\"满5年 玫瑰园，滨北小学，大3房，338万，带电梯，繁华地段\",\"distrct\":59201,\"cell\":\"玫瑰园\",\"houseArea\":130.00,\"price\":338.00,\"room\":3,\"hall\":2,\"toilet\":2,\"curFloor\":5,\"maxFloor\":30,\"orgId\":\"592009002002\",\"mutilFlag\":1,\"shareOrgId\":\"592009\",\"expDate\":\"201501040000000\",\"cloneNum\":16,\"tags\":\"\",\"contacter\":\"黄智城\",\"phone\":\"13774668916\",\"imgUrl\":\"http://img2.xmssoft.com/592/IMG_I/20141227/10/febe5d12054a7564e1b095b197d41227.jpg\"},{\"buildingId\":1501598,\"userId\":7585,\"postType\":0,\"buildingType\":1,\"title\":\"大唐世家地铁口免税房源加州花园3房带入户电梯138售310万\",\"distrct\":59202,\"cell\":\"加州花园\",\"houseArea\":137.80,\"price\":305.00,\"room\":3,\"hall\":2,\"toilet\":2,\"curFloor\":6,\"maxFloor\":9,\"orgId\":\"592009031003003\",\"mutilFlag\":1,\"shareOrgId\":\"592009\",\"expDate\":\"201501040000000\",\"cloneNum\":0,\"tags\":\"secure,hot,new\",\"contacter\":\"韩梅华\",\"phone\":\"13720891507\",\"imgUrl\":\"http://img1.xmssoft.com/592/IMG_I/20141118/18/6d5f184255c8dc2a21c55e267ee04046.jpg\"}],\"districts\":[{\"postSiteId\":59201,\"parentId\":592,\"siteName\":\"思明\"},{\"postSiteId\":59202,\"parentId\":592,\"siteName\":\"湖里\"},{\"postSiteId\":59203,\"parentId\":592,\"siteName\":\"集美\"},{\"postSiteId\":59204,\"parentId\":592,\"siteName\":\"海沧\"},{\"postSiteId\":59205,\"parentId\":592,\"siteName\":\"同安\"},{\"postSiteId\":59206,\"parentId\":592,\"siteName\":\"翔安\"}],\"tagList\":[{\"tagId\":1,\"tagName\":\"secure\",\"tagDesc\":\"放心\",\"icon\":\"http://static.xmssoft.com/common/images/tags/secure.gif\"},{\"tagId\":2,\"tagName\":\"hot\",\"tagDesc\":\"急推\",\"icon\":\"http://static.xmssoft.com/common/images/tags/hot.gif\"},{\"tagId\":3,\"tagName\":\"new\",\"tagDesc\":\"新房\",\"icon\":\"http://static.xmssoft.com/common/images/tags/new.gif\"},{\"tagId\":4,\"tagName\":\"focus\",\"tagDesc\":\"集攻\",\"icon\":\"http://static.xmssoft.com/common/images/tags/focus.gif\"}]}";
            return Content(json);
        }
        public ActionResult GetHouesShareCompany(List<int> houseId, int from = 1)
        {
            PublicUserModel loginUser = this.GetLoginUser();
            List<CompanyModel> userCompanyList = new List<CompanyModel>();
            userCompanyList = userBll.GetUserCompanyList(loginUser.UserID);
            userCompanyList = userCompanyList.Where(o => o.ParentId == 0 || o.CompanyId == loginUser.StoreId).ToList();
            int oneHouseId = 0;
            int shareCompanyId = 0;
            int ShareCompanyStoreId = 0;
            if (houseId.Count == 1)
            {
                oneHouseId = houseId[0];
                HouseBasicInfoShare shareHouseDetail = ncBase.CurrentEntities.HouseBasicInfoShare.Where(s => s.ShareHouseId == oneHouseId && s.ShareUserId == loginUser.UserID).FirstOrDefault();
                if (shareHouseDetail.IsNoNull())
                {
                    shareCompanyId = shareHouseDetail.ShareCompanyId ?? 0;
                    ShareCompanyStoreId = shareHouseDetail.ShareCompanyStoreId ?? 0;
                }
            }

            ViewBag.ShareCompanyId = shareCompanyId;
            ViewBag.ShareCompanyStoreId = ShareCompanyStoreId;
            ViewBag.UserCompanyId = loginUser.CompanyId;
            ViewBag.HouseId = string.Join(",", houseId);
            ViewBag.IsFrom = from;
            return View(userCompanyList);
        }
        /// <summary>
        /// 分享到总店还是分店
        /// </summary>
        /// <param name="houseId"></param>
        /// <param name="isStore">-100 不共享</param>
        /// <returns></returns>
        public JsonResult EditHouseShareCompany(string houseId, int isStore)
        {
            PublicUserModel loginUser = this.GetLoginUser();
            if (loginUser.CompanyId == 0 && loginUser.StoreId == 0)
            {
                return Json(new { msg = "您没有所属公司", code = -100 });
            }
            List<string> houseIds = houseId.Split(',').ToList();
            int successCount = 0;
            foreach (string item in houseIds)
            {
                int thisHouseId = 0;
                int.TryParse(item, out thisHouseId);
                int shareCompanyId = 0;
                int shareCompanyStoreId = 0;
                HouseBasicInfo thisHouse = ncBase.CurrentEntities.HouseBasicInfo.Where(s => s.HouseID == thisHouseId && loginUser.UserID == s.UserID && (s.BeColneHouseID == 0 || s.BeColneHouseID == null)).FirstOrDefault();
                if (thisHouse.IsNoNull())
                {
                    if (isStore == 1)
                    {
                        if (loginUser.StoreId > 0)
                        {
                            shareCompanyId = 0;
                            shareCompanyStoreId = (int)loginUser.StoreId;
                        }
                        else
                        {
                            shareCompanyId = (int)loginUser.CompanyId;
                            shareCompanyStoreId = 0;
                        }
                    }
                    else if (isStore == 0)
                    {
                        shareCompanyId = (int)loginUser.CompanyId;
                        shareCompanyStoreId = 0;
                    }
                    HouseBasicInfoShare thisShare = ncBase.CurrentEntities.HouseBasicInfoShare.Where(s => s.ShareHouseId == thisHouseId).FirstOrDefault();
                    if (thisShare.IsNoNull())
                    {

                        if (isStore == -100)
                        {
                            thisShare.ShareStatus = 0;
                        }
                        else
                        {
                            thisShare.ShareCompanyId = shareCompanyId;
                            thisShare.ShareCompanyStoreId = shareCompanyStoreId;
                            thisShare.ShareStatus = 1;
                            thisShare.ShareExpireDay = DateTime.Now.AddDays(7);
                        }
                        ncBase.CurrentEntities.SaveChanges();
                    }
                    else if (isStore != -100)
                    {
                        HouseBasicInfoShare newShare = new HouseBasicInfoShare()
                        {
                            ShareAddTime = DateTime.Now,
                            ShareCompanyId = shareCompanyId,
                            ShareCompanyStoreId = shareCompanyStoreId,
                            ShareHouseId = thisHouseId,
                            ShareCount = 0,
                            ShareExpireDay = DateTime.Now.AddDays(7),
                            ShareUserId = loginUser.UserID,
                            ShareStatus = 1
                        };
                        ncBase.CurrentEntities.HouseBasicInfoShare.AddObject(newShare);
                        ncBase.CurrentEntities.SaveChanges();
                    }
                    successCount++;

                }

            }
            if (successCount > 0)
            {
             
                return Json(new { msg = "成功" + successCount + "条", code = 1 });
            }
            else
            {
                return Json(new { msg = "房源不存在", code = -1 });
            }
        }
        #endregion

        #region 房源搬家

        public ActionResult MoveHouse()
        {
            int uid = this.GetLoginUser().UserID;
            UserSiteManage userSiteManage =
               ncBase.CurrentEntities.UserSiteManage.Where(
                   o => o.SiteID == 1 && o.UserID == uid)
                   .FirstOrDefault();
            ViewBag.SiteUserName = userSiteManage.IsNoNull() ? userSiteManage.SiteUserName : "";
            ViewBag.IsDebug = (uid <= 1000021 || uid == 1000160);
            UserSiteManageBll userSiteManageBll = new UserSiteManageBll();
            //  List<UserSiteManageModel> userSiteManageList = userSiteManageBll.GetUserSiteListByUserId(uid);//用户关联的站点

            return View();
        }
        public ActionResult SnatchWeb(ImportedHouseListReq parame)
        {
            IEnumerable<ImportedHouse> houses = new List<ImportedHouse>();
            PublicUserModel loginUser = this.GetLoginUser();
            UserSiteManage userSiteManage =
               ncBase.CurrentEntities.UserSiteManage.Where(
                   o => o.SiteID == parame.SiteId && o.UserID == loginUser.UserID)
                   .FirstOrDefault();

            ViewBag.SiteUserName = userSiteManage.IsNoNull() ? userSiteManage.SiteUserName : "";
            int totalSize = 0;
            if (userSiteManage.IsNoNull())
            {

                string cookie = string.Empty;

                //string cookie = Loginer_Xms.Login(userSiteManage.SiteUserName,
                //    CryptoUtility.TripleDESDecrypt(userSiteManage.SiteUserPwd));

                //if (!cookie.Contains("userInfo")) return Content("登陆失败"); //登录失败应该放到View那边，弄点样式
                parame.UserId = loginUser.UserID;
                parame.SiteUserName = userSiteManage.SiteUserName;
                houses = mogoHelper.GetImportHouseList(parame, ref totalSize);
            }
            ViewBag.TotalSize = totalSize;
            ViewBag.Parame = parame;
            ViewBag.UserId = loginUser.UserID;
            return View(houses);
        }

        public ActionResult Sync(int webId = 0, byte tradeType = 1, int buildingType = 1)
        {
            PublicUserModel loginUser = this.GetLoginUser();

            IEnumerable<ImportedHouse> houses = new List<ImportedHouse>();
            int uid = this.GetLoginUser().UserID;
            UserSiteManage userSiteManage =
               ncBase.CurrentEntities.UserSiteManage.Where(
                   o => o.SiteID == webId && o.UserID == uid)
                   .FirstOrDefault();

            ViewBag.SiteUserName = userSiteManage.IsNoNull() ? userSiteManage.SiteUserName : "";

            if (userSiteManage.IsNoNull())
            {
                string cookie = string.Empty;
                string prefix = string.Empty;
                string responErrorContent = string.Empty;
                int size = 999;

                switch (webId)
                {
                    case 1008:
                        prefix = "xms";
                        cookie = Loginer_Xms.Login(userSiteManage.SiteUserName, CryptoUtility.TripleDESDecrypt(userSiteManage.SiteUserPwd), null);
                        if (!cookie.Contains("userInfo")) return Content("登陆失败");//登录失败应该放到View那边，弄点样式
                        System.Net.ServicePointManager.Expect100Continue = false;
                        Dictionary<int, string> cList = GetCommunityList();//自己数据库的小区id和名称的映射
                        houses = Lister_Xms.GetHouseDetails(tradeType, buildingType, cookie, size, cList);
                        break;
                    case 1:
                        prefix = "ZJB";
                        cookie = Loginer_ZJB.Login(userSiteManage.SiteUserName, CryptoUtility.TripleDESDecrypt(userSiteManage.SiteUserPwd));
                        if (string.IsNullOrEmpty(cookie)) return Content("登陆失败");//登录失败应该放到View那边，弄点样式
                        System.Net.ServicePointManager.Expect100Continue = false;
                        houses = Lister_ZJB.GetHouseDetails(tradeType, cookie, size);
                        break;
                    case 2:
                        prefix = "soufunM";
                        cookie = Loginer_SoufunM.Login(userSiteManage.SiteUserName, CryptoUtility.TripleDESDecrypt(userSiteManage.SiteUserPwd));
                        if (!cookie.Contains("登录成功")) return Content("登陆失败");//登录失败应该放到View那边，弄点样式
                        System.Net.ServicePointManager.Expect100Continue = false;
                        houses = Lister_SoufunM.GetHouseDetails(tradeType, buildingType, cookie, size);
                        break;
                }
                //Todo:这边写数据到Mogo
                if (houses != null && houses.Count() > 0)
                {
                    mogoHelper.AddImportHouse(houses, webId, loginUser.UserID, string.IsNullOrEmpty(prefix) ? webId.ToString() : prefix, userSiteManage.SiteUserName);
                }
            }
            return Content("");
        }
        /// <summary>
        /// 导入房源
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public JsonResult ImportHouse(List<string> hids)
        {
            PublicUserModel loginUser = this.GetLoginUser();
            IEnumerable<ImportedHouse> houseListInMongo = mogoHelper.GetImportHouseListByIds(hids, loginUser.UserID);
            HouseBll houseBll = new HouseBll();
            int count = 0;
            //foreach (ImportedHouse item in houseListInMongo)
            //{
            //    item.MoveStatus = 1;///正在导入
            //    mogoHelper.SaveImportHouse(item);
            //}
            foreach (ImportedHouse item in houseListInMongo)
            {

                #region 房源基础信息表
                ImportedHouseRefUser thisRefUser = item.RefUser.Where(u => u.UserID == loginUser.UserID).FirstOrDefault();
                if (thisRefUser == null)
                {
                    thisRefUser = new ImportedHouseRefUser()
                    {
                        UserID = loginUser.UserID,
                        MoveStatus = 0,
                        MoveHouseId = 0
                    };
                    item.RefUser.Add(thisRefUser);
                }
                HouseParame houseParame = new HouseParame()
                {

                    Address = item.Address,
                    Balcony = item.Balcony,
                    BuildArea = item.BuildArea,
                    BuildType = item.BuildType,
                    CellLabel = item.CellLabel,
                    CityId = item.CityID,
                    CommunityId = item.CommunityID,
                    CommunityName = item.CommunityName,
                    CurFloor = item.CurFloor,
                    Distrctid = item.Distrctid,
                    FitmentStatus = item.FitmentStatus,
                    Hall = item.Hall,
                    HouseId = thisRefUser.MoveHouseId,
                    HouseImgs = SetImageList(item, 0),
                    HouseLabel = item.HouseLabel,
                    IsClone = false,
                    BeColneID = item.HouseID,
                    Kitchen = item.Kitchen,
                    LookHouseTime = item.LookHouseTime,
                    MaxFloor = item.MaxFloor,
                    Note = item.Note,
                    PayType = item.PayType,
                    PicNum = (item.roomImages == null ? 0 : item.roomImages.Length) + (item.fangxingImages == null ? 0 : item.fangxingImages.Length) + (item.outdoorImages == null ? 0 : item.outdoorImages.Length),
                    PointTo = item.PointTo,
                    Price = item.Price,
                    PriceUnit = item.PriceUnit,
                    RegionId = item.RegionID ?? 0,
                    Room = item.Room,
                    Title = item.Title,
                    Toilet = item.Toilet,
                    TradeType = item.TradeType,
                    UsedArea = item.UsedArea,
                    UsedYear = item.UsedYear ?? 0,
                    UserId = loginUser.UserID,
                    YiJuHua = item.YiJuHua
                };
                #endregion
                #region 房源 住宅
                HouseInfoParame houseInfoParame = new HouseInfoParame();
                if (item.houseInfo.IsNoNull())
                {
                    houseInfoParame = new HouseInfoParame()
                    {
                        AdvEquip = item.houseInfo.AdvEquip,
                        BasicEquipHouse = item.houseInfo.BasicEquip,
                        FiveYears = item.houseInfo.FiveYears ?? false,
                        HouseProperty = item.houseInfo.HouseProperty,
                        HouseStructure = item.houseInfo.HouseStructure,
                        HouseSubType = item.houseInfo.HouseSubType,
                        HouseType = item.houseInfo.HouseType,
                        LandYear = item.houseInfo.LandYear,
                        OnlyHouse = item.houseInfo.OnlyHouse ?? false
                    };
                }
                #endregion
                #region 房源 别墅
                VillaInfoParame villaInfoParame = new VillaInfoParame();
                if (item.villaInfo.IsNoNull())
                {
                    villaInfoParame = new VillaInfoParame()
                    {
                        AdvEquip = item.villaInfo.AdvEquip,
                        Basement = item.villaInfo.Basement ?? false,
                        BasicEquip = item.villaInfo.BasicEquip,
                        FiveYears = item.villaInfo.FiveYears ?? false,
                        Garage = item.villaInfo.Garage ?? false,
                        Garden = item.villaInfo.Garden ?? false,
                        HallType = item.villaInfo.HallType,
                        LandYear = item.villaInfo.LandYear,
                        OnlyHouse = item.villaInfo.OnlyHouse ?? false,
                        ParkLot = item.villaInfo.ParkLot ?? false,
                        VillaType = item.villaInfo.VillaType
                    };
                }
                #endregion
                #region 房源 商铺
                ShopInfoParame shopInfoParame = new ShopInfoParame();
                if (item.shopInfo.IsNoNull())
                {
                    shopInfoParame = new ShopInfoParame()
                    {
                        BasicEquip = item.shopInfo.BasicEquip,
                        Divide = item.shopInfo.Divide ?? false,
                        Fee = item.shopInfo.Fee ?? 0,
                        ShopStatus = item.shopInfo.ShopStatus,
                        ShopType = item.shopInfo.ShopType,
                        TargetField = item.shopInfo.TargetField,
                    };
                }
                #endregion
                #region 房源 写字楼
                OfficeInfoParame officeInfoParame = new OfficeInfoParame();
                if (item.officeInfo.IsNoNull())
                {
                    officeInfoParame = new OfficeInfoParame()
                    {
                        BasicEquip = item.officeInfo.BasicEquip,
                        Divide = item.officeInfo.Divide ?? false,
                        Fee = item.officeInfo.Fee ?? 0,
                        OfficeLevel = item.officeInfo.OfficeLevel,
                        OfficeType = item.officeInfo.OfficeType
                    };
                }
                #endregion
                #region 房源 工厂
                FactoryInfoParame factoryInfoParame = new FactoryInfoParame();
                if (item.factoryInfo.IsNoNull())
                {
                    factoryInfoParame = new FactoryInfoParame()
                    {
                        BasicEquip = item.factoryInfo.BasicEquip,
                        FactoryType = item.factoryInfo.FactoryType
                    };
                }
                #endregion
                //ToDo:赋值
                int houseid = houseBll.OperateHouse(houseParame, houseInfoParame, villaInfoParame, shopInfoParame, officeInfoParame, factoryInfoParame);
                if (houseid > 0)
                {
                    count++;
                    // item.MoveStatus = 2;///导入完成
                    // item.MoveHouseId = houseid;
                    thisRefUser.MoveStatus = 2;
                    thisRefUser.MoveHouseId = houseid;
                    mogoHelper.SaveImportHouse(item);
                }
            }
            return Json(new { count = count });
        }
        /// <summary>
        /// 导入房源
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public JsonResult ImportHouseBatch(List<string> hids)
        {
            PublicUserModel loginUser = this.GetLoginUser();
            IEnumerable<ImportedHouse> houseListInMongo = mogoHelper.GetImportHouseListByIds(hids, loginUser.UserID);
            HouseBll houseBll = new HouseBll();
            int count = 0;
            foreach (ImportedHouse item in houseListInMongo)
            {
                item.MoveStatus = 1;//正在导入
                mogoHelper.SaveImportHouse(item);
            }
            List<HouseParame> houseParameList = new List<HouseParame>();
            List<HouseInfoParame> houseInfoParameList = new List<HouseInfoParame>();
            List<VillaInfoParame> villaInfoParameList = new List<VillaInfoParame>();
            List<ShopInfoParame> shopInfoParameList = new List<ShopInfoParame>();
            List<OfficeInfoParame> officInfoParameList = new List<OfficeInfoParame>();
            List<FactoryInfoParame> factoryInfoParameList = new List<FactoryInfoParame>();
            List<HouseImgParame> houseImgParameList = new List<HouseImgParame>();
            int indexPoint = 1;
            foreach (ImportedHouse item in houseListInMongo)
            {
                List<HouseImgParame> houseImgList = new List<HouseImgParame>();
                houseImgList = SetImageList(item, indexPoint);

                ImportedHouseRefUser thisRefUser = item.RefUser.Where(u => u.UserID == loginUser.UserID).FirstOrDefault();
                if (thisRefUser == null)
                {
                    thisRefUser = new ImportedHouseRefUser()
                    {
                        UserID = loginUser.UserID,
                        MoveStatus = 0,
                        MoveHouseId = 0
                    };
                    item.RefUser.Add(thisRefUser);
                }

                #region 房源基础信息表
                HouseParame houseParame = new HouseParame()
                {

                    Address = item.Address,
                    Balcony = item.Balcony,
                    BuildArea = item.BuildArea,
                    BuildType = item.BuildType,
                    CellLabel = item.CellLabel,
                    CityId = item.CityID,
                    CommunityId = item.CommunityID,
                    CommunityName = item.CommunityName,
                    CurFloor = item.CurFloor,
                    Distrctid = item.Distrctid,
                    FitmentStatus = item.FitmentStatus,
                    Hall = item.Hall,
                    HouseId = thisRefUser.MoveHouseId,
                    HouseLabel = item.HouseLabel,
                    HouseImgs = houseImgList,
                    IsClone = false,
                    BeColneID = item.HouseID,
                    Kitchen = item.Kitchen,
                    LookHouseTime = item.LookHouseTime,
                    MaxFloor = item.MaxFloor,
                    Note = item.Note,
                    PayType = item.PayType ?? "",
                    PicNum = 0,
                    PointTo = item.PointTo,
                    Price = item.Price,
                    PriceUnit = item.PriceUnit,
                    RegionId = item.RegionID ?? 0,
                    Room = item.Room,
                    Title = item.Title,
                    Toilet = item.Toilet,
                    TradeType = item.TradeType,
                    UsedArea = item.UsedArea,
                    UsedYear = item.UsedYear ?? 0,
                    UserId = loginUser.UserID,
                    YiJuHua = item.YiJuHua,
                    IndexPoint = indexPoint
                };
                #endregion
                #region 房源 住宅
                HouseInfoParame houseInfoParame = new HouseInfoParame();
                if (item.houseInfo.IsNoNull())
                {
                    houseInfoParame = new HouseInfoParame()
                    {
                        AdvEquip = item.houseInfo.AdvEquip,
                        BasicEquip = item.houseInfo.BasicEquip,
                        FiveYears = item.houseInfo.FiveYears ?? false,
                        HouseProperty = item.houseInfo.HouseProperty,
                        HouseStructure = item.houseInfo.HouseStructure,
                        HouseSubType = item.houseInfo.HouseSubType,
                        HouseType = item.houseInfo.HouseType,
                        LandYear = item.houseInfo.LandYear,
                        OnlyHouse = item.houseInfo.OnlyHouse ?? false,
                        HouseID = item.MoveHouseId,
                        IndexPoint = indexPoint
                    };
                }
                #endregion
                #region 房源 别墅
                VillaInfoParame villaInfoParame = new VillaInfoParame();
                if (item.villaInfo.IsNoNull())
                {
                    villaInfoParame = new VillaInfoParame()
                    {
                        AdvEquip = item.villaInfo.AdvEquip,
                        Basement = item.villaInfo.Basement ?? false,
                        BasicEquip = item.villaInfo.BasicEquip,
                        FiveYears = item.villaInfo.FiveYears ?? false,
                        Garage = item.villaInfo.Garage ?? false,
                        Garden = item.villaInfo.Garden ?? false,
                        HallType = item.villaInfo.HallType,
                        LandYear = item.villaInfo.LandYear,
                        OnlyHouse = item.villaInfo.OnlyHouse ?? false,
                        ParkLot = item.villaInfo.ParkLot ?? false,
                        VillaType = item.villaInfo.VillaType,
                        IndexPoint = indexPoint,
                        HouseID = item.MoveHouseId
                    };
                }
                #endregion
                #region 房源 商铺
                ShopInfoParame shopInfoParame = new ShopInfoParame();
                if (item.shopInfo.IsNoNull())
                {
                    shopInfoParame = new ShopInfoParame()
                    {
                        BasicEquip = item.shopInfo.BasicEquip,
                        Divide = item.shopInfo.Divide ?? false,
                        Fee = item.shopInfo.Fee ?? 0,
                        ShopStatus = item.shopInfo.ShopStatus,
                        ShopType = item.shopInfo.ShopType,
                        TargetField = item.shopInfo.TargetField,
                        IndexPoint = indexPoint,
                        HouseID = item.MoveHouseId
                    };
                }
                #endregion
                #region 房源 写字楼
                OfficeInfoParame officeInfoParame = new OfficeInfoParame();
                if (item.officeInfo.IsNoNull())
                {
                    officeInfoParame = new OfficeInfoParame()
                    {
                        BasicEquip = item.officeInfo.BasicEquip,
                        Divide = item.officeInfo.Divide ?? false,
                        Fee = item.officeInfo.Fee ?? 0,
                        OfficeLevel = item.officeInfo.OfficeLevel,
                        OfficeType = item.officeInfo.OfficeType,
                        IndexPoint = indexPoint,
                        HouseID = item.MoveHouseId
                    };
                }
                #endregion
                #region 房源 工厂
                FactoryInfoParame factoryInfoParame = new FactoryInfoParame();
                if (item.factoryInfo.IsNoNull())
                {
                    factoryInfoParame = new FactoryInfoParame()
                    {
                        BasicEquip = item.factoryInfo.BasicEquip,
                        FactoryType = item.factoryInfo.FactoryType,
                        IndexPoint = indexPoint,
                        HouseID = item.MoveHouseId
                    };
                }
                #endregion
                //ToDo:赋值
                houseParameList.Add(houseParame);
                houseInfoParameList.Add(houseInfoParame);
                villaInfoParameList.Add(villaInfoParame);
                shopInfoParameList.Add(shopInfoParame);
                officInfoParameList.Add(officeInfoParame);
                factoryInfoParameList.Add(factoryInfoParame);
                houseImgParameList.AddRange(houseImgList);

                indexPoint++;
            }
            List<ImpHouseResultModel> impResultList = houseBll.ImpHouseBatch(houseParameList, houseInfoParameList, villaInfoParameList, shopInfoParameList, officInfoParameList, factoryInfoParameList, houseImgParameList, loginUser.UserID);
            if (impResultList.IsNoNull() && impResultList.Count > 0)
            {
                foreach (ImpHouseResultModel itemResult in impResultList)
                {

                    ImportedHouse itemImport = houseListInMongo.Where(o => o.HouseID == itemResult.BeCloneId).FirstOrDefault();
                    if (itemImport.IsNoNull())
                    {
                        count++;
                        ImportedHouseRefUser thisRefUser = itemImport.RefUser.Where(u => u.UserID == loginUser.UserID).FirstOrDefault();
                        if (thisRefUser == null)
                        {
                            thisRefUser = new ImportedHouseRefUser()
                            {
                                UserID = loginUser.UserID,
                                MoveStatus = 0,
                                MoveHouseId = 0
                            };
                            itemImport.RefUser.Add(thisRefUser);
                        }
                        thisRefUser.MoveStatus = 2;///导入完成
                        thisRefUser.MoveHouseId = itemResult.HouseId;
                        mogoHelper.SaveImportHouse(itemImport);
                    }
                }
            }
            return Json(new { count = count });
        }
        private List<HouseImgParame> SetImageList(ImportedHouse item, int indexPoint)
        {
            List<HouseImgParame> list = new List<HouseImgParame>();
            bool isLocalHost = Request.Url.Host.Contains("localhost") || Request.UserHostAddress == "127.0.0.1";
            for (int i = 0; i < item.roomImages.Length; i++)
            {
                try
                {
                    string url = ImageHelper.GrabPic(item.roomImages[i], isLocalHost);
                    if (!string.IsNullOrEmpty(item.roomImages[i]))
                    {
                        list.Add(new HouseImgParame()
                        {
                            imageUrl = url,
                            imageType = 1,
                            ImageType = 1,
                            IndexPoint = indexPoint,
                            IsCover = i == 0 ? true : false
                        });
                    }
                }
                catch (Exception ex)
                {

                }
            }
            for (int i = 0; i < item.fangxingImages.Length; i++)
            {
                try
                {
                    string url = ImageHelper.GrabPic(item.fangxingImages[i], isLocalHost);
                    if (!string.IsNullOrEmpty(item.fangxingImages[i]))
                    {
                        list.Add(new HouseImgParame()
                        {
                            imageUrl = url,
                            imageType = 2,
                            ImageType = 2,
                            IndexPoint = indexPoint
                        });
                    }
                }
                catch (Exception ex)
                {

                }
            }
            for (int i = 0; i < item.outdoorImages.Length; i++)
            {
                try
                {
                    string url = ImageHelper.GrabPic(item.outdoorImages[i], isLocalHost);
                    if (!string.IsNullOrEmpty(item.outdoorImages[i]))
                    {
                        list.Add(new HouseImgParame()
                        {
                            imageUrl = url,
                            imageType = 3,
                            ImageType = 3,
                            IndexPoint = indexPoint
                        });
                    }
                }
                catch (Exception ex)
                {

                }
            }
            return list;
        }

        #endregion

        #region 添加描述
        [HttpPost]
        [ValidateInput(false)]
        public JsonResult AddHouseDesc(string descName, string descContent, int postType, int id = 0)
        {

            bool flag = false;
            HouseUserDescribe houseUserDescribe = new HouseUserDescribe();
            int uid = this.GetLoginUser().UserID;
            if (id > 0)
            {
                houseUserDescribe = ncBase.CurrentEntities.HouseUserDescribe.Where(o => o.Id == id && o.UserId == uid).FirstOrDefault();

                if (houseUserDescribe.IsNull())
                {
                    houseUserDescribe = new HouseUserDescribe();
                }
                else
                {
                    flag = true;
                }
            }

            houseUserDescribe.Title = descName.IsNoNull() ? descName : ""; ;
            houseUserDescribe.HouseDescribe = descContent.IsNoNull() ? descContent : "";
            houseUserDescribe.TradeType = postType;
            houseUserDescribe.UserId = this.GetLoginUser().UserID;
            houseUserDescribe.AddTime = DateTime.Now;
            if (!flag)
            {
                int count = ncBase.CurrentEntities.HouseUserDescribe.Where(o => o.UserId == uid).Count();
                if (count > 19)
                {
                    return Json(new { hid = 0, msg = "您最多只能保存20套房源描述模板！" });
                }
                ncBase.CurrentEntities.AddToHouseUserDescribe(houseUserDescribe);
            }
            ncBase.CurrentEntities.SaveChanges();

            return Json(new { hid = houseUserDescribe.Id, msg = "" });


        }
        #endregion

        #region 房源描述

        public ActionResult GetHouseDescs(int postType = 0, int pageIndex = 1, int pageSize = 5)
        {

            string sqlwhere = "it.UserId=" + this.GetLoginUser().UserID;

            if (postType > 0)
            {
                sqlwhere += " And (it.TradeType=0 or it.TradeType=" + postType + ") ";
            }

            List<HouseUserDescribe> houseUserDescribes =
                 ncBase.CurrentEntities.HouseUserDescribe.Where(sqlwhere)
                     .OrderByDescending(o => o.Id)
                     .Skip(pageSize * (pageIndex - 1))
                     .Take(pageSize)
                     .ToList();
            ViewBag.Count = ncBase.CurrentEntities.HouseUserDescribe.Where(sqlwhere).Count();
            ViewBag.PageIndex = pageIndex;
            ViewBag.PageSize = pageSize;
            ViewBag.PageTotal = (int)Math.Ceiling((double)ViewBag.Count / pageSize);
            return View(houseUserDescribes);

        }

        public ActionResult GetHouseDescsByPage(int postType = 0, int pageNow = 1, int pageSize = 5, string title = "")
        {

            string sqlwhere = "it.UserId=" + this.GetLoginUser().UserID;
            if (!string.IsNullOrEmpty(title))
            {
                sqlwhere += " and it.Title like '%" + title + "%'";
            }
            if (postType > 0)
            {
                sqlwhere += " And (it.TradeType=0 or it.TradeType=" + postType + ") ";
            }

            List<HouseUserDescribe> houseUserDescribes =
                 ncBase.CurrentEntities.HouseUserDescribe.Where(sqlwhere)
                     .OrderByDescending(o => o.UserId)
                     .Skip(pageSize * (pageNow - 1))
                     .Take(pageSize)
                     .ToList();
            ViewBag.Count = ncBase.CurrentEntities.HouseUserDescribe.Where(sqlwhere).Count();
            ViewBag.PageIndex = pageNow;
            ViewBag.PageSize = pageSize;
            ViewBag.PageTotal = (int)Math.Ceiling((double)ViewBag.Count / pageSize);
            return View(houseUserDescribes);

        }
        public ActionResult GetAllHouseDescs()
        {

            string sqlwhere = "it.UserId=" + this.GetLoginUser().UserID;

            int pageIndex = 1;
            int pageSize = 50;

            List<HouseUserDescribe> houseUserDescribes =
                 ncBase.CurrentEntities.HouseUserDescribe.Where(sqlwhere)
                     .OrderByDescending(o => o.Id)
                     .Skip(pageSize * (pageIndex - 1))
                     .Take(pageSize)
                     .ToList();
            ViewBag.Count = ncBase.CurrentEntities.HouseUserDescribe.Where(sqlwhere).Count();
            ViewBag.PageIndex = pageIndex;
            ViewBag.PageSize = pageSize;
            ViewBag.PageTotal = (int)Math.Ceiling((double)ViewBag.Count / pageSize);
            return View(houseUserDescribes);


        }


        public JsonResult GetHouseDesc(int id)
        {

            HouseUserDescribe houseUserDescribe = ncBase.CurrentEntities.HouseUserDescribe.Where(o => o.Id == id).FirstOrDefault();

            if (houseUserDescribe.IsNull())
            {

                houseUserDescribe = new HouseUserDescribe();

            }
            return Json(houseUserDescribe, JsonRequestBehavior.AllowGet);

        }
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult RemoveHouseDesc(int hid = 0)
        {


            HouseUserDescribe houseUserDescribe = new HouseUserDescribe();
            int uid = this.GetLoginUser().UserID;
            if (hid > 0)
            {
                houseUserDescribe = ncBase.CurrentEntities.HouseUserDescribe.Where(o => o.Id == hid && o.UserId == uid).FirstOrDefault();

                if (houseUserDescribe.IsNoNull())
                {
                    ncBase.CurrentEntities.DeleteObject(houseUserDescribe);
                    ncBase.CurrentEntities.SaveChanges();
                    return Content("删除成功");
                }

            }

            return Content("删除失败");


        }

        #region 帮我写描述
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult GetDescView(FormCollection form)
        {

            string sqlwhere = "it.Status=1";

            int pageSize = 10;

            List<HouseDescribe> houseDescribes =
                 ncBase.CurrentEntities.HouseDescribe.Where(sqlwhere).OrderBy(o => Guid.NewGuid())
                     .Take(pageSize)
                     .ToList();

            return View(houseDescribes);

        }

        [HttpPost]
        [ValidateInput(false)]
        public JsonResult GetDesc(FormCollection form)
        {

            string sqlwhere = "it.Status=1";

            int pageSize = 10;

            List<HouseDescribe> houseDescribes =
                 ncBase.CurrentEntities.HouseDescribe.Where(sqlwhere).OrderBy(o => Guid.NewGuid())
                     .Take(pageSize)
                     .ToList();
            var list = houseDescribes.Select(h => new { h.Describe });

            return Json(list, JsonRequestBehavior.AllowGet);

        }
        #endregion

        #endregion

        #region 房源标题
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult GetTitleView(FormCollection form)
        {

            string sqlwhere = "it.Status=1";

            int pageSize = 10;

            List<HouseTitle> houseTitles =
                 ncBase.CurrentEntities.HouseTitle.Where(sqlwhere).OrderBy(o => Guid.NewGuid())
                     .Take(pageSize)
                     .ToList();

            return View(houseTitles);

        }

        [HttpPost]
        [ValidateInput(false)]
        public JsonResult GetTitle(FormCollection form)
        {

            string sqlwhere = "it.Status=1";

            int pageSize = 10;

            List<HouseTitle> houseTitles =
                 ncBase.CurrentEntities.HouseTitle.Where(sqlwhere).OrderBy(o => Guid.NewGuid())
                     .Take(pageSize)
                     .ToList();
            var list = houseTitles.Select(h => new { h.Title });

            return Json(list, JsonRequestBehavior.AllowGet);

        }

        #endregion

        #region 房源收藏页面和列表数据
        public ActionResult HouseCollection(int city = 592, int postType = 1)
        {
            int pi = 1;
            int ps = 10;
            ViewBag.tradeType = postType;
            ViewBag.pi = pi;
            ViewBag.ps = ps;
            ViewBag.city = city;
            return View();
        }
        public JsonResult GetHouseCollectionList(HouseListReq parames)
        {
            PublicUserModel thisUser = this.GetLoginUser();
            string userName = thisUser.Name;
            int userId = thisUser.UserID;
            HouseBll houseBll = new HouseBll();
            parames.userId = userId;
            parames.page = parames.page == 0 ? 1 : parames.page;
            parames.pageSize = parames.pageSize == 0 ? 10 : parames.pageSize;
            parames.userId = userId;
            int totalSize = 0;
            List<HouseCollection> houseList = houseBll.GetCollectionList(parames, ref totalSize);//房源列表

            var resultList = houseList.Select(h => new
            {
                h.Address,
                h.Balcony,
                h.BuildArea,
                h.BuildingType,
                h.CommunityName,
                CurFloor = h.CurFloor ?? 0,
                h.CityId,
                h.Distrctid,
                h.DistrctName,
                h.Hall,
                h.Id,
                h.Kitchen,
                MaxFloor = h.MaxFloor ?? 0,
                PicNum = h.PicNum ?? 0,
                h.Price,
                h.PriceUnit,
                h.Publisher,
                h.RegionID,
                h.RegionName,
                h.ReleaseTime,
                AddDate = h.ReleaseTime.IsNoNull() ? ((DateTime)h.ReleaseTime).ToString("MM-dd HH:mm") : "",
                h.Room,
                h.Source,
                h.Tel,
                h.Title,
                h.Toilet,
                h.TradeType,
                h.Url
            });
            return Json(new { data = resultList, totalSize = totalSize }, JsonRequestBehavior.AllowGet);
        }

        #region 房源收藏删除
        [HttpPost]
        public JsonResult DeleteHouseCollect(List<int> cId)
        {
            PublicUserModel loginUser = this.GetLoginUser();
            string cIds = string.Join(",", cId);
            int rows = ncBase.CurrentEntities.P_House_CollectionDelete(cIds, loginUser.UserID);
            return Json(new { code = rows });
        }
        #endregion

        #endregion

        #region 房源提醒设置

        public ActionResult HouseRemind()
        {
            PublicUserModel thisUser = this.GetLoginUser();
            List<HouseRemindSet> houseReminds =
                    ncBase.CurrentEntities.HouseRemindSet.Where(o => o.UserId == thisUser.UserID).ToList();

            return View(houseReminds);
        }
        [HttpPost]
        [ValidateInput(false)]
        public JsonResult SaveKeyword(string Keywords, int tradeType)
        {
            Keywords = Keywords.Trim().Replace(" ", "");
            HouseBll houseBll = new HouseBll();
            PublicUserModel thisUser = this.GetLoginUser();
            bool flag = false;
            HouseRemindSet houseRemind = ncBase.CurrentEntities.HouseRemindSet.Where(o => o.UserId == thisUser.UserID && o.TradeType == tradeType).FirstOrDefault();
            HouseRemindSet houseRemindOld = houseRemind;
            if (houseRemindOld.IsNoNull())
            {
                var KeywordListsOld = houseRemindOld.AttentionCommunity.Split(',');

                foreach (var s in KeywordListsOld)
                {
                    if (!string.IsNullOrEmpty(s))
                        houseBll.RemoveKeyword(s, tradeType, thisUser.CityID, thisUser.UserID);
                }
            }
            if (!string.IsNullOrEmpty(Keywords))
            {
                var KeywordLists = Keywords.Split(',');
                if (KeywordLists.Length > 0)
                {
                    Keywords = "";
                    var KeywordArr = KeywordLists.GroupBy(p => p).Select(p => p.Key).ToArray();
                    foreach (var s in KeywordArr)
                    {
                        if (!string.IsNullOrEmpty(s))
                        {
                            Keywords += s + ",";  //去重
                            houseBll.AddKeyword(s, tradeType, thisUser.CityID, thisUser.UserID);
                        }
                    }
                }
            }


            if (houseRemind.IsNull())
            {
                houseRemind = new HouseRemindSet();
                flag = true;
            }
            houseRemind.CityId = thisUser.CityID;
            houseRemind.AddTime = DateTime.Now;
            houseRemind.TradeType = tradeType;
            houseRemind.UserId = thisUser.UserID;
            houseRemind.AttentionCommunity = Keywords;
            if (flag)
            {
                ncBase.CurrentEntities.AddToHouseRemindSet(houseRemind);
            }
            ncBase.CurrentEntities.SaveChanges();
          
            return Json(1, JsonRequestBehavior.AllowGet);

        }

        #endregion

        private Dictionary<int, string> GetCommunityList()
        {
            Dictionary<int, string> cList = HttpContext.Cache["communityList"] as Dictionary<int, string>;
            if (cList == null)
            {
                cList = new Dictionary<int, string>();
                HouseBll houseBll = new HouseBll();
                List<Community> communityList = houseBll.GetAllHouseCommunityList();
                if (communityList != null && communityList.Count > 0)
                {
                    foreach (Community cItem in communityList)
                    {
                        if (!cList.ContainsKey(cItem.CommunityID))
                        {
                            cList.Add(cItem.CommunityID, cItem.Name);
                        }
                    }
                }
                HttpContext.Cache.Insert("communityList", cList, null, DateTime.Now.AddMinutes(20), TimeSpan.Zero);
            }
            return cList;
        }
        public JsonResult test(string imgUrl)
        {

            string fileExt = string.Empty;
            string newUrl = imgUrl;

            string de = CryptoUtility.TripleDESDecrypt(imgUrl);
            List<HouseImage> ilist = new List<HouseImage>();
            ilist.Add(new HouseImage
            {
                IsCover = false,
                HouseImageID = 1
            });
            ilist.Add(new HouseImage
            {
                IsCover = false,
                HouseImageID = 2
            });
            if (ilist.IsNoNull() && ilist.Count > 0)
            {
                var isCoverImg = ilist.Where(o => o.IsCover == true).FirstOrDefault();
                if (isCoverImg.IsNoNull())
                {

                    isCoverImg.IsCover = true;
                }
                else
                {
                    var firstCoverImg = ilist.FirstOrDefault();
                    firstCoverImg.IsCover = true;
                }

            }
            //de= HtmlAgility
            return Json(de, JsonRequestBehavior.AllowGet);

            //return Json(cookie + "|||" + proxy.Address.ToString(), JsonRequestBehavior.AllowGet);
        }

    }
}
