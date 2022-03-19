#region Includes
using System.Text.RegularExpressions;

using TS.LocalizationSystem.Scriptables;

using UnityEngine;
#endregion

namespace TS.LocalizationSystem
{
    public class LocaleManager
    {
        #region Variables

        private const string LOCALE_RESOURCES_PATH_FORMAT = "Locales/{0}";

        public delegate void OnLocaleChanged(LocaleConfig current, int updateNumber);
        public static OnLocaleChanged LocaleChanged;

        public static LocaleConfig Current
        {
            get
            {
                if (_current == null) { SetDefaultLocale(); }
                return _current;
            }
        }

        public static int UpdateNumber { get; private set; } = 0;

        private static LocaleConfig _current;
        private static TextCollection _currentCollection;


        #endregion

        private LocaleManager() { /* To prevent initialization */ }

        public static void SetLocale(SystemLanguage systemLanguage)
        {
            UpdateLocale(LocaleSettings.GetLocale(systemLanguage));
        }
        public static void SetLocale(Locale locale)
        {
            UpdateLocale(LocaleSettings.Locales[locale]);
        }
        public static void SetLocale(Classes.Language language)
        {
            if (language == null)
            {
                SetDefaultLocale();
                return;
            }

            LocaleConfig localeConfig = LocaleSettings.GetLocale(language);
            if (localeConfig == null)
            {
                SetDefaultLocale();
                return;
            }

            UpdateLocale(localeConfig);
        }
        public static void SetDefaultLocale()
        {
            UpdateLocale(LocaleSettings.GetDefault());
        }

        public static void UpdateLocale(LocaleConfig localeConfig)
        {
            if (localeConfig == null) return;

            _current = localeConfig;
            UpdateNumber++;

            _currentCollection = GetCollection(localeConfig);

            LocaleChanged?.Invoke(_current, UpdateNumber);
        }
        private static TextCollection GetCollection(LocaleConfig localeConfig)
        {
            // Load the asset corresponding to full locale.
            string path = string.Format(LOCALE_RESOURCES_PATH_FORMAT, localeConfig.ReferenceType);
            TextCollection textCollection = (TextCollection)Resources.Load(path, typeof(TextCollection));

            // If not found, default to the asset corresponding to the language.
            if (textCollection == null)
            {
                path = string.Format(LOCALE_RESOURCES_PATH_FORMAT, localeConfig.Language.InternalCode);
                textCollection = (TextCollection)Resources.Load(path, typeof(TextCollection));
            }

            // If not found, display error.
            if (textCollection == null)
            {
#if UNITY_EDITOR || DEVELOPMENT_BUILD
                Debug.LogError("No collection found at path " + path);
#endif
            }

            return textCollection;
        }

        public static string Localize(string key)
        {
            // If no collection is defined, skip.
            if (_currentCollection == null) return null;

            // If no config defined, set the default.
            if (Current == null)
            {
                UpdateLocale(LocaleSettings.GetDefault());
            }

            if (_currentCollection == null)
            {
                return "";
            }

            // Localize the given key.
            string text = _currentCollection.Localize(key);

            if (string.IsNullOrEmpty(text))
            {
#if UNITY_EDITOR || DEVELOPMENT_BUILD
                Debug.LogError("No value for key " + key);
#endif
            }

            return Regex.Unescape(text);
        }
        public static string LocalizeUpper(string key)
        {
            string text = Localize(key);
            if (string.IsNullOrEmpty(text)) return null;
            return text.ToUpper();
        }
    }
}