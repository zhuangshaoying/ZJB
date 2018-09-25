using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web.Http;
using MongoDB.Bson;
using ZJB.Core.Logging;

namespace ZJB.WX
{
    public  class WeixinConfig
    {
    
        private static Dictionary<string, WeixinInfo> _WeixinDic = null;
        public static Dictionary<string, WeixinInfo> WeixinDic
        {

            get
            {
                if (_WeixinDic == null)
                {
                    string filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Config/weixin.json");
                    if (File.Exists(filePath))
                    {
                        //Logger logger = new Logger("ExceptionLog");
                        var weixin = File.ReadAllText(filePath);
                        _WeixinDic = JsonConvert.DeserializeObject<Dictionary<string, WeixinInfo>>(weixin);
                        //logger.Debug(_WeixinDic.ToJson());
                    }
                    if (_WeixinDic == null)
                    {
                        _WeixinDic = new Dictionary<string, WeixinInfo>();
                        string appId = System.Configuration.ConfigurationManager.AppSettings["WeixinAppId"];
                        if (!string.IsNullOrEmpty(appId))
                        {
                            _WeixinDic.Add(appId, new WeixinInfo()
                                {
                                    AppId = appId,
                                    AppSecret = System.Configuration.ConfigurationManager.AppSettings["WeixinAppSecret"],
                                    AppToken = System.Configuration.ConfigurationManager.AppSettings["WeixinAppToken"],
                                    AppEncodingAESKey = System.Configuration.ConfigurationManager.AppSettings["WeixinAppSecret"],
                                    AppDesc = System.Configuration.ConfigurationManager.AppSettings["WeixinAppEncodingAESKey"] ?? "",
                                    AuthState="zhujia001",
                            }
                            );
                        }
                    }
                }
                return _WeixinDic;
            }
        }
        public static  WeixinInfo Get(string appId)
        {
            if (WeixinDic.ContainsKey(appId))
            {
                return WeixinDic[appId];
            }
            else
            {
                return null;
            }
        }
    }

    public class WeixinInfo
    {
        /// <summary>
        /// AppId
        /// </summary>
        public string AppId { get; set; }
        /// <summary>
        /// 说明
        /// </summary>
        public string AppDesc { get; set; }
        /// <summary>
        /// AppSecret
        /// </summary>
        public string AppSecret { get; set; }
        /// <summary>
        /// Token
        /// </summary>
        public string AppToken { get; set; }
        /// <summary>
        /// EncodingAESKey
        /// </summary>
        public string AppEncodingAESKey { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string AuthState { get; set; }

    }  
}
