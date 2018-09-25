using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ZJB.WX.Models
{
    public class DynamicPostReq
    {
        //发布动态和回复的一些参数
        // GET: /DynamicPostReq/

        public string detail { get; set; }
        public int UserId { get; set; }
        public string ImageList { get; set; }
        public int CityId { get; set; }
        /// <summary>
        /// 回复的主帖id
        /// </summary>
        public int replyId { get; set; }
        /// <summary>
        /// 回复的评论id
        /// </summary>
        public int replyCommentId { get; set; }

           /// <summary>
        /// 回复的主帖id
        /// </summary>
        public int DynamicId { get; set; }
        public int CommentId { get; set; }
        
    }
}
