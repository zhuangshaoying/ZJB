using System;
using System.Collections.Specialized;
using System.Globalization;
using System.IO;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Xml.Serialization;
using ZJB.Core.Utilities;
namespace ZJB.WX.Common.xms
{
    internal class Helper_Soufun
    {
        private const string agentApi = "http://agentappnew.3g.fang.com/http/agentservice.jsp?";
        private const string soufunApi = "http://soufunapp.3g.soufun.com/http/sfservice.jsp?";

        private const string queryTemplate =
            "messagename=lpcontent&city={0}&newcode={1}";

        private const string userLimitParam = "messagename=getUserLimitInfo&username={0}&city={1}";

        private static NameValueCollection GetHeaders()
        {
            return new NameValueCollection
            {
                {"X1", "0.0"},
                {"app-name", "Android_UnMap"},
                {"Y1", "0.0"},
                {"model", "Sony+Xperia+Z+-+4.2.2+-+API+17+-+1080x1920_1"},
                {"osVersion", "4.2.2"},
                {"imei", "000000000000000"},
                {"User-Agent", "Android_UnMap%7ESony+Xperia+Z+-+4.2.2+-+API+17+-+1080x1920_1%7E4.2.2"},
                {"city", "%E5%8E%A6%E9%97%A8"},
                {"version", "7.2.2"},
                {"posmode", "gps%2Cwifi"},
                {"phoneNumber", ""},
                {"company", "30001"},
//                {"Accept-Encoding", "gzip"},
                {"ispos", "0"},
                {"iscard", "0"},
                {"connmode", "Wifi"},
            };
        }

        public static NameValueCollection GetAgentHeaders(int userId)
        {
            return new NameValueCollection
            {
                {"app-name", "Android_Agent"},
                {"Agent", userId.ToString()},
                {"model", "Sony"},
                {"osVersion", "4.2.2"},
                {"imei", "000000000000000"},
                {"User-Agent", "Android_Agent~Sony"},
                {"version", "3.9.0"},
                {"posmode", "gps,wifi"},
                {"phoneNumber", ""},
                {"company", "-30000"},
//                {"Accept-Encoding", "gzip"},
                {"ispos", "0"},
                {"iscard", "0"},
                {"connmode", "Wifi"},
            };
        }

        public static NameValueCollection GetUserHeaders(int userId)
        {
            return new NameValueCollection
            {
                {"model", "Sony Xperia Z - 4.2.2 - API 17 - 1080x1920_1"},
                {"osVersion", "4.2.2"},
//                {"Accept-Encoding","gzip,deflate"},
                {"User-Agent", "Android_Agent~Sony Xperia Z - 4.2.2 - API 17 - 1080x1920_1~4.2.2"},
                {"app_name", "Android_Agent"},
//                {"Connection","Close"},
//                {"Accept","application/vnd.wap.xhtml+xml,application/xml,text/vnd.wap.wml,text/html,application/xhtml+xml,image/jpeg;q=0.5,image/png;q=0.5,image/gif;q=0.5,image/*;q=0.6,video/*,audio/*,*/*;q=0.6"},
                {"loginid", ""},
                {"userid", userId.ToString()},
                {"version", "3.9.0"},
                {"connmode", "Wifi"},
                {"imei", "000000000000000"},
                {"phoneNumber", ""},
                {"posmode", "gps,wifi"},
                {"ispos", "0"},
                {"iscard", "0"},
                {"company", "-30000"},
            };
        }

        public static WebHeaderCollection GetWebHeaders(int userId)
        {
            return new WebHeaderCollection
            {
                {"model", "Sony Xperia Z - 4.2.2 - API 17 - 1080x1920_1"},
                {"osVersion", "4.2.2"},
                {"app_name", "Android_Agent"},
                {"loginid", ""},
                {"userid", userId.ToString()},
                {"version", "3.9.0"},
                {"connmode", "Wifi"},
                {"imei", "000000000000000"},
                {"phoneNumber", ""},
                {"posmode", "gps,wifi"},
                {"ispos", "0"},
                {"iscard", "0"},
                {"company", "-30000"},
            };
        }

        public static T GetObjectFromSFUrl<T>(string xml)
        {
            if (string.IsNullOrEmpty(xml) || string.IsNullOrEmpty(xml.Trim()))
                return default(T);
            try
            {
                StreamReader mem2 = new StreamReader(
                   new MemoryStream(System.Text.Encoding.UTF8.GetBytes(xml)),
                   System.Text.Encoding.UTF8);

                XmlSerializer ser = new XmlSerializer(typeof(T));
               return (T)ser.Deserialize(mem2);
                //return XmlUtility.Deserialize<T>(xml);
            }
            catch (Exception e)
            {
                return default(T);
            }
        }

//           "city={0}&maptype=baidu&messagename=newhouseinfo&newcode={1}";
        public static T GetObjectFromSFUrl<T>(string id, string city)
        {
            string query = string.Format(queryTemplate,Uri.EscapeUriString(city), id);
            string url = soufunApi + query + "&wirelesscode=" +
                         StringUtility.ToMd5String(query + "soufunhttp").ToUpper();
            string xml;
            try
            {
                xml = HttpUtility.GetString(url, Encoding.UTF8, GetHeaders());
                //, new WebProxy(new Uri("http://localhost:8888")));
            }
            catch (Exception)
            {
                xml = null;
            }

            if (string.IsNullOrEmpty(xml) || string.IsNullOrEmpty(xml.Trim()))
                return default(T);
            try
            {
                return XmlUtility.Deserialize<T>(xml);
            }
            catch (Exception)
            {
                return default(T);
            }
        }

//        private static string EscapeUriString(string str)
//        {
//            return str == null ? "" : Uri.EscapeUriString(str);
//        }

        public static string GetString(string url, bool isWireless = false, int userId = 0)
        {
            WebHeaderCollection headers = GetWebHeaders(userId);
//            string ret = HttpUtility.GetString(url, headers, false,new WebProxy("localhost", 8888));
            string ret = HttpUtility.GetString(url, headers);
            return ret;
        }

        public static string GetWireless(string param)
        {
//            return StringUtility.ToMd5String(param + "soufunhttp");
            return StringUtility.ToMd5String(param + "cD@8^)t{");
        }


        //无线搜房帮:http://agentappnew.3g.fang.com/http/agentservice.jsp?messagename=getUserLimitInfo&username=mtccq&city=%E5%8E%A6%E9%97%A8&wirelesscode=ad22e8849887402417743ed219ee49cb
        //仅搜房帮：http://agentappnew.3g.fang.com/http/agentservice.jsp?messagename=getUserLimitInfo&username=yukihsx&city=%E5%8E%A6%E9%97%A8&wirelesscode=8c7e1104ade1f339330592f367328890

        public static string getUserLimit(string username, string city)
        {
            string param = string.Format(userLimitParam, username, Uri.EscapeUriString(city));
            string url = agentApi + param + "&wirelesscode=" + GetWireless(param).ToLower();
            return GetString(url);
        }

        /// <summary>
        ///     网页版RSA加密验证 new RSAKeyPair(exponent,d,modulus);
        /// </summary>
        /// <param name="exponent"></param>
        /// <param name="d"></param>
        /// <param name="modulus"></param>
        /// <param name="text"></param>
        /// <returns></returns>
        public static string getHex(string exponent, string d, string modulus, string text)
        {
            byte[] _e = HexStringToBytes(exponent);
            byte[] _d = HexStringToBytes(d);
            byte[] _m = HexStringToBytes(modulus);

            var RSA = new RSACryptoServiceProvider();
            RSA.ImportParameters(new RSAParameters {Modulus = _m, Exponent = _e, D = _d}); //导入公钥
            byte[] Transformation = StringToASCIIBytes(text);
            byte[] EncryptedSymmetricData = RSA.Encrypt(Transformation, false); //加密
            string hex = BytesToHexString(EncryptedSymmetricData).ToLower();
            return hex;
        }
        public static byte[] HexStringToBytes(string hex)
        {
            if (hex.Length == 0)
            {
                return new byte[] { 0 };
            }

            if (hex.Length % 2 == 1)
            {
                hex = "0" + hex;
            }

            var result = new byte[hex.Length / 2];

            for (int i = 0; i < hex.Length / 2; i++)
            {
                result[i] = byte.Parse(hex.Substring(2 * i, 2), NumberStyles.AllowHexSpecifier);
            }

            return result;
        }
        public static string BytesToHexString(byte[] input)
        {
            var hexString = new StringBuilder(64);

            for (int i = 0; i < input.Length; i++)
            {
                hexString.Append(String.Format("{0:X2}", input[i]));
            }
            return hexString.ToString();
        }
        public static byte[] StringToASCIIBytes(string input)
        {
            var enc = new ASCIIEncoding();
            return enc.GetBytes(input);
        }

    }
}