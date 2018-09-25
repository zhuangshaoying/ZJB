using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ZJB.Api.Models;
using ZJB.Api.Entity;
namespace ZJB.Opportal
{
    public class FeedbackListView
    {
        public List<FeedbackModel> FeedbackList { get; set; }
        public List<Notice> ReplayList { get; set; }
        public int TotalSize { get; set; }
    }
}