#region Includes
using System.Collections.Generic;
using System.Linq;

using UnityEngine;
#endregion

namespace TS.LocalizationSystem
{
    /// <summary>
    /// Defines the locale settings and available configuration options.
    /// </summary>
    public static class LocaleSettings
    {
        /// <summary>
        /// List of application supported languages.
        /// </summary>
        private static readonly Dictionary<Language, LanguageConfig> Languages = new Dictionary<Language, LanguageConfig>()
        {
            {
                Language.English, new LanguageConfig()
                {
                    InternalCode = LanguageCode.en,
                    ReferenceType = Language.English,
                    Name = "English",
                    ReadingDirection = LanguageReadingDirection.LeftToRight
                }
            },
            {
                Language.Portuguese, new LanguageConfig()
                {
                    InternalCode = LanguageCode.pt,
                    ReferenceType = Language.Portuguese,
                    Name = "Portuguese",
                    ReadingDirection = LanguageReadingDirection.LeftToRight
                }
            }
        };

        /// <summary>
        /// List of application supported locales.
        /// </summary>
        public static readonly Dictionary<Locale, LocaleConfig> Locales = new Dictionary<Locale, LocaleConfig>()
        {
            {
                Locale.en_US, new LocaleConfig()
                {
                    //Name = "English - United States",
                    Name = "English",
                    ReferenceType = Locale.en_US,
                    Language = Languages[Language.English],
                    SystemLanguages = new SystemLanguage[]
                    {
                        SystemLanguage.English
                    },
                    DateFormat = "dd-MM-yyyy",
                    DateSeparator = "/",
                    DateEndianness = LocaleConfig.EndiannessType.Little
                }
            },
            {
                Locale.pt_PT, new LocaleConfig()
                {
                    //Name = "Portuguese - Portugal",
                    Name = "Português",
                    ReferenceType = Locale.pt_PT,
                    Language = Languages[Language.Portuguese],
                    SystemLanguages = new SystemLanguage[]
                    {
                        SystemLanguage.Portuguese
                    },
                    DateFormat = "dd-MM-yyyy",
                    DateSeparator = "-",
                    DateEndianness = LocaleConfig.EndiannessType.Little
                }
            }
        };

        public static LocaleConfig GetLocale(SystemLanguage systemLanguage)
        {
            LocaleConfig localeConfig = Locales.FirstOrDefault(l => l.Value.SystemLanguages.Contains(systemLanguage)).Value;
            if (localeConfig == null)
            {
#if UNITY_EDITOR || DEVELOPMENT_BUILD
                Debug.LogWarning("No locale for " + systemLanguage);
#endif

                localeConfig = GetDefault();

#if UNITY_EDITOR || DEVELOPMENT_BUILD
                Debug.Log("Using default locale " + localeConfig);
#endif
            }
            return localeConfig;
        }
        public static LocaleConfig GetLocale(Classes.Language language)
        {
            Locale locale = LocaleConfig.GetLocale(language.IetfTag);
            if (locale != Locale.NotDefined) return Locales[locale];
            return null;
        }
        public static LocaleConfig GetDefault()
        {
            return Locales[Locale.en_US];
        }

        public static List<LocaleConfig> GetActiveLocales()
        {
            return Locales.Where(i => i.Value.IsEnabled).Select(i => i.Value).ToList();
        }

        public static List<Classes.Language> GetLanguages()
        {
            List<Classes.Language> languages = new List<Classes.Language>();
            int count = 1;
            foreach (var locale in Locales.Values)
            {
                Classes.Language language = new Classes.Language();
                language.id = count;
                language.name = locale.Name;
                language.direction = LanguageReadingDirectionHelper.ToString(locale.Language.ReadingDirection);
                language.locale = LocaleConfig.GetIeftTag(locale.ReferenceType);
                language.language_code = locale.Language.ISO;
                languages.Add(language);

                count++;
            }
            return languages;
        }
    }
}