using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Http;
using Microsoft.Ajax.Utilities;
using ZJB.Api.BLL;
using ZJB.Api.Common;
using ZJB.Api.Entity;
using ZJB.Api.Models;
using ZJB.Core.Utilities;
using ZJB.WX.Filters;
using MongoDB.Driver;
using ZJB.WX.Models.Client;

namespace ZJB.WX.Controllers.Api
{
    public partial class HomeController : BaseController
    {

        private NCBaseRule ncBase = new NCBaseRule();
        private UserBll userBll = new UserBll();
        private  AcceptedClients clients;

        #region 首页
       [HttpGet]
        [Token]
        public ApiResponse GetIndex()
        {
            int pageSize = 5;
            UserReportModel report = userBll.GetUserReport(GetCurrentUserId());
            List<Notice> noticeList = ncBase.CurrentEntities.Notice.Where(n => n.Type != 0).OrderByDescending(n => n.CreateTime).Take(pageSize).ToList();

            var list = new
            {
                Report = report,
                NoticeList=noticeList!=null?noticeList.Select(s => new { 
                    s.NoticeId,
                    NoticeContent ="http://"+ HttpContext.Current.Request.Url.Host + "/notice/phoneview?noticeId=" + s.NoticeId,
                    s.Title,
                    CreateTime = DateTimeUtility.ToUnixTime(Convert.ToDateTime(s.CreateTime))
               
                }):null,
            };
            return new ApiResponse(Metas.SUCCESS, list);
        }
      
        #endregion

        #region 获取城市详情
        [HttpGet]
        public ApiResponse GetRegion(int cityId = 592, long lastUpdateUnixTime = 0)
        {

            Regions cityRegions = ncBase.CurrentEntities.Regions.Where(n => n.RegionID == cityId).FirstOrDefault();
            List<Regions> regionses = new List<Regions>();
            if (cityRegions.IsNoNull())
            {
                var lastUpdateTime = DateTimeUtility.ToUnixTime(Convert.ToDateTime(cityRegions.UpdateTime));

            
                if (lastUpdateUnixTime < lastUpdateTime)
                {   regionses.Add(cityRegions);
                    List<Regions> regions =
                        ncBase.CurrentEntities.Regions.Where(n => n.CityID == cityRegions.RegionID).ToList();
                    if (regions.IsNoNull())
                    {
                        regionses.AddRange(regions);
                    }
                }
            }
            var list = cityRegions != null
                ? new
                {
                    CityId = cityRegions.RegionID,
                    CityName = cityRegions.Name,
                    LastUpdateTime = DateTimeUtility.ToUnixTime(Convert.ToDateTime(cityRegions.UpdateTime)),
                    Distrct = regionses != null
                        ? regionses.Where(n => n.Layer == 2).Select(
                            n => new
                            {
                                DistrctID = n.RegionID,
                                DistrctName = n.Name,
                                Region = regionses.Where(m => m.Layer == 3 && m.DistrctID == n.RegionID).Select(
                                    m => new
                                    {
                                        m.RegionID,
                                        RegionName = m.Name
                                    }
                                    )
                            })
                        : null
                }
                : null;
                
         

            return new ApiResponse(Metas.SUCCESS, list);
        }

        #endregion

        #region 获取七牛Token
        /// <summary>
        /// 获取七牛Token
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ApiResponse GetQiniuToken()
        {
            const uint expire = 60 * 60 * 24 * 30;
            DateTime expirationDate = DateTime.Now.AddDays(30);
            string token = QiniuUtil.GetUpToken(expire);
            // WriteLog(token);
            var model = new QiniuToken
            {
                Token = token,
                ExpirationTime = DateTimeConverter.ConvertDateTimeToDouble(expirationDate)
            };
            return new ApiResponse(Metas.SUCCESS, model);
        }
        #endregion

        #region 检查系统更新
        [HttpGet] 
        public ApiResponse CheckUpdate()
        {
            try
            {
                var request = HttpContext.Current.Request;
                if (clients == null || clients.Clients == null || clients.Clients.Count == 0)
                {
                    string config = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"Config/AcceptedClients.config");
                    using (var reader = new StreamReader(config, Encoding.UTF8))
                    {
                        string xml = reader.ReadToEnd();
                        clients = XmlUtility.Deserialize<AcceptedClients>(xml);
                    }
                }
                string userAgent = request.UserAgent;
                if (string.IsNullOrEmpty(request.UserAgent))
                {
                    userAgent = request.Headers["User-Agent"];
                }
                var client = clients.Clients.FirstOrDefault(
                    x =>
                        string.Equals(x.UserAgent, userAgent,
                            StringComparison.OrdinalIgnoreCase));
                if (client == null) return new ApiResponse(Metas.SUCCESS, null);
                Client newestClient = client;

                if (!string.IsNullOrEmpty(client.AppName) && !string.IsNullOrEmpty(client.OS))
                {
                    var appClients =
                        clients.Clients.Where(x => (x.AppName == client.AppName && x.OS == client.OS));
                    newestClient = appClients.OrderByDescending(x => x.InternalVer).FirstOrDefault();
                    newestClient.Update = newestClient.Update > 0 && newestClient.InternalVer > client.InternalVer
                        ? newestClient.Update
                        : 0;
                    newestClient.InternalVer = newestClient.Update > 0 ? newestClient.InternalVer : client.InternalVer;
                    newestClient.Version = newestClient.Update > 0
                        ? newestClient.UserAgent.Substring(newestClient.UserAgent.IndexOf("Ver",
                            StringComparison.Ordinal))
                        : client.UserAgent.Substring(newestClient.UserAgent.IndexOf("Ver", StringComparison.Ordinal));

                    if (newestClient.Update > 0 && newestClient.Update == 2 &&
                        !string.IsNullOrEmpty(newestClient.ForceVer))
                    {
                        var forceVers = newestClient.ForceVer.Split(',');
                        if (!forceVers.Contains<String>(client.InternalVer.ToString()))
                            newestClient.Update = 1;
                    }
                }
                else
                {
                    newestClient.Update = 0;
                    newestClient.Version =
                        newestClient.UserAgent.Substring(newestClient.UserAgent.IndexOf("Ver", StringComparison.Ordinal));
                }

                return new ApiResponse(Metas.SUCCESS,
                    new
                    {
                        newestClient.AppName,
                        newestClient.ChangeLog,
                        newestClient.FileName,
                        newestClient.FileSize,
                        newestClient.Update,
                        newestClient.Version,
                        newestClient.InternalVer,
                        newestClient.PublishDate,
                        newestClient.UpdateUrl
                    });
            }
            catch (Exception e)
            {
               
                return new ApiResponse(Metas.UNKNOWN_ERROR, null);
            }
        }
        #endregion

        #region 分享回调

        [HttpPost]
        [Token]
        public ApiResponse CallbackShareInfo([FromBody]ShareInfoReq model)
        {

            ShareLog shareLog =new ShareLog();
            shareLog.AddTime = DateTime.Now;
            shareLog.ShareContent = model.ShareContent;
            shareLog.ShareImgUrl = model.ShareImgUrl;
            shareLog.ShareTitle = model.ShareTitle;
            shareLog.ShareUrl = model.ShareUrl;
            shareLog.Source = model.Source;
            shareLog.UserId = GetCurrentUserId();
            ncBase.CurrentEntities.AddToShareLog(shareLog);
            ncBase.CurrentEntities.SaveChanges();

            int gainPoints = 0;
            string gainPointsMsg = "";
            DoTask(GetCurrentUserId(), PointsEnum.First_ShareHouse_App, out gainPoints);
            DoTask(GetCurrentUserId(), PointsEnum.EveryDay_ShareHouse_App, out gainPoints);
            gainPointsMsg = "完成“每日分享”任务";
            var result = new
            {
                GainPoints = gainPoints,
                GainPointsMsg = gainPointsMsg
            };
            return new ApiResponse(Metas.SUCCESS, result);
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
