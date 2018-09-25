using System;

namespace ZJB.Core.Utilities
{
    public static class DateTimeUtility
    {
        public static long ToUnixTime(DateTime dateTime)
        {
            return (long)dateTime.ToUniversalTime().Subtract(new DateTime(1970, 1, 1)).TotalSeconds;
        }

        public static DateTime FromUnixTime(long unixTime)
        {
            return new DateTime(1970, 1, 1).AddSeconds(unixTime).ToLocalTime();
        }
        public static long ToUnixTime_Milliseconds(DateTime dateTime)
        {
            return (long)dateTime.ToUniversalTime().Subtract(new DateTime(1970, 1, 1)).TotalMilliseconds;
        }

        public static DateTime FromUnixTime_Milliseconds(long unixTime)
        {
            return new DateTime(1970, 1, 1).AddMilliseconds(unixTime).ToLocalTime();
        }
        

        public static string GetDisplayTime(DateTime date)
        {
            if (date == DateTime.MinValue)
                return null;
            string displayTime;
            var val = DateTime.Now - date;
            if (val.TotalSeconds <= 10)
            {
                displayTime = "刚刚";
            }
            else if (val.TotalSeconds > 10 && val.TotalSeconds <= 60)
            {
                displayTime = string.Format("{0}秒前", val.Seconds);
            }
            else if (val.TotalMinutes >= 1 && val.TotalMinutes < 60)
            {
                displayTime = string.Format("{0}分钟前", val.Minutes);
            }
            else if (val.TotalHours >= 1 && val.TotalHours < 24)
            {
                displayTime = string.Format("{0}小时前", val.Hours);
            }
            else //if (val.TotalDays >= 1 && val.TotalHours < 4)
            {
                displayTime = string.Format("{0}天前", val.Days);
            }
            return displayTime;
        }
    }
}
