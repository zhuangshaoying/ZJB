using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ZJB.Api.DAL;
using ZJB.Api.Models;
using ZJB.Core.Injection;

namespace ZJB.Api.BLL
{
    public class PostLogBll
    {
        private readonly PostLogDal postLogDal = Container.Instance.Resolve<PostLogDal>();


        #region  发布日志列表
        /// <summary>
        /// 发布日志列表
        /// </summary>
        /// <returns></returns>
        public virtual List<PostLogModel> GetPostLogList(PostLogListParames parames, ref int totalSize)
        {
            return postLogDal.GetPostLogList(parames,ref totalSize);
        }
        /// <summary>
        /// 发布日志--房源查看统计
        /// </summary>
        /// <param name="parames"></param>
        /// <param name="totalSize"></param>
        /// <returns></returns>
        public virtual List<PostLogModel> GetPostLogGroupByHouseId(PostLogListParames parames, ref int totalSize)
        {
            return postLogDal.GetPostLogGroupByHouseId(parames, ref totalSize);
        }
         /// <summary>
        /// 发布日志--网站查看统计
        /// </summary>
        /// <param name="parames"></param>
        /// <param name="totalSize"></param>
        /// <returns></returns>
        public virtual List<PostLogModel> GetPostLogGroupByWebSite(int userid)
        {
            return postLogDal.GetPostLogGroupByWebSite(userid);
        }
        /// <summary>
        /// 后台管理-发布日志--网站查看统计 --
        /// </summary>
        /// <param name="parames"></param>
        /// <param name="totalSize"></param>
        /// <returns></returns>
        public virtual List<PostLogModel> GetPostLogGroupByWebSiteAdmin(int userid, int cityId = 592)
        {
            return postLogDal.GetPostLogGroupByWebSiteAdmin(userid, cityId);
        }
        #endregion

        #region 后台管理 房源推送统计
        public virtual List<StatModel> GetPostStat(StatReq parame)
        {
           return postLogDal.GetPostStat(parame);
        }
        #endregion
        #region 后台管理 站点分析 --总的发布分布 成功分布 失败分布
        public virtual List<StatModel> GetSiteAnalyseData(StatReq parame)
        {
            return postLogDal.GetSiteAnalyseData(parame);
        }
        #endregion
    }
}
