
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Reflection;
namespace ZJB.Core.Utilities
{
    /// <summary>
    /// 对象处理类
    /// </summary>
    public static partial class ObjectUtility
    {
        /// <summary>
        /// 取值为byte.MinValue。
        /// </summary>
        public static byte NullByte
        {
            get { return byte.MinValue; }
        }

        /// <summary>
        /// 空的16位整型，取值为-1。
        /// </summary>
        public static short NullShort
        {
            get { return -1; }
        }

        /// <summary>
        /// 空的32位整型，取值为-1。
        /// </summary>
        public static int NullInt
        {
            get { return -1; }
        }

        /// <summary>
        /// 空的64位整型，取值为-1。
        /// </summary>
        public static long NullLong
        {
            get { return -1; }
        }

        /// <summary>
        /// 空的单精度浮点数，取值float.MinValue。
        /// </summary>
        public static float NullSingle
        {
            get { return float.MinValue; }
        }

        /// <summary>
        /// 空的双精度浮点数，取值double.MinValue。
        /// </summary>
        public static double NullDouble
        {
            get { return double.MinValue; }
        }

        /// <summary>
        /// 空的十进制数，取值decimal.MinValue。
        /// </summary>
        public static decimal NullDecimal
        {
            get { return decimal.MinValue; }
        }

        /// <summary>
        /// 空日期对象，取值1900-01-01 00:00:00。
        /// </summary>
        public static DateTime NullDate
        {
            get { return Convert.ToDateTime("1900-01-01 00:00:00"); }
        }

        /// <summary>
        /// 空字符串对象，取值string.Empty。
        /// </summary>
        public static string NullString
        {
            get { return string.Empty; }
        }

        /// <summary>
        /// 空的布尔值，取值false。
        /// </summary>
        public static bool NullBoolean
        {
            get { return false; }
        }

        /// <summary>
        /// 空的Guid值，取值Guid.Empty。
        /// </summary>
        public static Guid NullGuid
        {
            get { return Guid.Empty; }
        }


        /// <summary>
        /// 判断一个对象是否为空
        /// </summary>
        /// <param name="instance">对象</param>
        /// <returns></returns>
        public static bool IsNull(this object instance)
        {
            if (instance != null)
            {
                if (instance is int)
                {
                    return instance.Equals(NullInt);
                }
                else if (instance is Single)
                {
                    return instance.Equals(NullSingle);
                }
                else if (instance is double)
                {
                    return instance.Equals(NullDouble);
                }
                else if (instance is decimal)
                {
                    return instance.Equals(NullDecimal);
                }
                else if (instance is DateTime)
                {
                    return ((DateTime)instance).Date.Equals(NullDate.Date);
                }
                else if (instance is string)
                {
                    return instance.ToString().IsNull();
                }
                else if (instance is bool)
                {
                    return instance.Equals(NullBoolean);
                }
                else if (instance is Guid)
                {
                    return instance.Equals(NullGuid);
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return true;
            }
        }

        /// <summary>
        /// 判断对象是否不为空
        /// </summary>
        /// <param name="instance"></param>
        /// <returns></returns>
        public static bool IsNoNull(this object instance)
        {
            if (instance != null)
            {
                if (instance is int)
                {
                    return !instance.Equals(NullInt);
                }
                else if (instance is Single)
                {
                    return !instance.Equals(NullSingle);
                }
                else if (instance is double)
                {
                    return !instance.Equals(NullDouble);
                }
                else if (instance is decimal)
                {
                    return !instance.Equals(NullDecimal);
                }
                else if (instance is DateTime)
                {
                    return !((DateTime)instance).Date.Equals(NullDate.Date);
                }
                else if (instance is string)
                {
                    return instance.ToString().IsNoNull();
                }
                else if (instance is bool)
                {
                    return !instance.Equals(NullBoolean);
                }
                else if (instance is Guid)
                {
                    return !instance.Equals(NullGuid);
                }
                else
                {
                    return true;
                }
            }
            else
            {
                return false;
            }
        }


    }
}
