using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using Microsoft.AspNet.SignalR;
using ZJB.Api.BLL;
using ZJB.Api.Models;
using System.Configuration;
using ZJB.Api.Common;
using ZJB.Core.Logging;

namespace ZJB.WX.Hubs
{
    public class ChatHub : Hub
    {
        // 静态属性
        public static List<ChatUser> ConnectedUsers = new List<ChatUser>(); // 在线用户列表
        public ChatBll chatBll = new ChatBll();
        private UserBll userBll = new UserBll();
        private WeiXinPublic wxPublic = new WeiXinPublic();
        private string s_appid = ConfigurationManager.AppSettings["WeixinAppId"]; //服务号
        private string wx_secret = ConfigurationManager.AppSettings["WeixinAppSecret"];
        private Logger logger = new Logger("ExceptionLog");
        /// <summary>
        /// 登录连线
        /// </summary>
        /// <param name="userID">用户ID</param>
        /// <param name="userName">用户名</param>
        /// <param name="deptName">部门名</param>
        public void Connect(int userID, string userName, string portrait, int groupid)//int chatid,
        {
            var id = Context.ConnectionId;

            if (ConnectedUsers.Count(x => x.ConnectionId == id) == 0)
            {
                if (ConnectedUsers.Count(x => x.UserID == userID && x.CID == groupid) > 0)
                {
                    var items = ConnectedUsers.Where(x => x.UserID == userID && x.CID == groupid).ToList();
                    foreach (var item in items)
                    {
                        Clients.AllExcept(id).onUserDisconnected(item.ConnectionId, item.NickName);
                    }
                    ConnectedUsers.RemoveAll(x => x.UserID == userID && x.CID == groupid);
                }
                //添加在线人员
                ConnectedUsers.Add(new ChatUser { ConnectionId = id, UserID = userID, NickName = userName, Portrait = portrait, CID = groupid, LastLoginTime = DateTime.Now });

                var user = ConnectedUsers.Where(x => x.UserID != userID && x.CID == groupid).ToList();
                //logger.Debug("--------------------");
                //logger.Debug(ConnectedUsers.Count.ToString());
                if (user != null && user.Count > 0)
                {
                    //var touser = user.Where(x => x.UserID == userID && x.CID == groupid).ToList()[0];
                    //logger.Debug("-------登陆战---------");
                    //logger.Debug(user[0].CID.ToString());
                    //logger.Debug(user[0].ConnectionId);
                    //logger.Debug(user[0].NickName);
                    //logger.Debug(user[0].UserID.ToString());
                    //logger.Debug("-------聊天者---------");
                    //logger.Debug(id);
                    //logger.Debug(userName);
                    //logger.Debug(portrait);
                   
                    Clients.Client(user[0].ConnectionId).ontoConnected(id, userName, portrait);
                }
                // 反馈信息给登录者
                Clients.Caller.onConnected(id, userName, ConnectedUsers);

                // 通知所有用户，有新用户连接
                //Clients.AllExcept(id).onNewUserConnected(id, userID, userName, groupid, portrait, DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));

            }
            else
            {

            }
        }
        /// <summary>
        /// 发送私聊
        /// </summary>
        /// <param name="toConnectionId">接收信息ConnectionId</param>
        /// <param name="message">信息</param>
        /// <param name="UserId">接收信息userid</param>
        /// <param name="ChatID">ObjectId</param>
        /// <param name="ChatName">title</param>
        public void SendPrivateMessage(string toConnectionId, string message, int UserId, string ChatID, string ChatName = "", int cid = 0)
        {
            string fromUserId = Context.ConnectionId;

            var toUser = ConnectedUsers.FirstOrDefault(x => x.ConnectionId == toConnectionId);
            var fromUser = ConnectedUsers.FirstOrDefault(x => x.ConnectionId == fromUserId);

            if (toUser != null && fromUser != null)//
            {
                logger.Debug(toUser + "在线");
                // send to 
                Clients.Client(toConnectionId).receivePrivateMessage(fromUserId, fromUser.NickName, message);//接收方信息
                chatBll.addChat(cid, fromUser.UserID, 1, message);
                // send to caller user
                //Clients.Caller.sendPrivateMessage(toUserId, fromUser.UserName, message);
            }
            else
            {
                logger.Debug(UserId+"不在线 1");
                var fUser = userBll.GetUserById(UserId);
                #region
                logger.Debug(UserId + "不在线 2");
                try
                {
                    wxPublic.WxPushMessage(fUser.OpenID, "您好，您有一条新的咨询消息。", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"),
                       fromUser.NickName, "房源发布", ChatName,
                       "http://www.zhujia001.com/chat/index?groupid=" + cid, "77T-pKSTN3IVsHOXlPr_RqpyKBy5RJFfQ-PAJaU7PBc");


                }
                catch (Exception ex)
                {

                    throw;
                }
                #endregion
                chatBll.addChat(cid, fromUser.UserID, 1, message);
                //表示对方不在线
                Clients.Caller.absentSubscriber();

            }
        }

        /// <summary>
        /// 离线
        /// </summary>
        public override System.Threading.Tasks.Task OnDisconnected(bool stopCalled)
        {
            var item = ConnectedUsers.FirstOrDefault(x => x.ConnectionId == Context.ConnectionId);
            if (item != null)
            {
                Clients.All.onUserDisconnected(item.ConnectionId, item.NickName);   //调用客户端用户离线通知
                ConnectedUsers.Remove(item);
            }
            return base.OnDisconnected(stopCalled);
        }
    }
}