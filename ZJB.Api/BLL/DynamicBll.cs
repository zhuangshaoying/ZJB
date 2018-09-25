using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ZJB.Api.DAL;
using ZJB.Api.Entity;
using ZJB.Api.Models;
using ZJB.Core.Injection;

namespace ZJB.Api.BLL
{
    public class DynamicBll
    {
        private readonly DynamicDal dynamicDal = Container.Instance.Resolve<DynamicDal>();

       #region 发布动态和回复
        /// <summary>
        /// 发布动态
        /// </summary>
        /// <param name="item"></param>
        /// <returns>返回动态id 小于0 则失败</returns>
        public virtual int DynamicAdd(DynamicModel item)
        {
            return dynamicDal.DynamicAdd(item);
        }
        /// <summary>
        /// 发布回复
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public virtual int DynamicCommentAdd(DynamicModel item)
        {
            return dynamicDal.DynamicCommentAdd(item);
        }
       #endregion
        #region 动态列表和回复列表
        /// <summary>
        /// 动态列表
        /// </summary>
        /// <param name="parame"></param>
        /// <returns></returns>
        public virtual List<DynamicModel> DynamicList(DynamicListReq parame)
        {
            return dynamicDal.DynamicList(parame);
        }
        /// <summary>
        /// 回复列表
        /// </summary>
        /// <param name="dynamicIds"></param>
        /// <returns></returns>
        public virtual List<DynamicModel> DynamicReplayListBydynamicIds(string dynamicIds,int pageSize=5,int lastId=0)
        {
            return dynamicDal.DynamicReplayListBydynamicIds(dynamicIds, pageSize, lastId);
        }
        #endregion
        #region  #region 动态的图片列表
        /// <summary>
        /// 动态的图片列表
        /// </summary>
        /// <param name="synamicIds"></param>
        /// <returns></returns>

        public virtual List<DynamicImage> DynamicImageListBydynamicIds(List<int> dynamicIds)
        {
            return dynamicDal.DynamicImageListBydynamicIds(dynamicIds);
        }
        #endregion
        #region 赞和取消赞
        public virtual int DynamicSupportAdd(int userId, int dynamicId)
        {
            return dynamicDal.DynamicSupportAdd(userId, dynamicId,-1);
        }
        public virtual int DynamicSupportAdd(int userId, int dynamicId,int status)
        {
            return dynamicDal.DynamicSupportAdd(userId, dynamicId, status);
        }
        #endregion
        #region 置顶和取消置顶
        /// <summary>
        /// 
        /// </summary>
        /// <param name="lybid"></param>
        /// <param name="otype">1是取消 0是置顶</param>
        /// <param name="days">天数</param>
        /// <returns></returns>
        public virtual int DynamicSetTop(int lybid, int otype, int days)
        {
            return dynamicDal.DynamicSetTop(lybid, otype, days);
        }
        #endregion
    }
}
