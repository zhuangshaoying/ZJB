using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ZJB.Api.Entity;
using ZJB.Api.BLL;
using ZJB.Core.Utilities;
namespace ZJB.Opportal.Controllers
{
    public class NoticeController : Controller
    {
        //
        // GET: /Notice/
        private NCBaseRule bcBase = new NCBaseRule();
        private NoticeBll noticeBll = new NoticeBll();
        public ActionResult Index()
        {
            return View();
        }
        public JsonResult ReplayFeedback(int feedbackId,int userId, string content)
        {
            Feedback thisFeedback = bcBase.CurrentEntities.Feedback.Where(f => f.FeedbackId == feedbackId).FirstOrDefault();
            if (thisFeedback != null && thisFeedback.FeedbackId > 0)
            {

                Notice newNotice = new Notice()
                {
                    CreateTime = DateTime.Now,
                    Title = "感谢您的反馈",
                    NoticeContent = content,
                    FeedbackId = feedbackId,
                    Type = 0,
                    CreateUserId=this.GetLoginUser().UserID,
                    Publisher=this.GetLoginUser().Name
                };
                bcBase.CurrentEntities.Notice.AddObject(newNotice);
                bcBase.CurrentEntities.SaveChanges();
                if (newNotice.NoticeId > 0)
                {

                    thisFeedback.IsReplay = true;
                    NoticeLog noticeLog = new NoticeLog()
                    {
                        AddTime = DateTime.Now,
                        IsRead = false,
                        NoticeId = newNotice.NoticeId,
                        ReceiverId = userId,
                        Type = 0,
                        SenderId = 0
                    };
                    bcBase.CurrentEntities.NoticeLog.AddObject(noticeLog);
                    bcBase.CurrentEntities.SaveChanges();
                    return Json(new { status = 0 });
                }
                return Json(new { status = 1 });
            }
        
            return Json(new { status = 1 });
        }
        public JsonResult GetNoticeByFeedbackId(int feedbackId)
        {
            List<Notice> noticeList = bcBase.CurrentEntities.Notice.Where(n => n.FeedbackId == feedbackId).ToList();
            return Json(new { data = noticeList });
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

            noticeList = bcBase.CurrentEntities.Notice
                .Where(n => n.Type == type)
                .OrderByDescending(n => n.CreateTime)
                .Skip((pi - 1) * ps)
                .Take(ps).ToList();

            totalSize = bcBase.CurrentEntities.Notice
                .Where(n => n.Type == type && n.Type != 0).ToList().Count;

            ViewBag.TotalSize = totalSize;
            ViewBag.pi = pi;
            ViewBag.ps = ps;
            ViewBag.type = type;
            if (noticeList != null)
                return View(noticeList);
            return View(new List<Notice>());
        }
        /// <summary>
        /// 发布公告 页面
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public ActionResult NoticeAdd(int noticeId=0,int type=1)
        {
            Notice noticeDetail = bcBase.CurrentEntities.Notice.Where(n => n.NoticeId == noticeId).FirstOrDefault();
            return View(noticeDetail);
        }
        /// <summary>
        /// 添加新公告 
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [ValidateInput(false)]
        public JsonResult NoticeNew(Notice entity,int receiverId=0)
        {
            entity.Publisher = this.GetLoginUser().Name;
            entity.CreateUserId = this.GetLoginUser().UserID;
            if (entity.Type == 0&&receiverId==0)
            {
                return Json(new { status = 0,msg="没有设置消息接收人" });
            }
            if (entity.NoticeId > 0)
            {
                Notice newNotice = bcBase.CurrentEntities.Notice.Where(n => n.NoticeId == entity.NoticeId).FirstOrDefault();
                if (newNotice.IsNoNull())//修改
                {
                    newNotice.Type = entity.Type;
                    newNotice.Title = entity.Title;
                    newNotice.NoticeContent = entity.NoticeContent;
                    newNotice.Publisher = entity.Publisher;
                    newNotice.CreateUserId = entity.CreateUserId;
                    bcBase.CurrentEntities.SaveChanges();
                    return Json(new { status = newNotice.NoticeId });
                }   
            }
           int row= noticeBll.NoticeAdd(entity, receiverId);
           return Json(new { status = row });
        }

        #region 首页公告
        /// <summary>
        /// 首页公告
        /// </summary>
        /// <returns></returns>
        public ActionResult HomeNoticeListView(int ps = 5)
        {
            List<Notice> noticeList = bcBase.CurrentEntities.Notice.Where(n => n.Type != 0).OrderByDescending(n => n.CreateTime).Take(ps).ToList();
            return View(noticeList);
        }
        #endregion
    }
}
