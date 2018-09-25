using System;
using System.Collections.Specialized;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;

namespace ZJB.WX.Common.UserVerifiler
{
   class UserVerifier
    {
    
  

        #region 站点验证
       public static bool CheckSite(int siteId, string siteUserName, string siteUserPwd)
        {
            bool flag = false;
           try
           {

            switch (siteId)
            {
                case 1: flag = UserVerifier_ZJB.IsValidate(siteUserName, siteUserPwd); break;
                case 2:
                case 4: flag = UserVerifier_SoufunM.IsValidate(siteUserName, siteUserPwd); break;
                case 3: flag = UserVerifier_Anjuke.IsValidate(siteUserName, siteUserPwd); break;
                case 5: flag = UserVerifier_58.IsValidate(siteUserName, siteUserPwd); break;
                case 6:
                case 7: flag = UserVerifier_58VIP.IsValidate(siteUserName, siteUserPwd); break;
                case 8:
                case 9:
                case 10: flag = UserVerifier_GanjiM.IsValidate(siteUserName, siteUserPwd); break;
                case 1006: flag = UserVerifier_XiaoyuM.IsValidate(siteUserName, siteUserPwd); break;
                case 1008: flag =true; break;
                case 1018: flag = UserVerifier_917.IsValidate(siteUserName, siteUserPwd); break;
                case 1022: flag = UserVerifier_Ffw.IsValidate(siteUserName, siteUserPwd); break;
                default :break;
            }

           }
           catch (Exception)
           {

               throw;
           }
           return flag;
        }
        #endregion
    }
}