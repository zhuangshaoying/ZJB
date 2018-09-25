using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using HtmlAgilityPack;
using ZJB.Api.Entity;
using ZJB.Core.Utilities;


namespace ZJB.WX.Common.xms
{
    internal class Lister_ZJB
    {
        //常见的条数可能是40条
        private const string listAllApi = "http://my.ZJB.com/JJR/HouseInfoManage.aspx?OrderID=1&page=1";
        private const string listApi = "http://my.ZJB.com/JJR/HouseInfoManage.aspx?OrderID=1&page={0}&pt={1}";
        private const string detailApi = "http://my.ZJB.com/esf/HouseInfosAdd.aspx?ID={0}&TradeType={1}";

        public static string GetOldest(string cookie)
        {
            string ret = HttpUtility.GetString(listAllApi,
                Helper_ZJB.GetHeaders(cookie));
            Match m = Regex.Match(ret, "type=\"checkbox\" value=\"(\\d+)\"");
            return m.Success ? m.Groups[1].Value : null;
        }

        public static IEnumerable<ImportedHouse> GetHouseDetails(byte tradeType, string cookie, int size)
        {
            int postType = ConvertPostType(tradeType);
            List<string> ids = GetHouseIds(postType, cookie, size);
            return ids.Select(t => GetHouseDetail(tradeType, t, cookie)).Where(o=>o!=null).ToList();
        }

        /// <summary>
        ///     获取房源列表的id
        /// </summary>
        /// <param name="postType">0 出售 1 出租</param>
        /// <param name="cookie">Cookie</param>
        /// <param name="size">数量</param>
        /// <returns></returns>
        private static List<string> GetHouseIds(int postType, string cookie, int size)
        {
            var ids = new List<string>();
            for (int i = 1; i < 30; i++)
            {
                string ret = HttpUtility.GetString(string.Format(listApi, i, postType),
                    Helper_ZJB.GetHeaders(cookie));
                MatchCollection ms = Regex.Matches(ret, "HouseInfosAdd\\.aspx\\?ID=(\\d+)");
                if (ms.Count <1)
                {
                    break;
                }
                var newids = from Match m in ms where m.Success select m.Groups[1].Value;
                ids.AddRange(newids);
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
        /// <param name="id">id</param>
        /// <param name="cookie">Cookie</param>
        /// <returns></returns>
        private static ImportedHouse GetHouseDetail(byte tradeType, string id, string cookie)
        {
            const int siteId = 1;
            string html = HttpUtility.GetString(string.Format(detailApi, id,tradeType),
                Helper_ZJB.GetHeaders(cookie));
            var doc = new HtmlDocument();
            doc.LoadHtml(html);
            HtmlNode node = doc.DocumentNode;
            HtmlNode title_Node = node.SelectSingleNode("//input[@id='ContentRight_txtT_HouseInfos_Address']");
            HtmlNode xiaoqu_Node = node.SelectSingleNode("//input[@id='ContentRight_txtZoneName_1']");
            HtmlNode address_Node = node.SelectSingleNode("//input[@id='ContentRight_Txt_AderssDesc']");
            HtmlNode area_Node = node.SelectSingleNode("//input[@id='ContentRight_txtT_HouseInfos_BuildArea']");
            HtmlNode areaUsed_Node = node.SelectSingleNode("//input[@id='ContentRight_txtT_HouseInfos_UsedArea']");
            HtmlNode price_Node = node.SelectSingleNode("//input[@id='ContentRight_txtT_HouseInfos_Price']");
            HtmlNode floor_Node = node.SelectSingleNode("//input[@id='ContentRight_txtT_HouseInfos_CurFloor']");
            HtmlNode florrAll_Node = node.SelectSingleNode("//input[@id='ContentRight_txtT_HouseInfos_MaxFloor']");
            HtmlNode note_Node = node.SelectSingleNode("//textarea[@id='ContentRight_txtT_HouseInfos_Note']");

            HtmlNode year_Node = node.SelectSingleNode("//input[@id='ContentRight_txtUsedYear']");
            HtmlNode pointTo_Node =
                node.SelectSingleNode(
                    "//select[@id='ContentRight_ddlT_HouseInfos_PointTo']/option[@selected='selected']");
            HtmlNode room_Node =
                node.SelectSingleNode("//select[@id='ContentRight_ddl_Room']/option[@selected='selected']");
            HtmlNode hall_Node =
                node.SelectSingleNode("//select[@id='ContentRight_ddl_Hall']/option[@selected='selected']");
            HtmlNode toilet_Node =
                node.SelectSingleNode("//select[@id='ContentRight_ddl_Toilet']/option[@selected='selected']");
            HtmlNode balcony_Node =
                node.SelectSingleNode("//select[@id='ContentRight_ddl_Balcony']/option[@selected='selected']");
            HtmlNode fitment_Node =
                node.SelectSingleNode("//input[@name='ctl00$ContentRight$rblT_HouseInfos_FitmentStauts' and checked='checked']");
            HtmlNode buildType_Node =
               node.SelectSingleNode("//select[@id='ContentRight_ddlT_HouseInfos_BuildType']/option[@selected='selected']");
//            HtmlNode pointTo_Node = node.SelectSingleNode("//select[@id='']/option[@selected='selected']");
//            HtmlNode pointTo_Node = node.SelectSingleNode("//select[@id='']/option[@selected='selected']");
//            HtmlNode note_Node = node.SelectSingleNode("//textarea[@id='");
//            HtmlNode note_Node = node.SelectSingleNode("//textarea[@id='");
//            HtmlNode note_Node = node.SelectSingleNode("//textarea[@id='");

//            MatchCollection imageMatches = new Regex("createImgDiv\\('(.*?)'").Matches(html);
//            string payType = new Regex("\\$\\(\"#payType\"\\)\\.val\\(\"(.*?)\"").Match(html).Groups[1].Value;
//            string year = new Regex("\\$\\(\"#saleHouse-basic-year\"\\)\\.val\\('(.*?)'").Match(html).Groups[1].Value;
//            string pointTo = new Regex("name=pointTo\\]\\[value='(.*?)'").Match(html).Groups[1].Value;
//            string lookTime = new Regex("name=lookTime\\]\\[value='(.*?)'").Match(html).Groups[1].Value;
//            string fitmentStatus = new Regex("name=fitmentStatus\\]\\[value='(.*?)'").Match(html).Groups[1].Value;
            var basicEquip = node.SelectNodes("//span[@id='ContentRight_chklT_HouseInfos_BasicEquip']/input[@checked = 'checked']");
            var equip_Nodes = node.SelectNodes("//span[@id='ContentRight_chklT_HouseInfos_AdvEquip']/input[@checked = 'checked']");
            var otherEquip_Nodes = node.SelectNodes("//span[@id='ContentRight_chklT_HouseInfos_OverEquip']/input[@checked = 'checked']");
            string equip = "";
            if (basicEquip != null)
            {
                equip = basicEquip.Aggregate(equip, (current, equip_Node) => current + (equip_Node.NextSibling.InnerText + ","));
            }
            if (equip_Nodes != null)
            {
                equip = equip_Nodes.Aggregate(equip, (current, equip_Node) => current + (equip_Node.NextSibling.InnerText + ","));
            }
            if (otherEquip_Nodes != null)
            {
                equip = otherEquip_Nodes.Aggregate(equip, (current, equip_Node) => current + (equip_Node.NextSibling.InnerText + ","));
            }


            string district = new Regex("change\\(\"(\\d+)\"").Match(html).Groups[1].Value;
            string area = new Regex("change\\(\"\\d+\", \"(\\d+)\"").Match(html).Groups[1].Value;
            int districtId = PostDataHelper.RMappingAreaID(district, siteId, 592);
           // int areaId = PostDataHelper.RMappingAreaID(area, siteId, districtId);
            var picMatches = Regex.Matches(html, "delpic.*?pos=(.*?)&.*?spath=(.*?)'");
            string[] pics=(from Match picMatch in picMatches select picMatch.Groups[1].Value).ToArray();
//            List<string> tmp = ;
            string communityName = xiaoqu_Node.GetAttributeValue("value", "");
            
            var community = PostDataHelper.RMappingCommunityModel(communityName, siteId);
            int communityId=0;
            if (community == null) return null;

            communityId = community.CommunityID;
            if (communityId == 0)
            {
                return null;
            }
             List<string> tmp1 = new List<string>();
            List<string> tmp2 = new List<string>();
            List<string> tmp3 = new List<string>();
            int sum = 0;
            foreach (Match picMatch in picMatches)
            {
                sum ++;
                string pos = Uri.UnescapeDataString(picMatch.Groups[1].Value);
                string pic = "http://img.ZJB.com/Upload/InfoImage" + Uri.UnescapeDataString(picMatch.Groups[2].Value);
                if (pos.Contains("型"))
                {
                    tmp3.Add(pic);
                }
                else if (pos.Contains("外") || pos.Contains("区"))
                {
                    tmp2.Add(pic);
                }
                else
                {
                    tmp1.Add(pic);
                }

            }
            decimal con = 1;
            decimal buildAreaValue=shortParse(GetAttributeValue(area_Node));
            decimal usedAreaValue = shortParse(GetAttributeValue(areaUsed_Node));
            usedAreaValue = usedAreaValue == 0 ? (decimal)(buildAreaValue - con) : usedAreaValue;
            var house = new ImportedHouse
            {
                HouseID = id,
                CommunityID = communityId,
                CommunityName = communityName,
                CurFloor = intParse(GetAttributeValue(floor_Node)),
                MaxFloor = intParse(GetAttributeValue(florrAll_Node)),
                BuildArea = buildAreaValue,
                UsedArea = usedAreaValue,
                UsedYear = intParse(GetAttributeValue(year_Node)),
                Price = shortParse(GetAttributeValue(price_Node)),
                PriceUnit = tradeType == 1 ? "万" : "元/月",
                Address = GetAttributeValue(address_Node),
                Title = title_Node.GetAttributeValue("value", ""),
                Note = note_Node == null ? "" : System.Web.HttpUtility.HtmlDecode(note_Node.InnerHtml),
//                YiJuHua = GetAttributeValue(yijuhua_Node),
//                HouseLabel = GetAttributeValue(tags_Node),
//                CellLabel = GetAttributeValue(tese_Node),
                Distrctid = districtId,
                CityID = community.CityID??0,
                RegionID = community.RegionID,
                BuildType = 1,
//                FitmentStatus = fitmentStatus,
                PointTo = GetFocus(GetAttributeValue(pointTo_Node)),
//                PayType = payType,
                Status = 1,
//                LookHouseTime = lookTime,
                TradeType = tradeType,
//                imageUrls = (from Match imageMatch in imageMatches select imageMatch.Groups[1].Value).ToArray(),
                Balcony = byteParse(GetAttributeValue(balcony_Node)),
                Toilet = byteParse(GetAttributeValue(toilet_Node)),
//                Kitchen = byteParse(GetAttributeValue)),
                Room = byteParse(GetAttributeValue(room_Node)),
                Hall = byteParse(GetAttributeValue(hall_Node)),
                FitmentStatus = GetAttributeValue(fitment_Node),
                roomImages = tmp1.ToArray(),
                outdoorImages = tmp2.ToArray(),
                fangxingImages = tmp3.ToArray(),
                PicNum =(byte) sum
                
            };
            string buildType = buildType_Node.GetAttributeValue("value", "");
            switch (buildType)
            {
                case "9":
                    {
                        house.villaInfo = new VillaInfo { BasicEquip = equip , AdvEquip = equip};
                        house.BuildType = 2;
                    }
                    break;
                case "3":
                    {
                        house.factoryInfo = new FactoryInfo { BasicEquip = equip };
                        house.BuildType = 5;
                    }
                    break;
                case "2":
                    {
                        house.officeInfo = new OfficeInfo { BasicEquip = equip };
                        house.BuildType = 4;
                    }
                    break;
                case "4":
                case "5":
                    {
                        house.shopInfo = new ShopInfo { BasicEquip = equip };
                        house.BuildType = 3;
                    }
                    break;
                default:
                    house.houseInfo = new HouseInfo { BasicEquip = equip, AdvEquip = equip };
                    house.BuildType = 1;
                    break;
            }

            return house;
        }

        private static byte byteParse(string attributeValue)
        {
            if (attributeValue == null)
            {
                return 0;
            }
            Match m = Regex.Match(attributeValue, "(\\d+)");
            return !m.Success ? (byte) 0 : byte.Parse(m.Groups[1].Value);
        }

        private static int ConvertPostType(int tradeType)
        {
            switch (tradeType)
            {
                case 1:
                    return 4;
                case 3:
                    return 1;
            }
            return 0;
        }

        private static string GetFocus(string pointTo)
        {
            switch (pointTo)
            {
                case "1": return "东";
                case "2": return "南";
                case "3": return "西";
                case "4": return "北";
                case "8": return "南北";
                case "7": return "东西";
                case "5": return "东南";
                case "6": return "西南";
                case "9": return "东北";
                case "10": return "西北";
                default:
                    return "东";
            }
        }

        private static decimal shortParse(string attributeValue)
        {
            if (attributeValue == null)
            {
                return 0;
            }
            Match m = Regex.Match(attributeValue, "(\\d+)");
            return !m.Success ? (decimal)0 : decimal.Parse(m.Groups[1].Value);
        }
        private static int intParse(string attributeValue)
        {
            if (attributeValue == null)
            {
                return 0;
            }
            Match m = Regex.Match(attributeValue, "(\\d+)");
            return !m.Success ? (int)0 : int.Parse(m.Groups[1].Value);
        }
        private static string GetAttributeValue(HtmlNode node)
        {
            return node == null ? null : node.GetAttributeValue("value", "");
        }
    }
}