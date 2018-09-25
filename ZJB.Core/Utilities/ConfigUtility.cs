using System.Configuration;

namespace ZJB.Core.Utilities
{
    public class ConfigUtility
    {
        public static string GetValue(string key)
        {
            return ConfigurationManager.AppSettings[key];
        }

        public static int GetIntValue(string key)
        {
            string valueString = GetValue(key);
            return int.Parse(valueString);
        }

        public static int? GetNullableIntValue(string key)
        {
            string valueString = GetValue(key);

            if (string.IsNullOrEmpty(valueString))
                return null;

            return int.Parse(GetValue(key));
        }

        public static bool GetBoolValue(string key)
        {
            string valueString = GetValue(key);
            return bool.Parse(valueString);
        }

        public static bool? GetNullableBoolValue(string key)
        {
            string valueString = GetValue(key);

            if (string.IsNullOrEmpty(valueString))
                return null;

            return bool.Parse(GetValue(key));
        }
    }
}
