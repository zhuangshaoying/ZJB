using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZJB.Api.Models
{
    public class ChatModel
    {
        public ChatModel()
        { }
        public int ID { set; get; }
        public int Cid { set; get; }
        public int UserID { set; get; }
        public string NickName { set; get; }
        public string Portrait { set; get; }
        //public int FUserID { set; get; }
        //public string FNickName { set; get; }
        //public string FPortrait { set; get; }
        public string AddTime { set; get; }

        public string Content { set; get; }
    }
    public class ChatUser
    {
        public ChatUser()
        { }
        public int CID { set; get; }

        public int ChatID { set; get; }
        public string NickName { set; get; }
        public int UserID { set; get; }
        public string Portrait { set; get; }
        /// <summary>
        /// 聊天连接ID
        /// </summary>
        public string ConnectionId { get; set; }
        public DateTime LastLoginTime { get; set; }
        public string Title { set; get; }
        public int ObjectId { set; get; }
    }
}
