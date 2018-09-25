using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Configuration;
using System.Web.Http;
using log4net;
using ZJB.Api;
using ZJB.Api.BLL;
using ZJB.Api.Entity;
using ZJB.Api.Model;
using ZJB.Api.Models;
using ZJB.Core.Injection;
using ZJB.Core.Utilities;
using ZJB.WX.Common;
using ZJB.WX.Common.UserVerifiler;
using ZJB.WX.Filters;

using ZJB.WX.Models.Client;


namespace ZJB.WX.Controllers.Api
{
    public class NewsController : BaseController
    {
        private ILog logger = LogManager.GetLogger("NewsLogger");
        private readonly NewsBll newsbll= Container.Instance.Resolve<NewsBll>();

        private UserTaskLogBll userTaskLog = new UserTaskLogBll();
        private NCBaseRule ncBase = new NCBaseRule();

        #region 获取新闻列表
        /// <summary>
        /// 获取新闻列表  
        /// </summary>
        /// <returns>json字符串</returns>
        [HttpGet]
        [IgnoreValidate]
        public ApiResponse GetNewsList(string cids="1705",int pagesize=10,int pageindex=1)
        {
            var channelids = cids;
            int count = 0;
            var wcmlist = newsbll.GetWcmDocumentsListByChanleIds(channelids, pageindex, pagesize,ref count);

            var list = new List<MobileNews>();
            if (wcmlist != null)
            {
                list.AddRange(from item in wcmlist
                              let imgurl = newsbll.GetNewsFirstImagePath(item.DOCPUBURL, item.DOCHTMLCON)
                              select new MobileNews()
                              {
                                  NewsId = item.DOCID,
                                  Title = item.DOCTITLE,
                                  ShowImg = string.IsNullOrEmpty(imgurl) ? "http://m.ZJB.com/images/news_default.jpg" : imgurl,
                                  Replaytime = DateTimeUtility.ToUnixTime(Convert.ToDateTime(item.DOCRELTIME)),
                                  Hits = item.HitsCount,
                                  Pls = item.replaynum,
                              });
            }
            return new ApiResponse(Metas.SUCCESS, list, count);
        }
        #endregion

        #region 获取新闻详细
        /// <summary>
        /// 获取新闻详细  
        /// </summary>
        /// <returns>json字符串</returns>
        [HttpGet]
        [IgnoreValidate]
        public ApiResponse GetNewsDetail(int id=0)
        {
            var count = 0;
           var newdetail=  newsbll.GetWcmDocumentDetailById(id);
            if (newdetail.IsNoNull() && newdetail.DOCID > 0)
            {
                count = 1;
            }
            var result = new
            {
                newdetail.DOCID,
               DOCCONTENT=GetNewsContent(newdetail.DOCPUBURL,newdetail.DOCHTMLCON),
                newdetail.DOCTITLE,
                DOCPUBTIME=DateTimeUtility.ToUnixTime(Convert.ToDateTime(newdetail.DOCPUBTIME)),
                newdetail.HitsCount,
                ReplayNum= newdetail.replaynum,
                
            };
            return new ApiResponse(Metas.SUCCESS, result, count);
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

        private static string GetNewsContent(string DocUrl,string content)
        {
            content = content.ToLower();
            content = content.Replace("height=", "").Replace("HEIGHT=", "").Replace("width=", "").Replace("WIDTH=", "");
            string newsurl = DocUrl;
            if (newsurl.IndexOf("/t", StringComparison.Ordinal) > 0)
            {
                newsurl = newsurl.Remove(newsurl.IndexOf("/t", System.StringComparison.Ordinal));
            }
            content = content.Replace("src=\"", "src=\"" + newsurl + "/").Replace("SRC=\"", "src=\"" + newsurl + "/");


            return NoHtml(content);
        }
        private static string NoHtml(string htmlstring)
        {
            //删除脚本
            htmlstring = Regex.Replace(htmlstring, @"<script[^>]*?>.*?</script>", "",
              RegexOptions.IgnoreCase);
            //删除HTML
            // Htmlstring = Regex.Replace(Htmlstring, @"<(img[^>]*)>", "",RegexOptions.IgnoreCase);
            htmlstring = Regex.Replace(htmlstring, @"<(/img[^>]*)>", "",
              RegexOptions.IgnoreCase);
            htmlstring = Regex.Replace(htmlstring, @"<(iframe[^>]*)>", "",
            RegexOptions.IgnoreCase);
            htmlstring = Regex.Replace(htmlstring, @"<(/iframe[^>]*)>", "",
              RegexOptions.IgnoreCase);
            htmlstring = Regex.Replace(htmlstring, @"([\r\n])[\s]+", "",
              RegexOptions.IgnoreCase);
            htmlstring = Regex.Replace(htmlstring, @"-->", "", RegexOptions.IgnoreCase);
            htmlstring = Regex.Replace(htmlstring, @"<!--.*", "", RegexOptions.IgnoreCase);
            htmlstring = Regex.Replace(htmlstring, @"&(quot|#34);", "\"",
              RegexOptions.IgnoreCase);
            htmlstring = Regex.Replace(htmlstring, @"&(amp|#38);", "&",
              RegexOptions.IgnoreCase);
            htmlstring = Regex.Replace(htmlstring, @"&(nbsp|#160);", "   ",
              RegexOptions.IgnoreCase);

            htmlstring = Regex.Replace(htmlstring, @"&(copy|#169);", "\xa9",
              RegexOptions.IgnoreCase);


            // Htmlstring = HttpContext.Current.Server.HtmlEncode(Htmlstring).Trim();

            return htmlstring;
        }
        #endregion
    }
}
