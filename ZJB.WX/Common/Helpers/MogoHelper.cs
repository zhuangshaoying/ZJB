using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Builders;
using ZJB.WX.Common.xms;

namespace ZJB.WX.Common
{
    public class MogoHelper
    {
        const string connectionName = "MongoDbHouseCaijiConnection";
        const string databaseName = "HeZi";
        System.Web.Script.Serialization.JavaScriptSerializer jss = new System.Web.Script.Serialization.JavaScriptSerializer();
        public virtual IEnumerable<ImportedHouse> GetImportHouseList(ImportedHouseListReq parames, ref int totalSize)
        {
            MongoServer houseCaijiMongoDbServer = GetMongoClient(connectionName);
            MongoDatabase houseDb = houseCaijiMongoDbServer.GetDatabase(databaseName);
            string dbCollection = "ImportHouse";
            MongoCollection<BsonDocument> houseCollection = houseDb.GetCollection(dbCollection);
            IMongoQuery query = new QueryDocument();
            IMongoQuery userQuery =Query.And(Query.EQ("UserID", parames.UserId));
            if (parames.status >= 0) {
                userQuery = Query.And(userQuery,Query.EQ("MoveStatus", parames.status));
            }
            query = Query.And(query, Query.EQ("SiteId", parames.SiteId));
            query = Query.And(query, Query.ElemMatch("RefUser", userQuery));
            query = Query.And(query, Query.EQ("SiteUserName", parames.SiteUserName));
            if (parames.CommunityId > 0) { query = Query.And(query, Query.EQ("CommunityID", parames.CommunityId)); }
            if (parames.BuildingType > 0) { query = Query.And(query, Query.EQ("BuildType", parames.BuildingType)); }
            if (parames.TradeType > 0) { query = Query.And(query, Query.EQ("TradeType", parames.TradeType)); }
            IMongoSortBy sort = new SortByDocument();
            sort = SortBy.Descending("PostTime");
            MongoCursor<ImportedHouse> dataList = houseCollection.FindAs<ImportedHouse>(query).SetSkip((parames.PageIndex - 1) * parames.PageSize).SetSortOrder(sort).SetLimit(parames.PageSize);
            totalSize = (int)dataList.Count(); 
            return dataList;
        }
        public virtual void AddImportHouse(IEnumerable<ImportedHouse> houseList,int siteId,int userId, string id_Prefix,string siteUserName)
        {
            MongoServer houseCaijiMongoDbServer = GetMongoClient(connectionName);
            MongoDatabase houseDb = houseCaijiMongoDbServer.GetDatabase(databaseName);
            string dbCollection = "ImportHouse";
            MongoCollection<BsonDocument> houseCollection = houseDb.GetCollection(dbCollection);

        

            var query = Query.EQ("RefUser.UserID", userId);
            var update = Update.Pull("RefUser", Query.EQ("UserID", userId));
            houseCollection.Update(query, update, UpdateFlags.Multi);

            foreach (ImportedHouse item in houseList)
            {
                string houseId=item.HouseID;
                item.MoveStatus = 0;
                item.HouseID = id_Prefix + "_" + item.HouseID;
                item.SiteUserName = siteUserName;
                item.SiteId = siteId;
                if (item.houseInfo != null)
                {
                    item.BuildType = 1;
                }
                else if (item.villaInfo != null)
                {
                    item.BuildType = 2;
                }
                else if (item.shopInfo != null)
                {
                    item.BuildType = 3;
                }
                else if (item.officeInfo != null)
                {
                    item.BuildType = 4;
                }
                else if (item.factoryInfo != null)
                {
                    item.BuildType = 5;
                }
                ImportedHouse oldHouse = houseCollection.FindOneAs<ImportedHouse>(Query.EQ("_id", item.HouseID));
                if (oldHouse != null)
                {
                    int moveHouseId = 0;
                    int moveStatus = 0;
                    if (oldHouse.RefUser == null)
                    {
                        item.RefUser = new List<ImportedHouseRefUser>();
                    }
                    else {
                        item.RefUser = oldHouse.RefUser;
                    }
                    ImportedHouseRefUser thisRefUser = item.RefUser.Where(u => u.UserID == userId).FirstOrDefault();
                    if (thisRefUser != null)
                    {
                        moveHouseId = thisRefUser.MoveHouseId;
                        moveStatus = thisRefUser.MoveStatus;
                    }
                    else if (oldHouse.UserID == userId)
                    {
                        moveHouseId = oldHouse.MoveHouseId;
                        moveStatus = oldHouse.MoveStatus;
                    }
                    //oldHouse.MoveStatus = 0;
                   // oldHouse.UserID = 0;
                   // oldHouse.MoveHouseId = 0;
                   // oldHouse.RefUser = null;
                    //string oldStr = ZJB.Core.Utilities.StringUtility.ToMd5String(jss.Serialize(oldHouse));
                    // string newStr = ZJB.Core.Utilities.StringUtility.ToMd5String(jss.Serialize(item));
                    //if (oldStr != newStr)
                    //{
                    if (thisRefUser != null)
                    {
                        thisRefUser.MoveStatus = moveStatus;
                        thisRefUser.MoveHouseId = moveHouseId;
                    }
                    else
                    {
                        thisRefUser = new ImportedHouseRefUser()
                        {
                             UserID=userId,
                             MoveStatus = moveStatus,
                             MoveHouseId = moveHouseId
                        };
                        item.RefUser.Add(thisRefUser);
                    }
                    houseCollection.Save(item);
                    //  }
                }
                else
                {
                    List<ImportedHouseRefUser> refUserList = new List<ImportedHouseRefUser>();
                    refUserList.Add(new ImportedHouseRefUser()
                    {
                        MoveHouseId = 0,
                        UserID = userId,
                        MoveStatus = 0
                    });
                    item.RefUser = refUserList;
                    houseCollection.Save(item);
                }
            }
        }
        private  MongoServer GetMongoClient(string connectName)
        {
            return new MongoClient(System.Configuration.ConfigurationManager.AppSettings[connectName]).GetServer();

        }
        public virtual IEnumerable<ImportedHouse> GetImportHouseListByIds(List<string> ids,int userId)
        {
            MongoServer houseCaijiMongoDbServer = GetMongoClient(connectionName);
            MongoDatabase houseDb = houseCaijiMongoDbServer.GetDatabase(databaseName);
            string dbCollection = "ImportHouse";
            MongoCollection<BsonDocument> houseCollection = houseDb.GetCollection(dbCollection);
            IMongoQuery query = new QueryDocument();
            query = Query.And(query, Query.EQ("RefUser.UserID", userId));
            query = Query.And(query, Query.In("_id", new BsonArray(ids)));
            IEnumerable<ImportedHouse> dataList = houseCollection.FindAs<ImportedHouse>(query);
            return dataList;
        }
        public virtual void SaveImportHouse(ImportedHouse house)
        {
            MongoServer houseCaijiMongoDbServer = GetMongoClient(connectionName);
            MongoDatabase houseDb = houseCaijiMongoDbServer.GetDatabase(databaseName);
            string dbCollection = "ImportHouse";
            MongoCollection<BsonDocument> houseCollection = houseDb.GetCollection(dbCollection);
            houseCollection.Save(house);
        }
    }
}