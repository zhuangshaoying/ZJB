using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ZJB.Api.BLL;
using ZJB.Api.Models;
using System.Text;
using ZJB.Api.Entity;
namespace ZJB.Opportal.Controllers
{
    public class FeedbackController : Controller
    {
        //
        // GET: /Feedback/
        private FeedbackBll feedbackBll = new FeedbackBll();
        private NoticeBll noticeBll = new NoticeBll();
        public ActionResult Index(FeedbackListReq parame)
        {
            parame.pi = parame.pi == 0 ? 1 : parame.pi;
            parame.ps = parame.ps == 0 ? 10 : parame.ps;
            int totalSize = 0;
            List<FeedbackModel> feedbackList = feedbackBll.GetFeedbackList(parame,ref totalSize);
            StringBuilder feedbackIds = new StringBuilder();
            foreach (FeedbackModel item in feedbackList)
            {
                feedbackIds.Append(item.FeedbackId+",");
            }
            if (feedbackIds.ToString().Length > 0)
                feedbackIds.Remove(feedbackIds.ToString().Length-1, 1);

            List<Notice> replayList = noticeBll.GetReplayListByIds(feedbackIds.ToString());
            FeedbackListView viewList = new FeedbackListView();
            viewList.FeedbackList = feedbackList;
            viewList.ReplayList = replayList;
            viewList.TotalSize = totalSize;
            ViewBag.pi=parame.pi;
            ViewBag.ps = parame.ps;
            return View(viewList);
        }

    }
}
