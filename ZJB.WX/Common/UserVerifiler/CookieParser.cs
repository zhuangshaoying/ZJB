
namespace ZJB.WX.Common.UserVerifiler
{
    class CookieParser
    { 
        #region Cookies解析（为的是解决 .Net 3.5 CookieContainer 的Bug）
        /// <summary>
        /// 把服务器返回的Set-Cookie标头信息翻译成
        /// <para>能放在Cookie标头上的信息</para>
        /// </summary>
        /// <param name="CookieStr">Set-Cookie标头信息</param>
        /// <returns></returns>
        public static string ParseSetCookie(string CookieStr)
        {
            if (CookieStr.Contains("=") && CookieStr.Contains(";"))//合法性验证
            {
                string[] oneCookie = CookieStr.Split(';');
                string returnmsg = "";
                string onename = "";
                foreach (string one in oneCookie)
                {
                    string ifThereisAComma = one;
                    if (ifThereisAComma.StartsWith(","))
                    {
                        //修复一个Bug：如果一个标头上只有一个Cookie，则解析出来的相应值之前会多一个逗号。
                        ifThereisAComma = ifThereisAComma.Substring(1);//截掉开头的逗号
                    }
                    onename = ParseOneNameAndValue(ifThereisAComma);//判断是否有逗号
                    returnmsg += onename;
                }

                return returnmsg;
            }
            else
            {
                return "";
            }
        }

        /// <summary>
        /// 从数组中查找指定的值，并返回其Index
        /// </summary>
        /// <param name="Value">查找什么？</param>
        /// <param name="Source">在哪个数组中找？</param>
        /// <returns>Index或-1</returns>
        public static int FindIndex(string Value, string[] Source)
        {
            for (int i = 0; i < Source.Length; i++)
            {
                if (Value == Source[i])
                {
                    return i;
                }
            }
            return -1;
        }

        /// <summary>
        /// 判断一个Name=Value的值是不是一个真正的Cookies
        /// <para>若是两个Cookies，则把两个Cookies用分号隔开；</para>
        /// <para>不是，则返回输入的NameAndValue值。</para>
        /// </summary>
        /// <param name="NameAndValue">Cookies Name=Value</param>
        /// <returns></returns>
        public static string ParseOneNameAndValue(string NameAndValue)
        {
            if (!NameAndValue.Contains("="))
            {
                return "";
            }

            string returnmsg = "";
            if (NameAndValue.Contains(","))//有逗号
            {
                foreach (string one in NameAndValue.Split(','))
                {
                    if (one.Contains("=") && (!NameAndValue.StartsWith(one)))//逗号旁有等号且不是第一个，说明逗号是分隔两个Set-Cookie标头的
                    {
                        string[] indexArr = NameAndValue.Split(',');

                        int index = FindIndex(one, indexArr);
                        string one_s = "";

                        for (int i = index; i < indexArr.Length; i++)
                        {
                            one_s += indexArr[i];
                        }
                        returnmsg += ParseOneNameAndValue(one_s);//判断后面那一堆字符是不是一个Cookie
                    }
                    else//不是标头分隔
                    {
                        returnmsg += isPathDomainOrDate(NameAndValue);
                    }
                }
            }

            else//没有逗号，那就是货真价实的一个Cookie。
            {
                returnmsg += isPathDomainOrDate(NameAndValue);
            }

            return returnmsg;
        }
        /// <summary>
        /// 检测Name=Value是不是服务器Set-Cookie标头里的path、domain和过期日期
        /// <para></para><para>有则返回空字符串，以去掉那些不能放在Cookie标头上的信息</para>
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static string isPathDomainOrDate(string input)
        {

            if (input.Trim().ToLower().StartsWith("path=") || input.Trim().ToLower().StartsWith("domain=")
                || input.Trim().ToLower().StartsWith("expires=") || input.Trim().ToLower().StartsWith("max-age=")
                || input.Trim().ToLower().StartsWith("version=") || input.Trim().ToLower().StartsWith("httponly"))
            {
                //把Path、Domain和过期日期去掉
                return "";
            }
            else
            {
                return input + ";";
            }
        }
        #endregion
    }
}
