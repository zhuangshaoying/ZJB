using System.Security.Policy;
using System.Web;
using System.Web.Mvc;

namespace System
{
    /// <summary>
    /// 表示需要用户登录才可以使用的特性
    /// <para>如果不需要处理用户登录，则请指定AllowAnonymousAttribute属性</para>
    /// </summary>
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Class, Inherited = true, AllowMultiple = true)]
    public class AuthorizationAttribute : FilterAttribute, IAuthorizationFilter
    {
        /// <summary>
        /// 默认构造函数
        /// </summary>
        public AuthorizationAttribute()
        {
            String authUrl = System.Configuration.ConfigurationManager.AppSettings["AuthUrl"];
            String saveKey = System.Configuration.ConfigurationManager.AppSettings["AuthSaveKey"];
            String saveType = System.Configuration.ConfigurationManager.AppSettings["AuthSaveType"];

            if (String.IsNullOrEmpty(authUrl))
            {
                this._AuthUrl = "/User/Login";
            }
            else
            {
                this._AuthUrl = authUrl;
            }
            if (String.IsNullOrEmpty(saveKey))
            {
                this._AuthSaveKey = "WXLoginedUser";
            }
            else
            {
                this._AuthSaveKey = saveKey;
            }
            if (String.IsNullOrEmpty(saveType))
            {
                this._AuthSaveType = "Session";
            }
            else
            {
                this._AuthSaveType = saveType;
            }
        }
        /// <summary>
        /// 构造函数重载
        /// </summary>
        /// <param name="loginUrl">表示没有登录跳转的登录地址</param>
        public AuthorizationAttribute(String authUrl)
            : this()
        {
            this._AuthUrl = authUrl;
        }
        /// <summary>
        /// 构造函数重载
        /// </summary>
        /// <param name="loginUrl">表示没有登录跳转的登录地址</param>
        /// <param name="saveKey">表示登录用来保存登陆信息的键名</param>
        public AuthorizationAttribute(String authUrl, String saveKey)
            : this(authUrl)
        {
            this._AuthSaveKey = saveKey;
            this._AuthSaveType = "SESSION_COOKIE";
        }
        /// <summary>
        /// 构造函数重载
        /// </summary>
        /// <param name="authUrl">表示没有登录跳转的登录地址</param>
        /// <param name="saveKey">表示登录用来保存登陆信息的键名</param>
        /// <param name="saveType">表示登录用来保存登陆信息的方式</param>
        public AuthorizationAttribute(String authUrl, String saveKey, String saveType)
            : this(authUrl, saveKey)
        {
            this._AuthSaveType = saveType;
        }

        private String _AuthUrl = String.Empty;
        /// <summary>
        /// 获取或者设置一个值，改值表示登录地址
        /// <para>如果web.config中未定义AuthUrl的值，则默认为/User/Login</para>
        /// </summary>
        public String AuthUrl
        {
            get { return _AuthUrl.Trim(); }
            set
            {
                if (String.IsNullOrEmpty(value))
                {
                    throw new ArgumentNullException("用于验证用户登录信息的登录地址不能为空！");
                }
                else
                {
                    _AuthUrl = value.Trim();
                }
            }
        }

        private String _AuthSaveKey = String.Empty;
        /// <summary>
        /// 获取或者设置一个值，改值表示登录用来保存登陆信息的键名
        /// <para>如果web.config中未定义AuthSaveKey的值，则默认为LoginedUser</para>
        /// </summary>
        public String AuthSaveKey
        {
            get { return _AuthSaveKey.Trim(); }
            set
            {
                if (String.IsNullOrEmpty(value))
                {
                    throw new ArgumentNullException("用于保存登陆信息的键名不能为空！");
                }
                else
                {
                    this._AuthSaveKey = value.Trim();
                }
            }
        }

        private String _AuthSaveType = String.Empty;
        /// <summary>
        /// 获取或者设置一个值，该值表示用来保存登陆信息的方式
        /// <para>如果web.config中未定义AuthSaveType的值，则默认为Session保存</para>
        /// </summary>
        public String AuthSaveType
        {
            get { return _AuthSaveType.Trim().ToUpper(); }
            set
            {
                if (String.IsNullOrEmpty(value))
                {
                    throw new ArgumentNullException("用于保存登陆信息的方式不能为空，只能为【Cookie】或者【Session】！");
                }
                else
                {
                    _AuthSaveType = value.Trim();
                }
            }
        }

        /// <summary>
        /// 处理用户登录
        /// </summary>
        /// <param name="filterContext"></param>
        public void OnAuthorization(AuthorizationContext filterContext)
        {
            var httpContext = HttpContext.Current;
            if (filterContext.HttpContext == null)
            {
                throw new Exception("此特性只适合于Web应用程序使用！");
            }
            else
            {
                var user = (filterContext.Controller as Controller).GetLoginUser();
                if (user != null)
                {

                }
                switch (AuthSaveType)
                {
                    case "SESSION":
                        if (filterContext.HttpContext.Session == null)
                        {
                            throw new Exception("服务器Session不可用！");
                        }
                        else if (!filterContext.ActionDescriptor.IsDefined(typeof(AllowAnonymousAttribute), true) && !filterContext.ActionDescriptor.ControllerDescriptor.IsDefined(typeof(AllowAnonymousAttribute), true))
                        {
                            if (filterContext.HttpContext.Session[_AuthSaveKey] == null)
                            {
                                filterContext.Result = new RedirectResult(_AuthUrl + "?tourl=" + httpContext.Request.Url);
                            }
                        }
                        break;
                    case "COOKIE":
                        if (!filterContext.ActionDescriptor.IsDefined(typeof(AllowAnonymousAttribute), true) && !filterContext.ActionDescriptor.ControllerDescriptor.IsDefined(typeof(AllowAnonymousAttribute), true))
                        {
                            if (filterContext.HttpContext.Request.Cookies[_AuthSaveKey] == null)
                            {
                                filterContext.Result = new RedirectResult(_AuthUrl + "?tourl=" + httpContext.Request.Url);
                            }
                        }
                        break;
                    case "test":
                        if (filterContext.HttpContext.Session == null)
                        {
                            if (!filterContext.ActionDescriptor.IsDefined(typeof(AllowAnonymousAttribute), true) && !filterContext.ActionDescriptor.ControllerDescriptor.IsDefined(typeof(AllowAnonymousAttribute), true))
                            {
                                if (filterContext.HttpContext.Request.Cookies[_AuthSaveKey] == null)
                                {
                                    filterContext.Result = new RedirectResult(_AuthUrl + "?tourl=" + httpContext.Request.Url);
                                }
                            }
                            else
                            {
                                throw new Exception("服务器Session不可用！");
                            }
                        }
                        else if (!filterContext.ActionDescriptor.IsDefined(typeof(AllowAnonymousAttribute), true) && !filterContext.ActionDescriptor.ControllerDescriptor.IsDefined(typeof(AllowAnonymousAttribute), true))
                        {
                            if (filterContext.HttpContext.Session[_AuthSaveKey] == null)
                            {
                                filterContext.Result = new RedirectResult(_AuthUrl + "?tourl=" + httpContext.Request.Url);
                            }
                        }
                        break;
                    case "SESSION_COOKIE":
                        if (!filterContext.ActionDescriptor.IsDefined(typeof(AllowAnonymousAttribute), true) && !filterContext.ActionDescriptor.ControllerDescriptor.IsDefined(typeof(AllowAnonymousAttribute), true))
                        {
                            // bool haveLoginStatus = (filterContext.HttpContext.Session != null && filterContext.HttpContext.Session[_AuthSaveKey] != null);
                            bool haveLoginStatus = (filterContext.HttpContext.Session != null && filterContext.HttpContext.Session[_AuthSaveKey] != null) || (filterContext.HttpContext.Request.Cookies[_AuthSaveKey] != null);
                            if (!haveLoginStatus)
                            {
                                string token = httpContext.Request.QueryString["token"];
                                if (!string.IsNullOrEmpty(token))
                                {
                                    filterContext.Result = new RedirectResult("/user/dologin" + "?token=" + token + "&tourl=" + HttpUtility.UrlEncode(httpContext.Request.RawUrl));
                                }
                                else
                                {
                                    if (httpContext.Request.UserAgent != null &&
                                        httpContext.Request.UserAgent.ToLower().Contains("micromessenger"))
                                    {

                                        filterContext.Result = new RedirectResult("http://www.zhujia001.com/OAuth2/WXLogin?&referer=" + httpContext.Request.RawUrl);


                                    }
                                    else
                                    {

                                        filterContext.Result = new RedirectResult(_AuthUrl + "?tourl=" + HttpUtility.UrlEncode(httpContext.Request.RawUrl));
                                    }
                                }
                            }
                        }
                        break;
                    default:
                        throw new ArgumentNullException("用于保存登陆信息的方式不能为空，只能为【Cookie】或者【Session】！");
                }
            }
        }
    }
}