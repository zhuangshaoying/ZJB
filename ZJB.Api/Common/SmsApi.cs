using System;
using System.Linq;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Runtime.Remoting.Channels;
using System.Text;
using System.Web.Script.Serialization;
using Microsoft.Practices.ObjectBuilder2;
using ZJB.Core.Utilities;

namespace ZJB.Api.Common
{
    public class SmsApi
    {

        private QmySms _sms = new QmySms
        {
            AccountSID = "ba71d873b1464160859e6c1a0c7ea1e6",
            AuthToken = "b0ad6a51e088497e8c5efbe8069d25cc",
            Version = "20141029",
            AppID = "91d27940fe1a40b6a7edb203dd096294"
        };



        public string SendSmsQmy(string receivers, string message)
        {

            _sms.To = receivers;
            _sms.Param = message;
            try
            {
                return QmyHelper.NewSendSms(_sms);
            }
            catch (Exception ignore)
            {

                try
                {
                    return QmyHelper.NewSendSms(_sms);
                }
                catch (Exception ignore1)
                {

                    return "";
                }
            }
        }


        public SendResult SendSms(string receivers, string message, Purpose purpose = Purpose.All, string signature = "【住家帮】")
        {
            try
            {
                return Send("http://sms.ZJB.com/api/sms/send", receivers, message, purpose, signature);
            }
            catch (Exception ignore)
            {
                return Send("http://sms2.ZJB.com/api/sms/send", receivers, message, purpose, signature);
            }
        }
        public SendResult SendSms4ZJB(string receivers, string[] message, string templateId = "33357")
        {
            try
            {
                return SendCCP("http://sms.ZJB.com/api/CCPSms/send", message, receivers, AppId.ZJB, templateId, "");

            }
            catch (Exception ignore)
            {
                return new SendResult();
            }
        }
        public SendResult SendSms4ZJB(string receivers, string message)
        {
            string[] arrMessage = new[] { message, "5" };
            string templateId = "33357";
            try
            {
                return SendCCP("http://sms.ZJB.com/api/CCPSms/send", arrMessage, receivers, AppId.ZJB, templateId, "");

            }
            catch (Exception ignore)
            {
                return new SendResult();
            }
        }
        public SendResult SendSms4FcHezi(string receivers, string[] message, string templateId = "35926")
        {
            try
            {
                return SendCCP("http://sms.ZJB.com/api/CCPSms/send", message, receivers, AppId.FcHezi, templateId, "");

            }
            catch (Exception ignore)
            {
                return new SendResult();
            }
        }
        public SendResult SendSms4FcHezi(string receivers, string message)
        {
            string[] arrMessage = new[] { message, "5" };
            try
            {
                return SendCCP("http://sms.ZJB.com/api/CCPSms/send", arrMessage, receivers, AppId.FcHezi, "35926", "");

            }
            catch (Exception ignore)
            {
                return new SendResult();
            }
        }

        public  SendResult SendCCP( string[] message, string receivers, int appid, string templateId
            )
        {
            string url = "http://sms.ZJB.com/api/CCPSms/send";
            string signatur = "";
            return SendCCP(url, message, receivers, (AppId)appid, templateId, signatur);

        }

        public SendResult SendVerificationCode(string receiver, string message, string signature = "【LinLiShe.CN】")
        {
            try
            {
                return Send("http://sms.ZJB.com/api/sms/send", receiver, message, Purpose.VerificationCode, signature);
            }
            catch (Exception ignore)
            {
                return Send("http://sms2.ZJB.com/api/sms/send", receiver, message, Purpose.VerificationCode, signature);
            }
        }
        private static SendResult SendCCP(string url, string[] message, string receivers, AppId appId, string templateId, string signature)
        {
            NameValueCollection collection = new NameValueCollection();
            int index = 0;
            message.ForEach(p =>
            {
                collection.Add("ArrMessage[" + index + "]", p);
                index++;
            });
            collection.Add("Receivers", receivers);
            collection.Add("Signature", signature);
            collection.Add("TemplateId", templateId);
            collection.Add("AppId", appId.ToString());
            string json = HttpUtility.PostHtml(url, collection, Encoding.UTF8);
            var serializer = new JavaScriptSerializer();
            return serializer.Deserialize<SendResult>(json);
        }
        private static SendResult Send(string url, string receivers, string message, Purpose purpose, string signature)
        {
            NameValueCollection collection = new NameValueCollection();
            collection.Add("Message", message);
            collection.Add("Receivers", receivers);
            collection.Add("Signature", signature);
            collection.Add("Purpose", purpose.ToString());
            string json = HttpUtility.PostHtml(url, collection, Encoding.UTF8);
            var serializer = new JavaScriptSerializer();
            return serializer.Deserialize<SendResult>(json);
        }

        public  SendResult SendCnsms(string message, string receivers)
        {
            NameValueCollection collection = new NameValueCollection();
            var url = "http://api.cnsms.cn/";
            collection.Add("ac", "send");
            collection.Add("uid", "118323");
            collection.Add("pwd", StringUtility.ToMd5String("ZJB2015"));
            collection.Add("mobile", receivers);
            collection.Add("content", System.Web.HttpUtility.UrlEncode(message));
            string json = HttpUtility.PostHtml(url, collection, Encoding.UTF8);
            SendResult sendResult = new SendResult();
            if (json == "100")
                sendResult.Status = Status.Success;
            else
            {
                sendResult.Status = Status.Failure;
            }
            return sendResult;
        }
    }

    public class SendResult
    {
        public Status Status { get; set; }
        public List<string> Failures { get; set; }
    }

    public enum Purpose
    {
        //验证码
        VerificationCode = 1,
        //服务器告警
        ServerNotification = 2,
        //活动通知
        CampaignNotification = 4,
        //不区分
        All = 1023
    }
    public enum AppId
    {
        ZJB = 0,
        FcHezi = 1,
        LinLiShe = 2,
    }
    public enum Status
    {
        Success = 0,
        PartialSuccess = 1,
        Failure = 2
    }
}
