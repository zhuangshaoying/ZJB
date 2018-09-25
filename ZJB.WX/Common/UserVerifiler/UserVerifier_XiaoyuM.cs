using System;
using System.Text;
using Newtonsoft.Json;
using ZJB.Core.Utilities;

namespace ZJB.WX.Common.UserVerifiler
{
    public class UserVerifier_XiaoyuM
    {
        public const string baseApi = "http://api.xiaoyu.com/mobile?key={0}";

        public static bool IsValidate(string username, string password)
        {
            string json = HttpUtility.GetString(GetLoginUrl(username, password, username, ""));
            return json.Contains("uid");
        }

        private static string GetLoginUrl(string username, string password, string mobileId, string channelId)
        {
            var obj = new
            {
                callback = "account.checkauth",
                @params = new
                {
                    username,
                    password,
                    mobileId,
                    channelId
                },
                uid = 0,
                timestamp = DateTimeUtility.ToUnixTime(DateTime.Now),
                token = ""
            };

            return string.Format(baseApi, GetKeyString(obj));
        }

        public static string GetKeyString(object obj)
        {
            string json = JsonConvert.SerializeObject(obj);

            byte[] data = Encoding.UTF8.GetBytes(json);
            return new XmfishEncoder().Encode(data).Replace("<", "%3C").Replace(">", "%3E");
        }

        public class XmfishEncoder
        {
            private static readonly char[] encodeTable =
            {
                '<', 'A', 'a', '0', 'B', 'b', '1', 'C', 'c', '2', 'D', 'd', '3',
                'E', 'e', '4', 'F', 'f', '5', 'G', 'g', '6', 'H', 'h', '7', 'I', 'i', '8', 'J', 'j', '9', 'K', 'k', 'L',
                'l',
                'M', 'm', 'N', 'n', 'O', 'o', 'P', 'p', 'Q', 'q', 'R', 'r', 'S', 's', 'T', 't', 'U', 'u', 'V', 'v', 'W',
                'w',
                'X', 'x', 'Y', 'y', 'Z', 'z', '>'
            };

            private static char last2byte = (char) 3;
            private static char last4byte = (char) 15;
            private static char last6byte = (char) 63;
            private static char lead2byte = (char) 192;
            private static char lead4byte = (char) 240;
            private static char lead6byte = (char) 252;


            public String Encode(byte[] array)
            {
                var sb = new StringBuilder(3 + (int) (1.34*array.Length));
                int i = 0;
                char c = '\0';
                for (int j = 0; j < array.Length; ++j)
                {
                    for (i %= 8; i < 8; i += 6)
                    {
                        switch (i)
                        {
                            case 0:
                            {
                                c = (char) Move((char) (array[j] & lead6byte), 2);
                                break;
                            }
                            case 2:
                            {
                                c = (char) (array[j] & last6byte);
                                break;
                            }
                            case 4:
                            {
                                c = (char) ((char) (array[j] & last4byte) << 2);
                                if (j + 1 < array.Length)
                                {
                                    c |= (char) Move((array[j + 1] & lead2byte), 6);
                                }
                                break;
                            }
                            case 6:
                            {
                                c = (char) ((char) (array[j] & last2byte) << 4);
                                if (j + 1 < array.Length)
                                {
                                    c |= (char) Move((array[j + 1] & lead4byte), 4);
                                }
                                break;
                            }
                        }
                        sb.Append(encodeTable[c]);
                    }
                }
                var sb2 = new StringBuilder(3 + (int) (1.34*array.Length));
                int length = sb.Length;
                char char1 = sb[(int) (1000.0*new Random().NextDouble())%length];
                var c2 = (char) ('\u0004' + char1%length%'\u0005');
                sb.Append(char1);
                for (char c3 = '\0'; c3 <= length; c3 += c2)
                {
                    var c4 = (char) (c3 + c2);
                    char c5 = '\0';
                    if (c4 > length)
                    {
                        while (c5 < c2 && c3 + c5 <= length && sb[c3 + c5] > '\0')
                        {
                            sb2.Append(sb[c3 + c5]);
                            ++c5;
                        }
                    }
                    else
                    {
                        while (c5 < c2 && -1 + (c3 + c2 - c5) <= length)
                        {
                            if (sb[-1 + (c3 + c2 - c5)] <= '\0')
                            {
                                break;
                            }
                            sb2.Append(sb[-1 + (c3 + c2 - c5)]);
                            ++c5;
                        }
                    }
                }
                return sb2.ToString();
            }

            private static int Move(int x, int y)
            {
                int mask = 0x7fffffff;
                for (int i = 0; i < y; i++)
                {
                    x >>= 1;
                    x &= mask;
                }
                return x;
            }
        }
    }
}