using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MongoDB.Bson;
using ZJB.Api.Models;

namespace ZJB.Api.DAL
{
    public class ChatDal : BaseDal
    {
        public ChatDal()
            : base("WX")
        { }
        public virtual int addChat(int CID, int UserID, int Status, string Content)
        {
            DbCommand cmd = GetSqlStringCommand("INSERT dbo.ChatMessage(CID,UserID,Content,Status,AddDate)VALUES( @CID,@UserID,@Content,@Status,GETDATE())");
            AddInParameter(cmd, "@CID", DbType.Int32, CID);
            AddInParameter(cmd, "@UserID", DbType.Int32, UserID);
            AddInParameter(cmd, "@Content", DbType.String, Content);
            AddInParameter(cmd, "@Status", DbType.Int32, Status);
            return ExecuteNonQuery(cmd); ;
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
            DbCommand cmd = GetStoredProcCommand("P_Api_SetChatGroupUser");
            AddInParameter(cmd, "@cid", DbType.Int32, cid);
            AddInParameter(cmd, "@userId", DbType.Int32, userId);
            AddInParameter(cmd, "@status", DbType.Int32, State);
            AddOutParameter(cmd, "@irows", DbType.Int32, 4);
            ExecuteNonQuery(cmd);
            return Convert.ToBoolean(GetOutputParameter<Int32>(cmd, "@irows"));
        }
        public virtual int addChatGroup(string Title, int ObjectId, int CreateUserId, int State)
        {
            DbCommand cmd = GetStoredProcCommand("P_Api_SetChatGroup");
            AddInParameter(cmd, "@Title", DbType.String, Title);
            AddInParameter(cmd, "@ObjectId", DbType.Int32, ObjectId);
            AddInParameter(cmd, "@CreateUserId", DbType.Int32, CreateUserId);
            AddInParameter(cmd, "@State", DbType.Int32, State);
            AddOutParameter(cmd, "@GroupId", DbType.Int32, 4);
            ExecuteNonQuery(cmd);
            return GetOutputParameter<Int32>(cmd, "@GroupId");
        }

        /// <summary>
        /// 查询聊天室用户信息
        /// </summary>
        /// <param name="cid"></param>
        /// <param name="userid">0查找所有人员</param>
        /// <returns></returns>
        public virtual List<ChatUser> GetUserChatGuoup(int cid, int userid = 0)
        {
            DbCommand cmd = GetSqlStringCommand(string.Format(@"SELECT g.*,u.UserID,u.NickName,u.Portrait FROM dbo.ChatGroup AS g
                                                INNER JOIN dbo.ChatGroupUser AS gu ON g.CID = gu.CID
                                                INNER JOIN dbo.PublicUser AS u ON u.UserID = gu.UserID
                                                WHERE g.CID = @cid AND g.State = 1 AND u.Status = 1 {0}", userid > 0 ? " AND u.UserID =@userid " : ""));
            AddInParameter(cmd, "@cid", DbType.Int32, cid);
            if (userid > 0)
            {
                AddInParameter(cmd, "@userid", DbType.Int32, userid);
            }
            DataSet ds = ExecuteDataSet(cmd);
            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                return BulidModelChatGroupList(ds.Tables[0].Select());
            }
            return new List<ChatUser>();
        }

        private List<ChatUser> BulidModelChatGroupList(IEnumerable<DataRow> rows)
        {
            return (from row in rows select ModelChatGroupList(row)).ToList();
        }
        private ChatUser ModelChatGroupList(DataRow dr)
        {
            return new ChatUser()
            {
                CID = To<int>(dr, "CID"),
                Title = To<string>(dr, "Title"),
                UserID = To<int>(dr, "UserID"),
                ObjectId = To<int>(dr, "ObjectId"),
                NickName = To<string>(dr, "NickName"),
                Portrait = To<string>(dr, "Portrait"),
            };
        }

        public virtual List<ChatModel> GetUserChatList(int pageIndex, int pageSize, int cid, ref int totalSize)
        {
            DbCommand cmd = GetStoredProcCommand("P_Api_GetChatGroupList");
            AddInParameter(cmd, "@pageIndex", DbType.Int32, pageIndex);
            AddInParameter(cmd, "@pageSize", DbType.Int32, pageSize);
            AddInParameter(cmd, "@cid", DbType.String, cid);
            AddOutParameter(cmd, "@totalSize", DbType.Int32, 4);
            DataSet ds = ExecuteDataSet(cmd);
            int.TryParse(cmd.Parameters["@totalSize"].Value.ToString(), out totalSize);
            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                return BulidModelList(ds.Tables[0].Select());
            }
            return new List<ChatModel>();
        }
        private List<ChatModel> BulidModelList(IEnumerable<DataRow> rows)
        {
            return (from row in rows select BulidModel(row)).ToList();
        }
        private ChatModel BulidModel(DataRow dr)
        {
            return new ChatModel()
            {
                ID = To<int>(dr, "ID"),
                UserID = To<int>(dr, "UserID"),
                NickName = To<string>(dr, "NickName"),
                Cid = To<int>(dr, "cid"),
                AddTime = To<string>(dr, "AddDate"),
                Content = To<string>(dr, "Content"),
                Portrait = To<string>(dr, "Portrait"),
            };
        }

    }
}
