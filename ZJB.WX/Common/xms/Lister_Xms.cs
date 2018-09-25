using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Data.Objects;
using System.Linq;
using System.Net;
using System.Text.RegularExpressions;
using HtmlAgilityPack;
using ZJB.Api.BLL;
using ZJB.Api.Entity;
using ZJB.Core.Utilities;

namespace ZJB.WX.Common.xms
{
    internal class Lister_Xms //获取列表
    {
        private const string baseApi = "http://release.xms.4846.com/release-web/ajax/houman/gethouse.do";

        private static NCDBEntities WX = new NCBaseRule().CurrentEntities;
        private const string paramApi =
            "postType={0}&buildingType={1}&buildingStatus=0&cell=&sort=&title=&isDraft={2}0&price1=&price2=&tags=&page={3}";

        private const string detailApi = "http://release.xms.4846.com/release-web/import/getimpview.do?postType=0" +
                                         "&buildingType={0}&editType=U&buildingId={1}&selectCell=";

        private static NameValueCollection GetHeaders(string cookie)
        {
            return new NameValueCollection
            {
                {"Cookie", cookie},
                {
                    "User-Agent",
                    "Mozilla/5.0 (Windows NT 6.1; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/39.0.2171.95 Safari/537.36"
                },
            };
        }

        /// <summary>
        ///     获取房源详情
        /// </summary>
        /// <param name="tradeType"></param>
        /// <param name="buildingType">1 住宅 2 别墅 3 商铺 4 写字楼 5 厂房</param>
        /// <param name="cookie">Cookie</param>
        /// ///
        /// <param name="size">数量</param>
        /// <returns></returns>
        public static IEnumerable<ImportedHouse> GetHouseDetails(byte tradeType,int buildingType,
            string cookie,
            int size,Dictionary<int,string> communityList=null)
        {
            int postType = ConvertPostType(tradeType);
           Dictionary<string,DateTime> ids = GetHouseIds(postType, buildingType, cookie, size);
           return ids.Select(t => GetHouseDetail(tradeType, buildingType, t.Key, cookie, t.Value, communityList)).Where(o => o != null).ToList();
        }
        private static int ConvertPostType(int tradeType)
        {
            switch (tradeType)
            {
                case 1:
                    return 0;
                case 3:
                    return 1;
            }
            return 0;
        }

        /// <summary>
        ///     获取房源列表的id
        /// </summary>
        /// <param name="postType">0 出售 1 出租</param>
        /// <param name="buildingType">1 住宅 2 别墅 3 商铺 4 写字楼 5 厂房</param>
        /// <param name="cookie">Cookie</param>
        /// <param name="size">数量</param>
        /// <returns></returns>
        private static Dictionary<string,DateTime> GetHouseIds(int postType, int buildingType, string cookie, int size)
        {
            var ids = new Dictionary<string, DateTime>();
            WebProxy proxy = KuaiDaili.GetProxy();
            for (int i = 1; i < 30; i++)
            {
                string html = HttpUtility.PostString(baseApi, string.Format(paramApi, postType, buildingType, 0, i), GetHeaders(cookie), proxy);
                //var doc = new HtmlDocument();
                //doc.LoadHtml(html);
                //HtmlNode node = doc.DocumentNode;
                //var trs = node.SelectNodes("//tbody[@id='houseTable-title']/tr");
                //foreach (HtmlNode tr in trs)
                //{
                    
                //}
                MatchCollection ms = Regex.Matches(html, "createTime=([\\d\\.\\s:-]+)&editType=U&buildingId=(\\d+)'");
                //ids.AddRange(from Match m in ms where m.Success select m.Groups[1].Value);

                foreach (Match m in ms)
                {
                    if (m.Success)
                    {
                        ids.Add(m.Groups[2].Value, DateTime.Parse(m.Groups[1].Value));
                    }
                }

                if (ids.Count() >= size)
                {
                    break;
                }
            }
            return ids;
        }

        /// <summary>
        ///     获取单个房源详情
        /// </summary>
        /// <param name="tradeType">id</param>
        /// <param name="buildingType">1 住宅 2 别墅 3 商铺 4 写字楼 5 厂房</param>
        /// <param name="id">id</param>
        /// <param name="cookie">Cookie</param>
        /// <returns></returns>
        private static ImportedHouse GetHouseDetail(byte tradeType, int buildingType, string id, string cookie, DateTime date, Dictionary<int, string> communityList)
        {
            const int siteId = 1008;
            string html = string.Empty;
            try
            {
                WebProxy proxy = KuaiDaili.GetProxy();
                 html = HttpUtility.GetString(string.Format(detailApi, buildingType, id),
                    GetHeaders(cookie), false, proxy);
            }
            catch (WebException ex)
            {
                
                return null;
            }
            var doc = new HtmlDocument();
            doc.LoadHtml(html);
            HtmlNode node = doc.DocumentNode;
            HtmlNode title_Node = node.SelectSingleNode("//input[@id='saleHouse-title']");
            HtmlNode xiaoquId_Node = node.SelectSingleNode("//input[@id='cellCode']");
            HtmlNode xiaoqu_Node = node.SelectSingleNode("//input[@id='cell']");
            HtmlNode address_Node = node.SelectSingleNode("//input[@id='addr']");
            HtmlNode area_Node = node.SelectSingleNode("//input[@id='houseArea']");
            HtmlNode areaUsed_Node = node.SelectSingleNode("//input[@id='areaUsed']");
            HtmlNode price_Node = node.SelectSingleNode("//input[@id='price']");
            HtmlNode floor_Node = node.SelectSingleNode("//input[@id='saleHouse-basic-flooron']");
            HtmlNode florrAll_Node = node.SelectSingleNode("//input[@id='saleHouse-basic-floorall']");
            HtmlNode note_Node = node.SelectSingleNode("//textarea[@id='houseDescribe']");
            HtmlNode yijuhua_Node = node.SelectSingleNode("//input[@id='saleHouse-tongcheng-tip']");
            HtmlNode tags_Node = node.SelectSingleNode("//input[@id='saleHouse-house-tip']");
            HtmlNode tese_Node = node.SelectSingleNode("//input[@id='saleHouse-eare-tip']");
            //HtmlNode createTime_Node = node.SelectSingleNode("//input[@id='releaseTime']");

            MatchCollection roomimageMatches = new Regex("createImgDiv\\('(.*?IMG_I.*?)'").Matches(html);
            MatchCollection outdoorimageMatches = new Regex("createImgDiv\\('(.*?IMG_O.*?)'").Matches(html);
            MatchCollection fangxingimageMatches = new Regex("createImgDiv\\('(.*?IMG_M.*?)'").Matches(html);
            string payType = new Regex("\\$\\(\"#payType\"\\)\\.val\\(\"(.*?)\"").Match(html).Groups[1].Value;
            string year = new Regex("\\$\\(\"#saleHouse-basic-year\"\\)\\.val\\('(.*?)'").Match(html).Groups[1].Value;
            string pointTo = new Regex("name=pointTo\\]\\[value='(.*?)'").Match(html).Groups[1].Value;
            string lookTime = new Regex("name=lookTime\\]\\[value='(.*?)'").Match(html).Groups[1].Value;
            string fitmentStatus = new Regex("name=fitmentStatus\\]\\[value='(.*?)'").Match(html).Groups[1].Value;

            string district = new Regex("\\$\\(\"#distrctSelect\"\\)\\.val\\('(\\d+)'").Match(html).Groups[1].Value;
            string area = new Regex("\\$\\(\"#areaSelect\"\\)\\.val\\('(\\d+)'").Match(html).Groups[1].Value;
            int districtId = PostDataHelper.RMappingAreaID(district, siteId, 592);
            string communityName = xiaoqu_Node.GetAttributeValue("value", "");
            var community = PostDataHelper.RMappingCommunityModel(xiaoquId_Node.GetAttributeValue("value", ""), siteId);
            if (community == null)
            {
                return null;
            }
            int communityId = community.CommunityID;

            int areaId = community.RegionID>0?(int)community.RegionID:PostDataHelper.RMappingAreaID(area, siteId, districtId);

            var house = new ImportedHouse
            {
                HouseID = id,
                CommunityID = communityId,
                CommunityName = communityList!=null&&communityList.ContainsKey(communityId)?communityList[communityId]:communityName,
                CurFloor = Helper_Xms.shortParse(Helper_Xms.GetAttributeValue(floor_Node)),
                MaxFloor = Helper_Xms.shortParse(Helper_Xms.GetAttributeValue(florrAll_Node)),
                BuildArea = Helper_Xms.shortParse(Helper_Xms.GetAttributeValue(area_Node)),
                UsedArea = Helper_Xms.shortParse(Helper_Xms.GetAttributeValue(areaUsed_Node)),
                UsedYear = Helper_Xms.shortParse(year),
                Price = Helper_Xms.shortParse(Helper_Xms.GetAttributeValue(price_Node)),
                PriceUnit = tradeType == 1 ? "万" : "元/月",
                Address = Helper_Xms.GetAttributeValue(address_Node),
                Title = title_Node.GetAttributeValue("value", ""),
                Note = note_Node == null ? "" : note_Node.InnerHtml,
                YiJuHua = Helper_Xms.GetAttributeValue(yijuhua_Node),
                HouseLabel = Helper_Xms.GetAttributeValue(tags_Node),
                CellLabel = Helper_Xms.GetAttributeValue(tese_Node),
                Distrctid = districtId,
                CityID = community.CityID ?? 0,
                RegionID = areaId,
                BuildType = (byte) buildingType,
                FitmentStatus = fitmentStatus,
                PointTo = pointTo,
                PayType = payType,
                Status = 1,
                LookHouseTime = lookTime,
                TradeType = tradeType,
                roomImages = (from Match imageMatch in roomimageMatches select imageMatch.Groups[1].Value).ToArray(),
                outdoorImages = (from Match imageMatch in outdoorimageMatches select imageMatch.Groups[1].Value).ToArray(),
                fangxingImages = (from Match imageMatch in fangxingimageMatches select imageMatch.Groups[1].Value).ToArray(),
                PostTime = date
            };
            switch (buildingType)
            {
                case 1:
                    house = Helper_Xms.GetHouseInfo(html, house);
                    break;
                case 2:
                    house = Helper_Xms.GetVillaInfo(html, house);
                    break;
                case 3:
                    house = Helper_Xms.GetShopInfo(html, house);
                    break;
                case 4:
                    house = Helper_Xms.GetOfficeInfo(html, house);
                    break;
                case 5:
                    house = Helper_Xms.GetFactoryInfo(html, house);
                    break;
            }
            return house;
        }


//        /// <summary>
//        /// 获取房源列表的全部id
//        /// </summary>
//        /// <param name="postType">0 出售 1 出租</param>
//        /// <param name="buildingType">1 住宅 2 别墅 3 商铺 4 写字楼 5 厂房</param>
//        /// <param name="isDraft">0 发布中 1 草稿 </param>
//        /// <param name="page">分页</param>
//        /// <param name="cookie">Cookie</param>
//        /// <returns></returns>
//        private static List<string> GetHouseIds(int postType, int buildingType, int isDraft, int page,
//            string cookie)
//        {
//            string ret = HttpUtility.PostString(baseApi, string.Format(paramApi, postType, buildingType, isDraft, page),
//                GetHeaders(cookie));
//            var ms = Regex.Matches(ret, "div id=\"(\\d+)");
//            return (from Match match in ms where match.Success select match.Groups[1].Value).ToList();
//        }

//        /// <summary>
//        /// 获取房源列表，包含一些基本的房源信息
//        /// </summary>
//        /// <param name="postType">0 出售 1 出租</param>
//        /// <param name="buildingType">1 住宅 2 别墅 3 商铺 4 写字楼 5 厂房</param>
//        /// <param name="isDraft">0 发布中 1 草稿 </param>
//        /// <param name="page">分页</param>
//        /// <param name="cookie">Cookie</param>
//        /// <returns></returns>
//        public static List<XmsHouseInfo> GetHouseList(int postType, int buildingType, int isDraft, int page,
//            string cookie)
//        {
//            string ret = HttpUtility.PostString(baseApi, string.Format(paramApi, postType, buildingType, isDraft, page),
//                GetHeaders(cookie));
//            var houseList = new List<XmsHouseInfo>();
//
//
//
//            throw new System.NotImplementedException();
////            var ms = Regex.Matches(ret, "div id=\"(\\d+)");
////            return houseList;
//        }
    }
}