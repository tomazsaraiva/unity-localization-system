#region Includes
using System.Text;

using TS.LocalizationSystem.Extensions;
using TS.LocalizationSystem.Utils;

using UnityEngine;
#endregion

namespace TS.LocalizationSystem
{
    /// <summary>
    /// All the locales supported by the application.
    /// https://www.science.co.il/language/Locale-codes.php
    /// </summary>
    public enum Locale
    {
        NotDefined,
        en_us,
        pt_pt
    }

    public class LocaleConfig
    {
        /// <summary>
        /// https://en.wikipedia.org/wiki/Endianness#Endian_dates
        /// </summary>
        public enum EndiannessType
        {
            Big,
            Little,
            Middle
        }

        #region Variables

        private const string TO_STRING_FORMAT = "{0} : {1}";

        public int ServerId { get; set; } = -1; // assigned at runtime
        public string Name { get; set; }
        public string FriendlyName { get; set; }
        public string DateFormat { get; set; }
        public string DateSeparator { get; set; }
        public EndiannessType DateEndianness { get; set; }
        public Locale ReferenceType { get; set; }
        public SystemLanguage[] SystemLanguages { get; set; }
        public LanguageConfig Language { get; set; }
        public string ThemeName { get; set; }

        public bool IsEnabled
        {
            get { return ServerId != -1; }
        }

        #endregion

        public override string ToString()
        {
            StringBuilder builder = new StringBuilder();
            builder.AppendFormat(TO_STRING_FORMAT, "Name", Name);
            builder.AppendFormat(TO_STRING_FORMAT, "Date Format", DateFormat);
            builder.AppendFormat(TO_STRING_FORMAT, "Locale", ReferenceType);
            builder.AppendFormat(TO_STRING_FORMAT, "System Languages", SystemLanguages.Print());
            builder.AppendFormat(TO_STRING_FORMAT, "Language", Language.ToString());
            builder.AppendFormat(TO_STRING_FORMAT, "Theme", ThemeName.ToString());
            return builder.ToString();
        }

        public static Locale GetLocale(string ietfTag)
        {
            // Replace char to match with enum
            ietfTag = ietfTag.Replace('-', '_');

            // Return locale or NotDefined.
            return EnumUtils.FromString(ietfTag, Locale.NotDefined);
        }
        public static string GetIeftTag(Locale locale)
        {
            return locale.ToString().Replace('_', '-');
        }
    }
}