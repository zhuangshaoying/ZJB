using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MongoDB.Bson.Serialization.Attributes;

namespace ZJB.Api.Models
{
    public class HouseCollectViewLog
    {
       [BsonId]
       ///采集过来的房源id
       public string id { get; set; }
        //经纪人阅读记录
       public List<Agent> Agents{ get; set;}
       public string AddTime { get; set; }

    }
    public class Agent
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }

    #region HouseCollectInfo
    public class HouseCollectInfo
    {
        private readonly string _address;
        private readonly int _balcony;
        private readonly string _buildArea;
        private readonly int _buildingType;
        private readonly string _communityName;
        private readonly int _curFloor;
        private readonly int _cityId;
        private readonly int _distrctid;
        private readonly string _distrctName;
        private readonly int _hall;
        private readonly string _id;
        private readonly int _kitchen;
        private readonly int _maxFloor;
        private readonly int _picNum;
        private readonly double _price;
        private readonly string _priceUnit;
        private readonly string _publisher;
        private readonly int _regionId;
        private readonly string _regionName;
        private readonly DateTime? _releaseTime;
        private readonly string _addDate;
        private readonly int _room;
        private readonly string _source;
        private readonly string _tel;
        private readonly string _title;
        private readonly int _toilet;
        private readonly int _tradeType;
        private readonly DateTime? _updateTime;
        private readonly string _url;
        private readonly int _houseType;
        private readonly int _viewCount;
        private readonly int _isRead;
        private readonly int _isJjr;

        public string Address
        {
            get { return _address; }
        }

        public int Balcony
        {
            get { return _balcony; }
        }

        public string BuildArea
        {
            get { return _buildArea; }
        }

        public int BuildingType
        {
            get { return _buildingType; }
        }

        public string CommunityName
        {
            get { return _communityName; }
        }

        public int CurFloor
        {
            get { return _curFloor; }
        }

        public int CityID
        {
            get { return _cityId; }
        }

        public int Distrctid
        {
            get { return _distrctid; }
        }

        public string DistrctName
        {
            get { return _distrctName; }
        }

        public int Hall
        {
            get { return _hall; }
        }

        public string Id
        {
            get { return _id; }
        }

        public int Kitchen
        {
            get { return _kitchen; }
        }

        public int MaxFloor
        {
            get { return _maxFloor; }
        }

        public int PicNum
        {
            get { return _picNum; }
        }

        public double Price
        {
            get { return _price; }
        }

        public string PriceUnit
        {
            get { return _priceUnit; }
        }

        public string Publisher
        {
            get { return _publisher; }
        }

        public int RegionID
        {
            get { return _regionId; }
        }

        public string RegionName
        {
            get { return _regionName; }
        }

        public DateTime? ReleaseTime
        {
            get { return _releaseTime; }
        }

        public string AddDate
        {
            get { return _addDate; }
        }

        public int Room
        {
            get { return _room; }
        }

        public string Source
        {
            get { return _source; }
        }

        public string Tel
        {
            get { return _tel; }
        }

        public string Title
        {
            get { return _title; }
        }

        public int Toilet
        {
            get { return _toilet; }
        }

        public int TradeType
        {
            get { return _tradeType; }
        }

        public DateTime? UpdateTime
        {
            get { return _updateTime; }
        }

        public string Url
        {
            get { return _url; }
        }

        public int houseType
        {
            get { return _houseType; }
        }

        public int viewCount
        {
            get { return _viewCount; }
        }

        public int isRead
        {
            get { return _isRead; }
        }

        public int isJjr
        {
            get { return _isJjr; }
        }

        public HouseCollectInfo(string address, int balcony, string buildArea, int buildingType, string communityName, int curFloor, int cityId, int distrctid, string distrctName, int hall, string id, int kitchen, int maxFloor, int picNum, double price, string priceUnit, string publisher, int regionId, string regionName, DateTime? releaseTime, string addDate, int room, string source, string tel, string title, int toilet, int tradeType, DateTime? updateTime, string url, int houseType, int viewCount, int isRead, int isJjr)
        {
            _address = address;
            _balcony = balcony;
            _buildArea = buildArea;
            _buildingType = buildingType;
            _communityName = communityName;
            _curFloor = curFloor;
            _cityId = cityId;
            _distrctid = distrctid;
            _distrctName = distrctName;
            _hall = hall;
            _id = id;
            _kitchen = kitchen;
            _maxFloor = maxFloor;
            _picNum = picNum;
            _price = price;
            _priceUnit = priceUnit;
            _publisher = publisher;
            _regionId = regionId;
            _regionName = regionName;
            _releaseTime = releaseTime;
            _addDate = addDate;
            _room = room;
            _source = source;
            _tel = tel;
            _title = title;
            _toilet = toilet;
            _tradeType = tradeType;
            _updateTime = updateTime;
            _url = url;
            _houseType = houseType;
            _viewCount = viewCount;
            _isRead = isRead;
            _isJjr = isJjr;
        }
    }
     #endregion

}
