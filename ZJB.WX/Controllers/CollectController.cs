using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MongoDB.Driver;
using MongoDB.Driver.Builders;
using ZJB.Api.Models;
using ZJB.Core.Utilities;

namespace ZJB.WX.Controllers
{
    public class CollectController : Controller
    {

        private string mongoserver = ConfigUtility.GetValue("MongoDbHouseCaijiConnection");// "mongodb://192.168.0.180";
        public ActionResult Index(int pageIndex=1, int pageSize=10, int cityId = 592)
        {

            return View();

        }

        public ActionResult GetHouseList(int pageIndex = 1, int pageSize = 10, int cityId = 592)
        {
            var client = new MongoClient(mongoserver);
            var server = client.GetServer();
            var database = server.GetDatabase("HeZi");
            var collection = database.GetCollection<HouseCrawler>("Chushou_592");

            IMongoQuery query = null;

            //query =
            //    Query.And(
            //       Query<HouseCrawler>.EQ(s => s.CityID, cityId)
            //        );

            IMongoSortBy sortBy = SortBy<HouseCrawler>.Descending(l => l.ReleaseTime);
            var houseList = collection.Find(query).SetSortOrder(sortBy).
                 SetSkip((pageIndex - 1) * pageSize).SetLimit(pageSize).Select(
                   c => new
                   {
                       id = c.Id.ToString(),
                       c.Address

                   }).ToList();


            return Json(houseList, JsonRequestBehavior.AllowGet);
        }

    }
}
