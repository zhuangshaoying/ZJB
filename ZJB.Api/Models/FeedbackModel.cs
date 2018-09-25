using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ZJB.Api.Models
{
    public class FeedbackModel
    {
        public int FeedbackId { get; set; }
        public string FeedbackContent { get; set; }
        public DateTime CreateTime { get; set; }
        public int UserId { get; set; }
        public string UserName { get; set; }
        public int IsReplay { get; set; }
    }
}
