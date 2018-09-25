using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Configuration;
using Senparc.Weixin.MP.AdvancedAPIs;
using Senparc.Weixin.MP.AdvancedAPIs.TemplateMessage;
using Senparc.Weixin.MP.Containers;
using ZJB.Core.Logging;


namespace ZJB.Api.Common
{
    public class WeiXinPublic
    {
        private string s_appid = ConfigurationManager.AppSettings["WeixinAppId"]; //服务号
        private string wx_secret = ConfigurationManager.AppSettings["WeixinAppSecret"];
        private Logger logger = new Logger("ExceptionLog");
                
        //标题委托房源通知
        //private string OrderTemp = "51o9MS_hB0RCXdciqIlqRZ0C58g6LjtA3Fp2Ph7cFHw";
        //private string OrderTemp = "TuGoUsJWShChs6dU9Lp7Kf3gg_Qi8AsBfk9dSpEMB4I";

        /// <summary>
        /// 直约业主
        /// </summary>
        //private string BespokeTemp = "AQa1EnpKlwIqX6DqIUw1jibkHYq9-Ft9NEUdFbavHdk";
        //private string BespokeTemp = "TuGoUsJWShChs6dU9Lp7Kf3gg_Qi8AsBfk9dSpEMB4I";

        public virtual SendTemplateMessageResult SendOrderStatusChangeMessag(string appId, string secret, string openId,
            string templateId, object temp, string url = "", string color = "#FF0000")
        {
            SendTemplateMessageResult result = new SendTemplateMessageResult();
            int i = 0;
            do
            {
                try
                {
                    i++;
                    logger.Debug("第"+i+"次 发送...");
                    //Task.Factory.StartNew(() => Senparc.Weixin.MP.AdvancedAPIs.TemplateApi.SendTemplateMessageAsync(appId, openId,
                    //        templateId, url, temp));

                    result = TemplateApi.SendTemplateMessage(appId, openId, templateId, url, temp);
                    //result = Senparc.Weixin.MP.AdvancedAPIs.TemplateApi.SendTemplateMessageAsync(appId, openId, templateId, url, temp).Result;
                    if (result != null && result.msgid > 0)
                    {
                        break;
                    }
                }
                catch (Exception ex)
                {
                    logger.Debug("发送消息"+i+" :"+ ex);
                }
            } while (i < 3);
            return result;
        }

        /// <summary>
        /// 消息推送
        /// </summary>
        /// <param name="openid"></param>
        /// <param name="first"></param>
        /// <param name="keyword1"></param>
        /// <param name="keyword2"></param>
        /// <param name="keyword3"></param>
        /// <param name="keyword4"></param>
        /// <param name="url"></param>
        /// <param name="temp">模版ID</param>
        /// <param name="remark">其它,默认为:速去查看详情</param>
        public void WxPushMessage(string openid, string first = "您好，您有一条新的看房预约消息。", string keyword1 = "",
            string keyword2 = "", string keyword3 = "", string keyword4 = "", string url = "",
            string temp = "hNBPR6KYN_5I1_w8pbihKmRRTwMM-pba0AYo3sN8POQ"
            , string remark = "速去查看详情"
            )
        {
            logger.Debug("运行...");
            if (!string.IsNullOrEmpty(openid)) //判断表里面是否有服务号是否有openid，如果没有，去授权一下获取用户openid
            {
                try
                {
                    var newTemplate = new QuestionTemplateData()
                    {
                        first = new TemplateDataItem(first),
                        remark = new TemplateDataItem(remark),
                    };
                    if (!string.IsNullOrEmpty(keyword1))
                    {
                        newTemplate.keyword1 = new TemplateDataItem(keyword1);
                    }
                    if (!string.IsNullOrEmpty(keyword2))
                    {
                        newTemplate.keyword2 = new TemplateDataItem(keyword2);
                    }
                    if (!string.IsNullOrEmpty(keyword3))
                    {
                        newTemplate.keyword3 = new TemplateDataItem(keyword3);
                    }
                    if (!string.IsNullOrEmpty(keyword4))
                    {
                        newTemplate.keyword4 = new TemplateDataItem(keyword4);
                    }
                    logger.Debug("准备发送...OpenID：" + openid);
                    //  SendOrderStatusChangeMessag(s_appid, wx_secret, openid, temp, newTemplate, url);
                    Task.Factory.StartNew(() => SendOrderStatusChangeMessag(s_appid, wx_secret, openid, temp, newTemplate, url));
                }
                catch (Exception ex)
                {
                   
                    logger.Debug("发送消息:" + ex);
                }
            }
        }
    }

    public class TemplateOrderData
    {
        public TemplateDataItem first { get; set; }
        public TemplateDataItem tradeDateTime { get; set; } //问题编号
        public TemplateDataItem orderType { get; set; } //问题标题
        public TemplateDataItem customerInfo { get; set; } //事件类型
        public TemplateDataItem remark { get; set; }

    }


    public class QuestionTemplateData
    {
        public TemplateDataItem first { get; set; }
        public TemplateDataItem keyword1 { get; set; } //问题编号
        public TemplateDataItem keyword2 { get; set; } //问题标题
        public TemplateDataItem keyword3 { get; set; } //事件类型
        public TemplateDataItem keyword4 { get; set; } //发生时间
        public TemplateDataItem remark { get; set; }

    }
}
