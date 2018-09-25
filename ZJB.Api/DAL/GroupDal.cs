using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using ZJB.Api.Entity;
using ZJB.Api.Models;
using System.Linq;
namespace ZJB.Api.DAL
{
    public class GroupDal : BaseDal
    {
        public GroupDal()
            : base("WX")
        { }
        #region 群组创建
        /// <summary>
        /// 群组创建
        /// </summary>
        /// <param name="groupName">名称</param>
        /// <param name="userId">创建者id</param>
        /// <param name="showType">0公开 可以在群组列表中搜索到 1 私密</param>
        /// <returns>-1 名称已经存在，-2创建数量达到上限，否则返回群组id</returns>
        public virtual int GroupAdd(string groupName, int userId, int showType)
        {
            DbCommand cmd = GetStoredProcCommand("P_Group_Add");
            AddInParameter(cmd, "@GroupName", DbType.String, groupName);
            AddInParameter(cmd, "@UserId", DbType.Int32, userId);
            AddInParameter(cmd, "@ShowType", DbType.Int32, showType);
            AddReturnParameter(cmd, "@ReturnValue", DbType.Int32);
            ExecuteNonQuery(cmd);
            int code = 0;
            int.TryParse(cmd.Parameters["@ReturnValue"].Value.ToString(), out code);
            return code;
        }
        #endregion

        #region 邀请加入群组
        /// <summary>
        /// 邀请加入群组
        /// </summary>
        /// <param name="groupId">群组id</param>
        /// <param name="userIds">用户id列表</param>
        /// <param name="inviteUserId">邀请人</param>
        /// <returns>-1 没有邀请权限，1 成功</returns>
        public virtual int GroupInvite(int groupId, string userIds, int inviteUserId)
        {
            DbCommand cmd = GetStoredProcCommand("P_Group_InviteUser");
            AddInParameter(cmd, "@GroupId", DbType.Int32, groupId);
            AddInParameter(cmd, "@UserIds", DbType.String, userIds);
            AddInParameter(cmd, "@InviteUserId", DbType.Int32, inviteUserId);
            AddReturnParameter(cmd, "@ReturnValue", DbType.Int32);
            ExecuteNonQuery(cmd);
            int code = 0;
            int.TryParse(cmd.Parameters["@ReturnValue"].Value.ToString(), out code);
            return code;
        }
        #endregion
        #region 申请加入群组
        /// <summary>
        /// 申请加入群组
        /// </summary>
        /// <param name="groupId">群组id</param>
        /// <param name="applyUserId">申请人id</param>
        /// <returns></returns>
        public virtual int GroupApplyAdd(int groupId, int applyUserId)
        {
            DbCommand cmd = GetStoredProcCommand("P_Group_ApplyAdd");
            AddInParameter(cmd, "@GroupId", DbType.Int32, groupId);
            AddInParameter(cmd, "@UserId", DbType.Int32, applyUserId);
            int code = ExecuteNonQuery(cmd);
            return code;
        }
        #endregion
        #region  审批群成员，移除群成员
        /// <summary>
        /// 审批群成员，移除群成员
        /// </summary>
        /// <param name="groupId">群组id</param>
        /// <param name="userId">用户id</param>
        /// <param name="checkUserId">操作员id</param>
        /// <param name="status">状态 0：移除 1：正常</param>
        /// <returns>-1 没有权限 1成功</returns>
        public virtual int GroupCheckUser(int groupId, int userId, int checkUserId, int status)
        {
            DbCommand cmd = GetStoredProcCommand("P_Group_CheckUser");
            AddInParameter(cmd, "@GroupId", DbType.Int32, groupId);
            AddInParameter(cmd, "@UserId", DbType.Int32, userId);
            AddInParameter(cmd, "@CheckUserId", DbType.Int32, checkUserId);
            AddInParameter(cmd, "@Status", DbType.Int32, status);
            AddReturnParameter(cmd, "@ReturnValue", DbType.Int32);
            ExecuteNonQuery(cmd);
            int code = 0;
            int.TryParse(cmd.Parameters["@ReturnValue"].Value.ToString(), out code);
            return code;
        }
        #endregion
        #region 升级管理员 取消管理员
        /// <summary>
        /// 升级管理员 取消管理员
        /// </summary>
        /// <param name="groupId"></param>
        /// <param name="operateUserId">操作员</param>
        /// <param name="userId">用户编号</param>
        /// <param name="memberType">用户类型 0 是普通成员 2：管理员</param>
        /// <returns>-100：memberType 类型传的不对，1：成功 -1 没有权限</returns>
        public virtual int GroupMemberTypeChange(int groupId, int operateUserId, int userId, int memberType)
        {
            DbCommand cmd = GetStoredProcCommand("P_Group_MemberType");
            AddInParameter(cmd, "@GroupId", DbType.Int32, groupId);
            AddInParameter(cmd, "@OperateUserId", DbType.Int32, operateUserId);
            AddInParameter(cmd, "@UserId", DbType.Int32, userId);
            AddInParameter(cmd, "@MemberType", DbType.Int32, memberType);
            AddReturnParameter(cmd, "@ReturnValue", DbType.Int32);
            ExecuteNonQuery(cmd);
            int code = 0;
            int.TryParse(cmd.Parameters["@ReturnValue"].Value.ToString(), out code);
            return code;
        }
        #endregion
        #region 修改群资料
        /// <summary>
        /// 修改群资料
        /// </summary>
        /// <param name="item"></param>
        /// <returns>1 成功 -1 没有权限</returns>
        public virtual int GroupEditInfo(UserGroup item)
        {
            DbCommand cmd = GetStoredProcCommand("P_Group_EditInfo");
            AddInParameter(cmd, "@UserId", DbType.Int32, item.UserId);
            AddInParameter(cmd, "@GroupId", DbType.Int32, item.Id);
            AddInParameter(cmd, "@GroupName", DbType.String, item.GroupName);
            AddInParameter(cmd, "@Description", DbType.String, item.Description);
            AddInParameter(cmd, "@ShowType", DbType.Int32, item.ShowType);
            AddInParameter(cmd, "@InviteType", DbType.Int32, item.InviteType);
            AddReturnParameter(cmd, "@ReturnValue", DbType.Int32);
            ExecuteNonQuery(cmd);
            int code = 0;
            int.TryParse(cmd.Parameters["@ReturnValue"].Value.ToString(), out code);
            return code;
        }
        #endregion
        #region 群成员列表
        /// <summary>
        /// 群成员列表
        /// </summary>
        /// <param name="parame"></param>
        /// <param name="totalSize"></param>
        /// <returns></returns>
        public virtual List<GroupMemberModel> GroupMemberList(GroupMemberListParame parame, ref int totalSize)
        {
            List<GroupMemberModel> userList = new List<GroupMemberModel>();
            DbCommand cmd = GetStoredProcCommand("P_Group_MemberList");
            AddInParameter(cmd, "@GroupId", DbType.Int32, parame.GroupId);
            AddInParameter(cmd, "@PageSize", DbType.Int32, parame.PageSize);
            AddInParameter(cmd, "@LastId", DbType.Int32, parame.LastId);
            AddInParameter(cmd, "@Status", DbType.Int32, parame.Status);
            AddInParameter(cmd, "@Keyword", DbType.String, parame.KeyWord);
            AddOutParameter(cmd, "@TotalSize", DbType.Int32, 4);
            DataSet ds = ExecuteDataSet(cmd);
            int.TryParse(cmd.Parameters["@TotalSize"].Value.ToString(), out totalSize);
            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                userList=BuildToModelList(ds.Tables[0].Select());
            }
            return userList;
        }
        private List<GroupMemberModel> BuildToModelList(IEnumerable<DataRow> drs)
        {
            return (from dr in drs select BuildToModel(dr)).ToList();
        }
        private GroupMemberModel BuildToModel(DataRow dr)
        {
            return new GroupMemberModel()
            {
                Id = To<int>(dr, "Id"),
                MemberType = To<int>(dr, "MemberType"),
                OperateUserId = To<int>(dr, "OperateUserId"),
                Portrait = To<string>(dr, "Portrait"),
                Tel = To<string>(dr, "Tel"),
                UserId = To<int>(dr, "UserId"),
                UserName = To<string>(dr, "UserName")
            };
        }
        #endregion

        #region 群列表
        public virtual List<GroupModel> GroupList(GroupListParame parame, ref int totalSize)
        {
           
            DbCommand cmd = GetStoredProcCommand("P_Group_List");
            AddInParameter(cmd, "@UserId", DbType.Int32, parame.UserId);
            AddInParameter(cmd, "@Keyword", DbType.String, parame.KeyWord);
            AddInParameter(cmd, "@PageSize", DbType.Int32, parame.PageSize);
            AddInParameter(cmd, "@LastId", DbType.Int32, parame.LastId);
            AddOutParameter(cmd, "@TotalSize", DbType.Int32, 4);
            DataSet ds = ExecuteDataSet(cmd);
            int.TryParse(cmd.Parameters["@TotalSize"].Value.ToString(), out totalSize);
            List<GroupModel> groupList = new List<GroupModel>();
            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                groupList = BuildGroupModelToList(ds.Tables[0].Select());
            }
            return groupList;
        }
        public virtual List<GroupModel> GroupListByLetter(GroupListParame parame, ref int totalSize)
        {
            DbCommand cmd = GetStoredProcCommand("P_Group_ListByLetter");
            AddInParameter(cmd, "@UserId", DbType.Int32, parame.UserId);
            AddInParameter(cmd, "@Keyword", DbType.String, parame.KeyWord);
            AddInParameter(cmd, "@PageSize", DbType.Int32, parame.PageSize);
            AddInParameter(cmd, "@LastId", DbType.Int32, parame.LastId);
            AddOutParameter(cmd, "@TotalSize", DbType.Int32, 4);
            DataSet ds = ExecuteDataSet(cmd);
            int.TryParse(cmd.Parameters["@TotalSize"].Value.ToString(), out totalSize);
            List<GroupModel> groupList = new List<GroupModel>();
            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                groupList = BuildGroupModelToList(ds.Tables[0].Select());
            }
            return groupList;
        }
        public virtual List<GroupModel> MyGroupList(GroupListParame parame, ref int totalSize)
        {
            DbCommand cmd = GetStoredProcCommand("P_Group_MyList");
            AddInParameter(cmd, "@UserId", DbType.Int32, parame.UserId);
            AddInParameter(cmd, "@Status", DbType.Int32, parame.Status);
            DataSet ds = ExecuteDataSet(cmd);
            List<GroupModel> groupList = new List<GroupModel>();
            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                groupList = BuildGroupModelToList(ds.Tables[0].Select());
            }
            return groupList;
        }
        private List<GroupModel> BuildGroupModelToList(IEnumerable<DataRow> drs)
        {
            return (from dr in drs select BuildGroupModel(dr)).ToList();
        }
        private GroupModel BuildGroupModel(DataRow dr)
        {
            return new GroupModel()
            {
                Id = To<int>(dr, "Id"),
                Description = IsContainColumns(dr, "Description") ? To<string>(dr, "Description") : "",
                GroupName = IsContainColumns(dr, "GroupName") ? To<string>(dr, "GroupName") : "",
                Icon = IsContainColumns(dr, "Icon") ? To<string>(dr, "Icon") : "",
                MemberCount = IsContainColumns(dr, "MemberCount") ? To<int>(dr, "MemberCount") : 0,
                MemberStatus = IsContainColumns(dr, "MemberStatus") ? To<int>(dr, "MemberStatus") : 0,
                MemberType = IsContainColumns(dr, "MemberType") ? To<int>(dr, "MemberType") : 0,
                ShowType = IsContainColumns(dr, "ShowType") ? To<int>(dr, "ShowType") : 1
            };
        }
        private bool IsContainColumns(DataRow dr,string columnsName)
        {
            return dr.Table.Columns.Contains(columnsName);
        }
        #endregion
    }
}
