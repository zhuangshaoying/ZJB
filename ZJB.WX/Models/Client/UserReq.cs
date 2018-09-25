using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ZJB.WX.Models.Client
{
    public class LoginReq
    {
        public string UserName
        {
            get;
            set;
        }
        public string Password
        {
            get;
            set;
        }
        public string Token
        {
            get;
            set;
        }
        
    }
    public class UserReq
    {
        public string Email
        {
            get;
            set;
        }
        public string Portrait
        {
            get;
            set;
        }
        public string EnrolnName
        {
            get;
            set;
        }
        public string OldPwd
        {
            get;
            set;
        }
        public string NewPwd
        {
            get;
            set;
        }
        public string Phone
        {
            get;
            set;
        }
        public int Type
        {
            get;
            set;
        }
        public string Captcha
        {
            get;
            set;
        }
    }

    public class FeedbackReq
    {
        public string Content
        {
            get;
            set;
        }
      
        
    }
    
    public class SiteUserReq
    {
        public string UserName
        {
            get;
            set;
        }
        public string Password
        {
            get;
            set;
        }
        public int SiteID
        {
            get;
            set;
        }
    }

    public class ShareInfoReq
    {
        public string ShareUrl
        {
            get;
            set;
        }
        public string ShareImgUrl
        {
            get;
            set;
        }
        public string ShareContent
        {
            get;
            set;
        }
        public string ShareTitle
        {
            get;
            set;
        }
        public string Source
        {
            get;
            set;
        }
    }
}