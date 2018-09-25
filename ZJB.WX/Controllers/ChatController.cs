using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.SignalR.Hubs;
using ZJB.Api.BLL;
using ZJB.Api.Models;
using ZJB.Core.Utilities;

namespace ZJB.WX.Controllers
{
    public class ChatController : BaseController
    {
        private UserBll userBll = new UserBll();
        private ChatBll chatBll = new ChatBll();
        private HouseBll houseBll = new HouseBll();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ChatId">房源id</param>
        /// <param name="ChatName">房源名称</param>
        /// <param name="touserid">房源发布用户id</param>
        /// <param name="toname">房源发布人名字</param>
        /// <returns></returns>
        [Authorization]
        public ActionResult Index(int chatid = 1, int groupid = 0)//string chatname = "", int fromuserid = 1004638
        {
            //var userid = this.GetLoginUser().UserID;
            var user = this.GetLoginUser();
            if (groupid > 0)
            {
                if (!chatBll.addChatGroupUser(groupid, user.UserID, 1))
                {
                    return RedirectToAction("Index", "Esf");
                }
                var chatgroupuser = chatBll.GetUserChatGuoup(groupid);
                ViewBag.GroupId = groupid;
                //var touser = userBll.GetUserById(fromuserid);
                ViewBag.UserID = user.UserID;
                ViewBag.NickName = user.NickName;
                ViewBag.ChatId = chatid;
                ViewBag.Portrait = user.Portrait;

                var touser = chatBll.GetUserChatGuoup(groupid).Where(x => x.UserID != user.UserID).FirstOrDefault();
                if (touser != null && touser.UserID > 0)
                {
                    ViewBag.FUserid = touser.UserID;
                    ViewBag.FName = touser.NickName;
                    ViewBag.ChatName = touser.Title;
                }
            }
            else
            {
                HouseListReq parames = new HouseListReq();
                parames.postType = 0;
                parames.buildingType = 0;
                parames.buildingStatus = 1;
                parames.cell = "";
                parames.sort = 7;
                parames.houseId = chatid;
                parames.title = "";
                parames.tags = "";
                parames.page = 1;
                parames.pageSize = 1;
                parames.userId = 0;
                parames.collectuserid = user.UserID;
                int totalSize = 0;
                var list = houseBll.GetEsfHouseList(parames, ref totalSize);
                if (list.IsNoNull() && list.Count > 0)
                {
                    string chatname = list[0].CommunityName;
                    int fromuserid = list[0].UserID;
                    ViewBag.GroupId = chatBll.addChatGroup(chatname, chatid, this.GetLoginUser().UserID, 1);
                    var touser = userBll.GetUserById(fromuserid);
                    ViewBag.UserID = user.UserID;
                    ViewBag.NickName = user.NickName;
                    ViewBag.ChatId = chatid;
                    ViewBag.Portrait = user.Portrait;
                    ViewBag.ChatName = chatname;
                    ViewBag.FUserid = fromuserid;
                    ViewBag.FName = touser.NickName;
                }
                else
                {
                    return RedirectToAction("Index", "Esf");
                }
            }
            return View();

        }



        [HttpGet]
        public JsonResult GetUserChatList(int pageSize = 10, int pageIndex = 1, int cid = 0)
        {
            int totalSize = 0;
            var list = chatBll.GetUserChatList(pageIndex, pageSize, cid, ref totalSize);
            return Json(new { data = list, totalCount = totalSize }, JsonRequestBehavior.AllowGet);
        }
    }
}
