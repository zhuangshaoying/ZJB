/*
 * 程序中文名称: 字符操作函数
 * 
 * 程序英文名称: ZJB.Core.Utilities
 * 
 * 程序版本: 1.1
 *    
 */

using System;
using System.Security.Cryptography;
using System.Text.RegularExpressions;
using System.Text;
namespace ZJB.Core.Utilities
{

    public static partial class StringUtility
    {
        private static string charset = "ABCDEFGHJKLMNPQRSTUVWXYZ23456789";
        private static string charsetNum = "0123456789";
        
        /// <summary>
        /// Remove illegal XML characters from a string.
        /// </summary>
        public static string SanitizeXmlString(string xml)
        {
            if (xml == null)
            {
                return String.Empty;
            }

            StringBuilder buffer = new StringBuilder(xml.Length);

            foreach (char c in xml)
            {
                if (IsLegalXmlChar(c))
                {
                    buffer.Append(c);
                }
            }

            return buffer.ToString();
        }

        /// <summary>
        /// Whether a given character is allowed by XML 1.0.
        /// </summary>
        private static bool IsLegalXmlChar(int character)
        {
            return
            (
                 character == 0x9 /* == '\t' == 9   */          ||
                 character == 0xA /* == '\n' == 10  */          ||
                 character == 0xD /* == '\r' == 13  */          ||
                (character >= 0x20 && character <= 0xD7FF) ||
                (character >= 0xE000 && character <= 0xFFFD) ||
                (character >= 0x10000 && character <= 0x10FFFF)
            );
        }
        public static string ToMd5String(string beforeHash)
        {
            MD5 md5 = new MD5CryptoServiceProvider();
            byte[] hashResult = md5.ComputeHash(Encoding.UTF8.GetBytes(beforeHash));

            return BitConverter.ToString(hashResult).Replace("-", string.Empty);
        }

        public static string NewGuidString()
        {
            return Guid.NewGuid().ToString().Replace("-", "");
        }

        public static string NormalizeLink(string link, string baseUrl)
        {
            if (link.StartsWith("http", StringComparison.OrdinalIgnoreCase))
                return link;

            string normalizedLink;

            int slashIndex = baseUrl.IndexOf('/', baseUrl.IndexOf("://") + 3);

            if (link.StartsWith("/"))
            {
                if (slashIndex < 0)
                    normalizedLink = baseUrl + link;
                else
                    normalizedLink = baseUrl.Substring(0, slashIndex) + link;
            }
            else
            {
                if (slashIndex < 0)
                    normalizedLink = baseUrl + "/" + link;
                else
                {
                    int lastSlashIndex = baseUrl.LastIndexOf("/");
                    if (link.StartsWith(".")) link = link.Remove(0, link.IndexOf("/") + 1);

                    normalizedLink = baseUrl.Substring(0, lastSlashIndex + 1) + link;
                }
            }

            return normalizedLink;
        }
        /// <summary>
        /// 随机获取6个字符
        /// </summary>
        /// <returns></returns>
        public static string GetSubfix()
        {
            Random random = new Random();
            StringBuilder buffer = new StringBuilder();

            for (int i = 0; i < 6; i++)
            {
                buffer.Append(charset[random.Next(charset.Length)]);
            }

            return buffer.ToString();
        }
        /// <summary>
        /// 随机获取6个数字
        /// </summary>
        /// <returns></returns>
        public static string GetValiCode()
        {
            return GetValiCode(6);

        }
        public static string GetValiCode(int length)
        {
            Random random = new Random();
            StringBuilder buffer = new StringBuilder();

            for (int i = 0; i < length; i++)
            {
                buffer.Append(charsetNum[random.Next(charsetNum.Length)]);
            }

            return buffer.ToString();

        }


        #region 删除字符串中的html标识代码
        /// <summary>
        /// 功能：删除字符串中的html标识代码后返回
        /// </summary>
        /// <param name="sStr">传入带html标识的字符串</param>
        /// <returns>返回删除html标识后的字符串</returns>
        public static string DelHtml(string sStr)
        {
            string temp;
            if (sStr == "")
            {
                temp = "";
                return temp;
            }
            sStr = sStr.Trim().Replace("&nbsp;", "");
            sStr = sStr.Replace("&gt;", "");
            sStr = sStr.Replace("&lt;", "");
            sStr = sStr.Replace("&quot;", "");
            sStr = sStr.Replace("\n", "");
            Regex objReg = new Regex("<[^>]*?>");
            temp = objReg.Replace(sStr, "");
            return temp;
        }
        #endregion




        #region 判断字符串是否为空
        /// <summary>
        /// 判断字符串是否为空
        /// </summary>
        /// <param name="value">字符串</param>
        /// <returns></returns>
        public static bool IsNull(this string value)
        {
            return string.IsNullOrEmpty(value);
        }
        #endregion

        #region 判断字符串是否不为空
        /// <summary>
        /// 判断字符串是否不为空
        /// </summary>
        /// <param name="value">字符串</param>
        /// <returns></returns>
        public static bool IsNoNull(this string value)
        {
            return !string.IsNullOrEmpty(value);
        }
        #endregion


        /// <summary>
        /// 判断字符串是否是一个合法的IP地址。
        /// </summary>
        /// <param name="ip"></param>
        /// <returns></returns>
        public static bool IsIP(this string ip)
        {
            return Regex.IsMatch(ip, @"^((2[0-4]\d|25[0-5]|[01]?\d\d?)\.){3}(2[0-4]\d|25[0-5]|[01]?\d\d?)$");
        }

        #region 判断输入的字符串是否完全匹配正则
        /// <summary>
        /// 判断输入的字符串是否完全匹配正则
        /// </summary>
        /// <param name="RegexExpression">正则表达式</param>
        /// <param name="str">待判断的字符串</param>
        /// <returns></returns>
        public static bool IsValiable(this string str, string RegexExpression)
        {
            bool blResult = false;
            Regex rep = new Regex(RegexExpression, RegexOptions.IgnoreCase);
            Match mc = rep.Match(str);

            if (mc.Success)
            {
                if (mc.Value == str) blResult = true;
            }


            return blResult;
        }
        #endregion

        #region 将16进制字符串转换成字节数组
        /// <summary>
        /// 将16进制字符串转换成字节数组 
        /// </summary>
        /// <param name="source">16进制字符串</param>
        /// <returns></returns>
        public static byte[] FromHexString(this string source)
        {
            if (string.IsNullOrEmpty(source))
            {
                return new byte[0];
            }
            char[] chars = source.ToCharArray();
            byte[] bytes = new byte[chars.Length / 2];
            for (int i = 0; i < (chars.Length / 2); i++)
            {
                bytes[i] = Convert.ToByte(chars[i * 2].ToString() + chars[(i * 2) + 1].ToString(), 0x10);
            }
            return bytes;
        }

        #endregion


        #region 指定截取文本长度的
        /// <summary>
        /// 指定截取文本长度的
        /// </summary>
        /// <param name="textData">文本字符串</param>
        /// <param name="Length">指定截断长度</param>
        /// <param name="Flag">是否保留HTML代码</param>
        /// <param name="AddString">附加字符串</param>
        /// <returns></returns>
        public static string CutText(this string textData, int Length, bool Flag, string AddString)
        {
            if (Encoding.Default.GetByteCount(textData) > Length)
            {
                if (textData == null)
                {
                    return "";
                }
                int i = 0;
                int j = 0;
                if (!Flag)
                {
                    textData = Regex.Replace(textData, @"\<[^\<^\>]*\>", "");
                    textData = textData.Replace("&nbsp;", "");
                }
                foreach (char Char in textData)
                {
                    if (Char > '\x007f')
                    {
                        i += 2;
                    }
                    else
                    {
                        i++;
                    }
                    if (i > Length)
                    {
                        if (AddString.IsNoNull())
                        {
                            textData = textData.Substring(0, j) + AddString;
                            return textData;
                        }
                        textData = textData.Substring(0, j);
                        return textData;
                    }
                    j++;
                }
            }
            return textData;
        }

        #endregion

        #region 指定截取文本长度的[默认...]
        /// <summary>
        /// 指定截取文本长度的
        /// </summary>
        /// <param name="textData">文本字符串</param>
        /// <param name="Length">指定截断长度</param>
        /// <param name="Flag">是否保留HTML代码</param>
        /// <returns></returns>
        public static string CutText(this string textData, int Length, bool Flag)
        {
            return CutText(textData, Length, Flag, "...");
        }
        #endregion

        #region 指定截取文本长度的
        /// <summary>
        /// 指定截取文本长度的
        /// </summary>
        /// <param name="textData">文本字符串</param>
        /// <param name="Length">指定截断长度</param>
        /// <returns></returns>
        public static string CutText(this string textData, int Length)
        {
            return CutText(textData, Length, false);
        }
        #endregion

        #region 指定截取文本长度的
        /// <summary>
        /// 指定截取文本长度的
        /// </summary>
        /// <param name="textData">文本字符串</param>
        /// <param name="Length">指定截断长度</param>
        /// <param name="cutTextTailTyeValue">选择未尾</param>
        /// <returns></returns>
        public static string CutText(this string textData, int Length, CutTextTailTye cutTextTailTyeValue)
        {
            if (cutTextTailTyeValue == CutTextTailTye.AddTail)
            {
                return CutText(textData, Length, false);
            }
            else if (cutTextTailTyeValue == CutTextTailTye.RemoveTail)
            {
                return CutText(textData, Length, false, "");
            }
            return "";
        }
        #endregion



        #region SQL语句过滤
        /// <summary>
        ///  原始SQL语句
        /// </summary>
        /// <param name="sqlString"> 原始SQL语句</param>
        /// <returns>过滤后的SQL语句</returns>
        public static string SqlFilter(this  string sqlString)
        {
            if (sqlString == null)
            {
                return "";
            }
            sqlString = sqlString.Replace("'", "''");
            sqlString = sqlString.Replace(";", "");
            return sqlString;
        }
        #endregion



        #region 获取汉字全部拼单
        /// <summary>
        /// 获取汉字全部拼单
        /// </summary>
        /// <param name="x">汉字串</param>
        /// <param name="isAddSpace">是否添加每个汉字空格</param>
        /// <returns></returns>

        public static string GetChineseSpell(this string x, bool isAddSpace)
        {
            int[] iA = new int[]
                          {
                           -20319 ,-20317 ,-20304 ,-20295 ,-20292 ,-20283 ,-20265 ,-20257 ,-20242 ,-20230
                           ,-20051 ,-20036 ,-20032 ,-20026 ,-20002 ,-19990 ,-19986 ,-19982 ,-19976 ,-19805
                           ,-19784 ,-19775 ,-19774 ,-19763 ,-19756 ,-19751 ,-19746 ,-19741 ,-19739 ,-19728
                           ,-19725 ,-19715 ,-19540 ,-19531 ,-19525 ,-19515 ,-19500 ,-19484 ,-19479 ,-19467
                           ,-19289 ,-19288 ,-19281 ,-19275 ,-19270 ,-19263 ,-19261 ,-19249 ,-19243 ,-19242
                           ,-19238 ,-19235 ,-19227 ,-19224 ,-19218 ,-19212 ,-19038 ,-19023 ,-19018 ,-19006
                           ,-19003 ,-18996 ,-18977 ,-18961 ,-18952 ,-18783 ,-18774 ,-18773 ,-18763 ,-18756
                           ,-18741 ,-18735 ,-18731 ,-18722 ,-18710 ,-18697 ,-18696 ,-18526 ,-18518 ,-18501
                           ,-18490 ,-18478 ,-18463 ,-18448 ,-18447 ,-18446 ,-18239 ,-18237 ,-18231 ,-18220
                           ,-18211 ,-18201 ,-18184 ,-18183 ,-18181 ,-18012 ,-17997 ,-17988 ,-17970 ,-17964
                           ,-17961 ,-17950 ,-17947 ,-17931 ,-17928 ,-17922 ,-17759 ,-17752 ,-17733 ,-17730
                           ,-17721 ,-17703 ,-17701 ,-17697 ,-17692 ,-17683 ,-17676 ,-17496 ,-17487 ,-17482
                           ,-17468 ,-17454 ,-17433 ,-17427 ,-17417 ,-17202 ,-17185 ,-16983 ,-16970 ,-16942
                           ,-16915 ,-16733 ,-16708 ,-16706 ,-16689 ,-16664 ,-16657 ,-16647 ,-16474 ,-16470
                           ,-16465 ,-16459 ,-16452 ,-16448 ,-16433 ,-16429 ,-16427 ,-16423 ,-16419 ,-16412
                           ,-16407 ,-16403 ,-16401 ,-16393 ,-16220 ,-16216 ,-16212 ,-16205 ,-16202 ,-16187
                           ,-16180 ,-16171 ,-16169 ,-16158 ,-16155 ,-15959 ,-15958 ,-15944 ,-15933 ,-15920
                           ,-15915 ,-15903 ,-15889 ,-15878 ,-15707 ,-15701 ,-15681 ,-15667 ,-15661 ,-15659
                           ,-15652 ,-15640 ,-15631 ,-15625 ,-15454 ,-15448 ,-15436 ,-15435 ,-15419 ,-15416
                           ,-15408 ,-15394 ,-15385 ,-15377 ,-15375 ,-15369 ,-15363 ,-15362 ,-15183 ,-15180
                           ,-15165 ,-15158 ,-15153 ,-15150 ,-15149 ,-15144 ,-15143 ,-15141 ,-15140 ,-15139
                           ,-15128 ,-15121 ,-15119 ,-15117 ,-15110 ,-15109 ,-14941 ,-14937 ,-14933 ,-14930
                           ,-14929 ,-14928 ,-14926 ,-14922 ,-14921 ,-14914 ,-14908 ,-14902 ,-14894 ,-14889
                           ,-14882 ,-14873 ,-14871 ,-14857 ,-14678 ,-14674 ,-14670 ,-14668 ,-14663 ,-14654
                           ,-14645 ,-14630 ,-14594 ,-14429 ,-14407 ,-14399 ,-14384 ,-14379 ,-14368 ,-14355
                           ,-14353 ,-14345 ,-14170 ,-14159 ,-14151 ,-14149 ,-14145 ,-14140 ,-14137 ,-14135
                           ,-14125 ,-14123 ,-14122 ,-14112 ,-14109 ,-14099 ,-14097 ,-14094 ,-14092 ,-14090
                           ,-14087 ,-14083 ,-13917 ,-13914 ,-13910 ,-13907 ,-13906 ,-13905 ,-13896 ,-13894
                           ,-13878 ,-13870 ,-13859 ,-13847 ,-13831 ,-13658 ,-13611 ,-13601 ,-13406 ,-13404
                           ,-13400 ,-13398 ,-13395 ,-13391 ,-13387 ,-13383 ,-13367 ,-13359 ,-13356 ,-13343
                           ,-13340 ,-13329 ,-13326 ,-13318 ,-13147 ,-13138 ,-13120 ,-13107 ,-13096 ,-13095
                           ,-13091 ,-13076 ,-13068 ,-13063 ,-13060 ,-12888 ,-12875 ,-12871 ,-12860 ,-12858
                           ,-12852 ,-12849 ,-12838 ,-12831 ,-12829 ,-12812 ,-12802 ,-12607 ,-12597 ,-12594
                           ,-12585 ,-12556 ,-12359 ,-12346 ,-12320 ,-12300 ,-12120 ,-12099 ,-12089 ,-12074
                           ,-12067 ,-12058 ,-12039 ,-11867 ,-11861 ,-11847 ,-11831 ,-11798 ,-11781 ,-11604
                           ,-11589 ,-11536 ,-11358 ,-11340 ,-11339 ,-11324 ,-11303 ,-11097 ,-11077 ,-11067
                           ,-11055 ,-11052 ,-11045 ,-11041 ,-11038 ,-11024 ,-11020 ,-11019 ,-11018 ,-11014
                           ,-10838 ,-10832 ,-10815 ,-10800 ,-10790 ,-10780 ,-10764 ,-10587 ,-10544 ,-10533
                           ,-10519 ,-10331 ,-10329 ,-10328 ,-10322 ,-10315 ,-10309 ,-10307 ,-10296 ,-10281
                           ,-10274 ,-10270 ,-10262 ,-10260 ,-10256 ,-10254
                          };
            string[] sA = new string[]
          {
           "a","ai","an","ang","ao"

           ,"ba","bai","ban","bang","bao","bei","ben","beng","bi","bian","biao","bie","bin"
           ,"bing","bo","bu"

           ,"ca","cai","can","cang","cao","ce","ceng","cha","chai","chan","chang","chao","che"
           ,"chen","cheng","chi","chong","chou","chu","chuai","chuan","chuang","chui","chun"
           ,"chuo","ci","cong","cou","cu","cuan","cui","cun","cuo"

           ,"da","dai","dan","dang","dao","de","deng","di","dian","diao","die","ding","diu"
           ,"dong","dou","du","duan","dui","dun","duo"

           ,"e","en","er"

           ,"fa","fan","fang","fei","fen","feng","fo","fou","fu"

           ,"ga","gai","gan","gang","gao","ge","gei","gen","geng","gong","gou","gu","gua","guai"
           ,"guan","guang","gui","gun","guo"

           ,"ha","hai","han","hang","hao","he","hei","hen","heng","hong","hou","hu","hua","huai"
           ,"huan","huang","hui","hun","huo"

           ,"ji","jia","jian","jiang","jiao","jie","jin","jing","jiong","jiu","ju","juan","jue"
           ,"jun"

           ,"ka","kai","kan","kang","kao","ke","ken","keng","kong","kou","ku","kua","kuai","kuan"
           ,"kuang","kui","kun","kuo"

           ,"la","lai","lan","lang","lao","le","lei","leng","li","lia","lian","liang","liao","lie"
           ,"lin","ling","liu","long","lou","lu","lv","luan","lue","lun","luo"

           ,"ma","mai","man","mang","mao","me","mei","men","meng","mi","mian","miao","mie","min"
           ,"ming","miu","mo","mou","mu"

           ,"na","nai","nan","nang","nao","ne","nei","nen","neng","ni","nian","niang","niao","nie"
           ,"nin","ning","niu","nong","nu","nv","nuan","nue","nuo"

           ,"o","ou"

           ,"pa","pai","pan","pang","pao","pei","pen","peng","pi","pian","piao","pie","pin","ping"
           ,"po","pu"

           ,"qi","qia","qian","qiang","qiao","qie","qin","qing","qiong","qiu","qu","quan","que"
           ,"qun"

           ,"ran","rang","rao","re","ren","reng","ri","rong","rou","ru","ruan","rui","run","ruo"

           ,"sa","sai","san","sang","sao","se","sen","seng","sha","shai","shan","shang","shao","she"
           ,"shen","sheng","shi","shou","shu","shua","shuai","shuan","shuang","shui","shun","shuo","si"
           ,"song","sou","su","suan","sui","sun","suo"

           ,"ta","tai","tan","tang","tao","te","teng","ti","tian","tiao","tie","ting","tong","tou","tu"
           ,"tuan","tui","tun","tuo"

           ,"wa","wai","wan","wang","wei","wen","weng","wo","wu"

           ,"xi","xia","xian","xiang","xiao","xie","xin","xing","xiong","xiu","xu","xuan","xue","xun"

           ,"ya","yan","yang","yao","ye","yi","yin","ying","yo","yong","you","yu","yuan","yue","yun"

           ,"za","zai","zan","zang","zao","ze","zei","zen","zeng","zha","zhai","zhan","zhang","zhao"
           ,"zhe","zhen","zheng","zhi","zhong","zhou","zhu","zhua","zhuai","zhuan","zhuang","zhui"
           ,"zhun","zhuo","zi","zong","zou","zu","zuan","zui","zun","zuo"
          };
            byte[] B = new byte[2];
            string s = "";
            char[] c = x.ToCharArray();
            for (int j = 0; j < c.Length; j++)
            {
                B = System.Text.Encoding.Default.GetBytes(c[j].ToString());
                if ((int)(B[0]) <= 160 && (int)(B[0]) >= 0)
                {
                    s += c[j];
                }
                else
                {
                    for (int i = (iA.Length - 1); i >= 0; i--)
                    {
                        if (iA[i] <= (int)(B[0]) * 256 + (int)(B[1]) - 65536)
                        {
                            s += sA[i] + (isAddSpace ? " " : "");
                            break;
                        }
                    }
                }
            }

            return s;
        }
        #endregion


        #region 获取汉字第一个拼音
        /// <summary>
        /// 获取汉字第一个拼音
        /// </summary>
        /// <param name="cn">单个汉字</param>
        /// <returns></returns>
        public static string GetChineseFirstSpell(this string cn)
        {

            byte[] arrCN = Encoding.Default.GetBytes(cn);
            if (arrCN.Length > 1)
            {
                int area = (short)arrCN[0];
                int pos = (short)arrCN[1];
                int code = (area << 8) + pos;
                int[] areacode = { 45217, 45253, 45761, 46318, 46826, 47010, 47297, 47614, 48119, 48119, 49062, 49324, 49896, 50371, 50614, 50622, 50906, 51387, 51446, 52218, 52698, 52698, 52698, 52980, 53689, 54481 };
                for (int i = 0; i < 26; i++)
                {
                    int max = 55290;
                    if (i != 25) max = areacode[i + 1];
                    if (areacode[i] <= code && code < max)
                    {
                        return Encoding.Default.GetString(new byte[] { (byte)(65 + i) });
                    }
                }
                return "?";
            }
            else return cn;

        }
        #endregion

        #region 获取汉字串各字第一个拼音
        /// <summary>
        /// 获取汉字串各字第一个拼音
        /// </summary>
        /// <param name="input">汉字串</param>
        /// <returns></returns>
        public static string GetChineseSpells(this string input)
        {

            int len = input.Length;
            string reVal = "";
            for (int i = 0; i < len; i++)
            {
                reVal += GetChineseFirstSpell(input.Substring(i, 1));
            }
            return reVal;

        }
        #endregion




        #region 移除指定的HTML文本中的所有HTML标签与一些HTML特殊符号
        /// <summary>
        /// 移除指定的HTML文本中的所有HTML标签与一些HTML特殊符号。
        /// </summary>
        /// <param name="html">Html</param>
        /// <param name="removePunctuation">是否移除标点符号。</param>
        /// <returns></returns>
        public static string CleanHtmlTags(this string html, bool removePunctuation)
        {
            html = StripEntities(html, true);

            html = StripTags(html, true);

            if (removePunctuation)
            {
                html = StripPunctuation(html, true);
            }
            return html;
        }
        #endregion



        #region 替换掉HTML文本中的<BR/>, <br/>字符串。
        /// <summary>
        /// 替换掉HTML文本中的<BR/>, <br/>字符串。
        /// </summary>
        /// <param name="html"></param>
        /// <param name="retainSpace">是否需要用空格分隔。</param>
        /// <returns></returns>
        public static string FormatText(this string html, bool retainSpace)
        {
            string brMatch = "\\s*<\\s*[bB][rR]\\s*/\\s*>\\s*";

            string replacement;
            if (retainSpace)
            {
                replacement = " \n"; //@SV"\n");
            }
            else
            {
                replacement = "\n"; //@SV"\n");
            }

            return Regex.Replace(html, brMatch, replacement);
        }
        #endregion


        #region 将www.test.com格式化为如下形式：<a href="www.test.net">www.test.net</a>
        /// <summary>
        /// 将www.test.com格式化为如下形式：<a href="www.test.net">www.test.net</a>
        /// </summary>
        /// <param name="website"></param>
        /// <returns></returns>
        public static string FormatWebsite(object website)
        {
            string returnValue = null;

            if (website != null)
            {
                returnValue = website.ToString().Trim();
                if (returnValue.Length > 0)
                {
                    if (returnValue.IndexOf(".") > -1)
                    {
                        returnValue = "<a href=\"" + (returnValue.IndexOf("://") == -1 ? "" : "http://") + returnValue + "\">" + returnValue + "</a>";
                    }
                }
            }
            return returnValue;
        }
        #endregion


        #region 去掉HTML中的一些特殊符号，如：&nbsp;&gt;等等。
        /// <summary>
        /// 去掉HTML中的一些特殊符号， 等等。
        /// </summary>
        /// <param name="html">html</param>
        /// <param name="retainSpace">是否去除空白</param>
        /// <returns></returns>
        public static string StripEntities(this string html, bool retainSpace)
        {

            //Set up Replacement String
            string replacement;
            if (retainSpace)
            {
                replacement = " ";
            }
            else
            {
                replacement = "";
            }

            return Regex.Replace(html, "&[^;]*;", replacement);
        }
        #endregion

        #region 去掉指定的HTML文本中的所有HTML标签
        /// <summary>
        /// 去掉指定的HTML文本中的所有HTML标签。
        /// </summary>
        /// <param name="html">html</param>
        /// <param name="retainSpace">是否去除空白</param>
        /// <returns></returns>
        public static string StripTags(this string html, bool retainSpace)
        {
            string replacement;
            if (retainSpace)
            {
                replacement = " ";
            }
            else
            {
                replacement = "";
            }
            return Regex.Replace(html, "<[^>]*>", replacement);
        }
        #endregion

        #region 移除HTML文本中的标点符号
        /// <summary>
        /// 移除HTML文本中的标点符号。
        /// </summary>
        /// <param name="html">html</param>
        /// <param name="retainSpace">是否去空白</param>
        /// <returns></returns>
        public static string StripPunctuation(this string html, bool retainSpace)
        {
            string punctuationMatch = "[~!#\\$%\\^&*\\(\\)-+=\\{\\[\\}\\]\\|;:\\x22\'<,>\\.\\?\\\\\\t\\r\\v\\f\\n]";
            Regex afterRegEx = new Regex(punctuationMatch + "\\s");
            Regex beforeRegEx = new Regex("\\s" + punctuationMatch);

            html += " ";

            string replacement;
            if (retainSpace)
            {
                replacement = " ";
            }
            else
            {
                replacement = "";
            }

            while (beforeRegEx.IsMatch(html))
            {
                html = beforeRegEx.Replace(html, replacement);
            }

            while (afterRegEx.IsMatch(html))
            {
                html = afterRegEx.Replace(html, replacement);
            }
            return html;
        }
        #endregion




        /// <summary>
        /// 移除所有空白字符串
        /// </summary>
        /// <param name="html">输入值</param>
        /// <param name="retainSpace">是否添加空值</param>
        /// <returns></returns>
        public static string StripWhiteSpace(this string html, bool retainSpace)
        {


            string replacement;
            if (retainSpace)
            {
                replacement = " ";
            }
            else
            {
                replacement = "";
            }


            return Regex.Replace(html, "\\s+", replacement);
        }

        /// <summary>
        /// 移除非A-Za-z0-9字符
        /// </summary>
        /// <param name="html">输入值</param>
        /// <param name="retainSpace">是否添加空值</param>
        /// <returns></returns>
        public static string StripNoWord(this string html, bool retainSpace)
        {


            string replacement;
            if (retainSpace)
            {
                replacement = " ";
            }
            else
            {
                replacement = "";
            }

            if (html == null)
            {
                return html;
            }
            else
            {
                return Regex.Replace(html, "\\W*", replacement);
            }
        }



        /// <summary>
        /// 去除字符
        /// </summary>
        /// <param name="value"></param>
        /// <param name="objChar"></param>
        /// <returns></returns>
        public static string Trim(this string value, char objChar)
        {
            return value.Trim(new char[] { objChar });
        }
        /// <summary>
        /// 去除逗号
        /// </summary>
        /// <param name="value">输入值</param>
        /// <returns></returns>
        public static string TrimComma(this string value)
        {
            return value.Trim(',');
        }

        /// <summary>
        /// 去除尾部字符
        /// </summary>
        /// <param name="value">输入值</param>
        /// <param name="objChar">字符</param>
        /// <returns></returns>
        public static string TrimEnd(this string value, char objChar)
        {
            return value.TrimEnd(new char[] { objChar });
        }
        /// <summary>
        ///  去除尾部逗号[,]
        /// </summary>
        /// <param name="value">输入值</param>
        /// <returns></returns>
        public static string TrimEndComma(this string value)
        {
            return value.TrimEnd(',');
        }
        /// <summary>
        /// 去除开始字符
        /// </summary>
        /// <param name="value">输入值</param>
        /// <param name="objChar">字符</param>
        /// <returns></returns>
        public static string TrimStart(this string value, char objChar)
        {
            return value.TrimStart(new char[] { objChar });
        }

        /// <summary>
        /// 去除开始逗号[,]
        /// </summary>
        /// <param name="value">输入值</param>
        /// <returns></returns>
        public static string TrimStartComma(this string value)
        {
            return value.TrimStart(',');
        }

        #region 标识字符串转化
        /// <summary>
        /// 标识字符串转化
        /// </summary>
        /// <param name="inputString">输入字符串如[123,456,789]</param>
        /// <param name="splitString">间隔字符[']</param>
        /// <returns>返回'123','456','789'</returns>
        public static string FormantIDstring(this string inputString, char splitString)
        {
            string[] arrString = inputString.Split(',');
            string outString = "";
            foreach (string rowString in arrString)
            {
                outString += splitString + rowString + splitString + ",";

            }
            outString = outString.TrimEnd(',');
            return outString;
        }
        #endregion

        #region 标识字符串转化[默认[']]
        /// <summary>
        /// 标识字符串转化
        /// </summary>
        /// <param name="inputString">输入字符串如[123,456,789]</param>
        /// <returns>返回Guid'123',Guid'456',Guid'789'</returns>
        public static string FormantEntityGuidID(this string inputString)
        {

            string[] arrString = inputString.Split(',');
            string outString = "";
            foreach (string rowString in arrString)
            {
                outString += "Guid'" + rowString + "',";

            }
            outString = outString.TrimEnd(',');
            return outString;

        }
        /// <summary>
        /// 标识字符串转化[默认[']]
        /// </summary>
        /// <param name="inputString">输入字符串如[123,456,789]</param>
        /// <returns>返回'123','456','789'</returns>
        public static string FormantStringID(this string inputString)
        {
            return inputString.FormantIDstring('\'');

        }
        #endregion


        /// <summary>

        /// 校验手机号码是否符合标准。

        /// </summary>

        /// <param name="mobile"></param>

        /// <returns></returns>

        public static bool ValidateMobile(string mobile)
        {

            if (string.IsNullOrEmpty(mobile))

                return false;



            return Regex.IsMatch(mobile, @"^(13|14|15|16|18|17|19)\d{9}$");

        }

        #region 处理URL
        /// <summary>
        /// url里有key的值，就替换为value,没有的话就追加.
        /// </summary>
        /// <param name="p_strUrl">当前Url</param>
        /// <param name="p_strParamText">参数名称</param>
        /// <param name="p_strParamValue">参数值</param>
        /// <returns></returns>
        public static string UrlEx(string p_strUrl, string p_strParamText, string p_strParamValue)
        {
            Regex reg = new Regex(string.Format("{0}=[^&]*", p_strParamText), RegexOptions.IgnoreCase);
            Regex reg1 = new Regex("[&]{2,}", RegexOptions.IgnoreCase);
            string strUrl = reg.Replace(p_strUrl, "");
            if (strUrl.IndexOf("?") == -1)
                strUrl += string.Format("?{0}={1}", p_strParamText, p_strParamValue);//?
            else
                strUrl += string.Format("&{0}={1}", p_strParamText, p_strParamValue);//&
            strUrl = reg1.Replace(strUrl, "&");
            strUrl = strUrl.Replace("?&", "?");

            return strUrl;
        }
        #endregion

    }


    #region 未尾类型
    /// <summary>
    /// 未尾类型
    /// </summary>
    public enum CutTextTailTye
    {
        /// <summary>
        /// 添加未尾[...]
        /// </summary>
        AddTail,
        /// <summary>
        /// 移除未尾[...]
        /// </summary>
        RemoveTail
    }
    #endregion

    /// <summary>
    /// 
    /// </summary>
    [FlagsAttribute()]
    public enum FilterFlag
    {
        /// <summary>
        /// 多行
        /// </summary>
        MultiLine = 1,
        /// <summary>
        ///HTML标签
        /// </summary>
        NoMarkup = 2,
        /// <summary>
        /// 脚本
        /// </summary>
        NoScripting = 4,
        /// <summary>
        /// SQL语句
        /// </summary>
        NoSQL = 8,
    }

 
}



