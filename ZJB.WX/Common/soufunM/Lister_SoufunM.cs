using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using ZJB.Api.Entity;
using ZJB.Core.Utilities;

namespace ZJB.WX.Common.xms
{
    internal class Lister_SoufunM //获取列表
    {
        private const string baseApi = "http://agentapphouse.3g.fang.com/http/agentservice.jsp?";

        private const string paramApi =
            "pagesize=200&jkversion=1&city={0}&agentid={1}&housetype={2}&flag=1&messagename=gethouselist" +
            "&pageindex=1&verifycode={3}&refreshtype=all";
        private const string detailParamApi =
           "flag=1&houseid={0}&messagename=gethouseinfo&verifycode={1}&htype={2}&purpose=0&agentid={3}&city={4}";

        public static IEnumerable<int> GetHouseIds(string city, int agentId, string houseType, string verifyCode,
            int userId, int? purpose = null)
        {
            string param = string.Format(paramApi, Uri.EscapeDataString(city),
                agentId, houseType, verifyCode);
            if (purpose != null)
            {
                param += "&purpose=" + purpose;
            }
            string url = baseApi + param + "&wirelesscode=" + Helper_Soufun.GetWireless(param).ToLower();
            string ret = HttpUtility.GetString(url, Helper_Soufun.GetUserHeaders(userId));
            MatchCollection ms = Regex.Matches(ret, "<houseid>(\\d+)");
            return (from Match m in ms where m.Success select int.Parse(m.Groups[1].Value)).ToArray();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="houseType">租售</param>
        /// <param name="purpose">建筑类型</param>
        /// <param name="cookie"></param>
        /// <param name="size"></param>
        /// <returns></returns>
        public static IEnumerable<ImportedHouse> GetHouseDetails(int houseType, int purpose, string cookie, int size)
        {
            const int siteId = 2;
            string type = houseType == 1 ? "cs" : "cz";
            int soufun_userId = int.Parse(GetMatch(cookie, "<userid>(\\d+)</userid>"));
            int agentId = int.Parse(GetMatch(cookie, "<agentid>(\\d+)</agentid>"));
            string verifyCode = GetMatch(cookie, "<verifyCode>(\\w+)</verifyCode>");
            string city = GetMatch(cookie, "<city>(\\w+)</city>");
            string param = string.Format(paramApi, Uri.EscapeDataString(city),
               agentId, type, verifyCode);
            if (purpose != 0)
            {
                param += "&purpose=" + GetPrupose(purpose);
            }
            string url = baseApi + param + "&wirelesscode=" + Helper_Soufun.GetWireless(param).ToLower();
            string ret = HttpUtility.GetString(url, Helper_Soufun.GetUserHeaders(soufun_userId));
            var houseList = Helper_Soufun.GetObjectFromSFUrl<HouseListRoot>(ret);

            List<ImportedHouse> houses = new List<ImportedHouse>();

            foreach (var list in houseList.houselist)
            {

                //flag=1&houseid={0}&messagename=gethouseinfo&verifycode={1}&htype={2}&purpose=0&agentid={3}&city={4}
                string detailparam = string.Format(detailParamApi, list.houseid, verifyCode, type,agentId, Uri.EscapeDataString(city));
                string detailUrl = baseApi + detailparam + "&wirelesscode=" + Helper_Soufun.GetWireless(detailparam).ToLower();
                string detailRet = HttpUtility.GetString(detailUrl, Helper_Soufun.GetUserHeaders(soufun_userId));
                magent_interface mi = Helper_Soufun.GetObjectFromSFUrl<magent_interface>(detailRet);
                if (mi != null)
                {
                    magent_interfaceHouse houseDetail = mi.house;
                    var community = PostDataHelper.RMappingCommunityModel(
                        houseDetail.projcode.ToString(), siteId);
                    if (community.IsNoNull())
                    {

                        //var roomArr = houseDetail.roomphotourls.Split(',');
                        //HashSet<string> rooms = new HashSet<string>();
                        //foreach (var room in roomArr) {
                        //    rooms.Add(room.Replace("120x120", "600x600"));
                        //}
                        //var fangxingArr = houseDetail.roomphotourls.Split(',');
                        //HashSet<string> fangxingArrS = new HashSet<string>();
                        //foreach (var room in fangxingArr)
                        //{
                        //    fangxingArrS.Add(room.Replace("120x120", "600x600"));
                        //}

                        string content;
                        try
                        {
                            string html = HttpUtility.GetString(mi.houseurl, Encoding.GetEncoding("gb2312"), null, true);
                            content = Regex.Match(html, "<div.*id=\"hsPro-pos\".*\n([\\w\\W]*?)<div.*id=\"hsPic-pos\">").Groups[1].Value;
                            if (string.IsNullOrEmpty(content))
                            {
                                content = houseDetail.boardcontent;
                            }
                        }
                        catch (Exception)
                        {
                            content = houseDetail.boardcontent;
                        }

                        ImportedHouse house = new ImportedHouse
                        {
                            Hall = houseDetail.hall,
                            Room = houseDetail.room,
                            BuildArea = houseDetail.buildingarea,
                            UsedArea = houseDetail.livearea,
                            PointTo = houseDetail.forward,
                            Title = houseDetail.boardtitle,
                            Note = content,
                            Price = houseDetail.price,
                            PayType = houseDetail.payinfo,
                            PriceUnit = houseDetail.pricetype,
                            CommunityName = houseDetail.projname,
                            Address = houseDetail.address,
                            Balcony = houseDetail.balcony,
                            MaxFloor = houseDetail.totalfloor,
                            CurFloor = houseDetail.floor,
                            Toilet = houseDetail.toilet,
                            UsedYear = houseDetail.createtime,
                            HouseID = list.houseid,
                            BuildType = BitConverter.GetBytes(houseType)[0],
                            TradeType = BitConverter.GetBytes(purpose)[0],
                            PicNum = houseDetail.imagecount,
                            Kitchen = houseDetail.kitchen,
                            CityID = community.CityID ?? 0,
                            Distrctid = community.Distrctid ?? 0,
                            RegionID = community.RegionID,
                            CommunityID = community.CommunityID,
                            roomImages = (from room in houseDetail.roomphotourls.Split(',') where !String.IsNullOrEmpty(room) select room.Replace("120x120", "600x600")).ToArray(),
                            fangxingImages = (from room in houseDetail.housephotourls.Split(',') where !String.IsNullOrEmpty(room) select room.Replace("120x120", "600x600")).ToArray(),
                            outdoorImages = new string[0],
                            FitmentStatus = houseDetail.fitment
                        };
                        string buildType = houseDetail.purpose;
                        switch (buildType)
                        {
                            case "别墅":
                                {
                                    house.villaInfo = new VillaInfo
                                    {
                                        BasicEquip = houseDetail.baseservice,
                                        AdvEquip = houseDetail.baseservice,
                                    };
                                }
                                break;

                            case "写字楼":
                                {
                                    house.officeInfo = new OfficeInfo
                                    {
                                        BasicEquip = houseDetail.baseservice,
                                        Fee = houseDetail.propertyfee
                                    };
                                }
                                break;
                            case "商铺":
                                {
                                    house.shopInfo = new ShopInfo { BasicEquip = houseDetail.baseservice };
                                }
                                break;
                            default:
                                house.houseInfo = new HouseInfo
                                {
                                    BasicEquip = houseDetail.baseservice,
                                    AdvEquip = houseDetail.baseservice,
                                };
                                break;
                        }

                        houses.Add(house);
                    }
                }
            }

            return houses;


        }
        private static int? GetPrupose(int? buildType)
        {
            switch (buildType)
            {
                case 1:
                    return 0;
                case 2:
                    return 1;
                case 3:
                    return 2;
                case 4:
                    return 3;
                default:
                    return null;
            }
        }
        private static string GetMatch(string cookie, string match)
        {
            return Regex.Match(cookie, match).Groups[1].Value;
        }
        

    }
}