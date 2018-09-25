using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ZJB.Api.Common;
using ZJB.Api.DAL;
using ZJB.Api.Entity;
using ZJB.Core.Injection;
using ZJB.Api.Models;
namespace ZJB.Api.BLL
{
    public class NoticeBll
    {
        private readonly NoticeDal noticeDal = Container.Instance.Resolve<NoticeDal>();
        public readonly string MySign = "【住家帮】";
        /// <summary>
        /// 获取用户未读的公告信息
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public virtual List<Notice> GetNotReadNoticeList(GetNotReadNoticeListReq parame,ref int totalSize)
        {
            return noticeDal.GetNotReadNoticeList(parame, ref totalSize);
        }

        public virtual List<Notice> GetNoticeList(GetNoticeListReq parame, ref int totalSize)
        {
            return noticeDal.GetNoticeList(parame, ref totalSize);
        }
        /// <summary>
        /// 设置为已读状态
        /// </summary>
        /// <param name="noticeId"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        public virtual int NoticeSetIsRead(int noticeId,int userId)
        {
            return noticeDal.NoticeSetIsRead(noticeId, userId);
        }

        #region 根据反馈id串 获取回复内容
        public List<Notice> GetReplayListByIds(string feedbackIds)
        {
            return noticeDal.GetReplayListByIds(feedbackIds);
        }
        #endregion
        /// <summary>
        /// 站内通知 如果type设置为0 不会显示再公告上，只有指定的receiverId私人才收的到
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="receiverId">不填表示群发</param>
        /// <returns></returns>
        public virtual int NoticeAdd(Notice entity, int receiverId = 0)
        {
           return noticeDal.NoticeAdd(entity, receiverId);
        }


        public virtual string SendUserCode(string tel, string Code,int min=5)
        {
            var result = "";

            if (!string.IsNullOrEmpty(tel))
            {

                SmsApi smsApi = new SmsApi();
                // 验证码:{1}，请在5分钟内完成输入验证码。
                string templet = "{0}您的验证码为{1}，请于{2}分钟内正确输入，如非本人操作，请忽略此短信。";
                string message = string.Format(templet, MySign, Code, min);
                result= smsApi.SendSmsQmy(tel, message);
            }
            return result;
        }
    }
}
