using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Mvc.Html;
using Antlr.Runtime;
using ZJB.Api.BLL;
using ZJB.Api.Common;
using ZJB.Api.Entity;
using ZJB.Api.Models;
using ZJB.Api.Models.Parame;
using ZJB.Core.Utilities;
using ZJB.WX.Filters;
using MongoDB.Driver;
using ZJB.WX.Models;
using ZJB.WX.Models.Client;

namespace ZJB.WX.Controllers.Api
{
    public class HouseController : BaseController
    {

        private NCBaseRule ncBase = new NCBaseRule();
        private HDictionary hd = HDictionary.Instance;

        #region 房源采集
       
        [Token]
        public ApiResponse GetHouseCollectList(int cityId = 592, int districtId = 0, int room = 0, int regionId = 0, int postType = 1, string webName = "", string title = "", int buildType = 1, int pageIndex = 1, int pageSize = 10, double minPrice = 0, double maxPrice=0)
        {
           
            int userId = GetCurrentUserId();
            HouseBll houseBll = new HouseBll();
            HouseListReq parames = new HouseListReq()
          {
              districtId = districtId,
              postType = postType,
              buildingType = buildType,
              webName = webName,
              regionId = regionId,
              cityId = cityId,
              roomType = room,
              page = pageIndex,
              pageSize = pageSize,
              title = title,
              minPrice = minPrice,
              maxPrice = maxPrice
          };
          
            int totalSize = 0;
            var houseList  = houseBll.GetHouseCollectList(parames, ref totalSize);//房源列表
            List<string> ids = new List<string>();
            if (houseList != null && houseList.Any())
            {
                ids = houseList.Select(v => v.Id).ToList();
            }
            var jjrTels = houseBll.GetJjrTels();
            var viewList = houseBll.GetHouseCollectReadLogByIds(ids).ToArray();
            var resultList = houseList.Select(h => new
            {
                h.Publisher,
                h.Tel,
                h.Title,
                h.Address,
                h.BuildArea,
                BuildType= h.BuildingType,
                BuildTypeName = hd.BuildingType(Convert.ToString(h.BuildingType)),
                h.CommunityName,
                CurFloor = h.CurFloor ?? 0,
                MaxFloor = h.MaxFloor ?? 0,
                h.CityID,
                h.Distrctid,
                h.DistrctName,
                h.RegionID,
                h.RegionName,
               CollectId=h.Id,
                PicNum = h.PicNum ?? 0,
                h.Price,
                h.PriceUnit,
                ReleaseTime=DateTimeUtility.ToUnixTime(Convert.ToDateTime(h.ReleaseTime)),
                UpdateTime = DateTimeUtility.ToUnixTime(Convert.ToDateTime(h.UpdateTime)),
                AddDate = DateTimeUtility.ToUnixTime(h.UpdateTime.IsNoNull() ? ((DateTime)h.UpdateTime) : DateTime.Now),
                h.Room,
                h.Hall,
                h.Toilet,
                h.Kitchen,
                h.Balcony,
                h.Source,
                h.TradeType,
                TradeTypeName = hd.TradeType(Convert.ToString(h.TradeType)),
                h.Url,
                HouseType = h.UpdateTime > h.ReleaseTime ? 1 : 0,// 0:新房源 ，1：刷新
                ViewCount = viewList == null ? 0 : ViewCount(viewList, h.Id),
                IsRead = viewList == null ? 0 : IsRead(viewList, h.Id, userId),
                IsJjr = jjrTels.Contains(h.Tel)
            });
            return new ApiResponse(Metas.SUCCESS, resultList, totalSize);
        }
        private int ViewCount(HouseCollectViewLog[] viewList, string id)
        {
            int count = 0;
            HouseCollectViewLog item = viewList.Where(v => v.id == id).FirstOrDefault();
            if (item != null)
            {
                count = item.Agents.Count;
            }
            return count;
        }
        private int IsRead(HouseCollectViewLog[] viewList, string id, int userId)
        {
            int isRead = 0;
            HouseCollectViewLog item = viewList.Where(v => v.id == id).FirstOrDefault();
            if (item != null)
            {
                isRead = item.Agents.Where(a => a.Id == userId).FirstOrDefault() == null ? 0 : 1;
            }
            return isRead;
        }
        #endregion
        
        #region 获取采集房源详细
       
        [Token]
        public ApiResponse GetCollectBrowser(string collectId)
        {
           
            int userId = GetCurrentUserId();
            HouseCollection newCollect = ncBase.CurrentEntities.HouseCollection.Where(c => c.CollectId == collectId && c.UserId == userId&&c.CollectStatus==1).FirstOrDefault();

            bool isCollection = newCollect.IsNoNull();
            var result = new
            {
                IsCollection = isCollection
            };
            return new ApiResponse(Metas.SUCCESS, result);
        }
    
        #endregion

        #region 我的收藏房源
        [Token]
        public ApiResponse GetHouseCollection(int cityId = 592, int districtId = 0, int room = 0, int regionId = 0, int postType = 1, string webName = "", string title = "", int buildType = 1, int pageIndex = 1, int pageSize = 10)
        {
           
            int userId = GetCurrentUserId();
            HouseBll houseBll = new HouseBll();
            HouseListReq parames = new HouseListReq()
          {
              userId=userId,
              districtId = districtId,
              postType = postType,
              buildingType = buildType,
              webName = webName,
              regionId = regionId,
              cityId = cityId,
              roomType = room,
              page = pageIndex,
              pageSize = pageSize,
              title = title,
              

          };
            
            int totalSize = 0;
            List<HouseCollection> houseList = houseBll.GetCollectionList(parames, ref totalSize);//房源列表

            var resultList = houseList.Select(h => new
            {
                h.Id,
                h.Publisher,
                h.Tel,
                h.Title,
                h.Address,
                h.BuildArea,
                BuildType = h.BuildingType,
                BuildTypeName = hd.BuildingType(Convert.ToString(h.BuildingType)),
                h.CommunityName,
                CurFloor = h.CurFloor ?? 0,
                MaxFloor = h.MaxFloor ?? 0,
                h.Distrctid,
                h.DistrctName,
                h.RegionID,
                h.RegionName,
                PicNum = h.PicNum ?? 0,
                h.Price,
                h.PriceUnit,
                ReleaseTime = DateTimeUtility.ToUnixTime(Convert.ToDateTime(h.ReleaseTime)),
                h.Room,
                h.Hall,
                h.Toilet,
                h.Kitchen,
                h.Balcony,
                h.Source,
                h.TradeType,
                TradeTypeName = hd.TradeType(Convert.ToString(h.TradeType)),
                h.Url,
                h.CollectId
            
            });
            return new ApiResponse(Metas.SUCCESS, resultList, totalSize);
        }
       
        #endregion

        #region 获取小区信息
        [System.Web.Http.HttpGet]
       
        public ApiResponse GetCellsByInput(string keyWord, int buildType)
        {
            List<Community> conCommunities =
                ncBase.CurrentEntities.Community.Where(o => o.Name.Contains(keyWord)).Take(10).ToList();
            if (conCommunities.IsNoNull())
            {
                var cellls = conCommunities.Select(com =>
               new
               {
                  com.Address,
                  com.RegionID,
                  com.SellPrice,
                  com.CommunityID,
                  CellCode = com.CommunityID,
                   CellName = com.Name,
                   CompletionDate = 0,
                   com.Distrctid
               });
                return new ApiResponse(Metas.SUCCESS, cellls);
            }

            return new ApiResponse(Metas.SUCCESS);
        }
        #endregion

        #region 获取采集的站点
        [System.Web.Http.HttpGet]
        [Token]
        public ApiResponse GetCollectWebList(int cityId=592)
        {
            HouseBll houseBll = new HouseBll();
            MongoCursor<HouseCollectSource> houseList = houseBll.GetHouseCollectSite(cityId);//
          
            List<string> lists = new List<string>();

            if (houseList.IsNoNull())
            {
                foreach (var list in houseList)
              {
                  lists.Add(list.Source);
              }
            }
            return new ApiResponse(Metas.SUCCESS, lists);

        }

        #endregion

        #region 我的房源
        [System.Web.Http.HttpGet]
        [Token]
        public ApiResponse GetMyHouseList(int cityId = 592,string cell="",int sort=0, int districtId = 0, int regionId = 0, int postType = 1,int buildingStatus=1, string title = "",string tags="", int buildType = 1, int pageIndex = 1, int pageSize = 10)
        {
            string url = "http://" + HttpContext.Current.Request.Url.Host;
            int userId = GetCurrentUserId();
            HouseListReq parames = new HouseListReq()
            {
                districtId = districtId, //行政区域ID
                postType = postType, //信息类型
                buildingType = buildType,  //房屋类型
                title = title,  //关键字搜索
                regionId = regionId, //路段ID
                cell=cell, //小区搜索
                sort = sort, //排序ID [更新时间排序,推送时间排序,价格从大到小,价格从小到大,面积从大到小,面积从小到大,标签排序]
                tags=tags, //标签
                cityId = cityId,// 城市ID
                page = pageIndex,  
                pageSize = pageSize,
                buildingStatus = buildingStatus  //信息状态
            };
            HouseBll houseBll = new HouseBll();
            RegionsBll regionBll = new RegionsBll();
            parames.userId = userId;
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
                    AddDate = DateTimeUtility.ToUnixTime(Convert.ToDateTime(houseItem.houseTable.AddDate)),
                    Address = houseItem.houseTable.Address,
                    Title = houseItem.houseTable.Title,
                    BuildArea = houseItem.houseTable.BuildArea,
                    BuildType = houseItem.houseTable.BuildType,
                    BuildTypeName = hd.BuildingType(Convert.ToString(houseItem.houseTable.BuildType)),
                    CellLabel = houseItem.houseTable.CellLabel,
                    CityID = houseItem.houseTable.CityID,
                    CommunityID = houseItem.houseTable.CommunityID,
                    CommunityName = houseItem.houseTable.CommunityName,
                    CurFloor = houseItem.houseTable.CurFloor,
                    Distrctid = houseItem.houseTable.Distrctid,
                    DistrctName = houseItem.regionTable.FirstOrDefault() != null ? houseItem.regionTable.FirstOrDefault().Name : "",
                    DeleteTime =DateTimeUtility.ToUnixTime(Convert.ToDateTime( houseItem.houseTable.DeleteTime)),
                    ExpireDay = DateTimeUtility.ToUnixTime(Convert.ToDateTime( houseItem.houseTable.ExpireDay)),
                    FitmentStatus = houseItem.houseTable.FitmentStatus,
                    Room = houseItem.houseTable.Room,
                    Hall = houseItem.houseTable.Hall,
                    Kitchen = houseItem.houseTable.Kitchen,
                    Toilet = houseItem.houseTable.Toilet,
                    Balcony = houseItem.houseTable.Balcony,
                    HouseID = houseItem.houseTable.HouseID,
                    HouseImgPath = houseItem.houseTable.HouseImgPath,
                    HouseLabel = houseItem.houseTable.HouseLabel,
                    InternalNum = houseItem.houseTable.InternalNum,
                    IP = houseItem.houseTable.IP,
                    LookHouseTime = houseItem.houseTable.LookHouseTime,
                    MaxFloor = houseItem.houseTable.MaxFloor,
                    Note = StringUtility.DelHtml(houseItem.houseTable.Note),
                    PayType = houseItem.houseTable.PayType,
                    PicNum = houseItem.houseTable.PicNum,
                    PointTo = houseItem.houseTable.PointTo,
                    PostTime =DateTimeUtility.ToUnixTime(Convert.ToDateTime( houseItem.houseTable.PostTime)),
                    Price = houseItem.houseTable.Price,
                    PriceUnit = houseItem.houseTable.PriceUnit,
                    PushTime = DateTimeUtility.ToUnixTime(Convert.ToDateTime(houseItem.houseTable.PushTime)),
                    RegionID = houseItem.houseTable.RegionID,
                     Tag = houseItem.houseTable.Tag,
                    TradeType = houseItem.houseTable.TradeType,
                    TradeTypeName = hd.TradeType(Convert.ToString(houseItem.houseTable.TradeType)),
                    UnitPrice = houseItem.houseTable.UnitPrice,
                    UsedArea = houseItem.houseTable.UsedArea,
                    UsedYear = houseItem.houseTable.UsedYear,
                    UserID = houseItem.houseTable.UserID,
                    YiJuHua = houseItem.houseTable.YiJuHua,
                    WebCount = houseItem.houseTable.WebCount,
                    OrderStatus = houseItem.houseTable.OrderStatus,
                    ShareInfo = new
                    {
                        ShareUrl = url+"/m/v/" + houseItem.houseTable.HouseID,
                        ShareImgUrl = string.IsNullOrEmpty(houseItem.houseTable.HouseImgPath)?url+"/Images/logo/logo.jpg":houseItem.houseTable.HouseImgPath,
                        ShareContent = houseItem.houseTable.Address,
                        ShareTitle = houseItem.houseTable.Title,
                    }
                }).ToList();


            return new ApiResponse(Metas.SUCCESS, resultList, totalSize);
        }
      
        #endregion

        #region 获取我发房源的小区
        [System.Web.Http.HttpGet]
        [Token]
        public ApiResponse GetMyHouseCommunityList(int postType = 1, int buildType = 1, int buildStatus = 1)
        {

            int userId = GetCurrentUserId();
            HouseBll houseBll = new HouseBll();
           var communityList = houseBll.GetUserHouseCommunityList(new GetUserHouseCommunityListReq()
            {
                BudlingType = buildType,
                BudlingStatus = buildStatus,
                PostType = postType,
                UserId = userId
            });
          var  resultList = communityList.IsNoNull()
                ? communityList.Select( p=>
               new {
                    p.CommunityID,
                   CommunityName= p.Name
                })
                : null;

            return new ApiResponse(Metas.SUCCESS, resultList);
           
        }

        #endregion

        #region 修改房源标签
        [System.Web.Http.HttpPost]
        [Token]
        public ApiResponse BatchSetHouseTags([FromBody]HouseReq model)
        {

            int userId = GetCurrentUserId();
            HouseBll houseBll = new HouseBll();
            houseBll.UpdateHouseTags(userId, model.HouseIds, model.Tags);
            return new ApiResponse(Metas.SUCCESS);

        }

        #endregion

        #region 修改房源状态
        [System.Web.Http.HttpPost]
        [Token]
        public ApiResponse ChangeHousesStatus([FromBody]HouseReq model)
        {

            int userId = GetCurrentUserId();
            if (string.IsNullOrEmpty(model.HouseIds))
            {
                return new ApiResponse(Metas.HOUSEID_NULL);
            }
          
            HouseBll houseBll = new HouseBll();
            ChangeHouseStatusReq parames = new ChangeHouseStatusReq
            {
                HouseIdsStr = model.HouseIds,
                ChangeToStatus = Convert.ToByte(model.BuildingStatus),
                UserId = userId
            };
                int count = houseBll.ChangeHouseStatus(parames);

                return new ApiResponse(Metas.SUCCESS,null, count);
        }

        #endregion

        #region 获取可发布站点
        [System.Web.Http.HttpPost]
        [Token]
        public ApiResponse GetEnableWebSite([FromBody]HouseReq model)
        {
            
      
            var credential = Request.GetCredential();
            int cityID = 0;
            int userId = 0;
            if (credential != null)
            {
                cityID = credential.CityId;
                userId = credential.UserID;
            }
            if (model.BuildType.IsNoNull() || model.BuildType < 1)
                model.BuildType = 1;

            SiteManageBll siteManageBll = new SiteManageBll();
            List<SiteManageModel> siteManageList = siteManageBll.GetSiteList(model.BuildType.ToString(), cityID);//所有站点

            UserSiteManageBll userSiteManageBll = new UserSiteManageBll();
            List<UserSiteManageModel> userSiteManageList = userSiteManageBll.GetUserSiteListByUserId(userId);//用户关联的站点

            List<SiteManageViewModel> viewList = (from site in siteManageList
                                                  join userSite in userSiteManageList on site.SiteID equals userSite.SiteID into viewListTemp
                                                  from viewItem in viewListTemp.DefaultIfEmpty()
                                                  where viewItem != null && viewItem.SiteStatus == 1
                                                  select new SiteManageViewModel()
                                                  {
                                                      SiteID = site.SiteID,
                                                      Logo = "http://" + HttpContext.Current.Request.Url.Host + "/" + site.Logo,
                                                      SiteName = site.SiteName,
                                                      SiteUserName = viewItem != null ? viewItem.SiteUserName : ""
                                                  }).ToList();
            var list = viewList.IsNoNull()
                ? viewList.Select(site => new
                {
                   site.SiteID,
                   site.Logo,
                   site.SiteName,
                   site.SiteUserName 
                })
                : null;


            return new ApiResponse(Metas.SUCCESS, list);

        }

        #endregion

        #region 获取发布房源日志
        [System.Web.Http.HttpGet]
        [Token]
        public ApiResponse GetPostLogList(string cell = "",long dateTime=0, int  houseId=0,int status=0, string title = "", int siteId=0, int pageIndex = 1, int pageSize = 10)
        {

            int userId = GetCurrentUserId();
            PostLogBll postLogBll = new PostLogBll();
            PostLogListParames parames=new PostLogListParames();
            int totalSize = 0;
            parames.houseId = houseId;
            parames.pageIndex = pageIndex;
            parames.pageSize = pageSize;
            parames.communityName = cell;
            parames.siteId = siteId;
            parames.title = title;
            parames.userId = userId;
            parames.status = status;
            if (dateTime > 0)
            {
                parames.time = DateTimeUtility.FromUnixTime(dateTime).Date;
            }
            List<PostLogModel> postLogList = postLogBll.GetPostLogList(parames, ref totalSize);
            if (postLogList.IsNoNull() && postLogList.Count > 0)
            {
                var resultList = postLogList.Select(p => new
                {
                   
                    ID = p.ID,
                    InfoID = p.InfoID,
                    SiteID = p.SiteID,
                    Status = p.Status,
                    TargetID = p.TargetID,
                    UserID = p.UserID,
                    TargetUrl = p.TargetUrl,
                    IsOrder = p.IsOrder,
                    Msg = p.Msg,
                    SiteUserName = p.SiteUserName,
                    TradeType = p.TradeType,
                    BuildType = p.BuildType,
                    BuildTypeName = hd.BuildingType(Convert.ToString(p.BuildType)),
                    CommunityName = p.CommunityName,
                    BuildArea = p.BuildArea,
                    CurFloor = p.CurFloor,
                    MaxFloor = p.MaxFloor,
                    Price = p.Price,
                    PriceUnit = p.PriceUnit,
                    Title = p.Title,
                    SiteName = p.SiteName,
                    DateTime = DateTimeUtility.ToUnixTime(p.DateTime),
                    BeginTime = DateTimeUtility.ToUnixTime(p.BeginTime),
                    PostType = p.PostType,
                   PostTypName=hd.PostTyp(Convert.ToString(p.PostType))
                });

            return new ApiResponse(Metas.SUCCESS, resultList, totalSize);
          }
            return new ApiResponse(Metas.SUCCESS);
          }
        #endregion

        #region 发布/预约发布房源
        [System.Web.Http.HttpPost]
        [Token]
        public ApiResponse ReleaseHouses([FromBody]HouseReq model)
        {
          
            int userId = GetCurrentUserId();
            HouseBll houseBll = new HouseBll();
        
            PostManageBll postBll = new PostManageBll();
            List<PostManageModel> postList = new List<PostManageModel>();
            if (string.IsNullOrEmpty(model.HouseIds))
            {
                return new ApiResponse(Metas.HOUSEID_NULL);
            }
            if (string.IsNullOrEmpty(model.WebSiteIds))
            {
                return new ApiResponse(Metas.WEBSITES_NULL);
            }
            string orderTime = "";
            if (!string.IsNullOrEmpty(model.OrderTime))
            {
                var orderTimeArr = model.OrderTime.Split(',');
                foreach (var s in orderTimeArr)
                {
                    long lTime = 0;
                    long.TryParse(s, out lTime);
                    if (lTime>0)
                    orderTime += ","+DateTimeUtility.FromUnixTime(lTime);
                }
                orderTime = orderTime.StartsWith(",") ? orderTime.Substring(1) : orderTime;
            }
            var houseids = model.HouseIds.Split(',');
            foreach (string houseId in houseids)
            {
                int hid = 0;
                int.TryParse(houseId, out hid);
                if (hid > 0)
                {
                    postList.Add(new PostManageModel()
                    {
                        HouseID = hid,
                        OrderTime = orderTime,
                        PostSites =model.WebSiteIds,
                        OrderSites = model.WebSiteIds,
                    });
                }
            }
            int rowCount = postBll.BatchPostManageAdd(userId, model.ReleaseType, postList);

           


            #region APP登录加分

            int First_RealseHouse = 0;
            int EveryDay_RealseHouse = 0;
            int gainPoints = 0;
            string gainPointsMsg = "";
            DoTask(userId, PointsEnum.First_RealseHouse, out First_RealseHouse);
            DoTask(userId, PointsEnum.EveryDay_RealseHouse, out EveryDay_RealseHouse);
            gainPoints = First_RealseHouse + EveryDay_RealseHouse;
            gainPointsMsg = "完成“发布房源”任务";

            #endregion

            var result = new
            {
                GainPoints = gainPoints,
                GainPointsMsg = gainPointsMsg
            };
            return new ApiResponse(Metas.SUCCESS, result, rowCount);

        }

        #endregion

        #region 房源详细
        [System.Web.Http.HttpGet]
        [Token]
        public ApiResponse HouseView(int houseId)
        {

            int userId = GetCurrentUserId();
            string url = "http://" + HttpContext.Current.Request.Url.Host;
            HouseBll houseBll = new HouseBll();

            HouseBasicInfo houseBasicInfo = new HouseBasicInfo();
            List<HouseImage> houseImages = new List<HouseImage>();
            HouseInfo houseInfo = new HouseInfo();
            VillaInfo villaInfo = new VillaInfo();
            ShopInfo shopInfo = new ShopInfo();
            OfficeInfo officeInfo = new OfficeInfo();
            FactoryInfo factoryInfo = new FactoryInfo();


            houseBasicInfo =
                ncBase.CurrentEntities.HouseBasicInfo.Where(o => o.HouseID == houseId&&o.Status==1).FirstOrDefault();

            if(houseBasicInfo.IsNull())
                return new ApiResponse(Metas.SUCCESS);


            switch (houseBasicInfo.BuildType)
            {
                case (int)BuildingType.House:
                    houseInfo = ncBase.CurrentEntities.HouseInfo.Where(o => o.HouseID == houseId).FirstOrDefault();
                    break;

                case (int)BuildingType.Villa:
                    villaInfo = ncBase.CurrentEntities.VillaInfo.Where(o => o.HouseID == houseId).FirstOrDefault();
                    break;

                case (int)BuildingType.Shop:
                    shopInfo = ncBase.CurrentEntities.ShopInfo.Where(o => o.HouseID == houseId).FirstOrDefault();
                    break;

                case (int)BuildingType.Office:
                    officeInfo = ncBase.CurrentEntities.OfficeInfo.Where(o => o.HouseID == houseId).FirstOrDefault();
                    break;

                case (int)BuildingType.Factory:
                    factoryInfo = ncBase.CurrentEntities.FactoryInfo.Where(o => o.HouseID == houseId).FirstOrDefault();
                    break;
            }


            houseImages = ncBase.CurrentEntities.HouseImage.Where(o => o.HouseID == houseId).ToList();

            var result = new
                {
                    HouseID = houseBasicInfo.HouseID,
                    Address = houseBasicInfo.Address,
                    Title = houseBasicInfo.Title,
                    BuildArea = houseBasicInfo.BuildArea,
                    BuildType = houseBasicInfo.BuildType,
                    BuildTypeName = hd.BuildingType(Convert.ToString(houseBasicInfo.BuildType)),
                    CellLabel = houseBasicInfo.CellLabel,
                    CityID = houseBasicInfo.CityID,
                    CommunityID = houseBasicInfo.CommunityID,
                    CommunityName = houseBasicInfo.CommunityName,
                    CurFloor = houseBasicInfo.CurFloor,
                    Distrctid = houseBasicInfo.Distrctid,
                    AddDate = DateTimeUtility.ToUnixTime(Convert.ToDateTime(houseBasicInfo.AddDate)),
                    DeleteTime = DateTimeUtility.ToUnixTime(Convert.ToDateTime(houseBasicInfo.DeleteTime)),
                    ExpireDay = DateTimeUtility.ToUnixTime(Convert.ToDateTime(houseBasicInfo.ExpireDay)),
                    FitmentStatus = houseBasicInfo.FitmentStatus,
                    Room = houseBasicInfo.Room,
                    Hall = houseBasicInfo.Hall,
                    Kitchen = houseBasicInfo.Kitchen,
                    Toilet = houseBasicInfo.Toilet,
                    Balcony = houseBasicInfo.Balcony,
                    HouseImgPath = houseBasicInfo.HouseImgPath,
                    HouseLabel = houseBasicInfo.HouseLabel,
                    InternalNum = houseBasicInfo.InternalNum,
                    IP = houseBasicInfo.IP,
                    LookHouseTime = houseBasicInfo.LookHouseTime,
                    MaxFloor = houseBasicInfo.MaxFloor,
                    Note = StringUtility.DelHtml(houseBasicInfo.Note),
                    HasHtmlNote=houseBasicInfo.Note,
                    PayType = houseBasicInfo.PayType,
                    PicNum = houseBasicInfo.PicNum,
                    PointTo = houseBasicInfo.PointTo,
                    PostTime = DateTimeUtility.ToUnixTime(Convert.ToDateTime(houseBasicInfo.PostTime)),
                    Price = houseBasicInfo.Price,
                    PriceUnit = houseBasicInfo.PriceUnit,
                    PushTime = DateTimeUtility.ToUnixTime(Convert.ToDateTime(houseBasicInfo.PushTime)),
                    RegionID = houseBasicInfo.RegionID,
                    Tag = houseBasicInfo.Tag,
                    TradeType = houseBasicInfo.TradeType,
                    TradeTypeName = hd.TradeType(Convert.ToString(houseBasicInfo.TradeType)),
                    UnitPrice = houseBasicInfo.UnitPrice,
                    UsedArea = houseBasicInfo.UsedArea,
                    UsedYear = houseBasicInfo.UsedYear,
                    UserID = houseBasicInfo.UserID,
                    YiJuHua = houseBasicInfo.YiJuHua,
                    houseBasicInfo.Source,
                    houseBasicInfo.Hits,
                    HouseInfo = houseInfo.IsNoNull()?new
                    {
                        houseInfo.FiveYears,
                        houseInfo.AdvEquip,
                        houseInfo.BasicEquip,
                        houseInfo.HouseProperty,
                        houseInfo.HouseStructure,
                        houseInfo.HouseSubType,
                        houseInfo.HouseType,
                        houseInfo.LandYear,
                        houseInfo.OnlyHouse
                    }:null,
                    VillaInfo = villaInfo.IsNoNull() ? new
                    {
                       
                        villaInfo.AdvEquip,
                        villaInfo.BasicEquip,
                        villaInfo.Basement,
                        villaInfo.Garage,
                        villaInfo.Garden,
                        villaInfo.ParkLot,
                        villaInfo.VillaType,
                        villaInfo.HallType,
                        villaInfo.LandYear,
                        villaInfo.OnlyHouse,
                        villaInfo.FiveYears
                    }:null,
                    ShopInfo = shopInfo.IsNoNull() ? new
                    {

                        shopInfo.ShopType,
                        shopInfo.BasicEquip,
                        shopInfo.ShopStatus,
                        shopInfo.TargetField,
                        shopInfo.Fee,
                        shopInfo.Divide
                      
                    } : null,
                    OfficeInfo = officeInfo.IsNoNull() ? new
                    {

                        officeInfo.OfficeType,
                        officeInfo.BasicEquip,
                        officeInfo.OfficeLevel,
                        officeInfo.Fee,
                        officeInfo.Divide

                    } : null,
                    FactoryInfo = factoryInfo.IsNoNull() ? new
                    {

                        factoryInfo.FactoryType,
                        factoryInfo.BasicEquip
                    } : null,
                    HouseImages = houseImages.IsNoNull() ?houseImages.Select(p=>new
                    {
                    p.ImagePath,
                    p.ImagePos,
                    p.ImageType,
                    ImageTypeName = HouseUtility.GetHouseImgTypeName(Convert.ToInt32(p.ImageType)),
                    p.IsCover,
                    p.OrderID
                    } ): null,
                    ShareInfo = new
                    {
                        ShareUrl = url+"/m/v/" + houseBasicInfo.HouseID,
                        ShareImgUrl = houseBasicInfo.HouseImgPath,
                        ShareContent = houseBasicInfo.Address,
                        ShareTitle = houseBasicInfo.Title,
                    }


                };


            return new ApiResponse(Metas.SUCCESS, result);
        }

        #endregion

        #region 录入房源
        [System.Web.Http.HttpPost]
        [Token]
        public ApiResponse EditHouse([FromBody]HouseReq item)
        {

            int userId = GetCurrentUserId();
            HouseBll houseBll = new HouseBll();
           int source = 0;
            if (HttpContext.Current.Request.UserAgent.Contains("IPHONE"))
            {
                source = (int)SourceType.Ios; ;  // ios 
            }
            else if (HttpContext.Current.Request.UserAgent.Contains("ANDROID"))
            {
                source = (int)SourceType.Android;  // android 
            }

            #region 验证


            if (item.PostType < 1 || item.PostType > 4)
            {
                return new ApiResponse(Metas.PostType_ERROR);
            }
            if (item.BuildType < 1 || item.BuildType > 5)
            {
                return new ApiResponse(Metas.BuildType_ERROR);
            }
            if (string.IsNullOrEmpty(item.Title) || string.IsNullOrEmpty(item.Address) || string.IsNullOrEmpty(item.CommunityName) || string.IsNullOrEmpty(item.CellLabel) || string.IsNullOrEmpty(item.HouseLabel)
                || item.CityId < 1 || item.DistrictId < 1 || item.Room < 1 || item.Price <=0 
                )
                return new ApiResponse(Metas.HOUSENULL);

            if (item.UsedArea > item.BuildArea)
                return new ApiResponse(Metas.BuildAreaError);

            if (item.CurFloor >item.MaxFloor)
                return new ApiResponse(Metas.CurFloorError);

            string houseDescribeDelHtml = StringUtility.DelHtml(item.Note);
            if (houseDescribeDelHtml.Length < 30 || houseDescribeDelHtml.Length > 3000)
                return new ApiResponse(Metas.HouseDescribeError);
             
            #endregion

            #region 房源基础信息表
            HouseParame houseParame = new HouseParame()
            {
                UserId = userId,
                HouseId = item.HouseId,
                TradeType = item.PostType,
                BuildType = item.BuildType,

                CommunityId = item.CommunityId,
                CommunityName = item.CommunityName,

                CityId = item.CityId,
                Distrctid = item.DistrictId,
                RegionId = item.RegionId,
                Address = item.Address,
               
                BuildArea = item.BuildArea,
                UsedArea = item.UsedArea < 0 ? (item.BuildArea - 1) : item.UsedArea,

                Room = item.Room,
                Hall = item.Hall,
                Toilet = item.Toilet,
                Kitchen = item.Kitchen,
                Balcony = item.Balcony,

                Price = item.Price,
                PriceUnit = item.PriceUnit,
                PayType = string.IsNullOrEmpty(item.PayType) ? "" : item.PayType,
                UsedYear = item.UsedYear,

                CurFloor = item.CurFloor,
                MaxFloor = item.MaxFloor,
                PointTo = item.PointTo.IsNull() ? "" : item.PointTo,
                LookHouseTime = item.LookHouseTime.IsNull() ? "" : item.LookHouseTime,
                FitmentStatus = item.FitmentStatus.IsNull() ? "" : item.FitmentStatus,
                InternalNum = item.InternalNum.IsNull() ? "" : item.InternalNum,
                CellLabel = item.CellLabel.IsNull() ? "" : item.CellLabel,
                HouseLabel = item.HouseLabel,
                YiJuHua = item.YiJuHua.IsNull() ? "" : item.YiJuHua,

                Title = item.Title,
                Note = item.Note,

                PicNum = (item.RoomImages == null ? 0 : item.RoomImages.Length) + (item.FangXingImages == null ? 0 : item.FangXingImages.Length) + (item.OutDoorImages == null ? 0 : item.OutDoorImages.Length),
                HouseImgs = SetImageList(item),
               
                IsClone = false,
                BeColneID = "",
              
                Source = source,
             
            };
            #endregion
            #region 房源 住宅
            HouseInfoParame houseInfoParame = new HouseInfoParame();
           
                houseInfoParame = new HouseInfoParame()
                {
                    AdvEquip = item.HouseAdvEquip.IsNull() ? "" : item.HouseAdvEquip,
                    BasicEquipHouse = item.HouseBasicEquip.IsNull() ? "" : item.HouseBasicEquip,
                    FiveYears = item.FiveYears ,
                    HouseProperty = item.HouseProperty.IsNull() ? "" : item.HouseProperty,
                    HouseStructure = item.HouseStructure.IsNull() ? "" : item.HouseStructure,
                    HouseSubType = item.HouseSubType.IsNull() ? "" : item.HouseSubType,
                    HouseType = item.HouseType.IsNull() ? "" : item.HouseType,
                    LandYear = item.LandYear.IsNull() ? "" : item.LandYear,
                    OnlyHouse = item.OnlyHouse
                };
         
            #endregion
            #region 房源 别墅
            VillaInfoParame villaInfoParame = new VillaInfoParame();
            
                villaInfoParame = new VillaInfoParame()
                {
                    AdvEquip = item.VillaAdvEquip.IsNull() ? "" : item.VillaAdvEquip,
                    Basement = item.Basement,
                    BasicEquip = item.VillaBasicEquip.IsNull() ? "" : item.VillaBasicEquip,
                    FiveYears = item.FiveYears ,
                    Garage = item.Garage ,
                    Garden = item.Garden,
                    HallType = item.HallType,
                    LandYear = item.LandYear,
                    OnlyHouse = item.OnlyHouse ,
                    ParkLot = item.ParkLot ,
                    VillaType = item.VillaType.IsNull() ? "" : item.VillaType,
                };
           
            #endregion
            #region 房源 商铺
            ShopInfoParame shopInfoParame = new ShopInfoParame();
          
                shopInfoParame = new ShopInfoParame()
                {
                    BasicEquip = item.ShopBasicEquip.IsNull() ? "" : item.ShopBasicEquip,
                    Divide = item.ShopDivide,
                    Fee = item.ShopFee,
                    ShopStatus = item.ShopStatus.IsNull() ? "" : item.ShopStatus,
                    ShopType = item.ShopType.IsNull() ? "" : item.ShopType,
                    TargetField = item.TargetField.IsNull() ? "" : item.TargetField,
                };
         
            #endregion
            #region 房源 写字楼
            OfficeInfoParame officeInfoParame = new OfficeInfoParame();
           
                officeInfoParame = new OfficeInfoParame()
                {
                    BasicEquip = item.OfficeBasicEquip.IsNull() ? "" : item.OfficeBasicEquip,
                    Divide = item.OfficeDivide ,
                    Fee = item.OfficeFee,
                    OfficeLevel = item.OfficeLevel.IsNull() ? "" : item.OfficeLevel,
                    OfficeType = item.OfficeType.IsNull() ? "" : item.OfficeType,
                };
          
            #endregion
            #region 房源 工厂
            FactoryInfoParame factoryInfoParame = new FactoryInfoParame();
          
                factoryInfoParame = new FactoryInfoParame()
                {
                    BasicEquip = item.FactoryBasicEquip.IsNull() ? "" : item.FactoryBasicEquip,
                    FactoryType = item.FactoryType.IsNull() ? "" : item.FactoryType,
                };
          
            #endregion

            int houseid = houseBll.OperateHouse(houseParame, houseInfoParame, villaInfoParame, shopInfoParame, officeInfoParame, factoryInfoParame);

            return new ApiResponse(Metas.SUCCESS, new { HouseId = houseid });

        }
        private List<HouseImgParame> SetImageList(HouseReq item)
        {
            List<HouseImgParame> list = new List<HouseImgParame>();

            if (!string.IsNullOrEmpty(item.RoomImages))
            {
                item.RoomImages = item.RoomImages.Replace("，", ",");
                var arrRoomImages = item.RoomImages.Split(',');
                foreach (string arrRoomImage in arrRoomImages)
                {
                    if (string.IsNullOrEmpty(arrRoomImage))
                    {
                        continue;
                    }
                    list.Add(new HouseImgParame()
                    {
                        imageUrl = arrRoomImage,
                        imageType = 1
                    });
                }
               
            }

            if (!string.IsNullOrEmpty(item.FangXingImages))
            {
                item.FangXingImages = item.FangXingImages.Replace("，", ",");
                var arrFangXingImages = item.FangXingImages.Split(',');
                foreach (string arrFangXingImage in arrFangXingImages)
                {
                    if (string.IsNullOrEmpty(arrFangXingImage))
                    {
                        continue;
                    }
                    list.Add(new HouseImgParame()
                    {
                        imageUrl = arrFangXingImage,
                        imageType = 2
                    });
                }

            }
            if (!string.IsNullOrEmpty(item.OutDoorImages))
            {
                item.OutDoorImages = item.OutDoorImages.Replace("，", ",");
                var arrOutDoorImages = item.OutDoorImages.Split(',');
                foreach (string arrOutDoorImage in arrOutDoorImages)
                {
                    if (string.IsNullOrEmpty(arrOutDoorImage))
                    {
                        continue;
                    }
                    list.Add(new HouseImgParame()
                    {
                        imageUrl = arrOutDoorImage,
                        imageType = 3
                    });
                }

            }

           
            return list;
        }
        #endregion

        #region 收藏采集房源
        [System.Web.Http.HttpPost]
        [Token]
        public ApiResponse AddHouseCollection([FromBody]HouseReq houseData)
        {

            int userId = GetCurrentUserId();
            HouseCollection newCollect = new HouseCollection();

            newCollect = ncBase.CurrentEntities.HouseCollection.Where(c => c.CollectId == houseData.CollectId && c.UserId == userId).FirstOrDefault();
            byte balcony = 0;
            byte.TryParse(houseData.Balcony.ToString(), out balcony);
            decimal buildArea = houseData.BuildArea;
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
                    BuildingType = houseData.BuildType,
                    CityId = houseData.CityId,
                    CollectId = houseData.CollectId,
                    CommunityName = houseData.CommunityName,
                    CurFloor = houseData.CurFloor,
                    Distrctid = houseData.DistrictId,
                    DistrctName = houseData.DistrictName,
                    Hall = hall,
                    Kitchen = kitchen,
                    MaxFloor = houseData.MaxFloor,
                    PicNum = houseData.PicNum,
                    Price = price,
                    PriceUnit = houseData.PriceUnit,
                    Publisher = houseData.Publisher,
                    RegionID = houseData.RegionId,
                    RegionName = houseData.RegionName,
                    ReleaseTime = DateTimeUtility.FromUnixTime(houseData.ReleaseTime),
                    Room = room,
                    Source = houseData.Source,
                    Tel = houseData.Tel,
                    Title = houseData.Title,
                    Toilet = toilet,
                    TradeType = houseData.PostType,
                    Url = houseData.Url,
                    UserId = userId,
                    CollectStatus=1
                };
                ncBase.CurrentEntities.HouseCollection.AddObject(newCollect);
            }
            else
            {

                newCollect.Address = houseData.Address;
                newCollect.AddTime = DateTime.Now;
                newCollect.Balcony = balcony;
                newCollect.BuildArea = buildArea;
                newCollect.BuildingType = houseData.BuildType;
                newCollect.CityId = houseData.CityId;
                newCollect.CollectId = houseData.CollectId;
                newCollect.CommunityName = houseData.CommunityName;
                newCollect.CurFloor = houseData.CurFloor;
                newCollect.Distrctid = houseData.DistrictId;
                newCollect.DistrctName = houseData.DistrictName;
                newCollect.Hall = hall;
                newCollect.Kitchen = kitchen;
                newCollect.MaxFloor = houseData.MaxFloor;
                newCollect.PicNum = houseData.PicNum;
                newCollect.Price = price;
                newCollect.PriceUnit = houseData.PriceUnit;
                newCollect.Publisher = houseData.Publisher;
                newCollect.RegionID = houseData.RegionId;
                newCollect.RegionName = houseData.RegionName;
                newCollect.ReleaseTime = DateTimeUtility.FromUnixTime(houseData.ReleaseTime);
                newCollect.Room = room;
                newCollect.Source = houseData.Source;
                newCollect.Tel = houseData.Tel;
                newCollect.Title = houseData.Title;
                newCollect.Toilet = toilet;
                newCollect.TradeType = houseData.PostType;
                newCollect.Url = houseData.Url;
                newCollect.UserId = userId;
                newCollect.CollectStatus = 1;
            }
            ncBase.CurrentEntities.SaveChanges();
            return new ApiResponse(Metas.SUCCESS, new { CollectId = newCollect.Id });
        }
      
        #endregion

        #region 获取房源提醒
        [System.Web.Http.HttpGet]
        [Token]
        public ApiResponse GetHouseRemind()
        {
            var thisUser = Request.GetCredential();
      
            List<HouseRemindSet> houseReminds =
                    ncBase.CurrentEntities.HouseRemindSet.Where(o => o.UserId == thisUser.UserID).ToList();
            var result = houseReminds.Select(p => new
            {
                p.AttentionCommunity,
                p.TradeType,
                p.CityId
            });
            return new ApiResponse(Metas.SUCCESS, result );
        }

        #endregion

        #region 房源提醒设置
        [System.Web.Http.HttpPost]
        [Token]
        public ApiResponse SetHouseRemind([FromBody]HouseRemindReq model)
        {
            var thisUser = Request.GetCredential();
           string Keywords =!string.IsNullOrEmpty(model.Keywords)? model.Keywords.Trim().Replace(" ", ""):"";
            HouseBll houseBll = new HouseBll();
      
            bool flag = false;
            HouseRemindSet houseRemind = ncBase.CurrentEntities.HouseRemindSet.Where(o => o.UserId == thisUser.UserID && o.TradeType == model.tradeType).FirstOrDefault();
            HouseRemindSet houseRemindOld = houseRemind;
            if (houseRemindOld.IsNoNull())
            {
                var KeywordListsOld = houseRemindOld.AttentionCommunity.Split(',');

                foreach (var s in KeywordListsOld)
                {
                    if (!string.IsNullOrEmpty(s))
                        houseBll.RemoveKeyword(s, model.tradeType, thisUser.CityId, thisUser.UserID);
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
                            houseBll.AddKeyword(s, model.tradeType, thisUser.CityId, thisUser.UserID);
                        }
                    }
                }
            }

            if (houseRemind.IsNull())
            {
                houseRemind = new HouseRemindSet();
                flag = true;
            }
            houseRemind.CityId =thisUser.CityId;
            houseRemind.AddTime = DateTime.Now;
            houseRemind.TradeType = model.tradeType;
            houseRemind.UserId = thisUser.UserID;
            houseRemind.AttentionCommunity = Keywords;
            if (flag)
            {
                ncBase.CurrentEntities.AddToHouseRemindSet(houseRemind);
            }
            ncBase.CurrentEntities.SaveChanges();
            int gainPoints = 0;
            string gainPointsMsg = "";
            DoTask(thisUser.UserID, PointsEnum.First_HouseRemind, out gainPoints); //设置房源提醒

            if(gainPoints>0)
            gainPointsMsg = "完成“房源提醒设置”任务";
            var result = new
            {
                GainPoints = gainPoints,
                GainPointsMsg = gainPointsMsg
            };
            return new ApiResponse(Metas.SUCCESS,result);
        }

        #endregion

        #region 私有方法
        /// <summary>
        /// 获取当前登录的用户ID
        /// </summary>
        /// <returns></returns>
        private int GetCurrentUserId()
        {
            var credential = Request.GetCredential();
            int userId = 0;
            if (credential != null)
            {
                userId = credential.UserID;
            }
            return userId;
        }
        #endregion

    }
}
