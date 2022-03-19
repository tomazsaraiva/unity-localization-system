#region Includes
using System.Text;

using TS.LocalizationSystem.Utils;
#endregion

namespace TS.LocalizationSystem
{
    /// <summary>
    /// All the languages available in the application.
    /// </summary>
    public enum Language
    {
        English = 0,
        Portuguese = 1
    }

    /// <summary>
    /// Codes for all languages avaiable in the application.
    /// </summary>
    public enum LanguageCode
    {
        en = 0,
        pt = 1
    }

    /// <summary>
    /// Enumeration for indicating the alignment of the UI depending on language
    /// </summary>
    public enum LanguageReadingDirection
    {
        LeftToRight = 0,
        RightToLeft = 1
    }

    public static class LanguageReadingDirectionHelper
    {
        public static string ToString(LanguageReadingDirection languageReadingDirection)
        {
            switch (languageReadingDirection)
            {
                case LanguageReadingDirection.LeftToRight: return "ltr";
                case LanguageReadingDirection.RightToLeft: return "rtl";
                default: return null;
            }
        }
    }

    public class LanguageConfig
    {
        #region Variables

        private const string TO_STRING_FORMAT = "{0} : {1}";

        public string Name { get; set; }
        public string ISO { get { return InternalCode.ToString(); } }
        public Language ReferenceType { get; set; }
        public LanguageCode InternalCode { get; set; }
        public LanguageReadingDirection ReadingDirection { get; set; }

        #endregion

        public override string ToString()
        {
            StringBuilder builder = new StringBuilder();
            builder.AppendFormat(TO_STRING_FORMAT, "Name", Name);
            builder.AppendFormat(TO_STRING_FORMAT, "ISO", ISO);
            builder.AppendFormat(TO_STRING_FORMAT, "Language", ReferenceType);
            builder.AppendFormat(TO_STRING_FORMAT, "Language Code", InternalCode);
            builder.AppendFormat(TO_STRING_FORMAT, "Language Reading Direction", ReadingDirection);
            return builder.ToString();
        }

        public static Language GetLanguage(string languageCode)
        {
            return EnumUtils.FromString(languageCode, Language.English);
        }
    }
}