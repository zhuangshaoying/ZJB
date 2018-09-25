using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Newtonsoft.Json;
using ZJB.Api.BLL;
using ZJB.Api.Models;
using ZJB.Api.Entity;
using ZJB.Core.Utilities;
using ZJB.WX.Filters;
using ZJB.WX.Models;
using System.Text.RegularExpressions;
using System.Data.Objects;
namespace ZJB.WX.Controllers
{
    [Authorization]
    [ActionLog(CheckPoints = false)]
    public class SocialController : Controller
    {
      
        private NCBaseRule ncBase = new NCBaseRule();
        private DynamicBll dynamicBll = new DynamicBll();
        private VoteSurveyBll voteBll = new VoteSurveyBll();
        private UserBll userBll = new UserBll();
        System.Web.Script.Serialization.JavaScriptSerializer jss = new System.Web.Script.Serialization.JavaScriptSerializer();

      
        public ActionResult Index()
        {
            PublicUserModel loginUser = this.GetLoginUser();
            int uid = loginUser.IsNoNull() ? loginUser.UserID : 0;
            VPublicUser userInfo = ncBase.CurrentEntities.VPublicUser.Where(c => c.UserID == uid).FirstOrDefault();
            UserTaskSignStat userSign = userBll.UserSingStat(userInfo.UserID);
            ViewBag.UserTaskSignStat = userSign;

            int taskKey = 0;
            #region 判断时间
            int timeOut = 4;//四小时时间
            DateTime nowTime = DateTime.Now;
            if (nowTime.Hour >= 8 && nowTime.Hour < 8 + timeOut)
            {
                taskKey = (int)PointsEnum.EveryDay_Sign_8;
            }
            else if (nowTime.Hour >= 15 && nowTime.Hour < 15 + timeOut)
            {
                taskKey = (int)PointsEnum.EveryDay_Sign_16;
            }

            #endregion

            if (loginUser.Points < 1)
                taskKey = 0;
            ViewBag.TaskKey = taskKey;
            ViewBag.Uid = CryptoUtility.TripleDESEncrypt(Convert.ToString(loginUser.UserID));
            return View(userInfo);
        }
        public ActionResult Test()
        {

            return View();
        }
        #region 动态列表和回复列表
        /// <summary>
        /// 动态列表
        /// </summary>
        /// <param name="parame"></param>
        /// <returns></returns>
    
        public JsonResult DynamicList(DynamicListReq parame)
        {
            PublicUserModel loginUser = this.GetLoginUser();
            parame.UserId = loginUser.UserID;
            parame.IsGetSupport = 1;
            List<DynamicModel> dynamicList = dynamicBll.DynamicList(parame);
          
            List<DynamicImage> dynamicImageList = new List<DynamicImage>();
            if (dynamicList.IsNoNull() && dynamicList.Count > 0)
            {
                dynamicImageList=dynamicBll.DynamicImageListBydynamicIds(dynamicList.Select(s => s.Id).ToList());
            }
            if(dynamicList!=null&&dynamicList.Count>0)
            {

                return Json(new { data = FormatDynamicList(dynamicList, dynamicImageList,loginUser.UserID) }, JsonRequestBehavior.AllowGet);

            }
            return Json(new {data=dynamicList},JsonRequestBehavior.AllowGet);
        }
        /// <summary>
        /// 回复列表
        /// </summary>
        /// <returns></returns>
      
        public JsonResult DynamicReplayList(string dynamicIds,int pageSize=5,int lastId=0)
        {
            List<string> dynamicIdList = string.IsNullOrEmpty(dynamicIds) ? new List<string>() : dynamicIds.Split(',').ToList();
            List<DynamicModel> dynamicReplayList = dynamicBll.DynamicReplayListBydynamicIds(dynamicIds, pageSize, lastId);
            var resultList = dynamicIdList.Select(c => new
            {
                DynamicId = c,
                CommentList = (dynamicReplayList != null && dynamicReplayList.Count > 0)
                ?
                dynamicReplayList.Where(o => o.ReplayId.ToString() == c).Select(r => new { 
                 AddTime=DateTimeUtility.GetDisplayTime(r.AddTime),
                 r.DynamicContent,
                 WrapComment = GetContentFace(r.DynamicContent),
                 r.UserId,
                 r.UserName,
                 r.Portrait,
                 r.Id,
                 r.ReplyCommentId,
                 r.ReplayUserId,
                 r.ReplayUserName
                })
                : null
            });
            return Json(new { data = resultList }, JsonRequestBehavior.AllowGet);

        }
        
        #endregion

        #region 添加动态和回复
        /// <summary>
        /// 添加动态
        /// </summary>
        /// <param name="parame"></param>
        /// <param name="ImageList"></param>
        /// <returns></returns>
        public JsonResult AddDynamic(DynamicPostReq parame)
        {
            if (string.IsNullOrEmpty(parame.detail))
            {
                return Json(new { status = -1 });
            }
          
            PublicUserModel loginUser=this.GetLoginUser();
            List<DynamicImage> ImageList=jss.Deserialize<List<DynamicImage>>(parame.ImageList);
            DynamicModel dynamicItem = new DynamicModel()
            {
                 UserId= loginUser.UserID,
                 ImageList=ImageList,
                 DynamicContent = parame.detail
            };
            int dynamicId = dynamicBll.DynamicAdd(dynamicItem);
            dynamicItem.Id = dynamicId;
            dynamicItem.UserName = loginUser.Name;
            dynamicItem.Portrait = loginUser.Portrait;
            dynamicItem.AddTime = DateTime.Now;
            dynamicItem.CompanyName = loginUser.CompanyName.IsNull() ? "" : loginUser.CompanyName;
            return Json(new { status = 1, data = FormatDyamic(dynamicItem, ImageList,loginUser.UserID) });
        }
        /// <summary>
        /// 添加回复
        /// </summary>
        /// <returns></returns>
        public JsonResult AddDynamicComment(DynamicPostReq parame)
        {
            if (string.IsNullOrWhiteSpace(parame.detail))
            {
                return Json(new { status = 0, msg = "参数错误" });
            }
            PublicUserModel loginUser = this.GetLoginUser();
            DynamicModel dynamicItem = new DynamicModel()
            {
                UserId = loginUser.UserID,
                ImageList = jss.Deserialize<List<DynamicImage>>(parame.ImageList),
                DynamicContent = parame.detail,
                Id=parame.replyId,
                ReplyCommentId = parame.replyCommentId
            };
            int dynamicReplayId = dynamicBll.DynamicCommentAdd(dynamicItem);
            return Json(new { status = dynamicReplayId, msg = "评论成功" });
        }
        #endregion

        #region 返回的动态列表格式化
        private dynamic FormatDynamicList(List<DynamicModel> dynamicList, List<DynamicImage> dynamicImageList,int userid)
        {
            return (from item in dynamicList select FormatDyamic(item, dynamicImageList.Where(i => i.DynamicId == item.Id).ToList(), userid));
        }
        private dynamic FormatDyamic(DynamicModel dynamicItem, List<DynamicImage> dynamicImageList,int userid)
        {
            return new
            {
                dynamicItem.Abstract,
                AddTime = DateTimeUtility.GetDisplayTime(dynamicItem.AddTime),
                dynamicItem.CityId,
                dynamicItem.CityName,
                dynamicItem.ClickNum,
                dynamicItem.CommentNum,
                dynamicItem.DynamicContent,
                WrapComment=GetContentFace(dynamicItem.DynamicContent),
                dynamicItem.Id,
                ImageList = dynamicImageList.Select(i => new
                {
                    i.ImageUrl,
                    i.ImageHeight,
                    i.ImageWidth
                }),
                Ip = dynamicItem.Ip,
                IsEssence = dynamicItem.IsEssence,
                dynamicItem.IsHelp,
                dynamicItem.IsHot,
                dynamicItem.IsMentionAll,
                IsTop=(dynamicItem.TopTime!=null&&dynamicItem.TopTime>DateTime.Now)?1:0,
                dynamicItem.LastCommentId,
                LastCommentTime = dynamicItem.LastCommentTime.ToString(),//DateTimeUtility.ToUnixTime_Milliseconds(d.LastCommentTime),
                dynamicItem.Lat,
                dynamicItem.Lng,
                dynamicItem.Location,
                dynamicItem.ShareNum,
                dynamicItem.State,
                dynamicItem.Title,
                TopTime = DateTimeUtility.ToUnixTime_Milliseconds(dynamicItem.TopTime),
                dynamicItem.Type,
                dynamicItem.UserId,
                dynamicItem.Visible,
                dynamicItem.VoteId,
                dynamicItem.UserName,
                dynamicItem.Portrait,
                SupportList =dynamicItem.SupportList!=null?dynamicItem.SupportList.Select(s => new { 
                s.UserName,
                s.UserId,
                s.Id,
                s.DynamicId
                }):null,
                IsSupport = dynamicItem.SupportList!=null&&dynamicItem.SupportList.Where(s => s.UserId == userid).ToList().Count > 0 ? true : false,
                RemindUsers = new List<int>(),
                CompanyName=dynamicItem.CompanyName
            };
        }
        #endregion

        #region 添加投票
        [HttpPost]
        public JsonResult VoteAdd(VoteAddReq parame)
        {
            PublicUserModel loginUser=this.GetLoginUser();
            VoteSurveyName newVote = new VoteSurveyName();
            List<VoteOptionName> optioNameList=new List<VoteOptionName>();
            newVote.CityId = loginUser.CityID;
            newVote.EndTime = parame.EndTime;
            newVote.UserId = loginUser.UserID;
            newVote.SurveyName = parame.SurveyName;
            newVote.SurveyType=parame.SurveyType;
            newVote.ViewData=parame.ViewData;
            optioNameList = jss.Deserialize<List<VoteOptionName>>(parame.OptionNameList);
            int result= voteBll.VoteAdd(newVote, optioNameList);
            return Json(new { status = result });
        }
        #endregion

        #region 获取动态里面的投票列表
        [HttpPost]
        public JsonResult GetVoteResult(string ids)
        {
            PublicUserModel loginUser=new PublicUserModel();
            loginUser = this.GetLoginUser();
            List<int> VoteIds = new List<int>();
            if (!string.IsNullOrEmpty(ids))
            {
                foreach (string item in ids.Split(',').ToList())
                {
                    int id = 0;
                    int.TryParse(item, out id);
                    VoteIds.Add(id);
                }
            }

            List<VoteSurveyName> voteList = voteBll.GetSurveyNameListByIds(VoteIds);
            List<VoteOptionName> voteOptionList = new List<VoteOptionName>();
            List<VoteSurveySubmitRecord> voteSubmitRecordList = new List<VoteSurveySubmitRecord>();
            if (voteList.IsNoNull())
            {
                voteOptionList = voteBll.GetOptionNameListByVoteIds(VoteIds);
                voteSubmitRecordList = voteBll.GetSubmitRecordListByVoteIds(VoteIds);
                var resultList = voteList.Select(s => new
                {
                    s.VoteId,
                    s.SurveyName,
                    s.SurveyType,
                    countdown = GetSyCount(((DateTime)s.EndTime)),
                    VoteNum=s.VoteNum==null?0:s.VoteNum,
                    s.UserId,
                    isGuoqi = s.EndTime > DateTime.Now ? 0 : 1,
                    isSubmit = voteSubmitRecordList.Where(r => r.UserId == loginUser.UserID && r.VoteId == s.VoteId).ToList().Count > 0 ? 1 : 0,
                    ViewData=(s.ViewData==2&&s.UserId!=loginUser.UserID)?-1:s.ViewData,
                    OptionList = voteOptionList.Where(o => o.VoteId == s.VoteId).Select(o => new
                    {
                        o.VoteId,
                        VoteNum=o.VoteNum==null?0:o.VoteNum,
                        o.OptionName,
                        o.OptionId,
                        percent=  o.VoteNum > 0 ? Math.Round((Double)((double)o.VoteNum * 100 / (s.VoteNum)), 2, MidpointRounding.AwayFromZero).ToString() : "0",
                        voteThis = voteSubmitRecordList.Where(r => r.UserId == loginUser.UserID && r.VoteId == s.VoteId&&r.OptionId==o.OptionId).ToList().Count>0?1:0
                    })   
                });
                return Json(resultList);
            }
           
            return Json(VoteIds);
        }
        private string GetSyCount(DateTime endtime)
        {
            string sycount;
            DateTime nowDt = DateTime.Now;
            DateTime endDt = endtime;
            TimeSpan ts = endDt - nowDt;
            if (ts.Days > 0)
            {
                sycount = ts.Days.ToString() + "天";
            }
            else
            {
                sycount = ts.Hours + "时" + ts.Minutes + "分" + ts.Seconds + "秒";
            }
            return sycount;
        }
        #endregion

        #region 提交投票
        public JsonResult SubmitVote(int voteId, string options, string dynamicContent,int isChange=0)
        {
            PublicUserModel loginUser=this.GetLoginUser();
            List<VoteOptionName> optionList = new List<VoteOptionName>();
            optionList = jss.Deserialize<List<VoteOptionName>>(options);
            int dynamicId = 0;
            int rows = voteBll.SubmitVote(voteId, loginUser.UserID, optionList,isChange, ref dynamicId);
            if (rows > 0 && dynamicId>0)
            {
                dynamicBll.DynamicCommentAdd(new DynamicModel
                {
                    Id = dynamicId,
                    UserId = loginUser.UserID,
                    DynamicContent = dynamicContent
                });
            }
            return Json(new { status=rows});
        }
        #endregion

        #region 赞和取消赞
        public JsonResult DynamicSupportAdd(int dynamicId)
        {
            PublicUserModel loginUser = this.GetLoginUser();
            int rows=dynamicBll.DynamicSupportAdd(loginUser.UserID,dynamicId);
            return Json(new { status = rows });
        }
        #endregion

        #region 右侧模块
        /// <summary>
        /// 首页->使用简报
        /// </summary>
        /// <returns></returns>
       [IgnoreValidate]
        public ActionResult UseReport()
        {
            UserReportModel report = userBll.GetUserReport(this.GetLoginUser().UserID);
            return View(report);
        }

        /// <summary>
        /// 首页公告
        /// </summary>
        /// <returns></returns>
       [IgnoreValidate]
        public ActionResult HomeNoticeListView(int ps = 5)
        {
            List<Notice> noticeList = ncBase.CurrentEntities.Notice.Where(n => n.Type != 0).OrderByDescending(n => n.CreateTime).Take(ps).ToList();
            return View(noticeList);
        }
        #endregion

        #region 动态删除 评论删除
        /// <summary>
        /// 动态删除
        /// </summary>
        /// <param name="dynamicId"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult DeleteDynamic(int dynamicId)
        {
            PublicUserModel loginUser = this.GetLoginUser();
            Dynamic item = ncBase.CurrentEntities.Dynamic.Where(d => d.Id == dynamicId && d.UserId == loginUser.UserID).FirstOrDefault();
            if (item.IsNoNull())
            {
                item.State = 0;
                item.Operator = loginUser.UserID;
                ncBase.CurrentEntities.SaveChanges();
                return Json(new { status = 1 });
            }
            return Json(new { status = 0 });

        }
        /// <summary>
        /// 评论删除
        /// </summary>
        /// <param name="dynamicId"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult DeleteDynamicComment(int commentId)
        {
            PublicUserModel loginUser = this.GetLoginUser();
            DynamicComment item = ncBase.CurrentEntities.DynamicComment.Where(d => d.Id == commentId && d.UserId == loginUser.UserID).FirstOrDefault();
            if (item.IsNoNull())
            {
                item.Status = 0;
                Dynamic thisDynamic = ncBase.CurrentEntities.Dynamic.Where(d => d.Id == item.DynamicId).FirstOrDefault();
                if (thisDynamic.IsNoNull())
                {
                    thisDynamic.CommentNum = thisDynamic.CommentNum-1;
                }
                ncBase.CurrentEntities.SaveChanges();
                return Json(new { status = 1 });
            }
            return Json(new { status = 0 });

        }
        #endregion

        #region 动态和评论内容的包装,如表情
        /// <summary>
        /// 获取评论的表情
        /// </summary>
        /// <param name="detail"></param>
        /// <returns></returns>
        private  string GetContentFace(string detail)
        {
            if (string.IsNullOrEmpty(detail))
                return string.Empty;
            List<DynamicFace> faceList = GetFaceList();
            MatchCollection matches = Regex.Matches(detail, @"\[\w+!*\]");
            foreach (Match m in matches)
            {
                string wk = m.ToString().Trim();
                wk = wk.Replace("[", "").Replace("]", "");
                string faceUrl = "";
                var selList = faceList.Where(f => f.Faceword == wk).ToList();
                if (selList.Count > 0)
                {
                    faceUrl = selList[0].Faceurl.ToString();
                }
                if (!string.IsNullOrEmpty(faceUrl))
                {
                    detail = detail.Replace(m.ToString(), "<span class='face_Img'><img src='" + faceUrl+ "' /></span>");
                }
            }
            return detail;
        }
        private List<DynamicFace> GetFaceList()
        {
            List<DynamicFace> faceList = HttpContext.Cache["faceList"] as List<DynamicFace>;
            if (faceList == null)
            {
                faceList = ncBase.CurrentEntities.DynamicFace.ToList();
                HttpContext.Cache.Insert("faceList", faceList, null, DateTime.Now.AddMinutes(10), TimeSpan.Zero);
            }
            return faceList;
        }
        #endregion

        #region 表情列表
        [OutputCache(Duration =12* 60 * 60)]
        public JsonResult GetFaceList(int typeId=0)
        {
            List<DynamicFace> faceList = GetFaceList();
            if (typeId > 0) {
                faceList = faceList.Where(f => f.Facetype == typeId).OrderBy(o=>o.Orderindex).ToList();
            }
            return Json(faceList,JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region 分享
        [ActionLog(CheckPoints=false)]
        public ActionResult FenXiang()
        {
            PublicUserModel loginUser = this.GetLoginUser();
            return View(loginUser);
        }
        #endregion

        #region 签到看排行
       [IgnoreValidate]
        public ActionResult SignTop(int pi=1)
        {
            PublicUserModel loginUser = this.GetLoginUser();
            int totalSize = 0;
            int ps = 10;
            pi = pi > 0 ? pi : 1;
            List<UserSignTop> topList = userBll.GetTodayUserSignTop(pi,ps,ref totalSize);
            ViewBag.PageCount = (totalSize / ps) + ((totalSize % ps) == 0 ? 0 : 1);
            ViewBag.PageIndex = pi;
           //右侧统计数据
            SignRightStat rightStat = userBll.GetSignRightStat(loginUser.UserID);
            ViewBag.RightStat = rightStat;
            ViewBag.UserName = loginUser.Name;
            ViewBag.Portrait = loginUser.Portrait;
            return View(topList);
        }
        #endregion

        #region  积分排行
      
        public ActionResult PointTop()
        {
            PublicUserModel loginUser = this.GetLoginUser();
            ObjectResult<P_User_PointsTop_Result> pointTopList = ncBase.CurrentEntities.P_User_PointsTop(100);

            P_User_PointTopRight_Result userPoint = ncBase.CurrentEntities.P_User_PointTopRight(loginUser.UserID).FirstOrDefault();
            if(userPoint.IsNoNull()){
                ViewBag.RightPointStat = userPoint;
            }
            return View(pointTopList);
        }
      
        #endregion

        #region 首页右侧的积分排行
        public ActionResult PointRightTop()
        {
            List<PublicUser> userList = ncBase.CurrentEntities.PublicUser.Where(u => u.UserID > 1000021 && u.UserID != 1000160).OrderByDescending(u => u.Points).Take(5).ToList();
            return View(userList);
        }
        #endregion

        #region 获取邀请排行榜
       [IgnoreValidate]
        public ActionResult InviteList(int top = 5, int pi = 1, int ps=5)
        {
            int totalSize = 0;
             ps = ps > 0 ? ps :5;
             pi = pi > 0 ? pi : 1;
             List<UserSignTop> topList = userBll.GetInviteList(pi, ps, ref totalSize);
            ViewBag.PageCount = (totalSize / ps) + ((totalSize % ps) == 0 ? 0 : 1);
            ViewBag.PageIndex = pi;
            return View(topList);
        }
        #endregion
    }
}
