using System.Collections.Generic;
using ZJB.Api.DAL;
using ZJB.Api.Entity;
using ZJB.Api.Models;
using ZJB.Core.Injection;

namespace ZJB.Api.BLL
{
    public class GroupBll
    {
        private readonly GroupDal groupDal = Container.Instance.Resolve<GroupDal>();
        #region 群组创建
        /// <summary>
        /// 群组创建 -1 名称已经存在，-2创建数量达到上限，否则返回群组id
        /// </summary>
        /// <param name="groupName">名称</param>
        /// <param name="userId">创建者id</param>
        /// <param name="showType">0公开 可以在群组列表中搜索到 1 私密</param>
        /// <returns>-1 名称已经存在，-2创建数量达到上限，否则返回群组id</returns>
        public virtual int GroupAdd(string groupName, int userId, int showType)
        {
            return groupDal.GroupAdd(groupName, userId, showType);
        }
        #endregion

        #region 邀请加入群组
        public virtual int GroupInvite(int groupId, string userIds, int inviteUserId)
        {
            return groupDal.GroupInvite(groupId, userIds, inviteUserId);
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
            return groupDal.GroupApplyAdd(groupId, applyUserId);
        }
        #endregion
        #region  审批群成员，移除群成员
        /// <summary>
        /// 审批群成员，移除群成员 -1 没有权限 1成功
        /// </summary>
        /// <param name="groupId">群组id</param>
        /// <param name="userId">用户id</param>
        /// <param name="checkUserId">操作员id</param>
        /// <param name="status">状态 0：移除 1：正常</param>
        /// <returns>-1 没有权限 1成功</returns>
        public virtual int GroupCheckUser(int groupId, int userId, int checkUserId, int status)
        {
            return groupDal.GroupCheckUser(groupId, userId, checkUserId, status);
        }
        #endregion
        #region 升级管理员 取消管理员
        public virtual int GroupMemberTypeChange(int groupId, int operateUserId, int userId, int memberType)
        {
            return groupDal.GroupMemberTypeChange(groupId, operateUserId, userId, memberType);
        }
        #endregion
        #region 修改群资料
        /// <summary>
        /// 修改群资料 1 成功 -1 没有权限
        /// </summary>
        /// <param name="item"></param>
        /// <returns>1 成功 -1 没有权限</returns>
        public virtual int GroupEditInfo(UserGroup item)
        {
            return groupDal.GroupEditInfo(item);
        }
        #endregion
        #region 群成员列表
        /// <summary>
        /// 群成员列表
        /// </summary>
        /// <param name="parame"></param>
        /// <param name="totalSize"></param>
        /// <returns></returns>
        public virtual List<GroupMemberModel> GroupMemberList(GroupMemberListParame parame,ref int totalSize)
        {
            return groupDal.GroupMemberList(parame,ref totalSize);
        }
        #endregion

        #region 群列表
        public virtual List<GroupModel> GroupList(GroupListParame parame, ref int totalSize)
        {
            if (parame.SearchType == 0)
            {
                return groupDal.GroupList(parame, ref totalSize);
            }
            else if (parame.SearchType == 1)
            {
                return groupDal.GroupListByLetter(parame, ref totalSize);
            }
            else if (parame.SearchType == 2)
            {
                return groupDal.MyGroupList(parame, ref totalSize);
            }
            return groupDal.GroupList(parame, ref totalSize);
        }
        #endregion

    }
}
