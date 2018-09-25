namespace ZJB.Api.Entity
{
    public enum ELoginStatus
    {
        /// <summary>
        /// 注册成功
        /// </summary>
        Success = 0,
        /// <summary>
        /// 邮箱已存在
        /// </summary>
        EmailExists = 1,
        /// <summary>
        /// 昵称已存在
        /// </summary>
        NickNameExists = 2,
        /// <summary>
        /// 手机号码已存在
        /// </summary>
        PhoneExists = 3,
        /// <summary>
        /// 用户名错误
        /// </summary>
        DisplayNameError = 4,
        /// <summary>
        /// 验证码错误
        /// </summary>
        CaptchaError = 5,
        /// <summary>
        /// 邮箱错误
        /// </summary>
        EmailError = 6,
        /// <summary>
        /// 手机号码错误
        /// </summary>
        PhoneError = 7,
        /// <summary>
        /// 密码错误
        /// </summary>
        PasswordError = 8,
        /// <summary>
        /// 密码格式错误
        /// </summary>
        PasswordFormatError = 9,
        /// <summary>
        /// 用户名格式错误
        /// </summary>
        DisplayNameFormatError = 10,
        /// <summary>
        /// 手机验证码发送失败
        /// </summary>
        PhoneSendFailed = 11,
        /// <summary>
        /// 找回密码邮箱发送失败
        /// </summary>
        EmailSendFailed = 12,
        /// <summary>
        /// 用户不存在
        /// </summary>
        UserNotExist = 13,
        /// <summary>
        /// openid存在
        /// </summary>
        OpenIdExists = 14,
        /// <summary>
        /// 用户无效
        /// </summary>
        UserInvalid = 15
    }
}
