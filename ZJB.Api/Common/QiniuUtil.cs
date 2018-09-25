using Newtonsoft.Json;
using Qiniu.Util;
using System;
using System.Configuration;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace ZJB.Api.Common
{
    public static class QiniuUtil
    {
        static readonly string Bucket = "zhujia";

        public static string GetUpToken(UInt32 expires)
        {
            return new PutPolicy(Bucket, expires).Token(new Mac());
        }
    }

    [JsonObject(MemberSerialization.OptIn)]
    public class PutPolicy
    {
        #region properties
        private string scope;
        private string callBackUrl;
        private string callBackBody;
        private string returnUrl;
        private string returnBody;
        private string asyncOps;
        private string saveKey;
        private int insertOnly;
        private int detectMime;
        private string mimeLimit;
        private long fsizeLimit;
        private string persistentOps;
        private string persistentNotifyUrl;
        private string endUser;
        private UInt64 expires = 3600;
        private UInt64 deadline = 0;

        /// <summary>
        /// 一般指文件要上传到的目标存储空间（Bucket）。若为”Bucket”，表示限定只能传到该Bucket（仅限于新增文件）；若为”Bucket:Key”，表示限定特定的文件，可修改该文件。
        /// </summary>
        [JsonProperty("scope")]
        public string Scope
        {
            get { return scope; }
            set { scope = value; }
        }

        /// <summary>
        /// 文件上传成功后，Qiniu-Cloud-Server 向 App-Server 发送POST请求的URL，必须是公网上可以正常进行POST请求并能响应 HTTP Status 200 OK 的有效 URL
        /// </summary>
        [JsonProperty("callBackUrl")]
        public string CallBackUrl
        {
            get { return callBackUrl; }
            set { callBackUrl = value; }
        }

        /// <summary>
        /// 文件上传成功后，Qiniu-Cloud-Server 向 App-Server 发送POST请求的数据。支持 魔法变量 和 自定义变量，不可与 returnBody 同时使用。
        /// </summary>
        [JsonProperty("callBackBody")]
        public string CallBackBody
        {
            get { return callBackBody; }
            set { callBackBody = value; }
        }

        /// <summary>
        /// 设置用于浏览器端文件上传成功后，浏览器执行301跳转的URL，一般为 HTML Form 上传时使用。文件上传成功后会跳转到 returnUrl?query_string, query_string 会包含 returnBody 内容。returnUrl 不可与 callbackUrl 同时使用
        /// </summary>
        [JsonProperty("returnUrl")]
        public string ReturnUrl
        {
            get { return returnUrl; }
            set { returnUrl = value; }
        }

        /// <summary>
        /// 文件上传成功后，自定义从 Qiniu-Cloud-Server 最终返回給终端 App-Client 的数据。支持 魔法变量，不可与 callbackBody 同时使用。
        /// </summary>    
        [JsonProperty("returnBody")]
        public string ReturnBody
        {
            get { return returnBody; }
            set { returnBody = value; }
        }

        /// <summary>
        /// 指定文件（图片/音频/视频）上传成功后异步地执行指定的预转操作。每个预转指令是一个API规格字符串，多个预转指令可以使用分号“;”隔开
        /// </summary>
        [JsonProperty("asyncOps")]
        public string AsyncOps
        {
            get { return asyncOps; }
            set { asyncOps = value; }
        }

        /// <summary>
        /// 给上传的文件添加唯一属主标识，特殊场景下非常有用，比如根据终端用户标识给图片或视频打水印
        /// </summary>
        [JsonProperty("endUser")]
        public string EndUser
        {
            get { return endUser; }
            set { endUser = value; }
        }

        /// <summary>
        /// 定义 uploadToken 的失效时间，Unix时间戳，精确到秒，缺省为 3600 秒
        /// </summary>
        [JsonProperty("deadline")]
        public UInt64 Deadline
        {
            get { return deadline; }
        }

        /// <summary>
        /// 可选, Gets or sets the save key.
        /// </summary>
        /// <value>The save key.</value>
        [JsonProperty("saveKey")]
        public string SaveKey
        {
            get
            {
                return saveKey;
            }
            set
            {
                saveKey = value;
            }
        }

        /// <summary>
        /// 可选。 若非0, 即使Scope为 Bucket:Key 的形式也是insert only.
        /// </summary>
        /// <value>The insert only.</value>
        [JsonProperty("insertOnly")]
        public int InsertOnly
        {
            get
            {
                return insertOnly;
            }
            set
            {
                insertOnly = value;
            }
        }

        /// <summary>
        /// 可选。若非0, 则服务端根据内容自动确定 MimeType */
        /// </summary>
        /// <value>The detect MIME.</value>
        [JsonProperty("detectMime")]
        public int DetectMime
        {
            get
            {
                return detectMime;
            }
            set
            {
                detectMime = value;
            }
        }

        /// <summary>
        /// 限定用户上传的文件类型
        /// 指定本字段值，七牛服务器会侦测文件内容以判断MimeType，再用判断值跟指定值进行匹配，匹配成功则允许上传，匹配失败返回400状态码
        /// 示例:
        ///1. “image/*“表示只允许上传图片类型
        ///2. “image/jpeg;image/png”表示只允许上传jpg和png类型的图片
        /// </summary>
        /// <value>The detect MIME.</value>
        [JsonProperty("mimeLimit")]
        public string MimeLimit
        {
            get
            {
                return mimeLimit;
            }
            set
            {
                mimeLimit = value;
            }
        }

        /// <summary>
        /// 可选, Gets or sets the fsize limit.
        /// </summary>
        /// <value>The fsize limit.</value>
        [JsonProperty("fsizeLimit")]
        public long FsizeLimit
        {
            get
            {
                return fsizeLimit;
            }
            set
            {
                fsizeLimit = value;
            }
        }

        /// <summary>
        /// 音视频转码持久化完成后，七牛的服务器会向用户发送处理结果通知。这里指定的url就是用于接收通知的接口。设置了`persistentOps`,则需要同时设置此字段
        /// </summary>
        [JsonProperty("persistentNotifyUrl")]
        public string PersistentNotifyUrl
        {
            get { return persistentNotifyUrl; }
            set { persistentNotifyUrl = value; }
        }

        /// <summary>
        /// 可指定音视频文件上传完成后，需要进行的转码持久化操作。asyncOps的处理结果保存在缓存当中，有可能失效。而persistentOps的处理结果以文件形式保存在bucket中，体验更佳。[数据处理(持久化)](http://docs.qiniu.com/api/persistent-ops.html
        /// </summary>
        [JsonProperty("persistentOps")]
        public string PersistentOps
        {
            get { return persistentOps; }
            set { persistentOps = value; }
        }
        #endregion
        /// <summary>
        /// Initializes a new instance of the <see cref="Qiniu.RS.PutPolicy"/> class.
        /// </summary>
        /// <param name="scope">Scope.</param>
        /// <param name="expires">Expires.</param>
        public PutPolicy(string scope, UInt32 expires = 108000)
        {
            Scope = scope;
            this.expires = expires;
        }

        /// <summary>
        /// 生成上传Token
        /// </summary>
        /// <returns></returns>
        public string Token(Mac mac = null)
        {
            if (string.IsNullOrEmpty(persistentOps) ^ string.IsNullOrEmpty(persistentNotifyUrl))
            {
                throw new Exception("PersistentOps and PersistentNotifyUrl error");
            }
            if (string.IsNullOrEmpty(callBackUrl) ^ string.IsNullOrEmpty(callBackBody))
            {
                throw new Exception("CallBackUrl and CallBackBody error");
            }
            if (!string.IsNullOrEmpty(returnUrl) && !string.IsNullOrEmpty(callBackUrl))
            {
                throw new Exception("returnUrl and callBackUrl error");
            }
            if (mac == null)
            {
                mac = new Mac(Config.ACCESS_KEY, Config.Encoding.GetBytes(Config.SECRET_KEY));
            }
            this.deadline = (UInt32)((DateTime.Now.ToUniversalTime().Ticks - 621355968000000000) / 10000000 + (long)expires);
            string flag = this.ToString();
            return mac.SignWithData(Config.Encoding.GetBytes(flag));
        }

        /// <summary>
        /// Returns a <see cref="System.String"/> that represents the current <see cref="Qiniu.RS.PutPolicy"/> in json formmat.
        /// </summary>
        /// <returns>A <see cref="System.String"/> that represents the current <see cref="Qiniu.RS.PutPolicy"/>.</returns>
        public override string ToString()
        {
            return QiniuJsonHelper.JsonEncode(this);
        }
    }

    public class Config
    {
        public static string USER_AGENT = "qiniu csharp-sdk v6.0.0";
        #region 帐户信息
        /// <summary>
        /// 七牛提供的公钥，用于识别用户
        /// </summary>
        public static string ACCESS_KEY = "ANTo1e2dRn12OnoXNicz9PmXTqsfw2c16kaT8igB";

    /// <summary>
    /// 七牛提供的秘钥，不要在客户端初始化该变量
    /// </summary>
    public static string SECRET_KEY = "8kAjrJtNInFaY0m2Yvj0QH2c4ThUgsldimKyhs6e";

        #endregion
        #region 七牛服务器地址
        /// <summary>
        /// 七牛资源管理服务器地址
        /// </summary>
        public static string RS_HOST = "http://rs.Qbox.me";
        /// <summary>
        /// 七牛资源上传服务器地址.
        /// </summary>
        public static string UP_HOST = "http://up.qiniu.com";
        /// <summary>
        /// 七牛资源列表服务器地址.
        /// </summary>
        public static string RSF_HOST = "http://rsf.Qbox.me";

        public static string PREFETCH_HOST = "http://iovip.qbox.me";

        public static string API_HOST = "http://api.qiniu.com";
        #endregion
        /// <summary>
        /// 七牛SDK对所有的字节编码采用utf-8形式 .
        /// </summary>
        public static Encoding Encoding = Encoding.UTF8;

        /// <summary>
        /// 初始化七牛帐户、请求地址等信息，不应在客户端调用。
        /// </summary>
        public static void Init()
        {
            USER_AGENT = ConfigurationManager.AppSettings["QiniuUserAgent"];
            ACCESS_KEY = ConfigurationManager.AppSettings["QiniuAccessKey"];
            SECRET_KEY = ConfigurationManager.AppSettings["QiniuSecretKey"];
            RS_HOST = ConfigurationManager.AppSettings["QiniuRSHOST"];
            UP_HOST = ConfigurationManager.AppSettings["QiniuUPHOST"];
            RSF_HOST = ConfigurationManager.AppSettings["QiniuRSFHOST"];
            PREFETCH_HOST = ConfigurationManager.AppSettings["QiniuPREFETCHHOST"];
            API_HOST = ConfigurationManager.AppSettings["QiniuAPIHOST"];
        }
    }

    /// <summary>
    /// 七牛消息认证(Message Authentication)
    /// </summary>
    public class Mac
    {

        private string accessKey;

        /// <summary>
        /// Gets or sets the access key.
        /// </summary>
        /// <value>The access key.</value>
        public string AccessKey
        {
            get { return accessKey; }
            set { accessKey = value; }
        }

        private byte[] secretKey;

        /// <summary>
        /// Gets the secret key.
        /// </summary>
        /// <value>The secret key.</value>
        public byte[] SecretKey
        {
            get { return secretKey; }
        }

        public Mac()
        {
            this.accessKey = Config.ACCESS_KEY;
            this.secretKey = Config.Encoding.GetBytes(Config.SECRET_KEY);
        }

        public Mac(string access, byte[] secretKey)
        {
            this.accessKey = access;
            this.secretKey = secretKey;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        private string _sign(byte[] data)
        {
            HMACSHA1 hmac = new HMACSHA1(SecretKey);
            byte[] digest = hmac.ComputeHash(data);
            return Base64URLSafe.Encode(digest);
        }

        /// <summary>
        /// Sign
        /// </summary>
        /// <param name="b"></param>
        /// <returns></returns>
        public string Sign(byte[] b)
        {
            return string.Format("{0}:{1}", this.accessKey, _sign(b));
        }

        /// <summary>
        /// SignWithData
        /// </summary>
        /// <param name="b"></param>
        /// <returns></returns>
        public string SignWithData(byte[] b)
        {
            string data = Base64URLSafe.Encode(b);
            return string.Format("{0}:{1}:{2}", this.accessKey, _sign(Config.Encoding.GetBytes(data)), data);
        }

        /// <summary>
        /// SignRequest
        /// </summary>
        /// <param name="request"></param>
        /// <param name="body"></param>
        /// <returns></returns>
        public string SignRequest(System.Net.HttpWebRequest request, byte[] body)
        {
            Uri u = request.Address;
            using (HMACSHA1 hmac = new HMACSHA1(secretKey))
            {
                string pathAndQuery = request.Address.PathAndQuery;
                byte[] pathAndQueryBytes = Config.Encoding.GetBytes(pathAndQuery);
                using (MemoryStream buffer = new MemoryStream())
                {
                    buffer.Write(pathAndQueryBytes, 0, pathAndQueryBytes.Length);
                    buffer.WriteByte((byte)'\n');
                    if (body.Length > 0)
                    {
                        buffer.Write(body, 0, body.Length);
                    }
                    byte[] digest = hmac.ComputeHash(buffer.ToArray());
                    string digestBase64 = Base64URLSafe.Encode(digest);
                    return this.accessKey + ":" + digestBase64;
                }
            }
        }
    }

    public static class QiniuJsonHelper
    {
        public static string JsonEncode(object obj)
        {
            JsonSerializerSettings setting = new JsonSerializerSettings();
            setting.NullValueHandling = NullValueHandling.Ignore;
            return JsonConvert.SerializeObject(obj, setting);
        }

        public static T ToObject<T>(string value)
        {
            return JsonConvert.DeserializeObject<T>(value);
        }
    }
}
