using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json;
using ZJB.Core.Utilities;

namespace ZJB.Api.Common
{
    public class DateTimeConverter : DateTimeConverterBase
    {
        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            DateTime date = new DateTime();
            DateTime.TryParse((string)reader.Value, out date);
            return date;
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            // writer.WriteValue(((DateTime)value).ToString("yyyy-MM-dd HH:mm:ss"));
            writer.WriteValue(DateTimeUtility.ToUnixTime((DateTime)value));
        }


        /// <summary>        
        /// 将Unix时间戳转换为DateTime类型时间        
        /// </summary>        
        /// <param name="d">double 型数字</param>        
        /// <returns>DateTime</returns>        
        public static System.DateTime ConvertIntDateTime(double d)
        {
            System.DateTime time = System.DateTime.MinValue;
            System.DateTime startTime = TimeZone.CurrentTimeZone.ToLocalTime(new System.DateTime(1970, 1, 1));
            time = startTime.AddSeconds(d);
            return time;
        }

        /// <summary>        
        /// 将c# DateTime时间格式转换为Unix时间戳格式        
        /// </summary>        
        /// <param name="time">时间</param>        
        /// <returns>double</returns>        
        public static double ConvertDateTimeToDouble(System.DateTime time)
        {
            double intResult = 0;
            System.DateTime startTime = TimeZone.CurrentTimeZone.ToLocalTime(new System.DateTime(1970, 1, 1));
            intResult = (time - startTime).TotalSeconds;
            return intResult;
        }

    }
}
