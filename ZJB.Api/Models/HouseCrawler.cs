using System;
using MongoDB.Bson.Serialization.Attributes;

namespace ZJB.Api.Models
{
    public class HouseCrawler
    {
        public HouseCrawler()
        {
        }

        public HouseCrawler(string id, string title, string url, string publisher, string tel, DateTime releaseTime,
            DateTime updateTime, string address, int cityId, string distrctName, int distrctid, string regionName,
            int regionId, string communityName, int tradeType, int buildingType, int room, int hall, int kitchen,
            int toilet, int balcony, double price, string priceUnit, int curFloor, int maxFloor, string buildArea,
            int picNum, string source, Agents[] agents)
        {
            Id = id;
            Title = title;
            Url = url;
            Publisher = publisher;
            Tel = tel;
            ReleaseTime = releaseTime;
            UpdateTime = updateTime;
            Address = address;
            CityID = cityId;
            DistrctName = distrctName;
            Distrctid = distrctid;
            RegionName = regionName;
            RegionID = regionId;
            CommunityName = communityName;
            TradeType = tradeType;
            BuildingType = buildingType;
            Room = room;
            Hall = hall;
            Kitchen = kitchen;
            Toilet = toilet;
            Balcony = balcony;
            Price = price;
            PriceUnit = priceUnit;
            CurFloor = curFloor;
            MaxFloor = maxFloor;
            BuildArea = buildArea;
            PicNum = picNum;
            Source = source;
            Agents = agents;
        }


        [BsonId]
        public string Id { get; set; }

        public string Title { get; set; }
        public string Url { get; set; }
        public string Publisher { get; set; }
        public string Tel { get; set; }
        public DateTime? ReleaseTime { get; set; }
        public DateTime? UpdateTime { get; set; }
        public string Address { get; set; }
        public int CityID { get; set; }
        public string DistrctName { get; set; }
        public int Distrctid { get; set; }
        public string RegionName { get; set; }
        public int RegionID { get; set; }
        public string CommunityName { get; set; }
        public int TradeType { get; set; }
        public int BuildingType { get; set; }
        public int Room { get; set; }
        public int Hall { get; set; }
        public int Kitchen { get; set; }
        public int Toilet { get; set; }
        public int Balcony { get; set; }
        public double Price { get; set; }
        public string PriceUnit { get; set; }
        public int? CurFloor { get; set; }
        public int? MaxFloor { get; set; }
        public string BuildArea { get; set; }
        public int? PicNum { get; set; }
        public string Source { get; set; }
        public Agents[] Agents { get; set; }
        [BsonIgnoreIfNull]
        public PicInfo[] Pics { get; set; }
    }

    public class Agents
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }
    public class PicInfo
    {
        public string Url { get; set; }
    }

    //主键为关键词+城市id拼接，方便插入。关键词和城市id分别有单独的字段，方便查询
    public class HouseCrawlerKeyword
    {
        public HouseCrawlerKeyword(string id, string keyword,int tradeType, int cityId, int[] userIds)
        {
            this.id = id;
            this.keyword = keyword;
            this.cityId = cityId;
            this.tradeType = tradeType;
            user_ids = userIds;
        }

        [BsonId]
        public string id { get; set; }//瑞景公园_592
        public int tradeType { get; set; }//1  （租售类型）
        public string keyword { get; set; }//瑞景公园
        public int cityId { get; set; }//592
        public int[] user_ids { get; set; }//
    }

}