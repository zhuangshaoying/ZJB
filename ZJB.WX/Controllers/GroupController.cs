using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ZJB.Api.Models;
using ZJB.Api.BLL;
using ZJB.Api.Entity;
using ZJB.Core.Utilities;
namespace ZJB.WX.Controllers
{
    [Authorization]
    public class GroupController : BaseController
    {
        //
        // GET: /Group/
        private GroupBll groupBll = new GroupBll();
        private NCBaseRule ncBase = new NCBaseRule();
        public ActionResult Index()
        {
            return View();
        }
        #region 群组列表数据
        public JsonResult GroupList(GroupListParame parame)
        {
            PublicUserModel loginUser = this.GetLoginUser();
            int totalSize = 0;
            parame.UserId = loginUser.UserID;
            List<GroupModel> groupList = groupBll.GroupList(parame,ref totalSize);
            return Json(groupList,JsonRequestBehavior.AllowGet);
        }
        #endregion
        #region  创建群组
        [HttpPost]
        public JsonResult GroupAdd(string GroupName,int ShowType)
        {
            PublicUserModel loginUser = this.GetLoginUser();
            ShowType = ShowType == 0 ? 0 : 1;
            int code = groupBll.GroupAdd(GroupName, loginUser.UserID, ShowType);
            string msg = string.Empty;
            if (code == -1)
            {
                msg = "群组名已经存在";
            }
            else if (code == -2)
            {
                msg = "最多只能创建3个群组";
            }
            else if (code > 0)
            {
                msg = "创建成功";
            }
            return Json(new { code = code, msg = msg });
        }
        #endregion
        #region  群组详细页，管理页
        public ActionResult Detail(int Id)
        {
            PublicUserModel loginUser = this.GetLoginUser();
            UserGroup groupDetail = ncBase.CurrentEntities.UserGroup.Where(g => g.Id == Id && g.UserId == loginUser.UserID).FirstOrDefault();
            if (groupDetail.IsNull())
            {
                Response.Redirect("/Group");
            }
            return View(groupDetail);
        }
        #endregion
        #region 编辑群组信息
        public JsonResult EditGroup(UserGroup item)
        {
            PublicUserModel loginUser = this.GetLoginUser();
            item.UserId = loginUser.UserID;
           int code= groupBll.GroupEditInfo(item);
            string msg=string.Empty;
           if (code == -1)
           {
               msg = "没有权限";
           }
           else if (code == -2)
           {
               msg = "修改失败，群组名已存在";
           }
           else if (code == 1)
           {
               msg = "修改成功";
           }
           return Json(new { code = code, msg = msg });
        }
        #endregion
        #region 申请加入群组
        public JsonResult Apply(int groupId)
        {
            PublicUserModel loginUser = this.GetLoginUser();
            int code = groupBll.GroupApplyAdd(groupId,loginUser.UserID);
            string msg = string.Empty;
            if (code > 0)
            {
                msg = "申请成功，请等待管理审批";
            }
            else
            {
                msg = "失败";
            }
            return Json(new { code = code, msg = msg });
        }
        #endregion
        #region 群成员列表，（也包括待审批的，status查询)
        public JsonResult MemberList(GroupMemberListParame parame)
        {
            int totalSize = 0;
            List<GroupMemberModel> memberList = groupBll.GroupMemberList(parame, ref totalSize);
            return Json(memberList,JsonRequestBehavior.AllowGet);
        }
        #endregion
        #region 审核成员
        public JsonResult CheckMember(int groupId, int status,int userId)
        {
            PublicUserModel loginUser=this.GetLoginUser();
            int code = groupBll.GroupCheckUser(groupId, userId, loginUser.UserID, status);
            string msg = "";
            if (code == -1)
            {
                msg = "没有权限";
            }
            else if (code == 1)
            {
                msg = "成功";
            }
            return Json(new { code = code, msg = msg });
        }
        #endregion
    }
}
