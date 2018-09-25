using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Caching;
using System.Web.Configuration;
using System.Web.Http;
using log4net;
using ZJB.Api.BLL;
using ZJB.Api.Entity;
using ZJB.Api.Models;
using ZJB.Core.Injection;
using ZJB.Core.Utilities;

using ZJB.WX.Filters;
using ZJB.WX.Models;
using ZJB.WX.Models.Client;


namespace ZJB.WX.Controllers.Api
{
    public class SocialController : BaseController
    {
        private NCBaseRule ncBase = new NCBaseRule();
        private DynamicBll dynamicBll = new DynamicBll();
        private VoteSurveyBll voteBll = new VoteSurveyBll();
        private UserBll userBll = new UserBll();
    System.Web.Script.Serialization.JavaScriptSerializer jss = new System.Web.Script.Serialization.JavaScriptSerializer();
        #region 动态列表和回复列表

        /// <summary>
        /// 动态列表
        /// </summary>
     /// <returns></returns>
        [HttpGet]
        [Token]
    public ApiResponse DynamicList(int pageSize = 10, long lastTime = 253367971200,int pageIndex = 1)
        {
            int uid = GetCurrentUserId();
            DynamicListReq parame= new DynamicListReq();
            parame.PageSize = pageSize;

            if (lastTime == 0)
            {parame.LastTime = DateTime.Now;}
            else
            {parame.LastTime = DateTimeUtility.FromUnixTime(lastTime);}

            parame.UserId = uid;
            parame.IsGetSupport = 1;
            parame.FirstComming = pageIndex ==1? 1 : 0;
            List<DynamicModel> dynamicList = dynamicBll.DynamicList(parame);

            List<DynamicImage> dynamicImageList = new List<DynamicImage>();
             List<DynamicModel> dynamicReplayList=new List<DynamicModel>();
            if (dynamicList.IsNoNull() && dynamicList.Count > 0)
            {
                dynamicImageList = dynamicBll.DynamicImageListBydynamicIds(dynamicList.Select(s => s.Id).ToList());


               List<int> list= dynamicList.Select(s => s.Id).ToList();
                string ids = "";
                foreach (int id in list)
                {
                    ids += "," + id;
                }
                if (ids.StartsWith(","))
                    ids = ids.Substring(1);
                dynamicReplayList = dynamicBll.DynamicReplayListBydynamicIds(ids, 5, 0);
            
              

            }
            if (dynamicList != null && dynamicList.Count > 0)
            {
                return new ApiResponse(Metas.SUCCESS, FormatDynamicList(dynamicList, dynamicImageList, dynamicReplayList,uid));
             
            }
            return new ApiResponse(Metas.SUCCESS);
        }
        /// <summary>
        /// 回复列表
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Token]
        public ApiResponse DynamicReplayList(string dynamicIds, int pageSize = 5, int lastId = 0)
        {
            List<string> dynamicIdList = string.IsNullOrEmpty(dynamicIds) ? new List<string>() : dynamicIds.Split(',').ToList();
            List<DynamicModel> dynamicReplayList = dynamicBll.DynamicReplayListBydynamicIds(dynamicIds, pageSize, lastId);
            var resultList = dynamicIdList.Select(c => new
            {
                DynamicId = c,
                CommentList = (dynamicReplayList != null && dynamicReplayList.Count > 0)
                ?
                dynamicReplayList.Where(o => o.ReplayId.ToString() == c).Select(r => new
                {
                    AddTime = DateTimeUtility.GetDisplayTime(r.AddTime),
                    r.DynamicContent,
                    WrapComment = GetContentFace(r.DynamicContent),
                    r.UserId,
                    r.UserName,
                    r.Portrait,
                    CommentId=r.Id,
                    r.ReplyCommentId,
                    r.ReplayUserId,
                    r.ReplayUserName
                })
                : null
            });
            return new ApiResponse(Metas.SUCCESS, resultList);
        }

        #endregion

        #region 添加动态和回复
        /// <summary>
        /// 添加动态
        /// </summary>
        /// <param name="parame"></param>
        /// <param name="ImageList"></param>
        /// <returns></returns>
       [HttpPost]
       [Token]
        public ApiResponse AddDynamic([FromBody]DynamicPostReq parame)
        {
            if (string.IsNullOrEmpty(parame.detail))
            {
             

                return new ApiResponse(Metas.Content_Null);
            }
            Credential  loginUser=Request.GetCredential();

            List<DynamicImage> ImageList = !string.IsNullOrEmpty(parame.ImageList)?jss.Deserialize<List<DynamicImage>>(parame.ImageList):new List<DynamicImage>();
            DynamicModel dynamicItem = new DynamicModel()
            {
                UserId = loginUser.UserID,
                ImageList = ImageList,
                DynamicContent = parame.detail
            };
            int dynamicId = dynamicBll.DynamicAdd(dynamicItem);
            dynamicItem.Id = dynamicId;
            dynamicItem.UserName = loginUser.Name;
            dynamicItem.Portrait = loginUser.Icon;
            dynamicItem.AddTime = DateTime.Now;
            
          return new ApiResponse(Metas.SUCCESS,FormatDyamic(dynamicItem, ImageList, new List<DynamicModel>(), loginUser.UserID));
        }
        /// <summary>
        /// 添加回复
        /// </summary>
        /// <returns></returns>
       [HttpPost]
       [Token]
        public ApiResponse AddDynamicComment([FromBody]DynamicPostReq parame)
        {
            if (string.IsNullOrWhiteSpace(parame.detail))
            {
             return new ApiResponse(Metas.Content_Null);
            }
              Credential  loginUser=Request.GetCredential();

              List<DynamicImage> ImageList = !string.IsNullOrEmpty(parame.ImageList) ? jss.Deserialize<List<DynamicImage>>(parame.ImageList) : new List<DynamicImage>();
            DynamicModel dynamicItem = new DynamicModel()
            {
                UserId = loginUser.UserID,
                ImageList = ImageList,
                DynamicContent = parame.detail,
                Id = parame.DynamicId,
                ReplyCommentId = parame.CommentId
            };
            int dynamicReplayId = dynamicBll.DynamicCommentAdd(dynamicItem);

            return new ApiResponse(Metas.SUCCESS, new { CommentId = dynamicReplayId });
        }
        #endregion

        #region 返回的动态列表格式化
        private dynamic FormatDynamicList(List<DynamicModel> dynamicList, List<DynamicImage> dynamicImageList,List<DynamicModel> dynamicModels, int userid)
        {
            return (from item in dynamicList select FormatDyamic(item, dynamicImageList.Where(i => i.DynamicId == item.Id).ToList(), dynamicModels.Where(i => i.ReplayId == item.Id).ToList(), userid));
        }
        private dynamic FormatDyamic(DynamicModel dynamicItem, List<DynamicImage> dynamicImageList,List<DynamicModel> dynamicModels, int userid)
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
                WrapComment = GetContentFace(dynamicItem.DynamicContent),
                DynamicId=dynamicItem.Id,
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
                IsTop = (dynamicItem.TopTime != null && dynamicItem.TopTime > DateTime.Now) ? 1 : 0,
                dynamicItem.LastCommentId,
                LastCommentTime = DateTimeUtility.ToUnixTime(dynamicItem.LastCommentTime),
                dynamicItem.Lat,
                dynamicItem.Lng,
                dynamicItem.Location,
                dynamicItem.ShareNum,
                dynamicItem.State,
                dynamicItem.Title,
                TopTime = DateTimeUtility.ToUnixTime(dynamicItem.TopTime),
                dynamicItem.Type,
                dynamicItem.UserId,
                dynamicItem.Visible,
                dynamicItem.VoteId,
                dynamicItem.UserName,
                Portrait =string.IsNullOrEmpty(dynamicItem.Portrait)?"":dynamicItem.Portrait + "?imageMogr2/strip|imageView2/1/w/42/h/42/q/85",
                SupportList = dynamicItem.SupportList != null ? dynamicItem.SupportList.Select(s => new
                {
                    s.UserName,
                    s.UserId,
                    s.Id,
                    s.DynamicId
                }) : null,
                ReplayList = dynamicModels.Select(r => new
                {
                    AddTime = DateTimeUtility.GetDisplayTime(r.AddTime),
                    r.DynamicContent,
                    WrapComment = GetContentFace(r.DynamicContent),
                    r.UserId,
                    r.UserName,
                    r.Portrait,
                    CommentId=r.Id,
                    ReplyCommentId= r.ReplyCommentId,
                    ReplayUserId= r.ReplayUserId,
                    ReplayUserName=r.ReplayUserName
                }),
                IsSupport = dynamicItem.SupportList != null && dynamicItem.SupportList.Where(s => s.UserId == userid).ToList().Count > 0 ? true : false,
                RemindUsers = new List<int>()
            };
        }
        #endregion

         #region 动态删除 评论删除
        /// <summary>
        /// 动态删除
        /// </summary>
        /// <param name="dynamicId"></param>
        /// <returns></returns>
        [HttpGet]
        [Token]
        public ApiResponse DeleteDynamic(int dynamicId)
        {
           Credential  loginUser=Request.GetCredential();
            Dynamic item = ncBase.CurrentEntities.Dynamic.Where(d => d.Id == dynamicId && d.UserId == loginUser.UserID).FirstOrDefault();
            if (item.IsNoNull())
            {
                item.State = 0;
                item.Operator = loginUser.UserID;
                ncBase.CurrentEntities.SaveChanges();
              return new ApiResponse(Metas.SUCCESS);
            }
          return new ApiResponse(Metas.UNKNOWN_ERROR);

        }
        /// <summary>
        /// 评论删除
        /// </summary>
        /// <param name="dynamicId"></param>
        /// <returns></returns>
        [HttpGet]
        [Token]
        public ApiResponse DeleteDynamicComment(int commentId)
        {
            Credential  loginUser=Request.GetCredential();
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
                  return new ApiResponse(Metas.SUCCESS);
            }
            return new ApiResponse(Metas.UNKNOWN_ERROR);
        }
        #endregion

        #region 赞和取消赞
        [HttpGet]
        [Token]
        public ApiResponse DynamicSupportAdd(int dynamicId,int  status)
        {

            int rows = dynamicBll.DynamicSupportAdd(GetCurrentUserId(), dynamicId, status);
            return new ApiResponse(Metas.SUCCESS);
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
            string url = "http://" + HttpContext.Current.Request.Url.Host;
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
                    detail = detail.Replace(m.ToString(), "<span class='face_Img'><img src='"+url + faceUrl+ "' /></span>");
                }
            }
            return detail;
        }
        private List<DynamicFace> GetFaceList()
        {

            List<DynamicFace> faceList = HttpContext.Current.Cache["faceList"] as List<DynamicFace>;
            if (faceList == null)
            {
                faceList = ncBase.CurrentEntities.DynamicFace.ToList();
                HttpContext.Current.Cache.Insert("faceList", faceList, null, DateTime.Now.AddMinutes(10), TimeSpan.Zero);
            }
            return faceList;
        }
        #endregion

        #region 表情列表
        public ApiResponse GetFaceList(int typeId = 0)
        {
            List<DynamicFace> faceList = GetFaceList();
            if (typeId > 0) {
                faceList = faceList.Where(f => f.Facetype == typeId).OrderBy(o=>o.Orderindex).ToList();
            }
          

   return new ApiResponse(Metas.SUCCESS,faceList);
        }
        #endregion

        #region 私有方法
        /// <summary>
        /// 获取当前登录的用户ID
        /// </summary>
        /// <returns></returns>
        private int GetCurrentUserId()
        {
            var credential = Request.GetCredential();
            int userId = 0;
            if (credential != null)
            {
                userId = credential.UserID;
            }
            return userId;
        }
        #endregion
    }
}
