using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ZJB.Api.BLL;
using ZJB.Api.Models;
using ZJB.Core.Injection;

namespace ZJB.WX.Controllers
{
    public class CommunityController : BaseController
    {
        //
        // GET: /Community/

        private NCBaseRule ncRule = Container.Instance.Resolve<NCBaseRule>();
        private HouseBll houseBll = new HouseBll();
        public ActionResult Index()
        {
            var results = ncRule.CurrentEntities.Community
              .Where(p =>  p.CityID == 592).OrderByDescending(P => P.Recommend).ThenByDescending(p => p.AddDate)
              .Take(10).ToList();

            var regionPrice = ncRule.CurrentEntities.RegionPrice.ToList();
            ViewData["regionPrice"] = regionPrice;
            return View(results);
        }
        public ActionResult List()
        {
            var results = ncRule.CurrentEntities.Community
              .Where(p => p.CityID == 592).OrderByDescending(P => P.Recommend).ThenByDescending(p=>p.AddDate)
              .Take(10).ToList();

            var regionPrice = ncRule.CurrentEntities.RegionPrice.ToList();
            ViewData["regionPrice"] = regionPrice;
            return View(results);
        }
        public ActionResult Search()
        {
            return View();
        }
        public ActionResult SearchByName(string name)
        {
            var results = ncRule.CurrentEntities.Community
                .Where(p => p.Name.Contains(name) && p.CityID==592)
                .Select(p => new
                {
                    house_id = p.CommunityID,
                    house_name = p.Name
                })
                .Take(20).ToList();
            return Json(results, JsonRequestBehavior.AllowGet);
            //var results = ncRule.CurrentEntities.Community
            //    .Where(p=>p.Name.Contains(name))
            //    .Take(20).ToList();
            //return Json(new
            //{
            //    data = results,
            //    totalSize = results.Count
            //}, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Detail(int communityId=0)
        {
            var data = ncRule.CurrentEntities.Community.FirstOrDefault(p => p.CommunityID == communityId);

            if (data == null)
            {
                return Content("小区不存在！");
            }
            this.ViewBag.Houses = GetEsf(communityId, 10);

            return View(data);
        }
        private List<List<HouseBasicInfoModel>> GetEsf(int communityId, int pageSize = 10)
        {
            List<List<HouseBasicInfoModel>> result = new List<List<HouseBasicInfoModel>>();
            int count = 0;
            for (int k = 0; k < 8; k++)
            {
                var houseList = (from hb in ncRule.CurrentEntities.HouseBasicInfo
                        where hb.CommunityID == communityId && hb.Status==1&&hb.TradeType==1
                        select new HouseBasicInfoModel
                        {
                            HouseID=hb.HouseID,
                            CommunityID = hb.HouseID,
                            Title=hb.Title,
                            Room=hb.Room,
                            Hall=hb.Hall,
                            Toilet =hb.Toilet,
                            HouseImgPath = hb.HouseImgPath,
                            BuildArea=hb.BuildArea,
                            Price=hb.Price,
                            UnitPrice=hb.UnitPrice,
                        }
                 );
                if (k > 0)
                {
                    houseList = houseList.Where(p => p.Room == k);
                }
                var item = houseList.Take(pageSize).ToList();
                count+= item.Count;
                if (k==0 || item.Count>0)
                {
                    result.Add(item);
                }
            }
            if (count == 0)
            {
                result = null;
            }
            return result;
        }

        private ActionResult GetEsfOld()
        {
                int tradetype = 0; int buildtype = 0; int status = 1; string cell = ""; int sort = 7; int houseId = 0;
                string title = ""; string tags = ""; int pageIndex = 1; int pageSize = 10; int userId = 0; int regionid = 0; int districtid = 0; int room = 0;
                int minprice = 0; int maxprice = 0;
                HouseListReq parames = new HouseListReq();
                parames.postType = tradetype;
                parames.buildingType = buildtype;
                parames.buildingStatus = status;
                parames.cell = cell;
                parames.sort = sort;
                parames.houseId = houseId;
                parames.title = title;
                parames.tags = tags;
                parames.page = pageIndex;
                parames.pageSize = pageSize;
                parames.userId = userId;
                parames.roomType = room;
                parames.districtId = districtid;
                parames.regionId = regionid;
                parames.maxPrice = maxprice;
                parames.minPrice = minprice;
                int totalSize = 0;
                var list = houseBll.GetEsfHouseList(parames, ref totalSize);
                return JsonReturnValue(new { totalSize = totalSize, list = list }, JsonRequestBehavior.AllowGet);
            }
        }
}
