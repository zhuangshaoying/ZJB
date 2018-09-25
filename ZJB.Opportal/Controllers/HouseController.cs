using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using System.Web.Services.Description;
using Antlr.Runtime;
using Microsoft.Ajax.Utilities;
using ZJB.Api.BLL;
using ZJB.Api.Common;
using ZJB.Api.Entity;
using ZJB.Api.Models;
using ZJB.Api.Models.Community;
using ZJB.Core.Injection;
using ZJB.Core.Utilities;
using ZJB.WX.Models;
using ZJB.Opportal.Common;
using ZJB.Web.Utilities;
using HouseModels = ZJB.Opportal.Models.HouseModels;
using ZJB.Opportal.Models;

namespace ZJB.Opportal.Controllers
{

    [Authorization]
    public class HouseController : BaseController
    {
        private readonly HouseBll houseBll = Container.Instance.Resolve<HouseBll>();
        private readonly RegionsBll regionBll = Container.Instance.Resolve<RegionsBll>();
        
        private NCBaseRule ncBase = new NCBaseRule();

        #region 默认页跳转到房源列表

        public ActionResult Index(int posttype = 1, int buildingType = 1, int buildingId = 0)
        {
            return RedirectToAction("GetHouseMainView");
        }

        #endregion

        #region 房源编辑页面
        /// <summary>
        /// 房源编辑页面
        /// </summary>
        /// <param name="posttype">信息类型 1 出售 2 求购 3 出租 4求租</param>
        /// <returns></returns>
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

        #endregion

        #region 出售 出租 录入 表单提交
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult OperateHouse(FormCollection form)
        {
            PublicUserModel loginUser = this.GetLoginUser();

            #region 基本信息
            int houseId = 0, buildingType, city, uid = loginUser.UserID;
            int.TryParse(form["houseId"], out houseId);  //房源ID
            int.TryParse(form["buildingType"], out buildingType);  //房屋类型
            int tradeType;
            int.TryParse(form["postType"], out tradeType); //发布类型
            int isChange = 0;
            int.TryParse(form["isChange"], out isChange);  //表单是否被修改

            int.TryParse(form["city"], out city);  //城市ID
            int communityId;
            int.TryParse(form["cellCode"], out communityId); // 小区ID
            string communityName = form["cell"];  //小区名

            int distrct;
            int.TryParse(form["distrct"], out distrct);  //行政区

            int region;
            int.TryParse(form["area"], out region);    //路段

            string address = form["addr"];  //地址

            double buildArea, usedArea;
            double.TryParse(form["houseArea"], out buildArea);    //建筑面积
            double.TryParse(form["areaUsed"], out usedArea);    //使用面积
            if (usedArea > buildArea)
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
            string houseDescribe = form["houseDescribe"]; //房源描述

            string imgUrlCover =""; //房源图封面

            string imgUrl = form["imgUrl"].IsNoNull() ? form["imgUrl"] : ""; //房源图 
            string imageType = form["imageType"].IsNoNull() ? form["imageType"] : ""; //房源图类型
            string imgDescribe =","+ (form["imgDescribe"].IsNoNull()? form["imgDescribe"] : ""); //房源图说明

            string[] imgUrlLists = imgUrl.Split(',');
            string[] imageTypeLists = imageType.Split(',');
            string[] imgDescribeLists = imgDescribe.Split(',');

            if (imgUrlLists.Length > 0 && string.IsNullOrEmpty(imgUrlCover))
            {
                imgUrlCover = imgUrlLists[0];
            }
            //   return JsonReturnValue(new { Message = imgUrl + " | " + imageType + " | " + imgDescribe + " | " + imgUrlCover + " | " + imageTypeCover + " | " + imgDescribeCover, id = houseId, }, JsonRequestBehavior.AllowGet);


            HouseBasicInfo houseBasicInfo = new HouseBasicInfo();
            if (houseId<1)
            {
                return Content("管理后台不能发布房源");
            }
              houseBasicInfo = ncBase.CurrentEntities.HouseBasicInfo.Where(o => o.HouseID == houseId).FirstOrDefault();
                if (houseBasicInfo.IsNull())
                {
                    return Content("该房源不存在");
                }
           
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
            houseBasicInfo.Room = BitConverter.GetBytes(room)[0];
            houseBasicInfo.Hall = BitConverter.GetBytes(hall)[0];
            houseBasicInfo.Kitchen = BitConverter.GetBytes(kitchen)[0];
            houseBasicInfo.Toilet = BitConverter.GetBytes(toilet)[0];
            houseBasicInfo.Balcony = BitConverter.GetBytes(balcony)[0];
            houseBasicInfo.PayType = payType;
            houseBasicInfo.HouseImgPath = imgUrlCover;
            houseBasicInfo.LowPay = Convert.ToDecimal(lowpay);
            houseBasicInfo.Tel = tel;
            houseBasicInfo.Contacts = contacts;
            houseBasicInfo.Video = video;

            if (houseId.Equals(0)) //添加操作时候
            {
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
                for (int i = 0; i < imgUrlLists.Length ; i++)
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
                        if (imgUrlCover==imgUrlLists[i])
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

            return RedirectToAction("GetHouseMainView", new { posttype = tradeType, buildType = buildingType });
            // return JsonReturnValue(new { Message = "房源提交成功", id = houseId, }, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region 房源列表页面
        /// <summary>
        /// 
        /// </summary>
        /// <param name="posttype">信息类型 1 出售 2 求购 3 出租 4求租</param>
        /// <returns></returns>
        public ActionResult GetHouseMainView(int posttype = 1, int buildType=1,int userId=0)
        {
            if (posttype == 0) posttype = 1;
            ViewBag.posttype = posttype;
            if (buildType == 0) buildType = 1;
            ViewBag.buildType = buildType;
            ViewBag.userId = userId;
            HouseBll houseBll = new HouseBll();
            List<HouseNumSumModel> sumList = houseBll.GetHouseNumSumData(userId, posttype);
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
            parames.userId=parames.userId.IsNull() ? 0 : parames.userId;
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
                    webCount = GetWebCount(houseItem.houseTable.PostSites, houseItem.houseTable.OrderSites),
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
        /// <returns></returns>
        [HttpPost]
        public JsonResult GetEnableWebSite(int buildType=0)
        {
            PublicUserModel loginUser = this.GetLoginUser();//当前用户

            SiteManageBll siteManageBll = new SiteManageBll();
            List<SiteManageModel> siteManageList = siteManageBll.GetSiteList(buildType.ToString(), loginUser.CityID);//所有站点


         
            List<SiteManageViewModel> viewList = (from  site in siteManageList
                                                 
                                                  select new SiteManageViewModel()
                                                  {
                                                      SiteID = site.SiteID,
                                                      Logo = site.Logo,
                                                      SiteName = site.SiteName,
                                                     
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

            string webSites = string.Empty;
            string times = string.Empty;
            if (Request.Form["webCheck"] != null && Request.Form["webCheck"].ToString() != "")
            {
                webSites = Request.Form["webCheck"].ToString();
            }
            if (Request.Form["time"] != null && Request.Form["time"].ToString() != "")
            {
                times = Request.Form["time"].ToString();
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
            int rowCount = postBll.BatchPostManageAdd(this.GetLoginUser().UserID, releaseType, postList);
            return Json(rowCount);
        }

        #endregion

        #region 获取小区信息
        [HttpPost]
        [ValidateInput(false)]

        public JsonResult GetCellsByInput(string inputStr, int buildingType)
        {
            List<Community> conCommunities =
                ncBase.CurrentEntities.Community.Where(o => o.Name.Contains(inputStr)).Take(10).ToList();
            if (conCommunities.IsNoNull())
            {
                dynamic cellls = conCommunities.Select(com =>
               new
               {
                   address = com.Address,
                   area = com.RegionID,
                   avgPrice = com.SellPrice,
                   cellCode = com.CommunityID,
                   cellName = com.Name,
                   completionDate = 0,
                   district = com.Distrctid
               });
                return Json(new CellResponse(cellls), JsonRequestBehavior.AllowGet);
            }

            return Json(new CellResponse(null), JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region 云刷新页面
     
        public ActionResult Refresh(int tabIndex = 0)
        {
            return View();
        }
        #endregion

        #region 云采集页面
        
        public ActionResult HouseCollect(int city = 592,int postType=1)
        {
            return View();
        }
        #endregion

        #region 房源共享

        public ActionResult ShareBuilding()
        {
            return View();
        }
        #endregion

        #region 房源搬家

        public ActionResult MoveHouse()
        {
            return View();
        }
        #endregion

        #region 小区匹配

        public ActionResult CommunityManage(int pageIndex = 1, int pageSize = 10, string name = "",int status=1)
        {
            int totalCount = 0;
            List<Community> list = houseBll.GetCircleListForManage(name, pageIndex, pageSize, status, out totalCount);
            ViewBag.PageIndex = pageIndex;
            ViewBag.PageSize = pageSize;
            ViewBag.PageTotal = (int)Math.Ceiling((double)totalCount / pageSize);
            return View(list);
        }

        public ActionResult CommunityDetail(int id=0)
        {
            Community community = houseBll.GetCommunityById(id);
            CommunityDetailModel result = new CommunityDetailModel();
            result.Community = community;
            List<RegionsModel> regionsList = regionBll.GetRegionList();
            result.CityList = regionsList.FindAll(x => x.Layer == 1);

            int? cityId = 0;
            if (result.Community != null && result.Community.CityID > 0)
            {
                cityId = result.Community.CityID;
            }
            else
            {
                cityId = result.CityList[0].RegionID;
            }

            result.DistrctList = regionsList.FindAll(x => (x.CityID == cityId && x.Layer == 2));
            int ?distictId=0;
           
            if (result.Community != null && result.Community.Distrctid > 0)
            {
                distictId = result.Community.Distrctid;
            }
            else
            {
                distictId = result.DistrctList[0].RegionID;
            }
            result.RegionList = regionsList.FindAll(x => (x.DistrctID == distictId && x.Layer == 3));
            return View(result);
        }

        public ActionResult CommunityMapping(int pageIndex = 1, int pageSize = 10, int siteId = 3, int cityId = 0, string name="")
        {
            int totalCount = 0;
            List<TargetLoupan> list = houseBll.GetTargetLoupanList(pageIndex, pageSize, siteId, cityId,name, out totalCount);

            List<RegionsModel> regionsList = regionBll.GetRegionList();
            
            ViewBag.CityList = regionsList.FindAll(x => (x.Layer == 1));
            ViewBag.PageIndex = pageIndex;
            ViewBag.PageSize = pageSize;
            ViewBag.PageTotal = (int)Math.Ceiling((double)totalCount / pageSize);
            return View(list);
        }

        public JsonResult GetMappingCircleByName(string name = "", int siteId = 0)
        {
            var list = houseBll.GetSimilarCommunityList(name, siteId);
            var obj = new Dictionary<string, object>
                {
                    {"list", list}
                };
            return JsonReturnValue(obj, JsonRequestBehavior.AllowGet);
        }

        public JsonResult MappingCircle(int siteId = 0, int id = 0, string communityId = "")
        {
            houseBll.MappingCircle(Convert.ToInt32(siteId), id, communityId);
            var success = new { Status = "成功" };
            return JsonReturnValue(success, JsonRequestBehavior.AllowGet);
        }

        public JsonResult FinishMapping(string id = "")
        {
            houseBll.FinishMapping(id);
            var success = new { Status = "成功" };
            return JsonReturnValue(success, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetDistrictByCityId(int cityId = 0)
        {
            List<RegionsModel> regionsList = regionBll.GetRegionList();
            regionsList = regionsList.FindAll(x => (x.CityID == cityId && x.Layer == 2));
            return Json(regionsList, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetRegionByDistictId(int distictId=0)
        {
            List<RegionsModel> regionsList = regionBll.GetRegionList();
            regionsList = regionsList.FindAll(x => (x.DistrctID == distictId && x.Layer == 3));
            return Json(regionsList, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [ValidateInput(false)]
        public JsonResult UpdateCommunityDetail(FormCollection form)
        {
            Community circle = new Community();
           
            circle.CommunityID = int.Parse(form["communityId"]);
            circle.Name = form["communityName"];
            circle.Lng = form["lng"];
            circle.Lat = form["lat"];
            circle.RegionID = int.Parse(form["region"]);
            circle.Address = form["addr"];
            circle.KaiFaShang = form["kaifashang"];
            circle.PeiTao = form["peitao"];
            circle.WuyeCompany = form["wuyeCompany"];
            circle.BuildType = (byte?) Convert.ToInt32(form["buildType"]);
            circle.Traffic = form["Traffic"];
            circle.CoverImg = form["hdImageUrl"];
            circle.CompleteDate = form["completedate"];
            circle.SellPrice = (decimal?)Convert.ToDecimal(form["sellprice"]);
           
            if (string.IsNullOrEmpty(form["status"]))
            {
                circle.Status = 0;
            }
            else
            {
                circle.Status = (byte?)Convert.ToInt32(form["status"]);
            }
            if (string.IsNullOrEmpty(form["recommend"]))
            {
                circle.Recommend = 0;
            }
            else
            {
                circle.Recommend = Convert.ToInt32(form["recommend"]);
            }
            if (string.IsNullOrEmpty(circle.Name))
            {
                var fail = new { ErrMsg = "小区不能为空" };
                return JsonReturnValue(fail, JsonRequestBehavior.AllowGet);
            }

            int id=houseBll.UpdateCommunityDetail(circle);
            var success = new {Id=id, Status = "成功" };
            return JsonReturnValue(success, JsonRequestBehavior.AllowGet);

        }

        #endregion

        #region 直约业主
        public ActionResult HouseInterview(int pageIndex = 1, int pageSize = 20, string keyWord = "")
        {
            string sqlwhere = "1==1";

            if (!string.IsNullOrEmpty(keyWord))
            {
                int i = 0;
                int.TryParse(keyWord, out i);
                if (i > 0)
                {
                    sqlwhere += " and it.UserID==" + keyWord;
                }
                else
                {
                    sqlwhere += " and (it.Name like '%" + keyWord + "%' or it.tel like '%" + keyWord + "%' )";
                }
            }

            List<VHouseInterview> houseInterviews =
                ncBase.CurrentEntities.VHouseInterview.Where(sqlwhere)
                    .OrderByDescending(o => o.AddTime)
                    .Skip(pageSize * (pageIndex - 1))
                    .Take(pageSize)
                    .ToList();
            ViewBag.Count = ncBase.CurrentEntities.VHouseInterview.Where(sqlwhere).Count();

            ViewBag.PageIndex = pageIndex;
            ViewBag.PageSize = pageSize;
            ViewBag.KeyWord = keyWord;
            ViewBag.PageTotal = (int)Math.Ceiling((double)ViewBag.Count / pageSize);
            return View(houseInterviews);
        }
        #endregion
    }
}
