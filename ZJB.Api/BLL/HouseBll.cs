using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Caching;
using MongoDB.Bson;
using MongoDB.Driver;
using ZJB.Api.Common;
using ZJB.Api.DAL;
using ZJB.Api.Entity;
using ZJB.Api.Model;
using ZJB.Api.Models;
using ZJB.Api.Models.Community;
using ZJB.Api.Models.Parame;
using ZJB.Core.Injection;
using ZJB.Core.Utilities;
using ZJB.Web.Utilities;
namespace ZJB.Api.BLL
{
    public class HouseBll
    {
        private readonly HouseDal houseDal = Container.Instance.Resolve<HouseDal>();

        #region 获取房源列表
        public virtual List<HouseBasicInfoModel> GetHouseList(HouseListReq parames, ref int totalSize)
        {
            return houseDal.GetHouseList(parames, ref totalSize);
        }

        public virtual List<HouseBasicInfoModel> GetEsfHouseList(HouseListReq parames, ref int totalSize)
        {
            return houseDal.GetEsfHouseList(parames, ref totalSize);
        }

        public virtual string SetInterview(int houseid, int userid, int tpye, string tel)
        {
            return houseDal.SetInterview(houseid, userid, tpye, tel);
        }
        public virtual int SetHouseImages(int houseid, string imgurls, int imgtype = 1, int communityid = 0, int userid = 0)
        {
            return houseDal.SetHouseImages(houseid, imgurls, imgtype, communityid, userid);
        }

        /// <summary>
        /// 举报
        /// </summary>
        /// <param name="aid"></param>
        /// <param name="houseid"></param>
        /// <param name="type"></param>
        /// <param name="contents"></param>
        /// <returns></returns>
        public virtual int AddAccusationLog(int aid, int userid, int houseid = 0, int type = 1, string contents = "", string tel = "")
        {
            return houseDal.AddAccusationLog(aid, userid, houseid, type, contents, tel);
        }

        public virtual int AddBrowse(int houseid, int userid, int tpye)
        {
            return houseDal.AddBrowse(houseid, userid, tpye);
        }
        public virtual List<PublicUserModel> GetBrowse(int houseid, int pageIndex, int pageSize, ref int totalSize)
        {
            return houseDal.GetBrowse(houseid, pageIndex, pageSize, ref totalSize);
        }



        /// <summary>
        /// 更新浏览数
        /// </summary>
        /// <param name="houseid"></param>
        /// <returns></returns>
        public virtual int SetHouseHits(int houseid)
        {
            return houseDal.SetHouseHits(houseid);
        }

        public virtual DataSet GetInterview(int houseid)
        {
            return houseDal.GetInterview(houseid);
        }
        public virtual DataSet GetConsult(int houseid)
        {
            return houseDal.GetConsult(houseid);
        }
        /// <summary>
        /// 预约和报价详细
        /// </summary>
        /// <param name="houseid"></param>
        /// <returns></returns>
        public virtual DataSet GetConsultInterview(int houseid)
        {
            return houseDal.GetConsultInterview(houseid);
        }

        public virtual int SetCollect(int houseid, int userid, int type)
        {
            return houseDal.SetCollect(houseid, userid, type);
        }

        /// <summary>
        /// 添加谈价磋商
        /// </summary>
        /// <param name="houseid"></param>
        /// <param name="userid"></param>
        /// <param name="tpye">0:申请，1：通知业主，2：磋商成功</param>
        /// <param name="tel"></param>
        /// <param name="price">价钱</param>
        /// <returns></returns>
        public virtual string AddConsult(int houseid, int userid, int tpye, string tel, double price)
        {
            return houseDal.AddConsult(houseid, userid, tpye, tel, price);
        }

        #endregion
        #region 前台房源列表

        /// <summary>
        /// 获取房源列表
        /// </summary>
        /// <param name="houseInfoParameter">搜索参数</param>
        /// <returns></returns>
        public virtual List<HouseBasicInfoModel> GetHouseBasicInfoList(HouseParame houseInfoParameter, out int rows)
        {
            return houseDal.GetHouseBasicInfoList(houseInfoParameter, out rows);
        }
        #endregion
        #region 批量更改房源状态
        public virtual int ChangeHouseStatus(ChangeHouseStatusReq parames)
        {
            return houseDal.ChangeHouseStatus(parames);
        }
        #endregion

        #region 获取用户房源的所有小区名
        public virtual List<CommunityModel> GetUserHouseCommunityList(GetUserHouseCommunityListReq parames)
        {
            return houseDal.GetUserHouseCommunityList(parames);
        }
        #endregion

        #region 房源数据总数统计
        public virtual List<HouseNumSumModel> GetHouseNumSumData(int userId, int posttype)
        {
            return houseDal.GetHouseNumSumData(userId);
        }
        #endregion

        #region 更改房源标签
        public virtual int UpdateHouseTags(int userId, string houseIds, string tags)
        {
            return houseDal.UpdateHouseTags(userId, houseIds, tags);
        }
        #endregion

        #region 删除房源图片
        public virtual int DelHouseImageByHouseID(int houseId, int userId)
        {
            return houseDal.DelHouseImageByHouseID(houseId, userId);
        }
        #endregion

        #region 管理后台 每日房源新增统计

        public virtual List<StatModel> GetHouseAddStat(StatReq parame)
        {
            return houseDal.GetHouseAddStat(parame);
        }
        #endregion

        #region 获取采集房源列表页面
        public virtual MongoCursor<HouseCrawler> GetHouseCollectList(HouseListReq parames, ref int totalSize)
        {
            return houseDal.GetHouseCollectList(parames, ref totalSize);
        }

        public virtual HouseCrawler GetHouseCollect(string id, string dbCollection)
        {
            return houseDal.GetHouseCollect(id, dbCollection);
        }

        /// <summary>
        /// 采集的房源 的网站来源字典表
        /// </summary>
        /// <param name="cityId"></param>
        /// <returns></returns>
        public virtual MongoCursor<HouseCollectSource> GetHouseCollectSite(int cityId)
        {
            return houseDal.GetHouseCollectSite(cityId);
        }
        ///<summary>
        /// 添加 已读
        ///</summary>
        ///<returns></returns>
        public virtual void HouseCollectReadAdd(string id, int userid, string username)
        {
            houseDal.HouseCollectReadAdd(id, userid, username);
        }
        /// <summary>
        /// 采集房源阅读数
        /// </summary>
        public virtual MongoCursor<HouseCollectViewLog> GetHouseCollectReadLogByIds(List<string> ids)
        {
            return houseDal.GetHouseCollectReadLogByIds(ids);
        }
        #endregion

        #region 获取我的收藏
        public List<HouseCollection> GetCollectionList(HouseListReq parame, ref int totalSize)
        {
            return houseDal.GetCollectionList(parame, ref totalSize);
        }
        #endregion

        #region 房源提醒关键字设置


        //增加关键词及对应的用户id
        public virtual void AddKeyword(string keyword, int tradeType, int cityId, int userId)
        {
            houseDal.AddKeyword(keyword: keyword, cityId: cityId, userId: userId, tradeType: tradeType);
        }
        //从关键词的user_ids中删除某个userid
        public virtual void RemoveKeyword(string keyword, int tradeType, int cityId, int userId)
        {
            houseDal.RemoveKeyword(keyword: keyword, cityId: cityId, userId: userId, tradeType: tradeType);
        }


        #endregion

        #region 房源发布/编辑

        /// <summary>
        /// 房源发布/编辑
        /// </summary>
        /// <param name="basicInfo"></param>
        /// <param name="houseInfoParame"></param>
        /// <param name="villaInfoParame"></param>
        /// <param name="shopInfoParame"></param>
        /// <param name="officeInfoParame"></param>
        /// <param name="factoryInfoParame"></param>
        /// <returns>
        /// basicInfo的HouseId大于0并且匹配UserId 才能编辑
        /// </returns>
        public virtual int OperateHouse(HouseParame basicInfo, HouseInfoParame houseInfoParame,
            VillaInfoParame villaInfoParame, ShopInfoParame shopInfoParame, OfficeInfoParame officeInfoParame,
            FactoryInfoParame factoryInfoParame)
        {
            return houseDal.OperateHouse(basicInfo, houseInfoParame, villaInfoParame, shopInfoParame, officeInfoParame,
                factoryInfoParame);
        }
        public virtual List<ImpHouseResultModel> ImpHouseBatch(List<HouseParame> basicInfoList, List<HouseInfoParame> houseInfoParame,
            List<VillaInfoParame> villaInfoParame, List<ShopInfoParame> shopInfoParame, List<OfficeInfoParame> officeInfoParame,
            List<FactoryInfoParame> factoryInfoParame, List<HouseImgParame> houseImgParameList, int userId)
        {

            #region 逻辑转换
            foreach (HouseParame basicInfo in basicInfoList)
            {
                int houseId = basicInfo.HouseId;
                #region 房源 基础表
                //房源图片相关
                string imgUrlCover = "";
                int picNum = basicInfo.HouseImgs.Count;
                if (basicInfo.HouseImgs.IsNoNull() && basicInfo.HouseImgs.Count > 0)
                {
                    var isCoverImg = basicInfo.HouseImgs.Where(o => o.IsCover == true).FirstOrDefault();
                    if (isCoverImg.IsNoNull())
                    {
                        imgUrlCover = isCoverImg.imageUrl;
                    }
                    else
                    {
                        imgUrlCover = basicInfo.HouseImgs.FirstOrDefault().imageUrl;
                    }

                }

                //价格相关
                decimal lowpay = 0; //最低首付
                string priceUnit;  //价格单位
                decimal unitPrice = 0;

                switch (basicInfo.TradeType)
                {
                    case (int)TradeType.Sell:
                        priceUnit = "万";
                        unitPrice = basicInfo.BuildArea > 0 ? basicInfo.Price * 10000 / basicInfo.BuildArea : 0;  //出售计算单价
                        lowpay = basicInfo.Price * 10000 * Convert.ToDecimal(0.30);  //最低首付计算
                        break;
                    case (int)TradeType.Rent:
                        switch (basicInfo.BuildType)
                        {
                            case (int)BuildingType.House:
                            case (int)BuildingType.Villa:
                                priceUnit = "元/月"; break;
                            default:
                                priceUnit = basicInfo.PriceUnit; break;
                        }
                        break;

                    default: priceUnit = basicInfo.PriceUnit; break;
                }
                Regex replare = new Regex("\\&[lr]dquo;");
                basicInfo.UnitPrice = unitPrice;
                basicInfo.PriceUnit = priceUnit;
                basicInfo.LowPay = lowpay;
                basicInfo.ExpireDay = DateTime.Now.AddDays(30);
                basicInfo.PicNum = picNum;
                basicInfo.Note = replare.Replace(basicInfo.Note, "\"");
                basicInfo.Status = (int)HouseStatus.Release;
                basicInfo.IP = IpUtility.GetIp();
                basicInfo.PostTime = DateTime.Now;
                basicInfo.HouseImgPath = imgUrlCover;

                if (houseId.Equals(0)) //添加操作时候
                {
                    basicInfo.IsClone = basicInfo.IsClone.IsNull() ? false : basicInfo.IsClone;
                    basicInfo.Tag = "";
                }
                #endregion
                List<HouseImgParame> thisImageList = houseImgParameList.Where(s => s.IndexPoint == basicInfo.IndexPoint).ToList();
                if (thisImageList.IsNoNull() && thisImageList.Count > 0)
                {
                    int i = 1;
                    foreach (HouseImgParame item in basicInfo.HouseImgs)
                    {
                        item.HouseID = houseId;
                        item.ImagePath = item.imageUrl;
                        item.ImagePos = item.imgDescribe;
                        item.OrderID = i;
                        item.AddTime = DateTime.Now;
                        item.Status = 1;
                        item.CommunityID = basicInfo.CommunityId;
                        item.UserID = basicInfo.UserId;
                        item.ImageType = item.ImageType;
                        i++;
                    }
                }
            }
            #endregion
            return houseDal.ImpHouseBatch(basicInfoList, houseInfoParame, villaInfoParame, shopInfoParame, officeInfoParame,
                  factoryInfoParame, houseImgParameList, userId);
        }
        #endregion

        public virtual HashSet<string> GetJjrTels()
        {
            HashSet<string> hashset;
            Cache cache = HttpContext.Current.Cache;
            var obj = cache.Get("JjrTels");
            if (obj == null)
            {
                var lines = System.IO.File.ReadAllLines(AppDomain.CurrentDomain.BaseDirectory + @"Config\JjrTels.txt");
                hashset = new HashSet<string>(lines);
                cache["JjrTels"] = hashset;
            }
            else
            {
                hashset = (HashSet<string>)obj;
            }
            return hashset;
        }
        #region 获取小区列表 
        public virtual List<Community> GetAllHouseCommunityList()
        {
            return houseDal.GetAllHouseCommunityList();
        }
        #endregion

        #region 获取小区历史价格列表 
        public virtual List<CommunityHistoryPrice> GetLpHistoryPriceList(int communityID)
        {
            return houseDal.GetLpHistoryPriceList(communityID);
        }
        #endregion


        #region 小区匹配
        public virtual List<TargetLoupan> GetTargetLoupanList(int index, int size, int siteId, int cityId, string name, out int totalCount)
        {
            return houseDal.GetTargetLoupanList(index, size, siteId, cityId, name, out totalCount);
        }

        public virtual List<SimilarCommunity> GetSimilarCommunityList(string name, int siteId)
        {
            List<SimilarCommunity> result = new List<SimilarCommunity>();
            List<SimilarCommunity> similarList = new List<SimilarCommunity>();
            List<SimilarCommunity> list = houseDal.GetCommunityTargetBySite(siteId);

            foreach (var l in list)
            {
                var level = TextUtility.GetRelateLevel(name, l.Name);
                if (level > 0.65)
                {
                    l.Level = Convert.ToDecimal(level);
                    result.Add(l);
                }
            }

            if (result.Count > 0)
            {
                string idList = "";
                foreach (var r in result)
                {
                    idList += r.CommunityID + ",";
                }
                similarList = houseDal.GetSimilarCommunityByIdList(idList);
                foreach (var s in similarList)
                {
                    SimilarCommunity sc = result.Find(x => x.CommunityID == s.CommunityID);
                    s.Level = sc.Level;
                }
            }
            return similarList;
        }

        public virtual void MappingCircle(int siteId, int id, string communityId)
        {
            houseDal.MappingCircle(siteId, id, communityId);
        }

        public virtual void FinishMapping(string id)
        {
            houseDal.FinishMapping(id);
        }

        public virtual List<Community> GetCircleListForManage(string circleName, int index, int size, int status, out int totalCount)
        {
            return houseDal.GetCircleListForManage(circleName, index, size, status, out totalCount);
        }

        public virtual Community GetCommunityById(int id)
        {
            return houseDal.GetCommunityById(id);
        }

        public virtual int UpdateCommunityDetail(Community community)
        {
            return houseDal.UpdateCommunityDetail(community);
        }
        #endregion

        #region 获取共享房源列表
        public virtual List<HouseBasicInfoModel> GetHouseShareList(HouseListReq parames, ref int totalSize)
        {
            return houseDal.GetHouseShareList(parames, ref totalSize);
        }
        #endregion


        #region 报备客户
        public virtual int AddBaoBei( int hid = 0, string name = "", int userId = 0,
                                                string tel = "",decimal price=0, int sex = 1, string appointmentTime = "", string notes = "")
        {
            return houseDal.AddBaoBei( hid, name, userId,
                                                   tel, price, sex, appointmentTime, notes);
        }
        #endregion

        #region 客户列表
        public virtual List<HousingSalesModel> GetBaobeiList(int userid,int state, int pageindex, int pagesize)
        {
           
            return houseDal.GetBaobeiList(userid, state, pageindex, pagesize);

        }
        #endregion
    }
}