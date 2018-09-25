using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ZJB.Api.Models
{
    /// <summary>
    /// 状态码公共信息类
    /// 0-1000
    /// </summary>
    public static class Metas
    {
        /// <summary>
        /// 用户登录凭据过期
        /// </summary>
        public static Meta CREDENTIAL_EXPIRED = new Meta() { Status = 100, Message = "用户登录凭据过期" };
        /// <summary>
        /// 权限不够、非法访问
        /// 非授权平台访问
        /// </summary>
        public static Meta INVALID_PERMISSION = new Meta() { Status = 110, Message = "非授权平台访问" };
        /// <summary>
        /// 用户登录凭据过期
        /// </summary>
        public static Meta INVALID_CREDENTIAL = new Meta() { Status = 120, Message = "无效的用户登录凭据" };
        /// <summary>
        /// 调用次数超限，包含调用频率超限
        /// </summary>
        public static Meta APP_CALL_LIMITED = new Meta() { Status = 130, Message = "调用次数超限" };
        /// <summary>
        /// 编码错误
        /// 一般是用户做http请求的时候没有用UTF-8编码请求造成的
        /// </summary>
        public static Meta INVALID_ENCODING = new Meta() { Status = 140, Message = "Http编码错误" };
        /// <summary>
        /// 参数错误
        /// 一般是用户传入参数非法引起的，请仔细检查入参格式、范围是否一一对应
        /// </summary>
        public static Meta PARAMETER_ERROR = new Meta() { Status = 1000, Message = "请求的参数错误" };
        /// <summary>
        /// 调用后端服务抛异常，服务不可用
        /// </summary>
        public static Meta SERVICE_UNAVAILABLE = new Meta() { Status = 50, Message = "服务器异常" };
        /// <summary>
        /// 未知错误
        /// </summary>
        public static Meta UNKNOWN_ERROR = new Meta() { Status = 44, Message = "未知错误" };
        /// <summary>
        /// 正确返回
        /// </summary>
        public static Meta SUCCESS = new Meta() { Status = 0, Message = "Ok" };
        /// <summary>
        /// 最新版本
        /// </summary>
        public static Meta LATEST_VERSION = new Meta() { Status = 200, Message = "当前版本已是最新版本" };
        /// <summary>
        /// 验证码发送失败
        /// </summary>
        public static Meta PHONE_SEND_ERROR = new Meta() { Status = 300, Message = "验证码发送失败" };
        /// <summary>
        /// 账号不存在
        /// </summary>
        public static Meta ACOUNT_NOT_EXISTS = new Meta() { Status = 111, Message = "账号不存在" };
        /// <summary>
        /// OpenID已存在
        /// </summary>
        public static Meta OPEN_ID_EXISTS = new Meta() { Status = 112, Message = "第三方账号已存在" };
        /// <summary>
        /// 用户被冻结
        /// </summary>
        public static Meta User_Invalid = new Meta() { Status = 112, Message = "用户被冻结" };
        /// <summary>
        ///密码不为空
        /// </summary>
        public static Meta PassWordNotNull = new Meta() { Status = 114, Message = "密码不为空" };
        /// <summary>
        ///密码为空
        /// </summary>
        public static Meta PassWordIsNull = new Meta() { Status = 115, Message = "密码为空" };
        /// <summary>
        /// 收藏失败
        /// </summary>
        public static Meta Collection_Error = new Meta() { Status = 116, Message = "收藏失败"};
         /// <summary>
        /// 帐号或者密码错误
        /// </summary>
        public static Meta ACCOUNT_OR_PASSWORD_WRONG = new Meta() { Status = 117, Message = "帐号或者密码错误" };
        /// <summary>
        /// 站点不存在
        /// </summary>
        public static Meta SITE_NOT_EXISTS = new Meta() { Status = 118, Message = "站点不存在" };
        /// <summary>
        /// 密码错误
        /// </summary>
        public static Meta PASSWORD_WRONG = new Meta() { Status = 119, Message = "密码错误" };
        /// <summary>
        /// 站点已经绑定
        /// </summary>
        public static Meta SITE_EXISTS = new Meta() { Status = 120, Message = "站点已经绑定" };

        /// <summary>
        /// 房源ID为空
        /// </summary>
        public static Meta HOUSEID_NULL = new Meta() { Status = 121, Message = "没有选中房源" };
        /// <summary>
        /// 发布站点
        /// </summary>
        public static Meta WEBSITES_NULL = new Meta() { Status = 122, Message = "没有选中发布网站" };
        /// <summary>
        /// 录入房源
        /// </summary>
        public static Meta BuildAreaError = new Meta() { Status = 123, Message = "建筑面积必须大于使用面积" };
          /// <summary>
        /// 录入房源
        /// </summary>
        public static Meta CurFloorError = new Meta() { Status = 124, Message = "总层数必须大于所在楼层" };
        /// <summary>
        /// 录入房源
        /// </summary>
        public static Meta HouseDescribeError = new Meta() { Status = 125, Message = "房源描述的字数为30-3000个字！" };
         /// <summary>
        /// 录入房源
        /// </summary>
        public static Meta HOUSENULL = new Meta() { Status = 126, Message = "请填写信息" };
           /// <summary>
        /// 录入房源
        /// </summary>
        public static Meta PostType_ERROR = new Meta() { Status = 127, Message = "发布类型错误" };
            /// <summary>
        /// 录入房源
        /// </summary>
        public static Meta BuildType_ERROR = new Meta() { Status = 128, Message = "房源类型错误" };

        /// <summary>
        /// 用户信息
        /// </summary>
        public static Meta EMAIL_NULL = new Meta() { Status = 129, Message = "请填写邮箱" };
        /// <summary>
        /// 用户信息
        /// </summary>
        public static Meta EMAIL_EXISTS = new Meta() { Status = 130, Message = "该邮箱已被使用" };
           /// <summary>
        /// 用户信息
        /// </summary>
        public static Meta Portrait_NULL = new Meta() { Status = 131, Message = "未来上传头像" };
        /// <summary>
        /// 用户信息
        /// </summary>
        public static Meta Info_NULL = new Meta() { Status = 132, Message = "请填写信息" };

           /// <summary>
        /// 用户信息
        /// </summary>
        public static Meta Pwd_NULL = new Meta() { Status = 133, Message = "密码不能为空" };
        /// <summary>
        ///密码太短
        /// </summary>
        public static Meta PwdLength_Wrong = new Meta() { Status = 134, Message = "密码至少6个字符以上" };
        /// <summary>
        /// 用户信息
        /// </summary>
        public static Meta Phone_NULL = new Meta() { Status = 135, Message = "手机号不能为空" };
        /// <summary>
        /// 用户信息
        /// </summary>
        public static Meta Captcha_Wrong = new Meta() { Status = 135, Message = "验证码错误" };

        /// <summary>
        /// 手机号码已存在
        /// </summary>
        public static Meta Phone_EXISTS = new Meta() { Status = 136, Message = "手机号码已存在" };
        /// <summary>
        /// 用户真实姓名长度错误
        /// </summary>
        public static Meta EnrolnName_LengthError = new Meta() { Status = 137, Message = "真实姓名长度为2-6个字" };
        /// <summary>
        /// 内容为空
        /// </summary>
        public static Meta Content_Null= new Meta() { Status = 138, Message = "内容为空" }; 

            /// <summary>
        /// 任务不存在
        /// </summary>
        public static Meta Task_Null = new Meta() { Status = 139, Message = "任务不存在" }; 
        
         /// <summary>
        /// 今日已签到
        /// </summary>
        public static Meta Sign_EXISTS = new Meta() { Status = 140, Message = "今日已签到" }; 

          /// <summary>
        /// 领取时间未到
        /// </summary>
        public static Meta Appointed_Time = new Meta() { Status = 141, Message = "领取时间未到" }; 
        
    }
}