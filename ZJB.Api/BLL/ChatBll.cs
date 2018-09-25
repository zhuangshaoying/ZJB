using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZJB.Api.DAL;
using ZJB.Api.Models;
using ZJB.Core.Injection;

namespace ZJB.Api.BLL
{
    public class ChatBll
    {
        private readonly ChatDal chatDal = Container.Instance.Resolve<ChatDal>();
        public virtual int addChat(int CID, int UserID, int Status, string Content)
        {
            return chatDal.addChat(CID, UserID, Status, Content);
        }

        /// <summary>
        /// 聊天室插入成员
        /// </summary>
        /// <param name="cid">聊天室id</param>
        /// <param name="userId">用户id</param>
        /// <param name="State">插入状态</param>
        /// <returns>true 成功，false 失败（成员超过2个 聊天室目前是1对1）</returns>
        public virtual bool addChatGroupUser(int cid, int userId, int State)
        {
            return chatDal.addChatGroupUser(cid, userId, State);
        }
        /// <summary>
        /// 查询聊天室用户信息
        /// </summary>
        /// <param name="cid"></param>
        /// <param name="userid">0查找所有人员</param>
        /// <returns></returns>
        public virtual List<ChatUser> GetUserChatGuoup(int cid, int userid = 0)
        {
            return chatDal.GetUserChatGuoup(cid, userid);
        }
        public virtual int addChatGroup(string Title, int ObjectId, int CreateUserId, int State)
        {
            return chatDal.addChatGroup(Title, ObjectId, CreateUserId, State);
        }
        public virtual List<ChatModel> GetUserChatList(int pageIndex, int pageSize, int cid, ref int totalSize)
        {
            return chatDal.GetUserChatList(pageIndex, pageSize, cid, ref totalSize);
        }
    }
}
