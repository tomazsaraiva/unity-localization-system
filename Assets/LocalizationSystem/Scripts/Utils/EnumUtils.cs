#region Includes
using System;
#endregion

namespace TS.LocalizationSystem.Utils
{
    public static class EnumUtils
    {
        public static string ToString<T>(T value)
        {
            return value.ToString();
        }

        public static string ToLowerString<T>(T value)
        {
            return ToString<T>(value).ToLower();
        }

        public static string ToUpperString<T>(T value)
        {
            return ToString<T>(value).ToUpper();
        }

        public static TEnum FromString<TEnum>(string text, TEnum defaultValue, bool caseInsensitive = true)
        {
            if (string.IsNullOrEmpty(text)) return defaultValue;
            if (!Enum.IsDefined(typeof(TEnum), text)) return defaultValue;
            return (TEnum)Enum.Parse(typeof(TEnum), text, caseInsensitive);
        }
    }
}