using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Text;

namespace ZJB.Api.Common
{

    internal class QmyHelper
    {
        const string urlTemplate = "https://api.qingmayun.com/{0}/accounts/{1}/SMS/templateSMS?sig={2}&timestamp={3}";
        private const string bodyTemplate = "{{\"templateSMS\": {{\"appId\": \"{0}\", \"templateId\": \"{1}\",  \"to\": \"{2}\", \"param\": \"{3}\"}}}}";
        private const string NewBodyTemplate = "accountSid={0}&smsContent={1}&to={2}&timestamp={3}&sig={4}&respDataType=JSON";
        //"{{\"templateSMS\": {{\"accountSid\": \"{0}\", \"smsContent\": \"{1}\",  \"to\": \"{2}\", \"timestamp\": \"{3}\",\"sig\":\"{4}\"}}}}";
        public static string SendSms(QmySms sms)
        {
            string timestamp = DateTime.Now.ToString("yyyyMMddHHmmss");
            string sig = ToMd5String(sms.AccountSID + sms.AuthToken + timestamp);
            string url = string.Format(urlTemplate, sms.Version, sms.AccountSID, sig, timestamp);
            string body = string.Format(bodyTemplate, sms.AppID, sms.EmailTemplateID, sms.To, sms.Param);

            WebClient client = new WebClient();
            client.Encoding = System.Text.Encoding.UTF8;
            client.Headers.Add("Accept: application/json");
            client.Headers.Add("Content-Type: application/json;charset=utf-8");
            //client.Proxy = new WebProxy("127.0.0.1:8888");
       
           
            return client.UploadString(url, body);
        }
        public static string NewSendSms(QmySms sms)
        {
            string url = "https://api.miaodiyun.com/20150822/industrySMS/sendSMS";
            string timestamp = DateTime.Now.ToString("yyyyMMddHHmmss");
            string sig = ToMd5String(sms.AccountSID + sms.AuthToken + timestamp);
            string body = string.Format(NewBodyTemplate, sms.AccountSID, sms.Param, sms.To, timestamp,sig);
            WebClient client = new WebClient();
            client.Encoding = System.Text.Encoding.UTF8;
            client.Headers.Add("Accept: application/json");
            client.Headers.Add("Content-Type: application/x-www-form-urlencoded");
            return client.UploadString(url, body);
        }
        public static string ToMd5String(string beforeHash)
        {
            MD5 md5 = new MD5CryptoServiceProvider();
            byte[] hashResult = md5.ComputeHash(Encoding.UTF8.GetBytes(beforeHash));

            return BitConverter.ToString(hashResult).Replace("-", string.Empty).ToLower();
        }
    }
}
