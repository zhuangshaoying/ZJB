using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Newtonsoft.Json;
using ZJB.Api.BLL;
using ZJB.Api.Entity;
using ZJB.Api.Models;
using ZJB.Core.Utilities;
using ZJB.WX.Filters;
using ZJB.WX.Models;
using ZJB.Web.Utilities;

namespace ZJB.WX.Controllers
{
    [Authorization]
    public class NoticeController : Controller
    {
        //
        // GET: /Notice/
        private NCBaseRule ncBase = new NCBaseRule();
        private NoticeBll noticeBll = new NoticeBll();
        public ActionResult Index()
        {
            return View();
        }
        /// <summary>
        /// 公告详细界面
        /// </summary>
        /// <param name="noticId"></param>
        /// <returns></returns>
        public ActionResult DetailView(int noticeId)
        {
            Notice noticeDetail = ncBase.CurrentEntities.Notice.Where(n => n.NoticeId == noticeId).FirstOrDefault();
            if (noticeDetail != null)
            {
                noticeBll.NoticeSetIsRead(noticeId, this.GetLoginUser().UserID);
                return View(noticeDetail);
            }
            return View(new Notice());
        }
        [AllowAnonymous]
        public ActionResult PhoneView(int noticeId)
        {
            Notice noticeDetail = ncBase.CurrentEntities.Notice.Where(n => n.NoticeId == noticeId).FirstOrDefault();
            if (noticeDetail != null)
            {
                if (this.GetLoginUser().IsNoNull())
                noticeBll.NoticeSetIsRead(noticeId, this.GetLoginUser().UserID);
                return View(noticeDetail);
            }
            return View(new Notice());
        }
        /// <summary>
        /// 公告列表
        /// </summary>
        /// <param name="type"></param>
        /// <param name="pi"></param>
        /// <param name="ps"></param>
        /// <returns></returns>
        public ActionResult NoticeList(int type = 1, int pi = 1, int ps = 10)
        {
            int totalSize = 0;
            List<Notice> noticeList = new List<Notice>();
            if (type == -1)///未读消息
            {
                noticeList=noticeBll.GetNotReadNoticeList(new GetNotReadNoticeListReq()
                {
                    pi = pi,
                    ps = ps,
                    userId = this.GetLoginUser().UserID
                }, ref totalSize);
               #region 将所有的未读消息更改为已读?
                //if (totalSize > 0)
                //{
 
                //}
               #endregion 
            }
           else if (type == 0)//私人消息
            {
                noticeList = noticeBll.GetNoticeList(new GetNoticeListReq()
                {
                    type=type,
                    pi = pi,
                    ps = ps,
                    userId = this.GetLoginUser().UserID
                }, ref totalSize);
            }
            else
            {
                noticeList = ncBase.CurrentEntities.Notice
                    .Where(n => n.Type == type)
                    .OrderByDescending(n => n.CreateTime)
                    .Skip((pi - 1) * ps)
                    .Take(ps).ToList();

                totalSize = ncBase.CurrentEntities.Notice
                    .Where(n => n.Type == type && n.Type != 0).ToList().Count;
            }
            ViewBag.TotalSize = totalSize;
            ViewBag.pi = pi;
            ViewBag.ps = ps;
            ViewBag.type = type;
            if (noticeList != null)
                return View(noticeList);
            return View(new List<Notice>());
        }
        /// <summary>
        /// 首页公告
        /// </summary>
        /// <returns></returns>
        public ActionResult HomeNoticeListView(int ps = 5)
        {
            DateTime dt = DateTime.Now.AddDays(1);
            List<Notice> noticeList = ncBase.CurrentEntities.Notice.Where(n => n.Type != 0).OrderByDescending(n => n.CreateTime).Take(ps).ToList();
            return View(noticeList);
        }
        public ActionResult ProductLog()
        {
            return View();
        }
        /// <summary>
        /// 获取未读消息公告
        /// </summary>
        /// <returns></returns>
        [IgnoreValidate]
        public JsonResult GetNoticeContentTip(GetNotReadNoticeListReq parame)
        {
            int totalSize = 0;
            parame.userId = this.GetLoginUser().UserID;
            parame.pi = 1;
            parame.ps = 10;
            var result = noticeBll.GetNotReadNoticeList(parame,ref totalSize).Select(n => new
            {
                n.Title,
                n.NoticeContent,
                n.NoticeId,
                n.Hits,
                n.Type,
                CreateTime = ((DateTime)n.CreateTime).ToString("yyyy-MM-dd")
            });
            return Json(new { data = result ,totalSize=totalSize}, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 全部设置为已读
        /// </summary>
        /// <returns></returns>
        public JsonResult NoticeSetAllIsRead()
        {
           return Json(noticeBll.NoticeSetIsRead(0,this.GetLoginUser().UserID));
        }
        #region 新房源提醒数
        /// <summary>
        /// 新房源提醒数
        /// </summary>
        /// <returns></returns>
         [IgnoreValidate]
        public JsonResult NewHouseNoticeCount()
        {
            // string ret = RedisHelper.OutQueue("HouseRemind_" + this.GetLoginUser().UserID);

            long count = RedisHelper.CountQueue("HouseRemind_" + this.GetLoginUser().UserID);

            return Json(count, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region 新房源提醒
        /// <summary>
        /// 新房源提醒
        /// </summary>
        /// <returns></returns>
   
        public ActionResult NewHouseNotice()
        {
            long num =  RedisHelper.CountQueue("HouseRemind_" + this.GetLoginUser().UserID);;
          List<NewHouseNoticeModel>  newHouseNoticeModels=new List<NewHouseNoticeModel>();
            int uid = this.GetLoginUser().UserID;
            ViewBag.Num = num;
            for (int i = 0; i < (num>20?20:num); i++)
            {
                string result = RedisHelper.OutQueue("HouseRemind_" + uid);
                if (!string.IsNullOrEmpty(result))
                {
                    //NewHouseNoticeModel newHouseNoticeModel = SerializeHelper.ParseFromJson<NewHouseNoticeModel>(result);
                    NewHouseNoticeModel newHouseNoticeModel = JsonConvert.DeserializeObject<NewHouseNoticeModel>(result);
                    if (newHouseNoticeModel.IsNoNull())
                    {
                        newHouseNoticeModels.Add(newHouseNoticeModel);
                    }
                }
            }

              return View(newHouseNoticeModels);
        }
      
   
        #endregion

        [AllowAnonymous]
        public ActionResult HtmlError()
        {
            
            return View();
        }

    }
}
