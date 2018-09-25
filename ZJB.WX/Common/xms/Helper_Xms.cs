
using System.Text.RegularExpressions;
using HtmlAgilityPack;
using ZJB.Api.Entity;

namespace ZJB.WX.Common.xms
{
    class Helper_Xms
    {
        public static ImportedHouse GetShopInfo(string html, ImportedHouse house)
        {
            /*
             setShop('商业街商铺','闲置中',
				'百货超市,家居建材,服饰鞋包,生活服务,美容美发,餐饮美食,休闲娱乐,其他','1','1','客梯,货梯,扶梯,空调,水','元/月');
            function setShop(shopType,shopStatus,targetField,hasFee,divide,basicEquip,priceType)
             */
            Match houseRegex = new Regex(
                "setShop\\('(?<shopType>.*?)','(?<shopStatus>.*?)'," +
                "'(?<targetField>.*?)','(?<hasFee>.*?)','(?<divide>.*?)','(?<basicEquip>.*?)','(?<priceType>.*?)'")
                .Match(html.Trim().Replace("\r", "").Replace("\n", "").Replace("\t", "").Replace("\t", ""));
            string shopType = houseRegex.Groups["shopType"].Value;
            string shopStatus = houseRegex.Groups["shopStatus"].Value;
            string basicEquip = houseRegex.Groups["basicEquip"].Value;
            string divide = houseRegex.Groups["divide"].Value;
            string targetField = houseRegex.Groups["targetField"].Value;
            string hasFee = houseRegex.Groups["hasFee"].Value;


            house.shopInfo = new ShopInfo
            {
                ShopType = shopType,
                ShopStatus = shopStatus,
                BasicEquip = basicEquip,
                Divide = (divide == "1"),
                TargetField = targetField,
                Fee = decimal.Parse(hasFee)
            };
            return house;
        }

        public static ImportedHouse GetOfficeInfo(string html, ImportedHouse house)
        {
            //function setOffice(officeType,officeLeve,hasFee,divide,basicEquip,priceType)
            //setOffice('纯写字楼','甲级','0',
            //				'1','水,电,煤气/天然气,暖气,电梯,车位,露台,阁楼,电话,彩电,热水器,洗衣机,冰箱,空调,家具,宽带网','万');
            Match houseRegex = new Regex(
                "setOffice\\('(?<officeType>.*?)','(?<officeLeve>.*?)','(?<hasFee>\\d+)'," +
                "'(?<divide>.*?)','(?<basicEquip>.*?)','(?<priceType>.*?)'").Match(
                    html.Trim().Replace("\r", "").Replace("\n", "").Replace("\t", ""));
            string officeType = houseRegex.Groups["officeType"].Value;
            string officeLeve = houseRegex.Groups["officeLeve"].Value;
            string hasFee = houseRegex.Groups["hasFee"].Value;
            string divide = houseRegex.Groups["divide"].Value;
            string basicEquip = houseRegex.Groups["basicEquip"].Value;
            //            string priceType = houseRegex.Groups["priceType"].Value;


            house.officeInfo = new OfficeInfo
            {
                OfficeType = officeType,
                OfficeLevel = officeLeve,
                BasicEquip = basicEquip,
                Divide = (divide == "1"),
                Fee = decimal.Parse(hasFee)
            };
            return house;
        }

        public static ImportedHouse GetFactoryInfo(string html, ImportedHouse house)
        {
            //function setFactory(factoryType, basicEquip, priceType) 
            Match houseRegex = new Regex(
                "setFactory\\('(?<factoryType>.*?)','(?<basicEquip>.*?)','(?<priceType>.*?)'").Match(
                    html.Trim().Replace("\r", "").Replace("\n", "").Replace("\t", ""));
            string factoryType = houseRegex.Groups["factoryType"].Value;
            string basicEquip = houseRegex.Groups["basicEquip"].Value;
            house.factoryInfo = new FactoryInfo
            {
                FactoryType = factoryType,
                BasicEquip = basicEquip,
            };
            return house;
        }

        public static ImportedHouse GetHouseInfo(string html, ImportedHouse house)
        {
            /*
             setHouse(houseType,room,hall,toilet,kitchen,balcony,advEquip,houseSubType,
                houseProperty,landYear,houseStructure,fiveYears,basicEquip,onlyHouse)
             
             setHouse(
                '普通住宅','3','1','1','1','1',
                '电话,热水器,彩电,空调,冰箱,洗衣机,家具,床,宽带网,微波炉,衣柜,沙发,厨具,独立卫生间,水,电,煤气/天然气,有线电视','多层[1-7层]','商品房','70年产权','板楼',
                '1','水,电,煤气/天然气,有线电视,电话,热水器,彩电,空调,冰箱,洗衣机,家具,床,宽带网,微波炉,衣柜,沙发,厨具,独立卫生间','1');
             */


            Match houseRegex = new Regex(
                "setHouse\\('(?<houseType>.*?)','(?<room>\\d+)','(?<hall>\\d+)'" +
                ",'(?<toilet>\\d+)','(?<kitchen>\\d+)','(?<balcony>\\d+)'," +
                "'(?<advEquip>.*?)','(?<houseSubType>.*?)','(?<houseProperty>.*?)','(?<landYear>.*?)','(?<houseStructure>.*?)'," +
                "'(?<fiveYears>.*?)','(?<basicEquip>.*?)','(?<onlyHouse>.*?)'").Match(
                    html.Trim().Replace("\r", "").Replace("\n", "").Replace("\t", ""));
            string houseType = houseRegex.Groups["houseType"].Value;
            string advEquip = houseRegex.Groups["advEquip"].Value;
            string houseSubType = houseRegex.Groups["houseSubType"].Value;
            string houseProperty = houseRegex.Groups["houseProperty"].Value;
            string landYear = houseRegex.Groups["landYear"].Value;
            string houseStructure = houseRegex.Groups["houseStructure"].Value;
            string fiveYears = houseRegex.Groups["fiveYears"].Value;
            string basicEquip = houseRegex.Groups["basicEquip"].Value;
            string onlyHouse = houseRegex.Groups["onlyHouse"].Value;
            string balcony = houseRegex.Groups["balcony"].Value;
            string toilet = houseRegex.Groups["toilet"].Value;
            string kitchen = houseRegex.Groups["kitchen"].Value;
            string hall = houseRegex.Groups["hall"].Value;
            string room = houseRegex.Groups["room"].Value;
            house.Balcony = byteParse(balcony);
            house.Toilet = byteParse(toilet);
            house.Kitchen = byteParse(kitchen);
            house.Room = byteParse(room);
            house.Hall = byteParse(hall);
            house.houseInfo = new HouseInfo
            {
                AdvEquip = advEquip,
                BasicEquip = basicEquip,
                FiveYears = (fiveYears == "1"),
                OnlyHouse = (onlyHouse == "1"),
                LandYear = landYear,
                HouseSubType = houseSubType,
                HouseProperty = houseProperty,
                HouseType = houseType,
                HouseStructure = houseStructure
            };
            return house;
        }

        public static ImportedHouse GetVillaInfo(string html, ImportedHouse house)
        {
            /*
             
             	
		setVilla('独栋','平层',
				'7','3','4','1','4',
				'0','0','0','0','水,电,煤气/天然气,电梯,露台,阁楼,电话',
				'水,电,煤气/天然气,电梯,露台,阁楼,电话','70年产权','1','0');
             function setVilla(villaType,hallType,room,hall,toilet,kitchen,balcony,
		        basement,garden,garage,parkLot,basicEquip,advEquip,landYear,fiveYears,onlyHouse) 
             */
            Match villaRegex = new Regex(
                "setVilla\\('(?<villaType>.*?)','(?<hallType>.*?)'," +
                "'(?<room>\\d+)','(?<hall>\\d+)','(?<toilet>\\d+)','(?<kitchen>\\d+)','(?<balcony>\\d+)'," +
                "'(?<basement>.*?)','(?<garden>.*?)','(?<garage>.*?)','(?<parkLot>.*?)','(?<basicEquip>.*?)'," +
                "'(?<advEquip>.*?)','(?<landYear>.*?)','(?<fiveYears>.*?)','(?<onlyHouse>.*?)'").Match(
                    html.Trim().Replace("\r", "").Replace("\n", "").Replace("\t", ""));
            string villaType = villaRegex.Groups["villaType"].Value;
            string hallType = villaRegex.Groups["hallType"].Value;
            string basement = villaRegex.Groups["basement"].Value;
            string garden = villaRegex.Groups["garden"].Value;
            string garage = villaRegex.Groups["garage"].Value;
            string parkLot = villaRegex.Groups["parkLot"].Value;
            string basicEquip = villaRegex.Groups["basicEquip"].Value;
            string advEquip = villaRegex.Groups["advEquip"].Value;
            string landYear = villaRegex.Groups["landYear"].Value;
            string fiveYears = villaRegex.Groups["fiveYears"].Value;
            string onlyHouse = villaRegex.Groups["onlyHouse"].Value;
            string balcony = villaRegex.Groups["balcony"].Value;
            string toilet = villaRegex.Groups["toilet"].Value;
            string kitchen = villaRegex.Groups["kitchen"].Value;
            string hall = villaRegex.Groups["hall"].Value;
            string room = villaRegex.Groups["room"].Value;
            house.Balcony = byteParse(balcony);
            house.Toilet = byteParse(toilet);
            house.Kitchen = byteParse(kitchen);
            house.Room = byteParse(room);
            house.Hall = byteParse(hall);
            house.villaInfo = new VillaInfo
            {
                AdvEquip = advEquip,
                BasicEquip = basicEquip,
                FiveYears = (fiveYears == "1"),
                OnlyHouse = (onlyHouse == "1"),
                LandYear = landYear,
                Garage = (garage == "1"),
                Garden = (garden == "1"),
                Basement = (basement == "1"),
                ParkLot = (parkLot == "1"),
                HallType = hallType,
                VillaType = villaType,
                //                HouseStructure = houseStructure
            };
            return house;
        }

        public static short shortParse(string attributeValue)
        {
            if (attributeValue == null)
            {
                return 0;
            }
            Match m = Regex.Match(attributeValue, "(\\d+)");
            return !m.Success ? (short)0 : short.Parse(m.Groups[1].Value);
        }

        public static byte byteParse(string attributeValue)
        {
            if (attributeValue == null)
            {
                return 0;
            }
            Match m = Regex.Match(attributeValue, "(\\d+)");
            return !m.Success ? (byte)0 : byte.Parse(m.Groups[1].Value);
        }

        public static string GetAttributeValue(HtmlNode node)
        {
            return node == null ? null : node.GetAttributeValue("value", "");
        }
    }
}
